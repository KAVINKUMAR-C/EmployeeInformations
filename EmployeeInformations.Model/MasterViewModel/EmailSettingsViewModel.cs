using EmployeeInformations.CoreModels.Model;

namespace EmployeeInformations.Model.MasterViewModel
{
    public class EmailSettingsViewModel
    {
        public int EmailSettingId { get; set; }
        public string FromEmail { get; set; }
        public string Host { get; set; }
        public int EmailPort { get; set; }
        public string? DisplayName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public int TitleId { get; set; }
        public int CompanyId { get; set; }      
        public List<SendEmails> SendEmails { get; set; }
        public List<SendEmailsEntity> SendEmailsEntitys { get; set; }
    }
}
