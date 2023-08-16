using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.ProjectTradingProvider;
using EPIC.InvestRepositories;
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

namespace EPIC.InvestDomain.Implements
{
    public class ProjectTradingProviderServices : IProjectTradingProviderServices
    {
        private readonly ILogger<ProjectTradingProviderServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ProjectTradingProviderRepository _projectTradingProviderRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ProjectTradingProviderServices(ILogger<ProjectTradingProviderServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _projectTradingProviderRepository = new ProjectTradingProviderRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public List<ProjectTradingProviderDto> FindAll(int projectId)
        {
            var result = new List<ProjectTradingProviderDto>();
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var listTradingProvider = _mapper.Map<List<ProjectTradingProviderDto>>(_projectTradingProviderRepository.FindAll(projectId, partnerId));

            foreach (var item in listTradingProvider)
            {
                var tradingItem = _mapper.Map<ProjectTradingProviderDto>(item);
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

        public void UpdateProjectTrading(int projectId, List<CreateProjectTradingProviderDto> input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            using (TransactionScope scope = new TransactionScope())
            {
                //Lấy danh sách đại lý trong dự án
                var listTradingProviders = _projectTradingProviderRepository.FindAll(projectId, partnerId);
                foreach (var item in listTradingProviders)
                {
                    var checkTrading = input.FirstOrDefault(f => f.Id == item.Id);
                    if (checkTrading == null)
                    {
                        _projectTradingProviderRepository.Delete(item.Id);
                    };
                }

                foreach (var item in input)
                {
                    //if (projectFind.HasTotalInvestmentSub == YesNo.YES )
                    // Thêm mới đại lý
                    if (item.Id == 0)
                    {
                        _projectTradingProviderRepository.Add(new ProjectTradingProvider
                        {
                            PartnerId = partnerId,
                            ProjectId = projectId,
                            TradingProviderId = item.TradingProviderId,
                            TotalInvestmentSub = item.TotalInvestmentSub,
                            CreatedBy = username
                        });
                    }
                    //Cập nhật thông tin đại lý
                    else
                    {
                        _projectTradingProviderRepository.Update(new ProjectTradingProvider
                        {
                            Id = item.Id,
                            PartnerId = partnerId,
                            ProjectId = projectId,
                            TradingProviderId = item.TradingProviderId,
                            TotalInvestmentSub = item.TotalInvestmentSub,
                            ModifiedBy = username
                        });
                    }
                }
                scope.Complete();
            }
            _projectTradingProviderRepository.CloseConnection();
        }
    }
}
