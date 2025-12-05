namespace EmployeeInformations.Model.AssetViewModel
{
    public class AssetTypes
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public string AssetCategoryName { get; set; }
        public int TypeNameCount { get; set; }
    }
}
