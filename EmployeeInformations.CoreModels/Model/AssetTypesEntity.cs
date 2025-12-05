using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("AssetTypes")]
    public class AssetTypesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
    }
}
