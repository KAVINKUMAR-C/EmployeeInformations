using EmployeeInformations.Common.Enums;
using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.BenefitViewModel
{
    public class EmployeeBenefitViewModel
    {
        public int BenefitId { get; set; }
        public int? CompanyId { get; set; }
        public int? EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string BenifitName { get; set; }
        public int? BenefitTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool EmployeeStatus { get; set; }
        public List<EmployeeBenefit> EmployeeBenefit { get; set; }
        public List<BenefitTypes> BenefitTypes { get; set; }
        public List<Employees> Employees { get; set; }
        public List<ReportingPerson> reportingPeople { get; set; }

    }
    public class EmployeeBenefit
    {
        public int BenefitId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int BenefitTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool EmployeeStatus { get; set; }
        public string EmployeeName { get; set; }
        public string BenefitName { get; set; }
        public string CompanyName { get; set; }
    }
    public class ViewTotalEmployeeBenefits
    {
        public int BenefitId { get; set; }
        public int EmpId { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public int BenefitTypeName { get; set; }
        public string BenefitName { get; set; }
        public Role RoleId { get; set; }
        public bool EmployeeStatus { get; set; }
        public List<EmployeeBenefit> ViewEmployeeBenefits { get; set; }
        public List<EmployeeMedicalBenefit> ViewEmployeeMedicalBenefits { get; set; }
    }
    public class ViewEmployeeBenefits
    {
        public int BenefitId { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public int BenefitTypeName { get; set; }
        public string BenefitName { get; set; }
        public bool EmployeeStatus { get; set; }
    }
    public class ViewEmployeeMedicalBenefits
    {
        public int MedicalBenefitId { get; set; }
        public string CompanyName { get; set; }
        public string Scheme { get; set; }
        public string Category { get; set; }
        public int Cost { get; set; }
        public string Member { get; set; }
        public string MembershipNumber { get; set; }
        public string EmployerName { get; set; }
        public string BenefitName { get; set; }
        public bool EmployeeStatus { get; set; }
    }
}

