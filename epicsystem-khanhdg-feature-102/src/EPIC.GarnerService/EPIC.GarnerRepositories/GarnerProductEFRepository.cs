using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.UsersPartner;
//using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerProductEFRepository : BaseEFRepository<GarnerProduct>
    {
        public GarnerProductEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerProduct.SEQ}")
        {
        }

        /// <summary>
        /// Thêm sản phẩm tích  lũy theo loại hình tích lũy
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerProduct Add(GarnerProduct input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            if(_dbSet.Any(p => p.Code == input.Code && p.Deleted == YesNo.NO))
            {
                ThrowException(ErrorCode.GarnerProductDuplicateCode);
            }

            var insertProduct = new GarnerProduct()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                Icon = input.Icon,
                ProductType = input.ProductType,
                Name = input.Name,
                Code = input.Code,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                MaxInvestor = input.MaxInvestor,
                MinInvestDay = input.MinInvestDay,
                CountType = input.CountType,
                GuaranteeOrganization = input.GuaranteeOrganization,
                IsPaymentGurantee = input.IsPaymentGurantee,
                IsCollateral = input.IsCollateral,
                Status = DistributionStatus.KHOI_TAO,
            };

            if (input.ProductType == GarnerProductTypes.CO_PHAN)
            {
                insertProduct.CpsIssuerId = input.CpsIssuerId;
                insertProduct.CpsDepositProviderId = input.CpsDepositProviderId;
                insertProduct.CpsPeriod = input.CpsPeriod;
                insertProduct.CpsPeriodUnit = input.CpsPeriodUnit;
                insertProduct.CpsInterestRate = input.CpsInterestRate;
                insertProduct.CpsInterestRateType = input.CpsInterestRateType;
                insertProduct.CpsInterestPeriod = input.CpsInterestPeriod;
                insertProduct.CpsInterestPeriodUnit = input.CpsInterestPeriodUnit;
                insertProduct.CpsNumberClosePer = input.CpsNumberClosePer;
                insertProduct.CpsParValue = input.CpsParValue;
                insertProduct.CpsQuantity = input.CpsQuantity;
                insertProduct.CpsIsListing = input.CpsIsListing;
                insertProduct.CpsIsAllowSBD = input.CpsIsAllowSBD;
            }

            if (input.ProductType == GarnerProductTypes.BAT_DONG_SAN)
            {
                insertProduct.InvOwnerId = input.InvOwnerId;
                insertProduct.InvGeneralContractorId = input.InvGeneralContractorId;
                insertProduct.InvTotalInvestmentDisplay = input.InvTotalInvestmentDisplay;
                insertProduct.InvTotalInvestment = input.InvTotalInvestment;
                insertProduct.InvArea = input.InvArea;
                insertProduct.InvLocationDescription = input.InvLocationDescription;
                insertProduct.InvLatitude = input.InvLatitude;
                insertProduct.InvLongitude = input.InvLongitude;
            }
            return _dbSet.Add(insertProduct).Entity;
        }

        /// <summary>
        /// Cập nhật sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerProduct Update(GarnerProduct input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}, username = {username}");

            var productFind = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (productFind != null)
            {
                productFind.Icon = input.Icon;
                productFind.ProductType = input.ProductType;
                productFind.Name = input.Name;
                productFind.Code = input.Code;
                productFind.StartDate = input.StartDate;
                productFind.EndDate = input.EndDate;
                productFind.MaxInvestor = input.MaxInvestor;
                productFind.MinInvestDay = input.MinInvestDay;
                productFind.CountType = input.CountType;
                productFind.GuaranteeOrganization = input.GuaranteeOrganization;
                productFind.IsPaymentGurantee = input.IsPaymentGurantee;
                productFind.IsCollateral = input.IsCollateral;
                productFind.ModifiedBy = username;
                productFind.ModifiedDate = DateTime.Now;

                if (input.ProductType == GarnerProductTypes.CO_PHAN)
                {
                    productFind.CpsIssuerId = input.CpsIssuerId;
                    productFind.CpsDepositProviderId = input.CpsDepositProviderId;
                    productFind.CpsPeriod = input.CpsPeriod;
                    productFind.CpsPeriodUnit = input.CpsPeriodUnit;
                    productFind.CpsInterestRate = input.CpsInterestRate;
                    productFind.CpsInterestRateType = input.CpsInterestRateType;
                    productFind.CpsInterestPeriod = input.CpsInterestPeriod;
                    productFind.CpsInterestPeriodUnit = input.CpsInterestPeriodUnit;
                    productFind.CpsNumberClosePer = input.CpsNumberClosePer;
                    productFind.CpsParValue = input.CpsParValue;
                    productFind.CpsQuantity = input.CpsQuantity;
                    productFind.CpsIsListing = input.CpsIsListing;
                    productFind.CpsIsAllowSBD = input.CpsIsAllowSBD;
                }

                if (input.ProductType == GarnerProductTypes.BAT_DONG_SAN)
                {
                    productFind.InvOwnerId = input.InvOwnerId;
                    productFind.InvGeneralContractorId = input.InvGeneralContractorId;
                    productFind.InvTotalInvestmentDisplay = input.InvTotalInvestmentDisplay;
                    productFind.InvTotalInvestment = input.InvTotalInvestment;
                    productFind.InvArea = input.InvArea;
                    productFind.InvLocationDescription = input.InvLocationDescription;
                    productFind.InvLatitude = input.InvLatitude;
                    productFind.InvLongitude = input.InvLongitude;
                }
            }
            return productFind;
        }

        public GarnerProduct FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}, partnerId = {partnerId}");
            return _dbSet.FirstOrDefault(x => x.Id == id && (partnerId == null || x.PartnerId == partnerId));
        }

        public PagingResult<GarnerProduct> FindAll(FilterGarnerProductDto input, int? partnerId = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");

            PagingResult<GarnerProduct> result = new();
            var garnerProductQuery = _dbSet.OrderByDescending(p => p.Id)
                .Where(r => r.Deleted == YesNo.NO
                        && (input.Code == null || input.Code == r.Code)
                        && (input.Name == null || input.Name == r.Name)
                        && (input.Status == null || input.Status == r.Status)
                        && (input.ProductType == null || input.ProductType == r.ProductType));
            if (partnerId != null)
            {
                garnerProductQuery = garnerProductQuery.Where(r => r.PartnerId == partnerId);
            }

            if (tradingProviderId != null)
            {
                garnerProductQuery = from gp in garnerProductQuery
                                     join ptd in _epicSchemaDbContext.GarnerProductTradingProviders
                                     on gp.Id equals ptd.ProductId
                                     where ptd.Deleted == YesNo.NO && ptd.TradingProviderId == tradingProviderId
                                     select gp;
            }

            if (input.PageSize != -1)
            {
                garnerProductQuery = garnerProductQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.TotalItems = garnerProductQuery.Count();
            result.Items = garnerProductQuery.ToList();
            return result;
        }

        /// <summary>
        /// thay đổi trạng thái status đóng và hoạt động 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public GarnerProduct ChangeStatus(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: Id = {id}, partnerId = {partnerId}");
            var product = _dbSet.FirstOrDefault(x => x.Id == id && (partnerId == null || x.PartnerId == partnerId));
            if (product.Status == GarnerProductStatus.DONG)
            {
                product.Status = GarnerProductStatus.HOAT_DONG;
            }
            else if (product.Status == GarnerProductStatus.HOAT_DONG)
            {
                product.Status = GarnerProductStatus.DONG;
            }
            return product;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy cho đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerProduct> GetListProductByTradingProvider(int tradingProviderId)
        {
            return (from product in _dbSet
                    join productTrading in _epicSchemaDbContext.GarnerProductTradingProviders on product.Id equals productTrading.ProductId
                    where productTrading.TradingProviderId == tradingProviderId && product.Deleted == YesNo.NO && productTrading.Deleted == YesNo.NO
                    && (product.StartDate != null && product.StartDate <= DateTime.Now)
                    && (product.EndDate != null && product.EndDate >= DateTime.Now)
                    select product).Distinct().ToList();
        }

    }
}
