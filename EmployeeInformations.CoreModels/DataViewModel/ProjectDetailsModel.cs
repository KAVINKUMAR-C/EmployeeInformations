using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class ProjectDetailsModel
    {
        public int ProjectId { get; set; }
        public int EmpId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectDescription { get; set; }
        public string Technology { get; set; }
        public int? Hours { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public decimal? ProjectCost { get; set; }

        public string ProjectRefNumber { get; set; }
        public int ClientCompanyId { get; set; }
        public string ProjectTypeName { get; set; }
        public string ClientCompanyName { get; set; }
        public string TechnologyName { get; set; }
        public string? CurrencyCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
