using EmployeeInformations.Business.IService;
using EmployeeInformations.Model.EmailDraftViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    public class EmailDraftController : BaseController
    {

        private readonly IEmailDraftService _emailDraftService;

        public EmailDraftController(IEmailDraftService emailDraftService, ICompanyService companyservice)
        {
            _emailDraftService = emailDraftService;

        }

        /// <summary>
        /// Logic to get all the emaildrafttype list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EmailDraftType()
        {
            var companyId = GetSessionValueForCompanyId;
            var emailDraftType = await _emailDraftService.GetAllEmailDraftType(companyId);
            return View();
        }
        /// <summary>
        /// Logic to get all the emaildrafttype list using pagination
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        [HttpGet]
        public async Task<IActionResult> EmailDraftTypePagination(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            var employeeCount = await _emailDraftService.EamilDraftTypesCount(pager, companyId);
            var employee = await _emailDraftService.GetAllEmailDraftTypes(pager, columnName, columnDirection, companyId);
            return Json(new
            {
                iTotalRecords = employeeCount,
                iTotalDisplayRecords = employeeCount,
                data = employee.EmailDraftPageniation,
            });
        }

        /// <summary>
        /// Logic to get edit the emaildraftcontent detail by particular editemaildraftcontent
        /// </summary>
        /// <param name="Id" >emaildraftcontent</param>
        [HttpGet]
        public IActionResult EditEmailDraftContent(int Id)
        {
            var draftContent = new EmailDraftContent();
            draftContent.Id = Id;
            return PartialView("EditEmailDraftContent", draftContent);
        }

        /// <summary>
        /// Logic to get create the editemaildraftcontent detail by particular editemaildraftcontent
        /// </summary>
        /// <param name="Id" >emaildraftcontent</param>
        [HttpGet]
        public async Task<IActionResult> UpdateEmailDraftContent(int Id)
        {
            var companyId = GetSessionValueForCompanyId;
            var emailDraft = await _emailDraftService.GetById(Id, companyId);
            var sendEmails = await _emailDraftService.GetAllSendEmails(companyId);
            emailDraft.sendEmails = sendEmails;
            return PartialView("UpdateEmailDraftContent", emailDraft);
        }

        /// <summary>
        /// Logic to get create the create email draft content detail 
        /// </summary>
        /// <param name="Id" >emaildraftcontent</param>
        [HttpPost]
        public async Task<int> CreateEmailDraftContent(EmailDraftContent emailDraftContent)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _emailDraftService.CreateEmailDraftContent(emailDraftContent,companyId);
            return result;
        }

        //Draft Type

        /// <summary>
        /// Logic to get update the emailDraftType detail  by particular emailDraftType
        /// </summary>
        /// <param name="emailDraftType" ></param>
        [HttpPost]
        public async Task<int> UpdateDraftType(EmailDraftType emailDraftType)
        {
            var result = await _emailDraftService.UpdateDraftType(emailDraftType);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the editemaildrafttype detail  by particular editemaildrafttype
        /// </summary>
        /// <param name="Id" >delete mail drafttype</param>
        [HttpPost]
        public async Task<IActionResult> DeletedEmailDraftType(int id)
        {
            var result = await _emailDraftService.DeletedEmailDraftType(id);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get check draftType the emaildrafttype detail  by particular draftType not allow repeated draftType
        /// </summary>
        /// <param name="draftType" ></param> 
        [HttpPost]
        public async Task<int> GetDraftTypeName(string draftType)
        {
            var companyId = GetSessionValueForCompanyId;
            var totalDraftTypeName = await _emailDraftService.GetDraftTypeName(draftType,companyId);
            return totalDraftTypeName;
        }

        /// <summary>
        /// Logic to get create the emailDraftType detail  by particular emailDraftType
        /// </summary>
        /// <param name="Id" >emailDraftType</param>
        [HttpPost]
        public async Task<int> AddEmailDraftType(EmailDraftType emailDraftType)
        {
            emailDraftType.CompanyId = GetSessionValueForCompanyId;
            var result = await _emailDraftService.CreateEmailDraftType(emailDraftType);
            return result;
        }

    }
}
