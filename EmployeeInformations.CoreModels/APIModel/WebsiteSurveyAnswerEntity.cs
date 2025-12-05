using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInformations.CoreModels.APIModel
{
    [Table("Website_SurveyAnswers")]
    public class WebsiteSurveyAnswerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyAnswerId { get; set; }
        public int SurveyQuestionId { get; set; }
        public int SurveyOptionId { get; set; }
        public string SurveyAnswer { get; set; }
        public int QuoteId { get; set; }

    }
}
