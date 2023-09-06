namespace ContosoUniversity.Models.ViewModels;

public class AssignedCourse
{
    public int CourseID { get; set; }
    public string Title { get; set; } = null!;
    public bool Assigned { get; set; }
}
