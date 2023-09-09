using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages;

public class AboutModel : PageModel
{
    private readonly SchoolContext _context;

    public AboutModel(SchoolContext context)
    {
        _context = context;
    }

    public IList<StudentAvgGrade> BestStudents { get; set; } = null!;
    public IList<EnrollmentDateGroup> EnrollmentsByDate { get; set; } = null!;
    
    public async Task OnGetAsync()
    {
        IQueryable<StudentAvgGrade> studentsGradesQuery =
            from s in _context.Students.Include(s => s.Enrollments)
            let avgGrade = s.Enrollments
                .Select(e => (double?)e.Grade)
                .Average()
            where avgGrade != null
            orderby avgGrade
            select new StudentAvgGrade {
                FirstMidName = s.FirstMidName,
                LastName = s.LastName,
                AvgGrade = (Grade)avgGrade.Value
            };
        BestStudents = await studentsGradesQuery
            .AsNoTracking()
            .Take(3)
            .ToListAsync();
        
        IQueryable<EnrollmentDateGroup> enrollmentsByDate =
            from student in _context.Students
            group student by student.EnrollmentDate into dateGroup
            select new EnrollmentDateGroup {
                EnrollmentDate = dateGroup.Key,
                StudentCount = dateGroup.Count()
            };
        EnrollmentsByDate = await enrollmentsByDate.AsNoTracking().ToListAsync();
    }
}
