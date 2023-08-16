using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.AppOrder;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.BondEntities.Dto.BondOrder;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.BondEntities.Dto.SaleInvestor;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Bank;
using EPIC.Entities.Dto.BondSecondaryContract;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.OrderPayment;
using EPIC.Entities.Dto.ProductBondPrimary;
using EPIC.Entities.Dto.ProductBondSecondPrice;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.IdentityRepositories;
using EPIC.RocketchatDomain.Interfaces;
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

namespace EPIC.BondDomain.Implements
{
    public class BondOrderService : IBondOrderService
    {
        private readonly ILogger<BondOrderService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BondOrderRepository _orderRepository;
        private readonly BondOrderPaymentRepository _orderPaymentRepository;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        private readonly BondCalendarRepository _calendarRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BankRepository _bankRepository;
        private readonly AuthOtpEFRepository _authOtpEfRepository;
        private readonly BondContractTemplateRepository _contractTemplateRepository;
        private readonly BondSecondaryContractRepository _bondSecondaryContractRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly IBondOrderShareService _bondOrderShareServices;
        private readonly SaleRepository _saleRepository;
        private readonly BondRenewalsRequestRepository _renewalsRequestRepository;
        private readonly IMapper _mapper;
        private readonly IBondSharedService _bondSharedService;
        private readonly IBondContractDataService _contractDataServices;
        private readonly IBondContractTemplateService _contractTemplateServices;
        private readonly IRocketChatServices _rocketChatServices;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly UserRepository _userRepository;
        private readonly SysVarRepository _sysVarRepository;
        private readonly BondBlockadeLiberationRepository _blockadeLiberationRepository;
        private readonly BondInterestPaymentRepository _interestPaymentRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly BondBackgroundJobService _bondBackgroundJobService;
        //private readonly EpicSchemaDbContext _epicSchemaDbContext;


        public BondOrderService(
            ILogger<BondOrderService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IBondSharedService bondSharedService,
            IBondContractDataService contractDataServices,
            IBondContractTemplateService contractTemplateServices,
            IRocketChatServices rocketChatServices,
            IBackgroundJobClient backgroundJobs,
            BondBackgroundJobService bondBackgroundJobService,
            IBondOrderShareService bondOrderShareServices,
            EpicSchemaDbContext dbContext,
            IOptions<FileConfig> fileConfig)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _orderPaymentRepository = new BondOrderPaymentRepository(_connectionString, _logger);
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, _logger);
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            _calendarRepository = new BondCalendarRepository(_connectionString, _logger);
            _authOtpEfRepository = new AuthOtpEFRepository(dbContext, logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _renewalsRequestRepository = new BondRenewalsRequestRepository(_connectionString, _logger);
            _contractTemplateRepository = new BondContractTemplateRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _distributionContractRepository = new BondDistributionContractRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _bondOrderShareServices = bondOrderShareServices;
            _contractDataServices = contractDataServices;
            _mapper = mapper;
            _bondSharedService = bondSharedService;
            _contractTemplateServices = contractTemplateServices;
            _rocketChatServices = rocketChatServices;
            _fileConfig = fileConfig;
            _backgroundJobs = backgroundJobs;
            _bondBackgroundJobService = bondBackgroundJobService;
            _userRepository = new UserRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            _blockadeLiberationRepository = new BondBlockadeLiberationRepository(_connectionString, _logger);
            _interestPaymentRepository = new BondInterestPaymentRepository(_connectionString, _logger);
        }

        public async Task<BondOrder> Add(CreateOrderDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = new BondOrder()
            {
                TradingProviderId = tradingProviderId,
                CifCode = input.CifCode,
                SecondaryId = input.SecondaryId,
                PolicyId = input.PolicyId,
                PolicyDetailId = input.PolicyDetailId,
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
                    order.InvestorIdenId = investorIdentificationFind.Id;
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
                        order.ContractAddressId = investorContractAddressFind.ContactAddressId;
                    }
                }
                else
                {
                    order.ContractAddressId = input.ContractAddressId;
                }
            }
            var result = _orderRepository.Add(order);
            await UpdateContractFile((int)result.Id);
            return result;
        }

        protected void UpdateContractFileOrderAdd(int orderId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }
            var productBondPolicy = _productBondSecondaryRepository.FindPolicyById(order.PolicyId, tradingProviderId);
            if (productBondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
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
                var saveFile = _contractDataServices.SaveContract(orderId, contract.Id);
                //Lưu đường dẫn vào bảng Bond Secondary Contract
                var bondSecondaryContract = new BondSecondaryContract
                {
                    ContractTempId = contract.Id,
                    FileTempUrl = saveFile.FileTempUrl,
                    OrderId = orderId,
                    TradingProviderId = tradingProviderId,
                    PageSign = saveFile?.PageSign ?? 1
                };
                _bondSecondaryContractRepository.Add(bondSecondaryContract);

            }
        }

        public BondOrderPayment AddPayment(CreateOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            return _orderPaymentRepository.Add(new BondOrderPayment
            {
                TradingProviderId = tradingProviderId,
                OrderId = input.OrderId,
                TranDate = input.TranDate,
                TranType = input.TranType,
                TranClassify = input.TranClassify,
                PaymentType = input.PaymentType,
                PaymentAmount = input.PaymentAmount,
                Description = input.Description,
                CreatedBy = username
            });
        }

        public int ApprovePayment(int orderPaymentId, int status)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderPaymentRepository.ApprovePayment(orderPaymentId, status, tradingProviderId, username);
        }

        public int Delete(int id)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.Delete(id, tradingProviderId);
        }

        public int DeleteOrderPayment(int id)
        {
            return _orderPaymentRepository.Delete(id);
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

        public int UpdatePolicyDetail(int orderId, int? PolicyDetailId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.UpdatePolicyDetail(orderId, tradingProviderId, PolicyDetailId, username);
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

        public PagingResult<BondOrderPayment> FindAll(int orderId, int pageSize, int pageNumber, string keyword, int? status)
        {
            return _orderPaymentRepository.FindAll(orderId, pageSize, pageNumber, keyword, status);
        }
        public PagingResult<ViewOrderDto> FindAll(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? policyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer)
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
            var orderList = _orderRepository.FindAll(tradingProviderId, taxCode, idNo, cifCode, phone, pageSize, pageNumber, keyword, status, groupStatus, source, bondSecondaryId, bondPolicy, policyDetailId, customerName, contractCode, tradingDate, deliveryStatus, orderer, listTradingChildIds);
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

                var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryById(orderFind.SecondaryId, orderFind.TradingProviderId);

                if (bondSecondaryFind != null)
                {
                    var listPolicies = _productBondSecondaryRepository.GetAllPolicy(bondSecondaryFind.Id, orderFind.TradingProviderId);
                    orderItem.BondPolicies = _mapper.Map<List<ViewProductBondPolicyDto>>(listPolicies);
                }
                var bondInfoFind = _productBondInfoRepository.FindById(orderFind.BondId);
                if (bondInfoFind != null)
                {
                    orderItem.BondInfo = _mapper.Map<ProductBondInfoDto>(bondInfoFind);
                }

                var bondPolicyFind = _productBondSecondaryRepository.FindPolicyById(orderFind.PolicyId, orderFind.TradingProviderId);
                if (bondPolicyFind != null)
                {
                    orderItem.BondPolicy = _mapper.Map<ViewProductBondPolicyDto>(bondPolicyFind);
                }

                var priceFind = _productBondSecondPriceRepository.FindByDate(orderFind.SecondaryId, orderFind.PaymentFullDate ?? DateTime.Now, orderFind.TradingProviderId);
                if (priceFind != null)
                {
                    orderItem.BuyPrice = priceFind.Price;
                }

                var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(orderFind.PolicyDetailId, orderFind.TradingProviderId);
                if (bondPolicyDetail != null)
                {
                    orderItem.BondPolicyDetail = _mapper.Map<ViewProductBondPolicyDetailDto>(bondPolicyDetail);
                }

                var blockadeLiberation = _blockadeLiberationRepository.FindById(orderFind.BlockadeLiberationId, orderFind.TradingProviderId);
                if (blockadeLiberation != null)
                {
                    orderItem.BlockadeLiberationId = blockadeLiberation.Id;
                }

                if (orderFind.Source == SourceOrder.ONLINE)
                {
                    orderFind.OrderSource = SourceOrderFE.KHACH_HANG;
                }
                else if (orderFind.Source == SourceOrder.OFFLINE && orderFind.SaleOrderId != null)
                {
                    orderFind.OrderSource = SourceOrderFE.SALE;
                }
                else if (orderFind.Source == SourceOrder.OFFLINE && orderFind.SaleOrderId == null)
                {
                    orderFind.OrderSource = SourceOrderFE.QUAN_TRI_VIEN;
                }
                items.Add(orderItem);
            }
            result.Items = items;
            return result;
        }

        // get all delivery status cua giao nhan hop dong
        public PagingResult<ViewOrderDto> FindAllDeliveryStatus(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? policyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, DateTime? pendingDate, DateTime? deliveryDate, DateTime? receivedDate, DateTime? finishedDate, DateTime? date)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderList = _orderRepository.FindAllDeliveryStatus(tradingProviderId, taxCode, idNo, cifCode, phone, pageSize, pageNumber, keyword, status, groupStatus, source, bondSecondaryId, bondPolicy, policyDetailId, customerName, contractCode, tradingDate, deliveryStatus, pendingDate, deliveryDate, receivedDate, finishedDate, date);
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

                var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryById(orderFind.SecondaryId, tradingProviderId);

                if (bondSecondaryFind != null)
                {
                    var listPolicies = _productBondSecondaryRepository.GetAllPolicy(bondSecondaryFind.Id, tradingProviderId);
                    orderItem.BondPolicies = _mapper.Map<List<ViewProductBondPolicyDto>>(listPolicies);
                }
                var bondInfoFind = _productBondInfoRepository.FindById(orderFind.BondId);
                if (bondInfoFind != null)
                {
                    orderItem.BondInfo = _mapper.Map<ProductBondInfoDto>(bondInfoFind);
                }

                var bondPolicyFind = _productBondSecondaryRepository.FindPolicyById(orderFind.PolicyId, tradingProviderId);
                if (bondPolicyFind != null)
                {
                    orderItem.BondPolicy = _mapper.Map<ViewProductBondPolicyDto>(bondPolicyFind);
                }

                var priceFind = _productBondSecondPriceRepository.FindByDate(orderFind.SecondaryId, orderFind.PaymentFullDate ?? DateTime.Now, tradingProviderId);
                if (priceFind != null)
                {
                    orderItem.BuyPrice = priceFind.Price;
                }

                var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(orderFind.PolicyDetailId, tradingProviderId);
                if (bondPolicyDetail != null)
                {
                    orderItem.BondPolicyDetail = _mapper.Map<ViewProductBondPolicyDetailDto>(bondPolicyDetail);
                }

                var blockadeLiberation = _blockadeLiberationRepository.FindById(orderFind.BlockadeLiberationId, tradingProviderId);
                if (blockadeLiberation != null)
                {
                    orderItem.BlockadeLiberationId = blockadeLiberation.Id;
                }
                items.Add(orderItem);
            }
            if (pendingDate != null)
            {
                items.OrderByDescending(s => s.PendingDate);
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

            result.IsRenewalsRequest = _renewalsRequestRepository.GetByOrderId(id, orderFind.TradingProviderId) is not null;

            //Tìm kiếm giá 
            var priceFind = _productBondSecondPriceRepository.FindByDate(orderFind.SecondaryId, orderFind.PaymentFullDate ?? DateTime.Now, orderFind.TradingProviderId);
            if (priceFind != null)
            {
                result.BuyPrice = priceFind.Price;
            }

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

            //Lấy thông thi lô trái phiếu
            var bondInfoFind = _productBondInfoRepository.FindById(orderFind.BondId);
            if (bondInfoFind != null)
            {
                result.BondInfo = _mapper.Map<ProductBondInfoDto>(bondInfoFind);
            }

            //Lấy thông tin phát hành sơ cấp
            var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryById(orderFind.SecondaryId, orderFind.TradingProviderId);
            if (bondSecondaryFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin phát hành thứ cấp"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            else if (bondSecondaryFind != null)
            {
                var bondSecondary = _mapper.Map<ViewProductBondSecondaryDto>(bondSecondaryFind);

                bondSecondary.SoLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(orderFind.TradingProviderId, bondSecondaryFind.PrimaryId);
                bondSecondary.SoLuongConLai = bondSecondary.SoLuongTraiPhieuNamGiu - _orderRepository.SumQuantity(tradingProviderId, bondSecondary.Id);
                //Tài khoản thụ hưởng của dlsc muốn nhận khi bán theo kỳ hạn
                result.BusinessCustomerBankAccId = bondSecondaryFind.BusinessCustomerBankAccId;

                var businesBankAccFind = _businessCustomerRepository.FindBusinessCusBankById(bondSecondaryFind.BusinessCustomerBankAccId);

                if (businesBankAccFind != null)
                {
                    result.TradingProviderBank = _mapper.Map<BusinessCustomerBankDto>(businesBankAccFind);
                }
                var bondPrimary = _productBondPrimaryRepository.FindById(bondSecondaryFind.PrimaryId, null);
                if (bondPrimary != null)
                {
                    bondSecondary.ProductBondInfo = _productBondInfoRepository.FindById(bondPrimary.BondId);
                }

                //Lấy thông tin tổ chức phát hành
                bondSecondary.ProductBondPrimary = _mapper.Map<ProductBondPrimaryDto>(bondPrimary);

                //Lấy thông tin chính sách và kỳ hạn
                bondSecondary.Policies = new List<ViewProductBondPolicyDto>();
                var policyList = _productBondSecondaryRepository.GetAllPolicy(bondSecondary.Id, bondSecondary.TradingProviderId);
                foreach (var policyItem in policyList)
                {
                    var policy = _mapper.Map<ViewProductBondPolicyDto>(policyItem);

                    policy.FakeId = policyItem.Id;
                    policy.Details = new List<ViewProductBondPolicyDetailDto>();

                    var policyDetailList = _productBondSecondaryRepository.GetAllPolicyDetail(policy.Id, bondSecondary.TradingProviderId);
                    foreach (var detailItem in policyDetailList)
                    {
                        var detail = _mapper.Map<ViewProductBondPolicyDetailDto>(detailItem);
                        detail.FakeId = detailItem.Id;
                        policy.Details.Add(detail);
                    }
                    bondSecondary.Policies.Add(policy);
                }
                result.BondSecondary = bondSecondary;
            }

            var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(orderFind.PolicyDetailId, orderFind.TradingProviderId);
            if (policyDetailFind != null)
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
                result.EndDate = _bondSharedService.CalculateDueDate(policyDetailFind, ngayDauTu);
            }

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

        public OrderPaymentDto FindPaymentById(int id)
        {
            return _orderPaymentRepository.FindById(id);
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

            return _orderRepository.Update(new BondOrder
            {
                Id = orderId,
                TradingProviderId = tradingProviderId,
                ModifiedBy = username,
                TotalValue = input.TotalValue,
                IsInterest = input.IsInterest,
                PolicyDetailId = input.PolicyDetailId,
                SaleReferralCode = input.SaleReferralCode,
                InvestorBankAccId = input.InvestorBankAccId,
                ContractAddressId = input.ContractAddressId,
            });
        }

        public int Update(UpdateOrderPaymentDto input, int orderPaymentId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderPaymentRepository.Update(new BondOrderPayment
            {
                Id = orderPaymentId,
                OrderId = input.OrderId,
                TradingProviderId = tradingProviderId,
                TranDate = input.TranDate,
                TranType = input.TranType,
                TranClassify = input.TranClassify,
                PaymentType = input.PaymentType,
                PaymentAmount = input.PaymentAmount,
                Description = input.Description,
                ModifiedBy = username
            });
        }

        public ProfitAndInterestDto GetProfitInfo(int orderId)
        {
            //Truyền TradingProvierId
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            #region Lấy Id từ các bảng
            //lấy thông tin lệnh
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }

            var cifFind = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mã cif"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            bool khachCaNhan = cifFind.InvestorId != null;

            //lấy thông tin lô
            var bondInfo = _productBondInfoRepository.FindById(order.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(order.SecondaryId, tradingProviderId);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }

            //Lấy thông tin chính sách
            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(order.PolicyId, tradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin chính sách
            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(order.PolicyDetailId, tradingProviderId);
            if (bondPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            }
            #endregion

            //tính lợi tức
            DateTime ngayBatDauTinhLai = DateTime.Now.Date;
            if (order.PaymentFullDate != null)
            {
                ngayBatDauTinhLai = order.PaymentFullDate.Value;
            }
            ngayBatDauTinhLai = _bondSharedService.NextWorkDay(ngayBatDauTinhLai); //kiểm tra nếu trùng ngày nghỉ cộng lên

            //Tổng giá trái phiếu giao dịch (số tiền đầu tư)
            decimal soTienDauTu = order.TotalValue;

            var result = _bondSharedService.CalculateListInterest(bondInfo, bondPolicy, bondPolicyDetail, ngayBatDauTinhLai, soTienDauTu, khachCaNhan);
            return result;
        }


        public ProductBondSecondPriceDto FindPriceByDate(int bondSecondaryId, DateTime priceDate)
        {
            int? tradingProviderId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype != UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER || usertype == UserTypes.EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var priceFind = _productBondSecondPriceRepository.FindByDate(bondSecondaryId, priceDate, tradingProviderId);
            return _mapper.Map<ProductBondSecondPriceDto>(priceFind);
        }

        public int UpdateInvestorBankAccount(int orderId, int? investorBankAccId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            return _orderRepository.UpdateInvestorBankAccount(orderId, investorBankAccId, tradingProviderId, userName);
        }

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
            var isInvestorActive = _investorEFRepository.IsInvestorActive(investorId);
            if (!isInvestorActive)
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorIdIsNotVerifiedId);
            }

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

                if (isSelfDoing)
                {
                    var userId = CommonUtils.GetCurrentUserId(_httpContext);
                    var maxOtpFailCount = _sysVarRepository.GetInvValueByName("AUTH", "OTP_INVALID_COUNT");

                    _authOtpEfRepository.CheckOtpByUserId(userId, input.OTP, _httpContext, SessionKeys.OTP_FAIL_COUNT, maxOtpFailCount);
                }

                order = _orderRepository.InvestorAdd(input, investorId, false, input.OTP, ipAddress, isSelfDoing, saleId);
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
            _backgroundJobs.Enqueue(() => _bondBackgroundJobService.UpdateContractFileApp(order.OrderId, order.TradingProviderId));
            _rocketChatServices.AddCurrentUserToTradingProviderChannel(order.TradingProviderId);
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

        public int UpdateSecondaryContractFileScan(UpdateBondSecondaryContractDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var bondSecondaryContract = _bondSecondaryContractRepository.FindById(input.SecondaryContractFileId);
            if (bondSecondaryContract == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng: {input.SecondaryContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
            }
            bondSecondaryContract.FileScanUrl = input.FileScanUrl;
            return _bondSecondaryContractRepository.Update(bondSecondaryContract);
        }

        public BondSecondaryContract CreateSecondaryContractFileScan(CreateBondSecondaryContractDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var bondSecondaryContract = new BondSecondaryContract
            {
                ContractTempId = input.ContractTempId,
                FileScanUrl = input.FileScanUrl,
                OrderId = input.OrderId,
                TradingProviderId = tradingProviderId
            };
            return _bondSecondaryContractRepository.Add(bondSecondaryContract);
        }

        public async Task UpdateContractFile(int orderId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }
            var productBondPolicy = _productBondSecondaryRepository.FindPolicyById(order.PolicyId, tradingProviderId);
            if (productBondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
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
                var saveFile = _contractDataServices.SaveContract(orderId, contract.Id);
                var saveFileApp = new SaveFileDto();
                if (contract.IsSign == IsSignPdf.No || contract.IsSign == null)
                {
                    // cập nhật trên web thì không ký điện tử
                    saveFileApp = await _contractDataServices.SaveContractPdfNotSign(orderId, contract.Id, order.PolicyDetailId);
                    if (File.Exists(saveFileApp.FilePathToBeDeleted)) //xóa file word app sinh tạm khi convert sang pdf
                    {
                        File.Delete(saveFileApp.FilePathToBeDeleted);
                    }
                    var filePathSignOld = saveFileApp.FilePathToBeDeleted.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_DOCX);
                    if (File.Exists(filePathSignOld)) //xóa file word app sinh tạm khi convert sang pdf, file có con dấu
                    {
                        File.Delete(filePathSignOld);
                    }
                }
                if (contract.SecondaryContractFileId != null)
                {
                    var bondSecondaryContract = _bondSecondaryContractRepository.FindById(contract.SecondaryContractFileId ?? 0);
                    if (bondSecondaryContract?.FileTempUrl != null)
                    {
                        string filePath = _fileConfig.Value.Path;

                        var fileResult = FileUtils.GetPhysicalPathNoCheckExists(bondSecondaryContract.FileTempUrl, filePath);
                        if (File.Exists(fileResult.FullPath))
                        {
                            File.Delete(fileResult.FullPath);
                        }
                    }
                    if (saveFileApp?.FileSignatureUrl != null && bondSecondaryContract?.FileSignatureUrl != null)
                    {
                        string filePath = _fileConfig.Value.Path;

                        //xóa file pdf không có con dấu
                        var fileResult = FileUtils.GetPhysicalPathNoCheckExists(bondSecondaryContract.FileSignatureUrl, filePath);
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
                    bondSecondaryContract.FileTempUrl = saveFile.FileTempUrl;
                    bondSecondaryContract.FileSignatureUrl = saveFileApp?.FileSignatureUrl ?? bondSecondaryContract.FileSignatureUrl;
                    bondSecondaryContract.PageSign = saveFileApp?.PageSign ?? bondSecondaryContract.PageSign;
                    _bondSecondaryContractRepository.Update(bondSecondaryContract);
                }
                else
                {
                    //Lưu đường dẫn vào bảng Bond Secondary Contract
                    var bondSecondaryContract = new BondSecondaryContract
                    {
                        ContractTempId = contract.Id,
                        FileTempUrl = saveFile.FileTempUrl,
                        FileSignatureUrl = saveFileApp?.FileSignatureUrl,
                        OrderId = orderId,
                        TradingProviderId = tradingProviderId,
                        PageSign = saveFileApp?.PageSign ?? 1
                    };
                    _bondSecondaryContractRepository.Add(bondSecondaryContract);
                }
            }
        }

        public void UpdateContractFileSignPdf(int orderId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }
            if (order.Status != OrderStatus.CHO_DUYET_HOP_DONG && order.Status != OrderStatus.DANG_DAU_TU)
            {
                throw new FaultException(new FaultReason($"Hợp đồng chưa thuộc trạng thái được phép ký chữ ký điện tử: {order.PolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var productBondPolicy = _productBondSecondaryRepository.FindPolicyById(order.PolicyId, tradingProviderId);
            if (productBondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = _contractTemplateServices.FindAllByOrder(-1, 1, null, orderId, tradingProviderId);

            if (contractTemplate != null)
            {
                foreach (var contract in contractTemplate.Items)
                {
                    if (contract.IsSign == IsSignPdf.No && contract.SecondaryContractFileId != null)
                    {
                        var saveFileApp = _contractDataServices.SaveContractSignPdf(contract.SecondaryContractFileId ?? 0, contract.Id, tradingProviderId);
                        var bondSecondaryContract = _bondSecondaryContractRepository.FindById(contract.SecondaryContractFileId ?? 0);
                        if (bondSecondaryContract.FileSignatureUrl != null)
                        {
                            string filePath = _fileConfig.Value.Path;

                            var fileResult = FileUtils.GetPhysicalPathNoCheckExists(bondSecondaryContract.FileSignatureUrl, filePath);
                            if (File.Exists(fileResult.FullPath))
                            {
                                File.Delete(fileResult.FullPath);
                            }

                            var filePdfTemp = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
                            if (File.Exists(filePdfTemp))
                            {
                                File.Delete(filePdfTemp);
                            }
                        }
                        //Lưu đường dẫn vào bảng Bond Secondary Contract  
                        bondSecondaryContract.FileSignatureUrl = saveFileApp.FileSignatureUrl;
                        bondSecondaryContract.IsSign = IsSignPdf.Yes;
                        _bondSecondaryContractRepository.Update(bondSecondaryContract);
                    }
                }
            }
        }

        public int ChangeDeliveryStatusDelivered(int orderId)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.ChangeDeliveryStatusDelivered(orderId, tradingProviderId, modifiedBy);
        }
        public int ChangeDeliveryStatusReceived(int orderId)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.ChangeDeliveryStatusReceived(orderId, tradingProviderId, modifiedBy);
        }
        public int ChangeDeliveryStatusDone(int orderId)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.ChangeDeliveryStatusDone(orderId, tradingProviderId, modifiedBy);
        }
        public int ChangeDeliveryStatusRecevired(string deliveryCode)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            return _orderRepository.ChangeDeliveryStatusReceviredApp(deliveryCode, investorId, modifiedBy);
        }

        public void VerifyPhone(string deliveryCode, string phone, int tradingProviderId)
        {
            var phoneDto = _orderRepository.GetPhoneByDeliveryCode(deliveryCode);
            if (phoneDto.Phone == phone && phoneDto.DeliveryStatus == DeliveryStatus.DELIVERY)
            {
                _investorRepository.GenerateOtpByPhone(phone);
            }
            else
            {
                throw new FaultException(new FaultReason($"Xác nhận số điện thoại không chính xác"), new FaultCode(((int)ErrorCode.InvestorOTPExpire).ToString()), "");
            }
        }


        /// <summary>
        /// App lấy theo trạng thái đang đầu tư
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public List<AppOrderInvestorDto> AppOrderGetAll(int groupStatus)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var findOrderList = _orderRepository.AppGetAll(investorId, groupStatus);

            DateTime ngayHienTai = DateTime.Now.Date;
            var result = new List<AppOrderInvestorDto>();
            foreach (var item in findOrderList)
            {
                try
                {
                    var order = _orderRepository.FindById(item.OrderId, null);
                    var bondInfo = _productBondInfoRepository.FindById(order.BondId);
                    if (bondInfo == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô trái phiếu"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
                    }

                    var issuerFind = _issuerRepository.FindById(bondInfo.IssuerId);
                    if (issuerFind != null)
                    {
                        item.IconIssuer = issuerFind.Image;
                    }
                    item.BondCode = bondInfo.BondCode;

                    var policyFind = _productBondSecondaryRepository.FindPolicyById(order.PolicyId, order.TradingProviderId, IsShowApp.YES);
                    if (policyFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
                    }
                    item.PolicyName = policyFind.Name;

                    var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(order.PolicyDetailId, order.TradingProviderId, IsShowApp.YES);
                    if (policyDetailFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
                    }

                    item.Interest = policyDetailFind.Profit;
                    item.InterestPeriodType = policyDetailFind.InterestPeriodType;
                    item.Profit = 0;
                    //Nếu là đang đầu tư, tính xem số ngày còn lại
                    if (item.Status == OrderStatus.DANG_DAU_TU)
                    {
                        DateTime ngayDaoHan = _bondSharedService.CalculateDueDate(policyDetailFind, item.PaymentFullDate.Value);
                        item.TimeRemaining = ngayDaoHan.Subtract(ngayHienTai).Days;
                        if (item.TimeRemaining < 0)
                        {
                            item.TimeRemaining = 0;
                        }
                        var profitNow = _bondSharedService.ProfitNow(item.PaymentFullDate ?? DateTime.Now.Date, ngayDaoHan, policyDetailFind.Profit, item.TotalValue ?? 0);
                        item.Profit = profitNow;
                    }
                    result.Add(item);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không tìm thấy thông tin sổ lệnh {item.OrderId} ");
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public AppOrderInvestorDetailDto AppOrderInvestorDetail(int orderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            return _bondOrderShareServices.ViewOrderDetail(orderId, investorId);
        }

        public AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyDetailId)
        {
            var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetailFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            return _mapper.Map<AppSaleByReferralCodeDto>(_saleRepository.FindSaleByReferralCode(referralCode, policyDetailFind.TradingProviderId));
        }

        public PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode)
        {
            var result = _orderRepository.GetPhoneByDeliveryCode(deliveryCode);
            var lastThreeChar = result.Phone.Substring(result.Phone.Length - 3);
            var firstChar = new string('*', result.Phone.Length - 3);
            result.Phone = firstChar + lastThreeChar;
            return result;
        }

        public decimal ChangeDeliveryStatusRecevired(string deliveryCode, string otp)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            decimal verifyOtpResult = _orderRepository.ChangeDeliveryStatusRecevired(deliveryCode, otp, modifiedBy);
            if (verifyOtpResult == TrueOrFalseNum.FALSE)
            {
                throw new FaultException(new FaultReason($"OTP không hợp lệ"), new FaultCode(((int)ErrorCode.InvestorOTPInvalid).ToString()), "");
            }
            else if (verifyOtpResult == TrueOrFalseNum.EXPIRE)
            {
                throw new FaultException(new FaultReason($"OTP hết hạn"), new FaultCode(((int)ErrorCode.InvestorOTPExpire).ToString()), "");
            }
            return verifyOtpResult;
        }

        public InterestCalculationDateDto CheckInvestmentDay(int policyDetailId, DateTime ngaybatdau)
        {
            var result = new InterestCalculationDateDto();
            var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);

            var ngayDauTu = _bondSharedService.NextWorkDay(ngaybatdau, policyDetailFind.TradingProviderId);

            var ngayKetThuc = _bondSharedService.CalculateDueDate(policyDetailFind, ngayDauTu);
            result.StartDate = ngayDauTu;
            result.EndDate = ngayKetThuc;
            return result;
        }

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

        /// <summary>
        /// Đổ và xử lý thông tin dữ liệu chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ngayBatDau"></param>
        /// <param name="ngayKetThuc"></param>
        /// <returns></returns>
        public List<ThoiGianChiTraThucDto> LayDanhSachNgayChiTra(DanhSachChiTraFitlerDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var listOrder = _orderRepository.FindOrderByTradingProviderId(tradingProviderId, input.ContractCode, input.TaxCode, input.Phone);
            var danhSachChiTra = new List<ThoiGianChiTraThucDto>();
            Dictionary<long, ThoiGianChiTraThucDto> dictThoiGianChiTra = new();
            var listThoiGianChiTra = new List<ThoiGianChiTraThucDto>();
            var result = new List<ThoiGianChiTraThucDto>();

            foreach (var orderItem in listOrder)
            {
                //Lấy kỳ hạn
                var policyDetailFind = _productBondSecondaryRepository.GetPolicyDetailByIdWithoutTradingProviderId(orderItem.PolicyDetailId, false);
                List<DateTime> thoiGianThuc = new();

                //Lấy ngày bắt đầu tính lãi
                var ngayDauKy = orderItem.InvestDate.Value.Date;

                //Tính ngày đáo hạn
                var ngayDaoHan = _orderRepository.CalculateDueDate(policyDetailFind, ngayDauKy, false);

                //bắt đầu dầu tính khoảng thời gian của các kì từ ngày bắt đầu tới ngày đáo hạn
                if (policyDetailFind.InterestType == InterestTypes.DINH_KY)
                {
                    //Tính thời gian thực của kỳ trả
                    while (ngayDauKy <= ngayDaoHan)
                    {
                        //set ngày cuối kỳ, mặc định ngày cuối kì sẽ được tính bằng cộng ngày
                        DateTime ngayCuoiKy = ngayDauKy.AddDays(policyDetailFind.InterestPeriodQuantity ?? 0);

                        //nếu đơn vị kỳ hạn là tháng thì tính ngày cuối kì bằng cách cộng tháng
                        if (policyDetailFind.InterestPeriodType == PeriodUnit.MONTH)
                        {
                            ngayCuoiKy = ngayDauKy.AddMonths(policyDetailFind.InterestPeriodQuantity ?? 0);
                        }
                        //nếu đơn vị kỳ hạn là năm, thì tính ngày cuối kì bằng cách cộng năm
                        else if (policyDetailFind.InterestPeriodType == PeriodUnit.YEAR)
                        {
                            ngayCuoiKy = ngayDauKy.AddYears(policyDetailFind.InterestPeriodQuantity ?? 0);
                        }

                        //Chuyển đến kỳ tiếp theo
                        ngayDauKy = ngayCuoiKy;

                        var ngayLamViec = _orderRepository.NextWorkDay(ngayCuoiKy.Date, policyDetailFind.TradingProviderId, false);

                        //nếu ngày làm việc vượt quá ngày đáo hạn thì set ngày làm việc = ngày đáo hạn
                        if (ngayLamViec > ngayDaoHan)
                        {
                            ngayLamViec = ngayDaoHan;
                        }

                        //trường hợp cộng thừa một tý 
                        if (thoiGianThuc.Count > 1 && ngayLamViec == thoiGianThuc[^1]) //trong trường hợp là phần tử cuối cùng trong thời gian thực
                        {
                            break;
                        }
                        thoiGianThuc.Add(ngayLamViec);
                    };

                    //nếu đây là cuối kì thì
                    if (thoiGianThuc.Count > 0 && thoiGianThuc[^1] < ngayDaoHan)
                    {
                        thoiGianThuc[^1] = ngayDaoHan;
                    }

                    //Thời gian thực tính tiền...
                    for (int j = 0; j < thoiGianThuc.Count; j++)
                    {
                        int soNgay;
                        bool isLastPeriod = j == thoiGianThuc.Count - 1;
                        if (j == 0) //kỳ trả đầu tiên
                        {
                            soNgay = (thoiGianThuc[j] - orderItem.InvestDate.Value.Date).Days;
                        }
                        else
                        {
                            soNgay = (thoiGianThuc[j] - thoiGianThuc[j - 1]).Days;
                        }

                        listThoiGianChiTra.Add(new ThoiGianChiTraThucDto
                        {
                            PayDate = thoiGianThuc[j],
                            LastPayDate = j > 0 ? thoiGianThuc[j - 1] : thoiGianThuc[j],
                            PolicyDetailId = policyDetailFind.Id,
                            OrderId = orderItem.Id,
                            CifCode = orderItem.CifCode,
                            PeriodIndex = j + 1,
                            SoNgay = soNgay,
                            IsLastPeriod = isLastPeriod
                        });
                    }
                }
                else if (policyDetailFind.InterestType == InterestTypes.CUOI_KY) //Trả cuối kỳ
                {
                    int soNgay = ngayDaoHan.Subtract(ngayDauKy.Date).Days;

                    listThoiGianChiTra.Add(new ThoiGianChiTraThucDto
                    {
                        PayDate = ngayDaoHan,
                        LastPayDate = ngayDaoHan,
                        PolicyDetailId = policyDetailFind.Id,
                        OrderId = orderItem.Id,
                        CifCode = orderItem.CifCode,
                        PeriodIndex = 1,
                        SoNgay = soNgay
                    });
                }
                //danh sách các ngày đã trả trong interestpayment của order id này 
                var listOrderDaChiTra = _interestPaymentRepository.GetListInterestPaymentByOrderId(orderItem.Id, orderItem.TradingProviderId).Select(x => x.PeriodIndex).ToList();

                //lọc và loại các kì hạn đã chi trả lấy những kì hạn chưa chi trả
                listThoiGianChiTra = listThoiGianChiTra.Where(e => !listOrderDaChiTra.Contains(e.PeriodIndex)).ToList();
                result.AddRange(listThoiGianChiTra);
            }
            _orderRepository.CloseConnection();

            var list = new List<ThoiGianChiTraThucDto>();

            if (input.NgayChiTra != null)
            {
                list = result.Where(e => e.PayDate <= input.NgayChiTra).OrderByDescending(r => r.PayDate.Date).ToList();
            }
            else
            {
                list = result.OrderByDescending(r => r.PayDate.Date).ToList();
            }
            return list;
        }

        public PagingResult<DanhSachChiTraDto> LapDanhSachChiTra(DanhSachChiTraFitlerDto input)
        {
            var lapDanhSachChiTra = new List<DanhSachChiTraDto>();
            var result = new PagingResult<DanhSachChiTraDto>();
            var danhSachNgayChiTra = LayDanhSachNgayChiTra(input);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            danhSachNgayChiTra = danhSachNgayChiTra.Skip(input.PageSize * (input.PageNumber - 1)).Take(input.PageSize).ToList();
            foreach (var item in danhSachNgayChiTra)
            {
                string messagerError = "";
                var orderFind = _orderRepository.FindById(item.OrderId, tradingProviderId);
                var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(item.PolicyDetailId);

                var policyFind = _productBondSecondaryRepository.FindPolicyById(policyDetailFind.PolicyId, policyDetailFind.TradingProviderId);

                //Tính tổng số tiền đã được chi trả
                var soTienDaChiTra = _orderRepository.InterestPaymentSumMoney(item.PayDate, item.OrderId, orderFind.TradingProviderId);

                decimal? loiTucKyNay = 0;
                decimal? thue = 0;
                decimal? tongTienThucNhan = 0;
                decimal thueLoiNhuan = 0;
                string name = null;
                var cifCodeFind = new CifCodes();
                try
                {
                    cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                }
                catch
                {
                    cifCodeFind = null;
                }

                if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    thueLoiNhuan = (policyFind.IncomeTax) / 100;
                    var investerIdentification = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);
                    if (investerIdentification != null)
                        name = investerIdentification.Fullname;
                }
                else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    if (businessCustomer != null)
                    {
                        name = businessCustomer.Name;
                    }
                }
                if (item.IsLastPeriod)
                {
                    loiTucKyNay = (orderFind.TotalValue * (policyDetailFind.Profit / 100) * item.SoNgay / 365) - soTienDaChiTra;
                    thue = loiTucKyNay * (policyFind.IncomeTax) / 100;
                    var listCoupon = new List<CouponInfoDto>();
                    try
                    {
                        listCoupon = _bondSharedService.GetListCoupon(orderFind.BondId, orderFind.SecondaryId, orderFind.TotalValue, orderFind.InvestDate ?? default, item.PayDate, thue ?? 0, tradingProviderId);
                    }
                    catch (Exception ex)
                    {
                        messagerError = $"{ex.Message}";
                        listCoupon = new List<CouponInfoDto>();
                    }

                    decimal sumListCoupon = listCoupon.Sum(e => e.ActuallyCoupon);
                    tongTienThucNhan = loiTucKyNay - thue - sumListCoupon;

                }
                else
                {
                    loiTucKyNay = (orderFind.TotalValue * (policyDetailFind.Profit / 100) * item.SoNgay / 365) - soTienDaChiTra;
                    thue = loiTucKyNay * (policyFind.IncomeTax) / 100;
                    tongTienThucNhan = loiTucKyNay - thue;
                }

                lapDanhSachChiTra.Add(new DanhSachChiTraDto
                {
                    PayDate = item.PayDate,
                    PolicyDetailId = item.PolicyDetailId,
                    OrderId = item.OrderId,
                    CifCode = orderFind.CifCode,
                    PeriodIndex = item.PeriodIndex,
                    Profit = Math.Round(loiTucKyNay ?? 0),
                    AmountMoney = Math.Round(tongTienThucNhan ?? 0),
                    ActuallyProfit = Math.Round(tongTienThucNhan ?? 0),
                    Tax = Math.Round(thue ?? 0),
                    SoNgay = item.SoNgay,
                    BondOrder = orderFind,
                    PolicyDetail = policyDetailFind,
                    ContractCode = orderFind.ContractCode,
                    Name = name,
                    Message = messagerError
                });
            }
            result.Items = _mapper.Map<List<DanhSachChiTraDto>>(lapDanhSachChiTra);
            result.TotalItems = result.Items.Count();
            return result;
        }

        public int UpdateIsSignByOrderId(int orderId)
        {
            return _bondSecondaryContractRepository.UpdateIsSignByOrderId(orderId);
        }

        /// <summary>
        /// Hủy lệnh cho App đang trong trạng thái khởi tạo hoặc chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int AppCancelOrder(int orderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _orderRepository.AppCancelOrder(investorId, orderId, username);
        }
    }
}
