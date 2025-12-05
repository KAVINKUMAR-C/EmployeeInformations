using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.TeamsViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class MasterMapper : Profile
    {
        public MasterMapper()
        {
            CreateMap<DesignationEntity, Designation>().ReverseMap();
            CreateMap<DepartmentEntity, Department>().ReverseMap();
            CreateMap<RoleEntity, RoleTableMaster>().ReverseMap();
            CreateMap<ModulesEntity, Modules>().ReverseMap();
            CreateMap<SubModulesEntity, SubModules>().ReverseMap();
            CreateMap<ModulesEntity, ModuleName>().ReverseMap();
            CreateMap<ProjectTypesEntity, ProjectTypeMaster>().ReverseMap();
            CreateMap<DocumentTypesEntity, DocumentType>().ReverseMap();
            CreateMap<SkillSetEntity, SkillSets>().ReverseMap();
            CreateMap<EmailSettingsEntity, EmailSettings>().ReverseMap();
            CreateMap<SendEmailsEntity, SendEmails>().ReverseMap();
            CreateMap<LeaveTypesEntity, LeaveTypeMaster>().ReverseMap();
            CreateMap<AnnouncementEntity, Announcement>().ReverseMap();
            CreateMap<DashboardMenusEntity, DashboardMenus>().ReverseMap();
            CreateMap<RelievingReasonEntity, RelievingReason>().ReverseMap();
            CreateMap<TicketTypesEntity, TicketTypes>().ReverseMap();
            CreateMap<TeamsMeetingEntity, Teams>().ReverseMap();
        }
    }
}
