namespace EmployeeInformations.Model.MasterViewModel
{
    public class LeaveTypeViewModel
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public bool Status { get; set; }
        public bool IsActive { get; set; }
        public List<LeaveTypeMaster> LeaveTypeMaster { get; set; }
    }

    //public class LeaveTypes
    //{
    //    public int Id { get; set; }
    //    public int LeaveTypeId { get; set; }
    //    public string LeaveType { get; set; }
    //    public bool IsDeleted { get; set; }
    //    public int CompanyId { get; set; }
    //    public bool Status { get; set; }
    //}



    public class LeaveTypeMaster
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public int LeaveTypeCount { get; set; }

    }

    public class LeaveTypeViewModels
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
    }

    public class MasterLeaveType
    {
        public int MasterLeaveId { get; set; }
        public string LeaveType { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class LeaveAssignViewModel
    {
        public int Id { get; set; }
        public string Leavetype { get; set; }
        public List<LeaveTypePrivilegeViewModel> LeaveTypePrivilegeViewModel { get; set; }
    }

    public class LeaveTypePrivilegeViewModel
    {
        public int Id { get; set; }
        public string Leavetype { get; set; }
        // public int ModuleId { get; set; }
        public string MenuName { get; set; }
        public int LeaveTypeId { get; set; }
        // public string SubModuleName { get; set; }
        public bool Status { get; set; }
    }

    public class AssignLeaveView
    {
        public int CompanyId { get; set; }
        public int LeaveTypeId { get; set; }
        public string IsEnabled { get; set; }
    }
}
