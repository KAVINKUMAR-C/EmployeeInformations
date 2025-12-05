using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeInformations.Controllers
{
    public class LoginController : Controller
    {
        private readonly IEmployeesService _employeesService;
        private readonly ICompanyContext _companyContext;
        private readonly ICompanyPolicyService _companyPolicyService;
     

        public LoginController(IEmployeesService employeesService, ICompanyContext companyContext, ICompanyPolicyService companyPolicyService)
        {
            _employeesService = employeesService;
            _companyContext = companyContext;
            _companyPolicyService = companyPolicyService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companyId = 1;
            var model = await _employeesService.GetAllEmployees(companyId);
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RedirectToLogin()
        {
            return View();
        }

        /// <summary>
        /// Logic to get login the employee detail  by particular employee
        /// </summary>
        /// <param name="employee" ></param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel employees)
        {
            if (!ModelState.IsValid)
            {
                return View(employees);
            }
            var userDetails = await _employeesService.GetByUserName(employees);
            if (userDetails.EmpId != 0)
            {

                var claimsIdentity = new ClaimsIdentity(new[] { new Claim("UserName", employees.UserName), new Claim("EmployeeName", userDetails.UserName), }, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(1),
                        AllowRefresh = true
                    });

                SetCookie("UserName", employees.UserName, 1);
                SetCookie("EmployeeName", userDetails.UserName, 1);

                HttpContext.Session.SetInt32("EmpId", userDetails.EmpId);
                HttpContext.Session.SetInt32("CompanyId", userDetails.CompanyId);
                HttpContext.Session.SetInt32("RoleId", (int)userDetails.RoleId);
                HttpContext.Session.SetString("UserName", userDetails.EmployeeUserId);
                HttpContext.Session.SetString("EmployeeName", userDetails.UserName);
                HttpContext.Session.SetInt32("IsOnboarding", userDetails.IsOnboarding == true ? 1 : 0);
                var companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
                companyId = userDetails.CompanyId;

                var displayMode = await _companyPolicyService.GetCompanySettingsByCompanyId(userDetails.CompanyId);

                _companyContext.DisplayMode = displayMode;
                try
                {
                    var create = await _employeesService.CreateEmployeeActivityLog(userDetails.EmpId, companyId);
                }
                catch (Exception ex)
                {
                    Common.Common.WriteServerErrorLog(" Employee Login ActivityLog : " + userDetails.EmpId + " StackTrace : " + ex.StackTrace + " msg : " + ex.Message);
                }

                HttpContext.Session.SetInt32("DisplayId", displayMode);
                if (userDetails.ProfileInfo != null && userDetails.ProfileInfo.ProfileName != "")
                {
                    HttpContext.Session.SetString("ProfileName", "/ProfileImages/" + userDetails.ProfileInfo.ProfileName);
                }
                else
                {
                    HttpContext.Session.SetString("ProfileName", "/ProfileImages/" + "profile.jpeg");
                }
                if (!userDetails.IsActive)
                {
                    return RedirectToAction("ResetPassword", new { officeEmail = userDetails.OfficeEmail });
                }
                if (userDetails != null)
                {
                    if (userDetails.IsOnboarding == true)
                    {
                        if (userDetails.RoleId == (Role)employees.RoleId)
                        {
                            return RedirectToAction("EmployeeHome", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("Home", "Dashboard");
                        }
                    }
                    else
                    {
                        return RedirectToAction("WelcomeAboard", "OBEmployees");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(employees);
                }
            }
            else
            {
                ModelState.AddModelError("NotExistAccount", "");
                return View();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Logic to get officeEmail
        /// </summary>
        /// <param name="employee" ></param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string officeEmail)
        {
            await _employeesService.ChangePassword(officeEmail);
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult NewPassword(string UserName)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            var decriptUserName = Common.Common.Decrypt(UserName);
            loginViewModel.OfficeEmail = decriptUserName;
            return View(loginViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string officeEmail)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.OfficeEmail = officeEmail;
            return View(loginViewModel);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> NewPasswordChange(LoginViewModel loginViewModel)
        {
            await _employeesService.UpdateNewPassword(loginViewModel);
            return RedirectToAction("Login", "Login");
        }

        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Remove("UserName");
            Remove("EmployeeName");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<bool> EmployeeIsActiveCheck(string officeEmail)
        {
            var employeeIsActiveCheck = await _employeesService.EmployeeIsActiveCheck(officeEmail);
            return employeeIsActiveCheck;
        }
        /// <summary>
        /// Logic to view all the EmployeeActivityLog list for employees
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> EmployeeActivityLog()
        {           
            return View();
        }
        /// <summary>
        /// Logic to get all the EmployeeActivityLog list for employees
        /// </summary>
        /// <param name="pager, columnName, columnDirection" >leave</param>

        [HttpGet]
        public async Task<IActionResult> GetEmployeeActivityLogDetails(SysDataTablePager pager ,string columnName, string columnDirection)
        {
            var companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var employeelogs = await _employeesService.GetAllEmployeeActivitylogs(pager,columnName,columnDirection, companyId);
            var employeecounts = await _employeesService.GetAllEmployeeActivityLogfilterCount(pager, companyId);
            return Json(new
            {
                data = employeelogs.EmployeeActivityLogViewModels,
                iTotalRecords = employeecounts,
                iTotalDisplayRecords = employeecounts
            });

        }
    }
}

            
        