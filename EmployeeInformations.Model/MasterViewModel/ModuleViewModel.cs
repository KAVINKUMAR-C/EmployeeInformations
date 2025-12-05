namespace EmployeeInformations.Model.MasterViewModel
{
    public class ModuleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Modules> Modules { get; set; }
    }
}