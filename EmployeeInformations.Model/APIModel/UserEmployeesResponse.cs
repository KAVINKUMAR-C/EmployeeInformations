namespace EmployeeInformations.Model.APIModel
{
    public class UserEmployeesResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public EmployeesLoginModel? Employees { get; set; }
    }
}
