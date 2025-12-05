using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Model.APIModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.APIController
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Website")]
    [ApiController]
    [EnableCors]
    public class WebsiteController : ControllerBase
    {
        private readonly IWebsiteService _websiteService;

        public WebsiteController(IWebsiteService websiteService)
        {
            _websiteService = websiteService;
        }

        [HttpPost("InsertContactUs")]
        public async Task<UserResponseModel> ContactUs(ContactUsRequestModel model)
        {
            var response = new UserResponseModel();
            if (model != null)
            {
                response = await _websiteService.InsertContactUsReauest(model);
            }
            return response;

        }

        [HttpGet("GetWebsiteJob/{JobId}")]
        public async Task<JobPostResponseModel> GetJobPost(int JobId)
        {
            var jobPostResponseModel = new JobPostResponseModel();
            if (JobId != null)
            {
                jobPostResponseModel = await _websiteService.GetJobPostByJobId(JobId);
            }

            return jobPostResponseModel;
        }


        [HttpGet("GetWebsiteAllJob")]
        public async Task<List<JobPostResponseModel>> GetAllJobPost()
        {
            var jobPostResponseModel = new List<JobPostResponseModel>();
            jobPostResponseModel = await _websiteService.GetAllJobPost();
            return jobPostResponseModel;
        }


        [HttpPost("WebsiteJobApply")]
        public async Task<UserResponseModel> JobPost(JobPostRequestModel model)
        {
            var response = new UserResponseModel();
            if (model != null)
            {
                response = await _websiteService.InsertJobPostReauest(model);
            }
            return response;
        }

        [HttpGet("GetWebsiteLeads")]
        public async Task<List<WebsiteLeadTypeResponseModel>> WebsiteLeads()
        {
            var WebsiteLeadTypeResponseModel = new List<WebsiteLeadTypeResponseModel>();
            WebsiteLeadTypeResponseModel = await _websiteService.GetWebsiteLeads();
            return WebsiteLeadTypeResponseModel;
        }

        [HttpGet("GetWebsiteServices")]
        public async Task<List<WebsiteServicesResponseModel>> WebsiteServices()
        {
            var websiteServicesResponseModel = await _websiteService.GetWebsiteServices();
            return websiteServicesResponseModel;
        }


        [HttpPost("InsertQuote")]
        public async Task<UserResponseModel> InsertQuoteRequest(WebsiteQuoteRequestModel model)
        {
            var response = new UserResponseModel();
            if (model != null)
            {
                response = await _websiteService.InsertQuoteRequest(model);
            }
            return response;

        }

        [HttpGet("GetWebsiteSurveyQuestions")]
        public async Task<List<WebsiteSurveyResponseModel>> GetWebsiteSurveyQuestionsRequest()
        {
            var websiteServicesResponseModel = await _websiteService.GetWebsiteSurveyQuestionsRequest();
            return websiteServicesResponseModel;
        }

        [HttpPost("InsertSoftwareConsultProfile")]
        public async Task<UserResponseModel> InsertSoftwareConsultRequest(WebsiteSurveyRequestModel model)
        {
            var response = new UserResponseModel();
            if (model != null)
            {
                response = await _websiteService.InsertSoftwareConsultProfileRequest(model);
            }
            return response;

        }

        [HttpPost("InsertSoftwareConsultSurvey")]
        public async Task<UserResponseModel> InsertSoftwareConsultSurveyRequest(List<SurveyAnswerRequestModel> model)
        {
            var response = new UserResponseModel();
            if (model != null)
            {
                response = await _websiteService.InsertSoftwareConsultSurveyRequest(model);
            }
            return response;

        }
    }
}