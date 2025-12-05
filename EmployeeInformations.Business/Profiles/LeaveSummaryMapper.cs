using AutoMapper;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.LeaveSummaryViewModel;


namespace EmployeeInformations.Business.Profiles
{
    public class LeaveSummaryMapper : Profile
    {
        public LeaveSummaryMapper()
        {
            CreateMap<LeaveTypesEntity, LeaveTypes>().ReverseMap();
            CreateMap<EmployeeAppliedLeaveEntity, EmployeeAppliedLeave>().ReverseMap();
            CreateMap<AllLeaveDetailsEntity, AllLeaveDetails>().ReverseMap();
            CreateMap<EmployeeHolidaysEntity, EmployeeHolidays>().ReverseMap();
            CreateMap<CompensatoryRequestsEntity, CompensatoryRequest>().ReverseMap();
            CreateMap<EmployeeCompensatoryFilter, CompensatoryRequestViewModel>().ReverseMap();
        }
    }
}
