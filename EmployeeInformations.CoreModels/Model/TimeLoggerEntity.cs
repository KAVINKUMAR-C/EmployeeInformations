using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("TimeLoggers")]
    public class TimeLoggerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 TimeLoggerId { get; set; }
        public Int32 CompanyId { get; set; }
        public Int32 EmployeeId { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string EntryStatus { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 LogSeconds { get; set; }
    }
}
