using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("ProjectDetails")]
    public class ProjectDetailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        public int EmpId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectDescription { get; set; }
        public string? Technology { get; set; }
        public int? Hours { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public decimal? ProjectCost { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ProjectRefNumber { get; set; }
        public int? ClientCompanyId { get; set; }
        //public int ClientId { get; set; }
        //public string ClientCompany { get; set; }

        //public string ProjectManagerId { get; set; }
        //public string TeamLeadId { get; set; }
        //public string EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string? Symbol { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
