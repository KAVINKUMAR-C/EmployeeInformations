using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformations.CoreModels.Configuration
{
    public class CompanySettings
    {
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string Industry { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public PhysicalAddress PhysicalAddress { get; set; }
        public string CompanyCountryCode { get; set; }
    }

    public class PhysicalAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class MasterData
    {
        public List<string> Departments { get; set; }
        public List<string> Designations { get; set; }
        public List<string> Roles { get; set; }
        public List<StateData> States { get; set; }
    }

    public class StateData
    {
        public string Name { get; set; }
        public List<string> Cities { get; set; }
    }

    public class SuperAdminSettings
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DefaultPassword { get; set; }
        public bool ForcePasswordChange { get; set; }
    }
}
