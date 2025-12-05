using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.BenefitViewModel
{
    public class EmployeeMedicalBenefitViewModel
    {
        public int MedicalBenefitId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int BenefitTypeId { get; set; }
        public string Scheme { get; set; }
        public string Category { get; set; }
        public int Cost { get; set; }
        public string Member { get; set; }
        public string MembershipNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<BenefitTypes> BenefitTypes { get; set; }
        public List<EmployeeMedicalBenefit> EmployeeMedicalBenefits { get; set; }
        public List<ReportingPerson> reportingPeople { get; set; }
    }
    public class EmployeeMedicalBenefit
    {
        public int MedicalBenefitId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int BenefitTypeId { get; set; }
        public string Scheme { get; set; }
        public string Category { get; set; }
        public int Cost { get; set; }
        public string Member { get; set; }
        public string MembershipNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string EmployeeName { get; set; }
        public bool EmployeeStatus { get; set; }
        public string BenefitName { get; set; }
        public string CompanyName { get; set; }
    }

}
