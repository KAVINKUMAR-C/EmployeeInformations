namespace EmployeeInformations.Model.AssetViewModel
{
    public class AssetTypeViewModal
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public List<AssetTypes> AssetTypes { get; set; }
        public List<AssetCategoryName> AssetCategoryName { get; set; }
    }

    public class AssetCategoryName
    {
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
