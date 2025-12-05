using AutoMapper;
using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.APIModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.API.Service
{
    public class TimeSheetAPIService : ITimeSheetAPIService
    {
        private readonly ITimeSheetRepository _timeSheetRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IProjectDetailsRepository _projectDetailsRepository;

        public TimeSheetAPIService(ITimeSheetRepository timeSheetRepository, IMapper mapper, IConfiguration config, IProjectDetailsRepository projectDetailsRepository)
        {
            _timeSheetRepository = timeSheetRepository;
            _mapper = mapper;
            _config = config;
            _projectDetailsRepository = projectDetailsRepository;
        }

        /// <summary>
                /// Logic to get all the timesheet list and  particular empId timesheet in api 
                /// </summary>
        /// <param name="empId" ></param>
        public async Task<List<TimeSheetRequestModel>> GetTimesheet(int empId, int companyId)
        {
            var timeSheetRequestModels = new List<TimeSheetRequestModel>();
            var timeSheetEntitys = await _timeSheetRepository.GetAllTimeSheet(empId, companyId);

            if (empId == 0)
            {
                foreach (var item in timeSheetEntitys)
                {
                    timeSheetRequestModels.Add(new TimeSheetRequestModel()
                    {
                        TimeSheetId = item.TimeSheetId,
                        EmpId = item.EmpId,
                        ProjectId = item.ProjectId,
                        TaskDescription = item.TaskDescription,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Startdate = item.StartTime,
                        AttachmentFilePath = item.AttachmentFilePath,
                        AttachmentFileName = item.AttachmentFileName,
                        CompanyId = item.CompanyId,
                    });
                }
            }
            else
            {
                foreach (var item in timeSheetEntitys)
                {
                    timeSheetRequestModels.Add(new TimeSheetRequestModel()
                    {
                        TimeSheetId = item.TimeSheetId,
                        EmpId = item.EmpId,
                        ProjectId = item.ProjectId,
                        TaskDescription = item.TaskDescription,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Startdate = item.StartTime,
                        AttachmentFilePath = item.AttachmentFilePath,
                        AttachmentFileName = item.AttachmentFileName,
                        CompanyId = item.CompanyId,
                    });
                }
            }

            return timeSheetRequestModels;
        }

        public async Task<UserTimeSheetResponse> InsertTimesheet(TimeSheetRequestModel timeSheetRequestModel)
        {
            var contactUsResponseModel = new UserTimeSheetResponse();
            if (timeSheetRequestModel != null)
            {

                if (timeSheetRequestModel.TimeSheetId == 0)
                {
                    var combinePath = "";
                    if (!string.IsNullOrEmpty(timeSheetRequestModel.Base64string))
                    {
                        var fileFormat = timeSheetRequestModel.FileFormat;
                        var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "TimeSheets");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath = Path.Combine(path, fileName);

                        Byte[] bytes = Convert.FromBase64String(timeSheetRequestModel.Base64string);
                        SaveByteArrayToFileWithFileStream(bytes, combinePath);
                    }
                    DateTime startTime = Convert.ToDateTime(timeSheetRequestModel.StrStartdate + " " + timeSheetRequestModel.TimeSheetStartTime);
                    DateTime endTime = Convert.ToDateTime(timeSheetRequestModel.StrStartdate + " " + timeSheetRequestModel.TimeSheetEndTime);
                    timeSheetRequestModel.CreatedDate = DateTime.Now;
                    timeSheetRequestModel.StartTime = startTime;
                    timeSheetRequestModel.EndTime = endTime;
                    timeSheetRequestModel.CreatedBy = timeSheetRequestModel.EmpId;
                    timeSheetRequestModel.AttachmentFilePath = combinePath.Replace(Directory.GetCurrentDirectory(), "~");
                    var timeSheetEntity = _mapper.Map<TimeSheetEntity>(timeSheetRequestModel);
                    var datas = await _timeSheetRepository.CreateTimeSheet(timeSheetEntity, timeSheetEntity.CompanyId);
                    contactUsResponseModel.IsSuccess = true;
                    contactUsResponseModel.Message = Common.Constant.Success;
                }
                else
                {

                    var combinePath = "";
                    if (!string.IsNullOrEmpty(timeSheetRequestModel.Base64string))
                    {
                        var fileFormat = timeSheetRequestModel.FileFormat;
                        var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "TimeSheets");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        combinePath = Path.Combine(path, fileName);

                        Byte[] bytes = Convert.FromBase64String(timeSheetRequestModel.Base64string);
                        SaveByteArrayToFileWithFileStream(bytes, combinePath);
                    }
                    Common.Common.WriteServerErrorLog(" StrStartdate : " + timeSheetRequestModel.StrStartdate);
                    Common.Common.WriteServerErrorLog(" timesheet TimeSheetStartTime : " + timeSheetRequestModel.TimeSheetStartTime);
                    Common.Common.WriteServerErrorLog(" timesheet TimeSheetEndTime : " + timeSheetRequestModel.TimeSheetEndTime);
                    var timeSheetEntity = await _timeSheetRepository.GetByTimeSheetId(timeSheetRequestModel.TimeSheetId, timeSheetRequestModel.CompanyId);
                    var startTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(timeSheetRequestModel.StrStartdate + " " + timeSheetRequestModel.TimeSheetStartTime);
                    var endTime = DateTimeExtensions.ConvertToNotNullDatetimeTimeSheet(timeSheetRequestModel.StrStartdate + " " + timeSheetRequestModel.TimeSheetEndTime);

                    timeSheetEntity.UpdatedBy = timeSheetRequestModel.EmpId;
                    timeSheetEntity.UpdatedDate = DateTime.Now;
                    timeSheetEntity.TaskName = timeSheetRequestModel.TaskName;
                    timeSheetEntity.TaskDescription = timeSheetRequestModel.TaskDescription;
                    timeSheetEntity.Status = timeSheetRequestModel.Status;
                    timeSheetEntity.ProjectId = timeSheetRequestModel.ProjectId;
                    timeSheetEntity.Startdate = DateTimeExtensions.ConvertToNotNullDatetime(timeSheetRequestModel.StrStartdate);
                    timeSheetEntity.StartTime = startTime;
                    timeSheetEntity.EndTime = endTime;
                    timeSheetEntity.CreatedBy = timeSheetRequestModel.CreatedBy;
                    timeSheetEntity.CreatedDate = timeSheetEntity.CreatedDate;
                    timeSheetEntity.TimeSheetId = timeSheetRequestModel.TimeSheetId;
                    timeSheetEntity.AttachmentFilePath = string.IsNullOrEmpty(combinePath.Replace(Directory.GetCurrentDirectory(), "~")) ? timeSheetEntity.AttachmentFilePath : combinePath.Replace(Directory.GetCurrentDirectory(), "~");
                    timeSheetEntity.AttachmentFileName = string.IsNullOrEmpty(combinePath.Replace(Directory.GetCurrentDirectory(), "~")) ? timeSheetEntity.AttachmentFileName : combinePath.Replace(Directory.GetCurrentDirectory(), "~");
                    var results = await _timeSheetRepository.CreateTimeSheet(timeSheetEntity, timeSheetEntity.CompanyId);
                    contactUsResponseModel.IsSuccess = true;
                    contactUsResponseModel.Message = Common.Constant.Success;
                }
            }
            else
            {
                contactUsResponseModel.IsSuccess = false;
                contactUsResponseModel.Message = Common.Constant.Failure;
            }
            return contactUsResponseModel;
        }

        public static void SaveByteArrayToFileWithFileStream(byte[] data, string filePath)
        {
            using var stream = File.Create(filePath);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
                /// Logic to get projectnames detail the list by particular empId
                /// </summary>
        /// <param name="empId" ></param>
        public async Task<List<ProjectNamesAPI>> GetAllProjectNamesByEmpId(int empId, int companyId)
        {
            var listofProjectNames = new List<ProjectNamesAPI>();
            if (empId == 0)
            {
                var listofProjects = await _timeSheetRepository.GetAllProjectNames(empId,companyId);
                listofProjectNames = _mapper.Map<List<ProjectNamesAPI>>(listofProjects);
            }
            else
            {
                var projectAssignation = await _projectDetailsRepository.GetByProjectAssignationByEmpId(empId, companyId);

                foreach (var projectName in projectAssignation)
                {
                    var project = await _projectDetailsRepository.GetByProjectId(projectName.ProjectId,companyId);
                    listofProjectNames.Add(new ProjectNamesAPI()
                    {
                        ProjectId = project.ProjectId,
                        ProjectName = project.ProjectName,
                    });
                }
            }
            return listofProjectNames;
        }


        /// <summary>
               /// Logic to get delete the timesheet detail by particular TimeSheetId
              /// </summary> 
        /// <param name="timeSheet" ></param>
        public async Task<bool> DeleteTimeSheet(int TimeSheetId, int companyId)
        {
            var result = await _timeSheetRepository.DeleteTimeSheet(TimeSheetId,companyId);
            return result;
        }

    }
}
