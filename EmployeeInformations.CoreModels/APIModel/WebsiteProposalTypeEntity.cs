using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{

    [Table("Website_ProposalTypes")]
    public class WebsiteProposalTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProposalTypeId { get; set; }
        public string ProposalTypeName { get; set; }

    }

}
