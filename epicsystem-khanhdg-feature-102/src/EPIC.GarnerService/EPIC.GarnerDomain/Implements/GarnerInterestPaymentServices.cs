using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.PayMoney;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.PaymentRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PrepareApproveRequestInterestPaymentDto = EPIC.GarnerEntities.Dto.GarnerInterestPayment.PrepareApproveRequestInterestPaymentDto;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerInterestPaymentServices : IGarnerInterestPaymentServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly GarnerCalendarEFRepository _garnerCalendarEFRepository;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly GarnerInterestPaymentDetailEFRepository _garnerInterestPaymentDetailEFRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerWithdrawalDetailEFRepository _garnerWithdrawalDetailEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly BankEFRepository _bankEFRepository;
        private readonly MsbRequestPaymentEFRepository _msbRequestPaymentEFRepository;
        private readonly MsbRequestPaymentDetailEFRepository _msbRequestPaymentDetailEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly MsbPayMoneyServices _msbPayMoneyServices;
        private readonly GarnerNotificationServices _garnerNotificationServices;

        public GarnerInterestPaymentServices(
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            GarnerNotificationServices garnerNotificationServices,
            MsbPayMoneyServices msbPayMoneyServices,
            ILogger<GarnerInterestPaymentServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _garnerNotificationServices = garnerNotificationServices;
            _garnerCalendarEFRepository = new GarnerCalendarEFRepository(dbContext, logger);
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
            _garnerInterestPaymentDetailEFRepository = new GarnerInterestPaymentDetailEFRepository(dbContext, logger);
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _garnerWithdrawalDetailEFRepository = new GarnerWithdrawalDetailEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _bankEFRepository = new BankEFRepository(dbContext, logger);
            _msbRequestPaymentEFRepository = new MsbRequestPaymentEFRepository(dbContext, logger);
            _msbRequestPaymentDetailEFRepository = new MsbRequestPaymentDetailEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _msbPayMoneyServices = msbPayMoneyServices;
        }

        /// <summary>
        /// Thêm chi trả
        /// </summary>
        public List<GarnerInterestPaymentSetUpDto> Add(List<CreateGarnerInterestPaymentDto> input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            List<GarnerInterestPaymentSetUpDto> listInterst = new();
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var interestPaymentItem in input)
            {
                var policyFind = _garnerPolicyEFRepository.FindById(interestPaymentItem.PolicyId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
                var interestPaymentItemInsert = _garnerInterestPaymentEFRepository.Add(new GarnerInterestPayment
                {
                    TradingProviderId = tradingProviderId,
                    CifCode = interestPaymentItem.CifCode,
                    AmountMoney = interestPaymentItem.AmountMoney,
                    PayDate = interestPaymentItem.PayDate,
                    PolicyId = interestPaymentItem.PolicyId,
                    DistributionId = policyFind.DistributionId,
                    PeriodIndex = interestPaymentItem.PeriodIndex,
                    CreatedBy = username
                });
                foreach (var detailItem in interestPaymentItem.Details)
                {
                    var orderFind = _garnerOrderEFRepository.FindById(detailItem.OrderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
                    _garnerInterestPaymentDetailEFRepository.Add(new GarnerInterestPaymentDetail
                    {
                        InterestPaymentId = interestPaymentItemInsert.Id,
                        OrderId = detailItem.OrderId,
                        AmountReceived = detailItem.AmountReceived,
                        Tax = detailItem.Tax,
                        ProfitRate = detailItem.ProfitRate,
                        Profit = detailItem.Profit,
                        TotalValueCurrent = orderFind.TotalValue,
                    });
                }
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return listInterst;
        }

        /// <summary>
        /// Lấy danh sách đã lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerInterestPaymentDto> FindAll(FilterGarnerInterestPaymentDto input)
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

            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<GarnerInterestPaymentDto> result = new();
            List<GarnerInterestPaymentDto> listInterestPayments = new();
            var interestPaymentFind = _garnerInterestPaymentEFRepository.FindAll(input);
            foreach (var item in interestPaymentFind.Items)
            {
                var policyFind = _garnerPolicyEFRepository.FindById(item.PolicyId);
                if (policyFind == null)
                {
                    _logger.LogError($"{nameof(FindAll)}: Không tìm thấy thông tin chính sách: policyId = {item.PolicyId}");
                    continue;
                }
                var distribution = _garnerDistributionEFRepository.FindById(policyFind.DistributionId, tradingProviderId);
                if (distribution == null)
                {
                    _logger.LogError($"{nameof(FindAll)}: Không tìm thấy thông tin phân phối: DistributionId = {policyFind.DistributionId}");
                    continue;
                }
                var product = _garnerProductEFRepository.FindById(distribution.ProductId);

                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(item.CifCode);
                BusinessCustomerDto businessCustomer = null;
                InvestorDto investor = null;
                if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomerInfo = _businessCustomerEFRepository.FindById(cifCodeFind.BusinessCustomerId ?? 0);
                    businessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerInfo);
                }
                else if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    var investorInfo = _investorEFRepository.FindById(cifCodeFind.InvestorId ?? 0);
                    investor = _mapper.Map<InvestorDto>(investorInfo);
                    var findIdentification = _investorEFRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0);
                    if (findIdentification != null)
                    {
                        investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(findIdentification);
                    }
                }

                List<GarnerInterestPaymentDetailDto> listInterestPaymentDetails = new();
                var interestPaymentDetails = _garnerInterestPaymentDetailEFRepository.GetAllByInterestPaymentId(item.Id);
                foreach (var interestPaymentDetailItem in interestPaymentDetails)
                {
                    var orderFind = _garnerOrderEFRepository.FindById(interestPaymentDetailItem.OrderId);
                    if (orderFind != null && orderFind.InvestDate != null)
                    {
                        var interestPaymentDetail = _mapper.Map<GarnerInterestPaymentDetailDto>(interestPaymentDetailItem);
                        interestPaymentDetail.Order = _mapper.Map<GarnerOrderDto>(orderFind);
                        interestPaymentDetail.InitTotalValue = orderFind.InitTotalValue;
                        interestPaymentDetail.ExistingAmount = orderFind.TotalValue;
                        interestPaymentDetail.NumberOfDays = (item.PayDate.Date - orderFind.InvestDate.Value.Date).Days;
                        listInterestPaymentDetails.Add(interestPaymentDetail);
                    }
                }
                listInterestPayments.Add(new GarnerInterestPaymentDto
                {
                    Id = item.Id,
                    CifCode = item.CifCode,
                    PolicyId = item.PolicyId,
                    ActuallyProfit = listInterestPaymentDetails.Sum(p => p.AmountReceived),
                    AllTotalValue = listInterestPaymentDetails.Sum(p => p.ExistingAmount),
                    Tax = listInterestPaymentDetails.Sum(p => p.Tax),
                    AmountMoney = item.AmountMoney,
                    PayDate = item.PayDate,
                    Details = listInterestPaymentDetails,
                    BusinessCustomer = businessCustomer,
                    Investor = investor,
                    Policy = _mapper.Map<GarnerPolicyDto>(policyFind),
                    Product = _mapper.Map<GarnerProductDto>(product)
                });
            }
            return new PagingResult<GarnerInterestPaymentDto>
            {
                Items = listInterestPayments.ToList(),
                TotalItems = listInterestPayments.Count
            };
        }

        /// <summary>
        /// Tìm theo Id của interestPayment
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        public GarnerInterestPaymentSetUpDto FindById(int interestPaymentId)
        {
            _logger.LogInformation($"{nameof(FindById)}: interestPaymentId = {interestPaymentId}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = new GarnerInterestPaymentSetUpDto();
            var interestPayment = _garnerInterestPaymentEFRepository.FindById(interestPaymentId, tradingProviderId);
            result.PayDate = interestPayment.PayDate;
            /*result.Tax = interestPayment.Tax;
            var orderFind = _garnerOrderEFRepository.FindById(interestPayment.Id).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);


            //Lấy thông tin khách hàng
            var cifCodeFind = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
            {
                var businessCustomerInfo = _businessCustomerEFRepository.FindById(cifCodeFind.BusinessCustomerId ?? 0);
                //result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerInfo);
            }
            else if (cifCodeFind != null && cifCodeFind.InvestorId != null)
            {
                var investorInfo = _investorEFRepository.FindById(cifCodeFind.InvestorId ?? 0);
                //result.Investor = _mapper.Map<InvestorDto>(investorInfo);
                var findIdentification = _investorEFRepository.GetDefaultIdentification(investorInfo.InvestorId);
                if (findIdentification != null)
                {
                    //result.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(findIdentification);
                }
            }*/

            return result;
        }

        /// <summary>
        /// Lấy danh sách đã lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagingResult<GarnerInterestPaymentByPolicyDto>> FindAllInterestPaymentPay(FilterGarnerInterestPaymentDto input)
        {
            DateTime now = DateTime.Now.Date;
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
            _logger.LogInformation($"{nameof(FindAllInterestPaymentPay)}: input = {JsonSerializer.Serialize(input)}, now = {now}");
            return new PagingResult<GarnerInterestPaymentByPolicyDto>();

            // Lấy thông tin CifCode
            var cifQuery = from cif in _cifCodeEFRepository.EntityNoTracking
                           join investor in _investorEFRepository.EntityNoTracking on cif.InvestorId equals investor.InvestorId into investors
                           from investor in investors.DefaultIfEmpty()
                           join businessCustomer in _businessCustomerEFRepository.EntityNoTracking on cif.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                           from businessCustomer in businessCustomers.DefaultIfEmpty()
                           select new
                           {
                               cif.CifCode,
                               Phone = (investor != null && investor.Phone != null) ? investor.Phone : (businessCustomer != null ? businessCustomer.Phone : null),
                           };

            // Lọc các thông tin trước khi xử lý
            var orderQuery = _garnerOrderEFRepository.EntityNoTracking
                .Where(o => o.Deleted == YesNo.NO && (tradingProviderId == null || o.TradingProviderId == tradingProviderId)
                    && (input.TradingProviderIds == null || input.TradingProviderIds.Contains(o.TradingProviderId))
                    && o.Status == OrderStatus.DANG_DAU_TU && o.InvestDate != null
                    && (input.Phone == null || cifQuery.Where(c => c.Phone == input.Phone).Any())
                    && (input.CifCode == null || cifQuery.Where(c => c.CifCode == input.CifCode).Any())
                    && (input.DistributionId == null || o.PolicyId == input.DistributionId)
                    && (input.PolicyId == null || o.PolicyId == input.PolicyId))
                .OrderByDescending(o => o.Id);

            // Lọc ra nhóm các chính sách
            var policyGroup = orderQuery.Select(p => p.PolicyId).Distinct();

            //Danh sách đổ ra kết quả
            List<GarnerInterestPaymentByPolicyDto> result = new();

            // Lặp theo từng chính sách
            foreach (var policyIdItem in policyGroup)
            {
                List<Task> tasks = new();
                var policyFind = _garnerPolicyEFRepository.FindById(policyIdItem);
                if (policyFind == null)
                {
                    _logger.LogError($"{nameof(FindAllInterestPaymentPay)}: Không tìm thấy thông tin chính sách: policyId = {policyIdItem}");
                    continue;
                }
                var distribution = _garnerDistributionEFRepository.FindById(policyFind.DistributionId);
                if (distribution == null)
                {
                    _logger.LogError($"{nameof(FindAllInterestPaymentPay)}: Không tìm thấy thông tin phân phối: distributionId = {policyFind.DistributionId}");
                    continue;
                }
                var product = _garnerProductEFRepository.FindById(distribution.ProductId);

                var orderGroupByPolicy = orderQuery.Where(o => o.PolicyId == policyIdItem);
                // Lọc ra nhóm hợp đồng theo CifCode và lấy ngày đầu tư nhỏ nhất trong các hợp đồng
                var groupOrderByCifCode = from order in orderGroupByPolicy.ToList()
                                          join cif in _cifCodeEFRepository.EntityNoTracking on order.CifCode equals cif.CifCode
                                          group order by order.CifCode into orderGroup
                                          select new
                                          {
                                              CifCode = orderGroup.Key,
                                              Orders = orderGroup,
                                              InvestDate = orderGroup.Select(o => o.InvestDate).Min(),
                                          };
                // Lặp theo từng nhóm CifCode với mỗi chính sách
                foreach (var cifCodeItem in groupOrderByCifCode)
                {
                    // Danh sách ngày chi trả
                    List<DateTime> listInterestDates = _garnerPolicyDetailEFRepository.GetListPayDateByPolicy(policyFind, cifCodeItem.InvestDate.Value);

                    List<GarnerInterestPaymentSetUpDto> listInterestPaymentByPolicyItems = new();
                    var interestPayment = _garnerInterestPaymentEFRepository.Entity.Where(p => p.CifCode == cifCodeItem.CifCode && p.PolicyId == policyIdItem && p.Deleted == YesNo.NO);

                    // kỳ trả lãi
                    int periodIndex = interestPayment.Count();

                    // Lặp theo các ngày chi trả
                    foreach (var item in listInterestDates)
                    {
                        ConcurrentBag<GarnerInterestPaymentSetUpDto> listInterestPaymentByInterestDateItem = new();

                        //Kiểm tra ngày nghỉ lễ
                        var interestDateItem = _garnerCalendarEFRepository.NextWorkDay(item, policyFind.TradingProviderId);

                        // Bỏ qua ngày chi trả đã được trả trước đó
                        if (interestPayment.Select(p => p.PayDate).Contains(interestDateItem))
                        {
                            continue;
                        }

                        // Lặp từng lệnh 
                        foreach (var order in cifCodeItem.Orders)
                        {
                            var task = Task.Run(() =>
                            {
                                var dbContext = CommonUtils.GetService<EpicSchemaDbContextTransient>(_httpContext);
                                var garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, _logger);
                                var garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, _logger);
                                var garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, _logger);
                                var garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, _logger);
                                var garnerCalendarEFRepository = new GarnerCalendarEFRepository(dbContext, _logger);

                                var policy = garnerPolicyEFRepository.FindById(order.PolicyId);
                                if (policy == null)
                                {
                                    _logger.LogError($"{nameof(FindAllInterestPaymentPay)}: Không tìm thấy thông tin chính sách: policyId = {order.PolicyId}");
                                    return;
                                }

                                if (order.InvestDate == null)
                                {
                                    _logger.LogError($"{nameof(FindAllInterestPaymentPay)}: ActiveDate = null, orderId = {order.Id}");
                                    return;
                                }
                                DateTime investDate = order.InvestDate.Value.Date;

                                // % thuế lợi nhuận
                                decimal taxRate = policy.IncomeTax / 100;

                                // Tra bậc 
                                var policyDetail = garnerPolicyDetailEFRepository.FindPolicyDetailByDate(order.PolicyId, investDate, interestDateItem);

                                //% lợi nhuận
                                decimal profitRate = policyDetail.Profit / 100;

                                //số tiền đã trả theo lệnh 
                                decimal profitPaid = garnerInterestPaymentEFRepository.ProfitPaid(order.Id, interestDateItem);

                                //số ngày đầu tư (Ngày tính lợi nhuận - ngày đầu tư của lệnh)
                                int numInvestDays = (interestDateItem - investDate).Days;
                                if (numInvestDays < 0)
                                {
                                    _logger.LogError($"{nameof(FindAllInterestPaymentPay)}: Ngày đầu tư đang lớn hơn ngày hiện tại: order.InvestDate = {order.InvestDate}, now = {now}");
                                    return;
                                }
                                // Số tiền đã trã được lập và số tiền chưa được lập tại thời điểm trước đó
                                profitPaid = profitPaid + listInterestPaymentByPolicyItems.Where(p => p.OrderId == order.Id && p.PayDate < interestDateItem).Sum(r => r.AmountReceived);

                                // Tính lợi nhuận
                                var calculateProfit = _garnerInterestPaymentEFRepository.CalculateProfit(policy.CalculateType, order.TotalValue, numInvestDays, profitRate, taxRate, profitPaid);

                                //kiểm tra nếu là kỳ cuối thì cộng thêm gốc
                                //actuallyProfit += order.TotalValue; //nếu actuallyProfit bị âm thì sẽ cộng vào gốc để bị bớt đi

                                //thêm chi trả cho lệnh vào bảng interest payment nếu là kỳ cuối thì sau khi duyệt update lại trạng thái order thành đã tất toán
                                //dùng Math.Round để làm tròn

                                listInterestPaymentByInterestDateItem.Add(new GarnerInterestPaymentSetUpDto
                                {
                                    CifCode = order.CifCode,
                                    Order = _mapper.Map<GarnerOrderDto>(order),
                                    OrderId = order.Id,
                                    InvestDate = order.InvestDate.Value,
                                    TotalValue = order.TotalValue,
                                    InitTotalValue = order.InitTotalValue,
                                    ProfitRate = profitRate,
                                    NumberOfDays = numInvestDays,
                                    ExistingAmount = order.TotalValue,
                                    Profit = Math.Round(calculateProfit.Profit),
                                    Tax = Math.Round(calculateProfit.Tax),
                                    AmountReceived = Math.Round(calculateProfit.ActualProfit),
                                    InvestDays = numInvestDays,
                                    PayDate = interestDateItem,
                                });
                            });
                            tasks.Add(task);
                        }

                        await Task.WhenAll(tasks);

                        if (listInterestPaymentByInterestDateItem.Count() > 0)
                        {
                            var cifCodeFind = _cifCodeEFRepository.FindByCifCode(cifCodeItem.CifCode);
                            BusinessCustomerDto businessCustomer = null;
                            InvestorDto investor = null;
                            if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                            {
                                var businessCustomerInfo = _businessCustomerEFRepository.FindById(cifCodeFind.BusinessCustomerId ?? 0);
                                businessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerInfo);
                            }
                            else if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                            {
                                var investorInfo = _investorEFRepository.FindById(cifCodeFind.InvestorId ?? 0);
                                investor = _mapper.Map<InvestorDto>(investorInfo);
                                var findIdentification = _investorEFRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0);
                                investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(findIdentification);
                            }

                            periodIndex = periodIndex + 1;
                            result.Add(new GarnerInterestPaymentByPolicyDto
                            {
                                PolicyId = policyFind.Id,
                                CifCode = cifCodeItem.CifCode,
                                PayDate = interestDateItem,
                                PeriodIndex = periodIndex,
                                AmountMoney = listInterestPaymentByInterestDateItem.Sum(p => p.AmountReceived),
                                AllTotalValue = listInterestPaymentByInterestDateItem.Sum(p => p.TotalValue),
                                Tax = listInterestPaymentByInterestDateItem.Sum(p => p.Tax),
                                Details = listInterestPaymentByInterestDateItem.ToList(),
                                Policy = _mapper.Map<GarnerPolicyDto>(policyFind),
                                Product = _mapper.Map<GarnerProductDto>(product),
                                BusinessCustomer = businessCustomer,
                                Investor = investor,
                            });
                        }
                        listInterestPaymentByPolicyItems.AddRange(listInterestPaymentByInterestDateItem);
                    }
                }
            }

            // Lọc đúng ngày chi trả
            if (input.IsExactDate == YesNo.YES)
            {
                result = result.Where(r => (input.PayDate == null ? r.PayDate == DateTime.Now : r.PayDate == input.PayDate))
                    .OrderByDescending(r => r.PayDate.Date).ToList();
            }
            // Lọc cả các ngày chưa chi trả trước đó
            else if (input.IsExactDate == YesNo.NO)
            {
                result = result.Where(r => (input.PayDate == null ? r.PayDate <= DateTime.Now : r.PayDate <= input.PayDate))
                    .OrderByDescending(r => r.PayDate.Date).ToList();
            }
            var totalItems = result.Count();
            result = result.Skip(input.Skip).Take(input.PageSize).ToList();
            return new PagingResult<GarnerInterestPaymentByPolicyDto>
            {
                Items = result,
                TotalItems = totalItems
            };
        }

        #region Approve
        public async Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestInterestPayment(PrepareApproveRequestInterestPaymentDto input)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(PrepareApproveRequestInterestPayment)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            List<int> distributionIds = new();
            //string prefixAccount = "968668868";
            var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == input.TradingBankAccId && e.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);

            var prepareResult = new MsbRequestPaymentWithErrorDto()
            {
                Id = (long)_msbRequestPaymentEFRepository.NextKey(),
                PrefixAccount = prefixAccount.PrefixMsb,
                Details = new(),
            };
            foreach (long interestPaymentId in input.InterestPaymentIds)
            {
                // Kiểm tra xem đã có yêu cầu chi tiền trước đó mà không thành công không
                var checkRequestPayment = _dbContext.MsbRequestPaymentDetail.Any(p => p.DataType == RequestPaymentDataTypes.GAN_INTEREST_PAYMENT && p.ReferId == interestPaymentId
                                            && p.Exception == null && p.Status == RequestPaymentStatus.KHOI_TAO);
                if (checkRequestPayment)
                {
                    _garnerInterestPaymentEFRepository.ThrowException(ErrorCode.PaymentMsbNotificationExistRequestNotSuccess, interestPaymentId);
                }

                var interestPayment = _garnerInterestPaymentEFRepository.FindById(interestPaymentId, tradingProviderId)
                    .ThrowIfNull(_dbContext, ErrorCode.GarnerInterestPaymentNotFound, interestPaymentId);

                if (interestPayment.Status != WithdrawalStatus.YEU_CAU && interestPayment.Status != WithdrawalStatus.CHO_PHAN_HOI)
                {
                    _garnerInterestPaymentEFRepository.ThrowException(ErrorCode.GarnerInterestPaymentNotInRequest);
                }

                var interestPaymentDetailQuery = _garnerInterestPaymentDetailEFRepository.EntityNoTracking.Where(d => d.InterestPaymentId == interestPayment.Id);

                if (!interestPaymentDetailQuery.Any())
                {
                    _garnerInterestPaymentDetailEFRepository.ThrowException(ErrorCode.GarnerInterestPaymentNotContaintDetail, $"InterestPaymentId = {interestPayment.Id}");
                }
                //distributionIds.Add(interestPayment.DistributionId);

                //lấy lệnh đầu tiên theo chính sách chi trả
                var orderFirst = _garnerOrderEFRepository.EntityNoTracking.Where(r => interestPaymentDetailQuery.Select(g => g.OrderId).Contains(r.Id))
                    .OrderBy(o => o.InvestDate).FirstOrDefault().ThrowIfNull(_dbContext, ErrorCode.GarnerInterestPaymentNotFoundFirstOrderByPolicy, interestPaymentId);

                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(orderFirst.CifCode)
                    .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound, orderFirst.CifCode);

                var prepareResultDetail = new MsbRequestDetailWithErrorDto
                {
                    Id = (long)_msbRequestPaymentDetailEFRepository.NextKey(),
                    DataType = RequestPaymentDataTypes.GAN_INTEREST_PAYMENT,
                    ReferId = interestPaymentId,
                    AmountMoney = interestPayment.AmountMoney, // Số tiền phải chi
                };

                if (cifCodeFind.InvestorId != null) //khách cá nhân
                {
                    var investorBankAcc = _investorBankAccountEFRepository
                        .FindById(orderFirst.InvestorBankAccId ?? 0, cifCodeFind.InvestorId)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestorBankAccNotFound, orderFirst.InvestorBankAccId);

                    var bankFind = _bankEFRepository.FindById(investorBankAcc.BankId)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBankNotFound, investorBankAcc.BankId);

                    prepareResultDetail.BankAccount = investorBankAcc.BankAccount;
                    prepareResultDetail.OwnerAccount = investorBankAcc.OwnerAccount;
                    prepareResultDetail.BankId = bankFind.BankId;
                    prepareResultDetail.Bin = bankFind.Bin;
                    prepareResultDetail.BankCode = bankFind.BankCode;
                    prepareResultDetail.BankName = bankFind.BankName;
                }
                else //khách doanh nghiệp
                {
                    var businessBankAcc = _businessCustomerBankEFRepository
                       .FindById(orderFirst.InvestorBankAccId ?? 0, cifCodeFind.BusinessCustomerId)
                       .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, orderFirst.BusinessCustomerBankAccId);

                    var bankFind = _bankEFRepository.FindById(businessBankAcc.BankId)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBankNotFound, businessBankAcc.BankId);

                    prepareResultDetail.BankAccount = businessBankAcc.BankAccNo;
                    prepareResultDetail.OwnerAccount = businessBankAcc.BankAccName;
                    prepareResultDetail.BankId = bankFind.BankId;
                    prepareResultDetail.Bin = bankFind.Bin;
                    prepareResultDetail.BankCode = bankFind.BankCode;
                    prepareResultDetail.BankName = bankFind.BankName;
                }
                if (string.IsNullOrWhiteSpace(prepareResultDetail.Bin))
                {
                    prepareResultDetail.ErrorMessage = $"Chưa cấu hình BIN cho ngân hàng {prepareResultDetail.BankCode}";
                }
                prepareResult.Details.Add(prepareResultDetail);
            }
            //Check xem các yêu cầu rút (withdrawal) có cùng 1 sản phẩm phân phối (distribution)hay không 
            if (distributionIds.GroupBy(e => e).Count() > 1)
            {
                _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerRequestIsNotTheSameDistribution);
            }

            //lọc ra những bank có bin khác null
            var detailRequest = prepareResult.Details.Where(d => d.Bin != null)
                .Select(d => new MsbRequestPayMoneyItemDto
                {
                    DetailId = d.Id,
                    BankAccount = d.BankAccount,
                    OwnerAccount = d.OwnerAccount,
                    AmountMoney = d.AmountMoney,
                    Note = $"{PaymentNotes.THANH_TOAN}{d.Id}",
                    ReceiveBankBin = d.Bin
                })
                .ToList();

            var request = new MsbRequestPayMoneyDto()
            {
                TId = prefixAccount.TId,
                MId = prefixAccount.MId,
                RequestId = prepareResult.Id,
                PrefixAccount = prepareResult.PrefixAccount,
                Details = detailRequest,
                AccessCode = prefixAccount.AccessCode
            };
            if (detailRequest.Count() > 0)
            {
                ResultRequestPayDto resultRequest = new();
                try
                {
                    //kết nối bank
                    resultRequest = await _msbPayMoneyServices.RequestPayMoney(request);
                }
                catch (Exception ex)
                {
                    var requestPaymentInsert = _msbRequestPaymentEFRepository.Add(new MsbRequestPayment
                    {
                        Id = prepareResult.Id,
                        RequestType = RequestPaymentTypes.CHI_TRA_LOI_TUC,
                        ProductType = ProductTypes.GARNER,
                        TradingProdiverId = tradingProviderId,
                        CreatedBy = username,
                    });
                    foreach (var detailItem in prepareResult.Details.Where(d => d.Bin != null))
                    {
                        _msbRequestPaymentDetailEFRepository.Add(new MsbRequestPaymentDetail
                        {
                            Id = detailItem.Id,
                            RequestId = prepareResult.Id,
                            DataType = detailItem.DataType,
                            ReferId = detailItem.ReferId,
                            Status = RequestPaymentStatus.FAILED,
                            BankId = detailItem.BankId,
                            OwnerAccount = detailItem.OwnerAccount,
                            OwnerAccountNo = detailItem.BankAccount,
                            Note = $"{PaymentNotes.THANH_TOAN}{detailItem.Id}",
                            Bin = detailItem.Bin,
                            Exception = ex.Message,
                            AmountMoney = detailItem.AmountMoney
                        });
                    }
                    _dbContext.SaveChanges();
                    throw;
                }

                foreach (var err in resultRequest.ErrorDetails)
                {
                    var detail = prepareResult.Details.FirstOrDefault(d => d.Id == err.DetailId)
                        .ThrowIfNull(ErrorCode.BadRequest, $"Yêu cầu chi hộ bị thay đổi id khi nhận kết quả từ bank, transId | detailId = {err.DetailId}");
                    detail.ErrorMessage = err.ErrorMessage; //gán lỗi của bank trả về
                }
                prepareResult.IsSuccess = resultRequest.ErrorDetails.Count == 0 && prepareResult.Details.Count(d => d.Bin == null) == 0;
                prepareResult.IsSubmitOtp = resultRequest.IsSubmitOtp;
            }
            return prepareResult;
        }
        #endregion

        /// <summary>
        /// Duyệt chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ApproveInterestPaymentOrder(ApproveInterestPaymentRenewalsOrderDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(ApproveInterestPaymentOrder)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            List<GarnerInterestPayment> interestPayments = new();
            MsbRequestPayment requestPayment = new MsbRequestPayment();
            List<MsbRequestPaymentDetail> listRequestPaymentDetails = new();

            // Kiểm tra thông tin các yêu cầu chi trả
            foreach (var interestPaymentId in input.InterestPaymentIds)
            {
                var interestPayment = _garnerInterestPaymentEFRepository.FindById(interestPaymentId, tradingProviderId)
                                         .ThrowIfNull(_dbContext, ErrorCode.GarnerInterestPaymentNotFound);

                // Nếu khác trạng thái khởi tạo hoặc chờ phản hồi
                if (interestPayment.Status != InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA && interestPayment.Status != InterestPaymentStatus.CHO_PHAN_HOI)
                {
                    _garnerInterestPaymentEFRepository.ThrowException(ErrorCode.GarnerWithdrawalNotInRequest);
                }
                // Yêu cầu chi có giống lúc chuẩn bị hay không
                if (input.Prepare != null && !input.Prepare.Details.Select(p => p.ReferId).Contains(interestPaymentId))
                {
                    _garnerInterestPaymentEFRepository.ThrowException(ErrorCode.GarnerWithdrawalIdNotInPrepareDetail);
                }
                interestPayments.Add(interestPayment);
            }

            // Đã lập chưa chi trả
            if (input.Status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN)
            {
                var transaction = _dbContext.Database.BeginTransaction();
                AddOrderPaymentPay(tradingProviderId, interestPayments, input.Status, input.TradingBankAccId, username, input.ApproveNote, MsbBankStatus.INIT);
                _dbContext.SaveChanges();
                transaction.Commit();
                // Duyệt không đi tiền thì gửi thông báo//Gửi thông báo khi thành công các lệnh. Duyệt chi tiền từ bank thì đợi notipayment
                foreach (var interestPayment in interestPayments)
                {
                    await _garnerNotificationServices.SendNotifyGarnerInterestPayment(interestPayment.Id);
                }
            }

            // Hủy yêu cầu chi tiền
            else if (input.Status == InterestPaymentStatus.HUY_DUYET)
            {
                foreach (var interestPayment in interestPayments)
                {
                    interestPayment.Status = input.Status;
                    interestPayment.ModifiedBy = username;
                    interestPayment.ModifiedDate = DateTime.Now;
                }
                _dbContext.SaveChanges();
            }

            // Chi tiền tự động
            if (input.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
            {
                var transaction = _dbContext.Database.BeginTransaction();
                // Chuyển sang trạng thái tạm khi duyệt đi tiền
                // Notifi thành công thì chuyển sang trạng thái duyệt đi tiền
                var waitResponseStatus = InterestPaymentStatus.CHO_PHAN_HOI;

                // Duyệt sang trạng thái chờ phản hồi chờ bank Noti thành công
                AddOrderPaymentPay(tradingProviderId, interestPayments, waitResponseStatus, input.TradingBankAccId, username, null, MsbBankStatus.INIT);

                var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == input.TradingBankAccId && e.Deleted == YesNo.NO)
                   .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);

                //lấy thông tin để lưu vào lô chi hộ
                requestPayment.Id = input.Prepare.Id;
                requestPayment.TradingProdiverId = tradingProviderId;
                requestPayment.ProductType = ProductTypes.GARNER;
                requestPayment.RequestType = RequestPaymentTypes.CHI_TRA_LOI_TUC;
                requestPayment.CreatedDate = DateTime.Now;
                requestPayment.CreatedBy = username;

                _msbRequestPaymentEFRepository.Add(requestPayment);
                foreach (var requestDetail in input.Prepare.Details)
                {
                    var requestPaymentDetail = new MsbRequestPaymentDetail()
                    {
                        Id = requestDetail.Id,
                        RequestId = requestPayment.Id,
                        DataType = RequestPaymentDataTypes.GAN_INTEREST_PAYMENT,
                        ReferId = requestDetail.ReferId,
                        Status = RequestPaymentStatus.KHOI_TAO,
                        BankId = requestDetail.BankId,
                        Bin = requestDetail.Bin,
                        Note = $"{PaymentNotes.THANH_TOAN}{requestDetail.Id}",
                        OwnerAccount = requestDetail.OwnerAccount,
                        OwnerAccountNo = requestDetail.BankAccount,
                        TradingBankAccId = input.TradingBankAccId ?? 0,
                        AmountMoney = requestDetail.AmountMoney
                    };
                    _msbRequestPaymentDetailEFRepository.Add(requestPaymentDetail);
                    listRequestPaymentDetails.Add(requestPaymentDetail);
                }
                _dbContext.SaveChanges();

                string exceptionMessage;
                try
                {
                    await _msbPayMoneyServices.TransferProcessOtp(new ProcessRequestPayDto
                    {
                        RequestId = requestPayment.Id,
                        Otp = input.Otp,
                        TId = prefixAccount.TId,
                        MId = prefixAccount.MId,
                        AccessCode = prefixAccount.AccessCode
                    });
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;
                    transaction.Rollback();

                    foreach (var detailItem in listRequestPaymentDetails)
                    {
                        detailItem.Status = RequestPaymentStatus.FAILED;
                        detailItem.Exception = ex.Message;
                    }
                    _msbRequestPaymentEFRepository.Entity.Add(requestPayment);
                    _msbRequestPaymentDetailEFRepository.Entity.AddRange(listRequestPaymentDetails);
                    _dbContext.SaveChanges();
                    throw;
                }
            }
        }

        /// <summary>
        /// Thêm thanh toán khi lập chi trả
        /// </summary>
        private List<GarnerInterestPayment> AddOrderPaymentPay(int tradingProviderId, List<GarnerInterestPayment> interestPayments, int status, int? tradingBankAccId, string username, int? approveNote, int? statusBank = null)
        {
            foreach (var interestPaymentItem in interestPayments)
            {
                DateTime now = DateTime.Now;
                interestPaymentItem.Status = status;
                interestPaymentItem.ApproveBy = username;
                interestPaymentItem.ApproveDate = now;
                interestPaymentItem.ApproveNote = approveNote;
                interestPaymentItem.ApproveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);

                if (statusBank != null)
                {
                    interestPaymentItem.StatusBank = MsbBankStatus.INIT;
                }

                var details = _garnerInterestPaymentDetailEFRepository.GetAllByInterestPaymentId(interestPaymentItem.Id);
                // Trạng thái duyệt ko đi tiền hoặc đi tiền xử lý các thông tin liên quan đến rút tiền
                foreach (var detail in details)
                {
                    var order = _garnerOrderEFRepository.FindById(detail.OrderId)
                        .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);

                    if (interestPaymentItem.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                    {
                        //Thêm chi (BondOrderPayment)
                        var orderPaymentSpend = new GarnerOrderPayment()
                        {
                            TradingProviderId = tradingProviderId,
                            OrderId = order.Id,
                            TranDate = now,
                            TranType = TranTypes.CHI,
                            TranClassify = TranClassifies.CHI_TRA_LOI_TUC,
                            PaymentType = PaymentTypes.CHUYEN_KHOAN,
                            PaymentAmount = detail.AmountReceived, // Số tiền chi trả của lệnh
                            Status = OrderPaymentStatus.DA_THANH_TOAN,
                            Description = PaymentNotes.CHI_LOI_NHUAN + order.ContractCode,
                            CreatedBy = username,
                            CreatedDate = now,
                            ApproveBy = username,
                            ApproveDate = now,
                            TradingBankAccId = tradingBankAccId,
                        };
                        _garnerOrderPaymentEFRepository.Add(orderPaymentSpend, username, tradingProviderId);
                    }
                }

                // Duyệt chi tiền tự động thì lưu lại lịch sử
                if (interestPaymentItem.Status == WithdrawalStatus.CHO_PHAN_HOI)
                {
                    _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate
                    {
                        UpdateTable = GarnerHistoryUpdateTables.GAN_INTEREST_PAYMENT,
                        RealTableId = interestPaymentItem.Id,
                        Action = ActionTypes.DUYET,
                        Summary = $"{username} thực hiện duyệt chi rút vốn tự động: {details.Sum(d => d.AmountReceived)}"
                    }, username);
                }
                _dbContext.SaveChanges();
            }
            return interestPayments;
        }
    }
}
