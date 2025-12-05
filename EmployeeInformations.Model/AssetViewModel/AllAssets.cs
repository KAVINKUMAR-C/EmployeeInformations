using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeDropdown = EmployeeInformations.Model.ReportsViewModel.EmployeeDropdown;

namespace EmployeeInformations.Model.AssetViewModel
{
    public class AllAssets
    {
        public int AllAssetsId { get; set; }
        public int AssetTypeId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string AssetName { get; set; }
        public string AssetCode { get; set; }
        public string? ProductNumber { get; set; }
        public string? ModelNumber { get; set; }
        public int? LocationId { get; set; }
        public int? PurchaseNumber { get; set; }
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
        public string AssetStatusName { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetCategoryName { get; set; }
        public string AssetEmployeeName { get; set; }
        public string LocationName { get; set; }
        public string brandName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<AssetStatus>? AssetStatus { get; set; }
        public List<AssetTypes>? AssetTypes { get; set; }
        public List<AssetCategory>? AssetCategory { get; set; }
        public List<Employees>? Employees { get; set; }
        public List<AssetBrandType>? AssetBrandType { get; set; }
        public List<BranchLocation>? BranchLocations { get; set; }
        public List<EmployeeDropdown>? reportingPeople { get; set; }
        public string StrPurchaseDate { get; set; }
        public string StrWarrantyDate { get; set; }
        public string StrReturnDate { get; set; }
        public string StrIssueDate { get; set; }
        public string StrWarrantyToDate { get; set; }
        public int AssetLogs { get; set; }
        public AssetLogEntity AssetLog { get; set; }
        public string ClassName { get; set; }
        public string? PurchaseOrder { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? VendorName { get; set; }
        public string? PurchaseNumberName { get; set; }
        public List<AssetViewModels>? assetViewModels { get; set; }
    }


    public class ViewAssets
    {
        public int AllAssetsId { get; set; }
        public string AssetTypeName { get; set; }
        public string CategoryName { get; set; }
        public string AssetName { get; set; }
        public string AssetCode { get; set; }
        public int Location { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Description { get; set; }
        public string? AssetStatusName { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? Remark { get; set; }
        public string? LocationName { get; set; }
        public string? ProductNumber { get; set; }
        public string BrandId { get; set; }
        public string? ModelNumber { get; set; }
        public int? PurchaseNumber { get; set; }
        public string? PurchaseOrder { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? VendorName { get; set; }
        public string? PurchaseNumberName { get; set; }

    }

    public class companylocations
    {
        public int PhysicalAddressState { get; set; }
        public List<BranchLocation>? BranchLocations { get; set; }
        public int CompanyId { get; set; }
        public string State { get; set; }

    }
}
