using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestCalendarEFRepository : BaseEFRepository<Calendar>
    {
        public InvestCalendarEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {
        }

        /// <summary>
        /// Check ngày nghỉ lễ
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public bool CheckHoliday(DateTime ngayDangXet, int tradingProviderId)
        {
            var calendar = _dbSet.FirstOrDefault(c => c.WorkingDate == ngayDangXet && c.TradingProviderId == tradingProviderId)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.GarnerCalenderNotFound);

            return calendar.IsDayOff == YesNo.YES;
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
                if (CheckHoliday(ngayLamViecTiepTheo, tradingProviderId))
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
    }
}
