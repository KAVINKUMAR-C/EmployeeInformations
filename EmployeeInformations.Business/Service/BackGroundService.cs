using ClosedXML.Excel;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.AttendanceViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using System.Net;
using System.Net.Mail;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using Attachment = System.Net.Mail.Attachment;
using DateTimeExtensions = EmployeeInformations.Common.Helpers.DateTimeExtensions;
//using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using Paragraph = iTextSharp.text.Paragraph;

namespace EmployeeInformations.Business.Service
{
    public class BackGroundService : IBackGroundService
    {

        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IProjectDetailsRepository _projectDetailsRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDashboardService _dashboardService;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IAttendanceService _attendanceService;


        public BackGroundService(ICompanyRepository companyRepository, IEmployeesRepository employeesRepository, IEmailDraftRepository emailDraftRepository, ILeaveRepository leaveRepository, IAttendanceRepository attendanceRepository, ITimeSheetRepository timeSheetRepository, IReportRepository reportRepository, IProjectDetailsRepository projectDetailsRepository, IHostingEnvironment hostingEnvironment, IDashboardService dashboardService, IDashboardRepository dashboardRepository, IAttendanceService attendanceService)
        {
            _companyRepository = companyRepository;
            _employeesRepository = employeesRepository;
            _emailDraftRepository = emailDraftRepository;
            _leaveRepository = leaveRepository;
            _attendanceRepository = attendanceRepository;
            _timeSheetRepository = timeSheetRepository;
            _reportRepository = reportRepository;
            _projectDetailsRepository = projectDetailsRepository;
            _hostingEnvironment = hostingEnvironment;
            _dashboardService = dashboardService;
            _dashboardRepository = dashboardRepository;
            _attendanceService = attendanceService;
        }

        /// <summary>
        /// Logic to Get Employees Work Anniversary
        /// </summary> 
        public async Task<string> EmployeesWorkAnniversary()
        {
            try
            {
                var companies = await _companyRepository.GetAllCompany();
                foreach (var company in companies)
                {
                    var employeeWorkAnniversaries = await _employeesRepository.EmployeesWorkAnniversary(company.CompanyId);

                    foreach (var anv in employeeWorkAnniversaries)
                    {
                        try
                        {
                            Common.Common.WriteServerErrorLog("Service Employees Work Anniversary  " + anv.EmpId);

                            var dateOfJoing = Convert.ToDateTime(anv.DateOfJoining).ToString("dd-MMM");
                            DateTime date = Convert.ToDateTime(anv.DateOfJoining);
                            var gender = anv.Gender;
                            DateTime previousBirthDate = date.AddDays(-1);

                            Common.Common.WriteServerErrorLog("Service Employees Work Anniversary Date: " + date);

                            var draftTypeId = (int)EmailDraftType.WorkAnniversary;
                            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);

                            if (DateTime.Now.Day == previousBirthDate.Day && DateTime.Now.Month == previousBirthDate.Month)
                            {
                                var empNames = string.Empty;
                                var year = DateTime.Now.Year - date.Year;

                                var body = EmailBodyContent.SendEmail_Body_WorkAnniversary(anv.FirstName, anv.LastName, anv.UserName, anv.DesignationName, anv.DepartmentName, dateOfJoing, gender, year, emailDraftContentEntity.DraftBody);

                                foreach (var repEmpId in anv.ReportingPersonEmplyeeIds)
                                {
                                    var emp = anv.Employees.FirstOrDefault(d => d.EmpId == repEmpId);
                                    if (emp != null)
                                    {
                                        empNames += emp.OfficeEmail + ",";
                                    }
                                }

                                var mail = new List<string>();
                                var ccMails = emailDraftContentEntity.Email.Split(",");
                                foreach (var repEmail in ccMails)
                                {
                                    if (repEmail != null)
                                    {
                                        empNames += repEmail + ",";
                                    }
                                }

                                empNames.Trim(new char[] { ',' });
                                var emailEntity = new EmailQueueEntity();
                                emailEntity.FromEmail = anv.FromEmail;
                                emailEntity.ToEmail = anv.OfficeEmail;
                                emailEntity.Subject = emailDraftContentEntity.Subject;
                                emailEntity.Body = body;
                                emailEntity.IsSend = false;
                                emailEntity.Reason = Constant.WorkAnniversary;
                                emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                                emailEntity.CompanyId = company.CompanyId;
                                emailEntity.CCEmail = empNames;
                                emailEntity.CreatedDate = DateTime.Now;
                                var result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
                            }

                        }
                        catch (Exception ex)
                        {
                            Common.Common.WriteServerErrorLog("Service Email Queue error " + ex.StackTrace.ToString() + ex.Message.ToString());
                        }
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Common.Common.WriteServerErrorLog("EmployeesWorkAnniversary Service Catch error " + ex.Message.ToString());
            }
            return "EmployeesWorkAnniversary Service Running Successfully.";
        }


        /// <summary>
        /// Logic to Get EmailQueue Scheduler
        /// </summary> 
        public async Task<string> EmailQueueScheduler()
        {
            try
            {
                var companies = await _companyRepository.GetAllCompany();
                foreach (var company in companies)
                {
                    var emailQueueSendMail = await _employeesRepository.EmailQueueSendMail(company.CompanyId);
                    if (emailQueueSendMail != null)
                    {
                        foreach (var item in emailQueueSendMail)
                        {
                            try
                            {
                                Common.Common.WriteServerErrorLog("Service Email Queue Sent Mail Task  1 " + item.EmailQueueID);
                                var fromEmail = item.EmailQueueFromEmail;
                                var password = Common.Common.Decrypt(item.Password);
                                var userName = item.UserName;
                                var host = item.Host;
                                var port = item.EmailPort;
                                var fromAddress = new MailAddress(fromEmail, item.EmailQueueDisplayName);

                                var smtp = new SmtpClient
                                {
                                    Host = host,
                                    Port = port,
                                    EnableSsl = true,
                                    DeliveryMethod = SmtpDeliveryMethod.Network,
                                    UseDefaultCredentials = false,
                                    Credentials = new NetworkCredential(userName, password)
                                };
                                var mailMessage = new MailMessage
                                {
                                    From = fromAddress,
                                    Subject = item.Subject,
                                    Body = item.Body,
                                    IsBodyHtml = true
                                };
                                Common.Common.WriteServerErrorLog("Service Email Queue Sent Mail Task  1 " + item.ToEmail);
                                //Add Attachments, here i gave one folder name, and it will add all files in that folder as attachments, Or if you want only one file also can add
                                try
                                {
                                    if (item.Attachments != null && !string.IsNullOrEmpty(item.Attachments))
                                    {
                                        var frm = item.Attachments.Split(",");
                                        var strFmtSkillId = string.Empty; var finalOutSkill = string.Empty;
                                        for (int i = 0; i < frm.Count(); i++)
                                        {
                                            var b = frm[i];
                                            strFmtSkillId += string.Format(b + ",");
                                            if (!string.IsNullOrEmpty(strFmtSkillId))
                                            {
                                                finalOutSkill = strFmtSkillId.Remove(strFmtSkillId.Length - 1, 1);
                                            }
                                            string attachmentsPath = b;
                                            var file = new Attachment(attachmentsPath);
                                            mailMessage.Attachments.Add(file);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Common.Common.WriteServerErrorLog(ex.ToString());
                                }

                                Common.Common.WriteServerErrorLog("EmailQueueSheduler Service Task  Attachement " + item.EmailQueueID);
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
                                Common.Common.WriteServerErrorLog("EmailQueueSheduler Service Task  to email " + item.EmailQueueID);
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
                                Common.Common.WriteServerErrorLog("EmailQueueSheduler Service Task  to cc email " + item.EmailQueueID);
                                smtp.Send(mailMessage);

                                Common.Common.WriteServerErrorLog("EmailQueueSheduler Service Task mail sent " + item.EmailQueueID);

                                item.IsSend = true;
                                var result = await _companyRepository.UpdateEmailQueue(item.EmailQueueID, true);

                                Common.Common.WriteServerErrorLog("EmailQueueSheduler Service Task IsSend " + item.EmailQueueID);
                            }
                            catch (Exception ex)
                            {
                                Common.Common.WriteServerErrorLog("EmailQueueSheduler Service error " + ex.StackTrace.ToString() + ex.Message.ToString());
                            }
                        }
                    }

                }
                Common.Common.WriteServerErrorLog("2nd Mail Scheduler Output : " + DateTime.Now);
                return "Success";
            }
            catch (Exception ex)
            {
                Common.Common.WriteServerErrorLog("EmailQueueSheduler Service Email Queue error " + ex.StackTrace.ToString() + ex.Message.ToString());
            }
            return "EmailQueueSheduler Service Running Successfully.";
        }


        /// <summary>
        /// Logic to Get Employee Birthday
        /// </summary> 
        public async Task<string> EmployeeBirthday()
        {
            try
            {
                var companies = await _companyRepository.GetAllCompany();
                foreach (var company in companies)
                {
                    var employeeWorkAnniversaries = await _employeesRepository.EmployeeBirthdayCelebration(company.CompanyId);

                    foreach (var bc in employeeWorkAnniversaries)
                    {
                        try
                        {
                            Common.Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  1 " + bc.EmpId);

                            Common.Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  2 " + bc.EmpId);
                            var dateOfBirth = Convert.ToDateTime(bc.DateOfBirth).ToString("dd-MMM");
                            DateTime date = Convert.ToDateTime(bc.DateOfBirth);
                            var gender = bc.Gender;
                            DateTime previousBirthDate = date.AddDays(-4);

                            var draftTypeId = (int)EmailDraftType.RequestBirthday;
                            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);

                            if (DateTime.Now.Day == previousBirthDate.Day && DateTime.Now.Month == previousBirthDate.Month)
                            {
                                Common.Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  5 ");

                                var empNames = string.Empty;
                                foreach (var repEmpId in bc.ReportingPersonEmplyeeIds)
                                {
                                    var emp = bc.Employees.FirstOrDefault(d => d.EmpId == repEmpId);
                                    if (emp != null)
                                    {
                                        empNames += emp.OfficeEmail + ",";
                                    }
                                }

                                empNames.Trim(new char[] { ',' });

                                var body = EmailBodyContent.SendEmail_Body_RequestBithday(bc.FirstName, bc.LastName, bc.UserName, bc.DesignationName, bc.DepartmentName, dateOfBirth, gender, emailDraftContentEntity.DraftBody);
                                Common.Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  designation " + bc.DesignationName);
                                Common.Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  department " + bc.DepartmentName);

                                var toEmail = empNames;
                                var emailEntity = new EmailQueueEntity();
                                emailEntity.FromEmail = bc.FromEmail;
                                emailEntity.ToEmail = toEmail;
                                emailEntity.Subject = emailDraftContentEntity.Subject;
                                emailEntity.Body = body;
                                emailEntity.IsSend = false;
                                emailEntity.Reason = Constant.BirthdayCelebration;
                                emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                                emailEntity.CompanyId = company.CompanyId;
                                emailEntity.CCEmail = emailDraftContentEntity.Email;
                                emailEntity.CreatedDate = DateTime.Now;
                                var result = await _companyRepository.InsertEmailQueueEntity(emailEntity);

                                Common.Common.WriteServerErrorLog("Service Leave year equal 1 Birthday date  6 " + bc.EmpId);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Common.Common.WriteServerErrorLog("EmployeeBirthday Service Catch Msg : " + ex.Message.ToString());
            }
            return "Employees Birthday Celebration Service Running Successfully.";
        }


        /// <summary>
        /// Logic to Get Employees Probation
        /// </summary> 
        public async Task<string> EmployeesProbation()
        {
            try
            {
                var companies = await _companyRepository.GetAllCompany();
                foreach (var company in companies)
                {
                    var employeeProbationCompletions = await _employeesRepository.EmployeeProbationCelebration(company.CompanyId);

                    foreach (var pro in employeeProbationCompletions)
                    {
                        try
                        {
                            var probationPeriod = pro.ProbationMonths != null ? pro.ProbationMonths : 0;

                            Common.Common.WriteServerErrorLog("EmployeesProbation Service Leave year equal 1 probation item.EmpId : " + pro.EmpId);

                            var probationMonth = probationPeriod;
                            var dateOfJoing = Convert.ToDateTime(pro.DateOfJoining).ToString(Constant.DateFormatMonthHyphen);
                            var probationDate = Convert.ToDateTime(pro.DateOfJoining).AddMonths(probationMonth).Date;
                            var beforeProbationDate = Convert.ToDateTime(pro.DateOfJoining).AddMonths(probationMonth).Date.AddDays(-7);
                            var currentDate = DateTime.Now.ToString(Constant.DateFormatMonthHyphen);
                            Common.Common.WriteServerErrorLog("EmployeesProbation Service Leave year equal  probation 1 : " + pro.EmpId);

                            if (beforeProbationDate == DateTime.Now.Date)
                            {
                                var draftTypeId = (int)EmailDraftType.RequestProbation;
                                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);

                                Common.Common.WriteServerErrorLog("EmployeesProbation Service Leave year equal 1 probation date  1 ");

                                var empNames = string.Empty;
                                foreach (var repEmpId in pro.ReportingPersonEmplyeeIds)
                                {
                                    var emp = pro.Employees.FirstOrDefault(d => d.EmpId == repEmpId);
                                    if (emp != null)
                                    {
                                        empNames += emp.OfficeEmail + ",";
                                    }
                                }

                                empNames.Trim(new char[] { ',' });

                                var bodyContent = EmailBodyContent.SendEmail_Body_RequestProbation(pro.FirstName, pro.LastName, pro.UserName, pro.DesignationName, pro.DepartmentName, currentDate, dateOfJoing, emailDraftContentEntity.DraftBody);
                                var toEmail = empNames;
                                Common.Common.WriteServerErrorLog("EmployeesProbation Service Leave year equal 1 probation date  2 ");
                                var emailEntity = new EmailQueueEntity();
                                emailEntity.FromEmail = pro.FromEmail;
                                emailEntity.ToEmail = toEmail;
                                emailEntity.Subject = emailDraftContentEntity.Subject;
                                emailEntity.Body = bodyContent;
                                emailEntity.IsSend = false;
                                emailEntity.Reason = Constant.RequestforProbation;
                                emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                                emailEntity.CCEmail = emailDraftContentEntity.Email;
                                emailEntity.CompanyId = company.CompanyId;
                                emailEntity.CreatedDate = DateTime.Now;
                                var result = await _companyRepository.InsertEmailQueueEntity(emailEntity);

                                Common.Common.WriteServerErrorLog("EmployeesProbation Service Leave year equal 1 probation date  3 ");
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Common.WriteServerErrorLog("EmployeesProbation Service Catch Msg : " + ex.ToString());
                        }
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Common.Common.WriteServerErrorLog("Service Catch Msg : " + ex.ToString());
            }
            return "Employees Probation Celebration Service Running Successfully.";
        }


        /// <summary>
        /// Logic to Get Leave Calculation
        /// </summary> 
        public async Task<bool> GetLeaveCalculation()
        {
            var result = false;
            try
            {
                var getAllCompany = await _companyRepository.GetAllCompany();

                foreach (var company in getAllCompany)
                {
                    var getAllEmployees = await _employeesRepository.GetEmployeesByCompnayId(company.CompanyId);
                    foreach (var item in getAllEmployees)
                    {
                        try
                        {
                            await LeaveCalculation(item);
                        }
                        catch (Exception ex)
                        {
                            Common.Common.WriteServerErrorLog("Leave Service Scheduler Catch Error :  " + ex.ToString());
                        }
                    }
                }
                Common.Common.WriteServerErrorLog("1st Scheduler : Leave : " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Common.Common.WriteServerErrorLog("Leave Service Scheduler Catch Error :  " + ex.ToString());
            }
            finally { result = true; }

            async Task LeaveCalculation(EmployeesEntity item)
            {
                try
                {
                    var profileInfo = await _employeesRepository.GetProfileByEmpId(item.EmpId);
                    Common.Common.WriteServerErrorLog("Service Leave EmpId : " + item.EmpId);
                    if (profileInfo != null)
                    {
                        Common.Common.WriteServerErrorLog("Service Leave EmpId : " + profileInfo.EmpId);
                        var joinDate = Convert.ToDateTime(profileInfo.DateOfJoining);
                        Common.Common.WriteServerErrorLog("Service Leave joinDate : " + joinDate);
                        var currentDate = DateTime.Now;
                        Common.Common.WriteServerErrorLog("Service Leave EmpId 1 : " + item.EmpId);
                        var firstDateOfCurrentYear = Convert.ToDateTime("01/01/" + currentDate.Year);
                        //var lastDateOfCurrentYear = Convert.ToDateTime("31/12/" + currentDate.Year);
                        var lastDateOfCurrentYear = Convert.ToDateTime("12/31/" + currentDate.Year);
                        Common.Common.WriteServerErrorLog("Service Leave firstDateOfCurrentYear : " + firstDateOfCurrentYear);
                        Common.Common.WriteServerErrorLog("Service Leave lastDateOfCurrentYear : " + lastDateOfCurrentYear);
                        // Casual Leave and Sick Leave
                        int casualLeave = 0;
                        int sickLeave = 0;
                        var probationDate = Convert.ToDateTime(item.ProbationDate);
                        Common.Common.WriteServerErrorLog("Service Leave probationDate : " + probationDate);
                        if (probationDate == DateTime.MinValue)
                        {

                            var leaveDetailsByEmpId = await _leaveRepository.GetLeaveDetailsByEmpIdAndCompanyid(item.EmpId, item.CompanyId);
                            Common.Common.WriteServerErrorLog("Service Leave Get Leave details For : " + item.EmpId);
                            if (leaveDetailsByEmpId == null)
                            {
                                Common.Common.WriteServerErrorLog("Service Leave Details for Probation Not Completed  : " + item.EmpId);
                                var allLeaveDetailsEntity = new AllLeaveDetailsEntity();
                                allLeaveDetailsEntity.LeaveYear = DateTime.Now.Year;
                                allLeaveDetailsEntity.CasualLeaveCount = 0;
                                allLeaveDetailsEntity.SickLeaveCount = 0;
                                allLeaveDetailsEntity.EarnedLeaveCount = 0;
                                allLeaveDetailsEntity.CompensatoryOffCount = 0;
                                allLeaveDetailsEntity.EmpId = item.EmpId;
                                allLeaveDetailsEntity.CompanyId = item.CompanyId;
                                result = await _leaveRepository.InsertAllLeaveDetailsByEmpId(allLeaveDetailsEntity);
                                Common.Common.WriteServerErrorLog("Service Insert Leave Details : " + result);
                            }
                        }
                        else
                        {
                            CasualAndSickLeaveCalculation(currentDate, ref firstDateOfCurrentYear, lastDateOfCurrentYear, out casualLeave, out sickLeave, probationDate);

                            // Earned Leave
                            int earnedLeave = await EarnedLeaveCalculation(joinDate, currentDate);

                            var leaveDetailsByEmpId = await _leaveRepository.GetLeaveDetailsByEmpIdAndCompanyid(item.EmpId, item.CompanyId);
                            Common.Common.WriteServerErrorLog("Service Leave Get Leave details For Others : " + item.EmpId);
                            if (leaveDetailsByEmpId == null)
                            {
                                Common.Common.WriteServerErrorLog("Service Leave Details for Others : " + item.EmpId);
                                var allLeaveDetailsEntity = new AllLeaveDetailsEntity();
                                allLeaveDetailsEntity.LeaveYear = DateTime.Now.Year;
                                allLeaveDetailsEntity.CasualLeaveCount = casualLeave;
                                allLeaveDetailsEntity.SickLeaveCount = sickLeave;
                                allLeaveDetailsEntity.EarnedLeaveCount = earnedLeave;
                                allLeaveDetailsEntity.EmpId = item.EmpId;
                                allLeaveDetailsEntity.CompanyId = item.CompanyId;
                                Common.Common.WriteServerErrorLog("Service Leave Details for Others Casual Leave : " + casualLeave);
                                Common.Common.WriteServerErrorLog("Service Leave Details for Others  Sick Leave: " + sickLeave);
                                Common.Common.WriteServerErrorLog("Service Leave Details for Others Earned Leave : " + earnedLeave);
                                result = await _leaveRepository.InsertAllLeaveDetailsByEmpId(allLeaveDetailsEntity);
                                Common.Common.WriteServerErrorLog("Service Insert Leave Details For Others : " + result);
                            }
                            else
                            {

                                leaveDetailsByEmpId.CasualLeaveCount = casualLeave;
                                leaveDetailsByEmpId.SickLeaveCount = sickLeave;
                                leaveDetailsByEmpId.EarnedLeaveCount = earnedLeave;
                                result = await _leaveRepository.InsertAllLeaveDetailsByEmpId(leaveDetailsByEmpId);
                                Common.Common.WriteServerErrorLog("Service Insert Leave Details : " + result);
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    Common.Common.WriteServerErrorLog("Service Catch Msg : " + ex.ToString());
                }

            }
            return result;
        }

        /// <summary>
        /// Logic to Get Earned Leave Calculation
        /// </summary> 
        /// <param name="joinDate" ></param>
        /// <param name="currentDate" ></param>
        public static async Task<int> EarnedLeaveCalculation(DateTime joinDate, DateTime currentDate)
        {
            var earnedLeave = 0;
            var day = GetDaysInYear(joinDate);
            Common.Common.WriteServerErrorLog("Service Leave year equal 1 day : " + day);
            var isEarnedLeave = MonthDifference(currentDate, joinDate) > 12 ? true : false;
            Common.Common.WriteServerErrorLog("Service Leave year equal 1 isEarnedLeave : " + isEarnedLeave);
            if (isEarnedLeave)
            {
                Common.Common.WriteServerErrorLog("Service Leave year equal 1 isEarnedLeave : " + isEarnedLeave);
                var earnedDate = joinDate.AddYears(1);
                var earnedDay = earnedDate.Day;
                if (earnedDay <= 10)
                {
                    earnedDate = Convert.ToDateTime(earnedDate.Month + "/" + "01/" + earnedDate.Year);
                    //earnedDate = Convert.ToDateTime("01/" + earnedDate.Month + "/" + earnedDate.Year);
                }
                else
                {
                    var earnedDateAddOneMonth = earnedDate.AddMonths(1);
                    earnedDate = Convert.ToDateTime(earnedDateAddOneMonth.Month + "/" + "01/" + earnedDateAddOneMonth.Year);
                    //  earnedDate = Convert.ToDateTime("01/" + earnedDateAddOneMonth.Month + "/" + earnedDateAddOneMonth.Year);
                }
                Common.Common.WriteServerErrorLog("Service Leave year equal 1 earnedDate : " + earnedDate + " month : " + earnedDate.Month);
                var diffEarnedDate = MonthDifference(currentDate, earnedDate);
                if (diffEarnedDate > 0)
                {
                    earnedLeave = diffEarnedDate;
                }
                Common.Common.WriteServerErrorLog("Service Leave year equal 1 earnedLeave : " + earnedLeave);
            }
            else
            {
                earnedLeave = 0;
            }

            return earnedLeave;
        }


        /// <summary>
        /// Logic to Get Days In Year
        /// </summary> 
        /// <param name="date" ></param>
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

        /// <summary>
        /// Logic to Get Month Difference
        /// </summary> 
        /// <param name="lValue" ></param>
        /// <param name="rValue" ></param>
        static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        /// <summary>
        /// Logic to Get Casual And Sick Leave Calculation
        /// </summary> 
        /// <param name="currentDate" ></param>
        /// <param name="firstDateOfCurrentYear" ></param>
        /// <param name="lastDateOfCurrentYear" ></param>
        /// <param name="casualLeave" ></param>
        /// <param name="sickLeave" ></param>
        /// <param name="probationDate" ></param>
        public static void CasualAndSickLeaveCalculation(DateTime currentDate, ref DateTime firstDateOfCurrentYear, DateTime lastDateOfCurrentYear, out int casualLeave, out int sickLeave, DateTime probationDate)
        {
            var monthCountAfterProbation = 0.0m;

            if (probationDate.Year == currentDate.Year)
            {
                Common.Common.WriteServerErrorLog("Service Leave year equal ");
                var date = probationDate.Day;
                if (date <= 10)
                {
                    firstDateOfCurrentYear = Convert.ToDateTime(probationDate.Month + "/" + "01/" + currentDate.Year);
                    // firstDateOfCurrentYear = Convert.ToDateTime("01/" + probationDate.Month + "/" + currentDate.Year);
                }
                else
                {
                    var probationDateAddOneMonth = probationDate.AddMonths(1);
                    firstDateOfCurrentYear = Convert.ToDateTime(probationDateAddOneMonth.Month + "/" + "01/" + probationDateAddOneMonth.Year);
                    //firstDateOfCurrentYear = Convert.ToDateTime("01/" + probationDateAddOneMonth.Month + "/" + probationDateAddOneMonth.Year);
                }
                Common.Common.WriteServerErrorLog("Service Leave year equal 1 " + firstDateOfCurrentYear + " month : " + firstDateOfCurrentYear.Month);
                for (int i = firstDateOfCurrentYear.Month; i <= lastDateOfCurrentYear.Month; i++)
                {
                    monthCountAfterProbation += 1;
                }

                if (monthCountAfterProbation == 1 || monthCountAfterProbation == 3 || monthCountAfterProbation == 5 || monthCountAfterProbation == 7 || monthCountAfterProbation == 9 || monthCountAfterProbation == 11)
                {
                    casualLeave = Convert.ToInt16((monthCountAfterProbation / 2) + 0.5m);
                    sickLeave = Convert.ToInt16((monthCountAfterProbation / 2) - 0.5m);
                }
                else
                {
                    casualLeave = Convert.ToInt16(monthCountAfterProbation / 2);
                    sickLeave = Convert.ToInt16(monthCountAfterProbation / 2);
                }
                Common.Common.WriteServerErrorLog("Service Leave year equal 2 ");
            }
            else
            {
                casualLeave = 6;
                sickLeave = 6;
            }

        }

        /// <summary>
        /// Logic to Get Attendance Log
        /// </summary> 
        public async Task<bool> AttendanceLog()
        {
            var result = false;
            try
            {
                Common.Common.WriteServerErrorLog("Service Attendance Log Entry Task");
                var getAttendanceDataFromEntities = await _attendanceRepository.GetWorkingHourList();
                var lastCount = 0;
                Common.Common.WriteServerErrorLog("Service Attendance Log Entry Task 1 ");
                if (getAttendanceDataFromEntities != null)
                {
                    lastCount = getAttendanceDataFromEntities.Id;
                }
                Common.Common.WriteServerErrorLog("Service Attendance Log Entry Task 2 ");
                var getAttendanceDataFromAttDbEntities = await _attendanceRepository.GetWorkingHourCount(lastCount);
                var attendanceEntities = new List<AttendanceEntitys>();
                foreach (var item in getAttendanceDataFromAttDbEntities)
                {
                    try
                    {
                        var attendanceEntity = new AttendanceEntitys();
                        // attendanceEntity.Id = item.Id;
                        attendanceEntity.EmployeeCode = item.EmployeeCode;
                        attendanceEntity.LogDateTime = item.LogDateTime;
                        attendanceEntity.LogDate = item.LogDate;
                        attendanceEntity.LogTime = item.LogTime;
                        attendanceEntity.Direction = item.Direction;
                        attendanceEntities.Add(attendanceEntity);

                        Common.Common.WriteServerErrorLog("Service Attendance Log Entry Task 4 " + item.Id);
                    }
                    catch (Exception ex)
                    {
                        Common.Common.WriteServerErrorLog("Service Email Queue error " + ex.StackTrace.ToString());
                    }
                }

                if (attendanceEntities.Count() > 0)
                {
                    result = await _attendanceRepository.InsertAttendanceEntitys(attendanceEntities);
                }
            }
            catch (Exception ex) { Common.Common.WriteServerErrorLog("Service Catch Msg : " + ex.ToString()); }

            return result;
        }

        /// <summary>
        /// Logic to Get Email Scheduler
        /// </summary> 
        /// <param name="companyId" ></param>
        public async Task<bool> EmailScheduler(int companyId)
        {
            try
            {
            
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler start");
                var MailScheduler = await _employeesRepository.GetMailSchedulerEntity(companyId);
                foreach (var mailSchedule in MailScheduler)
                {
                    if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyAttendance || mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance || mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyAttendance || mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyAttendance)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler start");
                        await AttendanceMailScheduler(mailSchedule);
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler complete");
                    }
                    else if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyTimeSheet || mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyTimeSheet ||
                        mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyTimeSheet || mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyTimeSheet)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - TimeSheetMailScheduler start");
                        await TimeSheetMailScheduler(mailSchedule);
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - TimeSheetMailScheduler complete");
                    }
                    else if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyLeave || mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyLeave || mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyLeave || mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyLeave)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler start - LeaveMailSchedule start");
                        await LeaveMailSchedule(mailSchedule, companyId);
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler start - LeaveMailSchedule complete");
                    }
                }

            }
            catch (Exception e)
            {

            }
            return true;
        }

        /// <summary>
        /// Logic to Get Attendance Mail Scheduler
        /// </summary> 
        /// <param name="mailSchedule" ></param>
        public async Task AttendanceMailScheduler(MailSchedulerEntity mailSchedule)
        {
            Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler start 01");
            var attendanceListViewModels = new AttendaceListViewModel();
            var sendMail = false;
            var isManagement = true;
            var combinedPath = new List<string>();
            List<DateTime> allDates = new List<DateTime>();

            var activeEmployee = new List<EmployeesEntity>();
            if (mailSchedule != null)
            {
                if (mailSchedule != null && mailSchedule.DurationId == (int)Duration.Once)
                {
                    if (mailSchedule.EmailDraftId == (int)EmailDraftType.DailyAttendance)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler start Daily");
                        var day = mailSchedule.MailTime.DayOfWeek;
                        if (day == DayOfWeek.Monday)
                        {
                            attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-3).ToString(Constant.DateFormat);
                            attendanceListViewModels.EndDate = attendanceListViewModels.StartDate;
                        }
                        else
                        {
                            attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-1).ToString(Constant.DateFormat);
                            attendanceListViewModels.EndDate = attendanceListViewModels.StartDate;
                        }
                    }
                    else if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler start weekely");
                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                        attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                    }
                    else if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance && isManagement)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler start weekely management");
                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                        attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                    }
                    else if (mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyAttendance)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler start monthly");
                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddMonths(-1).ToString(Constant.DateFormat);
                        attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                    }
                    else if (mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyAttendance)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - AttendanceMailScheduler start yearly");
                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddYears(-1).ToString(Constant.DateFormat);
                        attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                    }
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.StartDate" + attendanceListViewModels.StartDate);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.EndDate" + attendanceListViewModels.EndDate);
                }
                else if (mailSchedule?.DurationId == (int)Common.Enums.Duration.Daily)
                {
                    var day = mailSchedule.MailTime.DayOfWeek;
                    if (day == DayOfWeek.Monday)
                    {
                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-3).ToString(Constant.DateFormat);
                        attendanceListViewModels.EndDate = attendanceListViewModels.StartDate;
                    }
                    else
                    {
                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddDays(-1).ToString(Constant.DateFormat);
                        attendanceListViewModels.EndDate = attendanceListViewModels.StartDate;
                    }
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.StartDate d " + attendanceListViewModels.StartDate);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.EndDate d " + attendanceListViewModels.EndDate);

                }
                else if (mailSchedule?.DurationId == (int)Duration.Monthly)
                {
                    attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddMonths(-1).ToString(Constant.DateFormat);
                    attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.StartDate m " + attendanceListViewModels.StartDate);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.EndDate m " + attendanceListViewModels.EndDate);

                }
                else if (mailSchedule?.DurationId == (int)Duration.Yearly)
                {
                    attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddYears(-1).ToString(Constant.DateFormat);
                    attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.StartDate y " + attendanceListViewModels.StartDate);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - attendanceListViewModels.EndDate y " + attendanceListViewModels.EndDate);

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
                                if (mailSchedule.MailTime.Date >= DateTime.Now.Date && listDays[i] == Convert.ToString(DateTime.Now.DayOfWeek) && mailSchedule.WhomToSend != "1000000000")
                                {
                                    if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                                    {
                                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                                        attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.AddDays(-1).ToString(Constant.DateFormat);
                                    }
                                }
                                else if (mailSchedule.MailTime.Date <= DateTime.Now.Date && mailSchedule.MailTime.Date >= DateTime.Now.Date && listDays[i] == Convert.ToString(DateTime.Now.DayOfWeek) && mailSchedule.WhomToSend == "1000000000")
                                {
                                    if (mailSchedule.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                                    {
                                        attendanceListViewModels.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.AddDays(-7).ToString(Constant.DateFormat);
                                        attendanceListViewModels.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(DateTime.Now.ToString(Constant.DateFormat)).Date.AddDays(-1).ToString(Constant.DateFormat);
                                    }
                                }
                            }
                        }
                    }
                }
                if (attendanceListViewModels.StartDate != null && attendanceListViewModels.EndDate != null)
                {
                    var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(attendanceListViewModels.StartDate);
                    var filterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(attendanceListViewModels.EndDate);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - filterDate " + filterDate);
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - filterEndDate " + filterEndDate);

                    if (filterDate != DateTime.MinValue)
                    {
                        for (DateTime date = filterDate; date <= filterEndDate; date = date.AddDays(1))
                            allDates.Add(date);
                    }

                    if (allDates.Count > 0)
                    {
                        var getAllEmployee = await _employeesRepository.GetAllEmployeeByCompany(mailSchedule.CompanyId);
                        List<int> empIds = mailSchedule.WhomToSend.Split(',').Select(int.Parse).ToList();
                        if (empIds[0] != 0 && empIds.Count() > 0 && !empIds.Contains(1000000000))
                        {
                            foreach (int empId in empIds)
                            {
                                var getEmployee = getAllEmployee.Where(x => x.EmpId == empId).FirstOrDefault();
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
                            activeEmployee = getAllEmployee;
                        }

                        if (activeEmployee.Count() > 0)
                        {
                            Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - activeEmployee count > 0 ");

                            if (mailSchedule.WhomToSend == Constant.ZeroStr || empIds.Count() > 0 && !empIds.Contains(1000000000))
                            {
                                Common.Common.WriteServerErrorLog(" Service Employees Weekly attendance EmailScheduler - activeEmployee count > 0 001");
                                sendMail = await SendEmployeeAttendanceForAllEmployee(attendanceListViewModels, empIds, combinedPath, mailSchedule, activeEmployee, allDates, mailSchedule.CompanyId);
                            }
                            else if (mailSchedule.WhomToSend == "1000000000")
                            {
                                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - activeEmployee count > 0 002");
                                sendMail = await SendEmployeeAttendanceForManagement(attendanceListViewModels, empIds, combinedPath, mailSchedule, activeEmployee, allDates,mailSchedule.CompanyId);
                            }
                            else
                            {
                                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - activeEmployee count > 0 003");
                                sendMail = await SendMailForAttendance(attendanceListViewModels, mailSchedule, combinedPath,mailSchedule.CompanyId);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Logic to Get List Of Days
        /// </summary> 
        /// <param name="strMailDays" ></param>
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

        /// <summary>
        /// Logic to Get Send Employee Attendance For All Employee
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        /// <param name="empIds" ></param>
        /// <param name="fileId" ></param>
        /// <param name="mailSchedulerEntity" ></param>
        /// <param name="allDates" ></param>
        /// <param name="companyId" ></param>
        public async Task<bool> SendEmployeeAttendanceForAllEmployee(AttendaceListViewModel attendaceListViewModel, List<int> empIds, List<string> fileId, MailSchedulerEntity mailSchedulerEntity, List<EmployeesEntity> activeEmployee, List<DateTime> allDates,int companyId)
        {
            string time = string.Empty;
            var result = false;
            var combinedPath = new List<string>();
            var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate);
            var attendanceListViewModels = new AttendaceListViewModels();
            attendanceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            var attendanceList = await GetAllAttendancereport(attendaceListViewModel);
            var getAllTimeSheetList = await _timeSheetRepository.GetAllTimeSheetByCurrentDateAndCompanyId(filterDate, mailSchedulerEntity.CompanyId);
            var employees = await _employeesRepository.GetAllEmployeeByCompany(mailSchedulerEntity.CompanyId);
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

                    var attendanceViewModelList = await GetAllEmployeesAttendanceFilter(allDates, listEmployees, attendanceList, getAllTimeSheetList,companyId);

                    listEmployees = null;
                    var attendanceListDataModelEmployee = new AttendanceListDataModel();
                    var attendacelistViewModel = new AttendaceListViewModel();

                    if (attendanceViewModelList.AttendaceListViewModel.Count() > 0)
                    {

                        if (getEmployeeDetails?.EsslId !=null && getEmployeeDetails != null)
                        {
                            if (mailSchedulerEntity.DurationId == (int)Common.Enums.Duration.Daily || mailSchedulerEntity.EmailDraftId == (int)MailSchedulerDraft.DailyAttendance)
                            {
                                var stringToNum = Convert.ToString(getEmployeeDetails.EsslId);
                                var tableEmployeeId = stringToNum != null ? Int32.Parse(stringToNum) : 0;
                                var firstInTime = attendanceList.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date && x.Direction == Constant.EntryTypeIn);
                                var lastInTime = attendanceList.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date && x.Direction == Constant.EntryTypeOut);
                                var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                                var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";
                                toEmail = getEmployeeDetails.OfficeEmail;
                                if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                                {
                                    DateTime StartTime = Convert.ToDateTime(firstInTime?.LogDateTime);
                                    DateTime EndTime = Convert.ToDateTime(lastInTime?.LogDateTime);
                                    string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                                    dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                                    var breakHours = GetBreakHour(stringToNum, attendanceList, filterDate);
                                    var burningHours = GetTimeSheetWorkHours(getEmployeeDetails.EmpId, getAllTimeSheetList);
                                    var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                                    insideoffice = TimeSpan.Parse(insideoffice).TotalSeconds > 0 ? insideoffice : Constant.TimeFormatZero;
                                    attendacelistViewModel.EmployeeId = getEmployeeDetails.EmpId;
                                    attendacelistViewModel.UserName = getEmployeeDetails.UserName;
                                    attendacelistViewModel.EmployeeName = getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName;
                                    attendacelistViewModel.Date = filterDate.Date.ToString(Constant.DateFormat);
                                    attendacelistViewModel.TotalHours = dt;
                                    attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
                                    attendacelistViewModel.InsideOffice = insideoffice;
                                    attendacelistViewModel.BurningHours = burningHours;
                                    attendacelistViewModel.EntryTime = Convert.ToDateTime(firstLoginDate).ToString(Constant.TimeFormatWithFullForm);
                                    attendacelistViewModel.ExitTime = Convert.ToDateTime(lastLoginDate).ToString(Constant.TimeFormatWithFullForm);
                                    attendacelistViewModel.OfficeEmail = getEmployeeDetails.OfficeEmail;
                                    attendanceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
                                    if (!string.IsNullOrEmpty(attendacelistViewModel.InsideOffice))
                                    {
                                        time = attendacelistViewModel.InsideOffice;
                                        double seconds = TimeSpan.Parse(time).TotalSeconds;
                                        attendacelistViewModel.TotalSecounds = Convert.ToInt64(seconds);
                                    }
                                    attendanceListDataModelEmployee = new AttendanceListDataModel();
                                    attendanceListDataModelEmployee.EmployeeName = attendacelistViewModel.EmployeeName;
                                    attendanceListDataModelEmployee.UserName = attendacelistViewModel.UserName;
                                    attendanceListDataModelEmployee.Date = attendacelistViewModel.Date;
                                    attendanceListDataModelEmployee.TotalHours = attendacelistViewModel.TotalHours;
                                    attendanceListDataModelEmployee.BreakHours = attendacelistViewModel.BreakHours;
                                    attendanceListDataModelEmployee.InsideOffice = time;
                                    attendanceListDataModelEmployee.EntryTime = attendacelistViewModel.EntryTime;
                                    attendanceListDataModelEmployee.ExitTime = attendacelistViewModel.ExitTime;
                                    attendanceListDataModelEmployee.BurningHours = attendacelistViewModel.BurningHours;
                                    attendanceListDataModelEmployee.TotalSecounds = attendacelistViewModel.TotalSecounds;
                                }
                                
                            }
                            else
                            {
                                if (getEmployeeDetails?.EsslId !=null && getEmployeeDetails != null)
                                {
                                    var firstInTime = attendanceList.FirstOrDefault(x => Convert.ToInt32(x.EmployeeId) == getEmployeeDetails.EmpId && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date && x.Direction == Constant.EntryTypeIn);
                                    var timeLogEntitys = await _dashboardRepository.GetTimeLogEntityByEmpId(Convert.ToInt32(getEmployeeDetails.EmpId),Convert.ToDateTime(firstInTime.LogDate),Convert.ToInt32(companyId));
                                    var tFrom = timeLogEntitys.Select(x => Convert.ToString(x.ClockInTime)).FirstOrDefault();
                                    var tTo = timeLogEntitys.Where(x => x.ClockOutTime.HasValue && x.EntryStatus == Constant.EntryTypeOut).Select(x => x.ClockOutTime.Value.ToString()).LastOrDefault();
                                    double breaks = 0;
                                    var attendanceReportDatamodelList = await _dashboardService.GetTimeLog(Convert.ToInt32(getEmployeeDetails.EmpId), Convert.ToDateTime(tFrom),Convert.ToInt32(companyId));
                                    var breakHours = GetBreakHourForTimeLogger(Convert.ToString(Convert.ToInt32(getEmployeeDetails.EmpId)), timeLogEntitys, Convert.ToDateTime(tFrom));
                                    var BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM) + " " + Constant.Hrs;
                                    attendacelistViewModel.EmployeeId = getEmployeeDetails.EmpId;
                                    attendacelistViewModel.UserName = getEmployeeDetails.UserName;
                                    attendacelistViewModel.EmployeeName = getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName;
                                    attendacelistViewModel.Date = filterDate.Date.ToString(Constant.DateFormat);
                                    attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
                                    attendacelistViewModel.OfficeEmail = getEmployeeDetails.OfficeEmail;
                                    attendanceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);


                                    var attendanceReportDatamodel = await _dashboardService.GetTimeLog(getEmployeeDetails.EmpId, Convert.ToDateTime(tFrom),Convert.ToInt32(companyId));
                                    attendanceListDataModelEmployee = new AttendanceListDataModel();
                                    attendanceListDataModelEmployee.EmployeeName = getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName;
                                    attendanceListDataModelEmployee.UserName = getEmployeeDetails.UserName;
                                    attendanceListDataModelEmployee.Date = filterDate.Date.ToString(Constant.DateFormat);
                                    attendanceListDataModelEmployee.TotalHours = attendanceReportDatamodelList.TotalHours;
                                    attendanceListDataModelEmployee.InsideOffice = attendanceReportDatamodelList.InsideOffice;
                                    attendanceListDataModelEmployee.BreakHours = attendanceReportDatamodelList.BreakHours;
                                    attendanceListDataModelEmployee.EntryTime = timeLogEntitys.Select(x => Convert.ToString(x.ClockInTime)).FirstOrDefault();
                                    attendanceListDataModelEmployee.ExitTime = timeLogEntitys.Select(x => Convert.ToString(x.ClockOutTime)).FirstOrDefault();

                                }
                            }
                        }
                    }

                    toEmail = string.IsNullOrEmpty(getEmployee?.OfficeEmail) ? "" : getEmployee.OfficeEmail;
                    employeeName = getEmployeeDetails != null ? getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName : "";

                    if (attendanceViewModelList.AttendaceListViewModel.Count() >= 0 && mailSchedulerEntity.FileFormat == (int)Common.Enums.FileFormats.Excel)
                    {
                        var excel = await GenerateExcel(attendanceViewModelList.AttendaceListViewModel);
                        combinedPath.Add(excel);
                    }
                    else if (attendanceViewModelList.AttendaceListViewModel.Count() >= 0 && mailSchedulerEntity.FileFormat == (int)Common.Enums.FileFormats.Pdf)
                    {
                        var pdf = CreatePdfFiles(attendanceViewModelList, attendaceListViewModel.StartDate, attendaceListViewModel.EndDate);
                        combinedPath.Add(pdf);
                    }
                    else if (attendanceViewModelList.AttendaceListViewModel.Count() > 0 && mailSchedulerEntity.FileFormat == (int)Common.Enums.FileFormats.ExcelPDF)
                    {
                        var excel = await GenerateExcel(attendanceViewModelList.AttendaceListViewModel);
                        combinedPath.Add(excel);

                        var pdf = CreatePdfFiles(attendanceViewModelList, attendaceListViewModel.StartDate, attendaceListViewModel.EndDate);
                        combinedPath.Add(pdf);
                    }

                    if (attendanceListDataModelEmployee != null && mailSchedulerEntity.DurationId == (int)Duration.Daily || attendanceListDataModelEmployee != null && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.DailyAttendance)
                    {
                        var draftTypeId = (int)EmailDraftType.AttendanceLog;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);
                        if (emailDraftContentEntity != null && attendanceListDataModelEmployee.UserName != null)
                        {
                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceAll(attendanceListDataModelEmployee, emailDraftContentEntity.DraftBody);
                            result = await InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                        }
                    }
                    else if (combinedPath.Count() > 0 && mailSchedulerEntity.DurationId == (int)Duration.Custom || combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                    {
                        var draftTypeId = (int)EmailDraftType.WeeklyAttendance;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);
                        if (emailDraftContentEntity != null)
                        {
                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, employeeName, emailDraftContentEntity.DraftBody);
                            result = await InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                        }
                    }
                    else if (combinedPath.Count() > 0 && mailSchedulerEntity.DurationId == (int)Duration.Monthly || combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.MonthlyAttendance)
                    {
                        var draftTypeId = (int)EmailDraftType.MonthlyAttendance;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);
                        if (emailDraftContentEntity != null)
                        {
                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, employeeName, emailDraftContentEntity.DraftBody);
                            result = await InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                        }
                    }
                    else if (combinedPath.Count() > 0 && mailSchedulerEntity.DurationId == (int)Duration.Yearly || combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.YearlyAttendance)
                    {
                        var draftTypeId = (int)EmailDraftType.YearlyAttendance;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);
                        if (emailDraftContentEntity != null)
                        {
                            var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, employeeName, emailDraftContentEntity.DraftBody);
                            result = await InsertEmailAttendance(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                        }
                    }
                    combinedPath = new List<string>(0);


                    if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Daily)
                    {
                        var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                        DateTime mailTime = Convert.ToDateTime(mailScheduler?.MailTime.ToString(Constant.TimeFormat));
                        var mailDay = mailTime.DayOfWeek.ToString();
                        if (mailDay == Constant.Saturday)
                        {
                            var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(3).ToString(Constant.DateFormat));
                            var sendMailDaily = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                            mailScheduler.MailTime = sendMailDaily;
                        }
                        else
                        {
                            var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(1).ToString(Constant.DateFormat));
                            var sendMailDaily = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                            mailScheduler.MailTime = sendMailDaily;
                        }
                        await _companyRepository.CreateMailScheduler(mailScheduler);
                    }
                    else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Custom)
                    {
                        var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                        DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                        var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(7).ToString(Constant.DateFormat));
                        var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                        mailScheduler.MailTime = sendMail;
                        await _companyRepository.CreateMailScheduler(mailScheduler);
                    }
                    else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Monthly)
                    {
                        var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                        DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                        var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddMonths(1).ToString(Constant.DateFormat));
                        var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                        mailScheduler.MailTime = sendMail;
                        await _companyRepository.CreateMailScheduler(mailScheduler);
                    }
                    else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Yearly)
                    {
                        var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                        DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                        var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddYears(1).ToString(Constant.DateFormat));
                        var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                        mailScheduler.MailTime = sendMail;
                        await _companyRepository.CreateMailScheduler(mailScheduler);
                    }
                    else if (mailSchedulerEntity.DurationId == (int)Duration.Once && result == true)
                    {
                        var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                        mailScheduler.IsActive = false;
                        await _companyRepository.CreateMailScheduler(mailScheduler);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to Get Break Hour For TimeLogger
        /// </summary> 
        /// <param name="timeLoggerEntities" ></param>
        /// <param name="filterDate" ></param>
        public string GetBreakHourForTimeLogger(string empId, List<TimeLoggerEntity> timeLoggerEntities, DateTime filterDate)
        {
            var timeLoggerEty = timeLoggerEntities
                .Where(x => x.EmployeeId.ToString() == empId &&
                            (x.ClockInTime.Date == filterDate.Date ||
                             (x.ClockOutTime.HasValue && x.ClockOutTime.Value.Date == filterDate.Date)))
                .OrderBy(x => x.ClockInTime)
                .ToList();

            double totalBreakSeconds = 0;

            Console.WriteLine($"Filtered 'out' Entries for Employee {empId} on {filterDate.ToShortDateString()}");

            foreach (var record in timeLoggerEty.Where(x => x.EntryStatus?.ToLower() == Constant.EntryTypeOut))
            {
                if (record.ClockOutTime.HasValue)
                {
                    double breakDuration = (record.ClockOutTime.Value - record.ClockInTime).TotalSeconds;

                    if (breakDuration > 0)
                    {
                        totalBreakSeconds += breakDuration;
                        Console.WriteLine($"Break Duration Added: {breakDuration} sec | Total: {totalBreakSeconds} sec");
                    }
                }
            }

            totalBreakSeconds = Math.Max(totalBreakSeconds, 0);

            TimeSpan t = TimeSpan.FromSeconds(totalBreakSeconds);
            string result = $"{(t.Days * 24 + t.Hours):D2}:{t.Minutes:D2}:{t.Seconds:D2}";

            Console.WriteLine($"Final Total Break Time: {result}");

            return result;
        }

        /// <summary>
        /// Logic to Send Mail For Attendance
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        /// <param name="mailSchedulerEntity" ></param>
        /// <param name="fileId" ></param>
        /// <param name="companyId" ></param>
        public async Task<bool> SendMailForAttendance(AttendaceListViewModel attendaceListViewModel, MailSchedulerEntity mailSchedulerEntity, List<string> fileId, int companyId)
        {
            var result = false;
            var combinePath = new List<string>();
            if (attendaceListViewModel.EmployeeId == 0)
            {
                var draftTypeId = (int)EmailDraftType.AttendanceLogForManagement;
                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                MailMessage mailMessage = new MailMessage();
                var toEmail = emailDraftContentEntity.Email;
                var subject = emailDraftContentEntity.Subject;
                var mailBody = EmailBodyContent.SendEmail_Body_AttendanceForManagement(attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, emailDraftContentEntity.DraftBody);
                var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
                var emailEntity = new EmailQueueEntity();
                emailEntity.FromEmail = emailSettingsEntity.FromEmail;
                emailEntity.ToEmail = string.IsNullOrEmpty(toEmail) ? string.Empty : toEmail;
                emailEntity.Subject = subject;
                emailEntity.Body = mailBody;
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
                var filePath = combinePath;
                if (filePath.Count() > 0)
                {
                    var str = string.Join(",", filePath);
                    emailEntity.Attachments = str.TrimEnd(',');
                }
                emailEntity.IsSend = false;
                emailEntity.Reason = "EmployeesAttendanceDetailsReason";
                emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                emailEntity.CreatedDate = DateTime.Now;
                result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            }
            return result;
        }

        /// <summary>
        /// Logic to Send Employee Attendance For Management
        /// </summary> 
        /// <param name="attendaceListViewModel" ></param>
        /// <param name="empIds" ></param>
        /// <param name="fileId" ></param>
        /// <param name="mailSchedulerEntity" ></param>
        /// <param name="activeEmployee" ></param>
        /// <param name="allDates" ></param>
        /// <param name="companyId" ></param>
        public async Task<bool> SendEmployeeAttendanceForManagement(AttendaceListViewModel attendaceListViewModel, List<int> empIds, List<string> fileId, MailSchedulerEntity mailSchedulerEntity, List<EmployeesEntity> activeEmployee, List<DateTime> allDates,int companyId)
        {
            Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement ");
            var result = false;
            var combinedPath = new List<string>();
            var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate);
            var attendanceListViewModels = new AttendaceListViewModels();
            attendanceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
            var attendanceList = await GetAllAttendancereport(attendaceListViewModel);
            var employees = await _employeesRepository.GetAllEmployeeByCompany(mailSchedulerEntity.CompanyId);
            empIds = activeEmployee.Select(x => x.EmpId).ToList();
            var employeeName = "";
            var listEmployees = new List<EmployeesEntity>();
            if (empIds.Count() > 0)
            {
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - empIds count > 0 ");
                foreach (var emp in empIds)
                {


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
                }

                var adminEmails = listEmployees
                 .Where(x => x.RoleId == Convert.ToInt32(Role.Admin)) // only For Admins
                 .Select(x => new
                 {
                     OfficeEmail = x.OfficeEmail,
                     AdminName = $"{x.FirstName} {x.LastName}"
                 })
                 .ToList();
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 01");
                List<TimeSheetEntity> getAllTimeSheetList = new List<TimeSheetEntity>();
                var attendanceViewModelList = await GetAllEmployeesAttendanceFilter(allDates, listEmployees, attendanceList, getAllTimeSheetList,companyId);
                if (attendanceViewModelList.AttendaceListViewModel.Count() > 0)
                {
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 02");
                    if (attendanceViewModelList.AttendaceListViewModel.Count() > 0 && mailSchedulerEntity.FileFormat == (int)Common.Enums.FileFormats.Pdf)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03");
                        var pdf = CreatePdfFile(attendanceViewModelList, attendaceListViewModel.StartDate, attendaceListViewModel.EndDate, listEmployees);
                        combinedPath.Add(pdf);
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 04 pdf " + pdf);
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 04 combinedPath " + combinedPath);
                    }
                    if (combinedPath.Count() > 0 && mailSchedulerEntity.DurationId == (int)Duration.Custom || combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                    {
                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 05 ");
                        var draftTypeId = (int)EmailDraftType.WeeklyAttendance;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);

                        if (emailDraftContentEntity != null)
                        {
                            Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 06 ");
                            var results = new List<bool>();
                            Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 06 " + attendaceListViewModel.StartDate);
                            Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 06 " + attendaceListViewModel.EndDate);
                            foreach (var email in adminEmails)
                            {
                                var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(
                                    attendaceListViewModel.StartDate,
                                    attendaceListViewModel.EndDate,
                                    email.AdminName,
                                    emailDraftContentEntity.DraftBody
                                );
                                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 07 ");
                                // Process the email sequentially
                                result = await InsertEmailAttendance(email.OfficeEmail, emailDraftContentEntity, bodyContent, combinedPath);
                                results.Add(result);
                                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 08 ");
                            }
                        }
                    }
                    combinedPath = new List<string>(0);
                }
                if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Daily)
                {
                    var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                    DateTime mailTime = Convert.ToDateTime(mailScheduler?.MailTime.ToString(Constant.TimeFormat));
                    var mailDay = mailTime.DayOfWeek.ToString();
                    if (mailDay == Constant.Saturday)
                    {
                        var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(3).ToString(Constant.DateFormat));
                        var sendMailDaily = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                        mailScheduler.MailTime = sendMailDaily;
                    }
                    else
                    {
                        var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(1).ToString(Constant.DateFormat));
                        var sendMailDaily = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                        mailScheduler.MailTime = sendMailDaily;
                    }
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
                else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Custom)
                {
                    var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                    DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(7).ToString(Constant.DateFormat));
                    var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                    mailScheduler.MailTime = sendMail;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
                else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Monthly)
                {
                    var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                    DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddMonths(1).ToString(Constant.DateFormat));
                    var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                    mailScheduler.MailTime = sendMail;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
                else if (result == true && mailSchedulerEntity.DurationId == (int)Duration.Yearly)
                {
                    var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                    DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddYears(1).ToString(Constant.DateFormat));
                    var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                    mailScheduler.MailTime = sendMail;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
                else if (mailSchedulerEntity.DurationId == (int)Duration.Once && result == true)
                {
                    var mailScheduler = await _employeesRepository.GetMailSchedulerbyId(mailSchedulerEntity.SchedulerId);
                    mailScheduler.IsActive = false;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }

            }
            return result;
        }

        // Method to Get All Employees AttendanceListDataModel From Db

        public async Task<string> GenerateExcel(List<AttendaceListViewModel> attendaceListViewModels)
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
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/AttendanceDetails");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var filePath = Path.Combine(directoryPath, fileName);
                combinedPath = filePath;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    workbook.SaveAs(filePath);
                    memoryStream.Position = 0;
                    var content = memoryStream.ToArray();
                    //HttpContext.Session.Set(Constant.fileId, content);
                }
            }
            return combinedPath;
        }

        public string CreatePdfFile(AttendaceListViewModels attendaceListViewModels, string fromDate, string toDate, List<EmployeesEntity> listEmployees)
        {
            try
            {
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/AttendanceDetails");
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 directoryPath" + directoryPath);
                if (!Directory.Exists(directoryPath))
                {
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 directoryPath create");
                    Directory.CreateDirectory(directoryPath);
                }
                var currentTime = DateTime.Now;
                var currentDateTime = currentTime.ToString(Constant.DateFormatYMDHMS);
                var fileName = "Management_EmployeeAttendance" + Constant.Hyphen + currentDateTime + ".pdf";
                var filePath = Path.Combine(directoryPath, fileName);
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 filePath " + filePath);
                Document pdfDoc = new Document(iTextSharp.text.PageSize.A4, 40f, 40f, 40f, 40f); // Set margins (left, right, top, bottom)
                MemoryStream PDFData = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                BaseColor myColor = BaseColor.BLUE;
                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var titleFontBlue = FontFactory.GetFont("Arial", 14, BaseColor.BLUE);
                var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, myColor);
                BaseColor TableHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                Rectangle pageSize = writer.PageSize;
                // Open the Document for writing
                pdfDoc.Open();

                // Add elements to the document here

                // Create the header table 
                PdfPTable headerTable = new PdfPTable(3);
                headerTable.HorizontalAlignment = Element.ALIGN_CENTER;
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 200f, 5f, 350f });
                string webRootPath = _hostingEnvironment.WebRootPath;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                logo.ScaleToFit(100, 70);

                // Add logo to the header table
                PdfPCell pdfCellLogo = new PdfPCell(logo)
                {
                    Border = Rectangle.NO_BORDER,
                    BorderColorBottom = new BaseColor(System.Drawing.Color.Black),
                    BorderWidthBottom = 1f,
                    PaddingTop = 10f,
                    PaddingBottom = 10f
                };
                headerTable.AddCell(pdfCellLogo);

                // Add empty middle cell
                PdfPCell middleCell = new PdfPCell
                {
                    Border = Rectangle.NO_BORDER,
                    BorderColorBottom = new BaseColor(System.Drawing.Color.Black),
                    BorderWidthBottom = 1f
                };
                headerTable.AddCell(middleCell);

                // Add title to the header table
                PdfPTable nested = new PdfPTable(1)
                {
                    DefaultCell = { Border = Rectangle.NO_BORDER }
                };
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 fromDate " + fromDate);
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 toDate " + toDate);
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee Attendance Details - " + fromDate + " " + "To" + " " + toDate, titleFont))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 20f
                };
                nested.AddCell(nextPostCell1);
                nested.AddCell("");
                PdfPCell nestedHousing = new PdfPCell(nested)
                {
                    Border = Rectangle.NO_BORDER,
                    BorderColorBottom = new BaseColor(System.Drawing.Color.Black),
                    BorderWidthBottom = 1f,
                    Rowspan = 6,
                    PaddingTop = 10f
                };
                headerTable.AddCell(nestedHousing);

                // Add header and invoice table to the document
                pdfDoc.Add(headerTable);
                pdfDoc.Add(new Paragraph("\n\n\n\n\n\n"));
                // Fetch unique dates for the attendance data
                var dates = attendaceListViewModels.AttendaceListViewModel
                    .GroupBy(x => x.Date)
                    .Select(x => x.Key)
                    .OrderBy(date => date) // Ensure dates are in order
                    .ToList();

                // Create body table
                int totalColumns = dates.Count() + 4; // 1 for Name, 3 for THrs, BHrs, AHrs
                PdfPTable tableLayout = new PdfPTable(totalColumns)
                {
                    HorizontalAlignment = Element.ALIGN_CENTER, // Center the table
                    WidthPercentage = 85, // Set the PDF file width percentage
                    HeaderRows = 1 // Set the number of header rows
                };

                // Dynamically calculate header widths
                float extraWidth = 2f; // Factor to increase width for specific columns
                float baseWidth = 100f / totalColumns; // Even distribution for other columns
                //float[] headers = new float[totalColumns];
                float[] headers = new float[totalColumns];
                for (int i = 0; i < totalColumns; i++)
                {
                    headers[i] = (i < 4) ? 40f : 36f; // Adjust widths as needed
                }
                tableLayout.SetWidths(headers);

                //Working Code
                // Add header
                AddCellToHeader(tableLayout, "Employee Name");

                // Dynamically add date headers
                foreach (var date in dates)
                {
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 date start " + date);
                    AddCellToHeader(tableLayout, DateTimeExtensions.ConvertToNotNullDatetime(date).Date.ToString("dd/MM/yy"));

                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 date end " + date);
                }

                AddCellToHeader(tableLayout, "Actual Hours");
                AddCellToHeader(tableLayout, "Break Hours");
                AddCellToHeader(tableLayout, "Total Hours");

                // Add body
                var employees = listEmployees.ToList();

                // Initialize total expected working hours in seconds
                double totalExpectedWorkingHoursInSeconds = dates.Count * (8 * 3600);
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 totalExpectedWorkingHoursInSeconds");
                // Add body
                foreach (var employee in employees)
                {
                    AddCellToBody(tableLayout, employee.FirstName + " " + employee.LastName);

                    // Initialize total time in seconds
                    double totalHoursSumInSeconds = 0;
                    double totalBreakHoursSumInSeconds = 0;
                    double totalActualHoursSumInSeconds = 0;

                    // Calculate remaining expected working hours (to account for "N/A")
                    double remainingExpectedWorkingHoursInSeconds = totalExpectedWorkingHoursInSeconds;

                    // Process each date
                    foreach (var date in dates)
                    {

                        Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 date 01 start " + date);
                        var attendance = attendaceListViewModels.AttendaceListViewModel.FirstOrDefault(x => x.Date == date && x.EmployeeId == employee.EmpId);
                        if (attendance != null)
                        {
                            Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 date 01 start attendance not null");
                            // Parse InsideOffice time to TimeSpan and calculate total seconds
                            TimeSpan insideOfficeTime = ParseTime(attendance.InsideOffice);
                            double insideOfficeSeconds = insideOfficeTime.TotalSeconds;

                            // Parse BreakHours and TotalHours to TimeSpan and calculate total seconds
                            TimeSpan breakHoursTime = ParseTime(attendance.BreakHours);
                            double breakHoursSeconds = breakHoursTime.TotalSeconds;

                            TimeSpan totalHoursTime = ParseTime(attendance.TotalHours);
                            double totalHoursSeconds = totalHoursTime.TotalSeconds;

                            // Accumulate break hours and total hours
                            totalBreakHoursSumInSeconds += breakHoursSeconds;
                            totalHoursSumInSeconds += totalHoursSeconds;

                            // Subtract the processed day's working hours from the remaining expected hours
                            remainingExpectedWorkingHoursInSeconds -= (8 * 3600);

                            // Create the PdfPCell
                            PdfPCell insideOfficeCell = new PdfPCell(new Phrase(attendance.InsideOffice, bodyFont));

                            // Check if InsideOffice is less than 8 hours or 7 hours
                            if (insideOfficeSeconds < (8 * 3600))
                            {
                                insideOfficeCell.BackgroundColor = BaseColor.ORANGE;
                            }
                            if (insideOfficeSeconds < (7 * 3600))
                            {
                                insideOfficeCell.BackgroundColor = BaseColor.RED;
                            }

                            // Add the cell to the table
                            tableLayout.AddCell(insideOfficeCell);

                            // Accumulate total seconds for actual hours
                            totalActualHoursSumInSeconds += insideOfficeSeconds;
                        }
                        else
                        {
                            // Add "N/A" if no attendance data is available
                            PdfPCell naCell = new PdfPCell(new Phrase("N/A", bodyFont))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY
                            };
                            tableLayout.AddCell(naCell);

                            // Subtract 8 hours for each "N/A"
                            remainingExpectedWorkingHoursInSeconds -= (8 * 3600);

                        }
                    }
                    Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 date 01 start attendance end");

                    // Format and add the actual hours cell
                    string formattedActualHours = FormatTimeFromSeconds(totalActualHoursSumInSeconds);
                    PdfPCell actualHoursCell = new PdfPCell(new Phrase(formattedActualHours, bodyFont));
                    double thresholdInSecondsDanger = remainingExpectedWorkingHoursInSeconds - (5 * 3600);
                    double thresholdInSecondsWarning = remainingExpectedWorkingHoursInSeconds - (3 * 3600);

                    if (totalActualHoursSumInSeconds < thresholdInSecondsDanger)
                    {
                        actualHoursCell.BackgroundColor = BaseColor.RED;
                    }
                    else if (totalActualHoursSumInSeconds < thresholdInSecondsWarning)
                    {
                        actualHoursCell.BackgroundColor = BaseColor.ORANGE;
                    }

                    tableLayout.AddCell(actualHoursCell);

                    // Format and add the break hours cell
                    string formattedBreakHours = FormatTimeFromSeconds(totalBreakHoursSumInSeconds);
                    tableLayout.AddCell(new PdfPCell(new Phrase(formattedBreakHours, bodyFont)));

                    // Format and add the total hours cell
                    string formattedTotalHours = FormatTimeFromSeconds(totalHoursSumInSeconds);
                    tableLayout.AddCell(new PdfPCell(new Phrase(formattedTotalHours, bodyFont)));
                }

                // Add the table to the document
                pdfDoc.Add(tableLayout);
                PdfContentByte cb = writer.DirectContent;
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 rights");
                // Calculate the width of the text
                string text = DateTime.Now.Year + " " + "VpHospital. All Rights Reserved";
                float textWidth = bf.GetWidthPoint(text, 8);

                // Calculate the center of the page
                float centerX = (pageSize.Width - textWidth) / 2;

                // Set the position to the center of the page
                cb.SetTextMatrix(centerX, 20);  // Adjust Y-coordinate as necessary
                cb.ShowText(text);

                // Set the color for the text
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.EndText();

                // Draw a line at the bottom
                cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                cb.Stroke();

                // Close the document
                pdfDoc.Close();
                Common.Common.WriteServerErrorLog("Service Employees Weekly attendance EmailScheduler - SendEmployeeAttendanceForManagement 03 pdf close");
                return filePath;

            }
            catch (Exception ex)
            {

            }
            ;
            return null;
        }

        private TimeSpan ParseTime(string timeString)
        {
            if (TimeSpan.TryParse(timeString, out var timeSpan))
            {
                return timeSpan;
            }
            else
            {
                return TimeSpan.Zero; // Return 0 if the time string is invalid
            }
        }

        // Helper function to format total seconds as "hh:mm:ss"
        private string FormatTimeFromSeconds(double totalSeconds)
        {
            // Calculate hours, minutes, and seconds
            int hours = (int)(totalSeconds / 3600);
            int minutes = (int)((totalSeconds % 3600) / 60);
            int seconds = (int)(totalSeconds % 60);

            // Return the formatted time as "hh:mm:ss"
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }
        public string CreatePdfFiles(AttendaceListViewModels attendaceListViewModels, string fromDate, string toDate)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/AttendanceDetails");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var documentPath = string.Empty;
            var fileName = Guid.NewGuid().ToString() + "_" + "EmployeeAttendance" +
                DateTimeExtensions.ConvertToNotNullDatetime(fromDate).ToString("yyyyMMdd") + Constant.Hyphen +
                DateTimeExtensions.ConvertToNotNullDatetime(toDate).ToString("yyyyMMdd") + ".pdf";
            var filePath = Path.Combine(directoryPath, fileName);
            documentPath = directoryPath.Replace(directoryPath, "~/AttendanceDetails/") + fileName;
            Document pdfDoc = new Document();
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
            BaseColor myColor = BaseColor.BLUE;
            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, BaseColor.BLUE);
            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, myColor);
            BaseColor TableHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

            Rectangle pageSize = writer.PageSize;
            // Open the Document for writing
            pdfDoc.Open();
            //Add elements to the document here

            // Create the header table 
            PdfPTable headerTable = new PdfPTable(3);
            headerTable.HorizontalAlignment = 0;
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

            // headerTable.DefaultCell.Border = Rectangle.NO_BORDER;            
            headerTable.DefaultCell.Border = Rectangle.BOX; //for testing           
            string webRootPath = _hostingEnvironment.WebRootPath;
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
            logo.ScaleToFit(100, 70);

            {
                PdfPCell pdfCellLogo = new PdfPCell(logo);
                pdfCellLogo.Border = Rectangle.NO_BORDER;
                pdfCellLogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                pdfCellLogo.BorderWidthBottom = 1f;
                pdfCellLogo.PaddingTop = 10f;
                pdfCellLogo.PaddingBottom = 10f;
                headerTable.AddCell(pdfCellLogo);
            }

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                middleCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                middleCell.BorderWidthBottom = 1f;
                headerTable.AddCell(middleCell);
            }

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee Attendance Details - " + fromDate + " " + "To" + " " + toDate, titleFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.PaddingBottom = 20f;
                nested.AddCell(nextPostCell1);

                nested.AddCell("");
                PdfPCell nestedHousing = new PdfPCell(nested);
                nestedHousing.Border = Rectangle.NO_BORDER;
                nestedHousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                nestedHousing.BorderWidthBottom = 1f;
                nestedHousing.Rowspan = 6;
                nestedHousing.PaddingTop = 10f;
                headerTable.AddCell(nestedHousing);
            }

            PdfPTable InvoiceTable = new PdfPTable(3);
            InvoiceTable.HorizontalAlignment = 0;
            InvoiceTable.WidthPercentage = 100;
            InvoiceTable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
            InvoiceTable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                InvoiceTable.AddCell(middleCell);
            }

            {
                PdfPTable title = new PdfPTable(1);
                title.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell();
                nextPostCell1.Border = Rectangle.NO_BORDER;
                title.AddCell(nextPostCell1);
                title.AddCell("");
                PdfPCell nestedHousings = new PdfPCell(title);
                nestedHousings.Border = Rectangle.NO_BORDER;
                nestedHousings.Rowspan = 5;
                nestedHousings.PaddingBottom = 30f;
                nestedHousings.HorizontalAlignment = Element.ALIGN_CENTER;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                InvoiceTable.AddCell(nestedHousings);
            }

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                middleCell.PaddingTop = 20f;
                InvoiceTable.AddCell(middleCell);
            }

            pdfDoc.Add(headerTable);
            pdfDoc.Add(InvoiceTable);

            //Create body table
            PdfPTable tableLayout = new PdfPTable(9);
            float[] headers = { 15, 44, 33, 36, 36, 40, 40, 42, 38 }; //Header Widths  
            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 85; //Set the PDF File width percentage  
            tableLayout.HeaderRows = 0;

            //Add header
            AddCellToHeader(tableLayout, "Id");
            AddCellToHeader(tableLayout, "Name");
            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "EntryTime");
            AddCellToHeader(tableLayout, "ExitTime");
            AddCellToHeader(tableLayout, "TotalHours");
            AddCellToHeader(tableLayout, "BreakHours");
            AddCellToHeader(tableLayout, "ActualHours");
            AddCellToHeader(tableLayout, "TimeSheetHours");

            //Add body  

            foreach (var emp in attendaceListViewModels.AttendaceListViewModel)
            {

                //AddCellToBody(tableLayout, emp.EmployeeId.ToString());
                //AddCellToBody(tableLayout, emp.EmployeeName);
                //AddCellToBody(tableLayout, emp.Date.ToString());
                //AddCellToBody(tableLayout, emp.EntryTime.ToString());
                //AddCellToBody(tableLayout, emp.ExitTime.ToString());
                //AddCellToBody(tableLayout, emp.TotalHours.ToString());
                //AddCellToBody(tableLayout, emp.BreakHours.ToString());
                //AddCellToBody(tableLayout, emp.InsideOffice.ToString());
                //AddCellToBody(tableLayout, emp.BurningHours.ToString());

                AddCellToBody(tableLayout, emp.EmployeeId.ToString() ?? "");
                AddCellToBody(tableLayout, emp.EmployeeName ?? "");
                AddCellToBody(tableLayout, emp.Date?.ToString() ?? "");
                AddCellToBody(tableLayout, emp.EntryTime?.ToString() ?? "");
                AddCellToBody(tableLayout, emp.ExitTime?.ToString() ?? "");
                AddCellToBody(tableLayout, emp.TotalHours?.ToString() ?? "");
                AddCellToBody(tableLayout, emp.BreakHours?.ToString() ?? "");
                AddCellToBody(tableLayout, emp.InsideOffice?.ToString() ?? "");
                AddCellToBody(tableLayout, emp.BurningHours?.ToString() ?? "");

            }
            pdfDoc.Add(tableLayout);
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

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = WebColors.GetRGBColor("#1a76d1")
            });
        }
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 7, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5
            });
        }

        public async Task<List<AttendanceReportDateModel>> GetAllAttendancereport(AttendaceListViewModel attendaceListViewModel)
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
            var attendanceReportDateModels = await _attendanceRepository.GetAllEmployeesByAttendaceFilter("spGetAttendanceByEmployeeFilterData", p);
            attendanceReportDateModels = attendanceReportDateModels.AsQueryable().DistinctBy(x => x.LogDateTime).ToList();
            return attendanceReportDateModels;

        }
        public async Task<AttendaceListViewModels> GetAllEmployeesAttendanceFilter(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<AttendanceReportDateModel> attendanceReportDateModels, List<TimeSheetEntity> getAllTimeSheetlist,int companyId)
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
                        if (item.EsslId != null)
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
                            else
                            {
                                var timeLogEntitys = await _dashboardRepository.GetTimeLogEntityByEmpId(item.EmpId,date.Date,companyId);
                                var tFrom = timeLogEntitys.Select(x => Convert.ToString(x.ClockInTime)).FirstOrDefault();
                                var tTo = timeLogEntitys.Where(x => x.ClockOutTime.HasValue && x.EntryStatus == Constant.EntryTypeOut).Select(x => x.ClockOutTime.Value.ToString()).LastOrDefault();
                                double breaks = 0;
                                var attendanceReportDatamodelList = await _dashboardService.GetTimeLog(Convert.ToInt32(item.EmpId), Convert.ToDateTime(tFrom),Convert.ToInt32(companyId));
                                var breakHours = GetBreakHourForTimeLogger(Convert.ToString(Convert.ToInt32(item.EmpId)), timeLogEntitys, Convert.ToDateTime(tFrom));
                                var burningHours = GetTimeSheetWorkHours(item.EmpId, timeSheets);
                                attendacelistViewModel.EmployeeId = item.EmpId;
                                attendacelistViewModel.UserName = item.UserName;
                                attendacelistViewModel.EmployeeName = item.FirstName + " " + item.LastName;
                                attendacelistViewModel.Date = date.Date.ToString(Constant.DateFormat);
                                attendacelistViewModel.BreakHours = Convert.ToDateTime(breakHours).ToString(Constant.TimeFormat24HrsHM);
                                attendacelistViewModel.TotalHours = attendanceReportDatamodelList.TotalHours;
                                attendacelistViewModel.InsideOffice = attendanceReportDatamodelList.InsideOffice;
                                attendacelistViewModel.BurningHours = burningHours;
                                attendacelistViewModel.EntryTime = Convert.ToDateTime(tFrom).ToString(Constant.TimeFormatWithFullForm);
                                attendacelistViewModel.ExitTime = Convert.ToDateTime(tTo).ToString(Constant.TimeFormatWithFullForm);
                                attendaceListViewModels.AttendaceListViewModel.Add(attendacelistViewModel);
                            }
                        }
                    }
                }
                return attendaceListViewModels;
            }
            catch (Exception ex)
            {

            }
            return new AttendaceListViewModels();
        }
        public string GetBreakHour(string empId, List<AttendanceReportDateModel> attendanceEntities, DateTime filterDate)
        {
            var attendanceEty = attendanceEntities.Where(x => x.EmployeeId == empId && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date).ToList();
            var listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).ToList();

            if (listOfRecords.Count > 0)
            {
                var attendanceReportDateModel = listOfRecords.Take(1).FirstOrDefault();

                if (attendanceReportDateModel?.Direction == Constant.EntryTypeOut)
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
        public string GetTimeSheetWorkHours(int empId, List<TimeSheetEntity> timeSheetEntities)
        {
            double totalSeconds = 0;
            var answer = "";
            var timeSheets = timeSheetEntities.Where(x => x.EmpId == empId).ToList();
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
        public async Task<bool> InsertEmailAttendance(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, List<string> fileId)
        {
            var result = false;
            var combinePath = new List<string>();
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
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
                    combinePath.Add(item);
                }
            }
            var filePath = combinePath;
            if (filePath.Count() > 0)
            {
                var str = string.Join(",", filePath);
                emailEntity.Attachments = str.TrimEnd(',');
            }
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            if (emailEntity != null)
            {
                result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            }
            return result;
        }
        public async Task TimeSheetMailScheduler(MailSchedulerEntity mailSchedule)
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
                else if (mailSchedule?.DurationId == (int)Common.Enums.Duration.Monthly/*mailSchedule.EmailDraftId == (int)EmailDraftType.MonthlyAttendance*/)
                {
                    timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddMonths(-1).ToString(Constant.DateFormat);
                    timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                }
                //else if (mailSchedule?.DurationId == (int)Duration.Yearly /*mailSchedule.EmailDraftId == (int)EmailDraftType.YearlyAttendance*/)
                //{
                //    timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.AddYears(-1).ToString(Constant.DateFormat);
                //    timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(mailSchedule.MailTime.ToString(Constant.DateFormat)).Date.ToString(Constant.DateFormat);
                //}
                else if (mailSchedule?.DurationId == (int)Common.Enums.Duration.Custom)
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
                    var getAllEmployee = await _employeesRepository.GetAllEmployeeByCompany(mailSchedule.CompanyId);
                    List<int> empIds = mailSchedule.WhomToSend.Split(',').Select(int.Parse).ToList();
                    if (empIds[0] != 0 && empIds.Count() > 0)
                    {
                        foreach (int empId in empIds)
                        {
                            var getEmployee = getAllEmployee.Where(x => x.EmpId == empId).FirstOrDefault();
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
                        activeEmployee = getAllEmployee;
                    }

                    if (activeEmployee.Count() > 0)
                    {
                        if (mailSchedule.WhomToSend == Constant.ZeroStr || empIds.Count() > 0)
                        {
                            sendMail = await SendTimesheetForAllEmployee(timeSheetReports, empIds, combinedPath, mailSchedule, activeEmployee, allDates);
                        }
                        else
                        {
                            sendMail = await SendMailForTimeSheet(timeSheetReports, mailSchedule, combinedPath);
                        }
                    }
                }
            }
        }

        public async Task<bool> SendTimesheetForAllEmployee(TimeSheetReports timeSheetReports, List<int> empIds, List<string> fileId, MailSchedulerEntity mailSchedulerEntity, List<EmployeesEntity> activeEmployee, List<DateTime> allDates)
        {
            var result = false;
            var combinedPath = new List<string>();
            var filterDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartDate);
            timeSheetReports.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.StartDate).ToString(Constant.DateFormat);
            timeSheetReports.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetReports.EndDate).ToString(Constant.DateFormat);
            var timesheet = new TimeSheetReports();
            timesheet.FilterViewTimeSheet = new List<FilterViewTimeSheet>();
            var timesheetList = await GetAllTimesheetReport(timeSheetReports);
            var employees = await _employeesRepository.GetAllEmployeeByCompany(mailSchedulerEntity.CompanyId);
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

                    var attendanceViewModelList = await GetAllTimesheetFilter(allDates, listEmployees, timesheetList);
                    listEmployees = null;

                    var filterViewTimeSheetModel = new FilterViewTimeSheet();

                    if (getEmployeeDetails?.EmpId >= 0 && attendanceViewModelList.FilterViewTimeSheet?.Count() > 0)
                    {
                        toEmail = string.IsNullOrEmpty(getEmployee?.OfficeEmail) ? "" : getEmployee.OfficeEmail;
                        employeeName = getEmployee != null ? getEmployeeDetails.FirstName + " " + getEmployeeDetails.LastName : "";

                        if (mailSchedulerEntity.FileFormat == (int)FileFormats.Excel)
                        {
                            var excel = GenerateExcelTimeSheet(attendanceViewModelList.FilterViewTimeSheet);
                            combinedPath.Add(excel);
                        }
                        else if (mailSchedulerEntity.FileFormat == (int)Common.Enums.FileFormats.Pdf)
                        {
                            var pdf = CreatePdfTimeSheet(attendanceViewModelList, timeSheetReports.StartDate, timeSheetReports.EndDate);
                            combinedPath.Add(pdf);
                        }
                        else if (mailSchedulerEntity.FileFormat == (int)FileFormats.ExcelPDF)
                        {
                            var excel = GenerateExcelTimeSheet(attendanceViewModelList.FilterViewTimeSheet);
                            combinedPath.Add(excel);

                            var pdf = CreatePdfTimeSheet(attendanceViewModelList, timeSheetReports.StartDate, timeSheetReports.EndDate);
                            combinedPath.Add(pdf);
                        }

                        if (combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.WeeklyTimeSheet)
                        {
                            var draftTypeId = (int)EmailDraftType.WeeklyTimeSheet;
                            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, getEmployee.CompanyId);
                            if (emailDraftContentEntity != null)
                            {
                                var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(timeSheetReports.StartDate, timeSheetReports.EndDate, employeeName, emailDraftContentEntity.DraftBody);
                                result = await InsertEmailTimesheet(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                            }
                        }

                        else if (combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.MonthlyTimeSheet)
                        {
                            var draftTypeId = (int)EmailDraftType.MonthlyTimeSheet;
                            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);
                            if (emailDraftContentEntity != null)
                            {
                                var bodyContent = EmailBodyContent.SendEmail_Body_AttendanceForEmployeeWeekly(timeSheetReports.StartDate, timeSheetReports.EndDate, employeeName, emailDraftContentEntity.DraftBody);
                                result = await InsertEmailTimesheet(toEmail, emailDraftContentEntity, bodyContent, combinedPath);
                            }
                        }
                        combinedPath = new List<string>(0);
                    }

                }
                if (mailSchedulerEntity.DurationId == (int)Duration.Custom && result == true)
                {
                    var mailScheduler = await _companyRepository.GetMailSchedulerBySchedulerId(mailSchedulerEntity.SchedulerId);
                    DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddDays(7).ToString(Constant.DateFormat));
                    var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                    mailScheduler.MailTime = sendMail;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
                else if (mailSchedulerEntity.DurationId == (int)Duration.Monthly && result == true)
                {
                    var mailScheduler = await _companyRepository.GetMailSchedulerBySchedulerId(mailSchedulerEntity.SchedulerId);
                    DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));
                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddMonths(1).ToString(Constant.DateFormat));
                    var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);
                    mailScheduler.MailTime = sendMail;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
                else if (mailSchedulerEntity.DurationId == (int)Duration.Once && result == true)
                {
                    var mailScheduler = await _companyRepository.GetMailSchedulerBySchedulerId(mailSchedulerEntity.SchedulerId);
                    mailScheduler.IsActive = false;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
            }

            return result;
        }
        public string GenerateExcelTimeSheet(List<FilterViewTimeSheet> timeSheetReports)
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
                    var progress = "";
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
                        progress = Constant.Pending;
                    }
                    else if (Status == (int)TimeSheetStatus.Inprogress)
                    {
                        progress = Constant.InProgress;
                    }
                    else if (Status == (int)TimeSheetStatus.Completed)
                    {
                        progress = Constant.Completed;
                    }
                    worksheet.Cell(currentRow, 8).Value = progress;

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
        public string CreatePdfTimeSheet(TimeSheetReports timeSheet, string fromDate, string toDate)
        {

            string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "TimeSheet");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + "EmployeeTimeSheet" + DateTimeExtensions.ConvertToNotNullDatetime(fromDate).ToString("yyyyMMdd") + Constant.Hyphen + DateTimeExtensions.ConvertToNotNullDatetime(toDate).ToString("yyyyMMdd") + ".pdf");
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 10, 10);
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));



            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            BaseColor TableHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

            Rectangle pageSize = writer.PageSize;

            pdfDoc.Open();

            PdfPTable headerTable = new PdfPTable(3);
            headerTable.HorizontalAlignment = 0;
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 200f, 5f, 350f });



            headerTable.DefaultCell.Border = Rectangle.BOX;
            string webRootPath = _hostingEnvironment.WebRootPath;
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
            logo.ScaleToFit(100, 70);

            {
                PdfPCell pdfCellLogo = new PdfPCell(logo);
                pdfCellLogo.Border = Rectangle.NO_BORDER;
                pdfCellLogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                pdfCellLogo.BorderWidthBottom = 1f;
                pdfCellLogo.PaddingTop = 10f;
                pdfCellLogo.PaddingBottom = 10f;
                pdfCellLogo.PaddingBottom = 10f;
                headerTable.AddCell(pdfCellLogo);
            }

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                middleCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                middleCell.BorderWidthBottom = 1f;
                headerTable.AddCell(middleCell);
            }

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Time Sheet Log Report - " + fromDate + " " + "To" + " " + toDate, titleFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.PaddingBottom = 20f;
                nested.AddCell(nextPostCell1);

                nested.AddCell("");
                PdfPCell nestedHousing = new PdfPCell(nested);
                nestedHousing.Border = Rectangle.NO_BORDER;
                nestedHousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                nestedHousing.BorderWidthBottom = 1f;
                nestedHousing.Rowspan = 6;
                nestedHousing.PaddingTop = 10f;
                headerTable.AddCell(nestedHousing);
            }

            PdfPTable InvoiceTable = new PdfPTable(3);
            InvoiceTable.HorizontalAlignment = 0;
            InvoiceTable.WidthPercentage = 100;
            InvoiceTable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
            InvoiceTable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                InvoiceTable.AddCell(middleCell);
            }

            {
                PdfPTable title = new PdfPTable(1);
                title.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell();
                nextPostCell1.Border = Rectangle.NO_BORDER;
                title.AddCell(nextPostCell1);
                title.AddCell("");
                PdfPCell nestedHousings = new PdfPCell(title);
                nestedHousings.Border = Rectangle.NO_BORDER;
                nestedHousings.Rowspan = 5;
                nestedHousings.PaddingBottom = 30f;
                nestedHousings.HorizontalAlignment = Element.ALIGN_CENTER;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                InvoiceTable.AddCell(nestedHousings);
            }

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                middleCell.PaddingTop = 20f;
                InvoiceTable.AddCell(middleCell);
            }

            pdfDoc.Add(headerTable);
            pdfDoc.Add(InvoiceTable);

            //Create body table
            PdfPTable itemTable = new PdfPTable(8);
            float[] headers = { 58, 40, 52, 68, 45, 46, 44, 46 }; //Header Widths  
            itemTable.SetWidths(headers);

            AddCellToHeader(itemTable, "Name");
            AddCellToHeader(itemTable, "Id");
            AddCellToHeader(itemTable, "Project");
            AddCellToHeader(itemTable, "Task Name");
            AddCellToHeader(itemTable, "Date");
            AddCellToHeader(itemTable, "Start Time");
            AddCellToHeader(itemTable, "End Time");
            AddCellToHeader(itemTable, "Status");


            foreach (var emp in timeSheet.FilterViewTimeSheet)
            {
                AddCellToBody(itemTable, emp.FirstName + " " + emp.LastName);
                AddCellToBody(itemTable, emp.EmployeeUserId);
                AddCellToBody(itemTable, emp.ProjectName);
                AddCellToBody(itemTable, emp.TaskName);
                AddCellToBody(itemTable, emp.StartDate.ToString(Constant.DateFormat));
                AddCellToBody(itemTable, emp.StartTime.ToString(Constant.TimeFormatlog));
                AddCellToBody(itemTable, emp.EndTime.ToString(Constant.TimeFormatlog));
                if (emp.Status == 1)
                {
                    AddCellToBody(itemTable, Constant.Pending);
                }
                else if (emp.Status == 2)
                {
                    AddCellToBody(itemTable, Constant.InProgress);
                }
                else
                {
                    AddCellToBody(itemTable, Constant.Completed);
                }

            }
            pdfDoc.Add(itemTable);

            PdfContentByte cb = new PdfContentByte(writer);

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            cb = new PdfContentByte(writer);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(120), 20);
            cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
            cb.EndText();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
            cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
            cb.Stroke();
            pdfDoc.Close();
            return filePath;
        }
        public async Task<List<TimeSheetDataModel>> GetAllTimesheetReport(TimeSheetReports timesheetViewModel)
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
            var timesheetReportDateModels = await _reportRepository.GetAllEmployessByTimeSheetFilter("spGetTimeSheetByEmployeeFilterData", p);
            timesheetReportDateModels = timesheetReportDateModels.ToList();
            return timesheetReportDateModels;

        }
        public async Task<TimeSheetReports> GetAllTimesheetFilter(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<TimeSheetDataModel> timeSheetDataModels)
        {
            try
            {
                var timesheetData = new TimeSheetReports();
                timesheetData.FilterViewTimeSheet = new List<FilterViewTimeSheet>();

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

                                var getAllTimeSheetList = await _projectDetailsRepository.GetProjectByCompnayId(item.ProjectId, employee.CompanyId);
                                filterViewTimeSheet.Id = item.Id;
                                filterViewTimeSheet.EmployeeId = item.EmployeeId;
                                filterViewTimeSheet.StartDate = item.StartDate;
                                filterViewTimeSheet.Status = item.Status;
                                filterViewTimeSheet.StartTime = item.StartTime;
                                filterViewTimeSheet.EndTime = item.EndTime;
                                filterViewTimeSheet.ProjectId = item.ProjectId;
                                filterViewTimeSheet.ProjectName = getAllTimeSheetList.ProjectName;
                                filterViewTimeSheet.EmployeeUserId = item.EmployeeUserId;
                                filterViewTimeSheet.TaskDescription = item.TaskDescription;
                                filterViewTimeSheet.TaskName = item.TaskName;
                                filterViewTimeSheet.AttachmentFileName = item.AttachmentFileName;
                                filterViewTimeSheet.AttachmentFilePath = item.AttachmentFilePath;
                                filterViewTimeSheet.FirstName = item.FirstName;
                                filterViewTimeSheet.LastName = item.LastName;
                                timesheetData.FilterViewTimeSheet.Add(filterViewTimeSheet);
                            }

                        }
                    }
                }
                return timesheetData;
            }
            catch (Exception)
            {

            }
            return new TimeSheetReports();
        }
        public async Task<bool> InsertEmailTimesheet(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, List<string> fileId)
        {
            var result = false;
            var combinePath = new List<string>();
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
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
            var filePath = combinePath;
            if (filePath.Count() > 0)
            {
                var str = string.Join(",", filePath);
                emailEntity.Attachments = str.TrimEnd(',');
            }
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            if (emailEntity != null)
            {
                result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            }
            return result;
        }
        public async Task<bool> SendMailForTimeSheet(TimeSheetReports TimeSheetReportsViewModel, MailSchedulerEntity mailSchedulerEntity, List<string> fileId)
        {
            var result = false;
            var combinePath = new List<string>();
            if (TimeSheetReportsViewModel.EmployeeId == 0)
            {
                var draftTypeId = (int)EmailDraftType.AttendanceLogForManagement;
                var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(mailSchedulerEntity.EmailDraftId,mailSchedulerEntity.CompanyId);
                MailMessage mailMessage = new MailMessage();
                var toEmail = emailDraftContentEntity.Email;
                var subject = emailDraftContentEntity.Subject;
                var startDate = string.IsNullOrEmpty(TimeSheetReportsViewModel.StartTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(TimeSheetReportsViewModel.StartTime).ToString(Constant.DateFormat);
                var endDate = string.IsNullOrEmpty(TimeSheetReportsViewModel.EndTime) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(TimeSheetReportsViewModel.EndTime).ToString(Constant.DateFormat);
                var mailBody = EmailBodyContent.SendEmail_Body_AttendanceForManagement(startDate, endDate, emailDraftContentEntity.DraftBody);
                var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
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
                var filePath = combinePath;
                if (filePath.Count() > 0)
                {
                    var str = string.Join(",", filePath);
                    emailEntity.Attachments = str.TrimEnd(',');
                }
                emailEntity.IsSend = false;
                emailEntity.Reason = "EmployeesTimesheetDetailsReason";
                emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                emailEntity.CreatedDate = DateTime.Now;
                result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            }
            return result;
        }
        public async Task LeaveMailSchedule(MailSchedulerEntity mailSchedule,int companyId)
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
                    var getAllEmployee = await _employeesRepository.GetAllEmployeeByCompany(mailSchedule.CompanyId);
                    List<int> empIds = mailSchedule.WhomToSend.Split(',').Select(int.Parse).ToList();
                    if (empIds[0] != 0 && empIds.Count() > 0)
                    {
                        foreach (int empId in empIds)
                        {
                            var getEmployee = getAllEmployee.Where(x => x.EmpId == empId).FirstOrDefault();
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
                        activeEmployee = await _employeesRepository.GetAllActiveEmployees(companyId);
                    }
                    if (mailSchedule.WhomToSend == Constant.ZeroStr || empIds.Count() > 0)
                    {
                        sentMail = await SentMailAllEmployees(employeeLeaveViewModel, empIds, combinedPath, mailSchedule, activeEmployee, allDates, companyId);
                    }
                }
            }
        }
        public async Task<bool> SentMailAllEmployees(EmployeeLeaveViewModel employeeLeaveViewModel, List<int> empIds, List<string> fileId, MailSchedulerEntity mailSchedulerEntity, List<EmployeesEntity> activeEmployee, List<DateTime> allDates,int sessionCompanyId)

        {
            var result = false;
            var combinedPath = new List<string>();
            var filterDate = employeeLeaveViewModel.LeaveFromDate;
            var toDate = employeeLeaveViewModel.LeaveToDate;

            var leaveReport = await GetAllEmployeesByLeaveFilter(employeeLeaveViewModel);

            var employees = await _employeesRepository.GetAllEmployeeDetails(sessionCompanyId);
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

                    toMail = string.IsNullOrEmpty(employee?.OfficeEmail) ? string.Empty : employee.OfficeEmail;

                    var getEmployee = employees.Where(x => x.EmpId == item).FirstOrDefault();
                    var LeaveListModel = await GetAllEmployeeLeaveFilter(allDates, employeeList, leaveReport);
                    var employeeLeaveViewModels = new EmployeeLeaveViewModel();

                    if (getEmployee?.EsslId > 0 && LeaveListModel.leaveFilterViewModels.Count() > 0)
                    {
                        if (mailSchedulerEntity.FileFormat == (int)FileFormats.Excel)
                        {
                            var excel = await CreateExcel(LeaveListModel.leaveFilterViewModels);
                            combinedPath.Add(excel);
                        }

                        else if (mailSchedulerEntity.FileFormat == (int)FileFormats.Pdf)
                        {
                            var pdf = await CreatePdfForLeave(LeaveListModel.leaveFilterViewModels, filterDate, toDate);
                            combinedPath.Add(pdf);
                        }
                        else if (mailSchedulerEntity.FileFormat == (int)FileFormats.ExcelPDF)
                        {
                            var excel = await CreateExcel(LeaveListModel.leaveFilterViewModels);
                            combinedPath.Add(excel);

                            var pdf = await CreatePdfForLeave(LeaveListModel.leaveFilterViewModels, filterDate, toDate);
                            combinedPath.Add(pdf);
                        }
                        if (combinedPath.Count() > 0 && mailSchedulerEntity.EmailDraftId == (int)EmailDraftType.MonthlyLeave)
                        {
                            var draftTypeId = (int)EmailDraftType.MonthlyLeave;
                            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);
                            if (emailDraftContentEntity != null)
                            {
                                var bodyContent = EmailBodyContent.SendEmail_Body_LeaveForEmployeeWeekly(Convert.ToString(filterDate.ToString(Constant.DateFormat)), Convert.ToString(toDate.ToString(Constant.DateFormat)), getEmployee.FirstName, emailDraftContentEntity.DraftBody);
                                result = await InsertEmailLeave(toMail, emailDraftContentEntity, bodyContent, combinedPath);
                            }
                        }
                        combinedPath = new List<string>(0);
                    }
                }
                if (mailSchedulerEntity.DurationId == (int)Duration.Monthly && result == true)
                {
                    var mailScheduler = await _companyRepository.GetMailSchedulerBySchedulerId(mailSchedulerEntity.SchedulerId);

                    DateTime mailTime = Convert.ToDateTime(mailScheduler.MailTime.ToString(Constant.TimeFormat));

                    var mailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.AddMonths(1).ToString(Constant.DateFormat));

                    var sendMail = mailDate.AddHours(mailTime.Hour).AddMinutes(mailTime.Minute);

                    mailScheduler.MailTime = sendMail;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }
                else if (mailSchedulerEntity.DurationId == (int)Duration.Once && result == true)
                {
                    var mailScheduler = await _companyRepository.GetMailSchedulerBySchedulerId(mailSchedulerEntity.SchedulerId);
                    mailScheduler.IsActive = false;
                    await _companyRepository.CreateMailScheduler(mailScheduler);
                }

            }
            return result;
        }
        public async Task<EmployeeLeaveViewModel> GetAllEmployeeLeaveFilter(List<DateTime> allDates, List<EmployeesEntity> activeEmployee, List<LeaveReportDateModel> leaveReportDateModels)
        {

            var listOfRecord = new EmployeeLeaveViewModel();
            listOfRecord.leaveFilterViewModels = new List<LeaveFilterViewModel>();
            foreach (var date in allDates)
            {
                foreach (var employee in activeEmployee)
                {
                    var leaveReport = leaveReportDateModels.Where(x => x.EmployeeId == employee.EmpId && x.LeaveFromDate == date).ToList();

                    var leaveFilterView = new LeaveFilterViewModel();

                    if (employee.EsslId > 0)
                    {
                        foreach (var item in leaveReport)
                        {
                            leaveFilterView.EmployeeUserName = item.EmployeeUserId;
                            leaveFilterView.EmployeeName = item.FirstName + "" + item.LastName;
                            leaveFilterView.LeaveTypes = item.LeaveType;
                            leaveFilterView.LeaveFromDate = item.LeaveFromDate;
                            leaveFilterView.LeaveToDate = item.LeaveToDate;
                            leaveFilterView.Reason = item.Reason;
                            leaveFilterView.LeaveCount = item.LeaveCount;
                            listOfRecord.leaveFilterViewModels.Add(leaveFilterView);

                        }
                    }
                }
            }
            return listOfRecord;
        }
        public async Task<List<LeaveReportDateModel>> GetAllEmployeesByLeaveFilter(EmployeeLeaveViewModel employeeLeaveViewModel)
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

            var filterEmployeeLeave = await _reportRepository.GetAllEmployessByLeaveTypeFilter("spGetLeaveTypeCountByEmployeeFilterData", p);
            return filterEmployeeLeave;
        }

        public async Task<bool> InsertEmailLeave(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent, List<string> fileId)
        {
            var result = false;
            var combinePath = new List<string>();
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
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
            var filePath = combinePath;
            if (filePath.Count() > 0)
            {
                var str = string.Join(",", filePath);
                emailEntity.Attachments = str.TrimEnd(',');
            }
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            if (emailEntity != null)
            {
                result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            }
            return result;
        }
        public async Task<string> CreateExcel(List<LeaveFilterViewModel> employeeViewModel)
        {
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
            string? combinedPath = Path.Combine(path, fileNames);
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
        public async Task<string> CreatePdfForLeave(List<LeaveFilterViewModel> leaveFilters, DateTime filterDate, DateTime toDate)
        {
            string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "LeaveReport");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + Constant.LeaveReport + Convert.ToDateTime(filterDate).ToString(Constant.DateFormatYMD) + Constant.Hyphen + Convert.ToDateTime(toDate).ToString(Constant.DateFormatYMD) + Constant.pdf);
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4, 10, 10, 10, 10);
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
            PdfPTable headerTable = new PdfPTable(3);
            headerTable.HorizontalAlignment = 0;
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

            // headerTable.DefaultCell.Border = Rectangle.NO_BORDER;            
            headerTable.DefaultCell.Border = Rectangle.BOX; //for testing           
            string webRootPath = _hostingEnvironment.WebRootPath;
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
            logo.ScaleToFit(100, 70);

            {
                PdfPCell pdfCellLogo = new PdfPCell(logo);
                pdfCellLogo.Border = Rectangle.NO_BORDER;
                pdfCellLogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                pdfCellLogo.BorderWidthBottom = 1f;
                pdfCellLogo.PaddingTop = 10f;
                pdfCellLogo.PaddingBottom = 10f;
                headerTable.AddCell(pdfCellLogo);
            }

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                middleCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                middleCell.BorderWidthBottom = 1f;
                headerTable.AddCell(middleCell);
            }

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Leave Report - " + filterDate.ToString(Constant.DateFormat) + " " + "To" + " " + toDate.ToString(Constant.DateFormat), titleFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.PaddingBottom = 20f;
                nested.AddCell(nextPostCell1);

                PdfPTable nesteds = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nestedss = new PdfPCell(new Phrase("", titleFont));
                nestedss.Border = Rectangle.NO_BORDER;
                nestedss.PaddingBottom = 20f;
                nesteds.AddCell(nestedss);

                nested.AddCell(" ");
                PdfPCell nestedHousing = new PdfPCell(nested);
                nestedHousing.Border = Rectangle.NO_BORDER;
                nestedHousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                nestedHousing.BorderWidthBottom = 1f;
                nestedHousing.Rowspan = 6;
                nestedHousing.PaddingTop = 10f;
                headerTable.AddCell(nestedHousing);

                PdfPCell nestedHousings = new PdfPCell(nested);
                nestedHousings.Border = Rectangle.NO_BORDER;
                nestedHousings.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                nestedHousings.BorderWidthBottom = 1f;
                nestedHousings.Rowspan = 6;
                nestedHousings.PaddingTop = 10f;
                headerTable.AddCell(nestedHousings);
            }

            PdfPTable InvoiceTable = new PdfPTable(3);
            InvoiceTable.HorizontalAlignment = 0;
            InvoiceTable.WidthPercentage = 100;
            InvoiceTable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
            InvoiceTable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                InvoiceTable.AddCell(middleCell);
            }

            {
                PdfPTable title = new PdfPTable(1);
                title.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell();
                nextPostCell1.Border = Rectangle.NO_BORDER;
                title.AddCell(nextPostCell1);
                title.AddCell("");
                PdfPCell nestedHousings = new PdfPCell(title);
                nestedHousings.Border = Rectangle.NO_BORDER;
                nestedHousings.Rowspan = 5;
                nestedHousings.PaddingBottom = 30f;
                nestedHousings.HorizontalAlignment = Element.ALIGN_CENTER;
                title.HorizontalAlignment = Element.ALIGN_CENTER;
                InvoiceTable.AddCell(nestedHousings);
            }

            {
                PdfPCell middleCell = new PdfPCell();
                middleCell.Border = Rectangle.NO_BORDER;
                middleCell.PaddingTop = 20f;
                InvoiceTable.AddCell(middleCell);
            }

            pdfDoc.Add(headerTable);
            pdfDoc.Add(InvoiceTable);

            //Create body table
            PdfPTable tableLayout = new PdfPTable(7);
            float[] headers = { 40, 50, 46, 33, 36, 36, 36, }; //Header Widths  
            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 0;

            //Add header
            AddCellToHeader(tableLayout, Constant.EmployeeUserId);
            AddCellToHeader(tableLayout, Constant.UserName);
            AddCellToHeader(tableLayout, Constant.LeaveType);
            AddCellToHeader(tableLayout, Constant.LeaveFromDate);
            AddCellToHeader(tableLayout, Constant.LeaveToDate);
            AddCellToHeader(tableLayout, Constant.Reason);
            AddCellToHeader(tableLayout, Constant.LeaveCount);

            //Add body  
            foreach (var emp in leaveFilters)
            {
                AddCellToBody(tableLayout, emp.EmployeeUserName);
                AddCellToBody(tableLayout, emp.EmployeeName);
                AddCellToBody(tableLayout, emp.LeaveTypes);
                AddCellToBody(tableLayout, Convert.ToString(emp.LeaveFromDate.ToString(Constant.DateFormat)));
                AddCellToBody(tableLayout, Convert.ToString(emp.LeaveToDate.ToString(Constant.DateFormat)));
                AddCellToBody(tableLayout, emp.Reason);
                AddCellToBody(tableLayout, Convert.ToString(emp.LeaveCount));
            }
            pdfDoc.Add(tableLayout);

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
    }
}