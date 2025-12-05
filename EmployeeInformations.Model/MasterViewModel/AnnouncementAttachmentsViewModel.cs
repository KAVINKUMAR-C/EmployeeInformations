

namespace EmployeeInformations.Model.MasterViewModel
{
   public class AnnouncementAttachmentsViewModel
   {
        public int AttachmentId { get; set; }
        public int AnnouncementId { get; set; }
        public string AttachmentName { get; set; }
        public string Document { get; set; }
        public bool IsDeleted { get; set; }
   }
}
