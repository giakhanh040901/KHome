using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.User;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.IdentityRepositories;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Dto.GarnerNotification;
using EPIC.Notification.Dto.LoyaltyNotification;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Services
{
    public class LoyaltyNotificationServices
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        private readonly IOptions<LinkVoucherConfiguration> _linkVoucherConfiguration;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly LoyVoucherInvestorEFRepository _loyVoucherInvestorEFRepository;
        private readonly LoyPointInvestorEFRepoistory _loyPointInvestorEFRepository;
        private readonly LoyHisAccumulatePointEFRepository _loyHisAccumulatePointEFRepository;

        private readonly UsersEFRepository _usersEFRepository;

        //Common
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public LoyaltyNotificationServices(
            EpicSchemaDbContext dbContext,
            ILogger<LoyaltyNotificationServices> logger,
            IMapper mapper,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            SharedNotificationApiUtils sharedEmailApiUtils,
            IOptions<LinkVoucherConfiguration> linkVoucherConfiguration,
            IConfiguration configuration
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _env = env;

            _loyVoucherInvestorEFRepository = new LoyVoucherInvestorEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _loyHisAccumulatePointEFRepository = new LoyHisAccumulatePointEFRepository(dbContext, logger);
            _loyPointInvestorEFRepository = new LoyPointInvestorEFRepoistory(dbContext, logger);
            _usersEFRepository = new UsersEFRepository(_dbContext, _logger);

            //Common
            _httpContextAccessor = httpContextAccessor;
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _configuration = configuration;
            _baseUrl = _configuration["SharedApi:BaseUrl"];
        }

        /// <summary>
        /// Gửi thông báo gán voucher cho khách thành công
        /// </summary>
        /// <param name="voucherInvestorId"></param>
        /// <returns></returns>
        public async Task SendNotificationAddVoucherToInvestor(int voucherInvestorId)
        {
            _logger.LogInformation($"{nameof(SendNotificationAddVoucherToInvestor)}: Gửi notification gán voucher cho khách thành công => voucherInvestorId={voucherInvestorId}");

            var voucherInvestor = _loyVoucherInvestorEFRepository.GetDataToSendNotificationAddVoucherToInvestor(voucherInvestorId);

            if (voucherInvestor == null)
            {
                _logger.LogError($"{nameof(SendNotificationAddVoucherToInvestor)}: Không tìm thấy bản ghi VoucherInvestor khi gửi email/ sms/ push app. voucherInvestorId: {voucherInvestorId}");
                return;
            }

            var investor = _investorEFRepository.FindById(voucherInvestor.InvestorId);

            if (investor == null)
            {
                _logger.LogError($"{nameof(SendNotificationAddVoucherToInvestor)}: Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. InvestorId: {voucherInvestor.InvestorId}");
                return;
            }

            var user = _usersEFRepository.FindByInvestorId(voucherInvestor.InvestorId);

            if (user == null)
            {
                _logger.LogError($"{nameof(SendNotificationAddVoucherToInvestor)}: Không tìm thấy thông tin tài khoản user của khách hàng khi gửi email/ sms/ push app. InvestorId: {voucherInvestor.InvestorId}");
                return;
            }

            // Thông tin người nhận
            var receiver = new Receiver
            {
                Phone = investor.Phone,
                Email = new EmailNotifi
                {
                    Address = investor.Email,
                    Title = TitleEmail.LOYALTY_CONVERSION_POINT_SUCCESS
                },
                UserId = _mapper.Map<UserDto>(user)?.UserId.ToString(),
                FcmTokens = _usersEFRepository.GetFcmTokenByUserId(user.UserId)
            };

            // Nội dung email
            var content = new LoyaltyAddVoucherToInvestorContent()
            {
                CustomerName = voucherInvestor.Fullname,
                VoucherName = voucherInvestor.VoucherName,
                VoucherType = voucherInvestor.VoucherType == "DT" ? "Điện tử" : "Thẻ cứng",
                EndDate = voucherInvestor.EndDate?.ToString("dd/MM/yyyy"),
                StartDate = voucherInvestor.StartDate?.ToString("dd/MM/yyyy"),
            };

            // Template của đại lý nào
            var otherParams = new ParamsChooseTemplate
            {
                TradingProviderId = voucherInvestor.TradingProviderId,
            };

            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.THONG_BAO_GAN_VOUCHER_OK, receiver, otherParams);
        }

        /// <summary>
        /// Thông báo tích điểm thành công
        /// </summary>
        /// <param name="hisAccumulatePointId"></param>
        /// <returns></returns>
        public async Task SendNotificationAccumulatePointSuccess(int hisAccumulatePointId)
        {
            _logger.LogInformation($"{nameof(SendNotificationAccumulatePointSuccess)}: Gửi notification Thông báo tích điểm thành công => hisAccumulatePointId={hisAccumulatePointId}");

            var his = _loyHisAccumulatePointEFRepository.FindById(hisAccumulatePointId);
            if (his == null)
            {
                _logger.LogError($"{nameof(SendNotificationAccumulatePointSuccess)}: Không tìm thấy bản ghi lệnh tích điểm khi gửi email/ sms/ push app. hisAccumulatePointId: {hisAccumulatePointId}");
                return;
            }

            var invPoint = _loyPointInvestorEFRepository.Get(his.InvestorId, his.TradingProviderId);
            if (invPoint == null)
            {
                _logger.LogError($"{nameof(SendNotificationAccumulatePointSuccess)}: Không tìm thấy điểm của khách khi gửi email/ sms/ push app. his.InvestorId={his.InvestorId}; his.TradingProviderId={his.TradingProviderId}");
                return;
            }

            var investor = _investorEFRepository.FindById(his.InvestorId);
            if (investor == null)
            {
                _logger.LogError($"{nameof(SendNotificationAccumulatePointSuccess)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. his.InvestorId: {his.InvestorId}");
                return;
            }

            var iden = _investorEFRepository.GetDefaultIdentification(his.InvestorId);
            if (iden == null)
            {
                _logger.LogError($"{nameof(SendNotificationAccumulatePointSuccess)}: Không tìm thấy giấy tờ mặc định khi gửi email/ sms/ push app. his.InvestorId: {his.InvestorId}");
                return;
            }

            var user = _usersEFRepository.FindByInvestorId(investor.InvestorId);
            if (user == null)
            {
                _logger.LogError($"{nameof(SendNotificationAccumulatePointSuccess)}: Không tìm thấy thông tin tài khoản user của khách hàng khi gửi email/ sms/ push app. investor.InvestorId={investor.InvestorId}");
                return;
            }

            // Thông tin người nhận
            var receiver = new Receiver
            {
                Phone = investor.Phone,
                Email = new EmailNotifi
                {
                    Address = investor.Email,
                    Title = TitleEmail.LOYALTY_ACCUMULATE_POINT_SUCCESS
                },
                UserId = _mapper.Map<UserDto>(user)?.UserId.ToString(),
                FcmTokens = _usersEFRepository.GetFcmTokenByUserId(user.UserId)
            };

            // Nội dung email
            var content = new LoyAccumulatePointSuccessContent()
            {
                CustomerName = iden.Fullname,
                ApplyDate = his.ApplyDate?.ToString("dd/MM/yyyy"),
                Point = his.Point.ToString(),
                TotalPoint = invPoint.TotalPoint.ToString(),
                CurrentPoint = invPoint.CurrentPoint.ToString(),
            };

            // Template của đại lý nào
            var otherParams = new ParamsChooseTemplate
            {
                TradingProviderId = his.TradingProviderId,
            };

            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.LOYALTY_TICH_DIEM_OK, receiver, otherParams);
        }

        /// <summary>
        /// Thông báo tạo yêu cầu đổi ưu đãi thành công
        /// </summary>
        public async Task SendNotificationRequestConversionPoint(int conversionPointId)
        {
            _logger.LogInformation($"{nameof(SendNotificationRequestConversionPoint)}: Gửi notification Thông báo tạo yêu cầu đổi ưu đãi thành công => conversionPointId={conversionPointId}");

            var conversionPoint = _dbContext.LoyConversionPoints.FirstOrDefault(c => c.Id == conversionPointId && c.Deleted == YesNo.NO);
            if (conversionPoint == null)
            {
                _logger.LogError($"{nameof(SendNotificationRequestConversionPoint)}: Không tìm thấy bản ghi yêu cầu đổi ưu đãi khi gửi email/ sms/ push app. conversionPointId: {conversionPointId}");
                return;
            }
            /// Lấy danh sách yêu cầu chi tiết
            var conversionPointDetail = _dbContext.LoyConversionPointDetails.Where(c => c.ConversionPointId == conversionPointId && c.Deleted == YesNo.NO);
            if (conversionPointDetail.Count() == 0)
            {
                _logger.LogError($"{nameof(SendNotificationRequestConversionPoint)}: Không tìm thấy bản ghi chi tiết yêu cầu đổi ưu đãi khi gửi email/ sms/ push app. conversionPointId: {conversionPointId}");
                return;
            }
            var investor = _investorEFRepository.FindById(conversionPoint.InvestorId);
            if (investor == null)
            {
                _logger.LogError($"{nameof(SendNotificationRequestConversionPoint)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. his.InvestorId: {conversionPoint.InvestorId}");
                return;
            }

            var iden = _investorEFRepository.GetDefaultIdentification(conversionPoint.InvestorId);
            if (iden == null)
            {
                _logger.LogError($"{nameof(SendNotificationRequestConversionPoint)}: Không tìm thấy giấy tờ mặc định khi gửi email/ sms/ push app. his.InvestorId: {conversionPoint.InvestorId}");
                return;
            }

            var user = _usersEFRepository.FindByInvestorId(investor.InvestorId);
            if (user == null)
            {
                _logger.LogError($"{nameof(SendNotificationRequestConversionPoint)}: Không tìm thấy thông tin tài khoản user của khách hàng khi gửi email/ sms/ push app. investor.InvestorId={investor.InvestorId}");
                return;
            }

            // Thông tin người nhận
            var receiver = new Receiver
            {
                Phone = investor.Phone,
                Email = new EmailNotifi
                {
                    Address = investor.Email,
                    Title = TitleEmail.LOYALTY_CONSUME_POINT
                },
                UserId = _mapper.Map<UserDto>(user)?.UserId.ToString(),
                FcmTokens = _usersEFRepository.GetFcmTokenByUserId(user.UserId)
            };

            // Template của đại lý nào
            var otherParams = new ParamsChooseTemplate
            {
                TradingProviderId = conversionPoint.TradingProviderId,
            };
            foreach (var item in conversionPointDetail)
            {
                var voucher = _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == item.VoucherId && v.Deleted == YesNo.NO);
                if (voucher == null)
                {
                    _logger.LogError($"{nameof(SendNotificationRequestConversionPoint)}: Không tìm thấy thông tin voucher khi gửi email/ sms/ push app. voucherId= {item.VoucherId}");
                    continue;
                }
                // Nội dung email
                var content = new LoySendRequestConversionPointContent()
                {
                    CustomerName = iden.Fullname,
                    VoucherName = voucher.DisplayName,
                };

                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.LOYALTY_TAO_YEU_CAU_DOI_DIEM_VOUCHER, receiver, otherParams);
            }
        }

        /// <summary>
        /// Gửi thông báo khách đã tạo yêu cầu đổi điểm ưu đãi cho admin
        /// </summary>
        /// <param name="conversionPointId"></param>
        /// <returns></returns>
        public async Task SendNotificationExchangeRequestAdmin(int conversionPointId)
        {
            _logger.LogInformation($"{nameof(SendNotificationExchangeRequestAdmin)}: Gửi notification Thông báo Đã gửi yêu cầu đổi điểm => conversionPointId={conversionPointId}");

            var conversionPoint = _dbContext.LoyConversionPoints.FirstOrDefault(c => c.Id == conversionPointId && c.Deleted == YesNo.NO);
            if (conversionPoint == null)
            {
                _logger.LogError($"{nameof(SendNotificationExchangeRequestAdmin)}: Không tìm thấy bản ghi yêu cầu đổi ưu đãi khi gửi email/ sms/ push app. conversionPointId: {conversionPointId}");
                return;
            }

            // Yêu cầu đổi trên App là đổi 1 chi tiết 
            var conversionPointDetail = _dbContext.LoyConversionPointDetails.FirstOrDefault(c => c.ConversionPointId == conversionPointId && c.Deleted == YesNo.NO);
            if (conversionPointDetail == null)
            {
                _logger.LogError($"{nameof(SendNotificationExchangeRequestAdmin)}: Không tìm thấy bản ghi chi tiết yêu cầu đổi ưu đãi khi gửi email/ sms/ push app. conversionPointId: {conversionPointId}");
                return;
            }

            var voucher = _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == conversionPointDetail.VoucherId && v.Deleted == YesNo.NO);
            if (voucher == null)
            {
                _logger.LogError($"{nameof(SendNotificationRequestConversionPoint)}: Không tìm thấy thông tin voucher khi gửi email/ sms/ push app. voucherId= {conversionPointDetail.VoucherId}");
                return;
            }

            var investor = _investorEFRepository.FindById(conversionPoint.InvestorId);
            if (investor == null)
            {
                _logger.LogError($"{nameof(SendNotificationExchangeRequestAdmin)}: Gửi notification Thông báo Đã gửi yêu cầu đổi điểm => conversionPointId= {conversionPointId}");
                return;
            }

            var iden = _investorEFRepository.GetDefaultIdentification(conversionPoint.InvestorId);
            if (iden == null)
            {
                _logger.LogError($"{nameof(SendNotificationExchangeRequestAdmin)}: Gửi notification Thông báo Đã gửi yêu cầu đổi điểm => conversionPointId= {conversionPointId}");
                return;
            }

            var user = _usersEFRepository.FindByInvestorId(investor.InvestorId);
            if (user == null)
            {
                _logger.LogError($"{nameof(SendNotificationExchangeRequestAdmin)}: Không tìm thấy thông tin tài khoản user của khách hàng khi gửi email/ sms/ push app. investor.InvestorId={investor.InvestorId}");
                return;
            }

            // Thông tin người nhận
            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.LOYALTY_CONSUME_POINT
                },
            };

            // Nội dung email
            var content = new LoySendExchangeRequestAdminContent()
            {
                VoucherName = voucher.DisplayName,
                Point = conversionPointDetail.TotalConversionPoint.ToString(),
                CreatedDate = conversionPoint.CreatedDate?.ToString("dd/MM/yyyy HH:mm"),
                CurrentPoint = conversionPoint.CurrentPoint.ToString(),
                CustomerName = iden.Fullname,
                Phone = investor.Phone,
            };

            // Template của đại lý nào
            var otherParams = new ParamsChooseTemplate
            {
                TradingProviderId = conversionPoint.TradingProviderId,
            };

            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.LOYALTY_ADMIN_KHACH_TAO_YEU_CAU_DOI_DIEM_VOUCHER, receiver, otherParams);
        }

        /// <summary>
        /// Thông báo khách nhận voucher thành công | Yêu cầu đổi điểm voucher Trạng thái đang giao => hoàn thành
        /// </summary>
        /// <param name="conversionPointId"></param>
        /// <returns></returns>
        public async Task SendNotificationInvestorReceivedVoucher(int conversionPointId)
        {
            _logger.LogInformation($"{nameof(SendNotificationInvestorReceivedVoucher)}: Gửi notification khách nhận voucher thành công thành công => voucherInvestorId={conversionPointId}");

            var conversionPoint = _dbContext.LoyConversionPoints.FirstOrDefault(c => c.Id == conversionPointId && c.Deleted == YesNo.NO);
            if (conversionPoint == null)
            {
                _logger.LogError($"{nameof(SendNotificationInvestorReceivedVoucher)}: Không tìm thấy bản ghi yêu cầu đổi ưu đãi khi gửi email/ sms/ push app. conversionPointId: {conversionPointId}");
                return;
            }

            // Yêu cầu đổi trên App là đổi 1 chi tiết 
            var conversionPointDetail = _dbContext.LoyConversionPointDetails.Where(c => c.ConversionPointId == conversionPointId && c.Deleted == YesNo.NO);
            if (conversionPointDetail.Count() == 0)
            {
                _logger.LogError($"{nameof(SendNotificationInvestorReceivedVoucher)}: Không tìm thấy bản ghi chi tiết yêu cầu đổi ưu đãi khi gửi email/ sms/ push app. conversionPointId: {conversionPointId}");
                return;
            }

            var conversionPointLogStatusFinished = _dbContext.LoyConversionPointStatusLogs.FirstOrDefault(c => c.ConversionPointId == conversionPointId && c.Status == LoyConversionPointStatus.FINISHED && c.Deleted == YesNo.NO);
            if (conversionPointLogStatusFinished == null)
            {
                _logger.LogError($"{nameof(SendNotificationInvestorReceivedVoucher)}: Không tìm thấy bản ghi lịch sử trạng thái thành công của yêu cầu đổi ưu đãi khi gửi email/ sms/ push app. conversionPointId: {conversionPointId}");
                return;
            }

            var investor = _investorEFRepository.FindById(conversionPoint.InvestorId);
            if (investor == null)
            {
                _logger.LogError($"{nameof(SendNotificationInvestorReceivedVoucher)}: Không tìm thấy thông tin khách hàng khi gửi email/ sms/ push app. InvestorId: {conversionPoint.InvestorId}");
                return;
            }

            var iden = _investorEFRepository.GetDefaultIdentification(conversionPoint.InvestorId);
            if (iden == null)
            {
                _logger.LogError($"{nameof(SendNotificationInvestorReceivedVoucher)}: Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. InvestorId: {conversionPoint.InvestorId}");
                return;
            }

            var user = _usersEFRepository.FindByInvestorId(conversionPoint.InvestorId);
            if (user == null)
            {
                _logger.LogError($"{nameof(SendNotificationInvestorReceivedVoucher)}: Không tìm thấy thông tin tài khoản user của khách hàng khi gửi email/ sms/ push app. InvestorId: {conversionPoint.InvestorId}");
                return;
            }

            // Thông tin người nhận
            var receiver = new Receiver
            {
                Phone = investor.Phone,
                Email = new EmailNotifi
                {
                    Address = investor.Email,
                    Title = TitleEmail.LOYALTY_CONVERSION_POINT_SUCCESS
                },
                UserId = _mapper.Map<UserDto>(user)?.UserId.ToString(),
                FcmTokens = _usersEFRepository.GetFcmTokenByUserId(user.UserId)
            };


            // Template của đại lý nào
            var otherParams = new ParamsChooseTemplate
            {
                TradingProviderId = conversionPoint.TradingProviderId,
            };

            foreach (var item in conversionPointDetail)
            {
                var voucher = _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == item.VoucherId && v.Deleted == YesNo.NO);
                if (voucher == null)
                {
                    _logger.LogError($"{nameof(SendNotificationInvestorReceivedVoucher)}: Không tìm thấy thông tin voucher khi gửi email/ sms/ push app. voucherId= {item.VoucherId}");
                    continue;
                }
                // Nội dung email
                var content = new LoyaltyAddVoucherToInvestorContent()
                {
                    CustomerName = iden.Fullname,
                    VoucherName = voucher.DisplayName,
                    EndDate = voucher.ExpiredDate?.ToString("dd/MM/yyyy"),
                    StartDate = voucher.StartDate.ToString("dd/MM/yyyy"),
                    FinishedDate = conversionPointLogStatusFinished.CreatedDate?.ToString("dd/MM/yyyy"),
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.LOYALTY_NHAN_VOUCHER_OK, receiver, otherParams);
            }
        }
    }
}
