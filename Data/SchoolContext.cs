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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        modelBuilder.Entity<Course>().ToTable("Course");
    }
}
