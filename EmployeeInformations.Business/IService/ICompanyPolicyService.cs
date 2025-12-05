using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.CompanyViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface ICompanyPolicyService
    {
        Task<GetPolicy> GetAllPolicy(int companyId);       
        Task<int> GetCompanySettingsByCompanyId(int companyId);
        Task<bool> CreatePolicy(CompanyPolicy companyPolicy, int sessionEmployeeId,int companyId);
        Task<bool> DeletePloicy(CompanyPolicy companyPolicy);
        Task<List<PolicyAttachments>> GetPolicyDocumentAndFilePath(int policyId);
        Task<CompanyPolicyViewModel> GetAllCompanyPolicyViewModel(CompanyPolicy companyPolicy);
        Task<Int32> CreateCompanySetting(CompanySetting companySettingEntiy,int companyId);
    }
}
