using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestRepositories;
using EPIC.InvestSharedEntites.Dto.InvestShared;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Implements
{
    public class InvestSharedServices : IInvestSharedServices
    {
        private readonly ILogger _logger;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ProjectRepository _projectRepository;
        private readonly CalendarRepository _calendarRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly WithdrawalRepository _withdrawalSettlementRepository;
        private readonly InvestInterestPaymentEFRepository _interestPaymentEFRepository;

        public InvestSharedServices(ILogger<InvestSharedServices> logger,
            Entities.DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _calendarRepository = new CalendarRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _withdrawalSettlementRepository = new WithdrawalRepository(_connectionString, _logger);
            _interestPaymentEFRepository = new InvestInterestPaymentEFRepository(_dbContext, _logger);
        }

        public DateTime NextWorkDay(DateTime ngayDangXet, int tradingProviderId = 0)
        {
            DateTime ngayLamViecTiepTheo = ngayDangXet.Date;// new(ngayDangXet.Year, ngayDangXet.Month, ngayDangXet.Day);
            if (tradingProviderId == 0)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            while (true)
            {
                if (_calendarRepository.CheckHoliday(ngayLamViecTiepTheo, tradingProviderId, false))
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
            DateTime ngayLamViecTruoc = ngayDangXet.Date; //new(ngayDangXet.Year, ngayDangXet.Month, ngayDangXet.Day);
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

        /// <summary>
        /// Tính ngày đáo hạn
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="distributionCloseSellDate">Ngày kết thúc phân phối</param>
        /// <returns></returns>
        public DateTime CalculateDueDate(PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, DateTime? distributionCloseSellDate)
        {
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = policyDetail?.PeriodQuantity ?? 0;
            //Ngày đáo hạn
            DateTime ngayDaoHan = ngayBatDauTinhLai.Date;
            if (policyDetail?.InterestDays != null) //nếu có cài ngày chính xác
            {
                ngayDaoHan = ngayDaoHan.AddDays(policyDetail.InterestDays.Value);
            }
            else //không cài ngày chính xác
            {
                if (policyDetail?.PeriodType == PeriodUnit.DAY)
                {
                    ngayDaoHan = ngayDaoHan.AddDays(soKyDaoHan);
                }
                else if (policyDetail?.PeriodType == PeriodUnit.MONTH)
                {
                    ngayDaoHan = ngayDaoHan.AddMonths(soKyDaoHan);
                }
                else if (policyDetail?.PeriodType == PeriodUnit.YEAR)
                {
                    ngayDaoHan = ngayDaoHan.AddYears(soKyDaoHan);
                }
            }
            // Kiểm tra xem ngày đáo hạn có vượt quá ngày đóng phân phối distribution
            if (distributionCloseSellDate != null)
            {
                ngayDaoHan = (distributionCloseSellDate.Value.Date < ngayDaoHan) ? distributionCloseSellDate.Value.Date : ngayDaoHan;
            }
            ngayDaoHan = NextWorkDay(ngayDaoHan, policyDetail?.TradingProviderId ?? 0); //kiểm tra nếu trùng ngày nghỉ thì cộng lên
            return ngayDaoHan;
        }

        /// <summary>
        /// Mô tả dòng thời gian rút vốn gồm
        /// Thông tin thời gian, lãi suất của các kỳ hạn trong chính sách đang được sử dụng
        /// Thông tin kỳ hạn trả lãi và lợi nhuận
        /// </summary>
        /// <param name="ngayBatDauTinhLai"> Ngày bắt đầu tính lãi của hợp đồng</param>
        /// <param name="order"> Thông tin sổ lệnh</param>
        /// <param name="policyDetail"> Thông tin kì hạn đang được sử dụng của hợp đồng</param>
        /// <param name="policy"> Thông tin chính sách</param>
        /// <param name="thueLoiNhuan"> Thuế lợi nhuận nếu là khách hàng cá nhân</param>
        /// <returns></returns>
        public ThoiGianChiTraDto MoTaThoiGianRutVonInvest (DateTime ngayBatDauTinhLai, InvOrder order,  PolicyDetail policyDetail, Policy policy, decimal thueLoiNhuan, DateTime? distributionCloseSellDate)
        {
            var result = new ThoiGianChiTraDto();
            ngayBatDauTinhLai = ngayBatDauTinhLai.Date;

            // Lấy ngày đáo hạn lưu trong hợp đồng nếu chưa có thì tính
            DateTime ngayDaoHan = order.DueDate ?? CalculateDueDate(policyDetail, ngayBatDauTinhLai, distributionCloseSellDate);

            List<DateTime> thoiGianTraLai = _interestPaymentEFRepository.GetListPayDateByPolicyDetail(policyDetail, ngayBatDauTinhLai, ngayDaoHan);

            //Mô tả lợi nhuận khi khách hàng đầu tư
            List<ThoiGianTraLoiNhuanDto> thoiGianTraLoiNhuan = new ();

            List<ThoiGianKetThucGiaDinhDto> thoiGianKetThucGiaDinh = new();

            for (int i = 0; i < thoiGianTraLai.Count(); i++ )
            {
                decimal loiNhuanTraLai ;
                int soNgay;
                if (i == 0) //kỳ trả đầu tiên
                {
                    soNgay = (thoiGianTraLai[i] - ngayBatDauTinhLai.Date).Days;
                }
                else
                {
                    soNgay = (thoiGianTraLai[i] - thoiGianTraLai[i - 1]).Days;
                }

                loiNhuanTraLai = (order.TotalValue) * ((policyDetail.Profit ?? 0) /100) * soNgay / 365;

                thoiGianTraLoiNhuan.Add(new ThoiGianTraLoiNhuanDto
                {
                    ThoiGianTraLoiNhuan = thoiGianTraLai[i],
                    LoiNhuanTraLai = Math.Round(loiNhuanTraLai),
                    SoNgay = (thoiGianTraLai[i] - ngayBatDauTinhLai.Date).Days
                });
            }

            var listPolicyDetail = _policyRepository.GetAllPolicyDetail(policy.Id, order.TradingProviderId, Status.ACTIVE);
            foreach (var item in listPolicyDetail)
            {
                DateTime ngayKetThucGiaDinh = CalculateDueDate(item, ngayBatDauTinhLai, distributionCloseSellDate);

                // Nếu ngày kết thúc giả định lớn hơn ngày đáo hạn của hợp đồng
                if (order.DueDate != null && ngayKetThucGiaDinh > order.DueDate.Value.Date)
                {
                    ngayKetThucGiaDinh = order.DueDate.Value.Date;
                }
                int soNgay = (ngayKetThucGiaDinh - ngayBatDauTinhLai).Days;

                thoiGianKetThucGiaDinh.Add(new ThoiGianKetThucGiaDinhDto
                {
                    PolicyDetailId = item.Id,
                    ThoiGianKetThuc = ngayKetThucGiaDinh,
                    SoNgay = soNgay
                });
            }
            //listPolicyDetail.OrderBy(r => r.)
            result.ThoiGianTraLoiNhuan = thoiGianTraLoiNhuan;
            result.ThoiGianKetThucGiaDinh = thoiGianKetThucGiaDinh;
            return result;
        }

        /// <summary>
        /// Thông tin lợi nhuận sau khi rút vốn
        /// </summary>
        /// <param name="order"></param>
        /// <param name="policy"></param>
        /// <param name="policyDetail"></param>
        /// <param name="tongTienConDauTu">Số tiền còn lại sau khi đã rút ở những lần trước đó (nếu có)</param>
        /// <param name="soTienRut">Sối tiền rút</param>
        /// <param name="ngayRut">Ngày rút tiền</param>
        /// <param name="khachCaNhan">Nếu là khách hàng cá nhân thì tính thuế thu nhập</param>
        /// <returns></returns>
        public RutVonDto RutVonInvest(InvOrder order, Policy policy, PolicyDetail policyDetail, decimal tongTienConDauTu, decimal soTienRut, DateTime ngayRut, bool khachCaNhan, DateTime? distributionCloseSellDate)
        {
            ngayRut = ngayRut.Date;

            //Nếu là khách hàng cá nhân thì tính thuế thu nhập
            decimal thueLoiNhuan = 0;
            if (khachCaNhan)
            {
                thueLoiNhuan = (policy.IncomeTax ?? 0) / 100;
            }

            //Số tiền còn lại sau khi rút
            tongTienConDauTu = tongTienConDauTu - soTienRut;

            //   (1)   Số ngày tính từ ngày rút đến ngày bắt đầu tính lãi
            int soNgayDaDauTu = (ngayRut - order.InvestDate.Value.Date).Days;

            // Lập mô tả dòng thời gian để lấy kỳ hạn thích hợp
            var moTaThoiGian = MoTaThoiGianRutVonInvest(order.InvestDate.Value, order, policyDetail, policy, thueLoiNhuan, distributionCloseSellDate);

            //Lấy kỳ hạn cho thời gian rút vốn 
            var kyHanRutVon = moTaThoiGian.ThoiGianKetThucGiaDinh.Where(r => r.SoNgay <= soNgayDaDauTu).OrderByDescending(r => r.SoNgay).FirstOrDefault();

            //Nếu số ngày rút còn nhỏ hơn cả số ngày đầu tư của kỳ hạn bé nhất, thì phần trăm lợi nhuận = 0
            decimal phanTramLoiNhuan = 0;

            // Nếu tính theo kỳ hạn
            if (policy.CalculateWithdrawType == InvestCalculateWithdrawTypes.KY_HAN_THAP_HON_GAN_NHAT && kyHanRutVon != null)
            {
                //Lấy thông tin kỳ hạn mới
                var kyHanMoi = _policyRepository.FindPolicyDetailById(kyHanRutVon.PolicyDetailId);
                if (kyHanMoi != null)
                {
                    phanTramLoiNhuan = kyHanMoi.Profit ?? 0;
                }
            }
            // Nếu tính theo loại cố định
            else if (policy.CalculateWithdrawType == InvestCalculateWithdrawTypes.GIA_CO_DINH)
            {
                phanTramLoiNhuan = policy.ProfitRateDefault ?? 0;
            }

            //    (4)   Tính lợi nhuận rút vốn theo kỳ hạn mới
            decimal loiNhuanRutVon = (soTienRut * soNgayDaDauTu * (phanTramLoiNhuan / 100)) / 365;

            // Tìm ra số ngày của kỳ trả lãi đã nhận trước đó
            var loiNhuanTraLai = moTaThoiGian.ThoiGianTraLoiNhuan.Where(r => r.SoNgay <= soNgayDaDauTu).OrderByDescending(r => r.SoNgay).FirstOrDefault();

            decimal loiNhuanDaNhan = 0;
            int soNgayTinhThue = ngayRut.Subtract(order.InvestDate.Value.Date).Days;

            //Chưa có lợi nhuận trước đó
            if (loiNhuanTraLai != null)
            {
                //   (7)  Lợi nhuận đã nhận trước đó , Lợi nhuận khấu trừ
                loiNhuanDaNhan = ((soTienRut * loiNhuanTraLai.SoNgay * (policyDetail.Profit / 100)) / 365) ?? 0;
               
                soNgayTinhThue = (ngayRut - loiNhuanTraLai.ThoiGianTraLoiNhuan.Value).Days;
            }

            //  (5) Thuế thu nhập cá nhân
            decimal thueThuNhap = 0; 

            decimal loiNhuanThucNhan = 0;
            if (policy.CalculateType == CalculateTypes.GROSS)
            {
                //lợi tức thực nhận: Lợi tức rút vốn - lợi tức khấu trừ - thuế
                thueThuNhap = (soTienRut * (phanTramLoiNhuan / 100) / 365) * soNgayTinhThue * thueLoiNhuan;
                loiNhuanThucNhan = loiNhuanRutVon - loiNhuanDaNhan - thueThuNhap;
            }
            else if (policy.CalculateType == CalculateTypes.NET)
            {
                loiNhuanThucNhan = loiNhuanRutVon - loiNhuanDaNhan;
                //tính lại thuế
                thueThuNhap = soTienRut * (phanTramLoiNhuan / 100) / 365 * soNgayTinhThue / (1 - thueLoiNhuan) * thueLoiNhuan;
            }

            //  (6)  Phí rút vốn
            decimal phiRutVon = 0;
            if (policy.ExitFee != null && policy.ExitFeeType == ExitFeeTypes.RUT_THEO_SO_TIEN)
            {
                phiRutVon = soTienRut * ((policy.ExitFee ?? 0) / 100);
            }
            else if (policy.ExitFee != null && policy.ExitFeeType == ExitFeeTypes.RUT_THEO_NAM)
            {
                phiRutVon = (soTienRut * ((policy.ExitFee ?? 0) / 100) * soNgayDaDauTu) / 365;
            }

            // Số tiền thực nhận (8) = (2) + (4) - (5) - (6) - (7)
            var tienThucNhan = soTienRut + loiNhuanThucNhan - phiRutVon;

            return new RutVonDto
            {
                NumberOfDaysInvested = soNgayDaDauTu,
                WithdrawlDate = ngayRut.Date,                       // (1)
                AmountMoney = Math.Round(soTienRut),                // (2)
                ProfitRate = phanTramLoiNhuan,                       // (3)
                WithdrawalProfit = Math.Round(loiNhuanRutVon),      // (4)
                IncomeTax = Math.Round(thueThuNhap),                // (5)
                WithdrawalFee = Math.Round(phiRutVon),              // (6)
                ProfitReceived = Math.Round(loiNhuanDaNhan),   // (7)
                ActuallyAmount = Math.Round(tienThucNhan),     // (8)
                ActuallyProfit = Math.Round(loiNhuanThucNhan),
                Surplus = Math.Round(tongTienConDauTu),
                ExitFee = policy.ExitFee,
                ExitFeeType = policy.ExitFeeType
            }; 
        }    


        private List<ProfitInfoDto> GetListProfit(Policy policy, PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, DateTime ngayDaoHan,
            decimal soTienDauTu, decimal tySuatLoiTuc, decimal thueLoiNhuan, ProfitAndInterestDto result, int orderId, bool isApp = false)
        {
            //lợi tức
            List<ProfitInfoDto> dsLoiTuc = new(); 

            if (new int[] { InterestTypes.NGAY_CO_DINH, InterestTypes.DINH_KY, InterestTypes.NGAY_CUOI_THANG, InterestTypes.NGAY_DAU_THANG}.Contains(policyDetail?.InterestType ?? 0))//trả định kỳ
            {
                //tính danh sách kỳ trả lợi tức
                List<DateTime> thoiGianThuc = new();
                ngayDaoHan = ngayDaoHan.Date;
                DateTime ngayDauKy = ngayBatDauTinhLai.Date;

                //Tìm thời gian thực
                if (isApp)
                {
                    thoiGianThuc = _investOrderRepository.GetAllPaymentDate(orderId);
                }
                if (!isApp || (isApp && thoiGianThuc.Count == 0))
                {
                    thoiGianThuc = _interestPaymentEFRepository.GetListPayDateByPolicyDetail(policyDetail, ngayDauKy, ngayDaoHan);
                }
                decimal tienTamUng = 0;
                //ít nhất 1 kỳ
                for (int i = 0; i < thoiGianThuc.Count; i++)
                {
                    int soNgay;
                    if (i == 0) //kỳ trả đầu tiên
                    {
                        soNgay = (thoiGianThuc[i] - ngayBatDauTinhLai.Date).Days;
                    }
                    else
                    {
                        soNgay = (thoiGianThuc[i] - thoiGianThuc[i - 1]).Days;
                    }

                    decimal loiTucKyNay = 0;
                    decimal thue = 0;
                    decimal tongTienThucNhan = 0;

                    //Chi trả lợi tức chưa bao gồm thuế
                    if (policy.CalculateType == CalculateTypes.GROSS)
                    {
                        loiTucKyNay = (soTienDauTu * tySuatLoiTuc * soNgay / 365);
                        thue = loiTucKyNay * thueLoiNhuan;
                        tongTienThucNhan = loiTucKyNay - thue;
                    }

                    //Chi trả lợi tức đã bao gồm thuế
                    else if (policy.CalculateType == CalculateTypes.NET)
                    {
                        tongTienThucNhan = (soTienDauTu * tySuatLoiTuc * soNgay / 365);
                        loiTucKyNay = tongTienThucNhan / (1 - thueLoiNhuan);
                        thue = loiTucKyNay - tongTienThucNhan;
                    }

                    if (i < thoiGianThuc.Count - 1)
                    {
                        tienTamUng += Math.Round(tongTienThucNhan);
                    }

                    if (i == thoiGianThuc.Count - 1 || (thoiGianThuc.Count == 1 && i == 0)) //là kỳ cuối
                    {
                        //tính lợi tức cả kỳ
                        decimal loiTucCaKy = 0;
                        decimal thueCaKy = 0;
                        decimal loiTucCaKyThucTe = 0;

                        //Chi trả lợi tức chưa bao gồm thuế
                        if (policy.CalculateType == CalculateTypes.GROSS)
                        {
                            loiTucCaKy = soTienDauTu * tySuatLoiTuc * (ngayDaoHan - ngayBatDauTinhLai.Date).Days / 365;
                            thueCaKy = loiTucCaKy * thueLoiNhuan;
                            loiTucCaKyThucTe = loiTucCaKy - thueCaKy;
                        }

                        //Chi trả lợi tức đã bao gồm thuế
                        else if (policy.CalculateType == CalculateTypes.NET)
                        {
                            loiTucCaKyThucTe = (soTienDauTu * tySuatLoiTuc * (ngayDaoHan - ngayBatDauTinhLai.Date).Days / 365);
                            loiTucCaKy = loiTucCaKyThucTe / (1 - thueLoiNhuan);
                            thueCaKy = loiTucCaKy - loiTucCaKyThucTe;
                        }

                        result.AdvancePayment = Math.Round(tienTamUng);
                        result.AllProfit = Math.Round(loiTucCaKy);
                        result.TaxProfit = Math.Round(thueCaKy);
                        result.ActuallyProfit = Math.Round(loiTucCaKyThucTe);

                        //số tiền kỳ cuối nhận được
                        tongTienThucNhan = soTienDauTu + loiTucCaKyThucTe - tienTamUng; //số tiền đầu tư + lợi tức cả kỳ thực tế - tiền tạm ứng (tiền nhận trong kỳ)
                    }

                    dsLoiTuc.Add(new ProfitInfoDto
                    {
                        ReceivePeriod = dsLoiTuc.Count + 1,
                        ProfitRate = tySuatLoiTuc,
                        PayDate = thoiGianThuc[i],
                        ActuallyPayDate = thoiGianThuc[i],
                        Profit = Math.Round(loiTucKyNay),
                        Tax = Math.Round(thue),
                        ActuallyProfit = Math.Round(tongTienThucNhan),
                        NumberOfDays = soNgay,
                        Status = DateTime.Now >= thoiGianThuc[i] ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                    });
                }
            }
            else if (policyDetail?.InterestType == InterestTypes.CUOI_KY) //trả cuối kỳ
            {
                int soNgay = ngayDaoHan.Subtract(ngayBatDauTinhLai.Date).Days;

                decimal loiTuc = 0;
                decimal thue = 0;
                decimal loiTucThucNhan = 0;

                //Chi trả lợi tức chưa bao gồm thuế
                if (policy.CalculateType == CalculateTypes.GROSS)
                {
                    loiTuc = soTienDauTu * tySuatLoiTuc * soNgay / 365;
                    thue = loiTuc * thueLoiNhuan;
                    loiTucThucNhan = loiTuc - thue;
                }

                //Chi trả lợi tức đã bao gồm thuế
                else if (policy.CalculateType == CalculateTypes.NET)
                {
                    loiTucThucNhan = (soTienDauTu * tySuatLoiTuc * soNgay / 365);
                    loiTuc = loiTucThucNhan / (1 - thueLoiNhuan);
                    thue = loiTuc - loiTucThucNhan;
                }

                decimal tongTienThucNhan = soTienDauTu + loiTucThucNhan;

                result.AllProfit = Math.Round(loiTuc);
                result.TaxProfit = Math.Round(thue);
                result.ActuallyProfit = Math.Round(loiTucThucNhan);

                dsLoiTuc.Add(new ProfitInfoDto
                {
                    ReceivePeriod = dsLoiTuc.Count + 1,
                    ProfitRate = tySuatLoiTuc,
                    PayDate = ngayDaoHan,
                    ActuallyPayDate = ngayDaoHan,
                    Profit = Math.Round(loiTuc),
                    Tax = Math.Round(thue),
                    ActuallyProfit = Math.Round(tongTienThucNhan),
                    NumberOfDays = soNgay,
                    Status = DateTime.Now >= ngayDaoHan ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                });
            }
            return dsLoiTuc;
        }

        public ProfitAndInterestDto CalculateListInterest(Project project, Policy policy,
            PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan, DateTime? distributionCloseSellDate, int orderId = 0, bool isApp = false)
        {
            var result = new ProfitAndInterestDto();
            var orderFind = _dbContext.InvOrders.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO);
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = policyDetail?.PeriodQuantity ?? 0;

            // Lấy ngày đáo hạn lưu trong hợp đồng nếu chưa có thì tính
            DateTime ngayDaoHan = (orderFind?.DueDate != null) ? orderFind.DueDate.Value : CalculateDueDate(policyDetail, ngayBatDauTinhLai, distributionCloseSellDate);

            //Tỷ suất lợi tức
            decimal tySuatLoiTuc = (policyDetail?.Profit ?? 0) / 100;

            //thuế lợi nhuận, thuế thu nhập cá nhân
            decimal thueLoiNhuan = 0;
            if (khachCaNhan)
            {
                thueLoiNhuan = (policy?.IncomeTax ?? 0) / 100;
            }

            //lợi tức
            List<ProfitInfoDto> dsLoiTuc = new();

            dsLoiTuc = GetListProfit(policy, policyDetail, ngayBatDauTinhLai, ngayDaoHan, soTienDauTu, tySuatLoiTuc, thueLoiNhuan, result, orderId, isApp);

            result.ProfitInfo = dsLoiTuc;
            return result;
        }

        public CalculateProfitDto CalculateProfit(Policy policy,
            PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan,DateTime? distributionCloseSellDate)
        {
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = policyDetail.PeriodQuantity ?? 0;

            //Ngày đáo hạn
            DateTime ngayDaoHan = CalculateDueDate(policyDetail, ngayBatDauTinhLai, distributionCloseSellDate);

            //Tỷ suất lợi tức
            decimal tySuatLoiTuc = (policyDetail.Profit ?? 0) / 100;

            //thuế lợi nhuận, thuế thu nhập cá nhân
            decimal thueLoiNhuan = 0;
            if (khachCaNhan)
            {
                thueLoiNhuan = (policy.IncomeTax ?? 0) / 100;
            }

            //tính lợi tức cả kỳ
            decimal loiTucCaKy = 0;
            decimal thueCaKy = 0;
            decimal loiTucCaKyThucTe = 0;

            //Chi trả lợi tức chưa bao gồm thuế
            if (policy.CalculateType == CalculateTypes.GROSS)
            {
                loiTucCaKy = soTienDauTu * tySuatLoiTuc * (ngayDaoHan - ngayBatDauTinhLai.Date).Days / 365;
                thueCaKy = loiTucCaKy * thueLoiNhuan;
                loiTucCaKyThucTe = loiTucCaKy - thueCaKy;
            }

            //Chi trả lợi tức đã bao gồm thuế
            else if (policy.CalculateType == CalculateTypes.NET)
            {
                loiTucCaKyThucTe = (soTienDauTu * tySuatLoiTuc * (ngayDaoHan - ngayBatDauTinhLai.Date).Days / 365);
                loiTucCaKy = loiTucCaKyThucTe / (1 - thueLoiNhuan);
                thueCaKy = loiTucCaKy - loiTucCaKyThucTe;
            }

            return new CalculateProfitDto
            {
                Profit = Math.Round(loiTucCaKy),
                Tax = Math.Round(thueCaKy),
                ActuallyProfit = Math.Round(loiTucCaKyThucTe)
            };
        }
        private CashFlowDto GetCashFlowBase(decimal totalValue, DateTime paymentFullDate, PolicyDetail policyDetail, Policy policy, Distribution distribution, Project project, int orderId = 0, bool isApp = false)
        {
            DateTime ngayBatDauTinhLai = paymentFullDate.Date;

            var interestInfo = CalculateListInterest(project, policy, policyDetail, paymentFullDate, totalValue, true, distribution.CloseCellDate, orderId, isApp);
            ProfitInfoDto kyCuoi = interestInfo.ProfitInfo.OrderBy(o => o.PayDate).LastOrDefault();

            var interestPayment = _interestPaymentEFRepository.Entity.Where(r => r.OrderId == orderId && (new int[] {InterestPaymentStatus.DA_DUYET_CHI_TIEN, InterestPaymentStatus .DA_DUYET_KHONG_CHI_TIEN}).Contains(r.Status ?? 0));
            
            var dongTien = new List<ProfitDto>();
            foreach (var loiTucItem in interestInfo.ProfitInfo)
            {
                ProfitDto dongTienItem = new()
                {
                    ProfitPeriod = loiTucItem.ReceivePeriod,
                    ProfitPeriodName = (kyCuoi == loiTucItem) ? "Tiền nhận cuối kỳ" : $"Lợi nhuận kỳ {loiTucItem.ReceivePeriod}",
                    ReceiveValue = Math.Round(loiTucItem.ActuallyProfit ?? 0),
                    ReceiveDate = loiTucItem.PayDate,
                    Status = DateTime.Now >= loiTucItem.PayDate ? PeriodStatus.DEN_HAN : PeriodStatus.CHUA_DEN_HAN
                };
                if ((interestPayment.Select(p => p.PeriodIndex).Contains(loiTucItem.ReceivePeriod)) || (kyCuoi == loiTucItem && interestPayment.Where(p => p.IsLastPeriod == YesNo.YES).Any()))
                {
                    dongTienItem.Status = PeriodStatus.DA_TRA;
                }   
                dongTien.Add(dongTienItem);
            }

            dongTien = dongTien.OrderBy(o => o.ReceiveDate).ToList();

            var result = new CashFlowDto
            {
                TotalValue = totalValue,
                NumberOfDays = interestInfo.ProfitInfo.Sum(o => o.NumberOfDays ?? 0),
                InterestRate = policyDetail.Profit ?? 0,
                ActuallyProfit = Math.Round(interestInfo.ActuallyProfit),

                StartDate = ngayBatDauTinhLai,
                EndDate = kyCuoi?.PayDate,

                TotalReceiveValue = Math.Round(totalValue + interestInfo.ActuallyProfit),

                TotalPrice = Math.Round(totalValue + interestInfo.ActuallyProfit),

                CashFlow = dongTien,
                TaxProfit = interestInfo.TaxProfit,
                Tax = kyCuoi.Tax,
                BeforeTaxProfit = Math.Round(interestInfo.ActuallyProfit + interestInfo.TaxProfit)
            };

            result.FinalPeriod = Math.Round(result.TotalPrice - interestInfo.AdvancePayment);

            return result;
        }

        public CashFlowDto GetCashFlowContract(decimal totalValue, DateTime paymentFullDate, PolicyDetail policyDetail, Policy policy, Distribution distribution, Project project, int orderId = 0, bool isApp = false)
        {
            return GetCashFlowBase(totalValue, paymentFullDate, policyDetail, policy, distribution, project, orderId, isApp);
        }

        public CashFlowDto GetCashFlow(decimal totalValue, int policyDetailId)
        {
            #region lấy thông tin
            var policyDetail = _policyRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            //Lấy thông tin bán theo kỳ hạn
            var distribution = _distributionRepository.FindById(policy.DistributionId, policy.TradingProviderId);
            if (distribution == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }

            //lấy thông tin dự án
            var project = _projectRepository.FindById(distribution.ProjectId, null);
            if (project == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
            }
            #endregion
            return GetCashFlowBase(totalValue, DateTime.Now.Date, policyDetail, policy, distribution, project);
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
