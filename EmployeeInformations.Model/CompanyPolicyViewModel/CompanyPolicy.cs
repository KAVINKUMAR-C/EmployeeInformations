namespace EmployeeInformations.Model.CompanyPolicyViewModel
{
    public class CompanyPolicy
    {
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<PolicyAttachments> PolicyAttachments { get; set; }
        public List<GetPolicy> GetPolicy { get; set; }
    }

    public class PolicyAttachments
    {
        public int AttachmentId { get; set; }
        public int PolicyId { get; set; }
        public string Document { get; set; }
        public string AttachmentName { get; set; }
        public string splitName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class GetPolicy
    {
        public int? PolicyId { get; set; }
        public string? PolicyName { get; set; }
        public string? ClassName { get; set; }
        public string? DocumentFilePath { get; set; }
        public string? PolicyActionName { get; set; }
        public string? AttacmentName { get; set; }
        public string? PolicyViewImage { get; set; }
        public List<CompanyPolicy>? CompanyPolicy { get; set; }
        public List<PolicyAttachments>? PolicyAttachments { get; set; }
    }

    public class CompanyPolicyViewModel
    {
        public int PolicyId { get; set; }
        public int Companyname { get; set; }
        public CompanyPolicy? CompanyPolicy { get; set; }
        public string? Document { get; set; }
        public string? PolicyActionName { get; set; }
        public string? AttachmentName { get; set; }
        public string? PolicyViewImage { get; set; }
        public List<PolicyAttachments>? PolicyAttachmentsViewModel { get; set; }
    }

}
