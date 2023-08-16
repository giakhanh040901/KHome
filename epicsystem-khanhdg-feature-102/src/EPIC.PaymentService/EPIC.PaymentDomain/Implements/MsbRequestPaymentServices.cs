using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerRepositories;
using EPIC.InvestRepositories;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.PayMoney;
using EPIC.MSB.Services;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.PaymentRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.PaymentDomain.Implements
{
    public class MsbRequestPaymentServices : IMsbRequestPaymentServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<MsbRequestPaymentServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly MsbRequestPaymentEFRepository _msbRequestPaymentEFRepository;
        private readonly MsbRequestPaymentDetailEFRepository _msbRequestPaymentDetailEFRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepo;
        private readonly GarnerWithdrawalDetailEFRepository _garnerWithdrawalDetailEFRepo;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly MsbPayMoneyServices _msbPayMoneyServices;
        private readonly BankEFRepository _bankEFRepository;
        private readonly InvestWithdrawalEFRepository _investWithdrawalEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestInterestPaymentEFRepository _investInterestPaymentEFRepository;
        private readonly MsbNotificationPaymentRepository _msbNotificationPaymentRepository;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;

        public MsbRequestPaymentServices(EpicSchemaDbContext dbContext, IMapper mapper, ILogger<MsbRequestPaymentServices> logger, IHttpContextAccessor httpContext,
                                           MsbPayMoneyServices msbPayMoneyServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContext;
            _msbRequestPaymentEFRepository = new MsbRequestPaymentEFRepository(_dbContext, _logger);
            _msbRequestPaymentDetailEFRepository = new MsbRequestPaymentDetailEFRepository(_dbContext, _logger);
            _garnerWithdrawalEFRepo = new GarnerWithdrawalEFRepository(_dbContext, _logger);
            _garnerWithdrawalDetailEFRepo = new GarnerWithdrawalDetailEFRepository(_dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(_dbContext, logger);
            _investWithdrawalEFRepository = new InvestWithdrawalEFRepository(_dbContext, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
            _investInterestPaymentEFRepository = new InvestInterestPaymentEFRepository(_dbContext, _logger);
            _msbPayMoneyServices = msbPayMoneyServices;
            _bankEFRepository = new BankEFRepository(_dbContext, _logger);
            _msbNotificationPaymentRepository = new MsbNotificationPaymentRepository(_dbContext, _logger);
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Cập nhật request chi hộ  MSB
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void UpdateRequest(UpdateMsbRequestPaymentDto input)
        {
            _logger.LogInformation($"{nameof(UpdateRequest)}: input = {JsonSerializer.Serialize(input)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var getRequest = _msbRequestPaymentEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.PaymentRequestNotFound);

            var requestUpdate = _mapper.Map<MsbRequestPayment>(input);

            _msbRequestPaymentEFRepository.Udpate(requestUpdate, tradingProviderId);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Find By Id
        /// </summary>
        /// <returns></returns>
        public MsbRequestPaymentDto FindById(long requestId)
        {
            _logger.LogInformation($"{nameof(FindById)}: requestId = {requestId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var getRequest = _msbRequestPaymentEFRepository.FindById(requestId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.PaymentRequestNotFound);
            return _mapper.Map<MsbRequestPaymentDto>(getRequest);
        }

        /// <summary>
        /// Hủy yêu cầu chi tiền 
        /// </summary>
        public void CancelRequestPayment(long requestPaymentDetailId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(CancelRequestPayment)}: requestPaymentDetailId= {requestPaymentDetailId}, username= {username}, tradingProviderId={tradingProviderId}");

            var requestPaymentDetail = _dbContext.MsbRequestPaymentDetail.FirstOrDefault(p => p.Id == requestPaymentDetailId
                                        && ((p.DataType == RequestPaymentDataTypes.GAN_WITHDRAWAL && _dbContext.GarnerWithdrawals.Any(w => w.Id == p.ReferId && w.TradingProviderId == tradingProviderId && w.Deleted == YesNo.NO))
                                            || (p.DataType == RequestPaymentDataTypes.GAN_INTEREST_PAYMENT && _dbContext.GarnerInterestPayments.Any(w => w.Id == p.ReferId && w.TradingProviderId == tradingProviderId && w.Deleted == YesNo.NO))
                                            || (p.DataType == RequestPaymentDataTypes.EP_INV_WITHDRAWAL && _dbContext.InvestWithdrawals.Any(w => w.Id == p.ReferId && w.TradingProviderId == tradingProviderId && w.Deleted == YesNo.NO))
                                            || (p.DataType == RequestPaymentDataTypes.EP_INV_INTEREST_PAYMENT && _dbContext.InvestInterestPayments.Any(w => w.Id == p.ReferId && w.TradingProviderId == tradingProviderId && w.Deleted == YesNo.NO))))
                .ThrowIfNull(_dbContext, ErrorCode.PaymentRequestDetailNotFound);
            if (requestPaymentDetail.Status != RequestPaymentStatus.KHOI_TAO)
            {
                _msbRequestPaymentDetailEFRepository.ThrowException(ErrorCode.PaymentMsbNotificationCanNotCancel);
            }
            requestPaymentDetail.Status = RequestPaymentStatus.FAILED;
            requestPaymentDetail.Exception = $"{username} hủy yêu cầu chi tiền";
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Find All
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<ViewMsbRequestPaymentDto> FindAll(FilterMsbRequestDetailDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var requestDetails = _msbRequestPaymentDetailEFRepository.FindAll(input);
            var result = new PagingResult<ViewMsbRequestPaymentDto>();
            var listRequestPayment = new List<ViewMsbRequestPaymentDto>();
            foreach (var item in requestDetails.Items)
            {
                if (item.ProductType == ProductTypes.INVEST && item.DataType == RequestPaymentDataTypes.EP_INV_WITHDRAWAL)
                {
                    var investOrder = from withdrawal in _dbContext.InvestWithdrawals
                                      join order in _dbContext.InvOrders on withdrawal.OrderId equals order.Id
                                      where withdrawal.Id == item.ReferId && (input.ApproveDate == null || order.ApproveDate.Value.Date == input.ApproveDate.Value.Date)
                                      select new ViewInvestOrderDto()
                                      {
                                          Id = order.Id,
                                          ContractCode = order.ContractCode,
                                          ApproveDate = order.ApproveDate,
                                      };
                    item.InvestOrder = investOrder.FirstOrDefault();

                    if (investOrder.Any())
                    {
                        var orderContractFiles = _dbContext.InvestOrderContractFile.Where(gocf => gocf.OrderId == investOrder.FirstOrDefault().Id && gocf.Deleted == YesNo.NO
                                                                                            && gocf.TradingProviderId == tradingProviderId);

                        var contractCodeGen = orderContractFiles.Select(ocf => ocf.ContractCodeGen).Distinct();
                        if (contractCodeGen.Count() == 1)
                        {
                            item.InvestOrder.GenContractCode = contractCodeGen.First();
                        }
                        item.ContractCode = item.InvestOrder.GenContractCode ?? item.InvestOrder.ContractCode;

                    }

                    listRequestPayment.Add(item);
                }
                else if (item.ProductType == ProductTypes.INVEST && item.DataType == RequestPaymentDataTypes.EP_INV_INTEREST_PAYMENT)
                {
                    var investOrder = from interestPayment in _dbContext.InvestInterestPayments
                                      join order in _dbContext.InvOrders on interestPayment.OrderId equals order.Id
                                      where interestPayment.Id == item.ReferId && (input.ApproveDate == null || order.ApproveDate.Value.Date == input.ApproveDate.Value.Date)
                                      select new ViewInvestOrderDto()
                                      {
                                          Id = order.Id,
                                          ContractCode = order.ContractCode,
                                          ApproveDate = order.ApproveDate,
                                      };
                    item.InvestOrder = investOrder.FirstOrDefault();
                    if (investOrder.Any())
                    {
                        var orderContractFiles = _dbContext.InvestOrderContractFile.Where(gocf => gocf.OrderId == investOrder.FirstOrDefault().Id && gocf.Deleted == YesNo.NO
                                                                                            && gocf.TradingProviderId == tradingProviderId);

                        var contractCodeGen = orderContractFiles.Select(ocf => ocf.ContractCodeGen).Distinct();
                        if (contractCodeGen.Count() == 1)
                        {
                            item.InvestOrder.GenContractCode = contractCodeGen.First();
                        }
                        item.ContractCode = item.InvestOrder.GenContractCode ?? item.InvestOrder.ContractCode;
                    }
                    listRequestPayment.Add(item);
                }
                else if (item.ProductType == ProductTypes.GARNER && item.DataType == RequestPaymentDataTypes.GAN_WITHDRAWAL)
                {
                    var garnerOrders = from withdrawal in _dbContext.GarnerWithdrawals
                                       join withdrawalDetails in _dbContext.GarnerWithdrawalDetails on withdrawal.Id equals withdrawalDetails.WithdrawalId
                                       join order in _dbContext.GarnerOrders on withdrawalDetails.OrderId equals order.Id
                                       where withdrawal.Id == item.ReferId && (input.ApproveDate == null || order.ApproveDate.Value.Date == input.ApproveDate.Value.Date)
                                       select new ViewGarnerOrderDto()
                                       {
                                           Id = order.Id,
                                           ContractCode = order.ContractCode,
                                           ApproveDate = order.ApproveDate,
                                       };
                    var garnerWithdrawl = _garnerWithdrawalEFRepo.Entity.FirstOrDefault(o => o.Id == item.ReferId);
                    //Ngày yêu cầu rút vốn: Lấy createDate của bảng GanWithdrawal
                    item.WithdrawalDate = garnerWithdrawl?.CreatedDate;
                    item.GarnerOrders = garnerOrders.ToList();
                    foreach (var garnerOrder in item.GarnerOrders)
                    {
                        var orderContractFiles = _dbContext.GarnerOrderContractFiles.Where(gocf => gocf.OrderId == garnerOrder.Id && gocf.Deleted == YesNo.NO
                                                                                            && gocf.TradingProviderId == tradingProviderId);

                        var contractCodeGen = orderContractFiles.Select(ocf => ocf.ContractCodeGen).Distinct();
                        if (contractCodeGen.Count() == 1)
                        {
                            garnerOrder.GenContractCode = contractCodeGen.First();
                        }
                        item.ContractCode = garnerOrder.GenContractCode ?? garnerOrder.ContractCode;
                    }
                    listRequestPayment.Add(item);
                }
            }
            result.Items = listRequestPayment;
            result.Items = result.Items;
            result.TotalItems = requestDetails.TotalItems;
            return result;
        }

        public async Task SendOtp(long requestId, int tradingBankAccountId)
        {
            _logger.LogInformation($"{nameof(SendOtp)}: orderId = {requestId}, tradingBankAccountId = {tradingBankAccountId}");
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == tradingBankAccountId && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO)
                   .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotFound);

            await _msbPayMoneyServices.TransferProcess(requestId, prefixAccount.TId, prefixAccount.MId, prefixAccount.AccessCode);
        }

        public async Task<List<ResponseInquiryBatchDetailDto>> InquiryBatch(long requestId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            MsbRequestPayment request = _msbRequestPaymentEFRepository.FindById(requestId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.PaymentRequestNotFound);

            MsbRequestPaymentDetail paymentDetailFirst = _msbRequestPaymentDetailEFRepository.EntityNoTracking.FirstOrDefault(d => d.RequestId == requestId)
                .ThrowIfNull(_dbContext, ErrorCode.PaymentRequestDetailNotFound);

            var prefixAccount = _tradingMSBPrefixAccountEFRepository.EntityNoTracking
                .FirstOrDefault(a => a.TradingBankAccountId == paymentDetailFirst.TradingBankAccId && a.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotFound, paymentDetailFirst.TradingBankAccId);

            return await _msbPayMoneyServices.InquiryBatch(new InquiryBatchDto
            {
                RequestId = requestId,
                MId = prefixAccount.MId,
                TId = prefixAccount.TId,
                AccessCode = prefixAccount.AccessCode,
            });
        }
    }
}
