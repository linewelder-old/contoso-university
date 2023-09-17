using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students;

public class EditModel : PageModel
{
    private readonly SchoolContext _context;

    public EditModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Student Student { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        Student = student;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var studentToUpdate = await _context.Students.FindAsync(id);
        if (studentToUpdate is null)
        {
            return NotFound();
        }

        if (!await TryUpdateModelAsync(
            studentToUpdate,
            "student",
            s => s.FirstMidName, s => s.LastName,
            s => s.EnrollmentDate))
        {
            return Page();
        }

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Failed to save changes. Try again.");
            return Page();
        }
    }
}
