using EmployeeInformations.Model.APIModel;

namespace EmployeeInformations.Business.API.IService
{
    public interface ILeaveAPIService
    {
        Task<List<LeaveRequestModel>> GetEmployeeLeave(int empId,int companyId);
        Task<List<LeaveRequestModel>> GetApporvedEmployees(int empId,int companyId);
        Task<UserLeaveResponse> InsertLeave(LeaveRequestModel leaveRequestModel);
        Task<UserLeaveResponse> UpdateLeave(LeaveRequestModel leaveRequestModel);
        Task<UserLeaveResponse> InsertCompensatory(CompensatoryRequestAPI compensatoryRequestAPI);
        Task<LeaveRequestModel> GetAllLeaveDetails(int empId,int companyId);
        Task<List<CompensatoryOffRequestModel>> GetAllCompensatoryOff(int empId, int companyId);
        Task<List<LeaveRequestModel>> GetAllLeaveSummarys(int empId, int companyId);
    }
}
