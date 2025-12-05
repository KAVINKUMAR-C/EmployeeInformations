using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;

namespace EmployeeInformations.Common.Helpers
{
   
    public class EmailBodyContent
    {       
        public static string MailHeader()
        {
           
            return "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
          "<head><meta charset='utf-8'><meta name='viewport' content='width=device-width'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='x-apple-disable-message-reformatting'>" +
          "<title></title><link href='https://fonts.googleapis.com/css?family=Roboto:400,600' rel='stylesheet' type='text/css'>" +
          "<style>html,body {margin: 0 auto !important;padding: 0 !important;height: 100% !important;width: 100% !important;font-family: 'Roboto', sans-serif !important;font-size: 14px;" +
          "margin-bottom: 10px;line-height: 24px;color:#8094ae;font-weight: 400;}* {-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;margin: 0;padding: 0;}table,td {mso-table-lspace: 0pt !important;mso-table-rspace: 0pt !important;}" +
          "table {border-spacing: 0 !important;border-collapse: collapse !important;table-layout: fixed !important;margin: 0 auto !important;}table table table {table-layout: auto;}" +
          "a {text-decoration: none;}img {-ms-interpolation-mode:bicubic;}</style></head>" +
          "<body width='100%' style='margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f5f6fa;'><center style='width: 100%; background-color: #f5f6fa;'>" +
          "<table role='presentation' bgcolor='#f5f6fa' style='width:100%; border-collapse:collapse; border:0; border-spacing:0; background:#f5f6fa'><tbody><tr><td align='center' height='80' style='padding:20px;margin-top:50px;'>" +
          "<img src= 'https://portal.vphospital.com/images/logo.png' alt='' width='200' style='height:50px; display:block'> </td></tr><tr><td align='center' style='padding:0'>" +
          "<table role='presentation' bgcolor='#ffffff' style='background:#ffffff; width:602px; border-collapse:collapse; border:1px solid #e7e7e7; border-spacing:0; text-align:left'><tbody> <tr> <td style='padding: 30px 30px 20px'>";
        }

        public static string MailFooter()
        {
            return "---- <br> Thanks<br>Scope Thinkers </p> </td> </tr></tbody></table> <table style='width:620px;margin:auto'><tbody><tr><td style='margin-bottom: 15px;text-align:center;padding:25px 20px 0'><p style='font-size:13px;margin-bottom: 15px;'>Copyright © " + DateTime.Now.Year + " VpHospital. All rights reserved.</p></td></tr></tbody></table> </center></body></html>";
        }

        public static string WelcomeEmployeeEmailBodyContent(EmployeesEntity employees, string randomPassword, string domainName, string infoEmailName, string reportingPersion, string bodyContent)
        {
            var body = MailHeader() + bodyContent
            .Replace("%Username%", employees.FirstName + " " + employees.LastName)
            .Replace("%InfoEmailName%", infoEmailName)
            .Replace("%OfficeEmail%", employees.OfficeEmail)
            .Replace("%RandomPassword%", randomPassword)
            .Replace("%ReportingPersion%", reportingPersion)
            .Replace("%DomainName%", domainName) + MailFooter();
            //.Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_ForgotPassword(string encriptUserName, string domainName, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom: 10px;'>To ensure your security and convenience, please update your password using the link provided below.</p> <p style='margin-bottom: 15px;'><strong>URL :</strong> <a href='" + domainName + "Login/NewPassword?UserName=" + encriptUserName + "'>Link to Change Password</a></p> <p style='margin-top: 35px; margin-bottom: 15px;'>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%domainName%", domainName)
                .Replace("%encriptUserName%", encriptUserName) + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_ChangePassword(string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom: 10px;'> Your Password Successfully Changed.</p> "+ MailFooter();
            //return body;
            var body = MailHeader() + bodyContent + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }
        public static string SendEmail_Body_WebsiteContactUs(WebsiteContactUsEntity model)
        {
            var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'><strong>Hi Team</strong>,</p>" + "<p style='margin-bottom: 10px;'>Thank you for contacting us.</p>" + "<p style='margin-bottom: 15px;'><strong>Name :</strong> " + model.ContactName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Email :</strong> " + model.ContactEmail + "</p>" + "<p style='margin-bottom: 15px;'><strong> Mobile No :</strong> " + model.ContactPhoneNumber + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Website :</strong> " + model.ContactWebsiteName + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Description :</strong> " + model.ContactDescription + " </p>" + MailFooter();
            return body;
        }

        public static string SendEmail_Body_WebsiteJobPostRequest(string fullName, string email, string experience)
        {
            var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'><strong>Hi Team</strong>,</p>" + "<p style='margin-bottom: 15px;'><strong>Name :</strong> " + fullName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Email :</strong> " + email + "</p>" + "<p style='margin-bottom: 15px;'><strong> Experience :</strong> " + experience + "</p>" + MailFooter();
            return body;
        }

        public static string SendEmail_Body_WebsiteInsertQuoteRequest(WebsiteQuoteEntity websiteQuoteEntity, string serviceTypeName)
        {
            var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'><strong>Hi Team</strong>,</p>" + "<p style='margin-bottom: 15px;'><strong>Name :</strong>  " + websiteQuoteEntity.FirstName + " " + websiteQuoteEntity.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Email :</strong> " + websiteQuoteEntity.Email + "</p>" + "<p style='margin-bottom: 15px;'><strong> Refe No :</strong> " + websiteQuoteEntity.ERFN + "</p>" + "<p style='margin-bottom: 15px;'><strong> Service Type :</strong> " + serviceTypeName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Comment :</strong> " + websiteQuoteEntity.Comment + "</p>" + MailFooter();
            return body;
        }

        public static string SendEmail_Body_WebsiteInsertQuote_SenderRequest(string enquiryUserName, string eRFNumber)
        {
            var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'><p style='margin-bottom:10px;'><strong>Hello" + enquiryUserName + " </strong>,</p><p style='margin-bottom: 10px;'> I appreciate your interaction. Our support team will be in with you shortly to resolve the issue.</p>" + "<p style='margin-bottom: 10px;'>We are thankful for your patience!</p>" + "<p style='margin-bottom: 15px;'><strong>Your Ref No :</strong> " + eRFNumber + "</p>" + MailFooter();
            return body;
        }

        public static string SendEmail_Body_CreateLeave(EmployeesEntity employees, string leaveFromDate, string leaveToDate, decimal leaveDaysCount, string leavetype, string strDay, string reason, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'><p style='margin-bottom:10px;'><strong>Dear Management Team</strong>,</p> <p style='margin-bottom: 10px;'>" + employees.FirstName + " " + employees.LastName + " has requested a leave and is awaiting your response.</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Number of Leave :</strong> " + leaveDaysCount + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%EmployeeId%", employees.UserName)
                .Replace("%leavetype%", leavetype)
                .Replace("%Leavedate%", leaveFromDate + " " + "To" + " " + leaveToDate)
                .Replace("%LeaveDaysCount%", leaveDaysCount.ToString())
                .Replace("%Reason%", reason) + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_CreateCompensatoryOffRequest(EmployeesEntity employees, string workedDate, string leavetype, string remark, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%EmployeeId%", employees.UserName)
                .Replace("%Leavetype%", leavetype)
                .Replace("%Workeddate%", workedDate)
                .Replace("%Remark%", remark) + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_ApprovedLeave(EmployeesEntity employees, string leaveFromDate, string leaveToDate, string reason, string approveReason, string leavetype, string name, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'><strong>Dear " + employees.FirstName + " " + employees.LastName + "</strong>,</p>" + "<p style='margin-bottom: 10px;'>We would like to inform you that your leave request has been approved.</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Status :</strong> " + "<b> APPROVED ! </b>" + " </p>" + "<p style='margin-bottom: 10px;'>Relax and Restart!</p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Leavetype%", leavetype)
                .Replace("%Leavedate%", leaveFromDate + " " + "To" + " " + leaveToDate)
                .Replace("%Reason%", reason)
                .Replace("%Reportingperson%", name)
                .Replace("%Approvereason%", approveReason) + MailFooter();

            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_RejectLeave(EmployeesEntity employees, string leaveFromDate, string leaveToDate, string reason, string leavetype, string rejectReason, string name, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'><p style='margin-bottom:10px;'><strong>We apologise</strong>,</p> <p style='margin-bottom:10px;'><strong>Dear " + employees.FirstName + " " + employees.LastName + "</strong>,</p> <p style='margin-bottom: 10px;'>I considered your request for a leave of absence. you are aware that we are working hard to reach our deadline goals. As well as your request, we value your presence and work very much. This made make a decision somewhat difficult. I have consulted some peers in management prior to deciding.<p style='margin-bottom: 10px;'>I regret to inform you that I cannot approve your leave of absence at this time. This project really benefits from your participation.Regarding my response, I sincerely appreciate your patience.</p><p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Kindly Note :</strong> " + rejectReason + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Status :</strong> " + "<b> NOT APPROVED ! </b> for this time. I hope, you take in good." + " </p>" + "<p style='margin-bottom: 10px;'>Relax and Restart!</p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Leavetype%", leavetype)
                .Replace("%Leavedate%", leaveFromDate + " " + "To" + " " + leaveToDate)
                .Replace("%Reason%", reason)
                .Replace("%Reportingperson%", name)
                .Replace("%Rejectreason%", rejectReason) + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());

            return body;
        }

        public static string SendEmail_Body_RequestProbation(EmployeesEntity employees, string designation, string department, string currentDate, string dateOfJoing, string bodyContent)
        {
            // var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'>Verify and validate the end of the probationary period.</p> <p style='margin-bottom: 10px;'>For " + employees.FirstName + " " + employees.LastName + "<p style='margin-bottom: 15px;'><strong>Employee Details :</strong></p> <p style='margin-bottom: 15px;'><strong>Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Department :</strong> " + department + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Designation :</strong> " + designation + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Joining Date :</strong> " + dateOfJoing + " </p>" + MailFooter();
            // return body;
            var body = MailHeader() + bodyContent
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%Designation%", designation)
                .Replace("%Department%", department)
                .Replace("%DateOfJoing%", dateOfJoing) + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_RequestProbation(string firstName, string lastName, string userName, string designation, string department, string currentDate, string dateOfJoing, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%Employeeid%", userName)
                .Replace("%Username%", firstName + " " + lastName)
                .Replace("%Designation%", designation)
                .Replace("%Department%", department)
                .Replace("%DateOfJoing%", dateOfJoing) + MailFooter();

            return body;
        }

        public static string SendEmail_Body_RequestBithday(EmployeesEntity employees, string designation, string department, string dateOfBirth, int gender, string bodyContent)
        {
            var result = string.Empty;

            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%dateOfBirth%", dateOfBirth)
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Designation%", designation)
                .Replace("%Department%", department)
                + MailFooter();
            if ((int)Gender.Male == gender)
            {
                result = body.Replace("Mr. / Ms.", "Mr.").Replace("his / her", "his");
            }
            else
            {
                result = body.Replace("Mr. / Ms.", "Ms.").Replace("his / her", "her");
            }
            return result;
        }

        public static string SendEmail_Body_RequestBithday(string firstName, string lastName, string userName, string designation, string department, string dateOfBirth, int gender, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%Username%", firstName + " " + lastName)
                .Replace("%dateOfBirth%", dateOfBirth)
                .Replace("%Employeeid%", userName)
                .Replace("%Designation%", designation)
                .Replace("%Department%", department)
                + MailFooter();
            string? result;
            if ((int)Gender.Male == gender)
            {
                result = body.Replace("Mr. / Ms.", "Mr.").Replace("his / her", "his");
            }
            else
            {
                result = body.Replace("Mr. / Ms.", "Ms.").Replace("his / her", "her");
            }
            return result;
        }


        public static string SendEmail_Body_WorkAnniversary(EmployeesEntity employees, string designation, string department, string dateOfJoing, int gender, int year, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'><strong>A Good Remind</strong>,</p> <p style='margin-bottom: 10px;'>Our employee, Mr. / Ms. " + employees.FirstName + " " + employees.LastName + " is celebrating his / her birthday on " + dateOfBirth + ".<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Department :</strong> " + department + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Designation :</strong> " + designation + "</p>" + MailFooter();
            //return body;
            var result = string.Empty;
            var strYear = "";
            if (year > 1)
            {
                strYear = "years";
            }
            else
            {
                strYear = "year";
            }
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%Year%", Convert.ToString(year))
                .Replace("%StrYear%", strYear)
                .Replace("%DateOfJoing%", dateOfJoing)
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Designation%", designation)
                .Replace("%Department%", department)
                + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());            
            if ((int)Gender.Male == gender)
            {
                result = body.Replace("Mr. / Ms.", "Mr.").Replace("him / her", "him").Replace("his / her", "his");
            }
            else
            {
                result = body.Replace("Mr. / Ms.", "Ms.").Replace("him / her", "her").Replace("his / her", "her");
            }
            return result;
        }

        public static string SendEmail_Body_WorkAnniversary(string firstName, string lastName, string userName, string designation, string department, string dateOfJoing, int gender, int year, string bodyContent)
        {
            var strYear = year > 1 ? "years" : "year";

            var body = MailHeader() + bodyContent
                .Replace("%Username%", firstName + " " + lastName)
                .Replace("%Year%", Convert.ToString(year))
                .Replace("%StrYear%", strYear)
                .Replace("%DateOfJoing%", dateOfJoing)
                .Replace("%Employeeid%", userName)
                .Replace("%Designation%", designation)
                .Replace("%Department%", department)
                + MailFooter();
            
            string? result;            
            if ((int)Gender.Male == gender)
            {
                result = body.Replace("Mr. / Ms.", "Mr.").Replace("him / her", "him").Replace("his / her", "his");
            }
            else
            {
                result = body.Replace("Mr. / Ms.", "Ms.").Replace("him / her", "her").Replace("his / her", "her");
            }
            return result;
        }


        public static string SendEmail_Body_ProbationConfirmation(EmployeesEntity employees, string designation, string department, int probationPeriod, string acceptedBy, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom: 10px;'><strong>Dear " + employees.FirstName + " " + employees.LastName + "</strong>,</p>" + "<p style='margin-bottom: 10px;'>Following completion of your " + probationPeriod + "-month probation period at Scope Thinkers, we have reviewed your performance and found it to be satisfactory.</p>" + "<p style='margin-bottom: 10px;'>Given the foregoing, we are happy to let you know that the following information about you has been confirmed:</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" +
            //    "<p style='margin-bottom: 15px;'><strong>Department :</strong> " + department + "</p>" +
            //    "<p style='margin-bottom: 15px;'><strong>Designation :</strong> " + designation + "</p> " +
            //    "<p style='margin-bottom: 10px;'>We are overjoyed to welcome you into our journey!</p>" +
            //    MailFooter();

            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%EmployeeId%", employees.UserName)
                .Replace("%ProbationPeriod%", probationPeriod.ToString())
                .Replace("%Designation%", designation)
                .Replace("%Department%", department) 
                .Replace("%UserName%", acceptedBy) + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_MailCheck()
        {
            var body = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>" + "<head><meta charset='utf-8'><meta name='viewport' content='width=device-width'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='x-apple-disable-message-reformatting'>" + "<title></title><link href='https://fonts.googleapis.com/css?family=Roboto:400,600' rel='stylesheet' type='text/css'>" + "<style>html,body {margin: 0 auto !important;padding: 0 !important;height: 100% !important;width: 100% !important;font-family: 'Roboto', sans-serif !important;font-size: 14px;" + "margin-bottom: 10px;line-height: 24px;color:#8094ae;font-weight: 400;}* {-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;margin: 0;padding: 0;}table,td {mso-table-lspace: 0pt !important;mso-table-rspace: 0pt !important;}" + "table {border-spacing: 0 !important;border-collapse: collapse !important;table-layout: fixed !important;margin: 0 auto !important;}table table table {table-layout: auto;}" + "a {text-decoration: none;}img {-ms-interpolation-mode:bicubic;}</style></head>" + "<body width='100%' style='margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f5f6fa;'><center style='width: 100%; background-color: #f5f6fa;'><table width='100%' border='0' cellpadding='0' cellspacing='0' bgcolor='#f5f6fa'>" + "<tr><td style='padding: 40px 0;'><table style='width:620px;margin:auto;'><tbody><tr><td style='text-align: center; padding-bottom:25px'><a href='#'><img style='height: 40px' src='images/logo-dark2x.png' alt='logo'></a>" + "<p style='font-size: 14px; color: #6576ff; padding-top: 12px;'>Conceptual Base Modern Dashboard Theme</p></td></tr></tbody></table><table style='width:620px;margin:auto;background-color:#ffffff;'>" + "<tbody><tr><td style='padding: 30px 30px 20px'><p style='margin-bottom: 10px;'>Hi Ishtiyak,</p><p style='margin-bottom: 10px;'>We are pleased to have you as a member of TokenWiz Family.</p><p style='margin-bottom: 10px;'>Your account is now verified and you can purchase tokens for the ICO. Also you can submit your documents for the KYC from my Account page.</p><p style='margin-bottom: 15px;'>Hope you'll enjoy the experience, we're here if you have any questions, drop us a line at <a style='color: #6576ff; text-decoration:none;' href='mailto:info@yourwebsite.com'>info@yourwebsite.com</a> anytime. </p>" + "</td></tr></tbody></table><table style='width:620px;margin:auto;'><tbody><tr><td style='text-align: center; padding:25px 20px 0;'><p style='font-size: 13px;'>Copyright © 2020 DashLite. All rights reserved. <br> Template Made By <a style='color: #6576ff; text-decoration:none;' href='https://themeforest.net/user/softnio/portfolio'>Softnio</a>.</p>" + "<ul style='margin: 10px -4px 0;padding: 0;'><li style='display: inline-block; list-style: none; padding: 4px;'><a style='display: inline-block; height: 30px; width:30px;border-radius: 50%; background-color: #ffffff' href='#'><img style='width: 30px' src='images/brand-b.png' alt='brand'></a></li><li style='display: inline-block; list-style: none; padding: 4px;'><a style='display: inline-block; height: 30px; width:30px;border-radius: 50%; background-color: #ffffff' href='#'><img style='width: 30px' src='images/brand-e.png' alt='brand'></a></li>" + "<li style='display: inline-block; list-style: none; padding: 4px;'><a style='display: inline-block; height: 30px; width:30px;border-radius: 50%; background-color: #ffffff' href='#'><img style='width: 30px' src='images/brand-d.png' alt='brand'></a></li><li style='display: inline-block; list-style: none; padding: 4px;'><a style='display: inline-block; height: 30px; width:30px;border-radius: 50%; background-color: #ffffff' href='#'><img style='width: 30px' src='images/brand-c.png' alt='brand'></a></li>" + "</ul><p style='padding-top: 15px; font-size: 12px;'>This email was sent to you as a registered user of <a style='color: #6576ff; text-decoration:none;' href='https://softnio.com'>softnio.com</a>. To update your emails preferences <a style='color: #6576ff; text-decoration:none;' href='#'>click here</a>.</p></td></tr></tbody></table></td></tr></table>    </center></body></html>";
            return body;
        }

        public static string SendEmail_Body_MailCheck1()
        {
            var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom: 10px;'><strong>Dear User</strong>,</p> <p style='margin-bottom: 10px;'>We are excited to have you aboard!</p> <p style='margin-bottom: 10px;'>At Scope Thinkers, we are dedicated to providing our employees with the resources they need to succeed. We have planned your first days to help you settle in properly. The agenda provided is further information. As you can see, there will be plenty of time for you to read and submit your employment documentation (HR will be available to assist you during this process!).</p> <p style='margin-bottom: 10px;'>We are glad to send you a password and employee ID.</p> <p style='margin-bottom: 10px;'>User logs in and follow the instructions to complete the form with the appropriate documentation.</p> <p style='margin-bottom: 15px;'>If any queries and help, simply contact out support team (Mail ID).</p> <p style='margin-bottom: 15px;'><strong>Username :</strong> Management096</p> <p style='margin-bottom: 15px;'><strong>password :</strong> Pkbrz50uau</p> <p style='margin-bottom: 15px;'><strong>URL :</strong> <a href='#'>Link to login</a></p> <p style='margin-top: 35px; margin-bottom: 15px;'>";
            return body;
        }

        public static string SendEmail_Body_ProjectAssignation(EmployeesEntity employees, string projectName, string projectTypeName, DateTime projectStartDate, string projectManagerName, string projectAssignedBy, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'><p style='margin-bottom:10px;'><strong>Dear Management Team</strong>,</p> <p style='margin-bottom: 10px;'>" + employees.FirstName + " " + employees.LastName + " has requested a leave and is awaiting your response.</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Number of Leave :</strong> " + leaveDaysCount + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%ProjectName%", projectName)
                .Replace("%ProjectType%", projectTypeName)
                .Replace("%ProjectStartDate%", projectStartDate.ToString(Constant.DateFormat))
                .Replace("%ProjectManagerName%", projectManagerName)
                .Replace("%ProjectAssignee%", projectAssignedBy)
                + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_ProjectRemoveEmployee(EmployeesEntity employees, string projectName, string projectTypeName, DateTime projectStartDate, string projectManagerName, string projectRejectedBy, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'><p style='margin-bottom:10px;'><strong>Dear Management Team</strong>,</p> <p style='margin-bottom: 10px;'>" + employees.FirstName + " " + employees.LastName + " has requested a leave and is awaiting your response.</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Number of Leave :</strong> " + leaveDaysCount + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%ProjectName%", projectName)
                .Replace("%ProjectType%", projectTypeName)
                .Replace("%ProjectStartDate%", projectStartDate.ToString(Constant.DateFormat))
                .Replace("%ProjectManagerName%", projectManagerName)
                .Replace("%ProjectRejectedBy%", projectRejectedBy)
                + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_JobPostStatus(string jobName, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'><p style='margin-bottom:10px;'><strong>Dear Management Team</strong>,</p> <p style='margin-bottom: 10px;'>" + employees.FirstName + " " + employees.LastName + " has requested a leave and is awaiting your response.</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Number of Leave :</strong> " + leaveDaysCount + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
               .Replace("%JobName%", jobName)
                + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_Attendance(List<AttendanceListDataModel> attendanceListDataModels, string bodyContent)
        {
            var body = "";

            foreach (var attendance in attendanceListDataModels)
            {
                body = MailHeader() + bodyContent

               .Replace("%Username%", attendance.EmployeeName)
                   .Replace("%EmployeeId%", attendance.UserName)
                   .Replace("%Date%", attendance.Date)
                   .Replace("%TotalHours%", attendance.TotalHours)
                   .Replace("%BreakHours%", attendance.BreakHours)
                   .Replace("%InsideOffice%", attendance.InsideOffice)
                   .Replace("%EntryTime%", attendance.EntryTime)
                   .Replace("%ExitTime%", attendance.ExitTime)
                   .Replace("%BurningHours%", attendance.BurningHours) + MailFooter();
                if (attendance.TotalSecounds < 25200)
                {
                    body = body.Replace("%color_name%", "red").Replace("%textName%", "You haven't maintained your work hours properly.Please maintain your work hours as per the company policy.");
                }
                else if (attendance.TotalSecounds < 28800)
                {
                    body = body.Replace("%color_name%", "orange").Replace("%textName%", "Please maintain your work hours as per the company policy.");
                }
                else
                {
                    body = body.Replace("%color_name%", "green").Replace("%textName%", "Thanks for your effort."); ;
                }
            }

            return body;
        }

        public static string SendEmail_Body_AttendanceAll(AttendanceListDataModel attendanceListDataModels, string bodyContent)
        {
            var body = "";
            var colour = "";

            body = MailHeader() + bodyContent

           .Replace("%Username%", attendanceListDataModels.EmployeeName)
               .Replace("%EmployeeId%", attendanceListDataModels.UserName)
               .Replace("%Date%", attendanceListDataModels.Date)
               .Replace("%TotalHours%", attendanceListDataModels.TotalHours)
               .Replace("%BreakHours%", attendanceListDataModels.BreakHours)
               .Replace("%InsideOffice%", attendanceListDataModels.InsideOffice)
               .Replace("%EntryTime%", attendanceListDataModels.EntryTime)
               .Replace("%ExitTime%", attendanceListDataModels.ExitTime)
               .Replace("%BurningHours%", attendanceListDataModels.BurningHours) + MailFooter();
            if (attendanceListDataModels.TotalSecounds < 25200)
            {
                colour = "red";
                body = body.Replace("%color_name%", colour).Replace("%textName%", "You haven't maintained your work hours properly.Please maintain your work hours as per the company policy.");
            }
            else if (attendanceListDataModels.TotalSecounds < 28800)
            {
                colour = "orange";
                body = body.Replace("%color_name%", colour).Replace("%textName%", "Please maintain your work hours as per the company policy.");
            }
            else
            {
                colour = "green";
                body = body.Replace("%color_name%", colour).Replace("%textName%", "Thanks for your effort."); ;
            }
            body.Replace("%color_name%", colour);
            return body;
        }

        public static string SendEmail_Body_AttendanceForManagement(string fromDate, string toDate, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%FromDate%", fromDate)
                .Replace("%ToDate%", toDate) + MailFooter();
            return body;
        }
        public static string SendEmail_Body_AttendanceForEmployeeWeekly(string fromDate, string toDate, string employeeName, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%EmployeeName%", employeeName)
                .Replace("%FromDate%", fromDate)
                .Replace("%ToDate%", toDate) + MailFooter();
            return body;
        }
        public static string SendEmail_Body_LeaveForEmployeeWeekly(string fromDate, string toDate, string employeeName, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%EmployeeName%", employeeName)
                .Replace("%FromDate%", fromDate)
                .Replace("%ToDate%", toDate) + MailFooter();
            return body;
        }

        public static string SendEmail_Body_AttendanceForEmployeeMonth(string employeeName, string month, string year, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%EmployeeName%", employeeName)
                .Replace("%Month%", month)
                .Replace("%Year%", year) + MailFooter();
            return body;
        }

        public static string SendEmail_Body_ErrorMessage(string ex, string baseUrl, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                //.Replace("%text%", baseUrl)
                .Replace("%Error%", ex) + MailFooter();
            return body;
        }

        public static string SendEmail_Body_ApprovedCompensatoryOffRequest(EmployeesEntity employees, string workedDate, string remark, string approveReason, string leavetype, string name, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'><strong>Dear " + employees.FirstName + " " + employees.LastName + "</strong>,</p>" + "<p style='margin-bottom: 10px;'>We would like to inform you that your leave request has been approved.</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Status :</strong> " + "<b> APPROVED ! </b>" + " </p>" + "<p style='margin-bottom: 10px;'>Relax and Restart!</p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Leavetype%", leavetype)
                .Replace("%Workeddate%", workedDate)
                .Replace("%Remark%", remark)
                .Replace("%Reportingperson%", name)
                .Replace("%Approvereason%", approveReason) + MailFooter();

            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_RejectCompensatoryOffRequest(EmployeesEntity employees, string workedDate, string remark, string rejectReason, string leavetype, string name, string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'><p style='margin-bottom:10px;'><strong>We apologise</strong>,</p> <p style='margin-bottom:10px;'><strong>Dear " + employees.FirstName + " " + employees.LastName + "</strong>,</p> <p style='margin-bottom: 10px;'>I considered your request for a leave of absence. you are aware that we are working hard to reach our deadline goals. As well as your request, we value your presence and work very much. This made make a decision somewhat difficult. I have consulted some peers in management prior to deciding.<p style='margin-bottom: 10px;'>I regret to inform you that I cannot approve your leave of absence at this time. This project really benefits from your participation.Regarding my response, I sincerely appreciate your patience.</p><p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Kindly Note :</strong> " + rejectReason + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Status :</strong> " + "<b> NOT APPROVED ! </b> for this time. I hope, you take in good." + " </p>" + "<p style='margin-bottom: 10px;'>Relax and Restart!</p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%Username%", employees.FirstName + " " + employees.LastName)
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Leavetype%", leavetype)
                .Replace("%Workeddate%", workedDate)
                .Replace("%Remark%", remark)
                .Replace("%Reportingperson%", name)
                .Replace("%Rejectreason%", rejectReason) + MailFooter();
            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());

            return body;
        }

        public static string SendEmail_Body_ApplyExpense(ExpensesEntity expense,string draftBody,string firstName , string lastName,string userName,string fromDate ,string todate ,string totalAmount,string status , string empName, string count)
        {
            var body = MailHeader() + draftBody
                .Replace("%Username%", firstName +" " + lastName)
                .Replace("%EmployeeId%",userName)
                .Replace("%Expensetitle%",expense.ExpenseTitle)
                .Replace("%Fromdate%", fromDate)
                .Replace("%Todate%" ,todate)
                .Replace("%Noofattachment%",count)
                .Replace("%Totalamount%",totalAmount)
                .Replace("%EmpName%",empName)
                .Replace("%Status%",status) + MailFooter();
            return body;
        }

        public static string SendEmail_Body_Announcement(string announcement, string description, string strAnnouncementDate, string strAnnouncementEndDate,  string announcedBy ,string bodyContent)
        {
            //var body = MailHeader() + "<tr> <td style='padding: 30px 30px 20px'> <p style='margin-bottom:10px;'><strong>Dear " + employees.FirstName + " " + employees.LastName + "</strong>,</p>" + "<p style='margin-bottom: 10px;'>We would like to inform you that your leave request has been approved.</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Id :</strong> " + employees.UserName + "</p>" + "<p style='margin-bottom: 15px;'><strong>Employee Name :</strong> " + employees.FirstName + " " + employees.LastName + "</p>" + "<p style='margin-bottom: 15px;'><strong> Leave Type :</strong> " + leavetype + "</p>" + "<p style = 'margin-bottom: 15px;'><strong> Leave Date :</strong> " + leaveFromDate + " " + "To" + " " + leaveToDate + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Reason :</strong> " + reason + " </p>" + "<p style = 'margin-bottom: 15px;'><strong> Status :</strong> " + "<b> APPROVED ! </b>" + " </p>" + "<p style='margin-bottom: 10px;'>Relax and Restart!</p>" + MailFooter();
            //return body;
            var body = MailHeader() + bodyContent
                .Replace("%AnnouncementName%", announcement)
                .Replace ("%Description%", description)
                .Replace("%AnnouncementDate%", strAnnouncementDate)                
                .Replace("%AnnouncementEndDate%", strAnnouncementEndDate)
                .Replace("%Username%", announcedBy) + MailFooter();

            // .Replace("%CopyRightYear%", DateTime.Now.Year.ToString());
            return body;
        }

        public static string SendEmail_Body_HelpdeskTicket(EmployeesEntity employees, string bodyContent)
        {
            var body = MailHeader() + bodyContent
                .Replace("%Employeeid%", employees.UserName)
                .Replace("%Username%", employees.FirstName + " " + employees.LastName) + MailFooter();
            return body;
        }

        public static string Website_CandidateMenu(string FullName, string Email,  string CandidateScheduleName, string StartTime, string EndTime, string date, string employeesName, string meetingLink, string bodyContent)

        {
            var body = MailHeader() + bodyContent
            .Replace("%Candidatename%", FullName)
            .Replace("%Email%", Email)                       
            .Replace("%Status%", CandidateScheduleName)
            .Replace("%StartTime%", StartTime)
            .Replace("%EndTime%", EndTime)
            .Replace("%Date%", date)
            .Replace("%EmployeeName%", employeesName)
            .Replace("%MeetingLink%", meetingLink)
           + MailFooter();            
            return body;
        }

        public static string Website_CandidateMenuStatus(string FullName, string Email, string CandidateScheduleName, string CandidateStatusName, string bodyContent)
        {
            var body = MailHeader() + bodyContent
            .Replace("%Candidatename%", FullName)
            .Replace("%Email%", Email)
            .Replace("%Status%", CandidateScheduleName)
            .Replace("%CandidateStatus%", CandidateStatusName)
           + MailFooter();
            return body;
        }
    }

}
