using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students;

public class IndexModel : PageModel
{
    public enum SortOrder
    {
        NameAsc,
        NameDesc,
        DateAsc,
        DateDesc
    }

    private readonly SchoolContext _context;

    public IndexModel(SchoolContext context)
    {
        _context = context;
    }

    public IList<Student> Students { get;set; } = default!;

    /// <summary>
    /// Sorting parameter to be applied when name column header is pressed.
    /// </summary>
    public SortOrder NameNextSortOrder { get; set; }

    /// <summary>
    /// Sorting parameter to be applied when enrollment date column header is pressed.
    /// </summary>
    public SortOrder DateNextSortOrder { get; set; }

    public SortOrder CurrentSortOrder { get; set; }

    public async Task OnGetAsync(SortOrder? sortOrder)
    {
        NameNextSortOrder = sortOrder == SortOrder.NameAsc
            ? SortOrder.NameDesc : SortOrder.NameAsc;
        DateNextSortOrder = sortOrder == SortOrder.DateAsc
            ? SortOrder.DateDesc : SortOrder.DateAsc;
        CurrentSortOrder = sortOrder ?? SortOrder.NameAsc;

        IQueryable<Student> students = _context.Students;
        switch (CurrentSortOrder)
        {
            case SortOrder.NameAsc:
                students = students.OrderBy(s => s.LastName);
                break;

            case SortOrder.NameDesc:
                students = students.OrderByDescending(s => s.LastName);
                break;

            case SortOrder.DateAsc:
                students = students.OrderBy(s => s.EnrollmentDate);
                break;

            case SortOrder.DateDesc:
                students = students.OrderByDescending(s => s.EnrollmentDate);
                break;
        }

        if (_context.Students != null)
        {
            Students = await students.AsNoTracking().ToListAsync();
        }
    }
}
