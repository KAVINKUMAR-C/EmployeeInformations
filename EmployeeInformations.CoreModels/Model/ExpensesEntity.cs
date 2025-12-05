using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Expenses")]
    public class ExpensesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public string ExpenseTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalAmount { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }
        public int IsApproved { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }

    [Table("ExpenseDetails")]
    public class ExpenseDetailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }
        [ForeignKey("Expenses")]
        public int ExpenseId { get; set; }
        public virtual ExpensesEntity Expenses { get; set; }
        public int EmpId { get; set; }
        public string ExpenseCategory { get; set; }
        public decimal Amount { get; set; }
        public string BillNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? ExpenseName { get; set; }
        public string? Document { get; set; }
        public bool IsDeleted { get; set; }

    }

    [Table("ExpenseAttachments")]
    public class ExpenseAttachmentsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public int DetailId { get; set; }
        public string? ExpenseName { get; set; }
        public string? Document { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
