using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerShared;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Implements;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.ConstantVariables.Shared.Bank;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerOrderServices : IGarnerOrderServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly string _connectionString;
        private readonly ILogger<GarnerOrderServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly MsbCollectMoneyServices _msbCollectMoneyServices;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerProductTradingProviderEFRepository _garnerProductTradingProviderEFRepository;
        private readonly GarnerDistributionTradingBankAccountRepository _garnerDistributionTradingBankAccountRepository;
        private readonly GarnerApproveEFRepository _garnerApproveEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly SysVarEFRepository _sysVarRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvestorSaleEFRepository _investorSaleEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateEFRepository;
        private readonly GarnerOrderContractFileEFRepository _garnerOrderContractFileEFRepository;
        private readonly GarnerBackgroundJobServices _garnerBackgroundJobService;
        private readonly IGarnerContractTemplateServices _garnerContractTemplateServices;
        private readonly IGarnerContractDataServices _garnerContractDataServices;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly AuthOtpEFRepository _authOtpEFRepository;
        private readonly IGarnerFormulaServices _garnerFormulaServices;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly GarnerNotificationServices _garnerNotificationServices;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly IGarnerDistributionSharedServices _garnerDistributionSharedServices;
        private readonly IGarnerContractCodeServices _garnerContractCodeServices;
        private readonly IGarnerOrderContractFileServices _garnerOrderContractFileServices;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly IGarnerOrderPaymentServices _garnerOrderPaymentServices;

        public GarnerOrderServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerOrderServices> logger,
            IOptions<FileConfig> fileConfig,
            IHttpContextAccessor httpContextAccessor,
            IBackgroundJobClient backgroundJobs,
            IGarnerContractTemplateServices garnerContractTemplateServices,
            IGarnerContractDataServices garnerContractDataServices,
            GarnerBackgroundJobServices garnerBackgroundJobService,
            MsbCollectMoneyServices msbCollectMoneyServices,
            IGarnerFormulaServices garnerFormulaServices,
            GarnerNotificationServices garnerNotificationServices,
            IGarnerDistributionSharedServices garnerDistributionSharedServices,
            IGarnerContractCodeServices garnerContractCodeServices,
            IGarnerOrderContractFileServices garnerOrderContractFileServices,
            IGarnerOrderPaymentServices garnerOrderPaymentServices
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _msbCollectMoneyServices = msbCollectMoneyServices;
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _garnerProductTradingProviderEFRepository = new GarnerProductTradingProviderEFRepository(dbContext, logger);
            _garnerDistributionTradingBankAccountRepository = new GarnerDistributionTradingBankAccountRepository(dbContext, logger);
            _garnerApproveEFRepository = new GarnerApproveEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _sysVarRepository = new SysVarEFRepository(dbContext);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _investorSaleEFRepository = new InvestorSaleEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerContractTemplateEFRepository = new GarnerContractTemplateEFRepository(dbContext, logger);
            _garnerOrderContractFileEFRepository = new GarnerOrderContractFileEFRepository(dbContext, logger);
            _garnerContractTemplateServices = garnerContractTemplateServices;
            _garnerContractDataServices = garnerContractDataServices;
            _fileConfig = fileConfig;
            _backgroundJobs = backgroundJobs;
            _garnerBackgroundJobService = garnerBackgroundJobService;
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _authOtpEFRepository = new AuthOtpEFRepository(dbContext, logger);
            _garnerFormulaServices = garnerFormulaServices;
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
            _garnerNotificationServices = garnerNotificationServices;
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, logger);
            _investorRepository = new InvestorRepository(_connectionString, logger);
            _garnerDistributionSharedServices = garnerDistributionSharedServices;
            _garnerContractCodeServices = garnerContractCodeServices;
            _garnerOrderContractFileServices = garnerOrderContractFileServices;
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _garnerOrderPaymentServices = garnerOrderPaymentServices;
        }

        #region Order CMS

        private List<GarnerOrderMoreInfoDto> GetDataOrder(IEnumerable<GarnerOrder> orders)
        {
            var result = new List<GarnerOrderMoreInfoDto>();
            foreach (var item in orders)
            {
                var resultItem = new GarnerOrderMoreInfoDto();
                resultItem = _mapper.Map<GarnerOrderMoreInfoDto>(item);

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
                    if (cifCodeFind.Investor != null)
                    {
                        // Thông tin giấy tờ tùy thân
                        resultItem.Investor = _mapper.Map<Entities.Dto.Investor.InvestorDto>(cifCodeFind.Investor);
                        var findIdentification = _investorEFRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0);
                        if (findIdentification != null)
                        {
                            resultItem.Investor.InvestorIdentification = _mapper.Map<Entities.Dto.Investor.InvestorIdentificationDto>(findIdentification);
                        }
                    }
                }

                // Nếu là sản phẩm tích lũy Cổ Phần
                var product = _garnerProductEFRepository.FindById(item.ProductId);
                resultItem.Product = _mapper.Map<GarnerProductDto>(product);

                var policy = _garnerPolicyEFRepository.FindById(item.PolicyId);
                resultItem.Policy = _mapper.Map<GarnerPolicyMoreInfoDto>(policy);

                var policyDetail = _garnerPolicyDetailEFRepository.FindById(item.PolicyDetailId ?? 0, item.TradingProviderId);
                resultItem.PolicyDetail = _mapper.Map<GarnerPolicyDetailDto>(policyDetail);

                result.Add(resultItem);
            }
            return result;
        }

        /// <summary>
        /// Thêm hợp đồng sổ lệnh cho CMS
        /// </summary>
        /// <param name="input"></param>
        public async Task<GarnerOrderMoreInfoDto> Add(CreateGarnerOrderDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var transaction = _dbContext.Database.BeginTransaction();
            var result = await AddOrderCommon(input);
            transaction.Commit();
            return result;
        }

        /// <summary>
        /// Thêm lệnh (không transaction)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GarnerOrderMoreInfoDto> AddOrderCommon(CreateGarnerOrderDto input, bool isImport = false)
        {
            _logger.LogInformation($"{nameof(AddOrderCommon)}: input = {JsonSerializer.Serialize(input)}");

            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var ipCreated = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            var inputInsert = _mapper.Map<GarnerOrder>(input);
            //Lấy thông tin chính sách
            var getPolicy = _garnerPolicyEFRepository.FindById(input.PolicyId).ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);
            inputInsert.PolicyId = getPolicy.Id;
            //inputInsert.PolicyDetailId = input.PolicyDetailId;

            //Lấy thông tin Phân phối sản phẩm
            var getDistribution = _garnerDistributionEFRepository.FindById(getPolicy.DistributionId, tradingProviderId)
                .ThrowIfNull<GarnerDistribution>(_dbContext, ErrorCode.GarnerDistributionNotFound);

            if (!_garnerDistributionSharedServices.CheckAddTotalValue(getPolicy.DistributionId, input.TotalValue))
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderLimitMaxMoney);
            }
            inputInsert.IpAddressCreated = ipCreated;
            inputInsert.ProductId = getDistribution.ProductId;
            inputInsert.DistributionId = getPolicy.DistributionId;

            //Kiểm tra xem là nhà đầu tư cá nhân hay là nhà đầu tư doanh nghiệp
            var findCifCode = _cifCodeEFRepository.FindByCifCode(input.CifCode);
            if (findCifCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            else if (findCifCode.InvestorId != null)
            {
                if (!string.IsNullOrEmpty(input.SaleReferralCode))
                {
                    var saleReferralCode = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
                    inputInsert.SaleReferralCode = saleReferralCode.ReferralCode;
                    inputInsert.SaleReferralCodeSub = saleReferralCode.ReferralCodeSub;
                    inputInsert.DepartmentIdSub = saleReferralCode.DepartmentIdSub;
                    inputInsert.DepartmentId = saleReferralCode.DepartmentId;
                }
            }

            if (findCifCode != null && findCifCode.InvestorId != null)
            {
                inputInsert.InvestorBankAccId = input.InvestorBankAccId;

                inputInsert.InvestorIdenId = _investorEFRepository.GetDefaultIdentification(findCifCode.InvestorId ?? 0)?.Id;
            }
            else if (findCifCode != null && findCifCode.BusinessCustomerId != null)
            {
                inputInsert.InvestorBankAccId = input.BusinessCustomerBankAccId;
            }

            //Lấy thông tin ngân hàng đại lý
            var tradingProviderBank = _garnerDistributionTradingBankAccountRepository.FindFirstBankCollectByDistribution(getDistribution.Id)
                .ThrowIfNull<GarnerDistributionTradingBankAccount>(_dbContext, ErrorCode.GarnerDistributionTradingBankAccNotFound);
            inputInsert.TradingBankAccId = tradingProviderBank.BusinessCustomerBankAccId;

            //Nếu là import từ file thì status = CHờ duyệt hợp đồng
            if (isImport)
            {
                inputInsert.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                inputInsert.Source = SourceOrder.OFFLINE;
            }

            var result = _garnerOrderEFRepository.Add(inputInsert, tradingProviderId, username);
            _dbContext.SaveChanges();

            // Sinh file hợp đồng mẫu
            var data = _garnerContractDataServices.GetDataContractFile(result, tradingProviderId, true);
            await _garnerOrderContractFileServices.CreateContractFileByOrderAdd(result, data);
            return _mapper.Map<GarnerOrderMoreInfoDto>(result);
        }

        public GarnerOrderMoreInfoDto FindById(long orderId)
        {
            _logger.LogInformation($"{nameof(FindById)}: orderId= {orderId}");

            var orderFind = _garnerOrderEFRepository.FindById(orderId);
            var result = _mapper.Map<GarnerOrderMoreInfoDto>(orderFind);

            if (orderFind == null)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderNotFound);
            }
            //Lấy thông tin BusinessCusstomer / Investor
            var findInfobyCifCode = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode);

            if (findInfobyCifCode == null)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            else if (findInfobyCifCode.InvestorId != null)
            {
                var investor = _investorEFRepository.FindActiveInvestorById(findInfobyCifCode.InvestorId ?? 0);
                if (investor == null)
                {
                    _garnerOrderEFRepository.ThrowException(ErrorCode.InvestorNotFound);
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
                    _garnerOrderEFRepository.ThrowException(ErrorCode.CoreBussinessCustomerNotFound);
                }
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                result.BusinessCustomer.BusinessCustomerBanks = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(findInfobyCifCode.BusinessCustomerId ?? 0).ToList();
            }
            // Kiểm tra có phải là lần thanh toán đầu tiên
            result.IsFirstPayment = YesNo.NO;
            long firstPaymentBankId = 0;
            var firstPaymentFind = _garnerOrderPaymentEFRepository.GetFirstPayment(orderFind.Id);
            if (firstPaymentFind == null)
            {
                result.IsFirstPayment = YesNo.YES;
            }
            else
            {
                firstPaymentBankId = firstPaymentFind.TradingBankAccId ?? 0;
            }
            // Thông tin tài khoản ngân hàng thanh toán đầu tiên
            var firstPaymentBank = _businessCustomerRepository.FindBusinessCusBankById(firstPaymentBankId);
            if (firstPaymentBank != null)
            {
                result.FirstPaymentBankDto = _mapper.Map<FirstPaymentBankDto>(firstPaymentBank);
                result.FirstPaymentBankDto.BusinessCustomerBankId = (int)firstPaymentBankId;
            }
            // Thông tin dự án
            var product = _garnerProductEFRepository.FindById(orderFind.ProductId);
            if (product == null)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerProductNotFound);
            }
            result.Product = _mapper.Map<GarnerProductDto>(product);

            // Thông tin chính sách
            var policy = _garnerPolicyEFRepository.FindById(orderFind.PolicyId);
            if (policy == null)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerPolicyNotFound);
            }
            result.Policy = _mapper.Map<GarnerPolicyMoreInfoDto>(policy);

            if (orderFind.PolicyDetailId != null)
            {
                //Thông tin kỳ hạn
                var policyDetail = _garnerPolicyDetailEFRepository.FindById(orderFind.PolicyDetailId ?? 0);
                if (policyDetail == null)
                {
                    _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerPolicyDetailNotFound);
                }
                result.PolicyDetail = _mapper.Map<GarnerPolicyDetailDto>(policyDetail);
            }

            if (orderFind.SaleReferralCode != null)
            {
                var findSaleByReferralCode = _saleEFRepository.FindSaleByReferralCode(orderFind.SaleReferralCode);
                if (findSaleByReferralCode != null)
                {
                    var findSaleInTrading = _saleEFRepository.FindSaleTradingProvider(findSaleByReferralCode.SaleId, orderFind.TradingProviderId);
                    if (findSaleInTrading != null)
                    {
                        result.Sale = findSaleInTrading;
                        result.Sale.ManagerDepartmentName = findSaleInTrading.DepartmentName;
                        var findSaleName = _saleEFRepository.FindSaleName(orderFind.SaleReferralCode);
                        if (findSaleName != null)
                        {
                            result.Sale.SaleName = findSaleName.Name;
                        }
                    }
                }
            }

            AppSaleByReferralCodeDto saleFind = null;
            try
            {
                saleFind = _saleEFRepository.FindSaleByReferralCode(orderFind.SaleReferralCode, orderFind.TradingProviderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            if (saleFind != null)
            {
                result.Sale = _mapper.Map<ViewSaleDto>(saleFind);
                if (saleFind.InvestorId != null)
                {
                    var investorFind = _investorEFRepository.FindById(saleFind.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<Entities.Dto.Investor.InvestorDto>(investorFind);
                        result.Sale.Investor = investor;
                        var investorIdenDefaultFind = _investorEFRepository.GetDefaultIdentification(saleFind.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            result.Sale.Investor.InvestorIdentification = _mapper.Map<Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                        }

                        var investorFindBank = _investorEFRepository.FindListBank(saleFind.InvestorId ?? 0);
                        result.Sale.Investor.ListBank = _mapper.Map<List<InvestorBankAccount>>(investorFindBank);
                    }
                }
                else if (saleFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerEFRepository.FindById(saleFind.BusinessCustomerId ?? 0);
                    result.Sale.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    var listBank = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(businessCustomer.BusinessCustomerId ?? 0);
                    result.Sale.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank);
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
                    result.Sale.ManagerDepartmentName = departmentOfSaleFind.DepartmentName;
                }
                else
                {
                    _logger.LogError($"{nameof(FindById)}: Không tìm thấy phòng giao dịch quản lý hợp đồng: saleId = {saleFind.SaleId}, tradingProviderId = {orderFind.TradingProviderId}");
                }
            }
            return result;
        }

        public GarnerOrderCashFlowDto ProfitFuture(long orderId)
        {
            _logger.LogInformation($"{nameof(ProfitFuture)}: orderId = {orderId}");
            var orderQuery = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);

            GarnerOrderCashFlowDto result = new();
            _logger.LogInformation($"{nameof(ProfitFuture)}: orderId = {orderId}");
            result.Expecteds = _garnerFormulaServices.GetCashFlowOrder(orderId);

            List<GarnerOrderCashFlowActualDto> actuals = new();
            var listWithdrawal = _garnerWithdrawalEFRepository.WithdrawalByOrder(orderId);
            var initTotalValue = orderQuery.InitTotalValue;
            int withdrawalIndex = 0;
            foreach (var item in listWithdrawal)
            {
                if (item.Status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || item.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
                {
                    // Chung 1 trạng thái là đã đi tiền(đã duyệt)
                    item.Status = InterestPaymentStatus.DA_DUYET_CHI_TIEN;
                    initTotalValue = initTotalValue - item.AmountMoney;
                }
                // Chờ phản hồi = Trạng thái yêu cầu (Khởi tạo)
                else if (item.Status == InterestPaymentStatus.CHO_PHAN_HOI)
                {
                    item.Status = InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA;
                }
                withdrawalIndex = withdrawalIndex + 1;
                actuals.Add(new GarnerOrderCashFlowActualDto
                {
                    Id = item.Id,
                    PeriodIndexName = $"Rút vốn lần {withdrawalIndex}",
                    AmountMoney = item.AmountMoney,
                    TotalValue = item.TotalValue,
                    InitTotalValue = item.InitTotalValue,
                    NumberOfDays = item.NumberOfDays,
                    Profit = item.Profit,
                    Surplus = initTotalValue,
                    Tax = item.Tax,
                    AmountReceived = item.AmountReceived,
                    DeductibleProfit = item.DeductibleProfit,
                    PayDate = item.PayDate,
                    Status = item.Status
                });
            }
            result.Actuals = actuals;
            return result;
        }

        /// <summary>
        /// Lấy ra danh sách chính sách theo cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        public List<GarnerPolicy> GetListPolicyByCifCode(string cifCode)
        {
            _logger.LogInformation($"{nameof(GetListPolicyByCifCode)}: cifCode = {cifCode}");

            var result = new List<GarnerPolicy>();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyIds = _garnerOrderEFRepository.GetListPolicyIdByCifCode(cifCode, tradingProviderId);
            foreach (var policyId in policyIds)
            {
                var policy = _garnerPolicyEFRepository.FindById(policyId, tradingProviderId);
                result.Add(policy);
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách ngân hàng đặt lệnh theo cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        public List<BankAccountInfoDto> FindListBankOfCifCode(string cifCode)
        {
            _logger.LogInformation($"{nameof(GetListPolicyByCifCode)}: cifCode = {cifCode}");
            var result = _garnerFormulaServices.FindListBankOfCifCode(cifCode);
            return result;
        }

        /// <summary>
        /// Tìm kiếm order phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerOrderMoreInfoDto> FindAll(FilterGarnerOrderDto input, int[] status, bool isGroupByCustomer = false, bool? isDelivaryStatus = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, status = {status}");

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

            var resultPaging = new PagingResult<GarnerOrderMoreInfoDto>();

            var find = _garnerOrderEFRepository.FindAll(input, status, tradingProviderId, isGroupByCustomer, isDelivaryStatus);

            resultPaging.Items = find.Items;
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// tìm kiếm lịch sử của các bảng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerHistoryUpdateDto> FindAllHistoryTable(FilterGarnerHistoryDto input)
        {
            _logger.LogInformation($"{nameof(FindAllHistoryTable)}: input = {JsonSerializer.Serialize(input)}");
            var resultPaging = new PagingResult<GarnerHistoryUpdateDto>();
            var history = _garnerHistoryUpdateEFRepository.FindAllByTable(input);
            var garnerOrder = _mapper.Map<List<GarnerHistoryUpdateDto>>(history.Items);

            resultPaging.Items = garnerOrder;
            resultPaging.TotalItems = history.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<GarnerOrder> OrderApprove(long orderId)
        {
            _logger.LogInformation($"{nameof(OrderApprove)}: orderId = {orderId}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(orderId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound, orderId);
            if (_garnerDistributionSharedServices.LimitCalculationTrading(orderFind.ProductId, orderFind.TradingProviderId).TotalInvestmentSub < 0)
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerApproveOrderLimitMaxMoney);
            }
            decimal soTienDaDauTu = _garnerOrderEFRepository.SumValue(orderFind?.DistributionId ?? 0, null);
            var product = _garnerProductEFRepository.FindById(orderFind?.ProductId ?? 0);
            var orderApprove = new GarnerOrder();
            var check = product?.InvTotalInvestment;
            var hanMucDauTu = (product?.InvTotalInvestment == null || product?.InvTotalInvestment == 0) ? ((product?.CpsQuantity ?? 0) * (product?.CpsParValue ?? 0)) : product?.InvTotalInvestment;
            //check số tiền đặt lệnh phù hợp với hạn mức còn lại
            if (soTienDaDauTu > hanMucDauTu)
            {
                throw new FaultException(new FaultReason($"Số tiền đặt lệnh lớn hơn hạn mức còn lại"), new FaultCode(((int)ErrorCode.GarnerOrderExcceedRemailLimitDistribution).ToString()), "");
            }
            else
            {
                orderApprove = _garnerOrderEFRepository.OrderApprove(orderId, tradingProviderId, username);
                // Tìm cifCode nếu là nhà đầu tư cá nhân, thêm quan hệ với sale và đại lý
                var cifCode = _cifCodeEFRepository.FindByCifCode(orderApprove.CifCode);
                if (cifCode != null && cifCode.InvestorId != null)
                {
                    // Thêm quan hệ với đại lý
                    _investorEFRepository.InsertInvestorTradingProvider(cifCode.InvestorId ?? 0, tradingProviderId, username);

                    // Thêm quan hệ InvestorSale
                    var referralCode = (orderApprove.SaleReferralCodeSub != null) ? orderApprove.SaleReferralCodeSub : orderApprove.SaleReferralCode;
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
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    RealTableId = orderId,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_APPROVE_FILE,
                }, username);
                _dbContext.SaveChanges();
                await _garnerNotificationServices.SendNotifyGarnerOrderActive(orderId);
            }
            return orderApprove;
        }

        /// <summary>
        /// Thay đổi trạng thái giao nhận hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatus(long orderId)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatus)}: orderId = {orderId}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderApprove = _garnerOrderEFRepository.ChangeDeliveryStatus(orderId, username, tradingProviderId);
            _dbContext.SaveChanges();
            return orderApprove;
        }

        /// <summary>
        /// cập nhật hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerOrder Update(UpdateGarnerOrderDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            //Lấy thông tin order
            var orderFind = _garnerOrderEFRepository.FindById(input.Id).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            var garnerOrder = _mapper.Map<GarnerOrder>(input);
            if (input.SaleReferralCode != null)
            {

                var findSaleReferralCode = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
                garnerOrder.SaleReferralCode = findSaleReferralCode.ReferralCode;
                garnerOrder.DepartmentId = findSaleReferralCode.DepartmentId;
                garnerOrder.SaleReferralCodeSub = findSaleReferralCode.ReferralCodeSub;
                garnerOrder.DepartmentIdSub = findSaleReferralCode.DepartmentIdSub;
            }
            var orderApprove = _garnerOrderEFRepository.Update(garnerOrder, tradingProviderId, username);
            _dbContext.SaveChanges();
            return orderApprove;
        }

        /// <summary>
        /// xóa hợp đồng ở trạng thái khởi tạo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void Deleted(List<long> orderIds)
        {
            _logger.LogInformation($"{nameof(Deleted)}: orderId = {orderIds}");

            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var orderId in orderIds)
            {
                var orderDeleted = _garnerOrderEFRepository.Deleted(orderId, tradingProviderId, username);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Xử lý yêu cầu nhận hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrder ProcessContract(long orderId)
        {
            _logger.LogInformation($"{nameof(ProcessContract)}: orderId = {orderId}");

            //Lấy thông tin
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderProcessContract = _garnerOrderEFRepository.ProcessContract(orderId, username, tradingProviderId);

            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                RealTableId = orderId,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_PROCESS_CONTRACT,
            }, username);

            _dbContext.SaveChanges();
            return orderProcessContract;
        }

        /// <summary>
        /// lọc theo trạng thái 5 của giao nhận hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerOrderMoreInfoDto> FindAllDelivery(FilterGarnerOrderDto input)
        {
            _logger.LogInformation($"{nameof(FindAllDelivery)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            if (usertype != UserTypes.EPIC && usertype != UserTypes.ROOT_EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            var resultPaging = new PagingResult<GarnerOrderMoreInfoDto>();
            var find = _garnerOrderEFRepository.FindAll(input, new int[] { OrderStatus.DANG_DAU_TU }, tradingProviderId);

            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// Hủy duyệt hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerOrder OrderCancel(int id)
        {
            _logger.LogInformation($"{nameof(OrderCancel)}: id = {id}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(id).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            if (orderFind.Status != OrderStatus.DANG_DAU_TU)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.BondOrderCanNotCancel);
            }
            orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;

            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                RealTableId = id,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_CANCEL_FILE,
            }, username);

            _dbContext.SaveChanges();
            return orderFind;
        }

        /// <summary>
        /// Cập nhật nguồn đặt lệnh từ offline, sale đặt lệnh, sang online
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GarnerOrder> UpdateSource(long id)
        {
            _logger.LogInformation($"{nameof(UpdateSource)}: id = {id}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(id).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);

            _garnerHistoryUpdateEFRepository.HistoryOrderSource(id, username);

            orderFind.Source = SourceOrderFE.QUAN_TRI_VIEN;
            _dbContext.SaveChanges();

            //sinh lại hợp đồng online
            await _garnerOrderContractFileServices.UpdateContractFileUpdateSource(id);
            return orderFind;
        }

        /// <summary>
        /// Tìm kiếm order có trạng thái là 4 (có bộ lọc)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerOrderMoreInfoDto> FindAllByPolicy(FilterGarnerOrderDto input, int[] status)
        {
            _logger.LogInformation($"{nameof(FindAllByPolicy)}: input = {JsonSerializer.Serialize(input)}");

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

            var result = FindAll(input, status, true);
            var listGroup = result.Items
                .GroupBy(p => new { p.PolicyId, p.CifCode })
                .Select(o => o.First())
                .ToList();
            listGroup = _mapper.Map<List<GarnerOrderMoreInfoDto>>(listGroup);
            foreach (var item in listGroup)
            {
                var listOrder = result.Items.Where(o => o.PolicyId == item.PolicyId && o.CifCode == item.CifCode).ToList();
                var totalInitValue = listOrder.Sum(o => o.InitTotalValue);
                var totalValue = listOrder.Sum(o => o.TotalValue);
                item.InitTotalValue = totalInitValue;
                item.TotalValue = totalValue;
                foreach (var orderItem in listOrder)
                {
                    var profit = _garnerFormulaServices.CaculateProfitNow(orderItem.Id);
                    orderItem.ProfitNow = profit.Profit;
                    orderItem.InvestmentDayNow = profit.InvestmentDays;
                }
                item.ListOrder = listOrder.OrderBy(e => e.Id).ToList();
                item.ProfitNow = listOrder.Sum(o => o.ProfitNow);
                item.InvestmentDayNow = 0;
                if (listOrder.Any())
                {
                    item.InvestmentDayNow = listOrder.Max(o => o.InvestmentDayNow); //tích luỹ đến hiện tại thì lấy max trong danh sách
                }
            }
            result.Items = listGroup;
            result.TotalItems = result.TotalItems;
            return result;
        }

        public ExportResultDto ImportFileTemplate()
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string fileName = "Temp_Import_Order.xlsx";
            string path = Path.Combine(Environment.CurrentDirectory, "Data", "ExcelTemplate", fileName);
            using var workbook = new XLWorkbook(path);
            var ws = workbook.Worksheet(1);
            const string sheetDataValidation = "DV";
            var dataValidation = workbook.Worksheets.Add(sheetDataValidation);
            const int maxRow = 1000;

            //var orderList = _garnerOrderEFRepository.EntityNoTracking.Where(o => o.TradingProviderId == tradingProviderId 
            //                    && o.Deleted == YesNo.NO && o.Status == OrderStatus.DANG_DAU_TU).ToList();

            var productList = (from product in _dbContext.GarnerProducts
                               where product.Deleted == YesNo.NO
                               && _dbContext.GarnerOrders.Any(o => o.TradingProviderId == tradingProviderId
                               && o.ProductId == product.Id && o.Status == OrderStatus.DANG_DAU_TU && o.Deleted == YesNo.NO)
                               select product).OrderByDescending(o => o.Id).ToList();

            var products = new List<string>();
            var initialIndex = 1;
            StringBuilder templateValidation = new("=\"\"");
            var policyIndex = 0;

            for (int productIndex = 0; productIndex < productList.Count(); productIndex++)
            {
                var id = productList[productIndex].Id;
                var productType = productList[productIndex].ProductType;
                var productName = productList[productIndex].Name;
                var productString = string.Join('-', id, productType, productName);
                var cellProduct = dataValidation.Cell(productIndex + 1, 1);
                var productValue = cellProduct.SetValue(productString);
                products.Add(productString);
                //var policyList = (from product in _dbContext.GarnerProducts 
                //                 join distribution in _dbContext.GarnerDistributions on product.Id equals distribution.ProductId
                //                 join policy in _dbContext.GarnerPolicies on distribution.Id equals policy.DistributionId
                //                 where distribution.Deleted == YesNo.NO && policy.Deleted == YesNo.NO && _dbContext.GarnerDistributions.Any(o => o.ProductId == product.Id)
                //                 select policy).ToList();
                var policyList = (from policy in _dbContext.GarnerPolicies
                                  where _dbContext.GarnerDistributions.Any(o => o.TradingProviderId == tradingProviderId && o.ProductId == id && o.Deleted == YesNo.NO && o.Id == policy.DistributionId) && policy.Deleted == YesNo.NO
                                  select policy).ToList();
                if (policyList.Count == 0)
                {
                    continue;
                }
                foreach (var policy in policyList)
                {
                    var policyId = policy.Id;
                    var policyCode = policy.Code;
                    var policyName = policy.Name;
                    var policyString = string.Join('-', policyId, policyCode, policyName);
                    var policyCell = dataValidation.Cell(++policyIndex, 2);
                    var policyValue = policyCell.SetValue(policyString);
                }
                templateValidation.Replace("\"\"", $"IF(@={sheetDataValidation}!{cellProduct},{sheetDataValidation}!B{initialIndex}:B{policyIndex},\"\")");
                initialIndex = policyIndex + 1;
            }

            var templateString = templateValidation.ToString();
            for (int index = 2; index <= maxRow; index++)
            {
                ws.Cell(index, 2).Value = "-";
                var stringFormula = templateString.Replace("@", $"{ws.Cell(index, 2)}");
                ws.Cell($"C{index}")
                    .CreateDataValidation()
                    .List(stringFormula);
            }

            var productRange = dataValidation.Cell(1, 1).InsertData(products);

            ws.Range($"B2:B{maxRow}")
                .CreateDataValidation()
                .List(dataValidation.Range($"A1:A{maxRow}"));

            MemoryStream stream = new();
            workbook.SaveAs(stream);
            return new ExportResultDto
            {
                fileData = stream.ToArray(),
                fileDownloadName = fileName
            };
        }

        public async Task ImportExcelOrder(ImportExcelOrderDto dto)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ImportExcelOrder)} : dto = {JsonSerializer.Serialize(dto)}, username={username}, partnerId = {tradingProviderId}");

            //byte[] fileBytes;
            //using (var ms = new MemoryStream())
            //{
            //    dto.File.CopyTo(ms);
            //    fileBytes = ms.ToArray();
            //}
            //var fileName = $"{Path.GetTempFileName()}{Path.GetExtension(dto.File.FileName)}";
            //File.WriteAllBytes(fileName, fileBytes);
            using var wb = new XLWorkbook(dto.File.OpenReadStream());

            int colPhone = 1;
            int colProduct = 2;
            int colPolicy = 3;
            int colTotalValue = 4;
            int colAccount = 5;
            int colReferralCode = 6;

            var rows = wb.Worksheet(1).RowsUsed().Skip(1); // Skip header row
            var transaction = _dbContext.Database.BeginTransaction();

            //Thêm vào orderPayment
            foreach (var row in rows)
            {
                var productName = row.Cell(colProduct)?.Value.ToString().Trim();
                if (productName == "-" || string.IsNullOrEmpty(productName))
                {
                    break;
                }

                var productFind = _garnerProductEFRepository.EntityNoTracking
                    .FirstOrDefault(o => o.Deleted == YesNo.NO && (o.Id + "-" + o.ProductType + "-" + o.Name) == productName)
                    .ThrowIfNull($"Không tìm thấy sản phẩm ở dòng {row.RowNumber()} cột {colProduct}");
                var policyFind = row.Cell(colPolicy)?.Value.ToString().Trim();
                int? policyId = null;
                if (!string.IsNullOrEmpty(policyFind))
                {
                    var policyQuery = _garnerPolicyEFRepository.EntityNoTracking
                        .FirstOrDefault(o => o.Deleted == YesNo.NO && (o.Id + "-" + o.Code + "-" + o.Name) == policyFind)
                        .ThrowIfNull($"Không tìm thấy chính sách ở dòng {row.RowNumber()} cột {colPolicy}");
                    policyId = policyQuery.Id;
                }
                var phone = StringCheckRequired(row.Cell(colPhone)?.Value.ToString().Trim(), $"Cần nhập kí tự ở dòng {row.RowNumber()} cột {colPhone}");
                var totalValue = DecimalTryParse(row.Cell(colTotalValue)?.Value.ToString(), $"Dữ liệu không hợp lệ ở dòng {row.RowNumber()} cột {colTotalValue}");
                var account = StringCheckRequired(row.Cell(colAccount)?.Value.ToString().Trim(), $"Cần nhập kí tự ở dòng {row.RowNumber()} cột {colAccount}");
                var referralCode = row.Cell(colReferralCode)?.Value.ToString().Trim();

                var phoneFind = _investorEFRepository.EntityNoTracking.FirstOrDefault(o => o.Phone == phone)
                    .ThrowIfNull($"Số điện thoại \"{phone}\" không tồn tại trong hệ thống");
                var cifCodeFind = _cifCodeEFRepository.EntityNoTracking.FirstOrDefault(o => o.InvestorId == phoneFind.InvestorId);
                var cifCode = cifCodeFind.CifCode;
                var bankAccount = _investorBankAccountEFRepository.EntityNoTracking.FirstOrDefault(o => o.BankAccount == account)
                    .ThrowIfNull($"Số tài khoản \"{account}\" không tồn tại trong hệ thống");
                var investorBankAccount = bankAccount.Id;

                var order = await AddOrderCommon(new CreateGarnerOrderDto
                {
                    CifCode = cifCode,
                    PolicyId = policyId.Value,
                    SaleReferralCode = referralCode,
                    TotalValue = totalValue,
                    InvestorBankAccId = investorBankAccount,
                }, true);

                _dbContext.SaveChanges();
                var orderPayment = _garnerOrderPaymentEFRepository.Add(new GarnerOrderPayment
                {
                    OrderId = order.Id,
                    TranDate = DateTime.Now,
                    TranType = TranTypes.THU,
                    TradingBankAccId = order.TradingBankAccId,
                    PaymentType = PaymentTypes.TANG_TICH_LUY,
                    PaymentAmount = order.TotalValue,
                    TranClassify = TranClassifies.THANH_TOAN,
                    Description = "TT" + order.ContractCode,
                    Status = OrderPaymentStatus.DA_THANH_TOAN,
                    ApproveBy = username,
                    ApproveDate = DateTime.Now
                }, username, tradingProviderId);

                _dbContext.SaveChanges();

                _garnerOrderContractFileServices.UpdateContractFileSignPdf(order.Id);
                var orderApprove = await OrderApprove(order.Id);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Kiểm tra trường text bắt buộc nhập
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private string StringCheckRequired(string data, string message)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                _defErrorEFRepository.ThrowException(message);
            }
            return data;
        }

        /// <summary>
        /// Ép kiểu decimal dữ liệu từ excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private decimal DecimalTryParse(string data, string message)
        {
            if (!decimal.TryParse(data, out decimal result))
            {
                _defErrorEFRepository.ThrowException(message);
            }
            return result;
        }
        #endregion Order CMS

        #region Cập nhật các thông tin của hợp đồng

        /// <summary>
        /// Update Kỳ hạn của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        public GarnerOrder UpdatePolicyDetail(long orderId, int? policyDetailId)
        {
            _logger.LogInformation($"{nameof(UpdatePolicyDetail)}: orderId = {orderId}, policyDetailId = {policyDetailId}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderFind = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            _garnerHistoryUpdateEFRepository.HistoryPolicyDetail(orderId, policyDetailId, username);
            orderFind.PolicyDetailId = policyDetailId;
            orderFind.ModifiedBy = username;
            orderFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return orderFind;
        }

        /// <summary>
        /// Update chính sách của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public GarnerOrder UpdatePolicy(long orderId, int policyId)
        {
            _logger.LogInformation($"{nameof(UpdatePolicy)}: orderId = {orderId}, policyDetailId = {policyId}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var policyFind = _garnerPolicyEFRepository.FindById(policyId).ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var orderFind = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            var oldValue = _garnerPolicyEFRepository.FindById(orderFind.PolicyId).ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);
            if (orderFind.PolicyId != policyId)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(orderFind.Id, oldValue.Name, policyFind.Name, GarnerFieldName.UPDATE_POLICY_ID,
                    GarnerHistoryUpdateTables.GAN_ORDER, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }
            orderFind.PolicyId = policyId;
            orderFind.ModifiedBy = username;
            orderFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return orderFind;
        }

        /// <summary>
        /// Update số tiền đầu tư
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        public GarnerOrder UpdateTotalValue(long orderId, decimal? totalValue)
        {
            _logger.LogInformation($"{nameof(UpdateTotalValue)}: orderId = {orderId}, totalValue = {totalValue}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderFind = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            var sumOrderPayment = _garnerOrderPaymentEFRepository.SumPaymentAmount(orderId);

            // Nếu số tiền lớn hơn số tiền đã thanh toán
            if (totalValue > sumOrderPayment)
            {
                orderFind.Status = OrderStatus.CHO_THANH_TOAN;
                orderFind.InvestDate = null;
                orderFind.ActiveDate = null;
                orderFind.PaymentFullDate = null;
            }

            // Nếu số tiền nhỏ nhơ hoặc bằng số tiền đã thanh toán
            else if (totalValue <= sumOrderPayment)
            {
                orderFind.Status = OrderStatus.CHO_DUYET_HOP_DONG;
                orderFind.InvestDate = null;
                orderFind.ActiveDate = null;
            }

            _garnerHistoryUpdateEFRepository.HistoryTotalValue(orderId, totalValue, username);
            orderFind.TotalValue = totalValue.Value;
            orderFind.InitTotalValue = totalValue ?? 0;
            orderFind.ModifiedBy = username;
            orderFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();

            return orderFind;
        }

        /// <summary>
        /// Update Mã giới thiệu
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public GarnerOrder UpdateReferralCode(long orderId, string referralCode)
        {
            _logger.LogInformation($"{nameof(UpdateReferralCode)}: orderId = {orderId}, referralCode = {referralCode}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderQuery = _garnerOrderEFRepository.FindById(orderId)
                .ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);

            var findSaleReferralCode = _saleEFRepository.AppFindSaleOrderByReferralCode(referralCode, tradingProviderId);

            if ((findSaleReferralCode.ReferralCodeSub == null && orderQuery.SaleReferralCodeSub != null) || findSaleReferralCode.ReferralCodeSub != orderQuery.SaleReferralCodeSub)
            {
                var saleSubInfo = _saleEFRepository.FindSaleName(orderQuery.SaleReferralCodeSub);
                var saleInfoNew = _saleEFRepository.FindSaleName(findSaleReferralCode.ReferralCode);
                var saleSubInfoNew = _saleEFRepository.FindSaleName(findSaleReferralCode.ReferralCodeSub);
                string oldValue = orderQuery.SaleReferralCodeSub != null ? orderQuery.SaleReferralCodeSub + ": " + saleSubInfo.Name + " (" + saleSubInfo.SaleId + ") " + "DepartmentIdSub: " + orderQuery.DepartmentIdSub
                                    : null;

                string newValue = findSaleReferralCode.ReferralCodeSub != null ? findSaleReferralCode.ReferralCodeSub + ": " + saleSubInfoNew.Name + " (" + saleSubInfoNew.SaleId + ") " + "DepartmentIdSub: " + findSaleReferralCode.DepartmentIdSub
                                    : null;

                orderQuery.SaleReferralCodeSub = findSaleReferralCode.ReferralCodeSub;
                orderQuery.DepartmentIdSub = findSaleReferralCode.DepartmentIdSub;
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(orderId, oldValue, newValue, GarnerFieldName.UPDATE_SALE_REFERRAL_CODE_SUB, GarnerHistoryUpdateTables.GAN_ORDER, ActionTypes.CAP_NHAT, "Cập nhật mã giới thiệu bán hộ"), username);
            }
            if (findSaleReferralCode.ReferralCode == null || findSaleReferralCode.ReferralCode != orderQuery.SaleReferralCode)
            {
                var saleInfo = _saleEFRepository.FindSaleName(orderQuery.SaleReferralCode);
                var saleInfoNew = _saleEFRepository.FindSaleName(findSaleReferralCode.ReferralCode);
                string oldValue = orderQuery.SaleReferralCode != null ? orderQuery.SaleReferralCode + ": " + saleInfo.Name + " (" + saleInfo.SaleId + ") " + "DepartmentId: " + orderQuery.DepartmentId
                                    : null;

                string newValue = findSaleReferralCode.ReferralCode != null ? findSaleReferralCode.ReferralCode + ": " + saleInfoNew.Name + " (" + saleInfoNew.SaleId + ") " + "DepartmentId: " + findSaleReferralCode.DepartmentId
                                    : null;

                orderQuery.SaleReferralCode = findSaleReferralCode.ReferralCode;
                orderQuery.DepartmentId = findSaleReferralCode.DepartmentId;
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(orderId, oldValue, newValue, GarnerFieldName.UPDATE_SALE_REFERRAL_CODE, GarnerHistoryUpdateTables.GAN_ORDER, ActionTypes.CAP_NHAT, "Cập nhật mã giới thiệu"), username);
            }
            _dbContext.SaveChanges();
            return orderQuery;
        }

        /// <summary>
        /// Update tài khoản ngân hàng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="investorBankAccId"></param>
        /// <returns></returns>
        public GarnerOrder UpdateInvestorBankAccount(long orderId, int? investorBankAccId)
        {
            _logger.LogInformation($"{nameof(UpdatePolicyDetail)}: orderId = {orderId}, investorBankAccId = {investorBankAccId}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderFind = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            var findInfoByCifCode = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (findInfoByCifCode != null && findInfoByCifCode.InvestorId != null)
            {
                orderFind.InvestorBankAccId = investorBankAccId;
                _garnerHistoryUpdateEFRepository.HistoryBankAcc(orderId, investorBankAccId, username);
            }
            else if (findInfoByCifCode != null && findInfoByCifCode.BusinessCustomerId != null)
            {
                orderFind.BusinessCustomerBankAccId = investorBankAccId;
            }

            orderFind.ModifiedBy = username;
            orderFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();

            return orderFind;
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerOrder UpdateInfoCustomer(UpdateGarnerInfoCustomerDto input)
        {
            _logger.LogInformation($"{nameof(UpdateInfoCustomer)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderFind = _garnerOrderEFRepository.FindById(input.OrderId, tradingProviderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            //Lấy thông tin BusinessCusstomer / Investor
            var findInfobyCifCode = _cifCodeEFRepository.FindByCifCode(orderFind.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (findInfobyCifCode != null && findInfobyCifCode.InvestorId != null)
            {
                orderFind.InvestorIdenId = input.InvestorIdenId;
                orderFind.ContractAddressId = input.ContractAddressId;
                orderFind.InvestorBankAccId = input.CustomerBankAccId;
                _garnerHistoryUpdateEFRepository.HistoryBankAcc(input.OrderId, input.CustomerBankAccId, username);
                _garnerHistoryUpdateEFRepository.HistoryContactAddress(input.OrderId, input.ContractAddressId, username);
                _garnerHistoryUpdateEFRepository.HistoryinvestorIdentification(input.OrderId, input.InvestorIdenId, username);
            }
            else if (findInfobyCifCode != null && findInfobyCifCode.BusinessCustomerId != null)
            {
                orderFind.BusinessCustomerBankAccId = input.CustomerBankAccId;
            }
            orderFind.ModifiedBy = username;
            orderFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return orderFind;
        }

        #endregion Cập nhật các thông tin của hợp đồng

        #region Trạng thái hợp đồng

        /// đổi trạng thái chờ xử lý sang đang giao
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatusDelivered(long orderId)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatusDelivered)}: orderId = {orderId}");
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            if (orderFind.DeliveryStatus == null || orderFind.DeliveryStatus != DeliveryStatus.WAITING)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderDeliveryStatusDelivery);
            }
            var orderDelivery = _garnerOrderEFRepository.ChangeDeliveryStatusDelivered(orderId, tradingProviderId, modifiedBy);
            _dbContext.SaveChanges();
            return orderDelivery;
        }

        /// <summary>
        /// Đổi trạng thái từ đang giao sang đã nhận
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatusReceived(long orderId)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatusReceived)}: orderId = {orderId}");
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(orderId);
            if (orderFind.DeliveryStatus == null || orderFind.DeliveryStatus != DeliveryStatus.DELIVERY)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderDeliveryStatusReceived);
            }
            var orderDelivery = _garnerOrderEFRepository.ChangeDeliveryStatusReceived(orderId, tradingProviderId, modifiedBy);
            _dbContext.SaveChanges();
            return orderDelivery;
        }

        /// <summary>
        /// Đổi trạng thái từ đã nhận sang hoàn thành
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrder ChangeDeliveryStatusDone(long orderId)
        {
            _logger.LogInformation($"{nameof(ChangeDeliveryStatusDone)}: orderId = {orderId}");
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(orderId);
            if (orderFind.DeliveryStatus == null || orderFind.DeliveryStatus != DeliveryStatus.RECEIVE)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderDeliveryStatusDone);
            }
            var orderDelivery = _garnerOrderEFRepository.ChangeDeliveryStatusDone(orderId, tradingProviderId, modifiedBy);
            _dbContext.SaveChanges();
            return orderDelivery;
        }

        #endregion Trạng thái hợp đồng

        /// <summary>
        /// Gửi lại email thông báo
        /// </summary>
        /// <param name="orderId"></param>
        public async Task ResendNotifyOrderApprove(long orderId)
        {
            _logger.LogInformation($"{nameof(ResendNotifyOrderApprove)}: orderId = {orderId}");

            await _garnerNotificationServices.SendNotifyGarnerOrderActive(orderId);
        }

        #region Giao nhận hợp đồng

        /// <summary>
        /// Get phone không ẩn các số đầu
        /// </summary>
        /// <param name="deliveryCode"></param>
        /// <returns></returns>
        private PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode)
        {
            PhoneReceiveDto result = new();
            var order = _garnerOrderEFRepository.FindByDeliveryCode(deliveryCode).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderHasDeliveryCodeNotFound);
            string phone = string.Empty;
            result.DeliveryStatus = order.DeliveryStatus ?? 1;
            result.TradingProviderId = order.TradingProviderId;
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (cifCode.InvestorId != null)
            {
                var investor = _investorEFRepository.FindById(cifCode.InvestorId ?? 0).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
                phone = investor.Phone;
            }
            else
            {
                var businessCustomer = _businessCustomerEFRepository.FindById(cifCode.BusinessCustomerId ?? 0).ThrowIfNull(_dbContext, ErrorCode.CoreBussinessCustomerNotFound);
                phone = businessCustomer.Phone;
            }
            if (phone == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.InvestorPhoneNotFound);
            }
            result.Phone = phone;
            return result;
        }

        /// <summary>
        /// Get phone by delivery Code ẩn các số đầu
        /// </summary>
        /// <param name="deliveryCode"></param>
        /// <returns></returns>
        public PhoneReceiveDto GetPhoneByDeliveryCodeHidenChar(string deliveryCode)
        {
            var result = GetPhoneByDeliveryCode(deliveryCode);
            var lastThreeChar = result.Phone.Substring(result.Phone.Length - 3);
            var firstChar = new string('*', result.Phone.Length - 3);
            result.Phone = firstChar + lastThreeChar;
            return result;
        }

        public void VerifyPhone(string deliveryCode, string phone, int tradingProviderId)
        {
            var phoneDto = GetPhoneByDeliveryCode(deliveryCode);
            if (phoneDto.DeliveryStatus != DeliveryStatus.DELIVERY)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderDeliveryStatusReceived);
            }
            if (phoneDto.Phone == phone)
            {
                _investorRepository.GenerateOtpByPhone(phone);
            }
            else
            {
                _defErrorEFRepository.ThrowException(ErrorCode.InvestorPhoneNotFound);
            }
        }

        public void ChangeDeliveryStatusRecevired(string deliveryCode, string otp)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var order = _garnerOrderEFRepository.FindByDeliveryCode(deliveryCode).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderHasDeliveryCodeNotFound);
            string phone = string.Empty;
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (cifCode.InvestorId != null)
            {
                var investor = _investorEFRepository.FindById(cifCode.InvestorId ?? 0).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
                phone = investor.Phone;
            }
            else
            {
                var businessCustomer = _businessCustomerEFRepository.FindById(cifCode.BusinessCustomerId ?? 0).ThrowIfNull(_dbContext, ErrorCode.CoreBussinessCustomerNotFound);
                phone = businessCustomer.Phone;
            }
            if (phone == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.InvestorPhoneNotFound);
            }
            var otpDelivery = _authOtpEFRepository.FindActiveOtpByUserIdAndPhone(0, phone).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFoundOTP);
            if (otpDelivery.OtpCode == otp)
            {
                //check xem otp xòn hạn không?
                if (otpDelivery.ExpiredTime < DateTime.Now)
                {
                    _defErrorEFRepository.ThrowException(ErrorCode.InvestorOTPExpire);
                }
                else
                {
                    //check xem hợp đồng giao nhận có phải ở trạng thái "Đang giao" không?
                    if (order.DeliveryStatus != DeliveryStatus.DELIVERY)
                    {
                        _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderDeliveryStatusReceived);
                    }

                    //đổi trạng thái sang đã nhận
                    _garnerOrderEFRepository.UpdateStatusReceive(order.Id, order.TradingProviderId, modifiedBy);
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                _defErrorEFRepository.ThrowException(ErrorCode.InvestorOTPInvalid);
            }
        }

        #endregion Giao nhận hợp đồng

        #region App

        /// <summary>
        /// Kiểm tra trước khi thêm hợp đồng
        /// </summary>
        public void CheckOrder(AppCheckGarnerOrderDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            if (input.InvestorId != null)
            {
                _garnerOrderEFRepository.AppInvestorOrderAdd(_mapper.Map<GarnerOrder>(input), input.InvestorId ?? 0, username, true);
            }
            else
            {
                _garnerOrderEFRepository.AppInvestorOrderAdd(_mapper.Map<GarnerOrder>(input), investorId, username, true);
            }
        }

        /// <summary>
        /// Thêm hợp đồng BondOrder cho nhà đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AppGarnerOrderDto> InvestorOrderAdd(AppCreateGarnerOrderDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var data = await InvestorOrderAddCommon(input, investorId, isSelfDoing: true);
            return data;
        }

        /// <summary>
        /// Sale thêm hợp đồng BondOrder cho nhà đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AppGarnerOrderDto> SaleInvestorOrderAdd(AppSaleCreateGarnerOrderDto input)
        {
            var data = await InvestorOrderAddCommon(input, input.InvestorId, isSelfDoing: false);
            return data;
        }

        /// <summary>
        /// Thêm hợp đồng BondOrder cho nhà đầu tư (dùng chung cho tự đặt lệnh và sale đặt hộ)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AppGarnerOrderDto> InvestorOrderAddCommon(AppCreateGarnerOrderDto input, int investorId, bool isSelfDoing)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var ipCreated = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            _logger.LogInformation($"{nameof(InvestorOrderAddCommon)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}, username = {username}");

            // Nếu là khách hàng tự đặt lệnh thì mới check otp. Sale đặt hộ thì ko cần
            if (isSelfDoing)
            {
                var maxOtpFailCount = _sysVarRepository.GetInvValueByName("AUTH", "OTP_INVALID_COUNT");
                //_authOtpEFRepository.CheckOtpByUserId(CommonUtils.GetCurrentUserId(_httpContext), input.Otp, _httpContext, SessionKeys.OTP_FAIL_COUNT, maxOtpFailCount);
            }

            var inputInsert = new GarnerOrder();
            inputInsert = _mapper.Map<GarnerOrder>(input);

            // Tìm kiếm thông tin chính sách
            var policyFind = _garnerPolicyEFRepository.FindById(input.PolicyId)
                .ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);

            var distributionFind = _garnerDistributionEFRepository.FindById(policyFind.DistributionId)
                .ThrowIfNull<GarnerDistribution>(_dbContext, ErrorCode.GarnerDistributionNotFound);

            /* if (!_garnerDistributionSharedServices.CheckAddTotalValue(policyFind.DistributionId, input.TotalValue))
             {
                 _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderLimitMaxMoney);
             }*/

            inputInsert.ProductId = distributionFind.ProductId;
            inputInsert.DistributionId = policyFind.DistributionId;
            inputInsert.TradingProviderId = policyFind.TradingProviderId;
            inputInsert.IpAddressCreated = ipCreated;

            //Kiểm tra xem là nhà đầu tư cá nhân hay là nhà đầu tư doanh nghiệp
            var findCifCode = _cifCodeEFRepository.FindByInvestor(investorId);
            if (findCifCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            else if (findCifCode.InvestorId != null)
            {
                inputInsert.CifCode = findCifCode.CifCode;
                if (!string.IsNullOrWhiteSpace(input.SaleReferralCode))
                {
                    var findSale = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, policyFind.TradingProviderId);
                    if (findSale != null)
                    {
                        inputInsert.SaleReferralCode = findSale.ReferralCode;
                        inputInsert.DepartmentId = findSale.DepartmentId;
                        inputInsert.SaleReferralCodeSub = findSale.ReferralCodeSub;
                        inputInsert.DepartmentIdSub = findSale.DepartmentIdSub;
                    }
                }
            }
            //Lấy thông tin giấy tờ tùy thân
            inputInsert.InvestorIdenId = _investorEFRepository.GetDefaultIdentification(investorId)?.Id;

            //Lấy thông tin bank
            var getInvestorBank = _investorEFRepository.GetDefaultBankAccount(investorId);
            inputInsert.InvestorBankAccId = input.InvestorBankAccId;

            //Lấy thông tin ngân hàng đại lý
            var tradingProviderBank = _garnerDistributionTradingBankAccountRepository.FindFirstBankCollectByDistribution(policyFind.DistributionId)
                    .ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionTradingBankAccNotFound);
            inputInsert.TradingBankAccId = tradingProviderBank.BusinessCustomerBankAccId;

            // Thêm lệnh. Sale đặt hộ thì lưu thêm sale id
            if (isSelfDoing)
            {
                inputInsert.Source = SourceOrder.ONLINE;
                inputInsert.Status = OrderStatus.CHO_THANH_TOAN;
            }
            else
            {
                var saleId = CommonUtils.GetCurrentSaleId(_httpContext);

                inputInsert.SaleOrderId = saleId;
                inputInsert.Source = SourceOrder.OFFLINE;
                inputInsert.Status = OrderStatus.KHOI_TAO;
            }
            //Lấy thông tin product
            var product = _garnerProductEFRepository.FindById(distributionFind.ProductId).ThrowIfNull(_dbContext, ErrorCode.GarnerProductNotFound);

            //Hàm gen contract Code (Dựa vào contract template xem có trùng configContractId không, nếu tất cả trùng thì gen, nếu không thì null)

            inputInsert.ContractCode = _garnerContractCodeServices.GenOrderContractCodeDefault(new GenGarnerContractCodeDto()
            {
                Order = inputInsert,
                Product = product,
                Policy = policyFind,
            });

            var transaction = _dbContext.Database.BeginTransaction();
            var insertOrder = _garnerOrderEFRepository.AppInvestorOrderAdd(inputInsert, investorId, username);
            _dbContext.SaveChanges();
            var orderQuery = _garnerOrderEFRepository.FindById(insertOrder.Id);
            if (orderQuery == null) return null;
            var data = _garnerContractDataServices.GetDataContractFile(orderQuery, insertOrder.TradingProviderId, true);
            _backgroundJobs.Enqueue(() => _garnerBackgroundJobService.CreateContractFileByOrderApp(orderQuery, insertOrder.TradingProviderId, data));

            // Lấy danh sách ngân hàng thụ hưởng của đại lý sơ cấp
            insertOrder.TradingBankAccounts = await TradingBankAccountOfDistribution(insertOrder.Id, insertOrder.DistributionId, insertOrder.ContractCode, insertOrder.TotalValue);
            transaction.Commit();
            return insertOrder;
        }

        /// <summary>
        /// Kiểm tra sale theo đại lý của chính sách ch
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyId)
        {
            _logger.LogInformation($"{nameof(AppSaleOrderFindReferralCode)}: referralCode = {referralCode}, policyId = {policyId} ");

            var policyFind = _garnerPolicyEFRepository.FindById(policyId);
            if (policyFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyNotFound);
            }
            var result = _saleEFRepository.FindSaleByReferralCode(referralCode, policyFind.TradingProviderId);
            return _mapper.Map<AppSaleByReferralCodeDto>(result);
        }

        /// <summary>
        /// Lấy danh sách order của nhà đầu tư cá nhân
        /// Được nhóm theo chính sách Policy
        /// </summary>
        /// <param name="groupOrder"></param>
        /// <returns></returns>
        public List<AppGarnerOrderByPolicyDto> AppInvestorGetListOrder(int groupOrder)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            _logger.LogInformation($"{nameof(AppInvestorGetListOrder)}: investorId = {investorId}, groupOrder = {groupOrder} ");

            // Lấy danh sách hợp đồng sổ lệnh đã được gom nhóm
            var resultGroupPolicy = _garnerOrderEFRepository.AppGetListOrder(investorId, groupOrder);

            var now = DateTime.Now.Date;

            var result = new List<AppGarnerOrderByPolicyDto>();
            // For theo từng cụm chính sách
            foreach (var groupPolicyItem in resultGroupPolicy)
            {
                var resultItem = new AppGarnerOrderByPolicyDto();
                var resultOrderListItem = new List<AppGarnerOrderListDto>();
                var withdrawalFind = new GarnerWithdrawalByPolicyDto();
                resultItem = _mapper.Map<AppGarnerOrderByPolicyDto>(groupPolicyItem);

                // Tổng số dư : cộng dồng TotalValue của các hợp đồng sổ lệnh
                decimal totalCurrentBalance = 0;

                var policy = _garnerPolicyEFRepository.FindById(groupPolicyItem.PolicyId)
                    .ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);

                //Lấy thông tin chính sách để lấy ra số tiền rút vốn tối thiểu và số tiền rút tối đa
                resultItem.MinWithdraw = policy.MinWithdraw;
                resultItem.MaxWithdraw = policy.MaxWithdraw;

                // % thuế lợi nhuận
                decimal taxRate = policy.IncomeTax / 100;

                // For theo hợp đồng trong chính sách
                foreach (var orderItem in groupPolicyItem.Orders)
                {
                    var resultOrderItem = new AppGarnerOrderListDto();

                    resultOrderItem = _mapper.Map<AppGarnerOrderListDto>(orderItem);

                    // Nếu có thông tin sale bán hộ thì gán vào mã giới thiệu
                    if (orderItem.SaleReferralCodeSub != null)
                    {
                        resultOrderItem.SaleReferralCode = orderItem.SaleReferralCodeSub;
                    }
                    // Lấy thông tin tên sale
                    var findSale = _saleEFRepository.FindSaleName(resultOrderItem.SaleReferralCode);
                    if (findSale != null)
                    {
                        resultOrderItem.SaleName = findSale.Name;
                    }
                    totalCurrentBalance += orderItem.TotalValue;
                    // Nếu có ngày đầu tư
                    if (orderItem.InvestDate != null)
                    {
                        int numInvestDays = (now - orderItem.InvestDate.Value.Date).Days;

                        var policyDetail = _garnerPolicyDetailEFRepository.FindPolicyDetailByDate(orderItem.PolicyId, orderItem.InvestDate.Value.Date, now);
                        if (policyDetail != null)
                        {
                            //% lợi nhuận
                            decimal profitRate = policyDetail.Profit / 100;

                            //số tiền đã trả theo lệnh
                            decimal profitPaid = _garnerInterestPaymentEFRepository.ProfitPaid(orderItem.Id);

                            var profitResult = _garnerInterestPaymentEFRepository
                                .CalculateProfit(policy.CalculateType, orderItem.TotalValue, numInvestDays, profitRate, taxRate, profitPaid);

                            resultOrderItem.Profit = Math.Round(profitResult.ActualProfit);
                        }
                    }

                    resultItem.TotalCurrentProfit += resultOrderItem.Profit;
                    resultOrderListItem.Add(resultOrderItem);
                }

                //Nếu màn sổ lệnh check xem có yêu cầu rút vốn không
                if (groupOrder == AppOrderGroupStatus.SO_LENH)
                {
                    // Tìm xem có yêu cầu rút vốn hay không
                    var listWithdrawalFind = _garnerWithdrawalEFRepository.GetListWithdrawalByPolicyId(investorId, groupPolicyItem.PolicyId, new List<int> { WithdrawalStatus.CHO_PHAN_HOI, WithdrawalStatus.YEU_CAU });
                    foreach (var withdrawalItem in listWithdrawalFind)
                    {
                        //Lặp qua từng lệnh chờ rút để đổ data
                        var resultOrderItem = new AppGarnerOrderListDto()
                        {
                            CreatedDate = withdrawalItem.CreatedDate,
                            WithdrawalId = withdrawalItem.WithdrawalId,
                            WithdrawalMoney = withdrawalItem.AmountMoney,
                            OtherStatus = GarnerOrderOtherStatus.CHO_DUYET_RUT_VON
                        };
                        resultOrderListItem.Add(resultOrderItem);
                    }
                }

                resultItem.Orders = resultOrderListItem.OrderByDescending(o => o.Id).ToList();

                if (groupOrder == AppOrderGroupStatus.SO_LENH)
                {
                    resultItem.Orders = resultOrderListItem.OrderByDescending(o => o.CreatedDate).ToList();
                }

                // Tính số ngày tích lũy / Tính từ ngày đầu tư hợp đồng cũ nhất
                var cumulativeDays = groupPolicyItem.Orders.Min(o => o.ActiveDate);
                if (cumulativeDays != null)
                {
                    resultItem.CumulativeDays = (now - cumulativeDays.Value.Date).Days;
                }
                // Tổng số dư
                resultItem.TotalCurrentBalance = totalCurrentBalance;

                result.Add(resultItem);
            }
            return result;
        }

        /// <summary>
        /// Xem màn lịch sử đầu tư
        /// </summary>
        /// <param name="groupOrder"></param>
        /// <returns></returns>
        public List<AppGarnerOrderByPolicyDto> AppInvestorGetListOrderHistory(int groupOrder)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            _logger.LogInformation($"{nameof(AppInvestorGetListOrder)}: investorId = {investorId}, groupOrder = {groupOrder} ");

            // Lấy danh sách hợp đồng sổ lệnh đã được gom nhóm
            var resultGroupPolicy = _garnerOrderEFRepository.AppGetListOrder(investorId, groupOrder);

            var result = new List<AppGarnerOrderByPolicyDto>();
            // For theo từng cụm chính sách
            foreach (var groupPolicyItem in resultGroupPolicy)
            {
                var resultItem = new AppGarnerOrderByPolicyDto();
                var resultOrderListItem = new List<AppGarnerOrderListDto>();
                var listWithdrawalFind = new List<GarnerWithdrawalByPolicyDto>();
                resultItem = _mapper.Map<AppGarnerOrderByPolicyDto>(groupPolicyItem);

                // Hợp đồng tất toán / join đến yêu cầu rút vốn gần nhất được duyệt để lấy Id xem chi tiết rút vốn
                var orderSettlement = from order in resultItem.Orders
                                      join withdrawalDetail in _dbContext.GarnerWithdrawalDetails on order.Id equals withdrawalDetail.OrderId
                                      join withdrawal in _dbContext.GarnerWithdrawals on withdrawalDetail.WithdrawalId equals withdrawal.Id
                                      where (withdrawal.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN || withdrawal.Status == WithdrawalStatus.DUYET_DI_TIEN)
                                          && withdrawal.Deleted == YesNo.NO
                                          && order.Status == OrderStatus.TAT_TOAN
                                          && withdrawal.Id == (from order2 in resultItem.Orders // Dùng để lấy WithdrawalId lớn nhất
                                                               join withdrawalDetail2 in _dbContext.GarnerWithdrawalDetails on order2.Id equals withdrawalDetail2.OrderId
                                                               where order2.Id == order.Id
                                                               select withdrawalDetail2.WithdrawalId).Max()
                                      select new AppGarnerOrderListDto
                                      {
                                          Id = order.Id,
                                          ContractCode = order.ContractCode,
                                          PaymentFullDate = order.PaymentFullDate,
                                          ActiveDate = order.ActiveDate,
                                          InvestDate = order.InvestDate,
                                          BuyDate = order.BuyDate,
                                          Status = order.Status,
                                          InitTotalValue = order.InitTotalValue,
                                          TotalValue = order.TotalValue,
                                          CreatedDate = order.SettlementDate ?? order.BuyDate,
                                          WithdrawalId = withdrawal.Id,
                                          WithdrawalMoney = withdrawal.AmountMoney,
                                      };
                resultOrderListItem.AddRange(orderSettlement);

                // Tìm xem có yêu cầu rút vốn hay không
                listWithdrawalFind = _garnerWithdrawalEFRepository.GetListWithdrawalByPolicyId(investorId, groupPolicyItem.PolicyId, new List<int> { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.DUYET_KHONG_DI_TIEN, WithdrawalStatus.HUY_YEU_CAU });
                //Lặp qua từng lệnh chờ rút để đổ data
                foreach (var withdrawalItem in listWithdrawalFind)
                {
                    resultOrderListItem.Add(new AppGarnerOrderListDto()
                    {
                        // Nếu là hủy lấy ra ngày hủy
                        CreatedDate = withdrawalItem.Status == WithdrawalStatus.HUY_YEU_CAU ? withdrawalItem.CancelDate : withdrawalItem.ApproveDate,
                        WithdrawalId = withdrawalItem.WithdrawalId,
                        WithdrawalMoney = withdrawalItem.AmountMoney,
                        // Nếu là hủy thì đổ ra trạng thái Hủy
                        OtherStatus = withdrawalItem.Status == WithdrawalStatus.HUY_YEU_CAU ? GarnerOrderOtherStatus.HUY_DUYET_RUT_VON : GarnerOrderOtherStatus.RUT_VON_THANH_CONG
                    });
                }
                resultItem.Orders = resultOrderListItem.OrderByDescending(o => o.CreatedDate).ToList();
                //Lấy thông tin chính sách để lấy ra số tiền rút vốn tối thiểu và số tiền rút tối đa
                var getPolicy = _garnerPolicyEFRepository.FindById(groupPolicyItem.PolicyId).ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);
                resultItem.MinWithdraw = getPolicy.MinWithdraw;
                resultItem.MaxWithdraw = getPolicy.MaxWithdraw;

                result.Add(resultItem);
            }
            return result;
        }

        /// <summary>
        /// App xem chi tiết thông tin lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<AppGarnerOrderDetailDto> AppOrderDetail(long orderId)
        {
            var result = new AppGarnerOrderDetailDto();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            _logger.LogInformation($"{nameof(AppOrderDetail)}: investorId = {investorId}, orderId = {orderId} ");
            // Tìm hợp đồng
            var orderFind = _garnerOrderEFRepository.EntityNoTracking.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);

            // Tìm thông tin chính sách
            var policyFind = _garnerPolicyEFRepository.FindById(orderFind.PolicyId)
                .ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);

            result = _mapper.Map<AppGarnerOrderDetailDto>(orderFind);
            result.PolicyId = policyFind.Id;
            result.PolicyName = policyFind.Name;
            result.MaxWithdrawal = policyFind.MaxWithdraw;
            result.MinWithdrawal = policyFind.MinWithdraw;
            result.PaymentNote = PaymentNotes.THANH_TOAN + orderFind.ContractCode;

            // Hiển thị Chu kỳ nhận lợi thức
            if (policyFind.GarnerType == GarnerPolicyTypes.KHONG_CHON_KY_HAN)
            {
                result.InterestTypeName = InterestTypes.InterestTypeNames(policyFind.InterestType, policyFind.RepeatFixedDate);
            }

            var investortBankFind = _investorEFRepository.FindBankById(orderFind.InvestorBankAccId ?? 0);
            if (investortBankFind != null)
            {
                result.OwnerAccount = investortBankFind.OwnerAccount;
                result.BankAccount = investortBankFind.BankAccount;
                result.BankName = investortBankFind.CoreBankName;
            }

            var identificationFind = _investorEFRepository.GetIdentificationById(orderFind.InvestorIdenId ?? 0);
            if (identificationFind != null)
            {
                result.IdNo = identificationFind.IdNo;
                result.IdType = identificationFind.IdType;
            }

            result.SaleReferralCode = orderFind.SaleReferralCode;
            if (orderFind.SaleReferralCodeSub != null)
            {
                result.SaleReferralCode = orderFind.SaleReferralCodeSub;
            }
            var saleFind = _saleEFRepository.FindSaleName(result.SaleReferralCode);
            if (saleFind != null)
            {
                result.SaleName = saleFind?.Name;
            }
            //Lợi tức dự kiến
            //Tìm thông kin kỳ hạn để lấy lợi tức max
            var policyDetailList = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(policyFind.Id);
            if (policyDetailList.Any())
            {
                result.ExpectedProfit = policyDetailList.Max(e => e.Profit);
            }

            // Lấy danh sách ngân hàng thụ hưởng của đại lý sơ cấp
            // Để nhà đầu tư chọn thanh toán

            result.TradingBankAccounts = await TradingBankAccountOfDistribution(orderFind.Id, orderFind.DistributionId, orderFind.ContractCode, orderFind.TotalValue);
            return result;
        }

        /// <summary>
        /// App hủy duyệt trọng trạng thái chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        public void AppOrderCancel(long orderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppOrderCancel)}: investorId = {investorId}, orderId = {orderId} ");
            var cifCodeFind = _cifCodeEFRepository.FindByInvestor(investorId).ThrowIfNull<CifCodes>(_dbContext, ErrorCode.CoreCifCodeNotFound);
            var orderFind = _garnerOrderEFRepository.Entity.FirstOrDefault(o => o.Id == orderId && o.CifCode == cifCodeFind.CifCode && o.Deleted == YesNo.NO)
                            .ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);

            // Xem có trường hợp đã thanh toán nào không
            if (_dbContext.GarnerOrderPayments.Where(p => p.OrderId == orderId && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO).Any())
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderExistPaymentNotDeleted);
            }
            if (orderFind.Status == OrderStatus.CHO_THANH_TOAN || orderFind.Status == OrderStatus.KHOI_TAO)
            {
                orderFind.Deleted = YesNo.YES;
            }
            else
            {
                _garnerOrderEFRepository.ThrowException(ErrorCode.GarnerOrderNotInStatusCancel);
            }
            _dbContext.SaveChanges();
        }

        #endregion App

        #region Các hàm dùng lại

        /// <summary>
        /// Lấy danh sách ngân hàng thụ hưởng của đại lý cài trong sản phẩm tích lũy
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public async Task<List<AppTradingBankAccountDto>> TradingBankAccountOfDistribution(long orderId, int distributionId, string contractCode, decimal totalValue)
        {
            _logger.LogInformation($"{nameof(TradingBankAccountOfDistribution)}: orderId = {orderId}, distributionId = {distributionId}, contractCode = {contractCode}, totalValue = {totalValue}");
            var result = new List<AppTradingBankAccountDto>();
            var listTradingBankAccount = _garnerDistributionTradingBankAccountRepository.FindAllListByDistribution(distributionId, DistributionTradingBankAccountTypes.THU);
            foreach (var bankAccountItem in listTradingBankAccount)
            {
                var resultBankItem = new AppTradingBankAccountDto();
                var tradingBankFind = _businessCustomerEFRepository.FindBankById(bankAccountItem.BusinessCustomerBankAccId);
                if (tradingBankFind == null)
                {
                    continue;
                }
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
                                OrderCode = $"{ContractCodes.GARNER}{orderId}",
                                AmountMoney = totalValue,
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
                                _logger.LogError(ex, $"{nameof(TradingBankAccountOfDistribution)}: exception = {ex.Message}");
                            }
                        }
                    }
                }
                result.Add(resultBankItem);
            }
            return result;
        }

        #endregion Các hàm dùng lại
    }
}