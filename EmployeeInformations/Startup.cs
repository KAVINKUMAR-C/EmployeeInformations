using DocumentFormat.OpenXml.InkML;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Business.Service;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataSeeder;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Data.Repository;
using EmployeeInformations.DI;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Schedules;
using FluentScheduler;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeeInformations
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // PostgreSQL DB for Employees
            var dbConnectionString = Configuration.GetConnectionString("EmployeeInfoDbConnection");
            var attendanceDbConnectionString = Configuration.GetConnectionString("EmployeeAttendanceInfoDbConnection");

            // EmployeesDbContext
            services.AddDbContext<EmployeesDbContext>(options =>
                options.UseNpgsql(dbConnectionString, b =>
                    b.MigrationsAssembly("EmployeeInformations.CoreModels")));

            // AttendanceDbContext
            services.AddDbContext<AttendanceDbContext>(options =>
                options.UseNpgsql(attendanceDbConnectionString, b =>
                    b.MigrationsAssembly("EmployeeInformations.CoreModels")));

            // ✅ Authentication & Authorization
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login";
                    options.AccessDeniedPath = "/";
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.SlidingExpiration = true;
                });

            services.AddMvc(o =>
            {
                o.Filters.Add(new AuthorizeFilter(
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build()));
            });

            // ✅ Dependency Injection
            services.RegisterServices();
            services.RegisterRepository();
            services.AddSingleton<ICompanyContext, CompanyContext>();
            services.AddSingleton<ILog, Log>();

            // Add DatabaseSeeder as Scoped service
            services.AddScoped<DatabaseSeeder>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ✅ Configuration, AutoMapper, MemoryCache, HttpClient
            services.AddSingleton<ApprovalsSettings>(sp => sp.GetRequiredService<IOptions<ApprovalsSettings>>().Value);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddApiVersioning();

            // Other services
            services.AddControllersWithViews();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IAttendanceService, AttendanceService>();

            // Register your Dashboard service
            services.AddScoped<IDashboardService, DashboardService>();

            // ✅ Session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Seed database on startup
            SeedDatabase(app, logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRequestResponseLogging(); // ✅ Your custom middleware
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // ✅ Enable CORS globally
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}");
                endpoints.MapRazorPages();

                // ✅ Fallback for unmatched routes
                endpoints.MapFallbackToController("Login", "Login");
            });
        }

        private void SeedDatabase(IApplicationBuilder app, ILogger<Startup> logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    logger.LogInformation("Starting database seeding...");

                    var context = scope.ServiceProvider.GetRequiredService<EmployeesDbContext>();
                    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

                    // Apply migrations if needed
                    context.Database.Migrate();

                    // Seed the database
                    seeder.Seed();

                    logger.LogInformation("Database seeding completed successfully!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    // Don't throw here to allow the app to start even if seeding fails
                }
            }
        }
    }
}