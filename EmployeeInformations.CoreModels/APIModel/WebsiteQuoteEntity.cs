using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{

    [Table("Website_Proposals")]
    public class WebsiteQuoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuoteId { get; set; }
        public string ERFN { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ServicesId { get; set; }
        public string? Comment { get; set; }
        public string? FilePath { get; set; }
        public DateTime? ApplyDate { get; set; }
        public int ProposalTypeId { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Location { get; set; }

    }
}
