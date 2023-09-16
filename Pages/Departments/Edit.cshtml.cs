using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments;

public class EditModel : PageModel
{
    private readonly SchoolContext _context;

    public EditModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Department Department { get; set; } = null!;

    public SelectList InstructorSelectList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var department =  await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentID == id);
        if (department == null)
        {
            return NotFound();
        }

        Department = department;
        PopulateInstructorSelectList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var departmentToUpdate = await _context.Departments
            .Include(d => d.Administrator)
            .FirstOrDefaultAsync(d => d.DepartmentID == id);

        if (departmentToUpdate is null)
        {
            ModelState.AddModelError("", "The department was deleted by another user.");
            PopulateInstructorSelectList();
            return Page();
        }

        departmentToUpdate.ConcurrencyToken = Guid.NewGuid();
        // EF uses this to check the concurrency token stored in the database.
        _context
            .Entry(departmentToUpdate)
            .Property(d => d.ConcurrencyToken)
            .OriginalValue = Department.ConcurrencyToken;

        if (await TryUpdateModelAsync(
                departmentToUpdate,
                "department",
                d => d.Name, d => d.StartDate,
                d => d.Budget, d => d.InstructorID))
        {
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException e)
            {
                var entry = e.Entries.Single();
                var dbValues = await entry.GetDatabaseValuesAsync();
                if (dbValues is null)
                {
                    ModelState.AddModelError("", "The department was deleted by another user.");
                    PopulateInstructorSelectList();
                    return Page();
                }

                var dbEntity = (Department)dbValues.ToObject();
                await DisplayConcurrencyErrors(dbEntity, (Department)entry.Entity);

                // Update the expected concurrency token to accept changes on the next save.
                Department.ConcurrencyToken = dbEntity.ConcurrencyToken;
                // Remove the cached concurrency token for the new one to be displayed in the input field.
                ModelState.Remove($"{nameof(Department)}.{nameof(Department.ConcurrencyToken)}");
            }
        }

        PopulateInstructorSelectList();
        return Page();
    }

    private void PopulateInstructorSelectList()
    {
        InstructorSelectList = new SelectList(_context.Instructors, "ID", "FullName");
    }

    private async Task DisplayConcurrencyErrors(Department dbEntity, Department clientEntity)
    {
        if (dbEntity.Name != clientEntity.Name)
        {
            ModelState.AddModelError("Department.Name",
                $"Current value: {dbEntity.Name}");
        }

        if (dbEntity.Budget != clientEntity.Budget)
        {
            ModelState.AddModelError("Department.Budget",
                $"Current value: {dbEntity.Budget:C}");
        }

        if (dbEntity.StartDate != clientEntity.StartDate)
        {
            ModelState.AddModelError("Department.StartDate",
                $"Current value: {dbEntity.StartDate:d}");
        }

        if (dbEntity.InstructorID != clientEntity.InstructorID)
        {
            var instructor = await _context.Instructors.FindAsync(dbEntity.InstructorID);
            ModelState.AddModelError("Department.StartDate",
                $"Current value: {instructor?.FullName}");
        }

        ModelState.AddModelError("",
            "The department information has been updated since the page was loaded. " +
            "Your edit was cancelled and the current values have been displayed. " +
            "Click Save again to overwrite the information with your values.");
    }
}
