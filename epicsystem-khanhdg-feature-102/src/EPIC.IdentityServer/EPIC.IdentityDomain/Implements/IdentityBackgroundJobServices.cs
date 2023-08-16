using EPIC.Entities.Dto.RocketChat;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.Filter;
using Hangfire;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Implements
{
    public class IdentityBackgroundJobServices
    {
        private readonly IRocketChatServices _rocketchatServices;

        public IdentityBackgroundJobServices(
            IRocketChatServices rocketchatServices)
        {
            _rocketchatServices = rocketchatServices;
        }

        /// <summary>
        /// Tạo tài khoản rocketchat cho investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 5)]
        [Queue(HangfireQueues.Shared)]
        [HangfireLogEverything]
        public async Task CreateRocketChatUserForInvestor(CreateRocketChatUserDto dto)
        {
            await _rocketchatServices.CreateRocketchatUserForInvestor(dto);
        }
    }
}
