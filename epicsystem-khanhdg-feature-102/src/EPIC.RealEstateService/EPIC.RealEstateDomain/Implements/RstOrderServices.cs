using AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Sale;
using EPIC.FileDomain.Services;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.RealEstateEntities.Dto.RstOpenSellDetail;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.RealEstateEntities.Dto.RstOrderContractFile;
using EPIC.RealEstateEntities.Dto.RstOrderCoOwner;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.ConstantVariables.Shared.Bank;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using EPIC.CoreRepositoryExtensions;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOrderServices : IRstOrderServices
    {
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderPaymentEFRepository _rstOrderPaymentEFRepository;
        private readonly RstDistributionPolicyEFRepository _rstDistributionPolicyEFRepository;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstOrderServices> _logger;
        private readonly IFileServices _fileServices;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly MsbCollectMoneyServices _msbCollectMoneyServices;
        private readonly IRstSignalRBroadcastServices _rstSignalRBroadcastServices;
        private readonly RstBackgroundJobServices _rstBackgroundJobServices;
        private readonly RealEstateNotificationServices _rstNotificationServices;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvestorSaleEFRepository _investorSaleEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstProjectPolicyEFRepository _rstProjectPolicyEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstSellingPolicyEFRepository _rstSellingPolicyEFRepository;
        private readonly RstCartEFRepository _rstCartEFRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly RstOrderCoOwnerRepository _rstOrderCoOwnerRepository;
        private readonly RstDistributionProductItemEFRepository _rstDistributionProductItemEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;
        private readonly RstProductItemUtilityEFRepository _rstProductItemProjectUtilityEFRepository;
        private readonly RstProjectStructureEFRepository _rstProjectStructureEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly RstProductItemExtendRepository _rstProductItemExtendRepository;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;
        private readonly RstOpenSellBankEFRepository _rstOpenSellBankEFRepository;
        private readonly RstOpenSellContractTemplateEFRepository _rstOpenSellContractTemplateEFRepository;

        //Services
        private readonly RstOrderContractFileServices _rstOrderContractFileServices;

        public RstOrderServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstOrderServices> logger,
            IFileServices fileServices,
            IBackgroundJobClient backgroundJobs,
            IHttpContextAccessor httpContextAccessor,
            RstBackgroundJobServices rstBackgroundJobServices,
            IRstSignalRBroadcastServices rstSignalRBroadcastServices,
            MsbCollectMoneyServices msbCollectMoneyServices,
            RstOrderContractFileServices rstOrderContractFileServices, 
            RealEstateNotificationServices rstNotificationServices
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _fileServices = fileServices;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _backgroundJobs = backgroundJobs;
            _msbCollectMoneyServices = msbCollectMoneyServices;
            _rstSignalRBroadcastServices = rstSignalRBroadcastServices;
            _rstBackgroundJobServices = rstBackgroundJobServices;
            _rstNotificationServices = rstNotificationServices;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstOrderPaymentEFRepository = new RstOrderPaymentEFRepository(dbContext, logger);
            _rstDistributionPolicyEFRepository = new RstDistributionPolicyEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _investorSaleEFRepository = new InvestorSaleEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProjectPolicyEFRepository = new RstProjectPolicyEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstSellingPolicyEFRepository = new RstSellingPolicyEFRepository(dbContext, logger);
            _rstCartEFRepository = new RstCartEFRepository(dbContext, logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _rstOrderCoOwnerRepository = new RstOrderCoOwnerRepository(dbContext, logger);
            _rstDistributionProductItemEFRepository = new RstDistributionProductItemEFRepository(dbContext, logger);
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);
            _rstProductItemProjectUtilityEFRepository = new RstProductItemUtilityEFRepository(dbContext, logger);
            _rstProjectStructureEFRepository = new RstProjectStructureEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _rstProductItemExtendRepository = new RstProductItemExtendRepository(dbContext, logger);
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, logger);
            _rstOpenSellBankEFRepository = new RstOpenSellBankEFRepository(dbContext, logger);
            _rstOpenSellContractTemplateEFRepository = new RstOpenSellContractTemplateEFRepository(dbContext, logger);
            _rstOrderContractFileServices = rstOrderContractFileServices;
        }

        #region CMS
        private List<RstOrderMoreInfoDto> GetDataOrder(IEnumerable<RstOrder> orders)
        {
            var result = new List<RstOrderMoreInfoDto>();
            foreach (var item in orders)
            {
                var resultItem = new RstOrderMoreInfoDto();
                resultItem = _mapper.Map<RstOrderMoreInfoDto>(item);

                //Lấy thông tin khách hàng
                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(item.CifCode);
                if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomerInfo = _businessCustomerEFRepository.FindById(cifCodeFind.BusinessCustomerId ?? 0);
                    resultItem.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerInfo);
                    var listBank = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(cifCodeFind.BusinessCustomerId ?? 0);
                    resultItem.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank);
                }
                else if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    var investorInfo = _investorEFRepository.FindById(cifCodeFind.InvestorId ?? 0);
                    // Thông tin giấy tờ tùy thân
                    resultItem.Investor = _mapper.Map<Entities.Dto.Investor.InvestorDto>(investorInfo);
                    var findIdentification = _investorEFRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0);
                    if (findIdentification != null)
                    {
                        resultItem.Investor.InvestorIdentification = _mapper.Map<Entities.Dto.Investor.InvestorIdentificationDto>(findIdentification);
                    }
                }

                // Nếu là sản phẩm tích lũy Cổ Phần
                var productItem = _rstProductItemEFRepository.FindById(item.ProductItemId);
                resultItem.ProductItem = _mapper.Map<RstProductItemDto>(productItem);

                var project = _rstProjectEFRepository.FindById(productItem?.ProjectId ?? 0);
                resultItem.Project = _mapper.Map<RstProjectDto>(project);

                var distributionPolicy = _rstDistributionPolicyEFRepository.FindById(item.DistributionPolicyId);
                resultItem.DistributionPolicy = _mapper.Map<RstDistributionPolicyDto>(distributionPolicy);

                var openSellDetalFind = _rstOpenSellDetailEFRepository.FindById(item.OpenSellDetailId);
                if (openSellDetalFind == null)
                {
                    continue;
                }
                var openSellId = openSellDetalFind.OpenSellId;

                var openSell = _rstOpenSellEFRepository.FindById(openSellId);

                resultItem.KeepTime = openSell?.KeepTime;

                if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId == null)
                {
                    resultItem.Orderer = SourceOrderFE.QUAN_TRI_VIEN;
                }
                else if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId != null)
                {
                    resultItem.Orderer = SourceOrderFE.SALE;
                }
                else if (item.Source == SourceOrder.ONLINE)
                {
                    resultItem.Orderer = SourceOrderFE.KHACH_HANG;
                }
                 
                // Trường hợp hợp đồng hết thời gian cọc trạng thái hợp đồng sang hết thời gian
                if ((resultItem.Status == RstOrderStatus.KHOI_TAO || resultItem.Status == RstOrderStatus.CHO_THANH_TOAN_COC) 
                    && resultItem.ExpTimeDeposit != null && resultItem.ExpTimeDeposit < DateTime.Now)
                {
                    resultItem.TimeOutDeposit = true;
                    // Lấy giá cọc, giá lock căn của hợp đồng
                    var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByPolicy(productItem.Price ?? 0, item.DistributionPolicyId);
                    // Tổng số tiền đã được duyệt
                    var sumOrderPaymentApprove = _rstOrderPaymentEFRepository.Entity.Where(p => p.OrderId == item.Id && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN
                                                        && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN).Sum(p => p.PaymentAmount);

                    // Trường hợp hết thời gian nhưng hợp đồng đã khóa căn 
                    if (productItem.Status == RstProductItemStatus.KHOA_CAN && openSellDetalFind.Status == RstProductItemStatus.KHOA_CAN && sumOrderPaymentApprove >= productItemPrice.LockPrice)
                    {
                        resultItem.TimeOutDeposit = false;
                    }    
                }    

                var orderCoOwnerList = new List<RstOrderCoOwnerDto>();
                var coOwners = _rstOrderCoOwnerRepository.Entity.Where(o => o.OrderId == item.Id);
                foreach (var coOwner in coOwners)
                {
                    if (coOwner.InvestorIdenId != null)
                    {
                        var findIdentification = _investorEFRepository.GetIdentificationById(coOwner.InvestorIdenId.Value);
                        if (findIdentification != null)
                        {
                            var orderCoOwner = new RstOrderCoOwnerDto()
                            {
                                OrderId = coOwner.OrderId,
                                Id = coOwner.Id,
                                Fullname = findIdentification.Fullname,
                                Phone = coOwner.Phone,
                                Address = coOwner.Address,
                                IdType = findIdentification.IdType,
                                IdBackImageUrl = findIdentification.IdBackImageUrl,
                                IdFrontImageUrl = findIdentification.IdFrontImageUrl,
                                InvestorIdenId = coOwner.InvestorIdenId,
                                IdNo = findIdentification.IdNo,
                                DateOfBirth = findIdentification.DateOfBirth,
                                PlaceOfOrigin = findIdentification.PlaceOfOrigin,
                            };
                            orderCoOwnerList.Add(orderCoOwner);
                        }
                    }
                    else
                    {
                        var orderCoOwner = _mapper.Map<RstOrderCoOwnerDto>(coOwner);
                        orderCoOwnerList.Add(orderCoOwner);
                    }
                }
                resultItem.RstOrderCoOwner = orderCoOwnerList;
                resultItem.OrderType = (coOwners.Count() > 0) ? RstOrderTypes.DongSoHuu : RstOrderTypes.SoHuu;

                result.Add(resultItem);
            }
            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RstOrderMoreInfoDto> Add(CreateRstOrderDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}, ipAddress = {ipAddress}");

            var inputInsert = _mapper.Map<RstOrder>(input);

            //Tìm sản phẩm mở bán
            var openSellDetail = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(input.OpenSellDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var openSellDetailQuery = _rstOpenSellDetailEFRepository.FindById(input.OpenSellDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var distributionProductItem = _rstDistributionProductItemEFRepository.FindById(openSellDetailQuery.DistributionProductItemId)
                .ThrowIfNull(_dbContext, ErrorCode.RstDistributionProductItemNotFound);
            var productItem = _rstProductItemEFRepository.FindById(distributionProductItem.ProductItemId)
                .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            // Giá đặt cọc của dự án
            var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItem.Price ?? 0, distributionProductItem.DistributionId);
            inputInsert.DepositMoney = productItemPrice.DepositPrice;
            inputInsert.DistributionPolicyId = productItemPrice.DistributionPolicyId;
            inputInsert.ProductItemId = productItem.Id;
            inputInsert.OpenSellDetailId = openSellDetail.Id;
            // Tính đến thời gian hết hạn cọc
            inputInsert.ExpTimeDeposit = _rstOpenSellEFRepository.ExpTimeDeposit(openSellDetail.OpenSellId);
            inputInsert.IpAddressCreated = ipAddress;
            var phone = "";
            //Loại khách hàng
            //Kiểm tra xem là nhà đầu tư cá nhân hay là nhà đầu tư doanh nghiệp
            var findCifCode = _cifCodeEFRepository.FindByCifCode(input.CifCode);
            if (findCifCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            else if (findCifCode.InvestorId != null)
            {
                // Tìm kiếm thông tin sale nếu có mã giới thiệu
                if (!string.IsNullOrWhiteSpace(input.SaleReferralCode))
                {
                    var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
                    if (findSale != null)
                    {
                        inputInsert.SaleReferralCode = findSale.ReferralCode;
                        inputInsert.DepartmentId = findSale.DepartmentId;
                        inputInsert.SaleReferralCodeSub = findSale.ReferralCodeSub;
                        inputInsert.DepartmentIdSub = findSale.DepartmentIdSub;
                    }
                }
                var investorFind = _dbContext.Investors.FirstOrDefault(o => o.InvestorId == findCifCode.InvestorId);
                phone = investorFind.Phone;
            }

            if (findCifCode != null && findCifCode.InvestorId != null)
            {
                inputInsert.InvestorIdenId = _investorEFRepository.GetDefaultIdentification(findCifCode.InvestorId ?? 0)?.Id;
            }
            //Lấy Id mẫu hợp đồng
            RstOrder result = null;
            var transaction = _dbContext.Database.BeginTransaction();
            result = _rstOrderEFRepository.Add(inputInsert, tradingProviderId, username);
            // Nếu mở bán cài thời gian giữ chỗ
            // Không cài thời gian thì hợp đồng nào thanh toán trước thì giữ chỗ trước
            if (openSellDetail.KeepTime != null)
            {
                //Đổ trạng thái căn hộ và trạng thái sản phẩm của mở bán sang giữ chỗ
                productItem.Status = RstProductItemStatus.GIU_CHO;
                openSellDetailQuery.Status = RstProductItemStatus.GIU_CHO;
            }
            _dbContext.SaveChanges();
            // Thêm đồng sở hữu
            foreach (var orderCoOwner in input.RstOrderCoOwners)
            {
                if (orderCoOwner.InvestorIdenId != null)
                {
                    var findIdentification = _investorEFRepository.GetIdentificationById(orderCoOwner.InvestorIdenId.Value);
                    if (findIdentification != null)
                    {
                        _rstOrderCoOwnerRepository.Add(new RstOrderCoOwner
                        {
                            OrderId = result.Id,
                            InvestorIdenId = findIdentification.Id,
                            CreatedBy = username
                        });
                    }
                }
                else
                {
                    _rstOrderCoOwnerRepository.Add(new RstOrderCoOwner
                    {
                        OrderId = result.Id,
                        Fullname = orderCoOwner.Fullname,
                        Phone = orderCoOwner.Phone,
                        Address = orderCoOwner.Address,
                        IdType = orderCoOwner.IdType,
                        IdBackImageUrl = orderCoOwner.IdBackImageUrl,
                        IdFrontImageUrl = orderCoOwner.IdFrontImageUrl,
                        IdNo = orderCoOwner.IdNo,
                        DateOfBirth = orderCoOwner.DateOfBirth,
                        PlaceOfOrigin = orderCoOwner.PlaceOfOrigin,
                        CreatedBy = username
                    });
                }
            }

            // thêm lịch sử chỉnh sửa của sổ lệnh

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(result.Id, "Đang mở bán", "Giữ chỗ", "Trạng thái",
                        RstHistoryUpdateTables.RST_ORDER, ActionTypes.THEM_MOI, $"KH: {phone} - Đặt lệnh", DateTime.Now), username);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItem.Id, "Đang mở bán", "Giữ chỗ", "Trạng thái",
                        RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.THEM_MOI, $"KH: {phone} - Đặt lệnh", DateTime.Now), username);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, "Đang mở bán", "Giữ chỗ", "Trạng thái",
                        RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.THEM_MOI, $"KH: {phone} - Đặt lệnh", DateTime.Now), username);

            _dbContext.SaveChanges();
            var data = _rstOrderContractFileServices.GetReplaceTextContractFile(new RstExportContracDto()
            {
                ProductItemId = result.ProductItemId,
                ProjectId = productItem.ProjectId,
                OpenSellDetailId = result.OpenSellDetailId,
                InvestorId = findCifCode.InvestorId,
                BusinessCustomerId = findCifCode.BusinessCustomerId,
                IdentificationId = result.InvestorIdenId ?? 0,
                TradingProviderId = result.TradingProviderId,
            });
            // Sinh file hợp đồng mẫu
            await _rstOrderContractFileServices.CreateContractFileByOrder(result, data, tradingProviderId);
            transaction.Commit();
            // Nếu cài thời gian giữ chỗ ở mở bán
            if (openSellDetail.KeepTime != null)
            {
                string jobId = _backgroundJobs.Schedule(() => _rstBackgroundJobServices.UpdateDepositExpire(result.Id), TimeSpan.FromSeconds(openSellDetail.KeepTime.Value));
                result.DepositJobId = jobId;
                _dbContext.SaveChanges();
                await _rstSignalRBroadcastServices.BroadcastOpenSellDetailChangeStatus(openSellDetail.Id);
            }
            await _rstSignalRBroadcastServices.BroadcastUpdateOrderNewInProject(result.Id);
            return _mapper.Map<RstOrderMoreInfoDto>(result);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public RstOrderMoreInfoDto FindById(int orderId)
        {
            _logger.LogInformation($"{nameof(FindById)}: orderId= {orderId}");

            var orderFind = _rstOrderEFRepository.FindById(orderId).ThrowIfNull<RstOrder>(_dbContext, ErrorCode.RstOrderNotFound);
            var result = _mapper.Map<RstOrderMoreInfoDto>(orderFind);

            //Lấy thông tin BusinessCusstomer / Investor
            var findInfobyCifCode = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode);

            if (findInfobyCifCode == null)
            {
                _rstOrderEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            else if (findInfobyCifCode.InvestorId != null)
            {
                var investor = _investorEFRepository.FindActiveInvestorById(findInfobyCifCode.InvestorId ?? 0);
                if (investor == null)
                {
                    _rstOrderEFRepository.ThrowException(ErrorCode.InvestorNotFound);
                }
                // Thông tin giấy tờ tùy thân
                result.Investor = _mapper.Map<Entities.Dto.Investor.InvestorDto>(investor);
                result.Investor.ListBank = _investorEFRepository.FindListBank(investor.InvestorId)?.ToList();
                result.Investor.ListContactAddress = _investorEFRepository.GetListContactAddress(investor.InvestorId).ToList();
                var findIdentification = _investorEFRepository.GetIdentificationById(orderFind.InvestorIdenId ?? 0);
                if (findIdentification != null)
                {
                    result.Investor.InvestorIdentification = _mapper.Map<Entities.Dto.Investor.InvestorIdentificationDto>(findIdentification);
                }
                var findListIdentification = _investorEFRepository.GetListIdentification(findInfobyCifCode.InvestorId ?? 0).Where(i => i.Status == Status.ACTIVE);
                if (findIdentification != null)
                {
                    result.Investor.ListInvestorIdentification = _mapper.Map<List<Entities.Dto.Investor.InvestorIdentificationDto>>(findListIdentification);
                }
            }
            else
            {
                var businessCustomer = _businessCustomerEFRepository.FindById(findInfobyCifCode.BusinessCustomerId ?? 0);
                if (businessCustomer == null)
                {
                    _rstOrderEFRepository.ThrowException(ErrorCode.CoreBussinessCustomerNotFound);
                }
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                result.BusinessCustomer.BusinessCustomerBanks = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(findInfobyCifCode.BusinessCustomerId ?? 0).ToList();
            }

            var productItem = _rstProductItemEFRepository.FindById(orderFind.ProductItemId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            result.ProductItem = _mapper.Map<RstProductItemDto>(productItem);

            var prjectStructure = _rstProjectStructureEFRepository.FindById(productItem.BuildingDensityId.Value);
            result.ProductItem.ProjectStructure = _mapper.Map<RstProjectStructureDto>(prjectStructure);

            var distributionPolicy = _rstDistributionPolicyEFRepository.FindById(orderFind.DistributionPolicyId).ThrowIfNull<RstDistributionPolicy>(_dbContext, ErrorCode.RstDistributionPolicyNotFound);
            result.DistributionPolicy = _mapper.Map<RstDistributionPolicyDto>(distributionPolicy);

            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(orderFind.OpenSellDetailId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            result.OpenSellId = openSellDetail.OpenSellId;

            var openSell = _rstOpenSellEFRepository.FindById(openSellDetail.OpenSellId);

            var project = _rstProjectEFRepository.FindById(openSell.ProjectId);
            result.Project = _mapper.Map<RstProjectDto>(project);
            result.KeepTime = openSell.KeepTime;
            var distributionProductItem = _rstDistributionProductItemEFRepository.FindById(openSellDetail.DistributionProductItemId);
            // Giá đặt cọc của dự án
            var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItem.Price ?? 0, distributionProductItem.DistributionId);
            result.ProductItemPrice = productItemPrice;

            result.OrderPaymentBanks = new();
            var openSellBank = _rstOpenSellBankEFRepository.GetAll(openSell.Id);
            foreach (var bankItem in openSellBank)
            {
                BusinessCustomerBankDto bankAccount = new();
                if (bankItem.TradingBankAccountId != null)
                {
                    bankAccount = _businessCustomerEFRepository.FindBankById(bankItem.TradingBankAccountId ?? 0)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, bankItem.TradingBankAccountId);
                }
                else
                {
                    bankAccount = _businessCustomerEFRepository.FindBankById(bankItem.PartnerBankAccountId ?? 0)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, bankItem.PartnerBankAccountId);
                }
                result.OrderPaymentBanks.Add(new RstOpenSellBankDto
                {
                    Id = bankItem.Id,
                    OpenSellId = bankItem.OpenSellId,
                    TradingBankAccountId = bankItem.TradingBankAccountId,
                    PartnerBankAccountId = bankItem.PartnerBankAccountId,
                    BankAccount = bankAccount
                });
            }

            // Tìm thanh toán lần đầu tiên 
            var orderPaymentFirstQuery = _rstOrderPaymentEFRepository.Entity.FirstOrDefault(o => o.OrderId == orderId && o.TranType == TranTypes.THU && o.TranClassify == TranClassifies.THANH_TOAN && o.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                        && o.Status != OrderPaymentStatus.HUY_THANH_TOAN && o.Deleted == YesNo.NO);
            if (orderPaymentFirstQuery != null)
            {
                result.OrderPaymentFirstBank = new();
                var tradingBankAccount = _businessCustomerEFRepository.FindBankById(orderPaymentFirstQuery.TradingBankAccountId ?? 0);
                if (tradingBankAccount != null)
                {
                    result.OrderPaymentFirstBank.TradingBankAccountId = orderPaymentFirstQuery.TradingBankAccountId;
                    result.OrderPaymentFirstBank.BankAccount = tradingBankAccount;
                }
                var partnerBankAccount = _businessCustomerEFRepository.FindBankById(orderPaymentFirstQuery.PartnerBankAccountId ?? 0);
                if (partnerBankAccount != null)
                {
                    result.OrderPaymentFirstBank.PartnerBankAccountId = orderPaymentFirstQuery.PartnerBankAccountId;
                    result.OrderPaymentFirstBank.BankAccount = partnerBankAccount;
                }
            }

            var orderCoOwnerList = new List<RstOrderCoOwnerDto>();
            var coOwner = _rstOrderCoOwnerRepository.Entity.Where(o => o.OrderId == orderFind.Id && o.Deleted == YesNo.NO);
            foreach (var item in coOwner)
            {
                if (item.InvestorIdenId != null)
                {
                    var findIdentification = _investorEFRepository.GetIdentificationById(item.InvestorIdenId.Value);
                    if (findIdentification != null)
                    {
                        var orderCoOwner = new RstOrderCoOwnerDto()
                        {
                            OrderId = item.OrderId,
                            Id = item.Id,
                            Fullname = findIdentification.Fullname,
                            Phone = item.Phone,
                            Address = item.Address,
                            IdType = findIdentification.IdType,
                            IdBackImageUrl = findIdentification.IdBackImageUrl,
                            IdFrontImageUrl = findIdentification.IdFrontImageUrl,
                            InvestorIdenId = item.InvestorIdenId,
                            IdNo = findIdentification.IdNo,
                            DateOfBirth = findIdentification.DateOfBirth,
                            PlaceOfOrigin = findIdentification.PlaceOfOrigin,
                        };
                        orderCoOwnerList.Add(orderCoOwner);
                    }
                }
                else
                {
                    var orderCoOwner = _mapper.Map<RstOrderCoOwnerDto>(item);
                    orderCoOwnerList.Add(orderCoOwner);
                }
            }
            result.RstOrderCoOwner = orderCoOwnerList;

            if (orderFind.SaleOrderId != null)
            {
                result.SaleOrder = new();
                var saleQuery = _saleEFRepository.Entity.FirstOrDefault(s => s.SaleId == orderFind.SaleOrderId && s.Deleted == YesNo.NO);
                if (saleQuery != null && saleQuery.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(saleQuery.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorDto>(investorFind);
                        result.SaleOrder.Investor = investor;
                        var investorIdenDefaultFind = _investorEFRepository.GetDefaultIdentification(investorFind.InvestorId);

                        if (investorIdenDefaultFind != null)
                        {
                            result.SaleOrder.Investor.InvestorIdentification = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                    }
                }
                else if (saleQuery != null && saleQuery.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerEFRepository.FindById(saleQuery.BusinessCustomerId ?? 0);
                    result.SaleOrder.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                }
            }

            var saleFind = _saleEFRepository.FindSaleByReferralCode(orderFind.SaleReferralCode);
            if (saleFind != null)
            {
                result.Sale = _mapper.Map<ViewSaleDto>(saleFind);
                if (saleFind.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(saleFind.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorDto>(investorFind);
                        result.Sale.Investor = investor;
                        var investorIdenDefaultFind = _investorEFRepository.GetDefaultIdentification(saleFind.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            result.Sale.Investor.InvestorIdentification = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                    }
                }
                else if (saleFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerEFRepository.FindById(saleFind.BusinessCustomerId ?? 0);
                    result.Sale.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                }

                var departmentFind = _saleEFRepository.FindDepartmentById(orderFind.DepartmentId ?? 0, orderFind.TradingProviderId);
                if (departmentFind != null)
                {
                    result.DepartmentName = departmentFind.DepartmentName;
                }
                else
                {
                    _logger.LogError($"{nameof(FindById)}: Không tìm thấy phòng giao dịch quản lý hợp đồng: departmentId = {orderFind.DepartmentId}, tradingProviderId = {orderFind.TradingProviderId}");
                }

                var departmentOfSaleFind = _saleEFRepository.FindSaleTradingProvider(saleFind.SaleId, orderFind.TradingProviderId);
                if (departmentOfSaleFind != null)
                {
                    result.ManagerDepartmentName = departmentOfSaleFind.DepartmentName;
                }
                else
                {
                    _logger.LogError($"{nameof(FindById)}: Không tìm thấy phòng giao dịch quản lý hợp đồng: saleId = {saleFind.SaleId}, tradingProviderId = {orderFind.TradingProviderId}");
                }
            }

            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="status"></param>
        /// <param name="isGroupByCustomer"></param>
        /// <returns></returns>
        public PagingResult<RstOrderMoreInfoDto> FindAll(FilterRstOrderDto input, int[] status, bool isGroupByCustomer = false)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                _dbContext.CheckTradingRelationshipPartner(partnerId, input.TradingProviderIds);
            }
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, status = {status}, usertype = {usertype}, tradingProviderId = {tradingProviderId}");

            var resultPaging = new PagingResult<RstOrderMoreInfoDto>();

            var find = _rstOrderEFRepository.FindAll(input, status, tradingProviderId, isGroupByCustomer);

            var result = GetDataOrder(find.Items);

            resultPaging.Items = result;
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOrder Update(UpdateRstOrderDto input)
        {
            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            var transaction = _dbContext.Database.BeginTransaction();
            var orderFind = _rstOrderEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull<RstOrder>(_dbContext, ErrorCode.RstOrderNotFound);

            // lịch sử chỉnh sửa hợp đồng
            if (input.ContractAddressId != orderFind.ContractAddressId)
            {
                _rstHistoryUpdateEFRepository.HistoryContactAddress(orderFind.Id, input.ContractAddressId, username);
            }

            if (input.InvestorIdenId != orderFind.InvestorIdenId)
            {
                _rstHistoryUpdateEFRepository.HistoryinvestorIdentification(orderFind.Id, input.InvestorIdenId, username);
            }

            if (input.SaleReferralCode != orderFind.SaleReferralCode)
            {
                _rstHistoryUpdateEFRepository.HistoryReferralCode(orderFind.Id, input.SaleReferralCode, username);
                var saleQuery = _saleEFRepository.FindSaleTradingProviderByReferralCode(input.SaleReferralCode, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotFound);
                if (saleQuery.Status == Status.INACTIVE)
                {
                    _rstOrderEFRepository.ThrowException(ErrorCode.CoreSaleStatusIllegal);
                }
                orderFind.SaleReferralCode = input.SaleReferralCode;
                orderFind.DepartmentId = saleQuery.DepartmentId;
            }

            if (input.OpenSellDetailId != orderFind.OpenSellDetailId)
            {
                _rstHistoryUpdateEFRepository.HistoryOpenSellDetail(orderFind.Id, input.OpenSellDetailId, username);
            }
            if (input.PaymentType != orderFind.PaymentType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderFind.Id, RstOrderPaymentypes.PaymentTypeName(orderFind.PaymentType), RstOrderPaymentypes.PaymentTypeName(input.PaymentType), RstFieldName.UPDATE_ORDER_PAYMEN_TYPE,
                    RstHistoryUpdateTables.RST_ORDER, ActionTypes.CAP_NHAT, "cập nhật hình thức thanh toán", DateTime.Now), username);
            }

            //Lấy thông tin order
            orderFind.InvestorIdenId = input.InvestorIdenId;
            orderFind.ContractAddressId = input.ContractAddressId;
            orderFind.SaleReferralCode = input.SaleReferralCode;
            orderFind.PaymentType = input.PaymentType;
            //Tìm sản phẩm mở bán
            var openSellDetail = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(input.OpenSellDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var openSellDetailQuery = _rstOpenSellDetailEFRepository.FindById(input.OpenSellDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var distributionProductItem = _rstDistributionProductItemEFRepository.FindById(openSellDetailQuery.DistributionProductItemId)
                .ThrowIfNull<RstDistributionProductItem>(_dbContext, ErrorCode.RstDistributionProductItemNotFound);
            var productItem = _rstProductItemEFRepository.FindById(distributionProductItem.ProductItemId)
                .ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            orderFind.ProductItemId = productItem.Id;
            orderFind.OpenSellDetailId = openSellDetail.Id;

            var order = _rstOrderEFRepository.Update(orderFind, tradingProviderId, username);
            _dbContext.SaveChanges();
            UpdateOrderCoOwnerCommon(input.Id, input.RstOrderCoOwners);
            transaction.Commit();
            return order;
        }

        public void UpdateOrderCoOwner(UpdateListRstOrderCoOwnerDto input)
        {
            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateOrderCoOwner)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            var transaction = _dbContext.Database.BeginTransaction();
            var orderFind = _rstOrderEFRepository.FindById(input.OrderId, tradingProviderId).ThrowIfNull<RstOrder>(_dbContext, ErrorCode.RstOrderNotFound);
            UpdateOrderCoOwnerCommon(input.OrderId, input.OrderCoOwners);
            transaction.Commit();
        }

        public void UpdateOrderCoOwnerCommon(int orderId, List<UpdateRstOrderCoOwnerDto> orderCoOwners)
        {
            _logger.LogInformation($"{nameof(RstOrderCoOwnerRepository)}->{nameof(UpdateOrderCoOwner)}: {JsonSerializer.Serialize(orderCoOwners)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            orderCoOwners = orderCoOwners != null ? orderCoOwners : new List<UpdateRstOrderCoOwnerDto>();
            var orderCoOwnerQuery = _rstOrderCoOwnerRepository.Entity.Where(o => o.OrderId == orderId && o.Deleted == YesNo.NO);
            var orderCoOwnerRemove = orderCoOwnerQuery.Where(o => !orderCoOwners.Select(o => o.Id).Contains(o.Id));
            foreach (var item in orderCoOwnerRemove)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderId, null, null, null,
                    RstHistoryUpdateTables.RST_ORDER_CO_OWNER, ActionTypes.XOA, "Xóa người đồng sở hữu", DateTime.Now), username);
                item.Deleted = YesNo.YES;
            }

            foreach (var item in orderCoOwners)
            {
                // Đồng sỡ hữu đã có thì Update
                var orderCoOwner = _rstOrderCoOwnerRepository.Entity.FirstOrDefault(o => o.OrderId == orderId && o.Id == (item.Id ?? 0) && o.Deleted == YesNo.NO);
                if (orderCoOwner != null && orderCoOwner.InvestorIdenId == null)
                {
                    // lịch sử chỉnh sửa người đồng sở hữu
                    if (orderCoOwner.Fullname != item.Fullname)
                    {
                        _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderId, orderCoOwner.Fullname, item.Fullname, RstFieldName.UPDATE_ORDER_CO_OWNER_FULL_NAME,
                        RstHistoryUpdateTables.RST_ORDER_CO_OWNER, ActionTypes.CAP_NHAT, "Tên khách hàng", DateTime.Now), username);
                    }
                    if (orderCoOwner.PlaceOfOrigin != item.PlaceOfOrigin)
                    {
                        _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderId, orderCoOwner.PlaceOfOrigin, item.PlaceOfOrigin, RstFieldName.UPDATE_ORDER_CO_OWNER_PLACE_OF_ORIGIN,
                        RstHistoryUpdateTables.RST_ORDER_CO_OWNER, ActionTypes.CAP_NHAT, "Nguyên quán", DateTime.Now), username);
                    }
                    if (orderCoOwner.IdNo != item.IdNo)
                    {
                        _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderId, orderCoOwner.IdNo, item.IdNo, RstFieldName.UPDATE_ORDER_CO_OWNER_ID_NO,
                        RstHistoryUpdateTables.RST_ORDER_CO_OWNER, ActionTypes.CAP_NHAT, "Số giấy tờ", DateTime.Now), username);
                    }

                    orderCoOwner.Fullname = item.Fullname;
                    orderCoOwner.Address = item.Address;
                    orderCoOwner.PlaceOfOrigin = item.PlaceOfOrigin;
                    orderCoOwner.IdNo = item.IdNo;
                }
                // Đồng sỡ hữu chưa tồn tại thì là thêm mới
                else if (item.Id == null && item.InvestorIdenId != null)
                {
                    var findIdentification = _dbContext.InvestorIdentifications.FirstOrDefault(i => i.Id == item.InvestorIdenId && i.Deleted == YesNo.NO)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);
                    _rstOrderCoOwnerRepository.Add(new RstOrderCoOwner
                    {
                        OrderId = orderId,
                        InvestorIdenId = findIdentification.Id,
                        CreatedBy = username
                    });
                }
                else if (item.Id == null && item.InvestorIdenId == null)
                {
                    _rstOrderCoOwnerRepository.Add(new RstOrderCoOwner
                    {
                        OrderId = orderId,
                        Fullname = orderCoOwner.Fullname,
                        Phone = orderCoOwner.Phone,
                        Address = orderCoOwner.Address,
                        IdType = orderCoOwner.IdType,
                        IdBackImageUrl = orderCoOwner.IdBackImageUrl,
                        IdFrontImageUrl = orderCoOwner.IdFrontImageUrl,
                        IdNo = orderCoOwner.IdNo,
                        DateOfBirth = orderCoOwner.DateOfBirth,
                        PlaceOfOrigin = orderCoOwner.PlaceOfOrigin,
                        CreatedBy = username
                    });
                }
            }
            _dbContext.SaveChanges();
        }


        /// <summary>
        /// Cập nhật hình thức thanh toán
        /// </summary>
        /// <param name="input"></param>
        public void UpdatePaymentType(UpdateRstOrderPaymentTypeDto input)
        {
            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdatePaymentType)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            var orderFind = _rstOrderEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull<RstOrder>(_dbContext, ErrorCode.RstOrderNotFound);

            if (input.PaymentType != orderFind.PaymentType)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderFind.Id, RstOrderPaymentypes.PaymentTypeName(orderFind.PaymentType), RstOrderPaymentypes.PaymentTypeName(input.PaymentType), RstFieldName.UPDATE_ORDER_PAYMEN_TYPE,
                    RstHistoryUpdateTables.RST_ORDER, ActionTypes.CAP_NHAT, "Cập nhật hình thức thanh toán hợp đồng", DateTime.Now), username);
                orderFind.PaymentType = input.PaymentType;
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public RstOrder Deleted(int orderId)
        {
            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Deleted)}: orderId = {orderId}, username = {username}, tradingProviderId = {tradingProviderId}");

            var orderDeleted = _rstOrderEFRepository.Deleted(orderId, tradingProviderId, username);
            _dbContext.SaveChanges();
            return orderDeleted;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        public void DeleteCoOwner(int id, int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteCoOwner)}: id = {id}, orderId = {orderId}, username = {username}");
            var coOwnerFind = _rstOrderCoOwnerRepository.FindById(id, orderId).ThrowIfNull<RstOrderCoOwner>(_dbContext, ErrorCode.RstOrderCoOwnerNotFound);

            var coOwner = (from iden in _dbContext.InvestorIdentifications
                           where iden.Id == id && iden.Deleted == YesNo.NO
                           select iden.Fullname + " - " + iden.IdNo).FirstOrDefault();

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderId, null, null, null,
                    RstHistoryUpdateTables.RST_ORDER_CO_OWNER, ActionTypes.XOA, "Xóa người đồng sở hữu " + coOwner, DateTime.Now), username);
            _dbContext.Remove(coOwnerFind);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thêm đồng sở hữu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOrderCoOwner AddCoOwner(CreateRstOrderCoOwnerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteCoOwner)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            var inputInsert = _mapper.Map<RstOrderCoOwner>(input);
            inputInsert.CreatedBy = username;

            var coOwner = (from iden in _dbContext.InvestorIdentifications
                           where iden.Id == input.InvestorIdenId && iden.Deleted == YesNo.NO
                           select iden.Fullname + " - " + iden.IdNo).FirstOrDefault();
            if(coOwner != null)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(input.OrderId, null, null, null,
                        RstHistoryUpdateTables.RST_ORDER_CO_OWNER, ActionTypes.THEM_MOI,"Thêm mới người đồng sở hữu " + coOwner, DateTime.Now), username);
            }    

            _rstOrderCoOwnerRepository.Add(inputInsert);
            _dbContext.SaveChanges();
            return inputInsert;
        }

        /// <summary>
        /// Thay đổi nguồn đặt lệnh 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public RstOrder ChangeSource(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeSource)}: input = {JsonSerializer.Serialize(orderId)}, username = {orderId}, tradingProviderId = {tradingProviderId}, username = {username}");
            var orderFind = _rstOrderEFRepository.FindById(orderId, tradingProviderId)
                .ThrowIfNull<RstOrder>(_dbContext, ErrorCode.RstOrderNotFound);

            // lịch sử chỉnh sửa
            _rstHistoryUpdateEFRepository.HistoryOrderSource(orderId, username);

            // chuyển trạng thái
            var source = SourceOrder.ONLINE;
            if (orderFind.Source == source)
            {
                orderFind.Source = SourceOrder.OFFLINE;
            }
            else
            {
                orderFind.Source = SourceOrder.ONLINE;
            }
            orderFind.ModifiedDate = DateTime.Now;
            orderFind.ModifiedBy = username;

            _dbContext.SaveChanges();
            return orderFind;
        }

        /// <summary>
        /// Duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<RstOrder> OrderApprove(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(OrderApprove)}: input = {JsonSerializer.Serialize(orderId)}, username = {orderId}, tradingProviderId = {tradingProviderId}, username = {username}");
            var orderFind = _rstOrderEFRepository.FindById(orderId, tradingProviderId)
                .ThrowIfNull<RstOrder>(_dbContext, ErrorCode.RstOrderNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            _rstOrderEFRepository.Approve(orderFind.Id, username);
            var cifCode = _dbContext.CifCodes.FirstOrDefault(o => o.CifCode == orderFind.CifCode);

            string phone = null;
            if (cifCode != null && cifCode.InvestorId != null)
            {
                var investorFind = _dbContext.Investors.FirstOrDefault(o => o.InvestorId == cifCode.InvestorId);
                phone = investorFind?.Phone;
                // Thêm quan hệ với đại lý
                _investorEFRepository.InsertInvestorTradingProvider(cifCode.InvestorId ?? 0, orderFind.TradingProviderId, username);

                // Thêm quan hệ InvestorSale
                var referralCode = (orderFind.SaleReferralCodeSub != null) ? orderFind.SaleReferralCodeSub : orderFind.SaleReferralCode;
                var saleFind = _saleEFRepository.GetSaleByReferralCodeSelf(referralCode);
                if (saleFind != null)
                {
                    // insert bản ghi investorSale
                    _investorSaleEFRepository.InsertInvestorSale(new InvestorSale
                    {
                        InvestorId = cifCode.InvestorId ?? 0,
                        SaleId = saleFind.SaleId,
                        ReferralCode = referralCode
                    });
                }
            }

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate()
            {
                RealTableId = orderId,
                FieldName = RstFieldName.UPDATE_ORDER_APPROVE,
                UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                Action = ActionTypes.CAP_NHAT,
                Summary = RstHistoryUpdateSummary.SUMMARY_APPROVE_FILE,
                CreatedDate = DateTime.Now,
            }, username);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderFind.OpenSellDetailId, "Khoá căn", "Đã đặt cọc", "Trạng thái",
                        RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, $"KH: {phone} {RstHistoryUpdateSummary.DEPOSIT}", DateTime.Now, RstHistoryTypes.DatCoc), username);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderFind.ProductItemId, "Khoá căn", "Đã đặt cọc", "Trạng thái",
                        RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, $"KH: {phone} {RstHistoryUpdateSummary.DEPOSIT}" , DateTime.Now, RstHistoryTypes.DatCoc), username);

            _dbContext.SaveChanges();
            transaction.Commit();
            await _rstNotificationServices.SendNotifyRstOrderActive(orderId);
            if (orderFind.SaleReferralCode != null)
            {
                await _rstNotificationServices.SendNotifySaleOrderActive(orderId);
            }
            await _rstSignalRBroadcastServices.BroadcastOpenSellDetailChangeStatus(orderFind.OpenSellDetailId);
            return orderFind;
        }

        /// <summary>
        /// Huỷ duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public RstOrder OrderCancel(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(OrderCancel)}: input = {JsonSerializer.Serialize(orderId)}, username = {orderId}, tradingProviderId = {tradingProviderId}, username = {username}");
            var orderFind = _rstOrderEFRepository.FindById(orderId, tradingProviderId)
                .ThrowIfNull<RstOrder>(_dbContext, ErrorCode.RstOrderNotFound);
            var cifCode = _dbContext.CifCodes.FirstOrDefault(o => o.CifCode == orderFind.CifCode);
            var investorFind = _dbContext.Investors.FirstOrDefault(o => o.InvestorId == cifCode.InvestorId);
            var phone = investorFind.Phone;
            if (orderFind.Status == RstOrderStatus.DA_COC)
            {
                orderFind.Status = RstOrderStatus.CHO_DUYET_HOP_DONG_COC;
                var projectItem = _rstProductItemEFRepository.FindById(orderFind.ProductItemId)
                    .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
                var openSellDetail = _rstOpenSellDetailEFRepository.FindById(orderFind.OpenSellDetailId)
                    .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
                if (projectItem.Status == RstProductItemStatus.DA_COC && projectItem.Status == RstProductItemStatus.DA_COC)
                {
                    projectItem.Status = RstProductItemStatus.KHOA_CAN;
                    openSellDetail.Status = RstProductItemStatus.KHOA_CAN;
                    _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(projectItem.Id, "Giữ chỗ", "Khoá căn", "Trạng thái", RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.LOCK} cho KH: {phone}", DateTime.Now, RstHistoryTypes.KhoaCan), username);
                    _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, "Giữ chỗ", "Khoá căn", "Trạng thái", RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.LOCK} cho KH: {phone}", DateTime.Now, RstHistoryTypes.KhoaCan), username);
                }    
            }
            orderFind.ModifiedDate = DateTime.Now;
            orderFind.ModifiedBy = username;

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate()
            {
                RealTableId = orderId,
                FieldName = RstFieldName.UPDATE_ORDER_CANCEL,
                UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                Action = ActionTypes.CAP_NHAT,
                Summary = RstHistoryUpdateSummary.SUMMARY_CANCEL_FILE,
                CreatedDate = DateTime.Now,
            }, username);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderFind.OpenSellDetailId, null, null, null,
                        RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, $"KH: {phone} {RstHistoryUpdateSummary.CANCEL}", DateTime.Now, RstHistoryTypes.HuyCoc), username);

            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(orderFind.ProductItemId, null, null, null,
                        RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, $"KH: {phone} {RstHistoryUpdateSummary.CANCEL}", DateTime.Now, RstHistoryTypes.HuyCoc), username);
            _dbContext.SaveChanges();
            return orderFind;
        }

        public void ExtendedKeepTime(RstOrderExtendedKeepTimeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ExtendedKeepTime)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var order = _rstOrderEFRepository.Entity.FirstOrDefault(o => o.Id == input.OrderId && o.Deleted == YesNo.NO && o.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound);

            if (!new int[] { RstOrderStatus.KHOI_TAO, RstOrderStatus.CHO_THANH_TOAN_COC,
                RstOrderStatus.CHO_KY_HOP_DONG, RstOrderStatus.CHO_DUYET_HOP_DONG_COC }.Contains(order.Status))
            {
                _rstOrderEFRepository.ThrowException(ErrorCode.RstOrderCannotExtendedKeepTime);
            }

            //xoá job cũ
            if (order.DepositJobId != null)
            {
                _backgroundJobs.Delete(order.DepositJobId);
            }

            var productItem = _rstProductItemEFRepository.EntityNoTracking
                .Select(p => new
                {
                    p.Id,
                    p.Status,
                    p.Deleted
                })
                .FirstOrDefault(p => p.Id == order.ProductItemId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);

            if (productItem.Status != RstProductItemStatus.GIU_CHO)
            {
                _rstOrderEFRepository.ThrowException(ErrorCode.RstOrderCannotExtendedKeepTimeProductItemNotGiuCho);
            }

            // Lưu lại lý do gia hạn thời gian dữ chỗ
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate
            {
                RealTableId = input.OrderId,
                OldValue = "",
                NewValue = input.KeepTime.ToString(),
                FieldName = RstFieldName.UPDATE_ORDER_EXP_TIME_DEPOSIT,
                UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                Action = ActionTypes.CAP_NHAT,
                ActionUpdateType = RstActionUpdateTypes.UPDATE_ORDER_EXP_TIME_DEPOSIT,
                UpdateReason = input.Reason,
                Summary = input.Summary,
                CreatedDate = DateTime.Now,
            }, username);
            _dbContext.SaveChanges();

            string jobId = _backgroundJobs.Schedule(() => _rstBackgroundJobServices.UpdateDepositExpire(order.Id), TimeSpan.FromSeconds(input.KeepTime));
            order.DepositJobId = jobId;
            order.ExpTimeDeposit = DateTime.Now.AddSeconds(input.KeepTime);
            _dbContext.SaveChanges();
        }
        #endregion

        #region App
        /// <summary>
        /// Kiểm tra trước khi thêm hợp đồng
        /// </summary>
        public void CheckOrder(AppCreateRstOrderCheckDto input)
        {

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            if (input.InvestorId == null)
            {
                input.InvestorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            RstOpenSellDetailInfoDto openSellDetailQuery = null;

            if (input.OpenSellDetailId != null)
            {
                // Tìm kiếm thông tin của sản phẩm mở bán
                openSellDetailQuery = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(input.OpenSellDetailId ?? 0).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            }
            if (input.CartId != null)
            {
                var cartQuery = _rstCartEFRepository.GetByIdAndInvestorId(input.InvestorId ?? 0, input.CartId ?? 0).ThrowIfNull(_dbContext, ErrorCode.RstCartNotFoundByInvestor);
                openSellDetailQuery = _mapper.Map<RstOpenSellDetailInfoDto>(cartQuery);
            }

            if (openSellDetailQuery == null)
            {
                _rstOrderEFRepository.ThrowException(ErrorCode.RstOpenSellDetailNotFound);
            }

            //Kiểm tra xem là nhà đầu tư cá nhân hay là nhà đầu tư doanh nghiệp
            _cifCodeEFRepository.FindByInvestor(input.InvestorId ?? 0).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            _investorEFRepository.GetInvestorById(input.InvestorId ?? 0, false).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
            //Kiểm tra thông tin giấy tờ tùy thân
            _investorEFRepository.GetIdentificationById(input.InvestorIdenId ?? 0).ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);

            //Kiểm tra thông tin địa chỉ nhận bản cứng
            if (input.ContractAddressId != null)
            {
                _investorRepository.GetContactAddress(input.InvestorId ?? 0, input.ContractAddressId ?? 0)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestorContactAddressNotFound);
            }

            // Giá đặt cọc của dự án
            var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(openSellDetailQuery.Price, openSellDetailQuery.DistributionId);
            if (productItemPrice.DistributionPolicyId == 0)
            {
                _rstOrderEFRepository.ThrowException(ErrorCode.RstDistributionPolicyNotFound);
            }

            RstOrder inputInsert = _mapper.Map<RstOrder>(input);
            inputInsert.TradingProviderId = openSellDetailQuery.TradingProviderId;
            inputInsert.OpenSellDetailId = openSellDetailQuery.OpenSellDetailId;
            inputInsert.DistributionPolicyId = productItemPrice.DistributionPolicyId;

            _rstOrderEFRepository.AppInvestorOrderAdd(inputInsert, input.InvestorId ?? 0, username, true);
        }

        /// <summary>
        ///  Kiểm tra sản phẩm của mở bán trước khi đặt lệnh
        /// </summary>
        public void CheckOpenSellDetailBeforeOrder(int openSellDetailId)
        {
            // Tìm kiếm thông tin của sản phẩm mở bán
            _rstOpenSellDetailEFRepository.OpenSellDetailInfo(openSellDetailId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            _rstOrderEFRepository.CheckOpenSellDetail(openSellDetailId, true);
        }

        /// <summary>
        /// Nhà đầu tư tự thêm hợp đồng đặt lệnh
        /// </summary>
        public async Task<AppRstOrderDataSuccessDto> InvestorOrderAdd(AppCreateRstOrderDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var data = await InvestorOrderAddCommon(input, investorId, isSelfDoing: true);
            return data;
        }

        /// <summary>
        /// Sale thêm hợp đồng BondOrder cho nhà đầu tư
        /// </summary>
        public async Task<AppRstOrderDataSuccessDto> SaleInvestorOrderAdd(AppSaleCreateRstOrderDto input)
        {

            _logger.LogInformation($"{nameof(SaleInvestorOrderAdd)}: input = {JsonSerializer.Serialize(input)}, investorId = {input.InvestorId}");
            var data = await InvestorOrderAddCommon(input, input.InvestorId, isSelfDoing: false);
            return data;
        }

        /// <summary>
        /// Thêm hợp đồng BondOrder cho nhà đầu tư (dùng chung cho tự đặt lệnh và sale đặt hộ)
        /// </summary>
        private async Task<AppRstOrderDataSuccessDto> InvestorOrderAddCommon(AppCreateRstOrderDto input, int investorId, bool isSelfDoing)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            _logger.LogInformation($"{nameof(InvestorOrderAddCommon)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}, username = {username}");

            //RstOpenSellDetailInfoDto openSellDetailQuery = null;

            //if (input.OpenSellDetailId != null)
            //{
            //    // Tìm kiếm thông tin của sản phẩm mở bán
            //    openSellDetailQuery = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(input.OpenSellDetailId ?? 0).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            //}
            //if (input.CartId != null)
            //{
            //    var cartQuery = _rstCartEFRepository.GetByIdAndInvestorId(investorId, input.CartId ?? 0).ThrowIfNull(_dbContext, ErrorCode.RstCartNotFoundByInvestor);
            //    openSellDetailQuery = _mapper.Map<RstOpenSellDetailInfoDto>(cartQuery);
            //}

            //if (openSellDetailQuery == null)
            //{
            //    _rstOrderEFRepository.ThrowException(ErrorCode.RstOpenSellDetailNotFound);
            //}
            //Kiểm tra xem là nhà đầu tư cá nhân hay là nhà đầu tư doanh nghiệp
            var findCifCode = _cifCodeEFRepository.FindByInvestor(investorId).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            var investorQuery = _investorEFRepository.GetInvestorById(investorId, false).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
            var phone = investorQuery.Phone;
            //Kiểm tra thông tin giấy tờ tùy thân
            //var investorIdentification = _investorEFRepository.GetIdentificationById(input.InvestorIdenId ?? 0).ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);

            //Kiểm tra thông tin địa chỉ nhận bản cứng
            if (input.ContractAddressId != null)
            {
                _investorRepository.GetContactAddress(investorId, input.ContractAddressId ?? 0).ThrowIfNull(_dbContext, ErrorCode.InvestorContactAddressNotFound);
            }

            // Giá đặt cọc của dự án
            var productItemFind = _rstProductItemEFRepository.Entity.FirstOrDefault(o => o.Id == input.Id).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var productItemPrice = _rstProductItemEFRepository.ProductItemPrice(productItemFind.Price.Value);
            //if (productItemPrice.DistributionPolicyId == 0)
            //{
            //    _rstOrderEFRepository.ThrowException(ErrorCode.RstDistributionPolicyNotFound);
            //}

            RstOrder inputInsert = _mapper.Map<RstOrder>(input);
            inputInsert.ProductItemId = input.Id;
            //inputInsert.TradingProviderId = openSellDetailQuery.TradingProviderId;
            //inputInsert.OpenSellDetailId = openSellDetailQuery.OpenSellDetailId;
            inputInsert.DepositMoney = productItemPrice.DepositPrice;
            //inputInsert.DistributionPolicyId = productItemPrice.DistributionPolicyId;
            inputInsert.CifCode = findCifCode.CifCode;
            inputInsert.IpAddressCreated = ipAddress;
            inputInsert.InvestorId = investorId;
            // Tính đến thời gian hết hạn cọc
            //inputInsert.ExpTimeDeposit = _rstOpenSellEFRepository.ExpTimeDeposit(openSellDetailQuery.OpenSellId);
            // Tìm kiếm thông tin sale nếu có mã giới thiệu
            //if (!string.IsNullOrWhiteSpace(input.SaleReferralCode))
            //{
            //    var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, openSellDetailQuery.TradingProviderId);
            //    if (findSale != null)
            //    {
            //        inputInsert.SaleReferralCode = findSale.ReferralCode;
            //        inputInsert.DepartmentId = findSale.DepartmentId;
            //        inputInsert.SaleReferralCodeSub = findSale.ReferralCodeSub;
            //        inputInsert.DepartmentIdSub = findSale.DepartmentIdSub;
            //    }
            //}

            // Thêm lệnh. Sale đặt hộ thì lưu thêm sale id
            if (isSelfDoing)
            {
                inputInsert.Source = SourceOrder.ONLINE;
                inputInsert.Status = OrderStatus.CHO_THANH_TOAN;
                inputInsert.StatusMax = OrderStatus.CHO_THANH_TOAN;
            }
            else
            {
                var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
                inputInsert.SaleOrderId = saleId;
                inputInsert.Source = SourceOrder.OFFLINE;
                inputInsert.Status = OrderStatus.KHOI_TAO;
                inputInsert.StatusMax = OrderStatus.KHOI_TAO;
            }

            var transaction = _dbContext.Database.BeginTransaction();
            var insertOrder = _rstOrderEFRepository.AppInvestorOrderAdd(inputInsert, investorId, username);
            var productItemQuery = _rstProductItemEFRepository.Entity.FirstOrDefault(c => c.Id == inputInsert.ProductItemId && c.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            productItemQuery.Status = RstProductItemStatus.GIU_CHO;
            // Nếu có id từ giỏ hàng
            if (input.CartId != null)
            {
                var cartFind = _rstCartEFRepository.Entity.FirstOrDefault(c => c.Id == input.CartId && c.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.RstCartNotFoundByInvestor);
                cartFind.Status = RstCartStatus.DaGiaoDich;
                cartFind.TransDate = DateTime.Now;
            }

            // Nếu mở bán cài thời gian giữ chỗ
            // Không cài thời gian thì hợp đồng nào thanh toán trước thì giữ chỗ trước
            /*if (openSellDetailQuery.KeepTime != null)
            {
                //Đổ trạng thái căn hộ và trạng thái sản phẩm của mở bán sang giữ chỗ
                var productItemQuery = _rstProductItemEFRepository.Entity.FirstOrDefault(c => c.Id == inputInsert.ProductItemId && c.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
                productItemQuery.Status = RstProductItemStatus.GIU_CHO;
                var openSellDetail = _rstOpenSellDetailEFRepository.Entity.FirstOrDefault(c => c.Id == inputInsert.OpenSellDetailId && c.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
                openSellDetail.Status = RstProductItemStatus.GIU_CHO;
                //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItemQuery.Id, null, null, null, RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.HOLD} - KH: {phone}", DateTime.Now, RstHistoryTypes.GiuCho), username);
                //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, null, null, null, RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, $"{RstHistoryUpdateSummary.HOLD} - KH: {phone}", DateTime.Now, RstHistoryTypes.GiuCho), username);
            }*/
            _dbContext.SaveChanges();

            // Nếu đặt lệnh không qua giỏ hàng, tìm sản phẩm mở bán đã có trong giỏ hàng cập nhật trạng thái khác
            //var openSellInCart = _rstCartEFRepository.Entity.FirstOrDefault(c => input.CartId == null && c.InvestorId == investorId && c.OpenSellDetailId == input.OpenSellDetailId
            //                    && c.Status == RstCartStatus.KhoiTao && c.Deleted == YesNo.NO);
            //if (openSellInCart != null)
            //{
            //    openSellInCart.Status = RstCartStatus.DaGiaoDichKhac;
            //    openSellInCart.TransDate = DateTime.Now;
            //}

            #region Giấy tờ người đồng sở hữu
            if (input.OrderCoOwners != null)
            {
                var orderCoOwners = JsonSerializer.Deserialize<List<AppCreateRstOrderCoOwnerDto>>(input.OrderCoOwners);
                // Thêm danh sách người đồng sở hữu
                foreach (var orderCoOwnerItem in orderCoOwners)
                {
                    if (orderCoOwnerItem.InvestorIdenId != null || orderCoOwnerItem.InvestorIdenId == 0)
                    {
                        _rstOrderCoOwnerRepository.Add(new RstOrderCoOwner
                        {
                            OrderId = insertOrder.Id,
                            InvestorIdenId = orderCoOwnerItem.InvestorIdenId,
                        });
                    }
                    else
                    {
                        //if (input.IdFrontImages.FirstOrDefault(r => r.FileName == orderCoOwnerItem.IdFrontImageUrl) == null
                        //    || input.IdFrontImages.FirstOrDefault(r => r.FileName == orderCoOwnerItem.IdFrontImageUrl) == null)
                        //{
                        //    continue;
                        //}
                        //string frontImageUrl = _fileServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                        //{
                        //    File = input.IdFrontImages.FirstOrDefault(r => r.FileName == orderCoOwnerItem.IdFrontImageUrl),
                        //    Folder = FileFolder.INVESTOR,
                        //});
                        //string backImageUrl = _fileServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                        //{
                        //    File = input.IdFrontImages.FirstOrDefault(r => r.FileName == orderCoOwnerItem.IdFrontImageUrl),
                        //    Folder = FileFolder.INVESTOR,
                        //});
                        _rstOrderCoOwnerRepository.Add(new RstOrderCoOwner
                        {
                            OrderId = insertOrder.Id,
                            Fullname = orderCoOwnerItem.Fullname,
                            Phone = orderCoOwnerItem.Phone,
                            Address = orderCoOwnerItem.Address,
                            IdType = orderCoOwnerItem.IdType,
                            //IdBackImageUrl = frontImageUrl,
                            //IdFrontImageUrl = backImageUrl,
                            CreatedBy = username
                        });
                    }
                }
                _dbContext.SaveChanges();
            }
            #endregion
            //insertOrder.ProjectName = openSellDetailQuery.ProjectName;
            //insertOrder.Hotline = openSellDetailQuery.Hotline;
            insertOrder.Phone = investorQuery.Phone;
            //insertOrder.FullName = investorIdentification.Fullname;
            //insertOrder.ProductItemPrice = openSellDetailQuery.Price;
            //insertOrder.ProductItemCode = openSellDetailQuery.Code;
            //insertOrder.KeepTime = openSellDetailQuery.KeepTime;

            //insertOrder.TradingBankAccounts = await TradingBankAccountOfOpenSell(insertOrder.Id, openSellDetailQuery.OpenSellId, insertOrder.ContractCode, insertOrder.DepositMoney);
            //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(insertOrder.Id, "Đang mở bán", "Giữ chỗ", "Trạng thái",
            //            RstHistoryUpdateTables.RST_ORDER, ActionTypes.THEM_MOI, $"KH: {phone} - Đặt lệnh", DateTime.Now), username);

            //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetailQuery.ProductItemId, "Đang mở bán", "Giữ chỗ", "Trạng thái",
            //            RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.THEM_MOI, $"KH: {phone} - Đặt lệnh", DateTime.Now), username);

            //_rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetailQuery.OpenSellDetailId, "Đang mở bán", "Giữ chỗ", "Trạng thái",
            //            RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.THEM_MOI, $"KH: {phone} - Đặt lệnh", DateTime.Now), username);
            //var data = _rstOrderContractFileServices.GetReplaceTextContractFile(new RstExportContracDto()
            //{
            //    ProductItemId = inputInsert.ProductItemId,
            //    ProjectId = openSellDetailQuery.ProjectId,
            //    OpenSellDetailId = inputInsert.OpenSellDetailId,
            //    InvestorId = findCifCode.InvestorId,
            //    BusinessCustomerId = findCifCode.BusinessCustomerId,
            //    IdentificationId = inputInsert.InvestorIdenId ?? 0
            //});
            transaction.Commit();
            //_backgroundJobs.Enqueue(() => _rstOrderContractFileServices.CreateContractFileOrderApp(inputInsert, data, inputInsert.TradingProviderId));
            // Nếu cài thời gian giữ chỗ ở mở bán
            //if (openSellDetailQuery.KeepTime != null)
            //{
            //    string jobId = _backgroundJobs.Schedule(() => _rstBackgroundJobServices.UpdateDepositExpire(insertOrder.Id), TimeSpan.FromSeconds(openSellDetailQuery.KeepTime ?? 0));
            //    var orderFind = _rstOrderEFRepository.Entity.FirstOrDefault(o => o.Id == inputInsert.Id)
            //        .ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound);
            //    orderFind.DepositJobId = jobId;
            //    _dbContext.SaveChanges();
            //    await _rstSignalRBroadcastServices.BroadcastOpenSellDetailChangeStatus(openSellDetailQuery.OpenSellDetailId);
            //}
            return insertOrder;
        }

        /// <summary>
        /// Quản lý hợp đồng lệnh
        /// </summary>
        public async Task<List<AppRstOrderDto>> AppGetAllOrder(AppRstOrderFilterDto input, int groupStatus)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var resultOrderQuery = _rstOrderEFRepository.AppGetAllOrder(input, investorId, groupStatus);
            List<AppRstOrderDto> result = new List<AppRstOrderDto>();

            foreach (var item in resultOrderQuery)
            {
                var openSellDetail = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(item.OpenSellDetailId);
                if (openSellDetail != null)
                {
                    var paymentMoney = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(item.Id);
                    var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByPolicy(openSellDetail.Price, item.DistributionPolicyId);

                    // Tab Sổ lệnh - Kiểm tra lại trường hợp hết thời gian nhưng hợp đồng đã đủ điều kiện khóa căn thì bỏ qua
                    if (groupStatus == RstAppOrderGroupStatus.SO_LENH && item.ExpTimeDeposit < DateTime.Now && paymentMoney > productItemPrice.LockPrice
                        && openSellDetail.ProductItemStatus == RstProductItemStatus.KHOA_CAN && openSellDetail.OpenSellDetailStatus == RstProductItemStatus.KHOA_CAN)
                    {
                        continue;
                    }
                    // Tab Đang giao dịch - Kiểm tra lại trường hợp hết thời gian nhưng hợp đồng đã đủ điều kiện khóa căn thì bỏ qua
                    if (groupStatus == RstAppOrderGroupStatus.DANG_GIAO_DICH && item.Status == RstOrderStatus.CHO_THANH_TOAN_COC && item.ExpTimeDeposit < DateTime.Now 
                        && paymentMoney < productItemPrice.LockPrice)
                    {
                        continue;
                    }
                    AppRstOrderDto resultItem = _mapper.Map<AppRstOrderDto>(openSellDetail);
                    // Id của hợp đồng
                    resultItem.Id = item.Id;
                    resultItem.OpenSellDetailId = openSellDetail.Id;
                    resultItem.DepositMoney = item.DepositMoney;
                    resultItem.ExpTimeDeposit = item.ExpTimeDeposit;
                    resultItem.Status = item.Status;
                    // Số tiền đã thanh toán
                    resultItem.PaymentMoney = paymentMoney;
                    // Ảnh đại diện của căn hộ
                    resultItem.UrlImage = _dbContext.RstProductItemMedias.FirstOrDefault(m => m.ProductItemId == item.ProductItemId && m.Deleted == YesNo.NO
                                                                            && m.Location == RstProductItemMediaLocations.ANH_DAI_DIEN_CAN_HO)?.UrlImage;
                    // Trường hợp hợp đồng đang ở trạng thái khởi tạo
                    if (resultItem.Status == RstOrderStatus.KHOI_TAO || resultItem.Status == RstOrderStatus.CHO_THANH_TOAN_COC)
                    {
                        resultItem.CanContinueTrading = YesNo.NO;
                        // Nếu có thời gian giữ chỗ
                        if ((openSellDetail.ProductItemStatus == RstProductItemStatus.LOGIC_DANG_MO_BAN && (item.ExpTimeDeposit == null || (item.ExpTimeDeposit != null && item.ExpTimeDeposit < DateTime.Now)))
                            || (openSellDetail.ProductItemStatus == RstProductItemStatus.GIU_CHO && item.ExpTimeDeposit != null && item.ExpTimeDeposit > DateTime.Now))
                        {
                            resultItem.CanContinueTrading = YesNo.YES;
                            // Thông tin ngân hàng và thanh toán lệnh
                            resultItem.PaymentNote = PaymentNotes.THANH_TOAN + item.ContractCode;
                            //resultItem.TradingBankAccounts = await TradingBankAccountOfOpenSell(resultItem.Id, openSellDetail.OpenSellId, item.ContractCode, item.DepositMoney - resultItem.PaymentMoney);
                        }
                    }
                    result.Add(resultItem);
                }
            }
            return result;
        }

        /// <summary>
        /// Xem chi tiết hợp đồng
        /// </summary>
        public async Task<AppRstOrderDetailDto> AppOrderDetail(int orderId)
        {
            _logger.LogInformation($"{nameof(AppOrderDetail)}: openSellId = {orderId}");

            var orderQuery = _rstOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound);

            var openSellDetail = _rstOpenSellDetailEFRepository.OpenSellDetailInfo(orderQuery.OpenSellDetailId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var productItem = _rstProductItemEFRepository.AppProductItemDetail(orderQuery.OpenSellDetailId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            var projectStructure = _rstProjectStructureEFRepository.FindById(productItem.BuildingDensityId ?? 0);

            AppRstOrderDetailDto result = _mapper.Map<AppRstOrderDetailDto>(productItem);

            result.Id = orderId;
            result.ProjectCode = openSellDetail.ProjectCode;
            result.ProjectName = openSellDetail.ProjectName;
            result.ContractCode = orderQuery.ContractCode;
            result.DepositMoney = orderQuery.DepositMoney;
            result.ExpTimeDeposit = orderQuery.ExpTimeDeposit;
            result.OpenSellDetailStatus = openSellDetail.OpenSellDetailStatus;
            result.OrderStatus = orderQuery.Status;
            result.ProductItemId = productItem.Id;
            result.BuildingDensityName = projectStructure?.Name;
            result.PaymentNote = PaymentNotes.THANH_TOAN + orderQuery.ContractCode;
            result.OpenSellDetailId = openSellDetail.Id;
            result.OpenSellId = openSellDetail.OpenSellId;
            result.KeepTime = openSellDetail.KeepTime;
            // Số tiền đã thanh toán
            result.PaymentMoney = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(orderId);
            // Số tiền còn phải thanh toán
            result.PayableAmount = result.DepositMoney - result.PaymentMoney;
            result.PaymentType = orderQuery.PaymentType;

            #region Thông tin liên quan
            //Lấy ra danh sách tiện ích của sản phẩm
            result.Utilities = new();
            var getAllUtilityOfProjectItem = _rstProductItemProjectUtilityEFRepository.Entity.Where(e => e.ProductItemId == productItem.Id && e.Status == Status.ACTIVE && e.Deleted == YesNo.NO);
            foreach (var productItemUtility in getAllUtilityOfProjectItem)
            {
                var resultItem = new AppRstProductItemUtilityDto();
                var utility = RstProjectUtilityData.UtilityData.FirstOrDefault(e => e.Id == productItemUtility.ProductItemUtilityId);
                resultItem.Name = utility?.Name;
                resultItem.Icon = utility?.Icon;
                result.Utilities.Add(resultItem);
            }

            // Lấy mẫu hợp đồng đặt cọc căn hộ mở bán do đại lý cài
            result.DepositContracts = (from openSellContract in _dbContext.RstOpenSellContractTemplates
                                       join contractTemplateTemp in _dbContext.RstContractTemplateTemps on openSellContract.ContractTemplateTempId equals contractTemplateTemp.Id
                                       where openSellContract.Deleted == YesNo.NO && contractTemplateTemp.Deleted == YesNo.NO
                                       && openSellContract.Status == Status.ACTIVE && contractTemplateTemp.Status == Status.ACTIVE
                                       && contractTemplateTemp.ContractType == RstContractTypes.HD_DAT_COC && (contractTemplateTemp.ContractSource == ContractSources.ALL || contractTemplateTemp.ContractSource == ContractSources.ONLINE)
                                       select new AppRstOpenSellContractTemplateDto()
                                       {
                                           Id = openSellContract.Id,
                                           ContractTemplateTempName = contractTemplateTemp.Name,
                                           EffectiveDate = openSellContract.EffectiveDate,
                                       }).ToList();
            // Lấy chính sách ưu đãi
            result.Policys = _rstSellingPolicyEFRepository.AppRstPolicyForProductItem(openSellDetail.OpenSellId, productItem.Id);
            // Danh sách thông tin khác của căn hộ
            result.ProductItemExtends = _mapper.Map<List<AppRstProductItemExtendDto>>(_rstProductItemExtendRepository.GetAll(productItem.Id));

            // Lấy danh sách dòng tiền thanh toán đặt cọc
            result.OrderCashFlows = (from orderPayment in _rstOrderPaymentEFRepository.Entity
                                     where orderPayment.OrderId == orderId && orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN
                                     && orderPayment.TranClassify == TranClassifies.THANH_TOAN && orderPayment.TranType == TranTypes.THU && orderPayment.Deleted == YesNo.NO
                                     select new AppRstOrderCashFlowDto
                                     {
                                         PaymentAmount = orderPayment.PaymentAmount,
                                         Description = orderPayment.Description,
                                         OrderNo = orderPayment.OrderNo,
                                         Status = orderPayment.Status,
                                         TranDate = orderPayment.TranDate
                                     }).ToList();
            #endregion

            // Thông tin người đồng sở hữu của Lệnh
            result.OrderCoOwners = _rstOrderCoOwnerRepository.AppOrderCoOwners(orderId);
            if (orderQuery.Status == RstOrderStatus.KHOI_TAO || orderQuery.Status == RstOrderStatus.CHO_THANH_TOAN_COC)
            {
                result.TradingBankAccounts = await TradingBankAccountOfOpenSell(orderQuery.Id, openSellDetail.OpenSellId, orderQuery.ContractCode, result.PayableAmount);
            }

            // Tiến độ giao dịch của hợp đồng
            result.PaymentTransactions = new();

            // Thêm trước 1 loại thanh toán cọc , sau có thì để hàm riêng
            result.PaymentTransactions.Add(new AppPaymentTransactionDto
            {
                AmountMoney = result.DepositMoney,
                Status = (result.DepositMoney - result.PaymentMoney > 0) ? RstPaymentTransactionStatus.CHUA_THANH_CONG : RstPaymentTransactionStatus.THANH_CONG,
                TransactionIndex = RstPaymentTransactionIndex.DAT_COC
            });
            return result;
        }

        /// <summary>
        /// Kiểm tra mã giới thiệu thuộc đại lý đang bán căn hộ
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="openSellDetailId"></param>
        /// <returns></returns>
        public AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int openSellDetailId)
        {
            _logger.LogInformation($"{nameof(AppSaleOrderFindReferralCode)}: referralCode = {referralCode}, openSellDetailId = {openSellDetailId} ");
            var openSellQuery = (from openSellDetail in _dbContext.RstOpenSellDetails
                                 join openSell in _dbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                 where openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                                 select openSell).FirstOrDefault().ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            var result = _saleEFRepository.FindSaleByReferralCode(referralCode, openSellQuery.TradingProviderId);
            return _mapper.Map<AppSaleByReferralCodeDto>(result);
        }

        /// <summary>
        /// Xóa hợp đồng từ trên App
        /// </summary>
        public void AppDeleteOrder(int orderId)
        {
            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppDeleteOrder)}: orderId = {orderId}, username = {username}, investorId = {investorId}");

            var transaction = _dbContext.Database.BeginTransaction();
            var orderFind = _rstOrderEFRepository.AppDeleteOrder(orderId, investorId, username);
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Gia hạn hợp đồng trên App (gia hạn thời gian giữ chỗ)
        /// Khi hợp đồng hết thời gian giữ chỗ mà Căn hộ vẫn đang mở bán Status = 1
        /// </summary>
        /// <param name="orderId"></param>
        public void AppExtendedKeepTime(int orderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AppExtendedKeepTime)}: orderId = {orderId}, investorId = {investorId}, username ={username}");

            var ciCodeQuery = _cifCodeEFRepository.FindByInvestor(investorId).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            var orderQuery = _rstOrderEFRepository.Entity.FirstOrDefault(o => o.Id == orderId && o.CifCode == ciCodeQuery.CifCode && o.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound);

            // Check xem căn hộ nếu chưa được giao dịch thì mới được gia hạn
            var dataQuery = (from order in _dbContext.RstOrders
                             join productItem in _dbContext.RstProductItems on order.ProductItemId equals productItem.Id
                             join openSellDetail in _dbContext.RstOpenSellDetails on order.OpenSellDetailId equals openSellDetail.Id
                             join openSell in _dbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                             where order.Id == orderId && productItem.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                             && new int[] { RstOrderStatus.KHOI_TAO, RstOrderStatus.CHO_THANH_TOAN_COC }.Contains(order.Status)
                             && openSell.Status == RstDistributionStatus.DANG_BAN && openSellDetail.Status == RstProductItemStatus.KHOI_TAO
                             && productItem.Status == RstProductItemStatus.KHOI_TAO && order.ExpTimeDeposit < DateTime.Now && openSell.KeepTime != null
                             select new
                             {
                                 KeepTime = openSell.KeepTime
                             }).FirstOrDefault();
            if (dataQuery == null)
            {
                _rstOrderEFRepository.ThrowException(ErrorCode.RstOrderCannotExtendedKeepTime);
            }

            //xoá job cũ
            if (orderQuery.DepositJobId != null)
            {
                _backgroundJobs.Delete(orderQuery.DepositJobId);
            }

            // Lưu lại lý do gia hạn thời gian dữ chỗ
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate
            {
                RealTableId = orderId,
                OldValue = "",
                NewValue = dataQuery.KeepTime.ToString(),
                FieldName = RstFieldName.UPDATE_ORDER_EXP_TIME_DEPOSIT,
                UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                Action = ActionTypes.CAP_NHAT,
                ActionUpdateType = RstActionUpdateTypes.UPDATE_ORDER_EXP_TIME_DEPOSIT,
                UpdateReason = RstUpdateReasons.KHACH_XIN_GIA_HAN_THOI_GIAN,
                Summary = "Khách hàng gia hạn thời gian giữ chỗ hợp đồng",
                CreatedDate = DateTime.Now,
            }, username);
            _dbContext.SaveChanges();

            string jobId = _backgroundJobs.Schedule(() => _rstBackgroundJobServices.UpdateDepositExpire(orderQuery.Id), TimeSpan.FromSeconds((double)dataQuery.KeepTime));
            orderQuery.DepositJobId = jobId;
            orderQuery.ExpTimeDeposit = DateTime.Now.AddSeconds((double)dataQuery.KeepTime);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tài khoản nhận tiền thanh toán của mở bán 
        /// </summary>
        public async Task<List<AppTradingBankAccountDto>> TradingBankAccountOfOpenSell(long orderId, int openSellId, string contractCode, decimal depositMoney)
        {
            _logger.LogInformation($"{nameof(TradingBankAccountOfOpenSell)}: orderId = {orderId}, openSellId = {openSellId}, contractCode = {contractCode}, depositMoney = {depositMoney}");
            var result = new List<AppTradingBankAccountDto>();
            // Lấy danh sách ngân hàng nhận tiền của mở bán
            var openSellBanks = _rstOpenSellBankEFRepository.GetAll(openSellId);

            // Nếu hợp đồng đã có thanh toán trước đó thì chỉ lấy ra ngân hàng được thanh toán đó
            var orderPaymentFirstQuery = _rstOrderPaymentEFRepository.Entity.FirstOrDefault(o => o.OrderId == orderId && o.TranType == TranTypes.THU && o.TranClassify == TranClassifies.THANH_TOAN
                                        && o.PaymentType == PaymentTypes.CHUYEN_KHOAN && o.Status != OrderPaymentStatus.HUY_THANH_TOAN && o.Deleted == YesNo.NO);
            // Lọc
            if (orderPaymentFirstQuery != null)
            {
                openSellBanks = openSellBanks.Where(o => (orderPaymentFirstQuery.TradingBankAccountId == null || o.TradingBankAccountId == orderPaymentFirstQuery.TradingBankAccountId)
                                                      && (orderPaymentFirstQuery.PartnerBankAccountId == null || o.PartnerBankAccountId == orderPaymentFirstQuery.PartnerBankAccountId));
            }
            foreach (var bankItem in openSellBanks)
            {
                // Ngân hàng nhận tiền của đại lý hoặc là của đối tác
                int? receiveBankAccountId = (bankItem.TradingBankAccountId != null) ? bankItem.TradingBankAccountId : bankItem.PartnerBankAccountId;

                var resultBankItem = new AppTradingBankAccountDto();
                var tradingBankFind = _businessCustomerEFRepository.FindBankById(receiveBankAccountId ?? 0);
                if (tradingBankFind != null)
                {
                    resultBankItem = _mapper.Map<AppTradingBankAccountDto>(tradingBankFind);
                    if (tradingBankFind.BankId == FixBankId.Msb)
                    {
                        var prefixAcc = _tradingMSBPrefixAccountEFRepository.FindByTradingBankId(tradingBankFind.BusinessCustomerBankAccId);
                        if (prefixAcc != null)
                        {
                            // Sinh QrCode nếu không sinh được lấy như bình thường
                            try
                            {
                                var requestCollect = await _msbCollectMoneyServices.RequestCollectMoney(new MSB.Dto.CollectMoney.RequestCollectMoneyDto
                                {
                                    TId = prefixAcc.TId,
                                    MId = prefixAcc.MId,
                                    OrderCode = $"{ContractCodes.REAL_ESTATE}{orderId}",
                                    AmountMoney = depositMoney,
                                    OwnerAccount = tradingBankFind.BankAccName,
                                    PrefixAccount = prefixAcc.PrefixMsb,
                                    Note = contractCode
                                });
                                resultBankItem.BankAccNo = requestCollect.AccountNumber;
                                resultBankItem.QrCode = requestCollect.QrCode;
                            }
                            catch (Exception ex)
                            {
                                if (ex.GetType() != typeof(FaultException))
                                {
                                    _logger.LogError(ex, $"{nameof(TradingBankAccountOfOpenSell)}: exception = {ex.Message}");
                                }
                            }
                        }
                    }
                    result.Add(resultBankItem);
                }
            }
            return result;
        }

        public List<RstProductItemDto> FindAllProductItemByInvestor()
        {
            //int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}");
            var result = new List<RstProductItemDto>();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var orderFind = _dbContext.RstOrders.Where(o => o.InvestorId == investorId && o.Deleted == YesNo.NO);
            if (orderFind.Any() )
            {
                foreach( var productItemId in orderFind.Select(o => o.ProductItemId)) {
                    var productItemFind = _dbContext.RstProductItems.FirstOrDefault(o => o.Id == productItemId && o.Deleted == YesNo.NO);
                    var item = _mapper.Map<RstProductItemDto>(productItemFind);
                    item.ProductItemMedias = _dbContext.RstProductItemMedias.Where(i => i.ProductItemId == productItemId).Select(e => new AppRstProductItemMediaDto
                    {
                        Id = e.Id,
                        GroupTitle = e.GroupTitle,
                        ProductItemId = e.ProductItemId,
                        Location = e.Location,
                        MediaType = e.MediaType,
                        UrlImage = e.UrlImage,
                        UrlPath = e.UrlPath
                    });
                    result.Add(item);
                }
            }
            return result;
        }
        #endregion
    }
}
