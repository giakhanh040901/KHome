using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.Utils;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;

namespace EPIC.EventDomain.Implements
{
    public class EvtAppEventMediaService : IEvtAppEventMediaService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtAppEventMediaService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;

        public EvtAppEventMediaService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtAppEventMediaService> logger,
            IHttpContextAccessor httpContextAccessor
           )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
        }
        #region App 
        /// <summary>
        /// danh sách hình ảnh sự kiện 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<AppEventMediaDto> FindEventMediaByEventId(AppFilterEventMediaDto input)
        {
            _logger.LogInformation($"{nameof(FindEventMediaByEventId)} : input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<AppEventMediaDto>();
            var query = _dbContext.EvtEventMedias.Include(e => e.EventMediaDetails).Include(e => e.Event)
                                                .Where(e => e.Deleted == YesNo.NO
                                                             && e.Event.IsShowApp == YesNo.YES
                                                                && (input.MediaDetailType == null || e.EventMediaDetails.Any(em => em.MediaType == input.MediaType))
                                                             && (input.MediaType == null || e.MediaType == input.MediaType)
                                                             && e.EventId == input.EventId)
                                                .Select(e => new AppEventMediaDto
                                                {
                                                    Id = e.Id,
                                                    EventId = e.EventId,
                                                    GroupTitle = e.GroupTitle,
                                                    UrlImage = e.UrlImage,
                                                    UrlPath = e.UrlPath,
                                                    MediaType = e.MediaType,
                                                    Location = e.Location,
                                                    EvtEventMediaDetails = e.EventMediaDetails.Where(em => em.Deleted == YesNo.NO)
                                                        .Select(em => new AppEventMediaDetailDto
                                                        {
                                                            Id = em.Id,
                                                            UrlImage = em.UrlImage,
                                                            SortOrder = em.SortOrder,
                                                            MediaType = em.MediaType,
                                                        }),
                                                })
                                                .AsQueryable();
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }
        #endregion
    }
}
