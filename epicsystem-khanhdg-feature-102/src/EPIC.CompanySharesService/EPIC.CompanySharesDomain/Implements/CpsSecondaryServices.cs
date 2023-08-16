using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.JuridicalFile;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CpsSecondary;
using EPIC.Utils.ConstantVariables.CompanyShares;
using EPIC.CompanySharesEntities.Dto.CpsApp;
using EPIC.CompanySharesEntities.Dto.Policy;
using EPIC.CompanySharesEntities.Dto;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;
using EPIC.CompanySharesEntities.Dto.CpsInfo;
using EPIC.CompanySharesEntities.Dto.CpsSecondPrice;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsSecondaryServices : ICpsSecondaryServices
    {
        private readonly ILogger<CpsSecondaryServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly FileExcelSecondPrice _fileExcelSecondPrice;
        private readonly string _connectionString;
        private readonly CpsSecondaryRepository _cpsSecondaryRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly CpsInfoRepository _cpsInfoRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly CpsSecondaryPriceRepository _cpsSecondaryPriceRepository;
        private readonly PolicyTempRepository _cpsPolicyTempRepository;
        private readonly ApproveRepository _approveRepository;
        //private readonly BondIssuerRepository _issuerRepository;
        //private readonly BondPolicyFileRepository _policyFileRepository;
        //private readonly BondGuaranteeAssetRepository _guaranteeAssetRepository;
        private readonly OrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly ICpsSharedServices _cpsSharedService;
        //private readonly BondJuridicalFileRepository _juridicalFileRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorSaleRepository _investorSaleRepository;
        private readonly SaleRepository _saleRepository;
        //Overview
        //private readonly CpsInfoOverviewRepository _cpsInfoOverviewRepository;

        public CpsSecondaryServices(
            ILogger<CpsSecondaryServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IOptions<FileExcelSecondPrice> fileExcelSecondPrice,
            IMapper mapper,
            ICpsSharedServices cpsSharedService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _fileExcelSecondPrice = fileExcelSecondPrice.Value;
            _connectionString = databaseOptions.ConnectionString;
            _cpsSecondaryRepository = new CpsSecondaryRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _cpsInfoRepository = new CpsInfoRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _cpsSecondaryPriceRepository = new CpsSecondaryPriceRepository(_connectionString, _logger);
            _cpsPolicyTempRepository = new PolicyTempRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            //_issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            //_policyFileRepository = new BondPolicyFileRepository(_connectionString, _logger);
            //_guaranteeAssetRepository = new BondGuaranteeAssetRepository(_connectionString, _logger);
            _orderRepository = new OrderRepository(_connectionString, _logger);
            //_juridicalFileRepository = new BondJuridicalFileRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorSaleRepository = new InvestorSaleRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            //_cpsInfoOverviewRepository = new CpsInfoOverviewRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _cpsSharedService = cpsSharedService;
        }

        private void AddTradingBankAccount(List<int?> BusinessCustomerBankAcc, int secondaryId, string username, int tradingProviderId)
        {
            // Thêm nhiều tài khoản ngân hàng thụ hưởng
            if (BusinessCustomerBankAcc != null || BusinessCustomerBankAcc.Count != 0)
            {
                var updateTradingBankAccountFind = _cpsSecondaryRepository.GetAllTradingBankByDistribution(secondaryId);
                //Xóa đi những ngân hàng ko được truyền vào
                var updateTradingBankAccountRemove = updateTradingBankAccountFind.Where(p => !BusinessCustomerBankAcc.Contains(p.BusinessCustomerBankAccId)).ToList();
                foreach (var bankAccountItem in updateTradingBankAccountRemove)
                {
                    _cpsSecondaryRepository.DeletedDistributionTradingBankAcc(bankAccountItem.Id);
                }
                foreach (var item in BusinessCustomerBankAcc)
                {
                    // Nếu là thêm mới thì thêm vào
                    // Nếu loại ngân hàng chưa có trong list thì thêm vào, nếu đã có thì giữ nguyên
                    _cpsSecondaryRepository.AddDistributionTradingBankAcc(new SecondaryTradingBankAccount
                    {
                        SecondaryId = secondaryId,
                        BusinessCustomerBankAccId = item.Value,
                        CreatedBy = username,
                    }, tradingProviderId);
                }
            };
        }

        public CpsSecondary Add(CreateCpsSecondaryDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            CpsSecondary secondary = new CpsSecondary();

            using (TransactionScope scope = new TransactionScope())
            {
                secondary = _cpsSecondaryRepository.Add(new CpsSecondary
                {
                    TradingProviderId = tradingProviderId,
                    Id = input.SecondaryId,
                    //Quantity = input.Quantity,
                    OpenCellDate = input.OpenCellDate,
                    CloseCellDate = input.CloseCellDate,
                    CreatedBy = username
                });
                AddTradingBankAccount(input.BusinessCustomerBankAcc, secondary.Id, username, tradingProviderId);
                scope.Complete();
            }

            _cpsSecondaryRepository.CloseConnection();
            return secondary;
        }

        public PagingResult<ViewCpsSecondaryDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, bool isActive, string isClose)
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
            var query = _cpsSecondaryRepository.FindAll(pageSize, pageNumber, keyword, status, tradingProviderId, isClose);
            var result = new PagingResult<ViewCpsSecondaryDto>
            {
                TotalItems = query.TotalItems,
            };

            var items = new List<ViewCpsSecondaryDto>() { };

            if (query.Items != null)
            {
                foreach (var secondaryItem in query.Items) //lặp secondary
                {
                    var secondary = _mapper.Map<ViewCpsSecondaryDto>(secondaryItem);
                    /*var cpsPrimary = _cpsPrimaryRepository.FindById(secondaryItem.CpsPrimaryId, null);
                    CpsInfo cpsInfo = null;
                    if (cpsPrimary != null)
                    {
                        secondary.SoLuongCoPhanNamGiu = _distributionContractRepository.SumQuantity(secondaryItem.TradingProviderId, cpsPrimary.CpsPrimaryId);
                        secondary.SoLuongConLai = secondary.SoLuongCoPhanNamGiu - _orderRepository.SumQuantity(tradingProviderId, secondary.CpsSecondaryId);

                        cpsInfo = _cpsInfoRepository.FindById(cpsPrimary.CpsId);

                        secondary.HanMucToiDa = cpsInfo.ParValue * secondary.SoLuongCoPhanNamGiu - _orderRepository.SumValue(secondaryItem.TradingProviderId, secondaryItem.CpsSecondaryId);
                        secondary.CpsPrimary = _mapper.Map<CpsPrimaryDto>(cpsPrimary);
                    }
                    secondary.CpsInfo = cpsInfo;
                    secondary.CpsName = cpsInfo?.CpsName;*/
                    secondary.Policies = new List<ViewCpsPolicyDto>();

                    var policyList = _cpsSecondaryRepository.GetAllPolicy(secondaryItem.Id, secondary.TradingProviderId, statusActive);
                    foreach (var policyItem in policyList)
                    {
                        var policy = _mapper.Map<ViewCpsPolicyDto>(policyItem);
                        policy.FakeId = policyItem.Id;
                        policy.Details = new List<ViewCpsPolicyDetailDto>();

                        var policyDetailList = _cpsSecondaryRepository.GetAllPolicyDetail(policy.Id, secondary.TradingProviderId, statusActive);
                        foreach (var detailItem in policyDetailList)
                        {
                            var detail = _mapper.Map<ViewCpsPolicyDetailDto>(detailItem);
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

        public ViewCpsSecondaryDto FindById(int id)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUsername(_httpContext);
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            var first = _cpsSecondaryRepository.FindSecondaryById(id, tradingProviderId);
            var result = _mapper.Map<ViewCpsSecondaryDto>(first);
            /*var cpsPrimary = _cpsPrimaryRepository.FindById(first.CpsPrimaryId, null);
            CpsInfo cpsInfo = null;
            if (cpsPrimary != null)
            {
                result.CpsInfo = _cpsInfoRepository.FindById(cpsPrimary.CpsId);

                result.SoLuongCoPhanNamGiu = _distributionContractRepository.SumQuantity(first.TradingProviderId, cpsPrimary.CpsPrimaryId);
                result.SoLuongConLai = result.SoLuongCoPhanNamGiu - _orderRepository.SumQuantity(tradingProviderId, result.CpsSecondaryId);

                cpsInfo = _cpsInfoRepository.FindById(cpsPrimary.CpsId);

                result.HanMucToiDa = cpsInfo.ParValue * result.SoLuongCoPhanNamGiu - _orderRepository.SumValue(first.TradingProviderId, first.CpsSecondaryId);
                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(cpsPrimary.BusinessCustomerBankAccId ?? 0);
                if (businessCustomerBank != null)
                {
                    result.businessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
                }
                var tradingProvider = _tradingProviderRepository.FindById(cpsPrimary.TradingProviderId);
                if (tradingProvider != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    result.ListBusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
            }

            result.CpsPrimary = _mapper.Map<CpsPrimaryDto>(cpsPrimary);*/
            result.Policies = new List<ViewCpsPolicyDto>();

            var policyList = _cpsSecondaryRepository.GetAllPolicy(result.Id, result.TradingProviderId);
            foreach (var policyItem in policyList)
            {
                var policy = _mapper.Map<ViewCpsPolicyDto>(policyItem);

                policy.FakeId = policyItem.Id;
                policy.Details = new List<ViewCpsPolicyDetailDto>();

                var policyDetailList = _cpsSecondaryRepository.GetAllPolicyDetail(policy.Id, result.TradingProviderId);
                foreach (var detailItem in policyDetailList)
                {
                    var detail = _mapper.Map<ViewCpsPolicyDetailDto>(detailItem);
                    detail.FakeId = detailItem.Id;
                    policy.Details.Add(detail);
                }

                result.Policies.Add(policy);
            }

            return result;
        }

        public void IsClose(int cpsSecondaryId, string isClose)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            _cpsSecondaryRepository.IsClose(cpsSecondaryId, isClose, tradingProviderId);
        }

        public void PolicyIsShowApp(int policyId, string isShowApp)
        {
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _cpsSecondaryRepository.PolicyIsShowApp(policyId, isShowApp, tradingProviderId);
        }

        public void PolicyDetailIsShowApp(int policyDetailId, string isShowApp)
        {
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _cpsSecondaryRepository.PolicyDetailIsShowApp(policyDetailId, isShowApp, tradingProviderId);
        }

        public void SuperAdminApprove(int cpsSecondaryId, int status)
        {
            _cpsSecondaryRepository.SuperAdminApprove(cpsSecondaryId, status);
        }

        public void TradingProviderApprove(int cpsSecondaryId, int status)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _cpsSecondaryRepository.TradingProviderApprove(cpsSecondaryId, tradingProviderId, status);
        }

        public void TradingProviderSubmit(int cpsSecondaryId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _cpsSecondaryRepository.TradingProviderSubmit(cpsSecondaryId, tradingProviderId);
        }

        public void Update(UpdateCpsSecondaryDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            using (TransactionScope scope = new TransactionScope())
            {
                _cpsSecondaryRepository.Update(new CpsSecondary
                {
                    Id = input.SecondaryId,
                    TradingProviderId = tradingProviderId,
                    OpenCellDate = input.OpenCellDate,
                    CloseCellDate = input.CloseCellDate,
                    ModifiedBy = username,
                });
                AddTradingBankAccount(input.BusinessCustomerBankAcc, input.SecondaryId, username, tradingProviderId);
                scope.Complete();
            }

            _cpsSecondaryRepository.CloseConnection();
        }

        public void UpdatePolicy(int policyId, UpdateCpsPolicyDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _cpsSecondaryRepository.UpdatePolicy(new CpsPolicy
            {
                TradingProviderId = tradingProviderId,
                Id = body.PolicyId,
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

        public void UpdatePolicyDetail(int policyDetailId, UpdateCpsPolicyDetailDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new CpsPolicyDetail()
            {
                TradingProviderId = tradingProviderId,
                Id = body.PolicyDetailId,
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
            _cpsSecondaryRepository.UpdatePolicyDetail(result);
        }

        public void DeletePolicy(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _cpsSecondaryRepository.DeletePolicy(policyId, tradingProviderId);
        }

        public void DeletePolicyDetail(int policyDetailId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _cpsSecondaryRepository.DeletePolicyDetail(policyDetailId, tradingProviderId);
        }

        public void IsShowApp(int cpsSecondaryId, string isShowApp)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _cpsSecondaryRepository.IsShowApp(cpsSecondaryId, isShowApp, tradingProviderId);
        }

        public List<CpsPolicyDto> AddPolicySecondary(int policytempId, List<int> cpssecondaryId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyTemp = _cpsPolicyTempRepository.FindById(policytempId);
            var ListCpsPolicyDto = new List<CpsPolicyDto>();
            foreach (var listcpssecondaryId in cpssecondaryId)
            {
                var productPolicyTemp = _cpsSecondaryRepository.AddPolicy(new CpsPolicy
                {
                    SecondaryId = listcpssecondaryId,
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
                var result = new CpsPolicyDto();
                result = _mapper.Map<CpsPolicyDto>(productPolicyTemp);

                var policyDetailTemps = _cpsPolicyTempRepository.FindPolicyDetailTempByPolicyTempId(policyTemp.Id);
                if (policyDetailTemps != null)
                {
                    result.PolicyDetails = new List<CpsPolicyDetailDto>();
                    foreach (var policyDetailTemp in policyDetailTemps)
                    {
                        var policyDe = new CpsPolicyDetail();
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
                        var policyDetail = _cpsSecondaryRepository.AddPolicyDetail(policyDe);
                        var policyDetailResult = _mapper.Map<CpsPolicyDetailDto>(policyDetail);
                        result.PolicyDetails.Add(policyDetailResult);
                    }
                }
                ListCpsPolicyDto.Add(result);
            }
            return ListCpsPolicyDto;
        }

        public CpsPolicyDto AddPolicy(CreateCpsPolicySpecificDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var productPolicyTemp = _cpsSecondaryRepository.AddPolicy(new CpsPolicy
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
            var result = new CpsPolicyDto();
            result = _mapper.Map<CpsPolicyDto>(productPolicyTemp);

            var policyDetailTemps = _cpsPolicyTempRepository.FindPolicyDetailTempByPolicyTempId(body.PolicyTempId);
            if (policyDetailTemps != null)
            {
                result.PolicyDetails = new List<CpsPolicyDetailDto>();
                foreach (var policyDetailTemp in policyDetailTemps)
                {
                    var policyDe = new CpsPolicyDetail();
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

                    var policyDetail = _cpsSecondaryRepository.AddPolicyDetail(policyDe);
                    var policyDetailResult = _mapper.Map<CpsPolicyDetailDto>(policyDetail);
                    result.PolicyDetails.Add(policyDetailResult);
                }
            }
            return result;
        }

        public void AddPolicyDetail(CreateCpsPolicyDetailDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = new CpsPolicyDetail()
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
            _cpsSecondaryRepository.AddPolicyDetail(result);
        }

        public void ImportSecondPrice(IFormFile file, int cpsSecondaryId)
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
            var cpsSecondary = _cpsSecondaryRepository.FindSecondaryById(cpsSecondaryId, tradingProviderId);

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
            if (dateFirst.ToShortDateString() != cpsSecondary.OpenCellDate.Value.ToShortDateString())
            {
                throw new FaultException(new FaultReason($"Dữ liệu ngày ở hàng đầu tiên không trùng với ngày mở bán"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            if (dateLast.ToShortDateString() != cpsSecondary.CloseCellDate.Value.ToShortDateString())
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

                    _cpsSecondaryPriceRepository.Add(new CpsSecondaryPrice
                    {
                        TradingProviderId = tradingProviderId,
                        SecondaryId = cpsSecondaryId,
                        PriceDate = date,
                        Price = (decimal)price,
                        CreatedBy = username
                    });
                }
                scope.Complete();
            }

        }

        public void DeleteSecondPrice(int cpsSecondId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _cpsSecondaryPriceRepository.Delete(cpsSecondId, tradingProviderId);
        }

        public PagingResult<CpsSecondaryPrice> FindAll(int pageSize, int pageNumber, int cpsSecondaryId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _cpsSecondaryPriceRepository.FindAll(pageSize, pageNumber, cpsSecondaryId, tradingProviderId);
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
                    ActionType = ApproveAction.CAP_NHAT,
                    DataType = CpsDataTypes.COMPANY_SHARES_SECONDARY,
                    ReferId = input.Id,
                    Summary = input.Summary
                }, tradingProviderId);
                _cpsSecondaryRepository.CpsSecondaryRequest(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Duyet
        /// </summary>
        /// <param name="input"></param>
        public void Approve(ApproveStatusDto input)
        {
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CpsDataTypes.COMPANY_SHARES_SECONDARY);

                if (approve != null)
                {
                    _approveRepository.ApproveRequestStatus(new ApproveRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        ApproveNote = input.ApproveNote
                    });
                }
                _cpsSecondaryRepository.CpsSecondaryApprove(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
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
                var approve = _approveRepository.GetOneByActual(input.Id, CpsDataTypes.COMPANY_SHARES_SECONDARY);
                if (approve != null)
                {
                    _approveRepository.CheckRequest(new CheckRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        UserCheckId = userid,
                    });
                }
                _cpsSecondaryRepository.CpsSecondaryCheck(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Huy
        /// </summary>
        public void Cancel(CancelStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CpsDataTypes.COMPANY_SHARES_SECONDARY);
                if (approve != null)
                {
                    _approveRepository.CancelRequest(new CancelRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        CancelNote = input.CancelNote,
                    });
                }
                _cpsSecondaryRepository.CpsSecondaryCancel(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
        }

        public void UpdatePrice(UpdateSecondaryPriceDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _cpsSecondaryPriceRepository.Update(new CpsSecondaryPrice
            {
                Id = body.PriceId,
                TradingProviderId = tradingProviderId,
                SecondaryId = body.CpsSecondaryId,
                PriceDate = body.PriceDate,
                Price = body.Price,
                ModifiedBy = username,
            });
        }

        public List<AppCpsPolicyDetailDto> 
            FindAllListPolicyDetail(int policyId, decimal totalValue)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var cpsPolicyFind = _cpsSecondaryRepository.FindPolicyById(policyId, null);
            if (cpsPolicyFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }

            var cpsPolicyDetailFind = _cpsSecondaryRepository.FindAllPolicyDetail(policyId, null);
            var result = _mapper.Map<List<AppCpsPolicyDetailDto>>(cpsPolicyDetailFind);
            DateTime ngayBatDauTinhLai = DateTime.Now.Date;
            foreach (var item in result)
            {
                var cpsPolicyDetail = _cpsSecondaryRepository.FindPolicyDetailById(item.Id);
                if (cpsPolicyDetail == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.CpsPolicyDetailNotFound).ToString()), "");
                }

                var cpsPolicy = _cpsSecondaryRepository.FindPolicyById(cpsPolicyDetail.PolicyId, cpsPolicyDetail.TradingProviderId);
                if (cpsPolicy == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
                }

                var calProfitResult = _cpsSharedService.CalculateProfit(cpsPolicy, cpsPolicyDetail, ngayBatDauTinhLai, totalValue, true);
                item.CalProfit = calProfitResult.ActuallyProfit;
            }
            return result;
        }

        public List<CpsPolicyDetail> GetAllListPolicyDetailByPolicy(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetails = _cpsSecondaryRepository.GetAllPolicyDetail(policyId, tradingProviderId);
            var result = new List<CpsPolicyDetail>();
            foreach (var item in policyDetails)
            {
                result.Add(item);
            }
            return result;
        }

        public int ChangePolicyStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var prolicyId = _cpsSecondaryRepository.FindPolicyById(id, tradingProviderId);
            var status = CpsPolicyTemplate.ACTIVE;
            if (prolicyId.Status == CpsPolicyTemplate.ACTIVE)
            {
                status = CpsPolicyTemplate.DEACTIVE;
            }
            else
            {
                status = CpsPolicyTemplate.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _cpsSecondaryRepository.UpdatePolicyStatus(id, status, username);
        }
        public int ChangePolicyDetailStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetailId = _cpsSecondaryRepository.FindPolicyDetailById(id, tradingProviderId);
            var status = CpsPolicyDetailTemplate.ACTIVE;
            if (policyDetailId.Status == CpsPolicyDetailTemplate.ACTIVE)
            {
                status = CpsPolicyDetailTemplate.DEACTIVE;
            }
            else
            {
                status = CpsPolicyDetailTemplate.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _cpsSecondaryRepository.UpdatePolicyDetailStatus(id, status, username);
        }

        public List<ViewCpsSecondaryDto> FindAllOrder()
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var cpsSecondaryList = _cpsSecondaryRepository.FindAllOrder(tradingProviderId);
            var result = new List<ViewCpsSecondaryDto>();

            if (cpsSecondaryList != null)
            {
                foreach (var secondaryItem in cpsSecondaryList) //lặp secondary
                {
                    var secondary = _mapper.Map<ViewCpsSecondaryDto>(secondaryItem);
                    /*var cpsPrimary = _cpsPrimaryRepository.FindById(secondaryItem.CpsPrimaryId, null);
                    CpsInfo cpsInfo = null;
                    if (cpsPrimary != null)
                    {
                        secondary.SoLuongCoPhanNamGiu = _distributionContractRepository.SumQuantity(tradingProviderId, cpsPrimary.CpsPrimaryId);
                        secondary.SoLuongConLai = secondary.SoLuongCoPhanNamGiu - _orderRepository.SumQuantity(tradingProviderId, secondary.CpsSecondaryId);

                        cpsInfo = _cpsInfoRepository.FindById(cpsPrimary.CpsId);

                        secondary.HanMucToiDa = cpsInfo.ParValue * secondary.SoLuongCoPhanNamGiu - _orderRepository.SumValue(tradingProviderId, secondaryItem.CpsSecondaryId);
                        secondary.CpsPrimary = _mapper.Map<CpsPrimaryDto>(cpsPrimary);
                    }
                    secondary.CpsInfo = cpsInfo;
                    secondary.CpsName = cpsInfo?.CpsName;*/
                    secondary.Policies = new List<ViewCpsPolicyDto>();

                    var policyList = _cpsSecondaryRepository.GetAllPolicyOrder(secondaryItem.Id, secondary.TradingProviderId);
                    foreach (var policyItem in policyList)
                    {
                        var policy = _mapper.Map<ViewCpsPolicyDto>(policyItem);
                        policy.FakeId = policyItem.Id;
                        policy.Details = new List<ViewCpsPolicyDetailDto>();

                        var policyDetailList = _cpsSecondaryRepository.GetAllPolicyDetail(policy.Id, secondary.TradingProviderId, CpsPolicyStatus.ACTIVE);
                        foreach (var detailItem in policyDetailList)
                        {
                            var detail = _mapper.Map<ViewCpsPolicyDetailDto>(detailItem);
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

        #region App
        public List<CpsInfoSecondaryDto> FindAllCpsSecondary(string keyword, bool orderByInterestDesc)
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

            var findList = _cpsSecondaryRepository.FindAllProduct(keyword, investorId);

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
            var result = _mapper.Map<List<CpsInfoSecondaryDto>>(findList);
            return result;
        }

        // Lấy thông tin Lô cổ phần từ Id của bán theo kỳ hạn 
        public AppCpsInfoDto FindCpsById(int id)
        {
            var cpsSecondaryFind = _cpsSecondaryRepository.AppFindSecondaryById(id, null);
            if (cpsSecondaryFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.CpsSecondaryNotFound).ToString()), "");
            }

            var cpsInfo = _cpsInfoRepository.FindById(cpsSecondaryFind.SecondaryId);
            if (cpsInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô cổ phần"), new FaultCode(((int)ErrorCode.CpsInfoNotFound).ToString()), "");
            }
            
            //Lấy thông tin của Lô
            var result = _mapper.Map<AppCpsInfoDto>(cpsInfo);
            var cpsSecondary = _cpsSecondaryRepository.FindSecondaryById(id, null);

            //decimal soLuongCoPhanNamGiu = _distributionContractRepository.SumQuantity(cpsSecondary.TradingProviderId, cpsSecondary.PrimaryId);
            //decimal soLuongConLai = soLuongCoPhanNamGiu - _orderRepository.SumQuantity(cpsSecondary.TradingProviderId, cpsSecondary.Id);

            //Khi số lượng cổ phần nắm giữ (hợp đồng phân phối) bé hơn số lượng cái phiếu đã bán trong sổ lệnh
            //if (soLuongConLai < 0)
            //{
            //    soLuongConLai = 0;
            //}
            //decimal soTienToiDa = soLuongCoPhanNamGiu * (cpsInfo.ParValue ?? 0) - _orderRepository.SumValue(cpsSecondary.TradingProviderId, cpsSecondary.Id);
            
            //Id của bán theo kỳ hạn
            result.SecondaryId = id;
            //Số tiền tối đa
            //result.MaxMoney = soTienToiDa;
            if (cpsInfo != null)
            {
                //Lấy thông tin tổng quan
                result.CpsInfoOverview = new();
                result.CpsInfoOverview.ContentType = cpsSecondary.ContentType;
                result.CpsInfoOverview.OverviewContent = cpsSecondary.OverviewContent;
                result.CpsInfoOverview.OverviewImageUrl = cpsSecondary.OverviewImageUrl;

                /*var cpsInfotOverviewFile = _cpsInfoOverviewRepository.FindAllListFile(cpsSecondary.Id, cpsSecondary.TradingProviderId);
                result.CpsInfoOverview.CpsOverviewFiles = _mapper.Map<List<CpsInfoOverviewFileDto>>(cpsInfotOverviewFile);

                var cpsInfoOverviewOrg = _cpsInfoOverviewRepository.FindAllListOrg(cpsSecondary.Id, cpsSecondary.TradingProviderId);
                result.CpsInfoOverview.CpsOverviewOrgs = _mapper.Map<List<CpsInfoOverviewOrgDto>>(cpsInfoOverviewOrg);
                //Lấy thông tin của tổ chức phát hành
                var issuerFind = _issuerRepository.FindById(cpsInfo.IssuerId);
                if (issuerFind != null)
                {
                    result.BondIssuer = new AppIssuerDto()
                    {
                        IssuerId = issuerFind.IssuerId,
                        BusinessProfit = issuerFind.BusinessProfit,
                        BusinessTurover = issuerFind.BusinessTurnover,
                        ROA = issuerFind.ROA,
                        ROE = issuerFind.ROE,
                        Image = issuerFind.Image
                    };
                    var businessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(issuerFind.BusinessCustomerId);
                    if (businessCustomerFind != null)
                    {
                        result.BondIssuer.Name = businessCustomerFind.Name;
                        result.BondIssuer.TradingAddress = businessCustomerFind.TradingAddress;
                        result.BondIssuer.Name = businessCustomerFind.Name;
                        result.BondIssuer.RepName = businessCustomerFind.RepName;
                        result.BondIssuer.Capital = businessCustomerFind.Capital;
                        result.BondIssuer.Email = businessCustomerFind.Email;
                        result.BondIssuer.Phone = businessCustomerFind.Phone;
                        result.BondIssuer.Website = businessCustomerFind.Website;
                        result.BondIssuer.Fanpage = businessCustomerFind.Fanpage;
                    };
                }

                //Lấy thông tin chính sách
                if (cpsSecondaryFind != null)
                {
                    result.Profit = cpsSecondaryFind.Profit;
                    result.TradingProviderName = cpsSecondaryFind.TradingProviderName;

                    var policyFileFind = _policyFileRepository.FindAllPolicyFile(cpsSecondaryFind.CpsSecondaryId, null, -1, 0, null);
                    if (policyFileFind != null)
                    {
                        result.PolicyFiles = _mapper.Map<List<AppPolicyFileDto>>(policyFileFind.Items);
                    }
                }
                
                //Lấy thông tin tài sản đảm bảo
                var guaranteeAssetFind = _guaranteeAssetRepository.FindAll(cpsSecondaryFind.CpsId, -1, 0, null);
                if (guaranteeAssetFind != null)
                {
                    foreach (var item in guaranteeAssetFind.Items)
                    {
                        result.BondGuaranteeAsset = _mapper.Map<AppGuaranteeAssetDto>(item);

                        var listFileRepo = _guaranteeAssetRepository.FindAllByIdFile(item.GuaranteeAssetId);
                        var listFile = _mapper.Map<List<AppGuaranteeFileDto>>(listFileRepo);
                        result.BondGuaranteeAsset.GuaranteeFiles = listFile;
                    }
                }
                
                //Lấy hồ sơ pháp lý
                var juridicalFiles = _juridicalFileRepository.FindAllJuridicalFile(cpsSecondaryFind.CpsId, -1, 0, null);
                if (juridicalFiles != null)
                {
                    result.JuridicalFiles = new();
                    foreach (var item in juridicalFiles.Items)
                    {
                        result.JuridicalFiles.Add(_mapper.Map<JuridicalFileDto>(item));
                    }
                }

                //Lấy file hợp đồng phân phối
                var distributionContractFile = _distributionContractRepository.AppContractFileFind(cpsSecondary.BondPrimaryId, cpsSecondary.TradingProviderId);
                if (distributionContractFile != null)
                {
                    result.DistributionContractFiles = new();
                    foreach (var item in distributionContractFile)
                    {
                        result.DistributionContractFiles.Add(_mapper.Map<DistributionContractFileDto>(item));
                    }
                }*/
            }
            return result;
        }

        public List<AppCpsPolicyDto> FindAllListPolicy(int cpsSecondaryId)
        {
            var cpsSecondary = _cpsSecondaryRepository.FindSecondaryById(cpsSecondaryId, null);
            if (cpsSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.CpsSecondaryNotFound).ToString()), "");
            }
            var cpsPolicies = _cpsSecondaryRepository.FindAllPolicy(cpsSecondaryId, cpsSecondary.TradingProviderId);

            var cpsInfo = _cpsInfoRepository.FindById(cpsSecondary.CpsId);
            if (cpsInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.CpsInfoNotFound).ToString()), "");
            }

            /*decimal soLuongCoPhanNamGiu = _distributionContractRepository.SumQuantity(cpsSecondary.TradingProviderId, cpsSecondary.BondPrimaryId);
            decimal soLuongConLai = soLuongCoPhanNamGiu - _cpsOrderRepository.SumQuantity(cpsSecondary.TradingProviderId, cpsSecondary.Id);

            //Khi số lượng cổ phần nắm giữ (hợp đồng phân phối) bé hơn số lượng cái phiếu đã bán trong sổ lệnh
            if (soLuongConLai < 0)
            {
                soLuongConLai = 0;
            }
            decimal soTienToiDa = soLuongCoPhanNamGiu * (cpsInfo.ParValue ?? 0) - _cpsOrderRepository.SumValue(cpsSecondary.TradingProviderId, cpsSecondary.Id);*/
            var result = new List<AppCpsPolicyDto>();
            foreach (var item in cpsPolicies)
            {
                //Lấy thông tin lợi tức lớn nhất của policy trong
                var cpsPolicyDetailFind = _cpsSecondaryRepository.FindPolicyDetailMaxProfit(cpsSecondaryId);
                if (cpsPolicyDetailFind != null)
                {
                    //item.InterestType = cpsPolicyDetailFind.InterestType;
                }
                //item.MaxMoney = soTienToiDa;
                result.Add(item);
            }
            return result;
        }
        #endregion
    }
}
