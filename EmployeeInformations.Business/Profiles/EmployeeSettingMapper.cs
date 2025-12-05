using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.EmployeeSettingViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class EmployeeSettingMapper : Profile
    {
        public EmployeeSettingMapper()
        {
            CreateMap<EmployeeSettingEntity, EmployeeSetting>().ReverseMap();
        }
    }
}
