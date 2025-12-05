namespace EmployeeInformations.Model.MasterViewModel
{
    public class SubModules
    {
        public int SubModuleId { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string ModuleName { get; set; }
        public int NameCount { get; set; }
        public int CompanyId { get; set; }
    }

    //public class ModuleName
    //{
    //    public int ModuleId { get; set; }
    //    public string Name { get; set; }
    //}
}
