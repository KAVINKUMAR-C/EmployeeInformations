using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("ExperienceAttachments")]
    public class ExperienceAttachmentsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }
        public int EmpId { get; set; }
        public int ExperienceId { get; set; }
        public string Document { get; set; }
        public string ExperienceName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}