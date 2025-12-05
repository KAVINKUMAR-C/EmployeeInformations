using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.OnboardingViewModel;


namespace EmployeeInformations.Data.IRepository
{
     public interface IOBEmployeesRepository
    {
        Task<List<EmployeesEntity>> GetAllEmployees(int companyId);
        Task<List<DesignationEntity>> GetAllDesignation(int companyId);
        Task<List<DepartmentEntity>> GetAllDepartment(int companyId);
        Task<List<ProfileInfoEntity>> GetAllEmployeeProfile(int companyId);
        Task<ProfileInfoEntity> GetProfileByEmployeeId(int empId,int companyId);
        Task<AddressInfoEntity> GetAddressByEmployeeId(int empId,int companyId);
        Task<OtherDetailsEntity> GetOtherDetailsByEmployeeId(int empId,int companyId);
        Task<QualificationEntity> GetQualificationByEmployeeId(int empId, int companyId);
        Task<ExperienceEntity> GetExperienceByEmployeeId(int empId,int companyId);
        Task<BankDetailsEntity> GetBankDetailsByEmployeeId(int empId, int companyId);
        Task<List<StateEntity>> GetAllStates();
        Task<List<CityEntity>> GetAllCities();
        Task<Int32> CreateEmployee(EmployeesEntity employeesEntity,int companyId);
        Task CreateReportingPersons(List<ReportingPersonsEntity> reportingPersonsEntitys, int EmpId);
        Task<List<int>> GetReportingPersonEmployeeById(int EmpId,int companyId);
        Task<List<EmployeesEntity>> GetAllReportingPersons(int companyId);
        Task<List<RoleEntity>> GetAllRoleTable(int companyId);
        Task<int> GetEmployeeEmail(string officeEmail, int companyId);
        Task<int> GetPersonalEmail(string personalEmail);
        Task<EmployeesEntity> GetEmployeeById(int EmpId,int companyId);
        Task<List<SkillSetEntity>> GetAllSkills(int companyId);
        Task<List<BloodGroupEntity>> GetAllBloodGroup();
        Task<List<RelationshipTypeEntity>> GetAllRelationshipType();
        Task<int> AddProfileInfo(ProfileInfoEntity profileInfoEntity, bool isAdd = false);
        Task<List<CountryEntity>> GetAllCountry();
        Task<int> AddAddressInfo(AddressInfoEntity addressInfoEntity);
        Task<List<StateEntity>> GetByCountryId(int countryId);
        Task<List<CityEntity>> GetByStateId(int StateId);
        Task<List<OtherDetailsEntity>> GetAllOtherDetailsViewModel(int empId, int companyId);
        Task<List<DocumentTypesEntity>> GetAllDocumentTypes(int companyId);
        Task<int> InsertAndUpdateOtherDetails(OtherDetailsEntity otherDetailsEntity);
        Task<bool> InsertOtherDetailsAttachment(List<OtherDetailsAttachmentsEntity> otherDetailsAttachments, int detailId);
        Task<OtherDetailsEntity> GetotherDetailsBydetailId(int detailId, int CompanyId);
        Task<OtherDetailsAttachmentsEntity> GetOtherDetailsAttachmentsByEmployeeId(int detailId);
        Task<List<OtherDetailsAttachmentsEntity>> GetAllOtherDetailsAttachment(int detailId);
        Task DeleteOtherDetails(OtherDetailsEntity otherDetailsEntity);
        Task<List<OtherDetailsAttachmentsEntity>> GetOtherDetailsDocumentAndFilePath(int detailId);
        Task DeleteOtherDetailsAttachement(List<OtherDetailsAttachmentsEntity> otherDetailsAttachmentsEntity);
        Task<List<string>> GetDocumentNameByDetailId(int detailId);
        Task<List<QualificationEntity>> GetAllQulificationViewModel(int empId,int companyId);
        Task<bool> InsertQualificationAttachment(List<QualificationAttachmentsEntity> qualificationAttachments, int qualificationId);
         Task<Int32> InsertAndUpdateQualification(QualificationEntity qualificationEntity);
        Task<QualificationEntity> GetQualificationByQualificationId(int QualificationId,int companyId);
        Task<QualificationAttachmentsEntity> GetQualificationAttachmentsByEmployeeId(int qualificationId);
        Task<List<QualificationAttachmentsEntity>> GetAllQualificationAttachments(int qualificationId);
         Task<List<QualificationEntity>> GetAllQualification(int EmpId, int companyId);
        Task<List<string>> GetDocumentNameByQualificationId(int qualificationId);
        Task<List<QualificationAttachmentsEntity>> GetQualificationDocumentAndFilePath(int qualificationId);
        Task DeleteQualification(QualificationEntity qualificationEntity);
        Task DeleteQualificationAttachement(List<QualificationAttachmentsEntity> qualificationAttachmentsEntity);
        Task<List<ExperienceEntity>> GetAllExperienceViewModel(int empId, int companyId);
        Task<int> InsertAndUpdateExperience(ExperienceEntity experienceEntity);
        Task<bool> InsertExperienceAttachment(List<ExperienceAttachmentsEntity> experienceAttachments, int experienceId);
        Task<ExperienceEntity> GetExperienceByExperienceId(int ExperienceId,int companyId);
        Task<ExperienceAttachmentsEntity> GetExperienceAttachmentsByEmployeeId(int experienceId);
        Task<List<ExperienceAttachmentsEntity>> GetAllExperienceAttachment(int experienceId);
        Task<List<ExperienceEntity>> GetAllExperience(int EmpId,int companyId);
        Task<List<string>> GetDocumentNameByExperienceId(int experienceId);
        Task<List<ExperienceAttachmentsEntity>> GetExperienceDocumentAndFilePath(int experienceId);

        Task DeleteExperience(ExperienceEntity experienceEntity);
        Task DeleteExperienceAttachement(List<ExperienceAttachmentsEntity> experienceAttachmentsEntity);
        Task<int> AddBankDetails(BankDetailsEntity bankDetailsEntity);
        Task<EmployeesEntity> GetEmployeeByIdView(int EmpId,int companyID);
        Task<bool> GetRejectEmployees(OBEmployees employees,int companyId);
        Task<ProfileInfoEntity> GetProfileByEmployeeIdview(int empId,int companyId);
        Task<string> GetCountryNameByCountryId(int countryId);
        Task<string> GetStateNameByStateId(int stateId);
        Task<string> GetCityNameByCityId(int cityId);
        Task<string> GetCountryNameBySecondaryCountryId(int? SecondaryCountryId);
        Task<string> GetStateNameBySecondaryStateId(int? SecondaryStateId);
        Task<string> GetCityNameBySecondaryCityId(int? SecondaryCityId);
        Task<List<OtherDetailsEntity>> GetAllOtherDetailsView(int empId,int companyId);
        Task<List<QualificationEntity>> GetAllQulificationView(int empId,int companyId);
        Task<List<ExperienceEntity>> GetAllExperienceView(int empId,int companyId);
        Task<int> UpdateStatus(EmployeesEntity employeesEntity);
    }
}
