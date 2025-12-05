using EmployeeInformations.Model.TeamsViewModel;
using Microsoft.AspNetCore.Mvc;
using EmployeeInformations.Business.IService;
using Newtonsoft.Json;
using System.Text;
using EmployeeInformations.Model.PagerViewModel;
using System.Text.Json;
using EmployeeInformations.Common.Helpers;

namespace EmployeeInformations.Controllers
{
    public class TeamsController : BaseController
    {
        private readonly ITeamsMeetingService _teamsMeetingService;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _config;

        public TeamsController(  IHostEnvironment environment, ITeamsMeetingService teamsMeetingService, IConfiguration configuration)
        {                     
            _environment = environment;
            _teamsMeetingService = teamsMeetingService; 
            _config = configuration;           
        }

        //Teams
        [HttpGet]
        public async Task<IActionResult>TeamMeeting(string code)
        {            
            var teams = new Teams();
            teams.Code = code;
            return View(teams);
        }

        /// <summary>
        /// Logic to get all the teams meeting Filtered data and count 
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        [HttpGet]
        public async Task<IActionResult> GetTeamsMeeting(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var empId = Convert.ToInt32(sessionRoleId == 1 || sessionRoleId == 2 ? 0 : sessionEmployeeId);
            var teamMeetingFilter = await _teamsMeetingService.GetTeamMeetingEmployeesList( pager, empId, columnDirection, columnName, companyId);
            var teamMeetingFilterCount = await _teamsMeetingService.GetAllTeamMeetingByFilterCount(empId, pager, companyId);
            return Json(new
            {
                iTotalRecords = teamMeetingFilterCount,
                iTotalDisplayRecords = teamMeetingFilterCount,
                data = teamMeetingFilter.TeamMeetingModels,
            });            
        }

        /// <summary>
        /// Logic to get the Teams  create page
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> Teams(string code)
        {                       
            var teams = new Teams();
            teams.Code = code;           
            return View(teams);
        }


        /// <summary>
        ///  Logic to get create the teams details  
        /// </summary>
        /// <param name="teams" ></param>
        [HttpPost]
        public async Task<int> CreateTeamsMeetinges(Teams teams)
        {                                  
            var timeZone = Convert.ToString(_config.GetSection("AzureAD").GetSection("Timezone").Value);
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var employeeOfficeMail = await _teamsMeetingService.GetEmployeeOfficeMail(sessionEmployeeId, companyId);           
            DateTime startTime = Convert.ToDateTime(teams.StrStartdate + " " + teams.MeetingStartTime);
            DateTime endTime = Convert.ToDateTime(teams.StrStartdate + " " + teams.MeetingEndTime);
            var client = new HttpClient();
            string accessToken = "";
            if (GetSessionValueForAccessToken == "")
            {
                accessToken = await GetAccessToken(teams);
            }
            else
            {
                accessToken = GetSessionValueForAccessToken;
            }
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://graph.microsoft.com/beta/users/{employeeOfficeMail}/events");
             request.Headers.Add("Authorization", $"Bearer {accessToken}");
            GetSessionValueForAccessToken = $"Bearer {accessToken}";
            var meeting = new MeetingRequest
            {
                TransactionId = Guid.NewGuid(),
                subject = teams.MeetingName,
                allowNewTimeProposals = true,
                location = new Location()
                {
                    displayName = "online meeting"
                },
                body = new MeetingBody()
                {
                    contentType = "HTML",
                    content = teams.MeetingName
                },
                start = new MeetingTime()
                {
                    dateTime = startTime,
                    timeZone = timeZone
                },
                end = new MeetingTime()
                {
                    dateTime = endTime,
                    timeZone = timeZone
                }
            };

            var Mails = teams.AttendeeEmail.Split(";");
            meeting.attendees = new List<Attendee>();
           
                var attendees = new Attendee();
                foreach (var Email in Mails)
                {                   
                    var emailAddress = new EmailAddress();
                    emailAddress.address = Email;
                    emailAddress.name = Email.Replace("@","");
                    attendees.emailAddress = emailAddress;
                    attendees.type = "required";
                    meeting.attendees.Add(attendees);
                }                
                                          
            var json = JsonConvert.SerializeObject(meeting);
            var contents = new StringContent(json, Encoding.UTF8, "application/json");            
            request.Content = contents;
            var response = await client.SendAsync(request);
            var datas = await response.Content.ReadAsStringAsync();
            using (JsonDocument document = JsonDocument.Parse(datas))
            {
                JsonElement root = document.RootElement;
                if (root.TryGetProperty("id", out JsonElement idElement))
                {
                    teams.MeetingId =  idElement.GetString();
                }
                else
                {
                    throw new Exception("ID property not found in JSON.");
                }
            }
            var result = await _teamsMeetingService.CreateTeamsMeeting(teams, sessionEmployeeId, companyId);        
            response.EnsureSuccessStatusCode();
            return result;

        }

        /// <summary>
        /// Logic to get edit the teams detail  by particular teams
        /// </summary>
        /// <param name="TeamsMeetingId" >teams</param>
        [HttpGet]
        public IActionResult EditTeams(int TeamsMeetingId,string code)
        {
            var teams = new Teams();
            teams.TeamsMeetingId = TeamsMeetingId;
            teams.Code = code;
            return PartialView("EditTeams", teams);
        }


        /// <summary>
        /// Logic to get update the teams detail  by particular teams
        /// </summary>
        /// <param name="TeamsMeetingId" >teams</param>
        [HttpGet]
        public async Task<IActionResult> UpdateTeams(int TeamsMeetingId, string code)
        {
            var companyId = GetSessionValueForCompanyId;
            var teams = await _teamsMeetingService.GetByTeamsMeetingId(TeamsMeetingId,companyId);
            teams.Code = code;
            return PartialView("UpdateTeams", teams);
        }

        [HttpPost]
        public async Task<int> UpdateTeamsMeetinges(Teams teams)
        {            
            var client = new HttpClient();            
            string accessToken = "";
            var companyId = GetSessionValueForCompanyId;
            if (GetSessionValueForAccessToken == "")
            {
                 accessToken = await GetAccessToken(teams);
            }
            else
            {
                accessToken = GetSessionValueForAccessToken;
            }           

            var timeZone = Convert.ToString(_config.GetSection("AzureAD").GetSection("Timezone").Value);           
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var employeeOfficeMail = await _teamsMeetingService.GetEmployeeOfficeMail(sessionEmployeeId, companyId);
            var request = new HttpRequestMessage(HttpMethod.Patch, $"https://graph.microsoft.com/beta/users/{employeeOfficeMail}/events/{teams.MeetingId}");
            request.Headers.Add("Authorization", $"Bearer {accessToken}" );
            DateTime startTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(teams.StrStartdate + " " + teams.MeetingStartTime);
            DateTime endTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(teams.StrStartdate + " " + teams.MeetingEndTime);

            var meeting = new MeetingRequest
            {
                subject = teams.MeetingName,
                allowNewTimeProposals = true,
                location = new Location()
                {
                    displayName = "online meeting"
                },
                body = new MeetingBody()
                {
                    contentType = "HTML",
                    content = teams.MeetingName
                },
                start = new MeetingTime()
                {
                    dateTime = startTime,
                    timeZone = timeZone
                },
                end = new MeetingTime()
                {
                    dateTime = endTime,
                    timeZone = timeZone
                }
            };

            var Mails = teams.AttendeeEmail.Split(";");
            meeting.attendees = new List<Attendee>();

            var attendees = new Attendee();
            foreach (var Email in Mails)
            {
                var emailAddress = new EmailAddress();
                emailAddress.address = Email;
                emailAddress.name = Email.Replace("@", "");
                attendees.emailAddress = emailAddress;
                attendees.type = "required";
                meeting.attendees.Add(attendees);
            }
           
            var json = JsonConvert.SerializeObject(meeting);
            var contents = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = contents;
             var response = await client.SendAsync(request);
            var datas = await response.Content.ReadAsStringAsync();
            var result  = await _teamsMeetingService.CreateTeamsMeeting(teams, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the teams detail  by particular TeamsMeetingId
        /// </summary>
        /// <param name="TeamsMeetingId" ></param>
        /// <param name="code" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteTeamsMeeting(int TeamsMeetingId, string code)
        {
            var companyId = GetSessionValueForCompanyId;
            var teams = new Teams();
            var teamEntity = await _teamsMeetingService.GetByTeamsMeetingId(TeamsMeetingId, companyId);
            teams.TeamsMeetingId = TeamsMeetingId;
            teams.Code = code;
            string accessToken = "";
            if (GetSessionValueForAccessToken == "")
            {
                accessToken = await GetAccessToken(teams);
            }
            else
            {
                accessToken = GetSessionValueForAccessToken;
            }           
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var employeeOfficeMail = await _teamsMeetingService.GetEmployeeOfficeMail(sessionEmployeeId, companyId);
            var timeZone = Convert.ToString(_config.GetSection("AzureAD").GetSection("Timezone").Value);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://graph.microsoft.com/beta/users/{employeeOfficeMail}/events/{teamEntity.MeetingId}");
            request.Headers.Add("Authorization", $"Bearer {accessToken}");           
            var client = new HttpClient();            
            var response = await client.SendAsync(request);
            var datas = await response.Content.ReadAsStringAsync();
            var result = await _teamsMeetingService.DeleteTeamsMeeting(TeamsMeetingId, companyId);
            return Json(result);
        }

        /// <summary>
        /// Logic to get the AccessToken teams detail
        /// </summary>
        /// <param name="teams" ></param>
        public async Task<string> GetAccessToken(Teams teams)
        {
            var clientId = Convert.ToString(_config.GetSection("AzureAD").GetSection("ClientId").Value);
            var ClientSecret = Convert.ToString(_config.GetSection("AzureAD").GetSection("ClientSecret").Value);
            var redirectUri = Convert.ToString(_config.GetSection("AzureAD").GetSection("RedirectUri").Value);
            var tenantId = Convert.ToString(_config.GetSection("AzureAD").GetSection("TenantId").Value);
            var tokenEndpoint = Convert.ToString(_config.GetSection("AzureAD").GetSection("TokenEndpoint").Value);
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("code", teams.Code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
            });

            request.Content = content;

            var response = await client.SendAsync(request);
            string accessToken = "";
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();               
                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                accessToken = tokenResponse.access_token;
            }
            GetSessionValueForAccessToken = accessToken;
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return  accessToken;
        }
    }


}
