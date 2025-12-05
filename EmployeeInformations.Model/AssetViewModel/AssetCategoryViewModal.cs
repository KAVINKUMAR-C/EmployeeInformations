namespace EmployeeInformations.Model.AssetViewModel
{
    public class AssetCategoryViewModal
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string CompanyName { get; set; }
        public int AssetCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<AssetCategory> AssetCategory { get; set; }
    }
}
