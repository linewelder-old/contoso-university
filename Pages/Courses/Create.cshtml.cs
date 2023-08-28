using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Courses;

public class CreateModel : PageModel
{
    private readonly SchoolContext _context;

    public CreateModel(SchoolContext context)
    {
        _context = context;
    }

    public DepartmentList DepartmentsSL { get; set; } = null!;

    public IActionResult OnGet()
    {
        DepartmentsSL = new DepartmentList(_context, null);
        return Page();
    }

    [BindProperty]
    public Course Course { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        var course = new Course();

        if (await TryUpdateModelAsync(
            course,
            "course",
            s => s.CourseID, s => s.DepartmentID,
            s => s.Title, s => s.Credits))
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        DepartmentsSL = new DepartmentList(_context, course.DepartmentID);
        return Page();
    }
}
