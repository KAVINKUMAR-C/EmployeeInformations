using AutoMapper;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class ReportsMapper : Profile
    {
        public ReportsMapper()
        {

            CreateMap<EmployeeLeaveReportDataModel, FilterViewEmployeeLeave>().ReverseMap();
            CreateMap<LeaveReportDateModel, FilterViewEmployeeLeave>().ReverseMap();
            CreateMap<ManualLogReportDateModel, FilterViewManualLog>().ReverseMap();
            CreateMap<TimeSheetDataModel, FilterViewTimeSheet>().ReverseMap();
            CreateMap<ProjectDetailsEntity, ProjectNames>().ReverseMap();
        }
    }

}
