

using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class TeamMeetingModel
    {      
        public string? AttendeeEmail { get; set; }
        public string? MeetingName { get; set; }       
        public int TeamsMeetingId { get; set; }
        public string EmployeeName {  get; set; }     
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    [Keyless]
    public class TeamMeetingCount
    {
        public int TeamCount { get; set; }
    }
}
