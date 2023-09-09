using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.ViewModels;

public class EnrollmentDateGroup
{
    [Display(Name = "Enrollment Date"), DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }
    [Display(Name = "Students")]
    public int StudentCount { get; set; }
}
