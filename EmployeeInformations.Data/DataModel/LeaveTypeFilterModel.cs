namespace EmployeeInformations.Data.DataModel
{
    public class LeaveTypeFilterModel
    {
        public int EmployeeId { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveFromDate { get; set; }
        public string LeaveToDate { get; set; }
    }
}
