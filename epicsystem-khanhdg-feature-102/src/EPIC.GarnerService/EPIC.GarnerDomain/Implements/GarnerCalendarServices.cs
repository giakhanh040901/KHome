using EPIC.DataAccess.Base;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerCalendarServices : IGarnerCalendarServices
    {
        private readonly GarnerCalendarEFRepository _calendarRepository;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<GarnerCalendarServices> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public GarnerCalendarServices(
            EpicSchemaDbContext dbContext,
            ILogger<GarnerCalendarServices> logger,
            IHttpContextAccessor httpContext
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContext = httpContext;
            _calendarRepository = new GarnerCalendarEFRepository(dbContext, _logger);
        }

        public int AddByYear(CreateCalendarDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _calendarRepository.AddByYear(new GarnerCalendar
            {
                WorkingYear = input.WorkingYear,
                TradingProviderId = tradingProviderId
            });
            return result;
        }

        public List<GarnerCalendar> FindAll(int? workingYear)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: workingYear = {workingYear}");

            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var data = _calendarRepository.FindAllByWorkingYear(tradingProviderId, workingYear);

            return data;
        }

        public GarnerCalendar FindByWorkingDate(DateTime date)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: date = {date}");

            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _calendarRepository.FindByWorkingDate(date, tradingProviderId);
        }

        public void Update(UpdateCalendarDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");
            
            for (var currentDate = input.WorkingDate; currentDate <= (input.WorkingEndDate == null ? input.WorkingDate : input.WorkingEndDate); currentDate = currentDate.AddDays(1))
            {
                var calendar = FindByWorkingDate(currentDate);
                calendar.IsDayOff = input.IsDayOff;
                _calendarRepository.Update(calendar);
            }
        }
    }
}
