namespace EmployeeInformations.Model.MasterViewModel
{
    public class DashboardMenus
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public int MenuNameCount { get; set; }
    }
}
