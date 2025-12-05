using AutoMapper;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.CompanyViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class AssetMapper : Profile
    {
        public AssetMapper()
        {
            CreateMap<AssetCategoryEntity, AssetCategory>().ReverseMap();
            CreateMap<AssetCategoryEntity, AssetCategoryName>().ReverseMap();
            CreateMap<AssetTypesEntity, AssetTypes>().ReverseMap();
            CreateMap<AllAssetsEntity, AllAssets>().ReverseMap();
            CreateMap<AssetStatusEntity, AssetStatus>().ReverseMap();
            CreateMap<AssetLogEntity, AssetLog>().ReverseMap();
            CreateMap<CompanyEntity, companylocations>().ReverseMap();
            CreateMap<AssetBrandTypeEntity, AssetBrandType>().ReverseMap();
            CreateMap<BranchLocationEntity, BranchLocation>().ReverseMap();
            CreateMap<AssetLogReportDataModel, FilterViewAssetLogReport>().ReverseMap();
        }
    }
}
