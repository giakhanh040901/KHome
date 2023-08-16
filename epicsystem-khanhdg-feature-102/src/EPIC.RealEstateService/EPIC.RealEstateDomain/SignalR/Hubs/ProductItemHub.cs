using EPIC.DataAccess.Base;
using EPIC.RealEstateDomain.SignalR.Services;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.ConstantVariables.SignalR;
using EPIC.Utils.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;
using System;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.SignalR.Hubs
{
    [SignalRHub]
    public class ProductItemHub : Hub
    {
        private readonly ILogger _logger;
        private readonly ProductItemHubServices _productItemHubServices;
        private readonly EpicSchemaDbContext _dbContext;

        public ProductItemHub(
            ILogger<ProductItemHub> logger,
            ProductItemHubServices productItemHubServices,
            EpicSchemaDbContext dbContext)
        {
            _logger = logger;
            _productItemHubServices = productItemHubServices;
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

                case UserTypes.PARTNER:
                case UserTypes.ROOT_PARTNER:
                    int partnerId = int.Parse(Context.User.FindFirst(ClaimTypes.PartnerId).Value);
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.PrefixPartner + partnerId);
                    break;

                case UserTypes.TRADING_PROVIDER:
                case UserTypes.ROOT_TRADING_PROVIDER:
                    int tradingProviderId = int.Parse(Context.User.FindFirst(ClaimTypes.TradingProviderId).Value);
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.PrefixTradingProvider + tradingProviderId);
                    break;

                case UserTypes.INVESTOR:
                    //khách cá nhân cho chung kênh với đại lý nếu có quan hệ liên quan
                    //(tuy nhiên gặp vấn đề nếu khách này chưa có quan hệ với đại lý) => cách giải quyết là client sẽ gọi hàm join vào group này
                    int investorId = int.Parse(Context.User.FindFirst(ClaimTypes.InvestorId).Value);
                    var tradingProviderIds = _dbContext.InvestorTradingProviders
                        .Where(i => i.InvestorId == investorId && i.Deleted == YesNo.NO)
                        .Select(i => i.TradingProviderId)
                        .ToList();
                    foreach (var tradingProvider in tradingProviderIds)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.PrefixInvestorTradingProvider + tradingProvider);
                    }
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.Investor);
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

        /// <summary>
        /// Tài khoản app join vào group của trading để listen sự thay đổi của các sản phẩm tại đại lý đó
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public async Task InvestorJoinTrading(int tradingProviderId)
        {
            var userType = Context.User.FindFirst(ClaimTypes.UserType)?.Value;
            if (userType == UserTypes.INVESTOR)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, UserGroupNames.PrefixInvestorTradingProvider + tradingProviderId);
            }
        }

        //public ChannelReader<RstProductItemStatusSignalRDto> StreamProductItem()
        //{
        //    return _productItemHubServices.StreamProductItem()
        //        .AsChannelReader(SignalRConfig.MaxBufferSize);
        //}
    }
}
