using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using Microsoft.Extensions.Configuration;
using EmployeeInformations.Business.Utility.Helper;
using System.Text;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.APIModel;

namespace EmployeeInformations.Business.Service
{
    public class LeaveService : ILeaveService
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IEmployeeInformationService _employeeInformationService;
        private readonly IConfiguration _config;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;

        public LeaveService(ILeaveRepository leaveRepository, IMapper mapper, IEmployeesRepository employeesRepository, IEmployeeInformationService employeeInformationService, IConfiguration config, ICompanyRepository companyRepository, IEmailDraftRepository emailDraftRepository)
        {
            _leaveRepository = leaveRepository;
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _config = config;
            _employeeInformationService = employeeInformationService;
            _companyRepository = companyRepository;
            _emailDraftRepository = emailDraftRepository;
        }

        //Leaves

        /// <summary>
        /// Logic to get leave list by particular employees
        /// </summary> 
        /// <param name="empId, pager, columnName, columnDirection" ></param>
        public async Task<List<EmployeeLeavesModel>> GetAllLeaveSummary(SysDataTablePager pager, int empId, string columnName, string columnDirection, int companyId)
        {

            var result = new List<EmployeeLeavesModel>();
            if (empId == 0)
            {
                var listLeaveSummary = await _leaveRepository.GetAllEmployeesLeaves(pager, empId, columnName, columnDirection,companyId);
                result = listLeaveSummary;
                return result;
            }
            else
            {
                var getLeaveList = await _leaveRepository.GetReportingPersonsLeaves(pager, empId, columnName, columnDirection,companyId);
                result = getLeaveList;
                return result;
            }

        }

        /// <summary>
        /// Logic to get applyleave details list by particular employees
        /// </summary> 
        /// <param name="empId, pager, columnName, columnDirection" ></param>
        public async Task<List<EmployeeLeavesModel>> GetApplyEmployee(SysDataTablePager pager, int empId, string columnName, string columnDirection, int companyId)
        {
            var result = new List<EmployeeLeavesModel>();

            if (empId > 0)
            {
                var getLeaveByEmpId = await _leaveRepository.GetLeavesByEmpId(pager, empId, columnName, columnDirection,companyId);
                result = getLeaveByEmpId;
                return result;
            }
            return result;
        }

        /// <summary>
        /// Logic to get apporvedleave details list by particular employees
        /// </summary> 
        /// <param name="empId" ></param>
        public async Task<List<EmployeeLeaveViewModel>> GetApporvedEmployee(int empId)
        {
            var result = new List<EmployeeLeaveViewModel>();
            var getLeaveList = await _leaveRepository.GetReportingPersonsLeave(empId);
            result = getLeaveList;
            return result;
        }

        /// <summary>
        /// Logic to get apporvedleave details list by particular employees
        /// </summary> 
        /// <param name="empId, pager, columnName, columnDirection" ></param>
        public async Task<List<EmployeeLeavesModel>> GetApporvedEmployees(SysDataTablePager pager, int empId, string columnName, string columnDirection, int companyId)
        {
            var result = new List<EmployeeLeavesModel>();
            var getApprovedLeave = await _leaveRepository.GetReportingPersonsLeaves(pager, empId, columnName, columnDirection,companyId);
            result = getApprovedLeave;
            return result;
        }

        /// <summary>
        /// Logic to get workfromhome details list by particular employees
        /// </summary> 
        /// <param name="empId, pager"></param>
        public async Task<List<EmployeeLeaveViewModel>> GetAllWorkFromHomeSummary(int empId, int companyId)
        {
            var result = new List<EmployeeLeaveViewModel>();
            if (empId == 0)
            {
                var allEmployeesWFH = await _leaveRepository.GetAllWorkFromHome(companyId);
                result = allEmployeesWFH;
                return result;
            }
            else
            {
                var employeesWFH = await _leaveRepository.GetEmployeeReportingWorkFromHome(empId,companyId);
                result = employeesWFH;
                return result;
            }
        }

        /// <summary>
        /// Logic to get WorkFromHome data total count of the employees
        /// </summary>
        ///  <param name="empId, companyId,pager"></param>
        public async Task<int> WorkFromHomeCount(int empId, int companyId, SysDataTablePager pager)
        {
            var result = await _leaveRepository.WorkFromHomeCount(empId, companyId, pager);
            return result;
        }
        public async Task<int> WorkFromHomeForTeamLeadCount(int empId, int companyId, SysDataTablePager pager)
        {
            var result = await _leaveRepository.WorkFromHomeForTeamLeadCount(empId, companyId, pager);
            return result;
        }

        /// <summary>
        /// Logic to get  WorkFromHome Filter data of the employees
        /// </summary>
        /// <param name="empId, companyId,pager,columnName,columnDirection"></param>
        public async Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterData(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            var wfhFilter = await _leaveRepository.GetWorkFromHomeFilterData(empId, companyId, pager,columnName,columnDirection);
            return wfhFilter;

        }

        /// <summary>
        /// Logic to get  WorkFromHome Filter data of the employees
        /// </summary>
        /// <param name="empId, companyId,pager,columnName,columnDirection"></param>
        public async Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterDataForTeamLead(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            var wfhFilter = await _leaveRepository.GetWorkFromHomeFilterDataForTeamLead(empId, companyId, pager, columnName, columnDirection);
            return wfhFilter;

        }
        /// <summary>
        /// Logic to get leave list 
        /// </summary>        
        public async Task<List<LeaveTypes>> GetAllLeave()
        {
            var listLeave = await _leaveRepository.GetAllLeave();
            var result = _mapper.Map<List<LeaveTypes>>(listLeave);
            return result;
        }

        /// <summary>
        /// Logic to get leavedetails count in particular employees
        /// </summary> 
        /// <param name="empId" ></param>       
        public async Task<EmployeeLeaveViewModel> GetAllLeaveDetails(int empId, int companyId)
        {
            var employeeLeaveViewModel = new EmployeeLeaveViewModel();
            var leaveCounts = new List<LeaveCount>();
            var allLeaveDetails = await _leaveRepository.GetAllLeaveDetails(empId,companyId);
            var listLeaveSummarys = await _leaveRepository.GetLeaveByAppliedLeaveEmpId(empId,companyId);
            var leaveTypes = await _leaveRepository.GetLeaveType(empId);
            var leaveType = _mapper.Map<List<LeaveTypes>>(leaveTypes);

            var withoutPermissionLeaveTypes = leaveTypes.Where(x => x.LeaveTypeId != (int)LeavetypeStatus.Permission).ToList();
            foreach (var item in withoutPermissionLeaveTypes)
            {
                var leaveCount = new LeaveCount();
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
                    else if (leaveCount.LeaveType == Constant.LOP)
                    {
                        leaveCount.TotalLeave = 0.0m;
                    }
                    else if (leaveCount.LeaveType == Constant.WorkFromHome)
                    {
                        leaveCount.TotalLeave = 0.0m;
                    }
                }
                else
                {
                    leaveCount.TotalLeave = 0.0m;
                }

                leaveCount.AppliedLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 0).Sum(x => x.LeaveApplied);
                leaveCount.ApprovedLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 1).Sum(x => x.LeaveApplied);
                var count = Helpers.TotalLeaveCount(leaveCount.TotalLeave, (leaveCount.AppliedLeave + leaveCount.ApprovedLeave));
                if (count > 0)
                {
                    leaveCount.RemaingLeave = count;
                }
                else
                {
                    leaveCount.RemaingLeave = 0;
                }
                var appliedAndApproveLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved <= 1).Sum(x => x.LeaveApplied);
                leaveCount.SumOfAppliedLeaveAndApprovedLeave = Helpers.TotalLeaveCount(leaveCount.TotalLeave, appliedAndApproveLeave);
               // leaveCount.SumOfAppliedLeaveAndApprovedLeave = Helpers.TotalLeaveCount(leaveCount.TotalLeave, leaveCount.ApprovedLeave);
                leaveCounts.Add(leaveCount);
            }
            var casualleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.CasualLeave).Select(x => x.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var sickleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.SickLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var earnedleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.EarnedLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var maternityleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.MaternityLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var compensatoryOff = leaveCounts.Where(x => x.LeaveType == Constant.CompensatoryOff).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();


            employeeLeaveViewModel.LeaveCounts = leaveCounts;
            employeeLeaveViewModel.LeaveType = leaveType;
            employeeLeaveViewModel.CasualLeaveRemaining = casualleaveremaining;
            employeeLeaveViewModel.SickLeaveRemaining = sickleaveremaining;
            employeeLeaveViewModel.EarnedLeaveRemaining = earnedleaveremaining;
            employeeLeaveViewModel.MaternityLeaveRemaining = maternityleaveremaining;
            employeeLeaveViewModel.CompensatoryOffRemaining = compensatoryOff;
            return employeeLeaveViewModel;
        }

        /// <summary>
        /// Logic to Restrict leave applied already leave approved on the day 
        /// </summary> 
        /// <param name="empId" ></param>
        public async Task<bool> VerifyLeave(EmployeeLeaveViewModel leave, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var verify = await _leaveRepository.GetAllLeaveSummary(sessionEmployeeId, companyId);
           if (verify.Count != 0) {
                var filter = verify.Where(e => e.IsApproved == 1 && e.LeaveFromDate == DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate)).Count();
                if (filter == 0)
                {
                    var approved = verify.Where(e => e.IsApproved == 1 && e.LeaveFromDate.Month == DateTime.Now.Month).ToList();
                    foreach (var item in approved)
                    {
                        var sDate = item.LeaveFromDate;
                        var eDate = item.LeaveToDate;
                        var check = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate);
                        if (eDate == check)
                        {
                            result = true; break;
                        }
                        else
                        {
                            var count = eDate.Subtract(sDate).TotalDays;
                            if (count > 1)
                            {
                                return check >= sDate && check <= eDate;
                            }
                           
                        }
                    }
                }
                else
                {
                    result = true;
                }
           }
    
            
            return result;
        }

        /// <summary>
        /// Logic to get create leave by particular employees
        /// </summary> 
        /// <param name="leave" ></param> 
        /// <param name="sessionEmployeeId" ></param> 
        public async Task<bool> CreateLeave(EmployeeLeaveViewModel leave, int sessionEmployeeId, int companyId)
        {
            var results = false;
            var leaveDaysCount = 0;
            string halfDay = "0.5";
            decimal isHalfDay;
            decimal.TryParse(halfDay, out isHalfDay);

            decimal leaveCount;
            //var filter = verify.Where(e => e.LeaveFromDate == DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate)).Select(e=>e.IsApproved).Count();

            if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
            {
                DateTime fromTime = Convert.ToDateTime(leave.PermissionFromTime);
                DateTime toTime = Convert.ToDateTime(leave.PermissionToTime);

                leave.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate);
                leave.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate);

                var permissionFromHour = leave.LeaveFromDate.AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                var permissionToHour = leave.LeaveFromDate.AddHours(toTime.Hour).AddMinutes(toTime.Minute);
                var today = DateTime.Now;
                var employeeAppliedLeave = new EmployeeAppliedLeaveEntity();
                employeeAppliedLeave.EmpId = leave.EmpId;
                employeeAppliedLeave.AppliedLeaveTypeId = leave.AppliedLeaveTypeId;
                employeeAppliedLeave.LeaveFromDate = permissionFromHour;
                employeeAppliedLeave.LeaveToDate = permissionToHour;
                employeeAppliedLeave.Reason = leave.Reason;
                employeeAppliedLeave.LeaveApplied = 0;
                employeeAppliedLeave.IsApproved = 0;
                employeeAppliedLeave.IsDeleted = false;
                employeeAppliedLeave.CreatedDate = DateTime.Now;
                employeeAppliedLeave.CreatedBy = sessionEmployeeId;
                results = await _leaveRepository.CreateLeave(employeeAppliedLeave,companyId);
                return results;
            }
            else
            {
                var employeeAppliedLeave = new EmployeeAppliedLeaveEntity();
                employeeAppliedLeave.EmpId = leave.EmpId;
                employeeAppliedLeave.AppliedLeaveTypeId = leave.AppliedLeaveTypeId;
                employeeAppliedLeave.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate);
                employeeAppliedLeave.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveToDate);
                employeeAppliedLeave.Reason = leave.Reason;
                employeeAppliedLeave.LeaveFilePath = leave.LeaveFilePath;
                employeeAppliedLeave.LeaveName = leave.LeaveName;
                leaveDaysCount = await GetLeaveCount(employeeAppliedLeave.LeaveFromDate, employeeAppliedLeave.LeaveToDate);
                leaveCount = leave.IsHalfDay ? (isHalfDay) : leaveDaysCount;
                employeeAppliedLeave.LeaveApplied = leaveCount;
                employeeAppliedLeave.IsApproved = 0;
                employeeAppliedLeave.IsDeleted = false;
                employeeAppliedLeave.CreatedDate = DateTime.Now;
                employeeAppliedLeave.CreatedBy = sessionEmployeeId;
                Common.Common.WriteServerErrorLog(" CreateLeave : ");

                results = await _leaveRepository.CreateLeave(employeeAppliedLeave,companyId);

            }
            if (results == true)
            {
                var reason = leave.Reason;
                var employee = new List<Employees>();

                var userName = await _employeesRepository.GetEmployeeByIdForLeave(leave.EmpId, companyId);

                var leaveFromDate = string.Empty;
                var leaveToDate = string.Empty;

                if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    leaveFromDate = leave.StrLeaveFromDate; //leave.LeaveFromDate.ToString();
                    leaveToDate = leave.StrLeaveToDate; //leave.LeaveToDate.ToString();
                }
                else
                {
                    leaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate).ToString(Constant.DateFormatMonthHyphen);
                    leaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveToDate).ToString(Constant.DateFormatMonthHyphen);
                }

                var strDay = leaveDaysCount < 2 ? "day" : "days";
                var leavetype = string.Empty;
                if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.CasualLeave)
                {
                    leavetype = Constant.CasualLeave;
                }
                else if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.SickLeave)
                {
                    leavetype = Constant.SickLeave;
                }
                else if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.EarnedLeave)
                {
                    leavetype = Constant.EarnedLeave;
                }
                else if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.MaternityLeave)
                {
                    leavetype = Constant.MaternityLeave;
                }
                else if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    leavetype = Constant.Permission;
                }
                else if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.LOP)
                {
                    leavetype = Constant.LOP;
                }
                else if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                {
                    leavetype = Constant.WorkFromHome;
                }
                else if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.CompensatoryOff)
                {
                    leavetype = Constant.CompensatoryOff;
                }
                if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                {
                    var draftTypeId = (int)EmailDraftType.ApplyWorkFromHome;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var toMails = await GetToMails(userName.EmpId, emailDraftContentEntity.Email,companyId);
                    var bodyContent = EmailBodyContent.SendEmail_Body_CreateLeave(userName, leaveFromDate, leaveToDate, leaveCount, leavetype, strDay, reason, emailDraftContentEntity.DraftBody);
                    await InsertEmailApplyWorkFromHome(userName.OfficeEmail, emailDraftContentEntity, bodyContent, toMails);
                }
                else
                {

                    var draftTypeId = (int)EmailDraftType.ApplyLeave;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var toMails = await GetToMails(userName.EmpId, emailDraftContentEntity.Email,companyId);
                    var bodyContent = EmailBodyContent.SendEmail_Body_CreateLeave(userName, leaveFromDate, leaveToDate, leaveCount, leavetype, strDay, reason, emailDraftContentEntity.DraftBody);
                    await InsertEmailApplyLeave(userName.OfficeEmail, emailDraftContentEntity, bodyContent, toMails);
                }
            }
            return results;
        }

        /// <summary>
        /// Logic to get applyleave email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailApplyLeave(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent,string toMails)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.LeaveRequest;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = toMails;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }


        /// <summary>
               /// Logic to get update leave by particular employees
              /// </summary> 
        /// <param name="leave" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> UpdateLeave(EmployeeLeaveViewModel leave, int sessionEmployeeId, int companyId)
        {
            bool result = false;
            string halfDay = "0.5";
            decimal isHalfDay;
            decimal.TryParse(halfDay, out isHalfDay);
            if (leave.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
            {
                DateTime fromTime = Convert.ToDateTime(leave.PermissionFromTime);
                DateTime toTime = Convert.ToDateTime(leave.PermissionToTime);

                leave.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate);
                leave.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate);

                var permissionFromHour = leave.LeaveFromDate.AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                var permissionToHour = leave.LeaveFromDate.AddHours(toTime.Hour).AddMinutes(toTime.Minute);

                var employeeAppliedLeaveEntity = await _leaveRepository.GetLeaveByAppliedLeaveId(leave.AppliedLeaveId,companyId);
                employeeAppliedLeaveEntity.AppliedLeaveId = leave.AppliedLeaveId;
                employeeAppliedLeaveEntity.EmpId = leave.EmpId;
                employeeAppliedLeaveEntity.AppliedLeaveTypeId = leave.AppliedLeaveTypeId;
                employeeAppliedLeaveEntity.LeaveFromDate = permissionFromHour;
                employeeAppliedLeaveEntity.LeaveToDate = permissionToHour;
                employeeAppliedLeaveEntity.Reason = leave.Reason;
                employeeAppliedLeaveEntity.LeaveFilePath = string.IsNullOrEmpty(leave.LeaveFilePath) ? employeeAppliedLeaveEntity.LeaveFilePath : leave.LeaveFilePath;
                employeeAppliedLeaveEntity.LeaveName = string.IsNullOrEmpty(leave.LeaveName) ? employeeAppliedLeaveEntity.LeaveName : leave.LeaveName;
                employeeAppliedLeaveEntity.LeaveApplied = 0;
                employeeAppliedLeaveEntity.IsApproved = 0;
                employeeAppliedLeaveEntity.IsDeleted = false;
                employeeAppliedLeaveEntity.CreatedDate = employeeAppliedLeaveEntity.CreatedDate;
                employeeAppliedLeaveEntity.CreatedBy = employeeAppliedLeaveEntity.CreatedBy;
                employeeAppliedLeaveEntity.UpdatedDate = DateTime.Now;
                employeeAppliedLeaveEntity.UpdatedBy = sessionEmployeeId;
                result = await _leaveRepository.UpdateLeave(employeeAppliedLeaveEntity);
                return result;
            }
            else
            {
                // var employeeAppliedLeaveEntity = new EmployeeAppliedLeaveEntity();
                var employeeAppliedLeaveEntity = await _leaveRepository.GetLeaveByAppliedLeaveId(leave.AppliedLeaveId,companyId);
                employeeAppliedLeaveEntity.AppliedLeaveId = leave.AppliedLeaveId;
                employeeAppliedLeaveEntity.EmpId = leave.EmpId;
                employeeAppliedLeaveEntity.AppliedLeaveTypeId = leave.AppliedLeaveTypeId;

                employeeAppliedLeaveEntity.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveFromDate);
                employeeAppliedLeaveEntity.LeaveToDate =  DateTimeExtensions.ConvertToNotNullDatetime(leave.StrLeaveToDate);

                employeeAppliedLeaveEntity.Reason = leave.Reason;
                employeeAppliedLeaveEntity.LeaveFilePath = string.IsNullOrEmpty(leave.LeaveFilePath) ? employeeAppliedLeaveEntity.LeaveFilePath : leave.LeaveFilePath;
                employeeAppliedLeaveEntity.LeaveName = string.IsNullOrEmpty(leave.LeaveName) ? employeeAppliedLeaveEntity.LeaveName : leave.LeaveName;
                var leaveDaysCount = await GetLeaveCount(employeeAppliedLeaveEntity.LeaveFromDate, employeeAppliedLeaveEntity.LeaveToDate);
                employeeAppliedLeaveEntity.LeaveApplied = leave.IsHalfDay ? isHalfDay : leaveDaysCount;
                employeeAppliedLeaveEntity.IsApproved = 0;
                employeeAppliedLeaveEntity.IsDeleted = false;
                employeeAppliedLeaveEntity.CreatedDate = employeeAppliedLeaveEntity.CreatedDate;
                employeeAppliedLeaveEntity.CreatedBy = employeeAppliedLeaveEntity.CreatedBy;
                employeeAppliedLeaveEntity.UpdatedDate = DateTime.Now;
                employeeAppliedLeaveEntity.UpdatedBy = sessionEmployeeId;
                result = await _leaveRepository.UpdateLeave(employeeAppliedLeaveEntity);
                return result;
            }
        }

        /// <summary>
        /// Logic to get leavedate count by particular employees
        /// </summary> 
        /// <param name="leaveFromDate" ></param> 
        /// <param name="leaveToDate" ></param>
        public async Task<int> GetLeaveCount(DateTime leaveFromDate, DateTime leaveToDate)
        {
            var daysCount = 0;
            var startDate = leaveFromDate;
            var toDate = leaveToDate;

            var totcount = Convert.ToInt32(toDate.Subtract(startDate).TotalDays) + 1;
            if (startDate != DateTime.MinValue)
            {
                if (totcount >= 4)
                {
                    //for (DateTime date = startDate; date <= toDate; date = date.AddDays(1))
                    //{
                    //    if (startDate.DayOfWeek == DayOfWeek.Friday || startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday || startDate.DayOfWeek == DayOfWeek.Monday)
                    //    {
                    //        daysCount++;
                    //    }

                    //    else if (startDate.DayOfWeek == DayOfWeek.Monday || startDate.DayOfWeek == DayOfWeek.Tuesday || startDate.DayOfWeek == DayOfWeek.Wednesday || startDate.DayOfWeek == DayOfWeek.Thursday || startDate.DayOfWeek == DayOfWeek.Friday || startDate.DayOfWeek != DayOfWeek.Saturday ||
                    //        startDate.DayOfWeek != DayOfWeek.Sunday)
                    //    {
                    //        daysCount++;
                    //    }
                    //    startDate = startDate.AddDays(1);
                    //}
                    return totcount;
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

        /// <summary>
        /// Logic to get delete leave by particular employees
        /// </summary> 
        /// <param name="leave" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> DeleteLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId)
        {
            var result = await _leaveRepository.DeleteLeave(leave, sessionEmployeeId,companyId);
            return result;
        }

        /// <summary>
            /// Logic to get approved leave by particular employees
             /// </summary> 
        /// <param name="leave" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> ApprovedLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId)
        {
            var result = await _leaveRepository.ApprovedLeave(leave, sessionEmployeeId,companyId);
            var toEmails = new List<string>();
            if (result == true)
            {
                var getLeavedeatils = await _leaveRepository.GetLeaveByAppliedLeaveId(leave.AppliedLeaveId, companyId);
                var userName = await _employeesRepository.GetEmployeeById(getLeavedeatils.EmpId, companyId);
                var approverName = await _employeesRepository.GetEmployeeById(sessionEmployeeId, companyId);
                if (userName != null)
                {
                    toEmails.Add(Convert.ToString(userName.OfficeEmail));

                    var reason = getLeavedeatils.Reason;
                    var approveReason = getLeavedeatils.RejectReason;
                    var leaveFromDate = string.Empty;
                    var leaveToDate = string.Empty;

                    if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                    {
                        leaveFromDate = Convert.ToString(getLeavedeatils.LeaveFromDate);
                        leaveToDate = Convert.ToString(getLeavedeatils.LeaveToDate);
                    }
                    else
                    {
                        leaveFromDate = getLeavedeatils.LeaveFromDate.ToString(Constant.DateFormatMonthHyphen);
                        leaveToDate = getLeavedeatils.LeaveToDate.ToString(Constant.DateFormatMonthHyphen);
                    }

                    var leavetype = string.Empty;
                    if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.CasualLeave)
                    {
                        leavetype = Constant.CasualLeave;
                    }
                    else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.SickLeave)
                    {
                        leavetype = Constant.SickLeave;
                    }
                    else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.EarnedLeave)
                    {
                        leavetype = Constant.EarnedLeave;
                    }
                    else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.MaternityLeave)
                    {
                        leavetype = Constant.MaternityLeave;
                    }
                    else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                    {
                        leavetype = Constant.Permission;
                    }
                    else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.LOP)
                    {
                        leavetype = Constant.LOP;
                    }
                    else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                    {
                        leavetype = Constant.WorkFromHome;
                    }
                    else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.CompensatoryOff)
                    {
                        leavetype = Constant.CompensatoryOff;
                    }

                    var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(getLeavedeatils.EmpId,companyId);
                    Common.Common.WriteServerErrorLog(" leave.EmpId : " + leave.EmpId);
                    var name = string.Empty;
                    foreach (var item in reportingPersonEmployeeIds)
                    {
                        var dropdownProjectManager = new EmployeeAppliedLeave();
                        var names = await _employeesRepository.GetEmployeeByname(item.ReportingPersonEmpId, companyId);
                        dropdownProjectManager.splitName = names.FirstName + "  " + names.LastName;
                        name = dropdownProjectManager.splitName;

                    }
                    name = approverName.FirstName + " " + approverName.LastName;
                    var emailEntity = new EmailQueueEntity();
                    emailEntity.ToEmail = string.Join(",", toEmails);
                    if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                    {
                        var draftTypeId = (int)EmailDraftType.AcceptWorkFromHome;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);

                        var bodyContent = EmailBodyContent.SendEmail_Body_ApprovedLeave(userName, leaveFromDate, leaveToDate, reason, approveReason, leavetype, name, emailDraftContentEntity.DraftBody);
                        await InsertEmailAcceptWorkFromHome(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                    }
                    else
                    {
                        var draftTypeId = (int)EmailDraftType.AcceptLeave;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);

                        var bodyContent = EmailBodyContent.SendEmail_Body_ApprovedLeave(userName, leaveFromDate, leaveToDate, reason, approveReason, leavetype, name, emailDraftContentEntity.DraftBody);
                        await InsertEmailAcceptLeave(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Logic to get acceptleave email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailAcceptLeave(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.ToEmail = officeEmail;
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.ApproveLeave;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var approvedemail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }


        /// <summary>
              /// Logic to get rejectleave by particular employees
            /// </summary> 
        /// <param name="leave" ></param> 
        /// <param name="sessionEmployeeId" ></param>        
        public async Task<int> RejectLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId)
        {
            var result = await _leaveRepository.RejectLeave(leave, sessionEmployeeId,companyId);
            var toEmails = new List<string>();
            if (result == 1)
            {
                var getLeavedeatils = await _leaveRepository.GetLeaveByAppliedLeaveId(leave.AppliedLeaveId,companyId);
                var userName = await _employeesRepository.GetEmployeeById(getLeavedeatils.EmpId, companyId);
                var approverName = await _employeesRepository.GetEmployeeById(sessionEmployeeId, companyId);
                toEmails.Add(Convert.ToString(userName.OfficeEmail));
                var reason = getLeavedeatils.Reason;
                var rejectReason = !string.IsNullOrEmpty(getLeavedeatils.RejectReason) ? getLeavedeatils.RejectReason : string.Empty;
                var leaveFromDate = string.Empty;
                var leaveToDate = string.Empty;

                if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    leaveFromDate = getLeavedeatils.LeaveFromDate.ToString();
                    leaveToDate = getLeavedeatils.LeaveToDate.ToString();
                }
                else
                {
                    leaveFromDate = getLeavedeatils.LeaveFromDate.ToString(Constant.DateFormatMonthHyphen);
                    leaveToDate = getLeavedeatils.LeaveToDate.ToString(Constant.DateFormatMonthHyphen);
                }

                var leavetype = string.Empty;
                if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.CasualLeave)
                {
                    leavetype = Constant.CasualLeave;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.SickLeave)
                {
                    leavetype = Constant.SickLeave;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.EarnedLeave)
                {
                    leavetype = Constant.EarnedLeave;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.MaternityLeave)
                {
                    leavetype = Constant.MaternityLeave;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    leavetype = Constant.Permission;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.LOP)
                {
                    leavetype = Constant.LOP;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                {
                    leavetype = Constant.WorkFromHome;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.CompensatoryOff)
                {
                    leavetype = Constant.CompensatoryOff;
                }

                var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(getLeavedeatils.EmpId,companyId);
                Common.Common.WriteServerErrorLog(" leave.EmpId : " + leave.EmpId);
                var name = string.Empty;
                foreach (var item in reportingPersonEmployeeIds)
                {
                    var dropdownProjectManager = new EmployeeAppliedLeave();
                    var names = await _employeesRepository.GetEmployeeByname(item.ReportingPersonEmpId, companyId);
                    dropdownProjectManager.splitName = names.FirstName + "  " + names.LastName;
                    name = dropdownProjectManager.splitName;
                }
                name = approverName.FirstName + "  " + approverName.LastName;   

                if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                {
                    var draftTypeId = (int)EmailDraftType.RejectWorkFromHome;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var emailEntity = new EmailQueueEntity();
                    emailEntity.ToEmail = string.Join(",", toEmails);

                    var bodyContent = EmailBodyContent.SendEmail_Body_RejectLeave(userName, leaveFromDate, leaveToDate, reason, leavetype, rejectReason, name, emailDraftContentEntity.DraftBody);
                    await InsertEmailQueueForRejectWorkFromHome(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                }
                else
                {
                    var draftTypeId = (int)EmailDraftType.RejectLeave;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var emailEntity = new EmailQueueEntity();
                    emailEntity.ToEmail = string.Join(",", toEmails);

                    var bodyContent = EmailBodyContent.SendEmail_Body_RejectLeave(userName, leaveFromDate, leaveToDate, reason, leavetype, rejectReason, name, emailDraftContentEntity.DraftBody);
                    await InsertEmailQueue(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                }


                var subject = string.Empty;
                if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    subject = Constant.RejectPermission;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.LOP)
                {
                    subject = Constant.RejectLOP;
                }
                else if (getLeavedeatils.AppliedLeaveTypeId == (int)LeavetypeStatus.WorkFromHome)
                {
                    subject = Constant.RejectWorkFromHome;
                }
                else
                {
                    subject = Constant.RejectLeave;
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get rejectleave email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>  
        /// <param name="bodyContent" ></param>  
        private async Task InsertEmailQueue(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            //var displayName = "Denied Your Request - Management";
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.RejectLeave;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var approvedemail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
             /// Logic to get the leave detail by particular employees AppliedLeaveId
             /// </summary> 
        /// <param name="AppliedLeaveId" ></param>
        public async Task<EmployeeLeaveViewModel> GetLeaveByAppliedLeaveId(int AppliedLeaveId, int companyId)
        {
            var employee = new EmployeeLeaveViewModel();
            var listLeaveSummary = await _leaveRepository.GetLeaveByAppliedLeaveId(AppliedLeaveId,companyId);
            var employeeAppliedLeave = _mapper.Map<EmployeeAppliedLeave>(listLeaveSummary);

            if (listLeaveSummary.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
            {
                var StartTime = employeeAppliedLeave.LeaveFromDate.ToString(Constant.TimeFormat24HrsHM);
                var EndTime = employeeAppliedLeave.LeaveToDate.ToString(Constant.TimeFormat24HrsHM);
                DateTime startTime = Convert.ToDateTime(StartTime);
                DateTime endTime = Convert.ToDateTime(EndTime);
                employee.PermissionFromTime = startTime.ToString("hh:mm tt");
                employee.PermissionToTime = endTime.ToString("hh:mm tt");
                employee.AppliedLeaveId = employeeAppliedLeave.AppliedLeaveId;
                employee.EmpId = employeeAppliedLeave.EmpId;
                employee.AppliedLeaveTypeId = employeeAppliedLeave.AppliedLeaveTypeId;
                employee.LeaveFromDate = employeeAppliedLeave.LeaveFromDate;
                employee.LeaveToDate = employeeAppliedLeave.LeaveToDate;
                employee.TotalLeave = employeeAppliedLeave.LeaveApplied;
                employee.Reason = employeeAppliedLeave.Reason;
                employee.LeaveName = employeeAppliedLeave.LeaveName;
                employee.IsApproved = employeeAppliedLeave.IsApproved;
                employee.CreatedDate = employeeAppliedLeave.CreatedDate;
                employee.CreatedBy = employeeAppliedLeave.EmpId;
                employee.LeaveFilePath = employeeAppliedLeave.LeaveFilePath;
                employee.splitLeaveName = string.IsNullOrEmpty(employee.LeaveName) ? "" : employee.LeaveName.Substring(employee.LeaveName.LastIndexOf(".") + 1);
                return employee;
            }
            else
            {
                employee.AppliedLeaveId = employeeAppliedLeave.AppliedLeaveId;
                employee.EmpId = employeeAppliedLeave.EmpId;
                employee.AppliedLeaveTypeId = employeeAppliedLeave.AppliedLeaveTypeId;
                employee.LeaveFromDate = employeeAppliedLeave.LeaveFromDate;
                employee.LeaveToDate = employeeAppliedLeave.LeaveToDate;
                employee.TotalLeave = employeeAppliedLeave.LeaveApplied;
                employee.Reason = employeeAppliedLeave.Reason;
                employee.LeaveName = employeeAppliedLeave.LeaveName;
                employee.IsHalfDay = (listLeaveSummary.LeaveApplied != 0 && listLeaveSummary.LeaveApplied < 1) ? true : false;
                employee.IsApproved = employeeAppliedLeave.IsApproved;
                employee.CreatedDate = employeeAppliedLeave.CreatedDate;
                employee.CreatedBy = employeeAppliedLeave.EmpId;
                employee.LeaveFilePath = employeeAppliedLeave.LeaveFilePath;
                employee.splitLeaveName = string.IsNullOrEmpty(employee.LeaveName) ? "" : employee.LeaveName.Substring(employee.LeaveName.LastIndexOf(".") + 1);
                return employee;
            }
        }

        /// <summary>
        ///  Logic to get display view the leave detail  by particular employee leave
        /// </summary>
        /// <param name="appliedLeaveId" >leave</param> 
        public async Task<ViewEmployeeLeave> GetViewLeaveByAppliedLeaveId(int appliedLeaveId, int companyId)
        {
            var viewEmployeeLeave = new ViewEmployeeLeave();
            var leaveSummaryByAppliedId = await _leaveRepository.GetLeaveByAppliedLeaveId(appliedLeaveId, companyId);
            if (leaveSummaryByAppliedId != null)
            {
                if (leaveSummaryByAppliedId.AppliedLeaveTypeId == (int)LeavetypeStatus.Permission)
                {
                    viewEmployeeLeave.FromTime = leaveSummaryByAppliedId.LeaveFromDate.ToString(Constant.TimeFormat24HrsHM);
                    viewEmployeeLeave.ToTime = leaveSummaryByAppliedId.LeaveToDate.ToString(Constant.TimeFormat24HrsHM);
                    viewEmployeeLeave.AppliedLeaveTypeId = leaveSummaryByAppliedId.AppliedLeaveTypeId;
                    viewEmployeeLeave.LeaveTypeName = Convert.ToString((LeavetypeStatus)leaveSummaryByAppliedId.AppliedLeaveTypeId);
                    viewEmployeeLeave.LeaveFromDate = leaveSummaryByAppliedId.LeaveFromDate.ToString(Constant.DateFormatHyphen);
                    viewEmployeeLeave.LeaveToDate = leaveSummaryByAppliedId.LeaveToDate.ToString(Constant.DateFormatHyphen);
                    viewEmployeeLeave.Reason = leaveSummaryByAppliedId.Reason;
                    viewEmployeeLeave.LeaveFileName = string.IsNullOrEmpty(leaveSummaryByAppliedId.LeaveName) ? string.Empty : leaveSummaryByAppliedId.LeaveName;
                    viewEmployeeLeave.RejectReason = string.IsNullOrEmpty(leaveSummaryByAppliedId.RejectReason) ? string.Empty : leaveSummaryByAppliedId.RejectReason;
                    viewEmployeeLeave.Status = Convert.ToString((AppliedLeaveStatus)leaveSummaryByAppliedId.IsApproved);
                    viewEmployeeLeave.IsApprove = leaveSummaryByAppliedId.IsApproved;
                    viewEmployeeLeave.AppliedDayCount = leaveSummaryByAppliedId.LeaveApplied;
                    viewEmployeeLeave.LeaveAppliedDate = leaveSummaryByAppliedId.CreatedDate.ToString(Constant.DateFormatHyphen);
                    return viewEmployeeLeave;
                }
                else
                {
                    viewEmployeeLeave.AppliedLeaveTypeId = leaveSummaryByAppliedId.AppliedLeaveTypeId;
                    viewEmployeeLeave.LeaveTypeName = Convert.ToString((LeavetypeStatus)leaveSummaryByAppliedId.AppliedLeaveTypeId);
                    viewEmployeeLeave.LeaveFromDate = leaveSummaryByAppliedId.LeaveFromDate.ToString(Constant.DateFormatHyphen);
                    viewEmployeeLeave.LeaveToDate = leaveSummaryByAppliedId.LeaveToDate.ToString(Constant.DateFormatHyphen);
                    viewEmployeeLeave.Reason = leaveSummaryByAppliedId.Reason;
                    viewEmployeeLeave.RejectReason = string.IsNullOrEmpty(leaveSummaryByAppliedId.RejectReason) ? string.Empty : leaveSummaryByAppliedId.RejectReason;
                    viewEmployeeLeave.LeaveFileName = string.IsNullOrEmpty(leaveSummaryByAppliedId.LeaveName) ? string.Empty : leaveSummaryByAppliedId.LeaveName;
                    viewEmployeeLeave.Status = Convert.ToString((AppliedLeaveStatus)leaveSummaryByAppliedId.IsApproved);
                    viewEmployeeLeave.IsApprove = leaveSummaryByAppliedId.IsApproved;
                    viewEmployeeLeave.AppliedDayCount = leaveSummaryByAppliedId.LeaveApplied;
                    viewEmployeeLeave.LeaveAppliedDate = leaveSummaryByAppliedId.CreatedDate.ToString(Constant.DateFormatHyphen);
                    return viewEmployeeLeave;
                }
            }
            return viewEmployeeLeave;
        }

        /// <summary>
        /// Logic to get remainig leave list using for reports 
        /// </summary>   
        /// <param name="empId" ></param> 
        public async Task<EmployeeLeaveViewModel> GetAllRemainingLeave(int empId, int companyId)
         {
            try
            {
                var result = new EmployeeLeaveViewModel();
                if (empId == 0)
                {                    
                    var employeeLeave = await GetAllemployeeRemainingLeaves(empId,companyId);
                    result = employeeLeave;
                    return result;
                }
                else
                {                   
                    var employeeLeaves = await GetAllemployeeRemainingLeaves(empId, companyId);
                    result = employeeLeaves;
                    return result;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        /// <summary>
        /// Logic to get remainig leave list using for reports 
        /// </summary>   
        /// <param name="empId" ></param> 
        public async Task<EmployeeLeaveViewModel> GetAllemployeeRemainingLeaves(int empId,int companyId)
        {
            var employeeLeaveViewModel = new EmployeeLeaveViewModel();
            var employeeLeaveViewModels = new List<EmployeeLeaveViewModel>();
            employeeLeaveViewModel.EmployeeTotalLeaveDetails = new List<EmployeeTotalLeaveDetails>();
            var leaveCounts = new List<LeaveCount>();
            if(empId == 0)
            {
                employeeLeaveViewModels = await _leaveRepository.GetEmployeeLeaveViewModel(companyId);
            }
            else
            {
                employeeLeaveViewModels = await _leaveRepository.GetEmployeeLeaveByEmpId(empId,companyId);
            }            

            foreach (var employee in employeeLeaveViewModels)
            {
                foreach (var allLeave in employee.AllLeaveDetail)
                {
                    foreach (var item in employee.LeavesType)
                    {
                        var leaveCount = new LeaveCount();
                        leaveCount.LeaveType = item.LeaveType;

                        if (allLeave != null)
                        {
                            if (leaveCount.LeaveType == Constant.CasualLeave)
                            {
                                leaveCount.TotalLeave = allLeave.CasualLeaveCount;
                            }
                            else if (leaveCount.LeaveType == Constant.SickLeave)
                            {
                                leaveCount.TotalLeave = allLeave.SickLeaveCount;
                            }
                            else if (leaveCount.LeaveType == Constant.EarnedLeave)
                            {
                                leaveCount.TotalLeave = allLeave.EarnedLeaveCount;
                            }
                            else if (leaveCount.LeaveType == Constant.MaternityLeave)
                            {
                                leaveCount.TotalLeave = allLeave.MaternityLeaveCount;
                            }
                            else if (leaveCount.LeaveType == Constant.CompensatoryOff)
                            {
                                leaveCount.TotalLeave = allLeave.CompensatoryOffCount;
                            }
                            else if (leaveCount.LeaveType == Constant.LOP)
                            {
                                leaveCount.TotalLeave = 0.0m;
                            }
                            else if (leaveCount.LeaveType == Constant.WorkFromHome)
                            {
                                leaveCount.TotalLeave = 0.0m;
                            }
                        }
                        else
                        {
                            leaveCount.TotalLeave = 0.0m;
                        }

                        leaveCount.ApprovedLeave = employee.EmployeeAppliedLeave.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved <=1).Sum(x => x.LeaveApplied);
                        var count = Helpers.TotalLeaveCount(leaveCount.TotalLeave, leaveCount.ApprovedLeave);
                        if (count > 0)
                        {
                            leaveCount.RemaingLeave = count;
                            leaveCount.ApprovedLeave = leaveCount.ApprovedLeave;
                        }
                        else
                        {
                            leaveCount.RemaingLeave = 0;
                            leaveCount.ApprovedLeave = leaveCount.ApprovedLeave;
                        }
                        var appliedAndApproveLeave = employee.EmployeeAppliedLeave.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved <= 1).Sum(x => x.LeaveApplied);
                        leaveCount.SumOfAppliedLeaveAndApprovedLeave = (item.LeaveTypeId == 6) ? appliedAndApproveLeave : Helpers.TotalLeaveCount(leaveCount.TotalLeave, appliedAndApproveLeave);						
                        leaveCounts.Add(leaveCount);
                        leaveCount.EmpId = employee.EmpId;
                        leaveCount.EmployeeName = employee.EmployeeName;
                        leaveCount.UserName = employee.UserName;

                    }
                }

                var casualleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.CasualLeave && x.EmpId == employee.EmpId).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
                var sickleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.SickLeave && x.EmpId == employee.EmpId).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
                var earnedleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.EarnedLeave && x.EmpId == employee.EmpId).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
                var maternityleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.MaternityLeave && x.EmpId == employee.EmpId).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
                var compensatoryOff = leaveCounts.Where(x => x.LeaveType == Constant.CompensatoryOff && x.EmpId == employee.EmpId).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
                var lop = leaveCounts.Where(x => x.LeaveType == Constant.LOP && x.EmpId == employee.EmpId).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
                var leavecount = (casualleaveremaining + sickleaveremaining + earnedleaveremaining + maternityleaveremaining + compensatoryOff);
                var totalremaining = leavecount;
                var totals = leavecount;

                employeeLeaveViewModel.LeaveCounts = leaveCounts;                
                employeeLeaveViewModel.CasualLeaveRemaining = casualleaveremaining;
                employeeLeaveViewModel.SickLeaveRemaining = sickleaveremaining;
                employeeLeaveViewModel.EarnedLeaveRemaining = earnedleaveremaining;
                employeeLeaveViewModel.MaternityLeaveRemaining = maternityleaveremaining;
                employeeLeaveViewModel.CompensatoryOffRemaining = compensatoryOff;
                employeeLeaveViewModel.ApprovedLOP = lop;
                employeeLeaveViewModel.ApprovedLeave = leaveCounts.Where(x => x.EmpId == employee.EmpId).Sum(x => x.ApprovedLeave);

                var counts = leaveCounts.FirstOrDefault(x => x.EmpId == employee.EmpId);
                if (counts != null)
                {
                    employeeLeaveViewModel.EmployeeTotalLeaveDetails.Add(new EmployeeTotalLeaveDetails()
                    {
                        EmpId = counts.EmpId,
                        UserName = counts.UserName,
                        EmployeeName = counts.EmployeeName,
                        CasualLeaveRemaining = casualleaveremaining,
                        SickLeaveRemaining = sickleaveremaining,
                        EarnedLeaveRemaining = earnedleaveremaining,
                        MaternityLeaveRemaining = maternityleaveremaining,
                        CompensatoryOffRemaining = compensatoryOff,
                        ApprovedLOP= lop,
                        ApprovedLeave = employeeLeaveViewModel.ApprovedLeave,
                        RemaingLeave = totals,

                    });
                }
            }          
            return employeeLeaveViewModel;
        }


        //CompensatoryRequest

        /// <summary>
        /// Logic to get CompensatoryOff list by particular employees
        /// </summary>   
        /// <param name="empId" ></param> 
        public async Task<List<CompensatoryRequestViewModel>> GetAllCompensatoryOffRequests(int empId, int companyId)
        {
            var result = new List<CompensatoryRequestViewModel>();
            var pager = new SysDataTablePager();
            if (empId == 0)
            {
                var employeesComOffRequest = await _leaveRepository.GetAllEmployeeComOff(companyId);
                result = employeesComOffRequest;


                return result;

            }
            else
            {
                var employeeComOffRequest = await _leaveRepository.GetReportingEmployeesComOff(empId,companyId);
                result = employeeComOffRequest;
                return result;
            }
        }


        /// <summary>
        /// Logic to get CompensatoryOff list by particular  employees using filter
        /// </summary> 
        /// <param name="empId,columnDirection,columnName,pager" ></param> 
      
        public async Task<CompensatoryRequestViewModel> GetAllCompensatoryOffRequestsFilter(SysDataTablePager pager, int empId, string columnDirection, string columnName, int companyId)
        {
            var result = new CompensatoryRequestViewModel();
            result. employeeCompensatoryFilters = await _leaveRepository.GetAllEmployeeCompenSatoryDetails(pager, empId, columnDirection, columnName,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get CompensatoryOff list by particular  employeesCount using filter
        /// </summary> 
        /// <param name="empId" ></param> 
        /// <param name="pager" ></param> 
        public async Task<int> GetAllCompensatoryOffRequestsFilterCount(SysDataTablePager pager, int empId, int companyId)
        {
            var empcount = await _leaveRepository.GetAllEmployeeCompenSatoryDetailsCount(pager, empId,companyId);
            return empcount;
        }

        /// <summary>
        /// Logic to get CompensatoryOff list 
        /// </summary>
        /// <param name="compensatoryRequestsEntities" ></param>
        //public async Task<List<CompensatoryRequestViewModel>> GetAllCompensatoryRequests(List<CompensatoryRequestsEntity> compensatoryRequestsEntities)
        //{
        //    var result = new List<CompensatoryRequestViewModel>();
        //    var listEmployee = await _employeesRepository.GetAllEmployees();
        //    var listLeave = await _employeeInformationService.GetLeaveTypeData();
        //    var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile();
        //    var count = 1;
        //    foreach (var item in compensatoryRequestsEntities)
        //    {
        //        count = count == 6 ? 1 : count;
        //        var today = DateTime.Now;
        //        var getEmployee = listEmployee.FirstOrDefault(e => e.EmpId == item.EmpId);
        //        var employeeProfileImage = allEmployeeProfiles.FirstOrDefault(e => e.EmpId == item.EmpId);
        //        result.Add(new CompensatoryRequestViewModel()
        //        {
        //            CompensatoryId = item.CompensatoryId == 0 ? 0 : item.CompensatoryId,
        //            CompanyId = item.CompanyId,
        //            EmpId = item.EmpId,
        //            IsApproved = item.IsApproved,
        //            WorkedDate = item.WorkedDate,
        //            Remark = item.Remark,
        //            DayCount = item.DayCount,
        //            Reason = item.Reason,
        //            EmployeeUserName = getEmployee != null ? getEmployee.UserName : string.Empty,
        //            EmployeeName = getEmployee == null ? string.Empty : getEmployee?.FirstName + "" + getEmployee?.LastName,
        //            EmployeeStatus = getEmployee == null ? false : getEmployee.IsDeleted,
        //            EmployeeProfileImage = employeeProfileImage != null ? employeeProfileImage.ProfileName : string.Empty,
        //            ClassName = Common.Common.GetClassNameForLeaveDashboard(count)
        //        });
        //        count++;
        //    }
        //    result.OrderByDescending(x => x.WorkedDate).ToList();
        //    return result;
        //}

        /// <summary>
        /// Logic to get the create and update compensatoryoff detail by particular employees AppliedLeaveId
        /// </summary> 
        /// <param name="compensatoryRequest" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> CreateCompensatory(CompensatoryRequest compensatoryRequest, int sessionEmployeeId)
        {
            var result = false;

            if (compensatoryRequest != null)
            {
                compensatoryRequest.WorkedDate = DateTimeExtensions.ConvertToNotNullDatetime(compensatoryRequest.StrWorkedDate);
                var compensatoryRequestsEntity = new CompensatoryRequestsEntity();
                if (compensatoryRequest.CompensatoryId == 0)
                {
                    compensatoryRequestsEntity.EmpId = compensatoryRequest.EmpId;
                    compensatoryRequestsEntity.CompanyId = compensatoryRequest.CompanyId;
                    compensatoryRequestsEntity.WorkedDate = compensatoryRequest.WorkedDate;
                    compensatoryRequestsEntity.Remark = compensatoryRequest.Remark;
                    compensatoryRequestsEntity.IsApproved = 0;
                    compensatoryRequestsEntity.IsDeleted = false;
                    compensatoryRequestsEntity.DayCount = 0;
                    compensatoryRequestsEntity.CreatedBy = sessionEmployeeId;
                    compensatoryRequestsEntity.CreatedDate = DateTime.Now;
                    result = await _leaveRepository.CreateCompensatory(compensatoryRequestsEntity, compensatoryRequest.CompensatoryId);
                    if (result == true)
                    {
                        var remark = compensatoryRequest.Remark;
                        var employee = new List<Employees>();
                        var toEmails = new List<string>();
                        var userName = await _employeesRepository.GetEmployeeByIdForLeave(compensatoryRequest.EmpId,compensatoryRequest.CompanyId);
                        if (userName != null)
                        {
                            var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(compensatoryRequest.EmpId, compensatoryRequest.CompanyId);
                            foreach (var item in reportingPersonEmployeeIds)
                            {
                                var email = await _leaveRepository.GetEmployeeEmailByEmpIdForLeave(item.ReportingPersonEmpId);
                                toEmails.Add(email);
                                Common.Common.WriteServerErrorLog(" email : " + email);
                            }
                            var workedDate = string.Empty;
                            workedDate = compensatoryRequest.WorkedDate.ToString(Constant.DateFormatMonthHyphen);
                            var leavetype = string.Empty;
                            leavetype = Constant.CompensatoryOffRequest;
                            var draftTypeId = (int)EmailDraftType.ApplyCompensatoryOffRequest;
                            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,compensatoryRequest.CompanyId);
                            var bodyContent = EmailBodyContent.SendEmail_Body_CreateCompensatoryOffRequest(userName, workedDate, leavetype, remark, emailDraftContentEntity.DraftBody);
                            await InsertEmailApplyCompensatoryOffRequest(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                        }

                    }
                }
                else
                {

                    var compensatoryEntity = await _leaveRepository.GetCompensatoryRequestByCompensatoryId(compensatoryRequest.CompensatoryId);

                    compensatoryEntity.EmpId = compensatoryRequest.EmpId;
                    compensatoryEntity.WorkedDate = compensatoryRequest.WorkedDate;
                    compensatoryEntity.Remark = compensatoryRequest.Remark;
                    compensatoryEntity.UpdatedBy = sessionEmployeeId;
                    compensatoryEntity.UpdatedDate = DateTime.Now;
                    result = await _leaveRepository.CreateCompensatory(compensatoryEntity, compensatoryRequest.CompensatoryId);
                }
                return result;
            }
            return result;
        }

        /// <summary>
        /// Logic to get apply compensatoryoff email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
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
        /// Logic to get reject compensatoryoff by particular employees
        /// </summary> 
        /// <param name="compensatoryRequest" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> RejectCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId)
        {
            var result = await _leaveRepository.RejectCompensatoryOff(compensatoryRequest, sessionEmployeeId);
            var toEmails = new List<string>();
            if (result == 1)
            {
                var getLeavedeatils = await _leaveRepository.GetCompensatoryOffRequestByCompensatoryId(compensatoryRequest.CompensatoryId, compensatoryRequest.CompanyId);
                var userName = await _employeesRepository.GetEmployeeById(getLeavedeatils.EmpId,compensatoryRequest.CompanyId);
                if (userName != null)
                {
                    toEmails.Add(Convert.ToString(userName.OfficeEmail));

                    var remark = getLeavedeatils.Remark;
                    var rejectReason = getLeavedeatils.Reason;
                    var leaveFromDate = string.Empty;
                    var leaveToDate = string.Empty;


                    leaveFromDate = getLeavedeatils.WorkedDate.ToString(Constant.DateFormatMonthHyphen);

                    var leavetype = string.Empty;

                    leavetype = Constant.CompensatoryOffRequest;


                    var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(getLeavedeatils.EmpId,compensatoryRequest.CompanyId);
                    var name = string.Empty;
                    foreach (var item in reportingPersonEmployeeIds)
                    {
                        var dropdownProjectManager = new EmployeeAppliedLeave();
                        var names = await _employeesRepository.GetEmployeeByname(item.ReportingPersonEmpId, compensatoryRequest.CompanyId);
                        dropdownProjectManager.splitName = names.FirstName + "  " + names.LastName;
                        name = dropdownProjectManager.splitName;

                    }

                    var emailEntity = new EmailQueueEntity();
                    emailEntity.ToEmail = string.Join(",", toEmails);

                    var draftTypeId = (int)EmailDraftType.RejectCompensatoryOffRequest;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,compensatoryRequest.CompanyId);

                    var bodyContent = EmailBodyContent.SendEmail_Body_RejectCompensatoryOffRequest(userName, leaveFromDate, remark, rejectReason, leavetype, name, emailDraftContentEntity.DraftBody);
                    await InsertEmailRejectCompensatoryOffRequest(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get reject compensatoryoff email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailRejectCompensatoryOffRequest(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.ToEmail = officeEmail;
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.RejectCompensatoryOffRequest;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var approvedemail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
        /// Logic to get approved compensatoryoff by particular employees
        /// </summary> 
        /// <param name="compensatoryRequest" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> ApprovedCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId)
        {
            var result = await _leaveRepository.ApprovedCompensatoryOff(compensatoryRequest, sessionEmployeeId);
            if (result == true)
            {
                var allLeaveDetails = new AllLeaveDetailsEntity();
                var getEmployeeLeaveCount = await _leaveRepository.GetCompensatoryOffCountByEmpId(compensatoryRequest.EmpId,compensatoryRequest.CompanyId);
                var totalCount = getEmployeeLeaveCount.Sum(x => x.DayCount);
                var getEmployeeLeaveCounts = await _leaveRepository.GetAllLeaveDetails(compensatoryRequest.EmpId,compensatoryRequest.CompanyId);
                var compensatoryOff = getEmployeeLeaveCounts.CompensatoryOffCount;
                getEmployeeLeaveCounts.AllLeaveDetailId = getEmployeeLeaveCounts.AllLeaveDetailId;
                getEmployeeLeaveCounts.CompensatoryOffCount = totalCount;
                var updateLeave = await _leaveRepository.UpdateAllLeaveDeatils(getEmployeeLeaveCounts);
            }

            var toEmails = new List<string>();
            if (result == true)
            {
                var getLeavedeatils = await _leaveRepository.GetCompensatoryOffRequestByCompensatoryId(compensatoryRequest.CompensatoryId, compensatoryRequest.CompanyId);
                var userName = await _employeesRepository.GetEmployeeById(getLeavedeatils.EmpId,compensatoryRequest.CompanyId);
                if (userName != null)
                {
                    toEmails.Add(Convert.ToString(userName.OfficeEmail));
                    var remark = getLeavedeatils.Remark;
                    var approveReason = getLeavedeatils.Reason;
                    var leaveFromDate = string.Empty;
                    leaveFromDate = getLeavedeatils.WorkedDate.ToString(Constant.DateFormatMonthHyphen);
                    var leavetype = string.Empty;
                    leavetype = Constant.CompensatoryOffRequest;
                    var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(getLeavedeatils.EmpId,compensatoryRequest.CompanyId);
                    var name = string.Empty;
                    foreach (var item in reportingPersonEmployeeIds)
                    {
                        var dropdownProjectManager = new EmployeeAppliedLeave();
                        var names = await _employeesRepository.GetEmployeeByname(item.ReportingPersonEmpId,compensatoryRequest.CompanyId);
                        dropdownProjectManager.splitName = names.FirstName + "  " + names.LastName;
                        name = dropdownProjectManager.splitName;

                    }

                    var emailEntity = new EmailQueueEntity();
                    emailEntity.ToEmail = string.Join(",", toEmails);

                    var draftTypeId = (int)EmailDraftType.AcceptCompensatoryOffRequest;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,compensatoryRequest.CompanyId);

                    var bodyContent = EmailBodyContent.SendEmail_Body_ApprovedCompensatoryOffRequest(userName, leaveFromDate, remark, approveReason, leavetype, name, emailDraftContentEntity.DraftBody);
                    await InsertEmailAcceptCompensatoryOffRequest(userName.OfficeEmail, emailDraftContentEntity, bodyContent);
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get accept compensatoryoff email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailAcceptCompensatoryOffRequest(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.ToEmail = officeEmail;
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.ApproveCompensatoryOffRequest;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var approvedemail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
        ///  Logic to get display view the compensatoryoff detail  by particular employee compensatoryoff
        /// </summary>
        /// <param name="compensatoryId" ></param>
        public async Task<ViewCompensatoryOffRequest> GetViewCompensatoryOffRequestByCompensatoryId(int compensatoryId, int companyId)
        {
            var viewCompensatoryOffRequest = new ViewCompensatoryOffRequest();
            var compensatoryEntity = await _leaveRepository.GetCompensatoryOffRequestByCompensatoryId(compensatoryId,companyId);
            if (compensatoryEntity != null)
            {
                var employeeEntity = await _employeesRepository.GetEmployeeById(compensatoryEntity.EmpId, companyId);
                viewCompensatoryOffRequest.CompensatoryId = compensatoryEntity.CompensatoryId;
                viewCompensatoryOffRequest.EmpId = compensatoryEntity.EmpId;
                viewCompensatoryOffRequest.EmployeeName = employeeEntity == null ? "" : employeeEntity.FirstName + " " + employeeEntity.LastName;
                viewCompensatoryOffRequest.EmployeeUserName = employeeEntity == null ? "" : employeeEntity.UserName;
                viewCompensatoryOffRequest.Reason = string.IsNullOrEmpty(compensatoryEntity.Reason) ? string.Empty : compensatoryEntity.Reason;
                viewCompensatoryOffRequest.Remark = string.IsNullOrEmpty(compensatoryEntity.Remark) ? string.Empty : compensatoryEntity.Remark;
                viewCompensatoryOffRequest.WorkedDate = string.IsNullOrEmpty(compensatoryEntity.WorkedDate.ToString(Constant.DateFormat)) ? string.Empty : compensatoryEntity.WorkedDate.ToString(Constant.DateFormat);
                viewCompensatoryOffRequest.IsApproved = compensatoryEntity.IsApproved;
                viewCompensatoryOffRequest.DayCount = compensatoryEntity.DayCount;
                return viewCompensatoryOffRequest;
            }
            return viewCompensatoryOffRequest;
        }

        // Holidays

        /// <summary>
        /// Logic to get the leave detail by particular employees AppliedLeaveId
        /// </summary> 
        /// <param name="AppliedLeaveId" ></param>
        public async Task<EmployeeHolidaysViewModel> GetAllEmployeeHolidays(int year, int companyId)
        {
            var holidays = new EmployeeHolidaysViewModel();
            holidays = await _leaveRepository.GetAllEmployeesHolidays(year,companyId);
            return holidays;
        }

        /// <summary>
        /// Logic to get the create holiday detail 
        /// </summary> 
        /// <param name="employeeHolidays" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> CreateHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId, int companyId)
        {
            var result = false;
            if (employeeHolidays != null)
            {
               
                var holidays = new EmployeeHolidaysEntity();
                holidays.Title = employeeHolidays.Title;
                holidays.HolidayDate = DateTimeExtensions.ConvertToNotNullDatetime(employeeHolidays.HolidayDates);               
                holidays.CreatedBy = sessionEmployeeId;
                holidays.CreatedDate = DateTime.Now;
                result = await _leaveRepository.CreateHoliday(holidays, companyId);
                return result;
            }
            return result;
        }

        /// <summary>
        /// Logic to get the update holiday detail 
        /// </summary> 
        /// <param name="employeeHolidaysViewModel" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> UpdateHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId)
        {
            bool result = false;           
            if (employeeHolidays != null)
            {
                var employeeHolidaysEntity = new EmployeeHolidaysEntity();
                employeeHolidaysEntity.HolidayId = employeeHolidays.HolidayId;
                employeeHolidaysEntity.Title = employeeHolidays.Title;
                employeeHolidaysEntity.HolidayDate = employeeHolidays.HolidayDate;
                employeeHolidaysEntity.UpdatedBy = sessionEmployeeId;
                employeeHolidaysEntity.UpdatedDate = DateTime.Now;
                result = await _leaveRepository.UpdateHoliday(employeeHolidaysEntity);
                return result;
            }
            return result;
        }

        /// <summary>
        /// Logic to get the delete holiday detail 
        /// </summary> 
        /// <param name="employeeHolidays" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> DeleteHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId, int companyId)
        {
            var result = await _leaveRepository.DeleteHoliday(employeeHolidays, sessionEmployeeId,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get the holidaydate detail 
        /// </summary> 
        /// <param name="holidayDate" ></param>        
        public async Task<int> GetHolidayDate(string holidayDate, int companyId)
        {
            var totalModuleNameCount = await _leaveRepository.GetHolidayDate(holidayDate,companyId);
            return totalModuleNameCount;
        }

        /// <summary>
        /// Logic to get the holidaydate detail 
        /// </summary> 
        /// <param name="holidayDate" ></param> 
        /// <param name="holidayid" ></param> 
        public async Task<int> GetHolidayDatesId(string holidayDate,int holidayid, int companyId)
        { 
            var totalModuleNameCount = await _leaveRepository.GetHolidayDatesId(holidayDate, holidayid,companyId);
            if (totalModuleNameCount == 0)
            {
                var exist = await _leaveRepository.GetHolidayDate(holidayDate,companyId);
                return exist == 0 ? 1 : 0;
            }
            
            return totalModuleNameCount;
        }
               

        //WorkFromHome

        /// <summary>
        /// Logic to get reject workfromhome email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>  
        /// <param name="bodyContent" ></param> 
        private async Task InsertEmailQueueForRejectWorkFromHome(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            //var displayName = "Denied Your Request - Management";
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.RejectWorkFromHome;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var approvedemail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
        /// Logic to get acceptworkfromhome email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailAcceptWorkFromHome(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.ToEmail = officeEmail;
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.ApproveWorkFromHome;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var approvedemail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
        /// Logic to get apply workfromhome email
        /// </summary> 
        /// <param name="officeEmail" ></param> 
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailApplyWorkFromHome(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, string toMails)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.WorkFromHomeRequest;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = toMails;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        public async Task<string> GetToMails(int empId , string email, int companyId)
        {
            var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(empId,companyId);
            StringBuilder sb = new StringBuilder();
            sb.Append(email);
            foreach (var item in reportingPersonEmployeeIds)
            {
                var reportingPersonEmail = await _leaveRepository.GetEmployeeEmailByEmpIdForLeave(item.ReportingPersonEmpId);
                sb.Append(",");
                sb.Append(reportingPersonEmail);

            }
           string toMails = sb.ToString();
            return toMails;
        }


        /// <summary>
              /// Logic to get Allleavedetails count of all employees and reporting persons 
              /// </summary> 
        /// <param name="empId, pager" ></param>
        public async Task<int> GetAllLeaveSummaryCount(SysDataTablePager pager, int empId, int companyId)
        {
            return await _leaveRepository.GetAllLeaveSummaryCount(pager, empId,companyId);
        }

        /// <summary>
              /// Logic to get Allleavedetails count of employees
              /// </summary> 
        /// <param name="empId, pager" ></param>
        public async Task<int> GetEmployeesLeaveDetailsCount(SysDataTablePager pager, int empId, int companyId)
        {
            return await _leaveRepository.GetEmployeesLeaveDetailsCount(pager, empId,companyId);
        }
    }
}
