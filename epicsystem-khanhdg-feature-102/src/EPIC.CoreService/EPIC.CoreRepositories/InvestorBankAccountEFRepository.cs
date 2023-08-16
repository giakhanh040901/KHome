using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreRepositories
{
    public class InvestorBankAccountEFRepository : BaseEFRepository<InvestorBankAccount>
    {
        public InvestorBankAccountEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, InvestorBankAccount.SEQ)
        {
        }

        /// <summary>
        /// Tìm kiếm ngân hàng của nhà đầu tư cá nhân
        /// </summary>
        /// <returns></returns>
        public InvestorBankAccount FindById(int id, int? investorId = null)
        {
            _logger.LogInformation($"{nameof(InvestorBankAccountEFRepository)} -> {nameof(FindById)}: id = {id}, investorId = {investorId}");
            return _dbSet.FirstOrDefault(i => i.Id == id && (investorId == null || i.InvestorId == investorId) && i.Deleted == YesNo.NO);
        }

        public BankAccountInfoDto GetBankById(int id, int? investorId = null)
        {
            _logger.LogInformation($"{nameof(InvestorBankAccountEFRepository)} -> {nameof(GetBankById)}: id = {id}, investorId = {investorId}");
            return (from investorBank in _dbSet
                         join bank in _epicSchemaDbContext.CoreBanks on investorBank.BankId equals bank.BankId
                         where investorBank.Id == id && (investorId == null || investorBank.InvestorId == investorId) && investorBank.Deleted == YesNo.NO
                         select new BankAccountInfoDto
                         {
                             Id = investorBank.Id,
                             BankName = bank.BankName,
                             BankAccount = investorBank.BankAccount,
                             OwnerAccount = investorBank.OwnerAccount,
                             IsDefault = investorBank.IsDefault
                         }).FirstOrDefault();
        }

        /// <summary>
        /// Tìm tất cả InvestorBankAccount
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<InvestorBankAccount> FindAll(int? investorId = null)
        {
            _logger.LogInformation($"{nameof(InvestorBankAccountEFRepository)} -> {nameof(FindAll)}: investorId = {investorId}");
            return _dbSet.Where(i => (investorId == null || i.InvestorId == investorId) && i.Deleted == YesNo.NO).ToList();
        }

        /// <summary>
        /// Tìm kiếm ngân hàng của nhà đầu tư cá nhân theo investorId
        /// </summary>
        /// <returns></returns>
        public InvestorBankAccount FindByInvestorId(int? investorId = null)
        {
            _logger.LogInformation($"{nameof(InvestorBankAccountEFRepository)} -> {nameof(FindById)}: investorId = {investorId}");
            return _dbSet.FirstOrDefault(i => i.InvestorId == investorId && i.Deleted == YesNo.NO);
        }
    }
}
