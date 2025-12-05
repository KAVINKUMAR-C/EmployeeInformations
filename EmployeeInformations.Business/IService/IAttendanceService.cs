using EmployeeInformations.Model.AttendanceViewModel;
using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IAttendanceService
    {
        Task<AttendaceListViewModels> GetWorkingHourListForAll(int companyId);
        Task<AttendaceListViewModels> GetWorkingHourForAdmin(AttendaceListViewModel attendaceListViewModel);
        Task<AttendaceListViewModels> GetWorkingHourForEmployee(AttendaceListViewModel attendaceListViewModel);
        Task<AttendaceListViewModels> GetInOutListForAll(AttendaceListViewModel attendaceListViewModel,int companyId);
        Task<AttendaceListViewModels> GetAllEmployessByAttendanceFilter(AttendaceListViewModel attendaceListViewModel, int companyId);
        Task<bool> SendEmployeeAttendance(AttendaceListViewModel attendaceListViewModel, int companyId);
        Task<bool> SendEmployeeAttendanceForAllEmployee(AttendaceListViewModel attendaceListViewModel,int companyId);
        Task<bool> SendMail(AttendaceListViewModel attendaceListViewModel, List<string> fileId,int companyId);
        Task<AttendaceListViewModels> ViewAttendanceData(AttendaceListViewModel attendaceListViewModel,int companyId);
        Task<List<EmployeeDropdown>> GetByStatusId(int statusId,int companyId);
    }
}
