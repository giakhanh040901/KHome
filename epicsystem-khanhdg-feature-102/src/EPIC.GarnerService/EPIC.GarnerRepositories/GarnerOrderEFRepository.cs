using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.SaleAppStatistical;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EPIC.CoreRepositoryExtensions;
using EPIC.RepositoryExtensions;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Entities.Dto.RocketChat;
using MySqlX.XDevAPI.Common;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.InvestEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerDashboard;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.Entities.Dto.Sale;
using EPIC.Utils.Linq;
using EPIC.Entities.Dto.ManagerInvestor;

namespace EPIC.GarnerRepositories
{
    public class GarnerOrderEFRepository : BaseEFRepository<GarnerOrder>
    {
        public GarnerOrderEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerOrder.SEQ}")
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
        public GarnerOrder Add(GarnerOrder input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username} ");

            //kiểm tra trạng thái chính sách
            var policyFind = _epicSchemaDbContext.GarnerPolicies.FirstOrDefault(p => p.Id == input.PolicyId && p.Deleted == YesNo.NO);
            if (policyFind.Status == Status.INACTIVE)
            {
                ThrowException(ErrorCode.GarnerOrderPolicyStatusDeativate);
            }
            if (input.PolicyDetailId != null)
            {
                var policyDetailFind = _epicSchemaDbContext.GarnerPolicyDetails
                    .FirstOrDefault(p => p.Id == input.PolicyDetailId && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                    .ThrowIfNull(_epicSchemaDbContext, ErrorCode.GarnerPolicyNotFound);
                if (policyDetailFind.Status == Status.INACTIVE)
                {
                    ThrowException(ErrorCode.GarnerOrderPolicyDetailStatusDeativate);
                }
            }
            // Kiểm tra xem có vượt hạn mức tối thiểu trong chính sách
            if (input.TotalValue < policyFind.MinMoney)
            {
                ThrowException(ErrorCode.GarnerOrderCheckTotalValue);
            }

            //Kiểm tra thông tin khách hàng
            var findCifCode = _epicSchemaDbContext.CifCodes.FirstOrDefault(c => c.CifCode == input.CifCode && c.Deleted == YesNo.NO);
            if (findCifCode != null && findCifCode.InvestorId != null)
            {
                var findBankOfInvestor = _epicSchemaDbContext.InvestorBankAccounts.FirstOrDefault(b => b.Id == input.InvestorBankAccId && b.InvestorId == findCifCode.InvestorId && b.Deleted == YesNo.NO);
                if (findBankOfInvestor == null)
                {
                    ThrowException(ErrorCode.InvestorBankAccNotFound);
                }
            }
            else if (findCifCode != null && findCifCode.BusinessCustomerId != null)
            {
                var findBankOfBusiness = _epicSchemaDbContext.BusinessCustomerBanks.FirstOrDefault(b => b.BusinessCustomerBankAccId == input.BusinessCustomerBankAccId && b.BusinessCustomerId == findCifCode.BusinessCustomerId && b.Deleted == YesNo.NO);
                if (findBankOfBusiness == null)
                {
                    ThrowException(ErrorCode.CoreBusinessCustomerBankNotFound);
                }
            }

            var orderId = (long)NextKey();
            var result = _dbSet.Add(new GarnerOrder
            {
                Id = orderId,
                TradingProviderId = tradingProviderId,
                CifCode = input.CifCode,
                DepartmentId = input.DepartmentId,
                BuyDate = DateTime.Now,
                ProductId = input.ProductId,
                DistributionId = input.DistributionId,
                PolicyId = input.PolicyId,
                PolicyDetailId = input.PolicyDetailId,
                TotalValue = input.TotalValue,
                InitTotalValue = input.TotalValue,
                Source = input.Source == SourceOrder.ONLINE ? input.Source : SourceOrder.OFFLINE,
                Status = input.Status,
                ContractCode = ContractCode(orderId),
                TradingBankAccId = input.TradingBankAccId,
                BusinessCustomerBankAccId = input.BusinessCustomerBankAccId,
                InvestorBankAccId = input.InvestorBankAccId,
                SaleReferralCode = input.SaleReferralCode,
                InvestorIdenId = input.InvestorIdenId,
                ContractAddressId = input.ContractAddressId,
                SaleOrderId = input.SaleOrderId,
                SaleReferralCodeSub = input.SaleReferralCodeSub,
                DepartmentIdSub = input.DepartmentIdSub,
                DeliveryCode = _epicSchemaDbContext.FuncVerifyCodeGenerate(),
                CreatedDate = DateTime.Now,
                CreatedBy = username,
            });
            return result.Entity;
        }

        /// <summary>
        /// tìm kiếm theo Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrder FindById(long orderId, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: orderId = {orderId}");
            var order = _dbSet.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO && (tradingProviderId == null || o.TradingProviderId == tradingProviderId)
            && (partnerId == null || _epicSchemaDbContext.TradingProviderPartners.Where(t => t.PartnerId == partnerId).Select(o => o.TradingProviderId).Contains(o.TradingProviderId)));
            return order;
        }

        /// <summary>
        /// tìm kiếm theo delivery code
        /// </summary>
        /// <param name="deliveryCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerOrder FindByDeliveryCode(string deliveryCode, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindByDeliveryCode)}: deliveryCode = {deliveryCode}");
            var order = _dbSet.FirstOrDefault(o => o.DeliveryCode == deliveryCode && o.Deleted == YesNo.NO && (tradingProviderId == null || o.TradingProviderId == tradingProviderId));
            return order;
        }

        /// <summary>
        /// tìm tất cả order theo policyId với status >= 5 <=8 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public List<GarnerOrder> FindByPolicyId(int policyId)
        {
            _logger.LogInformation($"{nameof(FindByPolicyId)}: policyId = {policyId}");
            var order = _dbSet.Where(o => (o.Status >= OrderStatus.DANG_DAU_TU && o.Status <= OrderStatus.TAT_TOAN)
            && o.PolicyId == policyId && o.Deleted == YesNo.NO).ToList();
            return order;
        }

        /// <summary>
        /// Tìm kiếm phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<GarnerOrderMoreInfoDto> FindAll(FilterGarnerOrderDto input, int[] status, int? tradingProviderId = null, bool isGroupByCustomer = false, bool? isDeliveryStatus = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, isGroupByCustomer = {isGroupByCustomer}, isDeliveryStatus = {isDeliveryStatus}");

            PagingResult<GarnerOrderMoreInfoDto> result = new();

            var resultQuery = _epicSchemaDbContext.GarnerOrders.Include(order => order.CifCodes).ThenInclude(cifcode => cifcode.BusinessCustomer)
                                               .Include(order => order.CifCodes).ThenInclude(cifcode => cifcode.Investor).ThenInclude(investor => investor.InvestorIdentifications)
                                               .Include(order => order.Product)
                                               .Include(order => order.Distribution)
                                               .Include(order => order.Policy)
                                               .Include(order => order.PolicyDetail)
                                               .Include(order => order.OrderContracFiles)
                                               .Include(order => order.InvestorIdentification)
                                               .Where(order =>
                                                order.Deleted == YesNo.NO && status.Contains(order.Status)
                                                && ((input.TradingProviderIds != null && input.TradingProviderIds.Contains(order.TradingProviderId))
                                                    || (tradingProviderId != null && order.TradingProviderId == tradingProviderId))
                                                &&(input.ContractCodeGen == null || order.OrderContracFiles.Where(ocf => ocf.ContractCodeGen == input.ContractCodeGen).Any())
                                                && (input.ContractCode == null || (order.ContractCode.Contains(input.ContractCode) || order.OrderContracFiles.Where(ocf => ocf.ContractCodeGen == input.ContractCode && ocf.Deleted == YesNo.NO).Any()))
                                                && (input.Phone == null || ((order.CifCodes.Investor != null && order.CifCodes.Investor.Phone != null && order.CifCodes.Investor.Phone.Contains(input.Phone))
                                                                                || (order.CifCodes.BusinessCustomer != null && order.CifCodes.BusinessCustomer.Phone != null && order.CifCodes.BusinessCustomer.Phone.Contains(input.Phone))))
                                                && (input.IdNo == null || order.CifCodes.Investor.InvestorIdentifications.Where(iden => iden.IdNo.Contains(input.IdNo)).Any())
                                                && (input.TaxCode == null || order.CifCodes.BusinessCustomer.TaxCode == input.TaxCode)
                                                && (input.ProductType == null || order.Product.ProductType == input.ProductType)
                                                && (input.CifCode == null || order.CifCode.Contains(input.CifCode))
                                                && (input.BuyDate == null || order.BuyDate == input.BuyDate)
                                                && (input.DeliveryStatus == null || order.DeliveryStatus == input.DeliveryStatus)
                                                && (input.PendingDate == null || order.PendingDate.Value.Date == input.PendingDate)
                                                && (input.DeliveryDate == null || order.DeliveryDate.Value.Date == input.DeliveryDate)
                                                && (input.ReceivedDate == null || order.ReceivedDate.Value.Date == input.ReceivedDate)
                                                && (input.FinishedDate == null || order.FinishedDate.Value.Date == input.FinishedDate)
                                                && (input.Status == null || order.Status == input.Status)
                                                && (input.Source == null || order.Source == input.Source)
                                                && (input.DistributionId == null || order.DistributionId == input.DistributionId)
                                                && (input.PolicyId == null || order.PolicyId == input.PolicyId)
                                                && (input.DeliveryStatus == null || order.DeliveryStatus == input.DeliveryStatus)
                                                && (input.SettlementDate == null || (order.SettlementDate != null && order.SettlementDate.Value.Date == input.SettlementDate.Value.Date))
                                                && (isDeliveryStatus == null || order.DeliveryStatus != null)
                                               )
                                               .Select(order => new GarnerOrderMoreInfoDto()
                                               {
                                                   Id = order.Id,
                                                   TradingProviderId = order.TradingProviderId,
                                                   CifCode = order.CifCode,
                                                   DepartmentId = order.DepartmentId,
                                                   ProductId = order.ProductId,
                                                   DistributionId = order.DistributionId,
                                                   PolicyId = order.PolicyId,
                                                   Policy = order.Policy == null ? null : new GarnerEntities.Dto.GarnerPolicy.GarnerPolicyMoreInfoDto
                                                   {
                                                       Code = order.Policy.Code,
                                                       Name = order.Policy.Name,
                                                       StartDate = order.Policy.StartDate,
                                                       EndDate = order.Policy.EndDate,
                                                       SortOrder = order.Policy.SortOrder,
                                                   },
                                                   PolicyDetailId = order.PolicyDetailId,
                                                   Product = order.Product == null ? null : new GarnerProductDto()
                                                   {
                                                       Id = order.Product.Id,
                                                       Code = order.Product.Code,
                                                       CountType = order.Product.CountType,
                                                       CpsDepositProviderId = order.Product.CpsDepositProviderId,
                                                       CpsInterestPeriodUnit = order.Product.CpsInterestPeriodUnit,
                                                       CpsInterestPeriod = order.Product.CpsInterestPeriod,
                                                       CpsInterestRate = order.Product.CpsInterestRate,
                                                       CpsInterestRateType = order.Product.CpsInterestRateType,
                                                       CpsIsAllowSBD = order.Product.CpsIsAllowSBD,
                                                       CpsIsListing = order.Product.CpsIsListing,
                                                       CpsIssuerId = order.Product.CpsIssuerId,
                                                       CpsNumberClosePer = order.Product.CpsNumberClosePer,
                                                       CpsParValue = order.Product.CpsParValue,
                                                       CpsPeriod = order.Product.CpsPeriod,
                                                       CpsPeriodUnit = order.Product.CpsPeriodUnit,
                                                       CpsQuantity = order.Product.CpsQuantity,
                                                       EndDate = order.Product.EndDate,
                                                       Name = order.Product.Name,
                                                       StartDate = order.Product.StartDate,
                                                       Status = order.Product.Status,
                                                       ProductType = order.Product.ProductType
                                                   },
                                                   TotalValue = order.TotalValue,
                                                   InitTotalValue = order.InitTotalValue,
                                                   BuyDate = order.BuyDate,
                                                   Status = order.Status,
                                                   Source = order.Source,
                                                   ContractCode = order.ContractCode,
                                                   TradingBankAccId = order.TradingBankAccId,
                                                   BusinessCustomerBankAccId = order.BusinessCustomerBankAccId,
                                                   PaymentFullDate = order.PaymentFullDate,
                                                   InvestorBankAccId = order.InvestorBankAccId,
                                                   SaleReferralCode = order.SaleReferralCode,
                                                   DeliveryStatus = order.DeliveryStatus,
                                                   IpAddressCreated = order.IpAddressCreated,
                                                   DeliveryCode = order.DeliveryCode,
                                                   ActiveDate = order.ActiveDate,
                                                   InvestDate = order.InvestDate,
                                                   InvestorIdenId = (int) order.InvestorIdenId,
                                                   SettlementDate = order.SettlementDate,
                                                   SaleOrderId = order.SaleOrderId,
                                                   PendingDate = order.PendingDate,
                                                   DeliveryDate = order.DeliveryDate,
                                                   ReceivedDate = order.ReceivedDate,
                                                   FinishedDate = order.FinishedDate,
                                                   PendingDateModifiedBy = order.PendingDateModifiedBy,
                                                   DeliveryDateModifiedBy = order.DeliveryDateModifiedBy,
                                                   ReceivedDateModifiedBy = order.ReceivedDateModifiedBy,
                                                   FinishedDateModifiedBy = order.FinishedDateModifiedBy,
                                                   DepartmentIdSub = order.DepartmentIdSub,
                                                   SaleReferralCodeSub = order.SaleReferralCodeSub,
                                                   RenewalsPolicyDetailId = order.RenewalsPolicyDetailId,
                                                   RenewalsPolicyId = order.RenewalsPolicyId,
                                                   SettlementMethod = order.SettlementMethod,
                                                   ApproveBy = order.ApproveBy,
                                                   ApproveDate = order.ApproveDate,
                                                   CustomerName = order.CifCodes.BusinessCustomer == null ? (order.CifCodes.Investor.Name ?? order.InvestorIdentification.Fullname) : order.CifCodes.BusinessCustomer.Name,
                                                   Investor = order.CifCodes.Investor == null ? null : new InvestorDto()
                                                   {
                                                       InvestorId = order.CifCodes.Investor.InvestorId,
                                                       Email = order.CifCodes.Investor.Email,
                                                       Name = order.CifCodes.Investor.Name,
                                                       InvestorIdentification = order.CifCodes.Investor.InvestorIdentifications.FirstOrDefault() == null

                                                            ? new Entities.Dto.Investor.InvestorIdentificationDto()
                                                            {
                                                                CreatedBy = order.InvestorIdentification.CreatedBy,
                                                                CreatedDate = order.InvestorIdentification.CreatedDate,
                                                                Fullname = order.InvestorIdentification.Fullname,
                                                            }
                                                            : new Entities.Dto.Investor.InvestorIdentificationDto()
                                                            {
                                                                CreatedBy = order.CifCodes.Investor.InvestorIdentifications.FirstOrDefault().CreatedBy,
                                                                CreatedDate = order.CifCodes.Investor.InvestorIdentifications.FirstOrDefault().CreatedDate,
                                                                Fullname = order.CifCodes.Investor.InvestorIdentifications.FirstOrDefault().Fullname,
                                                            }
                                                   },
                                                   BusinessCustomer = order.CifCodes.BusinessCustomer == null ? null : new BusinessCustomerDto()
                                                   {
                                                       BusinessCustomerId = order.CifCodes.BusinessCustomer.BusinessCustomerId,
                                                       Code = order.CifCodes.BusinessCustomer.Code,
                                                       Name = order.CifCodes.BusinessCustomer.Name,
                                                       ShortName = order.CifCodes.BusinessCustomer.ShortName,
                                                       Address = order.CifCodes.BusinessCustomer.Address,
                                                       Phone = order.CifCodes.BusinessCustomer.Phone,
                                                       Email = order.CifCodes.BusinessCustomer.Email,
                                                       TaxCode = order.CifCodes.BusinessCustomer.TaxCode,
                                                   },
                                                   Orderer = (order.Source == SourceOrder.OFFLINE && order.SaleOrderId == null) ? SourceOrderFE.QUAN_TRI_VIEN 
                                                                : ((order.Source == SourceOrder.OFFLINE && order.SaleOrderId != null) ? SourceOrderFE.SALE 
                                                                    : ((order.Source == SourceOrder.ONLINE) ? SourceOrderFE.KHACH_HANG : null)),
                                                   InitTotalValueGroup = _epicSchemaDbContext.GarnerOrders.Where(o => o.PolicyId == order.PolicyId && o.CifCode == order.CifCode 
                                                                                                                && o.TradingProviderId == order.TradingProviderId && o.Deleted == YesNo.NO
                                                                                                                && (o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.PHONG_TOA))
                                                                                                           .Sum(o => o.InitTotalValue),
                                                   TotalValueGroup = _epicSchemaDbContext.GarnerOrders.Where(o => o.PolicyId == order.PolicyId && o.CifCode == order.CifCode
                                                                                                                && o.TradingProviderId == order.TradingProviderId && o.Deleted == YesNo.NO
                                                                                                                && (o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.PHONG_TOA))
                                                                                                           .Sum(o => o.TotalValue),
                                                });

            //Lọc theo nguồn đặt lệnh
            if (input.Orderer == SourceOrderFE.QUAN_TRI_VIEN)
            {
                resultQuery = resultQuery.Where(e => e.Source == SourceOrder.OFFLINE);
            }
            else if (input.Orderer == SourceOrderFE.KHACH_HANG)
            {
                resultQuery = resultQuery.Where(e => e.Source == SourceOrder.ONLINE && e.SaleOrderId == null);
            }
            else if (input.Orderer == SourceOrderFE.SALE)
            {
                resultQuery = resultQuery.Where(e => e.Source == SourceOrder.OFFLINE && e.SaleOrderId != null);
            }

            resultQuery = resultQuery.OrderByDescending(o => o.InvestDate).ThenByDescending(o => o.Id);
            resultQuery = resultQuery.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                if (isGroupByCustomer)
                {
                    var orderList = resultQuery.Select(o => new { o.Id, o.PolicyId, o.CifCode }).ToList();
                    var groupOrder = orderList.GroupBy(o => new { o.CifCode, o.PolicyId });
                    // đếm theo cha
                    result.TotalItems = groupOrder.Count();
                    groupOrder = groupOrder.Skip(input.Skip).Take(input.PageSize);
                    var listOrderId = groupOrder.SelectMany(o => o.Select(oo => oo.Id)).ToList();

                    resultQuery = resultQuery.Where(o => listOrderId.Contains(o.Id));
                }
                else
                {
                    // đếm tổng trước khi phân trang
                    result.TotalItems = resultQuery.Count();
                    resultQuery = resultQuery.Skip(input.Skip).Take(input.PageSize);
                }
            }

            result.Items = resultQuery;
            return result;
        }

        /// <summary>
        /// Lấy danh sách hành động đặt lệnh gần đây của người dùng | Dashboard
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <returns></returns>
        public List<GarnderDashboardActionsDto> DashboardGetNewAction(List<int> tradingProviderIds)
        {
            var query = (from order in _epicSchemaDbContext.GarnerOrders.AsNoTracking().Where(x => tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId) && x.Deleted == YesNo.NO)
                         from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.Id == order.InvestorIdenId && x.Deleted == YesNo.NO && x.IsDefault == YesNo.YES).DefaultIfEmpty()
                         from investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == iden.InvestorId && x.Deleted == YesNo.NO)
                         where new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN }.Contains(order.Status)
                         orderby order.CreatedDate descending
                         select new GarnderDashboardActionsDto
                         {
                             Action = GarnerDashboardAction.DAT_LENH_DAU_TU_MOI,
                             Avatar = investor.AvatarImageUrl,
                             Fullname = iden.Fullname,
                             CreatedDate = order.CreatedDate,
                             OrderId = order.Id,
                         }).Skip(0).Take(10);
            return query.ToList();
                        
        }

        /// <summary>
        /// Tìm kiếm theo mã hợp đồng
        /// </summary>
        /// <param name="contractCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerOrder FindByContractCode(string contractCode, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(FindByContractCode)}: contractCode = {contractCode}; tradingProviderId = {tradingProviderId}");
            return _dbSet.FirstOrDefault(o => o.ContractCode == contractCode && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm theo CifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerOrder> FindByCifCode(string cifCode, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerOrderEFRepository)}->{nameof(FindByCifCode)}: cifCode = {cifCode}; tradingProviderId = {tradingProviderId}");
            return _dbSet.Where(o => o.CifCode == cifCode && (tradingProviderId == null || o.TradingProviderId == tradingProviderId) && o.Deleted == YesNo.NO).ToList();
        }

        /// <summary>
        /// Duyệt hợp đồng, duyệt cả lệnh online
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public GarnerOrder OrderApprove(long orderId, int tradingProviderId, string modifiedBy, bool isAutoActive = false)
        {
            _logger.LogInformation($"{nameof(OrderApprove)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, modifiedBy = {modifiedBy}");

            //tìm order
            var orderFind = _dbSet.FirstOrDefault(o => o.TradingProviderId == tradingProviderId && o.Id == orderId && o.Deleted == YesNo.NO);
            if (orderFind == null)
            {
                ThrowException(ErrorCode.GarnerOrderNotFound, orderId);
            }
            modifiedBy ??= _epicSchemaDbContext.GetUserByCifCode(orderFind.CifCode);

            // Nếu là duyệt trên CMS thì kiểm tra trạng thái và nguồn đặt online hoặc sale đặt
            // Tự động active thì ko check
            if (!isAutoActive && !(orderFind.Status == OrderStatus.CHO_DUYET_HOP_DONG
                || ((orderFind.Source == SourceOrder.ONLINE || (orderFind.Source == SourceOrder.OFFLINE && orderFind.SaleOrderId != null))
                    && (orderFind.Status == OrderStatus.CHO_THANH_TOAN || orderFind.Status == OrderStatus.CHO_KY_HOP_DONG))
            ))
            {
                ThrowException(ErrorCode.GarnerOrderCanNotApprove);
            }
            // check nếu hợp đồng là offline và trạng thái giao nhận hợp đồng là null thì khi duyệt sẽ chuyển trạng thái giao nhận hợp đồng
            if (orderFind.Source == SourceOrder.OFFLINE && orderFind.DeliveryStatus == null)
            {
                orderFind.DeliveryStatus = DeliveryStatus.WAITING;
                orderFind.PendingDate = DateTime.Now;
                orderFind.PendingDateModifiedBy = modifiedBy;
            }
            //Tính tổng thanh toán được duyệt
            var listOrderPayment = _epicSchemaDbContext.GarnerOrderPayments.Where(o => o.OrderId == orderId && o.Status == OrderPaymentStatus.DA_THANH_TOAN && o.TranType == TranTypes.THU && o.Deleted == YesNo.NO).ToList();
            var totalPaymentValue = listOrderPayment.Sum(p => p.PaymentAmount);
            if (totalPaymentValue != orderFind.TotalValue) //check số tiền đã thanh toán có bằng total value
            {
                ThrowException(ErrorCode.GarnerOrderApproveCheckTotalValue);
            }
            DateTime? maxTransDate = listOrderPayment.Max(p => p.TranDate);
            orderFind.PaymentFullDate = maxTransDate;
            orderFind.InvestDate = maxTransDate;
            orderFind.Status = OrderStatus.DANG_DAU_TU;
            orderFind.ApproveBy = modifiedBy;
            orderFind.ApproveDate = DateTime.Now;
            orderFind.ModifiedBy = modifiedBy;
            orderFind.ModifiedDate = DateTime.Now;
            orderFind.ActiveDate = DateTime.Now;
            return orderFind;
        }

        /// <summary>
        /// Cập nhật order
        /// </summary>
        public GarnerOrder Update(GarnerOrder entity, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Update)}: entity = {JsonSerializer.Serialize(entity)}");
            var orderFind = _dbSet.FirstOrDefault(o => o.Id == entity.Id && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO);
            if (orderFind == null)
            {
                ThrowException(ErrorCode.GarnerOrderNotFound);
            }
            GarnerPolicyDetail policyDetailFind = null;
            if (entity.PolicyDetailId != null)
            {
                policyDetailFind = _epicSchemaDbContext.GarnerPolicyDetails
                    .FirstOrDefault(p => p.Id == entity.PolicyDetailId && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                    .ThrowIfNull(_epicSchemaDbContext, ErrorCode.GarnerPolicyNotFound);
                if (policyDetailFind.Status == Status.INACTIVE)
                {
                    ThrowException(ErrorCode.GarnerOrderPolicyDetailStatusDeativate);
                }
            }
            if (orderFind != null)
            {
                orderFind.TotalValue = entity.TotalValue;
                orderFind.PolicyId = entity.PolicyId;
                orderFind.PolicyDetailId = policyDetailFind?.Id;
                orderFind.SaleReferralCode = entity.SaleReferralCode;
                orderFind.DepartmentId = entity.DepartmentId;
                orderFind.InvestorBankAccId = entity.InvestorBankAccId;
                orderFind.ContractAddressId = entity.ContractAddressId;
                orderFind.ModifiedBy = username;
                orderFind.ModifiedDate = DateTime.Now;
            }
            return orderFind;
        }

        public void UpdateStatusReceive(long orderId, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(UpdateStatusReceive)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, username = {username}");
            var orderFind = _dbSet.FirstOrDefault(o => o.Id == orderId && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO);
            if (orderFind != null)
            {
                orderFind.DeliveryStatus = DeliveryStatus.RECEIVE;
                orderFind.ModifiedBy = username;
                orderFind.ModifiedDate = DateTime.Now;
            }
        }

        /// <summary>
        /// xóa order ở trạng thái khởi tạo
        /// </summary>
        public GarnerOrder Deleted(long orderId, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Deleted)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, username = {username}");
            var orderFind = _dbSet.FirstOrDefault(o => o.Id == orderId && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.GarnerOrderNotFound);

            if (orderFind.Status == OrderStatus.KHOI_TAO || orderFind.Status == OrderStatus.CHO_THANH_TOAN)
            {
                orderFind.ModifiedBy = username;
                orderFind.ModifiedDate = DateTime.Now;
                orderFind.Deleted = YesNo.YES;
            }
            else
            {
                ThrowException(ErrorCode.GarnerOrderStatusNotInitialState);
            }
            // Xem có trường hợp đã thanh toán nào không
            if (_epicSchemaDbContext.GarnerOrderPayments.Where(p => p.OrderId == orderId && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO).Any())
            {
                ThrowException(ErrorCode.GarnerOrderExistPaymentNotDeleted);
            }
            return orderFind;
        }

        public List<int> GetListPolicyIdByCifCode(string cifCode, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GetListPolicyIdByCifCode)}: cifCode = {cifCode}; tradingProviderId = {tradingProviderId}");

            return _dbSet.Where(o => o.CifCode == cifCode && o.Status > OrderStatus.KHOI_TAO && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO).Select(o => o.PolicyId).Distinct().ToList();
        }

        public List<GarnerOrder> FindOrderByPolicy(int policyId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindOrderByPolicy)}: policyId = {policyId}, tradingProviderId = {tradingProviderId}");

            var garnerOrder = _dbSet.Where(r => r.Deleted == YesNo.NO &&
                                    (r.Status == OrderStatus.DANG_DAU_TU || r.Status == OrderStatus.PHONG_TOA)
                                    && r.PolicyId == policyId);
            var result = new List<GarnerOrder>();
            foreach (var order in garnerOrder)
            {
                result.Add(order);
            }
            return result;
        }

        /// <summary>
        /// Số tiền đã đầu tư vào phân phối ở 2 trạng thái active và chờ duyệt hợp đồng
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public decimal SumValue(int distributionId, int? tradingProviderId)
        {
            decimal result = _dbSet.Where(o => o.DistributionId == distributionId && (tradingProviderId == null || o.TradingProviderId == tradingProviderId)
                                    && (o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.CHO_DUYET_HOP_DONG) && o.Deleted == YesNo.NO)
                                    .Select(o => o.TotalValue).Sum();
            return result;
        }
        #endregion

        #region App
        public AppGarnerOrderDto AppInvestorOrderAdd(GarnerOrder input, int investorId, string username, bool isCheck = false)
        {
            _logger.LogInformation($"{nameof(AppInvestorOrderAdd)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}, username = {username} ");

            // Kiểm tra thông tin khách hàng
            var findCifCode = _epicSchemaDbContext.CifCodes.FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO);
            if (findCifCode == null)
            {
                ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            // Kiểm tra thông tin ngân hàng thụ hưởng của khách hàng
            var findBankOfInvestor = _epicSchemaDbContext.InvestorBankAccounts.FirstOrDefault(b => b.Id == input.InvestorBankAccId && b.InvestorId == findCifCode.InvestorId && b.Deleted == YesNo.NO);
            if (findBankOfInvestor == null)
            {
                ThrowException(ErrorCode.InvestorBankAccNotFound);
            }


            //kiểm tra trạng thái chính sách
            var policyFind = _epicSchemaDbContext.GarnerPolicies.FirstOrDefault(p => p.Id == input.PolicyId && p.Deleted == YesNo.NO);
            if (policyFind.Status == Status.INACTIVE)
            {
                ThrowException(ErrorCode.GarnerOrderPolicyStatusDeativate);
            }

            var findDistribution = _epicSchemaDbContext.GarnerDistributions.FirstOrDefault(p => p.Id == policyFind.DistributionId && p.Deleted == YesNo.NO);
            if (findDistribution == null)
            {
                ThrowException(ErrorCode.GarnerDistributionNotFound);
            }
            // Phân phối sản phẩm có đóng không 
            if (findDistribution.IsClose == YesNo.YES)
            {
                ThrowException(ErrorCode.GarnerDistributionIsClose);
            }

            // Tổng giá trị hợp đồng đã đầu tư trước đó
            var totalValueOrderActive = _dbSet.Where(o => o.PolicyId == input.PolicyId && o.CifCode == findCifCode.CifCode && o.Status == OrderStatus.DANG_DAU_TU && o.Deleted == YesNo.NO).Sum(o => o.TotalValue);
            // Kiểm tra xem có vượt hạn mức tối đa trong chính sách (nếu có)
            if (policyFind.MaxMoney != null && (totalValueOrderActive + input.TotalValue > policyFind.MaxMoney))
            {
                ThrowException(ErrorCode.GarnerOrderCheckTotalValueBiggerMaxMoneyPolicy);
            }
            // Kiểm tra xem có vượt hạn mức tối thiểu trong chính sách
            if (input.TotalValue < policyFind.MinMoney)
            {
                ThrowException(ErrorCode.GarnerOrderCheckTotalValue);
            }
            AppGarnerOrderDto result = new();

            // Nếu là kiểm tra thì không lưu
            if (!isCheck)
            {
                var orderId = (long)NextKey();
                var insertOrder = _dbSet.Add(new GarnerOrder
                {
                    Id = orderId,
                    TradingProviderId = input.TradingProviderId,
                    CifCode = findCifCode.CifCode,
                    DepartmentId = input.DepartmentId,
                    ProductId = input.ProductId,
                    DistributionId = input.DistributionId,
                    PolicyId = input.PolicyId,
                    PolicyDetailId = input.PolicyDetailId,
                    TotalValue = input.TotalValue,
                    InitTotalValue = input.TotalValue,
                    Source = input.Source == 0 ? SourceOrder.ONLINE : input.Source,
                    ContractCode = ContractCode(orderId),
                    Status = input.Status,
                    TradingBankAccId = input.TradingBankAccId,
                    BusinessCustomerBankAccId = input.BusinessCustomerBankAccId,
                    InvestorBankAccId = input.InvestorBankAccId,
                    SaleReferralCode = input.SaleReferralCode,
                    InvestorIdenId = input.InvestorIdenId,
                    ContractAddressId = input.ContractAddressId,
                    SaleOrderId = input.SaleOrderId,
                    SaleReferralCodeSub = input.SaleReferralCodeSub,
                    DepartmentIdSub = input.DepartmentIdSub,
                    DeliveryCode = _epicSchemaDbContext.FuncVerifyCodeGenerate(),
                    CreatedDate = input.CreatedDate,
                    BuyDate = DateTime.Now,
                    CreatedBy = username,
                });

                // Đổ thông tin ra ngoài sau khi đặt lệnh xong
                result.PaymentNote = PaymentNotes.THANH_TOAN + insertOrder.Entity.ContractCode;
                result.Id = orderId;
                result.TotalValue = input.TotalValue;
                result.TradingProviderId = insertOrder.Entity.TradingProviderId;
                result.ContractCode = insertOrder.Entity.ContractCode;
                result.DistributionId = insertOrder.Entity.DistributionId;
                result.TradingBankAccId = insertOrder.Entity.TradingBankAccId;
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách hợp đồng của nhà đầu tư
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="groupOrder"></param>
        /// <returns></returns>
        public List<AppGarnerPolicyGroupOrderDto> AppGetListOrder(int investorId, int? groupOrder)
        {
            _logger.LogInformation($"{nameof(AppGetListOrder)}: investorId = {investorId}, groupOrder = {groupOrder} ");

            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                ThrowException(ErrorCode.CoreCifCodeNotFound);
            }

            var result = new List<AppGarnerPolicyGroupOrderDto>();

            //Lọc danh sách hợp đồng theo chính sách
            var groupPolicy = (from order in _dbSet.Where(o => o.CifCode == cifCodeFind.CifCode)
                               join policy in _epicSchemaDbContext.GarnerPolicies on order.PolicyId equals policy.Id
                               join distribution in _epicSchemaDbContext.GarnerDistributions on policy.DistributionId equals distribution.Id
                               join product in _epicSchemaDbContext.GarnerProducts on distribution.ProductId equals product.Id
                               join tradingProvider in _epicSchemaDbContext.TradingProviders on distribution.TradingProviderId equals tradingProvider.TradingProviderId
                               join businessCustomer in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                               where (groupOrder == AppOrderGroupStatus.DANG_DAU_TU && GroupStatusOrders.GroupStatus[AppOrderGroupStatus.DANG_DAU_TU].Contains(order.Status))

                               //Xem lịch sử sổ lệnh chưa thanh toán hoặc chưa rút
                               || (groupOrder == AppOrderGroupStatus.SO_LENH && (GroupStatusOrders.GroupStatus[AppOrderGroupStatus.SO_LENH].Contains(order.Status)
                                    //hoặc đang đầu tư nhưng có lệnh rút chưa duyệt
                                    || (order.Status == OrderStatus.DANG_DAU_TU &&
                                        (from withdrawalDetail in _epicSchemaDbContext.GarnerWithdrawalDetails
                                         join withdrawal in _epicSchemaDbContext.GarnerWithdrawals on withdrawalDetail.WithdrawalId equals withdrawal.Id
                                         where withdrawal.CifCode == cifCodeFind.CifCode && withdrawalDetail.OrderId == order.Id && (withdrawal.Status == WithdrawalStatus.YEU_CAU || withdrawal.Status == WithdrawalStatus.CHO_PHAN_HOI)
                                         select withdrawal.Status).Any()
                                    )))

                               // Xem lịch sử đầu tư
                               || (groupOrder == AppOrderGroupStatus.DA_TAT_TOAN && (GroupStatusOrders.GroupStatus[AppOrderGroupStatus.DA_TAT_TOAN].Contains(order.Status)
                                    //hoặc đang đầu tư nhưng có lệnh rút được duyệt
                                    || ((order.Status == OrderStatus.DANG_DAU_TU || order.Status == OrderStatus.TAT_TOAN) &&
                                        (from withdrawalDetail in _epicSchemaDbContext.GarnerWithdrawalDetails
                                         join withdrawal in _epicSchemaDbContext.GarnerWithdrawals on withdrawalDetail.WithdrawalId equals withdrawal.Id
                                         where withdrawal.CifCode == cifCodeFind.CifCode && withdrawalDetail.OrderId == order.Id && (withdrawal.Status == WithdrawalStatus.DUYET_DI_TIEN || withdrawal.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                                         select withdrawal.Status).Any()
                                    )))

                               && order.Deleted == YesNo.NO && policy.Id == order.PolicyId && order.DistributionId == distribution.Id
                               && policy.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO && product.Deleted == YesNo.NO && tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                               orderby order.Id descending
                               select new AppGarnerPolicyGroupOrderDto()
                               {
                                   PolicyId = policy.Id,
                                   PolicyName = policy.Name,
                                   ProductIcon = product.Icon,
                                   TradingProviderName = businessCustomer.Name
                               })
                         .ToList();

            result = groupPolicy.GroupBy(p => p.PolicyId).Select(pl => pl.First()).ToList();
            // Từ các chính sách lấy ra các hợp đồng sổ lệnh
            foreach (var item in result)
            {
                var orderItem = new List<GarnerOrder>();
                var selectOrder = _dbSet.Where(o => o.CifCode == cifCodeFind.CifCode
                                    && (groupOrder == AppOrderGroupStatus.DANG_DAU_TU && GroupStatusOrders.GroupStatus[AppOrderGroupStatus.DANG_DAU_TU].Contains(o.Status)

                                    || (groupOrder == AppOrderGroupStatus.SO_LENH && (GroupStatusOrders.GroupStatus[AppOrderGroupStatus.SO_LENH].Contains(o.Status)
                                        //hoặc đang đầu tư nhưng có lệnh rút chưa duyệt
                                        /*|| (o.Status == OrderStatus.DANG_DAU_TU &&
                                            (from withdrawalDetail in _epicSchemaDbContext.GarnerWithdrawalDetails
                                             join withdrawal in _epicSchemaDbContext.GarnerWithdrawals on withdrawalDetail.WithdrawalId equals withdrawal.Id
                                             where withdrawal.CifCode == cifCodeFind.CifCode && withdrawalDetail.Id == o.Id && (withdrawal.Status == WithdrawalStatus.YEU_CAU || withdrawal.Status == WithdrawalStatus.CHO_PHAN_HOI)
                                             select withdrawal.Status).Any()
                                        )*/
                                        ))
                                    || (groupOrder == AppOrderGroupStatus.DA_TAT_TOAN && (GroupStatusOrders.GroupStatus[AppOrderGroupStatus.DA_TAT_TOAN].Contains(o.Status)
                                    //hoặc đang đầu tư nhưng có lệnh rút được duyệt
                                    || ((o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.TAT_TOAN) &&
                                        (from withdrawalDetail in _epicSchemaDbContext.GarnerWithdrawalDetails
                                         join withdrawal in _epicSchemaDbContext.GarnerWithdrawals on withdrawalDetail.WithdrawalId equals withdrawal.Id
                                         where withdrawal.CifCode == cifCodeFind.CifCode && withdrawalDetail.OrderId == o.Id && (withdrawal.Status == WithdrawalStatus.DUYET_DI_TIEN || withdrawal.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                                         select withdrawal.Status).Any()
                                    )))
                                     ) && o.PolicyId == item.PolicyId && o.Deleted == YesNo.NO).ToList();
                item.Orders = selectOrder;
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách của sổ lệnh cyar khách hàng cá nhân
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<GarnerOrder> AppInvestorGetAllOrder(int investorId, int status)
        {
            _logger.LogInformation($"{nameof(AppInvestorGetAllOrder)}: investorId = {investorId}, status = {status} ");

            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                ThrowException(ErrorCode.CoreCifCodeNotFound);
            }

            return _dbSet.Where(o => o.CifCode == cifCodeFind.CifCode && o.Status == status && o.Deleted == YesNo.NO).ToList();
        }
        /// <summary>
        /// Xem chi tiết hợp đồng sổ lệnh
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrder AppOrderDetail(int investorId, long orderId)
        {
            _logger.LogInformation($"{nameof(AppOrderDetail)}: investorId = {investorId}, orderId = {orderId} ");

            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                ThrowException(ErrorCode.CoreCifCodeNotFound);
            }

            return _dbSet.FirstOrDefault(o => o.Id == orderId && o.CifCode == cifCodeFind.CifCode && o.Deleted == YesNo.NO);
        }

        public GarnerOrder ProcessContract(long orderId, string username, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(ProcessContract)}: orderId = {orderId}, username = {username}, tradingProviderId = {tradingProviderId}");
            var orderFind = _dbSet.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO && (tradingProviderId == null || o.TradingProviderId == tradingProviderId) && o.DeliveryStatus == null);

            if (orderFind != null)
            {
                orderFind.DeliveryStatus = DeliveryStatus.WAITING;
                orderFind.PendingDate = DateTime.Now;
                orderFind.PendingDateModifiedBy = username;
            }
            return orderFind;
        }
        #endregion

        #region Update các trạng thái hợp đồng
        /// <summary>
        /// đổi trạng thái chờ xử lý sang đang giao
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatusDelivered(long orderId, int tradingProviderId, string modifiedBy)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatusDelivered)}: orderId = {orderId}, tradingProviderId = {tradingProviderId},  modifiedBy = {modifiedBy}");

            var deliveryOderFind = _dbSet.FirstOrDefault(e => e.Id == orderId && e.TradingProviderId == tradingProviderId && e.Status == OrderStatus.DANG_DAU_TU && e.Deleted == YesNo.NO);

            if (deliveryOderFind != null && deliveryOderFind.DeliveryStatus == DeliveryStatus.WAITING)
            {
                deliveryOderFind.DeliveryStatus = DeliveryStatus.DELIVERY;
                deliveryOderFind.DeliveryDate = DateTime.Now;
                deliveryOderFind.DeliveryDateModifiedBy = modifiedBy;
            }
            return deliveryOderFind;
        }

        /// <summary>
        /// Đổi trạng thái từ đang giao sang đã nhận
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatusReceived(long orderId, int tradingProviderId, string modifiedBy)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatusReceived)}: orderId = {orderId}, tradingProviderId = {tradingProviderId},  modifiedBy = {modifiedBy}");

            var deliveryOderFind = _dbSet.FirstOrDefault(e => e.Id == orderId && e.TradingProviderId == tradingProviderId && e.Status == OrderStatus.DANG_DAU_TU && e.Deleted == YesNo.NO);

            if (deliveryOderFind != null && deliveryOderFind.DeliveryStatus == DeliveryStatus.DELIVERY)
            {
                deliveryOderFind.DeliveryStatus = DeliveryStatus.RECEIVE;
                deliveryOderFind.ReceivedDate = DateTime.Now;
                deliveryOderFind.ReceivedDateModifiedBy = modifiedBy;
            }
            return deliveryOderFind;
        }

        /// <summary>
        /// Đổi trạng thái từ đã nhận sang hoàn thành
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatusDone(long orderId, int tradingProviderId, string modifiedBy)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatusDone)}: orderId = {orderId}, tradingProviderId = {tradingProviderId},  modifiedBy = {modifiedBy}");

            var deliveryOderFind = _dbSet.FirstOrDefault(e => e.Id == orderId && e.TradingProviderId == tradingProviderId && e.Status == OrderStatus.DANG_DAU_TU && e.Deleted == YesNo.NO);

            if (deliveryOderFind != null && deliveryOderFind.DeliveryStatus == DeliveryStatus.RECEIVE)
            {
                deliveryOderFind.DeliveryStatus = DeliveryStatus.COMPLETE;
                deliveryOderFind.FinishedDate = DateTime.Now;
                deliveryOderFind.FinishedDateModifiedBy = modifiedBy;
            }
            return deliveryOderFind;
        }


        /// <summary>
        /// Thay đổi trạng thái giao nhận hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatus(long orderId, string modifiedBy, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatus)}: orderId = {orderId}, modifiedBy = {modifiedBy}, tradingProviderId = {tradingProviderId}");

            var deliveryOderFind = _dbSet.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO && (tradingProviderId == null || o.TradingProviderId == tradingProviderId)
            && o.Status == OrderStatus.DANG_DAU_TU);

            if (deliveryOderFind == null)
            {
                ThrowException(ErrorCode.GarnerOrderNotFound);
            }

            if (deliveryOderFind != null)
            {
                if (deliveryOderFind.DeliveryStatus == null)
                {
                    deliveryOderFind.DeliveryStatus = DeliveryStatus.WAITING;
                    deliveryOderFind.PendingDate = DateTime.Now;
                    deliveryOderFind.PendingDateModifiedBy = modifiedBy;
                }
                else if (deliveryOderFind.DeliveryStatus == DeliveryStatus.WAITING)
                {
                    deliveryOderFind.DeliveryStatus = DeliveryStatus.DELIVERY;
                    deliveryOderFind.DeliveryDate = DateTime.Now;
                    deliveryOderFind.DeliveryDateModifiedBy = modifiedBy;
                }
                else if (deliveryOderFind.DeliveryStatus == DeliveryStatus.DELIVERY)
                {
                    deliveryOderFind.DeliveryStatus = DeliveryStatus.RECEIVE;
                    deliveryOderFind.ReceivedDate = DateTime.Now;
                    deliveryOderFind.ReceivedDateModifiedBy = modifiedBy;
                }
                else if (deliveryOderFind.DeliveryStatus == DeliveryStatus.RECEIVE)
                {
                    deliveryOderFind.DeliveryStatus = DeliveryStatus.COMPLETE;
                    deliveryOderFind.FinishedDate = DateTime.Now;
                    deliveryOderFind.FinishedDateModifiedBy = modifiedBy;
                }
            }
            return deliveryOderFind;
        }

        public GarnerOrder ChangeStatus(long orderId, int status, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerOrderEFRepository)}->{nameof(ChangeStatus)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}");

            var orderFind = _dbSet.FirstOrDefault(e => e.Id == orderId && (tradingProviderId == null || e.TradingProviderId == tradingProviderId));

            if (orderFind != null)
            {
                orderFind.Status = status;
            }
            return orderFind;
        }
        #endregion

        #region Function

        /// <summary>
        /// Sinh mã hợp đồng
        /// </summary>
        /// <returns></returns>
        public string GenContractCode(OrderContractCodeDto input)
        {
            List<ConfigContractCodeDto> configContractCodes = new();
            var configContractCodeDetails = _epicSchemaDbContext.GarnerConfigContractCodeDetails.Where(d => d.ConfigContractCodeId == input.ConfigContractCodeId).OrderBy(o => o.SortOrder);

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
                configContractCodes.Add(new ConfigContractCodeDto { Key = item.Key, Value = value });
            }
            contractCode = ConfigContractCode.GenContractCode(configContractCodes);
            return contractCode;
        }

        private string ContractCode(long orderId)
        {
            return ContractCodes.GARNER + orderId.ToString().PadLeft(8, '0');
        }

        #endregion

        #region Thống kê cho Sale
        /// <summary>
        /// Lấy danh sách hợp đồng mà sale bán hoặc bán hộ
        /// </summary>
        /// <returns></returns>
        public IQueryable<HopDongSaleAppDto> GetAllOrderBySale(AppSaleFilterContractDto input)
        {
            _logger.LogInformation($"Garner {nameof(GetAllOrderBySale)}:input = {JsonSerializer.Serialize(input)}");
            // Lấy các thông tin hợp đồng của Sale bán được bao gồm cả bán hộ
            var result = from order in _dbSet
                         join cifCode in _epicSchemaDbContext.CifCodes on order.CifCode equals cifCode.CifCode
                         join investor in _epicSchemaDbContext.Investors on cifCode.InvestorId equals investor.InvestorId into investors
                         from investor in investors.DefaultIfEmpty()
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers on cifCode.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         where cifCode.Deleted == YesNo.NO && order.Deleted == YesNo.NO && (input.Status == null || order.Status == input.Status)
                            && (input.OrderListStatus == null || input.OrderListStatus.Contains(order.Status)) 
                            && (input.DepartmentId == null || (order.DepartmentId == input.DepartmentId))
                            && (input.StartDate == null || (order.PaymentFullDate != null && order.PaymentFullDate.Value.Date >= input.StartDate.Value.Date) 
                                || (order.PaymentFullDate == null && order.CreatedDate != null && order.CreatedDate.Value.Date >= input.StartDate.Value.Date))
                            && (input.EndDate == null || (order.PaymentFullDate != null && order.PaymentFullDate.Value.Date <= input.EndDate.Value.Date) 
                                || (order.PaymentFullDate == null && order.CreatedDate != null && order.CreatedDate.Value.Date <= input.EndDate.Value.Date))
                            && ((order.TradingProviderId == input.TradingProviderId && input.SaleReferralCode == order.SaleReferralCode)
                                || (input.ListTradingIds != null && input.ListTradingIds.Contains(order.TradingProviderId) && order.SaleReferralCodeSub == input.SaleReferralCode))
                         select new HopDongSaleAppDto()
                         {
                             OrderId = order.Id,
                             ContractCode = order.ContractCode,
                             OrderStatus = order.Status,
                             Status = order.Status,
                             CifCode = order.CifCode,
                             InvestDate = order.InvestDate,
                             TotalValue = order.TotalValue,
                             InitTotalValue = order.InitTotalValue,
                             BuyDate = order.BuyDate,
                             SettlementDate = order.SettlementDate,
                             PaymentFullDate = order.PaymentFullDate,
                             ProjectType = ProjectTypes.GARNER,
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
            _logger.LogInformation($"Garner {nameof(AppGetAllStatisticOrderBySale)}:input = {JsonSerializer.Serialize(input)}");
            // Lấy các thông tin hợp đồng của Sale bán được bao gồm cả bán hộ
            var result = from order in _dbSet where order.Deleted == YesNo.NO && (input.Status == null || order.Status == input.Status)
                            && order.SaleReferralCode != null && (input.OrderListStatus == null || input.OrderListStatus.Contains(order.Status))
                            && (input.DepartmentId == null || (order.DepartmentId == input.DepartmentId))
                            && (input.StartDate == null || (order.CreatedDate != null && order.CreatedDate.Value.Date >= input.StartDate.Value.Date))
                            && (input.EndDate == null || (order.CreatedDate != null && order.CreatedDate.Value.Date <= input.EndDate.Value.Date))
                            && ((order.TradingProviderId == input.TradingProviderId && input.ListSaleReferralCode.Contains(order.SaleReferralCode))
                                || (input.ListTradingIds != null && input.ListTradingIds.Contains(order.TradingProviderId) && input.ListSaleReferralCode.Contains(order.SaleReferralCodeSub)))
                         select new AppStatisticOrderBySale()
                         {
                             OrderId = order.Id,
                             Status = order.Status,
                             CifCode = order.CifCode,
                             InvestDate = order.InvestDate,
                             TotalValue = order.TotalValue,
                             InitTotalValue = order.InitTotalValue,
                             CreatedDate = order.BuyDate,
                             ProjectType = ProjectTypes.GARNER
                         };
            return result;
        }
        #endregion
    }
}
