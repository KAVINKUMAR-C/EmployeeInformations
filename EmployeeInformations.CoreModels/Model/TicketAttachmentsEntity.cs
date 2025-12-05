using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("TicketAttachments ")]
    public class TicketAttachmentsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int HelpdeskId { get; set; }
        public string AttachmentName { get; set; }
        public string Document { get; set; }
        public bool IsDeleted { get; set; }
    }
}
