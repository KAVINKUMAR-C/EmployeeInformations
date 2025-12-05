using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("BankDetails")]
    public class BankDetailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankId { get; set; }
        [ForeignKey("Employees")]
        public int EmpId { get; set; }
        public virtual EmployeesEntity Employees { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public Int64 AccountNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
