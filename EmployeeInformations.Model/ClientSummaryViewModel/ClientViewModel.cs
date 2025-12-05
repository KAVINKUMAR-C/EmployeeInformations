using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.ClientSummaryViewModel
{
    public class ClientViewModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCompany { get; set; }
        public string ClientDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string ClassName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public List<Country>? countrys { get; set; }
    }
}
