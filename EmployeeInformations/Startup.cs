using EmployeeInformations.Business.IService;
using EmployeeInformations.Business.Service;
using EmployeeInformations.CoreModels;
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
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

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

            // Optional: Uncomment if using SQL Server for Attendance
            // var attendanceDbConnection = Configuration.GetConnectionString("EmployeeAttendanceInfoDbConnection");
            // services.AddDbContext<AttendanceDbContext>(options => options.UseSqlServer(attendanceDbConnection));
        }
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    // ✅ PostgreSQL DB for Employees
        //    var dbConnectionString = Configuration.GetConnectionString("EmployeeInfoDbConnection");
        //    services.AddDbContext<EmployeesDbContext>(options =>
        //    {
        //        options.UseNpgsql(dbConnectionString, b =>
        //            b.MigrationsAssembly("EmployeeInformations.Data")); // 👈 This line is key
        //    });

        //    //services.AddDbContext<EmployeesDbContext>(options =>
        //    //{
        //    //    options.UseNpgsql(dbConnectionString);
        //    //});

        //    // ✅ SQL Server DB for Attendance (uncomment if needed)
        //    // var attendanceDbConnection = Configuration.GetConnectionString("EmployeeAttendanceInfoDbConnection");
        //    // services.AddDbContext<AttendanceDbContext>(options =>
        //    // {
        //    //     options.UseSqlServer(attendanceDbConnection);
        //    // });

        //    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //        .AddCookie(options =>
        //        {
        //            options.LoginPath = "/";
        //            options.AccessDeniedPath = "/";
        //        });

        //    services.RegisterServices();
        //    services.RegisterRepository();

        //    services.AddSingleton<ICompanyContext, CompanyContext>();
        //    services.AddSingleton<ILog, Log>();
        //    services.AddMemoryCache();
        //    services.AddControllersWithViews().AddRazorRuntimeCompilation();
        //    services.AddRazorPages();
        //    services.AddSingleton<IConfiguration>(Configuration);
        //    services.AddSingleton<ApprovalsSettings>(sp => sp.GetRequiredService<IOptions<ApprovalsSettings>>().Value);
        //    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        //    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //    services.AddApiVersioning();
        //    services.AddHttpClient();

        //    // ✅ Global authentication policy
        //    services.AddMvc(o =>
        //    {
        //        o.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
        //    });

        //    // ✅ Set Session Timeout. Default is 1 day.
        //    services.AddSession(options =>
        //    {
        //        options.IdleTimeout = TimeSpan.FromDays(1);
        //        // options.Cookie.HttpOnly = true;
        //        // options.Cookie.IsEssential = true;
        //    });

        //    services.ConfigureApplicationCookie(options =>
        //    {
        //        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        //        options.SlidingExpiration = true;
        //        options.LoginPath = "/Login/Login";
        //    });

        //    // services.AddSingleton<ICacheManager, MemoryCacheManager>();
        //    // services.AddSingleton<ILogService, LogService>();

        //    // ✅ Example: Job Scheduler with FluentScheduler (uncomment if needed)
        //    // var provider = services.BuildServiceProvider();
        //    // JobManager.Initialize(new EmployeeJobRegistry(
        //    //     provider.GetRequiredService<EmployeesDbContext>(),
        //    //     provider.GetRequiredService<AttendanceDbContext>(),
        //    //     provider.GetRequiredService<IHostingEnvironment>()
        //    // ));

        //    // services.AddSingleton<IHostedService, LifetimeEventsHostedService>();

        //    // ✅ Example: Use Autofac (if switching to Autofac DI)
        //    // var container = new ContainerBuilder();
        //    // container.Populate(services);
        //    // ILifetimeScope lifetimeScope = container.Build();
        //    // return new AutofacServiceProvider(lifetimeScope);

        //    // ✅ Example: Add SignalR (uncomment if needed)
        //    // services.AddSignalR(o =>
        //    // {
        //    //     o.EnableDetailedErrors = true;
        //    //     o.MaximumReceiveMessageSize = 102400000;
        //    // });
        //}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
    }
}