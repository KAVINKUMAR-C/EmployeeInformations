using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.Data.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public DashboardRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Time Logger

        /// <summary>
        /// Logic to get Add the timeloggerentitys detail
        /// </summary>   
        /// <param name="timeLoggerEntity" ></param>      
        public async Task<bool> InsertTimeLog(TimeLoggerEntity timeLoggerEntity)
        {
            await _dbContext.TimeLoggerEntitys.AddAsync(timeLoggerEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Logic to get update the timeloggerentitys detail
        /// </summary>   
        /// <param name="timeLoggerEntity" ></param> 
        public async Task<bool> UpdateTimeLog(TimeLoggerEntity timeLoggerEntity)
        {
            _dbContext.TimeLoggerEntitys.Update(timeLoggerEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Logic to get employeeId the timeloggerentitys detail
        /// </summary>   
        /// <param name="employeeId" >timeloggerentitys</param> 
        /// <param name="CompanyId" >timeloggerentitys</param> 
        /// <param name="ClockInTime.Date" >timeloggerentitys</param> 
        /// <param name="CreatedDate" >timeloggerentitys</param> 
        public async Task<List<TimeLoggerEntity>> GetTimeLogEntitysByEmployeeId(int employeeId, int companyId)
        {
            var timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.EmployeeId == employeeId && x.ClockInTime.Date == DateTime.Now.Date && x.CompanyId == companyId).OrderBy(x => x.CreatedDate).ToListAsync();
            return timeLoggerEntitys;
        }
        public async Task<List<TimeLoggerEntity>> GetTimeLogEntityByEmpId(int employeeId,DateTime date, int companyId)
        {
            var timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.EmployeeId == employeeId && x.ClockInTime.Date == date.Date && x.CompanyId == companyId).OrderBy(x => x.CreatedDate).ToListAsync();
            return timeLoggerEntitys;
        }

        /// <summary>
        /// Logic to get employeeId the timeloggerentitys detail by particular employeeId
        /// </summary>   
        /// <param name="employeeId" >timeloggerentitys</param> 
        /// <param name="CompanyId" >timeloggerentitys</param> 
        /// <param name="ClockInTime.Date" >timeloggerentitys</param> 
        /// <param name="CreatedDate" >timeloggerentitys</param> 
        public async Task<TimeLoggerEntity> GetLastTimeLogEntityByEmployeeId(int employeeId, int companyId)
        {
            var timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.EmployeeId == employeeId && x.ClockInTime.Date == DateTime.Now.Date && x.CompanyId == companyId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            return timeLoggerEntitys;
        }

        //// Project Details

        /// <summary>
        /// Logic to get projectdetails list
        /// </summary>         
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectDetailsEntity>> GetAllProjectDetails(int companyId)
        {
            return await _dbContext.ProjectDetails.Where(d => !d.IsDeleted && d.CompanyId == companyId).Take(5).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        /// <summary>
         /// Logic to get employeeId the timeloggerentitys detail by particular employeeId
        /// </summary>   
        /// <param name="employeeId" >timeloggerentitys</param> 
        /// <param name="fromDate" >timeloggerentitys</param> 
        /// <param name="toDate" >timeloggerentitys</param> 
        public async Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdList(int employeeId, DateTime fromDate, DateTime toDate, int companyId)
        {

            var timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.EmployeeId == employeeId && x.CreatedDate.Date >= fromDate && x.CreatedDate.Date <= toDate && x.CompanyId == companyId).OrderBy(x => x.EmployeeId).ToListAsync();

            return timeLoggerEntitys;

        }
        /// <summary>
         /// Logic to get employeeId the timeloggerentitys detail by particular employeeId using search filter
        /// </summary>   
        /// <param name="employeeId" >timeloggerentitys</param> 
        /// <param name="fromDate" >timeloggerentitys</param> 
        /// <param name="fTime" >timeloggerentitys</param> 
        /// <param name="sTime" >timeloggerentitys</param> 
        public async Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdDate(int employeeId, DateTime fromDate, string fTime, string sTime, int companyId)
        {
            var timeLoggerEntitys = new List<TimeLoggerEntity>();

            if (fTime == "" && sTime == "")
            {
                timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.EmployeeId == employeeId && x.CreatedDate.Date == fromDate.Date && x.CompanyId == companyId).OrderBy(x => x.EmployeeId).ToListAsync();
            }
            else
            {
                DateTime startTimes = Convert.ToDateTime(fromDate.Date.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(fTime).ToString("HH:mm:ss"));
                DateTime endTimes = Convert.ToDateTime(fromDate.Date.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(sTime).ToString("HH:mm:ss"));

                timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.EmployeeId == employeeId && x.CreatedDate.Date == fromDate.Date && x.CreatedDate <= endTimes && x.CreatedDate >= startTimes).OrderBy(x => x.EmployeeId).ToListAsync();

            }
            return timeLoggerEntitys;
        }
        /// <summary>
         /// Logic to get employeeId the timeloggerentitys all detail 
        /// </summary>   
        /// <param name="fromDate" >timeloggerentitys</param> 
        /// <param name="fTime" >timeloggerentitys</param> 
        /// <param name="sTime" >timeloggerentitys</param> 
        public async Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdDateList(DateTime fromDate, string fTime, string sTime, int companyId)
        {
            var timeLoggerEntitys = new List<TimeLoggerEntity>();
            if (fTime == "" && sTime == "")
            {
                timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.CreatedDate.Date == fromDate.Date && x.CompanyId == companyId).OrderBy(x => x.EmployeeId).ToListAsync();
            }
            else
            {
                DateTime startTimes = Convert.ToDateTime(fromDate.Date.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(fTime).ToString("HH:mm:ss"));
                DateTime endTimes = Convert.ToDateTime(fromDate.Date.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(sTime).ToString("HH:mm:ss"));

                timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.CreatedDate.Date == fromDate.Date && x.CompanyId == companyId && x.CreatedDate <= endTimes && x.CreatedDate >= startTimes).OrderBy(x => x.EmployeeId).ToListAsync();


            }
            return timeLoggerEntitys;
        }
        public async Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdDate(DateTime fromDate,int EmpId, int companyId)
        {
            var timeLoggerEntitys = new List<TimeLoggerEntity>();
            timeLoggerEntitys = await _dbContext.TimeLoggerEntitys.Where(x => x.CreatedDate.Date == fromDate.Date && x.CompanyId == companyId).OrderBy(x => x.EmployeeId).ToListAsync();
            return timeLoggerEntitys;
        }
    }
}
