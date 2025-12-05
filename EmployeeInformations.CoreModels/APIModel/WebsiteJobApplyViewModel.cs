using Microsoft.EntityFrameworkCore;


namespace EmployeeInformations.CoreModels.APIModel
{
    [Keyless]
    public class WebsiteJobApplyViewModel
    {
            public int JobApplyId { get; set;}
            public int JobId { get; set;}
            public DateTime? ApplyDate { get; set;}
            public string? JobName { get; set;}
            public string FullName { get; set;}
            public string? Email { get; set;}
            public string Experience { get; set;}
            public string SkillSet { get; set;}
            public string RelevantExperience { get; set;}
            public string FilePath { get; set;}          
            public bool? IsActive { get; set;}
            public string FileFormat { get; set;}
            public string? IsRecordForm { get; set;}

    }

    [Keyless]
    public class WebsideJobApplyCount
    {
        public int JobApplyCountId { get; set; }
    }
}
