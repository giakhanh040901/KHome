using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.Dashboard;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Records;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.CoreDomain.Implements
{
    public class DashboardServices : IDashboardServices
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly InvestorRegisterLogEFRepository _investorRegisterLogEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;

        public DashboardServices(
            EpicSchemaDbContext dbContext,
            ILogger<DashboardServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _investorRegisterLogEFRepository = new InvestorRegisterLogEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
        }

        public DashboardDto Dashboard(FilterCoreDashboardDto dto)
        {
            var result = new DashboardDto();

            List<int> tradingProviderIds = new List<int>();
            int? partnerId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                var listTradingProviderInPartners = from trading in _dbContext.TradingProviders
                                                    join businessCustomer in _dbContext.BusinessCustomers on trading.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                                    join tradingParner in _dbContext.TradingProviderPartners on trading.TradingProviderId equals tradingParner.TradingProviderId
                                                    where tradingParner.PartnerId == partnerId && trading.Deleted == YesNo.NO && tradingParner.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                                    select trading.TradingProviderId;
                if (dto.TradingProviderId != null)
                {
                    tradingProviderIds.Add(dto.TradingProviderId ?? 0);
                }
                else
                {
                    tradingProviderIds.AddRange(listTradingProviderInPartners);
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
            _logger.LogInformation($"{nameof(Dashboard)}: input = {JsonSerializer.Serialize(dto)}, partnerId = {partnerId}, tradingProviderIds = {JsonSerializer.Serialize(tradingProviderIds)}");

            result.DashboardOverview = DashboardOverview(tradingProviderIds, dto.StartDate, dto.EndDate, dto.TradingProviderId);
            result.FunnelCharts = FunnelChart();
            result.GenderCustomerCharts = new List<GenderCustomerChartDto>();
            result.GenerationCustomerCharts = new List<GenerationCustomerChartDto>();
            for (int i = 1; i < 13; i++)
            {
                result.GenderCustomerCharts.Add(new GenderCustomerChartDto()
                {
                    Month = (i < 10) ? $"0{i}/{DateTime.Now.Year}" : $"{i}/{DateTime.Now.Year}",
                    Male = CaculateTotalCustomerByGender(tradingProviderIds, i, Genders.MALE, dto.TradingProviderId),
                    Female = CaculateTotalCustomerByGender(tradingProviderIds, i, Genders.FEMALE, dto.TradingProviderId),
                    Total = CaculateTotalCustomerByGender(tradingProviderIds, i, Genders.MALE, dto.TradingProviderId) + CaculateTotalCustomerByGender(tradingProviderIds, i, Genders.FEMALE, dto.TradingProviderId),
                });
                result.GenerationCustomerCharts.Add(new GenerationCustomerChartDto()
                {
                    Month = (i < 10) ? $"0{i}/{DateTime.Now.Year}" : $"{i}/{DateTime.Now.Year}",
                    Millennial = CaculateTotalCustomerByGeneration(tradingProviderIds, i, new DateTime(1981,01,01), new DateTime(1997, 01, 01), dto.TradingProviderId),
                    BabyBoomer = CaculateTotalCustomerByGeneration(tradingProviderIds, i, new DateTime(1943, 01, 01), new DateTime(1965, 01, 01), dto.TradingProviderId),
                    GenerationX = CaculateTotalCustomerByGeneration(tradingProviderIds, i, new DateTime(1965, 01, 01), new DateTime(1981, 01, 01), dto.TradingProviderId),
                    GenerationZ = CaculateTotalCustomerByGeneration(tradingProviderIds, i, new DateTime(1997, 01, 01), new DateTime(2016, 01, 01), dto.TradingProviderId)
                });
            }
            return result;
        }

        /// <summary>
        /// Danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public PagingResult<CustomerListDto> CustomerList(FilterCoreDashboardDto dto)
        {
            List<int> tradingProviderIds = new List<int>();
            int? partnerId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                var listTradingProviderInPartners = from trading in _dbContext.TradingProviders
                                                    join businessCustomer in _dbContext.BusinessCustomers on trading.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                                    join tradingParner in _dbContext.TradingProviderPartners on trading.TradingProviderId equals tradingParner.TradingProviderId
                                                    where tradingParner.PartnerId == partnerId && trading.Deleted == YesNo.NO && tradingParner.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                                    select trading.TradingProviderId;
                if (dto.TradingProviderId != null)
                {
                    tradingProviderIds.Add(dto.TradingProviderId ?? 0);
                }
                else
                {
                    tradingProviderIds.AddRange(listTradingProviderInPartners);
                }
            }
            else if (userType == UserTypes.ROOT_TRADING_PROVIDER || userType == UserTypes.TRADING_PROVIDER)
            {
                tradingProviderIds.Add(CommonUtils.GetCurrentTradingProviderId(_httpContext));
            }
            else if (userType != UserTypes.ROOT_EPIC || userType == UserTypes.EPIC)
            {
                return null;
            }

            _logger.LogInformation($"{nameof(CustomerList)}: input = {JsonSerializer.Serialize(dto)}, partnerId = {partnerId}, tradingProviderIds = {JsonSerializer.Serialize(tradingProviderIds)}");
            var customerList = new List<CustomerListDto>();

            var customerOrder = (from cifCode in _dbContext.CifCodes
                              join investor in _dbContext.Investors on cifCode.InvestorId equals investor.InvestorId into investors
                              from investor in investors.DefaultIfEmpty()
                              let identification = _dbContext.InvestorIdentifications.Where(o => o.InvestorId == investor.InvestorId).OrderByDescending(x => x.IsDefault)
                                                   .ThenBy(x => x.Id).FirstOrDefault()
                              join businessCustomer in _dbContext.BusinessCustomers on cifCode.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                              from businessCustomer in businessCustomers.DefaultIfEmpty()
                              where cifCode.Deleted == YesNo.NO
                              && (_dbContext.InvOrders.Any(o => o.CifCode == cifCode.CifCode && o.Deleted == YesNo.NO && (tradingProviderIds.Contains(o.TradingProviderId) || (userType == UserTypes.ROOT_EPIC && dto.TradingProviderId == null)))
                                    || _dbContext.GarnerOrders.Any(o => o.CifCode == cifCode.CifCode && o.Deleted == YesNo.NO && (tradingProviderIds.Contains(o.TradingProviderId) || (userType == UserTypes.ROOT_EPIC && dto.TradingProviderId == null))) 
                                    || _dbContext.RstOrders.Any(o => o.CifCode == cifCode.CifCode && o.Deleted == YesNo.NO && (tradingProviderIds.Contains(o.TradingProviderId) || (userType == UserTypes.ROOT_EPIC && dto.TradingProviderId == null))))
                              // Nếu là Investor
                              && ((investor.Deleted == YesNo.NO && (investor.Status == Status.ACTIVE || investor.Status == Status.INACTIVE)
                                && (((userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC || userType == UserTypes.SUPER_ADMIN)
                                         && tradingProviderIds == null || (_dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                         && tradingProviderIds.Contains(investorTrading.TradingProviderId) && investorTrading.InvestorId == investor.InvestorId)))
                                     || ((userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                                     && (_dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                         && tradingProviderIds.Contains(investorTrading.TradingProviderId) && investorTrading.InvestorId == investor.InvestorId))
                                            || (from eis in _dbContext.InvestorSales
                                                join esctp in _dbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                                where eis.InvestorId == investor.InvestorId && esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO && tradingProviderIds.Contains(esctp.TradingProviderId)
                                                select eis.InvestorId).Any())
                                 )
                                 && (dto.StartDate == null || investor.CreatedDate != null && investor.CreatedDate.Value >= dto.StartDate.Value.Date)
                                 && (dto.EndDate == null || investor.CreatedDate != null && investor.CreatedDate.Value <= dto.EndDate.Value.Date))
                               // Nếu là businessCustomer
                               || (businessCustomer.Deleted == YesNo.NO
                                     && (((userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                                             && _dbContext.BusinessCustomerTradings.Any(o => o.BusinessCustomerId == businessCustomer.BusinessCustomerId && tradingProviderIds.Contains(o.TradingProviderId.Value)))
                                         || ((userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                                             && _dbContext.BusinessCustomerPartners.Any(o => o.BusinessCustomerId == businessCustomer.BusinessCustomerId && o.PartnerId == partnerId))))
                               )
                              select new
                              {
                                  InvestorId = cifCode.InvestorId,
                                  BusinessCustomerId = cifCode.BusinessCustomerId,
                                  Gender = identification.Sex,
                                  CifCode = cifCode.CifCode,
                                  Generation = (identification.DateOfBirth >= new DateTime(1943, 01, 01) && identification.DateOfBirth < new DateTime(1965, 01, 01)) ? Generation.BabyBoomer
                                                    : (identification.DateOfBirth >= new DateTime(1965, 01, 01) && identification.DateOfBirth < new DateTime(1981, 01, 01)) ? Generation.GenerationX
                                                    : (identification.DateOfBirth >= new DateTime(1981, 01, 01) && identification.DateOfBirth < new DateTime(1997, 01, 01)) ? Generation.GenerationY
                                                    : (identification.DateOfBirth >= new DateTime(1997, 01, 01) && identification.DateOfBirth < new DateTime(2016, 01, 01)) ? Generation.GenerationZ
                                                    : 0,
                              });

            var investActiveOrder = from c in _dbContext.CifCodes
                                    join order in _dbContext.InvOrders on c.CifCode equals order.CifCode
                                    where order.Deleted == YesNo.NO && order.Status == OrderStatus.DANG_DAU_TU
                                    && (tradingProviderIds.Contains(order.TradingProviderId) || (userType == UserTypes.ROOT_EPIC && dto.TradingProviderId == null))
                                    select new
                                    {
                                        order.ProjectId,
                                        order.CifCode,
                                        order.TotalValue,
                                    };

            var garnerActiveOrder = from c in _dbContext.CifCodes
                                    join order in _dbContext.GarnerOrders on c.CifCode equals order.CifCode
                                    where order.Deleted == YesNo.NO && order.Status == OrderStatus.DANG_DAU_TU
                                    && (tradingProviderIds.Contains(order.TradingProviderId) || (userType == UserTypes.ROOT_EPIC && dto.TradingProviderId == null))
                                    select new
                                    {
                                        order.PolicyId,
                                        order.CifCode,
                                        order.TotalValue,
                                        order.InitTotalValue,
                                    };

            var rstActiveProductItem = from c in _dbContext.CifCodes
                                       join order in _dbContext.RstOrders on c.CifCode equals order.CifCode
                                       join productItem in _dbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                       where c.InvestorId == c.InvestorId && order.Deleted == YesNo.NO && order.Status == RstOrderStatus.DA_COC
                                       && (tradingProviderIds.Contains(order.TradingProviderId) || (userType == UserTypes.ROOT_EPIC && dto.TradingProviderId == null)) && c.InvestorId != null
                                       select new
                                       {
                                           productItem.Id,
                                           order.CifCode,
                                           productItem.Price,
                                           PaymentAmountDeposit = _dbContext.RstOrderPayments.Where(p => p.OrderId == order.Id && p.Deleted == YesNo.NO && p.TranType == TranTypes.THU
                                                                    && p.TranClassify == TranClassifies.THANH_TOAN && p.Status == OrderPaymentStatus.DA_THANH_TOAN).Sum(p => p.PaymentAmount)
                                       };
            var investActivedAmount = (from c in _dbContext.CifCodes
                                       join order in _dbContext.InvOrders on c.CifCode equals order.CifCode
                                       where order.Deleted == YesNo.NO
                                       && order.SettlementDate < order.DueDate && c.InvestorId != null
                                       && (tradingProviderIds.Contains(order.TradingProviderId) || (userType == UserTypes.ROOT_EPIC && dto.TradingProviderId == null))
                                       select new
                                       {
                                           order.TotalValue,
                                           order.CifCode,
                                       });

            var customerResult = (from customer in customerOrder
                                         select new CustomerListDto
                                         {
                                             TotalActiveAmount = investActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue)
                                                                    + garnerActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue)
                                                                    + rstActiveProductItem.Where(i => i.CifCode == customer.CifCode).Sum(o => o.PaymentAmountDeposit),
                                             TotalActivedAmount = investActivedAmount.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue)
                                                                    + (garnerActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.InitTotalValue)
                                                                    - garnerActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue)),
                                             TotalAmount = (investActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue)
                                                                + garnerActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue))
                                                                + (investActivedAmount.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue)
                                                                + (garnerActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.InitTotalValue)
                                                                    - garnerActiveOrder.Where(i => i.CifCode == customer.CifCode).Sum(o => o.TotalValue))
                                                                + rstActiveProductItem.Where(i => i.CifCode == customer.CifCode).Sum(o => o.PaymentAmountDeposit)),
                                             NumberProducts = investActiveOrder.Where(i => i.CifCode == customer.CifCode).Select(o => o.ProjectId).Distinct().Count()
                                                             + garnerActiveOrder.Where(i => i.CifCode == customer.CifCode).Select(o => o.PolicyId).Distinct().Count()
                                                             + rstActiveProductItem.Where(i => i.CifCode == customer.CifCode).Select(o => o.Id).Distinct().Count(),
                                             CifCode = customer.CifCode,
                                             Gender = customer.Gender,
                                             Generation = customer.Generation,
                                         });

            var result = customerResult.Where(o => o.NumberProducts > 0).AsEnumerable().Select((o, index) => new CustomerListDto
            {
                STT = index + 1,
                NumberProducts = o.NumberProducts,
                TotalActiveAmount = o.TotalActiveAmount,
                TotalActivedAmount = o.TotalActivedAmount,
                TotalAmount = o.TotalAmount,
                Gender = o.Gender,
                CifCode = o.CifCode,
                Generation = o.Generation,
            });

            result = result.AsQueryable().OrderDynamic(dto.Sort);
            decimal totalItems = customerResult.Count();
            if (dto.PageSize != -1)
            {
                result = result.Skip(dto.Skip).Take(dto.PageSize);
            }
            return new PagingResult<CustomerListDto>()
            {
                Items = result,
                TotalItems = totalItems
            };
        }

        private FunnelChartDto FunnelChart()
        {
            var investorList = _investorRegisterLogEFRepository.EntityNoTracking.Where(o => o.Deleted == YesNo.NO);

            var registerNow = investorList.Where(o => o.Type == InvestorRegisterLogTypes.RegisterNow).Count();
            var otpSent = investorList.Where(o => o.Type == InvestorRegisterLogTypes.OtpSent).Count();
            var successfulOtp = investorList.Where(o => o.Type == InvestorRegisterLogTypes.SuccessfulOtp).Count();
            var successfulIdentification = investorList.Where(o => o.Type == InvestorRegisterLogTypes.SuccessfulIdentification).Count();
            var startEkyc = investorList.Where(o => o.Type == InvestorRegisterLogTypes.StartEkyc).Count();
            var successfulEkyc = investorList.Where(o => o.Type == InvestorRegisterLogTypes.SuccessfulEkyc).Count();
            var successfulBank = investorList.Where(o => o.Type == InvestorRegisterLogTypes.SuccessfulBank).Count();
            var completeRegistration = investorList.Where(o => o.Type == InvestorRegisterLogTypes.CompleteRegistration).Count();
            var funnelChart = new FunnelChartDto()
            {
                RegisterNow = registerNow,
                OtpSent = otpSent,
                SuccessfulOtp = successfulOtp,
                SuccessfulIdentification = successfulIdentification,
                StartEkyc = startEkyc,
                SuccessfulEkyc = successfulEkyc,
                SuccessfulBank = successfulBank,
                CompleteRegistration = completeRegistration
            };
            return funnelChart;
        }

        /// <summary>
        /// Tổng số lượng khách hàng theo giới tính
        /// </summary>
        /// <param name="month"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        private int CaculateTotalCustomerByGender(List<int> tradingProviderIds, int month, string gender, int? tradingProviderId)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            _logger.LogInformation($"{nameof(CaculateTotalCustomerByGender)}: month = {month}, gender = {gender}, tradingProviderIds = {JsonSerializer.Serialize(tradingProviderIds)}");

            var result = (from investor in _dbContext.Investors
                                  where investor.Deleted == YesNo.NO && (investor.Status == Status.ACTIVE || investor.Status == Status.INACTIVE)
                                  && (((userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC || userType == UserTypes.SUPER_ADMIN)
                                         && tradingProviderId == null || (_dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                             && tradingProviderIds.Contains(investorTrading.TradingProviderId) && investorTrading.InvestorId == investor.InvestorId))
                                         )
                                         || (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                                         && (_dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                             && tradingProviderIds.Contains(investorTrading.TradingProviderId) && investorTrading.InvestorId == investor.InvestorId)
                                         ) || (from eis in _dbContext.InvestorSales
                                               join esctp in _dbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                               where eis.InvestorId == investor.InvestorId && esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO && tradingProviderIds.Contains(esctp.TradingProviderId)
                                               select eis.InvestorId).Any())
                                   && investor.CreatedDate != null && investor.CreatedDate.Value.Month == month && investor.CreatedDate.Value.Year == DateTime.Now.Year
                                   && _dbContext.InvestorIdentifications.Any(identification => identification.InvestorId == investor.InvestorId
                                     && identification.Deleted == YesNo.NO && identification.Sex == gender
                                     && identification.IsDefault == YesNo.YES)
                                  select investor.InvestorId).Count();
            return result;
        }

        /// <summary>
        /// Tổng số lượng khách hàng theo thế hệ
        /// </summary>
        /// <param name="month"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private int CaculateTotalCustomerByGeneration(List<int> tradingProviderIds, int month, DateTime startDate, DateTime endDate, int? tradingProviderId)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            _logger.LogInformation($"{nameof(CaculateTotalCustomerByGeneration)}: startDate = {startDate}, endDate = {endDate}, tradingProviderIds = {JsonSerializer.Serialize(tradingProviderIds)}");

            var result = (from investor in _dbContext.Investors
                          where investor.Deleted == YesNo.NO && (investor.Status == Status.ACTIVE || investor.Status == Status.INACTIVE)
                          && (((userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC || userType == UserTypes.SUPER_ADMIN)
                                 && tradingProviderId == null || (_dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                     && tradingProviderIds.Contains(investorTrading.TradingProviderId) && investorTrading.InvestorId == investor.InvestorId))
                                 )
                                 || (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                                 && (_dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                     && tradingProviderIds.Contains(investorTrading.TradingProviderId) && investorTrading.InvestorId == investor.InvestorId)
                                 ) || (from eis in _dbContext.InvestorSales
                                       join esctp in _dbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                       where eis.InvestorId == investor.InvestorId && esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO && tradingProviderIds.Contains(esctp.TradingProviderId)
                                       select eis.InvestorId).Any())
                           && investor.CreatedDate != null && investor.CreatedDate.Value.Month == month && investor.CreatedDate.Value.Year == DateTime.Now.Year
                           && _dbContext.InvestorIdentifications.Any(identification => identification.InvestorId == investor.InvestorId
                             && identification.Deleted == YesNo.NO && (identification.DateOfBirth >= startDate && identification.DateOfBirth < endDate)
                             && identification.IsDefault == YesNo.YES)
                          select investor.InvestorId).Count();
            return result;
        }
        /// <summary>
        /// Tổng quan
        /// </summary>
        /// <returns></returns>
        private DashboardOverviewDto DashboardOverview(List<int> tradingProviderIds, DateTime? startDate, DateTime? endDate, int? tradingProviderId)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;
            _logger.LogInformation($"{nameof(DashboardOverview)}: startDate = {startDate}, endDate = {endDate}, tradingProviderIds =  {JsonSerializer.Serialize(tradingProviderIds)}");

            var listInvestorId = from investor in _dbContext.Investors
                                 where investor.Deleted == YesNo.NO && (investor.Status == Status.ACTIVE || investor.Status == Status.INACTIVE)
                                 select investor.InvestorId;
            var listInvestor = (from investor in _dbContext.Investors
                                where (startDate == null || startDate.Value.Date <= investor.CreatedDate.Value.Date) && (endDate == null || endDate.Value.Date >= investor.CreatedDate.Value.Date)
                                select investor);
            var listYearInvestor = from investor in _dbContext.Investors
                                   where (endDate == null ? investor.CreatedDate.Value.Year == DateTime.Now.Year : investor.CreatedDate.Value.Year == endDate.Value.Year)
                                   select investor;
            if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC || userType == UserTypes.SUPER_ADMIN)
            {
                listInvestorId = (from investorId in listInvestorId
                                  where (from investorTrading in _dbContext.InvestorTradingProviders
                                         where investorTrading.Deleted == YesNo.NO && tradingProviderIds.Contains(investorTrading.TradingProviderId)
                                         select investorTrading.InvestorId).Contains(investorId) || tradingProviderId == null
                                  select investorId);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                listInvestorId = (from investorId in listInvestorId
                                  where ((from investorTrading in _dbContext.InvestorTradingProviders
                                          where investorTrading.Deleted == YesNo.NO && tradingProviderIds.Contains(investorTrading.TradingProviderId)
                                          select investorTrading.InvestorId)
                                          .Union(from eis in _dbContext.InvestorSales
                                                 join esctp in _dbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                                 where esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO && tradingProviderIds.Contains(esctp.TradingProviderId)
                                                 select eis.InvestorId)).Contains(investorId)
                                  select investorId);
            }

            listInvestor = listInvestor.Where(o => listInvestorId.Contains(o.InvestorId));
            listYearInvestor = listYearInvestor.Where(o => listInvestorId.Contains(o.InvestorId));

            var listBusinessCustomer = (from business in _dbContext.BusinessCustomers
                                        where business.Deleted == YesNo.NO
                                        select business);
            var listYearBusinessCustomer = (from business in _dbContext.BusinessCustomers
                                            where business.Deleted == YesNo.NO
                                            select business);

            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                listBusinessCustomer = listBusinessCustomer.Where(b => _dbContext.BusinessCustomerPartners.Any(o => b.BusinessCustomerId == o.BusinessCustomerId && o.PartnerId == partnerId)
                                        && (startDate == null || startDate.Value.Date <= b.CreatedDate.Value.Date)
                                        && (endDate == null || endDate.Value.Date >= b.CreatedDate.Value.Date));

                listYearBusinessCustomer = listYearBusinessCustomer.Where(b => _dbContext.BusinessCustomerPartners.Any(o => b.BusinessCustomerId == o.BusinessCustomerId && o.PartnerId == partnerId)
                                            && (endDate == null ? b.CreatedDate.Value.Year == DateTime.Now.Year : b.CreatedDate.Value.Year == endDate.Value.Year));
            }
            else if (userType == UserTypes.ROOT_TRADING_PROVIDER || userType == UserTypes.TRADING_PROVIDER)
            {
                // Thêm thông tin doanh nghiệp của đại lý vào getAll

                listBusinessCustomer = listBusinessCustomer.Where(b => _dbContext.BusinessCustomerTradings.Any(o => o.BusinessCustomerId == b.BusinessCustomerId && tradingProviderIds.Contains(o.TradingProviderId.Value))
                                        && (startDate == null || startDate.Value.Date <= b.CreatedDate.Value.Date)
                                        && (endDate == null || endDate.Value.Date >= b.CreatedDate.Value.Date));

                listYearBusinessCustomer = listYearBusinessCustomer.Where(b => _dbContext.BusinessCustomerTradings.Any(o => o.BusinessCustomerId == b.BusinessCustomerId && tradingProviderIds.Contains(o.TradingProviderId.Value))
                                            && (endDate == null ? b.CreatedDate.Value.Year == DateTime.Now.Year : b.CreatedDate.Value.Year == endDate.Value.Year));
                //// Lấy thông tin đại lý
                //var tradingProviderFind = _dbContext.TradingProviders.FirstOrDefault(t => tradingProviderIds.Contains(t.TradingProviderId) && t.Deleted == YesNo.NO);
                //if (tradingProviderFind != null)
                //{
                //    var businessCustomerOfTrading = _dbContext.BusinessCustomers.FirstOrDefault(o => o.BusinessCustomerId == tradingProviderFind.BusinessCustomerId);
                //    if (businessCustomerOfTrading != null)
                //    {
                //        listBusinessCustomer.Add(businessCustomerOfTrading);
                //    }
                //}
            }
            var listTodayInvestor = listInvestor.Where(o => endDate == null ? o.CreatedDate.Value.Date == DateTime.Now.Date : o.CreatedDate.Value.Date == endDate.Value.Date);
            var listMonthInvestor = listInvestor.Where(o => endDate == null ? (o.CreatedDate.Value.Month == DateTime.Now.Month && o.CreatedDate.Value.Year == DateTime.Now.Year)
                                : (o.CreatedDate.Value.Month == endDate.Value.Month && o.CreatedDate.Value.Year == endDate.Value.Year));

            var listTodayBusinessCustomer = listBusinessCustomer.Where(o => endDate == null ? o.CreatedDate.Value.Date == DateTime.Now.Date : o.CreatedDate.Value.Date == endDate.Value.Date);
            var listMonthBusinessCustomer = listBusinessCustomer.Where(o => endDate == null ? (o.CreatedDate.Value.Month == DateTime.Now.Month && o.CreatedDate.Value.Year == DateTime.Now.Year)
                                : (o.CreatedDate.Value.Month == endDate.Value.Month && o.CreatedDate.Value.Year == endDate.Value.Year));

            var overview = new DashboardOverviewDto();
            overview.TotalInvestor = listInvestor.Count() + listBusinessCustomer.Count();
            overview.TotalTodayInvestor = listTodayInvestor.Count() + listTodayBusinessCustomer.Count();
            overview.TotalMonthInvestor = listMonthInvestor.Count() + listMonthBusinessCustomer.Count();
            overview.TotalYearInvestor = listYearInvestor.Count() + listYearBusinessCustomer.Count();
            overview.TotalDealInvestor = CountDealInvestor(listInvestor) + CountDealBusinessCustomer(listBusinessCustomer);
            overview.TotalTodayDealInvestor = CountDealInvestor(listTodayInvestor) + CountDealBusinessCustomer(listTodayBusinessCustomer);
            overview.TotalMonthDealInvestor = CountDealInvestor(listMonthInvestor) + CountDealBusinessCustomer(listMonthBusinessCustomer);
            overview.TotalYearDealInvestor = CountDealInvestor(listYearInvestor) + CountDealBusinessCustomer(listYearBusinessCustomer);
            return overview;
        }

        /// <summary>
        /// Hàm chung tính số lượng khách hàng tham gia đầu tư theo (tổng, hôm nay, tháng, năm) -> tỉ lệ giao dịch 
        /// </summary>
        /// <param name="investor"></param>
        /// <returns></returns>
        private int CountDealInvestor(IQueryable<Investor> investor)
        {
            var count = (from item in investor
                         join cifCode in _dbContext.CifCodes on item.InvestorId equals cifCode.InvestorId
                         where (_dbContext.InvOrders.Any(o => o.CifCode == cifCode.CifCode)
                         || _dbContext.GarnerOrders.Any(o => o.CifCode == cifCode.CifCode) || _dbContext.RstOrders.Any(o => o.CifCode == cifCode.CifCode))
                         select cifCode).Count();
            return count;
        }

        /// <summary>
        /// Hàm chung tính số lượng khách hàng doanh nghiệp tham gia đầu tư theo (tổng, hôm nay, tháng, năm) -> tỉ lệ giao dịch 
        /// </summary>
        /// <param name="businessCustomer"></param>
        /// <returns></returns>
        private int CountDealBusinessCustomer(IQueryable<BusinessCustomer> businessCustomer)
        {
            var count = (from item in businessCustomer
                         join cifCode in _dbContext.CifCodes on item.BusinessCustomerId equals cifCode.BusinessCustomerId
                         where (_dbContext.InvOrders.Any(o => o.CifCode == cifCode.CifCode)
                         || _dbContext.GarnerOrders.Any(o => o.CifCode == cifCode.CifCode) || _dbContext.RstOrders.Any(o => o.CifCode == cifCode.CifCode))
                         select cifCode).Count();
            return count;
        }
    }
}
