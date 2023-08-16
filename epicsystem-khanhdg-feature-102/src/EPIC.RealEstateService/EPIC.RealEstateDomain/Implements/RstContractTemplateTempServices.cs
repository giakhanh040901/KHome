using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstContractTemplateTempServices : IRstContractTemplateTempServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstContractTemplateTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstContractTemplateTempEFRepository _rstContractTemplateTempEFRepository;

        public RstContractTemplateTempServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstContractTemplateTempServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _rstContractTemplateTempEFRepository = new RstContractTemplateTempEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstContractTemplateTempDto Add(CreateRstContractTemplateTempDto input)
        {
            //Thoong tin đại lý, đối tác
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, userType = {userType}, username = {username}, partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");

            var contractTemplateTemp = _mapper.Map<RstContractTemplateTemp>(input);

            var insert = _rstContractTemplateTempEFRepository.Add(contractTemplateTemp, username, tradingProviderId,partnerId);
            _dbContext.SaveChanges();
            return _mapper.Map<RstContractTemplateTempDto>(insert);
        }

        /// <summary>
        /// Thay đổi trạng thái mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstContractTemplateTempDto ChangeStatus(int id)
        {
            //Thoong tin đại lý, đối tác
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            _logger.LogInformation($"{nameof(ChangeStatus)}: Id = {id}, userType = {userType}, username = {username}, partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");

            var contractTemplateTemp = _rstContractTemplateTempEFRepository.FindById(id, tradingProviderId).ThrowIfNull<RstContractTemplateTemp>(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
            var status = ContractTemplateStatus.ACTIVE;
            if (contractTemplateTemp.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var changeStatus = _rstContractTemplateTempEFRepository.ChangeStatus(id, status, tradingProviderId, partnerId);
            _dbContext.SaveChanges();
            return _mapper.Map<RstContractTemplateTempDto>(changeStatus);
        }

        /// <summary>
        /// Xóa mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            _logger.LogInformation($"{nameof(Delete)}: Id = {id}, userType = {userType}, username = {username}, partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");

            var contractTemplateTemp = _rstContractTemplateTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId && x.PartnerId == partnerId).ThrowIfNull<RstContractTemplateTemp>(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
           
            contractTemplateTemp.Deleted = YesNo.YES;
            contractTemplateTemp.ModifiedBy = username;
            contractTemplateTemp.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        public List<RstContractTemplateTempDto> FindAll()
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            _logger.LogInformation($"{nameof(FindAll)}: userType = {userType}, username = {username}, partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");

            var contractTemplateTemps = _rstContractTemplateTempEFRepository.FindAll(tradingProviderId, partnerId);
            return _mapper.Map<List<RstContractTemplateTempDto>>(contractTemplateTemps);
        }

        public PagingResult<RstContractTemplateTempDto> FindAllContractTemplateTemp(FilterRstContractTemplateTempDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}, userType = {userType}, username = {username}, partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");

            var resultPaging = new PagingResult<RstContractTemplateTempDto>();

            var find = _rstContractTemplateTempEFRepository.FindAllContractTemplateTemp(input, tradingProviderId, partnerId);

            resultPaging.Items = _mapper.Map<List<RstContractTemplateTempDto>>(find.Items);
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// Lấy mẫu hợp đồng của đối tác và đại lý cho biễu mẫu hợp đồng của mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstContractTemplateTempDto> GetAllContractTemplateTemp(FilterRstContractTemplateTempDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            _logger.LogInformation($"{nameof(GetAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}, userType = {userType}, username = {username}, partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");

            var resultPaging = new PagingResult<RstContractTemplateTempDto>();

            var find = _rstContractTemplateTempEFRepository.GetAllContractTemplateTemp(input, tradingProviderId, partnerId);

            resultPaging.Items = _mapper.Map<List<RstContractTemplateTempDto>>(find.Items);
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// Tìm mẫu hợp đồng mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstContractTemplateTempDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            _logger.LogInformation($"{nameof(FindById)}: id={id}, userType = {userType}, , partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");


            var contractTemplateTemp = _rstContractTemplateTempEFRepository.Entity.FirstOrDefault(x => x.Id == id 
                && x.TradingProviderId == tradingProviderId && x.PartnerId == partnerId && x.Deleted == YesNo.NO)
                .ThrowIfNull<RstContractTemplateTemp>(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
            return _mapper.Map<RstContractTemplateTempDto>(contractTemplateTemp);
        }

        /// <summary>
        /// Tìm danh sách mẫu hợp đồng mẫu không phân trang
        /// </summary>
        /// <param name="contractSource"></param>
        /// <returns></returns>
        public List<RstContractTemplateTempDto> GetAllContractTemplateTemp(int? contractSource = null)
        {
            _logger.LogInformation($"{nameof(GetAllContractTemplateTemp)}: contractSource = {contractSource}");
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            _logger.LogInformation($"{nameof(GetAllContractTemplateTemp)}: contractSource = {contractSource}, userType = {userType}, , partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");


            var contractTemplateTemps = _rstContractTemplateTempEFRepository.GetAllContractTemplateTemp(contractSource, tradingProviderId, partnerId);
            return _mapper.Map<List<RstContractTemplateTempDto>>(contractTemplateTemps);
        }

        /// <summary>
        /// Cập nhật mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstContractTemplateTempDto Update(UpdateRstContractTemplateTempDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, userType = {userType}, username = {username}, partnerId = {partnerId}, tradingProviderId = {tradingProviderId}");


            var contractTemplateTemp = _rstContractTemplateTempEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstContractTemplateTemp>(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
            var updateContractTepmlateTemp = _rstContractTemplateTempEFRepository.Update(_mapper.Map<RstContractTemplateTemp>(input), username, tradingProviderId, partnerId);
            _dbContext.SaveChanges();
            return _mapper.Map<RstContractTemplateTempDto>(updateContractTepmlateTemp);
        }
    }
}
