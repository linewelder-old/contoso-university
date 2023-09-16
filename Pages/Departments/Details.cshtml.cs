using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments;

public class DetailsModel : PageModel
{
    private readonly SchoolContext _context;

    public DetailsModel(SchoolContext context)
    {
        _context = context;
    }

    public Department Department { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Administrator)
            .FirstOrDefaultAsync(d => d.DepartmentID == id);
        if (department == null)
        {
            return NotFound();
        }

        Department = department;
        return Page();
    }
}
