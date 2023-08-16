using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.LoyaltyEntities.Dto.LoyPointInvestor;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Loyalty;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Implements
{
    public class LoyPointInvestorServices : ILoyPointInvestorServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LoyPointInvestorServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly LoyaltyNotificationServices _loyaltyNotificationServices;
        private readonly LoyVoucherEFRepository _loyVoucherEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly LoyPointInvestorEFRepoistory _loyPointInvestorEFRepoistory;
        private readonly LoyVoucherInvestorEFRepository _loyVoucherInvestorEFRepository;

        public LoyPointInvestorServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<LoyPointInvestorServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<LinkVoucherConfiguration> linkVoucherConfiguration,
            LoyaltyNotificationServices loyaltyNotificationServices
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;;
            _loyaltyNotificationServices = loyaltyNotificationServices;
            _loyVoucherInvestorEFRepository = new LoyVoucherInvestorEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _loyPointInvestorEFRepoistory = new LoyPointInvestorEFRepoistory(dbContext, logger);
        }

        /// <summary>
        /// Lịch sử quy đổi điểm của nhà đầu tư/ Tab Danh sách ưu đãi
        /// </summary>
        public PagingResult<PointInvestorConversionHistoryDto> FindAllVoucherConversionHistory(FilterPointInvestorConversionHistoryDto dto)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _loyPointInvestorEFRepoistory.FindAllVoucherConversionHistory(dto, tradingProviderId);
        }

        /// <summary>
        /// Xem chi tiết Tab Danh sách ưu đãi
        /// </summary>
        public PointInvestorConversionHistoryInfoDto VoucherConversionHistoryGetById(int conversionPointDetailId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = (from voucher in _dbContext.LoyVouchers
                          join conversionPointDetail in _dbContext.LoyConversionPointDetails on voucher.Id equals conversionPointDetail.VoucherId
                          join conversionPoint in _dbContext.LoyConversionPoints on conversionPointDetail.ConversionPointId equals conversionPoint.Id
                          join investor in _dbContext.Investors on conversionPoint.InvestorId equals investor.InvestorId
                          join pointInvestor in _dbContext.LoyPointInvestors on investor.InvestorId equals pointInvestor.InvestorId
                          where voucher.Deleted == YesNo.NO && conversionPointDetail.Deleted == YesNo.NO && conversionPoint.Deleted == YesNo.NO && investor.Deleted == YesNo.NO
                              && (tradingProviderId == voucher.TradingProviderId) && conversionPoint.TradingProviderId == voucher.TradingProviderId
                              && pointInvestor.TradingProviderId == voucher.TradingProviderId && conversionPointDetailId == conversionPointDetail.Id
                          select new PointInvestorConversionHistoryInfoDto
                          {
                              InvestorId = investor.InvestorId,
                              Phone = investor.Phone,
                              Email = investor.Email,
                              Fullname = _dbContext.InvestorIdentifications.FirstOrDefault(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO && i.Status == Status.ACTIVE
                                            && i.IsDefault == YesNo.YES).Fullname,
                              ConversionPointId = conversionPoint.Id,
                              LoyCurrentPoint = conversionPoint.CurrentPoint,
                              LoyTotalPoint = pointInvestor.TotalPoint,
                              VoucherPoint = conversionPointDetail.Point,
                              VoucherName = voucher.Name,
                              VoucherType = voucher.VoucherType,
                              VoucherValue = voucher.Value,
                              BannerImageUrl = voucher.BannerImageUrl,
                              VoucherUnit = voucher.Unit,
                          }).FirstOrDefault();
            if (result != null)
            {
                var conversionPointStatusLogQuery = _dbContext.LoyConversionPointStatusLogs.Where(c => c.ConversionPointId == result.ConversionPointId && c.Deleted == YesNo.NO)
                    .OrderBy(x => x.Id);
                result.ConversionPointStatusLogs = _mapper.Map<List<LoyConversionPointStatusLogDto>>(conversionPointStatusLogQuery);
            }
            return result;
        }
        /// <summary>
        /// Tìm kiếm nhà đầu tư theo số điện thoại và điểm ranh của theo đại lý
        /// </summary>
        public List<FindLoyPointInvestorDto> FindInvestorByPhone(string phone, int? investorId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _loyPointInvestorEFRepoistory.FindPointInvestor(phone, investorId, tradingProviderId);
        }
    }
}
