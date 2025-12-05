using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("ClientDetails")]
    public class ClientEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCompany { get; set; }
        public string ClientDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public int CompanyId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
    }
}
