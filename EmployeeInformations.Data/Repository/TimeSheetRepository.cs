using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.TimesheetSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Npgsql;



namespace EmployeeInformations.Data.Repository
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public TimeSheetRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //// project details 
       

        /// <summary>
        /// Logic to get empId the projectdetails detail by particular empId
        /// </summary>         
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectDetailsEntity>> GetAllProjectNames(int empId, int companyId)
        {
            if (empId == 0)
            {
                return await _dbContext.ProjectDetails.Where(x => !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
            else
            {
                return await _dbContext.ProjectDetails.Where(y => y.EmpId == empId && !y.IsDeleted && y.CompanyId == companyId).ToListAsync();
            }
        }


        /// <summary>
        /// Logic to get projrctId based projectname the projectdetails detail by particular projectId
        /// </summary>         
        /// <param name="projrctId" ></param>
        /// <param name="CompanyId" ></param>      
        public async Task<string> GetProjectName(int projrctId,int companyId)
        {
            var timeSheetEntity = await _dbContext.ProjectDetails.FirstOrDefaultAsync(e => e.ProjectId == projrctId && e.CompanyId == companyId);
            if (timeSheetEntity != null)
            {
                return timeSheetEntity.ProjectName;
            }
            return string.Empty;
        }

        ///// Time Sheet

        /// <summary>
        /// Logic to get empId the timesheet list detail by particular empId
        /// </summary> 
        /// <param name="empId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<TimeSheetEntity>> GetAllTimeSheet(int empId, int companyId)
        {
            if (empId == 0)
            {
                return await _dbContext.TimeSheet.Where(x => !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
            else
            {
                return await _dbContext.TimeSheet.Where(x => x.EmpId == empId && !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
        }


        /// <summary>
        /// Logic to get create and update the timesheet detail by particular timesheet
        /// </summary>         
        /// <param name="timeSheetEntity" ></param>     
        public async Task<int> CreateTimeSheet(TimeSheetEntity timeSheetEntity, int companyId)
        {
            var result = 0;
            if (timeSheetEntity?.TimeSheetId == 0)
            {
                timeSheetEntity.CompanyId = companyId;
                await _dbContext.TimeSheet.AddAsync(timeSheetEntity);
                await _dbContext.SaveChangesAsync();
                result = timeSheetEntity.TimeSheetId;
            }
            else
            {
                if (timeSheetEntity != null)
                {
                    _dbContext.TimeSheet.Update(timeSheetEntity);
                    await _dbContext.SaveChangesAsync();
                    result = timeSheetEntity.TimeSheetId;
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get deleted the timesheet detail by particular TimeSheetId
        /// </summary>         
        /// <param name="TimeSheetId" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeleteTimeSheet(int TimeSheetId, int companyId)
        {
            var result = false;
            var timeSheetEntity = await _dbContext.TimeSheet.FirstOrDefaultAsync(d => d.TimeSheetId == TimeSheetId && d.CompanyId == companyId);
            if (timeSheetEntity != null)
            {
                timeSheetEntity.IsDeleted = true;
                _dbContext.TimeSheet.Update(timeSheetEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get timeSheetId the timesheet detail by particular timeSheetId
        /// </summary> 
        /// <param name="timeSheetId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<TimeSheetEntity> GetByTimeSheetId(int timeSheetId, int companyId)
        {
            var timeSheetEntity = await _dbContext.TimeSheet.FirstOrDefaultAsync(x => x.TimeSheetId == timeSheetId && !x.IsDeleted && x.CompanyId == companyId);
            return timeSheetEntity ?? new TimeSheetEntity();
        }


        /// <summary>
        /// Logic to get empIds the timesheet detail by particular empIds
        /// </summary>         
        /// <param name="empIds" ></param>
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<TimeSheetEntity>> GetAllTimeSheets(List<int> empIds,int companyId)
        {
            var timeSheetEntity = await _dbContext.TimeSheet.Where(e => empIds.Contains(e.EmpId) && !e.IsDeleted && e.CompanyId == companyId).ToListAsync();
            return timeSheetEntity;
        }


        /// <summary>
        /// Logic to get filter the timesheet detail by particular dateTime flterDate
        /// </summary>         
        /// <param name="dateTime flterDate" ></param>
        /// <param name="CompanyId" ></param> 
        /// <param name="IsDeleted" ></param>
        public async Task<List<TimeSheetEntity>> GetAllTimeSheetByCurrentDate(DateTime flterDate, int companyId)
        {
            return await _dbContext.TimeSheet.Where(x => x.StartTime.Date == flterDate.Date && !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<TimeSheetEntity>> GetAllTimeSheetByCurrentDateAndCompanyId(DateTime flterDate, int companyId)
        {
            var timeSheetEntitys = await _dbContext.TimeSheet.Where(x => x.CreatedDate.Date == flterDate.Date && x.CompanyId == companyId).ToListAsync();
            return timeSheetEntitys;
        }


        /// <summary>
        /// Logic to get all employee timesheet details list
        /// </summary>                
        public async Task<List<TimeSheet>> GetAllEmployeesTimeSheet(int companyId)
        {
            var employeesTimesheet = (from timeSheet in _dbContext.TimeSheet
                                      join emp in _dbContext.Employees on timeSheet.EmpId equals emp.EmpId
                                      join project in _dbContext.ProjectDetails on timeSheet.ProjectId equals project.ProjectId
                                      where !timeSheet.IsDeleted && !project.IsDeleted && timeSheet.CompanyId == companyId  
                                      select new TimeSheet()
                                      {
                                          TimeSheetId = timeSheet.TimeSheetId,
                                          EmpId = timeSheet.EmpId,
                                          TaskDescription = timeSheet.TaskDescription,
                                          TaskName = timeSheet.TaskName,
                                          Startdate = timeSheet.Startdate,
                                          ProjectId = timeSheet.ProjectId,
                                          StartTime = timeSheet.StartTime,
                                          EndTime = timeSheet.EndTime,
                                          Status = timeSheet.Status,
                                          AttachmentFilePath = timeSheet.AttachmentFilePath,
                                          AttachmentFileName = timeSheet.AttachmentFileName,
                                          EmployeeName = emp.FirstName +" "+ emp.LastName,
                                          EmployeeStatus = emp.IsDeleted,
                                          ProjectName = project.ProjectName,
                                      }).ToList();
            return employeesTimesheet.ToList();
        }

        /// <summary>
        /// Logic to get employee and reporting person timesheet details list by particular empId
        /// </summary>         
        /// <param name="empId" ></param>       
        public async Task <List<TimeSheet>> GetEmployeeTimesheet(int empId, int companyId)
        {
            var employeeTimesheet = (from timesheet in _dbContext.TimeSheet
                                     join emp in _dbContext.Employees on timesheet.EmpId equals emp.EmpId
                                     join project in _dbContext.ProjectDetails on timesheet.ProjectId equals project.ProjectId
                                     where !timesheet.IsDeleted && !project.IsDeleted && !emp.IsDeleted && (_dbContext.ReportingPersonsEntities.Where(x => x.ReportingPersonEmpId == empId).Select(x => x.EmployeeId).Contains(timesheet.EmpId)
                                     || timesheet.EmpId == empId && companyId == timesheet.CompanyId)
                                     select new TimeSheet ()
                                     {
                                         TimeSheetId = timesheet.TimeSheetId,
                                         EmpId = timesheet.EmpId,
                                         TaskDescription = timesheet.TaskDescription,
                                         TaskName = timesheet.TaskName,
                                         Startdate = timesheet.Startdate,
                                         ProjectId = project.ProjectId,
                                         StartTime = timesheet.StartTime,
                                         EndTime = timesheet.EndTime,
                                         Status = timesheet.Status,
                                         AttachmentFilePath = timesheet.AttachmentFilePath,
                                         AttachmentFileName = timesheet.AttachmentFileName,
                                         EmployeeName = emp.FirstName +" "+ emp.LastName,
                                         EmployeeStatus = emp.IsDeleted,
                                         ProjectName = project.ProjectName,
                                     }).ToList();
            return employeeTimesheet.ToList();
        }

        /// <summary>
        /// Logic to get all employee timesheet details list and particular empId
        /// </summary>         
        /// <param name="empId,pager,columnDirection,columnName" ></param> 
        public async Task<List<TimeSheetModel>> GetTimeSheetByEmpIdFilterList(SysDataTablePager pager,int empId, string columnDirection, string columnName,int companyId)
        {
            try
            {         
                var timesheet = new TimeSheet();                
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,                   
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection,
                };
                
                var param = new NpgsqlParameter("@empId", empId);
                var paramcompany = new NpgsqlParameter("@companyId", companyId);
                var param1 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
                var data = await _dbContext.TimeSheetModels.FromSqlRaw("EXEC [dbo].[spGetTimeSheetFilterList] @empId,@companyId,@pagingSize ,@offsetValue,@searchText,@sorting", param, paramcompany, param1, param2, param3, param4).ToListAsync();                
                return data; 
            }
            catch (Exception ex)
            { 
                throw (ex);
            }
        }

        /// <summary>
        /// Logic to get all employee timesheet details list count and particular empId count
        /// </summary>         
        /// <param name="empId,pager" ></param> 
        public async Task<int> GetAllTimeSheetListCount(SysDataTablePager pager,int empId,int companyId)
        {
            
            if (pager.iDisplayStart >= pager.iDisplayLength)
            {
                pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
            }
            var _params = new
            {
                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = pager.sSearch
            };
            var paramcompany = new NpgsqlParameter("@companyId", companyId);
            var paramempId = new NpgsqlParameter("@empId", empId);
            var param = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            List<EmployeesDataCount> employeeCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetTimesheetFilterListCount] @companyId,@empId,@searchText", paramcompany, paramempId, param).ToListAsync();
            foreach (var item in employeeCounts)
            {
                var result = item.Id;
                return result;
            }
            return 0;
        }
    }
}
