 using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.BenefitViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IBenefitRepository
    {
        Task<List<BenefitTypesEntity>> GetBenefitTypes();
        Task<List<EmployeeBenefit>> GetAllEmployeeBenefits(int companyId);
        Task<List<EmployeeMedicalBenefit>> GetAllEmployeeMedicalBenefits(int companyId);
        Task<EmployeeBenefitEntity> GetBenefitByBenefitId(int benefitid,int companyId);
        Task<bool> AddBenefits(EmployeeBenefitEntity employeeBenefitEntity,int companyId);
        Task<bool> DeleteBenefit(EmployeeBenefitEntity employeeBenefitEntity, int sessionEmployeeId);
        Task<EmployeeMedicalBenefitEntity> GetMedicalBenefitByMedicalBenefitId(int medicalBenefitId,int companyId);
        Task<bool> AddMedicalBenefits(EmployeeMedicalBenefitEntity employeeMedicalBenefitEntity,int companyId);
        Task<bool> DeleteMedicalBenefit(EmployeeMedicalBenefitEntity employeeMedicalBenefitEntity, int sessionEmployeeId);
        Task<EmployeeBenefitEntity> GetEmployeeBenefitsByEmployeeId(int empId,int companyId);
        Task<EmployeeMedicalBenefitEntity> GetEmployeeMedicalBenefitsByEmployeeId(int empId,int companyId);
        Task<BenefitTypesEntity> GetBenefitTypeNameById(int benefitTypeId);
        Task<List<EmployeeBenefitEntity>> GetAllEmployeeBenefitsByEmpId(int empId,int companyId);
        Task<List<EmployeeMedicalBenefitEntity>> GetAllEmployeeMedicalBenefitsByEmpId(int empId,int companyId);
        Task<int> BenefitCount(int companyId, SysDataTablePager pager);
        Task<List<BenefitFilterViewModel>> GetBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection);
        Task<int> MedicalBenefitCount(int companyId, SysDataTablePager pager);
        Task<List<MedicalBenefitFilterViewModel>> GetMedicalBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection);
    }
}
