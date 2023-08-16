using Dapper.Oracle;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPIC.InvestRepositories
{
    public class InvestInterestPaymentRepository : BaseRepository
    {
        private const string PROC_INTEREST_PAYMENT_GET_ALL = "PKG_INV_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_GET_ALL";
        private const string PROC_INTEREST_PAYMENT_GET = "PKG_INV_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_GET";
        private const string PROC_CHANGE_STATUS_1_TO_2 = "PKG_INV_INTEREST_PAYMENT.PROC_CHANGE_STATUS_1_TO_2";
        private const string PROC_INTEREST_PAYMENT_ADD = "PKG_INV_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_ADD";
        private const string PROC_INTEREST_PAYMENT_RENEWALS = "PKG_INV_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_RENEWALS";
        private const string PROC_ORDER_GET = "PKG_INV_ORDER.PROC_ORDER_GET";
        private const string PROC_INTEREST_PAYMENT_GET_LIST = "PKG_INV_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_GET_LIST";
        private const string PROC_CALENDAR_NEXT_WORK_DAY = "PKG_INV_CALENDAR.PROC_CALENDAR_NEXT_WORK_DAY";
        private const string PROC_GET_ALL_INTEREST_PAYMENT_CLOSEST_PERIOD = "PKG_INV_INTEREST_PAYMENT.PROC_GET_ALL_INTEREST_PAYMENT_CLOSEST_PERIOD";
        private const string PROC_RENEWALS_LAST_PERIOD_ORDER = "PKG_INV_INTEREST_PAYMENT.PROC_RENEWALS_LAST_PERIOD_ORDER";
        private const string PROC_INVEST_DUE_EXPEND_REPORT = "PKG_INV_EXCEL_REPORT.INV_DUE_EXPEND";

        public InvestInterestPaymentRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public InvestInterestPayment InterestPaymentAdd(InvestInterestPayment entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestInterestPayment>(PROC_INTEREST_PAYMENT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_PERIOD_INDEX = entity.PeriodIndex,
                pv_CIF_CODE = entity.CifCode,
                pv_PROFIT = entity.Profit,
                pv_AMOUNT_MONEY = entity.AmountMoney,
                pv_TAX = entity.Tax,
                pv_TOTAL_VALUE_INVESTMENT = entity.TotalValueInvestment,
                pv_POLICY_DETAIL_ID = entity.PolicyDetailId,
                pv_PAY_DATE = entity.PayDate,
                pv_IS_LAST_PERIOD = entity.IsLastPeriod,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }

        public PagingResult<InvestInterestPayment> FindAll(InterestPaymentFilterDto input, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvestInterestPayment>(PROC_INTEREST_PAYMENT_GET_ALL, new
            {
                PAGE_SIZE = -1, // Phân trang trong services 
                PAGE_NUMBER = input.PageNumber,
                KEY_WORD = input.Keyword,
                pv_STATUS = input.Status,
                pv_INTEREST_PAYMENT_STATUS = input.InterestPaymentStatus,
                pv_NGAYCHITRA = input.NgayChiTra,
                pv_IS_EXACT_DATE = input.IsExactDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PHONE = input.Phone,
                pv_CONTRACT_CODE = input.ContractCode,
                pv_CIF_CODE = input.CifCode,
                pv_IS_LAST_PERIOD = input.IsLastPeriod,
                pv_TRADING_PROVIDER_CHILD_IDS = input.TradingProviderIds != null ? string.Join(',', input.TradingProviderIds) : null,
                pv_METHOD_INTEREST = input.MethodInterest,
            });
            return result;
        }

        public InvestInterestPayment FindById(int id, int? tradingProviderId = null, bool isClose = true)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<InvestInterestPayment>(PROC_INTEREST_PAYMENT_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }, isClose);
            return result;
        }

        /// <summary>
        /// Lấy list thông tin hợp đồng để tính danh sách dự chi
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<DueExpendReport> ListOrderDueExpend(int? partnerId, int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<DueExpendReport>(PROC_INVEST_DUE_EXPEND_REPORT, new
            {
                pv_PARTNER_ID = partnerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }).ToList();
            return result;
        }
        public List<InvestInterestPayment> InterestPaymentGetList(long orderId, int? status = null, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedure<InvestInterestPayment>(PROC_INTEREST_PAYMENT_GET_LIST, new
            {
                pv_ORDER_ID = orderId,
                pv_STATUS = status,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }).ToList();
            return result;
        }

        /// <summary>
        /// Chuyển status từ đã lập chưa chi trả sang đã chi trả
        /// (đang comment lại: Tái tục vốn nếu có)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public InvestInterestPayment ApproveInterestPayment(int? id, int? tradingProviderId, DateTime investDateNew, string approveIp, string username, bool isClose = true)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<InvestInterestPayment>(PROC_CHANGE_STATUS_1_TO_2, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVEST_DATE_NEW = investDateNew,
                pv_APPROVE_IP = approveIp,
                SESSION_USERNAME = username
            }, isClose);
            return result;
        }

        /// <summary>
        /// Lấy ngày trả của các hợp đồng ở kì gần nhất
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public DueExpendReport GetInterestPaymentPayDateClosestPeriod(int? tradingProviderId, int? orderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<DueExpendReport>(PROC_GET_ALL_INTEREST_PAYMENT_CLOSEST_PERIOD, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        /// <summary>
        /// Chi trả tái tục kỳ cuối
        /// </summary>
        /// <returns></returns>
        public InvestInterestPaymentApproveRenewalDto InterestPaymentLastPeriodOrder(int id, int? tradingProviderId, DateTime investDateNew, string approveIp, string username, bool isClose = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestInterestPaymentApproveRenewalDto>(PROC_RENEWALS_LAST_PERIOD_ORDER, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVEST_DATE_NEW = investDateNew,
                pv_APPROVE_IP = approveIp,
                SESSION_USERNAME = username
            }, isClose);
        }

        #region Một vài Proc được gọi lại để dùng transaction
        public InvOrder FindOrderById(long id, int? tradingProviderId = null, bool isClose = true)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<InvOrder>(PROC_ORDER_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }, isClose);
            return result;
        }
        public DateTime CalculateDueDate(PolicyDetail policyDetail, DateTime ngayBatDauTinhLai)
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
            ngayDaoHan = NextWorkDay(ngayDaoHan, policyDetail.TradingProviderId, false); //kiểm tra nếu trùng ngày nghỉ thì cộng lên
            return ngayDaoHan;
        }

        public DateTime NextWorkDay(DateTime workingDate, int tradingProviderId, bool isClose = true)
        {
            DateTime result = DateTime.Now;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_WORKING_DATE", workingDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Date, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_CALENDAR_NEXT_WORK_DAY, parameters, isClose);

            result = parameters.Get<DateTime>("pv_RESULT");
            return result;
        }
        #endregion
    }
}

