namespace EmployeeInformations.Model.AssetViewModel
{
    public class AllAssetViewModel
    {
        public int AllAssetsId { get; set; }
        public int AssetTypeId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string AssetName { get; set; }
        public string AssetCode { get; set; }
        public string ProductNumber { get; set; }
        public string ModelNumber { get; set; }
        public string? Location { get; set; }
        public string? PurchaseNumber { get; set; }
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
        public List<AllAssets> AllAssets { get; set; }
        public List<AssetStatusName> AssetStatusName { get; set; }
    }

    public class AssetStatusName
    {
        public int AssetStatusId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
