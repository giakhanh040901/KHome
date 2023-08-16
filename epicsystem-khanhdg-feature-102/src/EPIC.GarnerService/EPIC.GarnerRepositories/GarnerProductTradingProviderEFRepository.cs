using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
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
    public class GarnerProductTradingProviderEFRepository : BaseEFRepository<GarnerProductTradingProvider>
    {
        public GarnerProductTradingProviderEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerProductTradingProvider.SEQ}")
        {
        }

        #region CRUD
        /// <summary>
        /// Lấy danh sách đại lý tham gia theo sản phẩm
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<GarnerProductTradingProvider> FindAllList(int productId, int? partnerId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: productId = {productId}, partnerId = {partnerId}");
            
            return _dbSet.Where(p => p.ProductId == productId && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO).ToList();
        }

        /// <summary>
        /// Xem thông tin chi tiết phân phối sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public GarnerProductTradingProvider FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: productId = {id}, partnerId = {partnerId}");
            
            return _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Lấy thông tin phân phối sản phẩm của đại lý
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerProductTradingProvider FindByTradingProviderProduct(int productId, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: productId = {productId} tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(p => p.ProductId == productId && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thêm đại lý phân phối cho sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerProductTradingProvider Add(GarnerProductTradingProvider input, int partnerId, string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}, partner = {partnerId}, username = {username}");
            
            return _dbSet.Add(new GarnerProductTradingProvider
            {
                Id = (int)NextKey(),
                ProductId = input.ProductId,
                PartnerId = partnerId,
                TradingProviderId = input.TradingProviderId,
                HasTotalInvestmentSub = input.HasTotalInvestmentSub,
                TotalInvestmentSub = input.TotalInvestmentSub,
                IsProfitFromPartner = input.IsProfitFromPartner,
                Quantity = input.Quantity,
                DistributionDate = input.DistributionDate,
                CreatedBy = username
            }).Entity;
        }

        public void Update(GarnerProductTradingProvider input, int partnerId, string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input ={JsonSerializer.Serialize(input)}, partner = {partnerId}, username = {username}");

            var productTradingProviderFind = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && input.TradingProviderId == p.TradingProviderId && p.Deleted == YesNo.NO);
            if (productTradingProviderFind != null)
            {
                productTradingProviderFind.HasTotalInvestmentSub = input.HasTotalInvestmentSub;
                productTradingProviderFind.TotalInvestmentSub = input.TotalInvestmentSub;
                productTradingProviderFind.IsProfitFromPartner = input.IsProfitFromPartner;
                productTradingProviderFind.Quantity = input.Quantity;
                productTradingProviderFind.DistributionDate = input.DistributionDate;
                productTradingProviderFind.ModifiedBy = username;
                productTradingProviderFind.ModifiedDate = DateTime.Now;
                
                if (input.HasTotalInvestmentSub == YesNo.NO)
                {
                    productTradingProviderFind.TotalInvestmentSub = 0;
                    productTradingProviderFind.Quantity = 0;
                }    
            }    
        }

        public void Delete(int id, int partnerId, string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: id = {id}, partner = {partnerId}, username = {username}");

            var productTradingProviderFind = _dbSet.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (productTradingProviderFind != null)
            {
                productTradingProviderFind.Deleted = YesNo.YES;
                productTradingProviderFind.ModifiedBy = username;
                productTradingProviderFind.ModifiedDate = DateTime.Now;
            }
        }

        public List<BusinessCustomerBank> FindBankByTrading(int tradingProviderId, int? distributionId = null, int? type = null)
        {
            _logger.LogInformation($"{nameof(FindBankByTrading)}: tradingProviderId = {tradingProviderId}, distributionId = {distributionId}");
            List<BusinessCustomerBank> result = new ();
            if (distributionId != null)
            {
                result = (from d in _epicSchemaDbContext.GarnerDistributions
                join t in _epicSchemaDbContext.GarnerDistributionTradingBankAccounts
                on d.Id equals t.DistributionId
                join b in _epicSchemaDbContext.BusinessCustomerBanks on t.BusinessCustomerBankAccId equals b.BusinessCustomerBankAccId
                join c in _epicSchemaDbContext.CoreBanks.DefaultIfEmpty() on b.BankId equals c.BankId
                where (type == null || t.Type == type) && d.Id == distributionId && b.Deleted == YesNo.NO && d.Deleted == YesNo.NO && t.Deleted == YesNo.NO select new BusinessCustomerBank
                {
                    BusinessCustomerBankAccId = b.BusinessCustomerBankAccId,
                    BusinessCustomerId = b.BusinessCustomerId,
                    BankAccName = b.BankAccName,
                    BankAccNo = b.BankAccNo,
                    BankName = c.BankName,
                    BankBranchName = b.BankBranchName,
                    BankId = b.BankId,
                    Status = b.Status,
                    IsDefault = b.IsDefault,
                    CreatedDate = b.CreatedDate,
                    CreatedBy = b.CreatedBy,
                    ModifiedDate = b.ModifiedDate,
                    ModifiedBy = b.ModifiedBy,
                    Deleted = b.Deleted
                }
                ).ToList();
            }
            else if(distributionId == null)
            {
                result = (from b in _epicSchemaDbContext.BusinessCustomerBanks
                        join p in _epicSchemaDbContext.TradingProviders on tradingProviderId equals p.TradingProviderId
                        join c in _epicSchemaDbContext.CoreBanks.DefaultIfEmpty() on b.BankId equals c.BankId
                        where b.BusinessCustomerId == p.BusinessCustomerId && b.Deleted == YesNo.NO orderby b.IsDefault select new BusinessCustomerBank
                        {
                            BusinessCustomerBankAccId = b.BusinessCustomerBankAccId,
                            BusinessCustomerId = b.BusinessCustomerId,
                            BankAccName = b.BankAccName,
                            BankAccNo = b.BankAccNo,
                            BankName = c.BankName,
                            BankBranchName = b.BankBranchName,
                            BankId = b.BankId,
                            Status = b.Status,
                            IsDefault = b.IsDefault,
                            CreatedDate = b.CreatedDate,
                            CreatedBy = b.CreatedBy,
                            ModifiedDate = b.ModifiedDate,
                            ModifiedBy = b.ModifiedBy,
                            Deleted = b.Deleted
                        }).ToList();
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Thêm đại lý cho dự án
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /*public void UpdateProductTradingProvider(int productId, List<CreateGarnerProductTradingProviderDto> input, int partnerId, string username)
        {
            //Lấy danh sách đại lý tham gia đã có trong db
            var productTradingProviderListFind = FindAllList(productId, partnerId);

            //Xóa đi những đại lý không được truyền vào
            var productTradingProviderRemove = productTradingProviderListFind.Where(p => !input.Select(r => r.Id).Contains(p.Id)).ToList();
            foreach (var productTradingProviderItem in productTradingProviderRemove)
            {
                productTradingProviderItem.Deleted = YesNo.YES;
            }

            foreach (var item in input)
            {
                //Nếu là thêm mới thì thêm vào
                if (item.Id == TrueOrFalseNum.FALSE)
                {
                    _dbSet.Add(new GarnerProductTradingProvider
                    {
                        Id = (int)NextKey(),
                        ProductId = productId,
                        PartnerId = partnerId,
                        TradingProviderId = item.TradingProviderId,
                        TotalInvestmentSub = item.TotalInvestmentSub,
                        CreatedBy = username
                    });
                }    
                else
                {
                    var productTradingProviderFind = _dbSet.FirstOrDefault(p => p.Id == item.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
                    if (productTradingProviderFind != null)
                    {
                        productTradingProviderFind.TradingProviderId = item.TradingProviderId;
                        productTradingProviderFind.TotalInvestmentSub = item.TotalInvestmentSub;
                        productTradingProviderFind.ModifiedBy = username;
                        productTradingProviderFind.ModifiedDate = DateTime.Now;
                    }    
                }    
            }
        }*/
    }
}
