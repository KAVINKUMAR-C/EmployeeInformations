using EmployeeInformations.CoreModels.DbConnection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace EmployeeInformations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Allow local DateTime to work with PostgreSQL timestamptz
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var host = CreateHostBuilder(args).Build();

            // ✅ Check if migration argument is passed
            if (args != null && args.Length > 0 && args[0] == "--migrate")
            {
                RunMigrations(host);
                return;
            }

            host.Run();
        }

        private static void RunMigrations(IHost host)
        {
            Console.WriteLine("🚀 Starting database migrations for Render deployment...");

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Migrating Employees database...");
                    var employeesContext = services.GetRequiredService<EmployeesDbContext>();
                    employeesContext.Database.Migrate();
                    logger.LogInformation("✅ Employees database migrated successfully.");

                    logger.LogInformation("Migrating Attendance database...");
                    var attendanceContext = services.GetRequiredService<AttendanceDbContext>();
                    attendanceContext.Database.Migrate();
                    logger.LogInformation("✅ Attendance database migrated successfully.");

                    Console.WriteLine("🎉 All migrations completed successfully!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "❌ An error occurred while migrating the database.");
                    Console.WriteLine($"❌ Migration failed: {ex.Message}");
                    throw;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}