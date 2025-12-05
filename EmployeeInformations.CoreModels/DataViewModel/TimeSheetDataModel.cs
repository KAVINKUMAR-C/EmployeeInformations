namespace EmployeeInformations.CoreModels.DataViewModel
{
    public class TimeSheetDataModel
    {
        public Int32 Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ProjectId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int Status { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentFilePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeUserId { get; set; }
        public int CompanyId { get; set; }
    }
}
