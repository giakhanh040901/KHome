using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.Order;
using EPIC.CompanySharesEntities.Dto.OrderContractFile;
using EPIC.CompanySharesEntities.Dto.SaleInvestor;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Bank;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Investor;
//using EPIC.Entities.Dto.BondOrder;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Implements
{
    public class OrderServices : IOrderServices
    {
        private readonly ILogger<OrderServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly OrderRepository _orderRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        //private readonly BondOrderPaymentRepository _orderPaymentRepository;
        //private readonly BondIssuerRepository _issuerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        //private readonly BondInfoRepository _productBondInfoRepository;
        //private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly CpsSecondaryRepository _cpsSecondaryRepository;

        //private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        //private readonly BondCalendarRepository _calendarRepository;
        private readonly BankRepository _bankRepository;
        private readonly ContractTemplateRepository _contractTemplateRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;

        //private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly DepartmentRepository _departmentRepository;
        //private readonly IBondOrderShareServices _bondOrderShareServices;
        private readonly SaleRepository _saleRepository;
        //private readonly BondRenewalsRequestRepository _renewalsRequestRepository;
        //private readonly IBondSharedService _bondSharedService;
        private readonly IContractDataServices _contractDataServices;
        private readonly IContractTemplateServices _contractTemplateServices;
        //private readonly IRocketChatServices _rocketChatServices;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly UserRepository _userRepository;
        private readonly SysVarRepository _sysVarRepository;
        //private readonly BondBlockadeLiberationRepository _blockadeLiberationRepository;
        //private readonly BondInterestPaymentRepository _interestPaymentRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly CpsBackgroundJobService _cpsBackgroundJobService;


        public OrderServices(
            ILogger<OrderServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            //IBondSharedService bondSharedService,
            IContractDataServices contractDataServices,
            IContractTemplateServices contractTemplateServices,
            //IRocketChatServices rocketChatServices,
            IBackgroundJobClient backgroundJobs,
            CpsBackgroundJobService cpsBackgroundJobService,
            //IBondOrderShareServices bondOrderShareServices,
            IOptions<FileConfig> fileConfig)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _mapper = mapper;
            _orderRepository = new OrderRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            //_orderPaymentRepository = new BondOrderPaymentRepository(_connectionString, _logger);
            //_issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            //_productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            //_productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _cpsSecondaryRepository = new CpsSecondaryRepository(_connectionString, _logger);
            //_productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            //_calendarRepository = new BondCalendarRepository(_connectionString, _logger);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            //_renewalsRequestRepository = new BondRenewalsRequestRepository(_connectionString, _logger);
            _contractTemplateRepository = new ContractTemplateRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            //_distributionContractRepository = new BondDistributionContractRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            //_bondOrderShareServices = bondOrderShareServices;
            _contractDataServices = contractDataServices;
            //_mapper = mapper;
            //_bondSharedService = bondSharedService;
            _contractTemplateServices = contractTemplateServices;
            //_rocketChatServices = rocketChatServices;
            _fileConfig = fileConfig;
            _backgroundJobs = backgroundJobs;
            _cpsBackgroundJobService = cpsBackgroundJobService;
            _userRepository = new UserRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            //_blockadeLiberationRepository = new BondBlockadeLiberationRepository(_connectionString, _logger);
            //_interestPaymentRepository = new BondInterestPaymentRepository(_connectionString, _logger);
        }

        //public async Task<CpsOrder> Add(CreateOrderDto input)
        public CompanySharesEntities.DataEntities.Order Add(CreateOrderDto input) // tạm thời comment UpdateContractFile
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var cpsOrder = new CompanySharesEntities.DataEntities.Order()
            {
                TradingProviderId = tradingProviderId,
                CifCode = input.CifCode,
                CpsSecondaryId = input.SecondaryId,
                CpsPolicyId = input.PolicyId,
                CpsPolicyDetailId = input.PolicyDetailId,
                InvestorBankAccId = input.InvestorBankAccId,
                TotalValue = input.TotalValue,
                BuyDate = DateTime.Now,
                IsInterest = input.IsInterest,
                SaleReferralCode = input.SaleReferralCode,
                CreatedBy = username,
            };
            var cifCodeFind = _cifCodeRepository.GetByCifCode(input.CifCode);
            if (cifCodeFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mã cif của khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            if (cifCodeFind.InvestorId != null)
            {
                //Lấy giấy tờ mặc định
                var investorIdentificationFind = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);
                if (investorIdentificationFind != null)
                {
                    cpsOrder.InvestorIdenId = investorIdentificationFind.Id;
                }

                var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId ?? 0);
                if (investorFind != null)
                {
                    if (investorFind.ReferralCodeSelf == input.SaleReferralCode)
                    {
                        throw new FaultException(new FaultReason($"Mã giới thiệu đang trùng với khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                    }
                }
                //Lấy địa chỉ giao dịch mặc định
                if (input.ContractAddressId == null)
                {
                    var investorContractAddressFind = _investorRepository.GetContactAddressDefault(cifCodeFind.InvestorId ?? 0);
                    if (investorContractAddressFind != null)
                    {
                        cpsOrder.ContractAddressId = investorContractAddressFind.ContactAddressId;
                    }
                }
                else
                {
                    cpsOrder.ContractAddressId = input.ContractAddressId;
                }
            }
            var result = _orderRepository.Add(cpsOrder);
            //await UpdateContractFile((int)result.Id);
            return result;
        }

        public int Delete(int id)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.Delete(id, tradingProviderId);
        }

        public int UpdateTotalValue(int orderId, decimal? totalValue)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.UpdateTotalValue(orderId, tradingProviderId, totalValue, username);
        }

        public int UpdateReferralCode(int orderId, string referralCode)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _orderRepository.FindById(orderId, tradingProviderId);
            if (orderFind != null)
            {
                var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                if (cifCodeFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy mã cif của khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }

                if (cifCodeFind.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        if (investorFind.ReferralCodeSelf == referralCode)
                        {
                            throw new FaultException(new FaultReason($"Mã giới thiệu đang trùng với khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                        }
                    }
                }
            }
            return _orderRepository.UpdateReferralCode(orderId, tradingProviderId, referralCode, username);
        }

        public int UpdatePolicyDetail(int orderId, int? bondPolicyDetailId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.UpdatePolicyDetail(orderId, tradingProviderId, bondPolicyDetailId, username);
        }

        public int UpdateSource(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.UpdateSource(orderId, tradingProviderId, username);
        }

        /// <summary>
        /// Cập nhật phương thức tất toán cuối kỳ trên CMS
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public int UpdateSettlementMethod(int orderId, SettlementMethodDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.AppUpdateSettlementMethod(orderId, input, username, null, tradingProviderId);
        }

        /// <summary>
        /// Cập nhật phương thức tất toán cuối kỳ cho App
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public int AppUpdateSettlementMethod(int orderId, SettlementMethodDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            return _orderRepository.AppUpdateSettlementMethod(orderId, input, username, investorId);
        }

        public int OrderApprove(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.OrderApprove(orderId, tradingProviderId, username);
        }

        public int OrderCancel(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.OrderCancel(orderId, tradingProviderId, username);
        }

        public PagingResult<ViewOrderDto> FindAll(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer)
        {
            int? tradingProviderId = null;
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            List<int?> listTradingChildIds = null;
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

                var listTradingProvider = _partnerRepository.FindTradingProviderByPartner(partnerId ?? 0);
                listTradingChildIds = listTradingProvider.Select(t => t.TradingProviderId).Distinct().ToList();
            }
            var orderList = _orderRepository.FindAll(tradingProviderId, taxCode, idNo, cifCode, phone, pageSize, pageNumber, keyword, status, groupStatus, source, cpsSecondaryId, cpsPolicy, cpsPolicyDetailId, customerName, contractCode, tradingDate, deliveryStatus, orderer, listTradingChildIds);
            var result = new PagingResult<ViewOrderDto>();
            var items = new List<ViewOrderDto>();
            result.TotalItems = orderList.TotalItems;
            foreach (var orderFind in orderList.Items)
            {
                var orderItem = _mapper.Map<ViewOrderDto>(orderFind);

                orderItem.IsRenewalsRequest = orderItem.RenewalsRequestId is not null;

                var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                if (cifCodeFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy mã cif của khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                if (cifCodeFind.InvestorId != null && cifCodeFind.BusinessCustomerId == null)
                {
                    orderItem.CustomerType = CustomerType.IS_INVETOR;
                }
                if (cifCodeFind.InvestorId == null && cifCodeFind.BusinessCustomerId != null)
                {
                    orderItem.CustomerType = CustomerType.IS_BUSINESSCUSTOMER;
                }
                if (cifCodeFind.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId.Value);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        orderItem.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            orderItem.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                    }
                }
                else if (cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    if (businessCustomerFind != null)
                    {
                        var businessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerFind);
                        orderItem.BusinessCustomer = businessCustomer;
                    }
                }

                /*var cpsSecondaryFind = _productCpsSecondaryRepository.FindSecondaryById(orderFind.CpsSecondaryId ?? 0, orderFind.TradingProviderId);
                if (cpsSecondaryFind != null)
                {
                    var listPolicies = _productCpsSecondaryRepository.GetAllPolicy(cpsSecondaryFind.CpsSecondaryId, orderFind.TradingProviderId ?? 0);
                    orderItem.CpsPolicies = _mapper.Map<List<ViewProductCpsPolicyDto>>(listPolicies);
                }
                var cpsInfoFind = _productCpsInfoRepository.FindById(orderFind.ProductCpsId ?? 0);
                if (cpsInfoFind != null)
                {
                    orderItem.CpsInfo = _mapper.Map<ProductCpsInfoDto>(cpsInfoFind);
                }
                var cpsPolicyFind = _productCpsSecondaryRepository.FindPolicyById(orderFind.CpsPolicyId ?? 0, orderFind.TradingProviderId);
                if (cpsPolicyFind != null)
                {
                    orderItem.CpsPolicy = _mapper.Map<ViewProductCpsPolicyDto>(cpsPolicyFind);
                }
                var priceFind = _productCpsSecondPriceRepository.FindByDate(orderFind.CpsSecondaryId ?? 0, orderFind.PaymentFullDate ?? DateTime.Now, orderFind.TradingProviderId ?? 0);
                if (priceFind != null)
                {
                    orderItem.BuyPrice = priceFind.Price;
                }
                var cpsPolicyDetail = _productCpsSecondaryRepository.FindPolicyDetailById(orderFind.CpsPolicyDetailId ?? 0, orderFind.TradingProviderId ?? 0);
                if (cpsPolicyDetail != null)
                {
                    orderItem.CpsPolicyDetail = _mapper.Map<ViewCpsPolicyDetailDto>(cpsPolicyDetail);
                }
                var blockadeLiberation = _blockadeLiberationRepository.FindById(orderFind.BlockadeLiberationId, orderFind.TradingProviderId);
                if (blockadeLiberation != null)
                {
                    orderItem.BlockadeLiberationId = blockadeLiberation.Id;
                }*/

                items.Add(orderItem);
            }
            result.Items = items;
            return result;
        }

        // get all delivery status cua giao nhan hop dong
        public PagingResult<ViewOrderDto> FindAllDeliveryStatus(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderList = _orderRepository.FindAllDeliveryStatus(tradingProviderId, taxCode, idNo, cifCode, phone, pageSize, pageNumber, keyword, status, groupStatus, source, bondSecondaryId, bondPolicy, bondPolicyDetailId, customerName, contractCode, tradingDate, deliveryStatus);
            var result = new PagingResult<ViewOrderDto>();
            var items = new List<ViewOrderDto>();
            result.TotalItems = orderList.TotalItems;
            foreach (var orderFind in orderList.Items)
            {
                var orderItem = _mapper.Map<ViewOrderDto>(orderFind);
                var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                if (cifCodeFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy mã cif của khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                if (cifCodeFind.InvestorId != null && cifCodeFind.BusinessCustomerId == null)
                {
                    orderItem.CustomerType = CustomerType.IS_INVETOR;
                }
                if (cifCodeFind.InvestorId == null && cifCodeFind.BusinessCustomerId != null)
                {
                    orderItem.CustomerType = CustomerType.IS_BUSINESSCUSTOMER;
                }
                if (cifCodeFind.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId.Value);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        orderItem.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            orderItem.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                    }
                }
                else if (cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    if (businessCustomerFind != null)
                    {
                        var businessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerFind);
                        orderItem.BusinessCustomer = businessCustomer;
                    }
                }

                /*var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryById(orderFind.Id ?? 0, tradingProviderId);

                if (bondSecondaryFind != null)
                {
                    var listPolicies = _productBondSecondaryRepository.GetAllPolicy(bondSecondaryFind.Id, tradingProviderId);
                    orderItem.BondPolicies = _mapper.Map<List<ViewProductBondPolicyDto>>(listPolicies);
                }
                var bondInfoFind = _productBondInfoRepository.FindById(orderFind.BondId ?? 0);
                if (bondInfoFind != null)
                {
                    orderItem.BondInfo = _mapper.Map<ProductBondInfoDto>(bondInfoFind);
                }

                var bondPolicyFind = _productBondSecondaryRepository.FindPolicyById(orderFind.Id ?? 0, tradingProviderId);
                if (bondPolicyFind != null)
                {
                    orderItem.BondPolicy = _mapper.Map<ViewProductBondPolicyDto>(bondPolicyFind);
                }

                var priceFind = _productBondSecondPriceRepository.FindByDate(orderFind.Id ?? 0, orderFind.PaymentFullDate ?? DateTime.Now, tradingProviderId);
                if (priceFind != null)
                {
                    orderItem.BuyPrice = priceFind.Price;
                }

                var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(orderFind.PolicyDetailId ?? 0, tradingProviderId);
                if (bondPolicyDetail != null)
                {
                    orderItem.BondPolicyDetail = _mapper.Map<ViewProductBondPolicyDetailDto>(bondPolicyDetail);
                }

                var blockadeLiberation = _blockadeLiberationRepository.FindById(orderFind.BlockadeLiberationId, tradingProviderId);
                if (blockadeLiberation != null)
                {
                    orderItem.BlockadeLiberationId = blockadeLiberation.Id;
                }*/
                items.Add(orderItem);
            }
            result.Items = items;
            return result;
        }

        public ViewOrderDto FindById(int id)
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
            }

            var orderFind = _orderRepository.FindById(id, tradingProviderId, partnerId);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var result = _mapper.Map<ViewOrderDto>(orderFind);

            //result.IsRenewalsRequest = _renewalsRequestRepository.GetByOrderId(id, orderFind.TradingProviderId) is not null;

            //Tìm kiếm giá 
            /*var cspPriceFind = _productCpsSecondPriceRepository.FindByDate(orderFind.CpsSecondaryId ?? 0, orderFind.PaymentFullDate ?? DateTime.Now, orderFind.TradingProviderId);
            if (cspPriceFind != null)
            {
                result.BuyPrice = cspPriceFind.Price;
            }*/

            //Tìm kiếm là khách hàng cá nhân hay là khách hàng doanh nghiệpListContactAddress
            var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
            if (cifCodeFind != null)
            {
                if (cifCodeFind.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId.Value);
                    //var investorFind = _managerInvestorRepository.FindById(cifCodeFind.InvestorId.Value, false);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        var listBank = _managerInvestorRepository.GetListBank(investorFind.InvestorId, investorFind.InvestorGroupId, false)?.ToList();
                        var listContractAddress = _managerInvestorRepository.GetListContactAddress(-1, 0, null, investor.InvestorId, false).Items.ToList();
                        investor.ListBank = listBank;
                        result.ListContactAddress = listContractAddress;
                        result.Investor = investor;

                        var investorIdenDefaultFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
                        if (investorIdenDefaultFind != null)
                        {
                            result.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                    }
                }
                else if (cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId.Value);
                    if (businessCustomerFind != null)
                    {
                        var businessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerFind);
                        var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerFind.BusinessCustomerId ?? 0, -1, 0, null);
                        businessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                        result.BusinessCustomer = businessCustomer;
                    }
                }
            }

            //Lấy thông tin lô cổ phần
            /*var cspInfoFind = _productCpsInfoRepository.FindById(orderFind.ProductCpsId ?? 0);
            if (cspInfoFind != null)
            {
                result.CpsInfo = _mapper.Map<ProductCpsInfoDto>(cspInfoFind);
            }

            //Lấy thông tin phát hành sơ cấp
            var cpsSecondaryFind = _productCpsSecondaryRepository.FindSecondaryById(orderFind.CpsSecondaryId ?? 0, orderFind.TradingProviderId);
            if (cpsSecondaryFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin phát hành thứ cấp"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            else if (cpsSecondaryFind != null)
            {
                var secondary = _mapper.Map<ViewProductCpsSecondaryDto>(cpsSecondaryFind);

                secondary.SoLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(orderFind.TradingProviderId, cpsSecondaryFind.CpsPrimaryId);
                secondary.SoLuongConLai = secondary.SoLuongTraiPhieuNamGiu - _orderRepository.SumQuantity(tradingProviderId, secondary.CpsSecondaryId);
                //Tài khoản thụ hưởng của dlsc muốn nhận khi bán theo kỳ hạn
                result.BusinessCustomerBankAccId = cpsSecondaryFind.BusinessCustomerBankAccId;

                var businesBankAccFind = _businessCustomerRepository.FindBusinessCusBankById(cpsSecondaryFind.BusinessCustomerBankAccId ?? 0);

                if (businesBankAccFind != null)
                {
                    result.TradingProviderBank = _mapper.Map<BusinessCustomerBankDto>(businesBankAccFind);
                }
                var primary = _productCpsPrimaryRepository.FindById(cpsSecondaryFind.CpsPrimaryId, null);
                if (primary != null)
                {
                    secondary.ProductCpsInfo = _productCpsInfoRepository.FindById(primary.ProductCpsId);
                }

                //Lấy thông tin tổ chức phát hành
                secondary.ProductCpsPrimary = _mapper.Map<ProductCpsPrimaryDto>(primary);

                //Lấy thông tin chính sách và kỳ hạn
                secondary.Policies = new List<ViewProductCpsPolicyDto>();
                var policyList = _productCpsSecondaryRepository.GetAllPolicy(secondary.CpsSecondaryId, secondary.TradingProviderId);
                foreach (var policyItem in policyList)
                {
                    var policy = _mapper.Map<ViewProductCpsPolicyDto>(policyItem);

                    policy.FakeId = policyItem.CpsPolicyId;
                    policy.Details = new List<ViewProductCpsPolicyDetailDto>();

                    var cpsPolicyDetailList = _productCpsSecondaryRepository.GetAllPolicyDetail(policy.CpsPolicyId, secondary.TradingProviderId);
                    foreach (var detailItem in cpsPolicyDetailList)
                    {
                        var detail = _mapper.Map<ViewProductCpsPolicyDetailDto>(detailItem);
                        detail.FakeId = detailItem.CpsPolicyDetailId;
                        policy.Details.Add(detail);
                    }
                    secondary.Policies.Add(policy);
                }
                result.CpsSecondary = secondary;
            }

            var cpsPolicyDetailFind = _productCpsSecondaryRepository.FindPolicyDetailById(orderFind.CpsPolicyDetailId ?? 0, orderFind.TradingProviderId);
            if (cpsPolicyDetailFind != null)
            {
                var ngayDauTu = DateTime.Now.Date;
                if (result.BuyDate != null && result.InvestDate == null && result.PaymentFullDate == null)
                {
                    ngayDauTu = result.BuyDate.Value;
                }
                else if (result.InvestDate != null)
                {
                    ngayDauTu = result.InvestDate.Value;
                }
                result.EndDate = _cpsSharedService.CalculateDueDate(cpsPolicyDetailFind, ngayDauTu);
            }*/

            var businessCusBankFind = _businessCustomerRepository.FindBusinessCusBankById(orderFind.BusinessCustomerBankAccId ?? 0);
            if (businessCusBankFind != null)
            {
                var businessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCusBankFind);
                result.BusinessCustomerBank = businessCustomerBank;
            }

            AppSaleByReferralCodeDto saleFind = null;
            try
            {
                saleFind = _saleRepository.FindSaleByReferralCode(orderFind.SaleReferralCode, orderFind.TradingProviderId);
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
                    var investorFind = _investorRepository.FindById(saleFind.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        result.Sale.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleFind.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            result.Sale.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }

                        var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleFind.InvestorId ?? 0);
                        result.Sale.Investor.ListBank = _mapper.Map<List<InvestorBankAccount>>(investorFindBank);
                    }
                }
                else if (saleFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleFind.BusinessCustomerId ?? 0);
                    result.Sale.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    result.Sale.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }

                var departmentFind = _departmentRepository.FindById(orderFind.DepartmentId ?? 0, orderFind.TradingProviderId);
                if (departmentFind != null)
                {
                    result.DepartmentName = departmentFind.DepartmentName;
                }
                else
                {
                    _logger.LogError($"Không tìm thấy phòng giao dịch quản lý hợp đồng: departmentId = {orderFind.DepartmentId}, tradingProviderId = {orderFind.TradingProviderId}");
                }

                var departmentOfSaleFind = _departmentRepository.FindBySaleId(saleFind.SaleId, orderFind.TradingProviderId);
                if (departmentOfSaleFind != null)
                {
                    result.ManagerDepartmentName = departmentOfSaleFind.DepartmentName;
                }
                else
                {
                    _logger.LogError($"Không tìm thấy phòng giao dịch quản lý hợp đồng: saleId = {saleFind.SaleId}, tradingProviderId = {orderFind.TradingProviderId}");
                }
            }
            return result;
        }

        public int Update(UpdateOrderDto input, int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderFind = _orderRepository.FindById(orderId, tradingProviderId);
            if (orderFind != null)
            {
                var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                if (cifCodeFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy mã cif của khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }

                if (cifCodeFind.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        if (investorFind.ReferralCodeSelf == input.SaleReferralCode)
                        {
                            throw new FaultException(new FaultReason($"Mã giới thiệu đang trùng với khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                        }
                    }
                }
            }

            return _orderRepository.Update(new CompanySharesEntities.DataEntities.Order
            {
                Id = orderId,
                TradingProviderId = tradingProviderId,
                ModifiedBy = username,
                TotalValue = input.TotalValue,
                IsInterest = input.IsInterest,
                CpsPolicyDetailId = input.PolicyDetailId,
                SaleReferralCode = input.SaleReferralCode,
                InvestorBankAccId = input.InvestorBankAccId,
                ContractAddressId = input.ContractAddressId,
            });
        }

        //public ProfitAndInterestDto GetProfitInfo(int orderId)
        //{
        //    //Truyền TradingProvierId
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

        //    #region Lấy Id từ các bảng
        //    //lấy thông tin lệnh
        //    var order = _orderRepository.FindById(orderId, tradingProviderId);
        //    if (order == null)
        //    {
        //        throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
        //    }

        //    var cifFind = _cifCodeRepository.GetByCifCode(order.CifCode);
        //    if (cifFind == null)
        //    {
        //        throw new FaultException(new FaultReason($"Không tìm thấy mã cif"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
        //    }
        //    bool khachCaNhan = cifFind.InvestorId != null;
        //    #region chờ repo cps
        //    ////lấy thông tin lô
        //    //var bondInfo = _productBondInfoRepository.FindById(order.BondId ?? 0);
        //    //if (bondInfo == null)
        //    //{
        //    //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
        //    //}

        //    ////Lấy thông tin bán theo kỳ hạn
        //    //var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(order.Id ?? 0, tradingProviderId);
        //    //if (bondSecondary == null)
        //    //{
        //    //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
        //    //}

        //    ////Lấy thông tin chính sách
        //    //var bondPolicy = _productBondSecondaryRepository.FindPolicyById(order.Id ?? 0, tradingProviderId);
        //    //if (bondPolicy == null)
        //    //{
        //    //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
        //    //}

        //    ////Lấy thông tin chính sách
        //    //var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(order.PolicyDetailId ?? 0, tradingProviderId);
        //    //if (bondPolicyDetail == null)
        //    //{
        //    //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
        //    //}
        //    #endregion
        //    #endregion

        //    //tính lợi tức
        //    DateTime ngayBatDauTinhLai = DateTime.Now.Date;
        //    if (order.PaymentFullDate != null)
        //    {
        //        ngayBatDauTinhLai = order.PaymentFullDate.Value;
        //    }
        //    //ngayBatDauTinhLai = _bondSharedService.NextWorkDay(ngayBatDauTinhLai); //kiểm tra nếu trùng ngày nghỉ cộng lên

        //    //Tổng giá trái phiếu giao dịch (số tiền đầu tư)
        //    decimal soTienDauTu = order.TotalValue ?? 0;

        //    //var result = _bondSharedService.CalculateListInterest(bondInfo, bondPolicy, bondPolicyDetail, ngayBatDauTinhLai, soTienDauTu, khachCaNhan);
        //    return null;
        //}


        //public ProductBondSecondPriceDto FindPriceByDate(int bondSecondaryId, DateTime priceDate)
        //{
        //    int? tradingProviderId = null;
        //    var usertype = CommonUtils.GetCurrentUserType(_httpContext);
        //    if (usertype != UserData.PARTNER || usertype == UserData.ROOT_PARTNER || usertype == UserData.EPIC)
        //    {
        //        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    }
        //    var priceFind = _productBondSecondPriceRepository.FindByDate(bondSecondaryId, priceDate, tradingProviderId);
        //    return _mapper.Map<ProductBondSecondPriceDto>(priceFind);
        //}

        //public int UpdateInvestorBankAccount(int orderId, int? investorBankAccId)
        //{
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    var userName = CommonUtils.GetCurrentUsername(_httpContext);
        //    return _orderRepository.UpdateInvestorBankAccount(orderId, investorBankAccId, tradingProviderId, userName);
        //}

        public void CheckOrder(CheckOrderAppDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _orderRepository.InvestorAdd(input, investorId, true);
        }

        /// <summary>
        /// Tự đặt lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OrderAppDto InvestorAdd(CreateOrderAppDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            OrderAppDto order = InvestorAddCommon(input, investorId, true);
            return order;
        }

        /// <summary>
        /// Sale đặt hộ investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OrderAppDto SaleAddInvestorOrder(SaleInvestorAddOrderDto input)
        {
            var dto = _mapper.Map<CreateOrderAppDto>(input);

            OrderAppDto order = InvestorAddCommon(dto, input.InvestorId, false);
            return order;
        }

        /// <summary>
        /// Hàm đặt lệnh dùng chung
        /// </summary>
        /// <param name="input"></param>
        /// <param name="investorId"></param>
        /// <param name="isSelfDoing">Investor tự đặt hay sale đặt hộ</param>
        /// <returns></returns>
        public OrderAppDto InvestorAddCommon(CreateOrderAppDto input, int investorId, bool isSelfDoing)
        {
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            OrderAppDto order = null;

            int? saleId = null;
            if (!isSelfDoing)
            {
                saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            }

            try
            {
                if (input.IsReceiveContract && input.TranAddess == null)
                {
                    throw new FaultException(new FaultReason($"Địa chỉ giao dịch không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
                order = _orderRepository.InvestorAdd(input, investorId, false, input.OTP, ipAddress, isSelfDoing, saleId);
                _httpContext.HttpContext.Session.SetInt32(SessionKeys.PIN_FAIL_COUNT, 0);
            }
            catch (Exception ex)
            {
                var fex = ex as FaultException;
                if (fex != null)
                {
                    if (int.Parse(fex.Code.Name) == (int)ErrorCode.InvestorInvalidPin)
                    {
                        _investorRepository.HandleInvalidPin(_httpContext, _sysVarRepository, _userRepository);
                    }
                }
                throw;
            }
            _backgroundJobs.Enqueue(() => _cpsBackgroundJobService.UpdateContractFileApp(order.OrderId, order.TradingProviderId));
            //var rocketChatTask = _rocketChatServices.AddCurrentUserToTradingProviderChannel(order.TradingProviderId);
            //await Task.WhenAll(rocketChatTask);
            return order;
        }

        public List<InvestorBankAccountDto> FindAllListBank()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(investorId);
            return _mapper.Map<List<InvestorBankAccountDto>>(investorFindBank);
        }

        public List<BankSupportDto> GetListBankSupport(string keyword)
        {
            var bankSupport = _bankRepository.GetListBankSupport(keyword);
            return _mapper.Map<List<BankSupportDto>>(bankSupport);
        }

        #region update hợp đồng
        public int UpdateOrderContractFileScan(UpdateOrderContractFileDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderContractFileRepository = _orderContractFileRepository.FindById(input.OrderContractFileId);
            if (orderContractFileRepository == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng: {input.OrderContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
            }
            orderContractFileRepository.FileScanUrl = input.FileScanUrl;
            return _orderContractFileRepository.Update(orderContractFileRepository);
        }

        public CompanySharesEntities.DataEntities.OrderContractFile CreateOrderContractFileScan(CreateOrderContractFileDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderContractFile = new CompanySharesEntities.DataEntities.OrderContractFile
            {
                ContractTempId = input.ContractTempId,
                FileScanUrl = input.FileScanUrl,
                OrderId = input.OrderId,
                TradingProviderId = tradingProviderId
            };
            return _orderContractFileRepository.Add(orderContractFile);
        }

        /// <summary>
        /// Update hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task UpdateContractFile(int orderId)
        {
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.CpsOrderNotFound).ToString()), "");
            }
            var productBondPolicy = _cpsSecondaryRepository.FindPolicyById(order.CpsPolicyId ?? 0, tradingProviderId);
            if (productBondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.CpsPolicyId}"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = _contractTemplateServices.FindAllByOrder(-1, 1, null, orderId, tradingProviderId);
            if (contractTemplate == null)
            {
                _logger.LogError($"Không tìm thấy danh sách hợp đồng mẫu: orderId = {orderId}, tradingProviderId = {tradingProviderId}");
                return;
            }
            foreach (var contract in contractTemplate.Items)
            {
                //Fill hợp đồng và lưu trữ
                var saveFile = await _contractDataServices.SaveContract(orderId, contract.Id, contract.IsSign);
                var saveFileApp = new SaveFileDto();
                if (contract.IsSign == IsSignPdf.No || contract.IsSign == null)
                {
                    // cập nhật trên web thì không ký điện tử
                    //saveFileApp = await _contractDataServices.SaveContract(orderId, contract.Id, order.PolicyDetailId ?? 0);
                    var filePathSignOld = saveFile.FilePathToBeDeleted.Replace( ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_DOCX);
                    if (File.Exists(filePathSignOld)) //xóa file word app sinh tạm khi convert sang pdf, file có con dấu
                    {
                        File.Delete(filePathSignOld);
                    }
                }
                if (contract.OrderContractFileId != null)
                {
                    var orderContractFile = _orderContractFileRepository.FindById(contract.OrderContractFileId ?? 0);
                    if (orderContractFile?.FileTempUrl != null)
                    {                   
                        var fileResult = FileUtils.GetPhysicalPathNoCheckExists(orderContractFile.FileTempUrl, filePath);
                        if (File.Exists(fileResult.FullPath))
                        {
                            File.Delete(fileResult.FullPath);
                        }
                    }
                    if (saveFileApp?.FileSignatureUrl != null && orderContractFile?.FileSignatureUrl != null)
                    {
                        //xóa file pdf không có con dấu
                        var fileResult = FileUtils.GetPhysicalPathNoCheckExists(orderContractFile.FileSignatureUrl, filePath);
                        if (File.Exists(fileResult.FullPath))
                        {
                            File.Delete(fileResult.FullPath);
                        }

                        //xóa file pdf có con dấu
                        var FullSignPath = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
                        if (File.Exists(FullSignPath))
                        {
                            File.Delete(FullSignPath);
                        }
                    }

                    //Lưu đường dẫn vào bảng Bond Secondary Contract  
                    orderContractFile.FileTempUrl = saveFile.FileTempUrl;
                    orderContractFile.FileSignatureUrl = saveFile?.FileSignatureUrl ?? orderContractFile.FileSignatureUrl;
                    orderContractFile.PageSign = saveFile?.PageSign ?? orderContractFile.PageSign;
                    _orderContractFileRepository.Update(orderContractFile);
                }
                else
                {
                    //Lưu đường dẫn vào bảng Bond Secondary Contract
                    var orderContractFile = new CompanySharesEntities.DataEntities.OrderContractFile
                    {
                        ContractTempId = contract.Id,
                        FileTempUrl = saveFile.FileTempUrl,
                        FileSignatureUrl = saveFile?.FileSignatureUrl,
                        OrderId = orderId,
                        TradingProviderId = tradingProviderId,
                        PageSign = saveFile?.PageSign ?? 1
                    };
                    _orderContractFileRepository.Add(orderContractFile);
                }
            }
        }

        /// <summary>
        /// Ký điện tử
        /// </summary>
        /// <param name="orderId"></param>
        public void UpdateContractFileSignPdf(int orderId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.CpsOrderNotFound).ToString()), "");
            }
            if (order.Status != OrderStatus.CHO_DUYET_HOP_DONG && order.Status != OrderStatus.DANG_DAU_TU)
            {
                throw new FaultException(new FaultReason($"Hợp đồng chưa thuộc trạng thái được phép ký chữ ký điện tử: {order.CpsPolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var policy = _cpsSecondaryRepository.FindPolicyById(order.CpsPolicyId ?? 0, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.CpsPolicyId}"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = _contractTemplateServices.FindAllByOrder(-1, 1, null, orderId, tradingProviderId);

            if (contractTemplate != null)
            {
                foreach (var contract in contractTemplate.Items)
                {
                    if (contract.IsSign == IsSignPdf.No && contract.OrderContractFileId != null)
                    {
                        var saveFileApp = _contractDataServices.SignContractPdf(contract.OrderContractFileId ?? 0, contract.Id, tradingProviderId);
                        var orderContractFile = _orderContractFileRepository.FindById(contract.OrderContractFileId ?? 0);
                        if (orderContractFile.FileSignatureUrl != null)
                        {
                            string filePath = _fileConfig.Value.Path;

                            //xóa file pdf đã ký
                            var fileResult = FileUtils.GetPhysicalPathNoCheckExists(orderContractFile.FileSignatureUrl, filePath);
                            if (File.Exists(fileResult.FullPath))
                            {
                                File.Delete(fileResult.FullPath);
                            }
                            //xóa file pdf đã ký, có con dấu
                            var filePdfTemp = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
                            if (File.Exists(filePdfTemp))
                            {
                                File.Delete(filePdfTemp);
                            }
                        }
                        //Lưu đường dẫn vào bảng Bond Secondary Contract  
                        orderContractFile.FileSignatureUrl = saveFileApp.FileSignatureUrl;
                        orderContractFile.IsSign = IsSignPdf.Yes;
                        _orderContractFileRepository.Update(orderContractFile);
                    }
                }
            }
        }
        #endregion

        #region giao nhận hđ
        //public int ChangeDeliveryStatusDelivered(int orderId)
        //{
        //    var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    return _orderRepository.ChangeDeliveryStatusDelivered(orderId, tradingProviderId, modifiedBy);
        //}
        //public int ChangeDeliveryStatusReceived(int orderId)
        //{
        //    var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    return _orderRepository.ChangeDeliveryStatusReceived(orderId, tradingProviderId, modifiedBy);
        //}
        //public int ChangeDeliveryStatusDone(int orderId)
        //{
        //    var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    return _orderRepository.ChangeDeliveryStatusDone(orderId, tradingProviderId, modifiedBy);
        //}
        //public int ChangeDeliveryStatusRecevired(string deliveryCode)
        //{
        //    var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
        //    var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
        //    return _orderRepository.ChangeDeliveryStatusReceviredApp(deliveryCode, investorId, modifiedBy);
        //}

        //public void VerifyPhone(string deliveryCode, string phone, int tradingProviderId)
        //{
        //    var phoneDto = _orderRepository.GetPhoneByDeliveryCode(deliveryCode);
        //    if (phoneDto.Phone == phone && phoneDto.DeliveryStatus == DeliveryStatus.DELIVERY)
        //    {
        //        _investorRepository.GenerateOtpByPhone(phone);
        //    }
        //    else
        //    {
        //        throw new FaultException(new FaultReason($"Xác nhận số điện thoại không chính xác"), new FaultCode(((int)ErrorCode.InvestorOTPExpire).ToString()), "");
        //    }
        //}
        #endregion

        ///// <summary>
        ///// App lấy theo trạng thái đang đầu tư
        ///// </summary>
        ///// <returns></returns>
        ///// <exception cref="FaultException"></exception>
        //public List<AppOrderInvestorDto> AppOrderGetAll(int groupStatus)
        //{
        //    var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
        //    var findOrderList = _orderRepository.AppGetAll(investorId, groupStatus);

        //    DateTime ngayHienTai = DateTime.Now.Date;
        //    var result = new List<AppOrderInvestorDto>();
        //    foreach (var item in findOrderList)
        //    {
        //        try
        //        {
        //            var order = _orderRepository.FindById(item.Id, null);
        //            var bondInfo = _productBondInfoRepository.FindById(order.BondId ?? 0);
        //            if (bondInfo == null)
        //            {
        //                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô trái phiếu"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
        //            }

        //            var issuerFind = _issuerRepository.FindById(bondInfo.IssuerId);
        //            if (issuerFind != null)
        //            {
        //                item.IconIssuer = issuerFind.Image;
        //            }
        //            item.BondCode = bondInfo.BondCode;

        //            var policyFind = _productBondSecondaryRepository.FindPolicyById(order.Id ?? 0, order.TradingProviderId, IsShowApp.YES);
        //            if (policyFind == null)
        //            {
        //                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
        //            }
        //            item.PolicyName = policyFind.Name;

        //            var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(order.PolicyDetailId ?? 0, order.TradingProviderId, IsShowApp.YES);
        //            if (policyDetailFind == null)
        //            {
        //                throw new FaultException(new FaultReason($"Không tìm thấy thông kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
        //            }

        //            item.Interest = policyDetailFind.Profit;
        //            item.InterestPeriodType = policyDetailFind.InterestPeriodType;
        //            item.Profit = 0;
        //            //Nếu là đang đầu tư, tính xem số ngày còn lại
        //            if (item.Status == OrderStatus.DANG_DAU_TU)
        //            {
        //                DateTime ngayDaoHan = _bondSharedService.CalculateDueDate(policyDetailFind, item.PaymentFullDate.Value);
        //                item.TimeRemaining = ngayDaoHan.Subtract(ngayHienTai).Days;
        //                if (item.TimeRemaining < 0)
        //                {
        //                    item.TimeRemaining = 0;
        //                }
        //                var profitNow = _bondSharedService.ProfitNow(item.PaymentFullDate ?? DateTime.Now.Date, ngayDaoHan, policyDetailFind.Profit ?? 0, item.TotalValue ?? 0);
        //                item.Profit = profitNow;
        //            }
        //            result.Add(item);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"Không tìm thấy thông tin sổ lệnh {item.Id} ");
        //        }
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <returns></returns>
        ///// <exception cref="FaultException"></exception>
        //public AppOrderInvestorDetailDto AppOrderInvestorDetail(int orderId)
        //{
        //    var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
        //    return _bondOrderShareServices.ViewOrderDetail(orderId, investorId);
        //}

        public AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyDetailId)
        {
            //var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);
            //if (policyDetailFind == null)
            //{
            //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            //}
            //return _mapper.Map<AppSaleByReferralCodeDto>(_saleRepository.FindSaleByReferralCode(referralCode, policyDetailFind.TradingProviderId));
            return null;
        }

        //public PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode)
        //{
        //    var result = _orderRepository.GetPhoneByDeliveryCode(deliveryCode);
        //    var lastThreeChar = result.Phone.Substring(result.Phone.Length - 3);
        //    var firstChar = new string('*', result.Phone.Length - 3);
        //    result.Phone = firstChar + lastThreeChar;
        //    return result;
        //}

        //public decimal ChangeDeliveryStatusRecevired(string deliveryCode, string otp)
        //{
        //    var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
        //    decimal verifyOtpResult = _orderRepository.ChangeDeliveryStatusRecevired(deliveryCode, otp, modifiedBy);
        //    if (verifyOtpResult == TrueOrFalseNum.FALSE)
        //    {
        //        throw new FaultException(new FaultReason($"OTP không hợp lệ"), new FaultCode(((int)ErrorCode.InvestorOTPInvalid).ToString()), "");
        //    }
        //    else if (verifyOtpResult == TrueOrFalseNum.EXPIRE)
        //    {
        //        throw new FaultException(new FaultReason($"OTP hết hạn"), new FaultCode(((int)ErrorCode.InvestorOTPExpire).ToString()), "");
        //    }
        //    return verifyOtpResult;
        //}

        //public InterestCalculationDateDto CheckInvestmentDay(int policyDetailId, DateTime ngaybatdau)
        //{
        //    var result = new InterestCalculationDateDto();
        //    var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);

        //    var ngayDauTu = _bondSharedService.NextWorkDay(ngaybatdau, policyDetailFind.TradingProviderId);

        //    var ngayKetThuc = _bondSharedService.CalculateDueDate(policyDetailFind, ngayDauTu);
        //    result.StartDate = ngayDauTu;
        //    result.EndDate = ngayKetThuc;
        //    return result;
        //}

        /// <summary>
        /// Saler thỏa mãn thông tin trước khi đặt lệnh thì sẽ trả ra thông tin
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="secondaryId"></param>
        /// <returns></returns>
        public ViewCheckSaleBeforeAddOrderDto CheckSaleBeforeAddOrder(string referralCode, int secondaryId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var saleInfo = _orderRepository.AppCheckSaleBeforeAddOrder(referralCode, secondaryId, investorId);

            return saleInfo;
        }

        ///// <summary>
        ///// Đổ và xử lý thông tin dữ liệu chi trả
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="ngayBatDau"></param>
        ///// <param name="ngayKetThuc"></param>
        ///// <returns></returns>
        //public List<ThoiGianChiTraThucDto> LayDanhSachNgayChiTra(DanhSachChiTraFitlerDto input)
        //{
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    var listOrder = _orderRepository.FindOrderByTradingProviderId(tradingProviderId, input.ContractCode, input.TaxCode, input.Phone);
        //    var danhSachChiTra = new List<ThoiGianChiTraThucDto>();
        //    Dictionary<long, ThoiGianChiTraThucDto> dictThoiGianChiTra = new();
        //    var listThoiGianChiTra = new List<ThoiGianChiTraThucDto>();
        //    var result = new List<ThoiGianChiTraThucDto>();

        //    foreach (var orderItem in listOrder)
        //    {
        //        //Lấy kỳ hạn
        //        var policyDetailFind = _productBondSecondaryRepository.GetPolicyDetailByIdWithoutTradingProviderId(orderItem.PolicyDetailId ?? 0, false);
        //        List<DateTime> thoiGianThuc = new();

        //        //Lấy ngày bắt đầu tính lãi
        //        var ngayDauKy = orderItem.InvestDate.Value.Date;

        //        //Tính ngày đáo hạn
        //        var ngayDaoHan = _orderRepository.CalculateDueDate(policyDetailFind, ngayDauKy, false);

        //        //bắt đầu dầu tính khoảng thời gian của các kì từ ngày bắt đầu tới ngày đáo hạn
        //        if (policyDetailFind.InterestType == InterestType.DINH_KY)
        //        {
        //            //Tính thời gian thực của kỳ trả
        //            while (ngayDauKy <= ngayDaoHan)
        //            {
        //                //set ngày cuối kỳ, mặc định ngày cuối kì sẽ được tính bằng cộng ngày
        //                DateTime ngayCuoiKy = ngayDauKy.AddDays(policyDetailFind.InterestPeriodQuantity ?? 0);

        //                //nếu đơn vị kỳ hạn là tháng thì tính ngày cuối kì bằng cách cộng tháng
        //                if (policyDetailFind.InterestPeriodType == PeriodUnit.MONTH)
        //                {
        //                    ngayCuoiKy = ngayDauKy.AddMonths(policyDetailFind.InterestPeriodQuantity ?? 0);
        //                }
        //                //nếu đơn vị kỳ hạn là năm, thì tính ngày cuối kì bằng cách cộng năm
        //                else if (policyDetailFind.InterestPeriodType == PeriodUnit.YEAR)
        //                {
        //                    ngayCuoiKy = ngayDauKy.AddYears(policyDetailFind.InterestPeriodQuantity ?? 0);
        //                }

        //                //Chuyển đến kỳ tiếp theo
        //                ngayDauKy = ngayCuoiKy;

        //                var ngayLamViec = _orderRepository.NextWorkDay(ngayCuoiKy.Date, policyDetailFind.TradingProviderId, false);

        //                //nếu ngày làm việc vượt quá ngày đáo hạn thì set ngày làm việc = ngày đáo hạn
        //                if (ngayLamViec > ngayDaoHan)
        //                {
        //                    ngayLamViec = ngayDaoHan;
        //                }

        //                //trường hợp cộng thừa một tý 
        //                if (thoiGianThuc.Count > 1 && ngayLamViec == thoiGianThuc[^1]) //trong trường hợp là phần tử cuối cùng trong thời gian thực
        //                {
        //                    break;
        //                }
        //                thoiGianThuc.Add(ngayLamViec);
        //            };

        //            //nếu đây là cuối kì thì
        //            if (thoiGianThuc.Count > 0 && thoiGianThuc[^1] < ngayDaoHan)
        //            {
        //                thoiGianThuc[^1] = ngayDaoHan;
        //            }

        //            //Thời gian thực tính tiền...
        //            for (int j = 0; j < thoiGianThuc.Count; j++)
        //            {
        //                int soNgay;
        //                bool isLastPeriod = j == thoiGianThuc.Count - 1;
        //                if (j == 0) //kỳ trả đầu tiên
        //                {
        //                    soNgay = (thoiGianThuc[j] - orderItem.InvestDate.Value.Date).Days;
        //                }
        //                else
        //                {
        //                    soNgay = (thoiGianThuc[j] - thoiGianThuc[j - 1]).Days;
        //                }

        //                listThoiGianChiTra.Add(new ThoiGianChiTraThucDto
        //                {
        //                    PayDate = thoiGianThuc[j],
        //                    LastPayDate = j > 0 ? thoiGianThuc[j - 1] : thoiGianThuc[j],
        //                    PolicyDetailId = policyDetailFind.PolicyDetailId,
        //                    Id = orderItem.Id,
        //                    CifCode = orderItem.CifCode,
        //                    PeriodIndex = j + 1,
        //                    SoNgay = soNgay,
        //                    IsLastPeriod = isLastPeriod
        //                });
        //            }
        //        }
        //        else if (policyDetailFind.InterestType == InterestType.CUOI_KY) //Trả cuối kỳ
        //        {
        //            int soNgay = ngayDaoHan.Subtract(ngayDauKy.Date).Days;

        //            listThoiGianChiTra.Add(new ThoiGianChiTraThucDto
        //            {
        //                PayDate = ngayDaoHan,
        //                LastPayDate = ngayDaoHan,
        //                PolicyDetailId = policyDetailFind.PolicyDetailId,
        //                Id = orderItem.Id,
        //                CifCode = orderItem.CifCode,
        //                PeriodIndex = 1,
        //                SoNgay = soNgay
        //            });
        //        }
        //        //danh sách các ngày đã trả trong interestpayment của order id này 
        //        var listOrderDaChiTra = _interestPaymentRepository.GetListInterestPaymentByOrderId(orderItem.Id, orderItem.TradingProviderId).Select(x => x.PeriodIndex).ToList();

        //        //lọc và loại các kì hạn đã chi trả lấy những kì hạn chưa chi trả
        //        listThoiGianChiTra = listThoiGianChiTra.Where(e => !listOrderDaChiTra.Contains(e.PeriodIndex)).ToList();
        //        result.AddRange(listThoiGianChiTra);
        //    }
        //    _orderRepository.CloseConnection();

        //    var list = new List<ThoiGianChiTraThucDto>();

        //    if (input.NgayChiTra != null)
        //    {
        //        list = result.Where(e => e.PayDate <= input.NgayChiTra).OrderByDescending(r => r.PayDate.Date).ToList();
        //    }
        //    else
        //    {
        //        list = result.OrderByDescending(r => r.PayDate.Date).ToList();
        //    }
        //    return list;
        //}

        //public PagingResult<DanhSachChiTraDto> LapDanhSachChiTra(DanhSachChiTraFitlerDto input)
        //{
        //    var lapDanhSachChiTra = new List<DanhSachChiTraDto>();
        //    var result = new PagingResult<DanhSachChiTraDto>();
        //    var danhSachNgayChiTra = LayDanhSachNgayChiTra(input);
        //    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

        //    danhSachNgayChiTra = danhSachNgayChiTra.Skip(input.PageSize * (input.PageNumber - 1)).Take(input.PageSize).ToList();
        //    foreach (var item in danhSachNgayChiTra)
        //    {
        //        string messagerError = "";
        //        var orderFind = _orderRepository.FindById(item.Id, tradingProviderId);
        //        var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(item.PolicyDetailId);

        //        var policyFind = _productBondSecondaryRepository.FindPolicyById(policyDetailFind.Id, policyDetailFind.TradingProviderId);

        //        //Tính tổng số tiền đã được chi trả
        //        var soTienDaChiTra = _orderRepository.InterestPaymentSumMoney(item.PayDate, item.Id, orderFind.TradingProviderId);

        //        decimal? loiTucKyNay = 0;
        //        decimal? thue = 0;
        //        decimal? tongTienThucNhan = 0;
        //        decimal thueLoiNhuan = 0;
        //        string name = null;
        //        var cifCodeFind = new CifCodes();
        //        try
        //        {
        //            cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
        //        }
        //        catch
        //        {
        //            cifCodeFind = null;
        //        }

        //        if (cifCodeFind != null && cifCodeFind.InvestorId != null)
        //        {
        //            thueLoiNhuan = (policyFind.IncomeTax ?? 0) / 100;
        //            var investerIdentification = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);
        //            if (investerIdentification != null)
        //                name = investerIdentification.Fullname;
        //        }
        //        else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
        //        {
        //            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
        //            if (businessCustomer != null)
        //            {
        //                name = businessCustomer.Name;
        //            }
        //        }
        //        if (item.IsLastPeriod)
        //        {
        //            loiTucKyNay = (orderFind.TotalValue * (policyDetailFind.Profit / 100) * item.SoNgay / 365) - soTienDaChiTra;
        //            thue = loiTucKyNay * (policyFind.IncomeTax ?? 0) / 100;
        //            var listCoupon = new List<CouponInfoDto>();
        //            try
        //            {
        //                listCoupon = _bondSharedService.GetListCoupon(orderFind.BondId ?? 0, orderFind.Id ?? 0, orderFind.TotalValue ?? 0, orderFind.InvestDate ?? default, item.PayDate, thue ?? 0, tradingProviderId);
        //            }
        //            catch (Exception ex)
        //            {
        //                messagerError = $"{ex.Message}";
        //                listCoupon = new List<CouponInfoDto>();
        //            }

        //            decimal sumListCoupon = listCoupon.Sum(e => e.ActuallyCoupon);
        //            tongTienThucNhan = loiTucKyNay - thue - sumListCoupon;

        //        }
        //        else
        //        {
        //            loiTucKyNay = (orderFind.TotalValue * (policyDetailFind.Profit / 100) * item.SoNgay / 365) - soTienDaChiTra;
        //            thue = loiTucKyNay * (policyFind.IncomeTax ?? 0) / 100;
        //            tongTienThucNhan = loiTucKyNay - thue;
        //        }

        //        lapDanhSachChiTra.Add(new DanhSachChiTraDto
        //        {
        //            PayDate = item.PayDate,
        //            PolicyDetailId = item.PolicyDetailId,
        //            Id = item.Id,
        //            CifCode = orderFind.CifCode,
        //            PeriodIndex = item.PeriodIndex,
        //            Profit = Math.Floor(loiTucKyNay ?? 0),
        //            AmountMoney = Math.Floor(tongTienThucNhan ?? 0),
        //            ActuallyProfit = Math.Floor(tongTienThucNhan ?? 0),
        //            Tax = Math.Floor(thue ?? 0),
        //            SoNgay = item.SoNgay,
        //            BondOrder = orderFind,
        //            PolicyDetail = policyDetailFind,
        //            ContractCode = orderFind.ContractCode,
        //            Name = name,
        //            Message = messagerError
        //        });
        //    }
        //    result.Items = _mapper.Map<List<DanhSachChiTraDto>>(lapDanhSachChiTra);
        //    result.TotalItems = result.Items.Count();
        //    return result;
        //}
    }
}
