using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.HelpdeskViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace EmployeeInformations.Data.Repository
{
    public class HelpdeskRepository : IHelpdeskRepository
    {
        private readonly EmployeesDbContext _dbContext;
        public HelpdeskRepository(EmployeesDbContext employeesDbContext)
        {
            _dbContext = employeesDbContext;
        }

        /// <summary>
        /// Logic to get the list helptask details 
        /// </summary>   
        public async Task<List<Helpdesk>>GetAllHelpdesk(int companyId)
        {
            var helptask = await(from help in _dbContext.HelpdeskEntity
                                 join ticket in _dbContext.TicketTypesEntity on help.TicketTypeId equals ticket.TicketTypeId
                                 join employee in _dbContext.Employees on help.EmpId equals employee.EmpId
                                 where !help.IsDeleted && !ticket.IsDeleted && companyId == ticket.CompanyId
                                 select new Helpdesk ()
                                  {
                                      Id = help.Id,
                                      EmpId = help.EmpId,
                                      TicketTypeId = help.TicketTypeId,
                                      TicketType = ticket.TicketName,
                                      Description = help.Description,
                                      Status = help.Status,
                                      TicketStatus = Convert.ToString((TicketStatus)help.Status),
                                      CreatedDate = help.CreatedDate,
                                      UpdatedDate = help.UpdatedDate,
                                      EmployeeName = employee.FirstName + " " + employee.LastName,
                                  }).ToListAsync();
            return helptask;
        }
        /// <summary>
        /// Logic to get the list helptask details By using Filter 
        /// </summary>  
        /// <param name="pager,columnName,columnDirection"></param>  
        public async Task<List<HelpDeskFilterEntity>> GetAllHelpdeskDetailsByFilter(SysDataTablePager pager, string columnDirection, string columnName,int companyId)
        {
            var result = new List<HelpDeskFilterEntity>();
            var _params = new
            {
                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
                Sorting = columnName + " " + columnDirection,
            };

            var param1 = new NpgsqlParameter("@companyId", companyId);
            var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var param3 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
            var param4 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
            var param5 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
        

            try
            {
                result = await _dbContext.HelpDeskFilterEntity.FromSqlRaw("EXEC [dbo].[spGetHelpdeskDetails] @companyId,@searchText,@pagingSize,@offsetValue,@Sorting", param1, param2, param3, param4,param5).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        /// <summary>
        /// Logic to get the list helptask details By using Filter Count
        /// </summary>  
        /// <param name="pager" ></param> 

        public async Task<int> GetAllHelpdeskDetailsByFilterCount(SysDataTablePager pager, int companyId)
        {
            var result = 0;
            var _params = new
            {
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
            };

            var param1 = new NpgsqlParameter("@companyId", companyId);
            var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var HelpdeskDetails = await _dbContext.HelpDeskCount.FromSqlRaw("EXEC [dbo].[spGetHelpdeskDetailsCount] @companyId,@searchText", param1, param2).ToListAsync();
            foreach (var item in HelpdeskDetails)
            {
                result = item.EmployeeCount;
            }
            return result;
        }
        /// <summary>
        /// Logic to get Create,update the helpdekEntity detail 
        /// </summary>   
        /// <param name="helpdekEntity" ></param>      
        public async Task<int> UpsertHelpdesk(HelpdeskEntity helpdekEntity)
        {
            try
            {
                var result = 0;
                if (helpdekEntity?.Id == 0)
                {
                    await _dbContext.HelpdeskEntity.AddAsync(helpdekEntity);
                    await _dbContext.SaveChangesAsync();
                    result = helpdekEntity.Id;
                }
                else
                {
                    if (helpdekEntity != null)
                    {
                        _dbContext.HelpdeskEntity.Update(helpdekEntity);
                        await _dbContext.SaveChangesAsync();
                        result = helpdekEntity.Id;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
              
            }
            return 0;
        }

        //// Tickets Attachment

        /// <summary>
        /// Logic to get Create,updatethe attachmentsEntitys detail by particular helpdeskEntity
        /// </summary>   
        /// <param name="ticketAttachments" ></param>
        /// <param name="helpdeskId" ></param>
        public async Task<bool> InsertTicketAttachment(List<TicketAttachmentsEntity> ticketAttachments, int helpdeskId)
        {
            try
            {
                var result = false;
                var attachmentsEntitys = await _dbContext.TicketAttachmentsEntity.Where(x => x.HelpdeskId == helpdeskId).ToListAsync();
                if (attachmentsEntitys.Count() == 0)
                {
                    await _dbContext.TicketAttachmentsEntity.AddRangeAsync(ticketAttachments);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                else
                {
                    _dbContext.TicketAttachmentsEntity.RemoveRange(attachmentsEntitys);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.TicketAttachmentsEntity.UpdateRange(ticketAttachments);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                return result;
            }
            catch (Exception ex)
            {

            }
           return false;    
        }

        /// <summary>
        /// Logic to get delete the helpdeskEntity detail by particular helpdeskEntity
        /// </summary>   
        /// <param name="helpdeskEntity" ></param>      
        public async Task<bool> DeleteHelpdesk(HelpdeskEntity helpdeskEntity)
        {
            _dbContext.HelpdeskEntity.Update(helpdeskEntity);
           var result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }       

        /// <summary>
        /// Logic to get the list of helpdesk detail by particular Id
        /// </summary>
        /// <param name="helpdeskId" ></param>
        public async Task<HelpdeskEntity> GetHelpdeskByHelpdeskId(int helpdeskId)
        {
            var helpdeskEntity = await _dbContext.HelpdeskEntity.FirstOrDefaultAsync(w => w.Id == helpdeskId && !w.IsDeleted);
            return helpdeskEntity ?? new HelpdeskEntity();
        }

        /// <summary>
        /// Logic to get the list of TicketAttachmentsEntity detail get by particular helpdeskId
        /// </summary>
        /// <param name="helpdeskId" ></param>
        public async Task<List<TicketAttachmentsEntity>> GetTicketDocumentAndFilePath(int helpdeskId)
        {
            var TicketAttachmentsEntitys = await _dbContext.TicketAttachmentsEntity.Where(e => e.HelpdeskId == helpdeskId && !e.IsDeleted).ToListAsync();
            return TicketAttachmentsEntitys;
        }

        /// <summary>
        /// Logic to get the list of TicketAttachmentsEntity detail delete by particular helpdeskId
        /// </summary>
        /// <param name="helpdeskId" ></param>
        public async Task<bool> DeleteTicketAttachement(List<TicketAttachmentsEntity> ticketAttachmentsEntities)
        {
            _dbContext.TicketAttachmentsEntity.UpdateRange(ticketAttachmentsEntities);
            var result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }

        /// <summary>
        /// Logic to get the helpdesk detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        public async Task<HelpdeskEntity> GetByhelpId(int id)
        {
            var helpdeskEntity = await _dbContext.HelpdeskEntity.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted );
            return helpdeskEntity ?? new HelpdeskEntity();
        }

        
    }
}
