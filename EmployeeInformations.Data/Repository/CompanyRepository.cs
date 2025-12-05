using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public CompanyRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Company

        /// <summary>
        /// Logic to get Company list
        /// </summary>           
        /// <param name="IsDeleted" >emaildrafttype</param>      
        public async Task<List<CompanyEntity>> GetAllCompany()
        {
            return await _dbContext.Company.Where(g => !g.IsDeleted).ToListAsync();
        }


        /// <summary>
        /// Logic to get create and update the Company detail
        /// </summary>           
        /// <param name="companyEntity" ></param>        
        public async Task<int> CreateCompany(CompanyEntity companyEntity)
        {
            var result = 0;
            if (companyEntity?.CompanyId == 0)
            {
                await _dbContext.Company.AddAsync(companyEntity);
                await _dbContext.SaveChangesAsync();
                result = companyEntity != null ? companyEntity.CompanyId : 0;
            }
            else
            {
                if (companyEntity != null)
                {
                    _dbContext.Company.Update(companyEntity);
                    await _dbContext.SaveChangesAsync();
                }
                result = companyEntity != null ? companyEntity.CompanyId : 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the Company detail by particular companyId
        /// </summary>           
        /// <param name="CompanyId" >Company</param>  
        public async Task<bool> DeleteCompany(int CompanyId)
        {
            var result = false;
            var companyEntity = await _dbContext.Company.Where(d => d.CompanyId == CompanyId).FirstOrDefaultAsync();
            if (companyEntity != null)
            {
                companyEntity.IsDeleted = true;
                _dbContext.Company.Update(companyEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get companyId the Company detail by particular companyId
        /// </summary>           
        /// <param name="companyId" >Company</param> 
        /// <param name="IsDeleted" >Company</param>
        public async Task<CompanyEntity> GetByCompanyId(int companyId)
        {
            var company = await _dbContext.Company.FirstOrDefaultAsync(d => d.CompanyId == companyId && !d.IsDeleted);
            return company ?? new CompanyEntity();
        }


        /// <summary>
        /// Logic to get count of employees by id
        /// </summary>             
        /// <param name="CompanyId" ></param>     
        public async Task<int> GetMaxCountOfEmployeesByCompanyId(int companyId)
        {
            var count = await _dbContext.Employees.Where(d => d.CompanyId == companyId).CountAsync();
            return count;
        }

        ///Company Email Setting

        /// <summary>
        /// Logic to create a mail setting 
        /// </summary>             
        /// <param name="MailSchedulerEntity" ></param>     
        public async Task<bool> CreateMailScheduler(MailSchedulerEntity mailSchedulerEntity)
        {
            try
            {
                var result = false;
                mailSchedulerEntity.CompanyId = mailSchedulerEntity.CompanyId;
                if (mailSchedulerEntity?.SchedulerId == 0)
                {
                    await _dbContext.MailSchedulerEntity.AddAsync(mailSchedulerEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                else
                {
                    if (mailSchedulerEntity != null)
                    {
                        _dbContext.MailSchedulerEntity.Update(mailSchedulerEntity);
                        result = await _dbContext.SaveChangesAsync() > 0;
                    }
                }
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }

        }


        /// <summary>
        /// Logic to get delete the mail schedule detail by Id
        /// </summary>      
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeletedMailSchedule(int schedulerId, int companyId)
        {
            var result = false;
            var mailSchedulerEntity = await _dbContext.MailSchedulerEntity.FirstOrDefaultAsync(m => m.SchedulerId == schedulerId && m.CompanyId == companyId && !m.IsDeleted);
            if (mailSchedulerEntity != null)
            {
                mailSchedulerEntity.IsDeleted = true;
                _dbContext.MailSchedulerEntity.Update(mailSchedulerEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update the status in mail schedule d
        /// </summary>  
        /// <param name="CompanyId" ></param>
        public async Task<bool> StatusMailSchedule(MailSchedulerEntity mailSchedulerEntity)
        {
            var result = false;
            var mailSchedulerEntitys = await _dbContext.MailSchedulerEntity.Where(j => j.SchedulerId == mailSchedulerEntity.SchedulerId && !j.IsDeleted && j.CompanyId == mailSchedulerEntity.CompanyId).AsNoTracking().FirstOrDefaultAsync();
            if (mailSchedulerEntitys != null)
            {
                mailSchedulerEntitys.IsActive = mailSchedulerEntity.IsActive;
                _dbContext.MailSchedulerEntity.Update(mailSchedulerEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }

            return result;
        }


        /// <summary>
        /// Logic to get a mail setting by id
        /// </summary>             
        /// <param name="id" ></param>     
        public async Task<MailSchedulerEntity> GetMailSchedulerBySchedulerId(int schedulerId)
        {
            var mailSchedulerEntity = await _dbContext.MailSchedulerEntity.FirstOrDefaultAsync(d => d.SchedulerId == schedulerId && !d.IsDeleted);
            return mailSchedulerEntity ?? new MailSchedulerEntity();
        }
       
        /// Employees setting
        /// <summary>
        /// Logic to get probationmonth the emailsettings detail
        /// </summary>           
        /// <param name="Probationmonths" >emailsettings</param>
        public async Task<int> GetProbationMonth()
        {
            var emailEntity = await _dbContext.EmployeeSettingsEntity.Select(x => x.ProbationMonths).FirstOrDefaultAsync();
            return emailEntity;
        }

        /// Email setting
        /// <summary>
        /// Logic to get emailsettings detail
        /// </summary>           
        /// <param name="IsDeleted" >emailsettings</param>
        public async Task<EmailSettingsEntity> GetEmailSettingsEntity()
        {
            var emailEntity = await _dbContext.EmailSettings.FirstOrDefaultAsync(x => !x.IsDeleted);
            return emailEntity ?? new EmailSettingsEntity();
        }


        /// <summary>
        /// Logic to get email queue entity detail
        /// </summary> 
        /// <param name="emailQueueEntity" ></param>
        /// <param name="IsDeleted" >emailqueueentitys</param>
        public async Task<bool> InsertEmailQueueEntity(EmailQueueEntity emailQueueEntity)
        {
            var result = false;
            await _dbContext.EmailQueueEntitys.AddAsync(emailQueueEntity);
            result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }

        /// Branch Location

        /// <summary>
        /// Logic to get BranchLocation list
        /// </summary>           
        /// <param name="IsDeleted" >BranchLocation</param>         
        public async Task<List<BranchLocationEntity>> GetAllBranchLocation(int companyId)
        {
            return await _dbContext.BranchLocation.Where(f => !f.IsDeleted && f.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get create the assetcategory detail
        /// </summary>
        /// <param name="branchLocationEntity" ></param>
        public async Task<int> Create(BranchLocationEntity branchLocationEntity, int companyId)
        {
            var result = 0;
            if (branchLocationEntity?.BranchLocationId == 0)
            {
                branchLocationEntity.CompanyId = companyId;
                await _dbContext.BranchLocation.AddAsync(branchLocationEntity);
                await _dbContext.SaveChangesAsync();
                result = branchLocationEntity.BranchLocationId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update the branchLocation detail by particular branchLocation
        /// </summary>             
        /// <param name="branchLocationEntity" ></param>     
        public async Task UpdateBranchLocation(BranchLocationEntity branchLocationEntity, int companyId)
        {
            if (branchLocationEntity != null)
            {
                branchLocationEntity.CompanyId = companyId;
                _dbContext.BranchLocation.Update(branchLocationEntity);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get check branchLocationName the branchLocation detail by particular branchLocationName
        /// </summary>
        /// <param name="branchLocationName" ></param>        
        /// <param name="CompanyId" ></param>        
        public async Task<int> GetBranchLocationName(string branchLocationName, int companyId)
        {
            var branchLocationCount = await _dbContext.BranchLocation.Where(y => y.BranchLocationName == branchLocationName && y.CompanyId == companyId).CountAsync();
            return branchLocationCount;
        }


        /// <summary>
        /// Logic to get status the branchLocation detail
        /// </summary>
        /// <param name="branchLocationEntity" ></param>
        /// <param name="branchLocationEntity.BranchLocationId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task UpdateBranchStatus(BranchLocationEntity branchLocationEntity)
        {
            var branchLocationEntitys = await _dbContext.BranchLocation.FirstOrDefaultAsync(j => j.BranchLocationId == branchLocationEntity.BranchLocationId && !j.IsDeleted && j.CompanyId == branchLocationEntity.CompanyId);
            if (branchLocationEntitys != null)
            {
                branchLocationEntitys.IsActive = branchLocationEntity.IsActive;
                _dbContext.BranchLocation.Update(branchLocationEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the branchLocation detail by particular branchLocationId
        /// </summary>
        /// <param name="branchLocationId" ></param>        
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeletedBranch(int branchLocationId, int companyId)
        {
            var result = false;
            var branchLocationEntitys = await _dbContext.BranchLocation.FirstOrDefaultAsync(m => m.BranchLocationId == branchLocationId && m.CompanyId == companyId);
            if (branchLocationEntitys != null)
            {
                branchLocationEntitys.IsDeleted = true;
                _dbContext.BranchLocation.Update(branchLocationEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get companySettingId the companySetting detail by particular companySettingId
        /// </summary>           
        /// <param name="companySettingId" >companySetting</param>
        public async Task<CompanySettingEntity> GetByCompanySettingId(int CompanyId)
        {
            var companySettingEntity = await _dbContext.CompanySetting.FirstOrDefaultAsync(d => d.CompanyId == CompanyId);
            return companySettingEntity ?? new CompanySettingEntity();
        }

        /// <summary>
        /// Logic to get create and update the companysetting detail by particular companysetting
        /// </summary>   
        /// <param name="companySettingEntiy" ></param>
        public async Task<int> CreateEmailSettings(CompanySettingEntity companySettingEntiy)
        {
            var result = 0;
            if (companySettingEntiy?.CompanySettingId == 0)
            {
                await _dbContext.CompanySetting.AddAsync(companySettingEntiy);
                await _dbContext.SaveChangesAsync();
                result = companySettingEntiy.CompanySettingId;
            }
            else if (companySettingEntiy?.CompanySettingId != null)
            {
                var emailEntity = new CompanySettingEntity();
                companySettingEntiy.CompanyId = companySettingEntiy.CompanyId;
                emailEntity.ModeId = companySettingEntiy.ModeId;
                emailEntity.TimeZone = companySettingEntiy.TimeZone;
                emailEntity.Currency = companySettingEntiy.Currency;
                emailEntity.Language = companySettingEntiy.Language;
                companySettingEntiy.CompanyCode = companySettingEntiy.CompanyCode;
                companySettingEntiy.GSTNumber = companySettingEntiy.GSTNumber;
                companySettingEntiy.IsTimeLockEnable = companySettingEntiy.IsTimeLockEnable;
                _dbContext.CompanySetting.Update(companySettingEntiy);
                await _dbContext.SaveChangesAsync();
                result = companySettingEntiy.CompanySettingId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get companySettingId the companySetting detail by particular companySettingId
        /// </summary>           
        /// <param name="companySettingId" >companySetting</param>
        public async Task<CompanySettingEntity> GetcompanysettingId(int companySettingId)
        {
            var companySettingEntity = await _dbContext.CompanySetting.AsNoTracking().FirstOrDefaultAsync(d => d.CompanySettingId == companySettingId);
            return companySettingEntity ?? new CompanySettingEntity();
        }

        /// <summary>
         /// Logic to get delete the companySetting detail by particular companySettingId
        /// </summary>
        /// <param name="companySettingId" ></param>        
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeletedCompanySetting(int companySettingId)
        {
            var result = false;
            var companySettingEntitys = await _dbContext.CompanySetting.FirstOrDefaultAsync(m => m.CompanySettingId == companySettingId);
            if (companySettingEntitys != null)
            {
                companySettingEntitys.IsDeleted = true;
                _dbContext.CompanySetting.Update(companySettingEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get companysetting list
        /// </summary>           
        /// <param name="CompanyId" ></param>   
        /// <param name="IsDeleted" ></param>  
        public async Task<CompanySettingEntity> GetAllCompanySetting(int companyId)
        {
            var companySettingEntity = await _dbContext.CompanySetting.FirstOrDefaultAsync(c => !c.IsDeleted && c.CompanyId == companyId);
            return companySettingEntity ?? new CompanySettingEntity();
        }


        /// <summary>
        /// Logic to get EmailQueueEntity detail
        /// </summary>           
        /// <param name="IsSend" >EmailQueueEntity</param>
        public async Task<List<EmailQueueEntity>> GetEmailQueueEntity()
        {
            var emailQueueEntitys = await _dbContext.EmailQueueEntitys.Where(x => !x.IsSend).ToListAsync();
            return emailQueueEntitys;
        }

        /// <summary>
        /// Logic to get update IsSend the emailQueue detail by particular emailQueue
        /// </summary> 
        /// <param name="emailQueueEntity" ></param>   	
        public async Task<bool> UpdateEmailQueue(int emailQueueId, bool isSend)
        {
            var result = false;
            var emailQueueEntitys = await _dbContext.EmailQueueEntitys.FirstOrDefaultAsync(j => j.EmailQueueID == emailQueueId && !j.IsSend);
            if (emailQueueEntitys != null)
            {
                emailQueueEntitys.IsSend = true;
                _dbContext.EmailQueueEntitys.Update(emailQueueEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get EmployeeSettingEntity detail
        /// </summary> 
        public async Task<EmployeeSettingEntity> GetEmployeeSetting()
        {
            var employeeSetting = await _dbContext.EmployeeSettingsEntity.FirstOrDefaultAsync(x => !x.IsDeleted);
            return employeeSetting;
        }

        /// <summary>
        /// Logic to get email queue detail by particular companyId
        /// </summary> 
        /// <param name="companyId" ></param>   	
        public async Task<List<EmailQueueEntity>> GetEmailQueueEntity(int companyId)
        {
            var emailQueueEntitys = await _dbContext.EmailQueueEntitys.Where(x => !x.IsSend && x.CompanyId == companyId).ToListAsync();
            return emailQueueEntitys;
        }

        /// <summary>
        /// Logic to get email setting detail by particular companyId
        /// </summary> 
        /// <param name="companyId" ></param>   	
        public async Task<EmailSettingsEntity> GetEmailSettingsEntity(int companyId)
        {
            var emailEntity = await _dbContext.EmailSettings.FirstOrDefaultAsync(x => !x.IsDeleted && x.CompanyId == companyId);
            return emailEntity ?? new EmailSettingsEntity();
        }

        /// <summary>
        /// Logic to get all company details list
        /// </summary>        
        public async Task<List<Company>> GetAllCompanys()
        {
            var allCompany = await (from company in _dbContext.Company
                                    join city in _dbContext.cities on company.PhysicalAddressCity equals city.CityId
                                    join state in _dbContext.state on company.PhysicalAddressState equals state.StateId
                                    join mailingCity in _dbContext.cities on company.MailingAddressCity equals mailingCity.CityId
                                    into cities
                                    from companyCity in cities.DefaultIfEmpty()
                                    join mailingState in _dbContext.state on company.MailingAddressState equals mailingState.StateId
                                    into states
                                    from companyState in states.DefaultIfEmpty()
                                    where !company.IsDeleted
                                    select new Company()
                                    {
                                        CompanyId = company.CompanyId,
                                        CompanyName = company.CompanyName,
                                        CompanyPhoneNumber = company.CompanyPhoneNumber,
                                        CompanyEmail = company.CompanyEmail,
                                        Industry = company.Industry,
                                        ContactPersonName = company.ContactPersonFirstName + " " + company.ContactPersonLastName,
                                        PersonGender = Convert.ToString((Gender)company.ContactPersonGender),
                                        CompanyPhysicalstate = state.StateName,
                                        CompanyPhysicalcity = city.CityName,
                                        CompanyMailingstate = companyState.StateName,
                                        CompanyMailingcity = companyCity.CityName,
                                        ContactPersonEmail = company.ContactPersonEmail,
                                    }).ToListAsync();
            return allCompany;
        }

        /// <summary>
        /// Logic to get all branch location details list
        /// </summary>   
        public async Task<List<BranchLocation>> GetAllBranchLocations(int companyId)
        {
            var allBranchLocation = await (from branch in _dbContext.BranchLocation
                                           where !branch.IsDeleted && companyId == branch.CompanyId
                                           select new BranchLocation()
                                           {
                                               BranchLocationId = branch.BranchLocationId,
                                               BranchLocationName = branch.BranchLocationName,
                                               IsActive = branch.IsActive,
                                               CompanyId = branch.CompanyId,    
                                           }).ToListAsync();
            return allBranchLocation;
        }

        /// <summary>
        /// Logic to get all mail scheduler list detils
        /// </summary>                          
        public async Task<List<MailSchedulerViewModel>> GetAllMailSchedulerViewModel(int companyId)
        {
            var mailSchedulerViewModel = await (from mailScheduler in _dbContext.MailSchedulerEntity
                                                join company in _dbContext.Company on mailScheduler.CompanyId equals company.CompanyId
                                                where !mailScheduler.IsDeleted && companyId == mailScheduler.CompanyId 
                                                select new MailSchedulerViewModel()
                                                {
                                                    SchedulerId = mailScheduler.SchedulerId,
                                                    CompanyId= mailScheduler.CompanyId,
                                                    ReportName = mailScheduler.ReportName,
                                                    MailTime = mailScheduler.MailTime,
                                                    DurationId = mailScheduler.DurationId,
                                                    WhomToSend = mailScheduler.WhomToSend,
                                                    FileFormat = mailScheduler.FileFormat,
                                                    IsActive = mailScheduler.IsActive,
                                                    EmailDraftId = mailScheduler.EmailDraftId,
                                                    MailSendingDays = mailScheduler.MailSendingDays,
                                                    CompanyName = company.CompanyName
                                                }).ToListAsync();
            return mailSchedulerViewModel;
        }

        /// <summary>
        /// Logic to get company list count check 
        /// </summary>        
        /// <param name="pager" >company</param>      
        public async Task<int> GetCompanyListCount(SysDataTablePager pager)
        {                        
            var _params = new
            {               
                SearchText = pager.sSearch
            };            
            var param = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            List<CompanyCounts> compnayCounts = await _dbContext.CompnayCounts.FromSqlRaw("EXEC [dbo].[spGetCompanyFilterListCount] @searchText", param).ToListAsync();
            foreach (var item in compnayCounts)
            {
                var result = item.CompnayCountId;
                return result;
            }
            return 0;
        }


        /// <summary>
        /// Logic to get company list details  
        /// </summary>        
        /// <param name="pager,columnDirection,columnName" >company</param> 
        public async Task<List<CompanyViewModels>> GetCompanyDetailsList(SysDataTablePager pager, string columnDirection, string columnName)
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
                var companyList = await _dbContext.companyViewModels.FromSqlRaw("EXEC [dbo].[spGetCompnayFilterList] @pagingSize ,@offsetValue,@searchText,@sorting", param1, param2, param3, param4).ToListAsync();
                return companyList;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Logic to get of eMailScheduler List
        /// </summary>
        /// <param name="pager,companyId,columnName,columnDirection" ></param> 

        public async Task<List<EmailSchedulerViewModel>> GetAllEmailScheduler(SysDataTablePager pager,  string columnName, string columnDirection, int companyId)
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
                var param = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param3 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var data = await _dbContext.emailSchedulerViewModels.FromSqlRaw("EXEC [dbo].[spGetMailSchedulerFilterList] @companyId,@pagingSize,@offsetValue,@searchText,@sorting", param, param2, param3, param4, param5).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Logic to get count of eMailScheduler 
        /// </summary>
        /// <param name="pager" ></param> 


        public async Task<int> GetAllEmailSchedulerfilterCount(SysDataTablePager pager,int companyId)
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
                List<EmailSchedulerFilterCount> emailSchedulerFilterCounts = await _dbContext.emailSchedulerFilterCounts.FromSqlRaw("EXEC [dbo].[spGetMailSchedulerFilterListCount]  @companyId,@searchText",param1, param2).ToListAsync();
                foreach (var item in emailSchedulerFilterCounts)
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

    }
    
    
}
