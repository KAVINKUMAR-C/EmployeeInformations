
using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;



namespace EmployeeInformations.Model.APIModel
{
    public class WebsiteJobApplyModel
    {
        public int JobApplyId { get; set; }
        public int JobId { get; set; }
        public string? FullName { get; set; }
        public string SortFullName { get; set; }
        public string? Email { get; set; }
        public string? Experience { get; set; }
        public string? FilePath { get; set; }
        public DateTime ApplyDate { get; set; } = DateTime.Now;
        public string FileName { get; set; }
        public string? FileFormat { get; set; }
        public string SkillSet { get; set; }
        public string? RelevantExperience { get; set; }
        public string ClassName { get; set; }
        public string? JobName { get; set; }
        public string? Attachment { get; set; }
        public bool? IsActive { get; set; }
        public string? IsRecordForm { get; set; }

        public List<WebsiteJobViewModel>? WebsiteJobs { get; set; }
        public List<SkillSet>? skillSets { get; set; }

        public List<SkillSet> Skills { get; set; }
        public List<int> SkillId { get; set; }
        public string splitdocname { get; set; }
        public List<string> SkillNames { get; set; }
        public string SkillName { get; set; }
        public List<WebsiteJobApplyModel>? websiteJobApplyModel { get; set; }
        public List<WebsiteJobViewModel> WebsiteJobViewModels { get; set; }
        public string File { get; set; }
        public List<WebsiteJobApplyViewModel>? websiteJobApplyViewModels { get; set; }
    }
 
}
