using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.TimesheetSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.Service
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IProjectDetailsRepository _projectDetailsRepository;
        public TimeSheetService(ITimeSheetRepository timeSheetRepository, IMapper mapper, IConfiguration config, IEmployeesRepository employeesRepository, IProjectDetailsRepository projectDetailsRepository)
        {
            _timeSheetRepository = timeSheetRepository;
            _mapper = mapper;
            _config = config;
            _employeesRepository = employeesRepository;
            _projectDetailsRepository = projectDetailsRepository;
        }

        //Timesheet 

        /// <summary>
        /// Logic to get timesheet detail the list by particular empId
        /// </summary>
        /// <param name="empId" ></param>
        public async Task<List<TimeSheet>> GetAllTimeSheet(int empId, int companyId)
        {
            var listOfTimes = empId == 0 ? await _timeSheetRepository.GetAllEmployeesTimeSheet(companyId) : await _timeSheetRepository.GetEmployeeTimesheet(empId, companyId);
            return listOfTimes;
        }

        /// <summary>
        /// Logic to get timesheet detail the list by particular empId
        /// </summary>
        /// <param name="empId,pager,columnDirection,columnName" ></param>
        public async Task<TimeSheet> GetAllTimeSheets(SysDataTablePager pager, int empId,string columnDirection,string columnName, int companyId)
        {
            var listOfTimes = new TimeSheet();                                           
            listOfTimes.TimeSheetCount = await _timeSheetRepository.GetAllTimeSheetListCount(pager, empId, companyId);
            listOfTimes.TimeSheetModels = await _timeSheetRepository.GetTimeSheetByEmpIdFilterList(pager, empId, columnDirection, columnName,companyId);
            return listOfTimes;
        }


        /// <summary>
        /// Logic to get projectnames detail the list by particular empId
        /// </summary>
        /// <param name="empId" ></param>
        public async Task<List<ProjectNames>> GetAllProjectNames(int empId, int companyId)
        {
            if (empId == 0)
            {
                var listOfProjects = await _timeSheetRepository.GetAllProjectNames(empId, companyId);
                return _mapper.Map<List<ProjectNames>>(listOfProjects);
            }

            var projectAssignations = await _projectDetailsRepository.GetByProjectAssignationByEmpId(empId, companyId);
            var listOfProjectNames = new List<ProjectNames>();

            foreach (var projectName in projectAssignations)
            {
                var project = await _projectDetailsRepository.GetByProjectId(projectName.ProjectId, companyId);
                listOfProjectNames.Add(new ProjectNames
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName
                });
            }

            return listOfProjectNames;
        }

        /// <summary>
        /// Logic to get the timesheet detail by particular TimeSheetId
        /// </summary> 
        /// <param name="TimeSheetId" ></param>
        public async Task<TimeSheet> GetByTimeSheetId(int TimeSheetId, int companyId)
        {
            var timeSheet = new TimeSheet();
            var timeSheetEntity = await _timeSheetRepository.GetByTimeSheetId(TimeSheetId, companyId);
            var timeSheets = _mapper.Map<TimeSheet>(timeSheetEntity);
            if (timeSheetEntity != null)
            {
                var StartTime = timeSheets.StartTime.ToString("hh:mm tt");
                var EndTime = timeSheets.EndTime.ToString("hh:mm tt");
                DateTime startTime = Convert.ToDateTime(StartTime);
                DateTime endTime = Convert.ToDateTime(EndTime);
                timeSheet.EmpId = timeSheets.EmpId;
                timeSheet.TimeSheetStartTime = StartTime;
                timeSheet.TimeSheetEndTime = EndTime;
                timeSheet.TaskName = timeSheets.TaskName;
                timeSheet.TaskDescription = timeSheets.TaskDescription;
                timeSheet.Startdate = timeSheets.Startdate;
                //timeSheet.Enddate = timeSheets.Enddate;
                timeSheet.AttachmentFilePath = timeSheets.AttachmentFilePath;
                timeSheet.AttachmentFileName = timeSheets.AttachmentFileName;
                timeSheet.Status = timeSheets.Status;
                timeSheet.ProjectName = timeSheets.ProjectName;
                timeSheet.ProjectId = timeSheets.ProjectId;
                timeSheet.splitdocname = string.IsNullOrEmpty(timeSheets.AttachmentFileName) ? "" : timeSheets.AttachmentFileName.Substring(timeSheets.AttachmentFileName.LastIndexOf(".") + 1);
            }
            return timeSheet;
        }

        /// <summary>
        /// Logic to get create and update the timesheet detail by particular TimeSheetId
        /// </summary> 
        /// <param name="timeSheet" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> CreateTimeSheet(TimeSheet timeSheet, int sessionEmployeeId,int companyId)
        {
            var result = 0;
            if (timeSheet != null)
            {
                if (timeSheet.TimeSheetId == 0)
                {
                    DateTime startTime = Convert.ToDateTime(timeSheet.StrStartdate + " " + timeSheet.TimeSheetStartTime);
                    DateTime endTime = Convert.ToDateTime(timeSheet.StrStartdate + " " + timeSheet.TimeSheetEndTime);
                    timeSheet.StartTime = startTime; /*Convert.ToDateTime(timeSheet.Startdate).Date.AddHours(startTime.Hour).AddMinutes(startTime.Minute);*/
                    timeSheet.EndTime = endTime; /*Convert.ToDateTime(timeSheet.Startdate).Date.AddHours(endTime.Hour).AddMinutes(endTime.Minute);*/
                    timeSheet.CreatedBy = sessionEmployeeId;
                    timeSheet.EmpId = timeSheet.EmpId == 0 ? sessionEmployeeId : timeSheet.EmpId;
                    timeSheet.CreatedDate = Convert.ToDateTime(DateTime.Now);
                    timeSheet.Startdate = Convert.ToDateTime(timeSheet.StrStartdate);
                    var timeSheetEntity = _mapper.Map<TimeSheetEntity>(timeSheet);
                    var datas = await _timeSheetRepository.CreateTimeSheet(timeSheetEntity,companyId);
                    result = timeSheetEntity.TimeSheetId;
                }
                else
                {
                    Common.Common.WriteServerErrorLog(" StrStartdate : " + timeSheet.StrStartdate);
                    Common.Common.WriteServerErrorLog(" timesheet TimeSheetStartTime : " + timeSheet.TimeSheetStartTime);
                    Common.Common.WriteServerErrorLog(" timesheet TimeSheetEndTime : " + timeSheet.TimeSheetEndTime);
                    var timeSheetEntity = await _timeSheetRepository.GetByTimeSheetId(timeSheet.TimeSheetId,companyId);
                    var startTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(timeSheet.StrStartdate + " " + timeSheet.TimeSheetStartTime);
                    var endTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(timeSheet.StrStartdate + " " + timeSheet.TimeSheetEndTime);

                    timeSheetEntity.UpdatedBy = sessionEmployeeId;
                    timeSheetEntity.UpdatedDate = DateTime.Now;
                    timeSheetEntity.TaskName = timeSheet.TaskName;
                    timeSheetEntity.TaskDescription = timeSheet.TaskDescription;
                    timeSheetEntity.Status = timeSheet.Status;
                    timeSheetEntity.ProjectId = timeSheet.ProjectId;
                    timeSheetEntity.Startdate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheet.StrStartdate);
                    timeSheetEntity.StartTime = startTime;
                    timeSheetEntity.EndTime = endTime;
                    timeSheetEntity.AttachmentFilePath = string.IsNullOrEmpty(timeSheet.AttachmentFilePath) ? timeSheetEntity.AttachmentFilePath : timeSheet.AttachmentFilePath;
                    timeSheetEntity.AttachmentFileName = string.IsNullOrEmpty(timeSheet.AttachmentFileName) ? timeSheetEntity.AttachmentFileName : timeSheet.AttachmentFileName;
                    result = await _timeSheetRepository.CreateTimeSheet(timeSheetEntity,companyId);
                }
            }

            return result;
        }

        /// <summary>
        /// Logic to get delete the timesheet detail by particular TimeSheetId
        /// </summary> 
        /// <param name="timeSheet" ></param>
        public async Task<bool> DeleteTimeSheet(int TimeSheetId, int companyId)
        {
            var result = await _timeSheetRepository.DeleteTimeSheet(TimeSheetId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get dispaly view the timesheet detail by particular TimeSheetId
        /// </summary> 
        /// <param name="timeSheet" ></param>
        public async Task<ViewTimeSheet> GetTimeSheetDetailsByTimeSheetId(int timeSheetId, int companyId)
        {
            var viewTimeSheetModel = new ViewTimeSheet();
            var timeSheetEntity = await _timeSheetRepository.GetByTimeSheetId(timeSheetId,companyId);
            if (timeSheetEntity != null)
            {
                viewTimeSheetModel.StartTime = timeSheetEntity.StartTime.ToString("hh:mm:ss tt");
                viewTimeSheetModel.EndTime = timeSheetEntity.EndTime.ToString("hh:mm:ss tt");
                viewTimeSheetModel.TaskName = timeSheetEntity.TaskName;
                viewTimeSheetModel.TaskDescription = timeSheetEntity.TaskDescription;
                viewTimeSheetModel.Startdate = timeSheetEntity.Startdate.ToString("dd/MM/yyyy");
                viewTimeSheetModel.AttachmentFilePath = !string.IsNullOrEmpty(timeSheetEntity.AttachmentFilePath) ? timeSheetEntity.AttachmentFilePath : string.Empty;
                // viewTimeSheetModel.AttachmentFileName = timeSheets.AttachmentFileName;
                viewTimeSheetModel.Status = Convert.ToString((TimeSheetStatus)timeSheetEntity.Status);
                viewTimeSheetModel.ProjectName = await _timeSheetRepository.GetProjectName(timeSheetEntity.ProjectId,companyId);
            }
            return viewTimeSheetModel;
        }      
    }
}
