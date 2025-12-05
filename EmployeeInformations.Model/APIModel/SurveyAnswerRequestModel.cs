namespace EmployeeInformations.Model.APIModel
{
    public class SurveyAnswerRequestModel
    {
        public int SurveyQuestionId { get; set; }
        public int SurveyOptionId { get; set; }
        public string SurveyAnswer { get; set; }
        public int QuoteId { get; set; }
    }
}
