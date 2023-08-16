using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.RocketChat;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.Withdrawal;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
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
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.InvestDomain.Implements
{
    public class WithdrawalServices : IWithdrawalServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<WithdrawalServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly WithdrawalRepository _withdrawalRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly InvestPolicyRepository _investPolicyRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestWithdrawalEFRepository _investWithdrawalEFRepository;
        private readonly InvestOrderPaymentEFRepository _investOrderPaymentEFRepository;
        private readonly MsbRequestPaymentEFRepository _msbRequestPaymentEFRepository;
        private readonly MsbRequestPaymentDetailEFRepository _msbRequestPaymentDetailEFRepository;
        private readonly InvestOrderEFRepository _invOrderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly BankEFRepository _bankEFRepository;
        private readonly MsbPayMoneyServices _msbPayMoneyServices;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly IContractTemplateServices _contractTemplateServices;
        private readonly IContractDataServices _contractDataServices;
        private readonly NotificationServices _notificationServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly InvestNotificationServices _investNotificationServices;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        private readonly IInvestOrderContractFileServices _investOrderContractFileServices;


        public WithdrawalServices(ILogger<WithdrawalServices> logger,
            EpicSchemaDbContext dbContext,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            NotificationServices notificationServices,
            InvestNotificationServices investNotificationServices,
            IInvestSharedServices investSharedServices,
            IContractTemplateServices contractTemplateServices,
            IContractDataServices contractDataServices,
            MsbPayMoneyServices msbPayMoneyServices,
            IInvestContractCodeServices investContractCodeServices,
            IInvestOrderContractFileServices investOrderContractFileServices,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _withdrawalRepository = new WithdrawalRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _investPolicyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investWithdrawalEFRepository = new InvestWithdrawalEFRepository(dbContext, logger);
            _investOrderPaymentEFRepository = new InvestOrderPaymentEFRepository(dbContext, logger);
            _msbRequestPaymentEFRepository = new MsbRequestPaymentEFRepository(dbContext, logger);
            _msbRequestPaymentDetailEFRepository = new MsbRequestPaymentDetailEFRepository(dbContext, logger);
            _invOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _bankEFRepository = new BankEFRepository(dbContext, logger);
            _msbPayMoneyServices = msbPayMoneyServices;
            _investSharedServices = investSharedServices;
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _notificationServices = notificationServices;
            _investNotificationServices = investNotificationServices;
            _httpContext = httpContext;
            _mapper = mapper;
            _investContractCodeServices = investContractCodeServices;
            _investOrderContractFileServices = investOrderContractFileServices;
        }

        /// <summary>
        /// Yêu cầu rút vốn trên CMS
        /// </summary>
        /// <param name="input"></param>
        public void WithdrawalAdd(CreateWithdrawalDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var requestIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            _withdrawalRepository.WithdrawalAdd(new Withdrawal
            {
                TradingProviderId = tradingProviderId,
                OrderId = input.OrderId,
                AmountMoney = input.AmountMoney,
                WithdrawalDate = input.WithdrawalDate,
                RequestIp = requestIp,
                ModifiedBy = username
            });
        }

        /// <summary>
        /// Tạo yêu cầu rút vốn từ khách hàng cá nhân trên App
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Withdrawal> AppWithdrawalRequest(AppWithdrawalRequestDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var requestIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            var result = _withdrawalRepository.AppWithdrawalRequest(new Withdrawal
            {
                OrderId = input.OrderId,
                AmountMoney = input.AmountMoney,
                WithdrawalDate = DateTime.Now,
                RequestIp = requestIp,
                ModifiedBy = username
            }, investorId, input.Otp);
            await _investNotificationServices.SendNotifyAdminCustomerWithdrawal(result.Id);
            return result;
        }

        /// <summary>
        /// Phê duyệt yêu cầu rút vốn, chưa có duyệt đi tiền
        /// </summary>
        public async Task WithdrawalApprove(InvestApproveRequestWithdrawalDto input)
        {
            _logger.LogInformation($"{nameof(WithdrawalApprove)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var approveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            List<ReplaceTextDto> data = new();
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var withdrawalId in input.WithdrawalIds)
            {
                var withdrawalFind = _investWithdrawalEFRepository.Entity.FirstOrDefault(w => w.Id == withdrawalId && w.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound);

                if (input.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                {
                    var withdrawalInfo = new RutVonDto();

                    var orderFind = _investOrderRepository.FindById(withdrawalFind.OrderId ?? 0, tradingProviderId);
                    if (orderFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
                    }
                    else if (orderFind != null)
                    {
                        if (orderFind.InvestDate == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy thông tin ngày đầu tư của lệnh"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                        }

                        bool khachHangCaNhan = false;

                        // Là khách hàng cá nhân hay là doanh nghiệp
                        var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                        if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                        {
                            khachHangCaNhan = true;
                        }

                        // Trạng thái rút vốn : YÊU CẦU
                        // Tìm chính sách kỳ hạn để tính số tiền thực nhận 
                        var policyFind = _investPolicyRepository.FindPolicyById(orderFind.PolicyId);
                        if (policyFind == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
                        }
                        var policyDetailFind = _investPolicyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
                        if (policyFind == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                        }
                        var distributionFind = _dbContext.InvestDistributions.FirstOrDefault(d => d.Id == policyFind.DistributionId)
                            .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);

                        if (withdrawalFind.Status == WithdrawalStatus.YEU_CAU)
                        {
                            var withdrawalHistory = _investSharedServices.RutVonInvest(orderFind, policyFind, policyDetailFind, orderFind.TotalValue, withdrawalFind.AmountMoney ?? 0, withdrawalFind.WithdrawalDate, khachHangCaNhan, distributionFind.CloseCellDate);
                            withdrawalInfo = withdrawalHistory;
                        }
                        //Get base data
                        data = _contractDataServices.GetDataContractFile(orderFind, tradingProviderId, true);
                        //Get data cho hợp đồng rút
                        data.AddRange(_contractDataServices.GetDataWithdrawalContractFile(orderFind, policyFind, policyDetailFind, orderFind.TotalValue, withdrawalFind.AmountMoney ?? 0, withdrawalFind.WithdrawalDate, khachHangCaNhan, distributionFind.CloseCellDate));

                    }
                    var withdrawalTimes = _investWithdrawalEFRepository.FindAll(withdrawalFind.OrderId ?? 0).Count;
                    //Duyệt rút vốn : actuallyAmount: Số tiền thực nhận khi rút
                    var approveWithdrawal = _investWithdrawalEFRepository.ApproveWithdrawal(new InvestWithdrawalApproveDto
                    {
                        Id = withdrawalId,
                        TradingProviderId = tradingProviderId,
                        ApproveIp = approveIp,
                        Username = username,
                        ActuallyAmount = withdrawalInfo.ActuallyAmount,
                        ActuallyProfit = withdrawalInfo.ActuallyProfit ?? 0,
                        DeductibleProfit = withdrawalInfo.ProfitReceived ?? 0,
                        Profit = withdrawalInfo.WithdrawalProfit ?? 0,
                        Tax = withdrawalInfo.IncomeTax ?? 0,
                        WithdrawalFee = withdrawalInfo.WithdrawalFee ?? 0,
                    });
                    _dbContext.SaveChanges();

                    //Thêm thanh toán chi cho BondOrder
                    _investOrderPaymentEFRepository.Add(new OrderPayment
                    {
                        TradingProviderId = tradingProviderId,
                        OrderId = orderFind.Id,
                        TranDate = DateTime.Now,
                        TranType = TranTypes.CHI,
                        TranClassify = TranClassifies.THANH_TOAN,
                        PaymentType = PaymentTypes.TIEN_MAT,
                        Status = OrderPaymentStatus.DA_THANH_TOAN,
                        PaymentAmnount = withdrawalInfo.ActuallyAmount,
                        Description = $"CRV {orderFind.ContractCode}",
                        CreatedBy = username,
                        CreatedDate = DateTime.Now,
                        ApproveDate = DateTime.Now,
                        ApproveBy = username,
                    });
                    //Lấy thêm data hợp đồng rút tiền
                    await CreateContractFileWithDrawal(orderFind, withdrawalId, withdrawalTimes, data);
                    // Gửi thông báo rút vốn invest thành công
                    if (approveWithdrawal.Type == WithdrawalTypes.RUT_VON)
                    {
                        await _investNotificationServices.SendEmailInvestWithdrawalSuccess(withdrawalId);
                    }
                    else if (approveWithdrawal.Type == WithdrawalTypes.TAT_TOAN)
                    {
                        await _investNotificationServices.SendEmailInvestPrePayment(withdrawalId);
                    }
                }
                withdrawalFind.Status = input.Status;
                _dbContext.SaveChanges();
            }
            transaction.Commit();
        }

        /// <summary>
        /// Hủy phê duyệt rút vốn
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int WithdrawalCancel(long id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _withdrawalRepository.WithdrawalCancel(id, tradingProviderId, username);
        }

        public PagingResult<WithdrawalDto> FindAll(InvestWithdrawalFilterDto input)
        {
            int? tradingProviderId = null;
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
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

            input.TradingProviderId = tradingProviderId;
            var withdrawalList = _investWithdrawalEFRepository.FindAllWithdrawal(input);
            var result = new PagingResult<WithdrawalDto>();
            var items = new List<WithdrawalDto>();

            var withdrawalGroupByOrder = withdrawalList.AsEnumerable().GroupBy(x => x.OrderId);
            foreach (var groupOrderItem in withdrawalGroupByOrder)
            {
                var orderFind = _investOrderRepository.FindById(groupOrderItem.Key ?? 0, tradingProviderId);
                if (orderFind == null)
                {
                    continue;
                }
                var stt = 0;
                foreach (var withdrawalItem in groupOrderItem.OrderBy(w => w.WithdrawalDate))
                {
                    var withdrawalItemResult = _mapper.Map<WithdrawalDto>(withdrawalItem);
                    stt = ++stt;
                    withdrawalItemResult.WithdrawalIndex = $"Rút vốn lần {stt}";
                    withdrawalItemResult.Index = stt;
                    withdrawalItemResult.Order = orderFind;
                    withdrawalItemResult.MethodInterest = orderFind?.MethodInterest;
                    var projectFind = _projectRepository.FindById(orderFind.ProjectId);
                    withdrawalItemResult.Project = _mapper.Map<ProjectDto>(projectFind);
                    withdrawalItemResult.GenContractCode = _invOrderEFRepository.GetContractCodeGen(orderFind.Id) ?? withdrawalItem.Order?.ContractCode;
                    // Trường hợp chưa duyệt rút tiền thf xem 
                    if ((withdrawalItem.Status == WithdrawalStatus.YEU_CAU || withdrawalItem.Status == WithdrawalStatus.CHO_PHAN_HOI) && orderFind.Status == OrderStatus.DANG_DAU_TU && orderFind.InvestDate != null)
                    {
                        bool khachHangCaNhan = false;

                        // Là khách hàng cá nhân hay là doanh nghiệp
                        var cifCodeFind = _cifCodeRepository.GetByCifCode(withdrawalItem.CifCode);
                        if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                        {
                            khachHangCaNhan = true;
                            var investorInfo = _investorEFRepository.FindById(cifCodeFind.InvestorId ?? 0);
                            withdrawalItemResult.Investor = _mapper.Map<InvestorDto>(investorInfo);
                            var findIdentification = _investorEFRepository.GetDefaultIdentification(investorInfo.InvestorId);
                            if (findIdentification != null)
                            {
                                withdrawalItemResult.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(findIdentification);
                            }
                            withdrawalItemResult.Name = withdrawalItemResult.Investor.InvestorIdentification.Fullname;

                        }
                        else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                        {
                            var businessCustomerInfo = _businessCustomerEFRepository.FindById(cifCodeFind.BusinessCustomerId ?? 0);
                            withdrawalItemResult.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerInfo);
                            withdrawalItemResult.Name = withdrawalItemResult.BusinessCustomer.Name;
                        }

                        // Trạng thái rút vốn : YÊU CẦU
                        // Tìm chính sách kỳ hạn để tính số tiền thực nhận 
                        var policyFind = _investPolicyRepository.FindPolicyById(orderFind.PolicyId);
                        var policyDetailFind = _investPolicyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
                        var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyFind.DistributionId && r.Deleted == YesNo.NO);
                        if (policyFind != null && policyDetailFind != null)
                        {
                            var withdrawalHistory = _investSharedServices.RutVonInvest(orderFind, policyFind, policyDetailFind, orderFind.TotalValue, withdrawalItemResult.AmountMoney ?? 0, withdrawalItemResult.WithdrawalDate, khachHangCaNhan, distribution?.CloseCellDate);
                            withdrawalItemResult.ActuallyAmount = withdrawalHistory.ActuallyAmount;
                            withdrawalItemResult.PolicyCalculateType = policyFind.CalculateType;
                            withdrawalItemResult.ActuallyProfit = withdrawalHistory.ActuallyProfit ?? 0;
                            withdrawalItemResult.DeductibleProfit = withdrawalHistory.ProfitReceived ?? 0;
                            withdrawalItemResult.Profit = withdrawalHistory.WithdrawalProfit ?? 0;
                            withdrawalItemResult.Tax = withdrawalHistory.IncomeTax ?? 0;
                            withdrawalItemResult.WithdrawalFee = withdrawalHistory.WithdrawalFee ?? 0;
                        }
                    }
                    items.Add(withdrawalItemResult);
                }
            }
            result.TotalItems = withdrawalList.Count();
            var resultItems = items.AsQueryable().OrderDynamic(input.Sort);
            //Phân trang
            if (input.PageSize != -1)
            {
                resultItems = resultItems.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = resultItems;
            return result;
        }

        /// <summary>
        /// Sinh hợp đồng rút tiền
        /// </summary>
        /// <param name="order"></param>
        /// <param name="withDrawalId"></param>s
        /// <param name="replaceTexts"></param>
        /// <returns></returns>
        public async Task CreateContractFileWithDrawal(InvOrder order, long withDrawalId, int withdrawalTimes, List<ReplaceTextDto> replaceTexts)
        {
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policy = _investPolicyRepository.FindPolicyById(order.PolicyId, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //Lấy ra danh sách mẫu hợp đồng rút tiền
            var contractTemplate = _contractTemplateServices.FindAllByOrder(-1, 1, null, order.Id, tradingProviderId, ContractTypes.RUT_TIEN);
            if (contractTemplate == null)
            {
                _logger.LogError($"Không tìm thấy danh sách hợp đồng mẫu rút tiền: orderId = {order.Id}, tradingProviderId = {tradingProviderId}");
                return;
            }
            foreach (var contract in contractTemplate.Items)
            {
                var saveFileApp = new SaveFileDto();
                var contractCode = _investContractCodeServices.GetContractCode(order, policy, contract.ConfigContractId);
                replaceTexts.AddRange(
                new List<ReplaceTextDto>() {
                    new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE, contractCode),
                    new ReplaceTextDto(PropertiesContractFile.TRAN_CONTENT, contractCode),
                });
                //Fill hợp đồng và lưu trữ
                saveFileApp = await _contractDataServices.SaveContractApp(order, contract.Id, order.PolicyDetailId, replaceTexts);

                //Lưu đường dẫn vào bảng order contract file
                var orderContractFile = new OrderContractFile
                {
                    ContractTempId = contract.Id,
                    FileTempUrl = saveFileApp?.FileTempUrl,
                    FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                    FileTempPdfUrl = saveFileApp?.FileSignatureUrl,
                    OrderId = (int)order.Id,
                    TradingProviderId = tradingProviderId,
                    PageSign = saveFileApp?.PageSign ?? 1,
                    WithdrawalId = withDrawalId,
                    Times = withdrawalTimes + 1,
                    ContractCodeGen = contractCode,
                    CreatedBy = userName,
                    FileSignatureStampUrl = saveFileApp?.FileSignatureStampUrl,
                };
                _orderContractFileRepository.Add(orderContractFile);
            }
        }

        /// <summary>
        /// Gửi thông báo rút vốn/tất toán trước hạn thành công
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <param name="withdrawalType"></param>
        /// <returns></returns>
        public async Task ResendNotifyInvestWithdrawalSuccess(int withdrawalId, int withdrawalType)
        {
            _logger.LogInformation($"{nameof(ResendNotifyInvestWithdrawalSuccess)}: withdrawalId = {withdrawalId}, withdrawalType={withdrawalType}");

            if (withdrawalType == WithdrawalTypes.RUT_VON)
            {
                await _investNotificationServices.SendEmailInvestWithdrawalSuccess(withdrawalId);
            }
            else if (withdrawalType == WithdrawalTypes.TAT_TOAN)
            {
                await _investNotificationServices.SendEmailInvestPrePayment(withdrawalId);
            }
        }

        /// <summary>
        /// Chuẩn bị rút vốn
        /// </summary>
        /// <returns></returns>
        public async Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestWithdrawal(PrepareApproveRequestWithdrawalDto input)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(WithdrawalServices)} -> {nameof(PrepareApproveRequestWithdrawal)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

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
            foreach (long withdrawalId in input.WithdrawalIds)
            {
                // Kiểm tra xem đã có yêu cầu chi tiền trước đó mà không thành công không
                var checkRequestPayment = _dbContext.MsbRequestPaymentDetail.Any(p => p.DataType == RequestPaymentDataTypes.EP_INV_WITHDRAWAL && p.ReferId == withdrawalId
                                            && p.Exception == null && p.Status == RequestPaymentStatus.KHOI_TAO);
                if (checkRequestPayment)
                {
                    _investWithdrawalEFRepository.ThrowException(ErrorCode.PaymentMsbNotificationExistRequestNotSuccess, withdrawalId);
                }

                var withdrawal = _investWithdrawalEFRepository.FindById(withdrawalId, tradingProviderId)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound, withdrawalId);

                if (withdrawal.Status != WithdrawalStatus.YEU_CAU && withdrawal.Status != WithdrawalStatus.CHO_PHAN_HOI)
                {
                    _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestWithdrawalNotInRequest);
                }
                var withdrawalData = ViewWithdrawalData(withdrawalId);
                var orderFind = _invOrderEFRepository
                    .FindById(withdrawal.OrderId ?? 0, tradingProviderId)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound, withdrawal.OrderId);

                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode)
                    .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound, orderFind.CifCode);

                distributionIds.Add(orderFind.DistributionId);

                var prepareResultDetail = new MsbRequestDetailWithErrorDto
                {
                    Id = (long)_msbRequestPaymentDetailEFRepository.NextKey(),
                    DataType = RequestPaymentDataTypes.EP_INV_WITHDRAWAL,
                    ReferId = withdrawalId,
                    AmountMoney = withdrawalData.ActuallyAmount, // tiền thực nhận của hợp đồng
                };

                // Invest Ngân hàng thụ hưởng của Investor và Business vẫn đang dùng chung trường InvestorBankAccId
                // còn BusinessCustomerBankAccId là ngân hàng thụ hưởng của đại lý
                if (cifCodeFind.InvestorId != null) //khách cá nhân
                {
                    var investorBankAcc = _investorBankAccountEFRepository
                        .FindById(orderFind.InvestorBankAccId ?? 0)
                        .ThrowIfNull(_dbContext, ErrorCode.InvestorBankAccNotFound, orderFind.InvestorBankAccId);

                    var bankFind = _bankEFRepository.FindById(investorBankAcc.BankId)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBankNotFound, investorBankAcc.BankId);

                    prepareResultDetail.BankAccount = investorBankAcc.BankAccount;
                    prepareResultDetail.OwnerAccount = investorBankAcc.OwnerAccount;
                    prepareResultDetail.Bin = bankFind.Bin;
                    prepareResultDetail.BankId = bankFind.BankId;
                    prepareResultDetail.BankCode = bankFind.BankCode;
                    prepareResultDetail.BankName = bankFind.BankName;
                }
                else //khách doanh nghiệp
                {
                    var businessBankAcc = _businessCustomerBankEFRepository
                       .FindById(orderFind.BusinessCustomerBankAccId ?? 0)
                       .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, orderFind.InvestorBankAccId);

                    var bankFind = _bankEFRepository.FindById(businessBankAcc.BankId)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBankNotFound, businessBankAcc.BankId);

                    prepareResultDetail.BankAccount = businessBankAcc.BankAccNo;
                    prepareResultDetail.OwnerAccount = businessBankAcc.BankAccName;
                    prepareResultDetail.Bin = bankFind.Bin;
                    prepareResultDetail.BankId = bankFind.BankId;
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
                _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestRequestIsNotTheSameDistribution);
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
                        RequestType = RequestPaymentTypes.RUT_VON,
                        ProductType = ProductTypes.INVEST,
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

        public async Task ApproveRequestWithdrawal(InvestApproveRequestWithdrawalDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(ApproveRequestWithdrawal)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            MsbRequestPayment requestPayment = new MsbRequestPayment();
            List<MsbRequestPaymentDetail> listRequestPaymentDetails = new();

            foreach (var withdrawalId in input.WithdrawalIds)
            {
                var withdrawal = _investWithdrawalEFRepository.FindById(withdrawalId)
                                         .ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound);
                if (withdrawal.Status != WithdrawalStatus.YEU_CAU && withdrawal.Status != WithdrawalStatus.CHO_PHAN_HOI)
                {
                    _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestWithdrawalNotInRequest);
                }
                if (input.Prepare != null && !input.Prepare.Details.Select(p => p.ReferId).Contains(withdrawalId))
                {
                    _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestWithdrawalIdNotInPrepareDetail);
                }
            }

            //trừ tiền lệnh
            if (input.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
            {
                var transaction = _dbContext.Database.BeginTransaction();
                await WithdrawalApprove(tradingProviderId, input.WithdrawalIds, input.Status, input.TradingBankAccId, username, input.ApproveNote);
                _dbContext.SaveChanges();
                transaction.Commit();
                // Gửi thông báo khi rút vốn thành công
                foreach (var withdrawalId in input.WithdrawalIds)
                {
                    var withdrawal = _investWithdrawalEFRepository.FindById(withdrawalId).ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound);
                    if (withdrawal.Type == WithdrawalTypes.RUT_VON)
                    {
                        await _investNotificationServices.SendEmailInvestWithdrawalSuccess(withdrawalId);
                    }
                    else if (withdrawal.Type == WithdrawalTypes.TAT_TOAN)
                    {
                        await _investNotificationServices.SendEmailInvestPrePayment(withdrawalId);
                    }
                }
            }
            else if (input.Status == WithdrawalStatus.HUY_YEU_CAU)
            {
                foreach (var withdrawalId in input.WithdrawalIds)
                {
                    var withdrawal = _investWithdrawalEFRepository.FindById(withdrawalId)
                                         .ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound);
                    withdrawal.Status = input.Status;
                    withdrawal.ModifiedBy = username;
                    withdrawal.ModifiedDate = DateTime.Now;
                }
            }
            _dbContext.SaveChanges();

            if (input.Status == WithdrawalStatus.DUYET_DI_TIEN)
            {
                var transaction = _dbContext.Database.BeginTransaction();
                // Chuyển sang trạng thái tạm khi duyệt đi tiền
                // Notifi thành công thì chuyển sang trạng thái duyệt đi tiền
                var waitResponse = WithdrawalStatus.CHO_PHAN_HOI;
                //Lập thông tin BondOrder Payment loại chi tiền và gửi thông báo
                await WithdrawalApprove(tradingProviderId, input.WithdrawalIds, waitResponse, input.TradingBankAccId, username, null, MsbBankStatus.INIT);

                var prefixAccount = _dbContext.TradingMSBPrefixAccounts.FirstOrDefault(e => e.TradingBankAccountId == input.TradingBankAccId && e.Deleted == YesNo.NO)
                   .ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotConfigured);

                //lấy thông tin đểu lưu vào lô chi hộ
                requestPayment.Id = input.Prepare.Id;
                requestPayment.TradingProdiverId = tradingProviderId;
                requestPayment.ProductType = ProductTypes.INVEST;
                requestPayment.RequestType = RequestPaymentTypes.RUT_VON;
                requestPayment.CreatedDate = DateTime.Now;
                requestPayment.CreatedBy = username;

                _msbRequestPaymentEFRepository.Add(requestPayment);
                foreach (var requestDetail in input.Prepare.Details)
                {
                    var requestPaymentDetail = new MsbRequestPaymentDetail()
                    {
                        Id = requestDetail.Id,
                        RequestId = requestPayment.Id,
                        DataType = RequestPaymentDataTypes.EP_INV_WITHDRAWAL,
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
        /// Xem giá trị khi rút vốn để chi tiền đi, nếu cần lấy dữ liệu cho hợp đồng (getContractData: true)
        /// </summary>
        public InvestViewWithdrawalDataDto ViewWithdrawalData(long withdrawalId, bool getContractData = false)
        {
            var withdrawalFind = _investWithdrawalEFRepository.FindById(withdrawalId)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound);

            InvestViewWithdrawalDataDto result = new();
            RutVonDto withdrawalData = new();
            List<ReplaceTextDto> contractData = new();
            if (withdrawalFind.Status == WithdrawalStatus.YEU_CAU || withdrawalFind.Status == WithdrawalStatus.CHO_PHAN_HOI)
            {
                var orderFind = _investOrderRepository.FindById(withdrawalFind.OrderId ?? 0);
                if (orderFind == null)
                {
                    _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestOrderNotFound);
                }
                else if (orderFind != null)
                {
                    if (orderFind.Status != OrderStatus.DANG_DAU_TU)
                    {
                        _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestOrderNotInStatusActive);
                    }
                    if (orderFind.InvestDate == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin ngày đầu tư của lệnh"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                    }

                    bool khachHangCaNhan = false;

                    // Là khách hàng cá nhân hay là doanh nghiệp
                    var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                    if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                    {
                        khachHangCaNhan = true;
                    }

                    // Trạng thái rút vốn : YÊU CẦU
                    // Tìm chính sách kỳ hạn để tính số tiền thực nhận 
                    var policyFind = _investPolicyRepository.FindPolicyById(orderFind.PolicyId);
                    if (policyFind == null)
                    {
                        _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestPolicyNotFound);
                    }

                    // Tìm kỳ hạn của hợp đồng sổ lệnh
                    var policyDetailFind = _investPolicyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
                    if (policyDetailFind == null)
                    {
                        _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestPolicyDetailNotFound);
                    }
                    var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyFind.DistributionId && r.Deleted == YesNo.NO)
                                        .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
                    withdrawalData = _investSharedServices.RutVonInvest(orderFind, policyFind, policyDetailFind, orderFind.TotalValue, withdrawalFind.AmountMoney ?? 0, withdrawalFind.WithdrawalDate, khachHangCaNhan, distribution.CloseCellDate);
                    result = _mapper.Map<InvestViewWithdrawalDataDto>(withdrawalData);
                    /// Nếu cần dữ liệu của để cho hợp đồng
                    if (getContractData)
                    {
                        //Get base data
                        contractData = _contractDataServices.GetDataContractFile(orderFind, orderFind.TradingProviderId, true);
                        //Get data cho hợp đồng rút
                        contractData.AddRange(_contractDataServices.GetDataWithdrawalContractFile(orderFind, policyFind, policyDetailFind, orderFind.TotalValue, withdrawalFind.AmountMoney ?? 0, withdrawalFind.WithdrawalDate, khachHangCaNhan, distribution.CloseCellDate));
                        result.DataContractFileWithdrawal = contractData;
                    }
                }
            }
            return result;
        }

        public async Task WithdrawalApprove(int tradingProviderId, List<long> withdrawalIds, int status, int? tradingBankAccId, string username, int? approveNote, int? statusBank = null)
        {
            _logger.LogInformation($"{nameof(WithdrawalApprove)}: input = {JsonSerializer.Serialize(withdrawalIds)}");
            var approveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            List<ReplaceTextDto> data = new();
            foreach (var withdrawalId in withdrawalIds)
            {
                var withdrawalFind = _investWithdrawalEFRepository.Entity.FirstOrDefault(w => w.Id == withdrawalId && w.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestWithdrawalNotFound);

                var orderFind = _invOrderEFRepository.FindById(withdrawalFind.OrderId ?? 0);
                if (orderFind == null)
                {
                    _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestOrderNotFound);
                }

                // Lấy thông tin rút vốn và kèm dữ liệu cho hợp đồng
                var withdrawalInfoData = ViewWithdrawalData(withdrawalId, true);

                // Dữ liệu cho hợp đồng
                data = withdrawalInfoData.DataContractFileWithdrawal;

                var withdrawalTimes = _investWithdrawalEFRepository.FindAll(withdrawalFind.OrderId ?? 0).Count;
                //Duyệt rút vốn : actuallyAmount: Số tiền thực nhận khi rút
                var approveWithdrawal = _investWithdrawalEFRepository.ApproveWithdrawal(new InvestWithdrawalApproveDto
                {
                    Id = withdrawalId,
                    TradingProviderId = tradingProviderId,
                    ApproveIp = approveIp,
                    Username = username,
                    ActuallyAmount = withdrawalInfoData.ActuallyAmount,
                    ActuallyProfit = withdrawalInfoData.ActuallyProfit ?? 0,
                    DeductibleProfit = withdrawalInfoData.ProfitReceived ?? 0,
                    Profit = withdrawalInfoData.WithdrawalProfit ?? 0,
                    Tax = withdrawalInfoData.IncomeTax ?? 0,
                    WithdrawalFee = withdrawalInfoData.WithdrawalFee ?? 0,
                    ApproveNote = approveNote
                });

                _dbContext.SaveChanges();
                if (status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                {
                    if (approveWithdrawal.Type == WithdrawalTypes.RUT_VON)
                    {
                        if (orderFind.TotalValue - approveWithdrawal.AmountMoney < 0)
                        {
                            _investWithdrawalEFRepository.ThrowException(ErrorCode.InvestWithdrawalApproveTooLarge);
                        }
                        orderFind.TotalValue = orderFind.TotalValue - approveWithdrawal.AmountMoney ?? 0;
                    }
                    else if (approveWithdrawal.Type == WithdrawalTypes.TAT_TOAN)
                    {
                        orderFind.TotalValue = 0;
                        orderFind.Status = OrderStatus.TAT_TOAN;
                        orderFind.SettlementDate = approveWithdrawal.WithdrawalDate;
                    }

                    //Thêm thanh toán chi cho BondOrder
                    _investOrderPaymentEFRepository.Add(new OrderPayment
                    {
                        TradingProviderId = tradingProviderId,
                        OrderId = orderFind.Id,
                        TranDate = DateTime.Now,
                        TranType = TranTypes.CHI,
                        TranClassify = TranClassifies.THANH_TOAN,
                        PaymentType = PaymentTypes.CHUYEN_KHOAN,
                        Status = OrderPaymentStatus.DA_THANH_TOAN,
                        PaymentAmnount = withdrawalInfoData.ActuallyAmount,
                        Description = PaymentNotes.CHI_RUT_VON + orderFind.ContractCode,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now,
                        ApproveDate = DateTime.Now,
                        ApproveBy = username,
                        TradingBankAccId = tradingBankAccId ?? 0,
                    });
                }
                //Lấy thêm data hợp đồng rút tiền
                await CreateContractFileWithDrawal(orderFind, withdrawalId, withdrawalTimes, data);
                approveWithdrawal.Status = status;
                _dbContext.SaveChanges();
                try
                {
                    // Ký điện tử các file hợp đồng khi hợp đồng tái tục thành công
                    _investOrderContractFileServices.UpdateContractFileSignPdf(orderFind.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}
