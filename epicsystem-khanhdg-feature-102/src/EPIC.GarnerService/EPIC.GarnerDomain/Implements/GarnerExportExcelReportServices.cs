using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerExportExcel;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerExportExcelReportServices : IGarnerExportExcelReportServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerExportExcelReportServices> _logger;
        private readonly ILogger<GarnerFormulaServices> _loggerFomula;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerExportExcelReportRepositories _garnerExportExcelReportRepositories;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;

        public GarnerExportExcelReportServices(
            DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<GarnerExportExcelReportServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerExportExcelReportRepositories = new GarnerExportExcelReportRepositories(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
        }

        public async Task<ExportResultDto> ExcelGarnerTotalInvestmentAsync(DateTime? startDate, DateTime? endDate)
        {
            var listTotalInvestment = new List<GarnerListActualPaymentDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            //lấy tradingProviderId nếu user type là đại lý
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var result = new ExportResultDto();
            var totalAdministrationFind = _garnerExportExcelReportRepositories.ListInvestments(tradingProviderId, partnerId, startDate, endDate);

            List<Task> tasks = new();
            ConcurrentBag<GarnerListActualPaymentDto> listTotalInvestmentCurrentbag = new();
            foreach (var item in totalAdministrationFind)
            {
                var task = Task.Run(() =>
                {
                    var dbContext = CommonUtils.GetService<EpicSchemaDbContextTransient>(_httpContext);
                    var investorEFRepository = new InvestorEFRepository(dbContext, _logger);
                    var investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, _logger);
                    var businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, _logger);
                    var businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, _logger);
                    var saleEFRepository = new SaleEFRepository(dbContext, _logger);
                    var garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, _logger);
                    var bankEFRepository = new BankEFRepository(dbContext, _logger);
                    var garnerExportExcelReportRepositories = new GarnerExportExcelReportRepositories(dbContext, _logger);
                    var policyEFRepository = new GarnerPolicyEFRepository(dbContext, _logger);

                    var totalInvestmentData = new GarnerListActualPaymentDto();
                    totalInvestmentData.CurrentInvestment = garnerExportExcelReportRepositories.SumValueCurrentInvestment((int)item?.OrderId, tradingProviderId, item.TranDate);
                    totalInvestmentData.Cifcode = item.CifCode;
                    totalInvestmentData.TranDate = item.TranDate;
                    var policyFind = policyEFRepository.FindById(item.PolicyId ?? 0, item.TradingProviderId);

                    if (item.BusinessCustomerId == null && item.InvestorId != null)
                    {
                        var investorIden = investorEFRepository.GetDefaultIdentification(item?.InvestorId ?? 0);
                        var investorBankAccount = investorBankAccountEFRepository.FindById(item?.InvestorBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(investorBankAccount?.BankId ?? 0);

                        totalInvestmentData.CustomerName = investorIden?.Fullname;
                        totalInvestmentData.IdNo = investorIden?.IdNo;
                        totalInvestmentData.IdType = investorIden?.IdType;
                        totalInvestmentData.PermanentAddress = investorIden?.PlaceOfResidence;
                        totalInvestmentData.Sex = ExcelDataUtils.GenderDisplay(investorIden?.Sex);
                        totalInvestmentData.BankAccount = investorBankAccount?.BankAccount;
                        totalInvestmentData.BankName = bank?.BankName;
                        totalInvestmentData.OwnerBankAccount = investorBankAccount?.OwnerAccount;
                    }
                    else if (item.BusinessCustomerId != null && item.InvestorId == null)
                    {
                        var businessCustomer = businessCustomerEFRepository.FindById(item.BusinessCustomerId ?? 0);
                        var businessBankAccount = businessCustomerBankEFRepository.FindById(item?.BusinessCustomerBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(businessBankAccount?.BankId ?? 0);
                        totalInvestmentData.CustomerName = businessCustomer?.Name;
                        totalInvestmentData.IdNo = businessCustomer?.TaxCode;
                        totalInvestmentData.IdType = TaxCode.MA_SO_THUE;
                        totalInvestmentData.BankAccount = businessBankAccount?.BankAccNo;
                        totalInvestmentData.BankName = bank?.BankName;
                        totalInvestmentData.OwnerBankAccount = businessBankAccount?.BankAccName;
                    }

                    var saleInvestorInfo = saleEFRepository.FindSaleByRefferCode(item.SaleReferralCode, true, item.TradingProviderId);
                    var saleBusinessInfo = saleEFRepository.FindSaleByRefferCode(item.SaleReferralCode, false, item.TradingProviderId);

                    if (saleInvestorInfo != null && saleBusinessInfo == null)
                    {
                        totalInvestmentData.SalerName = saleInvestorInfo?.Fullname;
                        totalInvestmentData.SaleDepartmentName = saleInvestorInfo?.DepartmentName;
                    }
                    else if (saleInvestorInfo == null && saleBusinessInfo != null)
                    {
                        totalInvestmentData.SalerName = saleBusinessInfo?.Name;
                        totalInvestmentData.SaleDepartmentName = saleInvestorInfo?.DepartmentName;
                    }

                    totalInvestmentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);
                    totalInvestmentData.Source = ExcelDataUtils.TradingTypeDisplay(item.Source);
                    totalInvestmentData.ProductCode = item.ProductCode;
                    totalInvestmentData.ContractCode = item.ContractCode;
                    totalInvestmentData.ReferralCode = item.SaleReferralCode;
                    totalInvestmentData.PolicyCode = policyFind.Code;
                    totalInvestmentData.IsBlockage = item.Status == OrderStatus.PHONG_TOA ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;
                    totalInvestmentData.InvestDate = item.InvestDate?.Date;
                    if (item?.InvestDate != null)
                    {
                        totalInvestmentData.DueDate = garnerPolicyDetailEFRepository.CalculateDueDate(item?.InvestDate ?? new DateTime(), item.PeriodQuantity ?? 0, item.PeriodType);
                    }
                    totalInvestmentData.StatusDisplay = ExcelDataUtils.StatusOrder(item?.Status);
                    totalInvestmentData.CalculateTypeDisplay = ExcelDataUtils.CaculateTypeDisplay(policyFind.CalculateType);
                    totalInvestmentData.InitTotalValue = item.InitTotalValue;
                    totalInvestmentData.TotalValue = item.TotalValue;
                    totalInvestmentData.PaymentAmount = item.PaymentAmount;

                    listTotalInvestmentCurrentbag.Add(totalInvestmentData);
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            listTotalInvestment.AddRange(listTotalInvestmentCurrentbag);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("GARNER");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 5).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Địa chỉ thường trú";
            worksheet.Cell(currentRow, 7).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 8).Value = "Giới tính";
            worksheet.Cell(currentRow, 9).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 10).Value = "Tài khoản nhận tiền KH";
            worksheet.Cell(currentRow, 11).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 12).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 13).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 14).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 15).Value = "Phòng ban";
            worksheet.Cell(currentRow, 16).Value = "Kiểu giao dịch";
            worksheet.Cell(currentRow, 17).Value = "Sản phẩm";
            worksheet.Cell(currentRow, 18).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 19).Value = "Phong tỏa";
            worksheet.Cell(currentRow, 20).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 21).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 22).Value = "Thời hạn";
            worksheet.Cell(currentRow, 23).Value = "Tình trạng";
            worksheet.Cell(currentRow, 24).Value = "Gross/Net";
            worksheet.Cell(currentRow, 25).Value = "Giá trị đầu tư theo hợp đồng";
            worksheet.Cell(currentRow, 26).Value = "Số tiền khách hàng chuyển";
            worksheet.Cell(currentRow, 27).Value = "Giá trị đầu tư hiện tại";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;
            worksheet.Column("S").Width = 40;
            worksheet.Column("T").Width = 40;
            worksheet.Column("U").Width = 40;
            worksheet.Column("V").Width = 40;
            worksheet.Column("W").Width = 40;
            worksheet.Column("X").Width = 40;
            worksheet.Column("Y").Width = 40;
            worksheet.Column("Z").Width = 40;
            worksheet.Column("AA").Width = 40;
            worksheet.Column("AB").Width = 40;
            worksheet.Column("AC").Width = 40;

            foreach (var item in listTotalInvestment)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.TranDate;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.CustomerName;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.IdType;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.PermanentAddress;
                worksheet.Cell(currentRow, 7).Value = item.Cifcode;
                worksheet.Cell(currentRow, 8).Value = item?.Sex;
                worksheet.Cell(currentRow, 9).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAccount;
                worksheet.Cell(currentRow, 11).Value = item?.OwnerBankAccount;
                worksheet.Cell(currentRow, 12).Value = item?.BankName;
                worksheet.Cell(currentRow, 13).Value = item?.ReferralCode;
                worksheet.Cell(currentRow, 14).Value = item?.SalerName;
                worksheet.Cell(currentRow, 15).Value = item?.SaleDepartmentName;
                worksheet.Cell(currentRow, 16).Value = item?.Source;
                worksheet.Cell(currentRow, 17).Value = item.ProductCode;
                worksheet.Cell(currentRow, 18).Value = item.PolicyCode;
                worksheet.Cell(currentRow, 19).Value = item.IsBlockage;
                worksheet.Cell(currentRow, 20).Value = item.InvestDate;
                worksheet.Cell(currentRow, 21).Value = item.DueDate;
                worksheet.Cell(currentRow, 22).Value = item.PeriodTime;
                worksheet.Cell(currentRow, 23).Value = item.StatusDisplay;
                worksheet.Cell(currentRow, 24).Value = item.CalculateTypeDisplay;
                worksheet.Cell(currentRow, 25).Value = item.InitTotalValue;
                worksheet.Cell(currentRow, 26).Value = item.PaymentAmount;
                if (item.StatusDisplay == "Tất toán")
                {
                    worksheet.Cell(currentRow, 27).Value = 0;
                }
                else
                {
                    worksheet.Cell(currentRow, 27).Value = item.TotalValue;      // gia tri dau tu hien tai
                }
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo quản trị tổng hợp
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExcelGarnerListAdministrationAsync(DateTime? startDate, DateTime? endDate)
        {
            var listTotalInvestment = new List<GarnerListAdministrationDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            //lấy tradingProviderId nếu user type là đại lý
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var result = new ExportResultDto();
            List<GarnerListInvestment> totalAdministrationFind = new List<GarnerListInvestment>();

            var payWithdrawal = _garnerExportExcelReportRepositories.ListPayWithdrawal(tradingProviderId, partnerId, startDate, endDate);
            var interstPayment = _garnerExportExcelReportRepositories.ListGarnerInterstPayment(tradingProviderId, partnerId, startDate, endDate);
            var investment = _garnerExportExcelReportRepositories.ListInvestments(tradingProviderId, partnerId, startDate, endDate);

            totalAdministrationFind.AddRange(payWithdrawal);
            totalAdministrationFind.AddRange(interstPayment);
            totalAdministrationFind.AddRange(investment);

            List<Task> tasks = new();
            ConcurrentBag<GarnerListAdministrationDto> listTotalInvestmentCurrentbag = new();
            foreach (var item in totalAdministrationFind)
            {
                var task = Task.Run(() =>
                {
                    var dbContext = CommonUtils.GetService<EpicSchemaDbContextTransient>(_httpContext);
                    var investorEFRepository = new InvestorEFRepository(dbContext, _logger);
                    var investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, _logger);
                    var businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, _logger);
                    var businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, _logger);
                    var saleEFRepository = new SaleEFRepository(dbContext, _logger);
                    var policyEFRepository = new GarnerPolicyEFRepository(dbContext, _logger);
                    var garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, _logger);
                    var bankEFRepository = new BankEFRepository(dbContext, _logger);
                    var garnerExportExcelReportRepositories = new GarnerExportExcelReportRepositories(dbContext, _logger);
                    var garnerCalendarEFRepository = new GarnerCalendarEFRepository(dbContext, _logger);

                    var totalInvestmentData = new GarnerListAdministrationDto();
                    totalInvestmentData.CurrentInvestment = garnerExportExcelReportRepositories.SumValueCurrentInvestment((int)item?.OrderId, item.TradingProviderId, item.TranDate);
                    totalInvestmentData.Cifcode = item.CifCode;
                    totalInvestmentData.TranDate = item.TranDate?.Date;

                    if (item.BusinessCustomerId == null && item.InvestorId != null)
                    {
                        var investorIden = investorEFRepository.GetDefaultIdentification(item?.InvestorId ?? 0);
                        var investorBankAccount = investorBankAccountEFRepository.FindById(item?.InvestorBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(investorBankAccount?.BankId ?? 0);

                        totalInvestmentData.CustomerName = investorIden?.Fullname;
                        totalInvestmentData.IdNo = investorIden?.IdNo;
                        totalInvestmentData.IdType = investorIden?.IdType;
                        totalInvestmentData.PermanentAddress = investorIden?.PlaceOfResidence;
                        totalInvestmentData.Sex = ExcelDataUtils.GenderDisplay(investorIden?.Sex);
                        totalInvestmentData.BankAccount = investorBankAccount?.BankAccount;
                        totalInvestmentData.BankName = bank?.BankName;
                        totalInvestmentData.OwnerBankAccount = investorBankAccount?.OwnerAccount;
                    }
                    else if (item.BusinessCustomerId != null && item.InvestorId == null)
                    {
                        var businessCustomer = businessCustomerEFRepository.FindById(item.BusinessCustomerId ?? 0);
                        var businessBankAccount = businessCustomerBankEFRepository.FindById(item?.BusinessCustomerBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(businessBankAccount?.BankId ?? 0);
                        totalInvestmentData.CustomerName = businessCustomer?.Name;
                        totalInvestmentData.IdNo = businessCustomer?.TaxCode;
                        totalInvestmentData.IdType = TaxCode.MA_SO_THUE;
                        totalInvestmentData.BankAccount = businessBankAccount?.BankAccNo;
                        totalInvestmentData.BankName = bank?.BankName;
                        totalInvestmentData.OwnerBankAccount = businessBankAccount?.BankAccName;
                    }

                    if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId == null)
                    {
                        totalInvestmentData.Orderer = ExcelDataUtils.SourceOrderer(SourceOrderFE.QUAN_TRI_VIEN);
                    }
                    else if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId != null)
                    {
                        totalInvestmentData.Orderer = ExcelDataUtils.SourceOrderer(SourceOrderFE.SALE);
                    }
                    else if (item.Source == SourceOrder.ONLINE)
                    {
                        totalInvestmentData.Orderer = ExcelDataUtils.SourceOrderer(SourceOrderFE.KHACH_HANG);
                    }

                    var policyFind = policyEFRepository.FindById(item.PolicyId ?? 0, item.TradingProviderId);
                    var saleInvestorInfo = saleEFRepository.FindSaleByRefferCode(item.SaleReferralCode, true, tradingProviderId);
                    var saleBusinessInfo = saleEFRepository.FindSaleByRefferCode(item.SaleReferralCode, false, tradingProviderId);

                    if (saleInvestorInfo != null && saleBusinessInfo == null)
                    {
                        totalInvestmentData.SalerName = saleInvestorInfo?.Fullname;
                        totalInvestmentData.SaleDepartmentName = saleInvestorInfo?.DepartmentName;
                    }
                    else if (saleInvestorInfo == null && saleBusinessInfo != null)
                    {
                        totalInvestmentData.SalerName = saleBusinessInfo?.Name;
                        totalInvestmentData.SaleDepartmentName = saleBusinessInfo?.DepartmentName;
                    }
                    if (policyFind?.GarnerType != PolicyGarnerTypes.LINH_HOAT)
                    {
                        totalInvestmentData.PeriodTime = policyFind?.InterestPeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(policyFind?.InterestPeriodType);
                    }
                    totalInvestmentData.Source = ExcelDataUtils.TradingTypeDisplay(item.Source);
                    if (item?.ProfitRate == 0)
                    {
                        totalInvestmentData.ProfitRate = "";
                    }
                    else if (item?.GarnerType == PolicyGarnerTypes.LINH_HOAT)
                    {
                        totalInvestmentData.ProfitRate = "";
                    }    
                    else
                    {
                        totalInvestmentData.ProfitRate = item?.ProfitRate * 100 + "%";
                    }
                    totalInvestmentData.Profit = item.Profit;
                    totalInvestmentData.TranType = ExcelDataUtils.TranTypeDisplay(item.TranType);
                    totalInvestmentData.ProductCode = item.ProductCode;
                    totalInvestmentData.GarnerType = ExcelDataUtils.GarnerType(policyFind?.GarnerType);
                    totalInvestmentData.ContractCode = item.ContractCode;
                    totalInvestmentData.ReferralCode = item.SaleReferralCode;
                    totalInvestmentData.PolicyCode = policyFind?.Code;
                    totalInvestmentData.TranClassify = ExcelDataUtils.TranClassify(item.TranClassify);
                    totalInvestmentData.IsBlockage = item.Status == OrderStatus.PHONG_TOA ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;
                    totalInvestmentData.InvestDate = item.InvestDate?.Date;
                    totalInvestmentData.InitTotalValue = item.InitTotalValue;
                    if (item?.InvestDate != null && policyFind?.GarnerType != PolicyGarnerTypes.LINH_HOAT)
                    {
                        totalInvestmentData.DueDate = garnerPolicyDetailEFRepository.CalculateDueDate(item?.InvestDate ?? new DateTime(), policyFind?.InterestPeriodQuantity ?? 0, policyFind?.InterestPeriodType);
                    }
                    else if (item?.Status == OrderStatus.TAT_TOAN)
                    {
                        totalInvestmentData.DueDate = item.SettlementDate;
                    }
 
                    totalInvestmentData.StatusDisplay = ExcelDataUtils.StatusOrder(item?.Status);
                    totalInvestmentData.CalculateTypeDisplay = ExcelDataUtils.CaculateTypeDisplay(policyFind?.CalculateType);
                    totalInvestmentData.TotalValue = item.TotalValue;
                    totalInvestmentData.PaymentAmount = item.PaymentAmount;

                    listTotalInvestmentCurrentbag.Add(totalInvestmentData);
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            listTotalInvestment.AddRange(listTotalInvestmentCurrentbag);

            listTotalInvestment = listTotalInvestment.OrderBy(o => o.TranDate).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("GARNER");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 5).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Địa chỉ thường trú";
            worksheet.Cell(currentRow, 7).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 8).Value = "Giới tính";
            worksheet.Cell(currentRow, 9).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 10).Value = "Tài khoản nhận tiền KH";
            worksheet.Cell(currentRow, 11).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 12).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 13).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 14).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 15).Value = "Phòng ban";
            worksheet.Cell(currentRow, 16).Value = "Kiểu giao dịch";
            worksheet.Cell(currentRow, 17).Value = "Nguồn đặt";
            worksheet.Cell(currentRow, 18).Value = "Sản phẩm";
            worksheet.Cell(currentRow, 19).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 20).Value = "Loại hình kỳ hạn";
            worksheet.Cell(currentRow, 21).Value = "Phong tỏa";
            worksheet.Cell(currentRow, 22).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 23).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 24).Value = "Thời hạn";
            worksheet.Cell(currentRow, 25).Value = "Lợi tức";
            worksheet.Cell(currentRow, 26).Value = "Tình trạng";
            worksheet.Cell(currentRow, 27).Value = "Phần tiền";
            worksheet.Cell(currentRow, 28).Value = "Loại chi";
            worksheet.Cell(currentRow, 29).Value = "Gross/Net";
            worksheet.Cell(currentRow, 30).Value = "Giá trị đầu tư theo hợp đồng";
            worksheet.Cell(currentRow, 31).Value = "Số tiền chuyển";
            worksheet.Cell(currentRow, 32).Value = "Giá trị đầu tư hiện tại";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;
            worksheet.Column("S").Width = 40;
            worksheet.Column("T").Width = 40;
            worksheet.Column("U").Width = 40;
            worksheet.Column("V").Width = 40;
            worksheet.Column("W").Width = 40;
            worksheet.Column("X").Width = 40;
            worksheet.Column("Y").Width = 40;
            worksheet.Column("Z").Width = 40;
            worksheet.Column("AA").Width = 40;
            worksheet.Column("AB").Width = 40;
            worksheet.Column("AC").Width = 40;
            worksheet.Column("AD").Width = 40;
            worksheet.Column("AE").Width = 40;
            worksheet.Column("AF").Width = 40;
            worksheet.Column("AG").Width = 40;

            foreach (var item in listTotalInvestment)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.TranDate;               // ngay giao dich
                worksheet.Cell(currentRow, 2).Style.DateFormat.Format = "dd/MM/yyyy";
                worksheet.Cell(currentRow, 3).Value = "'" + item?.CustomerName;     // ten khach hang
                worksheet.Cell(currentRow, 4).Value = "'" + item?.IdNo;             // so giay to
                worksheet.Cell(currentRow, 5).Value = "'" + item?.IdType;           // loai giay to
                worksheet.Cell(currentRow, 6).Value = "'" + item?.PermanentAddress; //dia chi thuong tru
                worksheet.Cell(currentRow, 7).Value = "'" + item.Cifcode;           // ma khach hang
                worksheet.Cell(currentRow, 8).Value = item?.Sex;                    // gioi tinh
                worksheet.Cell(currentRow, 9).Value = "'" + item?.ContractCode;     //ma hop dong
                worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAccount;     // tai khoan nhan tien kh
                worksheet.Cell(currentRow, 11).Value = item?.OwnerBankAccount;      // ten chu tai khoan
                worksheet.Cell(currentRow, 12).Value = item?.BankName;              // ngan hang
                worksheet.Cell(currentRow, 13).Value = "'" + item?.ReferralCode;    // ma gioi thieu
                worksheet.Cell(currentRow, 14).Value = item?.SalerName;             //ten nguoi gioi thieu
                worksheet.Cell(currentRow, 15).Value = item?.SaleDepartmentName;    // phong ban
                worksheet.Cell(currentRow, 16).Value = item?.Source;                // kieu giao dich
                worksheet.Cell(currentRow, 17).Value = item?.Orderer;               // Nguồn
                worksheet.Cell(currentRow, 18).Value = "'" + item.ProductCode;      // san pham
                worksheet.Cell(currentRow, 19).Value = "'" + item.PolicyCode;       // ma chinh sach
                worksheet.Cell(currentRow, 20).Value = item.GarnerType;             // loai ky han
                worksheet.Cell(currentRow, 21).Value = item.IsBlockage;             // phong toa
                worksheet.Cell(currentRow, 22).Value = item.InvestDate;             // ngay dau tu
                worksheet.Cell(currentRow, 23).Value = item.DueDate;                // ngay ket thuc
                worksheet.Cell(currentRow, 24).Value = item.PeriodTime;             // thoi han
                worksheet.Cell(currentRow, 25).Value = "'" + item.ProfitRate;           // loi tuc
                worksheet.Cell(currentRow, 26).Value = item.StatusDisplay;          // tinh trang
                worksheet.Cell(currentRow, 27).Value = item.TranClassify;           // phan tien
                worksheet.Cell(currentRow, 28).Value = item?.TranType;              // loai chi
                worksheet.Cell(currentRow, 29).Value = item.CalculateTypeDisplay;   // gross net
                worksheet.Cell(currentRow, 30).Value = item.InitTotalValue;         // gia tri dau tu theo hop dong
                if (item.TranClassify == "Lợi tức")
                {
                    worksheet.Cell(currentRow, 31).Value = item.Profit;          // so tien chuyen
                }
                else
                {
                    worksheet.Cell(currentRow, 31).Value = item.PaymentAmount;
                }
                if (item.StatusDisplay == "Tất toán")
                {
                    worksheet.Cell(currentRow, 32).Value = 0;
                }
                else
                {
                    worksheet.Cell(currentRow, 32).Value = item.TotalValue;      // gia tri dau tu hien tai
                }

                if (item.TranClassify == "Lợi tức")
                {
                    currentRow++;
                    stt = ++stt;
                    worksheet.Cell(currentRow, 1).Value = stt;
                    worksheet.Cell(currentRow, 2).Value = item?.TranDate;               // ngay giao dich
                    worksheet.Cell(currentRow, 2).Style.DateFormat.Format = "dd/MM/yyyy";
                    worksheet.Cell(currentRow, 3).Value = "'" + item?.CustomerName;     // ten khach hang
                    worksheet.Cell(currentRow, 4).Value = "'" + item?.IdNo;             // so giay to
                    worksheet.Cell(currentRow, 5).Value = "'" + item?.IdType;           // loai giay to
                    worksheet.Cell(currentRow, 6).Value = "'" + item?.PermanentAddress; //dia chi thuong tru
                    worksheet.Cell(currentRow, 7).Value = "'" + item.Cifcode;           // ma khach hang
                    worksheet.Cell(currentRow, 8).Value = item?.Sex;                    // gioi tinh
                    worksheet.Cell(currentRow, 9).Value = "'" + item?.ContractCode;     //ma hop dong
                    worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAccount;     // tai khoan nhan tien kh
                    worksheet.Cell(currentRow, 11).Value = item?.OwnerBankAccount;      // ten chu tai khoan
                    worksheet.Cell(currentRow, 12).Value = item?.BankName;              // ngan hang
                    worksheet.Cell(currentRow, 13).Value = "'" + item?.ReferralCode;    // ma gioi thieu
                    worksheet.Cell(currentRow, 14).Value = item?.SalerName;             //ten nguoi gioi thieu
                    worksheet.Cell(currentRow, 15).Value = item?.SaleDepartmentName;    // phong ban
                    worksheet.Cell(currentRow, 16).Value = item?.Source;                // kieu giao dich
                    worksheet.Cell(currentRow, 17).Value = item?.Orderer;               // Nguồn
                    worksheet.Cell(currentRow, 18).Value = "'" + item.ProductCode;      // san pham
                    worksheet.Cell(currentRow, 19).Value = "'" + item.PolicyCode;       // ma chinh sach
                    worksheet.Cell(currentRow, 20).Value = item.GarnerType;             // loai ky han
                    worksheet.Cell(currentRow, 21).Value = item.IsBlockage;             // phong toa
                    worksheet.Cell(currentRow, 22).Value = item.InvestDate;             // ngay dau tu
                    worksheet.Cell(currentRow, 23).Value = item.DueDate;                // ngay ket thuc
                    worksheet.Cell(currentRow, 24).Value = item.PeriodTime;             // thoi han
                    worksheet.Cell(currentRow, 25).Value = "'" + item.ProfitRate;       // loi tuc
                    worksheet.Cell(currentRow, 26).Value = item.StatusDisplay;          // tinh trang
                    worksheet.Cell(currentRow, 27).Value = "Gốc";                       // phan tien
                    worksheet.Cell(currentRow, 28).Value = item?.TranType;              // loai chi
                    worksheet.Cell(currentRow, 29).Value = item.CalculateTypeDisplay;   // gross net
                    worksheet.Cell(currentRow, 30).Value = item.InitTotalValue;         // gia tri dau tu theo hop dong
                    worksheet.Cell(currentRow, 31).Value = item.PaymentAmount;          // so tien chuyen
                    if (item.StatusDisplay == "Tất toán")
                    {
                        worksheet.Cell(currentRow, 32).Value = 0;
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 32).Value = item.TotalValue;      // gia tri dau tu hien tai
                    }
                }
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }


        /// <summary>
        /// Báo cáo tổng chi trả đầu tư
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto TotalInvestmentPayment(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;

            List<int> tradingProviderIds = new List<int>();
            if (tradingProviderId != null)
            {
                tradingProviderIds.Add(tradingProviderId ?? 0);
            }
            else
            {
                tradingProviderIds.AddRange(_tradingProviderEFRepository.GetAllByPartnerId(partnerId ?? 0).Select(o => o.TradingProviderId).ToList());
            }

            List<GarnerListInvestmentPaymentDto> totalInvestmentPayment = new List<GarnerListInvestmentPaymentDto>();

            var interstPayment = _garnerExportExcelReportRepositories.TotalInterestPayment(tradingProviderId, startDate, endDate);
            interstPayment = interstPayment.Where(r => tradingProviderIds.Contains(r.TradingProviderId) && (startDate != null && r.PaymentDate?.Date >= startDate?.Date) && (endDate != null && r.PaymentDate?.Date <= endDate?.Date)).ToList();
            var payWithdrawal = _garnerExportExcelReportRepositories.TotalWithdrawalPayment(tradingProviderId, startDate, endDate);
            payWithdrawal = payWithdrawal.Where(r => tradingProviderIds.Contains(r.TradingProviderId) && (startDate != null && r.PaymentDate?.Date >= startDate?.Date) && (endDate != null && r.PaymentDate?.Date <= endDate?.Date)).ToList();


            /// ko group vao nua tach ra
            totalInvestmentPayment.AddRange(payWithdrawal);
            totalInvestmentPayment.AddRange(interstPayment);

            var listItem = totalInvestmentPayment.OrderBy(o => o.PaymentDate).GroupBy(i => i.PaymentDate.Value.Date)
                .Select(o => new
                {
                    Date = o.Key,
                    SumAmountMoney = o.Sum(d => d.AmountMoney)
                });

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Garner");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày chi trả";
            worksheet.Cell(currentRow, 3).Value = "Số tiền thực chi";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;

            foreach (var interest in listItem)
            {
                if (interest.SumAmountMoney == 0)
                {
                    continue;
                }
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = interest.Date;
                worksheet.Cell(currentRow, 3).Value = "'" + interest.SumAmountMoney;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo thực chi
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExcelGarnerActualPaymentAsync(DateTime? startDate, DateTime? endDate)
        {
            var listActualPayment = new List<GarnerListActualPaymentDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            //lấy tradingProviderId nếu user type là đại lý
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var result = new ExportResultDto();
            var totalActualPaymentFind = _garnerExportExcelReportRepositories.ListGarnerActualInvestment(tradingProviderId, partnerId, startDate, endDate);

            List<Task> tasks = new();
            ConcurrentBag<GarnerListActualPaymentDto> listActualPaymentConcurrentBag = new();
            foreach (var item in totalActualPaymentFind)
            {
                var task = Task.Run(() =>
                {
                    var dbContext = CommonUtils.GetService<EpicSchemaDbContextTransient>(_httpContext);
                    var investorEFRepository = new InvestorEFRepository(dbContext, _logger);
                    var investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, _logger);
                    var businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, _logger);
                    var businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, _logger);
                    var saleEFRepository = new SaleEFRepository(dbContext, _logger);
                    var garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, _logger);
                    var bankEFRepository = new BankEFRepository(dbContext, _logger);
                    var garnerExportExcelReportRepositories = new GarnerExportExcelReportRepositories(dbContext, _logger);
                    var actualPaymentData = new GarnerListActualPaymentDto();
                    var garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, _logger);
                    var garnerFormulaServices = new GarnerFormulaServices(dbContext, _mapper, _loggerFomula, _httpContext);
                    var garnerCalendarEFRepository = new GarnerCalendarEFRepository(dbContext, _logger);

                    actualPaymentData.CurrentInvestment = garnerExportExcelReportRepositories.SumValueCurrentInvestment((int)item?.OrderId, tradingProviderId, item.PayDate);
                    actualPaymentData.Cifcode = item.CifCode;
                    actualPaymentData.TranDate = item.TranDate;

                    if (item.BusinessCustomerId == null && item.InvestorId != null)
                    {
                        var investorIden = investorEFRepository.GetDefaultIdentification(item?.InvestorId ?? 0);
                        var investorBankAccount = investorBankAccountEFRepository.FindById(item?.InvestorBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(investorBankAccount?.BankId ?? 0);

                        actualPaymentData.CustomerName = investorIden?.Fullname;
                        actualPaymentData.IdNo = investorIden?.IdNo;
                        actualPaymentData.IdType = investorIden?.IdType;
                        actualPaymentData.PermanentAddress = investorIden?.PlaceOfResidence;
                        actualPaymentData.Sex = ExcelDataUtils.GenderDisplay(investorIden?.Sex);
                        actualPaymentData.BankAccount = investorBankAccount?.BankAccount;
                        actualPaymentData.BankName = bank?.BankName;
                        actualPaymentData.OwnerBankAccount = investorBankAccount?.OwnerAccount;
                    }
                    else if (item.BusinessCustomerId != null && item.InvestorId == null)
                    {
                        var businessCustomer = businessCustomerEFRepository.FindById(item.BusinessCustomerId ?? 0);
                        var businessBankAccount = businessCustomerBankEFRepository.FindById(item?.BusinessCustomerBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(businessBankAccount?.BankId ?? 0);
                        actualPaymentData.CustomerName = businessCustomer?.Name;
                        actualPaymentData.IdNo = businessCustomer?.TaxCode;
                        actualPaymentData.IdType = TaxCode.MA_SO_THUE;
                        actualPaymentData.BankAccount = businessBankAccount?.BankAccNo;
                        actualPaymentData.BankName = bank?.BankName;
                        actualPaymentData.OwnerBankAccount = businessBankAccount?.BankAccName;
                    }

                    var date = item.PayDate;
                    actualPaymentData.CurrentInvestment = _garnerExportExcelReportRepositories.SumValueCurrentInvestment((int)item?.OrderId, item?.TradingProviderId, item.Paydate);
                    actualPaymentData.WithdrawalAmount = item.WithdrawalAmount;
                    List<GarnerOrderWithdrawalDto> orderWithdraw = new();
                    CalculateGarnerWithdrawalDto resultCalcualte = new();
                    try
                    {
                        orderWithdraw = garnerWithdrawalEFRepository.CalOrderWithdraw(item.CifCode, item.PolicyId, item.AmountMoney ?? 0);
                        resultCalcualte = garnerFormulaServices.CalculateWithdrawal(orderWithdraw, DateTime.Now.Date);
                    }
                    catch
                    {
                    }

                    if (item.PeriodIndex == 0)
                    {
                        actualPaymentData.Profit = resultCalcualte.Profit;
                        actualPaymentData.IncomeTax = resultCalcualte.Tax;
                    }

                    actualPaymentData.PrincipalAmount = actualPaymentData.CurrentInvestment - item.WithdrawalAmount;
                    actualPaymentData.ContractCode = item.ContractCode;
                    actualPaymentData.PolicyCode = item.PolicyCode;
                    actualPaymentData.InvestDate = item.InvestDate?.Date;
                    actualPaymentData.TotalValue = item.TotalValue;
                    actualPaymentData.PayDate = item.Paydate;

                    listActualPaymentConcurrentBag.Add(actualPaymentData);

                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            listActualPayment.AddRange(listActualPaymentConcurrentBag);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("GARNER");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày chi";
            worksheet.Cell(currentRow, 3).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 4).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 7).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 8).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 9).Value = "Tổng giá trị đầu tư";
            worksheet.Cell(currentRow, 10).Value = "Tổng giá trị đầu tư hiện tại";
            worksheet.Cell(currentRow, 11).Value = "Loại chi";
            worksheet.Cell(currentRow, 12).Value = "Tiền gốc";
            worksheet.Cell(currentRow, 13).Value = "Giá trị rút vốn/ tất toán";
            worksheet.Cell(currentRow, 14).Value = "Tiền lãi";
            worksheet.Cell(currentRow, 15).Value = "Thuế TNCN";
            worksheet.Cell(currentRow, 16).Value = "Lợi tức khấu trừ";
            worksheet.Cell(currentRow, 17).Value = "Số tiền thực nhận";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;
            worksheet.Column("S").Width = 40;

            foreach (var item in listActualPayment)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.PayDate;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.CustomerName;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.IdType;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 7).Value = item.Cifcode;
                worksheet.Cell(currentRow, 8).Value = item?.PolicyCode;
                worksheet.Cell(currentRow, 9).Value = "'" + item?.TotalValue;
                worksheet.Cell(currentRow, 10).Value = item?.CurrentInvestment;
                worksheet.Cell(currentRow, 11).Value = item?.PaymentType;
                worksheet.Cell(currentRow, 12).Value = item?.PrincipalAmount;
                worksheet.Cell(currentRow, 13).Value = "'" + item?.WithdrawalAmount;
                worksheet.Cell(currentRow, 14).Value = item?.Profit;
                worksheet.Cell(currentRow, 15).Value = item?.IncomeTax;
                worksheet.Cell(currentRow, 16).Value = item?.TranType;
                worksheet.Cell(currentRow, 17).Value = "'" + item.ProductCode;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo danh sách sản phẩm tích lũy
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>  
        /// <returns></returns>
        public ExportResultDto ExcelListGarnerProduct(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Garner");
            var currentRow = 1;
            var stt = 0;

            var cumulative = (from plc in _dbContext.GarnerPolicies
                              join db in _dbContext.GarnerDistributions on plc.DistributionId equals db.Id
                              join pd in _dbContext.GarnerProducts on db.ProductId equals pd.Id
                              join ptpv in _dbContext.GarnerProductTradingProviders on pd.Id equals ptpv.ProductId
                              where pd.Deleted == YesNo.NO && plc.Deleted == YesNo.NO
                              && db.Deleted == YesNo.NO && ptpv.Deleted == YesNo.NO && db.TradingProviderId == tradingProviderId
                              && (startDate == null || startDate <= db.OpenCellDate) && (endDate == null || endDate >= db.OpenCellDate)
                              select new
                              {
                                  pd.ProductType,
                                  pd.Code,
                                  policyCode = plc.Code,
                                  plc.GarnerType,
                                  db.OpenCellDate,
                                  db.CloseCellDate,
                                  ptpv.DistributionDate,
                                  pd.InvTotalInvestment,
                                  pd.CpsQuantity,
                                  pd.CpsParValue,
                                  policyId = plc.Id,
                                  distributionId = db.Id,
                                  db.TradingProviderId
                              }).OrderByDescending(o => o.OpenCellDate).ToList();

            cumulative = cumulative.GroupBy(o => o.policyId).Select(i => i.FirstOrDefault()).ToList();

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Loại sản phẩm";
            worksheet.Cell(currentRow, 3).Value = "Mã sản phẩm";
            worksheet.Cell(currentRow, 4).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 5).Value = "Loại kỳ hạn";
            worksheet.Cell(currentRow, 6).Value = "Ngày mở bán";
            worksheet.Cell(currentRow, 7).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 8).Value = "Ngày phân phối đầu tư";
            worksheet.Cell(currentRow, 9).Value = "Hạn mức đầu tư";
            worksheet.Cell(currentRow, 10).Value = "Tổng giá trị còn lại";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("I").Style.NumberFormat.Format = "0";
            worksheet.Column("J").Width = 40;
            worksheet.Column("J").Style.NumberFormat.Format = "0";
            worksheet.Column("K").Width = 40;

            foreach (var item in cumulative)
            {
                decimal soTienDaDauTu = _garnerOrderEFRepository.SumValue(item.distributionId, item.TradingProviderId);
                decimal? remainAmount = 0;
                decimal? totalInvestment = 0;

                if (item?.InvTotalInvestment == null)
                {
                    remainAmount = (item?.CpsQuantity * item?.CpsParValue) - soTienDaDauTu;
                    totalInvestment = (item?.CpsQuantity * item?.CpsParValue);
                }
                else
                {
                    remainAmount = item?.InvTotalInvestment - soTienDaDauTu;
                    totalInvestment = item?.InvTotalInvestment;
                }

                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = ExcelDataUtils.GarnerProductType(item?.ProductType);
                worksheet.Cell(currentRow, 3).Value = "'" + item?.Code;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.policyCode;
                worksheet.Cell(currentRow, 5).Value = ExcelDataUtils.GarnerType(item?.GarnerType);
                worksheet.Cell(currentRow, 6).Value = item?.OpenCellDate?.Date.ToString("d");
                worksheet.Cell(currentRow, 7).Value = item?.CloseCellDate?.Date.ToString("d");
                worksheet.Cell(currentRow, 8).Value = item?.DistributionDate.Date.ToString("d");
                worksheet.Cell(currentRow, 9).Value = totalInvestment;
                worksheet.Cell(currentRow, 10).Value = remainAmount;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo danh sách tổng hợp các khoản đầu tư bán hộ
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExcelGarnerSaleInvestmentAsync(DateTime? startDate, DateTime? endDate)
        {
            var listTotalInvestment = new List<GarnerListAdministrationDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            //lấy tradingProviderId nếu user type là đại lý
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var result = new ExportResultDto();
            var totalAdministrationFind = _garnerExportExcelReportRepositories.ListOrderBySaleSub(tradingProviderId, partnerId, startDate, endDate);

            List<Task> tasks = new();
            ConcurrentBag<GarnerListAdministrationDto> listTotalInvestmentCurrentbag = new();
            foreach (var item in totalAdministrationFind)
            {
                var task = Task.Run(() =>
                {
                    var dbContext = CommonUtils.GetService<EpicSchemaDbContextTransient>(_httpContext);
                    var investorEFRepository = new InvestorEFRepository(dbContext, _logger);
                    var investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, _logger);
                    var businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, _logger);
                    var businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, _logger);
                    var saleEFRepository = new SaleEFRepository(dbContext, _logger);
                    var policyEFRepository = new GarnerPolicyEFRepository(dbContext, _logger);
                    var garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, _logger);
                    var bankEFRepository = new BankEFRepository(dbContext, _logger);
                    var garnerExportExcelReportRepositories = new GarnerExportExcelReportRepositories(dbContext, _logger);
                    var garnerCalendarEFRepository = new GarnerCalendarEFRepository(dbContext, _logger);

                    var totalInvestmentData = new GarnerListAdministrationDto
                    {
                        CurrentInvestment = garnerExportExcelReportRepositories.SumValueCurrentInvestment((int)item?.OrderId, item.TradingProviderId, item.TranDate),
                        Cifcode = item.CifCode,
                        TranDate = item.TranDate
                    };

                    if (item.BusinessCustomerId == null && item.InvestorId != null)
                    {
                        var investorIden = investorEFRepository.GetDefaultIdentification(item?.InvestorId ?? 0);
                        var investorBankAccount = investorBankAccountEFRepository.FindById(item?.InvestorBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(investorBankAccount?.BankId ?? 0);
                        var investor = investorEFRepository.FindById(item?.InvestorId ?? 0);
                        totalInvestmentData.Phone = investor.Phone;
                        totalInvestmentData.CustomerName = investorIden?.Fullname;
                        totalInvestmentData.IdNo = investorIden?.IdNo;
                        totalInvestmentData.IdType = investorIden?.IdType;
                        totalInvestmentData.PermanentAddress = investorIden?.PlaceOfResidence;
                        totalInvestmentData.Sex = ExcelDataUtils.GenderDisplay(investorIden?.Sex);
                        totalInvestmentData.BankAccount = investorBankAccount?.BankAccount;
                        totalInvestmentData.BankName = bank?.BankName;
                        totalInvestmentData.OwnerBankAccount = investorBankAccount?.OwnerAccount;
                    }
                    else if (item.BusinessCustomerId != null && item.InvestorId == null)
                    {
                        var businessCustomer = businessCustomerEFRepository.FindById(item.BusinessCustomerId ?? 0);
                        var businessBankAccount = businessCustomerBankEFRepository.FindById(item?.BusinessCustomerBankAccId ?? 0);
                        var bank = bankEFRepository.FindById(businessBankAccount?.BankId ?? 0);
                        totalInvestmentData.CustomerName = businessCustomer?.Name;
                        totalInvestmentData.IdNo = businessCustomer?.TaxCode;
                        totalInvestmentData.IdType = TaxCode.MA_SO_THUE;
                        totalInvestmentData.BankAccount = businessBankAccount?.BankAccNo;
                        totalInvestmentData.BankName = bank?.BankName;
                        totalInvestmentData.OwnerBankAccount = businessBankAccount?.BankAccName;
                        totalInvestmentData.Phone = businessCustomer.Phone;
                    }

                    if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId == null)
                    {
                        totalInvestmentData.Orderer = ExcelDataUtils.SourceOrderer(SourceOrderFE.QUAN_TRI_VIEN);
                    }
                    else if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId != null)
                    {
                        totalInvestmentData.Orderer = ExcelDataUtils.SourceOrderer(SourceOrderFE.SALE);
                    }
                    else if (item.Source == SourceOrder.ONLINE)
                    {
                        totalInvestmentData.Orderer = ExcelDataUtils.SourceOrderer(SourceOrderFE.KHACH_HANG);
                    }

                    var policyFind = policyEFRepository.FindById(item.PolicyId ?? 0, item.TradingProviderId);
                    totalInvestmentData.SaleReferralCodeSub = item.SaleReferralCodeSub;
                    totalInvestmentData.SalerName = saleEFRepository.FindSaleName(item.SaleReferralCodeSub)?.Name;

                    var saleInvestorInfo = saleEFRepository.FindSaleByRefferCode(item.SaleReferralCodeSub, true, null);
                    var saleBusinessInfo = saleEFRepository.FindSaleByRefferCode(item.SaleReferralCodeSub, false, null);

                    if (saleInvestorInfo != null && saleBusinessInfo == null)
                    {
                        totalInvestmentData.SalerName = saleInvestorInfo?.Fullname;
                        totalInvestmentData.SaleDepartmentName = saleInvestorInfo?.DepartmentName;
                    }
                    else if (saleInvestorInfo == null && saleBusinessInfo != null)
                    {
                        totalInvestmentData.SalerName = saleBusinessInfo?.Name;
                        totalInvestmentData.SaleDepartmentName = saleBusinessInfo?.DepartmentName;
                    }

                    if (policyFind?.GarnerType != PolicyGarnerTypes.LINH_HOAT)
                    {
                        totalInvestmentData.PeriodTime = policyFind?.InterestPeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(policyFind?.InterestPeriodType);
                    }
                    totalInvestmentData.Source = ExcelDataUtils.TradingTypeDisplay(item.Source);

                    if (item?.ProfitRate == 0)
                    {
                        totalInvestmentData.ProfitRate = "";
                    }
                    else
                    {
                        totalInvestmentData.ProfitRate = item?.ProfitRate + "%";
                    }

                    totalInvestmentData.Profit = item.Profit;
                    totalInvestmentData.ProductCode = item.ProductCode;
                    totalInvestmentData.GarnerType = ExcelDataUtils.GarnerType(policyFind?.GarnerType);
                    totalInvestmentData.ContractCode = item.ContractCode;
                    totalInvestmentData.OrderId = item.OrderId ?? 0;
                    totalInvestmentData.ReferralCode = item.SaleReferralCode;
                    totalInvestmentData.PolicyCode = policyFind?.Code;
                    totalInvestmentData.TranClassify = ExcelDataUtils.TranClassify(item.TranClassify);
                    totalInvestmentData.IsBlockage = item.Status == OrderStatus.PHONG_TOA ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;
                    totalInvestmentData.InvestDate = item.InvestDate?.Date;
                    totalInvestmentData.InitTotalValue = item.InitTotalValue;
                    if (item?.InvestDate != null && policyFind?.GarnerType != PolicyGarnerTypes.LINH_HOAT)
                    {
                        totalInvestmentData.DueDate = garnerPolicyDetailEFRepository.CalculateDueDate(item?.InvestDate ?? new DateTime(), policyFind?.InterestPeriodQuantity ?? 0, policyFind?.InterestPeriodType);
                    }
                    totalInvestmentData.StatusDisplay = ExcelDataUtils.StatusOrder(item?.Status);
                    totalInvestmentData.CalculateTypeDisplay = ExcelDataUtils.CaculateTypeDisplay(policyFind?.CalculateType);
                    totalInvestmentData.TotalValue = item.TotalValue;
                    totalInvestmentData.PaymentAmount = item.PaymentAmount;

                    listTotalInvestmentCurrentbag.Add(totalInvestmentData);
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            listTotalInvestment.AddRange(listTotalInvestmentCurrentbag);
            listTotalInvestment = listTotalInvestment.OrderByDescending(o => o.OrderId).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("GARNER");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Giới tính";
            worksheet.Cell(currentRow, 6).Value = "Số điện thoại";
            worksheet.Cell(currentRow, 7).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 8).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 9).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 10).Value = "Tài khoản nhận tiền KH";
            worksheet.Cell(currentRow, 11).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 12).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 13).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 14).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 15).Value = "Phòng ban";
            worksheet.Cell(currentRow, 16).Value = "Kiểu giao dịch";
            worksheet.Cell(currentRow, 17).Value = "Sản phẩm";
            worksheet.Cell(currentRow, 18).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 19).Value = "Phong tỏa";
            worksheet.Cell(currentRow, 20).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 21).Value = "Tình trạng";
            worksheet.Cell(currentRow, 22).Value = "Lợi tức";
            worksheet.Cell(currentRow, 23).Value = "Giá trị đầu tư theo hợp đồng";
            worksheet.Cell(currentRow, 24).Value = "Số tiền KH chuyển";
            worksheet.Cell(currentRow, 25).Value = "Giá trị đầu tư hiện tại";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;
            worksheet.Column("S").Width = 40;
            worksheet.Column("T").Width = 40;
            worksheet.Column("U").Width = 40;
            worksheet.Column("V").Width = 40;
            worksheet.Column("W").Width = 40;
            worksheet.Column("X").Width = 40;
            worksheet.Column("Y").Width = 40;
            worksheet.Column("Z").Width = 40;
            worksheet.Column("AA").Width = 40;
            worksheet.Column("AB").Width = 40;
            worksheet.Column("AC").Width = 40;

            foreach (var item in listTotalInvestment)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.TranDate;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.CustomerName;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.Cifcode;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.Sex;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.Phone;
                worksheet.Cell(currentRow, 7).Value = "'" + item.IdNo;
                worksheet.Cell(currentRow, 8).Value = item?.IdType;
                worksheet.Cell(currentRow, 9).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAccount;
                worksheet.Cell(currentRow, 11).Value = item?.OwnerBankAccount;
                worksheet.Cell(currentRow, 12).Value = item?.BankName;
                worksheet.Cell(currentRow, 13).Value = "'" + item?.SaleReferralCodeSub;
                worksheet.Cell(currentRow, 14).Value = item?.SalerName;
                worksheet.Cell(currentRow, 15).Value = item?.SaleDepartmentName;
                worksheet.Cell(currentRow, 16).Value = item?.Source;
                worksheet.Cell(currentRow, 17).Value = "'" + item.ProductCode;
                worksheet.Cell(currentRow, 18).Value = "'" + item.PolicyCode;
                worksheet.Cell(currentRow, 19).Value = item.IsBlockage;
                worksheet.Cell(currentRow, 20).Value = item.InvestDate;
                worksheet.Cell(currentRow, 21).Value = item.StatusDisplay;
                worksheet.Cell(currentRow, 22).Value = item.CalculateTypeDisplay;
                worksheet.Cell(currentRow, 23).Value = item.InitTotalValue;
                worksheet.Cell(currentRow, 24).Value = item.PaymentAmount;
                worksheet.Cell(currentRow, 25).Value = item.TotalValue;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }
    }
}
