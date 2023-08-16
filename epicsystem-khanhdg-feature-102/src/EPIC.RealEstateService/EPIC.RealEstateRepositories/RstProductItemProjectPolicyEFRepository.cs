using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItem;
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
    public class RstProductItemProjectPolicyEFRepository : BaseEFRepository<RstProductItemProjectPolicy>
    {
        public RstProductItemProjectPolicyEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProductItemProjectPolicy.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chính sách
        /// </summary>
        /// <returns></returns>
        public RstProductItemProjectPolicy Add(RstProductItemProjectPolicy input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemProjectPolicyEFRepository)}-> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

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
        public RstProductItemProjectPolicy FindById(int id, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemProjectPolicyEFRepository)}-> {nameof(FindById)}: id = {id}, partnerId = {partnerId}");
            var result = _dbSet.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// FindById
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstProductItemProjectPolicy> FindAll(FilterProductItemProjectUtilityDto input, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProductItemProjectPolicyEFRepository)}-> {nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");
            var result = new PagingResult<RstProductItemProjectPolicy>();
            var query = _dbSet.Where(e => e.PartnerId == partnerId && e.Deleted == YesNo.NO);
            result.TotalItems = query.Count();

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query.ToList();

            return result;
        }
    }
}
