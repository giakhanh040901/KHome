using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Dto.EvtTicketMedia;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
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
    public class EvtTicketService : IEvtTicketService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtTicketService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtTicketEFRepository _evtTicketEFRepository;
        private readonly EvtTicketMediaEFRepository _evtTicketMediaEFRepository;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;

        public EvtTicketService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtTicketService> logger,
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
            _evtTicketEFRepository = new EvtTicketEFRepository(_dbContext, _logger);
            _evtTicketMediaEFRepository = new EvtTicketMediaEFRepository(_dbContext, _logger);
            _fileExtensionServices = fileExtensionServices;
            _fileServices = fileServices;

        }

        public EvtTicketDto Add(CreateEvtTicketDto input)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingproviderId}");
            var eventDetail = _dbContext.EvtEventDetails.FirstOrDefault(e => e.Id == input.EventDetailId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtEventDetailNotFound);
            if ((eventDetail.StartDate != null && input.StartSellDate > eventDetail.EndDate) || (eventDetail.EndDate != null && input.EndSellDate > eventDetail.EndDate))
            {
                _evtTicketEFRepository.ThrowException(ErrorCode.EvtTicketIsPastTheEventTime);
            }
            var transaction = _dbContext.Database.BeginTransaction();
            EvtTicket inputInsert = _mapper.Map<EvtTicket>(input);
            inputInsert.Id = (int)_evtTicketEFRepository.NextKey();
            _dbContext.EvtTickets.Add(inputInsert);
            _dbContext.SaveChanges();
            if (input.UrlImages != null)
            {
                foreach (var item in input.UrlImages)
                {
                    var insertMedia = new EvtTicketMedia()
                    {
                        Id = (int)_evtTicketMediaEFRepository.NextKey(),
                        TicketId = inputInsert.Id,
                        UrlImage = item.UrlImage,
                        MediaType = _fileExtensionServices.GetMediaExtensionFile(item.UrlImage)
                    };
                    _dbContext.EvtTicketMedias.Add(insertMedia);
                };
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return _mapper.Map<EvtTicketDto>(inputInsert);
        }

        public void Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)} : id = {id}, tradingproviderId = {tradingProviderId}");
            var ticket = _dbContext.EvtTickets.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO ).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
            ticket.Deleted = YesNo.YES;

            var orderDetail = _dbContext.EvtOrderDetails.Include(od => od.Order).Where(od => od.TicketId == id && od.Order.Deleted == YesNo.NO);
            if (orderDetail.Any())
            {
                _evtTicketEFRepository.ThrowException(ErrorCode.EvtTicketIsInUse);
            }

            var ticketMedia = _dbContext.EvtTicketMedias.Where(tm => tm.TicketId == id);
            foreach (var item in ticketMedia)
            {
                _dbContext.EvtTicketMedias.Remove(item);
                _dbContext.SaveChanges();

                var image = _dbContext.EvtTicketMedias.Where(e => e.UrlImage == item.UrlImage);
                if (!image.Any())
                {
                    //Xóa ảnh
                    _fileServices.DeleteFile(item.UrlImage);
                }
            }
            _dbContext.SaveChanges();
        }

        public void DeleteTicketImage(int id)
        {
            _logger.LogInformation($"{nameof(DeleteTicketImage)} : id = {id}");
            var ticketMedia = _dbContext.EvtTicketMedias.FirstOrDefault(e => e.Id == id).ThrowIfNull(_dbContext, ErrorCode.EvtTicketMediaNotFound);
          
            _dbContext.EvtTicketMedias.Remove(ticketMedia);
            _dbContext.SaveChanges();

            var image = _dbContext.EvtTicketMedias.Where(e => e.UrlImage == ticketMedia.UrlImage);
            if (!image.Any())
            {
                //Xóa ảnh
                _fileServices.DeleteFile(ticketMedia.UrlImage);
            }
        }

        public PagingResult<EvtTicketDto> FindAll(FilterEvtTicketDto input)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingproviderId}");
            var result = new PagingResult<EvtTicketDto>();
            var query = _dbContext.EvtTickets.Where(t => t.Deleted == YesNo.NO && t.EventDetailId == input.EventDetailId)
                                            .Select(t => new EvtTicketDto
                                            {
                                                Id = t.Id,
                                                EventDetailId = t.EventDetailId,
                                                ContentType = t.ContentType,
                                                CreatedBy = t.CreatedBy,
                                                Description = t.Description,
                                                EndSellDate = t.EndSellDate,
                                                IsFree = t.IsFree,
                                                IsShowApp = t.IsShowApp,
                                                MaxBuy = t.MaxBuy,
                                                MinBuy = t.MinBuy,
                                                Name= t.Name,
                                                OverviewContent = t.OverviewContent,
                                                Price = t.Price,
                                                Quantity = t.Quantity,
                                                StartSellDate = t.StartSellDate,
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

        public IEnumerable<EvtTicketDto> ReplicateTicket(CreateListRepicateTicketDto input)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ReplicateTicket)} : input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingproviderId}");
            var result = new List<EvtTicketDto>();
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var ticket in input.ReplicateTickets)
            {
                var ticketFind = _dbContext.EvtTickets.FirstOrDefault(t => t.Id == ticket && t.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
                var ticketInsert = new EvtTicket
                {
                    Id = (int)_evtTicketEFRepository.NextKey(),
                    ContentType = ticketFind.ContentType,
                    Description = ticketFind.Description,
                    StartSellDate = ticketFind.StartSellDate,
                    EndSellDate = ticketFind.EndSellDate,
                    EventDetailId = input.EventDetailId,
                    IsShowApp = ticketFind.IsShowApp,
                    IsFree = ticketFind.IsFree,
                    Name = ticketFind.Name,
                    Price = ticketFind.Price,
                    Quantity = ticketFind.Quantity,
                    MinBuy = ticketFind.MinBuy,
                    MaxBuy = ticketFind.MaxBuy,
                    Status = ticketFind.Status,
                    OverviewContent = ticketFind.OverviewContent,
                };
                _dbContext.EvtTickets.Add(ticketInsert);
                _dbContext.SaveChanges();
                result.Add(_mapper.Map<EvtTicketDto>(ticketInsert));
                var ticketMediaFind = _dbContext.EvtTicketMedias.Where(t => t.TicketId == ticketFind.Id);
                foreach (var item in ticketMediaFind)
                {
                    var insertMedia = new EvtTicketMedia
                    {
                        Id = (int)_evtTicketMediaEFRepository.NextKey(),
                        TicketId = ticketInsert.Id,
                        UrlImage = item.UrlImage,
                        MediaType = item.MediaType,
                    };
                    _dbContext.EvtTicketMedias.Add(insertMedia);
                    _dbContext.SaveChanges();
                }
            }

            transaction.Commit();
            return result;
        }

        /// <summary>
        /// Cập nhật thông tin vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EvtTicketDto Update(UpdateEvtTicketDto input)
        {
            var tradingproviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingproviderId}");
            var ticket = _dbContext.EvtTickets.FirstOrDefault(e => e.Id == input.Id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
            ticket.Quantity = input.Quantity;
            ticket.ContentType = input.ContentType;
            ticket.OverviewContent = input.OverviewContent;
            ticket.IsShowApp = input.IsShowApp;
            ticket.IsFree = input.IsFree;
            ticket.MaxBuy = input.MaxBuy;
            ticket.MinBuy = input.MinBuy;
            ticket.Name = input.Name;
            ticket.Price = input.Price;
            ticket.StartSellDate = input.StartSellDate;
            ticket.EndSellDate = input.EndSellDate;
            ticket.Description = input.Description;

            //Xóa ảnh
            var ticketMedia = _dbContext.EvtTicketMedias.Where(e => e.TicketId == input.Id);
            foreach (var removeItem in ticketMedia)
            {
                if (!input.UrlImages.Where(t => t.Id != null).Select(t => t.Id).Contains(removeItem.Id))
                {
                    _dbContext.Remove(removeItem);
                    var image = _dbContext.EvtTicketMedias.Where(e => e.UrlImage == removeItem.UrlImage);
                    if (!image.Any())
                    {
                        //Xóa ảnh
                        _fileServices.DeleteFile(removeItem.UrlImage);
                    }
                }
            }
            //Cập nhật và thêm mới
            foreach (var item in input.UrlImages)
            {
                // Có id thì cập nhật
                if (item.Id != null)
                {
                    var updateImage = _dbContext.EvtTicketMedias.FirstOrDefault(tm => tm.Id == item.Id);
                    if (updateImage != null)
                    {
                        updateImage.UrlImage = item.UrlImage;
                    }
                }
                // Không có id thì thêm mới
                else
                {
                    var insertImage = new EvtTicketMedia
                    {
                        Id = (int)_evtTicketMediaEFRepository.NextKey(),
                        TicketId = input.Id,
                        UrlImage = item.UrlImage
                    };
                    _dbContext.EvtTicketMedias.Add(insertImage);
                }
            }
            _dbContext.SaveChanges();
            return _mapper.Map<EvtTicketDto>(ticket);
        }

        /// <summary>
        /// Thay đổi trạng thái của vé
        /// </summary>
        /// <param name="id"></param>
        public void UpdateStatus(int id)
        {
            _logger.LogInformation($"{nameof(UpdateStatus)} : id = {id}");
            var ticket = _dbContext.EvtTickets.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
            if (ticket.Status == EvtTicketStatus.KICH_HOAT)
            {
                ticket.Status = EvtTicketStatus.HUY_KICH_HOAT;
            } 
            else
            {
                ticket.Status = EvtTicketStatus.KICH_HOAT;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thay đổi trạng thái show app của vé
        /// </summary>
        /// <param name="id"></param>
        public void ChangeShowApp(int id)
        {
            _logger.LogInformation($"{nameof(ChangeShowApp)} : id = {id}");
            var ticket = _dbContext.EvtTickets.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
            if (ticket.IsShowApp == YesNo.YES)
            {
                ticket.IsShowApp = YesNo.NO;
            }
            else
            {
                ticket.IsShowApp = YesNo.YES;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Find by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EvtTicketDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(ChangeShowApp)} : id = {id}");
            var ticket = _dbContext.EvtTickets.Include(e => e.TicketMedias).FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtTicketNotFound);
            return new EvtTicketDto
            {
                Id = ticket.Id,
                Name = ticket.Name,
                IsFree = ticket.IsFree,
                Price = ticket.Price,
                Quantity = ticket.Quantity,
                MinBuy = ticket.MinBuy,
                MaxBuy = ticket.MaxBuy,
                StartSellDate = ticket.StartSellDate,
                EndSellDate = ticket.EndSellDate,
                Description = ticket.Description,
                OverviewContent = ticket.OverviewContent,
                ContentType = ticket.ContentType,
                IsShowApp = ticket.IsShowApp,
                UrlImages = ticket.TicketMedias.Select(t => new EventEntites.Dto.EvtTicketMedia.EvtTicketMediaDto
                {
                    Id = t.Id,
                    TicketId= t.TicketId,
                    UrlImage = t.UrlImage
                }),
                CreatedBy = ticket.CreatedBy,
                EventDetailId = ticket.EventDetailId
            };
        }

        public void UpdateTicketImage(UpdateTicketImageDto input)
        {
            _logger.LogInformation($"{nameof(DeleteTicketImage)} : input = {JsonSerializer.Serialize(input)}");
            var ticketMedia = _dbContext.EvtTicketMedias.FirstOrDefault(e => e.Id == input.Id).ThrowIfNull(_dbContext, ErrorCode.EvtTicketMediaNotFound);

            var image = _dbContext.EvtTicketMedias.Where(e => e.UrlImage == ticketMedia.UrlImage);
            if (!image.Any())
            {
                //Xóa ảnh
                _fileServices.DeleteFile(ticketMedia.UrlImage);
            }

            ticketMedia.UrlImage = input.UrlImage;
            _dbContext.SaveChanges();
        }
    }
}
