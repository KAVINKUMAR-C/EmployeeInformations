using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.AttendanceViewModel;
using Microsoft.EntityFrameworkCore;


namespace EmployeeInformations.Data.Repository
{
 //   public class AttendanceRepository : IAttendanceRepository
 //   {
 //       private readonly EmployeesDbContext _dbContext;
 //       private readonly AttendanceDbContext _dbAttendanceContext;
     
	//	public AttendanceRepository(EmployeesDbContext dbContext,AttendanceDbContext dbAttendanceContext)
 //       {
 //           _dbContext = dbContext;
 //           _dbAttendanceContext = dbAttendanceContext;
 //       }

 //       /// Attendance

 //       /// <summary>
 //        /// Logic to get working hours for all employee
 //        /// </summary> 
 //       /// <param name="AttendanceEntitys" ></param> 
 //       public async Task<List<AttendanceEntity>> GetWorkingHourListForAll()
 //       {
 //           var attendaceEntitys = await _dbAttendanceContext.AttendanceEntitys.ToListAsync();
 //           return attendaceEntitys;
 //       }


 //       /// <summary>
 //       /// Logic to getb in out details for all employee
 //       /// </summary> 
 //       /// <param name="viewAttendanceLog" ></param> 
 //       public async Task<List<AttendanceEntitys>> ViewAttendanceData(ViewAttendanceLog viewAttendanceLog)
 //       {
 //           var listOfEmployeeInOutDetails = await _dbContext.AttendanceEntity.Where(x => x.EmployeeCode == viewAttendanceLog.EmployeeCode).ToListAsync();
 //           return listOfEmployeeInOutDetails;
 //       }


 //       /// <summary>
 //       /// Logic to get working hours for all employee
 //       /// </summary> 
 //       /// <param name="AttendanceEntity" ></param> 
 //       public async Task<List<AttendanceEntitys>> GetWorkingHourListForAllAttendancedb()
 //       {
 //           var attendaceEntitys = await _dbContext.AttendanceEntity.ToListAsync();
 //           return attendaceEntitys;
 //       }

 //       ///Attendance Report DateModel

 //       /// <summary>
 //       /// Logic to get filter the attendance detail by particular employee leave
 //       /// </summary>         
 //       /// <param name="proc" ></param>
 //       /// <param name="values" ></param> 
 //       public async Task<List<AttendanceReportDateModel>> GetAllEmployeesByAttendaceFilter(string proc, List<KeyValuePair<string, string>> values)
 //       {
 //           var parameters = new object[values.Count];
 //           for (int i = 0; i < values.Count; i++)
 //               parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

 //           var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
 //           paramnames = paramnames.TrimEnd(',');
 //           proc = proc + " " + paramnames;

 //           var leaveReportDateModel = await _dbContext.AttendanceReportDateModel.FromSqlRaw<AttendanceReportDateModel>(proc, parameters).AsNoTracking().ToListAsync();
 //           return leaveReportDateModel;
 //       }		

	//	/// <summary>
	//	/// Logic to get working hours for all employee
	//	 /// </summary> 
	//	/// <param name="AttendanceEntitys" ></param> 
	//	public async Task<AttendanceEntitys> GetWorkingHourList()
	//	{
	//		var attendaceEntitys = await _dbContext.AttendanceEntity.OrderByDescending(x=>x.Id).FirstOrDefaultAsync();            
	//	   return attendaceEntitys;			        
	//	}


	//	/// <summary>
	//	/// Logic to get working hours for all EmployeeAttendaceCount
	//	/// </summary> 
	//	/// <param name="AttendanceEntitys" ></param> 
	//	/// <param name="lastCount" ></param>
	//	public async Task<List<AttendanceEntity>> GetWorkingHourCount(int lastCount)
 //       {
 //           var EmployeeAttendaceCount = await _dbAttendanceContext.AttendanceEntitys.Where(x => x.Id > lastCount).OrderBy(x => x.Id).ToListAsync();
 //           return EmployeeAttendaceCount;
	//	}

	//	/// <summary>
	//	/// Logic to get working hours added for all EmployeeAttendace
	//	/// </summary> 
	//	/// <param name="attendanceEntitys" ></param> 	
	//	public async Task<bool> InsertAttendanceEntitys(List<AttendanceEntitys> attendanceEntitys)
	//	{
	//		var result = false;
			
	//		if (attendanceEntitys.Count() > 0)
	//		{
	//			await _dbContext.AttendanceEntity.AddRangeAsync(attendanceEntitys);
 //               result = await _dbContext.SaveChangesAsync() > 0;
	//		}                       
	//		return result;
	//	}

 //       public async Task<List<MailSchedulerEntity>> GetMailScheduler()
 //       {
 //           var mailSchedulerEntitys = await _dbContext.MailSchedulerEntity.Where(x => x.MailTime <= DateTime.Now && !x.IsDeleted && x.IsActive).ToListAsync();
 //           return mailSchedulerEntitys;
	//	}
	//}
}
