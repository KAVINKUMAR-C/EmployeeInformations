using EmployeeInformations.Business.Utility.Caching;
using EmployeeInformations.Data.API.IRepository;
using EmployeeInformations.Data.API.Repository;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeInformations.DI
{
    public static class RepositoryHandlerModule
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            services.AddTransient<IEmployeesRepository, EmployeesRepository>();
            services.AddTransient<ILeaveRepository, LeaveRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddSingleton<MemoryCacheManager>();
            services.AddTransient<IProjectDetailsRepository, ProjectDetailsRepository>();
            services.AddTransient<ITimeSheetRepository, TimeSheetRepository>();
            services.AddTransient<IMasterRepository, MasterRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IAssetRepository, AssetRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IAuditLogRepository, AuditLogRepository>();
            services.AddTransient<IBenefitRepository, BenefitRepository>();
            services.AddTransient<IExpensesRepository, ExpensesRepository>();
            services.AddTransient<IEmailDraftRepository, EmailDraftRepository>();
            services.AddTransient<ICompanyPolicyRepository, CompanyPolicyRepository>();
            //services.AddTransient<IAttendanceRepository, AttendanceRepository>();
            services.AddTransient<IHomeRepository, HomeRepository>();
            services.AddTransient<IEmployeeSettingRepository, EmployeeSettingRepository>();
            services.AddTransient<IOBEmployeesRepository, OBEmployeesRepository>();
            services.AddTransient<IHelpdeskRepository, HelpdeskRepository>();
           // services.AddTransient<ITeamsMeetingRepository, TeamsMeetingRepository>();

            //API Repository
            //services.AddTransient<IWebsiteRepository, WebsiteRepository>();
            return services;
        }
    }
}
