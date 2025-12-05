using AutoMapper;
using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Business.Utility.Helper;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.API.Service
{
    public class LeaveAPIService : ILeaveAPIService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IEmployeeInformationService _employeeInformationService;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly ICompanyRepository _companyRepository;

        public LeaveAPIService(ILeaveRepository leaveRepository, IMapper mapper, IConfiguration config, IEmployeesRepository employeesRepository, IEmployeeInformationService employeeInformationService, IEmailDraftRepository emailDraftRepository, ICompanyRepository companyRepository)
        {
            _leaveRepository = leaveRepository;
            _mapper = mapper;
            _config = config;
            _employeesRepository = employeesRepository;
            _employeeInformationService = employeeInformationService;
            _emailDraftRepository = emailDraftRepository;
            _companyRepository = companyRepository;
        }

        public async Task<List<LeaveRequestModel>> GetAllLeaveSummarys(int empId,int companyId)
        {
            var result = new List<LeaveRequestModel>();
            if (empId == 0)
            {
                var listLeaveSummary = await _leaveRepository.GetAllLeaveSummary(empId, companyId);
                var getData = await GetAllemployeeLeave(listLeaveSummary, companyId);
                result = getData;
                return result;
            }
            else
            {
                var employeeIds = await _employeesRepository.GetAllEmployeeIdsReportingPersonForLeave(empId,companyId);
                var empIds = new List<int>();
                empIds.Add(empId);
                foreach (var employee in employeeIds)
                {
                    empIds.Add(employee.EmployeeId);
                }
                var getLeaveList = await _leaveRepository.GetAllLeaveSummarys(empIds,companyId);
                var getDataList = await GetAllemployeeLeave(getLeaveList, companyId);
                result = getDataList;
                return result;
            }
        }

        public async Task<List<LeaveRequestModel>> GetAllemployeeLeave(List<EmployeeAppliedLeaveEntity> employeeAppliedLeaves,int companyId)
        {
            var result = new List<LeaveRequestModel>();
            var listEmployee = await _employeesRepository.GetAllEmployees(companyId);
            var listLeave = await _employeeInformationService.GetLeaveTypeData();
            var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var count = 1;
            foreach (var item in employeeAppliedLeaves)
            {
                count = count == 6 ? 1 : count;
                var today = DateTime.Now;
                var getEmployee = listEmployee.FirstOrDefault(e => e.EmpId == item.EmpId);
                var getLeaveType = listLeave.FirstOrDefault(x => x.LeaveTypeId == item.AppliedLeaveTypeId);
                var employeeProfileImage = allEmployeeProfiles.FirstOrDefault(e => e.EmpId == item.EmpId);
                result.Add(new LeaveRequestModel()
                {
                    AppliedLeaveId = item.AppliedLeaveId == 0 ? 0 : item.AppliedLeaveId,
                    AppliedLeaveTypeId = item.AppliedLeaveTypeId,
                    EmpId = item.EmpId,
                    IsApproved = item.IsApproved,
                    AppliedLeave = item.LeaveApplied,
                    LeaveTypes = getLeaveType != null ? getLeaveType.LeaveType : string.Empty,
                    LeaveFromDate = item.LeaveFromDate,
                    LeaveToDate = item.LeaveToDate,
                    TotalLeave = item.LeaveApplied,
                    Reason = item.Reason,
                    LeaveFilePath = item.LeaveFilePath,
                    LeaveName = item.LeaveName,
                    EmployeeUserName = getEmployee != null ? getEmployee.UserName : string.Empty,
                    EmployeeName = getEmployee?.FirstName + "" + getEmployee?.LastName,
                    EmployeeProfileImage = employeeProfileImage != null ? employeeProfileImage.ProfileName : string.Empty,
                    ClassName = Common.Common.GetClassNameForLeaveDashboard(count)
                });
                count++;
            }
            result.OrderByDescending(x => x.CreatedDate).ToList();
            return result;
        }

        public async Task<List<LeaveRequestModel>> GetEmployeeLeave(int empId,int companyId)
        {
            var result = new List<LeaveRequestModel>();
            if (empId > 0)
            {
                var getLeaveList = await _leaveRepository.GetEmployeeIdsLeave(empId,companyId);
                var getData = await GetAllemployeeLeave(getLeaveList, companyId);
                result = getData;
                return result;
            }
            return result;
        }

        public async Task<List<LeaveRequestModel>> GetApporvedEmployees(int empId,int companyid)
        {
            var result = new List<LeaveRequestModel>();
            var employeeIds = await _employeesRepository.GetAllEmployeeIdsReportingPersonForLeave(empId,companyid);
            var empIds = new List<int>();
            foreach (var employee in employeeIds)
            {
                empIds.Add(employee.EmployeeId);
            }
            var getLeaveList = await _leaveRepository.GetAllLeaveSummarys(empIds,companyid);
            var getData = await GetAllemployeeLeave(getLeaveList, companyid);
            result = getData;
            return result;
        }

        public async Task<List<CompensatoryOffRequestModel>> GetAllCompensatoryOff(int empId,int companyId)
        {
            var result = new List<CompensatoryOffRequestModel>();
            var listLeaveSummary = await _leaveRepository.GetAllCompensatoryOffRequest(empId,companyId);

            if (empId == 0)
            {
                var GetComoffrequest = await GetAllCompensatoryRequests(listLeaveSummary,companyId);
                result = GetComoffrequest;
                return result;
            }
            else
            {
                var employeeIds = await _employeesRepository.GetAllEmployeeIdsReportingPersonForLeave(empId,companyId);
                var empIds = new List<int>();
                empIds.Add(empId);
                foreach (var employee in employeeIds)
                {
                    empIds.Add(employee.EmployeeId);
                }
                var getLeaveList = await _leaveRepository.GetAllCompensatoryOffRequests(empIds,companyId);
                var getCompOffRequest = await GetAllCompensatoryRequests(getLeaveList, companyId);
                result = getCompOffRequest;
                return result;
            }
        }

        public async Task<List<CompensatoryOffRequestModel>> GetAllCompensatoryRequests(List<CompensatoryRequestsEntity> compensatoryRequestsEntities,int companyId)
        {
            var result = new List<CompensatoryOffRequestModel>();
            var listEmployee = await _employeesRepository.GetAllEmployees(companyId);
            var listLeave = await _employeeInformationService.GetLeaveTypeData();
            var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var count = 1;
            foreach (var item in compensatoryRequestsEntities)
            {
                count = count == 6 ? 1 : count;
                var today = DateTime.Now;
                var getEmployee = listEmployee.FirstOrDefault(e => e.EmpId == item.EmpId && !e.IsDeleted);
                var employeeProfileImage = allEmployeeProfiles.FirstOrDefault(e => e.EmpId == item.EmpId);
                result.Add(new CompensatoryOffRequestModel()
                {
                    CompensatoryId = item.CompensatoryId == 0 ? 0 : item.CompensatoryId,
                    CompanyId = item.CompanyId,
                    EmpId = item.EmpId,
                    IsApproved = item.IsApproved,
                    WorkedDate = item.WorkedDate,
                    Remark = item.Remark,
                    DayCount = item.DayCount,
                    Reason = item.Reason,
                    EmployeeUserName = getEmployee != null ? getEmployee.UserName : string.Empty,
                    EmployeeName = getEmployee?.FirstName + "" + getEmployee?.LastName,
                    EmployeeProfileImage = employeeProfileImage != null ? employeeProfileImage.ProfileName : string.Empty,
                    ClassName = Common.Common.GetClassNameForLeaveDashboard(count)
                });
                count++;
            }
            result.OrderByDescending(x => x.WorkedDate).ToList();
            return result;
        }

        public async Task<UserLeaveResponse> InsertLeave(LeaveRequestModel leaveRequestModel)
        {
            var userLeaveResponse = new UserLeaveResponse();
            var results = false;
            var leaveDaysCount = 0;

            if (leaveRequestModel != null)
            {
                if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    var combinePath = "";
                    if (!string.IsNullOrEmpty(leaveRequestModel.Base64string))
                    {
                        var fileFormat = leaveRequestModel.FileFormat;
                        var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "LeaveAttachments");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath = Path.Combine(path, fileName);

                        Byte[] bytes = Convert.FromBase64String(leaveRequestModel.Base64string);
                        SaveByteArrayToFileWithFileStream(bytes, combinePath);
                    }


                    DateTime fromTime = Convert.ToDateTime(leaveRequestModel.PermissionFromTime);
                    DateTime toTime = Convert.ToDateTime(leaveRequestModel.PermissionToTime);

                    leaveRequestModel.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveFromDate);
                    leaveRequestModel.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveFromDate);

                    var permissionFromHour = leaveRequestModel.LeaveFromDate.AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                    var permissionToHour = leaveRequestModel.LeaveFromDate.AddHours(toTime.Hour).AddMinutes(toTime.Minute);
                    var today = DateTime.Now;
                    var employeeAppliedLeave = new EmployeeAppliedLeaveEntity();
                    employeeAppliedLeave.EmpId = leaveRequestModel.EmpId;
                    employeeAppliedLeave.AppliedLeaveTypeId = leaveRequestModel.AppliedLeaveTypeId;
                    employeeAppliedLeave.LeaveFromDate = permissionFromHour;
                    employeeAppliedLeave.LeaveToDate = permissionToHour;
                    employeeAppliedLeave.Reason = leaveRequestModel.Reason;
                    employeeAppliedLeave.LeaveApplied = 0;
                    employeeAppliedLeave.IsApproved = 0;
                    employeeAppliedLeave.IsDeleted = false;
                    employeeAppliedLeave.CreatedDate = DateTime.Now;
                    employeeAppliedLeave.CreatedBy = leaveRequestModel.EmpId;
                    employeeAppliedLeave.CompanyId = leaveRequestModel.CompanyId;
                    employeeAppliedLeave.LeaveFilePath = combinePath.Replace(Directory.GetCurrentDirectory(), "~");
                    results = await _leaveRepository.CreateLeave(employeeAppliedLeave, leaveRequestModel.CompanyId);
                    userLeaveResponse.IsSuccess = true;
                    userLeaveResponse.Message = Common.Constant.Success;
                }
                else
                {
                    var combinePath = "";
                    if (!string.IsNullOrEmpty(leaveRequestModel.Base64string))
                    {
                        var fileFormat = leaveRequestModel.FileFormat;
                        var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "LeaveAttachments");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath = Path.Combine(path, fileName);

                        Byte[] bytes = Convert.FromBase64String(leaveRequestModel.Base64string);
                        SaveByteArrayToFileWithFileStream(bytes, combinePath);
                    }
                    var employeeAppliedLeave = new EmployeeAppliedLeaveEntity();
                    employeeAppliedLeave.EmpId = leaveRequestModel.EmpId;
                    employeeAppliedLeave.AppliedLeaveTypeId = leaveRequestModel.AppliedLeaveTypeId;
                    employeeAppliedLeave.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveFromDate);
                    employeeAppliedLeave.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveToDate);
                    employeeAppliedLeave.Reason = leaveRequestModel.Reason;
                    employeeAppliedLeave.LeaveFilePath = combinePath.Replace(Directory.GetCurrentDirectory(), "~");
                    employeeAppliedLeave.LeaveName = leaveRequestModel.LeaveName;
                    leaveDaysCount = await GetLeaveCount(employeeAppliedLeave.LeaveFromDate, employeeAppliedLeave.LeaveToDate);
                    employeeAppliedLeave.LeaveApplied = leaveDaysCount;
                    employeeAppliedLeave.IsApproved = 0;
                    employeeAppliedLeave.IsDeleted = false;
                    employeeAppliedLeave.CreatedDate = DateTime.Now;
                    employeeAppliedLeave.CreatedBy = leaveRequestModel.EmpId;
                    employeeAppliedLeave.CompanyId = leaveRequestModel.CompanyId;
                    Common.Common.WriteServerErrorLog(" CreateLeave : ");

                    results = await _leaveRepository.CreateLeave(employeeAppliedLeave, leaveRequestModel.CompanyId);
                    userLeaveResponse.IsSuccess = true;
                    userLeaveResponse.Message = Common.Constant.Success;
                }
                if (results == true)
                {
                    var reason = leaveRequestModel.Reason;
                    var employee = new List<Employees>();
                    var toEmails = new List<string>();
                    var userName = await _employeesRepository.GetEmployeeByIdForLeave(leaveRequestModel.EmpId,leaveRequestModel.CompanyId);
                    var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(leaveRequestModel.EmpId,leaveRequestModel.CompanyId);
                    foreach (var item in reportingPersonEmployeeIds)
                    {
                        var email = await _leaveRepository.GetEmployeeEmailByEmpIdForLeave(item.ReportingPersonEmpId);
                        toEmails.Add(email);
                    }
                    var leaveFromDate = string.Empty;
                    var leaveToDate = string.Empty;

                    if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                    {
                        leaveFromDate = leaveRequestModel.StrLeaveFromDate; //leave.LeaveFromDate.ToString();
                        leaveToDate = leaveRequestModel.StrLeaveToDate; //leave.LeaveToDate.ToString();
                    }
                    else
                    {
                        leaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveFromDate).ToString(Constant.DateFormatMonthHyphen);
                        leaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveToDate).ToString(Constant.DateFormatMonthHyphen);
                    }

                    var strDay = leaveDaysCount < 2 ? "day" : "days";
                    var leavetype = string.Empty;
                    if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.CasualLeave)
                    {
                        leavetype = Constant.CasualLeave;
                    }
                    else if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.SickLeave)
                    {
                        leavetype = Constant.SickLeave;
                    }
                    else if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.EarnedLeave)
                    {
                        leavetype = Constant.EarnedLeave;
                    }
                    else if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.MaternityLeave)
                    {
                        leavetype = Constant.MaternityLeave;
                    }
                    else if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                    {
                        leavetype = Constant.Permission;
                    }
                    else if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.LOP)
                    {
                        leavetype = Constant.LOP;
                    }
                    else if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                    {
                        leavetype = Constant.WorkFromHome;
                    }
                    else if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.CompensatoryOff)
                    {
                        leavetype = Constant.CompensatoryOff;
                    }
                    if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                    {
                        var draftTypeId = (int)EmailDraftType.ApplyWorkFromHome;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,leaveRequestModel.CompanyId);
                        var bodyContent = EmailBodyContent.SendEmail_Body_CreateLeave(userName, leaveFromDate, leaveToDate, leaveDaysCount, leavetype, strDay, reason, emailDraftContentEntity.DraftBody);
                        await InsertEmailApplyWorkFromHome(userName.OfficeEmail, emailDraftContentEntity, bodyContent, leaveRequestModel.CompanyId);
                    }
                    else
                    {
                        var draftTypeId = (int)EmailDraftType.ApplyLeave;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, leaveRequestModel.CompanyId);
                        var bodyContent = EmailBodyContent.SendEmail_Body_CreateLeave(userName, leaveFromDate, leaveToDate, leaveDaysCount, leavetype, strDay, reason, emailDraftContentEntity.DraftBody);
                        await InsertEmailApplyLeave(userName.OfficeEmail, emailDraftContentEntity, bodyContent, leaveRequestModel.CompanyId);
                    }

                }
                return userLeaveResponse;
            }
            else
            {
                userLeaveResponse.IsSuccess = false;
                userLeaveResponse.Message = Common.Constant.Failure;
            }
            return userLeaveResponse;
        }

        public async Task<int> GetLeaveCount(DateTime leaveFromDate, DateTime leaveToDate)
        {
            var daysCount = 0;
            var startDate = leaveFromDate;
            var toDate = leaveToDate;

            var totcount = Convert.ToInt32(toDate.Subtract(startDate).TotalDays) + 1;
            if (startDate != DateTime.MinValue)
            {
                if (totcount == 4)
                {
                    for (DateTime date = startDate; date <= toDate; date = date.AddDays(1))
                    {
                        if (startDate.DayOfWeek == DayOfWeek.Friday || startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday || startDate.DayOfWeek == DayOfWeek.Monday)
                        {
                            daysCount++;
                        }

                        else if (startDate.DayOfWeek == DayOfWeek.Monday || startDate.DayOfWeek == DayOfWeek.Tuesday || startDate.DayOfWeek == DayOfWeek.Wednesday || startDate.DayOfWeek == DayOfWeek.Thursday || startDate.DayOfWeek == DayOfWeek.Friday || startDate.DayOfWeek != DayOfWeek.Saturday ||
                            startDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            daysCount++;
                        }
                        startDate = startDate.AddDays(1);
                    }
                    return daysCount;
                }
                else
                {
                    for (DateTime date = startDate; date <= toDate; date = date.AddDays(1))
                    {
                        if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            daysCount++;
                        }
                        startDate = startDate.AddDays(1);
                    }
                }
                return daysCount;
            }
            return daysCount;
        }

        private async Task InsertEmailApplyLeave(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent,int companyId)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.LeaveRequest;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }
        private async Task InsertEmailApplyWorkFromHome(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, int companyId)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.WorkFromHomeRequest;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            emailEntity.CompanyId = companyId;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        public static void SaveByteArrayToFileWithFileStream(byte[] data, string filePath)
        {
            using var stream = File.Create(filePath);
            stream.Write(data, 0, data.Length);
        }

        public async Task<UserLeaveResponse> UpdateLeave(LeaveRequestModel leaveRequestModel)
        {
            var userLeaveResponse = new UserLeaveResponse();
            bool result = false;
            if (leaveRequestModel != null)
            {
                if (leaveRequestModel.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    var combinePath = "";
                    if (!string.IsNullOrEmpty(leaveRequestModel.Base64string))
                    {
                        var fileFormat = leaveRequestModel.FileFormat;
                        var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "LeaveAttachments");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath = Path.Combine(path, fileName);

                        Byte[] bytes = Convert.FromBase64String(leaveRequestModel.Base64string);
                        SaveByteArrayToFileWithFileStream(bytes, combinePath);
                    }
                    DateTime fromTime = Convert.ToDateTime(leaveRequestModel.PermissionFromTime);
                    DateTime toTime = Convert.ToDateTime(leaveRequestModel.PermissionToTime);

                    leaveRequestModel.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveFromDate);
                    leaveRequestModel.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveToDate);

                    var permissionFromHour = leaveRequestModel.LeaveFromDate.AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                    var permissionToHour = leaveRequestModel.LeaveFromDate.AddHours(toTime.Hour).AddMinutes(toTime.Minute);

                    var employeeAppliedLeaveEntity = await _leaveRepository.GetLeaveByAppliedLeaveId(leaveRequestModel.AppliedLeaveId,leaveRequestModel.CompanyId);
                    employeeAppliedLeaveEntity.AppliedLeaveId = leaveRequestModel.AppliedLeaveId;
                    employeeAppliedLeaveEntity.EmpId = leaveRequestModel.EmpId;
                    employeeAppliedLeaveEntity.AppliedLeaveTypeId = leaveRequestModel.AppliedLeaveTypeId;
                    employeeAppliedLeaveEntity.LeaveFromDate = permissionFromHour;
                    employeeAppliedLeaveEntity.LeaveToDate = permissionToHour;
                    employeeAppliedLeaveEntity.Reason = leaveRequestModel.Reason;
                    employeeAppliedLeaveEntity.LeaveFilePath = string.IsNullOrEmpty(combinePath.Replace(Directory.GetCurrentDirectory(), "~")) ? employeeAppliedLeaveEntity.LeaveFilePath : combinePath.Replace(Directory.GetCurrentDirectory(), "~");
                    employeeAppliedLeaveEntity.LeaveName = string.IsNullOrEmpty(leaveRequestModel.LeaveName) ? employeeAppliedLeaveEntity.LeaveName : leaveRequestModel.LeaveName;
                    employeeAppliedLeaveEntity.LeaveApplied = 0;
                    employeeAppliedLeaveEntity.IsApproved = 0;
                    employeeAppliedLeaveEntity.IsDeleted = false;
                    employeeAppliedLeaveEntity.CreatedDate = employeeAppliedLeaveEntity.CreatedDate;
                    employeeAppliedLeaveEntity.CreatedBy = employeeAppliedLeaveEntity.CreatedBy;
                    employeeAppliedLeaveEntity.UpdatedDate = DateTime.Now;
                    employeeAppliedLeaveEntity.UpdatedBy = leaveRequestModel.EmpId;
                    result = await _leaveRepository.UpdateLeave(employeeAppliedLeaveEntity);
                    userLeaveResponse.IsSuccess = true;
                    userLeaveResponse.Message = Common.Constant.Success;
                    return userLeaveResponse;
                }
                else
                {
                    var combinePath = "";
                    if (!string.IsNullOrEmpty(leaveRequestModel.Base64string))
                    {
                        var fileFormat = leaveRequestModel.FileFormat;
                        var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "LeaveAttachments");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath = Path.Combine(path, fileName);

                        Byte[] bytes = Convert.FromBase64String(leaveRequestModel.Base64string);
                        SaveByteArrayToFileWithFileStream(bytes, combinePath);
                    }

                    var employeeAppliedLeaveEntity = await _leaveRepository.GetLeaveByAppliedLeaveId(leaveRequestModel.AppliedLeaveId,leaveRequestModel.CompanyId);
                    employeeAppliedLeaveEntity.AppliedLeaveId = leaveRequestModel.AppliedLeaveId;
                    employeeAppliedLeaveEntity.EmpId = leaveRequestModel.EmpId;
                    employeeAppliedLeaveEntity.AppliedLeaveTypeId = leaveRequestModel.AppliedLeaveTypeId;

                    employeeAppliedLeaveEntity.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveFromDate);
                    employeeAppliedLeaveEntity.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leaveRequestModel.StrLeaveToDate);

                    employeeAppliedLeaveEntity.Reason = leaveRequestModel.Reason;
                    employeeAppliedLeaveEntity.LeaveFilePath = string.IsNullOrEmpty(leaveRequestModel.LeaveFilePath) ? employeeAppliedLeaveEntity.LeaveFilePath : leaveRequestModel.LeaveFilePath;
                    employeeAppliedLeaveEntity.LeaveName = string.IsNullOrEmpty(leaveRequestModel.LeaveName) ? employeeAppliedLeaveEntity.LeaveName : leaveRequestModel.LeaveName;
                    var leaveDaysCount = await GetLeaveCount(employeeAppliedLeaveEntity.LeaveFromDate, employeeAppliedLeaveEntity.LeaveToDate);
                    employeeAppliedLeaveEntity.LeaveApplied = leaveDaysCount;
                    employeeAppliedLeaveEntity.IsApproved = 0;
                    employeeAppliedLeaveEntity.IsDeleted = false;
                    employeeAppliedLeaveEntity.CreatedDate = employeeAppliedLeaveEntity.CreatedDate;
                    employeeAppliedLeaveEntity.CreatedBy = employeeAppliedLeaveEntity.CreatedBy;
                    employeeAppliedLeaveEntity.UpdatedDate = DateTime.Now;
                    employeeAppliedLeaveEntity.UpdatedBy = leaveRequestModel.EmpId;
                    result = await _leaveRepository.UpdateLeave(employeeAppliedLeaveEntity);
                    userLeaveResponse.IsSuccess = true;
                    userLeaveResponse.Message = Common.Constant.Success;
                    return userLeaveResponse;
                }

            }
            else
            {
                userLeaveResponse.IsSuccess = false;
                userLeaveResponse.Message = Common.Constant.Failure;
            }
            return userLeaveResponse;
        }

        public async Task<UserLeaveResponse> InsertCompensatory(CompensatoryRequestAPI compensatoryRequestAPI)
        {
            var userLeaveResponse = new UserLeaveResponse();
            var result = false;

            if (compensatoryRequestAPI != null)
            {
                compensatoryRequestAPI.WorkedDate = DateTimeExtensions.ConvertToNotNullDatetime(compensatoryRequestAPI.StrWorkedDate);
                var compensatoryRequestsEntity = new CompensatoryRequestsEntity();
                if (compensatoryRequestAPI.CompensatoryId == 0)
                {
                    compensatoryRequestsEntity.EmpId = compensatoryRequestAPI.EmpId;
                    compensatoryRequestsEntity.CompanyId = compensatoryRequestAPI.CompanyId;
                    compensatoryRequestsEntity.WorkedDate = compensatoryRequestAPI.WorkedDate;
                    compensatoryRequestsEntity.Remark = compensatoryRequestAPI.Remark;
                    compensatoryRequestsEntity.IsApproved = 0;
                    compensatoryRequestsEntity.IsDeleted = false;
                    compensatoryRequestsEntity.DayCount = 0;
                    compensatoryRequestsEntity.CreatedBy = compensatoryRequestAPI.EmpId;
                    compensatoryRequestsEntity.CreatedDate = DateTime.Now;
                    result = await _leaveRepository.CreateCompensatory(compensatoryRequestsEntity, compensatoryRequestAPI.CompensatoryId);
                    if (result == true)
                    {
                        var remark = compensatoryRequestAPI.Remark;
                        var employee = new List<Employees>();
                        var toEmails = new List<string>();
                        var userName = await _employeesRepository.GetEmployeeByIdForLeave(compensatoryRequestAPI.EmpId,compensatoryRequestAPI.CompanyId);
                        if (userName != null)
                        {
                            var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(compensatoryRequestAPI.EmpId, compensatoryRequestAPI.CompanyId);
                            foreach (var item in reportingPersonEmployeeIds)
                            {
                                var email = await _leaveRepository.GetEmployeeEmailByEmpIdForLeave(item.ReportingPersonEmpId);
                                toEmails.Add(email);
                                Common.Common.WriteServerErrorLog(" email : " + email);
                            }
                            var workedDate = string.Empty;
                            workedDate = compensatoryRequestAPI.WorkedDate.ToString(Constant.DateFormatMonthHyphen);
                            var leavetype = string.Empty;
                            leavetype = Constant.CompensatoryOffRequest;
                            var draftTypeId = (int)EmailDraftType.ApplyCompensatoryOffRequest;
                            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,compensatoryRequestAPI.CompanyId);
                            var bodyContent = EmailBodyContent.SendEmail_Body_CreateCompensatoryOffRequest(userName, workedDate, leavetype, remark, emailDraftContentEntity.DraftBody);
                            await InsertEmailApplyCompensatoryOffRequest(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                            userLeaveResponse.IsSuccess = true;
                            userLeaveResponse.Message = Common.Constant.Success;
                        }

                    }
                }
                else
                {

                    var compensatoryEntity = await _leaveRepository.GetCompensatoryRequestByCompensatoryId(compensatoryRequestAPI.CompensatoryId);

                    compensatoryEntity.EmpId = compensatoryRequestAPI.EmpId;
                    compensatoryEntity.WorkedDate = compensatoryRequestAPI.WorkedDate;
                    compensatoryEntity.Remark = compensatoryRequestAPI.Remark;
                    compensatoryEntity.UpdatedBy = compensatoryRequestAPI.EmpId;
                    compensatoryEntity.UpdatedDate = DateTime.Now;
                    result = await _leaveRepository.CreateCompensatory(compensatoryEntity, compensatoryRequestAPI.CompensatoryId);
                    userLeaveResponse.IsSuccess = true;
                    userLeaveResponse.Message = Common.Constant.Success;
                }
                return userLeaveResponse;
            }
            else
            {
                userLeaveResponse.IsSuccess = false;
                userLeaveResponse.Message = Common.Constant.Failure;
            }
            return userLeaveResponse;
        }


        private async Task InsertEmailApplyCompensatoryOffRequest(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.CompensatoryOffRequest;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }


        /// <summary>
                /// Logic to get leavedetails count in particular employees
               /// </summary> 
        /// <param name="empId" ></param>       
        public async Task<LeaveRequestModel> GetAllLeaveDetails(int empId, int companyId)
        {
            var employeeLeaveViewModel = new LeaveRequestModel();
            var leaveCounts = new List<LeaveCountAPI>();
            var allLeaveDetails = await _leaveRepository.GetAllLeaveDetails(empId,companyId);
            var listLeaveSummarys = await _leaveRepository.GetLeaveByAppliedLeaveEmpId(empId,companyId);
            var leaveTypes = await GetAllLeave();
            var withoutPermissionLeaveTypes = leaveTypes.Where(x => x.LeaveTypeId != (int)LeavetypeStatus.Permission).ToList();
            foreach (var item in withoutPermissionLeaveTypes)
            {
                var leaveCount = new LeaveCountAPI();
                leaveCount.LeaveType = item.LeaveType;
                if (allLeaveDetails != null)
                {
                    if (leaveCount.LeaveType == Constant.CasualLeave)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.CasualLeaveCount;
                    }
                    else if (leaveCount.LeaveType == Constant.SickLeave)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.SickLeaveCount;
                    }
                    else if (leaveCount.LeaveType == Constant.EarnedLeave)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.EarnedLeaveCount;
                    }
                    else if (leaveCount.LeaveType == Constant.MaternityLeave)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.MaternityLeaveCount;
                    }
                    else if (leaveCount.LeaveType == Constant.CompensatoryOff)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.CompensatoryOffCount;
                    }
                }
                else
                {
                    leaveCount.TotalLeave = 0;
                }

                leaveCount.AppliedLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 0).Sum(x => x.LeaveApplied);
                leaveCount.ApprovedLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 1).Sum(x => x.LeaveApplied);
                var count = Helpers.TotalLeaveCount(leaveCount.TotalLeave,leaveCount.ApprovedLeave);
                if (count > 0)
                {
                    leaveCount.RemaingLeave = count;
                }
                else
                {
                    leaveCount.RemaingLeave = 0;
                }
                var appliedAndApproveLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved <= 1).Sum(x => x.LeaveApplied);
                leaveCount.SumOfAppliedLeaveAndApprovedLeave = Helpers.TotalLeaveCount(leaveCount.TotalLeave ,appliedAndApproveLeave);
                leaveCounts.Add(leaveCount);
            }
            var casualleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.CasualLeave).Select(x => x.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var sickleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.SickLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var earnedleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.EarnedLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var maternityleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.MaternityLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var compensatoryOff = leaveCounts.Where(x => x.LeaveType == Constant.CompensatoryOff).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();


            employeeLeaveViewModel.LeaveCountAPI = leaveCounts;
            employeeLeaveViewModel.LeaveTypesAPI = leaveTypes;
            employeeLeaveViewModel.CasualLeaveRemaining = casualleaveremaining;
            employeeLeaveViewModel.SickLeaveRemaining = sickleaveremaining;
            employeeLeaveViewModel.EarnedLeaveRemaining = earnedleaveremaining;
            employeeLeaveViewModel.MaternityLeaveRemaining = maternityleaveremaining;
            employeeLeaveViewModel.CompensatoryOffRemaining = compensatoryOff;
            return employeeLeaveViewModel;
        }

        public async Task<List<LeaveTypesAPI>> GetAllLeave()
        {
            var listLeave = await _leaveRepository.GetAllLeave();
            var result = _mapper.Map<List<LeaveTypesAPI>>(listLeave);
            return result;
        }
    }
}
