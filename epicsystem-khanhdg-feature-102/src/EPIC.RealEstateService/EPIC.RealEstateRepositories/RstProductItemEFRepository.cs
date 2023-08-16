using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.RealEstateEntities.Dto.RstDistributionProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OracleInternal.Sharding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProductItemEFRepository : BaseEFRepository<RstProductItem>
    {
        public RstProductItemEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProductItem.SEQ}")
        {
        }

        /// <summary>
        /// Thêm sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItem Add(RstProductItem input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            return _dbSet.Add(new RstProductItem()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                ProjectId = input.ProjectId,
                RedBookType = input.RedBookType,
                Code = input.Code,
                Name = input.Name,
                NumberFloor = input.NumberFloor,
                RoomType = input.RoomType,
                DoorDirection = input.DoorDirection,
                BalconyDirection = input.BalconyDirection,
                ProductLocation = input.ProductLocation,
                ProductType = input.ProductType,
                HandingType = input.HandingType,
                ViewDescription = input.ViewDescription,
                CarpetArea = input.CarpetArea,
                BuiltUpArea = input.BuiltUpArea,
                LandArea = input.LandArea,
                ConstructionArea = input.ConstructionArea,
                CompoundFloor = input.CompoundFloor,
                CompoundRoom = input.CompoundRoom,
                Price = input.Price,
                Status = RstProductItemStatus.KHOI_TAO,
                IsLock = YesNo.NO,
                BuildingDensityId = input.BuildingDensityId,
                ClassifyType = input.ClassifyType,
                UnitPrice = input.UnitPrice,
                PriceArea = input.PriceArea,
                NoFloor = input.NoFloor,
                HandoverTime = input.HandoverTime,
                FloorBuildingArea = input.FloorBuildingArea,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstProductItem Update(RstProductItem input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var productItem = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (productItem != null)
            {
                productItem.RedBookType = input.RedBookType;
                productItem.Code = input.Code;
                productItem.Name = input.Name;
                productItem.NumberFloor = input.NumberFloor;
                productItem.RoomType = input.RoomType;
                productItem.DoorDirection = input.DoorDirection;
                productItem.BalconyDirection = input.BalconyDirection;
                productItem.ProductLocation = input.ProductLocation;
                productItem.ProductType = input.ProductType;
                productItem.HandingType = input.HandingType;
                productItem.ViewDescription = input.ViewDescription;
                productItem.CarpetArea = input.CarpetArea;
                productItem.BuiltUpArea = input.BuiltUpArea;
                productItem.LandArea = input.LandArea;
                productItem.ConstructionArea = input.ConstructionArea;
                productItem.CompoundFloor = input.CompoundFloor;
                productItem.CompoundRoom = input.CompoundRoom;
                productItem.Price = input.Price;
                productItem.BuildingDensityId = input.BuildingDensityId;
                productItem.ClassifyType = input.ClassifyType;
                productItem.UnitPrice = input.UnitPrice;
                productItem.PriceArea = input.PriceArea;
                productItem.NoFloor = input.NoFloor;
                productItem.HandoverTime = input.HandoverTime;
                productItem.FloorBuildingArea = input.FloorBuildingArea;
                productItem.ModifiedBy = username;
                productItem.ModifiedDate = DateTime.Now;
            }

            return productItem;
        }

        /// <summary>
        /// Tìm sản phẩm bán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProductItem FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(FindById)}: id = {id}");

            return _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm danh sách sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstProductItem> FindAll(FilterRstProductItemDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            PagingResult<RstProductItem> result = new();
            var productItemQuery = from productItem in _dbSet
                                   let checkStatus = (from distributionProductItem in _epicSchemaDbContext.RstDistributionProductItems
                                                      join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on distributionProductItem.Id equals openSellDetail.DistributionProductItemId
                                                      join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                                      where distributionProductItem.ProductItemId == productItem.Id && distributionProductItem.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                                                      && openSell.Status != RstDistributionStatus.HUY_DUYET
                                                      select openSellDetail).Any() && productItem.Status == 1
                                   where productItem.ProjectId == input.ProjectId && productItem.Deleted == YesNo.NO
                                    && (partnerId == null || productItem.PartnerId == partnerId) && (input.Name == null || productItem.Name.Contains(input.Name))
                                    && (input.Code == null || productItem.Code.Contains(input.Code)) && (input.ClassifyType == null || productItem.ClassifyType == input.ClassifyType)
                                    && (input.BuildingDensityId == null || productItem.BuildingDensityId == input.BuildingDensityId)
                                    && (input.Status == null || productItem.Status == input.Status 
                                        || input.Status == RstProductItemStatus.LOGIC_DANG_MO_BAN && productItem.Status == RstProductItemStatus.KHOI_TAO && checkStatus
                                        || input.Status == RstProductItemStatus.LOGIC_CHUA_MO_BAN && productItem.Status == RstProductItemStatus.KHOI_TAO && !checkStatus)
                                    && (input.RedBookType == null || productItem.RedBookType == input.RedBookType)
                                   select productItem;

            result.TotalItems = productItemQuery.Count();
            productItemQuery = productItemQuery.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                productItemQuery = productItemQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = productItemQuery;
            return result;
        }

        public RstProductItem ChangeStatus(int id, int status, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(ChangeStatus)}: id = {id}, partnerId = {partnerId}, status = {status}");

            var productItem = _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);

            if (productItem != null)
            {
                productItem.Status = status;
            }

            return productItem;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm dự án có thể phân phối cho đại lý (Lọc những căn đã phân phối cho đại lý trước đó)
        /// </summary>
        public IQueryable<RstProductItem> GetAllProductItemCanDistributionForTrading(FilterRstProductItemCanDistributionDto input, int partnerId)
        {
            var distributionQuery = _epicSchemaDbContext.RstDistributions.FirstOrDefault(d => d.Id == input.DistributionId && d.PartnerId == partnerId && d.Deleted == YesNo.NO)
                                                                        .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstDistributionNotFound);
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(GetAllProductItemCanDistributionForTrading)}:  input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            // Danh sách sản phẩm đã phân phối cho đại lý
            var productItemDistributed = from distributionTrading in _epicSchemaDbContext.RstDistributions
                                         join distributionItemTrading in _epicSchemaDbContext.RstDistributionProductItems on distributionTrading.Id equals distributionItemTrading.DistributionId
                                         where distributionTrading.Deleted == YesNo.NO && distributionItemTrading.Deleted == YesNo.NO && distributionTrading.PartnerId == partnerId
                                         && distributionTrading.ProjectId == distributionQuery.ProjectId && distributionTrading.Status != RstDistributionStatus.HUY_DUYET
                                         && distributionTrading.TradingProviderId == distributionQuery.TradingProviderId
                                         select distributionItemTrading.ProductItemId;
            // Danh sách sản phẩm có thể phân phối cho đại lý
            var productItemCanDistribution = from project in _epicSchemaDbContext.RstProjects
                                             join productItem in _epicSchemaDbContext.RstProductItems on project.Id equals productItem.ProjectId
                                             where project.PartnerId == partnerId && project.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                             && project.Id == distributionQuery.ProjectId && productItem.IsLock == YesNo.NO && productItem.Status == RstProductItemStatus.KHOI_TAO
                                             && (input.Keyword == null || productItem.Name.Contains(input.Keyword) || productItem.Code.Contains(input.Keyword))
                                             && (input.BuildingDensityId == null || input.BuildingDensityId == productItem.BuildingDensityId)
                                             && (input.RedBookType == null || input.RedBookType == productItem.RedBookType)
                                             && (input.Status == null || input.Status == productItem.Status)
                                             select productItem;
            return productItemCanDistribution = productItemCanDistribution.Where(p => !productItemDistributed.Contains(p.Id)).OrderByDescending(r => r.Id);
        }

        #region App
        /// <summary>
        /// Lấy danh sách sản phẩm mở bán của đại lý theo dự án
        /// </summary>
        public IQueryable<AppGetAllProductItemDto> AppGetAllProductItem(AppFilterProductItemDto input)
        {
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(AppProductItemDetail)}: input = {JsonSerializer.Serialize(input)}");
            var productItemQuery = from openSellDetail in _epicSchemaDbContext.RstOpenSellDetails
                                   join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                   join distributionItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionItem.Id
                                   join productItem in _epicSchemaDbContext.RstProductItems on distributionItem.ProductItemId equals productItem.Id
                                   join distribution in _epicSchemaDbContext.RstDistributions on distributionItem.DistributionId equals distribution.Id
                                   join projectStructure in _epicSchemaDbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructure.Id
                                   where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                                   && openSell.StartDate.Date <= DateTime.Now.Date && distributionItem.Status == Status.ACTIVE && productItem.Deleted == YesNo.NO
                                   && distribution.Status == RstDistributionStatus.DANG_BAN && openSell.Status == RstDistributionStatus.DANG_BAN
                                   && openSell.Id == input.OpenSellId && openSell.IsShowApp == YesNo.YES && openSellDetail.IsShowApp == YesNo.YES
                                   && openSellDetail.IsLock == YesNo.NO && productItem.IsLock == YesNo.NO
                                   && (input.NoFloor == null || productItem.NoFloor == input.NoFloor)
                                   && (input.RedBook == null || input.RedBook.Contains(productItem.RedBookType.ToString()))
                                   && (input.DoorDirection == null || input.DoorDirection.Contains(productItem.DoorDirection.ToString()))
                                   && (input.MinPriceArea == null || productItem.PriceArea >= input.MinPriceArea)
                                   && (input.MaxPriceArea == null || productItem.PriceArea <= input.MaxPriceArea)
                                   && (input.MinSellingPrice == null || productItem.Price >= input.MinSellingPrice)
                                   && (input.MaxSellingPrice == null || productItem.Price <= input.MaxSellingPrice)
                                   && (input.RoomType == null || productItem.RoomType == input.RoomType)
                                   && (input.BuildingDensityId == null || productItem.BuildingDensityId == input.BuildingDensityId)
                                   && (input.Keyword == null || productItem.Code.ToLower().Contains(input.Keyword.ToLower()))
                                   select new AppGetAllProductItemDto()
                                   {
                                       Id = openSellDetail.Id,
                                       ProductItemId = productItem.Id,
                                       RedBookType = productItem.RedBookType,
                                       Code = productItem.Code,
                                       Name = productItem.Name,
                                       NoFloor = productItem.NoFloor,
                                       RoomType = productItem.RoomType,
                                       DoorDirection = productItem.DoorDirection,
                                       PriceArea = productItem.PriceArea,
                                       Price = productItem.Price,
                                       BuildingDensityId = productItem.BuildingDensityId,
                                       BuildingDensityType = projectStructure.BuildingDensityType,
                                       BuildingDensityName = projectStructure.Name,
                                       Status = (productItem.Status == RstProductItemStatus.KHOI_TAO) ? RstProductItemStatus.LOGIC_DANG_MO_BAN : productItem.Status,
                                       FloorBuildingArea = productItem.FloorBuildingArea,
                                       IsShowPrice = openSellDetail.IsShowPrice,
                                       ContactType = openSellDetail.ContactType,
                                       ContactPhone = openSellDetail.ContactPhone,

                                   };
            productItemQuery = productItemQuery.Where(r => (input.Status == null || input.Status.Contains(r.Status.ToString())
                                    || (input.Status.Contains(RstProductItemStatus.KHOA_CAN.ToString())) && r.Status == RstProductItemStatus.GIU_CHO
                                        && _epicSchemaDbContext.RstOrders.Any(o => r.ProductItemId == o.ProductItemId && o.Deleted == YesNo.NO && (o.ExpTimeDeposit == null || o.ExpTimeDeposit > DateTime.Now)
                                            && _epicSchemaDbContext.RstOrderPayments.Any(p => o.Id == p.OrderId && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN)))).OrderByDescending(x => x.Id);
            return productItemQuery;
        }

        /// <summary>
        /// Xem chi tiết sản phẩm căn từ mở bán
        /// </summary>
        public RstProductItem AppProductItemDetail(int openSellDetailId)
        {
            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(AppProductItemDetail)}: openSellDetailId = {openSellDetailId}");
            var result = from openSellDetail in _epicSchemaDbContext.RstOpenSellDetails
                         join distributionItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionItem.Id
                         join productItem in _epicSchemaDbContext.RstProductItems on distributionItem.ProductItemId equals productItem.Id
                         where openSellDetail.Id == openSellDetailId && openSellDetail.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO
                         select productItem;
            return result.FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// Tính giá trị lock căn, giá trị đặt cọc theo phân phối sản phẩm lấy ra chính sách
        /// </summary>
        public RstProductItemPriceDto ProductItemPriceByDistribution(decimal price , int distributionId)
        {
            RstProductItemPriceDto result = new();
            var distributionPolicy = _epicSchemaDbContext.RstDistributionPolicys.FirstOrDefault(r => r.DistributionId == distributionId && r.Status == Status.ACTIVE && r.Deleted == YesNo.NO);
            if (distributionPolicy != null)
            {
                result.DistributionPolicyId = distributionPolicy.Id;
                result.LockPrice = (distributionPolicy.LockType == RstDistributionPolicyTypes.GIA_CAN_HO) ? price * distributionPolicy.LockValue / 100 : distributionPolicy.LockValue;
                result.DepositPrice = (distributionPolicy.DepositType == RstDistributionPolicyTypes.GIA_CAN_HO) ? price * distributionPolicy.DepositValue / 100 : distributionPolicy.LockValue;
            }    
            return result;
        }

        /// <summary>
        /// Tính giá trị lock căn, giá trị đặt cọc theo chính sách phân phối
        /// </summary>
        public RstProductItemPriceDto ProductItemPriceByPolicy(decimal price, int policyId)
        {
            RstProductItemPriceDto result = new();
            var distributionPolicy = _epicSchemaDbContext.RstDistributionPolicys.FirstOrDefault(r => r.Id == policyId && r.Deleted == YesNo.NO);
            if (distributionPolicy != null)
            {
                result.DistributionPolicyId = distributionPolicy.Id;
                result.LockPrice = (distributionPolicy.LockType == RstDistributionPolicyTypes.GIA_CAN_HO) ? price * distributionPolicy.LockValue / 100 : distributionPolicy.LockValue;
                result.DepositPrice = (distributionPolicy.DepositType == RstDistributionPolicyTypes.GIA_CO_DINH) ? price * distributionPolicy.DepositValue / 100 : distributionPolicy.LockValue;
            }
            return result;
        }
    }
}
