using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class EmployeesDetailsDataModel
    {
        public int EmpId { get; set; }
        public string? EmployeeName { get; set; }
        public string? UserName { get; set; }
        public string? FatherName { get; set; } 
        public string? OfficeEmail { get; set; }
        public string? PersonalEmail { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsOnboarding { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsProbationary { get; set;}
        public byte? RoleId {  get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string? RejectReason { get; set; }
        public int? RelieveId { get; set;}
        public int? EsslId { get; set; }
        public int? CompanyId { get; set; }
        public string? EmployeeProfileImage { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfRelieving { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? ReleaveName { get; set; }
        public int? AllAssetId { get; set; }
        public int? BenefitId { get; set; }      
    }
}
