using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.PrivilegeViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleEntity, RoleViewModel>().ForMember(dest => dest.RoleId, opt => opt.Ignore()).ReverseMap();
        }


    }
}
