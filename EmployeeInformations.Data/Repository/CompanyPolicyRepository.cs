using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.CompanyPolicyViewModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.Data.Repository
{
    public class CompanyPolicyRepository : ICompanyPolicyRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public CompanyPolicyRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //// Company Policy

        /// <summary>
        /// Logic to get create the Companypolicy detail
        /// </summary>           
        /// <param name="companyPolicyEntity" ></param>   
        public async Task<int> CreatePolicy(CompanyPolicyEntity companyPolicyEntity,int companyId)
        {
            var result = 0;
            if (companyPolicyEntity?.PolicyId == 0)
            {
                companyPolicyEntity.CompanyId = companyId;
                await _dbContext.CompanyPolicyEntitys.AddAsync(companyPolicyEntity);
                await _dbContext.SaveChangesAsync();
                result = companyPolicyEntity != null ? companyPolicyEntity.PolicyId : 0;
            }
            return result;
        }        

        /// <summary>
         /// Logic to get policyId the Companypolicy detail by particular policyId
        /// </summary>           
        /// <param name="policyId" >companyPolicyEntity</param>   
        public async Task<CompanyPolicyEntity> GetPolicyByPolicyId(int policyId)
        {
            var expensesEntity = await _dbContext.CompanyPolicyEntitys.Where(x => x.PolicyId == policyId).FirstOrDefaultAsync();
            return expensesEntity;
        }


        /// <summary>
        /// Logic to get delete the companypolicyentitys detail
        /// </summary>           
        /// <param name="companyPolicyEntity" ></param>
        public async Task<bool> DeletePloicy(CompanyPolicyEntity companyPolicyEntity)
        {
            _dbContext.CompanyPolicyEntitys.Update(companyPolicyEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// Policy Attachment

        /// <summary>
        /// Logic to get policyId the policyattachmentsentitys detail by particular policyId
         /// </summary>           
        /// <param name="policyId" >policyattachmentsentitys</param>
        /// <param name="IsDeleted" >policyattachmentsentitys</param>
        public async Task<List<PolicyAttachmentsEntity>> GetPolicyDocumentAndFilePath(int policyId)
        {
            var docNmaes = await _dbContext.PolicyAttachmentsEntitys.Where(e => e.PolicyId == policyId && !e.IsDeleted).ToListAsync();
            return docNmaes;
        }


        /// <summary>
        /// Logic to get policyId the policyattachmentsentitys detail
        /// </summary>           
        /// <param name="policyId" >companyPolicyEntity</param> 
        /// <param name="IsDeleted" >companyPolicyEntity</param> 
        public async Task<List<PolicyAttachmentsEntity>> GetAllAttachmentByPolicyId(int policyId)
        {
            var expensesEntity = await _dbContext.PolicyAttachmentsEntitys.Where(x => x.PolicyId == policyId && !x.IsDeleted).ToListAsync();
            return expensesEntity;
        }


        /// <summary>
        /// Logic to get create the policyattachmentsentitys detail
        /// </summary>           
        /// <param name="policyAttachmentsEntitys,policyId" ></param>        
        public async Task<bool> InsertPolicyAttachment(List<PolicyAttachmentsEntity> policyAttachmentsEntitys, int policyId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.PolicyAttachmentsEntitys.Where(x => x.PolicyId == policyId).ToListAsync();
            if (attachmentsEntitys.Count() == 0)
            {
                await _dbContext.PolicyAttachmentsEntitys.AddRangeAsync(policyAttachmentsEntitys);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the policyattachmentsentitys detail 
        /// </summary>           
        /// <param name="policyAttachmentsEntity" ></param>   
        public async Task<bool> DeletePolicyAttachement(List<PolicyAttachmentsEntity> policyAttachmentsEntity)
        {
            var result = false;
            _dbContext.PolicyAttachmentsEntitys.UpdateRange(policyAttachmentsEntity);
           result = await _dbContext.SaveChangesAsync() > 0;
            return result;
        }

        /// Company setting

        /// <summary>
        /// Logic to get companyId the companysetting detail by particular companyId
        /// </summary>           
        /// <param name="CompanyId" ></param>   
        /// <param name="IsDeleted" ></param>  
        public async Task<int> GetCompanySettingsByCompanyId(int companyId)
        {
            var result = 0;
            var expensesEntity = await _dbContext.CompanySetting.Where(x => x.CompanyId == companyId && !x.IsDeleted).FirstOrDefaultAsync();
            if (expensesEntity != null)
            {
                 result = expensesEntity.ModeId;
            }
            return result;
        }


        /// <summary>
        /// Logic to get create and update the companysetting detail by particular companysetting modeId only
        /// </summary>   
        /// <param name="companySettingEntiy" ></param>
        public async Task<Int32> CreateCompanySetting(CompanySettingEntity companySettingEntiy, int companyId)
        {
            var companySetting = await _dbContext.CompanySetting.Where(x => x.CompanyId == companyId && !x.IsDeleted).AsNoTracking().FirstOrDefaultAsync();
            var result = 0;
            if (companySetting != null && companySetting.CompanySettingId > 0)
            {
                companySetting.ModeId = companySettingEntiy.ModeId;
                _dbContext.CompanySetting.Update(companySetting);
                await _dbContext.SaveChangesAsync();
                result = companySetting.CompanySettingId;
            }
            return result;
        }

        /// <summary>
        /// Logic to get all companypolicy details list 
        /// </summary>   
        public async Task<List<CompanyPolicy>> GetAllPolicys(int companyId)
        {
            var companyPolicys = await (from companyPolicy in _dbContext.CompanyPolicyEntitys
                                  join policyAttachment in _dbContext.PolicyAttachmentsEntitys on companyPolicy.PolicyId equals policyAttachment.PolicyId
                                  where !companyPolicy.IsDeleted && companyId == companyPolicy.CompanyId && !policyAttachment.IsDeleted
                                  select new CompanyPolicy ()
                                  {
                                      PolicyId = companyPolicy.PolicyId,
                                      PolicyName = companyPolicy.PolicyName,
                                  }).ToListAsync();
            return companyPolicys;
        }
    }
}
