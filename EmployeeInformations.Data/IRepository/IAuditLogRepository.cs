using EmployeeInformations.CoreModels.Model;

namespace EmployeeInformations.Data.IRepository
{
    public interface IAuditLogRepository
    {
        Task<bool> InsertAssetAuditLog(List<AssetLogEntity> assetLogEntitys,int companyId);
        Task<bool> CreateEmployeeAuditLog(List<EmployeesLogEntity> employeesLogEntiys,int companyId);
    }
}
