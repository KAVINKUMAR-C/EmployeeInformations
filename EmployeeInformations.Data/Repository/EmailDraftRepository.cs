using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.EmailDraftViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace EmployeeInformations.Data.Repository
{
    public class EmailDraftRepository : IEmailDraftRepository
    {

        private readonly EmployeesDbContext _dbContext;
        public EmailDraftRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        ////Email Draft Type

        /// <summary>
         /// Logic to get emaildrafttype list
         /// </summary>           
        /// <param name="IsDeleted" >emaildrafttype</param>
        /// <param name="CompanyId" >emaildrafttype</param>
        public async Task<List<EmailDraftTypeEntity>> GetAllEmailDraftType()
        {
            return await _dbContext.EmailDraftType.Where(g => !g.IsDeleted).ToListAsync();
        }


        /// <summary>
        /// Logic to get create the emaildrafttype detail
        /// </summary>           
        /// <param name="emailDraftTypeEntity" ></param>
        public async Task<int> CreateEmailDraftType(EmailDraftTypeEntity emailDraftTypeEntity)
        {
            var result = 0;
            if (emailDraftTypeEntity?.Id == 0)
            {
                await _dbContext.EmailDraftType.AddAsync(emailDraftTypeEntity);
                await _dbContext.SaveChangesAsync();
                result = emailDraftTypeEntity.Id;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update the emaildrafttype detail
        /// </summary>           
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="IsDeleted" >emaildrafttype</param>       
        public async Task UpdateDraftType(EmailDraftTypeEntity emailDraftTypeEntity)
        {
            var emailDraftTypeEntitys = await _dbContext.EmailDraftType.FirstOrDefaultAsync(j => j.Id == emailDraftTypeEntity.Id && !j.IsDeleted);
            if (emailDraftTypeEntitys != null)
            {
                emailDraftTypeEntitys.Status = emailDraftTypeEntity.Status;
                _dbContext.EmailDraftType.Update(emailDraftTypeEntitys);
                _dbContext.SaveChanges();
            }
        }


        /// <summary>
        /// Logic to get delete the emaildrafttype detail by particular Id
        /// </summary>           
        /// <param name="id" >emaildrafttype</param>              
        public async Task<bool> DeletedEmailDraftType(int id)
        {
            var result = false;
            var emailDraftTypeEntity = await _dbContext.EmailDraftType.FirstOrDefaultAsync(m => m.Id == id);
            if (emailDraftTypeEntity != null)
            {
                emailDraftTypeEntity.IsDeleted = true;
                _dbContext.EmailDraftType.Update(emailDraftTypeEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get Id the emaildrafttype detail by particular Id
        /// </summary>           
        /// <param name="Id" >emaildrafttype</param>
        public async Task<EmailDraftTypeEntity> GetById(int Id)
        {
            var emailDraftTypeEntity = await _dbContext.EmailDraftType.FirstOrDefaultAsync(d => d.Id == Id);
            return emailDraftTypeEntity ?? new EmailDraftTypeEntity();
        }


        /// <summary>
        /// Logic to get draftType count the emaildrafttype detail
        /// </summary>           
        /// <param name="draftType" >emaildrafttype</param>
        public async Task<int> GetDraftTypeName(string draftType, int companyId)
        {
            var draftTypeNameCount = await _dbContext.EmailDraftType.Where(y => y.DraftType == draftType && y.CompanyId == companyId).CountAsync();
            return draftTypeNameCount;
        }

        //// Email Draft Content 

        /// <summary>
        /// Logic to get create and update the emaildraftcontent detail
        /// </summary>           
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="CompanyId" >EmailDraftContent</param>
        public async Task<int> CreateEmailDraftContent(EmailDraftContentEntity emailDraftContentEntity, int companyId)
        {
            if (emailDraftContentEntity?.EmailDraftTypeId == 0)
            {
                emailDraftContentEntity.CompanyId = companyId;
                await _dbContext.EmailDraftContent.AddAsync(emailDraftContentEntity);
                await _dbContext.SaveChangesAsync();
                return emailDraftContentEntity.EmailDraftTypeId;
            }
            else
            {
                if (emailDraftContentEntity != null)
                {
                    emailDraftContentEntity.CompanyId = companyId;
                    _dbContext.EmailDraftContent.Update(emailDraftContentEntity);
                    _dbContext.SaveChanges();
                }
                return emailDraftContentEntity != null ? emailDraftContentEntity.EmailDraftTypeId : 0;
            }
        }


        /// <summary>
        /// Logic to get id the emaildraftcontent detail by particular id
        /// </summary>           
        /// <param name="Id" >emaildraftcontent</param>     
        public async Task<EmailDraftContentEntity> GetByEmailDraftTypeId(int Id, int companyId)
        {
            var emailDraftContentEntity = await _dbContext.EmailDraftContent.AsNoTracking().FirstOrDefaultAsync(d => d.Id == Id && d.CompanyId == companyId);
            return emailDraftContentEntity ?? new EmailDraftContentEntity();
        }
        public async Task<EmailDraftContentEntity> GetByEmailDraftType(int Id)
        {
            var emailDraftContentEntity = await _dbContext.EmailDraftContent.AsNoTracking().FirstOrDefaultAsync(d => d.Id == Id);
            return emailDraftContentEntity ?? new EmailDraftContentEntity();
        }


        /// <summary>
        /// Logic to get emailDraftTypeId the emaildraftcontent detail by particular emailDraftTypeId
        /// </summary>           
        /// <param name="emailDraftTypeId" >emaildraftcontent</param>
        public async Task<EmailDraftContentEntity> GetEmailTrapTypeId(int emailDraftTypeId, int companyId)
        {
            var emailDraftContentEntity = await _dbContext.EmailDraftContent.AsNoTracking().FirstOrDefaultAsync(d => d.EmailDraftTypeId == emailDraftTypeId && d.CompanyId == companyId);
            return emailDraftContentEntity ?? new EmailDraftContentEntity();
        }

        /// <summary>
        /// Logic to get emailDraftType details list 
        /// </summary>           
       
        public async Task <List<EmailDraftType>> GetAllEmailDraftTypes (int companyId)
        {
            var emailDraftType = (from emailDraft in _dbContext.EmailDraftType
                                  where !emailDraft.IsDeleted where emailDraft.CompanyId == companyId
                                   select new EmailDraftType
                                   {
                                       Id = emailDraft.Id,
                                       DraftType = emailDraft.DraftType,
                                       Status = emailDraft.Status,
                                       IsDeleted = emailDraft.IsDeleted,
                                       CompanyId = emailDraft.CompanyId
                                   }).ToList();
            return emailDraftType.ToList();
        }
        /// <summary>
        /// Logic to get all the emaildrafttype list using pagination
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        public async Task<List<EmailDraftEntites>>GetAllEmailDraftTypesByPagination(SysDataTablePager pager,string columnName, string columnDirection, int companyId)
        {
            
            var result = new List<EmailDraftEntites>();
            var _params = new
            {
                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
                Sorting = columnName + " " + columnDirection,
            };

      
            var param1 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var param2 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
            var param3 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
            var param4 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
            var param5 = new NpgsqlParameter("@companyId", companyId);


            try
            {
                result = await _dbContext.EmailDraft.FromSqlRaw("EXEC  [dbo].[spGetEmailDraftDetails]  @searchText,@pagingSize,@offsetValue,@Sorting,@companyId", param1, param2, param3, param4,param5).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        /// <summary>
        /// Logic to get all the emaildrafttype count using pagination
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        public async Task<int> GetAllEmailDraftTypesCountByPagination(SysDataTablePager pager, int companyId)
        {
            var result = 0;
            var emaildraft = new List<EmailDraftCount>();
            var _params = new
            {
              
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
                
            };

            var param1 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var param2 = new NpgsqlParameter("@companyId", companyId);

            try
            {
                emaildraft = await _dbContext.EmailDraftCount.FromSqlRaw("EXEC [dbo].[spGetEmailDraftDetailsCount]  @searchText,@companyId", param1,param2).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            foreach (var item in emaildraft)
            {
                result = item.EmailCount;
            }

            return result;
        }
    }
}
