using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class PartnerMsbPrefixAccountEFRepository : BaseEFRepository<PartnerMsbPrefixAccount>
    {
        public PartnerMsbPrefixAccountEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{PartnerMsbPrefixAccount.SEQ}")
        {
        }

        /// <summary>
        /// Thêm tài khoản đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PartnerMsbPrefixAccount Add(PartnerMsbPrefixAccount input)
        {
            _logger.LogInformation($"{nameof(PartnerMsbPrefixAccountEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");

            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Tìm kiếm theo id tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PartnerMsbPrefixAccount FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(PartnerMsbPrefixAccountEFRepository)}->{nameof(FindById)}: Id = {id}, partnerId = {partnerId}");
            return _dbSet.FirstOrDefault(t => t.Id == id && (partnerId == null || t.PartnerId == partnerId) && t.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm theo tài khoản ngân hàng của đối tác
        /// </summary>
        /// <param name="partnerBankAccId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PartnerMsbPrefixAccount FindByPartnerBankId(int partnerBankAccId, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(PartnerMsbPrefixAccountEFRepository)}->{nameof(FindByPartnerBankId)}: partnerBankAccId = {partnerBankAccId}, partnerId = {partnerId}");
            return _dbSet.FirstOrDefault(t => t.PartnerBankAccountId == partnerBankAccId && (partnerId == null || t.PartnerId == partnerId) && t.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Lấy danh sách tài khoản đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<PartnerMsbPrefixAccount> FindAll(FilterPartnerMsbPrefixAccountDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(PartnerMsbPrefixAccountEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");
            PagingResult<PartnerMsbPrefixAccount> result = new();
            var partnerIdMsbQuery = _dbSet.Where(t => t.Deleted == YesNo.NO
                                        && (input.PrefixMsb == null || input.PrefixMsb == t.PrefixMsb)
                                        && (partnerId == null || t.PartnerId == partnerId));

            if (input.PageSize != -1)
            {
                partnerIdMsbQuery = partnerIdMsbQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.TotalItems = partnerIdMsbQuery.Count();
            result.Items = partnerIdMsbQuery.ToList();
            return result;
        }
    }
}
