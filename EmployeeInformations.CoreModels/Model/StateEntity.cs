using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("State")]
    public class StateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public bool IsDeleted { get; set; }
    }


    [Table("City")]
    public class CityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
        public bool IsDeleted { get; set; }
    }

    [Table("Country")]
    public class CountryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        public string CCA2 { get; set; }
        public int? NumericCode { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}

