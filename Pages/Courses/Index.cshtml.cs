using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.ViewModels;

namespace ContosoUniversity.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly SchoolContext _context;

    public IndexModel(SchoolContext context)
    {
        _context = context;
    }

    public IList<CourseViewModel> Courses { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Courses = await _context.Courses
            .Include(c => c.Department)
            .Select(c => new CourseViewModel {
                CourseID = c.CourseID,
                Title = c.Title,
                Credits = c.Credits,
                DepartmentName = c.Department!.Name,
            })
            .ToListAsync();
    }
}
