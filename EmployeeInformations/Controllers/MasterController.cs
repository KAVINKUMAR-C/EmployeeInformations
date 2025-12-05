using EmployeeInformations.Business.IService;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace EmployeeInformations.Controllers
{
    public class MasterController : BaseController
    {

        private readonly IMasterService _masterService;
        private readonly IEmployeesService _employeesService;

        public MasterController(IMasterService masterService, IEmployeesService employeesService)
        {
            _masterService = masterService;
            _employeesService = employeesService;
        }

        //Designation

        /// <summary>
        /// Logic to get all the designation list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Designation()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var designation = await _masterService.GetAllDesignation(sessionCompanyId);
            return View(designation);
        }

        [HttpGet]
        public IActionResult CreateDesignation()
        {
            var designation = new Designation();
            return View(designation);
        }

        /// <summary>
        /// Logic to get create designation detail  
        /// </summary>
        /// <param name="designation" ></param>     
        [HttpPost]
        public async Task<IActionResult> AddDesignation(Designation designation)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.Create(designation, sessionCompanyId);
            return new JsonResult(result); 
        }

        /// <summary>
        /// Logic to get designation name count
        /// </summary>
        /// <param name="designationName" ></param>
        [HttpPost]
        public async Task<int> GetDesignationName(string designationName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var designationNameCount = await _masterService.GetDesignationName(designationName, sessionCompanyId);
            return designationNameCount;
        }

        /// <summary>
        /// Logic to get soft deleted the designation detail  by particular designation
        /// </summary>
        /// <param name="designationId" >designation</param>
        [HttpPost]
        public async Task<IActionResult> DeletedDesignation(int designationId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeletedDesignation(designationId, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the designation detail  by particular designation
        /// </summary>
        /// <param name="designation" ></param>
        [HttpPost]
        public async Task<int> UpdateDesignation(Designation designation)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateDesignation(designation, sessionCompanyId);
            return result;
        }

        //Department

        /// <summary>
        /// Logic to get all the department list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Department()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var department = await _masterService.GetAllDepartment(sessionCompanyId);
            return View(department);
        }

        [HttpGet]
        public IActionResult CreateDepartment()
        {
            var department = new Department();
            return View(department);
        }

        /// <summary>
        /// Logic to get create department detail  
        /// </summary>
        /// <param name="department" ></param>       
        [HttpPost]
        public async Task<IActionResult> AddDepartments(Department department)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateDepartment(department, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get department name count
        /// </summary>
        /// <param name="departmentName" ></param>
        [HttpPost]
        public async Task<int> GetDepartmentName(string departmentName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var departmentNameCount = await _masterService.GetDepartmentName(departmentName, sessionCompanyId);
            return departmentNameCount;
        }

        /// <summary>
        /// Logic to get soft deleted the department detail  by particular department
        /// </summary>
        /// <param name="departmentId" >department</param>
        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeleteDepartment(departmentId, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the department detail  by particular department
        /// </summary>
        /// <param name="department" ></param>
        [HttpPost]
        public async Task<int> UpdateDepartment(Department department)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateDepartment(department, sessionCompanyId);
            return result;
        }

        /// <summary>
        /// Logic to get all the role list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Role()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var role = await _masterService.GetAllRole(sessionCompanyId);
            return View(role);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            var roletable = new RoleTable();
            return View(roletable);
        }

        /// <summary>
        /// Logic to get create role detail  
        /// </summary>
        /// <param name="roleTableMaster" ></param>      
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleTableMaster roleTableMaster)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateRole(roleTableMaster, sessionCompanyId);
            return new JsonResult(result);
        }
        /// <summary>
        /// Logic to get soft deleted the role detail  by particular role
        /// </summary>
        /// <param name="roleId" >role</param>
        [HttpPost]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeleteRole(roleId, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the role detail  by particular role
        /// </summary>
        /// <param name="roleTableMaster" ></param>
        [HttpPost]
        public async Task<int> DeleteIsActive(RoleTableMaster roleTableMaster)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeleteIsActive(roleTableMaster,sessionCompanyId);
            return result;
        }

        /// <summary>
        /// Logic to get roleName count
        /// </summary>
        /// <param name="roleName" ></param>
        [HttpPost]
        public async Task<int> GetRoleName(string roleName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var roleNameCount = await _masterService.GetRoleName(roleName, sessionCompanyId);
            return roleNameCount;
        }


        // Modules

        /// <summary>
        /// Logic to get all the modules list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Modules()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var modules = await _masterService.GetAllModules(sessionCompanyId);
            return View(modules);
        }

        /// <summary>
        /// Logic to get create modules detail  
        /// </summary>
        /// <param name="modules" ></param>        
        [HttpPost]
        public async Task<IActionResult> AddModule(Modules modules)
        {
            modules.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateModule(modules);
            return new JsonResult(result);
        }
        /// <summary>
        /// Logic to get edit isactive the modules detail  by particular modules
        /// </summary>
        /// <param name="modules" ></param>
        [HttpPost]
        public async Task<int> UpdateModule(Modules modules)
        {
            var result = await _masterService.UpdateModule(modules);
            return result;
        }

        /// <summary>
        /// Logic to get check moduleName the module detail  by particular moduleName not allow repeated moduleName
        /// </summary>
        /// <param name="moduleName" ></param>  
        [HttpPost]
        public async Task<int> GetModuleName(string moduleName)
        {
            var moduleNameCount = await _masterService.GetModuleName(moduleName);
            return moduleNameCount;
        }

        //SubModules

        /// <summary>
        /// Logic to get all the submodules list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SubModules()
        {
            var subModules = new SubModulesViewModel();
            var companyId = GetSessionValueForCompanyId; ;
            var moduleName = await _masterService.GetAllModuleName(companyId);
            subModules = await _masterService.GetAllSubModules(companyId);
            subModules.ModuleName = moduleName;
            return View(subModules);
        }

        /// <summary>
        /// Logic to get create subModules detail  
        /// </summary>
        /// <param name="subModules" ></param>        
        [HttpPost]
        public async Task<IActionResult> AddSubModule(SubModules subModules)
        {
            subModules.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateSubModules(subModules);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get check name the submodule detail  by particular name not allow repeated name
        /// </summary>
        /// <param name="name" ></param> 
        [HttpPost]
        public async Task<int> GetSubModulesName(string name)
        {
            var companyId = GetSessionValueForCompanyId;
            var SubModulesNameCount = await _masterService.GetSubModulesName(name, companyId);
            return SubModulesNameCount;
        }

        /// <summary>
        /// Logic to get soft deleted the submodule detail  by particular submodule
        /// </summary>
        /// <param name="subModuleId" >submodule</param>
        [HttpPost]
        public async Task<IActionResult> DeleteSubModuleId(int subModuleId)
        {
            var result = await _masterService.DeleteSubModuleId(subModuleId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get soft deleted the module detail  by particular module
        /// </summary>
        /// <param name="id" >module</param>
        [HttpPost]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var result = await _masterService.DeleteModule(id);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the submodules detail  by particular submodules
        /// </summary>
        /// <param name="subModules" ></param> 
        [HttpPost]
        public async Task<int> UpdateSubModule(SubModules subModules)
        {
            var result = await _masterService.UpdateSubModule(subModules);
            return result;
        }

        //ProjectTypes

        /// <summary>
        /// Logic to get all the projecttpyes list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ProjectTypes()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var projectTypes = await _masterService.GetAllProjectTypes(sessionCompanyId);
            return View(projectTypes);
        }

        /// <summary>
        /// Logic to get create projecttype detail  
        /// </summary>
        /// <param name="projectTypeMaster" ></param>         
        [HttpPost]
        public async Task<IActionResult> AddProjectType(ProjectTypeMaster projectTypeMaster)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateProjectType(projectTypeMaster, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get check projectTypeName the projectType detail  by particular projectTypeName not allow repeated projectTypeName
        /// </summary>
        /// <param name="projectTypeName" ></param> 
        [HttpPost]
        public async Task<int> GetProjectTypes(string projectTypeName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var projectTypeNamecount = await _masterService.GetProjectTypes(projectTypeName, sessionCompanyId);
            return projectTypeNamecount;
        }

        /// <summary>
        /// Logic to get soft deleted the projecttype detail  by particular projecttype
        /// </summary>
        /// <param name="projectTypeId" >projecttype</param>
        [HttpPost]
        public async Task<IActionResult> DeleteProjectTypeId(int projectTypeId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeleteProjectTypeId(projectTypeId, sessionCompanyId);
            return new JsonResult(result);
        }

        //DocumentTypes

        /// <summary>
        /// Logic to get all the documenttpyes list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DocumentTypes()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var documentTypes = await _masterService.GetAllDocumentTypes(sessionCompanyId);
            return View(documentTypes);
        }

        /// <summary>
        /// Logic to get soft deleted the documenttype detail  by particular documenttype
        /// </summary>
        /// <param name="documentTypeId" >documenttype</param>
        [HttpPost]
        public async Task<IActionResult> DeletedDocumentTypeId(int documentTypeId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            await _masterService.DeletedDocumentTypeId(documentTypeId, sessionCompanyId);
            var result = await _masterService.GetAllDocumentTypes(sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get create documenttype detail  
        /// </summary>
        /// <param name="documentType" ></param>       
        [HttpPost]
        public async Task<IActionResult> AddDocumentTypes(DocumentType documentType)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateDocumentTypes(documentType, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the documentType detail  by particular documentType
        /// </summary>
        /// <param name="documentType" ></param> 
        [HttpPost]
        public async Task<int> UpdateDocumentTypes(DocumentType documentType)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateDocumentTypes(documentType, sessionCompanyId);
            return result;
        }

        [HttpPost]
        public async Task<int> GetDocumentTypes(string documentName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var documentTypeNamecount = await _masterService.GetDocumentTypes(documentName, sessionCompanyId);
            return documentTypeNamecount;
        }

        //SkillSet

        /// <summary>
        /// Logic to get all the skillset list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SkillSet()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var skillSet = await _masterService.GetAllSkillSet(sessionCompanyId);
            return View(skillSet);
        }

        /// <summary>
        /// Logic to get soft deleted the skillset detail  by particular skillset
        /// </summary>
        /// <param name="skillId" >skillset</param>
        [HttpPost]
        public async Task<IActionResult> DeletedSkillSetId(int skillId)
        {

            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeletedSkillSetId(skillId, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get create skillSets detail  
        /// </summary>
        /// <param name="skillSets" ></param>        
        [HttpPost]
        public async Task<IActionResult> AddSkillSets(SkillSets skillSets)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var skillSet = await _masterService.CreateskillSets(skillSets, sessionCompanyId);
            return new JsonResult(skillSet);
        }

        [HttpPost]
        public async Task<int> GetSkillSet(string skillName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var skillNameCount = await _masterService.GetSkillSet(skillName, sessionCompanyId);
            return skillNameCount;
        }

        /// <summary>
        /// Logic to get edit isactive the skillSets detail  by particular skillSets
        /// </summary>
        /// <param name="skillSets" ></param>  
        [HttpPost]
        public async Task<int> UpdateSkillSet(SkillSets skillSets)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateSkillSet(skillSets, sessionCompanyId);
            return result;
        }

        //EmailSettings

        /// <summary>
        /// Logic to get all the emailsettings list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EmailSettings()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var emailSettings = await _masterService.GetAllEmailSettings(sessionCompanyId);
            return View(emailSettings);
        }

        /// <summary>
        /// Logic to get create and update emailsettings the emailSettings detail  
        /// </summary>
        /// <param name="emailSettings" ></param> 
        [HttpPost]
        public async Task<int> CreateEmailSettings(EmailSettings emailSettings)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateEmailSettings(emailSettings, sessionCompanyId);
            return result;
        }       

        /// <summary>
        /// Logic to get create and update sendemails the emailSettings detail  
        /// </summary>
        /// <param name="sendEmails" ></param> 
        [HttpPost]
        public async Task<Int32> CreateAddSendEmails(SendEmails sendEmails)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.AddSendEmails(sendEmails, sessionCompanyId);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the sendemail detail  by particular sendemail
        /// </summary>
        /// <param name="emailListId" >sendemail</param>
        [HttpPost]
        public async Task<IActionResult> DeleteSendEmail(int emailListId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeleteSendEmail(emailListId, sessionCompanyId);
            return new JsonResult(result);
        }


        [HttpPost]
        public async Task<int> GetEmailCount(string fromEmail)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var emailSettingsEmailCount = await _masterService.GetEmailCount(fromEmail, sessionCompanyId);
            return emailSettingsEmailCount;
        }

        [HttpPost]
        public async Task<int> GetSendEmailCount(string emailId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var sendEmailCount = await _masterService.GetSendEmailCount(emailId, sessionCompanyId);
            return sendEmailCount;
        }

        //leave type

        /// <summary>
        /// Logic to get all the leavetype list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LeaveType()
        {
            var companyId = GetSessionValueForCompanyId;
            var leaveType = await _masterService.GetAllLeaveTypes(companyId);
            return View(leaveType);
        }

        /// <summary>
        /// Logic to get create the leavetype detail  
        /// </summary> 
        /// <param name="leaveTypeMaster" ></param>       
        [HttpPost]
        public async Task<IActionResult> AddLeaveType(LeaveTypeMaster leaveTypeMaster)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateLeaveType(leaveTypeMaster,companyId);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<int> GetLeaveType(string leaveType)
        {
            var companyId = GetSessionValueForCompanyId;
            var leaveTypeCount = await _masterService.GetLeaveType(leaveType,companyId);
            return leaveTypeCount;
        }

        /// <summary>
        /// Logic to get edit the leavetype detail  by particular leavetype
        /// </summary>   
        /// <param name="leaveTypeMaster" ></param>       
        [HttpPost]
        public async Task<int> UpadateLeaveType(LeaveTypeMaster leaveTypeMaster)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpadateLeave(leaveTypeMaster,companyId);
            return result;
        }       

        /// <summary>
        /// Logic to get soft deleted the leavetype detail  by particular leavetype
        /// </summary>
        /// <param name="leaveTypeId" >leavetype</param>
        [HttpPost]
        public async Task<IActionResult> DeleteLeaveType(int leaveTypeId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeleteLeaveType(leaveTypeId,companyId);
            return new JsonResult(result);
        }

        //Announcement

        /// <summary>
        /// Logic to get all the announcement list
        /// </summary>
        public async Task<IActionResult> Announcement()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var announcement = await _masterService.GetAnnouncement(sessionCompanyId);
            return View(announcement);
        }

        /// <summary>
        /// Logic to get all the Announcement Filtered data and count 
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        [HttpGet]
        public async Task<IActionResult> AnnouncementFilter(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            var announcementFilter = await _masterService.AnnouncementFilter(companyId, pager, columnName, columnDirection);
            var announcementFilterCount = await _masterService.AnnouncementFilterCount(companyId, pager);
            return Json(new
            {
                iTotalRecords = announcementFilterCount,
                iTotalDisplayRecords = announcementFilterCount,
                data = announcementFilter,
            });
        }

        /// <summary>
        ///Logic to get check count announcementName the announcement detail
        /// </summary> 
        /// <param name="announcementName" ></param> 
        [HttpPost]
        public async Task<int> GetAnnouncementName(string announcementName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var announcementNameCount = await _masterService.GetAnnouncementName(announcementName, sessionCompanyId);
            return announcementNameCount;
        }

        /// <summary>
        /// Logic to get create the announcement detail  
        /// </summary> 
        /// <param name="announcement" ></param>     
        /// <param name="file" ></param>    
        [HttpPost]
        public async Task <IActionResult> AddAnnouncements(Announcement announcement, ICollection<IFormFile> file)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            announcement.announcementAttachments = new List<AnnouncementAttachmentsViewModel>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var announcementAttachment = new AnnouncementAttachmentsViewModel();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Announcement");
                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    announcementAttachment.Document = path.Replace(path, "~/Announcement/") + fileName;
                    announcementAttachment.AttachmentName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    announcement.announcementAttachments.Add(announcementAttachment);
                }
            }
            var announcements = await _masterService.CreateAnnouncements(announcement, sessionEmployeeId,companyId);
            return new JsonResult(announcements);
        }

        /// <summary>
        /// Logic to get soft deleted the announcement detail  by particular announcement
        /// </summary>
        /// <param name="announcementId" >announcement</param>
        [HttpPost]
        public async Task<IActionResult> DeleteAnnouncement(int announcementId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.DeleteAnnouncement(announcementId, sessionCompanyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the announcement detail  by particular announcementid
        /// </summary>
        /// <param name="announcement" ></param> 
        [HttpPost]
        public async Task<int> UpdateAnnouncement(Announcement announcement)
        {
             announcement.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateAnnouncement(announcement);
            return result;
        }


        /// <summary>
        /// Logic to get the download details AnnouncementAttachments by particular announcementId
        /// </summary>
        /// <param name="announcementId" ></param>
        public async Task<FileResult> DownloadAnnouncementFile(int announcementId)
        {
            var docNmaes = await  _masterService.GetAnnouncementDocumentAndFilePath(announcementId);
            var empUserName = string.Empty;

            if (docNmaes.Count() == 1)
            {
                foreach (var item in docNmaes)
                {
                    string path = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                    var bytes = System.IO.File.ReadAllBytes(path);
                    var file = File(bytes, "application/octet-stream", item.Document);
                    file.FileDownloadName = empUserName + "_" + item.AttachmentName;
                    return file;
                }
            }
            else
            {
                var zipName = empUserName + "_" + $"archive-AnnouncementFiles-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in docNmaes)
                        {
                            string fPath = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                            var entry = archive.CreateEntry(System.IO.Path.GetFileName(fPath), CompressionLevel.Fastest);
                            using (var zipStream = entry.Open())
                            {
                                var bytes = System.IO.File.ReadAllBytes(fPath);
                                zipStream.Write(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    return File(ms.ToArray(), "application/zip", zipName);
                }
            }
            return null;

        }

        //DashboardMenus

        /// <summary>
        /// Logic to get all the dashboardMenus list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DashboardMenus()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var dashboardMenus = await _masterService.GetAllDashboardMenus(sessionCompanyId);
            return View(dashboardMenus);
        }

        /// <summary>
        /// Logic to get all menuName count check  the dashboardMenus list
        /// </summary>
        [HttpPost]
        public async Task<int> GetMenuName(string menuName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var menuNameCount = await _masterService.GetMenuName(menuName, sessionCompanyId);
            return menuNameCount;
        }

        /// <summary>
        /// Logic to get create the dashboardMenus detail  
        /// </summary> 
        /// <param name="dashboardMenus" ></param>            
        [HttpPost]
        public async Task<IActionResult> AddDashboardMenu(DashboardMenus dashboardMenus)
        {
            dashboardMenus.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateDashboardMenu(dashboardMenus);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the dashboardMenus detail  by particular dashboardMenus
        /// </summary>
        /// <param name="dashboardMenus" ></param>
        [HttpPost]
        public async Task<int> UpdateDashboardMenus(DashboardMenus dashboardMenus)
        {
            dashboardMenus.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateDashboardMenus(dashboardMenus);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the DashboardMenus detail  by particular DashboardMenus
        /// </summary>
        /// <param name="id" >menuId</param>
        [HttpPost]
        public async Task<IActionResult> DeleteDashboardMenus(int menuId)
        {
            var result = await _masterService.DeleteDashboardMenus(menuId);
            return new JsonResult(result);
        }

        //RelievingReason

        /// <summary>
        /// Logic to get all the relievingReason list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RelievingReason()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var relievingReason = await _masterService.GetAllRelievingReason(sessionCompanyId);
            return View(relievingReason);
        }

        /// <summary>
        /// Logic to get all relievingReasonName count check  the relievingReason list
        /// </summary>
        [HttpPost]
        public async Task<int> GetRelievingReasonName(string relievingReasonName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var relievingReasonNameCount = await _masterService.GetRelievingReasonName(relievingReasonName, sessionCompanyId);
            return relievingReasonNameCount;
        }

        /// <summary>
        /// Logic to get create the relievingReason detail  
        /// </summary> 
        /// <param name="relievingReason" ></param>           
        [HttpPost]
        public async Task<IActionResult> AddRelievingReasons(RelievingReason relievingReason)
        {
            relievingReason.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.CreateRelievingReasonNames(relievingReason);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit isactive the relievingReason detail  by particular relievingReason
        /// </summary>
        /// <param name="relievingReason" ></param>
        [HttpPost]
        public async Task<int> UpdateRelievingReasonststus(RelievingReason relievingReason)
        {
            relievingReason.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateRelievingReasonststus(relievingReason);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the relievingReason detail  by particular relievingReason
        /// </summary>
        /// <param name="id" >relievingReasonId</param>
        [HttpPost]
        public async Task<IActionResult> DeleteRelievingReason(int relievingReasonId)
        {
            var result = await _masterService.DeleteRelievingReason(relievingReasonId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get update the relievingReason detail  by particular relievingReason
        /// </summary>
        /// <param name="relievingReason" ></param>
        [HttpPost]
        public async Task<int> UpdateRelievingReason(RelievingReason relieving)
        {
             relieving.CompanyId = GetSessionValueForCompanyId;
            var result = await _masterService.UpdateRelievingReason(relieving);
            return result;
        }

        // TicketTypes

        /// <summary>
        /// Logic to get all the TicketTypes details 
        /// </summary>        
        [HttpGet]
        public async Task <IActionResult> TicketTypes ()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var ticketTypes = await _masterService.GetAllTicketTypes(sessionCompanyId);
            ticketTypes.reportingPeople = await _employeesService.GetAllReportingPerson(sessionCompanyId);
            return View(ticketTypes);
        }

        /// <summary>
        /// Logic to get Create the TicketTypes detail by particular ticketType
        /// </summary>  
        /// <param name="ticketType" ></param>
        [HttpPost]
        public async Task<IActionResult> CreateTicketType (TicketTypes ticketType)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            ticketType.CompanyId = sessionCompanyId;
            var ticketTypes = await _masterService.CreateTicketType(ticketType);
            return new JsonResult(ticketTypes);
        }

        /// <summary>
        /// Logic to get Delete the TicketTypes detail by particular ticketTypeId
        /// </summary>  
        /// <param name="ticketTypeId" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteTicketType(int ticketTypeId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var ticketTypes = await _masterService.DeleteTicketType(ticketTypeId, sessionCompanyId);
            return new JsonResult(ticketTypes);
        }

        /// <summary>
        /// Logic to get Edit the TicketTypes detail by particular ticketTypeId
        /// </summary>  
        /// <param name="ticketTypeId" ></param>
        [HttpGet]
        public async Task<IActionResult> EditTicketType (int ticketTypeId)
        {
            var ticketTypes = new TicketTypes();
            ticketTypes.TicketTypeId = ticketTypeId;
            return View(ticketTypes);
        }

        /// <summary>
        /// Logic to get Update the TicketTypes detail by particular ticketTypeId
        /// </summary>  
        /// <param name="ticketTypeId" ></param>
        [HttpGet]
        public async Task<IActionResult> UpdateTicketType (int ticketTypeId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var ticketType = await _masterService.GetByTicketTypeId(ticketTypeId, sessionCompanyId);          
            ticketType.reportingPeople = await _employeesService.GetAllReportingPerson(sessionCompanyId);
            return View(ticketType);
        }

        /// <summary>
        /// Logic to get Update the TicketTypes detail by particular ticketTypeId
        /// </summary>  
        /// <param name="ticketTypes" ></param>
        [HttpPost]
        public async Task <IActionResult> UpdateTicketType (TicketTypes ticketTypes)
        {
            ticketTypes.CompanyId = GetSessionValueForCompanyId;
            var ticketType = await _masterService.UpdateTicketType(ticketTypes);
            return new JsonResult(ticketType);
        }
    }
}
