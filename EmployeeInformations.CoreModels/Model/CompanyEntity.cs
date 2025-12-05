using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Company")]
    public class CompanyEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string Industry { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public int ContactPersonGender { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public string PhysicalAddress1 { get; set; }
        public string PhysicalAddress2 { get; set; }
        public int PhysicalAddressCity { get; set; }
        public int PhysicalAddressState { get; set; }
        public int PhysicalAddressZipCode { get; set; }
        public string? MailingAddress1 { get; set; }
        public string? MailingAddress2 { get; set; }
        public int? MailingAddressCity { get; set; }
        public int? MailingAddressState { get; set; }
        public int? MailingAddressZipCode { get; set; }
        public string? CompanyLogo { get; set; }
        public string? CompanyFilePath { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CountryCode { get; set; }
        public string? CompanyCountryCode { get; set; }
        public int PhysicalCountryId { get; set; }
        public bool IsActive { get; set; }
        public int? MailingCountryId { get; set; }
    }
}
