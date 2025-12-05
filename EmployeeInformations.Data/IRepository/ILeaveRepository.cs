using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface ILeaveRepository
    {

        Task<List<EmployeeAppliedLeaveEntity>> GetAllLeaveSummary(int empId,int companyId);
        Task<List<EmployeeAppliedLeaveEntity>> GetAllWorkFromHomeSummary(int empId,int companyId);
        Task<List<EmployeeAppliedLeaveEntity>> GetAllCompensatoryOffSummary(int empId, int companyId);
        Task<List<EmployeeAppliedLeaveEntity>> GetAllWorkFromHomeSummarys(List<int> empIds,int companyId);
        Task<List<EmployeeAppliedLeaveEntity>> GetAllCompensatoryOffSummarys(List<int> empIds, int companyId);
        Task<List<EmployeeLeaveViewModel>> GetLeaveByEmpId(int empId);
        Task<List<EmployeeLeaveViewModel>> GetAllEmployeesLeave(int companyId);
        Task<List<CompensatoryRequestsEntity>> GetAllCompensatoryOffRequest(int empId, int companyId);
        Task<EmployeeAppliedLeaveEntity> GetLeaveByAppliedLeaveId(int AppliedLeaveId,int companyId);
        Task<CompensatoryRequestsEntity> GetCompensatoryOffRequestByCompensatoryId(int CompensatoryId,int companyId);
        Task<List<EmployeeAppliedLeaveEntity>> GetLeaveByAppliedLeaveEmpId(int empId,int companyId);
        Task<List<LeaveTypesEntity>> GetAllLeave();
        Task<string> GetEmployeeEmailByEmpIdForLeave(int reporterEmpId);
        Task<AllLeaveDetailsEntity> GetAllLeaveDetails(int empId,int companyId);
        Task<List<CompensatoryRequestsEntity>> GetCompensatoryOffCountByEmpId(int empId,int companyId);
        Task<bool> CreateLeave(EmployeeAppliedLeaveEntity employeeAppliedLeaveEntity,int companyId);
        Task<bool> CreateCompensatory(CompensatoryRequestsEntity compensatoryRequestsEntity, int compensatoryId);
        Task<bool> CreateHoliday(EmployeeHolidaysEntity employeeHolidaysEntity,int companyId);
        Task<bool> UpdateLeave(EmployeeAppliedLeaveEntity employeeAppliedLeaveEntity);
        Task<bool> DeleteLeave(EmployeeAppliedLeave leave, int sessionEmployeeId,int companyId);
        Task<bool> ApprovedLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId);
        Task<int> RejectLeave(EmployeeAppliedLeave leave, int sessionEmployeeId,int companyId);
        Task<bool> ApprovedCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId);
        Task<int> RejectCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId);
        Task<List<EmployeeHolidaysEntity>> GetAllEmployeeHolidays(int companyId);
        Task<List<EmployeeHolidaysEntity>> GetAllEmployeeHolidaysForYear(int month, int year,int companyId);
        Task<bool> UpdateHoliday(EmployeeHolidaysEntity employeeHolidaysEntity);
        Task<bool> DeleteHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId,int companyId);
        Task<int> GetHolidayDate(string holidayDate,int companyId);
        Task<List<EmployeeAppliedLeaveEntity>> GetAllLeaveSummarys(List<int> empIds,int companyId);
        Task<List<CompensatoryRequestsEntity>> GetAllCompensatoryOffRequests(List<int> empIds,int companyId);
        Task<bool> InsertAllLeaveDetailsByEmpId(AllLeaveDetailsEntity allLeaveDetailsEntity);
        Task<List<EmployeeAppliedLeaveEntity>> GetAllLeaveDashboard(int companyId);
        Task<LeaveTypesEntity> GetAllLeaveDetailsDashboard(int AppliedLeaveTypeId,int companyId);
        Task<CompensatoryRequestsEntity> GetCompensatoryRequestByCompensatoryId(int compensatoryId);
        Task<bool> UpdateAllLeaveDeatils(AllLeaveDetailsEntity allLeaveDetailsEntity);
        Task<List<EmployeeAppliedLeaveEntity>> GetEmployeeIdsLeave(int empId,int companyId);
        Task<int> GetHolidayDatesId(string holidayDate, int holidayid,int companyId);

        Task<List<TopFiveLeaveViews>> GetTopFiveWfh();
        Task<List<TopFiveLeaveViews>> TopFiveLeaveTypeView(int empId);
        Task<List<LeaveList>> EmployeeHolidays(int companyId);
        Task<AllLeaveDetailsEntity> GetLeaveDetailsByEmpIdAndCompanyid(int empId, int companyId);
        Task<List<EmployeeLeavesModel>> GetAllEmployeesLeaves(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId);
        Task<List<EmployeeLeaveViewModel>> GetReportingPersonsLeave(int empId);
        Task<List<EmployeeLeaveViewModel>> GetAllWorkFromHome(int companyId);
        Task<List<EmployeeLeaveViewModel>> GetEmployeeReportingWorkFromHome(int empId,int companyId);
        Task<List<CompensatoryRequestViewModel>> GetAllEmployeeComOff(int companyId);
        Task<List<CompensatoryRequestViewModel>> GetReportingEmployeesComOff(int empId,int companyId);
        Task<EmployeeHolidaysViewModel> GetAllEmployeesHolidays(int year,int companyId);
        Task<List<LeaveTypes>> GetAllLeaveTypes();
        Task<List<EmployeeLeaveViewModel>> GetEmployeeLeaveViewModel(int companyId);
        Task<List<EmployeeLeaveViewModel>> GetEmployeeLeaveByEmpId(int empId,int companyId);
        Task<int> GetAllEmployeeHolidaysForMOnth(DateTime startdate, DateTime enddate, int companyId);
        Task<int> GetAllLeaveSummaryCount(SysDataTablePager pager, int empId, int companyId);        
        Task<int> WorkFromHomeCount(int empId, int companyId, SysDataTablePager pager);
        Task<int> WorkFromHomeForTeamLeadCount(int empId, int companyId, SysDataTablePager pager);
        Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterData(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection);
        Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterDataForTeamLead(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection);
        Task<List<EmployeeLeavesModel>> GetReportingPersonsLeaves(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId);       
        Task<List<EmployeeLeavesModel>> GetLeavesByEmpId(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId);
        Task<int> GetEmployeesLeaveDetailsCount(SysDataTablePager pager, int empId,int companyId);
        Task<List<EmployeeCompensatoryFilter>> GetAllEmployeeCompenSatoryDetails(SysDataTablePager pager, int Employee, string columnDirection, string columnName,int companyId);
        Task<int> GetAllEmployeeCompenSatoryDetailsCount(SysDataTablePager pager, int Employee,int companyId);
        Task<List<LeaveTypesEntity>> GetLeaveType(int empId);
    }
}
