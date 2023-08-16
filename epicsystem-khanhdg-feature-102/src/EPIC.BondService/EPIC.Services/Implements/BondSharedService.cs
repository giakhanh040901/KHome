using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.Order;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace EPIC.BondDomain.Implements
{
    public class BondSharedService : IBondSharedService
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BondInfoRepository _bondInfoRepository;
        private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        private readonly BondCalendarRepository _calendarRepository;
        private readonly BondPartnerCalendarRepository _partnerCalendarRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;

        public BondSharedService(ILogger<BondSharedService> logger,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _bondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            _calendarRepository = new BondCalendarRepository(_connectionString, _logger);
            _partnerCalendarRepository = new BondPartnerCalendarRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
        }

        public DateTime NextWorkDay(DateTime ngayDangXet, int tradingProviderId = 0, bool isClose = true)
        {
            DateTime ngayLamViecTiepTheo = ngayDangXet.Date;//new(ngayDangXet.Year, ngayDangXet.Month, ngayDangXet.Day);
            if (tradingProviderId == 0)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            while (true)
            {
                if (_calendarRepository.CheckHoliday(ngayLamViecTiepTheo, tradingProviderId, isClose))
                {
                    ngayLamViecTiepTheo = ngayLamViecTiepTheo.AddDays(1);
                }
                else
                {
                    return ngayLamViecTiepTheo;
                }
            }
        }

        public DateTime WorkDay(DateTime ngayDangXet, int soNgayLamViec, int tradingProviderId = 0)
        {
            DateTime ngayLamViecTruoc = ngayDangXet.Date;//new(ngayDangXet.Year, ngayDangXet.Month, ngayDangXet.Day);
            if (tradingProviderId == 0)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            int countEnd = Math.Abs(soNgayLamViec);
            int count = 0;
            while (count == countEnd)
            {
                if (!_calendarRepository.CheckHoliday(ngayLamViecTruoc, tradingProviderId)) //không phải ngày nghỉ + 1
                {
                    count++;
                }
                if (soNgayLamViec < 0)
                {
                    ngayLamViecTruoc = ngayLamViecTruoc.AddDays(-1);
                }
                else
                {
                    ngayLamViecTruoc = ngayLamViecTruoc.AddDays(1);
                }
            }
            return ngayLamViecTruoc;
        }

        public DateTime NextWorkDayPartner(DateTime ngayDangXet, int partnerId = 0)
        {
            DateTime ngayLamViecTiepTheo = ngayDangXet.Date; //new(ngayDangXet.Year, ngayDangXet.Month, ngayDangXet.Day);
            if (partnerId == 0)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            while (true)
            {
                if (_partnerCalendarRepository.CheckHoliday(ngayLamViecTiepTheo, partnerId))
                {
                    ngayLamViecTiepTheo = ngayLamViecTiepTheo.AddDays(1);
                }
                else
                {
                    return ngayLamViecTiepTheo;
                }
            }
        }

        public DateTime WorkDayPartner(DateTime ngayDangXet, int soNgayLamViec, int partnerId = 0)
        {
            DateTime ngayLamViecTruoc = ngayDangXet.Date; //new(ngayDangXet.Year, ngayDangXet.Month, ngayDangXet.Day);
            int countEnd = Math.Abs(soNgayLamViec);
            int count = 0;
            if (partnerId == 0)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            while (count == countEnd)
            {
                if (!_calendarRepository.CheckHoliday(ngayLamViecTruoc, partnerId)) //không phải ngày nghỉ + 1
                {
                    count++;
                }
                if (soNgayLamViec < 0)
                {
                    ngayLamViecTruoc = ngayLamViecTruoc.AddDays(-1);
                }
                else
                {
                    ngayLamViecTruoc = ngayLamViecTruoc.AddDays(1);
                }
            }
            return ngayLamViecTruoc;
        }

        public List<CouponInfoDto> GetListCoupon(int bondInfoId, int bondSecondaryId, decimal soTienDauTu, DateTime startDate, DateTime endDate, decimal incomeTax = 0, int tradingProviderId = 0)
        {
            var result = new List<CouponInfoDto>();
            var bondInfo = _bondInfoRepository.FindBondInfoById(bondInfoId);

            //Ngày phát hành
            DateTime ngayPhatHanh = bondInfo.IssueDate?.Date ?? new DateTime().Date;

            int soKyTraTraiTuc = bondInfo.BondPeriod;
            
            //Ngày đáo hạn
            DateTime ngayDaoHanLo = ngayPhatHanh.AddDays(soKyTraTraiTuc);

            if (bondInfo.BondPeriodUnit == PeriodUnit.YEAR)
            {
                ngayDaoHanLo = ngayPhatHanh.AddYears(soKyTraTraiTuc);
            }
            else if (bondInfo.BondPeriodUnit == PeriodUnit.MONTH)
            {
                ngayDaoHanLo = ngayPhatHanh.AddMonths(soKyTraTraiTuc);
            }

            int soNgayChotQuyen = bondInfo.NumberClosePer;

            int soKyHan = bondInfo.InterestPeriod ?? 0;

            decimal tySuatTraiTuc = (bondInfo.InterestRate) / 100;

            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId);
            if (tradingProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin đại lý: {tradingProviderId}"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            }
            
            //bỏ qua ngày nghỉ lễ
            ngayDaoHanLo = NextWorkDayPartner(ngayDaoHanLo, bondInfo.PartnerId);

            //List chứa thời gian thực mỗi kỳ
            List<DatePeriodDto> thoiGianTraTraiTuc = new();

            if (bondInfo.InterestRateType == InterestTypes.DINH_KY) //trả định kỳ
            {
                DateTime ngayDauKyHienTai = ngayPhatHanh;
                //Tìm thời gian thực
                while (true)
                {
                    DateTime ngayCuoiKy = ngayDauKyHienTai.AddDays(soKyHan);
                    if (bondInfo.InterestPeriodUnit == PeriodUnit.MONTH)
                    {
                        ngayCuoiKy = ngayDauKyHienTai.AddMonths(soKyHan);
                    }
                    else if (bondInfo.InterestPeriodUnit == PeriodUnit.YEAR)
                    {
                        ngayCuoiKy = ngayDauKyHienTai.AddYears(soKyHan);
                    }

                    if (ngayCuoiKy > ngayDaoHanLo)
                    {
                        break;
                    }

                    //Chuyển đến kỳ tiếp theo
                    ngayDauKyHienTai = ngayCuoiKy;

                    //bỏ qua ngày nghỉ lễ
                    DateTime ngayLamViecThucTe = NextWorkDayPartner(ngayCuoiKy, bondInfo.PartnerId);
                    DateTime ngayChotQuyenThucTe = WorkDayPartner(ngayLamViecThucTe, -1 * soNgayChotQuyen, bondInfo.PartnerId);

                    thoiGianTraTraiTuc.Add(new DatePeriodDto
                    {
                        ClosePerDate = ngayChotQuyenThucTe,
                        PayDate = ngayLamViecThucTe
                    });
                };

                //ngày kỳ cuối chưa trùng ngày đáo hạn
                if (thoiGianTraTraiTuc.Count > 0 && thoiGianTraTraiTuc[^1].PayDate < ngayDaoHanLo)
                {
                    thoiGianTraTraiTuc[^1].PayDate = ngayDaoHanLo;
                    thoiGianTraTraiTuc[^1].ClosePerDate = WorkDayPartner(ngayDaoHanLo, -1 * soNgayChotQuyen, bondInfo.PartnerId);
                }
            }
            else //trả cuối kỳ
            {
                DateTime ngayChotQuyenCuoiKy = WorkDayPartner(ngayDaoHanLo, -1 * soNgayChotQuyen, bondInfo.PartnerId);
                thoiGianTraTraiTuc.Add(new DatePeriodDto
                {
                    ClosePerDate = ngayChotQuyenCuoiKy,
                    PayDate = ngayDaoHanLo
                });
            }

            var soLuongDonGia = CalculateQuantityAndUnitPrice(soTienDauTu, bondSecondaryId, startDate, tradingProviderId);

            //tính trái tức
            for (int i = 0; i < thoiGianTraTraiTuc.Count; i++)
            {
                //không chứa ngày chốt quyền thì bỏ qua
                if (!(startDate <= thoiGianTraTraiTuc[i].ClosePerDate && endDate >= thoiGianTraTraiTuc[i].ClosePerDate))
                {
                    continue;
                }

                int soNgay;
                if (i == 0) //nếu là kỳ trả đầu tiên
                {
                    soNgay = (thoiGianTraTraiTuc[i].PayDate - ngayPhatHanh).Days;
                }
                else
                {
                    soNgay = (thoiGianTraTraiTuc[i].PayDate - thoiGianTraTraiTuc[i - 1].PayDate).Days;
                }

                decimal traiTucKyNay = (soLuongDonGia.Quantity * (bondInfo.ParValue)) * tySuatTraiTuc * soNgay / 365;

                decimal thue = traiTucKyNay * incomeTax;
                decimal tongTienThucNhan = traiTucKyNay - thue;

                result.Add(new CouponInfoDto
                {
                    ReceivePeriod = i + 1,
                    ClosePerDate = thoiGianTraTraiTuc[i].ClosePerDate,
                    PayDate = thoiGianTraTraiTuc[i].PayDate,
                    CouponRate = tySuatTraiTuc,
                    Coupon = Math.Round(traiTucKyNay),
                    Tax = Math.Round(thue),
                    ActuallyCoupon = Math.Round(tongTienThucNhan),
                    Status = DateTime.Now >= thoiGianTraTraiTuc[i].PayDate ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                });
            }
            return result;
        }

        public DateTime CalculateDueDate(BondPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, bool isClose = true)
        {
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = bondPolicyDetail.PeriodQuantity;
            //Ngày đáo hạn
            DateTime ngayDaoHan = ngayBatDauTinhLai.Date;
            if (bondPolicyDetail.InterestDays != null) //nếu có cài ngày chính xác
            {
                ngayDaoHan = ngayDaoHan.AddDays(bondPolicyDetail.InterestDays.Value);
            }
            else //không cài ngày chính xác
            {
                if (bondPolicyDetail.PeriodType == PeriodUnit.DAY)
                {
                    ngayDaoHan = ngayDaoHan.AddDays(soKyDaoHan);
                }
                else if (bondPolicyDetail.PeriodType == PeriodUnit.MONTH)
                {
                    ngayDaoHan = ngayDaoHan.AddMonths(soKyDaoHan);
                }
                else if (bondPolicyDetail.PeriodType == PeriodUnit.YEAR)
                {
                    ngayDaoHan = ngayDaoHan.AddYears(soKyDaoHan);
                }
            }
            ngayDaoHan = NextWorkDay(ngayDaoHan, bondPolicyDetail.TradingProviderId, isClose); //kiểm tra nếu trùng ngày nghỉ thì cộng lên

            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondPolicyDetail.SecondaryId, null);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy bán theo kỳ hạn: {bondPolicyDetail.SecondaryId}"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }

            var bondInfo = _bondInfoRepository.FindById(bondSecondary.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy lô: {bondSecondary.BondId}"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }

            //nếu ngày đáo hạn vượt quá ngày đáo hạn của lô gán về lô
            if (ngayDaoHan > bondInfo.DueDate && bondInfo.DueDate != null)
            {
                ngayDaoHan = bondInfo.DueDate.Value;
            }
            return ngayDaoHan;
        }

        /// <summary>
        /// Hàm lấy thời gian thực
        /// </summary>
        /// <param name="ngayDauKy"></param>
        /// <param name="ngayDaoHan"></param>
        /// <returns></returns>
        public List<DateTime> GetListRealTime(BondPolicyDetail policyDetail, DateTime ngayDauKy, DateTime ngayDaoHan)
        {
            //Sau bao lâu trả lãi một lần
            int soKyTra = policyDetail.InterestPeriodQuantity ?? 0;

            List<DateTime> thoiGianThuc = new List<DateTime>();
            DateTime ngayCuoiKy = ngayDauKy;
            int index = 0;
            while (ngayDauKy <= ngayDaoHan)
            {
                index++;
                ngayCuoiKy = ngayDauKy.AddDays(soKyTra * index);
                if (policyDetail.InterestPeriodType == PeriodUnit.MONTH)
                {
                    ngayCuoiKy = ngayDauKy.AddMonths(soKyTra * index);
                }
                else if (policyDetail.InterestPeriodType == PeriodUnit.YEAR)
                {
                    ngayCuoiKy = ngayDauKy.AddYears(soKyTra * index);
                }

                var ngayLamViec = NextWorkDay(ngayCuoiKy, policyDetail.TradingProviderId);

                //nếu ngày cuối của kỳ vượt quá ngày đáo hạn
                if (ngayLamViec > ngayDaoHan)
                {
                    break;
                }

                if (thoiGianThuc.Count > 1 && ngayLamViec == thoiGianThuc[^1]) //trường hợp cộng thừa 1 kỳ
                {
                    break;
                }

                thoiGianThuc.Add(ngayLamViec);
            };

            //nếu kỳ cuối chưa tới ngày đáo hạn
            if (thoiGianThuc.Count > 0 && thoiGianThuc[^1] < ngayDaoHan)
            {
                thoiGianThuc[^1] = ngayDaoHan;
            }

            return thoiGianThuc;
        }

        /// <summary>
        /// Tính danh sách lợi tức tùy theo định kỳ hay cuối kỳ của kỳ hạn
        /// </summary>
        /// <param name="bondPolicyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="ngayDaoHan"></param>
        /// <param name="soTienDauTu"></param>
        /// <param name="tySuatLoiTuc"></param>
        /// <param name="thueLoiNhuan"></param>
        /// <param name="tongTienTraiTuc"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<ProfitInfoDto> GetListProfit(BondPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, DateTime ngayDaoHan,
            decimal soTienDauTu, decimal tySuatLoiTuc, decimal thueLoiNhuan, decimal tongTienTraiTuc, ProfitAndInterestDto result)
        {
            //lợi tức
            List<ProfitInfoDto> dsLoiTuc = new();

            if (bondPolicyDetail.InterestType == InterestTypes.DINH_KY) //trả định kỳ
            {
                //Sau bao lâu trả lãi một lần
                int soKyTraTraiTuc = bondPolicyDetail.InterestPeriodQuantity ?? 0;

                DateTime ngayDauKy = ngayBatDauTinhLai.Date;
                //tính danh sách kỳ trả lợi tức
                List<DateTime> thoiGianThuc = GetListRealTime(bondPolicyDetail, ngayDauKy, ngayDaoHan);

                decimal tienTamUng = 0;

                //ít nhất 1 kỳ
                for (int i = 0; i < thoiGianThuc.Count; i++)
                {
                    int soNgay;
                    if (i == 0) //kỳ trả đầu tiên
                    {
                        soNgay = (thoiGianThuc[i] - ngayBatDauTinhLai).Days;
                    }
                    else
                    {
                        soNgay = (thoiGianThuc[i] - thoiGianThuc[i - 1]).Days;
                    }

                    decimal tongTienThucNhan = soTienDauTu * tySuatLoiTuc * soNgay / 365;
                    decimal loiTucKyNay = tongTienThucNhan / (1 - thueLoiNhuan);
                    decimal thue = loiTucKyNay - tongTienThucNhan;

                    if (i < thoiGianThuc.Count - 1) //bỏ không cộng kỳ cuối
                    {
                        tienTamUng += Math.Round(tongTienThucNhan);
                    }

                    if (i == thoiGianThuc.Count - 1 || (thoiGianThuc.Count == 1 && i == 0)) //là kỳ cuối
                    {
                        //tính lợi tức cả kỳ
                        decimal loiTucCaKyThucTe = soTienDauTu * tySuatLoiTuc * (ngayDaoHan - ngayBatDauTinhLai).Days / 365;
                        decimal loiTucCaKy = loiTucCaKyThucTe / (1 - thueLoiNhuan);
                        decimal thueCaKy = loiTucCaKy - loiTucCaKyThucTe;

                        result.AdvancePayment = Math.Round(tienTamUng);
                        result.AllProfit = Math.Round(loiTucCaKy);
                        result.TaxProfit = Math.Round(thueCaKy);
                        result.ActuallyProfit = Math.Round(loiTucCaKyThucTe);

                        //số tiền kỳ cuối nhận được
                        tongTienThucNhan = soTienDauTu + loiTucCaKyThucTe - tongTienTraiTuc - tienTamUng; //số tiền đầu tư + lợi tức cả kỳ thực tế - trái tức - tiền tạm ứng (tiền nhận trong kỳ)
                    }

                    dsLoiTuc.Add(new ProfitInfoDto
                    {
                        ReceivePeriod = dsLoiTuc.Count + 1,
                        ProfitRate = tySuatLoiTuc,
                        PayDate = thoiGianThuc[i],
                        Profit = Math.Round(loiTucKyNay),
                        Tax = Math.Round(thue),
                        ActuallyProfit = Math.Round(tongTienThucNhan),
                        NumberOfDays = soNgay,
                        Status = DateTime.Now >= thoiGianThuc[i] ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                    });
                }
            }
            else if (bondPolicyDetail.InterestType == InterestTypes.CUOI_KY) //trả cuối kỳ
            {
                int soNgay = ngayDaoHan.Subtract(ngayBatDauTinhLai).Days;
                //tính kiểu mới
                decimal loiTucThucNhan = soTienDauTu * tySuatLoiTuc * soNgay / 365;
                decimal loiTuc = loiTucThucNhan / (1 - thueLoiNhuan);
                decimal thue = loiTuc - loiTucThucNhan;

                decimal tongTienThucNhan = soTienDauTu + loiTucThucNhan;

                result.AllProfit = Math.Round(loiTuc);
                result.TaxProfit = Math.Round(thue);
                result.ActuallyProfit = Math.Round(loiTucThucNhan);

                dsLoiTuc.Add(new ProfitInfoDto
                {
                    ReceivePeriod = dsLoiTuc.Count + 1,
                    ProfitRate = tySuatLoiTuc,
                    PayDate = ngayDaoHan,
                    Profit = Math.Round(loiTuc),
                    Tax = Math.Round(thue),
                    ActuallyProfit = Math.Round(tongTienThucNhan),
                    NumberOfDays = soNgay,
                    Status = DateTime.Now >= ngayDaoHan ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                });
            }
            return dsLoiTuc;
        }

        public ProfitAndInterestDto CalculateListInterest(BondInfo bondInfo, BondPolicy bondPolicy,
            BondPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan)
        {
            var result = new ProfitAndInterestDto();
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = bondPolicyDetail.PeriodQuantity;

            ngayBatDauTinhLai = NextWorkDay(ngayBatDauTinhLai, bondPolicyDetail.TradingProviderId);

            //Ngày đáo hạn
            DateTime ngayDaoHan = CalculateDueDate(bondPolicyDetail, ngayBatDauTinhLai);

            //Tỷ suất lợi tức
            decimal tySuatLoiTuc = (bondPolicyDetail.Profit) / 100;

            //thuế lợi nhuận, thuế thu nhập cá nhân
            decimal thueLoiNhuan = 0;
            if (khachCaNhan)
            {
                thueLoiNhuan = (bondPolicy.IncomeTax) / 100;
            }

            decimal tongTienTraiTuc = 0;

            //lợi tức
            List<ProfitInfoDto> dsLoiTuc = new();
            List<CouponInfoDto> dsTraiTuc = new();

            if (bondPolicy.Classify == BondPolicyClassify.PNOTE) //PNOTE không tính trái tức
            {
                dsLoiTuc = GetListProfit(bondPolicyDetail, ngayBatDauTinhLai, ngayDaoHan, soTienDauTu, tySuatLoiTuc, thueLoiNhuan, tongTienTraiTuc, result);
            }
            else if (bondPolicy.Classify == BondPolicyClassify.PRO || bondPolicy.Classify == BondPolicyClassify.PROA) //PRO hoặc PRO A
            {
                dsTraiTuc = GetListCoupon(bondInfo.Id, bondPolicy.SecondaryId, soTienDauTu, ngayBatDauTinhLai, ngayDaoHan, thueLoiNhuan, bondPolicyDetail.TradingProviderId);
                if (dsTraiTuc.Count > 0)
                {
                    tongTienTraiTuc = dsTraiTuc.Sum(o => o.ActuallyCoupon);
                }
                dsLoiTuc = GetListProfit(bondPolicyDetail, ngayBatDauTinhLai, ngayDaoHan, soTienDauTu, tySuatLoiTuc, thueLoiNhuan, tongTienTraiTuc, result);
            }

            result.ProfitInfo = dsLoiTuc;
            result.CouponInfo = dsTraiTuc;
            return result;
        }

        public CalculateProfitDto CalculateProfit(BondPolicy bondPolicy,
            BondPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan)
        {
            ngayBatDauTinhLai = NextWorkDay(ngayBatDauTinhLai.Date, bondPolicyDetail.TradingProviderId);

            //Ngày đáo hạn
            DateTime ngayDaoHan = CalculateDueDate(bondPolicyDetail, ngayBatDauTinhLai.Date);

            //Tỷ suất lợi tức
            decimal tySuatLoiTuc = (bondPolicyDetail.Profit) / 100;

            //thuế lợi nhuận, thuế thu nhập cá nhân
            decimal thueLoiNhuan = 0;
            if (khachCaNhan)
            {
                thueLoiNhuan = (bondPolicy.IncomeTax) / 100;
            }

            //tính lợi tức cả kỳ
            decimal loiTucCaKyThucTe = soTienDauTu * tySuatLoiTuc * (ngayDaoHan - ngayBatDauTinhLai.Date).Days / 365;
            decimal loiTucCaKy = loiTucCaKyThucTe / (1 - thueLoiNhuan);
            decimal thueCaKy = loiTucCaKyThucTe - loiTucCaKy;

            return new CalculateProfitDto
            {
                Profit = Math.Round(loiTucCaKy),
                Tax = Math.Round(thueCaKy),
                ActuallyProfit = Math.Round(loiTucCaKyThucTe)
            };
        }

        /// <summary>
        /// Tính ngày trả trái tức
        /// </summary>
        /// <param name="ngayPhatHanh"></param>
        /// <param name="ngayDaoHan"></param>
        /// <param name="kyHanTraTraiTuc"></param>
        /// <param name="donViKyHan"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<DateTime> GetRealTimeCopon(DateTime ngayPhatHanh, DateTime ngayDaoHan, int kyHanTraTraiTuc, string donViKyHan, int partnerId)
        {
            DateTime ngayCuoiKy = ngayPhatHanh;
            List<DateTime> thoiGianThuc = new();
            //Tìm thời gian thực
            int index = 0;
            while (ngayCuoiKy <= ngayDaoHan)
            {
                index++;
                ngayCuoiKy = ngayPhatHanh.AddDays(kyHanTraTraiTuc * index);
                if (donViKyHan == PeriodUnit.MONTH)
                {
                    ngayCuoiKy = ngayPhatHanh.AddMonths(kyHanTraTraiTuc * index);
                }
                else if (donViKyHan == PeriodUnit.YEAR)
                {
                    ngayCuoiKy = ngayPhatHanh.AddYears(kyHanTraTraiTuc * index);
                }

                var ngayLamViec = NextWorkDayPartner(ngayCuoiKy, partnerId);

                //nếu ngày cuối của kỳ vượt quá ngày đáo hạn
                if (ngayLamViec > ngayDaoHan)
                {
                    break;
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
            return thoiGianThuc;
        }

        public CouponDto CalculateCouponByQuantity(BondInfo bondInfo, long quantity, int partnerId)
        {
            var result = new CouponDto
            {
                ProductBondId = bondInfo.Id,
                IssueDate = bondInfo.IssueDate ?? new(),
                ParValue = bondInfo.ParValue,
                BondPeriod = bondInfo.BondPeriod,
                BondPeriodUnit = bondInfo.BondPeriodUnit,
                InterestRate = bondInfo.InterestRate,
                InterestPeriod = bondInfo.InterestPeriod,
                InterestPeriodUnit = bondInfo.InterestPeriodUnit,
                InterestRateType = bondInfo.InterestRateType,
                Quantity = quantity,
            };

            List<CouponRecurentDto> couponRecurents = new List<CouponRecurentDto>();

            //Kỳ hạn trả trái tức
            int kyHanTraTraiTuc = result.InterestPeriod ?? 0;

            //if (kyHanTraTraiTuc == 0)
            //{
            //    throw new FaultException(new FaultReason($"Kỳ hạn trả trái tức = 0"), new FaultCode(((int)ErrorCode.InternalServerError).ToString()), "");
            //}

            //Ngày phát hành
            DateTime ngayPhatHanh = result.IssueDate;
            ngayPhatHanh = NextWorkDayPartner(ngayPhatHanh, partnerId);

            //Ngày đáo hạn
            DateTime ngayDaoHan = ngayPhatHanh.AddDays(result.BondPeriod ?? 0);

            if (result.BondPeriodUnit == PeriodUnit.YEAR)
            {
                ngayDaoHan = ngayPhatHanh.AddYears(result.BondPeriod ?? 0);
            }
            else if (result.BondPeriodUnit == PeriodUnit.MONTH)
            {
                ngayDaoHan = ngayPhatHanh.AddMonths(result.BondPeriod ?? 0);
            }
            ngayDaoHan = NextWorkDayPartner(ngayDaoHan, partnerId);

            //nếu ngày đáo hạn vượt quá ngày đáo hạn của lô gán về lô
            if (ngayDaoHan > bondInfo.DueDate && bondInfo.DueDate != null)
            {
                ngayDaoHan = bondInfo.DueDate.Value;
            }

            if (result.InterestRateType == InterestTypes.DINH_KY)
            {
                //List chứa thời gian thực mỗi kỳ
                List<DateTime> thoiGianThuc = GetRealTimeCopon(ngayPhatHanh, ngayDaoHan, kyHanTraTraiTuc, result.InterestPeriodUnit, partnerId);

                DateTime thoiGianThucItem = result.IssueDate;

                for (int i = 0; i < thoiGianThuc.Count; i++)
                {
                    thoiGianThucItem = thoiGianThuc[i];

                    if (i == 0)
                    {
                        int soNgayTinhLai = thoiGianThucItem.Subtract(ngayPhatHanh).Days;

                        decimal? traiTuc = (result.Quantity * result.ParValue * ((result.InterestRate ?? 0) / 100) * soNgayTinhLai) / 365;
                        //Thêm vào List
                        couponRecurents.Add(new CouponRecurentDto
                        {
                            Coupon = Math.Round(traiTuc ?? 0),
                            PayDate = thoiGianThucItem,
                            NumberOfDays = soNgayTinhLai,
                            ClosePerDate = thoiGianThucItem.AddDays(-1 * bondInfo.NumberClosePer)
                        });
                    }
                    else
                    {
                        int soNgayTinhLai = thoiGianThucItem.Subtract(thoiGianThuc[i - 1]).Days;

                        decimal? traiTuc = (result.Quantity * result.ParValue * ((result.InterestRate ?? 0) / 100) * soNgayTinhLai) / 365;

                        decimal tienGoc = 0;
                        if (i == thoiGianThuc.Count - 1)
                        {
                            decimal? tongCacKyTruoc = couponRecurents.Sum(s => s.Coupon) + traiTuc; //(các kỳ trước + kỳ này)

                            //Tổng số ngày tính lãi
                            int tongSoNgay = ngayDaoHan.Subtract(ngayPhatHanh).Days;

                            decimal? buTru = (result.Quantity * result.ParValue * ((result.InterestRate ?? 0) / 100) * tongSoNgay / 365) - tongCacKyTruoc;

                            tienGoc = ((bondInfo.Quantity) * (bondInfo.ParValue)) + (buTru ?? 0);
                        }

                        //Thêm vào List
                        couponRecurents.Add(new CouponRecurentDto
                        {
                            Coupon = Math.Round(traiTuc + tienGoc ?? 0),
                            PayDate = thoiGianThucItem,
                            NumberOfDays = soNgayTinhLai,
                            ClosePerDate = thoiGianThucItem.AddDays(-1 * bondInfo.NumberClosePer)
                        });
                    }
                }
            }
            else if (result.InterestRateType == InterestTypes.DINH_KY)
            {
                int tongSoNgay = ngayDaoHan.Subtract(ngayPhatHanh).Days;
                decimal? traiTucCuoiKy = ((result.Quantity * result.ParValue * (result.InterestRate / 100) * tongSoNgay) / 365);
                couponRecurents.Add(new CouponRecurentDto
                {
                    Coupon = Math.Round(traiTucCuoiKy ?? 0),
                    PayDate = ngayDaoHan,
                    NumberOfDays = tongSoNgay,
                    ClosePerDate = ngayDaoHan.AddDays(-1 * bondInfo.NumberClosePer)
                });
            }
            result.CouponRecurents = couponRecurents;
            return result;
        }

        private CashFlowDto GetCashFlowBase(decimal totalValue, DateTime paymentFullDate, BondPolicyDetail bondPolicyDetail, BondPolicy bondPolicy, BondSecondary bondSecondary, BondInfo bondInfo)
        {
            DateTime ngayBatDauTinhLai = NextWorkDay(paymentFullDate, bondPolicyDetail.TradingProviderId);

            var interestInfo = CalculateListInterest(bondInfo, bondPolicy, bondPolicyDetail, ngayBatDauTinhLai, totalValue, true);
            decimal traiTuc = interestInfo.CouponInfo.Sum(o => o.ActuallyCoupon);
            ProfitInfoDto kyCuoi = interestInfo.ProfitInfo.OrderBy(o => o.PayDate).LastOrDefault();

            var dongTien = new List<ProfitAppDto>();
            foreach (var traiTucItem in interestInfo.CouponInfo)
            {
                dongTien.Add(new ProfitAppDto
                {
                    CouponPeriod = traiTucItem.ReceivePeriod,
                    ReceiveValue = Math.Round(traiTucItem.ActuallyCoupon),
                    ReceiveDate = traiTucItem.PayDate,
                    Status = DateTime.Now >= traiTucItem.PayDate ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                });
            }

            foreach (var loiTucItem in interestInfo.ProfitInfo)
            {
                dongTien.Add(new ProfitAppDto
                {
                    ProfitPeriod = loiTucItem.ReceivePeriod,
                    ReceiveValue = Math.Round(loiTucItem.ActuallyProfit ?? 0),
                    ReceiveDate = loiTucItem.PayDate,
                    Status = DateTime.Now >= loiTucItem.PayDate ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                });
            }

            dongTien = dongTien.OrderBy(o => o.ReceiveDate).ToList();

            var soLuongDonGia = CalculateQuantityAndUnitPrice(totalValue, bondSecondary.Id, ngayBatDauTinhLai, bondSecondary.TradingProviderId);
            List<CouponInfoDto> couponInfos = GetListCoupon(bondInfo.Id, bondSecondary.Id, totalValue, ngayBatDauTinhLai, kyCuoi?.PayDate ?? DateTime.Now, bondPolicy.IncomeTax, bondSecondary.TradingProviderId);

            var result = new CashFlowDto
            {
                TotalValue = totalValue,
                NumberOfDays = interestInfo.ProfitInfo.Sum(o => o.NumberOfDays ?? 0),
                Coupon = traiTuc,
                InterestRate = bondPolicyDetail.Profit,
                ActuallyProfit = Math.Round(interestInfo.ActuallyProfit),

                Quantity = soLuongDonGia.Quantity,
                UnitPrice = soLuongDonGia.UnitPrice,

                StartDate = ngayBatDauTinhLai,
                EndDate = kyCuoi?.PayDate,

                TotalReceiveValue = Math.Round(totalValue + interestInfo.ActuallyProfit),

                AdvancePayment = interestInfo.AdvancePayment,

                TotalPrice = Math.Round(totalValue + interestInfo.ActuallyProfit - traiTuc),

                CashFlow = dongTien,
                CouponInfos = couponInfos
            };

            result.FinalPeriod = Math.Round(result.TotalPrice - interestInfo.AdvancePayment);
            return result;
        }

        public CashFlowDto GetCashFlowContract(decimal totalValue, DateTime paymentFullDate, BondPolicyDetail bondPolicyDetail, BondPolicy bondPolicy, BondSecondary bondSecondary, BondInfo bondInfo)
        {
            return GetCashFlowBase(totalValue, paymentFullDate, bondPolicyDetail, bondPolicy, bondSecondary, bondInfo);
        }

        public QuantityAndUnitPriceDto CalculateQuantityAndUnitPrice(decimal totalValue, int bondSecondaryId, DateTime ngayTinh, int tradingProviderId)
        {
            var bangGia = _productBondSecondPriceRepository.FindByDate(bondSecondaryId, ngayTinh, tradingProviderId);
            decimal donGia = 0;
            int soLuong = 0;
            if (bangGia != null)
            {
                soLuong = (int)(totalValue / (bangGia.Price > 0 ? bangGia.Price : 1));
                if (soLuong > 0)
                {
                    donGia = Math.Round(totalValue / soLuong);
                }
            }

            return new QuantityAndUnitPriceDto
            {
                Quantity = soLuong,
                UnitPrice = donGia
            };
        }

        public CashFlowDto GetCashFlow(decimal totalValue, int policyDetailId)
        {
            #region lấy thông tin
            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);
            if (bondPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            }

            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(bondPolicyDetail.PolicyId, bondPolicyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(bondPolicy.SecondaryId, bondPolicy.TradingProviderId);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }

            var bondPrimary = _productBondPrimaryRepository.FindById(bondSecondary.PrimaryId, null);
            if (bondPrimary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy phát hành sơ cấp"), new FaultCode(((int)ErrorCode.BondPrimaryNotFound).ToString()), "");
            }

            //lấy thông tin lô
            var bondInfo = _bondInfoRepository.FindById(bondPrimary.BondId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }
            #endregion
            return GetCashFlowBase(totalValue, DateTime.Now.Date, bondPolicyDetail, bondPolicy, bondSecondary, bondInfo);
        }

        public decimal ProfitNow(DateTime ngayDauTu, DateTime ngayDaoHan, decimal profit, decimal totalValue)
        {
            DateTime ngayHienTai = DateTime.Now.Date;
            ngayDaoHan = ngayDaoHan.Date;
            if (ngayHienTai > ngayDaoHan)
            {
                ngayHienTai = ngayDaoHan;
            }
            int day = ngayHienTai.Subtract(ngayDauTu.Date).Days;
            if (day < 0)
            {
                day = 0;
            }
            decimal profitNow = Math.Round((day * totalValue * (profit / 100)) / 365);
            return profitNow;
        }
    }
}
