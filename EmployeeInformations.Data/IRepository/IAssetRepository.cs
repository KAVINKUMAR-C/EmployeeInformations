using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IAssetRepository
    {
        Task<List<AssetCategoryEntity>> GetAllAssetCategory(int companyId);
        Task<int> Create(AssetCategoryEntity assetCategoryEntity,int companyId);
        Task<List<AssetTypesEntity>> GetAssetTypeIdByCategoryId(int categoryId,int companyId);
        Task<List<AllAssetsEntity>> GetAllAssetsByCategoryId(int categoryId,int companyId);
        Task UpdateAssetCategory(AssetCategoryEntity assetCategoryEntity, int companyId);
        Task<List<AssetTypesEntity>> GetAllAssetType(int companyId);
        Task<int> CreateAssetType(AssetTypesEntity assetTypesEntity,int companyId);
        Task<int> GetTypeName(string typeName,int companyId);
        Task<bool> DeletedAssetType(int id,int companyId);
        Task<bool> DeletedAssetTypes(List<AssetTypesEntity> assetTypesEntities);
        Task<bool> DeletedAllAssets(List<AllAssetsEntity> allAssetsEntities);
        Task UpdateAssetType(AssetTypesEntity assetTypesEntity,int companyId);
        Task<List<AllAssetsEntity>> GetAllAssets(int companyId);
        Task<List<AssetStatusEntity>> GetAllAssetStatus(int companyId);
        Task<int> CreateAssets(AllAssetsEntity allAssetsEntity,int companyId);
        Task<List<AllAssetsEntity>> GetAssetByEmployeeId(int empId,int companyId);
        Task<List<EmployeeBenefitEntity>> GetBenefitByEmployeeId(int empId,int companyId);
        Task<bool> DeleteAllAssets(AllAssetsEntity allAssetsEntity);
        Task<List<AssetLogEntity>> GetAllAssetLog(int companyId);
        Task<List<CompanyEntity>> GetCompany(int companyId);
        Task<List<AssetTypesEntity>> GetBycategoryId(int categoryId);
        Task<int> GetAssetsMaxCount(int categoryId, int companyId);
        Task<AssetCategoryEntity> GetAllAssetCategoryId(int categoryId,int companyId);
        Task Upadate(AssetCategoryEntity assetCategoryEntity,int companyId);
        Task UpadateAssetType(AssetTypesEntity assetTypesEntity,int companyId);
        Task<List<AssetBrandTypeEntity>> GetAllBrand(int companyId);
        Task<int> Create(AssetBrandTypeEntity assetBrandTypeEntity);
        Task UpdateAssetBrand(AssetBrandTypeEntity assetBrandTypeEntity);
        Task UpadateAssetBrandType(AssetBrandTypeEntity assetBrandTypeEntity);
        Task<AssetBrandTypeEntity> GetAssetBrandNameById(int BrandTypeId,int companyId);
        Task<List<AssetTypesEntity>> GetAssetType(int companyId);
        Task<List<AssetBrandTypeEntity>> GetAssetBrand(int companyId);
        Task<List<AssetLogReportDataModel>> GetAllAssetByEmployeeLogFilter(string proc, List<KeyValuePair<string, string>> values);
        Task<List<AllAssets>> GetAllAsset(int companyId);
        Task<List<AssetBrandType>> GetAllAssetBrandTypes(int companyId);
        Task<int> GetAllEmployeesAssetLogCount(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId);
        Task<List<AssetsLogModel>> GetAllEmployeesAssetLogList(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId);
        Task<List<AssetsLogModel>> GetAllEmployessByAssetLogFilter(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId);
        Task<List<AssetCategory>> GetAllAssetCategorys(int companyId);
        Task<int> GetCategoryName(string categoryName, int companyId);
        Task<int> GetCategoryCode(string categoryCode, int companyId);
        Task<bool> DeletedAssetCategory(int categoryId, int companyId);
        Task<List<AssetCategoryEntity>> GetAllAssetCategoryName(int companyId);
        Task<List<AssetTypes>> GetAllAssetTypes(int companyId);
        Task<int> GetAssetListCount(SysDataTablePager pager, int companyId);
        Task<List<AssetViewModels>> GetAssetDetailsList(SysDataTablePager pager, string columnDirection, string columnName, int companyId);
        Task<int> GetBrandName(string brandType, int companyId);
        Task<bool> DeletedAssetBrand(int brandTypeId, int companyId);
        Task<List<BranchLocationEntity>> GetAllBranchLocationId(int companyId);
        Task<AllAssetsEntity> GetByAllAssetsId(int allAssetsId, int companyId);
        Task<AssetTypesEntity> GetAssetTypeNameById(int AssetTypeId, int companyId);
        Task<AssetCategoryEntity> GetAssetCategoryNameByCategoryId(int categoryId, int companyId);
        Task<AssetStatusEntity> GetAssetStatusNameByName(int AssetStatusId, int companyId);

    }
}
