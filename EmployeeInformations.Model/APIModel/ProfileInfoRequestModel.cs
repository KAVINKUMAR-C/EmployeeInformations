using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.APIModel
{
    public class ProfileInfoRequestModel
    {
        public int ProfileId { get; set; }
        public int EmpId { get; set; }

        public int Gender { get; set; }
        public int MaritalStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int BloodGroup { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ProfileName { get; set; }
        public string ProfileFilePath { get; set; }
        public string ProfileViewImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ProfileActionName { get; set; }
        public string? ContactPersonName { get; set; }
        public int? RelationshipId { get; set; }
        public string? ContactNumber { get; set; }
        public string? CountryCodeNumber { get; set; }
        public string? OthersName { get; set; }
        public List<BloodGroup> BloodGroups { get; set; }
        public List<RelationshipType> RelationshipType { get; set; }



        // Datetime issue
        public string StrDateOfBirth { get; set; }
        public string StrDateOfJoining { get; set; }
    }
}
