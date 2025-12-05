
using EmployeeInformations.Model.MasterViewModel;

namespace EmployeeInformations.Model.EmailDraftViewModel
{
    public class EmailDraftContent
    {
        public int EmailDraftTypeId { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Subject { get; set; }
        public string DraftBody { get; set; }
        public string? DraftVariable { get; set; }
        public string DisplayName { get; set; }
        public string EmailDraftTypeName { get; set; }
        public string? Email { get; set; }
        public string strFmtEmailId { get; set; }

        public List<SendEmails>? sendEmails { get; set; }
        public List<EmailDraftType>? EmailDraftType { get; set; }

    }
}
