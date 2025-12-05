using EmployeeInformations.Business.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [AllowAnonymous]
    public class BackGroundController : BaseController
    {
        private readonly IBackGroundService _backGroundService;

  //      public BackGroundController(IBackGroundService backGroundService)
  //      {
  //          _backGroundService = backGroundService;
  //      }
  //      [HttpGet]
  //      public async Task<IActionResult> EmailQueueScheduler()
  //      {
  //          var emailQueue = await _backGroundService.EmailQueueScheduler();
  //          return new JsonResult(emailQueue);
  //      }

  //      [HttpGet]
  //      public async Task<IActionResult> EmployeesWorkAnniversary()
  //      {
  //          var workAnniversary = await _backGroundService.EmployeesWorkAnniversary();
  //          return new JsonResult(workAnniversary);
  //      }

  //      [HttpGet]
  //      public async Task<IActionResult> LeaveCalculation()
  //      {
  //          var leaveCalculation = await _backGroundService.GetLeaveCalculation();
  //          return new JsonResult(leaveCalculation);
  //      }

  //      [HttpGet]
  //      public async Task<IActionResult> EmployeeBirthday()
  //      {
  //          var employeeBirthday = await _backGroundService.EmployeeBirthday();
  //          return new JsonResult(employeeBirthday);
  //      }


        
		//[HttpGet]
		//public async Task<IActionResult> EmployeesProbation()
		//{
		//	var employeesprobation = await _backGroundService.EmployeesProbation();
		//	return new JsonResult(employeesprobation);
		//}


  //      [HttpGet]
  //      public async Task<IActionResult> AttendanceLog()
  //      {
  //          var attendanceLog = await _backGroundService.AttendanceLog();
  //          return new JsonResult(attendanceLog);
  //      }

  //      [HttpGet]
  //      public async Task<IActionResult> MailScheduler()
        
  //      {
  //          var companyId = GetSessionValueForCompanyId;
  //          var emailScheduler = await _backGroundService.EmailScheduler(companyId);
  //          return new JsonResult(emailScheduler);
  //      }
    }
}
