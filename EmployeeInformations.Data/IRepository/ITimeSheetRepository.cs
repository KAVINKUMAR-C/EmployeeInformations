using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.TimesheetSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface ITimeSheetRepository
    {

        Task<List<TimeSheetEntity>> GetAllTimeSheet(int empId,int companyId);
        Task<int> CreateTimeSheet(TimeSheetEntity timeSheetEntity,int companyId);
        Task<TimeSheetEntity> GetByTimeSheetId(int timeSheetId,int companyId);
        Task<bool> DeleteTimeSheet(int TimeSheetId,int companyId);
        Task<List<ProjectDetailsEntity>> GetAllProjectNames(int empId, int companyId);
        Task<string> GetProjectName(int projrctId,int companyId);
        Task<List<TimeSheetEntity>> GetAllTimeSheetByCurrentDate(DateTime flterDate,int companyId);
        Task<List<TimeSheetEntity>> GetAllTimeSheetByCurrentDateAndCompanyId(DateTime flterDate, int companyId);
        Task<List<TimeSheet>> GetAllEmployeesTimeSheet(int companyId);
        Task<List<TimeSheet>> GetEmployeeTimesheet(int empId, int companyId);
        Task<List<TimeSheetModel>> GetTimeSheetByEmpIdFilterList(SysDataTablePager pager, int empId, string columnDirection, string columnName,int companyId);
        Task<int> GetAllTimeSheetListCount(SysDataTablePager pager, int empId,int companyId);
        Task<List<TimeSheetEntity>> GetAllTimeSheets(List<int> empIds, int companyId);
    }
}
