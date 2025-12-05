

namespace EmployeeInformations.Model.APIModel
{
    public class ZoomMeetingSchedule
    {
        public string topic { get; set; }
        public DateTime start_time { get; set; }
        public int duration { get; set; }
        public bool default_password { get; set; }
        
    }
}
