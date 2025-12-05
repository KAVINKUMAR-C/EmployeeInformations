using EmployeeInformations.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Get Session Value And Set Common Use
        public int GetSessionValueForEmployeeId
        {
            get
            {
                int value = Convert.ToInt32(HttpContext.Session.GetInt32("EmpId"));
                return value == 0 ? 0 : value;
            }
        }

        //Get Session Value And Set Common Use
        public int GetSessionValueForCompanyId
        {
            get
            {
                int value = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
                return value == 0 ? 0 : value;
            }
        }

        //Get Session Value And Set Common Use
        public int GetSessionValueForRoleId
        {
            get
            {
                int value = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                return value == 0 ? 0 : value;
            }
        }

        // WebSite Job Apply

        public int GetSessionValueForCandidateId
        {
            get
            {
                int value = Convert.ToInt32(HttpContext.Session.GetInt32("CandidateMenuId"));
                return value == 0 ? 0 : value;
            }
            set
            {
                HttpContext.Session.SetInt32("CandidateMenuId", value);
            }
        }

      // Access Token 

        public  string GetSessionValueForAccessToken
        {
            get
            {
                string value = Convert.ToString(HttpContext.Session.GetString("accessToken"));
                return string.IsNullOrEmpty(value) ? string.Empty : value;
            }
            set
            {
                HttpContext.Session.SetString("accessToken", value);
            }
        }
    }
}
