using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.CompanyViewModel;

namespace EmployeeInformations.Business.Service
{
    public class CompanyPolicyService : ICompanyPolicyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyPolicyRepository _companyPolicyRepository;

        public CompanyPolicyService(ICompanyPolicyRepository companyPolicyRepository, IMapper mapper)
        {
            _companyPolicyRepository = companyPolicyRepository;
            _mapper = mapper;

        }

        //companypolicy

        /// <summary>
        /// Logic to get companypolicy list
        /// </summary>
        public async Task<GetPolicy> GetAllPolicy(int companyId)
        {           
            var policy  = new GetPolicy();
            policy.CompanyPolicy = await _companyPolicyRepository.GetAllPolicys(companyId);
            return policy;
        }

        /// <summary>
        /// Logic to get the companypolicy detail by companyId
        /// </summary>
        /// <param name="companyPolicy" ></param> 
        /// <param name="sessionEmployeeId" ></param>  
        public async Task<bool> CreatePolicy(CompanyPolicy companyPolicy, int sessionEmployeeId, int companyId)
        {
            var result = false;
            if (companyPolicy.PolicyId == 0)
            {
                companyPolicy.CreatedBy = sessionEmployeeId;
                companyPolicy.CreatedDate = DateTime.Now;
                var companyPolicyEntity = _mapper.Map<CompanyPolicyEntity>(companyPolicy);
                var policyId = await _companyPolicyRepository.CreatePolicy(companyPolicyEntity,companyId);
                if (companyPolicy.PolicyAttachments != null && companyPolicy.PolicyAttachments.Count() > 0)
                {
                    var attachmentsEntitys = new List<PolicyAttachmentsEntity>();
                    foreach (var item in companyPolicy.PolicyAttachments)
                    {
                        var attachmentsEntity = new PolicyAttachmentsEntity();
                        attachmentsEntity.PolicyId = policyId;

                        attachmentsEntity.Document = item.Document;
                        attachmentsEntity.AttachmentName = item.AttachmentName;
                        attachmentsEntitys.Add(attachmentsEntity);
                    }
                    result = await _companyPolicyRepository.InsertPolicyAttachment(attachmentsEntitys, policyId);
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get delete the companyPolicy detail 
        /// </summary>
        /// <param name="companyPolicy" ></param>
        public async Task<bool> DeletePloicy(CompanyPolicy companyPolicy)
        {
            var result = false;
            var companyPolicyEntity = await _companyPolicyRepository.GetPolicyByPolicyId(companyPolicy.PolicyId);
            companyPolicyEntity.IsDeleted = true;
            await _companyPolicyRepository.DeletePloicy(companyPolicyEntity);
            var CompanyPolicyAttachementEntitys = await _companyPolicyRepository.GetPolicyDocumentAndFilePath(companyPolicy.PolicyId);
            foreach (var item in CompanyPolicyAttachementEntitys)
            {
                item.IsDeleted = true;
            }
            result = await _companyPolicyRepository.DeletePolicyAttachement(CompanyPolicyAttachementEntitys);
            return result;
        }

        /// <summary>
        /// Logic to get downloadfile the companypolicyfile detail  by particular companypolicyfile
        /// </summary>
        /// <param name="policyId" >companypolicy</param>
        public async Task<List<PolicyAttachments>> GetPolicyDocumentAndFilePath(int policyId)
        {
            var companyPolicyDocumentFilePaths = new List<PolicyAttachments>();
            var docNamesAndFilePath = await _companyPolicyRepository.GetPolicyDocumentAndFilePath(policyId);
            foreach (var item in docNamesAndFilePath)
            {
                var companyPolicyDocumentFilePath = new PolicyAttachments();
                companyPolicyDocumentFilePath.Document = item.Document != null ? item.Document : string.Empty;
                companyPolicyDocumentFilePath.AttachmentName = item.AttachmentName != null ? item.AttachmentName : string.Empty;
                companyPolicyDocumentFilePaths.Add(companyPolicyDocumentFilePath);
            }
            return companyPolicyDocumentFilePaths;
        }

        /// <summary>
        ///  Logic to get view the companypolicy detail  by particular companypolicy
        /// </summary>
        /// <param name="companyPolicy" ></param>
        public async Task<CompanyPolicyViewModel> GetAllCompanyPolicyViewModel(CompanyPolicy companyPolicy)//view model
        {
            var qulificationModel = new CompanyPolicy();
            var policyAttachments = new List<PolicyAttachments>();
            var policyId = companyPolicy.PolicyId;
            var listOfCompanyPolicyEntity = await _companyPolicyRepository.GetPolicyByPolicyId(policyId);
            var listOfAttachments = await _companyPolicyRepository.GetAllAttachmentByPolicyId(policyId);
            var companyPolicyViewModel = new CompanyPolicyViewModel();
            companyPolicyViewModel.PolicyAttachmentsViewModel = new List<PolicyAttachments>();

            if (listOfAttachments != null)
            {
                foreach (var item in listOfAttachments)
                {
                    companyPolicyViewModel.PolicyAttachmentsViewModel.Add(new PolicyAttachments()
                    {
                        PolicyId = item.PolicyId,
                        AttachmentId = item.AttachmentId,
                        Document = item.Document,
                        AttachmentName = item.AttachmentName,
                        splitName = string.IsNullOrEmpty(item.AttachmentName) ? "" : item.AttachmentName.Substring(item.AttachmentName.LastIndexOf(".") + 1)
                    });
                }

            }
            if (listOfCompanyPolicyEntity != null)
            {
                qulificationModel = _mapper.Map<CompanyPolicy>(listOfCompanyPolicyEntity);
            }
            companyPolicyViewModel.CompanyPolicy = qulificationModel;

            return companyPolicyViewModel;
        }

        //companysetting

        /// <summary>
        /// Logic to get the companypolicy detail by companyId
        /// </summary>
        /// <param name="companyId" ></param> 
        public async Task<int> GetCompanySettingsByCompanyId(int companyId)
        {
            var companySetting = await _companyPolicyRepository.GetCompanySettingsByCompanyId(companyId);           
            return companySetting;
        }

        /// <summary>
        /// Logic to get create the companysetting detail in mode changes
        /// </summary>
        /// <param name="companySetting" ></param>
        public async Task<int> CreateCompanySetting(CompanySetting companySetting, int companyId)
        {
            var result = 0;
            if (companySetting != null)
            {
                if (companySetting.CompanySettingId == 0)
                {
                    var companySettingEntiy = _mapper.Map<CompanySettingEntity>(companySetting);
                    await _companyPolicyRepository.CreateCompanySetting(companySettingEntiy,companyId);
                    result = companySettingEntiy.CompanySettingId;
                }
            }
            return result;
        }
    }
}
