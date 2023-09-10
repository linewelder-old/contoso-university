using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors;

public class DeleteModel : PageModel
{
    private readonly SchoolContext _context;

    public DeleteModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Instructor Instructor { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);

        if (instructor == null)
        {
            return NotFound();
        }

        Instructor = instructor;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var instructor = await _context.Instructors
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.ID == id);
        if (instructor == null)
        {
            return RedirectToPage("./Index");
        }

        await _context.Departments
            .Where(d => d.InstructorID == id)
            .ForEachAsync(d => d.InstructorID = null);
        _context.Instructors.Remove(instructor);

        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
