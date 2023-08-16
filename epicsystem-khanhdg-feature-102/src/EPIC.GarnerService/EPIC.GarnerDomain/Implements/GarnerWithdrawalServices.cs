using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateApp;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Implements;
using EPIC.InvestEntities.DataEntities;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.PayMoney;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.PaymentRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Hangfire;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerWithdrawalServices : IGarnerWithdrawalServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly IGarnerFormulaServices _garnerFormulaServices;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerCalendarEFRepository _garnerCalendarEFRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly GarnerWithdrawalDetailEFRepository _garnerWithdrawalDetailEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly SysVarEFRepository _sysVarRepository;
        private readonly AuthOtpEFRepository _authOtpEFRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IGarnerContractDataServices _garnerContractDataServices;
        private readonly GarnerBackgroundJobServices _garnerBackgroundJobService;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly BankEFRepository _bankEFRepository;
        private readonly GarnerNotificationServices _garnerNotificationServices;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateEFRepository;


        //MSB
        private readonly MsbRequestPaymentEFRepository _msbRequestPaymentEFRepository;
        private readonly MsbRequestPaymentDetailEFRepository _msbRequestPaymentDetailEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;
        private readonly MsbPayMoneyServices _msbPayMoneyServices;

        public GarnerWithdrawalServices(
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<GarnerWithdrawalServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IGarnerFormulaServices garnerFormulaServices,
            IBackgroundJobClient backgroundJobs,
            GarnerBackgroundJobServices garnerBackgroundJobService,
            GarnerNotificationServices garnerNotificationServices,
            MsbPayMoneyServices msbPayMoneyServices,
            IGarnerContractDataServices garnerContractDataServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _garnerFormulaServices = garnerFormulaServices;
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerCalendarEFRepository = new GarnerCalendarEFRepository(dbContext, logger);
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, logger);
            _garnerWithdrawalDetailEFRepository = new GarnerWithdrawalDetailEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _authOtpEFRepository = new AuthOtpEFRepository(dbContext, logger);
            _sysVarRepository = new SysVarEFRepository(dbContext);
            _backgroundJobs = backgroundJobs;
            _garnerBackgroundJobService = garnerBackgroundJobService;
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _bankEFRepository = new BankEFRepository(dbContext, logger);
            _garnerNotificationServices = garnerNotificationServices;
            //
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerContractTemplateEFRepository = new GarnerContractTemplateEFRepository(dbContext, logger);

            _garnerContractDataServices = garnerContractDataServices;
            //
            _msbRequestPaymentEFRepository = new MsbRequestPaymentEFRepository(dbContext, logger);
            _msbRequestPaymentDetailEFRepository = new MsbRequestPaymentDetailEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
            _msbPayMoneyServices = msbPayMoneyServices;
        }

        public PagingResult<GarnerWithdrawalDto> FindAll(FilterGarnerWithdrawalDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

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
            // Nếu không phải Parner thì tradingProviderIds = null
            input.TradingProviderIds = (partnerId != null) ? input.TradingProviderIds : null;
            var listPolicy = new List<GarnerPolicyDto>();
            var resultPaging = new PagingResult<GarnerWithdrawalDto>();
            var findWithdraw = _garnerWithdrawalEFRepository.FindAll(input, tradingProviderId);

            resultPaging.Items = _mapper.Map<List<GarnerWithdrawalDto>>(findWithdraw.Items);
            resultPaging.TotalItems = findWithdraw.TotalItems;

            var withdrawalList = new List<GarnerWithdrawalDto>();

            foreach (var item in resultPaging.Items)
            {
                var withdrawal = new GarnerWithdrawalDto();
                withdrawal = _mapper.Map<GarnerWithdrawalDto>(item);

                var policyFind = _garnerPolicyEFRepository.FindById(item.PolicyId);
                if (policyFind == null)
                {
                    _logger.LogError($" PagingResult<GarnerWithdrawalDto> {nameof(FindAll)}: withdrawalId = {item.Id}, không tìm thấy thông tin chính sách: policyId = {item.PolicyId}");
                }
                else
                {
                    withdrawal.Policy = _mapper.Map<GarnerPolicyDto>(policyFind);
                }

                //Lấy thông tin khách hàng
                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(item.CifCode);

                if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomerInfo = _businessCustomerEFRepository.FindById(cifCodeFind.BusinessCustomerId ?? 0);
                    withdrawal.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerInfo);
                    withdrawal.BankAccountInfo = _investorBankAccountEFRepository.GetBankById(withdrawal.BankAccountId);
                }
                else if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    var investorInfo = _investorEFRepository.FindById(cifCodeFind.InvestorId ?? 0);
                    withdrawal.Investor = _mapper.Map<InvestorDto>(investorInfo);
                    var findIdentification = _investorEFRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0);
                    if (findIdentification != null && investorInfo != null)
                    {
                        item.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(findIdentification);
                    }
                    withdrawal.BankAccountInfo = _businessCustomerBankEFRepository.GetBankById(withdrawal.BankAccountId);
                }

                var withdrawalDetail = _garnerWithdrawalDetailEFRepository.FindByWithdrawalId(item.Id);
                withdrawal.WithdrawalDetail = new List<GarnerWithdrawalOrderDetailDto>();
                foreach (var withdrawalDetailItem in withdrawalDetail)
                {
                    var withdrawalDetailResult = new GarnerWithdrawalOrderDetailDto();
                    var orderFind = _garnerOrderEFRepository.FindById(withdrawalDetailItem.OrderId);
                    if (orderFind == null)
                    {
                        _logger.LogError($" PagingResult<GarnerWithdrawalDto> {nameof(FindAll)}: withdrawalId = {item.Id}, không tìm thấy thông tin hợp đồng: orderId = {withdrawalDetailItem.OrderId}");
                        continue;
                    }
                    withdrawalDetailResult = _mapper.Map<GarnerWithdrawalOrderDetailDto>(withdrawalDetailItem);
                    withdrawalDetailResult.GarnerOrder = _mapper.Map<GarnerOrderDto>(orderFind);
                    withdrawal.WithdrawalDetail.Add(withdrawalDetailResult);
                }

                var distribution = _garnerDistributionEFRepository.FindById(item.DistributionId, tradingProviderId);
                if (distribution != null)
                {
                    var product = _garnerProductEFRepository.FindById(distribution.ProductId);
                    if (product != null)
                    {
                        withdrawal.Product = _mapper.Map<GarnerProductDto>(product);
                    }
                }
                if (input.ContractCode != null)
                {
                    withdrawal.WithdrawalDetail = item.WithdrawalDetail.Where(o => o.GarnerOrder != null 
                                                                                   && (input.ContractCode == null 
                                                                                       || o.GarnerOrder.ContractCode == input.ContractCode 
                                                                                       || _dbContext.GarnerOrderContractFiles.Any(ocf => ocf.OrderId == o.GarnerOrder.Id && ocf.ContractCodeGen == input.ContractCode && ocf.Deleted == YesNo.NO))).ToList();
                }

                withdrawal.ActuallyProfit = item.WithdrawalDetail.Sum(w => w.ActuallyProfit);
                withdrawal.AmountReceived = item.WithdrawalDetail.Sum(w => w.AmountReceived);


                if (withdrawal.WithdrawalDetail.Any())
                {
                    withdrawalList.Add(withdrawal);
                }
            }

            if(input.Phone != null)
            {
                withdrawalList = withdrawalList.Where(i => i.Investor != null && (input.Phone == null || i.Investor.Phone == input.Phone)).ToList();
                resultPaging.Items = withdrawalList;
            }

            resultPaging.TotalItems = withdrawalList.Count();
            var resultItem = withdrawalList.AsQueryable().OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                resultPaging.Items = resultItem.Skip(input.Skip).Take(input.PageSize);
            }
            return resultPaging;
        }

        /// <summary>
        /// Xem rút vốn tích lũy
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <returns></returns>
        public GarnerWithdrawalByPolicyDto GetOrderWithdrawalById(long withdrawalId)
        {
            var withdrawalFind = _garnerWithdrawalEFRepository.FindById(withdrawalId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerWithdrawalNotFound);
            var result = _garnerWithdrawalEFRepository.GetOrderWithdrawalById(withdrawalId);

            var policyFind = _garnerPolicyEFRepository.FindById(withdrawalFind.PolicyId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            // Nếu chính sách tính theo NET thì ko hiện ra App
            if (policyFind.CalculateType == CalculateTypes.NET)
            {
                result.Tax = 0;
            }
            result.InvestorBankAccount = new();
            result.InvestorBankAccount = _investorBankAccountEFRepository.GetBankById(withdrawalFind.BankAccountId);  
            return result;
        }

        /// <summary>
        /// Yêu cầu rút vốn
        /// Nếu isApp = true kiểm tra xem có chi tự động không
        /// </summary>
        public async Task<CalculateGarnerWithdrawalDto> RequestWithdrawal(string cifCode, int policyId, decimal amount, DateTime withdrawDate, int source, int bankAccountId, bool isApp = false)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(RequestWithdrawal)}: cifCode = {cifCode}, policyId = {policyId}, amount = {amount}, source = {source}, username = {username}");

            var transaction = _dbContext.Database.BeginTransaction();
            var policy = _garnerPolicyEFRepository.FindById(policyId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var cifCodeFind = _cifCodeEFRepository.FindByCifCode(cifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            // Kiểm tra ngân hàng thụ hưởng
            if (cifCodeFind.InvestorId != null)
            {
                var investorBankAcc = _investorBankAccountEFRepository
                         .FindById(bankAccountId, cifCodeFind.InvestorId ?? 0)
                         .ThrowIfNull(_dbContext, ErrorCode.InvestorBankAccNotFound, bankAccountId);
            }
            else if (cifCodeFind.BusinessCustomerId != null)
            {
                var businessBankAcc = _businessCustomerBankEFRepository
                       .FindById(bankAccountId, cifCodeFind.BusinessCustomerId ?? 0)
                       .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, bankAccountId);
            }

            //var withdrawalFind = _garnerWithdrawalEFRepository.Entity.Where(e => e.CifCode == cifCode && e.Id == policyId && e.Status == WithdrawalStatus.YEU_CAU && e.Deleted == YesNo.NO);
            var withdrawalFind = _garnerWithdrawalEFRepository.GetOrderWithdrawalByPolicyId(cifCodeFind.InvestorId == null ? cifCodeFind.BusinessCustomerId.Value : cifCodeFind.InvestorId.Value, policyId, new List<int>{ WithdrawalStatus.YEU_CAU, WithdrawalStatus.CHO_PHAN_HOI});
            if (withdrawalFind != null)
            {
                _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerOrderWithdrawIsRequest);
            }

            var totalAmountMoney = _garnerOrderEFRepository.Entity.Where(e => e.PolicyId == policyId && e.CifCode == cifCode && e.Deleted == YesNo.NO && e.Status == OrderStatus.DANG_DAU_TU).Sum(e => e.TotalValue);

            if (totalAmountMoney < amount)
            {
                _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawAmountIsMoreThanTotalValue);
            }

            var withdrawal = _garnerWithdrawalEFRepository.Add(new GarnerWithdrawal
            {
                TradingProviderId = policy.TradingProviderId,
                DistributionId = policy.DistributionId,
                BankAccountId = bankAccountId,
                PolicyId = policyId,
                CifCode = cifCode,
                AmountMoney = amount,
                Source = source,
                WithdrawalDate = withdrawDate,
                CreatedBy = username,
            });
            _dbContext.SaveChanges();

            List<GarnerOrderWithdrawalDto> orderWithdraw = _garnerWithdrawalEFRepository.CalOrderWithdraw(cifCode, policyId, amount);
            CalculateGarnerWithdrawalDto result = _garnerFormulaServices.CalculateWithdrawal(orderWithdraw, withdrawal.WithdrawalDate);
            foreach (var item in orderWithdraw)
            {
                var ketQuaTinh = result.GarnerWithdrawalDetails.FirstOrDefault(d => d.OrderId == item.Order.Id);
                _garnerWithdrawalDetailEFRepository.Add(new GarnerWithdrawalDetail
                {
                    OrderId = item.Order.Id,
                    AmountMoney = item.AmountMoney,
                    WithdrawalId = withdrawal.Id,
                    Profit = ketQuaTinh.Profit,
                    DeductibleProfit = ketQuaTinh.DeductibleProfit,
                    ActuallyProfit = ketQuaTinh.ActuallyProfit,
                    Tax = ketQuaTinh.Tax,
                    WithdrawalFee = ketQuaTinh.WithdrawalFee,
                    AmountReceived = ketQuaTinh.AmountReceived,
                    ProfitRate = ketQuaTinh.ProfitRate
                });
                //lấy dữ liệu cho hợp đồng rút
                var data = _garnerContractDataServices.GetDataContractFile(item.Order.Id, item.Order.TradingProviderId, true);
                _backgroundJobs.Enqueue(() => _garnerBackgroundJobService.CreateOrUpdateContractFileByWithDrawalApp(item.Order.Id, item.Order.TradingProviderId, data, withdrawal.Id));
            }
            _dbContext.SaveChanges();
            transaction.Commit();

            // Chi tự động 
            var bankPaymentAuto = (from distributionTradingBankAccount in _dbContext.GarnerDistributionTradingBankAccounts
                                   join tradingMsbPrefixAccount in _dbContext.TradingMSBPrefixAccounts on distributionTradingBankAccount.BusinessCustomerBankAccId equals tradingMsbPrefixAccount.TradingBankAccountId
                                   where distributionTradingBankAccount.DistributionId == policy.DistributionId && distributionTradingBankAccount.Status == Status.ACTIVE
                                     && distributionTradingBankAccount.Type == DistributionTradingBankAccountTypes.CHI && distributionTradingBankAccount.Deleted == YesNo.NO
                                     && tradingMsbPrefixAccount.Deleted == YesNo.NO && tradingMsbPrefixAccount.TradingProviderId == policy.TradingProviderId
                                     && distributionTradingBankAccount.IsAuto == YesNo.YES && tradingMsbPrefixAccount.TIdWithoutOtp != null
                                   select distributionTradingBankAccount).FirstOrDefault();
            if (bankPaymentAuto != null && isApp)
            {
                try
                {
                    var testWithdrawal = await PrepareApproveRequestWithdrawal(new PrepareApproveRequestWithdrawalDto
                    {
                        TradingBankAccId = bankPaymentAuto.BusinessCustomerBankAccId,
                        WithdrawalIds = new List<long> { withdrawal.Id }
                    }, policy.TradingProviderId, isApp);
                }
                catch
                {

                }
            }
            if (source == SourceOrder.ONLINE)
            {
                //thông báo có khách yêu cầu rút
                await _garnerNotificationServices.SendNotifyAdminCustomerWithdrawal(withdrawal.Id);
            }
            return result;
        }

        /// <summary>
        /// Xem thay đổi trước khi gửi yêu cầu rút vốn
        /// </summary>
        /// <returns></returns>
        public ViewCalculateGarnerWithdrawalDto ViewChangeRequestWithdrawal(int policyId, decimal amountMoney)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(ViewChangeRequestWithdrawal)}: investorId = {investorId}, policyId = {policyId}, amount = {amountMoney}, username = {username}");

            ViewCalculateGarnerWithdrawalDto result = new();
            var cifCodeFind = _cifCodeEFRepository.FindByInvestor(investorId).ThrowIfNull<CifCodes>(_dbContext, ErrorCode.CoreCifCodeNotFound);
            var policy = _garnerPolicyEFRepository.FindById(policyId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);

            var withdrawalFind = _garnerWithdrawalEFRepository.GetOrderWithdrawalByPolicyId(investorId, policyId, new List<int> { WithdrawalStatus.YEU_CAU, WithdrawalStatus.CHO_PHAN_HOI });
            if (withdrawalFind != null)
            {
                _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerOrderWithdrawIsRequest);
            }

            var totalAmountMoney = _garnerOrderEFRepository.Entity.Where(e => e.PolicyId == policyId && e.CifCode == cifCodeFind.CifCode && e.Deleted == YesNo.NO && e.Status == OrderStatus.DANG_DAU_TU).Sum(e => e.TotalValue);
            if (totalAmountMoney < amountMoney)
            {
                _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawAmountIsMoreThanTotalValue);
            }
            List<GarnerOrderWithdrawalDto> orderWithdraw = _garnerWithdrawalEFRepository.CalOrderWithdraw(cifCodeFind.CifCode, policyId, amountMoney);
            CalculateGarnerWithdrawalDto resultCalculate = _garnerFormulaServices.CalculateWithdrawal(orderWithdraw, DateTime.Now.Date);
            result = _mapper.Map<ViewCalculateGarnerWithdrawalDto>(resultCalculate);
            
            // Nếu chính sách tính theo NET thì ko hiện ra App
            if (policy.CalculateType == CalculateTypes.NET)
            {
                result.Tax = 0;
            }    

            var orderSources = new List<string>();
            var orderContractFileWithdrawals = new List<GarnerContractTemplatesWithdrawalDto>();
            foreach (var withdrawalDetail in resultCalculate.GarnerWithdrawalDetails)
            {
                var order = _garnerOrderEFRepository.FindById(withdrawalDetail.OrderId);
                if (order != null)
                {
                    orderSources.Add(order.ContractCode);
                    GarnerContractTemplatesWithdrawalDto garnerContractTemplatesWithdrawal = new();
                    garnerContractTemplatesWithdrawal.OrderId = order.Id;
                    garnerContractTemplatesWithdrawal.ContractCode = order.ContractCode;
                    //Lấy danh sách mẫu hợp đồng rút theo
                    var contractTemplate = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(order.PolicyId, SharedContractTemplateType.INVESTOR, null, ContractTypes.RUT_TIEN, null, SourceOrder.ONLINE, Status.ACTIVE);
                    garnerContractTemplatesWithdrawal.GarnerContractTemplates = _mapper.Map<List<GarnerContractTemplateAppDto>>(contractTemplate);
                    orderContractFileWithdrawals.Add(garnerContractTemplatesWithdrawal);
                }
            }
            result.ListBanks = _garnerFormulaServices.FindListBankOfCifCode(cifCodeFind.CifCode);
            result.OrderSources = orderSources;
            result.OrderContractFileWithdrawals = orderContractFileWithdrawals;
            return result;
        }

        /// <summary>
        /// Thay đổi trạng thái ngân hàng khi Duyệt đi tiền
        /// </summary>
        /// <param name="ids"></param>
        public void ApproveChangeStatusBank (List<long> ids)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ApproveChangeStatusBank)}: input = {JsonSerializer.Serialize(ids)}, tradingProviderId = {tradingProviderId}");
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var id in ids)
            {
                var withdrawalFind = _garnerWithdrawalEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerWithdrawalNotFound);
                //Trạng thái có nằm trong trạng thái chi không
                if (withdrawalFind.Status != WithdrawalStatus.DUYET_DI_TIEN)
                {
                    _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawalStatusNotApprovePay);
                }
                //Trạng thái bank có trong chờ phản hồi không
                if (withdrawalFind.StatusBank != MsbBankStatus.INIT)
                {
                    _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawalStatusBankNotInit);
                }
                // Đổi trạng thái từ chờ phản hồi sang thành công
                withdrawalFind.StatusBank = MsbBankStatus.SUCCESS;
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public async Task<CalculateGarnerWithdrawalDto> AppRequestWithdrawal(AppWithdrawalRequestDto input)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppRequestWithdrawal)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}");

            var maxOtpFailCount = _sysVarRepository.GetInvValueByName("AUTH", "OTP_INVALID_COUNT");
            //_authOtpEFRepository.CheckOtpByUserId(CommonUtils.GetCurrentUserId(_httpContext), input.Otp, _httpContext, SessionKeys.OTP_FAIL_COUNT, maxOtpFailCount);

            var cifCodeFind = _cifCodeEFRepository.FindByInvestor(investorId)
                .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            var totalAmountMoney = _garnerOrderEFRepository.Entity.Where(e => e.PolicyId == input.PolicyId && e.Deleted == YesNo.NO && e.Status == OrderStatus.DANG_DAU_TU).Sum(e => e.TotalValue);

            if (totalAmountMoney < input.AmountMoney)
            {
                _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawAmountIsMoreThanTotalValue);
            }
            var result = await RequestWithdrawal(cifCodeFind.CifCode, input.PolicyId, input.AmountMoney, input.WithdrawDate ?? DateTime.Now, SourceOrder.ONLINE, input.BankAccountId, true);
            return result;
        }

        public async Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestWithdrawal(PrepareApproveRequestWithdrawalDto input, int? tradingProviderId = null, bool isApp = false)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            if (tradingProviderId == null)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }    
            _logger.LogInformation($"{nameof(PrepareApproveRequestWithdrawal)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            List<int> distributionIds = new();
            var withdrawals = new List<GarnerWithdrawal>();
            var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == input.TradingBankAccId && e.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);

            var prepareResult = new MsbRequestPaymentWithErrorDto()
            {
                Id = (long)_msbRequestPaymentEFRepository.NextKey(),
                PrefixAccount = prefixAccount.PrefixMsb,
                Details = new(),
            };
            foreach (long withdrawalId in input.WithdrawalIds)
            {
                // Kiểm tra xem đã có yêu cầu chi tiền trước đó mà không thành công không
                var checkRequestPayment = _dbContext.MsbRequestPaymentDetail.Any(p => p.DataType == RequestPaymentDataTypes.GAN_WITHDRAWAL && p.ReferId == withdrawalId 
                                            && p.Exception == null && p.Status == RequestPaymentStatus.KHOI_TAO);
                if (checkRequestPayment)
                {
                    _garnerWithdrawalEFRepository.ThrowException(ErrorCode.PaymentMsbNotificationExistRequestNotSuccess, withdrawalId);
                }   
                
                var withdrawal = _garnerWithdrawalEFRepository.FindById(withdrawalId, tradingProviderId)
                    .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderWithdrawalNotFound, withdrawalId);
                withdrawals.Add(withdrawal);
                if (withdrawal.Status != WithdrawalStatus.YEU_CAU && withdrawal.Status != WithdrawalStatus.CHO_PHAN_HOI)
                {
                    _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawalNotInRequest);
                }

                var withdrawalDetailQuery = _garnerWithdrawalDetailEFRepository.EntityNoTracking.Where(d => d.WithdrawalId == withdrawal.Id);
                if (!withdrawalDetailQuery.Any())
                {
                    _garnerWithdrawalDetailEFRepository.ThrowException(ErrorCode.GarnerWithdrawalNotContaintDetail, $"WithdrawalId = {withdrawal.Id}");
                }
                distributionIds.Add(withdrawal.DistributionId);
                //lấy lệnh đầu tiên
                var withdrawalDetailFirst = _garnerWithdrawalDetailEFRepository
                    .EntityNoTracking
                    .FirstOrDefault(d => d.WithdrawalId == withdrawal.Id)
                    .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderWithdrawalNotFound, withdrawalId);

                var orderFirst = _garnerOrderEFRepository
                    .FindById(withdrawalDetailFirst.OrderId, tradingProviderId)
                    .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound, withdrawalDetailFirst.OrderId);

                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(orderFirst.CifCode)
                    .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound, orderFirst.CifCode);

                var prepareResultDetail = new MsbRequestDetailWithErrorDto
                {
                    Id = (long)_msbRequestPaymentDetailEFRepository.NextKey(),
                    DataType = RequestPaymentDataTypes.GAN_WITHDRAWAL,
                    ReferId = withdrawalId,
                    AmountMoney = withdrawalDetailQuery.Sum(d => d.AmountReceived), //cộng tiền thực nhận của từng lệnh vào
                };

                if (cifCodeFind.InvestorId != null) //khách cá nhân
                {
                    var investorBankAcc = _investorBankAccountEFRepository
                        .FindById(withdrawal.BankAccountId, cifCodeFind.InvestorId)
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
                       .FindById(withdrawal.BankAccountId, cifCodeFind.BusinessCustomerId)
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
                TId = isApp ? prefixAccount.TIdWithoutOtp : prefixAccount.TId,
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
                    prepareResult.IsSuccess = resultRequest.ErrorDetails.Count == 0 && prepareResult.Details.Count(d => d.Bin == null) == 0;
                    if (resultRequest != null && !resultRequest.IsSubmitOtp && prepareResult.IsSuccess)
                    {
                        var requestPaymentInsert = _msbRequestPaymentEFRepository.Add(new MsbRequestPayment
                        {
                            Id = prepareResult.Id,
                            RequestType = RequestPaymentTypes.RUT_VON,
                            ProductType = ProductTypes.GARNER,
                            TradingProdiverId = tradingProviderId ?? 0,
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
                                Status = RequestPaymentStatus.SUCCESS,
                                BankId = detailItem.BankId,
                                OwnerAccount = detailItem.OwnerAccount,
                                OwnerAccountNo = detailItem.BankAccount,
                                Note = $"{PaymentNotes.THANH_TOAN}{detailItem.Id}",
                                Bin = detailItem.Bin,
                                AmountMoney = detailItem.AmountMoney,
                                TradingBankAccId = input.TradingBankAccId ?? 0
                            });
                        }
                        AddOrderPaymentPay(tradingProviderId ?? 0, withdrawals, WithdrawalStatus.DUYET_DI_TIEN, input.TradingBankAccId, username, null, MsbBankStatus.SUCCESS);
                        _dbContext.SaveChanges();
                    }
                }
                catch(Exception ex)
                {
                    var requestPaymentInsert = _msbRequestPaymentEFRepository.Add(new MsbRequestPayment
                    {
                        Id = prepareResult.Id,
                        RequestType = RequestPaymentTypes.RUT_VON,
                        ProductType = ProductTypes.GARNER,
                        TradingProdiverId = tradingProviderId ?? 0,
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
                            AmountMoney = detailItem.AmountMoney,
                            TradingBankAccId = input.TradingBankAccId ?? 0
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
                prepareResult.IsSubmitOtp = resultRequest.IsSubmitOtp;
            }
            return prepareResult;
        }

        public async Task ApproveRequestWithdrawal(GarnerApproveRequestWithdrawalDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(ApproveRequestWithdrawal)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId ={tradingProviderId}, username = {username}");

            List<GarnerWithdrawal> withdrawals = new();
            MsbRequestPayment requestPayment = new MsbRequestPayment();
            List<MsbRequestPaymentDetail> listRequestPaymentDetails = new();

            foreach (var withdrawalId in input.WithdrawalIds)
            {
                var withdrawal = _garnerWithdrawalEFRepository.FindById(withdrawalId)
                                         .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderWithdrawalNotFound);
                if (withdrawal.Status != WithdrawalStatus.YEU_CAU && withdrawal.Status != WithdrawalStatus.CHO_PHAN_HOI)
                {
                    _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawalNotInRequest);
                }
                if (input.Prepare != null && !input.Prepare.Details.Select(p => p.ReferId).Contains(withdrawalId))
                {
                    _garnerWithdrawalEFRepository.ThrowException(ErrorCode.GarnerWithdrawalIdNotInPrepareDetail);
                }    
                withdrawals.Add(withdrawal);
            }

            //trừ tiền lệnh
            if (input.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
            {
                var transaction = _dbContext.Database.BeginTransaction();
                withdrawals = AddOrderPaymentPay(tradingProviderId, withdrawals, input.Status, input.TradingBankAccId, username, input.ApproveNote);
                transaction.Commit();
                foreach (var withdrawalItem in withdrawals)
                {
                    await _garnerNotificationServices.SendNotifyGarnerOrderWithdraw(withdrawalItem.Id);
                }
            }
            else if (input.Status == WithdrawalStatus.HUY_YEU_CAU)
            {
                foreach (var withdrawal in withdrawals)
                {
                    withdrawal.Status = input.Status;
                    withdrawal.CancelBy = username;
                    withdrawal.CancelDate = DateTime.Now;
                }
            }
            _dbContext.SaveChanges();

            if (input.Status == WithdrawalStatus.DUYET_DI_TIEN)
            {
                await ApproveWithdrawal(withdrawals, input.Prepare, tradingProviderId, input.TradingBankAccId ?? 0, username, input.Otp);
            }
        }

        #region Func
            /// <summary>
            /// Duyệt chi tiền tự động sang bank
            /// </summary>
            /// <param name="withdrawals">Danh sách thông tin rút</param>
            /// <param name="prepare">Thông tin chuẩn bị thành công</param>
            /// <param name="tradingProviderId"> Id đại lý</param>
            /// <param name="tradingBankAccId">Ngân hàng chi tiền</param>
            /// <param name="username">Người tạo rút</param>
            /// <param name="otp"></param>
            /// <returns></returns>
        public async Task ApproveWithdrawal(List<GarnerWithdrawal> withdrawals, MsbRequestPaymentWithErrorDto prepare,int tradingProviderId, int tradingBankAccId, string username, string otp = null)
        {
            _logger.LogInformation($"{nameof(ApproveWithdrawal)}: withdrawals = {JsonSerializer.Serialize(withdrawals)}, withdrawals = {JsonSerializer.Serialize(prepare)}, tradingProviderId ={tradingProviderId}, tradingBankAccId = {tradingBankAccId}, username = {username}");
            MsbRequestPayment requestPayment = new MsbRequestPayment();
            List<MsbRequestPaymentDetail> listRequestPaymentDetails = new();
            var transaction = _dbContext.Database.BeginTransaction();
            // Chuyển sang trạng thái tạm khi duyệt đi tiền
            // Notifi thành công thì chuyển sang trạng thái duyệt đi tiền
            var waitResponse = WithdrawalStatus.CHO_PHAN_HOI;
            //Lập thông tin BondOrder Payment loại chi tiền và gửi thông báo
            withdrawals = AddOrderPaymentPay(tradingProviderId, withdrawals, waitResponse, tradingBankAccId, username, null, MsbBankStatus.INIT);

            var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == tradingBankAccId && e.Deleted == YesNo.NO)
               .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);

            //lấy thông tin đểu lưu vào lô chi hộ
            requestPayment.Id = prepare.Id;
            requestPayment.TradingProdiverId = tradingProviderId;
            requestPayment.ProductType = ProductTypes.GARNER;
            requestPayment.RequestType = RequestPaymentTypes.RUT_VON;
            requestPayment.CreatedDate = DateTime.Now;
            requestPayment.CreatedBy = username;

            _msbRequestPaymentEFRepository.Add(requestPayment);
            foreach (var requestDetail in prepare.Details)
            {
                var requestPaymentDetail = new MsbRequestPaymentDetail()
                {
                    Id = requestDetail.Id,
                    RequestId = requestPayment.Id,
                    DataType = RequestPaymentDataTypes.GAN_WITHDRAWAL,
                    ReferId = requestDetail.ReferId,
                    Status = RequestPaymentStatus.KHOI_TAO,
                    BankId = requestDetail.BankId,
                    Bin = requestDetail.Bin,
                    Note = $"{PaymentNotes.THANH_TOAN}{requestDetail.Id}",
                    OwnerAccount = requestDetail.OwnerAccount,
                    OwnerAccountNo = requestDetail.BankAccount,
                    TradingBankAccId = tradingBankAccId,
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
                    Otp = otp,
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

        /// <summary>
        /// Thêm thông tin chi tiền của BondOrderPayment, lấy dữ liệu cho hợp đồng và gửi thông báo cho nhà đầu tư
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="withdrawals"></param>
        /// <param name="status"> trạng thái truyền vào</param>
        /// <param name="tradingBankAccId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<GarnerWithdrawal> AddOrderPaymentPay(int tradingProviderId, List<GarnerWithdrawal> withdrawals, int status, int? tradingBankAccId, string username, int? approveNote, int? statusBank = null)
        {
            _logger.LogInformation($"{nameof(AddOrderPaymentPay)}: withdrawals = {JsonSerializer.Serialize(withdrawals)}, tradingProviderId = {tradingProviderId}, status = {status}, tradingBankAccId = {tradingBankAccId}, username = {username}");
            foreach (var withdrawalItem in withdrawals)
            {
                DateTime now = DateTime.Now;
                withdrawalItem.Status = status;
                withdrawalItem.ApproveBy = username;
                withdrawalItem.ApproveDate = now;
                withdrawalItem.ApproveNote = approveNote;
                if (statusBank != null)
                {
                    withdrawalItem.StatusBank = statusBank ?? MsbBankStatus.INIT;
                }

                var details = _garnerWithdrawalDetailEFRepository.FindByWithdrawalId(withdrawalItem.Id);
                // Trạng thái duyệt ko đi tiền hoặc đi tiền xử lý các thông tin liên quan đến rút tiền
                foreach (var detail in details)
                {
                    var order = _garnerOrderEFRepository.FindById(detail.OrderId)
                        .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
                    //lấy dữ liệu cho hợp đồng rút
                    var data = _garnerContractDataServices.GetDataContractFile(order.Id, order.TradingProviderId, true);
                    _backgroundJobs.Enqueue(() => _garnerBackgroundJobService.CreateOrUpdateContractFileByWithDrawalApp(order.Id, order.TradingProviderId, data, withdrawalItem.Id));

                    if (withdrawalItem.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN || withdrawalItem.Status == WithdrawalStatus.DUYET_DI_TIEN)
                    {
                        // Kiểm tra số tiền còn lại của mỗi hợp đồng
                        if (order.TotalValue == detail.AmountMoney)
                        {
                            order.TotalValue = 0;
                            order.Status = OrderStatus.TAT_TOAN;
                            order.SettlementDate = now;
                        }
                        else if (order.TotalValue > detail.AmountMoney)
                        {
                            order.TotalValue -= detail.AmountMoney;
                        }
                        //Thêm chi (GarnerOrderPayment)
                        var orderPaymentSpend = new GarnerOrderPayment()
                        {
                            TradingProviderId = tradingProviderId,
                            OrderId = order.Id,
                            TranDate = now,
                            TranType = TranTypes.CHI,
                            TranClassify = TranClassifies.RUT_VON,
                            PaymentType = PaymentTypes.CHUYEN_KHOAN,
                            PaymentAmount = detail.AmountMoney,
                            Status = OrderPaymentStatus.DA_THANH_TOAN,
                            Description = PaymentNotes.CHI_RUT_VON + order.ContractCode,
                            CreatedBy = username,
                            CreatedDate = now,
                            ApproveBy = username,
                            ApproveDate = now,
                            TradingBankAccId = tradingBankAccId,
                        };
                        _garnerOrderPaymentEFRepository.Add(orderPaymentSpend, username, tradingProviderId);
                    }
                }

                if (withdrawalItem.Status == WithdrawalStatus.CHO_PHAN_HOI || withdrawalItem.Status == WithdrawalStatus.DUYET_DI_TIEN)
                {
                    _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate
                    {
                        UpdateTable = GarnerHistoryUpdateTables.GAN_WITHDRAWAL,
                        RealTableId = withdrawalItem.Id,
                        Action = ActionTypes.DUYET,
                        Summary = $"{username} thực hiện duyệt chi rút vốn tự động: {details.Sum(d => d.AmountReceived)}"
                    }, username);
                }
                _dbContext.SaveChanges();
            }
            return withdrawals;
        }
        #endregion
    }
}
