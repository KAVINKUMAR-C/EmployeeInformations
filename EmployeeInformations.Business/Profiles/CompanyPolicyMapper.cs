using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.CompanyPolicyViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class CompanyPolicyMapper : Profile
    {
        public CompanyPolicyMapper()
        {
            CreateMap<CompanyPolicyEntity, CompanyPolicy>().ReverseMap();
            CreateMap<PolicyAttachmentsEntity, PolicyAttachments>().ReverseMap();
            CreateMap<ManualLogEntity, ManualLog>().ReverseMap();
        }
    }
}
