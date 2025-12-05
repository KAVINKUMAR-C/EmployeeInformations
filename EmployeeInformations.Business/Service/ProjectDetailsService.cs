using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ProjectSummaryViewModel;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace EmployeeInformations.Business.Service
{
    public class ProjectDetailsService : IProjectDetailsService
    {
        private readonly IProjectDetailsRepository _projectDetailsRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly ICompanyRepository _companyRepository;


        public ProjectDetailsService(IProjectDetailsRepository projectDetailsRepository, IMapper mapper, IConfiguration config, IEmployeesRepository employeesRepository, IClientRepository clientRepository, IEmailDraftRepository emailDraftRepository, ICompanyRepository companyRepository)
        {
            _projectDetailsRepository = projectDetailsRepository;
            _mapper = mapper;
            _config = config;
            _employeesRepository = employeesRepository;
            _clientRepository = clientRepository;
            _emailDraftRepository = emailDraftRepository;
            _companyRepository = companyRepository;
        }

        //ProjectDetails

        /// <summary>
        /// Logic to get projectdetails list
        /// </summary>  
        /// <param name="pager, columnName, columnDirection" ></param>
        public async Task<List<ProjectDetailsModel>> GetAllProjectDetails(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {                         
            var listOfProject = await _projectDetailsRepository.GetProjectDetail(pager, columnName, columnDirection,companyId);           
            return listOfProject;          
        }

        /// <summary>
        /// Logic to get comma separator the skill set
        /// </summary>  
        /// <param name="technologys" ></param>
        /// <param name="skillSets" ></param>
        public string GetTechnologyNameById(string technologys, List<SkillSetEntity> skillSets)
        {
            string technologyNames = string.Empty;
            if (!string.IsNullOrEmpty(technologys))
            {
                var splitEmpId = technologys.Split(',');
                foreach (var data in splitEmpId)
                {
                    var technology = Convert.ToInt32(data);
                    var value = skillSets.FirstOrDefault(r => r.SkillId == technology);
                    if (value != null)
                    {
                        technologyNames += value.SkillName + ",";
                    }
                }
            }
            return technologyNames.Trim(new char[] { ',' });
        }

        /// <summary>
        /// Logic to get projecttypes the list
        /// </summary>  
        public async Task<List<ProjectTypes>> GetAllProjectTypes(int companyId)
        {
            var listOfProjectTypes = new List<ProjectTypes>();
            var listProjectTypes = await _projectDetailsRepository.GetAllProjectTypes(companyId);
            if (listProjectTypes != null)
            {
                listOfProjectTypes = _mapper.Map<List<ProjectTypes>>(listProjectTypes);
            }
            return listOfProjectTypes;
        }

        /// <summary>
        /// Logic to get projectdetails the list
        /// </summary>  
        public async Task<List<ProjectDetails>> GetProjectDetails(int companyId)
        {
            var listOfProjectDetails = new List<ProjectDetails>();
            var listProjectDetails = await _projectDetailsRepository.GetAllProjectDetails(companyId);
            if (listProjectDetails != null)
            {
                listOfProjectDetails = _mapper.Map<List<ProjectDetails>>(listProjectDetails);
            }
            return listOfProjectDetails;
        }

        /// <summary>
        /// Logic to get all employees profile images
        /// </summary>  
        public List<EmployeeProfileImageName> GetByEmployeeProfileImageName(string employeeIds, List<EmployeesEntity> employeesEntitys, List<ProfileInfoEntity> profileInfoEntitys)
        {
            var profileiamge = new List<int>();
            var splitEmpId = employeeIds.Split(',');
            foreach (var item in splitEmpId)
            {
                var empId = Convert.ToInt32(item);
                var value = employeesEntitys.Where(r => r.EmpId == empId).FirstOrDefault();
                if (value != null)
                    profileiamge.Add(value.EmpId);
            }

            var employeeNameImage = new List<EmployeeProfileImageName>();
            if (!string.IsNullOrEmpty(employeeIds))
            {
                var count = 1;
                foreach (var empId in profileiamge)
                {
                    count = count > 5 ? 1 : count;
                    var employeeNameImages = new EmployeeProfileImageName();
                    var empEntity = employeesEntitys.Where(r => r.EmpId == empId).FirstOrDefault();
                    var profileEntity = profileInfoEntitys.FirstOrDefault(d => d.EmpId == empId);
                    if (empEntity != null)
                    {
                        employeeNameImages.EmpId = empEntity.EmpId;
                        employeeNameImages.UserName = empEntity.FirstName + "  " + empEntity.LastName;
                        employeeNameImages.SortUserName = empEntity.FirstName?.Substring(0, 1) + "" + empEntity.LastName?.Substring(0, 1);
                    }
                    employeeNameImages.EmployeeProfileImage = profileEntity != null ? profileEntity.ProfileName : string.Empty;
                    employeeNameImages.ClassName = Common.Common.GetClassNameForGrid(count);
                    if (!employeeNameImage.Any(j => j.EmpId == employeeNameImages.EmpId))
                    {
                        employeeNameImage.Add(employeeNameImages);
                    }
                    count++;
                }
            }
            return employeeNameImage;
        }

        /// <summary>
        /// Logic to get employee details the dropdwon for teamlead  by particular employeeIds
        /// </summary>  
        /// <param name="employeeIds" ></param>
        public async Task<List<DropdownTeamLead>> GetByEmployeeId(string employeeIds, int companyId)
        {
            var reportingEmpIds = new List<int>();
            // count of id
            var splitEmpId = employeeIds.Split(',');
            foreach (var item in splitEmpId)
            {
                var value = Convert.ToInt32(item);
                var ids = await _projectDetailsRepository.GetByEmployeeId(value, companyId);
                if (ids != null)
                    reportingEmpIds.AddRange(ids);
            }

            // get id ,name
            var dropdownTeamLeads = new List<DropdownTeamLead>();
            foreach (var emp in reportingEmpIds)
            {
                var dropdownTeamLead = new DropdownTeamLead();
                var employee = await _employeesRepository.GetEmployeeById(emp, companyId);
                if (employee != null)
                {
                    dropdownTeamLead.EmpId = employee.EmpId;
                    dropdownTeamLead.UserName = employee.UserName + " " + "-" + " " + employee.FirstName + "  " + employee.LastName;
                    if (!dropdownTeamLeads.Any(x => x.EmpId == dropdownTeamLead.EmpId))
                    {
                        dropdownTeamLeads.Add(dropdownTeamLead);
                    }
                }
            }

            return dropdownTeamLeads;
        }

        /// <summary>
        /// Logic to get employee details the dropdwon for projectmanager  by particular employeeIds
        /// </summary>  
        /// <param name="employeeIds" ></param>
        public async Task<List<DropdownProjectManager>> GetByEmployeeIds(string employeeIds, int companyId)
        {
            var reportingEmpIds = new List<int>();
            // count of id
            var splitEmpId = employeeIds.Split(',');
            foreach (var item in splitEmpId)
            {
                var value = Convert.ToInt32(item);
                var ids = await _projectDetailsRepository.GetByEmployeeId(value, companyId);
                if (ids != null)
                    reportingEmpIds.AddRange(ids);
            }
            // get id ,name
            var dropdownProjectManagers = new List<DropdownProjectManager>();
            foreach (var emp in reportingEmpIds)
            {
                var dropdownProjectManager = new DropdownProjectManager();
                var employee = await _employeesRepository.GetEmployeeById(emp, companyId);
                if (employee != null)
                {
                    dropdownProjectManager.EmpId = employee.EmpId;
                    dropdownProjectManager.UserName = employee.UserName + " " + "-" + " " + employee.FirstName + "  " + employee.LastName;
                    if (!dropdownProjectManagers.Any(y => y.EmpId == dropdownProjectManager.EmpId))
                    {
                        dropdownProjectManagers.Add(dropdownProjectManager);
                    }
                }
            }
            return dropdownProjectManagers;
        }

        /// <summary>
        /// Logic to get employee details the list
        /// </summary>         
        public async Task<List<DropdownEmployee>> GetAllEmployees(int companyId)
        {
            var listOfEmployees = new List<DropdownEmployee>();
            var listEmployees = await _projectDetailsRepository.GetAllEmployees(companyId);
            var allProfileImages = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var employees = _mapper.Map<List<DropdownEmployee>>(listEmployees);
            foreach (var item in employees)
            {
                var profileEntity = allProfileImages.FirstOrDefault(d => d.EmpId == item.EmpId);
                listOfEmployees.Add(new DropdownEmployee()
                {                      
                EmpId = item.EmpId,
                    FirstName = item?.UserName + " " + "-" + " " + item?.FirstName + " " + item?.LastName,
                });

            }
            return listOfEmployees;
        }

        /// <summary>
        /// Logic to get employee details the list for projectmanager
        /// </summary>     
        public async Task<List<DropdownProjectManagers>> GetAllProjectmanagers(int companyId)
        {
            var listOfEmployees = new List<DropdownProjectManagers>();
            var listEmployees = await _projectDetailsRepository.GetAllEmployees(companyId);
            var employees = _mapper.Map<List<DropdownProjectManagers>>(listEmployees);
            foreach (var item in employees)
            {
                listOfEmployees.Add(new DropdownProjectManagers()
                {
                    EmpId = item.EmpId,
                    UserName = item?.UserName + " " + "-" + " " + item?.FirstName + " " + item?.LastName,
                });
            }
            return listOfEmployees;
        }

        /// <summary>
        /// Logic to get employee details the list for teamlead
        /// </summary>   
        public async Task<List<DropdownTeamLeads>> GetAllTeamLeads(int companyId)
        {
            var listOfEmployees = new List<DropdownTeamLeads>();
            var listEmployees = await _projectDetailsRepository.GetAllEmployees(companyId);
            var employees = _mapper.Map<List<DropdownTeamLeads>>(listEmployees);
            foreach (var item in employees)
            {
                listOfEmployees.Add(new DropdownTeamLeads()
                {
                    EmpId = item.EmpId,
                    UserName = item?.UserName + " " + "-" + " " + item?.FirstName + " " + item?.LastName,
                });
            }
            return listOfEmployees;
        }

        /// <summary>
        /// Logic to get projectdetails the list 
        /// </summary>   
        public async Task<List<ProjectDetails>> GetProjectDetailsId(int companyId)
        {
            var listOfProjectDetailsId = new List<ProjectDetails>();
            var listProjectDetailsId = await _projectDetailsRepository.GetAllProjectDetails(companyId);
            var projectDetails = _mapper.Map<List<ProjectDetails>>(listProjectDetailsId);
            foreach (var item in projectDetails)
            {
                listOfProjectDetailsId.Add(new ProjectDetails()
                {
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName,
                });
            }
            return listOfProjectDetailsId;
        }

        /// <summary>
        /// Logic to get the project details by particular ProjectId
        /// </summary> 
        /// <param name="ProjectId" ></param>  
        public async Task<ProjectDetails> GetByProjectId(int ProjectId, int companyId)
        {
            var projectDetails = new ProjectDetails();
            var detalis = await _projectDetailsRepository.GetByProjectId(ProjectId, companyId);

            if (detalis != null)
            {
                projectDetails = _mapper.Map<ProjectDetails>(detalis);
            }
            return projectDetails;
        }

        /// <summary>
        /// Logic to get the projecttype details by particular ProjectTypeId
        /// </summary> 
        /// <param name="ProjectTypeId" ></param>  
        public async Task<List<ProjectTypes>> GetByProjectTypeId(int ProjectTypeId, int companyId)
        {
            var listOfProjectTypes = new List<ProjectTypes>();
            var listProjectTypes = await _projectDetailsRepository.GetAllProjectTypes(companyId);
            if (listProjectTypes != null)
            {
                listOfProjectTypes = _mapper.Map<List<ProjectTypes>>(listProjectTypes);
            }
            return listOfProjectTypes;
        }

        /// <summary>
        /// Logic to get the client details list
        /// </summary> 
        public async Task<List<ClientCompanys>> GetAllClientCompany(int companyId)
        {
            var listofClientCompany = new List<ClientCompanys>();
            var listofClient = await _projectDetailsRepository.GetAllClientCompany(companyId);
            if (listofClient != null)
            {
                listofClientCompany = _mapper.Map<List<ClientCompanys>>(listofClient);
            }
            return listofClientCompany;
        }

        /// <summary>
        /// Logic to get the skillset details list
        /// </summary> 
        public async Task<List<SkillSet>> GetAllSkills(int companyId)
        {
            var listOfSkill = new List<SkillSet>();
            var listSkills = await _employeesRepository.GetAllSkills(companyId);
            if (listSkills != null)
            {
                listOfSkill = _mapper.Map<List<SkillSet>>(listSkills);
            }

            return listOfSkill;
        }

        /// <summary>
        /// Logic to get create and update the projectdetails 
        /// </summary> 
        /// <param name="projectDetails" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> CreateProject(ProjectDetails projectDetails, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (projectDetails != null)
            {
                if (projectDetails.ProjectId == 0)
                {
                    projectDetails.CreatedBy = sessionEmployeeId;
                    projectDetails.CreatedDate = DateTime.Now;

                    if (projectDetails.ProjectTypeId == 1)
                    {
                        projectDetails.Startdate = DateTimeExtensions.ConvertToDatetime(projectDetails.StrStartdate);
                        projectDetails.Enddate = DateTimeExtensions.ConvertToDatetime(projectDetails.StrEnddate);
                    }
                    else if (projectDetails.ProjectTypeId == 2 || projectDetails.ProjectTypeId == 3)
                    {
                        projectDetails.Startdate = DateTimeExtensions.ConvertToDatetime(projectDetails.StrStartdate);
                        projectDetails.Enddate = DateTime.Now;
                    }
                    else
                    {
                        projectDetails.Startdate = DateTime.Now;
                        projectDetails.Enddate = DateTime.Now;
                    }
                    var projectDetailsEntity = _mapper.Map<ProjectDetailsEntity>(projectDetails);
                    var datas = await _projectDetailsRepository.CreateProject(projectDetailsEntity,companyId);
                    result = projectDetailsEntity.ProjectId;
                }
                else
                {
                    var projectDetailsEntity = await _projectDetailsRepository.GetByProjectId(projectDetails.ProjectId, companyId);
                    projectDetails.CreatedBy = projectDetailsEntity.CreatedBy;
                    projectDetails.CreatedDate = projectDetailsEntity.CreatedDate;
                    projectDetails.ProjectRefNumber = projectDetailsEntity.ProjectRefNumber;
                    projectDetails.UpdatedBy = sessionEmployeeId;
                    projectDetails.UpdatedDate = DateTime.Now;
                    if (projectDetails.ProjectTypeId == 1)
                    {
                        projectDetails.Startdate = DateTimeExtensions.ConvertToDatetime(projectDetails.StrStartdate);
                        projectDetails.Enddate = DateTimeExtensions.ConvertToDatetime(projectDetails.StrEnddate);
                    }
                    else if (projectDetails.ProjectTypeId == 2 || projectDetails.ProjectTypeId == 3)
                    {
                        projectDetails.Startdate = DateTimeExtensions.ConvertToDatetime(projectDetails.StrStartdate);
                        projectDetails.Enddate = DateTime.Now;
                    }
                    else
                    {
                        projectDetails.Startdate = DateTime.Now;
                        projectDetails.Enddate = DateTime.Now;
                    }
                    var strSikillName = projectDetails.Technology;
                    if (strSikillName.Count() > 0)
                    {
                        var str = string.Join(",", strSikillName);
                        projectDetailsEntity.Technology = str.TrimEnd(',');
                    }
                    var mapprojectDetailsEntity = _mapper.Map<ProjectDetailsEntity>(projectDetails);
                    var data = await _projectDetailsRepository.CreateProject(mapprojectDetailsEntity, companyId);
                    result = data;
                }
            }

            return result;
        }

        /// <summary>
        /// Logic to get remove the project get email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task RemoveProjectEmailEmployee(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.ProjectRejection;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
        /// Logic to get the project details by particular ProjectId
        /// </summary> 
        /// <param name="ProjectId" ></param> 
        public async Task<bool> DeleteProject(int ProjectId,int companyId)
        {
            var result = await _projectDetailsRepository.DeleteProject(ProjectId,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get RFPN number the project details 
        /// </summary>       
        public async Task<string> GetProjectId(int companyId)
        {
            var totalProjectCount = await _projectDetailsRepository.GetProjectMaxCount(companyId);
            var projectRefNumber = "RFPN-" + (totalProjectCount + 1).ToString("D3");
            return projectRefNumber;
        }

        /// <summary>
        ///  Logic to get dispaly the project detail by particular project
        /// </summary>
        /// <param name="ProjectId" ></param>
        public async Task<ViewProjectDetails> GetprojectByProjectId(int projectId, int companyId)
        {
            var viewProjectDetails = new ViewProjectDetails();
            var projectDetailsEntity = await _projectDetailsRepository.GetByProjectId(projectId, companyId);
            if (projectDetailsEntity != null)
            {
                var cliententity = await _clientRepository.GetByClientId(Convert.ToInt32(projectDetailsEntity.ClientCompanyId),companyId);
                var projectType = await _projectDetailsRepository.GetByProjectType(projectDetailsEntity.ProjectTypeId, companyId);
                var skillSets = await _employeesRepository.GetAllSkills(companyId);               
                viewProjectDetails.ClientCompanyName = cliententity.ClientCompany;
                viewProjectDetails.ProjectName = projectDetailsEntity.ProjectName;
                viewProjectDetails.ProjectTypeName = projectType.ProjectTypeName;
                viewProjectDetails.Technology = !string.IsNullOrEmpty(projectDetailsEntity.Technology) ? GetTechnologyNameById(projectDetailsEntity.Technology, skillSets) : string.Empty;
                viewProjectDetails.Hours = Convert.ToInt32(projectDetailsEntity.Hours);
                viewProjectDetails.Startdate = Convert.ToDateTime(projectDetailsEntity.Startdate);
                viewProjectDetails.Enddate = Convert.ToDateTime(projectDetailsEntity.Enddate);
                viewProjectDetails.ProjectCost = projectDetailsEntity.ProjectCost;
                viewProjectDetails.CurrencyCode = projectDetailsEntity.CurrencyCode; 
                viewProjectDetails.ProjectDescription = !string.IsNullOrEmpty(projectDetailsEntity.ProjectDescription) ? projectDetailsEntity.ProjectDescription : string.Empty;
            }
            return viewProjectDetails;
        }

        //projectassignation

        /// <summary>
        ///  Logic to get the projectassignation detail list
        /// </summary>
        public async Task<List<ProjectAssignationViewModel>> GetAllProjectAssignation(int companyId)
        {
            var projectAssignationViewModels = new List<ProjectAssignationViewModel>();

            var listProjectAssignationEntity = await _projectDetailsRepository.GetAllProjectAssignation(companyId);
            var groupByProjectId = listProjectAssignationEntity.GroupBy(x => x.ProjectId).ToList();

            var allProjectTypes = await _projectDetailsRepository.GetAllProjectDetails(companyId);
            var allemployee = await _employeesRepository.GetAllEmployees(companyId);
            var allProfileImages = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var departmentEntities = await _employeesRepository.GetAllDepartment(companyId);
            var desigantionEntities = await _employeesRepository.GetAllDesignation(companyId);
            var count = 1;
            foreach (var item in groupByProjectId)
            {
                count = count > 5 ? 1 : count;
                var projectAssignationViewModel = new ProjectAssignationViewModel();
                var projectEntity = allProjectTypes.FirstOrDefault(x => x.ProjectId == item.Key);

                var projectManagerByType = item.Where(x => x.Type == 3).ToList();
                var teamLeadByType = item.Where(x => x.Type == 2).ToList();
                var employeeByType = item.Where(x => x.Type == 1).ToList();
                var projectManagerProfileImageNames = new List<ProjectManagerProfileImageNames>();
                GetProjectManagerDetailByType(allemployee, allProfileImages, projectManagerByType, projectManagerProfileImageNames, departmentEntities, desigantionEntities);
                var teamLeadProfileImageNames = new List<TeamLeadProfileImageNames>();
                GetTeamLeadDetailByType(allemployee, allProfileImages, teamLeadByType, teamLeadProfileImageNames, departmentEntities, desigantionEntities);
                var employeeProfileImageNames = new List<EmployeeProfileImageNames>();
                GetEmployeeDetailByType(allemployee, allProfileImages, employeeByType, employeeProfileImageNames, departmentEntities, desigantionEntities);
                projectAssignationViewModel.ClassName = Common.Common.GetClassNameForGrid(count);
                projectAssignationViewModel.ProjectName = projectEntity != null ? projectEntity.ProjectName : null;
                projectAssignationViewModel.ProjectId = item.Key;
                projectAssignationViewModel.ProjectManagerProfileImageNames = projectManagerProfileImageNames.DistinctBy(y=>y.UserName).ToList();
                projectAssignationViewModel.TeamLeadProfileImageNames = teamLeadProfileImageNames.DistinctBy(y => y.UserName).ToList();
                projectAssignationViewModel.EmployeeProfileImageNames = employeeProfileImageNames.DistinctBy(y => y.UserName).ToList();
                projectAssignationViewModels.Add(projectAssignationViewModel);
                count++;
            }

              return projectAssignationViewModels;
        }

        /// <summary>
        ///  Logic to get the projectassignation detail by particular projectId
        /// </summary>
        /// <param name="ProjectId" ></param>
        public async Task<List<ProjectAssignationViewModel>> GetProjectAssignationImage(int projectId,int companyId)
        {
            var projectAssignationViewModels = new List<ProjectAssignationViewModel>();

            var listProjectAssignationEntity = await _projectDetailsRepository.GetByProjectAssignationId(projectId, companyId);
            var groupByProjectId = listProjectAssignationEntity.GroupBy(x => x.ProjectId).ToList();

            var allProjectTypes = await _projectDetailsRepository.GetAllProjectDetails(companyId);
            var allemployee = await _employeesRepository.GetAllEmployees(companyId);
            var allProfileImages = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var departmentEntities = await _employeesRepository.GetAllDepartment(companyId);
            var desigantionEntities = await _employeesRepository.GetAllDesignation(companyId);
            var count = 1;
            foreach (var item in groupByProjectId)
            {
                count = count > 5 ? 1 : count;
                var projectAssignationViewModel = new ProjectAssignationViewModel();
                var projectEntity = allProjectTypes.FirstOrDefault(x => x.ProjectId == item.Key);

                var projectManagerByType = item.Where(x => x.Type == 3).DistinctBy(x=>x.EmployeeId).ToList();
                var teamLeadByType = item.Where(x => x.Type == 2).DistinctBy(x=>x.EmployeeId).ToList();
                var employeeByType = item.Where(x => x.Type == 1).DistinctBy(x=> x.EmployeeId).ToList();
                var projectManagerProfileImageNames = new List<ProjectManagerProfileImageNames>();
                GetProjectManagerDetailByType(allemployee, allProfileImages, projectManagerByType, projectManagerProfileImageNames, departmentEntities, desigantionEntities);
                var teamLeadProfileImageNames = new List<TeamLeadProfileImageNames>();
                GetTeamLeadDetailByType(allemployee, allProfileImages, teamLeadByType, teamLeadProfileImageNames, departmentEntities, desigantionEntities);
                var employeeProfileImageNames = new List<EmployeeProfileImageNames>();
                GetEmployeeDetailByType(allemployee, allProfileImages, employeeByType, employeeProfileImageNames, departmentEntities, desigantionEntities);
                projectAssignationViewModel.ClassName = Common.Common.GetClassNameForGrid(count);
                projectAssignationViewModel.ProjectName = projectEntity != null ? projectEntity.ProjectName : string.Empty;
                projectAssignationViewModel.ProjectId = item.Key;
                projectAssignationViewModel.ProjectManagerProfileImageNames = projectManagerProfileImageNames;
                projectAssignationViewModel.TeamLeadProfileImageNames = teamLeadProfileImageNames;
                projectAssignationViewModel.EmployeeProfileImageNames = employeeProfileImageNames;
                projectAssignationViewModels.Add(projectAssignationViewModel);
                count++;
            }

            return projectAssignationViewModels;
        }



        /// <summary>
        ///  Logic to get the employee name and image for projectmanager
        /// </summary>
        /// <param name="allemployee" ></param>
        /// <param name="allProfileImages" ></param>
        /// <param name="projectManagerByType" ></param>
        /// <param name="projectManagerProfileImageNames" ></param>
        private static async void GetProjectManagerDetailByType(List<EmployeesEntity> allemployee, List<ProfileInfoEntity> allProfileImages, List<ProjectAssignationEntity> projectManagerByType, List<ProjectManagerProfileImageNames> projectManagerProfileImageNames,List<DepartmentEntity> departmentEntities,List<DesignationEntity> desigantionEntities)
        {
            var count = 1;
            foreach (var pm in projectManagerByType)
            {
                count = count > 5 ? 1 : count;
                var projectManagerProfileImageName = new ProjectManagerProfileImageNames();
                var employee = allemployee.FirstOrDefault(x => x.EmpId == pm.EmployeeId);
                
                if (employee != null)
                {
                    projectManagerProfileImageName.UserName = employee.FirstName + " " + employee.LastName;
                    projectManagerProfileImageName.SortUserName = employee.FirstName?.Substring(0, 1) + "" + employee.LastName.Substring(0, 1);
                    projectManagerProfileImageName.ClassName = Common.Common.GetClassNameForGrid(count);
                   
                }
                var profileEntity = allProfileImages.FirstOrDefault(x => x.EmpId == pm.EmployeeId);
                projectManagerProfileImageName.EmployeeProfileImage = profileEntity != null ? profileEntity.ProfileName : string.Empty;
                var departmentName = departmentEntities.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId);
                projectManagerProfileImageName.Department = departmentName != null? departmentName.DepartmentName : string.Empty;
                var designationName = desigantionEntities.FirstOrDefault(x =>x.DesignationId == employee.DesignationId);
                projectManagerProfileImageName.Designation = designationName != null ? designationName.DesignationName : string.Empty;
                projectManagerProfileImageNames.Add(projectManagerProfileImageName);
            }
        }

        /// <summary>
        ///  Logic to get the employee name and image for teamlead
        /// </summary>
        /// <param name="allemployee" ></param>
        /// <param name="allProfileImages" ></param>
        /// <param name="teamLeadByType" ></param>
        /// <param name="teamLeadProfileImageNames" ></param>
        private static void GetTeamLeadDetailByType(List<EmployeesEntity> allemployee, List<ProfileInfoEntity> allProfileImages, List<ProjectAssignationEntity> teamLeadByType, List<TeamLeadProfileImageNames> teamLeadProfileImageNames,List<DepartmentEntity> departmentEntities,List<DesignationEntity> desigantionEntities)
        {
            var count = 1;
            foreach (var pm in teamLeadByType)
            {
                count = count > 5 ? 1 : count;
                var teamLeadProfileImageName = new TeamLeadProfileImageNames();
                var employee = allemployee.FirstOrDefault(x => x.EmpId == pm.EmployeeId);
                if (employee != null)
                {
                    teamLeadProfileImageName.UserName = employee.FirstName + " " + employee.LastName;
                    teamLeadProfileImageName.SortUserName = employee.FirstName.Substring(0, 1) + "" + employee.LastName.Substring(0, 1);
                    teamLeadProfileImageName.ClassName = Common.Common.GetClassNameForLeaveDashboard(count);
                }
                var profileEntity = allProfileImages.FirstOrDefault(x => x.EmpId == pm.EmployeeId);
                teamLeadProfileImageName.EmployeeProfileImage = profileEntity != null ? profileEntity.ProfileName : string.Empty;
                var departmentName = departmentEntities.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId);
                teamLeadProfileImageName.Department = departmentName != null ? departmentName.DepartmentName : string.Empty;
                var designationName = desigantionEntities.FirstOrDefault(x => x.DesignationId == employee.DesignationId);
                teamLeadProfileImageName.Designation = designationName != null ? designationName.DesignationName : string.Empty;
                teamLeadProfileImageNames.Add(teamLeadProfileImageName);
            }
        }

        /// <summary>
        ///  Logic to get the employee name and image for employees
        /// </summary>
        /// <param name="allemployee" ></param>
        /// <param name="allProfileImages" ></param>
        /// <param name="employeeByType" ></param>
        /// <param name="employeeProfileImageNames" ></param>
        private static void GetEmployeeDetailByType(List<EmployeesEntity> allemployee, List<ProfileInfoEntity> allProfileImages, List<ProjectAssignationEntity> employeeByType, List<EmployeeProfileImageNames> employeeProfileImageNames, List<DepartmentEntity> departmentEntities, List<DesignationEntity> desigantionEntities)
        {
            var count = 1;
            foreach (var pm in employeeByType)
            {
                count = count > 5 ? 1 : count;
                var employeeProfileImageName = new EmployeeProfileImageNames();
                var employee = allemployee.FirstOrDefault(x => x.EmpId == pm.EmployeeId);
                if (employee != null)
                {
                    employeeProfileImageName.UserName = employee.FirstName + " " + employee.LastName;
                    employeeProfileImageName.SortUserName = employee.FirstName.Substring(0, 1) + "" + employee.LastName.Substring(0, 1);
                    employeeProfileImageName.ClassName = Common.Common.GetClassNameForGrid(count);
                }

                var profileEntity = allProfileImages.FirstOrDefault(x => x.EmpId == pm.EmployeeId);
                employeeProfileImageName.EmployeeProfileImage = profileEntity != null ? profileEntity.ProfileName : string.Empty;
                var departmentName = departmentEntities.FirstOrDefault(x => x.DepartmentId == employee.DepartmentId);
                employeeProfileImageName.Department = departmentName != null ? departmentName.DepartmentName : string.Empty;
                var designationName = desigantionEntities.FirstOrDefault(x => x.DesignationId == employee.DesignationId);
                employeeProfileImageName.Designation = designationName != null ? designationName.DesignationName : string.Empty;
                employeeProfileImageNames.Add(employeeProfileImageName);
                count++;
            }
        }

        /// <summary>
        ///  Logic to get create the projectassignation details
        /// </summary>
        /// <param name="projectAssignation" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> CreateProjectAssignation(ProjectAssignation projectAssignation, int sessionEmployeeId,int companyId)
        {
            var result = 0;
            if (projectAssignation?.Id == 0)
            {
                List<ProjectAssignationEntity> projectAssignationEntitys = new List<ProjectAssignationEntity>();
                var getTeamleadId = projectAssignation.TeamLeadId.Split(',');
                var getEmployeeId = projectAssignation.EmployeeId.Split(',');
                var getManagerEmpIds = projectAssignation.ProjectManagerId.Split(',');
                var projectManagerAssignation = EmployeeAssignation(projectAssignation.ProjectId, getManagerEmpIds, sessionEmployeeId, 3);
                var teamLeadAssignation = EmployeeAssignation(projectAssignation.ProjectId, getTeamleadId, sessionEmployeeId, 2);
                var employeeAssignation = EmployeeAssignation(projectAssignation.ProjectId, getEmployeeId, sessionEmployeeId, 1);
                projectAssignationEntitys.AddRange(projectManagerAssignation);
                projectAssignationEntitys.AddRange(teamLeadAssignation);
                projectAssignationEntitys.AddRange(employeeAssignation);
                var datas = await _projectDetailsRepository.CreateProjectAssignation(projectAssignationEntitys, companyId);
                //var employeeId = await _projectDetailsRepository.GetByEmployeeId(projectAssignation.EmpId);//reporting persons 
                var draftTypeId = (int)EmailDraftType.ProjectAssignation;
                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                var project = await _projectDetailsRepository.GetByProjectId(projectAssignation.ProjectId, companyId);
                var projectType = await _projectDetailsRepository.GetByProjectType(projectAssignation.ProjectId, companyId);
                var employees = await _employeesRepository.GetAllEmployeeDetails(companyId);
                var projectManagerName = "";
                var projectStartDate = project.Startdate;
                if (projectManagerAssignation.Count() > 0)
                {
                    var strname = new List<string>();
                    foreach (var item in projectManagerAssignation)
                    {
                        var employeeName = employees.FirstOrDefault(x => x.EmpId == Convert.ToInt32(item.EmployeeId));
                        strname.Add(employeeName.FirstName + " " + employeeName.LastName);
                    }

                    var str = String.Join(",", strname);
                    projectManagerName = str;
                }
                foreach (var item in getEmployeeId)
                {
                    var employee = employees.Where(x => x.EmpId == Convert.ToInt32(item)).FirstOrDefault();
                    var assignedBy = employees.Where(x => x.EmpId == sessionEmployeeId).FirstOrDefault();
                    if (employee != null)
                    {
                        var bodyContent = EmailBodyContent.SendEmail_Body_ProjectAssignation(employee, project.ProjectName, projectType.ProjectTypeName, Convert.ToDateTime(project.Startdate), projectManagerName, assignedBy.FirstName + " " + assignedBy.LastName, emailDraftContentEntity.DraftBody);
                        await InsertEmailProjectAssignation(employee.OfficeEmail, emailDraftContentEntity, bodyContent);
                    }
                }
                result = Convert.ToInt32(datas);
            }

            return result;
        }

        /// <summary>
        /// Logic to get create and update the project get email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailProjectAssignation(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.ProjectAssignation;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
        ///  Logic to get update the projectassignation details
        /// </summary>
        /// <param name="projectAssignation" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> UpdateProjectAssignation(ProjectAssignation projectAssignation, int sessionEmployeeId,int companyId)
        {
            try
            {
                var result = 0;
                projectAssignation.UpdatedDate = DateTime.Now;
                projectAssignation.UpdatedBy = sessionEmployeeId;
                List<ProjectAssignationEntity> projectAssignationEntitys = new List<ProjectAssignationEntity>();
                var getTeamleadId = projectAssignation.TeamLeadId.Split(',');
                var getEmployeeId = projectAssignation.EmployeeId.Split(',');
                var getManagerEmpIds = projectAssignation.ProjectManagerId.Split(',');
                var projectManagerAssignation = EmployeeAssignation(projectAssignation.ProjectId, getManagerEmpIds, sessionEmployeeId, 3);
                var teamLeadAssignation = EmployeeAssignation(projectAssignation.ProjectId, getTeamleadId, sessionEmployeeId, 2);
                var employeeAssignation = EmployeeAssignation(projectAssignation.ProjectId, getEmployeeId, sessionEmployeeId, 1);
                projectAssignationEntitys.AddRange(projectManagerAssignation);
                projectAssignationEntitys.AddRange(teamLeadAssignation);
                projectAssignationEntitys.AddRange(employeeAssignation);

                var project = await _projectDetailsRepository.GetByProjectId(projectAssignation.ProjectId, companyId);
                var employees = await _employeesRepository.GetAllEmployeeDetails(companyId);
                var getProjectAssignation = await _projectDetailsRepository.GetEmployeeByProjectId(projectAssignation.ProjectId, companyId);
                var draftTypeId = 0;
                EmailDraftContentEntity emailDraftContentEntity = null;
                List<int> first = new List<int>();
                foreach (var entity in getEmployeeId)
                {
                    first.Add(Convert.ToInt32(entity));
                }
                var second = getProjectAssignation.Where(x => x.Type == 1).Select(y => y.EmployeeId).ToList();
                if (getProjectAssignation.Count() > 0)
                {
                    var projectType = await _projectDetailsRepository.GetByProjectType(projectAssignation.ProjectId, companyId);
                    var projectManagerName = "";
                    var projectStartDate = project.Startdate;
                    if (projectManagerAssignation.Count() > 0)
                    {
                        var strname = new List<string>();
                        foreach (var items in projectManagerAssignation)
                        {
                            var employeeName = employees.FirstOrDefault(x => x.EmpId == Convert.ToInt32(items.EmployeeId));
                            strname.Add(employeeName.FirstName + " " + employeeName.LastName);
                        }

                        var str = String.Join(",", strname);
                        projectManagerName = str;
                    }

                    IEnumerable<int> firstDiffSecond = first.Except(second);
                    IEnumerable<int> secondDiffFirst = second.Except(first);
                    if (firstDiffSecond.Count() > 0)
                    {
                        draftTypeId = (int)EmailDraftType.ProjectAssignation;
                        emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,companyId);
                        foreach (var item in firstDiffSecond)
                        {
                            var employee = employees.FirstOrDefault(x => x.EmpId == Convert.ToInt32(item));
                            if (employee != null)
                            {
                                var assignedBy = employees.Where(x => x.EmpId == sessionEmployeeId).FirstOrDefault();
                                var bodyContent = EmailBodyContent.SendEmail_Body_ProjectAssignation(employee, project.ProjectName, projectType.ProjectTypeName, Convert.ToDateTime(project.Startdate), projectManagerName, assignedBy.FirstName + " " + assignedBy.LastName, emailDraftContentEntity.DraftBody);
                                await InsertEmailProjectAssignation(employee.OfficeEmail, emailDraftContentEntity, bodyContent);
                            }
                        }
                    }
                    else if (secondDiffFirst.Count() > 0)
                    {
                        draftTypeId = (int)EmailDraftType.ProjectRejection;
                        emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                        foreach (var item in secondDiffFirst)
                        {
                            var employee = employees.FirstOrDefault(x => x.EmpId == Convert.ToInt32(item));
                            if (employee != null)
                            {
                                var rejectedBy = employees.Where(x => x.EmpId == sessionEmployeeId).FirstOrDefault();
                                var bodyContent = EmailBodyContent.SendEmail_Body_ProjectRemoveEmployee(employee, project.ProjectName, projectType.ProjectTypeName, Convert.ToDateTime(project.Startdate), projectManagerName, rejectedBy.FirstName + " " + rejectedBy.LastName, emailDraftContentEntity.DraftBody);
                                await RemoveProjectEmailEmployee(employee.OfficeEmail, emailDraftContentEntity, bodyContent);
                            }
                        }
                    }
                }
                var data = await _projectDetailsRepository.UpdateProjectAssignation(projectAssignationEntitys, projectAssignation.ProjectId, companyId);
                result = Convert.ToInt32(data);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  Logic to get delete the projectassignation details
        /// </summary>
        /// <param name="projectAssignation" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> DeleteProjectAssignation(ProjectAssignation projectAssignation, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            var projectAssignationEntity = await _projectDetailsRepository.GetByProjectAssignationId(projectAssignation.ProjectId, companyId);
            projectAssignationEntity.ForEach(x =>
            {
                x.UpdatedDate = DateTime.Now;
                x.UpdatedBy = sessionEmployeeId;
                x.IsDeleted = true;
            });
            var data = await _projectDetailsRepository.DeleteProjectAssignation(projectAssignationEntity, companyId);
            result = Convert.ToInt32(data);

            return result;
        }

        /// <summary>
        ///  Logic to get the projectassignation details get types
        /// </summary>
        /// <param name="projectId" ></param>
        /// <param name="employeeAssignation" ></param>
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="type" ></param>
        private List<ProjectAssignationEntity> EmployeeAssignation(int projectId, string[] employeeAssignation, int sessionEmployeeId, int type)
        {
            var projectAssignationEntitys = new List<ProjectAssignationEntity>();
            foreach (var item in employeeAssignation)
            {
                ProjectAssignationEntity projectAssignationEntity = new ProjectAssignationEntity();
                projectAssignationEntity.ProjectId = projectId;
                projectAssignationEntity.Type = type;
                projectAssignationEntity.EmployeeId = Convert.ToInt32(item);
                projectAssignationEntity.CreatedBy = sessionEmployeeId;
                projectAssignationEntity.CreatedDate = DateTime.Now;
                projectAssignationEntitys.Add(projectAssignationEntity);
            }
            return projectAssignationEntitys;
        }

        /// <summary>
        /// Logic to get the projectassignation detail by particular projectId
        /// </summary> 
        /// <param name="projectId" ></param>
        public async Task<ProjectAssignation> GetByProjectAssignationId(int projectId,int companyId)
        {
            var projectDetails = new ProjectAssignation();
            projectDetails.ProjectAssignationName = await GetProjectAssignation(projectId, companyId);
            var projectAssignationEntitys = await _projectDetailsRepository.GetEmployeeByProjectId(projectId, companyId);
            projectDetails.DropdownEmployee = await GetAllEmployees(companyId);
            projectDetails.DropdownTeamLeads = await GetAllTeamLeads(companyId);
            projectDetails.DropdownProjectManagers = await GetAllProjectmanagers(companyId);
            projectDetails.ProjectDetails = await GetProjectDetailsId(companyId);
            return projectDetails;
        }

        /// <summary>
        /// Logic to get projectassignationname the projectassignation detail by particular projectId
        /// </summary> 
        /// <param name="projectId" ></param>
        public async Task<ProjectAssignationName> GetProjectAssignation(int projectId,int companyId)
        {
            var name = new ProjectAssignationName();
            var projectAssignationEntitys = await _projectDetailsRepository.GetEmployeeByProjectId(projectId, companyId);
            var ProjectId = projectAssignationEntitys.Where(y => y.ProjectId == projectId).Select(y => y.ProjectId).ToList();
            var projectManagerAssignations = projectAssignationEntitys.Where(x => x.Type == 3).Select(x => x.EmployeeId).ToList();
            var teamLeadAssignations = projectAssignationEntitys.Where(x => x.Type == 2).Select(x => x.EmployeeId).ToList();
            var employeeAssignations = projectAssignationEntitys.Where(x => x.Type == 1).Select(x => x.EmployeeId).ToList();

            var allemployee = await _employeesRepository.GetAllEmployees(companyId);
            var proNames = string.Empty;
            var teamNames = string.Empty;
            var empNames = string.Empty;

            var StrFmtProjectmanagerEmpId = "";
            var StrFmtTeamLeadEmpId = "";
            var StrFmtEmployeeEmpId = "";
            var finalOut = "";
            for (int i = 0; i < projectManagerAssignations.Count(); i++)
            {
                var b = projectManagerAssignations[i];
                StrFmtProjectmanagerEmpId += string.Format(b + ",");
            }
            if (!string.IsNullOrEmpty(StrFmtProjectmanagerEmpId))
            {
                finalOut = StrFmtProjectmanagerEmpId.Remove(StrFmtProjectmanagerEmpId.Length - 1, 1);
            }
            name.StrFmtProjectmanagerEmpId = finalOut;

            var finalOutTeamLead = "";
            for (int i = 0; i < teamLeadAssignations.Count(); i++)
            {
                var b = teamLeadAssignations[i];
                StrFmtTeamLeadEmpId += string.Format(b + ",");
            }
            if (!string.IsNullOrEmpty(StrFmtTeamLeadEmpId))
            {
                finalOutTeamLead = StrFmtTeamLeadEmpId.Remove(StrFmtTeamLeadEmpId.Length - 1, 1);
            }
            name.StrFmtTeamLeadEmpId = finalOutTeamLead;

            var finalOutEmployee = "";
            for (int i = 0; i < employeeAssignations.Count(); i++)
            {
                var b = employeeAssignations[i];
                StrFmtEmployeeEmpId += string.Format(b + ",");
            }
            if (!string.IsNullOrEmpty(StrFmtEmployeeEmpId))
            {
                finalOutEmployee = StrFmtEmployeeEmpId.Remove(StrFmtEmployeeEmpId.Length - 1, 1);
            }
            name.StrFmtEmployeeEmpId = finalOutEmployee;

            foreach (var eId in projectManagerAssignations)
            {
                var employeeEntity = allemployee.Where(r => r.EmpId == eId).FirstOrDefault();
                if (employeeEntity != null)
                {
                    proNames += employeeEntity.FirstName + employeeEntity.LastName + ",";
                }
            }
            foreach (var eId in teamLeadAssignations)
            {
                var employeeEntity = allemployee.Where(r => r.EmpId == eId).FirstOrDefault();
                if (employeeEntity != null)
                {
                    teamNames += employeeEntity.FirstName + employeeEntity.LastName + ",";
                }
            }
            foreach (var eId in employeeAssignations)
            {
                var employeeEntity = allemployee.Where(r => r.EmpId == eId).FirstOrDefault();
                if (employeeEntity != null)
                {
                    empNames += employeeEntity.FirstName + employeeEntity.LastName + ",";
                }
            }
            name.ProjectManagerName = proNames.Trim(new char[] { ',' });
            name.TeamLeadName = teamNames.Trim(new char[] { ',' });
            name.EmployeeName = empNames.Trim(new char[] { ',' });

            return name;
        }

        /// <summary>
        /// Logic to get currency the list
        /// </summary> 
        public async Task<List<CurrencyViewModel>> GetAllCurrency()
        {
            var listOfCurrencyViewModel = new List<CurrencyViewModel>();
            var listOfCurrencys = await _projectDetailsRepository.GetAllCurrency();
            if (listOfCurrencys != null)
            {
                listOfCurrencyViewModel = _mapper.Map<List<CurrencyViewModel>>(listOfCurrencys);
            }
            return listOfCurrencyViewModel;
        }

        /// <summary>
        /// Logic to get AllProjectdetails count  
        /// </summary> 
        /// <param name="pager" ></param>
        public async Task<int> GetAllProjectsCount(SysDataTablePager pager, int companyId)
        {
            return await _projectDetailsRepository.GetAllProjectsCount(pager, companyId);
        }
    }
}
