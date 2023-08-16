using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.Notification.Dto.Rocketchat;
using EPIC.RocketchatEntities.Dto.Integration;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.Notification.Services
{
    public class RocketChatNotificationServices
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        private readonly InvestorEFRepository _investorEFRepository;
        private readonly UsersEFRepository _usersEFRepository;

        //Common
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;

        public RocketChatNotificationServices(
            EpicSchemaDbContext dbContext,
            ILogger<RocketChatNotificationServices> logger,
            IMapper mapper,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            SharedNotificationApiUtils sharedEmailApiUtils
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _env = env;
            _mapper = mapper;
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _usersEFRepository = new UsersEFRepository(_dbContext, _logger);

            //Common
            _httpContextAccessor = httpContextAccessor;
            _sharedEmailApiUtils = sharedEmailApiUtils;
        }

        /// <summary>
        /// Gửi thông báo nhận được tin nhắn mới từ agent
        /// </summary>
        /// <param name="voucherInvestorId"></param>
        /// <returns></returns>
        public async Task SendNotificationAgentSentMsg(int investorId, string msgContent, int? tradingProviderId = 0)
        {
            _logger.LogInformation($"{nameof(SendNotificationAgentSentMsg)}: investorId = {investorId}; msgContent = {msgContent}, tradingProviderId = {tradingProviderId}");

            var investor = _investorEFRepository.FindById(investorId);
            if (investor == null)
            {
                _logger.LogError($"{nameof(SendNotificationAgentSentMsg)}: investor not found, InvestorId = {investorId}");
                return;
            }

            var user = _usersEFRepository.FindByInvestorId(investorId);
            if (user == null)
            {
                _logger.LogError($"{nameof(SendNotificationAgentSentMsg)}: user not found, InvestorId = {investorId}");
                return;
            }

            // Thông tin người nhận
            var receiver = new Receiver
            {
                Phone = investor.Phone,
                Email = new EmailNotifi
                {
                    Address = investor.Email,
                    Title = TitleEmail.CHAT_AGENT_SENT_MSG_TO_VISITOR
                },
                UserId = _mapper.Map<UserDto>(user)?.UserId.ToString(),
                FcmTokens = _usersEFRepository.GetFcmTokenByUserId(user.UserId)
            };

            // Nội dung email
            var content = new AgentSendMsgToVisitorDto()
            {
                Content = msgContent,

            };

            // Template của đại lý nào
            //var otherParams = new ParamsChooseTemplate
            //{
            //    TradingProviderId = tradingProviderId,
            //};
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.CHAT_RECEIVE_MSG, receiver, null);
        }

        public async Task SendNotificationSenderSendMsgToReceiver(int senderInvestorId, int receiverInvestorId, IntegrationDto integrationContent)
        {
            _logger.LogInformation($"{nameof(SendNotificationSenderSendMsgToReceiver)}: senderInvestorId = {senderInvestorId}, senderInvestorId = {senderInvestorId}; integrationContent = {JsonSerializer.Serialize(integrationContent)}");

            var senderInvestor = _dbContext.Investors.Include(e => e.InvestorIdentifications).FirstOrDefault(e => e.InvestorId == senderInvestorId && e.Deleted == YesNo.NO);
            if (senderInvestor == null)
            {
                _logger.LogError($"{nameof(SendNotificationSenderSendMsgToReceiver)}: investor (senderInvestor) not found, InvestorId (senderInvestor) = {senderInvestorId}");
                return;
            }

            var receiverInvestor = _dbContext.Investors.Include(e => e.InvestorIdentifications).FirstOrDefault(e => e.InvestorId == receiverInvestorId && e.Deleted == YesNo.NO);
            if (receiverInvestor == null)
            {
                _logger.LogError($"{nameof(SendNotificationSenderSendMsgToReceiver)}: investor (receiverInvestor) not found, InvestorId (receiverInvestor) = {receiverInvestorId}");
                return;
            }

            var senderUser = _usersEFRepository.FindByInvestorId(senderInvestorId);
            if (senderUser == null)
            {
                _logger.LogError($"{nameof(SendNotificationSenderSendMsgToReceiver)}: senderUser not found, InvestorId = {senderInvestorId}");
                return;
            }

            var receiverUser = _usersEFRepository.FindByInvestorId(receiverInvestorId);
            if (receiverUser == null)
            {
                _logger.LogError($"{nameof(SendNotificationSenderSendMsgToReceiver)}: receiverUser not found, InvestorId = {receiverInvestorId}");
                return;
            }

            // Thông tin người nhận
            var receiver = new Receiver
            {
                Phone = receiverInvestor.Phone,
                Email = new EmailNotifi
                {
                    Address = receiverInvestor.Email,
                    Title = TitleEmail.CHAT_AGENT_SENT_MSG_TO_VISITOR
                },
                UserId = _mapper.Map<UserDto>(receiverUser)?.UserId.ToString(),
                FcmTokens = _usersEFRepository.GetFcmTokenByUserId(receiverUser.UserId)
            };

            // Nội dung
            var content = new IntegrationContent()
            {
                Sender = new InvestorDirectMsgDto
                {
                    InvestorId = senderInvestor.InvestorId,
                    AvatarImageUrl = senderInvestor.AvatarImageUrl,
                    Phone = senderInvestor.Phone,
                    FullName = senderInvestor.InvestorIdentifications.Where(e => e.Deleted == YesNo.NO).OrderByDescending(e => e.IsDefault).Select(e => e.Fullname).FirstOrDefault()
                },
                Payload = integrationContent
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.CHAT_RECEIVE_DIRECT_MSG, receiver, null);
        }
    }
}
