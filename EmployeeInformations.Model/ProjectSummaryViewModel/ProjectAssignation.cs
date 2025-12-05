namespace EmployeeInformations.Model.ProjectSummaryViewModel
{
    public class ProjectAssignation
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int ProjectId { get; set; }
        // public int Type { get; set; }
        public string ProjectManagerId { get; set; }
        public string TeamLeadId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public int employeeIds { get; set; }

        public List<EmployeeProfileImageNames> EmployeeProfileImageNames { get; set; }
        public List<TeamLeadProfileImageNames> TeamLeadProfileImageNames { get; set; }
        public List<ProjectManagerProfileImageNames> ProjectManagerProfileImageNames { get; set; }
        public List<DropdownProjectManager> DropdownProjectManager { get; set; }
        public List<DropdownTeamLead> DropdownTeamLead { get; set; }
        public List<DropdownEmployee> DropdownEmployee { get; set; }
        public List<ProjectManager> ProjectManager { get; set; }
        public List<ProjectTeamLead> ProjectTeamLead { get; set; }
        public List<ProjectEmployee> ProjectEmployee { get; set; }
        public List<ProjectDetails> ProjectDetails { get; set; }
        public ProjectAssignationName ProjectAssignationName { get; set; }

        public List<DropdownTeamLeads> DropdownTeamLeads { get; set; }
        public List<DropdownProjectManagers> DropdownProjectManagers { get; set; }

        public string StrFmtProjectmanagerEmpId { get; set; }
        public string StrFmtTeamLeadEmpId { get; set; }
        public string StrFmtEmployeeEmpId { get; set; }
        public string ClassName { get; set; }
        public string? ProjectName { get; set; }
        public List<string> ProjectManagers { get; set; }
        public List<string> TeamLeads { get; set; }
        public List<string> Employees { get; set; }

        public List<ProjectAssignationViewModel> projectAssignationViewModels { get; set; }
    }


    public class ProjectAssignationViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ClassName { get; set; }
        public List<ProjectManagerProfileImageNames> ProjectManagerProfileImageNames { get; set; }
        public List<TeamLeadProfileImageNames> TeamLeadProfileImageNames { get; set; }
        public List<EmployeeProfileImageNames> EmployeeProfileImageNames { get; set; }

    }

    public class ProjectAssignationName
    {
        public string ProjectManagerName { get; set; }
        public string TeamLeadName { get; set; }
        public string EmployeeName { get; set; }
        public string StrFmtProjectmanagerEmpId { get; set; }
        public string StrFmtTeamLeadEmpId { get; set; }
        public string StrFmtEmployeeEmpId { get; set; }
    }

    public class ProjectManager
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
    }

    public class ProjectTeamLead
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
    }
    public class ProjectEmployee
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
    }


    public class EmployeeProfileImageNames
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string SortUserName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }

    public class ProjectManagerProfileImageNames
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string SortUserName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }

    public class TeamLeadProfileImageNames
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string SortUserName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }
}
