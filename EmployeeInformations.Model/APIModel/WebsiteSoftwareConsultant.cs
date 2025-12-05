namespace EmployeeInformations.Model.APIModel
{
    public class WebsiteSoftwareConsultant
    {
        public int SurveyAnswerId { get; set; }
        public int SurveyQuestionId { get; set; }
        public int SurveyOptionId { get; set; }
        public string SurveyAnswer { get; set; }
        public int QuoteId { get; set; }

        public string QuestionName { get; set; }
        public string OptionName { get; set; }
        public string QuoteName { get; set; }
    }
}
