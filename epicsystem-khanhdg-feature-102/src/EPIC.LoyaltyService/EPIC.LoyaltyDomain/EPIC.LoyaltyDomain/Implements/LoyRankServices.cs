using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Implements
{
    public class LoyRankServices : ILoyRankServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LoyRankServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<LinkVoucherConfiguration> _linkVoucherConfiguration;
        private readonly LoyaltyNotificationServices _loyaltyNotificationServices;
        private readonly LoyVoucherEFRepository _loyVoucherEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly LoyVoucherInvestorEFRepository _loyVoucherInvestorEFRepository;
        private readonly LoyHisAccumulatePointEFRepository _loyHisAccumulatePointEFRepository;
        private readonly LoyAccumulatePointStatusLogEFRepository _loyAccumulatePointStatusLogEFRepository;
        private readonly LoyRankEFRepoistory _loyRankEFRepository;
        private readonly LoyPointInvestorEFRepoistory _loyPointInvestorEFRepository;

        public LoyRankServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<LoyRankServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<LinkVoucherConfiguration> linkVoucherConfiguration,
            LoyaltyNotificationServices loyaltyNotificationServices
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _linkVoucherConfiguration = linkVoucherConfiguration;
            _loyaltyNotificationServices = loyaltyNotificationServices;
            _loyVoucherEFRepository = new LoyVoucherEFRepository(dbContext, logger, _linkVoucherConfiguration.Value);
            _loyVoucherInvestorEFRepository = new LoyVoucherInvestorEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _loyHisAccumulatePointEFRepository = new LoyHisAccumulatePointEFRepository(dbContext, logger);
            _loyAccumulatePointStatusLogEFRepository = new LoyAccumulatePointStatusLogEFRepository(dbContext, logger);
            _loyRankEFRepository = new LoyRankEFRepoistory(dbContext, logger);
            _loyPointInvestorEFRepository = new LoyPointInvestorEFRepoistory(dbContext, logger);
        }

        /// <summary>
        /// Thêm config xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        public void Add(AddRankDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = GetCurrentTradingProviderIdCommon();
            _logger.LogInformation($"{nameof(Add)}: dto={JsonSerializer.Serialize(dto)}");

            if (dto.PointStart >= dto.PointEnd)
            {
                _loyRankEFRepository.ThrowException(ErrorCode.LoyRankPointStartMustLessThanPointEnd);
            }

            var entity = _mapper.Map<LoyRank>(dto);
            entity.TradingProviderId = tradingProviderId;
            _loyRankEFRepository.Add(entity, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật trạng thái
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateStatus(UpdateStatusDto dto)
        {
            _logger.LogInformation($"{nameof(UpdateStatus)}: dto={JsonSerializer.Serialize(dto)}");
            var rank = _loyRankEFRepository.FindById(dto.Id).ThrowIfNull(_dbContext, ErrorCode.LoyRankNotFound);

            if (dto.Status == LoyRankStatus.ACTIVE)
            {
                var isOverride = _loyRankEFRepository.AnyOverrideRank(rank.Id, rank.PointStart, rank.PointEnd, rank.TradingProviderId);
                if (isOverride)
                {
                    _loyRankEFRepository.ThrowException(ErrorCode.LoyRankPointRangeOverride);
                }
                rank.ActiveDate = DateTime.Now;
            }
            else if (rank.Status == LoyRankStatus.DEACTIVE)
            {
                rank.DeactiveDate = DateTime.Now;
            }

            rank.Status = dto.Status;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xoá mềm rank
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRank(int id)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(DeleteRank)}: id={id}");

            _loyRankEFRepository.Delete(id, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật rank 
        /// </summary>
        /// <param name="dto"></param>
        public void Update(UpdateRankDto dto)
        {
            _logger.LogInformation($"{nameof(UpdateStatus)}: dto={JsonSerializer.Serialize(dto)}");
            var rank = _loyRankEFRepository.FindById(dto.Id).ThrowIfNull(_dbContext, ErrorCode.LoyRankNotFound);

            rank.Name = dto.Name;
            rank.Description = dto.Description;
            rank.PointStart = dto.PointStart;
            rank.PointEnd = dto.PointEnd;
            rank.ActiveDate = dto.ActiveDate;
            rank.DeactiveDate = dto.DeactiveDate;

            if (rank.Status == LoyRankStatus.ACTIVE)
            {
                var isOverride = _loyRankEFRepository.AnyOverrideRank(dto.Id, dto.PointStart, dto.PointEnd, rank.TradingProviderId);
                if (isOverride)
                {
                    _loyRankEFRepository.ThrowException(ErrorCode.LoyRankPointRangeOverride);
                }

                rank.ActiveDate = DateTime.Now;
            }
            else if (rank.Status == LoyRankStatus.DEACTIVE)
            {
                rank.DeactiveDate = DateTime.Now;
            }

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewFindRankDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id={id}");

            var data = _loyRankEFRepository.FindById(id);
            return _mapper.Map<ViewFindRankDto>(data);
        }

        /// <summary>
        /// Phân trang tìm kiếm cấu hình xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindRankDto> FindAll(FindAllRankDto dto)
        {
            _logger.LogInformation($"{nameof(FindAll)}: dto={JsonSerializer.Serialize(dto)}");
            var tradingProviderId = GetCurrentTradingProviderIdCommon();

            var data = _loyRankEFRepository.FindAll(dto, tradingProviderId);
            return data;
        }

        #region app
        /// <summary>
        /// App xem điểm và hạng
        /// </summary>
        /// <returns></returns>
        public AppViewRankPointDto FindByInvestorId(int? tradingProviderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            _logger.LogInformation($"{nameof(FindByInvestorId)}: investorId={investorId}; tradingProviderId={tradingProviderId}");

            // Hạng hiện tại của nhà đầu tư
            var rankCurrentOfInvestor = _loyRankEFRepository.FindByInvestorId(investorId, tradingProviderId);
            var invPoint = _loyPointInvestorEFRepository.Get(investorId, tradingProviderId);

            // Điểm bắt đầu của hạng tiếp theo
            var nextPointRank = _dbContext.LoyRanks.Where(r => r.TradingProviderId == tradingProviderId && r.Status == LoyRankStatus.ACTIVE && r.Deleted == YesNo.NO
                                && r.PointStart > invPoint.TotalPoint).Select(x => x.PointStart).Min();

            return new AppViewRankPointDto
            {
                LoyCurrentPoint = invPoint?.CurrentPoint,
                LoyTotalPoint = invPoint?.TotalPoint,
                LoyTempPoint = invPoint?.TempPoint,
                PointEnd = rankCurrentOfInvestor?.PointEnd,
                PointStart = rankCurrentOfInvestor?.PointStart,
                RankId = rankCurrentOfInvestor?.Id,
                RankName = rankCurrentOfInvestor?.Name,
                NextPointRank = nextPointRank
            };
        }
        #endregion

        /// <summary>
        /// Hàm chung Lấy đlsc hiện tại
        /// </summary>
        /// <returns></returns>
        private int? GetCurrentTradingProviderIdCommon()
        {
            string usertype = CommonUtils.GetCurrentUserType(_httpContext); ;

            int? tradingProviderId = -1;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                tradingProviderId = null;
            }

            return tradingProviderId;
        }

    }
}
