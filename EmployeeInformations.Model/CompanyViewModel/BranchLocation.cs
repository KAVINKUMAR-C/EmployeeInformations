namespace EmployeeInformations.Model.CompanyViewModel
{
    public class BranchLocation
    {
        public int BranchLocationId { get; set; }
        public string BranchLocationName { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int BranchLocationNameCount { get; set; }
    }

}

