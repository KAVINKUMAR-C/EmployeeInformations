using AutoMapper;
using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Business.Utility.Helper;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.APIDashboardModel;
using EmployeeInformations.Model.AttendanceViewModel;
using EmployeeInformations.Model.DashboardViewModel;

namespace EmployeeInformations.Business.API.Service
{
    public class DashboardAPIService : IDashboardAPIService
    {
        private readonly IMapper _mapper;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IProjectDetailsRepository _projectDetailsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMasterRepository _masterRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ICompanyPolicyRepository _companyPolicyRepository;
        private readonly IEmployeeInformationService _employeeInformationService;
        private readonly ICompanyRepository _companyRepository;

        public DashboardAPIService(IMapper mapper, IDashboardRepository dashboardRepository, IEmployeesRepository employeesRepository, IProjectDetailsRepository projectDetailsRepository, IClientRepository clientRepository, ILeaveRepository leaveRepository, IMasterRepository masterRepository, IAttendanceRepository attendanceRepository, ICompanyPolicyRepository companyPolicyRepository, IEmployeeInformationService employeeInformationService, ICompanyRepository companyRepository)
        {
            _mapper = mapper;
            _dashboardRepository = dashboardRepository;
            _employeesRepository = employeesRepository;
            _projectDetailsRepository = projectDetailsRepository;
            _clientRepository = clientRepository;
            _leaveRepository = leaveRepository;
            _masterRepository = masterRepository;
            _attendanceRepository = attendanceRepository;
            _companyPolicyRepository = companyPolicyRepository;
            _employeeInformationService = employeeInformationService;
            _companyRepository = companyRepository;
        }


        /// <summary>
                /// Logic to get dashboardViewModelapi details
               /// </summary>  
        public async Task<DashboardViewModelAPI> GetAllDashboardView(int employeeId, int roleId,int companyId)
        {
            var listOfDashboardViewModel = new DashboardViewModelAPI();
            var RolePrivileges = new List<int>();
            if (roleId >= 0)
            {
                var id = roleId;
                RolePrivileges = await _employeeInformationService.DashboardRolePermissionByRoleId(id);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TotalEmployee))
            {
                listOfDashboardViewModel.totalEmployeeViewAPIRequest = await GetTotalEmployeeDashBorad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.RecentJoiners))
            {
                listOfDashboardViewModel.totalEmployeeViewAPIRequest = await GetTotalEmployeeDashBorad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TotalProjects))
            {
                listOfDashboardViewModel.totalProjectViewAPIRequest = await GetTotalProjectDashBoarad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TotalClients))
            {
                listOfDashboardViewModel.totalClientViewAPIRequest = await GetTotalClientDashBoarad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.Department))
            {
                listOfDashboardViewModel.topFiveDepartmentsViewAPI = await GetTopFiveActiveDepartmentsDashBoarad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.Project))
            {
                listOfDashboardViewModel.topFiveProjectsViewAPI = await GetTotalFiveActiveProjectsDashBoarad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.UpcomingHolidays))
            {
                listOfDashboardViewModel.topFiveLeaveViewAPI = await GetTopFiveLeaveDashBoarad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.UpcomingCelebration))
            {
                listOfDashboardViewModel.topFiveCelebrationViewAPI = await GetTopFiveCelebration(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.AnnouncementDetails))
            {
                listOfDashboardViewModel.topFiveAnnuncementViewAPI = await GetTopFiveAnnuncements(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.LeaveWorkFromHome))
            {
                listOfDashboardViewModel.topFiveLeaveWfhViewAPI = await GetTopFiveLeaveWfh(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.Employee))
            {
                listOfDashboardViewModel.totalEmployeeLeaveViewAPI = await GetTotalEmployeeLeaveDashBorad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TodayPresentEmployees))
            {
                listOfDashboardViewModel.totalEmployeeLeaveViewAPI = await GetTotalEmployeeLeaveDashBorad(companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.LeaveApproved))
            {
                listOfDashboardViewModel.topFiveLeaveTypeViewAPI = await GetEmpLeave(employeeId, companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.LeavePending))
            {
                listOfDashboardViewModel.topFiveLeaveTypeViewAPI = await GetEmpLeave(employeeId, companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.RemainingLeave))
            {
                listOfDashboardViewModel.employeeLeaveCountAPI = await GetEmployeeLeaveCount(employeeId,companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TodayLockTime))
            {
                listOfDashboardViewModel.timeLogViewAPI = await GetTimeLogViewModel(employeeId, companyId);
            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.WorkingHours))
            {
                listOfDashboardViewModel.employeeWorkingHoursViewAPI = await GetEmployeeTotalHoursForWeek(employeeId,companyId);
            }
            listOfDashboardViewModel.topFiveLeaveTypeViewAPI = await GetLeaveApprove(employeeId, companyId);

            return listOfDashboardViewModel;
        }

        /// <summary>
                /// Logic to get totalemployeeviewapirequest list of home dashboard
               /// </summary>  
        public async Task<TotalEmployeeViewAPIRequest> GetTotalEmployeeDashBorad(int companyId)
        {
            var totalEmployeeView = new TotalEmployeeViewAPIRequest();
            var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfileIsDeleted(companyId);
            totalEmployeeView.TotalEmployeeCount = await _employeesRepository.GetEmployeeMaxCount(companyId);
            totalEmployeeView.TotalEmployeeCountPercentage = GetemployeeJoingPercentage(totalEmployeeView.TotalEmployeeCount, allEmployeeProfiles);
            var listOfEmpString = GetEmployeeJoiningByMonthInCount(allEmployeeProfiles);
            totalEmployeeView.Months = Convert.ToString(listOfEmpString[0]);
            totalEmployeeView.EmployeeByMonthCount = Convert.ToString(listOfEmpString[1]);
            totalEmployeeView.Employees = await GetAllEmployeesTopFive(companyId);
            return totalEmployeeView;
        }

        /// <summary>
                /// Logic to get profile 
               /// </summary>
        /// <param name="empTotalCount" ></param>
        /// <param name="allEmployeeProfiles" ></param>
        public string GetemployeeJoingPercentage(int empTotalCount, List<ProfileInfoEntity> allEmployeeProfiles)
        {
            var currenDate = DateTime.Now;
            var lastYear = currenDate.AddYears(-1);
            var fromDate = new DateTime(lastYear.Year, lastYear.Month, 1);
            var toDate = currenDate;
            var joingEmpCountInCurrentMonth = allEmployeeProfiles.Where(x => x.DateOfJoining >= fromDate && x.DateOfJoining <= toDate).Count();
            return joingEmpCountInCurrentMonth.ToString();
        }

        /// <summary>
                /// Logic to get profileinfo 
               /// </summary>
        /// <param name="allEmployeeProfiles" ></param>

        public List<string> GetEmployeeJoiningByMonthInCount(List<ProfileInfoEntity> allEmployeeProfiles)
        {
            var stringLastTweleMonths = string.Empty;
            var stringLastTweleMonthEmpCount = string.Empty;
            var listOfEmpString = new List<string>();
            var currentDate = DateTime.Now;
            List<DateTime> last12 = (from r in Enumerable.Range(0, 12) select currentDate.AddMonths(0 - r)).OrderBy(r => r).ToList();
            last12.ForEach(x =>
            {
                stringLastTweleMonths += $"{x:MMM}" + " " + x.Year + ",";

                var startDate = new DateTime(x.Year, x.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var empCountOfMonth = allEmployeeProfiles.Where(x => x.DateOfJoining >= startDate && x.DateOfJoining <= endDate).Count();

                stringLastTweleMonthEmpCount += empCountOfMonth + ",";
            });
            listOfEmpString.Add(stringLastTweleMonths.Trim(new char[] { ',' }));
            listOfEmpString.Add(stringLastTweleMonthEmpCount.Trim(new char[] { ',' }));
            return listOfEmpString;
        }

        /// <summary>
                /// Logic to get employeestopfive list
               /// </summary>  
        public async Task<List<EmployeesListAPI>> GetAllEmployeesTopFive(int companyId)
        {
            var listOfEmployees = new List<EmployeesListAPI>();
            var listOfEmployee = new TotalEmployeeViewAPIRequest();
            listOfEmployee.Employees = new List<EmployeesListAPI>();
            var listEmployee = await _employeesRepository.GetAllEmployees(companyId);
            var allDesignation = await _employeesRepository.GetAllDesignation(companyId);
            var allDepartment = await _employeesRepository.GetAllDepartment(companyId);
            var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var listE = listEmployee.OrderByDescending(x => x.CreatedDate).Take(5).ToList();
            int count = 1;
            foreach (var employee in listE)
            {
                count = count > 5 ? 1 : count;
                var profileByEmpId = allEmployeeProfiles.FirstOrDefault(x => x.EmpId == employee.EmpId);
                var clas = Common.Common.GetClassNameForGrid(count);
                var designationName = allDesignation.FirstOrDefault(x => x.DesignationId == employee.DesignationId);
                var departmentName = allDepartment.FirstOrDefault(y => y.DepartmentId == employee.DepartmentId);
                if (profileByEmpId != null)
                {
                    listOfEmployee.Employees.Add(new EmployeesListAPI()
                    {
                        UserName = employee.UserName,
                        EmpId = employee.EmpId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        EmployeeFullName = employee.FirstName + " " + employee.LastName,
                        DepartmentId = employee.DepartmentId,
                        DesignationId = employee.DesignationId,
                        RoleId = (Role)employee.RoleId,
                        OfficeEmail = employee.OfficeEmail,
                        CreatedDate = employee.CreatedDate,
                        JoingDate = profileByEmpId != null ? string.IsNullOrEmpty(Convert.ToString(profileByEmpId.DateOfJoining)) ? null : profileByEmpId.DateOfJoining : null,
                        EmployeeProfileImage = profileByEmpId != null ? string.IsNullOrEmpty(profileByEmpId.ProfileName) ? string.Empty : profileByEmpId.ProfileName : string.Empty,
                        EmployeeSortName = Common.Common.GetEmployeeSortName(employee.FirstName, employee.LastName),
                        DesignationName = designationName != null ? designationName.DesignationName : string.Empty,
                        DepartmentName = departmentName != null ? departmentName.DepartmentName : string.Empty,
                        ClassName = clas,
                    });
                    count++;
                }
            }
            return listOfEmployee.Employees;
        }


        /// <summary>
                /// Logic to get totalprojectviewapirequest list of api home and employee dashboard
               /// </summary>
        public async Task<TotalProjectViewAPIRequest> GetTotalProjectDashBoarad(int companyId)
        {
            var totalProjectView = new TotalProjectViewAPIRequest();
            var allProjectDetails = await _projectDetailsRepository.GetAllProjectDetails(companyId);
            totalProjectView.TotalProjectCount = await _projectDetailsRepository.GetProjectMaxCount(companyId);
            totalProjectView.TotalProjectCountPercentage = GetProjectPercentage(totalProjectView.TotalProjectCount, allProjectDetails);
            var listOfProjectString = GetProjectByMonthInCount(allProjectDetails);
            totalProjectView.ProjectMonths = Convert.ToString(listOfProjectString[0]);
            totalProjectView.ProjectByMonthCount = Convert.ToString(listOfProjectString[1]);
            return totalProjectView;
        }

        /// <summary>
                /// Logic to get project details 
               /// </summary>
        /// <param name="projectTotalCount" ></param>
        /// <param name="allProjectDetails" ></param>
        public string GetProjectPercentage(int projectTotalCount, List<ProjectDetailsEntity> allProjectDetails)
        {
            var currenDate = DateTime.Now;
            var lastYear = currenDate.AddYears(-1);
            var fromDate = new DateTime(lastYear.Year, lastYear.Month, 1);
            var toDate = currenDate;
            var createdProjectCountInCurrentMonth = allProjectDetails.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate).Count();
            return createdProjectCountInCurrentMonth.ToString();
        }

        /// <summary>
                /// Logic to get project list
               /// </summary>
        /// <param name="allProjectDetails" ></param>
        public List<string> GetProjectByMonthInCount(List<ProjectDetailsEntity> allProjectDetails)
        {
            var stringLastTweleMonths = string.Empty;
            var stringLastTweleMonthProjectCount = string.Empty;
            var listOfProjectString = new List<string>();
            var currentDate = DateTime.Now;
            List<DateTime> last12 = (from r in Enumerable.Range(0, 12) select currentDate.AddMonths(0 - r)).OrderBy(r => r).ToList();
            last12.ForEach(x =>
            {
                stringLastTweleMonths += $"{x:MMM}" + " " + x.Year + ",";

                var startDate = new DateTime(x.Year, x.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var projectCountOfMonth = allProjectDetails.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).Count();

                stringLastTweleMonthProjectCount += projectCountOfMonth + ",";
            });
            listOfProjectString.Add(stringLastTweleMonths.Trim(new char[] { ',' }));
            listOfProjectString.Add(stringLastTweleMonthProjectCount.Trim(new char[] { ',' }));

            return listOfProjectString;
        }

        /// <summary>
                /// Logic to get totalclientviewapirequest list of home api dashboard
               /// </summary>
        public async Task<TotalClientViewAPIRequest> GetTotalClientDashBoarad(int companyId)
        {
            var totalClientView = new TotalClientViewAPIRequest();
            var allClientDetails = await _clientRepository.GetAllClient(companyId);
            totalClientView.TotalClientCount = await _clientRepository.GetClientMaxCount(companyId);
            totalClientView.TotalClientCountPercentage = GetClientPercentage(totalClientView.TotalClientCount, allClientDetails);
            var listOfClientString = GetClientByMonthInCount(allClientDetails);
            totalClientView.ClientMonths = Convert.ToString(listOfClientString[0]);
            totalClientView.ClientByMonthCount = Convert.ToString(listOfClientString[1]);
            return totalClientView;
        }

        /// <summary>
                /// Logic to get client 
               /// </summary>
        /// <param name="clientTotalCount" ></param>
        /// <param name="allClientDetails" ></param>
        public string GetClientPercentage(int clientTotalCount, List<ClientEntity> allClientDetails)
        {
            //var totalEmpCount = await _employeesRepository.GetEmployeeMaxCount();            
            var currenDate = DateTime.Now;
            var lastYear = currenDate.AddYears(-1);
            var fromDate = new DateTime(lastYear.Year, lastYear.Month, 1);
            var toDate = currenDate;
            var createdClientCountInCurrentMonth = allClientDetails.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate).Count();
            return createdClientCountInCurrentMonth.ToString();
        }

        /// <summary>
                /// Logic to get client list
               /// </summary>
        /// <param name="allClientDetails" ></param>
        public List<string> GetClientByMonthInCount(List<ClientEntity> allClientDetails)
        {
            var stringLastTweleMonths = string.Empty;
            var stringLastTweleMonthProjectCount = string.Empty;
            var listOfClientString = new List<string>();
            var currentDate = DateTime.Now;
            List<DateTime> last12 = (from r in Enumerable.Range(0, 12) select currentDate.AddMonths(0 - r)).OrderBy(r => r).ToList();
            last12.ForEach(x =>
            {
                stringLastTweleMonths += $"{x:MMM}" + " " + x.Year + ",";
                var startDate = new DateTime(x.Year, x.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var projectCountOfMonth = allClientDetails.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).Count();

                stringLastTweleMonthProjectCount += projectCountOfMonth + ",";
            });
            listOfClientString.Add(stringLastTweleMonths.Trim(new char[] { ',' }));
            listOfClientString.Add(stringLastTweleMonthProjectCount.Trim(new char[] { ',' }));

            return listOfClientString;
        }

        /// <summary>
                /// Logic to get topfive departmentviewAPI list of home api dashboard
               /// </summary>
        public async Task<TopFiveDepartmentsViewAPI> GetTopFiveActiveDepartmentsDashBoarad(int companyId)
        {
            var topFiveDepartmentsView = new TopFiveDepartmentsViewAPI();
            var listofEmployees = await _employeesRepository.GetAllEmployeeDetails(companyId);
            topFiveDepartmentsView.departmentListAPI = await GetTopFiveDepartments(listofEmployees,companyId);
            return topFiveDepartmentsView;
        }

        /// <summary>
                /// Logic to get department list 
               /// </summary>
        /// <param name="employeesEntity" ></param>
        public async Task<List<DepartmentListAPI>> GetTopFiveDepartments(List<EmployeesEntity> employeesEntity,int companyId)
        {
            var topfive = employeesEntity.GroupBy(x => x.DepartmentId).ToList();
            var departmentLists = new List<DepartmentListAPI>();
            var topFivedepartment = employeesEntity.Take(4).ToList();
            var result = employeesEntity.GroupBy(p => new { p.DepartmentId }).Select(g => new
            {
                DepartmentId = g.Key,
                DepartmentCount = g.Count()

            }).OrderByDescending(x => x.DepartmentCount).Take(4).ToList();

            var count = 1;
            foreach (var item in result)
            {
                int deppartmentId = Convert.ToInt32(item.DepartmentId.DepartmentId);
                if (deppartmentId > 0)
                {
                    var getDepname = await _employeesRepository.GetDepartmentNameByDepartmentId(deppartmentId, companyId);
                    var departmentList = new DepartmentListAPI();
                    departmentList.DepartmentName = getDepname;
                    departmentList.DepartmentCount = item.DepartmentCount;
                    departmentList.DepartmentColor = Common.Common.GetClassNameForDepartmentDashboard(count);
                    departmentLists.Add(departmentList);
                    count++;
                }

            }

            return departmentLists;
        }

        /// <summary>
                /// Logic to get topfiveprojectviewapi list of home api dashboard
               /// </summary>
        public async Task<TopFiveProjectsViewAPI> GetTotalFiveActiveProjectsDashBoarad(int companyId)
        {
            var topFiveProjectsView = new TopFiveProjectsViewAPI();
            var projects = await _dashboardRepository.GetAllProjectDetails(companyId);
            topFiveProjectsView.ProjectLists = GetTopFiveProjects(projects);
            var strProjectListCount = await GetStringProjectNames(projects, companyId);
            topFiveProjectsView.StringProjectName = Convert.ToString(strProjectListCount[0]);
            topFiveProjectsView.ProjectByEmployeeCount = Convert.ToString(strProjectListCount[1]);
            return topFiveProjectsView;
        }

        /// <summary>
                /// Logic to get project list api 
               /// </summary>
        /// <param name="topFiveProject" ></param>
        public List<ProjectListAPI> GetTopFiveProjects(List<ProjectDetailsEntity> topFiveProject)
        {
            var projectLists = new List<ProjectListAPI>();
            var count = 1;
            foreach (var item in topFiveProject)
            {
                var project = new ProjectListAPI();
                project.ProjectName = item.ProjectName;
                project.ProjectColor = Common.Common.GetClassNameForProjectDashboard(count);
                projectLists.Add(project);
                count++;
            }
            return projectLists;
        }

        /// <summary>
                /// Logic to get projectdetails list api
               /// </summary>
        /// <param name="topFiveProject" ></param>
        public async Task<List<string>> GetStringProjectNames(List<ProjectDetailsEntity> topFiveProject,int companyId)
        {
            var listOfproject = new List<string>();
            var strProjectName = string.Empty;
            var strEmpCountByProject = string.Empty;
            foreach (var item in topFiveProject)
            {
                var totalEmployeeCount = await _projectDetailsRepository.GetEmployeeCountByProjectId(item.ProjectId, companyId);
                strProjectName += item.ProjectName + ",";
                var empCount = totalEmployeeCount;
                strEmpCountByProject += empCount + ",";
            }
            listOfproject.Add(strProjectName.Trim(new char[] { ',' }));
            listOfproject.Add(strEmpCountByProject.Trim(new char[] { ',' }));
            return listOfproject;
        }

        /// <summary>
                /// Logic to get topfive leave view api list of home and employee api dashboard
               /// </summary>
        public async Task<TopFiveLeaveViewAPI> GetTopFiveLeaveDashBoarad(int companyId)
        {
            var topFiveLeaveView = new TopFiveLeaveViewAPI();
            var listofLeave = await _leaveRepository.GetAllEmployeeHolidays(companyId);
            topFiveLeaveView.leaveListAPIs = GetTopFiveLeaves(listofLeave);
            return topFiveLeaveView;
        }

        /// <summary>
                /// Logic to get leave api list 
               /// </summary>
        /// <param name="employeeHolidaysEntities" ></param>
        public List<LeaveListAPI> GetTopFiveLeaves(List<EmployeeHolidaysEntity> employeeHolidaysEntities)
        {
            var currentDate = DateTime.Now;
            var topfive = employeeHolidaysEntities.Where(x => x.HolidayDate > currentDate).ToList();
            var leaveLists = new List<LeaveListAPI>();
            var topFivedepartment = topfive.Take(5).ToList();
            var count = 1;
            foreach (var item in topFivedepartment)
            {
                var leaveList = new LeaveListAPI();
                leaveList.LeaveDate = item.HolidayDate;
                leaveList.LeaveDay = Convert.ToString(Convert.ToDateTime(item.HolidayDate).DayOfWeek);
                leaveList.Title = item.Title;
                leaveList.leaveColor = Common.Common.GetClassNameForLeaveDashboard(count);
                leaveLists.Add(leaveList);
                count++;
            }
            return leaveLists;
        }

        /// <summary>
                /// Logic to get topfive celebration view list of home and employee api dashboard
               /// </summary>
        public async Task<TopFiveCelebrationViewAPI> GetTopFiveCelebration(int companyId)
        {
            var CelebrationView = new TopFiveCelebrationViewAPI();
            var listOfEmployeeProfile = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var employees = await _employeesRepository.GetAllEmployees(companyId);
            CelebrationView.Celebration = await GetTopFiveCelebration(listOfEmployeeProfile, employees);
            return CelebrationView;
        }

        /// <summary>
                /// Logic to get celebration api list 
               /// </summary>
        /// <param name="profileInfoEntities" ></param>
        /// <param name="employeesEntity" ></param>
        public async Task<List<CelebrationAPI>> GetTopFiveCelebration(List<ProfileInfoEntity> profileInfoEntities, List<EmployeesEntity> employeesEntity)
        {
            var currentDate = DateTime.Now;
            var topFiveCelebrationList = profileInfoEntities.Where(x => x.DateOfBirth.Month >= currentDate.Month && x.DateOfBirth.DayOfYear >= currentDate.DayOfYear && !x.IsDeleted).OrderBy(x => x.DateOfBirth.Month).ThenBy(x => x.DateOfBirth.DayOfYear).ToList();
            var topFiveCelebrations = employeesEntity.ToList();
            var celebrationLists = new List<CelebrationAPI>();
            var count = 1;
            topFiveCelebrationList = topFiveCelebrationList.Take(5).ToList();
            foreach (var item in topFiveCelebrationList)
            {
                var celebration = new CelebrationAPI();
                celebration.CelebrationDate = Convert.ToDateTime(item.DateOfBirth).ToString("dd-MMM");
                celebration.CelebrationColor = Common.Common.GetClassNameForLeaveDashboard(count);
                celebration.CelebrationName = Common.Constant.Birthday;
                var name = topFiveCelebrations.FirstOrDefault(x => x.EmpId == item.EmpId);
                celebration.EmployeeName = name != null ? name.FirstName + " " + name.LastName : "";
                celebrationLists.Add(celebration);
                count++;
            }

            return celebrationLists;
        }

        /// <summary>
                /// Logic to get topfive annuncement view list of home and employee dashboard
               /// </summary>
        public async Task<TopFiveAnnuncementViewAPI> GetTopFiveAnnuncements(int companyId)
        {
            try
            {
                var typeView = new TopFiveAnnuncementViewAPI();
                var listOfAnnuncement = await _masterRepository.GetAllAnnouncementactive(companyId);
                typeView.Announcement = await GetTopFiveAnnuncement(listOfAnnuncement, companyId);
                return typeView;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
                /// Logic to get announcementapi list 
               /// </summary>
        /// <param name="announcementEntities" ></param>

        public async Task<List<AnnouncementsAPI>> GetTopFiveAnnuncement(List<AnnouncementEntity> announcementEntities,int companyId)
        {
            var AnnouncementLists = new List<AnnouncementsAPI>();
            var currentDate = DateTime.Now;
            var topFiveLeave = announcementEntities.Where(g => !g.IsDeleted).OrderByDescending(y => y.CreatedDate).Take(5).ToList();
            var count = 1;
            foreach (var item in topFiveLeave)
            {
                var employee = await _employeesRepository.GetEmployeeById(item.CreatedBy, companyId);
                var AnnouncementList = new AnnouncementsAPI();
                AnnouncementList.AnnuncementId = item.AnnouncementId;
                AnnouncementList.AnnouncementName = item.AnnouncementName;
                AnnouncementList.Description = item.Description;
                AnnouncementList.CreatedDate = item.CreatedDate;
                AnnouncementList.AnnouncementColor = Common.Common.GetClassNameForLeaveDashboard(count);
                AnnouncementList.AnnouncerName = employee != null ? employee.FirstName + " " + employee.LastName : "";
                AnnouncementList.AnnouncementDate = item.AnnouncementDate.ToString(Constant.DateFormat);
                AnnouncementList.CreatedBy = item.CreatedBy;
                AnnouncementLists.Add(AnnouncementList);
                count++;
            }
            return AnnouncementLists;
        }

        /// <summary>
                /// Logic to get topfive leavewfh api view list of home and employee api dashboard
               /// </summary>
        public async Task<TopFiveLeaveWfhViewAPI> GetTopFiveLeaveWfh(int companyId)
        {
            var typeView = new TopFiveLeaveWfhViewAPI();
            var listOfLeave = await _leaveRepository.GetAllLeaveDashboard(companyId);
            var emp = await _employeesRepository.GetAllEmployees(companyId);
            var leave = await _leaveRepository.GetAllLeave();
            typeView.Leave = await GetTopFiveLeaveWfh(listOfLeave, emp, leave, companyId);
            return typeView;
        }

        /// <summary>
                /// Logic to get leave list 
               /// </summary>
        /// <param name="employeeAppliedLeaveEntities" ></param>
        /// <param name="employeesEntities" ></param>
        /// <param name="leaveTypesEntities" ></param>
        public async Task<List<LeaveAPI>> GetTopFiveLeaveWfh(List<EmployeeAppliedLeaveEntity> employeeAppliedLeaveEntities, List<EmployeesEntity> employeesEntities, List<LeaveTypesEntity> leaveTypesEntities,int companyId)
        {
            var currentDate = DateTime.Now;
            var listLeaveWfh = employeeAppliedLeaveEntities.Where(j => j.LeaveFromDate.Date == currentDate.Date && j.IsApproved == 1).ToList();
            var topFiveLeaveWfhEmployees = employeesEntities.ToList();
            var topFiveLeaveType = leaveTypesEntities.ToList();
            var LeaveLists = new List<LeaveAPI>();
            var topFiveLeaveWfh = listLeaveWfh.ToList();
            var count = 1;
            foreach (var item in topFiveLeaveWfh)
            {
                var LeaveList = new LeaveAPI();

                LeaveList.LeaveDate = item.LeaveFromDate;
                LeaveList.LeaveColor = Common.Common.GetClassNameForLeaveDashboard(count);
                LeaveList.LeaveType = Convert.ToString(item.AppliedLeaveTypeId);
                LeaveList.LeaveApproved = item.IsApproved;
                foreach (var employee in topFiveLeaveWfhEmployees)
                {
                    var name = await _employeesRepository.GetEmployeeByIdView(item.EmpId,companyId);
                    LeaveList.EmployeeName = name == null ? "" : name.FirstName + " " + name.LastName;
                }
                foreach (var items in topFiveLeaveType)
                {
                    var leavetype = await _leaveRepository.GetAllLeaveDetailsDashboard(item.AppliedLeaveTypeId,companyId);
                    LeaveList.LeaveType = leavetype.LeaveType;
                }

                LeaveLists.Add(LeaveList);
                count++;
            }
            return LeaveLists;
        }

        /// <summary>
                /// Logic to get totalemployeeleaveviewapi list of home and employee api dashboard
               /// </summary>
        public async Task<TotalEmployeeLeaveViewAPI> GetTotalEmployeeLeaveDashBorad(int companyId)
        {
            var totalEmployeeView = new TotalEmployeeLeaveViewAPI();
            totalEmployeeView.TotalEmployeeLeaves = new List<TotalEmployeeLeavesAPI>();
            var EndDate = DateTime.Now.ToString("dd/MM/yyyy");
            var StartDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
            var dFrom = string.IsNullOrEmpty(StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(StartDate).ToString("dd/MM/yyyy");
            var dTo = string.IsNullOrEmpty(EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(EndDate).ToString("dd/MM/yyyy");
            var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
            var flterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
            List<DateTime> allDates = new List<DateTime>();

            for (DateTime date = flterDate; date <= flterEndDate; date = date.AddDays(1))

                if (DayOfWeek.Saturday != date.DayOfWeek && DayOfWeek.Sunday != date.DayOfWeek)
                {
                    allDates.Add(date);
                }

            var empId = "0";
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@empId", empId),
                    new KeyValuePair<string, string>("@startDate",dFrom),
                    new KeyValuePair<string, string>("@endDate",dTo),
                };
            var getEmployeeAttendanceCount = await _attendanceRepository.GetAllEmployeesByAttendaceFilter("spGetAttendanceByEmployeeFilterData", p);
            var present = 0;
            var absent = 0;
            var getProfileList = await _employeesRepository.GetAllEmployeeProfile(companyId);
            var getAllEmployeeList = await _employeesRepository.GetAllEmployees(companyId);
            foreach (DateTime date in allDates)
            {
                var getProfileCount = getProfileList.Where(x => x.DateOfJoining <= date.Date).ToList();
                var strdate = date.ToString("yyyy-MM-dd");
                var list = getEmployeeAttendanceCount.Where(x => x.LogDate == strdate).GroupBy(x => x.EmployeeId).ToList();
                List<int> first = new List<int>();
                List<int> second = new List<int>();

                foreach (var item in getProfileCount)
                {
                    var esslId = getAllEmployeeList.FirstOrDefault(x => x.EmpId == item.EmpId);
                    if (esslId != null)
                    {
                        first.Add(Convert.ToInt32(esslId.EsslId));
                    }

                }

                foreach (var esslId in list)
                {
                    var id = Convert.ToInt32(esslId.Key);
                    var Getessldata = getAllEmployeeList.FirstOrDefault(x => x.EsslId == id && x.IsActive == true && !x.IsDeleted);
                    if (Getessldata != null)
                    {
                        second.Add(Convert.ToInt32(Getessldata.EsslId));
                    }
                }

                IEnumerable<int> firstDiffSecond = first.Except(second);
                IEnumerable<int> secondDiffFirst = second.Except(first);
                present = first.Count() - firstDiffSecond.Count();
                absent = firstDiffSecond.Count();
                totalEmployeeView.TotalEmployeeLeaves.Add(new TotalEmployeeLeavesAPI()
                {
                    Date = date,
                    TotalEmployeePresentCount = present,
                    TotalEmployeeAbsentCount = absent,
                });
            };

            var getCount = getProfileList.Where(x => x.DateOfJoining <= DateTime.Now.Date).ToList();
            var listOfEmpString = GetEmployeeLeaveByMonthInCount(totalEmployeeView);
            totalEmployeeView.StartDate = allDates.FirstOrDefault();
            totalEmployeeView.EndDate = allDates.LastOrDefault();
            totalEmployeeView.Months = Convert.ToString(listOfEmpString[0]);
            totalEmployeeView.TotalEmployeePresentCount = Convert.ToString(listOfEmpString[1]);
            totalEmployeeView.TotalEmployeeAbsentCount = Convert.ToString(listOfEmpString[2]);
            var split = totalEmployeeView.TotalEmployeePresentCount.Split(",");
            totalEmployeeView.TodayPresentEmployeeCount = Convert.ToInt32(split.LastOrDefault());
            totalEmployeeView.TotalEmployeeCount = getCount.Count();
            return totalEmployeeView;
        }

        /// <summary>
                /// Logic to get employeeLeavecount list
               /// </summary>
        /// <param name="totalEmployeeLeaveView" ></param>
        public List<string> GetEmployeeLeaveByMonthInCount(TotalEmployeeLeaveViewAPI totalEmployeeLeaveView)
        {
            var stringLastMonths = string.Empty;
            var stringPresentCount = string.Empty;
            var stringAbsentCount = string.Empty;
            var listOfEmpString = new List<string>();
            var currentDate = DateTime.Now;

            totalEmployeeLeaveView.TotalEmployeeLeaves.ForEach(x =>
            {
                stringLastMonths += $"{x.Date.ToString("dd")}" + " " + x.Date.ToString("MMM") + ",";
                stringPresentCount += x.TotalEmployeePresentCount + ",";
                stringAbsentCount += x.TotalEmployeeAbsentCount + ",";
            });

            listOfEmpString.Add(stringLastMonths.Trim(new char[] { ',' }));
            listOfEmpString.Add(stringPresentCount.Trim(new char[] { ',' }));
            listOfEmpString.Add(stringAbsentCount.Trim(new char[] { ',' }));
            return listOfEmpString;
        }

        /// <summary>
                /// Logic to get topfive leavetypeapi view list of home and employee api dashboard
               /// </summary>
        public async Task<TopFiveLeaveTypeViewAPI> GetEmpLeave(int empId,int companyId)
        {
            var typeView = new TopFiveLeaveTypeViewAPI();
            var emp = await _employeesRepository.GetAllEmployees(companyId);
            var leave = await _leaveRepository.GetAllLeave();
            var getLeaveList = await _leaveRepository.GetEmployeeIdsLeave(empId,companyId);
            typeView.Leave = await GetTopFiveLeave(getLeaveList, emp, leave, companyId);
            return typeView;
        }

        /// <summary>
                /// Logic to get leaveapi list 
               /// </summary>
        /// <param name="employeeAppliedLeaveEntities" ></param>
        /// <param name="employeesEntity" ></param>
        /// <param name="leaveTypesEntities" ></param>
        public async Task<List<LeaveAPI>> GetTopFiveLeave(List<EmployeeAppliedLeaveEntity> employeeAppliedLeaveEntities, List<EmployeesEntity> employeesEntity, List<LeaveTypesEntity> leaveTypesEntities,int companyId)
        {
            var currentDate = DateTime.Now;
            var topFiveLeave = employeeAppliedLeaveEntities.Where(g => g.LeaveFromDate >= currentDate).OrderBy(g => g.LeaveFromDate.Month).ThenBy(g => g.LeaveFromDate.Day).ToList();
            var topFiveLeaveEmployees = employeesEntity.ToList();
            var topFiveLeaveType = leaveTypesEntities.ToList();
            var LeaveLists = new List<LeaveAPI>();
            var topFiveLeaveList = topFiveLeave.ToList();
            var count = 1;
            foreach (var item in topFiveLeaveList)
            {
                count = count == 6 ? 1 : count;
                var LeaveList = new LeaveAPI();
                LeaveList.LeaveDate = item.LeaveFromDate;
                LeaveList.LeaveColor = Common.Common.GetClassNameForLeaveDashboard(count);
                LeaveList.LeaveType = Convert.ToString(item.AppliedLeaveTypeId);
                LeaveList.LeaveApproved = item.IsApproved;
                foreach (var employee in topFiveLeaveEmployees)
                {
                    var name = await _employeesRepository.GetEmployeeByIdView(item.EmpId, companyId);
                    LeaveList.EmployeeName = name == null ? "" : name.FirstName + " " + name.LastName;
                }
                foreach (var items in topFiveLeaveType)
                {
                    var leavetype = await _leaveRepository.GetAllLeaveDetailsDashboard(item.AppliedLeaveTypeId,companyId);
                    LeaveList.LeaveType = leavetype.LeaveType;
                }

                LeaveLists.Add(LeaveList);
                count++;
            }
            return LeaveLists;
        }

        /// <summary>
                /// Logic to get employeeleavecountapi 
               /// </summary>
        /// <param name="empId" ></param>
        public async Task<EmployeeLeaveCountAPI> GetEmployeeLeaveCount(int empId, int companyId)
        {
            var employeeLeaveCountView = new EmployeeLeaveCountAPI();
            var leaveCounts = new List<LeaveCountEmployeeAPI>();
            var allLeaveDetails = await _leaveRepository.GetAllLeaveDetails(empId,companyId);
            var listLeaveSummarys = await _leaveRepository.GetLeaveByAppliedLeaveEmpId(empId,companyId);

            var leaveTypes = await GetAllLeave();
            var withoutPermissionLeaveTypes = leaveTypes.Where(x => x.LeaveTypeId != 5).ToList();
            foreach (var item in withoutPermissionLeaveTypes)
            {
                var leaveCount = new LeaveCountEmployeeAPI();
                leaveCount.LeaveType = item.LeaveType;
                if (allLeaveDetails != null)
                {
                    if (leaveCount.LeaveType == Constant.CasualLeave)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.CasualLeaveCount;
                    }
                    else if (leaveCount.LeaveType == Constant.SickLeave)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.SickLeaveCount;
                    }
                    else if (leaveCount.LeaveType == Constant.EarnedLeave)
                    {
                        leaveCount.TotalLeave = allLeaveDetails.EarnedLeaveCount;
                    }
                }
                else
                {
                    leaveCount.TotalLeave = 0;
                }

                leaveCount.AppliedLeave =listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 0).Sum(x => x.LeaveApplied);
                leaveCount.ApprovedLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 1).Sum(x => x.LeaveApplied);
                leaveCount.RemaingLeave = leaveCount.TotalLeave - leaveCount.ApprovedLeave;
                var appliedAndApproveLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved <= 1).Sum(x => x.LeaveApplied);
                leaveCount.SumOfAppliedLeaveAndApprovedLeave = Helpers.TotalLeaveCount(leaveCount.TotalLeave ,appliedAndApproveLeave);
                leaveCounts.Add(leaveCount);
            }
            var casualleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.CasualLeave).Select(x => x.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var sickleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.SickLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var earnedleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.EarnedLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();

            employeeLeaveCountView.RemaingLeave = casualleaveremaining + sickleaveremaining + earnedleaveremaining;
            return employeeLeaveCountView;
        }

        /// <summary>
                /// Logic to get leave types employee api list
               /// </summary>

        public async Task<List<LeaveTypesEmployeeAPI>> GetAllLeave()
        {
            var listLeave = await _leaveRepository.GetAllLeave();
            var result = _mapper.Map<List<LeaveTypesEmployeeAPI>>(listLeave);
            return result;
        }

        /// <summary>
                /// Logic to get timelogviewapi 
               /// </summary>
        /// <param name="employeeId" ></param>
        public async Task<TimeLogViewAPI> GetTimeLogViewModel(int employeeId,int companyId)
        {
            var timeLogView = new TimeLogViewAPI();
            var timeLogEntitys = await _dashboardRepository.GetTimeLogEntitysByEmployeeId(employeeId, companyId);
            var clocktime = await _companyRepository.GetAllCompanySetting(companyId);
            var totalIn = timeLogEntitys.Count(e => e.EntryStatus == Constant.EntryTypeIn);
            var totalOut = timeLogEntitys.Count() - totalIn;

            timeLogView.EntryStatus = totalIn > totalOut ? Constant.EntryTypeIn : Constant.EntryTypeOut;
            var inTime = timeLogEntitys.Where(e => e.EntryStatus == Constant.EntryTypeIn).Sum(x => x.LogSeconds);
            double secs = Convert.ToDouble(inTime);

            TimeSpan t = TimeSpan.FromSeconds(secs);
            string answer = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes);
            double maximum = 32400;
            var percentage = secs == 0 ? 0 : Convert.ToInt16((secs / maximum) * 100);
            timeLogView.TimeClockPercentage = percentage;
            timeLogView.TimeOngoingTime = answer;
            timeLogView.TodayClockIn = clocktime.IsTimeLockEnable;
            return timeLogView;
        }

        /// <summary>
                /// Logic to get employeeworkinghoursview api
               /// </summary>
        /// <param name="employeeId" ></param>
        public async Task<EmployeeWorkingHoursViewAPI> GetEmployeeTotalHoursForWeek(int employeeId,int companyId)
        {
            try
            {
                var employeeLeaveCountView = new EmployeeWorkingHoursViewAPI();
                var attendaceListViewModels = new AttendaceListViewModels();
                var employeeIdValue = employeeId;
                var attendaceListViewModel = new AttendaceListViewModel();
                attendaceListViewModel.EmployeeId = employeeId;
                DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
                DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
                DateTime startdate = currentWeekStartDate.Date;
                var currentdate = DateTime.Now;
                attendaceListViewModel.StartDate = startdate.Date.ToString("dd/MM/yyyy");
                attendaceListViewModel.EndDate = currentdate.ToString("dd/MM/yyyy");
                attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
                var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString("dd/MM/yyyy");
                var dTo = string.IsNullOrEmpty(attendaceListViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString("dd/MM/yyyy");
                var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
                var flterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
                List<DateTime> allDates = new List<DateTime>();
                for (DateTime date = flterDate; date <= flterEndDate; date = date.AddDays(1))
                    allDates.Add(date);

                var esslId = await _employeesRepository.GetEmployeeById(attendaceListViewModel.EmployeeId, companyId);
                if (esslId != null)
                {
                    var empId = Convert.ToString(esslId.EsslId);
                    List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@empId", empId),
                    new KeyValuePair<string, string>("@startDate",dFrom),
                    new KeyValuePair<string, string>("@endDate",dTo),
                };

                    var employees = await _employeesRepository.GetAllEmployees(companyId);

                    var attendanceReportDateModels = await _attendanceRepository.GetAllEmployeesByAttendaceFilter("spGetAttendanceByEmployeeFilterData", p);
                    attendanceReportDateModels = attendanceReportDateModels.DistinctBy(x => x.LogDateTime).ToList();
                    var filterViewAttendaces = _mapper.Map<List<FilterViewAttendace>>(attendanceReportDateModels);

                    var listOfRec = filterViewAttendaces.TakeLast(1);
                    var lastRecord = listOfRec.FirstOrDefault(x => x.Direction == Common.Constant.EntryTypeIn);
                    var attendance = new AttendanceReportDateModel();
                    if (lastRecord != null)
                    {
                        foreach (var record in listOfRec)
                        {
                            filterViewAttendaces.Add(new FilterViewAttendace()
                            {
                                Id = 0,
                                EmployeeId = record.EmployeeId,
                                LogDate = DateTime.Now.ToString("yyyy-MM-dd"),
                                LogDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                Direction = Common.Constant.EntryTypeOut,
                            });
                        }
                    }

                    var employeeEntity = employees.FirstOrDefault(x => x.EmpId == employeeIdValue);
                    var essl = Convert.ToInt16(employeeEntity?.EsslId);
                    if (essl > 0)
                    {
                        var stringToNum = Convert.ToString(essl);
                        var tableEmployeeId = essl;
                        foreach (var date in allDates)
                        {
                            var firstInTime = filterViewAttendaces.FirstOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn);
                            var lastInTime = filterViewAttendaces.LastOrDefault(x => x.EmployeeId == stringToNum && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);
                            var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                            var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";
                            if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                            {
                                DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                                DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                                string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                                dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : "00:00:00";
                                var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                                var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                                var attendaceListModel = new AttendaceListViewModel();
                                attendaceListModel.EmployeeId = employeeIdValue;
                                attendaceListModel.TotalHours = dt;
                                attendaceListModel.InsideOffice = insideoffice;
                                attendaceListViewModels.AttendaceListViewModel.Add(attendaceListModel);
                            }
                        }
                    }
                }
                if (attendaceListViewModels.AttendaceListViewModel.Count() > 0)
                {
                    var totalHours = attendaceListViewModels.AttendaceListViewModel.Select(x => x.InsideOffice);
                    double seconds = 0;
                    var totalSeconds = new List<int>();
                    var totalHoursWorking = "";
                    var hours = "";
                    var min = "";
                    foreach (var item in totalHours)
                    {
                        string time = item;
                        seconds = TimeSpan.Parse(time).TotalSeconds;
                        totalSeconds.Add((int)seconds);
                    }
                    var totSec = totalSeconds.Sum();
                    if (totSec < 0)
                    {
                        totalHoursWorking = Constant.TimeFormatHMZero + " " + Constant.Hrs;
                        employeeLeaveCountView.TotalHours = totalHoursWorking;
                    }
                    else
                    {
                        TimeSpan time = TimeSpan.FromSeconds(totSec);

                        if (time.Days * 24 + time.Hours < 10)
                        {
                            hours = "0" + Convert.ToString(time.Days * 24 + time.Hours);
                        }
                        else
                        {
                            hours = Convert.ToString(time.Days * 24 + time.Hours);
                        }
                        if (time.Minutes < 10)
                        {
                            min = "0" + Convert.ToString(time.Minutes);
                        }
                        else
                        {
                            min = Convert.ToString(time.Minutes);
                        }
                        employeeLeaveCountView.TotalHours = hours + ":" + min + " " + Constant.Hrs;
                    }
                }
                else
                {
                    employeeLeaveCountView.TotalHours = Constant.TimeFormatHMZero + " " + Constant.Hrs;
                }

                return employeeLeaveCountView;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
                /// Logic to get getbreakhours api
               /// </summary>
        /// <param name="empId" ></param>
        /// <param name="attendanceEntities" ></param>
        /// <param name="filterDate" ></param>
        public string GetBreakHour(string empId, List<AttendanceReportDateModel> attendanceEntities, DateTime filterDate)
        {
            var attendanceEty = attendanceEntities.Where(x => x.EmployeeId == empId && Convert.ToDateTime(x.LogDateTime).Date == filterDate.Date).ToList();
            var listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).ToList();

            if (listOfRecords.Count > 0)
            {
                var attendanceReportDateModel = listOfRecords.Take(1).FirstOrDefault();

                if (attendanceReportDateModel.Direction == Constant.EntryTypeOut)
                {
                    listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).Skip(2).ToList();
                }
                else
                {
                    listOfRecords = attendanceEty.OrderBy(x => x.LogDateTime).Skip(1).ToList();
                }
            }
            var outEntry = DateTime.Now;
            var inEntry = DateTime.Now;
            double totalSeconds = 0;
            foreach (var item in listOfRecords)
            {
                if (item.Direction == Constant.EntryTypeOut)
                {
                    outEntry = Convert.ToDateTime(item.LogDateTime);
                }
                else if (item.Direction == Constant.EntryTypeIn)
                {
                    var seconds = (Convert.ToDateTime(item.LogDateTime) - outEntry).TotalSeconds;
                    totalSeconds += seconds;
                }
            }
            if (totalSeconds < 0)
            {
                return Common.Constant.TimeFormatZero;
            }
            else
            {
                TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
                string answer = Convert.ToString((time.Days * 24 + time.Hours) + ":" + time.Minutes + ":" + time.Seconds);
                return answer;
            }
        }

        /// <summary>
                /// Logic to get timeloggerviewmodel inserttimelog
               /// </summary>
        /// <param name="timeLoggerViewModel" ></param>
        public async Task<UserDashboardResponse> InsertTimeLog(TimeLoggerViewModelAPI timeLoggerViewModel)
        {
            var userDashboardResponse = new UserDashboardResponse();
            if (timeLoggerViewModel != null)
            {
                var entityTimeLog = await _dashboardRepository.GetLastTimeLogEntityByEmployeeId(timeLoggerViewModel.EmployeeId,timeLoggerViewModel.CompanyId);
                if (entityTimeLog != null)
                {
                    var seconds = (DateTime.Now - entityTimeLog.ClockInTime).TotalSeconds;
                    entityTimeLog.ClockOutTime = DateTime.Now;
                    entityTimeLog.LogSeconds = Convert.ToInt64(seconds);
                    var upd = await _dashboardRepository.UpdateTimeLog(entityTimeLog);
                }
                timeLoggerViewModel.CreatedDate = DateTime.Now;
                timeLoggerViewModel.ClockInTime = DateTime.Now;
                timeLoggerViewModel.LogSeconds = 0;
                var timeLoggerEntity = _mapper.Map<TimeLoggerEntity>(timeLoggerViewModel);
                var result = await _dashboardRepository.InsertTimeLog(timeLoggerEntity);
                userDashboardResponse.IsSuccess = true;
                userDashboardResponse.Message = Common.Constant.Success;
            }
            else
            {
                userDashboardResponse.IsSuccess = false;
                userDashboardResponse.Message = Common.Constant.Failure;
            }

            return userDashboardResponse;
        }


        /// <summary>
                /// Logic to get timelogviewmodelapi list
               /// </summary>
        /// <param name="timeLoggerViewModel" ></param>
        public async Task<UserTimeResponse> GetLoggedInTotalTimeLog(TimeLoggerViewModelAPI timeLoggerViewModel)
        {
            var userTimeResponse = new UserTimeResponse();
            if (timeLoggerViewModel != null)
            {
                var timelog = new TimeLogViewAPI();
                var timeLoggerEntitys = await _dashboardRepository.GetTimeLogEntitysByEmployeeId(timeLoggerViewModel.EmployeeId,timeLoggerViewModel.CompanyId);
                var lastInRecord = timeLoggerEntitys.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (lastInRecord != null)
                {
                    var inTime = timeLoggerEntitys.Where(x => x.EntryStatus == Constant.EntryTypeIn).Sum(x => x.LogSeconds);
                    var seconds = (DateTime.Now - lastInRecord.ClockInTime).TotalSeconds;
                    var totSeconds = inTime + seconds;
                    double secs = Convert.ToDouble(totSeconds);
                    TimeSpan t = TimeSpan.FromSeconds(secs);
                    string answer = string.Format("{0:D2}:{1:D2}",
                                    t.Hours,
                                    t.Minutes);
                    double maximum = 32400;
                    var percentage = secs == 0 ? 0 : Convert.ToInt16((secs / maximum) * 100);
                    timelog.TimeClockPercentage = percentage;
                    timelog.TimeOngoingTime = answer;

                    userTimeResponse.timeLogViewAPI = timelog;
                    userTimeResponse.IsSuccess = true;
                    userTimeResponse.Message = Common.Constant.Success;
                }
            }
            else
            {
                userTimeResponse.IsSuccess = false;
                userTimeResponse.Message = Common.Constant.Failure;
            }
            return userTimeResponse;
        }

        /// <summary>
                /// Logic to get announcement 
               /// </summary>
        /// <param name="annuncementId" ></param>
        public async Task<UserDashboardResponse> GetAnnouncementById(int announcementId,int companyId)
        {
            var userTimeResponse = new UserDashboardResponse();
            if (announcementId != null)
            {
                var announcementEntity = await _masterRepository.GetAnnouncementById(announcementId, companyId);
                var annuncerName = await _employeesRepository.GetEmployeeById(announcementEntity.CreatedBy, companyId);
                var announcement = new Announcements();
                announcement.AnnuncementId = announcementEntity.AnnouncementId;
                announcement.AnnouncementName = announcementEntity.AnnouncementName;
                announcement.Description = announcementEntity.Description;
                announcement.AnnouncerName = annuncerName.FirstName + " " + annuncerName.LastName;
                announcement.CreatedDate = announcementEntity.CreatedDate;
                announcement.AnnouncementDate = announcementEntity.AnnouncementDate.ToString(Constant.DateFormat);
                announcement.CreatedBy = announcementEntity.CreatedBy;
                userTimeResponse.IsSuccess = true;
                userTimeResponse.Message = Common.Constant.Success;
            }
            else
            {
                userTimeResponse.IsSuccess = false;
                userTimeResponse.Message = Common.Constant.Failure;
            }

            return userTimeResponse;
        }

        /// <summary>
                /// Logic to get topfive leavetype view api based on reporting person list of home and employee api dashboard
               /// </summary>
        public async Task<TopFiveLeaveTypeViewAPI> GetLeaveApprove(int empId,int companyId)
        {
            var typeView = new TopFiveLeaveTypeViewAPI();
            var emp = await _employeesRepository.GetAllEmployees(companyId);
            var leave = await _leaveRepository.GetAllLeave();
            var employeeIds = await _employeesRepository.GetAllEmployeeIdsReportingPersonForLeave(empId,companyId);

            var empIds = new List<int>();
            foreach (var employee in employeeIds)
            {
                empIds.Add(employee.EmployeeId);
            }

            var getLeaveList = await _leaveRepository.GetAllLeaveSummarys(empIds,companyId);
            typeView.Leave = await GetTopFiveLeave(getLeaveList, emp, leave, companyId);
            return typeView;
        }
    }
}
