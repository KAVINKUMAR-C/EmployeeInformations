namespace EmployeeInformations.Model.APIModel
{
    public class CandidatePrivilegesModel
    {
        public int CandidatePrivilegeId { get; set; }
        public int CandidatescheduleId { get; set; }
        public string IsEnabled { get; set; }        
        public int CompanyId { get; set; }
        public string SchedulerName { get; set; }
        public List<CandidatePrivileges> candidatePrivileges{get;set;}        
    }

    public class CandidatePrivileges
    {
        public int CandidatePrivilegeId { get; set; }
        public int CandidatescheduleId { get; set; }
        public bool? IsEnabled { get; set; }                
        public int CompanyId { get; set; }
        public string SchedulerName { get; set; } 
        public bool IsDeleted { get; set; }
    }

    public class CandidatePrivilegeAssign
    {
        public int CandidatescheduleId { get; set; }
        public string IsEnabled { get; set; }
    }
}
