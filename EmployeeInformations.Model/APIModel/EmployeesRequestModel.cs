using EmployeeInformations.Common.Enums;
using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.APIModel
{
    public class EmployeesRequestModel
    {
        public int EmpId { get; set; }

        //[Required(ErrorMessage = "UserName is required.")]
        public string? UserName { get; set; }
        public int CompanyId { get; set; }
        public string? Password { get; set; }

        //[Required(ErrorMessage = "FirstName is required.")]
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "LastName is required.")]
        public string? LastName { get; set; }
        public string? FatherName { get; set; }

        //[Required(ErrorMessage = "OfficeEmail is required.")]
        public string? OfficeEmail { get; set; }
        public string? PersonalEmail { get; set; }

        //[Required(ErrorMessage = "Designation is required.")]
        public int DesignationId { get; set; }
        public string? ReportingPerson { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }

        //[Required(ErrorMessage = "RolleName is required.")]
        public Role RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public int DepartmentId { get; set; }
        public bool IsProbationary { get; set; }
        public string? RejectReason { get; set; }
        public ProfileInfoRequestModel? ProfileInfo { get; set; }
        public List<ReportingPersons>? reportingPeople { get; set; }
        public List<int>? ReportingPersonEmpId { get; set; }
        public string? StrFmtReportingPersonEmpId { get; set; }
        public List<int>? SkillId { get; set; }
        public string? StrFmtSkillId { get; set; }
        public List<string>? SkillNames { get; set; }
        public List<SkillSet>? SkillSet { get; set; }
        public List<Designations>? Designations { get; set; }
        public List<Departments>? Departments { get; set; }
        public List<RoleViewModels>? RolesTables { get; set; }
        public List<RelievingReasons>? RelievingReason { get; set; }
        public int? RelieveId { get; set; }
        public int? EsslId { get; set; }
        public string? EmployeeUserId { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmployeeSortName { get; set; }
        public string? EmployeeProfileImage { get; set; }
        public string? DateofJoin { get; set; }
        public string? ClassName { get; set; }
        public string? ReleaveName { get; set; }
        public string? ProbationDays { get; set; }
        public int AllAssetId { get; set; }
        public int? BenefitId { get; set; }
        public int? MedicalBenefitId { get; set; }
        public string? ProfileCompletionPercentage { get; set; }
    }

    public class EmployeesLoginModel
    {
        public Role RoleId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int EmpId { get; set; }
        public string? RoleName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public int CompanyId { get; set; }
    }

    public class ReportingPersons
    {
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }

    }

    public class Designations
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
    }

    public class Departments
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

    }

    public class RoleViewModels
    {
        public Role RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class CompanyCode
    {
        public string UserName { get; set; }
    }

    public class RelievingReasons
    {
        public int RelieveId { get; set; }
        public string ReleavingType { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
    }

}
