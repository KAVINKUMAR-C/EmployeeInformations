using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.BenefitViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IBenefitService
    {
        Task<EmployeeBenefitViewModel> GetAllBenefitDetails(int companyId);
        Task<bool> AddBenefits(EmployeeBenefit employeeBenefit, int sessionEmployeeId,int companyId);
        Task<bool> DeleteBenefit(EmployeeBenefit employeeBenefit, int sessionEmployeeId);
        Task<EmployeeMedicalBenefitViewModel> GetAllMedicalBenefitDetails(int companyId);
        Task<bool> AddMedicalBenefits(EmployeeMedicalBenefit employeeMedicalBenefit, int sessionEmployeeId,int companyId);
        Task<bool> DeleteMedicalBenefit(EmployeeMedicalBenefit employeeMedicalBenefit, int sessionEmployeeId);
        // Task<ViewEmployeeBenefits> GetEmployeeBenefitsByEmployeeId(int empId);
        Task<ViewTotalEmployeeBenefits> GetEmployeeBenefitsByEmployeeId(int empId, int companyId);
        Task<EmployeeMedicalBenefit> GetMedicalBenefitsviewBenefitId(int MedicalBenefitId,int companyId);
        Task<int> BenefitCount(int companyId, SysDataTablePager pager);
        Task<List<BenefitFilterViewModel>> GetBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection);
        Task<int> MedicalBenefitCount(int companyId, SysDataTablePager pager);
        Task<List<MedicalBenefitFilterViewModel>> GetMedicalBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection);
        Task<EmployeeBenefit> GetBenefitsviewBenefitId(int BenefitId, int companyId);
        
    }
}
