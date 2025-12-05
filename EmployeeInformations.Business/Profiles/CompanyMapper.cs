using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.CompanyViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class CompanyMapper : Profile
    {
        public CompanyMapper()
        {
            CreateMap<CompanyEntity, Company>().ReverseMap();
            CreateMap<BranchLocationEntity, BranchLocation>().ReverseMap();
            CreateMap<MailSchedulerEntity, MailScheduler>().ReverseMap();
            CreateMap<CompanyEntity, CompanySetting>().ReverseMap();
        }

    }
}
