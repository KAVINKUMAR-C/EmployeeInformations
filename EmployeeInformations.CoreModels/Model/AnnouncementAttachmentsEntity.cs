using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("AnnouncementAttachments")]
    public class AnnouncementAttachmentsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }
        public int AnnouncementId { get; set; }
        public string AttachmentName { get; set; }
        public string Document { get; set; }
        public bool IsDeleted { get; set; }
    }
}
