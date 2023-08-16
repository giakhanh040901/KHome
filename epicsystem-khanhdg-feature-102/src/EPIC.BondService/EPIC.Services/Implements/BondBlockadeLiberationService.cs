using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BlockadeLiberation;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondBlockadeLiberationService : IBondBlockadeLiberationService
    {
        private readonly ILogger<BondBlockadeLiberationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BondBlockadeLiberationRepository _blockadeLiberationRepository;
        private readonly BondOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public BondBlockadeLiberationService(ILogger<BondBlockadeLiberationService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _blockadeLiberationRepository = new BondBlockadeLiberationRepository(_connectionString, _logger);
            _mapper = mapper;
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
        }

        public BondBlockadeLiberation Add(CreateBlockadeLiberationDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var blockadeLiberation = new BondBlockadeLiberation()
            {
                TradingProviderId = tradingProviderId,
                Type = input.Type,
                BlockadeDescription = input.BlockadeDescription, 
                BlockadeDate = input.BlockadeDate, 
                OrderId = input.OrderId, 
                Blockader = username,
                CreatedBy = username,
            };
            var result = _blockadeLiberationRepository.Add(blockadeLiberation);
            return result;
        }

        public PagingResult<BlockadeLiberationDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, int? type)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var blockadeLiberationList = _blockadeLiberationRepository.FindAll(tradingProviderId, pageSize, pageNumber, keyword, status, type);
            var result = new PagingResult<BlockadeLiberationDto>();
            var items = new List<BlockadeLiberationDto>();
            result.TotalItems = blockadeLiberationList.TotalItems;
            foreach (var blockadeLiberationFind in blockadeLiberationList.Items)
            {
                var blockadeLiberationItem = _mapper.Map<BlockadeLiberationDto>(blockadeLiberationFind);
                var orderFind = _orderRepository.FindById(blockadeLiberationFind.OrderId, tradingProviderId);
                if (orderFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin sổ lệnh"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                blockadeLiberationItem.ContractCode = orderFind.ContractCode;
                blockadeLiberationItem.TotalValue = orderFind.TotalValue;
                items.Add(blockadeLiberationItem);
            }
            result.Items = items;
            return result;
        }

        public BlockadeLiberationDto FindById(int? id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var blockadeLiberationFind = _blockadeLiberationRepository.FindById(id, tradingProviderId);
            if (blockadeLiberationFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin phong toả giải toả"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            var result = _mapper.Map<BlockadeLiberationDto>(blockadeLiberationFind);
            var orderFind = _orderRepository.FindById(blockadeLiberationFind.OrderId, tradingProviderId);
            if (orderFind != null)
            {
                result.ContractCode = orderFind.ContractCode;
                result.TotalValue = orderFind.TotalValue;
            }
            return result;
        }

        public int Update(UpdateBlockadeLiberationDto entity, int id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _blockadeLiberationRepository.Update(new BondBlockadeLiberation
            {
                Id = id,
                TradingProviderId = tradingProviderId,
                OrderId = entity.OrderId,
                Type = entity.Type,
                BlockadeDescription = entity.BlockadeDescription,
                BlockadeDate = entity.BlockadeDate,
                LiberationDescription = entity.LiberationDescription,
                LiberationDate = entity.LiberationDate,
                Liberator = username,
            });
        }
    }
}
