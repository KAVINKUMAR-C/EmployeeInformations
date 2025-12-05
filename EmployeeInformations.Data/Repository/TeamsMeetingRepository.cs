using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.CoreModels.Model;
using Microsoft.EntityFrameworkCore;
using EmployeeInformations.Model.TeamsViewModel;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class TeamsMeetingRepository : ITeamsMeetingRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public TeamsMeetingRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Logic to get create the teams 
        /// </summary>
        /// <param name="teamsMeetingEntity"></param>  
        public async Task <int> CreateTeamsMeeting(TeamsMeetingEntity teamsMeetingEntity, int companyId)
        {
            var result = 0;
            try
            {
                teamsMeetingEntity.CompanyId = companyId;
                if (teamsMeetingEntity?.TeamsMeetingId == 0)
                {                                      
                    await _dbContext.teamsMeetingEntities.AddAsync(teamsMeetingEntity);
                    await _dbContext.SaveChangesAsync();
                    result = teamsMeetingEntity.TeamsMeetingId;
                }
                else
                {
                    if (teamsMeetingEntity != null)
                    {
                         _dbContext.teamsMeetingEntities.Update(teamsMeetingEntity);
                        await _dbContext.SaveChangesAsync();
                        result = teamsMeetingEntity.TeamsMeetingId;
                    }
                }
                
            }
            catch (Exception ex) {
                
            }
            return result;
        }

        /// <summary>
        /// Logic to get empId the employees detail by particular empId
        /// </summary>  
        /// <param name="empId" >employees</param>
        /// <param name="CompanyId" >employees</param>
        public async Task<string> GetEmployeeOfficeMail(int empId, int companyId)
        {
            var employees = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmpId == empId && x.CompanyId == companyId);
            if (employees != null)
            {
                return employees.OfficeEmail;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get all employee teams details list and particular empId
        /// </summary>         
        /// <param name="empId,pager,columnDirection,columnName" ></param> 
        public async Task<List<TeamMeetingModel>> GetTeamMeetingEmployeesList(SysDataTablePager pager, int empId, string columnDirection, string columnName,int companyId)
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

                var param = new NpgsqlParameter("@empId", empId);
                var paramcompany = new NpgsqlParameter("@companyId", companyId);
                var param1 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
                var data = await _dbContext.TeamMeetingModels.FromSqlRaw("EXEC [dbo].[spGetTeamsMeetingList] @empId,@companyId,@pagingSize ,@offsetValue,@searchText,@sorting", param, paramcompany, param1, param2, param3, param4).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        /// <summary>
        /// Logic to get the list teams meeting details By using Filter Count
        /// </summary>  
        /// <param name="pager" ></param> 
        /// <param name="empId" ></param> 
        public async Task<int> GetAllTeamMeetingByFilterCount(SysDataTablePager pager, int empId, int companyId)
        {
            var result = 0;
            var _params = new
            {
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
            };

            var param1 = new NpgsqlParameter("@empId", empId);
            var param2 = new NpgsqlParameter("@companyId", companyId);
            var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var teamMeeting= await _dbContext.teamMeetingCounts.FromSqlRaw("EXEC [dbo].[spGetTeamMeetingListCount] @companyId,@empId,@searchText", param1, param2, param3).ToListAsync();
            foreach (var item in teamMeeting)
            {
                result = item.TeamCount;
            }
            return result;
        }



        /// <summary>
        /// Logic to get TeamsMeetingId the teams detail by particular TeamsMeetingId
         /// </summary>    
        /// <param name="TeamsMeetingId" >teams</param>
        /// <param name="IsDeleted" >teams</param>
        /// <param name="CompanyId" >teams</param>
        public async Task<TeamsMeetingEntity> GetByTeamsMeetingId(int teamsMeetingId, int companyId)
        {
            var teamsMeetingEntity = await _dbContext.teamsMeetingEntities.AsNoTracking().FirstOrDefaultAsync(d => d.TeamsMeetingId == teamsMeetingId && !d.IsDeleted && d.CompanyId == companyId);
            return teamsMeetingEntity ?? new TeamsMeetingEntity();
        }

        /// <summary>
        /// Logic to get delete the teams detail 
        /// </summary>
        /// <param name="teamsMeetingEntity" ></param>               
        public async Task<bool> DeleteTeamsMeeting(TeamsMeetingEntity teamsMeetingEntity)
        {
            var result = false;
            if (teamsMeetingEntity != null)
            {
                _dbContext.teamsMeetingEntities.Update(teamsMeetingEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

    }
}
