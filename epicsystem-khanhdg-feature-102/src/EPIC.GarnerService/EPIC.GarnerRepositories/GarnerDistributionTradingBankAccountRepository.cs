using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace EPIC.GarnerRepositories
{
    public class GarnerDistributionTradingBankAccountRepository : BaseEFRepository<GarnerDistributionTradingBankAccount>
    {
        public GarnerDistributionTradingBankAccountRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerDistributionTradingBankAccount.SEQ}")
        {
        }

        public List<GarnerDistributionTradingBankAccount> FindAllListByDistribution(int distributionId, int? type = null)
        {
            _logger.LogInformation($"{nameof(FindAllListByDistribution)}: distributionId = {distributionId}, type = {type}");
            return _dbSet.Where(b => b.DistributionId == distributionId && (type == null || b.Type == type) && b.Deleted == YesNo.NO).ToList();
        }

        /// <summary>
        /// lấy ngân hàng đầu tiên theo id bán theo kỳ hạn để thu
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public GarnerDistributionTradingBankAccount FindFirstBankCollectByDistribution(int distributionId)
        {
            _logger.LogInformation($"{nameof(FindFirstBankCollectByDistribution)}: distributionId = {distributionId}");
            return _dbSet.FirstOrDefault(b => b.DistributionId == distributionId && b.Type == DistributionTradingBankAccountTypes.THU && b.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thêm nhiều tài khoản ngân hàng thụ hưởng
        /// Chưa có thì thêm
        /// Có rồi thì giữ nguyên
        /// Đã có trong DB nhưng không truyền vào thì xóa
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="tradingBankAccountCollects"></param>
        /// <param name="username"></param>
        public void UpdateTradingBankAccount(int distributionId, List<int> tradingBankAccountCollects, List<int> tradingBankAccountPays, string username)
        {
            _logger.LogInformation($"{nameof(UpdateTradingBankAccount)}: tradingBankAccountCollects = {JsonSerializer.Serialize(tradingBankAccountCollects)}, tradingBankAccountPays = {JsonSerializer.Serialize(tradingBankAccountPays)}, username = {username}");

            //Lấy danh sách Tài khoản ngân hàng thụ hưởng của đại lý
            var updateTradingBankAccountFind = FindAllListByDistribution(distributionId);

            if (tradingBankAccountCollects.Count != 0)
            {
                //Xóa đi những ngân hàng ko được truyền vào
                var updateTradingBankAccountRemove = updateTradingBankAccountFind.Where(p => p.Type == DistributionTradingBankAccountTypes.THU && !tradingBankAccountCollects.Contains(p.BusinessCustomerBankAccId)).ToList();
                foreach (var bankAccountItem in updateTradingBankAccountRemove)
                {
                    bankAccountItem.Deleted = YesNo.YES;
                }

                foreach (var item in tradingBankAccountCollects)
                {
                    // Nếu là thêm mới thì thêm vào
                    // Nếu loại ngân hàng chưa có trong list thì thêm vào, nếu đã có thì giữ nguyên
                    if (!updateTradingBankAccountFind.Where(p => p.Type == DistributionTradingBankAccountTypes.THU).Select(p => p.BusinessCustomerBankAccId).Contains(item))
                    {
                        var findBank = _epicSchemaDbContext.BusinessCustomerBanks.FirstOrDefault(b => b.BusinessCustomerBankAccId == item && b.Deleted == YesNo.NO);
                        if (findBank == null)
                        {
                            _logger.LogError($" {nameof(UpdateTradingBankAccount)}:Không tìm thấy ngân hàng doanh nghiệp BusinessCustomerBankAccId = {item}");
                            continue;
                        }
                        _dbSet.Add(new GarnerDistributionTradingBankAccount
                        {
                            Id = (int)NextKey(),
                            DistributionId = distributionId,
                            BusinessCustomerBankAccId = item,
                            Type = DistributionTradingBankAccountTypes.THU,
                            CreatedBy = username
                        });
                    }
                }
            }
            if (tradingBankAccountPays.Count != 0)
            {
                //Xóa đi những ngân hàng ko được truyền vào
                var updateTradingBankAccountRemove = updateTradingBankAccountFind.Where(p => p.Type == DistributionTradingBankAccountTypes.CHI && !tradingBankAccountPays.Contains(p.BusinessCustomerBankAccId)).ToList();
                foreach (var bankAccountItem in updateTradingBankAccountRemove)
                {
                    bankAccountItem.Deleted = YesNo.YES;
                }

                foreach (var item in tradingBankAccountPays)
                {
                    // Nếu là thêm mới thì thêm vào
                    // Nếu loại ngân hàng chưa có trong list thì thêm vào, nếu đã có thì giữ nguyên
                    if (!updateTradingBankAccountFind.Where(p => p.Type == DistributionTradingBankAccountTypes.CHI).Select(p => p.BusinessCustomerBankAccId).Contains(item))
                    {
                        var findBank = _epicSchemaDbContext.BusinessCustomerBanks.FirstOrDefault(b => b.BusinessCustomerBankAccId == item && b.Deleted == YesNo.NO);
                        if (findBank == null)
                        {
                            _logger.LogError($" {nameof(UpdateTradingBankAccount)}:Không tìm thấy ngân hàng doanh nghiệp BusinessCustomerBankAccId = {item}");
                            continue;
                        }    

                        _dbSet.Add(new GarnerDistributionTradingBankAccount
                        {
                            Id = (int)NextKey(),
                            DistributionId = distributionId,
                            BusinessCustomerBankAccId = item,
                            Type = DistributionTradingBankAccountTypes.CHI,
                            CreatedBy = username
                        });
                    }
                }
            }
        }
    }
}
