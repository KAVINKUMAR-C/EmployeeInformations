using EmployeeInformations.Business.IService;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IEmployeesService _employeesService;

        public CompanyController(ICompanyService companyService, IEmployeesService employeesService)
        {
            _companyService = companyService;
            _employeesService = employeesService;
        }

        /// <summary>
        /// Logic to get all the company list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Company()
        {
            return View();
        }

        /// <summary>
        /// Logic to get all the company list
        /// </summary>
        /// <param name="pager,columnDirection,ColumnName" ></param>
        [HttpGet]
        public async Task<IActionResult>GetAllCompany(SysDataTablePager pager, string columnDirection, string ColumnName)
        {
            var companyCount = await _companyService.GetCompanyListCount(pager);
            var company = await _companyService.GetAllCompanyList(pager, columnDirection, ColumnName);
            return Json(new
            {
                data = company.companyViewModels,
                iTotalDisplayRecords = companyCount,
                iTotalRecords = companyCount
            });
        }

        /// <summary>
        /// Logic to get all the company create page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CreateCompany()
        {
                var companys = new Company();
                companys.countrys = await _employeesService.GetAllCountry();
                companys.states = await _employeesService.GetAllStates();
                companys.cities = await _employeesService.GetAllCities();
                return View(companys);
        }

        /// <summary>
        /// Logic to get create the company detail by particular company
        /// </summary>
        /// <param name="company" ></param>
        [HttpPost]
        public async Task<int> AddCompany(Company company, IFormFile file)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            if (file != null && file.Name != "")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Companylogo");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                company.CompanyFilePath = Path.Combine(path, file.FileName);
                company.CompanyLogo = file.FileName;
                using (var stream = new FileStream(company.CompanyFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            var result = await _companyService.CreateCompany(company, sessionEmployeeId);
            return result;
        }

        /// <summary>
        /// Logic to get company detail by particular company
        /// </summary>
        /// <param name="companyId" >company</param>
        [HttpGet]
        public IActionResult EditCompany(int companyId)
        {
            var company = new Company();
            company.CompanyId = companyId;
            return PartialView("EditCompany", company);
        }

        /// <summary>
        /// Logic to get the company detail by particular company
        /// </summary>
        /// <param name="companyId" >company</param>
        [HttpGet]
        public async Task<IActionResult> UpdateCompany(int companyId)
        {
            var company = await _companyService.GetByCompanyId(companyId);
            company.states = await _employeesService.GetByCountryId(company.PhysicalCountryId);
            company.cities = await _companyService.GetByStateId(company.PhysicalAddressState);
            company.CompanyMailingstates = await _employeesService.GetByCountryId(company.MailingCountryId);
            company.CompanyMailingcities = await _companyService.GetByStateId(company.MailingAddressState);            
            company.countrys = await _employeesService.GetAllCountry();
            return PartialView("UpdateCompany", company);
        }

        /// <summary>
        /// Logic to get update the company detail by particular company
        /// </summary>
        /// <param name="company" >company</param>
        [HttpPost]
        public async Task<Int32> UpdateCompany(Company company, IFormFile file)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;

            if (file != null && file.Name != "")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Companylogo");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                company.CompanyFilePath = Path.Combine(path, file.FileName);
                company.CompanyLogo = file.FileName;
                using (var stream = new FileStream(company.CompanyFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            var result = await _companyService.CreateCompany(company, sessionEmployeeId);
            return result;
        }

        /// <summary>
        /// Logic to get state
        /// </summary>
        /// <param name="stateId" ></param>
        [HttpGet]
        public async Task<IActionResult> GetByStateId(int stateId)
        {
            var result = await _companyService.GetByStateId(stateId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get soft deleted the company detail  by particular company
        /// </summary>
        /// <param name="companyId" >company</param>
        [HttpPost]
        public async Task<IActionResult> DeleteCompany(int companyId)
        {
            var result = await _companyService.DeleteCompany(companyId);
            return Json(result);
        }

        /// <summary>
        /// Logic to get company details
        /// </summary>
        /// <param name="companyId" >company</param>
        public async Task<IActionResult> ViewCompany(int companyId)
        {
            var company = await _companyService.GetByViewCompanyId(companyId);           
            return View(company);
        }

        /// <summary>
        /// Logic to get branch location
        /// </summary>        
        public async Task<IActionResult> BranchLocation()
        {
            var companyId = GetSessionValueForCompanyId;
            var branchLocation = await _companyService.GetAllBranchLocation(companyId);
            return View(branchLocation);
        }

        /// <summary>
        /// Logic to get create the branchLocation detail  by particular branchLocationname
        /// </summary>
        /// <param name="branchLocation" ></param>       
        [HttpPost]
        public async Task<IActionResult> AddBranchLocations(BranchLocation branchLocation)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _companyService.Create(branchLocation, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get particular branchLocationname count
        /// </summary>
        /// <param name="branchLocationName" ></param>
        [HttpPost]
        public async Task<int> GetBranchLocationName(string branchLocationName)
        {
            var companyId = GetSessionValueForCompanyId;
            var branchLocationNameCount = await _companyService.GetBranchLocationName(branchLocationName, companyId);
            return branchLocationNameCount;
        }

        /// <summary>
        /// Logic to get update status the branchLocation detail  by particular branchLocation
        /// </summary>
        /// <param name="branchLocation" ></param>
        [HttpPost]
        public async Task<int> UpdateBranchStatus(BranchLocation branchLocation)
        {
            branchLocation.CompanyId = GetSessionValueForCompanyId;
            var result = await _companyService.UpdateBranchStatus(branchLocation);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the  branchLocation detail  by particular  branchLocation
        /// </summary>
        /// <param name="Id" > branchLocation</param>
        [HttpPost]
        public async Task<IActionResult> DeletedBranch(int branchLocationId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _companyService.DeletedBranch(branchLocationId, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get update the branchLocation detail  by particular branchLocation
        /// </summary>
        /// <param name="branchLocation" ></param>
        [HttpPost]
        public async Task<int> UpdateBranchLocation(BranchLocation branchLocation)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _companyService.UpdateBranchLocation(branchLocation, companyId);
            return result;
        }

        //MailScheduler

        /// <summary>
        /// Logic to get all the eMailScheduler list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> MailScheduler()
        {
            return View();
        }
        /// <summary>
        /// Logic to get all the eMailScheduler List
        /// </summary>
        /// <param name="pager,companyId,columnName, columnDirection" >leave</param>


        [HttpGet]
        public async Task<IActionResult> GetCompanyMailScheduler(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            var mailscheduler = await _companyService.GetAllEmailScheduler(pager,columnName,columnDirection,companyId);
            var mailschedulercounts = await _companyService.GetAllEmailSchedulerfilterCount(pager, companyId);
            return Json(new
            {
                data = mailscheduler.emailSchedulerViewModels,
                iTotalRecords = mailschedulercounts,
                iTotalDisplayRecords = mailschedulercounts
            });

        }

        /// <summary>
        /// Logic to company email settings
        /// </summary> 
        /// <param name="mailScheduler" ></param>
        [HttpGet]
        public async Task<IActionResult> CreateMailScheduler()
        {
            var companyId = GetSessionValueForCompanyId;
            var mailScheduler = new MailSchedulerViewModels();
            mailScheduler.DropdownEmployee = await _companyService.GetAllEmployees(companyId);
            return View(mailScheduler);
        }

        /// <summary>
        /// Logic to company email settings
        /// </summary> 
        /// <param name="mailScheduler" ></param>
        [HttpPost]
        public async Task<IActionResult> CreateMailScheduler(MailScheduler mailScheduler)
        {
            var result = false;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            mailScheduler.CompanyId = GetSessionValueForCompanyId;
            result = await _companyService.CreateMailScheduler(mailScheduler, sessionEmployeeId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get edit the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="schedulerId" >timesheet</param>
        [HttpGet]
        public IActionResult EditScheduler(int schedulerId)
        {
            var mailScheduler = new MailScheduler();
            mailScheduler.SchedulerId = schedulerId;
            return PartialView("EditScheduler", mailScheduler);
        }

        /// <summary>
        /// Logic to get update the timesheet detail  by particular timesheet
          /// </summary>
        /// <param name="TimeSheetId" >timesheet</param>
        [HttpGet]
        public async Task<IActionResult> UpdateMailScheduler(int schedulerId)
        {
            var companyId = GetSessionValueForCompanyId;
            var mailScheduler = await _companyService.GetMailSchedulerBySchedulerId(schedulerId);
            mailScheduler.DropdownEmployee = await _companyService.GetAllEmployees(companyId);
            return PartialView("UpdateMailScheduler", mailScheduler);
        }

        /// <summary>
        /// Logic to get update the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="timeSheet,file" ></param>

        [HttpPost]
        public async Task<bool> UpdateMailScheduler(MailScheduler mailScheduler)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            mailScheduler.CompanyId = GetSessionValueForCompanyId;
            var result = await _companyService.CreateMailScheduler(mailScheduler, sessionEmployeeId);
            return result;
        }


        /// <summary>
        /// Logic to Deleted Mail Schedule
        /// </summary> 
        /// <param name="mailScheduler" ></param>
        [HttpPost]
        public async Task<IActionResult> DeletedMailSchedule(int schedulerId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _companyService.DeletedMailSchedule(schedulerId,companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to Change Status Mail Schedule
        /// </summary> 
        /// <param name="mailScheduler" ></param>
        [HttpPost]
        public async Task<IActionResult> StatusMailSchedule(MailScheduler mailScheduler)
        {
            mailScheduler.CompanyId = GetSessionValueForCompanyId;
            var result = await _companyService.StatusMailSchedule(mailScheduler);
            return new JsonResult(result);
        }


        /// <summary>
        /// Logic to get company detail by particular company
        /// </summary>
        /// <param name="companyId" >company</param>
        [HttpGet]
        public IActionResult EditCompanySetting(int companyId)
        {
            var companySetting = new CompanySetting();
            companySetting.CompanyId = companyId;
            return PartialView("EditCompanySetting", companySetting);
        }

        /// <summary>
        /// Logic to get create the companySetting detail by particular companySetting
        /// </summary>
        /// <param name="Id" >companySetting</param>
        [HttpGet]
        public async Task<IActionResult> UpdateCompanySetting(int companyId)
        {
            var companySetting = await _companyService.GetByCompanySettingId(companyId);
            return PartialView("UpdateCompanySetting", companySetting);
        }

        /// <summary>
        /// Logic to get create and update the companySetting detail 
        /// </summary>
        /// <param name="companySetting" ></param>
        [HttpPost]
        public async Task<int> CreateCompanySetting(CompanySetting companySetting)
        {
            companySetting.CompanyId = GetSessionValueForCompanyId;
            var result = await _companyService.CreateCompanySettings(companySetting);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the  companysetting detail  by particular  companySettingId
        /// </summary>
        /// <param name="companySettingId" ></param>
        [HttpPost]
        public async Task<IActionResult> DeletedCompanySetting(int companySettingId)
        {
            var result = await _companyService.DeletedCompanySetting(companySettingId);
            return new JsonResult(result);
        }

    }
}
