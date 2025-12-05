using EmployeeInformations.CoreModels.DataViewModel;

namespace EmployeeInformations.Model.LeaveSummaryViewModel
{
    public class CompensatoryRequest
    {
        public int CompensatoryId { get; set; }
        public int EmpId { get; set; }
        public DateTime WorkedDate { get; set; }
        public string Remark { get; set; }
        public string? Reason { get; set; }
        public int CompanyId { get; set; }
        public int IsApproved { get; set; }
        public int DayCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string StrWorkedDate { get; set; }

    }

    public class CompensatoryRequestViewModel
    {
        public int CompensatoryId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeUserName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime WorkedDate { get; set; }
        public string Remark { get; set; }
        public string Reason { get; set; }
        public int CompanyId { get; set; }
        public int IsApproved { get; set; }
        public int DayCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public bool EmployeeStatus { get; set; }
        public List<EmployeeCompensatoryFilter>? employeeCompensatoryFilters { get; set; }

    }

    public class ViewCompensatoryOffRequest
    {
        public int CompensatoryId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeUserName { get; set; }
        public string EmployeeName { get; set; }
        public string WorkedDate { get; set; }
        public string Remark { get; set; }
        public string Reason { get; set; }
        public int IsApproved { get; set; }
        public int Status { get; set; }
        public int DayCount { get; set; }
    }
}
