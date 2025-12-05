using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("Assets")]
    public class AllAssetsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AllAssetsId { get; set; }
        public int AssetTypeId { get; set; }
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string AssetName { get; set; }
        public string AssetCode { get; set; }
        public string? ProductNumber { get; set; }
        public string? ModelNumber { get; set; }
        public int? PurchaseNumber { get; set; }
        public int? LocationId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Description { get; set; }
        public int? AssetStatusId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? Remark { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? PurchaseOrder { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? VendorName { get; set; }
    }
}
