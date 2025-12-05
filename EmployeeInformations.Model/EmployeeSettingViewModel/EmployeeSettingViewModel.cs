namespace EmployeeInformations.Model.EmployeeSettingViewModel
{
    public class EmployeeSetting
    {
        public int EmployeeSettingId { get; set; }
        public int CompanyId { get; set; }
        public int ProbationMonths { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
