using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Announcement")]
    public class AnnouncementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnnouncementId { get; set; }
        public string AnnouncementName { get; set; }
        public string Description { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public DateTime AnnouncementEndDate { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string? EmpId { get; set; }
        public string? DesignationId { get; set; }
        public string? DepartmentId { get; set; }
        public int AnnouncementAssignee { get; set; }
    }
}
