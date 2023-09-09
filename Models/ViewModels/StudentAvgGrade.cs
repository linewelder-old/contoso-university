using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.ViewModels;

public class StudentAvgGrade
{
    [Display(Name = "First Name")] public string FirstMidName { get; set; } = null!;
    [Display(Name = "Last Name")] public string LastName { get; set; } = null!;
    [Display(Name = "Average Grade")] public Grade AvgGrade { get; set; }
}
