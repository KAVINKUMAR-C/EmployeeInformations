using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using Microsoft.OpenApi.Models;
using EmployeeInformations.DI;
using EmployeeInformations.Model.EmployeesViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace EmployeeInformations.WebAPI
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
            //services.AddDbContext<EmployeesDbContext>(options =>
            //{
            //    var appSettings = new ConfigurationBuilder()
            //                        .SetBasePath(Directory.GetCurrentDirectory())
            //                        .AddJsonFile("appsettings.json")
            //                        .Build();
            //    var dbConnectionString = appSettings.GetSection("AppSettings").GetSection("EmployeeInfoDbConnection").Value.ToString();
            //    //options.UseSqlServer("data source=172.16.2.66;database=EmployeeInfo;user Id=sa;password=Admin@123");
            //    options.UseSqlServer(dbConnectionString);
            //});
            //services.AddDbContext<AttendanceDbContext>(options =>
            //{
            //    var appSettings = new ConfigurationBuilder()
            //                        .SetBasePath(Directory.GetCurrentDirectory())
            //                        .AddJsonFile("appsettings.json")
            //                        .Build();
            //    var dbConnectionString = appSettings.GetSection("AppSettings").GetSection("EmployeeAttendanceInfoDbConnection").Value.ToString();
            //    options.UseSqlServer(dbConnectionString);
            //});
            var employeeDbConnection = Configuration.GetConnectionString("EmployeeInfoDbConnection");
            var attendanceDbConnection = Configuration.GetConnectionString("EmployeeAttendanceInfoDbConnection");

            // ✅ Register EmployeesDbContext with PostgreSQL
            services.AddDbContext<EmployeesDbContext>(options =>
            {
                options.UseNpgsql(employeeDbConnection, b =>
                    b.MigrationsAssembly("EmployeeInformations.Data")); // Change only if migrations are stored here
            });

            // ✅ Register AttendanceDbContext with SQL Server
            services.AddDbContext<AttendanceDbContext>(options =>
            {
                options.UseNpgsql(attendanceDbConnection);
            });

            services.RegisterServices();
            services.RegisterRepository();
            services.AddSingleton<ICompanyContext, CompanyContext>();
            // services.AddSingleton<ILog, Log>();
            services.AddMemoryCache();
            services.AddRazorPages();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ApprovalsSettings>(sp => sp.GetRequiredService<IOptions<ApprovalsSettings>>().Value);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddApiVersioning();
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddMvc();
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HRMS",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}


            /// Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRMS V1");
            });
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
