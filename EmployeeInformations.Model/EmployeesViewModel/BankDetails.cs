namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class BankDetails
    {
        public int EmpId { get; set; }
        public int BankId { get; set; }
        public int CompanyId { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public Int64 AccountNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVerified { get; set; }
    }
}
