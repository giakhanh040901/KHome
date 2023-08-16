using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Garner;
using System.ServiceModel;
using System.Drawing;
using System.Xml.Linq;
using EPIC.DataAccess.Models;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.GarnerDomain.Interfaces;
using EPIC.Entities;
using System.Text.Json;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using System.Net.Http;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.CoreRepositories;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Entities.DataEntities;
using MySqlX.XDevAPI.Common;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerProductServices : IGarnerProductServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerProductServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerProductTradingProviderEFRepository _garnerProductTradingProviderEFRepository;
        private readonly GarnerApproveEFRepository _garnerApproveEFRepository;
        private readonly GarnerProductFileEFRepository _garnerProductFileEFRepository;
        private readonly GarnerProductTypeEFRepository _garnerProductTypeEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;

        public GarnerProductServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerProductServices> logger,
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
            _garnerProductTypeEFRepository = new GarnerProductTypeEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _garnerProductFileEFRepository = new GarnerProductFileEFRepository(dbContext, logger);
        }

        public List<GarnerProductFileDto> FindAllListByProduct(int productId, int? documentType)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllListByProduct)}: productId = {productId},  documentType = {documentType}");
            var garnerProductFile = _garnerProductFileEFRepository.FindAllListByProduct(productId, documentType, partnerId);
            var resultItem = _mapper.Map<List<GarnerProductFileDto>>(garnerProductFile);
            return resultItem;
        }

        public GarnerProductFile UpdateProductFile(CreateGarnerProductFileDto input)
        {
            _logger.LogInformation($"{nameof(UpdateProductFile)}: input = {JsonSerializer.Serialize(input)}");
            
            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            _garnerHistoryUpdateEFRepository.HistoryCollateralUpdate(input, partnerId, username);
            var garnerProductFile = _garnerProductFileEFRepository.UpdateProductFile(input, partnerId, username);
            _dbContext.SaveChanges();
            return garnerProductFile;
        }

        public GarnerProductFile AddProductFile(int productId, CreateGarnerProductFileDto input)
        {
            _logger.LogInformation($"{nameof(AddProductFile)}:productId = {productId}, input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var garnerProductFile = _garnerProductFileEFRepository.AddProductFile(productId, input, partnerId, username);
            _dbContext.SaveChanges();
            return garnerProductFile;
        }

        public GarnerProductFile DeletedProductFile(int id)
        {
            _logger.LogInformation($"{nameof(DeletedProductFile)}: id = {id}");

            var productFileRemove = _garnerProductFileEFRepository.DeletedProductFile(id);
            _dbContext.SaveChanges();
            return productFileRemove;
        }

        /// <summary>
        /// Thêm sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        public void Add(CreateGarnerProductDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            
            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var productInsert = _garnerProductEFRepository.Add(_mapper.Map<GarnerProduct>(input), partnerId, username);

            // Nếu là sản phẩm tích lũy Bất động sản thì có thêm các loại hình dự án cho dự án
            if (input.ProductType == GarnerProductTypes.BAT_DONG_SAN && input.InvProductTypes != null)
            {
                _garnerProductTypeEFRepository.UpdateProductType(productInsert.Id, input.InvProductTypes.Cast<int>().ToList(), partnerId, username);
            }

            //Thêm lịch sử thêm sản phẩm
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT,
                RealTableId = productInsert.Id,
                Action = ActionTypes.THEM_MOI,
                Summary = input.Summary
            }, username);


            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Update(UpdateGarnerProductDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var productFind = _garnerProductEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (productFind == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            //Thêm lịch sử cập nhật sản phẩm

            //Mã Product
            if (productFind.Code != input.Code)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.Code, input.Code, GarnerFieldName.UPDATE_CODE, 
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            //Tên Product
            if (productFind.Name != input.Name)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.Name, input.Name, GarnerFieldName.UPDATE_NAME,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            //Số ký hạn
            if (productFind.CpsPeriod != input.CpsPeriod)
            {
                var oldValue = $"{productFind.CpsPeriod} {productFind.CpsPeriodUnit}";
                var newValue = $"{input.CpsPeriod} {input.CpsPeriodUnit}";
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, oldValue, newValue, GarnerFieldName.UPDATE_CPS_PERIOD,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            //Đơn vị kỳ hạn
            if (productFind.CpsPeriodUnit != input.CpsPeriodUnit)
            {
                var oldValue = $"{productFind.CpsPeriod} {productFind.CpsPeriodUnit}";
                var newValue = $"{input.CpsPeriod} {input.CpsPeriodUnit}";
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, oldValue, newValue, GarnerFieldName.UPDATE_CPS_PERIOD_UNIT,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            //Cổ tức
            if (productFind.CpsInterestRate != input.CpsInterestRate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CpsInterestRate?.ToString(), input.CpsInterestRate?.ToString(), GarnerFieldName.UPDATE_CPS_INTEREST_RATE,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Kiểu trả cổ tức
            if (productFind.CpsInterestRateType != input.CpsInterestRateType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CpsInterestRateType?.ToString(), input.CpsInterestRateType?.ToString() + "", GarnerFieldName.UPDATE_CPS_INTEREST_RATE_TYPE,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Số kỳ trả cổ tức
            if (productFind.CpsInterestPeriod != input.CpsInterestPeriod)
            {
                var oldValue = $"{productFind.CpsInterestPeriod} {productFind.CpsInterestPeriodUnit}";
                var newValue = $"{input.CpsInterestPeriod} {input.CpsInterestPeriodUnit}";
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, oldValue, newValue, GarnerFieldName.UPDATE_CPS_INTEREST_PERIOD,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Đơn vị số kỳ trả cổ tức
            if (productFind.CpsInterestPeriodUnit != input.CpsInterestPeriodUnit)
            {
                var oldValue = $"{productFind.CpsInterestPeriod} {productFind.CpsInterestPeriodUnit}";
                var newValue = $"{input.CpsInterestPeriod} {input.CpsInterestPeriodUnit}";
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, oldValue, newValue, GarnerFieldName.UPDATE_CPS_INTEREST_PERIOD_UNIT,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Số ngày chốt quyền 
            if (productFind.CpsNumberClosePer != input.CpsNumberClosePer)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CpsNumberClosePer?.ToString(), input.CpsNumberClosePer?.ToString(), GarnerFieldName.UPDATE_CPS_NUMBER_CLOSE_PER,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Hình thức trả cổ tức
            if (productFind.CountType != input.CountType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CountType.ToString(), input.CountType.ToString(), GarnerFieldName.UPDATE_COUNT_TYPE,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Mệnh giá
            if (productFind.CpsParValue != input.CpsParValue)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CpsParValue?.ToString(), input.CpsParValue?.ToString(), GarnerFieldName.UPDATE_PAR_VALUE,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Số Lượng cổ phần
            if (productFind.CpsQuantity != input.CpsQuantity)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CpsQuantity?.ToString(), input.CpsQuantity?.ToString(), GarnerFieldName.UPDATE_CPS_QUANTITY,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Ngày phát hành
            if (productFind.StartDate != input.StartDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.StartDate?.ToString(), input.StartDate?.ToString(), GarnerFieldName.UPDATE_START_DATE,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Ngày đáo hạn
            if (productFind.EndDate != input.EndDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.EndDate?.ToString(), input.EndDate?.ToString(), GarnerFieldName.UPDATE_END_DATE,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Số khách hàng tối đa
            if (productFind.MaxInvestor != input.MaxInvestor)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.MaxInvestor?.ToString(), input.MaxInvestor?.ToString(), GarnerFieldName.UPDATE_MAX_INVESTOR,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Số ngày nắm giữ tối thiểu
            if (productFind.MinInvestDay != input.MinInvestDay)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.MinInvestDay?.ToString(), input.MinInvestDay?.ToString(), GarnerFieldName.UPDATE_MIN_INVEST_DAY,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Bảo lãnh thanh toán 
            if (productFind.IsPaymentGurantee != input.IsPaymentGurantee)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.IsPaymentGurantee, input.IsPaymentGurantee, GarnerFieldName.UPDATE_CPS_IS_PAYMENT_GURANTEE,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Tài sản đảm bảo
            if (productFind.IsCollateral != input.IsCollateral)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.IsCollateral, input.IsCollateral, GarnerFieldName.UPDATE_IS_COLLATERAL,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Niêm yết
            if (productFind.CpsIsListing != input.CpsIsListing)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CpsIsListing, input.CpsIsListing, GarnerFieldName.UPDATE_CPS_IS_LISTING,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }

            // Cho bán lại trước hạn
            if (productFind.CpsIsAllowSBD != input.CpsIsAllowSBD)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate(productFind.Id, productFind.CpsIsAllowSBD, input.CpsIsAllowSBD, GarnerFieldName.UPDATE_CPS_IS_ALLOW_SBD,
                    GarnerHistoryUpdateTables.GAN_PRODUCT, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_GENNERAL_INFORMATION), username);
            }
            

            _garnerProductEFRepository.Update(_mapper.Map<GarnerProduct>(input), partnerId, username);

            // Nếu là sản phẩm tích lũy Bất động sản thì có thêm các loại hình dự án cho dự án
            if (input.ProductType == GarnerProductTypes.BAT_DONG_SAN && input.InvProductTypes != null)
            {
                _garnerProductTypeEFRepository.UpdateProductType(productFind.Id, input.InvProductTypes.Cast<int>().ToList(), partnerId, username);
            }

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa sản phẩm tích lũy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="summary"></param>
        /// <exception cref="FaultException"></exception>
        public void Delete(int id, string summary)
        {
            _logger.LogInformation($"{nameof(Delete)}: Id = {id}, Summary = {summary}");

            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var productFind = _garnerProductEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.PartnerId == partnerId);
            if (productFind == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)}"), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }
            productFind.Deleted = YesNo.YES;

            //Thêm lịch sử cập nhật sản phẩm
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT,
                RealTableId = productFind.Id,
                Action = ActionTypes.XOA,
                Summary = summary
            }, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thông tin chi tiết sản phẩm tích lũy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public GarnerProductDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");
            
            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var result = new GarnerProductDto();
            var productTradingProvider = new List<GarnerProductTradingProviderDto>();

            var productFind = _garnerProductEFRepository.FindById(id);
            if (productFind == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)}"), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            result = _mapper.Map<GarnerProductDto>(productFind);

            // Nếu là sản phẩm tích lũy Cổ Phần
            if (productFind.ProductType == GarnerProductTypes.CO_PHAN)
            {
                // Lấy loại hình dự án
                result.InvProductTypes = _garnerProductTypeEFRepository.FindAllByProductId(id).Select(g => g.Type).ToList();

                // Lấy thông tin Tổ chức phát hành
                result.CpsIssuer = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.CpsIssuerId ?? 0));

                // Lấy thông tin Đại lý lưu ký
                result.CpsDepositProvider = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.CpsDepositProviderId ?? 0));
            }

            // Nếu là sản phẩm tích lũy Bất Động sản
            else if (productFind.ProductType == GarnerProductTypes.BAT_DONG_SAN)
            {
                //Lấy loại hình dự án
                result.InvProductTypes = _garnerProductTypeEFRepository.FindAllByProductId(id).Select(g => g.Type).ToList();

                // Lấy thông tin Chủ đầu tư
                result.InvOwner = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.InvOwnerId ?? 0));

                // Lấy thông tin Tổng thầu
                result.InvGeneralContractor = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.InvGeneralContractorId ?? 0));
            }

            // Lấy thông tin Đại lý phân phối sản phẩm tích lũy
            var productTradingProviderFind = _garnerProductTradingProviderEFRepository.FindAllList(id, partnerId);
            foreach (var item in productTradingProviderFind)
            {
                var resultItem = _mapper.Map<GarnerProductTradingProviderDto>(item);

                var tradingProviderFind = _tradingProviderEFRepository.FindById(item.TradingProviderId);
                if (tradingProviderFind != null)
                {
                    resultItem.TradingProvider = _mapper.Map<TradingProviderDto>(tradingProviderFind);

                    var businessCustomerFind = _businessCustomerEFRepository.FindById(tradingProviderFind.BusinessCustomerId);
                    if (businessCustomerFind != null)
                    {
                        resultItem.TradingProvider.BusinessCustomer = businessCustomerFind;
                    }
                }
                productTradingProvider.Add(resultItem);
            }
            result.ProductTradingProvider = productTradingProvider;

            // Lịch sử update sản phẩm phân phối đầu tư
            var historyUpdates = _garnerHistoryUpdateEFRepository.FindByProductId(id);
            result.HistoryUpdate = historyUpdates;
            return result;
        }

        /// <summary>
        /// thay đổi trạng thái status đóng và hoạt động 
        /// </summary>
        /// <param name="productId"></param>
        public void ChangeStatus(int productId)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: distributionId = {productId}");
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var distributionFind = _garnerProductEFRepository.ChangeStatus(productId, partnerId);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerProductDto> FindAll(FilterGarnerProductDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            int? partnerId = null;
            if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }

            var resultPaging = new PagingResult<GarnerProductDto>();
            var result = new List<GarnerProductDto>();
            var find = _garnerProductEFRepository.FindAll(input, partnerId, tradingProviderId);
            foreach (var item in find.Items)
            {
                var resultItem = new GarnerProductDto();
                resultItem = _mapper.Map<GarnerProductDto>(item);
                // Nếu là sản phẩm tích lũy Cổ Phần
                if (item.ProductType == GarnerProductTypes.CO_PHAN)
                {
                    // Lấy thông tin Tổ chức phát hành
                    resultItem.CpsIssuer = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(item.CpsIssuerId ?? 0));
                }

                // Nếu là sản phẩm tích lũy Bất Động sản
                else if (item.ProductType == GarnerProductTypes.BAT_DONG_SAN)
                {
                    // Lấy thông tin Chủ đầu tư
                    resultItem.InvOwner = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(item.InvOwnerId ?? 0));
                }
                result.Add(resultItem);
            }
            // lọc theo tổ chức phát hành
            result = result.Where(p => input.IssuerId == null || (p.CpsIssuer?.BusinessCustomerId != null && input.IssuerId == p.CpsIssuer.BusinessCustomerId) || (p.InvOwner?.BusinessCustomerId != null && input.IssuerId == p.InvOwner.BusinessCustomerId)).ToList();
            resultPaging.Items = result;
            resultPaging.TotalItems = result.Count();
            return resultPaging;
        }


        /// <summary>
        /// lấy danh sách tổ chức phát hành không trùng nhau
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<BusinessCustomerDto> DistinctIssuer(FilterGarnerProductDto input)
        {
            _logger.LogInformation($"{nameof(DistinctIssuer)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            int? partnerId = null;
            if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            var businessCustomer = new List<BusinessCustomerDto>();
            var find = _garnerProductEFRepository.FindAll(input, partnerId, tradingProviderId);

            foreach (var item in find.Items)
            {
                var resultItem = new GarnerProductDto();
                resultItem = _mapper.Map<GarnerProductDto>(item);
                // Nếu là sản phẩm tích lũy Cổ Phần
                if (item.ProductType == GarnerProductTypes.CO_PHAN)
                {
                    // Lấy thông tin Tổ chức phát hành
                    var issuer = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(item.CpsIssuerId ?? 0));
                    if (issuer != null)
                    {
                        businessCustomer.Add(issuer);
                    }
                }

                // Nếu là sản phẩm tích lũy Bất Động sản
                else if (item.ProductType == GarnerProductTypes.BAT_DONG_SAN)
                {
                    // Lấy thông tin Chủ đầu tư
                    var ower = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(item.InvOwnerId ?? 0));
                    if (ower != null)
                    {
                        businessCustomer.Add(ower);
                    }
                }
            }
            businessCustomer = businessCustomer.GroupBy(b => new { b.BusinessCustomerId }).Select(g => g.First()).ToList();
            return businessCustomer;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy cho đại lý
        /// </summary>
        /// <returns></returns>
        public List<GarnerProductByTradingProviderDto> GetListProductByTradingProvider()
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var listProductFind = _garnerProductEFRepository.GetListProductByTradingProvider(tradingProviderId);
            var result = new List<GarnerProductByTradingProviderDto>();
            foreach (var item in listProductFind)
            {
                var resultItem = new GarnerProductByTradingProviderDto();
                resultItem = _mapper.Map<GarnerProductByTradingProviderDto>(item);

                // Nếu là sản phẩm tích lũy Cổ Phần
                if (item.ProductType == GarnerProductTypes.CO_PHAN)
                {
                    // Lấy thông tin Tổ chức phát hành
                    resultItem.CpsIssuer = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(item.CpsIssuerId ?? 0));
                }

                // Nếu là sản phẩm tích lũy Bất Động sản
                else if (item.ProductType == GarnerProductTypes.BAT_DONG_SAN)
                {
                    // Lấy thông tin Tổng thầu
                    resultItem.InvGeneralContractor = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(item.InvGeneralContractorId ?? 0));
                }

                var productTradingProvider = _garnerProductTradingProviderEFRepository.FindByTradingProviderProduct(item.Id, tradingProviderId);
                if (productTradingProvider != null)
                {
                    resultItem.TotalInvestmentSub = productTradingProvider.TotalInvestmentSub;
                    resultItem.IsProfitFromPartner = productTradingProvider.IsProfitFromPartner;
                }    
                result.Add(resultItem);
            }

            return result;
        }

        public List<GarnerHistoryUpdate> FindHistoryUpdateById(int id)
        {
            // Lịch sử update sản phẩm phân phối đầu tư
            var historyUpdates = _garnerHistoryUpdateEFRepository.FindByProductId(id);
            return historyUpdates;
        }
        #region Trình duyệt sản phẩm tích lũy
        /// <summary>
        /// Yêu cầu trình duyệt
        /// </summary>
        /// <param name="input"></param>
        public void Request(CreateGarnerRequestDto input)
        {
            _logger.LogInformation($"{nameof(Request)}: input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var actionType = ActionTypes.THEM_MOI;

            //Tìm kiếm thông tin dự án
            var findProduct = _garnerProductEFRepository.FindById(input.Id);
            if (findProduct == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)}"), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            // Status = 1 : trình duyệt với, Status = 4 trình duyệt lại sản phẩm
            if (findProduct.Status == DistributionStatus.KHOI_TAO || findProduct.Status == DistributionStatus.HUY_DUYET)
            {
                //Nếu đã tồn tại bản ghi trước đó
                var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_GARNER_PRODUCT);
                if (findRequest != null)
                {
                    actionType = ActionTypes.CAP_NHAT;
                }

                var request = _garnerApproveEFRepository.Request(new GarnerApprove
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = actionType,
                    DataType = GarnerApproveDataTypes.GAN_GARNER_PRODUCT,
                    ReferId = input.Id,
                    Summary = input.Summary,
                    CreatedBy = username,
                    TradingProviderId = null,
                    PartnerId = partnerId
                });

                findProduct.Status = DistributionStatus.CHO_DUYET;
                request.DataStatus = DistributionStatus.CHO_DUYET;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Duyệt sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Approve(GarnerApproveDto input)
        {
            _logger.LogInformation($"{nameof(Approve)}: input = {JsonSerializer.Serialize(input)}");

            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            //Tìm kiếm thông tin dự án
            var findProduct = _garnerProductEFRepository.FindById(input.Id);
            if (findProduct == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)}"), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            if (findProduct.Status == DistributionStatus.CHO_DUYET)
            {
                var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_GARNER_PRODUCT);
                if (findRequest == null)
                {
                    throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerApproveNotFound)), new FaultCode(((int)ErrorCode.GarnerApproveNotFound).ToString()), "");
                }
                else
                {
                    _garnerApproveEFRepository.Approve(new GarnerApprove
                    {
                        Id = findRequest.Id,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId,
                    });
                }
                findProduct.Status = DistributionStatus.HOAT_DONG;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Huy duyet san pham
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Cancel(GarnerCancelDto input)
        {
            _logger.LogInformation($"{nameof(Cancel)}: input = {JsonSerializer.Serialize(input)}");

            //Tìm kiếm thông tin dự án
            var findProduct = _garnerProductEFRepository.FindById(input.Id);
            if (findProduct == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_GARNER_PRODUCT);
            if (findRequest == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerApproveNotFound)), new FaultCode(((int)ErrorCode.GarnerApproveNotFound).ToString()), "");
            }
            else
            {
                _garnerApproveEFRepository.Cancel(new GarnerApprove
                {
                    Id = findRequest.Id,
                    CancelNote = input.CancelNote
                });
            }
            //Cập nhật trạng thái cho dự án
            findProduct.Status = DistributionStatus.HUY_DUYET;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Kiểm tra sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Check(GarnerCheckDto input)
        {
            _logger.LogInformation($"{nameof(Check)}: input = {JsonSerializer.Serialize(input)}");

            var userId = CommonUtils.GetCurrentUserId(_httpContext);

            //Tìm kiếm thông tin dự án
            var findProduct = _garnerProductEFRepository.FindById(input.Id);
            if (findProduct == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }

            var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_GARNER_PRODUCT);
            if (findRequest == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerApproveNotFound)), new FaultCode(((int)ErrorCode.GarnerApproveNotFound).ToString()), "");
            }
            else
            {
                _garnerApproveEFRepository.Cancel(new GarnerApprove
                {
                    Id = findRequest.Id,
                    UserCheckId = userId
                });
            }
            findProduct.IsCheck = YesNo.YES;
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
