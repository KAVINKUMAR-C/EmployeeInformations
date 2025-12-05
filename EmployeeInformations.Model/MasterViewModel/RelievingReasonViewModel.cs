namespace EmployeeInformations.Model.MasterViewModel
{
    public class RelievingReasonViewModel
    {
        public int RelievingReasonId { get; set; }
        public string RelievingReasonName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public List<RelievingReason> RelievingReason { get; set; }
    }
}
