using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{

    [Table("Website_Services")]
    public class WebsiteServicesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServicesId { get; set; }
        public string ServiceName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
