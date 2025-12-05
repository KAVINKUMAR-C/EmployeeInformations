using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("AssetLog")]
    public class AssetLogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetLogId { get; set; }
        public int AssetId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public string AssetNo { get; set; }
        public int AssetType { get; set; }
        public string AssetName { get; set; }
        public string? FieldName { get; set; }
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string Event { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
