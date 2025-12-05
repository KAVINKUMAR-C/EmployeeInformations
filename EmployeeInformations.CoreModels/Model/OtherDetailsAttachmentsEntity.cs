using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("OtherDetailsAttachments")]
    public class OtherDetailsAttachmentsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }
        public int EmpId { get; set; }
        public int DetailId { get; set; }
        public string Document { get; set; }
        public string DocumentName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
