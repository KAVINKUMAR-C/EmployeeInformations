using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.MasterViewModel
{
    public class TicketTypesViewModel
    {
        public int TicketTypeId { get; set; }
        public string TicketName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public string ReportingPersonId { get; set; }
        public List<ReportingPerson> reportingPeople { get; set; }
        public List<TicketTypes> ticketTypes { get; set; }
    }

    public class TicketTypes
    {
        public int TicketTypeId { get; set; }
        public string TicketName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public string ReportingPersonId { get; set; }     
        public int TicketNameCount { get; set; }
        public List<ReportingPerson> reportingPeople { get; set; }
        public string StrFmtReportingPersonId { get; set; }
        
    }

}
