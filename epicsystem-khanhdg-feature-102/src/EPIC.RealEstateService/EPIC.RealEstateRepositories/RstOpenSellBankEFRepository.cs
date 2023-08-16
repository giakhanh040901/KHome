using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstOpenSellBankEFRepository : BaseEFRepository<RstOpenSellBank>
    {
        public RstOpenSellBankEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOpenSellBank.SEQ}")
        {
        }

        /// <summary>
        /// Kiểm tra xem ngân hàng có thuộc đại lý hay không
        /// </summary>
        public void CheckTradingBankAccount(int tradingProviderId, int tradingBankAccountId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellBankEFRepository)}->{nameof(CheckTradingBankAccount)}: tradingProviderId = {tradingProviderId}, tradingBankAccountId = {tradingBankAccountId}");
            var checkPartnerBankAccount = (from tradingProvider in _epicSchemaDbContext.TradingProviders
                                           join businessCustomerBank in _epicSchemaDbContext.BusinessCustomerBanks on tradingProvider.BusinessCustomerId equals businessCustomerBank.BusinessCustomerId
                                           where tradingProvider.Deleted == YesNo.NO && businessCustomerBank.Deleted == YesNo.NO
                                           && tradingProvider.TradingProviderId == tradingProviderId && businessCustomerBank.BusinessCustomerBankAccId == tradingBankAccountId
                                           select businessCustomerBank).FirstOrDefault().ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOpenSellBankTradingNotFound, tradingBankAccountId);
        }

        public void Add(RstOpenSellBank input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellBankEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            _dbSet.Add(input);
        }

        public void UpdateOpenSellBank(int openSellId, List<CreateRstOpenSellBankDto> openSellBanks, string username)
        {
            _logger.LogInformation($"{nameof(RstProductItemExtendRepository)}-> {nameof(UpdateOpenSellBank)}: projectId = {openSellId}, extends = {JsonSerializer.Serialize(openSellBanks)}, username ={username}");

            //Lấy danh sách theo thông tin khác đã có theo dự án
            var openSellBankQuery = _dbSet.Where(t => t.OpenSellId == openSellId && t.Deleted == YesNo.NO);

            //Xóa đi những thông tin không được truyền vào
            var bankRemove = openSellBankQuery.Where(p => !openSellBanks.Select(e => e.Id).Contains(p.Id));
            foreach (var bankItem in bankRemove)
            {
                bankItem.Deleted = YesNo.YES;
                bankItem.ModifiedBy = username;
                bankItem.ModifiedDate = DateTime.Now;
            }

            foreach (var bankItem in openSellBanks)
            {
                //Nếu là thêm mới thì thêm vào
                if (!openSellBankQuery.Select(r => r.Id).Contains(bankItem.Id ?? 0))
                {
                    _dbSet.Add(new RstOpenSellBank
                    {
                        Id = (int)NextKey(),
                        OpenSellId = openSellId,
                        TradingBankAccountId = bankItem.TradingBankAccountId,
                        PartnerBankAccountId = bankItem.PartnerBankAccountId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = username,
                    });
                }
                else
                {
                    var bankFind = _dbSet.FirstOrDefault(e => e.Id == bankItem.Id && e.Deleted == YesNo.NO);
                    if (bankFind != null)
                    {
                        bankFind.TradingBankAccountId = bankItem.TradingBankAccountId;
                        bankFind.PartnerBankAccountId = bankItem.PartnerBankAccountId;
                    }
                }
            }
        }

        public IQueryable<RstOpenSellBank> GetAll(int openSellId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellBankEFRepository)}->{nameof(GetAll)}: openSellId = {openSellId}");
            return _dbSet.Where(b => b.OpenSellId == openSellId && b.Deleted == YesNo.NO);
        }
    }
}
