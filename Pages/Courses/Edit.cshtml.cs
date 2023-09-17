using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Courses;

public class EditModel : PageModel
{
    private readonly SchoolContext _context;

    public EditModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Course Course { get; set; } = default!;

    public DepartmentList DepartmentsSL { get; set; } = null!;

    public override PageResult Page()
    {
        DepartmentsSL = new DepartmentList(_context, Course.DepartmentID);
        return base.Page();
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Department)
            .FirstOrDefaultAsync(c => c.CourseID == id);
        if (course == null)
        {
            return NotFound();
        }

        Course = course;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync(
            course,
            "course",
            c => c.Title, c => c.DepartmentID, c => c.Credits))
        {
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        _context.Attach(Course).State = EntityState.Modified;
        return Page();
    }
}

