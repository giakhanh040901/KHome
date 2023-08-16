using AutoMapper;
using EPIC.CoreRepositoryExtensions;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.BlockadeLiberation;
using EPIC.InvestRepositories;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Implements
{
    public class BlockadeLiberationServices : IBlockadeLiberationServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<BlockadeLiberationServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BlockadeLiberationRepository _blockadeLiberationRepository;
        private readonly BlockadeLiberationEFRepository _blockadeLiberationEFRepository;
        private readonly InvestOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public BlockadeLiberationServices(ILogger<BlockadeLiberationServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            EpicSchemaDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _blockadeLiberationRepository = new BlockadeLiberationRepository(_connectionString, _logger);
            _mapper = mapper;
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _blockadeLiberationEFRepository = new BlockadeLiberationEFRepository(_dbContext, _logger);
        }

        public BlockadeLiberation Add(CreateBlockadeLiberationDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var blockadeLiberation = new BlockadeLiberation()
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

        public PagingResult<BlockadeLiberationDto> FindAll(FilterBlockadeLiberationDto input)
        {
            int? tradingProviderId = null;
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                _dbContext.CheckTradingRelationshipPartner(partnerId, input.TradingProviderIds);
            }
            //var blockadeLiberationList = _blockadeLiberationRepository.FindAll(tradingProviderId, input, input.TradingProviderIds);
            input.TradingProviderId = tradingProviderId;
            var blockadeLiberationList = _blockadeLiberationEFRepository.FindAll(input);
            var result = new PagingResult<BlockadeLiberationDto>();
            var items = new List<BlockadeLiberationDto>();
            result.TotalItems = blockadeLiberationList.TotalItems;
            foreach (var blockadeLiberationFind in blockadeLiberationList.Items)
            {
                try
                {
                    var blockadeLiberationItem = _mapper.Map<BlockadeLiberationDto>(blockadeLiberationFind);
                    blockadeLiberationItem.ContractCode = blockadeLiberationFind.Order?.ContractCode;
                    blockadeLiberationItem.TotalValue = blockadeLiberationFind.Order?.TotalValue;

                    var orderContractFiles = _dbContext.InvestOrderContractFile.Where(e => e.OrderId == blockadeLiberationFind.Order.Id && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct();
                    //Nếu các hợp đồng có chung 1 mã hđ được gen ra khi sinh hđ thì sẽ lấy theo contractCodeGen
                    if (orderContractFiles != null && orderContractFiles.Count() == 1) {
                        var contractCode = orderContractFiles.First();
                        blockadeLiberationItem.ContractCodeGen = contractCode;
                    }
                    items.Add(blockadeLiberationItem);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Không tìm thấy thông tin sổ lệnh có id: {blockadeLiberationFind.OrderId}");
                }
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
            return _blockadeLiberationRepository.Update(new BlockadeLiberation
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
