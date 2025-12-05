using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeInformations.Business.Service
{
    public class EmployeeInformationService : IEmployeeInformationService
    {

        private readonly EmployeesDbContext _dbHelper;
        private readonly IMemoryCache _memoryCache;
        public EmployeeInformationService(EmployeesDbContext dbHelper, IMemoryCache memoryCache)
        {
            _dbHelper = dbHelper;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Logic to get leavetypes list
        /// </summary> 
        public async Task<List<LeaveTypesEntity>> GetLeaveTypeData()
        {
            var output = _memoryCache.Get<List<LeaveTypesEntity>>(Constant.Leavetypesentitys);

            if (output is not null) return output;
            var leaveTypes = await _dbHelper.leaveTypes.Where(x => !x.IsDeleted).ToListAsync();
            _memoryCache.Set(Constant.Leavetypesentitys, leaveTypes, TimeSpan.FromMinutes(10));
            return leaveTypes;
        }

        /// <summary>
        /// Logic to get privilegesrole
        /// </summary>
        /// <param name="roleId" ></param>
        public async Task<List<int>> GetPrivilegesByRole(int roleId)
        {
            var output = _memoryCache.Get<List<int>>(Constant.Roleprivilegesentity);

            if (output is not null) return output;
            var rolePrivileges = await _dbHelper.RolePrivileges.Where(x => x.RoleId == roleId).Select(x => x.SubModuleId).ToListAsync();
            _memoryCache.Set(Constant.Roleprivilegesentity, rolePrivileges, TimeSpan.FromMinutes(10));
            return rolePrivileges;
        }

        /// <summary>
        /// Logic to get layoutrolepermissionbyroleId
        /// </summary>
        /// <param name="roleId" ></param>
        public async Task<List<int>> LayoutRolePermissionByRoleId(int roleId)
        {
            var rolePrivileges = await _dbHelper.RolePrivileges.Where(x => x.RoleId == roleId && x.IsEnabled).Select(x => x.SubModuleId).ToListAsync();
            return rolePrivileges;
        }

        public async Task<int> LayoutDisplayMode(int companyId)
        {
            var displayId = await _dbHelper.CompanySetting.Where(x => x.CompanyId == companyId).Select(x => x.ModeId).FirstOrDefaultAsync();
            return displayId;
        }

        /// <summary>
        /// Logic to get dashboardrolepermissionbyroleId
        /// </summary>
        /// <param name="roleId" ></param>
        public async Task<List<int>> DashboardRolePermissionByRoleId(int roleId)
        {
            var rolePrivileges = await _dbHelper.DashboardRolePrivilegesEntitys.Where(x => x.RoleId == roleId && x.IsEnabled).Select(x => x.MenuId).ToListAsync();
            return rolePrivileges;
        }

        /// <summary>
        /// Logic to get readandwritepermissionbyroleId
        /// </summary>
        /// <param name="subModuleId" ></param>
        /// <param name="roleId" ></param>
        public async Task<bool> ReadAndWritePermissionByRoleId(int subModuleId, int roleId)
        {
            var rolePrivileges = await _dbHelper.RolePrivileges.Where(x => x.RoleId == roleId && x.IsEnabled && x.SubModuleId == subModuleId).Select(x => x.IsWritable).FirstOrDefaultAsync();
            return rolePrivileges;
        }



    }

}
