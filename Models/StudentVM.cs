namespace ContosoUniversity.Models;

public class StudentVM
{
    public string LastName { get; set; } = null!;
    public string FirstMidName { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }
}