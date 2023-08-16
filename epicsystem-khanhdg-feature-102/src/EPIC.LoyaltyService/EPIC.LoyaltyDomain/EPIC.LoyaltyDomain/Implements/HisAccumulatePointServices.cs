using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Implements
{
    public class HisAccumulatePointServices : IHisAccumulatePointServices
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

        public HisAccumulatePointServices(EpicSchemaDbContext dbContext,
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

        }

        /// <summary>
        /// Thêm tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        public async Task Add(AddAccumulatePointDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            string usertype = CommonUtils.GetCurrentUsername(_httpContext);

            var tradingProviderId = GetCurrentTradingProviderIdCommon();

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(dto)}, username = {username}");
            var entity = _mapper.Map<LoyHisAccumulatePoint>(dto);
            entity.Source = LoySource.OFFLINE;
            entity.TradingProviderId = tradingProviderId;

            var toDayIsApplyDate = entity.ApplyDate.Value.Date <= DateTime.Now.Date;
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                var inv = _investorEFRepository.FindById(entity.InvestorId).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);

                processAddAccumulatePointCommon(inv, entity, tradingProviderId);

                var his = _loyHisAccumulatePointEFRepository.Add(entity, username);
                _dbContext.SaveChanges();

                // Ghi log
                _loyAccumulatePointStatusLogEFRepository.Add(new LoyAccumulatePointStatusLog
                {
                    Status = his.Status,
                    ExchangedPointStatus = his.ExchangedPointStatus,
                    HisAccumulatePointId = his.Id,
                    Source = LoySource.OFFLINE,
                    Note = "Tạo từ CMS",
                }, username);
                _dbContext.SaveChanges();

                tran.Commit();
            }

            // Gửi thông báo
            if (toDayIsApplyDate && entity.PointType == LoyPointTypes.TICH_DIEM)
            {
                await _loyaltyNotificationServices.SendNotificationAccumulatePointSuccess(entity.Id);
            }
        }

        /// <summary>
        /// Tạo điểm từ file excel
        /// </summary>
        /// <param name="dto"></param>
        public async Task ImportExcel(ImportExceAccumulatePointlDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = GetCurrentTradingProviderIdCommon();

            _logger.LogInformation($"{nameof(Add)} : dto = {JsonSerializer.Serialize(dto)}");

            //byte[] fileBytes;
            //using (var ms = new MemoryStream())
            //{
            //    dto.File.CopyTo(ms);
            //    fileBytes = ms.ToArray();
            //}
            //var fileName = $"{Path.GetTempFileName()}{Path.GetExtension(dto.File.FileName)}";
            //File.WriteAllBytes(fileName, fileBytes);
            using var wb = new XLWorkbook(dto.File.OpenReadStream());

            int colCifcode = 1;
            int colPointType = 2;
            int colReason = 3;
            int colPoint = 4;
            int colApplyDate = 5;

            var ws = wb.Worksheet(1);
            var catRow = ws.FirstRowUsed();

            var row = catRow.RowBelow();

            var source = LoySource.OFFLINE;
            // Danh sách Id gửi thông báo tích điểm
            List<int> accumulatePointIds = new();

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                while (!row.Cell(colCifcode).IsEmpty())
                {
                    var cifCode = StringCheckRequired(row.Cell(colCifcode).Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colCifcode).Address}");
                    var pointType = IntegerTryParse(row.Cell(colPointType)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colPointType).Address}");
                    var point = IntegerTryParse(row.Cell(colPoint)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colPoint).Address}");
                    var reason = IntegerTryParse(row.Cell(colReason)?.Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colReason).Address}");
                    var applyDateString = StringCheckRequired(row.Cell(colApplyDate).Value.ToString(), $"Dữ liệu không hợp lệ ở ô {row.Cell(colApplyDate).Address}");

                    // Kiểm tra khớp định dạng
                    var applyDate = DateTimeUtils.FromStrToDate(applyDateString);
                    if (applyDate == null)
                    {
                        _loyHisAccumulatePointEFRepository.ThrowException($"Dữ liệu không hợp lệ ở ô {row.Cell(colApplyDate).Address}");
                    }
                    var investor = _investorEFRepository.FindByCifCode(cifCode);
                    if (investor != null)
                    {
                        var entity = new LoyHisAccumulatePoint
                        {
                            InvestorId = investor.InvestorId,
                            ApplyDate = applyDate,
                            CurrentPoint = investor.LoyCurrentPoint,
                            Description = "Import excel",
                            PointType = pointType,
                            Point = point,
                            Source = source,
                            Reason = reason,
                            TradingProviderId = tradingProviderId
                        };

                        processAddAccumulatePointCommon(investor, entity, tradingProviderId);

                        var his = _loyHisAccumulatePointEFRepository.Add(entity, username);
                        _dbContext.SaveChanges();

                        // Ghi log
                        _loyAccumulatePointStatusLogEFRepository.Add(new LoyAccumulatePointStatusLog
                        {
                            Status = his.Status,
                            ExchangedPointStatus = his.ExchangedPointStatus,
                            HisAccumulatePointId = his.Id,
                            Source = source,
                            Note = "Tạo từ CMS",
                        }, username);

                        // Nếu là tích điểm thì thêm vào list
                        if (his.PointType == LoyPointTypes.TICH_DIEM)
                        {
                            accumulatePointIds.Add(his.Id);
                        }    
                    }
                    row = row.RowBelow();
                }
                _dbContext.SaveChanges();
                tran.Commit();

                // Gửi thông báo tích điểm thành công
                foreach (var item in accumulatePointIds)
                {
                    await _loyaltyNotificationServices.SendNotificationAccumulatePointSuccess(item);
                }
            }
        }

        /// <summary>
        /// Kiểm tra trường text bắt buộc nhập
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private string StringCheckRequired(string data, string message)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                _loyHisAccumulatePointEFRepository.ThrowException(message);
            }
            return data;
        }

        /// <summary>
        /// Ép kiểu int dữ liệu từ excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private int IntegerTryParse(string data, string message)
        {
            if (!int.TryParse(data, out int result))
            {
                _loyHisAccumulatePointEFRepository.ThrowException(message);
            }
            return result;
        }

        /// <summary>
        /// Hàm chung xử lý logic add lệnh tích điểm/tiêu điểm
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="entity"></param>
        private void processAddAccumulatePointCommon(Investor inv, LoyHisAccumulatePoint entity, int? tradingProviderId)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var toDayIsApplyDate = entity.ApplyDate <= DateTime.Now.Date;

            var invPoint = _loyPointInvestorEFRepository.Get(inv.InvestorId, tradingProviderId);
            var pointNotExist = invPoint == null;

            if (pointNotExist)
            {
                invPoint = new LoyPointInvestor
                {
                    InvestorId = inv.InvestorId,
                    TradingProviderId = tradingProviderId,
                };
            }

            // Nếu ngày áp dụng là hôm nay thì cộng/trừ luôn điểm cho khách đó
            if (toDayIsApplyDate)
            {
                // Lưu lại điểm hiện tại của khách trước khi tạo lệnh tích/tiêu điểm
                entity.CurrentPoint = invPoint.CurrentPoint ?? 0;

                if (entity.PointType == LoyPointTypes.TICH_DIEM)
                {
                    invPoint.TotalPoint = (invPoint.TotalPoint ?? 0) + entity.Point;
                    invPoint.CurrentPoint = (invPoint.CurrentPoint ?? 0) + entity.Point;
                }
                else if (entity.PointType == LoyPointTypes.TIEU_DIEM)
                {
                    // Điểm hiện tại ko đủ để tiêu điểm thì báo lỗi
                    if ((invPoint.CurrentPoint ?? 0) < entity.Point)
                    {
                        _investorEFRepository.ThrowException(ErrorCode.LoyAccumulatePointNotEnough);
                    }
                    invPoint.CurrentPoint = (invPoint.CurrentPoint ?? 0) - entity.Point;
                }

                if (pointNotExist)
                {
                    _loyPointInvestorEFRepository.Add(invPoint, username);
                }

            }

            // Nếu ngày áp dụng là hôm nay hoặc ngày quá khứ thì trạng thái hoàn thành luôn
            // Còn không thì để khởi tạo
            if (toDayIsApplyDate)
            {
                entity.Status = LoyHisAccumulatePointStatus.FINISHED;
                entity.ExchangedPointStatus = LoyExchangePointStatus.FINISHED;
            }
            else
            {
                entity.Status = LoyHisAccumulatePointStatus.CREATED;
                entity.ExchangedPointStatus = LoyExchangePointStatus.CREATED;
            }

            // Cộng trừ điểm dự kiến
            if (entity.PointType == LoyPointTypes.TIEU_DIEM)
            {
                invPoint.TempPoint -= entity.Point;
            }
            else if (entity.PointType == LoyPointTypes.TICH_DIEM)
            {
                invPoint.TempPoint += entity.Point;
            }
        }

        /// <summary>
        /// Hàm chung Lấy đlsc hiện tại
        /// </summary>
        /// <returns></returns>
        private int? GetCurrentTradingProviderIdCommon()
        {
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);

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

        /// <summary>
        /// Cập nhật thông tin tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        public void Update(UpdateAccumulatePointDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(dto)}, username = {username}");
            var entity = _loyHisAccumulatePointEFRepository.FindById(dto.Id).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherHisAccumulatePointNotFound);

            entity.Point = dto.Point;
            entity.PointType = dto.PointType;
            entity.Reason = dto.Reason;
            entity.Description = dto.Description;
            entity.InvestorId = dto.InvestorId;
            entity.ApplyDate = dto.ApplyDate;

            _loyHisAccumulatePointEFRepository.Update(entity, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật trạng thái yêu cầu đổi điểm (Exchanged point status)
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateExchangedPointStatus(UpdateHisAccumulateStatusDto dto, int exchangedPointStatus)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = GetCurrentTradingProviderIdCommon();

            _logger.LogInformation($"{nameof(UpdateExchangedPointStatus)}: input = {JsonSerializer.Serialize(dto)}, username = {username}");
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                var entity = _loyHisAccumulatePointEFRepository.FindById(dto.Id).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherHisAccumulatePointNotFound);

                // LẤy thông tin điểm
                var invPoint = _loyPointInvestorEFRepository.Get(entity.InvestorId, tradingProviderId);
                bool pointNotExist = invPoint == null;
                if (pointNotExist)
                {
                    invPoint = new LoyPointInvestor
                    {
                        InvestorId = entity.InvestorId,
                        TradingProviderId = tradingProviderId,
                    };
                }

                var log = new LoyAccumulatePointStatusLog
                {
                    ExchangedPointStatus = exchangedPointStatus,
                    HisAccumulatePointId = entity.Id,
                    Source = LoySource.OFFLINE,
                    Note = dto.Note,
                };

                // Nếu Tiêu điểm: Khởi tạo => Tiếp nhận thì trừ điểm; Status của lệnh chuyển sang trạng thái Hoàn thành
                if (entity.PointType == LoyPointTypes.TIEU_DIEM && exchangedPointStatus == LoyExchangePointStatus.PENDING && entity.Status == LoyExchangePointStatus.CREATED)
                {
                    var inv = _investorEFRepository.FindById(entity.InvestorId).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);

                    // Điểm hiện tại ko đủ để tiêu điểm thì báo lỗi
                    if ((invPoint.CurrentPoint ?? 0) < entity.Point)
                    {
                        _investorEFRepository.ThrowException(ErrorCode.LoyAccumulatePointNotEnough);
                    }

                    invPoint.CurrentPoint = (invPoint.CurrentPoint ?? 0) - entity.Point;

                    // Nếu có chọn ưu đãi thì gán voucher cho investor
                    if (dto.VoucherId != null)
                    {
                        _loyVoucherEFRepository.FindById(dto.VoucherId ?? 0).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);
                        _loyVoucherInvestorEFRepository.Add(new LoyVoucherInvestor
                        {
                            HisAccumulatePointId = entity.Id,
                            InvestorId = inv.InvestorId,
                            VoucherId = dto.VoucherId,
                        }, username);
                    }

                    // Chuyển Trạng thái lệnh sang hoàn thành
                    entity.Status = LoyHisAccumulatePointStatus.FINISHED;
                    log.Status = entity.Status;

                    // Lưu điểm nếu chưa có
                    if (pointNotExist)
                    {
                        _loyPointInvestorEFRepository.Add(invPoint, username);
                    }
                }
                // Nếu Tiêu điểm: Tiếp nhận, Đang giao, Đã nhận, Hoàn thành => Hủy yêu cầu thì cộng lại điểm
                else if (entity.PointType == LoyPointTypes.TIEU_DIEM && exchangedPointStatus == LoyExchangePointStatus.CANCELED && new int[] { LoyExchangePointStatus.PENDING, LoyExchangePointStatus.DELIVERY, LoyExchangePointStatus.FINISHED }.Contains(entity.Status ?? 0))
                {
                    invPoint.CurrentPoint = (invPoint.CurrentPoint ?? 0) + entity.Point;
                    invPoint.TempPoint = (invPoint.TempPoint ?? 0) + entity.Point;

                    // Chuyển trạng thái lệnh sang hủy
                    entity.Status = LoyHisAccumulatePointStatus.CANCELED;
                    log.Status = entity.Status;

                    // Lưu điểm nếu chưa có
                    if (pointNotExist)
                    {
                        _loyPointInvestorEFRepository.Add(invPoint, username);
                    }
                }

                // Cập nhật trạng thái
                entity.ExchangedPointStatus = exchangedPointStatus;

                // Ghi log
                _loyAccumulatePointStatusLogEFRepository.Add(log, username);

                _dbContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Hủy lệnh tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        public void Cancel(UpdateStatusCancelDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = GetCurrentTradingProviderIdCommon();

            _logger.LogInformation($"{nameof(UpdateExchangedPointStatus)}: input = {JsonSerializer.Serialize(dto)}, username = {username}");
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                var entity = _loyHisAccumulatePointEFRepository.FindById(dto.Id).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherHisAccumulatePointNotFound);
                // LẤy thông tin điểm
                var invPoint = _loyPointInvestorEFRepository.Get(entity.InvestorId, tradingProviderId);
                bool pointNotExist = invPoint == null;
                if (pointNotExist)
                {
                    invPoint = new LoyPointInvestor
                    {
                        InvestorId = entity.InvestorId,
                        TradingProviderId = tradingProviderId,
                    };
                }

                int? exchangedPointStatus = null;
                exchangedPointStatus = LoyExchangePointStatus.CANCELED;

                // Ghi log
                _loyAccumulatePointStatusLogEFRepository.Add(new LoyAccumulatePointStatusLog
                {
                    Status = LoyHisAccumulatePointStatus.CANCELED,
                    ExchangedPointStatus = exchangedPointStatus,
                    HisAccumulatePointId = entity.Id,
                    Source = LoySource.OFFLINE,
                    Note = "Hủy yêu cầu từ Quản lý tích điểm (CMS)",
                }, username);

                // Nếu Tích điểm: Hoàn thành => Hủy yêu cầu thì trừ lại điểm
                if (entity.PointType == LoyPointTypes.TICH_DIEM && entity.Status == LoyHisAccumulatePointStatus.FINISHED)
                {
                    invPoint.CurrentPoint = (invPoint.CurrentPoint ?? 0) - entity.Point;
                    invPoint.TotalPoint = (invPoint.TotalPoint ?? 0) - entity.Point;
                    invPoint.TempPoint = (invPoint.TempPoint ?? 0) - entity.Point;
                }
                // Nếu tiêu điểm: Hoàn thành => Hủy yêu cầu thì cộng lại điểm
                else if (entity.PointType == LoyPointTypes.TIEU_DIEM && entity.Status == LoyHisAccumulatePointStatus.FINISHED)
                {
                    invPoint.CurrentPoint = (invPoint.CurrentPoint ?? 0) + entity.Point;
                    invPoint.TempPoint = (invPoint.TempPoint ?? 0) + entity.Point;
                }

                // Cập nhật trạng thái
                entity.Status = LoyHisAccumulatePointStatus.CANCELED;
                entity.ExchangedPointStatus = exchangedPointStatus;

                _dbContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Xoá tích điểm/tiêu điểm
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");

            _loyHisAccumulatePointEFRepository.Delete(id);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm phân trang tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindAllHisAccumulatePointDto> FindAll(FindHisAccumulatePointDto dto)
        {
            var tradingProviderId = GetCurrentTradingProviderIdCommon();
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(dto)}; tradingProviderId={tradingProviderId}");

            var query = _loyHisAccumulatePointEFRepository.FindAll(dto, tradingProviderId);
            var finalItems = new List<ViewFindAllHisAccumulatePointDto>();

            if (query.Items != null)
            {
                foreach (var item in query.Items)
                {
                    item.Phone = StringUtils.HidePhone(item.Phone);

                    finalItems.Add(item);
                }
            }

            return new PagingResult<ViewFindAllHisAccumulatePointDto>
            {
                TotalItems = query.TotalItems,
                Items = finalItems
            };
        }

        /// <summary>
        /// Tìm kiếm yêu cầu đổi điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindAllHisAccumulatePointDto> FindAll(FindRequestConsumePointDto dto)
        {
            var tradingProviderId = GetCurrentTradingProviderIdCommon();
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(dto)}; tradingProviderId={tradingProviderId}");

            var query = _loyHisAccumulatePointEFRepository.FindAllRequestConsumePoint(dto, tradingProviderId);
            var finalItems = new List<ViewFindAllHisAccumulatePointDto>();

            if (query.Items != null)
            {
                foreach (var item in query.Items)
                {
                    item.Phone = StringUtils.HidePhone(item.Phone);
                    finalItems.Add(item);
                }
            }

            return new PagingResult<ViewFindAllHisAccumulatePointDto>
            {
                TotalItems = query.TotalItems,
                Items = finalItems
            };
        }

        /// <summary>
        /// Tìm kiếm theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindAllHisAccumulatePointDto> FindByInvestorId(int investorId, FindAccumulatePointByInvestorId dto)
        {
            var tradingProviderId = GetCurrentTradingProviderIdCommon();
            _logger.LogInformation($"{nameof(FindByInvestorId)}: dto = {JsonSerializer.Serialize(dto)}; investorId={investorId}; tradingProviderId={tradingProviderId}");

            var data = _loyHisAccumulatePointEFRepository.FindByInvestorIdPaging(investorId, dto, tradingProviderId);
            return data;
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewHisAccumulatePointDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");

            var data = _loyHisAccumulatePointEFRepository.FindById(id);
            var result = _mapper.Map<ViewHisAccumulatePointDto>(data);
            if (result != null)
            {
                var investor = _investorEFRepository.FindById(data.InvestorId);
                var iden = _investorEFRepository.GetDefaultIdentification(investor.InvestorId);

                // Thông tin investor
                if (investor != null)
                {
                    result.Investor = new ViewHisAccumulatePointInvestorDto
                    {
                        InvestorId = investor.InvestorId,
                        Email = investor.Email,
                        Phone = investor.Phone,
                        LoyTotalPoint = investor.LoyTotalPoint,
                        LoyCurrentPoint = investor.LoyCurrentPoint
                    };

                    if (iden != null)
                    {
                        result.Investor.Fullname = iden.Fullname;
                        result.Investor.IdNo = iden.IdNo;
                    }
                }

                // Chi tiết tiến trình
                var log = _loyAccumulatePointStatusLogEFRepository.FindByAccumulatePointId(result.Id);
                result.Logs = _mapper.Map<List<ViewHisAccumulatePointStatusLogDto>>(log);

                // Lấy voucher
                var voucher = _loyVoucherEFRepository.FindByHisAccumulatePointId(id);
                result.Voucher = _mapper.Map<ViewVoucherDto>(voucher);
            }

            return result;
        }

        /// <summary>
        /// Lấy list lý do tích điểm/tiêu điểm
        /// </summary>
        /// <param name="pointType"></param>
        /// <returns></returns>
        public List<ViewListReasonsDto> GetAccumulateReason(int? pointType)
        {
            _logger.LogInformation($"{nameof(GetAccumulateReason)}: pointType = {pointType}");

            return LoyHisAccumulatePointReasons.ConfigReasons.Where(x => (pointType == null) || (x.Type == null) || (x.Type == pointType)).ToList();
        }

        /// <summary>
        /// Get list voucher đang kích hoạt và chưa cấp phát cho ai
        /// </summary>
        /// <returns></returns>
        public List<ViewListVoucherDto> FindFreeVoucher()
        {
            _logger.LogInformation($"{nameof(FindFreeVoucher)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);

            int? tradingProviderId = -1;
            int? partnerId = -1;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (new string[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                tradingProviderId = null;
                partnerId = null;
            }

            var data = _loyVoucherEFRepository.FindFreeVoucher(tradingProviderId, partnerId);
            return _mapper.Map<List<ViewListVoucherDto>>(data);
        }

        #region app
        /// <summary>
        /// App tạo yêu cầu đổi điểm
        /// </summary>
        /// <param name="dto"></param>
        public void AppConsumePoint(AppConsumePointDto dto)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            _logger.LogInformation($"{nameof(AppConsumePoint)}: input = {JsonSerializer.Serialize(dto)}, username = {username}");

            var iden = _investorEFRepository.GetDefaultIdentification(investorId);

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                var inv = _investorEFRepository.FindById(investorId).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
                var invPoint = _loyPointInvestorEFRepository.Get(investorId, dto.TradingProviderId);
                string note = $"{iden.Fullname} ({inv.Phone}) yêu cầu đổi điểm từ app";

                // Check điểm có đủ để tạo yêu cầu ko
                if ((invPoint.TempPoint ?? 0) < dto.Point)
                {
                    _investorEFRepository.ThrowException(ErrorCode.LoyAccumulatePointNotEnough);
                }

                // Lưu yêu cầu đổi điểm
                var his = _loyHisAccumulatePointEFRepository.Add(new LoyHisAccumulatePoint
                {
                    ApplyDate = DateTime.Now,
                    InvestorId = investorId,
                    Point = dto.Point,
                    PointType = LoyPointTypes.TIEU_DIEM,
                    Reason = LoyHisAccumulatePointReasons.DOI_VOUCHER,
                    Status = LoyHisAccumulatePointStatus.CREATED,
                    ExchangedPointStatus = LoyExchangePointStatus.CREATED,
                    Description = note,
                    CurrentPoint = invPoint?.CurrentPoint,
                    TradingProviderId = dto.TradingProviderId,
                    Source = LoySource.ONLINE,
                }, username);
                _dbContext.SaveChanges();

                invPoint.TempPoint -= dto.Point;

                // Ghi log
                _loyAccumulatePointStatusLogEFRepository.Add(new LoyAccumulatePointStatusLog
                {
                    Status = LoyHisAccumulatePointStatus.CREATED,
                    ExchangedPointStatus = LoyExchangePointStatus.CREATED,
                    HisAccumulatePointId = his.Id,
                    Source = LoySource.ONLINE,
                    Note = note,
                }, username);

                _dbContext.SaveChanges();
                tran.Commit();
            }

        }

        /// <summary>
        /// App lấy lịch sử điểm thưởng
        /// </summary>
        /// <returns></returns>
        public List<AppViewAccumulatePointHistoryDto> AppGetAccumulatePointHistory(int? tradingProviderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppGetAccumulatePointHistory)} investorId={investorId}");

            var listHis = _loyHisAccumulatePointEFRepository.FindByInvestorId(investorId, tradingProviderId);
            var result = _mapper.Map<List<AppViewAccumulatePointHistoryDto>>(listHis);
            foreach (var item in result)
            {
                if (item.PointType == LoyPointTypes.TIEU_DIEM)
                {
                    item.ReasonName = "Đổi voucher";
                }
                else
                {
                    var reason = LoyHisAccumulatePointReasons.ConfigReasons.FirstOrDefault(x => x.Value == item.Reason);
                    item.ReasonName = reason?.Label;
                }
            }

            return result;
        }
        #endregion

    }
}
