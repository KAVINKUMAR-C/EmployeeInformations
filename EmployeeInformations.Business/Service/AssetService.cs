using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.AttendanceViewModel;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.X509;


namespace EmployeeInformations.Business.Service
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAuditLogRepository _auditLogRepository;


        public AssetService(IAssetRepository assetRepository, IEmployeesRepository employeesRepository, IMapper mapper, IConfiguration config, IAuditLogRepository auditLogRepository)
        {
            _assetRepository = assetRepository;
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _config = config;
            _auditLogRepository = auditLogRepository;
        }

        /// <summary>
        /// Logic to get assetcategory list
        /// </summary>        
        public async Task<AssetCategoryViewModal> GetAllAssetCategory(int companyId)
        {
            var assetCategoryViewModal = new AssetCategoryViewModal();
            assetCategoryViewModal.AssetCategory = await _assetRepository.GetAllAssetCategorys(companyId);
            return assetCategoryViewModal;
        }

        /// <summary>
        /// Logic to get create the assetcategory detail
        /// </summary>
        /// <param name="assetCategory" ></param>              
        public async Task<AssetCategory> Create(AssetCategory assetCategory, int companyId)
        {
            var assetCategorys = new AssetCategory();
            var totalcategoryName = await _assetRepository.GetCategoryName(assetCategory.CategoryName, assetCategory.CategoryId);

            if (totalcategoryName == 0)
            {
                var totalcategoryCode = await _assetRepository.GetCategoryCode(assetCategory.CategoryCode, assetCategory.CategoryId);
                if (totalcategoryCode == 0)
                {
                    if (assetCategory?.CategoryId == 0)
                    {
                        var assetCategoryEntity = _mapper.Map<AssetCategoryEntity>(assetCategory);
                        assetCategoryEntity.IsActive = true;
                        var datas = await _assetRepository.Create(assetCategoryEntity, companyId);
                        assetCategorys.CategoryId = assetCategoryEntity.CategoryId;
                    }
                }
                else
                {
                    assetCategorys.CategoryCodeCount = totalcategoryCode;
                }
            }
            else
            {
                assetCategorys.CategoryNameCount = totalcategoryName;
            }

            return assetCategorys;
        }




        /// <summary>
        /// Logic to get update the assetcategory detail
        /// </summary>
        /// <param name="assetCategory" ></param>
        public async Task<int> Upadate(AssetCategory assetCategory, int companyId)
        {
            var assetCategoryEntity = _mapper.Map<AssetCategoryEntity>(assetCategory);
            await _assetRepository.Upadate(assetCategoryEntity, companyId);
            var leaveType = assetCategoryEntity.CategoryId;
            return leaveType;
        }

        /// <summary>
        /// Logic to get check categoryname the assetcategory detail  by particular categoryname not allow repeated categoryname        
        /// </summary>
        /// <param name="categoryName" ></param> 
        public async Task<int> GetCategoryName(string categoryName, int companyId)
        {
            var totalcategoryName = await _assetRepository.GetCategoryName(categoryName, companyId);
            return totalcategoryName;
        }

        /// <summary>
        /// Logic to get check categoryCode the assetcategory detail  by particular categoryCode not allow repeated categoryCode
        /// </summary>
        /// <param name="categoryCode" ></param>         
        public async Task<int> GetCategoryCode(string categoryCode, int companyId)
        {
            var totalcategoryCode = await _assetRepository.GetCategoryCode(categoryCode, companyId);
            return totalcategoryCode;
        }

        /// <summary>
        /// Logic to get check categoryId the assetcategory detail  by particular categoryId 
        /// </summary>
        /// <param name="categoryId" ></param> 
        public async Task<bool> DeletedAssetCategory(int categoryId,int companyId)
        {
            var result = await _assetRepository.DeletedAssetCategory(categoryId, companyId);
            var assetTypesEntity = await _assetRepository.GetAssetTypeIdByCategoryId(categoryId,companyId);
            var assetsEntity = await _assetRepository.GetAllAssetsByCategoryId(categoryId,companyId);

            if (assetTypesEntity != null)
            {
                foreach (var item in assetTypesEntity)
                {
                    item.IsDeleted = true;
                }

                var results = await _assetRepository.DeletedAssetTypes(assetTypesEntity);
            }

            if (assetsEntity != null)
            {
                foreach (var item in assetsEntity)
                {
                    item.IsDeleted = true;
                }

                var results = await _assetRepository.DeletedAllAssets(assetsEntity);
            }

            return result;
        }

        /// <summary>
        /// Logic to get update status the assetcategory detail by particular assetCategory status 
        /// </summary>
        /// <param name="assetCategory" ></param>      
        public async Task<int> UpdateAssetCategory(AssetCategory assetCategory, int companyId)
        {
            var assetCategoryEntity = _mapper.Map<AssetCategoryEntity>(assetCategory);
            await _assetRepository.UpdateAssetCategory(assetCategoryEntity, companyId);
            var result = assetCategoryEntity.CategoryId;
            return result;
        }

        //AssetType

        /// <summary>
        /// Logic to get assettypelist
        /// </summary> 
        public async Task<AssetTypeViewModal> GetAllAssetType(int companyId)
        {
            var listOfAssetTypeViewModal = new AssetTypeViewModal();
            listOfAssetTypeViewModal.AssetTypes = await _assetRepository.GetAllAssetTypes(companyId);
            return listOfAssetTypeViewModal;
        }

        /// <summary>
        /// Logic to get create the assetTypes detail
        /// </summary>
        /// <param name="assetTypes" ></param>         
        public async Task<AssetTypes> CreateAssetType(AssetTypes assetTypes,int companyId)
        {
            var assetType = new AssetTypes();
            var totaltypeName = await _assetRepository.GetTypeName(assetTypes.TypeName,companyId);
            if (totaltypeName == 0)
            {
                if (assetTypes?.Id == 0)
                {
                    var assetTypesEntity = _mapper.Map<AssetTypesEntity>(assetTypes);
                    assetTypesEntity.IsActive = true;
                    var data = await _assetRepository.CreateAssetType(assetTypesEntity,companyId);
                    assetType.Id = assetTypesEntity.Id;
                }
            }
            else
            {
                assetType.TypeNameCount = totaltypeName;
            }
            return assetType;
        }


        /// <summary>
        /// Logic to get update the assetTypes detail
        /// </summary>
        /// <param name="assetTypes" ></param> 
        public async Task<int> UpadateAssetType(AssetTypes assetTypes, int companyId)
        {
            var assetTypesEntity = _mapper.Map<AssetTypesEntity>(assetTypes);
            await _assetRepository.UpadateAssetType(assetTypesEntity,companyId);
            var leaveType = assetTypesEntity.Id;
            return leaveType;
        }

        /// <summary>
        /// Logic to get list of assetcategory only use for assetTypes
        /// </summary> 
        public async Task<List<AssetCategoryName>> GetAllAssetCategoryName(int companyId)
        {
            var listofCategoryName = new List<AssetCategoryName>();
            var listofCategory = await _assetRepository.GetAllAssetCategoryName(companyId);
            if (listofCategory != null)
            {
                listofCategoryName = _mapper.Map<List<AssetCategoryName>>(listofCategory);
            }
            return listofCategoryName;
        }

        /// <summary>
        /// Logic to get check typeName the assettype detail  by particular typeName not allow repeated typeName
        /// </summary>
        /// <param name="typeName" ></param> 
        public async Task<int> GetTypeName(string typeName,int companyId)
        {
            var totaltypeName = await _assetRepository.GetTypeName(typeName,companyId);
            return totaltypeName;
        }

        /// <summary>
        /// Logic to get delete the assettype detail by particular id
        /// </summary>
        /// <param name="id" ></param> 
        public async Task<bool> DeletedAssetType(int id, int companyId)
        {
            var result = await _assetRepository.DeletedAssetType(id,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get update status the assetTypes detail by particular assetTypes status
        /// </summary>
        /// <param name="assetTypes" ></param> 
        public async Task<int> UpdateAssetType(AssetTypes assetTypes, int companyId)
        {
            var assetTypesEntity = _mapper.Map<AssetTypesEntity>(assetTypes);
            await _assetRepository.UpdateAssetType(assetTypesEntity,companyId);
            var result = assetTypesEntity.Id;
            return result;
        }


        //AllAssets

        /// <summary>
        /// Logic to get asset
        /// </summary> 
        public async Task<List<AllAssets>> GetAllAssets(int companyId)
        {
            var listOfAllAsset = new List<AllAssets>();
            listOfAllAsset = await _assetRepository.GetAllAsset(companyId);
            return listOfAllAsset;
        }

        /// <summary>
        /// Logic to get assetStatus
        /// </summary>
        public async Task<List<AssetStatus>> GetAllAssetStatus(int companyId)
        {
            var listOfAssetStatus = new List<AssetStatus>();
            var listAssetStatus = await _assetRepository.GetAllAssetStatus(companyId);
            if (listAssetStatus != null)
            {
                listOfAssetStatus = _mapper.Map<List<AssetStatus>>(listAssetStatus);
            }
            return listOfAssetStatus;
        }

        /// <summary>
        /// Logic to get list of assetcategory only use for asset
        /// </summary>
        public async Task<List<AssetCategory>> GetAssetCategory(int companyId)
        {
            var listOfAssetCategory = new List<AssetCategory>();
            var listAssetCategory = await _assetRepository.GetAllAssetCategoryName(companyId);
            if (listAssetCategory != null)
            {
                listOfAssetCategory = _mapper.Map<List<AssetCategory>>(listAssetCategory);
            }
            return listOfAssetCategory;
        }

        /// <summary>
        /// Logic to get list of assettype only use for asset
        /// </summary>
        public async Task<List<AssetTypes>> GetAssetType(int companyId)
        {
            var listOfAssetType = new List<AssetTypes>();
            var listAssetType = await _assetRepository.GetAssetType(companyId);
            if (listAssetType != null)
            {
                listOfAssetType = _mapper.Map<List<AssetTypes>>(listAssetType);
            }
            return listOfAssetType;
        }

        /// <summary>
        /// Logic to get list of assetbrand
        /// </summary>
        public async Task<List<AssetBrandType>> GetAssetBrand(int companyId)
        {
            var listOfAssetType = new List<AssetBrandType>();
            var listAssetType = await _assetRepository.GetAssetBrand(companyId);
            if (listAssetType != null)
            {
                listOfAssetType = _mapper.Map<List<AssetBrandType>>(listAssetType);
            }
            return listOfAssetType;
        }

        /// <summary>
        /// Logic to get list of assetbrand only use for asset
        /// </summary>
        public async Task<List<BranchLocation>> GetBranchLocationId(int companyId)
        {
            var listOfBranchLocation = new List<BranchLocation>();
            var listBranchLocation = await _assetRepository.GetAllBranchLocationId(companyId);
            if (listBranchLocation != null)
            {
                listOfBranchLocation = _mapper.Map<List<BranchLocation>>(listBranchLocation);
            }
            return listOfBranchLocation;
        }

        /// <summary>
        /// Logic to get create and update  the asset detail
        /// </summary>              
        /// <param name="allAssets" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> CreateAssets(AllAssets allAssets, int sessionEmployeeId,int companyId)
        {
            var result = 0;
            if (allAssets != null)
            {
                if (allAssets.AllAssetsId == 0)
                {
                    allAssets.CreatedBy = sessionEmployeeId;
                    allAssets.CompanyId = companyId;
                    allAssets.CreatedDate = DateTime.Now;
                    allAssets.PurchaseDate = string.IsNullOrEmpty(allAssets.StrPurchaseDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrPurchaseDate);
                    allAssets.WarrantyStartDate = string.IsNullOrEmpty(allAssets.StrWarrantyDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyDate);
                    allAssets.ReturnDate = string.IsNullOrEmpty(allAssets.StrReturnDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrReturnDate);
                    allAssets.IssueDate = string.IsNullOrEmpty(allAssets.StrIssueDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrIssueDate);
                    allAssets.WarrantyEndDate = string.IsNullOrEmpty(allAssets.StrWarrantyToDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyToDate);
                    var allAssetsEntity = _mapper.Map<AllAssetsEntity>(allAssets);
                    await _assetRepository.CreateAssets(allAssetsEntity,companyId);
                    result = allAssetsEntity.AllAssetsId;

                    await InsertAssetLog(allAssets.AssetName, allAssets.AssetCode, allAssets.AssetTypeId, result, Common.Constant.Add, sessionEmployeeId,companyId);
                }
                else
                {
                    var allAssetsEntity = await _assetRepository.GetByAllAssetsId(allAssets.AllAssetsId, companyId);
                    var changeLogs = await GetDiffrencetFieldName(allAssetsEntity, allAssets, companyId);
                    allAssetsEntity.UpdatedDate = DateTime.Now;
                    allAssetsEntity.UpdatedBy = sessionEmployeeId;
                    allAssetsEntity.PurchaseDate = string.IsNullOrEmpty(allAssets.StrPurchaseDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrPurchaseDate);
                    allAssetsEntity.WarrantyStartDate = string.IsNullOrEmpty(allAssets.StrWarrantyDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyDate);
                    allAssetsEntity.WarrantyEndDate = string.IsNullOrEmpty(allAssets.StrWarrantyToDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyToDate);
                    allAssetsEntity.ReturnDate = string.IsNullOrEmpty(allAssets.StrReturnDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrReturnDate);
                    allAssetsEntity.IssueDate = string.IsNullOrEmpty(allAssets.StrIssueDate) ? null : DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrIssueDate);
                    //allAssetsEntity.WarrantyDate = DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyDate);
                    //allAssetsEntity.ReturnDate = DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrReturnDate);
                    //allAssetsEntity.IssueDate = DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrIssueDate);
                    allAssetsEntity.AssetTypeId = allAssets.AssetTypeId;
                    allAssetsEntity.CategoryId = allAssets.CategoryId;
                    allAssetsEntity.AssetCode = allAssets.AssetCode;
                    allAssetsEntity.LocationId = allAssets.LocationId;
                    allAssetsEntity.EmployeeId = allAssets.EmployeeId;
                    allAssetsEntity.Remark = allAssets.Remark;
                    allAssetsEntity.AssetName = allAssets.AssetName;
                    allAssetsEntity.Description = allAssets.Description;
                    allAssetsEntity.AssetStatusId = allAssets.AssetStatusId;
                    allAssetsEntity.ProductNumber = allAssets.ProductNumber;
                    allAssetsEntity.BrandId = allAssets.BrandId;
                    allAssetsEntity.ModelNumber = allAssets.ModelNumber;
                    allAssetsEntity.PurchaseNumber = allAssets.PurchaseNumber;
                    allAssetsEntity.InvoiceNumber = allAssets.InvoiceNumber;
                    allAssetsEntity.PurchaseOrder = allAssets.PurchaseOrder;
                    allAssetsEntity.VendorName = allAssets.VendorName;
                    allAssets.CompanyId = companyId;
                    result = await _assetRepository.CreateAssets(allAssetsEntity, companyId);

                    await InsertAssetLog(allAssets.AssetName, allAssets.AssetCode, allAssets.AllAssetsId, allAssets.AssetTypeId, Common.Constant.Update, sessionEmployeeId, companyId, changeLogs);
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get create,delete and update  the InsertAssetLog
        /// </summary>              
        /// <param name="assetName" ></param>
        /// <param name="assetCode" ></param>
        /// <param name="assetId" ></param>
        /// <param name="assetTypeId" ></param>
        /// <param name="eventName" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="changeLogs" ></param>
        public async Task<bool> InsertAssetLog(string assetName, string assetCode, int assetId, int assetTypeId, string eventName, int sessionEmployeeId, int companyId, List<ChangeLog> changeLogs = null)
        {
            var assetLogEntitys = new List<AssetLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Delete)
            {
                var assetLogEntity = new AssetLogEntity();
                assetLogEntity.AssetId = assetId;
                assetLogEntity.AssetNo = assetCode;
                assetLogEntity.AssetType = assetTypeId;
                assetLogEntity.AssetName = assetName;
                assetLogEntity.Event = eventName;
                assetLogEntity.EmpId = sessionEmployeeId;
                assetLogEntity.CreatedBy = sessionEmployeeId;
                assetLogEntity.CreatedDate = DateTime.Now;
                assetLogEntitys.Add(assetLogEntity);
            }
            else
            {
                foreach (var item in changeLogs)
                {
                    var assetLogEntity = new AssetLogEntity();
                    assetLogEntity.AssetId = assetId;
                    assetLogEntity.AssetName = assetName;
                    assetLogEntity.AssetNo = assetCode;
                    assetLogEntity.AssetType = assetTypeId;
                    assetLogEntity.Event = eventName;
                    assetLogEntity.FieldName = item.FieldName;
                    assetLogEntity.PreviousValue = item.PreviousValue;
                    assetLogEntity.NewValue = item.NewValue;
                    assetLogEntity.EmpId = sessionEmployeeId;
                    assetLogEntity.CreatedBy = sessionEmployeeId;
                    assetLogEntity.CreatedDate = DateTime.Now;
                    assetLogEntitys.Add(assetLogEntity);
                }
            }
            var result = await _auditLogRepository.InsertAssetAuditLog(assetLogEntitys, companyId);
            return result;
        }


        /// <summary>
        /// Logic to get ChangeLog all propertys
        /// </summary> 
        /// <param name="allAssetsEntity" ></param>
        /// <param name="allAssets" ></param>
        public async Task<List<ChangeLog>> GetDiffrencetFieldName(AllAssetsEntity allAssetsEntity, AllAssets allAssets,int companyId)
        {
            var changeLogs = new List<ChangeLog>();
            if (allAssetsEntity != null)
            {
                var result = string.Empty;
                if (allAssetsEntity.AssetName != allAssets.AssetName)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.AssetName;
                    changeLog.PreviousValue = allAssetsEntity.AssetName;
                    changeLog.NewValue = allAssets.AssetName;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.AssetCode != allAssets.AssetCode)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.AssetCode;
                    changeLog.PreviousValue = allAssetsEntity.AssetCode;
                    changeLog.NewValue = allAssets.AssetCode;
                    changeLogs.Add(changeLog);
                }
                //if (allAssetsEntity.Entity != allAssets.Entity)
                //{
                //    var changeLog = new ChangeLog();
                //    changeLog.FieldName = "Entity";
                //    changeLog.PreviousValue = allAssetsEntity.Entity;
                //    changeLog.NewValue = allAssets.Entity;
                //    changeLogs.Add(changeLog);
                //}

                if (allAssetsEntity.LocationId != allAssets?.LocationId)
                {
                    var assetState = await _assetRepository.GetAllBranchLocationId(companyId);
                    var stateEntity = allAssetsEntity?.LocationId == 0 ? null : assetState.FirstOrDefault(m => m.BranchLocationId == allAssetsEntity.LocationId);
                    var assetStateEntity = allAssets?.LocationId == 0 ? null : assetState.FirstOrDefault(o => o.BranchLocationId == allAssets.LocationId);
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.Location;
                    changeLog.PreviousValue = stateEntity == null ? " " : stateEntity.BranchLocationName;
                    changeLog.NewValue = assetStateEntity == null ? " " : assetStateEntity.BranchLocationName;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.PurchaseDate != (!string.IsNullOrEmpty(allAssets.StrPurchaseDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrPurchaseDate) : null))
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.PurchaseDate;
                    changeLog.PreviousValue = allAssetsEntity.PurchaseDate?.ToString(Constant.DateFormat);
                    // changeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrPurchaseDate).ToString("dd-MM-yyyy");
                    changeLog.NewValue = !string.IsNullOrEmpty(allAssets.StrPurchaseDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrPurchaseDate).ToString(Constant.DateFormat) : null;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.WarrantyStartDate != (!string.IsNullOrEmpty(allAssets.StrWarrantyDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyDate) : null))
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.WarrantyStartDate;
                    changeLog.PreviousValue = allAssetsEntity.WarrantyStartDate?.ToString(Constant.DateFormat);
                    // changeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyDate).ToString("dd-MM-yyyy");
                    changeLog.NewValue = !string.IsNullOrEmpty(allAssets.StrWarrantyDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyDate).ToString(Constant.DateFormat) : null;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.ReturnDate != (!string.IsNullOrEmpty(allAssets.StrReturnDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrReturnDate) : null))
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.ReturnDate;
                    changeLog.PreviousValue = allAssetsEntity.ReturnDate?.ToString(Constant.DateFormat);
                    //changeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrReturnDate).ToString("dd-MM-yyyy");
                    changeLog.NewValue = !string.IsNullOrEmpty(allAssets.StrReturnDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrReturnDate).ToString(Constant.DateFormat) : null;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.Description != allAssets.Description)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.Description;
                    changeLog.PreviousValue = allAssetsEntity.Description;
                    changeLog.NewValue = allAssets.Description;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.AssetStatusId != allAssets.AssetStatusId)
                {
                    var assetStatus = await _assetRepository.GetAllAssetStatus(companyId);
                    var assetStatusEntity = allAssetsEntity.AssetStatusId == null ? null : assetStatus.FirstOrDefault(d => d.StatusId == allAssetsEntity.AssetStatusId);
                    var statusEntity = allAssets.AssetStatusId == null ? null : assetStatus.FirstOrDefault(s => s.StatusId == allAssets.AssetStatusId);
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.AssetStatusId;
                    changeLog.PreviousValue = assetStatusEntity == null ? " " : assetStatusEntity.StatusName;
                    changeLog.NewValue = statusEntity == null ? " " : statusEntity.StatusName;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.BrandId != allAssets.BrandId)
                {
                    var assetbrand = await _assetRepository.GetAllBrand(companyId);
                    var assetbrandEntity = allAssetsEntity.BrandId == null ? null : assetbrand.FirstOrDefault(f => f.BrandTypeId == allAssetsEntity.BrandId);
                    var brandEntity = allAssets.BrandId == null ? null : assetbrand.FirstOrDefault(t => t.BrandTypeId == allAssets.BrandId);
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.BrandId;
                    changeLog.PreviousValue = assetbrandEntity == null ? " " : assetbrandEntity.BrandType;
                    changeLog.NewValue = brandEntity == null ? " " : brandEntity.BrandType;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.EmployeeId != allAssets.EmployeeId)
                {
                    var assetEmployee = await _employeesRepository.GetAllEmployees(companyId);
                    var employeesEntity = allAssetsEntity.EmployeeId == null ? null : assetEmployee.FirstOrDefault(g => g.EmpId == allAssetsEntity.EmployeeId);
                    var assetsEntity = allAssets.EmployeeId == null ? null : assetEmployee.FirstOrDefault(l => l.EmpId == allAssets.EmployeeId);
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.EmployeeId;
                    changeLog.PreviousValue = employeesEntity == null ? " " : employeesEntity.FirstName;
                    changeLog.NewValue = assetsEntity == null ? " " : assetsEntity.FirstName;
                    changeLogs.Add(changeLog);

                }
                if (allAssetsEntity.IssueDate != (!string.IsNullOrEmpty(allAssets.StrIssueDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrIssueDate) : null))
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.IssueDate;
                    changeLog.PreviousValue = allAssetsEntity.IssueDate?.ToString(Constant.DateFormat);
                    // changeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrIssueDate).ToString("dd/MM/yyyy");
                    changeLog.NewValue = !string.IsNullOrEmpty(allAssets.StrIssueDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrIssueDate).ToString(Constant.DateFormat) : null;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.Remark != allAssets.Remark)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.Remark;
                    changeLog.PreviousValue = allAssetsEntity.Remark;
                    changeLog.NewValue = allAssets.Remark;
                    changeLogs.Add(changeLog);
                }
                if (allAssetsEntity.ProductNumber != allAssets.ProductNumber)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.ProductNumber;
                    changeLog.PreviousValue = allAssetsEntity.ProductNumber;
                    changeLog.NewValue = allAssets.ProductNumber;
                    changeLogs.Add(changeLog);
                }

                if (allAssetsEntity.ModelNumber != allAssets.ModelNumber)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.ModelNumber;
                    changeLog.PreviousValue = allAssetsEntity.ModelNumber;
                    changeLog.NewValue = allAssets.ModelNumber;
                    changeLogs.Add(changeLog);
                }
                //if (allAssetsEntity.PurchaseNumber != allAssets.PurchaseNumber)
                //{
                //    var changeLog = new ChangeLog();
                //    changeLog.FieldName = Common.Constant.PurchaseNumber;
                //    changeLog.PreviousValue = allAssetsEntity.PurchaseNumber;
                //    changeLog.NewValue = allAssets.PurchaseNumber;
                //    changeLogs.Add(changeLog);
                //}
                if (allAssetsEntity.PurchaseNumber != allAssets.PurchaseNumber)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.PurchaseNumber;
                    result = allAssetsEntity.PurchaseNumber == 1 ? Common.Constant.PurchaseOrder : Common.Constant.InvoiceNumber;
                    changeLog.PreviousValue = result;
                    result = allAssets.PurchaseNumber == 1 ? Common.Constant.InvoiceNumber : Common.Constant.InvoiceNumber;
                    changeLog.NewValue = result;
                    changeLogs.Add(changeLog);
                }

                if (allAssetsEntity.PurchaseOrder != allAssets.PurchaseOrder)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.PurchaseOrder;
                    changeLog.PreviousValue = allAssetsEntity.PurchaseOrder;
                    changeLog.NewValue = allAssets.PurchaseOrder;
                    changeLogs.Add(changeLog);
                }

                if (allAssetsEntity.InvoiceNumber != allAssets.InvoiceNumber)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.InvoiceNumber;
                    changeLog.PreviousValue = allAssetsEntity.InvoiceNumber;
                    changeLog.NewValue = allAssets.InvoiceNumber;
                    changeLogs.Add(changeLog);
                }

                if (allAssetsEntity.VendorName != allAssets.VendorName)
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.InvoiceNumber;
                    changeLog.PreviousValue = allAssetsEntity.VendorName;
                    changeLog.NewValue = allAssets.VendorName;
                    changeLogs.Add(changeLog);
                }

                if (allAssetsEntity.AssetTypeId != allAssets.AssetTypeId)
                {
                    var assetType = await _assetRepository.GetAllAssetType(companyId);
                    var assetTypesEntity = allAssetsEntity.AssetTypeId == 0 ? null : assetType.FirstOrDefault(e => e.Id == allAssetsEntity.AssetTypeId);
                    var typeEntity = allAssets.AssetTypeId == 0 ? null : assetType.FirstOrDefault(d => d.Id == allAssets.AssetTypeId);
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.AssetType;
                    changeLog.PreviousValue = assetTypesEntity == null ? " " : assetTypesEntity.TypeName;
                    changeLog.NewValue = typeEntity == null ? " " : typeEntity.TypeName;
                    changeLogs.Add(changeLog);
                }

                if (allAssetsEntity.CategoryId != allAssets.CategoryId)
                {
                    var assetCategory = await _assetRepository.GetAllAssetCategory(companyId);
                    var assetCategoryEntity = allAssetsEntity.CategoryId == 0 ? null : assetCategory.FirstOrDefault(f => f.CategoryId == allAssetsEntity.CategoryId);
                    var categoryEntity = allAssets.CategoryId == 0 ? null : assetCategory.FirstOrDefault(a => a.CategoryId == allAssets.CategoryId);
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.CategoryId;
                    changeLog.PreviousValue = assetCategoryEntity == null ? " " : assetCategoryEntity.CategoryName;
                    changeLog.NewValue = categoryEntity == null ? " " : categoryEntity.CategoryName;
                    changeLogs.Add(changeLog);
                }

                if (allAssetsEntity.WarrantyEndDate != (!string.IsNullOrEmpty(allAssets.StrWarrantyToDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyToDate) : null))
                {
                    var changeLog = new ChangeLog();
                    changeLog.FieldName = Common.Constant.WarrantyEndDate;
                    changeLog.PreviousValue = allAssetsEntity.WarrantyEndDate?.ToString(Constant.DateFormat);
                    changeLog.NewValue = !string.IsNullOrEmpty(allAssets.StrWarrantyToDate) ? DateTimeExtensions.ConvertToNotNullDatetime(allAssets.StrWarrantyToDate).ToString(Constant.DateFormat) : null;
                    changeLogs.Add(changeLog);
                }
            }

            return changeLogs;
        }

        /// <summary>
        /// Logic to get the asset detail by particular allAssetsId
        /// </summary> 
        /// <param name="allAssetsId" ></param>
        public async Task<AllAssets> GetByAllAssetsId(int allAssetsId, int companyId)
        {
            var allAsset = new AllAssets();
            var allAssetsEntity = await _assetRepository.GetByAllAssetsId(allAssetsId, companyId);
            var Asset = _mapper.Map<AllAssets>(allAssetsEntity);
            if (allAssetsEntity != null)
            {
                allAsset.AllAssetsId = Asset.AllAssetsId;
                allAsset.AssetTypeId = Asset.AssetTypeId;
                allAsset.CategoryId = Asset.CategoryId;
                allAsset.AssetCode = Asset.AssetCode;
                allAsset.LocationId = Asset.LocationId;
                allAsset.PurchaseDate = Asset.PurchaseDate;
                allAsset.WarrantyStartDate = Asset.WarrantyStartDate;
                allAsset.EmployeeId = Asset.EmployeeId;
                allAsset.Remark = Asset.Remark;
                allAsset.ReturnDate = Asset.ReturnDate;
                allAsset.IssueDate = Asset.IssueDate;
                allAsset.AssetName = Asset.AssetName;
                allAsset.Description = Asset.Description;
                allAsset.AssetStatusId = Asset.AssetStatusId;
                allAsset.BrandId = Asset.BrandId;
                allAsset.ProductNumber = Asset.ProductNumber;
                allAsset.ModelNumber = Asset.ModelNumber;
                allAsset.WarrantyEndDate = Asset.WarrantyEndDate;
                allAsset.PurchaseNumber = Asset.PurchaseNumber;
                allAsset.VendorName = Asset.VendorName;
                allAsset.PurchaseOrder = Asset.PurchaseOrder;
                allAsset.InvoiceNumber = Asset.InvoiceNumber;
            }
            return allAsset;
        }

        /// <summary>
        /// Logic to get the asset detail by particular empId use only for view employees
        /// </summary> 
        /// <param name="empId" ></param>
        public async Task<List<ViewAssets>> GetAssetByEmployeeId(int empId,int companyId)
        {
            var viewAssetModel = new List<ViewAssets>();
            var allAssetsEntity = await _assetRepository.GetAssetByEmployeeId(empId,companyId);
            var employee = await _employeesRepository.GetAllStates();
            foreach (var asset in allAssetsEntity)
            {
                var assetTypeId = await _assetRepository.GetAssetTypeNameById(asset.AssetTypeId, companyId);
                var categoryId = await _assetRepository.GetAssetCategoryNameByCategoryId(asset.CategoryId, companyId);
                var assetStatusId = await _assetRepository.GetAssetStatusNameByName(Convert.ToInt32(asset.AssetStatusId), companyId);
                var employeeId = await _employeesRepository.GetEmployeeById(Convert.ToInt32(asset.EmployeeId), companyId);
                var brandTypeId = await _assetRepository.GetAssetBrandNameById(asset.BrandId,companyId);
                var empEntity = asset.LocationId == null?null: employee.FirstOrDefault(x => x.StateId == asset.LocationId) ;
                viewAssetModel.Add(new ViewAssets()
                {
                    AllAssetsId = asset.AllAssetsId,
                    AssetTypeName = assetTypeId.TypeName,
                    CategoryName = categoryId.CategoryName,
                    AssetCode = asset.AssetCode,
                    LocationName = empEntity != null ? empEntity.StateName : string.Empty,
                    PurchaseDate = asset.PurchaseDate == null ? null : asset.PurchaseDate,
                    WarrantyStartDate = asset.WarrantyStartDate == null ? null : asset.WarrantyStartDate,
                    EmployeeName = employeeId.FirstName + " " + employeeId.LastName,
                    Remark = asset.Remark == null ? "" : asset.Remark,
                    ReturnDate = asset.ReturnDate == null ? null : asset.ReturnDate,
                    IssueDate = asset.IssueDate == null ? null : asset.IssueDate,
                    AssetName = asset.AssetName == null ? "" : asset.AssetName,
                    Description = asset.Description == null ? "" : asset.Description,
                    AssetStatusName = assetStatusId?.StatusName == null ? null : assetStatusId.StatusName,
                    BrandId = brandTypeId.BrandType,
                    ProductNumber = asset.ProductNumber == null ? null : asset.ProductNumber,
                    PurchaseNumberName = asset.PurchaseNumber == null ? null : Convert.ToString((PurchaseOrderInvoiceNumber)asset.PurchaseNumber),
                    ModelNumber = asset.ModelNumber == null ? null : asset.ModelNumber,
                    PurchaseNumber = asset.PurchaseNumber == null ? null : asset.PurchaseNumber,
                    PurchaseOrder = asset.PurchaseOrder == null ? null : asset.PurchaseOrder,
                    InvoiceNumber = asset.InvoiceNumber == null ? null : asset.InvoiceNumber,
                    VendorName = asset.VendorName == null ? null : asset.VendorName,

                });
            }
            return viewAssetModel;
        }

        /// <summary>
        /// Logic to get deleted asset detail by particular allAssetsId
        /// </summary> 
        /// <param name="allAssetsId" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> DeleteAllAssets(int allAssetsId, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var allAssetsEntity = await _assetRepository.GetByAllAssetsId(allAssetsId, companyId);
            if (allAssetsEntity != null)
            {
                allAssetsEntity.IsDeleted = true;
                await _assetRepository.DeleteAllAssets(allAssetsEntity);
                result = await InsertAssetLog(allAssetsEntity.AssetName, allAssetsEntity.AssetCode, allAssetsId, allAssetsEntity.AssetTypeId, Common.Constant.Delete, sessionEmployeeId, companyId);
            }
            return result;
        }

        /// <summary>
        /// Logic to get the view asset detail by particular allAssetsId 
        /// </summary> 
        /// <param name="allAssetsId" ></param>       
        public async Task<AllAssets> GetAssetByEmployeeIds(int allAssetsId,int companyId)
        {
            var asset = new AllAssets();
            var assetsEntity = await _assetRepository.GetByAllAssetsId(allAssetsId, companyId);
            var assetTypeId = await _assetRepository.GetAssetTypeNameById(assetsEntity.AssetTypeId, companyId);
            var categoryId = await _assetRepository.GetAssetCategoryNameByCategoryId(assetsEntity.CategoryId, companyId);
            var assetStatusId = await _assetRepository.GetAssetStatusNameByName(Convert.ToInt32(assetsEntity.AssetStatusId), companyId);
            var employeeId = await _employeesRepository.GetEmployeeById(Convert.ToInt32(assetsEntity.EmployeeId), companyId);
            var employee = await _employeesRepository.GetAllStates();
            var empEntity = employee.FirstOrDefault(x => x.StateId == assetsEntity.LocationId);
            var assets = _mapper.Map<AllAssets>(assetsEntity);
            if (assetsEntity != null)
            {
                asset.AllAssetsId = assets.AllAssetsId;
                asset.AssetTypeName = assetTypeId.TypeName;
                asset.AssetCategoryName = categoryId.CategoryName;
                asset.AssetCode = assets.AssetCode;
                asset.AssetStatusName = assetStatusId.StatusName;
                asset.LocationName = empEntity != null ? empEntity.StateName : string.Empty;
                asset.PurchaseDate = assets.PurchaseDate;
                asset.WarrantyStartDate = assets.WarrantyStartDate;
                asset.ReturnDate = assets.ReturnDate;
                asset.IssueDate = assets.IssueDate;
                asset.AssetEmployeeName = employeeId != null ? employeeId.FirstName + " " + employeeId.LastName : string.Empty;
                asset.Remark = assets.Remark;
                asset.AssetName = assets.AssetName;
                asset.Description = assets.Description;
                asset.ProductNumber = assets.ProductNumber;
                asset.ModelNumber = assets.ModelNumber;
                asset.PurchaseNumberName = assets.PurchaseNumber != null ? Convert.ToString((PurchaseOrderInvoiceNumber)assets.PurchaseNumber) : string.Empty;
                asset.VendorName = assets.VendorName;
                asset.PurchaseOrder = assets.PurchaseOrder;
                asset.InvoiceNumber = assets.InvoiceNumber;
            }
            return asset;
        }

        /// <summary>
        /// Logic to get categoryId based assettype  the assets detail  by particular categoryId
        /// </summary> 
        /// <param name="categoryId" ></param>
        public async Task<List<AssetTypes>> GetBycategoryId(int categoryId, int companyId)
        {
            var listAssetTypes = new List<AssetTypes>();
            var listcategoryId = await _assetRepository.GetBycategoryId(categoryId);
            if (listcategoryId.Count() > 0)
            {
                listAssetTypes = _mapper.Map<List<AssetTypes>>(listcategoryId);
            }
            return listAssetTypes;
        }

        /// <summary>
        /// Logic to get categoryId based assettype  the assets detail  by particular categoryId
        /// </summary> 
        /// <param name="categoryId" ></param>
        public async Task<string> GetRefCategoryId(int categoryId,int companyId)
        {
            var totalAssetCount = await _assetRepository.GetAssetsMaxCount(categoryId, companyId);
            var category = await _assetRepository.GetAllAssetCategoryId(categoryId,companyId);
            var assetRefNumber = category.CategoryCode.Substring(0, 3) + (totalAssetCount + 1).ToString("D3");
            return assetRefNumber;
        }

        /// <summary>
        /// Logic to get assetbrandtype list
        /// </summary> 
        public async Task<AssetBrandTypeViewModel> GetAllBrand(int companyId)
        {
            var listOfAssetBrandTypeViewModel = new AssetBrandTypeViewModel();
            listOfAssetBrandTypeViewModel.AssetBrandType = await _assetRepository.GetAllAssetBrandTypes(companyId);
            return listOfAssetBrandTypeViewModel;
        }

        /// <summary>
        /// Logic to get create  the assetbrandtype detail
        /// </summary>         
        public async Task<AssetBrandType> Create(AssetBrandType assetBrandType)
        {
            var assetBrandTypes = new AssetBrandType();
            var totaltype = await _assetRepository.GetBrandName(assetBrandType.BrandType, assetBrandType.CompanyId);
            if (totaltype == 0)
            {
                if (assetBrandType?.BrandTypeId == 0)
                {
                    var assetBrandTypeEntity = _mapper.Map<AssetBrandTypeEntity>(assetBrandType);
                    assetBrandTypeEntity.IsActive = true;
                    var typedata = await _assetRepository.Create(assetBrandTypeEntity);
                    assetBrandTypes.BrandTypeId = assetBrandTypeEntity.BrandTypeId;
                }
            }
            else
            {
                assetBrandTypes.BrandTypeCount = totaltype;
            }
            return assetBrandTypes;
        }

        /// <summary>              
        /// Logic to get check brandType the brandType detail by particular brandType
        /// </summary>
        /// <param name="brandType" ></param> 
        public async Task<int> GetBrandName(string brandType,int companyId)
        {
            var totaltype = await _assetRepository.GetBrandName(brandType, companyId);
            return totaltype;
        }

        /// <summary>
        /// Logic to get update status the assetbrandtype detail by particular assetbrandtype status       
        /// </summary>
        /// <param name="brandType" ></param> 
        public async Task<int> UpdateAssetBrand(AssetBrandType assetBrandType)
        {
            var assetBrandTypeEntity = _mapper.Map<AssetBrandTypeEntity>(assetBrandType);
            await _assetRepository.UpdateAssetBrand(assetBrandTypeEntity);
            var result = assetBrandTypeEntity.BrandTypeId;
            return result;
        }

        /// <summary>
        /// Logic to get update the assetbrandtype detail by particular assetbrandtype       
        /// </summary>
        /// <param name="assetBrandType" ></param>
        public async Task<int> UpadateAssetBrandType(AssetBrandType assetBrandType)
        {
            var assetBrandTypeEntity = _mapper.Map<AssetBrandTypeEntity>(assetBrandType);
            await _assetRepository.UpadateAssetBrandType(assetBrandTypeEntity);
            var result = assetBrandTypeEntity.BrandTypeId;
            return result;
        }

        /// <summary>
        /// Logic to get deleted  the assetbrandtype detail
        /// </summary>
        /// <param name="brandTypeId" ></param>
        public async Task<bool> DeletedAssetBrand(int brandTypeId,int companyId)
        {
            var result = await _assetRepository.DeletedAssetBrand(brandTypeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get all the assetlog list
        /// </summary>
        /// <param name="assetLogViewModel" ></param>
        /// <param name="companyId" ></param> 
        public async Task<AssetLogViewModel> GetAllAssetLog(AssetLogViewModel assetLogViewModel, int companyId)
        {
            var employeeId = assetLogViewModel.EmpId;
            var assetCompanyId = Convert.ToString(companyId);
            var assetLogViewModels = new AssetLogViewModel();
            assetLogViewModels.AssetLog = new List<AssetLog>();
            assetLogViewModels.AssetDropdowns = new List<AssetDropdown>();
            assetLogViewModels.AssetDetailsDropdown = new List<AssetDetailsDropdown>();
            var today = DateTime.Today.AddDays(-2).Date;
            var endday = DateTime.Now;
            assetLogViewModels.StartDate = today.Date.ToString(Constant.DateFormat);
            assetLogViewModels.EndDate = endday.Date.ToString(Constant.DateFormat);
            var assetCodeName = "";
            var dFrom = string.IsNullOrEmpty(assetLogViewModels.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModels.StartDate).ToString("MM/dd/yyyy");
            var dTo = string.IsNullOrEmpty(assetLogViewModels.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModels.EndDate).ToString("MM/dd/yyyy");
            if (employeeId == 0)
            {
                var empId = Constant.ZeroStr;
                List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@empId", empId),
                    new KeyValuePair<string, string>("@AssetNo",assetCodeName),
                    new KeyValuePair<string, string>("@startDate",dFrom),
                    new KeyValuePair<string, string>("@endDate",dTo),
                    new KeyValuePair<string, string>("@companyId",assetCompanyId),
                };
                var attendanceReportDateModels = await _assetRepository.GetAllAssetByEmployeeLogFilter("spGetAssetLogByEmployeeFilterData", p);
                var assettype = await _assetRepository.GetAllAssetType(companyId);
                foreach (var item in attendanceReportDateModels)
                {
                    var autherName = await _employeesRepository.GetEmployeeById(item.EmpId, companyId);
                    var assetType = await _assetRepository.GetAssetTypeNameById(item.AssetTypeId, companyId);
                    var assetLog = new AssetLog();
                    assetLog.AssetLogId = item.Id;
                    assetLog.AssetNo = item.AssetCode;
                    assetLog.AssetTypeName = assetType == null ? "" : assetType.TypeName;
                    assetLog.FieldName = string.IsNullOrEmpty(item.FieldName) ? string.Empty : item.FieldName;
                    assetLog.PreviousValue = string.IsNullOrEmpty(item.PreviousValue) ? string.Empty : item.PreviousValue;
                    assetLog.NewValue = string.IsNullOrEmpty(item.NewValue) ? string.Empty : item.NewValue;
                    assetLog.Event = string.IsNullOrEmpty(item.Event) ? string.Empty : item.Event;
                    assetLog.AuthorName = autherName == null ? "" : autherName.FirstName + " " + autherName.LastName;
                    assetLog.CreatedDate = Convert.ToDateTime(item.CreatedDate);
                    assetLogViewModels.AssetLog.Add(assetLog);
                }
            }
            assetLogViewModels.AssetDetailsDropdown = await GetAllAssetDetailsDropdown(companyId);
            assetLogViewModels.AssetDropdowns = await GetAllAssetDrropdown(companyId);
            return assetLogViewModels;
        }

        /// <summary>
        /// Logic to get all the employees assetdropdown
        /// </summary>        
        public async Task<List<AssetDropdown>> GetAllAssetDrropdown(int companyId)
        {
            var assetDropdowns = new List<AssetDropdown>();
            var listEmployee = await _employeesRepository.GetAllEmployees(companyId);
            if (listEmployee != null)
            {
                var assetdropdown = new AssetDropdown();
                assetdropdown.EmployeeId = 0;
                assetdropdown.EmployeeIdWithName = Common.Constant.AllEmployees;
                assetDropdowns.Add(assetdropdown);
                foreach (var item in listEmployee)
                {
                    var assetDropdown = new AssetDropdown();
                    assetDropdown.EmployeeId = item.EmpId;
                    assetDropdown.EmployeeName = item.UserName;
                    assetDropdown.EmployeeIdWithName = item.UserName + Constant.Hyphen + item.FirstName + " " + item.LastName;
                    assetDropdowns.Add(assetDropdown);
                }
            }
            return assetDropdowns;
        }

        /// <summary>
        /// Logic to get all the allmodules assetdropdown
        /// </summary> 
        public async Task<List<AssetDetailsDropdown>> GetAllAssetDetailsDropdown(int companyId)
        {
            var employeeDropdowns = new List<AssetDetailsDropdown>();
            var listEmployee = await _assetRepository.GetAllAssetLog(companyId);
            var filterSections = listEmployee.GroupBy(x => x.AssetNo).ToList();
            if (filterSections != null)
            {
                var employeedropdown = new AssetDetailsDropdown();
                employeedropdown.AssetCodeName = "";
                employeedropdown.AssetNo = Common.Constant.AllModules;
                employeeDropdowns.Add(employeedropdown);
                foreach (var item in filterSections)
                {
                    var employeeDropdown = new AssetDetailsDropdown();
                    employeedropdown.AssetCodeName = item.Key;
                    employeeDropdown.AssetNo = item.Key;
                    employeeDropdowns.Add(employeeDropdown);
                }
            }
            return employeeDropdowns;
        }

        /// <summary>
        /// Logic to get the assetlog filter
        /// </summary>
        /// <param name="assetLogViewModel" ></param>
        /// <param name="companyId" ></param> 
        public async Task<AssetLogViewModel> GetAllAssetByEmployeeLogFilter(AssetLogViewModel assetLogViewModel, int companyId)
        {
            var employeeChangeLogViewModels = new AssetLogViewModel
            {
                AssetLog = new List<AssetLog>()
            };
            var assetCodeName = assetLogViewModel.AssetNos == Common.Constant.AllModules ? "" : assetLogViewModel.AssetNos;
            var dFrom = string.IsNullOrEmpty(assetLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.StartDate).ToString("MM/dd/yyyy");
            var dTo = string.IsNullOrEmpty(assetLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(assetLogViewModel.EndDate).ToString("MM/dd/yyyy");
            var empId = assetLogViewModel.EmpId == 0 ? Constant.ZeroStr : Convert.ToString(assetLogViewModel.EmpId);

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@empId", empId),
                new KeyValuePair<string, string>("@AssetNo", assetCodeName),
                new KeyValuePair<string, string>("@startDate", dFrom),
                new KeyValuePair<string, string>("@endDate", dTo),
                new KeyValuePair<string, string>("@companyId", Convert.ToString(companyId))
            };

            var attendanceReportData = await _assetRepository.GetAllAssetByEmployeeLogFilter("spGetAssetLogByEmployeeFilterData", parameters);
            foreach (var item in attendanceReportData)
            {
                var author = await _employeesRepository.GetEmployeeById(item.EmpId, companyId);
                var assetType = await _assetRepository.GetAssetTypeNameById(item.AssetTypeId, companyId);

                employeeChangeLogViewModels.AssetLog.Add(new AssetLog
                {
                    AssetLogId = item.Id,
                    AssetNo = item.AssetCode,
                    AssetTypeName = assetType.TypeName == null ? "" : assetType.TypeName,
                    FieldName = item.FieldName == null ? "" : item.FieldName,
                    PreviousValue = item.PreviousValue == null ? "" : item.PreviousValue,
                    NewValue = item.NewValue == null ? "" : item.NewValue,
                    Event = item.Event == null ? "" : item.Event,
                    AuthorName = author == null ? "" : $"{author.FirstName} {author.LastName}",
                    CreatedDate = Convert.ToDateTime(item.CreatedDate)
                });
            }
            return employeeChangeLogViewModels;

        }

        /// <summary>
        /// Logic to get the assetlog filter count
        /// </summary>
        /// <param name="pager" ></param>
        public async Task<int> GetAssetListCount(SysDataTablePager pager,int companyId)
        {
            var assetsCount = await _assetRepository.GetAssetListCount(pager, companyId);
            return assetsCount;
        }

        /// <summary>
        /// Logic to get all employees details list 
        /// </summary>
        /// <param name="pager" ></param>
        public async Task<AllAssets> GetAllEmployeesList(SysDataTablePager pager, string columnDirection, string columnName,int companyId)
        {
            var assetListViewModel = new AllAssets();
            assetListViewModel.assetViewModels = await _assetRepository.GetAssetDetailsList(pager, columnDirection, columnName, companyId);
            return assetListViewModel;
        }

        /// <summary>
        ///  Logic to get the asset log detail 
        /// </summary>
        /// <param name="pager,assetLogViewModel,companyId" ></param>
        public async Task<AssetLogViewModel> GetAllEmployeesAssetLogList(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId)
        {
            var assetLogViewModels = new AssetLogViewModel();
            assetLogViewModels.AssetDropdowns = new List<AssetDropdown>();
            assetLogViewModels.AssetDetailsDropdown = new List<AssetDetailsDropdown>();
            assetLogViewModels.AssetDetailsDropdown = await GetAllAssetDetailsDropdown(companyId);
            assetLogViewModels.AssetDropdowns = await GetAllAssetDrropdown(companyId);
            assetLogViewModels.AssetsLogModels = await _assetRepository.GetAllEmployeesAssetLogList(pager, assetLogViewModel, companyId);
            return assetLogViewModels;
        }

        /// <summary>
        ///  Logic to get the asset log detail count
        /// </summary>
        /// <param name="pager,assetLogViewModel,companyId" ></param>
        public async Task<int> GetAllEmployeesAssetLogCount(SysDataTablePager pager, AssetLogViewModel assetLogViewModel,int companyId)
        {
            return await _assetRepository.GetAllEmployeesAssetLogCount(pager, assetLogViewModel, companyId);
        }

        /// <summary>
        ///  Logic to get the asset log filter detail 
        /// </summary>
        /// <param name="pager,assetLogViewModel,companyId" ></param>
        public async Task<AssetLogViewModel> GetAllEmployessByAssetLogFilter(SysDataTablePager pager, AssetLogViewModel assetLogViewModel, int companyId)
        {
            var assetLogViewModels = new AssetLogViewModel();
            assetLogViewModels.AssetsLogModels = await _assetRepository.GetAllEmployessByAssetLogFilter(pager, assetLogViewModel, companyId);
            return assetLogViewModels;
        }

    }
}
