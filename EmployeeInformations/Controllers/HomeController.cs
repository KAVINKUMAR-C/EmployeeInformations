using EmployeeInformations.Business.IService;
using EmployeeInformations.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        //public class HomeController : Controller
        //{
        //    private readonly IDashboardService dashboardService;
        //    public HomeController(IDashboardService dashboardService)
        //    {
        //        this.dashboardService = dashboardService;
        //    }

        //    [HttpGet]
        //    public async Task<IActionResult> DashBoard()
        //    {
        //        ViewBag.MenuScreen = "DashBoard";

        //        var dashboardDetails = await this.dashboardService.GetLiveTournaments();
        //        return View(dashboardDetails);
        //    }
        //}

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(string host, string path, string exmsg, string stacktrace)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            await _homeService.createError(host, path, exmsg, stacktrace, sessionEmployeeId, companyId);

            return View();
        }




    }
}