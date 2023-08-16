using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstSellingPolicyTempEFRepository : BaseEFRepository<RstSellingPolicyTemp>
    {
        public RstSellingPolicyTempEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstSellingPolicyTemp.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mới chính sách bán hàng của đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstSellingPolicyTemp Add(CreateRstSellingPolicyTempDto input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyTempEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            return _dbSet.Add(new RstSellingPolicyTemp()
            {
                Id = (int)NextKey(),
                TradingProviderId = tradingProviderId,
                Code = input.Code,
                Name = input.Name,
                SellingPolicyType = input.SellingPolicyType,
                Source = input.Source,
                ConversionValue = input.ConversionValue,
                FromValue = input.FromValue,
                ToValue = input.ToValue,
                Description = input.Description,
                FileName = input.FileName,
                FileUrl = input.FileUrl,
                CreatedDate = DateTime.Now,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật chính sách bán hàng của đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstSellingPolicyTemp Update(UpdateRstSellingPolicyTempDto input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyTempEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");
            var salesPolicy = _dbSet.FirstOrDefault(o => o.Id == input.Id && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO);
            if (salesPolicy != null)
            {
                salesPolicy.TradingProviderId = tradingProviderId;
                salesPolicy.Code = input.Code;
                salesPolicy.Name = input.Name;
                salesPolicy.SellingPolicyType = input.SellingPolicyType;
                salesPolicy.Source = input.Source;
                salesPolicy.ConversionValue = input.ConversionValue;
                salesPolicy.FromValue = input.FromValue;
                salesPolicy.ToValue = input.ToValue;
                salesPolicy.Description = input.Description;
                salesPolicy.FileName = input.FileName;
                salesPolicy.FileUrl = input.FileUrl;
                salesPolicy.ModifiedBy = username;
                salesPolicy.ModifiedDate = DateTime.Now;
            }
            return salesPolicy;
        }

        /// <summary>
        /// tìm kiếm chính sách bán hàng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstSellingPolicyTemp FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyTempEFRepository)}->{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(o => o.Id == id && (tradingProviderId == null || o.TradingProviderId == tradingProviderId) && o.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Xóa chính sách bán hàng
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyTempEFRepository)}->{nameof(Delete)}: id = {id}");

            var salesPolicy = _dbSet.FirstOrDefault(o => o.Id == id && o.Deleted == YesNo.NO);
            if (salesPolicy != null)
            {
                salesPolicy.Deleted = YesNo.YES;
            }
        }

        /// <summary>
        /// thay đổi trạng thái chính sách bán hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstSellingPolicyTemp ChangeStatus(int id, string status)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyTempEFRepository)}->{nameof(ChangeStatus)}: id = {id}, status = {status}");
            var salesPolicy = _dbSet.FirstOrDefault(o => o.Id == id && o.Deleted == YesNo.NO);
            if (salesPolicy != null)
            {
                salesPolicy.Status = status;
            }
            return salesPolicy;
        }

        /// <summary>
        /// tìm kiếm danh sách chính sách bán hàng có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<RstSellingPolicyTemp> FindAll(FilterRstSellingPolicyTempDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstSellingPolicyTempEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<RstSellingPolicyTemp> result = new();
            IQueryable<RstSellingPolicyTemp> salesPolicyQuery = _dbSet.Where(p => p.Deleted == YesNo.NO && (tradingProviderId == null || p.TradingProviderId == tradingProviderId)
                                                                             && (input.Keyword == null || (p.Name.Contains(input.Keyword) || p.Code.Contains(input.Keyword)))
                                                                             && (input.Status == null || p.Status == input.Status) && (input.Name == null || p.Name.Contains(input.Name))
                                                                             && (input.CreatedDate == null || (p.CreatedDate != null && input.CreatedDate.Value.Date == p.CreatedDate.Value.Date))
                                                                             && (input.Code == null || p.Name.Contains(input.Code)));

            result.TotalItems = salesPolicyQuery.Count();
            salesPolicyQuery = salesPolicyQuery.OrderByDescending(p => p.Id);

            if (input.PageSize != -1)
            {
                salesPolicyQuery = salesPolicyQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = salesPolicyQuery;
            return result;
        }
    }
}
