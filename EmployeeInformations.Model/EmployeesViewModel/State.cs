namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

    }
    public class City
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string? CityName { get; set; }
    }
    public class Country
    {
        public int CountryId { get; set; }
        public int? NumericCode { get; set; }
        public string Code { get; set; }
        public string Tld { get; set; }
        public string CCA2 { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public string Capital { get; set; }
    }

    public class ReportingPerson
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
    }

    public class ApprovalsSettings
    {
        public string Request { get; set; }
        public string Change { get; set; }
    }
}
