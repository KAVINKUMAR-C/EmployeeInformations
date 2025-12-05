
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface ITeamsMeetingRepository
    {
        Task<int> CreateTeamsMeeting(TeamsMeetingEntity teamsMeetingEntity,int companyId);
        Task<string> GetEmployeeOfficeMail(int empId,int companyId);
        Task<List<TeamMeetingModel>> GetTeamMeetingEmployeesList(SysDataTablePager pager, int empId, string columnDirection, string columnName,int companyId);
        Task<int> GetAllTeamMeetingByFilterCount(SysDataTablePager pager, int empId, int companyId);
        Task<TeamsMeetingEntity> GetByTeamsMeetingId(int teamsMeetingId, int companyId);
        Task<bool> DeleteTeamsMeeting(TeamsMeetingEntity teamsMeetingEntity);
    }
}
