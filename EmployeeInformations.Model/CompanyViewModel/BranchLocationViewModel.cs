namespace EmployeeInformations.Model.CompanyViewModel
{
    public class BranchLocationViewModel
    {
        public int BranchLocationId { get; set; }
        public string BranchLocationName { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<BranchLocation> BranchLocation { get; set; }
    }
}
