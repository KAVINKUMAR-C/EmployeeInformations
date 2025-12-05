namespace EmployeeInformations.Model.MasterViewModel
{
    public class Designation
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int DesignationNameCount { get; set; }
    }
}
