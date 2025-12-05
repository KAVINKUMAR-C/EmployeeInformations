using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.ClientSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public ClientRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //// Client

        /// <summary>
        /// Logic to get client
        /// </summary>           
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<ClientEntity>> GetAllClient(int companyId)
        {
            return await _dbContext.Client.Where(d => !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get clientId the client detail by particular clientId
        /// </summary>    
        /// <param name="ClientId" >client</param>
        /// <param name="IsDeleted" >client</param>
        /// <param name="CompanyId" >client</param>
        public async Task<ClientEntity> GetByClientId(int ClientId, int companyId)
        {
            var clientEntity = await _dbContext.Client.AsNoTracking().FirstOrDefaultAsync(d => d.ClientId == ClientId && !d.IsDeleted && d.CompanyId == companyId);
            return clientEntity ?? new ClientEntity();
        }


        /// <summary>
        /// Logic to get create and update the client detail 
        /// </summary>    
        /// <param name="clientEntity" ></param>       
        public async Task<int> CreateClient(ClientEntity clientEntity, int companyId)
        {
            if (clientEntity?.ClientId == 0)
            {
                clientEntity.CompanyId = companyId;
                await _dbContext.Client.AddAsync(clientEntity);
                await _dbContext.SaveChangesAsync();
                return clientEntity.ClientId;
            }
            else
            {
                if (clientEntity != null)
                {
                    clientEntity.CompanyId = companyId;
                    _dbContext.Client.Update(clientEntity);
                    await _dbContext.SaveChangesAsync();
                }
                return clientEntity != null ? clientEntity.ClientId : 0;
            }
        }


        /// <summary>
        /// Logic to get delete the client detail by particular clientId
        /// </summary>    
        /// <param name="ClientId" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeleteClient(int ClientId, int companyId)
        {
            var result = false;
            var clientEntity = await _dbContext.Client.Where(d => d.ClientId == ClientId && d.CompanyId == companyId).FirstOrDefaultAsync();
            if (clientEntity != null)
            {
                clientEntity.IsDeleted = true;
                _dbContext.Client.Update(clientEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get count the client detail 
        /// </summary>    
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> GetClientMaxCount(int companyId)
        {
            var count = await _dbContext.Client.Where(x => !x.IsDeleted && x.CompanyId == companyId).CountAsync();
            return count;
        }

        /// <summary>
        /// Logic to get all client details list
        /// </summary>    
        public async Task<List<ClientViewModel>> GetAllClients(int companyId)
        {
            var clients = (from client in _dbContext.Client
                           where !client.IsDeleted && companyId == client.CompanyId
                           orderby client.ClientId
                           select new ClientViewModel
                           {
                               ClientId = client.ClientId,
                               ClientName = client.ClientName,
                               ClientCompany = client.ClientCompany,
                               ClientDetails = client.ClientDetails,
                               Email = client.Email,
                               PhoneNumber = string.IsNullOrEmpty(client.PhoneNumber) ? string.Empty : client.PhoneNumber,
                           }).ToList();
             return clients.ToList();
        }
        /// <summary>
        ///  Logic to get all client count by SP
        /// </summary>
        /// <param name="companyId,pager" ></param>  
        public async Task<int> ClientViewCount(int companyId, SysDataTablePager pager)
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
                    SearchText = pager.sSearch
                }; 
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);

                List<ClientFilterCount>clientFilterCount = await _dbContext.clientFilterCount.FromSqlRaw("EXEC [dbo].[spGetClientFilterCount] @companyId,@searchText", param1, param2).ToListAsync();
                foreach (var item in clientFilterCount)
                {
                    result = item.Id;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        ///  Logic to get all client filter data by SP
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection" ></param>  
        public async Task<List<ClientFilterViewModel>> GetClientFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                
                var comId = Convert.ToString(companyId);
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
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
                int page = pager.sEcho;
                int records_per_page = pager.iDisplayLength;

                // Calculate start and end indices for pagination
                int start_index = page * records_per_page;
                start_index = start_index / 10;
                if (start_index < 0)
                {
                    start_index = 0;
                }
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", records_per_page);
                var param3 = new NpgsqlParameter("@offsetValue", start_index);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var data = await _dbContext.clientFilterViewModel.FromSqlRaw("EXEC [dbo].[spGetClientFilter] @companyId,@pagingSize,@offsetValue,@searchText,@sorting", param1, param2, param3, param4,param5).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
