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
    private readonly IConfiguration _configuration;

    public IndexModel(SchoolContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    /// Sorting parameter to be applied when name column header is pressed.
    /// </summary>
    public SortOrder NameNextSortOrder { get; set; }

    /// <summary>
    /// Sorting parameter to be applied when enrollment date column header is pressed.
    /// </summary>
    public SortOrder DateNextSortOrder { get; set; }

    public string CurrentFilter { get; set; } = null!;
    public SortOrder CurrentSortOrder { get; set; }

    public PaginatedList<Student> Students { get; set; } = null!;

    public async Task OnGetAsync(
        SortOrder sortOrder = default, string searchString = "", int pageIndex = 1)
    {
        NameNextSortOrder = sortOrder == SortOrder.NameAsc
            ? SortOrder.NameDesc : SortOrder.NameAsc;
        DateNextSortOrder = sortOrder == SortOrder.DateDesc
            ? SortOrder.DateAsc : SortOrder.DateDesc;

        CurrentSortOrder = sortOrder;
        CurrentFilter = searchString;

        IQueryable<Student> students = _context.Students;
        if (!string.IsNullOrEmpty(searchString))
        {
            students = students.Where(
                s => s.LastName.Contains(searchString) ||
                     s.FirstMidName.Contains(searchString));
        }

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

        var pageSize = _configuration.GetValue("PageSize", 4);
        Students = await PaginatedList<Student>.CreateAsync(
            students.AsNoTracking(), pageIndex, pageSize);
    }
}
