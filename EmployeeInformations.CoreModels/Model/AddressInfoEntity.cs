using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("AddressInfo")]
    public class AddressInfoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int AddressId { get; set; }
        [ForeignKey("Employees")]
        public int EmpId { get; set; }
        public virtual EmployeesEntity Employees { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int Pincode { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? SecondaryAddress1 { get; set; }
        public string? SecondaryAddress2 { get; set; }
        public int? SecondaryCityId { get; set; }
        public int? SecondaryStateId { get; set; }
        public int? SecondaryCountryId { get; set; }
        public int? SecondaryPincode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }




    }
}
