using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmailDraftType")]
    public class EmailDraftTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DraftType { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
    }
}
