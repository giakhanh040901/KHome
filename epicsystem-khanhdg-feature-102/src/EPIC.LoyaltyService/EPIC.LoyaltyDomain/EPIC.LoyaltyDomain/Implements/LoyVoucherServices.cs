using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Implements
{
    public class LoyVoucherServices : ILoyVoucherServices
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

        public LoyVoucherServices(EpicSchemaDbContext dbContext,
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
        }


        /// <summary>
        /// Thêm mới voucher
        /// </summary>
        /// <param name="dto"></param>
        public async Task<ViewVoucherDto> Add(AddVoucherDto dto)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (new string[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }

            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(dto)}, username={username}, tradingProviderId={tradingProviderId}, partnerId={partnerId}");

            // Ngày có hiệu lực phải nhỏ hơn ngày hết hạn
            if (dto.StartDate > dto.EndDate)
            {
                _loyVoucherEFRepository.ThrowException(ErrorCode.LoyVoucherStartDateLargerThanExpiredDate);
            }

            // Ngày kết thúc phải nhỏ hơn hoặc bằng ngày hết hạn
            if (dto.EndDate.HasValue && dto.EndDate > dto.ExpiredDate)
            {
                _loyVoucherEFRepository.ThrowException(ErrorCode.LoyVoucherEndDateMustLessThanExpiredDate);
            }

            // Nếu ngày kết thúc ko có giá trị thì gán theo ngày hết hạn
            if (!dto.EndDate.HasValue)
            {
                dto.EndDate = dto.ExpiredDate;
            }

            var listNotificationIds = new List<int>();

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                var mappedVoucher = _mapper.Map<LoyVoucher>(dto);

                mappedVoucher.TradingProviderId = tradingProviderId;
                mappedVoucher.PartnerId = partnerId;

                var voucher = _loyVoucherEFRepository.Add(mappedVoucher, username);
                _dbContext.SaveChanges();

                tran.Commit();

                // Gửi thông báo thành công
                //foreach (var item in listNotificationIds)
                //{
                //    await _loyaltyNotificationServices.SendNotificationAddVoucherToInvestor(item);
                //}

                return _mapper.Map<ViewVoucherDto>(voucher);
            }
        }

        /// <summary>
        /// Cập nhật voucher
        /// </summary>
        /// <param name="dto"></param>
        public async Task Update(UpdateVoucherDto dto)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var listNotificationIds = new List<int>();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(dto)}, username={username}");
                _loyVoucherEFRepository.FindById(dto.VoucherId).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);

                // Cập nhật voucher
                _loyVoucherEFRepository.Update(_mapper.Map<LoyVoucher>(dto), username);

                if (dto.ListInvestor != null)
                {
                    // Cập nhật voucher investor
                    var listVoucherInvestor = _loyVoucherInvestorEFRepository.FindInvestorVoucherByVoucherId(dto.VoucherId);

                    // Xóa voucher investor
                    foreach (var item in listVoucherInvestor)
                    {
                        if (!dto.ListInvestor.Any(x => x.VoucherInvestorId == item.Id))
                        {
                            _loyVoucherInvestorEFRepository.DeletedById(item.Id);
                        }
                    }

                    // Thêm mới voucher investor
                    foreach (var item in dto.ListInvestor)
                    {
                        if (item.VoucherInvestorId == 0)
                        {
                            var result = _loyVoucherInvestorEFRepository.Add(new LoyVoucherInvestor
                            {
                                InvestorId = item.InvestorId,
                                VoucherId = dto.VoucherId,
                                CreatedBy = username,
                            }, username);
                            listNotificationIds.Add(result.Id);
                        }
                    }
                }

                _dbContext.SaveChanges();
                tran.Commit();

                // Gửi thông báo thành công
                foreach (var item in listNotificationIds)
                {
                    await _loyaltyNotificationServices.SendNotificationAddVoucherToInvestor(item);
                }
            }
        }

        /// <summary>
        /// Tìm kiếm voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewListVoucherDto> FindAll(FindAllVoucherDto dto)
        {
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = -1;
            int? parnerId = -1;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                parnerId = null;
            }
            if (new string[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                parnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                tradingProviderId = null;
            }
            if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                tradingProviderId = null;
                parnerId = null;
            }

            _logger.LogInformation($"{nameof(FindAll)} : dto = {JsonSerializer.Serialize(dto)}, usertype={usertype}, tradingProviderId={tradingProviderId}, parnerId={parnerId}");

            return _loyVoucherEFRepository.FindAll(dto, tradingProviderId, parnerId);
        }

        /// <summary>
        /// Lịch sử cấp phát
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<VoucherConversionHistoryDto> FindAllVoucherConversionHistory(FilterVoucherConversionHistoryDto dto)
        {
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = -1;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            _logger.LogInformation($"{nameof(FindAll)} : dto = {JsonSerializer.Serialize(dto)}, usertype={usertype}, tradingProviderId={tradingProviderId}");

            return _loyVoucherEFRepository.FindAllVoucherConversionHistory(dto, tradingProviderId);
        }

        /// <summary>
        /// Lấy danh sách voucher để đổi điểm Tab yêu cầu đổi điểm
        /// </summary>
        /// <returns></returns>
        public List<ViewListVoucherDto> GetAllVoucherForConversionPoint(string keyword)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var voucherQuery = from voucher in _dbContext.LoyVouchers.Where(v => v.TradingProviderId == tradingProviderId && v.Deleted == YesNo.NO && v.EndDate >= DateTime.Now && v.Status == LoyVoucherStatus.KICH_HOAT)
                                   // Tính tổng số số lượng chuyển đổi của voucher
                               let totalQuantityVoucherConversionPoint = (from conversionPoint in _dbContext.LoyConversionPoints
                                                                          join conversionPointDetail in _dbContext.LoyConversionPointDetails on conversionPoint.Id equals conversionPointDetail.ConversionPointId
                                                                          where conversionPoint.TradingProviderId == voucher.TradingProviderId && conversionPoint.Deleted == YesNo.NO
                                                                          && conversionPoint.Status != LoyConversionPointStatus.CANCELED && conversionPointDetail.Deleted == YesNo.NO
                                                                          && conversionPointDetail.VoucherId == voucher.Id
                                                                          select conversionPointDetail.Quantity).Sum()
                               where voucher.PublishNum > totalQuantityVoucherConversionPoint
                               && (keyword == null || voucher.Name.ToLower().Contains(keyword.ToLower()))
                               select voucher;
            return _mapper.Map<List<ViewListVoucherDto>>(voucherQuery);
        }
        /// <summary>
        /// Tạo voucher từ list excel
        /// </summary>
        /// <param name="dto"></param>
        public async Task ImportExcelVoucher(ImportExcelVoucherDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (new string[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }

            _logger.LogInformation($"{nameof(Add)} : dto = {JsonSerializer.Serialize(dto)}, username={username}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            //byte[] fileBytes;
            //using (var ms = new MemoryStream())
            //{
            //    dto.File.CopyTo(ms);
            //    fileBytes = ms.ToArray();
            //}
            //var fileName = $"{Path.GetTempFileName()}{Path.GetExtension(dto.File.FileName)}";
            //File.WriteAllBytes(fileName, fileBytes);
            using var wb = new XLWorkbook(dto.File.OpenReadStream());

            int colVoucherType = 2;
            int colVoucherName = 3;
            int colLinkVoucher = 4;
            int colPoint = 5;
            int colValue = 6;
            int colUsername = 7;
            int colStartDate = 8;
            int colEndDate = 9;
            int colDescriptionContent = 10;

            var rows = wb.Worksheet(1).RowsUsed().Skip(1); // Skip header row

            int i = 0; // index của voucher
            int j = 0; // index của từng dòng excel
            var dictVoucher = new Dictionary<int, LoyVoucher> { };
            var dictUser = new Dictionary<int, int> { };
            var listCheck = new List<LoyVoucher>();

            var listNotificationIds = new List<int>();

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                // Thêm vào voucher
                foreach (var row in rows)
                {
                    var voucher = new LoyVoucher
                    {
                        Name = row.Cell(colVoucherName)?.Value.ToString().Trim(),
                        VoucherType = row.Cell(colVoucherType)?.Value.ToString().Trim(),
                        LinkVoucher = row.Cell(colLinkVoucher)?.Value.ToString(),
                        Point = Convert.ToInt32(row.Cell(colPoint)?.Value.ToString()),
                        Value = Convert.ToInt32(row.Cell(colValue)?.Value.ToString()),
                        DescriptionContentType = ContentTypes.MARKDOWN,
                        DescriptionContent = row.Cell(colDescriptionContent)?.Value.ToString(),
                        StartDate = DateTimeUtils.FromStrToDate(row.Cell(colStartDate)?.Value.ToString()) ?? DateTime.Now,
                        EndDate = DateTimeUtils.FromStrToDate(row.Cell(colEndDate)?.Value.ToString()),
                        PartnerId = partnerId,
                        TradingProviderId = tradingProviderId,
                    };

                    var newVoucher = _loyVoucherEFRepository.Add(voucher, username);
                    dictVoucher.Add(i, newVoucher);
                    listCheck.Add(voucher);
                    dictUser.Add(j, i);

                    i++;
                    j++;
                }
                _dbContext.SaveChanges();

                // Thêm vào voucher investor
                j = 0;
                foreach (var row in rows)
                {
                    var user = row.Cell(colUsername).Value.ToString();
                    var voucherIndex = dictUser[j];
                    var voucher = dictVoucher[voucherIndex];

                    var investor = _investorEFRepository.GetByUsernameSkipStatus(user, tradingProviderId);

                    if (investor != null)
                    {
                        var voucherInvestor = new LoyVoucherInvestor
                        {
                            VoucherId = voucher.Id,
                            InvestorId = investor.InvestorId,
                        };
                        _loyVoucherInvestorEFRepository.Add(voucherInvestor, username);
                        listNotificationIds.Add(voucherInvestor.Id);
                    }

                    j++;
                }
                _dbContext.SaveChanges();
                tran.Commit();

                // Gửi thông báo thành công
                foreach (var item in listNotificationIds)
                {
                    await _loyaltyNotificationServices.SendNotificationAddVoucherToInvestor(item);
                }
            }

        }

        /// <summary>
        /// Update trạng thái voucher cho khách
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateStatus(UpdateVoucherStatusDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateStatus)} : dto = {JsonSerializer.Serialize(dto)}, username={username}");

            _loyVoucherEFRepository.UpdateStatus(dto, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update nổi bật
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateIsHot(UpdateIsHotDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateIsHot)} : dto = {JsonSerializer.Serialize(dto)}, username={username}");

            var voucher = _loyVoucherEFRepository.FindById(dto.Id);
            voucher.IsHot = dto.IsHot;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update show app
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateIsShowApp(UpdateShowAppDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateIsShowApp)} : dto = {JsonSerializer.Serialize(dto)}, username={username}");

            var voucher = _loyVoucherEFRepository.FindById(dto.Id);
            voucher.IsShowApp = dto.IsShowApp;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update dùng trong vòng quay may mắn
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateIsUseInLuckyDraw(UpdateIsUseInLuckyDrawDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateIsUseInLuckyDraw)} : dto = {JsonSerializer.Serialize(dto)}, username={username}");

            var voucher = _loyVoucherEFRepository.FindById(dto.Id);
            voucher.IsUseInLuckyDraw = dto.IsUseInLuckyDraw;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Giao voucher cho khách
        /// </summary>
        /// <param name="dto"></param>
        public async Task ApplyVoucherToInvestor(ApplyVoucherToInvestorDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(ApplyVoucherToInvestor)} : dto = {JsonSerializer.Serialize(dto)}, username={username}");
            _loyVoucherEFRepository.FindById(dto.Id).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);

            var listNotificationIds = new List<int>();

            foreach (var investorId in dto.ListInvestorId)
            {
                _investorEFRepository.FindById(investorId).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);

                var result = _loyVoucherInvestorEFRepository.Add(new LoyVoucherInvestor
                {
                    VoucherId = dto.Id,
                    InvestorId = investorId,
                }, username);

                listNotificationIds.Add(result.Id);
            }

            _dbContext.SaveChanges();

            // Gửi thông báo thành công
            foreach (var item in listNotificationIds)
            {
                await _loyaltyNotificationServices.SendNotificationAddVoucherToInvestor(item);
            }
        }

        /// <summary>
        /// Tìm kiếm danh sách investor kèm thêm thông tin voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewInvestorVoucherDto> FindAllInvestorVoucher(FindAllInvestorForVoucherDto dto)
        {
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = -1;
            int? parnerId = -1;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                parnerId = null;
            }
            if (new string[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                parnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                tradingProviderId = null;
            }
            if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                tradingProviderId = null;
                parnerId = null;
            }

            _logger.LogInformation($"{nameof(FindAllInvestorVoucher)} : dto = {JsonSerializer.Serialize(dto)}, usertype={usertype}");
            return _loyVoucherInvestorEFRepository.FindInvestorVoucherPaging(dto, tradingProviderId, parnerId);
        }

        /// <summary>
        /// Xuất excel theo kq tìm kiếm khcn
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ExportResultDto ExportExcelInvestorVoucher(FindAllInvestorForVoucherDto dto)
        {
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = -1;
            int? parnerId = -1;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                parnerId = null;
            }
            if (new string[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                parnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                tradingProviderId = null;
            }
            if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                tradingProviderId = null;
                parnerId = null;
            }

            _logger.LogInformation($"{nameof(ExportExcelInvestorVoucher)} : dto = {JsonSerializer.Serialize(dto)}, usertype={usertype}");

            dto.PageSize = -1;
            var data = _loyVoucherInvestorEFRepository.FindInvestorVoucherPaging(dto, tradingProviderId, parnerId);

            var result = new ExportResultDto();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Quan_ly_khcn");
            var currentRow = 1;
            var stt = 0;

            int colId = 1;
            int colFullname = 2;
            int colPhone = 3;
            int colSex = 4;
            int colTotalPoint = 5;
            int colCurrentPoint = 6;
            int colRankName = 7;
            int colVoucherNum = 8;

            ws.Cell(currentRow, colId).Value = "ID";
            ws.Cell(currentRow, colFullname).Value = "Tên khách hàng";
            ws.Cell(currentRow, colPhone).Value = "Số điện thoại";
            ws.Cell(currentRow, colSex).Value = "Giới tính";
            ws.Cell(currentRow, colTotalPoint).Value = "Điểm tích lũy";
            ws.Cell(currentRow, colCurrentPoint).Value = "Hiện tại";
            ws.Cell(currentRow, colRankName).Value = "Hạng";
            ws.Cell(currentRow, colVoucherNum).Value = "Số voucher";

            if (data.TotalItems > 0)
            {
                foreach (var row in data.Items)
                {
                    currentRow++;

                    ws.Cell(currentRow, colId).Value = row.InvestorId;
                    ws.Cell(currentRow, colFullname).Value = row.Fullname;
                    ws.Cell(currentRow, colPhone).Value = row.Phone;
                    ws.Cell(currentRow, colSex).Value = row.Sex;
                    ws.Cell(currentRow, colTotalPoint).Value = row.LoyTotalPoint;
                    ws.Cell(currentRow, colCurrentPoint).Value = row.LoyCurrentPoint;
                    ws.Cell(currentRow, colRankName).Value = row.RankName;
                    ws.Cell(currentRow, colVoucherNum).Value = row.VoucherNum;
                }
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                result.fileData = stream.ToArray();
                return result;
            }

        }

        /// <summary>
        /// Lấy voucher theo id
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public ViewVoucherDto FindById(int voucherId)
        {
            var data = _loyVoucherEFRepository.FindById(voucherId).ThrowIfNull(_dbContext, ErrorCode.LoyVoucherNotFound);
            var voucher = _mapper.Map<ViewVoucherDto>(data);

            return voucher;
        }

        /// <summary>
        /// Lấy voucher theo investorid
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewVoucherByInvestorDto> FindByInvestorPaging(FindVoucherByInvestorIdDto dto)
        {
            _logger.LogInformation($"{nameof(FindByInvestorPaging)} : dto = {JsonSerializer.Serialize(dto)}");
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);

            int? tradingProviderId = -1;
            int? parnerId = -1;

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                parnerId = null;
            }
            if (new string[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                parnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                tradingProviderId = null;
            }
            if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                tradingProviderId = null;
                parnerId = null;
            }
            return _loyVoucherInvestorEFRepository.FindByInvestorIdPaging(dto, tradingProviderId, parnerId);
        }

        /// <summary>
        /// Xoá mềm voucher theo voucher id  (UPDATE TRẠNG THÁI)
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(int id)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteById)} : id = {id}, username={username}");

            _loyVoucherEFRepository.DeleteById(id, username);
            _dbContext.SaveChanges();
        }

        #region App
        /// <summary>
        /// App lấy voucher theo investor
        /// </summary>
        /// <returns></returns>
        public List<AppViewVoucherByInvestorDto> AppFindByInvestor(int? tradingProviderId, string useType)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppFindByInvestor)} : investorId = {investorId}, tradingProviderId = {tradingProviderId}, useType = {useType}");
            return _loyVoucherInvestorEFRepository.AppFindByInvestorId(investorId, tradingProviderId, useType);
        }

        /// <summary>
        /// Lấy danh sách 6 voucher nổi bật của đại lý
        /// </summary>
        public List<AppViewVoucherByInvestorDto> AppFindVoucherIsHot(int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(AppFindVoucherIsHot)}: tradingProviderId = {tradingProviderId}");
            return _loyVoucherEFRepository.AppFindVoucherIsHot(tradingProviderId);
        }

        /// <summary>
        /// Các loại hình voucher của đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<string> AppGetVoucherUseTypeOfTrading(int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(AppGetVoucherUseTypeOfTrading)} : tradingProviderId = {tradingProviderId}");
            var query = _dbContext.LoyVouchers.Where(x => x.Deleted == YesNo.NO && x.TradingProviderId == tradingProviderId
                && x.Status == LoyVoucherStatus.KICH_HOAT && x.StartDate <= DateTime.Now && (x.ExpiredDate == null || DateTime.Now <= x.ExpiredDate))
                .Select(x => x.UseType).Distinct().ToList();
            return query;
        }

        /// <summary>
        /// App lấy voucher hết hạn theo investor
        /// </summary>
        /// <returns></returns>
        public List<AppViewVoucherExpiredByInvestorDto> AppFindExpiredByInvestor()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(AppFindExpiredByInvestor)} : investorId = {JsonSerializer.Serialize(investorId)}");

            return _mapper.Map<List<AppViewVoucherExpiredByInvestorDto>>(_loyVoucherInvestorEFRepository.AppFindExpiredByInvestorId(investorId));
        }
        #endregion
    }
}
