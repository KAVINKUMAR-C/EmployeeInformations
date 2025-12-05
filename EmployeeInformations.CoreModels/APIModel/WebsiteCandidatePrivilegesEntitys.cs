
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_CandidatePrivileges")]
    public class WebsiteCandidatePrivilegesEntitys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CandidatePrivilegeId { get; set; }
        public int CandidatescheduleId { get; set; }
        public bool IsEnabled { get; set; }       
        public int CompanyId { get; set; }
    }
}
