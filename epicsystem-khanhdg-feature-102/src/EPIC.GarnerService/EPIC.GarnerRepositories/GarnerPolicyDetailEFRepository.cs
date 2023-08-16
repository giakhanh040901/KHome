using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.DataAccess.Base;
using EPIC.Entities.Dto.AssetManager;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerPolicyDetailEFRepository : BaseEFRepository<GarnerPolicyDetail>
    {
        public GarnerPolicyDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerPolicyDetail.SEQ}")
        {
        }

        /// <summary>
        /// Thêm kỳ hạn 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerPolicyDetail Add(GarnerPolicyDetail input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId= {tradingProviderId}, username: {username}");

            var polidyDetailInsert = new GarnerPolicyDetail
            {
                Id = (int)NextKey(),
                DistributionId = input.DistributionId,
                TradingProviderId = tradingProviderId,
                PolicyId = input.PolicyId,
                SortOrder = input.SortOrder,
                ShortName = input.ShortName,
                Name = input.Name,
                Status = Status.ACTIVE,
                Profit = input.Profit,
                InterestDays = input.InterestDays,
                PeriodType = input.PeriodType,
                PeriodQuantity = input.PeriodQuantity,
                InterestType = input.InterestType,
                InterestPeriodType = input.InterestPeriodType,
                InterestPeriodQuantity = input.InterestPeriodQuantity,
                RepeatFixedDate = input.RepeatFixedDate,
                CreatedBy = username
            };
            return _dbSet.Add(polidyDetailInsert).Entity;
        }

        /// <summary>
        /// Update kỳ hạn
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerPolicyDetail UpdatePolicyDetail(GarnerPolicyDetail input, string username, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(UpdatePolicyDetail)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username} ");

            var getPolicyDetail = _dbSet.FirstOrDefault(e => e.Id == input.Id && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
            if (getPolicyDetail != null)
            {
                getPolicyDetail.SortOrder = input.SortOrder;
                getPolicyDetail.ShortName = input.ShortName;
                getPolicyDetail.Name = input.Name;
                getPolicyDetail.IsShowApp = input.IsShowApp;
                getPolicyDetail.Status = input.Status;
                getPolicyDetail.Profit = input.Profit;
                getPolicyDetail.InterestDays = input.InterestDays;
                getPolicyDetail.PeriodType = input.PeriodType;
                getPolicyDetail.PeriodQuantity = input.PeriodQuantity;
                getPolicyDetail.InterestType = input.InterestType;
                getPolicyDetail.InterestPeriodType = input.InterestPeriodType;
                getPolicyDetail.InterestPeriodQuantity = input.InterestPeriodQuantity;
                getPolicyDetail.RepeatFixedDate = input.RepeatFixedDate;
                getPolicyDetail.ModifiedBy = username;
                getPolicyDetail.ModifiedDate = DateTime.Now;
            }
            return getPolicyDetail;
        }

        /// <summary>
        /// Tìm kiếm theo Id
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerPolicyDetail FindById(int policyDetailId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: PolicyDetailId = {policyDetailId}, tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(x => x.Id == policyDetailId && (tradingProviderId == null || x.TradingProviderId == tradingProviderId) && x.Deleted == YesNo.NO);

        }

        public List<GarnerPolicyDetail> GetAllPolicyDetailByPolicyId(int policyId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GetAllPolicyDetailByPolicyId)}: PolicyId = {policyId}");

            var policyDetailList = from gpd in _dbSet
                                   join gp in _epicSchemaDbContext.GarnerPolicies on gpd.PolicyId equals gp.Id
                                   where gp.Id == policyId && (tradingProviderId == null || gp.TradingProviderId == tradingProviderId) && gp.Deleted == YesNo.NO && gpd.Deleted == YesNo.NO
                                   orderby gpd.Id descending
                                   select gpd;

            return policyDetailList.ToList();
        }

        /// <summary>
        /// Tìm ra kỳ hạn còn active thông qua số ngày (tra bậc), không tìm thấy sẽ trả ra null
        /// </summary>
        /// <param name="policyId">Id chính sách</param>
        /// <param name="startDate">Thời gian bắt đầu</param>
        /// <param name="now">Thời gian hiện tại</param>
        /// <returns></returns>
        public GarnerPolicyDetail FindPolicyDetailByDate(int policyId, DateTime startDate, DateTime now)
        {
            _logger.LogInformation($"{nameof(FindPolicyDetailByDate)}: PolicyId = {policyId}, startDate = {startDate}, now = {now}");

            var policyDetails = _epicSchemaDbContext.GarnerPolicyDetails
                .Where(pd => pd.PolicyId == policyId && pd.Deleted == YesNo.NO && pd.Status == Status.ACTIVE)
                .OrderBy(pd => pd.PeriodType)
                .ThenBy(pd => pd.PeriodQuantity)
                .ToList();

            var result = new GarnerPolicyDetail(); //mặc định vào cái đầu tiên

            foreach (var policyDetail in policyDetails)
            {
                DateTime dueDate = CalculateDueDate(startDate, policyDetail.PeriodQuantity, policyDetail.PeriodType);
                if (now >= dueDate) //đến khoảng nào thì lấy khoảng đó
                {
                    result = policyDetail;
                }
            }
            return result;
        }

        /// <summary>
        /// Ngày trả lãi tiếp theo
        /// </summary>
        /// <param name="currDate"></param>
        /// <param name="periodQuantity"></param>
        /// <param name="periodType"></param>
        /// <returns></returns>
        public DateTime NextInterestDate(DateTime currDate, int periodQuantity, string periodType)
        {
            currDate = currDate.Date;
            DateTime dueDate = currDate;
            if (periodType == PeriodUnit.DAY)
            {
                dueDate = currDate.AddDays(periodQuantity);
            }
            else if (periodType == PeriodUnit.MONTH)
            {
                dueDate = currDate.AddMonths(periodQuantity);
            }
            else if (periodType == PeriodUnit.YEAR)
            {
                dueDate = currDate.AddYears(periodQuantity);
            }
            return dueDate;
        }

        /// <summary>
        /// Tính ngày cuối kỳ
        /// </summary>
        /// <param name="activeDate"></param>
        /// <param name="periodQuantity"></param>
        /// <param name="periodType"></param>
        /// <returns></returns>
        public DateTime CalculateDueDate(DateTime activeDate, int periodQuantity, string periodType)
        {
            return NextInterestDate(activeDate, periodQuantity, periodType);
        }

        /// <summary>
        /// Danh sách ngày trả theo chính sách
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="investDate"></param>
        /// <param name="now">Tính danh sách đến hạn chi trả đến ngày chọn (nếu truyền vào)</param>
        /// <returns></returns>
        public List<DateTime> GetListPayDateByPolicy(GarnerPolicy policy, DateTime investDate, DateTime? toDate = null)
        {
            investDate = investDate.Date;
            DateTime now = toDate == null ? DateTime.Now.Date : toDate.Value.Date;
            List<DateTime> result = new();

            if (policy.GarnerType == GarnerPolicyTypes.KHONG_CHON_KY_HAN)
            {
                if (policy.InterestType == InterestTypes.DINH_KY)
                {
                    //lặp để lấy
                    DateTime cur = investDate;
                    while (true)
                    {
                        cur = NextInterestDate(investDate, policy.InterestPeriodQuantity ?? 0, policy.InterestPeriodType);
                        if (cur > now)
                        {
                            break;
                        }
                        result.Add(cur);
                    }
                }
                else if (policy.InterestType == InterestTypes.CUOI_KY)
                {
                    if (policy.InterestPeriodQuantity != null && policy.InterestPeriodType != null)
                    {
                        //tính ngày cuối kỳ
                        result.Add(CalculateDueDate(investDate, policy.InterestPeriodQuantity ?? 0, policy.InterestPeriodType));
                    }    
                }
                else if (policy.InterestType == InterestTypes.NGAY_CO_DINH)
                {
                    for (DateTime curDate = investDate; curDate <= now; curDate = curDate.AddDays(1))
                    {
                        //trùng với ngày hàng tháng
                        if (curDate.Date.Day == policy.RepeatFixedDate)
                        {
                            result.Add(curDate);
                        }
                    }
                }
                else if (policy.InterestType == InterestTypes.NGAY_DAU_THANG)
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
                else if (policy.InterestType == InterestTypes.NGAY_CUOI_THANG)
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
            }
            else if (policy.GarnerType == GarnerPolicyTypes.CHON_KY_HAN)
            {

            }
            return result;
        }

        //public int CalculateDefaultNumberInvestDays(GarnerPolicyDetail policyDetail)
        //{

        //}
    }
}
