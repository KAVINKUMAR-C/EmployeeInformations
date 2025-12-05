using EmployeeInformations.Model.TimesheetSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface ITimeSheetService
    {
        Task<List<TimeSheet>> GetAllTimeSheet(int empId,int companyId);
        Task<int> CreateTimeSheet(TimeSheet timeSheet, int sessionEmployeeId,int companyId);
        Task<bool> DeleteTimeSheet(int TimeSheetId,int companyId);
        Task<TimeSheet> GetByTimeSheetId(int TimeSheetId,int companyId);
        Task<List<ProjectNames>> GetAllProjectNames(int empId,int companyId);
        Task<ViewTimeSheet> GetTimeSheetDetailsByTimeSheetId(int timeSheetId,int companyId);
        Task<TimeSheet> GetAllTimeSheets(SysDataTablePager pager, int empId, string columnDirection, string columnName,int companyId);
    }
}
