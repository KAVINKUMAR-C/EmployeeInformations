using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.Model
{
    [Table("EmailTitles")]
    public class EmailTitlesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TitleId { get; set; }
        public string EmailTitle { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
