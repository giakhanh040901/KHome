using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerProductTradingProviderServices : IGarnerProductTradingProviderServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerProductTradingProviderServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerProductTradingProviderEFRepository _garnerProductTradingProviderEFRepository;
        private readonly GarnerApproveEFRepository _garnerApproveEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;

        public GarnerProductTradingProviderServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerProductTradingProviderServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _garnerProductTradingProviderEFRepository = new GarnerProductTradingProviderEFRepository(dbContext, logger);
            _garnerApproveEFRepository = new GarnerApproveEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm mới sản phẩm cho đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Add(CreateGarnerProductTradingProviderDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var productFind = _garnerProductEFRepository.FindById(input.ProductId);
            if (productFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sản phẩm tích lũy"), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            // Nếu có yêu cầu hạn mức check Required
            if (input.HasTotalInvestmentSub == YesNo.YES && (input.TotalInvestmentSub == null || input.Quantity == null))
            {
                if (input.TotalInvestmentSub == null)
                {
                    throw new FaultException(new FaultReason($"Yêu cầu hạn mức: Số tiền không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }

                // Nếu là sản phẩm cổ phần hoặc cổ phiếu kiểm tra thêm số lượng
                else if ((productFind.ProductType == GarnerProductTypes.CO_PHAN || productFind.ProductType == GarnerProductTypes.CO_PHIEU) && input.Quantity == null)
                {
                    throw new FaultException(new FaultReason($"Yêu cầu hạn mức: Số lượng không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }

            // Yêu cầu hạn mức: Nếu là Cổ phần hoặc Cổ phiếu check hạn mức đầu tư
            if (input.HasTotalInvestmentSub == YesNo.YES && (productFind.ProductType == GarnerProductTypes.CO_PHAN || productFind.ProductType == GarnerProductTypes.CO_PHIEU))
            {
                //Danh sách đại lý đã được phân phối
                var productTradingProviderList = _garnerProductTradingProviderEFRepository.FindAllList(input.ProductId, partnerId);

                // Tổng số lượng đã phân phối cho các Đại lý
                var soLuongDaPhanPhoi = productTradingProviderList.Select(r => r.Quantity).Sum();

                // Tổng số tiền đã phân phối
                var soTienDaPhanPhoi = productTradingProviderList.Select(r => r.TotalInvestmentSub).Sum();

                // Số lượng đầu tư tối đa
                var soLuongDauTuToiDa = productFind.CpsQuantity - soLuongDaPhanPhoi;

                // Số tiền đầu tư tối đa
                var soTienDauTuToiDa = (productFind.CpsQuantity * productFind.CpsParValue) - soTienDaPhanPhoi;

                if (input.TotalInvestmentSub > soTienDauTuToiDa)
                {
                    throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductTradingProviderTotalInvestmentSubTooLong)), new FaultCode(((int)ErrorCode.GarnerProductTradingProviderTotalInvestmentSubTooLong).ToString()), "");
                }

                if (input.Quantity > soLuongDauTuToiDa)
                {
                    throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductTradingProviderQuantityTooLong)), new FaultCode(((int)ErrorCode.GarnerProductTradingProviderQuantityTooLong).ToString()), "");
                }

            }
            var productTradingProviderInsert = _garnerProductTradingProviderEFRepository.Add(_mapper.Map<GarnerProductTradingProvider>(input), partnerId, username);

            //Thêm lịch sử phân phối cho đại lý
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_TRADING_PROVIDER,
                RealTableId = productTradingProviderInsert.Id,
                Action = ActionTypes.THEM_MOI,
                Summary = input.Summary
            }, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật phân phối sản phẩm cho đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Update(UpdateGarnerProductTradingProviderDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var productFind = _garnerProductEFRepository.FindById(input.ProductId);
            if (productFind == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            //Có yêu cầu hạn mức
            if (input.HasTotalInvestmentSub == YesNo.YES && (input.TotalInvestmentSub == null || input.Quantity == null))
            {
                if (input.TotalInvestmentSub == null)
                {
                    throw new FaultException(new FaultReason($"Yêu cầu hạn mức: Số tiền không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }

                // Nếu là sản phẩm cổ phần hoặc cổ phiếu kiểm tra thêm số lượng
                else if ((productFind.ProductType == GarnerProductTypes.CO_PHAN || productFind.ProductType == GarnerProductTypes.CO_PHIEU) && input.Quantity == null)
                {
                    throw new FaultException(new FaultReason($"Yêu cầu hạn mức: Số lượng không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }

            // Yêu cầu hạn mức: Nếu là Cổ phần hoặc Cổ phiếu check hạn mức đầu tư
            if (input.HasTotalInvestmentSub == YesNo.YES && (productFind.ProductType == GarnerProductTypes.CO_PHAN || productFind.ProductType == GarnerProductTypes.CO_PHIEU))
            {
                //Danh sách đại lý đã được phân phối
                var productTradingProviderList = _garnerProductTradingProviderEFRepository.FindAllList(input.ProductId, partnerId);

                productTradingProviderList = productTradingProviderList.Where(p => !productTradingProviderList.Select(i => i.Id).Contains(input.Id)).ToList();

                // Tổng số lượng đã phân phối cho các Đại lý
                var soLuongDaPhanPhoi = productTradingProviderList.Select(r => r.Quantity).Sum();

                // Tổng số tiền đã phân phối
                var soTienDaPhanPhoi = productTradingProviderList.Select(r => r.TotalInvestmentSub).Sum();

                // Số lượng đầu tư tối đa
                var soLuongDauTuToiDa = productFind.CpsQuantity - soLuongDaPhanPhoi;

                // Số tiền đầu tư tối đa
                var soTienDauTuToiDa = (productFind.CpsQuantity * productFind.CpsParValue) - soTienDaPhanPhoi;

                if (input.TotalInvestmentSub > soTienDauTuToiDa)
                {
                    throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductTradingProviderTotalInvestmentSubTooLong)), new FaultCode(((int)ErrorCode.GarnerProductTradingProviderTotalInvestmentSubTooLong).ToString()), "");
                }

                if (input.Quantity > soLuongDauTuToiDa)
                {
                    throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductTradingProviderQuantityTooLong)), new FaultCode(((int)ErrorCode.GarnerProductTradingProviderQuantityTooLong).ToString()), "");
                }
            }

            // lịch sử cập nhật đại lý phân phối
            _garnerHistoryUpdateEFRepository.HistoryProductTradingProvider(_mapper.Map<GarnerProductTradingProvider>(input), partnerId, username);

            _garnerProductTradingProviderEFRepository.Update(_mapper.Map<GarnerProductTradingProvider>(input), partnerId, username);

            _dbContext.SaveChanges();
        }

        public List<GarnerProductTradingProviderDto> FindAllByProduct(int productId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: productId = {productId}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var tradingProvider = _garnerProductTradingProviderEFRepository.FindAllList(productId, partnerId);

            var result = _mapper.Map<List<GarnerProductTradingProviderDto>>(tradingProvider);
            foreach (var item in result)
            {
                var tradingProviderFind = _tradingProviderEFRepository.FindById(item.TradingProviderId);
                if (tradingProviderFind != null)
                {
                    item.TradingProvider = _mapper.Map<TradingProviderDto>(tradingProviderFind);

                    var businessCustomerFind = _businessCustomerEFRepository.FindById(tradingProviderFind.BusinessCustomerId);
                    if (businessCustomerFind != null)
                    {
                        item.TradingProvider.BusinessCustomer = businessCustomerFind;
                    }
                }
            }
            return result.OrderByDescending(e => e.Id).ToList();
        }

        /// <summary>
        /// Xem thông tin chi tiết phân phối đại lý
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerProductTradingProviderDto FindById(int id)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = new GarnerProductTradingProviderDto();
            var productTradingProviderFind = _garnerProductTradingProviderEFRepository.FindById(id, partnerId);
            result = _mapper.Map<GarnerProductTradingProviderDto>(productTradingProviderFind);

            var tradingProviderFind = _tradingProviderEFRepository.FindById(id);
            if (tradingProviderFind != null)
            {
                result.TradingProvider = _mapper.Map<TradingProviderDto>(tradingProviderFind);

                var businessCustomerFind = _businessCustomerEFRepository.FindById(tradingProviderFind.BusinessCustomerId);
                if (businessCustomerFind != null)
                {
                    result.TradingProvider.BusinessCustomer = businessCustomerFind;
                }
            }
            return result;
        }

        public List<BusinessCustomerBankDto> FindBankByTrading(int? distributionId = null, int? type = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: distributionId = {distributionId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var businessCustomerBanks = _garnerProductTradingProviderEFRepository.FindBankByTrading(tradingProviderId, distributionId, type);
            var result = new List<BusinessCustomerBankDto>();
            foreach (var businessCustomerBank in businessCustomerBanks)
            {
                result.Add(new BusinessCustomerBankDto()
                { 
                    BusinessCustomerBankAccId = businessCustomerBank.BusinessCustomerBankAccId,
                    BusinessCustomerId = businessCustomerBank.BusinessCustomerId,
                    BankAccName = businessCustomerBank.BankAccName,
                    BankAccNo = businessCustomerBank.BankAccNo,
                    BankName = businessCustomerBank.BankName,
                    BankId = businessCustomerBank.BankId,
                    Status = businessCustomerBank.Status,
                    IsDefault = businessCustomerBank.IsDefault,
                });
            }
            return result;
        }
    }
}
