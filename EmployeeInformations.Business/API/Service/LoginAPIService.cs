using AutoMapper;
using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.APIModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.API.Service
{
    public class LoginAPIService : ILoginAPIService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IMasterRepository _masterRepository;

        public LoginAPIService(IEmployeesRepository employeesRepository, IMapper mapper, IConfiguration config, IMasterRepository masterRepository)
        {
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _config = config;
            _masterRepository = masterRepository;
        }


        public async Task<UserEmployeesResponse> LoginDetails(LoginViewRequestModel employees)
        {
            var userEmployeesResponse = new UserEmployeesResponse();
            if (employees != null)
            {
                var employeePassword = employees.Password.Trim();
                var password = Common.Common.sha256_hash(employeePassword);
                var data = await _employeesRepository.GetByUserName(employees.UserName, password);
                var employee = new EmployeesLoginModel();
                var department = await _masterRepository.GetDepartmentByEmployeeId(data.DepartmentId,data.CompanyId);
                var designation = await _masterRepository.GetDesignationByEmployeeId(data.DesignationId,data.CompanyId);
                var role = await _masterRepository.GetRoleById(data.RoleId,data.CompanyId);
                if (data != null)
                {
                    employee.EmpId = data.EmpId;
                    employee.RoleId = (Role)data.RoleId;
                    employee.CompanyId = data.CompanyId;
                    employee.DepartmentId = data.DepartmentId;
                    employee.DesignationId = data.DesignationId;
                    employee.DepartmentName = department.DepartmentName;
                    employee.DesignationName = designation.DesignationName;
                    employee.RoleName = role.RoleName;
                }
                userEmployeesResponse.Employees = employee;
                userEmployeesResponse.IsSuccess = true;
                userEmployeesResponse.Message = Common.Constant.Success;
            }
            else
            {
                userEmployeesResponse.IsSuccess = false;
                userEmployeesResponse.Message = Common.Constant.Failure;
            }
            return userEmployeesResponse;

        }
    }
}