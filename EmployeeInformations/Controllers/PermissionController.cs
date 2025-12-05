using EmployeeInformations.Business.IService;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeInformations.Controllers
{
    public class PermissionController : BaseController
    {

        private readonly IPermissionService _permissionService;
        private readonly IReportService _reportService;
        public PermissionController(IPermissionService permissionService, IReportService reportService)
        {
            _permissionService = permissionService;
            _reportService = reportService;
        }


        /// <summary>
        /// Logic to get all the rolelist list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RoleList()
        {
            var companyId = GetSessionValueForCompanyId;
            var roleList = await _permissionService.GetAllRoles(companyId);
            return View(roleList);
        }

        /// <summary>
        /// Logic to get all the dashboardrolelist list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DashboardRoleList()
        {
            var companyId = GetSessionValueForCompanyId;
            var roleList = await _permissionService.GetAllRoles(companyId);
            return View(roleList);
        }

        /// <summary>
        /// Logic to get edit the role detail  by particular role
        /// </summary>
        /// <param name="roleId" >role</param>
        [HttpGet]
        public IActionResult EditRole(int roleId)
        {
            var roleViewModel = new RoleViewModel();
            roleViewModel.RoleId = (Common.Enums.Role)(short)roleId;
            return PartialView("EditRole", roleViewModel);
        }

        /// <summary>
        /// Logic to get edit the editdashbordrole detail  by particular editdashbordrole
        /// </summary>
        /// <param name="roleId" >role</param>
        [HttpGet]
        public IActionResult EditDashbordRole(int roleId)
        {
            var roleViewModel = new RoleViewModel();
            roleViewModel.RoleId = (Common.Enums.Role)(short)roleId;
            return PartialView("EditDashbordRole", roleViewModel);
        }

        /// <summary>
        /// Logic to get updatepermission the RolePermission detail  by particular role
        /// </summary>
        /// <param name="roleId" >role</param>
        [HttpGet]
        public async Task<IActionResult> UpdateRolePermission(int roleId)
        {
            var companyId = GetSessionValueForCompanyId;
            var roleViewModel = await _permissionService.GetRolePermissionByRoleId(roleId, companyId);
            return PartialView("UpdateRole", roleViewModel);
        }

        /// <summary>
        /// Logic to get UpdateDashboardRolePermission the RolePermission detail  by particular role
        /// </summary>
        /// <param name="roleId" >role</param>
        [HttpGet]
        public async Task<IActionResult> UpdateDashboardRolePermission(int roleId)
        {
            var companyId = GetSessionValueForCompanyId;
            var roleViewModel = await _permissionService.GetDashboardRolePermissionByRoleId(roleId, companyId);
            return PartialView("UpdateDashboardRolePermission", roleViewModel);
        }

        /// <summary>
        /// Logic to get addaccesscategorybyrole the RolePermission detail  by particular role
        /// </summary>
        /// <param name="assignRoleView" >assignRoleView</param>
        [HttpPost]
        public async Task<IActionResult> AddAccessCategoryByRole(List<AssignRoleView> assignRoleView)
        {
            var result = false;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var data = Convert.ToString(Request.Form["Permission"]);
            var model = JsonConvert.DeserializeObject<List<AssignRoleView>>(data);
            if (model != null)
            {
                result = await _permissionService.AddPrivilegeByRole(model, sessionEmployeeId,companyId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get adddashboardaccesscategorybyrole the RolePermission detail  by particular role
        /// </summary>
        /// <param name="assignRoleView" >assignRoleView</param>
        [HttpPost]
        public async Task<IActionResult> AddDashboardAccessCategoryByRole(List<AssignDashboardRoleView> assignRoleView)
        {
            var result = false;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var data = Convert.ToString(Request.Form["Permission"]);
            var model = JsonConvert.DeserializeObject<List<AssignDashboardRoleView>>(data);
            if (model != null)
            {
                result = await _permissionService.AddDashboardPrivilegeByRole(model, sessionEmployeeId, companyId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get all the dashboardrolelist list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LeavePermission()
        {
            var companyId = GetSessionValueForCompanyId;
            var leavePermission = new EmployeePrivilegesViewModel();
            var employee = await _reportService.GetAllEmployeesDrropdown(companyId);
            var employeePrivilages = await _permissionService.GetAllEmployeePrivileges();
            leavePermission.Employees = employee;
            return View(leavePermission);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeavePermission(SysDataTablePager pager, string columnDirection, string ColumnName)
        {
            var companyId = GetSessionValueForCompanyId;
            var count = await _permissionService.GetLeavePermissionCount(pager, companyId);
            var details = await _permissionService.GetLeavePermissionView(pager, columnDirection, ColumnName, companyId);
            return Json(new
            {
                data = details,
                iTotalDisplayRecords = count,
                iTotalRecords = count
            });
        }


        [HttpPost]
        public async Task<IActionResult> CreateLeavePermission(EmployeePrivilegesViewModel employeePrivilegesViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _permissionService.CreateLeavePermission(employeePrivilegesViewModel,companyId);
            return new JsonResult(result);

        }
    }
}
