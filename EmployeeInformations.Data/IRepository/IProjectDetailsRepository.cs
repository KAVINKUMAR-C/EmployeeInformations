using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ProjectSummaryViewModel;
using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IProjectDetailsRepository
    {
        Task<List<ProjectDetailsEntity>> GetAllProjectDetails(int companyId);
        Task<List<ProjectDetails>> GetAllProjectDetail(int companyId);
        Task<List<ProjectDetailsModel>> GetProjectDetail(SysDataTablePager pager, string columnName, string columnDirection, int companyId);
        Task<List<ProjectDetailsEntity>> GetAllProjectDetailsId(int empId,int companyId);
        Task<ProjectDetailsEntity> GetByProjectId(int ProjectId,int companyId);
        Task<int> CreateProject(ProjectDetailsEntity projectDetailsEntity,int companyId);
        Task<List<ProjectTypesEntity>> GetAllProjectTypes(int companyId);
        Task<bool> DeleteProject(int ProjectId,int companyId);
        //Task<List<ProjectDetailsEntity>> GetAllProjectDetailsIsDeleted();
        Task<int> GetProjectMaxCount(int companyId);
        Task<List<ClientEntity>> GetAllClientCompany(int companyId);
        Task<List<EmployeesEntity>> GetAllEmployees(int companyId);
        Task<List<int>> GetByEmployeeId(int employeeIds,int companyId);
        Task<List<int>> GetByEmployeeIdForProject(int employeeIds,int companyId);
        Task<List<SkillSetEntity>> GetAllSkillSet(int companyId);
        Task<ProjectTypesEntity> GetByProjectType(int ProjectTypeId,int companyId);
        Task<List<ProjectAssignationEntity>> GetAllProjectAssignation(int companyId);
        Task<List<ProjectAssignationEntity>> GetByProjectAssignationId(int ProjectId,int companyId);
        Task<bool> CreateProjectAssignation(List<ProjectAssignationEntity> projectAssignationEntity,int companyId);
        Task<bool> UpdateProjectAssignation(List<ProjectAssignationEntity> projectAssignationEntity, int projectId,int companyId);
        Task<bool> DeleteProjectAssignation(List<ProjectAssignationEntity> projectAssignationEntity,int companyId);
        Task<List<ProjectAssignationEntity>> GetEmployeeByProjectId(int projectId,int companyId);
        Task<int> GetEmployeeCountByProjectId(int projectId,int companyId);
        Task<List<ProjectAssignationEntity>> GetByProjectAssignationByEmpId(int empId,int companyId);
        Task<ProjectDetailsEntity> GetProjectByCompnayId(int projectId, int companyId);
        Task<List<CurrencyEntity>> GetAllCurrency();
        Task<List<ProjectNames>> GetProjectByEmpId(int empId, int companyId);
        Task<int> GetAllProjectsCount(SysDataTablePager pager,int companyId);

    }
}
