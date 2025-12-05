using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.API.IRepository
{
    public interface IWebsiteRepository
    {
        Task<bool> InsertContactUsReauest(WebsiteContactUsEntity contactUsEntity);
        Task<WebsiteJobApplyEntity> InsertJobPostReauest(JobPostRequestModel model);
        Task<WebsiteJobsEntity> GetJobPostByJobId(int JobId);
        Task<WebsiteJobApplyEntity> GetJobPostApplyByJobId(int JobId);
        Task<List<JobPostResponseModel>> GetAllJobPost();
        Task<List<WebsiteLeadTypeResponseModel>> GetWebsiteLeads();
        Task<List<WebsiteServicesResponseModel>> GetWebsiteServices();
        Task<int> InsertQuoteRequest(WebsiteQuoteEntity websiteQuoteEntity);
        Task<string> GetWebsiteServiceType(int serviceId);
        Task<string> GetMaxServiceCount();
        Task<List<WebsiteSurveyQuestionEntity>> GetWebsiteSurveyQuestions();
        Task<List<WebsiteSurveyQuestionOptionEntity>> GetWebsiteSurveyQuestionOptions();
        Task<bool> InsertSoftwareConsultSurveyRequest(List<WebsiteSurveyAnswerEntity> websiteSurveyAnswerEntity);

        /// <summary>
        /// Development
        /// </summary>
        /// <returns></returns>
        Task<List<WebsiteJobsEntity>> GetAllWebsiteJobPost();
        Task<int> GetWebsiteJobPostMaxCount();
        Task<bool> InsertWebsiteJobPost(WebsiteJobsEntity websiteJobsEntity);
        Task<WebsiteJobsEntity> GetWebsiteJobPostByJobId(int jobId);
        void DeleteWebsiteJobPost(int jobId);
        void ChangeStatusWebsiteJobPost(int jobId, int status);
        Task<int> WebsiteJobPostCount(SysDataTablePager pager);
        Task<List<WebsiteJobPostEntity>> WebsiteJobPostFilter(SysDataTablePager pager, string columnName, string columnDirection);


        Task<List<WebsiteJobApplyEntity>> GetAllJobApply();
        Task<bool> CreateSkills(SkillSetEntity skillSetEntity);
        Task<int> GetSkill(string SkillName);
        Task<List<WebsiteQuoteEntity>> GetAllWebsiteQuote();
        Task<List<WebsiteProposalTypeEntity>> GetAllWebsiteProposal();
        Task<List<WebsiteServicesEntity>> GetAllWebsiteServices();
        Task<List<WebsiteSurveyAnswerEntity>> GetAllWebsiteSurveyAnswer();
        Task<List<WebsiteSurveyQuestionEntity>> GetAllSurveyQuestion();
        Task<List<WebsiteSurveyQuestionOptionEntity>> GetAllSurveyQuestionOption();
        Task<WebsiteJobApplyEntity> GetWebsiteJobApplyId(int jobApplyId);      
        Task<bool> AddSiteJopApply(WebsiteJobApplyEntity websiteJobApplyEntity);
        Task<int> WebSiteApplyJobIsActive(int jobApplyId, bool isactive);
        Task<int> AddCandidateMenu(WebsiteCandidateMenuEntity websiteCandidateMenuEntity);        
        Task<bool> UpdateWebsiteAppliedJob(WebsiteJobApplyEntity websiteJobApplyEntity);        
        Task<List<WebsiteCandidateMenu>> GetWebsiteCandidateMenus();

        //CandidateSechudule
        Task<List<WebSiteCandidateScheduleEntity>> CandidateSechudule(int companyId);

        Task WebsiteUpdateCandidateStatus(WebsiteCandidateMenuEntity websiteCandidateMenuEntity);
        Task<List<WebsiteCandidatePrivilegesEntitys>> CandidatePrivileges(bool isEnabled,int companyId);
        Task<List<WebSiteCandidateScheduleEntity>> GetCandidateSechudule(int companyId);

        //CandidatePrivileges
        Task<List<WebsiteCandidatePrivilegesEntitys>> GetCandidatePrivileges(int companyId);
        Task<List<CandidatePrivileges>> GetCandidatePrivilege(int companyId);
        Task<List<WebsiteCandidatePrivilegesEntitys>> GetCandidatePrivilegeByRoleId(int candidatescheduleId, string delete,int companyId);
        Task<bool> AddCandidatePrivilegeByRoleId(List<WebsiteCandidatePrivilegesEntitys> websiteCandidatePrivilegesEntitys,int companyId);
        Task<int> CreateWebsiteCandidateSechudule(WebSiteCandidateScheduleEntity webSiteCandidateSechudule, int companyId);
        Task<int> UpdateWebsiteCandidateSechudule(WebSiteCandidateScheduleEntity webSiteCandidateSechudule);
        Task<int> DeleteWebsiteSchedule(int candidateId);       
        Task<bool> UpdateWebsiteCandidateMenu(WebsiteCandidateMenuEntity websiteCandidateMenu);
        Task<List<WebSiteCandidateScheduleEntity>> GetCandidateById(int candidateId, int companyId);
        Task<WebsiteCandidateMenuModel> GetCandidateMenuByIds(int candidateMenuId);
        Task<int> GetCandidateName(string scheduleName,int companyId);
        Task<List<EmployeesEntity>> GetAllEmployees(int companyId);
        Task<bool> UpdateWebsiteCandidateMenuMeetinglink(WebsiteCandidateMenuEntity websiteCandidateMenu);
        Task<List<WebsiteCandidatesMenuModel>> GetWebsiteCandidateMenu(SysDataTablePager pager, string columnName, string columnDirection, int companyId);
        Task<int> GetWebsiteCandidateMenuCount(SysDataTablePager pager,int companyId);
        Task<List<WebsiteCandidateMenusModel>> GetWebsiteCandidateMenusDetails(SysDataTablePager pager, string columnName, string columnDirection, int candidateScheduleId);
        Task<int> GetWebsiteCandidateMenusCount(SysDataTablePager pager, int candidateScheduleId);
        Task<List<WebsiteJobApplyViewModel>> GetWebsideJobApplyList(SysDataTablePager pager, string columnDirection, string columnName);
        Task<int> GetWebsideJobApplyListCount(SysDataTablePager pager);

    }
}
