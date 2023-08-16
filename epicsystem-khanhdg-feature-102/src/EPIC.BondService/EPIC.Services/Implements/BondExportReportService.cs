using AutoMapper;
using ClosedXML.Excel;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.ExportExcel;
using EPIC.BondEntities.Dto.AppOrder;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.ExportReport;
using EPIC.Entities.Dto.Issuer;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Bond;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BondShared;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;
using EPIC.BondEntities.Dto.BondInfo;

namespace EPIC.BondDomain.Implements
{
    public class BondExportReportService : IBondExportReportService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly IMapper _mapper;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly BondInfoRepository _bondInfoRepository;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BondDepositProviderRepository _depositProviderRepository;
        private readonly BondOrderRepository _bondOrderRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly BankRepository _bankRepository;
        private readonly IBondSharedService _bondSharedService;
        private readonly SaleRepository _saleRepository;
        private readonly BondInterestPaymentRepository _interestPaymentRepository;
        private readonly IBondOrderService _bondOrderService;

        public BondExportReportService(
            ILogger<BondExportReportService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IOptions<FileConfig> fileConfig,
            IBondSharedService bondSharedService,
            IBondOrderService bondOrderService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _mapper = mapper;
            _interestPaymentRepository = new BondInterestPaymentRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _bondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _depositProviderRepository = new BondDepositProviderRepository(_connectionString, _logger);
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _distributionContractRepository = new BondDistributionContractRepository(_connectionString, _logger);
            _fileConfig = fileConfig;
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _bondSharedService = bondSharedService;
            _bondOrderService = bondOrderService;
        }

        public ExportResultDto ExportExcelBondPackages(DateTime? startDate, DateTime? endDate)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var result = new ExportResultDto();
            var bondPackages = new List<BondPackagesDto>();

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                var bondSecondaryList = _productBondSecondaryRepository.FindSecondaryByDate(tradingProviderId, startDate, endDate, null);

                foreach (var bondSecondary in bondSecondaryList)
                {
                    var bondInfo = _bondInfoRepository.FindBondInfoById(bondSecondary.BondId);
                    var bondPrimary = _productBondPrimaryRepository.FindById(bondSecondary.PrimaryId, null);
                    var issuer = _issuerRepository.FindById(bondInfo.IssuerId);
                    var depositProvider = _depositProviderRepository.FindById(bondInfo.DepositProviderId);
                    var distributionContract = _distributionContractRepository.FindContractByPrimaryId(bondSecondary.PrimaryId);
                    var distributionContractTotal = _distributionContractRepository.SumQuantity(tradingProviderId, bondSecondary.PrimaryId);
                    var distributionContractTotalValue = _distributionContractRepository.SumTotalValue(tradingProviderId, bondSecondary.PrimaryId);
                    var pNoteQuantity = _productBondSecondaryRepository.SumQuantity(bondSecondary.Id, 3);
                    var proQuantity = _productBondSecondaryRepository.SumQuantity(bondSecondary.Id, 1);
                    var proAQuantity = _productBondSecondaryRepository.SumQuantity(bondSecondary.Id, 2);
                    var proProAQuantity = proQuantity + proAQuantity;
                    var sumValueOrder = _bondOrderRepository.SumValue(tradingProviderId, bondSecondary.Id);
                    var sumQuantityOrder = _bondOrderRepository.SumQuantityByStatus(tradingProviderId, bondSecondary.Id, 8);
                    var sumValueByStatusOrder = _bondOrderRepository.SumValueByStatus(tradingProviderId, bondSecondary.Id, 8);
                    var remainTotal = _distributionContractRepository.SumQuantity(tradingProviderId, bondSecondary.PrimaryId) - _bondOrderRepository.SumQuantity(tradingProviderId, bondSecondary.Id);
                    var productBondInfo = _mapper.Map<ProductBondInfoDto>(bondInfo);
                    var remainTotalValue = bondInfo.ParValue * bondPrimary.Quantity - sumValueOrder;

                    if (issuer != null)
                    {
                        var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                        if (businessCustomerIssuer != null)
                        {

                            var bondPackage = new BondPackagesDto();
                            bondPackage.BondCode = bondInfo.BondCode;
                            bondPackage.Name = businessCustomerIssuer.Name;
                            bondPackage.IssueDate = bondInfo.IssueDate;
                            bondPackage.DueDate = bondInfo.DueDate;
                            bondPackage.DateBuy = distributionContract.DateBuy;
                            bondPackage.PrimaryPurchaseTotal = distributionContractTotal;
                            bondPackage.PrimaryPurchaseTotalValue = distributionContractTotalValue;
                            bondPackage.PNOTESaleTotal = pNoteQuantity;
                            bondPackage.PROSaleTotal = proProAQuantity;
                            bondPackage.SaleTotal = sumValueOrder;
                            bondPackage.CustomerPurchaseTotal = sumQuantityOrder;
                            bondPackage.CustomerPurchaseTotalValue = sumValueByStatusOrder;
                            bondPackage.RemainTotal = remainTotal;
                            bondPackage.RemainTotalValue = remainTotalValue;

                            bondPackages.Add(bondPackage);
                        };
                    };
                }
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                var bondInfoList = _productBondSecondaryRepository.FindSecondaryByPartner(partnerId, startDate, endDate);
                foreach (var bondInfoItem in bondInfoList)
                {
                    var bondPackage = new BondPackagesDto();
                    var bondSecondaryList = _productBondSecondaryRepository.FindSecondaryByDate(bondInfoItem.TradingProviderId, startDate, endDate, bondInfoItem.BondId);
                    decimal primaryPurchaseTotal = 0;
                    decimal primaryPurchaseTotalValue = 0;
                    decimal pNOTESaleTotal = 0;
                    decimal pROSaleTotal = 0;
                    decimal saleTotal = 0;
                    decimal customerPurchaseTotal = 0;
                    decimal customerPurchaseTotalValue = 0;
                    decimal bondRemainTotal = 0;
                    decimal? bondremainTotalValue = 0;

                    foreach (var bondSecondary in bondSecondaryList)
                    {
                        var bondInfo = _bondInfoRepository.FindBondInfoById(bondSecondary.BondId);
                        var bondPrimary = _productBondPrimaryRepository.FindById(bondSecondary.PrimaryId, null);
                        var issuer = _issuerRepository.FindById(bondInfo.IssuerId);
                        var depositProvider = _depositProviderRepository.FindById(bondInfo.DepositProviderId);
                        var distributionContract = _distributionContractRepository.FindContractByPrimaryId(bondSecondary.PrimaryId);
                        var distributionContractTotal = _distributionContractRepository.SumQuantity(bondInfoItem.TradingProviderId, bondSecondary.PrimaryId);
                        var distributionContractTotalValue = _distributionContractRepository.SumTotalValue(bondInfoItem.TradingProviderId, bondSecondary.PrimaryId);
                        var pNoteQuantity = _productBondSecondaryRepository.SumQuantity(bondSecondary.Id, 3);
                        var proQuantity = _productBondSecondaryRepository.SumQuantity(bondSecondary.Id, 1);
                        var proAQuantity = _productBondSecondaryRepository.SumQuantity(bondSecondary.Id, 2);
                        var proProAQuantity = proQuantity + proAQuantity;
                        var sumValueOrder = _bondOrderRepository.SumValue(bondInfoItem.TradingProviderId, bondSecondary.Id);
                        var sumQuantityOrder = _bondOrderRepository.SumQuantityByStatus(bondInfoItem.TradingProviderId, bondSecondary.Id, 8);
                        var sumValueByStatusOrder = _bondOrderRepository.SumValueByStatus(bondInfoItem.TradingProviderId, bondSecondary.Id, 8);
                        var remainTotal = _distributionContractRepository.SumQuantity(bondInfoItem.TradingProviderId, bondSecondary.PrimaryId) - _bondOrderRepository.SumQuantity(bondInfoItem.TradingProviderId, bondSecondary.Id);
                        var productBondInfo = _mapper.Map<ProductBondInfoDto>(bondInfo);
                        var remainTotalValue = bondInfo.ParValue * bondPrimary.Quantity - sumValueOrder;
                        if (issuer != null)
                        {
                            var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                            if (businessCustomerIssuer != null)
                            {
                                bondPackage.Name = businessCustomerIssuer.Name;
                            };
                        };

                        bondPackage.BondCode = bondInfo.BondCode;
                        bondPackage.IssueDate = bondInfo.IssueDate;
                        bondPackage.DueDate = bondInfo.DueDate;

                        if (distributionContract != null)
                        {
                            bondPackage.DateBuy = distributionContract.DateBuy;
                        }

                        primaryPurchaseTotal += distributionContractTotal;
                        primaryPurchaseTotalValue += distributionContractTotalValue;
                        pNOTESaleTotal += pNoteQuantity;
                        pROSaleTotal += proProAQuantity;
                        saleTotal += sumValueOrder;
                        customerPurchaseTotal += sumQuantityOrder;
                        customerPurchaseTotalValue += sumValueByStatusOrder;
                        bondRemainTotal += remainTotal;
                        bondremainTotalValue += remainTotalValue;
                    };

                    bondPackage.PrimaryPurchaseTotal = primaryPurchaseTotal;
                    bondPackage.PrimaryPurchaseTotalValue = primaryPurchaseTotalValue;
                    bondPackage.PNOTESaleTotal = pNOTESaleTotal;
                    bondPackage.PROSaleTotal = pROSaleTotal;
                    bondPackage.SaleTotal = saleTotal;
                    bondPackage.CustomerPurchaseTotal = customerPurchaseTotal;
                    bondPackage.CustomerPurchaseTotalValue = customerPurchaseTotalValue;
                    bondPackage.RemainTotal = bondRemainTotal;
                    bondPackage.RemainTotalValue = bondremainTotalValue;

                    bondPackages.Add(bondPackage);
                }
            }
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Bonds");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã TP";
            worksheet.Cell(currentRow, 3).Value = "Tổ chức phát hành";
            worksheet.Cell(currentRow, 4).Value = "Ngày phát hành";
            worksheet.Cell(currentRow, 5).Value = "Ngày đáo hạn";
            worksheet.Cell(currentRow, 6).Value = "Ngày mua sơ cấp";
            worksheet.Cell(currentRow, 7).Value = "Tổng SL TP mua sơ cấp";
            worksheet.Cell(currentRow, 8).Value = "Tổng ST mua sơ cấp";
            worksheet.Cell(currentRow, 9).Value = "Tổng SL bán TP PNOTE";
            worksheet.Cell(currentRow, 10).Value = "Tổng SL bán TP PRO";
            worksheet.Cell(currentRow, 11).Value = "Tổng số tiền đã bán";
            worksheet.Cell(currentRow, 12).Value = "Tổng SL TP đã mua lại KH";
            worksheet.Cell(currentRow, 13).Value = "Tổng số tiền mua lại KH";
            worksheet.Cell(currentRow, 14).Value = "Tổng SL TP còn lại";
            worksheet.Cell(currentRow, 15).Value = "Tổng giá trị TP còn lại";
            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 20;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 20;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 20;
            worksheet.Column("J").Width = 20;
            worksheet.Column("K").Width = 20;
            worksheet.Column("L").Width = 25;
            worksheet.Column("M").Width = 20;
            worksheet.Column("N").Width = 20;
            worksheet.Column("O").Width = 20;
            worksheet.Column("P").Width = 20;
            foreach (var item in bondPackages)
            {
                currentRow++;
                ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item.BondCode;
                worksheet.Cell(currentRow, 3).Value = item.Name;
                worksheet.Cell(currentRow, 4).Value = item.IssueDate;
                worksheet.Cell(currentRow, 5).Value = item.DueDate;
                worksheet.Cell(currentRow, 6).Value = item.DateBuy;
                worksheet.Cell(currentRow, 7).Value = item.PrimaryPurchaseTotal;
                worksheet.Cell(currentRow, 8).Value = item.PrimaryPurchaseTotalValue;
                worksheet.Cell(currentRow, 9).Value = item.PNOTESaleTotal;
                worksheet.Cell(currentRow, 10).Value = item.PROSaleTotal;
                worksheet.Cell(currentRow, 11).Value = item.SaleTotal;
                worksheet.Cell(currentRow, 12).Value = item.CustomerPurchaseTotal;
                worksheet.Cell(currentRow, 13).Value = item.CustomerPurchaseTotalValue;
                worksheet.Cell(currentRow, 14).Value = item.RemainTotal;
                worksheet.Cell(currentRow, 15).Value = item.RemainTotalValue;
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        public ExportResultDto ExportBondInvestment(DateTime? startDate, DateTime? endDate)
        {
            var tradingProviderId = new int();
            var bondInvestmentListFind = new List<BondInvestmentDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                bondInvestmentListFind = _bondOrderRepository.ExportBondInvestment(tradingProviderId, startDate, endDate);
            }

            var result = new ExportResultDto();
            var bondInvestmentList = new List<BondInvestmentDto>();

            foreach (var item in bondInvestmentListFind)
            {
                var bondInvestment = new BondInvestmentDto();
                var cifCodeFind = _cifCodeRepository.GetByCifCode(item.CifCode);

                var statusDict = new Dictionary<int?, string>(){
                    {1, "Khởi tạo"},
                    {2, "Chờ thanh toán"},
                    {3, "Chờ ký hợp đồng"},
                    {4, "Chờ duyệt hợp đồng"},
                    {5, "Đang đầu tư"},
                    {6, "Phong toả"},
                    {7, "Giải toả"},
                    {8, "Tất toán"}
                };

                var classifyDict = new Dictionary<int?, string>(){
                    {1, "PRO"},
                    {2, "PROA"},
                    {3, "PNOTE"}
                };

                var tranTypeDict = new Dictionary<int?, string>(){
                    {1, "Thu"},
                    {2, "Chi"},
                };

                var donGiaHD = _bondSharedService.CalculateQuantityAndUnitPrice(item.TotalValue, item.BondSecondaryId, item.PaymentFullDate ?? DateTime.Now, item.TradingProviderId);
                if (cifCodeFind.InvestorId != null && cifCodeFind.BusinessCustomerId == null)
                {
                    var saleInfo = _saleRepository.GetSaleInfoByRefferalCode(tradingProviderId, item.SaleReferralCode);

                    //check souce nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    if (item.Source == 1)
                    {
                        bondInvestment.TradingType = "Online";
                    }
                    else if (item.Source == 2)
                    {
                        bondInvestment.TradingType = "Offline";
                    }
                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    if (item.PeriodType == PeriodType.NAM)
                    {
                        bondInvestment.KyHan = item.PeriodQuantity + " Năm";
                    }
                    else if (item.PeriodType == PeriodType.THANG)
                    {
                        bondInvestment.KyHan = item.PeriodQuantity + " Tháng";
                    }
                    else if (item.PeriodType == PeriodType.NGAY)
                    {
                        bondInvestment.KyHan = item.PeriodQuantity + " Ngày";
                    }
                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    if (item.PeriodType == PeriodType.NAM)
                    {
                        bondInvestment.PeriodTime = item.PeriodQuantity + " Năm";
                    }
                    else if (item.PeriodType == PeriodType.THANG)
                    {
                        bondInvestment.PeriodTime = item.PeriodQuantity + " Tháng";
                    }
                    else if (item.PeriodType == PeriodType.NGAY)
                    {
                        bondInvestment.PeriodTime = item.PeriodQuantity + " Ngày";
                    }

                    var sexDict = new Dictionary<string, string>()
                    {
                        {"F", "Nữ"},
                        {"M", "Nam"},
                    };
                    var investorBank = new InvestorBankAccount();
                    var bankInfo = new CoreBank();
                    var investorIdentificationFind = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);
                    if (investorIdentificationFind != null)
                    {
                        bondInvestment.CustomerName = investorIdentificationFind.Fullname;
                        if (investorIdentificationFind.Sex != null)
                        {
                            bondInvestment.Sex = sexDict[investorIdentificationFind.Sex];
                        }
                        bondInvestment.IdType = investorIdentificationFind.IdType;
                        bondInvestment.IdNo = investorIdentificationFind.IdNo;
                    }

                    var investorBankFind = _investorBankAccountRepository.GetByInvestorId(cifCodeFind.InvestorId ?? 0);
                    if (investorBankFind != null)
                    {
                        investorBank = investorBankFind;
                        bankInfo = _bankRepository.GetById(investorBank.BankId);
                    }
                    bondInvestment.CustomerType = UserTypes.INVESTOR;
                    bondInvestment.TranDate = item.TranDate;
                    bondInvestment.CifCode = item.CifCode;

                    bondInvestment.ContractCode = item.ContractCode;
                    bondInvestment.BankAccNo = investorBank.BankAccount;
                    bondInvestment.BondValue = item.BondValue;
                    bondInvestment.CustomerBankName = bankInfo.BankName;
                    bondInvestment.SaleReferralCode = item.SaleReferralCode;
                    if (saleInfo != null)
                    {
                        bondInvestment.SaleName = saleInfo.Fullname;
                        bondInvestment.DepartmentName = saleInfo.DepartmentName;
                    }

                    bondInvestment.Area = item.Area;
                    bondInvestment.Quantity = item.Quantity;
                    bondInvestment.ClassifyDisplay = classifyDict[item.Classify];
                    bondInvestment.InvName = item.InvName;
                    if (item.TranType == 1 || item.TranType == 2)
                    {
                        bondInvestment.TranTypeDisplay = tranTypeDict[item.TranType];
                    }
                    bondInvestment.BondCode = item.BondCode;
                    bondInvestment.PaymentFullDate = item.PaymentFullDate;
                    bondInvestment.DueDate = item.DueDate;
                    bondInvestment.Profit = item.Profit;
                    bondInvestment.UnitPrice = donGiaHD.UnitPrice;
                    bondInvestment.StatusDisplay = statusDict[item.StatusOrder];
                    bondInvestment.PaymentAmount = item.PaymentAmount;
                    bondInvestment.TotalValue = item.TotalValue;
                }
                else if (cifCodeFind.InvestorId == null && cifCodeFind.BusinessCustomerId != null)
                {
                    var saleInfo = _saleRepository.GetSaleInfoByRefferalCodeBusiness(tradingProviderId, item.SaleReferralCode);
                    //check souce nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    if (item.Source == 1)
                    {
                        bondInvestment.TradingType = "Online";
                    }
                    else if (item.Source == 2)
                    {
                        bondInvestment.TradingType = "Offline";
                    }

                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    if (item.PeriodType == PeriodType.NAM)
                    {
                        bondInvestment.KyHan = item.PeriodQuantity + " Năm";
                    }
                    else if (item.PeriodType == PeriodType.THANG)
                    {
                        bondInvestment.KyHan = item.PeriodQuantity + " Tháng";
                    }
                    else if (item.PeriodType == PeriodType.NGAY)
                    {
                        bondInvestment.KyHan = item.PeriodQuantity + " Ngày";
                    }

                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    var businessCustomerBank = new BusinessCustomerBank();
                    var bankInfo = new CoreBank();
                    var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(cifCodeFind.BusinessCustomerId ?? 0);
                    if (businessCustomerBankFind != null)
                    {
                        businessCustomerBank = businessCustomerBankFind;
                        bankInfo = _bankRepository.GetById(businessCustomerBank.BankId);
                    }

                    //check souce nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    if (item.Source == 1)
                    {
                        bondInvestment.TradingType = "Online";
                    }
                    else if (item.Source == 2)
                    {
                        bondInvestment.TradingType = "Offline";
                    }

                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    if (item.PeriodType == PeriodType.NAM)
                    {
                        bondInvestment.PeriodTime = item.PeriodQuantity + " Năm";
                    }
                    else if (item.PeriodType == PeriodType.THANG)
                    {
                        bondInvestment.PeriodTime = item.PeriodQuantity + " Tháng";
                    }
                    else if (item.PeriodType == PeriodType.NGAY)
                    {
                        bondInvestment.PeriodTime = item.PeriodQuantity + " Ngày";
                    }

                    bondInvestment.TranDate = item.TranDate;
                    bondInvestment.CifCode = item.CifCode;
                    bondInvestment.CustomerName = businessCustomer.Name;
                    bondInvestment.ContractCode = item.ContractCode;
                    bondInvestment.BondValue = item.BondValue;
                    bondInvestment.ClassifyDisplay = classifyDict[item.Classify];
                    bondInvestment.BankAccNo = businessCustomerBank.BankAccNo;
                    bondInvestment.CustomerBankName = bankInfo.BankName;
                    bondInvestment.SaleReferralCode = item.SaleReferralCode;
                    bondInvestment.Quantity = item.Quantity;
                    if (saleInfo != null)
                    {
                        bondInvestment.SaleName = saleInfo.Fullname;
                        bondInvestment.DepartmentName = saleInfo.DepartmentName;
                    }
                    bondInvestment.Area = item.Area;
                    if (item.TranType == 1 || item.TranType == 2)
                    {
                        bondInvestment.TranTypeDisplay = tranTypeDict[item.TranType];
                    }

                    bondInvestment.BondCode = item.BondCode;
                    bondInvestment.StatusDisplay = statusDict[item.StatusOrder];
                    bondInvestment.BondValue = item.BondValue;
                    bondInvestment.UnitPrice = donGiaHD.UnitPrice;
                    bondInvestment.PaymentFullDate = item.PaymentFullDate;
                    bondInvestment.PaymentAmount = item.PaymentAmount;
                    bondInvestment.DueDate = item.DueDate;
                    bondInvestment.Profit = item.Profit;
                    bondInvestment.TotalValue = item.TotalValue;
                }
                bondInvestmentList.Add(bondInvestment);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Bonds");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 7).Value = "Giới tính";
            worksheet.Cell(currentRow, 8).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 9).Value = "Tài khoản KH nhận tiền";
            worksheet.Cell(currentRow, 10).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 11).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 12).Value = "Phòng ban";
            worksheet.Cell(currentRow, 13).Value = "Khu vực";
            worksheet.Cell(currentRow, 14).Value = "Đầu tư";
            worksheet.Cell(currentRow, 15).Value = "Mã trái phiếu";
            worksheet.Cell(currentRow, 16).Value = "Loại sản phẩm";
            worksheet.Cell(currentRow, 17).Value = "Trạng thái hợp đồng";
            worksheet.Cell(currentRow, 18).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 19).Value = "Ngày đáo hạn";
            worksheet.Cell(currentRow, 20).Value = "Kỳ hạn";
            worksheet.Cell(currentRow, 21).Value = "Lợi tức";
            worksheet.Cell(currentRow, 22).Value = "Loại giao dịch";
            worksheet.Cell(currentRow, 23).Value = "Số lượng trái phiếu";
            worksheet.Cell(currentRow, 24).Value = "Đơn giá trên HĐ";
            worksheet.Cell(currentRow, 25).Value = "Giá trị đầu tư trên hợp đồng";
            worksheet.Cell(currentRow, 26).Value = "Giá trị trái phiếu theo mệnh giá";
            worksheet.Cell(currentRow, 27).Value = "Số tiền giao dịch";
            worksheet.Cell(currentRow, 28).Value = "Giá trị đầu tư còn lại";
            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 20;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 15;
            worksheet.Column("F").Width = 15;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 25;
            worksheet.Column("L").Width = 15;
            worksheet.Column("M").Width = 15;
            worksheet.Column("N").Width = 15;
            worksheet.Column("O").Width = 30;
            worksheet.Column("P").Width = 15;
            worksheet.Column("Q").Width = 20;
            worksheet.Column("R").Width = 20;
            worksheet.Column("S").Width = 20;
            worksheet.Column("T").Width = 20;
            worksheet.Column("U").Width = 20;
            worksheet.Column("V").Width = 15;
            worksheet.Column("W").Width = 20;
            worksheet.Column("X").Width = 20;
            worksheet.Column("Y").Width = 20;
            worksheet.Column("Z").Width = 20;
            worksheet.Column("AA").Width = 20;
            worksheet.Column("AB").Width = 20;
            worksheet.Column("AC").Width = 20;


            foreach (var item in bondInvestmentList)
            {
                currentRow++;
                ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.TranDate;
                worksheet.Cell(currentRow, 3).Value = "'" + item.CifCode;
                worksheet.Cell(currentRow, 4).Value = item.CustomerName;
                worksheet.Cell(currentRow, 5).Value = "'" + item.IdNo;
                worksheet.Cell(currentRow, 6).Value = item.IdType;
                worksheet.Cell(currentRow, 7).Value = item.Sex;
                worksheet.Cell(currentRow, 8).Value = "'" + item.ContractCode;
                worksheet.Cell(currentRow, 9).Value = "'" + item.BankAccNo;
                worksheet.Cell(currentRow, 10).Value = "'" + item.SaleReferralCode;
                worksheet.Cell(currentRow, 11).Value = item.SaleName;
                worksheet.Cell(currentRow, 12).Value = item.DepartmentName;
                worksheet.Cell(currentRow, 13).Value = item.Area;
                worksheet.Cell(currentRow, 14).Value = item.TradingType;
                worksheet.Cell(currentRow, 15).Value = "'" + item.BondCode;
                worksheet.Cell(currentRow, 16).Value = item.ClassifyDisplay;
                worksheet.Cell(currentRow, 17).Value = item.StatusDisplay;
                worksheet.Cell(currentRow, 18).Value = item.PaymentFullDate;
                worksheet.Cell(currentRow, 19).Value = item.DueDate;
                worksheet.Cell(currentRow, 20).Value = item.KyHan;
                worksheet.Cell(currentRow, 21).Value = item.Profit + "%";
                worksheet.Cell(currentRow, 22).Value = item.TranTypeDisplay;
                worksheet.Cell(currentRow, 23).Value = item.Quantity;
                worksheet.Cell(currentRow, 24).Value = item.UnitPrice;
                worksheet.Cell(currentRow, 25).Value = item.TotalValue;
                worksheet.Cell(currentRow, 26).Value = item.BondValue;
                worksheet.Cell(currentRow, 27).Value = item.PaymentAmount;
                worksheet.Cell(currentRow, 28).Value = 0;
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }


        public List<ThoiGianChiTraThucDto> LayDanhSachNgayChiTra()
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var listOrder = _bondOrderRepository.FindOrderByTradingProviderId(tradingProviderId, null, null, null);
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
                var ngayDaoHan = _bondOrderRepository.CalculateDueDate(policyDetailFind, ngayDauKy, false);

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

                        var ngayLamViec = _bondOrderRepository.NextWorkDay(ngayCuoiKy.Date, policyDetailFind.TradingProviderId, false);

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
                var listOrderDaChiTra = _interestPaymentRepository.GetListInterestPaymentByOrderId((int)orderItem.Id, orderItem.TradingProviderId).Select(x => x.PeriodIndex).ToList();

                //lọc và loại các kì hạn đã chi trả lấy những kì hạn chưa chi trả
                listThoiGianChiTra = listThoiGianChiTra.Where(e => !listOrderDaChiTra.Contains(e.PeriodIndex)).ToList();
                result.AddRange(listThoiGianChiTra);
            }
            _bondOrderRepository.CloseConnection();
            return result;
        }

        public List<DanhSachChiTraDto> LapDanhSachChiTra()
        {
            var lapDanhSachChiTra = new List<DanhSachChiTraDto>();
            var result = new List<DanhSachChiTraDto>();
            var danhSachNgayChiTra = LayDanhSachNgayChiTra();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            
            foreach (var item in danhSachNgayChiTra)
            {
                string messagerError = "";
                var orderFind = _bondOrderRepository.FindById(item.OrderId, tradingProviderId);
                var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(item.PolicyDetailId);

                var policyFind = _productBondSecondaryRepository.FindPolicyById(policyDetailFind.PolicyId, policyDetailFind.TradingProviderId);

                //Tính tổng số tiền đã được chi trả
                var soTienDaChiTra = _bondOrderRepository.InterestPaymentSumMoney(item.PayDate, item.OrderId, orderFind.TradingProviderId);

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
                    cifCodeFind = new CifCodes();
                }

                if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    thueLoiNhuan = (policyFind.IncomeTax) / 100;
                    var investerIdentification = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);
                    if(investerIdentification != null)
                    {
                        name = investerIdentification.Fullname;
                    }
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
                    PreviousPeriodPayDate = item.LastPayDate, 
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
            result = _mapper.Map<List<DanhSachChiTraDto>>(lapDanhSachChiTra);
            return result;
        }

        public ExportResultDto ExportInterestPrincipalDue(DateTime? startDate, DateTime? endDate)
        {
            var tradingProviderId = new int();
            var listInterestPrincipalDueFind = new List<InterestPrincipalDue>(); //list để lưu dữ liêu từ data đổ về
            var listChiTra = LapDanhSachChiTra();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                listInterestPrincipalDueFind = _bondOrderRepository.ExportInterestPrincipalDue(tradingProviderId, startDate, endDate);
            }

            var result = new ExportResultDto();
            var listInterestPrincipalDue = new List<InterestPrincipalDueDto>(); // list để add data thêm vào

            foreach (var item in listInterestPrincipalDueFind)
            {
                var interestPrincipalDue = new InterestPrincipalDueDto();
                var ngayChiTraFind = listChiTra.FirstOrDefault(ct => ct.OrderId == item.OrderId);
                var classifyDict = new Dictionary<int?, string>(){
                    {1, ClassifyType.PRO},
                    {2, ClassifyType.PROA},
                    {3, ClassifyType.PNOTE}
                };

                if (item.InvestorId != null && item.BusinessCustomerId == null)
                {
                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    if (item.PeriodType == PeriodType.NAM)
                    {
                        interestPrincipalDue.KyHan = item.PeriodQuantity + " Năm";
                    }
                    else if (item.PeriodType == PeriodType.THANG)
                    {
                        interestPrincipalDue.KyHan = item.PeriodQuantity + " Tháng";
                    }
                    else if (item.PeriodType == PeriodType.NGAY)
                    {
                        interestPrincipalDue.KyHan = item.PeriodQuantity + " Ngày";
                    }
                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    if (item.PeriodType == PeriodType.NAM)
                    {
                        interestPrincipalDue.PeriodTime = item.PeriodQuantity + " Năm";
                    }
                    else if (item.PeriodType == PeriodType.THANG)
                    {
                        interestPrincipalDue.PeriodTime = item.PeriodQuantity + " Tháng";
                    }
                    else if (item.PeriodType == PeriodType.NGAY)
                    {
                        interestPrincipalDue.PeriodTime = item.PeriodQuantity + " Ngày";
                    }

                    var investorBank = new InvestorBankAccount();
                    var bankInfo = new CoreBank();
                    
                    var investorIdentificationFind = _managerInvestorRepository.GetDefaultIdentification(item.InvestorId ?? 0, false);
                    
                    if (investorIdentificationFind != null)
                    {
                        interestPrincipalDue.CustomerName = investorIdentificationFind.Fullname;
                        interestPrincipalDue.IdNo = investorIdentificationFind.IdNo;
                        interestPrincipalDue.IdType = investorIdentificationFind.IdType;
                    }

                    var investorBankFind = _investorBankAccountRepository.GetByInvestorId(item.InvestorId ?? 0);
                    if (investorBankFind != null)
                    {
                        investorBank = investorBankFind;
                        bankInfo = _bankRepository.GetById(investorBank.BankId);
                    }
                    interestPrincipalDue.CustomerName = investorIdentificationFind.Fullname;
                    interestPrincipalDue.BankAccNo = investorBank.BankAccount;
                    interestPrincipalDue.OwnerAccount = investorBank.OwnerAccount;
                    interestPrincipalDue.CustomerBankName = bankInfo.BankName;
                }
                else if (item.InvestorId == null && item.BusinessCustomerId != null)
                {
                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    if (item.PeriodType == PeriodType.NAM)
                    {
                        interestPrincipalDue.KyHan = item.PeriodQuantity + " Năm";
                    }
                    else if (item.PeriodType == PeriodType.THANG)
                    {
                        interestPrincipalDue.KyHan = item.PeriodQuantity + " Tháng";
                    }
                    else if (item.PeriodType == PeriodType.NGAY)
                    {
                        interestPrincipalDue.KyHan = item.PeriodQuantity + " Ngày";
                    }

                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(item.BusinessCustomerId ?? 0);
                    var businessCustomerBank = new BusinessCustomerBank();
                    var bankInfo = new CoreBank();
                    var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(item.BusinessCustomerId ?? 0);
                    if (businessCustomerBankFind != null)
                    {
                        businessCustomerBank = businessCustomerBankFind;
                        bankInfo = _bankRepository.GetById(businessCustomerBank.BankId);
                    }

                    interestPrincipalDue.CustomerName = businessCustomer.Name;
                    interestPrincipalDue.BankAccNo = businessCustomerBank.BankAccNo;
                    interestPrincipalDue.OwnerAccount = businessCustomerBank.BankAccName;
                    interestPrincipalDue.CustomerBankName = bankInfo.BankName;

                }

                interestPrincipalDue.BondCode = item.BondCode;
                interestPrincipalDue.ClassifyDisplay = classifyDict[item.Classify];
                interestPrincipalDue.ContractCode = item.ContractCode;
                interestPrincipalDue.DueDate = item.DueDate;
                interestPrincipalDue.CifCode = item.CifCode;
                interestPrincipalDue.Profit = item.Profit;
                interestPrincipalDue.IncomeTax = ngayChiTraFind.Tax;
                interestPrincipalDue.InterestPeriod = item.InterestPeriod;
                interestPrincipalDue.TotalValue = item.TotalValue;
                interestPrincipalDue.Quantity = item.Quantity;
                interestPrincipalDue.PreviousPeriodPaydate = ngayChiTraFind.PreviousPeriodPayDate;
                interestPrincipalDue.ForecastMoney = ngayChiTraFind.AmountMoney;
                listInterestPrincipalDue.Add(interestPrincipalDue);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("TinhGocLaiDenHan");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày trả lãi kỳ trước";
            worksheet.Cell(currentRow, 3).Value = "Ngày đến hạn";
            worksheet.Cell(currentRow, 4).Value = "Mã HĐ";
            worksheet.Cell(currentRow, 5).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 6).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 7).Value = "Mã trái phiếu";
            worksheet.Cell(currentRow, 8).Value = "Loại trái phiếu";
            worksheet.Cell(currentRow, 9).Value = "Số lượng trái phiếu";
            worksheet.Cell(currentRow, 10).Value = "Lãi suất";
            worksheet.Cell(currentRow, 11).Value = "Kỳ hạn";
            worksheet.Cell(currentRow, 12).Value = "Giá trị hợp đồng";
            worksheet.Cell(currentRow, 13).Value = "Thuế TNCN";
            worksheet.Cell(currentRow, 14).Value = "Số tiền dự chi";
            worksheet.Cell(currentRow, 15).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 16).Value = "Số tài khoản";
            worksheet.Cell(currentRow, 17).Value = "Ngân hàng";
            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 20;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 30;
            worksheet.Column("F").Width = 30;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 25;
            worksheet.Column("L").Width = 15;
            worksheet.Column("M").Width = 15;
            worksheet.Column("N").Width = 15;
            worksheet.Column("O").Width = 30;
            worksheet.Column("P").Width = 15;
            worksheet.Column("Q").Width = 20;
            worksheet.Column("R").Width = 20;
            worksheet.Column("S").Width = 20;

            foreach (var item in listInterestPrincipalDue)
            {
                currentRow++;
                ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.PreviousPeriodPaydate;
                worksheet.Cell(currentRow, 3).Value = item.DueDate;
                worksheet.Cell(currentRow, 4).Value = item.ContractCode;
                worksheet.Cell(currentRow, 5).Value = item.CustomerName;
                worksheet.Cell(currentRow, 6).Value = item.CifCode;
                worksheet.Cell(currentRow, 7).Value = item.BondCode;
                worksheet.Cell(currentRow, 8).Value = item.ClassifyDisplay;
                worksheet.Cell(currentRow, 9).Value = item.Quantity;
                worksheet.Cell(currentRow, 10).Value = item.Profit + "%";
                worksheet.Cell(currentRow, 11).Value = item.KyHan;
                worksheet.Cell(currentRow, 12).Value = item.TotalValue;
                worksheet.Cell(currentRow, 13).Value = item.IncomeTax;
                worksheet.Cell(currentRow, 14).Value = item.ForecastMoney;
                worksheet.Cell(currentRow, 15).Value = item.OwnerAccount;
                worksheet.Cell(currentRow, 16).Value = item.BankAccNo;
                worksheet.Cell(currentRow, 17).Value = item.CustomerBankName;
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();

            return result;
        }
    }
}
