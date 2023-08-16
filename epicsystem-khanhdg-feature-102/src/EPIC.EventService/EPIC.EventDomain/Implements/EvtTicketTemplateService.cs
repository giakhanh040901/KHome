using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Dto.EvtTicketTemplate;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtTicketTemplateService : IEvtTicketTemplateService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtTicketTemplateService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtTicketTemplateEFRepository _evtTicketTemplateEFRepository;

        public EvtTicketTemplateService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtTicketTemplateService> logger,
            IHttpContextAccessor httpContextAccessor
           )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _evtTicketTemplateEFRepository = new EvtTicketTemplateEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Thêm mẫu vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ResponseEvtTicketTemplateDto Add(CreateEvtTicketTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            if(!_dbContext.EvtEvents.Any(e => e.Deleted == YesNo.NO && e.Id == input.EventId && e.TradingProviderId == tradingProviderId)) {
                _evtTicketTemplateEFRepository.ThrowException(ErrorCode.EvtEventNotFound);
            }
            
            var insert = _mapper.Map<EvtTicketTemplate>(input);
            insert.Id = (int)_evtTicketTemplateEFRepository.NextKey();
            if(!_dbContext.EvtTicketTemplates.Any(e => e.Deleted == YesNo.NO && e.EventId == input.EventId && e.Status == StatusCommon.ACTIVE))
            {
                insert.Status = StatusCommon.ACTIVE;
            };
           
            var result = _dbContext.EvtTicketTemplates.Add(insert);
            _dbContext.SaveChanges();

            return _mapper.Map<ResponseEvtTicketTemplateDto>(result.Entity);
        }

        /// <summary>
        /// sửa mẫu vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ResponseEvtTicketTemplateDto Update(UpdateEvtTicketTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            if (!_dbContext.EvtEvents.Any(e => e.Deleted == YesNo.NO && e.Id == input.EventId && e.TradingProviderId == tradingProviderId))
            {
                _evtTicketTemplateEFRepository.ThrowException(ErrorCode.EvtEventNotFound);
            }

            var ticketTemplateFind = _dbContext.EvtTicketTemplates
                                                    .FirstOrDefault(e => e.Deleted == YesNo.NO
                                                                    && e.Id == input.Id
                                                                    && e.EventId == input.EventId).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);

            var updatedTemplate = _mapper.Map(input, ticketTemplateFind);
            _dbContext.SaveChanges();

            return _mapper.Map<ResponseEvtTicketTemplateDto>(updatedTemplate);
        }

        /// <summary>
        /// danh sách mẫu vé của từng event
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<ResponseEvtTicketTemplateDto> FindAll(FilterEvtTicketTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            if (!_dbContext.EvtEvents.Any(e => e.Deleted == YesNo.NO && e.Id == input.EventId && e.TradingProviderId == tradingProviderId))
            {
                _evtTicketTemplateEFRepository.ThrowException(ErrorCode.EvtEventNotFound);
            }

            var result = new PagingResult<ResponseEvtTicketTemplateDto>();
            var query = _dbContext.EvtTicketTemplates.Where(t => t.Deleted == YesNo.NO && t.EventId == input.EventId)
                                            .Select(t => new ResponseEvtTicketTemplateDto
                                            {
                                                Id = t.Id,
                                                EventId = t.EventId,
                                                FileUrl = t.FileUrl,
                                                CreatedBy = t.CreatedBy,
                                                CreatedDate = t.CreatedDate,
                                                Name = t.Name,
                                                Status = t.Status,
                                            });
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        /// <summary>
        /// kích hoạt và hủy mẫu vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)} : id = {id}, tradingProviderId = {tradingProviderId}");

            var ticketTemplateFind = _dbContext.EvtTicketTemplates.Include(t => t.Event)
                                                    .FirstOrDefault(e => e.Deleted == YesNo.NO
                                                                    && e.Event.TradingProviderId == tradingProviderId
                                                                    && e.Id == id).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
            var activeTicketTemplates = _dbContext.EvtTicketTemplates
                                       .Include(t => t.Event)
                                       .Where(e => e.Deleted == YesNo.NO
                                                   && e.Event.TradingProviderId == tradingProviderId
                                                   && e.Event.Id == ticketTemplateFind.EventId
                                                   && e.Status == StatusCommon.ACTIVE);

            if(ticketTemplateFind.Status == StatusCommon.DEACTIVE)
            {
                foreach (var ticketTemplate in activeTicketTemplates)
                {
                    ticketTemplate.Status = StatusCommon.DEACTIVE;
                }
            }
            
            ticketTemplateFind.Status = (ticketTemplateFind.Status == StatusCommon.ACTIVE) 
                                            ? StatusCommon.DEACTIVE 
                                                : StatusCommon.ACTIVE;
            _dbContext.SaveChanges();
        }
    }
}
