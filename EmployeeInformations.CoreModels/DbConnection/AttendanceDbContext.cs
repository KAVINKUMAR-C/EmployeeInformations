using EmployeeInformations.CoreModels.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DbConnection
{
    public class AttendanceDbContext : DbContext
    {
        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : base(options)
        {

        }
        public DbSet<AttendanceEntity> AttendanceEntitys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: set default schema globally for all tables
            modelBuilder.HasDefaultSchema("public");

            // Explicit mapping for EmployeesEntity in case PostgreSQL needs it
            //modelBuilder.Entity<EmployeesEntity>().ToTable("employees", "public");
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp without time zone");
                    }
                }
            }
        }
    }
}
