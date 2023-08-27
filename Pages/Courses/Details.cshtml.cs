using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.ViewModels;

namespace ContosoUniversity.Pages.Courses;

public class DetailsModel : PageModel
{
    private readonly SchoolContext _context;

    public DetailsModel(SchoolContext context)
    {
        _context = context;
    }

    public CourseViewModel Course { get; set; } = default!; 

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var course = await _context.Courses
            .Select(c => new CourseViewModel {
                CourseID = c.CourseID,
                Title = c.Title,
                Credits = c.Credits,
                DepartmentName = c.Department!.Name,
            })
            .FirstOrDefaultAsync(m => m.CourseID == id);

        if (course == null)
        {
            return NotFound();
        }

        Course = course;
        return Page();
    }
}
