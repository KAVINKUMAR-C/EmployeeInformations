using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Business.API.Service;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Business.Service;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeInformations.DI
{
    public static class ServiceHandlerModule
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IEmployeeInformationService, EmployeeInformationService>();
            services.AddTransient<IEmployeesService, EmployeesService>();
            services.AddTransient<ILeaveService, LeaveService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IProjectDetailsService, ProjectDetailsService>();
            //services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<ITimeSheetService, TimeSheetService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IMasterService, MasterService>();
            services.AddTransient<IAssetService, AssetService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<ICompanyService, CompanyService>();
            //services.AddTransient<IAuditLogService, AuditLogService>();
            services.AddTransient<IBenefitService, BenefitService>();
            services.AddTransient<IEmailDraftService, EmailDraftService>();
            services.AddTransient<ICompanyPolicyService, CompanyPolicyService>();
            //services.AddTransient<IAttendanceService, AttendanceService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IExpensesService, ExpensesService>();
            services.AddTransient<IEmployeeSetting, EmployeeSettingService>();
            services.AddTransient<IOBEmployeeService, OBEmployeeService>();
			//services.AddTransient<IBackGroundService, BackGroundService>();
            services.AddTransient<IHelpdeskService, HelpdeskService>();
            //services.AddTransient<ITeamsMeetingService,TeamsMeetingService>();

            //API Service			
            //services.AddTransient<IDashboardAPIService, DashboardAPIService>();
            //services.AddTransient<ILoginAPIService, LoginAPIService>();
            //services.AddTransient<IEmployeesAPIService, EmployeesAPIService>();
            //services.AddTransient<ILeaveAPIService, LeaveAPIService>();
            //services.AddTransient<ITimeSheetAPIService, TimeSheetAPIService>();
            //services.AddTransient<IWebsiteService, WebsiteService>();
            return services;
        }
    }
}
