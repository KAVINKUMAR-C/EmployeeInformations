using EmployeeInformations.Model.APIModel;

namespace EmployeeInformations.Business.API.IService
{
    public interface ILoginAPIService
    {
        Task<UserEmployeesResponse> LoginDetails(LoginViewRequestModel employees);
    }
}
