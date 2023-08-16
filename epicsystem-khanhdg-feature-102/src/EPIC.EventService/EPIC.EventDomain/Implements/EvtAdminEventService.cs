using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtAdminEvent;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.Utils;
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

namespace EPIC.EventDomain.Implements
{
    public class EvtAdminEventService : IEvtAdminEventService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<EvtAppEventHistoryServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly EvtAdminEventEFRepository _evtAdminEventEFRepository;

        public EvtAdminEventService(
            EpicSchemaDbContext dbContext,
            ILogger<EvtAppEventHistoryServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper
            )
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _httpContext = httpContext;
            _mapper = mapper;
            _evtAdminEventEFRepository = new EvtAdminEventEFRepository(_dbContext, _logger);
        }

        public void Add(CreateEvtAdminEventDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            if (input.InvestorIds != null)
            {
                foreach (var item in input.InvestorIds)
                {
                    if (!_dbContext.EvtAdminEvents.Where(e => e.InvestorId == item && e.EventId == input.EventId).Any())
                    {
                        var insert = new EvtAdminEvent
                        {
                            Id = (int)_evtAdminEventEFRepository.NextKey(),
                            EventId = input.EventId,
                            InvestorId = item
                        };
                        _dbContext.EvtAdminEvents.Add(insert);
                    }
                    else
                    {
                        _evtAdminEventEFRepository.ThrowException(ErrorCode.EvtEventAdminExists);
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");
            var admin = _dbContext.EvtAdminEvents.FirstOrDefault(ae => ae.Id == id).ThrowIfNull(_dbContext, ErrorCode.EvtAdminEventNotFound);
            _dbContext.EvtAdminEvents.Remove(admin);
            _dbContext.SaveChanges();
        }

        public IEnumerable<EvtAdminEventDto> FindAll(int eventId)
        {
            _logger.LogInformation($"{nameof(Delete)}: eventId = {eventId}");
            var admins = _dbContext.EvtAdminEvents
                .Include(e => e.Event)
                .Include(e => e.Investor).ThenInclude(i => i.InvestorIdentifications)
                .Where(ae => ae.EventId == eventId)
                .Select(ae => new EvtAdminEventDto
                {
                    Id = ae.Id,
                    EventId = ae.EventId,
                    EventName = ae.Event.Name,
                    InvestorId = ae.InvestorId,
                    Phone = ae.Investor.Phone,
                    FullName = ae.Investor.InvestorIdentifications.OrderByDescending(id => id.IsDefault).ThenByDescending(e => e.Id).Select(e => e.Fullname).FirstOrDefault(),
                });
            return admins;
        }
    }
}
