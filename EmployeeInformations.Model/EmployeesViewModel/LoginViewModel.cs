using EmployeeInformations.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        public int EmpId { get; set; }
        public Role RoleId { get; set; }
        public string? OfficeEmail { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
        public byte[]? ProfileImage { get; set; }
    }

}
