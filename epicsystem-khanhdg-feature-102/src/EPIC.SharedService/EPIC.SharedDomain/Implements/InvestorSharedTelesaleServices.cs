using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.GarnerRepositories;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestRepositories;
using EPIC.SharedDomain.Interfaces;
using EPIC.SharedEntities.Dto.InvestorTelesale;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.SharedDomain.Implements
{
    public class InvestorSharedTelesaleServices : IInvestorSharedTelesaleServices
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly IMapper _mapper;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;

        public InvestorSharedTelesaleServices(EpicSchemaDbContext dbContext,
            ILogger<InvestorSharedTelesaleServices> logger,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _dbContext = dbContext;
            _mapper = mapper;
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _investOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, logger);
        }

        public object FindInvestInfo(string idNo)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Tìm danh sách hợp đồng đang đầu tư của invest + garner theo số giấy tờ của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public InvestorTelesaleDto FindActiveInfo(FilterInvestorTelesaleDto input)
        {
            _logger.LogInformation($"{nameof(FindActiveInfo)}: input = {JsonSerializer.Serialize(input)}");
            var identificationFind = _investorEFRepository.FindIdentificationByIdNo(input.IdNo).ThrowIfNull<InvestorIdentification>(_dbContext, Utils.ErrorCode.InvestorIdentificationNotFound);
            var result = new InvestorTelesaleDto();
            result.IdNo = identificationFind.IdNo;
            result.Fullname = identificationFind.Fullname;
            var cifCodeFind = _cifCodeEFRepository.Entity.FirstOrDefault(c => c.InvestorId == identificationFind.InvestorId);
            
            //Invest info
            var investOrderList = _investOrderEFRepository.Entity
                .Where(o => o.CifCode == cifCodeFind.CifCode && o.Status == OrderStatus.DANG_DAU_TU 
                && (input.StartDate == null || input.StartDate <= o.BuyDate) 
                && (input.EndDate == null || input.EndDate >= o.BuyDate));
            var investInfoList = new List<InvestInfoDto>();
            foreach (var investOrder in investOrderList)
            {
                var order = new InvestInfoDto()
                {
                    BuyDate = investOrder.BuyDate,
                    InitTotalValue = investOrder.InitTotalValue,
                };
                investInfoList.Add(order);

                order.Policy = new InvestPolicyDto();
                order.PolicyDetail = new InvestPolicyDetailDto();

                var policyFind = _policyRepository.FindPolicyById(investOrder.PolicyId, investOrder.TradingProviderId);
                if (policyFind == null)
                {
                    continue;
                }
                order.Policy.Code = policyFind.Code;
                order.Policy.Name = policyFind.Name;
                
                var policyDetailFind = _policyRepository.FindPolicyDetailById(investOrder.PolicyDetailId, investOrder.TradingProviderId);
                if (policyDetailFind == null)
                {
                    continue;
                }
                order.PolicyDetail.PeriodQuantity = policyDetailFind.PeriodQuantity;
                order.PolicyDetail.PeriodType = policyDetailFind.PeriodType;
            }
            result.InvestInfo = investInfoList;

            //Garner info
            var garnerOrderList = _garnerOrderEFRepository.Entity
                .Where(o => o.CifCode == cifCodeFind.CifCode && o.Status == OrderStatus.DANG_DAU_TU 
                && (input.StartDate == null || input.StartDate <= o.BuyDate)
                && (input.EndDate == null || input.EndDate >= o.BuyDate));
            var garnerInfoList = new List<GarnerInfoDto>();
            foreach (var garnerOrder in garnerOrderList)
            {
                var order = new GarnerInfoDto()
                {
                    BuyDate = garnerOrder.BuyDate,
                    InitTotalValue = garnerOrder.InitTotalValue,
                };
                garnerInfoList.Add(order);

                order.Policy = new GarnerPolicyDto();
                order.PolicyDetail = new GarnerPolicyDetailDto();

                var policyFind = _garnerPolicyEFRepository.FindById(garnerOrder.PolicyId, garnerOrder.TradingProviderId);
                if (policyFind == null)
                {
                    continue;
                }
                order.Policy.Code = policyFind.Code;
                order.Policy.Name = policyFind.Name;

                var policyDetailFind = _garnerPolicyDetailEFRepository.FindById(garnerOrder.PolicyDetailId ?? 0, garnerOrder.TradingProviderId);
                if (policyDetailFind == null)
                {
                    continue;
                }
                order.PolicyDetail.PeriodQuantity = policyDetailFind.PeriodQuantity;
                order.PolicyDetail.PeriodType = policyDetailFind.PeriodType;
            }
            result.GarnerInfo = garnerInfoList;

            return result;
        }
    }
}
