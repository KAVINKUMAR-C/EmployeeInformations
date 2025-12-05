using Microsoft.EntityFrameworkCore;


namespace EmployeeInformations.CoreModels.Model
{
    [Keyless]
    public class EmailDraftEntites
    {
        public int Id { get; set; }
        public string? DraftType { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public string? StatusName { get; set; }
    }
    [Keyless]
    public class EmailDraftCount
    {
        public int EmailCount { get; set; }

    }
}
