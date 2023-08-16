using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Calendar;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondCalendarService : IBondCalendarService
    {
        private readonly ILogger<BondCalendarService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondCalendarRepository _calendarRepository;
        private readonly IHttpContextAccessor _httpContext;

        public BondCalendarService(ILogger<BondCalendarService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _calendarRepository = new BondCalendarRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }

        public int AddByYear(CreateCalendarDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _calendarRepository.AddByYear(new BondCalendar
            {
                WorkingYear = input.WorkingYear,
                TradingProviderId = tradingProviderId
            });
            return result;
        }

        public List<BondCalendar> FindAll(int workingYear)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _calendarRepository.FindAllByWorkingYear(workingYear, tradingProviderId);
        }

        public BondCalendar FindByWorkingDate(DateTime date)
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
