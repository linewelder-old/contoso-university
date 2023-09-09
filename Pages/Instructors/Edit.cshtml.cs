using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Instructors;

public class EditModel : PageModel
{
    private readonly SchoolContext _context;

    public EditModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Instructor Instructor { get; set; } = null!;
    [BindProperty]
    public string? OfficeLocation { get; set; }
    
    public List<AssignedCourse> AssignedCourses { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var instructor = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.Courses)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.ID == id);
        if (instructor is null)
        {
            return NotFound();
        }

        Instructor = instructor;
        OfficeLocation = instructor.OfficeAssignment?.Location;
        await PopulateAssignedCourses(_context, instructor);
        return Page();
    }
    
    private async Task PopulateAssignedCourses(
        SchoolContext context, Instructor instructor)
    {
        if (instructor.Courses is null)
        {
            throw new ArgumentNullException(null, "Instructor must have assigned courses loaded.");
        }

        AssignedCourses.Clear();
        await foreach (var course in context.Courses)
        {
            AssignedCourses.Add(new AssignedCourse {
                CourseID = course.CourseID,
                Title = course.Title,
                Assigned = instructor.Courses.Any(
                    ic => ic.CourseID == course.CourseID)
            });
        }
    }

    public async Task<IActionResult> OnPostAsync(int id, string[] selectedCourses)
    {
        var instructorToUpdate = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.ID == id);
        if (instructorToUpdate is null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync(
            instructorToUpdate,
            "Instructor",
            i => i.FirstMidName, i => i.LastName,
            i => i.HireDate, i => i.OfficeAssignment))
        {
            if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location)) {
                instructorToUpdate.OfficeAssignment = null;
            }

            await UpdateInstructorCourses(selectedCourses, instructorToUpdate);

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        await _context.Entry(instructorToUpdate)
            .Collection(i => i.Courses!)
            .LoadAsync();
        await PopulateAssignedCourses(_context, instructorToUpdate);
        return Page();
    }

    private async Task UpdateInstructorCourses(string[] selectedCourses, Instructor instructor)
    {
        var selectedCourseIds = new HashSet<int>(selectedCourses.Select(int.Parse));
        var instructorCourses = new HashSet<int>(
            instructor.Courses!.Select(c => c.CourseID));
        await foreach (var course in _context.Courses)
        {
            var isSelected = selectedCourseIds.Contains(course.CourseID);
            var isInstructors = instructorCourses.Contains(course.CourseID);
            if (isInstructors && !isSelected)
            {
                instructor.Courses!.Remove(course);
            }
            else if (isSelected && !isInstructors)
            {
                instructor.Courses!.Add(course);
            }
        }
    }
}
