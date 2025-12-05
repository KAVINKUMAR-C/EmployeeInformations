using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.APIModel
{
    public class WebsiteJobViewModel
    {
        public int JobId { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string? Designation { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set; }
        public string? JobType { get; set; }
        public int NoOfPositions { get; set; }
        public string Location { get; set; }
        public string JobSummary { get; set; }
        public DateTime JobPostedDate { get; set; }
        public string? RelevantExperience { get; set; }
        public string SkillName { get; set; }
        public List<string> SkillNames { get; set; }
        public List<int> SkillId { get; set; }
        public string? StrFmtSkillId { get; set; }
        public List<SkillSet> skillSet { get; set; }
        public List<WebsiteJobViewModel>? websiteJobViewModel { get; set; }
        public int IsDeleted { get; set; } = 0;
    }
}
