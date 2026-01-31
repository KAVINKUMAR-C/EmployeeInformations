using ClosedXML.Excel;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.AttendanceViewModel;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using FluentScheduler;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;
using System.Net.Mail;
using Attachment = System.Net.Mail.Attachment;
using DateTimeExtensions = EmployeeInformations.Common.Helpers.DateTimeExtensions;
using Duration = EmployeeInformations.Common.Enums.Duration;
using FileFormats = EmployeeInformations.Common.Enums.FileFormats;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace EmployeeInformations.Schedules
{
    //public class EmployeeJobRegistry : Registry
    //{
    //    public EmployeeJobRegistry(EmployeesDbContext employeesDbContext, AttendanceDbContext attendanceDbContext, IHostingEnvironment hostingEnvironment)
    //    {
    //        //Schedule(new EmployeeLeaveCalculationTask(employeesDbContext)).ToRunNow().AndEvery(1).Days();
    //        //Schedule(new EmployeeRequestProbationCompletionTask(employeesDbContext)).ToRunNow().AndEvery(1).Days();
    //        //Schedule(new EmployeeBirthdayCelebrationTask(employeesDbContext)).ToRunNow().AndEvery(1).Days();
    //        //Schedule(new EmployeeWorkAnniversaryCelebrationTask(employeesDbContext)).ToRunNow().AndEvery(1).Days();
    //        //Schedule(new EmailQueueSentMailTask(employeesDbContext)).ToRunNow().AndEvery(60).Seconds();
    //        //Schedule(new AttendanceLogEntryTask(employeesDbContext, attendanceDbContext)).ToRunNow().AndEvery(600).Seconds();
    //        //Schedule(new MailSchedulerTask(employeesDbContext, hostingEnvironment)).ToRunNow().AndEvery(300).Seconds();
    //    }
    //}
}

public class EmployeeLeaveCalculationTask : IJob
{
    private readonly EmployeesDbContext _employeesDbContext;

    public EmployeeLeaveCalculationTask(EmployeesDbContext employeesDbContext)
    {
        _employeesDbContext = employeesDbContext;

    }

    //TODO: Leave calculation by Year
    public void Execute()
    {
        try
        {
            var getAllCompany = _employeesDbContext.Company.Where(x => !x.IsDeleted).ToList();

            //var getAllEmployees = _employeesDbContext.Employees.Where(x => x.EmpId == 158).ToList();

            foreach (var company in getAllCompany)
            {
                var getAllEmployees = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == company.CompanyId).ToList();
                foreach (var item in getAllEmployees)
                {
                    try
                    {
                        LeaveCalculation(item);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            Common.WriteServerErrorLog("1st Scheduler : Leave : " + DateTime.Now);
        }
        catch (Exception)
        {

        }

         void LeaveCalculation(EmployeesEntity item)
        {
            var profileInfo = _employeesDbContext.ProfileInfo.Where(x => x.EmpId == item.EmpId).FirstOrDefault();
            Common.WriteServerErrorLog("Service Leave EmpId : " + item.EmpId);
            if (profileInfo != null)
            {
                Common.WriteServerErrorLog("Service Leave EmpId : " + profileInfo.EmpId);
                var joinDate = Convert.ToDateTime(profileInfo.DateOfJoining);
                Common.WriteServerErrorLog("Service Leave joinDate : " + joinDate);
                var currentDate = DateTime.Now;
                Common.WriteServerErrorLog("Service Leave EmpId 1 : " + item.EmpId);
                var firstDateOfCurrentYear = Convert.ToDateTime("01/01/" + currentDate.Year);
                var lastDateOfCurrentYear = Convert.ToDateTime("31/12/" + currentDate.Year);
                Common.WriteServerErrorLog("Service Leave firstDateOfCurrentYear : " + firstDateOfCurrentYear);
                Common.WriteServerErrorLog("Service Leave lastDateOfCurrentYear : " + lastDateOfCurrentYear);
                // Casual Leave and Sick Leave
                decimal casualLeave = 0.0m;
                decimal sickLeave = 0.0m;
                var probationDate = Convert.ToDateTime(item.ProbationDate);
                Common.WriteServerErrorLog("Service Leave probationDate : " + probationDate);
                if (probationDate == DateTime.MinValue)
                {
                    var allLeaveDetailsByEmpId = _employeesDbContext.AllLeaveDetails.Where(x => x.EmpId == item.EmpId && x.LeaveYear == DateTime.Now.Year).FirstOrDefault();
                    if (allLeaveDetailsByEmpId == null)
                    {
                        Common.WriteServerErrorLog("Service Leave year equal 1 earnedLeave item.EmpId : " + item.EmpId);
                        var allLeaveDetailsEntity = new AllLeaveDetailsEntity();
                        allLeaveDetailsEntity.LeaveYear = DateTime.Now.Year;
                        allLeaveDetailsEntity.CasualLeaveCount = 0;
                        allLeaveDetailsEntity.SickLeaveCount = 0;
                        allLeaveDetailsEntity.EarnedLeaveCount = 0;
                        allLeaveDetailsEntity.CompensatoryOffCount = 0;
                        allLeaveDetailsEntity.EmpId = item.EmpId;
                        allLeaveDetailsEntity.CompanyId = item.CompanyId;
                        _employeesDbContext.AllLeaveDetails.Add(allLeaveDetailsEntity);
                        _employeesDbContext.SaveChanges();
                    }
                }
                else
                {
                    CasualAndSickLeaveCalculation(currentDate, ref firstDateOfCurrentYear, lastDateOfCurrentYear, out casualLeave, out sickLeave, probationDate);

                    // Earned Leave
                    decimal earnedLeave = EarnedLeaveCalculation(joinDate, currentDate);

                    var allLeaveDetailsByEmpId = _employeesDbContext.AllLeaveDetails.Where(x => x.EmpId == item.EmpId && x.LeaveYear == DateTime.Now.Year).FirstOrDefault();
                    if (allLeaveDetailsByEmpId == null)
                    {
                        Common.WriteServerErrorLog("Service Leave year equal 1 earnedLeave item.EmpId : " + item.EmpId);
                        var allLeaveDetailsEntity = new AllLeaveDetailsEntity();
                        allLeaveDetailsEntity.LeaveYear = DateTime.Now.Year;
                        allLeaveDetailsEntity.CasualLeaveCount = casualLeave;
                        allLeaveDetailsEntity.SickLeaveCount = sickLeave;
                        allLeaveDetailsEntity.EarnedLeaveCount = earnedLeave;
                        allLeaveDetailsEntity.EmpId = item.EmpId;
                        allLeaveDetailsEntity.CompanyId = item.CompanyId;
                        _employeesDbContext.AllLeaveDetails.Add(allLeaveDetailsEntity);
                        _employeesDbContext.SaveChanges();
                    }
                    else
                    {
                        allLeaveDetailsByEmpId.CasualLeaveCount = casualLeave;
                        allLeaveDetailsByEmpId.SickLeaveCount = sickLeave;
                        allLeaveDetailsByEmpId.EarnedLeaveCount = earnedLeave;
                        _employeesDbContext.AllLeaveDetails.Update(allLeaveDetailsByEmpId);
                        _employeesDbContext.SaveChanges();
                    }
                }

            }
        }
    }

    public static void CasualAndSickLeaveCalculation(DateTime currentDate, ref DateTime firstDateOfCurrentYear, DateTime lastDateOfCurrentYear, out decimal casualLeave, out decimal sickLeave, DateTime probationDate)
    {
        var monthCountAfterProbation = 0.0m;

        if (probationDate.Year == currentDate.Year)
        {
            Common.WriteServerErrorLog("Service Leave year equal ");
            var date = probationDate.Day;
            if (date <= 10)
            {
                firstDateOfCurrentYear = Convert.ToDateTime(probationDate.Month + "/" + "01/" + currentDate.Year);
            }
            else
            {
                var probationDateAddOneMonth = probationDate.AddMonths(1);
                firstDateOfCurrentYear = Convert.ToDateTime(probationDateAddOneMonth.Month + "/" + "01/" + probationDateAddOneMonth.Year);
            }
            Common.WriteServerErrorLog("Service Leave year equal 1 " + firstDateOfCurrentYear + " month : " + firstDateOfCurrentYear.Month);
            for (int i = firstDateOfCurrentYear.Month; i <= lastDateOfCurrentYear.Month; i++)
            {
                monthCountAfterProbation += 1;
            }

            if (monthCountAfterProbation == 1 || monthCountAfterProbation == 3 || monthCountAfterProbation == 5 || monthCountAfterProbation == 7 || monthCountAfterProbation == 9 || monthCountAfterProbation == 11)
            {
                casualLeave = Convert.ToDecimal((monthCountAfterProbation / 2) + 0.5m);
                sickLeave = Convert.ToDecimal((monthCountAfterProbation / 2) - 0.5m);
            }
            else
            {
                casualLeave = Convert.ToDecimal(monthCountAfterProbation / 2);
                sickLeave = Convert.ToDecimal(monthCountAfterProbation / 2);
            }
            Common.WriteServerErrorLog("Service Leave year equal 2 ");
        }
        else
        {
            casualLeave = Convert.ToDecimal(6);
            sickLeave = Convert.ToDecimal(6);
        }

    }

    public static decimal EarnedLeaveCalculation(DateTime joinDate, DateTime currentDate)
    {
        decimal earnedLeave = 0.0m;

        var day = GetDaysInYear(joinDate);
        Common.WriteServerErrorLog("Service Leave year equal 1 day : " + day);
        var isEarnedLeave = MonthDifference(currentDate, joinDate) > 12 ? true : false;
        Common.WriteServerErrorLog("Service Leave year equal 1 isEarnedLeave : " + isEarnedLeave);
        if (isEarnedLeave)
        {
            Common.WriteServerErrorLog("Service Leave year equal 1 isEarnedLeave : " + isEarnedLeave);
            var earnedDate = joinDate.AddYears(1);
            var earnedDay = earnedDate.Day;
            if (earnedDay <= 10)
            {
                earnedDate = Convert.ToDateTime(earnedDate.Month + "/" + "01/" + earnedDate.Year);
            }
            else
            {
                var earnedDateAddOneMonth = earnedDate.AddMonths(1);
                earnedDate = Convert.ToDateTime(earnedDateAddOneMonth.Month + "/" + "01/" + earnedDateAddOneMonth.Year);
            }
            Common.WriteServerErrorLog("Service Leave year equal 1 earnedDate : " + earnedDate + " month : " + earnedDate.Month);
            var diffEarnedDate = MonthDifference(currentDate, earnedDate);
            if (diffEarnedDate > 0)
            {
                 
              earnedLeave = Convert.ToDecimal(diffEarnedDate);  
            }
            Common.WriteServerErrorLog("Service Leave year equal 1 earnedLeave : " + earnedLeave);
        }
        else
        {
            earnedLeave = 0.0m;
        }

        return earnedLeave;
    }

    static int GetDaysInYear(DateTime date)
    {
        if (date.Equals(DateTime.MinValue))
        {
            return -1;
        }

        DateTime thisYear = new DateTime(date.Year, 1, 1);
        DateTime nextYear = new DateTime(date.Year + 1, 1, 1);

        return (nextYear - thisYear).Days;
    }

    static int MonthDifference(DateTime lValue, DateTime rValue)
    {
        return  (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        
      
    }
}

public class EmployeeRequestProbationCompletionTask : IJob
{
    private readonly EmployeesDbContext _employeesDbContext;

    public EmployeeRequestProbationCompletionTask(EmployeesDbContext employeesDbContext)
    {
        _employeesDbContext = employeesDbContext;
    }

    //TODO: Leave calculation by Year
    public void Execute()
    {
        try
        {
            var currentMonth = DateTime.Now.Month;

            var getAllEmployees = _employeesDbContext.Employees.Where(x => !x.IsDeleted && !x.IsProbationary).ToList();
            var getEmailEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
            var getEmployeeSetting = _employeesDbContext.EmployeeSettingsEntity.FirstOrDefault(x => !x.IsDeleted);
            var probationPeriod = getEmployeeSetting != null ? getEmployeeSetting.ProbationMonths : 0;
            var allEmployees = _employeesDbContext.Employees.Where(x => !x.IsDeleted).ToList();
            foreach (var item in getAllEmployees)
            {
                try
                {
                    Common.WriteServerErrorLog("Service Leave year equal 1 probation item.EmpId : " + item.EmpId);
                    var profileInfo = _employeesDbContext.ProfileInfo.Where(x => x.EmpId == item.EmpId).FirstOrDefault();
                    if (profileInfo != null)
                    {
                        var probationMonth = probationPeriod;
                        var dateOfJoing = Convert.ToDateTime(profileInfo.DateOfJoining).ToString(Constant.DateFormatMonthHyphen);
                        var probationDate = Convert.ToDateTime(profileInfo.DateOfJoining).AddMonths(probationMonth).Date;
                        var beforeProbationDate = Convert.ToDateTime(profileInfo.DateOfJoining).AddMonths(probationMonth).Date.AddDays(-7);
                        var currentDate = DateTime.Now.ToString(Constant.DateFormatMonthHyphen);
                        Common.WriteServerErrorLog("Service Leave year equal  probation 1 : " + item.EmpId);
                        var draftTypeId = (int)EmailDraftType.RequestProbation;
                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.FirstOrDefault(x => x.Id == draftTypeId);
                        if (beforeProbationDate == DateTime.Now.Date)
                        {
                            Common.WriteServerErrorLog("Service Leave year equal 1 probation date  1 ");
                            var designation = _employeesDbContext.Designations.FirstOrDefault(x => x.DesignationId == item.DesignationId)?.DesignationName;
                            var department = _employeesDbContext.Departments.FirstOrDefault(x => x.DepartmentId == item.DepartmentId)?.DepartmentName;
                            var requestEmployeeProbations = _employeesDbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == item.EmpId).ToList();
                            var empNames = string.Empty;
                            foreach (var repEmpId in requestEmployeeProbations)
                            {
                                var emp = allEmployees.FirstOrDefault(d => d.EmpId == repEmpId.ReportingPersonEmpId);
                                if (emp != null)
                                {
                                    empNames += emp.OfficeEmail + ",";
                                }
                            }
                            empNames.Trim(new char[] { ',' });

                            var bodyContent = EmailBodyContent.SendEmail_Body_RequestProbation(item, designation, department, currentDate, dateOfJoing, emailDraftContentEntity.DraftBody);
                            var toEmail = empNames;
                            Common.WriteServerErrorLog("Service Leave year equal 1 probation date  2 ");
                            var emailEntity = new EmailQueueEntity();
                            emailEntity.FromEmail = getEmailEntity.FromEmail;
                            emailEntity.ToEmail = toEmail;
                            emailEntity.Subject = emailDraftContentEntity.Subject;
                            emailEntity.Body = bodyContent;
                            emailEntity.IsSend = false;
                            emailEntity.Reason = Constant.RequestforProbation;
                            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                            emailEntity.CCEmail = emailDraftContentEntity.Email;
                            emailEntity.CompanyId = item.CompanyId;
                            emailEntity.CreatedDate = DateTime.Now;
                            _employeesDbContext.EmailQueueEntitys.Add(emailEntity);
                            _employeesDbContext.SaveChanges();
                            Common.WriteServerErrorLog("Service Leave year equal 1 probation date  3 ");
                        }

                    }

                }
                catch (Exception)
                {

                }
            }
        }
        catch (Exception)
        {

        }
    }
}

public class EmployeeBirthdayCelebrationTask : IJob
{
    private readonly EmployeesDbContext _employeesDbContext;

    public EmployeeBirthdayCelebrationTask(EmployeesDbContext employeesDbContext)
    {
        _employeesDbContext = employeesDbContext;
    }

    //TODO: Birthday calculation by Year
    public void Execute()
    {
        try
        {
            var currentMonth = DateTime.Now.Month;
            var getAllEmployees = _employeesDbContext.Employees.Where(x => !x.IsDeleted).ToList();

            foreach (var item in getAllEmployees)
            {
                try
                {
                    Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  1 " + item.EmpId);
                    var profileInfo = _employeesDbContext.ProfileInfo.Where(x => x.EmpId == item.EmpId).FirstOrDefault();
                    if (profileInfo != null)
                    {
                        Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  2 " + item.EmpId);
                        var dateOfBirth = Convert.ToDateTime(profileInfo.DateOfBirth).ToString("dd-MMM");
                        DateTime date = Convert.ToDateTime(profileInfo.DateOfBirth);
                        var gender = profileInfo.Gender;
                        DateTime previousBirthDate = date.AddDays(-4);

                        var draftTypeId = (int)EmailDraftType.RequestBirthday;
                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.FirstOrDefault(x => x.Id == draftTypeId);

                        if (DateTime.Now.Day == previousBirthDate.Day && DateTime.Now.Month == previousBirthDate.Month)
                        {
                            Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  5 ");
                            var designation = _employeesDbContext.Designations.FirstOrDefault(x => x.DesignationId == item.DesignationId)?.DesignationName;
                            var department = _employeesDbContext.Departments.FirstOrDefault(x => x.DepartmentId == item.DepartmentId)?.DepartmentName;

                            var requestEmployeeProbations = _employeesDbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == item.EmpId).ToList();
                            var empNames = string.Empty;
                            foreach (var repEmpId in requestEmployeeProbations)
                            {
                                var emp = getAllEmployees.FirstOrDefault(d => d.EmpId == repEmpId.ReportingPersonEmpId);
                                if (emp != null)
                                {
                                    empNames += emp.OfficeEmail + ",";
                                }
                            }
                            empNames.Trim(new char[] { ',' });

                            var body = EmailBodyContent.SendEmail_Body_RequestBithday(item, designation, department, dateOfBirth, gender, emailDraftContentEntity.DraftBody);
                            Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  designation " + designation);
                            Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  department " + department);
                            //var subject = "Request for Birthday Celebration";
                            //var displayName = "Support Team";
                            var getEmailEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
                            var toEmail = empNames;
                            var emailEntity = new EmailQueueEntity();
                            emailEntity.FromEmail = getEmailEntity.FromEmail;
                            emailEntity.ToEmail = toEmail;
                            emailEntity.Subject = emailDraftContentEntity.Subject;
                            emailEntity.Body = body;
                            emailEntity.IsSend = false;
                            emailEntity.Reason = Constant.BirthdayCelebration;
                            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                            emailEntity.CompanyId = item.CompanyId;
                            emailEntity.CCEmail = emailDraftContentEntity.Email;
                            emailEntity.CreatedDate = DateTime.Now;
                            _employeesDbContext.EmailQueueEntitys.Add(emailEntity);
                            _employeesDbContext.SaveChanges();
                            Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  6 " + item.EmpId);
                            // Common.SendEmailForLeave(_configuration, body, new List<string>(), subject, displayName);
                            Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  7 " + item.EmpId);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
        }
        catch (Exception)
        {

        }
    }
}

public class EmployeeWorkAnniversaryCelebrationTask : IJob
{
    private readonly EmployeesDbContext _employeesDbContext;

    public EmployeeWorkAnniversaryCelebrationTask(EmployeesDbContext employeesDbContext)
    {
        _employeesDbContext = employeesDbContext;
    }

    //TODO: Work Anniversary
    public void Execute()
    {
        try
        {
            var currentMonth = DateTime.Now.Month;
            var getAllEmployees = _employeesDbContext.Employees.Where(x => !x.IsDeleted).ToList();

            foreach (var item in getAllEmployees)
            {
                try
                {
                    var profileInfo = _employeesDbContext.ProfileInfo.Where(x => x.EmpId == item.EmpId).FirstOrDefault();
                    if (profileInfo != null)
                    {
                        var dateOfJoing = Convert.ToDateTime(profileInfo.DateOfJoining).ToString("dd-MMM");
                        DateTime date = Convert.ToDateTime(profileInfo.DateOfJoining);
                        var gender = profileInfo.Gender;
                        DateTime previousBirthDate = date;

                        var draftTypeId = (int)EmailDraftType.WorkAnniversary;
                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.FirstOrDefault(x => x.Id == draftTypeId);

                        if (DateTime.Now.Day == previousBirthDate.Day && DateTime.Now.Month == previousBirthDate.Month)
                        {
                            var designation = _employeesDbContext.Designations.FirstOrDefault(x => x.DesignationId == item.DesignationId)?.DesignationName;
                            var department = _employeesDbContext.Departments.FirstOrDefault(x => x.DepartmentId == item.DepartmentId)?.DepartmentName;

                            var requestEmployeeProbations = _employeesDbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == item.EmpId).ToList();
                            var empNames = string.Empty;

                            var year = DateTime.Now.Year - date.Year;

                            var body = EmailBodyContent.SendEmail_Body_WorkAnniversary(item, designation, department, dateOfJoing, gender, year, emailDraftContentEntity.DraftBody);

                            foreach (var repEmpId in requestEmployeeProbations)
                            {
                                var emp = getAllEmployees.FirstOrDefault(d => d.EmpId == repEmpId.ReportingPersonEmpId);
                                if (emp != null)
                                {
                                    empNames += emp.OfficeEmail + ",";
                                }
                            }

                            var mail = new List<string>();
                            var mails = emailDraftContentEntity.Email.Split(",");

                            foreach (var mailing in mails)
                            {
                                mail.Add(mailing);
                            }

                            foreach (var repEmpId in mail)
                            {
                                if (repEmpId != null)
                                {
                                    empNames += repEmpId + ",";
                                }
                            }

                            var getEmailEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
                            empNames.Trim(new char[] { ',' });
                            var emailEntity = new EmailQueueEntity();
                            emailEntity.FromEmail = getEmailEntity.FromEmail;
                            emailEntity.ToEmail = item.OfficeEmail;
                            emailEntity.Subject = emailDraftContentEntity.Subject;
                            emailEntity.Body = body;
                            emailEntity.IsSend = false;
                            emailEntity.Reason = Constant.WorkAnniversary;
                            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                            emailEntity.CompanyId = item.CompanyId;
                            emailEntity.CCEmail = empNames;
                            emailEntity.CreatedDate = DateTime.Now;
                            _employeesDbContext.EmailQueueEntitys.Add(emailEntity);
                            _employeesDbContext.SaveChanges();
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        catch (Exception)
        {

        }
    }
}

public class EmailQueueSentMailTask : IJob
{
    private readonly EmployeesDbContext _employeesDbContext;

    public EmailQueueSentMailTask(EmployeesDbContext employeesDbContext)
    {
        _employeesDbContext = employeesDbContext;
    }

    //TODO: Birthday calculation by Year
    public void Execute()
    {
        try
        {
            var getAllEmailQueueEntitys =  _employeesDbContext.EmailQueueEntitys.Where(x => !x.IsSend).ToList();
            var getEmailEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
            foreach (var item in getAllEmailQueueEntitys)
            {
                try
                {
                    Common.WriteServerErrorLog("Service Email Queue Sent Mail Task  1 " + item.EmailQueueID);
                    var fromEmail = item.FromEmail;
                    var password = Common.Decrypt(getEmailEntity.Password);
                    var userName = getEmailEntity.UserName;
                    var host = getEmailEntity.Host;
                    var port = getEmailEntity.EmailPort;
                    var fromAddress = new MailAddress(fromEmail, item.DisplayName);

                    var smtp = new SmtpClient
                    {
                        Host = host,
                        Port = port,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(userName, password)
                    };
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = fromAddress;
                    mailMessage.Subject = item.Subject;
                    mailMessage.Body = item.Body;
                    mailMessage.IsBodyHtml = true;
                    Common.WriteServerErrorLog("Service Email Queue Sent Mail Task  1 " + item.ToEmail);
                    //Add Attachments, here i gave one folder name, and it will add all files in that folder as attachments, Or if you want only one file also can add
                    try
                    {
                        if (item.Attachments != null && !string.IsNullOrEmpty(item.Attachments))
                        {
                            var frm = item.Attachments.Split(",");
                            var strFmtSkillId = "";
                            var finalOutSkill = "";
                            for (int i = 0; i < frm.Count(); i++)
                            {
                                var b = frm[i];
                                strFmtSkillId += string.Format(b + ",");
                                if (!string.IsNullOrEmpty(strFmtSkillId))
                                {
                                    finalOutSkill = strFmtSkillId.Remove(strFmtSkillId.Length - 1, 1);
                                }
                                string attachmentsPath = b;
                                Attachment file = new Attachment(attachmentsPath);
                                mailMessage.Attachments.Add(file);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Common.WriteServerErrorLog(ex.ToString());
                    }                   
                    //if (!string.IsNullOrEmpty(item.Attachments))
                    //{
                    //    string attachmentsPath = item.Attachments;
                    //    Attachment file = new Attachment(attachmentsPath);
                    //    mailMessage.Attachments.Add(file);
                    //}
                    Common.WriteServerErrorLog("Service Email Queue Sent Mail Task  Attachement " + item.EmailQueueID);
                    if (!string.IsNullOrEmpty(item.ToEmail))
                    {
                        var toEmails = item.ToEmail.Split(',');
                        foreach (var cc in toEmails)
                        {
                            if (!string.IsNullOrEmpty(cc))
                            {
                                mailMessage.To.Add(new MailAddress(cc));
                            }
                        }
                    }
                    Common.WriteServerErrorLog("Service Email Queue Sent Mail Task  to email " + item.EmailQueueID);
                    if (!string.IsNullOrEmpty(item.CCEmail))
                    {
                        var ccList = item.CCEmail.Split(',');
                        foreach (var cc in ccList)
                        {
                            if (!string.IsNullOrEmpty(cc))
                            {
                                mailMessage.CC.Add(new MailAddress(cc));
                            }
                        }
                    }
                    Common.WriteServerErrorLog("Service Email Queue Sent Mail Task  to cc email " + item.EmailQueueID);
                    smtp.Send(mailMessage);

                    Common.WriteServerErrorLog("Service Email Queue Sent Mail Task mail sent " + item.EmailQueueID);

                    item.IsSend = true;
                    _employeesDbContext.EmailQueueEntitys.Update(item);
                    _employeesDbContext.SaveChanges();

                    Common.WriteServerErrorLog("Service Email Queue Sent Mail Task IsSend " + item.EmailQueueID);
                }
                catch (Exception ex)
                {
                    Common.WriteServerErrorLog("Service Email Queue error " + ex.StackTrace.ToString());
                }
            }
            Common.WriteServerErrorLog("2nd Mail Scheduler Output : " + DateTime.Now);
        }
        catch (Exception)
        {

        }
    }


}

//public class AttendanceLogEntryTask : IJob
//{
//    private readonly EmployeesDbContext _employeesDbContext;
//    private readonly AttendanceDbContext _attendanceDbContext;

//    public AttendanceLogEntryTask(EmployeesDbContext employeesDbContext, AttendanceDbContext attendanceDbContext)
//    {
//        _employeesDbContext = employeesDbContext;
//        _attendanceDbContext = attendanceDbContext;
//    }

//    //TODO: Attendance Log Entry
//    public void Execute()
//    {
//        try
//        {
//            Common.WriteServerErrorLog("Service Attendance Log Entry Task");
//            var getAttendanceDataFromEntitys = _employeesDbContext.AttendanceEntity.OrderByDescending(x => x.Id).FirstOrDefault();
//            var lastCount = 0;
//            Common.WriteServerErrorLog("Service Attendance Log Entry Task 1 ");
//            if (getAttendanceDataFromEntitys != null)
//            {
//                lastCount = getAttendanceDataFromEntitys.Id;
//            }
//            Common.WriteServerErrorLog("Service Attendance Log Entry Task 2 ");
//            var getAttendanceDataFromAttDbEntitys = _attendanceDbContext.AttendanceEntitys.Where(x => x.Id > lastCount).OrderBy(x => x.Id).ToList();
//            var attendanceEntitys = new List<AttendanceEntitys>();
//            foreach (var item in getAttendanceDataFromAttDbEntitys)
//            {
//                try
//                {
//                    var attendanceEntity = new AttendanceEntitys();
//                    // attendanceEntity.Id = item.Id;
//                    attendanceEntity.EmployeeCode = item.EmployeeCode;
//                    attendanceEntity.LogDateTime = item.LogDateTime;
//                    attendanceEntity.LogDate = item.LogDate;
//                    attendanceEntity.LogTime = item.LogTime;
//                    attendanceEntity.Direction = item.Direction;
//                    attendanceEntitys.Add(attendanceEntity);

//                    Common.WriteServerErrorLog("Service Attendance Log Entry Task 4 " + item.Id);

//                }
//                catch (Exception ex)
//                {
//                    Common.WriteServerErrorLog("Service Email Queue error " + ex.StackTrace.ToString());
//                }
//            }

//            if (attendanceEntitys.Count() > 0)
//            {
//                _employeesDbContext.BulkInsert(attendanceEntitys);
//                //_employeesDbContext.AttendanceEntity.AddRange(attendanceEntitys);
//                //_employeesDbContext.SaveChanges();
//            }
//        }
//        catch (Exception)
//        {

//        }
//    }


//}

public class MailSchedulerTask : IJob
{
    private readonly EmployeesDbContext _employeesDbContext;
    private readonly IHostingEnvironment _hostingEnvironment;

    public MailSchedulerTask(EmployeesDbContext employeesDbContext, IHostingEnvironment hostingEnvironment)
    {
        _employeesDbContext = employeesDbContext;
        _hostingEnvironment = hostingEnvironment;

    }

    //TODO: Leave calculation by Year
    public void Execute()
    {

        try
          {
            var MailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.MailTime <= DateTime.Now && !x.IsDeleted && x.IsActive).ToList();
            foreach (var mailSchedule in MailScheduler)
            {
                if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyAttendance || mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance || mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyAttendance || mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyAttendance)
                {
                    AttendanceMailScheduler(mailSchedule);
                }
                else if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyTimeSheet || mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyTimeSheet ||
                    mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyTimeSheet || mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyTimeSheet)
                {
                    TimeSheetMailScheduler(mailSchedule);
                }
                else if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyLeave || mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyLeave || mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyLeave || mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyLeave)
                {
                    LeaveMailSchedule(mailSchedule);
                }
            }
        }
        catch (Exception)
        {

        }
    }

    static int GetDaysInYear(DateTime date)
    {
        if (date.Equals(DateTime.MinValue))
        {
            return -1;
        }

        DateTime thisYear = new DateTime(date.Year, 1, 1);
        DateTime nextYear = new DateTime(date.Year + 1, 1, 1);

        return (nextYear - thisYear).Days;
    }

    static int MonthDifference(DateTime lValue, DateTime rValue)
    {
        return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
    }

    public void AttendanceMailScheduler(MailSchedulerEntity mailSchedule)
    {
        var attendaceListViewModels = new AttendaceListViewModel();
        var sendMail = false;
        var combinedPath = new List<string>();
        List<DateTime> allDates = new List<DateTime>();

        var activeEmployee = new List<EmployeesEntity>();
        if (mailSchedule != null)
        {
            if (mailSchedule != null && mailSchedule.DurationId == (int)Duration.Once)
            {
                if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyAttendance)
                {                   
                    var day = mailSchedule.MailTime.DayOfWeek;                   
                    if (day == DayOfWeek.Monday)
                    {
                        attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-3).ToString(Constant.DateFormat);
                        attendaceListViewModels.EndDate = attendaceListViewModels.StartDate;
                    }
                    else
                    {
                        attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-1).ToString(Constant.DateFormat);
                        attendaceListViewModels.EndDate = attendaceListViewModels.StartDate;
                    }                   
                }
                else if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                {
                    attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                    attendaceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                }
                else if (mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyAttendance)
                {
                    attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddMonths(-1).ToString(Constant.DateFormat);
                    attendaceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                }
                else if (mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyAttendance)
                {
                    attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddYears(-1).ToString(Constant.DateFormat);
                    attendaceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                }

            }
            else if (mailSchedule?.DurationId == (int)Duration.Daily)
            {
                var day = mailSchedule.MailTime.DayOfWeek;
                if (day == DayOfWeek.Monday)
                {
                    attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-3).ToString(Constant.DateFormat);
                    attendaceListViewModels.EndDate = attendaceListViewModels.StartDate;
                }
                else
                {
                    attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-1).ToString(Constant.DateFormat);
                    attendaceListViewModels.EndDate = attendaceListViewModels.StartDate;
                }
            }
            else if (mailSchedule?.DurationId == (int)Duration.Monthly)
            {
                attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddMonths(-1).ToString(Constant.DateFormat);
                attendaceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
            }
            else if (mailSchedule?.DurationId == (int)Duration.Yearly)
            {
                attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddYears(-1).ToString(Constant.DateFormat);
                attendaceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
            }
            else if (mailSchedule?.DurationId == (int)Duration.Custom)
            {
                var listDays = new List<string>();               
                if (mailSchedule.MailSendingDays != null)
                {
                    listDays = ListOfDays(mailSchedule.MailSendingDays);
                    if (listDays.Count() > 0)
                    {
                        for (int i = 0; i < listDays.Count(); i++)
                        {
                            if (mailSchedule.MailTime.Date >= DateTime.Now.Date && listDays[i] == Convert.ToString(DateTime.Now.DayOfWeek))
                            {                            
                                if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                                {
                                    attendaceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                                    attendaceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                                }                               
                            }
                        }
                    }
                }
            }

            var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModels.StartDate);
            var filterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModels.EndDate);

            if (filterDate != DateTime.MinValue)
            {
                for (DateTime date = filterDate; date <= filterEndDate; date = date.AddDays(1))
                    allDates.Add(date);
            }

            if (allDates.Count > 0)
            {
                var getallemployee = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == mailSchedule.CompanyId).ToList();
                List<int> empIds = mailSchedule.WhomToSend.Split(',').Select(int.Parse).ToList();

                if (empIds[0] != 0 && empIds.Count() > 0)
                {
                    foreach (int empId in empIds)
                    {
                        var getEmployee = getallemployee.Where(x => x.EmpId == empId).FirstOrDefault();
                        if (getEmployee != null)
                        {
                            activeEmployee.Add(new EmployeesEntity()
                            {
                                EmpId = getEmployee.EmpId,
                                EsslId = getEmployee.EsslId,
                                OfficeEmail = getEmployee.OfficeEmail,
                                UserName = getEmployee.UserName,
                                FirstName = getEmployee.FirstName,
                                LastName = getEmployee.LastName,
                                CompanyId = getEmployee.CompanyId,
                                RoleId = getEmployee.RoleId,
                                DesignationId = getEmployee.DesignationId,
                                DepartmentId = getEmployee.DepartmentId,
                                CreatedBy = getEmployee.CreatedBy,
                                CreatedDate = getEmployee.CreatedDate,
                            });
                        }
                    }
                }
                else
                {
                    activeEmployee = getallemployee;
                }

                if (activeEmployee.Count() > 0)
                {
                    if (mailSchedule.WhomToSend == Constant.ZeroStr || empIds.Count() > 0)
                    {
                       // sendMail = SendEmployeeAttendanceForAllEmployee(attendaceListViewModels, empIds, combinedPath, mailSchedule, activeEmployee, allDates);
                    }
                    else
                    {
                        sendMail = SendMail(attendaceListViewModels, mailSchedule, combinedPath);
                    }
                }
            }
        }       
    }

    public List<string> ListOfDays(string strMailDays)
    {
        var listDays = new List<string>();

        if (strMailDays != null)
        {
            List<int> mailDays = strMailDays.Split(',').Select(int.Parse).ToList();          

            if (mailDays[0] == 1)
            {
                listDays.Add(Constant.Sunday);
            }
            if (mailDays[1] == 1)
            {
                listDays.Add(Constant.Monday);
            }
            if (mailDays[2] == 1)
            {
                listDays.Add(Constant.Tuesday);
            }
            if (mailDays[3] == 1)
            {
                listDays.Add(Constant.Wednesday);
            }
            if (mailDays[4] == 1)
            {
                listDays.Add(Constant.Thursday);
            }
            if (mailDays[5] == 1)
            {
                listDays.Add(Constant.Friday);
            }
            if (mailDays[6] == 1)
            {
                listDays.Add(Constant.Saturday);
            }
        }
        return listDays;
    }

    // Method to Get All Employees AttendanceListDataModel From Db
    public string GenarateExcel(List<AttendaceListViewModel> attendaceListViewModels)
    {
        var combinedPath = "";
        if (attendaceListViewModels.Count() > 0)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employee Attendance Details");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
            worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
            worksheet.Cell(currentRow, 3).Value = Constant.Date;
            worksheet.Cell(currentRow, 4).Value = Constant.EntryTime;
            worksheet.Cell(currentRow, 5).Value = Constant.ExitTime;
            worksheet.Cell(currentRow, 6).Value = Constant.TotalHours;
            worksheet.Cell(currentRow, 7).Value = Constant.BreakHours;
            worksheet.Cell(currentRow, 8).Value = Constant.ActualHours;
            worksheet.Cell(currentRow, 9).Value = Constant.TimeSheetHours;
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 9).Style.Font.Bold = true;

            foreach (var user in attendaceListViewModels)
            {
                currentRow++;
                worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                worksheet.Cell(currentRow, 1).Value = user.UserName;
                worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.Date);
                worksheet.Cell(currentRow, 4).Value = Convert.ToString(user.EntryTime);
                worksheet.Cell(currentRow, 5).Value = Convert.ToString(user.ExitTime);
                worksheet.Cell(currentRow, 6).Value = user.TotalHours;
                worksheet.Cell(currentRow, 7).Value = user.BreakHours;
                worksheet.Cell(currentRow, 8).Value = user.InsideOffice;
                worksheet.Cell(currentRow, 9).Value = user.BurningHours;
            }
            var fileName = string.Format("EmployeeAttendanceDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
            var fileId = Guid.NewGuid().ToString() + "_" + fileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
            combinedPath = Path.Combine(path, fileNames);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                workbook.SaveAs(combinedPath);
                memoryStream.Position = 0;
                var content = memoryStream.ToArray();
                //HttpContext.Session.Set(Constant.fileId, content);
            }
        }
        return combinedPath;
    }


    // Method to Get All Employees AttendanceListDataModel From Db
    public List<AttendanceReportDateModel> GetAllAttendancereport(AttendaceListViewModel attendaceListViewModel)
    {

        var status = Constant.ZeroStr;
        var empId = Constant.ZeroStr;
        List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("@empId",empId),
            new KeyValuePair<string, string>("@startDate",attendaceListViewModel.StartDate),
            new KeyValuePair<string, string>("@endDate",attendaceListViewModel.EndDate),
        new KeyValuePair<string, string>("@employeeStatus",status),
        };
        var attendanceReportDateModels = GetAllEmployeesByAttendaceFilter("spGetAttendanceByEmployeeFilterData", p);
        attendanceReportDateModels = attendanceReportDateModels.DistinctBy(x => x.LogDateTime).ToList();
        return attendanceReportDateModels;

    }
    // Method to Get All Employees AttendanceListDataModel From Db
    public List<AttendanceReportDateModel> GetAllEmployeesByAttendaceFilter(string proc, List<KeyValuePair<string, string>> values)
    {
        var parameters = new object[values.Count];
        for (int i = 0; i < values.Count; i++)
            parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

        var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
        paramnames = paramnames.TrimEnd(',');
        proc = proc + " " + paramnames;

        var leaveReportDateModel = _employeesDbContext.AttendanceReportDateModel.FromSqlRaw<AttendanceReportDateModel>(proc, parameters).ToList();
        return leaveReportDateModel;
    }
    // Method to Get All Employees AttendanceDetails
    public AttendaceListViewModels GetAllEmployessAttendanceFilter(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<AttendanceReportDateModel> attendanceReportDateModels, List<TimeSheetEntity> getAllTimeSheetlist)
    {
        try
        {
            var attendaceListViewModels = new AttendaceListViewModels();
            attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();

            foreach (var date in allDates)
            {
                foreach (var item in activeEmployee)
                {
                    var attendacelistViewModel = new AttendaceListViewModel();
                    if (item.EsslId != 0)
                    {

                        var timeSheets = getAllTimeSheetlist.Where(x => x.Startdate.Date == date.Date && !x.IsDeleted).ToList();
                        var stringToNum = Convert.ToString(item.EsslId);

                        var firstInTime = attendanceReportDateModels.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                        var lastInTime = attendanceReportDateModels.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);

                        var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                        var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";

                        if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                        {
                            DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                            DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                            string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                            double seconds = TimeSpan.Parse(dt).TotalSeconds;
                            dt = seconds > 0 ? dt : dt = Constant.TimeFormatZero;
                            var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                            var burningHours = GetTimeSheetWorkHours(item.EmpId, timeSheets);
                            var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                            insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                            attendacelistViewModel.EmployeeId = item.EmpId;
                            attendacelistViewModel.UserName = item.UserName;
                            attendacelistViewModel.EmployeeName = item.FirstName + " " + item.LastName;
                            attendacelistViewModel.Date = date.Date.ToString(Constant.DateFormat);
                            attendacelistViewModel.TotalHours = dt;
                            attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
                            attendacelistViewModel.InsideOffice = insideoffice;
                            attendacelistViewModel.BurningHours = burningHours;
                            attendacelistViewModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                            attendacelistViewModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                            attendacelistViewModel.Id = 0;
                            attendaceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
                        }
                    }
                }
            }
            return attendaceListViewModels;
        }
        catch (Exception)
        {

        }
        return null;
    }
    // Method to Calculate Break Hours
    public string GetBreakHour(string empId, List<AttendanceReportDateModel> attendanceEntities, DateTime filterDate)
    {
        var attendanceEty = attendanceEntities.Where(x => x.EmployeeId == empId && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date).ToList();
        var listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).ToList();

        if (listOfRecords.Count > 0)
        {
            var attendanceReportDateModel = listOfRecords.Take(1).FirstOrDefault();

            if (attendanceReportDateModel.Direction == Constant.EntryTypeOut)
            {
                listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).Skip(2).ToList();
            }
            else
            {
                listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).Skip(1).ToList();
            }
        }
        var outEntry = DateTime.Now;
        var inEntry = DateTime.Now;
        double totalSeconds = 0;
        foreach (var item in listOfRecords)
        {
            if (item.Direction == Constant.EntryTypeOut)
            {
                outEntry = Convert.ToDateTime(item.LogDateTime);
            }
            else if (item.Direction == Constant.EntryTypeIn)
            {
                var seconds = (Convert.ToDateTime(item.LogDateTime) - outEntry).TotalSeconds;
                totalSeconds += seconds;
            }
        }
        if (totalSeconds < 0)
        {
            return Constant.TimeFormatZero;
        }
        else
        {
            TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
            var answer = Convert.ToString((t.Days * 24 + t.Hours) + ":" + t.Minutes + ":" + t.Seconds);
            return answer;
        }
    }
    // Method to Calculate TimeSheet Hours
    public string GetTimeSheetWorkHours(int empId, List<TimeSheetEntity> timeSheetEntitys)
    {
        double totalSeconds = 0;
        var answer = "";
        var timeSheets = timeSheetEntitys.Where(x => x.EmpId == empId).ToList();
        foreach (var ts in timeSheets)
        {
            var seconds = (ts.EndTime - ts.StartTime).TotalSeconds;
            totalSeconds += seconds;
        }

        if (totalSeconds <= 0)
        {
            answer = Constant.TimeFormatZero;
        }
        else
        {
            TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
            answer = Convert.ToString((t.Days * 24 + t.Hours) + ":" + t.Minutes + ":" + t.Seconds);
        }
        return answer;
    }

    // Method to Send Mail For Management
    public bool SendMail(AttendaceListViewModel attendaceListViewModel, MailSchedulerEntity mailSchedulerEntity, List<string> fileId)
    {
        var result = false;
        var combinePath = new List<string>();
        if (attendaceListViewModel.EmployeeId == 0)
        {
            var draftTypeId = (int)EmailDraftType.AttendanceLogForManagement;
            var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(d => d.Id == draftTypeId && d.CompanyId == mailSchedulerEntity.CompanyId).FirstOrDefault();
            MailMessage mailMessage = new MailMessage();
            var toEmail = emailDraftContentEntity.Email;
            var subject = emailDraftContentEntity.Subject;
            var mailBody = EmailBodyContent.SendEmail_Body_AttendanceForManagement(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, emailDraftContentEntity.DraftBody);
            var emailSettingsEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = toEmail;
            emailEntity.Subject = subject;
            emailEntity.Body = mailBody;
            emailEntity.CCEmail = "kavinkumar.c@vphospital.com";

            foreach (var item in fileId)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var fileName = item;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    combinePath.Add(Path.Combine(path, fileName));
                }

            }
            var strSikillName = combinePath;
            if (strSikillName.Count() > 0)
            {
                var str = string.Join(",", strSikillName);
                emailEntity.Attachments = str.TrimEnd(',');
            }
            emailEntity.IsSend = false;
            emailEntity.Reason = "EmployeesAttendanceDetailsReason";
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CreatedDate = DateTime.Now;
            result = InsertEmailQueueEntity(emailEntity);
        }
        return result;
    }

    // Method to Insert Email Queue Entity
    public bool InsertEmailQueueEntity(EmailQueueEntity emailQueueEntity)
    {
        var result = false;
        emailQueueEntity.CompanyId = 1;
        _employeesDbContext.EmailQueueEntitys.Add(emailQueueEntity);
        result = _employeesDbContext.SaveChanges() > 0;
        return result;
    }

    // Method to Create Pdf File 
    //public string CreatePdfFiles(AttendaceListViewModels attendaceListViewModels, string fromDate, string toDate)
    //{
    //    string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "AttendanceDetails");

    //    if (!Directory.Exists(directoryPath))
    //    {
    //        Directory.CreateDirectory(directoryPath);
    //    }

    //    string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + "EmployeeAttendance" + Convert.ToDateTime(fromDate).ToString("yyyyMMdd") + Constant.Hyphen + Convert.ToDateTime(toDate).ToString("yyyyMMdd") + ".pdf");
    //    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
    //    MemoryStream PDFData = new MemoryStream();
    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

    //    var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //    var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.Blue);
    //    var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
    //    var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
    //    var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.Blue);
    //    BaseColor TabelHeaderBackGroundColor = WebColors.GetRgbColor("#EEEEEE");

    //    Rectangle pageSize = writer.PageSize;
    //    // Open the Document for writing
    //    pdfDoc.Open();
    //    //Add elements to the document here

    //    // Create the header table 
    //    PdfPTable headertable = new PdfPTable(3);
    //    headertable.HorizontalAlignment = 0;
    //    headertable.WidthPercentage = 100;
    //    headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

    //    // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
    //    headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
    //    string webRootPath = _hostingEnvironment.WebRootPath;
    //    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
    //    logo.ScaleToFit(100, 70);

    //    {
    //        PdfPCell pdfCelllogo = new PdfPCell(logo);
    //        pdfCelllogo.Border = Rectangle.NO_BORDER;
    //        pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
    //        pdfCelllogo.BorderWidthBottom = 1f;
    //        pdfCelllogo.PaddingTop = 10f;
    //        pdfCelllogo.PaddingBottom = 10f;
    //        headertable.AddCell(pdfCelllogo);
    //    }

    //    {
    //        PdfPCell middlecell = new PdfPCell();
    //        middlecell.Border = Rectangle.NO_BORDER;
    //        middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
    //        middlecell.BorderWidthBottom = 1f;
    //        headertable.AddCell(middlecell);
    //    }

    //    {
    //        PdfPTable nested = new PdfPTable(1);
    //        nested.DefaultCell.Border = Rectangle.NO_BORDER;
    //        PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee Attendance Details - " + fromDate + " " + "To" + " " + toDate, titleFont));
    //        nextPostCell1.Border = Rectangle.NO_BORDER;
    //        nextPostCell1.PaddingBottom = 20f;
    //        nested.AddCell(nextPostCell1);

    //        nested.AddCell("");
    //        PdfPCell nesthousing = new PdfPCell(nested);
    //        nesthousing.Border = Rectangle.NO_BORDER;
    //        nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
    //        nesthousing.BorderWidthBottom = 1f;
    //        nesthousing.Rowspan = 6;
    //        nesthousing.PaddingTop = 10f;
    //        headertable.AddCell(nesthousing);
    //    }

    //    PdfPTable Invoicetable = new PdfPTable(3);
    //    Invoicetable.HorizontalAlignment = 0;
    //    Invoicetable.WidthPercentage = 100;
    //    Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
    //    Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

    //    {
    //        PdfPCell middlecell = new PdfPCell();
    //        middlecell.Border = Rectangle.NO_BORDER;
    //        Invoicetable.AddCell(middlecell);
    //    }

//        {
//                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
//    emptyCell.Border = Rectangle.NO_BORDER;
//                    Invoicetable.AddCell(emptyCell);
//                }

//{
//    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
//    emptyCell.Border = Rectangle.NO_BORDER;
//    Invoicetable.AddCell(emptyCell);
//}

//    {
//        PdfPCell middlecell = new PdfPCell();
//        middlecell.Border = Rectangle.NO_BORDER;
//        middlecell.PaddingTop = 20f;
//        Invoicetable.AddCell(middlecell);
//    }

//    pdfDoc.Add(headertable);
//    pdfDoc.Add(Invoicetable);

//    //Create body table
//    PdfPTable tableLayout = new PdfPTable(9);
//    float[] headers = { 15, 46, 33, 36, 36, 36, 38, 42, 35 }; //Header Widths  
//    tableLayout.SetWidths(headers);
//    tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
//    tableLayout.HeaderRows = 0;

//    //Add header
//    AddCellToHeader(tableLayout, "Id");
//    AddCellToHeader(tableLayout, "Name");
//    AddCellToHeader(tableLayout, "Date");
//    AddCellToHeader(tableLayout, "EntryTime");
//    AddCellToHeader(tableLayout, "ExitTime");
//    AddCellToHeader(tableLayout, "TotalHours");
//    AddCellToHeader(tableLayout, "BreakHours");
//    AddCellToHeader(tableLayout, "ActualHours");
//    AddCellToHeader(tableLayout, "TimeSheetHours");

//    //Add body  

//    foreach (var emp in attendaceListViewModels.AttendaceListViewModel)
//    {

//        AddCellToBody(tableLayout, emp.EmployeeId.ToString());
//        AddCellToBody(tableLayout, emp.EmployeeName);
//        AddCellToBody(tableLayout, emp.Date.ToString());
//        AddCellToBody(tableLayout, emp.EntryTime.ToString());
//        AddCellToBody(tableLayout, emp.ExitTime.ToString());
//        AddCellToBody(tableLayout, emp.TotalHours.ToString());
//        AddCellToBody(tableLayout, emp.BreakHours.ToString());
//        AddCellToBody(tableLayout, emp.InsideOffice.ToString());
//        AddCellToBody(tableLayout, emp.BurningHours.ToString());

//    }
//    pdfDoc.Add(tableLayout);

//    PdfContentByte cb = new PdfContentByte(writer);

//    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
//    cb = new PdfContentByte(writer);
//    cb = writer.DirectContent;
//    cb.BeginText();
//    cb.SetFontAndSize(bf, 8);
//    cb.SetTextMatrix(pageSize.GetLeft(120), 20);
//    cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
//    cb.SetColorFill(BaseColor.LightGray);
//    cb.EndText();

//    //Move the pointer and draw line to separate footer section from rest of page
//    cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
//    cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
//    cb.Stroke();
//    pdfDoc.Close();
//    return filePath;
//}

//// Method to add single cell to the Header  
//private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
//{
//    tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.HELVETICA, 8, 1, BaseColor.White)))
//    {
//        HorizontalAlignment = Element.ALIGN_CENTER,
//        Padding = 5,
//        BackgroundColor = WebColors.GetRgbColor("#1a76d1")
//    });
//}

//// Method to add single cell to the body  
//private static void AddCellToBody(PdfPTable tableLayout, string cellText)
//{
//    tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.HELVETICA, 7, 1, BaseColor.DarkGray)))
//    {
//        HorizontalAlignment = Element.ALIGN_LEFT,
//        Padding = 5
//    });
//}

//// Method to Send mail
//public bool SendEmployeeAttendanceForAllEmployee(AttendaceListViewModel attendaceListViewModel, List<int> empIds, List<string> fileId, MailSchedulerEntity mailSchedulerEntity, List<EmployeesEntity> activeEmployee, List<DateTime> allDates)
//{
//    var result = false;
//    var combinedPath = new List<string>();
//    var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate);
//    var attendaceListViewModels = new AttendaceListViewModels();
//    attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
//    var attendanceList = GetAllAttendancereport(attendaceListViewModel);
//    var getAllTimeSheetlist = _employeesDbContext.TimeSheet.Where(x => x.CreatedDate.Date == flterDate.Date && x.CompanyId == mailSchedulerEntity.CompanyId).ToList();
//    var employees = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == mailSchedulerEntity.CompanyId).ToList();
//    if (empIds[0] == 0)
//    {
//        empIds = activeEmployee.Select(x => x.EmpId).ToList();
//    }
//    var toEmail = "";
//    var employeeName = "";
//    if (empIds.Count() > 0)
//    {
//        foreach (var emp in empIds)
//        {
//            var listEmployees = new List<EmployeesEntity>();

//            var getEmployee = employees.Where(x => x.EmpId == emp).FirstOrDefault();

//            if (getEmployee != null)
//            {
//                listEmployees.Add(new EmployeesEntity()
//                {
//                    EmpId = getEmployee.EmpId,
//                    EsslId = getEmployee.EsslId,
//                    OfficeEmail = getEmployee.OfficeEmail,
//                    UserName = getEmployee.UserName,
//                    FirstName = getEmployee.FirstName,
//                    LastName = getEmployee.LastName,
//                    CompanyId = getEmployee.CompanyId,
//                    RoleId = getEmployee.RoleId,
//                    DesignationId = getEmployee.DesignationId,
//                    DepartmentId = getEmployee.DepartmentId,
//                    CreatedBy = getEmployee.CreatedBy,
//                    CreatedDate = getEmployee.CreatedDate,
//                });
//            }

//            var getEmployeeDetails = employees.Where(x => x.EmpId == emp).FirstOrDefault();

//            var attendanceViewModelList = GetAllEmployessAttendanceFilter(allDates, listEmployees, attendanceList, getAllTimeSheetlist);

//            listEmployees = null;

//            if (attendanceViewModelList.AttendaceListViewModel.Count() >  0)
//            {
//                var attendacelistViewModel = new AttendaceListViewModel();
//                if (getEmployeeDetails.EsslId > 0 && getEmployeeDetails != null)
//                {
//                    if (mailSchedulerEntity.FileFormat == 0 && mailSchedulerEntity.DurationId == (int)Duration.Daily || mailSchedulerEntity.EmailDraftId == (int)MailSchedulerDraft.DailyAttendance)
//                    {
//                        var stringToNum = Convert.ToString(getEmployeeDetails.EsslId);
//                        var tableEmployeeId = Int32.Parse(stringToNum);
//                        var firstInTime = attendanceList.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == flterDate.Date && x.Direction == Constant.EntryTypeIn);
//                        var lastInTime = attendanceList.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == flterDate.Date && x.Direction == Constant.EntryTypeOut);
//                        var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
//                        var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";
//                        toEmail = getEmployeeDetails.OfficeEmail;
//                        if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
//                        {
//                            DateTime StartTime = Convert.ToDateTime(firstInTime?.LogDateTime);
//                            DateTime EndTime = Convert.ToDateTime(lastInTime?.LogDateTime);
//                            string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
//                            dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
//                            var breakHours = GetBreakHour(stringToNum, attendanceList, flterDate);
//                            var burningHours = GetTimeSheetWorkHours(getEmployeeDetails.EmpId, getAllTimeSheetlist);
//                            var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
//                            insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
//                            attendacelistViewModel.EmployeeId = getEmployeeDetails.EmpId;
//                            attendacelistViewModel.UserName = getEmployeeDetails.UserName;
//                            attendacelistViewModel.EmployeeName = getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName;
//                            attendacelistViewModel.Date = flterDate.Date.ToString(Constant.DateFormat);
//                            attendacelistViewModel.TotalHours = dt;
//                            attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
//                            attendacelistViewModel.InsideOffice = insideoffice;
//                            attendacelistViewModel.BurningHours = burningHours;
//                            attendacelistViewModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
//                            attendacelistViewModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
//                            attendacelistViewModel.OfficeEmail = getEmployeeDetails.OfficeEmail;
//                            attendaceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
//                            if (attendacelistViewModel.InsideOffice != null)
//                            {
//                                string time = attendacelistViewModel.InsideOffice;
//                                double seconds = TimeSpan.Parse(time).TotalSeconds;
//                                attendacelistViewModel.TotalSecounds = Convert.ToInt64(seconds);
//                            }
//                        }
//                    }

//                    var attendanceListDataModels = new AttendanceListDataModel();
//                    attendanceListDataModels.EmployeeName = attendacelistViewModel.EmployeeName;
//                    attendanceListDataModels.UserName = attendacelistViewModel.UserName;
//                    attendanceListDataModels.Date = attendacelistViewModel.Date;
//                    attendanceListDataModels.TotalHours = attendacelistViewModel.TotalHours;
//                    attendanceListDataModels.BreakHours = attendacelistViewModel.BreakHours;
//                    attendanceListDataModels.InsideOffice = attendacelistViewModel.InsideOffice;
//                    attendanceListDataModels.EntryTime = attendacelistViewModel.EntryTime;
//                    attendanceListDataModels.ExitTime = attendacelistViewModel.ExitTime;
//                    attendanceListDataModels.BurningHours = attendacelistViewModel.BurningHours;
//                    attendanceListDataModels.TotalSecounds = attendacelistViewModel.TotalSecounds;

//                    toEmail = getEmployee.OfficeEmail;
//                    employeeName = getEmployeeDetails != null ? getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName : String.Empty;

//                    if (attendanceViewModelList.AttendaceListViewModel.Count() > 0 && mailSchedulerEntity.FileFormat == (int)FileFormats.Excel)
//                    {
//                        var excel = GenarateExcel(attendanceViewModelList.AttendaceListViewModel);
//                        combinedPath.Add(excel);
//                    }
//                    else if (attendanceViewModelList.AttendaceListViewModel.Count() > 0 && mailSchedulerEntity.FileFormat == (int)FileFormats.Pdf)
//                    {
//                        var pdf = CreatePdfFiles(attendanceViewModelList, attendaceListViewModel.StartDate, attendaceListViewModel.EndDate);
//                        combinedPath.Add(pdf);
//                    }
//                    else if (attendanceViewModelList.AttendaceListViewModel.Count() > 0 && mailSchedulerEntity.FileFormat == (int)FileFormats.ExcelPDF)
//                    {
//                        var excel = GenarateExcel(attendanceViewModelList.AttendaceListViewModel);
//                        combinedPath.Add(excel);

//                        var pdf = CreatePdfFiles(attendanceViewModelList, attendaceListViewModel.StartDate, attendaceListViewModel.EndDate);
//                        combinedPath.Add(pdf);
//                    }

//                    if (attendanceListDataModels != null && mailSchedulerEntity.DurationId == (int)Duration.Daily || attendanceListDataModels != null && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.DailyAttendance)
//                    {
//                        var draftTypeId = (int)EmailDraftType.AttendanceLog;
//                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(x => x.Id == draftTypeId).FirstOrDefault();
//                        if (emailDraftContentEntity != null && attendanceListDataModels.UserName != null)
//                        {
//                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceAll(attendanceListDataModels, emailDraftContentEntity.DraftBody);
//                            result = InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
//                        }
//                    }
//                    else if (combinedPath.Count() > 0 && mailSchedulerEntity.DurationId == (int)Duration.Custom || combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
//                    {
//                        var draftTypeId = (int)EmailDraftType.WeeklyAttendance;
//                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(x => x.Id == draftTypeId).FirstOrDefault();
//                        if (emailDraftContentEntity != null)
//                        {
//                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, employeeName, emailDraftContentEntity.DraftBody);
//                            result = InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
//                        }
//                    }
//                    else if (combinedPath.Count() > 0 && mailSchedulerEntity.DurationId == (int)Duration.Monthly || combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.MonthlyAttendance)
//                    {
//                        var draftTypeId = (int)EmailDraftType.MonthlyAttendance;
//                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(x => x.Id == draftTypeId).FirstOrDefault();
//                        if (emailDraftContentEntity != null)
//                        {
//                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, employeeName, emailDraftContentEntity.DraftBody);
//                            result = InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
//                        }
//                    }
//                    else if (combinedPath.Count() > 0 && mailSchedulerEntity.DurationId == (int)Duration.Yearly || combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.YearlyAttendance)
//                    {
//                        var draftTypeId = (int)EmailDraftType.YearlyAttendance;
//                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(x => x.Id == draftTypeId).FirstOrDefault();
//                        if (emailDraftContentEntity != null)
//                        {
//                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, employeeName, emailDraftContentEntity.DraftBody);
//                            result = InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
//                        }
//                    }
//                    combinedPath = new List<string>(0);
//                }
//            }
//            if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Daily)
//            {
//                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
//                DateTime mailTime = Convert.ToDateTime(mailScheduler?.MailTime.ToString(Constant.TimeFormat));
//                var mailday = mailTime.DayOfWeek.ToString();
//                if (mailday == Constant.Saturday)
//                {
//                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(3).ToString(Constant.DateFormat));
//                    var sendMailDaily = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
//                    mailScheduler.MailTime = sendMailDaily;
//                }
//                else
//                {
//                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(1).ToString(Constant.DateFormat));
//                    var sendMailDaily = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
//                    mailScheduler.MailTime = sendMailDaily;
//                }

//                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
//                _employeesDbContext.SaveChanges();
//            }
//            else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Custom)
//            {
//                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
//                DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
//                var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(7).ToString(Constant.DateFormat));
//                var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
//                mailScheduler.MailTime = sendMail;                    
//                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
//                _employeesDbContext.SaveChanges();
//            }
//            else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Monthly)
//            {
//                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
//                DateTime mailTime = Convert.ToDateTime(mailScheduler?.MailTime.ToString(Constant.TimeFormat));
//                var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddMonths(1).ToString(Constant.DateFormat));
//                var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
//                mailScheduler.MailTime = sendMail;                    
//                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
//                _employeesDbContext.SaveChanges();
//            }
//            else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Yearly)
//            {
//                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
//                DateTime mailTime = Convert.ToDateTime(mailScheduler?.MailTime.ToString(Constant.TimeFormat));
//                var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddYears(1).ToString(Constant.DateFormat));
//                var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
//                mailScheduler.MailTime = sendMail;                  
//                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
//                _employeesDbContext.SaveChanges();
//            }
//            else if (mailSchedulerEntity.DurationId == (int)Duration.Once && result == true)
//            {
//                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
//                mailScheduler.IsActive = false;
//                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
//                _employeesDbContext.SaveChanges();
//            }
//        }
//    }                
//    return result;
//}   

// Method to Insert Email
public bool InsertEmailAttendance(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, List<string> fileId)
    {
        var result = false;
        var combinePath = new List<string>();
        var emailSettingsEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
        var emailEntity = new EmailQueueEntity();
        emailEntity.FromEmail = emailSettingsEntity.FromEmail;
        emailEntity.ToEmail = officeEmail;
        emailEntity.Subject = emailDraftContentEntity.Subject;
        emailEntity.Body = bodyContent;
        emailEntity.Reason = Constant.AttendanceLogReason;
        emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
        emailEntity.CCEmail = emailDraftContentEntity.Email;

        foreach (var item in fileId)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var fileName = item;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                combinePath.Add(Path.Combine(path, fileName));
            }
        }
        var strSikillName = combinePath;
        if (strSikillName.Count() > 0)
        {
            var str = string.Join(",", strSikillName);
            emailEntity.Attachments = str.TrimEnd(',');
        }
        emailEntity.IsSend = false;
        emailEntity.CreatedDate = DateTime.Now;
        if (emailEntity != null)
        {
            result = InsertEmailQueue(emailEntity);
        }
        return result;
    }

    // Method to Insert Email Queue
    public bool InsertEmailQueue(EmailQueueEntity emailQueueEntity)
    {
        var result = false;
        if (emailQueueEntity != null)
        {
            emailQueueEntity.CompanyId = 1;
            _employeesDbContext.EmailQueueEntitys.Add(emailQueueEntity);
            result = _employeesDbContext.SaveChanges() > 0;
        }
        return result;
    }

    //TimeSheetMailScheduler

    public void TimeSheetMailScheduler(MailSchedulerEntity mailSchedule)
    {
        var timeSheetReports = new TimeSheetReports();
        var sendMail = false;
        var combinedPath = new List<string>();
        List<DateTime> allDates = new List<DateTime>();
        var activeEmployee = new List<EmployeesEntity>();

        if (mailSchedule != null)
        {
            if (mailSchedule != null && mailSchedule.DurationId == (int)Duration.Once)
            {
                if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyTimeSheet)
                {
                    timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                    timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                }
                else if (mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyTimeSheet)
                {
                    timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddMonths(-1).ToString(Constant.DateFormat);
                    timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                }
            }
            //else if (mailSchedule?.DurationId == (int)Duration.Daily)
            //{
            //    timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-1).ToString(Constant.DateFormat);
            //    timeSheetReports.EndDate = timeSheetReports.StartDate;
            //}
            else if (mailSchedule?.DurationId == (int)Duration.Monthly/*mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyAttendance*/)
            {
                timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddMonths(-1).ToString(Constant.DateFormat);
                timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
            }
            //else if (mailSchedule?.DurationId == (int)Duration.Yearly /*mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyAttendance*/)
            //{
            //    timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddYears(-1).ToString(Constant.DateFormat);
            //    timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
            //}
            else if (mailSchedule?.DurationId == (int)Duration.Custom)
            {
                var listDays = new List<string>();
                if (mailSchedule.MailSendingDays != null)
                {
                    listDays = ListOfDays(mailSchedule.MailSendingDays);
                    if (listDays.Count() > 0)
                    {
                        for (int i = 0; i < listDays.Count(); i++)
                        {
                            if (mailSchedule.MailTime.Date >= DateTime.Now.Date && listDays[i] == Convert.ToString(DateTime.Now.DayOfWeek))
                            {
                                if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyTimeSheet)
                                {
                                    timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                                    timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                                }
                            }
                        }
                    }
                }
            }

            var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartDate);
            var filterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.EndDate);

            if (filterDate != DateTime.MinValue)
            {
                for (DateTime date = filterDate; date <= filterEndDate; date = date.AddDays(1))
                    allDates.Add(date);
            }

            if (allDates.Count > 0)
            {
                var getallemployee = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == mailSchedule.CompanyId).ToList();
                List<int> empIds = mailSchedule.WhomToSend.Split(',').Select(int.Parse).ToList();
                if (empIds[0] != 0 && empIds.Count() > 0)
                {
                    foreach (int empId in empIds)
                    {
                        var getEmployee = getallemployee.Where(x => x.EmpId == empId).FirstOrDefault();
                        if (getEmployee != null)
                        {
                            activeEmployee.Add(new EmployeesEntity()
                            {
                                EmpId = getEmployee.EmpId,
                                EsslId = getEmployee.EsslId,
                                OfficeEmail = getEmployee.OfficeEmail,
                                UserName = getEmployee.UserName,
                                FirstName = getEmployee.FirstName,
                                LastName = getEmployee.LastName,
                                CompanyId = getEmployee.CompanyId,
                                RoleId = getEmployee.RoleId,
                                DesignationId = getEmployee.DesignationId,
                                DepartmentId = getEmployee.DepartmentId,
                                CreatedBy = getEmployee.CreatedBy,
                                CreatedDate = getEmployee.CreatedDate,
                            });
                        }
                    }
                }
                else
                {
                    activeEmployee = getallemployee;
                }

                if (activeEmployee.Count() > 0)
                {
                    if (mailSchedule.WhomToSend == Constant.ZeroStr || empIds.Count() > 0)
                    {
                        sendMail = SendTimesheetForAllEmployee(timeSheetReports, empIds, combinedPath, mailSchedule, activeEmployee, allDates);
                    }
                    else
                    {
                        sendMail = SendMailTimeSheet(timeSheetReports, mailSchedule, combinedPath);
                    }
                }
            }
        }
    }

    // Method to Send mail timesheet 

    public bool SendTimesheetForAllEmployee(TimeSheetReports timeSheetReports, List<int> empIds, List<string> fileId, MailSchedulerEntity mailSchedulerEntity, List<EmployeesEntity> activeEmployee, List<DateTime> allDates)
    {
        var result = false;
        var combinedPath = new List<string>();
        var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartDate);
        timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartDate).ToString(Constant.DateFormat);
        timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.EndDate).ToString(Constant.DateFormat);
        var timesheet = new TimeSheetReports();
        timesheet.FilterViewTimeSheet = new List<FilterViewTimeSheet>();
        var timesheetList = GetAllTimesheetReport(timeSheetReports);

        var employees = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == mailSchedulerEntity.CompanyId).ToList();
        if (empIds[0] == 0)
        {
            empIds = activeEmployee.Select(x => x.EmpId).ToList();
        }
        var toEmail = "";
        var employeeName = "";
        if (empIds.Count() > 0)
        {
            foreach (var emp in empIds)
            {
                var listEmployees = new List<EmployeesEntity>();

                var getEmployee = employees.Where(x => x.EmpId == emp).FirstOrDefault();

                if (getEmployee != null)
                {
                    listEmployees.Add(new EmployeesEntity()
                    {
                        EmpId = getEmployee.EmpId,
                        EsslId = getEmployee.EsslId,
                        OfficeEmail = getEmployee.OfficeEmail,
                        UserName = getEmployee.UserName,
                        FirstName = getEmployee.FirstName,
                        LastName = getEmployee.LastName,
                        CompanyId = getEmployee.CompanyId,
                        RoleId = getEmployee.RoleId,
                        DesignationId = getEmployee.DesignationId,
                        DepartmentId = getEmployee.DepartmentId,
                        CreatedBy = getEmployee.CreatedBy,
                        CreatedDate = getEmployee.CreatedDate,
                    });
                }
                var getEmployeeDetails = employees.Where(x => x.EmpId == emp).FirstOrDefault();

                var attendanceViewModelList = GetAllTimesheetFilter(allDates, listEmployees, timesheetList);
                listEmployees = null;

                var filterViewTimeSheetModel = new FilterViewTimeSheet();

                if (getEmployeeDetails.EmpId >= 0 && attendanceViewModelList.FilterViewTimeSheet.Count() > 0)
                {
                    toEmail = getEmployee.OfficeEmail;
                    employeeName = getEmployee != null ? getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName : String.Empty;

                    if (mailSchedulerEntity.FileFormat == (int)FileFormats.Excel)
                    {
                        var excel = GenarateExcelTimeSheet(attendanceViewModelList.FilterViewTimeSheet);
                        combinedPath.Add(excel);
                    }
                    //else if (mailSchedulerEntity.FileFormat == (int)FileFormats.Pdf)
                    //{
                    //    var pdf = CreatePdfTimeSheet(attendanceViewModelList, timeSheetReports.StartDate, timeSheetReports.EndDate);
                    //    combinedPath.Add(pdf);
                    //}
                    else if (mailSchedulerEntity.FileFormat == (int)FileFormats.ExcelPDF)
                    {
                        var excel = GenarateExcelTimeSheet(attendanceViewModelList.FilterViewTimeSheet);
                        combinedPath.Add(excel);

                        //var pdf = CreatePdfTimeSheet(attendanceViewModelList, timeSheetReports.StartDate, timeSheetReports.EndDate);
                        //combinedPath.Add(pdf);
                    }

                    if (combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.WeeklyTimeSheet)
                    {
                        var draftTypeId = (int)EmailDraftType.WeeklyTimeSheet;
                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(x => x.Id == draftTypeId).FirstOrDefault();
                        if (emailDraftContentEntity != null)
                        {
                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(timeSheetReports.StartDate, timeSheetReports.EndDate, employeeName, emailDraftContentEntity.DraftBody);
                            result = InsertEmailTimesheet(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                        }
                    }

                    else if (combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.MonthlyTimeSheet)
                    {
                        var draftTypeId = (int)EmailDraftType.MonthlyTimeSheet;
                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(x => x.Id == draftTypeId).FirstOrDefault();
                        if (emailDraftContentEntity != null)
                        {
                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(timeSheetReports.StartDate, timeSheetReports.EndDate, employeeName, emailDraftContentEntity.DraftBody);
                            result = InsertEmailTimesheet(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                        }
                    }
                    combinedPath = new List<string>(0);
                }

            }
            if (mailSchedulerEntity.DurationId == (int)Duration.Custom && result == true)
            {
                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
                DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(7).ToString(Constant.DateFormat));
                var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                mailScheduler.MailTime = sendMail;                
                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
                _employeesDbContext.SaveChanges();
            }
            else if (mailSchedulerEntity.DurationId == (int)Duration.Monthly && result == true)
            {
                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
                DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddMonths(1).ToString(Constant.DateFormat));
                var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                mailScheduler.MailTime = sendMail;               
                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
                _employeesDbContext.SaveChanges();
            }
            else if (mailSchedulerEntity.DurationId == (int)Duration.Once && result == true)
            {
                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
                mailScheduler.IsActive = false;
                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
                _employeesDbContext.SaveChanges();
            }
        }

        return result;
    }

    // Method to Get All Employees TimesheetDetails
    public TimeSheetReports GetAllTimesheetFilter(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<TimeSheetDataModel> timeSheetDataModels)
    {
        try
        {
            var timesheetdata = new TimeSheetReports();
            timesheetdata.FilterViewTimeSheet = new List<FilterViewTimeSheet>();

            foreach (var date in allDates)
            {
                foreach (var employee in activeEmployee)
                {
                    var timesheet = timeSheetDataModels.Where(x => x.EmployeeId == employee.EmpId && x.StartDate == date).ToList();

                    var filterViewTimeSheet = new FilterViewTimeSheet();
                    if (employee.EmpId > 0 && timeSheetDataModels.Count() > 0)
                    {
                        foreach (var item in timesheet)
                        {
                            var getAllTimeSheetlist = _employeesDbContext.ProjectDetails.Where(x => x.ProjectId == item.ProjectId && x.CompanyId == employee.CompanyId).FirstOrDefault();
                            filterViewTimeSheet.Id = item.Id;
                            filterViewTimeSheet.EmployeeId = item.EmployeeId;
                            filterViewTimeSheet.StartDate = item.StartDate;
                            filterViewTimeSheet.Status = item.Status;
                            filterViewTimeSheet.StartTime = item.StartTime;
                            filterViewTimeSheet.EndTime = item.EndTime;
                            filterViewTimeSheet.ProjectId = item.ProjectId;
                            filterViewTimeSheet.ProjectName = getAllTimeSheetlist.ProjectName;
                            filterViewTimeSheet.EmployeeUserId = item.EmployeeUserId;
                            filterViewTimeSheet.TaskDescription = item.TaskDescription;
                            filterViewTimeSheet.TaskName = item.TaskName;
                            filterViewTimeSheet.AttachmentFileName = item.AttachmentFileName;
                            filterViewTimeSheet.AttachmentFilePath = item.AttachmentFilePath;
                            filterViewTimeSheet.FirstName = item.FirstName;
                            filterViewTimeSheet.LastName = item.LastName;
                            timesheetdata.FilterViewTimeSheet.Add(filterViewTimeSheet);
                        }

                    }
                }
            }
            return timesheetdata;
        }
        catch (Exception)
        {

        }
        return null;
    }


    // Method to Get All Employees AttendanceListDataModel From Db

    public List<TimeSheetDataModel> GetAllTimesheetReport(TimeSheetReports timesheetViewModel)
    {
        var dFrom = string.IsNullOrEmpty(timesheetViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(timesheetViewModel.StartDate).ToString(Constant.DateFormat);
        var dTo = string.IsNullOrEmpty(timesheetViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(timesheetViewModel.StartDate).ToString(Constant.DateFormat);
        var status = Constant.ZeroStr;
        var empId = Constant.ZeroStr;
        List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
            new KeyValuePair<string, string>("@empId", empId),
                new KeyValuePair<string, string>("@projectId",timesheetViewModel.ProjectId.ToString()),
                new KeyValuePair<string, string>("@startTime",timesheetViewModel.StartDate),
                new KeyValuePair<string, string>("@endTime",timesheetViewModel.EndDate),
            };
        var timesheetReportDateModels = GetAllEmployessByTimeSheetFilter("spGetTimeSheetByEmployeeFilterData", p);
        timesheetReportDateModels = timesheetReportDateModels.ToList();
        return timesheetReportDateModels;

    }


    public List<TimeSheetDataModel> GetAllEmployessByTimeSheetFilter(string proc, List<KeyValuePair<string, string>> values)
    {
        var parameters = new object[values.Count];
        for (int i = 0; i < values.Count; i++)
            parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

        var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
        paramnames = paramnames.TrimEnd(',');
        proc = proc + " " + paramnames;

        var timesheetReportDateModels = _employeesDbContext.TimeSheetDataModel.FromSqlRaw<TimeSheetDataModel>(proc, parameters).ToList();
        return timesheetReportDateModels;

    }

    // Method to Send Mail For Management
    public bool SendMailTimeSheet(TimeSheetReports TimeSheetReportsViewModel, MailSchedulerEntity mailSchedulerEntity, List<string> fileId)
    {
        var result = false;
        var combinePath = new List<string>();
        if (TimeSheetReportsViewModel.EmployeeId == 0)
        {
            var draftTypeId = (int)EmailDraftType.AttendanceLogForManagement;
            var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(d => d.Id == draftTypeId && d.CompanyId == mailSchedulerEntity.CompanyId).FirstOrDefault();
            MailMessage mailMessage = new MailMessage();
            var toEmail = emailDraftContentEntity.Email;
            var subject = emailDraftContentEntity.Subject;
            var startDate = string.IsNullOrEmpty(TimeSheetReportsViewModel.StartTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(TimeSheetReportsViewModel.StartTime).ToString(Constant.DateFormat);
            var endDate = string.IsNullOrEmpty(TimeSheetReportsViewModel.EndTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(TimeSheetReportsViewModel.EndTime).ToString(Constant.DateFormat);
            var mailBody = EmailBodyContent.SendEmail_Body_AttendanceForManagement(startDate, endDate, emailDraftContentEntity.DraftBody);
            var emailSettingsEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = toEmail;
            emailEntity.Subject = subject;
            emailEntity.Body = mailBody;
            emailEntity.CCEmail = emailDraftContentEntity.Email;

            foreach (var item in fileId)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var fileName = item;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesTimesheetDetails");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    combinePath.Add(Path.Combine(path, fileName));
                }

            }
            var strSikillName = combinePath;
            if (strSikillName.Count() > 0)
            {
                var str = string.Join(",", strSikillName);
                emailEntity.Attachments = str.TrimEnd(',');
            }
            emailEntity.IsSend = false;
            emailEntity.Reason = "EmployeesTimesheetDetailsReason";
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CreatedDate = DateTime.Now;
            result = InsertEmailQueueEntity(emailEntity);
        }
        return result;
    }

    // method InsertEmailTimesheet

    public bool InsertEmailTimesheet(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, List<string> fileId)
    {
        var result = false;
        var combinePath = new List<string>();
        var emailSettingsEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
        var emailEntity = new EmailQueueEntity();
        emailEntity.FromEmail = emailSettingsEntity.FromEmail;
        emailEntity.ToEmail = officeEmail;
        emailEntity.Subject = emailDraftContentEntity.Subject;
        emailEntity.Body = bodyContent;
        emailEntity.Reason = Constant.TimeSheetReason;
        emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
        emailEntity.CCEmail = emailDraftContentEntity.Email;

        foreach (var item in fileId)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var fileName = item;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesTimesheetDetails");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                combinePath.Add(Path.Combine(path, fileName));
            }
        }
        var strSikillName = combinePath;
        if (strSikillName.Count() > 0)
        {
            var str = string.Join(",", strSikillName);
            emailEntity.Attachments = str.TrimEnd(',');
        }
        emailEntity.IsSend = false;
        emailEntity.CreatedDate = DateTime.Now;
        if (emailEntity != null)
        {
            result = InsertEmailQueue(emailEntity);
        }
        return result;
    }

    // Method to Get All Employees TimeSheetDataModel From Db
    public string GenarateExcelTimeSheet(List<FilterViewTimeSheet> timeSheetReports)
    {
        var combinedPath = "";
        if (timeSheetReports.Count() > 0)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employee TimeSheet Details");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
            worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
            worksheet.Cell(currentRow, 3).Value = Constant.ProjectName;
            worksheet.Cell(currentRow, 4).Value = Constant.TaskName;
            worksheet.Cell(currentRow, 5).Value = Constant.Date;
            worksheet.Cell(currentRow, 6).Value = Constant.StartTime;
            worksheet.Cell(currentRow, 7).Value = Constant.EndTime;
            worksheet.Cell(currentRow, 8).Value = Constant.Status;
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 8).Style.Font.Bold = true;

            foreach (var user in timeSheetReports)
            {
                var strstaus = "";
                var Status = user.Status;
                currentRow++;
                worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.DateFormatRevers);
                worksheet.Cell(currentRow, 6).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                worksheet.Cell(currentRow, 7).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                worksheet.Cell(currentRow, 1).Value = user.EmployeeUserId;
                worksheet.Cell(currentRow, 2).Value = user.FirstName + "" + user.LastName;
                worksheet.Cell(currentRow, 3).Value = user.ProjectName;
                worksheet.Cell(currentRow, 4).Value = user.TaskName;
                worksheet.Cell(currentRow, 5).Value = user.StartDate;
                worksheet.Cell(currentRow, 6).Value = user.StartTime;
                worksheet.Cell(currentRow, 7).Value = user.EndTime;
                if (Status == (int)TimeSheetStatus.Pending)
                {
                    strstaus = Constant.Pending;
                }
                else if (Status == (int)TimeSheetStatus.Inprogress)
                {
                    strstaus = Constant.InProgress;
                }
                else if (Status == (int)TimeSheetStatus.Completed)
                {
                    strstaus = Constant.Completed;
                }
                worksheet.Cell(currentRow, 8).Value = strstaus;

            }

            var fileName = string.Format("EmployeeTimeSheetDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
            var fileId = Guid.NewGuid().ToString() + "_" + fileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeeTimeSheetDetails");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
            combinedPath = Path.Combine(path, fileNames);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                workbook.SaveAs(combinedPath);
                memoryStream.Position = 0;
                var content = memoryStream.ToArray();
                // HttpContext.Session.Set(Constant.fileId, content);
            }

        }
        return combinedPath;
    }

    // Method to Create Pdf TimeSheet File 
    //public string CreatePdfTimeSheet(TimeSheetReports timeSheet, string fromDate, string toDate)
    //{

    //    string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "TimeSheet");
    //    if (!Directory.Exists(directoryPath))
    //    {
    //        Directory.CreateDirectory(directoryPath);
    //    }
    //    string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + "EmployeeTimeSheet" + DateTimeExtensions.ConvertToNotNullDatetime(fromDate).ToString("yyyyMMdd") + Constant.Hyphen + DateTimeExtensions.ConvertToNotNullDatetime(toDate).ToString("yyyyMMdd") + ".pdf");
    //    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
    //    MemoryStream PDFData = new MemoryStream();
    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));



    //    var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
    //    var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
    //    var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
    //    var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
    //    var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
    //    BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

    //    Rectangle pageSize = writer.PageSize;

    //    pdfDoc.Open();

    //    PdfPTable headertable = new PdfPTable(3);
    //    headertable.HorizontalAlignment = 0;
    //    headertable.WidthPercentage = 100;
    //    headertable.SetWidths(new float[] { 200f, 5f, 350f });



    //    headertable.DefaultCell.Border = Rectangle.BOX;
    //    string webRootPath = _hostingEnvironment.WebRootPath;
    //    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
    //    logo.ScaleToFit(100, 70);

    //    {
    //        PdfPCell pdfCelllogo = new PdfPCell(logo);
    //        pdfCelllogo.Border = Rectangle.NO_BORDER;
    //        pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
    //        pdfCelllogo.BorderWidthBottom = 1f;
    //        pdfCelllogo.PaddingTop = 10f;
    //        pdfCelllogo.PaddingBottom = 10f;
    //        headertable.AddCell(pdfCelllogo);
    //    }

    //    {
    //        PdfPCell middlecell = new PdfPCell();
    //        middlecell.Border = Rectangle.NO_BORDER;
    //        middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
    //        middlecell.BorderWidthBottom = 1f;
    //        headertable.AddCell(middlecell);
    //    }

    //    {
    //        PdfPTable nested = new PdfPTable(1);
    //        nested.DefaultCell.Border = Rectangle.NO_BORDER;
    //        PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Time Sheet Log Report - " + fromDate + " " + "To" + " " + toDate, titleFont));
    //        nextPostCell1.Border = Rectangle.NO_BORDER;
    //        nextPostCell1.PaddingBottom = 20f;
    //        nested.AddCell(nextPostCell1);

    //        nested.AddCell("");
    //        PdfPCell nesthousing = new PdfPCell(nested);
    //        nesthousing.Border = Rectangle.NO_BORDER;
    //        nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
    //        nesthousing.BorderWidthBottom = 1f;
    //        nesthousing.Rowspan = 6;
    //        nesthousing.PaddingTop = 10f;
    //        headertable.AddCell(nesthousing);
    //    }

    //    PdfPTable Invoicetable = new PdfPTable(3);
    //    Invoicetable.HorizontalAlignment = 0;
    //    Invoicetable.WidthPercentage = 100;
    //    Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
    //    Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

    //    {
    //        PdfPCell middlecell = new PdfPCell();
    //        middlecell.Border = Rectangle.NO_BORDER;
    //        Invoicetable.AddCell(middlecell);
    //    }

//    {
//     PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
//    emptyCell.Border = Rectangle.NO_BORDER;
//     Invoicetable.AddCell(emptyCell);
// }

//{
//    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
//    emptyCell.Border = Rectangle.NO_BORDER;
//    Invoicetable.AddCell(emptyCell);
//}

//    {
//        PdfPCell middlecell = new PdfPCell();
//        middlecell.Border = Rectangle.NO_BORDER;
//        middlecell.PaddingTop = 20f;
//        Invoicetable.AddCell(middlecell);
//    }

//    pdfDoc.Add(headertable);
//    pdfDoc.Add(Invoicetable);

//    //Create body table
//    PdfPTable itemTable = new PdfPTable(8);
//    float[] headers = { 58, 40, 52, 68, 45, 46, 44, 46 }; //Header Widths  
//    itemTable.SetWidths(headers);

//    //AddCellToHeader(itemTable, "Name");
//    //AddCellToHeader(itemTable, "Id");
//    //AddCellToHeader(itemTable, "Project");
//    //AddCellToHeader(itemTable, "Task Name");
//    //AddCellToHeader(itemTable, "Date");
//    //AddCellToHeader(itemTable, "Start Time");
//    //AddCellToHeader(itemTable, "End Time");
//    //AddCellToHeader(itemTable, "Status");


//    //foreach (var emp in timeSheet.FilterViewTimeSheet)
//    //{
//    //    AddCellToBody(itemTable, emp.FirstName + " " + emp.LastName);
//    //    AddCellToBody(itemTable, emp.EmployeeUserId);
//    //    AddCellToBody(itemTable, emp.ProjectName);
//    //    AddCellToBody(itemTable, emp.TaskName);
//    //    AddCellToBody(itemTable, emp.StartDate.ToString(Constant.DateFormat));
//    //    AddCellToBody(itemTable, emp.StartTime.ToString(Constant.TimeFormatlog));
//    //    AddCellToBody(itemTable, emp.EndTime.ToString(Constant.TimeFormatlog));
//    //    if (emp.Status == 1)
//    //    {
//    //        AddCellToBody(itemTable, Constant.Pending);
//    //    }
//    //    else if (emp.Status == 2)
//    //    {
//    //        AddCellToBody(itemTable, Constant.InProgress);
//    //    }
//    //    else
//    //    {
//    //        AddCellToBody(itemTable, Constant.Completed);
//    //    }

//    }
//    pdfDoc.Add(itemTable);

//    PdfContentByte cb = new PdfContentByte(writer);

//    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
//    cb = new PdfContentByte(writer);
//    cb = writer.DirectContent;
//    cb.BeginText();
//    cb.SetFontAndSize(bf, 8);
//    cb.SetTextMatrix(pageSize.GetLeft(120), 20);
//    cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
//    cb.EndText();

//    //Move the pointer and draw line to separate footer section from rest of page
//    cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
//    cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
//    cb.Stroke();
//    pdfDoc.Close();
//    return filePath;
//}

//LeaveMailSchedule

public void LeaveMailSchedule(MailSchedulerEntity mailSchedule)
    {
        var employeeLeaveViewModel = new EmployeeLeaveViewModel();
        var sentMail = false;
        var combinedPath = new List<string>();
        List<DateTime> allDates = new List<DateTime>();
        var activeEmployee = new List<EmployeesEntity>();

        if (mailSchedule != null)
        {
            if (mailSchedule.DurationId == (int)Duration.Once)
            {

                if (mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyLeave)
                {
                    employeeLeaveViewModel.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.AddMonths(-1).ToString(Constant.DateFormat));
                    employeeLeaveViewModel.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat));
                }
            }
            else if (mailSchedule?.DurationId == (int)Duration.Monthly)
            {
                employeeLeaveViewModel.LeaveFromDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.AddMonths(-1).ToString(Constant.DateFormat));
                employeeLeaveViewModel.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat));
            }

            var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(employeeLeaveViewModel.LeaveFromDate.ToString(Constant.DateFormat));
            var filterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(employeeLeaveViewModel.LeaveToDate.ToString(Constant.DateFormat));

            if (filterDate != DateTime.MinValue)
            {
                for (DateTime date = filterDate; date <= filterEndDate; date = date.AddDays(1))
                    allDates.Add(date);
            }

            //else if (mailSchedule.DurationId == (int)Duration.Custom)
            //{

            //    var listDays = new List<string>();

            //    if (mailSchedule.MailSendingDays != null)
            //    {
            //        listDays = ListOfDays(mailSchedule.MailSendingDays);

            //        if (listDays.Count() > 0)
            //        {
            //            for (int i = 0; i < listDays.Count(); i++)
            //            {
            //                if (mailSchedule.MailTime.Date == DateTime.Now.Date && listDays[i] == Convert.ToString(DateTime.Now.DayOfWeek))
            //                {

            //                    if (mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyLeave)
            //                    {
            //                        employeeLeaveViewModel.LeaveFromDate = mailSchedule.MailTime.AddMonths(-1).Date;
            //                        employeeLeaveViewModel.LeaveToDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat));
            //                    }

            //                    var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(employeeLeaveViewModel.LeaveFromDate.ToString(Constant.DateFormat));
            //                    var filterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(employeeLeaveViewModel.LeaveToDate.ToString(Constant.DateFormat));

            //                    if (filterDate != DateTime.MinValue)
            //                    {
            //                        for (DateTime date = filterDate; date <= filterEndDate; date = date.AddDays(1))
            //                            allDates.Add(date);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            if (allDates.Count() > 0)
            {
                var getallemployee = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == mailSchedule.CompanyId).ToList();
                List<int> empIds = mailSchedule.WhomToSend.Split(',').Select(int.Parse).ToList();
                if (empIds[0] != 0 && empIds.Count() > 0)
                {
                    foreach (int empId in empIds)
                    {
                        var getEmployee = getallemployee.Where(x => x.EmpId == empId).FirstOrDefault();
                        if (getEmployee != null)
                        {
                            activeEmployee.Add(new EmployeesEntity()
                            {
                                EmpId = getEmployee.EmpId,
                                EsslId = getEmployee.EsslId,
                                OfficeEmail = getEmployee.OfficeEmail,
                                UserName = getEmployee.UserName,
                                FirstName = getEmployee.FirstName,
                                LastName = getEmployee.LastName,
                                CompanyId = getEmployee.CompanyId,
                                RoleId = getEmployee.RoleId,
                                DesignationId = getEmployee.DesignationId,
                                DepartmentId = getEmployee.DepartmentId,
                                CreatedBy = getEmployee.CreatedBy,
                                CreatedDate = getEmployee.CreatedDate,
                            });
                        }
                    }
                }
                else
                {
                    activeEmployee = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == mailSchedule.CompanyId).ToList();
                }
                if (mailSchedule.WhomToSend == Constant.ZeroStr || empIds.Count() > 0)
                {
                    sentMail = SentMailAllEmployees(employeeLeaveViewModel, empIds, combinedPath, mailSchedule, activeEmployee, allDates);
                }
            }
        }
    }

    public bool SentMailAllEmployees(EmployeeLeaveViewModel employeeLeaveViewModel, List<int> empIds, List<string> fileId, MailSchedulerEntity mailSchedulerEntity, List<EmployeesEntity> activeEmployee, List<DateTime> allDates)

    {
        var result = false;
        var combinedPath = new List<string>();
        var filterDate = employeeLeaveViewModel.LeaveFromDate;
        var toDate = employeeLeaveViewModel.LeaveToDate;

        var leaveReport = GetAllEmployessByLeaveFilter(employeeLeaveViewModel);

        var employees = _employeesDbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == mailSchedulerEntity.CompanyId).ToList();
        if (empIds[0] == 0)
        {
            empIds = activeEmployee.Select(x => x.EmpId).ToList();
        }
        var toMail = "";
        if (empIds.Count() > 0)
        {
            foreach (var item in empIds)
            {
                var employeeList = new List<EmployeesEntity>();

                var employee = employees.Where(x => x.EmpId == item).FirstOrDefault();
                if (employee != null)
                {
                    employeeList.Add(new EmployeesEntity()
                    {
                        EmpId = employee.EmpId,
                        EsslId = employee.EsslId,
                        OfficeEmail = employee.OfficeEmail,
                        UserName = employee.UserName,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        CompanyId = employee.CompanyId,
                        RoleId = employee.RoleId,
                        DesignationId = employee.DesignationId,
                        DepartmentId = employee.DepartmentId,
                        CreatedBy = employee.CreatedBy,
                        CreatedDate = employee.CreatedDate,
                    });
                }

                toMail = employee.OfficeEmail;

                var getEmployee = employees.Where(x => x.EmpId == item).FirstOrDefault();
                var LeaveListModel = GetAllEmployeeLeaveFilter(allDates, employeeList, leaveReport);
                var employeeLeaveViewModels = new EmployeeLeaveViewModel();

                if (getEmployee.EsslId > 0 && LeaveListModel.leaveFilterViewModels.Count() > 0)
                {
                    if (mailSchedulerEntity.FileFormat == (int)FileFormats.Excel)
                    {
                        var excel = CreateExcel(LeaveListModel.leaveFilterViewModels);
                        combinedPath.Add(excel);
                    }

                    else if (mailSchedulerEntity.FileFormat == (int)FileFormats.Pdf)
                    {
                        var pdf = CreatePdfForLeave(LeaveListModel.leaveFilterViewModels, filterDate, toDate);
                        combinedPath.Add(pdf);
                    }
                    else if (mailSchedulerEntity.FileFormat == (int)FileFormats.ExcelPDF)
                    {
                        var excel = CreateExcel(LeaveListModel.leaveFilterViewModels);
                        combinedPath.Add(excel);

                        var pdf = CreatePdfForLeave(LeaveListModel.leaveFilterViewModels, filterDate, toDate);
                        combinedPath.Add(pdf);
                    }
                    if (combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.MonthlyLeave)
                    {
                        var draftTypeId = (int)EmailDraftType.MonthlyLeave;
                        var emailDraftContentEntity = _employeesDbContext.EmailDraftContent.Where(x => x.Id == draftTypeId).FirstOrDefault();
                        if (emailDraftContentEntity != null)
                        {
                            var bodyContent = EmailBodyContent.SendEmail_Body_LeaveForEmployeeWeekly(Convert.ToString(filterDate.ToString(Constant.DateFormat)), Convert.ToString(toDate.ToString(Constant.DateFormat)), getEmployee.FirstName, emailDraftContentEntity.DraftBody);
                            result = InsertEmailLeave(toMail, emailDraftContentEntity, bodyContent, combinedPath);
                        }
                    }
                    combinedPath = new List<string>(0);
                }
            }
            if (mailSchedulerEntity.DurationId == (int)Duration.Monthly && result == true)
            {
                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();

                DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));

                var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddMonths(1).ToString(Constant.DateFormat));

                var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);

                mailScheduler.MailTime = sendMail;                
                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
                _employeesDbContext.SaveChanges();
            }
            else if (mailSchedulerEntity.DurationId == (int)Duration.Once && result == true)
            {
                var mailScheduler = _employeesDbContext.MailSchedulerEntity.Where(x => x.SchedulerId == mailSchedulerEntity.SchedulerId).FirstOrDefault();
                mailScheduler.IsActive = false;
                _employeesDbContext.MailSchedulerEntity.Update(mailScheduler);
                _employeesDbContext.SaveChanges();
            }
        }
        return result;
    }

    public bool InsertEmailLeave(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, List<string> fileId)
    {
        var result = false;
        var combinePath = new List<string>();
        var emailSettingsEntity = _employeesDbContext.EmailSettings.FirstOrDefault(x => !x.IsDeleted);
        var emailEntity = new EmailQueueEntity();
        emailEntity.FromEmail = emailSettingsEntity.FromEmail;
        emailEntity.ToEmail = officeEmail;
        emailEntity.Subject = emailDraftContentEntity.Subject;
        emailEntity.Body = bodyContent;
        emailEntity.Reason = Constant.LeaveReport;
        emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
        emailEntity.CCEmail = emailDraftContentEntity.Email;

        foreach (var item in fileId)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var fileName = item;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesLeaveDetails");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                combinePath.Add(Path.Combine(path, fileName));
            }
        }
        var strSikillName = combinePath;
        if (strSikillName.Count() > 0)
        {
            var str = string.Join(",", strSikillName);
            emailEntity.Attachments = str.TrimEnd(',');
        }
        emailEntity.IsSend = false;
        emailEntity.CreatedDate = DateTime.Now;
        if (emailEntity != null)
        {
            result = InsertEmailQueue(emailEntity);
        }
        return result;
    }

    public string CreateExcel(List<LeaveFilterViewModel> employeeViewModel)
    {
        var combinedPath = "";
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Employee Leave Details");
        var currentRow = 1;
        worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
        worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
        worksheet.Cell(currentRow, 3).Value = Constant.LeaveType;
        worksheet.Cell(currentRow, 4).Value = Constant.LeaveFromDate;
        worksheet.Cell(currentRow, 5).Value = Constant.LeaveToDate;
        worksheet.Cell(currentRow, 6).Value = Constant.Reason;
        worksheet.Cell(currentRow, 7).Value = Constant.LeaveCount;
        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
        foreach (var user in employeeViewModel)
        {
            currentRow++;
            worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat(Constant.DateFormat);
            worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.DateFormat);
            worksheet.Cell(currentRow, 1).Value = user.EmployeeUserName;
            worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
            worksheet.Cell(currentRow, 3).Value = user.LeaveTypes;
            worksheet.Cell(currentRow, 4).Value = user.LeaveFromDate;
            worksheet.Cell(currentRow, 5).Value = user.LeaveToDate;
            worksheet.Cell(currentRow, 6).Value = user.Reason;
            worksheet.Cell(currentRow, 7).Value = user.LeaveCount;

        }
        var fileName = string.Format("EmployeeLeaveDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
        var fileId = Guid.NewGuid().ToString() + "_" + fileName;
        var path = Path.Combine(Directory.GetCurrentDirectory(), "LeaveAttachments");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
        combinedPath = Path.Combine(path, fileNames);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            workbook.SaveAs(memoryStream);
            workbook.SaveAs(combinedPath);
            memoryStream.Position = 0;
            var content = memoryStream.ToArray();
            //HttpContext.Session.Set(Constant.fileId, content);
        }
        return combinedPath;
    }

    public string CreatePdfForLeave(List<LeaveFilterViewModel> leaveFilters, DateTime filterDate, DateTime toDate)
    {
        string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "LeaveReport");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + Constant.LeaveReport + Convert.ToDateTime(filterDate).ToString(Constant.DateFormatYMD) + Constant.Hyphen + Convert.ToDateTime(toDate).ToString(Constant.DateFormatYMD) + Constant.pdf);
        Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
        MemoryStream PDFData = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

        var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
        var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
        var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
        var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
        BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

        Rectangle pageSize = writer.PageSize;
        // Open the Document for writing
        pdfDoc.Open();
        //Add elements to the document here

        // Create the header table 
        PdfPTable headertable = new PdfPTable(3);
        headertable.HorizontalAlignment = 0;
        headertable.WidthPercentage = 100;
        headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

        // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
        headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
        string webRootPath = _hostingEnvironment.WebRootPath;
        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
        logo.ScaleToFit(100, 70);

        {
            PdfPCell pdfCelllogo = new PdfPCell(logo);
            pdfCelllogo.Border = Rectangle.NO_BORDER;
            pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            pdfCelllogo.BorderWidthBottom = 1f;
            pdfCelllogo.PaddingTop = 10f;
            pdfCelllogo.PaddingBottom = 10f;
            headertable.AddCell(pdfCelllogo);
        }

        {
            PdfPCell middlecell = new PdfPCell();
            middlecell.Border = Rectangle.NO_BORDER;
            middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            middlecell.BorderWidthBottom = 1f;
            headertable.AddCell(middlecell);
        }

        {
            PdfPTable nested = new PdfPTable(1);
            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Leave Report - " + filterDate.ToString(Constant.DateFormat) + " " + "To" + " " + toDate.ToString(Constant.DateFormat), titleFont));
            nextPostCell1.Border = Rectangle.NO_BORDER;
            nextPostCell1.PaddingBottom = 20f;
            nested.AddCell(nextPostCell1);

            nested.AddCell("");
            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Border = Rectangle.NO_BORDER;
            nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            nesthousing.BorderWidthBottom = 1f;
            nesthousing.Rowspan = 6;
            nesthousing.PaddingTop = 10f;
            headertable.AddCell(nesthousing);
        }

        PdfPTable Invoicetable = new PdfPTable(3);
        Invoicetable.HorizontalAlignment = 0;
        Invoicetable.WidthPercentage = 100;
        Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
        Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

        {
            PdfPCell middlecell = new PdfPCell();
            middlecell.Border = Rectangle.NO_BORDER;
            Invoicetable.AddCell(middlecell);
        }

        {
            PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
            emptyCell.Border = Rectangle.NO_BORDER;
            Invoicetable.AddCell(emptyCell);
        }

        {
            PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
            emptyCell.Border = Rectangle.NO_BORDER;
            Invoicetable.AddCell(emptyCell);
        }

        {
            PdfPCell middlecell = new PdfPCell();
            middlecell.Border = Rectangle.NO_BORDER;
            middlecell.PaddingTop = 20f;
            Invoicetable.AddCell(middlecell);
        }

        pdfDoc.Add(headertable);
        pdfDoc.Add(Invoicetable);

        //Create body table
        PdfPTable tableLayout = new PdfPTable(7);
        float[] headers = { 40, 50, 46, 33, 36, 36, 36, }; //Header Widths  
        tableLayout.SetWidths(headers);
        tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
        tableLayout.HeaderRows = 0;

        ////Add header
        //AddCellToHader(tableLayout, Constant.EmployeeUserId);
        //AddCellToHeader(tableLayout, Constant.UserName);
        //AddCellToHeader(tableLayout, Constant.LeaveType);
        //AddCellToHeader(tableLayout, Constant.LeaveFromDate);
        //AddCellToHeader(tableLayout, Constant.LeaveToDate);
        //AddCellToHeader(tableLayout, Constant.Reason);
        //AddCellToHeader(tableLayout, Constant.LeaveCount);

        ////Add body  
        //foreach (var emp in leaveFilters)
        //{
        //    AddCellToBody(tableLayout, emp.EmployeeUserName);
        //    AddCellToBody(tableLayout, emp.EmployeeName);
        //    AddCellToBody(tableLayout, emp.LeaveTypes);
        //    AddCellToBody(tableLayout, Convert.ToString(emp.LeaveFromDate.ToString(Constant.DateFormat)));
        //    AddCellToBody(tableLayout, Convert.ToString(emp.LeaveToDate.ToString(Constant.DateFormat)));
        //    AddCellToBody(tableLayout, emp.Reason);
        //    AddCellToBody(tableLayout, Convert.ToString(emp.LeaveCount));
        //}
        //pdfDoc.Add(tableLayout);

        PdfContentByte cb = new PdfContentByte(writer);

        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
        cb = new PdfContentByte(writer);
        cb = writer.DirectContent;
        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.SetTextMatrix(pageSize.GetLeft(120), 20);
        cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
        cb.SetColorFill(BaseColor.LIGHT_GRAY);
        cb.EndText();

        //Move the pointer and draw line to separate footer section from rest of page
        cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
        cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
        cb.Stroke();
        pdfDoc.Close();
        return filePath;
    }
    public List<LeaveReportDateModel> GetAllEmployessByLeaveFilter(EmployeeLeaveViewModel employeeLeaveViewModel)
    {

        var dFrom = employeeLeaveViewModel.LeaveFromDate.ToString(Constant.DateFormatMDY);
        var dTo = employeeLeaveViewModel.LeaveToDate.ToString(Constant.DateFormatMDY);
        var leaveTypeId = Convert.ToString(0);
        var empId = Convert.ToString(0);

        List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@empId", empId),
                new KeyValuePair<string, string>("@leaveTypeId",leaveTypeId),
                new KeyValuePair<string, string>("@startDate",dFrom),
                new KeyValuePair<string, string>("@endDate",dTo),
            };
        var filterEmployeeLeave = GetAllEmployeeLeave("spGetLeaveTypeCountByEmployeeFilterData", p);
        return filterEmployeeLeave;
    }
    public List<LeaveReportDateModel> GetAllEmployeeLeave(string proc, List<KeyValuePair<string, string>> values)
    {
        var parameters = new object[values.Count];
        for (int i = 0; i < values.Count; i++)
            parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

        var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
        paramnames = paramnames.TrimEnd(',');
        proc = proc + " " + paramnames;

        var leaveReportDateModel = _employeesDbContext.LeaveReportDateModel.FromSqlRaw<LeaveReportDateModel>(proc, parameters).ToList();
        return leaveReportDateModel;


    }
    public EmployeeLeaveViewModel GetAllEmployeeLeaveFilter(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<LeaveReportDateModel> leaveReportDateModels)
    {

        var listOfRecord = new EmployeeLeaveViewModel();
        listOfRecord.leaveFilterViewModels = new List<LeaveFilterViewModel>();
        foreach (var date in allDates)
        {
            foreach (var employee in activeEmployee)
            {
                var leaveReport = leaveReportDateModels.Where(x => x.EmployeeId == employee.EmpId && x.LeaveFromDate == date).ToList();

                var leavefilterview = new LeaveFilterViewModel();

                if (employee.EsslId > 0)
                {
                    foreach (var item in leaveReport)
                    {
                        leavefilterview.EmployeeUserName = item.EmployeeUserId;
                        leavefilterview.EmployeeName = item.FirstName + "" + item.LastName;
                        leavefilterview.LeaveTypes = item.LeaveType;
                        leavefilterview.LeaveFromDate = item.LeaveFromDate;
                        leavefilterview.LeaveToDate = item.LeaveToDate;
                        leavefilterview.Reason = item.Reason;
                        leavefilterview.LeaveCount = item.LeaveCount;
                        listOfRecord.leaveFilterViewModels.Add(leavefilterview);

                    }
                }
            }
        }
        return listOfRecord;
    }


}
