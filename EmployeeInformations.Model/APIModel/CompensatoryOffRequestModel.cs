namespace EmployeeInformations.Model.APIModel
{
    public class CompensatoryOffRequestModel
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
    }



    public class CompensatoryRequestAPI
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
}
