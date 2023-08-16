using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.InvestRepositories
{
    public class InvestInterestPaymentEFRepository : BaseEFRepository<InvestInterestPayment>
    {
        public InvestInterestPaymentEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestInterestPayment.SEQ}")
        {
        }

        public PagingResult<DanhSachChiTraDto> FindAllInterestPayment(InterestPaymentFilterDto input, int? tradingProviderId)
        {
            var interestPayment = _dbSet.Include(e => e.Order)
                                    .ThenInclude(c => c.InvestorIdentification)
                                .Include(e => e.Order)
                                    .ThenInclude(c => c.Project)
                                .Include(o => o.CifCodes)
                                    .ThenInclude(c => c.Investor)
                                .Include(o => o.CifCodes)
                                    .ThenInclude(c => c.BusinessCustomer)
                                .Include(o => o.PolicyDetail)
                                .Where(p => p.Deleted == YesNo.NO && p.CifCodes != null && p.PayDate != null && p.Order.Deleted == YesNo.NO && p.Order.Project.Deleted == YesNo.NO
                                && (input.InterestPaymentStatus == null || input.InterestPaymentStatus == p.Status)
                                && (input.Status == null || (input.Status == InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA && InterestPaymentStatus.NHOM_CHUA_CHI_TRA.Contains(p.Status ?? 0))
                                    || (input.Status == InterestPaymentStatus.DA_DUYET_CHI_TIEN && InterestPaymentStatus.NHOM_DA_CHI_TRA.Contains(p.Status ?? 0)))
                                && ((input.IsExactDate == null || input.IsExactDate == YesNo.YES) && ((input.NgayChiTra != null ? input.NgayChiTra.Value.Date : DateTime.Now.Date) == p.PayDate.Value.Date)
                                    || (input.IsExactDate == YesNo.NO && (input.NgayChiTra != null ? input.NgayChiTra.Value.Date : DateTime.Now.Date) >= p.PayDate.Value.Date))
                                && (input.CifCode == null || input.CifCode == p.CifCode)
                                && (input.IsLastPeriod == null || input.IsLastPeriod == p.IsLastPeriod)
                                && (input.MethodInterest == null || p.Order.MethodInterest == input.MethodInterest)
                                && (input.Phone == null || input.Phone == p.CifCodes.Investor.Phone || input.Phone == p.CifCodes.Investor.RepresentativePhone
                                    || input.CifCode == p.CifCodes.BusinessCustomer.Phone)
                                && (input.IdNo == null || input.IdNo == p.Order.InvestorIdentification.IdNo)
                                && (input.TaxCode == null || input.TaxCode == p.CifCodes.BusinessCustomer.TaxCode)
                                && ((input.ContractCode == null || p.Order.ContractCode.Contains(input.ContractCode) || (_epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == p.Order.Id && e.ContractCodeGen.Contains(input.ContractCode)).Any())))
                                && (tradingProviderId == null || tradingProviderId == p.TradingProviderId)
                                && (input.SettlementMethod == null || (input.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && !_epicSchemaDbContext.InvestRenewalsRequests.Any(r => r.OrderId == p.OrderId && r.SettlementMethod != SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && r.Deleted == YesNo.NO
                                                    && (r.Status == InvestRenewalsRequestStatus.KHOI_TAO || r.Status == InvestRenewalsRequestStatus.DA_DUYET)))
                                    || (_epicSchemaDbContext.InvestRenewalsRequests.Where(r => r.OrderId == p.OrderId && r.SettlementMethod != SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && r.Deleted == YesNo.NO
                                                    && (r.Status == InvestRenewalsRequestStatus.KHOI_TAO || r.Status == InvestRenewalsRequestStatus.DA_DUYET)).OrderByDescending(r => r.Id).FirstOrDefault().SettlementMethod == input.SettlementMethod)))
                                .Select(p => new DanhSachChiTraDto
                                {
                                    Id = p.Id,
                                    PayDate = p.PayDate.Value,
                                    ActuallyProfit = p.Profit,
                                    Profit = p.Profit,
                                    AmountMoney = p.AmountMoney,
                                    ApproveBy = p.ApproveBy,
                                    ApproveDate = p.ApproveDate,
                                    CifCode = p.CifCode,
                                    CreatedBy = p.CreatedBy,
                                    CreatedDate = p.CreatedDate,
                                    InterestPaymentStatus = p.Status,
                                    StatusBank = p.StatusBank,
                                    OrderId = p.OrderId,
                                    OrderStatus = p.Order.Status,
                                    IsLastPeriod = p.IsLastPeriod,
                                    PeriodIndex = p.PeriodIndex ?? 0,
                                    TotalValueCurrent = p.TotalValueCurrent,
                                    Tax = p.Tax,
                                    DistributionId = p.Order.DistributionId,
                                    MethodInterest = p.Order.MethodInterest,
                                    TotalValue = p.Order.TotalValue,
                                    PeriodQuantity = p.PolicyDetail.PeriodQuantity,
                                    PolicyDetailName = p.PolicyDetail.Name,
                                    PeriodType = p.PolicyDetail.PeriodType,
                                    TotalValueInvestment = p.TotalValueInvestment,
                                    InvCode = p.Order.Project.InvCode,
                                    PolicyDetailId = p.PolicyDetailId ?? 0,
                                    Name = p.CifCodes.BusinessCustomer == null ? (p.Order.InvestorIdentification.Fullname) : p.CifCodes.BusinessCustomer.Name,
                                    InvName = p.Order.Project.InvName,
                                    // Nếu mã hợp đồng trong contractFile trùng nhau thì lấy ra, nếu không thì lấy mã hợp đồng
                                    ContractCode = _epicSchemaDbContext.InvestOrderContractFile
                                                    .Where(o => o.OrderId == p.OrderId && o.Deleted == YesNo.NO)
                                                    .Select(o => o.ContractCodeGen)
                                                    .Distinct().Count() == 1 ? _epicSchemaDbContext.InvestOrderContractFile
                                                                               .Where(o => o.OrderId == p.OrderId && o.Deleted == YesNo.NO).FirstOrDefault().ContractCodeGen
                                                                             : p.Order.ContractCode
                                });
            interestPayment = interestPayment.OrderByDescending(x => x.PayDate);
            int totalItem = interestPayment.Count();
            interestPayment = interestPayment.OrderDynamic(input.Sort);

            return new PagingResult<DanhSachChiTraDto>
            {
                TotalItems = totalItem,
                Items = interestPayment.Skip(input.Skip).Take(input.PageSize)
            };
        }

        /// <summary>
        /// Thêm chi trả
        /// </summary>
        public InvestInterestPayment Add(InvestInterestPayment input)
        {
            _logger.LogInformation($"{nameof(InvestInterestPaymentEFRepository)} -> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.Status = InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA;
            input.Deleted = YesNo.NO;
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Thêm ngày chi trả
        /// </summary>
        /// <param name="input"></param>
        public void AddInvestInterestPaymentDate(InvestInterestPaymentDate input)
        {
            _logger.LogInformation($"{nameof(InvestInterestPaymentEFRepository)} -> {nameof(AddInvestInterestPaymentDate)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey(InvestInterestPaymentDate.SEQ);
            _epicSchemaDbContext.InvestInterestPaymentDates.Add(input);
        }

        public InvestInterestPayment FindById(long id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(InvestInterestPaymentEFRepository)}{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(r => r.Id == id && (tradingProviderId == null || r.TradingProviderId == tradingProviderId) && r.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Duyệt chi tiền các kỳ bình thường
        /// </summary>
        /// <returns></returns>
        public InvestInterestPayment ApproveInterestPayment(InvestInterestPayment input, int tradingProviderId, int status, string approveIp, string username, int? approveNote)
        {
            var interestPayment = _dbSet.FirstOrDefault(i => i.Id == input.Id && i.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestInterestPaymentNotFound);

            var order = _epicSchemaDbContext.InvOrders.FirstOrDefault(o => o.Id == interestPayment.OrderId && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestOrderNotFound, interestPayment.OrderId);
            decimal interestPaymentMoney = interestPayment.AmountMoney;
            // Nếu là chi trả cuối kỳ kiểm tra xem có yêu cầu tái tục hay không
            if (interestPayment.IsLastPeriod == YesNo.YES)
            {
                var renewalsRequest = _epicSchemaDbContext.InvestRenewalsRequests.Where(r => r.OrderId == order.Id && r.SettlementMethod != SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && r.Status != InvestRenewalsRequestStatus.DA_HUY);
                if (renewalsRequest.Any())
                {
                    ThrowException(ErrorCode.InvesInterestPaymentApproveExistRenewalRequest);
                }
                interestPaymentMoney += interestPayment.TotalValueInvestment;
            }

            // Duyệt mới update lại số tiền
            if (status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || status == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
            {
                interestPayment.AmountMoney = interestPaymentMoney;
            }

            // Duyệt không đi tiền hoặc chờ phản hồi từ bank (Chi tự động)
            if (status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || status == InterestPaymentStatus.CHO_PHAN_HOI)
            {
                interestPayment.Status = status;
                interestPayment.ApproveBy = username;
                interestPayment.ApproveDate = DateTime.Now;
                interestPayment.ApproveIp = approveIp;
                interestPayment.ApproveNote = approveNote;
            }
            return interestPayment;
        }

        /// <summary>
        /// Tái tục kỳ cuối
        /// </summary>
        public InvestInterestPaymentRenewalLastPeriodDto RenewalInterestLastPeriodOrder(InvestInterestPayment input, int tradingProviderId, int status, string approveIp, string username, int? approveNote)
        {
            _logger.LogInformation($"{nameof(InvestInterestPaymentEFRepository)} -> {nameof(RenewalInterestLastPeriodOrder)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var interestPayment = _dbSet.FirstOrDefault(i => i.Id == input.Id && i.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestInterestPaymentNotFound);

            var order = _epicSchemaDbContext.InvOrders.FirstOrDefault(o => o.Id == interestPayment.OrderId && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestOrderNotFound, interestPayment.OrderId);

            // Nếu hợp đồng khong trong trạng thái đang đầu tư
            if (order.Status != OrderStatus.DANG_DAU_TU)
            {
                ThrowException(ErrorCode.InvestOrderNotInStatusActive, interestPayment.OrderId);
            }

            // Tìm yêu cầu tái tục hợp đồng và nếu không tồn tại yêu cầu tái tục Exception
            var renewalsRequest = _epicSchemaDbContext.InvestRenewalsRequests.FirstOrDefault(r => r.OrderId == order.Id && r.SettlementMethod != SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && r.Status != InvestRenewalsRequestStatus.DA_HUY);
            if (renewalsRequest == null)
            {
                ThrowException(ErrorCode.InvesOrderRenewalRequestNotExist);
            }

            // Số tiền khi chi trả
            decimal interestPaymentMoney = 0;
            // Tổng số tiền đầu tư sau khi tái tục hợp đồng mới
            decimal initTotalValueOrderNew = order.TotalValue;

            if (renewalsRequest.SettlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC)
            {
                interestPaymentMoney = interestPayment.AmountMoney;
            }
            else if (renewalsRequest.SettlementMethod == SettlementMethod.TAI_TUC_GOC_VA_LOI_NHUAN)
            {
                initTotalValueOrderNew = initTotalValueOrderNew + interestPayment.AmountMoney;
            }

            // Duyệt mới update lại số tiền (Chi tự động)
            if (status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || status == InterestPaymentStatus.DA_DUYET_CHI_TIEN)
            {
                interestPayment.AmountMoney = interestPaymentMoney;
            }

            // Duyệt không đi tiền hoặc chờ phản hồi từ bank
            if (status == InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN || status == InterestPaymentStatus.CHO_PHAN_HOI)
            {
                interestPayment.Status = status;
                interestPayment.ApproveBy = username;
                interestPayment.ApproveDate = DateTime.Now;
                interestPayment.ApproveIp = approveIp;
                interestPayment.ApproveNote = approveNote;
            }
            return new InvestInterestPaymentRenewalLastPeriodDto
            {
                Id = interestPayment.Id,
                InitTotalValueOrderNew = initTotalValueOrderNew,
                SettlementMethod = renewalsRequest.SettlementMethod,
                RenewalsPolicyDetailId = renewalsRequest.RenewalsPolicyDetailId,
                InterestPaymentMoney = interestPaymentMoney,
            };
        }

        public decimal CaculateCashOut(List<int> tradingProviderIds, DateTime? now = null)
        {
            _logger.LogInformation($"{nameof(CaculateCashOut)}: tradingProviderIds={JsonSerializer.Serialize(tradingProviderIds)}; now={now}");

            var result = _dbSet.AsNoTracking().Where(x => (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId ?? 0)) && x.Deleted == YesNo.NO
                            && new int[] { InterestPaymentStatus.DA_DUYET_CHI_TIEN, InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN }.Contains(x.Status ?? 0)
                            && (now == null || (x.PayDate != null && x.PayDate.Value.Date == now.Value.Date))).Sum(x => x.AmountMoney);
            return result;
        }

        #region Tính toán chi trả lợi tức
        /// <summary>
        /// Danh sách ngày trả theo chính sách
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <param name="investDate"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public List<DateTime> GetListPayDateByPolicyDetail(PolicyDetail policyDetail, DateTime investDate, DateTime dueDate)
        {
            investDate = investDate.Date;   
            DateTime now = dueDate.Date;
            List<DateTime> result = new();

            // Số kỳ chi trả tối đa
            int maxPeriod = 0;

            if (policyDetail.PeriodType == PeriodUnit.DAY)
            {
                if (policyDetail.InterestPeriodType == PeriodUnit.DAY)
                {
                    maxPeriod = (policyDetail.PeriodQuantity ?? 0) / (policyDetail.InterestPeriodQuantity ?? 1);
                }
                else if (policyDetail.InterestPeriodType == PeriodUnit.MONTH)
                {
                    maxPeriod = (policyDetail.PeriodQuantity ?? 0) / (policyDetail.InterestPeriodQuantity != null ? 30 * policyDetail.InterestPeriodQuantity.Value : 1);
                }
                else if (policyDetail.InterestPeriodType == PeriodUnit.YEAR)
                {
                    maxPeriod = (policyDetail.PeriodQuantity ?? 0) / (policyDetail.InterestPeriodQuantity != null ? 365 * policyDetail.InterestPeriodQuantity.Value : 1);
                }
            }
            else if (policyDetail.PeriodType == PeriodUnit.MONTH)
            {
                if (policyDetail.InterestPeriodType == PeriodUnit.DAY)
                {
                    maxPeriod = investDate.AddMonths(policyDetail.PeriodQuantity ?? 0).Subtract(investDate).Days / (policyDetail.InterestPeriodQuantity ?? 1);
                }
                else if (policyDetail.InterestPeriodType == PeriodUnit.MONTH)
                {
                    maxPeriod = (policyDetail.PeriodQuantity ?? 0) / (policyDetail.InterestPeriodQuantity ?? 1);
                }
                else if (policyDetail.InterestPeriodType == PeriodUnit.YEAR)
                {
                    maxPeriod = (policyDetail.PeriodQuantity ?? 0) / (12 * (policyDetail.InterestPeriodQuantity ?? 1));
                }
            }
            else if (policyDetail.PeriodType == PeriodUnit.YEAR)
            {
                if (policyDetail.InterestPeriodType == PeriodUnit.DAY)
                {
                    maxPeriod = investDate.AddYears(policyDetail.PeriodQuantity ?? 0).Subtract(investDate).Days / (policyDetail.InterestPeriodQuantity ?? 1);
                }
                else if (policyDetail.InterestPeriodType == PeriodUnit.MONTH)
                {
                    maxPeriod = (12 * policyDetail.PeriodQuantity ?? 0) / (policyDetail.InterestPeriodQuantity ?? 1);
                }
                else if (policyDetail.PeriodType == PeriodUnit.YEAR)
                {
                    maxPeriod = (policyDetail.PeriodQuantity ?? 0) / (policyDetail.InterestPeriodQuantity ?? 1);
                }
            }

            if (policyDetail.InterestType == InterestTypes.DINH_KY)
            {
                var ngayDauKy = investDate.Date;
                //Tính thời gian thực của các kỳ trả
                while (ngayDauKy <= dueDate)
                {
                    DateTime ngayCuoiKy = ngayDauKy.AddDays(policyDetail.InterestPeriodQuantity ?? 0);

                    if (policyDetail.InterestPeriodType == PeriodUnit.MONTH)
                    {
                        ngayCuoiKy = ngayDauKy.AddMonths(policyDetail.InterestPeriodQuantity ?? 0);
                    }
                    else if (policyDetail.InterestPeriodType == PeriodUnit.YEAR)
                    {
                        ngayCuoiKy = ngayDauKy.AddYears(policyDetail.InterestPeriodQuantity ?? 0);
                    }

                    //Chuyển đến kỳ tiếp theo
                    ngayDauKy = ngayCuoiKy;

                    var ngayLamViec = NextWorkDay(ngayCuoiKy.Date, policyDetail.TradingProviderId);

                    //nếu ngày chi trả vượt quá ngày đáo hạn
                    if (ngayLamViec > dueDate)
                    {
                        // Tổng các kỳ đã chi trả được tính chưa bằng Số kỳ chi trả tối đa - maxPeriod
                        if (maxPeriod != 0 && maxPeriod > result.Count())
                        {
                            // Gán làm ngày cuối kỳ
                            ngayLamViec = dueDate;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (result.Count > 1 && ngayLamViec == result[^1]) //trường hợp cộng thừa 1 kỳ
                    {
                        break;
                    }

                    result.Add(ngayLamViec);
                };
            }
            else if (policyDetail.InterestType == InterestTypes.NGAY_CO_DINH)
            {
                for (DateTime curDate = investDate; curDate <= now; curDate = curDate.AddDays(1))
                {
                    //trùng với ngày hàng tháng
                    if (curDate.Date.Day == policyDetail.FixedPaymentDate)
                    {
                        result.Add(curDate);
                    }
                }
            }
            else if (policyDetail.InterestType == InterestTypes.NGAY_DAU_THANG)
            {
                for (DateTime curDate = investDate; curDate <= now; curDate = curDate.AddDays(1))
                {
                    //trùng với ngày đầu tháng
                    if (curDate.Date.Day == 1)
                    {
                        result.Add(curDate);
                    }
                }
            }
            else if (policyDetail.InterestType == InterestTypes.NGAY_CUOI_THANG)
            {
                for (DateTime curDate = investDate; curDate <= now; curDate = curDate.AddDays(1))
                {
                    //trùng với ngày cuối tháng
                    if (curDate.Day == DateTime.DaysInMonth(curDate.Year, curDate.Month))
                    {
                        result.Add(curDate);
                    }
                }
            }

            //ngày kỳ cuối chưa trùng ngày đáo hạn
            if (result.Count > 0 && result[^1] < dueDate)
            {
                result[^1] = dueDate;
            }
            return result;
        }

        /// <summary>
        /// Ngày làm việc tiếp theo
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public DateTime NextWorkDay(DateTime ngayDangXet, int tradingProviderId)
        {
            DateTime ngayLamViecTiepTheo = ngayDangXet.Date;
            while (true)
            {
                var calendar = _epicSchemaDbContext.InvestCalendars.FirstOrDefault(c => c.WorkingDate == ngayLamViecTiepTheo && c.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestCalenderNotFound);
                if (calendar.IsDayOff == YesNo.YES)
                {
                    ngayLamViecTiepTheo = ngayLamViecTiepTheo.AddDays(1);
                }
                else
                {
                    return ngayLamViecTiepTheo;
                }
            }
        }

        /// <summary>
        /// Tính ngày đáo hạn theo chính sách
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <returns></returns>
        public DateTime CalculateDueDate(PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, DateTime? distributionCloseSellDate)
        {
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = policyDetail.PeriodQuantity ?? 0;
            //Ngày đáo hạn
            DateTime ngayDaoHan = ngayBatDauTinhLai.Date;
            if (policyDetail.InterestDays != null) //nếu có cài ngày chính xác
            {
                ngayDaoHan = ngayDaoHan.AddDays(policyDetail.InterestDays.Value);
            }
            else //không cài ngày chính xác
            {
                if (policyDetail.PeriodType == PeriodUnit.DAY)
                {
                    ngayDaoHan = ngayDaoHan.AddDays(soKyDaoHan);
                }
                else if (policyDetail.PeriodType == PeriodUnit.MONTH)
                {
                    ngayDaoHan = ngayDaoHan.AddMonths(soKyDaoHan);
                }
                else if (policyDetail.PeriodType == PeriodUnit.YEAR)
                {
                    ngayDaoHan = ngayDaoHan.AddYears(soKyDaoHan);
                }
            }
            // Kiểm tra xem ngày đáo hạn có vượt quá ngày đóng phân phối distribution
            if (distributionCloseSellDate != null)
            {
                ngayDaoHan = (distributionCloseSellDate.Value.Date < ngayDaoHan) ? distributionCloseSellDate.Value.Date : ngayDaoHan;
            }
            ngayDaoHan = NextWorkDay(ngayDaoHan.Date, policyDetail.TradingProviderId);
            ngayDaoHan = ngayDaoHan.Date;
            return ngayDaoHan;
        }
        #endregion
    }
}
