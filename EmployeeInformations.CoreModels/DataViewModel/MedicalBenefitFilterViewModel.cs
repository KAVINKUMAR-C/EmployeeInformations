
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class MedicalBenefitFilterViewModel
    {
        public int MedicalBenefitId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int BenefitTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool EmployeeStatus { get; set; }
        public bool? Isdeleted { get; set; }
        public string? EmployeeName { get; set; }
        public string? BenefitName { get; set; }
        public int Cost { get; set; }
        public string? Member { get; set; }
        public string? MembershipNumber { get; set; }
        public string? Category { get; set; }
        public string? Scheme { get; set; }
    }
    public class MedicalBenefitFilterCount
    {
        public int Id { get; set; }
    }
}
