using AutoMapper;
using ClosedXML.Excel;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.AttendanceViewModel;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeInformations.Business.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IDashboardService _dashboardService;
        public AttendanceService(IAttendanceRepository attendanceRepository, IEmployeesRepository employeesRepository, ITimeSheetRepository timeSheetRepository, IReportRepository reportRepository, IMapper mapper, IEmailDraftRepository emailDraftRepository,
            ICompanyRepository companyRepository, IConfiguration config, ILeaveRepository leaveRepository, IDashboardRepository dashboardRepository, IDashboardService dashboardService)
        {
            _attendanceRepository = attendanceRepository;
            _employeesRepository = employeesRepository;
            _timeSheetRepository = timeSheetRepository;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _emailDraftRepository = emailDraftRepository;
            _companyRepository = companyRepository;
            _config = config;
            _leaveRepository = leaveRepository;
            _dashboardRepository = dashboardRepository;
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Logic to get the attendace detail by particular employees
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        public async Task<AttendaceListViewModels> GetWorkingHourListForAll(int companyId)
        {
            var attendaceListViewModels = new AttendaceListViewModels();
            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            attendaceListViewModels.EmployeeStatus = 1;

            var startDate = DateTime.Now.ToString(Constant.DateFormat);
            var dFrom = string.IsNullOrEmpty(startDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(startDate).ToString(Constant.DateFormat);
            var dTo = string.IsNullOrEmpty(startDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(startDate).ToString(Constant.DateFormat);
            var tFrom = "";
            var tTo = "";
            var companyid = Convert.ToString(companyId);

            attendaceListViewModels.StrStartDate = dFrom;
            attendaceListViewModels.StrEndDate = dTo;

            var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat));
            var status = Constant.ZeroStr;
            var empId = Constant.ZeroStr;
            var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyid);
            var employees = await _employeesRepository.GetAllEmployees(companyId);
            var timeSheets = await _timeSheetRepository.GetAllTimeSheetByCurrentDate(flterDate,companyId);
            attendanceReportDateModels = attendanceReportDateModels.ToList();
            foreach (var item in employees)
            {
                var attendacelistViewModel = new AttendaceListViewModel();
                if (item.EsslId > 0)
                {
                    var getEmployeeRecord = attendanceReportDateModels.Where(x => x.EmployeeId == Convert.ToString(item.EsslId) && x.LogDate == DateTime.Now.Date.ToString("yyyy-MM-dd")).ToList();
                    var listOfRec = getEmployeeRecord.TakeLast(1);
                    var lastRecord = listOfRec.FirstOrDefault(x => x.Direction == Constant.EntryTypeIn);
                    var attendance = new AttendanceReportDateModel();
                    if (lastRecord != null)
                    {
                        foreach (var record in listOfRec)
                        {
                            getEmployeeRecord.Add(new AttendanceReportDateModel()
                            {
                                Id = 0,
                                EmployeeId = record.EmployeeId,
                                LogDate = DateTime.Now.ToString("yyyy-MM-dd"),
                                LogDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                Direction = Constant.EntryTypeOut,
                            });
                        }
                    }

                    var stringToNum = Convert.ToString(item.EsslId);
                    var tableEmployeeId = Int32.Parse(stringToNum);
                    var firstInTime = getEmployeeRecord.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == flterDate.Date && x.Direction == Constant.EntryTypeIn);
                    var lastInTime = getEmployeeRecord.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == flterDate.Date && x.Direction == Constant.EntryTypeOut);
                    var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                    var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";

                    if (!string.IsNullOrEmpty(firstLoginDate) /*&& !string.IsNullOrEmpty(lastLoginDate)*/)
                    {
                        DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                        DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                        string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                        dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                        var breakHours = GetBreakHoursForBreak(stringToNum, getEmployeeRecord, flterDate);
                        var burningHours = GetTimeSheetWorkHours(item.EmpId, timeSheets);
                        var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                        attendacelistViewModel.EmployeeId = item.EmpId;
                        attendacelistViewModel.UserName = item.UserName;
                        attendacelistViewModel.EmployeeName = item.FirstName + " " + item.LastName;
                        attendacelistViewModel.Status = item.IsDeleted;
                        attendacelistViewModel.Date = flterDate.Date.ToString(Constant.DateFormat);
                        attendacelistViewModel.TotalHours = dt;
                        attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
                        attendacelistViewModel.InsideOffice = insideoffice;
                        attendacelistViewModel.BurningHours = burningHours;
                        attendacelistViewModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                        attendacelistViewModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                        attendaceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
                    }
                }
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to get the attendace detail by particular employees
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        public async Task<AttendaceListViewModels> GetWorkingHourForAdmin(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                var attendaceListViewModels = new AttendaceListViewModels();
                attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
                attendaceListViewModels.AttendanceStatuses = new List<AttendanceStatus>();
                var employees = await _employeesRepository.GetAllEmployees(attendaceListViewModel.CompanyId);
                var isActiveEmployees = new List<EmployeesEntity>();
                attendaceListViewModels.Dates = new List<int>();
                attendaceListViewModels.ListOfDate = new List<ListOfDate>();
                var empId = "";
                var StartDate = "";
                var EndDate = "";
                var status = "";
                var month = 0;
                var year = 0;
                if (attendaceListViewModel.Month == 0)
                {
                    empId = Constant.ZeroStr;
                    month = DateTime.Now.Month;
                    year = DateTime.Now.Year;

                    if (month == 1 && year == DateTime.Now.Year)
                    {
                        month = DateTime.Now.AddMonths(-1).Month + 1;
                        year = DateTime.Now.AddYears(-1).Year;
                    }

                    var firstDayOfMonth = new DateTime(year, month - 1, 1);
                    var lastDayOfMonth = new DateTime(year, month - 1, DateTime.DaysInMonth(year, month - 1));
                    EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
                    StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
                    status = Constant.ZeroStr;
                    isActiveEmployees = employees.Where(x => !x.IsDeleted).ToList();
                }
                else if (attendaceListViewModel.Month != 0 && attendaceListViewModel.EmployeeId > 0)
                {
                    var employeeStatus = attendaceListViewModel.EmployeeStatus == 0 ? false : true;
                    var getemployee = employees.Where(x => x.EmpId == attendaceListViewModel.EmployeeId && x.IsDeleted == employeeStatus).FirstOrDefault();
                    if (getemployee != null)
                    {
                        empId = Convert.ToString(getemployee.EsslId);
                        StartDate = attendaceListViewModel.StartDate;
                        EndDate = attendaceListViewModel.EndDate;
                        status = attendaceListViewModel.EmployeeStatus.ToString();
                    }
                }
                else
                {
                    empId = Constant.ZeroStr;
                    StartDate = attendaceListViewModel.StartDate;
                    EndDate = attendaceListViewModel.EndDate;
                    status = attendaceListViewModel.EmployeeStatus.ToString();
                    var employeeStatus = attendaceListViewModel.EmployeeStatus == 0 ? false : true;
                    isActiveEmployees = employees.Where(x => x.IsDeleted == employeeStatus).ToList();
                }
                if (empId != "")
                {
                    var AttendaceListViewModel = new List<AttendaceListViewModel>();
                    var dFrom = string.IsNullOrEmpty(StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(StartDate).ToString(Constant.DateFormat);
                    var dTo = string.IsNullOrEmpty(EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(EndDate).ToString(Constant.DateFormat);
                    var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
                    var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
                    var companyId = Convert.ToString(attendaceListViewModel.CompanyId);
                    var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
                    var flterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
                    var holiday = false;
                    var dateOfHoliday = new DateTime();
                    var getAllHolidays = await _leaveRepository.GetAllEmployeeHolidaysForYear(flterDate.Month, flterDate.Year,attendaceListViewModel.CompanyId);

                    List<DateTime> allDates = new List<DateTime>();
                    for (DateTime date = flterDate; date <= flterEndDate; date = date.AddDays(1))

                        if (date != DateTime.MinValue)
                        {

                            var getHolidayDate = getAllHolidays.Where(x => x.HolidayDate.Date == date.Date).Select(y => y.HolidayDate.Date).FirstOrDefault();
                            dateOfHoliday = getHolidayDate == null ? DateTime.MinValue : getHolidayDate;

                            allDates.Add(date);
                            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || dateOfHoliday != DateTime.MinValue && dateOfHoliday.DayOfWeek != DayOfWeek.Saturday || dateOfHoliday != DateTime.MinValue && dateOfHoliday.DayOfWeek != DayOfWeek.Sunday)
                            {
                                holiday = true;
                            }
                            else
                            {
                                holiday = false;
                            }

                            attendaceListViewModels.ListOfDate.Add(new ListOfDate()
                            {
                                Dates = Convert.ToInt32(date.ToString("dd")),
                                Holidays = holiday,
                            });
                        }
                    //used common method for get attandance data
                    var attendanceReportDateModelss = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);

                    if (empId == Constant.ZeroStr && attendaceListViewModel.Month == 0)
                    {
                        //used common method for get list of attendance status
                        var getData = await GetAttendanceStatus(empId, isActiveEmployees, allDates, attendanceReportDateModelss, getAllHolidays, attendaceListViewModel.CompanyId);
                        attendaceListViewModels.AttendanceStatuses = getData;
                        attendaceListViewModels.Month = month - 1;
                        attendaceListViewModels.Year = year;
                        attendaceListViewModels.EmployeeStatus = 1;
                    }
                    else if (empId == Constant.ZeroStr && status == Constant.ZeroStr)
                    {
                        //used common method for get list of attendance status
                        var getData = await GetAttendanceStatus(empId, isActiveEmployees, allDates, attendanceReportDateModelss, getAllHolidays, attendaceListViewModel.CompanyId);
                        attendaceListViewModels.AttendanceStatuses = getData;
                    }
                    else if (empId == Constant.ZeroStr && status != Constant.ZeroStr)
                    {
                        //used common method for get list of attendance status
                        var getData = await GetAttendanceStatus(empId, isActiveEmployees, allDates, attendanceReportDateModelss, getAllHolidays, attendaceListViewModel.CompanyId);
                        attendaceListViewModels.AttendanceStatuses = getData;

                    }
                    else
                    {
                        var getEmployee = employees.Where(x => x.EmpId == attendaceListViewModel.EmployeeId).ToList();
                        //used common method for get list of attendance status
                        var getData = await GetAttendanceStatus(empId, getEmployee, allDates, attendanceReportDateModelss, getAllHolidays, attendaceListViewModel.CompanyId);
                        attendaceListViewModels.AttendanceStatuses = getData;
                    }
                }
                return attendaceListViewModels;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// Logic to Get Working Hour For particular employees
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        public async Task<AttendaceListViewModels> GetWorkingHourForEmployee(AttendaceListViewModel attendaceListViewModel)
        {
            var attendaceListViewModels = new AttendaceListViewModels();
            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            attendaceListViewModels.AttendanceStatuses = new List<AttendanceStatus>();
            var employees = await _employeesRepository.GetAllEmployees(attendaceListViewModel.CompanyId);
            var isActiveEmployees = new List<EmployeesEntity>();
            attendaceListViewModels.Dates = new List<int>();
            attendaceListViewModels.ListOfDate = new List<ListOfDate>();
            var empId = "";
            var StartDate = "";
            var EndDate = "";
            var status = "";
            var month = 0;
            var year = 0;
            if (attendaceListViewModel.Month == 0)
            {
                empId = Constant.ZeroStr;
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
                var firstDayOfMonth = new DateTime(year, month, 1);
                var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
                StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
                status = Constant.ZeroStr;
                //isActiveEmployees = employees.Where(x => !x.IsDeleted).ToList();

                var listOfEmpIds = new List<int>();
                listOfEmpIds.Add(attendaceListViewModel.EmployeeId);
                var employeeEntity = await _employeesRepository.GetAllEmployeeIdsReportingPersonForLeave(attendaceListViewModel.EmployeeId,attendaceListViewModel.CompanyId);

                foreach (var id in employeeEntity)
                {
                    listOfEmpIds.Add(id.EmployeeId);
                }

                foreach (var items in listOfEmpIds)
                {
                    var employeeDetails = employees.Where(x => !x.IsDeleted && x.EmpId == items).FirstOrDefault();
                    if (employeeDetails != null)
                    {
                        isActiveEmployees.Add(new EmployeesEntity()
                        {
                            EmpId = employeeDetails.EmpId,
                            UserName = employeeDetails.UserName,
                            FirstName = employeeDetails.FirstName,
                            LastName = employeeDetails.LastName,
                            IsDeleted = employeeDetails.IsDeleted,
                            OfficeEmail = employeeDetails.OfficeEmail,
                            IsActive = employeeDetails.IsActive,
                            EsslId = employeeDetails.EsslId,
                            CompanyId = employeeDetails.CompanyId,
                        });
                    }
                }
            }
            if (empId != "")
            {
                var AttendaceListViewModel = new List<AttendaceListViewModel>();
                var dFrom = string.IsNullOrEmpty(StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(StartDate).ToString(Constant.DateFormat);
                var dTo = string.IsNullOrEmpty(EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(EndDate).ToString(Constant.DateFormat);
                var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
                var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";

                var companyId = Convert.ToString(attendaceListViewModel.CompanyId);
                var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
                var flterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
                var holiday = false;
                var dateOfHoliday = new DateTime();
                var getAllHolidays = await _leaveRepository.GetAllEmployeeHolidaysForYear(flterDate.Month, flterDate.Year,attendaceListViewModel.CompanyId);


                List<DateTime> allDates = new List<DateTime>();
                for (DateTime date = flterDate; date <= flterEndDate; date = date.AddDays(1))

                    if (date != DateTime.MinValue)
                    {
                        //var getHolidayDate = getAllHolidays.Where(x => x.HolidayDate.Date == date.Date).Select(y => y.HolidayDate.Date).FirstOrDefault();
                        var getHolidayDate = getAllHolidays
                       .Where(x => x.HolidayDate.Date.Equals(date.Date))
                       .Select(y => y.HolidayDate.Date)
                       .FirstOrDefault();

                        dateOfHoliday = getHolidayDate == null ? DateTime.MinValue : getHolidayDate;

                        allDates.Add(date);
                        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || dateOfHoliday != DateTime.MinValue && dateOfHoliday.DayOfWeek != DayOfWeek.Saturday || dateOfHoliday != DateTime.MinValue && dateOfHoliday.DayOfWeek != DayOfWeek.Sunday)
                        {
                            holiday = true;
                        }
                        else
                        {
                            holiday = false;
                        }
                        foreach (var getDate in isActiveEmployees)
                        {
                            if (getDate.EsslId > 0 && isActiveEmployees.Count <= 1)
                            {
                                var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);
                                var tableEmployee = Convert.ToString(getDate.EsslId);
                                bool firstInTime = false; // Initialize firstInTime
                                 //firstInTime = attendanceReportDateModels.Any(x => x.EmployeeId == tableEmployee && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                                if (attendanceReportDateModels.Any(x => x.EmployeeId == tableEmployee && x.LogDateTime != null && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn))
                                {
                                    firstInTime = true;
                                }
                                if (!firstInTime)
                                {
                                    var fromDate = date.Date;
                                    var timeLoggerEntitys = await _dashboardRepository.GetLastTimeLogEntityByEmployeeIdDate(fromDate,Convert.ToInt32(tableEmployee), attendaceListViewModel.CompanyId);
                                    if (timeLoggerEntitys.Any())
                                    {
                                        firstInTime = true;
                                    }
                                }
                                firstInTime = attendanceReportDateModels.Any(x => x.EmployeeId == tableEmployee && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                                if (firstInTime == true && holiday == true)
                                {
                                    holiday = false;
                                    attendaceListViewModels.ListOfDate.Add(new ListOfDate()
                                    {
                                        Dates = Convert.ToInt32(date.ToString("dd")),
                                        Holidays = holiday,

                                    });
                                }
                                else
                                {
                                    attendaceListViewModels.ListOfDate.Add(new ListOfDate()
                                    {
                                        Dates = Convert.ToInt32(date.ToString("dd")),
                                        Holidays = holiday,

                                    });
                                }
                            }
                        }
                        if (isActiveEmployees.Count > 1)
                        {
                            attendaceListViewModels.ListOfDate.Add(new ListOfDate()
                            {
                                Dates = Convert.ToInt32(date.ToString("dd")),
                                Holidays = holiday,

                            });
                        }
                    }
                //used common method for get attandance data
                var attendanceReportDateModelss = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);
                if (attendanceReportDateModelss.Count == 0)
                {
                    //attendanceReportDateModelss = await _dashboardService.GetTimeLog(Convert.ToInt32(empId), Convert.ToDateTime(dFrom));
                    var timeLog = await _dashboardService.GetTimeLog(Convert.ToInt32(attendaceListViewModel.EmployeeId), Convert.ToDateTime(dFrom),Convert.ToInt32(companyId));
                    attendanceReportDateModelss.Add(new AttendanceReportDateModel
                    {
                        Direction = timeLog.EntryStatus,
                        EmployeeId = Convert.ToString(timeLog.EmpId),
                        LogDate = timeLog.TodayClockIn.ToString(),

                    });
                }

                if (empId == Constant.ZeroStr && attendaceListViewModel.Month == 0)
                {
                    //used common method for get list of attendance status
                    var getData = await GetAttendanceStatus(empId, isActiveEmployees, allDates, attendanceReportDateModelss, getAllHolidays, attendaceListViewModel.CompanyId);
                    attendaceListViewModels.AttendanceStatuses = getData;
                    attendaceListViewModels.Month = month;
                    attendaceListViewModels.Year = year;
                    attendaceListViewModels.EmployeeStatus = 1;
                }
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to get attendancestatus check the attendace detail by particular employees 
        /// </summary> 
        /// <param name="empId" ></param>
        /// <param name="employees" ></param> 
        /// <param name="allDates" ></param> 
        /// <param name="attendanceReportDateModels" ></param> 
        public async Task<List<AttendanceStatus>> GetAttendanceStatus(string empId, List<EmployeesEntity> employees, List<DateTime> allDates, List<AttendanceReportDateModel> attendanceReportDateModels, List<EmployeeHolidaysEntity> getAllHolidays,int companyId)
        {
            var attendaceListViewModels = new List<AttendanceStatus>();
            var timeSheetss = await _timeSheetRepository.GetAllTimeSheet(Convert.ToInt32(empId),companyId);
            var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var dt = "";
            var breakHours = "";
            var burningHours = "";
            var count = 1;
            var lst = new List<string>();
            var listOfPresentDays = new List<bool>();
            var lstdate = new List<bool>();
            var dateOfHoliday = new DateTime();
            var listOfDays = new List<DateTime>();
            var listOfHolidays = new List<DateTime>();

            foreach (var item in allDates)
            {
                var getHolidayDate = getAllHolidays.Where(x => x.HolidayDate.Date == item.Date).Select(y => y.HolidayDate.Date).FirstOrDefault();
                dateOfHoliday = getHolidayDate == null ? DateTime.MinValue : getHolidayDate;
                if (item.DayOfWeek == DayOfWeek.Saturday || item.DayOfWeek == DayOfWeek.Sunday)
                {

                }
                else
                {
                    listOfDays.Add(item);
                }

                if (dateOfHoliday != DateTime.MinValue)
                {
                    if (dateOfHoliday.DayOfWeek == DayOfWeek.Saturday || dateOfHoliday.DayOfWeek == DayOfWeek.Sunday)
                    {

                    }
                    else
                    {
                        listOfHolidays.Add(dateOfHoliday);
                    }
                }
            }

            if (attendanceReportDateModels.Count() > 0)
            {
                foreach (var item in employees)
                {
                    var employeeProfileImage = allEmployeeProfiles.FirstOrDefault(e => e.EmpId == item.EmpId);
                    var attendacelistViewModel = new AttendaceListViewModel();

                    var status = new AttendanceStatus();
                    status.EntryStatusDate = new List<EntryStatusDate>();

                    lst = new List<string>(0);
                    listOfPresentDays = new List<bool>(0);

                    status.EntryStatusDate = new List<EntryStatusDate>(0);

                    var entrystatus = new EntryStatusDate();
                    count = count == 6 ? 1 : count;
                    if (item.EsslId > 0)
                    {
                        try
                        {
                            status.EmployeeId = item.EmpId;
                            status.EmployeeUserName = item != null ? item.UserName : string.Empty;
                            status.EmployeeName = item?.FirstName + " " + item?.LastName;
                            status.Status = item.IsDeleted;
                            status.EmployeeShortName = item?.FirstName.Substring(0, 1) + "" + item.LastName.Substring(0, 1);
                            status.EmployeeProfileImage = employeeProfileImage != null ? employeeProfileImage.ProfileName : string.Empty;
                            status.ClassName = Common.Common.GetClassNameForLeaveDashboard(count);
                            foreach (var date in allDates)
                            {
                                var attendaceViewModels = new AttendaceListViewModels();
                                var stringToNum = Convert.ToString(item.EsslId);
                                if (stringToNum != "")
                                {
                                    var tableEmployeeId = Int32.Parse(stringToNum);
                                    var firstInTime = attendanceReportDateModels.Any(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                                    //Adding TimeLogEntity Here
                                    if (date != DateTime.MinValue && date <= DateTime.Now && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !firstInTime)
                                    {
                                        var timeLogEntitys = await _dashboardRepository.GetTimeLogEntitysByEmployeeId(item.EmpId,companyId);
                                        if (timeLogEntitys.Any())
                                        {
                                            var tFrom = timeLogEntitys.Select(x => Convert.ToString(x.ClockInTime)).FirstOrDefault();
                                            var tTo = timeLogEntitys.Where(x => x.ClockOutTime.HasValue && x.EntryStatus == Constant.EntryTypeOut).Select(x => x.ClockOutTime.Value.ToString()).LastOrDefault();
                                            var attendanceReportDatamodelList = await _dashboardService.GetTimeLog(item.EmpId,date,companyId);

                                            attendaceViewModels.TotalHours = attendanceReportDatamodelList.TotalHours;
                                            attendaceViewModels.InsideOffice = attendanceReportDatamodelList.InsideOffice;
                                            attendaceViewModels.BreakHours = attendanceReportDatamodelList.BreakHours;
                                        }
                                    }
                                    var datesin = date.Date.ToString(Constant.DateFormat);
                                    lst.Add(datesin);
                                    var attendaceListViewModel = new AttendaceListViewModel();
                                    attendaceListViewModel.EsslId = (int)item.EsslId;
                                    attendaceListViewModel.StartDate = datesin;
                                    attendaceListViewModel.CompanyId = item.CompanyId;
                                    attendaceListViewModel.EmployeeId = item.EmpId;
                                    attendaceListViewModel.Status = item.IsDeleted;
                                    var esslId = item;
                                    var attandances = await GetEmployeeRecords(attendanceReportDateModels, attendaceViewModels, esslId, date);
                                    status.TotalHours = attandances.TotalHours;
                                    status.BreakHours = attandances.BreakHours;
                                    status.BurningHours = attandances.BurningHours;
                                    var actualHours = Convert.ToDateTime(attandances.InsideOffice).TimeOfDay;
                                    var thresholdTime = TimeSpan.Parse("02:00:00");
                                    var noTime = TimeSpan.Parse("00:00:00");
                                    if (actualHours <= thresholdTime && date != DateTime.Now.Date)
                                    {
                                        status.EntryStatusDate.Add(new EntryStatusDate()
                                        {
                                            EntryStatus = false,
                                            Date = date.Date.ToString(Constant.DateFormat),
                                        });
                                    }
                                    else if (actualHours == noTime)
                                    {
                                        status.EntryStatusDate.Add(new EntryStatusDate()
                                        {
                                            EntryStatus = false,
                                            Date = date.Date.ToString(Constant.DateFormat),
                                        });
                                    }
                                    else
                                    {
                                        status.EntryStatusDate.Add(new EntryStatusDate()
                                        {
                                            EntryStatus = true,
                                            Date = date.Date.ToString(Constant.DateFormat),
                                        });
                                    }

                                    if (actualHours < thresholdTime)
                                    { }
                                    else
                                    {
                                        listOfPresentDays.Add(true);
                                    }
                                }
                                status.Date = lst;
                                status.PresentCount = listOfPresentDays.Count() + "/" + Convert.ToString(listOfDays.Count() - listOfHolidays.Count());
                            }
                            attendaceListViewModels.Add(status);
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                }
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to get in/outstatus check the attendace detail by particular employees 
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>        
        public async Task<AttendaceListViewModels> GetInOutListForAll(AttendaceListViewModel attendaceListViewModel,int sessionCompanyId)
        {
            var attendaceListViewModels = new AttendaceListViewModels();
            attendaceListViewModels.ViewAttendanceLog = new List<ViewAttendanceLog>();

            attendaceListViewModel.StartDate = DateTime.Now.ToString(Constant.DateFormat);
            
            var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
            var dTo = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
            var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
            var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
            var companyId = Convert.ToString(attendaceListViewModel.CompanyId);

            var userId = attendaceListViewModel.EmployeeId;
            var employeeList = userId == 1 ? await _employeesRepository.GetAllEmployeeDetails(sessionCompanyId) : await _employeesRepository.GetAllEmpById(userId, sessionCompanyId);

            var esslId = employeeList.FirstOrDefault(x => x.EmpId == attendaceListViewModel.EmployeeId);
            var flterDate = DateTime.Now;
            var status = Convert.ToString(0);
            var empId = Constant.ZeroStr;
                        
            var employeeEntity = new EmployeesEntity();
            List<DateTime> allDates = new List<DateTime>();
            var formDate = Convert.ToDateTime(DateTime.Now); 
            var toDate = Convert.ToDateTime(DateTime.Now);
            for (DateTime dates = formDate; dates <= toDate; dates = dates.AddDays(1))
            {
                allDates.Add(dates);
            }


            var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);
            attendanceReportDateModels = attendanceReportDateModels.ToList();
            var attendanceReportDatamodel = await _dashboardRepository.GetLastTimeLogEntityByEmployeeIdList(attendaceListViewModel.EmployeeId, formDate, toDate, attendaceListViewModel.CompanyId);

            foreach (var item in attendanceReportDateModels)
            {
                employeeEntity = employeeList.FirstOrDefault(x => x.EsslId == Convert.ToInt32(item.EmployeeId));
                if (employeeEntity != null)
                {
                    attendaceListViewModels.ViewAttendanceLog.Add(new ViewAttendanceLog()
                    {
                        Id = item.Id,
                        EmployeeId = employeeEntity.UserName,
                        EmployeeName = employeeEntity.FirstName + " " + employeeEntity.LastName,
                        Status = employeeEntity.IsDeleted,
                        EmployeeCode = item.EmployeeId,
                        LogDateTime = Convert.ToDateTime(item.LogDateTime).ToString(Constant.DateTimeFormat),
                        LogDate = Convert.ToDateTime(item.LogDateTime).ToString(Constant.DateFormat),
                        LogTime = Convert.ToDateTime(item.LogDateTime).ToString(Constant.TimeFormatWithFullForm),
                        Direction = item.Direction,

                    });
                }
            }
            foreach (var data in allDates)
            {

                var checkdates = data.Date;

                var fdate = checkdates.Date.ToString("dd/MM/yyyy");

                var empid = Convert.ToString(userId);
                var attendancereport = await GetAllAttendancereport(empId, fdate, fdate, tFrom, tTo, status, companyId);

                if (attendancereport.Count() == 0)
                {
                    var attendanceReportDatamodelList = await _dashboardRepository.GetLastTimeLogEntityByEmployeeIdDateList(data.Date, tFrom, tTo,attendaceListViewModel.CompanyId); 
                    var timeLogEntitys = await _dashboardRepository.GetTimeLogEntityByEmpId(Convert.ToInt32(empid), checkdates,Convert.ToInt32(companyId));
                    if (timeLogEntitys.Any())
                    {
                        foreach (var timeLogEntity in timeLogEntitys)
                        {
                            attendaceListViewModels.ViewAttendanceLog.Add(new ViewAttendanceLog()
                            {
                                Id = timeLogEntity.EmployeeId,
                                EmployeeId = employeeEntity.UserName,
                                EmployeeName = employeeEntity.FirstName + " " + employeeEntity.LastName,
                                Status = employeeEntity.IsDeleted,
                                EmployeeCode = Convert.ToString(timeLogEntity.EmployeeId),
                                LogDateTime = Convert.ToDateTime(timeLogEntity.CreatedDate).ToString(Constant.DateTimeFormat),
                                LogDate = Convert.ToDateTime(timeLogEntity.CreatedDate).ToString(Constant.DateFormat),
                                LogTime = Convert.ToDateTime(timeLogEntity.CreatedDate).ToString(Constant.TimeFormatWithFullForm),
                                Direction = timeLogEntity.EntryStatus,
                            });
                        }
                    }
                    if (attendaceListViewModel.EmployeeId != 0)
                    {
                        double breaks = 0;
                        double totalHours = 0;
                        attendanceReportDatamodelList = await _dashboardRepository.GetLastTimeLogEntityByEmployeeIdDate(attendaceListViewModel.EmployeeId, data.Date, tFrom, tTo,attendaceListViewModel.CompanyId);
                        foreach (var item in attendanceReportDatamodelList)
                        {
                            if (attendanceReportDatamodel.Count() > 0)
                            {
                                totalHours += item.LogSeconds;
                            }
                        }
                        var attendanceReportDatamodelLists = await _dashboardService.GetTimeLog(Convert.ToInt32(empid), checkdates,Convert.ToInt32(companyId));
                        var breakHours = GetBreakHourForTimeLogger(Convert.ToString(empid), timeLogEntitys, checkdates);
                        var BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM) + " " + Constant.Hrs;


                        attendaceListViewModels.TotalHours = attendanceReportDatamodelLists.TotalHours;
                        attendaceListViewModels.InsideOffice = attendanceReportDatamodelLists.InsideOffice;
                        attendaceListViewModels.BreakHours = BreakHours;
                        // Order the TimeLogView list by EmpId
                        var attendances = attendaceListViewModels.ViewAttendanceLog.OrderBy(x => x.EmployeeId).ToList();
                        attendaceListViewModels.ViewAttendanceLog = attendances;

                        return attendaceListViewModels;

                    }
                }

            }
            if (attendanceReportDateModels.Count() > 0)
            {
                var filterDate = DateTime.Now;
                var attendanceViewModels = await GetEmployeeRecords(attendanceReportDateModels, attendaceListViewModels, esslId, filterDate);
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to Get Employee Records
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        /// <param name="esslId" ></param>
        /// <param name="filterDate" ></param>
        public async Task<AttendaceListViewModels> GetEmployeeRecords(List<AttendanceReportDateModel> attendanceReportDateModels, AttendaceListViewModels attendaceListViewModels, EmployeesEntity esslId, DateTime filterDate)
        {
            var getEmployeeRecord = attendanceReportDateModels.Where(x => x.EmployeeId == Convert.ToString(esslId?.EsslId) && x.LogDate == filterDate.Date.ToString("yyyy-MM-dd")).ToList();

            var listOfRec = getEmployeeRecord.TakeLast(1);
            var lastRecord = listOfRec.FirstOrDefault(x => x.Direction == Constant.EntryTypeIn);
            var attendance = new AttendanceReportDateModel();
            if (lastRecord != null)
            {
                foreach (var record in listOfRec)
                {
                    getEmployeeRecord.Add(new AttendanceReportDateModel()
                    {
                        Id = 0,
                        EmployeeId = record.EmployeeId,
                        LogDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        LogDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Direction = Constant.EntryTypeOut,
                    });
                }
            }

            var stringToNum = Convert.ToString(esslId?.EsslId);
            var firstInTime = getEmployeeRecord.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date && x.Direction == Constant.EntryTypeIn);
            var lastInTime = getEmployeeRecord.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date && x.Direction == Constant.EntryTypeOut);
            var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
            var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";


            if (!string.IsNullOrEmpty(firstLoginDate))
            {
                DateTime StartTime = Convert.ToDateTime(firstInTime?.LogDateTime);
                DateTime EndTime = Convert.ToDateTime(lastInTime?.LogDateTime);
                string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                var breakHours = GetBreakHoursForBreak(stringToNum, getEmployeeRecord, filterDate);
                var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                attendaceListViewModels.EmployeeId = esslId.EmpId;
                attendaceListViewModels.EmployeeName = esslId.FirstName + " " + esslId.LastName;
                attendaceListViewModels.Date = filterDate.Date.ToString(Constant.DateFormat);
                attendaceListViewModels.TotalHours = dt;
                attendaceListViewModels.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
                attendaceListViewModels.InsideOffice = insideoffice;
                if (attendaceListViewModels.InsideOffice != null)
                {
                    string time = attendaceListViewModels.InsideOffice;
                    double seconds = TimeSpan.Parse(time).TotalSeconds;
                    attendaceListViewModels.TotalSecounds = Convert.ToInt64(seconds);
                }
            }
            return attendaceListViewModels;
        }


        /// <summary>
        /// Logic to get breakhours check the attendace detail by particular employees 
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>    
        public string GetBreakHours(string empId, List<AttendanceEntity> attendanceEntities)
        {
            var attendanceEty = attendanceEntities.Where(x => x.EmployeeCode == empId && Convert.ToDateTime(x.LogDate) == DateTime.Now.Date).Skip(1).ToList();
            var outEntry = DateTime.Now;
            var inEntry = DateTime.Now;
            double totalSeconds = 0;
            foreach (var item in attendanceEty)
            {
                if (item.Direction == Constant.EntryTypeOut)
                {
                    outEntry = Convert.ToDateTime(item.LogDateTime);
                }
                else if (item.Direction == Constant.EntryTypeIn)
                {
                    var seconds = (Convert.ToDateTime(item.LogDateTime) - outEntry).TotalSeconds;
                    totalSeconds += seconds;
                }
            }
            TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
            return answer;
        }

        /// <summary>
        /// Logic to get breakhours check the attendace detail 
        /// </summary> 
        /// <param name="empId" ></param> 
        /// <param name="attendanceEntities" ></param> 
        /// <param name="filterDate" ></param> 
        public string GetBreakHour(string empId, List<AttendanceReportDateModel> attendanceEntities, DateTime filterDate)
        {
            var attendanceEty = attendanceEntities.Where(x => x.EmployeeId == empId && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date).ToList();
            var listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).ToList();

            if (listOfRecords.Count > 0)
            {
                var attendanceReportDateModel = listOfRecords.Take(1).FirstOrDefault();

                if (attendanceReportDateModel.Direction == Constant.EntryTypeOut)
                {
                    listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).Skip(2).ToList();
                }
                else
                {
                    listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).Skip(1).ToList();
                }
            }
            var outEntry = DateTime.Now;
            var inEntry = DateTime.Now;
            double totalSeconds = 0;
            foreach (var item in listOfRecords)
            {
                if (item.Direction == Constant.EntryTypeOut)
                {
                    outEntry = Convert.ToDateTime(item.LogDateTime);
                }
                else if (item.Direction == Constant.EntryTypeIn)
                {
                    var seconds = (Convert.ToDateTime(item.LogDateTime) - outEntry).TotalSeconds;
                    totalSeconds += seconds;
                }
            }
            if (totalSeconds < 0)
            {
                return Constant.TimeFormatZero;
            }
            else
            {
                TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
                var answer = Convert.ToString((t.Days * 24 + t.Hours).ToString("D2") + ":" + t.Minutes.ToString("D2") + ":" + t.Seconds.ToString("D2"));
                return answer;
            }

        }


        /// <summary>
        /// Logic to Get Break Hour For TimeLogger
        /// </summary> 
        /// <param name="empId" ></param>
        /// <param name="timeLoggerEntities" ></param>
        /// <param name="filterDate" ></param>
        public string GetBreakHourForTimeLogger(string empId, List<TimeLoggerEntity> timeLoggerEntities, DateTime filterDate)
        {
            var timeLoggerEty = timeLoggerEntities
                .Where(x => x.EmployeeId.ToString() == empId &&
                            (x.ClockInTime.Date == filterDate.Date ||
                             (x.ClockOutTime.HasValue && x.ClockOutTime.Value.Date == filterDate.Date)))
                .OrderBy(x => x.ClockInTime)
                .ToList();

            double totalBreakSeconds = 0;

            Console.WriteLine($"Filtered 'out' Entries for Employee {empId} on {filterDate.ToShortDateString()}");

            foreach (var record in timeLoggerEty.Where(x => x.EntryStatus?.ToLower() == Constant.EntryTypeOut))
            {
                if (record.ClockOutTime.HasValue)
                {
                    double breakDuration = (record.ClockOutTime.Value - record.ClockInTime).TotalSeconds;

                    if (breakDuration > 0)
                    {
                        totalBreakSeconds += breakDuration;
                        Console.WriteLine($"Break Duration Added: {breakDuration} sec | Total: {totalBreakSeconds} sec");
                    }
                }
            }

            totalBreakSeconds = Math.Max(totalBreakSeconds, 0);

            TimeSpan t = TimeSpan.FromSeconds(totalBreakSeconds);
            string result = $"{(t.Days * 24 + t.Hours):D2}:{t.Minutes:D2}:{t.Seconds:D2}";

            Console.WriteLine($"Final Total Break Time: {result}");

            return result;
        }


        /// <summary>
        /// Logic to get breakhours check the attendace detail 
        /// </summary> 
        /// <param name="empId" ></param> 
        /// <param name="attendanceEntities" ></param> 
        /// <param name="filterDate" ></param> 
        public string GetBreakHoursForBreak(string empId, List<AttendanceReportDateModel> attendanceEntities, DateTime filterDate)
        {
            var attendanceEty = attendanceEntities.Skip(1).ToList();
            var outEntry = DateTime.Now;
            var inEntry = DateTime.Now;
            double totalSeconds = 0;
            foreach (var item in attendanceEty)
            {
                if (item.Direction == Constant.EntryTypeOut)
                {
                    outEntry = Convert.ToDateTime(item.LogDateTime);
                }
                else if (item.Direction == Constant.EntryTypeIn)
                {
                    var seconds = (Convert.ToDateTime(item.LogDateTime) - outEntry).TotalSeconds;
                    totalSeconds += seconds;
                }
            }
            if (totalSeconds < 0)
            {
                return Constant.TimeFormatZero;
            }
            else
            {
                TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
                var answer = Convert.ToString((t.Days * 24 + t.Hours).ToString("D2") + ":" + t.Minutes.ToString("D2") + ":" + t.Seconds.ToString("D2"));
                return answer;
            }
        }

        /// <summary>
        /// Logic to get timesheethours check the timesheet detail 
        /// </summary> 
        /// <param name="empId" ></param> 
        /// <param name="timeSheetEntitys" ></param> 
        public string GetTimeSheetWorkHours(int empId, List<TimeSheetEntity> timeSheetEntitys)
        {
            double totalSeconds = 0;
            var answer = "";
            var timeSheets = timeSheetEntitys.Where(x => x.EmpId == empId).ToList();
            foreach (var ts in timeSheets)
            {
                var seconds = (ts.EndTime - ts.StartTime).TotalSeconds;
                totalSeconds += seconds;
            }

            if (totalSeconds <= 0)
            {
                answer = Constant.TimeFormatZero;
            }
            else
            {
                TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
                answer = Convert.ToString((t.Days * 24 + t.Hours).ToString("D2") + ":" + t.Minutes.ToString("D2") + ":" + t.Seconds.ToString("D2"));
            }
            return answer;
        }

        /// <summary>
        /// Logic to get filter  the attendace detail  by particular attendaceemployee
        /// </summary>
        /// <param name="attendaceListViewModel" >attendaceListViewModel</param>
        public async Task<AttendaceListViewModels> GetAllEmployessByAttendanceFilter(AttendaceListViewModel attendaceListViewModel, int companyId)
        {
            var attendaceListViewModels = new AttendaceListViewModels();

            var employeeIdValue = attendaceListViewModel.EmployeeId;

            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            attendaceListViewModels.ViewAttendanceLog = new List<ViewAttendanceLog>();

            var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
            var dTo = string.IsNullOrEmpty(attendaceListViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString(Constant.DateFormat);
            var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
            var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
            var companyID = Convert.ToString(attendaceListViewModel.CompanyId);
            var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
            var flterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
            //get all timesheet data
            var getAllTimeSheetlist = await _timeSheetRepository.GetAllTimeSheet(Convert.ToInt32(employeeIdValue),companyId);
            //get all employee data
            var employees = await _employeesRepository.GetAllEmployees(companyId);

            List<DateTime> allDates = new List<DateTime>();

            for (DateTime date = flterDate; date <= flterEndDate; date = date.AddDays(1))
                allDates.Add(date);

            var status = Convert.ToString(attendaceListViewModel.EmployeeStatus);

            if (employeeIdValue == 0 && status == Constant.ZeroStr)
            {
                var empId = Constant.ZeroStr;
                //used common method for code simplification for get attendance data
                var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyID);
                var activeEmployee = employees.Where(x => !x.IsDeleted).ToList();
                //used common method for code simplification
                var getAttendanceData = await GetAllEmployessAttendanceFilter(allDates, activeEmployee, attendanceReportDateModels, getAllTimeSheetlist);
                attendaceListViewModels.AttendaceListViewModel = getAttendanceData.AttendaceListViewModel;
            }
            else if (employeeIdValue == 0 && status != Constant.ZeroStr)
            {
                var empId = Constant.ZeroStr;
                //used common method for code simplification
                var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyID);
                var inActiveEmployees = employees.Where(x => x.IsDeleted).ToList();
                //used common method for code simplification for get attendance data
                var getAttendanceData = await GetAllEmployessAttendanceFilter(allDates, inActiveEmployees, attendanceReportDateModels, getAllTimeSheetlist);
                attendaceListViewModels.AttendaceListViewModel = getAttendanceData.AttendaceListViewModel;
            }
            else if (employeeIdValue > 0 && status == Constant.ZeroStr)
            {
                var esslId = await _employeesRepository.GetEmployeeById(attendaceListViewModel.EmployeeId, companyId);
                var attendanceListViewModels = await GetAllEmployessAttendanceReportandFilter(attendaceListViewModel, employees, getAllTimeSheetlist, allDates, esslId);
                return attendanceListViewModels;
            }
            else if (employeeIdValue > 0 && status != Constant.ZeroStr)
            {
                var esslId = await _employeesRepository.GetEmployeeByIdView(attendaceListViewModel.EmployeeId, attendaceListViewModel.CompanyId);
                var attendanceListViewModels = await GetAllEmployessAttendanceReportandFilter(attendaceListViewModel, employees, getAllTimeSheetlist, allDates, esslId);
                return attendanceListViewModels;
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to Get All Employess Attendance Report and Filter
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        /// <param name="employees" ></param>
        /// <param name="getAllTimeSheetlist" ></param>
        /// <param name="allDates" ></param>
        /// <param name="esslId" ></param>
        public async Task<AttendaceListViewModels> GetAllEmployessAttendanceReportandFilter(AttendaceListViewModel attendaceListViewModel, List<EmployeesEntity> employees, List<TimeSheetEntity> getAllTimeSheetlist, List<DateTime> allDates, EmployeesEntity esslId)
        {
            var attendaceListViewModels = new AttendaceListViewModels();
            if (esslId != null)
            {
                var empId = Convert.ToString(esslId.EsslId);
                var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
                var dTo = string.IsNullOrEmpty(attendaceListViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString(Constant.DateFormat);
                var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
                var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
                var companyId = Convert.ToString(attendaceListViewModel.CompanyId);
                var status = Convert.ToString(attendaceListViewModel.EmployeeStatus);
                //used common method for code simplification
                var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);
                var deleted = Convert.ToBoolean(attendaceListViewModel.EmployeeStatus);
                var filterViewAttendaces = _mapper.Map<List<FilterViewAttendace>>(attendanceReportDateModels);
                var employeeEntity = new EmployeesEntity();
                if (deleted)
                {
                    employeeEntity = employees.FirstOrDefault(x => x.EmpId == attendaceListViewModel.EmployeeId && x.IsDeleted == true);
                }
                else
                {
                    employeeEntity = employees.FirstOrDefault(x => x.EmpId == attendaceListViewModel.EmployeeId && !x.IsDeleted);
                }
                var getEmployeeAttendance = await GetEmployessAttendanceFilter(allDates, employeeEntity, filterViewAttendaces, getAllTimeSheetlist, attendanceReportDateModels);
                attendaceListViewModels.AttendaceListViewModel = getEmployeeAttendance.AttendaceListViewModel;
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to get attendacereport the attendace detail  
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="dFrom" ></param>
        /// <param name="dTo" ></param>
        /// <param name="status" ></param>
        public async Task<List<AttendanceReportDateModel>> GetAllAttendancereport(string empId, string dFrom, string dTo, string tFrom, string tTo, string status, string companyId)
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@empId", empId),
                new KeyValuePair<string, string>("@startDate",dFrom),
                new KeyValuePair<string, string>("@endDate",dTo),
                new KeyValuePair<string, string>("@startTime", tFrom),
                new KeyValuePair<string, string>("@endTime", tTo),
                new KeyValuePair<string, string>("@employeeStatus",status),
                new KeyValuePair<string, string>("@companyId",companyId),
            };
            var attendanceReportDateModels = await _attendanceRepository.GetAllEmployeesByAttendaceFilter("spGetInandOutByEmployeeFilterData", p);
            var attendanceReportDateModel = attendanceReportDateModels.OrderBy(x => x.EmployeeId).ToList();
            attendanceReportDateModels = attendanceReportDateModels.DistinctBy(x => x.LogDateTime).ToList();
            return attendanceReportDateModels;
        }

        /// <summary>
        /// Logic to get attendace detail  
        /// </summary>
        /// <param name="allDates" ></param>
        /// <param name="activeEmployee" ></param>
        /// <param name="attendanceReportDateModels" ></param>
        /// <param name="getAllTimeSheetlist" ></param>
        public async Task<AttendaceListViewModels> GetAllEmployessAttendanceFilter(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<AttendanceReportDateModel> attendanceReportDateModels, List<TimeSheetEntity> getAllTimeSheetlist)
        {
            var attendaceListViewModels = new AttendaceListViewModels();


            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();

            foreach (var date in allDates)
            {
                foreach (var item in activeEmployee)
                {
                    var attendacelistViewModel = new AttendaceListViewModel();
                    if (item.EsslId != null)
                    {

                        var timeSheets = getAllTimeSheetlist.Where(x => x.StartTime.Date == date.Date && !x.IsDeleted).ToList();
                        var stringToNum = Convert.ToString(item.EsslId);
                        var tableEmployeeId = Int32.Parse(stringToNum);

                        var firstInTime = attendanceReportDateModels.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                        var lastInTime = attendanceReportDateModels.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);

                        var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                        var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";

                        if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                        {
                            DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                            DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                            string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                            double seconds = TimeSpan.Parse(dt).TotalSeconds;
                            dt = seconds > 0 ? dt : dt = Constant.TimeFormatZero;
                            var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                            var burningHours = GetTimeSheetWorkHours(item.EmpId, timeSheets);
                            var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                            insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                            attendacelistViewModel.EmployeeId = item.EmpId;
                            attendacelistViewModel.UserName = item.UserName;
                            attendacelistViewModel.EmployeeName = item.FirstName + " " + item.LastName;
                            attendacelistViewModel.Status = item.IsDeleted;
                            attendacelistViewModel.Date = date.Date.ToString(Constant.DateFormat);
                            attendacelistViewModel.TotalHours = dt;
                            attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
                            attendacelistViewModel.InsideOffice = insideoffice;
                            attendacelistViewModel.BurningHours = burningHours;
                            attendacelistViewModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                            attendacelistViewModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                            attendacelistViewModel.Id = 0;
                            attendaceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
                        }
                    }
                }
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to get attendacefilter detail  
        /// </summary>
        /// <param name="allDates" ></param>
        /// <param name="activeEmployee" ></param>
        /// <param name="filterViewAttendaces" ></param>
        /// <param name="getAllTimeSheetlist" ></param>
        /// <param name="attendanceReportDateModels" ></param>
        public async Task<AttendaceListViewModels> GetEmployessAttendanceFilter(List<DateTime> allDates, EmployeesEntity employeeEntity, List<FilterViewAttendace> filterViewAttendaces, List<TimeSheetEntity> getAllTimeSheetlist, List<AttendanceReportDateModel> attendanceReportDateModels)
        {
            var attendaceListViewModels = new AttendaceListViewModels();
            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            var essl = Convert.ToInt16(employeeEntity?.EsslId);
            var stringToNum = Convert.ToString(essl);
            var tableEmployeeId = essl;
            foreach (var date in allDates)
            {
                var timeSheets = getAllTimeSheetlist.Where(x => x.StartTime.Date == date.Date && !x.IsDeleted).ToList();
                var firstInTime = filterViewAttendaces.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                var lastInTime = filterViewAttendaces.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);
                var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";
                if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                {
                    DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                    DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                    string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                    dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                    var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                    var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                    insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                    var burningHours = GetTimeSheetWorkHours(employeeEntity.EmpId, timeSheets);
                    var attendaceListModel = new AttendaceListViewModel();
                    attendaceListModel.EmployeeId = employeeEntity.EmpId;
                    attendaceListModel.UserName = employeeEntity == null ? "" : employeeEntity.UserName;
                    attendaceListModel.EmployeeName = employeeEntity != null ? employeeEntity.FirstName + " " + employeeEntity.LastName : "";
                    attendaceListModel.Status = employeeEntity != null ? employeeEntity.IsDeleted : false;
                    attendaceListModel.Date = date.ToString(Constant.DateFormat);
                    attendaceListModel.TotalHours = dt;
                    attendaceListModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat);
                    attendaceListModel.InsideOffice = insideoffice;
                    attendaceListModel.BurningHours = burningHours;
                    attendaceListModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                    attendaceListModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                    attendaceListViewModels.AttendaceListViewModel.Add(attendaceListModel);

                }
            }
            return attendaceListViewModels;
        }

        /// <summary>
        /// Logic to get send the employeeattendancedetails detail  by particular employeeattendancedetails
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        public async Task<bool> SendEmployeeAttendance(AttendaceListViewModel attendaceListViewModel, int companyId)
        {
            var result = false;
            var attendaceListViewModels = new AttendaceListViewModels();
            var combinedPath = new List<string>();

            var employeeIdValue = attendaceListViewModel.EmployeeId;

            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            Common.Common.WriteServerErrorLog(" Attendance EmployeeId Search : " + attendaceListViewModel.EmployeeId);
            var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
            var dTo = string.IsNullOrEmpty(attendaceListViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString(Constant.DateFormat);
            var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
            var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
            var companyID = Convert.ToString(attendaceListViewModel.CompanyId);
            var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
            var flterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
            var getAllTimeSheetlist = await _timeSheetRepository.GetAllTimeSheet(Convert.ToInt32(employeeIdValue),companyId);
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = flterDate; date <= flterEndDate; date = date.AddDays(1))
                allDates.Add(date);

            var esslId = await _employeesRepository.GetEmployeeById(attendaceListViewModel.EmployeeId, companyId);
            if (esslId != null && allDates.Count == 1)
            {
                var empId = Convert.ToString(esslId.EsslId);
                var status = Convert.ToString(attendaceListViewModel.EmployeeStatus);
                var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyID);
                var employees = await _employeesRepository.GetAllEmployees(companyId);
                attendanceReportDateModels = attendanceReportDateModels.ToList();
                var filterViewAttendaces = _mapper.Map<List<FilterViewAttendace>>(attendanceReportDateModels);
                var employeeEntity = employees.FirstOrDefault(x => x.EmpId == employeeIdValue);
                var essl = Convert.ToInt16(employeeEntity?.EsslId);
                var stringToNum = Convert.ToString(essl);
                var tableEmployeeId = essl;

                foreach (var date in allDates)
                {
                    var timeSheets = getAllTimeSheetlist.Where(x => x.StartTime.Date == date.Date && !x.IsDeleted).ToList();
                    var firstInTime = filterViewAttendaces.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                    var lastInTime = filterViewAttendaces.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);
                    var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                    var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";
                    if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                    {
                        DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                        DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                        string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                        dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                        var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                        var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                        insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                        var burningHours = GetTimeSheetWorkHours(employeeIdValue, timeSheets);
                        var attendaceListModel = new AttendaceListViewModel();
                        attendaceListModel.EmployeeId = employeeIdValue;
                        attendaceListModel.UserName = employeeEntity == null ? "" : employeeEntity.UserName;
                        attendaceListModel.EmployeeName = employeeEntity != null ? employeeEntity.FirstName + " " + employeeEntity.LastName : "";
                        attendaceListModel.Status = employeeEntity != null ? employeeEntity.IsDeleted : false;
                        attendaceListModel.Date = date.ToString(Constant.DateFormat);
                        attendaceListModel.TotalHours = dt;
                        attendaceListModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat);
                        attendaceListModel.InsideOffice = insideoffice;
                        attendaceListModel.BurningHours = burningHours;
                        if (attendaceListModel.InsideOffice != null)
                        {
                            string time = attendaceListModel.InsideOffice;
                            double seconds = TimeSpan.Parse(time).TotalSeconds;
                            attendaceListModel.TotalSecounds = Convert.ToInt64(seconds);
                        }
                        attendaceListModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                        attendaceListModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                        attendaceListViewModels.AttendaceListViewModel.Add(attendaceListModel);
                        Common.Common.WriteServerErrorLog("Id : " + employeeIdValue + " Total Hr: " + dt + " Break Hr: " + breakHours + " Burnig Hr : " + burningHours);

                    }
                    var attendanceListDataModel = _mapper.Map<List<AttendanceListDataModel>>(attendaceListViewModels.AttendaceListViewModel);
                    if (attendanceListDataModel.Count() > 0 && attendaceListViewModel.EmployeeStatus == 0)
                    {
                        var draftTypeId = (int)EmailDraftType.AttendanceLog;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                        var bodyContent = EmailBodyContent.SendEmail_Body_Attendance(attendanceListDataModel, emailDraftContentEntity.DraftBody);
                        result = await InsertEmailAttendance(employeeEntity.OfficeEmail, emailDraftContentEntity, bodyContent, combinedPath);
                    }
                }
            }
            else
            {
                var attendanceReportDateModels = await GetAllEmployessByAttendanceFilter(attendaceListViewModel,companyId);
                var employees = await _employeesRepository.GetAllEmployees(companyId);
                if (attendanceReportDateModels.AttendaceListViewModel.Count() > 0)
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Employee Attendance Details");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
                    worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
                    worksheet.Cell(currentRow, 3).Value = Constant.Date;
                    worksheet.Cell(currentRow, 4).Value = Constant.EntryTime;
                    worksheet.Cell(currentRow, 5).Value = Constant.ExitTime;
                    worksheet.Cell(currentRow, 6).Value = Constant.TotalHours;
                    worksheet.Cell(currentRow, 7).Value = Constant.BreakHours;
                    worksheet.Cell(currentRow, 8).Value = Constant.ActualHours;
                    worksheet.Cell(currentRow, 9).Value = Constant.TimeSheetHours;
                    worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 9).Style.Font.Bold = true;

                    foreach (var user in attendanceReportDateModels.AttendaceListViewModel)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                        worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                        worksheet.Cell(currentRow, 1).Value = user.UserName;
                        worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                        worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.Date);
                        worksheet.Cell(currentRow, 4).Value = Convert.ToString(user.EntryTime);
                        worksheet.Cell(currentRow, 5).Value = Convert.ToString(user.ExitTime);
                        worksheet.Cell(currentRow, 6).Value = user.TotalHours;
                        worksheet.Cell(currentRow, 7).Value = user.BreakHours;
                        worksheet.Cell(currentRow, 8).Value = user.InsideOffice;
                        worksheet.Cell(currentRow, 9).Value = user.BurningHours;
                    }
                    var fileName = string.Format("EmployeeAttendanceDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                    var fileId = Guid.NewGuid().ToString() + "_" + fileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
                    combinedPath.Add(Path.Combine(path, fileNames));
                    var compath = Path.Combine(path, fileNames);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        workbook.SaveAs(compath);
                        memoryStream.Position = 0;
                        var content = memoryStream.ToArray();
                        // HttpContext.Session.Set(Constant.fileId, content);
                    }
                }
                result = await SendMail(attendaceListViewModel, combinedPath, companyId);
            }
            return result;
        }

        /// <summary>
        /// Logic to get send the employeeattendancedetails details 
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        public async Task<bool> SendEmployeeAttendanceForAllEmployee(AttendaceListViewModel attendaceListViewModel,int sessionCompanyId)
        {
            var result = false;
            var attendanceModel = new AttendanceListDataModel();
            var attendaceListViewModels = new AttendaceListViewModels();
            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            var combinedPath = new List<string>();
            List<DateTime> allDates = new List<DateTime>();

            var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
            var dTo = string.IsNullOrEmpty(attendaceListViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString(Constant.DateFormat);
            var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
            var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
            var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
            var companyId = Convert.ToString(attendaceListViewModel.CompanyId);

            var esslId = await _employeesRepository.GetEmployeeById(attendaceListViewModel.EmployeeId,sessionCompanyId);
            var empId = esslId == null ? Constant.ZeroStr : Convert.ToString(esslId.EsslId);
            var status = Convert.ToString(attendaceListViewModel.EmployeeStatus);

            var employees = await _employeesRepository.GetAllEmployees(sessionCompanyId);
            var timeSheets = await _timeSheetRepository.GetAllTimeSheetByCurrentDate(flterDate.Date, attendaceListViewModel.CompanyId);
            var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);
            attendanceReportDateModels = attendanceReportDateModels.ToList();

            var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
            var filterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);

            if (filterDate != DateTime.MinValue)
            {
                for (DateTime date = filterDate; date <= filterEndDate; date = date.AddDays(1))
                    allDates.Add(date);
            }

            if (attendaceListViewModel.Month > 0 || allDates.Count() > 1)
            {
                List<int> empIds = new List<int>();

                if (attendaceListViewModel.Month > 0 || allDates.Count > 1)
                {
                    empIds = employees.Select(x => x.EmpId).ToList();
                }
                else
                {
                    empIds.Add(attendaceListViewModel.EmployeeId);
                }
                empIds = empIds.Distinct().ToList();
                foreach (var emp in empIds)
                {
                    var listEmployees = new List<EmployeesEntity>();

                    var getEmployee = employees.Where(x => x.EmpId == emp).FirstOrDefault();
                    if (getEmployee.EsslId > 0)
                    {
                        if (getEmployee != null)
                        {
                            listEmployees.Add(new EmployeesEntity()
                            {
                                EmpId = getEmployee.EmpId,
                                EsslId = getEmployee.EsslId,
                                OfficeEmail = getEmployee.OfficeEmail,
                                UserName = getEmployee.UserName,
                                FirstName = getEmployee.FirstName,
                                LastName = getEmployee.LastName,
                                CompanyId = getEmployee.CompanyId,
                                RoleId = getEmployee.RoleId,
                                DesignationId = getEmployee.DesignationId,
                                DepartmentId = getEmployee.DepartmentId,
                                CreatedBy = getEmployee.CreatedBy,
                                CreatedDate = getEmployee.CreatedDate,
                            });
                        }

                        var getEmployeeDetails = employees.Where(x => x.EmpId == emp && !x.IsDeleted).FirstOrDefault();

                        var attendanceviewModelList = GetAllEmployessAttendanceData(allDates, listEmployees, attendanceReportDateModels, timeSheets);
                        if (attendanceviewModelList.AttendaceListViewModel.Count() > 0)
                        {
                            var excel = GenarateExcel(attendanceviewModelList.AttendaceListViewModel);
                            combinedPath.Add(excel);

                            if (attendaceListViewModel.Month > 0 && excel != "")
                            {
                                var draftTypeId = (int)EmailDraftType.MonthlyAttendance;
                                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,sessionCompanyId);

                                if (emailDraftContentEntity != null)
                                {
                                    var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName, emailDraftContentEntity.DraftBody);
                                    result = await InsertEmailAttendance(getEmployeeDetails.OfficeEmail, emailDraftContentEntity, bodyContent, combinedPath);
                                }
                            }
                            else
                            {
                                var draftTypeId = (int)EmailDraftType.AttendanceLogEmployees;
                                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,sessionCompanyId);
                                if (emailDraftContentEntity != null)
                                {
                                    var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName, emailDraftContentEntity.DraftBody);
                                    result = await InsertEmailAttendance(getEmployeeDetails.OfficeEmail, emailDraftContentEntity, bodyContent, combinedPath);
                                }
                            }
                        }

                    }
                    combinedPath = new List<string>(0);
                }
            }
            else
            {
                foreach (var item in employees)
                {
                    var attendacelistViewModel = new AttendaceListViewModel();
                    if (item.EsslId > 0)
                    {
                        var stringToNum = Convert.ToString(item.EsslId);
                        var tableEmployeeId = Int32.Parse(stringToNum);
                        var firstInTime = attendanceReportDateModels.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == flterDate.Date && x.Direction == Constant.EntryTypeIn);
                        var lastInTime = attendanceReportDateModels.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == flterDate.Date && x.Direction == Constant.EntryTypeOut);
                        var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                        var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";
                        if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                        {
                            DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                            DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                            string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                            dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                            var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, flterDate);
                            var burningHours = GetTimeSheetWorkHours(item.EmpId, timeSheets);
                            var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                            insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                            attendacelistViewModel.EmployeeId = item.EmpId;
                            attendacelistViewModel.UserName = item.UserName;
                            attendacelistViewModel.EmployeeName = item.FirstName + " " + item.LastName;
                            attendacelistViewModel.Status = item.IsDeleted;
                            attendacelistViewModel.Date = flterDate.Date.ToString(Constant.DateFormat);
                            attendacelistViewModel.TotalHours = dt;
                            attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat);
                            attendacelistViewModel.InsideOffice = insideoffice;
                            attendacelistViewModel.BurningHours = burningHours;
                            attendacelistViewModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                            attendacelistViewModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                            attendacelistViewModel.OfficeEmail = item.OfficeEmail;
                            attendaceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
                            if (attendacelistViewModel.InsideOffice != null)
                            {
                                string time = attendacelistViewModel.InsideOffice;
                                double seconds = TimeSpan.Parse(time).TotalSeconds;
                                attendacelistViewModel.TotalSecounds = Convert.ToInt64(seconds);
                            }

                            attendanceModel = _mapper.Map<AttendanceListDataModel>(attendacelistViewModel);
                            if (attendanceModel != null)
                            {
                                var draftTypeId = (int)EmailDraftType.AttendanceLog;
                                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, Convert.ToInt32(companyId));
                                var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceAll(attendanceModel, emailDraftContentEntity.DraftBody);
                                result = await InsertEmailAttendance(attendanceModel.OfficeEmail, emailDraftContentEntity, bodyContent, combinedPath);
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to Get All Employess Attendance Data
        /// </summary> 
        /// <param name="allDates" ></param>
        /// <param name="activeEmployee" ></param>
        /// <param name="attendanceReportDateModels" ></param>
        /// <param name="getAllTimeSheetlist" ></param>
        public AttendaceListViewModels GetAllEmployessAttendanceData(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<AttendanceReportDateModel> attendanceReportDateModels, List<TimeSheetEntity> getAllTimeSheetlist)
        {
            try
            {
                var attendaceListViewModels = new AttendaceListViewModels();
                attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();

                foreach (var date in allDates)
                {
                    foreach (var item in activeEmployee)
                    {
                        var attendacelistViewModel = new AttendaceListViewModel();
                        if (item.EsslId != 0)
                        {

                            var timeSheets = getAllTimeSheetlist.Where(x => x.Startdate.Date == date.Date && !x.IsDeleted).ToList();
                            var stringToNum = Convert.ToString(item.EsslId);

                            var firstInTime = attendanceReportDateModels.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                            var lastInTime = attendanceReportDateModels.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);

                            var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                            var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";

                            if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                            {
                                DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                                DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                                string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                                double seconds = TimeSpan.Parse(dt).TotalSeconds;
                                dt = seconds > 0 ? dt : dt = Constant.TimeFormatZero;
                                var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                                var burningHours = GetTimeSheetWorkHours(item.EmpId, timeSheets);
                                var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                                insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                                attendacelistViewModel.EmployeeId = item.EmpId;
                                attendacelistViewModel.UserName = item.UserName;
                                attendacelistViewModel.EmployeeName = item.FirstName + " " + item.LastName;
                                attendacelistViewModel.Status = item.IsDeleted;
                                attendacelistViewModel.Date = date.Date.ToString(Constant.DateFormat);
                                attendacelistViewModel.TotalHours = dt;
                                attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat);
                                attendacelistViewModel.InsideOffice = insideoffice;
                                attendacelistViewModel.BurningHours = burningHours;
                                attendacelistViewModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                                attendacelistViewModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                                attendacelistViewModel.Id = 0;
                                attendaceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
                            }
                        }
                    }
                }
                return attendaceListViewModels;
            }
            catch (Exception)
            {

            }
            return null;
        }

        /// <summary>
        /// Logic to Genarate Excel
        /// </summary> 
        /// <param name="attendaceListViewModels" ></param>
        public string GenarateExcel(List<AttendaceListViewModel> attendaceListViewModels)
        {
            var combinedPath = "";
            if (attendaceListViewModels.Count() > 0)
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Employee Attendance Details");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
                worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
                worksheet.Cell(currentRow, 3).Value = Constant.Date;
                worksheet.Cell(currentRow, 4).Value = Constant.EntryTime;
                worksheet.Cell(currentRow, 5).Value = Constant.ExitTime;
                worksheet.Cell(currentRow, 6).Value = Constant.TotalHours;
                worksheet.Cell(currentRow, 7).Value = Constant.BreakHours;
                worksheet.Cell(currentRow, 8).Value = Constant.ActualHours;
                worksheet.Cell(currentRow, 9).Value = Constant.TimeSheetHours;
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 9).Style.Font.Bold = true;

                foreach (var user in attendaceListViewModels)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                    worksheet.Cell(currentRow, 1).Value = user.UserName;
                    worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                    worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.Date);
                    worksheet.Cell(currentRow, 4).Value = Convert.ToString(user.EntryTime);
                    worksheet.Cell(currentRow, 5).Value = Convert.ToString(user.ExitTime);
                    worksheet.Cell(currentRow, 6).Value = user.TotalHours;
                    worksheet.Cell(currentRow, 7).Value = user.BreakHours;
                    worksheet.Cell(currentRow, 8).Value = user.InsideOffice;
                    worksheet.Cell(currentRow, 9).Value = user.BurningHours;
                }
                var fileName = string.Format("EmployeeAttendanceDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                var fileId = Guid.NewGuid().ToString() + "_" + fileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
                combinedPath = Path.Combine(path, fileNames);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    workbook.SaveAs(combinedPath);
                    memoryStream.Position = 0;
                    var content = memoryStream.ToArray();
                    //HttpContext.Session.Set(Constant.fileId, content);
                }
            }
            return combinedPath;
        }


        /// <summary>
        /// Logic to get send mail the employeeattendancedetails details 
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        /// <param name=Constant.fileId ></param>
        public async Task<bool> SendMail(AttendaceListViewModel attendaceListViewModel, List<string> fileId,int companyId)
        {
            var combinePath = new List<string>();
            var empIds = new List<int>();
            var result = false;
            if (attendaceListViewModel.EmployeeId == 0)
            {
                var draftTypeId = (int)EmailDraftType.AttendanceLogForManagement;
                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                var toEmail = _config.GetSection(Constant.vphospitalSupportEmailId).Value.ToString();
                var subject = emailDraftContentEntity.Subject;
                var mailBody = EmailBodyContent.SendEmail_Body_AttendanceForManagement(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, emailDraftContentEntity.DraftBody);
                var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
                var emailEntity = new EmailQueueEntity();
                emailEntity.FromEmail = emailSettingsEntity.FromEmail;
                emailEntity.ToEmail = toEmail;
                emailEntity.Subject = subject;
                emailEntity.Body = mailBody;
                emailEntity.CCEmail = emailDraftContentEntity.Email;

                foreach (var item in fileId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var fileName = item;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath.Add(Path.Combine(path, fileName));
                    }

                }
                var strSikillName = combinePath;
                if (strSikillName.Count() > 0)
                {
                    var str = string.Join(",", strSikillName);
                    emailEntity.Attachments = str.TrimEnd(',');
                }
                emailEntity.IsSend = false;
                emailEntity.Reason = Common.Constant.EmployeesAttendanceDetailsReason;
                emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                emailEntity.CreatedDate = DateTime.Now;
                result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
                return result;
            }
            else
            {
                var draftTypeId = (int)EmailDraftType.AttendanceLogEmployeeMonthly;
                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                var employee = await _employeesRepository.GetEmployeeById(attendaceListViewModel.EmployeeId, companyId);
                var employeename = employee.FirstName + " " + employee.LastName;
                var toEmail = employee.OfficeEmail;
                var subject = emailDraftContentEntity.Subject;
                var mailBody = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeMonth(employeename, DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString("MMM"), DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString("yyyy"), emailDraftContentEntity.DraftBody);
                var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
                var emailEntity = new EmailQueueEntity();
                emailEntity.FromEmail = emailSettingsEntity.FromEmail;
                emailEntity.ToEmail = toEmail;
                emailEntity.Subject = subject;
                emailEntity.Body = mailBody;
                emailEntity.CCEmail = emailDraftContentEntity.Email;

                foreach (var item in fileId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var fileName = item;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath.Add(Path.Combine(path, fileName));
                    }
                }
                var strSikillName = combinePath;
                if (strSikillName.Count() > 0)
                {
                    var str = string.Join(",", strSikillName);
                    emailEntity.Attachments = str.TrimEnd(',');
                }
                emailEntity.IsSend = false;
                emailEntity.Reason = Common.Constant.EmployeesAttendanceDetailsReason;
                emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                emailEntity.CreatedDate = DateTime.Now;
                result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
                return result;
            }
        }


        /// <summary>
        /// Logic to get send mail the employeeattendancedetails details 
        /// </summary>
        /// <param name="officeEmail" ></param>
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task<bool> InsertEmailAttendance(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, List<string> fileId)
        {
            var result = false;
            var combinePath = new List<string>();
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Constant.AttendanceLogReason;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;

            foreach (var item in fileId)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var fileName = item;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    combinePath.Add(Path.Combine(path, fileName));
                }
            }
            var strSikillName = combinePath;
            if (strSikillName.Count() > 0)
            {
                var str = string.Join(",", strSikillName);
                emailEntity.Attachments = str.TrimEnd(',');
            }

            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            return result;
        }

        /// <summary>
        /// Logic to get viewattendace filter the attendancedetails details 
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>        
        public async Task<AttendaceListViewModels> ViewAttendanceData(AttendaceListViewModel attendaceListViewModel,int sessionCompanyId)
        {
            try
            {
                var attendaceListViewModels = new AttendaceListViewModels();
                var employeeIdValue = attendaceListViewModel.EmployeeId;
                var employeeList = await _employeesRepository.GetAllEmployeeDetails(sessionCompanyId);
                var employees = await _employeesRepository.GetAllEmployees(sessionCompanyId);
                attendaceListViewModels.ViewAttendanceLog = new List<ViewAttendanceLog>();
                var userId = attendaceListViewModel.EmployeeId;
                var employeeLists = userId == 1 ? await _employeesRepository.GetAllEmployeeDetails(sessionCompanyId) : await _employeesRepository.GetAllEmpById(userId, sessionCompanyId);
                var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
                var dTo = string.IsNullOrEmpty(attendaceListViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString(Constant.DateFormat);
                var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
                var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
                var status = "";
                var companyId = Convert.ToString(attendaceListViewModel.CompanyId);

                var formDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
                var toDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
                List<DateTime> allDates = new List<DateTime>();

                for (DateTime dates = formDate; dates <= toDate; dates = dates.AddDays(1))
                {
                    allDates.Add(dates);
                }
                if (employeeIdValue == 0)
                {
                    var empId = Convert.ToString(employeeIdValue);
                    if (attendaceListViewModel?.EmployeeStatus != null)
                    {
                        status = Convert.ToString(attendaceListViewModel.EmployeeStatus);
                    }
                    else
                    {
                        status = Convert.ToString(0);
                    }

                    var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);
                    attendanceReportDateModels = attendanceReportDateModels.ToList();

                    foreach (var attendanceReportModel in attendanceReportDateModels)
                    {

                        var employeeEntity = await _employeesRepository.GetEmployeeByEssId(Convert.ToInt32(attendanceReportModel.EmployeeId),Convert.ToInt32(companyId));
                        if (employeeEntity != null)
                        {
                            attendaceListViewModels.ViewAttendanceLog.Add(new ViewAttendanceLog()
                            {
                                Id = attendanceReportModel.Id,
                                EmpId = Convert.ToInt32(attendanceReportModel.EmployeeId),
                                Employee = employeeEntity.EmpId,
                                EmployeeId = employeeEntity == null ? "" : employeeEntity.UserName,
                                EmployeeName = employeeEntity == null ? "" : employeeEntity.FirstName + " " + employeeEntity.LastName,
                                Status = employeeEntity == null ? false : employeeEntity.IsDeleted,
                                EmployeeCode = attendanceReportModel.EmployeeId,
                                LogDateTime = Convert.ToDateTime(attendanceReportModel.LogDateTime).ToString(Constant.DateTimeFormat),
                                LogDate = Convert.ToDateTime(attendanceReportModel.LogDateTime).ToString(Constant.DateFormat),
                                LogTime = Convert.ToDateTime(attendanceReportModel.LogDateTime).ToString(Constant.TimeFormatWithFullForm),
                                Direction = attendanceReportModel.Direction,
                            });
                        }
                    }
                    foreach (var data in allDates)
                    {
                        var checkdates = data.Date;
                        var fdate = checkdates.Date.ToString("dd/MM/yyyy");
                        var attendancereport = await GetAllAttendancereport(empId, fdate, fdate, tFrom, tTo, status, companyId);
                        var employeeEntitys = new EmployeesEntity();
                        if (attendancereport.Count() == 0)
                        {
                            var attendanceReportDatamodelList = await _dashboardRepository.GetLastTimeLogEntityByEmployeeIdDateList(data.Date, tFrom, tTo,attendaceListViewModel.CompanyId);
                            foreach (var item in attendanceReportDatamodelList)
                            {
                                if (attendaceListViewModel.EmployeeStatus == 1)
                                {
                                    var inActiveEmployees = employees.Where(x => x.IsDeleted).ToList();
                                    employeeEntitys = inActiveEmployees.FirstOrDefault(x => x.EmpId == Convert.ToInt32(item.EmployeeId));
                                }
                                else
                                {
                                    employeeEntitys = employeeList.FirstOrDefault(x => x.EmpId == Convert.ToInt32(item.EmployeeId));
                                }
                                if (employeeEntitys != null)
                                {
                                    attendaceListViewModels.ViewAttendanceLog.Add(new ViewAttendanceLog()
                                    {
                                        Id = item.EmployeeId,
                                        EmployeeId = employeeEntitys.UserName,
                                        EmployeeName = employeeEntitys.FirstName + " " + employeeEntitys.LastName,
                                        Status = employeeEntitys.IsDeleted,
                                        EmployeeCode = Convert.ToString(item.EmployeeId),
                                        LogDateTime = Convert.ToDateTime(item.CreatedDate).ToString(Constant.DateTimeFormat),
                                        LogDate = Convert.ToDateTime(item.CreatedDate).ToString(Constant.DateFormat),
                                        LogTime = Convert.ToDateTime(item.CreatedDate).ToString(Constant.TimeFormatWithFullForm),
                                        Direction = item.EntryStatus,

                                    });
                                }
                            }
                        }

                    }
                }
                else
                {
                    var employeeEntity = await _employeesRepository.GetEmployeeByIdView(attendaceListViewModel.EmployeeId, attendaceListViewModel.CompanyId);

                    if (employeeEntity != null && employeeEntity.EsslId > 0)
                    {
                        var empId = Convert.ToString(employeeEntity.EsslId);
                        status = Convert.ToString(employeeEntity.IsDeleted == false ? 0 : 1);

                        var attendanceReportDateModels = await GetAllAttendancereport(empId, dFrom, dTo, tFrom, tTo, status, companyId);
                        attendanceReportDateModels = attendanceReportDateModels.DistinctBy(x => x.LogDateTime).ToList();

                        if (tFrom == "" && tTo == "")
                        {
                            var listOfRec = attendanceReportDateModels.TakeLast(1);
                            var lastRecord = listOfRec.FirstOrDefault(x => x.Direction == Constant.EntryTypeIn);
                            var attendanceReport = new AttendanceReportDateModel();
                            if (lastRecord != null)
                            {
                                foreach (var record in listOfRec)
                                {
                                    var dateTime = Convert.ToDateTime(record.LogDateTime);
                                    if (dateTime.Date == DateTime.Now.Date)
                                    {
                                        attendanceReportDateModels.Add(new AttendanceReportDateModel()
                                        {
                                            Id = 0,
                                            EmployeeId = record.EmployeeId,
                                            LogDate = DateTime.Now.ToString("yyyy-MM-dd"),
                                            LogDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Direction = Constant.EntryTypeOut,
                                        });
                                    }
                                    else
                                    {
                                        attendanceReportDateModels.Add(new AttendanceReportDateModel()
                                        {
                                            Id = 0,
                                            EmployeeId = record.EmployeeId,
                                            LogDate = Convert.ToDateTime(record.LogDate).ToString("yyyy-MM-dd"),
                                            LogDateTime = Convert.ToDateTime(record.LogDate + " " + "11:59:59 PM").ToString("yyyy-MM-dd HH:mm:ss"),
                                            Direction = Constant.EntryTypeOut,
                                        });
                                    }
                                }
                            }
                        }

                        var BreakHours = "";
                        var InsideOffice = "";
                        var BurningHours = "";
                        var TotalHours = "";
                        var EntryTime = "";
                        var ExitTime = "";
                        var WorkingHours = "";

                        var getAllTimeSheetlist = await _timeSheetRepository.GetAllTimeSheet(employeeIdValue,Convert.ToInt32(companyId));
                        var filterViewAttendaces = _mapper.Map<List<FilterViewAttendace>>(attendanceReportDateModels);
                        var essl = Convert.ToInt16(employeeEntity?.EsslId);

                        var date = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
                        var stringToNum = Convert.ToString(essl);
                        var tableEmployeeId = essl;

                        //get time sheet data
                        var timeSheets = getAllTimeSheetlist.Where(x => x.StartTime.Date == date.Date && !x.IsDeleted && x.EmpId == employeeIdValue).ToList();
                        var firstInTime = filterViewAttendaces.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                        var lastInTime = filterViewAttendaces.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);
                        var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                        var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";

                        //get separte datas
                        if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                        {
                            double maximum = 28800;
                            DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                            DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                            string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                            dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                            var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                            var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                            insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                            var burningHours = GetTimeSheetWorkHours(employeeIdValue, timeSheets);

                            var seconds = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).TotalSeconds;

                            BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM) + " " + Constant.Hrs;
                            InsideOffice = insideoffice + " " + Constant.Hrs;
                            BurningHours = Convert.ToDateTime(burningHours).ToString(Constant.TimeFormat24HrsHM) + " " + Constant.Hrs;
                            TotalHours = Convert.ToDateTime(dt).ToString(Constant.TimeFormatForHM) + " " + Constant.Hrs;
                            EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.DateTimeFormatMonth);
                            ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.DateTimeFormatMonth);

                            double secs = Convert.ToDouble(seconds);
                            var percentage = secs == 0 ? 0 : Convert.ToInt16((secs / maximum) * 100);
                            WorkingHours = percentage.ToString();

                            attendaceListViewModels.TotalHours = TotalHours;
                            attendaceListViewModels.InsideOffice = InsideOffice;
                            attendaceListViewModels.BreakHours = BreakHours;

                            if (attendaceListViewModels.InsideOffice != null)
                            {
                                var time = insideoffice;
                                double totalseconds = TimeSpan.Parse(time).TotalSeconds;
                                attendaceListViewModels.TotalSecounds = Convert.ToInt64(totalseconds);
                            }
                        }
                        else
                        {
                            // Fetch data from TimeLogger if firstLoginDate or lastLoginDate is empty
                            var timeLogEntitys = await _dashboardRepository.GetTimeLogEntityByEmpId(employeeIdValue,date,Convert.ToInt32(companyId));

                            if (timeLogEntitys.Any())
                            {
                                tFrom = timeLogEntitys.Select(x => Convert.ToString(x.ClockInTime)).FirstOrDefault().ToString();
                                tTo = timeLogEntitys.Where(x => x.ClockOutTime.HasValue && x.EntryStatus == Constant.EntryTypeOut)
                                                    .Select(x => x.ClockOutTime.Value.ToString()).LastOrDefault();

                                // Fetch attendance report data model list from the dashboard service
                                var attendanceReportDatamodelList = await _dashboardService.GetTimeLog(employeeIdValue,date,Convert.ToInt32(companyId));
                                double maximum = 28800;
                                DateTime StartTime = Convert.ToDateTime(tFrom);
                                DateTime EndTime = Convert.ToDateTime(tTo);
                                string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                                dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                                var seconds = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(attendanceReportDatamodelList.BreakHours)).TotalSeconds;
                                EntryTime = Convert.ToDateTime(tFrom).ToString(Constant.DateTimeFormatMonth);
                                // Assuming tTo is defined and might be null
                                ExitTime = string.IsNullOrEmpty(tTo)? StartTime.Date.AddHours(23).AddMinutes(59).ToString(Constant.DateTimeFormatMonth): Convert.ToDateTime(tTo).ToString(Constant.DateTimeFormatMonth);
                                var burningHours = GetTimeSheetWorkHours(employeeIdValue, timeSheets);
                                double secs = Convert.ToDouble(seconds);
                                var percentage = secs == 0 ? 0 : Convert.ToInt16((secs / maximum) * 100);
                                WorkingHours = percentage.ToString();
                                var breakHours = GetBreakHourForTimeLogger(Convert.ToString(employeeIdValue), timeLogEntitys, date);
                                BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM) + " " + Constant.Hrs;
                                foreach (var timeLogEntity in timeLogEntitys)
                                {
                                    attendaceListViewModels.ViewAttendanceLog.Add(new ViewAttendanceLog()
                                    {
                                        Id = timeLogEntity.TimeLoggerId,
                                        EmployeeId = employeeEntity.UserName,
                                        EmployeeName = employeeEntity.FirstName + " " + employeeEntity.LastName,
                                        Status = employeeEntity.IsDeleted,
                                        EmployeeCode = Convert.ToString(timeLogEntity.EmployeeId),
                                        LogDateTime = Convert.ToDateTime(timeLogEntity.CreatedDate).ToString(Constant.DateTimeFormat),
                                        LogDate = Convert.ToDateTime(timeLogEntity.CreatedDate).ToString(Constant.DateFormat),
                                        LogTime = Convert.ToDateTime(timeLogEntity.CreatedDate).ToString(Constant.TimeFormatWithFullForm),
                                        Direction = timeLogEntity.EntryStatus,
                                        BreakHours = breakHours == string.Empty ? null : breakHours,
                                        TotalHours = attendanceReportDatamodelList.TotalHours == string.Empty ? null : attendanceReportDatamodelList.TotalHours,
                                        InsideOffice = attendanceReportDatamodelList.InsideOffice == string.Empty ? null : attendanceReportDatamodelList.InsideOffice,
                                        BurningHours = burningHours == string.Empty ? null : burningHours,
                                        EntryTime = Convert.ToString(EntryTime) == string.Empty ? null : Convert.ToString(EntryTime),
                                        ExitTime = ExitTime == string.Empty ? null : ExitTime,
                                        WorkingHours = WorkingHours == string.Empty ? 0 : Convert.ToInt32(WorkingHours),
                                    });
                                }
                                // Set attendance list view model properties
                                attendaceListViewModels.TotalHours = attendanceReportDatamodelList.TotalHours;
                                attendaceListViewModels.InsideOffice = attendanceReportDatamodelList.InsideOffice;
                                attendaceListViewModels.BreakHours = breakHours;
                                // Order the TimeLogView list by EmpId
                                var attendances = attendaceListViewModels.ViewAttendanceLog.OrderBy(x => x.EmployeeId).ToList();
                                attendaceListViewModels.ViewAttendanceLog = attendances;

                                return attendaceListViewModels;
                            }
                        }
                            //get In/Out Details List
                            foreach (var attendanceReportModel in attendanceReportDateModels)
                            {
                                attendaceListViewModels.ViewAttendanceLog.Add(new ViewAttendanceLog()
                                {
                                    Id = attendanceReportModel.Id,
                                    EmployeeId = employeeEntity == null ? "" : employeeEntity.UserName,
                                    Employee = employeeEntity.EmpId,
                                    EmployeeName = employeeEntity == null ? "" : employeeEntity.FirstName + " " + employeeEntity.LastName,
                                    Status = employeeEntity == null ? false : employeeEntity.IsDeleted,
                                    EmployeeCode = attendanceReportModel.EmployeeId,
                                    LogDateTime = Convert.ToDateTime(attendanceReportModel.LogDateTime).ToString(Constant.DateTimeFormat),
                                    LogDate = Convert.ToDateTime(attendanceReportModel.LogDateTime).ToString(Constant.DateFormat),
                                    LogTime = Convert.ToDateTime(attendanceReportModel.LogDateTime).ToString(Constant.TimeFormatWithFullForm),
                                    Direction = attendanceReportModel.Direction,
                                    BreakHours = BreakHours == string.Empty ? null : BreakHours,
                                    TotalHours = TotalHours == string.Empty ? null : TotalHours,
                                    InsideOffice = InsideOffice == string.Empty ? null : InsideOffice,
                                    BurningHours = BurningHours == string.Empty ? null : BurningHours,
                                    EntryTime = EntryTime == string.Empty ? null : EntryTime,
                                    ExitTime = ExitTime == string.Empty ? null : ExitTime,
                                    WorkingHours = WorkingHours == string.Empty ? 0 : Convert.ToInt32(WorkingHours),

                            });
                        }
                        foreach (var data in allDates)
                        {

                            var checkdates = data.Date;
                            var fdate = checkdates.Date.ToString("dd/MM/yyyy");

                            var attendancereport = await GetAllAttendancereport(empId, fdate, fdate, tFrom, tTo, status, companyId);

                            if (attendancereport.Count() == 0)
                            {

                                var attendanceReportDatamodelList = await _dashboardRepository.GetLastTimeLogEntityByEmployeeIdDate(attendaceListViewModel.EmployeeId, data.Date, tFrom, tTo,attendaceListViewModel.CompanyId);

                                foreach (var item in attendanceReportDatamodelList)
                                {

                                    employeeEntity = employeeLists.FirstOrDefault(x => x.EmpId == Convert.ToInt32(item.EmployeeId));
                                    if (employeeEntity != null)
                                    {

                                        attendaceListViewModels.ViewAttendanceLog.Add(new ViewAttendanceLog()
                                        {
                                            Id = item.EmployeeId,
                                            EmployeeId = employeeEntity.UserName,
                                            EmployeeName = employeeEntity.FirstName + " " + employeeEntity.LastName,
                                            Status = employeeEntity.IsDeleted,
                                            EmployeeCode = Convert.ToString(item.EmployeeId),
                                            LogDateTime = Convert.ToDateTime(item.CreatedDate).ToString(Constant.DateTimeFormat),
                                            LogDate = Convert.ToDateTime(item.CreatedDate).ToString(Constant.DateFormat),
                                            LogTime = Convert.ToDateTime(item.CreatedDate).ToString(Constant.TimeFormatWithFullForm),
                                            Direction = item.EntryStatus,

                                        });
                                    }
                                }

                                if (allDates.Count() == 1)
                                {
                                    double breaks = 0;
                                    double totalHours = 0;

                                    foreach (var item in attendanceReportDatamodelList)
                                    {
                                        if (attendanceReportDatamodelList.Count() > 0)
                                        {
                                            totalHours += item.LogSeconds;
                                        }
                                    }
                                    var breakHours = attendanceReportDatamodelList.Where(x => x.EmployeeId == employeeEntity.EmpId && x.EntryStatus == Constant.EntryTypeOut).ToList();
                                    foreach (var items in breakHours)
                                    {
                                        breaks += items.LogSeconds;
                                    }

                                    var InsideOfficehours = totalHours - breaks;
                                    InsideOffice = await GetHours(InsideOfficehours);
                                    BreakHours = await GetHours(breaks);
                                    TotalHours = await GetHours(totalHours);
                                    attendaceListViewModels.TotalHours = TotalHours;
                                    attendaceListViewModels.InsideOffice = InsideOffice;
                                    attendaceListViewModels.BreakHours = BreakHours;

                                }
                            }
                        }
                    }
                }
                var attendance = attendaceListViewModels.ViewAttendanceLog.OrderBy(x => x.EmpId).ToList();
                attendaceListViewModels.ViewAttendanceLog = attendance;

                return attendaceListViewModels;
            }
            catch (Exception ex) { throw ex; }

        }

        /// <summary>
        /// Logic to get status filter the attendancedetails details 
        /// </summary>
        /// <param name="statusId" ></param> 
        public async Task<List<EmployeeDropdown>> GetByStatusId(int statusId,int companyId)
        {
            var staus = statusId == 0 ? false : true;
            var listEmployee = await _employeesRepository.GetByStatus(staus,companyId);
            var employeeDropdowns = new List<EmployeeDropdown>();
            if (listEmployee != null)
            {
                var employeeDropdown1 = new EmployeeDropdown();
                employeeDropdown1.EmployeeId = 0;
                employeeDropdown1.EmployeeIdWithName = Common.Constant.AllEmployees;
                employeeDropdowns.Add(employeeDropdown1);
                foreach (var item in listEmployee)
                {
                    var employeeDropdown = new EmployeeDropdown();
                    employeeDropdown.EmployeeId = item.EmpId;
                    employeeDropdown.EmployeeName = item.UserName;
                    employeeDropdown.EmployeeIdWithName = item.UserName + Constant.Hyphen + item.FirstName + " " + item.LastName;
                    employeeDropdowns.Add(employeeDropdown);
                }
            }
            return employeeDropdowns;
        }

        /// <summary>
        /// Logic to get Hours  
        /// </summary>
        /// <param name="totalSeconds" ></param> 
        public async Task<string> GetHours(double totalSeconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
            var hour = (t.Days * 24 + t.Hours).ToString("D2");
            var answer = Convert.ToString(hour + ":" + t.Minutes.ToString("D2") + ":" + t.Seconds.ToString("D2"));
            return answer;
        }
    }
}
