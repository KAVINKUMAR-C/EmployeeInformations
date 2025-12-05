namespace EmployeeInformations.Model.MasterViewModel
{
    public class SendEmailsViewModel
    {
        public int EmailListId { get; set; }
        public int EmailSettingId { get; set; }
        public string EmailId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public List<SendEmails> SendEmails { get; set; }

    }
}
