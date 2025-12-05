namespace EmployeeInformations.Model.APIModel
{
    public class WebsiteQuoteRequestModel
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public List<string>? ServicesId { get; set; }
        public string? Comment { get; set; }
        public string Base64string { get; set; }
        public string? FileFormat { get; set; }
        public int ProposalTypeId { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Location { get; set; }
    }

    public class WebsiteSurveyRequestModel
    {
        public string FirstName { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Location { get; set; }
        public int? ServicesId { get; set; }
        public string? Comment { get; set; }
        public int ProposalTypeId { get; set; }
    }
}
