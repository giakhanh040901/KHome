using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDetail;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.EventEntites.Dto.EvtEventMediaDetail;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.Utils;
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
    public class EvtEventMediaDetailServices : IEvtEventMediaDetailServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtEventMediaDetailServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly EvtEventMediaEFRepository _evtEventMediaEFRepository;
        private readonly EvtEventMediaDetailEFRepository _evtEventMediaDetailEFRepository;

        public EvtEventMediaDetailServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtEventMediaDetailServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IFileExtensionServices fileExtensionServices,
            IFileServices fileServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _fileExtensionServices = fileExtensionServices;
            _fileServices = fileServices;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _evtEventMediaEFRepository = new EvtEventMediaEFRepository(dbContext, logger);
            _evtEventMediaDetailEFRepository = new EvtEventMediaDetailEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm nhóm hình ảnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IEnumerable<EvtEventMediaDetailDto> Add(CreateEvtEventMediaDetailsDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var mediaList = new List<EvtEventMediaDetailDto>();
            var transaction = _dbContext.Database.BeginTransaction();

            var eventMedia = new EvtEventMedia()
            {
                Id = (int)_evtEventMediaEFRepository.NextKey(),
                GroupTitle = input.GroupTitle,
                EventId = input.EventId,
                TradingProviderId = tradingProviderId,
                CreatedBy = username
            };
            _evtEventMediaEFRepository.Entity.Add(eventMedia);
            _dbContext.SaveChanges();

            foreach (var item in input.EventMediaDetails)
            {
                var insert = new EvtEventMediaDetail
                {
                    Id = (int)_evtEventMediaDetailEFRepository.NextKey(),
                    TradingProviderId = tradingProviderId,
                    UrlImage = item.UrlImage,
                    CreatedBy = username,
                    EventMediaId = eventMedia.Id,
                    MediaType = _fileExtensionServices.GetMediaExtensionFile(item.UrlImage),
                };
                insert.SortOrder = insert.Id;
                _evtEventMediaDetailEFRepository.Entity.Add(insert);
                
                mediaList.Add(new EvtEventMediaDetailDto
                {
                    Id = insert.Id,
                    TradingProviderId = insert.TradingProviderId,
                    UrlImage = insert.UrlImage,
                    EventMediaId = insert.EventMediaId,
                    Status = insert.Status,
                    SortOrder = insert.SortOrder,
                    MediaType = insert.MediaType,
                    Deleted = insert.Deleted
                });
            }

            _dbContext.SaveChanges();
            transaction.Commit();
            return mediaList;
        }

        /// <summary>
        /// update thứ tự nhóm hình ảnh dự án
        /// </summary>
        /// <returns></returns>
        public void UpdateSortOrder(EvtEventMediaDetailSortDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateSortOrder)}: input = {input}, partnerId = {tradingProviderId}");

            foreach (var item in input.Sort)
            {
                var mediaFind = _evtEventMediaDetailEFRepository.Entity
                                    .FirstOrDefault(e => e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO
                                        && e.EventMedia.EventId == input.EventId && e.Id == item.EventMediaDetailId);
                if (mediaFind != null)
                {
                    mediaFind.SortOrder = item.SortOrder;
                }
            }
            _dbContext.SaveChanges();
        }

        public IEnumerable<EvtEventMediaDetailDto> AddListMediaDetail(AddEvtEventMediaDetailsDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(AddListMediaDetail)}:input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var mediaDetailList = new List<EvtEventMediaDetailDto>();

            foreach (var item in input.EventMediaDetails)
            {
                var insert = new EvtEventMediaDetail
                {
                    Id = (int)_evtEventMediaDetailEFRepository.NextKey(),
                    TradingProviderId = tradingProviderId,
                    UrlImage = item.UrlImage,
                    CreatedBy = username,
                    EventMediaId = input.EventMediaId,
                    MediaType = _fileExtensionServices.GetMediaExtensionFile(item.UrlImage),
                };
                insert.SortOrder = insert.Id;
                _evtEventMediaDetailEFRepository.Entity.Add(insert);
                mediaDetailList.Add(new EvtEventMediaDetailDto
                {
                    Id = insert.Id,
                    TradingProviderId = insert.TradingProviderId,
                    UrlImage = insert.UrlImage,
                    EventMediaId = insert.EventMediaId,
                    Status = insert.Status,
                    SortOrder = insert.SortOrder,
                    MediaType = insert.MediaType,
                    Deleted = insert.Deleted
                });
            }
            _dbContext.SaveChanges();
            return mediaDetailList;
        }

        /// <summary>
        /// Cập nhật nhóm hình ảnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EvtEventMediaDetailDto Update(UpdateEvtEventMediaDetailDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var mediaDetailFind = _evtEventMediaDetailEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaDetailNotFound);
            mediaDetailFind.UrlImage = input.UrlImage;
            mediaDetailFind.ModifiedBy = username;
            mediaDetailFind.ModifiedDate = DateTime.Now;
            mediaDetailFind.MediaType = _fileExtensionServices.GetMediaExtensionFile(mediaDetailFind.UrlImage);

            _dbContext.SaveChanges();
            return _mapper.Map<EvtEventMediaDetailDto>(mediaDetailFind);
        }

        /// <summary>
        /// Xoá nhóm hình ảnh
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, tradingProviderId = {tradingProviderId}");

            var media = _evtEventMediaDetailEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaDetailNotFound);

            _dbContext.Remove(media);
            _dbContext.SaveChanges();
            var listUrlImageMedia = _evtEventMediaDetailEFRepository.EntityNoTracking.Where(o => o.UrlImage == media.UrlImage && o.Deleted == YesNo.NO);
            if (!listUrlImageMedia.Any())
            {
                _fileServices.DeleteFile(media.UrlImage);
            }
        }

        /// <summary>
        /// Tìm nhóm hình ảnh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EvtEventMediaDetailDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");
            var media = _evtEventMediaDetailEFRepository.Entity.Select(e => new EvtEventMediaDetailDto
            {
                Id = e.Id,
                TradingProviderId = e.TradingProviderId,
                EventMediaId = e.EventMediaId,
                UrlImage = e.UrlImage,
                MediaType = e.MediaType,
                Status = e.Status,
                SortOrder = e.SortOrder,
                Deleted = e.Deleted
            }).FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaDetailNotFound);
            return media;
        }

        /// <summary>
        /// Đổi trạng thái nhóm hình ảnh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void ChangeStatus(int id, string status)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, tradingProviderId = {tradingProviderId}");

            var media = _evtEventMediaDetailEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtEventMediaDetailNotFound);
            media.Status = status;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Danh sách nhóm hình ảnh dự án order by theo tên nhóm
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvtEventMediaDto> Find(int eventId, string status)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Find)}, tradingProviderId = {tradingProviderId}, eventId = {eventId}, status = {status}");
            var result = _evtEventMediaEFRepository.EntityNoTracking
                            .Where(o => o.EventId == eventId && o.Deleted == YesNo.NO && o.TradingProviderId == tradingProviderId && o.GroupTitle != null)
                            .OrderByDescending(o => o.Location).ThenBy(o => o.SortOrder)
                            .Select(e => new EvtEventMediaDto
                            {
                                Id = e.Id,
                                EventId = e.EventId,
                                GroupTitle = e.GroupTitle,
                                EvtEventMediaDetail = e.EventMediaDetails.Where(o => o.Deleted == YesNo.NO
                                                        && (status == null || o.Status == status))
                                                        .Select(detail => new EvtEventMediaDetailDto
                                                        {
                                                            Id = detail.Id,
                                                            TradingProviderId = detail.TradingProviderId,
                                                            GroupTitle = detail.EventMedia.GroupTitle,
                                                            EventMediaId = detail.EventMediaId,
                                                            MediaType = detail.MediaType,
                                                            UrlImage = detail.UrlImage,
                                                            Status = detail.Status,
                                                            Location = detail.EventMedia.Location,
                                                            SortOrder = detail.SortOrder,
                                                        })
                            });
            return result;
        }
    }
}
