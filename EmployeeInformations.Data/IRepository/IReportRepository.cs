using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IReportRepository
    {
        Task<List<LeaveReportDateModel>> GetAllEmployessByLeaveTypeFilter(string proc, List<KeyValuePair<string, string>> values);
        Task<List<ManualLogReportDateModel>> GetAllEmployessByManualLogFilter(string proc, List<KeyValuePair<string, string>> values);
        Task<List<TimeSheetDataModel>> GetAllEmployessByTimeSheetFilter(string proc, List<KeyValuePair<string, string>> values);
        Task<bool> ImoprtFile(List<ManualLogEntity> ManualLogEntitys);       
        //Task<List<EmployeeDropdown>> GetAllEmployeesDropdown(int companyId);
        Task<List<ProjectNames>> GetAllProjectNames(int empId,int companyId);
        Task<List<ManualLog>> GetAllManulLogs();
        Task<List<EmployeeDropdown>> GetAllEmployeesDropdown(int companyId);
        Task<int> GetAllEmployeesListCount(SysDataTablePager pager,int companyId);
        Task<List<EmployeesDataModel>> GetAllEmployeesList(SysDataTablePager pager, string columnDirection, string ColumnName,int companyId);
        Task<List<EmployeesDataModel>> GetAllEmployeeFilter(string proc, List<KeyValuePair<string, string>> values);
        Task<int> GetFilterTimeSheetReportCount(TimeSheetReports timeSheetReports, SysDataTablePager pager);
        Task<List<TimeSheetReportViewModel>> GetTimeSheetFilter(TimeSheetReports timeSheetReports,SysDataTablePager pager, string columnName, string columnDirection);
        Task<List<EmployeeLeaveReportDataModel>> GetLeaveReportByEmployeeId(SysDataTablePager pager, Reports reports, string columnDirection, string columnName);
        Task<int> GetLeaveReportByEmployeeIdCount(SysDataTablePager pager, Reports reports);
        
    }
}
