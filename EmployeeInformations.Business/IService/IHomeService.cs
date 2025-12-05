namespace EmployeeInformations.Business.IService
{
    public interface IHomeService
    {
        Task<int> createError(string host, string path, string exmsg, string stacktrace, int sessionEmployeeId, int companyId);
    }
}
