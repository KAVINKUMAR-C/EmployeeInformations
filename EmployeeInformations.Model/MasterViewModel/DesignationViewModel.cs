namespace EmployeeInformations.Model.MasterViewModel
{
    public class DesignationViewModel
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public List<Designation> Designation { get; set; }
    }
}
