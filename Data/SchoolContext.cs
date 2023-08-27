using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data;

public class SchoolContext : DbContext
{
    public SchoolContext (DbContextOptions<SchoolContext> options)
        : base(options) {}

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Instructor> Instructors { get; set; } = null!;
    public DbSet<OfficeAssignment> OfficeAssignments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Instructor>().ToTable(nameof(Instructor));
        modelBuilder.Entity<Student>().ToTable(nameof(Student));
        modelBuilder.Entity<Enrollment>().ToTable(nameof(Enrollment));
        modelBuilder.Entity<Course>().ToTable(nameof(Course))
            .HasMany(c => c.Instructors)
            .WithMany(i => i.Courses);
    }
}
