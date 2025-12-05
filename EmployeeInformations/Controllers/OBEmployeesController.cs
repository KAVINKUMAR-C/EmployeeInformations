using EmployeeInformations.Business.IService;
using EmployeeInformations.Model.OnboardingViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace EmployeeInformations.Controllers
{
    public class OBEmployeesController : BaseController
    {
        public readonly IOBEmployeeService _OBEmployeeService;

        public OBEmployeesController(IOBEmployeeService oBEmployeeService)
        {
            _OBEmployeeService = oBEmployeeService;
        }

        

        [HttpGet]
        public async Task<IActionResult> OBEmployeeInfo()
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var details = await _OBEmployeeService.GetEmployee(sessionCompanyId);
            return View(details);
        }

        public async Task<IActionResult>InitialPage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>WelcomeAboard()
        {
            var empId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var viewModel = await _OBEmployeeService.GetEmployee(empId, companyId);
            return View(viewModel);
        }

        [HttpGet]
         public async Task<IActionResult> CreateEmployees()
        {
            var companyId = GetSessionValueForCompanyId;
            var employee = new OBEmployees();
            var userName = await _OBEmployeeService.GetEmployeeId(companyId);
            employee.UserName = userName;
            employee.reportingPeople = await _OBEmployeeService.GetAllReportingPerson(companyId);
            employee.Designations = await _OBEmployeeService.GetAllDesignation(companyId);
            employee.Departments = await _OBEmployeeService.GetAllDepartment(companyId);
            employee.RolesTables = await _OBEmployeeService.GetAllRoleTable(companyId);
            return View(employee);
        }

        [HttpGet]
        public IActionResult EditEmployees()
        {
            var employee = new OBEmployees();
             var EmpId = GetSessionValueForEmployeeId;
            employee.EmpId = EmpId;
            return PartialView("EditOBEmployees", employee);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int EmpId)
        {
            var companyId = GetSessionValueForCompanyId;
            var employees = await _OBEmployeeService.GetEmployeeById(EmpId,companyId);
            var skillSets = await _OBEmployeeService.GetAllSkills(companyId);
            employees.SkillSet = skillSets;
            return PartialView("_PartialUpdateOBEmployee", employees);
        }
        [HttpPost]
        public async Task<Int32> UpdateEmployee(OBEmployees employees)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _OBEmployeeService.CreateEmployee(employees, sessionEmployeeId,companyId);
            return result;
        }

        public async Task<IActionResult> GetRejectEmployees(OBEmployees employees)
        {
            var result = false;
            var companyId = GetSessionValueForCompanyId;
            if (employees != null)
            {
                result = await _OBEmployeeService.GetRejectEmployees(employees,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }
        public async Task<int> GetEmailCount(string officeEmail)
        {
            var companyId = GetSessionValueForCompanyId;
            var officeEmailCount = await _OBEmployeeService.GetEmployeeEmail(officeEmail, companyId);
            return officeEmailCount;
        }

        [HttpPost]
        public async Task<int> GetPersonalCount(string personalEmail, int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var personalEmailCount = await _OBEmployeeService.GetPersonalEmail(personalEmail, empId,companyId);
            return personalEmailCount;
        }

        [HttpPost]

        public async Task<int> CreateOBEmployee(OBEmployees employees)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var output = await _OBEmployeeService.CreateEmployee(employees, sessionEmployeeId,companyId);
            return output;
        }
        [HttpGet]
        public async Task<IActionResult> CreateProfile(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var profileInfo = await _OBEmployeeService.GetProfileByEmployeeId(empId,companyId);
            profileInfo.BloodGroups = await _OBEmployeeService.GetAllBloodGroup();
            profileInfo.RelationshipType = await _OBEmployeeService.GetAllRelationshipType();
            return PartialView("_PartialOBProfileInfo", profileInfo);
        }
        [HttpPost]
        public async Task<int> AddProfileInfo(OBProfileInfo profileInfo, IFormFile file)
        {

            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
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
            var result = await _OBEmployeeService.AddProfileInfo(profileInfo, sessionEmployeeId,companyId);
            return result;          
        }

        [HttpGet]
        public async Task<IActionResult> CreateAddress(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var addressInfo = await _OBEmployeeService.GetAddressByEmployeeId(empId,companyId);
            addressInfo.states = await _OBEmployeeService.GetByCountryId(addressInfo.CountryId);
            addressInfo.cities = await _OBEmployeeService.GetByStateId(addressInfo.StateId);
            addressInfo.Secondarystates = await _OBEmployeeService.GetByCountryId(addressInfo.SecondaryCountryId);
            addressInfo.Secondarycities = await _OBEmployeeService.GetByStateId(addressInfo.SecondaryStateId);
            addressInfo.country = await _OBEmployeeService.GetAllCountry();
            return PartialView("_PartialOBAddressInfo", addressInfo);
        }
        [HttpPost]
        public async Task<int> AddAddressInfo(OBAddressInfo addressInfo)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _OBEmployeeService.AddAddressInfo(addressInfo, sessionEmployeeId,companyId);
            return result;
        }
        [HttpGet]
        public async Task<IActionResult> GetByCountryId(int countryId)
        {
            var result = await _OBEmployeeService.GetByCountryId(countryId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetByStateId(int StateId)
        {
            var result = await _OBEmployeeService.GetByStateId(StateId);
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> CreateOtherDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            // var otherDetails = await _employeesService.GetOtherDetailsByEmployeeId(empId);
            var otherDetails = await _OBEmployeeService.GetAllOtherDetailsViewModel(empId, companyId);
            var getAllDocumentTypes = await _OBEmployeeService.GetAllDocumentTypes(companyId);
            otherDetails.documentTypes = getAllDocumentTypes;
            otherDetails.EmpId = empId;
            return PartialView("_PartialOBOtherDetails", otherDetails);
        }
        [HttpPost]
        public async Task<IActionResult> AddOthersdetails(OBOtherDetails otherDetails, ICollection<IFormFile> file)
        {
            var result = new OBOtherDetailsViewModel();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            otherDetails.OtherDetailsAttachments = new List<OBOtherDetailsAttachments>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var otherDetailsAttachments = new OBOtherDetailsAttachments();
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
                var output = await _OBEmployeeService.InsertAndUpdateOtherDetails(otherDetails, sessionEmployeeId,companyId);
                result = await _OBEmployeeService.GetAllOtherDetailsViewModel(otherDetails.EmpId,companyId);
            }
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOtherDetails(OBOtherDetails otherDetails, ICollection<IFormFile> file)
        {
            var result = new OBOtherDetailsViewModel();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            otherDetails.OtherDetailsAttachments = new List<OBOtherDetailsAttachments>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var otherDetailsAttachments = new OBOtherDetailsAttachments();
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
                var output = await _OBEmployeeService.InsertAndUpdateOtherDetails(otherDetails, sessionEmployeeId, companyId);
                result = await _OBEmployeeService.GetAllOtherDetailsViewModel(otherDetails.EmpId,companyId);
            }
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOtherDetails(OBOtherDetails otherDetails)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = new OBOtherDetailsViewModel();
            if (otherDetails != null)
            {
                await _OBEmployeeService.DeleteOtherDetails(otherDetails);
                result = await _OBEmployeeService.GetAllOtherDetailsViewModel(otherDetails.EmpId,companyId);
            }
            return new JsonResult(result);
        }
        public async Task<List<string>> GetOtherDetailsDocument(int detailId)
        {
            var docNmaes = await _OBEmployeeService.GetDocumentNameByDetailId(detailId);
            return docNmaes;
        }
        public async Task<FileResult> DownloadOtherDetailsFile(int detailId)
        {
            var companyId = GetSessionValueForCompanyId;
            var docNmaes = await _OBEmployeeService.GetOtherDetailsDocumentAndFilePath(detailId);
            var empUserName = string.Empty;
            if (docNmaes.Count() > 0)
            {
                var empId = docNmaes.FirstOrDefault()?.EmpId ?? 0;
                var getUserName = await _OBEmployeeService.GetEmployeeById(empId, companyId);
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
        [HttpGet]
        public async Task<IActionResult> CreateQualification(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var qualification = await _OBEmployeeService.GetAllQulificationViewModel(empId,companyId);
            return PartialView("_PartialOBQualificationInfo", qualification);
        }
        public FileResult DownloadFiles(string Document, string QualificationName)
        {

            string path = Path.GetFullPath(Document);
            var bytes = System.IO.File.ReadAllBytes(path);
            var file = File(bytes, "application/octet-stream", Document);
            file.FileDownloadName = QualificationName;
            return file;
        }
        [HttpPost]
        public async Task<IActionResult> AddQualification(OBQulification qualification, ICollection<IFormFile> file)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = new List<OBQulification>();
            qualification.QualificationAttachments = new List<OBQualificationAttachment>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var qualificationAttachment = new OBQualificationAttachment();
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
                var output = await _OBEmployeeService.InsertAndUpdateQualification(qualification, sessionEmployeeId,companyId);
                result = await _OBEmployeeService.GetAllQualification(qualification.EmpId,companyId);
            }
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQualification(OBQulification qualifications, ICollection<IFormFile> file)
        {
            var result = new List<OBQulification>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId; 
            qualifications.QualificationAttachments = new List<OBQualificationAttachment>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var qualificationAttachment = new OBQualificationAttachment();
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
                var output = await _OBEmployeeService.InsertAndUpdateQualification(qualifications, sessionEmployeeId,companyId);
                if (output)
                {
                    result = await _OBEmployeeService.GetAllQualification(qualifications.EmpId, companyId);
                }
            }

            return new JsonResult(result);
        }
        public async Task<List<string>> GetQualificationDocument(int qualificationId)
        {
            var docNmaes = await _OBEmployeeService.GetDocumentNameByQualificationId(qualificationId);
            return docNmaes;
        }
        public async Task<List<OBQualificationDocumentFilePath>> GetQualificationDocumentAndFilePath(int qualificationId)
        {
            var docNmaes = await _OBEmployeeService.GetQualificationDocumentAndFilePath(qualificationId);
            return docNmaes;
        }
        public async Task<List<string>> GetDocumentNameByDetailId(int detailId)
        {
            var docNmaes = await _OBEmployeeService.GetDocumentNameByDetailId(detailId);
            return docNmaes;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQualification(OBQulification qualification)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = new List<OBQulification>();
            if (qualification != null)
            {
                await _OBEmployeeService.DeleteQualification(qualification);
                result = await _OBEmployeeService.GetAllQualification(qualification.EmpId, companyId);
            }
            return new JsonResult(result);
        }
        public async Task<FileResult> DownloadQualificationFile(int qualificationId)
        {
            var companyId = GetSessionValueForCompanyId;
            var docNmaes = await _OBEmployeeService.GetQualificationDocumentAndFilePath(qualificationId);
            var empUserName = string.Empty;
            if (docNmaes.Count() > 0)
            {
                var empId = docNmaes.FirstOrDefault()?.EmpId ?? 0;
                var getUserName = await _OBEmployeeService.GetEmployeeById(empId,companyId);
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
        public async Task<IActionResult> Createexperience(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var experience = await _OBEmployeeService.GetAllExperienceViewModel(empId,companyId);
            return PartialView("_PartialOBExperience", experience);
        }
        [HttpPost]
        public async Task<IActionResult> AddExperience(OBExperience experience, ICollection<IFormFile> file)
        {
            var result = new List<OBExperience>();
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            experience.ExperienceAttachment = new List<OBExperienceAttachment>();

            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var experienceAttachment = new OBExperienceAttachment();
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
                var output = await _OBEmployeeService.InsertAndUpdateExperience(experience, sessionEmployeeId);
                result = await _OBEmployeeService.GetAllExperience(experience.EmpId,companyId);
            }
            return new JsonResult(result);

        }
        public async Task<List<string>> GetExperienceDocument(int experienceId)
        {
            var docNmaes = await _OBEmployeeService.GetDocumentNameByExperienceId(experienceId);
            return docNmaes;
        }
        public async Task<List<OBExperienceDocumentFilePath>> GetExperienceDocumentAndFilePath(int experienceId)
        {
            var docNmaes = await _OBEmployeeService.GetExperienceDocumentAndFilePath(experienceId);
            return docNmaes;
        }
        [HttpPost]
        public async Task<IActionResult> UpdateExperience(OBExperience experience, ICollection<IFormFile> file)
        {
            var result = new List<OBExperience>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            experience.ExperienceAttachment = new List<OBExperienceAttachment>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var experienceAttachment = new OBExperienceAttachment();
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
                var output = await _OBEmployeeService.InsertAndUpdateExperience(experience, sessionEmployeeId);
                if (output)
                {
                    result = await _OBEmployeeService.GetAllExperience(experience.EmpId,companyId);
                }
            }
            return new JsonResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteExperience(OBExperience experience)
        {
            var result = new List<OBExperience>();
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            if (experience != null)
            {
                await _OBEmployeeService.DeleteExperience(experience, sessionEmployeeId);
                result = await _OBEmployeeService.GetAllExperience(experience.EmpId,companyId);
            }
            return new JsonResult(result);
        }
        public async Task<FileResult> DownloadExperienceFile(int experienceId)
        {
            var companyId = GetSessionValueForCompanyId;
            var docNmaes = await _OBEmployeeService.GetExperienceDocumentAndFilePath(experienceId);
            var empUserName = string.Empty;
            if (docNmaes.Count() > 0)
            {
                var empId = docNmaes.FirstOrDefault()?.EmpId ?? 0;
                var getUserName = await _OBEmployeeService.GetEmployeeById(empId,companyId);
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
        [HttpGet]
        public async Task<IActionResult> CreateBankDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var bankDetails = await _OBEmployeeService.GetBankDetailsByEmployeeId(empId, companyId);
            return PartialView("_PartialOBBankDetails", bankDetails);
        }
        [HttpPost]
        public async Task<int> AddBankDetails(OBBankDetails bankDetails)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _OBEmployeeService.AddBankDetails(bankDetails, sessionEmployeeId,companyId);
            return result;
        }

        // View

        [HttpGet]
        public async Task<IActionResult> OBViewEmployee(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var employee = new OBEmployees();
            if (empId == 0)
            {
                empId = GetSessionValueForEmployeeId;
            }
            employee.EmpId = empId;
            employee = await _OBEmployeeService.GetEmployeeByEmployeeIdView(empId,companyId);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> OBViewEmployeeInfo(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var employee = await _OBEmployeeService.GetEmployeeByEmployeeId(empId,companyId);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> OBViewProfileInfo(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var employee = await _OBEmployeeService.GetEmployeeProfileByEmployeeId(empId,companyId);
            return View(employee);
        }
        [HttpGet]
        public async Task<IActionResult> OBViewAddressInfo(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var addressInfo = await _OBEmployeeService.GetEmployeeAddressByEmployeeId(empId,companyId);
            return View(addressInfo);
        }
        [HttpGet]
        public async Task<IActionResult> OBViewOtherDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var otherDetailsViewModel = await _OBEmployeeService.GetAllOtherDetailsView(empId,companyId);
            return View(otherDetailsViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> OBViewQualification(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var qualificationViewModel = await _OBEmployeeService.GetAllQulificationView(empId,companyId);
            return View(qualificationViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> OBViewExperience(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var experienceViewModel = await _OBEmployeeService.GetAllExperienceView(empId,companyId);
            return View(experienceViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> OBViewBankDetails(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var bankDetails = await _OBEmployeeService.GetBankDetailsByEmployeeId(empId, companyId);
            return View(bankDetails);
        }

        [HttpGet]
        public async Task<IActionResult>OnboardingCompletion()
        {
            var empId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var roleId = GetSessionValueForRoleId;
            var profileCompletion = await _OBEmployeeService.GetProfileCompletion(empId, companyId);
            profileCompletion.RoleId = (Common.Enums.Role)(short)roleId;
            profileCompletion.EmpId = empId;
            return View(profileCompletion);
        }

        [HttpPost]
        public async Task<int> UpdateOnboarding( int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _OBEmployeeService.UpdateStatus(empId, companyId);
           
            if(result == 1)
            {
                HttpContext.Session.SetInt32("IsOnboarding", result);
            }
            
            return result;
        }
    }
}
