using EmployeeInformations.Model.APIModel;

namespace EmployeeInformations.Business.API.IService
{
    public interface IEmployeesAPIService
    {
        Task<UserTimeSheetResponse> InsertEmployees(EmployeesRequestModel employees,int companyId);
        Task<EmployeesRequestModel> GetEmployeeById(int empId, int companyId);
        Task<bool> GetRejectEmployees(EmployeesRequestModel employees);
        Task<List<EmployeesRequestModel>> GetAllEmployees(int empId, int companyId);
        Task<List<RelievingReasons>> GetAllRelievingReason(int companyId);
        Task<List<ReportingPersons>> GetAllReportingPersons(int companyId);
        Task<string> GetEmployeeUserName(int companyId);
        Task<List<Designations>> GetAllDesignation(int companyId);
        Task<List<RoleViewModels>> GetAllRoleTable(int companyId);
        Task<List<Departments>> GetAllDepartment(int companyId);

    }
}
