using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_SurveyQuestion")]
    public class WebsiteSurveyQuestionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyQuestionId { get; set; }
        public string SurveyQuestionName { get; set; }
        public bool IsActive { get; set; }
    }
}
