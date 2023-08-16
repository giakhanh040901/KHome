using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProductItemMediaDetailEFRepository : BaseEFRepository<RstProductItemMediaDetail>
    {
        public RstProductItemMediaDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProductItemMediaDetail.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItemMediaDetail Add(RstProductItemMediaDetail input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaDetailEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");
            return _dbSet.Add(new RstProductItemMediaDetail()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                UrlImage = input.UrlImage,
                MediaType = input.MediaType,
                ProductItemMediaId = input.ProductItemMediaId,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật hình ảnh sản phẩm 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstProductItemMediaDetail Update(RstProductItemMediaDetail input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaDetailEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (media != null)
            {
                media.UrlImage = input.UrlImage;
                media.ModifiedBy = username;
                media.ModifiedDate = DateTime.Now;
            }

            return media;
        }

        /// <summary>
        /// Tìm hình ảnh sản phẩm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItemMediaDetail FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaDetailEFRepository)}->{nameof(FindById)}: id = {id}, partnerId = {partnerId}");

            return _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Đổi trạng thái hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItemMediaDetail ChangeStatus(int id, string status, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaDetailEFRepository)}->{nameof(ChangeStatus)}: id = {id}, partnerId = {partnerId}, status = {status}");

            var media = _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);

            if (media != null)
            {
                media.Status = status;
            }

            return media;
        }

        /// <summary>
        /// Tìm hình ảnh sản phẩm orderBy GroupTitle
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProductItemMediaDetailDto> Find(int productItemId, int? partnerId = null, string status = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaDetailEFRepository)}->{nameof(Find)}: productItemId = {productItemId}, " +
                $"partnerId = {partnerId}, status = {status}");

            var mediaList = from mediaDetail in _dbSet
                            join media in _epicSchemaDbContext.RstProductItemMedias on mediaDetail.ProductItemMediaId equals media.Id
                            where (mediaDetail.PartnerId == partnerId || partnerId == null) && mediaDetail.Deleted == YesNo.NO
                            && media.ProductItemId == productItemId && (status == null || mediaDetail.Status == status)
                            orderby media.GroupTitle descending
                            select new RstProductItemMediaDetailDto()
                            {
                                Id = mediaDetail.Id,
                                PartnerId = mediaDetail.PartnerId,
                                GroupTitle = media.GroupTitle,
                                ProductItemMediaId = mediaDetail.ProductItemMediaId,
                                MediaType = mediaDetail.MediaType,
                                UrlImage = mediaDetail.UrlImage,
                                Status = mediaDetail.Status,
                                SortOrder = mediaDetail.SortOrder
                            };

            return mediaList.ToList();
        }

        /// <summary>
        /// Xoá hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        public void Delete(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaDetailEFRepository)}->{nameof(Delete)}: id = {id}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(o => o.Id == id && (partnerId == null || o.PartnerId == partnerId) && o.Deleted == YesNo.NO);
            if (media != null)
            {
                _dbContext.Remove(media);
            }
        }

        /// <summary>
        /// Tìm nhóm hình ảnh theo ProductItemMediaId
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProductItemMediaDetailDto> FindByProjectMediaId(int productItemMediaId, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaDetailEFRepository)}->{nameof(FindByProjectMediaId)}: productItemMediaId = {productItemMediaId}, partnerId = {partnerId}");

            var mediaList = from mediaDetail in _dbSet
                            join media in _epicSchemaDbContext.RstProductItemMedias on mediaDetail.ProductItemMediaId equals media.Id
                            where (mediaDetail.PartnerId == partnerId || partnerId == null) && mediaDetail.Deleted == YesNo.NO
                            && media.Id == productItemMediaId
                            orderby media.GroupTitle descending
                            select new RstProductItemMediaDetailDto()
                            {
                                Id = mediaDetail.Id,
                                PartnerId = mediaDetail.PartnerId,
                                GroupTitle = media.GroupTitle,
                                ProductItemMediaId = mediaDetail.ProductItemMediaId,
                                MediaType = mediaDetail.MediaType,
                                UrlImage = mediaDetail.UrlImage,
                                Status = mediaDetail.Status
                            };

            return mediaList.ToList();
        }
    }
}
