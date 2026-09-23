using Microsoft.EntityFrameworkCore;
using StudentManagment.Models;

namespace StudentManagment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AcademicYear> AcademicYears { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Programme> Programme { get; set; }
    }
}