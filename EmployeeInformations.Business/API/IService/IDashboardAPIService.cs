using EmployeeInformations.Model.APIDashboardModel;

namespace EmployeeInformations.Business.API.IService
{
    public interface IDashboardAPIService
    {
        Task<TotalEmployeeViewAPIRequest> GetTotalEmployeeDashBorad(int companyId);
        Task<TotalProjectViewAPIRequest> GetTotalProjectDashBoarad(int companyId);
        Task<TotalClientViewAPIRequest> GetTotalClientDashBoarad(int companyId);
        Task<TopFiveDepartmentsViewAPI> GetTopFiveActiveDepartmentsDashBoarad(int companyId);
        Task<TopFiveProjectsViewAPI> GetTotalFiveActiveProjectsDashBoarad(int companyId);
        Task<TopFiveLeaveViewAPI> GetTopFiveLeaveDashBoarad(int companyId);
        Task<TopFiveCelebrationViewAPI> GetTopFiveCelebration(int companyId);
        Task<TopFiveAnnuncementViewAPI> GetTopFiveAnnuncements(int companyId);
        Task<TotalEmployeeLeaveViewAPI> GetTotalEmployeeLeaveDashBorad(int companyId);
        Task<TopFiveLeaveTypeViewAPI> GetEmpLeave(int empId, int companyId);
        Task<EmployeeLeaveCountAPI> GetEmployeeLeaveCount(int empId,int companyId);
        Task<TimeLogViewAPI> GetTimeLogViewModel(int employeeId, int companyId);
        Task<EmployeeWorkingHoursViewAPI> GetEmployeeTotalHoursForWeek(int employeeId,int companyId);
        Task<UserDashboardResponse> InsertTimeLog(TimeLoggerViewModelAPI timeLoggerViewModel);
        Task<UserTimeResponse> GetLoggedInTotalTimeLog(TimeLoggerViewModelAPI timeLoggerViewModel);
        Task<UserDashboardResponse> GetAnnouncementById(int announcementId, int companyId);
        Task<DashboardViewModelAPI> GetAllDashboardView(int employeeId, int roleId, int companyId);
        Task<TopFiveLeaveWfhViewAPI> GetTopFiveLeaveWfh(int companyId);
        Task<TopFiveLeaveTypeViewAPI> GetLeaveApprove(int empId, int companyId);

    }
}
