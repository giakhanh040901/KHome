using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstOpenSellDetail;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstOpenSellDetailEFRepository : BaseEFRepository<RstOpenSellDetail>
    {
        public RstOpenSellDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOpenSellDetail.SEQ}")
        {
        }

        public RstOpenSellDetail Add(RstOpenSellDetail input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.IsShowPrice = YesNo.YES; 
            input.IsShowApp = YesNo.YES;
            input.Status = RstProductItemStatus.KHOI_TAO;
            input.IsLock = YesNo.NO;
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        public RstOpenSellDetail FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOpenSellDetailEFRepository)}->{nameof(FindById)}: tradingProviderId = {tradingProviderId}, id={id}");
            var result = from openSellDetail in _dbSet
                         join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                         where openSellDetail.Id == id && openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                         && (tradingProviderId == null || openSell.TradingProviderId == tradingProviderId)
                         select openSellDetail;
            return result.FirstOrDefault();
        }

        public IQueryable<RstOpenSellDetail> GetAllByOpenSell(int openSellId, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellDetailEFRepository)}->{nameof(GetAllByOpenSell)}: tradingProviderId = {tradingProviderId}, openSellId={openSellId}");
            var result = from openSellDetail in _dbSet
                         join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                         where openSell.Id == openSellId && openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                         && (tradingProviderId == null || openSell.TradingProviderId == tradingProviderId)
                         select openSellDetail;
            return result;
        }

        /// <summary>
        /// Kiểm tra xem sản phẩm của phân phối đã nằm trong sản phẩm mở bán của đại lý hay chưa
        /// </summary>
        /// <returns></returns>
        public bool CheckAddOpenSellDetail(int tradingProviderId, int distributionItemId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellDetailEFRepository)}->{nameof(CheckAddOpenSellDetail)}: tradingProviderId = {tradingProviderId}, distributionItemId={distributionItemId}");
            var result = from openSellDetail in _dbSet
                         join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                         where openSell.TradingProviderId == tradingProviderId && openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                         && openSellDetail.DistributionProductItemId == distributionItemId
                         && !(new List<int> { RstDistributionStatus.HET_HANG, RstDistributionStatus.HUY_DUYET }).Contains(openSell.Status)
                         select openSellDetail;
            return result.Any();
        }

        //public PagingResult<RstOpenSellDetail> FindAll(FilterRstOpenSellDetailDto input, int? tradingProviderId)
        //{
        //    _logger.LogInformation($"{nameof(RstOpenSellDetailEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}");

        //    PagingResult<RstOpenSellDetail> result = new();

        //    var openSellDetailQuery = (from openSellDetail in _dbSet
        //                               join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
        //                               join distributionProjectItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProjectItem.Id
        //                               join productItem in _epicSchemaDbContext.RstProductItems on distributionProjectItem.ProductItemId equals productItem.Id
        //                               where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
        //                               && openSell.Id == input.OpenSellId && openSell.TradingProviderId == tradingProviderId
        //                               && (input.Keyword == null || productItem.Name.Contains(input.Keyword) || productItem.Code.Contains(input.Keyword))
        //                               && (input.RedBookType == null || productItem.RedBookType == input.RedBookType)
        //                               && (input.BuildingDensityId == null || productItem.BuildingDensityId == input.BuildingDensityId)
        //                               select openSellDetail);
        //    if (input.Level != null && input.ProjectId != null)
        //    {
        //        var projectStructureFind = from openSellDetail in openSellDetailQuery
        //                                   join distributionProjectItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProjectItem.Id
        //                                   join productItem in _epicSchemaDbContext.RstProductItems on distributionProjectItem.ProductItemId equals productItem.Id
        //                                   join projectStructre in _epicSchemaDbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructre.Id
        //                                   where projectStructre.Level == input.Level && projectStructre.ProjectId == input.ProjectId
        //                                   && openSellDetail.Deleted == YesNo.NO && projectStructre.Deleted == YesNo.NO
        //                                   select openSellDetail;
        //        openSellDetailQuery = projectStructureFind;
        //    }

        //    result.TotalItems = openSellDetailQuery.Count();
        //    openSellDetailQuery = openSellDetailQuery.OrderDynamic(input.Sort);
        //    if (input.PageSize != -1)
        //    {
        //        openSellDetailQuery = openSellDetailQuery.Skip(input.Skip).Take(input.PageSize);
        //    }
        //    result.Items = openSellDetailQuery;
        //    return result;
        //}

        public PagingResult<RstOpenSellDetailDto> FindAll(FilterRstOpenSellDetailDto input, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellDetailEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}");

            PagingResult<RstOpenSellDetailDto> result = new();

            var openSellDetailQuery = (from openSellDetail in _dbSet
                                       join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                       join distributionProjectItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProjectItem.Id
                                       join productItem in _epicSchemaDbContext.RstProductItems on distributionProjectItem.ProductItemId equals productItem.Id
                                       where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                                       && openSell.Id == input.OpenSellId && openSell.TradingProviderId == tradingProviderId
                                       && (input.Keyword == null || productItem.Name.Contains(input.Keyword) || productItem.Code.Contains(input.Keyword))
                                       && (input.RedBookType == null || productItem.RedBookType == input.RedBookType)
                                       && (input.BuildingDensityId == null || productItem.BuildingDensityId == input.BuildingDensityId)
                                       && (input.Status == null || openSellDetail.Status == input.Status)
                                       select new RstOpenSellDetailDto
                                       {
                                           Id = openSellDetail.Id,
                                           OpenSellId = openSellDetail.OpenSellId,
                                           DistributionProductItemId = openSellDetail.DistributionProductItemId,
                                           IsLock = openSellDetail.IsLock,
                                           IsShowPrice = openSellDetail.IsShowPrice,
                                           ContactType = openSellDetail.ContactType,
                                           ContactPhone = openSellDetail.ContactPhone,
                                           Status = openSellDetail.Status,
                                           IsShowApp = openSellDetail.IsShowApp,
                                           Deleted = openSellDetail.Deleted,
                                           ProductItemPriceArea = productItem.PriceArea,
                                           ProductItemPrice = productItem.Price,
                                       });
            if (input.Level != null && input.ProjectId != null)
            {
                var projectStructureFind = from openSellDetail in openSellDetailQuery
                                           join distributionProjectItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProjectItem.Id
                                           join productItem in _epicSchemaDbContext.RstProductItems on distributionProjectItem.ProductItemId equals productItem.Id
                                           join projectStructre in _epicSchemaDbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructre.Id
                                           where projectStructre.Level == input.Level && projectStructre.ProjectId == input.ProjectId
                                           && openSellDetail.Deleted == YesNo.NO && projectStructre.Deleted == YesNo.NO
                                           select openSellDetail;
                openSellDetailQuery = projectStructureFind;
            }

            result.TotalItems = openSellDetailQuery.Count();
            openSellDetailQuery = openSellDetailQuery.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                openSellDetailQuery = openSellDetailQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = openSellDetailQuery;
            return result;
        }

        public IQueryable<RstOpenSellDetail> GetAllOpenSellDetailForOrder(FilterRstOpenSellDetailForOrder input, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellDetailEFRepository)}->{nameof(GetAllOpenSellDetailForOrder)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}");

            var openSellDetailQuery = (from openSellDetail in _dbSet
                                       join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                       join distributionProjectItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProjectItem.Id
                                       join distribution in _epicSchemaDbContext.RstDistributions on distributionProjectItem.DistributionId equals distribution.Id
                                       join productItem in _epicSchemaDbContext.RstProductItems on distributionProjectItem.ProductItemId equals productItem.Id
                                       where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                       && openSell.Status == RstDistributionStatus.DANG_BAN && distribution.Status == RstDistributionStatus.DANG_BAN
                                       && openSell.StartDate.Date <= DateTime.Now.Date && distribution.StartDate.Date <= DateTime.Now.Date
                                       && openSellDetail.Status == RstProductItemStatus.KHOI_TAO && productItem.Status == RstProductItemStatus.KHOI_TAO
                                       && openSell.ProjectId == input.ProjectId && openSell.TradingProviderId == tradingProviderId
                                       && ((input.Keyword == null && openSellDetail.Status < RstProductItemStatus.KHOA_CAN && productItem.Status < RstProductItemStatus.KHOA_CAN
                                            && distributionProjectItem.Status == Status.ACTIVE && openSellDetail.IsLock == YesNo.NO && productItem.IsLock == YesNo.NO)
                                            ||(input.Keyword != null && (productItem.Code.ToLower().Contains(input.Keyword.ToLower()) || productItem.Name.ToLower().Contains(input.Keyword.ToLower()))))
                                       select openSellDetail);
            if (input.Keyword != null && openSellDetailQuery.FirstOrDefault() != null)
            {
                var checkStatus = (from openSellDetail in _dbSet
                                   join distributionProjectItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProjectItem.Id
                                   join productItem in _epicSchemaDbContext.RstProductItems on distributionProjectItem.ProductItemId equals productItem.Id
                                   where openSellDetail.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                   && openSellDetail.Id == openSellDetailQuery.FirstOrDefault().Id
                                   select new
                                   {
                                       DistributionItemStatus = distributionProjectItem.Status,
                                       ProductItemStatus = productItem.Status,
                                       ProductItemIsLock = productItem.IsLock,
                                       OpenSellDetailIsLock = openSellDetail.IsLock,
                                   }).FirstOrDefault();
                // Căn bị đại lý khóa ở mở bán
                if (checkStatus.OpenSellDetailIsLock == YesNo.YES)
                {
                    ThrowException(ErrorCode.RstOpenSellDetailLockByTradingProvider);
                }

                // Căn bị đối tác khóa trên căn hộ
                if (checkStatus.ProductItemIsLock == YesNo.YES)
                {
                    ThrowException(ErrorCode.RstOpenSellDetailLockByPartner);
                }
                // Trạng thái của sản phẩm ở phân phối có bị khóa hay không A/D
                if (checkStatus.DistributionItemStatus == Status.INACTIVE)
                {
                    ThrowException(ErrorCode.RstOpenSellDetailLockInDistributionItem);
                }
                // Trạng thái sản phẩm của mở bán có khóa căn hay không
                if (checkStatus.ProductItemStatus == RstProductItemStatus.KHOA_CAN)
                {
                    ThrowException(ErrorCode.RstOpenSellDetailStatusLock);
                }
                else if (!(new List<int> { RstProductItemStatus.KHOI_TAO, RstProductItemStatus.GIU_CHO }.Contains(checkStatus.ProductItemStatus)))
                {
                    ThrowException(ErrorCode.RstOpenSellDetailStatusTrading);
                }
            }

            return openSellDetailQuery = openSellDetailQuery.OrderByDescending(d => d.Id);
        }

        /// <summary>
        /// Các thông tin liên quan đến chi tiết sản phẩm mở bán
        /// </summary>
        /// <param name="openSellDetailId"></param>
        /// <returns></returns>
        public RstOpenSellDetailInfoDto OpenSellDetailInfo(int openSellDetailId)
        {
            var result = from openSellDetail in _epicSchemaDbContext.RstOpenSellDetails
                         join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                         join distributionItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionItem.Id
                         join productItem in _epicSchemaDbContext.RstProductItems on distributionItem.ProductItemId equals productItem.Id
                         join project in _epicSchemaDbContext.RstProjects on productItem.ProjectId equals project.Id
                         join distribution in _epicSchemaDbContext.RstDistributions on distributionItem.DistributionId equals distribution.Id
                         where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                         && distributionItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                         && openSellDetail.Id == openSellDetailId
                         select new RstOpenSellDetailInfoDto
                         {
                             Id = openSellDetailId,
                             TradingProviderId = openSell.TradingProviderId,
                             OpenSellDetailId = openSellDetail.Id,
                             OpenSellId = openSell.Id,
                             DistributionId = distributionItem.DistributionId,
                             ProjectId = productItem.ProjectId,
                             ProductItemId = productItem.Id,
                             Code = productItem.Code,
                             ProjectCode = project.Code,
                             ProjectName = project.Name,
                             Name = productItem.Name,
                             DoorDirection = productItem.DoorDirection,
                             RoomType = productItem.RoomType,
                             Hotline = openSell.Hotline,
                             PriceArea = productItem.PriceArea,
                             Price = productItem.Price ?? 0,
                             ProductItemStatus = (productItem.Status == RstProductItemStatus.KHOI_TAO && openSell.StartDate.Date <= DateTime.Now.Date && distribution.StartDate.Date <= DateTime.Now.Date) 
                                                 ? RstProductItemStatus.LOGIC_DANG_MO_BAN
                                                 : productItem.Status,
                             OpenSellDetailStatus = openSellDetail.Status,
                             KeepTime = openSell.KeepTime,
                             IsShowPrice = openSellDetail.IsShowPrice,
                             ContactPhone = openSellDetail.ContactPhone,
                             ContactType = openSellDetail.ContactType,
                         };
            return result.FirstOrDefault();
        }
    }
}
