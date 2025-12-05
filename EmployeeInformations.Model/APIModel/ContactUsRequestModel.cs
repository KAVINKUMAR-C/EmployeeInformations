namespace EmployeeInformations.Model.APIModel
{
    public class ContactUsRequestModel
    {
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? ContactWebsiteName { get; set; }
        public string? ContactDescription { get; set; }
        public int? ContactLeadTypeId { get; set; }
    }
}
