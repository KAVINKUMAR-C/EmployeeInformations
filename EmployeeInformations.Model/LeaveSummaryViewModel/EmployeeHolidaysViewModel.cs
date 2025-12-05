using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Model.LeaveSummaryViewModel
{
    public class EmployeeHolidaysViewModel
    {
        public int HolidayId { get; set; }
        public string Title { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Holiday {  get; set; }    
        public string HolidayName { get; set; }
        public int Year { get; set; }
        public List<Years>? Years { get; set; }
        public List<EmployeeHolidays>? EmployeeHolidays { get; set; }
    }

}
