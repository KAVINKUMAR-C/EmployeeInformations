

using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.OnboardingViewModel
{
    public class OBAddressInfo
    {
        public int AddressId { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int Pincode { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public string? SecondaryAddress1 { get; set; }
        public string? SecondaryAddress2 { get; set; }
        public int? SecondaryCityId { get; set; }
        public int SecondaryStateId { get; set; }
        public int SecondaryCountryId { get; set; }
        public int? SecondaryPincode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public List<State>? states { get; set; }
        public List<City>? cities { get; set; }
        public List<Country>? country { get; set; }
        public List<State>? Secondarystates { get; set; }
        public List<City>? Secondarycities { get; set; }

    }
    public class OBViewAddressInfo
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public int Pincode { get; set; }

        public string SecondaryAddress1 { get; set; }
        public string SecondaryAddress2 { get; set; }
        public string SecondaryCityName { get; set; }
        public string SecondaryStateName { get; set; }
        public string SecondaryCountryName { get; set; }
        public int? SecondaryPincode { get; set; }
    }
}
