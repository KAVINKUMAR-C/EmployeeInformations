namespace EmployeeInformations.Model.MasterViewModel
{
    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public List<Department> Department { get; set; }
    }
}
