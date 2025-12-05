using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.OnboardingViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class OBEmployeesMapper : Profile
    {
        public OBEmployeesMapper()
        {
            CreateMap<EmployeesEntity, OBEmployees>().ReverseMap();

            CreateMap<ProfileInfoEntity , OBProfileInfo>().ReverseMap();

            CreateMap<AddressInfoEntity, OBAddressInfo>().ReverseMap();

            CreateMap<OtherDetailsEntity, OBOtherDetails>().ReverseMap();
            CreateMap<DocumentTypesEntity , OBDocumentTypes>().ReverseMap();
            CreateMap<QualificationEntity ,OBQulification>().ReverseMap();  
            CreateMap<ExperienceEntity , OBExperience>().ReverseMap();   
            CreateMap<BankDetailsEntity , OBBankDetails>().ReverseMap();    
        }
    }
}
