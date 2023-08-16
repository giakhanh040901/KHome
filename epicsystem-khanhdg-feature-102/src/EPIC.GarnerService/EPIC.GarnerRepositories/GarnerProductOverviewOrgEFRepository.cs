using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.GarnerRepositories
{
    public class GarnerProductOverviewOrgEFRepository : BaseEFRepository<GarnerProductOverviewOrg>
    {
        public GarnerProductOverviewOrgEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerProductOverviewOrg.SEQ}")
        {
        }

        public List<GarnerProductOverviewOrg> FindAllListByDistribution(int distributionId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: distributionId = {distributionId}, tradingProviderId = {tradingProviderId}");

            return _dbSet.Where(d => d.DistributionId == distributionId && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO).ToList();
        }

        public void UpdateProductOverviewOrg(int distributionId, List<CreateGarnerProductOverviewOrgDto> input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            //Lấy danh sách tổng quan tổ chức đã có trong db
            var productOvervierOrgFind = FindAllListByDistribution(distributionId, tradingProviderId);

            //Xóa đi những tổ chức không được truyền vào
            var productOverviewOrgRemove = productOvervierOrgFind.Where(p => !input.Select(r => r.Id).Contains(p.Id)).ToList();
            foreach (var productOverviewOrgItem in productOverviewOrgRemove)
            {
                productOverviewOrgItem.Deleted = YesNo.YES;
            }

            foreach (var item in input)
            {
                //Nếu là thêm mới thì thêm vào
                if (item.Id == TrueOrFalseNum.FALSE)
                {
                    _dbSet.Add(new GarnerProductOverviewOrg
                    {
                        Id = (int)NextKey(),
                        DistributionId = distributionId,
                        TradingProviderId = tradingProviderId,
                        Name = item.Name,
                        Code = item.Code,
                        Role = item.Role,
                        Icon = item.Icon,
                        Url = item.Url,
                        Status = Status.ACTIVE,
                        CreatedBy = username
                    });
                }
                else
                {
                    var orgFind = _dbSet.FirstOrDefault(p => p.Id == item.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
                    if (orgFind != null)
                    {
                        orgFind.Name = item.Name;
                        orgFind.Icon = item.Icon;
                        orgFind.Code = item.Code;
                        orgFind.Role = item.Role;
                        orgFind.Url = item.Url;
                        orgFind.ModifiedBy = username;
                        orgFind.ModifiedDate = DateTime.Now;
                    }
                }
            }
        }
    }
}
