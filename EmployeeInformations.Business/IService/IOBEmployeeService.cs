using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.OnboardingViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;


namespace EmployeeInformations.Business.IService
{
    public interface IOBEmployeeService
    {
     
        Task<int> CreateEmployee(OBEmployees employees, int sessionEmployeeId,int companyId);
        Task<List<ReportingPerson>> GetAllReportingPerson(int companyId);
        Task<List<Designation>> GetAllDesignation(int companyId);
        Task<WelcomeAboard> GetEmployee(int empId, int companyId);
        Task<List<Department>> GetAllDepartment(int companyId);
        Task<int> GetEmployeeEmail(string officeEmail,int companyId);
        Task<int> GetPersonalEmail(string personalEmail, int empId,int companyId);
        Task<List<RoleViewModel>> GetAllRoleTable(int companyId);
        Task<OBEmployees> GetEmployeeById(int EmpId,int companyId);
        Task<List<SkillSet>> GetAllSkills(int companyId);
        Task<OBProfileInfo> GetProfileByEmployeeId(int empId,int companyId);
        Task<List<BloodGroup>> GetAllBloodGroup();
        Task<List<RelationshipType>> GetAllRelationshipType();
        Task<int> AddProfileInfo(OBProfileInfo profileInfo, int sessionEmployeeId,int companyId);
        Task<OBAddressInfo> GetAddressByEmployeeId(int empId,int companyId);
        Task<List<State>> GetAllStates();
        Task<List<City>> GetAllCities();
        Task<List<Country>> GetAllCountry();
        Task<int> AddAddressInfo(OBAddressInfo addressInfo, int sessionEmployeeId,int companyId);
        Task<List<State>> GetByCountryId(int countryId);
        Task<List<City>> GetByStateId(int StateId);
        Task<OBOtherDetailsViewModel> GetAllOtherDetailsViewModel(int empId,int companyId);
        Task<List<OBDocumentTypes>> GetAllDocumentTypes(int companyId);
        Task<bool> InsertAndUpdateOtherDetails(OBOtherDetails otherDetails, int sessionEmployeeId,int companyId);
        Task DeleteOtherDetails(OBOtherDetails otherDetails);
        Task<List<string>> GetDocumentNameByDetailId(int detailId);
         Task<List<OBOtherDetailsDocumentFilePath>> GetOtherDetailsDocumentAndFilePath(int detailId);
        Task<OBQualificationViewModel> GetAllQulificationViewModel(int empId,int companyId);
        Task<bool> InsertAndUpdateQualification(OBQulification qualification, int sessionEmployeeId,int companyId);
        Task<List<OBQulification>> GetAllQualification(int EmpId,int companyId);
        Task<List<string>> GetDocumentNameByQualificationId(int qualificationId);
        Task<List<OBQualificationDocumentFilePath>> GetQualificationDocumentAndFilePath(int qualificationId);
        Task DeleteQualification(OBQulification qualification);
        Task<OBExperienceViewModel> GetAllExperienceViewModel(int empId,int companyId);
        Task<bool> InsertAndUpdateExperience(OBExperience experience, int sessionEmployeeId);
        Task<List<OBExperience>> GetAllExperience(int EmpId,int companyId);
        Task<List<string>> GetDocumentNameByExperienceId(int experienceId);
        Task<List<OBExperienceDocumentFilePath>> GetExperienceDocumentAndFilePath(int experienceId);
        Task DeleteExperience(OBExperience experience, int sessionEmployeeId);
        Task<OBBankDetails> GetBankDetailsByEmployeeId(int empId,int companyId);
        Task<int> AddBankDetails(OBBankDetails bankDetails, int sessionEmployeeId,int companyId);
        Task<OnboardingCompletion> GetProfileCompletion(int empId, int companyId);
        // view
        Task<OBViewEmployee> GetEmployeeByEmployeeId(int empId, int companyId);
        Task<OBEmployees> GetEmployeeByEmployeeIdView(int empId, int companyId);
        Task<bool> GetRejectEmployees(OBEmployees employees,int companyId);
        Task<OBViewProfile> GetEmployeeProfileByEmployeeId(int empId,int companyId);
        Task<OBViewAddressInfo> GetEmployeeAddressByEmployeeId(int empId, int companyId);
        Task<OBOtherDetailsViewModel> GetAllOtherDetailsView(int empId,int companyId);
        Task<OBQualificationViewModel> GetAllQulificationView(int empId,int companyId);
        Task<OBExperienceViewModel> GetAllExperienceView(int empId,int companyId);
         Task<int> UpdateStatus(int empId,int companyId);
        Task<List<OBEmployees>> GetEmployee(int comapnyId);
        Task<string> GetEmployeeId(int companyId);
    }
}
