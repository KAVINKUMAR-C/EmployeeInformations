using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class ClientFilterViewModel
    {
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? ClientCompany { get; set; }
        public string? ClientDetails { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class ClientFilterCount
    {
        public int Id { get; set; }
    }
}
