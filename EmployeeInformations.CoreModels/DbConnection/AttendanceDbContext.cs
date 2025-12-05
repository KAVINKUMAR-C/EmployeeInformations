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

    }
}
