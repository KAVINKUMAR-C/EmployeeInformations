namespace EmployeeInformations.Model.MasterViewModel
{
    public class EmailSettings
    {
        public int EmailSettingId { get; set; }
        public string FromEmail { get; set; }
        public string Host { get; set; }
        public int EmailPort { get; set; }
        public string? DisplayName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int CompanyId { get; set; }       
        public bool IsDeleted { get; set; }
    }
}
