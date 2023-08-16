using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.RocketChat;
using EPIC.IdentityRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.CoreSharedServices.Implements
{
    public class BackgroundJobIdentityService
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        private readonly UserRepository _userRepository;
        private readonly IRocketChatServices _rocketChatServices;

        public BackgroundJobIdentityService(
            ILogger<BackgroundJobIdentityService> logger,
            IRocketChatServices rocketChatServices,
            DatabaseOptions databaseOptions)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _userRepository = new UserRepository(_connectionString, _logger);
            _rocketChatServices = rocketChatServices;
        }

        /// <summary>
        /// Tạo tài khoản trên rocketchat cho investor sau khi investor đăng ký trên app
        /// </summary>
        /// <param name="investor"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 5)]
        [Queue(HangfireQueues.Shared)]
        [HangfireLogEverything]
        public async Task AppRegisterInvestorSuccess(Investor investor)
        {
            string defaultDepartmentName = _rocketChatServices.GetDefaultDepartmentName();

            // xử lý rocketchat
            if (investor != null)
            {
                var user = _userRepository.FindByInvestorId(investor.InvestorId);

                if (user != null)
                {
                    // tạo user rocketchat
                    await _rocketChatServices.CreateRocketchatUserForInvestor(new CreateRocketChatUserDto
                    {
                        email = investor.Email,
                        name = user.DisplayName ?? user.UserName,
                        password = _rocketChatServices.genPasswordByUser(user),
                        username = user.UserName,
                        roles = new List<string> { "user" },
                    });
                }
                else
                {
                    _logger.LogError($"App không tạo được tài khoản rocketchat khi hoàn tất luồng đăng ký. Không tìm thấy user với investorId = {investor.InvestorId}");
                }
            }
            else
            {
                _logger.LogError($"App không tạo được tài khoản rocketchat khi hoàn tất luồng đăng ký. Không tìm thấy investor với phone = {investor.Phone}");
            }
        }
    }
}
