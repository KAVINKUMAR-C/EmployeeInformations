namespace EmployeeInformations.Model.AssetViewModel
{
    public class AssetBrandTypeViewModel
    {
        public int BrandTypeId { get; set; }
        public string BrandType { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<AssetBrandType> AssetBrandType { get; set; }
    }
}
