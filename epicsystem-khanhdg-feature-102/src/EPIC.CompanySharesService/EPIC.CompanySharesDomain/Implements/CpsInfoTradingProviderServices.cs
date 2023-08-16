using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CompanySharesInfoTradingProvider;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsInfoTradingProviderServices : ICpsInfoTradingProviderServices
    {
        private readonly ILogger<CpsInfoTradingProviderServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly CpsInfoTradingProviderRepository _cpsInfoTradingProviderRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly CpsInfoRepository _cpsInfoRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public CpsInfoTradingProviderServices(ILogger<CpsInfoTradingProviderServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _cpsInfoTradingProviderRepository = new CpsInfoTradingProviderRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _cpsInfoRepository = new CpsInfoRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public List<CpsInfoTradingProviderDto> FindAll(int projectId)
        {
            var result = new List<CpsInfoTradingProviderDto>();
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var listTradingProvider = _mapper.Map<List<CpsInfoTradingProviderDto>>(_cpsInfoTradingProviderRepository.FindAll(projectId, partnerId));

            foreach (var item in listTradingProvider)
            {
                var tradingItem = _mapper.Map<CpsInfoTradingProviderDto>(item);
                tradingItem.TradingProvider = new();
                tradingItem.TradingProvider = _tradingProviderRepository.FindTradingProviderById(item.TradingProviderId);
                if (tradingItem.TradingProvider != null)
                {
                    tradingItem.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(_businessCustomerRepository.FindBusinessCustomerById(tradingItem.TradingProvider.BusinessCustomerId ?? 0));
                }
                result.Add(tradingItem);
            }
            return result;
        }

        public void UpdateProjectTrading(int CpsInfoId, List<CreateCpsInfoTradingProviderDto> input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            using (TransactionScope scope = new TransactionScope())
            {
                //Lấy danh sách đại lý trong dự án
                var listTradingProviders = _cpsInfoTradingProviderRepository.FindAll(CpsInfoId, partnerId);
                foreach (var item in listTradingProviders)
                {
                    var checkTrading = input.FirstOrDefault(f => f.Id == item.Id);
                    if (checkTrading == null)
                    {
                        _cpsInfoTradingProviderRepository.Delete(item.Id);
                    };
                }

                var CPSInfoFind = _cpsInfoRepository.FindById(CpsInfoId);

                foreach (var item in input)
                {
                    if (CPSInfoFind.HasTotalInvestmentSub == YesNo.YES)
                    {
                        if (item.TotalInvestmentSub == null)
                        {
                            throw new ArgumentException("Hạn mức đầu tư không được bỏ trống");
                        }
                    }
                    // Thêm mới đại lý
                    if (item.Id == 0)
                    {
                        _cpsInfoTradingProviderRepository.Add(new CpsInfoTradingProvider
                        {
                            PartnerId = partnerId,
                            CpsInfoId = CpsInfoId,
                            TradingProviderId = item.TradingProviderId,
                            TotalInvestmentSub = item.TotalInvestmentSub,
                            CreatedBy = username
                        });
                    }
                    //Cập nhật thông tin đại lý
                    else
                    {
                        _cpsInfoTradingProviderRepository.Update(new CpsInfoTradingProvider
                        {
                            Id = item.Id,
                            PartnerId = partnerId,
                            CpsInfoId = CpsInfoId,
                            TradingProviderId = item.TradingProviderId,
                            TotalInvestmentSub = item.TotalInvestmentSub,
                            ModifiedBy = username
                        });
                    }
                }
                scope.Complete();
            }
            _cpsInfoTradingProviderRepository.CloseConnection();
        }
    }
}
