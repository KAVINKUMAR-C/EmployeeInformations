namespace EmployeeInformations.Model.MasterViewModel
{
    public class Modules
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<ModuleName> ModuleName { get; set; }
        public int NameCount { get; set; }
        public int CompanyId { get; set; }
    }
}