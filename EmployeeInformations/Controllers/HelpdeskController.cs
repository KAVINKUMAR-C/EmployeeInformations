using EmployeeInformations.Business.IService;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.HelpdeskViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace EmployeeInformations.Controllers
{
    public class HelpdeskController : BaseController
    {
        private readonly IHelpdeskService _helpdeskService;
        private readonly IMasterRepository _masterRepository;
        public HelpdeskController (IHelpdeskService helpdeskService , IMasterRepository masterRepository)
        {
            _helpdeskService = helpdeskService;
            _masterRepository = masterRepository;
        }

        /// <summary>
        /// Logic to get all the helpdesk list
        /// </summary>
        public async Task<IActionResult> Helpdesks()
        {
            var companyId = GetSessionValueForCompanyId;
            var helpdeskViewModel = new HelpdeskViewModel();
            //helpdeskViewModel = await _helpdeskService.GetAllHelpdesks();
            helpdeskViewModel.TicketTypes = await _masterRepository.GetTicketTypes(companyId);
         
            return View(helpdeskViewModel);
        }
        /// <summary>
        /// Logic to get all the helpdesk list
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>  
        [HttpGet]
        public async Task<IActionResult>HelpdesksFilter(SysDataTablePager pager, string columnDirection, string columnName)
        {
            var companyId = GetSessionValueForCompanyId;
            var employeeCount = await _helpdeskService.GetAllHelpdesksFilterCount(pager,companyId);
           var employee = await _helpdeskService.GetAllHelpdeskFilter(pager,columnDirection,columnName,companyId);
           employee.TicketTypes = await _masterRepository.GetTicketTypes(companyId);
            //return View(employeeCount);
            return Json(new
            {
                iTotalRecords = employeeCount,
                iTotalDisplayRecords = employeeCount,
                data = employee.helpDeskEntities,
            });
        }
        /// <summary>
        /// Logic to get the create and update helpdesk 
        /// </summary>
        /// <param name="helpdeskViewModel" ></param>
        /// <param name="file" ></param>   
        public async Task<IActionResult> UpsertHelpdesk(HelpdeskViewModel helpdeskViewModel, ICollection<IFormFile> file)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result  = false;
            helpdeskViewModel.TicketAttachments = new List<TicketAttachments>();
            foreach (var item in file)
            {
                if (file != null && file.Count() > 0)
                {
                    var qualificationAttachment = new TicketAttachments();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/HelpdeskTicket");
                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var combinedPath = Path.Combine(path, fileName);
                    qualificationAttachment.Document = path.Replace(path, "~/HelpdeskTicket/") + fileName;
                    qualificationAttachment.AttachmentName = fileName;
                    using (var stream = new FileStream(combinedPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    helpdeskViewModel.TicketAttachments.Add(qualificationAttachment);
                }
            }
            if (helpdeskViewModel != null)
            {
                 result = await _helpdeskService.UpsertHelpdesk(helpdeskViewModel, sessionEmployeeId, sessionCompanyId);               
            }
            return new JsonResult(result);  
        }

        /// <summary>
        /// Logic to get the delete helpdesk detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteHelpdesk(int id)
        {           
            var result = await _helpdeskService.DeleteHelpdesk(id);                                       
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get the edit helpdesk detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        [HttpGet]
        public async Task<IActionResult> EditHelpdesk(int Id)
        {
            var helpdesk = new Helpdesk();
            helpdesk.Id = Id;
            return View(helpdesk);
        }

        /// <summary>
        /// Logic to get the delete helpdesk detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        [HttpGet]
        public async Task<IActionResult> UpdateHelpdesk(int Id)
        {
            var companyId = GetSessionValueForCompanyId;
            var helpdeskViewModel = await _helpdeskService.GetAllHelpdeskViewModel(Id);
            helpdeskViewModel.TicketTypes = await _masterRepository.GetAllTicketTypes(companyId);
            return View(helpdeskViewModel);
        }

        /// <summary>
        /// Logic to get the download detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        public async Task<FileResult> DownloadHelpdeskFile(int Id)
        {
            var docNmaes = await _helpdeskService.GetTicketDocumentAndFilePath(Id);
            var empUserName = string.Empty;
            
            if (docNmaes.Count() == 1)
            {
                foreach (var item in docNmaes)
                {
                    string path = item.Document.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
                    var bytes = System.IO.File.ReadAllBytes(path);
                    var file = File(bytes, "application/octet-stream", item.Document);
                    file.FileDownloadName = empUserName + "_" + item.AttachmentName;
                    return file;
                }
            }
            else
            {
                var zipName = empUserName + "_" + $"archive-HelpdeskFiles-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
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
        /// Logic to get the view detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        [HttpGet]
        public async Task<IActionResult> ViewHelpDesk(int Id)
        {
            var sessionCompanyId = GetSessionValueForCompanyId;
            var viewHelpDesk = await _helpdeskService.ViewHelpdesk(Id, sessionCompanyId);
            return View(viewHelpDesk);
        }
    }
}
