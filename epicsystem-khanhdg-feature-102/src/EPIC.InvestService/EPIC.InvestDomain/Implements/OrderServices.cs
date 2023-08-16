using AutoMapper;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.OrderContractFile;
using EPIC.InvestEntities.Dto.OrderPayment;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.RenewalsRequest;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.InvestSharedEntites.Dto.Order;
using EPIC.Notification.Services;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.ConstantVariables.Shared.Sysvar;
using EPIC.Utils.DataUtils;
using EPIC.Utils.EnumType;
using EPIC.Utils.Linq;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using OrderStatus = EPIC.Utils.ConstantVariables.Shared.OrderStatus;

namespace EPIC.InvestDomain.Implements
{
    public class OrderServices : IOrderServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<OrderServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly InvestOrderRepository _orderRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly InvestOrderPaymentRepository _orderPaymentRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly InvestRepositories.CalendarRepository _calendarRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly IInvestOrderContractFileServices _investOrderContractFileServices;
        private readonly IContractDataServices _contractDataServices;
        private readonly UserRepository _userRepository;
        private readonly SysVarRepository _sysVarRepository;
        private readonly OwnerRepository _ownerRepository;
        private readonly InvestInterestPaymentRepository _interestPaymentRepository;
        private readonly InvestInterestPaymentEFRepository _interestPaymentEFRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly InvRenewalsRequestRepository _renewalsRequestRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly IInvestOrderShareServices _investOrderShareServices;
        private readonly IRocketChatServices _rocketChatServices;
        private readonly InvestRepositories.BlockadeLiberationRepository _blockadeLiberationRepository;
        private readonly InvestRepositories.InvestOrderRepository _investOrderRepository;
        private readonly WithdrawalRepository _withdrawalSettlementRepository;
        private readonly SaleRepository _saleRepository;
        private readonly InvestNotificationServices _investSendEmailServices;
        private readonly InvestBackgroundJobService _investBackgroundJobService;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly GeneralContractorRepository _generalContractorRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly InvestorToDoEFRepository _investorToDoEFRepository;
        private readonly InvestorSaleEFRepository _investorSaleEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly InvestHistoryUpdateEFRepository _investHistoryUpdateEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvRenewalsRequestRepository _invRenewalsRequestRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        private readonly InvestOrderContractFileEFRepository _investOrderContractFileEFRepository;
        private readonly InvestCalendarEFRepository _investCalendarEFRepository;
        private readonly InvestContractTemplateEFRepository _investContractTemplateEFRepository;

        public OrderServices(
            EpicSchemaDbContext dbContext,
            ILogger<OrderServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IOptions<FileConfig> fileConfig,
            IInvestSharedServices investSharedServices,
            IInvestOrderContractFileServices investOrderContractFileServices,
            IContractDataServices contractDataServices,
            IRocketChatServices rocketChatServices,
            InvestNotificationServices investendEmailServices,
            InvestBackgroundJobService investBackgroundJobService,
            IBackgroundJobClient backgroundJobs,
            IInvestContractCodeServices investContractCodeServices,
            IInvestOrderShareServices investOrderShareServices)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _calendarRepository = new InvestRepositories.CalendarRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _orderPaymentRepository = new InvestOrderPaymentRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
            _ownerRepository = new OwnerRepository(_connectionString, _logger);
            _interestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _interestPaymentEFRepository = new InvestInterestPaymentEFRepository(_dbContext, logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _renewalsRequestRepository = new InvRenewalsRequestRepository(_connectionString, _logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(_dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, logger);
            _investOrderShareServices = investOrderShareServices;
            _investSharedServices = investSharedServices;
            _mapper = mapper;
            _fileConfig = fileConfig;
            _investOrderContractFileServices = investOrderContractFileServices;
            _contractDataServices = contractDataServices;
            _rocketChatServices = rocketChatServices;
            _investSendEmailServices = investendEmailServices;
            _blockadeLiberationRepository = new InvestRepositories.BlockadeLiberationRepository(_connectionString, _logger);
            _investOrderRepository = new InvestRepositories.InvestOrderRepository(_connectionString, _logger);
            _withdrawalSettlementRepository = new WithdrawalRepository(_connectionString, _logger);
            _investBackgroundJobService = investBackgroundJobService;
            _backgroundJobs = backgroundJobs;
            _investorEFRepository = new InvestorEFRepository(_dbContext, logger);
            _generalContractorRepository = new GeneralContractorRepository(_connectionString, logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, logger);
            _investorToDoEFRepository = new InvestorToDoEFRepository(_dbContext, logger);
            _investorSaleEFRepository = new InvestorSaleEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _investHistoryUpdateEFRepository = new InvestHistoryUpdateEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _invRenewalsRequestRepository = new InvRenewalsRequestRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _investContractTemplateEFRepository = new InvestContractTemplateEFRepository(dbContext, logger);
            _investContractCodeServices = investContractCodeServices;
            _investOrderContractFileEFRepository = new InvestOrderContractFileEFRepository(_dbContext, logger);
            _investCalendarEFRepository = new InvestCalendarEFRepository(_dbContext, logger);
        }

        public ViewOrderDto Add(CreateOrderDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = new InvOrder()
            {
                TradingProviderId = tradingProviderId,
                CifCode = input.CifCode,
                DistributionId = input.DistributionId,
                PolicyId = input.PolicyId,
                PolicyDetailId = input.PolicyDetailId,
                InvestorBankAccId = input.InvestorBankAccId,
                TotalValue = input.TotalValue,
                IsInterest = input.IsInterest,
                CreatedBy = username,
            };

            var cifCodeFind = _cifCodeRepository.GetByCifCode(input.CifCode);
            if (cifCodeFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mã cif của khách hàng"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            if (cifCodeFind.InvestorId != null)
            {
                if (input.SaleReferralCode != null)
                {
                    var saleReferralCode = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
                    order.SaleReferralCode = saleReferralCode.ReferralCode;
                    order.SaleReferralCodeSub = saleReferralCode.ReferralCodeSub;
                    order.DepartmentIdSub = saleReferralCode.DepartmentIdSub;
                }
                var investorIdentifiFind = _investorIdentificationRepository.GetByInvestorId(cifCodeFind.InvestorId ?? 0);
                if (investorIdentifiFind != null)
                {
                    order.InvestorIdenId = investorIdentifiFind.Id;
                }

                var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId ?? 0);
                if (investorFind != null)
                {
                    /*if (investorFind.ReferralCodeSelf == input.SaleReferralCode)
                    {
                        throw new FaultException(new FaultReason($"Mã giới thiệu đang trùng với khách hàng"), new FaultCode(((int)ErrorCode.SaleReferralCodeExistReferralCodeSaleOrder).ToString()), "");
                    }*/
                }

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
            List<ReplaceTextDto> data = new();
            var transaction = _orderRepository.BeginTransaction();
            var insertOrder = _orderRepository.Add(order, false);
            _dbContext.SaveChanges();
            //Get data cho hợp đồng
            data = _contractDataServices.GetDataContractFile(insertOrder, tradingProviderId, true);
            transaction.Commit();
            // Sinh file hợp đồng mẫu
            var jobId = _backgroundJobs.Enqueue(() => _investBackgroundJobService.UpdateContractFile(insertOrder, tradingProviderId, data, username));
            var orderFind = _dbContext.InvOrders.FirstOrDefault(e => e.Id == insertOrder.Id).ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
            orderFind.BackgroundJobId = jobId; 
            _dbContext.SaveChanges();
            var result = _mapper.Map<ViewOrderDto>(insertOrder);
            return result;
        }

        public void Delete(List<int> orderIds)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var id in orderIds)
            {
                var order = _dbContext.InvOrders.FirstOrDefault(o => o.Id == id && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                            .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
                if (_dbContext.InvestOrderPayments.Any(p => p.OrderId == order.Id && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO))
                {
                    _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderCanNotDeleteCuzExistPayment);
                }  
                
                if (order.Status == OrderStatus.KHOI_TAO || order.Status == OrderStatus.CHO_THANH_TOAN)
                {
                    order.Deleted = YesNo.YES;
                    order.ModifiedBy = username;
                    order.ModifiedDate = DateTime.Now;
                }
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public PagingResult<ViewOrderDto> FindAll(InvestOrderFilterDto input, int? groupStatus)
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

            input.GroupStatus = groupStatus;
            input.TradingProviderId = tradingProviderId;
            var orderList = _investOrderEFRepository.FindAll(input);

            var result = new PagingResult<ViewOrderDto>();
            var items = new List<ViewOrderDto>();
            result.TotalItems = orderList.TotalItems;
            foreach (var orderFind in orderList.Items)
            {
                var orderItem = _mapper.Map<ViewOrderDto>(orderFind);
                orderItem.Investor = _mapper.Map<Entities.Dto.Investor.InvestorDto>(orderFind.CifCodes.Investor);
                orderItem.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(orderFind.CifCodes.BusinessCustomer);
                orderItem.Distribution = _mapper.Map<ViewDistributionDto>(orderFind.Distribution);
                orderItem.Policy = _mapper.Map<ViewPolicyDto>(orderFind.Policy);
                orderItem.PolicyDetail = _mapper.Map<ViewPolicyDetailDto>(orderFind.PolicyDetail);
                orderItem.BondPolicies = _mapper.Map<List<ViewPolicyDto>>(orderFind.Distribution.Policies);
                orderItem.Project = _mapper.Map<ProjectDto>(orderFind.Project);

                orderItem.IsRenewalsRequest = _renewalsRequestRepository.GetByOrderId(orderFind.Id, tradingProviderId ?? 0) is not null;
                orderItem.IsWithdrawalRequest = _withdrawalSettlementRepository.WithdrawalGetList(orderFind.Id, tradingProviderId ?? 0, InvestWithdrawalStatus.YEU_CAU_RUT).Count() != 0;

                if (orderFind.CifCodes.Investor != null)
                {
                    var investorIdenDefaultFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
                    if (investorIdenDefaultFind != null)
                    {
                        orderItem.Investor.InvestorIdentification = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                    }
                }

                if (orderFind.CifCodes.BusinessCustomer == null)
                {
                    orderItem.CustomerType = CustomerType.IS_INVETOR;
                }
                if (orderFind.CifCodes.Investor == null)
                {
                    orderItem.CustomerType = CustomerType.IS_BUSINESSCUSTOMER;
                }

                if (orderItem.Source == SourceOrder.ONLINE)
                {
                    orderItem.OrderSource = SourceOrderFE.KHACH_HANG;
                }
                else if (orderItem.Source == SourceOrder.OFFLINE && orderItem.SaleOrderId == null)
                {
                    orderItem.OrderSource = SourceOrderFE.QUAN_TRI_VIEN;
                }
                else if (orderItem.SaleOrderId != null)
                {
                    orderItem.OrderSource = SourceOrderFE.SALE;
                }

                orderItem.GenContractCode = _investOrderEFRepository.GetContractCodeGen(orderFind.Id);
                items.Add(orderItem);
            }
            result.Items = items;
            return result;
        }

        // get all delivery status cua giao nhan hop dong
        public PagingResult<ViewOrderDto> FindAllDeliveryStatus(InvestOrderFilterDto input, int? groupStatus)
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

            //var orderList = _orderRepository.FindAllDeliveryStatus(tradingProviderId, groupStatus, input, input.TradingProviderIds);
            input.GroupStatus = groupStatus;
            input.TradingProviderId = tradingProviderId;
            var orderList = _investOrderEFRepository.FindAllDeliveryStatus(input);
            var result = new PagingResult<ViewOrderDto>();
            var items = new List<ViewOrderDto>();
            result.TotalItems = orderList.TotalItems;
            foreach (var orderFind in orderList.Items)
            {
                var orderItem = _mapper.Map<ViewOrderDto>(orderFind);
                if (orderFind.CifCodes != null)
                {
                    if (orderFind.CifCodes.InvestorId != null && orderFind.CifCodes.BusinessCustomerId == null)
                    {
                        orderItem.CustomerType = CustomerType.IS_INVETOR;
                    }
                    if (orderFind.CifCodes.InvestorId == null && orderFind.CifCodes.BusinessCustomerId != null)
                    {
                        orderItem.CustomerType = CustomerType.IS_BUSINESSCUSTOMER;
                    }
                }

                var orderContractFile = _dbContext.InvestOrderContractFile.Where(o => o.OrderId == orderFind.Id && o.Deleted == YesNo.NO);
                if (orderContractFile.Any())
                {
                    var contractCodeGen = orderContractFile.FirstOrDefault().ContractCodeGen;
                    orderItem.GenContractCode = orderContractFile.All(r => r.ContractCodeGen == contractCodeGen) ? contractCodeGen : null;
                }

                if (orderFind.CifCodes.Investor != null)
                {
                    var investor = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorDto>(orderFind.CifCodes.Investor);
                    orderItem.Investor = investor;
                    var investorIdenDefaultFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
                    if (investorIdenDefaultFind != null)
                    {
                        orderItem.Investor.InvestorIdentification = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                    }
                }
                orderItem.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(orderFind.CifCodes.BusinessCustomer);
                orderItem.Distribution = _mapper.Map<ViewDistributionDto>(orderFind.Distribution);
                orderItem.Policy = _mapper.Map<ViewPolicyDto>(orderFind.Policy);
                orderItem.PolicyDetail = _mapper.Map<ViewPolicyDetailDto>(orderFind.PolicyDetail);
                orderItem.Project = _mapper.Map<ProjectDto>(orderFind.Project);
                orderItem.GenContractCode = _investOrderEFRepository.GetContractCodeGen(orderFind.Id);
                items.Add(orderItem);
            }
            if (input.PendingDate != null)
            {
                items.OrderByDescending(s => s.PendingDate);
            }
            result.Items = items;
            return result;
        }

        public PagingResult<InvestOrderInvestmentHistoryDto> FindAllInvestHistory(FilterInvestOrderDto input, int[] status)
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
            _logger.LogInformation($"{nameof(OrderServices)}->{nameof(FindAllInvestHistory)}: input = {JsonSerializer.Serialize(input)}, status = {status}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var result = _investOrderEFRepository.FindAllInvestHistory(input, status, tradingProviderId);
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

            // Hợp đồng gốc của hợp đồng nếu là tái tục
            if (orderFind.RenewalsReferIdOriginal != null)
            {
                var orderFindOriginal = _orderRepository.FindById(orderFind.RenewalsReferIdOriginal ?? 0, tradingProviderId, partnerId);
                if (orderFindOriginal != null)
                {
                    result.ContractCodeOriginal = orderFindOriginal.ContractCode;
                }
            }
            // Lấy chính sách từ hợp đồng
            var policyQuery = _policyRepository.FindPolicyById(orderFind.PolicyId, tradingProviderId);
            if (policyQuery != null)
            {
                // Loại tái tục hợp đồng của chính sách
                result.RenewalsType = policyQuery.RenewalsType;
            }

            // Kiểm tra có phải là lần thanh toán đầu tiên
            result.IsFirstPayment = YesNo.NO;

            long firstPaymentBankId = 0;
            var paymentSuccessList = _orderPaymentRepository.GetListPaymentSuccess(id);
            if (paymentSuccessList.Count == 0)
            {
                result.IsFirstPayment = YesNo.YES;
            }
            else
            {
                var firstPayment = paymentSuccessList.FirstOrDefault();
                if (firstPayment != null)
                {
                    firstPaymentBankId = firstPayment.TradingBankAccId;
                }
            }

            result.IsRenewalsRequest = _renewalsRequestRepository.GetByOrderId(orderFind.Id, tradingProviderId ?? 0) is not null;
            result.IsWithdrawalRequest = _withdrawalSettlementRepository.WithdrawalGetList(orderFind.Id, tradingProviderId ?? 0, InvestWithdrawalStatus.YEU_CAU_RUT) is not null;
            //Tìm kiếm là khách hàng cá nhân hay là khách hàng doanh nghiệp
            var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);

            //Lấy ra danh sách mẫu hợp đồng để gen mã hợp đồng
            var configContractTemplates = (from ct in _dbContext.InvestContractTemplates
                                    join ctt in _dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id into contracTempTemps
                                    from contractTemp in contracTempTemps.DefaultIfEmpty()
                                    join ocf in _dbContext.InvestOrderContractFile on ct.Id equals ocf.ContractTempId into orderContractFiles
                                    from orderContractFile in orderContractFiles.DefaultIfEmpty()
                                    where (ct.PolicyId == policyQuery.Id && ct.TradingProviderId == tradingProviderId && ct.Deleted == YesNo.NO && ct.Status == Utils.Status.ACTIVE
                                    && ((ct.ContractType == null && contractTemp.ContractType == ContractTypes.DAT_LENH) || (ct.ContractType != null && ct.ContractType == ContractTypes.DAT_LENH))
                                    && ((contractTemp.ContractSource == orderFind.Source || contractTemp.ContractSource == ContractSources.ALL) || (ct.ContractSource == orderFind.Source || ct.ContractSource == ContractSources.ALL)))
                                    select ct.ConfigContractId);

            //if (configContractTemplates.Distinct().Count() == 1)
            //{
            //    var contractCode = _investContractCodeServices.GetContractCode(orderFind, policyQuery, configContractTemplates.FirstOrDefault());
            //    result.GenContractCode = contractCode;
            //}

            var contractCodeGen = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == orderFind.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
            if (contractCodeGen.Count() == 1)
            {
                result.GenContractCode = contractCodeGen.First();
            }

            if (cifCodeFind != null && cifCodeFind.InvestorId != null)
            {
                var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId.Value);
                //var investorFind = _managerInvestorRepository.FindById(cifCodeFind.InvestorId.Value, false);
                if (investorFind != null)
                {
                    var investor = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorDto>(investorFind);
                    var listBank = _managerInvestorRepository.GetListBank(investorFind.InvestorId, investorFind.InvestorGroupId, false)?.ToList();
                    var listContractAddress = _managerInvestorRepository.GetListContactAddress(-1, 0, null, investor.InvestorId, false).Items.ToList();

                    investor.ListBank = listBank;
                    result.Investor = investor;
                    result.ListContactAddress = listContractAddress;

                    var investorIdenDefaultFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
                    if (investorIdenDefaultFind != null)
                    {
                        result.Investor.InvestorIdentification = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                    }

                    var listInvestorIdenDefaultFind = _investorIdentificationRepository.GetListByInvestorId(investorFind.InvestorId);
                    if (investorIdenDefaultFind != null)
                    {
                        result.Investor.ListInvestorIdentification = _mapper.Map<List<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>>(listInvestorIdenDefaultFind);
                    }
                }
            }
            else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
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

            //Lấy thông tin phát hành thứ cấp
            var distributionFind = _distributionRepository.FindById(orderFind.DistributionId, orderFind.TradingProviderId);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin phát hành thứ cấp"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            else if (distributionFind != null)
            {
                var distribution = _mapper.Map<ViewDistributionDto>(distributionFind);

                //Tài khoản thụ hưởng của dlsc muốn nhận khi bán theo kỳ hạn
                result.BusinessCustomerBankAccId = distributionFind.BusinessCustomerBankAccId;
                if (result.IsFirstPayment == YesNo.NO) // lấy thông tin bank đầu tiên thanh toán
                {
                    var businesBankAccFind = _businessCustomerRepository.FindBusinessCusBankById(firstPaymentBankId);
                    if (businesBankAccFind != null)
                    {
                        result.TradingProviderBank = _mapper.Map<BusinessCustomerBankDto>(businesBankAccFind);
                        result.TradingProviderBank.BusinessCustomerBankAccId = (int)firstPaymentBankId;
                    }
                    else // lấy theo distribution
                    {
                        businesBankAccFind = _businessCustomerRepository.FindBusinessCusBankById(distributionFind.BusinessCustomerBankAccId ?? 0);
                        if (businesBankAccFind != null)
                        {
                            result.TradingProviderBank = _mapper.Map<BusinessCustomerBankDto>(businesBankAccFind);
                            result.TradingProviderBank.BusinessCustomerBankAccId = distributionFind.BusinessCustomerBankAccId ?? 0;
                        }
                    }
                }

                //Lấy thông tin chính sách và kỳ hạn
                distribution.Policies = new List<ViewPolicyDto>();
                var policyList = _policyRepository.GetAllPolicy(distribution.Id, distribution.TradingProviderId);
                foreach (var policyItem in policyList)
                {
                    var policy = _mapper.Map<ViewPolicyDto>(policyItem);

                    policy.FakeId = policyItem.Id;
                    policy.PolicyDetail = new List<ViewPolicyDetailDto>();

                    var policyDetailList = _policyRepository.GetAllPolicyDetail(policy.Id, distribution.TradingProviderId);
                    foreach (var detailItem in policyDetailList)
                    {
                        var detail = _mapper.Map<ViewPolicyDetailDto>(detailItem);
                        detail.FakeId = detailItem.Id;
                        policy.PolicyDetail.Add(detail);
                    }
                    distribution.Policies.Add(policy);
                }
                result.Distribution = distribution;
            }

            var policyDetailFind = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId, orderFind.TradingProviderId);
            if (policyDetailFind != null)
            {
                var ngayDauTu = DateTime.Now.Date;
                if (result.BuyDate != null && result.PaymentFullDate == null && result.InvestDate == null)
                {
                    ngayDauTu = result.BuyDate.Value.Date;
                }
                else if (result.InvestDate != null)
                {
                    ngayDauTu = result.InvestDate.Value;
                }
                result.InvestDate = ngayDauTu;
                result.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, ngayDauTu, distributionFind.CloseCellDate);
            }

            if (orderFind.SaleOrderId != null)
            {
                result.SaleOrder = new();
                var saleQuery = _saleEFRepository.Entity.FirstOrDefault(s => s.SaleId == orderFind.SaleOrderId && s.Deleted == YesNo.NO);
                if (saleQuery != null && saleQuery.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(saleQuery.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorDto>(investorFind);
                        result.SaleOrder.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleQuery.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            result.SaleOrder.Investor.InvestorIdentification = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                    }
                }
                else if (saleQuery != null && saleQuery.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleQuery.BusinessCustomerId ?? 0);
                    result.SaleOrder.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                }
            }

            var saleFind = _saleEFRepository.FindSaleByReferralCode(orderFind.SaleReferralCode);
            if (saleFind != null)
            {
                result.Sale = _mapper.Map<ViewSaleDto>(saleFind);
                if (saleFind.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(saleFind.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorDto>(investorFind);
                        result.Sale.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleFind.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            result.Sale.Investor.InvestorIdentification = _mapper.Map<EPIC.Entities.Dto.Investor.InvestorIdentificationDto>(investorIdenDefaultFind);
                        }

                        var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleFind.InvestorId ?? 0);
                        result.Sale.Investor.ListBank = _mapper.Map<List<Entities.DataEntities.InvestorBankAccount>>(investorFindBank);
                    }
                }
                else if (saleFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleFind.BusinessCustomerId ?? 0);
                    result.Sale.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    result.Sale.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }

                var departmentFind = _departmentRepository.FindBySaleId(saleFind.SaleId, orderFind.TradingProviderId);
                if (departmentFind != null)
                {
                    result.DepartmentName = departmentFind.DepartmentName;
                }
                else
                {
                    _logger.LogError($"Không tìm thấy phòng giao dịch quản lý hợp đồng: departmentId = {orderFind.DepartmentId}, tradingProviderId = {orderFind.TradingProviderId}");
                }

                var departmentOfSaleFind = _departmentRepository.FindById(orderFind.DepartmentId ?? 0, orderFind.TradingProviderId);
                if (departmentOfSaleFind != null)
                {
                    result.ManagerDepartmentName = departmentOfSaleFind.DepartmentName;
                }
                else
                {
                    _logger.LogError($"Không tìm thấy phòng giao dịch quản lý hợp đồng: saleId = {saleFind.SaleId}, tradingProviderId = {orderFind.TradingProviderId}");
                }
                result.SaleSubName = _saleEFRepository.FindSaleName(orderFind.SaleReferralCodeSub)?.Name;
                result.DepartmentNameSub = _dbContext.Departments.FirstOrDefault(r => r.DepartmentId == orderFind.DepartmentIdSub && r.Deleted == YesNo.NO)?.DepartmentName;
            }
            return result;
        }

        public int Update(UpdateOrderDto input, int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _investOrderRepository.FindById(orderId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

            var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode)
                .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            var updateOrder = new InvOrder
            {
                Id = orderId,
                TradingProviderId = tradingProviderId,
                TotalValue = input.TotalValue,
                IsInterest = input.IsInterest,
                PolicyDetailId = input.PolicyDetailId,
                SaleReferralCode = input.SaleReferralCode,
                InvestorBankAccId = input.InvestorBankAccId,
                ContractAddressId = input.ContractAddressId,
                ModifiedBy = username,
            };
            if (cifCodeFind.InvestorId != null)
            {
                var investorFind = _investorRepository.FindById(cifCodeFind.InvestorId ?? 0);
                if (investorFind != null && input.SaleReferralCode != null)
                {
                    var findSaleReferralCode = _saleEFRepository.AppFindSaleOrderByReferralCode(input.SaleReferralCode, tradingProviderId);
                    updateOrder.SaleReferralCode = findSaleReferralCode.ReferralCode;
                    updateOrder.DepartmentId = findSaleReferralCode.DepartmentId;
                    updateOrder.SaleReferralCodeSub = findSaleReferralCode.ReferralCodeSub;
                    updateOrder.DepartmentIdSub = findSaleReferralCode.DepartmentIdSub;
                }
            }
            return _orderRepository.Update(updateOrder);
        }

        public OrderPaymentDto OrderPaymentAdd(CreateOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var insertPayment = _orderPaymentRepository.Add(new OrderPayment
            {
                TradingProviderId = tradingProviderId,
                OrderId = input.OrderId,
                TradingBankAccId = input.TradingBankAccId,
                TranDate = input.TranDate,
                TranType = input.TranType,
                TranClassify = input.TranClassify,
                PaymentType = input.PaymentType,
                PaymentAmnount = input.PaymentAmnount,
                Description = input.Description,
                CreatedBy = username
            });
            return _mapper.Map<OrderPaymentDto>(insertPayment);
        }

        public int ApproveOrderPayment(int orderPaymentId, int status)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var findOrderPayment = _orderPaymentRepository.FindById(orderPaymentId);
            var orderBeforApprove = _interestPaymentRepository.FindOrderById(orderPaymentId);

            if (findOrderPayment == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin thanh toán"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var result = _orderPaymentRepository.ApprovePayment(orderPaymentId, status, tradingProviderId, username);
            var orderFind = _investOrderEFRepository.FindById(findOrderPayment.OrderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

            // Hạn mức tối đa của phân phối
            var maxTotalInvestment = _investOrderRepository.MaxTotalInvestment(orderFind.TradingProviderId, orderFind.DistributionId);
            // Ghi lại log khi duyệt thanh toán
            _logger.LogInformation($"{(status == OrderPaymentStatus.DA_THANH_TOAN ? "Phê duyệt" : "Hủy duyệt")} thanh toán hợp đồng {orderFind.ContractCode} Ngày {DateTime.Now:dd-MM-yyyy}. Hạn mức còn lại là: {maxTotalInvestment}");

            // Nếu hạn mức dưới 0, tắt showApp và ghi lại thời gian
            if (maxTotalInvestment <= 0)
            {
                var distributionFind = _dbContext.InvestDistributions.FirstOrDefault(d => d.Id == orderFind.DistributionId && d.Deleted == YesNo.NO && d.TradingProviderId == tradingProviderId);
                distributionFind.IsShowApp = YesNo.NO;
                _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(orderFind.DistributionId, YesNo.YES, YesNo.NO, InvestFieldName.UPDATE_DISTRIBUTION_IS_SHOW_APP, InvestHistoryUpdateTables.INV_DISTRIBUTION,
                    ActionTypes.CAP_NHAT, $"Hệ thống tắt show app khi phê duyệt thanh toán Hợp đồng {orderFind.ContractCode}", DateTime.Now), username);
                _dbContext.SaveChanges();
            }

            var data = _contractDataServices.GetDataContractFile((int)findOrderPayment.OrderId, tradingProviderId, true);

            //Thay đổi Bank khách hàng nên phải update lại hợp đồng
            //_backgroundJobs.Enqueue(() => _investBackgroundJobService.UpdateContractFile(orderFind, tradingProviderId, data, username));
            return result;
        }

        public OrderPaymentDto FindPaymentById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _mapper.Map<OrderPaymentDto>(_orderPaymentRepository.FindById(id, tradingProviderId));
        }
        public int UpdateOrderPayment(UpdateOrderPaymentDto input, int Id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderPaymentRepository.Update(new OrderPayment
            {
                Id = Id,
                OrderId = input.OrderId,
                TradingProviderId = tradingProviderId,
                TranDate = input.TranDate,
                TranType = input.TranType,
                TranClassify = input.TranClassify,
                PaymentType = input.PaymentType,
                PaymentAmnount = input.PaymentAmnount,
                Description = input.Description,
                ModifiedBy = username
            });
        }

        public int DeleteOrderPayment(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderPaymentRepository.Delete(id, tradingProviderId);
        }

        public PagingResult<OrderPayment> FindAllOrderPayment(int orderId, int pageSize, int pageNumber, string keyword, int? status)
        {
            return _orderPaymentRepository.FindAll(orderId, pageSize, pageNumber, keyword, status);
        }

        public async Task UpdateContractFileUpdateSource(int orderId)
        {
            string filePath = _fileConfig.Value.Path;
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _orderRepository.FindById(orderId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

            var orderContractFiles = _orderContractFileRepository.FindAll(orderId, tradingProviderId);

            //Xóa các file hợp đồng offline của order
            foreach (var contracFile in orderContractFiles)
            {
                FileUtils.RemoveOrderContractFile(new OrderContractFileRemoveDto
                {
                    FileScanUrl = contracFile.FileScanUrl,
                    FileTempPdfUrl = contracFile.FileTempPdfUrl,
                    FileSignatureUrl = contracFile.FileSignatureUrl,
                    FileTempUrl = contracFile.FileTempUrl,
                }, filePath);
            }

            //Xóa tất cả hợp dồng offline của lệnh
            _orderContractFileRepository.DeleteByOrderId(orderId);
            var data = _contractDataServices.GetDataContractFile(order, tradingProviderId, true);
            //sinh lại hợp đồng
            await _investOrderContractFileServices.UpdateContractFile(order, data, ContractTypes.DAT_LENH);
        }

        public void CheckOrder(AppCheckOrderDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _orderRepository.OrderInvestorAdd(input, investorId, true);
        }

        /// <summary>
        /// Tự đặt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AppOrderDto> OrderInvestorAdd(AppCreateOrderDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            AppOrderDto order = await OrderInvestorAddCommon(input, investorId, true);
            return order;
        }

        /// <summary>
        /// Sale đặt lệnh investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AppOrderDto> SaleOrderInvestorAdd(AppSaleInvestorCreateOrderDto input)
        {
            var dto = _mapper.Map<AppCreateOrderDto>(input);
            AppOrderDto order = await OrderInvestorAddCommon(dto, input.InvestorId, false);
            return order;
        }

        /// <summary>
        /// Đặt lệnh dùng chung
        /// </summary>
        /// <param name="input"></param>
        /// <param name="investorId"></param>
        /// <param name="isSelfDoing"></param>
        /// <returns></returns>
        public async Task<AppOrderDto> OrderInvestorAddCommon(AppCreateOrderDto input, int investorId, bool isSelfDoing)
        {
            var isInvestorActive = _investorEFRepository.IsInvestorActive(investorId);
            if (!isInvestorActive)
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorIdIsNotVerifiedId);
            }

            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            AppOrderDto order = null;

            int? saleId = null;
            if (!isSelfDoing)
            {
                saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            }
            else
            {
                if (input.IsReceiveContract && input.TranAddess == null)
                {
                    throw new FaultException(new FaultReason($"Địa chỉ giao dịch không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }

            List<ReplaceTextDto> data = new();

            try
            {
                if (input.TranAddess == null)
                {
                    input.IsReceiveContract = false;
                }
                //var transection = _orderRepository.BeginTransaction();
                order = _orderRepository.OrderInvestorAdd(input, investorId, false, input.OTP, ipAddress, isSelfDoing, saleId, username);
                data = _contractDataServices.GetDataContractFile(order.Id, order.TradingProviderId, true);
                //transection.Commit();
                var orderQuery = _orderRepository.FindById(order.Id);
                if (orderQuery == null)
                {
                    _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderNotFound);
                }
                _backgroundJobs.Enqueue(() => _investBackgroundJobService.UpdateContractFile(orderQuery, order.TradingProviderId, data, username));

                var policyDetail = _policyRepository.FindPolicyDetailById(input.PolicyDetailId ?? 0);
                var policy = _policyRepository.FindPolicyById(policyDetail?.PolicyId ?? 0);
                if (policy != null)
                {
                    var orderContractFile = _investContractCodeServices.GenOrderContractCodeDefault(new GenInvestContractCodeDto
                    {
                        Order = orderQuery,
                        Policy = policy
                    });
                    order.ContractCode = orderContractFile ?? order.ContractCode;
                    order.PaymentNote = PaymentNotes.THANH_TOAN + order.ContractCode;
                }
                order.TradingBankAccounts = await _investOrderShareServices.TradingBankAccountOfDistribution(order, order.DistributionId);
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

            _rocketChatServices.AddCurrentUserToTradingProviderChannel(order.TradingProviderId);
            return order;
        }

        public int UpdateSettlementMethod(int orderId, SettlementMethodDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.AppUpdateSettlementMethod(orderId, input, username, null, tradingProviderId);
        }

        public int AppUpdateSettlementMethod(int orderId, SettlementMethodDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            return _orderRepository.AppUpdateSettlementMethod(orderId, input, username, investorId);
        }

        public int UpdateTotalValue(int id, decimal? totalValue)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.UpdateTotalValue(id, tradingProviderId, totalValue, username);
        }

        public int UpdateReferralCode(int id, string referralCode)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            SaleInBusinessCustomerSaleSubDto findSaleReferralCode = new();
            if (!string.IsNullOrEmpty(referralCode))
            {
                findSaleReferralCode = _saleEFRepository.AppFindSaleOrderByReferralCode(referralCode, tradingProviderId);
            }
            var orderQuery = _investOrderEFRepository.FindById(id, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

            if ((findSaleReferralCode.ReferralCodeSub == null && orderQuery.SaleReferralCodeSub != null) || findSaleReferralCode.ReferralCodeSub != orderQuery.SaleReferralCodeSub)
            {
                var saleSubInfo = _saleEFRepository.FindSaleName(orderQuery.SaleReferralCodeSub);
                var saleSubInfoNew = _saleEFRepository.FindSaleName(findSaleReferralCode.ReferralCodeSub);
                string oldValue = orderQuery.SaleReferralCodeSub != null ? orderQuery.SaleReferralCodeSub + ": " + saleSubInfo.Name + " (" + saleSubInfo.SaleId + ") " + "DepartmentIdSub: " + orderQuery.DepartmentIdSub
                                    : null;

                string newValue = findSaleReferralCode.ReferralCodeSub != null ? findSaleReferralCode.ReferralCodeSub + ": " + saleSubInfoNew.Name + " (" + saleSubInfoNew.SaleId + ") " + "DepartmentIdSub: " + findSaleReferralCode.DepartmentIdSub
                                    : null;

                orderQuery.SaleReferralCodeSub = findSaleReferralCode.ReferralCodeSub;
                orderQuery.DepartmentIdSub = findSaleReferralCode.DepartmentIdSub;
                _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(id, oldValue, newValue, InvestFieldName.UPDATE_ORDER_SALE_REFERRAL_CODE_SUB, InvestHistoryUpdateTables.INV_ORDER, ActionTypes.CAP_NHAT, "Cập nhật mã giới thiệu bán hộ", DateTime.Now), username);
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
                _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(id, oldValue, newValue, InvestFieldName.UPDATE_ORDER_SALE_REFERRAL_CODE, InvestHistoryUpdateTables.INV_ORDER, ActionTypes.CAP_NHAT, "Cập nhật mã giới thiệu", DateTime.Now), username);
            }
            _dbContext.SaveChanges();
            return -1;
        }

        public int UpdatePolicyDetail(int id, int? policyDetailId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _dbContext.InvOrders.FirstOrDefault(o => o.Id == id && o.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            var oldValue = _dbContext.InvestPolicyDetails.Include(i => i.Policy)
                .Where(p => p.Id == order.PolicyDetailId && p.Deleted == YesNo.NO && p.Policy.Deleted == YesNo.NO)
                .Select(x => $"{x.Name} - {x.Policy.Name} ({x.Id})").FirstOrDefault();

            var newValue = _dbContext.InvestPolicyDetails.Include(i => i.Policy)
                .Include(i => i.Distribution)
                .Where(p => p.Id == policyDetailId && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO && p.Policy.Deleted == YesNo.NO)
                .Select(x => new
                {
                    NewValue = $"{x.Name} - {x.Policy.Name} ({x.Id})",
                    PolicyId = x.PolicyId,
                    PolicyMaxMoney = x.Policy.MaxMoney,
                    PolicyDetailId = x.Id,
                    PolicyDetail = x,
                    DistributionCloseSellDate = x.Distribution.CloseCellDate
                }).FirstOrDefault().ThrowIfNull(_dbContext, ErrorCode.InvestPolicyDetailNotFound);

            if (order.Status == OrderStatus.DANG_DAU_TU)
            {
                if (_dbContext.InvestInterestPayments.Any(o => o.OrderId == order.Id && o.Deleted == YesNo.NO && o.Status != InterestPaymentStatus.HUY_DUYET))
                {
                    // Đã có yêu cầu chi trả không được thay đổi
                    _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderInterstPaymentRequestExist);
                }
                if (_dbContext.InvestWithdrawals.Any(o => o.OrderId == order.Id && o.Deleted == YesNo.NO && o.Status != WithdrawalStatus.HUY_YEU_CAU))
                {
                    // Đã có yêu cầu rút vốn không được thay đổi
                    _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderWithdrawalRequestExist);
                }

                //Tính lại ngày đáo hạn
                order.DueDate = _investOrderRepository.CalculateDueDate(newValue.PolicyDetail, order.InvestDate.Value, newValue.DistributionCloseSellDate, false);
            }
            else if (!new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_DUYET_HOP_DONG, OrderStatus.CHO_THANH_TOAN }.Contains(order.Status))
            {
                _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderStatusUpdateInvalid);
            }
            //Cập nhật thông tin
            order.PolicyId = newValue.PolicyId;
            order.PolicyDetailId = newValue.PolicyDetailId;
            order.ModifiedBy = username;
            order.ModifiedDate = DateTime.Now;

            // Cập nhật lịch sử
            _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(id, oldValue, newValue.NewValue, InvestFieldName.UPDATE_ORDER_POLICY_DETAIL_ID, InvestHistoryUpdateTables.INV_ORDER, ActionTypes.CAP_NHAT, "Cập nhật kỳ hạn của hợp đồng", DateTime.Now), username);
            _dbContext.SaveChanges();
            transaction.Commit();
            if (order.Status == OrderStatus.DANG_DAU_TU)
            {
                //Xóa đi thời gian cũ
                var interestPaymentDate = _dbContext.InvestInterestPaymentDates.Where(o => o.OrderId == order.Id);
                _dbContext.InvestInterestPaymentDates.RemoveRange(interestPaymentDate);

                // Lưu lại thời gian mới
                var profitInfo = ExpectedCashFlow((int)order.Id);
                foreach (var profitItem in profitInfo.ProfitInfo)
                {
                    _interestPaymentEFRepository.AddInvestInterestPaymentDate(new InvestInterestPaymentDate
                    {
                        OrderId = (int)order.Id,
                        PayDate = profitItem.PayDate ?? DateTime.MinValue,
                        PeriodIndex = profitInfo.ProfitInfo.IndexOf(profitItem)
                    });
                }
                _dbContext.SaveChanges();
            }
            return id;
        }

        public async Task<int> UpdateSource(int id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _investOrderEFRepository.FindById(id)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
            if (orderFind.BackgroundJobId != null)
            {
                IStorageConnection connection = JobStorage.Current.GetConnection();
                JobData jobData = connection.GetJobData(orderFind.BackgroundJobId);
                if (jobData != null && jobData.State == EnumBackgroundJobState.Processing)
                {
                    throw new FaultException(new FaultReason($"Đang sinh hợp đồng, vui lòng chờ trong giây lát"), new FaultCode(((int)ErrorCode.InvestOrderContractFileIsCreating).ToString()), "");
                }
            }
            var result = _orderRepository.UpdateSource(id, tradingProviderId, username);

            // Cập nhật lịch sử chuyển online
            _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(id, orderFind.Source.ToString(), ContractSources.ONLINE.ToString(), InvestFieldName.UPDATE_ORDER_SOURCE
                , InvestHistoryUpdateTables.INV_ORDER, ActionTypes.CAP_NHAT, "Cập nhật chuyển online", DateTime.Now), username);
            _dbContext.SaveChanges();
            //sinh lại hợp đồng online
            await UpdateContractFileUpdateSource(id);
            return result;
        }

        public async Task<int> OrderApprove(int id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var orderFind = _orderRepository.FindById(id, tradingProviderId)
                            ?? throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            int result;
            var projectFind = _projectRepository.FindById(orderFind.ProjectId)
                            ?? throw new FaultException(new FaultReason($"Không tìm thấy thông tin của dự án"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            var policyDetailFind = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyDetailNotFound);
            var distribution = _distributionRepository.FindById(orderFind.DistributionId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);

            var policy = _dbContext.InvestPolicies.FirstOrDefault(p => p.Id == orderFind.PolicyId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound);
            // Chính sách bị khóa thì không được phép duyệt hợp đồng
            if (policy.Status == Status.INACTIVE)
            {
                _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderApproveCuzPolicyDeactive);
            }

            // Hạn mức tối đa của phân phối
            var maxTotalInvestment = _investOrderRepository.MaxTotalInvestment(orderFind.TradingProviderId, orderFind.DistributionId);
            if (maxTotalInvestment < 0)
            {
                throw new FaultException(new FaultReason($"Số tiền đặt lệnh lớn hơn hạn mức còn lại"), new FaultCode(((int)ErrorCode.InvestOrderExcceedRemailLimitDistribution).ToString()), "");
            }
            else
            {
                result = _orderRepository.OrderApprove(id, tradingProviderId, username);
            }

            if (maxTotalInvestment <= 0)
            {
                _distributionRepository.IsShowApp(orderFind.DistributionId, YesNo.NO, tradingProviderId);
            }
            var orderAfterApprove = _investOrderEFRepository.FindById(id, tradingProviderId);
            if (orderAfterApprove != null && orderAfterApprove.InvestDate != null && orderAfterApprove.Status == OrderStatus.DANG_DAU_TU)
            {
                // Tính ngày đáo hạn lưu vào Order
                DateTime dueDate = _orderRepository.CalculateDueDate(policyDetailFind, orderAfterApprove.InvestDate.Value, distribution.CloseCellDate);
                orderAfterApprove.DueDate = dueDate;
            }
            // Tìm cifCode nếu là nhà đầu tư cá nhân, thêm quan hệ với sale
            var findCifCode = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
            if (findCifCode != null && findCifCode.InvestorId != null)
            {
                // Thêm quan hệ InvestorSale
                var referralCode = orderFind.SaleReferralCodeSub ?? orderFind.SaleReferralCode;
                var saleFind = _saleEFRepository.FindSaleByReferralCode(referralCode);
                if (saleFind != null)
                {
                    // insert bản ghi investorSale
                    _investorSaleEFRepository.InsertInvestorSale(new Entities.DataEntities.InvestorSale
                    {
                        InvestorId = findCifCode.InvestorId ?? 0,
                        SaleId = saleFind.SaleId,
                        ReferralCode = referralCode
                    });
                }
            }
            // Lưu lại thời gian
            var profitInfo = ExpectedCashFlow(id);
            foreach (var item in profitInfo.ProfitInfo)
            {
                _investOrderRepository.AddPaymentDate(id, profitInfo.ProfitInfo.IndexOf(item) + 1, item.PayDate);
            }
            _dbContext.SaveChanges();
            if (orderFind.SaleReferralCode != null)
            {
                await _investSendEmailServices.SendNotifySaleInvestOrderActive(id, DateTime.Now);
            }
            return result;
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

        public async Task<int> ChangeDeliveryStatusReceviredApp(string deliveryCode)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = _orderRepository.ChangeDeliveryStatusReceviredApp(deliveryCode, investorId, modifiedBy);
            await _investSendEmailServices.SendNotifyQRContractDelivery(deliveryCode);
            return result;
        }

        public async Task<decimal> ChangeDeliveryStatusRecevired(string deliveryCode, string otp)
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
            await _investSendEmailServices.SendNotifyQRContractDelivery(deliveryCode);
            return verifyOtpResult;
        }

        public void VerifyPhone(string deliveryCode, string phone, int tradingProviderId)
        {
            var phoneDto = _orderRepository.GetPhoneByDeliveryCode(deliveryCode);
            if (phoneDto.Phone == phone)
            {
                _investorRepository.GenerateOtpByPhone(phone);
            }
            else
            {
                throw new FaultException(new FaultReason($"Xác nhận số điện thoại không chính xác"), new FaultCode(((int)ErrorCode.InvestorOTPExpire).ToString()), "");
            }
        }

        public int OrderCancel(int id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _orderRepository.OrderCancel(id, tradingProviderId, username);

            // Cập nhật lịch sử hủy duyệt hợp đồng
            _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(id, OrderStatus.DANG_DAU_TU.ToString(), OrderStatus.CHO_DUYET_HOP_DONG.ToString(), InvestFieldName.UPDATE_ORDER_STATUS
                , InvestHistoryUpdateTables.INV_ORDER, ActionTypes.CAP_NHAT, "Cập nhật hủy duyệt hợp đồng", DateTime.Now), username);
            _dbContext.SaveChanges();
            return -1;
        }

        public int UpdateInvestorBankAccount(int id, int? investorBankAccId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            return _orderRepository.UpdateInvestorBankAccount(id, investorBankAccId, tradingProviderId, userName);
        }

        public int UpdateInfoCustomer(int id, int? investorBankAccId, int? contractAddressId, int? investorIdenId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            return _orderRepository.UpdateInfoCustomer(id, investorBankAccId, contractAddressId, investorIdenId, tradingProviderId, userName);
        }

        public OrderContractFile AddOrderContractFile(CreateOrderContractFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderContractFile = new OrderContractFile()
            {
                TradingProviderId = tradingProviderId,
                OrderId = input.OrderId,
                ContractTempId = input.ContractTempId,
                FileScanUrl = input.FileScanUrl,
                FileSignatureUrl = input.FileSignatureUrl,
                FileTempUrl = input.FileTempUrl,
                CreatedBy = username
            };
            return _orderContractFileRepository.Add(orderContractFile);
        }

        public void UpdateOrderContractFile(UpdateOrderContractFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var orderContract = _investOrderContractFileEFRepository.FindById(input.OrderContractFileId).ThrowIfNull(_dbContext, ErrorCode.InvestOrderContractFileNotFound);
            orderContract.FileScanUrl = input.FileScanUrl;
            orderContract.ModifiedBy = username;
            orderContract.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// App lấy theo trạng thái đang đầu tư
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public List<AppInvestOrderInvestorDto> AppOrderGetAll(int groupStatus)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var findOrderList = _orderRepository.AppGetAll(investorId, groupStatus);

            DateTime ngayHienTai = DateTime.Now.Date;
            var result = new List<AppInvestOrderInvestorDto>();
            foreach (var item in findOrderList)
            {
                try
                {
                    var order = _orderRepository.FindById(item.Id, null);
                    var projectFind = _projectRepository.FindById(order.ProjectId, null);
                    if (projectFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                    }
                    item.InvCode = projectFind.InvCode;
                    item.IconProject = projectFind.Image;
                    item.InitTotalValue = order.InitTotalValue;

                    var policyFind = _policyRepository.FindPolicyById(order.PolicyId, order.TradingProviderId);
                    if (policyFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                    }

                    var distributionFind = _distributionRepository.FindById(order.DistributionId, order.TradingProviderId);
                    if (distributionFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin phân phối sản phẩm"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                    }
                    item.PolicyName = policyFind.Name;

                    var policyDetailFind = _policyRepository.FindPolicyDetailById(order.PolicyDetailId, order.TradingProviderId);
                    if (policyDetailFind == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông kỳ hạn"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                    }
                    item.ContractCode = order.ContractCode;
                    item.Interest = policyDetailFind.Profit;
                    item.InterestPeriodType = policyDetailFind.InterestPeriodType;
                    item.Profit = 0;

                    //Nếu là đang đầu tư, tính xem số ngày còn lại
                    if (item.Status == OrderStatus.DANG_DAU_TU)
                    {
                        DateTime ngayDaoHan = order.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.PaymentFullDate ?? DateTime.Now.Date, distributionFind.CloseCellDate);
                        item.TimeRemaining = ngayDaoHan.Subtract(ngayHienTai).Days;
                        if (item.TimeRemaining < 0)
                        {
                            item.TimeRemaining = 0;
                        }
                        var profitNow = _investSharedServices.ProfitNow(item.PaymentFullDate ?? DateTime.Now.Date, ngayDaoHan, policyDetailFind.Profit ?? 0, item.TotalValue ?? 0);
                        item.Profit = profitNow;
                    }

                    //Nếu là tất toán, kiểm tra xem tất toán trước hạn hay đúng hạn
                    else if (item.Status == OrderStatus.TAT_TOAN)
                    {
                        // Tính ngày đáo hạn
                        DateTime ngayDaoHan = order.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.PaymentFullDate ?? DateTime.Now.Date, distributionFind.CloseCellDate);

                        // Nếu ngày đáo hạn bé hơn ngày đáo hạn thực tế, thì là Tất toán trước hạn
                        if (order.SettlementDate < ngayDaoHan)
                        {
                            item.IsPrepayment = true;
                        }
                        // Lấy danh sách đã chi trả của hợp đồng
                        var listInterestPayment = _interestPaymentRepository.InterestPaymentGetList(item.Id, OrderPaymentStatus.DA_THANH_TOAN);
                        item.TotalValue = order.InitTotalValue;
                        // Tổng lợi nhuận đã được chi cho hợp đồng
                        item.Profit = listInterestPayment?.Sum(p => p.AmountMoney);
                    }
                    result.Add(item);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không tìm thấy thông tin sổ lệnh {item.Id} ");
                }
            }
            return result;
        }

        /// <summary>
        /// Nhà đầu tư xem thông tin hợp đồng sổ lệnh 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<AppInvestOrderInvestorDetailDto> AppOrderInvestorDetail(int orderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            return await _investOrderShareServices.ViewOrderDetail(orderId, investorId);
        }

        /// <summary>
        /// Tính toán dòng tiền dự tính
        /// </summary>
        /// <param name="orderId"></param>
        public ProfitAndInterestDto ExpectedCashFlow(int orderId)
        {
            #region Lấy Id từ các bảng
            //lấy thông tin lệnh
            var order = _orderRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

            var cifFind = _cifCodeRepository.GetByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            bool khachCaNhan = cifFind.InvestorId != null;

            var policyDetail = _policyRepository.FindPolicyDetailById(order.PolicyDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyDetailNotFound);
            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound);

            //Lấy thông tin bán theo kỳ hạn
            var distribution = _distributionRepository.FindById(policy.DistributionId, policy.TradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);

            //lấy thông tin dự án
            var project = _projectRepository.FindById(distribution.ProjectId, null)
                .ThrowIfNull(_dbContext, ErrorCode.InvestProjectNotFound);
            #endregion

            //tính lợi tức
            DateTime ngayBatDauTinhLai = DateTime.Now.Date;
            if (order.PaymentFullDate != null)
            {
                ngayBatDauTinhLai = order.PaymentFullDate.Value;
            }
            ngayBatDauTinhLai = (order.InvestDate != null) ? order.InvestDate.Value : ngayBatDauTinhLai;

            //Tổng giá trái phiếu giao dịch (số tiền đầu tư)
            decimal soTienDauTu = order.TotalValue;

            ProfitAndInterestDto result = _investSharedServices.CalculateListInterest(project, policy, policyDetail, ngayBatDauTinhLai, soTienDauTu, khachCaNhan, distribution.CloseCellDate, orderId);
            return result;
        }
        public InvestOrderCashFlowDto GetProfitInfo(int orderId)
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

            InvestOrderCashFlowDto result = new();

            #region Lấy Id từ các bảng
            //lấy thông tin lệnh
            var order = _orderRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);

            var cifFind = _cifCodeRepository.GetByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            bool khachCaNhan = cifFind.InvestorId != null;

            var policyDetail = _policyRepository.FindPolicyDetailById(order.PolicyDetailId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyDetailNotFound);
            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound);
            //Lấy thông tin bán theo kỳ hạn
            var distribution = _distributionRepository.FindById(policy.DistributionId, policy.TradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
            #endregion

            result.ExpectedCashFlow = ExpectedCashFlow(orderId);

            var withdrawal = _withdrawalSettlementRepository.WithdrawalGetList(orderId, order.TradingProviderId).OrderBy(w => w.WithdrawalDate);
            List<InvestOrderInterestAndWithdrawalDto> actualCashFlow = new();

            // Dòng tiền thực tế khi rút tiền
            var stt = 0;
            decimal totalValue = order.InitTotalValue;
            foreach (var item in withdrawal)
            {
                stt = ++stt;
                InvestOrderInterestAndWithdrawalDto withdrawalItem = new();
                // Nếu đã duyệt tiền thì trừ đi số tiền lấy số dư tại thời điểm được rút
                if (item.Status == WithdrawalStatus.DUYET_DI_TIEN || item.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                {
                    // Chung 1 trạng thái là đã đi tiền(đã duyệt)
                    item.Status = WithdrawalStatus.DUYET_DI_TIEN;

                    withdrawalItem = new InvestOrderInterestAndWithdrawalDto
                    {
                        Description = $"Rút vốn lần {stt}",
                        PayDate = item.WithdrawalDate,
                        NumberOfDays = (item.WithdrawalDate - order.InvestDate.Value.Date).Days,
                        Surplus = totalValue,
                        WithdrawalMoney = item.AmountMoney ?? 0,
                        Profit = item.Profit,
                        ActuallyAmount = item.ActuallyAmount ?? 0,
                        DeductibleProfit = item.DeductibleProfit,
                        Tax = item.Tax,
                        ActuallyProfit = item.ActuallyProfit,
                        Status = item.Status
                    };
                    totalValue -= (item.AmountMoney ?? 0);
                }
                else
                {
                    // Chờ phản hồi = Trạng thái yêu cầu (Khởi tạo)
                    if (item.Status == WithdrawalStatus.CHO_PHAN_HOI)
                    {
                        item.Status = WithdrawalStatus.YEU_CAU;
                    }
                    var withdrawalInfo = _investSharedServices.RutVonInvest(order, policy, policyDetail, totalValue, item.AmountMoney ?? 0, item.WithdrawalDate, khachCaNhan, distribution.CloseCellDate);
                    withdrawalItem = new InvestOrderInterestAndWithdrawalDto
                    {
                        Description = $"Rút vốn lần {stt}",
                        PayDate = item.WithdrawalDate,
                        NumberOfDays = (item.WithdrawalDate - order.InvestDate.Value.Date).Days,
                        Surplus = withdrawalInfo.Surplus ?? 0,
                        WithdrawalMoney = item.AmountMoney ?? 0,
                        Profit = withdrawalInfo.ActuallyProfit,
                        ActuallyAmount = withdrawalInfo.ActuallyAmount,
                        DeductibleProfit = withdrawalInfo.ProfitReceived,
                        Tax = withdrawalInfo.IncomeTax,
                        ActuallyProfit = withdrawalInfo.ActuallyProfit,
                        Status = item.Status
                    };
                }
                actualCashFlow.Add(withdrawalItem);
            }

            // Dòng tiền thực tế khi chi trả
            var interestPayment = _interestPaymentRepository.InterestPaymentGetList(order.Id);
            foreach (var item in interestPayment)
            {
                if (item.Status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || item.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
                {
                    // Chung 1 trạng thái là đã đi tiền(đã duyệt)
                    item.Status = InterestPaymentStatus.DA_DUYET_CHI_TIEN;
                }
                // Chờ phản hồi = Trạng thái yêu cầu (Khởi tạo)
                else if (item.Status == InterestPaymentStatus.CHO_PHAN_HOI)
                {
                    item.Status = InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA;
                }

                var description = (item.IsLastPeriod == YesNo.YES) ? "Tiền nhận cuối kỳ" : $"Lợi tức kỳ {item.PeriodIndex}";
                if (order.Status == OrderStatus.TAT_TOAN && item.IsLastPeriod == YesNo.YES && order.SettlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC)
                {
                    description = "Tái tục gốc";
                }
                else if (order.Status == OrderStatus.TAT_TOAN && item.IsLastPeriod == YesNo.YES && order.SettlementMethod == SettlementMethod.TAI_TUC_GOC_VA_LOI_NHUAN)
                {
                    description = "Tái tục gốc + LN";
                }
                actualCashFlow.Add(new InvestOrderInterestAndWithdrawalDto
                {
                    // Nếu là kỳ cuối thì nội dung là Tiền nhận cuối kỳ
                    Description = description,
                    PayDate = item.PayDate,
                    NumberOfDays = (item.PayDate.Value.Date - order.InvestDate.Value.Date).Days,
                    Surplus = order.TotalValue,
                    WithdrawalMoney = item.TotalValueInvestment,
                    Profit = item.Profit,
                    ActuallyAmount = (item.Status == InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA) ? (item.AmountMoney + item.TotalValueInvestment) : item.AmountMoney,
                    Tax = item.Tax ?? 0,
                    ActuallyProfit = item.AmountMoney,
                    Status = item.Status
                });
            }
            result.ActualCashFlow = actualCashFlow;
            return result;
        }

        public AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyDetailId)
        {
            var policyDetailFind = _policyRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetailFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            return _mapper.Map<AppSaleByReferralCodeDto>(_saleRepository.FindSaleByReferralCode(referralCode, policyDetailFind.TradingProviderId));
        }
        public PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode)
        {
            var result = _orderRepository.GetPhoneByDeliveryCode(deliveryCode);
            if (result == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy số điện thoại của nhà đầu tư"), new FaultCode(((int)ErrorCode.InvestorPhoneExisted).ToString()), "");
            }
            var lastThreeChar = result.Phone.Substring(result.Phone.Length - 3);
            var firstChar = new string('*', result.Phone.Length - 3);
            result.Phone = firstChar + lastThreeChar;
            return result;
        }
        public InvestInterestCalculationDateDto CheckInvestmentDay(int policyDetailId, DateTime ngaybatdau)
        {
            var result = new InvestInterestCalculationDateDto();
            var policyDetailFind = _policyRepository.FindPolicyDetailById(policyDetailId);

            var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyDetailFind.DistributionId && r.Deleted == YesNo.NO);
            var ngayDauTu = _investSharedServices.NextWorkDay(ngaybatdau, policyDetailFind.TradingProviderId);

            var ngayKetThuc = _investSharedServices.CalculateDueDate(policyDetailFind, ngayDauTu, distribution?.CloseCellDate);
            result.StartDate = ngayDauTu;
            result.EndDate = ngayKetThuc;
            return result;
        }

        #region chi trả, rút vốn
        public RutVonDto AppViewThayDoiKhiRutVon(long orderId, decimal soTienRut)
        {
            return ThongTinMoTaRutVon(orderId, soTienRut, DateTime.Now.Date);
        }

        public RutVonDto RutVon(long orderId, decimal soTienRut, DateTime ngayRut)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return ThongTinMoTaRutVon(orderId, soTienRut, ngayRut, tradingProviderId);
        }

        public RutVonDto ThongTinMoTaRutVon(long orderId, decimal soTienRut, DateTime ngayRut, int? tradingProviderId = null)
        {
            #region Lấy Id từ các bảng
            //lấy thông tin lệnh
            var orderFind = _orderRepository.FindById(orderId, tradingProviderId);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }

            if (orderFind.Status != InvestOrderStatus.DANG_DAU_TU)
            {
                throw new FaultException(new FaultReason($"Hợp đồng không trong trạng thái hoạt động không thể rút vốn"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var cifFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
            if (cifFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mã cif"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            bool khachCaNhan = cifFind.InvestorId != null;

            var policyDetail = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policy.DistributionId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
            #endregion

            //Số tiền đang còn được đầu tư của hợp đồng
            var soTienDangDauTu = orderFind.TotalValue;

            var result = _investSharedServices.RutVonInvest(orderFind, policy, policyDetail, soTienDangDauTu, soTienRut, ngayRut, khachCaNhan, distribution.CloseCellDate);
            // App không hiện thuế
            result.IncomeTax = 0;
            return result;
        }

        public List<RutVonDto> TheoDoiRutTruocHan(long orderId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            //Danh sách thông tin đã được lập để rút vốn
            List<RutVonDto> danhSachDaRutVon = new();
            #region Lấy Id từ các bảng
            //lấy thông tin lệnh
            var orderFind = _orderRepository.FindById(orderId, tradingProviderId);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }

            var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
            if (cifCodeFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mã cif"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            //Nếu là khách hàng cá nhân
            bool khachHangCaNhan = cifCodeFind.InvestorId != null;

            var policyDetail = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }
            var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policy.DistributionId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
            #endregion

            //Số tiền còn đầu tư sau khi đã rút vốn // Mặc định là số tiền ban đầu
            var soTienConLaiSauRut = orderFind.InitTotalValue;

            //Lấy danh sách lịch sử rút vốn của sổ lệnh trong trạng thái ĐÃ CHI TRẢ
            var listLichSuRutVon = _withdrawalSettlementRepository.WithdrawalGetList(orderId, tradingProviderId, InvestWithdrawalStatus.DA_CHI_TRA).OrderBy(r => r.WithdrawalDate);
            listLichSuRutVon = listLichSuRutVon.OrderBy(rv => rv.WithdrawalDate);
            var stt = 0;
            foreach (var item in listLichSuRutVon)
            {
                stt = ++stt;

                //Số tiền còn đầu tư sau khi các đợt rút vốn
                soTienConLaiSauRut -= item.AmountMoney ?? 0;

                //Lấy ra thông tin còn lại của sổ lệnh sau khi rút
                var ketQuaSauRut = _investSharedServices.RutVonInvest(orderFind, policy, policyDetail, soTienConLaiSauRut, item.AmountMoney ?? 0, item.WithdrawalDate, khachHangCaNhan, distribution.CloseCellDate);
                ketQuaSauRut.WithdrawalIndex = stt;
                danhSachDaRutVon.Add(ketQuaSauRut);
            }
            danhSachDaRutVon = danhSachDaRutVon.OrderByDescending(r => r.WithdrawlDate).ToList();
            return danhSachDaRutVon;
        }

        /// <summary>
        /// Lấy danh sách ngày chi trả của hợp đồng
        /// </summary>
        /// <param name="ngayChiTra"></param>
        /// <param name="isExactDate">Có lọc theo ngày chính xác hay không (Y: lọc chính xác ngày, N: lọc bằng ngày hoặc bé hơn)</param>
        /// <param name="phone"></param>
        /// <param name="contractCode"></param>
        /// <param name="taxCode"></param>
        /// <returns></returns>
        public async Task<List<ThoiGianChiTraThucDto>> LayDanhSachNgayChiTra(FilterCalulationInterestPayment input, bool isAllSystem = false)
        {
            List<InvestOrderDataForInvestPaymentDto> listOrder = new();
            // Lấy hết các hợp đồng đang active
            if (isAllSystem)
            {
                /*listOrder = _investOrderEFRepository.Entity
                    .Where(o => o.Status == OrderStatus.DANG_DAU_TU && o.Deleted == YesNo.NO && (input.MethodInterest == null || o.MethodInterest == input.MethodInterest))
                    .ToList();*/
            }
            else
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
                    _dbContext.CheckTradingRelationshipPartner(partnerId, input.TradingProviderChildIds);
                }
                // Nếu không phải Parner thì tradingProviderChildIds = null
                input.TradingProviderChildIds = (partnerId != null) ? input.TradingProviderChildIds : null;
                //listOrder = _orderRepository.GetAllListOrder(tradingProviderId, InvestOrderStatus.DANG_DAU_TU, phone, contractCode, taxCode, null, projectId, tradingProviderChildIds, methodInterest);
                listOrder = _investOrderEFRepository.GetAllListOrderCalulationInterestPayment(new FilterCalulationInterestPayment
                {
                    Phone = input.Phone,
                    ContractCode = input.ContractCode,
                    TaxCode = input.TaxCode,
                    ProjectId = input.ProjectId,
                    TradingProviderChildIds = input.TradingProviderChildIds,
                    MethodInterest = input.MethodInterest,
                    CifCode = input.CifCode,
                    IdNo = input.IdNo,
                    SettlementMethod = input.SettlementMethod,
                }, tradingProviderId, partnerId).ToList();
            }

            List<Task> tasks = new();
            List<ThoiGianChiTraThucDto> result = new();
            ConcurrentBag<ThoiGianChiTraThucDto> concurrentBagResult = new();
            foreach (var orderItem in listOrder)
            {
                var task = Task.Run(() =>
                {
                    var dbContext = CommonUtils.GetService<EpicSchemaDbContextTransient>(_httpContext);
                    var investInterestPaymentEFRepository = new InvestInterestPaymentEFRepository(dbContext, _logger);
                    var investCalendarEFRepository = new InvestCalendarEFRepository(dbContext, _logger);
                    var policy = dbContext.InvestPolicies.Any(p => p.Id == orderItem.PolicyId && p.Deleted == YesNo.NO);
                    if (!policy) return;

                    List<ThoiGianChiTraThucDto> listThoiGianChiTraTrongHopDong = new();

                    //Lấy kỳ hạn
                    var policyDetailFind = dbContext.InvestPolicyDetails.FirstOrDefault(p => p.Id == orderItem.PolicyDetailId && p.TradingProviderId == orderItem.TradingProviderId && p.Deleted == YesNo.NO);
                    if (policyDetailFind == null) return;

                    // Tìm kiếm phân phối
                    var distribution = dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyDetailFind.DistributionId && r.Deleted == YesNo.NO);
                    if (distribution == null) return;

                    //Lấy ngày bắt đầu tính lãi
                    var ngayDauKy = orderItem.InvestDate.Value.Date;

                    // Lấy ngày đáo hạn lưu trong hợp đồng nếu chưa có thì tính
                    DateTime ngayDaoHan = orderItem.DueDate ?? investInterestPaymentEFRepository.CalculateDueDate(policyDetailFind, ngayDauKy, distribution.CloseCellDate);

                    if (new int[] { InterestTypes.NGAY_CO_DINH, InterestTypes.DINH_KY, InterestTypes.NGAY_CUOI_THANG, InterestTypes.NGAY_DAU_THANG }.Contains(policyDetailFind.InterestType ?? 0))
                    {
                        //Tính thời gian thực của các kỳ trả
                        List<DateTime> thoiGianThuc = investInterestPaymentEFRepository.GetListPayDateByPolicyDetail(policyDetailFind, ngayDauKy, ngayDaoHan);

                        //Thời gian thực tính tiền...
                        for (int j = 0; j < thoiGianThuc.Count; j++)
                        {
                            bool isLastPeriod = j == thoiGianThuc.Count - 1;

                            // Kiểm tra xem có kỳ chi trả trước đó đã được lập hay chưa
                            var kyDaLapTruocDo = dbContext.InvestInterestPayments.Where(p => p.OrderId == orderItem.Id && p.PayDate < thoiGianThuc[j] && p.PeriodIndex == j
                                                    && p.Status != InterestPaymentStatus.HUY_DUYET && p.Deleted == YesNo.NO).OrderByDescending(p => p.PeriodIndex).Select(p => p.PayDate).FirstOrDefault();

                            // Tính số ngày của kỳ đầu tiên 
                            int soNgay = (thoiGianThuc[j] - orderItem.InvestDate.Value.Date).Days;

                            // Nếu kỳ trước đó đã được lập (Ngày của Kỳ hiện tại - Ngày của kỳ trước đó đã lập)
                            if (kyDaLapTruocDo != null)
                            {
                                soNgay = (thoiGianThuc[j] - kyDaLapTruocDo.Value.Date).Days;
                            }
                            else if (kyDaLapTruocDo == null && j > 0)
                            {
                                soNgay = (thoiGianThuc[j] - thoiGianThuc[j - 1]).Days;
                            }

                            listThoiGianChiTraTrongHopDong.Add(new ThoiGianChiTraThucDto
                            {
                                PayDate = thoiGianThuc[j],
                                LastPayDate = j > 0 ? thoiGianThuc[j - 1] : thoiGianThuc[j],
                                InvestLastPayDate = kyDaLapTruocDo,
                                OrderId = orderItem.Id,
                                PolicyDetailId = policyDetailFind.Id,
                                PeriodIndex = j + 1,
                                SoNgay = soNgay,
                                ContractCode = orderItem.ContractCode,
                                TradingProviderId = orderItem.TradingProviderId,
                                CifCode = orderItem.CifCode,
                                PolicyId = policyDetailFind.PolicyId,
                                TotalValue = orderItem.TotalValue,
                                IsLastPeriod = isLastPeriod,
                                DueDate = ngayDaoHan,
                                CustomerName = orderItem.CustomerName,
                                Profit = policyDetailFind.Profit
                            });
                        }
                    }
                    else if (policyDetailFind.InterestType == InterestTypes.CUOI_KY) //Trả cuối kỳ
                    {
                        int soNgay = ngayDaoHan.Subtract(ngayDauKy.Date).Days;
                        listThoiGianChiTraTrongHopDong.Add(new ThoiGianChiTraThucDto
                        {
                            PayDate = ngayDaoHan,
                            LastPayDate = ngayDaoHan,
                            PolicyDetailId = policyDetailFind.Id,
                            OrderId = orderItem.Id,
                            PeriodIndex = 0,
                            SoNgay = soNgay,
                            ContractCode = orderItem.ContractCode,
                            TradingProviderId = orderItem.TradingProviderId,
                            CifCode = orderItem.CifCode,
                            TotalValue = orderItem.TotalValue,
                            IsLastPeriod = true,
                            DueDate = ngayDaoHan,
                            CustomerName = orderItem.CustomerName,
                            Profit = policyDetailFind.Profit,
                            PolicyId = orderItem.PolicyId,
                        });
                    }

                    //Lấy ra danh sách đã lập chi trả của hợp đồng sổ lệnh này
                    var listCacKyChiTraDaLap = dbContext.InvestInterestPayments.Where(p => p.OrderId == orderItem.Id && p.TradingProviderId == orderItem.TradingProviderId
                                                    && p.Status != InterestPaymentStatus.HUY_DUYET && p.Deleted == YesNo.NO).Select(r => r.PeriodIndex);

                    //Lọc ra những kỳ đã đến ngày chọn chi trả mà chưa được lập chi trả
                    listThoiGianChiTraTrongHopDong = listThoiGianChiTraTrongHopDong.Where(x => !listCacKyChiTraDaLap.Contains(x.PeriodIndex)).ToList();
                    //test.Concat(listThoiGianChiTraTrongHopDong);
                    foreach (var item in listThoiGianChiTraTrongHopDong)
                    {

                        concurrentBagResult.Add(item);
                    }

                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            result = concurrentBagResult.OrderByDescending(x => x.PayDate).ThenBy(x => x.OrderId).ToList();
            if (input.IsExactDate == YesNo.YES)
            {
                result = result.Where(r => (input.NgayChiTra == null ? r.PayDate == DateTime.Now : r.PayDate == input.NgayChiTra))
                .OrderByDescending(r => r.PayDate.Date).ToList();
            }
            else if (input.IsExactDate == YesNo.NO)
            {
                result = result.Where(r => (input.NgayChiTra == null ? r?.PayDate <= DateTime.Now : r?.PayDate <= input.NgayChiTra))
                    .OrderByDescending(r => r.PayDate.Date).ToList();
            }
            return result;
        }

        /// <summary>
        /// Hiện danh sách phân trang để lập chi, 
        /// isLastPeriod = true thì chỉ lấy là kỳ cuối, còn false thì lấy các kỳ chi trả bình thường
        /// </summary>
        /// <returns></returns>
        public async Task<PagingResult<DanhSachChiTraDto>> LapDanhSachChiTra(InterestPaymentFilterDto input, bool isLastPeriod)
        {
            var lapDanhSachChiTra = new List<DanhSachChiTraDto>();
            var result = new PagingResult<DanhSachChiTraDto>();
            var ngayChiTra = input.NgayChiTra != null ? input.NgayChiTra.Value.Date : DateTime.Now.Date;

            var danhSachNgayChiTra = await LayDanhSachNgayChiTra(new FilterCalulationInterestPayment
            {
                NgayChiTra = ngayChiTra,
                IsExactDate = input.IsExactDate,
                Phone = input.Phone,
                ContractCode = input.ContractCode,
                TaxCode = input.TaxCode,
                ProjectId = input.ProjectId,
                MethodInterest = input.MethodInterest,
                TradingProviderChildIds = input.TradingProviderIds,
                CifCode = input.CifCode,
                IdNo = input.IdNo,
                SettlementMethod = input.SettlementMethod
            });

            danhSachNgayChiTra = danhSachNgayChiTra.Where(p => p.IsLastPeriod == isLastPeriod).ToList();

            result.TotalItems = danhSachNgayChiTra.Count();
            if (input.Sort == null)
            {
                danhSachNgayChiTra = danhSachNgayChiTra.Skip(input.Skip).Take(input.PageSize).ToList();
            }

            foreach (var item in danhSachNgayChiTra)
            {
                var orderFind = _dbContext.InvOrders.FirstOrDefault(o => o.Id == item.OrderId && o.Deleted == YesNo.NO);
                if (orderFind == null) continue;
                //Lấy thông tin chính sách
                var policyFind = _dbContext.InvestPolicies.FirstOrDefault(p => p.Id == orderFind.PolicyId && p.Deleted == YesNo.NO);
                if (policyFind == null) continue;

                var policyDetail = _dbContext.InvestPolicyDetails.FirstOrDefault(i => i.Id == orderFind.PolicyDetailId);

                var projectFind = _dbContext.InvestProjects.FirstOrDefault(p => p.Id == orderFind.ProjectId && p.Deleted == YesNo.NO);

                DanhSachChiTraDto resultItem = new DanhSachChiTraDto
                {
                    PayDate = item.PayDate,
                    PolicyDetailId = item.PolicyDetailId,
                    OrderId = item.OrderId,
                    CifCode = item.CifCode,
                    PeriodIndex = item.PeriodIndex,
                    SoNgay = item.SoNgay,
                    ContractCode = item.ContractCode,
                    PeriodType = policyDetail?.PeriodType,
                    PeriodQuantity = policyDetail?.PeriodQuantity,
                    PolicyDetailName = policyDetail.Name,
                    TotalValue = item.TotalValue ?? 0,
                    OrderStatus = orderFind.Status,
                    DistributionId = orderFind.DistributionId,
                    Name = item.CustomerName,
                    InvCode = projectFind.InvCode,
                    InvName = projectFind.InvName
                };

                decimal? loiTucKyNay = 0;
                decimal? thue = 0;
                decimal? tongTienThucNhan = 0;
                decimal? tongTienDangDauTuChoKyCuoi = 0; //được sử dụng nếu là kỳ cuối thì là vốn đang đầu tư
                decimal thueLoiNhuan = 0;

                //Lấy CifCode nếu là Khách hàng cá nhân thì tính thuế
                var cifCodeFind = _dbContext.CifCodes.FirstOrDefault(c => c.CifCode == item.CifCode && c.Deleted == YesNo.NO);
                if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    thueLoiNhuan = (policyFind.IncomeTax ?? 0) / 100;
                    resultItem.InvestorBank = _investorBankAccountEFRepository.GetBankById(orderFind.InvestorBankAccId ?? 0);
                }
                else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    resultItem.BusinessCustomerBank = _businessCustomerEFRepository.FindBankById(orderFind.InvestorBankAccId ?? 0);
                }
                resultItem.GenContractCode = _investOrderEFRepository.GetContractCodeGen(item.OrderId);

                var renewalsRequest = new InvRenewalsRequestDto();

                //(bỏ qua) Tính số tiền chi trả của kỳ trước đó nếu có (Bỏ qua GROSS hoặc NET)
                var soTienCuaKyTruoc = 0; //item.TotalValue * (item.Profit / 100) * item.PayDate.Date.Subtract(item.LastPayDate.Value.Date).Days / 365;
                if (!item.IsLastPeriod)
                {
                    //Chi trả lợi tức chưa bao gồm thuế`
                    if (policyFind.CalculateType == CalculateTypes.GROSS)
                    {
                        loiTucKyNay = (item.TotalValue * (item.Profit / 100) * item.SoNgay / 365) - soTienCuaKyTruoc;
                        thue = loiTucKyNay * thueLoiNhuan;
                        tongTienThucNhan = loiTucKyNay - thue;
                    }

                    //Chi trả lợi tức đã bao gồm thuế
                    else if (policyFind.CalculateType == CalculateTypes.NET)
                    {
                        tongTienThucNhan = (item.TotalValue * (item.Profit / 100) * item.SoNgay / 365) - soTienCuaKyTruoc;
                        loiTucKyNay = tongTienThucNhan / (1 - thueLoiNhuan);
                        thue = loiTucKyNay - tongTienThucNhan;
                    }
                }
                //Nếu là kỳ cuối
                else if (item.IsLastPeriod)
                {
                    decimal? loiTucCaKyThucTe = 0;

                    //Chi trả lợi tức chưa bao gồm thuế
                    if (policyFind.CalculateType == CalculateTypes.GROSS)
                    {
                        loiTucKyNay = (item.TotalValue * (item.Profit / 100) * item.SoNgay / 365) - soTienCuaKyTruoc;
                        thue = loiTucKyNay * thueLoiNhuan;
                        loiTucCaKyThucTe = loiTucKyNay - thue;
                    }

                    //Chi trả lợi tức đã bao gồm thuế
                    else if (policyFind.CalculateType == CalculateTypes.NET)
                    {
                        loiTucCaKyThucTe = (item.TotalValue * (item.Profit / 100) * item.SoNgay / 365) - soTienCuaKyTruoc;
                        loiTucKyNay = loiTucCaKyThucTe / (1 - thueLoiNhuan);
                        thue = loiTucKyNay - loiTucCaKyThucTe;
                    }

                    //số tiền kỳ cuối nhận được
                    tongTienThucNhan = loiTucCaKyThucTe;
                    tongTienDangDauTuChoKyCuoi = item.TotalValue;

                    // Kiểm tra xem có yêu cầu tái tục hay không/ Lấy yêu cầu tái tục mới nhất
                    var renewalsRequestFind = _dbContext.InvestRenewalsRequests.Where(r => r.OrderId == item.OrderId && r.SettlementMethod != SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && r.Deleted == YesNo.NO
                                                && (r.Status == InvestRenewalsRequestStatus.KHOI_TAO || r.Status == InvestRenewalsRequestStatus.DA_DUYET)).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (renewalsRequestFind != null)
                    {
                        var renewarsPolicyDetail = _dbContext.InvestPolicyDetails.FirstOrDefault(p => p.Id == renewalsRequestFind.RenewalsPolicyDetailId && p.Deleted == YesNo.NO);
                        if (renewarsPolicyDetail != null)
                        {
                            renewalsRequest.Id = renewalsRequestFind.Id;
                            renewalsRequest.OrderId = renewalsRequestFind.OrderId;
                            renewalsRequest.SettlementMethod = renewalsRequestFind.SettlementMethod;
                            renewalsRequest.RenewalsPolicyDetailId = renewalsRequestFind.RenewalsPolicyDetailId;
                            renewalsRequest.PolicyDetail = _mapper.Map<PolicyDetailDto>(renewarsPolicyDetail);
                            /*var renewarsPolicy = _policyRepository.FindPolicyById(renewarsPolicyDetail.PolicyId);
                            if (renewarsPolicy != null)
                            {
                                renewalsRequest.Policy = _mapper.Map<PolicyDto>(renewarsPolicy);
                            }*/
                        }
                    }
                }

                resultItem.PolicyCalculateType = policyFind.CalculateType;
                resultItem.AmountMoney = Math.Round(tongTienThucNhan ?? 0);
                resultItem.Profit = Math.Round(loiTucKyNay ?? 0);
                resultItem.ActuallyProfit = Math.Round(tongTienThucNhan ?? 0);
                resultItem.TotalValueInvestment = tongTienDangDauTuChoKyCuoi;
                resultItem.Tax = Math.Round(thue ?? 0);
                resultItem.IsLastPeriod = item.IsLastPeriod ? YesNo.YES : YesNo.NO;
                resultItem.RenewalsRequest = renewalsRequest;
                resultItem.TotalValueCurrent = item.TotalValue ?? 0;

                lapDanhSachChiTra.Add(resultItem);
            }

            //lapDanhSachChiTra = lapDanhSachChiTra.OrderByDescending(x => x.PayDate).ThenBy(x => x.OrderId).ToList();
            result.Items = lapDanhSachChiTra;
            if (input.Sort != null)
            {
                //Phân trang
                var orderedLapDanhSachChiTra = lapDanhSachChiTra.AsQueryable().OrderDynamic(input.Sort);
                orderedLapDanhSachChiTra = orderedLapDanhSachChiTra.Skip(input.Skip).Take(input.PageSize);
                result.Items = orderedLapDanhSachChiTra;
            }
            return result;
        }
        #endregion

        public int AppRequestDeliveryStatus(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            return _orderRepository.AppRequestDeliveryStatus(orderId, investorId, username);
        }

        public async Task<List<ThoiGianChiTraThucDto>> NoticePaymentDueDate()
        {
            DateTime now = DateTime.Now.Date;
            var result = new List<ThoiGianChiTraThucDto>();
            //Tìm đến trước ngày hiện tại xx Ngày để lọc
            var varValue = _sysVarRepository.GetVarByName(GrNames.ORDER, VarNames.DUE_DATE_FILTER_LENGTH)?.VarValue;
            if (varValue != null)
            {
                var afterDay = Int32.Parse(varValue);
                now = now.AddDays(+afterDay);
            }
            //Sysvar tính lại ngày
            var denHanChiTra = await LayDanhSachNgayChiTra(new FilterCalulationInterestPayment
            {
                NgayChiTra = now,
                IsExactDate = YesNo.NO,
            });
            denHanChiTra = denHanChiTra.Where(c => c.IsLastPeriod == IsLastPeriod.YES).ToList();
            // Nhóm theo cifCode của khách hàng
            var groupByCifCode = denHanChiTra.GroupBy(r => r.CifCode);
            foreach (var cifCodeItem in groupByCifCode)
            {
                // Check xem cifCode có tồn tại
                var findCifCode = _cifCodeRepository.GetByCifCode(cifCodeItem.Key);

                // Đếm xem cifCodeItem này chứa bao nhiêu hợp đồng sắp đến hạn
                var countPaymentDueDate = cifCodeItem.Count();
                if (findCifCode != null && findCifCode.InvestorId != null)
                {
                    var investorTodoFind = _investorToDoEFRepository.FindToDoByInvestorId(findCifCode.InvestorId ?? 0, InvestorToDoTypes.INVEST_DEN_HAN);
                    if (investorTodoFind == null)
                    {
                        _investorToDoEFRepository.Add(new CoreEntities.DataEntities.InvestorToDo
                        {
                            InvestorId = findCifCode.InvestorId ?? 0,
                            Type = InvestorToDoTypes.INVEST_DEN_HAN,
                            Detail = InvestorToDoDetails.Details(InvestorToDoTypes.INVEST_DEN_HAN, countPaymentDueDate)
                        });
                    }
                    else
                    {
                        investorTodoFind.Detail = InvestorToDoDetails.Details(InvestorToDoTypes.INVEST_DEN_HAN, countPaymentDueDate);
                    }
                    result.AddRange(cifCodeItem);
                    _dbContext.SaveChanges();
                }
            }
            return result;
        }

        public async Task NotifyPaymentDueDate()
        {
            _logger.LogInformation($"{nameof(OrderServices)}->{nameof(NotifyPaymentDueDate)}");
            DateTime now = DateTime.Now.Date;

            // Lấy toàn bộ danh sách hợp đồng active của cả hệ thống
            var denHanChiTra = await LayDanhSachNgayChiTra(new FilterCalulationInterestPayment
            {
                NgayChiTra = now
            }, true);
            denHanChiTra = denHanChiTra.Where(c => c.IsLastPeriod == IsLastPeriod.YES).ToList();
            var interestPaymentByPolicy = denHanChiTra.GroupBy(p => p.PolicyId);

            foreach (var policyItem in interestPaymentByPolicy)
            {
                var policyFind = _policyRepository.FindPolicyById(policyItem.Key ?? 0);
                if (policyFind != null && policyFind.ExpirationRenewals != 0)
                {
                    continue;
                }
                now = now.AddDays(+policyFind.ExpirationRenewals);
                foreach (var item in policyItem)
                {
                    if (item.DueDate == now)
                    {
                        await _investSendEmailServices.SendEmailNoticePaymentDueDate(item.OrderId, item.DueDate);
                    }
                }
            }
        }

        /// <summary>
        /// Saler thỏa mãn thông tin trước khi đặt lệnh thì sẽ trả ra thông tin
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public ViewCheckSaleBeforeAddOrderDto CheckSaleBeforeAddOrder(string referralCode, int distributionId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var saleInfo = _orderRepository.AppCheckSaleBeforeAddOrder(referralCode, distributionId, investorId);

            return saleInfo;
        }

        //Hàm lấy ngày chi trả của kì gần nhất
        public ThoiGianChiTraThucDto LayNgayChiTra(int? orderId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var listOrder = _orderRepository.GetAllListOrder(tradingProviderId, InvestOrderStatus.DANG_DAU_TU, null, null, null, null, null, null);
            Dictionary<long, ThoiGianChiTraThucDto> dictThoiGianChiTra = new();

            foreach (var orderItem in listOrder)
            {
                //Task task = Task.Run (() =>
                //{
                //Lấy kỳ hạn
                var policyDetailFind = _policyRepository.FindPolicyDetailById(orderItem.PolicyDetailId, orderItem.TradingProviderId, false);
                var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyDetailFind.DistributionId && r.Deleted == YesNo.NO);
                List<DateTime> thoiGianThuc = new();

                //Lấy ngày bắt đầu tính lãi
                var ngayDauKy = orderItem.InvestDate.Value.Date;

                //Tính ngày đáo hạn
                var ngayDaoHan = _orderRepository.CalculateDueDate(policyDetailFind, ngayDauKy.Date, distribution?.CloseCellDate, false);

                if (policyDetailFind.InterestType == InterestTypes.DINH_KY) //trả định kỳ
                {
                    //Tính thời gian thực của các kỳ trả
                    while (ngayDauKy <= ngayDaoHan)
                    {
                        DateTime ngayCuoiKy = ngayDauKy.AddDays(policyDetailFind.InterestPeriodQuantity ?? 0);

                        if (policyDetailFind.InterestPeriodType == PeriodUnit.MONTH)
                        {
                            ngayCuoiKy = ngayDauKy.AddMonths(policyDetailFind.InterestPeriodQuantity ?? 0);
                        }
                        else if (policyDetailFind.InterestPeriodType == PeriodUnit.YEAR)
                        {
                            ngayCuoiKy = ngayDauKy.AddYears(policyDetailFind.InterestPeriodQuantity ?? 0);
                        }

                        //Chuyển đến kỳ tiếp theo
                        ngayDauKy = ngayCuoiKy;

                        var ngayLamViec = _orderRepository.NextWorkDay(ngayCuoiKy.Date, policyDetailFind.TradingProviderId, false);
                        //nếu ngày đáo hạn vượt quá ngày đáo hạn
                        if (ngayLamViec > ngayDaoHan)
                        {
                            ngayLamViec = ngayDaoHan;
                        }

                        if (thoiGianThuc.Count > 1 && ngayLamViec == thoiGianThuc[^1]) //trường hợp cộng thừa 1 kỳ
                        {
                            break;
                        }

                        thoiGianThuc.Add(ngayLamViec);
                    };

                    if (thoiGianThuc.Count > 0 && thoiGianThuc[^1] < ngayDaoHan)
                    {
                        thoiGianThuc[^1] = ngayDaoHan;
                    }

                    //Thời gian thực tính tiền...
                    for (int j = 0; j < thoiGianThuc.Count; j++)
                    {
                        int soNgay;

                        soNgay = (thoiGianThuc[j] - orderItem.InvestDate.Value.Date).Days;

                        if (dictThoiGianChiTra.ContainsKey(orderItem.Id))
                        {
                            dictThoiGianChiTra[orderItem.Id] = new ThoiGianChiTraThucDto
                            {
                                PayDate = ngayDaoHan,
                                PolicyDetailId = policyDetailFind.Id,
                                OrderId = orderItem.Id,
                                PeriodIndex = j + 1,
                                SoNgay = soNgay
                            };
                        }
                        else
                        {
                            dictThoiGianChiTra.Add(orderItem.Id, new ThoiGianChiTraThucDto
                            {
                                PayDate = ngayDaoHan,
                                PolicyDetailId = policyDetailFind.Id,
                                OrderId = orderItem.Id,
                                PeriodIndex = j + 1,
                                SoNgay = soNgay
                            });
                        }
                    }
                }
                else if (policyDetailFind.InterestType == InterestTypes.CUOI_KY) //Trả cuối kỳ
                {
                    int soNgay = ngayDaoHan.Subtract(ngayDauKy.Date).Days;
                    if (dictThoiGianChiTra.ContainsKey(orderItem.Id))
                    {
                        dictThoiGianChiTra[orderItem.Id] = new ThoiGianChiTraThucDto
                        {
                            PayDate = ngayDaoHan,
                            PolicyDetailId = policyDetailFind.Id,
                            OrderId = orderItem.Id,
                            PeriodIndex = 1,
                            SoNgay = soNgay
                        };
                    }
                    else
                    {
                        dictThoiGianChiTra.Add(orderItem.Id, new ThoiGianChiTraThucDto
                        {
                            PayDate = ngayDaoHan,
                            PolicyDetailId = policyDetailFind.Id,
                            OrderId = orderItem.Id,
                            PeriodIndex = 1,
                            SoNgay = soNgay
                        });
                    }
                }
            }
            _orderRepository.CloseConnection();

            ThoiGianChiTraThucDto result = dictThoiGianChiTra.Values.FirstOrDefault(r => r.OrderId == orderId);
            return result;
        }

        public PagingResult<HistoryUpdateDto> GetOrderHistoryUpdate(int pageNumber, int? pageSize, string keyword, int orderId)
        {
            var result = _investOrderRepository.GetAllOrderHistoryUpdate(pageNumber, pageSize, keyword, orderId);
            return result;
        }

        /// <summary>
        /// Hủy ký file hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int UpdateIsSignByOrderId(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _orderContractFileRepository.UpdateIsSignByOrderId(orderId);

            // Cập nhật lịch sử hủy ký file hợp đồng
            _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(orderId, null, null, null
                , InvestHistoryUpdateTables.INV_ORDER, ActionTypes.CAP_NHAT, "Cập nhật hủy ký file hợp đồng", DateTime.Now), username);
            _dbContext.SaveChanges();
            return -1;
        }

        public int ProcessContract(int orderId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderRepository.ProcessContract(new InvOrder
            {
                Id = orderId,
                TradingProviderId = tradingProviderId,
                PendingDateModifiedBy = username,
            });
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
        public PagingResult<OrderRenewalsRequestDto> GetAllRenewalsRequest(FilterRenewalsRequestDto input)
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
            PagingResult<OrderRenewalsRequestDto> result = new();
            //var renewalsRequests = _invRenewalsRequestRepository.GetAll(tradingProviderId, input);
            //var renewalsRequests = _invRenewalsRequestEFRepository.GetAll(tradingProviderId, input).AsEnumerable();

            var renewalsRequests = from req in _dbContext.InvestRenewalsRequests
                                   join order in _dbContext.InvOrders.Include(o => o.Project).Include(o => o.Policy).Include(o => o.PolicyDetail).Include(o => o.CifCodes) on req.OrderId equals order.Id
                                   join approve in _dbContext.InvestApproves.Where(approve => approve.DataType == InvestApproveDataTypes.EP_INV_RENEWALS_REQUEST) on req.Id equals approve.ReferId

                                   let orderOriginal = _dbContext.InvOrders.FirstOrDefault(o => o.Id == order.RenewalsReferIdOriginal && o.Deleted == YesNo.NO)
                                   let orderOriginalContractFile = orderOriginal == null ? null : _dbContext.InvestOrderContractFile.Where(e => e.OrderId == orderOriginal.Id).FirstOrDefault()

                                   let orderRenewal = _dbContext.InvOrders.FirstOrDefault(o => o.RenewalsReferId == order.Id && o.Deleted == YesNo.NO)
                                   let orderRenewalContractFile = orderRenewal == null ? null : _dbContext.InvestOrderContractFile.Where(o => o.OrderId == orderRenewal.Id && o.Deleted == YesNo.NO).Select(contract => contract.ContractCodeGen).FirstOrDefault()
                                   let orderRenewalContractCode = orderRenewalContractFile ?? order.ContractCode

                                   let orderContractCodes = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).FirstOrDefault()
                                   let orderContractCode = orderContractCodes ?? order.ContractCode

                                   let interestPayment = _dbContext.InvestInterestPayments.FirstOrDefault(p => p.OrderId == order.Id && (p.Status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN
                                                                       || p.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN) && p.Deleted == YesNo.NO && p.IsLastPeriod == YesNo.YES)
                                   let investor = req.Order.CifCodes == null ? null : req.Order.CifCodes.Investor
                                   let businessCustomer = req.Order.CifCodes == null ? null : req.Order.CifCodes.BusinessCustomer
                                   let identitfication = _dbContext.InvestorIdentifications.Where(iden => iden.InvestorId == investor.InvestorId && iden.IsDefault == YesNo.YES && iden.Deleted == YesNo.NO).FirstOrDefault() ?? null
                                   let policy = req.Order.Policy == null ? null : req.Order.Policy
                                   let project = req.Order.Project == null ? null : req.Order.Project
                                   let policyDetail = req.Order.PolicyDetail == null ? null : req.Order.PolicyDetail

                                   where (tradingProviderId != null && order.TradingProviderId == tradingProviderId) || (input.TradingProviderIds != null && input.TradingProviderIds.Contains(order.TradingProviderId))
                                         && (input.SettlementMethod == null || input.SettlementMethod == req.SettlementMethod)

                                   select new OrderRenewalsRequestDto()
                                   {
                                       Id = req.Id,
                                       OrderId = order.Id,
                                       SettlementMethod = req.SettlementMethod,
                                       RenewalsPolicyDetailId = req.RenewalsPolicyDetailId,
                                       RequestDate = approve.RequestDate,
                                       OrderNewId = orderRenewal == null ? 0 : orderRenewal.Id,
                                       ContractCode = orderRenewal == null ? null : orderRenewal.ContractCode,
                                       ApproveRenewalBy = interestPayment == null ? null : interestPayment.ApproveBy,
                                       ApproveRenewalDate = interestPayment == null ? null : interestPayment.ApproveDate,

                                       RenewalMoney = orderRenewal == null ? null : orderRenewal.InitTotalValue,
                                       GenContractCode = (req.Status == InvestRenewalsRequestStatus.KHOI_TAO || req.Status == InvestRenewalsRequestStatus.DA_HUY) ? null : orderRenewalContractCode,
                                       ContractCodeOriginal = orderOriginal == null
                                                               ? orderContractCode
                                                               : (orderOriginalContractFile != null ? orderOriginalContractFile.ContractCodeGen : orderOriginal.ContractCode),
                                       RenewalsType = order.Policy == null ? 0 : order.Policy.RenewalsType,
                                       Investor = investor == null ? null : new EPIC.Entities.Dto.Investor.InvestorDto()
                                       {
                                           InvestorId = investor.InvestorId,
                                           Name = investor.Name,
                                           Address = investor.Address,
                                           ContactAddress = investor.ContactAddress,
                                           AvatarImageUrl = investor.AvatarImageUrl,
                                           Email = investor.Email,
                                           InvestorIdentification = identitfication == null ? null : new Entities.Dto.Investor.InvestorIdentificationDto()
                                           {
                                               CreatedBy = identitfication.CreatedBy,
                                               CreatedDate = identitfication.CreatedDate,
                                               Fullname = identitfication.Fullname,
                                           }
                                       },

                                       BusinessCustomer = businessCustomer == null ? null : new BusinessCustomerDto()
                                       {
                                           BusinessCustomerId = businessCustomer.BusinessCustomerId,
                                           Code = businessCustomer.Code,
                                           Name = businessCustomer.Name,
                                           ShortName = businessCustomer.ShortName,
                                           Address = businessCustomer.Address,
                                           Phone = businessCustomer.Phone,
                                           Email = businessCustomer.Email,
                                           TaxCode = businessCustomer.TaxCode,
                                       },

                                       Project = project == null ? null : new ProjectDto()
                                       {
                                           Id = project.Id,
                                           InvCode = project.InvCode,
                                           InvName = project.InvName,

                                       },

                                       Policy = policy == null ? null : new PolicyDto()
                                       {
                                           Id = policy.Id,
                                           Code = policy.Code,
                                           Name = policy.Name,
                                           StartDate = policy.StartDate,
                                           EndDate = policy.EndDate,
                                       },

                                       PolicyDetail = policyDetail == null ? null : new PolicyDetailDto()
                                       {
                                           Id = policyDetail.Id,
                                           DistributionId = policyDetail.DistributionId,
                                           Name = policyDetail.Name,
                                           PeriodQuantity = policyDetail.PeriodQuantity,
                                           PeriodType = policyDetail.PeriodType,
                                           PolicyId = policyDetail.PolicyId,
                                           Profit = policyDetail.Profit,
                                           ShortName = policyDetail.ShortName,
                                           STT = policyDetail.STT,
                                           InterestDays = policyDetail.InterestDays,
                                           InterestType = policyDetail.InterestType,
                                       },
                                       OrderSource = order.Source == SourceOrder.ONLINE ? SourceOrderFE.KHACH_HANG : ((order.Source == SourceOrder.OFFLINE && order.SaleOrderId != null) ? SourceOrderFE.SALE : (order.Source == SourceOrder.OFFLINE && req.Order.SaleOrderId == null) ? SourceOrderFE.QUAN_TRI_VIEN : null),
                                       Source = order.Source,
                                       PolicyId = order.Policy.Id,
                                       CustomerName = investor == null ? (identitfication == null ? (businessCustomer == null ? null : businessCustomer.Name) : identitfication.Fullname) : null,
                                       Status = req.Status
                                   };

            // Lọc theo mã hợp đồng
            if (input.ContractCode != null)
            {
                renewalsRequests = renewalsRequests.Where(o => (o.ContractCode != null && o.ContractCode.Contains(input.ContractCode))
                                                    || (o.ContractCodeOriginal != null && o.ContractCodeOriginal.Contains(input.ContractCode)
                                                    || (o.GenContractCode != null && o.GenContractCode.Contains(input.ContractCode)))
                                                    );
            }

            // Lọc theo cifCode
            if (input.CifCode != null)
            {
                renewalsRequests = from item in renewalsRequests
                                   join order in _dbContext.InvOrders on item.OrderId equals order.Id
                                   join cifCode in _dbContext.CifCodes on order.CifCode equals cifCode.CifCode
                                   where order.CifCode == input.CifCode && cifCode.Deleted == YesNo.NO && order.Deleted == YesNo.NO
                                   select item;
            }
            // Lọc theo số điện thoại
            if (input.Phone != null)
            {
                renewalsRequests = renewalsRequests.Where(o => (o.Investor != null && o.Investor.Phone != null && o.Investor.Phone.Contains(input.Phone)) || (o.BusinessCustomer != null && o.BusinessCustomer.Phone.Contains(input.Phone)));
            }
            // Lọc theo status
            if (input.Status != null)
            {
                renewalsRequests = renewalsRequests.Where(o => o.Status == input.Status);
            }
            //Lọc theo nguồn đặt lệnh
            if (input.OrderSource != null)
            {
                renewalsRequests = renewalsRequests.Where(o => o.OrderSource == input.OrderSource);
            }

            // Lọc theo loại hình đặt
            if (input.Source != null)
            {
                renewalsRequests = renewalsRequests.Where(o => o.Source == input.Source);
            }
            // Lọc theo phân phối sản phẩm
            if (input.DistributionId != null)
            {
                renewalsRequests = renewalsRequests.Where(o => (o.PolicyDetail != null && o.PolicyDetail.DistributionId == input.DistributionId));
            }

            // Lọc theo kỳ hạn
            if (input.PolicyDetailId != null)
            {
                renewalsRequests = renewalsRequests.Where(o => (o.PolicyDetail != null && o.PolicyDetail.Id == input.PolicyDetailId));
            }
            // Lọc theo chính sách
            if (input.PolicyId != null)
            {
                renewalsRequests = renewalsRequests.Where(o => o.PolicyId == input.PolicyId);
            }
            // Lọc theo nhiều chính sách
            if (input.Policy != null)
            {
                List<int> policyIds = input.Policy.Split(',')?.Select(Int32.Parse)?.ToList();
                renewalsRequests = renewalsRequests.Where(o => (o.Policy != null && policyIds.Contains(o.Policy.Id)));
            }

            if (input.Orderer != null)
            {
                renewalsRequests = from item in renewalsRequests
                                   join order in _dbContext.InvOrders on item.OrderId equals order.Id
                                   join cifCode in _dbContext.CifCodes on order.CifCode equals cifCode.CifCode
                                   where cifCode.Deleted == YesNo.NO && order.Deleted == YesNo.NO
                                   && ((input.Orderer == SourceOrderFE.KHACH_HANG && order.Source == SourceOrder.ONLINE)
                                   || (input.Orderer == SourceOrderFE.QUAN_TRI_VIEN && order.Source == SourceOrder.OFFLINE)
                                   || (input.Orderer == SourceOrderFE.SALE && order.SaleOrderId != null && order.Source == SourceOrder.ONLINE))
                                   select item;
            }

            result.TotalItems = renewalsRequests.Count();

            renewalsRequests = renewalsRequests.OrderByDescending(w => w.RequestDate);
            renewalsRequests = renewalsRequests.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                result.Items = renewalsRequests.Skip(input.Skip).Take(input.PageSize);
            }
            return result;
        }

        /// <summary>
        /// Hủy tái tục mới nhất theo orderId
        /// </summary>
        /// <param name="orderId"></param>
        public void CancelRenewalRequestByOrderId(long orderId)
        {
            // tìm investRenewalsRequest theo orderId
            var investRenewalsRequest = _renewalsRequestRepository.GetListByOrderId(orderId).ToList().OrderByDescending(o => o.Id);
            if (investRenewalsRequest != null)
            {
                var investRenewalsRequestNew = investRenewalsRequest.FirstOrDefault();
                _renewalsRequestRepository.CancelRequest(investRenewalsRequestNew.Id, null);
            }
        }

        /// <summary>
        ///  Cập nhật lại dòng tiền
        /// </summary>
        public void UpdateOrderCashFlow(int orderId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateOrderCashFlow)}: orderId = {orderId}; tradingProviderId = {tradingProviderId}");
            var orderFind = _dbContext.InvOrders.FirstOrDefault(o => o.Id == orderId && tradingProviderId == o.TradingProviderId && o.Status == OrderStatus.DANG_DAU_TU && o.InvestDate != null && o.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestOrderNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            if (_dbContext.InvestInterestPayments.Any(o => o.OrderId == orderId && o.Deleted == YesNo.NO && o.Status != InterestPaymentStatus.HUY_DUYET))
            {
                // Đã có yêu cầu chi trả không được thay đổi
                _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderInterstPaymentRequestExist);
            }
            if (_dbContext.InvestWithdrawals.Any(o => o.OrderId == orderId && o.Deleted == YesNo.NO && o.Status != WithdrawalStatus.HUY_YEU_CAU))
            {
                // Đã có yêu cầu rút vốn không được thay đổi
                _investOrderEFRepository.ThrowException(ErrorCode.InvestOrderWithdrawalRequestExist);
            }
            //Lấy kỳ hạn
            var policyDetailFind = _dbContext.InvestPolicyDetails.FirstOrDefault(r => r.Id == orderFind.PolicyDetailId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyDetailNotFound);

            var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == orderFind.DistributionId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
            //Tính ngày đáo hạn
            orderFind.DueDate = _investOrderRepository.CalculateDueDate(policyDetailFind, orderFind.InvestDate.Value, distribution.CloseCellDate, false);

            _dbContext.SaveChanges();
            //Xóa đi thời gian cũ
            var interestPaymentDate = _dbContext.InvestInterestPaymentDates.Where(o => o.OrderId == orderId);
            _dbContext.InvestInterestPaymentDates.RemoveRange(interestPaymentDate);

            // Lưu lại thời gian mới
            var profitInfo = ExpectedCashFlow(orderId);
            foreach (var profitItem in profitInfo.ProfitInfo)
            {
                _interestPaymentEFRepository.AddInvestInterestPaymentDate(new InvestInterestPaymentDate
                {
                    OrderId = orderId,
                    PayDate = profitItem.PayDate ?? DateTime.MinValue,
                    PeriodIndex = profitInfo.ProfitInfo.IndexOf(profitItem)
                });
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }
    }
}