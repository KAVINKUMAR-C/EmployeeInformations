using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.BenefitViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.Service
{
    public class BenefitService : IBenefitService
    {
        private readonly IBenefitRepository _benefitRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public BenefitService(IBenefitRepository benefitRepository, IEmployeesRepository employeesRepository, IMapper mapper, IConfiguration config, ICompanyRepository companyRepository)
        {
            _benefitRepository = benefitRepository;
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _config = config;
            _companyRepository = companyRepository;
        }

        //public async Task<List<BenefitTypes>> GetBenefitTypes()
        //{
        //    var listOfGetBenefitTypes = new List<BenefitTypes>();
        //    var listGetBenefitType = await _benefitRepository.GetBenefitTypes();
        //    if (listGetBenefitType != null)
        //    {
        //        listOfGetBenefitTypes = _mapper.Map<List<BenefitTypes>>(listGetBenefitType);
        //    }
        //    return listOfGetBenefitTypes;
        //}

        //Benefit

        /// <summary>
        /// Logic to get benefittypes list
        /// </summary>
        public async Task<EmployeeBenefitViewModel> GetAllBenefitDetails(int companyId)
        {
            var employeeBenefitViewModel = new EmployeeBenefitViewModel();
            var listOfBenefitTypes = await _benefitRepository.GetBenefitTypes();
            listOfBenefitTypes = listOfBenefitTypes.Where(x => x.BenefitTypeId != (int)BenefitType.MedicalBenefit).ToList();
            var listOfEmployees = await _employeesRepository.GetAllEmployees(companyId);

            var result = new List<ReportingPerson>();
            var employeesList = _mapper.Map<List<Employees>>(listOfEmployees);
            foreach (var item in employeesList)
            {
                result.Add(new ReportingPerson()
                {
                    EmpId = item.EmpId,
                    EmployeeName = item.UserName + " - " + item?.FirstName + " " + item?.LastName,
                });
            }

            if (listOfBenefitTypes.Count() > 0)
            {
                var listEmployeeBenefits = await _benefitRepository.GetAllEmployeeBenefits(companyId);
                var benefitTypes = _mapper.Map<List<BenefitTypes>>(listOfBenefitTypes);
                employeeBenefitViewModel.EmployeeBenefit = listEmployeeBenefits;
                employeeBenefitViewModel.BenefitTypes = benefitTypes;
                employeeBenefitViewModel.reportingPeople = result;
            }
            return employeeBenefitViewModel;
        }

        /// <summary>
        /// Logic to get create and update the employeebenefit detail
        /// </summary>
        /// <param name="employeeBenefit" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> AddBenefits(EmployeeBenefit employeeBenefit, int sessionEmployeeId, int companyId)
        {
            var result = false;

            if (employeeBenefit.BenefitId == 0)
            {
                employeeBenefit.CreatedDate = DateTime.Now;
                employeeBenefit.CreatedBy = sessionEmployeeId;
                var employeeBenefitEntity = _mapper.Map<EmployeeBenefitEntity>(employeeBenefit);
                if (employeeBenefitEntity != null)
                {
                    result = await _benefitRepository.AddBenefits(employeeBenefitEntity, companyId);
                }
            }
            else
            {
                var employeebenefitEntitys = await _benefitRepository.GetBenefitByBenefitId(employeeBenefit.BenefitId, employeeBenefit.CompanyId);                            
                if (employeebenefitEntitys != null)
                {
                    employeebenefitEntitys.BenefitId = employeeBenefit.BenefitId;
                    employeebenefitEntitys.EmpId = employeeBenefit.EmpId;
                    employeebenefitEntitys.BenefitTypeId = employeeBenefit.BenefitTypeId;
                    employeebenefitEntitys.UpdatedDate = DateTime.Now;
                    employeebenefitEntitys.UpdatedBy = sessionEmployeeId;
                    result = await _benefitRepository.AddBenefits(employeebenefitEntitys, companyId);
                }
            }

            return result;
        }

        /// <summary>
        /// Logic to get deleted the employeebenefit detail
        /// </summary>
        /// <param name="employeeBenefit" ></param>        
        public async Task<bool> DeleteBenefit(EmployeeBenefit employeeBenefit, int sessionEmployeeId)
        {
            var employeebenefitEntity = new EmployeeBenefitEntity();
            employeebenefitEntity.BenefitId = employeeBenefit.BenefitId;
            employeebenefitEntity.CompanyId = employeeBenefit.CompanyId;
            var result = false;
            if (employeeBenefit != null)
            {
                result = await _benefitRepository.DeleteBenefit(employeebenefitEntity, sessionEmployeeId);
            }

            return result;
        }

        /// <summary>
        /// Logic to get the employeeMedicalBenefit and employeeBenefit detail by particular empId use only for view employees
        /// </summary>
        /// <param name="empId" ></param>  
        public async Task<ViewTotalEmployeeBenefits> GetEmployeeBenefitsByEmployeeId(int empId,int companyId)
        {
            var employeeBenefit = new List<EmployeeBenefit>();
            var employeeMedicalBenefits = new List<EmployeeMedicalBenefit>();
            var listOfEmployeeBenefitEntity = await _benefitRepository.GetAllEmployeeBenefitsByEmpId(empId,companyId);
            var listOfEmployeeMedicalBenefitEntity = await _benefitRepository.GetAllEmployeeMedicalBenefitsByEmpId(empId,companyId);
            var employeeBenefitViewModel = new ViewTotalEmployeeBenefits();
            employeeBenefitViewModel.EmpId = empId;
            var employeeEntity = await _employeesRepository.GetEmployeeByIdView(empId, companyId);
            if (listOfEmployeeBenefitEntity.Count() > 0)
            {
                foreach (var item in listOfEmployeeBenefitEntity)
                {
                    var benefitEntity = await _benefitRepository.GetBenefitTypeNameById(item.BenefitTypeId);
                    var employeeBenefits = new EmployeeBenefit();
                    employeeBenefits.EmployeeName = employeeEntity != null ? employeeEntity.FirstName + " " + employeeEntity.LastName : String.Empty;
                    employeeBenefits.BenefitName = benefitEntity != null ? benefitEntity.BenefitName : String.Empty;
                    employeeBenefits.EmployeeStatus = employeeEntity != null ? employeeEntity.IsDeleted : false;
                    employeeBenefit.Add(employeeBenefits);
                }
            }

            if (listOfEmployeeMedicalBenefitEntity.Count() > 0)
            {
                foreach (var item in listOfEmployeeMedicalBenefitEntity)
                {
                    var allMedicalBenefitsEntity = await _benefitRepository.GetEmployeeMedicalBenefitsByEmployeeId(item.EmpId,companyId);
                    var benefitTypesEntity = await _benefitRepository.GetBenefitTypeNameById(item.BenefitTypeId);
                    var companyEntity = await _companyRepository.GetByCompanyId(item.CompanyId);
                    var employeeMedicalBenefit = new EmployeeMedicalBenefit();
                    employeeMedicalBenefit.EmployeeName = employeeEntity != null ? employeeEntity.FirstName + " " + employeeEntity.LastName : String.Empty;
                    employeeMedicalBenefit.EmployeeStatus = employeeEntity != null ? employeeEntity.IsDeleted : false;
                    employeeMedicalBenefit.BenefitName = benefitTypesEntity != null ? benefitTypesEntity.BenefitName : String.Empty;
                    employeeMedicalBenefit.CompanyName = companyEntity != null ? companyEntity.CompanyName : String.Empty;
                    employeeMedicalBenefit.Scheme = allMedicalBenefitsEntity.Scheme;
                    employeeMedicalBenefit.Category = allMedicalBenefitsEntity.Category;
                    employeeMedicalBenefit.Cost = allMedicalBenefitsEntity.Cost;
                    employeeMedicalBenefit.Member = allMedicalBenefitsEntity.Member;
                    employeeMedicalBenefit.MembershipNumber = allMedicalBenefitsEntity.MembershipNumber;

                    employeeMedicalBenefits.Add(employeeMedicalBenefit);
                }
            }
            employeeBenefitViewModel.ViewEmployeeBenefits = employeeBenefit;
            employeeBenefitViewModel.ViewEmployeeMedicalBenefits = employeeMedicalBenefits;
            return employeeBenefitViewModel;
        }

        /// <summary>
        /// Logic to get view the employeeBenefit detail by particular BenefitId 
        /// </summary>
        /// <param name="BenefitId" ></param>
        public async Task<EmployeeBenefit> GetBenefitsviewBenefitId(int BenefitId,int companyId)
        {
            var employeeBenefit = new EmployeeBenefit();
            var employeeBenefitEntity = await _benefitRepository.GetBenefitByBenefitId(BenefitId,companyId);
            var employee = await _employeesRepository.GetEmployeeByIdView(employeeBenefitEntity.EmpId,companyId);
            var benefitTypeId = await _benefitRepository.GetBenefitTypeNameById(employeeBenefitEntity.BenefitTypeId);
            var employeeBenefits = _mapper.Map<EmployeeBenefit>(employeeBenefitEntity);
            if (employeeBenefits != null)
            {
                employeeBenefit.EmployeeName = employee != null ? employee.FirstName + " " + employee.LastName : String.Empty;
                employeeBenefit.BenefitName = benefitTypeId.BenefitName;
                employeeBenefit.EmployeeStatus = employee != null ? employee.IsDeleted : false;
            }
            return employeeBenefit;
        }

        //medicalbenefit

        /// <summary>
        /// Logic to get medicalbenefit list
        /// </summary>
        public async Task<EmployeeMedicalBenefitViewModel> GetAllMedicalBenefitDetails(int companyId)
        {
            var employeeMedicalBenefitViewModel = new EmployeeMedicalBenefitViewModel();
            var listOfBenefitTypes = await _benefitRepository.GetBenefitTypes();
            var listOfEmployees = await _employeesRepository.GetAllEmployees(companyId);
            employeeMedicalBenefitViewModel.BenefitTypeId = (int)BenefitType.MedicalBenefit;
            var result = new List<ReportingPerson>();
            var employeeslist = _mapper.Map<List<Employees>>(listOfEmployees);
            foreach (var item in employeeslist)
            {
                result.Add(new ReportingPerson()
                {
                    EmpId = item.EmpId,
                    EmployeeName = item.UserName + " - " + item?.FirstName + " " + item?.LastName,
                });
            }

            if (listOfBenefitTypes.Count() > 0)
            {
                var listEmployeeMedicalBenefits = await _benefitRepository.GetAllEmployeeMedicalBenefits(companyId);
                var benefitTypes = _mapper.Map<List<BenefitTypes>>(listOfBenefitTypes);
                employeeMedicalBenefitViewModel.BenefitTypes = benefitTypes;
                employeeMedicalBenefitViewModel.reportingPeople = result;
                employeeMedicalBenefitViewModel.EmployeeMedicalBenefits = listEmployeeMedicalBenefits;
            }
            return employeeMedicalBenefitViewModel;
        }


        /// <summary>
        /// Logic to get create abd update  the employeemedicalbenefit detail
         /// </summary>
        /// <param name="employeeMedicalBenefit" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> AddMedicalBenefits(EmployeeMedicalBenefit employeeMedicalBenefit, int sessionEmployeeId, int companyId)
        {
            var result = false;
            if (employeeMedicalBenefit.MedicalBenefitId == 0)
            {
                employeeMedicalBenefit.CreatedDate = DateTime.Now;
                employeeMedicalBenefit.CreatedBy = sessionEmployeeId;
                var employeeMedicalBenefitEntity = _mapper.Map<EmployeeMedicalBenefitEntity>(employeeMedicalBenefit);
                result = await _benefitRepository.AddMedicalBenefits(employeeMedicalBenefitEntity,companyId);
            }
            else
            {
                var employeebenefitEntitys = await _benefitRepository.GetMedicalBenefitByMedicalBenefitId(employeeMedicalBenefit.MedicalBenefitId,employeeMedicalBenefit.CompanyId);
                employeebenefitEntitys.EmpId = employeeMedicalBenefit.EmpId;
                employeebenefitEntitys.BenefitTypeId = employeeMedicalBenefit.BenefitTypeId;
                employeebenefitEntitys.Scheme = employeeMedicalBenefit.Scheme;
                employeebenefitEntitys.MembershipNumber = employeeMedicalBenefit.MembershipNumber;
                employeebenefitEntitys.Member = employeeMedicalBenefit.Member; employeebenefitEntitys.Category = employeeMedicalBenefit.Category;
                employeebenefitEntitys.Cost = employeeMedicalBenefit.Cost;
                employeebenefitEntitys.MedicalBenefitId = employeeMedicalBenefit.MedicalBenefitId;
                employeebenefitEntitys.UpdatedDate = DateTime.Now;
                employeebenefitEntitys.UpdatedBy = sessionEmployeeId;               
                result = await _benefitRepository.AddMedicalBenefits(employeebenefitEntitys, companyId);
            }
            return result;
        }

        /// <summary>
        /// Logic to get deleted  the employeemedicalbenefit detail
        /// </summary>
        /// <param name="employeeMedicalBenefit" ></param>        
        public async Task<bool> DeleteMedicalBenefit(EmployeeMedicalBenefit employeeMedicalBenefit, int sessionEmployeeId)
        {
            var employeeMedicalBenefitEntity = new EmployeeMedicalBenefitEntity();
            employeeMedicalBenefitEntity.MedicalBenefitId = employeeMedicalBenefit.MedicalBenefitId;
            employeeMedicalBenefitEntity.CompanyId = employeeMedicalBenefit.CompanyId;
            var result = false;
            if (employeeMedicalBenefit != null)
            {
                result = await _benefitRepository.DeleteMedicalBenefit(employeeMedicalBenefitEntity, sessionEmployeeId);
            }
            return result;
        }

        /// <summary>
        /// Logic to get view the employeeMedicalBenefit detail by particular MedicalBenefitId 
        /// </summary>
        /// <param name="MedicalBenefitId" ></param>
        public async Task<EmployeeMedicalBenefit> GetMedicalBenefitsviewBenefitId(int MedicalBenefitId,int companyId)
        {
            var employeeMedicalBenefit = new EmployeeMedicalBenefit();
            var employeeMedicalBenefitEntity = await _benefitRepository.GetMedicalBenefitByMedicalBenefitId(MedicalBenefitId,companyId);
            var employee = await _employeesRepository.GetEmployeeByIdView(employeeMedicalBenefitEntity.EmpId, companyId);
            var employeeMedicalBenefits = _mapper.Map<EmployeeMedicalBenefit>(employeeMedicalBenefitEntity);
            if (employeeMedicalBenefits != null)
            {
                employeeMedicalBenefit.EmployeeName = employee != null ? employee.FirstName + " " + employee.LastName : String.Empty;
                employeeMedicalBenefit.BenefitTypeId = employeeMedicalBenefits.BenefitTypeId;
                employeeMedicalBenefit.Scheme = employeeMedicalBenefits.Scheme;
                employeeMedicalBenefit.Category = employeeMedicalBenefits.Category;
                employeeMedicalBenefit.Cost = employeeMedicalBenefits.Cost;
                employeeMedicalBenefit.Member = employeeMedicalBenefits.Member;
                employeeMedicalBenefit.MembershipNumber = employeeMedicalBenefits.MembershipNumber;
            }
            return employeeMedicalBenefit;
        }

        /// <summary>
        ///  Logic to get all Benefit count 
        /// </summary>
        /// <param name="empId,companyId,pager" ></param>    
        public async Task<int> BenefitCount(int companyId, SysDataTablePager pager)
        {
            var result = await _benefitRepository.BenefitCount(companyId, pager);
            return result;
        }

        /// <summary>
        ///  Logic to get all Benefit filter data 
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection" ></param>  
        public async Task<List<BenefitFilterViewModel>> GetBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            var benefitfilter = await _benefitRepository.GetBenefitFilterView(companyId, pager, columnName, columnDirection);
            return benefitfilter;

        }

        /// <summary>
        ///  Logic to get all Medical Benefit count 
        /// </summary>
        /// <param name="companyId,pager" ></param>    
        public async Task<int> MedicalBenefitCount(int companyId, SysDataTablePager pager)
        {
            var medicalbenefitfilterCount = await _benefitRepository.MedicalBenefitCount(companyId, pager);
            return medicalbenefitfilterCount;
        }

        /// <summary>
        ///  Logic to get all Medical Benefit filter data 
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection" ></param>  
        public async Task<List<MedicalBenefitFilterViewModel>> GetMedicalBenefitFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            var medicalbenefitfilter = await _benefitRepository.GetMedicalBenefitFilterView(companyId, pager, columnName, columnDirection);
            return medicalbenefitfilter;

        }

    }
}
