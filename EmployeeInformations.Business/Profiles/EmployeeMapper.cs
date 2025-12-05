using AutoMapper;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using EmployeesPrivileges = EmployeeInformations.CoreModels.Model.EmployeesPrivileges;


namespace EmployeeInformations.Business.Profiles
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            //CreateMap<EmployeesEntity, Employees>().ReverseMap();
            //CreateMap<EmployeesEntity, Employees>()
            // .ForMember(dest => dest.RoleId, opt => opt.Ignore())
            // .ReverseMap();
            CreateMap<Employees, EmployeesEntity>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (byte)src.RoleId));

            CreateMap<EmployeesEntity, Employees>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (Role)src.RoleId));

            CreateMap<ProfileInfoEntity, ProfileInfo>().ReverseMap();
            CreateMap<AddressInfoEntity, AddressInfo>().ReverseMap();
            CreateMap<StateEntity, State>().ReverseMap();
            CreateMap<CityEntity, City>().ReverseMap();
            CreateMap<CountryEntity, Country>().ReverseMap();
            CreateMap<SkillSetEntity, SkillSet>().ReverseMap();
            CreateMap<OtherDetailsEntity, OtherDetails>().ReverseMap();
            CreateMap<QualificationEntity, Qualification>().ReverseMap();
            CreateMap<ExperienceEntity, Experience>().ReverseMap();
            CreateMap<BankDetailsEntity, BankDetails>().ReverseMap();
            CreateMap<DesignationEntity, Designation>().ReverseMap();
            CreateMap<DepartmentEntity, Department>().ReverseMap();
            CreateMap<DocumentTypesEntity, DocumentTypes>().ReverseMap();
            CreateMap<BloodGroupEntity, BloodGroup>().ReverseMap();
            CreateMap<EmployeesReleaveingTypeEntity, EmployeesReleaveingType>().ReverseMap();
            CreateMap<RelationshipTypeEntity, RelationshipType>().ReverseMap();
            CreateMap<EmployeesLogEntity, EmployeesLog>().ReverseMap();
            CreateMap<EmployeesLogReportDataModel, FilterViewEmployeeLogReport>().ReverseMap();
            CreateMap<CompanySettingEntity, CompanySetting>().ReverseMap();
            CreateMap<EmployeeActivityLogEntity, EmployeeActivityLog>().ReverseMap();
            CreateMap<SalaryEntity, salarys>().ReverseMap();
            //CreateMap<EmployeesDetailsDataModel, Employees>().ReverseMap();
            CreateMap<EmployeesDetailsDataModel, Employees>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (Role)src.RoleId.Value)) // for enum
            .ReverseMap()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (byte)src.RoleId)); // reverse
            CreateMap<EmployeesPrivileges, EmployeePrivilegesViewModel>().ReverseMap();
            CreateMap<EmployeesPrivileges, EmployeesPrivilegesViewModel>().ReverseMap();
        }
    }
}
