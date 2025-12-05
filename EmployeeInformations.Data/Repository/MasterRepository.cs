using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class MasterRepository : IMasterRepository
    {
        private readonly EmployeesDbContext _dbContext;
        public MasterRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        ///Designation

        /// <summary>
         /// Logic to get designations list
        /// </summary>         
        public async Task<List<Designation>> GetAllDesignation(int companyId)
        {
            var designations = await (from designation in _dbContext.Designations
                                      where !designation.IsDeleted && designation.CompanyId == companyId
                                      select new Designation()
                                      {
                                          DesignationId = designation.DesignationId,
                                          DesignationName = designation.DesignationName,
                                          IsActive = designation.IsActive,
                                      }).ToListAsync();
            return designations;
        }


        /// <summary>
        /// Logic to get create the designation detail
        /// </summary>   
        /// <param name="designationEntity" ></param>              
        public async Task<int> Create(DesignationEntity designationEntity)
        {
            var result = 0;
            if (designationEntity?.DesignationId == 0)
            {
                designationEntity.IsActive = true;
                await _dbContext.Designations.AddAsync(designationEntity);
                await _dbContext.SaveChangesAsync();
                result = designationEntity.DesignationId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update isactive the designation detail by particular designation
        /// </summary> 
        /// <param name="designationEntity" ></param>   
        /// <param name="designationId" ></param>              
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task UpdateDesignation(DesignationEntity designationEntity)
        {
            var departmentEntitys = await _dbContext.Designations.FirstOrDefaultAsync(j => j.DesignationId == designationEntity.DesignationId && !j.IsDeleted && j.CompanyId == designationEntity.CompanyId);
            if (departmentEntitys != null)
            {
                departmentEntitys.IsActive = designationEntity.IsActive;
                _dbContext.Designations.Update(departmentEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the designation detail by particular designation
        /// </summary> 
        /// <param name="designationId" ></param>              
        /// <param name="CompanyId" ></param> 
        public async Task<bool> DeletedDesignation(int designationId, int sessionCompanyId)
        {
            var result = false;
            var designationEntity = await _dbContext.Designations.FirstOrDefaultAsync(m => m.DesignationId == designationId && m.CompanyId == sessionCompanyId);
            if (designationEntity != null)
            {
                designationEntity.IsDeleted = true;
                _dbContext.Designations.Update(designationEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get designationId the designations detail by particular designations
        /// </summary>   
        /// <param name="designationId" ></param>       
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<DesignationEntity> GetDesignationByEmployeeId(int designationId, int companyId)
        {
            var designationEntity = await _dbContext.Designations.FirstOrDefaultAsync(c => !c.IsDeleted && c.DesignationId == designationId && c.CompanyId == companyId);
            return designationEntity ?? new DesignationEntity();
        }


        /// <summary>
        /// Logic to get count designationname the designation detail by particular designation
        /// </summary> 
        /// <param name="departmentname" ></param>              
        /// <param name="CompanyId" ></param> 
        public async Task<int> GetDesignationName(string designationName, int companyId)
        {
            var designationNameCount = await _dbContext.Designations.Where(y => y.DesignationName == designationName && y.CompanyId == companyId && y.IsDeleted == false).CountAsync();
            return designationNameCount;
        }

        /// Department

        /// <summary>
         /// Logic to get departmentId the department detail by particular department
        /// </summary>   
        /// <param name="departmentId" ></param>       
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<DepartmentEntity> GetDepartmentByEmployeeId(int departmentId, int companyId)
        {
            var departmentEntity = await _dbContext.Departments.FirstOrDefaultAsync(c => !c.IsDeleted && c.DepartmentId == departmentId && c.CompanyId == companyId);
            return departmentEntity ?? new DepartmentEntity();
        }


        /// <summary>
        /// Logic to get department list
        /// </summary>          
        public async Task<List<Department>> GetAllDepartment(int companyId)
        {
            var departments = await (from department in _dbContext.Departments
                                     where !department.IsDeleted && companyId == department.CompanyId
                                     select new Department()
                                     {
                                         DepartmentId = department.DepartmentId,
                                         DepartmentName = department.DepartmentName,
                                         IsActive = department.IsActive,
                                     }).ToListAsync();
            return departments;
        }

        /// <summary>
        /// Logic to get create the department detail
        /// </summary>   
        /// <param name="departmentEntity" ></param>
        public async Task<int> CreateDepartment(DepartmentEntity departmentEntity)
        {
            var result = 0;
            if (departmentEntity?.DepartmentId == 0)
            {
                departmentEntity.IsActive = true;
                await _dbContext.Departments.AddAsync(departmentEntity);
                await _dbContext.SaveChangesAsync();
                result = departmentEntity.DepartmentId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update isactive the department detail by particular department
        /// </summary> 
        /// <param name="departmentEntity" ></param>   
        /// <param name="DepartmentId" ></param>              
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task UpdateDepartment(DepartmentEntity departmentEntity)
        {
            var departmentEntitys = await _dbContext.Departments.FirstOrDefaultAsync(h => h.DepartmentId == departmentEntity.DepartmentId && !h.IsDeleted && h.CompanyId == departmentEntity.CompanyId);
            if (departmentEntitys != null)
            {
                departmentEntitys.IsActive = departmentEntity.IsActive;
                _dbContext.Departments.Update(departmentEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the department detail by particular department
        /// </summary> 
        /// <param name="departmentId" ></param>              
        /// <param name="CompanyId" ></param> 
        public async Task<bool> DeleteDepartment(int departmentId, int CompanyId)
        {
            var result = false;
            var departmentEntity = await _dbContext.Departments.FirstOrDefaultAsync(m => m.DepartmentId == departmentId && m.CompanyId == CompanyId);
            if (departmentEntity != null)
            {
                departmentEntity.IsDeleted = true;
                _dbContext.Departments.Update(departmentEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get count departmentname the department detail by particular department
        /// </summary> 
        /// <param name="departmentname" ></param>              
        /// <param name="CompanyId" ></param> 
        public async Task<int> GetDepartmentName(string departmentName, int companyId)
        {
            var DepartmentNameCount = await _dbContext.Departments.Where(x => x.DepartmentName == departmentName && x.CompanyId == companyId && x.IsDeleted == false).CountAsync();
            return DepartmentNameCount;
        }

        //Role

        /// <summary>
        /// Logic to get role list
        /// </summary>   
        public async Task<List<RoleTableMaster>> GetAllRoles(int companyId)
        {
            var roles = await (from role in _dbContext.RoleEntities
                               where !role.IsDeleted && companyId == role.CompanyId
                               select new RoleTableMaster()
                               {
                                   RoleId = (Role)role.RoleId,
                                   RoleName = role.RoleName,
                                   IsActive = role.IsActive,
                               }).ToListAsync();
            return roles;
        }


        /// <summary>
        /// Logic to get role the role detail by particular role
        /// </summary>   
        /// <param name="roleId" ></param>       
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<RoleEntity> GetRoleById(int roleId, int companyId)
        {
            var departmentEntity = await _dbContext.RoleEntities.FirstOrDefaultAsync(c => !c.IsDeleted && c.RoleId == roleId && c.CompanyId == companyId);
            return departmentEntity ?? new RoleEntity();
        }


        /// <summary>
        /// Logic to get create the role detail
        /// </summary>   
        /// <param name="roleEntity" ></param>
        public async Task<int> CreateRole(RoleEntity roleEntity)
        {
            try
            {
                var result = 0;
                if (roleEntity?.RoleId == 0)
                {
                    await _dbContext.RoleEntities.AddAsync(roleEntity);
                    await _dbContext.SaveChangesAsync();
                    result = roleEntity.RoleId;
                }
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Logic to get delete the role detail by particular role
        /// </summary>   
        /// <param name="roleId" ></param>       
        /// <param name="CompanyId" ></param>        
        public async Task<bool> DeleteRole(int roleId, int companyId)
        {
            var result = false;
            var roleEntity = await _dbContext.RoleEntities.FirstOrDefaultAsync(d => d.RoleId == roleId && d.CompanyId == companyId);
            if (roleEntity != null)
            {
                roleEntity.IsDeleted = true;
                _dbContext.RoleEntities.Update(roleEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete isactive the role detail by particular role
        /// </summary> 
        /// <param name="roleEntity" ></param> 
        /// <param name="roleId" ></param>       
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task DeleteIsActive(RoleEntity roleEntity)
        {
            var roleEntitys = await _dbContext.RoleEntities.FirstOrDefaultAsync(d => d.RoleId == roleEntity.RoleId && !d.IsDeleted && d.CompanyId == roleEntity.CompanyId);
            if (roleEntitys != null)
            {
                roleEntitys.IsActive = roleEntity.IsActive;
                _dbContext.RoleEntities.Update(roleEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get count rolename the role detail by particular role
        /// </summary> 
        /// <param name="rolename" ></param>              
        /// <param name="CompanyId" ></param>       
        public async Task<int> GetRoleName(string roleName, int companyId)
        {
            var roleNameCount = await _dbContext.RoleEntities.Where(x => x.RoleName == roleName && x.CompanyId == companyId).CountAsync();
            return roleNameCount;
        }

        // Module

        /// <summary>
        /// Logic to get modules list
        /// </summary>                
        public async Task<List<Modules>> GetAllModules(int companyId)
        {
            var modules = await (from module in _dbContext.Modules
                                 where !module.IsDeleted && module.CompanyId == companyId
                                 select new Modules()
                                 {
                                     Id = module.Id,
                                     Name = module.Name,
                                     IsActive = module.IsActive,
                                 }).ToListAsync();
            return modules;
        }

        /// <summary>
          /// Logic to get create the modules detail
          /// </summary> 
        /// <param name="modulesEntity" ></param>          
        public async Task<int> CreateModule(ModulesEntity modulesEntity)
        {
            var result = 0;
            if (modulesEntity?.Id == 0)
            {
                await _dbContext.Modules.AddAsync(modulesEntity);
                await _dbContext.SaveChangesAsync();
                result = modulesEntity.Id;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update the modules detail
        /// </summary> 
        /// <param name="modulesEntity" ></param> 
        public async Task UpdateModule(ModulesEntity modulesEntity)
        {
            var moduleEntity = await _dbContext.Modules.FirstOrDefaultAsync(x => x.Id == modulesEntity.Id && !x.IsDeleted && x.CompanyId == modulesEntity.CompanyId);
            if (moduleEntity != null)
            {
                moduleEntity.IsActive = modulesEntity.IsActive;
                _dbContext.Modules.Update(moduleEntity);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Logic to get check count the modules detail 
        /// </summary> 
        /// <param name="moduleName" >modules</param>           
        public async Task<int> GetModuleName(string moduleName)
        {
            var moduleNameCount = await _dbContext.Modules.Where(x => x.Name == moduleName).CountAsync();
            return moduleNameCount;
        }


        /// <summary>
        /// Logic to get delete the modules detail by particular modules
        /// </summary> 
        /// <param name="id" >modules</param> 
        public async Task<bool> DeleteModule(int id)
        {
            var result = false;
            var modulesEntity = await _dbContext.Modules.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (modulesEntity != null)
            {
                modulesEntity.IsDeleted = true;
                _dbContext.Modules.Update(modulesEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        // SubModules

        /// <summary>
        /// Logic to get submodules list
        /// </summary> 
        public async Task<List<SubModules>> GetAllSubModules(int companyId)
        {
            var subModules = await (from subModule in _dbContext.SubModulesEntitys
                                    join module in _dbContext.Modules on subModule.ModuleId equals module.Id
                                    where !subModule.IsDeleted && subModule.SubModuleId != (int)SubModule.ModuleList && subModule.SubModuleId != (int)SubModule.SubModuleList
                                    && subModule.CompanyId == companyId
                                    select new SubModules()
                                    {
                                        SubModuleId = subModule.SubModuleId,
                                        ModuleId = subModule.ModuleId,
                                        ModuleName = module.Name,
                                        IsActive = subModule.IsActive,
                                        Name = subModule.Name,
                                        CompanyId = subModule.CompanyId
                                    }).ToListAsync();
            return subModules;
        }

        /// <summary>
        /// Logic to get create the submodules detail
        /// </summary> 
        /// <param name="subModulesEntity" ></param> 
        public async Task<int> CreateSubModule(SubModulesEntity subModulesEntity)
        {
            var result = 0;
            if (subModulesEntity?.SubModuleId == 0)
            {
                await _dbContext.SubModulesEntitys.AddAsync(subModulesEntity);
                await _dbContext.SaveChangesAsync();
                result = subModulesEntity.SubModuleId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get check count the submodules detail 
        /// </summary> 
        /// <param name="name" >modules</param>
        public async Task<int> GetSubModulesName(string name, int companyId)
        {
            var subModuleNameCount = await _dbContext.SubModulesEntitys.Where(g => g.Name == name && g.CompanyId == companyId).CountAsync();
            return subModuleNameCount;
        }


        /// <summary>
        /// Logic to get modules list
        /// </summary> 
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<ModulesEntity>> GetAllModuleName(int companyId)
        {
            var modules = await _dbContext.Modules.Where(u => u.IsActive && !u.IsDeleted && u.CompanyId == companyId).ToListAsync();
            return modules;
        }


        /// <summary>
        /// Logic to get update the submodules detail by particular submodules
        /// </summary> 
        /// <param name="subModulesEntity" ></param>
        public async Task UpdateSubModule(SubModulesEntity subModulesEntity)
        {
            var subModulesEntitys = await _dbContext.SubModulesEntitys.FirstOrDefaultAsync(x => x.SubModuleId == subModulesEntity.SubModuleId && !x.IsDeleted);
            if (subModulesEntitys != null)
            {
                subModulesEntitys.IsActive = subModulesEntity.IsActive;
                _dbContext.SubModulesEntitys.Update(subModulesEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the submodules detail by particular submodules
        /// </summary> 
        /// <param name="subModuleId" >modules</param>
        public async Task<bool> DeleteSubModuleId(int subModuleId)
        {
            var result = false;
            var subModulesEntitys = await _dbContext.SubModulesEntitys.FirstOrDefaultAsync(d => d.SubModuleId == subModuleId);
            if (subModulesEntitys != null)
            {
                subModulesEntitys.IsDeleted = true;
                _dbContext.Update(subModulesEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        // ProjectTypes

        /// <summary>
        /// Logic to get projecttype list
        /// </summary>        
        public async Task<List<ProjectTypeMaster>> GetAllProjectTypes(int companyId)
        {
            var projectTypes = await (from projectType in _dbContext.ProjectTypes
                                      where !projectType.IsDeleted && companyId == projectType.CompanyId
                                      select new ProjectTypeMaster()
                                      {
                                          ProjectTypeId = projectType.ProjectTypeId,
                                          ProjectTypeName = projectType.ProjectTypeName,
                                      }).ToListAsync();
            return projectTypes;
        }


        /// <summary>
        /// Logic to get create the projecttype detail
        /// </summary> 
        /// <param name="projectTypesEntity" ></param>    
        public async Task<int> CreateProjectTypes(ProjectTypesEntity projectTypesEntity)
        {
            var result = 0;
            if (projectTypesEntity?.ProjectTypeId == 0)
            {
                await _dbContext.ProjectTypes.AddAsync(projectTypesEntity);
                await _dbContext.SaveChangesAsync();
                result = projectTypesEntity != null ? projectTypesEntity.ProjectTypeId : 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get check count the projecttype detail 
        /// </summary> 
        /// <param name="projectTypeName" >projecttype</param>
        public async Task<int> GetProjectTypes(string projectTypeName, int CompanyId)
        {
            var projectTypeNameCount = await _dbContext.ProjectTypes.Where(g => g.ProjectTypeName == projectTypeName && g.CompanyId == CompanyId && g.IsDeleted == false).CountAsync();
            return projectTypeNameCount;
        }


        /// <summary>
        /// Logic to get delete projectTypeId the projecttype detail by particular projecttype
        /// </summary> 
        /// <param name="projectTypeId" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeleteProjectTypeId(int projectTypeId, int companyId)
        {
            var result = false;
            var projectTypeEntitys = await _dbContext.ProjectTypes.FirstOrDefaultAsync(d => d.ProjectTypeId == projectTypeId && d.CompanyId == companyId);
            if (projectTypeEntitys != null)
            {
                projectTypeEntitys.IsDeleted = true;
                _dbContext.Update(projectTypeEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        //DocumentTypes

        /// <summary>
        /// Logic to get documenttype list
        /// </summary> 
        public async Task<List<DocumentType>> GetAllDocumentTypes(int companyId)
        {
            var documentTypes = await (from documentType in _dbContext.documentTypesEntities
                                       where !documentType.IsDeleted && companyId == documentType.CompanyId
                                       select new DocumentType()
                                       {
                                           DocumentTypeId = documentType.DocumentTypeId,
                                           DocumentName = documentType.DocumentName,
                                           IsActive = documentType.IsActive,
                                       }).ToListAsync();
            return documentTypes;
        }


        /// <summary>
        /// Logic to get delete documentTypeId the documenttype detail by particular documenttype
        /// </summary> 
        /// <param name="documentTypeId" >documenttype</param>
        /// <param name="CompanyId" >documenttype</param>
        public async Task DeletedDocumentTypeId(int documentTypeId, int companyId)
        {
            var DocumentTypeEntity = await _dbContext.documentTypesEntities.FirstOrDefaultAsync(k => k.DocumentTypeId == documentTypeId && k.CompanyId == companyId);
            if (DocumentTypeEntity != null)
            {
                DocumentTypeEntity.IsDeleted = true;
                _dbContext.Update(DocumentTypeEntity);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get create the documenttype detail
        /// </summary> 
        /// <param name="documentTypesEntity" ></param>
        public async Task<int> CreateDocumentType(DocumentTypesEntity documentTypesEntity)
        {
            var result = 0;
            if (documentTypesEntity?.DocumentTypeId == 0)
            {
                await _dbContext.documentTypesEntities.AddAsync(documentTypesEntity);
                await _dbContext.SaveChangesAsync();
                result = documentTypesEntity.DocumentTypeId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update isactive the documenttype detail by particular documenttype
        /// </summary> 
        /// <param name="documentTypesEntity" ></param>
        public async Task UpdateDocumentTypes(DocumentTypesEntity documentTypesEntity)
        {
            var documentTypesEntitys = await _dbContext.documentTypesEntities.FirstOrDefaultAsync(f => f.DocumentTypeId == documentTypesEntity.DocumentTypeId && !f.IsDeleted && f.CompanyId == documentTypesEntity.CompanyId);
            if (documentTypesEntitys != null)
            {
                documentTypesEntitys.IsActive = documentTypesEntity.IsActive;
                _dbContext.documentTypesEntities.Update(documentTypesEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get check count the documenttype detail 
        /// </summary> 
        /// <param name="documentName" >documenttype</param>
        /// <param name="CompanyId" >documenttype</param>
        public async Task<int> GetDocumentTypes(string documentName, int companyId)
        {
            var documentTypeNameCount = await _dbContext.documentTypesEntities.Where(g => g.DocumentName == documentName && g.CompanyId == companyId).CountAsync();
            return documentTypeNameCount;
        }

        //SkillSet

        /// <summary>
        /// Logic to get skillset list 
        /// </summary> 
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<SkillSetEntity>> GetAllSkillSet(int companyId)
        {
            return await _dbContext.SkillSets.Where(s => !s.IsDeleted && s.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get create the skillset detail 
        /// </summary> 
        /// <param name="skillSetEntity" ></param>        
        public async Task<int> CreateskillSet(SkillSetEntity skillSetEntity)
        {
            var result = 0;
            if (skillSetEntity?.SkillId == 0)
            {
                await _dbContext.SkillSets.AddAsync(skillSetEntity);
                await _dbContext.SaveChangesAsync();
                result = skillSetEntity.SkillId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update the skillset detail by particular skillset
        /// </summary> 
        /// <param name="skillSetEntity" ></param>
        /// <param name="CompanyId" >skillset</param>
        /// <param name="IsDeleted" >skillset</param>
        public async Task UpdateSkillSet(SkillSetEntity skillSetEntity)
        {
            var skillSetEntitys = await _dbContext.SkillSets.FirstOrDefaultAsync(f => f.SkillId == skillSetEntity.SkillId && !f.IsDeleted && f.CompanyId == skillSetEntity.CompanyId);
            if (skillSetEntitys != null)
            {
                skillSetEntitys.IsActive = skillSetEntity.IsActive;
                _dbContext.SkillSets.Update(skillSetEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete skillId the skillset detail by particular skillset
        /// </summary> 
        /// <param name="skillId" >skillset</param>
        /// <param name="CompanyId" >skillset</param>
        public async Task<bool> DeletedSkillSetId(int skillId,int companyId)
        {
            var result = false;
            var skillSetEntity = await _dbContext.SkillSets.FirstOrDefaultAsync(k => k.SkillId == skillId && k.CompanyId == companyId);
            if (skillSetEntity != null)
            {
                skillSetEntity.IsDeleted = true;
                _dbContext.Update(skillSetEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get check count the skillset detail
        /// </summary> 
        /// <param name="skillName" >skillset</param>
        /// <param name="CompanyId" >skillset</param>
        public async Task<int> GetSkillSet(string skillName, int companyId)
        {
            var skillSetCount = await _dbContext.SkillSets.Where(k => k.SkillName == skillName && k.CompanyId == companyId).CountAsync();
            return skillSetCount;
        }

        //Email Setting

        /// <summary>
        /// Logic to get emailsettings list
        /// </summary>                
        public async Task<EmailSettingsViewModel> GetAllEmailSettings(int companyId)
        {
            var emailSettings = await (from emailSetting in _dbContext.EmailSettings
                                       from sendmails in _dbContext.SendEmails
                                       where !emailSetting.IsDeleted && companyId == emailSetting.CompanyId
                                       select new EmailSettingsViewModel()
                                       {
                                           EmailSettingId = emailSetting.EmailSettingId,
                                           FromEmail = emailSetting.FromEmail,
                                           Host = emailSetting.Host,
                                           EmailPort = emailSetting.EmailPort,
                                           DisplayName = emailSetting.DisplayName,
                                           UserName = emailSetting.UserName,
                                           Password = Common.Common.Decrypt(emailSetting.Password),
                                           CompanyId = emailSetting.CompanyId,
                                           SendEmailsEntitys = _dbContext.SendEmails.Where(s => !s.IsDeleted && companyId == s.CompanyId).ToList(),
                                       }).FirstOrDefaultAsync();
            return emailSettings ?? new EmailSettingsViewModel();
        }

        /// <summary>
        /// Logic to get create  and update the emailsettings detail by particular emailsettings
        /// </summary>        
        /// <param name="emailSettingsEntity" ></param>       
        public async Task<int> CreateEmailSettings(EmailSettingsEntity emailSettingsEntity)
        {
            var result = 0;
            if (emailSettingsEntity?.EmailSettingId == 0)
            {
                await _dbContext.EmailSettings.AddAsync(emailSettingsEntity);
                await _dbContext.SaveChangesAsync();
                result = emailSettingsEntity.EmailSettingId;
            }
            else if (emailSettingsEntity?.EmailSettingId != null)
            {
                var emailEntity = new EmailSettingsEntity();
                emailEntity.FromEmail = emailSettingsEntity.FromEmail;
                emailEntity.Host = emailSettingsEntity.Host;
                emailEntity.EmailPort = emailSettingsEntity.EmailPort;
                emailEntity.UserName = emailSettingsEntity.UserName;
                emailSettingsEntity.Password = emailSettingsEntity.Password;
                _dbContext.EmailSettings.Update(emailSettingsEntity);
                await _dbContext.SaveChangesAsync();
                result = emailSettingsEntity.EmailSettingId;
            }
            return result;
        }

        /// <summary>
        /// Logic to get check count fromEmail the emailsetting detail 
        /// </summary>        
        /// <param name="fromEmail" >emailsetting</param>
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<int> GetEmailCount(string fromEmail, int companyId)
        {
            var emailCount = await _dbContext.EmailSettings.Where(x => x.FromEmail.ToLower() == fromEmail.ToLower() && !x.IsDeleted && x.CompanyId == companyId).CountAsync();
            return emailCount;
        }

        //send mail

        /// <summary>
        /// Logic to get update the sendemail detail by particular sendemail
        /// </summary>        
        /// <param name="emailListId" >sendemail</param>
        /// <param name="CompanyId" ></param>
        public async Task<SendEmailsEntity> GetSendEmailsByEmailListId(int emailListId,int companyId)
        {
            var sendEmailsEntity = await _dbContext.SendEmails.AsNoTracking().FirstOrDefaultAsync(g => g.EmailListId == emailListId && g.CompanyId == companyId);
            return sendEmailsEntity ?? new SendEmailsEntity();
        }

        /// <summary>
        /// Logic to get create and update  the sendemail detail by particular sendemail
        /// </summary>        
        /// <param name="sendEmailsEntity" ></param>       
        public async Task<int> AddSendEmails(SendEmailsEntity sendEmailsEntity)
        {
            var result = 0;
            if (sendEmailsEntity?.EmailListId == 0)
            {
                await _dbContext.SendEmails.AddAsync(sendEmailsEntity);
                await _dbContext.SaveChangesAsync();
                result = sendEmailsEntity.EmailSettingId;
            }
            else
            {
                if (sendEmailsEntity != null)
                {
                    _dbContext.SendEmails.Update(sendEmailsEntity);
                    await _dbContext.SaveChangesAsync();
                    result = sendEmailsEntity.EmailSettingId;
                }
            }
            return result;
        }

        /// <summary>
         /// Logic to get sendemails list
          /// </summary>         
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<SendEmailsEntity>> GetAllSendEmails(int companyId)
        {
            return await _dbContext.SendEmails.Where(c => !c.IsDeleted && c.CompanyId == companyId).ToListAsync();
        }

        /// <summary>
        /// Logic to get delete emailListId the sendemail detail by particular sendemail
        /// </summary>        
        /// <param name="emailListId" >sendemail</param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeleteSendEmail(int emailListId, int companyId)
        {
            var result = false;
            var sendEmailsEntity = await _dbContext.SendEmails.FirstOrDefaultAsync(d => d.EmailListId == emailListId && d.CompanyId == companyId);
            if (sendEmailsEntity != null)
            {
                sendEmailsEntity.IsDeleted = true;
                _dbContext.Update(sendEmailsEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
          /// Logic to get check count emailId the sendemail detail 
          /// </summary>        
        /// <param name="emailId" >sendemail</param>
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<int> GetSendEmailCount(string emailId,int companyId)
        {
            var emailCount = await _dbContext.SendEmails.Where(x => x.EmailId.ToLower() == emailId.ToLower() && !x.IsDeleted && x.CompanyId == companyId).CountAsync();
            return emailCount;
        }

        //LeaveTypes

        /// <summary>
        /// Logic to get leavetype list 
        /// </summary>             
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<LeaveTypeMaster>> GetAllLeaveTypes(int companyId)
        {
            var leaveTypes = await (from leavetype in _dbContext.leaveTypes
                                    where !leavetype.IsDeleted && companyId == leavetype.CompanyId
                                    select new LeaveTypeMaster
                                    {
                                        LeaveTypeId = leavetype.LeaveTypeId,
                                        LeaveType = leavetype.LeaveType,
                                        IsActive = leavetype.IsActive,
                                    }).ToListAsync();
            return leaveTypes;
        }

        public async Task<List<LeaveTypeMaster>> GetAllCompanyLeaveType()
        {
            var leaveTypes = await (from leavetype in _dbContext.leaveTypes
                                    where !leavetype.IsDeleted 
                                    select new LeaveTypeMaster
                                    {
                                        LeaveTypeId = leavetype.LeaveTypeId,
                                        LeaveType = leavetype.LeaveType,
                                        IsActive = leavetype.IsActive,
                                    }).ToListAsync();
            return leaveTypes;
        }

        /// <summary>
        /// Logic to get create the leavetype detail 
        /// </summary>             
        /// <param name="leaveTypesEntity" ></param>
        public async Task<int> CreateLeaveType(LeaveTypesEntity leaveTypesEntity, int companyId)
        {
            var result = 0;
            if (leaveTypesEntity.Id == 0)
            {
                leaveTypesEntity.CompanyId = companyId;
                await _dbContext.leaveTypes.AddAsync(leaveTypesEntity);
                await _dbContext.SaveChangesAsync();
                result = leaveTypesEntity.LeaveTypeId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get check count leaveType the leaveType detail 
        /// </summary>             
        /// <param name="leaveType" >leaveType</param>
        /// <param name="CompanyId" >leaveType</param>
        public async Task<int> GetLeaveType(string leaveType, int companyId)
        {
            var leaveTypeNameCount = await _dbContext.leaveTypes.Where(d => d.LeaveType == leaveType && d.CompanyId == companyId && !d.IsDeleted).CountAsync();
            return leaveTypeNameCount;
        }


        /// <summary>
        /// Logic to get update the leaveType detail by particular leaveType
        /// </summary>             
        /// <param name="leaveTypesEntity" ></param>     
        public async Task UpadateLeave(LeaveTypesEntity leaveTypesEntity, int companyId)
        {

            if (leaveTypesEntity != null)
            {
                leaveTypesEntity.CompanyId = companyId;
                _dbContext.leaveTypes.Update(leaveTypesEntity);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the leaveType detail by particular leaveType
        /// </summary>             
        /// <param name="leaveTypeId" >leaveType</param> 
        /// <param name="CompanyId" >leaveType</param> 
        public async Task<bool> DeleteLeaveType(int leaveTypeId, int companyId)
        {
            var result = false;
            var leaveTypesEntitys = await _dbContext.leaveTypes.FirstOrDefaultAsync(m => m.LeaveTypeId == leaveTypeId && m.CompanyId == companyId);
            if (leaveTypesEntitys != null)
            {
                leaveTypesEntitys.IsDeleted = true;
                _dbContext.leaveTypes.Update(leaveTypesEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        //Announcement
        /// <summary>
        /// Logic to get AnnouncementFilter data of the employees
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection"></param>
        public async Task<List<AnnouncementFilterViewModel>> AnnouncementFilter(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                var result = 0;
                var comId = Convert.ToString(companyId);
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param = new NpgsqlParameter("@companyId", comId);
                var param1 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", _params.Sorting);

                var data = await _dbContext.announcementFilter.FromSqlRaw("EXEC [dbo].[spGetAnnouncementFilter] @companyId,@pagingSize,@offsetValue,@searchText,@sorting", param, param1, param2, param3, param4).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///  Logic to get all AnnouncementFilterCount by SP
        /// </summary>
        /// <param name="companyId,pager" ></param>  
        public async Task<int> AnnouncementFilterCount(int companyId, SysDataTablePager pager)
        {
            try
            {
                var result = 0;

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {

                    SearchText = pager.sSearch
                };
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);

                List<AnnouncementFilterCount> announcementFilterCount = await _dbContext.announcementFilterCount.FromSqlRaw("EXEC [dbo].[spGetAnnouncementFilterCount] @companyId,@searchText", param1, param2).ToListAsync();
                foreach (var item in announcementFilterCount)
                {
                    result = item.Id;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        /// <summary>
        /// Logic to get announcement list
        /// </summary>             
        /// <param name="IsDeleted" ></param> 
        /// <param name="CompanyId" ></param>

        public async Task<List<Announcement>> GetAllAnnouncement(int companyId)
        {
            var announcements = await (from announcement in _dbContext.Announcement
                                       where !announcement.IsDeleted && announcement.CompanyId == companyId
                                       select new Announcement()
                                       {
                                           AnnouncementId = announcement.AnnouncementId,
                                           AnnouncementName = announcement.AnnouncementName,
                                           Description = announcement.Description,
                                           IsActive = announcement.IsActive,
                                           CompanyId = companyId,
                                           AssigneeName = Convert.ToString((AnnouncementAssignee)announcement.AnnouncementAssignee),
                                           AnnouncementDate = announcement.AnnouncementDate,
                                           AnnouncementEndDate = announcement.AnnouncementEndDate,
                                       }).ToListAsync();
            return announcements;
        }

        /// <summary>
        /// Logic to get announcement list
        /// </summary>             
        /// <param name="IsDeleted" ></param> 
        /// <param name="CompanyId" ></param>
        /// <param name="IsActive" ></param> 
        public async Task<List<AnnouncementEntity>> GetAllAnnouncementactive(int companyId)
        {
            return await _dbContext.Announcement.Where(m => !m.IsDeleted && m.IsActive && m.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get check count announcementName the announcement detail
        /// </summary>             
        /// <param name="announcementName" >announcement</param> 
        /// <param name="CompanyId" >announcement</param>
        public async Task<int> GetAnnouncementName(string announcementName,int companyId)
        {
            var announcementNameCount = await _dbContext.Announcement.Where(j => j.AnnouncementName == announcementName && j.CompanyId == companyId).CountAsync();
            return announcementNameCount;
        }


        /// <summary>
        /// Logic to get create the announcement detail
        /// </summary>             
        /// <param name="announcementEntity" ></param>       
        public async Task<int> CreateAnnouncement(AnnouncementEntity announcementEntity, int companyId)
        {
            var result = 0;
            if (announcementEntity?.AnnouncementId == 0)
            {
                announcementEntity.CompanyId = companyId;
                await _dbContext.Announcement.AddAsync(announcementEntity);
                await _dbContext.SaveChangesAsync();
                result = announcementEntity.AnnouncementId;
            }
            return result;
        }

        /// <summary>
        /// Logic to get delete announcementId the announcement detail by particular announcement
        /// </summary>             
        /// <param name="announcementId" >announcement</param> 
        /// <param name="CompanyId" >announcement</param> 
        public async Task<bool> DeleteAnnouncement(int announcementId,int companyId)
        {
            var result = false;
            var announcementEntity = await _dbContext.Announcement.FirstOrDefaultAsync(m => m.AnnouncementId == announcementId && m.CompanyId == companyId);
            if (announcementEntity != null)
            {
                announcementEntity.IsDeleted = true;
                _dbContext.Announcement.Update(announcementEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        ///  Logic to get announcementId the announcement detail by particular announcement
         /// </summary>             
        /// <param name="announcementId" >announcement</param> 
        /// <param name="CompanyId" >announcement</param> 
        public async Task<AnnouncementEntity> GetAnnouncementById(int announcementId,int companyId)
        {
            var announcementEntity = await _dbContext.Announcement.FirstOrDefaultAsync(m => m.AnnouncementId == announcementId && m.CompanyId == companyId);
            return announcementEntity;
        }


        /// <summary>
        /// Logic to get update the announcement detail by particular announcementId
        /// </summary>         
        /// <param name="announcementEntity" ></param>
        public async Task UpdateAnnouncement(AnnouncementEntity announcementEntitys)
        {
            try
            {
                var announcementEntity = await _dbContext.Announcement.FirstOrDefaultAsync(x => x.AnnouncementId == announcementEntitys.AnnouncementId && !x.IsDeleted);
                if (announcementEntity != null)
                {
                    announcementEntity.IsActive = announcementEntitys.IsActive;
                    announcementEntity.AnnouncementDate = announcementEntitys.AnnouncementDate;
                    announcementEntity.AnnouncementEndDate = announcementEntitys.AnnouncementEndDate;
                    _dbContext.Announcement.Update(announcementEntity);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Logic to get create and update the announcement Attachments detail by particular announcementId
        /// </summary>         
        /// <param name="announcementAttachments" ></param>
        /// <param name="announcementId" ></param>
        public async Task<bool> InsertAnnouncementAttachment(List<AnnouncementAttachmentsEntity> announcementAttachments, int announcementId)
        {
            try
            {
                var result = false;
                var attachmentsEntitys = await _dbContext.announcementAttachmentsEntities.Where(x => x.AnnouncementId == announcementId).ToListAsync();
                if (attachmentsEntitys.Count() == 0)
                {
                    await _dbContext.announcementAttachmentsEntities.AddRangeAsync(announcementAttachments);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                else
                {
                    _dbContext.announcementAttachmentsEntities.RemoveRange(attachmentsEntitys);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.announcementAttachmentsEntities.UpdateRange(announcementAttachments);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                return result;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<List<AnnouncementAttachmentsEntity>> GetAnnouncementDocumentAndFilePath(int announcementId)
        {
            var announcementAttachmentsEntity = await _dbContext.announcementAttachmentsEntities.Where(e => e.AnnouncementId == announcementId && !e.IsDeleted).ToListAsync();
            return announcementAttachmentsEntity;
        }


        //Dashboardmenus

        /// <summary>
        /// Logic to get dashboardMenus list
        /// </summary>             
        /// <param name="IsDeleted" ></param> 
        /// <param name="CompanyId" ></param>
        public async Task<List<DashboardMenus>> GetAllDashboardMenus(int sessionCompanyId)
        {

            var dashboardMenus = await (from dashboardMenu in _dbContext.DashboardMenusEntitys
                                        where !dashboardMenu.IsDeleted && sessionCompanyId == dashboardMenu.CompanyId
                                        select new DashboardMenus
                                        {
                                            MenuId = dashboardMenu.MenuId,
                                            MenuName = dashboardMenu.MenuName,
                                            IsActive = dashboardMenu.IsActive,
                                        }).ToListAsync();
            return dashboardMenus;
        }

        /// <summary>
        /// Logic to get check count menuName the DashboardMenusEntitys detail
        /// </summary>             
        /// <param name="menuName" >DashboardMenusEntitys</param> 
        /// <param name="CompanyId" >DashboardMenusEntitys</param>
        public async Task<int> GetMenuName(string menuName,int companyId)
        {
            var menuNameCount = await _dbContext.DashboardMenusEntitys.Where(j => j.MenuName == menuName && j.CompanyId == companyId).CountAsync();
            return menuNameCount;
        }


        /// <summary>
        /// Logic to get create the DashboardMenus detail
        /// </summary>             
        /// <param name="dashboardMenusEntity" ></param>       
        public async Task<int> CreateDashboardMenus(DashboardMenusEntity dashboardMenusEntity)
        {
            var result = 0;
            if (dashboardMenusEntity?.MenuId == 0)
            {
                await _dbContext.DashboardMenusEntitys.AddAsync(dashboardMenusEntity);
                await _dbContext.SaveChangesAsync();
                result = dashboardMenusEntity.MenuId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update isactive the DashboardMenus detail
        /// </summary> 
        /// <param name="dashboardMenusEntity" ></param> 
        public async Task UpdateDashboardMenus(DashboardMenusEntity dashboardMenusEntity)
        {
            var DashboardMenusEntitys = await _dbContext.DashboardMenusEntitys.FirstOrDefaultAsync(x => x.MenuId == dashboardMenusEntity.MenuId && !x.IsDeleted);
            if (DashboardMenusEntitys != null)
            {
                DashboardMenusEntitys.IsActive = dashboardMenusEntity.IsActive;
                _dbContext.DashboardMenusEntitys.Update(DashboardMenusEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the DashboardMenus detail by particular DashboardMenus
        /// </summary> 
        /// <param name="id" >DashboardMenusEntitys</param> 
        public async Task<bool> DeleteDashboardMenus(int menuId)
        {
            var result = false;
            var dashboardMenusEntity = await _dbContext.DashboardMenusEntitys.Where(m => m.MenuId == menuId).FirstOrDefaultAsync();
            if (dashboardMenusEntity != null)
            {
                dashboardMenusEntity.IsDeleted = true;
                _dbContext.DashboardMenusEntitys.Update(dashboardMenusEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        ///// Relieving Reason

        /// <summary>
         /// Logic to get relievingreasonentity list
         /// </summary>             
        /// <param name="IsDeleted" ></param> 
        /// <param name="CompanyId" ></param>
        public async Task<List<RelievingReasonEntity>> GetAllRelievingReason(int comapnyId)
        {
            return await _dbContext.RelievingReasonEntity.Where(m => !m.IsDeleted && m.CompanyId == comapnyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get check count relievingReasonName the RelievingReasonEntity detail
        /// </summary>             
        /// <param name="relievingReasonName" >RelievingReasonEntity</param> 
        /// <param name="CompanyId" >RelievingReasonEntity</param>
        public async Task<int> GetRelievingReasonName(string relievingReasonName,int companyId)
        {
            var relievingReasonNameCount = await _dbContext.RelievingReasonEntity.Where(j => j.RelievingReasonName == relievingReasonName && j.CompanyId == companyId).CountAsync();
            return relievingReasonNameCount;
        }


        /// <summary>
        /// Logic to get create the RelievingReason detail
        /// </summary>             
        /// <param name="relievingReasonEntity" ></param>       
        public async Task<int> CreateRelievingReasonName(RelievingReasonEntity relievingReasonEntity)
        {
            var result = 0;
            if (relievingReasonEntity?.RelievingReasonId == 0)
            {
                await _dbContext.RelievingReasonEntity.AddAsync(relievingReasonEntity);
                await _dbContext.SaveChangesAsync();
                result = relievingReasonEntity.RelievingReasonId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update the relievingReason detail by particular relievingReason
        /// </summary>             
        /// <param name="relievingReasonEntity" ></param>     
        public async Task UpdateRelievingReason(RelievingReasonEntity relievingReasonEntity)
        {
            if (relievingReasonEntity != null)
            {
                _dbContext.RelievingReasonEntity.Update(relievingReasonEntity);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get update isactive the relievingReason detail by particular relievingReason
        /// </summary> 
        /// <param name="relievingReasonEntity" ></param>   
        /// <param name="RelievingReasonId" ></param>              
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task UpdateRelievingReasonststus(RelievingReasonEntity relievingReasonEntity)
        {
            var relievingReasonEntitys = await _dbContext.RelievingReasonEntity.FirstOrDefaultAsync(h => h.RelievingReasonId == relievingReasonEntity.RelievingReasonId && !h.IsDeleted && h.CompanyId == relievingReasonEntity.CompanyId);
            if (relievingReasonEntitys != null)
            {
                relievingReasonEntitys.IsActive = relievingReasonEntity.IsActive;
                _dbContext.RelievingReasonEntity.Update(relievingReasonEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the relievingReason detail by particular relievingReason
        /// </summary> 
        /// <param name="id" >DashboardMenusEntitys</param> 
        public async Task<bool> DeleteRelievingReason(int relievingReasonId)
        {
            var result = false;
            var relievingReasonEntitys = await _dbContext.RelievingReasonEntity.Where(m => m.RelievingReasonId == relievingReasonId).FirstOrDefaultAsync();
            if (relievingReasonEntitys != null)
            {
                relievingReasonEntitys.IsDeleted = true;
                _dbContext.RelievingReasonEntity.Update(relievingReasonEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// get all announcement details for currentdate more than announcement date
        /// </summary>
        /// <returns></returns>
        public async Task<List<Announcements>> GetAnnouncement()
        {
            var currentDate = DateTime.Now;
            var announcements = await (from emp in _dbContext.Employees
                                       join anno in _dbContext.Announcement on emp.EmpId equals anno.CreatedBy
                                       where (anno.CreatedBy == emp.EmpId && !anno.IsDeleted && anno.AnnouncementEndDate.Date >= currentDate.Date && anno.AnnouncementDate.Date == currentDate.Date)
                                       select new Announcements()
                                       {
                                           AnnuncementId = anno.AnnouncementId,
                                           AnnouncementName = anno.AnnouncementName,
                                           AnnouncerName = emp.FirstName + " " + emp.LastName,
                                           Description = anno.Description,
                                           CreatedDate = anno.CreatedDate,
                                           AnnouncementDate = anno.AnnouncementDate.ToString(Constant.DateFormat),
                                           CreatedBy = anno.CreatedBy,
                                       }).Take(5).ToListAsync();
            return announcements;
        }
        /// <summary>
        /// get all relieving reasons for current company 
        /// </summary>
        /// <returns></returns>
        public async Task<List<RelievingReason>> GetAllRelievingReasons(int companyId)
        {
            var relievingReason = await (from relieving in _dbContext.RelievingReasonEntity
                                         where !relieving.IsDeleted && companyId == relieving.CompanyId
                                         select new RelievingReason
                                         {
                                             RelievingReasonId = relieving.RelievingReasonId,
                                             RelievingReasonName = relieving.RelievingReasonName,
                                             IsActive = relieving.IsActive,
                                         }).ToListAsync();
            return relievingReason;
        }

        /// <summary>
        /// get all department for announcement
        /// </summary>               
        public async Task<List<Department>> GetDepartment(int companyId)
        {
            var department = new Department();
            department.DepartmentId = 0;
            department.DepartmentName = Common.Constant.AllDepartment;
            var departmentDropdowns = new List<Department>();
            departmentDropdowns.Add(department);
            var departments = await (from departmentEntity in _dbContext.Departments
                                     where !departmentEntity.IsDeleted && companyId == departmentEntity.CompanyId
                                     select new Department()
                                     {
                                         DepartmentId = departmentEntity.DepartmentId,
                                         DepartmentName = departmentEntity.DepartmentName,
                                         IsActive = departmentEntity.IsActive,
                                     }).ToListAsync();
            departmentDropdowns.AddRange(departments);
            return departmentDropdowns;
        }

        /// <summary>
        /// get all designation for announcement
        /// </summary>   
        public async Task<List<Designation>> GetDesignation(int companyId)
        {
            var designation = new Designation();
            designation.DesignationId = 0;
            designation.DesignationName = Common.Constant.AllDesignation;
            var designationDropdowns = new List<Designation>();
            designationDropdowns.Add(designation);
            var designations = await (from designationEntity in _dbContext.Designations
                                      where !designationEntity.IsDeleted && companyId == designationEntity.CompanyId
                                      select new Designation()
                                      {
                                          DesignationId = designationEntity.DesignationId,
                                          DesignationName = designationEntity.DesignationName,
                                          IsActive = designationEntity.IsActive,
                                      }).ToListAsync();
            designationDropdowns.AddRange(designations);
            return designationDropdowns;
        }

        //TicketTypes

        /// <summary>
        /// get all ticketTypes
        /// </summary>
        public async Task<List<TicketTypes>> GetAllTicketTypes(int companyId)
        {
            var ticketTypes = await (from TicketTypes in _dbContext.TicketTypesEntity
                                     where !TicketTypes.IsDeleted && TicketTypes.CompanyId == companyId
                                     select new TicketTypes()
                                     {
                                         TicketTypeId = TicketTypes.TicketTypeId,
                                         TicketName = TicketTypes.TicketName,
                                         IsActive = TicketTypes.IsActive,
                                         CompanyId = TicketTypes.CompanyId,
                                     }).ToListAsync();
            return ticketTypes;
        }

        /// <summary>
        /// get all ticketTypes using helpdesk 
        /// </summary>
        public async Task<List<TicketTypes>> GetTicketTypes(int companyId)
        {
            var ticketTypes = await (from TicketTypes in _dbContext.TicketTypesEntity
                                     where !TicketTypes.IsDeleted && TicketTypes.IsActive && TicketTypes.CompanyId == companyId
                                     select new TicketTypes()
                                     {
                                         TicketTypeId = TicketTypes.TicketTypeId,
                                         TicketName = TicketTypes.TicketName,
                                         IsActive = TicketTypes.IsActive,
                                         CompanyId = TicketTypes.CompanyId,
                                     }).ToListAsync();
            return ticketTypes;
        }

        /// <summary>
        /// Logic to get count check the ticketTypes detail by particular ticketTypes
        /// </summary> 
        /// <param name="ticketName" ></param> 
        public async Task<int> GetTicketName(string ticketName,int companyId)
        {
            var TicketNameCount = await _dbContext.TicketTypesEntity.Where(x => x.TicketName == ticketName && !x.IsDeleted && x.CompanyId == companyId).CountAsync();
            return TicketNameCount;
        }

        /// <summary>
        /// Logic to get create and update the ticketTypes detail by particular ticketTypes
        /// </summary> 
        /// <param name="ticketTypesEntity" ></param> 
        public async Task<int> CreateTicketType(TicketTypesEntity ticketTypesEntity)
        {
            var result = 0;
            if (ticketTypesEntity?.TicketTypeId == 0)
            {
                await _dbContext.TicketTypesEntity.AddAsync(ticketTypesEntity);
                await _dbContext.SaveChangesAsync();
                result = ticketTypesEntity.TicketTypeId;
            }
            else
            {
                _dbContext.TicketTypesEntity.Update(ticketTypesEntity);
                await _dbContext.SaveChangesAsync();
                result = ticketTypesEntity.TicketTypeId;
            }
            return result;
        }

        /// <summary>
        /// Logic to get deleted the ticketTypes detail by particular ticketTypeId
        /// </summary> 
        /// <param name="ticketTypeId" ></param> 
        public async Task<bool> DeleteTicketType(int ticketTypeId,int companyId)
        {
            var result = false;
            var ticketTypeEntity = await _dbContext.TicketTypesEntity.FirstOrDefaultAsync(m => m.TicketTypeId == ticketTypeId && m.CompanyId == companyId);
            if (ticketTypeEntity != null)
            {
                ticketTypeEntity.IsDeleted = true;
                _dbContext.TicketTypesEntity.Update(ticketTypeEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get the ticketTypes detail by particular ticketTypeId
        /// </summary> 
        /// <param name="ticketTypeId" ></param> 
        public async Task<TicketTypesEntity> GetByTicketTypeId(int ticketTypeId,int companyId)
        {
            var ticketTypesEntity = await _dbContext.TicketTypesEntity.FirstOrDefaultAsync(x => x.TicketTypeId == ticketTypeId && !x.IsDeleted && companyId == x.CompanyId);
            return ticketTypesEntity ?? new TicketTypesEntity();
        }


    }
}
