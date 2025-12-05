using EmployeeInformations.Model.APIModel;

namespace EmployeeInformations.Business.API.IService
{
    public interface ITimeSheetAPIService
    {
        Task<List<TimeSheetRequestModel>> GetTimesheet(int empId, int companyId);
        Task<UserTimeSheetResponse> InsertTimesheet(TimeSheetRequestModel timeSheetRequestModel);
        Task<List<ProjectNamesAPI>> GetAllProjectNamesByEmpId(int empId, int companyId);
        Task<bool> DeleteTimeSheet(int TimeSheetId, int companyId);
    }
}
