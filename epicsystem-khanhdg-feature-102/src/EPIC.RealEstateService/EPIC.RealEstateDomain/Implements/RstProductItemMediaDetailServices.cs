using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProductItemMediaDetailServices : IRstProductItemMediaDetailServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProductItemMediaDetailServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProductItemMediaDetailEFRepository _rstProductItemMediaDetailEFRepository;
        private readonly RstProductItemMediaEFRepository _rstProductItemMediaEFRepository;

        public RstProductItemMediaDetailServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProductItemMediaDetailServices> logger,
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
            _rstProductItemMediaDetailEFRepository = new RstProductItemMediaDetailEFRepository(dbContext, logger);
            _rstProductItemMediaEFRepository = new RstProductItemMediaEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm nhóm hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProductItemMediaDetail> Add(CreateRstProductItemMediaDetailsDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            //var productItemMedia = _rstProductItemMediaEFRepository.FindById(input.ProductItemMediaId).ThrowIfNull<RstProductItemMedia>(_dbContext, ErrorCode.RstProductItemMediaDetailNotFound);

            var mediaList = new List<RstProductItemMediaDetail>();
            var transaction = _dbContext.Database.BeginTransaction();
            var productItemMedia = _rstProductItemMediaEFRepository.Add(new RstProductItemMedia() { 
                ProductItemId = input.ProductItemId,
                GroupTitle = input.GroupTitle,
            }, username, partnerId);
            _dbContext.SaveChanges();

            foreach (var item in input.RstProductItemMediaDetails)
            {
                var insert = _rstProductItemMediaDetailEFRepository.Add(_mapper.Map<RstProductItemMediaDetail>(item), username, partnerId);
                insert.ProductItemMediaId = productItemMedia.Id;
                var media = _fileExtensionServices.GetMediaExtensionFile(insert.UrlImage);
                insert.MediaType = media;
                insert.SortOrder = insert.Id;
                mediaList.Add(insert);
            }

            productItemMedia.GroupTitle = input.GroupTitle;

            _dbContext.SaveChanges();
            transaction.Commit();
            return mediaList;
        }

        /// <summary>
        /// Cập nhật nhóm hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProductItemMediaDetail Update(UpdateRstProductItemMediaDetailDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var mediaDetailFind = _rstProductItemMediaDetailEFRepository
                .Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstProductItemMediaDetail>(_dbContext, ErrorCode.RstProductItemMediaDetailNotFound);
            var mediaFind = _rstProductItemMediaEFRepository.FindById(mediaDetailFind.ProductItemMediaId, partnerId);
            var updateMedia = _rstProductItemMediaDetailEFRepository.Update(_mapper.Map<RstProductItemMediaDetail>(input), partnerId, username);
            var media = _fileExtensionServices.GetMediaExtensionFile(updateMedia.UrlImage);
            updateMedia.MediaType = media;

            _dbContext.SaveChanges();
            return updateMedia;
        }

        /// <summary>
        /// Xoá nhóm hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, partnerId = {partnerId}");

            var media = _rstProductItemMediaDetailEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstProductItemMediaDetail>(_dbContext, ErrorCode.RstProductItemMediaDetailNotFound);

            _rstProductItemMediaDetailEFRepository.Delete(id, partnerId);
            _fileServices.DeleteFile(media.UrlImage);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm nhóm hình ảnh sản phẩm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProductItemMediaDetail FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, partnerId = {partnerId}");
            var media = _rstProductItemMediaDetailEFRepository.FindById(id, partnerId).ThrowIfNull<RstProductItemMediaDetail>(_dbContext, ErrorCode.RstProductItemMediaDetailNotFound);

            return media;
        }

        /// <summary>
        /// Đổi trạng thái nhóm hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstProductItemMediaDetail ChangeStatus(int id, string status)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, partnerId = {partnerId}");

            var media = _rstProductItemMediaDetailEFRepository.FindById(id).ThrowIfNull<RstProductItemMediaDetail>(_dbContext, ErrorCode.RstProductItemMediaDetailNotFound);

            var changeStatus = _rstProductItemMediaDetailEFRepository.ChangeStatus(id, status, partnerId);
            _dbContext.SaveChanges();
            return changeStatus;
        }

        /// <summary>
        /// Danh sách hình ảnh sản phẩm order by theo tên nhóm
        /// </summary>
        /// <returns></returns>
        public List<RstProductItemMediaDto> Find(int productItemId, string status)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Find)}, partnerId = {partnerId}, productItemId = {productItemId}, " +
                $"status = {status}");

            var rstProductItemMedia = new List<RstProductItemMediaDto>();
            var listRstProductItemMedia = _rstProductItemMediaEFRepository.Find(productItemId, partnerId);
            var rstProductItemMediaFind = _mapper.Map<List<RstProductItemMediaDto>>(listRstProductItemMedia);

            var listMediaDetail = _rstProductItemMediaDetailEFRepository.Find(productItemId, partnerId, status);
            foreach (var itemMedia in rstProductItemMediaFind)
            {
                if (itemMedia.GroupTitle != null)
                {
                    var listRstProductItemMediaDetail = new List<RstProductItemMediaDetailDto>();
                    foreach (var item in listMediaDetail)
                    {
                        if (item.ProductItemMediaId == itemMedia.Id && item.GroupTitle == itemMedia.GroupTitle)
                        {
                            listRstProductItemMediaDetail.Add(item);
                        }
                    }
                    itemMedia.ProductItemMediaDetails = listRstProductItemMediaDetail.OrderBy(o => o.SortOrder).ToList();
                    rstProductItemMedia.Add(itemMedia);
                }
            }
            return rstProductItemMedia;
        }

        /// <summary>
        /// Danh sách nhóm hình ảnh sản phẩm order by theo tên nhóm
        /// </summary>
        /// <returns></returns>
        public List<RstProductItemMediaDetail> AddListMediaDetail(AddRstProductItemMediaDetailsDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(AddListMediaDetail)}:input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");


            var mediaDetailList = new List<RstProductItemMediaDetail>();
            var transaction = _dbContext.Database.BeginTransaction();

            foreach (var item in input.RstProductItemMediaDetails)
            {
                var insert = _rstProductItemMediaDetailEFRepository.Add(_mapper.Map<RstProductItemMediaDetail>(item), username, partnerId);
                insert.ProductItemMediaId = input.ProductItemMediaId;
                var media = _fileExtensionServices.GetMediaExtensionFile(insert.UrlImage);
                insert.MediaType = media;
                mediaDetailList.Add(insert);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return mediaDetailList;
        }

        /// <summary>
        /// update thứ tự hình ảnh dự án
        /// </summary>
        /// <returns></returns>
        public void UpdateSortOrder(RstProductItemMediaDetailSortDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateSortOrder)}: input = {input}, partnerId = {partnerId}");

            var result = _rstProductItemMediaDetailEFRepository.Find(input.ProductItemId, partnerId);

            foreach (var item in result)
            {
                var sort = input.Sort.FirstOrDefault(o => o.ProductItemMediaDetailId == item.Id);
                var productItemMediaDetailFind = _rstProductItemMediaDetailEFRepository.FindById(item.Id);
                if (sort != null)
                {
                    productItemMediaDetailFind.SortOrder = sort.SortOrder;
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
