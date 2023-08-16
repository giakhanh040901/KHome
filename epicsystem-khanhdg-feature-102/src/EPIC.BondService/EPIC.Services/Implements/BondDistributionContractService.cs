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
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.DistributionContractPayment;
using EPIC.Entities.Dto.Issuer;
using EPIC.Entities.Dto.ProductBondPrimary;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ServiceModel;
using System.Transactions;

namespace EPIC.BondDomain.Implements
{
    public class BondDistributionContractService : IBondDistributionContractService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly BondInfoRepository _bondInfoRepository;
        private readonly BondPrimaryRepository _bondPrimaryRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BondDepositProviderRepository _depositProviderRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly ApproveRepository _approveRepository;
        private readonly IBondSharedService _bondSharedService;

        public BondDistributionContractService(
            ILogger<BondDistributionContractService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IBondSharedService bondSharedService)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _distributionContractRepository = new BondDistributionContractRepository(_connectionString, _logger);
            _bondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _bondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _depositProviderRepository = new BondDepositProviderRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _bondSharedService = bondSharedService;
        }

        public BondDistributionContract Add(CreateDistributionContractDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contract = new BondDistributionContract()
            {
                PartnerId = partnerId,
                TradingProviderId = input.TradingProviderId,
                BondPrimaryId = input.PrimaryId,
                Quantity = input.Quantity,
                TotalValue = input.TotalValue,
                DateBuy = input.DateBuy,
                CreatedBy = username
            };
            return _distributionContractRepository.Add(contract);
        }

        public BondDistributionContractPayment Add(CreateDistributionContractPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contract = new BondDistributionContractPayment()
            {
                DistributionContractId = input.DistributionContractId,
                TransactionType = input.TransactionType,
                PaymentType = input.PaymentType,
                TotalValue = input.TotalValue,
                Description = input.Description,
                TradingDate = input.TradingDate,
                CreatedBy = username
            };
            return _distributionContractRepository.Add(contract);
        }

        public DistributionContractFile Add(CreateDistributionContractFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contract = new DistributionContractFile()
            {
                DistributionContractId = input.DistributionContractId,
                Title = input.Title,
                FileUrl = input.FileUrl,
                CreatedBy = username
            };
            return _distributionContractRepository.Add(contract);
        }

        public int ContractFileApprove(int id)
        {
            return _distributionContractRepository.ContractFileApprove(id, CommonUtils.GetCurrentUsername(_httpContext)); throw new System.NotImplementedException();
        }


        public void ContractFileCancel(CancelStatusDto input)
        {
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.DISTRIBUTION_CONTRACT_FILE);
                if (approve != null)
                {
                    _approveRepository.CancelRequest(new CancelRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        CancelNote = input.CancelNote,
                    });
                }
                _distributionContractRepository.ContractFileCancel(input.Id, CommonUtils.GetCurrentUsername(_httpContext));
                transaction.Complete();
            }
            _distributionContractRepository.CloseConnection();
        }

        public int ContractPaymentApprove(int id)
        {
            return _distributionContractRepository.ContractPaymentApprove(id, CommonUtils.GetCurrentUsername(_httpContext));
        }

        public int ContractPaymentCancel(int id)
        {
            return _distributionContractRepository.ContractPaymentCancel(id, CommonUtils.GetCurrentUsername(_httpContext));
        }

        public int Delete(int id)
        {
            return _distributionContractRepository.Delete(id, CommonUtils.GetCurrentPartnerId(_httpContext));
        }

        public int DeleteContractFile(int id)
        {
            return _distributionContractRepository.DeleteContractFile(id);
        }

        public int DeleteContractPayment(int id)
        {
            return _distributionContractRepository.DeleteContractPayment(id);
        }

        public int Duyet(int id)
        {
            return _distributionContractRepository.Duyet(id);
        }

        public PagingResult<DistributionContractFile> FindAllContractFile(int contractId, int pageSize, int pageNumber, string keyword)
        {
            return _distributionContractRepository.FindAllContractFile(contractId, pageSize, pageNumber, keyword);
        }

        public PagingResult<DistributionContractPaymentDto> FindAllContractPayment(int contractId, int pageSize, int pageNumber, string keyword)
        {
            return _distributionContractRepository.FindAllContractPayment(contractId, pageSize, pageNumber, keyword);
        }

        public PagingResult<DistributionContractDto> FindAll(int pageSize, int pageNumber, string keyword, int? status)
        { 
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var contractList = _distributionContractRepository.FindAllContract(partnerId, pageSize, pageNumber, keyword, status);
            var result = new PagingResult<DistributionContractDto>();
            var items = new List<DistributionContractDto>();
            result.TotalItems = contractList.TotalItems;
            foreach (var bond in contractList.Items)
            {
                var contract = _distributionContractRepository.FindContractById(bond.DistributionContractId);
                var bondContract = _mapper.Map<DistributionContractDto>(contract);
                    bondContract.BondName = bond.BondName;
                    bondContract.TradingProviderName = bond.TradingProviderName;

                var bondPrimary = _bondPrimaryRepository.FindById(contract.PrimaryId, null);
                if (bondPrimary != null)
                {
                    bondContract.BondPrimary = _mapper.Map<ProductBondPrimaryDto>(bondPrimary);
                };

                var bondInfo = _bondInfoRepository.FindById(bondPrimary.BondId);
                if (bondInfo != null)
                {
                    bondContract.ProductBondInfo = _mapper.Map<ProductBondInfoDto>(bondInfo);
                };

                var tradingProvider = _tradingProviderRepository.FindById(contract.TradingProviderId);
                if (tradingProvider != null)
                {
                    bondContract.TradingProvider = new TradingProviderDto()
                    {
                        TradingProviderId = tradingProvider.TradingProviderId,
                        BusinessCustomerId = tradingProvider.BusinessCustomerId
                    };

                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                    if (businessCustomer != null)
                    {
                        bondContract.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    };
                };
                items.Add(bondContract);
            }
            result.Items = items;
            return result;
        }
        public DistributionContractDto FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var contract = _distributionContractRepository.FindById(id, partnerId);

            var result = new DistributionContractDto
            {
                DistributionContractId = id,
                TradingProviderId = contract.TradingProviderId,
                PrimaryId = contract.BondPrimaryId,
                ContractCode = contract.ContractCode,
                Status = contract.Status,
                Quantity = contract.Quantity,
                TotalValue = contract.TotalValue,
                DateBuy = contract.DateBuy,
                CreatedDate = contract.CreatedDate
            };

            var tradingProvider = _tradingProviderRepository.FindById(contract.TradingProviderId);
            if (tradingProvider != null)
            {
                result.TradingProvider = new TradingProviderDto()
                {
                    TradingProviderId = tradingProvider.TradingProviderId,
                    BusinessCustomerId = tradingProvider.BusinessCustomerId
                };
                var businessCustomerTradingProvider = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                if (businessCustomerTradingProvider != null)
                {
                    result.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerTradingProvider);

                    var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(tradingProvider.BusinessCustomerId);
                    if (businessCustomerBank != null)
                    {
                        result.TradingProvider.BusinessCustomer.BankAccNo = businessCustomerBank.BankAccNo;
                        result.TradingProvider.BusinessCustomer.BankAccName = businessCustomerBank.BankAccName;
                        result.TradingProvider.BusinessCustomer.BankName = businessCustomerBank.BankName;
                    }
                };
            };

            var bondPrimary = _bondPrimaryRepository.FindById(contract.BondPrimaryId, null);
            if (bondPrimary != null)
            {
                result.BondPrimary = _mapper.Map<ProductBondPrimaryDto>(bondPrimary);
                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(bondPrimary.BusinessCustomerBankAccId);
                result.BondPrimary.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
            };

            var bondInfo = _bondInfoRepository.FindById(bondPrimary.BondId);
            if (bondInfo != null)
            {
                result.ProductBondInfo = _mapper.Map<ProductBondInfoDto>(bondInfo);
            };

            var issuer = _issuerRepository.FindById(bondInfo.IssuerId);
            if (issuer != null)
            {
                result.ProductBondInfo.Issuer = new ViewIssuerDto()
                {
                    IssuerId = issuer.Id,
                    BusinessCustomerId = issuer.BusinessCustomerId
                };

                var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                if (businessCustomerIssuer != null)
                {
                    result.ProductBondInfo.Issuer.BusinessCustomer = new BusinessCustomerDto()
                    {
                        BusinessCustomerId = businessCustomerIssuer.BusinessCustomerId,
                        TaxCode = businessCustomerIssuer.TaxCode,
                        ShortName = businessCustomerIssuer.ShortName,
                        Code = businessCustomerIssuer.Code,
                        Name = businessCustomerIssuer.Name,
                    };
                };
            };

            var depositProvider = _depositProviderRepository.FindById(bondInfo.DepositProviderId);
            if (depositProvider != null)
            {
                result.ProductBondInfo.DepositProvider = new DepositProviderDto()
                {
                    DepositProviderId = depositProvider.Id,
                    BusinessCustomerId = depositProvider.BusinessCustomerId,

                };
                var businessCustomerDeposit = _businessCustomerRepository.FindBusinessCustomerById(depositProvider.BusinessCustomerId);
                if (businessCustomerDeposit != null)
                {
                    result.ProductBondInfo.DepositProvider.BusinessCustomer = new BusinessCustomerDto()
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

        public DistributionContractFile FindContractFileById(int id)
        {
            return _distributionContractRepository.FindContractFileById(id);
        }

        public BondDistributionContractPayment FindPaymentById(int id)
        {
            return _distributionContractRepository.FindPaymentById(id);
        }

        public int Update(int contractId, UpdateDistributionContractDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _distributionContractRepository.Update(new BondDistributionContract
            {
                PartnerId = partnerId,
                Id = contractId,
                Quantity = input.Quantity,
                TotalValue = input.TotalValue,
                DateBuy = input.DateBuy,
                ModifiedBy = username
            });
        }

        public int UpdateContractPayment(int paymentId, UpdateDistributionContractPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _distributionContractRepository.UpdatePayment(new BondDistributionContractPayment
            {
                Id = paymentId,
                TransactionType = input.TransactionType,
                PaymentType = input.PaymentType,
                TotalValue = input.TotalValue,
                Description = input.Description,
                ModifiedBy = username,
                TradingDate = input.TradingDate,
            });
        }

        public CouponDto FindCouponById(int id)
        {
            var distributionContract = _distributionContractRepository.FindById(id, null);
            if (distributionContract == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng phân phối"), new FaultCode(((int)ErrorCode.BondDitributionContractNotFound).ToString()), "");
            }

            var bondPrimary = _bondPrimaryRepository.FindById(distributionContract.BondPrimaryId, null);
            if (bondPrimary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy phát hành sơ cấp"), new FaultCode(((int)ErrorCode.BondPrimaryNotFound).ToString()), "");
            }

            var bondInfo = _productBondInfoRepository.FindBondInfoById(bondPrimary.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy lô trái phiếu"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }

            var result = _bondSharedService.CalculateCouponByQuantity(bondInfo, distributionContract.Quantity, distributionContract.PartnerId);
            return result;
        }
    }
}
