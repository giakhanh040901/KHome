using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.Issuer;
using EPIC.Entities.Dto.ProductBondPrimary;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Transactions;

namespace EPIC.BondDomain.Implements
{
    public class BondPrimaryService : IBondPrimaryService
    {
        private readonly ILogger<BondPrimaryService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApproveRepository _approveRepository;
        private readonly BondOrderRepository _bondOrderRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly IMapper _mapper;

        public BondPrimaryService(
            ILogger<BondPrimaryService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _distributionContractRepository = new BondDistributionContractRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public BondPrimary Add(CreateProductBondPrimaryDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = _productBondPrimaryRepository.Add(new BondPrimary
            {
                PartnerId = partnerId,
                BondId = input.ProductBondId,
                TradingProviderId = input.TradingProviderId,
                BusinessCustomerBankAccId = input.BusinessCustomerBankAccId,
                Code = input.Code,
                Name = input.Name,
                ContractCode = input.ContractCode,
                OpenSellDate = input.OpenSellDate,
                CloseSellDate = input.CloseSellDate,
                Quantity = input.Quantity,
                MinMoney = input.MinMoney,
                PriceType = input.PriceType,
                MaxInvestor = input.MaxInvestor,
                CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
            });
            return result;
        }

        public int Delete(int id)
        {
            return _productBondPrimaryRepository.Delete(id, CommonUtils.GetCurrentPartnerId(_httpContext));
        }

        public PagingResult<BondPrimary> FindAll(int pageSize, int pageNumber, string keyword)
        {
            return _productBondPrimaryRepository.FindAll(pageSize, pageNumber, keyword);
        }

        public PagingResult<ProductBondPrimaryDto> FindAllProductBondPrimary(int pageSize, int pageNumber, string keyword, string status)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var bondPrimaryList = _productBondPrimaryRepository.FindAllProductBondPrimary(partnerId, pageSize, pageNumber, keyword, status);
            var result = new PagingResult<ProductBondPrimaryDto>();
            var items = new List<ProductBondPrimaryDto>();
            result.TotalItems = bondPrimaryList.TotalItems;
            foreach (var bondPrimary in bondPrimaryList.Items)
            {
                var tradingProvider = _tradingProviderRepository.FindById(bondPrimary.TradingProviderId);
                var productBondPrimary = _mapper.Map<ProductBondPrimaryDto>(bondPrimary);

                var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryByPrimaryId(bondPrimary.Id);
                decimal quantityOrder = 0;
                decimal soLuongTraiPhieuNamGiu = 0;
                if (bondSecondaryFind != null)
                {
                    soLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(bondSecondaryFind.TradingProviderId, bondPrimary.Id);
                    quantityOrder = _bondOrderRepository.SumQuantity(bondSecondaryFind.TradingProviderId, bondSecondaryFind.Id);
                }
                productBondPrimary.SoLuongTraiPhieuNamGiu = soLuongTraiPhieuNamGiu;
                productBondPrimary.SoLuongTraiPhieuConLai = soLuongTraiPhieuNamGiu - quantityOrder;

                var bondInfo = _productBondInfoRepository.FindById(bondPrimary.BondId);
                if (bondInfo != null)
                {
                    productBondPrimary.BondName = bondInfo.BondName;
                    productBondPrimary.ProductBondInfo = _mapper.Map<ProductBondInfoDto>(bondInfo);

                    var issuer = _issuerRepository.FindById(bondInfo.IssuerId);
                    if (issuer != null)
                    {
                        productBondPrimary.ProductBondInfo.Issuer = new ViewIssuerDto
                        {
                            IssuerId = issuer.Id,
                            BusinessCustomerId = issuer.BusinessCustomerId,
                        };
                        var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                        if (businessCustomerIssuer != null)
                        {
                            productBondPrimary.ProductBondInfo.Issuer.BusinessCustomer = new BusinessCustomerDto
                            {
                                Name = businessCustomerIssuer.Name,
                                ShortName = businessCustomerIssuer.ShortName,
                                TaxCode = businessCustomerIssuer.TaxCode,
                                Email = businessCustomerIssuer.Email,
                            };

                            var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerIssuer.BusinessCustomerId ?? 0, -1, 0, null);
                            productBondPrimary.ProductBondInfo.Issuer.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                        }
                    }
                }
                if (tradingProvider != null)
                {
                    productBondPrimary.TradingProvider = new TradingProviderDto
                    {
                        TradingProviderId = tradingProvider.TradingProviderId,
                    };
                    var businessCustomerTradingProvider = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                    if (businessCustomerTradingProvider != null)
                    {
                        productBondPrimary.TradingProviderName = businessCustomerTradingProvider.Name;
                        productBondPrimary.TradingProvider.BusinessCustomer = new BusinessCustomerDto
                        {
                            Name = businessCustomerTradingProvider.Name,
                            ShortName = businessCustomerTradingProvider.ShortName,
                            TaxCode = businessCustomerTradingProvider.TaxCode,
                            Email = businessCustomerTradingProvider.Email,
                        };

                        var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerTradingProvider.BusinessCustomerId ?? 0, -1, 0, null);
                        productBondPrimary.TradingProvider.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                    }
                }
                items.Add(productBondPrimary);
            }
            result.Items = items;
            return result;
        }

        public ProductBondPrimaryDto FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var productBondPrimary = _productBondPrimaryRepository.FindById(id, partnerId);
            if (productBondPrimary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin phát hành sơ cấp: {id}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            var result = _mapper.Map<ProductBondPrimaryDto>(productBondPrimary);

            var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryByPrimaryId(productBondPrimary.Id);
            decimal quantityOrder = 0;
            decimal soLuongTraiPhieuNamGiu = 0;
            if (bondSecondaryFind != null)
            {
                soLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(bondSecondaryFind.TradingProviderId, productBondPrimary.Id);
                quantityOrder = _bondOrderRepository.SumQuantity(bondSecondaryFind.TradingProviderId, bondSecondaryFind.Id);
            }
            result.SoLuongTraiPhieuNamGiu = soLuongTraiPhieuNamGiu;
            result.SoLuongTraiPhieuConLai = soLuongTraiPhieuNamGiu - quantityOrder;

            result.IsCheck = productBondPrimary.IsCheck;
            var productBondInfo = _productBondInfoRepository.FindById(productBondPrimary.BondId);
            if (productBondInfo != null)
            {
                result.ProductBondInfo = _mapper.Map<ProductBondInfoDto>(productBondInfo);

                //Tính số lượng trái phiếu đã bán = tổng số lượng hợp đồng phân phối trong các phát hành sơ cấp
                var bondPrimaryFind = _productBondPrimaryRepository.GetAllByInfo(productBondInfo.Id);
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
                result.ProductBondInfo.SoLuongConLai = result.ProductBondInfo.Quantity ?? 0 - soLuongDaBan;

                var issuer = _issuerRepository.FindById(productBondInfo.IssuerId);
                if (issuer != null)
                {
                    result.ProductBondInfo.Issuer = new ViewIssuerDto
                    {
                        IssuerId = issuer.Id,
                        BusinessCustomerId = issuer.BusinessCustomerId

                    };
                    var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                    if (businessCustomerIssuer != null)
                    {
                        result.ProductBondInfo.Issuer.BusinessCustomer = new BusinessCustomerDto
                        {
                            Name = businessCustomerIssuer.Name,
                            TaxCode = businessCustomerIssuer.TaxCode,
                            Email = businessCustomerIssuer.Email,
                        };
                        result.ProductBondInfo.Issuer.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerIssuer);

                        var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerIssuer.BusinessCustomerId ?? 0, -1, 0, null);
                        result.ProductBondInfo.Issuer.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                    };
                }
            }

            var tradingProvider = _tradingProviderRepository.FindById(productBondPrimary.TradingProviderId);
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
                    result.TradingProvider.BusinessCustomer = new BusinessCustomerDto()
                    {
                        BusinessCustomerId = businessCustomerTradingProvider.BusinessCustomerId,
                        ShortName = businessCustomerTradingProvider.ShortName,
                        Name = businessCustomerTradingProvider.Name,
                        TaxCode = businessCustomerTradingProvider.TaxCode,
                        Email = businessCustomerTradingProvider.Email,
                    };
                };
            };
            return result;
        }

        public int Update(int id, UpdateProductBondPrimaryDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _productBondPrimaryRepository.Update(new BondPrimary
            {
                Id = id,
                PartnerId = partnerId,
                BondId = input.ProductBondId,
                TradingProviderId = input.TradingProviderId,
                BusinessCustomerBankAccId = input.BusinessCustomerBankAccId,
                ContractCode = input.ContractCode,
                Code = input.Code,
                Name = input.Name,
                OpenSellDate = input.OpenSellDate,
                CloseSellDate = input.CloseSellDate,
                Quantity = input.Quantity,
                MinMoney = input.MinMoney,
                PriceType = input.PriceType,
                MaxInvestor = input.MaxInvestor,
                Status = input.Status,
                ModifiedBy = username
            });
        }

        public BondInfo FindProductBondInfoById(int id)
        {
            return _productBondInfoRepository.FindById(id);
        }

        public void Approve(ApproveStatusDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_PRIMARY);

                if (approve != null)
                {
                    _approveRepository.ApproveRequestStatus(new ApproveRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId
                    });
                }
                _productBondPrimaryRepository.BondPrimaryApprove(input.Id);
                transaction.Complete();
            }
            _productBondPrimaryRepository.CloseConnection();
        }


        public void Request(RequestStatusDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var actionType = ActionTypes.THEM_MOI;
                var checkIsUpdate = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_PRIMARY);
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
                    DataType = CoreApproveDataType.PRODUCT_BOND_PRIMARY,
                    ReferId = input.Id,
                    Summary = input.Summary
                }, null, partnerId);
                _productBondPrimaryRepository.BondPrimaryRequest(input.Id);
                transaction.Complete();
            }
            _productBondPrimaryRepository.CloseConnection();
        }
        public void Check(CheckStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_PRIMARY);
                if (approve != null)
                {
                    _approveRepository.CheckRequest(new CheckRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        UserCheckId = userid,
                    });
                }
                _productBondPrimaryRepository.BondPrimaryCheck(input.Id);
                transaction.Complete();
            }
            _productBondPrimaryRepository.CloseConnection();
        }

        public void Cancel(CancelStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_PRIMARY);
                if (approve != null)
                {
                    _approveRepository.CancelRequest(new CancelRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        CancelNote = input.CancelNote,
                    });
                }
                _productBondPrimaryRepository.BondPrimaryCancel(input.Id);
                transaction.Complete();
            }
            _productBondPrimaryRepository.CloseConnection();
        }

        public int DuyetBondPrimary(int bondPrimaryId, string status)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductBondPrimaryDto> GetAllByCurrentTradingProvider()
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new List<ProductBondPrimaryDto>() { };
            var primaryList = _productBondPrimaryRepository.GetAllByTrading(tradingProviderId);
            foreach (var primaryItem in primaryList)
            {
                var detail = _mapper.Map<ProductBondPrimaryDto>(primaryItem);

                //Số lượng trái phiếu Nắm giữ ở hợp đồng phân phối
                var soLuongTraiPhieuHopDongPhanPhoi = _distributionContractRepository.SumQuantity(tradingProviderId, primaryItem.Id);
                detail.SoLuongTraiPhieuNamGiu = soLuongTraiPhieuHopDongPhanPhoi;
                detail.SoLuongTraiPhieuConLai = primaryItem.Quantity - soLuongTraiPhieuHopDongPhanPhoi;

                var bondSecondaryFind = _productBondSecondaryRepository.FindAll(0, -1, null, 0, tradingProviderId);

                //Tổng số lượng Quantity ở Sổ lệnh (ORDER) đã bán
                decimal soLuongSoLenhDaBan = 0;
                foreach (var bondSecondary in bondSecondaryFind.Items)
                {
                    var sumQuantityOrder = _bondOrderRepository.SumQuantity(tradingProviderId, bondSecondary.Id);
                    soLuongSoLenhDaBan += sumQuantityOrder;
                }
                detail.SoLuongConLai = soLuongTraiPhieuHopDongPhanPhoi - soLuongSoLenhDaBan;

                var productBondInfo = _productBondInfoRepository.FindById(primaryItem.BondId);
                if (productBondInfo != null)
                {
                    detail.BondName = productBondInfo.BondName;
                    detail.BondCode = productBondInfo.BondCode;
                    detail.ProductBondInfo = _mapper.Map<ProductBondInfoDto>(productBondInfo);
                }

                var tradingProvider = _tradingProviderRepository.FindById(primaryItem.TradingProviderId);
                if (tradingProvider != null)
                {
                    detail.TradingProvider = new TradingProviderDto
                    {
                        TradingProviderId = tradingProvider.TradingProviderId,
                    };

                    var businessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                    if (businessCustomerFind != null)
                    {
                        detail.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerFind);
                        var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerFind.BusinessCustomerId ?? 0, -1, 0, null);
                        detail.TradingProvider.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                    }
                }
                result.Add(detail);
            }
            return result;
        }

        public IEnumerable<ProductBondPrimaryDto> GetAllByTradingProvider(int tradingProviderId)
        {
            var result = new List<ProductBondPrimaryDto>() { };
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var primaryList = _productBondPrimaryRepository.GetAllByTradingForPartner(tradingProviderId, partnerId);
            foreach (var primaryItem in primaryList)
            {
                var detail = _mapper.Map<ProductBondPrimaryDto>(primaryItem);

                detail.SoLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(tradingProviderId, primaryItem.Id);
                detail.SoLuongTraiPhieuConLai = primaryItem.Quantity - _distributionContractRepository.SumQuantity(tradingProviderId, primaryItem.Id);
                var productBondInfo = _productBondInfoRepository.FindById(primaryItem.BondId);
                if (productBondInfo != null)
                {
                    detail.BondName = productBondInfo.BondName;
                    detail.ProductBondInfo = _mapper.Map<ProductBondInfoDto>(productBondInfo);
                }
                result.Add(detail);
            }
            return result;
        }
    }
}
