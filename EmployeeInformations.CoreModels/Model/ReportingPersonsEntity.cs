using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("ReportingPersons")]
    public class ReportingPersonsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportingPersonId { get; set; }
        public int EmployeeId { get; set; }
        public int ReportingPersonEmpId { get; set; }
        public int CompanyId { get; set; }

    }
}
