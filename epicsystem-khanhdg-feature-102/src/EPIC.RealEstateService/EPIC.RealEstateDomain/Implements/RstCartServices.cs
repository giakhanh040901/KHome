using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstCartServices : IRstCartServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstCartServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstProjectPolicyEFRepository _rstProjectPolicyEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstSellingPolicyEFRepository _rstSellingPolicyEFRepository;
        private readonly RstCartEFRepository _rstCartEFRepository;
        private readonly RstDistributionProductItemEFRepository _rstDistributionProductItemEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;

        public RstCartServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstCartServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProjectPolicyEFRepository = new RstProjectPolicyEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstSellingPolicyEFRepository = new RstSellingPolicyEFRepository(dbContext, logger);
            _rstCartEFRepository = new RstCartEFRepository(dbContext, logger);
            _rstDistributionProductItemEFRepository = new RstDistributionProductItemEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        public void Add(CreateRstCartDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: {JsonSerializer.Serialize(input)}, investorId = {investorId}, username = {username}");

            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var openSellDetailIdItem in input.OpenSellDetailId)
            {
                // Kiểm tra xem đã lập sản phẩm mở bán chưa
                if (_rstCartEFRepository.EntityNoTracking.Where(c => c.InvestorId == investorId && c.OpenSellDetailId == openSellDetailIdItem
                    && c.Status == RstCartStatus.KhoiTao && c.Deleted == YesNo.NO).Any())
                {
                    _rstCartEFRepository.ThrowException(ErrorCode.RstCartExistOpenSellDetail);
                }
                var checkOpenSellDetail = (from openSellDetail in _rstOpenSellDetailEFRepository.EntityNoTracking
                                           join openSell in _rstOpenSellEFRepository.EntityNoTracking on openSellDetail.OpenSellId equals openSell.Id
                                           join distributionProductItem in _rstDistributionProductItemEFRepository.EntityNoTracking on openSellDetail.DistributionProductItemId equals distributionProductItem.Id
                                           join productItem in _rstProductItemEFRepository.EntityNoTracking on distributionProductItem.ProductItemId equals productItem.Id
                                           where openSellDetail.Id == openSellDetailIdItem && openSellDetail.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO
                                           && productItem.Deleted == YesNo.NO && openSellDetail.Status == RstProductItemStatus.KHOI_TAO && productItem.Status == RstProductItemStatus.KHOI_TAO
                                           && openSellDetail.IsLock == YesNo.NO && productItem.IsLock == YesNo.NO && distributionProductItem.Status == Status.ACTIVE
                                           && openSell.StartDate.Date <= DateTime.Now.Date
                                           select productItem).Any();
                if (!checkOpenSellDetail)
                {
                    _rstCartEFRepository.ThrowException(ErrorCode.RstCartOpenSellDetailStatusNotSelling);
                }  
                
                _rstCartEFRepository.Add(new RstCart
                {
                    InvestorId = investorId,
                    CreatedBy = username,
                    OpenSellDetailId = openSellDetailIdItem,
                });
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Xóa sản phẩm trong giỏ hàng
        /// </summary>
        public void Delete(int id)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, investorId = {investorId}, username = {username}");
            var cartQuery = _rstCartEFRepository.FindById(id, investorId).ThrowIfNull(_dbContext, ErrorCode.RstCartNotFoundByInvestor);
            cartQuery.Deleted = YesNo.YES;
            cartQuery.ModifiedBy = username;
            cartQuery.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Danh sách giỏ hàng của nhà đầu tư
        /// </summary>
        public List<AppRstCartDto> GetAllCart()
        {
            List<AppRstCartDto> result = new();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllCart)}: investorId = {investorId}");
            var cartQuery = _rstCartEFRepository.FindCartByInvestor(investorId, null, RstCartStatus.KhoiTao);
            foreach (var item in cartQuery)
            {
                var resultItem = _mapper.Map<AppRstCartDto>(item);
                // Chính sách ưu đãi
                resultItem.Policys = _rstSellingPolicyEFRepository.AppRstPolicyForProductItem(item.OpenSellId, item.ProductItemId);

                // Ảnh đại diện của căn hộ
                resultItem.UrlImage = _dbContext.RstProductItemMedias.FirstOrDefault(m => m.ProductItemId == item.ProductItemId && m.Deleted == YesNo.NO
                                                                        && m.Location == RstProductItemMediaLocations.ANH_DAI_DIEN_CAN_HO)?.UrlImage;
                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(item.Price, item.DistributionId);
                // Gía trị lock căn
                resultItem.LockPrice = productItemPrice.LockPrice;
                resultItem.DepositPrice = productItemPrice.DepositPrice;
                result.Add(resultItem);
            }
            return result;
        }
    }
}
