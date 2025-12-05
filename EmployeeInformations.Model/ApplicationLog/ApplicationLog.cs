namespace EmployeeInformations.Model.ApplicationLog
{
    public class ApplicationLog
    {
        public int ApplicationLogId { get; set; }
        public string? Host { get; set; }
        public string? Path { get; set; }
        public string? Error { get; set; }
        public string? ExecptionMessage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
