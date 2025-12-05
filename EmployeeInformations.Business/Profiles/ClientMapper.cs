using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.ClientSummaryViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class ClientMapper : Profile
    {
        public ClientMapper()
        {
            CreateMap<ClientEntity, ClientViewModel>().ReverseMap();
        }


    }
}
