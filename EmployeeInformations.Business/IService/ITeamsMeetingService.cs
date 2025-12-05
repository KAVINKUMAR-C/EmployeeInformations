using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.TeamsViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface ITeamsMeetingService
    {
        Task<int> CreateTeamsMeeting(Teams teams, int sessionEmployeeId,int companyId);
        Task<string> GetEmployeeOfficeMail(int sessionEmployeeId,int companyId);
        Task<Teams> GetTeamMeetingEmployeesList( SysDataTablePager pager, int empId, string columnDirection, string columnName,int companyId);
        Task<int> GetAllTeamMeetingByFilterCount(int empId, SysDataTablePager pager, int companyId);
        Task<Teams> GetByTeamsMeetingId(int teamsMeetingId, int companyId);
        Task<bool> DeleteTeamsMeeting(int teamsMeetingId,int companyId);
    }
}
