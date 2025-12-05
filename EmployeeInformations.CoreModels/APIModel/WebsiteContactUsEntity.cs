using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_ContactUs")]
    public class WebsiteContactUsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? ContactWebsiteName { get; set; }
        public string? ContactDescription { get; set; }
        public int? ContactLeadTypeId { get; set; }
    }
}
