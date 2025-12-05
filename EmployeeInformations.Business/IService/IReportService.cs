using EmployeeInformations.Model;﻿
using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ReportsViewModel;


namespace EmployeeInformations.Business.IService
{
    public interface IReportService
    {
        Task<List<EmployeeDropdown>> GetAllEmployeesDrropdown(int companyId);
        Task<List<Years>> GetYear();
        Task<List<LeaveTypes>> GetAllLeave();
        Task<List<ProjectNames>> GetAllProjectNames(int empId,int companyId);
        Task<ManualLogReports> GetAllEmployessByManualLogFilter(ManualLogReports manualLogReports);
        Task<TimeSheetReports> GetAllEmployessByTimeSheetFilter(TimeSheetReports timeSheetReports, int companyId);
        Task<TimeSheetReports> GetAllEmployessByTimeSheetFilters(TimeSheetReports timeSheetReports,SysDataTablePager pager, string columnName, string columnDirection);
        Task<int> GetFilterTimeSheetReportCount(TimeSheetReports timeSheetReports, SysDataTablePager pager);
        Task<bool> ImoprtFile(List<ManualLog> manualLog);
        Task<List<ManualLog>> GetAllManualLog();
        Task<DropdownProjects> GetByEmployeeIdForProject(string employeeIds, int companyId);
        Task<List<FilterViewEmployeeLeave>> GetAllReportsLeave();
    
        Task<int> GetAllEmployeesListCount(SysDataTablePager pager, int companyId);
        Task<EmployeesListViewModel> GetAllEmployeesList(SysDataTablePager pager, string columnDirection, string ColumnName,int companyId);
        Task<EmployeesListViewModel> GetAllEmployeesListPdf(int companyId);
        Task<int> GetAllEmployessByLeaveTypeFilterDataCount(SysDataTablePager pager, Reports reports);
        Task<List<FilterViewEmployeeLeave>> GetAllEmployeeLeaveReports(SysDataTablePager pager, string columnDirection, string columnName);
        Task<int> GetAllEmployessCount(SysDataTablePager pager);
        Task<List<EmployeeDropdown>> GetEmpDropDown(int id, int companyId);
        Task<List<FilterViewEmployeeLeave>> GetAllEmployessByLeaveTypeFilter(Reports reports, int companyId);
        Task<List<FilterViewEmployeeLeave>> GetAllEmployessByLeaveTypeFilterData(SysDataTablePager pager, Reports reports, string columnDirection, string columnName, int companyId);
    }
}