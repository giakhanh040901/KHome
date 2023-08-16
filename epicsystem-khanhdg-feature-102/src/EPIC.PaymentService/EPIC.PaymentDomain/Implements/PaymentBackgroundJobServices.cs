using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.EventDomain.Interfaces;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerRepositories;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestRepositories;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.PaymentRepositories;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.PaymentDomain.Implements
{
    public class PaymentBackgroundJobServices
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _env;
        private readonly EpicSchemaDbContext _dbContext;

        //core
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly UsersEFRepository _usersEFRepository;

        //msb
        private readonly MsbCollectMoneyServices _msbCollectMoneyServices;
        private readonly MsbNotificationRepository _msbPaymentRepository;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;

        //bond
        private readonly BondOrderRepository _bondOrderRepository;

        //invest
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestOrderPaymentEFRepository _investOrderPaymentEFRepository;
        private readonly IInvestOrderContractFileServices _investOrderContractFileServices;
        private readonly InvestNotificationServices _investNotificationServices;
        private readonly ProjectRepository _investProjectRepository;
        private readonly DistributionRepository _investDistributionRepository;
        private readonly InvestHistoryUpdateEFRepository _investHistoryUpdateEFRepository;
        private readonly InvestOrderRepository _investOrderRepository;

        //garner
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly IGarnerOrderContractFileServices _garnerOrderContractFileServices;
        private readonly GarnerNotificationServices _garnerNotificationServices;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;

        //realestate
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderPaymentEFRepository _rstOrderPaymentEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RealEstateNotificationServices _rstNotificationServices;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly InvestorSaleEFRepository _investorSaleEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        //event
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly IEvtOrderTicketFillService _evtOrderTicketFillService;
        private readonly IEvtSignalRBroadcastService _evtSignalRBroadcastService;

        public PaymentBackgroundJobServices(
            ILogger<PaymentBackgroundJobServices> logger,
            DatabaseOptions databaseOptions,
            IWebHostEnvironment env,
            EpicSchemaDbContext dbContext,
            MsbCollectMoneyServices msbCollectMoneyServices,
            IInvestOrderContractFileServices investOrderContractFileServices,
            InvestNotificationServices investNotificationServices,
            IGarnerOrderContractFileServices garnerOrderContractFileServices,
            GarnerNotificationServices garnerNotificationServices,
            EventNotificationServices eventNotificationServices,
            RealEstateNotificationServices rstNotificationServices,
            IEvtOrderTicketFillService evtOrderTicketFillService,
            IEvtSignalRBroadcastService evtSignalRBroadcastService)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _env = env;
            _dbContext = dbContext;
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _usersEFRepository = new UsersEFRepository(dbContext, logger);
            _msbCollectMoneyServices = msbCollectMoneyServices;
            _msbPaymentRepository = new MsbNotificationRepository(dbContext, logger);
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _investProjectRepository = new ProjectRepository(_connectionString, logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, logger);

            _bondOrderRepository = new BondOrderRepository(_connectionString, logger);

            _investOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _investOrderPaymentEFRepository = new InvestOrderPaymentEFRepository(dbContext, logger);
            _investDistributionRepository = new DistributionRepository(_connectionString, logger);
            _investOrderContractFileServices = investOrderContractFileServices;
            _investNotificationServices = investNotificationServices;
            _investHistoryUpdateEFRepository = new InvestHistoryUpdateEFRepository(dbContext, logger);

            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerOrderContractFileServices = garnerOrderContractFileServices;
            _garnerNotificationServices = garnerNotificationServices;

            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstOrderPaymentEFRepository = new RstOrderPaymentEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstNotificationServices = rstNotificationServices;
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _investorSaleEFRepository = new InvestorSaleEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _eventNotificationServices = eventNotificationServices;
            _evtOrderTicketFillService = evtOrderTicketFillService;
            _evtSignalRBroadcastService = evtSignalRBroadcastService;
        }

        /// <summary>
        /// Active lệnh invest<br/>
        /// Gồm các công việc: Đổi trạng thái lệnh (dựa theo số tiền chuyển đủ không), ký điện tử, gửi thông báo hợp đồng active
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 60, 120, 180, 600 })]
        [Queue(HangfireQueues.Payment)]
        [HangfireLogEverything]
        public async Task ActiveInvest(long orderId)
        {
            var orderFind = _investOrderEFRepository.FindById(orderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound, orderId);

            // Nếu không thuộc một trong các trạng thái này thì bỏ qua
            if (!new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG }.Contains(orderFind.Status))
            {
                return;
            }

            var transaction = _dbContext.Database.BeginTransaction();
            string modifiedBy = _dbContext.GetUserByCifCode(orderFind.CifCode);
            try
            {
                //Thay đổi OrderStatus theo tổng số tiền đã chuyển vào
                decimal totalValue = orderFind.TotalValue;
                decimal totalPaymentValue = _investOrderPaymentEFRepository.SumPaymentAmount(orderFind.Id);
                if (totalPaymentValue == totalValue)
                {
                    var policyDetailFind = _dbContext.InvestPolicyDetails.FirstOrDefault(p => p.Id == orderFind.PolicyDetailId && p.Deleted == YesNo.NO)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyDetailNotFound);
                    var distribution = _dbContext.InvestDistributions.FirstOrDefault(p => p.Id == orderFind.DistributionId && p.Deleted == YesNo.NO)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);

                    //Check thông tin đại lý xem có cấu hình chữ ký số hay không, nếu không thì không ký và không active hợp đồng, hợp đồng chuyển sang trạng thái Chờ duyệt hợp đồng
                    var tradingProviderInfo = _dbContext.TradingProviders.FirstOrDefault(t => t.TradingProviderId == orderFind.TradingProviderId && t.Deleted == YesNo.NO)
                        .ThrowIfNull(_dbContext, ErrorCode.TradingProviderNotFound, orderFind.TradingProviderId);
                    if (tradingProviderInfo.Secret != null && tradingProviderInfo.Server != null && tradingProviderInfo.Key != null && tradingProviderInfo.StampImageUrl != null)
                    {
                        //Tính tổng thanh toán được duyệt
                        var listOrderPayment = _dbContext.InvestOrderPayments.Where(o => o.OrderId == orderId && o.Status == OrderPaymentStatus.DA_THANH_TOAN && o.TranType == TranTypes.THU && o.Deleted == YesNo.NO).ToList();
                        DateTime? maxTransDate = listOrderPayment.Max(p => p.TranDate);
                        orderFind.PaymentFullDate = maxTransDate;
                        orderFind.InvestDate = maxTransDate;
                        orderFind.Status = OrderStatus.DANG_DAU_TU;
                        orderFind.ApproveBy = modifiedBy;
                        orderFind.ApproveDate = DateTime.Now;
                        orderFind.ModifiedBy = modifiedBy;
                        orderFind.ModifiedDate = DateTime.Now;
                        orderFind.ActiveDate = DateTime.Now;

                        // Cập nhật trạng thái giao nhận hợp đồng
                        if (orderFind.Source == SourceOrder.OFFLINE && orderFind.DeliveryStatus == null)
                        {
                            orderFind.DeliveryStatus = DeliveryStatus.WAITING;
                            orderFind.PendingDate = DateTime.Now;
                            orderFind.PendingDateModifiedBy = modifiedBy;
                        }
                        else if (orderFind.DeliveryStatus == DeliveryStatus.WAITING && orderFind.PendingDate == null)
                        {
                            orderFind.PendingDate = DateTime.Now;
                            orderFind.PendingDateModifiedBy = modifiedBy;
                        }

                        // Tính ngày đáo hạn lưu vào Order
                        DateTime dueDate = _investOrderRepository.CalculateDueDate(policyDetailFind, orderFind.InvestDate.Value, distribution.CloseCellDate);
                        orderFind.DueDate = dueDate;
                        var cifCode = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode);
                        if (cifCode != null && cifCode.InvestorId != null)
                        {
                            _investorEFRepository.InsertInvestorTradingProvider(cifCode.InvestorId ?? 0, orderFind.TradingProviderId);

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
                        //Ký điện tử
                        _investOrderContractFileServices.UpdateContractFileSignPdf(orderId, ContractTypes.DAT_LENH);
                    }
                    else
                    {
                        orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                    }
                }
                else if (totalPaymentValue > totalValue)
                {
                    orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                }
                var projectFind = _investProjectRepository.FindById(orderFind.ProjectId);
                _dbContext.SaveChanges();
                transaction.Commit();
                await _investNotificationServices.SendEmailInvestOrderActive(orderId);
                if (orderFind.SaleReferralCode != null)
                {
                    await _investNotificationServices.SendNotifySaleInvestOrderActive(orderId, DateTime.Now);
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Active lệnh garner<br/>
        /// Gồm các công việc: Đổi trạng thái lệnh (dựa theo số tiền chuyển đủ không), ký điện tử, gửi thông báo hợp đồng active
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 60, 120, 180, 600 })]
        [Queue(HangfireQueues.Payment)]
        [HangfireLogEverything]
        public async Task ActiveGarner(long orderId)
        {
            var orderFind = _garnerOrderEFRepository.FindById(orderId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound, orderId);
            string modifiedBy = _dbContext.GetUserByCifCode(orderFind.CifCode);

            // Nếu không thuộc một trong các trạng thái này thì bỏ qua
            if (!new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG }.Contains(orderFind.Status))
            {
                return;
            }
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // Đổi OrderStatus theo tổng số tiền đã chuyển vào
                decimal totalValue = orderFind.TotalValue;
                decimal totalPaymentValue = _garnerOrderPaymentEFRepository.SumPaymentAmount(orderFind.Id);
                if (totalPaymentValue == totalValue)
                {
                    //Check thông tin đại lý xem có cấu hình chữ ký số hay không, nếu không thì không ký và không active hợp đồng, hợp đồng chuyển sang trạng thái Chờ duyệt hợp đồng
                    var tradingProviderInfo = _dbContext.TradingProviders.FirstOrDefault(t => t.TradingProviderId == orderFind.TradingProviderId && t.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.TradingProviderNotFound, orderFind.TradingProviderId);
                    if (tradingProviderInfo.Secret != null && tradingProviderInfo.Server != null && tradingProviderInfo.Key != null && tradingProviderInfo.StampImageUrl != null)
                    {
                        //Tính tổng thanh toán được duyệt
                        var listOrderPayment = _dbContext.GarnerOrderPayments.Where(o => o.OrderId == orderId && o.Status == OrderPaymentStatus.DA_THANH_TOAN && o.TranType == TranTypes.THU && o.Deleted == YesNo.NO).ToList();
                        DateTime? maxTransDate = listOrderPayment.Max(p => p.TranDate);
                        orderFind.PaymentFullDate = maxTransDate;
                        orderFind.InvestDate = maxTransDate;
                        orderFind.Status = OrderStatus.DANG_DAU_TU;
                        orderFind.ApproveBy = modifiedBy;
                        orderFind.ApproveDate = DateTime.Now;
                        orderFind.ModifiedBy = modifiedBy;
                        orderFind.ModifiedDate = DateTime.Now;
                        orderFind.ActiveDate = DateTime.Now;

                        // Tính ngày đáo hạn lưu vào Order
                        var cifCode = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode);
                        if (cifCode != null && cifCode.InvestorId != null)
                        {
                            _investorEFRepository.InsertInvestorTradingProvider(cifCode.InvestorId ?? 0, orderFind.TradingProviderId);

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
                        //Ký điện tử
                        _garnerOrderContractFileServices.UpdateContractFileSignPdf(orderId);
                    }
                    else
                    {
                        orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                    }
                }
                else if (totalPaymentValue > totalValue)
                {
                    orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                }
                var product = _garnerProductEFRepository.FindById(orderFind?.ProductId ?? 0);
                var distribution = _garnerDistributionEFRepository.FindById(orderFind?.DistributionId ?? 0);

                //đếm tiền các lệnh đã đầu tư hoặc đang chờ duyệt hợp đồng
                var sumMoneyOrder = _garnerOrderEFRepository.SumValue(orderFind?.DistributionId ?? 0, null);
                var hanMucDauTu = (product?.InvTotalInvestment == null || product?.InvTotalInvestment == 0) ? ((product?.CpsQuantity ?? 0) * (product?.CpsParValue ?? 0)) : product?.InvTotalInvestment;
                if (sumMoneyOrder >= hanMucDauTu)
                {
                    distribution.IsShowApp = YesNo.NO;
                }
                _dbContext.SaveChanges();
                transaction.Commit();

                await _garnerNotificationServices.SendNotifyGarnerOrderActive(orderId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Active lệnh RealEstate<br/>
        /// Gồm các công việc: Đổi trạng thái lệnh (dựa theo số tiền chuyển đủ không), ký điện tử, gửi thông báo hợp đồng active
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 60, 120, 180, 600 })]
        [Queue(HangfireQueues.Payment)]
        [HangfireLogEverything]
        public async Task ActiveRealEstate(int orderId)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            var orderFind = _rstOrderEFRepository.FindById(orderId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound, orderId);

            // Nếu không thuộc một trong các trạng thái này thì bỏ qua
            if (!new int[] { RstOrderStatus.KHOI_TAO, RstOrderStatus.CHO_THANH_TOAN_COC, RstOrderStatus.CHO_DUYET_HOP_DONG_COC }.Contains(orderFind.Status))
            {
                return;
            }

            var productItem = _rstProductItemEFRepository.FindById(orderFind.ProductItemId)
                .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(orderFind.OpenSellDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            try
            {
                // Đổi OrderStatus theo tổng số tiền đã chuyển vào
                decimal depositMoney = orderFind.DepositMoney;
                decimal totalPaymentValue = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(orderFind.Id);
                // Lấy giá cọc, giá lock căn của hợp đồng
                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByPolicy(productItem.Price ?? 0, orderFind.DistributionPolicyId);

                // Kiểm tra và cập nhật trạng thái hợp đồng nếu đủ số tiền cọc
                if (totalPaymentValue >= depositMoney)
                {
                    orderFind.Status = RstOrderStatus.CHO_DUYET_HOP_DONG_COC;
                    var orderApprove = _rstOrderEFRepository.Approve(orderFind.Id, null, PaymentTypes.CHUYEN_KHOAN);
                    var cifCode = _cifCodeEFRepository.FindByCifCode(orderApprove.CifCode);
                    if (cifCode != null && cifCode.InvestorId != null)
                    {
                        _investorEFRepository.InsertInvestorTradingProvider(cifCode.InvestorId ?? 0, orderFind.TradingProviderId);

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
                }
                else
                {
                    orderFind.Status = RstOrderStatus.CHO_THANH_TOAN_COC;
                }
                orderFind.StatusMax = (orderFind.Status > orderFind.StatusMax) ? orderFind.Status : orderFind.StatusMax;

                // Căn hộ chưa được khóa căn, nếu hợp đồng có đủ hoặc thừa số tiền giữ chỗ thì cho Khóa căn
                if ((productItem.Status == RstProductItemStatus.KHOI_TAO || productItem.Status == RstProductItemStatus.GIU_CHO)
                    && totalPaymentValue >= productItemPrice.LockPrice && totalPaymentValue < depositMoney)
                {
                    productItem.Status = RstProductItemStatus.KHOA_CAN;
                    var orderApprove = _rstOrderEFRepository.Approve(orderFind.Id, null, PaymentTypes.CHUYEN_KHOAN);
                    var cifCode = _cifCodeEFRepository.FindByCifCode(orderApprove.CifCode);
                    if (cifCode != null && cifCode.InvestorId != null)
                    {
                        _investorEFRepository.InsertInvestorTradingProvider(cifCode.InvestorId ?? 0, orderFind.TradingProviderId);
                    }
                }
                // Trường hợp căn đã khóa thì check xem khóa căn có phải là do hợp đồng hay không
                else if (productItem.Status == RstProductItemStatus.KHOA_CAN && (openSellDetail.Status != RstProductItemStatus.KHOA_CAN
                    || (openSellDetail.Status == RstProductItemStatus.KHOA_CAN && totalPaymentValue < productItemPrice.LockPrice)))
                {
                    _rstOrderPaymentEFRepository.ThrowException(ErrorCode.RstOrderPaymentCanNotApproveCuzProductItemLockNotOfOrder);
                }

                _dbContext.SaveChanges();
                transaction.Commit();

                await _rstNotificationServices.SendNotifyRstOrderActive(orderId);
                if (orderFind.SaleReferralCode != null)
                {
                    await _rstNotificationServices.SendNotifySaleOrderActive(orderId);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Active lệnh event<br/>
        /// Gồm các công việc: Đổi trạng thái lệnh (dựa theo số tiền chuyển đủ không), ký điện tử, gửi thông báo hợp đồng active
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 60, 120, 180, 600 })]
        [Queue(HangfireQueues.Payment)]
        [HangfireLogEverything]
        public async Task ActiveEvent(int orderId)
        {
            var orderFind = _dbContext.EvtOrders.FirstOrDefault(ed => ed.Id == orderId && ed.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtOrderNotFound, orderId);

            // Nếu không thuộc một trong các trạng thái này thì bỏ qua
            if (!new int[] { EvtOrderStatus.KHOI_TAO, EvtOrderStatus.CHO_THANH_TOAN }.Contains(orderFind.Status))
            {
                return;
            }
            var transaction = _dbContext.Database.BeginTransaction();

            var cifCodeFind = _dbContext.CifCodes.FirstOrDefault(cc => cc.InvestorId == orderFind.InvestorId && cc.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string modifiedBy = _dbContext.GetUserByCifCode(cifCodeFind.CifCode);

            //Thay đổi OrderStatus theo tổng số tiền đã chuyển vào
            decimal totalMoney = _dbContext.EvtOrderDetails
                    .Where(od => od.OrderId == orderFind.Id)
                    .Sum(od => od.Quantity * od.Price);

            decimal totalPaymentValue = _dbContext.EvtOrderPayments.Where(o => o.OrderId == orderFind.Id
                                                                        && o.TranClassify == TranClassifies.THANH_TOAN
                                                                        && o.Deleted == YesNo.NO && o.Status == OrderPaymentStatus.DA_THANH_TOAN)
                                                             .Sum(o => o.PaymentAmount);
            if (totalPaymentValue == totalMoney)
            {
                var tradingProviderInfo = _dbContext.TradingProviders.FirstOrDefault(t => t.TradingProviderId == orderFind.TradingProviderId && t.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.TradingProviderNotFound, orderFind.TradingProviderId);

                //Tính tổng thanh toán được duyệt
                var listOrderPayment = _dbContext.EvtOrderPayments.Where(o => o.OrderId == orderId && o.Status == OrderPaymentStatus.DA_THANH_TOAN && o.Deleted == YesNo.NO);
                DateTime? maxTransDate = listOrderPayment.Max(p => p.TranDate);
                orderFind.Status = EvtOrderStatus.HOP_LE;
                orderFind.ApproveDate = DateTime.Now;
                orderFind.ApproveBy = modifiedBy;
                orderFind.ModifiedBy = modifiedBy;
                orderFind.ModifiedDate = DateTime.Now;

                // Cập nhật trạng thái giao nhận vé, hoa don sự kiện

                if (orderFind.IsRequestReceiveRecipt)
                {
                    orderFind.DeliveryInvoiceStatus = EventDeliveryStatus.WAITING;
                    orderFind.PendingInvoiceDate = DateTime.Now;
                    orderFind.PendingInvoiceDateModifiedBy = modifiedBy;
                }

                if (orderFind.IsReceiveHardTicket)
                {
                    orderFind.DeliveryStatus = EventDeliveryStatus.WAITING;
                    orderFind.PendingDate = DateTime.Now;
                    orderFind.PendingDateModifiedBy = modifiedBy;
                }

                _dbContext.SaveChanges();
                await _eventNotificationServices.SendNotifyRegisterTicket(orderFind.Id);
                //đợi fill vé
                await _evtOrderTicketFillService.FillOrderTicket(orderFind.Id);

                _investorEFRepository.InsertInvestorTradingProvider(orderFind.InvestorId, orderFind.TradingProviderId);
                //Thêm quan hệ InvestorSale
                if (orderFind.ReferralSaleId != null)
                {
                    // insert bản ghi investorSale
                    _investorSaleEFRepository.InsertInvestorSale(new InvestorSale
                    {
                        InvestorId = orderFind.InvestorId,
                        SaleId = orderFind.ReferralSaleId,
                        ReferralCode = orderFind.Sale?.Investor?.ReferralCode
                    });
                }
                _dbContext.SaveChanges();
                await _evtSignalRBroadcastService.BroadcastOrderActive(orderFind.Id);
            }
            else if (totalPaymentValue > totalMoney)
            {
                orderFind.Status = EvtOrderStatus.CHO_XU_LY;
                _dbContext.SaveChanges();
                await _evtSignalRBroadcastService.BroadcastOrderActive(orderFind.Id);
            }
            transaction.Commit();
        }
    }
}
