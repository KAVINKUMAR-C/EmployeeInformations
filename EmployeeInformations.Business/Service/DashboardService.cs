using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Business.Utility.Helper;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.AttendanceViewModel;
using EmployeeInformations.Model.DashboardViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Ocsp;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EmployeeInformations.Business.Service
{

    public class DashboardService : IDashboardService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IProjectDetailsRepository _projectDetailsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMasterRepository _masterRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ICompanyPolicyRepository _companyPolicyRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IConfiguration _config;

        private readonly IEmployeeInformationService _employeeInformationService;
        public DashboardService(IEmployeesRepository employeesRepository, IProjectDetailsRepository projectDetailsRepository, IClientRepository clientRepository, IMapper mapper,
            IDashboardRepository dashboardRepository, ILeaveRepository leaveRepository, IMasterRepository masterRepository, IAttendanceRepository
            attendanceRepository, ICompanyPolicyRepository companyPolicyRepository, IEmployeeInformationService employeeInformationService, ICompanyRepository companyRepository, IConfiguration config)
        {
            _employeesRepository = employeesRepository;
            _projectDetailsRepository = projectDetailsRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _dashboardRepository = dashboardRepository;
            _leaveRepository = leaveRepository;
            _masterRepository = masterRepository;
            _attendanceRepository = attendanceRepository;
            _companyPolicyRepository = companyPolicyRepository;
            _employeeInformationService = employeeInformationService;
            _companyRepository = companyRepository;
            _config = config;
        }

        /// <summary>
        /// Logic to get dashboardViewModel details
        /// </summary>  
        public async Task<DashboardViewModel> GetAllDashboardView(int sessionEmployeeId, int roleId,int companyId)
        {
            var listOfDashboardViewModel = new DashboardViewModel();

            var RolePrivileges = new List<int>();
            if (roleId >= 0)
            {
                try
                {
                    var id = roleId;
                    RolePrivileges = await _employeeInformationService.DashboardRolePermissionByRoleId(id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TotalEmployee))
            {
                try
                {
                    listOfDashboardViewModel.TotalEmployeeView = await GetTotalEmployeeDashBorad(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.RecentJoiners))
            {
                try
                {
                    // listOfDashboardViewModel.TotalEmployeeView = await GetTotalEmployeeDashBorad();
                    listOfDashboardViewModel.EmployeesList = await GetEmployeesTopFive(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TotalProjects))
            {
                try
                {
                    listOfDashboardViewModel.TotalProjectView = await GetTotalProjectDashBoarad(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.Employee))
            {
                try
                {
                    listOfDashboardViewModel.TotalEmployeeLeaveView = await GetTotalEmployeeLeaveDashBorad(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TodayPresentEmployees))
            {
                try
                {
                    listOfDashboardViewModel.TotalEmployeeLeaveView = await GetTotalEmployeeLeaveDashBorad(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TotalClients))
            {
                try
                {
                    listOfDashboardViewModel.TotalClientView = await GetTotalClientDashBoarad(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.Project))
            {
                try
                {
                    listOfDashboardViewModel.TopFiveProjectsView = await GetTotalFiveActiveProjectsDashBoarad(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.Department))
            {
                try
                {
                    listOfDashboardViewModel.TopFiveDepartmentsView = await GetTopFiveActiveDepartmentsDashBoarad(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.UpcomingHolidays))
            {
                try
                {
                    // listOfDashboardViewModel.TopFiveLeaveView = await GetTopFiveLeaveDashBoarad();
                    listOfDashboardViewModel.LeaveLists = await EmployeeHolidays(companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.UpcomingCelebration))
            {
                try
                {
                    listOfDashboardViewModel.Celebrations = await TopFiveCelebration(companyId);
                    //listOfDashboardViewModel.TopFiveCelebrationView = await GetTopFiveCelebration();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.AnnouncementDetails))
            {
                try
                {
                    //listOfDashboardViewModel.TopFiveAnnuncementView = await GetTopFiveAnnuncements();
                    listOfDashboardViewModel.Announcements = await GetAnnouncement();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.LeaveWorkFromHome))
            {
                try
                {
                    // listOfDashboardViewModel.TopFiveLeaveWfhView = await GetTopFiveLeaveWfh();
                    listOfDashboardViewModel.TopFiveLeaveViews = await GetTopFiveWfh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.LeaveApproved))
            {
                try
                {
                    //listOfDashboardViewModel.TopFiveLeaveTypeView = await GetEmpLeave(sessionEmployeeId);
                    listOfDashboardViewModel.TopFiveLeaveViews = await GetLeave(sessionEmployeeId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.LeavePending))
            {
                try
                {
                    //listOfDashboardViewModel.TopFiveLeaveTypeView = await GetEmpLeave(sessionEmployeeId);
                    listOfDashboardViewModel.TopFiveLeaveViews = await GetLeave(sessionEmployeeId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.RemainingLeave))
            {
                try
                {
                    listOfDashboardViewModel.EmployeeLeaveCount = await GetEmployeeLeaveCount(sessionEmployeeId,companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.TodayLockTime))
            {
                try
                {
                    listOfDashboardViewModel.TimeLogViewModel = await GetTimeLogViewModel(sessionEmployeeId, companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.WorkingHours))
            {
                try
                {
                    listOfDashboardViewModel.EmployeeWorkingHoursView = await GetEmployeeTotalHoursForWeek(sessionEmployeeId, companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            if (RolePrivileges.Contains((int)EmployeeDashboardPages.LeaveApproveReportPerson))
            {
                try
                {
                    listOfDashboardViewModel.TopFiveLeaveTypeView = await GetLeaveApprove(sessionEmployeeId, companyId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            try
            {
                listOfDashboardViewModel.TotalActiveEmployeesView = await GetTotalActiveEmployeesDashBoarad(companyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listOfDashboardViewModel;
        }


        /// <summary>
        /// Logic to get totalemployeeview list of home dashboard
        /// </summary>  
        public async Task<TotalEmployeeView> GetTotalEmployeeDashBorad(int companyId)
        {
            var totalEmployeeView = new TotalEmployeeView();
            var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfileIsDeleted(companyId);
            totalEmployeeView.TotalEmployeeCount = await _employeesRepository.GetEmployeeMaxCount(companyId);
            totalEmployeeView.TotalEmployeeCountPercentage = GetemployeeJoingPercentage(allEmployeeProfiles);
            var listOfEmpString = GetEmployeeJoiningByMonthInCount(allEmployeeProfiles);
            totalEmployeeView.Months = Convert.ToString(listOfEmpString[0]);
            totalEmployeeView.EmployeeByMonthCount = Convert.ToString(listOfEmpString[1]);
            return totalEmployeeView;
        }

        public async Task<List<EmployeesList>> GetEmployeesTopFive(int companyId)
        {
            var employees = new List<EmployeesList>();
            employees = await _employeesRepository.GetEmployeesTopFive(companyId);
            return employees;
        }

        /// <summary>
        /// Logic to get totalemployeeleaveview list of home and employee dashboard
        /// </summary>
        public async Task<TotalEmployeeLeaveView> GetTotalEmployeeLeaveDashBorad(int companyId)
        {
            var totalEmployeeView = new TotalEmployeeLeaveView();
            totalEmployeeView.TotalEmployeeLeaves = new List<TotalEmployeeLeaves>();
            var EndDate = DateTime.Now.ToString(Constant.DateFormat);
            var StartDate = DateTime.Now.AddMonths(-1).ToString(Constant.DateFormat);
            var AttendaceListViewModel = new List<AttendaceListViewModel>();
            var dFrom = string.IsNullOrEmpty(StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(StartDate).ToString(Constant.DateFormat);
            var dTo = string.IsNullOrEmpty(EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(EndDate).ToString(Constant.DateFormat);
            var flterDate = DateTimeExtensions.ConvertToNotNullDatetime(dFrom);
            var flterEndDate = DateTimeExtensions.ConvertToNotNullDatetime(dTo);
            List<DateTime> allDates = new List<DateTime>();

            for (DateTime date = flterDate; date <= flterEndDate; date = date.AddDays(1))

                if (DayOfWeek.Saturday != date.DayOfWeek && DayOfWeek.Sunday != date.DayOfWeek)
                {
                    allDates.Add(date);
                }

            var empId = Constant.ZeroStr;
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
                var getProfileCount = getProfileList.Where(x => x.DateOfJoining <= date.Date && !x.IsDeleted).ToList();
                var strdate = date.ToString(Constant.DateFormatYMDHyphen);
                var list = getEmployeeAttendanceCount.Where(x => x.LogDate == strdate).GroupBy(x => x.EmployeeId).ToList();
                List<int> first = new List<int>();
                List<int> second = new List<int>();

                foreach (var item in getProfileCount)
                {
                    var esslId = getAllEmployeeList.FirstOrDefault(x => x.EmpId == item.EmpId && !x.IsDeleted);
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
                totalEmployeeView.TotalEmployeeLeaves.Add(new TotalEmployeeLeaves()
                {
                    Date = date,
                    TotalEmployeePresentCount = present,
                    TotalEmployeeAbsentCount = absent,
                });
            };

            var getCount = getProfileList.Where(x => x.DateOfJoining <= DateTime.Now.Date && !x.IsDeleted).ToList();
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
        /// Logic to get totalprojectview list of home dashboard
        /// </summary>
        public async Task<TotalProjectView> GetTotalProjectDashBoarad(int companyId)
        {
            var totalProjectView = new TotalProjectView();
            var allProjectDetails = await _projectDetailsRepository.GetAllProjectDetails(companyId);
            totalProjectView.TotalProjectCount = await _projectDetailsRepository.GetProjectMaxCount(companyId);
            totalProjectView.TotalProjectCountPercentage = GetProjectPercentage(totalProjectView.TotalProjectCount, allProjectDetails);
            var listOfProjectString = GetProjectByMonthInCount(allProjectDetails);
            totalProjectView.ProjectMonths = Convert.ToString(listOfProjectString[0]);
            totalProjectView.ProjectByMonthCount = Convert.ToString(listOfProjectString[1]);
            return totalProjectView;
        }

        /// <summary>
        /// Logic to get totalclientview list of home dashboard
        /// </summary>
        public async Task<TotalClientView> GetTotalClientDashBoarad(int companyId)
        {
            var totalClientView = new TotalClientView();
            var allClientDetails = await _clientRepository.GetAllClient(companyId);
            totalClientView.TotalClientCount = await _clientRepository.GetClientMaxCount(companyId);
            totalClientView.TotalClientCountPercentage = GetClientPercentage(totalClientView.TotalClientCount, allClientDetails);
            var listOfClientString = GetClientByMonthInCount(allClientDetails);
            totalClientView.ClientMonths = Convert.ToString(listOfClientString[0]);
            totalClientView.ClientByMonthCount = Convert.ToString(listOfClientString[1]);
            return totalClientView;
        }

        /// <summary>
        /// Logic to get total active employee view list 
        /// </summary>
        public async Task<TotalActiveEmployeesView> GetTotalActiveEmployeesDashBoarad(int companyId)
        {
            var totalActiveEmployeesView = new TotalActiveEmployeesView();
            var allActiveEmployeesDetails = await _employeesRepository.GetAllActiveEmployees(companyId);
            totalActiveEmployeesView.TotalActiveEmployeesCount = await _employeesRepository.GetActiveEmployeeMaxCount(companyId);
            totalActiveEmployeesView.TotalActiveEmployeeCountPercentage = GetActiveEmployeesPercentage(totalActiveEmployeesView.TotalActiveEmployeesCount, allActiveEmployeesDetails);
            var listOfEmpString = GetActiveEmployeesByMonthInCount(allActiveEmployeesDetails);
            totalActiveEmployeesView.ActiveEmployeesMonths = Convert.ToString(listOfEmpString[0]);
            totalActiveEmployeesView.ActiveEmployeesByMonthCount = Convert.ToString(listOfEmpString[1]);
            return totalActiveEmployeesView;
        }

        /// <summary>
        /// Logic to get topfiveprojectview list of home dashboard
        /// </summary>
        public async Task<TopFiveProjectsView> GetTotalFiveActiveProjectsDashBoarad(int companyId)
        {
            var topFiveProjectsView = new TopFiveProjectsView();
            var projects = await _dashboardRepository.GetAllProjectDetails(companyId);
            topFiveProjectsView.ProjectLists = GetTopFiveProjects(projects);
            var strProjectListCount = await GetStringProjectNames(projects, companyId);
            topFiveProjectsView.StringProjectName = Convert.ToString(strProjectListCount[0]);
            topFiveProjectsView.ProjectByEmployeeCount = Convert.ToString(strProjectListCount[1]);
            return topFiveProjectsView;
        }

        /// <summary>
        /// Logic to get topfive departmentview list of home dashboard
        /// </summary>
        public async Task<TopFiveDepartmentsView> GetTopFiveActiveDepartmentsDashBoarad(int companyId)
        {
            var topFiveDepartmentsView = new TopFiveDepartmentsView();
            var listofEmployees = await _employeesRepository.GetAllEmployeeDetails(companyId);
            topFiveDepartmentsView.DepartmentLists = await GetTopFiveDepartments(listofEmployees,companyId);
            return topFiveDepartmentsView;
        }

        /// <summary>
        /// Logic to get topfive leave view list of home and employee dashboard
        /// </summary>
        public async Task<TopFiveLeaveView> GetTopFiveLeaveDashBoarad(int companyId)
        {
            var topFiveLeaveView = new TopFiveLeaveView();
            var listofLeave = await _leaveRepository.GetAllEmployeeHolidays(companyId);
            topFiveLeaveView.LeaveLists = GetTopFiveLeaves(listofLeave);
            return topFiveLeaveView;
        }

        public async Task<List<LeaveList>> EmployeeHolidays(int companyId)
        {
            var holidays = new List<LeaveList>();
            holidays = await _leaveRepository.EmployeeHolidays(companyId);
            return holidays;
        }

        /// <summary>
        /// Logic to get topfive celebration view list of home and employee dashboard
        /// </summary>
        public async Task<TopFiveCelebrationView> GetTopFiveCelebration(int companyId)
        {
            var CelebrationView = new TopFiveCelebrationView();
            var listOfEmployeeProfile = await _employeesRepository.GetAllEmployeeProfileIsDeleted(companyId);
            var employees = await _employeesRepository.GetAllEmployeeDetails(companyId);
            CelebrationView.Celebration = await GetTopFiveCelebration(listOfEmployeeProfile, employees);
            return CelebrationView;
        }


        public async Task<List<Celebration>> TopFiveCelebration(int companyId)
        {
            var celebrations = new List<Celebration>();
            celebrations = await _employeesRepository.GetTopFiveCelebration(companyId);
            return celebrations;
        }

        /// <summary>
        /// Logic to get topfive leavetype view list of home and employee dashboard
        /// </summary>
        //public async Task<TopFiveLeaveTypeView> GetEmpLeave(int empId)
        //{
        //    var typeView = new TopFiveLeaveTypeView();
        //    var emp = await _employeesRepository.GetAllEmployees();
        //    var leave = await _leaveRepository.GetAllLeave();
        //    var getLeaveList = await _leaveRepository.GetEmployeeIdsLeave(empId);
        //    typeView.Leave = await GetTopFiveLeave(getLeaveList, emp, leave);
        //    return typeView;
        //}

        public async Task<List<TopFiveLeaveViews>> GetLeave(int empId)
        {
            var leaveViews = new List<TopFiveLeaveViews>();
            leaveViews = await _leaveRepository.TopFiveLeaveTypeView(empId);
            return leaveViews;
        }


        /// <summary>
        /// Logic to get topfive leavetype view based on reporting person list of home and employee dashboard
        /// </summary>
        public async Task<TopFiveLeaveTypeView> GetLeaveApprove(int empId,int companyId)
        {
            var typeView = new TopFiveLeaveTypeView();
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

        /// <summary>
        /// Logic to get topfive leavewfh view list of home and employee dashboard
         /// </summary>
        public async Task<TopFiveLeaveWfhView> GetTopFiveLeaveWfh(int companyId)
        {
            var typeView = new TopFiveLeaveWfhView();
            var listOfLeave = await _leaveRepository.GetAllLeaveDashboard(companyId);
            var emp = await _employeesRepository.GetAllEmployees(companyId);
            var leave = await _leaveRepository.GetAllLeave();
            typeView.Leave = await GetTopFiveLeaveWfh(listOfLeave, emp, leave, companyId);
            return typeView;
        }

        public async Task<List<TopFiveLeaveViews>> GetTopFiveWfh()
        {
            var wrokfromhomes = new List<TopFiveLeaveViews>();
            wrokfromhomes = await _leaveRepository.GetTopFiveWfh();
            return wrokfromhomes;
        }

        /// <summary>
        /// Logic to get topfive annuncement view list of home and employee dashboard
        /// </summary>
        public async Task<TopFiveAnnuncementView> GetTopFiveAnnuncements(int companyId)
        {
            try
            {
                var typeView = new TopFiveAnnuncementView();
                var listOfAnnuncement = await _masterRepository.GetAllAnnouncementactive(companyId);
                typeView.Announcement = await GetTopFiveAnnuncement(listOfAnnuncement);
                return typeView;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Announcements>> GetAnnouncement()
        {
            var announcements = new List<Announcements>();
            announcements = await _masterRepository.GetAnnouncement();
            return announcements;
        }

        /// <summary>
        /// Logic to get project list 
        /// </summary>
        /// <param name="topFiveProject" ></param>
        public List<ProjectList> GetTopFiveProjects(List<ProjectDetailsEntity> topFiveProject)
        {
            var projectLists = new List<ProjectList>();
            var count = 1;
            foreach (var item in topFiveProject)
            {
                var project = new ProjectList();
                project.ProjectName = item.ProjectName;
                project.ProjectColor = Common.Common.GetClassNameForProjectDashboard(count);
                projectLists.Add(project);
                count++;
            }
            return projectLists;
        }

        /// <summary>
        /// Logic to get department list 
        /// </summary>
        /// <param name="employeesEntity" ></param>
        public async Task<List<DepartmentList>> GetTopFiveDepartments(List<EmployeesEntity> employeesEntity,int companyId)
        {
            var topfive = employeesEntity.GroupBy(x => x.DepartmentId).ToList();
            var departmentLists = new List<DepartmentList>();
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
                    var departmentList = new DepartmentList();
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
        /// Logic to get leave list 
        /// </summary>
        /// <param name="employeeHolidaysEntities" ></param>
        public List<LeaveList> GetTopFiveLeaves(List<EmployeeHolidaysEntity> employeeHolidaysEntities)
        {
            var currentDate = DateTime.Now;
            var topfive = employeeHolidaysEntities.Where(x => x.HolidayDate > currentDate).ToList();
            var leaveLists = new List<LeaveList>();
            var topFivedepartment = topfive.Take(5).ToList();
            var count = 1;
            foreach (var item in topFivedepartment)
            {
                var leaveList = new LeaveList();
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
        /// Logic to get celebration list 
        /// </summary>
        /// <param name="profileInfoEntities" ></param>
        /// <param name="employeesEntity" ></param>
        public async Task<List<Celebration>> GetTopFiveCelebration(List<ProfileInfoEntity> profileInfoEntities, List<EmployeesEntity> employeesEntity)
        {
            var currentDate = DateTime.Now;
            var topFiveCelebrationList = profileInfoEntities.Where(x => x.DateOfBirth.Month >= currentDate.Month && x.DateOfBirth.DayOfYear >= currentDate.DayOfYear && !x.IsDeleted).OrderBy(x => x.DateOfBirth.Month).ThenBy(x => x.DateOfBirth.DayOfYear).ToList();
            var topFiveCelebrations = employeesEntity.ToList();
            var celebrationLists = new List<Celebration>();
            var count = 1;
            topFiveCelebrationList = topFiveCelebrationList.Take(5).ToList();
            foreach (var item in topFiveCelebrationList)
            {
                var celebration = new Celebration();
                celebration.CelebrationDate = Convert.ToDateTime(item.DateOfBirth).ToString("dd-MMM");
                celebration.CelebrationColor = Common.Common.GetClassNameForLeaveDashboard(count);
                celebration.CelebrationName = Common.Constant.Birthday;
                var getEmployee = topFiveCelebrations.FirstOrDefault(x => x.EmpId == item.EmpId && !x.IsDeleted);
                celebration.EmployeeName = getEmployee != null ? getEmployee.FirstName + " " + getEmployee.LastName : "";
                celebration.EmpId = getEmployee == null ? 0 : getEmployee.EmpId;
                celebration.EmployeeStatus = getEmployee == null ? false : getEmployee.IsDeleted;
                celebrationLists.Add(celebration);
                count++;
            }

            return celebrationLists;
        }

        /// <summary>
        /// Logic to get leave list 
         /// </summary>
        /// <param name="employeeAppliedLeaveEntities" ></param>
        /// <param name="employeesEntities" ></param>
        /// <param name="leaveTypesEntities" ></param>
        public async Task<List<Leave>> GetTopFiveLeaveWfh(List<EmployeeAppliedLeaveEntity> employeeAppliedLeaveEntities, List<EmployeesEntity> employeesEntities, List<LeaveTypesEntity> leaveTypesEntities,int companyId)
        {
            var currentDate = DateTime.Now;
            var listLeaveWfh = employeeAppliedLeaveEntities.Where(j => j.LeaveFromDate.Date == currentDate.Date && j.IsApproved == 1).ToList();
            var topFiveLeaveWfhEmployees = employeesEntities.ToList();
            var topFiveLeaveType = leaveTypesEntities.ToList();
            var LeaveLists = new List<Leave>();
            var topFiveLeaveWfh = listLeaveWfh.ToList();
            var count = 1;
            foreach (var item in topFiveLeaveWfh)
            {
                var LeaveList = new Leave();

                LeaveList.LeaveDate = item.LeaveFromDate;
                LeaveList.LeaveColor = Common.Common.GetClassNameForLeaveDashboard(count);
                LeaveList.LeaveType = Convert.ToString(item.AppliedLeaveTypeId);
                LeaveList.LeaveApproved = item.IsApproved;
                foreach (var employee in topFiveLeaveWfhEmployees)
                {
                    var getEmployee = await _employeesRepository.GetEmployeeByIdView(item.EmpId, companyId);
                    LeaveList.EmployeeName = getEmployee == null ? "" : getEmployee.FirstName + " " + getEmployee.LastName;
                    LeaveList.EmpId = getEmployee == null ? 0 : getEmployee.EmpId;
                    LeaveList.EmployeeStatus = getEmployee == null ? false : getEmployee.IsDeleted;
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
        /// Logic to get leave list 
        /// </summary>
        /// <param name="employeeAppliedLeaveEntities" ></param>
        /// <param name="employeesEntity" ></param>
        /// <param name="leaveTypesEntities" ></param>
        public async Task<List<Leave>> GetTopFiveLeave(List<EmployeeAppliedLeaveEntity> employeeAppliedLeaveEntities, List<EmployeesEntity> employeesEntity, List<LeaveTypesEntity> leaveTypesEntities, int companyId)
        {
            var currentDate = DateTime.Now;
            var topFiveLeave = employeeAppliedLeaveEntities.Where(g => g.LeaveFromDate >= currentDate).OrderBy(g => g.LeaveFromDate.Month).ThenBy(g => g.LeaveFromDate.Day).ToList();
            var topFiveLeaveEmployees = employeesEntity.ToList();
            var topFiveLeaveType = leaveTypesEntities.ToList();
            var LeaveLists = new List<Leave>();
            var topFiveLeaveList = topFiveLeave.ToList();
            var count = 1;
            foreach (var item in topFiveLeaveList)
            {
                count = count == 6 ? 1 : count;
                var LeaveList = new Leave();
                LeaveList.LeaveDate = item.LeaveFromDate;
                LeaveList.LeaveColor = Common.Common.GetClassNameForLeaveDashboard(count);
                LeaveList.LeaveType = Convert.ToString(item.AppliedLeaveTypeId);
                LeaveList.LeaveApproved = item.IsApproved;
                foreach (var employee in topFiveLeaveEmployees)
                {
                    var getEmployee = await _employeesRepository.GetEmployeeByIdView(item.EmpId, companyId);
                    LeaveList.EmployeeName = getEmployee == null ? "" : getEmployee.FirstName + " " + getEmployee.LastName;
                    LeaveList.EmpId = getEmployee == null ? 0 : getEmployee.EmpId;
                    LeaveList.EmployeeStatus = getEmployee == null ? false : getEmployee.IsDeleted;
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
        /// Logic to get announcement list x.
        /// </summary>
        /// <param name="announcementEntities" ></param>

        public async Task<List<Announcements>> GetTopFiveAnnuncement(List<AnnouncementEntity> announcementEntities)
        {
            var AnnouncementLists = new List<Announcements>();
            var currentDate = DateTime.Now;
            var topFiveLeave = announcementEntities.Where(x => x.AnnouncementEndDate.Date >= currentDate.Date && x.AnnouncementDate.Date == currentDate.Date).Take(5).ToList();
            var count = 1;
            foreach (var item in topFiveLeave)
            {
                var employee = await _employeesRepository.GetEmployeeById(item.CreatedBy,item.CompanyId);
                var AnnouncementList = new Announcements();
                AnnouncementList.AnnuncementId = item.AnnouncementId;
                AnnouncementList.AnnouncementName = item.AnnouncementName;
                AnnouncementList.Description = item.Description;
                AnnouncementList.CreatedDate = item.CreatedDate;
                AnnouncementList.AnnouncementDate = item.AnnouncementDate.ToString(Constant.DateFormat);
                AnnouncementList.AnnouncementColor = Common.Common.GetClassNameForLeaveDashboard(count);
                AnnouncementList.AnnouncerName = employee != null ? employee.FirstName + " " + employee.LastName : "";
                AnnouncementList.CreatedBy = item.CreatedBy;
                AnnouncementLists.Add(AnnouncementList);
                count++;
            }
            return AnnouncementLists;
        }

        /// <summary>
        /// Logic to get employeeleavecount 
        /// </summary>
        /// <param name="empId" ></param>
        public async Task<EmployeeLeaveCount> GetEmployeeLeaveCount(int empId, int companyId)
        {
            var employeeLeaveCountView = new EmployeeLeaveCount();
            var leaveCounts = new List<LeaveCountEmployee>();
            var allLeaveDetails = await _leaveRepository.GetAllLeaveDetails(empId,companyId);
            var listLeaveSummarys = await _leaveRepository.GetLeaveByAppliedLeaveEmpId(empId,companyId);
            var leaveTypes = await GetAllLeave();
            var withoutPermissionLeaveTypes = leaveTypes.Where(x => x.LeaveTypeId != 5).ToList();
            foreach (var item in withoutPermissionLeaveTypes)
            {
                var leaveCount = new LeaveCountEmployee();
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

                leaveCount.AppliedLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 0).Sum(x => x.LeaveApplied);
                leaveCount.ApprovedLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved == 1).Sum(x => x.LeaveApplied);
                leaveCount.RemaingLeave = leaveCount.TotalLeave - leaveCount.AppliedLeave;
                var appliedAndApproveLeave = listLeaveSummarys.Where(x => x.AppliedLeaveTypeId == item.LeaveTypeId && x.IsApproved <= 1).Sum(x => x.LeaveApplied);
                leaveCount.SumOfAppliedLeaveAndApprovedLeave = Helpers.TotalLeaveCount(leaveCount.TotalLeave, leaveCount.AppliedLeave);
                leaveCount.RemaingLeave = leaveCount.TotalLeave - (leaveCount.AppliedLeave + leaveCount.ApprovedLeave);
                leaveCount.SumOfAppliedLeaveAndApprovedLeave = Helpers.TotalLeaveCount(leaveCount.TotalLeave, (leaveCount.AppliedLeave + leaveCount.ApprovedLeave));
                leaveCounts.Add(leaveCount);
            }
            var casualleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.CasualLeave).Select(x => x.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var sickleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.SickLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();
            var earnedleaveremaining = leaveCounts.Where(x => x.LeaveType == Constant.EarnedLeave).Select(y => y.SumOfAppliedLeaveAndApprovedLeave).FirstOrDefault();

            employeeLeaveCountView.RemaingLeave = casualleaveremaining + sickleaveremaining + earnedleaveremaining;
            return employeeLeaveCountView;
        }

        /// <summary>
        /// Logic to get employeeworkinghoursview 
        /// </summary>
        /// <param name="employeeId" ></param>
        public async Task<EmployeeWorkingHoursView> GetEmployeeTotalHoursForWeek(int employeeId,int companyId)
        {
            try
            {
                var employeeLeaveCountView = new EmployeeWorkingHoursView();
                var attendaceListViewModels = new AttendaceListViewModels();
                var employeeIdValue = employeeId;
                var attendaceListViewModel = new AttendaceListViewModel();
                attendaceListViewModel.EmployeeId = employeeId;
                DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
                DayOfWeek endofweek = DayOfWeek.Friday;
                DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
                DateTime currentWeekEndDate = (currentWeekStartDate).AddDays(4).Date;
                DateTime startdate = currentWeekStartDate.Date;
                var currentdate = DateTime.Now;
                attendaceListViewModel.StartDate = startdate.Date.ToString(Constant.DateFormat);
                attendaceListViewModel.EndDate = currentdate.ToString(Constant.DateFormat);
                attendaceListViewModels.AttendaceListViewModel = new List<AttendaceListViewModel>();
                var dFrom = string.IsNullOrEmpty(attendaceListViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
                var dTo = string.IsNullOrEmpty(attendaceListViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString(Constant.DateFormat);
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
                    attendanceReportDateModels = attendanceReportDateModels.Distinct().ToList();
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
                            var firstInTime = filterViewAttendaces.FirstOrDefault(x => x.EmployeeId == Convert.ToString(employeeIdValue) && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeIn); 
                            var lastInTime = filterViewAttendaces.LastOrDefault(x => x.EmployeeId == Convert.ToString(employeeIdValue) && Convert.ToDateTime(x.LogDateTime).Date == date.Date && x.Direction == Constant.EntryTypeOut);
                            var firstLoginDate = firstInTime != null ? firstInTime.LogDateTime : "";
                            var lastLoginDate = lastInTime != null ? lastInTime.LogDateTime : "";
                            if (string.IsNullOrEmpty(firstLoginDate) && string.IsNullOrEmpty(lastLoginDate))
                            {
                                var tFrom = string.IsNullOrEmpty(attendaceListViewModel.EntryTime) ? "" : attendaceListViewModel.EntryTime;
                                var tTo = !string.IsNullOrEmpty(attendaceListViewModel.ExitTime) ? attendaceListViewModel.ExitTime : "";
                                var attendanceReportDatamodelList = await _dashboardRepository.GetLastTimeLogEntityByEmployeeIdDateList(date, tFrom, tTo,companyId);
                                foreach (var item in attendanceReportDatamodelList)
                                {
                                     employeeEntity = employees.FirstOrDefault(x => x.EmpId == employeeIdValue);
                                    if (employeeEntity != null)
                                    {
                                        DateTime StartTime = Convert.ToDateTime(item.ClockInTime);
                                        DateTime EndTime = Convert.ToDateTime(item.ClockOutTime);
                                        string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                                        dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
                                        var breakHours = GetBreakHour(stringToNum, attendanceReportDateModels, date);
                                        var insideoffice = Convert.ToDateTime(dt).Subtract(Convert.ToDateTime(breakHours)).ToString().Split('.')[0].ToString();
                                        attendaceListViewModels.AttendaceListViewModel.Add(new AttendaceListViewModel
                                        {
                                            EmployeeId = employeeIdValue,
                                            TotalHours = dt,
                                            InsideOffice = insideoffice
                                        });
                                    }
                                }
                                var attendanceReportDatamodelLists = await GetTimeLog(employeeIdValue,date,companyId);
                                var attendaceListModel = new AttendaceListViewModel();
                                attendaceListModel.EmployeeId = employeeIdValue;
                                attendaceListModel.TotalHours = attendanceReportDatamodelLists.TotalHours;
                                attendaceListModel.InsideOffice = attendanceReportDatamodelLists.InsideOffice;
                                attendaceListViewModels.AttendaceListViewModel.Add(attendaceListModel);
                            }
                            else if (!string.IsNullOrEmpty(firstLoginDate) && !string.IsNullOrEmpty(lastLoginDate))
                            {
                                DateTime StartTime = Convert.ToDateTime(firstInTime.LogDateTime);
                                DateTime EndTime = Convert.ToDateTime(lastInTime.LogDateTime);
                                string dt = EndTime.Subtract(StartTime).ToString().Split('.')[0].ToString();
                                dt = TimeSpan.Parse(dt).TotalSeconds > 0 ? dt : Constant.TimeFormatZero;
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
                        hours = (time.Days * 24 + time.Hours < 10) ? Constant.ZeroStr + Convert.ToString(time.Days * 24 + time.Hours) : hours = Convert.ToString(time.Days * 24 + time.Hours);
                        min = (time.Minutes < 10) ? Constant.ZeroStr + Convert.ToString(time.Minutes) : Convert.ToString(time.Minutes);
                        employeeLeaveCountView.TotalHours = hours + ":" + min + " " + Constant.Hrs;
                    }
                }
                else
                {
                    employeeLeaveCountView.TotalHours = Constant.TimeFormatHMZero + " " + Constant.Hrs;
                }

                    var weeklytotalHour = Convert.ToInt32(_config.GetSection("WorkingHours").GetSection("TotalHour").Value);
                    var dayHour = Convert.ToInt32(_config.GetSection("WorkingHours").GetSection("DayHour").Value);
                    var getHolidayDate = await _leaveRepository.GetAllEmployeeHolidaysForMOnth(startdate, currentWeekEndDate,companyId);
                    employeeLeaveCountView.TotalDayHours = weeklytotalHour - (getHolidayDate * dayHour);

                    return employeeLeaveCountView;
                }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Logic to get getbreakhours 
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
        /// Logic to get leave types employee list
        /// </summary>        
        public async Task<List<LeaveTypesEmployee>> GetAllLeave()
        {
            var listLeave = await _leaveRepository.GetAllLeave();
            var result = _mapper.Map<List<LeaveTypesEmployee>>(listLeave);
            return result;
        }

        /// <summary>
        /// Logic to get timelogview 
        /// </summary>
        /// <param name="employeeId" ></param>
        public async Task<TimeLogView> GetTimeLogViewModel(int employeeId,int companyId)
        {
            var timeLogView = new TimeLogView();
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
            // Calculate Break Hours, Inside Office Time, and Total Hours directly in the method
            double totalBreakSeconds = 0;
            double totalInsideOfficeSeconds = 0;

            DateTime? lastClockOutTime = null;
            foreach (var entry in timeLogEntitys)
            {
                if (entry.EntryStatus == Constant.EntryTypeIn)
                {
                    // If there was a previous clock-out, calculate the break duration
                    if (lastClockOutTime.HasValue)
                    {
                        double breakDuration = (entry.ClockInTime - lastClockOutTime.Value).TotalSeconds;
                        if (breakDuration > 60) // Only count breaks longer than 1 minute
                        {
                            totalBreakSeconds += breakDuration;
                        }
                    }
                    // Add the log seconds for inside office time
                    totalInsideOfficeSeconds += entry.LogSeconds;
                }
                else if (entry.EntryStatus == Constant.EntryTypeOut)
                {
                    lastClockOutTime = entry.ClockOutTime; // Update last clock-out time
                }
            }

            // Calculate formatted break hours
            TimeSpan breakTime = TimeSpan.FromSeconds(totalBreakSeconds);
            string breakTimeFormatted = FormatTimeSpan(breakTime);

            // Calculate formatted inside office time
            TimeSpan officeTime = TimeSpan.FromSeconds(totalInsideOfficeSeconds);
            string officeTimeFormatted = FormatTimeSpan(officeTime);

            // Total hours calculation (inside office + breaks)
            TimeSpan totalTime = TimeSpan.FromSeconds(totalInsideOfficeSeconds + totalBreakSeconds);
            string totalTimeFormatted = FormatTimeSpan(totalTime);

            // Set time log view properties
            timeLogView.TotalHours = totalTimeFormatted;
            timeLogView.BreakHours = breakTimeFormatted;
            timeLogView.InsideOffice = officeTimeFormatted;
            return timeLogView;
        }
        private string FormatTimeSpan(TimeSpan ts)
        {
            return $"{(ts.Days * 24 + ts.Hours):00}:{ts.Minutes:00}:{ts.Seconds:00}";
        }
        public async Task<TimeLogView> GetTimeLog(int employeeId, DateTime date,int companyId)
        {
            var timeLogView = new TimeLogView();
            var timeLogEntitys = await _dashboardRepository.GetTimeLogEntityByEmpId(employeeId,date,companyId);
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
            // Calculate Break Hours, Inside Office Time, and Total Hours directly in the method
            double totalBreakSeconds = 0;
            double totalInsideOfficeSeconds = 0;

            DateTime? lastClockOutTime = null;
            foreach (var entry in timeLogEntitys)
            {
                if (entry.EntryStatus == Constant.EntryTypeIn)
                {
                    // If there was a previous clock-out, calculate the break duration
                    if (lastClockOutTime.HasValue)
                    {
                        double breakDuration = (entry.ClockInTime - lastClockOutTime.Value).TotalSeconds;
                        if (breakDuration > 60) // Only count breaks longer than 1 minute
                        {
                            totalBreakSeconds += breakDuration;
                        }
                    }
                    // Add the log seconds for inside office time
                    totalInsideOfficeSeconds += entry.LogSeconds;
                }
                else if (entry.EntryStatus == Constant.EntryTypeOut)
                {
                    lastClockOutTime = entry.ClockOutTime; // Update last clock-out time
                }
            }

            // Calculate formatted break hours
            TimeSpan breakTime = TimeSpan.FromSeconds(totalBreakSeconds);
            string breakTimeFormatted = FormatTimeSpan(breakTime);

            // Calculate formatted inside office time
            TimeSpan officeTime = TimeSpan.FromSeconds(totalInsideOfficeSeconds);
            string officeTimeFormatted = FormatTimeSpan(officeTime);

            // Total hours calculation (inside office + breaks)
            TimeSpan totalTime = TimeSpan.FromSeconds(totalInsideOfficeSeconds + totalBreakSeconds);
            string totalTimeFormatted = FormatTimeSpan(totalTime);

            // Set time log view properties
            timeLogView.TotalHours = totalTimeFormatted;
            timeLogView.BreakHours = breakTimeFormatted;
            timeLogView.InsideOffice = officeTimeFormatted;
            return timeLogView;
        }
        /// <summary>
        /// Logic to get projectdetails list
        /// </summary>
        /// <param name="topFiveProject" ></param>
        public async Task<List<string>> GetStringProjectNames(List<ProjectDetailsEntity> topFiveProject, int companyId)
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
        /// Logic to get timeloggerviewmodel inserttimelog
        /// </summary>
        /// <param name="timeLoggerViewModel" ></param>
        public async Task<bool> InsertTimeLog(TimeLoggerViewModel timeLoggerViewModel)
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
            return result;
        }

        /// <summary>
        /// Logic to get timelogview list
        /// </summary>
        /// <param name="timeLoggerViewModel" ></param>
        public async Task<TimeLogView> GetLoggedInTotalTimeLog(TimeLoggerViewModel timeLoggerViewModel)
        {
            var timeLogView = new TimeLogView();
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
                timeLogView.TimeClockPercentage = percentage;
                timeLogView.TimeOngoingTime = answer;
            }

            return timeLogView;
        }

        /// <summary>
        /// Logic to get profile 
        /// </summary>
        /// <param name="empTotalCount" ></param>
        /// <param name="allEmployeeProfiles" ></param>
        public string GetemployeeJoingPercentage(List<ProfileInfoEntity> allEmployeeProfiles)
        {
            //var totalEmpCount = await _employeesRepository.GetEmployeeMaxCount();
            var currenDate = DateTime.Now;
            var lastYear = currenDate.AddYears(-1);
            var fromDate = new DateTime(lastYear.Year, lastYear.Month, 1);
            var toDate = currenDate;
            var joingEmpCountInCurrentMonth = allEmployeeProfiles.Where(x => x.DateOfJoining >= fromDate && x.DateOfJoining <= toDate).Count();
            return joingEmpCountInCurrentMonth.ToString();
        }

        /// <summary>
        /// Logic to get project details 
        /// </summary>
        /// <param name="projectTotalCount" ></param>
        /// <param name="allProjectDetails" ></param>
        public string GetProjectPercentage(int projectTotalCount, List<ProjectDetailsEntity> allProjectDetails)
        {
            //var totalEmpCount = await _employeesRepository.GetEmployeeMaxCount();            
            var currenDate = DateTime.Now;
            var lastYear = currenDate.AddYears(-1);
            var fromDate = new DateTime(lastYear.Year, lastYear.Month, 1);
            var toDate = currenDate;
            var createdProjectCountInCurrentMonth = allProjectDetails.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate).Count();
            return createdProjectCountInCurrentMonth.ToString();
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
        /// Logic to get employee 
        /// </summary>
        /// <param name="activeEmployeesTotalCount" ></param>
        /// <param name="allActiveEmployeesDetails" ></param>
        public string GetActiveEmployeesPercentage(int activeEmployeesTotalCount, List<EmployeesEntity> allActiveEmployeesDetails)
        {
            //var totalEmpCount = await _employeesRepository.GetEmployeeMaxCount();            
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = DateTime.Now;
            var createdActiveEmployeesCountInCurrentMonth = allActiveEmployeesDetails.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate).Count();
            var percentage = createdActiveEmployeesCountInCurrentMonth == 0 ? 0 : (activeEmployeesTotalCount - createdActiveEmployeesCountInCurrentMonth / createdActiveEmployeesCountInCurrentMonth) * 100;
            return percentage.ToString(".");
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
        /// Logic to get employeeLeavecount list
        /// </summary>
        /// <param name="totalEmployeeLeaveView" ></param>
        public List<string> GetEmployeeLeaveByMonthInCount(TotalEmployeeLeaveView totalEmployeeLeaveView)
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
        /// Logic to get employee list
        /// </summary>
        /// <param name="allActiveEmployeesDetails" ></param>
        public List<string> GetActiveEmployeesByMonthInCount(List<EmployeesEntity> allActiveEmployeesDetails)
        {
            var stringLastTweleMonths = string.Empty;
            var stringLastTweleMonthProjectCount = string.Empty;
            var listOfActiveEmployeesString = new List<string>();
            var currentDate = DateTime.Now;
            List<DateTime> last12 = (from r in Enumerable.Range(0, 12) select currentDate.AddMonths(0 - r)).OrderBy(r => r).ToList();
            last12.ForEach(x =>
            {
                stringLastTweleMonths += $"{x:MMM}" + " " + x.Year + ",";

                var startDate = new DateTime(x.Year, x.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var projectCountOfMonth = allActiveEmployeesDetails.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).Count();

                stringLastTweleMonthProjectCount += projectCountOfMonth + ",";
            });
            listOfActiveEmployeesString.Add(stringLastTweleMonths.Trim(new char[] { ',' }));
            listOfActiveEmployeesString.Add(stringLastTweleMonthProjectCount.Trim(new char[] { ',' }));

            return listOfActiveEmployeesString;
        }

        /// <summary>
        /// Logic to get announcement 
        /// </summary>
        /// <param name="annuncementId" ></param>
        public async Task<Announcements> GetAnnouncementById(int annuncementId,int companyId)
        {
            var announcementEntity = await _masterRepository.GetAnnouncementById(annuncementId,companyId);
            var annuncerName = await _employeesRepository.GetEmployeeById(announcementEntity.CreatedBy, companyId);
            var announcement = new Announcements();
            announcement.AnnuncementId = announcementEntity.AnnouncementId;
            announcement.AnnouncementName = announcementEntity.AnnouncementName;
            announcement.Description = announcementEntity.Description;
            announcement.AnnouncerName = annuncerName.FirstName + " " + annuncerName.LastName;
            announcement.CreatedDate = announcementEntity.CreatedDate;
            announcement.AnnouncementDate = announcementEntity.AnnouncementDate.ToString(Constant.DateFormat);
            announcement.AnnouncementEndDate = announcementEntity.AnnouncementEndDate.ToString(Constant.DateFormat);
            announcement.CreatedBy = announcementEntity.CreatedBy;
            return announcement;
        }

    }
}
