using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.EmailDraftViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class EmailDraftMapper : Profile
    {
        public EmailDraftMapper()
        {
            CreateMap<EmailDraftTypeEntity, EmailDraftType>().ReverseMap();
            CreateMap<EmailDraftContentEntity, EmailDraftContent>().ReverseMap();
            CreateMap<EmailDraftTypeEntity, EmailDraftContent>().ReverseMap();
        }
    }
}
