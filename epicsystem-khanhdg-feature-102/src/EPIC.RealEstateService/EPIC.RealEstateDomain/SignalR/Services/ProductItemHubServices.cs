using EPIC.DataAccess.Base;
using EPIC.RealEstateDomain.SignalR.Hubs;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils.ConstantVariables.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.SignalR.Services
{
    public class ProductItemHubServices
    {
        private readonly ILogger _logger;
        private readonly Subject<RstProductItemStatusSignalRDto> _subjectProductItem = new();
        private readonly IHubContext<ProductItemHub> _hub;

        public ProductItemHubServices(
            ILogger<ProductItemHubServices> logger,
            IHubContext<ProductItemHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }

        public IObservable<RstProductItemStatusSignalRDto> StreamProductItem()
        {
            return _subjectProductItem;
        }

        /// <summary>
        /// Thông báo đến các client thay đổi thông tin product item
        /// </summary>
        /// <param name="productItemId"></param>
        public void BroadcastProductItemUpdate(int productItemId)
        {
            //broadcast đến tài khoản đối tác

            //broadcast đến tài khoản khách cá nhân

            //Hub.Clients.Group()
        }
    }
}
