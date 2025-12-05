using System.ComponentModel;

namespace EmployeeInformations.Common.Enums
{

    public enum TimeSheetStatus
    {
        Pending = 1,
        Inprogress = 2,
        Completed = 3,
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
    }

    public enum PurchaseOrderInvoiceNumber
    {
        PurchaseOrder = 1,
        InvoiceNumber = 2,
    }

    public enum MaritalStatus
    {
        Married = 1,
        Single = 2,
    }

    public enum ActiveModes
    {
        DarkMode = 1,
        LightMode = 0,
    }

    public enum LeavetypeStatus
    {
        CasualLeave = 1,
        SickLeave = 2,
        EarnedLeave = 3,
        MaternityLeave = 4,
        Permission = 5,
        LOP = 6,
        WorkFromHome = 7,
        CompensatoryOff = 8,
    }
    public enum OtherDetailsStatus
    {
        Aaharcard = 1,
        Pancard = 2,
        Passport = 3,
        DrivingLicence = 4,
        UANNumber =6,
    }

    public enum AppliedLeaveStatus
    {
        Pending = 0,
        Approved = 1,
        Reject = 2,
    }


    public enum EmailDraftType
    {
        WelcomeEmployee = 1,
        AcceptProbation = 2,
        ForgotPassword = 3,
        ChangePassword = 4,
        ApplyLeave = 5,
        AcceptLeave = 6,
        RejectLeave = 7,
        RequestBirthday = 8,
        RequestProbation = 9,
        ProjectAssignation = 10,
        ProjectRejection = 11,
        JobPostActive = 12,
        JobPostDeactive = 13,
        AttendanceLog = 14,
        ErrorMessage = 15,
        AttendanceLogForManagement = 16,
        ApplyWorkFromHome = 17,
        AcceptWorkFromHome = 18,
        RejectWorkFromHome = 19,
        ApplyCompensatoryOffRequest = 20,
        AcceptCompensatoryOffRequest = 21,
        RejectCompensatoryOffRequest = 22,
        AttendanceLogEmployeeMonthly = 23,
        DailyAttendance = 24,
        WeeklyAttendance = 25,
        MonthlyAttendance = 26,
        YearlyAttendance = 27,
        DailyLeave = 28,
        WeeklyLeave = 29,
        MonthlyLeave = 30,
        YearlyLeave = 31,
        DailyTimeSheet = 32,
        WeeklyTimeSheet = 33,
        MonthlyTimeSheet = 34,
        YearlyTimeSheet = 35,
        WorkAnniversary = 36,
        AttendanceLogEmployees = 37,
        Announcemeent = 38,
        ApplyExpense = 39,
        HelpdeskTicket = 40,
        WebSiteCandidateMenu =41,
        WebSiteCandidateMenuReject = 42,
    }

    public enum MailSchedulerDraft
    {
        DailyAttendance = 24,
        WeeklyAttendance = 25,
        MonthlyAttendance = 26,
        YearlyAttendance = 27,
        MonthlyLeave = 30,
        WeeklyTimeSheet = 33,
        MonthlyTimeSheet = 34,
    }

    public enum Months
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum Duration
    {
        Once = 1,
        Daily = 2,
        Monthly = 3,
        Yearly = 4,
        Custom = 5
    }

    public enum FileFormats
    {
        Excel = 1,
        Pdf = 2,
        ExcelPDF = 3,
    }

    public enum EmployeeStatus
    {
        Active = 1,
        InActive = 2
    }

    public enum BenefitType
    {        
        MedicalBenefit = 4
    }

    public enum ExpenseStatus
    {
        WaitingForApproval = 0,
        Approved = 1,
        Rejected = 2,
        Paid = 3,
    }

    public enum AnnouncementAssignee
    {
        Employees = 1,
        Designation = 2,
        Department = 3,
    }

    public enum SubModule
    {
        ModuleList = 14,
        SubModuleList = 15,
    }

    public enum Role : byte
    {
        Admin = 1,
        HR = 2,
        ProjectManager = 3,
        TeamLead = 4,
        Employee = 5,
        Sales = 6,
        HRAssistant = 7,
    }

    public enum TicketStatus
    {
        Pending = 0,
        Open = 1,       
        InProgress = 2,
        Closed = 3,
        NotFixed = 4, 
    }

}
