using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.BenefitViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class BenefitRepository : IBenefitRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public BenefitRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Benefit Type

        /// <summary>
        /// Logic to get benefitTypeId the benefittypesentitys detail by particular benefitTypeId
        /// </summary>           
        /// <param name="benefitTypeId" >employeemedicalbenefitentitys</param>       
        public async Task<BenefitTypesEntity> GetBenefitTypeNameById(int benefitTypeId)
        {
            var benefit = await _dbContext.BenefitTypesEntitys.Where(l => l.BenefitTypeId == benefitTypeId).FirstOrDefaultAsync();
            return benefit ?? new BenefitTypesEntity();
        }

        /// <summary>
        /// Logic to get benefittypes list
        /// </summary>           
        /// <param name="IsDeleted" ></param> 
        public async Task<List<BenefitTypesEntity>> GetBenefitTypes()
        {
            return await _dbContext.BenefitTypesEntitys.Where(l => !l.IsDeleted).ToListAsync();
        }

        //// Benefit

        /// <summary>
        /// Logic to get employeesbenefit list
        /// </summary>           
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<EmployeeBenefit>> GetAllEmployeeBenefits(int companyId)
        {
            var employeeBenefits = await (from employeeBenefit in _dbContext.EmployeeBenefitEntitys

                                          join benefitType in _dbContext.BenefitTypesEntitys on employeeBenefit.BenefitTypeId equals benefitType.BenefitTypeId

                                          join employee in _dbContext.Employees on employeeBenefit.EmpId equals employee.EmpId

                                          where !employeeBenefit.IsDeleted && employeeBenefit.CompanyId == companyId && !benefitType.IsDeleted && employee.CompanyId == employeeBenefit.CompanyId
                                          select new EmployeeBenefit()
                                          {
                                              BenefitId = employeeBenefit.BenefitId,
                                              CompanyId = employeeBenefit.CompanyId,
                                              EmpId = employeeBenefit.EmpId,
                                              BenefitTypeId = employeeBenefit.BenefitTypeId,
                                              CreatedDate = employeeBenefit.CreatedDate,
                                              IsDeleted = employeeBenefit.IsDeleted,
                                              EmployeeStatus = employee.IsDeleted,
                                              EmployeeName = employee.FirstName + " " + employee.LastName,
                                              BenefitName = benefitType.BenefitName,

                                          }).ToListAsync();
            return employeeBenefits;
        }


        /// <summary>
        /// Logic to get empId the employeebenefitentitys detail
        /// </summary>           
        /// <param name="empId" >employeebenefitentitys</param> 
        /// <param name="IsDeleted" >employeebenefitentitys</param> 
        /// <param name="CompanyId" >employeebenefitentitys</param> 
        public async Task<List<EmployeeBenefitEntity>> GetAllEmployeeBenefitsByEmpId(int empId, int companyId)
        {
            var benefitEntity = await _dbContext.EmployeeBenefitEntitys.Where(b => b.EmpId == empId && !b.IsDeleted && b.CompanyId == companyId).ToListAsync();
            return benefitEntity ?? new List<EmployeeBenefitEntity>();
        }


        /// <summary>
        /// Logic to get benefitId the employeesbenefit detail by particular benefitId
        /// </summary>           
        /// <param name="benefitId" >employeebenefitentitys</param>
        /// <param name="CompanyId" >employeebenefitentitys</param>
        public async Task<EmployeeBenefitEntity> GetBenefitByBenefitId(int benefitId, int companyId)
        {
            var benefit = await _dbContext.EmployeeBenefitEntitys.Where(l => l.BenefitId == benefitId && l.CompanyId == companyId).AsNoTracking().FirstOrDefaultAsync();
            return benefit ?? new EmployeeBenefitEntity();
        }



        /// <summary>
         /// Logic to get create and update the employeesbenefit detail 
         /// </summary>           
        /// <param name="employeeBenefitEntity" ></param>
        public async Task<bool> AddBenefits(EmployeeBenefitEntity employeeBenefitEntity, int companyId)
        {
            var result = false;
            if (employeeBenefitEntity?.BenefitId == 0)
            {
                employeeBenefitEntity.CompanyId = companyId;
                await _dbContext.EmployeeBenefitEntitys.AddAsync(employeeBenefitEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                if (employeeBenefitEntity != null)
                {
                    try
                    {
                        employeeBenefitEntity.CompanyId = companyId;
                        _dbContext.EmployeeBenefitEntitys.Update(employeeBenefitEntity);
                        result = await _dbContext.SaveChangesAsync() > 0;
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the employeesbenefit detail 
        /// </summary>           
        /// <param name="employeeBenefitEntity" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeleteBenefit(EmployeeBenefitEntity employeeBenefitEntity, int sessionEmployeeId)
        {
            var benifit = await _dbContext.EmployeeBenefitEntitys.FirstOrDefaultAsync(d => d.BenefitId == employeeBenefitEntity.BenefitId && d.CompanyId == employeeBenefitEntity.CompanyId);
            var result = false;
            if (benifit != null)
            {
                benifit.IsDeleted = true;
                benifit.UpdatedBy = sessionEmployeeId;
                benifit.UpdatedDate = DateTime.Now;
                _dbContext.EmployeeBenefitEntitys.Update(benifit);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        ///  Logic to get empId the employeebenefitentitys detail by particular empId
        /// </summary>           
        /// <param name="empId" >employeebenefitentitys</param>
        /// <param name="CompanyId" >employeebenefitentitys</param>
        public async Task<EmployeeBenefitEntity> GetEmployeeBenefitsByEmployeeId(int empId, int companyId)
        {
            var benefit = await _dbContext.EmployeeBenefitEntitys.Where(l => l.EmpId == empId && l.CompanyId == companyId).AsNoTracking().FirstOrDefaultAsync();
            return benefit ?? new EmployeeBenefitEntity();
        }

        /// Medical Benefit

        /// <summary>
        /// Logic to get employeesmedicalbenefit list
        /// </summary>           
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<EmployeeMedicalBenefit>> GetAllEmployeeMedicalBenefits(int companyId)
        {
            var employeeMedicalBenefits = await (from employeeMedicalBenefit in _dbContext.EmployeeMedicalBenefitEntitys

                                                 join benefitType in _dbContext.BenefitTypesEntitys on employeeMedicalBenefit.BenefitTypeId equals benefitType.BenefitTypeId

                                                 join employee in _dbContext.Employees on employeeMedicalBenefit.EmpId equals employee.EmpId

                                                 where !employeeMedicalBenefit.IsDeleted && employeeMedicalBenefit.CompanyId == companyId && !benefitType.IsDeleted && employee.CompanyId == employeeMedicalBenefit.CompanyId
                                                 orderby employee.IsDeleted
                                                 select new EmployeeMedicalBenefit()
                                                 {
                                                     MedicalBenefitId = employeeMedicalBenefit.MedicalBenefitId,
                                                     CompanyId = employeeMedicalBenefit.CompanyId,
                                                     EmpId = employeeMedicalBenefit.EmpId,
                                                     BenefitTypeId = employeeMedicalBenefit.BenefitTypeId,
                                                     CreatedDate = employeeMedicalBenefit.CreatedDate,
                                                     IsDeleted = employeeMedicalBenefit.IsDeleted,
                                                     EmployeeStatus = employee.IsDeleted,
                                                     EmployeeName = employee.FirstName + " " + employee.LastName,
                                                     BenefitName = benefitType.BenefitName,
                                                     Cost = employeeMedicalBenefit.Cost,
                                                     Member = employeeMedicalBenefit.Member,
                                                     MembershipNumber = employeeMedicalBenefit.MembershipNumber,
                                                     Category = employeeMedicalBenefit.Category,
                                                     Scheme = employeeMedicalBenefit.Scheme,

                                                 }).ToListAsync();
            return employeeMedicalBenefits;
        }


        /// <summary>
        /// Logic to get medicalBenefitId the employeesmedicalbenefit detail by particular medicalBenefitId
        /// </summary>           
        /// <param name="medicalBenefitId" >employeemedicalbenefitentitys</param>
        /// <param name="CompanyId" >employeemedicalbenefitentitys</param>
        public async Task<EmployeeMedicalBenefitEntity> GetMedicalBenefitByMedicalBenefitId(int medicalBenefitId, int companyId)
        {
            var benefit = await _dbContext.EmployeeMedicalBenefitEntitys.Where(l => l.MedicalBenefitId == medicalBenefitId && l.CompanyId == companyId).AsNoTracking().FirstOrDefaultAsync();
            return benefit ?? new EmployeeMedicalBenefitEntity();
        }


        /// <summary>
        /// Logic to get create and update the employeemedicalbenefitentitys detail 
        /// </summary>           
        /// <param name="employeeMedicalBenefitEntity" ></param>
        public async Task<bool> AddMedicalBenefits(EmployeeMedicalBenefitEntity employeeMedicalBenefitEntity, int companyId)
        {
            var result = false;
            if (employeeMedicalBenefitEntity?.MedicalBenefitId == 0)
            {
                employeeMedicalBenefitEntity.CompanyId = companyId;
                await _dbContext.EmployeeMedicalBenefitEntitys.AddAsync(employeeMedicalBenefitEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                try
                {
                    if (employeeMedicalBenefitEntity != null)
                    {
                        employeeMedicalBenefitEntity.CompanyId = employeeMedicalBenefitEntity.CompanyId;
                        _dbContext.EmployeeMedicalBenefitEntitys.Update(employeeMedicalBenefitEntity);
                        result = await _dbContext.SaveChangesAsync() > 0;
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the employeemedicalbenefitentitys detail 
        /// </summary>           
        /// <param name="employeeMedicalBenefitEntity" ></param>
        public async Task<bool> DeleteMedicalBenefit(EmployeeMedicalBenefitEntity employeeMedicalBenefitEntity, int sessionEmployeeId)
        {
            var medicalBenifit = await _dbContext.EmployeeMedicalBenefitEntitys.FirstOrDefaultAsync(d => d.MedicalBenefitId == employeeMedicalBenefitEntity.MedicalBenefitId && d.CompanyId == employeeMedicalBenefitEntity.CompanyId);
            var result = false;
            if (medicalBenifit != null)
            {
                medicalBenifit.IsDeleted = true;
                medicalBenifit.UpdatedBy = sessionEmployeeId;
                medicalBenifit.UpdatedDate = DateTime.Now;
                _dbContext.EmployeeMedicalBenefitEntitys.Update(medicalBenifit);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        ///  Logic to get empId the employeemedicalbenefitentitys detail by particular empId
         /// </summary>           
        /// <param name="empId" >employeemedicalbenefitentitys</param>
        /// <param name="CompanyId" >employeemedicalbenefitentitys</param>
        /// <param name="IsDeleted" >employeemedicalbenefitentitys</param>
        public async Task<EmployeeMedicalBenefitEntity> GetEmployeeMedicalBenefitsByEmployeeId(int empId, int companyId)
        {
            var benefit = await _dbContext.EmployeeMedicalBenefitEntitys.Where(l => l.EmpId == empId && !l.IsDeleted && l.CompanyId == companyId).FirstOrDefaultAsync();
            return benefit ?? new EmployeeMedicalBenefitEntity();
        }


        /// <summary>
        /// Logic to get empId the employeemedicalbenefitentitys detail
        /// </summary>           
        /// <param name="empId" >employeemedicalbenefitentitys</param> 
        /// <param name="IsDeleted" >employeemedicalbenefitentitys</param> 
        /// <param name="CompanyId" >employeemedicalbenefitentitys</param> 
        public async Task<List<EmployeeMedicalBenefitEntity>> GetAllEmployeeMedicalBenefitsByEmpId(int empId, int companyId)
        {
            var medicalBenefitEntity = await _dbContext.EmployeeMedicalBenefitEntitys.Where(b => b.EmpId == empId && !b.IsDeleted && b.CompanyId == companyId).ToListAsync();
            return medicalBenefitEntity ?? new List<EmployeeMedicalBenefitEntity>();
        }

        /// <summary>
        ///  Logic to get all Benefit filter data count by SP
        /// </summary>
        /// <param name="companyId,pager" ></param>  
        public async Task<int> BenefitCount(int companyId, SysDataTablePager pager)
        {
            try
            {
                var result = 0;
                
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);

                List<BenefitFilterCount> benefitFilterCount = await _dbContext.benefitFilterCount.FromSqlRaw("EXEC [dbo].[spGetBenefitsCount] @companyId,@searchText",param1, param2).ToListAsync();
                foreach (var item in benefitFilterCount)
                {
                    result = item.Id;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        ///  Logic to get all Benefit filter data by SP
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection" ></param>  
        public async Task<List<BenefitFilterViewModel>> GetBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                var comId = Convert.ToString(companyId);
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param3 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);
                var data = await _dbContext.benefitFilterViewModel.FromSqlRaw("EXEC [dbo].[spGetBenefitsFilter]@companyId,@pagingSize,@offsetValue,@searchText,@sorting", param1, param2, param3, param4,param5).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///  Logic to get all Medical Benefit filter data count by SP
        /// </summary>
        /// <param name="companyId,pager" ></param>  
        public async Task<int> MedicalBenefitCount(int companyId, SysDataTablePager pager)
        {
            try
            {
                var result = 0;
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);

                List<MedicalBenefitFilterCount> medicalbenefitFilterCount = await _dbContext.medicalBenefitFilterCount.FromSqlRaw("EXEC [dbo].[spGetMedicalBenefitsCount] @companyId,@searchText", param1, param2).ToListAsync();
                foreach (var item in medicalbenefitFilterCount)
                {
                    result = item.Id;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        ///  Logic to get all MedicalBenefit filter data by SP
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection" ></param>  
        public async Task<List<MedicalBenefitFilterViewModel>> GetMedicalBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                var comId = Convert.ToString(companyId);
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param3 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var data = await _dbContext.medicalBenefitFilterViewModel.FromSqlRaw("EXEC [dbo].[spGetMedicalBenefitsFilter]@companyId,@pagingSize,@offsetValue,@searchText,@sorting", param1, param2, param3, param4,param5).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

    }
}
