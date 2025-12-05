using EmployeeInformations.CoreModels.DataViewModel;

namespace EmployeeInformations.Model.CompanyViewModel
{
    public class MailScheduler
    {
        public int SchedulerId { get; set; }
        public int CompanyId { get; set; }
        public int FileFormat { get; set; }
        public string ReportName { get; set; }
        public string WhomToSend { get; set; }
        public string MailSendingDays { get; set; }
        public List<string> EmployeeId { get; set; }
        public List<string> SendigStatus { get; set; }
        public int DurationId { get; set; }
        public int EmailDraftId { get; set; }
        public string StrMailDate { get; set; }
        public string StrMailTime { get; set; }
        public DateTime MailDate { get; set; }
        public DateTime MailTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class MailSchedulerViewModels
    {
        public int SchedulerId { get; set; }
        public int CompanyId { get; set; }
        public int FileFormat { get; set; }
        public string ReportName { get; set; }
        public string EmployeeId { get; set; }
        public string MailSendingDays { get; set; }
        public int EmailDraftId { get; set; }
        public string WhomToSend { get; set; }
        public List<string> EmployeeIds { get; set; }
        public List<int> SendigStatus { get; set; }
        public int DurationId { get; set; }
        public DateTime MailDate { get; set; }
        public DateTime MailTime { get; set; }
        public string StrMailDate { get; set; }
        public string StrMailTime { get; set; }
        public bool IsActive { get; set; }
        public List<Duration> Durations { get; set; }
        public List<FileFormats> FileFormats { get; set; }
        public List<DropdownEmployee> DropdownEmployee { get; set; }
        public List<MailSchedulerViewModel> mailSchedulerViewModel { get; set; }

        public List<EmailSchedulerViewModel>? emailSchedulerViewModels { get; set; }
    }

    public class DropdownEmployee
    {
        public int EmpId { get; set; }
        public string? FirstName { get; set; }
    }

    public class FileFormats
    {
        public int FileId { get; set; }
        public string FileFormatName { get; set; }
    }

    public class Duration
    {
        public int DurationId { get; set; }
        public string DurationName { get; set; }
    }

    public class MailSchedulerViewModel
    {
        public int SchedulerId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int FileFormat { get; set; }
        public string FileFormating { get; set; }
        public string ReportName { get; set; }
        public string WhomToSend { get; set; }
        public string MailSendingDays { get; set; }
        public int DurationId { get; set; }
        public int? EmailDraftId { get; set; }
        public string Duration { get; set; }
        public DateTime MailDate { get; set; }
        public DateTime MailTime { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
