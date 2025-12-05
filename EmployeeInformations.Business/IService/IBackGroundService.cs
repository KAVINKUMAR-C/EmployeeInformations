

namespace EmployeeInformations.Business.IService
{
	public interface IBackGroundService
	{
        Task<string> EmailQueueScheduler();
        Task<string> EmployeesWorkAnniversary();
        Task<bool> GetLeaveCalculation();
        Task<string> EmployeeBirthday();
        Task<string> EmployeesProbation();
        Task<bool> AttendanceLog();
        Task<bool> EmailScheduler(int companyId);
    }
}
