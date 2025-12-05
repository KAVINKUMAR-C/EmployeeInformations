namespace EmployeeInformations.Model.APIModel
{
    public class WebsiteSurveyResponseModel
    {
        public string SurveyQuestionName { get; set; }
        public WebsiteSurveyOptionsModel WebsiteSurveyOptionsModels { get; set; }

    }

    public class WebsiteSurveyOptionsModel
    {
        public int SurveyOptionId { get; set; }
        public int SurveyQuestionId { get; set; }
        public string? SurveyQuestionOptionName { get; set; }
    }
}
