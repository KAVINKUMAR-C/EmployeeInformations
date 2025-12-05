using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_SurveyQuestionOptions")]
    public class WebsiteSurveyQuestionOptionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyOptionId { get; set; }
        public int SurveyQuestionId { get; set; }
        public string? SurveyQuestionOptionName { get; set; }
        public bool IsActive { get; set; }
    }
}
