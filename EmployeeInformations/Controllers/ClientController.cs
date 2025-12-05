using EmployeeInformations.Business.IService;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.ClientSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly IEmployeesService _employeesService;

        public ClientController(IClientService clientService, IEmployeesService employeesService)
        {
            _clientService = clientService;
            _employeesService = employeesService;
        }

        //Client

        /// <summary>
        /// Logic to get all the Client list 
        /// </summary>
        /// 
        public async Task<IActionResult> Client()
        {
            return View();
        }
        /// <summary>
        /// Logic to get all the client Filtered data and count 
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        [HttpGet]
        public async Task<IActionResult> ClientFilter(SysDataTablePager pager,string columnName,string columnDirection)
        {
            var companyId = GetSessionValueForCompanyId;
            var clientFilter = await _clientService.GetClientFilterView(companyId, pager, columnName, columnDirection);
            var clientFilterCount = await _clientService.ClientViewCount(companyId, pager);
            return Json(new
            {
                iTotalRecords = clientFilterCount,
                iTotalDisplayRecords = clientFilterCount,
                data = clientFilter,
            });            
        }

        /// <summary>
        /// Logic to get empty view
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CreateClient()
        {
            var client = new ClientViewModel();
            client.countrys = await _employeesService.GetAllCountry();
            return View(client);
        }

        /// <summary>
        /// Logic to get edit the client detail  by particular client
        /// </summary>
        /// <param name="ClientId" >client</param>
        [HttpGet]
        public IActionResult EditClient(int ClientId)
        {
            var clients = new ClientViewModel();
            clients.ClientId = ClientId;
            return PartialView("EditClient", clients);
        }

        /// <summary>
        ///  Logic to get create the client detail  by particular client
        /// </summary>
        /// <param name="clientViewModel" ></param>
        [HttpPost]
        public async Task<int> AddClient(ClientViewModel clientViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _clientService.CreateClient(clientViewModel, sessionEmployeeId,companyId);
            return result;
        }

        /// <summary>
        ///  Logic to get update the client detail  by particular client
        /// </summary>
        /// <param name="clientViewModel" ></param>
        [HttpPost]
        public async Task<int> UpdateClient(ClientViewModel clientViewModel)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = await _clientService.CreateClient(clientViewModel, sessionEmployeeId,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get update the client detail  by particular client
        /// </summary>
        /// <param name="ClientId" >client</param>
        [HttpGet]
        public async Task<IActionResult> UpdateClient(int ClientId)
        {
            var companyId = GetSessionValueForCompanyId;
            var clients = await _clientService.GetByClientId(ClientId,companyId);
            clients.countrys = await _employeesService.GetAllCountry();
            return PartialView("ClientUpdate", clients);
        }

        /// <summary>
        ///  Logic to get dispaly the client detail  by particular client
        /// </summary>
        /// <param name="ClientId" >client</param>
        [HttpGet]
        public async Task<IActionResult> ViewClient(int ClientId)
        {
            var companyId = GetSessionValueForCompanyId;
            var clients = await _clientService.GetByViewClientId(ClientId,companyId);
            return View(clients);
        }

        /// <summary>
        /// Logic to get soft deleted the client detail  by particular client
        /// </summary>
        /// <param name="ClientId" >client</param>
        [HttpPost]
        public async Task<IActionResult> DeleteClient(int ClientId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _clientService.DeleteClient(ClientId,companyId);
            return new JsonResult(result);
        }

    }

}
