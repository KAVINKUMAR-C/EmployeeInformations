

using EmployeeInformations.Common.Enums;

namespace EmployeeInformations.Model.OnboardingViewModel
{
    public class OnboardingCompletion
    {
        public int EmpId { get; set; }
        public Role RoleId { get; set; }
        public string CompanyName { get; set; }
         public string EmployeeName { get; set; }
        public int WelcomeBoard {get; set; }
        public string ProfileCompletion { get; set; }
        public int Intro { get; set; }  
        public int ProfileInfo { get; set; }
        public int AddressInfo { get; set; }
        public int OtherDetails { get; set; }
        public int QualificationInfo { get; set; }
        public int ExperienceInfo { get; set; }
        public int BankDetails { get; set; }
         
    }
}
