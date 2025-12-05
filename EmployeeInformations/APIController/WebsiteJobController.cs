using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Controllers;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;




namespace EmployeeInformations.APIController
{
    [CheckSessionIsAvailable]
    public class WebsiteJobController : BaseController
    {
        private readonly IWebsiteService _websiteService;
        private readonly IEmployeesService _employeesService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly int cachekey;

        public WebsiteJobController(IWebsiteService websiteService, IEmployeesService employeesService, IHttpClientFactory httpClient, IConfiguration config, IMemoryCache cache)
        {
            _websiteService = websiteService;
            _employeesService = employeesService;
            _clientFactory = httpClient;
            _config = config;
            _cache = cache;
        }
        public IActionResult Index()
        {
            return View();
        }
        //WebsiteJobPost

        /// <summary>
        /// Logic to get all the WebsiteJobPost list 
        /// </summary>
        /// 
        public async Task<IActionResult> WebsiteJobPost()
        {
            return View();
        }

        /// <summary>
        /// Logic to get all the WebsiteJobPost filter and count 
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        [HttpGet]
        public async Task<IActionResult> WebsiteJobPostFilter(SysDataTablePager pager, string columnName, string columnDirection)
        {
          
            var websiteJobPostFilter = await _websiteService.WebsiteJobPostFilter(pager, columnName, columnDirection);
            var websiteJobPostFilterCount = await _websiteService.WebsiteJobPostCount(pager);
            return Json(new
            {
                iTotalRecords = websiteJobPostFilterCount,
                iTotalDisplayRecords = websiteJobPostFilterCount,
                data = websiteJobPostFilter,
            });
        }


        [HttpGet]
        public async Task<IActionResult> CreateJobPost()
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteJobViewModel = new WebsiteJobViewModel();
            var jobCode = await _websiteService.GetWebsiteJobPostId();
            var listOfSkills = await _employeesService.GetAllSkills(companyId);
            websiteJobViewModel.skillSet = listOfSkills;
            websiteJobViewModel.JobCode = jobCode;
            return View(websiteJobViewModel);
        }

        [HttpPost]
        public async Task<bool> CreateWebsiteJobPost(WebsiteJobViewModel websiteJobViewModel)
        {
            var result = await _websiteService.InsertWebsiteJobPost(websiteJobViewModel);
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> EditJobPost(int jobId)
        {
            var websiteJobViewModel = new WebsiteJobViewModel();
            websiteJobViewModel.JobId = jobId;
            return PartialView("EditJobPost", websiteJobViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateJobPost(int jobId)
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteJobViewModel = new WebsiteJobViewModel();
            websiteJobViewModel.JobId = jobId;
            websiteJobViewModel = await _websiteService.GetWebsiteJobPostByJobId(jobId);
            var listOfSkills = await _employeesService.GetAllSkills(companyId);
            websiteJobViewModel.skillSet = listOfSkills;
            return PartialView("UpdateJobPost", websiteJobViewModel);
        }

        public async Task<bool> DeleteWebsiteJobPost(int jobId)
        {
            var result = await _websiteService.DeleteWebsiteJobPost(jobId);
            return result;
        }

        public async Task<bool> ChangeStatusWebsiteJobPost(int jobId, int status)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _websiteService.ChangeStatusWebsiteJobPost(jobId, status, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get all the WebsiteJobApply list 
        /// </summary>
     
        [HttpGet]
        public async Task<IActionResult> WebsiteJobApply()
        {
           // var websiteJobApplyModel = await _websiteService.GetAllJobApply();
            return View();
        }

        /// <summary>
        /// Logic to get all the WebsiteJobApply list 
        /// </summary>
        /// <param name="pager,columnDirection,ColumnName" >company</param>    
        [HttpGet]
        public async Task<IActionResult> GetAllWebsiteJobApplyDetails(SysDataTablePager pager, string columnDirection, string ColumnName)
        {
            var websiteJobApplyCount = await _websiteService.GetWebsideJobApplyListCount(pager);
            var websiteJobApply = await _websiteService.GetWebsideJobApplyList(pager, columnDirection, ColumnName);
            return Json(new
            {
                data = websiteJobApply.websiteJobApplyViewModels,
                iTotalDisplayRecords = websiteJobApplyCount,
                iTotalRecords = websiteJobApplyCount
            });
        }



        [HttpGet]
        public async Task<IActionResult> CreateSiteJopApply()
        {
            var companyId = GetSessionValueForCompanyId;
            var webSiteCreate = await _websiteService.CreateWebsiteJob(companyId);
            return View(webSiteCreate);
        }
        [HttpPost]
        public async Task<int> WebSiteJobApplyIsActive(int JobApplyId, bool IsActive)
        {
            var result = await _websiteService.WebSiteApplyJobIsActive(JobApplyId, IsActive);
            return result;
        }
        [HttpPost]
        public async Task<bool> AddSiteJopApply(WebsiteJobApplyModel websiteJobApply, IFormFile file)
        {
            var companyId = GetSessionValueForCompanyId;

            if (file != null && file.Name != "")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/WebSiteJobApply");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var combinedPath = Path.Combine(path, fileName);
                websiteJobApply.FilePath = combinedPath;
                using (var stream = new FileStream(combinedPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            var result = await _websiteService.AddSiteJopApply(websiteJobApply,companyId);
            return result;
        }

        public FileResult DownloadFile(string FilePath)
        {
            if (FilePath != null)
            {
                var filepath = FilePath.Replace("~", Directory.GetCurrentDirectory());
                var filename = FilePath.Split('/').Last();
                string path = Path.GetFullPath(filepath);
                //Read the File data into Byte Array.
                var bytes = System.IO.File.ReadAllBytes(path);
                var file = File(bytes, "application/octet-stream", filepath);

                file.FileDownloadName = filename;
                //Send the File to Download.
                return file;
            }
            return null;
        }


        public async Task<FileResult> DownloadFiles(int jobApplyId)
        {
            var jobApply = await _websiteService.GetWebsiteJobApplyId(jobApplyId);
            var filepath = jobApply.FilePath.Replace("~", Directory.GetCurrentDirectory());
            var filename = jobApply.FilePath.Split('/').Last();
            string path = Path.GetFullPath(filepath);
            //Read the File data into Byte Array.
            var bytes = System.IO.File.ReadAllBytes(path);
            var file = File(bytes, "application/octet-stream", filepath);
            file.FileDownloadName = jobApply.FullName + "_" + jobApply.SkillSet + "_" + filename;
            //Send the File to Download.
            return file;
        }

        [HttpPost]
        public async Task<bool> CreateSkills(SkillSet skill)
        {
            var result = await _websiteService.CreateSkills(skill);
            return result;
        }

        [HttpPost]
        public async Task<int> GetSkillCount(string SkillName)
        {
            var officeEmailCount = await _websiteService.GetSkill(SkillName);
            return officeEmailCount;
        }

        [HttpGet]
        public async Task<IActionResult> QuickQuote()
        {
            var role = await _websiteService.GetAllWebsiteQuote();
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> SoftwareConsultant()
        {
            var answersViewModel = await _websiteService.GetAllWebsiteSurveyAnswer();
            return View(answersViewModel);
        }


        //websitejobapply
        [HttpGet]
        public async Task<IActionResult> EditAppliedJob(int jobApplyId)
        {
            var websiteJobApplyModel = new WebsiteJobApplyModel();
            websiteJobApplyModel.JobApplyId = jobApplyId;
            return View(websiteJobApplyModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAppliedJob(int jobApplyId)
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteJobApplyModel = new WebsiteJobApplyModel();
            websiteJobApplyModel.JobApplyId = jobApplyId;
            websiteJobApplyModel = await _websiteService.GetWebsiteJobApplyId(jobApplyId);
            websiteJobApplyModel.WebsiteJobViewModels = await _websiteService.GetAllWebsiteJobPostName();
            websiteJobApplyModel.Skills = await _employeesService.GetAllSkills(companyId);
            return PartialView("UpdateAppliedJob", websiteJobApplyModel);
        }

        [HttpPost]
        public async Task<bool> UpdateWebsiteAppliedJob(WebsiteJobApplyModel websiteJobApplyModel, IFormFile file)
        {
            if (file != null && file.Name != "")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/WebSiteJobApply");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var combinedPath = Path.Combine(path, fileName);
                websiteJobApplyModel.FilePath = combinedPath;
                using (var stream = new FileStream(combinedPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            var result = await _websiteService.UpdateWebsiteAppliedJob(websiteJobApplyModel);
            return result;
        }
        //[HttpGet]
        //public async Task<IActionResult> WebsiteCandidateMenu()
        //{
        //    var websiteCandidateMenu = await _websiteService.GetWebsiteCandidateMenus();
        //    websiteCandidateMenu.DropdownEmployee = await _websiteService.GetAllEmployees();
        //    return View(websiteCandidateMenu);
        //}

        /// <summary>
        /// Logic to get all the WebsiteCandidateMenu list 
        /// </summary>        
        [HttpGet]
        public async Task<IActionResult> WebsiteCandidateMenu()
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteCandidateMenu = new WebsiteCandidateMenuModel();
            websiteCandidateMenu.DropdownEmployee = await _websiteService.GetAllEmployees(companyId);
            websiteCandidateMenu.WebsiteCandidateSchedules = await _websiteService.GetCandidateSechudule(companyId);
            return View(websiteCandidateMenu);
        }

        /// <summary>
        /// Logic to get all the WebsiteCandidateMenu list 
        /// </summary>
        /// <param name="pager,columnDirection,ColumnName" >company</param> 

        [HttpGet]
        public async Task<IActionResult> GetWebsiteCandidateMenu(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteCandidateMenu = await _websiteService.GetWebsiteCandidateMenu(pager, columnName, columnDirection,companyId); 
            var cntCandidateMenu = await _websiteService.GetWebsiteCandidateMenuCount(pager, companyId);
            return Json(new
            {
                data = websiteCandidateMenu,
                iTotalRecords = cntCandidateMenu,
                iTotalDisplayRecords = cntCandidateMenu
            });
        }

        [HttpGet]
        public async Task<IActionResult> CandidateSchedule()
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteCandidate = await _websiteService.CandidateSchedule(companyId);
            return View(websiteCandidate);
        }

        [HttpPost]
        public async Task<int> WebsiteUpdateCandidateStatus(WebsiteCandidateMenuModel websiteCandidateMenuModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _websiteService.WebsiteUpdateCandidateStatus(websiteCandidateMenuModel, companyId);
            return result;
        }


        [HttpGet]
        public async Task<IActionResult> CandidatePrivilege()
        {
            var companyId = GetSessionValueForCompanyId;
            var candidatePrivilege = await _websiteService.GetCandidatePrivileges(companyId);
            return View(candidatePrivilege);
        }

        [HttpPost]
        public async Task<IActionResult> AddDashboardAccessCategoryByRole(List<CandidatePrivilegeAssign> candidatePrivileges)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            var data = Convert.ToString(Request.Form["Candidate"]);
            var model = JsonConvert.DeserializeObject<List<CandidatePrivilegeAssign>>(data);
            if (model != null)
            {
                result = await _websiteService.AddCandidatePrivilegeByRoleId(model, companyId);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> CreateWebsiteSchedule(WebsiteCandidateSchedule websiteCandidateSchedule)
        {
            var companyId = GetSessionValueForCompanyId;
            var webSiteCreate = await _websiteService.CreateCandidateSchedule(websiteCandidateSchedule, companyId);
            return new JsonResult(webSiteCreate);
        }
        [HttpPost]
        public async Task<int> UpdateWebsiteSchedule(WebsiteCandidateSchedule websiteCandidateSchedule)
        {
            var webSiteCreate = await _websiteService.UpdateCandidateSchedule(websiteCandidateSchedule);
            return webSiteCreate;
        }

        [HttpPost]
        public async Task<int> DeleteWebsiteSchedule(int candidateId)
        {
            var result = await _websiteService.DeleteWebsiteSchedule(candidateId);
            return result;
        }

        [HttpPost]
        public async Task<bool> UpdateWebsiteCandidateMenu(WebsiteCandidateMenuModel websiteCandidateMenuModel)
        {

            var result = await _websiteService.UpdateWebsiteCandidateMenu(websiteCandidateMenuModel);

            return result;
        }
        [HttpGet]
        public async Task<IActionResult> WebsiteCandidateScheduleByid(int candidateId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _websiteService.GetCandidateById(candidateId, companyId);
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> SelectedCandidateMenu()
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteCandidateMenu = await _websiteService.GetWebsiteCandidateMenus(companyId);
            return View();
        }
        /// <summary>
        /// Logic to get WebsiteCandidateMenu selected details
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>
        [HttpGet]
        public async Task<IActionResult> SelectedCandidateMenuDetails(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var websiteCandidateMenu = await _websiteService.GetWebsiteCandidateMenusDetails(pager, columnName, columnDirection);
            var websiteCount = await _websiteService.GetWebsiteCandidateMenusCount(pager);
            return Json(new
            {
                iTotalRecords = websiteCount,
                iTotalDisplayRecords = websiteCount,
                data = websiteCandidateMenu.websiteCandidateMenuModels,
            });

        }

        /// <summary>
        /// Logic to get WebsiteCandidateMenu rejected details
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>
        [HttpGet]
        public async Task<IActionResult> RejectedCandidateMenu()
        {
            var companyId = GetSessionValueForCompanyId;
            var websiteCandidateMenu = await _websiteService.GetWebsiteCandidateMenus(companyId);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RejectedCandidateMenuDetails(SysDataTablePager pager, string columnName, string columnDirection)
        {
            
            var websiteCandidateMenu = await _websiteService.GetWebsiteCandidateRejectedDetails(pager, columnName, columnDirection);
            var websiteCount = await _websiteService.RejectedCandidateCount(pager);
            return Json(new
            {
                iTotalRecords = websiteCount,
                iTotalDisplayRecords = websiteCount,
                data = websiteCandidateMenu.websiteCandidateMenuModels,
            });
            return View(websiteCandidateMenu);
        }
        public async Task<IActionResult> OAuthCallback(string code, int candidateId)
        {
            var companyId = GetSessionValueForCompanyId;
            if (candidateId != 0)
            {
                HttpContext.Session.SetInt32("CandidateMenuId", candidateId);
            }

            var canId = GetSessionValueForCandidateId;

            if (code == null)
            {
                var redirectUri = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("RedirectUrl").Value);
                var clientId = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("ClientId").Value);
                var Oauthurl = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("RedirectUrlWithId").Value);
                string zoomAuthUrl = $"{Oauthurl}{clientId}&redirect_uri={redirectUri}";
                return Redirect(zoomAuthUrl);
            }
            var generatelink = await _websiteService.WebsiteCandidateZoomMeetingSchedule(canId, code, companyId);
            return RedirectToAction("WebsiteCandidateMenu", "WebsiteJob");

        }

    }
}
