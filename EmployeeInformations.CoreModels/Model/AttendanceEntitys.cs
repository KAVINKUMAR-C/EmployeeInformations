using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{

    [Table("AttendanceLogs")]
    public class AttendanceEntitys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("S.no")]
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string? LogDateTime { get; set; }
        public string? LogDate { get; set; }
        public string? LogTime { get; set; }
        public string? Direction { get; set; }
    }
}
