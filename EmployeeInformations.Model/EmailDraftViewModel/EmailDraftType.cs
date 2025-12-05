using EmployeeInformations.CoreModels.Model;
namespace EmployeeInformations.Model.EmailDraftViewModel
{
    public class EmailDraftType
    {
        public int Id { get; set; }
        public string DraftType { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
    }

    public class EmailDraftTypeViewModel
    {
        public int Id { get; set; }
        public string DraftType { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public List<EmailDraftType> EmailDraftType { get; set; }
        public List<EmailDraftEntites>EmailDraftPageniation { get; set; }
    }
}
