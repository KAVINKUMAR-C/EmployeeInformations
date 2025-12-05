using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.Service
{
    public class MasterService : IMasterService
    {
        private readonly IMasterRepository _masterRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public MasterService(IMasterRepository masterRepository, IReportRepository reportRepository, IEmailDraftRepository emailDraftRepository, IEmployeesRepository employeesRepository,
            ICompanyRepository companyRepository, IMapper mapper, IConfiguration config)
        {
            _masterRepository = masterRepository;
            _reportRepository = reportRepository;
            _emailDraftRepository = emailDraftRepository;
            _employeesRepository = employeesRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _config = config;
        }

        //Designation

        /// <summary>
        /// Logic to get designation list
        /// </summary>        
        public async Task<DesignationViewModel> GetAllDesignation(int sessionCompanyId)
        {
          
            var designationViewModel = new DesignationViewModel();
            designationViewModel.Designation = await _masterRepository.GetAllDesignation(sessionCompanyId);
            return designationViewModel;
        }

        /// <summary>
        /// Logic to get  create designation details
        /// </summary>
        /// <param name="designation" ></param>       
        public async Task<Designation> Create(Designation designation,int sessionCompanyId)
        {
            var designations = new Designation();
            var totaldepartmentName = await _masterRepository.GetDesignationName(designation.DesignationName, sessionCompanyId);
            if (totaldepartmentName == 0)
            {
                if (designation != null)
                {
                    if (designation.DesignationId == 0)
                    {
                        var designationEntity = _mapper.Map<DesignationEntity>(designation);
                        designationEntity.CompanyId = sessionCompanyId;
                        var datas = await _masterRepository.Create(designationEntity);
                        designations.DesignationId = designationEntity.DesignationId;
                    }
                }
            }
            else
            {
                designations.DesignationNameCount = totaldepartmentName;
            }
            return designations;
        }

        /// <summary>
        /// Logic to get check designationName the designation detail  by particular designationName not allow repeated designationName
        /// </summary>
        /// <param name="designationName" ></param>
        public async Task<int> GetDesignationName(string designationName,int sessionCompanyId)
        {
            var totaldepartmentName = await _masterRepository.GetDesignationName(designationName, sessionCompanyId);
            return totaldepartmentName;
        }

        /// <summary>
        /// Logic to get edit isactive the designation detail  by particular designation
        /// </summary>
        /// <param name="designation" ></param>
        public async Task<int> UpdateDesignation(Designation designation, int sessionCompanyId)
        {
            var designationEntity = _mapper.Map<DesignationEntity>(designation);
            designationEntity.CompanyId = sessionCompanyId;
            await _masterRepository.UpdateDesignation(designationEntity);
            var result = designationEntity.DesignationId;
            return result;
        }

        /// <summary>
        /// Logic to get delete designationdetails by particular designationId
        /// </summary> 
        /// <param name="departmentId" ></param> 
        public async Task<bool> DeletedDesignation(int designationId,int sessionCompanyId)
        {
            var result = await _masterRepository.DeletedDesignation(designationId, sessionCompanyId);
            return result;
        }

        //Department

        /// <summary>
        /// Logic to get department list
        /// </summary>    
        public async Task<DepartmentViewModel> GetAllDepartment(int companyId)
        {
            var departmentViewModel = new DepartmentViewModel();
            departmentViewModel.Department = await _masterRepository.GetAllDepartment(companyId);
            return departmentViewModel;
        }

        /// <summary>
        /// Logic to get  create department details
        /// </summary>
        /// <param name="department" ></param>       
        public async Task<Department> CreateDepartment(Department department,int companyId)
        {
            var departments = new Department();
            var totalDepartmentName = await _masterRepository.GetDepartmentName(department.DepartmentName, companyId);
            if (totalDepartmentName == 0)
            {
                if (department != null)
                {
                    if (department.DepartmentId == 0)
                    {
                        var departmentEntity = _mapper.Map<DepartmentEntity>(department);
                        departmentEntity.CompanyId = companyId;
                        var value = await _masterRepository.CreateDepartment(departmentEntity);
                        departments.DepartmentId = departmentEntity.DepartmentId;
                    }
                }
            }
            else
            {
                departments.DepartmentNameCount = totalDepartmentName;
            }
            return departments;
        }

        /// <summary>
        /// Logic to get check departmentName the department detail  by particular departmentName not allow repeated departmentName
        /// </summary>
        /// <param name="departmentName" ></param>
        public async Task<int> GetDepartmentName(string departmentName,int companyId)
        {
            var totalDepartmentName = await _masterRepository.GetDepartmentName(departmentName,companyId);
            return totalDepartmentName;
        }

        /// <summary>
        /// Logic to get edit isactive the department detail  by particular department
        /// </summary>
        /// <param name="department" ></param>
        public async Task<int> UpdateDepartment(Department department,int companyId)
        {
            var departmentEntity = _mapper.Map<DepartmentEntity>(department);
            departmentEntity.CompanyId = companyId;
            await _masterRepository.UpdateDepartment(departmentEntity);
            var result = departmentEntity.DepartmentId;
            return result;
        }

        /// <summary>
        /// Logic to get delete departmentdetails by particular departmentId
        /// </summary> 
        /// <param name="departmentId" ></param> 
        public async Task<bool> DeleteDepartment(int departmentId,int companyId)
        {
            var result = await _masterRepository.DeleteDepartment(departmentId, companyId);
            return result;
        }

        //Role

        /// <summary>
        /// Logic to get role list
        /// </summary>        
        public async Task<RoleTableViewModel> GetAllRole(int companyId)
        {
            var roleTableViewModel = new RoleTableViewModel();
            roleTableViewModel.RoleTableMaster = await _masterRepository.GetAllRoles(companyId);
            return roleTableViewModel;
        }

        /// <summary>
        /// Logic to get  create role details
        /// </summary>
        /// <param name="roleTableMaster" ></param>      
        public async Task<RoleTableMaster> CreateRole(RoleTableMaster roleTableMaster,int companyId)
        {
            var totalRoleNameCount = await _masterRepository.GetRoleName(roleTableMaster.RoleName,companyId);

            var roleTable = new RoleTableMaster();
            if (totalRoleNameCount == 0)
            {
                if (roleTableMaster != null)
                {
                    if (roleTableMaster.RoleId == 0)
                    {
                        var roleEntityEntity = _mapper.Map<RoleEntity>(roleTableMaster);
                        roleEntityEntity.IsActive = true;
                        roleEntityEntity.CompanyId = companyId;
                        var data = await _masterRepository.CreateRole(roleEntityEntity);
                        roleTable.RoleId = (Role)roleEntityEntity.RoleId;
                    }
                }
            }
            else
            {
                roleTable.RoleNameCount = totalRoleNameCount;
            }
            return roleTable;
        }

        /// <summary>
        /// Logic to get delete the role detail by particular roleId
        /// </summary>
        /// <param name="roleId" ></param>
        public async Task<bool> DeleteRole(int roleId,int companyId)
        {
            var result = await _masterRepository.DeleteRole(roleId,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get update isactive the role detail  by particular role
        /// </summary>
        /// <param name="roleTableMaster" ></param>
        public async Task<int> DeleteIsActive(RoleTableMaster roleTableMaster,int companyId)
        {
            var roleTableEntity = _mapper.Map<RoleEntity>(roleTableMaster);
            roleTableEntity.CompanyId = companyId;
            await _masterRepository.DeleteIsActive(roleTableEntity);
            var result = roleTableEntity.RoleId;
            return result;
        }

        /// <summary>
        /// Logic to get check roleName the role detail  by particular roleName not allow repeated roleName
        /// </summary>
        /// <param name="roleName" ></param>
        public async Task<int> GetRoleName(string roleName,int companyId) 
        {
            var totalRoleNameCount = await _masterRepository.GetRoleName(roleName, companyId);
            return totalRoleNameCount;
        }

        // Module 

        /// <summary>
        /// Logic to get modules details the list
        /// </summary>
        public async Task<ModuleViewModel> GetAllModules(int sessionCompanyId)
        {
            var moduleViewModel = new ModuleViewModel();
            moduleViewModel.Modules = await _masterRepository.GetAllModules(sessionCompanyId);
            return moduleViewModel;
        }

        /// <summary>
        /// Logic to get  create modules details
        /// </summary>
        /// <param name="modules" ></param>        
        public async Task<Modules> CreateModule(Modules modules)
        {
            var totalModuleNameCount = await _masterRepository.GetModuleName(modules.Name);
            var module = new Modules();
            if (totalModuleNameCount == 0)
            {
                if (modules != null)
                {
                    if (modules.Id == 0)
                    {
                        var modulesEntity = _mapper.Map<ModulesEntity>(modules);
                        modulesEntity.IsActive = true;
                        module.Id = await _masterRepository.CreateModule(modulesEntity);
                    }
                }
            }
            else
            {
                module.NameCount = totalModuleNameCount;
            }
            return module;
        }

        /// <summary>
        /// Logic to get update isactive the modules detail  by particular modules
        /// </summary>
        /// <param name="modules" ></param>
        public async Task<int> UpdateModule(Modules modules)
        {
            var modulesEntity = _mapper.Map<ModulesEntity>(modules);
            await _masterRepository.UpdateModule(modulesEntity);
            var result = modulesEntity.Id;
            return result;
        }

        /// <summary>
        /// Logic to get check moduleName the module detail  by particular moduleName not allow repeated moduleName
        /// </summary>
        /// <param name="moduleName" ></param>   
        public async Task<int> GetModuleName(string moduleName)
        {
            var totalModuleNameCount = await _masterRepository.GetModuleName(moduleName);
            return totalModuleNameCount;
        }

        /// <summary>
        /// Logic to get delete Moduledetails by particular id
        /// </summary> 
        /// <param name="id" ></param> 
        public async Task<bool> DeleteModule(int id)
        {
            var result = await _masterRepository.DeleteModule(id);
            return result;
        }

        //SubModules

        /// <summary>
        /// Logic to get submodules details list
        /// </summary>       
        public async Task<SubModulesViewModel> GetAllSubModules(int companyId)
        {
            var subModulesViewModel = new SubModulesViewModel();
            subModulesViewModel.SubModules = await _masterRepository.GetAllSubModules(companyId);
            return subModulesViewModel;
        }

        /// <summary>
        /// Logic to get  create subModules details
        /// </summary>
        /// <param name="subModules" ></param>       
        public async Task<SubModules> CreateSubModules(SubModules subModules)
        {
            var totalcountsubModuleName = await _masterRepository.GetSubModulesName(subModules.Name,subModules.CompanyId);
            var subModule = new SubModules();
            if (totalcountsubModuleName == 0)
            {
                if (subModules != null)
                {
                    if (subModules.SubModuleId == 0)
                    {
                        var subModulesEntity = _mapper.Map<SubModulesEntity>(subModules);
                        subModulesEntity.IsActive = true;
                        var datas = await _masterRepository.CreateSubModule(subModulesEntity);
                        subModule.SubModuleId = subModulesEntity.SubModuleId;
                    }
                }
            }
            else
            {
                subModule.NameCount = totalcountsubModuleName;
            }
            return subModule;
        }

        /// <summary>
        /// Logic to get check name the submodule detail  by particular name not allow repeated name
        /// </summary>
        /// <param name="name" ></param> 
        public async Task<int> GetSubModulesName(string name, int companyId)
        {
            var totalcountsubModuleName = await _masterRepository.GetSubModulesName(name, companyId);
            return totalcountsubModuleName;
        }

        /// <summary>
        /// Logic to get modules details list using for submodules
        /// </summary> 
        public async Task<List<ModuleName>> GetAllModuleName(int companyId)
        {
            var listofModuleName = new List<ModuleName>();
            var listofModule = await _masterRepository.GetAllModuleName(companyId);
            if (listofModule != null)
            {
                listofModuleName = _mapper.Map<List<ModuleName>>(listofModule);
            }
            return listofModuleName;
        }

        /// <summary>
        /// Logic to get delete subModuledetails by particular subModuleId
        /// </summary> 
        /// <param name="subModuleId" ></param> 
        public async Task<bool> DeleteSubModuleId(int subModuleId)
        {
            var result = await _masterRepository.DeleteSubModuleId(subModuleId);
            return result;
        }

        /// <summary>
        /// Logic to get update isactive the submodules detail  by particular submodules
        /// </summary>
        /// <param name="subModules" ></param> 
        public async Task<int> UpdateSubModule(SubModules subModules)
        {
            var subModulesEntity = _mapper.Map<SubModulesEntity>(subModules);
            await _masterRepository.UpdateSubModule(subModulesEntity);
            var result = subModulesEntity.SubModuleId;
            return result;
        }

        //ProjectTypes

        /// <summary>
        /// Logic to get projectstype details list
        /// </summary>
        public async Task<ProjectTypeViewModule> GetAllProjectTypes(int sessionCompanyId)
        {
            var projectTypeViewModule = new ProjectTypeViewModule();

            projectTypeViewModule.ProjectTypeMaster = await _masterRepository.GetAllProjectTypes(sessionCompanyId);
            return projectTypeViewModule;
        }

        /// <summary>
        /// Logic to get create projecttype details by particular projectTypeMaster
        /// </summary>
        /// /// <param name="projectTypeMaster" ></param>        
        public async Task<ProjectTypeMaster> CreateProjectType(ProjectTypeMaster projectTypeMaster,int CompanyId)
        {
            var projectTypeMasters = new ProjectTypeMaster();
            var totalcountprojectTypeName = await _masterRepository.GetProjectTypes(projectTypeMaster.ProjectTypeName, CompanyId);
            if (totalcountprojectTypeName == 0)
            {
                if (projectTypeMaster != null)
                {
                    if (projectTypeMaster.ProjectTypeId == 0)
                    {
                        var projectTypesEntity = _mapper.Map<ProjectTypesEntity>(projectTypeMaster);
                        projectTypesEntity.CompanyId = CompanyId;
                        var project = await _masterRepository.CreateProjectTypes(projectTypesEntity);
                        projectTypeMasters.ProjectTypeId = projectTypesEntity.ProjectTypeId;
                    }
                }
            }
            else
            {
                projectTypeMasters.ProjectTypeNameCount = totalcountprojectTypeName;
            }
            return projectTypeMasters;
        }

        /// <summary>
        /// Logic to get check projectTypeName the projectType detail  by particular projectTypeName not allow repeated projectTypeName
        /// </summary>
        /// <param name="projectTypeName" ></param> 
        public async Task<int> GetProjectTypes(string projectTypeName,int companyId)
        {
            var totalcountprojectTypeName = await _masterRepository.GetProjectTypes(projectTypeName, companyId);
            return totalcountprojectTypeName;
        }

        /// <summary>
        /// Logic to get delete projectType details by particular projectTypeId
        /// </summary> 
        /// <param name="projectTypeId" ></param> 
        public async Task<bool> DeleteProjectTypeId(int projectTypeId,int companyId)
        {
            var result = await _masterRepository.DeleteProjectTypeId(projectTypeId, companyId);
            return result;
        }

        //DocumentTypes

        /// <summary>
        /// Logic to get documentType details list
        /// </summary>         
        public async Task<DocumentTypesViewModel> GetAllDocumentTypes(int companyId)
        {
            var documentTypesViewModel = new DocumentTypesViewModel();
            documentTypesViewModel.DocumentType = await _masterRepository.GetAllDocumentTypes(companyId);
            return documentTypesViewModel;
        }

        /// <summary>
        /// Logic to get delete documentType details by particular documentTypeId
        /// </summary> 
        /// <param name="documentTypeId" ></param> 
        public async Task DeletedDocumentTypeId(int documentTypeId,int companyId)
        {
            await _masterRepository.DeletedDocumentTypeId(documentTypeId, companyId);
        }

        /// <summary>
        /// Logic to get create documentType details by particular documentType
        /// </summary> 
        /// <param name="documentType" ></param>       
        public async Task<DocumentType> CreateDocumentTypes(DocumentType documentType,int companyId)
        {
            var totalcountdocumentTypeName = await _masterRepository.GetDocumentTypes(documentType.DocumentName, companyId);
            var documentTypes = new DocumentType();
            if (totalcountdocumentTypeName == 0)
            {
                if (documentType != null)
                {
                    if (documentType.DocumentTypeId == 0)
                    {
                        var documentTypesEntity = _mapper.Map<DocumentTypesEntity>(documentType);
                        documentTypesEntity.IsActive = true;
                        documentTypesEntity.CompanyId = companyId;
                        var document = await _masterRepository.CreateDocumentType(documentTypesEntity);
                        documentTypes.DocumentTypeId = documentTypesEntity.DocumentTypeId;
                    }
                }
            }
            else
            {
                documentTypes.DocumentNameCount = totalcountdocumentTypeName;
            }
            return documentTypes;
        }

        /// <summary>
        /// Logic to get update isactive the documenttype detail by particular documenttype
        /// </summary> 
        /// <param name="documentType" ></param>
        public async Task<int> UpdateDocumentTypes(DocumentType documentType,int companyId)
        {
            var documentTypesEntity = _mapper.Map<DocumentTypesEntity>(documentType);
            documentTypesEntity.CompanyId = companyId;
            await _masterRepository.UpdateDocumentTypes(documentTypesEntity);
            var result = documentTypesEntity.DocumentTypeId;
            return result;
        }

        /// <summary>
        /// Logic to get check count the documenttype detail 
        /// </summary> 
        /// <param name="documentName" ></param>      
        public async Task<int> GetDocumentTypes(string documentName,int companyId)
        {
            var totalcountdocumentTypeName = await _masterRepository.GetDocumentTypes(documentName, companyId);
            return totalcountdocumentTypeName;
        }

        //SkillSet

        /// <summary>
        /// Logic to get check count the skillset detail 
        /// </summary>        
        public async Task<SkillSetViewModel> GetAllSkillSet(int companyId)
        {
            var skillSets = new List<SkillSets>();
            var SkillSetViewModel = new SkillSetViewModel();
            var SkillSetEntitys = await _masterRepository.GetAllSkillSet(companyId);
            if (SkillSetEntitys != null)
            {
                skillSets = _mapper.Map<List<SkillSets>>(SkillSetEntitys);
            }
            SkillSetViewModel.SkillSets = skillSets;
            return SkillSetViewModel;
        }

        /// <summary>
        /// Logic to get delete skillset details by particular skillId
        /// </summary> 
        /// <param name="skillId" ></param>
        public async Task<bool> DeletedSkillSetId(int skillId,int companyId)
        {
            var result = await _masterRepository.DeletedSkillSetId(skillId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get create skillset details by particular skillSets
        /// </summary> 
        /// <param name="skillSets" ></param>      
        public async Task<SkillSets> CreateskillSets(SkillSets skillSets,int companyId)
        {
            var totalCountSkillSet = await _masterRepository.GetSkillSet(skillSets.SkillName, companyId);
            var SkillSet = new SkillSets();
            if (totalCountSkillSet == 0)
            {
                if (skillSets != null)
                {
                    if (skillSets.SkillId == 0)
                    {
                        var skillSetEntity = _mapper.Map<SkillSetEntity>(skillSets);
                        skillSetEntity.IsActive = true;
                        skillSetEntity.CompanyId = companyId;
                        var skillset = await _masterRepository.CreateskillSet(skillSetEntity);
                        SkillSet.SkillId = skillSetEntity.SkillId;
                    }
                }
            }
            else
            {
                SkillSet.SkillNameCount = totalCountSkillSet;
            }
            return SkillSet;
        }

        /// <summary>
        /// Logic to get check count the skillset detail
        /// </summary> 
        /// <param name="skillName" ></param>       
        public async Task<int> GetSkillSet(string skillName,int companyId)
        {
            var totalCountSkillSet = await _masterRepository.GetSkillSet(skillName, companyId);
            return totalCountSkillSet;
        }

        /// <summary>
        /// Logic to get edit isactive the skillSets detail  by particular skillSets
        /// </summary> 
        /// <param name="skillSets" ></param>       
        public async Task<int> UpdateSkillSet(SkillSets skillSets,int companyId)
        {
            var skillSetEntity = _mapper.Map<SkillSetEntity>(skillSets);
            skillSetEntity.CompanyId = companyId;
            await _masterRepository.UpdateSkillSet(skillSetEntity);
            var result = skillSetEntity.SkillId;
            return result;
        }

        //EmailSettings

        /// <summary>
        /// Logic to get emailsetting details list
        /// </summary>       
        public async Task<EmailSettingsViewModel> GetAllEmailSettings(int companyId)
        {
            var emailSettingsViewModel = new EmailSettingsViewModel();
            emailSettingsViewModel = await _masterRepository.GetAllEmailSettings(companyId);
            return emailSettingsViewModel;
        }

        /// <summary>
        /// Logic to get create and update the emailsetting details 
        /// </summary>
        /// <param name="emailSettings" ></param> 
        public async Task<int> CreateEmailSettings(EmailSettings emailSettings,int companyId)
        {
            var result = 0;
            if (emailSettings != null)
            {
                if (emailSettings.EmailSettingId == 0)
                {
                    var emailSettingsEntity = _mapper.Map<EmailSettingsEntity>(emailSettings);
                    emailSettingsEntity.Password = Common.Common.Encrypt(emailSettings.Password);
                    emailSettingsEntity.CompanyId = companyId;
                    result = await _masterRepository.CreateEmailSettings(emailSettingsEntity);
                    return result;
                }
                else
                {

                    var emailSettingsEntity = _mapper.Map<EmailSettingsEntity>(emailSettings);
                    emailSettingsEntity.Password = Common.Common.Encrypt(emailSettings.Password);
                    emailSettingsEntity.CompanyId = companyId;
                    result = await _masterRepository.CreateEmailSettings(emailSettingsEntity);
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get create and update the sendEmails details 
        /// </summary>
        /// <param name="sendEmails" ></param>
        public async Task<int> AddSendEmails(SendEmails sendEmails,int companyId)
        {
            var result = 0;
            if (sendEmails != null)
            {
                if (sendEmails.EmailListId == 0)
                {

                    var sendEmailsEntity = _mapper.Map<SendEmailsEntity>(sendEmails);
                    sendEmailsEntity.CompanyId = companyId;
                    var add = await _masterRepository.AddSendEmails(sendEmailsEntity);
                    result = sendEmailsEntity.EmailSettingId;
                }
                else
                {
                    var sendEmailsEntity = await _masterRepository.GetSendEmailsByEmailListId(sendEmails.EmailListId, companyId);
                    var mapsendEmailsEntity = _mapper.Map<SendEmailsEntity>(sendEmails);
                    sendEmailsEntity.CompanyId = companyId;
                    var data = await _masterRepository.AddSendEmails(mapsendEmailsEntity);
                    result = sendEmailsEntity.EmailSettingId;
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get delete the sendEmails details by particular emailListId 
        /// </summary>
        /// <param name="emailListId" ></param>
        public async Task<bool> DeleteSendEmail(int emailListId,int companyId)
        {
            var result = await _masterRepository.DeleteSendEmail(emailListId, companyId);
            return result;
        }

        /// <summary>
        ///  Logic to get check count fromEmail the emailsetting detail  
        /// </summary>
        /// <param name="fromEmail" ></param>   
        public async Task<int> GetEmailCount(string fromEmail,int companyId)
        {
            var totalEmailSettingCount = await _masterRepository.GetEmailCount(fromEmail, companyId);
            return totalEmailSettingCount;
        }

        /// <summary>
        /// Logic to get check count emailId the sendemail detail 
        /// </summary>        
        /// <param name="emailId" ></param>       
        public async Task<int> GetSendEmailCount(string emailId,int companyId)
        {
            var totalSendEmailCount = await _masterRepository.GetSendEmailCount(emailId, companyId);
            return totalSendEmailCount;
        }

        // leave type

        /// <summary>
        /// Logic to get leavetype details list 
        /// </summary>   
        public async Task<LeaveTypeViewModel> GetAllLeaveTypes(int companyId)
        {
            var leaveTypes = new List<LeaveTypeMaster>();
            var leaveTypesViewModel = new LeaveTypeViewModel();
            var leaveTypesEntitys = await _masterRepository.GetAllLeaveTypes(companyId);
            if (leaveTypesEntitys != null)
            {
                leaveTypes = _mapper.Map<List<LeaveTypeMaster>>(leaveTypesEntitys);
            }
            leaveTypesViewModel.LeaveTypeMaster = leaveTypes;
            return leaveTypesViewModel;
        }

        /// <summary>
        /// Logic to get create the leavetype details  
        /// </summary> 
        /// <param name="leaveTypeMaster"></param>        
        public async Task<LeaveTypeMaster> CreateLeaveType(LeaveTypeMaster leaveTypeMaster, int companyId)
        {
            var totalleaveType = await _masterRepository.GetLeaveType(leaveTypeMaster.LeaveType,companyId);
            var leaveTypes = new LeaveTypeMaster();
            var leave = await _masterRepository.GetAllCompanyLeaveType();
            var existingLeave = leave.FirstOrDefault(x => x.LeaveType.Equals(leaveTypeMaster.LeaveType, StringComparison.OrdinalIgnoreCase));
            if (totalleaveType == 0 && existingLeave != null)
            {
                if (leaveTypeMaster != null)
                {
                    if (leaveTypeMaster.LeaveTypeId == 0)
                    { 
                        leaveTypeMaster.LeaveTypeId = existingLeave.LeaveTypeId; 
                        leaveTypeMaster.IsActive = true;
                        var leaveTypeEntity = _mapper.Map<LeaveTypesEntity>(leaveTypeMaster);
                        leaveTypeEntity.IsActive = true;
                        var value = await _masterRepository.CreateLeaveType(leaveTypeEntity,companyId);
                        leaveTypes.LeaveTypeId = leaveTypeEntity.LeaveTypeId;
                        leaveTypes.CompanyId = leaveTypeEntity.CompanyId;
                    }
                }
            }
            else
            {
                leaveTypes.LeaveTypeCount = totalleaveType;
            }
            return leaveTypes;
        }

        //public async Task<LeaveTypeMaster> CreateLeaveType(LeaveTypeMaster leaveTypeMaster)
        //{
        //    var totalleaveType = await _masterRepository.GetLeaveType(leaveTypeMaster.LeaveType);
        //    var leave = await _masterRepository.GetAllCompanyLeaveType();  // List<LeaveTypeMaster>
        //    var leaveTypes = new LeaveTypeMaster();

        //    if (totalleaveType == 0)
        //    {
        //        if (leaveTypeMaster != null)
        //        {
        //            // Check if leave already exists in the list
        //            var existingLeave = leave.FirstOrDefault(x => x.LeaveType.Equals(leaveTypeMaster.LeaveType, StringComparison.OrdinalIgnoreCase));

        //            if (existingLeave != null)
        //            {
        //                // If already exists, assign existing ID
        //                leaveTypeMaster.LeaveTypeId = existingLeave.LeaveTypeId;
        //                var leaveTypeEntity = _mapper.Map<LeaveTypesEntity>(leaveTypeMaster);
        //                leaveTypeEntity.IsActive = true;
        //                var value = await _masterRepository.CreateLeaveType(leaveTypeEntity);
        //                leaveTypes.LeaveTypeId = leaveTypeEntity.LeaveTypeId;
        //                leaveTypes.IsActive = leaveTypeEntity.IsActive;
        //                leaveTypes.LeaveType = leaveTypeEntity.LeaveType;
        //                leaveTypes.CompanyId = leaveTypeEntity.CompanyId;
        //            }
        //            else
        //            {
        //                // Else, create a new leave type
        //                leaveTypeMaster.IsActive = true;
        //                var leaveTypeEntity = _mapper.Map<LeaveTypesEntity>(leaveTypeMaster);
        //                leaveTypeEntity.IsActive = true;

        //                var value = await _masterRepository.CreateLeaveType(leaveTypeEntity);
        //                leaveTypes.LeaveTypeId = leaveTypeEntity.LeaveTypeId;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        leaveTypes.LeaveTypeCount = totalleaveType;
        //    }

        //    return leaveTypes;
        //}


        /// <summary>
        ///Logic to get check count leaveType the leaveType detail 
        /// </summary>             
        /// <param name="leaveType" >leaveType</param>
        public async Task<int> GetLeaveType(string leaveType, int companyId)
        {
            var totalleaveType = await _masterRepository.GetLeaveType(leaveType,companyId);
            return totalleaveType;
        }

        /// <summary>
        ///Logic to get update the leaveType detail by particular leaveType
        /// </summary>             
        /// <param name="leaveTypeMaster" ></param>    
        public async Task<int> UpadateLeave(LeaveTypeMaster leaveTypeMaster, int companyId)
        {
            var leaveTypeEntity = _mapper.Map<LeaveTypesEntity>(leaveTypeMaster);
            await _masterRepository.UpadateLeave(leaveTypeEntity,companyId);
            var leaveType = leaveTypeEntity.LeaveTypeId;
            return leaveType;
        }

        /// <summary>
        ///Logic to get delete the leaveType detail by particular leaveTypeId
        /// </summary>             
        /// <param name="leaveTypeId" ></param>    
        public async Task<bool> DeleteLeaveType(int leaveTypeId, int companyId)
        {
            var result = await _masterRepository.DeleteLeaveType(leaveTypeId,companyId);
            return result;
        }

        // Announcement
        /// <summary>
        /// Logic to get AnnouncementFilterCount of the employees
        /// </summary>
        ///  <param name="companyId,pager"></param>
        public async Task<int> AnnouncementFilterCount(int companyId, SysDataTablePager pager)
        {
            var announcementFilterCount = await _masterRepository.AnnouncementFilterCount(companyId, pager);
            return announcementFilterCount;
        }

        /// <summary>
        /// Logic to get  AnnouncementFilter data of the employees
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection"></param>
        public async Task<List<AnnouncementFilterViewModel>> AnnouncementFilter(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            var announcementFilter = await _masterRepository.AnnouncementFilter(companyId, pager, columnName, columnDirection);
            return announcementFilter;

        }


        /// <summary>
        ///Logic to get announcement detail list
        /// </summary>  
        public async Task<AnnouncementViewModel> GetAllAnnouncement(int companyId)
        {
            var announcements = new List<Announcement>();
            var announcement = new Announcement();
            var announcementViewModel = new AnnouncementViewModel();
            var announcementEntitys = await _masterRepository.GetAllAnnouncement(companyId);

            foreach (var item in announcementEntitys)
            {
                var getattatchmentList = await _masterRepository.GetAnnouncementDocumentAndFilePath(item.AnnouncementId);
                foreach (var document in getattatchmentList)
                {
                    announcement = new Announcement();
                    item.Filepath = document.Document;
                    announcements.Add(announcement);
                }
            }

            if (announcementEntitys != null)
            {
                announcements = _mapper.Map<List<Announcement>>(announcementEntitys);
            }
            announcementViewModel.Announcement = announcements;
            announcementViewModel.ReportingPeople = await _reportRepository.GetAllEmployeesDropdown(companyId);
            announcementViewModel.Designation = await _masterRepository.GetDesignation(companyId);
            announcementViewModel.Department = await _masterRepository.GetDepartment(companyId);
            return announcementViewModel;
        }
        /// <summary>
        ///Logic to get announcementDetail
        /// </summary> 

        public async Task<AnnouncementViewModel> GetAnnouncement(int companyId)
        {

            var announcementViewModel = new AnnouncementViewModel();
            announcementViewModel.ReportingPeople = await _reportRepository.GetAllEmployeesDropdown(companyId);
            announcementViewModel.Designation = await _masterRepository.GetDesignation(companyId);
            announcementViewModel.Department = await _masterRepository.GetDepartment(companyId);
            return announcementViewModel;
        }
        /// <summary>
        ///Logic to get check count announcementName the announcement detail
        /// </summary> 
        /// <param name="announcementName" ></param> 
        public async Task<int> GetAnnouncementName(string announcementName,int companyId)
        {
            var totalannouncementName = await _masterRepository.GetAnnouncementName(announcementName, companyId);
            return totalannouncementName;
        }

        /// <summary>
        ///Logic to get create the announcement detail
        /// </summary> 
        /// <param name="announcement" ></param>
        /// <param name="sessionEmployeeId" ></param>       
        public async Task<Announcement> CreateAnnouncements(Announcement announcements, int sessionEmployeeId,int companyId)
        {
            var result = false;
            var announcement = new Announcement();
            if (announcements != null)
            {
                announcements.AnnouncementDate = DateTimeExtensions.ConvertToNotNullDatetime(announcements.strAnnouncementDate);
                announcements.AnnouncementEndDate = DateTimeExtensions.ConvertToNotNullDatetime(announcements.strAnnouncementEndDate);
                if (announcements.AnnouncementId == 0)
                {
                    var announcementEntitys = _mapper.Map<AnnouncementEntity>(announcements);
                    announcementEntitys.CreatedDate = DateTime.Now;
                    announcementEntitys.CreatedBy = sessionEmployeeId;
                    var value = await _masterRepository.CreateAnnouncement(announcementEntitys,companyId);
                    announcement.AnnouncementId = announcementEntitys.AnnouncementId;


                    if (announcements.announcementAttachments != null && announcements.announcementAttachments.Count() > 0)
                    {
                        var attachmentsEntitys = new List<AnnouncementAttachmentsEntity>();
                        foreach (var item in announcements.announcementAttachments)
                        {
                            var attachmentsEntity = new AnnouncementAttachmentsEntity();
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.AttachmentName = item.AttachmentName;
                            attachmentsEntity.AttachmentId = item.AttachmentId;
                            attachmentsEntity.AnnouncementId = announcementEntitys.AnnouncementId;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _masterRepository.InsertAnnouncementAttachment(attachmentsEntitys, announcementEntitys.AnnouncementId);
                    }

                    var document = await _masterRepository.GetAnnouncementDocumentAndFilePath(announcementEntitys.AnnouncementId);
                    string OfficeEmails = string.Empty;

                    string attachments = string.Empty;
                    var combinePath = new List<string>();
                    var ids = new List<int>();

                    if (announcements.EmpId != null)
                    {
                        var splitEmpId = announcements.EmpId.Split(',');
                        foreach (var data in splitEmpId)
                        {
                            ids.Add(Convert.ToInt32(data));
                            if (ids[0] == 0)
                            {
                                OfficeEmails += await GetOfficeEmails(companyId);
                            }
                        }
                        var employees = await _employeesRepository.GetTeambyId(ids,companyId);
                        foreach (var item in employees)
                        {
                            OfficeEmails += item.OfficeEmail + ",";
                        }

                    }
                    else if (announcements.DepartmentId != null)
                    {
                        var splitDepartmentId = announcements.DepartmentId.Split(',');
                        foreach (var data in splitDepartmentId)
                        {
                            ids.Add(Convert.ToInt32(data));

                            if (ids[0] == 0)
                            {
                                OfficeEmails += await GetOfficeEmails(companyId);
                            }
                        }
                        var email = await _employeesRepository.GetDepartmentById(ids, companyId);
                        foreach (var item in email)
                        {
                            OfficeEmails += item.OfficeEmail + ",";
                        }
                    }
                    else
                    {
                        var splitDesignationId = announcements.DesignationId.Split(',');
                        foreach (var data in splitDesignationId)
                        {
                            ids.Add(Convert.ToInt32(data));
                            if (ids[0] == 0)
                            {
                                OfficeEmails += await GetOfficeEmails(companyId);
                            }
                        }
                        var email = await _employeesRepository.GetDesignationById(ids, companyId);
                        foreach (var item in email)
                        {
                            OfficeEmails += item.OfficeEmail + ",";
                        }
                    }

                    OfficeEmails.Trim(new char[] { ',' });

                    var emailEntity = new EmailQueueEntity();
                    foreach (var item in document)
                    {
                        if (!string.IsNullOrEmpty(item.AttachmentName))
                        {
                            var fileName = item.AttachmentName;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Announcement");
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            combinePath.Add(Path.Combine(path, fileName));
                        }
                    }
                    var strSikillName = combinePath;
                    if (strSikillName.Count() > 0)
                    {
                        var str = string.Join(",", strSikillName);
                        emailEntity.Attachments = str.TrimEnd(',');
                    }
                    var names = await _employeesRepository.GetEmployeeByname(sessionEmployeeId, companyId);
                    var announcedBy = names.FirstName + "  " + names.LastName; ;
                    var draftTypeId = (int)EmailDraftType.Announcemeent;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var subject = emailDraftContentEntity.Subject;
                    var bodyContent = EmailBodyContent.SendEmail_Body_Announcement(announcements.AnnouncementName, announcements.Description, announcements.strAnnouncementDate, announcements.strAnnouncementEndDate, announcedBy, emailDraftContentEntity.DraftBody);
                    var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
                    emailEntity.FromEmail = emailSettingsEntity.FromEmail;
                    emailEntity.ToEmail = OfficeEmails;
                    emailEntity.Subject = subject;
                    emailEntity.Body = bodyContent;
                    emailEntity.CCEmail = emailDraftContentEntity.Email;
                    emailEntity.IsSend = false;
                    emailEntity.Reason = Common.Constant.Announcement;
                    emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                    emailEntity.CreatedDate = DateTime.Now;
                    await _companyRepository.InsertEmailQueueEntity(emailEntity);

                }
            }

            return announcement;
        }

        public async Task<string> GetOfficeEmails(int companyId)
        {
            string OfficeEmails = string.Empty;
            var employees = await _employeesRepository.GetAllEmployees(companyId);
            foreach (var item in employees)
            {
                OfficeEmails += item.OfficeEmail + ",";
            }
            return OfficeEmails.Trim();
        }


        /// <summary>
        ///Logic to get create the announcement detail
        /// </summary> 
        /// <param name="announcement" ></param>
        /// <param name="sessionEmployeeId" ></param>   
        public async Task<List<AnnouncementAttachmentsViewModel>> GetAnnouncementDocumentAndFilePath(int announcementId)
        {
            var attachmentsDocumentFilePaths = new List<AnnouncementAttachmentsViewModel>();
            var docNmaesAndFilePath = await _masterRepository.GetAnnouncementDocumentAndFilePath(announcementId);
            foreach (var item in docNmaesAndFilePath)
            {
                var attachmentsDocumentFilePath = new AnnouncementAttachmentsViewModel();
                attachmentsDocumentFilePath.Document = item.Document;
                attachmentsDocumentFilePath.AttachmentName = item.AttachmentName;
                attachmentsDocumentFilePath.AnnouncementId = item.AnnouncementId;
                attachmentsDocumentFilePaths.Add(attachmentsDocumentFilePath);
            }
            return attachmentsDocumentFilePaths;
        }

        /// <summary>
        /// Logic to get update isactive the announcement detail  by particular announcementid
        /// </summary>
        /// <param name="announcement" ></param> 
        public async Task<int> UpdateAnnouncement(Announcement announcement)
        {
            var announcementEntity = _mapper.Map<AnnouncementEntity>(announcement);
            announcementEntity.AnnouncementDate = DateTimeExtensions.ConvertToNotNullDatetime(announcement.strAnnouncementDate);
            announcementEntity.AnnouncementEndDate = DateTimeExtensions.ConvertToNotNullDatetime(announcement.strAnnouncementEndDate);
            await _masterRepository.UpdateAnnouncement(announcementEntity);
            return announcementEntity.AnnouncementId;
        }

        /// <summary>
        ///Logic to get create the announcement detail by particular announcementId
        /// </summary> 
        /// <param name="announcementId" ></param>
        public async Task<bool> DeleteAnnouncement(int announcementId,int companyId)
        {
            var result = await _masterRepository.DeleteAnnouncement(announcementId,companyId);
            return result;
        }

        //DashboardMenus

        /// <summary>
        ///Logic to get dashboardmenus detail list
        /// </summary> 
        public async Task<DashboardMenusViewModel> GetAllDashboardMenus(int sessionCompanyId)
        {
            var dashboardMenusViewModel = new DashboardMenusViewModel();
            dashboardMenusViewModel.DashboardMenus = await _masterRepository.GetAllDashboardMenus(sessionCompanyId);
            return dashboardMenusViewModel;
        }

        /// <summary>
        ///Logic to get check count menuName the DashboardMenus detail
        /// </summary>             
        /// <param name="menuName" ></param> 
        public async Task<int> GetMenuName(string menuName,int companyId)
        {
            var totalmenuName = await _masterRepository.GetMenuName(menuName,companyId);
            return totalmenuName;
        }

        /// <summary>
        ///Logic to get create the DashboardMenus detail
        /// </summary>             
        /// <param name="dashboardMenus" ></param>        
        public async Task<DashboardMenus> CreateDashboardMenu(DashboardMenus dashboardMenus)
        {
            var totalmenuName = await _masterRepository.GetMenuName(dashboardMenus.MenuName, dashboardMenus.CompanyId);
            var dashboardMenu = new DashboardMenus();
            if (totalmenuName == 0)
            {
                if (dashboardMenus != null)
                {
                    if (dashboardMenus.MenuId == 0)
                    {
                        var dashboardMenusEntity = _mapper.Map<DashboardMenusEntity>(dashboardMenus);
                        dashboardMenusEntity.IsActive = true;
                        var value = await _masterRepository.CreateDashboardMenus(dashboardMenusEntity);
                        dashboardMenu.MenuId = dashboardMenusEntity.MenuId;
                    }
                }
            }
            else
            {
                dashboardMenu.MenuNameCount = totalmenuName;
            }
            return dashboardMenu;
        }

        /// <summary>
        ///Logic to get update isactive the DashboardMenus detail by particular dashboardMenus
        /// </summary>             
        /// <param name="dashboardMenus" ></param> 
        public async Task<int> UpdateDashboardMenus(DashboardMenus dashboardMenus)
        {
            var dashboardMenusEntity = _mapper.Map<DashboardMenusEntity>(dashboardMenus);
            await _masterRepository.UpdateDashboardMenus(dashboardMenusEntity);
            var result = dashboardMenusEntity.MenuId;
            return result;
        }

        /// <summary>
        ///Logic to get delete the DashboardMenus detail by particular menuId
        /// </summary>             
        /// <param name="menuId" ></param>
        public async Task<bool> DeleteDashboardMenus(int menuId)
        {
            var result = await _masterRepository.DeleteDashboardMenus(menuId);
            return result;
        }

        //RelievingReason

        /// <summary>
        ///Logic to get relievingreason detail list
        /// </summary>                  
        public async Task<RelievingReasonViewModel> GetAllRelievingReason(int companyId)
        {
            var relievingReasonViewModel = new RelievingReasonViewModel();
            relievingReasonViewModel.RelievingReason = await _masterRepository.GetAllRelievingReasons(companyId);
            return relievingReasonViewModel;
        }

        /// <summary>
        /// Logic to get all relievingReasonName count check  the relievingReason list
        /// </summary> 
        /// <param name="relievingReasonName" ></param> 
        public async Task<int> GetRelievingReasonName(string relievingReasonName,int companyId)
        {
            var totalrelievingReasonName = await _masterRepository.GetRelievingReasonName(relievingReasonName, companyId);
            return totalrelievingReasonName;
        }

        /// <summary>
        /// Logic to get create RelievingReason details the by particular RelievingReason
        /// </summary> 
        /// <param name="RelievingReason" ></param>       
        public async Task<RelievingReason> CreateRelievingReasonNames(RelievingReason RelievingReason)
        {
            var totalrelievingReasonName = await _masterRepository.GetRelievingReasonName(RelievingReason.RelievingReasonName, RelievingReason.CompanyId);
            var relievingReason = new RelievingReason();
            if (totalrelievingReasonName == 0)
            {
                if (RelievingReason != null)
                {
                    if (RelievingReason.RelievingReasonId == 0)
                    {
                        var relievingReasonEntity = _mapper.Map<RelievingReasonEntity>(RelievingReason);
                        relievingReasonEntity.IsActive = true;
                        var value = await _masterRepository.CreateRelievingReasonName(relievingReasonEntity);
                        relievingReason.RelievingReasonId = relievingReasonEntity.RelievingReasonId;
                    }
                }
            }
            else
            {
                relievingReason.RelievingReasonNameCount = totalrelievingReasonName;
            }
            return relievingReason;
        }

        /// <summary>
        /// Logic to get edit isactive the relievingReason detail  by particular relievingReason
        /// </summary> 
        /// <param name="RelievingReason" ></param> 
        public async Task<int> UpdateRelievingReasonststus(RelievingReason relievingReason)
        {
            var relievingReasonEntity = _mapper.Map<RelievingReasonEntity>(relievingReason);
            await _masterRepository.UpdateRelievingReasonststus(relievingReasonEntity);
            var result = relievingReasonEntity.RelievingReasonId;
            return result;
        }

        /// <summary>
        /// Logic to get delete the relievingReason detail  by particular relievingReasonId
        /// </summary> 
        /// <param name="relievingReasonId" ></param> 
        public async Task<bool> DeleteRelievingReason(int relievingReasonId)
        {
            var result = await _masterRepository.DeleteRelievingReason(relievingReasonId);
            return result;
        }

        /// <summary>
        /// Logic to get update the relievingReason detail  by particular relievingReason
        /// </summary>
        /// <param name="relievingReason" ></param>
        public async Task<int> UpdateRelievingReason(RelievingReason relievingReason)
        {
            var relievingReasonEntity = _mapper.Map<RelievingReasonEntity>(relievingReason);
            await _masterRepository.UpdateRelievingReason(relievingReasonEntity);
            var leaveType = relievingReasonEntity.RelievingReasonId;
            return leaveType;
        }

        //TicketTypes

        /// <summary>
        /// Logic to get all the ticketTypes
        /// </summary>       
        public async Task<TicketTypesViewModel> GetAllTicketTypes(int companyId)
        {
            var ticketTypesViewModel = new TicketTypesViewModel();
            ticketTypesViewModel.ticketTypes = await _masterRepository.GetAllTicketTypes(companyId);
            return ticketTypesViewModel;
        }

        /// <summary>
        /// Logic to get create the ticketTypes
        /// </summary>  
        /// <param name="ticketType" ></param>
        public async Task<TicketTypes> CreateTicketType(TicketTypes ticketType)
        {
            var ticketTypes = new TicketTypes();
            var totalTicketName = await _masterRepository.GetTicketName(ticketType.TicketName,ticketType.CompanyId);
            if (totalTicketName == 0)
            {
                if (ticketType != null)
                {
                    if (ticketType.TicketTypeId == 0)
                    {
                        var ticketTypesEntity = _mapper.Map<TicketTypesEntity>(ticketType);
                        ticketTypesEntity.IsActive = true;
                        var value = await _masterRepository.CreateTicketType(ticketTypesEntity);
                        ticketTypes.TicketTypeId = ticketTypesEntity.TicketTypeId;
                    }
                }
            }
            else
            {
                ticketTypes.TicketNameCount = totalTicketName;
            }
            return ticketTypes;
        }

        /// <summary>
        /// Logic to get update the ticketTypes detail  by particular ticketTypes
        /// </summary>    
        /// <param name="ticketType" ></param>
        public async Task<TicketTypes> UpdateTicketType(TicketTypes ticketType)
        {
            var ticketTypes = new TicketTypes();
            if (ticketType != null)
            {
                var ticketTypesEntity = _mapper.Map<TicketTypesEntity>(ticketType);
                var value = await _masterRepository.CreateTicketType(ticketTypesEntity);
                ticketTypes.TicketTypeId = ticketTypesEntity.TicketTypeId;
            }
            return ticketTypes;
        }

        /// <summary>
        /// Logic to get delete the ticketTypes detail  by particular ticketTypes
        /// </summary>    
        /// <param name="ticketTypeId" ></param>
        public async Task<bool> DeleteTicketType(int ticketTypeId,int companyId)
        {
            var result = await _masterRepository.DeleteTicketType(ticketTypeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to all the ticketTypes detail get by particular ticketTypeId
        /// </summary>    
        /// <param name="ticketTypeId" ></param>
        public async Task<TicketTypes> GetByTicketTypeId(int ticketTypeId,int companyId)
        {
            var ticketType = new TicketTypes();
            var ticketTypesEntity = await _masterRepository.GetByTicketTypeId(ticketTypeId, companyId);
            if (ticketTypesEntity != null)
            {
                ticketType.TicketTypeId = ticketTypesEntity.TicketTypeId;
                ticketType.TicketName = ticketTypesEntity.TicketName;
                ticketType.IsActive = ticketTypesEntity.IsActive;

                ticketType.StrFmtReportingPersonId = !string.IsNullOrEmpty(ticketTypesEntity.ReportingPersonId)
                    ? string.Join(",", ticketTypesEntity.ReportingPersonId.Split(","))
                    : string.Empty;
            }

            return ticketType;
        }
    }
}
