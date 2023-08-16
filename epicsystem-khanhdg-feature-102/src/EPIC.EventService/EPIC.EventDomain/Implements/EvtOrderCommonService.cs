using AutoMapper;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Implements
{
    public class EvtOrderCommonService : IEvtOrderCommonService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EvtOrderCommonService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EvtEventEFRepository _evtEventEFRepository;
        private readonly EvtEventDetailEFRepository _evtEventDetailEFRepository;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;

        public EvtOrderCommonService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<EvtOrderCommonService> logger,
            IHttpContextAccessor httpContextAccessor,
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices
           )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _evtEventEFRepository = new EvtEventEFRepository(_dbContext, _logger);
            _evtEventDetailEFRepository = new EvtEventDetailEFRepository(_dbContext, _logger);
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
        }

        /// <summary>
        /// gen mảng ticket code
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public List<string> ListTicketCodeGenerate(int number, int length)
        {
            var result = new List<string>();

            while (result.Count < number)
            {
                var charNumberRandom = RandomNumberUtils.RandomCharNumber(length, ContractCodes.EVENT + "-");

                var checkTicketCode = _dbContext.EvtOrderTicketDetails.Any(o => o.TicketCode == charNumberRandom);
                if (!checkTicketCode)
                {
                    result.Add(charNumberRandom);
                }
            }

            return result;
        }

        /// <summary>
        /// kiem tra so ve con lai
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public bool CheckRemainingTickets(int ticketId)
        {
            var remainingTickets = _dbContext.EvtTickets.Include(t => t.OrderDetails)
                                            .ThenInclude(od => od.Order)
                                            .Where(t => t.Id == ticketId
                                                && t.Deleted == YesNo.NO
                                                )
                                            .Select(t => t.Quantity - t.OrderDetails
                                                .Where(od => od.TicketId == t.Id
                                                    && (od.Order.Status == EvtOrderStatus.HOP_LE
                                                        || (od.Order.Status == EvtOrderStatus.CHO_THANH_TOAN
                                                            && od.Order.ExpiredTime >= DateTime.Now)))
                                                .Select(od => od.Quantity)
                                                .Sum())
                                            .FirstOrDefault();
            return remainingTickets > 0;
        }

        /// <summary>
        /// kiem tra so ve con lai trên app
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public bool AppCheckRemainingTickets(int ticketId)
        {
            var remainingTickets = _dbContext.EvtTickets.Include(t => t.OrderDetails)
                                            .ThenInclude(od => od.Order)
                                            .Where(t => t.Id == ticketId
                                                && DateTime.Now <= t.EndSellDate
                                                && t.Deleted == YesNo.NO
                                                )
                                            .Select(t => t.Quantity - t.OrderDetails
                                                .Where(od => od.TicketId == t.Id
                                                    && (od.Order.Status == EvtOrderStatus.HOP_LE
                                                        || (od.Order.Status == EvtOrderStatus.CHO_THANH_TOAN
                                                            && od.Order.ExpiredTime >= DateTime.Now)))
                                                .Select(od => od.Quantity)
                                                .Sum())
                                            .FirstOrDefault();
            return remainingTickets > 0;
        }

        /// <summary>
        /// tạo contract code
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string ContractCode(int orderId)
        {
            return ContractCodes.EVENT + orderId.ToString().PadLeft(8, '0');
        }

        /// <summary>
        /// sinh contractCodeGen
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GenContractCode(OrderContractCodeDto input)
        {
            List<ConfigContractCodeDto> configContractCodes = new();
            var configContractCodeDetails = _dbContext.EvtConfigContractCodeDetails.Where(d => d.ConfigContractCodeId == input.ConfigContractCodeId).OrderBy(o => o.SortOrder);

            string contractCode = null;
            foreach (var item in configContractCodeDetails)
            {
                string value = null;
                if (item.Key == ConfigContractCode.ORDER_ID)
                {
                    value = input.OrderId.ToString();
                }
                if (item.Key == ConfigContractCode.FIX_TEXT)
                {
                    value = item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.BUY_DATE && input.BuyDate != null)
                {
                    value = input.BuyDate.Value.ToString("ddMMyyyy");
                }
                else if (item.Key == ConfigContractCode.INVEST_DATE && input.InvestDate != null)
                {
                    value = input.InvestDate.Value.ToString("ddMMyyyy");
                }
                else if (item.Key == ConfigContractCode.EVENT_CODE && input.EventCode != null)
                {
                    value = item.Value.ToUnSign();
                }

                configContractCodes.Add(new ConfigContractCodeDto { Key = item.Key, Value = value });
            }
            contractCode = ConfigContractCode.GenContractCode(configContractCodes);
            return contractCode;
        }
    }
}
