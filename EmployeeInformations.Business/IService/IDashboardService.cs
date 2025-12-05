using EmployeeInformations.Model.DashboardViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IDashboardService
    {
        Task<TotalEmployeeView> GetTotalEmployeeDashBorad(int companyId);
        Task<TotalProjectView> GetTotalProjectDashBoarad(int companyId);
        Task<TotalClientView> GetTotalClientDashBoarad(int companyId);
        Task<TotalActiveEmployeesView> GetTotalActiveEmployeesDashBoarad(int companyId);
        Task<TopFiveProjectsView> GetTotalFiveActiveProjectsDashBoarad(int companyId);
        Task<TopFiveDepartmentsView> GetTopFiveActiveDepartmentsDashBoarad(int companyId);
        Task<TopFiveLeaveView> GetTopFiveLeaveDashBoarad(int companyId);
        Task<bool> InsertTimeLog(TimeLoggerViewModel timeLoggerViewModel);
        Task<TimeLogView> GetLoggedInTotalTimeLog(TimeLoggerViewModel timeLoggerViewModel);
        Task<TopFiveCelebrationView> GetTopFiveCelebration(int companyId);
        Task<TopFiveAnnuncementView> GetTopFiveAnnuncements(int companyId);
        Task<TopFiveLeaveWfhView> GetTopFiveLeaveWfh(int companyId);
        Task<TotalEmployeeLeaveView> GetTotalEmployeeLeaveDashBorad(int companyId);
        Task<EmployeeLeaveCount> GetEmployeeLeaveCount(int empId, int companyId);
        Task<EmployeeWorkingHoursView> GetEmployeeTotalHoursForWeek(int employeeId,int companyId);
        Task<List<TopFiveLeaveViews>> GetTopFiveWfh();
        Task<List<TopFiveLeaveViews>> GetLeave(int empId);
        Task<List<Announcements>> GetAnnouncement();
        Task<Announcements> GetAnnouncementById(int annuncementId, int companyId);
        Task<TimeLogView> GetTimeLogViewModel(int employeeId,int companyId);
        Task<DashboardViewModel> GetAllDashboardView(int sessionEmployeeId, int roleId, int companyId);
        Task<TopFiveLeaveTypeView> GetLeaveApprove(int empId, int companyId);
        Task<TimeLogView> GetTimeLog(int employeeId,DateTime date,int companyId);
    }
}
