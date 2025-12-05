using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;

namespace EmployeeInformations.Data.Repository
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public AuditLogRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// AuditLog

        /// <summary>
        /// Logic to get insert the assetlog detail 
        /// </summary> 
        /// <param name="assetLogEntitys" ></param>
        public async Task<bool> InsertAssetAuditLog(List<AssetLogEntity> assetLogEntitys, int companyId)
        {
            var result = false;
            if (assetLogEntitys.Count > 0)
            {
                assetLogEntitys.ForEach(x =>
                {
                    x.CompanyId = companyId;
                });
                await _dbContext.AssetLog.AddRangeAsync(assetLogEntitys);
               result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get create the assetlog detail 
        /// </summary> 
        /// <param name="employeesLogEntiys" ></param>
        public async Task<bool> CreateEmployeeAuditLog(List<EmployeesLogEntity> employeesLogEntiys, int companyId)
        {
            if (employeesLogEntiys.Count > 0)
            {
                employeesLogEntiys.ForEach(h =>
                {
                    h.CompanyId = companyId;
                });
                await _dbContext.EmployeesLog.AddRangeAsync(employeesLogEntiys);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }

    }
}
