using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.PartnerCalendar;
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
    public class BondPartnerCalendarService : IBondPartnerCalendarService
    {
        private readonly ILogger<BondPartnerCalendarService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondPartnerCalendarRepository _partnerCalendarRepository;
        private readonly IHttpContextAccessor _httpContext;

        public BondPartnerCalendarService(ILogger<BondPartnerCalendarService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _partnerCalendarRepository = new BondPartnerCalendarRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }
        public int AddByYear(CreatePartnerCalendarDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = _partnerCalendarRepository.AddByYear(new BondPartnerCalendar
            {
                WorkingYear = input.WorkingYear,
                PartnerId = partnerId
            });
            return result;
        }

        public List<BondPartnerCalendar> FindAll(int workingYear)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _partnerCalendarRepository.FindAllByWorkingYear(workingYear, partnerId);
        }

        public BondPartnerCalendar FindByWorkingDate(DateTime date)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _partnerCalendarRepository.FindByWorkingDate(date, partnerId);
        }

        public int Update(UpdatePartnerCalendarDto input)
        {
            var partnerCalendar = FindByWorkingDate(input.WorkingDate);

            partnerCalendar.BusDate = input.BusDate;
            partnerCalendar.IsDayOff = input.IsDayOff;

            return _partnerCalendarRepository.Update(partnerCalendar);
        }
    }
}
