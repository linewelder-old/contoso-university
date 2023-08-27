using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models;

public class Instructor
{
    public int ID { get; set; }

    [Display(Name = "Last Name")]
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Display(Name = "First Name")]
    [Column("FirstName")]
    [Required]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    public string FirstMidName { get; set; } = null!;

    [Display(Name = "Hire Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime HireDate { get; set; }

    [Display(Name = "Full Name")]
    public string FullName => $"{LastName}, {FirstMidName}";

    public ICollection<Course>? Courses { get; set; }
    public OfficeAssignment? OfficeAssignment { get; set; }
}
