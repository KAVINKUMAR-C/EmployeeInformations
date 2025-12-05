using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IEmployeesService
    {
        Task<List<Qualification>> GetAllQualification(int EmpId,int companyId);
        Task<List<Experience>> GetAllExperience(int EmpId, int companyId);
        Task<bool> AcceptProbation(Employees employees, int sessionEmployeeId,int companyId);
        Task<Int32> CreateEmployee(Employees employees, int sessionEmployeeId, int companyId);
        Task<bool> InsertAndUpdateQualification(Qualification qualification, int sessionEmployeeId,int companyId);
        Task<bool> InsertAndUpdateOtherDetails(OtherDetails otherDetails, int sessionEmployeeId);
        Task<bool> UpdateSkill(Employees employees);
        Task DeleteOtherDetails(OtherDetails otherDetails);
        Task DeleteQualification(Qualification qualification);
        Task<bool> InsertAndUpdateExperience(Experience experience, int sessionEmployeeId);
        Task DeleteExperience(Experience experience, int sessionEmployeeId);
        Task<List<State>> GetAllStates();
        Task<List<Country>> GetAllCountry();
        Task<List<SkillSet>> GetAllSkills(int companyId);
        Task<List<DocumentTypes>> GetAllDocumentTypes(int companyId);
        Task<List<City>> GetAllCities();
        Task<List<City>> GetByStateId(int StateId);
        Task<List<State>> GetByCountryId(int countryId);
        Task UpdateNewPassword(LoginViewModel loginViewModel);
        Task<bool> EmployeeIsActiveCheck(string officeEmail);

        // Task<Qualification> GetQualificationByEmployeeId(int empId);
        Task<OtherDetailsViewModel> GetAllOtherDetailsViewModel(int empId,int companyId);
        Task<ExperienceViewModel> GetAllExperienceViewModel(int empId,int companyId);
        Task<int> GetPersonalEmail(string personalEmail, int empId,int companyId);
        Task<bool> GetEmpId(int empId, string CurrentPassword,int companyId);
        Task ChangePassword(string officeEmail);

        //Task<Qualification> GetQualificationByQualificationId(int QualificationId);
        Task<BankDetails> GetBankDetailsByEmployeeId(int empId,int companyId);
        Task<int> AddBankDetails(BankDetails bankDetails, int sessionEmployeeId,int companyId);
        // void GetByCurrentPassword(ChangePasswordViewModel changePasswordViewModel);
        Task<bool> UpdateEmployeeCurrentPassword(ChangePasswordViewModel changePasswordViewModel,int companyId);
        Task<List<BloodGroup>> GetAllBloodGroup();
        Task<bool> GetRejectEmployees(Employees employees);
        Task<List<string>> GetDocumentNameByQualificationId(int qualificationId);
        Task<List<QualificationDocumentFilePath>> GetQualificationDocumentAndFilePath(int qualificationId);
        Task<List<string>> GetDocumentNameByExperienceId(int experienceId);
        Task<List<ExperienceDocumentFilePath>> GetExperienceDocumentAndFilePath(int experienceId);
        Task<List<string>> GetDocumentNameByDetailId(int detailId);
        Task<List<OtherDetailsDocumentFilePath>> GetOtherDetailsDocumentAndFilePath(int detailId);
        Task<List<RelationshipType>> GetAllRelationshipType();
        Task<bool> employeeIsverified(int empId,int companyId);
        Task<EmployeeChangeLogViewModel> GetAllEmployeesChangeLogs(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId);
        Task<EmployeeChangeLogViewModel> GetAllEmployessByEmployeeLogFilter(EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId);
        Task<ExperienceViewModel> GetAllExperienceView(int empId,int companyId);
        Task<OtherDetailsViewModel> GetAllOtherDetailsView(int empId,int companyId);
        Task<List<EmployeeActivityLog>> GetEmployeeActivity(int companyId);
        Task<SalaryViewModel> GetAllSalaryByEmpId(int EmpId);
        Task<int> AddSalaryDetails(salarys salarys, int sessionEmployeeId,int companyId);
        Task<List<salarys>> GetSalaryByEmpId(int EmpId);
        Task DeleteSalary(salarys salarys);
        Task<int> GetBySalaryCount(int month, int empId,int year);
        Task<EmployeeChangeLogViewModel> GetAllEmployeesByEmployeeLogFilter(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId);
        Task<int> GetAllEmployessByEmployeeLogCount(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId);
        Task<List<EmployeeDropdown>> GetAllEmployeesDrropdown(int companyId);
        Task<List<EmployeeDetailsDropdown>> GetAllEmployeeDetailsDrropdown(int companyId);
        Task<EmployeeActivityLog> GetAllEmployeeActivitylogs(SysDataTablePager pager,string columnName, string columnDirection,int companyId);
        Task<int> GetAllEmployeeActivityLogfilterCount(SysDataTablePager pager,int companyId);
        Task<List<ReportingPerson>> GetAllReportingPerson(int companyId);
        Task<Employees> GetEmployeeById(int EmpId, int companyId);
        Task<List<Employees>> GetAllReleaveTypes(int companyId);
        Task<int> GetEmployeesListCount(SysDataTablePager pager, int companyId);
        Task<List<Employees>> GetEmployeesDetailsList(SysDataTablePager pager, string columnDirection, string ColumnName, int companyId);
        Task<ViewEmployee> GetEmployeeByEmployeeId(int empId, int companyId);
        Task<Employees> GetEmployeeByEmployeeIdView(int empId, int companyId);
        Task<string> GetEmployeeId(int companyId);
        Task<int> GetEmployeeEmail(string officeEmail, int companyId);
        Task<ProfileInfo> GetProfileByEmployeeId(int empId, int companyId);
        Task<int> AddProfileInfo(ProfileInfo profileInfo, int sessionEmployeeId, int companyId);
        Task<ViewProfile> GetEmployeeProfileByEmployeeId(int empId, int companyId);
        Task<ViewAddressInfo> GetEmployeeAddressByEmployeeId(int empId, int companyId);
        Task<AddressInfo> GetAddressByEmployeeId(int empId, int companyId);
        Task<int> AddAddressInfo(AddressInfo addressInfo, int sessionEmployeeId, int companyId);
        Task<List<Employees>> GetAllEmployees(int companyId);
        Task<Employees> GetByUserName(LoginViewModel employees);
        Task<List<Designation>> GetAllDesignation(int companyId);
        Task<List<Department>> GetAllDepartment(int companyId);
        Task<List<RoleViewModel>> GetAllRoleTable(int companyId);
        Task<QulificationViewModel> GetAllQulificationView(int empId, int companyId);
        Task<QulificationViewModel> GetAllQulificationViewModel(int empId, int companyId);
        Task<bool> CreateEmployeeActivityLog(int empId, int companyId);
    }
}
