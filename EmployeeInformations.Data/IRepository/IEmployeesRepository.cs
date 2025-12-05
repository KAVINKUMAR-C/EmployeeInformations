using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.Model;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IEmployeesRepository
    {
        Task<EmployeesEntity> GetByUserName(string username, string password);
        Task<List<EmployeesEntity>> GetAllEmployeeDetails(int companyId);
        Task<List<EmployeesEntity>> GetTeambyId(List<int> empIds,int companyId);

        // Task<List<Qualification>> GetAllQualification(int EmpId);
        Task<List<QualificationEntity>> GetAllQualification(int EmpId,int companyId);
        Task<List<OtherDetailsEntity>> GetAllOtherDetails(int EmpId,int companyId);
        //Task<List<Experience>> GetAllExperience(int EmpId);
        Task<List<ExperienceEntity>> GetAllExperience(int EmpId, int companyId);
        Task<EmployeesEntity> GetEmployeeById(int EmpId,int companyId);
        Task<EmployeesEntity> GetEmployeeByEssId(int esslId,int companyId);
        Task<EmployeesEntity> GetEmployeeByEssIdIsActive(int esslId,int companyId);
        Task<EmployeesEntity> GetEmployeeByIdForLeave(int EmpId,int companyId);
        Task<List<ReportingPersonsEntity>> GetAllReportingPersonsEmpIdForLeave(int empId,int companyId);
        Task<List<ReportingPersonsEntity>> GetAllEmployeeIdsReportingPersonForLeave(int empId,int companyId);
        Task<List<int>> GetReportingPersonEmployeeById(int EmpId,int companyId);
        Task<bool> AcceptProbation(Employees employees, int sessionEmployeeId,int companyId);
        Task<Int32> CreateEmployee(EmployeesEntity employeesEntity,int companyId);
        Task<Int32> CreateEmployeeFromCompany(EmployeesEntity employeesEntity);
        Task CreateReportingPersons(List<ReportingPersonsEntity> reportingPersonsEntitys, int EmpId);
        Task<Int32> InsertAndUpdateQualification(QualificationEntity qualification);
        Task<bool> InsertQualificationAttachment(List<QualificationAttachmentsEntity> qualificationAttachments, int qualificationId);
        Task<bool> InsertExperienceAttachment(List<ExperienceAttachmentsEntity> experienceAttachments, int experienceId);
        Task<bool> InsertOtherDetailsAttachment(List<OtherDetailsAttachmentsEntity> otherDetailsAttachments, int detailId);
        Task<int> InsertAndUpdateOtherDetails(OtherDetailsEntity otherDetailsEntity);
        Task DeleteQualification(QualificationEntity qualification);
        Task DeleteQualificationAttachement(List<QualificationAttachmentsEntity> qualificationAttachmentsEntity);
        Task DeleteOtherDetailsAttachement(List<OtherDetailsAttachmentsEntity> otherDetailsAttachmentsEntity);
        Task DeleteOtherDetails(OtherDetailsEntity otherDetailsEntity);
        Task<bool> UpdateSkill(EmployeesEntity employeesEntitys);
        Task<int> InsertAndUpdateExperience(ExperienceEntity experienceEntity);
        Task DeleteExperienceAttachement(List<ExperienceAttachmentsEntity> experienceAttachmentsEntity);
        Task DeleteExperience(ExperienceEntity experienceEntity);
        Task<List<StateEntity>> GetAllStates();
        Task<List<CountryEntity>> GetAllCountry();
        Task<List<DocumentTypesEntity>> GetAllDocumentTypes(int companyId);
        Task<List<SkillSetEntity>> GetAllSkills(int companyId);
        Task<List<CityEntity>> GetAllCities();
        Task<List<CityEntity>> GetByStateId(int StateId);
        Task<List<StateEntity>> GetByCountryId(int countryId);
        Task<List<EmployeesEntity>> GetByStatus(bool statusId, int companyId);
        Task<EmployeesEntity> getEmloyeeDetailByOfficeEmail(string officeEmail);
        Task UpdateNewPassword(EmployeesEntity employeesEntity);
        Task<int> GetEmployeeMaxCount(int companyId);
        Task<int> GetTopFiveEmployees(int companyId);
        Task<string> GetDesignationNameByDesignationId(int designationId);
        Task<List<EmployeesEntity>> GetAllEmployeesByBackground();
        Task<string> GetDepartmentByDepartmentName(int departmentId);
        Task<List<EmployeesEntity>> GetEmployeesByIsProbationary();
        Task<List<ReportingPersonsEntity>> GetAllReportingPersonsEmpId(int empId);
        Task<int> GetActiveEmployeeMaxCount(int companyId);
        Task<bool> EmployeeIsActiveCheck(string officeEmail);
        Task<int> AddProfileInfo(ProfileInfoEntity profileInfoEntity, bool isAdd = false);
        Task<int> AddAddressInfo(AddressInfoEntity addressInfoEntity);
        Task<List<EmployeesEntity>> GetAllActiveEmployees(int companyId);
        Task<OtherDetailsEntity> GetOtherDetailsByEmployeeId(int empId,int companyId);

        Task<QualificationEntity> GetQualificationByEmployeeId(int empId,int companyId);

        Task<List<ExperienceEntity>> GetAllExperienceViewModel(int empId, int companyId);
        Task<List<OtherDetailsEntity>> GetAllOtherDetailsViewModel(int empId,int companyId);
        Task<ExperienceEntity> GetExperienceByEmployeeId(int empId,int companyId);

        Task<int> GetPersonalEmail(string personalEmail);
        Task<QualificationEntity> GetQualificationByQualificationId(int QualificationId,int companyId);
        Task<OtherDetailsEntity> GetotherDetailsBydetailId(int detailId,int companyId);
        Task<int> AddBankDetails(BankDetailsEntity bankDetailsEntity);
        Task<BankDetailsEntity> GetBankDetailsByEmployeeId(int empId,int companyId);
        Task<ExperienceEntity> GetExperienceByExperienceId(int ExperienceId,int companyId);

        //void GetByCurrentPassword(ChangePasswordViewModel changePasswordViewModel);
        Task<bool> UpdateEmployeeCurrentPassword(EmployeesEntity employeesEntity);
        Task<List<BloodGroupEntity>> GetAllBloodGroup();
        Task<string> GetDepartmentNameByDepartmentId(int departmentId,int companyId);
        Task<string> GetCountryNameByCountryId(int countryId);
        Task<string> GetStateNameByStateId(int stateId);
        Task<string> GetCityNameByCityId(int cityId);
        Task<string> GetBloodGroupNameById(int bloodGroupId);
        Task<bool> GetRejectEmployees(Employees employees);
        Task<List<string>> GetDocumentNameByQualificationId(int qualificationId);
        Task<List<QualificationAttachmentsEntity>> GetQualificationDocumentAndFilePath(int qualificationId);
        Task<List<string>> GetDocumentNameByExperienceId(int experienceId);
        Task<List<ExperienceAttachmentsEntity>> GetExperienceDocumentAndFilePath(int experienceId);
        Task<List<string>> GetDocumentNameByDetailId(int detailId);
        Task<List<OtherDetailsAttachmentsEntity>> GetOtherDetailsDocumentAndFilePath(int detailId);
        Task<List<RelationshipTypeEntity>> GetAllRelationshipType();

        Task<bool> employeeIsverified(int empId,int companyId);
        Task<List<EmployeesLogEntity>> GetAllEmployeesLog(int companyId);
        Task<ReportingPersonsEntity> GetReportingPersonEmployeeId(int EmpId,int companyId);
        Task<QualificationAttachmentsEntity> GetQualificationAttachmentsByEmployeeId(int qualificationId);
        Task<ExperienceAttachmentsEntity> GetExperienceAttachmentsByEmployeeId(int experienceId);
        Task<OtherDetailsAttachmentsEntity> GetOtherDetailsAttachmentsByEmployeeId(int detailId);
        Task<List<EmployeesLogReportDataModel>> GetAllEmployessByEmployeeLogFilter(string proc, List<KeyValuePair<string, string>> values);
        Task<List<OtherDetailsAttachmentsEntity>> GetAllOtherDetailsAttachment(int detailId);

        Task<List<QualificationAttachmentsEntity>> GetAllQualificationAttachments(int qualificationId);
        Task<List<ExperienceAttachmentsEntity>> GetAllExperienceAttachment(int experienceId);

        Task<EmployeesEntity> GetEmployeeByname(int ReportingPersonId,int companyId);
        Task<List<AttendanceEntitys>> GetAllEmployeeAttendaceByDate(DateTime logDate);
        Task<List<ExperienceEntity>> GetAllExperienceView(int empId,int companyId);
        Task<List<OtherDetailsEntity>> GetAllOtherDetailsView(int empId, int companyId);
        Task<bool> CreateEmployeeLogActivity(EmployeeActivityLogEntity employeeLogActivityEntity,int companyId);
        Task<EmployeeActivityLogEntity> GetEmployeeLoginDetails(int empId,int companyId);  
        Task<string> GetCountryNameBySecondaryCountryId(int? SecondaryCountryId);
        Task<string> GetStateNameBySecondaryStateId(int? SecondaryStateId);
        Task<string> GetCityNameBySecondaryCityId(int? SecondaryCityId);
        Task<ProfileInfoEntity> GetProfileByEmpId(int empId);
        Task<List<EmployeesEntity>> GetEmployeesByCompnayId(int companyId);
        Task<List<MailSchedulerEntity>> GetMailSchedulerEntity(int companyId);
        Task<List<EmployeesEntity>> GetAllEmployeeByCompany(int id);
        Task<MailSchedulerEntity> GetMailSchedulerbyId(int id);
        Task<List<EmployeeWorkAnniversary>> EmployeesWorkAnniversary(int companyId);
        Task<List<EmployeeBirthday>> EmployeeBirthdayCelebration(int companyId);
        Task<List<EmployeeProbation>> EmployeeProbationCelebration(int companyId);
        Task<List<Celebration>> GetTopFiveCelebration(int companyId);
        Task<List<EmployeesList>> GetEmployeesTopFive(int companyId);
        Task<List<BackgroundEmailQueueModel>> EmailQueueSendMail(int companyId);
        Task<List<Employees>> GetAllEmployee(int companyId);
        Task<List<EmployeeActivityLog>> GetAllEmployeeActivityLogs(int comapnyId);
        Task<List<EmployeesEntity>> GetDesignationById(List<int> DesignationId, int companyId);      
        Task<int> AddSalaryDetails(SalaryEntity salaryEntity);
        Task<SalaryEntity> GetBysalaryId(int salaryId);
        Task DeleteSalary(SalaryEntity salaryEntity);
        Task<int> GetSalaryMonth(int month, int empId,int year);
        Task<List<salarys>> GetSalaryByEmpId(int empId);
        Task<List<EmployeeActivityLogViewModel>> GetEmployeeActivitylogs(SysDataTablePager pager ,string columnName, string columnDirection,int companyId);
        Task<int> GetAllEmployeeActivityLogfilterCount(SysDataTablePager pager, int companyId);
        Task<List<EmployeeLog>> GetAllEmployessByFilter(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId);
        Task<int> GetAllEmployessByEmployeeLogCount(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId);
        Task<List<EmployeeLog>> GetAllEmployessLogFirst(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId);
        Task<List<EmployeesDetailsDataModel>> GetEmployeeDetailsList(SysDataTablePager pager, string columnDirection, string ColumnName,int companyId);
        Task<List<EmployeesEntity>> GetAllReportingPersons(int companyId);
        Task<List<EmployeesReleaveingTypeEntity>> GetAllReleaveTypes(int companyId);
        Task<int> GetEmployeesListCount(SysDataTablePager pager, int companyId);
        Task<EmployeesEntity> GetEmployeeByIdView(int EmpId, int companyId);
        Task<int> GetEmployeeEmail(string officeEmail, int companyId);
        Task<ProfileInfoEntity> GetProfileByEmployeeId(int empId, int companyId);
        Task<AddressInfoEntity> GetAddressByEmployeeId(int empId, int companyId);
        Task<List<EmployeeAppliedLeaveEntity>> GetAllEmployeeLeaveIsDeleted(int companyId);
        Task<List<ProfileInfoEntity>> GetAllEmployeeProfileByJoiningDateIsDeleted(DateTime currentdate, int companyId);
        Task<List<ProfileInfoEntity>> GetAllEmployeeProfileIsDeleted(int companyId);
        Task<List<ProfileInfoEntity>> GetAllEmployeeProfile(int companyId);
        Task<ProfileInfoEntity> GetProfileByEmployeeIdview(int empId, int companyId);
        Task<List<DesignationEntity>> GetAllDesignation(int companyId);
        Task<List<DepartmentEntity>> GetAllDepartment(int companyId);
        Task<List<RoleEntity>> GetAllRoleTable(int companyId);
        Task<List<QualificationEntity>> GetAllQulificationView(int empId, int companyId);
        Task<List<QualificationEntity>> GetAllQulificationViewModel(int empId, int companyId);
        Task<List<EmployeesEntity>> GetDepartmentById(List<int> DepartmentId, int companyId);
        Task<List<EmployeesEntity>> GetAllEmpById(int empId, int companyId);
        Task<List<EmployeesEntity>> GetAllEmployees(int companyId);
    }
}

