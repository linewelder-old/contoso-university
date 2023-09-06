using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;

namespace ContosoUniversity.Pages.Instructors;

public class InstructorPageModel : PageModel
{
    public List<AssignedCourse> AssignedCourses { get; set; } = new();

    public async Task PopulateAssignedCourses(
        SchoolContext context, Instructor instructor)
    {
        if (instructor.Courses is null)
        {
            throw new ArgumentNullException(null, "Instructor must have assigned courses loaded.");
        }

        AssignedCourses.Clear();
        await foreach (var course in context.Courses)
        {
            AssignedCourses.Add(new AssignedCourse {
                CourseID = course.CourseID,
                Title = course.Title,
                Assigned = instructor.Courses.Any(
                    ic => ic.CourseID == course.CourseID)
            });
        }
    }
}
