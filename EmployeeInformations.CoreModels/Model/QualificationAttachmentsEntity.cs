using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("QualificationAttachments")]
    public class QualificationAttachmentsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }
        public int EmpId { get; set; }
        public int QualificationId { get; set; }
        public string? Document { get; set; }
        public string? QualificationName { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}

