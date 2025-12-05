using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.CompanyViewModel
{
    public class CompanySetting
    {
        public int CompanySettingId { get; set; }
        public int CompanyId { get; set; }
        public int ModeId { get; set; }
        public string TimeZone { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public string GSTNumber { get; set; }
        public string CompanyCode { get; set; }
        public bool IsTimeLockEnable { get; set; }
        public bool IsDeleted { get; set; }
        public List<Country> countrys { get; set; }
        public List<Company>? Company { get; set; }
    }
}
