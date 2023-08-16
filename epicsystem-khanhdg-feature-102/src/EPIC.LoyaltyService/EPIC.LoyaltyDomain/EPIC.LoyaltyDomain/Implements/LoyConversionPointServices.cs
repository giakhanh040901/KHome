using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.LoyaltyEntities.Dto.LoyPointInvestor;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Implements
{
    public class LoyConversionPointServices : ILoyConversionPointServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LoyVoucherServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOptions<LinkVoucherConfiguration> _linkVoucherConfiguration;
        private readonly LoyaltyNotificationServices _loyaltyNotificationServices;
        private readonly LoyVoucherEFRepository _loyVoucherEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly LoyVoucherInvestorEFRepository _loyVoucherInvestorEFRepository;
        private readonly LoyHisAccumulatePointEFRepository _loyHisAccumulatePointEFRepository;
        private readonly LoyAccumulatePointStatusLogEFRepository _loyAccumulatePointStatusLogEFRepository;
        private readonly LoyPointInvestorEFRepoistory _loyPointInvestorEFRepository;
        private readonly LoyConversionPointEFRepository _loyConversionPointEFRepository;
        private readonly LoyConversionPointStatusLogEFRepository _loyConversionPointStatusLogEFRepository;

        public LoyConversionPointServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<LoyVoucherServices> logger,
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
            _loyPointInvestorEFRepository = new LoyPointInvestorEFRepoistory(dbContext, logger);
            _loyConversionPointEFRepository = new LoyConversionPointEFRepository(dbContext, logger);
            _loyConversionPointStatusLogEFRepository = new LoyConversionPointStatusLogEFRepository(dbContext, logger);
        }

        public LoyConversionPointDto Add(AddLoyConversionPointDto input)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            string usertype = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            var entity = _mapper.Map<LoyConversionPoint>(input);
            entity.Source = LoySource.OFFLINE;
            entity.TradingProviderId = tradingProviderId;
            entity.CreatedBy = username;
            var transaction = _dbContext.Database.BeginTransaction();
            // Kiểm tra xem nhà đầu tư đã có point chưa
            var investorPoint = _loyPointInvestorEFRepository.Get(input.InvestorId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyPointInvestorNotFound);
            entity.CurrentPoint = investorPoint.CurrentPoint;
            entity.IsMinusPoint = (input.AllocationType == LoyAllocationTypes.TANG_KHACH_HANG && input.IsMinusPoint == null) ? YesNo.NO : input.IsMinusPoint;

            // Thêm yêu cầu quy đồi
            var conversionPointInsert = _loyConversionPointEFRepository.Add(entity);
            _dbContext.SaveChanges();
            // Chi tiết quy đổi
            foreach (var item in input.Details)
            {
                // Tìm kiếm voucher
                var voucher = _loyVoucherEFRepository.FindById(item.VoucherId)
                    .ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);

                // Tổng số voucher mà nhà đầu tư đã đổi trước đó
                var totalQuantityVoucherOfInvestor = _dbContext.LoyConversionPointDetails.Where(d => d.VoucherId == voucher.Id && d.Deleted == YesNo.NO && _dbContext.LoyConversionPoints.Any(c => c.Id == d.ConversionPointId
                                && c.Status != LoyConversionPointStatus.CANCELED && c.InvestorId == input.InvestorId && d.Deleted == YesNo.NO)).Sum(d => d.Quantity);
                if (voucher.ExchangeRoundNum != null && voucher.ExchangeRoundNum < totalQuantityVoucherOfInvestor + item.Quantity)
                {
                    // Vượt quá hạn mức quy đổi voucher
                    _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyVoucherConversionIsOverLimit);
                }

                // Tổng số điểm quy đổi của voucher
                var totalConversionPoint = 0;

                // Trừ số tiền quy đổi khi Khách đổi điểm hoặc Tặng khách hàng và trừ điểm
                if (conversionPointInsert.AllocationType == LoyAllocationTypes.KHACH_HANG_DOI_DIEM
                    || conversionPointInsert.AllocationType == LoyAllocationTypes.TANG_KHACH_HANG && conversionPointInsert.IsMinusPoint == YesNo.YES)
                {
                    // Tính số điểm quy đổi
                    totalConversionPoint = voucher.Point * item.Quantity;
                    // Trừ điểm tích lũy
                    investorPoint.CurrentPoint -= totalConversionPoint;

                    // Nếu Điểm tích lũy dưới 0 thì báo lỗi
                    if (investorPoint.CurrentPoint < 0)
                    {
                        _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyRedemptionPointExceeded);
                    }
                }

                // Thêm chi tiết quy đổi
                _loyConversionPointEFRepository.LoyConversionPointDetailAdd(new LoyConversionPointDetail
                {
                    ConversionPointId = conversionPointInsert.Id,
                    VoucherId = item.VoucherId,
                    Quantity = item.Quantity,
                    Point = voucher.Point,
                    TotalConversionPoint = totalConversionPoint,
                    CreatedBy = username
                });
            }

            // Lưu lại lịch sử khi tạo yêu cầu
            _loyConversionPointStatusLogEFRepository.Add(new LoyConversionPointStatusLog
            {
                ConversionPointId = conversionPointInsert.Id,
                Note = input.Description,
                Status = LoyConversionPointStatus.CREATED,
                CreatedBy = username,
                Source = LoySource.OFFLINE
            });

            _dbContext.SaveChanges();
            transaction.Commit();
            return _mapper.Map<LoyConversionPointDto>(conversionPointInsert);
        }

        public void Update(UpdateLoyConversionPointDto input)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            string usertype = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            var transaction = _dbContext.Database.BeginTransaction();

            var conversionPointQuery = _loyConversionPointEFRepository.FindById(input.Id, tradingProviderId);
            // 
            var isMinusPointBeforeUpdate = conversionPointQuery.IsMinusPoint;
            // Cập nhật thông tin
            conversionPointQuery.AllocationType = input.AllocationType;
            conversionPointQuery.RequestType = input.RequestType;
            input.IsMinusPoint = (input.AllocationType == LoyAllocationTypes.TANG_KHACH_HANG && input.IsMinusPoint == null) ? YesNo.NO : input.IsMinusPoint;
            conversionPointQuery.IsMinusPoint = (input.AllocationType == LoyAllocationTypes.TANG_KHACH_HANG) ? input.IsMinusPoint : YesNo.YES;

            conversionPointQuery.Description = input.Description;
            _dbContext.SaveChanges();

            // Chuyển từ không trừ điểm khách hàng sang trừ điểm
            bool isChangeNotMinusToMinus = isMinusPointBeforeUpdate != conversionPointQuery.IsMinusPoint;

            // Kiểm tra xem có được trừ điểm khách hàng không
            bool isMinusPoint = conversionPointQuery.AllocationType == LoyAllocationTypes.KHACH_HANG_DOI_DIEM || conversionPointQuery.AllocationType == LoyAllocationTypes.TANG_KHACH_HANG && conversionPointQuery.IsMinusPoint == YesNo.YES;

            // Kiểm tra xem nhà đầu tư đã có point chưa
            var investorPointQuery = _loyPointInvestorEFRepository.Get(conversionPointQuery.InvestorId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyPointInvestorNotFound);

            int investorPoint = 0;
            // Danh sách chi tiết
            var conversionPointDetailQuery = _dbContext.LoyConversionPointDetails.Where(c => c.ConversionPointId == input.Id && c.Deleted == YesNo.NO);
            // Remove đi những voucher bị xóa 
            var conversionPointDetailRemove = conversionPointDetailQuery.Where(c => !input.Details.Select(d => d.Id).Contains(c.Id));
            foreach (var item in conversionPointDetailRemove)
            {
                // Trước khi update cho phép trừ điểm thì trả lại điểm cho Investor
                if (isMinusPointBeforeUpdate == YesNo.YES)
                {
                    // Cộng các lại các điểm đã được quy đổi
                    investorPoint += item.Point * item.Quantity;
                }
                item.Deleted = YesNo.YES;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }

            // Chi tiết quy đổi
            foreach (var item in input.Details)
            {
                // Kiểm tra thay đổi trong các chi tiết trước đó
                if (item.Id != 0 && item.Id != null)
                {
                    var conversionPointDetail = _dbContext.LoyConversionPointDetails.FirstOrDefault(c => c.Id == item.Id && c.ConversionPointId == conversionPointQuery.Id && c.Deleted == YesNo.NO)
                        .ThrowIfNull(_dbContext, ErrorCode.NotFound);

                    // Tổng số điểm quy đổi của voucher
                    var totalConversionPoint = 0;
                    // Trường hợp hay đổi voucher
                    if (item.VoucherId != conversionPointDetail.VoucherId)
                    {
                        // Tìm kiếm voucher mới thay đổi
                        var voucherNew = _loyVoucherEFRepository.FindById(item.VoucherId).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);

                        // Tổng số voucher mà nhà đầu tư đã đổi trước đó
                        var totalQuantityVoucherOfInvestor = _dbContext.LoyConversionPointDetails.Where(d => d.VoucherId == voucherNew.Id && d.Deleted == YesNo.NO && _dbContext.LoyConversionPoints.Any(c => c.Id == d.ConversionPointId
                                        && c.Status != LoyConversionPointStatus.CANCELED && c.InvestorId == conversionPointQuery.InvestorId && d.Deleted == YesNo.NO)).Sum(d => d.Quantity);
                        if (voucherNew.ExchangeRoundNum != null && voucherNew.ExchangeRoundNum < totalQuantityVoucherOfInvestor + item.Quantity)
                        {
                            // Vượt quá hạn mức quy đổi voucher
                            _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyVoucherConversionIsOverLimit);
                        }

                        // Trước khi update là trừ điểm thì hoàn lại điểm cho Investor
                        if (isMinusPointBeforeUpdate == YesNo.YES)
                        {
                            // Tính lại số điểm của Investor. Hoàn lại điểm được quy đổi ở chi tiết này
                            investorPoint += conversionPointDetail.Point * conversionPointDetail.Quantity;
                        }

                        // Nếu thoản mãn điều kiện trừ tiền của khách
                        if (isMinusPoint)
                        {
                            //Tổng số điểm quy đổi
                            totalConversionPoint = voucherNew.Point * item.Quantity;

                            // Tính lại số điểm của Investor. Trừ đi tổng số điểm quy đổi của voucher mới
                            investorPoint -= totalConversionPoint;
                        }

                        conversionPointDetail.TotalConversionPoint = totalConversionPoint;
                        conversionPointDetail.Point = voucherNew.Point;
                        conversionPointDetail.Quantity = item.Quantity;
                        conversionPointDetail.VoucherId = item.VoucherId;
                        conversionPointDetail.ModifiedBy = username;
                        conversionPointDetail.CreatedDate = DateTime.Now;
                    }

                    // Trường hợp chỉ thay đổi số lượng
                    if (conversionPointDetail.Quantity != item.Quantity && item.VoucherId == conversionPointDetail.VoucherId)
                    {
                        // Tìm kiếm voucher mới thay đổi
                        var voucher = _loyVoucherEFRepository.FindById(item.VoucherId).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);
                        // Tổng số voucher mà nhà đầu tư đã đổi trước đó
                        var totalQuantityVoucherOfInvestor = _dbContext.LoyConversionPointDetails.Where(d => d.VoucherId == voucher.Id && d.Deleted == YesNo.NO && _dbContext.LoyConversionPoints.Any(c => c.Id == d.ConversionPointId
                                        && c.Status != LoyConversionPointStatus.CANCELED && c.InvestorId == conversionPointQuery.InvestorId && d.Deleted == YesNo.NO)).Sum(d => d.Quantity);
                        if (voucher.ExchangeRoundNum != null && voucher.ExchangeRoundNum < totalQuantityVoucherOfInvestor - conversionPointDetail.Quantity + item.Quantity)
                        {
                            // Vượt quá hạn mức quy đổi voucher
                            _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyVoucherConversionIsOverLimit);
                        }

                        // Trước khi update là trừ điểm thì hoàn lại điểm cho Investor
                        if (isMinusPointBeforeUpdate == YesNo.YES)
                        {
                            // Tính lại số điểm của Investor. Hoàn lại điểm được quy đổi ở chi tiết này
                            investorPoint += conversionPointDetail.Point * conversionPointDetail.Quantity;
                        }

                        // Nếu thoản mãn điều kiện trừ tiền của khách
                        if (isMinusPoint)
                        {
                            //Tổng số điểm quy đổi mới
                            totalConversionPoint = conversionPointDetail.Point * item.Quantity;

                            // Tính lại số điểm của Investor. Trừ đi tổng số điểm quy đổi của voucher mới
                            investorPoint -= totalConversionPoint;
                        }

                        conversionPointDetail.TotalConversionPoint = totalConversionPoint;
                        conversionPointDetail.Quantity = item.Quantity;
                        conversionPointDetail.ModifiedBy = username;
                        conversionPointDetail.CreatedDate = DateTime.Now;
                    }

                    // Nếu Điểm tích lũy dưới 0 thì báo lỗi
                    if (investorPoint < 0)
                    {
                        _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyRedemptionPointExceeded);
                    }
                }
                else
                {
                    // Tìm kiếm voucher mới thay đổi
                    var voucher = _loyVoucherEFRepository.FindById(item.VoucherId).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);

                    // Tổng số voucher mà nhà đầu tư đã đổi trước đó
                    var totalQuantityVoucherOfInvestor = _dbContext.LoyConversionPointDetails.Where(d => d.VoucherId == voucher.Id && d.Deleted == YesNo.NO && _dbContext.LoyConversionPoints.Any(c => c.Id == d.ConversionPointId
                                    && c.Status != LoyConversionPointStatus.CANCELED && c.InvestorId == conversionPointQuery.InvestorId && d.Deleted == YesNo.NO)).Sum(d => d.Quantity);
                    if (voucher.ExchangeRoundNum != null && voucher.ExchangeRoundNum < totalQuantityVoucherOfInvestor + item.Quantity)
                    {
                        // Vượt quá hạn mức quy đổi voucher
                        _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyVoucherConversionIsOverLimit);
                    }

                    // Tổng số điểm quy đổi của voucher
                    var totalConversionPoint = 0;

                    // Trừ số điểm quy đổi
                    if (isMinusPoint)
                    {
                        // Tính số điểm quy đổi
                        totalConversionPoint = voucher.Point * item.Quantity;
                        // Trừ điểm tích lũy
                        investorPoint -= totalConversionPoint;
                        // Nếu Điểm tích lũy dưới 0 thì báo lỗi
                        if (investorPoint < 0)
                        {
                            _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyRedemptionPointExceeded);
                        }
                    }

                    // Thêm chi tiết quy đổi
                    _loyConversionPointEFRepository.LoyConversionPointDetailAdd(new LoyConversionPointDetail
                    {
                        ConversionPointId = conversionPointQuery.Id,
                        VoucherId = item.VoucherId,
                        TotalConversionPoint = totalConversionPoint,
                        Quantity = item.Quantity,
                        Point = voucher.Point,
                        CreatedBy = username
                    });
                }
            }
            investorPointQuery.CurrentPoint = investorPoint;
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Đổi trạng thái 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="conversionPointStatus"></param>
        public async Task ChangeStatusConversionPoint(UpdateLoyConversionPointStatusDto input, int conversionPointStatus)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var transaction = _dbContext.Database.BeginTransaction();
            var conversionPointQuery = _loyConversionPointEFRepository.FindById(input.Id, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyConversionPointNotFound);

            var conversionPointStatusLog = new LoyConversionPointStatusLog()
            {
                ConversionPointId = conversionPointQuery.Id,
                Note = input.Note,
                CreatedBy = username,
                Source = LoySource.OFFLINE
            };

            if (conversionPointStatus == LoyConversionPointStatus.PENDING)
            {
                conversionPointQuery.Status = LoyConversionPointStatus.PENDING;
                conversionPointStatusLog.Status = LoyConversionPointStatus.PENDING;
            }

            else if (conversionPointStatus == LoyConversionPointStatus.DELIVERY)
            {
                conversionPointQuery.Status = LoyConversionPointStatus.DELIVERY;
                conversionPointStatusLog.Status = LoyConversionPointStatus.DELIVERY;
            }
            else if (conversionPointStatus == LoyConversionPointStatus.FINISHED)
            {
                conversionPointQuery.Status = LoyConversionPointStatus.FINISHED;
                conversionPointStatusLog.Status = LoyConversionPointStatus.FINISHED;
            }

            _loyConversionPointStatusLogEFRepository.Add(conversionPointStatusLog);
            _dbContext.SaveChanges();
            transaction.Commit();
            if (conversionPointStatus == LoyConversionPointStatus.FINISHED)
            {
                await _loyaltyNotificationServices.SendNotificationInvestorReceivedVoucher(conversionPointQuery.Id);
            }
        }

        /// <summary>
        /// Hủy yêu cầu chuyển đổi. Nếu hủy đi thì trả lại điểm cho khách hàng
        /// </summary>
        public void ChangeStatusCancel(UpdateLoyConversionPointStatusDto input)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var transaction = _dbContext.Database.BeginTransaction();
            var conversionPointQuery = _loyConversionPointEFRepository.FindById(input.Id, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyConversionPointNotFound);

            // Trường hợp trừ điểm thì trả lại điểm cho khách hàng
            if (conversionPointQuery.AllocationType == LoyAllocationTypes.KHACH_HANG_DOI_DIEM
                || conversionPointQuery.AllocationType == LoyAllocationTypes.TANG_KHACH_HANG && conversionPointQuery.IsMinusPoint == YesNo.YES)
            {
                var investorPointQuery = _loyPointInvestorEFRepository.Get(conversionPointQuery.InvestorId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyPointInvestorNotFound);
                int totalConversionPoint = 0;
                // Danh sách chi tiết
                var conversionPointDetailQuery = _dbContext.LoyConversionPointDetails.Where(c => c.ConversionPointId == input.Id && c.Deleted == YesNo.NO);
                foreach (var item in conversionPointDetailQuery)
                {
                    totalConversionPoint += item.Quantity * item.Point;
                }

                investorPointQuery.CurrentPoint += totalConversionPoint;

                // Khách yêu cầu. Hủy duyệt thì thì các yêu cầu sau sẽ được cộng thêm số điểm đã trừ
                var conversionPointAfter = _dbContext.LoyConversionPoints.Where(c => c.Id > conversionPointQuery.Id && c.TradingProviderId == tradingProviderId
                                            && c.InvestorId == conversionPointQuery.InvestorId && c.Deleted == YesNo.NO);
                foreach (var item in conversionPointAfter)
                {
                    item.CurrentPoint += totalConversionPoint;
                }
            }

            conversionPointQuery.Status = LoyConversionPointStatus.CANCELED;
            // Lưu lại lịch sử hủy duyệt
            _loyConversionPointStatusLogEFRepository.Add(new LoyConversionPointStatusLog
            {
                ConversionPointId = conversionPointQuery.Id,
                Note = input.Note,
                Status = LoyConversionPointStatus.CANCELED,
                CreatedBy = username,
                Source = LoySource.OFFLINE
            });
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public PagingResult<FindAllLoyConversionPointDto> FindAll(FilterLoyConversionPointDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var query = _loyConversionPointEFRepository.FindAll(dto, tradingProviderId);
            var finalItems = new List<FindAllLoyConversionPointDto>();

            foreach (var item in query.Items)
            {
                item.Phone = StringUtils.HidePhone(item.Phone);
                finalItems.Add(item);
            }

            return new PagingResult<FindAllLoyConversionPointDto>
            {
                TotalItems = query.TotalItems,
                Items = finalItems
            };
        }

        /// <summary>
        /// Xem chi tiết 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LoyConversionPointDto FindById(int id)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var conversionPointQuery = _loyConversionPointEFRepository.FindById(id, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyConversionPointNotFound);

            var result = _mapper.Map<LoyConversionPointDto>(conversionPointQuery);
            result.Details = new();
            var conversionPointDetailQuery = _dbContext.LoyConversionPointDetails.Where(c => c.ConversionPointId == conversionPointQuery.Id && c.Deleted == YesNo.NO);
            foreach (var item in conversionPointDetailQuery)
            {
                result.Details.Add(new LoyConversionPointDetailDto
                {
                    Id = item.Id,
                    VoucherId = item.VoucherId,
                    Point = item.Point,
                    Quantity = item.Quantity,
                    TotalConversionPoint = item.TotalConversionPoint,
                    VoucherInfo = _mapper.Map<ViewVoucherDto>(_dbContext.LoyVouchers.FirstOrDefault(v => v.Id == item.VoucherId)),
                });
            }

            var conversionPointStatusLogQuery = _dbContext.LoyConversionPointStatusLogs.Where(c => c.ConversionPointId == conversionPointQuery.Id && c.Deleted == YesNo.NO)
                .OrderBy(x => x.Id);
            result.StatusLogs = _mapper.Map<List<LoyConversionPointStatusLogDto>>(conversionPointStatusLogQuery);
            return result;
        }

        #region App
        public async Task<LoyConversionPointDto> AppConversionPointVoucher(AppCreateConversionPointDto input)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            _logger.LogInformation($"{nameof(AppConversionPointVoucher)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            // Tìm kiếm voucher
            var voucher = _loyVoucherEFRepository.FindById(input.VoucherId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);

            // Kiểm tra xem nhà đầu tư đã có point chưa
            var investorPoint = _loyPointInvestorEFRepository.Get(investorId, voucher.TradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.LoyPointInvestorNotFound);

            // Nếu Điểm tích lũy dưới 0 thì báo lỗi
            if (investorPoint.CurrentPoint < 0)
            {
                _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyRedemptionPointExceeded);
            }

            var transaction = _dbContext.Database.BeginTransaction();
            var entity = new LoyConversionPoint
            {
                InvestorId = investorId,
                AllocationType = LoyAllocationTypes.KHACH_HANG_DOI_DIEM,
                RequestType = LoyRequestTypes.DOI_VOUCHER,
                CreatedBy = username,
                RequestDate = DateTime.Now,
                TradingProviderId = voucher.TradingProviderId,
                Source = LoySource.ONLINE,
                Description = "Khách hàng yêu cầu đổi voucher",
                CurrentPoint = investorPoint.CurrentPoint
            };
            // Thêm yêu cầu quy đồi
            var conversionPointInsert = _loyConversionPointEFRepository.Add(entity);
            _dbContext.SaveChanges();

            // Tổng số voucher mà nhà đầu tư đã đổi trước đó
            var totalQuantityVoucherOfInvestor = _dbContext.LoyConversionPointDetails.Where(d => d.VoucherId == voucher.Id && d.Deleted == YesNo.NO && _dbContext.LoyConversionPoints.Any(c => c.Id == d.ConversionPointId
                            && c.Status != LoyConversionPointStatus.CANCELED && c.InvestorId == investorId && d.Deleted == YesNo.NO)).Sum(d => d.Quantity);
            if (voucher.ExchangeRoundNum != null && voucher.ExchangeRoundNum < totalQuantityVoucherOfInvestor + input.Quantity)
            {
                // Vượt quá hạn mức quy đổi voucher
                _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyVoucherConversionIsOverLimit);
            }

            // Tính số điểm quy đổi
            var totalConversionPoint = voucher.Point * input.Quantity;
            // Trừ điểm tích lũy
            investorPoint.CurrentPoint -= totalConversionPoint;

            // Nếu Điểm tích lũy dưới 0 thì báo lỗi
            if (investorPoint.CurrentPoint < 0)
            {
                _loyConversionPointEFRepository.ThrowException(ErrorCode.LoyRedemptionPointExceeded);
            }

            // Thêm chi tiết quy đổi
            _loyConversionPointEFRepository.LoyConversionPointDetailAdd(new LoyConversionPointDetail
            {
                ConversionPointId = conversionPointInsert.Id,
                VoucherId = input.VoucherId,
                Quantity = input.Quantity,
                Point = voucher.Point,
                TotalConversionPoint = totalConversionPoint,
                CreatedBy = username
            });

            // Lưu lại lịch sử khi tạo yêu cầu
            _loyConversionPointStatusLogEFRepository.Add(new LoyConversionPointStatusLog
            {
                ConversionPointId = conversionPointInsert.Id,
                Note = conversionPointInsert.Description,
                Status = LoyConversionPointStatus.CREATED,
                CreatedBy = username,
                Source = LoySource.ONLINE
            });

            _dbContext.SaveChanges();
            transaction.Commit();
            await _loyaltyNotificationServices.SendNotificationExchangeRequestAdmin(conversionPointInsert.Id);
            return _mapper.Map<LoyConversionPointDto>(conversionPointInsert);
        }

        /// <summary>
        /// Lấy danh sách Ưu đãi voucher có thể sử dụng
        /// </summary>
        /// <returns></returns>
        public List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherCanUse(int tradingProviderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppInvestorVoucherCanUse)} : investorId = {investorId}");
            var result = _loyConversionPointEFRepository.FindAllVoucherByInvestor(tradingProviderId, investorId, LoyConversionPointStatus.FINISHED, false, null, true);
            return result.ToList();
        }

        /// <summary>
        /// Lấy danh sách Ưu đãi voucher có thể sử dụng Lấy 4 voucher mới nhất
        /// </summary>
        /// <returns></returns>
        public List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherNewCanUse(int tradingProviderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppInvestorVoucherNewCanUse)} : investorId = {investorId}");
            var result = _loyConversionPointEFRepository.FindAllVoucherByInvestor(tradingProviderId, investorId, LoyConversionPointStatus.FINISHED, false, 4, true);
            return result.ToList();
        }

        /// <summary>
        /// Lấy danh sách Giao dịch voucher đã đổi
        /// </summary>
        /// <returns></returns>
        public List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherIsConversionPoint(int tradingProviderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppInvestorVoucherIsConversionPoint)} : investorId = {investorId}");
            var result = _loyConversionPointEFRepository.FindAllVoucherByInvestor(tradingProviderId, investorId, null, false, null, true);
            return result.OrderByDescending(x => x.ConversionPointDetailId).ToList();
        }

        /// <summary>
        /// Lấy danh sách Voucher đã hết hạn sử dụng
        /// </summary>
        /// <returns></returns>
        public List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherIsExpired(int tradingProviderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppInvestorVoucherIsExpired)} : investorId = {investorId}");
            var result = _loyConversionPointEFRepository.FindAllVoucherByInvestor(tradingProviderId, investorId, null, true);
            return result.ToList();
        }

        /// <summary>
        /// Xem chi tiết voucher
        /// </summary>
        /// <param name="conversionPointDetailId"></param>
        /// <returns></returns>
        public AppLoyConversionPointByInvestorInfoDto AppInvestorVoucherInfo(int conversionPointDetailId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppInvestorVoucherInfo)} : investorId = {investorId}");

            var findData = (from conversionPoint in _dbContext.LoyConversionPoints
                            join conversiontPointDetail in _dbContext.LoyConversionPointDetails on conversionPoint.Id equals conversiontPointDetail.ConversionPointId
                            where conversionPoint.Deleted == YesNo.NO && conversiontPointDetail.Deleted == YesNo.NO
                            && conversiontPointDetail.Id == conversionPointDetailId && conversionPoint.InvestorId == investorId
                            select new AppLoyConversionPointByInvestorInfoDto
                            {
                                VoucherId = conversiontPointDetail.VoucherId,
                                TradingProviderShortName = _dbContext.BusinessCustomers.FirstOrDefault(x => x.Deleted == YesNo.NO && _dbContext.TradingProviders.Any(t => t.TradingProviderId == conversionPoint.TradingProviderId
                                                               && t.Deleted == YesNo.NO && x.BusinessCustomerId == t.BusinessCustomerId)).ShortName,
                            }).FirstOrDefault();
            var result = findData;
            if (result != null)
            {
                var voucher = _loyVoucherEFRepository.FindById(result.VoucherId)
                    .ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);
                result = _mapper.Map<AppLoyConversionPointByInvestorInfoDto>(voucher);
                result.VoucherId = voucher.Id;
                result.TradingProviderShortName = findData.TradingProviderShortName;
                result.CustomerName = _dbContext.InvestorIdentifications.FirstOrDefault(i => i.InvestorId == investorId && i.Deleted == YesNo.NO && i.IsDefault == YesNo.YES
                                        && i.Status == Status.ACTIVE)?.Fullname;
            }
            return result;
        }

        /// <summary>
        /// Lịch sử chuyển đổi điểm của nhà đầu tư trong đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        public List<AppHistoryConversionPointDto> AppHistoryConversionPoint(int tradingProviderId)
        {
            var result = new List<AppHistoryConversionPointDto>();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var accumulatePoint = _dbContext.LoyHisAccumulatePoints.Where(a => a.InvestorId == investorId && a.TradingProviderId == tradingProviderId && a.Deleted == YesNo.NO).ToList()
                .Select(a => new AppHistoryConversionPointDto
                {
                    ConversionType = LoyConversionTypes.TICH_DIEM,
                    Date = _dbContext.LoyAccumulatePointStatusLogs.FirstOrDefault(p => p.Deleted == YesNo.NO && p.HisAccumulatePointId == a.Id && p.Status == a.Status)?.CreatedDate,
                    HistoryName = LoyHisAccumulatePointReasons.ConfigReasons.FirstOrDefault(x => x.Value == a.Reason).Label,
                    Point = a.Point,
                    Status = a.Status
                });
            result.AddRange(accumulatePoint);

            var conversionVoucher = (from conversionPoint in _dbContext.LoyConversionPoints
                                     join conversionPointDetail in _dbContext.LoyConversionPointDetails on conversionPoint.Id equals conversionPointDetail.ConversionPointId
                                     join voucher in _dbContext.LoyVouchers on conversionPointDetail.VoucherId equals voucher.Id
                                     where conversionPoint.InvestorId == investorId && conversionPoint.TradingProviderId == tradingProviderId && conversionPoint.Deleted == YesNo.NO
                                     && conversionPointDetail.Deleted == YesNo.NO && voucher.Deleted == YesNo.NO
                                     select new
                                     {
                                         Quantity = conversionPointDetail.Quantity,
                                         HistoryName = voucher.DisplayName,
                                         Point = conversionPointDetail.Point,
                                         Status = conversionPoint.Status,
                                         Date = _dbContext.LoyConversionPointStatusLogs.FirstOrDefault(c => c.Deleted == YesNo.NO && c.ConversionPointId == conversionPoint.Id && conversionPoint.Status == c.Status).CreatedDate
                                     }).ToList()
                                    .SelectMany(p => Enumerable.Repeat(new AppHistoryConversionPointDto
                                    {
                                        ConversionType = LoyConversionTypes.VOUCHER,
                                        HistoryName = p.HistoryName,
                                        Status = p.Status,
                                        Date = p.Date,
                                        Point = p.Point
                                    }, p.Quantity));
            result.AddRange(conversionVoucher);
            return result.OrderByDescending(x => x.Date).ToList();
        }
        #endregion
    }
}
