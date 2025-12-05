using AutoMapper;
using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.APIDashboardModel;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Business.API.Profiles
{
    public class JobPostResponseModelMapper : Profile
    {
        public JobPostResponseModelMapper()
        {
            //vphospital Website
            CreateMap<WebsiteContactUsEntity, ContactUsRequestModel>().ReverseMap();
            CreateMap<WebsiteJobsEntity, JobPostResponseModel>().ReverseMap();
            CreateMap<WebsiteJobsEntity, WebsiteJobViewModel>().ReverseMap();
            CreateMap<WebsiteJobApplyEntity, WebsiteJobApplyModel>().ReverseMap();
            CreateMap<WebsiteLeadTypeEntity, WebsiteLeadTypeResponseModel>().ReverseMap();
            CreateMap<WebsiteServicesEntity, WebsiteServicesResponseModel>().ReverseMap();
            CreateMap<WebsiteQuoteEntity, WebsiteQuoteRequestModel>().ReverseMap();
            CreateMap<WebsiteQuoteEntity, WebsiteSurveyRequestModel>().ReverseMap();
            CreateMap<WebsiteSurveyAnswerEntity, SurveyAnswerRequestModel>().ReverseMap();
            CreateMap<WebsiteQuoteEntity, WebsiteQuickQuoteViewModel>().ReverseMap();
            CreateMap<WebsiteSurveyAnswerEntity, WebsiteSoftwareConsultant>().ReverseMap();
            CreateMap<WebsiteCandidateMenuEntity, WebsiteCandidateMenuModel>().ReverseMap();
            CreateMap<WebsiteCandidateMenuEntity, WebsiteJobApplyEntity>().ReverseMap();
            CreateMap<WebSiteCandidateScheduleEntity, WebsiteCandidateSchedule>().ReverseMap();
            CreateMap<EmployeesEntity, DropdownEmployees>().ReverseMap();

            // Time sheet
            CreateMap<TimeSheetEntity, TimeSheetRequestModel>().ReverseMap();

            //leave 
            CreateMap<EmployeeAppliedLeaveEntity, LeaveRequestModel>().ReverseMap();
            CreateMap<LeaveTypesEntity, LeaveTypesAPI>().ReverseMap();
            //Employees
            CreateMap<EmployeesEntity, EmployeesRequestModel>().ReverseMap();
            CreateMap<DesignationEntity, Designations>().ReverseMap();
            CreateMap<DepartmentEntity, Departments>().ReverseMap();
            CreateMap<RoleEntity, RoleViewModels>().ReverseMap();
            CreateMap<RelievingReasonEntity, RelievingReasons>().ReverseMap();
            CreateMap<EmployeesReleaveingTypeEntity, RelievingReasons>().ReverseMap();
            CreateMap<EmployeesRequestModel, Employees>().ReverseMap();
            //Dashboard
            CreateMap<LeaveTypesEntity, LeaveTypesEmployeeAPI>().ReverseMap();
            CreateMap<TimeLoggerEntity, TimeLoggerViewModelAPI>().ReverseMap();
        }
    }
}
