using EPIC.CoreEntities.Dto.Sale;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.Sale;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstOrderEFRepository : BaseEFRepository<RstOrder>
    {
        public RstOrderEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOrder.SEQ}")
        {
        }

        #region CMS
        /// <summary>
        /// Thêm hợp đồng sổ lệnh trên CMS
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstOrder Add(RstOrder input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username} ");
            CheckOpenSellDetail(input.OpenSellDetailId);
            var orderId = (int)NextKey();
            input.Id = orderId;
            input.TradingProviderId = tradingProviderId;
            input.ContractCode = ContractCode(orderId);
            input.Source = SourceOrder.OFFLINE;
            input.Status = RstOrderStatus.KHOI_TAO;
            input.StatusMax = RstOrderStatus.KHOI_TAO;
            input.CreatedBy = username;
            var result = _dbSet.Add(input);
            return result.Entity;
        }

        /// <summary>
        /// Tìm kiếm theo Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrder FindById(int orderId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(FindById)}: orderId ={orderId}, tradingProviderId = {tradingProviderId}");

            var order = _dbSet.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO && (tradingProviderId == null || o.TradingProviderId == tradingProviderId));
            return order;
        }

        /// <summary>
        /// Tìm kiếm phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <param name="status"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="isGroupByCustomer"></param>
        /// <returns></returns>
        public PagingResult<RstOrder> FindAll(FilterRstOrderDto input, int[] status, int? tradingProviderId = null, bool isGroupByCustomer = false)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, " +
                $"status = {status}, isGroupByCustomer = {isGroupByCustomer}");

            PagingResult<RstOrder> result = new();

            IQueryable<RstOrder> orderQuery = from order in _dbSet
                                              join productItem in _epicSchemaDbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                              join project in _epicSchemaDbContext.RstProjects on productItem.ProjectId equals project.Id
                                              join cifcode in _epicSchemaDbContext.CifCodes on order.CifCode equals cifcode.CifCode
                                              join businessCustomer in _epicSchemaDbContext.BusinessCustomers on cifcode.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                                              from businessCustomer in businessCustomers.DefaultIfEmpty()
                                              join investor in _epicSchemaDbContext.Investors on cifcode.InvestorId equals investor.InvestorId into investors
                                              from investor in investors.DefaultIfEmpty()
                                              where order.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && project.Deleted == YesNo.NO
                                              && (input.ProjectId == null || project.Id == input.ProjectId)
                                              && (input.Status == null || order.Status == input.Status) && status.Contains(order.Status)
                                              && (input.DepositDate == null || (order.DepositDate != null && order.DepositDate.Value.Date == input.DepositDate.Value.Date))
                                              && (input.ContractCode == null || order.ContractCode.Contains(input.ContractCode)
                                                                             || _epicSchemaDbContext.RstOrderContractFiles.Any(o => o.ContractCodeGen == input.ContractCode
                                                                                && o.OrderId == order.Id && o.Deleted == YesNo.NO))
                                              && (input.CifCode == null || order.CifCode.Contains(input.CifCode))
                                              && (input.Source == null || order.Source == input.Source)
                                              && (input.Phone == null || businessCustomer.Phone.Contains(input.Phone) || investor.Phone.Contains(input.Phone))
                                              && (input.TaxCode == null || businessCustomer.TaxCode == input.TaxCode)
                                              && (input.IdNo == null || _epicSchemaDbContext.InvestorIdentifications.Any(i => i.InvestorId == investor.InvestorId && i.IdNo.Contains(input.IdNo) && i.Deleted == YesNo.NO))
                                              && (input.ContractCodeGen == null || _epicSchemaDbContext.RstOrderContractFiles.Any(o => o.ContractCodeGen == input.ContractCodeGen
                                                                                    && o.OrderId == order.Id && o.Deleted == YesNo.NO))
                                              select order;

            if (tradingProviderId != null)
            {
                orderQuery = orderQuery.Where(o => o.TradingProviderId == tradingProviderId);
            }
            else
            {
                orderQuery = orderQuery.Where(o => input.TradingProviderIds != null && input.TradingProviderIds.Contains(o.TradingProviderId));
            }

            //Lọc theo nguồn đặt lệnh
            if (input.Orderer == SourceOrderFE.QUAN_TRI_VIEN)
            {
                orderQuery = orderQuery.Where(e => e.Source == SourceOrder.OFFLINE);
            }
            else if (input.Orderer == SourceOrderFE.KHACH_HANG)
            {
                orderQuery = orderQuery.Where(e => e.Source == SourceOrder.ONLINE && e.SaleOrderId == null);
            }
            else if (input.Orderer == SourceOrderFE.SALE)
            {
                orderQuery = orderQuery.Where(e => e.Source == SourceOrder.OFFLINE && e.SaleOrderId != null);
            }

            result.TotalItems = orderQuery.Count();
            orderQuery = orderQuery.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                // đếm tổng trước khi phân trang
                orderQuery = orderQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = orderQuery;
            return result;
        }

        /// <summary>
        /// Tìm kiếm theo mã hợp đồng
        /// </summary>
        /// <param name="contractCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrder FindByContractCode(string contractCode, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(FindByContractCode)}: contractCode = {contractCode}; tradingProviderId = {tradingProviderId}");
            return _dbSet.FirstOrDefault(o => o.ContractCode == contractCode && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm theo CifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<RstOrder> FindByCifCode(string cifCode, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(FindByCifCode)}: cifCode = {cifCode}; tradingProviderId = {tradingProviderId}");
            return _dbSet.Where(o => o.CifCode == cifCode && (tradingProviderId == null || o.TradingProviderId == tradingProviderId) && o.Deleted == YesNo.NO).ToList();
        }

        /// <summary>
        /// Cập nhật sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstOrder Update(RstOrder input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var orderFind = _dbSet.FirstOrDefault(o => o.Id == input.Id && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO).ThrowIfNull<RstOrder>(_epicSchemaDbContext, ErrorCode.RstOrderNotFound);
            if (orderFind != null)
            {
                orderFind.InvestorIdenId = input.InvestorIdenId;
                orderFind.SaleReferralCode = input.SaleReferralCode;
                orderFind.ContractAddressId = input.ContractAddressId;
                orderFind.PaymentType = input.PaymentType;
                orderFind.ModifiedBy = username;
                orderFind.ModifiedDate = DateTime.Now;
            }
            return orderFind;
        }

        public RstOrder Approve(int orderId, string username, int? paymentType = null)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(Approve)}: orderId = {orderId}, username = {username} ");

            var orderFind = _dbSet.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOrderNotFound);

            //Đổ trạng thái căn hộ và trạng thái sản phẩm của mở bán sang giữ chỗ
            var productItemQuery = _epicSchemaDbContext.RstProductItems.FirstOrDefault(c => c.Id == orderFind.ProductItemId && c.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstProductItemNotFound);
            var openSellDetail = _epicSchemaDbContext.RstOpenSellDetails.FirstOrDefault(c => c.Id == orderFind.OpenSellDetailId && c.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOpenSellDetailNotFound);

            var distributionProductItem = _epicSchemaDbContext.RstDistributionProductItems.FirstOrDefault(c => c.Id == openSellDetail.DistributionProductItemId && c.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstDistributionProductItemNotFound);

            // Nếu căn hộ đang bị khóa thì không cho duyệt hợp đồng cọc
            if (distributionProductItem.Status == Status.INACTIVE)
            {
                ThrowException(ErrorCode.RstOrderCanNotApproveCuzDistributionProductItemLock);
            }
            // Nếu trạng thái khác đã khóa căn 
            if (productItemQuery.Status != RstProductItemStatus.KHOA_CAN)
            {
                ThrowException(ErrorCode.RstOrderCanNotApproveCuzProductItemStatus);
            }

            if (orderFind.Status != RstOrderStatus.CHO_DUYET_HOP_DONG_COC)
            {
                ThrowException(ErrorCode.RstOrderCanNotApproveOrderStatusInValid);
            }
            // Tổng thanh toán cọc của hợp đồng
            var orderPaymentQuery = _epicSchemaDbContext.RstOrderPayments.Where(p => p.OrderId == orderId && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN
                                                && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN && (paymentType == null || p.PaymentType == paymentType));
            var depositAmount = orderPaymentQuery.Sum(p => p.PaymentAmount);
            // Nếu số tiền thanh toán cọc bé hơn số tiền cọc 
            if (orderFind.DepositMoney > depositAmount)
            {
                ThrowException(ErrorCode.RstOrderCanNotApproveDepositMoneyNotEqual);
            }

            productItemQuery.Status = RstProductItemStatus.DA_COC;
            openSellDetail.Status = RstProductItemStatus.DA_COC;

            orderFind.Status = RstOrderStatus.DA_COC;
            orderFind.StatusMax = (orderFind.Status > orderFind.StatusMax) ? orderFind.Status : orderFind.StatusMax;
            orderFind.ApproveBy = username;
            orderFind.ApproveDate = DateTime.Now;
            orderFind.DepositDate = orderPaymentQuery.Max(p => p.TranDate);
            return orderFind;
        }

        /// <summary>
        /// xóa order ở trạng thái khởi tạo
        /// </summary>
        public RstOrder Deleted(int orderId, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(Deleted)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, username = {username}");
            var orderFind = _dbSet.FirstOrDefault(o => o.Id == orderId && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO).ThrowIfNull<RstOrder>(_epicSchemaDbContext, ErrorCode.RstOrderNotFound);

            var result = DeletedCommon(orderFind, username);
            return orderFind;
        }

        /// <summary>
        /// Xóa order từ trên App
        /// </summary>
        public RstOrder AppDeleteOrder(int orderId, int investorId, string username)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(AppDeleteOrder)}: orderId = {orderId}, investorId = {investorId}, username = {username}");

            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                ThrowException(ErrorCode.CoreCifCodeNotFound);
            }

            var orderFind = _dbSet.FirstOrDefault(o => o.Id == orderId && o.CifCode == cifCodeFind.CifCode && o.Deleted == YesNo.NO).ThrowIfNull<RstOrder>(_epicSchemaDbContext, ErrorCode.RstOrderNotFound);
            var result = DeletedCommon(orderFind, username);
            return orderFind;
        }

        public RstOrder DeletedCommon(RstOrder orderData, string username)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(AppDeleteOrder)}: input = {JsonSerializer.Serialize(orderData)}, username = {username}");
            // Xem có trường hợp đã thanh toán nào không
            if (_epicSchemaDbContext.RstOrderPayments.Where(p => p.OrderId == orderData.Id && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO).Any())
            {
                ThrowException(ErrorCode.RstOrderExistPaymentNotDeleted);
            }

            //Đổ trạng thái căn hộ và trạng thái mở bán về khởi tạo nếu chưa hết thời gian giữ cọc
            if (orderData.Status < RstOrderStatus.CHO_DUYET_HOP_DONG_COC && orderData.ExpTimeDeposit != null && orderData.ExpTimeDeposit > DateTime.Now)
            {
                var productItemQuery = _epicSchemaDbContext.RstProductItems.FirstOrDefault(c => c.Id == orderData.ProductItemId && c.Deleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstProductItemNotFound);
                var openSellDetail = _epicSchemaDbContext.RstOpenSellDetails.FirstOrDefault(c => c.Id == orderData.OpenSellDetailId && c.Deleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOpenSellDetailNotFound);
                if (productItemQuery.Status == RstProductItemStatus.GIU_CHO && openSellDetail.Status == RstProductItemStatus.GIU_CHO)
                {
                    productItemQuery.Status = RstProductItemStatus.KHOI_TAO;
                    openSellDetail.Status = RstProductItemStatus.KHOI_TAO;
                }
            }

            if (orderData.Status == OrderStatus.KHOI_TAO || orderData.Status == OrderStatus.CHO_THANH_TOAN)
            {
                orderData.ModifiedBy = username;
                orderData.ModifiedDate = DateTime.Now;
                orderData.Deleted = YesNo.YES;
            }
            else
            {
                ThrowException(ErrorCode.RstOrderStatusNotInitialState);
            }
            return orderData;
        }
        #endregion

        #region App
        public AppRstOrderDataSuccessDto AppInvestorOrderAdd(RstOrder input, int investorId, string username, bool isCheck = false)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(AppInvestorOrderAdd)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}, username = {username}, isCheck ={isCheck}");

            //kiểm tra trạng thái chính sách
            var policyFind = _epicSchemaDbContext.RstDistributionPolicys.FirstOrDefault(p => p.Id == input.DistributionPolicyId && p.Deleted == YesNo.NO);
            if (policyFind.Status == Status.INACTIVE)
            {
                ThrowException(ErrorCode.RstDistributionPolicyNotActive);
            }

            // Kiểm tra xem sản phẩm mở bán có hợp lệ hay không
            CheckOpenSellDetail(input.OpenSellDetailId, true);
            AppRstOrderDataSuccessDto result = new();
            // Nếu là kiểm tra thì không lưu
            if (!isCheck)
            {
                input.Id = (int)NextKey();
                input.ContractCode = ContractCode(input.Id);
                input.CreatedBy = username;
                var insertOrder = _dbSet.Add(input).Entity;

                result.Id = insertOrder.Id;
                result.PaymentNote = PaymentNotes.THANH_TOAN + insertOrder.ContractCode;
                result.ContractCode = insertOrder.ContractCode;
                result.ExpTimeDeposit = insertOrder.ExpTimeDeposit;
                result.DepositMoney = insertOrder.DepositMoney;
                result.OpenSellDetailId = insertOrder.OpenSellDetailId;
            }
            return result;
        }

        /// <summary>
        /// Xem danh sách hợp đồng của nhà đầu tư
        /// </summary>
        public IQueryable<RstOrder> AppGetAllOrder(AppRstOrderFilterDto input, int investorId, int groupStatus)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(AppGetAllOrder)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}, groupStatus = {groupStatus}");

            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                ThrowException(ErrorCode.CoreCifCodeNotFound);
            }

            var result = from order in _dbSet
                         join productItem in _epicSchemaDbContext.RstProductItems on order.ProductItemId equals productItem.Id
                         join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on order.OpenSellDetailId equals openSellDetail.Id
                         where order.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                         && order.CifCode == cifCodeFind.CifCode && (input.Status == null || order.Status == input.Status)
                         && (input.StartDate == null || (order.CreatedDate != null && order.CreatedDate.Value.Date >= input.StartDate.Value.Date))
                         && (input.EndDate == null || (order.CreatedDate != null && order.CreatedDate.Value.Date <= input.EndDate.Value.Date))
                         // Lấy trạng tại đang ở màn sổ lệnh gồm các lệnh khởi tạo và các lệnh chờ thanh toán cọc chưa có thanh toán 
                         // đã có thanh toán nhưng chưa được khóa căn và hết thời gian
                         && ((groupStatus == RstAppOrderGroupStatus.SO_LENH && (order.Status == RstOrderStatus.KHOI_TAO
                                || (order.Status == RstOrderStatus.CHO_THANH_TOAN_COC && (order.ExpTimeDeposit < DateTime.Now
                                    || (!(_epicSchemaDbContext.RstOrderPayments.Where(p => p.OrderId == order.Id && p.TranClassify == TranClassifies.THANH_TOAN
                                    && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO)).Any()) && order.ExpTimeDeposit > DateTime.Now))
                            ))
                            // Đang giao dịch Lấy trạng thái chờ duyệt hợp đồng hoặc đã cọc
                            // Trạng thái chờ thanh toán khi : có thanh toán trong hợp đồng và (đã đủ tiền giữ chỗ sang khóa căn hoặc chưa đủ tiền nhưng còn thời gian giữ chỗ)
                            || (groupStatus == RstAppOrderGroupStatus.DANG_GIAO_DICH && (order.Status == RstOrderStatus.CHO_DUYET_HOP_DONG_COC || order.Status == RstOrderStatus.DA_COC
                                || (order.Status == RstOrderStatus.CHO_THANH_TOAN_COC
                                    && ((_epicSchemaDbContext.RstOrderPayments.Where(p => p.OrderId == order.Id && p.TranClassify == TranClassifies.THANH_TOAN
                                    && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO)).Any())
                                    && (openSellDetail.Status == RstProductItemStatus.KHOA_CAN || order.ExpTimeDeposit > DateTime.Now)))
                            ))
                         select order;
            return result.OrderByDescending(o => o.Id);
        }

        public List<RstOrder> AppGetAllOrderByInvestorId(int investorId, int? status = null)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(AppGetAllOrderByInvestorId)}: investorId = {investorId}, status = {status}");
            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            var result = _dbSet.Where(e => e.CifCode == cifCodeFind.CifCode && e.Deleted == YesNo.NO && (status == null || e.Status == status));
            return result.ToList();
        }
        #endregion

        #region Function
        private string ContractCode(int orderId)
        {
            return ContractCodes.REAL_ESTATE + orderId.ToString().PadLeft(8, '0');
        }

        /// <summary>
        /// Kiểm tra sản phẩm mở bán có hợp lệ hay không
        /// </summary>
        public void CheckOpenSellDetail(int openSellDetailId, bool isCheckApp = false)
        {
            var openSellDetailQuery = (from openSell in _epicSchemaDbContext.RstOpenSells
                                       join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on openSell.Id equals openSellDetail.OpenSellId
                                       join distributionProductItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProductItem.Id
                                       join distribution in _epicSchemaDbContext.RstDistributions on distributionProductItem.DistributionId equals distribution.Id
                                       join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                                       where openSellDetail.Id == openSellDetailId && openSell.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                                       && distributionProductItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                       select new
                                       {
                                           OpenSellStatus = openSell.Status,
                                           OpenSellStartDate = openSell.StartDate,
                                           DistributionProductItemStatus = distributionProductItem.Status,
                                           DistributionStatus = distribution.Status,
                                           DistributionStartDate = distribution.StartDate,
                                           ProductItemStatus = productItem.Status,
                                           ProductItemIsLock = productItem.IsLock,
                                           OpenSellDetailIsLock = openSellDetail.IsLock,
                                       }).FirstOrDefault().ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOpenSellDetailNotFound);

            // Căn bị đại lý khóa ở mở bán
            if (openSellDetailQuery.OpenSellDetailIsLock == YesNo.YES)
            {
                ThrowException(ErrorCode.RstOpenSellDetailLockByTradingProvider);
            }

            // Căn bị đối tác khóa trên căn hộ
            if (openSellDetailQuery.ProductItemIsLock == YesNo.YES)
            {
                ThrowException(ErrorCode.RstOpenSellDetailLockByPartner);
            }

            // Nếu trạng thái phân phối không trong trạng thái đang bán
            if (openSellDetailQuery.DistributionStatus != RstDistributionStatus.DANG_BAN || openSellDetailQuery.DistributionStartDate.Date > DateTime.Now.Date)
            {
                ThrowException(ErrorCode.RstDistributionNotInTrading);
            }
            // Sản phẩm của phân phối có đang được bán không
            if (openSellDetailQuery.DistributionProductItemStatus != Status.ACTIVE)
            {
                ThrowException(ErrorCode.RstDistributionProductItemNotActive);
            }
            // Mở bán có đang được phép bán hay không
            if (openSellDetailQuery.OpenSellStatus != RstDistributionStatus.DANG_BAN || openSellDetailQuery.OpenSellStartDate.Date > DateTime.Now.Date)
            {
                ThrowException(ErrorCode.RstOpenSellNotInTrading);
            }
            // Kiểm tra xem có căn hộ đã được giao dịch hay chưa
            if ((isCheckApp && openSellDetailQuery.ProductItemStatus != RstProductItemStatus.KHOI_TAO)
                || (!isCheckApp && !(new List<int> { RstProductItemStatus.KHOI_TAO, RstProductItemStatus.GIU_CHO }).Contains(openSellDetailQuery.ProductItemStatus)))
            {
                ThrowException(ErrorCode.RstProductItemInTrading);
            }
        }
        #endregion

        /// <summary>
        /// Thông tin hợp đồng mới nhất được tạo trong dự án
        /// </summary>
        public InfoOrderNewInProjectDto InfoOrderNewInProject(int orderId)
        {
            var orderQuery = (from order in _dbSet
                              join cifCode in _epicSchemaDbContext.CifCodes on order.CifCode equals cifCode.CifCode
                              join investor in _epicSchemaDbContext.Investors on cifCode.InvestorId equals investor.InvestorId into investors
                              from investor in investors.DefaultIfEmpty()
                              join businessCustomer in _epicSchemaDbContext.BusinessCustomers on cifCode.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                              from businessCustomer in businessCustomers.DefaultIfEmpty()
                              join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on order.OpenSellDetailId equals openSellDetail.Id
                              join productItem in _epicSchemaDbContext.RstProductItems on order.ProductItemId equals productItem.Id
                              where order.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                              && order.Id == orderId && cifCode.Deleted == YesNo.NO
                              select new
                              {
                                  CifCode = order.CifCode,
                                  TradingProviderId = order.TradingProviderId,
                                  ExpTimeDeposit = order.ExpTimeDeposit,
                                  Status = order.Status,
                                  OpenSellId = openSellDetail.OpenSellId,
                                  ProjectId = productItem.ProjectId,
                                  Code = productItem.Code,
                                  AvatarImageUrl = businessCustomer == null ? investor.AvatarImageUrl : investor.AvatarImageUrl,
                                  FullName = businessCustomer == null ? _epicSchemaDbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO).OrderByDescending(v => v.IsDefault).FirstOrDefault().Fullname
                                                                        : businessCustomer.Name
                              }).FirstOrDefault();
            if (orderQuery == null) return null;

            // Thông tin đại lý
            var tradingProviderInfo = (from tradingProvider in _epicSchemaDbContext.TradingProviders
                                       join businessCustomer in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                       where tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                       && tradingProvider.TradingProviderId == orderQuery.TradingProviderId
                                       select new
                                       {
                                           tradingProvider.AliasName,
                                           businessCustomer.Name
                                       }).FirstOrDefault();
            var result = new InfoOrderNewInProjectDto()
            {
                ProjectId = orderQuery.ProjectId,
                OpenSellId = orderQuery.OpenSellId,
                ExpTimeDeposit = orderQuery.ExpTimeDeposit,
                OrderStatus = orderQuery.Status,
                FullName = orderQuery.FullName,
                AliasName = tradingProviderInfo?.AliasName,
                TradingProviderName = tradingProviderInfo?.Name,
                AvatarImageUrl = orderQuery.AvatarImageUrl,
                ProductItemCode = orderQuery.Code
            };
            return result;
        }

        public IQueryable<AppRstOrderForSaleDto> GetAllOrderBySale(AppSaleFilterContractDto input)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(GetAllOrderBySale)}: input = {JsonSerializer.Serialize(input)}");
            // Lấy các thông tin hợp đồng của Sale bán được bao gồm cả bán hộ
            var result = from order in _dbSet
                         join cifCode in _epicSchemaDbContext.CifCodes on order.CifCode equals cifCode.CifCode
                         join productItem in _epicSchemaDbContext.RstProductItems on order.ProductItemId equals productItem.Id
                         join investor in _epicSchemaDbContext.Investors on cifCode.InvestorId equals investor.InvestorId into investors
                         from investor in investors.DefaultIfEmpty()
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers on cifCode.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         where cifCode.Deleted == YesNo.NO && order.Deleted == YesNo.NO && (input.DepartmentId == null || (order.DepartmentId == input.DepartmentId))
                         && (input.Status == null || order.Status == input.Status)
                         && (order.Status != RstOrderStatus.KHOI_TAO || (order.Status == RstOrderStatus.KHOI_TAO && (order.ExpTimeDeposit == null || order.ExpTimeDeposit > DateTime.Now
                            || (order.ExpTimeDeposit < DateTime.Now && _epicSchemaDbContext.RstOrderPayments.Any(p => p.OrderId == order.Id && p.Deleted == YesNo.NO
                            && p.TranClassify == TranClassifies.THANH_TOAN && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN)))))
                         && (input.OrderListStatus == null || input.OrderListStatus.Contains(order.Status))
                         && (input.StartDate == null || (order.DepositDate != null && order.DepositDate.Value.Date >= input.StartDate.Value.Date)
                            || (order.DepositDate == null && order.CreatedDate != null && order.CreatedDate.Value.Date >= input.StartDate.Value.Date))
                         && (input.EndDate == null || (order.DepositDate != null && order.DepositDate.Value.Date <= input.EndDate.Value.Date)
                            || (order.DepositDate == null && order.CreatedDate != null && order.CreatedDate.Value.Date <= input.EndDate.Value.Date))
                         && ((order.TradingProviderId == input.TradingProviderId && input.SaleReferralCode == order.SaleReferralCode)
                            || (input.ListTradingIds != null && input.ListTradingIds.Contains(order.TradingProviderId) && order.SaleReferralCodeSub == input.SaleReferralCode))
                         && investor.Deleted == YesNo.NO
                         select new AppRstOrderForSaleDto()
                         {
                             OrderId = order.Id,
                             ContractCode = order.ContractCode,
                             OrderStatus = order.Status,
                             Status = order.Status,
                             CifCode = order.CifCode,
                             BuyDate = order.CreatedDate,
                             ProductItemPrice = productItem.Price ?? 0,
                             DepositDate = order.DepositDate,
                             ProjectType = ProjectTypes.REAL_ESTATE,
                             AvatarImageUrl = (businessCustomer != null) ? businessCustomer.AvatarImageUrl : investor.AvatarImageUrl,
                             ReferralCode = (businessCustomer != null) ? businessCustomer.ReferralCodeSelf : investor.ReferralCodeSelf,
                             CustomerName = (businessCustomer != null) ? businessCustomer.Name :
                                        (_epicSchemaDbContext.InvestorIdentifications
                                        .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                                        .OrderByDescending(ii => ii.IsDefault)
                                        .ThenByDescending(ii => ii.Id)
                                        .Select(ii => ii.Fullname)
                                        .FirstOrDefault())
                         };
            return result.Where(o => (input.Keyword == null || o.CustomerName.ToLower().Contains(input.Keyword.ToLower())
                            || o.ReferralCode.ToLower().Contains(input.Keyword.ToLower())
                            || o.ContractCode.ToLower().Contains(input.Keyword.ToLower())));
        }

        public IQueryable<AppStatisticOrderBySale> AppGetAllStatisticOrderBySale(AppContractByListSaleFilterDto input)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(AppGetAllStatisticOrderBySale)}: input = {JsonSerializer.Serialize(input)}");
            // Lấy các thông tin hợp đồng của Sale bán được bao gồm cả bán hộ
            var result = from order in _dbSet
                         join productItem in _epicSchemaDbContext.RstProductItems on order.ProductItemId equals productItem.Id
                         where order.Deleted == YesNo.NO && (input.DepartmentId == null || (order.DepartmentId == input.DepartmentId))
                         && order.SaleReferralCode != null && (input.Status == null || order.Status == input.Status) && (input.OrderListStatus == null || input.OrderListStatus.Contains(order.Status))
                         && (input.StartDate == null || (order.DepositDate != null && order.DepositDate.Value.Date >= input.StartDate.Value.Date))
                         && (input.EndDate == null || (order.DepositDate != null && order.DepositDate.Value.Date <= input.EndDate.Value.Date))
                         && ((order.TradingProviderId == input.TradingProviderId && input.ListSaleReferralCode.Contains(order.SaleReferralCode))
                            || (input.ListTradingIds != null && input.ListTradingIds.Contains(order.TradingProviderId) && input.ListSaleReferralCode.Contains(order.SaleReferralCodeSub)))
                         select new AppStatisticOrderBySale()
                         {
                             OrderId = order.Id,
                             Status = order.Status,
                             CifCode = order.CifCode,
                             CreatedDate = order.CreatedDate,
                             ProductItemPrice = productItem.Price ?? 0,
                             InitTotalValue = productItem.Price ?? 0,
                             TotalValue = productItem.Price ?? 0,
                             DepositDate = order.DepositDate,
                             InvestDate = order.DepositDate,
                             ProjectType = ProjectTypes.REAL_ESTATE,
                         };
            return result;
        }


        /// <summary>
        ///  Tài sản của bất động sản của nhà đầu tư theo số tiền thanh toán được duyệt
        /// </summary>
        public decimal AssetRealEstateOrder(int investorId)
        {
            decimal sumOrderPayment = (from cifCode in _epicSchemaDbContext.CifCodes
                                       join order in _dbSet on cifCode.CifCode equals order.CifCode
                                       join orderPayment in _epicSchemaDbContext.RstOrderPayments on order.Id equals orderPayment.OrderId
                                       where cifCode.InvestorId == investorId && cifCode.Deleted == YesNo.NO && order.Deleted == YesNo.NO
                                       && orderPayment.Deleted == YesNo.NO && orderPayment.TranType == TranTypes.THU && orderPayment.TranClassify == TranClassifies.THANH_TOAN
                                       && orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN
                                       select orderPayment).Sum(p => p.PaymentAmount);
            return sumOrderPayment;
        }

        #region Mã hợp đồng
        /// <summary>
        /// Sinh mã hợp đồng
        /// </summary>
        /// <returns></returns>
        public string GenContractCode(OrderContractCodeDto input)
        {
            List<ConfigContractCodeDto> configContractCodes = new();
            var configContractCodeDetails = _epicSchemaDbContext.RstConfigContractCodeDetails.Where(d => d.ConfigContractCodeId == input.ConfigContractCodeId).OrderBy(o => o.SortOrder);

            string contractCode = null;
            foreach (var item in configContractCodeDetails)
            {
                string value = null;
                if (item.Key == ConfigContractCode.ORDER_ID)
                {
                    value = input.OrderId.ToString();
                }
                if (item.Key == ConfigContractCode.ORDER_ID_PREFIX_0)
                {
                    value = input.OrderId?.ToString().PadLeft(8, '0');
                }
                else if (item.Key == ConfigContractCode.PRODUCT_TYPE)
                {
                    value = input.ProductType.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.PRODUCT_CODE)
                {
                    value = input.ProductCode.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.PRODUCT_NAME)
                {
                    value = input.ProductName.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.POLICY_NAME)
                {
                    value = input.PolicyName.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.POLICY_CODE)
                {
                    value = input.PolicyCode.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.SHORT_NAME)
                {
                    value = input.ShortName.FirstLetterEachWord().ToUnSign() ?? input.ShortNameBusiness.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.FIX_TEXT)
                {
                    value = item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.DATE_DD && input.Now != null)
                {
                    value = input.Now.Value.ToString("dd");
                }
                else if (item.Key == ConfigContractCode.DATE_MM && input.Now != null)
                {
                    value = input.Now.Value.ToString("MM");
                }
                else if (item.Key == ConfigContractCode.DATE_YY && input.Now != null)
                {
                    value = input.Now.Value.ToString("yy");
                }
                else if (item.Key == ConfigContractCode.DATE_YYYY && input.Now != null)
                {
                    value = input.Now.Value.ToString("yyyy");
                }
                else if (item.Key == ConfigContractCode.DATE_DD_MM_YYYY && input.Now != null)
                {
                    value = input.Now.Value.ToString("ddMMyyyy");
                }
                else if (item.Key == ConfigContractCode.BUY_DATE && input.BuyDate != null)
                {
                    value = input.BuyDate.Value.ToString("ddMMyyyy");
                }
                else if (item.Key == ConfigContractCode.PAYMENT_FULL_DATE && input.PaymentFullDate != null)
                {
                    value = input.PaymentFullDate.Value.ToString("ddMMyyyy");
                }
                else if (item.Key == ConfigContractCode.INVEST_DATE && input.InvestDate != null)
                {
                    value = input.InvestDate.Value.ToString("ddMMyyyy");
                }
                else if (item.Key == ConfigContractCode.PROJECT_CODE && input.ProjectCode != null)
                {
                    value = item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.RST_PRODUCT_ITEM_CODE && input.RstProductItemCode != null)
                {
                    value = item.Value.ToUnSign();
                }
                configContractCodes.Add(new ConfigContractCodeDto { Key = item.Key, Value = value });
            }
            contractCode = ConfigContractCode.GenContractCode(configContractCodes);
            return contractCode;
        }
        #endregion
    }
}
