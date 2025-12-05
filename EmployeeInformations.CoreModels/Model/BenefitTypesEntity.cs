using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("BenefitTypes")]
    public class BenefitTypesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BenefitTypeId { get; set; }
        public string BenefitName { get; set; }
        public bool IsDeleted { get; set; }
    }


    [Table("EmployeeBenefit")]
    public class EmployeeBenefitEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BenefitId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public int BenefitTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    [Table("EmployeeMedicalBenefit")]
    public class EmployeeMedicalBenefitEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    }
}
