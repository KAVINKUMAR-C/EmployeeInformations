using EmployeeInformations.Business.IService;
using EmployeeInformations.Model.EmployeeSettingViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{

    public class EmployeeSettingController : BaseController
    {
        private readonly IEmployeeSetting _employeeSetting;

        public EmployeeSettingController(IEmployeeSetting employeeSetting)
        {
            _employeeSetting = employeeSetting;
        }

        /// <summary>
        /// Logic to get the employeesettings details 
        /// </summary>   
        [HttpGet]
        public async Task<IActionResult> CreateSetting()
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _employeeSetting.GetEmployeeSetting(companyId);
            return View(result);
        }

        /// <summary>
        /// Logic to get create  and update the employeesettings detail 
        /// </summary>        
        /// <param name="employeeSetting" ></param> 
        [HttpPost]
        public async Task<bool> AddSetting(EmployeeSetting employeeSetting)
        {
            var sessionValue = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = await _employeeSetting.CreateSetting(employeeSetting, sessionValue,companyId);
            return result;
        }

    }
}
