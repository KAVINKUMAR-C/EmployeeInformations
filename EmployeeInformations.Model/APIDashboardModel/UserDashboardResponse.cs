namespace EmployeeInformations.Model.APIDashboardModel
{
    public class UserDashboardResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }


    public class UserTimeResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public TimeLogViewAPI? timeLogViewAPI { get; set; }
    }



}
