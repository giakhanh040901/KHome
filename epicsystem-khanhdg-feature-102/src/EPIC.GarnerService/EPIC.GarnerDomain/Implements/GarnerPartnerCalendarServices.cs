using EPIC.DataAccess.Base;
using EPIC.Entities.Dto.PartnerCalendar;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerPartnerCalendarServices : IGarnerPartnerCalendarServices
    {
        private readonly GarnerPartnerCalendarEFRepository _partnerCalendarEFRepository;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<GarnerPartnerCalendarServices> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public GarnerPartnerCalendarServices(
            EpicSchemaDbContext dbContext,
            ILogger<GarnerPartnerCalendarServices> logger,
            IHttpContextAccessor httpContext
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContext = httpContext;
            _partnerCalendarEFRepository = new GarnerPartnerCalendarEFRepository(dbContext, _logger);
        }

        public int AddByYear(CreatePartnerCalendarDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = _partnerCalendarEFRepository.AddByYear(new GarnerPartnerCalendar
            {
                WorkingYear = input.WorkingYear,
                PartnerId = partnerId
            });
            return result;
        }

        public List<GarnerPartnerCalendar> FindAll(int? workingYear)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: workingYear = {workingYear}");

            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var data = _partnerCalendarEFRepository.FindAllByWorkingYear(partnerId, workingYear);

            return data;
        }

        public GarnerPartnerCalendar FindByWorkingDate(DateTime date)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: date = {date}");

            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _partnerCalendarEFRepository.FindByWorkingDate(date, partnerId);
        }

        public void Update(UpdateCalendarDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            for (var currentDate = input.WorkingDate; currentDate <= (input.WorkingEndDate == null ? input.WorkingDate : input.WorkingEndDate); currentDate = currentDate.AddDays(1))
            {
                var calendar = FindByWorkingDate(currentDate);
                calendar.IsDayOff = input.IsDayOff;
                _partnerCalendarEFRepository.Update(calendar);
            }
        }
    }
}   
