using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("employees", Schema = "public")] // Ensure PostgreSQL schema & case match
    public class EmployeesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }

        public string UserName { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? FatherName { get; set; }

        public string OfficeEmail { get; set; }

        public int DesignationId { get; set; }

        public int DepartmentId { get; set; }

        public string? PersonalEmail { get; set; }

        public string? SkillName { get; set; }

        public bool IsActive { get; set; }

        public bool IsProbationary { get; set; }

        public DateTime? ProbationDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public byte RoleId { get; set; }

        public bool IsDeleted { get; set; }

        public string? RejectReason { get; set; }

        public int? RelieveId { get; set; }

        public int? EsslId { get; set; }

        public bool IsVerified { get; set; }

        public DateTime? ReleavedDate { get; set; }

        public bool IsOnboarding { get; set; }

        public string? UANNumber { get; set; }
    }
}

