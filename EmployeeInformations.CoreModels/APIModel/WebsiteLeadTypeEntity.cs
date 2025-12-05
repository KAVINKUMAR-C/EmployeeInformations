using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_LeadType")]
    public class WebsiteLeadTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadTypeId { get; set; }
        public string LeadType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
