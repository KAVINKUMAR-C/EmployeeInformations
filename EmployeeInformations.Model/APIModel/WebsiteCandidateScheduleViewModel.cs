
namespace EmployeeInformations.Model.APIModel
{
    public class WebsiteCandidateScheduleViewModel
    {
        public int CandidateScheduleId { get; set; }
        public string? ScheduleName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }

        public List<WebsiteCandidateSchedule> websiteCandidateSchedules { get; set; }
    }

    public class WebsiteCandidateSchedule
    {
        public int CandidateScheduleId { get; set; }
        public string? ScheduleName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public int CandidateScheduleCount { get; set; }


    }
}
