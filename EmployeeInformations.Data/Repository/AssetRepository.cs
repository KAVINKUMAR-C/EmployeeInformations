using EmployeeInformations.Common;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class AssetRepository : IAssetRepository
    {

        private readonly EmployeesDbContext _dbContext;

        public AssetRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Asset category

        /// <summary>
        /// Logic to get assetcategory list
        /// </summary>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<AssetCategoryEntity>> GetAllAssetCategory(int companyId)
        {
            return await _dbContext.AssetCategory.Where(g => !g.IsDeleted && g.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get create the assetcategory detail
        /// </summary>
        /// <param name="assetCategoryEntity" ></param>
        public async Task<int> Create(AssetCategoryEntity assetCategoryEntity, int companyId)
        {
            var result = 0;
            if (assetCategoryEntity?.CategoryId == 0)
            {
                assetCategoryEntity.CompanyId = companyId;
                await _dbContext.AssetCategory.AddAsync(assetCategoryEntity);
                await _dbContext.SaveChangesAsync();
                result = assetCategoryEntity.CategoryId;
            }

            return result;
        }


        /// <summary>
        /// Logic to get update the assetcategory detail by particular assetcategory
        /// </summary>             
        /// <param name="assetCategoryEntity" ></param>     
        public async Task Upadate(AssetCategoryEntity assetCategoryEntity, int companyId)
        {
            try
            {
                if (assetCategoryEntity != null)
                {
                    assetCategoryEntity.CompanyId = companyId;
                    _dbContext.AssetCategory.Update(assetCategoryEntity);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
         /// Logic to get check categoryname the assetcategory detail  by particular categoryname
         /// </summary>
        /// <param name="categoryName" ></param>
        /// <param name="CompanyId" ></param> 
        public async Task<int> GetCategoryName(string categoryName,int companyId)
        {
            var categoryNameCount = await _dbContext.AssetCategory.Where(y => y.CategoryName == categoryName && y.CompanyId == companyId).CountAsync();
            return categoryNameCount;
        }


        /// <summary>
        /// Logic to get check categoryCode the assetcategory detail  by particular categoryCode
        /// </summary>
        /// <param name="categoryCode" ></param>
        /// <param name="CompanyId" ></param> 
        public async Task<int> GetCategoryCode(string categoryCode,int companyId)
        {
            var categoryCodeCount = await _dbContext.AssetCategory.Where(g => g.CategoryCode == categoryCode && g.CompanyId == companyId).CountAsync();
            return categoryCodeCount;
        }


        /// <summary>
        /// Logic to get assetcategory list
        /// </summary>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<AssetCategoryEntity>> GetAllAssetCategoryName(int companyId)
        {
            var result = await _dbContext.AssetCategory.Where(u => !u.IsDeleted && u.IsActive && u.CompanyId == companyId).ToListAsync();
            return result;
        }


        /// <summary>
        /// Logic to get create the asset detail
        /// </summary>              
        /// <param name="allAssetsEntity" ></param>
        public async Task<int> CreateAssets(AllAssetsEntity allAssetsEntity, int companyId)
        {
            var result = 0;
            var assetCategoryEntity = await _dbContext.AssetCategory.FirstOrDefaultAsync(m => m.CategoryId == allAssetsEntity.CategoryId && m.CompanyId == companyId);
            var totalAssetCount = await GetAssetsMaxCount(allAssetsEntity.CategoryId, companyId);

            if (allAssetsEntity?.AllAssetsId == 0)
            {
                assetCategoryEntity.AssetCount = totalAssetCount + 1;
                allAssetsEntity.CompanyId = companyId;
                _dbContext.AssetCategory.Update(assetCategoryEntity);
                await _dbContext.SaveChangesAsync();
                await _dbContext.AllAssets.AddAsync(allAssetsEntity);
                await _dbContext.SaveChangesAsync();
                result = allAssetsEntity.AllAssetsId;
            }
            else
            {
                if (allAssetsEntity != null)
                {
                    assetCategoryEntity.AssetCount = totalAssetCount + 1;
                    _dbContext.AssetCategory.Update(assetCategoryEntity);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.AllAssets.Update(allAssetsEntity);
                    await _dbContext.SaveChangesAsync();
                }
                result = allAssetsEntity != null ? allAssetsEntity.AllAssetsId : result;
            }
            return result;
        }


        /// <summary>
        /// Logic to get assetcategory the asset detail
        /// </summary>
        /// <param name="assetCategoryEntity" ></param>
        /// <param name="categoryId" ></param>
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task UpdateAssetCategory(AssetCategoryEntity assetCategoryEntity, int companyId)
        {
            var assetCategoryEntitys = await _dbContext.AssetCategory.FirstOrDefaultAsync(j => j.CategoryId == assetCategoryEntity.CategoryId && !j.IsDeleted && j.CompanyId == companyId);
            if (assetCategoryEntitys != null)
            {
                assetCategoryEntitys.IsActive = assetCategoryEntity.IsActive;
                _dbContext.AssetCategory.Update(assetCategoryEntitys);
                _dbContext.SaveChanges();
            }
        }


        /// <summary>
        /// Logic to get delete the assetcategory detail  by particular categoryid
        /// </summary>
        /// <param name="categoryId" ></param>
        /// <param name="CompanyId" ></param> 
        public async Task<bool> DeletedAssetCategory(int categoryId,int companyId)
        {
            var result = false;
            var assetCategoryEntity = await _dbContext.AssetCategory.FirstOrDefaultAsync(m => m.CategoryId == categoryId && m.CompanyId == companyId);
            if (assetCategoryEntity != null)
            {
                assetCategoryEntity.IsDeleted = true;
                _dbContext.AssetCategory.Update(assetCategoryEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get categoryId the assetcategory detail by particular categoryId
        /// </summary>
        /// <param name="categoryId" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<AssetCategoryEntity> GetAssetCategoryNameByCategoryId(int categoryId,int companyId)
        {
            var allAssetsEntity = await _dbContext.AssetCategory.AsNoTracking().FirstOrDefaultAsync(d => d.CategoryId == categoryId && !d.IsDeleted && d.CompanyId == companyId);
            return allAssetsEntity ?? new AssetCategoryEntity();
        }


        /// <summary>
        /// Logic to get categoryId the assetcategory detail by particular categoryId
        /// </summary>
        /// <param name="categoryId" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param> 
        public async Task<AssetCategoryEntity> GetAllAssetCategoryId(int categoryId, int companyId)
        {
            var result = await _dbContext.AssetCategory.Where(u => u.CategoryId == categoryId && !u.IsDeleted && u.IsActive && u.CompanyId == companyId).FirstOrDefaultAsync();
            return result;
        }

        //AssetType

        /// <summary>
        /// Logic to get assettype list
        /// </summary>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<AssetTypesEntity>> GetAllAssetType(int companyId)
        {
            return await _dbContext.AssetTypes.Where(l => !l.IsDeleted && l.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get assettype list
        /// </summary>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<AssetTypesEntity>> GetAssetType(int companyId)
        {
            return await _dbContext.AssetTypes.Where(l => !l.IsDeleted && l.IsActive && l.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get assettype the asset detail
          /// </summary>
        /// <param name="assetTypesEntity" ></param>        
        public async Task<int> CreateAssetType(AssetTypesEntity assetTypesEntity,int companyId)
        {
            var result = 0;
            if (assetTypesEntity?.Id == 0)
            {
                assetTypesEntity.CompanyId = companyId;
                await _dbContext.AssetTypes.AddAsync(assetTypesEntity);
                await _dbContext.SaveChangesAsync();
                result = assetTypesEntity.Id;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update the assettypes detail by particular assettypes
        /// </summary>             
        /// <param name="assetTypesEntity" ></param>     
        public async Task UpadateAssetType(AssetTypesEntity assetTypesEntity, int companyId)
        {
            if (assetTypesEntity != null)
            {
                assetTypesEntity.CompanyId = companyId;
                _dbContext.AssetTypes.Update(assetTypesEntity);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get check typename the assettype detail by particular typename
        /// </summary>
        /// <param name="typeName" ></param>        
        /// <param name="CompanyId" ></param>        
        public async Task<int> GetTypeName(string typeName, int companyId)
        {
            var typeNameCount = await _dbContext.AssetTypes.Where(y => y.TypeName == typeName && y.CompanyId == companyId).CountAsync();
            return typeNameCount;
        }


        /// <summary>
        /// Logic to get id the assettype detail by particular id
        /// </summary>
        /// <param name="id" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<AssetTypesEntity> GetAssetTypeNameById(int AssetTypeId,int companyId)
        {
            var allAssetsEntity = await _dbContext.AssetTypes.AsNoTracking().FirstOrDefaultAsync(d => d.Id == AssetTypeId && !d.IsDeleted && d.CompanyId == companyId);
            return allAssetsEntity ?? new AssetTypesEntity();
        }


        /// <summary>
        /// Logic to get delete the assettype detail by particular id
        /// </summary>
        /// <param name="id" ></param>        
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeletedAssetType(int id, int companyId)
        {
            var result = false;
            var assetTypesEntity = await _dbContext.AssetTypes.FirstOrDefaultAsync(m => m.Id == id && m.CompanyId == companyId);
            if (assetTypesEntity != null)
            {
                assetTypesEntity.IsDeleted = true;
                _dbContext.AssetTypes.Update(assetTypesEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get categoryId the AssetTypes detail 
        /// </summary>   
        /// <param name="categoryId" >AssetTypes</param>
        /// <param name="IsDeleted" >AssetTypes</param>
        public async Task<List<AssetTypesEntity>> GetBycategoryId(int categoryId)
        {
            var result = await _dbContext.AssetTypes.Where(c => c.CategoryId == categoryId && !c.IsDeleted).ToListAsync();
            return result;
        }


        /// <summary>
        /// Logic to get delete the assettype detail
        /// </summary>
        /// <param name="assetTypesEntities" ></param>                
        public async Task<bool> DeletedAssetTypes(List<AssetTypesEntity> assetTypesEntities)
        {
            var result = false;
            _dbContext.AssetTypes.UpdateRange(assetTypesEntities);
            result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }


        /// <summary>
        /// Logic to get categoryid the assettypes detail by particular categoryid
        /// </summary>
        /// <param name="categoryId" ></param>
        /// <param name="CompanyId" ></param> 
        public async Task<List<AssetTypesEntity>> GetAssetTypeIdByCategoryId(int categoryId,int companyId)
        {
            var assetTypesEntity = await _dbContext.AssetTypes.Where(m => m.CategoryId == categoryId && m.CompanyId == companyId).ToListAsync();
            return assetTypesEntity;
        }


        /// <summary>
        /// Logic to get status the assettype detail
        /// </summary>
        /// <param name="assetTypesEntity" ></param>
        /// <param name="assetTypesEntity.Id" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task UpdateAssetType(AssetTypesEntity assetTypesEntity, int companyId)
        {
            var assetTypesEntitys = await _dbContext.AssetTypes.FirstOrDefaultAsync(j => j.Id == assetTypesEntity.Id && !j.IsDeleted && j.CompanyId == companyId);
            if (assetTypesEntitys != null)
            {
                assetTypesEntitys.IsActive = assetTypesEntity.IsActive;
                _dbContext.AssetTypes.Update(assetTypesEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }

        ///All Assets

        /// <summary>
        /// Logic to get asset list
        /// </summary>       
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<AllAssetsEntity>> GetAllAssets(int companyId)
        {
            return await _dbContext.AllAssets.Where(m => !m.IsDeleted && m.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get company list
        /// </summary>              
        /// <param name="CompanyId" ></param>
        public async Task<List<CompanyEntity>> GetCompany(int companyId)
        {
            return await _dbContext.Company.Where(d => d.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get delete the asset detail
        /// </summary>
        /// <param name="allAssetsEntities" ></param>
        public async Task<bool> DeletedAllAssets(List<AllAssetsEntity> allAssetsEntities)
        {
            var result = false;
            _dbContext.AllAssets.UpdateRange(allAssetsEntities);
            result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }


        /// <summary>
        /// Logic to get empId the asset detail by particular empId
        /// </summary>
        /// <param name="empId" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<AllAssetsEntity>> GetAssetByEmployeeId(int empId, int companyId)
        {
            var allAssetsEntity = await _dbContext.AllAssets.AsNoTracking().Where(d => d.EmployeeId == empId && !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
            return allAssetsEntity;
        }


        /// <summary>
        /// Logic to get delete the asset detail by particular allAssetsId
        /// </summary>
        /// <param name="allAssetsId" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param> 
        public async Task<AllAssetsEntity> GetByAllAssetsId(int allAssetsId,int companyId)
        {
            var allAssetsEntity = await _dbContext.AllAssets.AsNoTracking().FirstOrDefaultAsync(d => d.AllAssetsId == allAssetsId && !d.IsDeleted && d.CompanyId == companyId);
            return allAssetsEntity ?? new AllAssetsEntity();
        }


        /// <summary>
        /// Logic to get assetcode count AllAssets list
        /// </summary>         
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> GetAssetsMaxCount(int categoryId, int companyId)
        {
            var count = await _dbContext.AllAssets.Where(d => d.CategoryId == categoryId && !d.IsDeleted && d.CompanyId == companyId).CountAsync();
            return count;
        }


        /// <summary>
        /// Logic to get delete the assets detail 
        /// </summary>
        /// <param name="allAssetsEntity" ></param>               
        public async Task<bool> DeleteAllAssets(AllAssetsEntity allAssetsEntity)
        {
            var result = false;
            if (allAssetsEntity != null)
            {
                _dbContext.AllAssets.Update(allAssetsEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
         /// Logic to get categoryid the asset detail by particular categoryid
         /// </summary>
        /// <param name="categoryId" ></param>
        /// <param name="CompanyId" ></param> 
        public async Task<List<AllAssetsEntity>> GetAllAssetsByCategoryId(int categoryId, int companyId)
        {
            var allAssetEntity = await _dbContext.AllAssets.Where(m => m.CategoryId == categoryId && m.CompanyId == companyId).ToListAsync();
            return allAssetEntity;
        }

        /// Asset status

        /// <summary>
        /// Logic to get statusId the assetstatus detail by particular statusId
        /// </summary>
        /// <param name="statusId" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<AssetStatusEntity> GetAssetStatusNameByName(int AssetStatusId,int companyId)
        {
            var allAssetsEntity = await _dbContext.AssetStatus.AsNoTracking().FirstOrDefaultAsync(d => d.StatusId == AssetStatusId && d.CompanyId == companyId && !d.IsDeleted);
            return allAssetsEntity ?? new AssetStatusEntity();
        }


        /// <summary>
        /// Logic to get assetstatus list
        /// </summary>              
        /// <param name="CompanyId" ></param>
        public async Task<List<AssetStatusEntity>> GetAllAssetStatus(int companyId)
        {
            return await _dbContext.AssetStatus.Where(x => x.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get empId the employeebenefitentitys detail by particular empId
        /// </summary>
        /// <param name="empId" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<EmployeeBenefitEntity>> GetBenefitByEmployeeId(int empId, int companyId)
        {
            var employeeBenefitEntitys = await _dbContext.EmployeeBenefitEntitys.AsNoTracking().Where(d => d.EmpId == empId && !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
            return employeeBenefitEntitys;
        }


        /// <summary>
        /// Logic to get AssetLog list
        /// </summary> 
        /// <param name="CompanyId" ></param>  
        public async Task<List<AssetLogEntity>> GetAllAssetLog(int companyId)
        {
            return await _dbContext.AssetLog.Where(x => x.CompanyId == companyId).ToListAsync();
        }

        //// Asset Brand Type 

        /// <summary>
        /// Logic to get create the asset Brand
        /// </summary>              
        /// <param name="CompanyId" ></param>
        public async Task<List<AssetBrandTypeEntity>> GetAllBrand(int companyId)
        {
            return await _dbContext.AssetBrandType.Where(h => !h.IsDeleted && h.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get id the brandtype detail by particular BrandTypeId
        /// </summary>
        /// <param name="BrandTypeId" ></param>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<AssetBrandTypeEntity> GetAssetBrandNameById(int BrandTypeId, int companyId)
        {
            var assetBrandTypeEntity = await _dbContext.AssetBrandType.AsNoTracking().FirstOrDefaultAsync(d => d.BrandTypeId == BrandTypeId && !d.IsDeleted && d.CompanyId == companyId);
            return assetBrandTypeEntity ?? new AssetBrandTypeEntity();
        }


        /// <summary>
        /// Logic to get assettype list
        /// </summary>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<AssetBrandTypeEntity>> GetAssetBrand(int companyId)
        {
            return await _dbContext.AssetBrandType.Where(l => !l.IsDeleted && l.IsActive && l.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get check brandType the brandType detail by particular brandType
        /// </summary>
        /// <param name="brandType" ></param>        
        /// <param name="CompanyId" ></param>        
        public async Task<int> GetBrandName(string brandType,int companyId)
        {
            var typeNameCount = await _dbContext.AssetBrandType.Where(y => y.BrandType == brandType && y.CompanyId == companyId).CountAsync();
            return typeNameCount;
        }


        /// <summary>
        /// Logic to get create the asset Brand Type
        /// </summary>              
        /// <param name="allAssetsEntity" ></param>
        public async Task<int> Create(AssetBrandTypeEntity assetBrandTypeEntity)
        {
            var result = 0;
            if (assetBrandTypeEntity?.BrandTypeId == 0)
            {
                assetBrandTypeEntity.CompanyId = assetBrandTypeEntity.CompanyId;
                await _dbContext.AssetBrandType.AddAsync(assetBrandTypeEntity);
                await _dbContext.SaveChangesAsync();
                result = assetBrandTypeEntity.BrandTypeId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get status the assetbrand detail
        /// </summary>
        /// <param name="assetBrandTypeEntity" ></param>
        /// <param name="assetBrandTypeEntity.BrandTypeId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task UpdateAssetBrand(AssetBrandTypeEntity assetBrandTypeEntity)
        {
            var assetBrandTypeEntitys = await _dbContext.AssetBrandType.FirstOrDefaultAsync(j => j.BrandTypeId == assetBrandTypeEntity.BrandTypeId && !j.IsDeleted && j.CompanyId == assetBrandTypeEntity.CompanyId);
            if (assetBrandTypeEntitys != null)
            {
                assetBrandTypeEntitys.IsActive = assetBrandTypeEntity.IsActive;
                _dbContext.AssetBrandType.Update(assetBrandTypeEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get update the assetbrand detail by particular assetbrand
        /// </summary>             
        /// <param name="assetBrandTypeEntity" ></param>     
        public async Task UpadateAssetBrandType(AssetBrandTypeEntity assetBrandTypeEntity)
        {
            if (assetBrandTypeEntity != null)
            {
                _dbContext.AssetBrandType.Update(assetBrandTypeEntity);
                await _dbContext.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Logic to get delete the assetbrand detail by particular brandTypeId
        /// </summary>
        /// <param name="id" ></param>        
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeletedAssetBrand(int brandTypeId,int companyId)
        {
            var result = false;
            var assetBrandTypeEntity = await _dbContext.AssetBrandType.FirstOrDefaultAsync(m => m.BrandTypeId == brandTypeId && m.CompanyId == companyId);
            if (assetBrandTypeEntity != null)
            {
                assetBrandTypeEntity.IsDeleted = true;
                _dbContext.AssetBrandType.Update(assetBrandTypeEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// Branch Location

        /// <summary>
         /// Logic to get branchLocation list
         /// </summary>        
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<BranchLocationEntity>> GetAllBranchLocationId(int companyId)
        {
            var result = await _dbContext.BranchLocation.Where(u => !u.IsDeleted && u.IsActive && u.CompanyId == companyId).ToListAsync();
            return result;
        }

        /// AssetLogReportDataModel

        /// <summary>
        /// Logic to get filterassetlog the assetLogReportDataModel detail 
        /// </summary>        
        /// <param name="proc,values" >assetLogReportDataModel</param>      
        public async Task<List<AssetLogReportDataModel>> GetAllAssetByEmployeeLogFilter(string proc, List<KeyValuePair<string, string>> values)
        {
            try
            {
                var parameters = new object[values.Count];
                for (int i = 0; i < values.Count; i++)
                    parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

                var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
                paramnames = paramnames.TrimEnd(',');
                proc = proc + " " + paramnames;

                var leaveReportDateModel = await _dbContext.AssetLogReportDataModel.FromSqlRaw<AssetLogReportDataModel>(proc, parameters).AsNoTracking().ToListAsync();
                return leaveReportDateModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logic to get all asset details list
        /// </summary>
        public async Task<List<AllAssets>> GetAllAsset(int companyId)
        {
            var allAsset = await (from asset in _dbContext.AllAssets
                                  join assetType in _dbContext.AssetTypes on asset.AssetTypeId equals assetType.Id
                                  join assetCategory in _dbContext.AssetCategory on asset.CategoryId equals assetCategory.CategoryId
                                  join brand in _dbContext.AssetBrandType on asset.BrandId equals brand.BrandTypeId
                                  join emp in _dbContext.Employees on asset.EmployeeId equals emp.EmpId
                                  into employeedetails
                                  from employee in employeedetails.DefaultIfEmpty()
                                  join location in _dbContext.BranchLocation on asset.LocationId equals location.BranchLocationId
                                  into branchlocation
                                  from branch in branchlocation.DefaultIfEmpty()
                                  join status in _dbContext.AssetStatus on asset.AssetStatusId equals status.StatusId
                                  into assetStatus
                                  from statusasset in assetStatus.DefaultIfEmpty()
                                  where (!asset.IsDeleted && companyId == asset.CompanyId && !brand.IsDeleted &&
                                  !assetCategory.IsDeleted && !assetType.IsDeleted)
                                  select new AllAssets()
                                  {
                                      AssetStatusName = statusasset.StatusName,
                                      AssetEmployeeName = employee.FirstName + " " + employee.LastName + " - " + employee.UserName,
                                      LocationName = branch.BranchLocationName,
                                      AssetTypeName = assetType.TypeName,
                                      AssetCategoryName = assetCategory.CategoryName,
                                      brandName = brand.BrandType,
                                      AssetName = asset.AssetName,
                                      IssueDate = asset.IssueDate,
                                      AssetCode = asset.AssetCode,
                                      AllAssetsId = asset.AllAssetsId,
                                  }).ToListAsync();
            return allAsset;
        }

        /// <summary>
        /// Logic to get all asset category details list
        /// </summary>
        public async Task <List<AssetCategory>>GetAllAssetCategorys(int companyId)
        {
            var allCategory = await ( from  category in _dbContext.AssetCategory
                                where !category.IsDeleted && category.CompanyId == companyId
                                      select new AssetCategory()
                                {
                                    CategoryId = category.CategoryId,
                                    CategoryCode = category.CategoryCode,
                                    CategoryName = category.CategoryName,
                                    IsActive = category.IsActive,
                                }).ToListAsync();
            return allCategory;
        }

        /// <summary>
        /// Logic to get all asset type details list
        /// </summary>
        public async Task<List<AssetTypes>> GetAllAssetTypes(int companyId)
        {
              var allAssetType = await (from type in _dbContext.AssetTypes
                                       join category in _dbContext.AssetCategory on type.CategoryId equals category.CategoryId
                                       where !type.IsDeleted && !category.IsDeleted && category.IsActive && companyId == type.CompanyId
                                       select new AssetTypes()
                                       {
                                           Id = type.Id,
                                           CategoryId = category.CategoryId,
                                           TypeName = type.TypeName,
                                           IsActive = type.IsActive,
                                           AssetCategoryName = category.CategoryName,
                                           
                                       }).ToListAsync();
                return allAssetType;          
        }

        /// <summary>
        /// Logic to get all asset brand type details list
        /// </summary>
        public async Task<List<AssetBrandType>> GetAllAssetBrandTypes(int companyId)
        {
            var allBrandType = await (from brandType in _dbContext.AssetBrandType
                                      where !brandType.IsDeleted && companyId == brandType.CompanyId
                                      select new AssetBrandType()
                                      {
                                          BrandTypeId = brandType.BrandTypeId,
                                          BrandType = brandType.BrandType,
                                          IsActive= brandType.IsActive,
                                          CompanyId = brandType.CompanyId,
                                      }).ToListAsync();
            return allBrandType;
        }

        /// <summary>
        /// Logic to get asset list count check 
        /// </summary>        
        /// <param name="pager" >asset</param>      
        public async Task<int> GetAssetListCount(SysDataTablePager pager,int companyId )
        {
          
            if (pager.iDisplayStart >= pager.iDisplayLength)
            {
                pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
            }
            var _params = new
            {
                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = pager.sSearch
            };
            var paramcompany = new NpgsqlParameter("@companyId", companyId);
            var param = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            List<AssetCount> employeeCounts = await _dbContext.AssetCounts.FromSqlRaw("EXEC [dbo].[spGetAssetFilterListCount] @companyId,@searchText", paramcompany, param).ToListAsync();
            foreach (var item in employeeCounts)
            {
                var result = item.AssetCountId;
                return result;
            }
            return 0;
        }

        /// <summary>
        /// Logic to get asset list details  
        /// </summary>        
        /// <param name="pager,columnDirection,columnName" >asset</param> 
        public async Task<List<AssetViewModels>> GetAssetDetailsList(SysDataTablePager pager, string columnDirection, string columnName,int companyId)
        {
            try
            {
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection,
                };
                var paramcompany = new NpgsqlParameter("@companyId", companyId);
                var param1 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
                var employeeList = await _dbContext.AssetModels.FromSqlRaw("EXEC [dbo].[spGetAssetFilterList] @companyId,@pagingSize ,@offsetValue,@searchText,@sorting", paramcompany, param1, param2, param3, param4).ToListAsync();
                return employeeList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Logic to get asset Log list count details  
        /// </summary>        
        /// <param name="pager,assetLogViewModel,companyId" >asset</param> 
        public async Task<int> GetAllEmployeesAssetLogCount(SysDataTablePager pager, AssetLogViewModel assetLogViewModel,int companyId)
        {
            try
            {                              
                var assetName = "";
                if (assetLogViewModel.AssetNo == Common.Constant.AllModules)
                {
                    assetName = "";
                }
                else
                {
                    assetName = assetLogViewModel.AssetNo;
                }
                var dFrom = string.IsNullOrEmpty(assetLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.StartDate).ToString(Constant.DateFormatYMDHyphen);
                var dTo = string.IsNullOrEmpty(assetLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.EndDate).ToString(Constant.DateFormatYMDHyphen);

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };

                var param = new NpgsqlParameter("@empId", assetLogViewModel.EmpId);
                var param1 = new NpgsqlParameter("@AssetNo", assetName);
                var param2 = new NpgsqlParameter("@startDate", dFrom);
                var param3 = new NpgsqlParameter("@endDate", dTo);
                var param4 = new NpgsqlParameter("@companyId", companyId);
                var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                List<AssetCount> assetCounts = await _dbContext.AssetCounts.FromSqlRaw("EXEC [dbo].[spGetAssetLogFilterListCount] @companyId,@empId,@AssetNo,@startDate, @endDate,@searchText", param4, param, param1, param2, param3, param5).ToListAsync();
                foreach (var item in assetCounts)
                {
                    var result = item.AssetCountId;
                    return result;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Logic to get asset Log filter list details  
        /// </summary>        
        /// <param name="pager,assetLogViewModel,companyId" >asset</param> 
        public async Task<List<AssetsLogModel>> GetAllEmployeesAssetLogList(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId)
        {
            try
            {                            
                var lastDate = DateTime.Today.AddDays(-2).Date;
                var today = DateTime.Now.Date;
                assetLogViewModel.StartDate = lastDate.Date.ToString(Constant.DateFormat);
                assetLogViewModel.EndDate = today.Date.ToString(Constant.DateFormat);
                var assetName = "";
                var dFrom = string.IsNullOrEmpty(assetLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.StartDate).ToString(Constant.DateFormatMDY);
                var dTo = string.IsNullOrEmpty(assetLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.EndDate).ToString(Constant.DateFormatMDY);

                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                var _params = new
                {
                    OffsetValue = pager.sEcho,
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = assetLogViewModel.ColumnName + " " + assetLogViewModel.ColumnDirection
                };
                var empId = 0;
                var assetLogViewModels = await AssetLogSpParameters(empId, assetName, dFrom, dTo, companyId, pager, _params.Sorting);

                return assetLogViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logic to get asset Log list details  
        /// </summary>        
        /// <param name="empId,assetName,dFrom,dTo,companyId,pager,sort" >asset</param> 
        public async Task<List<AssetsLogModel>> AssetLogSpParameters(int empId, string assetName, string dFrom, string dTo, int companyId, SysDataTablePager pager, string sort)
        {
            var param = new NpgsqlParameter("@empId", empId);
            var param1 = new NpgsqlParameter("@AssetNo", assetName);
            var param2 = new NpgsqlParameter("@startDate", dFrom);
            var param3 = new NpgsqlParameter("@endDate", dTo);
            var param4 = new NpgsqlParameter("@companyId", companyId);
            var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
            var param6 = new NpgsqlParameter("@offsetValue", pager.sEcho);
            var param7 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
            var param8 = new NpgsqlParameter("@sorting", sort);

            var assetLogViewModels = await _dbContext.AssetsLogModels.FromSqlRaw("EXEC [dbo].[spGetAssetLogFilterDataList] @empId,@AssetNo,@startDate, @endDate, @companyId, @searchText,@offsetValue,@pagingSize,@sorting", param, param1, param2, param3, param4, param5, param6, param7, param8).ToListAsync();
            return assetLogViewModels;
        }

        /// <summary>
        /// Logic to get asset Log filter list details  
        /// </summary>        
        /// <param name="pager,assetLogViewModel,companyId" >asset</param> 
        public async Task<List<AssetsLogModel>> GetAllEmployessByAssetLogFilter(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId)
        {
            try
            {                                        
                var assetName = "";
                if (assetLogViewModel.AssetNo == Common.Constant.AllModules)
                {
                    assetName = "";
                }
                else
                {
                    assetName = assetLogViewModel.AssetNo;
                }
                var dFrom = string.IsNullOrEmpty(assetLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.StartDate).ToString(Constant.DateFormatYMDHyphen);
                var dTo = string.IsNullOrEmpty(assetLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.EndDate).ToString(Constant.DateFormatYMDHyphen);
                if (pager.iDisplayLength != 0)
                {
                    if (pager.iDisplayStart >= pager.iDisplayLength)
                    {
                        pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                    }
                }
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var sort = assetLogViewModel.ColumnName + " " + assetLogViewModel.ColumnDirection;
                if (assetLogViewModel.EmpId == 0 && pager.iDisplayLength != 0)
                {                  
                    var filteredAssetLog = await AssetLogSpParameters(assetLogViewModel.EmpId, assetName, dFrom, dTo, companyId, pager, sort);
                    return filteredAssetLog;
                }
                else if (assetLogViewModel.EmpId == 0 && pager.iDisplayLength == 0)
                {                   
                    var filteredAssetLog = await AssetLogSpParameters(assetLogViewModel.EmpId, assetName, dFrom, dTo, companyId, pager, sort);
                    return filteredAssetLog;
                }
                else
                {                   
                    var filteredAssetLog = await AssetLogSpParameters(assetLogViewModel.EmpId, assetName, dFrom, dTo, companyId, pager, sort);
                    return filteredAssetLog;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
