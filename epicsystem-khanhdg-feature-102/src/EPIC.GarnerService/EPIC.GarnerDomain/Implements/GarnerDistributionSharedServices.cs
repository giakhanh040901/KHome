using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerDistributionSharedServices : IGarnerDistributionSharedServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly GarnerProductTradingProviderEFRepository _garnerProductTradingProviderEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;

        public GarnerDistributionSharedServices(
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<GarnerDistributionSharedServices> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _garnerProductTradingProviderEFRepository = new GarnerProductTradingProviderEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Tính số lượng phân phối còn lại nếu đại lý chưa cài hạn mức
        /// Chưa đủ trường hợp: gồm các đại lý chưa cài hạn mức khác đã được tạo những hợp đồng Active (soTienDauTuToiDa chưa trừ thêm số lượng hợp đồng này)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="tradingProviderId"></param>
        public GarnerProductTradingProviderDto LimitCalculationTrading(int productId, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(LimitCalculationTrading)}: productId = {productId}, tradingProviderId = {tradingProviderId}");
            var result = new GarnerProductTradingProviderDto();

            var productTradingProviderFind = _garnerProductTradingProviderEFRepository.FindByTradingProviderProduct(productId, tradingProviderId);
            if (productTradingProviderFind != null)
            {
                result = _mapper.Map<GarnerProductTradingProviderDto>(productTradingProviderFind);
                // Nếu chưa cài hạn mức
                if (productTradingProviderFind.HasTotalInvestmentSub == YesNo.NO)
                {
                    // Tìm thông tin sản phẩm
                    var productFind = _garnerProductEFRepository.FindById(productId);
                    if (productFind != null)
                    {
                        //Danh sách đại lý đã được phân phối
                        var productTradingProviderList = _garnerProductTradingProviderEFRepository.FindAllList(productId);

                        // Tổng số lượng đã phân phối cho các Đại lý
                        var soLuongDaPhanPhoi = productTradingProviderList.Select(r => r.Quantity).Sum();

                        // Tổng số tiền đã phân phối
                        var soTienDaPhanPhoi = productTradingProviderList.Select(r => r.TotalInvestmentSub).Sum();

                        // Số lượng đầu tư tối đa
                        var soLuongDauTuToiDa = productFind.CpsQuantity - soLuongDaPhanPhoi;

                        // Số tiền đầu tư tối đa
                        decimal? soTienDauTuToiDa = (productFind.CpsQuantity * productFind.CpsParValue) - soTienDaPhanPhoi;

                        result.Quantity = soLuongDauTuToiDa;
                        result.TotalInvestmentSub = soTienDauTuToiDa;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra số tiền hạn mức còn đủ thêm total value
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        public bool CheckAddTotalValue(int distributionId, decimal totalValue)
        {
            _logger.LogInformation($"{nameof(CheckAddTotalValue)}: distributionId = {distributionId}, totalValue = {totalValue}");
            var distributionFind = _dbContext.GarnerDistributions.FirstOrDefault(d => d.Id == distributionId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);

            var project = _garnerProductEFRepository.FindById(distributionFind.ProductId);

            var limitCalculationTrading = LimitCalculationTrading(distributionFind.ProductId, distributionFind.TradingProviderId);

            //số tiền đã đầu tư
            var sumValue = _garnerOrderEFRepository.SumValue(distributionId, distributionFind.TradingProviderId);

            //nếu TotalInvestmentSub null hoặc bằng 0 thì tính theo công thức CpsParValue * CpsQuantity
            var hanMucDauTu = (limitCalculationTrading.TotalInvestmentSub == null || limitCalculationTrading.TotalInvestmentSub == 0) ? (project?.CpsParValue ?? 0) * (project?.CpsQuantity ?? 0) : limitCalculationTrading.TotalInvestmentSub;

            return hanMucDauTu - sumValue >= totalValue;
        }
    }
}
