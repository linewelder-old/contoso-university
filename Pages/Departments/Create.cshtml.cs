using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Departments;

public class CreateModel : PageModel
{
    private readonly SchoolContext _context;

    public CreateModel(SchoolContext context)
    {
        _context = context;
    }

    public override PageResult Page()
    {
        InstructorSelectList = new SelectList(_context.Instructors, "ID", "FullName");
        return base.Page();
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Department Department { get; set; } = null!;

    public SelectList InstructorSelectList { get; set; } = null!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var department = new Department();
        if (!await TryUpdateModelAsync(
                department, "department",
                d => d.Name, d => d.Budget,
                d => d.StartDate, d => d.InstructorID))
        {
            return Page();
        }

        try
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Failed to save the new department. Try again later.");
            return Page();
        }
    }
}
