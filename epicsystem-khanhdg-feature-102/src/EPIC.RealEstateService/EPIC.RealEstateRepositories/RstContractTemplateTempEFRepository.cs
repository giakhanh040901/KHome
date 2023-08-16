using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using EPIC.RealEstateEntities.DataEntities;
using System.Text.Json;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.Utils.ConstantVariables.RealEstate;

namespace EPIC.RealEstateRepositories
{
    public class RstContractTemplateTempEFRepository : BaseEFRepository<RstContractTemplateTemp>
    {
        public RstContractTemplateTempEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstContractTemplateTemp.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstContractTemplateTemp Add(RstContractTemplateTemp input, string username, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}, parnerId = {partnerId}");

            return _dbSet.Add(new RstContractTemplateTemp()
            {
                Id = (int)NextKey(),
                Name = input.Name,
                TradingProviderId = tradingProviderId,
                PartnerId = partnerId,
                ContractType = input.ContractType,
                ContractSource = input.ContractSource,
                FileInvestor = input.FileInvestor,
                FileBusinessCustomer = input.FileBusinessCustomer,
                Description = input.Description,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstContractTemplateTemp Update(RstContractTemplateTemp input, string username, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username},tradingProviderId = {tradingProviderId}, parnerId = {partnerId}");
            var contractTemplateTemp = _dbSet.FirstOrDefault(p => p.Id == input.Id && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) 
                                                                                   && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO) ;
            if (contractTemplateTemp != null)
            {
                contractTemplateTemp.Name = input.Name;
                contractTemplateTemp.ContractType = input.ContractType;
                contractTemplateTemp.ContractSource = input.ContractSource;
                contractTemplateTemp.FileInvestor = input.FileInvestor;
                contractTemplateTemp.FileBusinessCustomer = input.FileBusinessCustomer;
                contractTemplateTemp.Description = input.Description;
                contractTemplateTemp.ModifiedBy = username;
                contractTemplateTemp.ModifiedDate = DateTime.Now;
            }
            return contractTemplateTemp;
        }

        public RstContractTemplateTemp FindById(int id, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}, tradingproviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && (partnerId == null || d.PartnerId == partnerId) && d.Deleted == YesNo.NO);
        }

        public List<RstContractTemplateTemp> FindAll(int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(Update)} : tradingProviderId =  {tradingProviderId}, parnerId =  {partnerId}");

            var contractTemplateTemps = _dbSet.Where(pt => (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) 
                                                    && (partnerId == null || pt.PartnerId == partnerId) && pt.Deleted == YesNo.NO).OrderByDescending(e => e.Id).ToList();
            return contractTemplateTemps;
        }

        public PagingResult<RstContractTemplateTemp> FindAllContractTemplateTemp(FilterRstContractTemplateTempDto input, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            PagingResult<RstContractTemplateTemp> result = new();
            IQueryable<RstContractTemplateTemp> contractTemplateTempQuery = _dbSet.Where(r => r.Deleted == YesNo.NO && (tradingProviderId == null || r.TradingProviderId == tradingProviderId)
                                                                                         && (partnerId == null || r.PartnerId == partnerId)
                                                                                         && (input.CreatedDate == null || (r.CreatedDate != null && input.CreatedDate.Value.Date == r.CreatedDate.Value.Date)))
                .OrderByDescending(o => o.Id);

            if (input.ContractSource == ContractSources.ONLINE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractSource == ContractSources.ONLINE);
            }
            else if (input.ContractSource == ContractSources.OFFLINE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractSource == ContractSources.OFFLINE);
            }

            if (input.Status == Status.ACTIVE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.Status == Status.ACTIVE);
            }
            else if (input.Status == Status.INACTIVE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.Status == Status.INACTIVE);
            }

            if (input.ContractType == ContractTypes.DAT_LENH)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.DAT_LENH);
            }
            else if (input.ContractType == ContractTypes.RUT_TIEN)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.RUT_TIEN);
            }
            else if (input.ContractType == ContractTypes.TAI_TUC_GOC)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.TAI_TUC_GOC);
            }
            else if (input.ContractType == ContractTypes.RUT_TIEN_APP)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.RUT_TIEN_APP);
            }

            if (input.Keyword != null)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(p => p.Name.Contains(input.Keyword));
            }

            result.TotalItems = contractTemplateTempQuery.Count();

            if (input.PageSize != -1)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = contractTemplateTempQuery;
            return result;
        }

        public PagingResult<RstContractTemplateTemp> GetAllContractTemplateTemp(FilterRstContractTemplateTempDto input, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(GetAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            PagingResult<RstContractTemplateTemp> result = new();
            IQueryable<RstContractTemplateTemp> contractTemplateTempQuery = _dbSet.Where(r => r.Deleted == YesNo.NO
                                                                                && (input.CreatedDate == null || (r.CreatedDate != null && input.CreatedDate.Value.Date == r.CreatedDate.Value.Date)))
                                                                                .OrderByDescending(o => o.Id);

            if (input.Type == RstOpenSellContractTypes.TRADING_PROVIDER)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.TradingProviderId == tradingProviderId);
            }

            else if (input.Type == RstOpenSellContractTypes.PARTNER)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => _epicSchemaDbContext.TradingProviderPartners.Where(o => o.TradingProviderId == tradingProviderId).Select(o => o.PartnerId).Contains(c.PartnerId.Value));
            }

            if (input.ContractSource == ContractSources.ONLINE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractSource == ContractSources.ONLINE);
            }
            else if (input.ContractSource == ContractSources.OFFLINE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractSource == ContractSources.OFFLINE);
            }

            if (input.Status == Status.ACTIVE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.Status == Status.ACTIVE);
            }
            else if (input.Status == Status.INACTIVE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.Status == Status.INACTIVE);
            }

            if (input.ContractType == ContractTypes.DAT_LENH)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.DAT_LENH);
            }
            else if (input.ContractType == ContractTypes.RUT_TIEN)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.RUT_TIEN);
            }
            else if (input.ContractType == ContractTypes.TAI_TUC_GOC)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.TAI_TUC_GOC);
            }
            else if (input.ContractType == ContractTypes.RUT_TIEN_APP)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.RUT_TIEN_APP);
            }

            if (input.Keyword != null)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(p => p.Name.Contains(input.Keyword));
            }

            result.TotalItems = contractTemplateTempQuery.Count();

            if (input.PageSize != -1)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = contractTemplateTempQuery;
            return result;
        }

        /// <summary>
        /// Tìm mẫu hợp đồng mẫu không phân trang
        /// </summary>
        /// <param name="contractSource"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<RstContractTemplateTemp> GetAllContractTemplateTemp(int? contractSource = null, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: contractSource = {contractSource}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var contractTemplateTemps = _dbSet.Where(pt => (contractSource == null || pt.ContractSource == contractSource) 
                                                          && (tradingProviderId == null || pt.TradingProviderId == tradingProviderId)
                                                          && (partnerId == null || pt.PartnerId == partnerId)
                                                          && pt.Deleted == YesNo.NO && pt.Status == Status.ACTIVE).OrderByDescending(o => o.Id).ToList();
            return contractTemplateTemps;
        }

        /// <summary>
        /// Tìm mẫu hợp đồng mẫu không phân trang cho app
        /// </summary>
        /// <param name="openSellDetailId"></param>
        /// <param name="contractSource"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstOpenSellContractTemplateDto> GetAllForApp(int openSellDetailId, int? contractType = null, int? contractSource = null, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: contractSource = {contractSource}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var contractTemplateTemps = from openSellDetail in _epicSchemaDbContext.RstOpenSellDetails
                                        join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                        join openSellContractTemplate in _epicSchemaDbContext.RstOpenSellContractTemplates on openSell.Id equals openSellContractTemplate.OpenSellId
                                        join contractTemplateTemp in _dbSet on openSellContractTemplate.ContractTemplateTempId equals contractTemplateTemp.Id
                                        join distributionPolicy in _epicSchemaDbContext.RstDistributionPolicys on openSellContractTemplate.DistributionPolicyId equals distributionPolicy.Id
                                        where openSellDetail.Id == openSellDetailId
                                        && openSellContractTemplate.Status == Status.ACTIVE
                                        && openSellContractTemplate.Deleted == YesNo.NO
                                        && contractTemplateTemp.Deleted == YesNo.NO
                                        && (contractSource == null || contractTemplateTemp.ContractSource == contractSource)
                                        && (contractType == null || contractTemplateTemp.ContractType == contractType)
                                        select new RstOpenSellContractTemplateDto ()
                                        {
                                            Id = openSellContractTemplate.Id,
                                            ContractTemplateTempId = contractTemplateTemp.Id,
                                            ContractTemplateTempName = contractTemplateTemp.Name,
                                            Status = openSellContractTemplate.Status
                                        };
          
            return contractTemplateTemps.ToList();
        }

        public RstContractTemplateTemp ChangeStatus(int id, string status, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");
            var contractTemplateTemp = _dbSet.FirstOrDefault(p => p.Id == id && (tradingProviderId == null || p.TradingProviderId == tradingProviderId)
                                                                    && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
            if (contractTemplateTemp != null)
            {
                contractTemplateTemp.Status = status;
            }
            return contractTemplateTemp;
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng đặt lệnh theo chính sách 
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <param name="type">Dành cho nhà đầu tư cá nhân hay chuyên nghiệp</param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstContractTemplateTempForOrderDto FindByIdForUpdateContractFile(int contractTemplateId, string customerType, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindByIdForUpdateContractFile)}: contractTemplateId = {contractTemplateId}, customerType = {customerType}, tradingproviderId = {tradingProviderId}");

            var contractTemplates = from contractTemplate in _dbSet
                                    where (contractTemplate.Id == contractTemplateId && (tradingProviderId == null || contractTemplate.TradingProviderId == tradingProviderId) && contractTemplate.Deleted == YesNo.NO)
                                    select new RstContractTemplateTempForOrderDto
                                    {
                                        ContractSource = contractTemplate.ContractSource,
                                        ContractTemplateUrl = customerType == SharedContractTemplateType.INVESTOR ? contractTemplate.FileInvestor : contractTemplate.FileBusinessCustomer,
                                        TradingProviderId = contractTemplate.TradingProviderId,
                                        ContractTemplateTempId = contractTemplate.Id,
                                        ContractType = contractTemplate.ContractType,
                                        Name = contractTemplate.Name,
                                        FileBusinessCustomer = contractTemplate.FileBusinessCustomer,
                                        FileInvestor = contractTemplate.FileInvestor,
                                        Id = contractTemplate.Id,
                                        Status = contractTemplate.Status,
                                    };
            return contractTemplates.FirstOrDefault();
        }

        /// <summary>
        /// Tìm kiếm danh sách mẫu hợp đồng thông tin mẫu hợp đồng mẫu theo chính sách 
        /// </summary>
        /// <param name="openSellDetailId"></param>
        /// <param name="type">Dành cho nhà đầu tư cá nhân hay chuyên nghiệp</param>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractType">1: Hợp đồng rút vốn; 2: Hợp đồng đầu tư</param>
        /// <param name="displayType">Hiển thị trước hay sau khi duyệt</param>
        /// <param name="contractSource">Online, offline</param>
        /// <returns></returns>
        public List<RstContractTemplateTempForOrderDto> FindAllForUpdateContractFile(int openSellDetailId, string type, int? tradingProviderId = null, int? contractType = null, string displayType = null, int? contractSource = null, string status = null)
        {
            _logger.LogInformation($"{nameof(FindAllForUpdateContractFile)}: openSellDetailId = {openSellDetailId}, tradingproviderId = {tradingProviderId}, contractType = {contractType}, displayType = {displayType}, contractSource = {contractSource}, type = {type}");

            var contractTemplates = from openSellDetail in _epicSchemaDbContext.RstOpenSellDetails
                                    join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                    join openSellContract in _epicSchemaDbContext.RstOpenSellContractTemplates on openSell.Id equals openSellContract.OpenSellId
                                    join contractTemplateTemp in _dbSet on openSellContract.ContractTemplateTempId equals contractTemplateTemp.Id
                                    where (openSellDetail.Id == openSellDetailId && (tradingProviderId == null || openSellContract.TradingProviderId == tradingProviderId) && openSellContract.Deleted == YesNo.NO 
                                    && contractTemplateTemp.Status == Status.ACTIVE
                                    && (status == null || openSellContract.Status == status)
                                    && (contractType == null || contractTemplateTemp.ContractType == contractType)
                                    && (contractSource == null || (contractTemplateTemp.ContractSource == contractSource || contractTemplateTemp.ContractSource == ContractSources.ALL))
                                    && (displayType == null || openSellContract.DisplayType == displayType))
                                    select new RstContractTemplateTempForOrderDto
                                    {
                                        OpenSellId = openSellContract.OpenSellId,
                                        ContractSource = contractTemplateTemp.ContractSource,
                                        TradingProviderId = openSellContract.TradingProviderId,
                                        ContractTemplateTempId = contractTemplateTemp.Id,
                                        ContractTemplateUrl = type == SharedContractTemplateType.INVESTOR ? contractTemplateTemp.FileInvestor : contractTemplateTemp.FileBusinessCustomer,
                                        ContractType = contractTemplateTemp.ContractType,
                                        DisplayType = openSellContract.DisplayType,
                                        Name = contractTemplateTemp.Name,
                                        FileBusinessCustomer = contractTemplateTemp.FileBusinessCustomer,
                                        FileInvestor = contractTemplateTemp.FileInvestor,
                                        ConfigContractId = openSellContract.ConfigContractCodeId,
                                        EffectiveDate = openSellContract.EffectiveDate,
                                        Id = contractTemplateTemp.Id,
                                        Status = openSellContract.Status,
                                        CreatedDate = openSellContract.CreatedDate
                                    };
            return contractTemplates.ToList();
        }
    }
}
