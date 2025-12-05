using EmployeeInformations.Business.IService;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ProjectSummaryViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{

    public class ProjectDetailsController : BaseController
    {

        private readonly IProjectDetailsService _projectDetailsService;

        public ProjectDetailsController(IProjectDetailsService projectDetailsService)
        {
            this._projectDetailsService = projectDetailsService;
        }

        /// <summary>
        /// Logic to view all the project list
        /// </summary>
        
        [HttpGet]
        public async Task<IActionResult> Project()
        {
            return View();
        }

        /// <summary>
        /// Logic to get all the project list
        /// </summary>
        /// /// <param name="pager, columnName, columnDirection" >project</param>
        [HttpGet]
        public async Task<IActionResult> GetProject(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            var project = await _projectDetailsService.GetAllProjectDetails(pager, columnName, columnDirection,companyId);
            var cntProject = await _projectDetailsService.GetAllProjectsCount(pager,companyId);
            return Json(new
            {
                data = project,
                iTotalRecords = cntProject,
                iTotalDisplayRecords = cntProject
            });
        }

        /// <summary>
        /// Logic to get edit the project detail  by particular project
        /// </summary>
        /// <param name="ProjectId" >project</param>
        [HttpGet]
        public IActionResult EditProject(int ProjectId)
        {
            var project = new ProjectDetails();
            project.ProjectId = ProjectId;
            return PartialView("EditProject", project);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var projects = new ProjectDetails();
            var projectRefNumber = await _projectDetailsService.GetProjectId(companyId);
            projects.ProjectRefNumber = projectRefNumber;
            projects.EmpId = sessionEmployeeId;
            projects.ProjectTypes = await _projectDetailsService.GetAllProjectTypes(companyId);
            projects.ClientCompany = await _projectDetailsService.GetAllClientCompany(companyId);
            projects.SkillSets = await _projectDetailsService.GetAllSkills(companyId);
            projects.Currency = await _projectDetailsService.GetAllCurrency();
            return View(projects);
        }

        /// <summary>
        /// Logic to get create project detail  
        /// </summary>
        /// <param name="projectDetails" ></param>
        [HttpPost]
        public async Task<int> AddProject(ProjectDetails projectDetails)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = await _projectDetailsService.CreateProject(projectDetails, sessionEmployeeId,companyId);
            return result;
        }

        [HttpPost]
        public async Task<Int32> UpdateProject(ProjectDetails projectDetails)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = await _projectDetailsService.CreateProject(projectDetails, sessionEmployeeId,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get update project detail by particular project 
        /// </summary>
        /// <param name="ProjectId" >project</param>
        [HttpGet]
        public async Task<IActionResult> UpdateProject(int ProjectId)
        {
            var companyId = GetSessionValueForCompanyId;
            var projects = await _projectDetailsService.GetByProjectId(ProjectId,companyId);
            projects.ProjectTypes = await _projectDetailsService.GetAllProjectTypes(companyId);
            projects.ClientCompany = await _projectDetailsService.GetAllClientCompany(companyId);
            projects.SkillSets = await _projectDetailsService.GetAllSkills(companyId);
            projects.Currency = await _projectDetailsService.GetAllCurrency();
            return PartialView("UpdateProject", projects);
        }

        [HttpGet]
        public async Task<IActionResult> GetByProjectTypeId(int ProjectTypeId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _projectDetailsService.GetByProjectTypeId(ProjectTypeId, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get soft deleted the project detail  by particular project
        /// </summary>
        /// <param name="ProjectId" >project</param>
        [HttpPost]
        public async Task<IActionResult> DeleteProject(int ProjectId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _projectDetailsService.DeleteProject(ProjectId,companyId);
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByEmployeeId(string employeeIds)
        {
            var companyId = GetSessionValueForCompanyId;
            if (string.IsNullOrEmpty(employeeIds))
            {
                var employees = await _projectDetailsService.GetAllEmployees(companyId);
                return new JsonResult(employees);
            }
            var result = await _projectDetailsService.GetByEmployeeId(employeeIds, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        ///  Logic to get dispaly the project detail  by particular project
        /// </summary>
        /// <param name="ProjectId" >project</param>
        [HttpGet]
        public async Task<IActionResult> ViewProject(int ProjectId)
        {
            var companyId = GetSessionValueForCompanyId;
            var projects = await _projectDetailsService.GetprojectByProjectId(ProjectId, companyId);
            return PartialView("ViewProject", projects);
        }

        //project assignation

        /// <summary>
        /// Logic to get all the projectassignation list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Assignation()
        {
            var companyId = GetSessionValueForCompanyId;
            var projectAssignation = await _projectDetailsService.GetAllProjectAssignation(companyId);
            return View(projectAssignation);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProjectAssignation()
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var projectAssignation = new ProjectAssignation();
            projectAssignation.EmpId = sessionEmployeeId;
            projectAssignation.ProjectDetails = await _projectDetailsService.GetProjectDetails(companyId);
            projectAssignation.DropdownEmployee = await _projectDetailsService.GetAllEmployees(companyId);
            projectAssignation.DropdownTeamLead = new List<DropdownTeamLead>();
            projectAssignation.DropdownProjectManager = new List<DropdownProjectManager>();
            return View(projectAssignation);
        }

        /// <summary>
        ///  Logic to get create the projectassignation detail  by particular projectassignation
        /// </summary>
        /// <param name="projectAssignation" ></param>
        [HttpPost]
        public async Task<int> AddProjectAssignation(ProjectAssignation projectAssignation)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _projectDetailsService.CreateProjectAssignation(projectAssignation, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get update the projectassignation detail  by particular projectassignation
        /// </summary>
        /// <param name="projectId" >projectassignation</param>
        [HttpGet]
        public async Task<IActionResult> UpdateProjectAssignation(int projectId)
        {
            var companyId = GetSessionValueForCompanyId;
            var projectAssignation = await _projectDetailsService.GetByProjectAssignationId(projectId, companyId);
            projectAssignation.ProjectDetails = await _projectDetailsService.GetProjectDetails(companyId);
            projectAssignation.projectAssignationViewModels = await _projectDetailsService.GetProjectAssignationImage(projectId, companyId);
            projectAssignation.ProjectId = projectId;
            return PartialView("UpdateProjectAssignation", projectAssignation);
        }


        /// <summary>
        /// Logic to get edit the projectassignation detail  by particular projectassignation
        /// </summary>
        /// <param name="ProjectId" >projectassignation</param>
        [HttpGet]
        public async Task<IActionResult> EditProjectAssignation(int projectId)
        {
            var projectAssignation = new ProjectAssignation();
            projectAssignation.ProjectId = projectId;
            return PartialView("EditProjectAssignation", projectAssignation);
        }

        [HttpPost]
        public async Task<int> UpdateProjectAssignation(ProjectAssignation projectAssignation)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _projectDetailsService.UpdateProjectAssignation(projectAssignation, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get soft delete the projectassignation detail  by particular projectassignation
        /// </summary>
        /// <param name="projectAssignation" ></param>
        [HttpPost]
        public async Task<int> DeleteProjectAssignation(ProjectAssignation projectAssignation)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = await _projectDetailsService.DeleteProjectAssignation(projectAssignation, sessionEmployeeId, companyId);
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> GetProjectAssignation(ProjectAssignationViewModel projectAssignationViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var projectAssignations = await _projectDetailsService.GetProjectAssignationImage(projectAssignationViewModel.ProjectId, companyId);
            return new JsonResult(projectAssignations);
        }
    }
}
