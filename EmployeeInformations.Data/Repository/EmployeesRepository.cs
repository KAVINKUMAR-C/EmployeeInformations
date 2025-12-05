using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Data.Model;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{

    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly EmployeesDbContext _dbContext;
        public EmployeesRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Employees
        /// <summary>
          /// Logic to get  check username,password the employee detail by particular employee
        /// </summary>   
        /// <param name="username,password" >employee</param>       
        public async Task<EmployeesEntity> GetByUserName(string username, string password)
        {
            try {
                var employee = await _dbContext.Employees
               .FirstOrDefaultAsync(e =>
                   e.OfficeEmail == username &&
                   e.Password == password &&
                   !e.IsDeleted);

                return employee ?? new EmployeesEntity();
            }
            catch (Exception e)
            {

            }
            return new EmployeesEntity();
        }
        /// <summary>
        /// Logic to get employee list 
        /// </summary>           
        /// <param name="CompanyId" >employee</param>
        public async Task<List<EmployeesEntity>> GetAllEmployees(int companyId)
        {
            return await _dbContext.Employees.AsNoTracking().Where(x => x.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get employee list using dashboard and benefit
        /// </summary>           
        /// <param name="CompanyId" >employee</param>
        /// <param name="IsDeleted" >employee</param>
        public async Task<List<EmployeesEntity>> GetAllEmployeeDetails(int companyId)
        {
            return await _dbContext.Employees.Where(x => x.CompanyId == companyId && !x.IsDeleted).AsNoTracking().ToListAsync();

        }
        /// <summary>
        /// Logic to get employee list using attandance and In/Out details
        /// </summary>           
        /// <param name="CompanyId" >employee</param>
        /// <param name="IsDeleted" >employee</param>
        public async Task<List<EmployeesEntity>> GetAllEmpById(int empId,int companyId)
        {
            var result = await _dbContext.Employees.Where(x => x.CompanyId == companyId && !x.IsDeleted && x.EmpId == empId).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<List<EmployeesEntity>> GetTeambyId(List<int> empIds,int companyId)
        {
            var employeeEntity = await _dbContext.Employees.Where(e => empIds.Contains(e.EmpId) && e.CompanyId == companyId && !e.IsDeleted).AsNoTracking().ToListAsync();
            return employeeEntity;
        }


        /// <summary>
        /// Logic to get employee list using dashboard
        /// </summary>           
        /// <param name="CompanyId" >employee</param>
        /// <param name="IsActive" >employee</param>
        public async Task<List<EmployeesEntity>> GetAllActiveEmployees(int companyId)
        {
            return await _dbContext.Employees.Where(x => x.CompanyId == companyId && x.IsActive).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Logic to get employee list using reportingpersons
        /// </summary>           
        /// <param name="CompanyId" >employee</param>
        /// <param name="IsDeleted" >employee</param>
        public async Task<List<EmployeesEntity>> GetAllReportingPersons(int companyId)
        {
            return await _dbContext.Employees.Where(x => x.CompanyId == companyId && !x.IsDeleted && x.RoleId != Convert.ToInt32(Role.Employee)).ToListAsync();
        }


        /// <summary>
        /// Logic to get empId the employees detail by particular employees
        /// </summary>   
        /// <param name="empId" >employees</param>
        /// <param name="CompanyId" >employees</param>
        /// <param name="IsDeleted" >employees</param>
        public async Task<EmployeesEntity> GetEmployeeById(int EmpId,int companyId)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmpId == EmpId && e.CompanyId == companyId && !e.IsDeleted);
            return employeesEntity ?? new EmployeesEntity();
        }


        /// <summary>
        /// Logic to get empId the employees detail by particular employees only for employee view 
        /// </summary>   
        /// <param name="empId" >employees</param>
        /// <param name="CompanyId" >employees</param>       
        public async Task<EmployeesEntity> GetEmployeeByIdView(int EmpId,int companyId)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmpId == EmpId && e.CompanyId == companyId);
            return employeesEntity;
        }


        /// <summary>
        /// Logic to get employees by esslId
        /// </summary>   
        /// <param name="esslId" ></param>
        /// <param name="CompanyId" ></param>       
        public async Task<EmployeesEntity> GetEmployeeByEssId(int essl,int companyId)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EsslId == essl && e.CompanyId == companyId);
            return employeesEntity;
        }


        /// <summary>
        /// Logic to get esslId Active employees
        /// </summary>   
        /// <param name="esslId" ></param>
        /// <param name="CompanyId" ></param> 
        public async Task<EmployeesEntity> GetEmployeeByEssIdIsActive(int essl,int companyId)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EsslId == essl && e.CompanyId == companyId && !e.IsDeleted && e.IsActive == true);
            return employeesEntity;
        }


        /// <summary>
        /// Logic to get empId the employees detail by particular employees leave
        /// </summary>   
        /// <param name="empId" >employees</param>
        /// <param name="CompanyId" >employees</param>
        /// <param name="IsDeleted" >employees</param>
        public async Task<EmployeesEntity> GetEmployeeByIdForLeave(int EmpId,int companyId)
        {
            Common.Common.WriteServerErrorLog(" leave.EmpId : " + EmpId);
            var employeesEntity = await _dbContext.Employees.Where(e => e.EmpId == EmpId && e.CompanyId == companyId && e.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync();
            Common.Common.WriteServerErrorLog(" leave.EmpId : " + employeesEntity?.FirstName);
            return employeesEntity ?? new EmployeesEntity();
        }


        /// <summary>
        /// Logic to get acceptprobation the employees detail by particular employees
        /// </summary>   
        /// <param name="employees,sessionEmployeeId" >employees</param>
        /// <param name="CompanyId" >employees</param>
        public async Task<bool> AcceptProbation(Employees employees, int sessionEmployeeId,int companyId)
        {
            var result = false;
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmpId == employees.EmpId && e.CompanyId == companyId);
            if (employeesEntity != null)
            {
                employeesEntity.IsProbationary = true;
                employeesEntity.ProbationDate = DateTime.Now;
                _dbContext.Employees.Update(employeesEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get create and update the employees detail by particular employees
        /// </summary>   
        /// <param name="employeesEntity" ></param>
        public async Task<Int32> CreateEmployee(EmployeesEntity employeesEntity, int companyId)
        {
            var result = 0;
            try
            {
                if (employeesEntity?.EmpId == 0)
                {
                    employeesEntity.CompanyId = companyId;
                    await _dbContext.Employees.AddAsync(employeesEntity);
                    await _dbContext.SaveChangesAsync();

                    result = employeesEntity.EmpId;
                }
                else
                {
                    if (employeesEntity != null)
                    {
                        _dbContext.Employees.Update(employeesEntity);
                        await _dbContext.SaveChangesAsync();
                        result = employeesEntity.EmpId;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }


        /// <summary>
        /// Logic to get createemployeescompany the employees detail
        /// </summary>   
        /// <param name="employeesEntity" ></param>
        public async Task<Int32> CreateEmployeeFromCompany(EmployeesEntity employeesEntity)
        {
            var result = 0;
            if (employeesEntity?.EmpId == 0)
            {
                await _dbContext.Employees.AddAsync(employeesEntity);
                await _dbContext.SaveChangesAsync();
                result = employeesEntity.EmpId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get employees Status
        /// </summary>   
        /// <param name="employees" ></param>
        public async Task<List<EmployeesEntity>> GetByStatus(bool status, int companyId)
        {
            return await _dbContext.Employees.Where(c => c.IsDeleted == status && c.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get officeEmail the employees detail by particular employees
        /// </summary>   
        /// <param name="officeEmail" >employees</param>       
        public async Task<EmployeesEntity> getEmloyeeDetailByOfficeEmail(string officeEmail)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.OfficeEmail == officeEmail);
            return employeesEntity ?? new EmployeesEntity();
        }


        /// <summary>
        /// Logic to get updatepassword the employees detail by particular employees
        /// </summary>   
        /// <param name="employeesEntity" ></param>
        public async Task UpdateNewPassword(EmployeesEntity employeesEntity)
        {
            _dbContext.Employees.Update(employeesEntity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Logic to get updatecurrentpassword the employees detail by particular employees
        /// </summary>   
        /// <param name="employeesEntity" ></param>
        public async Task<bool> UpdateEmployeeCurrentPassword(EmployeesEntity employeesEntity)
        {
            var result = false;
            _dbContext.Employees.Update(employeesEntity);
            result = await _dbContext.SaveChangesAsync() > 0;          
            return result;
        }


        /// <summary>
        /// Logic to get update skill the employees detail by particular employees
        /// </summary>   
        /// <param name="employeesEntity" ></param> 
        /// <param name="CompanyId" >employees</param>
        public async Task<bool> UpdateSkill(EmployeesEntity employeesEntity)
        {
            var result = false;
            var employeesEntitys = await _dbContext.Employees.Where(e => e.EmpId == employeesEntity.EmpId && e.CompanyId == employeesEntity.CompanyId).FirstOrDefaultAsync();
            if (employeesEntitys != null)
            {
                employeesEntitys.SkillName = employeesEntity.SkillName;
                _dbContext.Employees.Update(employeesEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
                return result;
            }
            return result;
        }


        /// <summary>
        /// Logic to check employeee status
        /// </summary>   
        /// <param name="employeesEntity" ></param> 
        public async Task<bool> employeeIsverified(int empId,int companyId)
        {
            var result = false;
            var employeesEntitys = await _dbContext.Employees.FirstOrDefaultAsync(m => m.EmpId == empId && m.CompanyId == companyId);
            if (employeesEntitys != null)
            {
                employeesEntitys.IsVerified = true;
                _dbContext.Employees.Update(employeesEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get employee name using reporting person Id
        /// </summary>        
        /// <param name="ReportingPersonId" ></param>      
        public async Task<EmployeesEntity> GetEmployeeByname(int ReportingPersonId,int companyId)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmpId == ReportingPersonId && e.CompanyId == companyId && !e.IsDeleted);
            return employeesEntity;
        }


        /// <summary>
        /// Logic to get employee count  the employees detail 
        /// </summary>   
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> GetEmployeeMaxCount(int companyId)
        {
            var count = await _dbContext.Employees.Where(x => x.CompanyId == companyId && x.EsslId > 0).CountAsync();
            return count;
        }


        /// <summary>
        /// Logic to get topfiveemployees count  the employees detail 
        /// </summary>   
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> GetTopFiveEmployees(int companyId)
        {
            var topDepartment = await _dbContext.Employees.Where(x => !x.IsDeleted && x.CompanyId == companyId).CountAsync();
            return topDepartment;
        }


        /// <summary>
        /// Logic to get activeemployees count  the employees detail 
        /// </summary>   
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> GetActiveEmployeeMaxCount(int companyId)
        {
            var count = await _dbContext.Employees.Where(x => x.IsActive && x.CompanyId == companyId).CountAsync();
            return count;
        }


        /// <summary>
        /// Logic to get officeEmail count  the employees detail 
        /// </summary>   
        /// <param name="officeEmail" >employees</param>
        /// <param name="CompanyId" >employees</param>
        public async Task<int> GetEmployeeEmail(string officeEmail,int companyId)
        {
            var emailCount = await _dbContext.Employees.Where(x => x.OfficeEmail.ToLower() == officeEmail.ToLower() && x.CompanyId == companyId).CountAsync();
            return emailCount;
        }


        /// <summary>
        /// Logic to get isactive officeEmail count  the employees detail by particular employees
        /// </summary>   
        /// <param name="officeEmail" >employees</param>       
        public async Task<bool> EmployeeIsActiveCheck(string officeEmail)
        {
            var emailIsActiveCheck = false;
            var email = await _dbContext.Employees.Where(x => x.OfficeEmail.ToLower() == officeEmail.ToLower()).FirstOrDefaultAsync();
            emailIsActiveCheck = email != null && email.IsActive ? true : false;
            return emailIsActiveCheck;
        }


        /// <summary>
        /// Logic to get isactive personalEmail count  the employees detail by particular employees
        /// </summary>   
        /// <param name="personalEmail" >employees</param> 
        /// <param name="IsDeleted" >employees</param> 
        public async Task<int> GetPersonalEmail(string personalEmail)
        {
            var personal = await _dbContext.Employees.Where(y => !string.IsNullOrEmpty(y.PersonalEmail) && y.PersonalEmail.ToLower() == personalEmail.ToLower() && !y.IsDeleted).CountAsync();
            return personal;
        }

        /// <summary>
         /// Logic to get rejectemployees the employees detail by particular employees
        /// </summary>  
        /// <param name="employees" ></param>
        /// <param name="CompanyId" >employees</param>
        public async Task<bool> GetRejectEmployees(Employees employees)
        {
            var result = false;
            var employeeEntities = await _dbContext.Employees.Where(e => e.EmpId == employees.EmpId && e.CompanyId == employees.CompanyId).FirstOrDefaultAsync();
            if (employeeEntities != null)
            {
                var profileEntity = await _dbContext.ProfileInfo.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == employees.CompanyId).FirstOrDefaultAsync();
                var addressEntity = await _dbContext.AddressInfo.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == employees.CompanyId).FirstOrDefaultAsync();
                var otherDetailsEntity = await _dbContext.OtherDetails.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == employees.CompanyId).FirstOrDefaultAsync();
                var experienceEntity = await _dbContext.Experience.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == employees.CompanyId).FirstOrDefaultAsync();
                var qualificationEntity = await _dbContext.Qualification.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == employees.CompanyId).FirstOrDefaultAsync();
                var bankDetailsEntity = await _dbContext.BankDetails.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == employees.CompanyId).FirstOrDefaultAsync();
                if (employeeEntities != null)
                {
                    if (profileEntity != null)
                    {
                        profileEntity.IsDeleted = true;
                        _dbContext.ProfileInfo.Update(profileEntity);
                    }
                    if (addressEntity != null)
                    {
                        addressEntity.IsDeleted = true;
                        _dbContext.AddressInfo.Update(addressEntity);
                    }
                    if (otherDetailsEntity != null)
                    {
                        otherDetailsEntity.IsDeleted = true;
                        _dbContext.OtherDetails.Update(otherDetailsEntity);
                    }
                    if (experienceEntity != null)
                    {
                        experienceEntity.IsDeleted = true;
                        _dbContext.Experience.Update(experienceEntity);
                    }
                    if (qualificationEntity != null)
                    {
                        qualificationEntity.IsDeleted = true;
                        _dbContext.Qualification.Update(qualificationEntity);
                    }
                    if (bankDetailsEntity != null)
                    {
                        bankDetailsEntity.IsDeleted = true;
                        _dbContext.BankDetails.Update(bankDetailsEntity);
                    }

                    employeeEntities.RejectReason = employees.RejectReason;
                    employeeEntities.RelieveId = employees.RelieveId;
                    employeeEntities.IsDeleted = true;
                    employeeEntities.ReleavedDate= employees.ReleavedDate;
                    _dbContext.Employees.Update(employeeEntities);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get bloodgroup list
        /// </summary>   
        public async Task<List<BloodGroupEntity>> GetAllBloodGroup()
        {
            return await _dbContext.BloodGroup.ToListAsync();
        }


        /// <summary>
        /// Logic to get designations list
        /// </summary>           
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<DesignationEntity>> GetAllDesignation(int companyId)
        {
            return await _dbContext.Designations.Where(d => d.CompanyId == companyId && d.IsActive && !d.IsDeleted).ToListAsync();
        }

        //// Department

        /// <summary>
          /// Logic to get departments list
         /// </summary>           
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<DepartmentEntity>> GetAllDepartment(int companyId)
        {
            return await _dbContext.Departments.Where(f => f.CompanyId == companyId && f.IsActive && !f.IsDeleted).ToListAsync();
        }


        /// <summary>
        /// Logic to get departmentId the departments detail by particular departments
        /// </summary>  
        /// <param name="departmentId" >departments</param>
        /// <param name="CompanyId" >departments</param>
        public async Task<string> GetDepartmentNameByDepartmentId(int departmentId, int companyId)
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.DepartmentId == departmentId && x.CompanyId == companyId);
            if (department != null)
            {
                return department.DepartmentName;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get role list
        /// </summary>           
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<RoleEntity>> GetAllRoleTable(int companyId)
        {
            return await _dbContext.RoleEntities.Where(f => f.CompanyId == companyId && f.IsActive && !f.IsDeleted).ToListAsync();
        }

        //// Reporting person

        /// <summary>
        /// Logic to get empId the reportingpersonsentities detail by particular reportingpersonsentities
         /// </summary>   
        /// <param name="empId" >reportingpersonsentities</param>
        /// <param name="CompanyId" >reportingpersonsentities</param>
        /// <param name="IsDeleted" >reportingpersonsentities</param>
        public async Task<ReportingPersonsEntity> GetReportingPersonEmployeeId(int EmpId,int companyId)
        {
            var employeesEntity = await _dbContext.ReportingPersonsEntities.FirstOrDefaultAsync(e => e.EmployeeId == EmpId && e.CompanyId == companyId);
            return employeesEntity;
        }


        /// <summary>
        /// Logic to get empId the reportingpersonsentities detail 
        /// </summary>   
        /// <param name="empId" >reportingpersonsentities</param>
        /// <param name="CompanyId" >reportingpersonsentities</param>   
        public async Task<List<ReportingPersonsEntity>> GetAllReportingPersonsEmpIdForLeave(int empId, int companyId)
        {
            return await _dbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == empId && x.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get empId the reportingpersonsentities detail
        /// </summary>   
        /// <param name="empId" >reportingpersonsentities</param>
        /// <param name="CompanyId" >reportingpersonsentities</param>
        public async Task<List<ReportingPersonsEntity>> GetAllEmployeeIdsReportingPersonForLeave(int empId,int companyId)
        {
            return await _dbContext.ReportingPersonsEntities.Where(x => x.ReportingPersonEmpId == empId && x.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get empId the reportingpersonsentities detail 
        /// </summary>   
        /// <param name="empId" >reportingpersonsentities</param>
        /// <param name="CompanyId" >reportingpersonsentities</param>
        /// <param name="reportingpersonempId" >reportingpersonsentities</param>
        public async Task<List<int>> GetReportingPersonEmployeeById(int EmpId,int companyId)
        {
            IList<Employees> reportersList = new List<Employees>();
            var reportingPersonsEntity = await _dbContext.ReportingPersonsEntities.Where(e => e.EmployeeId == EmpId && e.CompanyId == companyId).Select(x => x.ReportingPersonEmpId).ToListAsync();
            return reportingPersonsEntity;
        }


        /// <summary>
        /// Logic to get Create,update,delete the reportingpersonsentities detail
        /// </summary>   
        /// <param name="reportingPersonsEntitys,EmpId" >reportingpersonsentities</param>
        public async Task CreateReportingPersons(List<ReportingPersonsEntity> reportingPersonsEntitys, int EmpId)
        {
            var reportingPersonsEntities = await _dbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == EmpId).ToListAsync();
            if (reportingPersonsEntities.Count() == 0)
            {
                await _dbContext.ReportingPersonsEntities.AddRangeAsync(reportingPersonsEntitys);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _dbContext.ReportingPersonsEntities.RemoveRange(reportingPersonsEntities);
                await _dbContext.SaveChangesAsync();

                _dbContext.ReportingPersonsEntities.UpdateRange(reportingPersonsEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// Experience 

        /// <summary>
        /// Logic to get empId the experience detail 
         /// </summary>   
        /// <param name="empId" >experience</param>
        /// <param name="CompanyId" >experience</param>
        /// <param name="IsDeleted" >experience</param>
        public async Task<List<ExperienceEntity>> GetAllExperience(int EmpId, int companyId)
        {
            var experienceEntity = await _dbContext.Experience.Where(x => x.EmpId == EmpId && x.Employees.CompanyId == companyId && !x.IsDeleted).ToListAsync();
            return experienceEntity;
        }


        /// <summary>
        /// Logic to get empId the experience detail 
        /// </summary>   
        /// <param name="empId" >experience</param>
        /// <param name="CompanyId" >experience</param>
        /// <param name="IsDeleted" >experience</param>
        public async Task<List<ExperienceEntity>> GetAllExperienceViewModel(int empId, int companyId)
        {
            var experienceEntity = await _dbContext.Experience.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId && !e.IsDeleted).ToListAsync();
            return experienceEntity;
        }


        /// <summary>
        /// Logic to get empId the experience detail only foe view 
        /// </summary>   
        /// <param name="empId" >experience</param>
        /// <param name="CompanyId" >experience</param>      
        public async Task<List<ExperienceEntity>> GetAllExperienceView(int empId, int companyId)
        {
            var experienceEntity = await _dbContext.Experience.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId && !e.IsDeleted).ToListAsync();
            return experienceEntity;
        }


        /// <summary>
        /// Logic to get empId the experience detail by particular experience
        /// </summary>   
        /// <param name="empId" >experience</param>
        /// <param name="CompanyId" >experience</param>
        /// <param name="IsDeleted" >experience</param>
        public async Task<ExperienceEntity> GetExperienceByEmployeeId(int empId,int companyId)
        {
            var experienceEntity = await _dbContext.Experience.AsNoTracking().FirstOrDefaultAsync(q => q.EmpId == empId && q.Employees.CompanyId == companyId && !q.IsDeleted);
            return experienceEntity;
        }


        /// <summary>
        /// Logic to get experienceId the experience detail by particular experience
        /// </summary>   
        /// <param name="experienceId" >experience</param>
        /// <param name="CompanyId" >experience</param>
        /// <param name="IsDeleted" >experience</param>
        public async Task<ExperienceEntity> GetExperienceByExperienceId(int ExperienceId,int companyId)
        {
            var experienceEntity = await _dbContext.Experience.AsNoTracking().FirstOrDefaultAsync(g => g.ExperienceId == ExperienceId && g.Employees.CompanyId == companyId && !g.IsDeleted);
            return experienceEntity ?? new ExperienceEntity();
        }


        /// <summary>
        /// Logic to get create and update the experience detail by particular experience
        /// </summary>   
        /// <param name="experienceEntity" ></param>
        public async Task<int> InsertAndUpdateExperience(ExperienceEntity experienceEntity)
        {
            var result = 0;
            if (experienceEntity?.ExperienceId == 0)
            {
                await _dbContext.Experience.AddAsync(experienceEntity);
                await _dbContext.SaveChangesAsync();
                result = experienceEntity.ExperienceId;
            }
            else
            {
                if (experienceEntity != null)
                {
                    _dbContext.Experience.Update(experienceEntity);
                    await _dbContext.SaveChangesAsync();
                    result = experienceEntity.ExperienceId;
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the experience detail by particular experience
        /// </summary>   
        /// <param name="experienceEntity" ></param>
        public async Task DeleteExperience(ExperienceEntity experienceEntity)
        {
            _dbContext.Experience.Update(experienceEntity);
            await _dbContext.SaveChangesAsync();
        }

        ///// Other Details 

        /// <summary>
        /// Logic to get empId the otherdetails detail 
        /// </summary>   
        /// <param name="empId" >otherdetails</param>
        /// <param name="CompanyId" >otherdetails</param>
        /// <param name="IsDeleted" >otherdetails</param>
        public async Task<List<OtherDetailsEntity>> GetAllOtherDetailsViewModel(int empId, int companyId)
        {
            var otherdetailsEntity = await _dbContext.OtherDetails.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId && !e.IsDeleted).ToListAsync();
            return otherdetailsEntity;
        }


        /// <summary>
        /// Logic to get empId the otherdetails detail only for view
        /// </summary>   
        /// <param name="empId" >otherdetails</param>
        /// <param name="CompanyId" >otherdetails</param>        
        public async Task<List<OtherDetailsEntity>> GetAllOtherDetailsView(int empId,int companyId)
        {
            var otherdetailsEntity = await _dbContext.OtherDetails.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId && !e.IsDeleted).ToListAsync();
            return otherdetailsEntity;
        }


        /// <summary>
        /// Logic to get empId the otherdetails detail 
        /// </summary>   
        /// <param name="empId" >otherdetails</param>
        /// <param name="CompanyId" >otherdetails</param>
        /// <param name="IsDeleted" >otherdetails</param>
        public async Task<List<OtherDetailsEntity>> GetAllOtherDetails(int EmpId, int companyId)
        {
            var otherDetailsEntity = await _dbContext.OtherDetails.Where(x => x.EmpId == EmpId && x.Employees.CompanyId == companyId && !x.IsDeleted).ToListAsync();
            return otherDetailsEntity;
        }


        /// <summary>
        /// Logic to get empId the otherdetails detail by particular otherdetails
        /// </summary>   
        /// <param name="empId" >otherdetails</param>
        /// <param name="CompanyId" >otherdetails</param>
        /// <param name="IsDeleted" >otherdetails</param>
        public async Task<OtherDetailsEntity> GetOtherDetailsByEmployeeId(int empId,int companyId)
        {
            var otherDetailsEntity = await _dbContext.OtherDetails.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return otherDetailsEntity;
        }


        /// <summary>
        /// Logic to get detailId the otherdetails detail by particular otherdetails
        /// </summary>   
        /// <param name="detailId" >otherdetails</param>
        /// <param name="CompanyId" >otherdetails</param>
        /// <param name="IsDeleted" >otherdetails</param>
        public async Task<OtherDetailsEntity> GetotherDetailsBydetailId(int detailId, int companyId)
        {
            var otherDetailsEntity = await _dbContext.OtherDetails.AsNoTracking().FirstOrDefaultAsync(w => w.DetailId == detailId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return otherDetailsEntity ?? new OtherDetailsEntity();
        }


        /// <summary>
        /// Logic to get create and update the otherdetails detail by particular otherdetails
        /// </summary>   
        /// <param name="otherDetailsEntity" ></param>
        public async Task<int> InsertAndUpdateOtherDetails(OtherDetailsEntity otherDetailsEntity)
        {
            var result = 0;
            if (otherDetailsEntity?.DetailId == 0)
            {
                await _dbContext.OtherDetails.AddAsync(otherDetailsEntity);
                await _dbContext.SaveChangesAsync();
                result = otherDetailsEntity.DetailId;

            }
            else
            {
                if (otherDetailsEntity != null)
                {
                    _dbContext.OtherDetails.Update(otherDetailsEntity);
                    await _dbContext.SaveChangesAsync();
                    result = otherDetailsEntity.DetailId;
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the otherdetails detail by particular otherdetails
        /// </summary>   
        /// <param name="otherDetailsEntity" ></param>
        public async Task DeleteOtherDetails(OtherDetailsEntity otherDetailsEntity)
        {
            _dbContext.OtherDetails.Update(otherDetailsEntity);
            await _dbContext.SaveChangesAsync();
        }

        //// Qualification

        /// <summary>
        /// Logic to get empId the qualification detail 
         /// </summary>   
        /// <param name="empId" >qualification</param>
        /// <param name="CompanyId" >qualification</param>
        /// <param name="IsDeleted" >qualification</param>
        public async Task<List<QualificationEntity>> GetAllQulificationViewModel(int empId,int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.Where(b => b.EmpId == empId && b.Employees.CompanyId == companyId && !b.IsDeleted).ToListAsync();
            return qualificationEntity;
        }


        /// <summary>
        /// Logic to get Create,update the qualification detail
        /// </summary>   
        /// <param name="qualificationEntity" ></param>
        public async Task<Int32> InsertAndUpdateQualification(QualificationEntity qualificationEntity)
        {
            var result = 0;
            if (qualificationEntity?.QualificationId == 0)
            {
                await _dbContext.Qualification.AddAsync(qualificationEntity);
                await _dbContext.SaveChangesAsync();
                result = qualificationEntity.QualificationId;
            }
            else
            {
                if (qualificationEntity != null)
                {
                    _dbContext.Qualification.Update(qualificationEntity);
                    await _dbContext.SaveChangesAsync();
                    result = qualificationEntity.QualificationId;
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the qualification detail by particular qualification
        /// </summary>   
        /// <param name="qualificationEntity" ></param>
        public async Task DeleteQualification(QualificationEntity qualificationEntity)
        {
            _dbContext.Qualification.Update(qualificationEntity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Logic to get empId the qualification detail only for view
        /// </summary>   
        /// <param name="empId" >qualification</param>
        /// <param name="CompanyId" >qualification</param>      
        public async Task<List<QualificationEntity>> GetAllQulificationView(int empId,int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.Where(b => b.EmpId == empId && b.Employees.CompanyId == companyId && !b.IsDeleted).ToListAsync();
            return qualificationEntity;
        }


        /// <summary>
        /// Logic to get empId using view the qualification detail 
        /// </summary>   
        /// <param name="empId" >qualification</param>
        /// <param name="CompanyId" >qualification</param>
        /// <param name="IsDeleted" >qualification</param>
        public async Task<List<QualificationEntity>> GetAllQualification(int EmpId,int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.Where(x => x.EmpId == EmpId && x.Employees.CompanyId == companyId && !x.IsDeleted).ToListAsync();
            return qualificationEntity;
        }


        /// <summary>
        /// Logic to get empId the qualification detail by particular qualification
        /// </summary>   
        /// <param name="empId" >qualification</param>
        /// <param name="CompanyId" >qualification</param>
        /// <param name="IsDeleted" >qualification</param>
        public async Task<QualificationEntity> GetQualificationByEmployeeId(int empId, int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return qualificationEntity;
        }


        /// <summary>
        /// Logic to get qualificationId the qualification detail by particular qualification
        /// </summary>   
        /// <param name="qualificationId" >qualification</param>
        /// <param name="CompanyId" >qualification</param>
        /// <param name="IsDeleted" >qualification</param>
        public async Task<QualificationEntity> GetQualificationByQualificationId(int QualificationId, int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.FirstOrDefaultAsync(w => w.QualificationId == QualificationId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return qualificationEntity ?? new QualificationEntity();
        }

        //// Profile Info

        /// <summary>
        /// Logic to get empId the profileinfo detail by particular profileinfo
        /// </summary>   
        /// <param name="empId" >profileinfo</param>
        /// <param name="CompanyId" >profileinfo</param>
        /// <param name="IsDeleted" >profileinfo</param>
        public async Task<ProfileInfoEntity> GetProfileByEmployeeId(int empId,int companyId)
        {
            var profileInfo = await _dbContext.ProfileInfo.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return profileInfo;
        }


        /// <summary>
        /// Logic to get create and update the profileinfo detail by particular profileinfo
        /// </summary>   
        /// <param name="profileInfoEntity" ></param>
        public async Task<int> AddProfileInfo(ProfileInfoEntity profileInfoEntity, bool isAdd = false)
        {
            var result = 0;
            Common.Common.WriteServerErrorLog("DateOfJoining : " + profileInfoEntity.DateOfJoining);
            if (profileInfoEntity?.ProfileId == 0 && isAdd)
            {
                await _dbContext.ProfileInfo.AddAsync(profileInfoEntity);
                await _dbContext.SaveChangesAsync();
                result = profileInfoEntity.EmpId;
            }
            else
            {
                if (profileInfoEntity != null)
                {
                    _dbContext.ProfileInfo.Update(profileInfoEntity);
                    await _dbContext.SaveChangesAsync();
                    result = profileInfoEntity.EmpId;
                }
            }

            return result;
        }


        /// <summary>
        /// Logic to get empId the profileinfo detail by particular profileinfo only for profileview
        /// </summary>   
        /// <param name="empId" >profileinfo</param>
        /// <param name="CompanyId" >profileinfo</param>
        /// <param name="IsDeleted" >profileinfo</param>
        public async Task<ProfileInfoEntity> GetProfileByEmployeeIdview(int empId,int companyId)
        {
            var profileInfo = await _dbContext.ProfileInfo.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId);
            return profileInfo;
        }


        /// <summary>
        /// Logic to get empId the profileinfo detail
        /// </summary>    
        /// <param name="CompanyId" >profileinfo</param>       
        public async Task<List<ProfileInfoEntity>> GetAllEmployeeProfile(int companyId)
        {
            return await _dbContext.ProfileInfo.AsNoTracking().Where(x => x.Employees.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get empId the profileinfo detail
        /// </summary>    
        /// <param name="CompanyId" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<ProfileInfoEntity>> GetAllEmployeeProfileIsDeleted(int companyId)
        {
            return await _dbContext.ProfileInfo.Where(x => !x.IsDeleted && x.Employees.CompanyId == companyId).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Logic to get employees by joining date 
        /// </summary>    
        /// <param name="Current date" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<ProfileInfoEntity>> GetAllEmployeeProfileByJoiningDateIsDeleted(DateTime currentdate, int companyId)
        {
            return await _dbContext.ProfileInfo.Where(x => x.DateOfJoining <= currentdate.Date && !x.IsDeleted && x.Employees.CompanyId == companyId).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Logic to get employees leave
        /// </summary>    
        /// <param name="Current date" ></param>
        /// <param name="IsDeleted" ></param>
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllEmployeeLeaveIsDeleted(int companyId)
        {
            return await _dbContext.employeeAppliedLeaveEntities.Where(x => !x.IsDeleted && x.CompanyId == companyId).AsNoTracking().ToListAsync();
        }

        /// Address Info 

        /// <summary>
        /// Logic to get empId the addressinfo detail by particular addressinfo
        /// </summary>    
        /// <param name="CompanyId" >addressinfo</param>
        /// <param name="empId" >addressinfo</param>
        public async Task<AddressInfoEntity> GetAddressByEmployeeId(int empId,int companyId)
        {
            var addressInfoEntity = await _dbContext.AddressInfo.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId);
            return addressInfoEntity;
        }


        /// <summary>
        /// Logic to get create and update the addressinfo detail by particular addressinfo
        /// </summary>   
        /// <param name="addressInfoEntity" ></param>    
        public async Task<int> AddAddressInfo(AddressInfoEntity addressInfoEntity)
        {
            var result = 0;
            if (addressInfoEntity?.AddressId == 0)
            {
                await _dbContext.AddressInfo.AddAsync(addressInfoEntity);
                await _dbContext.SaveChangesAsync();
                result = addressInfoEntity.EmpId;
            }
            else
            {
                if (addressInfoEntity != null)
                {
                    _dbContext.AddressInfo.Update(addressInfoEntity);
                    await _dbContext.SaveChangesAsync();
                    result = addressInfoEntity.EmpId;
                }
            }

            return result;
        }

        //// Bank Details 

        /// <summary>
         /// Logic to get empId the bankdetails detail by particular bankdetails
         /// </summary>   
        /// <param name="empId" >bankdetails</param>
        /// <param name="CompanyId" >bankdetails</param>       
        public async Task<BankDetailsEntity> GetBankDetailsByEmployeeId(int empId, int companyId)
        {
            var bankDetailsEntity = await _dbContext.BankDetails.AsNoTracking().FirstOrDefaultAsync(g => g.EmpId == empId && g.Employees.CompanyId == companyId);
            return bankDetailsEntity;
        }


        /// <summary>
        /// Logic to get create and update the bankdetails detail by particular bankdetails
        /// </summary>   
        /// <param name="bankDetailsEntity" ></param>
        public async Task<int> AddBankDetails(BankDetailsEntity bankDetailsEntity)
        {
            var result = 0;
            if (bankDetailsEntity?.BankId == 0)
            {
                await _dbContext.BankDetails.AddAsync(bankDetailsEntity);
                await _dbContext.SaveChangesAsync();
                result = bankDetailsEntity.EmpId;
            }
            else
            {
                if (bankDetailsEntity != null)
                {
                    _dbContext.BankDetails.Update(bankDetailsEntity);
                    await _dbContext.SaveChangesAsync();
                    result = bankDetailsEntity.EmpId;
                }
            }
            return result;
        }

        //// Qualification Attachment

        /// <summary>
         /// Logic to get Create,update,delete the qualificationattachmententitys detail
         /// </summary>   
        /// <param name="qualificationAttachments,qualificationId" ></param>
        public async Task<bool> InsertQualificationAttachment(List<QualificationAttachmentsEntity> qualificationAttachments, int qualificationId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.QualificationAttachmentEntitys.Where(x => x.QualificationId == qualificationId).ToListAsync();
            if (attachmentsEntitys.Count() == 0)
            {
                await _dbContext.QualificationAttachmentEntitys.AddRangeAsync(qualificationAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                _dbContext.QualificationAttachmentEntitys.RemoveRange(attachmentsEntitys);
                await _dbContext.SaveChangesAsync();

                _dbContext.QualificationAttachmentEntitys.UpdateRange(qualificationAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the qualificationattachmententitys detail by particular qualificationattachmententitys
        /// </summary>   
        /// <param name="qualificationAttachmentsEntity" ></param>
        public async Task DeleteQualificationAttachement(List<QualificationAttachmentsEntity> qualificationAttachmentsEntity)
        {
            _dbContext.QualificationAttachmentEntitys.UpdateRange(qualificationAttachmentsEntity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Logic to get qualificationId the qualificationattachmententitys detail 
        /// </summary>  
        /// <param name="qualificationId" >qualificationattachmententitys</param>
        /// <param name="IsDeleted" >qualificationattachmententitys</param>
        public async Task<List<QualificationAttachmentsEntity>> GetQualificationDocumentAndFilePath(int qualificationId)
        {
            var docNmaes = await _dbContext.QualificationAttachmentEntitys.Where(e => e.QualificationId == qualificationId && !e.IsDeleted).ToListAsync();
            return docNmaes;
        }


        /// <summary>
        /// Logic to get qualificationId using employeeslog the qualificationattachmententitys detail by particular qualificationId
        /// </summary>        
        /// <param name="qualificationId" >qualificationattachmententitys</param>
        public async Task<QualificationAttachmentsEntity> GetQualificationAttachmentsByEmployeeId(int qualificationId)
        {
            var qualificationAttachmentsEntity = await _dbContext.QualificationAttachmentEntitys.FirstOrDefaultAsync(g => g.QualificationId == qualificationId);
            return qualificationAttachmentsEntity;
        }


        /// <summary>
        /// Logic to get qualificationId using employeeslog the qualificationattachmententitys detail 
        /// </summary>        
        /// <param name="qualificationId" >qualificationattachmententitys</param>
        /// <param name="IsDeleted" >qualificationattachmententitys</param>
        public async Task<List<QualificationAttachmentsEntity>> GetAllQualificationAttachments(int qualificationId)
        {
            return await _dbContext.QualificationAttachmentEntitys.Where(x => x.QualificationId == qualificationId && !x.IsDeleted).ToListAsync();
        }


        /// <summary>
        /// Logic to get qualificationId based QualificationName the qualificationattachmententitys detail 
        /// </summary>  
        /// <param name="qualificationId" >qualificationattachmententitys</param>
        /// <param name="IsDeleted" >qualificationattachmententitys</param>
        /// <param name="QualificationName" >qualificationattachmententitys</param>
        public async Task<List<string>> GetDocumentNameByQualificationId(int qualificationId)
        {
            var quaificationName = new List<string>();
            var result = await _dbContext.QualificationAttachmentEntitys.Where(e => e.QualificationId == qualificationId && !e.IsDeleted).Select(x => x.QualificationName).ToListAsync();
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        quaificationName.Add(item);
                    }
                }
                return quaificationName;
            }
            return new List<string>();
        }

        ///// Experience Attachment

        /// <summary>
        /// Logic to get Create,update,delete the experienceattachmentsentitys detail
         /// </summary>   
        /// <param name="experienceAttachments,experienceId" ></param>
        public async Task<bool> InsertExperienceAttachment(List<ExperienceAttachmentsEntity> experienceAttachments, int experienceId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.ExperienceAttachmentsEntitys.Where(x => x.ExperienceId == experienceId).ToListAsync();
            if (attachmentsEntitys.Count() == 0)
            {
                await _dbContext.ExperienceAttachmentsEntitys.AddRangeAsync(experienceAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                _dbContext.ExperienceAttachmentsEntitys.RemoveRange(attachmentsEntitys);
                await _dbContext.SaveChangesAsync();

                _dbContext.ExperienceAttachmentsEntitys.UpdateRange(experienceAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get experienceId based experiencename  the experienceattachmententitys detail 
        /// </summary>  
        /// <param name="experienceId" >experienceattachmententitys</param>
        /// <param name="IsDeleted" >experienceattachmententitys</param>
        /// <param name="experiencename" >experienceattachmententitys</param>
        public async Task<List<string>> GetDocumentNameByExperienceId(int experienceId)
        {
            var result = await _dbContext.ExperienceAttachmentsEntitys.Where(e => e.ExperienceId == experienceId && !e.IsDeleted).Select(x => x.ExperienceName).ToListAsync();
            return result;
        }


        /// <summary>
        /// Logic to get experienceId the experienceattachmententitys detail 
        /// </summary>  
        /// <param name="experienceId" >experienceattachmententitys</param>
        /// <param name="IsDeleted" >experienceattachmententitys</param>
        public async Task<List<ExperienceAttachmentsEntity>> GetExperienceDocumentAndFilePath(int experienceId)
        {
            var docNmae = await _dbContext.ExperienceAttachmentsEntitys.Where(e => e.ExperienceId == experienceId && !e.IsDeleted).ToListAsync();
            return docNmae;
        }


        /// <summary>
         /// Logic to get delete the experienceattachmententitys detail by particular experienceattachmententitys
         /// </summary>   
        /// <param name="experienceAttachmentsEntity" ></param>
        public async Task DeleteExperienceAttachement(List<ExperienceAttachmentsEntity> experienceAttachmentsEntity)
        {
            _dbContext.ExperienceAttachmentsEntitys.UpdateRange(experienceAttachmentsEntity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Logic to get experienceId using employeeslog the experienceattachmententitys detail by particular experienceId
        /// </summary>        
        /// <param name="experienceId" >experienceattachmententitys</param>
        public async Task<ExperienceAttachmentsEntity> GetExperienceAttachmentsByEmployeeId(int experienceId)
        {
            var experienceAttachmentsEntity = await _dbContext.ExperienceAttachmentsEntitys.FirstOrDefaultAsync(g => g.ExperienceId == experienceId);
            return experienceAttachmentsEntity;
        }


        /// <summary>
        /// Logic to get experienceId using employeeslog the experienceattachmententitys detail 
        /// </summary>        
        /// <param name="experienceId" >experienceattachmententitys</param>
        /// <param name="IsDeleted" >experienceattachmententitys</param>
        public async Task<List<ExperienceAttachmentsEntity>> GetAllExperienceAttachment(int experienceId)
        {
            return await _dbContext.ExperienceAttachmentsEntitys.Where(x => x.ExperienceId == experienceId && !x.IsDeleted).ToListAsync();
        }

        //// other details attachment 

        /// <summary>
        /// Logic to get Create,update,delete the otherdetailsattachmentsentitys detail
        /// </summary>   
        /// <param name="experienceAttachments,detailId" ></param>
        public async Task<bool> InsertOtherDetailsAttachment(List<OtherDetailsAttachmentsEntity> otherDetailsAttachments, int detailId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.OtherDetailsAttachmentsEntitys.Where(x => x.DetailId == detailId).ToListAsync();
            if (attachmentsEntitys.Count() == 0)
            {
                await _dbContext.OtherDetailsAttachmentsEntitys.AddRangeAsync(otherDetailsAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                _dbContext.OtherDetailsAttachmentsEntitys.RemoveRange(attachmentsEntitys);
                await _dbContext.SaveChangesAsync();

                _dbContext.OtherDetailsAttachmentsEntitys.UpdateRange(otherDetailsAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the otherdetailsAttachmentsEntity detail by particular otherdetailsAttachmentsEntity
        /// </summary>   
        /// <param name="otherdetailsAttachmentsEntity" ></param>
        public async Task DeleteOtherDetailsAttachement(List<OtherDetailsAttachmentsEntity> otherDetailsAttachmentsEntity)
        {
            _dbContext.OtherDetailsAttachmentsEntitys.UpdateRange(otherDetailsAttachmentsEntity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Logic to get detailId based DocumentName the otherdetailsattachmententitys detail 
        /// </summary>  
        /// <param name="detailId" >otherdetailsattachmententitys</param>
        /// <param name="IsDeleted" >otherdetailsattachmententitys</param>
        /// <param name="DocumentName" >otherdetailsattachmententitys</param>
        public async Task<List<string>> GetDocumentNameByDetailId(int detailId)
        {
            var result = await _dbContext.OtherDetailsAttachmentsEntitys.Where(e => e.DetailId == detailId && !e.IsDeleted).Select(x => x.DocumentName).ToListAsync();
            return result;
        }


        /// <summary>
        /// Logic to get detailId the otherdetailsattachmententitys detail 
        /// </summary>  
        /// <param name="detailId" >otherdetailsattachmententitys</param>
        /// <param name="IsDeleted" >otherdetailsattachmententitys</param>       
        public async Task<List<OtherDetailsAttachmentsEntity>> GetOtherDetailsDocumentAndFilePath(int detailId)
        {
            var docNmae = await _dbContext.OtherDetailsAttachmentsEntitys.Where(e => e.DetailId == detailId && !e.IsDeleted).ToListAsync();
            return docNmae;
        }


        /// <summary>
        /// Logic to get detailId using employeeslog the otherdetailsattachmententitys detail by particular detailId
        /// </summary>        
        /// <param name="detailId" >otherdetailsattachmententitys</param>
        public async Task<OtherDetailsAttachmentsEntity> GetOtherDetailsAttachmentsByEmployeeId(int detailId)
        {
            var otherDetailsAttachmentsEntity = await _dbContext.OtherDetailsAttachmentsEntitys.FirstOrDefaultAsync(k => k.DetailId == detailId);
            return otherDetailsAttachmentsEntity;
        }


        /// <summary>
        /// Logic to get detailId using employeeslog the otherdetailsattachmententitys detail 
        /// </summary>        
        /// <param name="detailId" >otherdetailsattachmententitys</param>
        /// <param name="IsDeleted" >otherdetailsattachmententitys</param>
        public async Task<List<OtherDetailsAttachmentsEntity>> GetAllOtherDetailsAttachment(int detailId)
        {
            return await _dbContext.OtherDetailsAttachmentsEntitys.Where(x => x.DetailId == detailId && !x.IsDeleted).ToListAsync();
        }


        /// <summary>
        /// Logic to get states list
        /// </summary>         
        public async Task<List<StateEntity>> GetAllStates()
        {
            return await _dbContext.state.ToListAsync();
        }


        /// <summary>
        /// Logic to get countryId the state detail 
        /// </summary>   
        /// <param name="countryId" >cities</param>       
        public async Task<List<StateEntity>> GetByCountryId(int countryId)
        {
            return await _dbContext.state.Where(c => c.CountryId == countryId).ToListAsync();
        }


        /// <summary>
        /// Logic to get employeesreleaveingtypeentities list
        /// </summary> 
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<EmployeesReleaveingTypeEntity>> GetAllReleaveTypes(int companyId)
        {
            try
            {
                return await _dbContext.EmployeesReleaveingTypeEntities.Where(x => !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
         /// Logic to get skills list
        /// </summary> 
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<SkillSetEntity>> GetAllSkills(int companyId)
        {
            return await _dbContext.SkillSets.Where(c => c.IsActive && !c.IsDeleted && c.CompanyId == companyId).ToListAsync();
        }

        /// <summary>
        /// Logic to get documenttypes list
        /// </summary> 
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        /// <param name="IsActive" ></param>
        public async Task<List<DocumentTypesEntity>> GetAllDocumentTypes(int companyId)
        {
            return await _dbContext.documentTypesEntities.Where(c => c.IsActive && !c.IsDeleted && c.CompanyId == companyId).ToListAsync();
        }

        //// city 

        /// <summary>
         /// Logic to get city list
        /// </summary> 
        /// <param name="IsDeleted" ></param>
        public async Task<List<CityEntity>> GetAllCities()
        {
            return await _dbContext.cities.Where(x => !x.IsDeleted).ToListAsync();
        }


        /// <summary>
        /// Logic to get stateId the city detail 
        /// </summary>   
        /// <param name="StateId" >cities</param>
        /// <param name="IsDeleted" >cities</param>
        public async Task<List<CityEntity>> GetByStateId(int StateId)
        {
            return await _dbContext.cities.Where(c => c.StateId == StateId && !c.IsDeleted).ToListAsync();
        }


        /// <summary>
        /// Logic to get cityId the cities detail by particular cityId
        /// </summary>  
        /// <param name="cityId" >cities</param> 
        public async Task<string> GetCityNameByCityId(int cityId)
        {
            var cities = await _dbContext.cities.FirstOrDefaultAsync(x => x.CityId == cityId && !x.IsDeleted);
            if (cities != null)
            {
                return cities.CityName;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get secondaryCityId the cities detail by particular cityId
        /// </summary>  
        /// <param name="SecondaryCityId" >cities</param> 
        public async Task<string> GetCityNameBySecondaryCityId(int? SecondaryCityId)
        {
            var cities = await _dbContext.cities.FirstOrDefaultAsync(x => x.CityId == SecondaryCityId && !x.IsDeleted);
            if (cities != null)
            {
                return cities.CityName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Logic to get relationshiptype list
        /// </summary>   
        public async Task<List<RelationshipTypeEntity>> GetAllRelationshipType()
        {
            return await _dbContext.Relationship.ToListAsync();
        }

        ///// Country 

        /// <summary>
        /// Logic to get country list
        /// </summary>         
        public async Task<List<CountryEntity>> GetAllCountry()
        {
            return await _dbContext.CountryEntities.ToListAsync();
        }


        /// <summary>
        /// Logic to get countryId the countryentities detail by particular countryentities
        /// </summary>  
        /// <param name="countryId" >countryentities</param>       
        public async Task<string> GetCountryNameByCountryId(int countryId)
        {
            var countryEntities = await _dbContext.CountryEntities.FirstOrDefaultAsync(x => x.CountryId == countryId);
            if (countryEntities != null)
            {
                return countryEntities.Name;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get secondarycountryId the countryentities detail by particular countryentities
        /// </summary>  
        /// <param name="SecondaryCountryId" >countryentities</param>       
        public async Task<string> GetCountryNameBySecondaryCountryId(int? SecondaryCountryId)
        {
            var countryEntities = await _dbContext.CountryEntities.FirstOrDefaultAsync(x => x.CountryId == SecondaryCountryId);
            if (countryEntities != null)
            {
                return countryEntities.Name;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get stateId the state detail by particular state
        /// </summary>  
        /// <param name="stateId" >state</param> 
        public async Task<string> GetStateNameByStateId(int stateId)
        {
            var state = await _dbContext.state.FirstOrDefaultAsync(x => x.StateId == stateId);
            if (state != null)
            {
                return state.StateName;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get secondaryStateId the state detail by particular state
        /// </summary>  
        /// <param name="SecondaryStateId" >state</param> 
        public async Task<string> GetStateNameBySecondaryStateId(int? SecondaryStateId)
        {
            var state = await _dbContext.state.FirstOrDefaultAsync(x => x.StateId == SecondaryStateId);
            if (state != null)
            {
                return state.StateName;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get bloodGroupId the bloodgroup detail by particular bloodGroupId
        /// </summary>  
        /// <param name="bloodGroupId" >bloodgroup</param> 
        public async Task<string> GetBloodGroupNameById(int bloodGroupId)
        {
            var bloodGroup = await _dbContext.BloodGroup.FirstOrDefaultAsync(x => x.BloodGroupId == bloodGroupId);
            if (bloodGroup != null)
            {
                return bloodGroup.BloodGroupName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Logic to get filteremployeelog the employeeslogreportdatamodel detail 
        /// </summary>        
        /// <param name="proc,values" >employeeslogreportdatamodel</param>      
        public async Task<List<EmployeesLogReportDataModel>> GetAllEmployessByEmployeeLogFilter(string proc, List<KeyValuePair<string, string>> values)
        {
            try
            {
                var parameters = new object[values.Count];
                for (int i = 0; i < values.Count; i++)
                    parameters[i] = new NpgsqlParameter(values[i].Key, values[i].Value);

                var paramnames = values.Aggregate("", (current, item) => current + item.Key + ",");
                paramnames = paramnames.TrimEnd(',');
                proc = proc + " " + paramnames;

                var leaveReportDateModel = await _dbContext.EmployeesLogReportDataModel.FromSqlRaw<EmployeesLogReportDataModel>(proc, parameters).AsNoTracking().ToListAsync();
                return leaveReportDateModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EmployeeLog>> GetAllEmployessLogFirst(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {
            try
            {
                var employeeIdValue = employeeChangeLogViewModel.EmpId;
                var companyid = Convert.ToString(companyId);
                var lastDate = DateTime.Today.AddDays(-2).Date;
                var today = DateTime.Now.Date;
                employeeChangeLogViewModel.StartDate = lastDate.Date.ToString(Constant.DateFormat);
                employeeChangeLogViewModel.EndDate = today.Date.ToString(Constant.DateFormat);
                var moduleName = "";
                var dFrom = string.IsNullOrEmpty(employeeChangeLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.StartDate).ToString(Constant.DateFormatMDY);
                var dTo = string.IsNullOrEmpty(employeeChangeLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.EndDate).ToString(Constant.DateFormatMDY);

                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                var _params = new
                {
                    OffsetValue = pager.sEcho,
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = employeeChangeLogViewModel.ColumnName + " " + employeeChangeLogViewModel.ColumnDirection
                };
                var empId = 0;
                var initialEmployeesLog = await EmpLogSpParameters(empId, moduleName, dFrom, dTo, companyId, pager, _params.Sorting);

                return initialEmployeesLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EmployeeLog>> GetAllEmployessByFilter(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {
            try
            {
                var employeeIdValue = employeeChangeLogViewModel.EmpId;
                var companyid = Convert.ToString(companyId);
                var moduleName = "";
                if (employeeChangeLogViewModel.EmployeeModules == Common.Constant.AllModules)
                {
                    moduleName = "";
                }
                else
                {
                    moduleName = employeeChangeLogViewModel.EmployeeModules;
                }
                var dFrom = string.IsNullOrEmpty(employeeChangeLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.StartDate).ToString(Constant.DateFormatYMDHyphen);
                var dTo = string.IsNullOrEmpty(employeeChangeLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.EndDate).ToString(Constant.DateFormatYMDHyphen);
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
                var sort = employeeChangeLogViewModel.ColumnName + " " + employeeChangeLogViewModel.ColumnDirection;

                if (employeeIdValue == 0 && pager.iDisplayLength != 0)
                {
                    var empId = 0;

                    var filteredEmpLog = await EmpLogSpParameters(empId, moduleName, dFrom, dTo, companyId, pager, sort);

                    return filteredEmpLog;
                }
                else if (employeeIdValue == 0 && pager.iDisplayLength == 0)
                {
                    var empId = employeeIdValue;
                    var filteredEmpLog = await EmpLogSpParameters(empId, moduleName, dFrom, dTo, companyId, pager, sort);

                    return filteredEmpLog;
                }
                else
                {
                    var empId = employeeIdValue;
                    var filteredEmpLog = await EmpLogSpParameters(empId, moduleName, dFrom, dTo, companyId, pager, sort);

                    return filteredEmpLog;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
         /// Logic to get employees attendance by log date 
        /// </summary>        
        /// <param name="logDate" ></param>      
        public async Task<List<AttendanceEntitys>> GetAllEmployeeAttendaceByDate(DateTime logDate)
        {
            var logdate = logDate.ToString("yyyy-MM-dd");
            var attendaceEntitys = await _dbContext.AttendanceEntity.Where(x => x.LogDate == logdate).ToListAsync();
            return attendaceEntitys;
        }

        ////// Employee log activity

        /// <summary>
        /// Logic to insert and update employees Activities
        /// </summary>        
        /// <param name="CompanyId" ></param>
        public async Task<bool> CreateEmployeeLogActivity(EmployeeActivityLogEntity employeeLogActivityEntity, int companyId)
        {

            employeeLogActivityEntity.CompanyId = companyId;
            if (employeeLogActivityEntity.Id == 0)
            {
                await _dbContext.EmployeeActivityLogEntitys.AddAsync(employeeLogActivityEntity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _dbContext.EmployeeActivityLogEntitys.Update(employeeLogActivityEntity);
                _dbContext.SaveChanges();
            }
            return true;

        }


        /// <summary>
        /// Logic to get employees login details using empId
        /// </summary>        
        /// <param name="empId" ></param>      
        public async Task<EmployeeActivityLogEntity> GetEmployeeLoginDetails(int empId,int companyId)
        {
            var employeeActivityLogEntity = await _dbContext.EmployeeActivityLogEntitys.AsTracking().FirstOrDefaultAsync(e => e.EmployeeId == empId && e.CompanyId == companyId);
            return employeeActivityLogEntity;
        }         
       
        /// <summary>
        /// Logic to get employeeslog 
        /// </summary>        
        /// <param name="CompanyId" >employees</param>
        public async Task<List<EmployeesLogEntity>> GetAllEmployeesLog(int companyId)
        {
            return await _dbContext.EmployeesLog.Where(x => x.CompanyId == companyId).ToListAsync();
        }

        /// <summary>
        /// Logic to get designationId the designation detail by particular designation
        /// </summary>  
        /// <param name="designationId" >designation</param>		
        public async Task<string> GetDesignationNameByDesignationId(int designationId)
        {
            var designation = await _dbContext.Designations.FirstOrDefaultAsync(x => x.DesignationId == designationId);
            if (designation != null)
            {
                return designation.DesignationName;
            }
            return string.Empty;
        }


        /// <summary>
        /// Logic to get employees
        /// </summary>        		
        public async Task<List<EmployeesEntity>> GetAllEmployeesByBackground()
        {
            return await _dbContext.Employees.Where(x => !x.IsDeleted).ToListAsync();
        }

        /// <summary>
        /// Logic to get profile the empId detail by particular empId
        /// </summary>   
        /// <param name="empId" >profile</param>
        public async Task<ProfileInfoEntity> GetProfileByEmpId(int empId)
        {
            var profile = _dbContext.ProfileInfo.AsNoTracking().FirstOrDefault(x => x.EmpId == empId);
            return profile;
        }

        /// <summary>
        /// Logic to get departmentId the department detail by particular department
        /// </summary>  
        /// <param name="departmentId" >department</param>
        public async Task<string> GetDepartmentByDepartmentName(int departmentId)
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(x => x.DepartmentId == departmentId);
            if (department != null)
            {
                return department.DepartmentName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Logic to get Employees the companyId detail by particular companyId
        /// </summary>   
        /// <param name="companyId" >Employees</param>
        public async Task<List<EmployeesEntity>> GetEmployeesByCompnayId(int companyId)
        {
            var company = await _dbContext.Employees.Where(x => x.CompanyId == companyId && !x.IsDeleted).ToListAsync();
            return company;
        }

        /// <summary>
        /// Logic to get employees IsProbationary
        /// </summary>    
        public async Task<List<EmployeesEntity>> GetEmployeesByIsProbationary()
        {
            var probation = await _dbContext.Employees.Where(x => !x.IsDeleted && !x.IsProbationary).ToListAsync();
            return probation;
        }

        /// <summary>
        /// Logic to get empId the reportingpersonsentities detail 
        /// </summary>   
        /// <param name="empId" >reportingpersonsentities</param>		  
        public async Task<List<ReportingPersonsEntity>> GetAllReportingPersonsEmpId(int empId)
        {
            return await _dbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == empId).ToListAsync();
        }

        /// <summary>
        /// Logic to get mailSchedulerEntity detail 
        /// </summary>           
        public async Task<List<MailSchedulerEntity>> GetMailSchedulerEntity(int companyId)
        {
            return await _dbContext.MailSchedulerEntity.AsNoTracking().Where(x => x.MailTime <= DateTime.Now && !x.IsDeleted && x.IsActive && x.CompanyId == companyId).ToListAsync();
        }

        /// <summary>
        /// Logic to get Employees the companyId detail by particular companyId
        /// </summary>   
        /// <param name="id" >Employees</param>
        public async Task<List<EmployeesEntity>> GetAllEmployeeByCompany(int id)
        {
            try
            {
                return await _dbContext.Employees.AsNoTracking().Where(x => !x.IsDeleted && x.CompanyId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        /// <summary>
        /// Logic to get mailSchedulerEntity by particular schedulerId
        /// </summary>   
        /// <param name="id" >MailSchedulerEntity</param>
        public async Task<MailSchedulerEntity> GetMailSchedulerbyId(int id)
        {
            return _dbContext.MailSchedulerEntity.Where(x => x.SchedulerId == id).FirstOrDefault();
        }

        /// <summary>
        /// Logic to get EmployeesEntity by particular DepartmentId
        /// </summary>   
        /// <param name="DepartmentId" >EmployeesEntity</param>
        public async Task<List<EmployeesEntity>> GetDepartmentById(List<int> DepartmentId,int companyId)
        {
            var employeesEntity = await _dbContext.Employees.Where(e => DepartmentId.Contains(e.DepartmentId) && e.CompanyId == companyId).ToListAsync();
            return employeesEntity;
        }       

        /// <summary>
        /// Logic to get EmployeesEntity by particular DesignationId
        /// </summary>   
        /// <param name="DesignationId" >EmployeesEntity</param>
        public async Task<List<EmployeesEntity>> GetDesignationById( List<int> DesignationId,int companyId)
        {
            var employeesEntity = await _dbContext.Employees.Where(e => DesignationId.Contains(e.DesignationId) && e.CompanyId == companyId).ToListAsync ();
            return employeesEntity;
        }

        // dashboard

        /// <summary>
        /// Logic to get the Employee WorkAnniversary by particular companyId
        /// </summary>   
        /// <param name="companyId" ></param>
        public async Task<List<EmployeeWorkAnniversary>> EmployeesWorkAnniversary(int companyId)
        {
            var employeeWorkAnniversary = await (from emp in _dbContext.Employees
                                           join pro in _dbContext.ProfileInfo on emp.EmpId equals pro.EmpId
                                           join deg in _dbContext.Designations on emp.DesignationId equals deg.DesignationId
                                           join dep in _dbContext.Departments on emp.DepartmentId equals dep.DepartmentId
                                           join rep in _dbContext.ReportingPersonsEntities on emp.EmpId equals rep.EmployeeId
                                           join cmy in _dbContext.Company on emp.CompanyId equals cmy.CompanyId
                                           join mail in _dbContext.EmailSettings on cmy.CompanyId equals mail.CompanyId
                                           where emp.CompanyId == companyId && cmy.IsDeleted == false && emp.IsDeleted == false && pro.IsDeleted == false &&
                                           mail.IsDeleted == false
                                           select new EmployeeWorkAnniversary()
                                           {
                                               EmpId = emp.EmpId,
                                               FirstName = emp.FirstName,
                                               LastName = emp.LastName,
                                               UserName = emp.UserName,
                                               DateOfJoining = pro.DateOfJoining,
                                               Gender = pro.Gender,
                                               DesignationName = deg.DesignationName,
                                               DepartmentName = dep.DepartmentName,
                                               OfficeEmail = emp.OfficeEmail,
                                               Employees = _dbContext.Employees.ToList(),
                                               ReportingPersonEmplyeeIds = _dbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == emp.EmpId).Select(x => x.ReportingPersonEmpId).ToList(),
                                               FromEmail = mail.FromEmail,
                                           }).ToListAsync();

            return employeeWorkAnniversary;
        }

        /// <summary>
        /// Logic to get the Employee BirthdayCelebration by particular companyId
        /// </summary>   
        /// <param name="companyId" ></param>
        public async Task<List<EmployeeBirthday>> EmployeeBirthdayCelebration(int companyId)
        {
            var employeeBirthdayCelebrations = await (from emp in _dbContext.Employees
                                                join pro in _dbContext.ProfileInfo on emp.EmpId equals pro.EmpId
                                                join deg in _dbContext.Designations on emp.DesignationId equals deg.DesignationId
                                                join dep in _dbContext.Departments on emp.DepartmentId equals dep.DepartmentId
                                                join rep in _dbContext.ReportingPersonsEntities on emp.EmpId equals rep.EmployeeId
                                                join cmy in _dbContext.Company on emp.CompanyId equals cmy.CompanyId
                                                join mail in _dbContext.EmailSettings on cmy.CompanyId equals mail.CompanyId
                                                where emp.CompanyId == companyId && cmy.IsDeleted == false && emp.IsDeleted == false && pro.IsDeleted == false &&
                                                mail.IsDeleted == false
                                                select new EmployeeBirthday()
                                                {
                                                    EmpId = emp.EmpId,
                                                    FirstName = emp.FirstName,
                                                    LastName = emp.LastName,
                                                    UserName = emp.UserName,
                                                    DateOfBirth = pro.DateOfBirth,
                                                    Gender = pro.Gender,
                                                    DesignationName = deg.DesignationName,
                                                    DepartmentName = dep.DepartmentName,
                                                    OfficeEmail = emp.OfficeEmail,
                                                    Employees = _dbContext.Employees.ToList(),
                                                    ReportingPersonEmplyeeIds = _dbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == emp.EmpId).Select(x => x.ReportingPersonEmpId).ToList(),
                                                    FromEmail = mail.FromEmail,
                                                }).ToListAsync();

            return employeeBirthdayCelebrations;
        }

        /// <summary>
        /// Logic to get the Employee ProbationCelebration by particular companyId
        /// </summary>   
        /// <param name="companyId" ></param>
        public async Task<List<EmployeeProbation>> EmployeeProbationCelebration(int companyId)
        {
            var employeeProbationCelebrations = await (from emp in _dbContext.Employees
                                                 join pro in _dbContext.ProfileInfo on emp.EmpId equals pro.EmpId
                                                 join deg in _dbContext.Designations on emp.DesignationId equals deg.DesignationId
                                                 join dep in _dbContext.Departments on emp.DepartmentId equals dep.DepartmentId
                                                 join rep in _dbContext.ReportingPersonsEntities on emp.EmpId equals rep.EmployeeId
                                                 join cmy in _dbContext.Company on emp.CompanyId equals cmy.CompanyId
                                                 join mail in _dbContext.EmailSettings on cmy.CompanyId equals mail.CompanyId
                                                 join empStg in _dbContext.EmployeeSettingsEntity on cmy.CompanyId equals empStg.CompanyId
                                                 where emp.CompanyId == companyId && cmy.IsDeleted == false && emp.IsProbationary == false && emp.IsDeleted == false && pro.IsDeleted == false &&
                                                 mail.IsDeleted == false
                                                 select new EmployeeProbation()
                                                 {
                                                     EmpId = emp.EmpId,
                                                     FirstName = emp.FirstName,
                                                     LastName = emp.LastName,
                                                     UserName = emp.UserName,
                                                     DateOfJoining = pro.DateOfJoining,
                                                     Gender = pro.Gender,
                                                     DesignationName = deg.DesignationName,
                                                     DepartmentName = dep.DepartmentName,
                                                     OfficeEmail = emp.OfficeEmail,
                                                     Employees = _dbContext.Employees.ToList(),
                                                     ReportingPersonEmplyeeIds = _dbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == emp.EmpId).Select(x => x.ReportingPersonEmpId).ToList(),
                                                     FromEmail = mail.FromEmail,
                                                     ProbationMonths = empStg.ProbationMonths,
                                                 }).ToListAsync();

            return employeeProbationCelebrations;
        }

        /// <summary>
        /// Logic to get the EmployeeS TopFiveCelebration details list 
        /// </summary>          
        public async Task <List<Celebration>> GetTopFiveCelebration(int companyId)
        {            
            var currentDate = DateTime.Now;
            var celebrations = await (from emp in _dbContext.Employees
                                join pro in _dbContext.ProfileInfo on emp.EmpId equals pro.EmpId
                                where pro.DateOfBirth.Month >= currentDate.Month && pro.DateOfBirth.DayOfYear >= currentDate.DayOfYear && !pro.IsDeleted && !emp.IsDeleted   
                                && emp.CompanyId == companyId
                                      orderby pro.DateOfBirth.Month , pro.DateOfBirth.DayOfYear                            
                                select new Celebration()
                                    {
                                        CelebrationName = Common.Constant.Birthday,
                                        EmployeeName = emp.FirstName + " " +emp.LastName,
                                        CelebrationDate = Convert.ToDateTime(pro.DateOfBirth).ToString("dd-MMM"),
                                        EmployeeStatus = emp.IsDeleted,
                                        EmpId = emp.EmpId,                                                                          
                                }).Take(5).ToListAsync();                                    
            return celebrations;
        }

        /// <summary>
        /// Logic to get the EmployeesTopFive details list 
        /// </summary>  
        public async Task<List<EmployeesList>> GetEmployeesTopFive(int companyId)
        {            
            var currentDate = DateTime.Now;
            var employees = await (from emp in _dbContext.Employees
                             join des in _dbContext.Designations on emp.DesignationId equals des.DesignationId
                             join dep in _dbContext.Departments on emp.DepartmentId equals dep.DepartmentId
                             join pro in _dbContext.ProfileInfo on emp.EmpId equals pro.EmpId
                             where !pro.IsDeleted && emp.CompanyId == companyId
                             orderby emp.CreatedDate descending 
                             select new EmployeesList()
                             {
                                 UserName = emp.UserName,
                                 EmpId = emp.EmpId,
                                 FirstName = emp.FirstName,
                                 LastName = emp.LastName,
                                 EmployeeFullName = emp.FirstName + " " + emp.LastName,
                                 DepartmentId = emp.DepartmentId,
                                 DesignationId = emp.DesignationId,
                                 RoleId = (Role)emp.RoleId,
                                 OfficeEmail = emp.OfficeEmail,
                                 CreatedDate = emp.CreatedDate,
                                 EmployeeSortName = Common.Common.GetEmployeeSortName(emp.FirstName, emp.LastName),
                                 DesignationName = des.DesignationName ,
                                 DepartmentName = dep.DepartmentName,
                                 EmployeeStatus = emp.IsDeleted,
                                 JoingDate = pro.DateOfJoining,
                                 EmployeeProfileImage = pro.ProfileName,
                           }).Take(5).ToListAsync();
            return employees;
        }

        /// <summary>
        /// Logic to get the EmailQueueSendMail details by particular companyId
        /// </summary>  
        /// <param name="companyId" ></param>
        public async Task<List<BackgroundEmailQueueModel>> EmailQueueSendMail(int companyId)
        {
            var emails = await (from eq in _dbContext.EmailQueueEntitys
                          join cmy in _dbContext.Company on eq.CompanyId equals cmy.CompanyId
                          join ems in _dbContext.EmailSettings on cmy.CompanyId equals ems.CompanyId
                          where cmy.CompanyId == companyId && cmy.IsDeleted == false && eq.IsSend == false
                          select new BackgroundEmailQueueModel()
                          {
                              FromEmail = ems.FromEmail,
                              Host = ems.Host,
                              EmailPort = ems.EmailPort,
                              DisplayName = ems.DisplayName,
                              Password = ems.Password,
                              UserName = ems.UserName,
                              EmailQueueID = eq.EmailQueueID,
                              EmailQueueFromEmail = eq.FromEmail,
                              ToEmail = eq.ToEmail,
                              Subject = eq.Subject,
                              Body = eq.Body,
                              IsSend = eq.IsSend,
                              Reason = eq.Reason,
                              EmailQueueDisplayName = eq.DisplayName,
                              Attachments = eq.Attachments,
                              CCEmail = eq.CCEmail,
                          }).ToListAsync();

            return emails;
        }

        /// <summary>
        /// Logic to get the all employees details list
        /// </summary> 
        public async Task<List<Employees>> GetAllEmployee(int companyId)
        {
            var allEmployees = await (from employees in _dbContext.Employees                                      
                                      join designation in _dbContext.Designations on employees.DesignationId equals designation.DesignationId
                                      join department in _dbContext.Departments on employees.DepartmentId equals department.DepartmentId
                                      join profile in _dbContext.ProfileInfo on employees.EmpId equals profile.EmpId
                                      into profileImage from profileDetails in profileImage.DefaultIfEmpty()
                                      join releaveTypes in _dbContext.RelievingReasonEntity on employees.RelieveId equals releaveTypes.RelievingReasonId
                                      into releave from releaveReason in releave.DefaultIfEmpty()                                     
                                      
                                      where employees.CompanyId == companyId
                                      select new Employees()
                                      {
                                          EmpId = employees.EmpId,
                                          UserName = employees.UserName,
                                          FatherName = employees.FatherName,
                                          FirstName = employees.FirstName, 
                                          LastName = employees.LastName,
                                          OfficeEmail = employees.OfficeEmail,
                                          PersonalEmail = employees.PersonalEmail,
                                          IsActive = employees.IsActive,
                                          IsDeleted = employees.IsDeleted,
                                          IsOnboarding = employees.IsOnboarding,
                                          IsVerified = employees.IsVerified,
                                          IsProbationary = employees.IsProbationary,
                                          RoleId = (Role)employees.RoleId,
                                          DepartmentId = employees.DepartmentId,
                                          DesignationId = employees.DesignationId,
                                          RejectReason = employees.RejectReason,
                                          RelieveId = employees.RelieveId,
                                          EsslId = employees.EsslId,
                                          CompanyId = employees.CompanyId,
                                          EmployeeProfileImage = profileDetails != null ? profileDetails.ProfileName : string.Empty,
                                          DateofJoin = profileDetails != null ? profileDetails.DateOfJoining.ToString(Constant.DateFormat) : string.Empty,
                                          EmployeeSortName = Common.Common.GetEmployeeSortName(employees.FirstName, employees.LastName),
                                          DesignationName = designation.DesignationName != null ? designation.DesignationName : string.Empty,
                                          DepartmentName = department.DepartmentName != null ? department.DepartmentName : string.Empty,
                                          ReleaveName = releaveReason.RelievingReasonName != null ? releaveReason.RelievingReasonName : string.Empty,
                                          AllAssetId = _dbContext.AllAssets.Where(a => a.EmployeeId == employees.EmpId  && companyId == a.CompanyId).Count(),
                                          BenefitId = _dbContext.EmployeeBenefitEntitys.Where(b => b.EmpId == employees.EmpId && companyId == b.CompanyId).Count(),
                                          RelievingReasonEntity = _dbContext.RelievingReasonEntity.Where(r =>!r.IsDeleted && r.CompanyId == companyId).ToList(),                                          
                                      }).ToListAsync();
            return allEmployees;

        }

        /// <summary>
        /// Logic to get employees Activities log list 
        /// </summary>               
        public async Task<List<EmployeeActivityLog>> GetAllEmployeeActivityLogs (int companyId)
        {
            var employeeActivityLogs = await (from activeLog in _dbContext.EmployeeActivityLogEntitys
                                        join employees in _dbContext.Employees on activeLog.EmployeeId equals employees.EmpId
                                        join company in _dbContext.Company on activeLog.CompanyId equals company.CompanyId
                                        where companyId == activeLog.CompanyId && companyId == employees.CompanyId && !company.IsDeleted
                                        select new EmployeeActivityLog ()
                                        {
                                            Id = activeLog.Id,
                                            CompanyId = activeLog.CompanyId,
                                            companyName = company.CompanyName,
                                            EmployeeId = activeLog.EmployeeId,
                                            EmployeeName = employees.UserName,
                                            LastLoginDate = activeLog.LastLoginDate,
                                            IPAddress = activeLog.IPAddress,
                                        }).ToListAsync ();
            return employeeActivityLogs;
        }

        // Salary Modules

        /// <summary>
        /// Logic to get list of salary detail by particular employees 
        /// </summary>
        /// <param name="empId" ></param> 
        public async Task<List<salarys>> GetSalaryByEmpId(int empId)
        {
            var salarys = await (from salary in _dbContext.salaryEntities
                                     where  salary.EmpId == empId && !salary.IsDeleted 
                                     select new salarys()
                                     {
                                         EmpId = salary.EmpId,
                                         Amount = salary.Amount,
                                         Month = salary.Month,
                                         MonthName =  Convert.ToString((Months)salary.Month),
                                         Year = salary.Year, 
                                         SalaryId = salary.SalaryId,
                                     }).ToListAsync();
            return salarys;
        }

        /// <summary>
        /// Logic to get create and update salary detail 
        /// </summary>
        /// <param name="salaryId" ></param> 
        public async Task<int> AddSalaryDetails(SalaryEntity salaryEntity)
        {
            var result = 0;
            if (salaryEntity?.SalaryId == 0)
            {
                await _dbContext.salaryEntities.AddAsync(salaryEntity);
                await _dbContext.SaveChangesAsync();
                result = salaryEntity.EmpId;
            }
            else
            {
                if (salaryEntity != null)
                {
                    _dbContext.salaryEntities.Update(salaryEntity);
                    await _dbContext.SaveChangesAsync();
                    result = salaryEntity.EmpId;
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get salary detail by particular salaryId 
        /// </summary>
        /// <param name="salaryId" ></param> 
        public async Task<SalaryEntity> GetBysalaryId (int salaryId)
        {
            var salaryEntity = await _dbContext.salaryEntities.AsNoTracking ().FirstOrDefaultAsync(x => x.SalaryId == salaryId && !x.IsDeleted);
            return salaryEntity;
        }

        /// <summary>
        /// Logic to get delete salary detail 
        /// </summary>
        /// <param name="salaryEntity" ></param>     
        public async Task DeleteSalary(SalaryEntity  salaryEntity)
        {
            _dbContext.salaryEntities.Update(salaryEntity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Logic to get count of salary detail by particular employees 
        /// </summary>
        /// <param name="empId,month,year" ></param> 
        public async Task<int> GetSalaryMonth(int month, int empId, int year)
        {
            var salaryYearCount = await _dbContext.salaryEntities.Where(x => x.Month == month && x.EmpId == empId && x.Year == year && !x.IsDeleted).CountAsync();
            return salaryYearCount;
        }


        /// <summary>
        /// Logic to get count of Employee Log for all employees 
        /// </summary>
        /// <param name="pager,employeeChangeLogViewModel,companyId" ></param> 


        public async Task<int> GetAllEmployessByEmployeeLogCount(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {
            try
            {
                var employeeIdValue = employeeChangeLogViewModel.EmpId;
                var companyid = Convert.ToString(companyId);
                var moduleName = "";
                if (employeeChangeLogViewModel.EmployeeModules == Common.Constant.AllModules)
                {
                    moduleName = "";
                }
                else
                {
                    moduleName = employeeChangeLogViewModel.EmployeeModules;
                }
                var dFrom = string.IsNullOrEmpty(employeeChangeLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.StartDate).ToString(Constant.DateFormatYMDHyphen);
                var dTo = string.IsNullOrEmpty(employeeChangeLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.EndDate).ToString(Constant.DateFormatYMDHyphen);

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };

                var param = new NpgsqlParameter("@empId", employeeIdValue);
                var param1 = new NpgsqlParameter("@EmployeeModules", moduleName);
                var param2 = new NpgsqlParameter("@startDate", dFrom);
                var param3 = new NpgsqlParameter("@endDate", dTo);
                var param4 = new NpgsqlParameter("@companyId", companyId);
                var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                List<EmployeesDataCount> employeeCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetEmployeeLogFilterCount] @empId,@employeeModules,@startDate, @endDate, @companyId,@searchText", param, param1, param2, param3, param4, param5).ToListAsync();
                foreach (var item in employeeCounts)
                {
                    var result = item.Id;
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
        /// Logic for sending parameters to get Employee Filter data in common method  
        /// </summary>
        /// <param name="empId,moduleName,dFrom, dTo, companyId, pager, sort" ></param> 

        public async Task<List<EmployeeLog>> EmpLogSpParameters(int empId, string moduleName, string dFrom, string dTo, int companyId, SysDataTablePager pager, string sort)
        {
            var param = new NpgsqlParameter("@empId", empId);
            var param1 = new NpgsqlParameter("@EmployeeModules", moduleName);
            var param2 = new NpgsqlParameter("@startDate", dFrom);
            var param3 = new NpgsqlParameter("@endDate", dTo);
            var param4 = new NpgsqlParameter("@companyId", companyId);
            var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
            var param6 = new NpgsqlParameter("@offsetValue", pager.sEcho);
            var param7 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
            var param8 = new NpgsqlParameter("@sorting", sort);

            var getEmpLog = await _dbContext.EmployeesLogReport.FromSqlRaw("EXEC [dbo].[spGetEmployeeLogFilterData] @empId,@employeeModules,@startDate, @endDate, @companyId, @searchText,@offsetValue,@pagingSize,@sorting", param, param1, param2, param3, param4, param5, param6, param7, param8).ToListAsync();
            return getEmpLog;
        }

        /// <summary>
        /// Logic to get of Employee Activity Log for all employees 
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param> 

        public async Task<List<EmployeeActivityLogViewModel>> GetEmployeeActivitylogs(SysDataTablePager pager,string columnName, string columnDirection,int companyId)
        {
            try
            {
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
                    OffsetValue = pager.sEcho,
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param3 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);
                var data = await _dbContext.employeeActivityLogViewModels.FromSqlRaw("EXEC [dbo].[spGetEmployeesActivityFilterList] @companyId,@pagingSize,@offsetValue,@searchText ,@sorting",  param,param2, param3, param4,param5).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Logic to get count of Employee Activity Log for all employees 
        /// </summary>
        /// <param name="pager" ></param> 

        public async Task<int> GetAllEmployeeActivityLogfilterCount(SysDataTablePager pager,int companyId)
        {
            try
            {
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
                List<EmployeeActivityLogFilterCount> employeeActivityCounts = await _dbContext.employeeActivityFilterCounts.FromSqlRaw("EXEC [dbo].[spGetEmployeesActivityFilterListCount]  @companyId,@searchText", param1, param2).ToListAsync();
                foreach (var item in employeeActivityCounts)
                {
                    var result = item.Id;
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
        /// Logic to get count of Employee  
        /// </summary>
        /// <param name="pager" ></param> 
        public async Task<int> GetEmployeesListCount(SysDataTablePager pager,int companyId)
        {
     
            if (pager.sSearch == null)
            {
                pager.sSearch = "";
            }
            var _params = new
            {               
                SearchText = pager.sSearch
            };
            var paramcompany = new NpgsqlParameter("@companyId", companyId);
            var param = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            List<EmployeesDataCount> employeeCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetEmployeesFilterListCount]  @companyId,@searchText", paramcompany, param).ToListAsync();
            foreach (var item in employeeCounts)
            {
                var result = item.Id;
                return result;
            }
            return 0;
        }

        /// <summary>
        /// Logic to get the Employee list details 
        /// </summary>
        /// <param name="pager,columnDirection,ColumnName" ></param> 
        public async Task<List<EmployeesDetailsDataModel>> GetEmployeeDetailsList(SysDataTablePager pager, string columnDirection, string ColumnName,int companyId)
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
                    Sorting = ColumnName + " " + columnDirection,
                };
                var paramcompany = new NpgsqlParameter("@companyId", companyId);
                var param1 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param2 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param4 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
                var employeeList = await _dbContext.EmployeesDetailsDataModels.FromSqlRaw("EXEC [dbo].[spGetEmployeesFilterList] @companyId,@pagingSize ,@offsetValue,@searchText,@sorting", paramcompany, param1, param2, param3, param4).ToListAsync();
                return employeeList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        
        }
    }
}

   



