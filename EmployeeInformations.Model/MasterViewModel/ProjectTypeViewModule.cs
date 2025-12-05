using EmployeeInformations.Model.ProjectSummaryViewModel;

namespace EmployeeInformations.Model.MasterViewModel
{
    public class ProjectTypeViewModule
    {
        public int ProjectTypeId { get; set; }
        public string? ProjectTypeName { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProjectTypeMaster> ProjectTypeMaster { get; set; }
    }

    public class ProjectTypeMaster
    {
        public int ProjectTypeId { get; set; }
        public string? ProjectTypeName { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProjectTypes> ProjectType { get; set; }
        public int ProjectTypeNameCount { get; set; }
    }
}