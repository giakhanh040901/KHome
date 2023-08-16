using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileEntities.Settings;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerDashboard;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Implements;
using EPIC.InvestEntities.Dto.Dashboard;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerDashboardServices : IGarnerDashboardServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerDashboardServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        private readonly GarnerWithdrawalEFRepository _garnerWithdrawalEFRepository;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;

        public GarnerDashboardServices(EpicSchemaDbContext dbContext,
                DatabaseOptions databaseOptions,
                IMapper mapper,
                ILogger<GarnerDashboardServices> logger,
                IHttpContextAccessor httpContextAccessor,
                IGarnerFormulaServices garnerFormulaServices
                )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _garnerWithdrawalEFRepository = new GarnerWithdrawalEFRepository(dbContext, logger);
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Get thông tin dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public GarnerDashboardDto Dashboard(GarnerDashboardFindDto dto)
        {
            // Ngày hiện tại
            DateTime now = DateTime.Now;
            GarnerDashboardDto result = new();
            List<int> tradingProviderIds = new List<int>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                tradingProviderIds = _tradingProviderEFRepository.GetAllByPartnerId(partnerId)?.Select(r => r.TradingProviderId).ToList();
            }
            else if (userType == UserTypes.ROOT_TRADING_PROVIDER || userType == UserTypes.TRADING_PROVIDER)
            {
                tradingProviderIds.Add(CommonUtils.GetCurrentTradingProviderId(_httpContext));
            }    
            else if (userType != UserTypes.ROOT_EPIC || userType == UserTypes.EPIC)
            {
                return result;
            }    

            // Dòng tiền vào
            result.CashInFlow = new GarnerDashboardOverviewDto()
            {
                MoneyDay = CashInFlow(false, tradingProviderIds, now),
                Cummulative = CashInFlow(true, tradingProviderIds)
            };

            // Dòng tiền ra
            result.CashOutFlow = new GarnerDashboardOverviewDto()
            {
                MoneyDay = CashOutFlow(tradingProviderIds, now),
                Cummulative = CashOutFlow(tradingProviderIds)
            };

            // Số dư
            result.CashRemain = new GarnerDashboardOverviewDto()
            {
                MoneyDay = result.CashInFlow.MoneyDay - _garnerWithdrawalEFRepository.CaculateCashOut(tradingProviderIds, now),
                Cummulative = result.CashInFlow.Cummulative - _garnerWithdrawalEFRepository.CaculateCashOut(tradingProviderIds),
            };

            // Dòng tiền theo thời gian
            var timeflow = new List<GarnerDashboardTimeFlowDto> { };
            var date = dto.FirstDate;
            while (date <= dto.EndDate)
            {
                decimal value = 0;

                var doanhSo = CashInFlow(false, tradingProviderIds, date);
                var chiTraLoiTuc = _garnerInterestPaymentEFRepository.CalculateInterestPayment(tradingProviderIds, date);
                var soTienRut = _garnerWithdrawalEFRepository.CaculateCashOut(tradingProviderIds, date);
                var loiTucRut = _garnerWithdrawalEFRepository.CaculateProfitOut(tradingProviderIds, date);

                value = doanhSo - chiTraLoiTuc - soTienRut - loiTucRut;

                timeflow.Add(new GarnerDashboardTimeFlowDto
                {
                    Date = date,
                    Value = value
                });
                date = date.AddDays(1);
            }

            result.TimeFlow = timeflow;

            // Doanh số theo kỳ hạn
            result.ListPolicy = _garnerOrderPaymentEFRepository.CalculatePaymentByPolicy(tradingProviderIds, dto.ProductId);

            // Doanh số theo phòng ban (nếu là đại lý)
            if (userType == UserTypes.ROOT_TRADING_PROVIDER || userType == UserTypes.TRADING_PROVIDER)
            {
                var tienVao = _garnerOrderPaymentEFRepository.CalculateCashInByTrading(tradingProviderIds.First(), dto.FirstDate, dto.EndDate, dto.ProductId).ToDictionary(x => x.DepartmentId, x => x);
                var tienRa = _garnerWithdrawalEFRepository.CalculateCashOutByTrading(tradingProviderIds.First(), dto.FirstDate, dto.EndDate, dto.ProductId).ToDictionary(x => x.DepartmentId, x => x);

                result.CashInByTrading = new List<GarnerDashboardCashInByTrading>() { };

                foreach (var tmpTienVao in tienVao)
                {
                    decimal tienRaAmount = 0;
                    if (tienRa.ContainsKey(tmpTienVao.Value.DepartmentId))
                    {
                        tienRaAmount = tienRa[tmpTienVao.Value.DepartmentId].Amout;
                    }

                    result.CashInByTrading.Add(new GarnerDashboardCashInByTrading
                    {
                        Amout = tmpTienVao.Value.Amout,
                        DepartmentId = tmpTienVao.Value.DepartmentId,
                        DepartmentName = tmpTienVao.Value.DepartmentName,
                        Remain = tmpTienVao.Value.Amout - tienRaAmount,
                    });
                }

                var tienKhachVangLai = _dbContext.GarnerOrders.Where(o => o.Deleted == YesNo.NO && (o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.TAT_TOAN) && o.DepartmentId == null && tradingProviderIds.Contains(o.TradingProviderId)
                                    && (dto.FirstDate.Date <= o.InvestDate.Value.Date) && (dto.EndDate.Date >= o.InvestDate.Value.Date));
                result.CashInByTrading.Add(new GarnerDashboardCashInByTrading()
                {
                    Amout = tienKhachVangLai.Sum(o => o.InitTotalValue),
                    DepartmentName = "Khách hàng vãng lai",
                    Remain = tienKhachVangLai.Sum(o => o.TotalValue),
                });
                result.CashInByTrading = result.CashInByTrading.OrderByDescending(o => o.Amout).ToList();
            }

            // Doanh số theo phòng ban (nếu là partner)
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                var tienVao = _garnerOrderPaymentEFRepository.CalculateCashInByPartner(tradingProviderIds, dto.FirstDate, dto.EndDate, dto.ProductId).ToDictionary(x => x.TradingProviderId, x => x);
                var tienRa = _garnerWithdrawalEFRepository.CalculateCashOutByPartner(tradingProviderIds, dto.FirstDate, dto.EndDate, dto.ProductId).ToDictionary(x => x.TradingProviderId, x => x);

                result.CashInByPartner = new List<GarnerDashboardCashInByPartner>() { };

                foreach (var tmpTienVao in tienVao)
                {
                    decimal tienRaAmount = 0;
                    if (tienRa.ContainsKey(tmpTienVao.Value.TradingProviderId))
                    {
                        tienRaAmount = tienRa[tmpTienVao.Value.TradingProviderId].Amout;
                    }

                    result.CashInByPartner.Add(new GarnerDashboardCashInByPartner
                    {
                        Amout = tmpTienVao.Value.Amout,
                        TradingProviderId = tmpTienVao.Value.TradingProviderId,
                        TradingProviderName = tmpTienVao.Value.TradingProviderName,
                        TradingProviderShortName = tmpTienVao.Value.TradingProviderShortName,
                        Remain = tmpTienVao.Value.Amout - tienRaAmount,
                    });
                }
            }

            // Thực chi theo tháng
            result.CashOutByMonths = new List<GarnerDashboardCashOutByMonth>();
            for (int i = 1; i < 13; i++)
            {
                result.CashOutByMonths.Add(new GarnerDashboardCashOutByMonth
                {
                    Month = i,
                    Amount = _garnerOrderPaymentEFRepository.CalculateCashOutByMonth(tradingProviderIds, i)
                });
            }

            // Hành động người dùng cục bên phải
            result.Actions = _garnerOrderEFRepository.DashboardGetNewAction(tradingProviderIds)
                            ?.Concat(_garnerWithdrawalEFRepository.DashboardGetNewAction(tradingProviderIds))
                            .OrderByDescending(x => x.CreatedDate)
                            .Take(10)
                            .ToList();

            return result;
        }

        /// <summary>
        /// Dòng tiền vào
        /// </summary>
        /// <param name="isCummulative"></param>
        /// <param name="tradingProviderIds"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        private decimal CashInFlow(bool isCummulative, List<int> tradingProviderIds, DateTime? now = null)
        {
            decimal result = 0;
            if (isCummulative)
            {
                result = (from payment in _dbContext.GarnerOrderPayments
                          join order in _dbContext.GarnerOrders on payment.OrderId equals order.Id
                          where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.THU
                          && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                          && order.Deleted == YesNo.NO
                          select payment).Sum(p => p.PaymentAmount);
                // Tổng số tiền của các hợp đồng đang active
                //var moneyOrderActive = _garnerOrderEFRepository.Entity.Where(r => r.Status == OrderStatus.DANG_DAU_TU
                //                                                && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(r.TradingProviderId))
                //                                                && r.Deleted == YesNo.NO).Sum(o => o.TotalValue);

                //// Tổng số tiền của các chi trả được duyệt nhưng chưa active hợp đồng
                //var moneyPaymentApprove = (from payment in _dbContext.GarnerOrderPayments
                //                           join order in _dbContext.GarnerOrders on payment.OrderId equals order.Id
                //                           where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.THU
                //                           && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                //                           && (new List<int> { OrderStatus.CHO_KY_HOP_DONG, OrderStatus.CHO_DUYET_HOP_DONG, OrderStatus.CHO_THANH_TOAN }).Contains(order.Status) && order.Deleted == YesNo.NO
                //                           select payment).Sum(p => p.PaymentAmount);
                //result = moneyOrderActive + moneyPaymentApprove;
            }
            else
            {
                // Tổng số tiền được duyệt thanh toán trong ngày
                result = (from payment in _dbContext.GarnerOrderPayments
                          join order in _dbContext.GarnerOrders on payment.OrderId equals order.Id
                          where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.THU
                          && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                          && order.Deleted == YesNo.NO && (now == null || order.PaymentFullDate != null && order.PaymentFullDate.Value.Date == now.Value.Date)
                          select payment).Sum(p => p.PaymentAmount);

            }
            return result;
        }

        /// <summary>
        /// Dòng tiền ra
        /// </summary>
        private decimal CashOutFlow(List<int> tradingProviderIds,  DateTime? now = null)
        {
            decimal result = 0;
            // Lấy giá trị được rút hoặc chi được lập sau khi duyệt ở bảng Thanh toán
            result = (from payment in _dbContext.GarnerOrderPayments
                      join order in _dbContext.GarnerOrders on payment.OrderId equals order.Id
                      where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.CHI
                      && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                      && order.Deleted == YesNo.NO && (now == null || payment.ApproveDate != null && payment.ApproveDate.Value.Date == now.Value.Date)
                      select payment).Sum(p => p.PaymentAmount);

            return result;
        }

        /// <summary>
        /// Lấy list product để fe chọn trong màn dashboard
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerDashboardProductPickListDto> GetPicklistProductByTrading(int? tradingProviderId)
        {
            string usrtype = CommonUtils.GetCurrentUserType(_httpContext);
            if (new string[] {UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER}.Contains(usrtype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var result = _garnerProductEFRepository.GetListProductByTradingProvider(tradingProviderId ?? 0);

            return _mapper.Map<List<GarnerDashboardProductPickListDto>>(result);
        }

    }
}
