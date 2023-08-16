using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Dto.EvtTicketMedia;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.Utils;
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
    public class EvtEventDescriptionMediaServices : IEvtEventDescriptionMediaServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtEventDescriptionMediaServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;
        private readonly EvtEventDescriptionMediaEFRepository _evtEventDescriptionMediaEFRepository;

        public EvtEventDescriptionMediaServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtEventDescriptionMediaServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IFileExtensionServices fileExtensionServices,
            IFileServices fileServices
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _fileExtensionServices = fileExtensionServices;
            _fileServices = fileServices;
            _evtEventDescriptionMediaEFRepository = new EvtEventDescriptionMediaEFRepository(_dbContext, logger);
        }

        public IEnumerable<EvtEventDescriptionMediaDto> Add(CreateEvtEventDescriptionDto input)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingproviderId}");
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == input.EventId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventNotFound);
            var result = new List<EvtEventDescriptionMediaDto>();
            foreach (var item in input.EventDescriptionMedias)
            {
                var insertMedia = new EvtEventDescriptionMedia()
                {
                    Id = (int)_evtEventDescriptionMediaEFRepository.NextKey(),
                    EventId = input.EventId,
                    UrlImage = item.UrlImage,
                    MediaType = _fileExtensionServices.GetMediaExtensionFile(item.UrlImage)
                };
                _dbContext.EvtEventDescriptionMedias.Add(insertMedia);
                result.Add(_mapper.Map<EvtEventDescriptionMediaDto>(insertMedia));
            };
            _dbContext.SaveChanges();
            return result;
        }

        public void Update(UpdateEvtEventDescriptionMediaDto input)
        {
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}");
            var descriptionMedia = _dbContext.EvtEventDescriptionMedias.FirstOrDefault(e => e.Id == input.Id).ThrowIfNull(_dbContext, ErrorCode.EvtEventDescriptionMediaNotFound);

            var image = _dbContext.EvtEventDescriptionMedias.Where(e => e.UrlImage == descriptionMedia.UrlImage);
            if (!image.Any())
            {
                //Xóa ảnh
                _fileServices.DeleteFile(descriptionMedia.UrlImage);
            }

            descriptionMedia.UrlImage = input.UrlImage;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)} : id = {id}");
            var descriptionMedia = _dbContext.EvtEventDescriptionMedias.FirstOrDefault(e => e.Id == id).ThrowIfNull(_dbContext, ErrorCode.EvtEventDescriptionMediaNotFound);

            _dbContext.EvtEventDescriptionMedias.Remove(descriptionMedia);
            _dbContext.SaveChanges();

            var image = _dbContext.EvtEventDescriptionMedias.Where(e => e.UrlImage == descriptionMedia.UrlImage);
            if (!image.Any())
            {
                //Xóa ảnh
                _fileServices.DeleteFile(descriptionMedia.UrlImage);
            }
        }

        public IEnumerable<EvtEventDescriptionMediaDto> FindByEventId(int eventId)
        {
            _logger.LogInformation($"{nameof(FindByEventId)} : eventId = {eventId}");
            var descriptionMedia = _dbContext.EvtEventDescriptionMedias.Where(e => e.EventId == eventId)
                                            .Select(e => new EvtEventDescriptionMediaDto
                                            {
                                                Id = e.EventId,
                                                EventId = e.EventId,
                                                UrlImage = e.UrlImage,
                                            });
            return descriptionMedia;
        }
    }
}
