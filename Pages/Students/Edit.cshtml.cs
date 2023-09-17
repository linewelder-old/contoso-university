using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students;

public class EditModel : PageModel
{
    private readonly SchoolContext _context;
    private readonly ILogger<EditModel> _logger;

    public EditModel(SchoolContext context, ILogger<EditModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Student Student { get; set; } = default!;
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, bool? saveChangesError)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        if (saveChangesError.GetValueOrDefault())
        {
            ErrorMessage = $"Update {id} failed. Try again.";
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
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Edit student {} failed.", id);
            return RedirectToAction("./Edit",
                new { id, saveChangesError = true });
        }
    }
}
