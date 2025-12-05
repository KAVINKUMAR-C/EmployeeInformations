using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using Microsoft.Extensions.Configuration;
using EmployeePrivilegesViewModel = EmployeeInformations.Model.ReportsViewModel.EmployeePrivilegesViewModel;

namespace EmployeeInformations.Business.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper, IConfiguration config)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _config = config;
        }

        //Roles

        /// <summary>
        /// Logic to get role list
        /// </summary>  
        public async Task<List<RoleViewModel>> GetAllRoles(int companyId)
        {
            var roleViewModels = new List<RoleViewModel>();
            roleViewModels = await _permissionRepository.GetAllRoles(companyId);           
            return roleViewModels;
        }

        //RolePermission

        /// <summary>
        /// Logic to get addroleprivilege the RolePermission detail by particular role
        /// </summary>
        /// <param name="assignRoleView" ></param>
        /// <param name="sessionEmployeeId" ></param> 
        public async Task<bool> AddPrivilegeByRole(List<AssignRoleView> assignRoleViews, int sessionEmployeeId,int companyId)
        {
            if (assignRoleViews != null && assignRoleViews.Count() > 0)
            {
                var roleId = assignRoleViews.FirstOrDefault()?.RoleId ?? 0;
                await _permissionRepository.GetRolePrivilegeByRoleId((int)roleId, Common.Constant.Delete, companyId);
                var rolePrivilegesEntitys = new List<RolePrivilegesEntity>();

                foreach (var item in assignRoleViews)
                {
                    var rolePrivilegesEntity = new RolePrivilegesEntity();
                    rolePrivilegesEntity.RoleId = (byte)roleId;
                    rolePrivilegesEntity.SubModuleId = item.SubModuleId;
                    rolePrivilegesEntity.IsEnabled = item.IsEnabled == Constant.ZeroStr ? false : true;
                    rolePrivilegesEntity.IsWritable = item.IsWritable == Constant.ZeroStr ? false : true;
                    rolePrivilegesEntity.UpdatedBy = sessionEmployeeId;
                    rolePrivilegesEntity.UpdatedDate = DateTime.Now;
                    rolePrivilegesEntitys.Add(rolePrivilegesEntity);
                }

                var result = await _permissionRepository.AddPrivilegeByRoleId(rolePrivilegesEntitys, companyId);
                return result;
            }
            return true;
        }

        /// <summary>
        /// Logic to get addroleprivilegedashboard the RolePermission detail by particular role
        /// </summary>
        /// <param name="assignDashboardRoleView" ></param>
        /// <param name="sessionEmployeeId" ></param> 
        public async Task<bool> AddDashboardPrivilegeByRole(List<AssignDashboardRoleView> assignDashboardRoleView, int sessionEmployeeId, int companyId)
        {
            if (assignDashboardRoleView != null && assignDashboardRoleView.Count() > 0)
            {
                var roleId = assignDashboardRoleView.FirstOrDefault()?.RoleId ?? 0;
                await _permissionRepository.GetDashboardRolePrivilegeByRoleId((int)roleId, Common.Constant.Delete, companyId);
                var rolePrivilegesEntitys = new List<DashboardRolePrivilegesEntity>();

                foreach (var item in assignDashboardRoleView)
                {
                    var rolePrivilegesEntity = new DashboardRolePrivilegesEntity();
                    rolePrivilegesEntity.RoleId = (byte)roleId;
                    rolePrivilegesEntity.MenuId = item.MenuId;
                    rolePrivilegesEntity.IsEnabled = item.IsEnabled == Constant.ZeroStr ? false : true;
                    rolePrivilegesEntity.UpdatedBy = sessionEmployeeId;
                    rolePrivilegesEntity.UpdatedDate = DateTime.Now;
                    rolePrivilegesEntitys.Add(rolePrivilegesEntity);
                }

                var result = await _permissionRepository.AddDashboardPrivilegeByRoleId(rolePrivilegesEntitys, companyId);
                return result;
            }
            return true;
        }

        /// <summary>
        /// Logic to get update roleprivilege the RolePermission detail by particular role
        /// </summary>
        /// <param name="roleId" ></param>       
        public async Task<RoleAssignViewModel> GetRolePermissionByRoleId(int roleId, int companyId)
        {
            var roleAssignViewModel = new RoleAssignViewModel();
            var result = new List<List<RolePrivilegeViewModel>>();
            var privileges = await _permissionRepository.GetRolePrivilegeByRoleId(roleId, "", companyId);
            var roleEntity = await _permissionRepository.GetRolesByRoleId(roleId, companyId);
            var moduleEntitys = await _permissionRepository.GetAllModules();
            var subModuleEntitys = await _permissionRepository.GetAllSubModules();

            var rolePrivilegeViewModels = new List<RolePrivilegeViewModel>();
            if (privileges.Count() == 0)
            {
                for (var i = 0; i < subModuleEntitys.Count(); i++)
                {
                    var rolePrivilegeViewModel = new RolePrivilegeViewModel();
                    rolePrivilegeViewModel.Id = roleId;
                    rolePrivilegeViewModel.Rolename = roleEntity.RoleName;
                    rolePrivilegeViewModel.SubModuleId = subModuleEntitys[i].SubModuleId;
                    rolePrivilegeViewModel.SubModuleName = subModuleEntitys[i].Name;
                    var moduleEntity = moduleEntitys.FirstOrDefault(x => x.Id == subModuleEntitys[i].ModuleId);
                    rolePrivilegeViewModel.ModuleName = moduleEntity != null ? moduleEntity.Name : string.Empty;
                    rolePrivilegeViewModel.ModuleId = subModuleEntitys[i].ModuleId;
                    rolePrivilegeViewModel.IsEnabled = false;
                    rolePrivilegeViewModel.IsWritable = false;
                    rolePrivilegeViewModels.Add(rolePrivilegeViewModel);
                }
            }
            else
            {
                for (var i = 0; i < subModuleEntitys.Count(); i++)
                {
                    var rolePrivilegeViewModel = new RolePrivilegeViewModel();
                    rolePrivilegeViewModel.Id = roleId;
                    rolePrivilegeViewModel.Rolename = roleEntity.RoleName;
                    rolePrivilegeViewModel.SubModuleId = subModuleEntitys[i].SubModuleId;
                    rolePrivilegeViewModel.SubModuleName = subModuleEntitys[i].Name;
                    var moduleEntity = moduleEntitys.FirstOrDefault(x => x.Id == subModuleEntitys[i].ModuleId);
                    rolePrivilegeViewModel.ModuleName = moduleEntity != null ? moduleEntity.Name : string.Empty;
                    rolePrivilegeViewModel.ModuleId = subModuleEntitys[i].ModuleId;
                    var privillageEntity = privileges.FirstOrDefault(x => x.SubModuleId == subModuleEntitys[i].SubModuleId);
                    rolePrivilegeViewModel.IsEnabled = privillageEntity != null ? privillageEntity.IsEnabled : false;
                    rolePrivilegeViewModel.IsWritable = privillageEntity != null ? privillageEntity.IsWritable : false;
                    rolePrivilegeViewModels.Add(rolePrivilegeViewModel);
                }
            }
            result = rolePrivilegeViewModels.GroupBy(u => u.ModuleId).Select(grp => grp.ToList()).ToList();
            roleAssignViewModel.Id = roleId;
            roleAssignViewModel.Rolename = roleEntity.RoleName;
            roleAssignViewModel.RolePrivilegeViewModels = result;
            return roleAssignViewModel;
        }

        //Dashboard RolePermission

        /// <summary>
        /// Logic to get update roleprivilegedashboard the RolePermission detail by particular role
        /// </summary>
        /// <param name="roleId" ></param>
        public async Task<DashboardRoleAssignViewModel> GetDashboardRolePermissionByRoleId(int roleId, int companyId)
        {
            var roleAssignViewModel = new DashboardRoleAssignViewModel();
            var privileges = await _permissionRepository.GetDashboardRolePrivilegeByRoleId(roleId, "", companyId);
            var roleEntity = await _permissionRepository.GetRolesByRoleId(roleId, companyId);
            var subModuleEntitys = await _permissionRepository.GetAllDashboardMenus();

            var rolePrivilegeViewModels = new List<DashboardRolePrivilegeViewModel>();
            if (privileges.Count() == 0)
            {
                for (var i = 0; i < subModuleEntitys.Count(); i++)
                {
                    var rolePrivilegeViewModel = new DashboardRolePrivilegeViewModel();
                    rolePrivilegeViewModel.Id = roleId;
                    rolePrivilegeViewModel.Rolename = roleEntity.RoleName;
                    rolePrivilegeViewModel.MenuId = subModuleEntitys[i].MenuId;
                    rolePrivilegeViewModel.MenuName = subModuleEntitys[i].MenuName;
                    rolePrivilegeViewModel.IsEnabled = false;
                    rolePrivilegeViewModels.Add(rolePrivilegeViewModel);
                }
            }
            else
            {
                for (var i = 0; i < subModuleEntitys.Count(); i++)
                {
                    var rolePrivilegeViewModel = new DashboardRolePrivilegeViewModel();
                    rolePrivilegeViewModel.Id = roleId;
                    rolePrivilegeViewModel.Rolename = roleEntity.RoleName;
                    rolePrivilegeViewModel.MenuId = subModuleEntitys[i].MenuId;
                    rolePrivilegeViewModel.MenuName = subModuleEntitys[i].MenuName;
                    var privillageEntity = privileges.FirstOrDefault(x => x.MenuId == subModuleEntitys[i].MenuId);
                    rolePrivilegeViewModel.IsEnabled = privillageEntity != null ? privillageEntity.IsEnabled : false;
                    rolePrivilegeViewModels.Add(rolePrivilegeViewModel);
                }
            }
            roleAssignViewModel.Id = roleId;
            roleAssignViewModel.Rolename = roleEntity.RoleName;
            roleAssignViewModel.DashboardRolePrivilegeViewModel = rolePrivilegeViewModels;
            return roleAssignViewModel;
        }

        public async Task<bool> CreateLeavePermission(EmployeePrivilegesViewModel employeePrivileges, int companyId)
        {
            var result = false;
            var employees = _mapper.Map<EmployeesPrivileges>(employeePrivileges);
            var privilegeNameCount = await _permissionRepository.GetLeavePermission(employees, companyId);
            if(privilegeNameCount == 0)
            {
                result = await _permissionRepository.CreateLeavePermission(employees, companyId);
            }
            else
            {
                result = false;
            }
                return result;
        }

        public async Task<int> GetLeavePermissionCount(SysDataTablePager pager, int companyId)
        {
            var employeesfilterCount = await _permissionRepository.GetLeavePermissionCount(pager, companyId);
            return employeesfilterCount;
        }

       
        public async Task<List<EmployeesPrivilegesViewModel>> GetLeavePermissionView(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {
            try
            {
                //var employeePrivileges = new EmployeePrivilegesViewModel();
                var employeePrivileges = await _permissionRepository.GetLeavePermissionView(pager, columnName, columnDirection, companyId);
                var employeesfilter = _mapper.Map<List<EmployeesPrivilegesViewModel>>(employeePrivileges);
                return employeePrivileges;
            }
           catch(Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<EmployeesPrivileges>> GetAllEmployeePrivileges()
        {
            var result = await _permissionRepository.GetAllEmployeePrivileges();
            return result;
        }
    }
}
