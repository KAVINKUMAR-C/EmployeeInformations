using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("AllLeaveDetails")]
    public class AllLeaveDetailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AllLeaveDetailId { get; set; }
        public int LeaveYear { get; set; }
        public int EmpId { get; set; }
        public decimal CasualLeaveCount { get; set; }
        public decimal SickLeaveCount { get; set; }
        public decimal EarnedLeaveCount { get; set; }
        public decimal MaternityLeaveCount { get; set; }
        public decimal CompensatoryOffCount { get; set; }
        public decimal CasualLeaveTaken { get; set; }
        public decimal SickLeaveTaken { get; set; }
        public decimal EarnedLeaveTaken { get; set; }
        public decimal MaternityLeaveTaken { get; set; }
        public decimal CompensatoryOffTaken { get; set; }
        public int CompanyId { get; set; }
    }
}
