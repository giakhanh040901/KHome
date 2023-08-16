using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.PartnerBankAccount;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class PartnerBankAccountEFRepository : BaseEFRepository<PartnerBankAccount>
    {
        public PartnerBankAccountEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, PartnerBankAccount.SEQ)
        {
        }

        /// <summary>
        /// Thêm tài khoản ngân hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PartnerBankAccount Add(PartnerBankAccount input)
        {
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var checkBank = _epicSchemaDbContext.PartnerBankAccounts.Any(o => o.PartnerId == input.PartnerId 
                                && o.BankAccNo == input.BankAccNo && o.BankAccName == input.BankAccName && o.Deleted == YesNo.NO);
            if (checkBank)
            {
                ThrowException(ErrorCode.CoreBankNoAndBankIsExists);
            }
            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập nhật tài khoản ngân hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PartnerBankAccount Update(PartnerBankAccount input)
        {
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var checkBank = _epicSchemaDbContext.PartnerBankAccounts.Any(o => o.PartnerId == input.PartnerId
                                && o.BankAccNo == input.BankAccNo && o.BankAccName == input.BankAccName && o.Deleted == YesNo.NO);
            if (checkBank)
            {
                ThrowException(ErrorCode.CoreBankNoAndBankIsExists);
            }
            return _dbSet.Update(input).Entity;
        }

        /// <summary>
        /// Tìm tài khoản khách hàng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PartnerBankAccount FindById(int id, int? partnerId)
        {
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(FindById)}: id = {id}, partnerId = {partnerId}");
            return _dbSet.FirstOrDefault(b => b.Id == id && (b.PartnerId == partnerId) && b.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm tài khoản ngân hàng theo đối tác
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<PartnerBankAccount> FindByPartnerId(int partnerId)
        {
            _logger.LogInformation($"{nameof(BusinessCustomerBankEFRepository)} -> {nameof(FindById)}: partnerId = {partnerId}");
            return _dbSet.Where(b => (b.PartnerId == partnerId) && b.Deleted == YesNo.NO).ToList();
        }

        public PagingResult<PartnerBankAccountDto> FindAll(FilterPartnerBankAccountDto input)
        {
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(FindById)}: input = {JsonSerializer.Serialize(input)}");
            PagingResult<PartnerBankAccountDto> result = new();
            var query = from partnerBankAccount in _epicSchemaDbContext.PartnerBankAccounts
                        join bank in _epicSchemaDbContext.CoreBanks on partnerBankAccount.BankId equals bank.BankId into banks
                        from b in banks.DefaultIfEmpty()
                        where partnerBankAccount.PartnerId == input.PartnerId && partnerBankAccount.Deleted == YesNo.NO
                        select new PartnerBankAccountDto
                        {
                            Id = partnerBankAccount.Id,
                            BankId = partnerBankAccount.BankId,
                            BankAccName = partnerBankAccount.BankAccName,
                            BankAccNo = partnerBankAccount.BankAccNo,
                            BankName = b.BankName,
                            IsDefault = partnerBankAccount.IsDefault,
                            PartnerId = partnerBankAccount.PartnerId,
                            Status = partnerBankAccount.Status
                        };

            result.TotalItems = query.Count();
            query = query.OrderByDescending(d => d.IsDefault).ThenByDescending(d => d.Id);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query;
            return result;
        }
    }
}
