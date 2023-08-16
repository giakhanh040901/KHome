using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Calendar;
using EPIC.CompanySharesRepositories;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CalendarServices : ICalendarServices
    {
        private readonly ILogger<CalendarServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly CalendarRepository _calendarRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CalendarServices(ILogger<CalendarServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _calendarRepository = new CalendarRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }

        public int AddByYear(CreateCalendarDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _calendarRepository.AddByYear(new Calendar
            {
                WorkingYear = input.WorkingYear,
                TradingProviderId = tradingProviderId
            });
            return result;
        }

        public List<Calendar> FindAll(int workingYear)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _calendarRepository.FindAllByWorkingYear(workingYear, tradingProviderId);
        }

        public Calendar FindByWorkingDate(DateTime date)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _calendarRepository.FindByWorkingDate(date, tradingProviderId);
        }

        public int Update(UpdateCalendarDto input)
        {
            var calendar = FindByWorkingDate(input.WorkingDate);

            calendar.BusDate = input.BusDate;
            calendar.IsDayOff = input.IsDayOff;

            return _calendarRepository.Update(calendar);
        }
    }
}