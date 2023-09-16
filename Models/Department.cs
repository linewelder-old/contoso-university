using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models;

public class Department
{
    public int DepartmentID { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [DataType(DataType.Currency)]
    [Column(TypeName = "money")]
    public decimal Budget { get; set; }

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime StartDate { get; set; }

    public int? InstructorID { get; set; }

    public Guid ConcurrencyToken { get; set; } = Guid.NewGuid();

    public Instructor? Administrator { get; set; }
    public ICollection<Course>? Courses { get; set; }
}
