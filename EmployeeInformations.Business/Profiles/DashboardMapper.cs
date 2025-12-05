using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.DashboardViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class DashboardMapper : Profile
    {
        public DashboardMapper()
        {
            CreateMap<TimeLoggerEntity, TimeLoggerViewModel>().ReverseMap();
            CreateMap<LeaveTypesEntity, LeaveTypesEmployee>().ReverseMap();
        }
    }

}
