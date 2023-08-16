using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.PartnerCalendar;
using EPIC.CompanySharesRepositories;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Implements
{
    public class PartnerCalendarServices : IPartnerCalendarServices
    {
        private readonly ILogger<PartnerCalendarServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly PartnerCalendarRepository _partnerCalendarRepository;
        private readonly IHttpContextAccessor _httpContext;

        public PartnerCalendarServices(ILogger<PartnerCalendarServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _partnerCalendarRepository = new PartnerCalendarRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }
        public int AddByYear(CreatePartnerCalendarDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = _partnerCalendarRepository.AddByYear(new PartnerCalendar
            {
                WorkingYear = input.WorkingYear,
                PartnerId = partnerId
            });
            return result;
        }

        public List<PartnerCalendar> FindAll(int workingYear)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _partnerCalendarRepository.FindAllByWorkingYear(workingYear, partnerId);
        }

        public PartnerCalendar FindByWorkingDate(DateTime date)
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
