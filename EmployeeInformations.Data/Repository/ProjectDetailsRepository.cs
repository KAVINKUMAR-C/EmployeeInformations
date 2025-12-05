using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ProjectSummaryViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace EmployeeInformations.Data.Repository
{
    public class ProjectDetailsRepository : IProjectDetailsRepository
    {
        private readonly EmployeesDbContext _dbContext;
        public ProjectDetailsRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// project Details 

        /// <summary>
        /// Logic to get projectdetails list
        /// </summary>         
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectDetailsEntity>> GetAllProjectDetails(int companyId)
        {
            return await _dbContext.ProjectDetails.Where(d => !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<ProjectDetails>> GetAllProjectDetail(int companyId)
        {
            var projectDetails = await (from projectDetail in _dbContext.ProjectDetails

                                        join projectType in _dbContext.ProjectTypes on projectDetail.ProjectTypeId equals projectType.ProjectTypeId

                                        join client in _dbContext.Client on projectDetail.ClientCompanyId equals client.ClientId

                                        where !projectDetail.IsDeleted && projectDetail.CompanyId == companyId && !projectType.IsDeleted && !client.IsDeleted && projectType.CompanyId == companyId && client.CompanyId == projectDetail.CompanyId
                                        select new ProjectDetails()
                                        {
                                            ProjectId = projectDetail.ProjectId,
                                            ProjectName = projectDetail.ProjectName,
                                            ProjectTypeId = projectDetail.ProjectTypeId,
                                            ProjectDescription = projectDetail.ProjectDescription,
                                            EmpId = projectDetail.EmpId,
                                            Hours = projectDetail.Hours,
                                            Technology = projectDetail.Technology,
                                            Startdate = projectDetail.Startdate,
                                            Enddate = projectDetail.Enddate,
                                            ProjectCost = projectDetail.ProjectCost,
                                            ProjectRefNumber = projectDetail.ProjectRefNumber,
                                            ClientCompanyId = projectDetail.ClientCompanyId,
                                            ProjectTypeName = projectType.ProjectTypeName,
                                            ClientCompanyName = client.ClientCompany,
                                            TechnologyName = projectDetail.Technology,
                                            CurrencyCode = projectDetail.CurrencyCode,
                                            CreatedDate = projectDetail.CreatedDate,
                                        }).OrderByDescending(x => x.CreatedDate).ToListAsync();
            return projectDetails;


        }

        /// <summary>
        /// Logic to get projectdetails list
        /// </summary>         
        /// <param name="pager, columnName, columnDirection" ></param>
        public async Task<List<ProjectDetailsModel>> GetProjectDetail(SysDataTablePager pager, string columnName, string columnDirection,int companyId)
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
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param3 = new NpgsqlParameter("@offsetValue", pager.sEcho);
                var param4 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
                var param5 = new NpgsqlParameter("@sorting", _params.Sorting);

                var allProjectDetails = await _dbContext.projectDetailsEntities.FromSqlRaw("EXEC [dbo].[spGetProjectDetailsFilterList] @companyId, @pagingSize, @offsetValue, @searchText, @sorting", param1, param2, param3, param4, param5).ToListAsync();
                return allProjectDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Logic to get create and update the projectdetails
        /// </summary> 
        /// <param name="projectDetailsEntity" ></param>         
        public async Task<int> CreateProject(ProjectDetailsEntity projectDetailsEntity,int companyId)
        {
            var result = 0;
            try
            {
                if (projectDetailsEntity?.ProjectId == 0)
                {
                    projectDetailsEntity.CompanyId = companyId;
                    await _dbContext.ProjectDetails.AddAsync(projectDetailsEntity);
                    await _dbContext.SaveChangesAsync();
                    result = projectDetailsEntity.ProjectId;
                }
                else
                {
                    if (projectDetailsEntity != null)
                    {
                        projectDetailsEntity.CompanyId = companyId;
                        _dbContext.ProjectDetails.Update(projectDetailsEntity);
                        await _dbContext.SaveChangesAsync();
                        result = projectDetailsEntity.ProjectId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }


        /// <summary>
        /// Logic to get ProjectId the projectdetails detail by particular projectdetails
        /// </summary> 
        /// <param name="ProjectId" ></param>        
        /// <param name="CompanyId" ></param>        
        public async Task<bool> DeleteProject(int ProjectId, int companyId)
        {
            var result = false;
            var projectDetailsEntity = await _dbContext.ProjectDetails.FirstOrDefaultAsync(d => d.ProjectId == ProjectId && d.CompanyId == companyId);
            if (projectDetailsEntity != null)
            {
                projectDetailsEntity.IsDeleted = true;
                _dbContext.ProjectDetails.Update(projectDetailsEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get rfpn projectdetails list
        /// </summary>         
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<int> GetProjectMaxCount(int companyId)
        {
            var count = await _dbContext.ProjectDetails.Where(d => !d.IsDeleted && d.CompanyId == companyId).CountAsync();
            return count;
        }


        /// <summary>
        /// Logic to get empId the projectdetails detail by particular projectdetails
        /// </summary>   
        /// <param name="empId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectDetailsEntity>> GetAllProjectDetailsId(int empId, int companyId)
        {
            var result = new List<ProjectDetailsEntity>();
            if (empId == 0)
            {
                result = await _dbContext.ProjectDetails.Where(x => !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
            else
            {
                result = await _dbContext.ProjectDetails.Where(x => x.EmpId == empId && !x.IsDeleted && x.CompanyId == companyId).ToListAsync();
            }
            return result;
        }


        /// <summary>
        /// Logic to get projectId the projectdetails detail by particular projectdetails
        /// </summary>   
        /// <param name="projectId" ></param>
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<ProjectDetailsEntity> GetByProjectId(int ProjectId, int companyId)
        {
            var projectDetailsEntity = await _dbContext.ProjectDetails.AsNoTracking().FirstOrDefaultAsync(d => d.ProjectId == ProjectId && !d.IsDeleted && d.CompanyId == companyId);
            return projectDetailsEntity ?? new ProjectDetailsEntity();
        }

        //// project Type

        /// <summary>
           /// Logic to get projecttype list
         /// </summary>          
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectTypesEntity>> GetAllProjectTypes(int companyId)
        {
            return await _dbContext.ProjectTypes.Where(d => !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get projecttypeid the projecttype detail by particular projectdetails
        /// </summary>          
        /// <param name="projecttypeid" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<ProjectTypesEntity> GetByProjectType(int ProjectTypeId, int companyId)
        {
            var projectTypesEntity = await _dbContext.ProjectTypes.FirstOrDefaultAsync(d => d.ProjectTypeId == ProjectTypeId && d.CompanyId == companyId);
            return projectTypesEntity ?? new ProjectTypesEntity();
        }


        /// <summary>
        /// Logic to get client list
        /// </summary>          
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<ClientEntity>> GetAllClientCompany(int companyId)
        {
            var result = await _dbContext.Client.Where(u => !u.IsDeleted && u.CompanyId == companyId).ToListAsync();
            return result;
        }


        /// <summary>
        /// Logic to get skillset list
        /// </summary>          
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<SkillSetEntity>> GetAllSkillSet(int companyId)
        {
            var result = await _dbContext.SkillSets.Where(d => !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
            return result;
        }


        /// <summary>
        /// Logic to get employees list
        /// </summary>          
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<EmployeesEntity>> GetAllEmployees(int companyId)
        {
            return await _dbContext.Employees.Where(d => !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get employeeIds the reportingpersonsentities detail by particular employee
        /// </summary> 
        /// <param name="employeeIds" ></param> 
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        /// <param name="ReportingPersonEmpId" ></param>
        public async Task<List<int>> GetByEmployeeId(int employeeIds, int companyId)
        {
            return await _dbContext.ReportingPersonsEntities.Where(e => e.EmployeeId == employeeIds && e.CompanyId == companyId).Select(b => b.ReportingPersonEmpId).ToListAsync();
        }

        //// project Assignation 

        /// <summary>
        /// Logic to get employeeIds the projectassignation detail by particular employee
        /// </summary> 
        /// <param name="employeeIds" ></param> 
        /// <param name="IsDeleted" ></param>
        /// <param name="CompanyId" ></param>
        public async Task<List<int>> GetByEmployeeIdForProject(int employeeIds, int companyId)
        {
            return await _dbContext.ProjectAssignation.Where(e => e.EmployeeId == employeeIds && e.CompanyId == companyId && !e.IsDeleted).Select(b => b.ProjectId).Distinct().ToListAsync();
        }


        /// <summary>
        /// Logic to get projectAssignation list
        /// </summary> 
        /// <param name="IsDeleted" ></param>        
        /// <param name="CompanyId" ></param> 
        public async Task<List<ProjectAssignationEntity>> GetAllProjectAssignation(int companyId)
        {
            return await _dbContext.ProjectAssignation.Where(d => !d.IsDeleted && d.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get create the projectAssignation detail 
        /// </summary> 
        /// <param name="projectAssignationEntity" ></param> 
        public async Task<bool> CreateProjectAssignation(List<ProjectAssignationEntity> projectAssignationEntity, int companyId)
        {
            var result = false;
            if (projectAssignationEntity.Count() > 0)
            {
                projectAssignationEntity.ForEach(x =>
                {
                    x.CompanyId = companyId;
                });
                await _dbContext.ProjectAssignation.AddRangeAsync(projectAssignationEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get update projectid the projectAssignation detail by particular projectAssignation
        /// </summary> 
        /// <param name="projectAssignationEntity" ></param>
        /// <param name="projectId" ></param>
        public async Task<bool> UpdateProjectAssignation(List<ProjectAssignationEntity> projectAssignationEntity, int projectId, int companyId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.ProjectAssignation.Where(x => x.ProjectId == projectId).ToListAsync();

            projectAssignationEntity.ForEach(x =>
            {
                x.CompanyId = companyId;
            });
            _dbContext.ProjectAssignation.RemoveRange(attachmentsEntitys);
            await _dbContext.SaveChangesAsync();

            await _dbContext.ProjectAssignation.AddRangeAsync(projectAssignationEntity);
            result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }


        /// <summary>
        /// Logic to get delete the projectAssignation detail by particular projectAssignation
        /// </summary> 
        /// <param name="projectAssignationEntity" ></param>      
        public async Task<bool> DeleteProjectAssignation(List<ProjectAssignationEntity> projectAssignationEntity, int companyId)
        {
            var result = false;
            projectAssignationEntity.ForEach(x =>
            {
                x.CompanyId = companyId;
            });

            _dbContext.ProjectAssignation.UpdateRange(projectAssignationEntity);
            result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }


        /// <summary>
        /// Logic to get projectid the projectAssignation detail by particular projectAssignation
        /// </summary> 
        /// <param name="projectid" ></param>  
        /// <param name="IsDeleted" ></param>        
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectAssignationEntity>> GetByProjectAssignationId(int ProjectId, int companyId)
        {
            var projectAssignationEntity = await _dbContext.ProjectAssignation.AsNoTracking().Where(h => h.ProjectId == ProjectId && h.CompanyId == companyId && !h.IsDeleted).ToListAsync();
            return projectAssignationEntity;
        }


        /// <summary>
        /// Logic to get projectid the projectAssignation detail by particular projectAssignation
        /// </summary> 
        /// <param name="empId" ></param>  
        /// <param name="IsDeleted" ></param>        
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectAssignationEntity>> GetByProjectAssignationByEmpId(int empId, int companyId)
        {
            var projectAssignationEntity = await _dbContext.ProjectAssignation.AsNoTracking().Where(h => h.EmployeeId == empId && h.CompanyId == companyId && !h.IsDeleted).ToListAsync();
            return projectAssignationEntity;
        }


        /// <summary>
        /// Logic to get projectid the projectAssignation detail by particular projectAssignation
        /// </summary> 
        /// <param name="projectid" ></param>                
        /// <param name="CompanyId" ></param>
        public async Task<List<ProjectAssignationEntity>> GetEmployeeByProjectId(int projectId, int companyId)
        {
            return await _dbContext.ProjectAssignation.Where(e => e.ProjectId == projectId && e.CompanyId == companyId).ToListAsync();
        }


        /// <summary>
        /// Logic to get projectid count the projectAssignation detail by particular projectAssignation
        /// </summary> 
        /// <param name="projectid" ></param>                
        /// <param name="CompanyId" ></param>
        public async Task<int> GetEmployeeCountByProjectId(int projectId, int companyId)
        {
            return await _dbContext.ProjectAssignation.Where(e => e.ProjectId == projectId && e.CompanyId == companyId).CountAsync();
        }

        /// <summary>
        /// Logic to get projectid and companyId the ProjectDetailsEntity detail by particular projectId and companyId
        /// </summary> 
        /// <param name="projectid" ></param>                
        /// <param name="CompanyId" ></param>
        public async Task<ProjectDetailsEntity> GetProjectByCompnayId(int projectId, int companyId)
        {
            var projectDetailsEntity = await _dbContext.ProjectDetails.AsNoTracking().FirstOrDefaultAsync(d => d.ProjectId == projectId && d.CompanyId == companyId);
            return projectDetailsEntity;
        }

        /// <summary>
        /// Logic to get skillset list
        /// </summary>                  
        public async Task<List<CurrencyEntity>> GetAllCurrency()
        {
            return await _dbContext.CurrencyEntity.ToListAsync();
        }

        /// <summary>
        /// Logic to get all the ProjectDetails details by particular empId 
        /// </summary> 
        /// <param name="empId" ></param>  
        public async Task<List<ProjectNames>> GetProjectByEmpId(int empId,int companyId)
        {
          var projectByEmpId = await(from projectAssignation in _dbContext.ProjectAssignation
                                     join project in _dbContext.ProjectDetails on projectAssignation.ProjectId equals project.ProjectId
                                     where projectAssignation.EmployeeId == empId && !projectAssignation.IsDeleted && companyId == 
                                     projectAssignation.CompanyId && !project.IsDeleted && companyId == project.CompanyId
                                     select new ProjectNames ()
                                     {
                                         ProjectId = project.ProjectId,
                                         ProjectName = project.ProjectName,

                                     }).ToListAsync();
            return projectByEmpId;
        }

        /// <summary>
        /// Logic to get AllProjectdetails count 
        /// </summary> 
        /// <param name="pager" ></param>
        public async Task<int> GetAllProjectsCount(SysDataTablePager pager,int companyId)
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
                List<EmployeesDataCount> allLeaveSummaryCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetProjectDetailsFilterCount] @companyId,@searchText", param1, param2).ToListAsync();
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
    }
}
