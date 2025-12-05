using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface ILeaveService
    {

        Task<List<EmployeeLeavesModel>> GetAllLeaveSummary(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId);
        Task<List<EmployeeLeaveViewModel>> GetAllWorkFromHomeSummary(int empId,int companyId);
        Task<List<CompensatoryRequestViewModel>> GetAllCompensatoryOffRequests(int empId,int companyId);
        Task<EmployeeLeaveViewModel> GetAllLeaveDetails(int empId,int companyId);
        Task<List<LeaveTypes>> GetAllLeave();
        Task<EmployeeLeaveViewModel> GetLeaveByAppliedLeaveId(int AppliedLeaveId,int companyId);
        Task<bool> CreateLeave(EmployeeLeaveViewModel leave, int sessionEmployeeId,int companyId);
        Task<bool> CreateCompensatory(CompensatoryRequest compensatoryRequest, int sessionEmployeeId);
        Task<bool> CreateHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId,int companyId);
        Task<bool> UpdateLeave(EmployeeLeaveViewModel leave, int sessionEmployeeId,int companyId);
        Task<bool> DeleteLeave(EmployeeAppliedLeave leave, int sessionEmployeeId,int companyId);
        Task<bool> ApprovedLeave(EmployeeAppliedLeave leave, int sessionEmployeeId,int companyId);
        Task<int> RejectLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId);
        Task<bool> ApprovedCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId);
        Task<int> RejectCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId);
        Task<EmployeeHolidaysViewModel> GetAllEmployeeHolidays(int year,int companyId);
        Task<bool> UpdateHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId);
        Task<bool> DeleteHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId,int companyId);
        Task<int> GetHolidayDate(string holidayDate,int companyId);
        Task<ViewEmployeeLeave> GetViewLeaveByAppliedLeaveId(int appliedLeaveId,int companyId);
        Task<ViewCompensatoryOffRequest> GetViewCompensatoryOffRequestByCompensatoryId(int compensatoryId,int companyId);
        //Task<List<EmployeeLeaveViewModel>> GetApplyEmployee(int empId);
        Task<List<EmployeeLeaveViewModel>> GetApporvedEmployee(int empId);     
        Task<int> GetHolidayDatesId(string holidayDate, int holidayid,int companyId);
        Task<bool> VerifyLeave(EmployeeLeaveViewModel leave, int sessionEmployeeId,int companyId);
        Task<string> GetToMails(int empId, string email,int companyId);
        Task<EmployeeLeaveViewModel> GetAllRemainingLeave(int empId,int companyId);
        Task<int> GetAllLeaveSummaryCount(SysDataTablePager pager, int empId,int companyId);       
        Task<List<EmployeeLeavesModel>> GetApporvedEmployees(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId);
        Task<int> WorkFromHomeCount(int empId, int companyId, SysDataTablePager pager);
        Task<int> WorkFromHomeForTeamLeadCount(int empId, int companyId, SysDataTablePager pager);
        Task<List<EmployeeLeavesModel>> GetApplyEmployee(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId);
        Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterData(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection);
        Task<int> GetEmployeesLeaveDetailsCount(SysDataTablePager pager, int empId, int companyId);
        Task<CompensatoryRequestViewModel> GetAllCompensatoryOffRequestsFilter(SysDataTablePager pager, int empId, string columnDirection, string columnName,int companyId);
        Task<int> GetAllCompensatoryOffRequestsFilterCount(SysDataTablePager pager, int empId,int companyId);
        Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterDataForTeamLead(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection);
    }
}
