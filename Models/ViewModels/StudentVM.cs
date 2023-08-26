using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.ViewModels;

public class StudentVM
{
    public string LastName { get; set; } = null!;
    public string FirstMidName { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; }
}