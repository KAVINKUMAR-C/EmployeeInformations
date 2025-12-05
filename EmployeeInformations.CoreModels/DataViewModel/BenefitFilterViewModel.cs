using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class BenefitFilterViewModel
    {
        public int BenefitId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int BenefitTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Isdeleted { get; set; }
        public bool EmployeeStatus { get; set; }
        public string? EmployeeName { get; set; }
        public string? BenefitName { get; set; }
    }
    public class BenefitFilterCount
    {
        public int Id { get; set; }
    }
}
