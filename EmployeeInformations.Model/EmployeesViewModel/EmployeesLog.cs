

using EmployeeInformations.CoreModels.DataViewModel;

namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class EmployeesLog
    {
        public int EmployeesLogId { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string SectionName { get; set; }
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string Event { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorName { get; set; }
        public int EmployeeId { get; set; }
    }

    public class EmployeesChangeLog
    {
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
    }

    public class EmployeeChangeLogViewModel
    {
        public int EmployeesLogId { get; set; }
        public int EmpId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CompanyId { get; set; }
        public string SectionName { get; set; }
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string Event { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorName { get; set; }
        public string EmployeeModules { get; set; }
        public List<EmployeesLog> EmployeesLog { get; set; }
        public List<EmployeeDropdown> ReportingPeople { get; set; }
        public List<EmployeeDetailsDropdown> EmployeeDetails { get; set; }
        public string? ColumnName { get; set; }
        public string? ColumnDirection { get; set; }

        public List<EmployeeLog>EmployeeLogs { get; set; }

    }

    public class EmployeeDropdown
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeIdWithName { get; set; }
    }

    public class EmployeeDetailsDropdown
    {
        public string ModuleName { get; set; }
        public string EmployeeModules { get; set; }
    }

    public class FilterViewEmployeeLogReport
    {
        public int Id { get; set; }
        public string? FieldName { get; set; }
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string? Event { get; set; }
        public int EmpId { get; set; }
        //public string AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
