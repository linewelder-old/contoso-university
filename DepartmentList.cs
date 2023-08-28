using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity;

public class DepartmentList : SelectList
{
    public DepartmentList(SchoolContext context, object? selected = null)
        : base(
            context.Departments.OrderBy(d => d.Name),
            nameof(Department.DepartmentID),
            nameof(Department.Name),
            selected
        ) {}
}
