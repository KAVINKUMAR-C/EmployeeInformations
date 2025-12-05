namespace EmployeeInformations.Model.MasterViewModel
{
    public class SubModulesViewModel
    {
        public int SubModuleId { get; set; }
        public string? Name { get; set; }
        public int ModuleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<SubModules> SubModules { get; set; }
        public List<ModuleName> ModuleName { get; set; }
        public int CompanyId { get; set; }
    }

    public class ModuleName
    {
        public int ModuleId { get; set; }
        public string? Name { get; set; }
        public int Id { get; set; }
    }
}
