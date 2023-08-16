using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CpsInfo;
using EPIC.CompanySharesEntities.Dto.CpsShared;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CalendarRepository = EPIC.CompanySharesRepositories.CalendarRepository;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsSharedServices : ICpsSharedServices
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly CpsInfoRepository _cpsInfoRepository;
        private readonly CalendarRepository _calendarRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly CpsSecondaryRepository _cpsSecondaryRepository;

        public CpsSharedServices(ILogger<CpsSharedServices> logger,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _cpsInfoRepository = new CpsInfoRepository(_connectionString, _logger);
            _calendarRepository = new CalendarRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _cpsSecondaryRepository = new CpsSecondaryRepository(_connectionString, _logger);
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

        public DateTime CalculateDueDate(CpsPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, bool isClose = true)
        {
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = bondPolicyDetail.PeriodQuantity ?? 0;
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

            var bondSecondary = _cpsSecondaryRepository.FindSecondaryById(bondPolicyDetail.SecondaryId, null);
            if (bondSecondary == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy bán theo kỳ hạn: {bondPolicyDetail.SecondaryId}"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }

            var bondInfo = _cpsInfoRepository.FindById(bondSecondary.CpsId);
            if (bondInfo == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy lô: {bondSecondary.CpsId}"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }

            //nếu ngày đáo hạn vượt quá ngày đáo hạn của lô gán về lô
            if (ngayDaoHan > bondInfo.DueDate && bondInfo.DueDate != null)
            {
                ngayDaoHan = bondInfo.DueDate.Value;
            }
            return ngayDaoHan;
        }

        public DividendCpsDto CalculateDividendByQuantity(CpsInfo cpsInfo, long quantity, int partnerId)
        {
            var result = new DividendCpsDto
            {
                Id = cpsInfo.Id,
                IssueDate = cpsInfo.IssueDate ?? new(),
                ParValue = cpsInfo.ParValue,
                Period = cpsInfo.Period,
                PeriodUnit = cpsInfo.PeriodUnit,
                InterestRate = cpsInfo.InterestRate,
                InterestPeriod = cpsInfo.InterestPeriod,
                InterestPeriodUnit = cpsInfo.InterestPeriodUnit,
                InterestRateType = cpsInfo.InterestRateType,
                Quantity = quantity,
            };

            List<DividendRecurentDto> dividendRecurents = new List<DividendRecurentDto>();

            //Kỳ hạn trả trái tức
            int kyHanTraTraiTuc = result.InterestPeriod ?? 0;

            if (kyHanTraTraiTuc == 0)
            {
                throw new FaultException(new FaultReason($"Kỳ hạn trả trái tức = 0"), new FaultCode(((int)ErrorCode.InternalServerError).ToString()), "");
            }

            //Ngày phát hành
            DateTime ngayPhatHanh = result.IssueDate;
            ngayPhatHanh = NextWorkDayPartner(ngayPhatHanh, partnerId);

            //Ngày đáo hạn
            DateTime ngayDaoHan = ngayPhatHanh.AddDays(result.Period ?? 0);

            if (result.PeriodUnit == PeriodUnit.YEAR)
            {
                ngayDaoHan = ngayPhatHanh.AddYears(result.Period ?? 0);
            }
            else if (result.PeriodUnit == PeriodUnit.MONTH)
            {
                ngayDaoHan = ngayPhatHanh.AddMonths(result.Period ?? 0);
            }
            ngayDaoHan = NextWorkDayPartner(ngayDaoHan, partnerId);

            //nếu ngày đáo hạn vượt quá ngày đáo hạn của lô gán về lô
            if (ngayDaoHan > cpsInfo.DueDate && cpsInfo.DueDate != null)
            {
                ngayDaoHan = cpsInfo.DueDate.Value;
            }

            if (result.InterestRateType == InterestTypes.DINH_KY)
            {
                //List chứa thời gian thực mỗi kỳ
                List<DateTime> thoiGianThuc = GetRealTimeDividend(ngayPhatHanh, ngayDaoHan, kyHanTraTraiTuc, result.InterestPeriodUnit, partnerId);

                DateTime thoiGianThucItem = result.IssueDate;

                for (int i = 0; i < thoiGianThuc.Count; i++)
                {
                    thoiGianThucItem = thoiGianThuc[i];

                    if (i == 0)
                    {
                        int soNgayTinhLai = thoiGianThucItem.Subtract(ngayPhatHanh).Days;

                        decimal? traiTuc = (result.Quantity * result.ParValue * ((decimal)result.InterestRate / 100) * soNgayTinhLai) / 365;
                        //Thêm vào List
                        dividendRecurents.Add(new DividendRecurentDto
                        {
                            Dividend = Math.Floor(traiTuc ?? 0),
                            PayDate = thoiGianThucItem,
                            NumberOfDays = soNgayTinhLai,
                            ClosePerDate = thoiGianThucItem.AddDays(-1 * cpsInfo.NumberClosePer ?? 0)
                        });
                    }
                    else
                    {
                        int soNgayTinhLai = thoiGianThucItem.Subtract(thoiGianThuc[i - 1]).Days;

                        decimal? traiTuc = (result.Quantity * result.ParValue * ((decimal)result.InterestRate / 100) * soNgayTinhLai) / 365;

                        decimal tienGoc = 0;
                        if (i == thoiGianThuc.Count - 1)
                        {
                            decimal? tongCacKyTruoc = dividendRecurents.Sum(s => s.Dividend) + traiTuc; //(các kỳ trước + kỳ này)

                            //Tổng số ngày tính lãi
                            int tongSoNgay = ngayDaoHan.Subtract(ngayPhatHanh).Days;

                            decimal? buTru = (result.Quantity * result.ParValue * ((decimal)result.InterestRate / 100) * tongSoNgay / 365) - tongCacKyTruoc;

                            tienGoc = ((cpsInfo.Quantity ?? 0) * (cpsInfo.ParValue ?? 0)) + (buTru ?? 0);
                        }

                        //Thêm vào List
                        dividendRecurents.Add(new DividendRecurentDto
                        {
                            Dividend = Math.Floor(traiTuc + tienGoc ?? 0),
                            PayDate = thoiGianThucItem,
                            NumberOfDays = soNgayTinhLai,
                            ClosePerDate = thoiGianThucItem.AddDays(-1 * cpsInfo.NumberClosePer ?? 0)
                        });
                    }
                }
            }
            else if (result.InterestRateType == InterestTypes.CUOI_KY)
            {
                int tongSoNgay = ngayDaoHan.Subtract(ngayPhatHanh).Days;
                decimal? traiTucCuoiKy = ((result.Quantity * result.ParValue * ((decimal)result.InterestRate / 100) * tongSoNgay) / 365);
                dividendRecurents.Add(new DividendRecurentDto
                {
                    Dividend = Math.Floor(traiTucCuoiKy ?? 0),
                    PayDate = ngayDaoHan,
                    NumberOfDays = tongSoNgay,
                    ClosePerDate = ngayDaoHan.AddDays(-1 * cpsInfo.NumberClosePer ?? 0)
                });
            }
            result.DividendRecurents = dividendRecurents;
            return result;
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
                if (_calendarRepository.CheckHoliday(ngayLamViecTiepTheo, partnerId))
                {
                    ngayLamViecTiepTheo = ngayLamViecTiepTheo.AddDays(1);
                }
                else
                {
                    return ngayLamViecTiepTheo;
                }
            }
        }

        public List<DateTime> GetRealTimeDividend(DateTime ngayPhatHanh, DateTime ngayDaoHan, int kyHanTraTraiTuc, string donViKyHan, int partnerId)
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

        public CalculateProfitDto CalculateProfit(CpsPolicy bondPolicy,
            CpsPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan)
        {
            ngayBatDauTinhLai = NextWorkDay(ngayBatDauTinhLai.Date, bondPolicyDetail.TradingProviderId);

            //Ngày đáo hạn
            DateTime ngayDaoHan = CalculateDueDate(bondPolicyDetail, ngayBatDauTinhLai.Date);

            //Tỷ suất lợi tức
            decimal tySuatLoiTuc = (bondPolicyDetail.Profit ?? 0) / 100;

            //thuế lợi nhuận, thuế thu nhập cá nhân
            decimal thueLoiNhuan = 0;
            if (khachCaNhan)
            {
                thueLoiNhuan = (bondPolicy.IncomeTax ?? 0) / 100;
            }

            //tính lợi tức cả kỳ
            decimal loiTucCaKyThucTe = soTienDauTu * tySuatLoiTuc * (ngayDaoHan - ngayBatDauTinhLai.Date).Days / 365;
            decimal loiTucCaKy = loiTucCaKyThucTe / (1 - thueLoiNhuan);
            decimal thueCaKy = loiTucCaKyThucTe - loiTucCaKy;

            return new CalculateProfitDto
            {
                Profit = Math.Floor(loiTucCaKy),
                Tax = Math.Floor(thueCaKy),
                ActuallyProfit = Math.Floor(loiTucCaKyThucTe)
            };
        }
    }
}
