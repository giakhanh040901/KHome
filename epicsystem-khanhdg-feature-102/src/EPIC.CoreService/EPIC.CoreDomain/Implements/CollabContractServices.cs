using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.CollabContractTemp;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CollabContractTemp;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class CollabContractServices : ICollabContractServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly EpicSchemaDbContext _dbContext;

        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly CollabContractTemplateRepository _collapContractTemplateRepository;
        private readonly CollabContractTemplateEFRepository _collapContractTemplateEFRepository;
        private readonly SaleRepository _saleRepository;
        private readonly SaleCollabContractRepository _saleCollabContractRepository;

        public CollabContractServices(
            EpicSchemaDbContext dbContext,
            ILogger<CollabContractServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
            _httpContext = httpContext;
            _connectionString = databaseOptions.ConnectionString;
            _collapContractTemplateRepository = new CollabContractTemplateRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _saleCollabContractRepository = new SaleCollabContractRepository(_connectionString, _logger);
            _collapContractTemplateEFRepository = new CollabContractTemplateEFRepository(_dbContext, _logger);
        }

        public CollabContractTemp Add(CreateCollabContractTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var collabContractTemp = new CollabContractTemp
            {
                TradingProviderId = tradingProviderId,
                Title = input.Title,
                FileUrl = input.FileUrl,
                Type = input.Type,
                CreatedBy = username
            };
            return _collapContractTemplateRepository.Add(collabContractTemp);
        }

        public PagingResult<ViewCollabContractTempDto> FindAll(FilterCollabContractTempDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            input.TradigProviderId = tradingProviderId;
            return _collapContractTemplateEFRepository.FindAll(input);
        }

        public int Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _collapContractTemplateRepository.Delete(id, tradingProviderId);
        }

        public ViewCollabContractTempDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _collapContractTemplateRepository.FindById(id, tradingProviderId);
            return result;
        }


        public int Update(UpdateCollabContractTempDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _collapContractTemplateRepository.Update(new CollabContractTemp
            {
                Id = body.Id,
                TradingProviderId = tradingProviderId,
                Title = body.Title,
                FileUrl = body.FileUrl,
                Type = body.Type,
                ModifiedBy = username
            });
        }

        public PagingResult<ViewCollabContractTempSaleDto> FindAllBySale(int pageSize, int pageNumber, int saleId, int? tradingProvider)
        {
            var tradingProviderId = tradingProvider ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var sale = _saleRepository.FindById(saleId, tradingProviderId);
            if (sale == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sale: {saleId}"), new FaultCode(((int)ErrorCode.CoreSaleNotFound).ToString()), "");
            }
            string type = CollabContractTempType.INVESTOR;
            if(sale.BusinessCustomerId != null)
            {
                type = CollabContractTempType.BUSINESS_CUSTOMER;
            }
            var contractTemplate = _collapContractTemplateRepository.FindAll(pageSize, pageNumber, null,  null, tradingProviderId, type);
            if (contractTemplate.TotalItems < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng sale tradingProviderId: { tradingProviderId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var result = new PagingResult<ViewCollabContractTempSaleDto>();
            var contractTemplatebyOrders = new List<ViewCollabContractTempSaleDto>();
            int totalItems = 0;
            foreach (var item in contractTemplate.Items)
            {
                var templateContract = new ViewCollabContractTempSaleDto();
                var collabContractFile = _saleCollabContractRepository.Find(saleId, tradingProviderId, item.Id);
                if (collabContractFile != null)
                {
                    //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                    templateContract.Id = item.Id;
                    templateContract.Title = item.Title;
                    templateContract.TradingProviderId = collabContractFile.TradingProviderId;
                    templateContract.CollabContractId = collabContractFile.Id;
                    templateContract.FileTempUrl = collabContractFile.FileTempUrl;
                    templateContract.FileSignatureUrl = collabContractFile.FileSignatureUrl;
                    templateContract.FileScanUrl = collabContractFile.FileScanUrl;
                    templateContract.IsSign = collabContractFile.IsSign;

                }
                else
                {
                    //chưa có file scan nên phải check xem đã xóa và trạng thái
                    if (item.Status != BondPolicyTemplate.DEACTIVE)
                    {
                        templateContract.Id = item.Id;
                        templateContract.Title = item.Title;
                    }
                }
                if (templateContract.Id > 0)
                {
                    contractTemplatebyOrders.Add(templateContract);
                    totalItems += 1;
                }

            }
            result.Items = contractTemplatebyOrders;
            result.TotalItems = totalItems;
            return result;
        }


    }
}
