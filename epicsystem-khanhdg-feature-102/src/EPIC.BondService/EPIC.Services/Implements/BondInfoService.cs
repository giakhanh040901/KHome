using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.DepositProvider;
using EPIC.Entities.Dto.Issuer;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Transactions;

namespace EPIC.BondDomain.Implements
{
    public class BondInfoService : IBondProductBondInfoService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BondDepositProviderRepository _depositProviderRepository;
        private readonly BondInfoRepository _bondInfoRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly BondCalendarRepository _calendarRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly BondOrderRepository _bondOrderRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly IMapper _mapper;
        private readonly IBondSharedService _bondSharedService;

        public BondInfoService(
            ILogger<BondInfoService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IBondSharedService bondSharedService)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _depositProviderRepository = new BondDepositProviderRepository(_connectionString, _logger);
            _bondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _calendarRepository = new BondCalendarRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _distributionContractRepository = new BondDistributionContractRepository(_connectionString, _logger);
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _mapper = mapper;
            _bondSharedService = bondSharedService;
        }

        public BondInfo Add(CreateProductBondInfoDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var productBondInfo = new BondInfo
            {
                IssuerId = input.IssuerId,
                DepositProviderId = input.DepositProviderId,
                BondTypeId = input.BondTypeId,
                BondCode = input.BondCode,
                BondName = input.BondName,
                Description = input.Description,
                Content = input.Content,
                IssueDate = input.IssueDate,
                DueDate = input.DueDate,
                ParValue = input.ParValue,
                BondPeriod = input.BondPeriod,
                BondPeriodUnit = input.BondPeriodUnit,
                InterestRate = input.InterestRate,
                InterestPeriod = input.InterestPeriod,
                InterestPeriodUnit = input.InterestPeriodUnit,
                InterestRateType = input.InterestRateType,
                IsPaymentGuarantee = input.IsPaymentGuarantee,
                AllowSbd = input.AllowSbd,
                AllowSbdMonth = input.AllowSbdMonth,
                IsCollateral = input.IsCollateral,
                MaxInvestor = input.MaxInvestor,
                NumberClosePer = input.NumberClosePer,
                CountType = input.CountType,
                NiemYet = input.NiemYet,
                Quantity = input.Quantity,
                IsCheck = input.IsCheck,
                PolicyPaymentContent = input.PolicyPaymentContent,
                Icon = input.Icon,
                PartnerId = partnerId,
                CreatedBy = username
            };
            return _productBondInfoRepository.Add(productBondInfo);
        }

        public int Delete(int id)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _productBondInfoRepository.Delete(id, partnerId);
        }

        public PagingResult<ProductBondInfoDto> FindAll(int pageSize, int pageNumber, string keyword, string status, string isCheck, DateTime? issueDate, DateTime? dueDate)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var bondInfoList = _productBondInfoRepository.FindAllProductBondInfo(pageSize, pageNumber, keyword, status, partnerId, isCheck, issueDate, dueDate);
            var result = new PagingResult<ProductBondInfoDto>();
            var items = new List<ProductBondInfoDto>();
            result.TotalItems = bondInfoList.TotalItems;
            foreach (var bond in bondInfoList.Items)
            {
                var bondInfoById = _bondInfoRepository.FindById(bond.ProductBondId);
                var bondInfo = _mapper.Map<ProductBondInfoDto>(bond);

                var bondPrimaryFind = _productBondPrimaryRepository.GetAllByInfo(bondInfo.ProductBondId);
                long soLuongDaBan = 0;
                long sumQuantityOrder = 0;
                foreach (var bondPrimary in bondPrimaryFind)
                {
                    var bondSecondary = _productBondSecondaryRepository.FindSecondaryByPrimaryId(bondPrimary.Id);
                    if (bondSecondary != null)
                    {
                        sumQuantityOrder = _bondOrderRepository.SumQuantity(bondSecondary.TradingProviderId, bondSecondary.Id);
                    }
                    soLuongDaBan += sumQuantityOrder;
                }

                bondInfo.SoLuongConLai = bondInfo.Quantity - soLuongDaBan;

                var issuer = _issuerRepository.FindById(bondInfoById.IssuerId);
                if (issuer != null)
                {
                    bondInfo.Issuer = new ViewIssuerDto()
                    {
                        IssuerId = issuer.Id,
                        BusinessCustomerId = issuer.BusinessCustomerId
                    };

                    var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                    if (businessCustomerIssuer != null)
                    {
                        bondInfo.Issuer.BusinessCustomer = new BusinessCustomerDto()
                        {
                            BusinessCustomerId = businessCustomerIssuer.BusinessCustomerId,
                            TaxCode = businessCustomerIssuer.TaxCode,
                            ShortName = businessCustomerIssuer.ShortName,
                            Code = businessCustomerIssuer.Code,
                            Name = businessCustomerIssuer.Name,
                        };
                        var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerIssuer.BusinessCustomerId ?? 0, -1, 0, null);
                        bondInfo.Issuer.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                    };
                };

                var depositProvider = _depositProviderRepository.FindById(bondInfoById.DepositProviderId);
                if (depositProvider != null)
                {
                    bondInfo.DepositProvider = new DepositProviderDto()
                    {
                        DepositProviderId = depositProvider.Id,
                        BusinessCustomerId = depositProvider.BusinessCustomerId,
                    };

                    var businessCustomerDeposit = _businessCustomerRepository.FindBusinessCustomerById(depositProvider.BusinessCustomerId);
                    if (businessCustomerDeposit != null)
                    {
                        bondInfo.DepositProvider.BusinessCustomer = new BusinessCustomerDto()
                        {
                            BusinessCustomerId = businessCustomerDeposit.BusinessCustomerId,
                            TaxCode = businessCustomerDeposit.TaxCode,
                            ShortName = businessCustomerDeposit.ShortName,
                            Code = businessCustomerDeposit.Code,
                            Name = businessCustomerDeposit.Name,
                        };
                    };
                }
                items.Add(bondInfo);
            }
            result.Items = items;
            return result;
        }

        public ProductBondInfoDto FindById(int id)
        {
            var bondInfo = _bondInfoRepository.FindBondInfoById(id);
            var issuer = _issuerRepository.FindById(bondInfo.IssuerId);
            var depositProvider = _depositProviderRepository.FindById(bondInfo.DepositProviderId);

            var result = _mapper.Map<ProductBondInfoDto>(bondInfo);

            var bondPrimaryFind = _productBondPrimaryRepository.GetAllByInfo(bondInfo.Id);
            long soLuongDaBan = 0;
            long sumQuantityOrder = 0;
            foreach (var bondPrimary in bondPrimaryFind)
            {
                var bondSecondary = _productBondSecondaryRepository.FindSecondaryByPrimaryId(bondPrimary.Id);
                if (bondSecondary != null)
                {
                    sumQuantityOrder = _bondOrderRepository.SumQuantity(bondSecondary.TradingProviderId, bondSecondary.Id);
                }
                soLuongDaBan += sumQuantityOrder;
            }

            result.SoLuongConLai = bondInfo.Quantity - soLuongDaBan;

            if (issuer != null)
            {
                result.Issuer = new ViewIssuerDto()
                {
                    IssuerId = issuer.Id,
                    BusinessCustomerId = issuer.BusinessCustomerId
                };

                var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                if (businessCustomerIssuer != null)
                {
                    result.Issuer.BusinessCustomer = new BusinessCustomerDto()
                    {
                        BusinessCustomerId = businessCustomerIssuer.BusinessCustomerId,
                        TaxCode = businessCustomerIssuer.TaxCode,
                        ShortName = businessCustomerIssuer.ShortName,
                        Code = businessCustomerIssuer.Code,
                        Name = businessCustomerIssuer.Name,
                    };
                };
            };

            if (depositProvider != null)
            {
                result.DepositProvider = new DepositProviderDto()
                {
                    DepositProviderId = depositProvider.Id,
                    BusinessCustomerId = depositProvider.BusinessCustomerId,
                };

                var businessCustomerDeposit = _businessCustomerRepository.FindBusinessCustomerById(depositProvider.BusinessCustomerId);
                if (businessCustomerDeposit != null)
                {
                    result.DepositProvider.BusinessCustomer = new BusinessCustomerDto()
                    {
                        BusinessCustomerId = businessCustomerDeposit.BusinessCustomerId,
                        TaxCode = businessCustomerDeposit.TaxCode,
                        ShortName = businessCustomerDeposit.ShortName,
                        Code = businessCustomerDeposit.Code,
                        Name = businessCustomerDeposit.Name,
                    };
                };
            }
            return result;
        }

        public CouponDto FindCouponById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var bondInfo = _bondInfoRepository.FindBondInfoById(id);
            var result = _bondSharedService.CalculateCouponByQuantity(bondInfo, bondInfo.Quantity, partnerId);
            return result;
        }

        public int Update(int id, UpdateProductBondInfoDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _bondInfoRepository.Update(new BondInfo
            {
                Id = id,
                IssuerId = input.IssuerId,
                DepositProviderId = input.DepositProviderId,
                BondTypeId = input.BondTypeId,
                BondCode = input.BondCode,
                BondName = input.BondName,
                Description = input.Description,
                Content = input.Content,
                IssueDate = input.IssueDate,
                DueDate = input.DueDate,
                ParValue = input.ParValue,
                BondPeriod = input.BondPeriod,
                BondPeriodUnit = input.BondPeriodUnit,
                InterestRate = input.InterestRate,
                InterestPeriod = input.InterestPeriod,
                InterestPeriodUnit = input.InterestPeriodUnit,
                InterestRateType = input.InterestRateType,
                IsPaymentGuarantee = input.IsPaymentGuarantee,
                AllowSbd = input.AllowSbd,
                AllowSbdMonth = input.AllowSbdMonth,
                IsCollateral = input.IsCollateral,
                MaxInvestor = input.MaxInvestor,
                NumberClosePer = input.NumberClosePer,
                CountType = input.CountType,
                NiemYet = input.NiemYet,
                Quantity = input.Quantity,
                IsCheck = input.IsCheck,
                PolicyPaymentContent = input.PolicyPaymentContent,
                Icon = input.Icon,
                ModifiedBy = username,
                PartnerId = partnerId
            });
        }

        public void Request(RequestStatusDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var actionType = ActionTypes.THEM_MOI;
                var checkIsUpdate = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_INFO);
                if (checkIsUpdate != null)
                {
                    actionType = ActionTypes.CAP_NHAT;
                }
                _approveRepository.CreateApproveRequest(new CreateApproveRequestDto
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = actionType,
                    DataType = CoreApproveDataType.PRODUCT_BOND_INFO,
                    ReferId = input.Id,
                    Summary = input.Summary
                }, null, partnerId);
                _productBondInfoRepository.BondInfoRequest(input.Id);
                transaction.Complete();
            }
            _productBondInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Duyet
        /// </summary>
        /// <param name="input"></param>
        public void Approve(ApproveStatusDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_INFO);

                if (approve != null)
                {
                    _approveRepository.ApproveRequestStatus(new ApproveRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId
                    });
                }
                _productBondInfoRepository.BondInfoApprove(input.Id);
                transaction.Complete();
            }
            _productBondInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Xac minh
        /// </summary>
        /// <param name="input"></param>
        public void Check(CheckStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_INFO);
                if (approve != null)
                {
                    _approveRepository.CheckRequest(new CheckRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        UserCheckId = userid,
                    });
                }
                _productBondInfoRepository.BondInfoCheck(input.Id);
                transaction.Complete();
            }
            _productBondInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Huy
        /// </summary>
        public void Cancel(CancelStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_INFO);
                if (approve != null)
                {
                    _approveRepository.CancelRequest(new CancelRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        CancelNote = input.CancelNote,
                    });
                }
                _productBondInfoRepository.BondInfoCancel(input.Id);
                transaction.Complete();
            }
            _productBondInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Đóng
        /// </summary>
        public void CloseOpen(int id)
        {
            _productBondInfoRepository.BondInfoClose(id);
        }
    }
}