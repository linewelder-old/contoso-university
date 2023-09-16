using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments;

public class IndexModel : PageModel
{
    private readonly SchoolContext _context;

    public IndexModel(SchoolContext context)
    {
        _context = context;
    }

    public IList<Department> Department { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Department = await _context.Departments
            .Include(d => d.Administrator).ToListAsync();
    }
}
