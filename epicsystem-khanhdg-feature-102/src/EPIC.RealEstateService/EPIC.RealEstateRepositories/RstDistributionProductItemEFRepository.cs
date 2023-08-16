using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionProductItem;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstDistributionProductItemEFRepository : BaseEFRepository<RstDistributionProductItem>
    {
        public RstDistributionProductItemEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstDistributionProductItem.SEQ}")
        {
        }

        public RstDistributionProductItem Add(RstDistributionProductItem input)
        {
            _logger.LogInformation($"{nameof(RstDistributionProductItemEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.Status = Status.ACTIVE;
            input.Deleted = YesNo.NO;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Tìm kiếm căn
        /// </summary>
        public RstDistributionProductItem FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstDistributionProductItemEFRepository)}->{nameof(FindById)}: id = {id}");
            var result = from distributionProductItem in _dbSet
                         join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                         where distributionProductItem.Id == id && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                         select distributionProductItem;
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Đối tác khóa sản phẩm của phân phối
        /// </summary>
        public RstDistributionProductItem LockDistributionItem(int id, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstDistributionProductItemEFRepository)}->{nameof(LockDistributionItem)}: id = {id}, username = {username}");
            var distributionItemQuery = (from distributionProductItem in _dbSet
                                         join distribution in _epicSchemaDbContext.RstDistributions on distributionProductItem.DistributionId equals distribution.Id
                                         where distributionProductItem.Id == id && distributionProductItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                                         && distribution.PartnerId == partnerId
                                         select distributionProductItem).FirstOrDefault().ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstDistributionProductItemNotFound);

            if (distributionItemQuery.Status == Status.ACTIVE)
            {
                // Kiểm tra xem sản phẩm của phân phối có đang được mở bán với trạng thái giữ chỗ hay đã cọc không
                var openSellDetailCheck = (from openSellDetail in _epicSchemaDbContext.RstOpenSellDetails
                                           join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                           where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                                           && (openSellDetail.Status == RstProductItemStatus.GIU_CHO || openSellDetail.Status == RstProductItemStatus.DA_COC)
                                           && openSellDetail.DistributionProductItemId == id && openSell.Status == RstDistributionStatus.DANG_BAN
                                           select openSellDetail).Any();
                if (openSellDetailCheck)
                {
                    ThrowException(ErrorCode.RstDistributionProductItemHaveTrading);
                }
                distributionItemQuery.Status = Status.INACTIVE;
            }
            else
            {
                distributionItemQuery.Status = Status.ACTIVE;
            }
            distributionItemQuery.ModifiedBy = username;
            distributionItemQuery.ModifiedDate = DateTime.Now;
            return distributionItemQuery;
        }

        //public IQueryable<RstDistributionProductItem> GetAllByDistribution(int distributionId)
        //{
        //    _logger.LogInformation($"{nameof(RstDistributionProductItemEFRepository)}->{nameof(GetAllByDistribution)}: DistributionId = {distributionId}");
        //    return (from distributionProductItem in _dbSet
        //            join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
        //            where distributionProductItem.DistributionId == distributionId && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
        //            select distributionProductItem);
        //}

        public IQueryable<RstDistributionProductItemDto> GetAllByDistribution(int distributionId)
        {
            _logger.LogInformation($"{nameof(RstDistributionProductItemEFRepository)}->{nameof(GetAllByDistribution)}: DistributionId = {distributionId}");
            return (from distributionProductItem in _dbSet
                    join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                    where distributionProductItem.DistributionId == distributionId && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                    select new RstDistributionProductItemDto
                    {
                        Id = distributionProductItem.Id,
                        ProductItemId = productItem.Id,
                        ProductItemNoFloor = productItem.NoFloor,
                        ProductItemNumberFloor = productItem.NumberFloor,
                        ProductItemRoomType = productItem.RoomType,
                        ProductItemPrice = productItem.Price,
                        ProductItemPriceArea = productItem.PriceArea,
                        ProductItemUnitPrice = productItem.UnitPrice,
                        Status = distributionProductItem.Status,
                        CreatedDate = DateTime.Now,
                    });
        }

        /// <summary>
        /// Lấy thông tin mở bán từ DistributionItem
        /// </summary>
        public IQueryable<RstOpenSellDetail> CheckDistributionItem(int distributionId, int? distributionItemId, List<int> openDetailStatus)
        {
            _logger.LogInformation($"{nameof(RstDistributionProductItemEFRepository)}->{nameof(CheckDistributionItem)}: DistributionId = {distributionId}, openDetailStatus = {openDetailStatus}");
            return from distributionItem in _dbSet
                   join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on distributionItem.Id equals openSellDetail.DistributionProductItemId
                   where distributionItem.DistributionId == distributionId && (distributionItemId == null || distributionItemId == distributionItem.Id) && distributionItem.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                   && (openDetailStatus == null || openDetailStatus.Contains(openSellDetail.Status))
                   select openSellDetail;
        }

        /// <summary>
        /// Lấy danh sách phân phối căn hộ theo đại lý
        /// </summary>
        public IQueryable<RstDistributionProductItem> GetAllProductItemByTrading(FilterRstDistributionProductItemByTradingDto input, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstDistributionProductItemEFRepository)}->{nameof(GetAllProductItemByTrading)}:  input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            // Lấy danh sách Id sản phẩm của các phân phối thuộc dự án cho đại lý đã được mở bán 
            var distributiontItemIsDistributed = from openSell in _epicSchemaDbContext.RstOpenSells
                                                 join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on openSell.Id equals openSellDetail.OpenSellId
                                                 where openSell.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                                                 && openSell.ProjectId == input.ProjectId && openSell.TradingProviderId == tradingProviderId
                                                 && openSell.Status != RstDistributionStatus.HUY_DUYET
                                                 select openSellDetail.DistributionProductItemId;

            return (from distributionProductItem in _dbSet
                    join distribution in _epicSchemaDbContext.RstDistributions on distributionProductItem.DistributionId equals distribution.Id
                    join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                    where distribution.TradingProviderId == tradingProviderId && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                    && distribution.ProjectId == input.ProjectId && !distributiontItemIsDistributed.Contains(distributionProductItem.Id) // Lọc đi các căn đã phân phối
                    && distribution.Status == RstDistributionStatus.DANG_BAN && distributionProductItem.Status == Status.ACTIVE && productItem.IsLock == YesNo.NO
                    && (input.Keyword == null || productItem.Name.Contains(input.Keyword) || productItem.Code.Contains(input.Keyword))
                    && (input.BuildingDensityId == null || input.BuildingDensityId == productItem.BuildingDensityId)
                    && (input.RedBookType == null || input.RedBookType == productItem.RedBookType)
                    select distributionProductItem).OrderByDescending(r => r.Id);
        }
    }
}
