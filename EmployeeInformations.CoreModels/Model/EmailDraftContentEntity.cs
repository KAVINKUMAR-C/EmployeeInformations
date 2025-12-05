using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmailDraftContent")]
    public class EmailDraftContentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailDraftTypeId { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Subject { get; set; }
        public string DraftBody { get; set; }
        public string? DraftVariable { get; set; }
        public string DisplayName { get; set; }
        public string? Email { get; set; }
    }
}
