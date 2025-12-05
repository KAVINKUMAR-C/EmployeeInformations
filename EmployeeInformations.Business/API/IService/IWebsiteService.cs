using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.API.IService
{
    public interface IWebsiteService
    {
        Task<UserResponseModel> InsertContactUsReauest(ContactUsRequestModel model);
        Task<UserResponseModel> InsertJobPostReauest(JobPostRequestModel model);
        Task<JobPostResponseModel> GetJobPostByJobId(int JobId);
        Task<List<JobPostResponseModel>> GetAllJobPost();
        Task<List<WebsiteLeadTypeResponseModel>> GetWebsiteLeads();
        Task<List<WebsiteServicesResponseModel>> GetWebsiteServices();
        Task<UserResponseModel> InsertQuoteRequest(WebsiteQuoteRequestModel websiteQuoteRequestModel);
        Task<List<WebsiteSurveyResponseModel>> GetWebsiteSurveyQuestionsRequest();
        Task<UserResponseModel> InsertSoftwareConsultProfileRequest(WebsiteSurveyRequestModel websiteQuoteRequestModel);
        Task<UserResponseModel> InsertSoftwareConsultSurveyRequest(List<SurveyAnswerRequestModel> websiteSurveyAnswerRequestModel);

        // Website Development
        Task<int> GetSkill(string SkillName);
        Task<List<WebsiteJobViewModel>> GetAllWebsiteJobPost();
        Task<string> GetWebsiteJobPostId();
        Task<bool> InsertWebsiteJobPost(WebsiteJobViewModel websiteJobViewModel);
        Task<WebsiteJobViewModel> GetWebsiteJobPostByJobId(int jobId);
        Task<bool> DeleteWebsiteJobPost(int jobId);
        Task<bool> CreateSkills(SkillSet skill);
        Task<bool> ChangeStatusWebsiteJobPost(int jobId, int status,int companyId);
        Task<List<WebsiteJobApplyModel>> GetAllJobApply();
        Task<List<WebsiteQuickQuoteViewModel>> GetAllWebsiteQuote();
        Task<List<WebsiteSoftwareConsultant>> GetAllWebsiteSurveyAnswer();
        Task<WebsiteJobApplyModel> GetWebsiteJobApplyId(int jobApplyId);
        Task<WebsiteJobApplyModel> CreateWebsiteJob(int companyId);
        Task<bool> AddSiteJopApply(WebsiteJobApplyModel websiteJobApply,int companyId);
        Task<int> WebSiteApplyJobIsActive(int jobId, bool isActive);
        Task<bool> UpdateWebsiteAppliedJob(WebsiteJobApplyModel websiteJobApplyModel);
        Task<List<WebsiteJobViewModel>> GetAllWebsiteJobPostName();
        Task<WebsiteCandidateMenuModel> GetWebsiteCandidateMenus(int companyId);
        Task<WebsiteCandidateScheduleViewModel> CandidateSchedule(int companyId);
        Task<int> WebsiteUpdateCandidateStatus(WebsiteCandidateMenuModel websiteCandidateMenuModel,int companyId);
        Task<CandidatePrivilegesModel> GetCandidatePrivileges(int companyId);
        Task<bool> AddCandidatePrivilegeByRoleId(List<CandidatePrivilegeAssign> candidatePrivileges,int companyId);
        Task<WebsiteCandidateSchedule> CreateCandidateSchedule(WebsiteCandidateSchedule websiteCandidate,int companyId);
        Task<int> UpdateCandidateSchedule(WebsiteCandidateSchedule websiteCandidate);
        Task<int> DeleteWebsiteSchedule(int candidateId);
        Task<bool> UpdateWebsiteCandidateMenu(WebsiteCandidateMenuModel websiteCandidateMenuModel);
        Task<WebsiteCandidateMenuModel> GetCandidateById(int candidateid,int companyId);
        Task<List<DropdownEmployees>> GetAllEmployees(int companyId);
        Task<WebsiteCandidateMenuModel> GetCandidateMenuById(int id);
        Task<bool> UpdateWebsiteCandidateMenuMeetingLink(WebsiteCandidateMenuModel websiteCandidateMenu,int companyId);
        Task<string> CreateZoomMeeting(string accessToken, ZoomMeetingSchedule meeting);
       Task<WebsiteCandidateMenuModel> WebsiteCandidateZoomMeetingSchedule(int id, string code,int companyId);
        Task<int> WebsiteJobPostCount(SysDataTablePager pager);
        Task<List<WebsiteJobPostEntity>> WebsiteJobPostFilter(SysDataTablePager pager, string columnName, string columnDirection);        
        Task<List<WebsiteCandidatesMenuModel>> GetWebsiteCandidateMenu(SysDataTablePager pager, string columnName, string columnDirection,int companyId);
        Task<List<WebsiteCandidateSchedule>> GetCandidateSechudule(int companyId);
        Task<int> GetWebsiteCandidateMenuCount(SysDataTablePager pager,int companyId);        
        Task<WebsiteCandidateMenuModel> GetWebsiteCandidateMenusDetails(SysDataTablePager pager, string columnName, string columnDirection);
        Task<WebsiteCandidateMenuModel> GetWebsiteCandidateRejectedDetails(SysDataTablePager pager, string columnName, string columnDirection);
        Task<int> GetWebsiteCandidateMenusCount(SysDataTablePager pager);
        Task<int> RejectedCandidateCount(SysDataTablePager pager);
        Task<WebsiteJobApplyModel> GetWebsideJobApplyList(SysDataTablePager pager, string columnDirection, string columnName);
        Task<int> GetWebsideJobApplyListCount(SysDataTablePager pager);
    }
}
