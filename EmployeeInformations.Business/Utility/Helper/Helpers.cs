

namespace EmployeeInformations.Business.Utility.Helper
{
    public class Helpers
    {
        public static decimal TotalLeaveCount( decimal totalLeave , decimal approvedLeave)
        {
            var totalLeaveCount = totalLeave - approvedLeave;
            return totalLeaveCount;
        }
    }
}
