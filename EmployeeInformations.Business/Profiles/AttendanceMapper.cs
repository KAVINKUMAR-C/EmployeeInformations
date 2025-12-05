using AutoMapper;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.AttendanceViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class AttendanceMapper : Profile
    {
        public AttendanceMapper()
        {
            CreateMap<AttendanceReportDateModel, FilterViewAttendace>().ReverseMap();
            CreateMap<AttendaceListViewModel, AttendanceListDataModel>().ReverseMap();
        }
    }
}
