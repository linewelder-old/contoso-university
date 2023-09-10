using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors;

public class CreateModel : PageModel
{
    private readonly SchoolContext _context;

    public CreateModel(SchoolContext context)
    {
        _context = context;
    }
    
    public List<Course> Courses { get; set; } = null!;

    public async Task<IActionResult> OnGet()
    {
        Courses = await _context.Courses.ToListAsync();
        return Page();
    }

    [BindProperty]
    public Instructor Instructor { get; set; } = default!;
    
    public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var newInstructor = new Instructor();
        if (!await TryUpdateModelAsync(
                newInstructor,
                "instructor",
                i => i.FirstMidName, i => i.LastName,
                i => i.HireDate, i => i.OfficeAssignment
            ))
        {
            return Page();
        }

        if (string.IsNullOrWhiteSpace(newInstructor.OfficeAssignment?.Location))
        {
            newInstructor.OfficeAssignment = null;
        }

        if (selectedCourses.Length != 0)
        {
            newInstructor.Courses = new List<Course>();
            await _context.Courses.LoadAsync();
            
            foreach (var courseId in selectedCourses)
            {
                var foundCourse = await _context.Courses.FindAsync(int.Parse(courseId));
                if (foundCourse is not null)
                {
                    newInstructor.Courses.Add(foundCourse);
                }
            }
        }
        
        _context.Instructors.Add(newInstructor);
        await _context.SaveChangesAsync();

        Courses = await _context.Courses.ToListAsync();
        return RedirectToPage("./Index");
    }
}
