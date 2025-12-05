using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.BenefitViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class BenefitMapper : Profile
    {
        public BenefitMapper()
        {
            CreateMap<BenefitTypesEntity, BenefitTypes>().ReverseMap();
            CreateMap<EmployeeBenefitEntity, EmployeeBenefit>().ReverseMap();
            CreateMap<EmployeeMedicalBenefitEntity, EmployeeMedicalBenefit>().ReverseMap();
        }

    }
}
