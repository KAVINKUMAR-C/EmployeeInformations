using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using EmployeeInformations.Model;

namespace EmployeeInformations.Business.Service
{
    public class ReportService : IReportService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IProjectDetailsRepository _projectDetailsRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        public ReportService(ILeaveRepository leaveRepository, IEmployeesRepository employeesRepository, IReportRepository reportRepository, IMapper mapper, IProjectDetailsRepository projectDetailsRepository, ITimeSheetRepository timeSheetRepository)
        {
            _leaveRepository = leaveRepository;
            _employeesRepository = employeesRepository;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _projectDetailsRepository = projectDetailsRepository;
            _timeSheetRepository = timeSheetRepository;
        }


        //Leave Report

        /// <summary>
        /// Logic to get employees list
        /// </summary>        
        public async Task<List<EmployeeDropdown>> GetAllEmployeesDrropdown(int companyId)
        {
            var employeeDropdowns = new List<EmployeeDropdown>();
            employeeDropdowns = await _reportRepository.GetAllEmployeesDropdown(companyId);
            return employeeDropdowns;
        }
        public async Task<List<EmployeeDropdown>> GetEmpDropDown(int id,int companyId)
        {
            var employeeDropdowns = new List<EmployeeDropdown>();
            var employeeIds = await _employeesRepository.GetAllEmployeeIdsReportingPersonForLeave(id,companyId);

            var empIds = new List<int> { id };
            empIds.AddRange(employeeIds.Select(e => e.EmployeeId));

            var teamList = employeeIds.Count > 0
                ? await _employeesRepository.GetTeambyId(empIds,companyId)
                : await _employeesRepository.GetAllEmpById(id, companyId);

            if (teamList != null)
            {
                employeeDropdowns.AddRange(teamList.Select(item => new EmployeeDropdown
                {
                    EmployeeId = item.EmpId,
                    EmployeeName = item.UserName,
                    EmployeeIdWithName = $"{item.UserName} {Constant.Hyphen} {item.FirstName} {item.LastName}"
                }));
            }


            return employeeDropdowns;
        }

        /// <summary>
        /// Logic to get year list using attendance
        /// </summary> 
        public async Task<List<Years>> GetYear()
        {
            var startYear = DateTime.Now.AddYears(-4).Year;
            var currentYear = DateTime.Now.Year;

            var employeeDropdowns = Enumerable.Range(startYear, currentYear - startYear + 1)
                .Select(year => new Years
                {
                    Year = year,
                    StrYear = year.ToString()
                }).ToList();

            return employeeDropdowns;
        }

        /// <summary>
        /// Logic to get leavetype list
        /// </summary> 
        public async Task<List<LeaveTypes>> GetAllLeave()
        {
            var leaveTypes = await _leaveRepository.GetAllLeaveTypes();
            return leaveTypes;
        }

        public async Task<List<FilterViewEmployeeLeave>> GetAllReportsLeave()
        {
            var reportLeave = new List<FilterViewEmployeeLeave>();
            var report = new Reports();
            //var employees = await _employeesRepository.GetAllEmployees();
            // var isActiveEmployees = new List<EmployeesEntity>();
            var empId = Constant.ZeroStr;
            var leaveTypeId = Constant.ZeroStr;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
            var StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
            //isActiveEmployees = employees.Where(x => !x.IsDeleted).ToList();
            var leaveReportDateModels = await GetAllLeaveReport(empId, leaveTypeId, StartDate, EndDate);
            foreach (var leaveReport in leaveReportDateModels)
            {
                reportLeave.Add(new FilterViewEmployeeLeave()
                {
                    EmployeeSortName = leaveReport.EmployeeSortName,
                    EmployeeUserId = leaveReport.EmployeeUserId,
                    FirstName = leaveReport.FirstName,
                    LastName = leaveReport.LastName,
                    LeaveType = leaveReport.LeaveType,
                    LeaveFromDate = leaveReport.LeaveFromDate,
                    LeaveToDate = leaveReport.LeaveToDate,
                    LeaveCount = leaveReport.LeaveCount,
                });
            }
            return reportLeave;
        }
        /// <summary>
        /// Logic to get all the leavereports list
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        public async Task<List<FilterViewEmployeeLeave>>GetAllEmployeeLeaveReports(SysDataTablePager pager, string columnDirection, string columnName)
        {
            var reportLeave = new List<FilterViewEmployeeLeave>();
            var report = new Reports();
            var empId = Constant.ZeroStr;
            var leaveTypeId = Constant.ZeroStr;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
            var StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
            report.LeaveFromDate = StartDate;
            report.LeaveToDate=EndDate;
            report.EmployeeId = Convert.ToInt32(empId);
            report.LeaveTypeId = Convert.ToInt32(leaveTypeId);
            var result = await _reportRepository.GetLeaveReportByEmployeeId(pager, report, columnDirection, columnName);
            var employeeLeave = _mapper.Map<List<FilterViewEmployeeLeave>>(result);
            foreach (var item in employeeLeave)
            {

                var firstLetterFirstName = string.IsNullOrEmpty(item.EmployeeName) ? "" : item.EmployeeName.Substring(0, 1);
                item.EmployeeSortName = firstLetterFirstName;
                item.LeaveFromDate = item.LeaveFromDate.Date;
            }
            return employeeLeave;

        }

        public async Task<List<FilterViewEmployeeLeave>> GetAllLeaveReport(string empId, string leaveTypeId, string StartDate, string EndDate)
        {
            var dFrom = string.IsNullOrEmpty(StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(StartDate).ToString(Constant.DateFormatMDY);
            var dTo = string.IsNullOrEmpty(EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(EndDate).ToString(Constant.DateFormatMDY);
            var leaveTypeIds = Convert.ToString(leaveTypeId);
            var empIds = Convert.ToString(empId);
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@empId", empIds),
                new KeyValuePair<string, string>("@leaveTypeId",leaveTypeIds),
                new KeyValuePair<string, string>("@startDate",dFrom),
                new KeyValuePair<string, string>("@endDate",dTo),
            };
            var LeaveReportDateModel = await _reportRepository.GetAllEmployessByLeaveTypeFilter("spGetLeaveTypeCountByEmployeeFilterData", p);
            var result = _mapper.Map<List<FilterViewEmployeeLeave>>(LeaveReportDateModel);
            foreach (var item in result)
            {
                item.EmployeeSortName = Common.Common.GetEmployeeSortName(item.FirstName, item.LastName);
            }
            return result;
        }


        /// <summary>
        /// Logic to get filter employee leave list 
        /// </summary> 
        /// <param name="reports" ></param>  
        public async Task<List<FilterViewEmployeeLeave>> GetAllEmployessByLeaveTypeFilter(Reports reports,int companyId)
        {
            var dFrom = string.IsNullOrEmpty(reports.LeaveFromDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(reports.LeaveFromDate).ToString(Constant.DateFormatMDY);
            var dTo = string.IsNullOrEmpty(reports.LeaveToDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(reports.LeaveToDate).ToString(Constant.DateFormatMDY);
            var leaveTypeId = Convert.ToString(reports.LeaveTypeId);
            var empId = Convert.ToString(reports.EmployeeId);
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@empId", empId),
                new KeyValuePair<string, string>("@leaveTypeId",leaveTypeId),
                new KeyValuePair<string, string>("@startDate",dFrom),
                new KeyValuePair<string, string>("@endDate",dTo),
                new KeyValuePair<string, string>("@status", Convert.ToString(reports.EmployeeStatus))

            };
            var LeaveReportDateModel = await _reportRepository.GetAllEmployessByLeaveTypeFilter("spGetLeaveTypeCountByEmployeeFilterData", p);
            var leaveFromDates = (reports.FromOrder == "desc" && reports.FromDatecol == 3) ? LeaveReportDateModel.OrderByDescending(x => x.LeaveFromDate) : LeaveReportDateModel.OrderBy(x => x.LeaveFromDate);
            var leaveToDates = ((reports.FromOrder == "desc") && (reports.FromDatecol == 4)) ? LeaveReportDateModel.OrderByDescending(x => x.LeaveToDate) : LeaveReportDateModel.OrderBy(x => x.LeaveToDate);
            var result = reports.FromDatecol == 3 ? _mapper.Map<List<FilterViewEmployeeLeave>>(leaveFromDates) : _mapper.Map<List<FilterViewEmployeeLeave>>(leaveToDates);
            var activeEmp = await _employeesRepository.GetAllEmployees(companyId);
            foreach (var item in result)
            {
                var employee = activeEmp.FirstOrDefault(e => e.EmpId == item.EmployeeId);
                item.EmployeeSortName = Common.Common.GetEmployeeSortName(item.FirstName, item.LastName);
                item.IsDeleted = employee == null ? false : employee.IsDeleted;
            }

            return result;
        }

        //Timesheet Report

        /// <summary>
        /// Logic to get filter timesheet list 
        /// </summary> 
        /// <param name="timeSheetReports" ></param>
        public async Task<TimeSheetReports> GetAllEmployessByTimeSheetFilter(TimeSheetReports timeSheetReports, int companyId)
        {
            var manualLogReport = new TimeSheetReports();
            manualLogReport.FilterViewTimeSheet = new List<FilterViewTimeSheet>();
            var dFrom = string.IsNullOrEmpty(timeSheetReports.StartTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartTime).ToString(Constant.DateFormat);
            var dTo = string.IsNullOrEmpty(timeSheetReports.EndTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.EndTime).ToString(Constant.DateFormat);
            var empId = Convert.ToString(timeSheetReports.EmployeeId);
            var projectId = Convert.ToString(timeSheetReports.ProjectId);
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@empId", empId),
                new KeyValuePair<string, string>("@projectId",projectId),
                new KeyValuePair<string, string>("@startTime",dFrom),
                new KeyValuePair<string, string>("@endTime",dTo),
            };
            var getallProject = await _projectDetailsRepository.GetAllProjectDetails(companyId);
            var LeaveReportDateModel = await _reportRepository.GetAllEmployessByTimeSheetFilter("spGetTimeSheetByEmployeeFilterData", p);
            var timesheetReport = LeaveReportDateModel.Where(x => x.CompanyId == companyId).ToList();

            var reportTimeSheet = (timeSheetReports.StartDateOrder == "desc" && timeSheetReports.StartDatecol == 4) ? timesheetReport.OrderByDescending(x => x.StartDate) : timesheetReport.OrderBy(x => x.StartDate);


            foreach (var item in reportTimeSheet)
            {
                var projectName = getallProject.FirstOrDefault(x => x.ProjectId == item.ProjectId).ProjectName;
                manualLogReport.FilterViewTimeSheet.Add(new FilterViewTimeSheet()
                {

                    Id = item.Id,
                    EmployeeId = item.EmployeeId,
                    StartDate = item.StartDate,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ProjectId = item.ProjectId,
                    ProjectName = getallProject.FirstOrDefault(x => x.ProjectId == item.ProjectId).ProjectName == null ? "" : getallProject.FirstOrDefault(x => x.ProjectId == item.ProjectId).ProjectName,
                    TaskName = item.TaskName,
                    TaskDescription = item.TaskDescription,
                    Status = item.Status,
                    AttachmentFileName = string.IsNullOrEmpty(item.AttachmentFileName) ? "" : item.AttachmentFileName,
                    AttachmentFilePath = string.IsNullOrEmpty(item.AttachmentFilePath) ? "" : item.AttachmentFilePath,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    EmployeeUserId = item.EmployeeUserId,
                });
            }
            return manualLogReport;
        }
        /// <summary>
        /// Logic to get TimeSheetReport Filter count
        /// <param name="timeSheetReports,pager" ></param>  
        public async Task<int> GetFilterTimeSheetReportCount(TimeSheetReports timeSheetReports, SysDataTablePager pager)
        {
            var timeSheetReportCount = await _reportRepository.GetFilterTimeSheetReportCount(timeSheetReports, pager);
            return timeSheetReportCount;
        }
        /// <summary>
        /// Logic to get TimeSheetReport Filter data
        /// <param name="timeSheetReports,pager,columnName,columnDirection" ></param>  
        public async Task<TimeSheetReports> GetAllEmployessByTimeSheetFilters(TimeSheetReports timeSheetReports, SysDataTablePager pager,string columnName, string columnDirection)
        {
            var reportFilterTimeSheet = new TimeSheetReports();
            reportFilterTimeSheet.timeSheetReport = await _reportRepository.GetTimeSheetFilter(timeSheetReports, pager,columnName,columnDirection);
            return reportFilterTimeSheet;
        }
        /// <summary>
        /// Logic to get project details list by particular empId
        /// </summary> 
        /// <param name="empId" ></param>  
        public async Task<List<ProjectNames>> GetAllProjectNames(int empId, int companyId)
        {
            var listofProjectNames = new List<ProjectNames>();
            if (empId == 0)
            {
                listofProjectNames = await _reportRepository.GetAllProjectNames(empId,companyId);
            }
            else
            {
                listofProjectNames = await _projectDetailsRepository.GetProjectByEmpId(empId, companyId);
            }
            return listofProjectNames;
        }

        /// <summary>
        /// Logic to get dropdown project details list by particular employeeIds
        /// </summary> 
        /// <param name="employeeIds" ></param>  
        public async Task<DropdownProjects> GetByEmployeeIdForProject(string employeeIds, int companyId)
        {
            var dropdownProjects = new DropdownProjects();
            dropdownProjects.ProjectNames = new List<ProjectNames>();
            var reportingEmpIds = new List<int>();
            // count of id
            if (employeeIds == Constant.ZeroStr)
            {
                var listofProjects = await _timeSheetRepository.GetAllProjectNames(Convert.ToInt32(employeeIds),companyId);
                var employeeDropdown1 = new ProjectNames();
                employeeDropdown1.ProjectId = 0;
                employeeDropdown1.ProjectName = Common.Constant.AllProjects;
                dropdownProjects.ProjectNames.Add(employeeDropdown1);
                foreach (var item in listofProjects)
                {
                    var leaveType = new ProjectNames();
                    leaveType.ProjectId = item.ProjectId;
                    leaveType.ProjectName = item.ProjectName;
                    dropdownProjects.ProjectNames.Add(leaveType);
                }
            }

            else if (employeeIds != Constant.ZeroStr)
            {
                var splitEmpId = employeeIds.Split(',');
                foreach (var item in splitEmpId)
                {
                    var value = Convert.ToInt32(item);
                    var ids = await _projectDetailsRepository.GetByEmployeeIdForProject(value, companyId);
                    if (ids != null)
                        reportingEmpIds.AddRange(ids);
                }
                if (reportingEmpIds.Count() > 0)
                {
                    // get id ,name      
                    var employeeDropdown1 = new ProjectNames();
                    employeeDropdown1.ProjectId = 0;
                    employeeDropdown1.ProjectName = Common.Constant.AllProjects;
                    dropdownProjects.ProjectNames.Add(employeeDropdown1);
                    foreach (var emp in reportingEmpIds)
                    {
                        var project = await _projectDetailsRepository.GetByProjectId(emp, companyId);

                        dropdownProjects.ProjectNames.Add(new ProjectNames()
                        {
                            ProjectId = project.ProjectId,
                            ProjectName = project.ProjectName,
                        });

                    }
                    var projects = dropdownProjects.ProjectNames.Distinct().ToList();
                    dropdownProjects.ProjectNames = projects.ToList();
                }
                else
                {
                    var projectsList = new List<ProjectNames>();
                    projectsList.Add(new ProjectNames()
                    {
                        ProjectId = 0,
                        ProjectName = Common.Constant.AllProjects,
                    });
                    dropdownProjects.ProjectNames = projectsList.ToList();
                }
            }
            return dropdownProjects;
        }

        //ManualLog Report

        /// <summary>
        /// Logic to get ManualLog detail the list 
        /// </summary>
        public async Task<List<ManualLog>> GetAllManualLog()
        {
            var manualLogs = await _reportRepository.GetAllManulLogs();
            return manualLogs;
        }

        /// <summary>
        /// Logic to get filter manualLog list 
         /// </summary> 
        /// <param name="manualLogReports" ></param>  
        public async Task<ManualLogReports> GetAllEmployessByManualLogFilter(ManualLogReports manualLogReports)
        {
            var manualLogReport = new ManualLogReports();
            manualLogReport.ManualLog = new List<FilterViewManualLog>();
            var dFrom = string.IsNullOrEmpty(manualLogReports.StartTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(manualLogReports.StartTime).ToString(Constant.DateFormatMDY);
            var dTo = string.IsNullOrEmpty(manualLogReports.EndTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(manualLogReports.EndTime).ToString(Constant.DateFormatMDY);
            var empId = Convert.ToString(manualLogReports.EmployeeId);
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@empId", empId),
                new KeyValuePair<string, string>("@startTime",dFrom),
                new KeyValuePair<string, string>("@endTime",dTo),
            };

            var LeaveReportDateModel = await _reportRepository.GetAllEmployessByManualLogFilter("spGetManualLogByEmployeeFilterData", p);
            var result = _mapper.Map<List<FilterViewManualLog>>(LeaveReportDateModel);
            manualLogReport.ManualLog = result;
            return manualLogReport;
        }

        /// <summary>
        /// Logic to get filter ImoprtFile the ManualLog detail  by particular ManualLog ImoprtFile
        /// </summary>
        /// <param name="manualLog" ></param>
        public async Task<bool> ImoprtFile(List<ManualLog> manualLog)
        {
            var result = false;
            try
            {

                if (manualLog.Count > 0)
                {
                    var manualLogEntitys = new List<ManualLogEntity>();
                    foreach (var manualLogEntity in manualLog)
                    {
                        manualLogEntitys.Add(new ManualLogEntity()
                        {
                            Sno = manualLogEntity.Sno,
                            UserName = manualLogEntity.UserName,
                            StartTime = Convert.ToDateTime(manualLogEntity.StartTime),
                            EndTime = Convert.ToDateTime(manualLogEntity.StartTime),
                            EntryStatus = manualLogEntity.EntryStatus,
                            TotalHours = manualLogEntity.TotalHours,
                            BreakHours = manualLogEntity.BreakHours,
                            EmpId = manualLogEntity.EmpId

                        });

                    }
                    result = await _reportRepository.ImoprtFile(manualLogEntitys);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Logic to get all the leavereports using filter list
        /// </summary>
        /// <param name="pager,reports,columnName,columnDirection" ></param>
        public async Task<List<FilterViewEmployeeLeave>> GetAllEmployessByLeaveTypeFilterData(SysDataTablePager pager, Reports reports, string columnDirection,string columnName,int companyId)
        {
           
            var result = await _reportRepository.GetLeaveReportByEmployeeId(pager, reports,columnDirection,columnName);
            var employeeLeave = _mapper.Map<List<FilterViewEmployeeLeave>>(result);
            var activeEmp = await _employeesRepository.GetAllEmployees(companyId);

            foreach (var item in employeeLeave)
            {
                var employee = activeEmp.FirstOrDefault(e => e.EmpId == item.EmployeeId);

                var firstLetterFirstName = string.IsNullOrEmpty(item.EmployeeName) ? "" : item.EmployeeName.Substring(0, 1);
                item.EmployeeSortName = firstLetterFirstName;
                item.IsDeleted = employee == null ? false : employee.IsDeleted;
            }
            return employeeLeave;

        }

        /// <summary>
        /// Logic to get all employees count
        /// </summary>
        /// <param name="pager" ></param>
        public async Task<int> GetAllEmployeesListCount(SysDataTablePager pager, int companyId)
        {
            var employeesCount = await _reportRepository.GetAllEmployeesListCount(pager, companyId);
            return employeesCount;
        }

        /// <summary>
        /// Logic to get all employees details list 
        /// </summary>
        /// <param name="pager" ></param>
        public async Task<EmployeesListViewModel>GetAllEmployeesList(SysDataTablePager pager, string columnDirection, string ColumnName,int companyId)
        {
            var employeesListViewModel = new EmployeesListViewModel();
            employeesListViewModel.EmployeesDataModel = await _reportRepository.GetAllEmployeesList(pager, columnDirection, ColumnName,companyId);
            return employeesListViewModel;
        }

        /// <summary>
        /// Logic to get all employees details list 
        /// </summary>
        /// <param name="companyId" ></param>
        public async Task<EmployeesListViewModel> GetAllEmployeesListPdf(int companyId)
        {
            var employeesListViewModel = new EmployeesListViewModel();
            var companyid = Convert.ToString(companyId);
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {                 
                 new KeyValuePair<string, string>("@companyId",companyid),
            };
            employeesListViewModel.EmployeesDataModel = await _reportRepository.GetAllEmployeeFilter("spGetAllEmployeesFilterDataPdf", p);
            return employeesListViewModel;
           

        }
        /// <summary>
        /// Logic to Get All EmployessByLeaveType FilterData Count 
        /// </summary>
        /// <param name="pager,reports" ></param>
        public async Task<int> GetAllEmployessByLeaveTypeFilterDataCount(SysDataTablePager pager, Reports reports)
        {

            var employeeLeaveCount = await _reportRepository.GetLeaveReportByEmployeeIdCount(pager, reports);

            return employeeLeaveCount;
        }

        public async Task<int> GetAllEmployessCount(SysDataTablePager pager)
        {
            var report = new Reports();
            var empId = Constant.ZeroStr;
            var leaveTypeId = Constant.ZeroStr;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
            var StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
            report.LeaveFromDate = StartDate;
            report.LeaveToDate = EndDate;
            report.EmployeeId = Convert.ToInt32(empId);
            report.LeaveTypeId = Convert.ToInt32(leaveTypeId);

            var employeeLeaveCount = await _reportRepository.GetLeaveReportByEmployeeIdCount(pager, report);

            return employeeLeaveCount;
        }
    }
}
