using EmployeeInformations.CoreModels.Model;

namespace EmployeeInformations.Data.IRepository
{
    public interface IDashboardRepository
    {
        Task<bool> InsertTimeLog(TimeLoggerEntity timeLoggerEntity);
        Task<bool> UpdateTimeLog(TimeLoggerEntity timeLoggerEntity);
        Task<List<TimeLoggerEntity>> GetTimeLogEntitysByEmployeeId(int employeeId,int companyId);
        Task<TimeLoggerEntity> GetLastTimeLogEntityByEmployeeId(int employeeId,int companyId);
        Task<List<ProjectDetailsEntity>> GetAllProjectDetails(int companyId);
        Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdList(int employeeId, DateTime fromDate, DateTime toDate,int companyId);
        Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdDate(int employeeId, DateTime fromDate, string fromTime, string startTime,int companyId);
        Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdDateList( DateTime fromDate, string fromTime, string startTime,int companyId);
        Task<List<TimeLoggerEntity>> GetLastTimeLogEntityByEmployeeIdDate(DateTime fromDate, int EmpId,int companyId);
        Task<List<TimeLoggerEntity>> GetTimeLogEntityByEmpId(int employeeId, DateTime date,int companyId);
    }
}
