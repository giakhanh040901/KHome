using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.TradingProviderConfig;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.EventDomain.Implements;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class TradingProviderConfigServices : ITradingProviderConfigServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<TradingProviderConfigServices> _logger;
        private readonly NotificationServices _sendEmailServices;
        private readonly EventNotificationServices _eventNotificationServices;
        private readonly EvtBackgroundJobServices _evtBackgroundJobServices;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderConfigEFRepository _tradingProviderConfigEFRepository;

        public TradingProviderConfigServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<TradingProviderConfigServices> logger,
            IHttpContextAccessor httpContextAccessor,
            NotificationServices sendEmailServices,
            EventNotificationServices eventNotificationServices,
            EvtBackgroundJobServices evtBackgroundJobServices,
            IBackgroundJobClient backgroundJobs)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _sendEmailServices = sendEmailServices;
            _eventNotificationServices = eventNotificationServices;
            _evtBackgroundJobServices = evtBackgroundJobServices;
            _backgroundJobs = backgroundJobs;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _tradingProviderConfigEFRepository = new TradingProviderConfigEFRepository(_dbContext, _logger);
        }
        #region Hangfire

        /// <summary>
        /// Xóa tự động các hợp đồng không có thanh toán sau 1 khoản thời gian do đại lý cài
        /// </summary>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Core)]
        [HangfireLogEverything]
        public void DeletedOrderTimeUpByTradingProvider()
        {
            _logger.LogInformation($"{nameof(DeletedOrderTimeUpByTradingProvider)}");
            var tradingProviders = _dbContext.TradingProviders.AsNoTracking().Where(t => t.Deleted == YesNo.NO);
            foreach (var tradingItem in tradingProviders)
            {
                var tradingProviderConfig = _dbContext.TradingProviderConfigs.AsNoTracking().Where(c => c.TradingProviderId == tradingItem.TradingProviderId);
                if (!tradingProviderConfig.Any()) continue;

                var keyDeletedInvest = tradingProviderConfig.FirstOrDefault(r => r.Key == TradingProviderConfigKeys.DeletedOrderInvest);
                int numberDateDeleteInvestOrder;
                if (keyDeletedInvest != null && int.TryParse(keyDeletedInvest.Value, out numberDateDeleteInvestOrder) && numberDateDeleteInvestOrder > 0)
                {
                    var orderDelete = _dbContext.InvOrders.Where(o => !_dbContext.InvestOrderPayments.Any(p => p.OrderId == o.Id && p.Status != OrderPaymentStatus.HUY_THANH_TOAN && p.Deleted == YesNo.NO)
                                        && o.TradingProviderId == tradingItem.TradingProviderId && new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN }.Contains(o.Status)
                                        && o.BuyDate != null && (o.BuyDate.Value.Date.AddDays(numberDateDeleteInvestOrder) < DateTime.Now.Date) && o.Deleted == YesNo.NO);
                    foreach (var item in orderDelete)
                    {
                        item.Deleted = YesNo.YES;
                        item.ModifiedDate = DateTime.Now;
                    }
                    _dbContext.SaveChanges();
                }

                var keyDeletedGarner = tradingProviderConfig.FirstOrDefault(r => r.Key == TradingProviderConfigKeys.DeletedOrderGarner);
                int numerDateDeleteGarnerOrder;
                if (keyDeletedGarner != null && int.TryParse(keyDeletedInvest.Value, out numerDateDeleteGarnerOrder) && numerDateDeleteGarnerOrder > 0)
                {
                    var orderDelete = _dbContext.GarnerOrders.Where(o => !_dbContext.GarnerOrderPayments.AsNoTracking().Any(p => p.OrderId == o.Id && p.Status != OrderPaymentStatus.HUY_THANH_TOAN && p.Deleted == YesNo.NO)
                                        && o.TradingProviderId == tradingItem.TradingProviderId && new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN }.Contains(o.Status)
                                        && (o.BuyDate.Date.AddDays(numerDateDeleteGarnerOrder) < DateTime.Now.Date) && o.Deleted == YesNo.NO);
                    foreach (var item in orderDelete)
                    {
                        item.Deleted = YesNo.YES;
                        item.ModifiedDate = DateTime.Now;
                    }
                    _dbContext.SaveChanges();
                }

                var keyDeletedRealEstate = tradingProviderConfig.FirstOrDefault(r => r.Key == TradingProviderConfigKeys.DeletedOrderRealEstate);
                int numerDateDeleteRealEstateOrder;
                if (keyDeletedRealEstate != null && int.TryParse(keyDeletedRealEstate.Value, out numerDateDeleteRealEstateOrder) && numerDateDeleteRealEstateOrder > 0)
                {
                    var orderDelete = _dbContext.RstOrders.Where(o => !_dbContext.RstOrderPayments.Any(p => p.OrderId == o.Id && p.Status != OrderPaymentStatus.HUY_THANH_TOAN && p.Deleted == YesNo.NO)
                                        && new int[] { RstOrderStatus.KHOI_TAO, RstOrderStatus.CHO_THANH_TOAN_COC }.Contains(o.Status)
                                        && o.TradingProviderId == tradingItem.TradingProviderId && o.ExpTimeDeposit < DateTime.Now
                                        && o.CreatedDate != null && (o.CreatedDate.Value.Date.AddDays(numerDateDeleteRealEstateOrder) < DateTime.Now.Date) && o.Deleted == YesNo.NO);
                    foreach (var item in orderDelete)
                    {
                        item.Deleted = YesNo.YES;
                        item.ModifiedDate = DateTime.Now;
                    }
                    _dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Gửi thông báo sinh nhật cho các đại lý chưa cài thời gian gửi thông báo
        /// </summary>
        public void SendNotiHappyBirthDayByTradingProvider()
        {
            var tradingProviders = from tradingProvider in _dbContext.TradingProviders.AsNoTracking()
                                   join user in _dbContext.Users.AsNoTracking() on tradingProvider.TradingProviderId equals user.TradingProviderId
                                   where tradingProvider.Deleted == YesNo.NO && user.IsDeleted == YesNo.NO
                                   select tradingProvider;

            foreach (var tradingItem in tradingProviders)
            {
                var tradingProviderConfig = _dbContext.TradingProviderConfigs.AsNoTracking().Where(c => c.TradingProviderId == tradingItem.TradingProviderId);

                if (!tradingProviderConfig.Any(r => r.Key == TradingProviderConfigKeys.TimeSendNotiBirthDayCustomerForSale))
                {
                    string jobId = $"sendNotiBirthdayCustomerForSale_TradingProviderId:{tradingItem.TradingProviderId}";
                    RecurringJob.AddOrUpdate(jobId, () => SendNotiCustomerBirthDayForSale(tradingItem.TradingProviderId, null), Cron.Daily());
                }

                if (!tradingProviderConfig.Any(r => r.Key == TradingProviderConfigKeys.TimeSendNotiHappyBirthDayCustomer))
                {
                    string jobId = $"sendNotiHappyBirthdayCustomer_TradingProviderId:{tradingItem.TradingProviderId}";
                    RecurringJob.AddOrUpdate(jobId, () => SendNotiCustomerHappyBirthDay(tradingItem.TradingProviderId), Cron.Daily());
                }
            }
        }

        /// <summary>
        /// Gửi thông báo sắp đến sinh nhật nhà đầu tư cho Sale
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="timeDay">Thời gian tính ngày</param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Core)]
        [HangfireLogEverything]
        public async Task SendNotiCustomerBirthDayForSale(int tradingProviderId, int? timeDay)
        {
            var investorQuery = from investor in _dbContext.Investors
                                let dateOfBirth = _dbContext.InvestorIdentifications.Where(r => r.InvestorId == investor.InvestorId && r.Deleted == YesNo.NO)
                                            .OrderByDescending(ii => ii.IsDefault).ThenByDescending(ii => ii.Id).FirstOrDefault().DateOfBirth
                                where investor.Deleted == YesNo.NO && investor.Status == InvestorStatus.ACTIVE
                                && dateOfBirth.Value.Month == DateTime.Now.Month && dateOfBirth.Value.Day - DateTime.Now.Day == (timeDay ?? 1)
                                select investor.InvestorId;
            if (investorQuery.Any())
            {
                var investorSaleQuery = from investorSale in _dbContext.InvestorSales.Where(r => investorQuery.Contains(r.InvestorId) && r.Deleted == YesNo.NO)
                                        join sale in _dbContext.Sales on investorSale.SaleId equals sale.SaleId
                                        join saleTradingProvider in _dbContext.SaleTradingProviders on sale.SaleId equals saleTradingProvider.SaleId
                                        where sale.Deleted == YesNo.NO && saleTradingProvider.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE
                                        && saleTradingProvider.TradingProviderId == tradingProviderId
                                        select investorSale;
                foreach (var item in investorSaleQuery)
                {
                    await _sendEmailServices.SendEmailSaleNotiCustomerBirthDay(item.SaleId ?? 0, item.InvestorId, tradingProviderId);
                }
            }
        }

        /// <summary>
        /// Gửi thông báo sinh nhật nhà đầu tư
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [Queue(HangfireQueues.Core)]
        [HangfireLogEverything]
        public async Task SendNotiCustomerHappyBirthDay(int tradingProviderId)
        {
            var investorQuery = from investor in _dbContext.Investors
                                let dateOfBirth = _dbContext.InvestorIdentifications.Where(r => r.InvestorId == investor.InvestorId && r.Deleted == YesNo.NO)
                                            .OrderByDescending(ii => ii.IsDefault).ThenByDescending(ii => ii.Id).FirstOrDefault().DateOfBirth
                                where investor.Deleted == YesNo.NO && investor.Status == InvestorStatus.ACTIVE
                                && dateOfBirth.Value.Month == DateTime.Now.Month && dateOfBirth.Value.Day == DateTime.Now.Day
                                && _dbContext.InvestorTradingProviders.Any(t => t.TradingProviderId == tradingProviderId && t.InvestorId == investor.InvestorId && t.Deleted == YesNo.NO)
                                select investor.InvestorId;
            foreach (var item in investorQuery)
            {
                await _sendEmailServices.SendEmailNotiCustomerHappyBirthDay(item, tradingProviderId);
            }
        }

        /// <summary>
        /// Gửi thông báo đến khách hàng, sự kiện sắp diễn ra
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        private void SendNotiCustomerEventUpcoming(int tradingProviderId)
        {
            var configTimeSendNoti = _dbContext.TradingProviderConfigs.FirstOrDefault(c => c.TradingProviderId == tradingProviderId && c.Key == TradingProviderConfigKeys.EventTimeSendNotiEventUpcomingForCustomer
                                   && c.Deleted == YesNo.NO);

            var configDaySendNoti = _dbContext.TradingProviderConfigs.FirstOrDefault(c => c.TradingProviderId == tradingProviderId && c.Key == TradingProviderConfigKeys.EventDaySendNotiEventUpcomingForCustomer
                                        && c.Deleted == YesNo.NO);
            int day = int.TryParse(configDaySendNoti?.Value, out day) ? day : 1;

            var eventDetails = from eventDetail in _dbContext.EvtEventDetails
                                join events in _dbContext.EvtEvents on eventDetail.EventId equals events.Id
                                where eventDetail.Deleted == YesNo.NO && events.Deleted == YesNo.NO
                                && events.TradingProviderId == tradingProviderId
                                && eventDetail.StartDate != null && eventDetail.Status == EventDetailStatus.KICH_HOAT
                                && eventDetail.StartDate.Value.Date >= DateTime.Now.Date
                               select eventDetail;

            foreach (var item in eventDetails)
            {
                // Lấy thời gian gửi thông báo
                DateTime timeSend = item.StartDate.Value.Date.AddDays(-day);
                try
                {

                    string[] time = configTimeSendNoti.Value.Split(':');
                    int hours = int.Parse(time[0]);
                    int minutes = int.Parse(time[1]);

                    timeSend = timeSend.AddHours(hours).AddMinutes(minutes);
                }
                catch
                {
                    timeSend = timeSend.AddHours(9);
                }

                if (item.UpcomingJobId != null)
                {
                    // Xóa job hiện có của sự kiện
                    _backgroundJobs.Delete(item.UpcomingJobId);
                }

                if (timeSend < DateTime.Now)
                {  
                    continue;
                }
                else
                {
                    // Sinh job mới
                    string jobId = _backgroundJobs.Schedule(() => _evtBackgroundJobServices.SendEventUpComing(item.Id), timeSend);
                    item.UpcomingJobId = jobId;
                }
                _dbContext.SaveChanges();
            }
        }
        #endregion

        #region CRUD
        /// <summary>
        /// Xem danh sách Config
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<TradingProviderConfigDto> GetAll(string keyword)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(GetAll)}: keyword = {keyword}, tradingProviderId = {tradingProviderId}, username = {username}");
            var result = _tradingProviderConfigEFRepository.EntityNoTracking.Where(r => r.TradingProviderId == tradingProviderId && (keyword == null || r.Key == keyword)
                && r.Deleted == YesNo.NO)
                .Select(x => new TradingProviderConfigDto
                {
                    Key = x.Key,
                    Value = x.Value,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                });
            return result.ToList();
        }

        /// <summary>
        /// Thêm nhiều config cho đại lý
        /// </summary>
        /// <param name="input"></param>
        public void AddMutipleConfig(List<CreateTradingProviderConfigDto> input)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var item in input)
            {
                Add(item);
            }
            transaction.Commit();
        }

        /// <summary>
        /// Update nhiều config cho đại lý
        /// </summary>
        /// <param name="input"></param>
        public void UpdateMutipleConfig(List<CreateTradingProviderConfigDto> input)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var item in input)
            {
                Update(item);
            }
            transaction.Commit();
        }

        public void Add(CreateTradingProviderConfigDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");
            if (!TradingProviderConfigKeys.ConfigKeys.Contains(input.Key))
            {
                _tradingProviderConfigEFRepository.ThrowException(ErrorCode.CoreTradingProviderConfigNotExistKey);
            }
            var checkKeyConfigData = _dbContext.TradingProviderConfigs.Any(t => t.TradingProviderId == tradingProviderId && t.Key == input.Key && t.Deleted == YesNo.NO);
            if (checkKeyConfigData)
            {
                _tradingProviderConfigEFRepository.ThrowException(ErrorCode.CoreTradingProviderConfigExistKeyData);
            }

            // Nếu là Key gửi thông báo cho
            if (input.Key == TradingProviderConfigKeys.TimeSendNotiBirthDayCustomerForSale || input.Key == TradingProviderConfigKeys.TimeSendNotiHappyBirthDayCustomer
                || input.Key == TradingProviderConfigKeys.EventTimeSendNotiEventUpcomingForCustomer)
            {
                string[] time = input.Value.Split(':');
                try
                {
                    int hours = int.Parse(time[0]);
                    int minutes = int.Parse(time[1]);

                    if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    _tradingProviderConfigEFRepository.ThrowException(ErrorCode.CoreTradingProviderConfigValueSendNotiInvalid);
                }
            }
            var insert = _tradingProviderConfigEFRepository.Add(new TradingProviderConfig
            {
                TradingProviderId = tradingProviderId,
                Key = input.Key,
                Value = input.Value,
                CreatedBy = username
            });

            // Nếu là thông báo có sắp có sinh nhật Nhà đầu tư đến Tư vấn viên
            if (input.Key == TradingProviderConfigKeys.TimeSendNotiBirthDayCustomerForSale)
            {
                string[] time = input.Value.Split(':');
                int hours = int.Parse(time[0]);
                int minutes = int.Parse(time[1]);
                // Lấy giờ gửi theo múi giờ UTC+7
                DateTime timeDaily = DateTime.Today.AddHours(hours).AddHours(7);
                string jobId = $"sendNotiBirthdayCustomerForSale_TradingProviderId:{tradingProviderId}";
                RecurringJob.AddOrUpdate(jobId, () => SendNotiCustomerBirthDayForSale(tradingProviderId, null), Cron.Daily(timeDaily.Hour, minutes));
                insert.BackgroundJobId = jobId;
            }
            // Nếu là thông báo chúc mừng sinh nhật nhà đầu tư
            else if (input.Key == TradingProviderConfigKeys.TimeSendNotiHappyBirthDayCustomer)
            {
                string[] time = input.Value.Split(':');
                int hours = int.Parse(time[0]);
                int minutes = int.Parse(time[1]);
                // Lấy giờ gửi theo múi giờ UTC+7
                DateTime timeDaily = DateTime.Today.AddHours(hours).AddHours(7);
                string jobId = $"sendNotiHappyBirthdayCustomer_TradingProviderId:{tradingProviderId}";
                RecurringJob.AddOrUpdate(jobId, () => SendNotiCustomerHappyBirthDay(tradingProviderId), Cron.Daily(timeDaily.Hour, minutes));
                insert.BackgroundJobId = jobId;
            }
            // Nếu là thông báo sự kiện sắp diễn ra
            else if (input.Key == TradingProviderConfigKeys.EventTimeSendNotiEventUpcomingForCustomer || input.Key == TradingProviderConfigKeys.EventDaySendNotiEventUpcomingForCustomer)
            {
                SendNotiCustomerEventUpcoming(tradingProviderId);
            }
            _dbContext.SaveChanges();
        }

        public void Update(CreateTradingProviderConfigDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");
            if (!TradingProviderConfigKeys.ConfigKeys.Contains(input.Key))
            {
                _tradingProviderConfigEFRepository.ThrowException(ErrorCode.CoreTradingProviderConfigNotExistKey);
            }
            var tradingProviderConfig = _dbContext.TradingProviderConfigs.FirstOrDefault(t => t.TradingProviderId == tradingProviderId && t.Key == input.Key && t.Deleted == YesNo.NO);
            if (tradingProviderConfig != null)
            {
                tradingProviderConfig.Value = input.Value;
                tradingProviderConfig.ModifiedBy = username;
                tradingProviderConfig.ModifiedDate = DateTime.Now;

                // Nếu là thông báo có sắp có sinh nhật Nhà đầu tư
                if (input.Key == TradingProviderConfigKeys.TimeSendNotiBirthDayCustomerForSale || input.Key == TradingProviderConfigKeys.TimeSendNotiHappyBirthDayCustomer
                    || input.Key == TradingProviderConfigKeys.EventTimeSendNotiEventUpcomingForCustomer)
                {
                    string[] time = input.Value.Split(':');
                    try
                    {
                        int hours = int.Parse(time[0]);
                        int minutes = int.Parse(time[1]);

                        // Lấy giờ gửi theo múi giờ UTC+7
                        DateTime timeDaily = DateTime.Today.AddHours(hours).AddHours(-7);

                        if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
                        {
                            throw new Exception();
                        }
                        if (input.Key == TradingProviderConfigKeys.TimeSendNotiBirthDayCustomerForSale)
                        {
                            RecurringJob.AddOrUpdate(tradingProviderConfig.BackgroundJobId, () => SendNotiCustomerBirthDayForSale(tradingProviderId, null), Cron.Daily(timeDaily.Hour, minutes));
                        }
                        else if (input.Key == TradingProviderConfigKeys.TimeSendNotiHappyBirthDayCustomer)
                        {
                            RecurringJob.AddOrUpdate(tradingProviderConfig.BackgroundJobId, () => SendNotiCustomerHappyBirthDay(tradingProviderId), Cron.Daily(timeDaily.Hour, minutes));
                        }
                    }
                    catch
                    {
                        _tradingProviderConfigEFRepository.ThrowException(ErrorCode.CoreTradingProviderConfigValueSendNotiInvalid);
                    }
                }

                if (input.Key == TradingProviderConfigKeys.EventTimeSendNotiEventUpcomingForCustomer || input.Key == TradingProviderConfigKeys.EventDaySendNotiEventUpcomingForCustomer)
                {
                    SendNotiCustomerEventUpcoming(tradingProviderId);
                }
            }
            _dbContext.SaveChanges();
        }

        public void Delete(string key)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: key = {key}, tradingProviderId = {tradingProviderId}, username = {username}");
            var tradingProviderConfig = _dbContext.TradingProviderConfigs.FirstOrDefault(t => t.TradingProviderId == tradingProviderId && t.Key == key && t.Deleted == YesNo.NO);
            if (tradingProviderConfig != null)
            {
                // Nếu là thông báo có sắp có sinh nhật Nhà đầu tư
                if (tradingProviderConfig.Key == TradingProviderConfigKeys.TimeSendNotiBirthDayCustomerForSale || tradingProviderConfig.Key == TradingProviderConfigKeys.TimeSendNotiHappyBirthDayCustomer
                    || tradingProviderConfig.Key == TradingProviderConfigKeys.EventTimeSendNotiEventUpcomingForCustomer)
                {
                    RecurringJob.RemoveIfExists(tradingProviderConfig.BackgroundJobId);
                }
                tradingProviderConfig.Deleted = YesNo.YES;
                tradingProviderConfig.ModifiedBy = username;
                tradingProviderConfig.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
