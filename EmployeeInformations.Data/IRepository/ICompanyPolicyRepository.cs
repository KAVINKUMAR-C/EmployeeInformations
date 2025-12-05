using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.CompanyPolicyViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface ICompanyPolicyRepository
    {              
        Task<int> GetCompanySettingsByCompanyId(int companyId);
        Task<Int32> CreateCompanySetting(CompanySettingEntity companySettingEntiy, int companyId);
        Task<int> CreatePolicy(CompanyPolicyEntity companyPolicyEntity,int companyId);
        Task<CompanyPolicyEntity> GetPolicyByPolicyId(int policyId);
        Task<bool> DeletePloicy(CompanyPolicyEntity companyPolicyEntity);
        Task<List<PolicyAttachmentsEntity>> GetAllAttachmentByPolicyId(int policyId);
        Task<bool> InsertPolicyAttachment(List<PolicyAttachmentsEntity> policyAttachmentsEntities, int policyId);
        Task<List<PolicyAttachmentsEntity>> GetPolicyDocumentAndFilePath(int policyId);
        Task<bool> DeletePolicyAttachement(List<PolicyAttachmentsEntity> policyAttachmentsEntity);
        Task<List<CompanyPolicy>> GetAllPolicys(int companyId);
    }
}
