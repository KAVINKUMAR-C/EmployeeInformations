using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.ProjectSummaryViewModel
{
    public class ProjectDetails
    {
        public int ProjectId { get; set; }
        public int EmpId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectDescription { get; set; }
        public string Technology { get; set; }
        public int? Hours { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public decimal? ProjectCost { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ProjectRefNumber { get; set; }
        public int? ClientCompanyId { get; set; }
        public string? ProjectManagerId { get; set; }
        public string? TeamLeadId { get; set; }
        public string? EmployeeId { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? ClientCompanyName { get; set; }
        public string? TechnologyName { get; set; }
        public List<ProjectTypes>? ProjectTypes { get; set; }
        public List<ClientCompanys>? ClientCompany { get; set; }
        //public List<DropdownProjectManager> DropdownProjectManager { get; set; }
        //public List<DropdownTeamLead> DropdownTeamLead { get; set; }
        //public List<DropdownEmployee> DropdownEmployee { get; set; }
        //public List<EmployeeProfileImageName> EmployeeProfileImageNames { get; set; }
        //public List<EmployeeProfileImageName> TeamLeadProfileImageNames { get; set; }
        //public List<EmployeeProfileImageName> ProjectManagerProfileImageNames { get; set; }
        public List<SkillSet>? SkillSets { get; set; }
        public string Symbol {  get; set; }

        public string CurrencyCode { get; set; }

        //public List<DropdownProjectManagers> DropdownProjectManagers { get; set; }
        //public List<DropdownTeamLeads> DropdownTeamLeads { get; set; }
        public List<CurrencyViewModel> Currency { get; set; }

        // Datetime issue
        public string StrStartdate { get; set; }
        public string StrEnddate { get; set; }
        public int employeeIds { get; set; }
        public string ClassName { get; set; }
    }

    public class EmployeeProfileImageName
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string SortUserName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
    }


    public class DropdownProjectManager
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
    }

    public class DropdownTeamLead
    {
        public int EmpId { get; set; }
        public string? UserName { get; set; }
    }

    public class DropdownEmployee
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmpName { get; set; }
        public string UserName { get; set; }

    }

    public class DropdownProjectManagers
    {
        public int EmpId { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

    }
    public class DropdownTeamLeads
    {
        public int EmpId { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }


    public class ClientCompanys
    {
        public int ClientId { get; set; }
        public string ClientCompany { get; set; }
        public string ClientCompanyId { get; set; }
    }

    public class SkillSets
    {
        public int SkillId { get; set; }
        public string? SkillName { get; set; }
        public string? Technology { get; set; }

    }


    public class ViewProjectDetails
    {
        public int ProjectId { get; set; }
        public int EmpId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectTypeId { get; set; }
        public string ProjectDescription { get; set; }
        public string Technology { get; set; }
        public int Hours { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public decimal? ProjectCost { get; set; }

        public string ProjectRefNumber { get; set; }
        public string ClientCompanyId { get; set; }
        public string ProjectManagerId { get; set; }
        public string TeamLeadId { get; set; }
        public string EmployeeId { get; set; }
        public string ProjectTypeName { get; set; }
        public string ClientCompanyName { get; set; }
        public string TechnologyName { get; set; }
        public string CurrencyCode { get; set; }
    }


}
