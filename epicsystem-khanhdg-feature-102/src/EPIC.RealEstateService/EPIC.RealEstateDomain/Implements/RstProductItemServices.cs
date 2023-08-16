using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.FileEntities.Settings;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProductItemServices : IRstProductItemServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly ILogger<RstProductItemServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRstSignalRBroadcastServices _rstSignalRBroadcastServices;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstProductItemUtilityEFRepository _rstProductItemProjectUtilityEFRepository;
        private readonly RstProductItemProjectPolicyEFRepository _rstProductItemProjectPolicyEFRepository;
        private readonly RstProjectStructureEFRepository _rstProjectStructureEFRepository;
        private readonly RstProductItemMediaEFRepository _rstProductItemMediaEFRepository;
        private readonly RstProductItemMediaDetailEFRepository _rstProductItemMediaDetailEFRepository;
        private readonly RstProjectUtilityEFRepository _rstProjectUtilityEFRepository;
        private readonly RstProjectUtilityExtendEFRepository _rstProjectUtilityExtendEFRepository;
        private readonly RstProjectPolicyEFRepository _rstProjectPolicyEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstSellingPolicyEFRepository _rstSellingPolicyEFRepository;
        private readonly RstCartEFRepository _rstCartEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly RstProductItemExtendRepository _rstProductItemExtendRepository;
        private readonly RstApproveEFRepository _rstApproveEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstProductItemMaterialFileEFRepository _rstProductItemMaterialFileEFRepository;
        private readonly RstProductItemDesignDiagramFileEFRepository _rstProductItemDiagramFileEFRepository;

        public RstProductItemServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProductItemServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<FileConfig> fileConfig,
            IRstSignalRBroadcastServices rstSignalRBroadcastServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _fileConfig = fileConfig;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstSignalRBroadcastServices = rstSignalRBroadcastServices;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProjectUtilityEFRepository = new RstProjectUtilityEFRepository(dbContext, logger);
            _rstProjectUtilityExtendEFRepository = new RstProjectUtilityExtendEFRepository(dbContext, logger);
            _rstProductItemProjectUtilityEFRepository = new RstProductItemUtilityEFRepository(dbContext, logger);
            _rstProductItemProjectPolicyEFRepository = new RstProductItemProjectPolicyEFRepository(dbContext, logger);
            _rstProjectStructureEFRepository = new RstProjectStructureEFRepository(dbContext, logger);
            _rstProductItemMediaEFRepository = new RstProductItemMediaEFRepository(dbContext, logger);
            _rstProductItemMediaDetailEFRepository = new RstProductItemMediaDetailEFRepository(dbContext, logger);
            _rstProjectPolicyEFRepository = new RstProjectPolicyEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstSellingPolicyEFRepository = new RstSellingPolicyEFRepository(dbContext, logger);
            _rstCartEFRepository = new RstCartEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _rstProductItemExtendRepository = new RstProductItemExtendRepository(dbContext, logger);
            _rstApproveEFRepository = new RstApproveEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstProductItemMaterialFileEFRepository = new RstProductItemMaterialFileEFRepository(_dbContext, _logger);
            _rstProductItemDiagramFileEFRepository = new RstProductItemDesignDiagramFileEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Thêm sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProductItem Add(CreateRstProductItemDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var insert = _rstProductItemEFRepository.Add(_mapper.Map<RstProductItem>(input), username, partnerId);
            _rstProductItemExtendRepository.UpdateProductItemExtends(insert.Id, _mapper.Map<List<RstProductItemExtend>>(input.ProductItemExtends), username);
            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Cập nhật sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProductItem Update(UpdateRstProductItemDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var productItem = _rstProductItemEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            if (input.ClassifyType != productItem.ClassifyType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstClassifyType.GetClassifyTypeText(productItem.ClassifyType), RstClassifyType.GetClassifyTypeText(input.ClassifyType), RstFieldName.UPDATE_PRODUCT_ITEM_CLASSIFY_TYPE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.BuildingDensityId != productItem.BuildingDensityId)
            {
                var oldValue = _rstProjectStructureEFRepository.FindById(productItem.BuildingDensityId ?? 0).Name;
                var newValue = _rstProjectStructureEFRepository.FindById(input.BuildingDensityId).Name;

                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, oldValue, newValue, RstFieldName.UPDATE_PRODUCT_ITEM_BUILDING_DENSITY_ID,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.RedBookType != productItem.RedBookType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstRedBookTypes.RedBookType(productItem.RedBookType), RstRedBookTypes.RedBookType(input.RedBookType), RstFieldName.UPDATE_PRODUCT_ITEM_RED_BOOK_TYPE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.Code != productItem.Code)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.Code, input.Code, RstFieldName.UPDATE_PRODUCT_ITEM_CODE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.Name != productItem.Name)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.Name, input.Name, RstFieldName.UPDATE_PRODUCT_ITEM_NAME,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.NumberFloor != productItem.NumberFloor)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.NumberFloor, input.NumberFloor, RstFieldName.UPDATE_PRODUCT_ITEM_NUMBER_FLOOR,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.NoFloor != productItem.NoFloor)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.NoFloor, input.NoFloor, RstFieldName.UPDATE_PRODUCT_ITEM_NO_FLOOR,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.RoomType != productItem.RoomType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstRoomTypes.RoomType(productItem.RoomType), RstRoomTypes.RoomType(input.RoomType), RstFieldName.UPDATE_PRODUCT_ITEM_ROOM_TYPE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.DoorDirection != productItem.DoorDirection)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstDirections.Directions(productItem.DoorDirection), RstDirections.Directions(input.DoorDirection), RstFieldName.UPDATE_PRODUCT_ITEM_DOOR_DIRECTION,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.BalconyDirection != productItem.BalconyDirection)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstDirections.Directions(productItem.BalconyDirection), RstDirections.Directions(input.BalconyDirection), RstFieldName.UPDATE_PRODUCT_ITEM_BALCONY_DIRECTION,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.ProductLocation != productItem.ProductLocation)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstProductLocations.ProductLocation(productItem.ProductLocation), RstProductLocations.ProductLocation(input.ProductLocation), RstFieldName.UPDATE_PRODUCT_ITEM_PRODUCT_LOCATION,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.ProductType != productItem.ProductType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstProductTypes.RstProductType(productItem.ProductType), RstProductTypes.RstProductType(input.ProductType), RstFieldName.UPDATE_PRODUCT_ITEM_PRODUCT_TYPE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.HandingType != productItem.HandingType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, RstHandingTypes.HandingType(productItem.HandingType), RstHandingTypes.HandingType(input.HandingType), RstFieldName.UPDATE_PRODUCT_ITEM_HANDING_TYPE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.ViewDescription != productItem.ViewDescription)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.ViewDescription, input.ViewDescription, RstFieldName.UPDATE_PRODUCT_ITEM_VIEW_DESCRIPTION,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.HandoverTime != productItem.HandoverTime)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem?.HandoverTime?.Date.ToString(), input?.HandoverTime?.Date.ToString(), RstFieldName.UPDATE_PRODUCT_ITEM_HANDOVER_TIME,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.CarpetArea != productItem.CarpetArea)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.CarpetArea + "", input.CarpetArea + "", RstFieldName.UPDATE_PRODUCT_ITEM_CARPET_AREA,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.BuiltUpArea != productItem.BuiltUpArea)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.BuiltUpArea + "", input.BuiltUpArea + "", RstFieldName.UPDATE_PRODUCT_ITEM_BUILT_UP_AREA,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.FloorBuildingArea != productItem.FloorBuildingArea)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.FloorBuildingArea + "", input.FloorBuildingArea + "", RstFieldName.UPDATE_PRODUCT_ITEM_FLOOR_BUILDING_AREA,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.PriceArea != productItem.PriceArea)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.PriceArea + "", input.PriceArea + "", RstFieldName.UPDATE_PRODUCT_ITEM_PRICE_AREA,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.CompoundRoom != productItem.CompoundRoom)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.CompoundRoom, input.CompoundRoom, RstFieldName.UPDATE_PRODUCT_ITEM_COMPOUND_ROOM,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.CompoundFloor != productItem.CompoundFloor)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.CompoundFloor, input.CompoundFloor, RstFieldName.UPDATE_PRODUCT_ITEM_COMPOUND_FLOOR,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.UnitPrice != productItem.UnitPrice)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.UnitPrice + "", input.UnitPrice + "", RstFieldName.UPDATE_PRODUCT_ITEM_UNIT_PRICE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            if (input.Price != productItem.Price)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.Id, productItem.Price + "", input.Price + "", RstFieldName.UPDATE_PRODUCT_ITEM_PRICE,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.UPDATE, DateTime.Now, RstHistoryTypes.CapNhat), username);
            }
            var updateProductItem = _rstProductItemEFRepository.Update(_mapper.Map<RstProductItem>(input), partnerId, username);
            _rstProductItemExtendRepository.UpdateProductItemExtends(input.Id, _mapper.Map<List<RstProductItemExtend>>(input.ProductItemExtends), username);
            _dbContext.SaveChanges();
            return updateProductItem;
        }

        /// <summary>
        /// Reset trạng thái của căn hộ về trạng thái Khởi tạo
        /// </summary>
        public async Task ResetStatusProductItem(int productItemId)
        {
            _logger.LogInformation($"{nameof(Update)}: productItemId = {productItemId}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var productItem = _rstProductItemEFRepository.FindById(productItemId, partnerId)
                .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            // Lấy các sản phẩm mở bán của căn hộ
            var openSellDetails = from openSellDetail in _dbContext.RstOpenSellDetails
                                  join distributionProductItem in _dbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProductItem.Id
                                  where distributionProductItem.ProductItemId == productItemId
                                  && openSellDetail.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO
                                  select openSellDetail;

            // Update trạng thái các sản phẩm mở bán của căn hộ về Khởi tạo
            foreach (var item in openSellDetails)
            {
                item.Status = RstProductItemStatus.KHOI_TAO;
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(item.Id, null, null, null, RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.INITIALIZE, DateTime.Now, RstHistoryTypes.KhoiTao), username);
            }

            // Lấy các hợp đồng căn hộ đã có thanh toán được duyệt
            var orders = _dbContext.RstOrders.Where(o => o.ProductItemId == productItemId && o.Deleted == YesNo.NO
                                                && _dbContext.OrderPayments.Any(p => p.OrderId == o.Id && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN
                                                && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN));
            // Update sang trạng thái Phong tỏa 
            foreach (var item in orders)
            {
                item.Status = RstOrderStatus.PHONG_TOA;
                item.StatusMax = (item.Status > item.StatusMax) ? item.Status : item.StatusMax;
            }
            productItem.Status = RstProductItemStatus.KHOI_TAO;
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItem.Id, null, null, null, RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.INITIALIZE, DateTime.Now, RstHistoryTypes.KhoiTao), username);

            _dbContext.SaveChanges();
            transaction.Commit();

            // Đếm lại số lượng khi có thanh đổi
            foreach (var item in openSellDetails)
            {
                await _rstSignalRBroadcastServices.BroadcastOpenSellDetailChangeStatus(item.Id);
            }
        }

        /// <summary>
        /// Xoá sản phẩm bán
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var productItem = _rstProductItemEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);

            productItem.Deleted = YesNo.YES;
            productItem.ModifiedBy = username;
            productItem.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm sản phẩm bán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProductItemDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var productItem = _rstProductItemEFRepository.FindById(id, partnerId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            var result = _mapper.Map<RstProductItemDto>(productItem);
            result.ProductItemExtends = _mapper.Map<List<RstProductItemExtendDto>>(_rstProductItemExtendRepository.GetAll(id));
            return result;
        }

        /// <summary>
        /// Danh sách sản phẩm bán có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstProductItemDto> FindAll(FilterRstProductItemDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            var productItemQuery = _rstProductItemEFRepository.FindAll(input, partnerId);
            List<RstProductItemDto> resultItem = _mapper.Map<List<RstProductItemDto>>(productItemQuery.Items);
            foreach (var item in resultItem)
            {
                if (item.Status == RstProductItemStatus.KHOI_TAO)
                {
                    item.Status = RstProductItemStatus.LOGIC_CHUA_MO_BAN;

                    var checkStatus = (from distributionProductItem in _dbContext.RstDistributionProductItems
                                       join openSellDetail in _dbContext.RstOpenSellDetails on distributionProductItem.Id equals openSellDetail.DistributionProductItemId
                                       join openSell in _dbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                       where distributionProductItem.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                                       && distributionProductItem.ProductItemId == item.Id && openSell.Status != RstDistributionStatus.HUY_DUYET
                                       select openSellDetail).Any();
                    if (checkStatus)
                    {
                        item.Status = RstProductItemStatus.LOGIC_DANG_MO_BAN;
                    }
                }
                List<string> ListTradingProviderName = (from distributionProductItem in _dbContext.RstDistributionProductItems
                                                        join distribution in _dbContext.RstDistributions on distributionProductItem.DistributionId equals distribution.Id
                                                        join tradingprovider in _dbContext.TradingProviders on distribution.TradingProviderId equals tradingprovider.TradingProviderId
                                                        join businessCustomer in _dbContext.BusinessCustomers on tradingprovider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                                        where distributionProductItem.ProductItemId == item.Id && distributionProductItem.Deleted == YesNo.NO
                                                        && distribution.Deleted == YesNo.NO && tradingprovider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                                        select businessCustomer.Name).Distinct().ToList();
                item.ListTradingProviderName = ListTradingProviderName;
            }
            return new PagingResult<RstProductItemDto>
            {
                TotalItems = productItemQuery.TotalItems,
                Items = resultItem
            };
        }

        /// <summary>
        /// Thông tin hợp đồng mới nhất được tạo trong dự án
        /// </summary>
        public InfoOrderNewInProjectDto InfoOrderNewInProject(int projectId)
        {
            var order = _dbContext.RstOrders.Select(o => new { o.Id, o.ProductItemId, o.Status, o.Deleted })
                            .Where(o => o.Deleted == YesNo.NO && _dbContext.RstProductItems.Any(p => p.Id == o.ProductItemId && p.Deleted == YesNo.NO && p.ProjectId == projectId))
                            .OrderByDescending(o => o.Id).FirstOrDefault();
            if (order == null)
            {
                return null;
            }
            var result = _rstOrderEFRepository.InfoOrderNewInProject(order.Id);
            return result;
        }

        /// <summary>
        /// Cập nhật mô tả sơ đồ thiết kế cho căn hộ
        /// </summary>
        /// <param name="input"></param>
        public void UpdateProductItemDesignDiagramContent(UpdateRstProductItemDesignDiagramDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateProductItemDesignDiagramContent)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            var productFind = _rstProductItemEFRepository.FindById(input.Id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            productFind.DesignDiagramContent = input.DesignDiagramContent;
            productFind.DesignDiagramContentType = input.DesignDiagramContentType;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật mô tả vật liệu thi công cho căn hộ
        /// </summary>
        /// <param name="input"></param>
        public void UpdateProductItemMaterialContent(UpdateRstProductItemMaterialDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateProductItemDesignDiagramContent)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            var productFind = _rstProductItemEFRepository.FindById(input.Id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            productFind.MaterialContent = input.MaterialContent;
            productFind.MaterialContentType = input.MaterialContentType;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Khoá căn
        /// </summary>
        /// <param name="input"></param>
        public void LockProductItem(RstProductItemLockingDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(LockProductItem)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}, userId = {userId}");

            // Tìm kiếm thông tin căn
            var productItem = _rstProductItemEFRepository.FindById(input.Id, partnerId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);

            // Nếu căn hộ không nằm trong trạng thái khởi tạo hoặc giữ chỗ thì CĐT không được phép khóa căn nữa
            if (productItem.IsLock == YesNo.NO && !(new List<int> { RstProductItemStatus.KHOI_TAO, RstProductItemStatus.GIU_CHO }.Contains(productItem.Status)))
            {
                _rstProductItemEFRepository.ThrowException(ErrorCode.RstProductItemCanNotLockCuzStatusTrading);
            }

            // Lưu lại lịch sử người mở khóa căn hộ
            //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate
            //{
            //    RealTableId = productItem.Id,
            //    OldValue = RstProductItemIsLock.IsLock(productItem.IsLock),
            //    NewValue = RstProductItemIsLock.IsLock(productItem.IsLock == YesNo.NO ? YesNo.YES : YesNo.NO),
            //    FieldName = RstFieldName.UPDATE_STATUS,
            //    UpdateTable = RstHistoryUpdateTables.RST_PRODUCT_ITEM,
            //    Action = ActionTypes.CAP_NHAT,
            //    ActionUpdateType = RstActionUpdateTypes.LOCK,
            //    UpdateReason = productItem.IsLock == YesNo.NO ? input.LockingReason : RstUpdateReasons.MO_KHOA,
            //    Summary = productItem.IsLock == YesNo.NO ? input.Summary : "Mở khóa căn",
            //    CreatedDate = DateTime.Now,
            //}, username);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate
            {
                RealTableId = productItem.Id,
                OldValue = RstProductItemIsLock.IsLock(productItem.IsLock),
                NewValue = RstProductItemIsLock.IsLock(productItem.IsLock == YesNo.NO ? YesNo.YES : YesNo.NO),
                FieldName = RstFieldName.UPDATE_STATUS,
                UpdateTable = RstHistoryUpdateTables.RST_PRODUCT_ITEM,
                Action = ActionTypes.CAP_NHAT,
                ActionUpdateType = RstActionUpdateTypes.LOCK,
                UpdateReason = productItem.IsLock == YesNo.NO ? input.LockingReason : RstUpdateReasons.MO_KHOA,
                Summary = productItem.IsLock == YesNo.NO ? "Khóa căn"  : "Mở khóa căn",
                CreatedDate = DateTime.Now,
                Type = productItem.IsLock == YesNo.NO ? RstHistoryTypes.KhoaCan : RstHistoryTypes.MoCan
            }, username);

            productItem.IsLock = productItem.IsLock == YesNo.NO ? YesNo.YES : YesNo.NO;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thêm căn hộ từ file excel
        /// </summary>
        /// <param name="dto"></param>
        public void ImportExcelProductItem(ImportExcelProductItemDto dto)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ImportExcelProductItem)} : dto = {JsonSerializer.Serialize(dto)}, username={username}, partnerId = {partnerId}");
            string filePath = _fileConfig.Value.Path;
            //byte[] fileBytes;
            //using var ms = new MemoryStream();

            //dto.File.CopyTo(ms);
            //fileBytes = ms.ToArray();

            //var fileName = $"{Path.GetTempFileName()}{Path.GetExtension(dto.File.FileName)}";
            //File.WriteAllBytes(fileName, fileBytes);
            using var wb = new XLWorkbook(dto.File.OpenReadStream());

            int colBuildingDensityId = 1;
            int colClassifyType = 2;
            int colRedBookType = 3;
            int colNumberFloor = 4;
            int colName = 5;
            int colCode = 6;
            int colDoorDirection = 7;
            int colBalconyDirection = 8;
            int colProductLocation = 9;
            int colRoomType = 10;
            int colHandingType = 11;
            int colViewDescription = 12;
            int colCarpetArea = 13;
            int colBuiltUpArea = 14;
            int colPriceArea = 15;
            int colPrice = 16;
            int colUnitPrice = 17;
            int colCopySource = 18;
            int colPolicy = 19;
            int colUtility = 20;
            int colMaterial = 21;
            int colDesignDiagram = 22;
            int colMedia = 23;

            var rows = wb.Worksheet(1).RowsUsed().Skip(1); // Skip header row
            var transaction = _dbContext.Database.BeginTransaction();
            // Thêm vào productItem
            foreach (var row in rows)
            {
                var structureName = row.Cell(colBuildingDensityId)?.Value.ToString().Trim();
                if (structureName == "-" || string.IsNullOrEmpty(structureName))
                {
                    break;
                }

                var projectStructureChildren = _rstProjectStructureEFRepository.Entity
                    .FirstOrDefault(o => o.ProjectId == dto.ProjectId && o.Deleted == YesNo.NO && (o.Id + "-" + o.Code + "-" + o.Name) == structureName)
                    .ThrowIfNull($"Không tìm thấy cấu trúc dự án ở ô {row.Cell(colBuildingDensityId).Address}");
                RstProductItem copyFrom = null;
                var productItemName = row.Cell(colCopySource)?.Value.ToString().Trim();
                if (!string.IsNullOrEmpty(productItemName))
                {
                    var productItemQuery = _rstProductItemEFRepository.Entity
                        .FirstOrDefault(o => o.BuildingDensityId == projectStructureChildren.Id && o.Deleted == YesNo.NO && (o.Id + "-" + o.Code + "-" + o.Name) == productItemName)
                        .ThrowIfNull($"Không tìm thấy căn hộ trong cấu trúc: \"{projectStructureChildren.Id}-{projectStructureChildren.Name}\" ở ô {row.Cell(colCopySource).Address}");
                    copyFrom = _rstProductItemEFRepository.FindById(productItemQuery.Id, partnerId)
                    .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
                    var policy = RstCopyFromTypes.YesNoCheck(row.Cell(colPolicy)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colPolicy).Address}");
                    var utility = RstCopyFromTypes.YesNoCheck(row.Cell(colUtility)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colUtility).Address}");
                    var material = RstCopyFromTypes.YesNoCheck(row.Cell(colMaterial)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colMaterial).Address}");
                    var designDiagram = RstCopyFromTypes.YesNoCheck(row.Cell(colDesignDiagram)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colDesignDiagram).Address}");
                    var mediaCheck = RstCopyFromTypes.YesNoCheck(row.Cell(colMedia)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colMedia).Address}");
                }

                var priceArea = DecimalTryParse(row.Cell(colPriceArea)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colPriceArea).Address}");
                var price = DecimalTryParse(row.Cell(colPrice)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colPrice).Address}");
                var unitPrice = DecimalTryParse(row.Cell(colUnitPrice)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colUnitPrice).Address}");
                var landArea = DecimalTryParse(row.Cell(colCarpetArea)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colCarpetArea).Address}");
                var constructionArea = DecimalTryParse(row.Cell(colBuiltUpArea)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colBuiltUpArea).Address}");
                var carpetArea = DecimalTryParse(row.Cell(colCarpetArea)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colCarpetArea).Address}");
                var builtUpArea = DecimalTryParse(row.Cell(colBuiltUpArea)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô  {row.Cell(colBuiltUpArea).Address}");
                var name = StringCheckRequired(row.Cell(colName)?.Value.ToString().Trim(), $"Cần nhập kí tự ở ô {row.Cell(colName).Address}");
                var code = StringCheckRequired(row.Cell(colCode)?.Value.ToString().Trim(), $"Cần nhập kí tự ở ô {row.Cell(colCode).Address}");
                var classifyType = RstProductItemData.ClassifyType(row.Cell(colClassifyType)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colClassifyType).Address}");
                var doorDirection = RstProductItemData.DoorDirection(row.Cell(colDoorDirection)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colDoorDirection).Address}");
                var roomType = row.Cell(colRoomType)?.Value.ToString() == null ? RstProductItemData.RoomType(row.Cell(colRoomType)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colRoomType).Address}") : null;
                var redBookType = RstProductItemData.RedBookType(row.Cell(colRedBookType)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colRedBookType).Address}");
                var handingType = RstProductItemData.HandingType(row.Cell(colHandingType)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colHandingType).Address}");
                var projectStructureFind = _rstProjectStructureEFRepository.FindById(projectStructureChildren.Id, partnerId)
                    .ThrowIfNull(_dbContext, ErrorCode.RstProjectStructureNotFound);
                var numberFloor = row.Cell(colNumberFloor)?.Value.ToString().Trim();
                var viewDescription = row.Cell(colViewDescription)?.Value.ToString().Trim();

                var policyValue = row.Cell(colPolicy)?.Value.ToString().Trim();
                var materialValue = row.Cell(colMaterial)?.Value.ToString().Trim();
                var designDiagramValue = row.Cell(colDesignDiagram)?.Value.ToString().Trim();
                var ultilityValue = row.Cell(colUtility)?.Value.ToString().Trim();
                var mediaValue = row.Cell(colMedia)?.Value.ToString().Trim();

                var productLocation = row.Cell(colProductLocation)?.Value.ToString();
                var balconyDirection = row.Cell(colBalconyDirection)?.Value.ToString();
                // Mã căn hộ đã tồn tại trong cấu trúc
                var productItemFind = _dbContext.RstProductItems.FirstOrDefault(o => o.BuildingDensityId == projectStructureChildren.Id && o.Code == code);

                if (productItemFind != null && (productItemFind.Status == RstProductItemStatus.KHOI_TAO
                    || productItemFind.Status == RstProductItemStatus.GIU_CHO || productItemFind.Status == RstProductItemStatus.KHOA_CAN))
                {
                    productItemFind.Name = name;
                    productItemFind.ClassifyType = classifyType;
                    productItemFind.NumberFloor = numberFloor;
                    productItemFind.CarpetArea = carpetArea;
                    productItemFind.BuiltUpArea = builtUpArea;
                    productItemFind.DoorDirection = doorDirection;
                    productItemFind.ViewDescription = viewDescription;
                    productItemFind.RoomType = roomType;
                    productItemFind.Price = price;
                    productItemFind.UnitPrice = unitPrice;
                    productItemFind.LandArea = landArea;
                    productItemFind.ConstructionArea = constructionArea;
                    productItemFind.BalconyDirection = !string.IsNullOrEmpty(balconyDirection) ? RstProductItemData.DoorDirection(balconyDirection, $"Dữ liệu không hợp lệ ở ô {row.Cell(colBalconyDirection).Address}") : null;
                    productItemFind.RedBookType = redBookType;
                    productItemFind.ProductLocation = !string.IsNullOrEmpty(productLocation) ? RstProductItemData.ProductLocation(productLocation, $"Dữ liệu không hợp lệ ở ô {row.Cell(colProductLocation).Address}") : null;
                    productItemFind.HandingType = handingType;
                    productItemFind.PriceArea = priceArea;
                    if (classifyType == RstClassifyType.CanHoThongThuong || classifyType == RstClassifyType.CanHoStudio || classifyType == RstClassifyType.CanHoShophouse
                        || classifyType == RstClassifyType.CanHoPenthouse || classifyType == RstClassifyType.CanHoDuplex || classifyType == RstClassifyType.DuplexPool || classifyType == RstClassifyType.CanHoSkyVilla)
                    {
                        productItemFind.NoFloor = numberFloor;
                    }
                    else if (classifyType == RstClassifyType.NhaONongThon || classifyType == RstClassifyType.BietThuNhaO || classifyType == RstClassifyType.LienKe
                        || classifyType == RstClassifyType.ChungCuThapTang || classifyType == RstClassifyType.CanShophouse || classifyType == RstClassifyType.BietThuNghiDuong
                        || classifyType == RstClassifyType.Villa || classifyType == RstClassifyType.BoutiqueHotel)
                    {
                        productItemFind.NumberFloor = numberFloor;
                    }
                    _dbContext.SaveChanges();
                    if (copyFrom != null)
                    {
                        if (policyValue == RstCopyFromTypes.Yes)
                        {
                            var projectPolicyCopyFrom = _rstProductItemProjectPolicyEFRepository.EntityNoTracking.Where(o => o.ProductItemId == copyFrom.Id && o.Deleted == YesNo.NO);
                            var productItemFindPolicy = _rstProductItemProjectPolicyEFRepository.EntityNoTracking.Where(o => o.ProductItemId == productItemFind.Id && o.Deleted == YesNo.NO);

                            var result = projectPolicyCopyFrom.Where(pI => !productItemFindPolicy.Any(p => p.ProjectPolicyId == pI.ProjectPolicyId)).ToList();
                            if (result.Count > 0)
                            {
                                foreach (var items in result)
                                {
                                    _rstProductItemProjectPolicyEFRepository.Add(new RstProductItemProjectPolicy()
                                    {
                                        ProductItemId = productItemFind.Id,
                                        PartnerId = items.PartnerId,
                                        ProjectPolicyId = items.ProjectPolicyId,
                                        Status = items.Status,
                                        CreatedDate = DateTime.Now,
                                    }, username, partnerId);
                                }
                            }
                        }

                        if (materialValue == RstCopyFromTypes.Yes)
                        {
                            productItemFind.MaterialContent = copyFrom.MaterialContent;
                            productItemFind.MaterialContentType = copyFrom.MaterialContentType;

                            //File vật liệu
                            var productItemMaterialFiles = _dbContext.RstProductItemMaterialFiles.Where(p => p.ProductItemId == productItemFind.Id);
                            var sourceProductItemMaterialFiles = _dbContext.RstProductItemMaterialFiles.Where(p => p.ProductItemId == copyFrom.Id);
                            foreach (var sourceMaterialFile in sourceProductItemMaterialFiles)
                            {
                                var materialFile = _dbContext.RstProductItemMaterialFiles.FirstOrDefault(p => p.ProductItemId == productItemFind.Id && p.Name.ToLower() == sourceMaterialFile.Name.ToLower());
                                if (materialFile == null)
                                {
                                    _dbContext.RstProductItemMaterialFiles.Add(new RstProductItemMaterialFile
                                    {
                                        Id = (int)_rstProductItemMaterialFileEFRepository.NextKey(),
                                        ProductItemId = productItemFind.Id,
                                        Name = sourceMaterialFile.Name,
                                        FileUrl = sourceMaterialFile.FileUrl,
                                    });
                                }
                                else
                                {

                                    var file = _dbContext.RstProductItemMaterialFiles.Where(e => e.FileUrl == materialFile.FileUrl && e.Id != materialFile.Id);
                                    if (!file.Any())
                                    {
                                        var fileResult = FileUtils.GetPhysicalPath(materialFile.FileUrl, filePath);
                                        if (File.Exists(fileResult.FullPath))
                                        {
                                            File.Delete(fileResult.FullPath);
                                        }
                                    }
                                    materialFile.FileUrl = sourceMaterialFile.FileUrl;
                                }
                            }
                        }

                        if (designDiagramValue == RstCopyFromTypes.Yes)
                        {
                            productItemFind.DesignDiagramContent = copyFrom.DesignDiagramContent;
                            productItemFind.DesignDiagramContentType = copyFrom.DesignDiagramContentType;

                            //File sơ đồ thiết kế
                            var productItemDesignDiagramFiles = _dbContext.RstProductItemDesignDiagramFiles.Where(p => p.ProductItemId == productItemFind.Id);
                            var sourceProductItemDesignDiagramFiles = _dbContext.RstProductItemDesignDiagramFiles.Where(p => p.ProductItemId == copyFrom.Id);
                            foreach (var sourceDesignDiagramFile in sourceProductItemDesignDiagramFiles)
                            {
                                var designDiagramFile = _dbContext.RstProductItemDesignDiagramFiles.FirstOrDefault(p => p.ProductItemId == productItemFind.Id && p.Name.ToLower() == sourceDesignDiagramFile.Name.ToLower());
                                if (designDiagramFile == null)
                                {
                                    _dbContext.RstProductItemDesignDiagramFiles.Add(new RstProductItemDesignDiagramFile
                                    {
                                        Id = (int)_rstProductItemDiagramFileEFRepository.NextKey(),
                                        ProductItemId = productItemFind.Id,
                                        Name = sourceDesignDiagramFile.Name,
                                        FileUrl = sourceDesignDiagramFile.FileUrl,
                                    });
                                }
                                else
                                {
                                    var file = _dbContext.RstProductItemDesignDiagramFiles.Where(e => e.FileUrl == designDiagramFile.FileUrl && e.Id != designDiagramFile.Id);
                                    if (!file.Any()) 
                                    {
                                        var fileResult = FileUtils.GetPhysicalPath(designDiagramFile.FileUrl, filePath);
                                        if (File.Exists(fileResult.FullPath))
                                        {
                                            File.Delete(fileResult.FullPath);
                                        }
                                    }
                                    designDiagramFile.FileUrl = sourceDesignDiagramFile.FileUrl;
                                }
                            }
                        }

                        if (ultilityValue == RstCopyFromTypes.Yes)
                        {
                            var ultilityCopyFrom = _rstProductItemProjectUtilityEFRepository.EntityNoTracking.Where(o => o.ProductItemId == copyFrom.Id && o.Deleted == YesNo.NO);
                            var productItemFindUltility = _rstProductItemProjectUtilityEFRepository.EntityNoTracking.Where(o => o.ProductItemId == productItemFind.Id && o.Deleted == YesNo.NO);
                            var result = ultilityCopyFrom.Where(pI => !productItemFindUltility.Any(u => u.ProductItemUtilityId == pI.ProductItemUtilityId)).ToList();
                            foreach (var items in result)
                            {
                                _rstProductItemProjectUtilityEFRepository.Add(new RstProductItemUtility()
                                {
                                    ProductItemId = productItemFind.Id,
                                    PartnerId = items.PartnerId,
                                    ProductItemUtilityId = items.ProductItemUtilityId,
                                    Status = items.Status,
                                    CreatedDate = DateTime.Now,
                                }, username, partnerId);
                            }
                        }

                        if (mediaValue == RstCopyFromTypes.Yes)
                        {
                            var mediaCopyFrom = _rstProductItemMediaEFRepository.EntityNoTracking.Where(o => o.ProductItemId == copyFrom.Id && o.Deleted == YesNo.NO && o.GroupTitle == null);
                            var productItemFindMedia = _rstProductItemMediaEFRepository.EntityNoTracking.Where(o => o.ProductItemId == productItemFind.Id && o.Deleted == YesNo.NO && o.GroupTitle == null);
                            var result = mediaCopyFrom.Where(pI => !productItemFindMedia.Any(m => m.UrlImage == pI.UrlImage)).ToList();
                            foreach (var items in result)
                            {
                                var mediaDetail = _rstProductItemMediaDetailEFRepository.EntityNoTracking.Where(o => o.ProductItemMediaId == items.Id);

                                var newMedia = _rstProductItemMediaEFRepository.Add(new RstProductItemMedia()
                                {
                                    ProductItemId = productItemFind.Id,
                                    PartnerId = items.PartnerId,
                                    GroupTitle = items.GroupTitle,
                                    Location = items.Location,
                                    MediaType = items.MediaType,
                                    UrlImage = items.UrlImage,
                                    UrlPath = items.UrlPath,
                                    Status = items.Status,
                                    CreatedDate = DateTime.Now,
                                }, username, partnerId);
                            }
                            //check mediaDetail
                            var listMediaCopyFrom = _rstProductItemMediaEFRepository.EntityNoTracking.Where(o => o.ProductItemId == copyFrom.Id && o.Deleted == YesNo.NO && o.GroupTitle != null);
                            var listProductItemFindMedia = _rstProductItemMediaEFRepository.EntityNoTracking.Where(o => o.ProductItemId == productItemFind.Id && o.Deleted == YesNo.NO && o.GroupTitle != null);
                            var mediaSameGroupTitle = listMediaCopyFrom.Where(pI => listProductItemFindMedia.Any(m => m.GroupTitle == pI.GroupTitle)).ToList();
                            foreach (var items in mediaSameGroupTitle)
                            {
                                var mediaDetailCopyFrom = _rstProductItemMediaDetailEFRepository.EntityNoTracking.Where(o => o.ProductItemMediaId == items.Id && o.Deleted == YesNo.NO);
                                var mediaDetailproductFind = _rstProductItemMediaDetailEFRepository.EntityNoTracking.Where(o => o.ProductItemMediaId == productItemFind.Id && o.Deleted == YesNo.NO);
                                var mediaDetailResult = mediaDetailCopyFrom.Where(pI => !mediaDetailproductFind.Any(m => m.UrlImage == pI.UrlImage)).ToList();
                                foreach (var itemMediaDetail in mediaDetailResult)
                                {
                                    _rstProductItemMediaDetailEFRepository.Add(new RstProductItemMediaDetail()
                                    {
                                        ProductItemMediaId = itemMediaDetail.ProductItemMediaId,
                                        PartnerId = partnerId,
                                        UrlImage = itemMediaDetail.UrlImage,
                                        MediaType = itemMediaDetail.MediaType,
                                        Status = itemMediaDetail.Status,
                                        CreatedDate = DateTime.Now,
                                    }, username, partnerId);
                                }
                            }
                            
                            var media = listMediaCopyFrom.Where(pI => !listProductItemFindMedia.Any(m => m.GroupTitle == pI.GroupTitle)).ToList();
                            foreach (var items in media)
                            {
                                var mediaDetail = _rstProductItemMediaDetailEFRepository.EntityNoTracking.Where(o => o.ProductItemMediaId == items.Id);

                                var newMedia = _rstProductItemMediaEFRepository.Add(new RstProductItemMedia()
                                {
                                    ProductItemId = productItemFind.Id,
                                    PartnerId = items.PartnerId,
                                    GroupTitle = items.GroupTitle,
                                    Location = items.Location,
                                    MediaType = items.MediaType,
                                    UrlImage = items.UrlImage,
                                    UrlPath = items.UrlPath,
                                    Status = items.Status,
                                    CreatedDate = DateTime.Now,
                                }, username, partnerId);


                                foreach (var itemMediaDetail in mediaDetail)
                                {
                                    _rstProductItemMediaDetailEFRepository.Add(new RstProductItemMediaDetail()
                                    {
                                        ProductItemMediaId = newMedia.Id,
                                        PartnerId = partnerId,
                                        UrlImage = itemMediaDetail.UrlImage,
                                        MediaType = itemMediaDetail.MediaType,
                                        Status = itemMediaDetail.Status,
                                        CreatedDate = DateTime.Now,
                                    }, username, partnerId);
                                }
                            }

                        }
                        _dbContext.SaveChanges();
                    }
                }
                else
                {
                    var productItem = _rstProductItemEFRepository.Add(new RstProductItem
                    {
                        ProjectId = projectStructureFind.ProjectId,
                        Name = name,
                        BuildingDensityId = projectStructureChildren.Id,
                        ClassifyType = classifyType,
                        Code = code,
                        CarpetArea = carpetArea,
                        BuiltUpArea = builtUpArea,
                        DoorDirection = doorDirection,
                        ViewDescription = viewDescription,
                        RoomType = roomType,
                        Price = price,
                        UnitPrice = unitPrice,
                        LandArea = landArea,
                        ConstructionArea = constructionArea,
                        BalconyDirection = !string.IsNullOrEmpty(balconyDirection) ? RstProductItemData.DoorDirection(balconyDirection, $"Dữ liệu không hợp lệ ở ô {row.Cell(colBalconyDirection).Address}") : null,
                        RedBookType = redBookType,
                        ProductLocation = !string.IsNullOrEmpty(productLocation) ? RstProductItemData.ProductLocation(productLocation, $"Dữ liệu không hợp lệ ở ô {row.Cell(colProductLocation).Address}") : null,
                        HandingType = handingType,
                        PriceArea = priceArea,
                    }, username, partnerId);
                    _dbContext.SaveChanges();

                    if (classifyType == RstClassifyType.CanHoThongThuong || classifyType == RstClassifyType.CanHoStudio || classifyType == RstClassifyType.CanHoShophouse
                        || classifyType == RstClassifyType.CanHoPenthouse || classifyType == RstClassifyType.CanHoDuplex || classifyType == RstClassifyType.DuplexPool || classifyType == RstClassifyType.CanHoSkyVilla)
                    {
                        productItem.NoFloor = numberFloor;
                    }
                    else if (classifyType == RstClassifyType.NhaONongThon || classifyType == RstClassifyType.BietThuNhaO || classifyType == RstClassifyType.LienKe
                        || classifyType == RstClassifyType.ChungCuThapTang || classifyType == RstClassifyType.CanShophouse || classifyType == RstClassifyType.BietThuNghiDuong
                        || classifyType == RstClassifyType.Villa || classifyType == RstClassifyType.BoutiqueHotel)
                    {
                        productItem.NumberFloor = numberFloor;
                    }

                    if (copyFrom != null)
                    {
                        if (policyValue == RstCopyFromTypes.Yes)
                        {
                            CreatePolicy(productItem, copyFrom.Id);
                        }

                        if (materialValue == RstCopyFromTypes.Yes)
                        {
                            productItem.MaterialContent = copyFrom.MaterialContent;
                            productItem.MaterialContentType = copyFrom.MaterialContentType;

                            //File vật liệu
                            var productItemMaterialFiles = _dbContext.RstProductItemMaterialFiles.Where(p => p.ProductItemId == productItemFind.Id);
                            var sourceProductItemMaterialFiles = _dbContext.RstProductItemMaterialFiles.Where(p => p.ProductItemId == copyFrom.Id);
                            foreach (var sourceMaterialFile in sourceProductItemMaterialFiles)
                            {
                                _dbContext.RstProductItemMaterialFiles.Add(new RstProductItemMaterialFile
                                {
                                    Id = (int)_rstProductItemMaterialFileEFRepository.NextKey(),
                                    ProductItemId = productItem.Id,
                                    Name = sourceMaterialFile.Name,
                                    FileUrl = sourceMaterialFile.FileUrl,
                                });
                            }
                        }

                        if (designDiagramValue == RstCopyFromTypes.Yes)
                        {
                            productItem.DesignDiagramContent = copyFrom.DesignDiagramContent;
                            productItem.DesignDiagramContentType = copyFrom.DesignDiagramContentType;

                            //File sơ đồ thiết kế
                            var productItemDesignDiagramFiles = _dbContext.RstProductItemDesignDiagramFiles.Where(p => p.ProductItemId == productItemFind.Id);
                            var sourceProductItemDesignDiagramFiles = _dbContext.RstProductItemDesignDiagramFiles.Where(p => p.ProductItemId == copyFrom.Id);
                            foreach (var sourceDesignDiagramFile in sourceProductItemDesignDiagramFiles)
                            {
                                _dbContext.RstProductItemDesignDiagramFiles.Add(new RstProductItemDesignDiagramFile
                                {
                                    Id = (int)_rstProductItemDiagramFileEFRepository.NextKey(),
                                    ProductItemId = productItem.Id,
                                    Name = sourceDesignDiagramFile.Name,
                                    FileUrl = sourceDesignDiagramFile.FileUrl,
                                });
                            }
                        }

                        if (ultilityValue == RstCopyFromTypes.Yes)
                        {
                            CreateUltility(productItem, copyFrom.Id);
                        }

                        if (mediaValue == RstCopyFromTypes.Yes)
                        {
                            CreateMedia(productItem, copyFrom.Id);
                        }
                    }
                    _dbContext.SaveChanges();
                }
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        private void CreateUltility(RstProductItem input, int productItemId)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(CreateUltility)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            var ultility = _rstProductItemProjectUtilityEFRepository.EntityNoTracking.Where(o => o.ProductItemId == productItemId && o.Deleted == YesNo.NO);
            foreach (var items in ultility)
            {
                _rstProductItemProjectUtilityEFRepository.Add(new RstProductItemUtility()
                {
                    ProductItemId = input.Id,
                    PartnerId = items.PartnerId,
                    ProductItemUtilityId = items.ProductItemUtilityId,
                    Status = items.Status,
                    CreatedDate = DateTime.Now,
                }, username, partnerId);
            }
        }

        private void CreatePolicy(RstProductItem input, int productItemId)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(CreateUltility)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            var projectPolicy = _rstProductItemProjectPolicyEFRepository.EntityNoTracking.Where(o => o.ProductItemId == productItemId && o.Deleted == YesNo.NO);
            foreach (var items in projectPolicy)
            {
                _rstProductItemProjectPolicyEFRepository.Add(new RstProductItemProjectPolicy()
                {
                    ProductItemId = input.Id,
                    PartnerId = items.PartnerId,
                    ProjectPolicyId = items.ProjectPolicyId,
                    Status = items.Status,
                    CreatedDate = DateTime.Now,
                }, username, partnerId);
            }
        }

        private void CreateMedia(RstProductItem input, int productItemId)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(CreateUltility)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            var media = _rstProductItemMediaEFRepository.EntityNoTracking.Where(o => o.ProductItemId == productItemId && o.Deleted == YesNo.NO);
            foreach (var items in media)
            {
                var mediaDetail = _rstProductItemMediaDetailEFRepository.EntityNoTracking.Where(o => o.ProductItemMediaId == items.Id);

                var newMedia = _rstProductItemMediaEFRepository.Add(new RstProductItemMedia()
                {
                    ProductItemId = input.Id,
                    PartnerId = items.PartnerId,
                    GroupTitle = items.GroupTitle,
                    Location = items.Location,
                    MediaType = items.MediaType,
                    UrlImage = items.UrlImage,
                    UrlPath = items.UrlPath,
                    Status = items.Status,
                    CreatedDate = DateTime.Now,
                }, username, partnerId);


                foreach (var itemMediaDetail in mediaDetail)
                {
                    _rstProductItemMediaDetailEFRepository.Add(new RstProductItemMediaDetail()
                    {
                        ProductItemMediaId = newMedia.Id,
                        PartnerId = partnerId,
                        UrlImage = itemMediaDetail.UrlImage,
                        MediaType = itemMediaDetail.MediaType,
                        Status = itemMediaDetail.Status,
                        CreatedDate = DateTime.Now,
                    }, username, partnerId);
                }
            }
        }
        /// <summary>
        /// Ép kiểu decimal dữ liệu từ excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private decimal DecimalTryParse(string data, string message)
        {
            if (!decimal.TryParse(data, out decimal result))
            {
                _defErrorEFRepository.ThrowException(message);
            }
            return result;
        }

        /// <summary>
        /// Ép kiểu int dữ liệu từ excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private int IntTryParse(string data, string message)
        {
            if (!int.TryParse(data, out int result))
            {
                _defErrorEFRepository.ThrowException(message);
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra trường text bắt buộc nhập
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private string StringCheckRequired(string data, string message)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                _defErrorEFRepository.ThrowException(message);
            }
            return data;
        }

        private string BuildingDensityTextType(string data, string message)
        {
            var text = $"{data}-{data}-{data}";

            if (data != text)
            {
                _defErrorEFRepository.ThrowException(message);
            }
            return data;
        }

        /// <summary>
        /// Nhân bản căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        public List<RstProductItem> ReplicateProductItem(CreateRstListProductItemReplicationDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ReplicateProductItem)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            // Tìm kiếm thông tin căn
            var productItem = _rstProductItemEFRepository.FindById(input.ProductItemId, partnerId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            var listProductItemReplication = new List<RstProductItem>();
            foreach (var item in input.Items)
            {
                var productItemReplication = _rstProductItemEFRepository.Add(new RstProductItem
                {
                    ProjectId = productItem.ProjectId,
                    RedBookType = productItem.RedBookType,
                    Code = item.Code,
                    Name = item.Name,
                    NumberFloor = item.NumberFloor ?? productItem.NumberFloor,
                    RoomType = productItem.RoomType,
                    DoorDirection = productItem.DoorDirection,
                    BalconyDirection = productItem.BalconyDirection,
                    ProductLocation = productItem.ProductLocation,
                    ProductType = productItem.ProductType,
                    HandingType = productItem.HandingType,
                    ViewDescription = productItem.ViewDescription,
                    CarpetArea = productItem.CarpetArea,
                    BuiltUpArea = productItem.BuiltUpArea,
                    LandArea = productItem.LandArea,
                    ConstructionArea = productItem.ConstructionArea,
                    CompoundFloor = productItem.CompoundFloor,
                    CompoundRoom = productItem.CompoundRoom,
                    Price = productItem.Price,
                    Status = RstProductItemStatus.KHOI_TAO,
                    BuildingDensityId = productItem.BuildingDensityId,
                    ClassifyType = productItem.ClassifyType,
                    UnitPrice = productItem.UnitPrice,
                    PriceArea = productItem.PriceArea,
                    NoFloor = productItem.NoFloor,
                    HandoverTime = productItem.HandoverTime,
                    FloorBuildingArea = productItem.FloorBuildingArea,
                }, username, partnerId);
                if (input.VatLieu)
                {
                    productItemReplication.MaterialContent = productItem.MaterialContent;
                    productItemReplication.MaterialContentType = productItem.MaterialContentType;
                }

                if (input.SoDoThietKe)
                {
                    productItemReplication.DesignDiagramContent = productItem.DesignDiagramContent;
                    productItemReplication.DesignDiagramContentType = productItem.DesignDiagramContentType;
                }

                if (item.NoFloor != null)
                {
                    productItemReplication.NoFloor = item.NoFloor;
                }
                listProductItemReplication.Add(productItemReplication);

                if (input.ChinhSachUuDaiCDT)
                {
                    CreatePolicy(productItemReplication, input.ProductItemId);
                }

                if (input.TienIch)
                {
                    CreateUltility(productItemReplication, input.ProductItemId);
                }

                if (input.HinhAnh)
                {
                    CreateMedia(productItemReplication, input.ProductItemId);
                }
            }
            _dbContext.SaveChanges();
            return listProductItemReplication;
        }

        /// <summary>
        /// Đổi trạng thái căn hộ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstProductItem ChangeStatus(int id, int status)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, partnerId = {partnerId}");

            var productItem = _rstProductItemEFRepository.FindById(id).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            var changeStatus = _rstProductItemEFRepository.ChangeStatus(id, status, partnerId);
            productItem.ModifiedBy = username;
            productItem.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return changeStatus;
        }

        public ExportResultDto ImportFileTemplate(int projectId)
        {
            string fileName = "Temp_Update_BDS.xlsx";
            string path = Path.Combine(Environment.CurrentDirectory, "Data", "ExcelTemplate", fileName);
            using var workbook = new XLWorkbook(path);
            var ws = workbook.Worksheet(1);
            const string sheetDataValidation = "DV";
            var dataValidation = workbook.Worksheets.Add(sheetDataValidation);
            const int maxRow = 1000;

            ws.Range($"J2:J{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstProductItemRoomTypeText.All.ToArray())}\"");

            ws.Range($"B2:B{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstProductItemClassifyTypeText.All.ToArray())}\"");

            ws.Range($"C2:C{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstRedBookTypeText.All.ToArray())}\"");

            ws.Range($"G2:G{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstProductItemDirectionText.All.ToArray())}\"");

            ws.Range($"H2:H{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstProductItemDirectionText.All.ToArray())}\"");

            ws.Range($"I2:I{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstProductLocationText.All.ToArray())}\"");

            ws.Range($"K2:K{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstHandingTypeText.All.ToArray())}\"");

            var projectStructure = new List<string>();
            var productItemList = new List<string>();
            var childList = _rstProjectStructureEFRepository.FindAllChildByProjectId(projectId).ToList();

            var initialIndex = 1;
            StringBuilder templateValidation = new("=\"\"");
            var productIndex = 0;
            for (int projectIndex = 0; projectIndex < childList.Count; projectIndex++)
            {
                var id = childList[projectIndex].Id;
                var code = childList[projectIndex].Code;
                var name = childList[projectIndex].Name;
                var projecStructureString = string.Join('-', id, code, name);
                var cellProjectStructure = dataValidation.Cell(projectIndex + 1, 1);
                var projectStructureValue = cellProjectStructure.SetValue(projecStructureString);
                projectStructure.Add(projecStructureString);
                var productItem = _rstProductItemEFRepository.Entity.Where(o => o.BuildingDensityId == id).ToList();
                if (productItem.Count == 0)
                {
                    continue;
                }
                foreach (var product in productItem)
                {
                    var productItemId = product.Id;
                    var productItemCode = product.Code;
                    var productItemName = product.Name;
                    var productItemString = string.Join('-', productItemId, productItemCode, productItemName);
                    var productItemCell = dataValidation.Cell(++productIndex, 2);
                    var productItemValue = productItemCell.SetValue(productItemString);
                }
                templateValidation.Replace("\"\"", $"IF(@={sheetDataValidation}!{cellProjectStructure},{sheetDataValidation}!B{initialIndex}:B{productIndex},\"\")");
                initialIndex = productIndex + 1;
            }

            var templateString = templateValidation.ToString();
            for (int index = 2; index <= maxRow; index++)
            {
                ws.Cell(index, 1).Value = "-";
                var stringFormula = templateString.Replace("@", $"{ws.Cell(index, 1)}");
                ws.Cell($"R{index}")
                    .CreateDataValidation()
                    .List(stringFormula);
            }

            var projectStructureRange = dataValidation.Cell(1, 1).InsertData(projectStructure);

            ws.Range($"A2:A{maxRow}")
                .CreateDataValidation()
                .List(dataValidation.Range($"A1:A{maxRow}"));

            ws.Range($"S2:S{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstCopyFromTypes.All.ToArray())}\"");

            ws.Range($"T2:T{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstCopyFromTypes.All.ToArray())}\"");

            ws.Range($"U2:U{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstCopyFromTypes.All.ToArray())}\"");

            ws.Range($"V2:V{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstCopyFromTypes.All.ToArray())}\"");

            ws.Range($"W2:W{maxRow}")
                .CreateDataValidation()
                .List($"\"{string.Join(",", RstCopyFromTypes.All.ToArray())}\"");

            MemoryStream stream = new();
            workbook.SaveAs(stream);
            return new ExportResultDto
            {
                fileData = stream.ToArray(),
                fileDownloadName = fileName
            };
        }

        /// <summary>
        /// Lấy danh sách sản phẩm dự án có thể phân phối cho đại lý (Lọc những căn đã phân phối cho đại lý trước đó)
        /// </summary>
        public List<RstProductItemDto> GetAllProductItemCanDistributionForTrading(FilterRstProductItemCanDistributionDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllProductItemCanDistributionForTrading)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");
            var productItemQuery = _rstProductItemEFRepository.GetAllProductItemCanDistributionForTrading(input, partnerId);
            List<RstProductItemDto> result = _mapper.Map<List<RstProductItemDto>>(productItemQuery);
            return result;
        }

        #region Tiện ích căn hộ

        /// <summary>
        /// Lấy danh sách tiện ích có thể chọn
        /// </summary>
        /// <param name="input"></param>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        public PagingResult<RstSelectProductItemProjectUtilityDto> FindAllProjectUtility(FilterProductItemProjectUtilityDto input)
        {
            _logger.LogInformation($"{nameof(FindAllProjectUtility)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = new PagingResult<RstSelectProductItemProjectUtilityDto>();
            var resultItems = new List<RstSelectProductItemProjectUtilityDto>();

            var productItemFind = _rstProductItemEFRepository.FindById(input.ProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);

            //Lấy ra danh sách tiện ích của dự án cài đặt
            var projectUtilities = _rstProjectUtilityEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProjectId == productItemFind.ProjectId && e.Deleted == YesNo.NO);
            var projectUtilitiyExtends = _rstProjectUtilityExtendEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProjectId == productItemFind.ProjectId && e.Deleted == YesNo.NO);

            //Data Utility
            var listDefault = RstProjectUtilityData.UtilityData;
            var groupUtilityData = GroupRstProjectUtility.GroupData;

            var productItemUtilityQuery = _rstProductItemProjectUtilityEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO);

            var query = from utility in listDefault
                        join projectUtility in projectUtilities on utility.Id equals projectUtility.UtilityId
                        join productItemUtility in productItemUtilityQuery on projectUtility.Id equals productItemUtility.ProductItemUtilityId into itemUtilitys
                        from itemUtility in itemUtilitys.DefaultIfEmpty()
                        where (input.Keyword == null || utility.Name.Contains(input.Keyword) || utility.Id.ToString() == input.Keyword)
                        && (input.Selected == null || itemUtilitys.Any())
                        && (input.Status == null || itemUtility.Status == input.Status)
                        && (input.UtilityId == null || itemUtility.Id == input.UtilityId)
                        select new
                        {
                            utility,
                            projectUtility,
                            itemUtility
                        };
            var productItemUtilityExtend = productItemUtilityQuery.Where(e => e.ProjectUtilityExtendId != null);

            var utilityExtendQuery = from utilityExtend in projectUtilitiyExtends
                                     join productItemUtility in productItemUtilityExtend on utilityExtend.Id equals productItemUtility.ProjectUtilityExtendId into productItemUtilityUtilitys
                                     from productItemUtility in productItemUtilityUtilitys.DefaultIfEmpty()
                                     where utilityExtend.Status == Status.ACTIVE
                                     && (input.Selected == null || productItemUtility != null)
                                     && (input.Keyword == null || utilityExtend.Title.Contains(input.Keyword) || utilityExtend.Id.ToString() == input.Keyword)
                                     && (input.UtilityId == null || utilityExtend.Id == input.UtilityId)
                                     select new
                                     {
                                         productItemUtility,
                                         utilityExtend
                                     };
            result.TotalItems = query.Count();
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            foreach (var item in query)
            {
                //Lấy ra tên và nhóm tiện ích
                resultItems.Add(new RstSelectProductItemProjectUtilityDto()
                {
                    Id = item.itemUtility?.Id,
                    ProjectUtilityId = item.projectUtility.Id,
                    UtilityId = item.projectUtility.UtilityId,
                    Name = item.utility.Name,
                    IsHighlight = item.projectUtility.IsHighlight,
                    Type = item.utility.Type,
                    IsProductItemSelected = item.itemUtility == null ? YesNo.NO : YesNo.YES,
                    Status = item.itemUtility?.Status
                });
            }

            foreach (var item in utilityExtendQuery)
            {
                //Lấy ra tên và nhóm tiện ích
                resultItems.Add(new RstSelectProductItemProjectUtilityDto()
                {
                    Id = item.productItemUtility?.Id,
                    ProjectUtilityExtendId = item.utilityExtend.Id,
                    Name = item.utilityExtend.Title,
                    Type = item.utilityExtend.Type,
                    IsProductItemSelected = item.productItemUtility == null ? YesNo.NO : YesNo.YES,
                    Status = item.productItemUtility?.Status
                });
            }

            result.Items = resultItems;
            return result;
        }

        /// <summary>
        /// Thêm danh sách tiện ích được chọn
        /// </summary>
        /// <param name="input"></param>
        public void AddProductItemProjectUtility(CreateRstProductItemUtilityDto input)
        {
            _logger.LogInformation($"{nameof(AddProductItemProjectUtility)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUserType(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var productItemFind = _rstProductItemEFRepository.FindById(input.ProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            //Lấy ra danh sách tiện ích của dự án cài đặt
            var listItemUtility = _rstProjectUtilityEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProjectId == productItemFind.ProjectId && e.Deleted == YesNo.NO).Select(e => e.Id).ToList();
            var listItemUtilityExtend = _rstProjectUtilityExtendEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProjectId == productItemFind.ProjectId && e.Deleted == YesNo.NO).Select(e => e.Id).ToList();

            //Lấy ra danh sách tiện ích trong Db
            var getAllUtility = _rstProductItemProjectUtilityEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProductItemUtilityId != null && e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO).Select(e => e.ProductItemUtilityId).ToList();
            var getAllUtilityExtend = _rstProductItemProjectUtilityEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProjectUtilityExtendId != null && e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO).Select(e => e.ProjectUtilityExtendId).ToList();
            //Check đầu vàos
            if (listItemUtility.Except(input.ProductItemUtilities.Cast<int>()).Count() == listItemUtility.Count() && input.ProductItemUtilities.Count() > 0)
            {
                _rstProductItemEFRepository.ThrowException(ErrorCode.RstProjectUtilityIsNotSelected);
            }

            //Check đầu vàos
            if (listItemUtilityExtend.Except(input.ProjectUtilityExtends.Cast<int>()).Count() == listItemUtilityExtend.Count() && input.ProjectUtilityExtends.Count() > 0)
            {
                _rstProductItemEFRepository.ThrowException(ErrorCode.RstProjectUtilityIsNotSelected);
            }

            //Xóa những tiện ích có trong db nhưng truyền vào không có
            var deleteProductItemUtilitys = getAllUtility.Except(input.ProductItemUtilities);
            var deleteProductItemUtilityExtends = getAllUtilityExtend.Except(input.ProjectUtilityExtends);
            foreach (var deleteItem in deleteProductItemUtilitys)
            {
                var deleteProductItemUtility = _rstProductItemProjectUtilityEFRepository.Entity.FirstOrDefault(e => e.PartnerId == partnerId && e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO && e.ProductItemUtilityId == deleteItem);
                deleteProductItemUtility.Deleted = YesNo.YES;
                deleteProductItemUtility.ModifiedDate = DateTime.Now;
                deleteProductItemUtility.ModifiedBy = username;
            }

            foreach (var deleteItem in deleteProductItemUtilityExtends)
            {
                var deleteProductItemUtility = _rstProductItemProjectUtilityEFRepository.Entity.FirstOrDefault(e => e.PartnerId == partnerId && e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO && e.ProjectUtilityExtendId == deleteItem);
                deleteProductItemUtility.Deleted = YesNo.YES;
                deleteProductItemUtility.ModifiedDate = DateTime.Now;
                deleteProductItemUtility.ModifiedBy = username;
            }

            //Thêm nhiều tiện ích vào căn hộ
            var insertProductItemUtilitys = input.ProductItemUtilities.Except(getAllUtility);
            var insertProductItemUtilityExtends = input.ProjectUtilityExtends.Except(getAllUtilityExtend);
            foreach (var insertItem in insertProductItemUtilitys)
            {
                var insertProductUtility = new RstProductItemUtility()
                {
                    ProductItemId = input.ProductItemId,
                    ProductItemUtilityId = insertItem
                };
                _rstProductItemProjectUtilityEFRepository.Add(insertProductUtility, username, partnerId);
            }

            foreach (var insertItem in insertProductItemUtilityExtends)
            {
                var insertProductUtility = new RstProductItemUtility()
                {
                    ProductItemId = input.ProductItemId,
                    ProjectUtilityExtendId = insertItem
                };
                _rstProductItemProjectUtilityEFRepository.Add(insertProductUtility, username, partnerId);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Find All danh sách đã được chọn
        /// </summary>
        /// <param name="input"></param>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        public PagingResult<RstProductItemUtilityDto> FindAllProjectUtilitySelected(FilterProductItemProjectUtilityDto input, int productItemId)
        {
            _logger.LogInformation($"{nameof(AddProductItemProjectUtility)}: input = {JsonSerializer.Serialize(input)}, productItemId = {productItemId}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUserType(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var listItem = new List<RstProductItemUtilityDto>();
            var result = new PagingResult<RstProductItemUtilityDto>();

            //Data Utility
            var listDefault = RstProjectUtilityData.UtilityData;
            var groupUtilityData = GroupRstProjectUtility.GroupData;

            //Lấy ra danh sách tiện ích trong Db
            var getAllUtility = _rstProductItemProjectUtilityEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProductItemId == productItemId && e.Deleted == YesNo.NO);
            foreach (var productItemUtility in getAllUtility)
            {
                var resultItem = new RstProductItemUtilityDto();
                var utility = listDefault.FirstOrDefault(e => e.Id == productItemUtility.ProductItemUtilityId);
                resultItem.Id = productItemUtility.Id;
                resultItem.IsProductItemSelected = YesNo.YES;
                resultItem.UtilityId = utility.Id;
                resultItem.Status = productItemUtility.Status;
                resultItem.Name = utility.Name;
                listItem.Add(resultItem);
            }

            var returnList = listItem.Where(e => (input.Status == null || e.Status == input.Status) && (input.Keyword == null || e.Name.Contains(input.Keyword)));
            result.TotalItems = listItem.Count();

            if (input.PageSize != -1)
            {
                returnList = returnList.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = returnList.ToList();

            return result;
        }

        /// <summary>
        /// Thay đổi trạng thái tiện ích căn hộ
        /// </summary>
        /// <param name="id"></param>
        public void ChangeStatusProductItemUtility(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatusProductItemUtility)} : id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var mediaUpdate = _rstProductItemProjectUtilityEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProductItemUtilityNotFound);

            if (mediaUpdate.Status == Status.ACTIVE)
            {
                mediaUpdate.Status = Status.INACTIVE;
            }
            else
            {
                mediaUpdate.Status = Status.ACTIVE;
            }
            mediaUpdate.ModifiedBy = username;
            mediaUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }
        #endregion

        #region Chính sách ưu đãi từ CĐT
        public void AddProductItemProjectPolicy(CreateRstProductItemProjectPolicyDto input)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(AddProductItemProjectPolicy)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");
            var transaction = _dbContext.Database.BeginTransaction();
            var productItemFind = _rstProductItemEFRepository.FindById(input.ProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            //Lấy ra danh sách chinhs của dự án cài đặt
            var listItemPolicy = _rstProjectPolicyEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProjectId == productItemFind.ProjectId && e.Deleted == YesNo.NO).Select(e => e.Id).ToList();

            //Lấy ra danh sách tiện ích trong Db
            var getAllPolicy = _rstProductItemProjectPolicyEFRepository.Entity.Where(e => e.PartnerId == partnerId && e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO).Select(e => e.ProjectPolicyId).ToList();

            //Xóa những tiện ích có trong db nhưng truyền vào không có
            var deleteProductItemPolicys = getAllPolicy.Except(input.ProductItemProjectPolicies);
            foreach (var deleteItem in deleteProductItemPolicys)
            {
                var deleteProductItemPolicy = _rstProductItemProjectPolicyEFRepository.Entity.FirstOrDefault(e => e.PartnerId == partnerId && e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO && e.ProjectPolicyId == deleteItem);
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItemFind.Id, null, null, null,
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.XOA, "Xóa chính sách ưu đãi chủ đầu tư", DateTime.Now), username);
                deleteProductItemPolicy.Deleted = YesNo.YES;
                deleteProductItemPolicy.ModifiedDate = DateTime.Now;
                deleteProductItemPolicy.ModifiedBy = username;
            }

            //Thêm nhiều tiện ích vào căn hộ
            var insertProductItempolicys = input.ProductItemProjectPolicies.Except(getAllPolicy);
            foreach (var insertItem in insertProductItempolicys)
            {
                var insertProductItemPolicy = new RstProductItemProjectPolicy()
                {
                    ProductItemId = input.ProductItemId,
                    ProjectPolicyId = insertItem
                };
                var projectPolicyFind = _rstProjectPolicyEFRepository.EntityNoTracking.FirstOrDefault(o => o.Id == insertItem);
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItemFind.Id, null, projectPolicyFind?.Code, "CS ưu đãi của CĐT",
                    RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.THEM_MOI, "Thêm mới chính sách ưu đãi chủ đầu tư", DateTime.Now), username);
                _rstProductItemProjectPolicyEFRepository.Add(insertProductItemPolicy, username, partnerId);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public PagingResult<RstProductItemProjectPolicyDto> FindAllProjectPolicy(FilterProductItemProjectPolicyDto input)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllProjectPolicy)}: input = {JsonSerializer.Serialize(input)},  userType = {usertype}, partnerId = {partnerId}");

            var result = new PagingResult<RstProductItemProjectPolicyDto>();
            var resultItems = new List<RstProductItemProjectPolicyDto>();

            var productItemFind = _rstProductItemEFRepository.FindById(input.ProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);

            //Lấy ra danh sách tiện ích của dự án cài đặt
            var projectPolicies = _rstProjectPolicyEFRepository.Entity;

            //Data Utility
            var productItemProjectpolicyQuery = _rstProductItemProjectPolicyEFRepository.Entity.Where(e => e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO);

            var query = from policy in projectPolicies
                        join productItemPolicy in productItemProjectpolicyQuery on policy.Id equals productItemPolicy.ProjectPolicyId into listProductIemtPolicy
                        from itemPolicy in listProductIemtPolicy.DefaultIfEmpty()
                        where policy.PartnerId == partnerId && policy.ProjectId == productItemFind.ProjectId && policy.Deleted == YesNo.NO
                        && (input.Selected == null || itemPolicy != null)
                        && (input.PolicyType == null || policy.PolicyType == input.PolicyType)
                        && (input.Keyword == null || policy.Code.Contains(input.Keyword) || policy.Name.Contains(input.Keyword))
                        && (input.Status == null || itemPolicy.Status == input.Status)
                        select new
                        {
                            policy,
                            itemPolicy
                        };

            result.TotalItems = query.Count();
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            foreach (var item in query)
            {
                //Lấy ra tên và nhóm tiện ích
                resultItems.Add(new RstProductItemProjectPolicyDto()
                {
                    Id = item.itemPolicy?.Id,
                    ProjectPolicyId = item.policy.Id,
                    Code = item.policy.Code,
                    Name = item.policy.Name,
                    PolicyType = item.policy.PolicyType,
                    isProductItemSelected = item.itemPolicy == null ? YesNo.NO : YesNo.YES,
                    Status = item.itemPolicy?.Status,
                    ProjectPolicyStatus = item.policy?.Status
                });
            }

            result.Items = resultItems;
            return result;
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách ưu đãi
        /// </summary>
        /// <param name="id"></param>
        public void ChangeStatusProductItemPolicy(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatusProductItemPolicy)} : id = {id}, username = {username}, partnerId = {partnerId}");


            var productItemPolicyUpdate = _rstProductItemProjectPolicyEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProductItemPolicyNotFound);

            var oldValue = RstProductItemPolicyStatus.ProductItemPolicyStatus(productItemPolicyUpdate.Status);
            var newValue = RstProductItemPolicyStatus.ProductItemPolicyStatus(productItemPolicyUpdate.Status == Status.INACTIVE ? Status.ACTIVE : Status.INACTIVE);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItemPolicyUpdate.ProductItemId, oldValue, newValue, RstFieldName.UPDATE_PRODUCT_ITEM_STATUS, RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, "Đổi trạng thái chính sách", DateTime.Now), username);

            if (productItemPolicyUpdate.Status == Status.ACTIVE)
            {
                productItemPolicyUpdate.Status = Status.INACTIVE;
            }
            else
            {
                productItemPolicyUpdate.Status = Status.ACTIVE;
            }
            productItemPolicyUpdate.ModifiedBy = username;
            productItemPolicyUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }
        #endregion

        #region App
        /// <summary>
        /// Lấy danh sách sản phẩm mở bán của đại lý theo dự án
        /// </summary>
        public List<AppGetAllProductItemDto> AppGetAllProjectItem(AppFilterProductItemDto input)
        {
            _logger.LogInformation($"{nameof(AppGetAllProjectItem)}: input = {JsonSerializer.Serialize(input)}");
            var productItemQuery = _rstProductItemEFRepository.AppGetAllProductItem(input);
            List<AppGetAllProductItemDto> result = new();
            foreach (var item in productItemQuery)
            {
                // Nếu đang được giữ chỗ thì tìm kiếm đến hợp đồng đang giữ chỗ
                if (item.Status == RstProductItemStatus.GIU_CHO)
                {
                    var orderHoldProductItem = (from order in _dbContext.RstOrders.AsNoTracking()
                                                join openSellDetail in _dbContext.RstOpenSellDetails.AsNoTracking() on order.OpenSellDetailId equals openSellDetail.Id
                                                join productItem in _dbContext.RstProductItems.AsNoTracking() on order.ProductItemId equals productItem.Id
                                                where order.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                                && item.Id == order.OpenSellDetailId && item.ProductItemId == order.ProductItemId 
                                                && productItem.Status == RstProductItemStatus.GIU_CHO && openSellDetail.Status == RstProductItemStatus.GIU_CHO
                                                && order.ExpTimeDeposit != null && order.ExpTimeDeposit > DateTime.Now
                                                select order).OrderByDescending(r => r.Id).FirstOrDefault();
                    // Thời gian hết giữ chỗ căn hộ
                    item.ExpTimeHold = orderHoldProductItem?.ExpTimeDeposit;

                    // Nếu đã giữ chỗ thì kiểm tra xem căn hộ có phát sinh giao dịch đang trong thời gian giữ chỗ hay chưa 
                    var checkPayment = _dbContext.RstOrders.Any(o => item.ProductItemId == o.ProductItemId && o.Deleted == YesNo.NO && (o.ExpTimeDeposit == null || o.ExpTimeDeposit > DateTime.Now)
                                        && _dbContext.RstOrderPayments.Any(p => o.Id == p.OrderId && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN));
                    item.HavePaymentOrder = checkPayment ? YesNo.YES : null;

                    // Nếu lọc trạng thái giữ chỗ mà đang có phát sinh giao dịch thì bỏ qua
                    // Hiển thị phát sinh khi lọc trạng thái Khóa căn
                    if (input.Status != null && input.Status.Contains(RstProductItemStatus.GIU_CHO.ToString()) && checkPayment)
                    {
                        continue;
                    }
                }
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Các tham số chuẩn bị cho lọc sản phẩm dự án
        /// </summary>
        public AppGetParamsFindProductItemDto AppGetParamsFindProductItem(int openSellId)
        {
            _logger.LogInformation($"{nameof(AppGetParamsFindProductItem)}: projectId = {openSellId}");
            AppGetParamsFindProductItemDto result = new();
            var projectItem = _rstProductItemEFRepository.AppGetAllProductItem(new AppFilterProductItemDto
            {
                OpenSellId = openSellId,
            });
            var openSellQuery = _rstOpenSellEFRepository.FindById(openSellId);
            result.MinSellingPrice = projectItem.Min(r => r.Price);
            result.MaxSellingPrice = projectItem.Max(r => r.Price);
            result.BuildingDensity = new();
            var buildingDensity = _rstProjectStructureEFRepository.FindAllChildByProjectId(openSellQuery?.ProjectId ?? 0);
            foreach (var item in buildingDensity)
            {
                if (projectItem.Select(r => r.BuildingDensityId).Contains(item.Id))
                {
                    var resultItem = _mapper.Map<RstProjectStructureChildDto>(item);
                    resultItem.NoFloors = projectItem.Where(r => r.BuildingDensityId == item.Id).Select(r => r.NoFloor).Distinct().ToList();
                    result.BuildingDensity.Add(resultItem);
                }
            }
            return result;
        }

        /// <summary>
        /// Thông tin chi tiết của sản phẩm mở bán
        /// </summary>
        public AppRstProductItemDetailDto AppProductItemDetail(int openSellDetailId)
        {
            _logger.LogInformation($"{nameof(AppProductItemDetail)}: openSellId = {openSellDetailId}");
            var openSellDetail = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(openSellDetailId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var productItem = _rstProductItemEFRepository.AppProductItemDetail(openSellDetailId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);

            //tăng lượng view của product item
            ++productItem.ViewCount;
            _dbContext.SaveChanges();

            var openSell = _rstOpenSellEFRepository.FindById(openSellDetail.OpenSellId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            var rating = from rate in _dbContext.RstRatings
                         join order in _dbContext.RstOrders on rate.OrderId equals order.Id
                         where order.OpenSellDetailId == openSellDetailId && order.Deleted == YesNo.NO
                         select rate;

            var totalRating = rating.Count();
            var avarageRating = rating.Any() ? Decimal.Round(rating.Select(o => (decimal)o.Rate).Average(), 1) : 0;

            AppRstProductItemDetailDto result = _mapper.Map<AppRstProductItemDetailDto>(productItem);
            result.TradingProviderId = openSell.TradingProviderId;
            result.Id = openSellDetailId;
            result.ProductItemId = productItem.Id;
            result.OpenSellId = openSellDetail.OpenSellId;
            result.ProjectCode = openSellDetail.ProjectCode;
            result.ProjectName = openSellDetail.ProjectName;
            result.IsShowPrice = openSellDetail.IsShowPrice;
            result.ContactPhone = openSellDetail.ContactPhone;
            result.ContactType = openSellDetail.ContactType;
            result.KeepTime = openSellDetail.KeepTime;
            // Tính giá cọc, giá khóa căn
            var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItem.Price ?? 0, openSellDetail.DistributionId);
            result.DepositPrice = productItemPrice.DepositPrice;
            result.LockPrice = productItemPrice.LockPrice;
            var projectStructure = _rstProjectStructureEFRepository.FindById(productItem.BuildingDensityId ?? 0);
            // Ảnh đại diện của căn hộ
            result.UrlImage = _dbContext.RstProductItemMedias.FirstOrDefault(m => m.ProductItemId == productItem.Id && m.Deleted == YesNo.NO
                                                                    && m.Location == RstProductItemMediaLocations.ANH_DAI_DIEN_CAN_HO)?.UrlImage;
            // Đánh giá
            result.TotalParticipants = 1000 + _rstOrderEFRepository.EntityNoTracking
                .Where(o => o.Status == RstOrderStatus.DA_COC)
                .Select(o => o.CifCode)
                .Distinct()
                .Count();
            result.RatingRate = avarageRating;
            result.TotalReviewers = totalRating;

            //Lấy ra danh sách tiện ích của sản phẩm
            result.Utilities = (from utilityData in RstProductItemUtilityData.UtilityData
                                join productItemUtility in _rstProductItemProjectUtilityEFRepository.EntityNoTracking on utilityData.Id equals productItemUtility.ProductItemUtilityId
                                where productItemUtility.ProductItemId == productItem.Id && productItemUtility.Status == Status.ACTIVE && productItemUtility.Deleted == YesNo.NO
                                select new AppRstProductItemUtilityDto
                                {
                                    Name = utilityData.Name,
                                    Icon = utilityData.Icon,
                                }).ToList();
            // Lấy mẫu hợp đồng đặt cọc căn hộ mở bán do đại lý cài
            result.DepositContracts = (from openSellContract in _dbContext.RstOpenSellContractTemplates
                                       join contractTemplateTemp in _dbContext.RstContractTemplateTemps on openSellContract.ContractTemplateTempId equals contractTemplateTemp.Id
                                       where openSellContract.Deleted == YesNo.NO && contractTemplateTemp.Deleted == YesNo.NO
                                       && openSellContract.Status == Status.ACTIVE && contractTemplateTemp.Status == Status.ACTIVE
                                       && contractTemplateTemp.ContractType == RstContractTypes.HD_DAT_COC && contractTemplateTemp.ContractSource == ContractSources.ONLINE
                                       select new AppRstOpenSellContractTemplateDto()
                                       {
                                           Id = openSellContract.Id,
                                           ContractTemplateTempName = contractTemplateTemp.Name,
                                           EffectiveDate = openSellContract.EffectiveDate,
                                       }).ToList();
            // Lấy chính sách ưu đãi
            result.Policys = _rstSellingPolicyEFRepository.AppRstPolicyForProductItem(openSellDetail.OpenSellId, productItem.Id);
            // Lấy thông tin khác của sản phẩm dự án
            result.ProductItemExtends = _mapper.Map<List<AppRstProductItemExtendDto>>(_rstProductItemExtendRepository.GetAll(productItem.Id));
            result.BuildingDensityName = projectStructure?.Name;
            return result;
        }

        /// <summary>
        /// Thông tin hình ảnh của sản phẩm mở bán
        /// </summary>
        public List<AppRstProductItemMediaDto> AppProductItemDetailMedia(int openSellDetailId)
        {
            _logger.LogInformation($"{nameof(AppProductItemDetailMedia)}: openSellDetailId = {openSellDetailId}");
            var productItem = _rstProductItemEFRepository.AppProductItemDetail(openSellDetailId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            var productItemMedia = _rstProductItemMediaEFRepository.Entity.Where(r => r.ProductItemId == productItem.Id && r.Status == Status.ACTIVE && r.Deleted == YesNo.NO);
            var result = _mapper.Map<List<AppRstProductItemMediaDto>>(productItemMedia);
            return result;
        }

        /// <summary>
        /// Tìm các sản phẩm mở bán tương tự
        /// </summary>
        public AppRstProductItemSimilarDto OpenSellDetailSimilar(int openSellDetailId)
        {
            AppRstProductItemSimilarDto result = new();
            _logger.LogInformation($"{nameof(OpenSellDetailSimilar)}: openSellDetailId = {openSellDetailId}");

            // Tìm ra được căn hộ hiện tại
            var productItemCurrent = _rstProductItemEFRepository.AppProductItemDetail(openSellDetailId);

            // Lấy ra các căn hộ tương tự trong cùng dự án
            var openSellDetailSimilar = from openSellDetail in _dbContext.RstOpenSellDetails
                                        join openSell in _dbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                        join distributionItem in _dbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionItem.Id
                                        join productItem in _dbContext.RstProductItems on distributionItem.ProductItemId equals productItem.Id
                                        join project in _dbContext.RstProjects on productItem.ProjectId equals project.Id
                                        join distribution in _dbContext.RstDistributions on distributionItem.DistributionId equals distribution.Id
                                        where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                                        && productItem.Deleted == YesNo.NO && productItem.Status == RstProductItemStatus.KHOI_TAO
                                        && openSell.Status == RstDistributionStatus.DANG_BAN && distribution.Status == RstDistributionStatus.DANG_BAN
                                        && openSell.ProjectId == productItemCurrent.ProjectId && distribution.ProjectId == productItemCurrent.ProjectId
                                        && productItem.Price == productItemCurrent.Price && distributionItem.Status == Status.ACTIVE
                                        //&& (Math.Abs(productItem.PriceArea ?? 0 - productItemCurrent.PriceArea ?? 0) <= 20)
                                        && openSellDetail.IsLock == YesNo.NO && productItem.IsLock == YesNo.NO
                                        && openSell.StartDate.Date <= DateTime.Now.Date && distribution.StartDate.Date <= DateTime.Now.Date
                                        select new RstCartDetailDto
                                        {
                                            TradingProviderId = openSell.TradingProviderId,
                                            OpenSellDetailId = openSellDetail.Id,
                                            OpenSellId = openSell.Id,
                                            DistributionId = distributionItem.DistributionId,
                                            ProductItemId = productItem.Id,
                                            Code = productItem.Code,
                                            ProjectCode = project.Code,
                                            ProjectName = productItem.Name,
                                            Name = productItem.Name,
                                            DoorDirection = productItem.DoorDirection,
                                            RoomType = productItem.RoomType,
                                            PriceArea = productItem.PriceArea,
                                            Price = productItem.Price ?? 0,
                                            ProductItemStatus = productItem.Status,
                                            KeepTime = openSell.KeepTime
                                        };
            openSellDetailSimilar = openSellDetailSimilar.OrderBy(r => r.Price).ThenBy(r => r.PriceArea);

            result.ProductItemCode = productItemCurrent.Code;
            result.ProjectCode = openSellDetailSimilar.Select(r => r.ProjectCode).FirstOrDefault();
            result.TradingDate = DateTime.Now; // Tạm thời để thời gian hiện tại
            result.ProductItemSimilars = new();
            foreach (var item in openSellDetailSimilar)
            {
                var resultItem = _mapper.Map<AppRstProductItemSimilarDetailDto>(item);
                // Chính sách ưu đãi
                resultItem.Policys = _rstSellingPolicyEFRepository.AppRstPolicyForProductItem(item.OpenSellId, item.ProductItemId);

                // Ảnh đại diện của căn hộ
                resultItem.UrlImage = _dbContext.RstProductItemMedias.FirstOrDefault(m => m.ProductItemId == item.ProductItemId && m.Deleted == YesNo.NO
                                                                        && m.Location == RstProductItemMediaLocations.ANH_DAI_DIEN_CAN_HO)?.UrlImage;

                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(item.Price, item.DistributionId);
                // Gía trị lock căn
                resultItem.LockPrice = productItemPrice.LockPrice;
                resultItem.DepositPrice = productItemPrice.DepositPrice;
                result.ProductItemSimilars.Add(resultItem);
            }
            return result;
        }
        #endregion
    }
}
