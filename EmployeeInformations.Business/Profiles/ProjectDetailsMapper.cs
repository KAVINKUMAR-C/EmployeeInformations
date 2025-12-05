using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.ProjectSummaryViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class ProjectDetailsMapper : Profile
    {
        public ProjectDetailsMapper()
        {
            CreateMap<ProjectDetailsEntity, ProjectDetails>().ReverseMap();
            CreateMap<ProjectTypesEntity, ProjectTypes>().ReverseMap();
            CreateMap<ClientEntity, ClientCompanys>().ReverseMap();
            CreateMap<EmployeesEntity, DropdownProjectManager>().ReverseMap();
            CreateMap<EmployeesEntity, DropdownTeamLead>().ReverseMap();
            CreateMap<EmployeesEntity, DropdownEmployee>().ReverseMap();
            CreateMap<EmployeesEntity, DropdownProjectManagers>().ReverseMap();
            CreateMap<EmployeesEntity, DropdownTeamLeads>().ReverseMap();
            CreateMap<ReportingPersonsEntity, ReportingPerson>().ReverseMap();
            CreateMap<SkillSetEntity, SkillSets>().ReverseMap();
            CreateMap<ProjectAssignationEntity, ProjectAssignation>().ReverseMap();
            CreateMap<CurrencyEntity, CurrencyViewModel>().ReverseMap();
        }
    }
}
