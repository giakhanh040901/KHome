using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.GarnerRepositories;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.ConstantVariables.Shared.Bank;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtAppOrderService : IEvtAppOrderService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtAppOrderService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;
        private readonly IEvtOrderCommonService _evtOrderCommonService;
        private readonly EvtOrderEFRepository _evtOrderEFRepository;
        private readonly EvtOrderDetailEFRepository _evtOrderDetailEFRepository;
        private readonly EvtOrderTicketDetailEFRepository _evtOrderTicketDetailEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly EvtEventBankAccountEFRepository _evtEventBankAccountEFRepository;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;
        private readonly MsbCollectMoneyServices _msbCollectMoneyServices;
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly IEvtSignalRBroadcastService _evtSignalRBroadcastService;
        private readonly IEvtOrderTicketFillService _orderTicketFillService;
        private readonly IBackgroundJobClient _backgroundJobs;

        public EvtAppOrderService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtAppOrderService> logger,
            IHttpContextAccessor httpContextAccessor,
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices,
            IEvtOrderCommonService evtOrderCommonService,
            MsbCollectMoneyServices msbCollectMoneyServices,
            EventNotificationServices eventNotificationServices,
            IEvtSignalRBroadcastService evtSignalRBroadcastService,
            IEvtOrderTicketFillService orderTicketFillService,
            IBackgroundJobClient backgroundJobs)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
            _evtOrderCommonService = evtOrderCommonService;
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _evtOrderEFRepository = new EvtOrderEFRepository(_dbContext, _logger);
            _evtOrderDetailEFRepository = new EvtOrderDetailEFRepository(_dbContext, _logger);
            _evtOrderTicketDetailEFRepository = new EvtOrderTicketDetailEFRepository(_dbContext, _logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _evtEventBankAccountEFRepository = new EvtEventBankAccountEFRepository(dbContext, logger);
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, logger);
            _msbCollectMoneyServices = msbCollectMoneyServices;
            _eventNotificationServices = eventNotificationServices;
            _evtSignalRBroadcastService = evtSignalRBroadcastService;
            _orderTicketFillService = orderTicketFillService;
            _backgroundJobs = backgroundJobs;
        }

        private List<int> GetListTradingProviderIds(bool isSaleView)
        {
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }
            var listTradingProviderIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId, isSaleView);
            return listTradingProviderIds;
        }

        /// <summary>
        /// danh sách event Detail lúc đặt lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSaleView"></param>
        /// <returns></returns>
        public IEnumerable<AppEventDetailDto> FindEventDetailsById(int id, bool isSaleView)
        {
            _logger.LogInformation($"{nameof(FindEventDetailsById)} : id = {id}, isSaleView = {isSaleView}");

            var listTradingProviderIds = GetListTradingProviderIds(isSaleView);
            var evtEvent = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == id && e.IsShowApp == YesNo.YES && e.Deleted == YesNo.NO && (listTradingProviderIds.Count == 0 || listTradingProviderIds.Contains(e.TradingProviderId))).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);

            var query = _dbContext.EvtEventDetails.Include(e => e.Event)
                                        .Include(e => e.Tickets)
                                        .Include(e => e.Orders).ThenInclude(ed => ed.OrderDetails)
                                        .Where(e => e.Deleted == YesNo.NO && e.EndDate >= DateTime.Now
                                                        && e.Event.Id == id)
                                        .Select(e => new AppEventDetailDto
                                        {
                                            Id = e.Id,
                                            StartDate = e.StartDate,
                                            EndDate = e.EndDate,
                                            IsShowRemaingTicketApp = e.IsShowRemaingTicketApp,
                                            Tickets = e.Tickets.Where(t => t.Deleted == YesNo.NO
                                                                        && t.StartSellDate <= DateTime.Now
                                                                        && t.EndSellDate >= DateTime.Now
                                                                        && t.Status == EvtTicketStatus.KICH_HOAT
                                                                        && t.IsShowApp == YesNo.YES)
                                                    .OrderBy(ti => ti.StartSellDate)
                                                    .Select(t => new AppTicketInfoDto
                                                    {
                                                        Id = t.Id,
                                                        EventDetailId = t.EventDetailId,
                                                        Name = t.Name,
                                                        IsFree = t.IsFree,
                                                        Price = t.Price,
                                                        Quantity = t.Quantity,
                                                        MinBuy = t.MinBuy,
                                                        MaxBuy = t.MaxBuy,
                                                        StartSellDate = t.StartSellDate,
                                                        EndSellDate = t.EndSellDate,
                                                        Description = t.Description,
                                                        RemainingTickets = t.Quantity - e.Orders.Where(o =>
                                                                               (o.Status == EvtOrderStatus.HOP_LE || (o.Status == EvtOrderStatus.CHO_THANH_TOAN && o.ExpiredTime >= DateTime.Now))
                                                                               && o.EventDetailId == t.EventDetailId)
                                                                              .SelectMany(o => o.OrderDetails).Where(od => od.TicketId == t.Id).Select(od => od.Quantity)
                                                                              .Sum(),
                                                        IsShowRemaingTicketApp = t.EventDetail.IsShowRemaingTicketApp,
                                                    })
                                        });
            return query;
        }

        public async Task<int> Add(AppCreateEvtOrderDto input, bool isSelfDoing, int? investorIdSaleOrder)
        {
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, isSelfDoing = {isSelfDoing}, investorIdSaleOrder = {investorIdSaleOrder}");
            int? investorId;
            if (investorIdSaleOrder != null)
            {
                investorId = investorIdSaleOrder;
            }
            else
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            // kiểm tra thông tin
            var eventDetail = _dbContext.EvtEventDetails.FirstOrDefault(e => e.Id == input.EventDetailId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventDetail.EventId && e.Deleted == YesNo.NO);
            var investorIdentification = _dbContext.InvestorIdentifications.FirstOrDefault(e => e.Id == input.InvestorIdenId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);
          
            if (input.ContractAddressId != null && input.IsReceiveHardTicket && !(_dbContext.InvestorContactAddresses.Where(c => c.Deleted == YesNo.NO && c.ContactAddressId == input.ContractAddressId && c.InvestorId == investorIdentification.InvestorId).Any()))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.InvestorContractNotFound);
            }

            if (input.IsReceiveHardTicket && !eventFind.CanExportTicket)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderReceiveHardTicketNotFound);
            }
            if (input.IsRequestReceiveRecipt && !eventFind.CanExportRequestRecipt)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderRequestReceiveReciptNotFound);
            }

            var transaction = _dbContext.Database.BeginTransaction();

            var orderId = (int)_evtOrderEFRepository.NextKey();

            var contractCode = _evtOrderCommonService.GenContractCode(new OrderContractCodeDto
            {
                OrderId = orderId,
                ConfigContractCodeId = eventFind.ConfigContractCodeId,
                BuyDate = DateTime.Now,
            });
            var insert = new EvtOrder
            {
                Id = orderId,
                ContractAddressId = input.ContractAddressId,
                TradingProviderId = eventFind.TradingProviderId,
                ContractCode = _evtOrderCommonService.ContractCode(orderId),
                ContractCodeGen = contractCode,
                EventDetailId = input.EventDetailId,
                InvestorId = investorIdentification.InvestorId,
                InvestorIdenId = input.InvestorIdenId,
                ExpiredTime = eventDetail.PaymentWaittingTime != null ? DateTime.Now.AddSeconds((double)eventDetail.PaymentWaittingTime) : null,
                IsReceiveHardTicket = input.IsReceiveHardTicket,
                IsRequestReceiveRecipt = input.IsRequestReceiveRecipt,
                IpAddressCreated = ipAddress,
            };

            //tu mua hay sale mua
            if (isSelfDoing)
            {
                insert.Source = SourceOrder.ONLINE;
                insert.Status = OrderStatus.CHO_THANH_TOAN;
                // Tìm kiếm thông tin sale nếu có mã giới thiệu
                if (!string.IsNullOrWhiteSpace(input.SaleReferralCode))
                {
                    var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, eventFind.TradingProviderId);
                    if (findSale != null)
                    {
                        insert.ReferralSaleId = findSale.SaleId;
                        insert.DepartmentId = findSale.DepartmentId;
                    }
                }
            }
            else
            {
                int saleId = CommonUtils.GetCurrentSaleId(_httpContext);
                var sale = _dbContext.Sales.FirstOrDefault(e => e.SaleId == saleId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotFound);
                var findCifCode = _dbContext.CifCodes.FirstOrDefault(c => c.InvestorId == sale.InvestorId && c.Deleted == YesNo.NO);
                var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(findCifCode.CifCode, eventFind.TradingProviderId);
                if (findSale != null)
                {
                    insert.ReferralSaleId = findSale.SaleId;
                    insert.DepartmentId = findSale.DepartmentId;
                }
                insert.Source = SourceOrder.OFFLINE;
                insert.Status = OrderStatus.KHOI_TAO;
            }

            var result = _dbContext.EvtOrders.Add(insert);
            _dbContext.SaveChanges();

            int countFree = 0;
            if (input.OrderDetails != null)
            {
                foreach (var item in input.OrderDetails)
                {
                    var ticket = _dbContext.EvtTickets.FirstOrDefault(e => e.Id == item.TicketId
                                                                        && e.StartSellDate <= DateTime.Now
                                                                        && e.EndSellDate >= DateTime.Now
                                                                        && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
                    
                    if (!_evtOrderCommonService.AppCheckRemainingTickets(item.TicketId))
                    {
                        _defErrorEFRepository.ThrowException(ErrorCode.EvtTicketsSoldOut, ticket.Name);
                    };

                    if (ticket.IsFree)
                    {
                        countFree++;
                    };

                    if (countFree == input.OrderDetails.Count)
                    {
                        result.Entity.Status = EvtOrderStatus.HOP_LE;
                        result.Entity.ApproveDate = DateTime.Now;
                        result.Entity.ApproveBy = userName;
                        result.Entity.IsFree = true;
                        _dbContext.SaveChanges();
                    }
                   

                    if (ticket != null)
                    {
                        var orderDetailId = (int)_evtOrderDetailEFRepository.NextKey();
                        _dbContext.EvtOrderDetails.Add(new EvtOrderDetail
                        {
                            Id = orderDetailId,
                            OrderId = result.Entity.Id,
                            TicketId = item.TicketId,
                            Quantity = item.Quantity,
                            Price = ticket.IsFree ? 0 : ticket.Price
                        });
                        _dbContext.SaveChanges();

                        //lay ra list TicketCode da random
                        List<string> listTicketCode = _evtOrderCommonService.ListTicketCodeGenerate(item.Quantity, EvtOrderMoreConst.DO_DAI_CHUOI_TICKET_CODE);
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            _dbContext.EvtOrderTicketDetails.Add(new EvtOrderTicketDetail
                            {
                                Id = (int)_evtOrderTicketDetailEFRepository.NextKey(),
                                OrderDetailId = orderDetailId,
                                TicketId = item.TicketId,
                                Status = EvtOrderTicketStatus.CHUA_THAM_GIA,
                                TicketCode = listTicketCode[i],
                            });
                        }
                        _dbContext.SaveChanges();
                    }
                }
            }
            if (result.Entity.Status == EvtOrderStatus.HOP_LE)
            {
                await _eventNotificationServices.SendNotifyRegisterTicket(result.Entity.Id);
                //Xử lý gọi hàm cập nhật realtime
                await _evtSignalRBroadcastService.BroadcastEventQuantityTicket(eventFind.Id);
                var jobId = _backgroundJobs.Enqueue(() => _orderTicketFillService.FillOrderTicket(result.Entity.Id));
                result.Entity.TicketJobId = jobId;
                _dbContext.SaveChanges();
                if (input.IsRequestReceiveRecipt)
                {
                    await _eventNotificationServices.SendAdminNotifyReceiveRecipt(result.Entity.Id);
                }
                if (input.IsReceiveHardTicket)
                {
                    await _eventNotificationServices.SendAdminNotifyReceiveHardTicket(result.Entity.Id);
                }
            }
            transaction.Commit();
            return orderId;
        }

        public async Task<IEnumerable<AppTradingBankAccountDto>> TradingBankAccountOfEvent(int orderId, int eventId, string contractCode, decimal totalValue)
        {
            _logger.LogInformation($"{nameof(TradingBankAccountOfEvent)}: orderId = {orderId}, eventId = {eventId}, contractCode = {contractCode}, totalValue = {totalValue}");
            var result = new List<AppTradingBankAccountDto>();
            var listBankAccount = _evtEventBankAccountEFRepository.Entity.Where(b => b.EventId == eventId && b.Deleted == YesNo.NO);
            foreach (var bankAccountItem in listBankAccount)
            {
                var resultBankItem = new AppTradingBankAccountDto();
                var tradingBankFind = _dbContext.BusinessCustomerBanks.Include(b => b.BusinessCustomer)
                                       .Where(b => b.BusinessCustomerBankAccId == bankAccountItem.BusinessCustomerBankAccId && b.Deleted == YesNo.NO)
                                       .Select(b => new BusinessCustomerBankDto
                                       {
                                           BusinessCustomerBankAccId = bankAccountItem.BusinessCustomerBankAccId,
                                           BusinessCustomerId = b.BusinessCustomerId,
                                           BankAccName = b.BankAccName,
                                           BankAccNo = b.BankAccNo,
                                           BankCode = b.CoreBank.BankCode,
                                           Logo = b.CoreBank.Logo,
                                           BankId = b.BankId,
                                           BankName = b.CoreBank.BankName,
                                           FullBankName = b.CoreBank.FullBankName,
                                           Status = b.Status,
                                           IsDefault = b.IsDefault,
                                           AvatarTradingImageUrl = b.BusinessCustomer.AvatarImageUrl
                                       }).FirstOrDefault();
                if (tradingBankFind == null)
                {
                    continue;
                }
                resultBankItem = _mapper.Map<AppTradingBankAccountDto>(tradingBankFind);

                var orderDetailFind = _evtOrderDetailEFRepository.EntityNoTracking.Where(o => o.OrderId == orderId);
                if (orderDetailFind.Any())
                {
                    resultBankItem.TotalValue = orderDetailFind.Sum(o => o.Quantity * o.Price);
                }
                resultBankItem.Note = $"TT {contractCode}";
                if (tradingBankFind.BankId == FixBankId.Msb)
                {
                    var prefixAcc = _tradingMSBPrefixAccountEFRepository.Entity.FirstOrDefault(t => t.TradingBankAccountId == tradingBankFind.BusinessCustomerBankAccId && t.Deleted == YesNo.NO);
                    if (prefixAcc != null)
                    {
                        // Sinh QrCode nếu không sinh được lấy như bình thường
                        try
                        {
                            var requestCollect = await _msbCollectMoneyServices.RequestCollectMoney(new MSB.Dto.CollectMoney.RequestCollectMoneyDto
                            {
                                TId = prefixAcc.TId,
                                MId = prefixAcc.MId,
                                OrderCode = $"{ContractCodes.EVENT}{orderId}",
                                AmountMoney = totalValue,
                                OwnerAccount = tradingBankFind.BankAccName,
                                PrefixAccount = prefixAcc.PrefixMsb,
                                Note = contractCode,
                            });
                            resultBankItem.BankAccNo = requestCollect.AccountNumber;
                            resultBankItem.QrCode = requestCollect.QrCode;
                        }
                        catch (Exception ex)
                        {
                            if (ex.GetType() != typeof(FaultException))
                            {
                                _logger.LogError(ex, $"{nameof(TradingBankAccountOfEvent)}: exception = {ex.Message}");
                            }
                        }
                    }
                }
                result.Add(resultBankItem);
            }
            return result;
        }

        /// <summary>
        /// Hủy đặt vé khi lệnh ở trạng thái chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        public void CancelOrder(int orderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var orderFind = _dbContext.EvtOrders.FirstOrDefault(o => o.Id == orderId && o.InvestorId == investorId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            var orderPayment = _dbContext.EvtOrderPayments.Any(o => o.OrderId == orderId && o.Status == OrderPaymentStatus.DA_THANH_TOAN && o.Deleted == YesNo.NO);
            if (orderPayment)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderNotDelete);
            }
            
            if (!(orderFind.Status == EvtOrderStatus.CHO_THANH_TOAN || orderFind.Status == EvtOrderStatus.KHOI_TAO))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderNotDelete);
            }
            orderFind.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// app yeu cau nhan hoa don, ve cung
        /// </summary>
        /// <param name="input"></param>
        public async Task InvoiceTicketRequest(InvoiceTicketRequestDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(InvoiceTicketRequest)}:  username = {username}, investorId = {investorId}");
            var orderFind = _dbContext.EvtOrders.Include(o => o.OrderPayments)
                                    .FirstOrDefault(o => o.InvestorId == investorId 
                                                        && !o.OrderPayments.Any(od => od.Status == OrderPaymentStatus.DA_THANH_TOAN)
                                                        && o.Id == input.Id 
                                                        && o.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound);
            if(!_dbContext.InvestorContactAddresses.Any(i => i.Deleted == YesNo.NO 
                                                    && i.ContactAddressId == input.ContractAddressId
                                                    && i.InvestorId == investorId))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.InvestorContactAddressNotFound);
            }
            orderFind.ContractAddressId = input.ContractAddressId;

            if (input.IsRequestReceiveRecipt &&_dbContext.EvtEventDetails.Include(e => e.Event).Any(e => e.Id == orderFind.EventDetailId && e.Deleted == YesNo.NO
                                                                       && !e.Event.CanExportRequestRecipt && e.Event.Deleted == YesNo.NO))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderRequestReceiveReciptNotFound);
            }

            if (input.IsReceiveHardTicket && _dbContext.EvtEventDetails.Include(e => e.Event).Any(e => e.Id == orderFind.EventDetailId && e.Deleted == YesNo.NO
                                                                     && !e.Event.CanExportTicket && e.Event.Deleted == YesNo.NO))
            {
                _evtOrderEFRepository.ThrowException(ErrorCode.EvtOrderReceiveHardTicketNotFound);
            }

            if (input.IsRequestReceiveRecipt && orderFind.DeliveryInvoiceStatus != null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderInvoiceRequestVaild);
            }
            else if(input.IsRequestReceiveRecipt)
            {
                orderFind.IsRequestReceiveRecipt = true;
                orderFind.DeliveryInvoiceStatus = EventDeliveryStatus.WAITING;
                orderFind.PendingInvoiceDate = DateTime.Now;
                orderFind.PendingInvoiceDateModifiedBy = username;
                await _eventNotificationServices.SendAdminNotifyReceiveRecipt(orderFind.Id);
            }

            if (input.IsReceiveHardTicket && orderFind.DeliveryStatus != null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.EvtOrderTicketRequestVaild);
            }
            else if (input.IsReceiveHardTicket)
            {
                orderFind.IsReceiveHardTicket = true;
                orderFind.DeliveryStatus = EventDeliveryStatus.WAITING;
                orderFind.PendingDate = DateTime.Now;
                orderFind.PendingDateModifiedBy = username;
                await _eventNotificationServices.SendAdminNotifyReceiveHardTicket(orderFind.Id);
            }
            _dbContext.SaveChanges();
        }
    }
}
