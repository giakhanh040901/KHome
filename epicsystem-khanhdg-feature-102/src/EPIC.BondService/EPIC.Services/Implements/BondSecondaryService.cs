using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondInfoOverviewFile;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.BondEntities.Dto.BondSecondaryOverviewOrg;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.Issuer;
using EPIC.Entities.Dto.JuridicalFile;
using EPIC.Entities.Dto.ProductBond;
using EPIC.Entities.Dto.ProductBondPrimary;
using EPIC.Entities.Dto.ProductBondSecondPrice;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace EPIC.BondDomain.Implements
{
    public class BondSecondaryService : IBondSecondaryService
    {
        private readonly ILogger<BondSecondaryService> _logger;
        private readonly IConfiguration _configuration;
        private readonly FileExcelSecondPrice _fileExcelSecondPrice;
        private readonly string _connectionString;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly BondPolicyTempRepository _productBondPolicyTempRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BondPolicyFileRepository _policyFileRepository;
        private readonly BondGuaranteeAssetRepository _guaranteeAssetRepository;
        private readonly BondOrderRepository _bondOrderRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IBondSharedService _bondSharedService;
        private readonly BondJuridicalFileRepository _juridicalFileRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorSaleRepository _investorSaleRepository;
        private readonly SaleRepository _saleRepository;
        //Overview
        private readonly BondInfoOverviewRepository _bondInfoOverviewRepository;

        public BondSecondaryService(
            ILogger<BondSecondaryService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IOptions<FileExcelSecondPrice> fileExcelSecondPrice,
            IMapper mapper,
            IBondSharedService bondSharedService)
        {
            _logger = logger;
            _configuration = configuration;
            _fileExcelSecondPrice = fileExcelSecondPrice.Value;
            _connectionString = databaseOptions.ConnectionString;
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            _distributionContractRepository = new BondDistributionContractRepository(_connectionString, _logger);
            _productBondPolicyTempRepository = new BondPolicyTempRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _policyFileRepository = new BondPolicyFileRepository(_connectionString, _logger);
            _guaranteeAssetRepository = new BondGuaranteeAssetRepository(_connectionString, _logger);
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _juridicalFileRepository = new BondJuridicalFileRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorSaleRepository = new InvestorSaleRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _bondInfoOverviewRepository = new BondInfoOverviewRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _bondSharedService = bondSharedService;
        }

        public BondSecondary Add(CreateProductBondSecondaryDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            BondSecondary secondary = new BondSecondary();

            using (TransactionScope scope = new TransactionScope())
            {
                secondary = _productBondSecondaryRepository.Add(new BondSecondary
                {
                    TradingProviderId = tradingProviderId,
                    PrimaryId = input.BondPrimaryId,
                    Id = input.SecondaryId,
                    BusinessCustomerBankAccId = input.BusinessCustomerBankAccId,
                    //Quantity = input.Quantity,
                    OpenSellDate = input.OpenCellDate,
                    CloseSellDate = input.CloseCellDate,
                    CreatedBy = username
                });
                scope.Complete();
            }

            _productBondSecondaryRepository.CloseConnection();
            return secondary;
        }

        public PagingResult<ViewProductBondSecondaryDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, bool isActive, string isClose)
        {
            string statusActive = null;
            //string isClose = null;
            if (isActive)
            {
                statusActive = Status.ACTIVE;
                isClose = YesNo.NO;
            }

            int? tradingProviderId = null; 
            var userType = CommonUtils.GetCurrentUsername(_httpContext);
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            }
            var query = _productBondSecondaryRepository.FindAll(pageSize, pageNumber, keyword, status, tradingProviderId, isClose);
            var result = new PagingResult<ViewProductBondSecondaryDto>
            {
                TotalItems = query.TotalItems,
            };

            var items = new List<ViewProductBondSecondaryDto>() { };

            if (query.Items != null)
            {
                foreach (var secondaryItem in query.Items) //lặp secondary
                {
                    var secondary = _mapper.Map<ViewProductBondSecondaryDto>(secondaryItem);
                    var bondPrimary = _productBondPrimaryRepository.FindById(secondaryItem.PrimaryId, null);
                    BondInfo bondInfo = null;
                    if (bondPrimary != null)
                    {
                        secondary.SoLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(secondaryItem.TradingProviderId, bondPrimary.Id);
                        secondary.SoLuongConLai = secondary.SoLuongTraiPhieuNamGiu - _bondOrderRepository.SumQuantity(tradingProviderId, secondary.Id);

                        bondInfo = _productBondInfoRepository.FindById(bondPrimary.BondId);

                        secondary.HanMucToiDa = bondInfo.ParValue * secondary.SoLuongTraiPhieuNamGiu - _bondOrderRepository.SumValue(secondaryItem.TradingProviderId, secondaryItem.Id);
                        secondary.ProductBondPrimary = _mapper.Map<ProductBondPrimaryDto>(bondPrimary);
                    }
                    secondary.ProductBondInfo = bondInfo;
                    secondary.BondName = bondInfo?.BondName;
                    secondary.Policies = new List<ViewProductBondPolicyDto>();

                    var policyList = _productBondSecondaryRepository.GetAllPolicy(secondaryItem.Id, secondary.TradingProviderId, statusActive);
                    foreach (var policyItem in policyList)
                    {
                        var policy = _mapper.Map<ViewProductBondPolicyDto>(policyItem);
                        policy.FakeId = policyItem.Id;
                        policy.Details = new List<ViewProductBondPolicyDetailDto>();

                        var policyDetailList = _productBondSecondaryRepository.GetAllPolicyDetail(policy.Id, secondary.TradingProviderId, statusActive);
                        foreach (var detailItem in policyDetailList)
                        {
                            var detail = _mapper.Map<ViewProductBondPolicyDetailDto>(detailItem);
                            detail.FakeId = detailItem.Id;
                            policy.Details.Add(detail);
                        }
                        secondary.Policies.Add(policy);
                    }
                    items.Add(secondary);
                }
            }
            result.Items = items;
            return result;
        }

        public ViewProductBondSecondaryDto FindById(int id)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUsername(_httpContext);
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            }

            var first = _productBondSecondaryRepository.FindSecondaryById(id, tradingProviderId);
            var result = _mapper.Map<ViewProductBondSecondaryDto>(first);
            var bondPrimary = _productBondPrimaryRepository.FindById(first.PrimaryId, null);
            BondInfo bondInfo = null;
            if (bondPrimary != null)
            {
                result.ProductBondInfo = _productBondInfoRepository.FindById(bondPrimary.BondId);

                result.SoLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(first.TradingProviderId, bondPrimary.Id);
                result.SoLuongConLai = result.SoLuongTraiPhieuNamGiu - _bondOrderRepository.SumQuantity(tradingProviderId, result.Id);

                bondInfo = _productBondInfoRepository.FindById(bondPrimary.BondId);

                result.HanMucToiDa = bondInfo.ParValue * result.SoLuongTraiPhieuNamGiu - _bondOrderRepository.SumValue(first.TradingProviderId, first.Id);
                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(bondPrimary.BusinessCustomerBankAccId);
                if (businessCustomerBank != null)
                {
                    result.businessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
                }
                var tradingProvider = _tradingProviderRepository.FindById(bondPrimary.TradingProviderId);
                if (tradingProvider != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    result.ListBusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
            }

            result.ProductBondPrimary = _mapper.Map<ProductBondPrimaryDto>(bondPrimary);
            result.Policies = new List<ViewProductBondPolicyDto>();

            var policyList = _productBondSecondaryRepository.GetAllPolicy(result.Id, result.TradingProviderId);
            foreach (var policyItem in policyList)
            {
                var policy = _mapper.Map<ViewProductBondPolicyDto>(policyItem);

                policy.FakeId = policyItem.Id;
                policy.Details = new List<ViewProductBondPolicyDetailDto>();

                var policyDetailList = _productBondSecondaryRepository.GetAllPolicyDetail(policy.Id, result.TradingProviderId);
                foreach (var detailItem in policyDetailList)
                {
                    var detail = _mapper.Map<ViewProductBondPolicyDetailDto>(detailItem);
                    detail.FakeId = detailItem.Id;
                    policy.Details.Add(detail);
                }

                result.Policies.Add(policy);
            }

            return result;
        }

        public void IsClose(int bondSecondaryId, string isClose)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if ( userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER )
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            _productBondSecondaryRepository.IsClose(bondSecondaryId, isClose, tradingProviderId);
        }

        public void PolicyIsShowApp(int policyId, string isShowApp)
        {
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _productBondSecondaryRepository.PolicyIsShowApp(policyId, isShowApp, tradingProviderId);
        }

        public void PolicyDetailIsShowApp(int policyDetailId, string isShowApp)
        {
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _productBondSecondaryRepository.PolicyDetailIsShowApp(policyDetailId, isShowApp, tradingProviderId);
        }

        public void SuperAdminApprove(int bondSecondaryId, int status)
        {
            _productBondSecondaryRepository.SuperAdminApprove(bondSecondaryId, status);
        }

        public void TradingProviderApprove(int bondSecondaryId, int status)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _productBondSecondaryRepository.TradingProviderApprove(bondSecondaryId, tradingProviderId, status);
        }

        public void TradingProviderSubmit(int bondSecondaryId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _productBondSecondaryRepository.TradingProviderSubmit(bondSecondaryId, tradingProviderId);
        }

        public void Update(UpdateProductBondSecondaryDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            using (TransactionScope scope = new TransactionScope())
            {
                _productBondSecondaryRepository.Update(new BondSecondary
                {
                    Id = input.Id,
                    TradingProviderId = tradingProviderId,
                    BusinessCustomerBankAccId = input.BusinessCustomerBankAccId,
                    OpenSellDate = input.OpenCellDate,
                    CloseSellDate = input.CloseCellDate,
                    ModifiedBy = username,
                });
                scope.Complete();
            }

            _productBondSecondaryRepository.CloseConnection();
        }

        public void UpdatePolicy(int policyId, UpdateProductBondPolicyDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _productBondSecondaryRepository.UpdatePolicy(new BondPolicy
            {
                TradingProviderId = tradingProviderId,
                Id = body.Id,
                Code = body.Code,
                Name = body.Name,
                Type = body.Type,
                InvestorType = body.InvestorType,
                IncomeTax = body.IncomeTax,
                TransferTax = body.TransferTax,
                MinMoney = body.MinMoney,
                IsTransfer = body.IsTransfer,
                Classify = body.Classify,
                Status = body.Status,
                StartDate = body.StartDate,
                EndDate = body.EndDate,
                Description = body.Description,
                ModifiedBy = username,
            });
        }

        public void UpdatePolicyDetail(int policyDetailId, UpdateProductBondPolicyDetailDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new BondPolicyDetail()
            {
                TradingProviderId = tradingProviderId,
                Id = body.Id,
                STT = body.STT,
                ShortName = body.ShortName,
                Name = body.Name,
                PeriodType = body.PeriodType,
                PeriodQuantity = body.PeriodQuantity,
                InterestType = body.InterestType,
                InterestPeriodQuantity = body.InterestPeriodQuantity,
                InterestPeriodType = body.InterestPeriodType,
                Profit = body.Profit,
                InterestDays = body.InterestDays,
                Status = body.Status,
                ModifiedBy = username,
            };
            if (result.InterestType == InterestTypes.DINH_KY)
            {
                if (result.InterestPeriodQuantity == null || result.InterestPeriodType == null)
                {
                    throw new FaultException(new FaultReason($"Kỳ hạn {result.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }
            _productBondSecondaryRepository.UpdatePolicyDetail(result);
        }

        public void DeletePolicy(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _productBondSecondaryRepository.DeletePolicy(policyId, tradingProviderId);
        }

        public void DeletePolicyDetail(int policyDetailId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _productBondSecondaryRepository.DeletePolicyDetail(policyDetailId, tradingProviderId);
        }

        public void IsShowApp(int bondSecondaryId, string isShowApp)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _productBondSecondaryRepository.IsShowApp(bondSecondaryId, isShowApp, tradingProviderId);
        }

        public List<ProductBondPolicyDto> AddPolicySecondary(int policytempId, List<int> bondsecondaryId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyTemp = _productBondPolicyTempRepository.FindById(policytempId);
            var ListProductBondPolicyDto = new List<ProductBondPolicyDto>();
            foreach (var listbondsecondaryId in bondsecondaryId)
            {
                var productPolicyTemp = _productBondSecondaryRepository.AddPolicy(new BondPolicy
                {
                    SecondaryId = listbondsecondaryId,
                    TradingProviderId = tradingProviderId,
                    Code = policyTemp.Code,
                    Name = policyTemp.Code,
                    Type = policyTemp.Type,
                    InvestorType = policyTemp.InvestorType,
                    IncomeTax = policyTemp.IncomeTax,
                    TransferTax = policyTemp.TransferTax,
                    MinMoney = policyTemp.MinMoney,
                    IsTransfer = policyTemp.IsTransfer,
                    CreatedBy = username
                });
                var result = new ProductBondPolicyDto();
                result = _mapper.Map<ProductBondPolicyDto>(productPolicyTemp);

                var policyDetailTemps = _productBondPolicyTempRepository.FindBondPolicyDetailTempByPolicyTempId(policyTemp.Id);
                if (policyDetailTemps != null)
                {
                    result.PolicyDetail = new List<ProductBondPolicyDetailDto>();
                    foreach (var policyDetailTemp in policyDetailTemps)
                    {
                        var policyDe = new BondPolicyDetail();
                        policyDe.PolicyId = productPolicyTemp.Id;
                        policyDe.PeriodQuantity = (int)policyDetailTemp.PeriodQuantity;
                        policyDe.SecondaryId = productPolicyTemp.SecondaryId;
                        policyDe.STT = policyDetailTemp.STT;
                        policyDe.ShortName = policyDetailTemp.ShortName;
                        policyDe.Name = policyDetailTemp.Name;
                        policyDe.PeriodType = policyDetailTemp.PeriodType;
                        policyDe.InterestType = policyDetailTemp.InterestType;
                        policyDe.InterestPeriodType = policyDetailTemp.InterestPeriodType;
                        policyDe.InterestPeriodQuantity = policyDetailTemp.InterestPeriodQuantity;
                        policyDe.Status = policyDetailTemp.Status;
                        policyDe.Profit = policyDetailTemp.Profit;
                        policyDe.InterestDays = policyDetailTemp.InterestDays;
                        policyDe.CreatedDate = policyDetailTemp.CreatedDate;
                        policyDe.CreatedBy = policyDetailTemp.CreatedBy;
                        policyDe.ModifiedBy = policyDetailTemp.ModifiedBy;
                        policyDe.ModifiedDate = policyDetailTemp.ModifiedDate;
                        policyDe.Deleted = policyDetailTemp.Deleted;
                        policyDe.TradingProviderId = tradingProviderId;
                        var policyDetail = _productBondSecondaryRepository.AddPolicyDetailReturnResult(policyDe);
                        var policyDetailResult = _mapper.Map<ProductBondPolicyDetailDto>(policyDetail);
                        result.PolicyDetail.Add(policyDetailResult);
                    }
                }
                ListProductBondPolicyDto.Add(result);
            }
            return ListProductBondPolicyDto;
        }

        public ProductBondPolicyDto AddPolicy(CreateProductBondPolicySpecificDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var productPolicyTemp = _productBondSecondaryRepository.AddPolicy(new BondPolicy
            {
                SecondaryId = body.SecondaryId,
                TradingProviderId = tradingProviderId,
                Code = body.Code,
                Name = body.Code,
                Type = body.Type,
                InvestorType = body.InvestorType,
                IncomeTax = body.IncomeTax,
                TransferTax = body.TransferTax,
                MinMoney = body.MinMoney,
                IsTransfer = body.IsTransfer,
                Classify = body.Classify,
                StartDate = body.StartDate,
                EndDate = body.EndDate,
                Description = body.Description,
                CreatedBy = username
            });
            var result = new ProductBondPolicyDto();
            result = _mapper.Map<ProductBondPolicyDto>(productPolicyTemp);

            var policyDetailTemps = _productBondPolicyTempRepository.FindBondPolicyDetailTempByPolicyTempId(body.BondPolicyTempId);
            if (policyDetailTemps != null)
            {
                result.PolicyDetail = new List<ProductBondPolicyDetailDto>();
                foreach (var policyDetailTemp in policyDetailTemps)
                {
                    var policyDe = new BondPolicyDetail();
                    policyDe.PolicyId = productPolicyTemp.Id;
                    policyDe.PeriodQuantity = (int)policyDetailTemp.PeriodQuantity;
                    policyDe.SecondaryId = productPolicyTemp.SecondaryId;
                    policyDe.STT = policyDetailTemp.STT;
                    policyDe.ShortName = policyDetailTemp.ShortName;
                    policyDe.Name = policyDetailTemp.Name;
                    policyDe.PeriodType = policyDetailTemp.PeriodType;
                    policyDe.InterestType = policyDetailTemp.InterestType;
                    policyDe.InterestPeriodType = policyDetailTemp.InterestPeriodType;
                    policyDe.InterestPeriodQuantity = policyDetailTemp.InterestPeriodQuantity;
                    policyDe.Status = policyDetailTemp.Status;
                    policyDe.Profit = policyDetailTemp.Profit;
                    policyDe.InterestDays = policyDetailTemp.InterestDays;
                    policyDe.CreatedDate = policyDetailTemp.CreatedDate;
                    policyDe.CreatedBy = policyDetailTemp.CreatedBy;
                    policyDe.ModifiedBy = policyDetailTemp.ModifiedBy;
                    policyDe.ModifiedDate = policyDetailTemp.ModifiedDate;
                    policyDe.Deleted = policyDetailTemp.Deleted;
                    policyDe.TradingProviderId = tradingProviderId;

                    var policyDetail = _productBondSecondaryRepository.AddPolicyDetailReturnResult(policyDe);
                    var policyDetailResult = _mapper.Map<ProductBondPolicyDetailDto>(policyDetail);
                    result.PolicyDetail.Add(policyDetailResult);
                }
            }
            return result;
        }

        public void AddPolicyDetail(CreateProductBondPolicyDetailDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = new BondPolicyDetail()
            {
                TradingProviderId = tradingProviderId,
                PolicyId = body.PolicyId,
                STT = body.STT,
                ShortName = body.ShortName,
                Name = body.Name,
                PeriodType = body.PeriodType,
                PeriodQuantity = body.PeriodQuantity,
                InterestType = body.InterestType,
                InterestPeriodQuantity = body.InterestPeriodQuantity,
                InterestPeriodType = body.InterestPeriodType,
                Profit = body.Profit,
                InterestDays = body.InterestDays,
                CreatedBy = username
            };

            if (result.InterestType == InterestTypes.DINH_KY)
            {
                if (result.InterestPeriodQuantity == null || result.InterestPeriodType == null)
                {
                    throw new FaultException(new FaultReason($"Kỳ hạn {result.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }
            _productBondSecondaryRepository.AddPolicyDetail(result);
        }

        public void ImportSecondPrice(IFormFile file, int bondSecondaryId)
        {
            if (file == null)
            {
                throw new FaultException(new FaultReason($"File được upload không có nội dung."), new FaultCode(((int)ErrorCode.FileUploadNoContent).ToString()), "");
            }
            if (file.Length > _fileExcelSecondPrice.LimitUpload)
            {
                throw new FaultException(new FaultReason($"Kích thước file không được vượt quá {_fileExcelSecondPrice.LimitUpload / (1024 * 1024)} MB."), new FaultCode(((int)ErrorCode.FileOverUploadLimit).ToString()), "");
            }

            MemoryStream memoryStream = new();
            file.CopyTo(memoryStream);
            SpreadsheetDocument doc = SpreadsheetDocument.Open(memoryStream, false);
            WorkbookPart workbookPart = doc.WorkbookPart;

            List<WorksheetPart> worksheetParts = workbookPart.WorksheetParts.ToList();
            WorksheetPart worksheetPart = worksheetParts[0];
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            List<Row> rows = sheetData.Elements<Row>().ToList();

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondSecondaryId, tradingProviderId);

            var rowCount = 0;
            try
            {
                for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                {
                    var row = rows[rowIndex];
                    if (string.IsNullOrWhiteSpace(rows[1].InnerText))
                    {
                        throw new FaultException(new FaultReason($"Dữ liệu dòng đầu tiên trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }
                    if (string.IsNullOrWhiteSpace(row.InnerText))
                    {
                        break;
                    };
                    rowCount++;

                }
                if (rowCount == 0)
                {
                    throw new FaultException(new FaultReason($"File excel không có dữ liệu"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new FaultException(new FaultReason($"File excel không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            Cell cellDateFirst = rows[1].Elements<Cell>().ToList()[0];
            Cell cellDateLast = rows[rowCount - 1].Elements<Cell>().ToList()[0];

            if (!double.TryParse(cellDateFirst.InnerText, out double dateInnerTextFirst))
            {
                throw new FaultException(new FaultReason($"Thông tin ngày tại dòng đầu tiên không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            if (!double.TryParse(cellDateLast.InnerText, out double dateInnerTextLast))
            {
                throw new FaultException(new FaultReason($"Thông tin ngày tại hàng cuối cùng không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            DateTime dateFirst = DateTime.FromOADate(dateInnerTextFirst);
            DateTime dateLast = DateTime.FromOADate(dateInnerTextLast);
            if (dateFirst.ToShortDateString() != bondSecondary.OpenSellDate.Value.ToShortDateString())
            {
                throw new FaultException(new FaultReason($"Dữ liệu ngày ở hàng đầu tiên không trùng với ngày mở bán"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            if (dateLast.ToShortDateString() != bondSecondary.CloseSellDate.Value.ToShortDateString())
            {
                throw new FaultException(new FaultReason($"Dữ liệu ngày ở hàng cuối cùng không trùng với ngày đáo hạn"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            using (TransactionScope scope = new TransactionScope())
            {
                for (int i = 1; i < rowCount; i++)
                {
                    var cells = rows[i].Elements<Cell>().ToList();

                    Cell cellDate = cells[0];

                    Cell cellPrice = cells[1];

                    if (!double.TryParse(cellDate?.InnerText, out double dateInnerText))
                    {
                        throw new FaultException(new FaultReason($"Dòng thứ {i} có dữ liệu không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }

                    if (!double.TryParse(cellPrice.InnerText, out double price))
                    {
                        throw new FaultException(new FaultReason($"Dòng thứ {i} có dữ liệu không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }

                    DateTime date = DateTime.FromOADate(dateInnerText);
                    DateTime datePrevious = DateTime.FromOADate(dateInnerText);

                    if (i > 1)
                    {

                        var cellsPrevious = rows[i - 1]?.Elements<Cell>().ToList();
                        Cell cellDatePrevious = cellsPrevious[0];

                        if (!double.TryParse(cellDatePrevious?.InnerText, out double dateInnerText2))
                        {
                            throw new FaultException(new FaultReason($"Hàng thứ {i} có dữ liệu không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }

                        DateTime datePrevios = DateTime.FromOADate(dateInnerText2);

                        if ((date.Date - datePrevios.Date).Days != 1)
                        {
                            throw new FaultException(new FaultReason($"Dữ liệu ngày hàng thứ {i} và {i + 1} không liên tiếp với nhau "), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }
                    }

                    _productBondSecondPriceRepository.Add(new BondSecondPrice
                    {
                        TradingProviderId = tradingProviderId,
                        SecondaryId = bondSecondaryId,
                        PriceDate = date,
                        Price = (decimal)price,
                        CreatedBy = username
                    });
                }
                scope.Complete();
            }

        }

        public void DeleteSecondPrice(int bondSecondId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _productBondSecondPriceRepository.Delete(bondSecondId, tradingProviderId);
        }

        public PagingResult<BondSecondPrice> FindAll(int pageSize, int pageNumber, int bondSecondaryId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _productBondSecondPriceRepository.FindAll(pageSize, pageNumber, bondSecondaryId, tradingProviderId);
        }

        public void Request(RequestStatusDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                _approveRepository.CreateApproveRequest(new CreateApproveRequestDto
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = ActionTypes.CAP_NHAT,
                    DataType = CoreApproveDataType.PRODUCT_BOND_SECONDARY,
                    ReferId = input.Id,
                    Summary = input.Summary
                }, tradingProviderId);
                _productBondSecondaryRepository.BondSecondaryRequest(input.Id);
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
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_SECONDARY);

                if (approve != null)
                {
                    _approveRepository.ApproveRequestStatus(new ApproveRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId
                    });
                }
                _productBondSecondaryRepository.BondSecondaryApprove(input.Id);
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
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_SECONDARY);
                if (approve != null)
                {
                    _approveRepository.CheckRequest(new CheckRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        UserCheckId = userid,
                    });
                }
                _productBondSecondaryRepository.BondSecondaryCheck(input.Id);
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
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.PRODUCT_BOND_SECONDARY);
                if (approve != null)
                {
                    _approveRepository.CancelRequest(new CancelRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        CancelNote = input.CancelNote,
                    });
                }
                _productBondSecondaryRepository.BondSecondaryCancel(input.Id);
                transaction.Complete();
            }
            _productBondInfoRepository.CloseConnection();
        }

        public void UpdatePrice(UpdateSecondaryPriceDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _productBondSecondPriceRepository.Update(new BondSecondPrice
            {
                Id = body.Id,
                TradingProviderId = tradingProviderId,
                SecondaryId = body.SecondaryId,
                PriceDate = body.PriceDate,
                Price = body.Price,
                ModifiedBy = username,
            });
        }

        public List<BondInfoSecondaryDto> FindAllBondSecondary(string keyword, bool orderByInterestDesc)
        {
            int? investorId = null; //trường hợp không đăng nhập
            int? saleId = null;
            //Danh sách đại lý bảng hàng của investor
            var listTradingProvider = new List<AppListTradingProviderDto>();

            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
                //Kiểm tra xem investor có phải là saler hay không
                var checkSale = _saleRepository.AppCheckSaler(investorId ?? 0);
                if (checkSale.Status == CheckSaler.IsSaler)
                {
                    saleId = CommonUtils.GetCurrentSaleId(_httpContext);
                    //Lấy danh sách đại lý mà sale đang thuộc
                    var listTradingInSale = _saleRepository.AppListTradingProviderBySale(saleId ?? 0);
                    listTradingProvider.AddRange(listTradingInSale);
                }
            }
            catch
            {
                investorId = null;
            }

            var findList = _productBondSecondaryRepository.FindAllProduct(keyword, investorId);

            // Trường hợp INVESTOR chọn SALE là Sale mặc định trong bảng INVESTOR_SALE
            // Nếu Sale được chọn làm mặc định thuộc các ở trong đại lý
            // Nếu đại lý của sale đang thuộc, đại lý đang là sale của đại lý khác (đại lý là sale của kênh bán hộ)
            var listTradingBySaleBusiness = _investorSaleRepository.BusinessSaleListTrading(investorId ?? 0);
            if (listTradingBySaleBusiness.Count > 0)
            {
                listTradingProvider.AddRange(listTradingBySaleBusiness);
            }

            //Nếu có danh sách đại lý mà investor đang thuộc thì lọc bảng hàng
            if (listTradingProvider.Count > 0)
            {
                var tradingIds = listTradingProvider.Select(l => l.TradingProviderId).ToList();
                //Lọc bảng hàng mà investor đã thuộc đại lý trong đấy
                findList = findList.Where(o => tradingIds.Contains(o.TradingProviderId));
            }

            //Sắp xếp theo lợi tức
            findList = findList.OrderByDescending(s => s.Profit);
            if (orderByInterestDesc)
            {
                findList = findList.OrderBy(s => s.Profit);
            }
            var result = _mapper.Map<List<BondInfoSecondaryDto>>(findList);
            return result;
        }

        // Lấy thông tin Lô trái phiếu từ Id của bán theo kỳ hạn 
        public AppBondInfoDto FindBondById(int id)
        {
            var bondSecondaryFind = _productBondSecondaryRepository.AppFindSecondaryById(id, null);
            if (bondSecondaryFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }

            var bondInfo = _productBondInfoRepository.FindBondInfoById(bondSecondaryFind.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô trái phiếu"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }


            //Lấy thông tin của Lô
            var result = _mapper.Map<AppBondInfoDto>(bondInfo);

            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(id, null);
            var bondPrimary = _productBondPrimaryRepository.FindById(bondSecondary.PrimaryId, null);
            if (bondPrimary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin phát hành sơ cấp"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            decimal soLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(bondSecondary.TradingProviderId, bondSecondary.PrimaryId);
            decimal soLuongConLai = soLuongTraiPhieuNamGiu - _bondOrderRepository.SumQuantity(bondSecondary.TradingProviderId, bondSecondary.Id);

            //Khi số lượng trái phiếu nắm giữ (hợp đồng phân phối) bé hơn số lượng cái phiếu đã bán trong sổ lệnh
            if (soLuongConLai < 0)
            {
                soLuongConLai = 0;
            }
            decimal soTienToiDa = soLuongTraiPhieuNamGiu * (bondInfo.ParValue) - _bondOrderRepository.SumValue(bondSecondary.TradingProviderId, bondSecondary.Id);

            //Id của bán theo kỳ hạn
            result.SecondaryId = id;
            //Số tiền tối đa
            result.MaxMoney = soTienToiDa;
            if (bondInfo != null)
            {
                //Lấy thông tin tổng quan
                result.BondInfoOverview = new();
                result.BondInfoOverview.ContentType = bondSecondary.ContentType;
                result.BondInfoOverview.OverviewContent = bondSecondary.OverviewContent;
                result.BondInfoOverview.OverviewImageUrl = bondSecondary.OverviewImageUrl;

                var bondInfotOverviewFile = _bondInfoOverviewRepository.FindAllListFile(bondSecondary.Id, bondSecondary.TradingProviderId);
                result.BondInfoOverview.BondOverviewFiles = _mapper.Map<List<BondInfoOverviewFileDto>>(bondInfotOverviewFile);

                var bondInfoOverviewOrg = _bondInfoOverviewRepository.FindAllListOrg(bondSecondary.Id, bondSecondary.TradingProviderId);
                result.BondInfoOverview.BondOverviewOrgs = _mapper.Map<List<BondInfoOverviewOrgDto>>(bondInfoOverviewOrg);
                //Lấy thông tin của tổ chức phát hành
                var issuerFind = _issuerRepository.FindById(bondInfo.IssuerId);
                if (issuerFind != null)
                {
                    result.Issuer = new AppIssuerDto()
                    {
                        IssuerId = issuerFind.Id,
                        BusinessProfit = issuerFind.BusinessProfit,
                        BusinessTurover = issuerFind.BusinessTurnover,
                        ROA = issuerFind.ROA,
                        ROE = issuerFind.ROE,
                        Image = issuerFind.Image
                    };
                    var businessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(issuerFind.BusinessCustomerId);
                    if (businessCustomerFind != null)
                    {
                        result.Issuer.Name = businessCustomerFind.Name;
                        result.Issuer.TradingAddress = businessCustomerFind.TradingAddress;
                        result.Issuer.Name = businessCustomerFind.Name;
                        result.Issuer.RepName = businessCustomerFind.RepName;
                        result.Issuer.Capital = businessCustomerFind.Capital;
                        result.Issuer.Email = businessCustomerFind.Email;
                        result.Issuer.Phone = businessCustomerFind.Phone;
                        result.Issuer.Website = businessCustomerFind.Website;
                        result.Issuer.Fanpage = businessCustomerFind.Fanpage;
                    };
                }

                //Lấy thông tin chính sách
                if (bondSecondaryFind != null)
                {
                    result.Profit = bondSecondaryFind.Profit;
                    result.TradingProviderName = bondSecondaryFind.TradingProviderName;

                    var policyFileFind = _policyFileRepository.FindAllPolicyFile(bondSecondaryFind.SecondaryId, null, -1, 0, null);
                    if (policyFileFind != null)
                    {
                        result.PolicyFiles = _mapper.Map<List<AppPolicyFileDto>>(policyFileFind.Items);
                    }
                }

                //Lấy thông tin tài sản đảm bảo
                var guaranteeAssetFind = _guaranteeAssetRepository.FindAll(bondSecondaryFind.BondId, -1, 0, null);
                if (guaranteeAssetFind != null)
                {
                    foreach (var item in guaranteeAssetFind.Items)
                    {
                        result.GuaranteeAsset = _mapper.Map<AppGuaranteeAssetDto>(item);

                        var listFileRepo = _guaranteeAssetRepository.FindAllByIdFile(item.GuaranteeAssetId);
                        var listFile = _mapper.Map<List<AppGuaranteeFileDto>>(listFileRepo);
                        result.GuaranteeAsset.GuaranteeFiles = listFile;
                    }
                }

                //Lấy hồ sơ pháp lý
                var juridicalFiles = _juridicalFileRepository.FindAllJuridicalFile(bondSecondaryFind.BondId, -1, 0, null);
                if (juridicalFiles != null)
                {
                    result.JuridicalFiles = new();
                    foreach (var item in juridicalFiles.Items)
                    {
                        result.JuridicalFiles.Add(_mapper.Map<JuridicalFileDto>(item));
                    }
                }

                //Lấy file hợp đồng phân phối
                var distributionContractFile = _distributionContractRepository.AppContractFileFind(bondSecondary.PrimaryId, bondSecondary.TradingProviderId);
                if (distributionContractFile != null)
                {
                    result.DistributionContractFiles = new();
                    foreach (var item in distributionContractFile)
                    {
                        result.DistributionContractFiles.Add(_mapper.Map<DistributionContractFileDto>(item));
                    }
                }
            }
            return result;
        }

        public List<AppBondPolicyDto> FindAllListPolicy(int bondSecondaryId)
        {
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondSecondaryId, null);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }
            var bondPolicies = _productBondSecondaryRepository.FindAllProductPolicy(bondSecondaryId, bondSecondary.TradingProviderId);

            var bondInfo = _productBondInfoRepository.FindById(bondSecondary.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }

            decimal soLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(bondSecondary.TradingProviderId, bondSecondary.PrimaryId);
            decimal soLuongConLai = soLuongTraiPhieuNamGiu - _bondOrderRepository.SumQuantity(bondSecondary.TradingProviderId, bondSecondary.Id);

            //Khi số lượng trái phiếu nắm giữ (hợp đồng phân phối) bé hơn số lượng cái phiếu đã bán trong sổ lệnh
            if (soLuongConLai < 0)
            {
                soLuongConLai = 0;
            }
            decimal soTienToiDa = soLuongTraiPhieuNamGiu * (bondInfo.ParValue) - _bondOrderRepository.SumValue(bondSecondary.TradingProviderId, bondSecondary.Id);
            var result = new List<AppBondPolicyDto>();
            foreach (var item in bondPolicies)
            {
                //Lấy thông tin lợi tức lớn nhất của policy trong
                var bondPolicyDetailFind = _productBondSecondaryRepository.FindPolicyDetailMaxProfit(bondSecondaryId);
                if (bondPolicyDetailFind != null)
                {
                    item.InterestType = bondPolicyDetailFind.InterestType;
                }
                item.MaxMoney = soTienToiDa;
                result.Add(item);
            }
            return result;
        }

        public List<AppBondPolicyDetailDto> FindAllListPolicyDetail(int policyId, decimal totalValue)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var bondPolicyFind = _productBondSecondaryRepository.FindPolicyById(policyId, null);
            if (bondPolicyFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            var bondPolicyDetailFind = _productBondSecondaryRepository.FindAllProductPolicyDetail(policyId, null);
            var result = _mapper.Map<List<AppBondPolicyDetailDto>>(bondPolicyDetailFind);
            DateTime ngayBatDauTinhLai = DateTime.Now.Date;
            foreach (var item in result)
            {
                var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(item.Id);
                if (bondPolicyDetail == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
                }

                var bondPolicy = _productBondSecondaryRepository.FindPolicyById(bondPolicyDetail.PolicyId, bondPolicyDetail.TradingProviderId);
                if (bondPolicy == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
                }

                var calProfitResult = _bondSharedService.CalculateProfit(bondPolicy, bondPolicyDetail, ngayBatDauTinhLai, totalValue, true);
                item.CalProfit = calProfitResult.ActuallyProfit;
            }
            return result;
        }

        public List<BondPolicyDetail> GetAllListPolicyDetailByPolicy(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetails = _productBondSecondaryRepository.GetAllPolicyDetail(policyId, tradingProviderId);
            var result = new List<BondPolicyDetail>();
            foreach (var item in policyDetails)
            {
                result.Add(item);
            }
            return result;
        }

        public int ChangePolicyStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var prolicyId = _productBondSecondaryRepository.FindPolicyById(id, tradingProviderId);
            var status = BondPolicyTemplate.ACTIVE;
            if (prolicyId.Status == BondPolicyTemplate.ACTIVE)
            {
                status = BondPolicyTemplate.DEACTIVE;
            }
            else
            {
                status = BondPolicyTemplate.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _productBondSecondaryRepository.UpdatePolicyStatus(id, status, username);
        }
        public int ChangePolicyDetailStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetailId = _productBondSecondaryRepository.FindPolicyDetailById(id, tradingProviderId);
            var status = BondPolicyDetailTemplate.ACTIVE;
            if (policyDetailId.Status == BondPolicyDetailTemplate.ACTIVE)
            {
                status = BondPolicyDetailTemplate.DEACTIVE;
            }
            else
            {
                status = BondPolicyDetailTemplate.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _productBondSecondaryRepository.UpdatePolicyDetailStatus(id, status, username);
        }

        public List<ViewProductBondSecondaryDto> FindAllOrder()
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var bondSecondaryList = _productBondSecondaryRepository.FindAllOrder(tradingProviderId);
            var result = new List<ViewProductBondSecondaryDto>();

            if (bondSecondaryList != null)
            {
                foreach (var secondaryItem in bondSecondaryList) //lặp secondary
                {
                    var secondary = _mapper.Map<ViewProductBondSecondaryDto>(secondaryItem);
                    var bondPrimary = _productBondPrimaryRepository.FindById(secondaryItem.PrimaryId, null);
                    BondInfo bondInfo = null;
                    if (bondPrimary != null)
                    {
                        secondary.SoLuongTraiPhieuNamGiu = _distributionContractRepository.SumQuantity(tradingProviderId, bondPrimary.Id);
                        secondary.SoLuongConLai = secondary.SoLuongTraiPhieuNamGiu - _bondOrderRepository.SumQuantity(tradingProviderId, secondary.Id);

                        bondInfo = _productBondInfoRepository.FindById(bondPrimary.BondId);

                        secondary.HanMucToiDa = bondInfo.ParValue * secondary.SoLuongTraiPhieuNamGiu - _bondOrderRepository.SumValue(tradingProviderId, secondaryItem.Id);
                        secondary.ProductBondPrimary = _mapper.Map<ProductBondPrimaryDto>(bondPrimary);
                    }
                    secondary.ProductBondInfo = bondInfo;
                    secondary.BondName = bondInfo?.BondName;
                    secondary.Policies = new List<ViewProductBondPolicyDto>();

                    var policyList = _productBondSecondaryRepository.GetAllPolicyOrder(secondaryItem.Id, secondary.TradingProviderId);
                    foreach (var policyItem in policyList)
                    {
                        var policy = _mapper.Map<ViewProductBondPolicyDto>(policyItem);
                        policy.FakeId = policyItem.Id;
                        policy.Details = new List<ViewProductBondPolicyDetailDto>();

                        var policyDetailList = _productBondSecondaryRepository.GetAllPolicyDetail(policy.Id, secondary.TradingProviderId, Status.ACTIVE);
                        foreach (var detailItem in policyDetailList)
                        {
                            var detail = _mapper.Map<ViewProductBondPolicyDetailDto>(detailItem);
                            detail.FakeId = detailItem.Id;
                            policy.Details.Add(detail);
                        }
                        secondary.Policies.Add(policy);
                    }
                    result.Add(secondary);
                }
            }
            return result;
        }
    }
}

