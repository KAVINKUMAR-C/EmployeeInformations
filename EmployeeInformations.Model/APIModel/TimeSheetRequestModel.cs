namespace EmployeeInformations.Model.APIModel
{
    public class TimeSheetRequestModel
    {
        public int TimeSheetId { get; set; }
        public int ProjectId { get; set; }
        public int EmpId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TaskName { get; set; }
        public int Status { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentFilePath { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Startdate { get; set; }
        public int CompanyId { get; set; }
        public string StrStartdate { get; set; }
        public string TimeSheetStartTime { get; set; }
        public string TimeSheetEndTime { get; set; }
        public string? Base64string { get; set; }
        public string? FileFormat { get; set; }

        public List<ProjectNamesAPI>? ProjectNamesAPI { get; set; }
    }

    public class ProjectNamesAPI
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
