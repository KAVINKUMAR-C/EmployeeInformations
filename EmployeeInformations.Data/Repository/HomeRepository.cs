using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;

namespace EmployeeInformations.Data.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public HomeRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// ApplicationLog

        /// <summary>
        /// Logic to create application log
        /// </summary>   
        /// <param name="applicationLogEntity" ></param>
        /// <param name="applicationLogId" ></param>

        public async Task<int> CreateApplicationLog(ApplicationLogEntity applicationLogEntity)
        {
            if (applicationLogEntity?.ApplicationLogId == 0)
            {
                await _dbContext.ApplicationLog.AddAsync(applicationLogEntity);
                await _dbContext.SaveChangesAsync();
                return applicationLogEntity.ApplicationLogId;
            }
            return 0;
        }
    }
}
