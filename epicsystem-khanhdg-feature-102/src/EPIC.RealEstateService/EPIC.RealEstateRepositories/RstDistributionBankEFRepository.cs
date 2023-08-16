using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using EPIC.Entities.DataEntities;

namespace EPIC.RealEstateRepositories
{
    public class RstDistributionBankEFRepository : BaseEFRepository<RstDistributionBank>
    {
        public RstDistributionBankEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstDistributionBank.SEQ}")
        {
        }

        /// <summary>
        ///  Kiểm tra xem ngân hàng của đối tác (chủ đầu tư owner có tồn tại hay không)
        /// </summary>
        public void CheckPartnerBankAccount(int projectId, int partnerBankAccountId)
        {
            _logger.LogInformation($"{nameof(RstDistributionBankEFRepository)}->{nameof(CheckPartnerBankAccount)}: projectId = {projectId}, partnerBankAccountId = {partnerBankAccountId}");
            var checkPartnerBankAccount = (from project in _epicSchemaDbContext.RstProjects
                                          join owner in _epicSchemaDbContext.RstOwners on project.OwnerId equals owner.Id
                                          join businessCustomerBank in _epicSchemaDbContext.BusinessCustomerBanks on owner.BusinessCustomerId equals businessCustomerBank.BusinessCustomerId
                                          where project.Deleted == YesNo.NO && owner.Deleted == YesNo.NO && businessCustomerBank.Deleted == YesNo.NO
                                          && project.Id == projectId && businessCustomerBank.BusinessCustomerBankAccId == partnerBankAccountId
                                          select businessCustomerBank).FirstOrDefault().ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstDistributionBankPartnerNotFound);
        }
        public void Add(RstDistributionBank input)
        {
            _logger.LogInformation($"{nameof(RstDistributionBankEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            _dbSet.Add(input);
        }

        public void UpdateDistributionBank(int distributionId, List<int> partnerBankAccountIds, string username)
        {
            _logger.LogInformation($"{nameof(RstDistributionBankEFRepository)}-> {nameof(UpdateDistributionBank)}: projectId = {distributionId}, partnerBankAccountIds = {JsonSerializer.Serialize(partnerBankAccountIds)}, username ={username}");

            var distributionQuery = _epicSchemaDbContext.RstDistributions.FirstOrDefault(r => r.Id == distributionId && r.Deleted == YesNo.NO)
                                    .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstDistributionNotFound);

            //Lấy danh sách theo thông tin khác đã có theo dự án
            var distributionBankQuery = _dbSet.Where(t => t.DistributionId == distributionId && t.Deleted == YesNo.NO);

            // Trường hợp null thì trả về mảng rỗng
            partnerBankAccountIds = (partnerBankAccountIds != null ? partnerBankAccountIds : new List<int>());

            //Xóa đi những thông tin không được truyền vào
            var bankRemove = distributionBankQuery.Where(p => !partnerBankAccountIds.Contains(p.PartnerBankAccountId));
            foreach (var bankItem in bankRemove)
            {
                bankItem.Deleted = YesNo.YES;
                bankItem.ModifiedBy = username;
                bankItem.ModifiedDate = DateTime.Now;
            }

            foreach (var bankItem in partnerBankAccountIds)
            {
                //Nếu là thêm mới thì thêm vào
                if (!distributionBankQuery.Select(r => r.PartnerBankAccountId).Contains(bankItem))
                {
                    CheckPartnerBankAccount(distributionQuery.ProjectId, bankItem);
                    _dbSet.Add(new RstDistributionBank
                    {
                        Id = (int)NextKey(),
                        DistributionId = distributionId,
                        PartnerBankAccountId = bankItem,
                        CreatedDate = DateTime.Now,
                        CreatedBy = username,
                    });
                }
            }
        }

        public IQueryable<RstDistributionBank> GetAll(int distributionId)
        {
            _logger.LogInformation($"{nameof(RstDistributionBankEFRepository)}->{nameof(GetAll)}: distributionId = {distributionId}");
            return _dbSet.Where(b => b.DistributionId == distributionId && b.Deleted == YesNo.NO);
        }
    }
}
