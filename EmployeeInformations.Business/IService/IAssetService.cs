using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IAssetService
    {
        Task<int> UpdateAssetCategory(AssetCategory assetCategory,int companyId);
        Task<List<AllAssets>> GetAllAssets(int companyId);
        Task<int> GetTypeName(string typeName,int companyId);
        Task<bool> DeletedAssetType(int id,int companyId);
        Task<int> UpdateAssetType(AssetTypes assetTypes,int companyId);
        Task<List<AssetStatus>> GetAllAssetStatus(int companyId);
        Task<List<AssetTypes>> GetAssetType(int companyId);
        Task<AllAssets> GetByAllAssetsId(int allAssetsId,int companyId);
        Task<List<ViewAssets>> GetAssetByEmployeeId(int empId,int companyId);
        Task<bool> DeleteAllAssets(int allAssetsId, int sessionEmployeeId,int companyId);
        Task<AllAssets> GetAssetByEmployeeIds(int allAssetsId,int companyId);
        Task<List<AssetTypes>> GetBycategoryId(int categoryId, int companyId);
        Task<string> GetRefCategoryId(int categoryId, int companyId);
        Task<int> Upadate(AssetCategory assetCategory,int companyId);
        Task<int> UpadateAssetType(AssetTypes assetTypes,int companyId);
        Task<AssetBrandTypeViewModel> GetAllBrand(int companyId);       
        Task<int> UpdateAssetBrand(AssetBrandType assetBrandType);
        Task<int> UpadateAssetBrandType(AssetBrandType assetBrandType);
        Task<List<AssetBrandType>> GetAssetBrand(int companyId);
        Task<AssetLogViewModel> GetAllAssetLog(AssetLogViewModel assetLogViewModel, int companyId);
        Task<AssetLogViewModel> GetAllAssetByEmployeeLogFilter(AssetLogViewModel assetLogViewModel, int companyId);
        Task<AssetCategory> Create(AssetCategory assetCategory,int companyId);
        Task<AssetBrandType> Create(AssetBrandType assetBrandType);
        Task<AssetTypes> CreateAssetType(AssetTypes assetTypes,int companyId);
        Task<AssetLogViewModel> GetAllEmployeesAssetLogList(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId);
        Task<List<AssetDetailsDropdown>> GetAllAssetDetailsDropdown(int companyId);
        Task<List<AssetDropdown>> GetAllAssetDrropdown(int companyId);
        Task<int> GetAllEmployeesAssetLogCount(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId);
        Task<AssetLogViewModel> GetAllEmployessByAssetLogFilter(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId);
        Task<AssetCategoryViewModal> GetAllAssetCategory(int companyId);
        Task<int> GetCategoryName(string categoryName, int companyId);
        Task<int> GetCategoryCode(string categoryCode, int companyId);
        Task<bool> DeletedAssetCategory(int categoryId, int companyId);
        Task<List<AssetCategoryName>> GetAllAssetCategoryName(int companyId);
        Task<AssetTypeViewModal> GetAllAssetType(int companyId);
        Task<List<AssetCategory>> GetAssetCategory(int companyId);
        Task<int> GetAssetListCount(SysDataTablePager pager, int companyId);
        Task<AllAssets> GetAllEmployeesList(SysDataTablePager pager, string columnDirection, string columnName, int companyId);
        Task<int> GetBrandName(string brandType, int companyId);
        Task<bool> DeletedAssetBrand(int brandTypeId, int companyId);
        Task<List<BranchLocation>> GetBranchLocationId(int companyId);
        Task<int> CreateAssets(AllAssets allAssets, int sessionEmployeeId, int companyId);

    }
}

