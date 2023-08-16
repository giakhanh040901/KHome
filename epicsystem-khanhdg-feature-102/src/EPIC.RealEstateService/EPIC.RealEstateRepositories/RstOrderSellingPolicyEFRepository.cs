using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.EntitiesBase.Interfaces.Policy;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy;
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
    public class RstOrderSellingPolicyEFRepository : BaseEFRepository<RstOrderSellingPolicy>
    {
        public RstOrderSellingPolicyEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOrderSellingPolicy.SEQ}")
        {
        }

        public RstOrderSellingPolicy Add(RstOrderSellingPolicy input, string username)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username} ");

            var result = _dbSet.Add(new RstOrderSellingPolicy
            {
                Id = (int)NextKey(),
                CreatedBy = username,
                OrderId = input.OrderId,
                ProductItemProjectPolicyId = input.ProductItemProjectPolicyId,
                SellingPolicyId = input.SellingPolicyId,
                CreatedDate = DateTime.Now,
            });
            return result.Entity;
        }

        public RstOrderSellingPolicy FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstOrderEFRepository)}->{nameof(FindById)}: id = {id}");

            var result = _dbSet.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO);
            return result;
        }

        public List<RstOrderSellingPolicyDto> FindPolicyByOrder (FilterRstOrderSellingPolicyDto input)
        {
            List<RstOrderSellingPolicyDto> resultItem = new();
            
            //Chính sách mở bán
            var sellingPolicyQuery = from order in _epicSchemaDbContext.RstOrders
                                     join productItem in _epicSchemaDbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                     join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on order.OpenSellDetailId equals openSellDetail.Id
                                     join sellingPolicy in _epicSchemaDbContext.RstSellingPolicies on openSellDetail.OpenSellId equals sellingPolicy.OpenSellId
                                     join sellingPolicyTemp in _epicSchemaDbContext.RstSellingPolicyTemps on sellingPolicy.SellingPolicyTempId equals sellingPolicyTemp.Id
                                     join orderSellingPolicy in _epicSchemaDbContext.RstOrderSellingPolicies on sellingPolicy.Id equals orderSellingPolicy.SellingPolicyId into orderSellingPolicies
                                     from orderSellingPolicy in orderSellingPolicies.DefaultIfEmpty()
                                     where order.Id == input.OrderId && sellingPolicy.Deleted == YesNo.NO && sellingPolicyTemp.Deleted == YesNo.NO 
                                     && (order.Source == SourceOrder.ALL || (sellingPolicyTemp.Source == SourceOrder.ALL || sellingPolicyTemp.Source == order.Source)) 
                                     && sellingPolicy.Status == Status.ACTIVE && sellingPolicyTemp.Status == Status.ACTIVE
                                     && (input.IsSelected == null || orderSellingPolicy != null)
                                     && ((sellingPolicyTemp.SellingPolicyType == RstSalesPolicyType.GiaTriCoDinh 
                                        && (productItem.Price <= sellingPolicyTemp.ToValue && productItem.Price >= sellingPolicyTemp.FromValue))
                                        || sellingPolicyTemp.SellingPolicyType == RstSalesPolicyType.GiaTriCanHo)
                                     select new
                                     {
                                         sellingPolicy,
                                         sellingPolicyTemp,
                                         orderSellingPolicy
                                     };
            //Chính sách ưu đãi CĐT
            var productPolicyQuery = from order in _epicSchemaDbContext.RstOrders
                                     join productItem in _epicSchemaDbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                     join productPolicy in _epicSchemaDbContext.RstProductItemProjectPolicys on productItem.Id equals productPolicy.ProductItemId
                                     join projectPolicy in _epicSchemaDbContext.RstProjectPolicys on productPolicy.ProjectPolicyId equals projectPolicy.Id
                                     join orderSellingPolicy in _epicSchemaDbContext.RstOrderSellingPolicies on productPolicy.Id equals orderSellingPolicy.ProductItemProjectPolicyId into orderSellingPolicies
                                     from orderSellingPolicy in orderSellingPolicies.DefaultIfEmpty()
                                     where order.Id == input.OrderId && productItem.Deleted == YesNo.NO && productPolicy.Deleted == YesNo.NO
                                     && (order.Source == SourceOrder.ALL || (projectPolicy.Source == SourceOrder.ALL || projectPolicy.Source == order.Source))
                                     && productPolicy.Status == Status.ACTIVE && projectPolicy.Status == Status.ACTIVE
                                     && (input.IsSelected == null || orderSellingPolicy != null)
                                     select new
                                     {
                                         productPolicy,
                                         projectPolicy,
                                         orderSellingPolicy
                                     };

            foreach (var item in sellingPolicyQuery)
            {
                var sellingPolicyInsert = new RstOrderSellingPolicyDto()
                {
                    Id = item.orderSellingPolicy?.Id,
                    SellingPolicyId = item.sellingPolicy.Id,
                    Code = item.sellingPolicyTemp.Code,
                    Name = item.sellingPolicyTemp.Name,
                    ConversionValue = item.sellingPolicyTemp.ConversionValue,
                    PolicyType = AppRstPolicyTypes.FROM_TRADING_PROVIDER,
                    IsSelected = item.orderSellingPolicy == null ? YesNo.NO : YesNo.YES,
                    Source = item.sellingPolicyTemp.Source
                };
                resultItem.Add(sellingPolicyInsert);
            }

            foreach (var item in productPolicyQuery)
            {
                var sellingPolicyInsert = new RstOrderSellingPolicyDto()
                {
                    Id = item.orderSellingPolicy?.Id,
                    ProjectPolicyId = item.projectPolicy.Id,
                    Code = item.projectPolicy.Code,
                    Name = item.projectPolicy.Name,
                    ConversionValue = item.projectPolicy.ConversionValue,
                    PolicyType = AppRstPolicyTypes.FROM_PARTNER,
                    IsSelected = item.orderSellingPolicy == null ? YesNo.NO : YesNo.YES,
                    Source = item.projectPolicy.Source
                };
                resultItem.Add(sellingPolicyInsert);
            }
            return resultItem;
        }
    }
}
