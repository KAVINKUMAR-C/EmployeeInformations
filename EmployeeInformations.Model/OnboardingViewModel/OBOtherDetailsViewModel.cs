
namespace EmployeeInformations.Model.OnboardingViewModel
{
    public class OBOtherDetailsViewModel
    {
        public int EmpId { get; set; }
        public List<OBOtherDetails>? OtherDetails { get; set; }
        public int CompanyId { get; set; }
        public string DocumentFilePath { get; set; }
        public string? DocumentActionName { get; set; }
        public string DocumentName { get; set; }
        public string? DocumentViewImage { get; set; }
        public int DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public string? StrFmtDocumentId { get; set; }
        public List<OBDocumentTypes> documentTypes { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? UANNumber { get; set; }
    }
}
