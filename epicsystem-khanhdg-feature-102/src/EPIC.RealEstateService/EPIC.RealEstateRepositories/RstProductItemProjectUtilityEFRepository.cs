using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProductItemUtilityEFRepository : BaseEFRepository<RstProductItemUtility>
    {
        public RstProductItemUtilityEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProductItemUtility.SEQ}")
        {
        }

        /// <summary>
        /// Thêm tiện ích
        /// </summary>
        /// <returns></returns>
        public RstProductItemUtility Add(RstProductItemUtility input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemUtilityEFRepository)}-> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            input.Id = (int)NextKey();
            input.PartnerId = partnerId;
            input.CreatedDate = DateTime.Now;
            input.CreatedBy = username;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// FindById
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItemUtility FindById(int id, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemUtilityEFRepository)}-> {nameof(Add)}: id = {id}, partnerId = {partnerId}");
            var result = _dbSet.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// FindById
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstProductItemUtility> FindAll(FilterProductItemProjectUtilityDto input, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemUtilityEFRepository)}-> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<RstProductItemUtility>();
            var query = _dbSet.Where(e => e.PartnerId == partnerId && e.Deleted == YesNo.NO);
            result.TotalItems = query.Count();

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query.ToList();

            return result;
        }

        /// <summary>
        /// Tìm product item theo id tiện ích
        /// </summary>
        /// <param name="productItemUtilityId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProductItemUtility FindByProductItemUtilityId(int? productItemUtilityId, int partnerId, int productItemId)
        {

            _logger.LogInformation($"{nameof(RstProductItemEFRepository)}->{nameof(FindByProductItemUtilityId)}:  productItemUtilityId = {productItemUtilityId}, parnerId = {partnerId}");
            return _dbSet.FirstOrDefault(e => e.ProductItemUtilityId == productItemUtilityId && e.ProductItemId == productItemId && e.PartnerId == partnerId && e.Deleted == YesNo.NO);
        }
    }
}
