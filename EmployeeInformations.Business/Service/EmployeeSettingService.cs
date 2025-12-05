using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.EmployeeSettingViewModel;

namespace EmployeeInformations.Business.Service
{
    public class EmployeeSettingService : IEmployeeSetting
    {
        private readonly IEmployeeSettingRepository _employeeSettingRepository;
        private readonly IMapper _mapper;

        public EmployeeSettingService(IEmployeeSettingRepository employeeSettingRepository, IMapper mapper)
        {
            _employeeSettingRepository = employeeSettingRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Logic to get the employeesettings details 
        /// </summary>   
        public async Task<EmployeeSetting> GetEmployeeSetting(int companyId)
        {
            var employeeSetting = new EmployeeSetting();           
            employeeSetting = await _employeeSettingRepository.GetEmployeeSetting(companyId);
            return employeeSetting;
        }

        /// <summary>
        /// Logic to get create  and update the employeesettings detail 
        /// </summary>        
        /// <param name="employeeSetting" ></param> 
        /// <param name="sessionValue" ></param> 
        public async Task<bool> CreateSetting(EmployeeSetting employeeSetting, int sessionValue, int companyId)
        {
            var entity = new EmployeeSettingEntity();
            employeeSetting.CreatedBy = sessionValue;
            employeeSetting.CreatedDate = DateTime.Now;
            if (employeeSetting != null)
            {
                entity = _mapper.Map<EmployeeSettingEntity>(employeeSetting);
            }
            var data = _employeeSettingRepository.CreateSetting(entity,companyId);
            return data != null;
        }
    }
}
