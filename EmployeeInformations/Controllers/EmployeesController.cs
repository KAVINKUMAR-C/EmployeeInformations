using ClosedXML.Excel;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using PageSize = iTextSharp.text.PageSize;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class EmployeesController : BaseController
    {

        private readonly IEmployeesService _employeesService;
        private readonly IAssetService _assetservice;
        private readonly IBenefitService _benefitService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IReportService _reportService;

        public EmployeesController(IEmployeesService employeesService, IAssetService assetservice, IBenefitService benefitService, IHostingEnvironment hostingEnvironment, IReportService reportService)
        {
            _employeesService = employeesService;
            _assetservice = assetservice;
            _benefitService = benefitService;
            _hostingEnvironment = hostingEnvironment;
            _reportService = reportService;
        }

        /// <summary>
        /// Logic to get all the employee list
        /// </summary>

        //[HttpGet]
        //public async Task<IActionResult> EmployeesInfo()
        //{
        //    HttpContext.Session.SetString("LastView", Constant.EmployeesInfo);
        //    HttpContext.Session.SetString("LastController", Constant.Employees);
           // var employees = await _employeesService.GetAllEmployees();
        //    return View(employees);
        //}


        [HttpGet]
        public async Task<IActionResult> EmployeesInfo()
        {
            HttpContext.Session.SetString("LastView", Constant.EmployeesInfo);
            HttpContext.Session.SetString("LastController", Constant.Employees);
            var sessionCompanyId = GetSessionValueForCompanyId;
            var employees = await _employeesService.GetAllReleaveTypes(sessionCompanyId);
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult>GetAllEmployeesDetails (SysDataTablePager pager, string columnDirection, string ColumnName)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var employeesCount = await _employeesService.GetEmployeesListCount(pager, sessionCompanyId);
            var employees = await _employeesService.GetEmployeesDetailsList(pager, columnDirection, ColumnName, sessionCompanyId);
            return Json(new
            {
                data = employees,
                iTotalDisplayRecords = employeesCount,
                iTotalRecords = employeesCount
            });
        }

        /// <summary>
        /// Logic to get display the employeeinfo detail  by particular employee
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewEmployeeInfo(int empId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var employee = await _employeesService.GetEmployeeByEmployeeId(empId, sessionCompanyId);
            return View(employee);
        }

        //Employees Details

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var sessionCompanyId = GetSessionValueForCompanyId;
            var employee = new Employees();
            var userName = await _employeesService.GetEmployeeId(sessionCompanyId);
            employee.UserName = userName;
            employee.reportingPeople = await _employeesService.GetAllReportingPerson(sessionCompanyId);
            employee.Designations = await _employeesService.GetAllDesignation(sessionCompanyId);
            employee.Departments = await _employeesService.GetAllDepartment(sessionCompanyId);
            employee.RolesTables = await _employeesService.GetAllRoleTable(sessionCompanyId);
            return View(employee);
        }

        /// <summary>
            /// Logic to get create the employees 
            /// </summary>
        /// <param name="employees" ></param>

        [HttpPost]
        public async Task<Int32> CreateEmployee(Employees employees)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _employeesService.CreateEmployee(employees, sessionEmployeeId, sessionCompanyId);
            return result;
        }

        /// <summary>
        /// Logic to get edit the employee detail  by particular employee
        /// </summary>
        /// <param name="EmpId" >employee</param>

        [HttpGet]
        public IActionResult EditEmployees(int EmpId)
        {
            var employee = new Employees { EmpId = EmpId == 0 ? GetSessionValueForEmployeeId : EmpId };
            return PartialView("EditEmployees", employee);
        }

        /// <summary>
            /// Logic to get update the employees 
            /// </summary>
        /// <param name="employees" ></param>

        [HttpPost]
        public async Task<Int32> UpdateEmployee(Employees employees)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _employeesService.CreateEmployee(employees, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
            /// Logic to get update the employee detail  by particular employee
             /// </summary>
        /// <param name="EmpId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int EmpId)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var employees = await _employeesService.GetEmployeeById(EmpId, sessionEmployeeId);
            var skillSets = await _employeesService.GetAllSkills(companyId);
            employees.RolesTables = await _employeesService.GetAllRoleTable(companyId);
            employees.SkillSet = skillSets;
            return PartialView("_UpdateEmployee", employees);
        }

        /// <summary>
        /// Logic to get reject the employees detail  by particular employees
          /// </summary>
        /// <param name="employees" ></param>

        public async Task<IActionResult> GetRejectEmployees(Employees employees)
        {
            employees.CompanyId = GetSessionValueForCompanyId;
            var result = employees != null && await _employeesService.GetRejectEmployees(employees);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get display the employee detail  by particular employee
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewEmployee(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            empId = empId == 0 ? GetSessionValueForEmployeeId : empId;
            var employee = await _employeesService.GetEmployeeByEmployeeIdView(empId, companyId);
            return View(employee);
        }

        //Profile Info

        /// <summary>
        /// Logic to get create the profileInfo 
        /// </summary>
        /// <param name="empId" >profile</param>

        [HttpGet]
        public async Task<IActionResult> CreateProfile(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var profileInfo = await _employeesService.GetProfileByEmployeeId(empId, companyId);
            profileInfo.BloodGroups = await _employeesService.GetAllBloodGroup();
            profileInfo.RelationshipType = await _employeesService.GetAllRelationshipType();
            return PartialView("_PartialProfileInfo", profileInfo);
        }

        /// <summary>
        /// Logic to get add and update the profileInfo detail  by particular profileInfo
        /// </summary>
        /// <param name="profileInfo,file" ></param>

        [HttpPost]
        public async Task<int> AddProfileInfo(ProfileInfo profileInfo, IFormFile file)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            if (file != null && file.Name != "")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProfileImages");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var combinedPath = Path.Combine(path, fileName);
                profileInfo.ProfileFilePath = Path.Combine(path, file.FileName);
                profileInfo.ProfileName = fileName;
                // using (var stream = new FileStream(profileInfo.ProfileFilePath, FileMode.Create))
                using (var stream = new FileStream(combinedPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            var result = await _employeesService.AddProfileInfo(profileInfo, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get display the profileinfo detail  by particular profileinfo
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewProfileInfo(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var employee = await _employeesService.GetEmployeeProfileByEmployeeId(empId, companyId);
            return View(employee);
        }

        //Address Info

        /// <summary>
        /// Logic to get create the addressInfo 
        /// </summary>
        /// <param name="empId" >address</param>

        [HttpGet]
        public async Task<IActionResult> CreateAddress(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var addressInfo = await _employeesService.GetAddressByEmployeeId(empId,companyId);
            addressInfo.states = await _employeesService.GetByCountryId(addressInfo.CountryId);
            addressInfo.cities = await _employeesService.GetByStateId(addressInfo.StateId);
            addressInfo.Secondarystates = await _employeesService.GetByCountryId(addressInfo.SecondaryCountryId);
            addressInfo.Secondarycities = await _employeesService.GetByStateId(addressInfo.SecondaryStateId);
            addressInfo.country = await _employeesService.GetAllCountry();
            return PartialView("_PartialAddressInfo", addressInfo);
        }


        /// <summary>
        /// Logic to get add and update the addressInfo detail  by particular addressInfo
        /// </summary>
        /// <param name="addressInfo" ></param>

        [HttpPost]
        public async Task<int> AddAddressInfo(AddressInfo addressInfo)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _employeesService.AddAddressInfo(addressInfo, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get add and update the address detail  by particular address
        /// </summary>
        /// <param name="countryId" >address</param>

        [HttpGet]
        public async Task<IActionResult> GetByCountryId(int countryId)
        {
            var result = await _employeesService.GetByCountryId(countryId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get add and update the address detail  by particular address
        /// </summary>
        /// <param name="StateId" >address</param>

        [HttpGet]
        public async Task<IActionResult> GetByStateId(int StateId)
        {
            var result = await _employeesService.GetByStateId(StateId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get display the addressinfo detail  by particular addressinfo
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewAddressInfo(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var addressInfo = await _employeesService.GetEmployeeAddressByEmployeeId(empId,companyId);
            return View(addressInfo);
        }

        //Other Detail

        /// <summary>
        /// Logic to get create the otherDetails 
        /// </summary>
        /// <param name="empId" >otherDetails</param>

        [HttpGet]
        public async Task<IActionResult> CreateOtherDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            // var otherDetails = await _employeesService.GetOtherDetailsByEmployeeId(empId);
            var otherDetails = await _employeesService.GetAllOtherDetailsViewModel(empId,companyId);
            var getAllDocumentTypes = await _employeesService.GetAllDocumentTypes(companyId);
            otherDetails.documentTypes = getAllDocumentTypes;
            otherDetails.EmpId = empId;
            return PartialView("_PartialOtherDetails", otherDetails);
        }

        /// <summary>
        /// Logic to get add the otherDetails detail  by particular otherDetails
        /// </summary>
        /// <param name="otherDetails,file" ></param>

        [HttpPost]
        public async Task<IActionResult> AddOthersdetails(OtherDetails otherDetails, ICollection<IFormFile> file)
        {
            var result = new OtherDetailsViewModel();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            otherDetails.OtherDetailsAttachments = new List<OtherDetailsAttachments>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var otherDetailsAttachments = new OtherDetailsAttachments();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/OtherDetails");
                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    otherDetailsAttachments.Document = path.Replace(path, "~/OtherDetails/") + fileName;
                    otherDetailsAttachments.DocumentName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    otherDetails.OtherDetailsAttachments.Add(otherDetailsAttachments);
                }
            }
            if (otherDetails != null)
            {
                var output = await _employeesService.InsertAndUpdateOtherDetails(otherDetails, sessionEmployeeId);
                result = await _employeesService.GetAllOtherDetailsViewModel(otherDetails.EmpId,otherDetails.CompanyId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get filepath the ortherdetails detail  by particular detailId
        /// </summary>
        /// <param name="detailId" ></param>
        public async Task<List<OtherDetailsDocumentFilePath>> GetOtherDetailsDocumentAndFilePath(int detailId)
        {
            var docNmaes = await _employeesService.GetOtherDetailsDocumentAndFilePath(detailId);
            return docNmaes;
        }

        /// <summary>
        /// Logic to get update the otherDetails detail  by particular otherDetails
        /// </summary>
        /// <param name="otherDetails,file" ></param>

        [HttpPost]
        public async Task<IActionResult> UpdateOtherDetails(OtherDetails otherDetails, ICollection<IFormFile> file)
        {
            var result = new OtherDetailsViewModel();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            otherDetails.OtherDetailsAttachments = new List<OtherDetailsAttachments>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var otherDetailsAttachments = new OtherDetailsAttachments();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/OtherDetails");
                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    otherDetailsAttachments.Document = path.Replace(path, "~/OtherDetails/") + fileName;
                    otherDetailsAttachments.DocumentName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    otherDetails.OtherDetailsAttachments.Add(otherDetailsAttachments);
                }
            }
            if (otherDetails != null)
            {
                var output = await _employeesService.InsertAndUpdateOtherDetails(otherDetails, sessionEmployeeId);
                result = await _employeesService.GetAllOtherDetailsViewModel(otherDetails.EmpId,otherDetails.CompanyId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get delete the otherDetails detail  by particular otherDetails
        /// </summary>
        /// <param name="otherDetails" ></param>

        [HttpPost]
        public async Task<IActionResult> DeleteOtherDetails(OtherDetails otherDetails)
        {
            var result = new OtherDetailsViewModel();
            if (otherDetails != null)
            {
                await _employeesService.DeleteOtherDetails(otherDetails);
                result = await _employeesService.GetAllOtherDetailsViewModel(otherDetails.EmpId, otherDetails.CompanyId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get filename the otherdetails detail  by particular detailId
        /// </summary>
        /// <param name="detailId" ></param>
        public async Task<List<string>> GetOtherDetailsDocument(int detailId)
        {
            var docNmaes = await _employeesService.GetDocumentNameByDetailId(detailId);
            return docNmaes;
        }

        /// <summary>
        /// Logic to get download the otherdetailsdocument detail  by particular otherdetails
          /// </summary>
        /// <param name="detailId" >otherdetails</param>

        public async Task<FileResult> DownloadOtherDetailsFile(int detailId)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var docNmaes = await _employeesService.GetOtherDetailsDocumentAndFilePath(detailId);
            var empUserName = string.Empty;
            if (docNmaes.Count() > 0)
            {
                var empId = docNmaes.FirstOrDefault()?.EmpId ?? 0;
                var getUserName = await _employeesService.GetEmployeeById(empId, sessionEmployeeId);
                empUserName = getUserName.UserName;
            }
            if (docNmaes.Count() == 1)
            {
                foreach (var item in docNmaes)
                {
                    string path = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                    var bytes = System.IO.File.ReadAllBytes(path);
                    var file = File(bytes, "application/octet-stream", item.Document);
                    file.FileDownloadName = empUserName + "_" + item.DocumentName;
                    return file;
                }
            }
            else
            {
                var zipName = empUserName + "_" + $"archive-OtherDetailsFiles-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in docNmaes)
                        {
                            string fPath = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                            var entry = archive.CreateEntry(System.IO.Path.GetFileName(fPath), CompressionLevel.Fastest);
                            using (var zipStream = entry.Open())
                            {
                                var bytes = System.IO.File.ReadAllBytes(fPath);
                                zipStream.Write(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    return File(ms.ToArray(), "application/zip", zipName);
                }
            }
            return null;

        }

        /// <summary>
        /// Logic to get display the otherdetails detail  by particular otherdetails
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewOtherDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var otherDetailsViewModel = await _employeesService.GetAllOtherDetailsView(empId,companyId);
            return View(otherDetailsViewModel);
        }

        //Qualification Details

        /// <summary>
        /// Logic to get create the qualification 
        /// </summary>
        /// <param name="empId" >qualification</param>

        [HttpGet]
        public async Task<IActionResult> CreateQualification(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var qualification = await _employeesService.GetAllQulificationViewModel(empId, companyId);
            return PartialView("_PartialQualificationInfo", qualification);
        }

        public FileResult DownloadFiles(string Document, string QualificationName)
        {

            string path = Path.GetFullPath(Document);
            var bytes = System.IO.File.ReadAllBytes(path);
            var file = File(bytes, "application/octet-stream", Document);
            file.FileDownloadName = QualificationName;
            return file;
        }

        /// <summary>
        /// Logic to get add the qualification detail  by particular qualification
        /// </summary>
        /// <param name="qualification,file" ></param>

        [HttpPost]
        public async Task<IActionResult> AddQualification(Qualification qualification, ICollection<IFormFile> file)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = new List<Qualification>();
            qualification.QualificationAttachments = new List<QualificationAttachment>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var qualificationAttachment = new QualificationAttachment();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Qualifications");
                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    qualificationAttachment.Document = path.Replace(path, "~/Qualifications/") + fileName;
                    qualificationAttachment.QualificationName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    qualification.QualificationAttachments.Add(qualificationAttachment);
                }
            }
            if (qualification != null)
            {
                var output = await _employeesService.InsertAndUpdateQualification(qualification, sessionEmployeeId,companyId);
                result = await _employeesService.GetAllQualification(qualification.EmpId, companyId);
            }
            return new JsonResult(result);
        }


        /// <summary>
        /// Logic to get update the qualification detail  by particular qualification
        /// </summary>
        /// <param name="qualification,file" ></param>

        [HttpPost]
        public async Task<IActionResult> UpdateQualification(Qualification qualifications, ICollection<IFormFile> file)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = new List<Qualification>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            qualifications.QualificationAttachments = new List<QualificationAttachment>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var qualificationAttachment = new QualificationAttachment();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Qualifications");

                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    qualificationAttachment.Document = path.Replace(path, "~/Qualifications/") + fileName;
                    qualificationAttachment.QualificationName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    qualifications.QualificationAttachments.Add(qualificationAttachment);
                }
            }
            if (qualifications != null)
            {
                var output = await _employeesService.InsertAndUpdateQualification(qualifications, sessionEmployeeId,companyId);
                if (output)
                {
                    result = await _employeesService.GetAllQualification(qualifications.EmpId, companyId);
                }
            }

            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get filename the qualification detail  by particular qualificationId
        /// </summary>
        /// <param name="qualificationId" ></param>
        public async Task<List<string>> GetQualificationDocument(int qualificationId)
        {
            var docNmaes = await _employeesService.GetDocumentNameByQualificationId(qualificationId);
            return docNmaes;
        }


        /// <summary>
        /// Logic to get filepath the qualification detail  by particular qualificationId
        /// </summary>
        /// <param name="qualificationId" ></param>
        public async Task<List<QualificationDocumentFilePath>> GetQualificationDocumentAndFilePath(int qualificationId)
        {
            var docNmaes = await _employeesService.GetQualificationDocumentAndFilePath(qualificationId);
            return docNmaes;
        }

        /// <summary>
        /// Logic to get filename the ortherdetails detail  by particular detailId
        /// </summary>
        /// <param name="detailId" ></param>
        public async Task<List<string>> GetDocumentNameByDetailId(int detailId)
        {
            var docNmaes = await _employeesService.GetDocumentNameByDetailId(detailId);
            return docNmaes;
        }

        /// <summary>
        /// Logic to get delete the qualification detail  by particular qualification
            /// </summary>
        /// <param name="qualification" ></param>

        [HttpPost]
        public async Task<IActionResult> DeleteQualification(Qualification qualification)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = new List<Qualification>();
            if (qualification != null)
            {
                await _employeesService.DeleteQualification(qualification);
                result = await _employeesService.GetAllQualification(qualification.EmpId, companyId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get download the qualificationdocument detail  by particular qualification
          /// </summary>
        /// <param name="qualificationId" >qualification</param>

        public async Task<FileResult> DownloadQualificationFile(int qualificationId)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var docNmaes = await _employeesService.GetQualificationDocumentAndFilePath(qualificationId);
            var empUserName = string.Empty;
            if (docNmaes.Count() > 0)
            {
                var empId = docNmaes.FirstOrDefault()?.EmpId ?? 0;
                var getUserName = await _employeesService.GetEmployeeById(empId, sessionEmployeeId);
                empUserName = getUserName.UserName;
            }
            if (docNmaes.Count() == 1)
            {
                foreach (var item in docNmaes)
                {
                    string path = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                    var bytes = System.IO.File.ReadAllBytes(path);
                    var file = File(bytes, "application/octet-stream", item.Document);
                    file.FileDownloadName = empUserName + "_" + item.QualificationName;
                    return file;
                }
            }
            else
            {
                var zipName = empUserName + "_" + $"archive-QualificationFiles-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in docNmaes)
                        {
                            string fPath = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                            var entry = archive.CreateEntry(System.IO.Path.GetFileName(fPath), CompressionLevel.Fastest);
                            using (var zipStream = entry.Open())
                            {
                                var bytes = System.IO.File.ReadAllBytes(fPath);
                                zipStream.Write(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    return File(ms.ToArray(), "application/zip", zipName);
                }
            }
            return null;

        }

        /// <summary>
        /// Logic to get display the qualification detail  by particular qualification
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewQualification(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var qualificationViewModel = await _employeesService.GetAllQulificationView(empId, companyId);
            return View(qualificationViewModel);
        }

        //Experience Details

        /// <summary>
           /// Logic to get create the experience 
               /// </summary>
        /// <param name="empId" >experience</param>

        [HttpGet]
        public async Task<IActionResult> Createexperience(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var experience = await _employeesService.GetAllExperienceViewModel(empId,companyId);
            return PartialView("_PartialExperience", experience);
        }

        /// <summary>
        /// Logic to get add the experience detail  by particular experience
        /// </summary>
        /// <param name="experience,file" ></param>

        [HttpPost]
        public async Task<IActionResult> AddExperience(Experience experience, ICollection<IFormFile> file)
        {
            var result = new List<Experience>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            experience.ExperienceAttachment = new List<ExperienceAttachment>();

            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var experienceAttachment = new ExperienceAttachment();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Experience");

                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    experienceAttachment.Document = path.Replace(path, "~/Experience/") + fileName;
                    experienceAttachment.ExperienceName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    experience.ExperienceAttachment.Add(experienceAttachment);
                }
            }
            if (experience != null)
            {
                var output = await _employeesService.InsertAndUpdateExperience(experience, sessionEmployeeId);
                result = await _employeesService.GetAllExperience(experience.EmpId,experience.CompanyId);
            }
            return new JsonResult(result);

        }

        /// <summary>
        /// Logic to get experience document path by particular experience
        /// </summary>
        /// <param name="experienceId" ></param>
        public async Task<List<string>> GetExperienceDocument(int experienceId)
        {
            var docNmaes = await _employeesService.GetDocumentNameByExperienceId(experienceId);
            return docNmaes;
        }

        /// <summary>
        /// Logic to get experience document path by particular experience
        /// </summary>
        /// <param name="experienceId" ></param>
        public async Task<List<ExperienceDocumentFilePath>> GetExperienceDocumentAndFilePath(int experienceId)
        {
            var docNmaes = await _employeesService.GetExperienceDocumentAndFilePath(experienceId);
            return docNmaes;
        }

        /// <summary>
        /// Logic to get update the experience detail  by particular experience
        /// </summary>
        /// <param name="experience,file" ></param>

        [HttpPost]
        public async Task<IActionResult> UpdateExperience(Experience experience, ICollection<IFormFile> file)
        {
            var result = new List<Experience>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            experience.ExperienceAttachment = new List<ExperienceAttachment>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var experienceAttachment = new ExperienceAttachment();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Experience");
                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    experienceAttachment.Document = path.Replace(path, "~/Experience/") + fileName;
                    experienceAttachment.ExperienceName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    experience.ExperienceAttachment.Add(experienceAttachment);
                }
            }
            if (experience != null)
            {
                var output = await _employeesService.InsertAndUpdateExperience(experience, sessionEmployeeId);
                if (output)
                {
                    result = await _employeesService.GetAllExperience(experience.EmpId,experience.CompanyId);
                }
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get delete the experience detail  by particular experience
        /// </summary>
        /// <param name="experience" ></param>

        [HttpPost]
        public async Task<IActionResult> DeleteExperience(Experience experience)
        {
            var result = new List<Experience>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            if (experience != null)
            {
                await _employeesService.DeleteExperience(experience, sessionEmployeeId);
                result = await _employeesService.GetAllExperience(experience.EmpId,experience.CompanyId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get download the experiencedocument detail  by particular experience
        /// </summary>
        /// <param name="experienceId" >experience</param>

        public async Task<FileResult> DownloadExperienceFile(int experienceId)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var docNmaes = await _employeesService.GetExperienceDocumentAndFilePath(experienceId);
            var empUserName = string.Empty;
            if (docNmaes.Count() > 0)
            {
                var empId = docNmaes.FirstOrDefault()?.EmpId ?? 0;
                var getUserName = await _employeesService.GetEmployeeById(empId, sessionEmployeeId);
                empUserName = getUserName.UserName;
            }
            if (docNmaes.Count() == 1)
            {
                foreach (var item in docNmaes)
                {
                    string path = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                    var bytes = System.IO.File.ReadAllBytes(path);
                    var file = File(bytes, "application/octet-stream", item.Document);
                    file.FileDownloadName = empUserName + "_" + item.ExperienceName;
                    return file;
                }
            }
            else
            {
                var zipName = empUserName + "_" + $"archive-ExperienceFiles-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in docNmaes)
                        {
                            string fPath = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                            var entry = archive.CreateEntry(System.IO.Path.GetFileName(fPath), CompressionLevel.Fastest);
                            using (var zipStream = entry.Open())
                            {
                                var bytes = System.IO.File.ReadAllBytes(fPath);
                                zipStream.Write(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    return File(ms.ToArray(), "application/zip", zipName);
                }
            }
            return null;
        }

        /// <summary>
        /// Logic to get display the experience detail  by particular experience
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewExperience(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var experienceViewModel = await _employeesService.GetAllExperienceView(empId,companyId);
            return View(experienceViewModel);
        }

        //Bank Details

        /// <summary>
        /// Logic to get create the bankDetails 
        /// </summary>
        /// <param name="empId" >bankDetails</param>

        [HttpGet]
        public async Task<IActionResult> CreateBankDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var bankDetails = await _employeesService.GetBankDetailsByEmployeeId(empId,companyId);
            return PartialView("_PartialBankDetails", bankDetails);
        }


        /// <summary>
        /// Logic to get add and update the bankDetails detail  by particular bankDetails
          /// </summary>
        /// <param name="bankDetails" ></param>

        [HttpPost]
        public async Task<int> AddBankDetails(BankDetails bankDetails)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            bankDetails.CompanyId = GetSessionValueForCompanyId;
            var result = await _employeesService.AddBankDetails(bankDetails, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get display the bankdetails detail  by particular bankdetails
        /// </summary>
        /// <param name="empId" >employee</param>
        [HttpGet]
        public async Task<IActionResult> ViewBankDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var bankDetails = await _employeesService.GetBankDetailsByEmployeeId(empId,companyId);
            return View(bankDetails);
        }

        //DownLoads
        public FileResult DownloadFile(string Document, string AttachmentName)
        {
            string path = Path.GetFullPath(Document);
            //Read the File data into Byte Array.
            var bytes = System.IO.File.ReadAllBytes(path);
            var file = File(bytes, "application/octet-stream", Document);
            file.FileDownloadName = AttachmentName;
            //Send the File to Download.
            return file;
        }
        public FileResult DownloadedFiled(string Document, string DocumentName)
        {
            string path = Path.GetFullPath(Document);
            var bytes = System.IO.File.ReadAllBytes(path);
            var file = File(bytes, "application/octet-stream", Document);
            file.FileDownloadName = DocumentName;
            return file;
        }

        //Probation

        /// <summary>
        /// Logic to get probationaccept the employees detail  by particular employees
        /// </summary>
        /// <param name="employees" ></param>

        [HttpPost]
        public async Task<IActionResult> AcceptProbation(Employees employees,int companyId)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var sessionCompanyId = GetSessionValueForCompanyId;
            var data = false;
            if (employees != null)
            {
                data = await _employeesService.AcceptProbation(employees, sessionEmployeeId, sessionCompanyId);
            }
            return new JsonResult(data);
        }

        //Password Details

        /// <summary>
        /// Logic to get employee change password
        /// </summary>

        [AllowAnonymous]
        [HttpGet]
        public IActionResult EmployeeChangePassword()
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var changePasswordViewModel = new ChangePasswordViewModel();
            changePasswordViewModel.EmpId = sessionEmployeeId;
            return View(changePasswordViewModel);
        }

        /// <summary>
        /// Logic to get employee current password
        /// </summary>
        [HttpPost]
        public async Task<bool> GetChangePassword(int empId, string CurrentPassword)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _employeesService.GetEmpId(empId, CurrentPassword, companyId);
            return result;
        }

        /// <summary>
        /// Logic to update employee current password
        /// </summary>
        [HttpPost]
        public async Task UpdateEmployeeCurrentPassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            await _employeesService.UpdateEmployeeCurrentPassword(changePasswordViewModel, companyId);
        }

        /// <summary>
        /// Logic to get officeEmail count  the employees detail 
        /// </summary>   
        /// <param name="officeEmail" >employees</param> 
        [HttpPost]
        public async Task<int> GetEmailCount(string officeEmail)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var officeEmailCount = await _employeesService.GetEmployeeEmail(officeEmail, sessionCompanyId);
            return officeEmailCount;
        }

        /// <summary>
        /// Logic to get isactive personalEmail count  the employees detail by particular employees
        /// </summary>   
        /// <param name="personalEmail" >employees</param> 
        /// <param name="empId" >employees</param> 
        [HttpPost]
        public async Task<int> GetPersonalCount(string personalEmail, int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var personalEmailCount = await _employeesService.GetPersonalEmail(personalEmail, empId, companyId);
            return personalEmailCount;
        }

        //Skill

        /// <summary>
        /// Logic to get update the skillset detail  by particular skillset
        /// </summary>
        /// <param name="employees" ></param>

        [HttpPost]
        public async Task<IActionResult> UpdateSkill(Employees employees)
        {
            var result = await _employeesService.UpdateSkill(employees);
            return new JsonResult(result);
        }
        /// <summary>
        /// Logic to get isverified the employees detail  by particular employees
        /// </summary>
        /// <param name="empId" >employees</param>

        [HttpPost]
        public async Task<IActionResult> EmployeeIsverified(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _employeesService.employeeIsverified(empId, companyId);
            return new JsonResult(result);
        }

        //Employee ChangeLog
        
        /// <summary>
        /// Logic to get employeeschangelog list
        /// </summary> 
        /// <param name="employeeChangeLogViewModel "  ></param>
        public async Task<IActionResult> EmployeesChangeLog(EmployeeChangeLogViewModel employeeChangeLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            employeeChangeLogViewModel.ReportingPeople = await _employeesService.GetAllEmployeesDrropdown(companyId);
            employeeChangeLogViewModel.EmployeeDetails = await _employeesService.GetAllEmployeeDetailsDrropdown(companyId);
            return View(employeeChangeLogViewModel);
        }

        [HttpGet]
        /// <summary>
        /// Logic to get filter the employeeschangelog details  
        /// </summary>
        /// <param name="employeeChangeLogViewModel ,pager"  ></param>
        public async Task<IActionResult> EmployeesChangeLogFilter(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var employeesChangeLog = await _employeesService.GetAllEmployeesChangeLogs(pager, employeeChangeLogViewModel, companyId);
            var cntEmp = await _employeesService.GetAllEmployessByEmployeeLogCount(pager, employeeChangeLogViewModel, companyId);
            return Json(new
            {
                data = employeesChangeLog.EmployeeLogs,
                iTotalRecords = cntEmp,
                iTotalDisplayRecords = cntEmp
            });
        }

        /// <summary>
        /// Logic to get filter the employeeschangelog details  
        /// </summary>
        /// <param name="employeeChangeLogViewModel,pager" ></param>
        [HttpGet]
        public async Task<IActionResult> FilterEmployeeLog(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var attendanceList = await _employeesService.GetAllEmployeesByEmployeeLogFilter(pager, employeeChangeLogViewModel, companyId);
            var cntEmp = await _employeesService.GetAllEmployessByEmployeeLogCount(pager, employeeChangeLogViewModel, companyId);
            return Json(new
            {
                data = attendanceList.EmployeeLogs,
                iTotalRecords = cntEmp,
                iTotalDisplayRecords = cntEmp
            });
        }

        /// <summary>
        /// Logic to get DownloadExcel the employeeChangeLogViewModel detail  by particular employeereport
        /// </summary>
        /// <param name="fileGuid" ></param>
        public async Task<string> DownloadExcel(EmployeeChangeLogViewModel employeeChangeLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var fileId = "";
            var attendanceList = await _employeesService.GetAllEmployessByEmployeeLogFilter(employeeChangeLogViewModel, companyId);
            if (attendanceList.EmployeesLog.Count() > 0)
            {
                if (employeeChangeLogViewModel.EmpId == 0)
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("EmployeeLog Details");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = Constant.FieldName;
                    worksheet.Cell(currentRow, 2).Value = Constant.PreviousValue;
                    worksheet.Cell(currentRow, 3).Value = Constant.NewValue;
                    worksheet.Cell(currentRow, 4).Value = Constant.Event;
                    worksheet.Cell(currentRow, 5).Value = Constant.AuthorName;
                    worksheet.Cell(currentRow, 6).Value = Constant.TimeStamp;

                    foreach (var user in attendanceList.EmployeesLog)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = user.FieldName;
                        worksheet.Cell(currentRow, 2).Value = user.PreviousValue;
                        worksheet.Cell(currentRow, 3).Value = user.NewValue;
                        worksheet.Cell(currentRow, 4).Value = user.Event;
                        worksheet.Cell(currentRow, 5).Value = user.AuthorName;
                        worksheet.Cell(currentRow, 6).Value = user.CreatedDate;

                    }
                    var fileName = string.Format("EmployeelogDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
                    fileId = Guid.NewGuid().ToString() + Constant.Hyphen + fileName;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        var content = memoryStream.ToArray();
                        HttpContext.Session.Set(Constant.fileId, content);
                    }
                    return fileId;
                }
                else
                {
                    var employeeDetails = await _employeesService.GetEmployeeById(employeeChangeLogViewModel.EmpId, companyId);
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add(employeeDetails.UserName + "_Attendance Details");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = Common.Constant.FieldName;
                    worksheet.Cell(currentRow, 2).Value = Common.Constant.PreviousValue;
                    worksheet.Cell(currentRow, 3).Value = Common.Constant.NewValue;
                    worksheet.Cell(currentRow, 4).Value = Common.Constant.Event;
                    worksheet.Cell(currentRow, 5).Value = Common.Constant.AuthorName;
                    worksheet.Cell(currentRow, 6).Value = Common.Constant.TimeStamp;

                    foreach (var user in attendanceList.EmployeesLog)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = user.FieldName;
                        worksheet.Cell(currentRow, 2).Value = user.PreviousValue;
                        worksheet.Cell(currentRow, 3).Value = user.NewValue;
                        worksheet.Cell(currentRow, 4).Value = user.Event;
                        worksheet.Cell(currentRow, 5).Value = user.AuthorName;
                        worksheet.Cell(currentRow, 6).Value = user.CreatedDate;

                    }
                    var fileName = string.Format(employeeDetails.UserName + "_EmployeelogDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
                    fileId = Guid.NewGuid().ToString() + "_" + fileName;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        var content = memoryStream.ToArray();
                        HttpContext.Session.Set(Constant.fileId, content);
                    }
                    return fileId;
                }
            }
            return fileId;
        }


        /// <summary>
        /// Logic to get download the employeeschangelog details  
        /// </summary>
        /// <param name="fileGuid" ></param>

        [HttpGet]
        public virtual ActionResult Download(string fileGuid)
        {
            var fileName = string.Format("EmployeeLogDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                //HttpContext.Session.Set(Constant.fileId,null);
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                // redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Logic to get create pdffile the employeeschangelog details  
        /// </summary>
        /// <param name="employeeChangeLogViewModel" ></param>
        [HttpPost]
        public async Task<string> CreatePdf(EmployeeChangeLogViewModel employeeChangeLogViewModel)
        {

            var companyId = GetSessionValueForCompanyId;
            var employees = await _employeesService.GetAllEmployessByEmployeeLogFilter(employeeChangeLogViewModel, companyId);
            if (employees.EmployeesLog.Count > 0)
            {

                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                MemoryStream PDFData = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                Rectangle pageSize = writer.PageSize;
                // Open the Document for writing
                pdfDoc.Open();
                //Add elements to the document here

                // Create the header table 
                PdfPTable headertable = new PdfPTable(3);
                headertable.HorizontalAlignment = 0;
                headertable.WidthPercentage = 100;
                headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

                // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
                headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
                string webRootPath = _hostingEnvironment.WebRootPath;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                logo.ScaleToFit(100, 70);

                {
                    PdfPCell pdfCelllogo = new PdfPCell(logo);
                    pdfCelllogo.Border = Rectangle.NO_BORDER;
                    pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    pdfCelllogo.BorderWidthBottom = 1f;
                    pdfCelllogo.PaddingTop = 10f;
                    pdfCelllogo.PaddingBottom = 10f;
                    headertable.AddCell(pdfCelllogo);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    middlecell.BorderWidthBottom = 1f;
                    headertable.AddCell(middlecell);
                }

                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee Log Report - " + employeeChangeLogViewModel.StartDate + " " + "To" + " " + employeeChangeLogViewModel.EndDate, titleFont));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nextPostCell1.PaddingBottom = 20f;
                    nested.AddCell(nextPostCell1);

                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 6;
                    nesthousing.PaddingTop = 10f;
                    headertable.AddCell(nesthousing);
                }

                PdfPTable Invoicetable = new PdfPTable(3);
                Invoicetable.HorizontalAlignment = 0;
                Invoicetable.WidthPercentage = 100;
                Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
                Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(middlecell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.PaddingTop = 20f;
                    Invoicetable.AddCell(middlecell);
                }

                pdfDoc.Add(headertable);
                pdfDoc.Add(Invoicetable);

                //Create body table
                PdfPTable itemTable = new PdfPTable(6);
                float[] headers = { 45, 58, 55, 58, 52, 55 }; //Header Widths  
                itemTable.SetWidths(headers);

                //Add header
                AddCellToHeader(itemTable, "FieldName");
                AddCellToHeader(itemTable, "PreviousValue");
                AddCellToHeader(itemTable, "NewValue");
                AddCellToHeader(itemTable, "Event");
                AddCellToHeader(itemTable, "AuthorName");
                AddCellToHeader(itemTable, "CreatedDate");

                foreach (var emp in employees.EmployeesLog)
                {

                    AddCellToBody(itemTable, emp.FieldName);
                    AddCellToBody(itemTable, emp.PreviousValue);
                    AddCellToBody(itemTable, emp.NewValue);
                    AddCellToBody(itemTable, emp.Event);
                    AddCellToBody(itemTable, emp.AuthorName);
                    AddCellToBody(itemTable, emp.CreatedDate.ToString());

                }
                pdfDoc.Add(itemTable);

                PdfContentByte cb = new PdfContentByte(writer);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                cb = new PdfContentByte(writer);
                cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.EndText();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                cb.Stroke();
                string strPDFFileName = string.Format("EmployeeLogReport " + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.pdf);
                pdfDoc.Close();
                var content = PDFData.ToArray();
                HttpContext.Session.Set(Constant.fileId, content);
                var fileId = Guid.NewGuid().ToString() + "_" + strPDFFileName;
                return fileId;
            }
            return "";
        }

        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 10, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = WebColors.GetRGBColor("#1a76d1")
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });
        }


        /// <summary>
        /// Logic to get download pdf the employeeschangelog details  
        /// </summary>
        /// <param name="fileGuid" ></param>

        [HttpGet]
        public virtual ActionResult DownloadPdf(string fileGuid)
        {
            var fileName = string.Format("EmployeeLogReport_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.pdf);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                return File(data, "pdf/application", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        // View Employee Profile 

        /// <summary>
        /// Logic to get display the asset detail  by particular asset
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewAssets(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var assetsDetails = await _assetservice.GetAssetByEmployeeId(empId, companyId);
            return View(assetsDetails);
        }

        /// <summary>
        /// Logic to get display the benefit detail  by particular benefit
        /// </summary>
        /// <param name="empId" >employee</param>

        [HttpGet]
        public async Task<IActionResult> ViewBenefits(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var benefitsDetails = await _benefitService.GetEmployeeBenefitsByEmployeeId(empId, companyId);
            return View(benefitsDetails);
        }

        /// <summary>
        /// Logic to get all list salary details
        /// </summary>
        /// <param name="salarys" ></param>
        [HttpGet]
        public async Task<IActionResult> GetAllSalaryByEmpId(int empId)
        {           
            var salarys = await _employeesService.GetAllSalaryByEmpId(empId);
            salarys.Years = await _reportService.GetYear();
            //salarys.EmpId = empId;
            return PartialView("_PartialSalary", salarys);
        }

        /// <summary>
        /// Logic to get create and update the salary details
        /// </summary>
        /// <param name="salarys" ></param>
        [HttpPost]
        public async Task<IActionResult> AddSalaryDetails(salarys salarys)
        {
            var result = new List<salarys>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var salary = await _employeesService.AddSalaryDetails(salarys, sessionEmployeeId,companyId);
            result = await _employeesService.GetSalaryByEmpId(salary);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get count check the salary detail by particular salarys
        /// </summary>
        /// <param name="month,empId,year" ></param>
        [HttpPost]
        public async Task<int> GetBySalaryCount(int month,int empId,int year)
        {            
            var salaryMonthCount = await _employeesService.GetBySalaryCount(month, empId, year);           
            return salaryMonthCount;
        }       

        /// <summary>
        /// Logic to get delete the salary detail by particular salarys
        /// </summary>
        /// <param name="salarys" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteSalary(salarys salarys)
        {
            var result = new List<salarys>();
            if (salarys != null)
            {
                await _employeesService.DeleteSalary(salarys);
                result = await _employeesService.GetSalaryByEmpId(salarys.EmpId);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get salary detail by particular employees
        /// </summary>
        /// <param name="empId" ></param>
        [HttpGet]
        public async Task<IActionResult> ViewSalary(int empId)
        {
            var experienceViewModel = await _employeesService.GetAllSalaryByEmpId(empId);
            return View(experienceViewModel);
        }
    }

}
