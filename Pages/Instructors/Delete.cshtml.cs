using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

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
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor != null)
        {
            Instructor = instructor;
            _context.Instructors.Remove(Instructor);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
