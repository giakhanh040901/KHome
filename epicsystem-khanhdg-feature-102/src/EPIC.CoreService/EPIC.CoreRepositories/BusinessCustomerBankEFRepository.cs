using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.CoreRepositories
{
    public class BusinessCustomerBankEFRepository : BaseEFRepository<BusinessCustomerBank>
    {
        public BusinessCustomerBankEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, BusinessCustomerBank.SEQ)
        {
        }

        /// <summary>
        /// Thêm ngân hàng 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public BusinessCustomerBank Add(BusinessCustomerBank input)
        {
            _logger.LogInformation($"{nameof(BusinessCustomerBankEFRepository)} -> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var checkBank = _epicSchemaDbContext.BusinessCustomerBanks.Any(b => b.BusinessCustomerId == input.BusinessCustomerId
                && b.BankAccNo == input.BankAccNo && b.BankName == input.BankName && b.Deleted == YesNo.NO);
            // Kiểm tra số tài khoản
            if (checkBank)
            {
                ThrowException(ErrorCode.CoreBankNoAndBankIsExists);
            }    
            input.BusinessCustomerBankAccId = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;
            input.IsDefault = YesNo.NO;
            input.Status = TrueOrFalseNum.TRUE;
            return _dbSet.Add(input).Entity;
        }

        public void Update(BusinessCustomerBank input)
        {
            _logger.LogInformation($"{nameof(BusinessCustomerBankEFRepository)} -> {nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var businessCustomerBank = _dbSet.FirstOrDefault(b => b.BusinessCustomerBankAccId == input.BusinessCustomerBankAccId && b.BusinessCustomerId == input.BusinessCustomerId
                                        && b.Deleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.CoreBusinessCustomerBankNotFound);

            var checkBank = _epicSchemaDbContext.BusinessCustomerBanks.Any(b => b.BusinessCustomerId == input.BusinessCustomerId
                && b.BankAccNo == input.BankAccNo && b.BankName == input.BankName && b.Deleted == YesNo.NO);
            // Kiểm tra số tài khoản
            if (checkBank)
            {
                ThrowException(ErrorCode.CoreBankNoAndBankIsExists);
            }
            businessCustomerBank.BankAccName = input.BankAccName;
            businessCustomerBank.BankAccNo = input.BankAccNo;
            businessCustomerBank.BankId = input.BankId;
            businessCustomerBank.BankName = input.BankName;
            businessCustomerBank.BankBranchName = input.BankBranchName;
            businessCustomerBank.ModifiedBy = businessCustomerBank.ModifiedBy;
            businessCustomerBank.ModifiedDate = DateTime.Now;
        }

        public BusinessCustomerBank FindById(int id, int? businessCustomerId = null)
        {
            _logger.LogInformation($"{nameof(BusinessCustomerBankEFRepository)} -> {nameof(FindById)}: id = {id}, businessCustomerId = {businessCustomerId}");
            return _dbSet.FirstOrDefault(b => b.BusinessCustomerBankAccId == id && (businessCustomerId == null || b.BusinessCustomerId == businessCustomerId) && b.Deleted == YesNo.NO);
        }

        public List<BusinessCustomerBank> FindByBusinessCustomerId(int businessCustomerId)
        {
            _logger.LogInformation($"{nameof(BusinessCustomerBankEFRepository)} -> {nameof(FindById)}: businessCustomerId = {businessCustomerId}");
            return _dbSet.Where(b => (b.BusinessCustomerId == businessCustomerId) && b.Deleted == YesNo.NO).ToList();
        }

        public BankAccountInfoDto GetBankById(int id, int? businessCustomerId = null)
        {
            _logger.LogInformation($"{nameof(InvestorBankAccountEFRepository)} -> {nameof(GetBankById)}: id = {id}, investorId = {businessCustomerId}");
            return (from businessCustomerBank in _dbSet
                    join bank in _epicSchemaDbContext.CoreBanks on businessCustomerBank.BankId equals bank.BankId
                    where businessCustomerBank.BusinessCustomerBankAccId == id && (businessCustomerId == null || businessCustomerBank.BusinessCustomerId == businessCustomerId) && businessCustomerBank.Deleted == YesNo.NO
                    select new BankAccountInfoDto
                    {
                        Id = businessCustomerBank.BusinessCustomerBankAccId,
                        BankName = bank.BankName,
                        BankAccount = businessCustomerBank.BankAccNo,
                        OwnerAccount = businessCustomerBank.BankAccName,
                        IsDefault = businessCustomerBank.IsDefault
                    }).FirstOrDefault();
        }
    }
}
