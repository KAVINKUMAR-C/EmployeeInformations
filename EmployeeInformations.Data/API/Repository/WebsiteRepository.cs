using AutoMapper;
using EmployeeInformations.CoreModels;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.API.IRepository;
using EmployeeInformations.Model.APIModel;
using Microsoft.EntityFrameworkCore;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.CoreModels.DataViewModel;
using Npgsql;

namespace EmployeeInformations.Data.API.Repository
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly EmployeesDbContext _dbContext;
        private readonly IMapper _mapper;
        public WebsiteRepository(EmployeesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> InsertContactUsReauest(WebsiteContactUsEntity contactUsEntity)
        {
            _dbContext.WebsiteContactUsEntitys.Add(contactUsEntity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<WebsiteJobApplyEntity> InsertJobPostReauest(JobPostRequestModel model)
        {
            var jobApplyEntity = new WebsiteJobApplyEntity();

            jobApplyEntity.JobId = model.JobId;
            jobApplyEntity.FullName = model.FullName;
            jobApplyEntity.Email = model.Email;
            jobApplyEntity.Experience = model.Experience;
            jobApplyEntity.SkillSet = model.SkillSet;
            jobApplyEntity.RelevantExperience = model.RelevantExperience;

            var combinePath = "";
            if (!string.IsNullOrEmpty(model.Base64string))
            {
                var fileFormat = model.FileFormat;
                var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/WebSiteJobApply");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                combinePath = Path.Combine(path, fileName);

                Byte[] bytes = Convert.FromBase64String(model.Base64string);
                SaveByteArrayToFileWithFileStream(bytes, combinePath);
            }

            jobApplyEntity.FilePath = combinePath;
            jobApplyEntity.ApplyDate = DateTime.Now;
            jobApplyEntity.IsActive = false;
            jobApplyEntity.IsRecordForm = "vphospital Site";
            _dbContext.WebsiteJobApplyEntitys.Add(jobApplyEntity);
            _dbContext.SaveChanges();
            return jobApplyEntity;
        }

        public async Task<WebsiteJobsEntity> GetJobPostByJobId(int JobId)
        {
            var jobPostEntityModel = await _dbContext.WebsiteJobsEntitys.FirstOrDefaultAsync(x => x.JobId == JobId);
            return jobPostEntityModel;
        }

        public async Task<WebsiteJobApplyEntity> GetJobPostApplyByJobId(int JobId)
        {
            var jobPostEntityModel = await _dbContext.WebsiteJobApplyEntitys.FirstOrDefaultAsync(x => x.JobId == JobId);
            return jobPostEntityModel;
        }

        public async Task<List<JobPostResponseModel>> GetAllJobPost()
        {
            var jobPostModel = new List<JobPostResponseModel>();
            var jobPostEntityModels = await _dbContext.WebsiteJobsEntitys.Where(x => x.IsDeleted == 2).ToListAsync();
            jobPostModel = _mapper.Map<List<JobPostResponseModel>>(jobPostEntityModels);
            jobPostModel = jobPostModel.OrderByDescending(x => x.JobPostedDate).ToList();
            if (jobPostEntityModels.Count() > 0)
            {
                return jobPostModel;
            }
            else
            {
                jobPostModel = new List<JobPostResponseModel>();
            }
            return jobPostModel;
        }

        public async Task<List<WebsiteLeadTypeResponseModel>> GetWebsiteLeads()
        {
            var websiteLeadTypeEntitysModels = await _dbContext.WebsiteLeadTypeEntitys.ToListAsync();
            var websiteLeadTypeResponseModels = _mapper.Map<List<WebsiteLeadTypeResponseModel>>(websiteLeadTypeEntitysModels);
            return websiteLeadTypeResponseModels;
        }

        public async Task<List<WebsiteServicesResponseModel>> GetWebsiteServices()
        {
            var websiteServicesEntitysModels = await _dbContext.WebsiteServicesEntitys.ToListAsync();
            var websiteServicesResponseModel = _mapper.Map<List<WebsiteServicesResponseModel>>(websiteServicesEntitysModels);
            return websiteServicesResponseModel;
        }

        public async Task<List<WebsiteSurveyQuestionEntity>> GetWebsiteSurveyQuestions()
        {
            return await _dbContext.WebsiteSurveyQuestionEntitys.Where(x => x.IsActive).ToListAsync();
        }
        public async Task<List<WebsiteSurveyQuestionOptionEntity>> GetWebsiteSurveyQuestionOptions()
        {
            return await _dbContext.WebsiteSurveyQuestionOptionEntitys.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<int> InsertQuoteRequest(WebsiteQuoteEntity websiteQuoteEntity)
        {
            _dbContext.WebsiteQuotesEntitys.Add(websiteQuoteEntity);
            _dbContext.SaveChanges();

            var quoteId = websiteQuoteEntity.QuoteId;
            return quoteId;
        }

        public async Task<string> GetMaxServiceCount()
        {
            var maxServiceCount = await _dbContext.WebsiteQuotesEntitys.CountAsync();
            var eRFNumber = "ERFN-" + (maxServiceCount + 1).ToString("D3");
            return eRFNumber;
        }

        public async Task<string> GetWebsiteServiceType(int serviceId)
        {
            var websiteServicesEntitys = await _dbContext.WebsiteServicesEntitys.FirstOrDefaultAsync(x => x.ServicesId == serviceId);
            if (websiteServicesEntitys != null)
            {
                return websiteServicesEntitys?.ServiceName;
            }
            return string.Empty;
        }

        public async Task<bool> InsertSoftwareConsultSurveyRequest(List<WebsiteSurveyAnswerEntity> websiteWebsiteSurveyAnswerEntity)
        {
            _dbContext.WebsiteSurveyAnswerEntitys.AddRange(websiteWebsiteSurveyAnswerEntity);
            _dbContext.SaveChanges();
            return true;
        }
        public static void SaveByteArrayToFileWithFileStream(byte[] data, string filePath)
        {
            using var stream = File.Create(filePath);
            stream.Write(data, 0, data.Length);
        }
        /// <summary>
        ///  Logic to get all WebsiteJobPost count by SP
        /// </summary>
        /// <param name="pager" ></param>  
        public async Task<int> WebsiteJobPostCount(SysDataTablePager pager)
        {
            try
            {
                var result = 0;

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }

                var param = new NpgsqlParameter("searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);

                List<WebsiteJobPostCount> websiteJobPostFilterCount =
                    await _dbContext.WebsiteJobspostcount
                        .FromSqlRaw("SELECT * FROM sp_get_website_job_postcount(@searchText)", param)
                        .ToListAsync();

                foreach (var item in websiteJobPostFilterCount)
                {
                    result = item.Id;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Logic to get WebsiteJobPost Filtered data of the employees
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>
        public async Task<List<WebsiteJobPostEntity>> WebsiteJobPostFilter(SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                var result = 0;
             
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param1 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", _params.Sorting);

                var data = await _dbContext.WebsiteJobspost.FromSqlRaw("EXEC [dbo].[spGetWebsiteJobPostFilter] @pagingSize,@offsetValue,@searchText,@sorting", param1, param2, param3, param4).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public async Task<List<WebsiteJobsEntity>> GetAllWebsiteJobPost()
        {
            return await _dbContext.WebsiteJobsEntitys.Where(x => x.IsDeleted != 1).ToListAsync();
        }

        public async Task<int> GetWebsiteJobPostMaxCount()
        {
            var count = await _dbContext.WebsiteJobsEntitys.CountAsync();
            return count;
        }


        public async Task<WebsiteJobsEntity> GetWebsiteJobPostByJobId(int jobId)
        {
            return await _dbContext.WebsiteJobsEntitys.AsNoTracking().FirstOrDefaultAsync(x => x.JobId == jobId && x.IsDeleted != 1);
        }

        public async Task<bool> InsertWebsiteJobPost(WebsiteJobsEntity websiteJobsEntity)
        {
            var result = false;
            var jobStatus = await GetWebsiteJobPostByJobId(websiteJobsEntity.JobId);
            try
            {
                websiteJobsEntity.JobPostedDate = DateTime.Now;
                if (websiteJobsEntity?.JobId == 0)
                {
                    await _dbContext.WebsiteJobsEntitys.AddAsync(websiteJobsEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                else
                {
                    if (websiteJobsEntity != null)
                    {
                        websiteJobsEntity.UpdatedDate = DateTime.Now;
                        websiteJobsEntity.IsDeleted = jobStatus.IsDeleted;
                        _dbContext.WebsiteJobsEntitys.Update(websiteJobsEntity);
                        result = await _dbContext.SaveChangesAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        public void DeleteWebsiteJobPost(int jobId)
        {
            var websiteJobsEntitys = _dbContext.WebsiteJobsEntitys.FirstOrDefault(e => e.JobId == jobId);
            try
            {
                if (websiteJobsEntitys != null)
                {
                    websiteJobsEntitys.IsDeleted = 1;
                    _dbContext.WebsiteJobsEntitys.Update(websiteJobsEntitys);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        public void ChangeStatusWebsiteJobPost(int jobId, int status)
        {
            var websiteJobsEntitys = _dbContext.WebsiteJobsEntitys.FirstOrDefault(e => e.JobId == jobId);
            try
            {
                if (websiteJobsEntitys != null)
                {
                    websiteJobsEntitys.IsDeleted = status;
                    websiteJobsEntitys.JobPostedDate = DateTime.Now;
                    _dbContext.WebsiteJobsEntitys.Update(websiteJobsEntitys);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<List<WebsiteJobApplyEntity>> GetAllJobApply()
        {
            try
            {
                var websiteJobApplyEntitys = await _dbContext.WebsiteJobApplyEntitys.ToListAsync();
                return websiteJobApplyEntitys;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> CreateSkills(SkillSetEntity skillSetEntity)
        {
            var result = false;
            try
            {

                if (skillSetEntity != null)
                {
                    await _dbContext.SkillSets.AddAsync(skillSetEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                    result = true;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }
        public async Task<int> GetSkill(string SkillName)
        {
            var skillCount = await _dbContext.SkillSets.Where(x => x.SkillName == SkillName).CountAsync();
            return skillCount;
        }

        public async Task<List<WebsiteQuoteEntity>> GetAllWebsiteQuote()
        {
            return await _dbContext.WebsiteQuotesEntitys.ToListAsync();
        }

        public async Task<List<WebsiteProposalTypeEntity>> GetAllWebsiteProposal()
        {
            return await _dbContext.WebsiteProposalTypeEntitys.ToListAsync();
        }

        public async Task<List<WebsiteServicesEntity>> GetAllWebsiteServices()
        {
            return await _dbContext.WebsiteServicesEntitys.Where(t => !t.IsDeleted).ToListAsync();
        }

        public async Task<List<WebsiteSurveyAnswerEntity>> GetAllWebsiteSurveyAnswer()
        {
            return await _dbContext.WebsiteSurveyAnswerEntitys.ToListAsync();
        }

        public async Task<List<WebsiteSurveyQuestionEntity>> GetAllSurveyQuestion()
        {
            return await _dbContext.WebsiteSurveyQuestionEntitys.ToListAsync();
        }

        public async Task<List<WebsiteSurveyQuestionOptionEntity>> GetAllSurveyQuestionOption()
        {
            return await _dbContext.WebsiteSurveyQuestionOptionEntitys.ToListAsync();
        }

        public async Task<WebsiteJobApplyEntity> GetWebsiteJobApplyId(int jobApplyId)
        {
            return await _dbContext.WebsiteJobApplyEntitys.FirstOrDefaultAsync(x => x.JobApplyId == jobApplyId);
        }

        public async Task<bool> AddSiteJopApply(WebsiteJobApplyEntity websiteJobApplyEntity)
        {
            var result = false;
            try
            {

                if (websiteJobApplyEntity != null)
                {
                    await _dbContext.WebsiteJobApplyEntitys.AddAsync(websiteJobApplyEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                    result = true;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }
        public async Task<int> WebSiteApplyJobIsActive(int jobApplyId, bool isactive)
        {
            var websiteJobsEntitys = _dbContext.WebsiteJobApplyEntitys.FirstOrDefault(e => e.JobApplyId == jobApplyId);
            websiteJobsEntitys.IsActive = isactive;
            _dbContext.WebsiteJobApplyEntitys.Update(websiteJobsEntitys);
            _dbContext.SaveChanges();
            return jobApplyId;

        }
        public async Task<int> AddCandidateMenu(WebsiteCandidateMenuEntity websiteCandidateMenuEntity)
        {
            var result = 0;
            if (websiteCandidateMenuEntity != null)
            {
                await _dbContext.websiteCandidateMenuEntities.AddAsync(websiteCandidateMenuEntity);
                _dbContext.SaveChanges();
                result = websiteCandidateMenuEntity.JobApplyId;
            }

            return result;
        }

        public async Task<bool> UpdateWebsiteAppliedJob(WebsiteJobApplyEntity websiteJobApplyEntity)
        {
            var result = false;
            try
            {
                if (websiteJobApplyEntity != null)
                {
                    _dbContext.WebsiteJobApplyEntitys.Update(websiteJobApplyEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        

        public async Task<List<WebsiteCandidateMenu>> GetWebsiteCandidateMenus()
        {
            var candidateStatusId = 1;
            var websiteCandidateMenus = await (from candidateMenus in _dbContext.websiteCandidateMenuEntities
                                               join job in _dbContext.WebsiteJobsEntitys on candidateMenus.JobId equals job.JobId
                                               join candidateSchedule in _dbContext.WebsiteCandidateScheduleEntities on candidateMenus.CandidateScheduleId equals candidateSchedule.CandidateScheduleId
                                               into Schedule
                                               from branch in Schedule.DefaultIfEmpty()
                                               select new WebsiteCandidateMenu
                                               {
                                                   CandidateScheduleId = candidateMenus.CandidateScheduleId,
                                                   CandidateMenuId = candidateMenus.CandidateMenuId,
                                                   JobApplyId = candidateMenus.JobApplyId,
                                                   JobId = candidateMenus.JobId,
                                                   JobName = job.JobName,
                                                   FullName = candidateMenus.FullName,
                                                   FilePath = candidateMenus.FilePath,
                                                   ApplyDate = candidateMenus.ApplyDate,
                                                   Attachment = candidateMenus.Attachment,
                                                   Email = candidateMenus.Email,
                                                   Experience = candidateMenus.Experience,
                                                   RelevantExperience = candidateMenus.RelevantExperience,
                                                   SkillSet = candidateMenus.SkillSet,
                                                   IsRecordForm = candidateMenus.IsRecordForm,
                                                   CandidateScheduleName = branch.ScheduleName,
                                                   SortFullName = !string.IsNullOrEmpty(candidateMenus.FullName) ? candidateMenus.FullName.Substring(0, 1) : string.Empty,
                                                   CandidateStatusName = Convert.ToString((CandidateStatus)candidateMenus.CandidateStatusId),
                                                   CandidateStatusId = candidateMenus.CandidateStatusId,
                                                   File = string.IsNullOrEmpty(candidateMenus.FilePath) ? "" : candidateMenus.FilePath.Substring(candidateMenus.FilePath.LastIndexOf("/") + 1),
                                                   ScheduledDate = candidateMenus.ScheduledDate,
                                                   StartTime = candidateMenus.StartTime,
                                                   EndTime = candidateMenus.EndTime,
                                                   StrScheduledDate = candidateMenus.ScheduledDate.ToString(),
                                                   EmployeeName = candidateMenus.EmployeeName,
                                                   MeetingLink = candidateMenus.MeetingLink
                                               }).ToListAsync();
            return websiteCandidateMenus;
        }
        /// <summary>
        /// Logic to get WebsiteCandidateMenu details
        /// </summary>
        /// <param name="pager,columnName,columnDirection,candidateScheduleId"></param>
        public async Task<List<WebsiteCandidateMenusModel>> GetWebsiteCandidateMenusDetails(SysDataTablePager pager, string columnName, string columnDirection, int candidateScheduleId)
        {

            var result = new List<WebsiteCandidateMenusModel>();
            var _params = new
            {
                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
                Sorting = columnName + " " + columnDirection,
            };


            var param1 = new NpgsqlParameter("@candidateScheduleId", candidateScheduleId);
            var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var param3 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
            var param4 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
            var param5 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);


            try
            {
                result = await _dbContext.websiteCandidateMenuModel.FromSqlRaw("EXEC [dbo].[spGetWebsiteCandidateMenus] @candidateScheduleId,@searchText,@pagingSize,@offsetValue,@Sorting", param1, param2, param3, param4, param5).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

            /// <summary>
            /// Logic to get all the WebsiteCandidateMenu details 
            /// </summary>
            /// <param name="pager" >WebsiteCandidateMenu</param> 
        public async Task<List<WebsiteCandidatesMenuModel>> GetWebsiteCandidateMenu(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {
            try
            {             
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = pager.sEcho,
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param3 = new NpgsqlParameter("@offsetValue", pager.sEcho);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var webCandidateMenu = await _dbContext.websiteCandidateMenu.FromSqlRaw("EXEC [dbo].[spGetWebsiteCandidateMenuFilterList] @companyId, @pagingSize, @offsetValue, @searchText, @sorting", param1, param2, param3, param4, param5).ToListAsync();
                return webCandidateMenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Logic to get WebsiteCandidateMenu Count
        /// </summary>
        /// <param name="pager,columnName,columnDirection,candidateScheduleId"></param>
        public async Task<int> GetWebsiteCandidateMenusCount(SysDataTablePager pager, int candidateScheduleId)
        {
            var result = 0;
           
            var _params = new
            {               
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
            };

            var param1 = new NpgsqlParameter("@candidateScheduleId", candidateScheduleId);
            var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var CandidateMenus = await _dbContext.websiteCandidateMenuModelCount.FromSqlRaw("EXEC [dbo].[spGetWebsiteCandidateMenusCount] @candidateScheduleId,@searchText", param1, param2).ToListAsync();
            foreach (var item in CandidateMenus)
            {
                result = item.EmployeeCount;
            }
            return result;
        }

        public async Task<List<WebSiteCandidateScheduleEntity>> CandidateSechudule(int companyId)
        {
            var candidateEntity = await _dbContext.WebsiteCandidateScheduleEntities.Where(model => !model.IsDeleted == true && model.CompanyId == companyId).ToListAsync();
            return candidateEntity;
        }

        public async Task<int> CreateWebsiteCandidateSechudule(WebSiteCandidateScheduleEntity webSiteCandidateSechudule, int companyId)
        {
            var result = 0;
            if (webSiteCandidateSechudule != null)
            {
                webSiteCandidateSechudule.CompanyId = companyId;
                await _dbContext.WebsiteCandidateScheduleEntities.AddAsync(webSiteCandidateSechudule);
                await _dbContext.SaveChangesAsync();
                result = webSiteCandidateSechudule.CandidateScheduleId;
            }
            return result;
        }
        public async Task<int> UpdateWebsiteCandidateSechudule(WebSiteCandidateScheduleEntity webSiteCandidateSechudule)
        {
            var candidateEntity = await _dbContext.WebsiteCandidateScheduleEntities.Where(model => model.CandidateScheduleId == webSiteCandidateSechudule.CandidateScheduleId).FirstOrDefaultAsync();
            if (candidateEntity != null)
            {
                candidateEntity.ScheduleName = webSiteCandidateSechudule.ScheduleName;
                candidateEntity.IsActive = webSiteCandidateSechudule.IsActive;
                _dbContext.WebsiteCandidateScheduleEntities.Update(candidateEntity);
                await _dbContext.SaveChangesAsync();
            }
            return webSiteCandidateSechudule.CandidateScheduleId;
        }
        public async Task<int> DeleteWebsiteSchedule(int candidateId)
        {
            var candidateEntity = await _dbContext.WebsiteCandidateScheduleEntities.Where(model => model.CandidateScheduleId == candidateId).FirstOrDefaultAsync();
            if (candidateEntity != null)
            {
                candidateEntity.IsDeleted = true;
                _dbContext.WebsiteCandidateScheduleEntities.Update(candidateEntity);
                await _dbContext.SaveChangesAsync();
            }
            return candidateId;
        }

        public async Task<int> GetCandidateName(string scheduleName, int companyId)
        {
            var candidateNameCount = await _dbContext.WebsiteCandidateScheduleEntities.Where(y => y.ScheduleName == scheduleName && y.CompanyId == companyId).CountAsync();
            return candidateNameCount;
        }

        public async Task<List<WebSiteCandidateScheduleEntity>> GetCandidateSechudule(int companyId)
        {

            var candidateEntity = await (from candidateSchedule in _dbContext.WebsiteCandidateScheduleEntities
                                         join candidatePrivilege in _dbContext.websiteCandidatePrivilegesEntitys on candidateSchedule.CandidateScheduleId equals candidatePrivilege.CandidatescheduleId
                                         where candidatePrivilege.IsEnabled && candidatePrivilege.CompanyId == companyId
                                         select new WebSiteCandidateScheduleEntity()
                                         {
                                             CandidateScheduleId = candidateSchedule.CandidateScheduleId,
                                             ScheduleName = candidateSchedule.ScheduleName,

                                         }).ToListAsync();
            return candidateEntity;
        }

        public async Task WebsiteUpdateCandidateStatus(WebsiteCandidateMenuEntity websiteCandidateMenuEntity)
        {
            try
            {
                var candidateMenuEntity = await _dbContext.websiteCandidateMenuEntities.Where(c => c.CandidateMenuId == websiteCandidateMenuEntity.CandidateMenuId).FirstOrDefaultAsync();
                if (candidateMenuEntity != null)
                {
                    candidateMenuEntity.CandidateStatusId = websiteCandidateMenuEntity.CandidateStatusId;
                    _dbContext.websiteCandidateMenuEntities.Update(candidateMenuEntity);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch { }
        }

        //CandidatePrivilege

        public async Task<List<CandidatePrivileges>> GetCandidatePrivilege(int companyId)
        {
            var candidatePrivileges = await (from candidatePrivilege in _dbContext.websiteCandidatePrivilegesEntitys
                                             join candidate in _dbContext.WebsiteCandidateScheduleEntities on candidatePrivilege.CandidatescheduleId equals candidate.CandidateScheduleId
                                             where candidate.IsActive == true && candidate.IsDeleted == false && companyId == candidate.CompanyId
                                             select new CandidatePrivileges
                                             {
                                                 IsEnabled = candidatePrivilege.IsEnabled,
                                                 CandidatescheduleId = candidate.CandidateScheduleId,
                                                 SchedulerName = candidate.ScheduleName,
                                                 CompanyId = candidate.CompanyId,
                                                 CandidatePrivilegeId = candidatePrivilege.CandidatePrivilegeId

                                             }).ToListAsync();
            return candidatePrivileges;
        }


        public async Task<List<WebsiteCandidatePrivilegesEntitys>> GetCandidatePrivileges(int companyId)
        {
            var CandidatePrivilegesEntitys = await _dbContext.websiteCandidatePrivilegesEntitys.Where(m => m.CompanyId == companyId).ToListAsync();
            return CandidatePrivilegesEntitys;
        }
        public async Task<List<WebsiteCandidatePrivilegesEntitys>> CandidatePrivileges(bool isEnabled, int companyId)
        {
            var CandidatePrivilegesEntitys = await _dbContext.websiteCandidatePrivilegesEntitys.Where(m => m.CompanyId == companyId && m.IsEnabled == isEnabled).ToListAsync();
            return CandidatePrivilegesEntitys;
        }

        public async Task<List<WebsiteCandidatePrivilegesEntitys>> GetCandidatePrivilegeByRoleId(int candidatescheduleId, string delete, int companyId)
        {
            var candidatePrivilegeEntitys = await _dbContext.websiteCandidatePrivilegesEntitys.AsTracking().Where(x => x.CandidatescheduleId == candidatescheduleId && x.CompanyId == companyId).ToListAsync();
            if (delete == Common.Constant.Delete)
            {
                _dbContext.websiteCandidatePrivilegesEntitys.RemoveRange(candidatePrivilegeEntitys);
                await _dbContext.SaveChangesAsync();
            }
            return candidatePrivilegeEntitys;
        }

        public async Task<bool> AddCandidatePrivilegeByRoleId(List<WebsiteCandidatePrivilegesEntitys> websiteCandidatePrivilegesEntitys, int companyId)
        {
            if (websiteCandidatePrivilegesEntitys.Count() > 0)
            {
                websiteCandidatePrivilegesEntitys.ForEach(x =>
                {
                    x.CompanyId = companyId;
                });
                await _dbContext.websiteCandidatePrivilegesEntitys.AddRangeAsync(websiteCandidatePrivilegesEntitys);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> UpdateWebsiteCandidateMenu(WebsiteCandidateMenuEntity websiteCandidateMenu)
        {
            var result = false;
            try
            {
                if (websiteCandidateMenu != null)
                {
                    var websiteJobsEntitys = _dbContext.websiteCandidateMenuEntities.FirstOrDefault(e => e.CandidateMenuId == websiteCandidateMenu.CandidateMenuId);
                    websiteJobsEntitys.CandidateScheduleId = websiteCandidateMenu.CandidateScheduleId;
                    websiteJobsEntitys.ScheduledDate = websiteCandidateMenu.ScheduledDate;
                    websiteJobsEntitys.StartTime = websiteCandidateMenu.StartTime;
                    websiteJobsEntitys.EndTime = websiteCandidateMenu.EndTime;
                    websiteJobsEntitys.EmployeeName = websiteCandidateMenu.EmployeeName;
                    websiteJobsEntitys.EndTime = websiteCandidateMenu.EndTime;
                    websiteJobsEntitys.MeetingLink = websiteCandidateMenu.MeetingLink;
                    _dbContext.Update(websiteJobsEntitys);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public async Task<bool> UpdateWebsiteCandidateMenuMeetinglink(WebsiteCandidateMenuEntity websiteCandidateMenu)
        {
            var result = false;
            try
            {
                if (websiteCandidateMenu != null)
                {
                    var websiteJobsEntitys = _dbContext.websiteCandidateMenuEntities.FirstOrDefault(e => e.CandidateMenuId == websiteCandidateMenu.CandidateMenuId);

                    websiteJobsEntitys.MeetingLink = websiteCandidateMenu.MeetingLink;
                    _dbContext.Update(websiteJobsEntitys);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<List<WebSiteCandidateScheduleEntity>> GetCandidateById(int candidateId, int companyId)
        {
            var result = await _dbContext.WebsiteCandidateScheduleEntities.Where(model => model.CandidateScheduleId >= candidateId && companyId == model.CompanyId && model.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<WebsiteCandidateMenuModel> GetCandidateMenuByIds(int candidateMenuId)
        {

            var candidate = await (from candidateMenu in _dbContext.websiteCandidateMenuEntities
                                   join candidateSchedule in _dbContext.WebsiteCandidateScheduleEntities on candidateMenu.CandidateScheduleId equals candidateSchedule.CandidateScheduleId
                                   into Schedule
                                   from branch in Schedule.DefaultIfEmpty<WebSiteCandidateScheduleEntity>()
                                   where candidateMenu.CandidateMenuId == candidateMenuId
                                   select new WebsiteCandidateMenuModel()
                                   {
                                       CandidateScheduleId = candidateMenu.CandidateScheduleId,
                                       CandidateScheduleName = branch.ScheduleName,
                                       CandidateMenuId = candidateMenuId,
                                       CandidateStatusId = candidateMenu.CandidateStatusId,
                                       JobApplyId = candidateMenu.JobApplyId,
                                       JobId = candidateMenu.JobId,
                                       FullName = candidateMenu.FullName,
                                       Email = candidateMenu.Email,
                                       Experience = candidateMenu.Experience,
                                       FilePath = candidateMenu.FilePath,
                                       SkillSet = candidateMenu.SkillSet,
                                       RelevantExperience = candidateMenu.RelevantExperience,
                                       ApplyDate = candidateMenu.ApplyDate,
                                       Attachment = candidateMenu.Attachment,
                                       IsRecordForm = candidateMenu.IsRecordForm,
                                       CandidateStatusName = Convert.ToString((CandidateStatus)candidateMenu.CandidateStatusId),
                                       ScheduledDate = candidateMenu.ScheduledDate,
                                       StartTime = candidateMenu.StartTime,
                                       EndTime = candidateMenu.EndTime,
                                       EmployeeName = candidateMenu.EmployeeName,
                                       MeetingLink = candidateMenu.MeetingLink
                                   }).FirstOrDefaultAsync<WebsiteCandidateMenuModel>();
            return candidate;


        }

        public async Task<List<EmployeesEntity>> GetAllEmployees(int companyId)
        {
            return await _dbContext.Employees.Where(d => !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
        }

        /// <summary>
        /// Logic to get WebsiteCandidateMenu count 
        /// </summary>  
        /// <param name=" pager" ></param>
        public async Task<int> GetWebsiteCandidateMenuCount(SysDataTablePager pager,int companyId)
        {
            try
            {
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };

                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                List<EmployeesDataCount> getWebCandidateMenuCount = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetWebsiteCandidateMenuFilterCount] @companyId,@searchText", param1, param2).ToListAsync();
                foreach (var item in getWebCandidateMenuCount)
                {
                    var result = item.Id;
                    return result;
                }
                return 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Logic to get WebsiteJobApply  list details  
        /// </summary>        
        /// <param name="pager,columnDirection,columnName" >WebsiteJobApply</param> 
        public async Task<List<WebsiteJobApplyViewModel>> GetWebsideJobApplyList(SysDataTablePager pager, string columnDirection, string columnName)
        {
            try
            {
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection,
                };

                var param1 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
                var websideJobApply = await _dbContext.websideJobApplyViewModels.FromSqlRaw("EXEC [dbo].[spGetAllJobApply]  @pagingSize ,@offsetValue,@searchText,@sorting", param1, param2, param3, param4).ToListAsync();
                return websideJobApply;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Logic to get WebsideJobApply list count check 
        /// </summary>        
        /// <param name="pager" >WebsideJobApply</param>      
        public async Task<int> GetWebsideJobApplyListCount(SysDataTablePager pager)
        {
            var _params = new
            {
                SearchText = pager.sSearch
            };
            var param = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            List<WebsideJobApplyCount> websideJobApplyCount = await _dbContext.websideJobApplyCounts.FromSqlRaw("EXEC [dbo].[spGetAllJobApplyCount]  @searchText", param).ToListAsync();
            foreach (var item in websideJobApplyCount)
            {
                var result = item.JobApplyCountId;
                return result;
            }
            return 0;
        }

    }
}
