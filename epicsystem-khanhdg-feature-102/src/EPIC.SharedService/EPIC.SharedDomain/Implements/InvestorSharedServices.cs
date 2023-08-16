using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.IdentityRepositories;
using EPIC.Notification.Services;
using EPIC.SharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Recognition.FPT;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedDomain.Implements
{
    public class InvestorSharedServices : IInvestorSharedServices
    {
        private readonly ILogger<InvestorSharedServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly InvestNotificationServices _investSendEmailServices;
        private readonly GarnerNotificationServices _garnerSendEmailServices;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly InvestorEFRepository _investorEFRepository;

        public InvestorSharedServices(ILogger<InvestorSharedServices> logger, 
            DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext,
            IHttpContextAccessor httpContext, IMapper mapper,
            InvestNotificationServices investSendEmailServices,
            GarnerNotificationServices garnerSendEmailServices)
        {
            _logger = logger;
            _httpContext = httpContext;
            _mapper = mapper;
            _investSendEmailServices = investSendEmailServices;
            _garnerSendEmailServices = garnerSendEmailServices;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Đổi trạng thái giao nhận hợp đồng từ đang giao thành nhận hợp đồng
        /// </summary>
        public async Task ChangeDeliveryStatusRecevired(string deliveryCode)
        {
            var modifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeDeliveryStatusRecevired)}: deliveryCode = {deliveryCode}, username = {modifiedBy}, investorId = {investorId}");

            var investOrderQuery = (from investOrder in _dbContext.InvOrders
                                    join cifCode in _dbContext.CifCodes on investOrder.CifCode equals cifCode.CifCode
                                    where cifCode.Deleted == YesNo.NO && investOrder.Deleted == YesNo.NO
                                    && cifCode.InvestorId == investorId && investOrder.DeliveryCode == deliveryCode
                                    select investOrder).FirstOrDefault();
            if (investOrderQuery != null)
            {
                if (investOrderQuery.Status != OrderStatus.DANG_DAU_TU) _investorEFRepository.ThrowException(ErrorCode.InvestOrderStatusIsNotActive);
                if (investOrderQuery.DeliveryStatus != DeliveryStatus.DELIVERY) _investorEFRepository.ThrowException(ErrorCode.InvestOrderDeliveryStatusDelivered);
                else
                {
                    investOrderQuery.DeliveryStatus = DeliveryStatus.RECEIVE;
                    investOrderQuery.ReceivedDate = DateTime.Now;
                    investOrderQuery.ReceivedDateModifiedBy = modifiedBy;
                    _dbContext.SaveChanges();
                }
                await _investSendEmailServices.SendNotifyQRContractDelivery(deliveryCode);
                return;
            }

            var garnerOrderQuery = (from garnerOrder in _dbContext.GarnerOrders
                                    join cifCode in _dbContext.CifCodes on garnerOrder.CifCode equals cifCode.CifCode
                                    where cifCode.Deleted == YesNo.NO && garnerOrder.Deleted == YesNo.NO
                                    && cifCode.InvestorId == investorId && garnerOrder.DeliveryCode == deliveryCode
                                    select garnerOrder).FirstOrDefault();
            if (garnerOrderQuery != null)
            {
                if (garnerOrderQuery.Status != OrderStatus.DANG_DAU_TU) _investorEFRepository.ThrowException(ErrorCode.GarnerOrderStatusIsNotActive);
                if (garnerOrderQuery.DeliveryStatus != DeliveryStatus.DELIVERY) _investorEFRepository.ThrowException(ErrorCode.GarnerOrderDeliveryStatusDelivered);
                else
                {
                    garnerOrderQuery.DeliveryStatus = DeliveryStatus.RECEIVE;
                    garnerOrderQuery.ReceivedDate = DateTime.Now;
                    garnerOrderQuery.ReceivedDateModifiedBy = modifiedBy;
                    _dbContext.SaveChanges();
                }
                await _garnerSendEmailServices.SendNotifyQRContractDelivery(deliveryCode, investorId);
                return;
            }

            _investorEFRepository.ThrowException(ErrorCode.CoreInvestorOrderDeliveryCodeNotFound);
            return;
        }
    }
}
