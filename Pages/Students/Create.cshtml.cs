using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Students;

public class CreateModel : PageModel
{
    private readonly SchoolContext _context;
    private readonly ILogger<DeleteModel> _logger;

    public CreateModel(SchoolContext context, ILogger<DeleteModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet(bool? saveChangesError = false)
    {
        if (saveChangesError.GetValueOrDefault())
        {
            ErrorMessage = $"Create failed. Try again.";
        }

        return Page();
    }

    [BindProperty]
    public StudentVM StudentVM { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var entry = _context.Add(new Student());
            entry.CurrentValues.SetValues(StudentVM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Create student failed.");
            return RedirectToAction("./Create",
                new { saveChangesError = true });
        }
    }
}
