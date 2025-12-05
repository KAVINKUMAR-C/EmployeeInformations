namespace EmployeeInformations.CoreModels.DataViewModel
{
    public class EmployeesLogReportDataModel
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
