using System;

namespace EmployeeInformations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Allow local DateTime to work with PostgreSQL timestamptz
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}