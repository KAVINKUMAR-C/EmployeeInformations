using EmployeeInformations.Business.IService;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.CompanyViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace EmployeeInformations.Controllers
{
    public class CompanyPolicyController : BaseController
    {
        private readonly ICompanyPolicyService _companyPolicyService;
        private readonly ICompanyService _companyService;

        private readonly IProjectDetailsService _projectDetailsService;

        public CompanyPolicyController(ICompanyPolicyService companyPolicyService, ICompanyService companyService, IProjectDetailsService projectDetailsService)
        {
            _companyPolicyService = companyPolicyService;
            _companyService = companyService;
            _projectDetailsService = projectDetailsService;
        }

        //company policy

        /// <summary>
        /// Logic to get all the companypolicy list
        /// </summary>
        [CheckSessionIsAvailable]
        public async Task<IActionResult> CompanyPolicy()
        {
            var companyId = GetSessionValueForCompanyId;
            var policy = await _companyPolicyService.GetAllPolicy(companyId);
            return View(policy);
        }

        /// <summary>
        /// Logic to get create the companypolicy detail  by particular companypolicy
        /// </summary>
        /// <param name="companyPolicy,file" ></param>
        [HttpPost]
        public async Task<IActionResult> CreateCompanyPolicy(CompanyPolicy companyPolicy, ICollection<IFormFile> file)
        {
            var output = false;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            companyPolicy.PolicyAttachments = new List<PolicyAttachments>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var policyAttachments = new PolicyAttachments();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/CompanyPolicy");
                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    policyAttachments.Document = path.Replace(path, "~/CompanyPolicy/") + fileName;
                    policyAttachments.AttachmentName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    companyPolicy.PolicyAttachments.Add(policyAttachments);
                }
            }
            if (companyPolicy != null)
            {
                output = await _companyPolicyService.CreatePolicy(companyPolicy, sessionEmployeeId,companyId);
            }
            return new JsonResult(output);
        }

        /// <summary>
        /// Logic to get soft deleted the companypolicy detail  by particular companypolicy
        /// </summary>
        /// <param name="companyPolicy" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteCompanyPolicy(CompanyPolicy companyPolicy)
        {
            var output = await _companyPolicyService.DeletePloicy(companyPolicy);
            return new JsonResult(output);
        }

        /// <summary>
        /// Logic to get downloadfile the companypolicyfile detail  by particular companypolicyfile
        /// </summary>
        /// <param name="policyId" >companypolicy</param>
        [HttpGet]
        public async Task<FileResult> DownloadPolicyFile(int policyId)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var docNames = await _companyPolicyService.GetPolicyDocumentAndFilePath(policyId);
            var companyName = string.Empty;
            if (docNames.Count() > 0)
            {
                var getUserName = await _companyService.GetByCompanyId(sessionCompanyId);
                companyName = getUserName.CompanyName;
            }
            if (docNames.Count() == 1)
            {
                foreach (var item in docNames)
                {
                    string path = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                    var bytes = System.IO.File.ReadAllBytes(path);
                    var file = File(bytes, "application/octet-stream", item.Document);
                    file.FileDownloadName = companyName + "_" + item.AttachmentName;
                    return file;
                }
            }
            else
            {
                var zipName = companyName + "_" + $"archive-PolicyFiles-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in docNames)
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
        ///  Logic to get view the companypolicy detail  by particular companypolicy
        /// </summary>
        /// <param name="policyId" >companypolicy</param>
        [HttpPost]
        public async Task<IActionResult> ViewCompanyPolicy(CompanyPolicy companyPolicy)
        {
            var benefitsDetails = await _companyPolicyService.GetAllCompanyPolicyViewModel(companyPolicy);
            return new JsonResult(benefitsDetails);
        }

        //Company Settings

        /// <summary>
        /// Logic to get create and update the companySetting detail by particular modeid only
        /// </summary>
        /// <param name="companySetting" ></param>
        [HttpPost]
        public async Task<Int32> CompanySetting(CompanySetting companySetting)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            HttpContext.Session.SetInt32("DisplayId", companySetting.ModeId);
            var result = await _companyPolicyService.CreateCompanySetting(companySetting,companyId);
            return result;
        }

    }
}

