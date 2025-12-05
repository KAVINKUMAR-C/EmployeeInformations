using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using EmployeePrivilegesCount = EmployeeInformations.CoreModels.Model.EmployeePrivilegesCount;

namespace EmployeeInformations.Data.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public PermissionRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Role 

        /// <summary>
        /// Logic to get role list
        /// </summary>                 
        public async Task<List<RoleViewModel>> GetAllRoles(int companyId)
        {
            var rolePermission = await (from rolepermission in _dbContext.RoleEntities
                                        where !rolepermission.IsDeleted && companyId == rolepermission.CompanyId
                                        select new RoleViewModel ()
                                        {
                                            RoleId = (Common.Enums.Role)rolepermission.RoleId,
                                            RoleName = rolepermission.RoleName,
                                            IsActive = rolepermission.IsActive,
                                        }).ToListAsync();
            return rolePermission;
        }


        /// <summary>
           /// Logic to get roleId the role detail by particular role
             /// </summary>   
        /// <param name="roleId" ></param>       
        /// <param name="CompanyId" ></param>
        public async Task<RoleEntity> GetRolesByRoleId(int roleId, int companyId)
        {
            var roleEntity = await _dbContext.RoleEntities.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == roleId && x.CompanyId == companyId);
            return roleEntity ?? new RoleEntity();
        }

        /// Role privileges

        /// <summary>
           /// Logic to get roleId delete the roleprivileges detail by particular roleprivileges
           /// </summary>   
        /// <param name="roleId,delete" ></param>       
        /// <param name="CompanyId" ></param>
        public async Task<List<RolePrivilegesEntity>> GetRolePrivilegeByRoleId(int roleId, string delete,int companyId)
        {
            var rolePrivilegesEntitys = await _dbContext.RolePrivileges.AsTracking().Where(x => x.RoleId == roleId && x.CompanyId == companyId).ToListAsync();
            if (delete == Common.Constant.Delete)
            {
                _dbContext.RolePrivileges.RemoveRange(rolePrivilegesEntitys);
                await _dbContext.SaveChangesAsync();
            }
            return rolePrivilegesEntitys;
        }


        /// <summary>
        /// Logic to get roleId the roleprivileges  detail by particular roleprivileges
        /// </summary>         
        /// <param name="roleId" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<RolePrivilegesEntity>> LayoutRolePermissionByRoleId(int roleId, int companyId)
        {
            var rolePrivileges = await _dbContext.RolePrivileges.Where(x => x.RoleId == roleId && x.CompanyId == companyId).ToListAsync();
            return rolePrivileges;
        }

        /// Dashboard role privilege

        /// <summary>
         /// Logic the roleprivileges detail by particular roleprivileges for remove
          /// </summary>   
        /// <param name="roleId,delete" ></param>       
        /// <param name="CompanyId" ></param>
        public async Task<List<DashboardRolePrivilegesEntity>> GetDashboardRolePrivilegeByRoleId(int roleId, string delete, int companyId)
        {
            var rolePrivilegesEntitys = await _dbContext.DashboardRolePrivilegesEntitys.AsTracking().Where(x => x.RoleId == roleId && x.CompanyId == companyId).ToListAsync();
            if (delete == Common.Constant.Delete)
            {
                _dbContext.DashboardRolePrivilegesEntitys.RemoveRange(rolePrivilegesEntitys);
                await _dbContext.SaveChangesAsync();
            }
            return rolePrivilegesEntitys;
        }

        /// <summary>
        /// Logic to get add the roleprivileges detail by particular roleprivileges
        /// </summary>   
        /// <param name="rolePrivilegesEntitys" ></param>             
        public async Task<bool> AddPrivilegeByRoleId(List<RolePrivilegesEntity> rolePrivilegesEntitys, int companyId)
        {
            if (rolePrivilegesEntitys.Count() > 0)
            {
                rolePrivilegesEntitys.ForEach(x =>
                {
                    x.CompanyId = companyId;
                });
                await _dbContext.RolePrivileges.AddRangeAsync(rolePrivilegesEntitys);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// Logic to  add the dashboard role privileges roleId
        /// </summary>   
        /// <param name="DashboardRolePrivilegesEntitys" ></param>   
        public async Task<bool> AddDashboardPrivilegeByRoleId(List<DashboardRolePrivilegesEntity> dashboardRolePrivilegesEntity, int companyId)
        {
            if (dashboardRolePrivilegesEntity.Count() > 0)
            {
                dashboardRolePrivilegesEntity.ForEach(x =>
                {
                    x.CompanyId = companyId;
                });
                await _dbContext.DashboardRolePrivilegesEntitys.AddRangeAsync(dashboardRolePrivilegesEntity);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }


        /// <summary>
        /// Logic to get modules list
        /// </summary>         
        /// <param name="IsActive" ></param>      
        public async Task<List<ModulesEntity>> GetAllModules()
        {
            return await _dbContext.Modules.Where(p => p.IsActive).ToListAsync();
        }


        /// <summary>
        /// Logic to get submodules list
        /// </summary>         
        /// <param name="IsActive" ></param>   
        public async Task<List<SubModulesEntity>> GetAllSubModules()
        {
            return await _dbContext.SubModulesEntitys.Where(q => q.IsActive).ToListAsync();
        }


        /// <summary>
        /// Logic to get dashboard menu list
        /// </summary>         
        /// <param name="IsActive" ></param>
        public async Task<List<DashboardMenusEntity>> GetAllDashboardMenus()
        {
            return await _dbContext.DashboardMenusEntitys.Where(q => q.IsActive).ToListAsync();
        }
        
        public async Task<bool> CreateLeavePermission(EmployeesPrivileges employeePrivileges, int companyId)
        {
            try
            {
                var Privileges = await _dbContext.EmployeePrivileges.Where(x => x.PrivilegeID == employeePrivileges.PrivilegeID && x.CompanyId == companyId).FirstOrDefaultAsync();
                if (employeePrivileges.PrivilegeID == 0)
                {
                    await _dbContext.EmployeePrivileges.AddAsync(employeePrivileges);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    Privileges.IsEarnLeave = employeePrivileges.IsEarnLeave;
                    _dbContext.EmployeePrivileges.Update(Privileges);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> GetLeavePermission(EmployeesPrivileges employeePrivileges, int companyId)
        {
            return await _dbContext.EmployeePrivileges.Where(x => x.EmployeeID == employeePrivileges.EmployeeID && x.CompanyId == companyId).CountAsync();
        }
        public async Task<List<EmployeesPrivilegesViewModel>> GetLeavePermissionView(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {
            try
            {
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param1 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", _params.Sorting);
                var param5 = new NpgsqlParameter("@companyId", companyId);
                var data = await _dbContext.EmployeesPrivilegesViewModel.FromSqlRaw("EXEC [dbo].[spGetEmployeePrivileges] @pagingSize ,@offsetValue,@searchText,@sorting,@companyId", param1, param2, param3, param4,param5).ToListAsync();                
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Logic to get EventSubCategory filter view Count
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public async Task<int> GetLeavePermissionCount(SysDataTablePager pager, int companyId)
        {
            try
            {
                var result = 0;

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };
                var param1 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param2 = new NpgsqlParameter("@companyId", companyId);
                List<EmployeePrivilegesCount> employeeCounts = await _dbContext.EmployeePrivilegesCount.FromSqlRaw("EXEC [dbo].[spGetEmployeePrivilegesCount] @searchText,@companyId", param1,param2).ToListAsync();
                foreach (var item in employeeCounts)
                {
                    result = item.Id;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<EmployeesPrivileges>> GetAllEmployeePrivileges()
        {
            return await _dbContext.EmployeePrivileges.ToListAsync();
        }
    }
}