using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.InvestRepositories;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstExportExcel;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;
using static EPIC.Utils.DisplayType;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstExportExcelReportServices : IRstExportExcelReportServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstExportExcelReportServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstExportExcelReportRepositories _rstExportExcelReportRepositories;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly BankEFRepository _bankEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;

        public RstExportExcelReportServices(
            DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<RstExportExcelReportServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _rstExportExcelReportRepositories = new RstExportExcelReportRepositories(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _bankEFRepository = new BankEFRepository(dbContext, logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(dbContext, logger);
        }

        public async Task<ExportResultDto> ProductProjectOverview(DateTime? startDate, DateTime? endDate)
        {
            var ListProductProjectOverview = new List<RstListProductProjectOverviewDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var result = new ExportResultDto();
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;

            var productProjectOverview = new List<RstProductProjectOverviewDto>();

            if (tradingProviderId != null)
            {
                productProjectOverview = _rstExportExcelReportRepositories.ProductProjectOverviewTradingProvider(tradingProviderId, startDate, endDate).ToList();
            }
            if (partnerId != null)
            {
                productProjectOverview = _rstExportExcelReportRepositories.ProductProjectOverviewPartner(partnerId, startDate, endDate).ToList();
            }

            var listItem = productProjectOverview.GroupBy(o => new { o.Code, o.BuildingDensityType, o.BuildingDensityName }).Select(o => new
            {
                Code = o.Key.Code,
                BuildingDensityType = o.Key.BuildingDensityType,
                BuildingDensityName = o.Key.BuildingDensityName,

                totalItem = o.Count(),
                totalSell = o.Count(i => i.Status >= RstProductItemStatus.DA_COC),
                // tổng các căn hộ
                CHThongThuong = o.Count(i => i.ClassifyType == RstClassifyType.CanHoThongThuong),
                CHStudio = o.Count(i => i.ClassifyType == RstClassifyType.CanHoStudio),
                CHOfficetel = o.Count(i => i.ClassifyType == RstClassifyType.CanHoOfficetel),
                CHShophouse = o.Count(i => i.ClassifyType == RstClassifyType.CanHoShophouse),
                CHPenthouse = o.Count(i => i.ClassifyType == RstClassifyType.CanHoPenthouse),
                CHDuplex = o.Count(i => i.ClassifyType == RstClassifyType.CanHoDuplex),
                CHSkyVilla = o.Count(i => i.ClassifyType == RstClassifyType.CanHoSkyVilla),
                BietThu = (o.Count(i => i.ClassifyType == RstClassifyType.BietThuNghiDuong) + o.Count(i => i.ClassifyType == RstClassifyType.BietThuNhaO)),
                Villa = o.Count(i => i.ClassifyType == RstClassifyType.Villa),
                LienKe = o.Count(i => i.ClassifyType == RstClassifyType.LienKe),
                NhaO = o.Count(i => i.ClassifyType == RstClassifyType.NhaONongThon),
                ChungCuThapTang = o.Count(i => i.ClassifyType == RstClassifyType.ChungCuThapTang),
                CanShophouse = o.Count(i => i.ClassifyType == RstClassifyType.CanShophouse),

                // tổng những căn hộ đã bán
                SellCHThongThuong = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanHoThongThuong),
                SellCHStudio = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanHoStudio),
                SellCHOfficetel = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanHoOfficetel),
                SellCHShophouse = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanHoShophouse),
                SellCHPenthouse = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanHoPenthouse),
                SellCHDuplex = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanHoDuplex),
                SellCHSkyVilla = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanHoSkyVilla),
                SellBietThu = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.BietThuNghiDuong)
                    + o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.BietThuNhaO),
                SellVilla = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.Villa),
                SellLienKe = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.LienKe),
                SellNhaO = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.NhaONongThon),
                SellChungCuThapTang = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.ChungCuThapTang),
                SellCanShophouse = o.Count(i => i.Status >= RstProductItemStatus.DA_COC && i.ClassifyType == RstClassifyType.CanShophouse),
            });

            List<Task> tasks = new();
            ConcurrentBag<RstListProductProjectOverviewDto> listProductProjectOverviewCurrentbag = new();
            foreach (var item in listItem)
            {
                //var task = Task.Run(() =>
                //{
                // Số lượng chưa bán
                decimal soLuongChuaBan = item.totalItem - item.totalSell;

                // Tỷ lệ hàng tồn
                decimal tyLeHangTon = (soLuongChuaBan / item.totalItem) * 100;

                var ItemProductProjectOverview = new RstListProductProjectOverviewDto();
                ItemProductProjectOverview.Code = item.Code;
                ItemProductProjectOverview.BuildingDensityType = ExcelDataUtils.RstBuildingDensityType(item.BuildingDensityType) + " " + item.BuildingDensityName;
                ItemProductProjectOverview.CHThongThuong = item.CHThongThuong;
                ItemProductProjectOverview.CHStudio = item.CHStudio;
                ItemProductProjectOverview.CHShophouse = item.CHShophouse;
                ItemProductProjectOverview.CHPenthouse = item.CHPenthouse;
                ItemProductProjectOverview.CHDuplex = item.CHDuplex;
                ItemProductProjectOverview.CHSkyVilla = item.CHSkyVilla;
                ItemProductProjectOverview.BietThu = item.BietThu;
                ItemProductProjectOverview.Villa = item.Villa;
                ItemProductProjectOverview.LienKe = item.LienKe;
                ItemProductProjectOverview.NhaO = item.NhaO;
                ItemProductProjectOverview.CHOfficetel = item.CHOfficetel;
                ItemProductProjectOverview.ChungCuThapTang = item.ChungCuThapTang;
                ItemProductProjectOverview.CanShophouse = item.CanShophouse;
                ItemProductProjectOverview.SellCHThongThuong = item.SellCHThongThuong;
                ItemProductProjectOverview.SellCHStudio = item.SellCHStudio;
                ItemProductProjectOverview.SellCHShophouse = item.SellCHShophouse;
                ItemProductProjectOverview.SellCHPenthouse = item.SellCHPenthouse;
                ItemProductProjectOverview.SellCHDuplex = item.SellCHDuplex;
                ItemProductProjectOverview.SellCHSkyVilla = item.SellCHSkyVilla;
                ItemProductProjectOverview.SellBietThu = item.SellBietThu;
                ItemProductProjectOverview.SellVilla = item.SellVilla;
                ItemProductProjectOverview.SellLienKe = item.SellLienKe;
                ItemProductProjectOverview.SellNhaO = item.SellNhaO;
                ItemProductProjectOverview.SellCHOfficetel = item.SellCHOfficetel;
                ItemProductProjectOverview.SellChungCuThapTang = item.SellChungCuThapTang;
                ItemProductProjectOverview.SellCanShophouse = item.SellCanShophouse;
                ItemProductProjectOverview.SoLuongChuaBan = soLuongChuaBan;
                ItemProductProjectOverview.TyLeHangTon = tyLeHangTon;

                listProductProjectOverviewCurrentbag.Add(ItemProductProjectOverview);
            };
            //    tasks.Add(task);
            //}

            await Task.WhenAll(tasks);

            ListProductProjectOverview.AddRange(listProductProjectOverviewCurrentbag);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("RST");
            var currentRow = 2;
            var stt = 0;
            worksheet.Cell(1, 4).Value = "Tổng số lượng hàng";
            worksheet.Range("D1:P2").Style.Fill.BackgroundColor = XLColor.FromArgb(226, 239, 218);

            worksheet.Cell(1, 17).Value = "Tổng số lượng hàng đã bán";
            worksheet.Range("Q1:AC2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 242, 204);


            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Dự án";
            worksheet.Cell(currentRow, 3).Value = "Phân khu/Tòa";
            worksheet.Cell(currentRow, 4).Value = "CH thông thường";
            worksheet.Cell(currentRow, 5).Value = "CH Studio";
            worksheet.Cell(currentRow, 6).Value = "CH Shophouse";
            worksheet.Cell(currentRow, 7).Value = "CH Penthouse";
            worksheet.Cell(currentRow, 8).Value = "CH Duplex";
            worksheet.Cell(currentRow, 9).Value = "CH Sky Villa";
            worksheet.Cell(currentRow, 10).Value = "Biệt thự";
            worksheet.Cell(currentRow, 11).Value = "Villa";
            worksheet.Cell(currentRow, 12).Value = "Liền kề";
            worksheet.Cell(currentRow, 13).Value = "Nhà ở";
            worksheet.Cell(currentRow, 14).Value = "CH Officetel";
            worksheet.Cell(currentRow, 15).Value = "Chung cư tầng thấp";
            worksheet.Cell(currentRow, 16).Value = "Căn Shophouse";
            worksheet.Cell(currentRow, 17).Value = "CH thông thường";
            worksheet.Cell(currentRow, 18).Value = "CH Studio";
            worksheet.Cell(currentRow, 19).Value = "CH Shophouse";
            worksheet.Cell(currentRow, 20).Value = "CH Penthouse";
            worksheet.Cell(currentRow, 21).Value = "CH Duplex";
            worksheet.Cell(currentRow, 22).Value = "CH Sky Villa";
            worksheet.Cell(currentRow, 23).Value = "Biệt thự";
            worksheet.Cell(currentRow, 24).Value = "Villa";
            worksheet.Cell(currentRow, 25).Value = "Liền kề";
            worksheet.Cell(currentRow, 26).Value = "Nhà ở";
            worksheet.Cell(currentRow, 27).Value = "CH Officetel";
            worksheet.Cell(currentRow, 28).Value = "Chung cư tầng thấp";
            worksheet.Cell(currentRow, 29).Value = "Căn Shophouse";
            worksheet.Cell(currentRow, 30).Value = "Số lượng chưa bán";
            worksheet.Cell(currentRow, 31).Value = "Tỷ lệ hàng tồn";


            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 20;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 20;
            worksheet.Column("J").Width = 20;
            worksheet.Column("K").Width = 20;
            worksheet.Column("L").Width = 20;
            worksheet.Column("M").Width = 20;
            worksheet.Column("N").Width = 20;
            worksheet.Column("O").Width = 20;
            worksheet.Column("P").Width = 20;
            worksheet.Column("Q").Width = 20;
            worksheet.Column("R").Width = 20;
            worksheet.Column("S").Width = 20;
            worksheet.Column("T").Width = 20;
            worksheet.Column("U").Width = 20;
            worksheet.Column("V").Width = 20;
            worksheet.Column("W").Width = 20;
            worksheet.Column("X").Width = 20;
            worksheet.Column("Y").Width = 20;
            worksheet.Column("Z").Width = 20;
            worksheet.Column("AA").Width = 20;
            worksheet.Column("AB").Width = 20;
            worksheet.Column("AC").Width = 20;
            worksheet.Column("AD").Width = 20;
            worksheet.Column("AE").Width = 20;
            worksheet.Column("AE").Style.NumberFormat.Format = "0.00";
            worksheet.Column("AF").Width = 20;
            worksheet.Column("AG").Width = 20;

            foreach (var item in ListProductProjectOverview)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.Code;
                worksheet.Cell(currentRow, 3).Value = item.BuildingDensityType;
                worksheet.Cell(currentRow, 4).Value = item.CHThongThuong;
                worksheet.Cell(currentRow, 5).Value = item.CHStudio;
                worksheet.Cell(currentRow, 6).Value = item.CHShophouse;
                worksheet.Cell(currentRow, 7).Value = item.CHPenthouse;
                worksheet.Cell(currentRow, 8).Value = item.CHDuplex;
                worksheet.Cell(currentRow, 9).Value = item.CHSkyVilla;
                worksheet.Cell(currentRow, 10).Value = item.BietThu;
                worksheet.Cell(currentRow, 11).Value = item.Villa;
                worksheet.Cell(currentRow, 12).Value = item.LienKe;
                worksheet.Cell(currentRow, 13).Value = item.NhaO;
                worksheet.Cell(currentRow, 14).Value = item.CHOfficetel;
                worksheet.Cell(currentRow, 15).Value = item.ChungCuThapTang;
                worksheet.Cell(currentRow, 16).Value = item.CanShophouse;
                worksheet.Cell(currentRow, 17).Value = item.SellCHThongThuong;
                worksheet.Cell(currentRow, 18).Value = item.SellCHStudio;
                worksheet.Cell(currentRow, 19).Value = item.SellCHShophouse;
                worksheet.Cell(currentRow, 20).Value = item.SellCHPenthouse;
                worksheet.Cell(currentRow, 21).Value = item.SellCHDuplex;
                worksheet.Cell(currentRow, 22).Value = item.SellCHSkyVilla;
                worksheet.Cell(currentRow, 23).Value = item.SellBietThu;
                worksheet.Cell(currentRow, 24).Value = item.SellVilla;
                worksheet.Cell(currentRow, 25).Value = item.SellLienKe;
                worksheet.Cell(currentRow, 26).Value = item.SellNhaO;
                worksheet.Cell(currentRow, 27).Value = item.SellCHOfficetel;
                worksheet.Cell(currentRow, 28).Value = item.SellChungCuThapTang;
                worksheet.Cell(currentRow, 29).Value = item.SellCanShophouse;
                worksheet.Cell(currentRow, 30).Value = item.SoLuongChuaBan;
                worksheet.Cell(currentRow, 31).Value = item.TyLeHangTon;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        public async Task<ExportResultDto> SyntheticMoneyProject(DateTime? startDate, DateTime? endDate)
        {
            var ListProductProjectOverview = new List<RstListSyntheticMoneyProjectDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var result = new ExportResultDto();
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;

            var ProductProjectOverview = new List<RstSyntheticMoneyProjectDto>();


            if (tradingProviderId != null)
            {
                ProductProjectOverview = _rstExportExcelReportRepositories.SyntheticMoneyProjectTradingProvider(tradingProviderId, startDate, endDate).OrderByDescending(o => o.Code).ToList();
            }
            if (partnerId != null)
            {
                ProductProjectOverview = _rstExportExcelReportRepositories.SyntheticMoneyProjectPartner(partnerId, startDate, endDate).OrderByDescending(o => o.Code).ToList();
            }

            var listItem = ProductProjectOverview.GroupBy(o => new { o.Code, o.ClassifyType }).Select(o => new
            {
                Code = o.Key.Code,
                ClassifyType = o.Key.ClassifyType,
                total = o.Count( i=> i.Status >= RstProductItemStatus.DA_COC),
                paymentAmount = o.Where(i => i.Status == RstProductItemStatus.DA_COC).Sum(i => i.PaymentAmount)
            });

            List<Task> tasks = new();
            ConcurrentBag<RstListSyntheticMoneyProjectDto> listProductProjectOverviewCurrentbag = new();
            foreach (var item in listItem)
            {
                var ItemProductProjectOverview = new RstListSyntheticMoneyProjectDto();
                ItemProductProjectOverview.Code = item.Code;
                ItemProductProjectOverview.ClassifyType = RstClassifyType.GetClassifyTypeText(item.ClassifyType);
                ItemProductProjectOverview.Total = item.total;
                ItemProductProjectOverview.PaymentAmount = item.paymentAmount;
                listProductProjectOverviewCurrentbag.Add(ItemProductProjectOverview);
            };

            await Task.WhenAll(tasks);

            ListProductProjectOverview.AddRange(listProductProjectOverviewCurrentbag);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("RST");
            var currentRow = 2;
            var stt = 0;
            worksheet.Cell(1, 3).Value = "Đặt cọc";
            worksheet.Range("C1:E2").Style.Fill.BackgroundColor = XLColor.FromArgb(226, 239, 218);

            worksheet.Cell(1, 6).Value = "Số lượng hàng đã bán";
            worksheet.Range("F1:I2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 242, 204);

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Dự án";
            worksheet.Cell(currentRow, 3).Value = "Loại căn hộ";
            worksheet.Cell(currentRow, 4).Value = "Số lượng";
            worksheet.Cell(currentRow, 5).Value = "Số Tiền";
            worksheet.Cell(currentRow, 6).Value = "Số lượng";
            worksheet.Cell(currentRow, 7).Value = "GTHC";                // Giá bán căn hộ
            worksheet.Cell(currentRow, 8).Value = "Số tiền phải thu";
            worksheet.Cell(currentRow, 9).Value = "Số tiền còn lại phải thu";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 20;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 20;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 20;
            worksheet.Column("J").Width = 20;
            worksheet.Column("K").Width = 20;
            worksheet.Column("L").Width = 20;
            worksheet.Column("M").Width = 20;

            foreach (var item in ListProductProjectOverview)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.Code;
                worksheet.Cell(currentRow, 3).Value = item.ClassifyType;
                worksheet.Cell(currentRow, 4).Value = item.Total;
                worksheet.Cell(currentRow, 5).Value = item.PaymentAmount;

            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        public async Task<ExportResultDto> SyntheticTrading(DateTime? startDate, DateTime? endDate)
        {
            var ListProductProjectOverview = new List<RstListSyntheticTradingDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var result = new ExportResultDto();
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

            var syntheticTrading = new List<RstListSyntheticTradingDto>();

            // list data
            var synthetictradings = _rstExportExcelReportRepositories.SyntheticTrading(tradingProviderId, startDate, endDate).ToList();
            synthetictradings = synthetictradings.Where(r => tradingProviderIds.Contains(r.TradingProviderId) && (startDate != null && r.CreatedDate.Value.Date >= startDate.Value.Date) && (endDate != null && r.CreatedDate.Value.Date <= endDate.Value.Date)).ToList();

            List<Task> tasks = new();

            ConcurrentBag<RstListSyntheticTradingDto> listSynthetictradingsCurrentbag = new();
            foreach (var item in synthetictradings)
            {
                var ItemSynthetictradings = new RstListSyntheticTradingDto();
                ItemSynthetictradings.CreatedDate = item.CreatedDate;
                ItemSynthetictradings.ContractCode = item.ContractCode;
                ItemSynthetictradings.BuildingDensityType = ExcelDataUtils.RstBuildingDensityType(item.BuildingDensityType) + " " + item.BuildingDensityName;
                var cifCodeFind = _cifCodeEFRepository.FindByCifCode(item.CifCode);
                if(cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    var investorIden = _investorEFRepository.GetDefaultIdentification(cifCodeFind?.InvestorId ?? 0);
                    //var investorBankAccount = _investorBankAccountEFRepository.FindById(item?.InvestorBankAccId ?? 0);
                    //var bank = _bankEFRepository.FindById(investorBankAccount?.BankId ?? 0);
                    ItemSynthetictradings.Fullname = investorIden?.Fullname;
                    ItemSynthetictradings.IdNo = investorIden?.IdNo;
                    ItemSynthetictradings.IdType = investorIden?.IdType;
                    ItemSynthetictradings.PlaceOfResidence = investorIden?.PlaceOfResidence;
                    //ItemSynthetictradings.BankName = bank?.BankName;
                    //ItemSynthetictradings.OwnerBankAccount = investorBankAccount?.OwnerAccount;
                }
                else if(cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerEFRepository.FindById(cifCodeFind.BusinessCustomerId ?? 0);
                    ItemSynthetictradings.Fullname = businessCustomer?.Name;
                    ItemSynthetictradings.IdNo = businessCustomer?.TaxCode;
                    ItemSynthetictradings.IdType = DisplayType.TaxCode.MA_SO_THUE;
                }

                var saleInvestorInfo = _saleEFRepository.FindSaleByRefferCode(item.SaleReferralCode, true, item.TradingProviderId);
                var saleBusinessInfo = _saleEFRepository.FindSaleByRefferCode(item.SaleReferralCode, false, item.TradingProviderId);

                if (saleInvestorInfo != null && saleBusinessInfo == null)
                {
                    ItemSynthetictradings.SalerName = saleInvestorInfo?.Fullname;
                    ItemSynthetictradings.SaleDepartmentName = saleInvestorInfo?.DepartmentName;
                }
                else if (saleInvestorInfo == null && saleBusinessInfo != null)
                {
                    ItemSynthetictradings.SalerName = saleBusinessInfo?.Name;
                    ItemSynthetictradings.SaleDepartmentName = saleInvestorInfo?.DepartmentName;
                }

                ItemSynthetictradings.TranClassify = RstOrderPaymentsTranClassify.TranClassify(item.TranClassify);
                ItemSynthetictradings.PaymentType = RstPaymentType.PaymentType(item.PaymentType);
                ItemSynthetictradings.ClassifyType = RstClassifyType.GetClassifyTypeText(item.ClassifyType);
                ItemSynthetictradings.ProductItemStatus = ExcelDataUtils.StatusProductItem(item.ProductItemStatus);
                ItemSynthetictradings.CarpetArea = item.CarpetArea;
                ItemSynthetictradings.BuiltUpArea = item.BuiltUpArea;
                ItemSynthetictradings.Price = item.Price;
                ItemSynthetictradings.ProjectCode = item.ProjectCode;
                ItemSynthetictradings.CifCode = item.CifCode;
                ItemSynthetictradings.SaleReferralCode = item.SaleReferralCode;
                ItemSynthetictradings.PaymentAmount = item.PaymentAmount;
                ItemSynthetictradings.PayingCustomer = item.PaymentAmount / item.Price;
                listSynthetictradingsCurrentbag.Add(ItemSynthetictradings);
            };

            await Task.WhenAll(tasks);

            ListProductProjectOverview.AddRange(listSynthetictradingsCurrentbag);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("RST");
            var currentRow = 2;
            var stt = 0;
            worksheet.Cell(1, 6).Value = "Thông tin khách hàng";
            worksheet.Range("F1:M2").Style.Fill.BackgroundColor = XLColor.FromArgb(217, 225, 242);

            worksheet.Cell(1, 14).Value = "Thông tin giao dịch";
            worksheet.Range("N1:S2").Style.Fill.BackgroundColor = XLColor.FromArgb(226, 239, 218);

            worksheet.Cell(1, 20).Value = "Thông tin BĐS";
            worksheet.Range("T1:Y2").Style.Fill.BackgroundColor = XLColor.FromArgb(255, 242, 204);

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 4).Value = "Dự án";
            worksheet.Cell(currentRow, 5).Value = "Phân khu/Tòa";
            worksheet.Cell(currentRow, 6).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 7).Value = "Mã KH";
            worksheet.Cell(currentRow, 8).Value = "loại giấy tờ";
            worksheet.Cell(currentRow, 9).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 10).Value = "Địa chỉ thường trú";
            worksheet.Cell(currentRow, 11).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 12).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 13).Value = "Phòng ban";
            worksheet.Cell(currentRow, 14).Value = "Kiểu giao dịch";
            worksheet.Cell(currentRow, 15).Value = "Hình thức";
            worksheet.Cell(currentRow, 16).Value = "Tài khoản chuyển tiền KH(nếu có)";
            worksheet.Cell(currentRow, 17).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 18).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 19).Value = "Số tiền thu KH lũy kế";
            worksheet.Cell(currentRow, 20).Value = "Loại căn hộ";
            worksheet.Cell(currentRow, 21).Value = "Tình trạng căn hộ";
            worksheet.Cell(currentRow, 22).Value = "Diện tích thông thủy";
            worksheet.Cell(currentRow, 23).Value = "Diện tích tim tường";
            worksheet.Cell(currentRow, 24).Value = "Giá bán căn hộ";
            worksheet.Cell(currentRow, 25).Value = "%KH thanh toán";

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
            worksheet.Column("V").Width = 20;
            worksheet.Column("W").Width = 20;
            worksheet.Column("X").Width = 20;
            worksheet.Column("Y").Width = 20;

            foreach (var item in ListProductProjectOverview)
            {
                string payingCustomer = item.PayingCustomer?.ToString("F2");

                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.CreatedDate;
                worksheet.Cell(currentRow, 3).Value = item.ContractCode;
                worksheet.Cell(currentRow, 4).Value = item.ProjectCode;
                worksheet.Cell(currentRow, 5).Value = item.BuildingDensityType;
                worksheet.Cell(currentRow, 6).Value = item.Fullname;
                worksheet.Cell(currentRow, 7).Value = item.CifCode;
                worksheet.Cell(currentRow, 8).Value = item.IdType;
                worksheet.Cell(currentRow, 9).Value = item.IdNo;
                worksheet.Cell(currentRow, 10).Value = item.PlaceOfResidence;
                worksheet.Cell(currentRow, 11).Value = item.SaleReferralCode;
                worksheet.Cell(currentRow, 12).Value = item.SalerName;
                worksheet.Cell(currentRow, 13).Value = item.SaleDepartmentName;
                worksheet.Cell(currentRow, 14).Value = item.TranClassify;
                worksheet.Cell(currentRow, 15).Value = item.PaymentType;
                //worksheet.Cell(currentRow, 16).Value = item.CreatedDate;
                //worksheet.Cell(currentRow, 17).Value = item.CreatedDate;
                //worksheet.Cell(currentRow, 18).Value = item.CreatedDate;
                worksheet.Cell(currentRow, 19).Value = item.PaymentAmount;
                worksheet.Cell(currentRow, 20).Value = item.ClassifyType;
                worksheet.Cell(currentRow, 21).Value = item.ProductItemStatus;
                worksheet.Cell(currentRow, 22).Value = item.CarpetArea;
                worksheet.Cell(currentRow, 23).Value = item.BuiltUpArea;
                worksheet.Cell(currentRow, 24).Value = item.Price;
                worksheet.Cell(currentRow, 25).Value = decimal.Parse(payingCustomer) + "%";
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }
    }
}