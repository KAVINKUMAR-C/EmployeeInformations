using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.TimesheetSummaryViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class TimeSheetMapper : Profile
    {
        public TimeSheetMapper()
        {
            CreateMap<TimeSheetEntity, TimeSheet>().ReverseMap();
            CreateMap<TimeSheetEntity, ProjectNames>().ReverseMap();
            CreateMap<ProjectDetailsEntity, ProjectNames>().ReverseMap();

        }
    }
}
