using EmployeeInformations.Common;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public ReportRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// LeaveReportDateModel

        /// <summary>
          /// Logic to get filter the leavereportdatemodel detail by particular employee leave
         /// </summary>         
        /// <param name="proc" ></param>
        /// <param name="values" ></param>       
        public async Task<List<LeaveReportDateModel>> GetAllEmployessByLeaveTypeFilter(string proc, List<KeyValuePair<string, string>> values)
        {
            var parameters = new object[values.Count];
            for (int i = 0; i < values.Count; i++)
                parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

            var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
            paramnames = paramnames.TrimEnd(',');
            proc = proc + " " + paramnames;


            var leaveReportDateModel = await _dbContext.LeaveReportDateModel.FromSqlRaw<LeaveReportDateModel>(proc, parameters).ToListAsync();
            return leaveReportDateModel;
        }

        ////ManualLogReportDateModel

        /// <summary>
        /// Logic to get filter the manuallogreportdatemodel detail by particular employee leave
        /// </summary>         
        /// <param name="proc" ></param>
        /// <param name="values" ></param> 
        public async Task<List<ManualLogReportDateModel>> GetAllEmployessByManualLogFilter(string proc, List<KeyValuePair<string, string>> values)
        {

            var parameters = new object[values.Count];
            for (int i = 0; i < values.Count; i++)
                parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

            var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
            paramnames = paramnames.TrimEnd(',');
            proc = proc + " " + paramnames;

            var leaveReportDateModel = await _dbContext.ManualLogReportDateModel.FromSqlRaw<ManualLogReportDateModel>(proc, parameters).ToListAsync();
            return leaveReportDateModel;

        }

        ////  TimeSheetDataModel

        /// <summary>
         /// Logic to get filter the timesheet detail by particular employee 
        /// </summary>         
        /// <param name="proc" ></param>
        /// <param name="values" ></param> 
        public async Task<List<TimeSheetDataModel>> GetAllEmployessByTimeSheetFilter(string proc, List<KeyValuePair<string, string>> values)
        {
            try
            {
                var parameters = new object[values.Count];
                for (int i = 0; i < values.Count; i++)
                    parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

                var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
                paramnames = paramnames.TrimEnd(',');
                proc = proc + " " + paramnames;

                var leaveReportDateModel = await _dbContext.TimeSheetDataModel.FromSqlRaw<TimeSheetDataModel>(proc, parameters).ToListAsync();
                return leaveReportDateModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
         /// Logic to get filter the timesheetreport count 
        /// </summary>      
        /// <param name="timeSheetReports,pager" ></param> 
        public async Task<int> GetFilterTimeSheetReportCount(TimeSheetReports timeSheetReports, SysDataTablePager pager)
        {
            try            
            {
                var lastDate = DateTime.Today.AddDays(-7).Date;
                var today = DateTime.Now.Date;

                var dFrom = "";
                var dTo = "";
                if (string.IsNullOrEmpty(timeSheetReports.StartTime))
                {
                    dFrom = lastDate.ToString(Constant.DateFormatMDY);
                    dTo = today.ToString(Constant.DateFormatMDY);
                }
                else
                {
                     dFrom = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartTime).ToString(Constant.DateFormatMDY);
                     dTo = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.EndTime).ToString(Constant.DateFormatMDY);
                }

                var result = 0;                              
                
                var empId = Convert.ToString(timeSheetReports.EmployeeId);
                var projectId = Convert.ToString(timeSheetReports.ProjectId);         
                var _params = new
                {
                    SearchText = pager.sSearch
                };

                var param = new NpgsqlParameter("@empId", timeSheetReports.EmployeeId);
                var param1 = new NpgsqlParameter("@projectId", timeSheetReports.ProjectId);
                var param2 = new NpgsqlParameter("@companyId", timeSheetReports.CompanyId);
                var param3 = new NpgsqlParameter("@startDate", dFrom);
                var param4 = new NpgsqlParameter("@endDate", dTo);
                var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);

                List<TimeSheetFilterCount> timeSheetFilterCount = await _dbContext.TimeSheetReportCount.FromSqlRaw("EXEC [dbo].[spGetTimeSheetReportFilterCount] @empId, @projectId,@companyId, @startDate, @endDate,@searchText", param, param1, param2, param3, param4, param5).ToListAsync();
                foreach (var item in timeSheetFilterCount)
                {
                    result = item.Id;
                }
                return result;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
         /// Logic to get filter the timesheetreport data 
        /// </summary>      
        /// <param name="timeSheetReports,pager,columnName,columnDirection" ></param> 
        public async Task<List<TimeSheetReportViewModel>> GetTimeSheetFilter(TimeSheetReports timeSheetReports, SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                
                var lastDate = DateTime.Today.AddDays(-7).Date;
                var today = DateTime.Now.Date;

                var dFrom = "";
                var dTo = "";
                if (string.IsNullOrEmpty(timeSheetReports.StartTime))
                {
                    dFrom = lastDate.ToString(Constant.DateFormatMDY);
                    dTo =today.ToString(Constant.DateFormatMDY);
                }
                else
                {
                    dFrom = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartTime).ToString(Constant.DateFormatMDY);
                    dTo = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.EndTime).ToString(Constant.DateFormatMDY);
                }
                

                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param = new NpgsqlParameter("@empId", timeSheetReports.EmployeeId);
                var param1 = new NpgsqlParameter("@projectId", timeSheetReports.ProjectId);
                var param2 = new NpgsqlParameter("@companyId", timeSheetReports.CompanyId);
                var param3 = new NpgsqlParameter("@startDate", dFrom);
                var param4 = new NpgsqlParameter("@endDate", dTo);
                var param5 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param6 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param7 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param8 = new NpgsqlParameter("@sorting", _params.Sorting);


                return await _dbContext.TimeSheetReport.FromSqlRaw("EXEC [dbo].[spGetTimeSheetReportFilter] @empId,@projectId,@companyId,@startDate,@endDate,@offsetValue,@pagingSize,@searchText,@sorting", param, param1, param2, param3,param4,param5,param6,param7,param8).ToListAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //Mannual Log

        /// <summary>
        /// Logic to get imoportfile the manuallogentitys detail 
        /// </summary>         
        /// <param name="ManualLogEntitys" ></param>       
        public async Task<bool> ImoprtFile(List<ManualLogEntity> ManualLogEntitys)
        {
            var result = false;
            if (ManualLogEntitys.Count() > 0)
            {
                await _dbContext.ManualLogEntitys.AddRangeAsync(ManualLogEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get employee dropdown  list
        /// </summary> 
        public async Task<List<EmployeeDropdown>> GetAllEmployeeDropdown(int companyId)
        {
            var employee = new EmployeeDropdown();
            employee.EmployeeId = 0;
            employee.EmployeeIdWithName = Common.Constant.AllEmployees;
            var employeeDropdowns = new List<EmployeeDropdown>();
            employeeDropdowns.Add(employee);
            var allEmployees = await (from employees in _dbContext.Employees
                                      where companyId == employees.CompanyId && employees.IsDeleted == false
                                      select new EmployeeDropdown()
                                      {
                                          EmployeeIdWithName = employees.UserName + " " + Constant.Hyphen + " " + employees.FirstName + " " + employees.LastName,
                                          EmployeeId = employees.EmpId,
                                          EmployeeName = employees.UserName,
                                      }).ToListAsync();

            employeeDropdowns.AddRange(allEmployees);
            return employeeDropdowns;
        }

        /// <summary>
        /// Logic to get employee dropdown  list
        /// </summary> 
        /// <param name="IsDeleted" ></param> 
        public async Task<List<EmployeeDropdown>> GetAllEmployeesDropdown(int companyId)
        {
            var employee = new EmployeeDropdown();
            employee.EmployeeId = 0;
            employee.EmployeeIdWithName = Common.Constant.AllEmployees;
            var employeeDropdowns = new List<EmployeeDropdown>();
            employeeDropdowns.Add(employee);
            var allEmployees = await (from employees in _dbContext.Employees
                                      where companyId == employees.CompanyId && !employees.IsDeleted

                                      select new EmployeeDropdown()
                                      {
                                          EmployeeIdWithName = employees.UserName + " " + Constant.Hyphen + " " + employees.FirstName + " " + employees.LastName,
                                          EmployeeId = employees.EmpId,
                                          EmployeeName = employees.UserName,
                                      }).ToListAsync();

            employeeDropdowns.AddRange(allEmployees);
            return employeeDropdowns;
        }

        /// <summary>
        /// Logic to get all project name details list by particular empId 
        /// </summary> 
        /// <param name="empId" ></param>  
        public async Task<List<ProjectNames>> GetAllProjectNames(int empId, int companyId)
        {
            var project = new ProjectNames();
            project.ProjectId = 0;
            project.ProjectName = Common.Constant.AllProjects;
            var listOfProjectNames = new List<ProjectNames>();
            listOfProjectNames.Add(project);

            var projectNames = await (from projects in _dbContext.ProjectDetails
                                      where !projects.IsDeleted && companyId == projects.CompanyId
                                      select new ProjectNames()
                                      {
                                          ProjectId = projects.ProjectId,
                                          ProjectName = projects.ProjectName
                                      }).ToListAsync();
            listOfProjectNames.AddRange(projectNames);
            return listOfProjectNames;
        }

        /// <summary>
        /// Logic to get manuallog list
        /// </summary> 
        public async Task<List<ManualLog>> GetAllManulLogs()
        {
            var manualLog = await (from manualLogs in _dbContext.ManualLogEntitys
                                  select new ManualLog()
                                  {
                                      Sno = manualLogs.Sno,
                                      UserName = manualLogs.UserName,
                                      StartTime = manualLogs.StartTime.ToString(),
                                      EndTime = manualLogs.EndTime.ToString(),
                                      EntryStatus = manualLogs.EntryStatus,
                                      TotalHours = manualLogs.TotalHours,
                                      BreakHours = manualLogs.BreakHours,
                                      EmpId = manualLogs.EmpId,
                                  }).ToListAsync();
            return manualLog;
        }

        /// <summary>
        /// Logic to get all employees list count
        /// </summary> 
        /// <param name="pager" ></param>  
        public async Task<int>GetAllEmployeesListCount (SysDataTablePager pager,int companyId)
        {
            var _params = new
            {                            
                SearchText = pager.sSearch
            };            
            var paramcompany = new NpgsqlParameter("@companyId", companyId);           
            var param = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            List<EmployeesDataCount> employeeCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetAllEmployeesFilterListCount] @companyId,@searchText",paramcompany,param).ToListAsync();
            foreach (var item in employeeCounts)
            {
                var result = item.Id;
                return result;
            }
            return 0;
        }

        /// <summary>
        /// Logic to get all employees details list 
        /// </summary> 
        /// <param name="pager" ></param>  
        public async Task<List<EmployeesDataModel>> GetAllEmployeesList(SysDataTablePager pager, string columnDirection, string ColumnName,int companyId)
        {
            var _params = new
            {
                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = pager.sSearch,
                Sorting = ColumnName + " " + columnDirection,
            };
            var paramcompany = new NpgsqlParameter("@companyId", companyId);
            var param1 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
            var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
            var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var param4 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
            var employeeList = await _dbContext.EmployeesDataModels.FromSqlRaw("EXEC [dbo].[spGetAllEmployeesFilterList] @companyId,@pagingSize ,@offsetValue,@searchText,@sorting", paramcompany, param1, param2, param3, param4).ToListAsync();
            return employeeList;

        }
        /// <summary>
        /// Logic to get all GetLeaveReportByEmployeeId details list 
        /// </summary> 
        /// <param name="pager,reports,columnDirection,columnName" ></param>  

        public async Task<List<EmployeeLeaveReportDataModel>> GetLeaveReportByEmployeeId(SysDataTablePager pager, Reports reports, string columnDirection, string columnName)
        {
            var leaveReportList = new List<EmployeeLeaveReportDataModel>();

            var _params = new
            {

                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
                Sorting = columnName + " " + columnDirection,
            };

            var startDate = string.IsNullOrEmpty(reports.LeaveFromDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(reports.LeaveFromDate).ToString(Constant.DateFormatMDY);
            var endDate = string.IsNullOrEmpty(reports.LeaveToDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(reports.LeaveToDate).ToString(Constant.DateFormatMDY);
            var empId = reports.EmployeeId;
            var leaveTypeId = reports.LeaveTypeId;
            var status = reports.EmployeeStatus;
            var param = new NpgsqlParameter("@empId", empId);
            var param1 = new NpgsqlParameter("@leaveTypeId", leaveTypeId);
            var param2 = new NpgsqlParameter("@startDate", startDate);
            var param3 = new NpgsqlParameter("@endDate", endDate);
            var param4 = new NpgsqlParameter("@status", status);
            var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var param6 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
            var param7 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
            var param8 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
            try
            {
                leaveReportList = await _dbContext.EmployeeLeaveReportDataModel.FromSqlRaw("EXEC [dbo].[spGetLeaveTypeByEmpIdFilterData] @empId,@leaveTypeId,@startDate,@endDate,@status,@searchText,@pagingSize ,@offsetValue,@sorting", param, param1, param2, param3, param4, param5, param6, param7, param8).ToListAsync();

            }
            catch (Exception ex)
            {

            }

            return leaveReportList;
        }
        /// <summary>
        /// Logic to get all GetLeaveReport Count  
        /// </summary> 
        /// <param name="pager,reports" ></param>  

        public async Task<int> GetLeaveReportByEmployeeIdCount(SysDataTablePager pager, Reports reports)
        {
            var leaveReportList = new List<EmployeeLeaveReportCount>();
            var _params = new
            {
                SearchText = pager.sSearch
            };

            var startDate = string.IsNullOrEmpty(reports.LeaveFromDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(reports.LeaveFromDate).ToString(Constant.DateFormatMDY);
            var endDate = string.IsNullOrEmpty(reports.LeaveToDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(reports.LeaveToDate).ToString(Constant.DateFormatMDY);
            var empId = reports.EmployeeId;
            var leaveTypeId = reports.LeaveTypeId;
            var status = reports.EmployeeStatus;
            var param = new NpgsqlParameter("@empId", empId);
            var param1 = new NpgsqlParameter("@leaveTypeId", leaveTypeId);
            var param2 = new NpgsqlParameter("@startDate", startDate);
            var param3 = new NpgsqlParameter("@endDate", endDate);
            var param4 = new NpgsqlParameter("@status", status);
            var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            try
            {
                leaveReportList = await _dbContext.EmployeeLeaveReportCounts.FromSqlRaw("EXEC [dbo].[spGetLeaveTypeCountByEmpIdFilterData]  @empId,@leaveTypeId,@startDate,@endDate,@status,@searchText", param, param1, param2, param3, param4, param5).ToListAsync();

            }
            catch (Exception ex)
            {

            }
            var employeeCount = 0;
            foreach (var leaveReport in leaveReportList)
            {
                employeeCount = leaveReport.EmployeeCount;
            }

            return employeeCount;


        }

        /// <summary>
        /// Logic to get all employees details list get pdf
        /// </summary> 
        /// <param name="proc" ></param>  
        /// <param name="values" ></param>  
        public async Task<List<EmployeesDataModel>> GetAllEmployeeFilter(string proc, List<KeyValuePair<string, string>> values)
        {
            try
            {
                var parameters = new object[values.Count];
                for (int i = 0; i < values.Count; i++)
                parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);
                var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
                paramnames = paramnames.TrimEnd(',');
                proc = proc + " " + paramnames;

                var employeesDataModels = await _dbContext.EmployeesDataModels.FromSqlRaw<EmployeesDataModel>(proc, parameters).AsNoTracking().ToListAsync();
                return employeesDataModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
