namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class OtherDetailsViewModel
    {
        public int EmpId { get; set; }
        public List<OtherDetails>? OtherDetails { get; set; }
        public int CompanyId { get; set; }
        public string DocumentFilePath { get; set; }
        public string? DocumentActionName { get; set; }
        public string DocumentName { get; set; }
        public string? DocumentViewImage { get; set; }
        public int DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public string? StrFmtDocumentId { get; set; }
        public List<DocumentTypes> documentTypes { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? UANNumber { get; set; }
    }
}
