

using EmployeeInformations.CoreModels.DataViewModel;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EmployeeInformations.Model.TeamsViewModel
{
    public class Teams
    {       
        public DateTime Start { get; set; }          
        public string? AttendeeEmail { get; set; }        
        public string? MeetingName { get; set; } 
        public int CompanyId { get; set; }
        public int TeamsMeetingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string Code { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string MeetingStartTime { get; set; }
        public string MeetingEndTime { get; set; }
        public string StrStartdate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MeetingId { get; set; }
        public List<TeamMeetingModel> TeamMeetingModels { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }


    public class MeetingRequest
    {
        public string subject { get; set; }
        public MeetingBody body { get; set; }
        public MeetingTime start { get; set; }
        public MeetingTime end { get; set; }
        public Location location { get; set; }
        public List<Attendee>? attendees { get; set; }
        public bool allowNewTimeProposals { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid? TransactionId { get; set; }
    }

    public class MeetingTime
    {
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class MeetingBody
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }


    public class TokenResponse
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    public class Location
    {
        public string displayName { get; set; }
    }


    public class Attendee
    {
        public EmailAddress? emailAddress { get; set; }
        public string? type { get; set; }
    }

    public class EmailAddress
    {
        public string? address { get; set; }
        public string? name { get; set; }
    }

}
