using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities.Dto.ContractData;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EPIC.LoyaltyDomain.Implements
{
    public class LoyExportExcelReportServices : ILoyExportExcelReportServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LoyVoucherServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly SaleEFRepository _saleEFRepository;

        public LoyExportExcelReportServices(
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<LoyVoucherServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _saleEFRepository = new SaleEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Báo cáo danh sách yêu cầu đổi voucher
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto ReportConversionPoint(DateTime? startDate, DateTime? endDate)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var result = new ExportResultDto();
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            var conversionPointQuery = (from conversionPoint in _dbContext.LoyConversionPoints
                                        join conversionPointDetail in _dbContext.LoyConversionPointDetails on conversionPoint.Id equals conversionPointDetail.ConversionPointId
                                        join voucher in _dbContext.LoyVouchers on conversionPointDetail.VoucherId equals voucher.Id
                                        let pendingDate = _dbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.PENDING).OrderByDescending(x => x.Id).FirstOrDefault()
                                        let deliveryDate = _dbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.DELIVERY).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                                        let finishedDate = _dbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.FINISHED).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                                        join investor in _dbContext.Investors on conversionPoint.InvestorId equals investor.InvestorId
                                        let investorIdentification = _dbContext.InvestorIdentifications.Where(l => l.InvestorId == investor.InvestorId && l.Status == Status.ACTIVE && l.Deleted == YesNo.NO).OrderByDescending(x => x.IsDefault).FirstOrDefault()
                                        join cifCode in _dbContext.CifCodes on investor.InvestorId equals cifCode.InvestorId
                                        from pointInvestor in _dbContext.LoyPointInvestors.AsNoTracking().Where(x => x.InvestorId == investor.InvestorId && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                        from rank in _dbContext.LoyRanks.AsNoTracking().Where(x => x.PointStart <= pointInvestor.TotalPoint && pointInvestor.TotalPoint <= x.PointEnd && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE).DefaultIfEmpty()
                                            // Tìm kiếm quản lý đã quét mã khi tại tài khoản
                                        from managerIsInvestor in _dbContext.Investors.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                        from identificationManagerIsInvestor in _dbContext.InvestorIdentifications.Where(l => l.InvestorId == managerIsInvestor.InvestorId && l.Status == Status.ACTIVE && l.Deleted == YesNo.NO).OrderByDescending(x => x.IsDefault).Take(1).DefaultIfEmpty()
                                        from managerIsBusiness in _dbContext.BusinessCustomers.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                        from sale in _dbContext.Sales.Where(x => ((managerIsInvestor != null && managerIsInvestor.InvestorId == x.InvestorId) || (managerIsBusiness != null && managerIsBusiness.BusinessCustomerId == x.BusinessCustomerId)) && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                        from departmentSale in _dbContext.DepartmentSales.Where(x => x.SaleId == sale.SaleId && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO).Take(1).DefaultIfEmpty()

                                        where conversionPoint.TradingProviderId == tradingProviderId && voucher.Deleted == YesNo.NO && conversionPointDetail.Deleted == YesNo.NO && conversionPoint.Deleted == YesNo.NO
                                        && cifCode.Deleted == YesNo.NO
                                        && (startDate == null || (conversionPoint.CreatedDate != null && startDate <= conversionPoint.CreatedDate.Value.Date))
                                        && (endDate == null || (conversionPoint.CreatedDate != null && endDate >= conversionPoint.CreatedDate.Value.Date))
                                        orderby conversionPoint.Id
                                        select new
                                        {
                                            // Mã khách hàng
                                            CifCode = cifCode.CifCode,
                                            // Số giấy tờ
                                            IdNo = investorIdentification.IdNo,
                                            // Tên khách hàng
                                            Fullname = investorIdentification.Fullname,
                                            // Tên Hạng
                                            RankName = rank.Name,
                                            // Số điểm tích lũy
                                            TotalPoint = pointInvestor.TotalPoint,
                                            // Số điểm hiện tại
                                            CurrentPoint = conversionPoint.CurrentPoint,
                                            // Số điểm quy đổi
                                            TotalConversionPoint = conversionPointDetail.TotalConversionPoint,
                                            // Tên voucher
                                            VoucherName = voucher.Name,
                                            // Lô
                                            VoucherCode = voucher.Code,
                                            // Loại voucher
                                            VoucherType = LoyVoucherTypes.VoucherTypeName(voucher.VoucherType),
                                            // Số lượng quy đổi
                                            ConversionPointDetailQuantity = conversionPointDetail.Quantity,
                                            // Hình thức
                                            Source = conversionPoint.Source == LoySource.OFFLINE ? "Offline" : "Online",
                                            // Tên phòng ban 
                                            DepartmentName = _dbContext.Departments.FirstOrDefault(p => p.DepartmentId == departmentSale.DepartmentId && p.Deleted == YesNo.NO).DepartmentName,
                                            // Mã giới thiệu quét
                                            ReferralCodeManager = investor.ReferralCode,
                                            // Tên của mã giới thiệu
                                            ManagerName = identificationManagerIsInvestor.Fullname ?? managerIsBusiness.Name,
                                            // Trạng thái
                                            Status = LoyConversionPointStatus.ConversionPointStatusName(conversionPoint.Status),
                                            // Ngày tạo yêu cầu
                                            CreatedDate = conversionPoint.CreatedDate,
                                            // Người tạo
                                            CreatedBy = conversionPoint.Source == LoySource.OFFLINE ? conversionPoint.CreatedBy : "Khách hàng",
                                            // Thời gian tiếp nhận yêu cầu
                                            PendingDate = pendingDate.CreatedDate,
                                            // Người duyệt yêu cầu
                                            PendingBy = pendingDate.CreatedBy,
                                            //Thời gian bàn giao
                                            DeliveryDate = deliveryDate,
                                            // Thời gian hoàn thành
                                            FinishedDate = finishedDate,
                                        });

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("LOY");
            var currentRow = 2;
            var stt = 0;
            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 3).Value = "Giấy tờ tùy thân";
            worksheet.Cell(currentRow, 4).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Tên hạng";
            worksheet.Cell(currentRow, 6).Value = "Số điểm tích lũy";
            worksheet.Cell(currentRow, 7).Value = "Số điểm hiện tại";
            worksheet.Cell(currentRow, 8).Value = "Số điểm quy đổi";
            worksheet.Cell(currentRow, 9).Value = "Loại voucher";
            worksheet.Cell(currentRow, 10).Value = "Tên voucher";
            worksheet.Cell(currentRow, 11).Value = "Mã lô voucher";
            worksheet.Cell(currentRow, 12).Value = "Số lượng";
            worksheet.Cell(currentRow, 13).Value = "Hình thức";
            worksheet.Cell(currentRow, 14).Value = "Phòng ban";
            worksheet.Cell(currentRow, 15).Value = "Mã giới thiệu quản lý cấp 1";
            worksheet.Cell(currentRow, 16).Value = "Tên giới thiệu quản lý cấp 1";
            worksheet.Cell(currentRow, 17).Value = "Trạng thái";
            worksheet.Cell(currentRow, 18).Value = "Ngày tạo yêu cầu";
            worksheet.Cell(currentRow, 19).Value = "Người tạo yêu cầu";
            worksheet.Cell(currentRow, 20).Value = "Thời gian tiếp nhận yêu cầu";
            worksheet.Cell(currentRow, 21).Value = "Người duyệt yêu cầu";
            worksheet.Cell(currentRow, 22).Value = "Thời gian bàn giao";
            worksheet.Cell(currentRow, 23).Value = "Thời gian hoàn thành";


            worksheet.Column("A").Width = 5;
            worksheet.Column("B").Width = 15;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 20;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 20;
            worksheet.Column("J").Width = 30;
            worksheet.Column("K").Width = 20;
            worksheet.Column("L").Width = 10;
            worksheet.Column("M").Width = 10;
            worksheet.Column("N").Width = 20;
            worksheet.Column("O").Width = 30;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 20;
            worksheet.Column("R").Width = 20;
            worksheet.Column("S").Width = 20;
            worksheet.Column("T").Width = 30;
            worksheet.Column("U").Width = 20;
            worksheet.Column("V").Width = 20;
            worksheet.Column("W").Width = 20;

            foreach (var item in conversionPointQuery)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item.CifCode;
                worksheet.Cell(currentRow, 3).Value = "'" + item.IdNo;
                worksheet.Cell(currentRow, 4).Value = item.Fullname;
                worksheet.Cell(currentRow, 5).Value = item.RankName;
                worksheet.Cell(currentRow, 6).Value = item.TotalPoint;
                worksheet.Cell(currentRow, 7).Value = item.CurrentPoint;
                worksheet.Cell(currentRow, 8).Value = item.TotalConversionPoint;
                worksheet.Cell(currentRow, 9).Value = item.VoucherType;
                worksheet.Cell(currentRow, 10).Value = item.VoucherName;
                worksheet.Cell(currentRow, 11).Value = "'" + item.VoucherCode;
                worksheet.Cell(currentRow, 12).Value = item.ConversionPointDetailQuantity;
                worksheet.Cell(currentRow, 13).Value = item.Source;
                worksheet.Cell(currentRow, 14).Value = item.DepartmentName;
                worksheet.Cell(currentRow, 15).Value = "'" + item.ReferralCodeManager;
                worksheet.Cell(currentRow, 16).Value = item.ManagerName;
                worksheet.Cell(currentRow, 17).Value = item.Status;
                worksheet.Cell(currentRow, 18).Value = item.CreatedDate;
                worksheet.Cell(currentRow, 19).Value = item.CreatedBy;
                worksheet.Cell(currentRow, 20).Value = item.PendingDate;
                worksheet.Cell(currentRow, 21).Value = item.PendingBy;
                worksheet.Cell(currentRow, 22).Value = item.DeliveryDate;
                worksheet.Cell(currentRow, 23).Value = item.FinishedDate;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo xuất nhập tồn voucher
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto ReportVoucher(DateTime? startDate, DateTime? endDate)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var result = new ExportResultDto();
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            var voucherQuery = (from voucher in _dbContext.LoyVouchers
                                let quantityConversion = _dbContext.LoyConversionPointDetails.Where(cpd => cpd.VoucherId == voucher.Id && cpd.Deleted == YesNo.NO
                                    && _dbContext.LoyConversionPoints.Any(cp => cp.TradingProviderId == tradingProviderId && cp.Id == cpd.ConversionPointId && cp.Status == LoyConversionPointStatus.FINISHED)).Sum(x => x.Quantity)
                                where voucher.TradingProviderId == tradingProviderId && voucher.Deleted == YesNo.NO
                                && (startDate == null || (voucher.CreatedDate != null && startDate <= voucher.CreatedDate.Value.Date))
                                && (endDate == null || (voucher.CreatedDate != null && endDate >= voucher.CreatedDate.Value.Date))
                                orderby voucher.Id
                                select new
                                {
                                    // Tên voucher
                                    VoucherName = voucher.Name,
                                    // Lô
                                    VoucherCode = voucher.Code,
                                    // Loại hình voucher
                                    VoucherType = LoyVoucherTypes.VoucherTypeName(voucher.VoucherType),
                                    //Loại voucher
                                    UseType = LoyVoucherUseTypes.VoucherUseTypeName(voucher.UseType),
                                    //Ngày hiệu lực
                                    StartDate = voucher.StartDate,
                                    //Ngày hết hạn
                                    ExpiredDate = voucher.ExpiredDate,
                                    //Giá trị voucher
                                    Value = voucher.Value,
                                    // Số lượng nhập/phát hành
                                    PublishNum = voucher.PublishNum,
                                    //Ngày nhập
                                    BatchEntryDate = voucher.BatchEntryDate,
                                    // Số lượng quy đổi
                                    QuantityConversion = quantityConversion,
                                    // Số lượng tồn kho
                                    QuantityInventory = voucher.PublishNum - quantityConversion,
                                    //Người cài đặt
                                    CreatedBy = voucher.CreatedBy,

                                });

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("LOY");
            var currentRow = 1;
            var stt = 0;
            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã Lô";
            worksheet.Cell(currentRow, 3).Value = "Tên voucher";
            worksheet.Cell(currentRow, 4).Value = "Loại hình";
            worksheet.Cell(currentRow, 5).Value = "Loại voucher";
            worksheet.Cell(currentRow, 6).Value = "Ngày hiệu lực";
            worksheet.Cell(currentRow, 7).Value = "Ngày hết hạn";
            worksheet.Cell(currentRow, 8).Value = "Giá trị";
            worksheet.Cell(currentRow, 9).Value = "Số lượng nhập";
            worksheet.Cell(currentRow, 10).Value = "Ngày nhập";
            worksheet.Cell(currentRow, 11).Value = "Số lượng cấp phát";
            worksheet.Cell(currentRow, 12).Value = "Số lượng tồn kho";
            worksheet.Cell(currentRow, 13).Value = "Người cài đặt";


            worksheet.Column("A").Width = 5;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 30;
            worksheet.Column("D").Width = 20;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 20;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 15;
            worksheet.Column("J").Width = 20;
            worksheet.Column("K").Width = 20;
            worksheet.Column("L").Width = 15;
            worksheet.Column("M").Width = 20;
            worksheet.Column("N").Width = 20;
            worksheet.Column("O").Width = 30;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 20;
            worksheet.Column("R").Width = 20;
            worksheet.Column("S").Width = 20;
            worksheet.Column("T").Width = 30;
            worksheet.Column("U").Width = 20;
            worksheet.Column("V").Width = 20;
            worksheet.Column("W").Width = 20;

            foreach (var item in voucherQuery)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item.VoucherCode;
                worksheet.Cell(currentRow, 3).Value = item.VoucherName;
                worksheet.Cell(currentRow, 4).Value = item.VoucherType;
                worksheet.Cell(currentRow, 5).Value = item.UseType;
                worksheet.Cell(currentRow, 6).Value = item.StartDate;
                worksheet.Cell(currentRow, 7).Value = item.ExpiredDate;
                worksheet.Cell(currentRow, 8).Value = item.Value;
                worksheet.Cell(currentRow, 9).Value = item.PublishNum;
                worksheet.Cell(currentRow, 10).Value = item.BatchEntryDate;
                worksheet.Cell(currentRow, 11).Value = item.QuantityConversion;
                worksheet.Cell(currentRow, 12).Value = item.QuantityInventory;
                worksheet.Cell(currentRow, 13).Value = item.CreatedBy;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }
    }
}
