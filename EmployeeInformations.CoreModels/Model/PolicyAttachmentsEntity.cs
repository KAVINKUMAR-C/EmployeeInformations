using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("PolicyAttachments")]
    public class PolicyAttachmentsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }
        public int PolicyId { get; set; }
        public string Document { get; set; }
        public string AttachmentName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
