

using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class EmployeeLeaveReportCount
    {
        public int EmployeeCount { get; set; }
    }
}
