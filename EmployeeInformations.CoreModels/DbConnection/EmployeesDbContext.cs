using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using Microsoft.EntityFrameworkCore;


namespace EmployeeInformations.CoreModels.DbConnection
{
    public class EmployeesDbContext : DbContext
    {
        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
        {

        }
        public DbSet<EmployeesEntity> Employees { get; set; }
        public DbSet<AddressInfoEntity> AddressInfo { get; set; }
        public DbSet<ExperienceEntity> Experience { get; set; }
        public DbSet<OtherDetailsEntity> OtherDetails { get; set; }
        public DbSet<ProfileInfoEntity> ProfileInfo { get; set; }
        public DbSet<QualificationEntity> Qualification { get; set; }
        public DbSet<CountryEntity> CountryEntities { get; set; }
        public DbSet<StateEntity> state { get; set; }
        public DbSet<SkillSetEntity> SkillSets { get; set; }
        public DbSet<CityEntity> cities { get; set; }
        public DbSet<LeaveTypesEntity> leaveTypes { get; set; }
        public DbSet<EmployeeAppliedLeaveEntity> employeeAppliedLeaveEntities { get; set; }
        public DbSet<BankDetailsEntity> BankDetails { get; set; }
        public DbSet<AllLeaveDetailsEntity> AllLeaveDetails { get; set; }
        public DbSet<ReportingPersonsEntity> ReportingPersonsEntities { get; set; }
        public DbSet<ClientEntity> Client { get; set; }
        public DbSet<ProjectDetailsEntity> ProjectDetails { get; set; }
        public DbSet<ProjectTypesEntity> ProjectTypes { get; set; }
        public DbSet<TimeSheetEntity> TimeSheet { get; set; }
        public DbSet<RolePrivilegesEntity> RolePrivileges { get; set; }
        public DbSet<DashboardRolePrivilegesEntity> DashboardRolePrivilegesEntitys { get; set; }
        public DbSet<DashboardMenusEntity> DashboardMenusEntitys { get; set; }
        public DbSet<RoleEntity> RoleEntities { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<DesignationEntity> Designations { get; set; }
        public DbSet<DocumentTypesEntity> documentTypesEntities { get; set; }
        public DbSet<ModulesEntity> Modules { get; set; }
        public DbSet<SubModulesEntity> SubModulesEntitys { get; set; }
        public DbSet<EmployeeHolidaysEntity> EmployeeHolidaysEntities { get; set; }
        public DbSet<BloodGroupEntity> BloodGroup { get; set; }
        public DbSet<EmailSettingsEntity> EmailSettings { get; set; }
        public DbSet<SendEmailsEntity> SendEmails { get; set; }
        public DbSet<QualificationAttachmentsEntity> QualificationAttachmentEntitys { get; set; }
        public DbSet<ExperienceAttachmentsEntity> ExperienceAttachmentsEntitys { get; set; }
        public DbSet<OtherDetailsAttachmentsEntity> OtherDetailsAttachmentsEntitys { get; set; }
        public DbSet<AssetCategoryEntity> AssetCategory { get; set; }
        public DbSet<AssetTypesEntity> AssetTypes { get; set; }
        public DbSet<AllAssetsEntity> AllAssets { get; set; }
        public DbSet<AssetStatusEntity> AssetStatus { get; set; }
        public DbSet<AssetLogEntity> AssetLog { get; set; }
        public DbSet<TimeLoggerEntity> TimeLoggerEntitys { get; set; }
        public DbSet<CompanyEntity> Company { get; set; }
        public DbSet<CompanySettingEntity> CompanySetting { get; set; }
        public DbSet<EmployeesReleaveingTypeEntity> EmployeesReleaveingTypeEntities { get; set; }
        public DbSet<RelationshipTypeEntity> Relationship { get; set; }
        public DbSet<BenefitTypesEntity> BenefitTypesEntitys { get; set; }
        public DbSet<EmployeeBenefitEntity> EmployeeBenefitEntitys { get; set; }
        public DbSet<EmployeeMedicalBenefitEntity> EmployeeMedicalBenefitEntitys { get; set; }
        public DbSet<ExpensesEntity> ExpensesEntitys { get; set; }
        public DbSet<ExpenseDetailsEntity> ExpenseDetailsEntitys { get; set; }
        public DbSet<ExpenseAttachmentsEntity> ExpenseAttachmentsEntitys { get; set; }
        public DbSet<ProjectAssignationEntity> ProjectAssignation { get; set; }
        public DbSet<EmailQueueEntity> EmailQueueEntitys { get; set; }
        public DbSet<EmailDraftTypeEntity> EmailDraftType { get; set; }
        public DbSet<EmailDraftContentEntity> EmailDraftContent { get; set; }
        public DbSet<CompanyPolicyEntity> CompanyPolicyEntitys { get; set; }
        public DbSet<PolicyAttachmentsEntity> PolicyAttachmentsEntitys { get; set; }
        public DbSet<ManualLogEntity> ManualLogEntitys { get; set; }
        public DbSet<AnnouncementEntity> Announcement { get; set; }
        public DbSet<EmployeesLogEntity> EmployeesLog { get; set; }
        public DbSet<ApplicationLogEntity> ApplicationLog { get; set; }
        public DbSet<AttendanceEntitys> AttendanceEntity { get; set; }
        public DbSet<CompensatoryRequestsEntity> CompensatoryRequestsEntity { get; set; }
        public DbSet<AssetBrandTypeEntity> AssetBrandType { get; set; }
        public DbSet<BranchLocationEntity> BranchLocation { get; set; }
        public DbSet<EmployeeActivityLogEntity> EmployeeActivityLogEntitys { get; set; }
        public DbSet<RelievingReasonEntity> RelievingReasonEntity { get; set; }
        public DbSet<MailSchedulerEntity> MailSchedulerEntity { get; set; }
        public DbSet<EmployeeSettingEntity> EmployeeSettingsEntity { get; set; }
        public DbSet<CurrencyEntity> CurrencyEntity { get; set; }
        public DbSet<TicketTypesEntity> TicketTypesEntity { get; set; }
        public DbSet<HelpdeskEntity> HelpdeskEntity { get; set; }
        public DbSet<HelpDeskFilterEntity> HelpDeskFilterEntity { get; set; }
        public DbSet<HelpDeskCount> HelpDeskCount { get; set; }
        public DbSet<TicketAttachmentsEntity> TicketAttachmentsEntity { get; set; }
        public DbSet<AnnouncementAttachmentsEntity> announcementAttachmentsEntities { get; set; }
        public DbSet<SalaryEntity> salaryEntities { get; set; }
        public DbSet<TeamsMeetingEntity> teamsMeetingEntities { get; set; }
        public DbSet<EmployeesPrivileges> EmployeePrivileges { get; set; }
        public DbSet<EmployeesPrivilegesViewModel> EmployeesPrivilegesViewModel { get; set; }
        public DbSet<EmployeePrivilegesCount> EmployeePrivilegesCount { get; set; }

        // API Entity 
        public DbSet<WebsiteContactUsEntity> WebsiteContactUsEntitys { get; set; }
        public DbSet<WebsiteJobApplyEntity> WebsiteJobApplyEntitys { get; set; }
        public DbSet<WebsiteJobsEntity> WebsiteJobsEntitys { get; set; }
        public DbSet<WebsiteLeadTypeEntity> WebsiteLeadTypeEntitys { get; set; }
        public DbSet<WebsiteServicesEntity> WebsiteServicesEntitys { get; set; }
        public DbSet<WebsiteQuoteEntity> WebsiteQuotesEntitys { get; set; }
        public DbSet<WebsiteSurveyQuestionEntity> WebsiteSurveyQuestionEntitys { get; set; }
        public DbSet<WebsiteSurveyQuestionOptionEntity> WebsiteSurveyQuestionOptionEntitys { get; set; }
        public DbSet<WebsiteSurveyAnswerEntity> WebsiteSurveyAnswerEntitys { get; set; }
        public DbSet<WebsiteProposalTypeEntity> WebsiteProposalTypeEntitys { get; set; }
        public DbSet<WebsiteCandidateMenuEntity> websiteCandidateMenuEntities { get; set; }
        public DbSet<WebsiteCandidateMenusModel> websiteCandidateMenuModel { get; set; }
        public DbSet<WebsiteCandidateMenuModelCount> websiteCandidateMenuModelCount { get; set; }
        public DbSet<WebSiteCandidateScheduleEntity> WebsiteCandidateScheduleEntities { get; set; }
        public DbSet<WebsiteCandidatePrivilegesEntitys> websiteCandidatePrivilegesEntitys { get; set; }

        //this is used for store procedure not having any table in the database
        public DbSet<LeaveReportDateModel> LeaveReportDateModel { get; set; }
        public DbSet<ManualLogReportDateModel> ManualLogReportDateModel { get; set; }
        public DbSet<TimeSheetDataModel> TimeSheetDataModel { get; set; }
        public DbSet<AttendanceReportDateModel> AttendanceReportDateModel { get; set; }
        public DbSet<EmployeesLogReportDataModel> EmployeesLogReportDataModel { get; set; }
        public DbSet<AssetLogReportDataModel> AssetLogReportDataModel { get; set; }
        public DbSet<EmployeeLeaveReportCount> EmployeeLeaveReportCounts { get; set; }
        public DbSet<EmployeeLeaveReportDataModel> EmployeeLeaveReportDataModel { get; set; }
        public DbSet<CompensatoryEmployeeCount> CompensatoryRequestsCountModel {  get; set; }
        public DbSet<EmployeeCompensatoryFilter> EmployeeCompensatoryFilterModel {  get; set; }


        //sending mail purpose only
        public DbSet<AttendanceListDataModel> AttendanceListDataModel { get; set; }
        public DbSet<TimeSheetDataModel> timeSheetDataModels { get; set; }
        public DbSet <TimeSheetModel> TimeSheetModels { get; set; }
        public DbSet<EmployeeLog> EmployeesLogReport { get; set; }
        public DbSet<EmployeesCount> employeesCounts { get; set; }
        public DbSet<EmployeesDataCount> EmployeesDataCounts { get; set; }
        public DbSet<EmployeesDataModel> EmployeesDataModels { get; set; }
        public DbSet<TimeSheetReportViewModel> TimeSheetReport { get; set; }
        public DbSet<TimeSheetFilterCount> TimeSheetReportCount { get; set; }
        public DbSet<EmployeeLeavesModel> employeeAppliedLeavesEntities { get; set; }
        public DbSet<WorkFromHomeFilterCount> workFromHomeFilterCount { get; set; }
        public DbSet<WorkFromHomeFilterViewmodel> workFromHomeFilter { get; set; }
        public DbSet<ClientFilterCount> clientFilterCount { get; set; }
        public DbSet<ClientFilterViewModel> clientFilterViewModel { get; set; }       
        public DbSet<AssetCount> AssetCounts { get; set; }
        public DbSet<AssetViewModels> AssetModels { get; set; }
        public DbSet<AssetsLogModel> AssetsLogModels { get; set; }
        public DbSet<EmployeesDetailsDataModel> EmployeesDetailsDataModels { get; set; }
        public DbSet<ExpensesDataModel> employeeExpensesEntities { get; set; }
        public DbSet<EmployeeActivityLogFilterCount> employeeActivityFilterCounts { get; set; }
        public DbSet<EmployeeActivityLogViewModel> employeeActivityLogViewModels { get; set; }
        public DbSet<EmailDraftEntites> EmailDraft { get; set; }
        public DbSet<EmailDraftCount> EmailDraftCount { get; set; }        
        public DbSet<BenefitFilterCount> benefitFilterCount { get; set; }
        public DbSet<BenefitFilterViewModel> benefitFilterViewModel { get; set; }
        public DbSet<MedicalBenefitFilterCount> medicalBenefitFilterCount { get; set; }
        public DbSet<MedicalBenefitFilterViewModel> medicalBenefitFilterViewModel { get; set; }
        public DbSet<CompanyCounts> CompnayCounts { get; set; }
        public DbSet<CompanyViewModels> companyViewModels { get; set; }
        public DbSet<ProjectDetailsModel> projectDetailsEntities { get; set; }
        public DbSet<WebsiteJobPostEntity> WebsiteJobspost { get; set; }
        public DbSet<WebsiteJobPostCount> WebsiteJobspostcount { get; set; }
        public DbSet<WebsiteCandidatesMenuModel> websiteCandidateMenu { get; set; }
        public DbSet<AnnouncementFilterViewModel> announcementFilter { get; set; }
        public DbSet<AnnouncementFilterCount> announcementFilterCount { get; set; }
        public DbSet<EmailSchedulerFilterCount> emailSchedulerFilterCounts { get; set; }
        public DbSet<EmailSchedulerViewModel> emailSchedulerViewModels { get; set; }
        public DbSet<WebsiteJobApplyViewModel> websideJobApplyViewModels { get; set; }
        public DbSet<WebsideJobApplyCount> websideJobApplyCounts { get; set; }
        public DbSet<TeamMeetingModel> TeamMeetingModels { get; set; }
        public DbSet<TeamMeetingCount> teamMeetingCounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: set default schema globally for all tables
            modelBuilder.HasDefaultSchema("public");

            // Explicit mapping for EmployeesEntity in case PostgreSQL needs it
            //modelBuilder.Entity<EmployeesEntity>().ToTable("employees", "public");
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp without time zone");
                    }
                }
            }
        }

    }
}

