using EmployeeInformations.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class ChangePasswordViewModel
    {
        public int EmpId { get; set; }
        public Role RoleId { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string? Password { get; set; }
        public string? OfficeEmail { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? Re_EnterPassword { get; set; }
    }
}
