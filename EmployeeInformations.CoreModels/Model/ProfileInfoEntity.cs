using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("ProfileInfo")]
    public class ProfileInfoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }
        [ForeignKey("Employees")]
        public int EmpId { get; set; }
        public virtual EmployeesEntity Employees { get; set; }
        public int Gender { get; set; }

        public int MaritalStatus { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int BloodGroup { get; set; }

        public DateTime DateOfJoining { get; set; }

        public string CountryCode { get; set; }

        public string PhoneNumber { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ProfileName { get; set; }
        public string ProfileFilePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? ContactPersonName { get; set; }
        public int? RelationshipId { get; set; }
        public string? ContactNumber { get; set; }
        public string? CountryCodeNumber { get; set; }
        public string? OthersName { get; set; }
    }
}
