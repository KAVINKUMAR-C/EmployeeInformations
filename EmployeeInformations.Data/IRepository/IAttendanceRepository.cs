using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.AttendanceViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IAttendanceRepository
    {
        Task<List<AttendanceEntity>> GetWorkingHourListForAll();
        Task<List<AttendanceEntitys>> GetWorkingHourListForAllAttendancedb();
        Task<List<AttendanceReportDateModel>> GetAllEmployeesByAttendaceFilter(string proc, List<KeyValuePair<string, string>> values);
        Task<List<AttendanceEntitys>> ViewAttendanceData(ViewAttendanceLog viewAttendanceLog);
        Task<AttendanceEntitys> GetWorkingHourList();
        Task<List<AttendanceEntity>> GetWorkingHourCount(int lastCount);
        Task<bool> InsertAttendanceEntitys(List<AttendanceEntitys> attendanceEntitys);
        Task<List<MailSchedulerEntity>> GetMailScheduler();
	}
}
