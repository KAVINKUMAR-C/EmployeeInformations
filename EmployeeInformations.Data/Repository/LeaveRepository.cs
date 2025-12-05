using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace EmployeeInformations.Data.Repository
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly EmployeesDbContext _dbContext;
        public LeaveRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Employee Applied Leave

        /// <summary>
          /// Logic to get leavesummary list basd on empId
         /// </summary>   
        /// <param name="empId" >employeeappliedleaveentities</param>
        /// <param name="IsDeleted" >employeeappliedleaveentities</param>
        /// <param name="CompanyId" >employeeappliedleaveentities</param>
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllLeaveSummary(int empId, int companyId)
        {
            var result = new List<EmployeeAppliedLeaveEntity>();
            if (empId == 0)
            {
                result = await _dbContext.employeeAppliedLeaveEntities.Where(x => !x.IsDeleted && x.AppliedLeaveTypeId != 7 && x.CompanyId == companyId).ToListAsync();
            }
            else
            {
                result = await _dbContext.employeeAppliedLeaveEntities.Where(x => x.EmpId == empId && !x.IsDeleted && x.CompanyId == companyId && x.AppliedLeaveTypeId != 7).ToListAsync();
            }
            return result;
        }


        /// <summary>
        /// Logic to get work from home summary, based on empId
        /// </summary>   
        /// <param name="empId" >employeeappliedleaveentities</param>
        /// <param name="IsDeleted" >employeeappliedleaveentities</param>
        /// <param name="CompanyId" >employeeappliedleaveentities</param>
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllWorkFromHomeSummary(int empId, int companyId)
        {
            var result = new List<EmployeeAppliedLeaveEntity>();
            if (empId == 0)
            {
                result = await _dbContext.employeeAppliedLeaveEntities.Where(x => !x.IsDeleted && x.CompanyId == companyId && x.AppliedLeaveTypeId == 7).ToListAsync();
            }
            else
            {
                result = await _dbContext.employeeAppliedLeaveEntities.Where(x => x.EmpId == empId && !x.IsDeleted && x.CompanyId == companyId && x.AppliedLeaveTypeId == 7).ToListAsync();
            }
            return result;
        }


        /// <summary>
        /// Logic to get compensatoryoff  summary, based on empId
        /// </summary>   
        /// <param name="empId" >employeeappliedleaveentities</param>
        /// <param name="IsDeleted" >employeeappliedleaveentities</param>
        /// <param name="CompanyId" >employeeappliedleaveentities</param>
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllCompensatoryOffSummary(int empId, int companyId)
        {
            var result = new List<EmployeeAppliedLeaveEntity>();
            if (empId == 0)
            {
                result = await _dbContext.employeeAppliedLeaveEntities.Where(x => !x.IsDeleted && x.CompanyId == companyId && x.AppliedLeaveTypeId == 8).ToListAsync();
            }
            else
            {
                result = await _dbContext.employeeAppliedLeaveEntities.Where(x => x.EmpId == empId && !x.IsDeleted && x.CompanyId == companyId && x.AppliedLeaveTypeId == 8).ToListAsync();
            }
            return result;
        }


        /// <summary>
        /// Logic to get leavedashboard list 
        /// </summary>           
        /// <param name="IsDeleted" >employeeappliedleaveentities</param>
        /// <param name="CompanyId" >employeeappliedleaveentities</param>
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllLeaveDashboard(int companyId)
        {
            return await _dbContext.employeeAppliedLeaveEntities.Where(x => !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get create leave the employeeappliedleaveentities detail
        /// </summary>   
        /// <param name="employeeAppliedLeaveEntity" ></param>      
        public async Task<bool> CreateLeave(EmployeeAppliedLeaveEntity employeeAppliedLeaveEntity, int companyId)
        {
            var result = false;
            if (employeeAppliedLeaveEntity != null)
            {
                employeeAppliedLeaveEntity.CompanyId = companyId;
                await _dbContext.employeeAppliedLeaveEntities.AddAsync(employeeAppliedLeaveEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update leave the employeeappliedleaveentities detail by particular employeeappliedleaveentities
        /// </summary>   
        /// <param name="employeeAppliedLeaveEntity" ></param>
        public async Task<bool> UpdateLeave(EmployeeAppliedLeaveEntity employeeAppliedLeaveEntity)
        {
            var result = false;
            if (employeeAppliedLeaveEntity != null)
            {
                _dbContext.employeeAppliedLeaveEntities.Update(employeeAppliedLeaveEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete leave the employeeappliedleaveentities detail by particular employeeappliedleaveentities
        /// </summary>   
        /// <param name="leave,sessionEmployeeId" >employeeappliedleaveentities</param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeleteLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var employeeAppliedLeaveEntities = await _dbContext.employeeAppliedLeaveEntities.Where(e => e.AppliedLeaveId == leave.AppliedLeaveId && !e.IsDeleted && e.CompanyId == companyId).FirstOrDefaultAsync();
            if (employeeAppliedLeaveEntities != null)
            {
                employeeAppliedLeaveEntities.IsDeleted = true;
                _dbContext.employeeAppliedLeaveEntities.Update(employeeAppliedLeaveEntities);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get empId the employeeappliedleaveentities detail by particular employeeappliedleaveentities
        /// </summary>   
        /// <param name="empId" >employeeappliedleaveentities</param>
        /// <param name="IsDeleted" >employeeappliedleaveentities</param>
        /// <param name="CompanyId" >employeeappliedleaveentities</param>
        public async Task<List<EmployeeAppliedLeaveEntity>> GetLeaveByAppliedLeaveEmpId(int empId, int companyId)
        {
            var results = new List<EmployeeAppliedLeaveEntity>();
            var result = new EmployeeAppliedLeaveEntity();
            
            if (empId > 0)
            {
                results = await _dbContext.employeeAppliedLeaveEntities.Where(x => x.EmpId == empId && !x.IsDeleted && x.CompanyId == companyId && x.CreatedDate.Year == DateTime.Now.Year).ToListAsync();
            }
            return results;
        }


        /// <summary>
        /// Logic to get delete leave the employeeappliedleaveentities detail by particular employeeappliedleaveentities
        /// </summary>   
        /// <param name="leave,sessionEmployeeId" >employeeappliedleaveentities</param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> ApprovedLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var employeeAppliedLeaveEntities = await _dbContext.employeeAppliedLeaveEntities.Where(e => e.AppliedLeaveId == leave.AppliedLeaveId && !e.IsDeleted && e.CompanyId == companyId).FirstOrDefaultAsync();
            if (employeeAppliedLeaveEntities != null)
            {
                employeeAppliedLeaveEntities.IsApproved = 1;
                employeeAppliedLeaveEntities.RejectReason = leave.RejectReason;
                employeeAppliedLeaveEntities.UpdatedDate = DateTime.Now;
                employeeAppliedLeaveEntities.UpdatedBy = sessionEmployeeId;
                _dbContext.employeeAppliedLeaveEntities.Update(employeeAppliedLeaveEntities);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get reject leave the employeeappliedleaveentities detail by particular employeeappliedleaveentities
        /// </summary>   
        /// <param name="leave,sessionEmployeeId" >employeeappliedleaveentities</param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> RejectLeave(EmployeeAppliedLeave leave, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            var employeeAppliedLeaveEntities = await _dbContext.employeeAppliedLeaveEntities.Where(e => e.AppliedLeaveId == leave.AppliedLeaveId && !e.IsDeleted && e.CompanyId == companyId).FirstOrDefaultAsync();
            if (employeeAppliedLeaveEntities != null)
            {
                employeeAppliedLeaveEntities.IsApproved = 2;
                employeeAppliedLeaveEntities.RejectReason = leave.RejectReason;
                employeeAppliedLeaveEntities.UpdatedDate = DateTime.Now;
                employeeAppliedLeaveEntities.UpdatedBy = sessionEmployeeId;
                _dbContext.employeeAppliedLeaveEntities.Update(employeeAppliedLeaveEntities);
                await _dbContext.SaveChangesAsync();
                result = 1;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update leave the employeeappliedleaveentities detail by particular employeeappliedleaveentities
        /// </summary>   
        /// <param name="id" >employeeappliedleaveentities</param>       
        /// <param name="CompanyId" >employeeappliedleaveentities</param>GetCompensatoryOffRequestByCompensatoryId
        public async Task<EmployeeAppliedLeaveEntity> GetLeaveByAppliedLeaveId(int id, int companyId)
        {
            var result = new EmployeeAppliedLeaveEntity();
            if (id > 0)
            {
                result = await _dbContext.employeeAppliedLeaveEntities.FirstOrDefaultAsync(x => x.AppliedLeaveId == id && x.CompanyId == companyId);
            }

            return result ?? new EmployeeAppliedLeaveEntity();
        }


        /// <summary>
        /// Logic to get empIds the employeeappliedleaveentities detail by particular employeeappliedleaveentities
        /// </summary>   
        /// <param name="empIds" >employeeappliedleaveentities</param>       
        /// <param name="CompanyId" >employeeappliedleaveentities</param> 
        /// <param name="IsDeleted" >employeeappliedleaveentities</param> 
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllLeaveSummarys(List<int> empIds, int companyId)
        {
            var reportingPersonsEntity = await _dbContext.employeeAppliedLeaveEntities.Where(e => empIds.Contains(e.EmpId) && !e.IsDeleted && e.CompanyId == companyId && e.AppliedLeaveTypeId != 7).ToListAsync();
            return reportingPersonsEntity;
		}


        /// <summary>
        /// Logic to get all employees applied leaves
        /// </summary>   
        /// <param name="empIds" >employeeappliedleaveentities</param>       
        /// <param name="CompanyId" >employeeappliedleaveentities</param> 
        /// <param name="IsDeleted" >employeeappliedleaveentities</param> 
        public async Task<List<EmployeeAppliedLeaveEntity>> GetEmployeeIdsLeave(int empId, int companyId)
        {
            return await _dbContext.employeeAppliedLeaveEntities.Where(x => x.EmpId == empId && x.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get all employees work from home summary
        /// </summary>   
        /// <param name="empIds" >employeeappliedleaveentities</param>       
        /// <param name="CompanyId" >employeeappliedleaveentities</param> 
        /// <param name="IsDeleted" >employeeappliedleaveentities</param> 
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllWorkFromHomeSummarys(List<int> empIds, int companyId)
        {
            List<EmployeeAppliedLeaveEntity> result = new List<EmployeeAppliedLeaveEntity>();
            var reportingPersonsEntity = await _dbContext.employeeAppliedLeaveEntities.Where(e => empIds.Contains(e.EmpId) && !e.IsDeleted && e.CompanyId == companyId && e.AppliedLeaveTypeId == 7).ToListAsync();
            return reportingPersonsEntity;

        }


        /// <summary>
        /// Logic to get all employees compensatoryoff summary
        /// </summary>   
        /// <param name="empIds" >employeeappliedleaveentities</param>       
        /// <param name="CompanyId" >employeeappliedleaveentities</param> 
        /// <param name="IsDeleted" >employeeappliedleaveentities</param> 
        public async Task<List<EmployeeAppliedLeaveEntity>> GetAllCompensatoryOffSummarys(List<int> empIds, int companyId)
        {
            List<EmployeeAppliedLeaveEntity> result = new List<EmployeeAppliedLeaveEntity>();
            var reportingPersonsEntity = await _dbContext.employeeAppliedLeaveEntities.Where(e => empIds.Contains(e.EmpId) && !e.IsDeleted && e.CompanyId == companyId && e.AppliedLeaveTypeId == 7).ToListAsync();
            return reportingPersonsEntity;

        }


        /// <summary>
         /// Logic to get empId the leavetype detail by particular leavetype
         /// </summary>   
        /// <param name="empId" >allleavedetails</param>
        /// <param name="LeaveYear" >allleavedetails</param>
        /// <param name="CompanyId" >allleavedetails</param>
        public async Task<AllLeaveDetailsEntity> GetAllLeaveDetails(int empId, int companyId)
        {
            var allLeaveDetailsEntity = new AllLeaveDetailsEntity();
            if (empId == 0)
            {
                allLeaveDetailsEntity = await _dbContext.AllLeaveDetails.Where(x => x.EmpId == empId && x.CompanyId == companyId && x.LeaveYear == DateTime.Now.Year).AsNoTracking().FirstOrDefaultAsync();
            }
            else
            {
                allLeaveDetailsEntity = await _dbContext.AllLeaveDetails.Where(x => x.EmpId == empId && x.CompanyId == companyId && x.LeaveYear == DateTime.Now.Year).AsNoTracking().FirstOrDefaultAsync();
            }
            return allLeaveDetailsEntity;
        }

        /// <summary>
        /// Logic to get empId the allLeaveDetails detail by particular empId and companyId 
        /// </summary>   
        /// <param name="empId" >allleavedetails</param>
        /// <param name="LeaveYear" >allleavedetails</param>
        /// <param name="CompanyId" >allleavedetails</param>
        public async Task<AllLeaveDetailsEntity> GetLeaveDetailsByEmpIdAndCompanyid(int empId, int companyId)
        {
            var allLeaveDetailsEntity = new AllLeaveDetailsEntity();
            allLeaveDetailsEntity = await _dbContext.AllLeaveDetails.Where(x => x.EmpId == empId && x.CompanyId == companyId && x.LeaveYear == DateTime.Now.Year).AsNoTracking().FirstOrDefaultAsync();
            return allLeaveDetailsEntity;
        }

        /// <summary>
        /// Logic to get create the allleavedetails  detail
        /// </summary>   
        /// <param name="allLeaveDetailsEntity" ></param> 
        public async Task<bool> InsertAllLeaveDetailsByEmpId(AllLeaveDetailsEntity allLeaveDetailsEntity)
        {
            try
            {
                var result = false;
                if (allLeaveDetailsEntity != null && allLeaveDetailsEntity.AllLeaveDetailId == 0)
                {
                    allLeaveDetailsEntity.CompanyId = allLeaveDetailsEntity.CompanyId == 0 ? allLeaveDetailsEntity.CompanyId : allLeaveDetailsEntity.CompanyId;
                    await _dbContext.AllLeaveDetails.AddAsync(allLeaveDetailsEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                else
                {
                    allLeaveDetailsEntity.CompanyId = allLeaveDetailsEntity.CompanyId == 0 ? allLeaveDetailsEntity.CompanyId : allLeaveDetailsEntity.CompanyId;
                    _dbContext.AllLeaveDetails.Update(allLeaveDetailsEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Logic to update all leave details 
        /// </summary>   
        /// <param name="allLeaveDetailsEntity" ></param> 
        public async Task<bool> UpdateAllLeaveDeatils(AllLeaveDetailsEntity allLeaveDetailsEntity)
        {
            var result = false;
            try
            {
                if (allLeaveDetailsEntity != null)
                {
                    allLeaveDetailsEntity.CompanyId = allLeaveDetailsEntity.CompanyId;
                    _dbContext.AllLeaveDetails.Update(allLeaveDetailsEntity);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// Logic to get reporterEmpId the employees detail by particular employees
        /// </summary>   
        /// <param name="reporterEmpId" >employees</param>
        /// <param name="OfficeEmail" >employees</param>      
        public async Task<string> GetEmployeeEmailByEmpIdForLeave(int reporterEmpId)
        {
            var result = await _dbContext.Employees.Where(x => x.EmpId == reporterEmpId).Select(y => y.OfficeEmail).FirstOrDefaultAsync();
            return result ?? string.Empty;
        }

        /// Leave Type 

        /// <summary>
        /// Logic to get leave list 
        /// </summary>                   
        public async Task<List<LeaveTypesEntity>> GetAllLeave()
        {
            var result = await _dbContext.leaveTypes.Where(x => !x.IsDeleted && x.IsActive).ToListAsync();
            return result;
        }
        public async Task<List<LeaveTypesEntity>> GetLeaveType(int empId)
        {
            var privilage = await _dbContext.EmployeePrivileges.Where(x => x.EmployeeID == empId).FirstOrDefaultAsync();
            if(privilage?.IsEarnLeave == true && privilage != null)
            {
                var result = await _dbContext.leaveTypes.Where(x => !x.IsDeleted && x.IsActive).ToListAsync();
                return result;
            }
            else
            {
                var result = await _dbContext.leaveTypes.Where(x => !x.IsDeleted && x.IsActive && x.LeaveTypeId !=3 && x.LeaveTypeId !=31).ToListAsync();
                return result;
            }
            
        }

        /// <summary>
        /// Logic to get appliedLeaveTypeId the leavetype detail by particular leavetype
        /// </summary>   
        /// <param name="AppliedLeaveTypeId" >leavetype</param>      
        /// <param name="CompanyId" >leavetype</param>
        public async Task<LeaveTypesEntity> GetAllLeaveDetailsDashboard(int AppliedLeaveTypeId, int companyId)
        {
            var leaveTypesEntity = await _dbContext.leaveTypes.FirstOrDefaultAsync(r => r.LeaveTypeId == AppliedLeaveTypeId && r.CompanyId == companyId);
            return leaveTypesEntity ?? new LeaveTypesEntity();
        }

        //// Employee Holiday

        /// <summary>
        /// Logic to get create holiday the employeeholidaysentities detail
        /// </summary>   
        /// <param name="employeeHolidaysEntity" ></param> 
        public async Task<bool> CreateHoliday(EmployeeHolidaysEntity employeeHolidaysEntity, int companyId)
        {
            var result = false;
            if (employeeHolidaysEntity != null)
            {
                employeeHolidaysEntity.CompanyId = companyId;
                await _dbContext.EmployeeHolidaysEntities.AddAsync(employeeHolidaysEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
         /// Logic to get update holiday the employeeholidaysentities detail by particular employeeholidaysentities
          /// </summary>   
        /// <param name="employeeHolidaysEntity" ></param>       
        /// <param name="CompanyId" >employeeholidaysentities</param>
        public async Task<bool> UpdateHoliday(EmployeeHolidaysEntity employeeHolidaysEntity)
        {
            var result = false;
            var employeeholidaysEntity = await _dbContext.EmployeeHolidaysEntities.Where(e => e.HolidayId == employeeHolidaysEntity.HolidayId && e.CompanyId == employeeHolidaysEntity.CompanyId).FirstOrDefaultAsync();
            if (employeeholidaysEntity != null)
            {
                employeeholidaysEntity.Title = employeeHolidaysEntity.Title;
                employeeholidaysEntity.UpdatedBy = employeeHolidaysEntity.UpdatedBy;
                employeeholidaysEntity.UpdatedDate = employeeHolidaysEntity.UpdatedDate;
                _dbContext.EmployeeHolidaysEntities.Update(employeeholidaysEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete holiday the employeeholidaysentities detail by particular employeeholidaysentities
        /// </summary>   
        /// <param name="employeeHolidays,sessionEmployeeId" ></param>       
        /// <param name="CompanyId" >employeeholidaysentities</param>
        /// <param name="IsDeleted" >employeeholidaysentities</param>
        public async Task<bool> DeleteHoliday(EmployeeHolidays employeeHolidays, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var employeeHolidaysEntities = await _dbContext.EmployeeHolidaysEntities.Where(e => e.HolidayId == employeeHolidays.HolidayId && !e.IsDeleted && e.CompanyId == companyId).FirstOrDefaultAsync();
            if (employeeHolidaysEntities != null)
            {
                employeeHolidaysEntities.IsDeleted = true;
                employeeHolidaysEntities.UpdatedBy = sessionEmployeeId;
                employeeHolidaysEntities.UpdatedDate = DateTime.Now;
                _dbContext.EmployeeHolidaysEntities.Update(employeeHolidaysEntities);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get holidayDate count the employeeholidaysentities detail 
        /// </summary>   
        /// <param name="holidayDate" ></param>       
        /// <param name="CompanyId" >employeeholidaysentities</param>     
        public async Task<int> GetHolidayDate(string holidayDate, int companyId)
        {
            var changeDate = DateTimeExtensions.ConvertToNotNullDatetime(holidayDate);
            var moduleNameCount = await _dbContext.EmployeeHolidaysEntities.Where(x => x.HolidayDate == changeDate && x.CompanyId == companyId && !x.IsDeleted).CountAsync();
            return moduleNameCount;
        }

        /// <summary>
        /// Logic to get holidayDate count the employeeholidaysentities detail 
        /// </summary>   
        /// <param name="holidayDate" ></param> 
        /// <param name="holidayid" ></param>
        /// <param name="CompanyId" >employeeholidaysentities</param>  
        public async Task<int> GetHolidayDatesId(string holidayDate, int holidayid, int companyId)
        {
            var changedDate = DateTimeExtensions.ConvertToNotNullDatetime(holidayDate);
            var moduleNameCount = await _dbContext.EmployeeHolidaysEntities.Where(x => x.HolidayDate == changedDate && x.HolidayId == holidayid && x.CompanyId == companyId).CountAsync();
            return moduleNameCount;
        }

        /// <summary>
        /// Logic to get holidays list 
        /// </summary>   
        /// <param name="IsDeleted" >employeeholidaysentities</param>       
        /// <param name="CompanyId" >employeeholidaysentities</param>
        public async Task<List<EmployeeHolidaysEntity>> GetAllEmployeeHolidays(int companyId)
        {
            var result = await _dbContext.EmployeeHolidaysEntities.Where(x => !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            return result;
        }

        /// <summary>
        /// Logic to get holidays list 
        /// </summary>   
        /// <param name="IsDeleted" >employeeholidaysentities</param>  
        /// <param name="CompanyId" >employeeholidaysentities</param>
        public async Task<List<EmployeeHolidaysEntity>> GetAllEmployeeHolidaysForYear(int month, int year, int companyId)
        {
            var result = await _dbContext.EmployeeHolidaysEntities.Where(x => x.HolidayDate.Month == month && x.HolidayDate.Year == year && !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            return result;
        }

        /// <summary>
        /// Logic to get holidays count 
        /// </summary>   
        /// <param name="startdate" >employeeholidaysentities</param>  
        /// <param name="enddate" >employeeholidaysentities</param>
        public async Task<int> GetAllEmployeeHolidaysForMOnth(DateTime startdate, DateTime enddate, int companyId)
        {
            var result =  _dbContext.EmployeeHolidaysEntities.Where(x => x.HolidayDate.Date >= startdate && x.HolidayDate.Date <= enddate && !x.IsDeleted && x.CompanyId == companyId).Select(y =>y.HolidayDate.Date).Count();
            return result;
        }


        //// Compensatory

        /// <summary>
         /// Logic to create and update compensatory using copensatoryId
          /// </summary>   
        /// <param name="compensatoryRequestsEntity" ></param> 
        public async Task<bool> CreateCompensatory(CompensatoryRequestsEntity compensatoryRequestsEntity, int compensatoryId)
        {

            var result = false;
            if (compensatoryRequestsEntity != null && compensatoryId == 0)
            {
                await _dbContext.CompensatoryRequestsEntity.AddAsync(compensatoryRequestsEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                _dbContext.CompensatoryRequestsEntity.Update(compensatoryRequestsEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get compensatory count by empId
        /// </summary>   
        /// <param name="empId" >CompensatoryRequestsEntity</param>
        /// <param name="CompanyId" >CompensatoryRequestsEntity</param>
        public async Task<List<CompensatoryRequestsEntity>> GetCompensatoryOffCountByEmpId(int empId,int companyId)
        {
            var allLeaveDetailsEntity = await _dbContext.CompensatoryRequestsEntity.Where(x => x.EmpId == empId && x.CompanyId == companyId && !x.IsDeleted && x.IsApproved == 1).ToListAsync();
            return allLeaveDetailsEntity;
        }


        /// <summary>
        /// Logic to approved the compensatory request 
         /// </summary>   
        /// <param name="sessionEmployeeId" >CompensatoryRequestsEntity</param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<bool> ApprovedCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId)
        {
            var result = false;
            var compensatoryRequestsEntity = await _dbContext.CompensatoryRequestsEntity.Where(e => e.CompensatoryId == compensatoryRequest.CompensatoryId && !e.IsDeleted && e.CompanyId == compensatoryRequest.CompanyId).FirstOrDefaultAsync();
            if (compensatoryRequestsEntity != null)
            {
                compensatoryRequestsEntity.IsApproved = 1;
                compensatoryRequestsEntity.Reason = compensatoryRequest.Reason;
                compensatoryRequestsEntity.DayCount = 1;
                _dbContext.CompensatoryRequestsEntity.Update(compensatoryRequestsEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to reject the compensatory request 
        /// </summary>   
        /// <param name="sessionEmployeeId" >CompensatoryRequestsEntity</param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> RejectCompensatoryOff(CompensatoryRequest compensatoryRequest, int sessionEmployeeId)
        {
            var result = 0;
            var compensatoryRequestsEntity = await _dbContext.CompensatoryRequestsEntity.Where(e => e.CompensatoryId == compensatoryRequest.CompensatoryId && !e.IsDeleted && e.CompanyId == compensatoryRequest.CompanyId).FirstOrDefaultAsync();
            if (compensatoryRequestsEntity != null)
            {
                compensatoryRequestsEntity.IsApproved = 2;
                compensatoryRequestsEntity.DayCount = 0;
                compensatoryRequestsEntity.Reason = compensatoryRequest.Reason;
                _dbContext.CompensatoryRequestsEntity.Update(compensatoryRequestsEntity);
                await _dbContext.SaveChangesAsync();
                result = 1;
            }
            return result;
        }


        /// <summary>
        /// Logic to get compensatory request by compensatory Id
        /// </summary>   
        /// <param name="compensatoryId" >CompensatoryRequestsEntity</param>
        /// <param name="CompanyId" ></param>
        public async Task<CompensatoryRequestsEntity> GetCompensatoryOffRequestByCompensatoryId(int compensatoryId,int companyId)
        {
            var result = new CompensatoryRequestsEntity();
            if (compensatoryId > 0)
            {
                result = await _dbContext.CompensatoryRequestsEntity.FirstOrDefaultAsync(x => x.CompensatoryId == compensatoryId && x.CompanyId == companyId);
            }

            return result ?? new CompensatoryRequestsEntity();
        }


        /// <summary>
        /// Logic to get all employees compensatoryoff request
        /// </summary>   
        /// <param name="empIds" >CompensatoryRequestsEntity</param>       
        /// <param name="CompanyId" >CompensatoryRequestsEntity</param> 
        /// <param name="IsDeleted" >CompensatoryRequestsEntity</param> 
        public async Task<List<CompensatoryRequestsEntity>> GetAllCompensatoryOffRequest(int empId,int companyId)
        {
            var result = new List<CompensatoryRequestsEntity>();
            if (empId == 0)
            {
                result = await _dbContext.CompensatoryRequestsEntity.Where(x => !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
            else
            {
                result = await _dbContext.CompensatoryRequestsEntity.Where(x => x.EmpId == empId && !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
            return result;
        }


        /// <summary>
        /// Logic to get all reporting person compensatoryoff request
        /// </summary>   
        /// <param name="empIds" >CompensatoryRequestsEntity</param>       
        /// <param name="CompanyId" >CompensatoryRequestsEntity</param> 
        /// <param name="IsDeleted" >CompensatoryRequestsEntity</param> 
        public async Task<List<CompensatoryRequestsEntity>> GetAllCompensatoryOffRequests(List<int> empIds, int companyId)
        {
            var result = new List<CompensatoryRequestsEntity>();
            var reportingPersonsEntity = await _dbContext.CompensatoryRequestsEntity.Where(e => empIds.Contains(e.EmpId) && !e.IsDeleted && e.CompanyId == companyId).ToListAsync();
            return reportingPersonsEntity;

        }


        /// <summary>
        /// Logic to get compensatoryoff request by compensatoryId
        /// </summary>   
        /// <param name="compensatoryId" >CompensatoryRequestsEntity</param>   
        public async Task<CompensatoryRequestsEntity> GetCompensatoryRequestByCompensatoryId(int compensatoryId)
        {
            var entity = await _dbContext.CompensatoryRequestsEntity.Where(x => x.CompensatoryId == compensatoryId).FirstOrDefaultAsync();
            return entity;
        }

        /// <summary>
        /// Logic to get top five wfh using dashboard
        /// </summary>          
        public async Task<List<TopFiveLeaveViews>> GetTopFiveWfh()
        {
            var currentDate = DateTime.Now;
            var topFiveLeaveWfhView = await (from emp in _dbContext.Employees
                                             join applea in _dbContext.employeeAppliedLeaveEntities on emp.EmpId equals applea.EmpId
                                             join leave in _dbContext.leaveTypes on applea.AppliedLeaveTypeId equals leave.LeaveTypeId
                                             where applea.LeaveFromDate.Date == currentDate.Date && applea.IsApproved == 1 && !applea.IsDeleted
                                             select new TopFiveLeaveViews()
                                             {
                                                 EmpId = emp.EmpId,
                                                 EmployeeName = emp.FirstName + " " + emp.LastName,
                                                 LeaveType = leave.LeaveType,
                                                 LeaveDate = applea.LeaveFromDate,
                                                 EmployeeStatus = emp.IsDeleted,
                                             }).ToListAsync();
            return topFiveLeaveWfhView;
        }

        /// <summary>
        /// Logic to get top five leave type view details by particular empId
        /// </summary>   
        /// <param name="empId" ></param> 
        public async Task<List<TopFiveLeaveViews>> TopFiveLeaveTypeView(int empId)
        {
            var currentDate = DateTime.Now;
            var TopFiveLeaveTypeView = await (from emp in _dbContext.Employees
                                              join appleave in _dbContext.employeeAppliedLeaveEntities on empId equals appleave.EmpId
                                              join leave in _dbContext.leaveTypes on appleave.AppliedLeaveTypeId equals leave.LeaveTypeId
                                              where (appleave.LeaveFromDate.Date >= currentDate.Date && !appleave.IsDeleted && appleave.IsApproved <= 1 && emp.EmpId == empId)
                                              orderby (appleave.LeaveFromDate.Month)
                                              select new TopFiveLeaveViews()
                                              {
                                                  EmpId = emp.EmpId,
                                                  EmployeeName = emp.FirstName + " " + emp.LastName,
                                                  LeaveType = leave.LeaveType,
                                                  LeaveDate = appleave.LeaveFromDate,
                                                  LeaveApproved = appleave.IsApproved,
                                                  EmployeeStatus = emp.IsDeleted,
                                              }).ToListAsync();
            return TopFiveLeaveTypeView;
        }

        /// <summary>
        /// Logic to get all employees holidays list
        /// </summary>          
        public async Task<List<LeaveList>> EmployeeHolidays(int companyId)
        {
            var currentDate = DateTime.Now;
            var employeeHolidays = await (from holi in _dbContext.EmployeeHolidaysEntities
                                          where (holi.HolidayDate > currentDate && !holi.IsDeleted && holi.HolidayDate.Year == currentDate.Year && holi.CompanyId == companyId)
                                          orderby holi.HolidayDate
                                          select new LeaveList()
                                          {
                                              LeaveDate = holi.HolidayDate,
                                              LeaveDay = Convert.ToString(Convert.ToDateTime(holi.HolidayDate).DayOfWeek),
                                              Title = holi.Title,
                                          }).Take(5).ToListAsync();
            return employeeHolidays;
        }

        /// <summary>
        /// Logic to get  employee leave by particular empId
        /// </summary> 
        /// <param name="empId" ></param> 
        public async Task<List<EmployeeLeaveViewModel>> GetLeaveByEmpId(int empId)
        {
            var employeeLeave = await (from empLeave in _dbContext.employeeAppliedLeaveEntities
                                       join leaveType in _dbContext.leaveTypes on empLeave.AppliedLeaveTypeId equals leaveType.LeaveTypeId
                                       join pro in _dbContext.ProfileInfo on empId equals pro.EmpId
                                       join emp in _dbContext.Employees on empId equals emp.EmpId
                                       where (empLeave.EmpId == empId)
                                       select new EmployeeLeaveViewModel()
                                       {
                                           AppliedLeaveId = empLeave.AppliedLeaveId,
                                           AppliedLeaveTypeId = empLeave.AppliedLeaveTypeId,
                                           EmpId = empId,
                                           IsApproved = empLeave.IsApproved,
                                           AppliedLeave = empLeave.LeaveApplied,
                                           LeaveTypes = leaveType.LeaveType,
                                           LeaveFromDate = empLeave.LeaveFromDate,
                                           LeaveToDate = empLeave.LeaveToDate,
                                           TotalLeave = empLeave.LeaveApplied,
                                           Reason = empLeave.Reason,
                                           LeaveFilePath = empLeave.LeaveFilePath,
                                           LeaveName = empLeave.LeaveName,
                                           EmployeeUserName = emp.UserName,
                                           EmployeeName = emp.FirstName + " " + emp.LastName,
                                           EmployeeProfileImage = pro.ProfileName,
                                           EmployeeStatus = emp.IsDeleted,
                                       }).ToListAsync();
            return employeeLeave;
        }

        /// <summary>
        /// Logic to get  employee leave by particular empId
        /// </summary> 
        /// <param name="empId, pager, columnName, columnDirection" ></param> 
        public async Task<List<EmployeeLeavesModel>> GetLeavesByEmpId(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId)
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
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param3 = new NpgsqlParameter("@offsetValue", pager.sEcho);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var empLeaveById = await _dbContext.employeeAppliedLeavesEntities.FromSqlRaw("EXEC [dbo].[spGetEmployeeLeaveDetails] @empId, @companyId, @pagingSize, @offsetValue, @searchText, @sorting", param, param1, param2, param3, param4, param5).ToListAsync();
                return empLeaveById;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Logic to get all employee leave details list
        /// </summary>         
        public async Task<List<EmployeeLeaveViewModel>> GetAllEmployeesLeave(int companyId)
        {
            var employeeLeave = await (from empLeave in _dbContext.employeeAppliedLeaveEntities
                                       join leaveType in _dbContext.leaveTypes on empLeave.AppliedLeaveTypeId equals leaveType.LeaveTypeId
                                       join pro in _dbContext.ProfileInfo on empLeave.EmpId equals pro.EmpId
                                       join emp in _dbContext.Employees on empLeave.EmpId equals emp.EmpId
                                       where (!empLeave.IsDeleted && empLeave.AppliedLeaveTypeId != 7 && companyId == empLeave.CompanyId)
                                       select new EmployeeLeaveViewModel()
                                       {
                                           AppliedLeaveId = empLeave.AppliedLeaveId,
                                           AppliedLeaveTypeId = empLeave.AppliedLeaveTypeId,
                                           EmpId = empLeave.EmpId,
                                           IsApproved = empLeave.IsApproved,
                                           AppliedLeave = empLeave.LeaveApplied,
                                           LeaveTypes = leaveType.LeaveType,
                                           LeaveFromDate = empLeave.LeaveFromDate,
                                           LeaveToDate = empLeave.LeaveToDate,
                                           TotalLeave = empLeave.LeaveApplied,
                                           Reason = empLeave.Reason,
                                           LeaveFilePath = empLeave.LeaveFilePath,
                                           LeaveName = empLeave.LeaveName,
                                           EmployeeUserName = emp.UserName,
                                           EmployeeName = emp.FirstName + " " + emp.LastName,
                                           EmployeeProfileImage = pro.ProfileName,
                                           EmployeeStatus = emp.IsDeleted,
                                       }).ToListAsync();
            return employeeLeave;
        }

        /// <summary>
        /// Logic to get all employee leave details list
        /// </summary>
        /// <param name="empId, pager, columnName, columnDirection" ></param> 
        public async Task<List<EmployeeLeavesModel>> GetAllEmployeesLeaves(SysDataTablePager pager, int empId, string columnName, string columnDirection,int companyId)
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
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param3 = new NpgsqlParameter("@offsetValue", pager.sEcho);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var allEmpLeave = await _dbContext.employeeAppliedLeavesEntities.FromSqlRaw("EXEC [dbo].[spGetLeaveDetailsFilterList] @empId, @companyId, @pagingSize, @offsetValue, @searchText, @sorting", param, param1, param2, param3, param4, param5).ToListAsync();
                return allEmpLeave;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Logic to get employee reporting person leave details  by particular empId
        /// </summary> 
        /// <param name="empId" ></param>
        public async Task<List<EmployeeLeaveViewModel>> GetReportingPersonsLeave(int empId)
        {
            var employeeLeave = await (from empLeave in _dbContext.employeeAppliedLeaveEntities
                                       join leaveType in _dbContext.leaveTypes on empLeave.AppliedLeaveTypeId equals leaveType.LeaveTypeId
                                       join pro in _dbContext.ProfileInfo on empLeave.EmpId equals pro.EmpId
                                       join emp in _dbContext.Employees on empLeave.EmpId equals emp.EmpId
                                       where (_dbContext.ReportingPersonsEntities.Where(e => e.ReportingPersonEmpId == empId).Select(e => e.EmployeeId).Contains(empLeave.EmpId) && empLeave.AppliedLeaveTypeId != 7)
                                       select new EmployeeLeaveViewModel()
                                       {
                                           AppliedLeaveId = empLeave.AppliedLeaveId,
                                           AppliedLeaveTypeId = empLeave.AppliedLeaveTypeId,
                                           EmpId = empLeave.EmpId,
                                           IsApproved = empLeave.IsApproved,
                                           AppliedLeave = empLeave.LeaveApplied,
                                           LeaveTypes = leaveType.LeaveType,
                                           LeaveFromDate = empLeave.LeaveFromDate,
                                           LeaveToDate = empLeave.LeaveToDate,
                                           TotalLeave = empLeave.LeaveApplied,
                                           Reason = empLeave.Reason,
                                           LeaveFilePath = empLeave.LeaveFilePath,
                                           LeaveName = empLeave.LeaveName,
                                           EmployeeUserName = emp.UserName,
                                           EmployeeName = emp.FirstName + " " + emp.LastName,
                                           EmployeeProfileImage = pro.ProfileName,
                                           EmployeeStatus = emp.IsDeleted,
                                       }).ToListAsync();
            return employeeLeave;
        }


        /// <summary>
        /// Logic to get employee reporting person leave details  by particular empId
        /// </summary> 
        /// <param name="empId, pager, columnName, columnDirection" ></param>
        public async Task<List<EmployeeLeavesModel>> GetReportingPersonsLeaves(SysDataTablePager pager, int empId, string columnName, string columnDirection, int companyId)
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
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param3 = new NpgsqlParameter("@offsetValue", pager.sEcho);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var repLeaveModel = await _dbContext.employeeAppliedLeavesEntities.FromSqlRaw("EXEC [dbo].[spGetLeaveDetailsFilterList] @empId, @companyId, @pagingSize, @offsetValue, @searchText, @sorting", param, param1, param2, param3, param4, param5).ToListAsync();
                return repLeaveModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logic to get all employees work from home leave details list
        /// </summary>        
        public async Task<List<EmployeeLeaveViewModel>> GetAllWorkFromHome(int companyId)
        {
            var employeesWFH = await (from empLeave in _dbContext.employeeAppliedLeaveEntities
                                      join emp in _dbContext.Employees on empLeave.EmpId equals emp.EmpId
                                      join pro in _dbContext.ProfileInfo on empLeave.EmpId equals pro.EmpId
                                      join leaveType in _dbContext.leaveTypes on empLeave.AppliedLeaveTypeId equals leaveType.LeaveTypeId
                                      where (!empLeave.IsDeleted && empLeave.AppliedLeaveTypeId == 7 && companyId == empLeave.CompanyId)
                                      select new EmployeeLeaveViewModel()
                                      {
                                          AppliedLeaveId = empLeave.AppliedLeaveId,
                                          AppliedLeaveTypeId = empLeave.AppliedLeaveTypeId,
                                          EmpId = empLeave.EmpId,
                                          IsApproved = empLeave.IsApproved,
                                          AppliedLeave = empLeave.LeaveApplied,
                                          LeaveTypes = leaveType.LeaveType,
                                          LeaveFromDate = empLeave.LeaveFromDate,
                                          LeaveToDate = empLeave.LeaveToDate,
                                          TotalLeave = empLeave.LeaveApplied,
                                          Reason = empLeave.Reason,
                                          LeaveFilePath = empLeave.LeaveFilePath,
                                          LeaveName = empLeave.LeaveName,
                                          EmployeeUserName = emp.UserName,
                                          EmployeeName = emp.FirstName + " " + emp.LastName,
                                          EmployeeProfileImage = pro.ProfileName,
                                          EmployeeStatus = emp.IsDeleted,
                                      }).ToListAsync();
            return employeesWFH;

        }

        /// <summary>
        /// Logic to get employee and reporting person work from home leave details by particular empId
        /// </summary> 
        /// <param name="empId" ></param>
        public async Task<List<EmployeeLeaveViewModel>> GetEmployeeReportingWorkFromHome(int empId, int companyId)
        {

            var employeeWFH = await (from empLeave in _dbContext.employeeAppliedLeaveEntities
                                     join emp in _dbContext.Employees on empLeave.EmpId equals emp.EmpId
                                     join pro in _dbContext.ProfileInfo on empLeave.EmpId equals pro.EmpId
                                     join leaveType in _dbContext.leaveTypes on empLeave.AppliedLeaveTypeId equals leaveType.LeaveTypeId
                                     where ((_dbContext.ReportingPersonsEntities.Where(y => y.ReportingPersonEmpId == empId).Select(y => y.EmployeeId).Contains(empLeave.EmpId))
                                     || empLeave.EmpId == empId) && !empLeave.IsDeleted && empLeave.AppliedLeaveTypeId == 7 && companyId == empLeave.CompanyId
                                     select new EmployeeLeaveViewModel()
                                     {
                                         AppliedLeaveId = empLeave.AppliedLeaveId,
                                         AppliedLeaveTypeId = empLeave.AppliedLeaveTypeId,
                                         EmpId = empLeave.EmpId,
                                         IsApproved = empLeave.IsApproved,
                                         AppliedLeave = empLeave.LeaveApplied,
                                         LeaveTypes = leaveType.LeaveType,
                                         LeaveFromDate = empLeave.LeaveFromDate,
                                         LeaveToDate = empLeave.LeaveToDate,
                                         TotalLeave = empLeave.LeaveApplied,
                                         Reason = empLeave.Reason,
                                         LeaveFilePath = empLeave.LeaveFilePath,
                                         LeaveName = empLeave.LeaveName,
                                         EmployeeUserName = emp.UserName,
                                         EmployeeName = emp.FirstName + " " + emp.LastName,
                                         EmployeeProfileImage = pro.ProfileName,
                                         EmployeeStatus = emp.IsDeleted,
                                     }).ToListAsync();
            return employeeWFH;
        }

        /// <summary>
        /// Logic to get all employee com off request leave details list
        /// </summary>       
        public async Task<List<CompensatoryRequestViewModel>> GetAllEmployeeComOff(int companyId)
        {
            var employeesComOff = await (from comOff in _dbContext.CompensatoryRequestsEntity
                                         join emp in _dbContext.Employees on comOff.EmpId equals emp.EmpId
                                         join pro in _dbContext.ProfileInfo on comOff.EmpId equals pro.EmpId
                                         where !comOff.IsDeleted && companyId == comOff.CompanyId
                                         select new CompensatoryRequestViewModel()
                                         {
                                             CompanyId = comOff.CompanyId,
                                             CompensatoryId = comOff.CompensatoryId,
                                             EmpId = comOff.EmpId,
                                             IsApproved = comOff.IsApproved,
                                             WorkedDate = comOff.WorkedDate,
                                             Remark = comOff.Remark,
                                             Reason = string.IsNullOrEmpty(comOff.Reason) ? string.Empty : comOff.Reason,
                                             DayCount = comOff.DayCount,
                                             EmployeeUserName = emp.UserName,
                                             EmployeeName = emp.FirstName + " " + emp.LastName,
                                             EmployeeStatus = emp.IsDeleted,
                                             EmployeeProfileImage = pro.ProfileName,
                                         }).ToListAsync();
            return employeesComOff;
        }

        /// <summary>
        /// Logic to get WorkFromHome data count of the employees
        /// </summary>
        /// <param name="empId, companyId,pager"></param>
        public async Task<int> WorkFromHomeCount(int empId, int companyId, SysDataTablePager pager)
        {
            try
            {
                var result = 0;
                var employeeId = Convert.ToString(empId);
                var comId = Convert.ToString(companyId);
                
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);

                List<WorkFromHomeFilterCount> workFromHomeFilterCount = await _dbContext.workFromHomeFilterCount.FromSqlRaw("EXEC [dbo].[spGetWorkFromHomeCount] @empId, @companyId,@searchText", param, param1, param2).ToListAsync();
                foreach (var item in workFromHomeFilterCount)
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

        public async Task<int> WorkFromHomeForTeamLeadCount(int empId, int companyId, SysDataTablePager pager)
        {
            try
            {
                var result = 0;
                var employeeId = Convert.ToString(empId);
                var comId = Convert.ToString(companyId);

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);

                List<WorkFromHomeFilterCount> workFromHomeFilterCount = await _dbContext.workFromHomeFilterCount.FromSqlRaw("EXEC [dbo].[spGetWorkFromHomeFilterForTeamLeadCount] @empId, @companyId,@searchText", param, param1, param2).ToListAsync();
                foreach (var item in workFromHomeFilterCount)
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
        /// Logic to get WorkFromHome Filtered data of the employees
        /// </summary>
        /// <param name="empId, companyId,pager,columnName,columnDirection"></param>
        public async Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterData(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                var result = 0;
                var employeeId = Convert.ToString(empId);
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
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param3 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var data = await _dbContext.workFromHomeFilter.FromSqlRaw("EXEC [dbo].[spGetWorkFromHomeFilter] @empId,@companyId,@pagingSize,@offsetValue,@searchText,@sorting", param, param1, param2, param3, param4,param5).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Logic to get WorkFromHome Filtered data of the employees
        /// </summary>
        /// <param name="empId, companyId,pager,columnName,columnDirection"></param>
        public async Task<List<WorkFromHomeFilterViewmodel>> GetWorkFromHomeFilterDataForTeamLead(int empId, int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            try
            {
                var result = 0;
                var employeeId = Convert.ToString(empId);
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
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", _params.PagingSize);
                var param3 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var data = await _dbContext.workFromHomeFilter.FromSqlRaw("EXEC [dbo].[spGetWorkFromHomeFilterForTeamLead] @empId,@companyId,@pagingSize,@offsetValue,@searchText,@sorting", param, param1, param2, param3, param4, param5).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Logic to get all employee com off request leave details list
        /// </summary>   
        /// <param name="Employee" ></param> 
        /// <param name="pager" ></param> 
        public async Task<List<EmployeeCompensatoryFilter>> GetAllEmployeeCompenSatoryDetails(SysDataTablePager pager, int Employee, string columnDirection, string columnName, int companyId)
        {
            var empId = Employee;
            var _params = new
            {
                OffsetValue = (pager.sEcho == 0) ? 0 : (pager.sEcho),
                PagingSize = pager.iDisplayLength,
                SearchText = (pager.sSearch == null) ? "" : pager.sSearch,
                Sorting = columnName + " " + columnDirection,
            };
            var param = new NpgsqlParameter("@empId", empId);
            var param1 = new NpgsqlParameter("@companyId", companyId);
            var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            var param3 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
            var param4 = new NpgsqlParameter("@offsetValue", _params.OffsetValue);
            var param5 = new NpgsqlParameter("@sorting", string.IsNullOrEmpty(_params.Sorting) ? DBNull.Value : (object)_params.Sorting);
            try
            {
               var data = await _dbContext.EmployeeCompensatoryFilterModel.FromSqlRaw("EXEC [dbo].[spGetCompensatoryDetailsForTeamLead] @empId,@companyId,@searchText,@pagingSize,@offsetValue,@sorting", param, param1, param2, param3, param4, param5).ToListAsync();
                return data;
            }
            catch (Exception ex) { }
            var data1 = new List<EmployeeCompensatoryFilter>();
            return data1;
        }
        /// <summary>
        /// Logic to get all employee Count com off request leave details list
        /// </summary>   
        /// <param name="Employee" ></param> 
        /// <param name="pager" ></param> 
        public async Task<int> GetAllEmployeeCompenSatoryDetailsCount(SysDataTablePager pager, int Employee, int companyId)
        {

            var result = 0;
            var empId = Employee;

            var _params = new
            {

                SearchText = pager.sSearch,
            };

            var param = new NpgsqlParameter("@empId", empId);
            var param1 = new NpgsqlParameter("@companyId", companyId);
            var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
            
                var data = await _dbContext.CompensatoryRequestsCountModel.FromSqlRaw("EXEC [dbo].[spGetCompensatoryDetailsForTeamLeadCount] @empId,@companyId,@searchText", param, param1, param2).ToListAsync();

            var empCount = 0;

            foreach(var item in data)
            {
                empCount = item.EmployeeCount;
            }
            return empCount;



        }

        /// <summary>
        /// Logic to get employee and reporting person com off request leave details by particular empId
        /// </summary>   
        /// <param name="empId" ></param>
        public async Task<List<CompensatoryRequestViewModel>> GetReportingEmployeesComOff(int empId,int companyId)
        {
            var employeeComOff = await (from comOff in _dbContext.CompensatoryRequestsEntity
                                        join emp in _dbContext.Employees on comOff.EmpId equals emp.EmpId
                                        join pro in _dbContext.ProfileInfo on comOff.EmpId equals pro.EmpId
                                        where ((_dbContext.ReportingPersonsEntities.Where(x => x.ReportingPersonEmpId == empId).Select(x => x.EmployeeId).Contains(comOff.EmpId))
                                        || comOff.EmpId == empId) && !comOff.IsDeleted && companyId == comOff.CompanyId
                                        select new CompensatoryRequestViewModel()
                                        {
                                            CompanyId = comOff.CompanyId,
                                            EmpId = comOff.EmpId,
                                            CompensatoryId = comOff.CompensatoryId,
                                            IsApproved = comOff.IsApproved,
                                            WorkedDate = comOff.WorkedDate,
                                            Reason = string.IsNullOrEmpty(comOff.Reason) ? string.Empty : comOff.Reason,
                                            Remark = comOff.Remark,
                                            DayCount = comOff.DayCount,
                                            EmployeeName = emp.FirstName + " " + emp.LastName,
                                            EmployeeStatus = emp.IsDeleted,
                                            EmployeeProfileImage = pro.ProfileName,
                                        }).ToListAsync();
            return employeeComOff;
        }

        /// <summary>
        /// Logic to get employees holidays details list
        /// </summary>          
        public async Task<EmployeeHolidaysViewModel> GetAllEmployeesHolidays(int year, int companyId)
        {
            var employeeHolidays = new EmployeeHolidaysViewModel();
            var currentDate = DateTime.Now;
            if (year == 0)
            {
                employeeHolidays.EmployeeHolidays = await (from holiday in _dbContext.EmployeeHolidaysEntities
                                                           where !holiday.IsDeleted && companyId == holiday.CompanyId && holiday.HolidayDate.Year == currentDate.Year
                                                           select new EmployeeHolidays
                                                           {
                                                               HolidayId = holiday.HolidayId,
                                                               Title = holiday.Title,
                                                               HolidayDate = holiday.HolidayDate,
                                                               Holiday = holiday.HolidayDate.ToString("dd/MM/yyyy"),

                                                               HolidayName = holiday.HolidayDate.DayOfWeek.ToString(),
                                                           }).ToListAsync();
                return employeeHolidays;
            }
            else
            {
                employeeHolidays.EmployeeHolidays = await (from holiday in _dbContext.EmployeeHolidaysEntities
                                                           where !holiday.IsDeleted && companyId == holiday.CompanyId && holiday.HolidayDate.Year == year
                                                           select new EmployeeHolidays
                                                           {
                                                               HolidayId = holiday.HolidayId,
                                                               Title = holiday.Title,
                                                               HolidayDate = holiday.HolidayDate,
                                                               Holiday = holiday.HolidayDate.ToString("dd/MM/yyyy"),

                                                               HolidayName = holiday.HolidayDate.DayOfWeek.ToString(),
                                                           }).ToListAsync();
                return employeeHolidays;
            }

        }

        /// <summary>
        /// Logic to get all leave type details list
        /// </summary>
        public async Task<List<LeaveTypes>> GetAllLeaveTypes()
        {
            var leaveType = new LeaveTypes();
            leaveType.LeaveTypeId = 0;
            leaveType.LeaveType = Common.Constant.AllLeaveType;
            var listOfLeaveType = new List<LeaveTypes>();
            listOfLeaveType.Add(leaveType);
            var leaveTypes = await (from leavetypes in _dbContext.leaveTypes
                                    where !leavetypes.IsDeleted && leavetypes.IsActive
                                    select new LeaveTypes()
                                    {
                                        LeaveTypeId = leavetypes.LeaveTypeId,
                                        LeaveType = leavetypes.LeaveType,
                                    }).ToListAsync();
            listOfLeaveType.AddRange(leaveTypes);
            return listOfLeaveType;
        }

        /// <summary>
        /// Logic to get all employee remainig leave type details list
        /// </summary>
        public async Task<List<EmployeeLeaveViewModel>> GetEmployeeLeaveViewModel(int companyId)
        {
            var employeeLeaveViewModel = await (from employees in _dbContext.Employees
                                                where companyId == employees.CompanyId && !employees.IsDeleted
                                                select new EmployeeLeaveViewModel()
                                                {
                                                    EmpId = employees.EmpId,
                                                    EmployeeName = employees.FirstName + " " + employees.LastName,
                                                    UserName = employees.UserName,
                                                    LeavesType = _dbContext.leaveTypes.Where(x => !x.IsDeleted && x.IsActive && x.LeaveTypeId != (int)LeavetypeStatus.Permission).ToList(),
                                                    AllLeaveDetail = _dbContext.AllLeaveDetails.Where(x => x.EmpId == employees.EmpId && x.CompanyId == companyId && x.LeaveYear == DateTime.Now.Year).ToList(),
                                                    EmployeeAppliedLeave = _dbContext.employeeAppliedLeaveEntities.Where(x => x.EmpId == employees.EmpId && !x.IsDeleted && x.CompanyId == companyId && x.CreatedDate.Year == DateTime.Now.Year).ToList(),
                                                }).ToListAsync();
            return employeeLeaveViewModel;
        }

        /// <summary>
        /// Logic to get employee and reporting person remainig leave details by particular empId
        /// </summary>   
        /// <param name="empId" ></param>
        public async Task<List<EmployeeLeaveViewModel>> GetEmployeeLeaveByEmpId(int empId, int companyId)
        {
            var employeeLeave = await (from employees in _dbContext.Employees
                                       where !employees.IsDeleted && (_dbContext.ReportingPersonsEntities.Where(r => r.ReportingPersonEmpId == empId).Select(r => r.EmployeeId).Contains(employees.EmpId))
                                       || employees.EmpId == empId && employees.CompanyId == companyId
                                       select new EmployeeLeaveViewModel()
                                       {
                                           EmpId = employees.EmpId,
                                           EmployeeName = employees.FirstName + " " + employees.LastName,
                                           UserName = employees.UserName,
                                           LeavesType = _dbContext.leaveTypes.Where(l => !l.IsDeleted && l.IsActive && l.LeaveTypeId != (int)LeavetypeStatus.Permission).ToList(),
                                           AllLeaveDetail = _dbContext.AllLeaveDetails.Where(a => a.EmpId == employees.EmpId && a.CompanyId == companyId && a.LeaveYear == DateTime.Now.Year).ToList(),
                                           EmployeeAppliedLeave = _dbContext.employeeAppliedLeaveEntities.Where(x => x.EmpId == employees.EmpId && !x.IsDeleted && x.CompanyId == companyId && x.CreatedDate.Year == DateTime.Now.Year).ToList(),
                                       }).ToListAsync();
            return employeeLeave;
        }


        /// <summary>
              /// Logic to get Allleavedetails count of all employees and reporting person
              /// </summary> 
        /// <param name="empId, pager" ></param>
        public async Task<int> GetAllLeaveSummaryCount(SysDataTablePager pager, int empId,int companyId)
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

                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                List<EmployeesDataCount> allLeaveSummaryCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetLeaveDetailsFilterCount] @empId, @companyId,@searchText", param, param1, param2).ToListAsync();
                foreach (var item in allLeaveSummaryCounts)
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
              /// Logic to get Allleavedetails count of employees
              /// </summary> 
        /// <param name="empId, pager" ></param>
        public async Task<int> GetEmployeesLeaveDetailsCount(SysDataTablePager pager, int empId,int companyId)
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

                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                List<EmployeesDataCount> empLeaveCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetEmployeesLeaveDetailsCount] @empId, @companyId, @searchText", param, param1, param2).ToListAsync();
                foreach (var item in empLeaveCounts)
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
    }
}
