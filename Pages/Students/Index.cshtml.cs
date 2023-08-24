using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students;

public class IndexModel : PageModel
{
    private readonly SchoolContext _context;

    public IndexModel(SchoolContext context)
    {
        _context = context;
    }

    public IList<Student> Students { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Students != null)
        {
            Students = await _context.Students.ToListAsync();
        }
    }
}
