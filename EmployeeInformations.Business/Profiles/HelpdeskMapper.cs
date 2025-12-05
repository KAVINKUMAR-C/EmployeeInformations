using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.HelpdeskViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformations.Business.Profiles
{
    public class HelpdeskMapper : Profile
    {
        public HelpdeskMapper() {
            CreateMap<HelpdeskEntity, HelpdeskViewModel>().ReverseMap();
            CreateMap<TicketAttachmentsEntity, TicketAttachments>().ReverseMap();
            CreateMap<HelpdeskEntity, Helpdesk>().ReverseMap();
        }
    }
}
