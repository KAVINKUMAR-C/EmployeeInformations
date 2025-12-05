using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("TeamsMeeting")]
    public class TeamsMeetingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamsMeetingId { get; set; }
        public string MeetingName { get; set; }
        public string AttendeeEmail { get; set; }
        public DateTime StartDate { get;set; }
        public DateTime EndDate { get;set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string MeetingId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
