using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.SaleInvestor;
using EPIC.FileDomain.Services;
using EPIC.IdentityRepositories;
using EPIC.ImageAPI.Models;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Recognition.FPT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class SaleInvestorServices : ISaleInvestorServices
    {
        private readonly ILogger<SaleInvestorServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IFileServices _fileServices;
        private readonly IInvestorServices _investorServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly OCRUtils _ocr;
        private readonly IOptions<RecognitionApiConfiguration> _recognitionApiConfig;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly SaleInvestorRepository _saleInvestorRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly SaleRepository _saleRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly UserRepository _userRepository;


        public SaleInvestorServices(ILogger<SaleInvestorServices> logger, DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext,
            IHttpContextAccessor httpContext, IMapper mapper,
            IOptions<RecognitionApiConfiguration> recognitionApiConfiguration,
            NotificationServices sendEmailServices,
            IFileServices fileServices, IInvestorServices investorServices)
        {
            _logger = logger;
            _httpContext = httpContext;
            _mapper = mapper;
            _fileServices = fileServices;
            _investorServices = investorServices;
            _sendEmailServices = sendEmailServices;
            _recognitionApiConfig = recognitionApiConfiguration;
            _dbContext = dbContext;
            _ocr = new OCRUtils(_recognitionApiConfig.Value, _logger);
            _connectionString = databaseOptions.ConnectionString;
            _saleInvestorRepository = new SaleInvestorRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Tạo investor
        /// </summary>
        /// <param name="dto"></param>
        public ResultAddInvestorDto RegisterInvestor(SaleRegisterInvestorDto dto)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            string password = CommonUtils.CreateMD5("123456Aa@");

            return _saleInvestorRepository.RegisterInvestor(dto, investorId, password, username);
        }

        /// <summary>
        /// Ekyc giấy tờ tuỳ thân
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EKYCOcrResultDto> UpdateEkycIdentification(EkycSaleInvestorDto input)
        {
            EKYCOcrResultDto resultOcr = new();

            string frontImageUrl = "";
            string backImageUrl = "";

            bool isSuccess = false;

            if (input.Type == CardTypesInput.CCCD || input.Type == CardTypesInput.CMND)
            {
                //front id
                OCRFrontIdDataNewType frontData = await _ocr.ReadFrontIdDataNewType(input.FrontImage);

                frontImageUrl = _fileServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                //back id
                OCRBackIdDataNewType backData = await _ocr.ReadBackIdDataNewType(input.BackImage);
                _ocr.CheckDifferenceImage(frontData, backData);
                var issueDate = _ocr.ConvertStringIssudeDateToDateTime(backData.IssueDate);

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(frontData.Id),
                    Name = RecognitionUtils.GetValueStandard(frontData.Name),
                    Sex = OCRGenders.ConvertStandard(RecognitionUtils.GetValueStandard(frontData.Sex)),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(frontData.Dob),
                    IdIssueDate = issueDate,
                    IdIssueExpDate = _ocr.ProccessExpDate(frontData.Doe, issueDate, input.Type.ToUpper()),
                    IdIssuer = RecognitionUtils.GetValueStandard(backData.IssueLoc),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(frontData.Home),
                    PlaceOfResidence = RecognitionUtils.GetValueStandard(frontData.Address),
                    Nationality = RecognitionUtils.GetValueStandard(frontData.Nationality)
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp((input.Type == CardTypesInput.CMND) ? resultOcr.IdIssueDate?.AddYears(15) : resultOcr.IdIssueExpDate);
                if (input.Type == CardTypesInput.CMND)
                {
                    resultOcr.IdIssueExpDate = resultOcr.IdIssueExpDate ?? resultOcr.IdIssueDate?.AddYears(15);
                }

                backImageUrl = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.BackImage,
                    Folder = FileFolder.INVESTOR,
                });

                isSuccess = true;
            }
            else if (input.Type == CardTypesInput.PASSPORT)
            {
                OCRDataPassport passportData = await _ocr.ReadPassport(input.FrontImage);

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    Name = RecognitionUtils.GetValueStandard(passportData.Name),
                    Sex = OCRGenders.ConvertStandard(passportData.Sex),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Dob),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(passportData.Pob),
                    PassportIdNumber = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    IdIssueDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doi),
                    IdIssueExpDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doe),
                    Nationality = "",
                    IdIssuer = passportData.IdIssuer,
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp(resultOcr.IdIssueExpDate);

                frontImageUrl = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                isSuccess = true;
            }

            if (isSuccess)
            {
                var dto = new CreateIdentificationTemporaryDto
                {
                    IdFrontImageUrl = frontImageUrl,
                    Fullname = resultOcr.Name,
                    IdBackImageUrl = backImageUrl,
                    IdNo = resultOcr.IdNo,
                    IdType = input.Type?.ToUpper(),
                    Sex = resultOcr.Sex,
                    DateOfBirth = resultOcr.DateOfBirth,
                    PlaceOfOrigin = resultOcr.PlaceOfOrigin,
                    PlaceOfResidence = resultOcr.PlaceOfResidence,
                    IdIssuer = resultOcr.IdIssuer,
                    IdExpiredDate = resultOcr.IdIssueExpDate,
                    IdDate = resultOcr.IdIssueDate,
                    Nationality = resultOcr.Nationality,
                    InvestorId = input.InvestorId,
                };

                _saleInvestorRepository.CreateIdentification(dto);
            }

            return resultOcr;
        }

        /// <summary>
        /// Xác nhận và cập nhật lại thông tin ekyc
        /// </summary>
        /// <param name="dto"></param>
        public void ConfirmAndUpdateEkyc(SaleInvestorConfirmUpdateDto dto)
        {

            if ((dto.IdExpiredDate.HasValue && dto.IdExpiredDate < DateTime.Now) || (!dto.IdExpiredDate.HasValue && dto.IdDate.AddYears(15) < DateTime.Now))
            {
                throw new FaultException(new FaultReason($"Thông tin giấy tờ đã hết hạn"), new FaultCode(((int)ErrorCode.InvestorIdExpired).ToString()), "");
            }
            _saleInvestorRepository.ConfirmAndUpdateEkyc(dto);
        }

        /// <summary>
        /// Cập nhật avatar
        /// </summary>
        /// <param name="dto"></param>
        public string UploadAvatar(SaleInvestorUploadAvatarDto dto)
        {

            string fileUrl = _fileServices.UploadFileID(new UploadFileModel
            {
                Folder = FileFolder.INVESTOR,
                File = dto.Image
            });

            _saleInvestorRepository.UploadAvatar(dto.InvestorId, fileUrl);

            return fileUrl;
        }

        /// <summary>
        /// Thêm địa chỉ liên lạc
        /// </summary>
        /// <param name="dto"></param>
        public void AddContactAddress(CreateContactAddressDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _saleInvestorRepository.AddContactAddress(dto, username);
        }

        /// <summary>
        /// Thêm tk ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        public async Task AddBank(CreateBankDto dto)
        {
            _saleInvestorRepository.AddBank(dto);

            var investor = _managerInvestorRepository.FindById(dto.InvestorId, false);

            string phone = investor?.Phone;
            //Gửi email thông báo xác minh tài khoản thành công
            await _sendEmailServices.SendEmailVerificationAccountSuccess(phone);
            await _sendEmailServices.SendNotifyEnterReferralWhenRegister(phone);
        }

        /// <summary>
        /// Upload file nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        public void UploadProfFile(SaleInvestorUploadProfFileDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int userId = CommonUtils.GetCurrentUserId(_httpContext);

            _investorServices.UploadProfFileCommon(new UploadProfFileDto
            {
                ProfFile = dto.ProfFile
            }, dto.InvestorId, username, userId);
        }

        /// <summary>
        /// Lấy danh sách investor theo sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public StatisticInvestorBySaleDto GetListInvestorBySale(GetInvestorBySaleDto dto)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var saleFind = _saleEFRepository.SaleInfoInDepartment(saleId, dto.TradingProviderId);
            if (saleFind == null) return null;
            //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)
            var listTradingIds = _saleRepository.GetListTradingProviderIdFrom4Level(dto.TradingProviderId);

            StatisticInvestorBySaleDto result = new();
            result.Investors = new();
            var investorInfo = from investorSale in _dbContext.InvestorSales.AsNoTracking()
                               join investor in _dbContext.Investors.AsNoTracking() on investorSale.InvestorId equals investor.InvestorId
                               where investorSale.Deleted == YesNo.NO && investor.Deleted == YesNo.NO
                               && investor.Step > InvestorAppStep.DA_DANG_KY //&& investor.FaceImageUrl != null
                               && investorSale.SaleId == saleId && investor.Status != InvestorStatus.TEMP
                               && investorSale.Id == _dbContext.InvestorSales.FirstOrDefault(r => r.SaleId == saleId && r.InvestorId == investor.InvestorId).Id
                               select new ViewInvestorsBySaleDto
                               {
                                   InvestorId = investor.InvestorId,
                                   Phone = investor.Phone,
                                   AvatarImageUrl = investor.AvatarImageUrl,
                                   ReferralCode = investor.ReferralCodeSelf,
                                   CreatedDate = investorSale.CreatedDate,
                                   Status = investor.Status,
                                   FullName = _dbContext.InvestorIdentifications
                                                .Where(ii => ii.InvestorId == investor.InvestorId && ii.Status != InvestorStatus.TEMP && ii.Deleted == YesNo.NO)
                                                .OrderByDescending(ii => ii.IsDefault)
                                                .ThenByDescending(ii => ii.Id)
                                                .Select(ii => ii.Fullname)
                                                .FirstOrDefault()
                               };
            // Lọc các investor theo thời gian hoặc trạng thái
            var investorInfoFilter = investorInfo.Where(i => (dto.Status == null || dto.Status == i.Status)
                                       && (dto.StartDate == null || dto.StartDate.Value.Date <= i.CreatedDate.Value.Date)
                                       && (dto.EndDate == null || dto.EndDate.Value.Date >= i.CreatedDate.Value.Date));
            foreach (var item in investorInfoFilter)
            {
                // Tính doanh số hợp đồng của các nhà đầu tư
                var statisticOrderByInvestor = StatisticOrderByInvestor(new int[] { item.InvestorId }, saleFind.ReferralCode, dto.TradingProviderId, listTradingIds, dto.StartDate, dto.EndDate);
                item.TotalValueMoney = statisticOrderByInvestor.Sum(r => r.InitTotalValue);
                item.Balance = statisticOrderByInvestor.Sum(r => r.TotalValue);
                item.TotalContract = statisticOrderByInvestor.Count();
                result.Investors.Add(item);
            }

            DateTime endDate = dto.EndDate ?? DateTime.Today.Date;

            // Nếu không truyền ngày thì mặc định là 1 tuần
            DateTime startDate = dto.StartDate ?? DateTime.Today.Date.AddDays(-6);

            // Liệt kê ngày trong khoảng thời gian đã chọn
            var dates = Enumerable.Range(0, (endDate.Date - startDate.Date).Days + 1)
                      .Select(offset => startDate.Date.AddDays(offset));

            // Tổng số hợp đồng của nhà đầu tư quét sale
            var statisticOrderByAllInvestor = StatisticOrderByInvestor(investorInfo.Select(r => r.InvestorId).ToArray(), saleFind.ReferralCode, dto.TradingProviderId, listTradingIds, startDate, endDate);

            // Lấy tổng số hợp đồng theo ngày
            var totalContract = statisticOrderByAllInvestor.GroupBy(r => r.ActiveDate.Date)
                .Select(g => new StatisticOrderByInvestorWithTimeDto { Date = g.Key, TotalContract = g.Count() });

            // Lấy tổng số nhà đầu tư theo ngày
            var totalInvestor = investorInfo.GroupBy(r => r.CreatedDate.Value.Date)
                .Select(g => new StatisticOrderByInvestorWithTimeDto { Date = g.Key, TotalInvestor = g.Count() });

            //Gộp cả tổng hợp đồng và tổng nhà đầu tư lại theo ngày
            var totalStatistic = Enumerable.Empty<StatisticOrderByInvestorWithTimeDto>();
            var statisticInvestor = totalStatistic.Union(totalInvestor).Union(totalContract)
                .GroupBy(r => r.Date)
                .Select(g => new StatisticOrderByInvestorWithTimeDto
                {
                    Date = g.Key,
                    TotalInvestor = g.Sum(x => x.TotalInvestor),
                    TotalContract = g.Sum(x => x.TotalContract),
                });

            // Chèn thêm các ngày chưa có trong dữ liệu hợp đồng
            var dataStatisticOrders = dates.GroupJoin(
                                        statisticInvestor,
                                        date => date,
                                        order => order.Date,
                                        (date, orderGroup) => new StatisticOrderByInvestorWithTimeDto
                                        {
                                            Date = date,
                                            TotalInvestor = orderGroup.Sum(o => o.TotalInvestor),
                                            TotalContract = orderGroup.Sum(o => o.TotalContract)
                                        }).OrderBy(o => o.Date);

            IEnumerable<StatisticOrderByInvestorWithTimeDto> statisticOrders = Enumerable.Empty<StatisticOrderByInvestorWithTimeDto>();
            // Nhóm các dữ liệu
            switch ((endDate.Date - startDate.Date).Days)
            {
                case <= 30: // Nếu dưới 30 ngày, group by theo ngày
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => o.Date.Date)
                                        .Select(g => new StatisticOrderByInvestorWithTimeDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Day,
                                            TotalInvestor = g.Sum(o => o.TotalInvestor),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.DAY
                                        });
                    break;
                case > 30 and <= 70: // Nếu từ 30 - 70 ngày, group by theo tuần
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                                        .Select(g => new StatisticOrderByInvestorWithTimeDto
                                        {
                                            Date = g.Min(o => o.Date),
                                            Time = g.Key,
                                            TotalInvestor = g.Sum(o => o.TotalInvestor),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.WEEK
                                        });
                    break;
                default: // Nếu từ 70 ngày trở lên, group by theo tháng
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => new DateTime(o.Date.Year, o.Date.Month, 1))
                                        .Select(g => new StatisticOrderByInvestorWithTimeDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Month,
                                            TotalInvestor = g.Sum(o => o.TotalInvestor),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.MONTH
                                        });
                    break;
            }
            result.StatisticInvestorWithTime = statisticOrders.OrderBy(r => r.Date).ToList();
            result.Investors = result.Investors.OrderByDescending(o => o.TotalValueMoney).ThenBy(o => o.CreatedDate)
                                .Select((item, i) => { item.Index = (item.TotalValueMoney != 0) ? i + 1 : null; return item; }).ToList();
            return result;
        }

        /// <summary>
        /// Xem chi tiết nhà đầu tư theo sale và doanh số trong đại lý
        /// </summary>
        public InvestorInfoBySaleDto InvestorInfoBySale(int investorId, int tradingProviderId)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var saleFind = _saleEFRepository.SaleInfoInDepartment(saleId, tradingProviderId);
            if (saleFind == null) return null;
            //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)
            var listTradingIds = _saleRepository.GetListTradingProviderIdFrom4Level(tradingProviderId);

            // Tìm kiếm nhà đầu tư
            var investor = _dbContext.Investors.AsNoTracking().FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO);
            if (investor == null) return null;

            var result = new InvestorInfoBySaleDto
            {
                InvestorId = investor.InvestorId,
                Phone = investor.Phone,
                AvatarImageUrl = investor.AvatarImageUrl,
                ReferralCode = investor.ReferralCodeSelf,
                CreatedDate = investor.CreatedDate,
                Email = investor.Email,
                FullName = _dbContext.InvestorIdentifications
                             .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                             .OrderByDescending(ii => ii.IsDefault)
                             .ThenByDescending(ii => ii.Id)
                             .Select(ii => ii.Fullname)
                             .FirstOrDefault()
            };

            // Nếu có quan hệ với Sale thì lấy ngày đấy
            var investorSale = _dbContext.InvestorSales.FirstOrDefault(s => s.InvestorId == investorId && s.SaleId == saleId && s.Deleted == YesNo.NO);
            result.CreatedDate = (investorSale != null) ? investorSale.CreatedDate : result.CreatedDate;

            var statisticOrderByInvestor = StatisticOrderByInvestor(new int[] { investorId }, saleFind.ReferralCode, tradingProviderId, listTradingIds, null, null);
            result.InvestTotalValueMoney = statisticOrderByInvestor.Where(r => r.ProjectType == ProjectTypes.INVEST).Sum(r => r.InitTotalValue);
            result.GarnerTotalValueMoney = statisticOrderByInvestor.Where(r => r.ProjectType == ProjectTypes.GARNER).Sum(r => r.InitTotalValue);
            result.RealEstateTotalValueMoney = statisticOrderByInvestor.Where(r => r.ProjectType == ProjectTypes.REAL_ESTATE).Sum(r => r.InitTotalValue);
            return result;
        }

        /// <summary>
        /// Doanh số của nhà đầu tư trong đại lý
        /// </summary>
        public IEnumerable<StatisticOrderByInvestorDto> StatisticOrderByInvestor(int[] investorIds, string saleReferralCode, int tradingProviderId, List<int> tradingProviderIds, DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<StatisticOrderByInvestorDto> result = Enumerable.Empty<StatisticOrderByInvestorDto>();
            if (!investorIds.Any()) return result;
            var invest = from cifCode in _dbContext.CifCodes
                         join investOrder in _dbContext.InvOrders on cifCode.CifCode equals investOrder.CifCode
                         where cifCode.Deleted == YesNo.NO && investOrder.Deleted == YesNo.NO && investOrder.InvestDate != null
                         && (new List<int> { OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN }).Contains(investOrder.Status)
                         && (investOrder.Status == OrderStatus.DANG_DAU_TU || (investOrder.Status == OrderStatus.TAT_TOAN && investOrder.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC))
                         && investorIds.Contains(cifCode.InvestorId ?? 0)
                         && (startDate == null || startDate.Value.Date <= investOrder.InvestDate.Value.Date)
                         && (endDate == null || endDate.Value.Date >= investOrder.InvestDate.Value.Date)
                         && ((investOrder.TradingProviderId == tradingProviderId && saleReferralCode == investOrder.SaleReferralCode)
                            || (tradingProviderIds != null && tradingProviderIds.Contains(investOrder.TradingProviderId) && saleReferralCode == investOrder.SaleReferralCodeSub))
                         select new StatisticOrderByInvestorDto
                         {
                             InitTotalValue = investOrder.InitTotalValue,
                             TotalValue = investOrder.TotalValue,
                             ProjectType = ProjectTypes.INVEST,
                             ActiveDate = investOrder.InvestDate.Value
                         };

            var garner = from cifCode in _dbContext.CifCodes
                         join garnerOrder in _dbContext.GarnerOrders on cifCode.CifCode equals garnerOrder.CifCode
                         where cifCode.Deleted == YesNo.NO && garnerOrder.Deleted == YesNo.NO && garnerOrder.InvestDate != null
                         && (new List<int> { OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN }).Contains(garnerOrder.Status)
                         && garnerOrder.TradingProviderId == tradingProviderId && investorIds.Contains(cifCode.InvestorId ?? 0)
                         && (startDate == null || startDate.Value.Date <= garnerOrder.InvestDate.Value.Date)
                         && (endDate == null || endDate.Value.Date >= garnerOrder.InvestDate.Value.Date)
                         && ((garnerOrder.TradingProviderId == tradingProviderId && saleReferralCode == garnerOrder.SaleReferralCode)
                            || (tradingProviderIds != null && tradingProviderIds.Contains(garnerOrder.TradingProviderId) && saleReferralCode == garnerOrder.SaleReferralCodeSub))
                         select new StatisticOrderByInvestorDto
                         {
                             InitTotalValue = garnerOrder.InitTotalValue,
                             TotalValue = garnerOrder.TotalValue,
                             ActiveDate = garnerOrder.InvestDate.Value,
                             ProjectType = ProjectTypes.GARNER
                         };
            var realEstate = from cifCode in _dbContext.CifCodes
                             join rstOrder in _dbContext.RstOrders on cifCode.CifCode equals rstOrder.CifCode
                             join productItem in _dbContext.RstProductItems on rstOrder.ProductItemId equals productItem.Id
                             where cifCode.Deleted == YesNo.NO && rstOrder.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && rstOrder.DepositDate != null
                             && (new List<int> { RstOrderStatus.DA_COC }).Contains(rstOrder.Status) && productItem.Status == RstProductItemStatus.DA_COC
                             && rstOrder.TradingProviderId == tradingProviderId && investorIds.Contains(cifCode.InvestorId ?? 0)
                             && (startDate == null || startDate.Value.Date <= rstOrder.DepositDate.Value.Date)
                             && (endDate == null || endDate.Value.Date >= rstOrder.DepositDate.Value.Date)
                             && ((rstOrder.TradingProviderId == tradingProviderId && saleReferralCode == rstOrder.SaleReferralCode)
                                || (tradingProviderIds != null && tradingProviderIds.Contains(rstOrder.TradingProviderId) && saleReferralCode == rstOrder.SaleReferralCodeSub))
                             select new StatisticOrderByInvestorDto
                             {
                                 InitTotalValue = productItem.Price ?? 0,
                                 TotalValue = productItem.Price ?? 0,
                                 ActiveDate = rstOrder.DepositDate.Value,
                                 ProjectType = ProjectTypes.REAL_ESTATE
                             };
            return result.Union(invest).Union(garner).Union(realEstate);
        }

        /// <summary>
        /// Lọc investor cho sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<ViewInvestorsBySaleDto> FilterInvestor(FilterManagerInvestorDto dto)
        {
            dto.RequireKeyword = true;

            var query = _managerInvestorRepository.FilterInvestor(dto);

            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            if (query.Items != null)
            {
                var data = _mapper.Map<List<ViewInvestorsBySaleDto>>(query.Items);
                return data?.Where(i => i.InvestorId != investorId)?.ToList();
            }

            return null;
        }

        /// <summary>
        /// Sale xem thông tin chi tiết khách hàng
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public ViewInvestorInfoForSaleDto GetDetailInvestorInfo(int investorId)
        {

            int saleId = CommonUtils.GetCurrentSaleId(_httpContext);

            //bool isOfSale = _saleInvestorRepository.IsInvestorOfSale(investorId, saleId);

            //if (isOfSale)
            //{
            var investor = _managerInvestorRepository.FindById(investorId, false);

            if (investor != null)
            {
                var result = new ViewInvestorInfoForSaleDto();
                result = _mapper.Map<ViewInvestorInfoForSaleDto>(investor);

                var user = _userRepository.FindByInvestorId(investorId);
                result.UserStatus = user?.Status;

                var iden = _managerInvestorRepository.GetDefaultIdentification(investorId, false);
                if (iden != null)
                {
                    result.Fullname = iden?.Fullname;
                    result.DefaultIdentification = _mapper.Map<ViewIdentificationDto>(iden);
                }
                else
                {
                    result.Fullname = user.UserName;
                }
                // Danh sách thông tin của nhà đầu tư
                var listIden = _investorEFRepository.GetListIdentification(investorId).Where(i => i.Status == Status.ACTIVE);
                result.ListIdentifications = _mapper.Map<List<ViewIdentificationDto>>(listIden);
                var bank = _investorEFRepository.GetDefaultBankAccount(investorId);
                if (bank != null)
                {
                    result.DefaultBank = _mapper.Map<ViewInvestorBankAccountDto>(bank);
                    var coreBank = _dbContext.CoreBanks.FirstOrDefault(b => b.BankId == bank.BankId);
                    result.DefaultBank.BankName = coreBank?.BankName;
                    result.DefaultBank.CoreBankName = coreBank?.BankName;
                    result.DefaultBank.CoreFullBankName = coreBank?.FullBankName;
                    result.DefaultBank.CoreLogo = coreBank?.Logo;
                }

                var contactAddress = _managerInvestorRepository.GetDefaultContactAddress(investorId, false);
                if (contactAddress != null)
                {
                    result.DefaultContactAddress = _mapper.Map<ViewInvestorContactAddressDto>(contactAddress);
                }

                return result;
            }
            //}

            return null;
        }

    }
}
