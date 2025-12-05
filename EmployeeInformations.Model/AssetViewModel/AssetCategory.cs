namespace EmployeeInformations.Model.AssetViewModel
{
    public class AssetCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public int AssetCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
		public int CategoryNameCount { get; set; }
		public int CategoryCodeCount { get; set; }
	}
}
