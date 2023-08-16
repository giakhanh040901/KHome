using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstConfigContractCode;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstConfigContractCodeEFRepository : BaseEFRepository<RstConfigContractCode>
    {
        public RstConfigContractCodeEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstConfigContractCode.SEQ}")
        {
        }

        /// <summary>
        /// Thêm cấu trúc mã hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstConfigContractCode Add(RstConfigContractCode input, string username, int? partnerId = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeEFRepository)}->{nameof(Add)}: input = {input}");
            return _dbSet.Add(new RstConfigContractCode()
            {
                Id = (int)NextKey(),
                TradingProviderId = tradingProviderId,
                PartnerId = partnerId,
                Name = input.Name,
                Description = input.Description,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Danh sách cấu trúc mã hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstConfigContractCode> GetAll(FilterRstConfigContractCodeDto input, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeEFRepository)}->{nameof(GetAll)}: input = {input}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            PagingResult<RstConfigContractCode> result = new();
            IQueryable<RstConfigContractCode> query = _dbSet.Where(e => (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && (partnerId == null || e.PartnerId == partnerId) && e.Deleted == YesNo.NO
                                                        && (input.CreatedDate == null || (e.CreatedDate != null && input.CreatedDate.Value.Date == e.CreatedDate.Value.Date))
                                                        && (input.Status == null || input.Status == e.Status) && (input.Keyword == null || e.Name.Contains(input.Keyword)));

            result.TotalItems = query.Count();

            query = query.OrderByDescending(o => o.Id);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }

        /// <summary>
        /// lấy cấu trúc mã cho mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstConfigContractCode> GetAllConfigContractCode(FilterRstConfigContractCodeDto input, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeEFRepository)}->{nameof(GetAllConfigContractCode)}: input = {input}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            PagingResult<RstConfigContractCode> result = new();
            IQueryable<RstConfigContractCode> query = _dbSet.Where(e => e.Deleted == YesNo.NO);

            // lọc theo trạng thái
            query = query.Where(e => input.Status == null || input.Status == e.Status);

            // lọc theo keyword
            query = query.Where(e => input.Keyword == null || e.Name.Contains(input.Keyword));

            if (input.Type == RstOpenSellContractTypes.TRADING_PROVIDER)
            {
                query = query.Where(e => e.TradingProviderId == tradingProviderId);
            }

            else if (input.Type == RstOpenSellContractTypes.PARTNER)
            {
                query = query.Where(e => _epicSchemaDbContext.TradingProviderPartners.Where(o => o.TradingProviderId == tradingProviderId).Select(o => o.PartnerId).Contains(e.PartnerId.Value));
            }

            result.TotalItems = query.Count();

            query = query.OrderByDescending(o => o.Id);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }

        /// <summary>
        /// Danh sách cấu trúc mã hợp đồng ở trạng thái Active
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstConfigContractCode> GetAllStatusActive(int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeEFRepository)}->{nameof(GetAllStatusActive)}: tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var result = _dbSet.Where(e => (partnerId == null || e.PartnerId == partnerId) && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Status == Status.ACTIVE && e.Deleted == YesNo.NO).OrderByDescending(o => o.Id);
            return result.ToList();
        }

        /// <summary>
        /// Tìm cấu trúc mã hợp đồng theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstConfigContractCode FindById(int configContractCodeId, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeEFRepository)}->{nameof(FindById)}: configContractCodeId = {configContractCodeId}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var result = _dbSet.FirstOrDefault(e => e.Id == configContractCodeId &&
                                    (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && (partnerId == null || e.PartnerId == partnerId) && e.Deleted == YesNo.NO);
            return result;
        }
    }
}
