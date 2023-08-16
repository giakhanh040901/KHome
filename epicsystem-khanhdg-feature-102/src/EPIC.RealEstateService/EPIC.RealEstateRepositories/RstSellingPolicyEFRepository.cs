using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.RealEstate;
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
    public class RstSellingPolicyEFRepository : BaseEFRepository<RstSellingPolicy>
    {
        public RstSellingPolicyEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstSellingPolicy.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chính sách mở bán
        /// </summary>
        /// <returns></returns>
        public RstSellingPolicy Add(RstSellingPolicy input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyEFRepository)}-> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            input.Id = (int)NextKey();
            input.TradingProviderId = tradingProviderId;
            input.CreatedDate = DateTime.Now;
            input.CreatedBy = username;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// FindById
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstSellingPolicy FindById(int id, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyEFRepository)}-> {nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");
            var result = _dbSet.FirstOrDefault(e => e.Id == id && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// FindAll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<RstSellingPolicy> FindAll(FilterRstSellingPolicyDto input, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyEFRepository)}-> {nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var result = new PagingResult<RstSellingPolicy>();
            var query = _dbSet.Where(e => e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);
            result.TotalItems = query.Count();

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query.ToList();
            return result;
        }

        /// <summary>
        /// Lấy chính sách mở bán đang active
        /// </summary>
        /// <param name="openSellId"></param>
        /// <returns></returns>
        public List<RstSellingPolicy> FindAllByOpenSellId(int openSellId)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyEFRepository)}-> {nameof(FindAll)}: OpenSellId = {openSellId}");
            var query = _dbSet.Where(e => e.OpenSellId == openSellId && e.Deleted == YesNo.NO && e.Status == Status.ACTIVE);
            return query.ToList();
        }

        /// <summary>
        /// Chính sách ưu đãi của căn hộ mở bán
        /// Ghép chung toàn bộ chính sách của căn hộ bao gồm: chính sách từ CĐT và chính sách mở bán của đại lý với loại hình bán online
        /// </summary>
        public List<AppRstSellingPolicyDto> AppRstPolicyForProductItem(int openSellId, int productItemId)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyEFRepository)}-> {nameof(AppRstPolicyForProductItem)}: openSellId = {openSellId}, productItemId = {productItemId}");
            List<AppRstSellingPolicyDto> result = new();

            // Lấy chính sách ưu đãi từ Mở bán
            var policyFromOpenSell = from sellingPolicy in _dbSet
                                     join sellingPolicyTemp in _epicSchemaDbContext.RstSellingPolicyTemps on sellingPolicy.SellingPolicyTempId equals sellingPolicyTemp.Id
                                     where sellingPolicy.Deleted == YesNo.NO && sellingPolicyTemp.Deleted == YesNo.NO
                                     && sellingPolicy.Status == Status.ACTIVE && sellingPolicyTemp.Status == Status.ACTIVE
                                     && sellingPolicy.OpenSellId == openSellId && (sellingPolicyTemp.Source == ContractSources.ONLINE || sellingPolicyTemp.Source == ContractSources.ALL)
                                     select new AppRstSellingPolicyDto()
                                     {
                                         Id = sellingPolicy.Id,
                                         Code = sellingPolicyTemp.Code,
                                         Name = sellingPolicyTemp.Name,
                                         PolicyType = AppRstPolicyTypes.FROM_TRADING_PROVIDER,
                                         ConversionValue = sellingPolicyTemp.ConversionValue,
                                         Description = sellingPolicyTemp.Description,
                                     };
            // Lấy chính sách từ sản phẩm căn hộ do chủ đầu tư cài
            var policyFromProductItem = from productItemPolicy in _epicSchemaDbContext.RstProductItemProjectPolicys
                                        join projectPolicy in _epicSchemaDbContext.RstProjectPolicys on productItemPolicy.ProjectPolicyId equals projectPolicy.Id
                                        where projectPolicy.Deleted == YesNo.NO && productItemPolicy.Deleted == YesNo.NO
                                        && productItemPolicy.Status == Status.ACTIVE && projectPolicy.Status == Status.ACTIVE
                                        && (projectPolicy.Source == ContractSources.ONLINE || projectPolicy.Source == ContractSources.ALL)
                                        && productItemPolicy.ProductItemId == productItemId
                                        select new AppRstSellingPolicyDto()
                                        {
                                            Id = productItemPolicy.Id,
                                            Code = projectPolicy.Code,
                                            Name = projectPolicy.Name,
                                            PolicyType = AppRstPolicyTypes.FROM_PARTNER,
                                            ConversionValue = projectPolicy.ConversionValue,
                                            Description = projectPolicy.Description,
                                        };
            result.AddRange(policyFromOpenSell);
            result.AddRange(policyFromProductItem);
            return result;
        }
    }
}
