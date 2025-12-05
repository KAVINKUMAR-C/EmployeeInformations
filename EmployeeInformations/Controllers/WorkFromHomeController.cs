using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    public class WorkFromHomeController : BaseController
    {
        private readonly ILeaveService _leaveService;
        private readonly IEmployeesService _employeesService;
        public WorkFromHomeController(ILeaveService leaveService, IEmployeesService employeesService)
        {
            _leaveService = leaveService;
            _employeesService = employeesService;
        }

        //WorkFromHome

        /// <summary>
        /// Logic to get all the WorkFromHome list of the employees
        /// </summary>
        /// 
        public async Task<IActionResult> WorkFromHome()
        {
            HttpContext.Session.SetString("LastView", Constant.WorkFromHome);
            HttpContext.Session.SetString("LastController", Constant.WorkFromHome);
            return View();
        }

       
       
        /// <summary>
        /// Logic to get WorkFromHome Filtered data and count of the employees
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>
        [HttpGet]
        public async Task<IActionResult> WorkFromHomeFilter(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
            var companyId = GetSessionValueForCompanyId;
            var wfhFilter = await _leaveService.GetWorkFromHomeFilterDataForTeamLead(empId, companyId, pager, columnName,columnDirection);
            var wfhFilterCount = await _leaveService.WorkFromHomeForTeamLeadCount(empId, companyId, pager);
            return Json(new
            {
                iTotalRecords = wfhFilterCount,
                iTotalDisplayRecords = wfhFilterCount,
                data = wfhFilter,
            });
        }

        //[HttpGet]
        //public async Task<IActionResult> CompensatoryOff()
        //{
        //    var sessionRoleId = GetSessionValueForRoleId;
        //    var sessionEmployeeId = GetSessionValueForEmployeeId;
        //    var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
        //    var leaveSummary = await _leaveService.GetAllCompensatoryOffSummary(empId);
        //    return View(leaveSummary);
        //}        

        /// <summary>
        /// Logic to get all the CreateWorkFromHome list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CreateWorkFromHome()
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var workFromHome = new EmployeeLeaveViewModel();
            var workfromhome = await _leaveService.GetAllLeave();
            workFromHome.LeaveType = workfromhome.Where(x => x.LeaveTypeId == 7).ToList();
            workFromHome.EmpId = sessionEmployeeId;
            return View(workFromHome);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateWorkFromHome(EmployeeLeaveViewModel leave, IFormFile file)
        //{
        //     var sessionEmployeeId = GetSessionValueForEmployeeId;
        //    if (file != null && file.Name != "")
        //    {
        //        //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/LeaveAttachments");

        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/LeaveAttachments");
        //        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        //        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //        var combinedPath = Path.Combine(path, fileName);
        //        leave.LeaveFilePath = path.Replace(path, "~/LeaveAttachments/") + fileName;
        //        leave.LeaveName = fileName;
        //        using (var stream = new FileStream(combinedPath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //        }
        //    }
        //    var result = await _leaveService.CreateLeave(leave, sessionEmployeeId);
        //    return new JsonResult(result);


        //    //var qualificationAttachment = new QualificationAttachment();
        //    //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Qualifications");
        //    ////create folder if not exist
        //    //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        //    //var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
        //    //var combinedPath = Path.Combine(path, fileName);
        //    //qualificationAttachment.Document = path.Replace(path, "~/Qualifications/") + fileName;
        //    //qualificationAttachment.QualificationName = fileName;
        //    //using (var stream = new FileStream(combinedPath, FileMode.Create))
        //    //{
        //    //    item.CopyTo(stream);
        //    //}
        //    //qualification.QualificationAttachments.Add(qualificationAttachment);
        //}        

        /// <summary>
        /// Logic to get download the leave detail  by particular employee leave
        /// </summary> 
        /// <param name="appliedLeaveId" >leave</param> 
        public async Task<FileResult> DownloadFile(int appliedLeaveId)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var leaveSummary = await _leaveService.GetLeaveByAppliedLeaveId(appliedLeaveId,companyId);
            if (leaveSummary != null)
            {
                var empUserName = string.Empty;
                var empId = leaveSummary.EmpId;
                var getUserName = await _employeesService.GetEmployeeById(empId, sessionEmployeeId);
                empUserName = getUserName.UserName;
                string path = !string.IsNullOrEmpty(leaveSummary.LeaveFilePath) ? leaveSummary.LeaveFilePath.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/")) : string.Empty;
                //Read the File data into Byte Array.
                var bytes = System.IO.File.ReadAllBytes(path);
                var file = File(bytes, "application/octet-stream", leaveSummary.LeaveFilePath);
                file.FileDownloadName = empUserName + "_" + leaveSummary.LeaveFilePath;
                //Send the File to Download.
                return file;
            }
            return null;

        }

        /// <summary>
        /// Logic to get update the leave detail  by particular leave
        /// </summary>
        /// <param name="AppliedLeaveId" >leave</param>
        [HttpGet]
        public async Task<IActionResult> UpdateWorkFromHome(int AppliedLeaveId)
        {
            var companyId = GetSessionValueForCompanyId;
            var leaveSummary = await _leaveService.GetLeaveByAppliedLeaveId(AppliedLeaveId,companyId);
            var workfromhome = await _leaveService.GetAllLeave();
            leaveSummary.LeaveType = workfromhome.Where(x => x.LeaveTypeId == 7).ToList();
            return PartialView("UpdateWorkFromHome", leaveSummary);
        }

        /// <summary>
        /// Logic to get edit the leave detail  by particular leave
        /// </summary>
        /// <param name="AppliedLeaveId" >leave</param>
        [HttpGet]
        public IActionResult ChangeWorkFromHome(int AppliedLeaveId)
        {
            var employeeLeaveViewModel = new EmployeeLeaveViewModel();
            employeeLeaveViewModel.AppliedLeaveId = AppliedLeaveId;
            return PartialView("WorkFromHomemanagement", employeeLeaveViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWorkFromHome(EmployeeLeaveViewModel leave, IFormFile file)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            if (file != null && file.Name != "")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/LeaveAttachments");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var combinedPath = Path.Combine(path, fileName);
                leave.LeaveFilePath = path.Replace(path, "~/LeaveAttachments/") + fileName;
                leave.LeaveName = fileName;
                using (var stream = new FileStream(combinedPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            var result = await _leaveService.UpdateLeave(leave, sessionEmployeeId,companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get soft deleted the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteWorkFromHome(EmployeeAppliedLeave leave)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (leave != null)
            {
                result = await _leaveService.DeleteLeave(leave, sessionEmployeeId,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get approvedleave the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> ApprovedWorkFromHome(EmployeeAppliedLeave leave)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (leave != null)
            {
                result = await _leaveService.ApprovedLeave(leave, sessionEmployeeId,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get rejectleave the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> RejectWorkFromHome(EmployeeAppliedLeave leave)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = 0;
            if (leave != null)
            {
                result = await _leaveService.RejectLeave(leave, sessionEmployeeId,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        ///  Logic to get display the leave detail  by particular leave
        /// </summary>
        /// <param name="appliedLeaveId" >leave</param>
        [HttpGet]
        public async Task<IActionResult> ViewEmployeeWorkFromHome(int appliedLeaveId)
        {
            var companyId = GetSessionValueForCompanyId;
            var viewEmployeeWorkFromHomeSummary = await _leaveService.GetViewLeaveByAppliedLeaveId(appliedLeaveId,companyId);
            return View(viewEmployeeWorkFromHomeSummary);
        }

        //CompensatoryOff

        /// <summary>
        /// Logic to get all the CompensatoryOffRequests list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CompensatoryOffRequests()
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
            var leaveSummary = await _leaveService.GetAllCompensatoryOffRequests(empId, companyId);
            HttpContext.Session.SetString("LastView", Constant.CompensatoryOffRequests);
            HttpContext.Session.SetString("LastController", Constant.WorkFromHome);
            return View(leaveSummary);
        }
        /// <summary>
        /// Logic to get all the CompensatoryOffRequests list
        /// </summary>
        /// <param name="columnDirection,columnName,pager" ></param> 
        [HttpGet]
        public async Task<IActionResult> CompensatoryOffRequestsFilter(SysDataTablePager pager, string columnDirection, string columnName)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
            var employee = await _leaveService.GetAllCompensatoryOffRequestsFilter(pager,empId,columnDirection,columnName,companyId);
            var employeeCount = await _leaveService.GetAllCompensatoryOffRequestsFilterCount(pager, empId,companyId);
            HttpContext.Session.SetString("LastView", Constant.CompensatoryOffRequests);
            HttpContext.Session.SetString("LastController", Constant.WorkFromHome);
            return Json(new
            {
                iTotalRecords = employeeCount,
                iTotalDisplayRecords = employeeCount,
                data = employee.employeeCompensatoryFilters,
            });
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompensatoryRequest(CompensatoryRequest compensatoryRequest)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var sessionCompanyId = GetSessionValueForCompanyId;
            compensatoryRequest.CompanyId = sessionCompanyId;
            var result = await _leaveService.CreateCompensatory(compensatoryRequest, sessionEmployeeId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get approvedleave the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> ApprovedCompensatoryOff(CompensatoryRequest compensatoryRequest)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = false;
            if (compensatoryRequest != null)
            {
                compensatoryRequest.CompanyId = GetSessionValueForCompanyId;
                result = await _leaveService.ApprovedCompensatoryOff(compensatoryRequest, sessionEmployeeId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get rejectleave the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> RejectCompensatoryOff(CompensatoryRequest compensatoryRequest)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = 0;
            if (compensatoryRequest != null)
            {
                result = await _leaveService.RejectCompensatoryOff(compensatoryRequest, sessionEmployeeId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to View Employee CompensatoryOff
        /// </summary>
        /// <param name="compensatoryId" ></param>
        [HttpGet]
        public async Task<IActionResult> ViewEmployeeCompensatotyOff(int compensatoryId)
        {
            var companyId = GetSessionValueForCompanyId;
            var viewEmployeeWorkFromHomeSummary = await _leaveService.GetViewCompensatoryOffRequestByCompensatoryId(compensatoryId, companyId);
            return View(viewEmployeeWorkFromHomeSummary);
        }


    }
}