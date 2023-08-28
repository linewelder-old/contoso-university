using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity;

public class DepartmentList : SelectList
{
    public DepartmentList(SchoolContext _context, object? selected = null)
        : base(
            _context.Departments.OrderBy(d => d.Name),
            nameof(Department.DepartmentID),
            nameof(Department.Name),
            selected
        ) {}
}
