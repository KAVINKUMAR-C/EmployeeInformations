namespace EmployeeInformations.Model.LeaveSummaryViewModel
{
    public class EmployeeHolidays
    {
        public int HolidayId { get; set; }
        public string Title { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Holiday {  get; set; }    
        public string HolidayName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string HolidayDates { get; set; }
    }
}
