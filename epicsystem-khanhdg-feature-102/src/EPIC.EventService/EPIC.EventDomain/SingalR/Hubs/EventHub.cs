using EPIC.DataAccess.Base;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.SignalR;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.EventDomain.SingalR.Hubs
{
    public class EventHub : Hub
    {
        private readonly ILogger _logger;
        private readonly EpicSchemaDbContext _dbContext;

        public EventHub(
            ILogger<EventHub> logger,
            EpicSchemaDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public override async Task OnConnectedAsync()
        {
            var userType = Context.User.FindFirst(ClaimTypes.UserType)?.Value;
            switch (userType)
            {
                case UserTypes.EPIC:
                case UserTypes.ROOT_EPIC:
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.Epic);
                    break;

                case UserTypes.TRADING_PROVIDER:
                case UserTypes.ROOT_TRADING_PROVIDER:
                    int tradingProviderId = int.Parse(Context.User.FindFirst(ClaimTypes.TradingProviderId).Value);
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.PrefixTradingProvider + tradingProviderId);
                    break;

                case UserTypes.INVESTOR:
                    int investorId = int.Parse(Context.User.FindFirst(ClaimTypes.InvestorId).Value);
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.Investor + investorId);
                    break;

                default:
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.Anonymous);
                    break;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
