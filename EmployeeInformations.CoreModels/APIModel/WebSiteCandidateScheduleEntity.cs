
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_CandidateSchedule")]
    public class WebSiteCandidateScheduleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CandidateScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }


    }
}
