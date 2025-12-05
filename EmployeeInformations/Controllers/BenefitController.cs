using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.BenefitViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class BenefitController : BaseController
    {
        private readonly IBenefitService _benefitService;

        public BenefitController(IBenefitService benefitService)
        {
            _benefitService = benefitService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logic to get all the benefit tab
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> BenefitTab()
        {
            HttpContext.Session.SetString("LastView", Constant.BenefitTab);
            HttpContext.Session.SetString("LastController", Constant.Benefit);
            return View();
        }
        /// <summary>
        /// Logic to get all the benefit tab Filtered data and count 
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        /// 
        [HttpGet]
        public async Task<IActionResult> BenefitFilter(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            HttpContext.Session.SetString("LastView", Constant.BenefitTab);
            HttpContext.Session.SetString("LastController", Constant.Benefit);
            var filter = await _benefitService.GetBenefitFilterView(companyId, pager, columnName, columnDirection);
            var filterCount = await _benefitService.BenefitCount(companyId, pager);
            return Json(new
            {
                iTotalRecords = filterCount,
                iTotalDisplayRecords = filterCount,
                data = filter,
            });
        }
        /// <summary>
        /// Logic to get all the benefittab list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBenefitTab()
        {
            var companyId = GetSessionValueForCompanyId;
            var benefits = await _benefitService.GetAllBenefitDetails(companyId);
            return PartialView("Benefit", benefits);
        }

        //Benifit

        /// <summary>
        ///  Logic to get upsert the employee benefit details
        /// </summary>
        /// <param name="employeeBenefit" ></param>

        public async Task<IActionResult> AddBenefits(EmployeeBenefit employeeBenefit)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            if (employeeBenefit != null)
            {
                result = await _benefitService.AddBenefits(employeeBenefit, sessionEmployeeId, companyId);
            }
            var benefits = await _benefitService.GetAllBenefitDetails(companyId);
            return new JsonResult(benefits);
        }

        /// <summary>
        /// Logic to get soft deleted the benefit detail  by particular benefit
        /// </summary>
        /// <param name="benefit" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteBenefit(EmployeeBenefit benefit)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            benefit.CompanyId = GetSessionValueForCompanyId;
            if (benefit != null)
            {
                result = await _benefitService.DeleteBenefit(benefit, sessionEmployeeId);
            }
            var benefits = await _benefitService.GetAllBenefitDetails(companyId);
            return new JsonResult(benefits);
        }

        /// <summary>
        /// Logic to get view the Benefit 
        /// </summary>
        /// <param name="benefitId" >Benefit</param>
        [HttpGet]
        public async Task<IActionResult> ViewEmployeeBenefits(int benefitId)
        {
            var companyId = GetSessionValueForCompanyId;
            var benefitsDetails = await _benefitService.GetBenefitsviewBenefitId(benefitId, companyId);
            return View(benefitsDetails);
        }

        //Medical Benefit

        /// <summary>
        /// Logic to get all the medical benefit tab list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMedicalBenefitTab()
        {
            var companyId = GetSessionValueForCompanyId;
            var medicalBenefits = await _benefitService.GetAllMedicalBenefitDetails(companyId);
            return PartialView("MedicalBenefit", medicalBenefits);
        }
        /// <summary>
        /// Logic to get all the MedicalBenefit Filtered data and count 
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        /// 
        [HttpGet]
        public async Task<IActionResult> MedicalBenefitFilter(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            var filter = await _benefitService.GetMedicalBenefitFilterView(companyId, pager, columnName, columnDirection);
            var filterCount = await _benefitService.MedicalBenefitCount(companyId, pager);
            return Json(new
            {
                iTotalRecords = filterCount,
                iTotalDisplayRecords = filterCount,
                data = filter,
            });
        }
        /// <summary>
        ///  Logic to upsert the employee Medical Benefit details
        /// </summary>
        /// <param name="employeeMedicalBenefit" ></param>

        public async Task<IActionResult> AddMedicalBenefits(EmployeeMedicalBenefit employeeMedicalBenefit)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (employeeMedicalBenefit != null)
            {
                var sessionEmployeeId = GetSessionValueForEmployeeId;
                result = await _benefitService.AddMedicalBenefits(employeeMedicalBenefit, sessionEmployeeId, companyId);
            }
            var medicalBenefits = await _benefitService.GetAllMedicalBenefitDetails(companyId);
            return new JsonResult(medicalBenefits);
        }

        /// <summary>
        /// Logic to get soft deleted the medicalbenefit detail by particular medicalbenefit
        /// </summary>  
        /// <param name="employeeMedicalBenefit" ></param> 
        [HttpPost]
        public async Task<IActionResult> DeleteMedicalBenefit(EmployeeMedicalBenefit employeeMedicalBenefit)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (employeeMedicalBenefit != null)
            {
                var sessionEmployeeId = GetSessionValueForEmployeeId;
                employeeMedicalBenefit.CompanyId = GetSessionValueForCompanyId;
                result = await _benefitService.DeleteMedicalBenefit(employeeMedicalBenefit, sessionEmployeeId);
            }
            var medicalBenefits = await _benefitService.GetAllMedicalBenefitDetails(companyId);
            return new JsonResult(medicalBenefits);
        }

        /// <summary>
        /// Logic to get view the MedicalBenefit 
        /// </summary>
        /// <param name="MedicalBenefitId" >MedicalBenefit</param>
        [HttpGet]
        public async Task<IActionResult> ViewEmployeeMedicalBenefits(int MedicalBenefitId)
        {
            var companyId = GetSessionValueForCompanyId;
            var benefitsDetails = await _benefitService.GetMedicalBenefitsviewBenefitId(MedicalBenefitId, companyId);
            return View(benefitsDetails);
        }
    }
}
