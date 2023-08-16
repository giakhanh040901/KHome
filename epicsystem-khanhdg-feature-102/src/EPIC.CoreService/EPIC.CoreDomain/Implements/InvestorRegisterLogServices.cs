using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.InvestorRegistorLog;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class InvestorRegisterLogServices : IInvestorRegisterLogServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestorRegisterLogServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly InvestorRegisterLogEFRepository _investorRegisterLogEFRepository;

        public InvestorRegisterLogServices(EpicSchemaDbContext dbContext,
                 DatabaseOptions databaseOptions,
                 IMapper mapper,
                 ILogger<InvestorRegisterLogServices> logger,
                 IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _investorRegisterLogEFRepository = new InvestorRegisterLogEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm log khi tài khoản được đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public InvestorRegisterLog Add(CreateInvestorRegisterLogDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, ipAddress = {ipAddress}");
            var insert = _mapper.Map<InvestorRegisterLog>(input);
            insert.IpRequest = ipAddress;
            insert.CreatedBy = username;
            var result = _investorRegisterLogEFRepository.Add(insert);
            _dbContext.SaveChanges();

            return result;
        }
    }
}
