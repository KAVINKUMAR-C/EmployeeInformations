
namespace EmployeeInformations.Model.OnboardingViewModel
{
     public class OBOtherDetails
    {
        public int DetailId { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        //public string DocumentName { get; set; }
        public string DocumentTypeName { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        //public string Document { get; set; }
        public string? DocumentActionName { get; set; }
        public string? DocumentViewImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<OBDocumentTypes> documentTypes { get; set; }
        public List<int> DocumentId { get; set; }
        public string StrFmtDocumentId { get; set; }
        public bool IsDeleted { get; set; }
        public string? UANNumber { get; set; }
        public List<OBOtherDetailsAttachments> OtherDetailsAttachments { get; set; }
        // Datetime Issue
        public string StrValidFrom { get; set; }
        public string StrValidTo { get; set; }
    }
    public class OBOtherDetailsAttachments
    {
        public int AttachmentId { get; set; }
        public int EmpId { get; set; }
        public int DetailId { get; set; }
        public string Document { get; set; }
        public string DocumentName { get; set; }

    }

    public class OBOtherDetailsDocumentFilePath
    {
        public string DocumentName { get; set; }
        public string Document { get; set; }
        public int EmpId { get; set; }
    }
   
}

