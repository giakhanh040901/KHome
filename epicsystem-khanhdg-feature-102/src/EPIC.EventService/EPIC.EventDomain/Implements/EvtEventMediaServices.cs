using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.EventEntites.Dto.EvtEventMediaDetail;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.Utils;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.EventDomain.Implements
{
    public class EvtEventMediaServices : IEvtEventMediaServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtEventMediaServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly EvtEventMediaEFRepository _evtEventMediaEFRepository;
        private readonly EvtEventMediaDetailEFRepository _evtEventMediaDetailEFRepository;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;

        public EvtEventMediaServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtEventMediaServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IFileExtensionServices fileExtensionServices,
            IFileServices fileServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _evtEventMediaEFRepository = new EvtEventMediaEFRepository(dbContext, logger);
            _evtEventMediaDetailEFRepository = new EvtEventMediaDetailEFRepository(dbContext, logger);
            _fileExtensionServices = fileExtensionServices;
            _fileServices = fileServices;
        }

        /// <summary>
        /// Thêm hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ViewEvtEventMediaDto> Add(CreateEvtEventMediasDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            var mediaList = new List<ViewEvtEventMediaDto>();
            foreach (var item in input.EventMedias)
            {
                var insert = new EvtEventMedia {
                    Id = (int)_evtEventMediaEFRepository.NextKey(),
                    TradingProviderId = tradingProviderId,
                    EventId = item.EventId,
                    GroupTitle = item.GroupTitle,
                    UrlImage = item.UrlImage,
                    UrlPath = item.UrlPath,
                    CreatedBy = username,
                    Location = input.Location
                };
                if (item.UrlImage != null)
                {
                    insert.MediaType = _fileExtensionServices.GetMediaExtensionFile(item.UrlImage);
                    insert.SortOrder = insert.Id;
                }
                _dbContext.Add(insert);
                mediaList.Add(_mapper.Map<ViewEvtEventMediaDto>(insert));
            }

            _dbContext.SaveChanges();
            return mediaList;
        }

        /// <summary>
        /// Cập nhật hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ViewEvtEventMediaDto Update(UpdateEvtEventMediaDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            var mediaFind = _evtEventMediaEFRepository.Entity
                .FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaNotFound);
            if (input.GroupTitle == null)
            {
                mediaFind.UrlPath = input.UrlPath;
                mediaFind.UrlImage = input.UrlImage;
                mediaFind.MediaType = _fileExtensionServices.GetMediaExtensionFile(mediaFind.UrlImage);
            }
            else
            {
                mediaFind.GroupTitle = input.GroupTitle;
            }
            mediaFind.ModifiedBy = username;
            mediaFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return _mapper.Map<ViewEvtEventMediaDto>(mediaFind);
        }

        /// <summary>
        /// Xoá hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, username = {username}, tradingProviderId = {tradingProviderId}");

            var media = _evtEventMediaEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaNotFound);

            var mediaDetail = _evtEventMediaDetailEFRepository.Entity
                .Where(e => e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO && e.EventMedia.Id == media.Id);
            _evtEventMediaDetailEFRepository.Entity.RemoveRange(mediaDetail);

            foreach (var item in mediaDetail)
            {
                var listUrlImageMediaDetail = _evtEventMediaDetailEFRepository.EntityNoTracking.Where(o => o.UrlImage == item.UrlImage && o.Deleted == YesNo.NO);
                if (!listUrlImageMediaDetail.Any())
                {
                    _fileServices.DeleteFile(item.UrlImage);
                }
            }

            _dbContext.Remove(media);
            _dbContext.SaveChanges();
            var listUrlImageMedia = _evtEventMediaEFRepository.EntityNoTracking.Where(o => o.UrlImage == media.UrlImage && o.Deleted == YesNo.NO);
            if (!listUrlImageMedia.Any())
            {
                _fileServices.DeleteFile(media.UrlImage);
            }
        }

        /// <summary>
        /// Tìm hình ảnh sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewEvtEventMediaDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");

            var media = _evtEventMediaEFRepository.EntityNoTracking.Select(e => new ViewEvtEventMediaDto
            {
                Id = e.Id,
                TradingProviderId = e.TradingProviderId,
                GroupTitle = e.GroupTitle,
                MediaType = e.MediaType,
                UrlImage = e.UrlImage,
                Status = e.Status,
                EventId = e.EventId,
                Location = e.Location,
                SortOrder = e.SortOrder,
                UrlPath = e.UrlPath,
                Deleted = e.Deleted
            }).FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaNotFound);
            return media;
        }

        /// <summary>
        /// Danh sách hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<ViewEvtEventMediaDto> FindAll(FilterEvtEventMediaDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<ViewEvtEventMediaDto> result = new();
            IQueryable<ViewEvtEventMediaDto> mediaQuery = _evtEventMediaEFRepository.EntityNoTracking.Where(p => p.Deleted == YesNo.NO 
                                                    && p.TradingProviderId == tradingProviderId
                                                    && (input.Status == null || p.Status == input.Status)
                                                    && (input.Location == null || p.Location == input.Location)
                                                    && (input.EventId == null || p.EventId == input.EventId)
                                                    && (input.MediaType == null || p.MediaType == input.MediaType))
                                                    .Select(e => new ViewEvtEventMediaDto
                                                    {
                                                        Id = e.Id,
                                                        TradingProviderId = e.TradingProviderId,
                                                        GroupTitle = e.GroupTitle,
                                                        MediaType = e.MediaType,
                                                        UrlImage = e.UrlImage,
                                                        Status = e.Status,
                                                        EventId = e.EventId,
                                                        Location = e.Location,
                                                        SortOrder = e.SortOrder,
                                                        UrlPath = e.UrlPath,
                                                        Deleted = e.Deleted
                                                    });
            result.TotalItems = mediaQuery.Count();
            mediaQuery = mediaQuery.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                mediaQuery = mediaQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = mediaQuery;
            return result;
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void ChangeStatus(int id, string status)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, partnerId = {tradingProviderId}");

            var media = _evtEventMediaEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaNotFound);
            media.Status = status;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Danh sách hình ảnh sự kiện orderBy location
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViewEvtEventMediaDto> Find(int eventId, string location, string status)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Find)}: eventId = {eventId}, tradingProviderId = {tradingProviderId}, " +
                $" location = {location}, status = {status}");

            var result = _evtEventMediaEFRepository.EntityNoTracking.Where(o => o.EventId == eventId && o.Deleted == YesNo.NO
                                                                            && o.TradingProviderId == tradingProviderId
                                                                            && (location == null || o.Location == location)
                                                                            && (status == null || o.Status == status))
                                                                    .Select(e => new ViewEvtEventMediaDto
                                                                    {
                                                                        Id = e.Id,
                                                                        TradingProviderId = e.TradingProviderId,
                                                                        GroupTitle = e.GroupTitle,
                                                                        MediaType = e.MediaType,
                                                                        UrlImage = e.UrlImage,
                                                                        Status = e.Status,
                                                                        EventId = e.EventId,
                                                                        Location = e.Location,
                                                                        SortOrder = e.SortOrder,
                                                                        UrlPath = e.UrlPath,
                                                                        Deleted = e.Deleted
                                                                    }); 
            result = result.OrderByDescending(o => o.Location).ThenBy(o => o.SortOrder);
            return result.ToList();
        }

        /// <summary>
        /// update thứ tự hình ảnh dự án
        /// </summary>
        /// <returns></returns>
        public void UpdateSortOrder(EvtEventMediaSortDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateSortOrder)}: input = {input}, tradingProviderId = {tradingProviderId}");

            foreach (var item in input.Sort)
            {
                var mediaFind = _evtEventMediaEFRepository.Entity.FirstOrDefault(o => o.EventId == input.EventId && o.Deleted == YesNo.NO
                                                                            && o.TradingProviderId == tradingProviderId
                                                                            && o.Id == item.EventMediaId
                                                                            && (input.Location == null || o.Location == input.Location));
                if (mediaFind != null)
                {
                    mediaFind.SortOrder = item.SortOrder;
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
