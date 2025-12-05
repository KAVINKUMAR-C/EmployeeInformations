namespace EmployeeInformations.Model.APIModel
{
    public class JobPostRequestModel
    {
        public int JobId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Experience { get; set; }
        public string? SkillSet { get; set; }
        public string? RelevantExperience { get; set; }
        public string Base64string { get; set; }
        public string? FileFormat { get; set; }
    }
}
