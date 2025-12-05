


using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class EmployeeLeaveReportDataModel
    {

     
        public int Id { get; set; }
        public string? EmployeeUserId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal? LeaveCount { get; set; }
        public string? LeaveType { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public string? Reason { get; set; }
        public int AppliedLeaveTypeId { get; set; }
   
    }
}
