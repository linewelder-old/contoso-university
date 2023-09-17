using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments;

public class DeleteModel : PageModel
{
    private readonly SchoolContext _context;

    public DeleteModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Department Department { get; set; } = default!;
    public string? ConcurrencyErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, bool? concurrencyError)
    {
        var department = await _context.Departments
            .Include(d => d.Administrator)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DepartmentID == id);
        if (department is null) return NotFound();
        Department = department;

        if (concurrencyError.GetValueOrDefault())
        {
            ConcurrencyErrorMessage =
                "The department information has been updated since the page was loaded. " +
                "Your delete request was cancelled and the current values have been displayed. " +
                "Click Delete again if you still want to delete the department.";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            if (!await _context.Departments.AnyAsync(
                    d => d.DepartmentID == id))
            {
                return RedirectToPage("./Index");
            }

            _context.Departments.Remove(Department);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (DbUpdateConcurrencyException)
        {
            return RedirectToPage("./Delete",
                new { id, concurrencyError = true });
        }
    }
}
