using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class CompanyViewModels
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set;}
        public string CompanyPhoneNumber { get; set;}
        public string CompanyEmail { get; set;}
        public string Industry { get; set;}
        public string  ContactPersonName { get;set;}
        public string ContactPersonEmail { get; set;}
    }
    [Keyless]
    public class CompanyCounts
    {
        public int CompnayCountId { get; set; }
    }
}
