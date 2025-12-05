using EmployeeInformations.Common.Enums;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;
using System.ComponentModel.DataAnnotations;


namespace EmployeeInformations.Model.OnboardingViewModel
{
     public class OBEmployees
    {

        public int EmpId { get; set; }

        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }
        public int CompanyId { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }
        public string? FatherName { get; set; }

        [Required(ErrorMessage = "OfficeEmail is required.")]
        public string OfficeEmail { get; set; }
        public string? PersonalEmail { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public int DesignationId { get; set; }
        public string ReportingPerson { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }

        [Required(ErrorMessage = "RolleName is required.")]
        public Role RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public int DepartmentId { get; set; }
        // public string DepartmentName { get; set; }
        public bool IsProbationary { get; set; }
        public string? RejectReason { get; set; }
        public ProfileInfo? ProfileInfo { get; set; }
        public List<ReportingPerson> reportingPeople { get; set; }
        public List<int> ReportingPersonEmpId { get; set; }
        public string StrFmtReportingPersonEmpId { get; set; }
        public List<int> SkillId { get; set; }
        public string StrFmtSkillId { get; set; }
        public List<string> SkillNames { get; set; }
        public List<SkillSet> SkillSet { get; set; }
        public List<Designation> Designations { get; set; }
        public List<Department> Departments { get; set; }
        public List<RoleViewModel> RolesTables { get; set; }
        public List<RelievingReason> RelievingReason { get; set; }
        public int? RelieveId { get; set; }
        public int? EsslId { get; set; }
        public string EmployeeUserId { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeSortName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string DateofJoin { get; set; }
        public string ClassName { get; set; }
        public string ReleaveName { get; set; }
        public string ProbationDays { get; set; }
        public int AllAssetId { get; set; }
        public int? BenefitId { get; set; }
        public int? MedicalBenefitId { get; set; }
        public string ProfileCompletionPercentage { get; set; }
    }
    public class OBViewEmployee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string OfficeEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string SkillName { get; set; }
        public string ReportingPerson { get; set; }
        public string EmployeeSortName { get; set; }
    }
}
