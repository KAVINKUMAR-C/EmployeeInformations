using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.EmployeeSettingViewModel;
using Microsoft.EntityFrameworkCore;
namespace EmployeeInformations.Data.Repository
{
    public class EmployeeSettingRepository : IEmployeeSettingRepository
    {
        private readonly EmployeesDbContext _dbContext;
        public EmployeeSettingRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        /// <summary>
        /// Logic to get create  and update the employeesettings detail 
        /// </summary>        
        /// <param name="entity" ></param> 
        public async Task<bool> CreateSetting(EmployeeSettingEntity entity, int companyId)
        {
            var result = false;
            if (entity != null)
            {
                entity.CompanyId = companyId;
                if (entity.EmployeeSettingId == 0)
                {
                    _dbContext.EmployeeSettingsEntity.Add(entity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                _dbContext.EmployeeSettingsEntity.Update(entity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get the employeesettings details 
        /// </summary>        
        public async Task<EmployeeSetting> GetEmployeeSetting(int companyId)
        {
            var employeeSettings = await (from employeesetting in _dbContext.EmployeeSettingsEntity
                                          where !employeesetting.IsDeleted && companyId == employeesetting.CompanyId
                                          select new EmployeeSetting()
                                          {
                                              EmployeeSettingId = employeesetting.EmployeeSettingId,
                                              CompanyId = employeesetting.CompanyId,
                                              ProbationMonths = employeesetting.ProbationMonths,
                                          }).FirstOrDefaultAsync();
            return employeeSettings;
        }

    }
}
