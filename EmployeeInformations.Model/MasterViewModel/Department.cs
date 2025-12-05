namespace EmployeeInformations.Model.MasterViewModel
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentNameCount { get; set; }
    }
}
