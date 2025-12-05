namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class ExperienceViewModel
    {
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public List<Experience>? Experiences { get; set; }
        public string? DocumentFilePath { get; set; }
    }
}
