using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
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
    public class RstProductItemMediaEFRepository : BaseEFRepository<RstProductItemMedia>
    {
        public RstProductItemMediaEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProductItemMedia.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItemMedia Add(RstProductItemMedia input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");
            return _dbSet.Add(new RstProductItemMedia()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                ProductItemId = input.ProductItemId,
                GroupTitle = input.GroupTitle,
                MediaType = input.MediaType,
                UrlImage = input.UrlImage,
                UrlPath = input.UrlPath,
                Location = input.Location,
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
        public RstProductItemMedia Update(RstProductItemMedia input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (media != null)
            {
                media.GroupTitle = input.GroupTitle;
                media.UrlImage = input.UrlImage;
                media.Location = input.Location;
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
        public RstProductItemMedia FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaEFRepository)}->{nameof(FindById)}: id = {id}, partnerId = {partnerId}");

            return _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItemMedia ChangeStatus(int id, string status, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaEFRepository)}->{nameof(ChangeStatus)}: id = {id}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);

            if (media != null)
            {
                media.Status = status;
            }

            return media;
        }

        /// <summary>
        /// Tìm hình ảnh sản phẩm orderBy location
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProductItemMedia> Find(int productItemId, int? partnerId = null, string location = null, string status = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaEFRepository)}->{nameof(Find)}: productItemId = {productItemId}, partnerId = {partnerId}," +
                $" location = {location}, status = {status}");

            var mediaList = _dbSet.Where(o => o.ProductItemId == productItemId && o.Deleted == YesNo.NO 
            && (partnerId == null || o.PartnerId == partnerId)
            && (location == null || o.Location == location)
            && (status == null || o.Status == status)).OrderByDescending(o => o.Location).ThenBy(o => o.SortOrder).ToList();

            return mediaList;
        }

        /// <summary>
        /// Xoá hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
            public void Delete(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProductItemMediaEFRepository)}->{nameof(Find)}: id = {id}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(o => o.Id == id && (partnerId == null || o.PartnerId == partnerId) && o.Deleted == YesNo.NO);
            if (media != null)
            {
                _dbContext.Remove(media);
            }
        }
    }
}
