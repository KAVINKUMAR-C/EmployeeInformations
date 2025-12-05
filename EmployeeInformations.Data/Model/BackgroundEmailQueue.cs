using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformations.Data.Model
{
    public class BackgroundEmailQueueModel
    {
        // email setting property
        public int EmailSettingId { get; set; }
        public string FromEmail { get; set; }
        public string Host { get; set; }
        public int EmailPort { get; set; }
        public string? DisplayName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        // email queue property
        public int EmailQueueID { get; set; }
        public string EmailQueueFromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSend { get; set; }
        public string? Reason { get; set; }
        public string? EmailQueueDisplayName { get; set; }
        public string? Attachments { get; set; }
        public string? CCEmail { get; set; }
    }
}
