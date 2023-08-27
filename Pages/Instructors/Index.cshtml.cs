using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Instructors;

public class IndexModel : PageModel
{
    private readonly SchoolContext _context;

    public IndexModel(SchoolContext context)
    {
        _context = context;
    }

    public IList<Instructor> Instructors { get; set; } = null!;
    public Instructor? SelectedInstructor { get; set; }
    public Course? SelectedCourse { get; set; }

    public async Task OnGetAsync(int? selectedID, int? selectedCourseID)
    {
        Instructors = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.Courses!)
                .ThenInclude(c => c.Department)
            .OrderBy(i => i.LastName)
            .ToListAsync();
        
        if (selectedID is not null)
        {
            SelectedInstructor = Instructors
                .Where(i => i.ID == selectedID)
                .SingleOrDefault();
        }

        if (selectedCourseID is not null)
        {
            SelectedCourse = SelectedInstructor?.Courses!
                .Where(c => c.CourseID == selectedCourseID)
                .SingleOrDefault();
            
            if (SelectedCourse is null)
            {
                return;
            }

            SelectedCourse.Enrollments = await _context.Enrollments
                .Where(e => e.CourseID == selectedCourseID)
                .Include(e => e.Student)
                .ToListAsync();
        }
    }
}
