using AutoMapper;
using ClosedXML.Excel;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestRepositories;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.CollectMoney;
using EPIC.MSB.Dto.Inquiry;
using EPIC.MSB.Dto.Notification;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentRepositories;
using EPIC.PaymentSharedEntities.Dto;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OrderPayment = EPIC.InvestEntities.DataEntities.OrderPayment;

namespace EPIC.PaymentDomain.Implements
{
    public class MsbPaymentServices : IMsbPaymentServices
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IInvestOrderContractFileServices _investOrderContractFileServices;
        private readonly IGarnerOrderContractFileServices _garnerOrderContractFileServices;
        private readonly string _connectionString;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly DefErrorEFRepository _defErrorEFRepository;

        //core
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly NotificationServices _notificationServices;
        private readonly BankEFRepository _bankEFRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvRenewalsRequestRepository _invRenewalsRequestRepository;

        //msb
        private readonly MsbCollectMoneyServices _msbCollectMoneyServices;
        private readonly MsbPayMoneyServices _msbPayMoneyServices;
        private readonly MsbInquiryServices _msbInquiryServices;
        private readonly MsbNotificationRepository _msbPaymentRepository;
        private readonly MsbNotificationPaymentRepository _msbNotificationPaymentRepository;
        private readonly MsbRequestPaymentDetailEFRepository _msbRequestPaymentDetailEFRepository;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;
        private readonly PartnerMsbPrefixAccountEFRepository _partnerMsbPrefixAccountEFRepository;

        //bond
        private readonly BondOrderRepository _bondOrderRepository;

        //invest
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvestOrderRepository _invOrderRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestOrderPaymentEFRepository _investOrderPaymentEFRepository;
        private readonly InvestWithdrawalEFRepository _investWithdrawalEFRepository;
        private readonly InvestNotificationServices _investNotificationServices;
        private readonly InvestOrderPaymentEFRepository _invOrderPaymentEFRepository;
        private readonly InvestOrderEFRepository _invOrderEFRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly InvestInterestPaymentEFRepository _investInterestPaymentEFRepository;
        private readonly IInterestPaymentServices _investInterestPaymentServices;
        private readonly IRstSignalRBroadcastServices _rstSignalRBroadcastServices;
        private readonly ProjectRepository _investProjectRepository;
        private readonly DistributionRepository _investDistributionRepository;

        //garner
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly GarnerWithdrawalDetailEFRepository _garnerWithdrawalDetailEFRepository;
        private readonly GarnerNotificationServices _garnerNotificationServices;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly GarnerInterestPaymentDetailEFRepository _garnerInterestPaymentDetailEFRepository;

        //realestate
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderPaymentEFRepository _rstOrderPaymentEFRepository;
        private readonly RealEstateNotificationServices _rstNotificationServices;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly InvestHistoryUpdateEFRepository _investHistoryUpdateEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;
        private readonly PaymentBackgroundJobServices _paymentBackgroundJobServices;

        //event
        //private readonly IEvtOrderService _evtOrderServices;
        private readonly EvtOrderEFRepository _evtOrderRepository;
        private readonly EvtOrderPaymentEFRepository _evtOrderPaymentEFRepository;
        private readonly IEvtSignalRBroadcastService _evtSignalRBroadcastService;
        private readonly EventNotificationServices _eventNotificationServices;

        public MsbPaymentServices(
            ILogger<MsbPaymentServices> logger,
            IHttpContextAccessor httpContext,
            IWebHostEnvironment env,
            IMapper mapper,
            IBackgroundJobClient backgroundJobs,
            IRstSignalRBroadcastServices rstSignalRBroadcastServices,
            IInvestOrderContractFileServices investOrderContractFileServices,
            IGarnerOrderContractFileServices garnerOrderContractFileServices,
            IInterestPaymentServices investInterestPaymentServices,
            DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext,
            MsbCollectMoneyServices msbCollectMoneyServices,
            MsbPayMoneyServices msbPayMoneyServices,
            MsbInquiryServices msbInquiryServices,
            GarnerNotificationServices garnerNotificationServices,
            InvestNotificationServices investNotificationServices,
            NotificationServices notificationServices,
            PaymentBackgroundJobServices paymentBackgroundJobServices,
            EventNotificationServices eventNotificationServices,
            RealEstateNotificationServices rstNotificationServices,
            IEvtSignalRBroadcastService evtSignalRBroadcastService)
        {
            _logger = logger;
            _httpContext = httpContext;
            _env = env;
            _mapper = mapper;
            _backgroundJobs = backgroundJobs;
            _investOrderContractFileServices = investOrderContractFileServices;
            _garnerOrderContractFileServices = garnerOrderContractFileServices;
            _investInterestPaymentServices = investInterestPaymentServices;
            _rstSignalRBroadcastServices = rstSignalRBroadcastServices;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _msbPaymentRepository = new MsbNotificationRepository(dbContext, logger);
            _msbNotificationPaymentRepository = new MsbNotificationPaymentRepository(dbContext, logger);
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _msbRequestPaymentDetailEFRepository = new MsbRequestPaymentDetailEFRepository(dbContext, logger);
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, logger);
            _garnerWithdrawalDetailEFRepository = new GarnerWithdrawalDetailEFRepository(dbContext, logger);
            _investInterestPaymentEFRepository = new InvestInterestPaymentEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _invOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _msbCollectMoneyServices = msbCollectMoneyServices;
            _msbPayMoneyServices = msbPayMoneyServices;
            _msbInquiryServices = msbInquiryServices;
            _garnerNotificationServices = garnerNotificationServices;
            _investNotificationServices = investNotificationServices;
            _bankEFRepository = new BankEFRepository(dbContext, logger);
            _usersEFRepository = new UsersEFRepository(dbContext, logger);
            _investOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _notificationServices = notificationServices;
            _investOrderPaymentEFRepository = new InvestOrderPaymentEFRepository(dbContext, logger);
            _investWithdrawalEFRepository = new InvestWithdrawalEFRepository(dbContext, logger);
            _invOrderPaymentEFRepository = new InvestOrderPaymentEFRepository(dbContext, logger);
            _invOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
            _garnerInterestPaymentDetailEFRepository = new GarnerInterestPaymentDetailEFRepository(dbContext, logger);
            _projectRepository = new ProjectRepository(_connectionString, logger);
            _distributionRepository = new DistributionRepository(_connectionString, logger);
            _paymentBackgroundJobServices = paymentBackgroundJobServices;
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, logger, _httpContext);
            _investorRepository = new InvestorRepository(_connectionString, logger);
            _invRenewalsRequestRepository = new InvRenewalsRequestRepository(_connectionString, logger);
            _investProjectRepository = new ProjectRepository(_connectionString, logger);
            _investDistributionRepository = new DistributionRepository(_connectionString, logger);
            _rstNotificationServices = rstNotificationServices;
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstOrderPaymentEFRepository = new RstOrderPaymentEFRepository(dbContext, logger);
            _partnerMsbPrefixAccountEFRepository = new PartnerMsbPrefixAccountEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _investHistoryUpdateEFRepository = new InvestHistoryUpdateEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);
            _eventNotificationServices = eventNotificationServices;
            _evtOrderRepository = new EvtOrderEFRepository(dbContext, logger);
            _evtOrderPaymentEFRepository = new EvtOrderPaymentEFRepository(dbContext, logger);
            _evtSignalRBroadcastService = evtSignalRBroadcastService;
        }

        public PagingResult<MsbNotificationDto> FindAll(MsbFilterPaymentDto input)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                if (!string.IsNullOrWhiteSpace(input.PrefixMsb))
                {
                    //kiểm tra prefix có phải của đại lý không
                    if (!_tradingMSBPrefixAccountEFRepository.Entity.Any(p => p.PrefixMsb == input.PrefixMsb))
                    {
                        input.PrefixMsb = null;
                    }
                }
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                input.PrefixMsb = null;
            }
            var query = _msbPaymentRepository.FindAll(input);
            return new PagingResult<MsbNotificationDto>
            {
                TotalItems = query.TotalItems,
                Items = _mapper.Map<List<MsbNotificationDto>>(query.Items)
            };
        }

        public async Task<string> InquiryBankAccount(int bankId, string bankAccount)
        {
            string ownerAccount = null;
            var bank = _bankEFRepository.FindById(bankId)
                .ThrowIfNull(_dbContext, ErrorCode.CoreBankNotFound);

            if (string.IsNullOrWhiteSpace(bank.Bin))
            {
                _bankEFRepository.ThrowException(ErrorCode.CoreBankBinIsNull);
            }
            ownerAccount = await _msbInquiryServices.InquiryAccount(new InquiryAccountDto
            {
                BankAccount = bankAccount,
                Bin = bank.Bin,
            });
            return ownerAccount;
        }

        class CheckSignatureDto
        {
            public string Username { get; set; }
            public int TradingBankAccountId { get; set; }
        }

        class CheckSignatureTradingPartnerDto
        {
            public string Username { get; set; }
            public int? TradingBankAccountId { get; set; }
            public int? PartnerBankAccountId { get; set; }
        }

        private CheckSignatureTradingPartnerDto CheckSignatureTradingPartner(ReceiveNotificationDto notification, string cifCode, string prefixAccount, int? tradingProviderId = null, int? partnerId = null)
        {
            var cifCodeFind = _cifCodeEFRepository.FindByCifCode(cifCode);
            if (cifCodeFind == null)
            {
                _cifCodeEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound, cifCodeFind);
            }

            var username = _usersEFRepository.FindUserNameByInvestorId(cifCodeFind.InvestorId ?? 0);

            var tradingMSBPrefixFind = _tradingMSBPrefixAccountEFRepository.EntityNoTracking.FirstOrDefault(t => t.TradingProviderId == tradingProviderId && t.PrefixMsb == prefixAccount && t.Deleted == YesNo.NO);
            var partnerMsbPrefixFind = _partnerMsbPrefixAccountEFRepository.EntityNoTracking.FirstOrDefault(p => p.PartnerId == partnerId && p.PrefixMsb == prefixAccount && p.Deleted == YesNo.NO);
            if (tradingMSBPrefixFind != null)
            {
                // Kiểm tra chữ ký
                if (!EnvironmentNames.Develops.Contains(_env.EnvironmentName))
                {
                    _msbCollectMoneyServices.HandleNotificationSignature(notification, tradingMSBPrefixFind.AccessCode);
                }

                return new CheckSignatureTradingPartnerDto
                {
                    TradingBankAccountId = tradingMSBPrefixFind.TradingBankAccountId,
                    Username = username,
                };
            }
            else if (partnerMsbPrefixFind != null)
            {
                // Kiểm tra chữ ký
                if (!EnvironmentNames.Develops.Contains(_env.EnvironmentName))
                {
                    _msbCollectMoneyServices.HandleNotificationSignature(notification, partnerMsbPrefixFind.AccessCode);
                }

                return new CheckSignatureTradingPartnerDto
                {
                    PartnerBankAccountId = partnerMsbPrefixFind.PartnerBankAccountId,
                    Username = username,
                };
            }
            else
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreMsbPrefixAccountNotFound);
            }

            return null;
        }

        /// <summary>
        /// Kiểm tra chữ ký và trả về id ngân hàng thụ hưởng của đại lý
        /// </summary>
        private CheckSignatureDto CheckSignature(ReceiveNotificationDto notification, string cifCode, string prefixAccount, int tradingProviderId)
        {
            var cifCodeFind = _cifCodeEFRepository.FindByCifCode(cifCode);
            if (cifCodeFind == null)
            {
                _cifCodeEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound, cifCodeFind);
            }

            var username = _usersEFRepository.FindUserNameByInvestorId(cifCodeFind.InvestorId ?? 0);

            var tradingMSBPrefixFind = _tradingMSBPrefixAccountEFRepository.FindOne(prefixAccount, tradingProviderId);
            if (tradingMSBPrefixFind == null)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.CoreTradingMSBPrefixAccountNotFound);
            }

            // Kiểm tra chữ ký
            if (!EnvironmentNames.Develops.Contains(_env.EnvironmentName))
            {
                _msbCollectMoneyServices.HandleNotificationSignature(notification, tradingMSBPrefixFind.AccessCode);
            }

            return new CheckSignatureDto
            {
                TradingBankAccountId = tradingMSBPrefixFind.TradingBankAccountId,
                Username = username,
            };
        }

        private async Task<(long orderId, long orderPaymentId, int? tradingProviderId)> ReceiveNotificationGarnerAsync(ReceiveNotificationDto notification, string orderCode, string prefixAccount, DateTime tranDate, decimal tranAmount)
        {
            _logger.LogInformation($"{nameof(ReceiveNotificationGarnerAsync)}: notification = {JsonSerializer.Serialize(notification)}, orderCode = {orderCode}, prefixAccount = {prefixAccount}, tranDate = {tranDate}, tranAmount = {tranAmount}");
            long orderId = long.Parse(orderCode.Replace(ContractCodes.GARNER, ""));

            var orderFind = _garnerOrderEFRepository.FindById(orderId);
            if (orderFind == null)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderNotFound, orderId);
            }

            var resultCheck = CheckSignature(notification, orderFind.CifCode, prefixAccount, orderFind.TradingProviderId);

            // Nếu hợp đồng khác các trạng thái sau thì không thêm thông tin thanh toán
            if (!new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG }.Contains(orderFind.Status))
            {
                return (orderId, 0, orderFind.TradingProviderId);
            }

            var orderPaymentFirstQuery = _garnerOrderPaymentEFRepository.Entity.FirstOrDefault(o => o.OrderId == orderId && o.TranType == TranTypes.THU && o.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                            && o.TranClassify == TranClassifies.THANH_TOAN && o.Status != OrderPaymentStatus.HUY_THANH_TOAN && o.Deleted == YesNo.NO);
            if (orderPaymentFirstQuery == null)
            {
                orderFind.TradingBankAccId = resultCheck.TradingBankAccountId;
            }
            var insertPayment = _garnerOrderPaymentEFRepository.Add(new GarnerOrderPayment
            {
                OrderId = orderFind.Id,
                OrderNo = notification.TranSeq,
                TradingBankAccId = resultCheck.TradingBankAccountId,
                TranDate = tranDate,
                TranType = TranTypes.THU,
                TranClassify = TranClassifies.THANH_TOAN,
                PaymentType = PaymentTypes.CHUYEN_KHOAN,
                Status = OrderPaymentStatus.DA_THANH_TOAN,
                PaymentAmount = tranAmount,
                CreatedDate = DateTime.Now,
                ApproveBy = resultCheck.Username,
                ApproveDate = DateTime.Now,
                Description = $"{PaymentNotes.THANH_TOAN}{orderFind.ContractCode}",
            }, resultCheck.Username, orderFind.TradingProviderId);
            decimal totalPaymentValue = _garnerOrderPaymentEFRepository.SumPaymentAmount(orderFind.Id);

            if (new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG }.Contains(orderFind.Status)
                && totalPaymentValue + tranAmount >= orderFind.TotalValue)
            {
                orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;
            }
            _dbContext.SaveChanges();
            await _garnerNotificationServices.SendNotifyAdminCustomerPayment(insertPayment.Id);

            //Thông báo chuyển tiền thành công
            await _garnerNotificationServices.SendNotifyGarnerApprovePayment(insertPayment.Id);

            //active background
            _backgroundJobs.Enqueue(() => _paymentBackgroundJobServices.ActiveGarner(orderId));
            return (orderId, insertPayment.Id, orderFind.TradingProviderId);
        }

        private async Task<(long orderId, long orderPaymentId, int? tradingProviderId)> ReceiveNotificationRealEstatesAsync(ReceiveNotificationDto notification, string orderCode, string prefixAccount, DateTime tranDate, decimal tranAmount)
        {
            _logger.LogInformation($"{nameof(ReceiveNotificationRealEstatesAsync)}: notification = {JsonSerializer.Serialize(notification)}, orderCode = {orderCode}, prefixAccount = {prefixAccount}, tranDate = {tranDate}, tranAmount = {tranAmount}");
            int orderId = int.Parse(orderCode.Replace(ContractCodes.REAL_ESTATE, ""));

            var orderFind = _rstOrderEFRepository.FindById(orderId);
            if (orderFind == null)
            {
                _rstOrderEFRepository.ThrowException(ErrorCode.RstOrderNotFound, orderId);
            }

            var projectItem = _rstProductItemEFRepository.FindById(orderFind.ProductItemId)
                .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(orderFind.OpenSellDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var openSell = _rstOpenSellEFRepository.FindById(openSellDetail.OpenSellId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var projectFind = _rstProjectEFRepository.Entity.FirstOrDefault(p => p.Id == projectItem.ProjectId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            var productItem = _rstProductItemEFRepository.FindById(orderFind.ProductItemId)
                .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var cifCode = _dbContext.CifCodes.FirstOrDefault(o => o.CifCode == orderFind.CifCode);
            var investorFind = _dbContext.Investors.FirstOrDefault(o => o.InvestorId == cifCode.InvestorId);
            var phone = investorFind.Phone;

            var openSellDetailStatusInit = openSellDetail.Status;
            //var resultCheck = CheckSignature(notification, orderFind.CifCode, prefixAccount, orderFind.TradingProviderId);
            var resultCheck = CheckSignatureTradingPartner(notification, orderFind.CifCode, prefixAccount, orderFind.TradingProviderId, projectFind.PartnerId);

            // Nếu hợp đồng khác các trạng thái sau thì không thêm thông tin thanh toán
            if (!new int[] { RstOrderStatus.KHOI_TAO, RstOrderStatus.CHO_THANH_TOAN_COC, RstOrderStatus.CHO_DUYET_HOP_DONG_COC }.Contains(orderFind.Status))
            {
                return (orderId, 0, orderFind.TradingProviderId);
            }

            var insertPayment = _rstOrderPaymentEFRepository.Add(new RstOrderPayment
            {
                OrderId = orderFind.Id,
                OrderNo = notification.TranSeq,
                TradingProviderId = orderFind.TradingProviderId,
                TradingBankAccountId = resultCheck.TradingBankAccountId,
                PartnerBankAccountId = resultCheck.PartnerBankAccountId, //Hiện tại partner không có active tự động
                TranDate = tranDate,
                TranType = TranTypes.THU,
                TranClassify = TranClassifies.THANH_TOAN,
                PaymentType = PaymentTypes.CHUYEN_KHOAN,
                Status = OrderPaymentStatus.DA_THANH_TOAN,
                PaymentAmount = tranAmount,
                CreatedDate = DateTime.Now,
                ApproveBy = resultCheck.Username,
                ApproveDate = DateTime.Now,
                Description = $"{PaymentNotes.THANH_TOAN}{orderFind.ContractCode}",
            }, resultCheck.Username, orderFind.TradingProviderId);
            decimal totalPaymentValue = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(orderFind.Id);

            // Hợp đồng đang thuộc trong các trạng thái thì xử lý hợp đồng
            if (new int[] { RstOrderStatus.KHOI_TAO, RstOrderStatus.CHO_THANH_TOAN_COC, RstOrderStatus.CHO_DUYET_HOP_DONG_COC }.Contains(orderFind.Status))
            {
                var isFullPayment = false;
                if (totalPaymentValue + tranAmount >= orderFind.DepositMoney)
                {
                    orderFind.Status = RstOrderStatus.CHO_DUYET_HOP_DONG_COC;
                    isFullPayment = true;
                };

                // Kiểm tra đủ tiền thì khóa căn
                try
                {
                    // Lấy giá cọc, giá lock căn của hợp đồng
                    var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByPolicy(projectItem.Price ?? 0, orderFind.DistributionPolicyId);
                    // Tổng số tiền đã được duyệt
                    var sumOrderPaymentApprove = _rstOrderPaymentEFRepository.Entity.Where(p => p.OrderId == orderFind.Id && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN
                                                        && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN).Sum(p => p.PaymentAmount);

                    // Căn hộ chưa được khóa căn, nếu hợp đồng có đủ hoặc thừa số tiền giữ chỗ thì cho Khóa căn
                    if ((projectItem.Status == RstProductItemStatus.KHOI_TAO || projectItem.Status == RstProductItemStatus.GIU_CHO)
                        && sumOrderPaymentApprove + tranAmount >= productItemPrice.LockPrice)
                    {
                        projectItem.Status = RstProductItemStatus.KHOA_CAN;
                        openSellDetail.Status = RstProductItemStatus.KHOA_CAN;
                    }
                    // Trường hợp căn đã khóa thì check xem khóa căn có phải là do hợp đồng hay không
                    else if (projectItem.Status == RstProductItemStatus.KHOA_CAN && (openSellDetail.Status != RstProductItemStatus.KHOA_CAN
                        || (openSellDetail.Status == RstProductItemStatus.KHOA_CAN && sumOrderPaymentApprove < productItemPrice.LockPrice)))
                    {
                        _rstOrderPaymentEFRepository.ThrowException(ErrorCode.RstOrderPaymentCanNotApproveCuzProductItemLockNotOfOrder);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{nameof(ReceiveNotificationRealEstatesAsync)}: {ex.Message}");
                }

                // Lưu lại lịch sử thay đổi trạng thái căn hộ
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(productItem.Id, "Giữ chỗ", (isFullPayment) ? "Khoá căn" : "Giữ chỗ", "Trạng thái",
                            RstHistoryUpdateTables.RST_PRODUCT_ITEM, ActionTypes.CAP_NHAT, $"KH: {phone} thanh toán: {NumberToText.ConvertNumberIS((double)tranAmount)}", DateTime.Now, RstHistoryTypes.ThanhToan), phone);
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, "Giữ chỗ", (isFullPayment) ? "Khoá căn" : "Giữ chỗ", "Trạng thái",
                            RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, $"KH: {phone} thanh toán: {NumberToText.ConvertNumberIS((double)tranAmount)}", DateTime.Now), phone);
            }

            // Lưu lại lịch sử thanh toán
            _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(insertPayment.OrderId, null, "Thanh toán", null,
                        RstHistoryUpdateTables.RST_ORDER_PAYMENT, ActionTypes.CAP_NHAT, $"Phê duyệt thanh toán", DateTime.Now), phone);
            _dbContext.SaveChanges();

            // Duyệt/Hủy thanh toán thì đếm số căn hộ phát sinh thanh toán
            if (orderFind.ExpTimeDeposit > DateTime.Now && projectItem.Status == RstProductItemStatus.GIU_CHO)
            {
                await _rstSignalRBroadcastServices.BroadcatProductItemHasPaymentOrder(orderFind.ProductItemId, openSell.Id);
            }

            // Trường hợp thanh toán có thay đổi trạng thái sản phẩm của mở bán
            if (openSellDetailStatusInit != openSellDetail.Status)
            {
                await _rstSignalRBroadcastServices.BroadcastOpenSellDetailChangeStatus(openSellDetail.Id);
            }
            //Thông báo quản trị khách vào tiền
            await _rstNotificationServices.SendNotifyAdminCustomerPayment(insertPayment.Id);

            //Thông báo chuyển tiền thành công
            await _rstNotificationServices.SendNotifyRstApprovePayment(insertPayment.Id);
            //active background
            _backgroundJobs.Enqueue(() => _paymentBackgroundJobServices.ActiveRealEstate(orderId));
            return (orderId, insertPayment.Id, orderFind.TradingProviderId);
        }

        private async Task<(long orderId, long orderPaymentId, int? tradingProviderId)> ReceiveNotificationInvestAsync(ReceiveNotificationDto notification, string orderCode, string prefixAccount, DateTime tranDate, decimal tranAmount)
        {
            _logger.LogInformation($"{nameof(ReceiveNotificationInvestAsync)}: notification = {JsonSerializer.Serialize(notification)}, orderCode = {orderCode}, prefixAccount = {prefixAccount}, tranDate = {tranDate}, tranAmount = {tranAmount}");
            long orderId = long.Parse(orderCode.Replace(ContractCodes.INVEST, ""));

            var orderFind = _investOrderEFRepository.FindById(orderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound, orderId); 

            var policy = _dbContext.InvestPolicies.FirstOrDefault(p => p.Id == orderFind.PolicyId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound);

            var resultCheck = CheckSignature(notification, orderFind.CifCode, prefixAccount, orderFind.TradingProviderId);

            // Nếu hợp đồng khác các trạng thái sau thì không thêm thông tin thanh toán
            if (!new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG }.Contains(orderFind.Status))
            {
                return (orderId, 0, orderFind.TradingProviderId);
            }

            var orderPaymentFirstQuery = _investOrderPaymentEFRepository.Entity.OrderBy(o => o.TranDate).FirstOrDefault(o => o.OrderId == orderId && o.TranType == TranTypes.THU && o.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                            && o.TranClassify == TranClassifies.THANH_TOAN && o.PaymentType == PaymentTypes.CHUYEN_KHOAN && o.Status != OrderPaymentStatus.HUY_THANH_TOAN && o.Deleted == YesNo.NO);
            if (orderPaymentFirstQuery == null)
            {
                orderFind.BusinessCustomerBankAccId = resultCheck.TradingBankAccountId;
            }
            var genContractCode = _investOrderEFRepository.GetContractCodeGen(orderFind.Id);
            var insertPayment = _investOrderPaymentEFRepository.Add(new OrderPayment
            {
                OrderId = orderFind.Id,
                PaymentNo = notification.TranSeq,
                TradingBankAccId = resultCheck.TradingBankAccountId,
                TranDate = tranDate,
                TranType = TranTypes.THU,
                TranClassify = TranClassifies.THANH_TOAN,
                PaymentType = PaymentTypes.CHUYEN_KHOAN,
                Status = OrderPaymentStatus.DA_THANH_TOAN,
                PaymentAmnount = tranAmount,
                CreatedDate = DateTime.Now,
                ApproveBy = resultCheck.Username,
                ApproveDate = DateTime.Now,
                Description = $"{PaymentNotes.THANH_TOAN}{genContractCode ?? orderFind.ContractCode}",
                CreatedBy = resultCheck.Username,
                TradingProviderId = orderFind.TradingProviderId
            });
            decimal totalPaymentValue = _investOrderPaymentEFRepository.SumPaymentAmount(orderFind.Id);

            if (new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG }.Contains(orderFind.Status)
                && totalPaymentValue + tranAmount >= orderFind.TotalValue)
            {
                orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                orderFind.PaymentFullDate = (orderPaymentFirstQuery == null) ? tranDate : orderPaymentFirstQuery.TranDate;

            }
            _dbContext.SaveChanges();

            // Hạn mức tối đa của phân phối
            var maxTotalInvestment = _invOrderRepository.MaxTotalInvestment(orderFind.TradingProviderId, orderFind.DistributionId);
            // Ghi lại log khi duyệt thanh toán
            _logger.LogInformation($"Phê duyệt thanh toán hợp đồng {orderFind.ContractCode} Ngày {DateTime.Now:dd-MM-yyyy}. Hạn mức còn lại là: {maxTotalInvestment}");
            if (maxTotalInvestment <= 0)
            {
                var distributionFind = _dbContext.InvestDistributions.FirstOrDefault(d => d.Id == orderFind.DistributionId && d.Deleted == YesNo.NO && d.TradingProviderId == orderFind.TradingProviderId);
                distributionFind.IsShowApp = YesNo.NO;
                _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(orderFind.DistributionId, YesNo.YES, YesNo.NO, InvestFieldName.UPDATE_DISTRIBUTION_IS_SHOW_APP, InvestHistoryUpdateTables.INV_DISTRIBUTION,
                    ActionTypes.CAP_NHAT, $"Hệ thống tắt show app khi thu tự động hợp đồng {orderFind.ContractCode}", DateTime.Now), resultCheck.Username);
            }
            _dbContext.SaveChanges();
            await _investNotificationServices.SendNotifyAdminCustomerPayment(insertPayment.Id);

            // thông báo chuyển tiền thành công
            await _investNotificationServices.SendEmailInvestApprovePayment(insertPayment.Id);
            
            // Chính sách đang kích hoạt mới được Active hợp đồng
            if (policy.Status == Status.ACTIVE)
            {
                //active background
                _backgroundJobs.Enqueue(() => _paymentBackgroundJobServices.ActiveInvest(orderId));
            }    
            return (orderId, insertPayment.Id, orderFind.TradingProviderId);
        }

        private async Task<(int orderId, long orderPaymentId, int? tradingProviderId)> ReceiveNotificationEventAsync(ReceiveNotificationDto notification, string orderCode, string prefixAccount, DateTime tranDate, decimal tranAmount)
        {
            _logger.LogInformation($"{nameof(ReceiveNotificationEventAsync)}: notification = {JsonSerializer.Serialize(notification)}, orderCode = {orderCode}, prefixAccount = {prefixAccount}, tranDate = {tranDate}, tranAmount = {tranAmount}");
            int orderId = int.Parse(orderCode.Replace(ContractCodes.EVENT, ""));

            var orderFind = _dbContext.EvtOrders.FirstOrDefault(ed => ed.Id == orderId && ed.Deleted == YesNo.NO && ed.ExpiredTime >= DateTime.Now).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound, orderId);
            var eventDetailFind = _dbContext.EvtEventDetails.FirstOrDefault(ed => ed.Id == orderFind.EventDetailId && ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventDetailFind.EventId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            foreach (var item in orderFind.OrderDetails)
            {
                var ticketFind = _dbContext.EvtTickets.FirstOrDefault(t => t.Id == item.TicketId && t.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
                var soldTicket = _dbContext.EvtOrderDetails.Include(ed => ed.Order)
                                                            .Where(od => (od.Order.Status == EvtOrderStatus.HOP_LE || (od.Order.Status == EvtOrderStatus.CHO_THANH_TOAN && od.Id != item.OrderId))
                                                                    && od.TicketId == item.TicketId)
                                                            .Sum(t => t.Quantity);
                if (ticketFind.Quantity - soldTicket < item.Quantity)
                {
                    return (orderId, 0, orderFind.TradingProviderId);
                }
            }

            var investorFind = _dbContext.Investors.FirstOrDefault(o => o.InvestorId == orderFind.InvestorId && o.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
            var identifyFind = _dbContext.InvestorIdentifications.FirstOrDefault(o => o.Id == orderFind.InvestorIdenId && o.InvestorId == orderFind.InvestorId && o.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);
            var cifCodeFind = _dbContext.CifCodes.FirstOrDefault(cc => cc.InvestorId == orderFind.InvestorId && cc.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            var resultCheck = CheckSignature(notification, cifCodeFind.CifCode, prefixAccount, orderFind.TradingProviderId);

            // Nếu hợp đồng khác các trạng thái sau thì không thêm thông tin thanh toán
            if (!new int[] { EvtOrderStatus.KHOI_TAO, EvtOrderStatus.CHO_THANH_TOAN }.Contains(orderFind.Status))
            {
                return (orderId, 0, orderFind.TradingProviderId);
            }

            int paymentId = (int)_evtOrderPaymentEFRepository.NextKey();
            var insertPayment = _dbContext.EvtOrderPayments.Add(new EvtOrderPayment
            {
                Id = paymentId,
                OrderId = orderFind.Id,
                OrderNo = notification.TranSeq,
                TradingProviderId = orderFind.TradingProviderId,
                TradingBankAccountId = resultCheck.TradingBankAccountId,
                TranDate = tranDate,
                TranClassify = TranClassifies.THANH_TOAN,
                PaymentType = PaymentTypes.CHUYEN_KHOAN,
                Status = OrderPaymentStatus.DA_THANH_TOAN,
                PaymentAmount = tranAmount,
                CreatedDate = DateTime.Now,
                CreatedBy = resultCheck.Username,
                ApproveBy = resultCheck.Username,
                ApproveDate = DateTime.Now,
                Description = $"{PaymentNotes.THANH_TOAN}{orderFind.ContractCode}",
            });

            decimal totalPaymentValue = _dbContext.OrderPayments.Where(o => o.OrderId == orderFind.Id
                                                                            && o.TranType == TranTypes.THU
                                                                            && o.TranClassify == TranClassifies.THANH_TOAN
                                                                            && o.Deleted == YesNo.NO && o.Status == OrderPaymentStatus.DA_THANH_TOAN)
                                                                 .Sum(o => o.PaymentAmnount) ?? 0;
            decimal totalMoney = _dbContext.EvtOrderDetails
                         .Where(od => od.OrderId == orderFind.Id)
                         .Sum(od => od.Quantity * od.Price);

            if (new int[] { EvtOrderStatus.KHOI_TAO, EvtOrderStatus.CHO_THANH_TOAN }.Contains(orderFind.Status)
                && totalPaymentValue + tranAmount >= totalMoney)
            {
                orderFind.Status = EvtOrderStatus.CHO_XU_LY;
            }
            _dbContext.SaveChanges();

            // Ghi lại log khi duyệt thanh toán
            _logger.LogInformation($"Phê duyệt thanh toán hợp đồng {orderFind.ContractCode} Ngày {DateTime.Now:dd-MM-yyyy}");

            await _eventNotificationServices.SendNotifyApprovePaymentOrder(insertPayment.Entity.Id);
            await _eventNotificationServices.SendAdminNotifyApprovePaymentOrder(insertPayment.Entity.Id);
            await _evtSignalRBroadcastService.BroadcastOrderPaymentActive(paymentId);

            //active background
            _backgroundJobs.Enqueue(() => _paymentBackgroundJobServices.ActiveEvent(orderId));
            return (orderId, paymentId, orderFind.TradingProviderId);
        }

        /// <summary>
        /// Lưu notification vào trước và xử lý sau
        /// </summary>
        /// <param name="input"></param>
        public async Task ReceiveNotificationAsync(ReceiveNotificationDto input)
        {
            _logger.LogInformation($"{nameof(ReceiveNotificationAsync)}: input = {JsonSerializer.Serialize(input)}");
            string requestIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            var msbNotify = _mapper.Map<MsbNotification>(input);
            msbNotify.Ip = requestIp;
            msbNotify.Status = MsbNotificationStatus.FAIL;
            // Kiểm tra tranSeq có bị lặp
            var msbDupNotiFind = _msbPaymentRepository.FindByTranSeq(input.TranSeq);
            if (msbDupNotiFind != null)
            {
                msbNotify.Status = MsbNotificationStatus.DUPLICATE;
            }

            // Ngày yêu cầu được translated từ TranDate
            DateTime transferDate;
            if (DateTime.TryParseExact(input.TranDate, "yyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out transferDate))
            {
                msbNotify.TransferDate = transferDate;
            }

            msbNotify = _msbPaymentRepository.Add(msbNotify);
            _dbContext.SaveChanges();
            if (msbNotify.Status == MsbNotificationStatus.DUPLICATE)
            {
                return;
            }
            int? tradingProviderId = null;
            try
            {
                //xử lý nhưng chưa kiểm tra chữ ký, sau khi tìm ra order id -> tradingProviderId -> prefixAccount -> kiểm tra chữ ký
                ResultNotificationDto resultNotify = _msbCollectMoneyServices.HandleNotification(input);
                if (resultNotify.OrderCode.StartsWith(ContractCodes.INVEST))
                {
                    msbNotify.ProjectType = ContractCodes.INVEST;
                    msbNotify.ReferId = long.Parse(resultNotify.OrderCode.Replace(ContractCodes.INVEST, ""));
                    var result = await ReceiveNotificationInvestAsync(input, resultNotify.OrderCode, resultNotify.PrefixAccount, resultNotify.TranDate, resultNotify.TranAmount);
                    tradingProviderId = result.tradingProviderId;
                }
                else if (resultNotify.OrderCode.StartsWith(ContractCodes.GARNER))
                {
                    msbNotify.ProjectType = ContractCodes.GARNER;
                    msbNotify.ReferId = long.Parse(resultNotify.OrderCode.Replace(ContractCodes.GARNER, ""));
                    var result = await ReceiveNotificationGarnerAsync(input, resultNotify.OrderCode, resultNotify.PrefixAccount, resultNotify.TranDate, resultNotify.TranAmount);
                    tradingProviderId = result.tradingProviderId;
                }
                else if (resultNotify.OrderCode.StartsWith(ContractCodes.REAL_ESTATE))
                {
                    msbNotify.ProjectType = ContractCodes.REAL_ESTATE;
                    msbNotify.ReferId = long.Parse(resultNotify.OrderCode.Replace(ContractCodes.REAL_ESTATE, ""));
                    var result = await ReceiveNotificationRealEstatesAsync(input, resultNotify.OrderCode, resultNotify.PrefixAccount, resultNotify.TranDate, resultNotify.TranAmount);
                    tradingProviderId = result.tradingProviderId;
                }
                else if (resultNotify.OrderCode.StartsWith(ContractCodes.EVENT))
                {
                    msbNotify.ProjectType = ContractCodes.EVENT;
                    msbNotify.ReferId = long.Parse(resultNotify.OrderCode.Replace(ContractCodes.EVENT, ""));
                    var result = await ReceiveNotificationEventAsync(input, resultNotify.OrderCode, resultNotify.PrefixAccount, resultNotify.TranDate, resultNotify.TranAmount);
                    tradingProviderId = result.tradingProviderId;
                }
                msbNotify.TradingProviderId = tradingProviderId;
                msbNotify.Status = MsbNotificationStatus.SUCCESS;
                msbNotify.Exception = null;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // xử lý nếu xảy ra ngoại lệ lưu vào trường exception của notification vừa lưu
                _logger.LogError(ex, $"{nameof(ReceiveNotificationAsync)}");
                msbNotify.Exception = ex.Message;
                _msbPaymentRepository.Update(msbNotify);
                _dbContext.SaveChanges();
            }
        }

        public async Task ReceiveNotificationPayment(ReceiveNotificationPaymentDto input)
        {
            _logger.LogInformation($"{nameof(ReceiveNotificationPayment)}: input = {JsonSerializer.Serialize(input)}");
            string requestIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            var entity = _mapper.Map<MsbNotificationPayment>(input);
            entity.Ip = requestIp;
            entity.HandleStatus = MsbNotificationStatus.FAIL;
            var msbDupNotiFind = _msbNotificationPaymentRepository.Entity.FirstOrDefault(n => n.TransId == input.TransId);
            if (msbDupNotiFind != null)
            {
                entity.HandleStatus = MsbNotificationStatus.DUPLICATE;
            }
            entity = _msbNotificationPaymentRepository.Add(entity);
            _dbContext.SaveChanges();
            if (entity.HandleStatus == MsbNotificationStatus.DUPLICATE)
            {
                return;
            }
            try
            {
                string transIdStr = input.TransId?.Replace(MsbPrefixRequestId.Prefix, "");
                if (!long.TryParse(transIdStr, out long transId))
                {
                    throw new Exception($"Không parse transId = {transIdStr}, transIdStr = {transIdStr}");
                }
                var findRequestPaymentDetail = _msbRequestPaymentDetailEFRepository.FindById(transId)
                                    .ThrowIfNull(_dbContext, ErrorCode.PaymentRequestDetailNotFound);

                var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == findRequestPaymentDetail.TradingBankAccId &&
                                                                                           e.TId == input.TId.ToString() && e.MId == input.MId.ToString() && e.Deleted == YesNo.NO)
                   .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);

                //kiểm tra chữ ký
                if (!EnvironmentNames.Develops.Contains(_env.EnvironmentName))
                {
                    _msbPayMoneyServices.HandleNotificationPayment(input, prefixAccount.AccessCode);
                }

                // Trạng thái từ Bank đổ về nếu thành công
                if (input.Status == MsbPayStatus.Success)
                {
                    await UpdateStatusBankByReferId(findRequestPaymentDetail.DataType, findRequestPaymentDetail.ReferId, MsbNotifyPaymentStatus.SUCCESS, findRequestPaymentDetail.TradingBankAccId);
                    findRequestPaymentDetail.Status = MsbRequestDetailStatus.SUCCESS;
                }
                // Nếu thất bại
                else
                {
                    // Nếu trạng thái từ bank lỗi, cho phép yêu cầu từ bảng Refer được thực hiện lại yêu cầu
                    await UpdateStatusBankByReferId(findRequestPaymentDetail.DataType, findRequestPaymentDetail.ReferId, MsbNotifyPaymentStatus.FAIL);
                    findRequestPaymentDetail.Status = MsbRequestDetailStatus.FAIL;
                }

                // Nếu các xử lý ở trên thành công thì lưu lại
                entity.HandleStatus = MsbNotificationStatus.SUCCESS;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // xử lý nếu xảy ra ngoại lệ lưu vào trường exception của notification vừa lưu
                _logger.LogError(ex, $"{nameof(ReceiveNotificationPayment)}");
                entity.Exception = ex.Message;
                _msbNotificationPaymentRepository.Update(entity);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật trạng thái của ngân hàng vào bảng Refer của yêu cầu
        /// </summary>
        public async Task UpdateStatusBankByReferId(int dataType, long referId, int bankStatus, int? tradingBankAccId = null)
        {
            if (dataType == RequestPaymentDataTypes.GAN_WITHDRAWAL)
            {
                var ganWithdrawal = _garnerWithdrawalEFRepository.FindById(referId).ThrowIfNull(_dbContext, ErrorCode.GarnerWithdrawalNotFound);

                //Nếu trạng thái từ bank lỗi, cho phép yêu cầu rút được thực hiện lại
                if (bankStatus == MsbNotifyPaymentStatus.FAIL)
                {
                    ganWithdrawal.StatusBank = MsbBankStatus.FAIL;
                }
                else if (bankStatus == MsbNotifyPaymentStatus.SUCCESS)
                {
                    var details = _garnerWithdrawalDetailEFRepository.FindByWithdrawalId(referId);

                    foreach (var detail in details)
                    {
                        var order = _garnerOrderEFRepository.FindById(detail.OrderId)
                                .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
                        if (order.TotalValue == detail.AmountMoney)
                        {
                            order.TotalValue = 0;
                            order.Status = OrderStatus.TAT_TOAN;
                            order.SettlementDate = ganWithdrawal.ApproveDate;
                        }
                        else if (order.TotalValue > detail.AmountMoney)
                        {
                            order.TotalValue -= detail.AmountMoney;
                        }
                        //Thêm chi (BondOrderPayment)
                        var orderPaymentSpend = new GarnerOrderPayment()
                        {
                            TradingProviderId = ganWithdrawal.TradingProviderId,
                            OrderId = order.Id,
                            TranDate = ganWithdrawal.ApproveDate,
                            TranType = TranTypes.CHI,
                            TranClassify = TranClassifies.RUT_VON,
                            PaymentType = PaymentTypes.CHUYEN_KHOAN,
                            PaymentAmount = detail.AmountMoney,
                            Status = OrderPaymentStatus.DA_THANH_TOAN,
                            CreatedBy = ganWithdrawal.ApproveBy,
                            CreatedDate = DateTime.Now,
                            ApproveBy = ganWithdrawal.ApproveBy,
                            ApproveDate = ganWithdrawal.ApproveDate,
                            TradingBankAccId = tradingBankAccId,
                            Description = PaymentNotes.CHI_RUT_VON + order.ContractCode
                        };
                        _garnerOrderPaymentEFRepository.Add(orderPaymentSpend, ganWithdrawal.ApproveBy, ganWithdrawal.TradingProviderId);
                    }
                    ganWithdrawal.Status = WithdrawalStatus.DUYET_DI_TIEN;
                    ganWithdrawal.StatusBank = MsbBankStatus.SUCCESS;
                    _dbContext.SaveChanges();
                    await _garnerNotificationServices.SendNotifyGarnerOrderWithdraw(referId);
                }
            }

            if (dataType == RequestPaymentDataTypes.GAN_INTEREST_PAYMENT)
            {
                var ganInterestPayment = _garnerInterestPaymentEFRepository.FindById(referId).ThrowIfNull(_dbContext, ErrorCode.GarnerInterestPaymentNotFound);

                //Nếu trạng thái từ bank lỗi, cho phép yêu cầu rút được thực hiện lại
                if (bankStatus == MsbNotifyPaymentStatus.FAIL)
                {
                    ganInterestPayment.StatusBank = MsbBankStatus.FAIL;
                }
                else if (bankStatus == MsbNotifyPaymentStatus.SUCCESS)
                {
                    var details = _garnerInterestPaymentDetailEFRepository.GetAllByInterestPaymentId(referId);

                    foreach (var detail in details)
                    {
                        var order = _garnerOrderEFRepository.FindById(detail.OrderId)
                                .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
                        //Thêm chi (BondOrderPayment)
                        var orderPaymentSpend = new GarnerOrderPayment()
                        {
                            TradingProviderId = ganInterestPayment.TradingProviderId,
                            OrderId = order.Id,
                            TranDate = ganInterestPayment.PayDate,
                            TranType = TranTypes.CHI,
                            TranClassify = TranClassifies.CHI_TRA_LOI_TUC,
                            PaymentType = PaymentTypes.CHUYEN_KHOAN,
                            PaymentAmount = detail.AmountReceived,
                            Status = OrderPaymentStatus.DA_THANH_TOAN,
                            CreatedBy = ganInterestPayment.ApproveBy,
                            CreatedDate = DateTime.Now,
                            ApproveBy = ganInterestPayment.ApproveBy,
                            ApproveDate = ganInterestPayment.ApproveDate,
                            TradingBankAccId = tradingBankAccId,
                            Description = PaymentNotes.CHI_LOI_NHUAN + order.ContractCode
                        };
                        _garnerOrderPaymentEFRepository.Add(orderPaymentSpend, ganInterestPayment.ApproveBy, ganInterestPayment.TradingProviderId);
                    }
                    ganInterestPayment.Status = InterestPaymentStatus.DA_DUYET_CHI_TIEN;
                    ganInterestPayment.StatusBank = MsbBankStatus.SUCCESS;
                    _dbContext.SaveChanges();
                    await _garnerNotificationServices.SendNotifyGarnerInterestPayment(referId);
                }
            }

            else if (dataType == RequestPaymentDataTypes.EP_INV_INTEREST_PAYMENT)
            {
                var investInterestPayment = _investInterestPaymentEFRepository.Entity.FirstOrDefault(i => i.Id == referId && i.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestInterestPaymentNotFound);

                //Nếu trạng thái từ bank lỗi, cho phép yêu cầu rút được thực hiện lại
                if (bankStatus == MsbNotifyPaymentStatus.FAIL)
                {
                    investInterestPayment.StatusBank = MsbBankStatus.FAIL;
                }
                else if (bankStatus == MsbNotifyPaymentStatus.SUCCESS)
                {
                    List<InvestInterestPayment> investInterestPayments = new();
                    investInterestPayments.Add(investInterestPayment);
                    var orderFind = _invOrderEFRepository.FindById(investInterestPayment.OrderId).ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
                    var renewalsRequestQuery = _invRenewalsRequestRepository.GetByOrderId(investInterestPayment.OrderId);
                    if (renewalsRequestQuery == null)
                    {
                        _investInterestPaymentServices.InterestPayment(investInterestPayments, investInterestPayment.TradingProviderId ?? 0, tradingBankAccId, InterestPaymentStatus.DA_DUYET_CHI_TIEN, investInterestPayment.ApproveBy, null, bankStatus);
                    }
                    else
                    {
                        await _investInterestPaymentServices.RenewalInterestPayment(investInterestPayments, investInterestPayment.TradingProviderId ?? 0, tradingBankAccId, InterestPaymentStatus.DA_DUYET_CHI_TIEN, investInterestPayment.ApproveBy, null, bankStatus);

                    }
                    investInterestPayment.Status = InterestPaymentStatus.DA_DUYET_CHI_TIEN;
                    investInterestPayment.StatusBank = MsbBankStatus.SUCCESS;

                    if (investInterestPayment.IsLastPeriod == YesNo.NO)
                    {
                        await _investNotificationServices.SendEmailInvestInterestPaymentSuccess(investInterestPayment.Id);
                    }
                    else if (investInterestPayment.IsLastPeriod == YesNo.YES)
                    {
                        await _investNotificationServices.SendEmailInvestInterestPaymentSettlementSuccess(investInterestPayment.Id);
                    }
                }
            }

            else if (dataType == RequestPaymentDataTypes.EP_INV_WITHDRAWAL)
            {
                var investWithdrawal = _investWithdrawalEFRepository.Entity.FirstOrDefault(i => i.Id == referId && i.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound);

                //Nếu trạng thái từ bank lỗi, cho phép yêu cầu rút được thực hiện lại
                if (bankStatus == MsbNotifyPaymentStatus.FAIL)
                {
                    investWithdrawal.StatusBank = MsbBankStatus.FAIL;
                }
                else if (bankStatus == MsbNotifyPaymentStatus.SUCCESS)
                {
                    investWithdrawal.Status = WithdrawalStatus.DUYET_DI_TIEN;
                    investWithdrawal.StatusBank = MsbBankStatus.SUCCESS;

                    var orderFind = _invOrderEFRepository.FindById(investWithdrawal.OrderId ?? 0).ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
                    if (investWithdrawal.Type == WithdrawalTypes.RUT_VON)
                    {
                        if (orderFind.TotalValue - investWithdrawal.AmountMoney < 0)
                        {
                            _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestWithdrawalApproveTooLarge);
                        }
                        orderFind.TotalValue = orderFind.TotalValue - (investWithdrawal.AmountMoney ?? 0);
                    }
                    else if (investWithdrawal.Type == WithdrawalTypes.TAT_TOAN)
                    {
                        orderFind.TotalValue = 0;
                        orderFind.Status = OrderStatus.TAT_TOAN;
                        orderFind.SettlementDate = investWithdrawal.WithdrawalDate;
                    }

                    var genContractCode = _investOrderEFRepository.GetContractCodeGen(orderFind.Id);
                    _invOrderPaymentEFRepository.Add(new OrderPayment
                    {
                        TradingProviderId = orderFind.TradingProviderId,
                        OrderId = orderFind.Id,
                        TranDate = DateTime.Now,
                        TranType = TranTypes.CHI,
                        TranClassify = TranClassifies.RUT_VON,
                        PaymentType = PaymentTypes.CHUYEN_KHOAN,
                        Status = OrderPaymentStatus.DA_THANH_TOAN,
                        PaymentAmnount = investWithdrawal.ActuallyAmount,
                        Description = PaymentNotes.CHI_RUT_VON + (genContractCode ?? orderFind.ContractCode),
                        CreatedBy = investWithdrawal.ApproveBy,
                        CreatedDate = DateTime.Now,
                        ApproveDate = investWithdrawal.ApproveDate,
                        ApproveBy = investWithdrawal.ApproveBy,
                        TradingBankAccId = tradingBankAccId ?? 0,
                    });

                    if (investWithdrawal.Type == WithdrawalTypes.RUT_VON)
                    {
                        await _investNotificationServices.SendEmailInvestWithdrawalSuccess(investWithdrawal.Id);
                    }
                    else if (investWithdrawal.Type == WithdrawalTypes.TAT_TOAN)
                    {
                        await _investNotificationServices.SendEmailInvestPrePayment(investWithdrawal.Id);
                    }
                }
            }
        }

        public PagingResult<ViewMsbCollectionPaymentDto> FindAllCollectionPayment(MsbCollectionPaymentFilterDto input)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUsername(_httpContext);
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            var queryCollectionPaymenty = _msbPaymentRepository.FindAllCollectionPayment(input, tradingProviderId);
            var result = new PagingResult<ViewMsbCollectionPaymentDto>
            {
                Items = queryCollectionPaymenty.Items
            };
            foreach (var item in result.Items)
            {
                long orderId = item.ReferId ?? 0;

                // Lấy thông tin order 
                if (item.ProjectType == ContractCodes.GARNER)
                {
                    var order = _garnerOrderEFRepository.FindById(orderId);
                    if (order != null)
                    {
                        item.GarnerOrder = _mapper.Map<GarnerOrderDto>(order);
                    }
                }
                else if (item.ProjectType == ContractCodes.INVEST)
                {
                    var order = _invOrderRepository.FindById(orderId);
                    if (order != null)
                    {
                        item.InvestOrder = _mapper.Map<ViewOrderDto>(order); ;
                    }
                }
                else if (item.ProjectType == ContractCodes.REAL_ESTATE)
                {
                    var order = _rstOrderEFRepository.FindById((int)orderId);
                    if (order != null)
                    {
                        item.RstOrder = _mapper.Map<RstOrderDto>(order);
                    }
                }
                else if (item.ProjectType == ContractCodes.EVENT)
                {
                    var order = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == (int)orderId);
                    if (order != null)
                    {
                        item.EvtOrder = _mapper.Map<EvtOrderDto>(order);
                    }
                }
            }
            result.Items = result.Items;
            result.TotalItems = queryCollectionPaymenty.TotalItems;
            return result;
        }

        public async Task<ExportResultDto> GetAllInvestorBankAccountList(FilterInvestorBankAccountDto input)
        {
            var result = new ExportResultDto();
            var investorBankAccountList = new List<InvestorBankAccountMsbDto>();
            var investor = _investorRepository.GetAll().OrderBy(o => o.InvestorId).Skip(input.Skip).Take(input.PageSize);
            //var investor = _investorEFRepository.EntityNoTracking.Where(o => input.InvestorIds.Contains(o.InvestorId));


            foreach (var item in investor)
            {
                var investorBankAccountFind = new InvestorBankAccountMsbDto();
                var investorIdentification = _managerInvestorRepository.GetDefaultIdentification(item.InvestorId, false);
                var investorBankAccount = _investorBankAccountEFRepository.FindByInvestorId(item.InvestorId);
                investorBankAccountFind.InvestorId = item.InvestorId;
                if (investorIdentification == null)
                {
                    continue;
                }
                investorBankAccountFind.Name = investorIdentification.Fullname;

                investorBankAccountFind.Phone = item.Phone;

                if (investorBankAccount == null)
                {
                    continue;
                }
                investorBankAccountFind.BankAccount = investorBankAccount.BankAccount;
                var bank = _bankEFRepository.FindById(investorBankAccount.BankId);

                if (bank == null)
                {
                    continue;
                }

                investorBankAccountFind.BankName = bank.BankName;
                if (string.IsNullOrWhiteSpace(bank.Bin))
                {
                    investorBankAccountFind.Reason = $"Chưa cấu hình BIN cho ngân hàng {bank.BankCode}";
                }

                try
                {
                    await _msbInquiryServices.InquiryAccount(new InquiryAccountDto
                    {
                        BankAccount = investorBankAccount.BankAccount,
                        Bin = bank.Bin,
                    });
                }
                catch (Exception ex)
                {

                    investorBankAccountFind.Reason = ex.Message;
                    investorBankAccountList.Add(investorBankAccountFind);
                }

            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Payment");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Họ tên";
            worksheet.Cell(currentRow, 3).Value = "InvestorId";
            worksheet.Cell(currentRow, 4).Value = "Số điện thoại";
            worksheet.Cell(currentRow, 5).Value = "Tên ngân hàng";
            worksheet.Cell(currentRow, 6).Value = "Số tài khoản";
            worksheet.Cell(currentRow, 7).Value = "Lý do";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 20;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 60;
            worksheet.Column("G").Width = 60;

            foreach (var item in investorBankAccountList)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item.Name;
                worksheet.Cell(currentRow, 3).Value = "'" + item.InvestorId;
                worksheet.Cell(currentRow, 4).Value = "'" + item.Phone;
                worksheet.Cell(currentRow, 5).Value = item.BankName;
                worksheet.Cell(currentRow, 6).Value = "'" + item.BankAccount;
                worksheet.Cell(currentRow, 7).Value = item.Reason;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }
    }
}

