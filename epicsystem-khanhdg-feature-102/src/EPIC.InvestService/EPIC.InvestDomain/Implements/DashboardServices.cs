using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.Dashboard;
using EPIC.InvestEntities.Dto.ExportExcel;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.InvestDomain.Implements
{
    public class DashboardServices : IDashboardServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<DashboardServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly DashboardRepository _dashboardRepository;
        private readonly InvestInterestPaymentRepository _investInterestPaymentRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly InvestInterestPaymentEFRepository _interestPaymentEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly InvestOrderPaymentEFRepository _investOrderPaymentEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestWithdrawalEFRepository _investWithdrawalEFRepository;
        private readonly ProjectRepository _projectRepository;

        public DashboardServices(
            EpicSchemaDbContext dbContext,
            ILogger<DashboardServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IInvestSharedServices investSharedServices,
            IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _dashboardRepository = new DashboardRepository(_connectionString, _logger);
            _investInterestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _interestPaymentEFRepository = new InvestInterestPaymentEFRepository(dbContext, logger);

            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _investOrderPaymentEFRepository = new InvestOrderPaymentEFRepository(dbContext, logger);
            _investOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _investWithdrawalEFRepository = new InvestWithdrawalEFRepository(dbContext, logger);
            _investSharedServices = investSharedServices;

        }

        /// <summary>
        /// Thông tin sản phẩm của đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<InvestDashboardDistributionPickDto> GetPicklistDistributionByTrading(int? tradingProviderId)
        {
            string usrtype = CommonUtils.GetCurrentUserType(_httpContext);
            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usrtype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            List<InvestDashboardDistributionPickDto> result = new();
            var distribution = _distributionRepository.FindAllOrder(tradingProviderId, null);
            foreach (var item in distribution)
            {
                var project = _projectRepository.FindById(item.ProjectId);
                if (project != null)
                {
                    result.Add(new InvestDashboardDistributionPickDto()
                    {
                        Id = item.Id,
                        Code = project.InvCode,
                        Name = project.InvName
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy thông số invest dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ViewInvDashboard GetInvDashboard(GetInvDashboardDto dto)
        {
            // Ngày hiện tại
            DateTime now = DateTime.Now;
            ViewInvDashboard result = new();
            List<int> tradingProviderIds = new List<int>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                var listTradingProviderIds = _tradingProviderEFRepository.GetAllByPartnerId(partnerId)?.Select(r => r.TradingProviderId).ToList();

                if (dto.TradingProviderId != null && listTradingProviderIds.Contains(dto.TradingProviderId.Value))
                {
                    tradingProviderIds.Add(dto.TradingProviderId.Value);
                }
                else
                {
                    tradingProviderIds = listTradingProviderIds;
                }
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
            result.TienVao = new DashboardTienVaoDto()
            {
                TienVao = DashboardCashInFlow(false, tradingProviderIds, dto.DistributionId, now),
                LuyKe = DashboardCashInFlow(true, tradingProviderIds, dto.DistributionId)
            };

            // Dòng tiền ra
            result.TienRa = new DashboardTienRaDto()
            {
                TienRa = DashboardCashOutFlow(tradingProviderIds, dto.DistributionId, now),
                LuyKe = DashboardCashOutFlow(tradingProviderIds, dto.DistributionId)
            };

            // Số dư
            result.SoDu = new DashboardSoDuDto()
            {
                SoDu = result.TienVao.TienVao - result.TienRa.TienRa,
                LuyKe = result.TienVao.LuyKe - result.TienRa.LuyKe,
            };

            // Dòng tiền theo thời gian
            result.DongTienTheoNgay = DashboardCashflowInDay(tradingProviderIds, dto.FirstDate, dto.EndDate, dto.DistributionId);

            // DS theo kỳ hạn sản phẩm
            result.DanhSachTheoKyHanSP = DashboardCashInPolicyDetail(tradingProviderIds, dto.DistributionId);

            if (userType == UserTypes.ROOT_TRADING_PROVIDER || userType == UserTypes.TRADING_PROVIDER || dto.TradingProviderId != null)
            {
                result.DashboardCashInDepartment = CashflowInDepartmentByTrading(tradingProviderIds, dto.DistributionId, dto.FirstDate, dto.EndDate);
            }

            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                result.DashboardCashTradingByPartner = CashflowTradingProviderByPartner(partnerId, dto.DistributionId, dto.FirstDate, dto.EndDate);
            }
            // Báo cáo thực chi tiền ra
            result.CashOutByMonthInYear = Enumerable.Range(1, 12)
                    .Select(i => new DashboardCashOutByMonthInYearDto
                    {
                        Month = i,
                        Amount = CalculateCashOutByMonth(tradingProviderIds, i, dto.DistributionId)
                    }).ToList();

            result.Actions = _investOrderEFRepository.DashboardGetNewAction(tradingProviderIds);
            return result;
        }

        /// <summary>
        /// Tiền vào
        /// </summary>
        public decimal DashboardCashInFlow(bool isCummulative, List<int> tradingProviderIds, int? distributionId, DateTime? now = null)
        {
            decimal result = _dbContext.InvOrders.AsNoTracking().Where(o => o.Deleted == YesNo.NO && tradingProviderIds.Contains(o.TradingProviderId)
                             && (distributionId == null || o.DistributionId == distributionId)
                             && (o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.TAT_TOAN)
                             && (now == null || (o.InvestDate != null && o.InvestDate.Value.Date == now.Value.Date))).Sum(o => o.InitTotalValue);
            return result;
        }

        /// <summary>
        /// Tiền ra
        /// </summary>
        public decimal DashboardCashOutFlow(List<int> tradingProviderIds, int? distributionId, DateTime? now = null)
        {
            decimal result = 0;
            var interestPaymentMoney = (from order in _dbContext.InvOrders
                                        join interestPayment in _dbContext.InvestInterestPayments on order.Id equals interestPayment.OrderId
                                        where order.Deleted == YesNo.NO && interestPayment.Deleted == YesNo.NO
                                        && new int[] { InterestPaymentStatus.DA_DUYET_CHI_TIEN, InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN }.Contains(interestPayment.Status ?? 0)
                                        && (tradingProviderIds.Any() && tradingProviderIds.Contains(order.TradingProviderId)) && interestPayment.IsLastPeriod == YesNo.YES
                                        && (distributionId == null || order.DistributionId == distributionId)
                                        && (now == null || (interestPayment.PayDate != null && interestPayment.PayDate.Value.Date == now.Value.Date))
                                        select new
                                        {
                                            InterestPaymentMoney = interestPayment.TotalValueInvestment,
                                        }).Sum(r => r.InterestPaymentMoney);

            var withdrawalMoney = (from order in _dbContext.InvOrders.AsNoTracking()
                                   join withdrawal in _dbContext.InvestWithdrawals.AsNoTracking() on order.Id equals withdrawal.OrderId
                                   where order.Deleted == YesNo.NO && withdrawal.Deleted == YesNo.NO && (tradingProviderIds.Any() && tradingProviderIds.Contains(order.TradingProviderId))
                                   && new int[] { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.DUYET_KHONG_DI_TIEN }.Contains(withdrawal.Status ?? 0)
                                   && (now == null || withdrawal.WithdrawalDate.Date == now.Value.Date) && (distributionId == null || order.DistributionId == distributionId)
                                   select withdrawal).Sum(w => w.AmountMoney ?? 0);
            result = interestPaymentMoney + withdrawalMoney;
            return result;
        }

        /// <summary>
        /// Báo cáo doanh số và số dư khu vực
        /// </summary>
        private List<InvestDashboardCashInDepartmentByTrading> CashflowInDepartmentByTrading(List<int> tradingProviderIds, int? distributionId, DateTime? firstDate, DateTime? endDate)
        {
            var tradingProviderId = tradingProviderIds.FirstOrDefault();

            var departmentInTrading = _dbContext.Departments.AsNoTracking().Where(d => d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            List<InvestDashboardCashInDepartmentByTrading> result = new();
            foreach (var item in departmentInTrading)
            {
                var cashInDepartment = CashInByDepartment(item.DepartmentId, distributionId, firstDate, endDate);
                result.Add(new InvestDashboardCashInDepartmentByTrading
                {
                    DepartmentId = item.DepartmentId,
                    Name = item.DepartmentName,
                    Amount = cashInDepartment.totalValue,
                    Remain = cashInDepartment.remain,
                    Type = InvestCashflowDepartmentTypes.Department
                });
            }

            var tienKhachVangLai = _dbContext.InvOrders.Where(o => o.Deleted == YesNo.NO && (o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.TAT_TOAN) && o.SaleReferralCode == null && tradingProviderIds.Contains(o.TradingProviderId)
                                    && (firstDate == null || firstDate.Value.Date <= o.InvestDate.Value.Date) && (endDate == null || endDate.Value.Date >= o.InvestDate.Value.Date));

            var cashflowInTradingSub = (from order in _dbContext.InvOrders.AsNoTracking()
                                        join departmentSub in _dbContext.Departments.AsNoTracking() on order.DepartmentIdSub equals departmentSub.DepartmentId
                                        join tradingProviderSub in _dbContext.TradingProviders.AsNoTracking() on departmentSub.TradingProviderId equals tradingProviderSub.TradingProviderId
                                        join businessCustomeSub in _dbContext.BusinessCustomers.AsNoTracking() on tradingProviderSub.BusinessCustomerId equals businessCustomeSub.BusinessCustomerId
                                        where order.Deleted == YesNo.NO && departmentSub.Deleted == YesNo.NO && tradingProviderSub.Deleted == YesNo.NO && businessCustomeSub.Deleted == YesNo.NO
                                        && (firstDate == null || firstDate.Value.Date <= order.InvestDate.Value.Date) && (endDate == null || endDate.Value.Date >= order.InvestDate.Value.Date)
                                        && order.TradingProviderId == tradingProviderId && (order.Status == OrderStatus.DANG_DAU_TU || (order.Status == OrderStatus.TAT_TOAN && order.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC))
                                        select new
                                        {
                                            order.InitTotalValue,
                                            order.TotalValue,
                                            businessCustomeSub.Name,
                                            businessCustomeSub.ShortName,
                                            tradingProviderSub.TradingProviderId
                                        })
                                       .GroupBy(x => new
                                       {
                                           x.Name,
                                           x.ShortName,
                                           x.TradingProviderId
                                       })
                                       .Select(x => new InvestDashboardCashInDepartmentByTrading
                                       {
                                           TradingProviderIdSub = x.Key.TradingProviderId,
                                           Name = x.Key.Name,
                                           ShortName = x.Key.ShortName,
                                           Amount = x.Sum(t => t.InitTotalValue),
                                           Remain = x.Sum(t => t.TotalValue),
                                           Type = InvestCashflowDepartmentTypes.TradingProvider
                                       });

            result.AddRange(cashflowInTradingSub);
            result.Add(new InvestDashboardCashInDepartmentByTrading()
            {
                Amount = tienKhachVangLai.Sum(o => o.InitTotalValue),
                Name = "Khách hàng vãng lai",
                Remain = tienKhachVangLai.Sum(o => o.TotalValue),
                Type = InvestCashflowDepartmentTypes.TradingProvider
            });
            return result.OrderByDescending(o => o.Amount).ToList();
        }

        /// <summary>
        /// Báo cáo doanh số và số dư của đại lý
        /// </summary>
        public List<InvestDashboardCashTradingByPartner> CashflowTradingProviderByPartner(int partnerId, int? distributionId, DateTime? firstDate, DateTime? endDate)
        {
            var cashflowInTradingByPartner = (from order in _dbContext.InvOrders.AsNoTracking()
                                              join project in _dbContext.InvestProjects.AsNoTracking() on order.ProjectId equals project.Id
                                              join tradingProvider in _dbContext.TradingProviders.AsNoTracking() on order.TradingProviderId equals tradingProvider.TradingProviderId
                                              join businessCustomer in _dbContext.BusinessCustomers.AsNoTracking() on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                              where order.Deleted == YesNo.NO && project.Deleted == YesNo.NO && tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                              && (firstDate == null || firstDate.Value.Date <= order.InvestDate.Value.Date) && (endDate == null || endDate.Value.Date >= order.InvestDate.Value.Date)
                                              && project.PartnerId == partnerId && (order.Status == OrderStatus.DANG_DAU_TU || order.Status == OrderStatus.TAT_TOAN) && (distributionId == null || order.DistributionId == distributionId)
                                              select new
                                              {
                                                  order.InitTotalValue,
                                                  order.TotalValue,
                                                  businessCustomer.Name,
                                                  businessCustomer.ShortName,
                                                  tradingProvider.TradingProviderId
                                              })
                                           .GroupBy(x => new
                                           {
                                               x.Name,
                                               x.ShortName,
                                               x.TradingProviderId
                                           })
                                           .Select(x => new InvestDashboardCashTradingByPartner
                                           {
                                               TradingProviderId = x.Key.TradingProviderId,
                                               Name = x.Key.Name,
                                               ShortName = x.Key.ShortName,
                                               Amount = x.Sum(t => t.InitTotalValue),
                                               Remain = x.Sum(t => t.TotalValue),
                                           });
            return cashflowInTradingByPartner.ToList();
        }

        /// <summary>
        /// Đệ quy doanh số, số dư của các phòng ban theo phân phối
        /// </summary>
        private (decimal totalValue, decimal remain) CashInByDepartment(int departmentId, int? distributionId, DateTime? firstDate, DateTime? endDate)
        {
            decimal totalValue = 0;
            decimal remain = 0;
            // Tổng giá trị đầu tư (doanh số)
            var initTotalValue = _investOrderEFRepository.EntityNoTracking.Where(o => o.DepartmentId == departmentId && (distributionId == null || o.DistributionId == distributionId)
                        && (o.Status == OrderStatus.DANG_DAU_TU || (o.Status == OrderStatus.TAT_TOAN && o.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC))
                        && (firstDate != null && firstDate.Value.Date <= o.InvestDate.Value.Date) && o.Deleted == YesNo.NO
                        && (endDate != null && endDate.Value.Date >= o.InvestDate.Value.Date)
                        ).Sum(o => o.InitTotalValue);

            // Tổng giá trị đầu tư còn lại (số dư)
            var remainTotalValue = _investOrderEFRepository.EntityNoTracking.Where(o => o.DepartmentId == departmentId && (distributionId == null || o.DistributionId == distributionId)
                        && (o.Status == OrderStatus.DANG_DAU_TU || (o.Status == OrderStatus.TAT_TOAN && o.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC))
                        && (firstDate != null && firstDate.Value.Date <= o.InvestDate.Value.Date) && o.Deleted == YesNo.NO
                        && (endDate != null && endDate.Value.Date >= o.InvestDate.Value.Date)
                        ).Sum(o => o.TotalValue);

            totalValue += initTotalValue;
            remain += remainTotalValue;
            var subDepartments = _dbContext.Departments.AsNoTracking().Where(d => d.ParentId == departmentId && d.Deleted == YesNo.NO);
            foreach (var subDepartment in subDepartments)
            {
                var resultSub = CashInByDepartment(subDepartment.DepartmentId, distributionId, firstDate, endDate);
                totalValue += resultSub.totalValue;
                remain += resultSub.remain;
            }
            return (totalValue, remain);
        }

        /// <summary>
        /// Đệ quy doanh số của các phòng ban theo phân phối
        /// </summary>
        private decimal CashInByDepartmentId(int departmentId, int? distributionId)
        {
            decimal result = 0;
            var initTotalValue = _investOrderEFRepository.EntityNoTracking.Where(o => o.DepartmentId == departmentId && o.DistributionId == distributionId
                        && o.Status == OrderStatus.DANG_DAU_TU && o.Deleted == YesNo.NO).Sum(o => o.InitTotalValue);
            result += initTotalValue;
            var subDepartments = _dbContext.Departments.AsNoTracking().Where(d => d.ParentId == departmentId && d.Deleted == YesNo.NO);
            foreach (var subDepartment in subDepartments)
            {
                result += CashInByDepartmentId(subDepartment.DepartmentId, distributionId);
            }
            return result;
        }

        /// <summary>
        /// Doanh số bán hàng theo kỳ hạn sản phẩm 
        /// </summary>
        private List<DashboardDanhSachTheoKyHanSP> DashboardCashInPolicyDetail(List<int> tradingProviderIds, int? distributionId)
        {
            var result = (from distribution in _dbContext.InvestDistributions
                          join policy in _dbContext.InvestPolicies on distribution.Id equals policy.DistributionId
                          join policyDetail in _dbContext.InvestPolicyDetails on policy.Id equals policyDetail.PolicyId
                          join order in _dbContext.InvOrders on policyDetail.Id equals order.PolicyDetailId
                          where distribution.Deleted == YesNo.NO && policy.Deleted == YesNo.NO && policyDetail.Deleted == YesNo.NO
                          && order.Status == OrderStatus.DANG_DAU_TU && (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(order.TradingProviderId))
                          && (distributionId == null || distribution.Id == distributionId) && order.Deleted == YesNo.NO
                          select new
                          {
                              periodQuantity = policyDetail.PeriodQuantity ?? 0,
                              periodType = policyDetail.PeriodType,
                              InitTotalValue = order.TotalValue,
                          })
                         .GroupBy(x => new
                         {
                             x.periodType,
                             x.periodQuantity,
                         })
                         .Select(x => new DashboardDanhSachTheoKyHanSP
                         {
                             PeriodQuantity = x.Key.periodQuantity,
                             PeriodType = x.Key.periodType,
                             TotalValue = x.Sum(s => s.InitTotalValue),
                         }).OrderBy(x => x.PeriodType).ThenBy(x => x.PeriodQuantity).ToList();
            return result;
        }

        /// <summary>
        /// Xem dòng tiền theo ngày
        /// </summary>
        private List<DashboardDongTienTheoNgayDto> DashboardCashflowInDay(List<int> tradingProviderIds, DateTime? startDate, DateTime? endDate, int? distributionId = null)
        {
            // Dòng tiền theo thời gian
            var result = new List<DashboardDongTienTheoNgayDto>();
            var date = startDate;
            while (date <= endDate)
            {
                decimal value = 0;
                var doanhSo = _investOrderEFRepository.EntityNoTracking.Where(o => tradingProviderIds.Contains(o.TradingProviderId) && o.Status == OrderStatus.DANG_DAU_TU
                                && (distributionId == null || o.DistributionId == distributionId)
                                && o.Deleted == YesNo.NO && (date == null || (o.InvestDate != null && o.InvestDate.Value.Date == date.Value.Date))).Sum(o => o.InitTotalValue);
                var chiTraLoiTuc = _interestPaymentEFRepository.CaculateCashOut(tradingProviderIds, date);
                var soTienRut = _investWithdrawalEFRepository.DashboardCaculateCashOut(tradingProviderIds, date);
                value = doanhSo - chiTraLoiTuc - soTienRut;
                result.Add(new DashboardDongTienTheoNgayDto
                {
                    Date = date,
                    TotalValue = value
                });
                date = date.Value.AddDays(1);
            }
            return result;
        }

        /// <summary>
        /// Tính dòng tiền đã chi Theo Tháng trong năm  
        /// </summary>
        public decimal CalculateCashOutByMonth(List<int> tradingProviderIds, int month, int? distributionId = null)
        {
            decimal result = 0;
            // Lấy giá trị được rút hoặc chi được lập sau khi duyệt ở bảng Thanh toán
            result = (from payment in _dbContext.InvestOrderPayments.AsNoTracking()
                      join order in _dbContext.InvOrders.AsNoTracking() on payment.OrderId equals order.Id
                      where order.MethodInterest == InvestMethodInterests.DoPayment
                      && order.Deleted == YesNo.NO
                      && payment.Deleted == YesNo.NO
                      && payment.Status == OrderPaymentStatus.DA_THANH_TOAN
                      && payment.TranType == TranTypes.CHI
                      && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                      && (distributionId == null || order.DistributionId == distributionId)
                      && order.Deleted == YesNo.NO
                      && (payment.ApproveDate != null && payment.ApproveDate.Value.Date.Month == month && payment.ApproveDate.Value.Year == DateTime.Now.Year)
                      select payment).Sum(p => p.PaymentAmnount ?? 0);
            return result;
        }
    }
}
