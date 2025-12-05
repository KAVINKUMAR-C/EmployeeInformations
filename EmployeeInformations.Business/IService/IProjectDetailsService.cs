using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ProjectSummaryViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IProjectDetailsService
    {
        Task<List<ProjectDetailsModel>> GetAllProjectDetails(SysDataTablePager pager, string columnName, string columnDirection,int companyId);
        Task<ProjectDetails> GetByProjectId(int ProjectId,int companyId);
        Task<int> CreateProject(ProjectDetails projectDetails, int sessionEmployeeId,int companyId);
        Task<List<ProjectTypes>> GetAllProjectTypes(int companyId);
        Task<List<ProjectTypes>> GetByProjectTypeId(int ProjectTypeId,int companyId);
        Task<bool> DeleteProject(int ProjectId,int companyId);
        Task<string> GetProjectId(int companyId);
        Task<List<ClientCompanys>> GetAllClientCompany(int companyId);
        Task<List<DropdownTeamLead>> GetByEmployeeId(string employeeIds,int companyId);
        Task<List<DropdownProjectManager>> GetByEmployeeIds(string employeeIds,int companyId);
        Task<ViewProjectDetails> GetprojectByProjectId(int ProjectId,int companyId);
        Task<List<SkillSet>> GetAllSkills(int companyId);
        Task<int> CreateProjectAssignation(ProjectAssignation projectAssignation, int sessionEmployeeId, int companyId);
        Task<int> DeleteProjectAssignation(ProjectAssignation projectAssignation, int sessionEmployeeId,int companyId);
        Task<List<ProjectDetails>> GetProjectDetails(int companyId);
        Task<ProjectAssignation> GetByProjectAssignationId(int id,int companyId);
        Task<List<CurrencyViewModel>> GetAllCurrency();
        Task<int> GetAllProjectsCount(SysDataTablePager pager,int companyId);
        Task<List<ProjectAssignationViewModel>> GetAllProjectAssignation(int companyId);
        Task<List<ProjectAssignationViewModel>> GetProjectAssignationImage(int projectId, int companyId);
        Task<List<DropdownEmployee>> GetAllEmployees(int companyId);
        Task<int> UpdateProjectAssignation(ProjectAssignation projectAssignation, int sessionEmployeeId, int companyId);

    }
}
