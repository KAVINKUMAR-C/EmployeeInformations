using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Data.Repository;
using EmployeeInformations.Model.ClientSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.TeamsViewModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.Service
{
    public class TeamsMeetingService: ITeamsMeetingService
    {
        private readonly ITeamsMeetingRepository _teamsMeetingRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;


        public TeamsMeetingService(ITeamsMeetingRepository teamsMeetingRepository, IMapper mapper, IConfiguration configuration)
        {
            _teamsMeetingRepository = teamsMeetingRepository;
            _mapper = mapper;
            _config = configuration;
        }

        /// <summary>
        /// Logic to get create the teams 
        /// </summary>
        /// <param name="teams"></param>
        /// <param name="sessionEmployeeId"></param>       
        public async Task<int> CreateTeamsMeeting(Teams teams, int sessionEmployeeId,int companyId )
        {
            var result = 0;
            try
            {
                if (teams != null)
                {
                    if (teams.TeamsMeetingId == 0)
                    {
                        DateTime startTime = Convert.ToDateTime(teams.StrStartdate + " " + teams.MeetingStartTime);
                        DateTime endTime = Convert.ToDateTime(teams.StrStartdate + " " + teams.MeetingEndTime);
                        teams.CreatedBy = sessionEmployeeId;
                        teams.CreatedDate = DateTime.Now;
                        teams. StartDate = startTime;
                        teams.EndDate = endTime;
                        var teamsMeetingEntity = _mapper.Map<TeamsMeetingEntity>(teams);
                        var datas = await _teamsMeetingRepository.CreateTeamsMeeting(teamsMeetingEntity,companyId);
                        result = teamsMeetingEntity.TeamsMeetingId;
                    }
                    else
                    {
                        var teamsEntity = await _teamsMeetingRepository.GetByTeamsMeetingId(teams.TeamsMeetingId,companyId);
                        DateTime startTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(teams.StrStartdate + " " + teams.MeetingStartTime);
                        DateTime endTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(teams.StrStartdate + " " + teams.MeetingEndTime);                      
                        teamsEntity.UpdatedDate = DateTime.Now;
                        teamsEntity.UpdatedBy = sessionEmployeeId;
                        teamsEntity.TeamsMeetingId = teams.TeamsMeetingId;
                        teamsEntity.MeetingId = teams.MeetingId;
                        teamsEntity.MeetingName = teams.MeetingName;
                        teamsEntity.AttendeeEmail = teams.AttendeeEmail;
                        teamsEntity.StartDate = startTime;
                        teamsEntity.EndDate = endTime;
                        result = await _teamsMeetingRepository.CreateTeamsMeeting(teamsEntity, companyId);

                    }


                }
            }
            catch (Exception ex) { 

            }
            return result;
        }


        /// <summary>
        /// Logic to get Employee OfficeMail  by particular employees 
        /// </summary>       
        /// <param name="sessionEmployeeId"></param>
        public async Task<string> GetEmployeeOfficeMail(int sessionEmployeeId, int companyId)
        {
            return await _teamsMeetingRepository.GetEmployeeOfficeMail(sessionEmployeeId,companyId);
        }

        /// <summary>
        /// Logic to get all teams meeting list
        /// </summary>       
        /// <param name="companyId"></param>
        /// <param name="pager"></param>
        /// <param name="empId"></param>
        /// <param name="columnDirection"></param>
        /// <param name="columnName"></param>
        public async Task<Teams> GetTeamMeetingEmployeesList(SysDataTablePager pager, int empId, string columnDirection, string columnName, int companyId)
        {
            var teams = new Teams();
            teams.TeamMeetingModels = await _teamsMeetingRepository.GetTeamMeetingEmployeesList( pager, empId, columnDirection, columnName,companyId);
            return teams;
        }

        /// <summary>
        ///  Logic to get all team meeting count 
        /// </summary>
        /// <param name="empId,pager" ></param>    
        public async Task<int> GetAllTeamMeetingByFilterCount(int empId, SysDataTablePager pager, int companyId)
        {
            var teamMeetingcount = await _teamsMeetingRepository.GetAllTeamMeetingByFilterCount(pager, empId,companyId);
            return teamMeetingcount;
        }

        /// <summary>
        ///  Logic to get the teams detail by particular teamsMeetingId
        /// </summary>
        /// <param name="teamsMeetingId" ></param>
        public async Task<Teams> GetByTeamsMeetingId(int teamsMeetingId, int companyId)
        {
            var teams = new Teams();
            var teamsMeetingEntity = await _teamsMeetingRepository.GetByTeamsMeetingId(teamsMeetingId,companyId);
            var teamsModel = _mapper.Map<Teams>(teamsMeetingEntity);
            if (teamsMeetingEntity != null)
            {
                var StartTime = teamsModel.StartDate.ToString("hh:mm tt");
                var EndTime = teamsModel.EndDate.ToString("hh:mm tt");
                teams.TeamsMeetingId = teamsMeetingId;
                teams.MeetingName = teamsModel.MeetingName;
                teams.AttendeeEmail = teamsModel.AttendeeEmail;
                teams.MeetingStartTime = StartTime;
                teams.MeetingEndTime = EndTime;
                teams.StartDate = teamsModel.StartDate;
                teams.MeetingId = teamsModel.MeetingId;
            }
            return teams;
        }

        /// <summary>
        /// Logic to get deleted teams detail by particular teamsMeetingId
        /// </summary> 
        /// <param name="teamsMeetingId" ></param>
        public async Task<bool> DeleteTeamsMeeting(int teamsMeetingId, int companyId)
        {
            var result = false;
            var teamsMeetingEntity = await _teamsMeetingRepository.GetByTeamsMeetingId(teamsMeetingId, companyId);
            if (teamsMeetingEntity != null)
            {
                teamsMeetingEntity.IsDeleted = true;
                result =  await _teamsMeetingRepository.DeleteTeamsMeeting(teamsMeetingEntity);                
            }
            return result;
        }
    }
}
