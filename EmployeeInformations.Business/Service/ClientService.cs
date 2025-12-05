using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.ClientSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.Extensions.Configuration;


namespace EmployeeInformations.Business.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IEmployeesRepository _employeesRepository;

        public ClientService(IClientRepository clientRepository, IMapper mapper, IConfiguration config, IEmployeesRepository employeesRepository)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _config = config;
            _employeesRepository = employeesRepository;
        }

        /// <summary>
        /// Logic to get client list
        /// </summary>
        public async Task<List<ClientViewModel>> GetAllClient(int companyId)
        {
            var listOfClient = new List<ClientViewModel>();
            listOfClient = await _clientRepository.GetAllClients(companyId);           
            return listOfClient;
        }

        /// <summary>
        ///  Logic to get the client detail by particular ClientId
        /// </summary>
        /// <param name="ClientId" ></param>
        public async Task<ClientViewModel> GetByClientId(int ClientId, int companyId)
        {
            var client = new ClientViewModel();
            var clientEntity = await _clientRepository.GetByClientId(ClientId,companyId);
            if (clientEntity != null)
            {
                client = _mapper.Map<ClientViewModel>(clientEntity);
            }
            return client;
        }

        /// <summary>
        ///  Logic to get view client detail 
        /// </summary>
        /// <param name="client" ></param>      
        public async Task<ClientViewModel> GetByViewClientId(int ClientId, int companyId)
        {
            var client = new ClientViewModel();
            var clientEntity = await _clientRepository.GetByClientId(ClientId,companyId);
            var clients = _mapper.Map<ClientViewModel>(clientEntity);
            if (clientEntity != null)
            {
                client.ClientId = clients.ClientId;
                client.ClientName = clients.ClientName;
                client.PhoneNumber = clients.PhoneNumber;
                client.ClientCompany = clients.ClientCompany;
                client.ClientDetails = clients.ClientDetails;
                client.Email = clients.Email;
                client.Address = clients.Address;
                client.CountryName = await _employeesRepository.GetCountryNameByCountryId(clientEntity.CountryId);
                client.ZipCode = clients.ZipCode;
                client.CountryId = clients.CountryId;
                client.CountryCode = clients.CountryCode;
            }
            return client;
        }

        /// <summary>
        ///  Logic to get create and update client detail 
        /// </summary>
        /// <param name="client" ></param> 
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> CreateClient(ClientViewModel client, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (client != null)
            {
                if (client.ClientId == 0)
                {
                    client.CreatedBy = sessionEmployeeId;
                    client.CreatedDate = DateTime.Now;
                    var clientEntity = _mapper.Map<ClientEntity>(client);
                    var datas = await _clientRepository.CreateClient(clientEntity,companyId);
                    result = clientEntity.ClientId;
                }
                else
                {
                    var clientEntity = await _clientRepository.GetByClientId(client.ClientId,companyId);
                    client.UpdatedDate = DateTime.Now;
                    client.UpdatedBy = sessionEmployeeId;
                    client.CreatedBy = clientEntity.CreatedBy;
                    client.CreatedDate = clientEntity.CreatedDate;
                    var mapclientEntity = _mapper.Map<ClientEntity>(client);
                    var datas = await _clientRepository.CreateClient(mapclientEntity,companyId);
                    result = clientEntity.ClientId;
                }
            }
            return result;
        }

        /// <summary>
        ///  Logic to get delete client detail by particular ClientId
        /// </summary>
        /// <param name="client" ></param>         
        public async Task<bool> DeleteClient(int ClientId, int companyId)
        {
            var result = await _clientRepository.DeleteClient(ClientId,companyId);
            return result;
        }

        /// <summary>
        ///  Logic to get all client count 
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection" ></param>    
        public async Task<int> ClientViewCount(int companyId, SysDataTablePager pager)
        {
            var clientFiltercount = await _clientRepository.ClientViewCount(companyId, pager);
            return clientFiltercount;
        }

        /// <summary>
        ///  Logic to get all client filter data 
        /// </summary>
        /// <param name="companyId,pager,columnName,columnDirection" ></param>  
        public async Task<List<ClientFilterViewModel>> GetClientFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection)
        {
            var clientFilter = await _clientRepository.GetClientFilterView(companyId, pager,columnName,columnDirection);
            return clientFilter;

        }

    }
}
