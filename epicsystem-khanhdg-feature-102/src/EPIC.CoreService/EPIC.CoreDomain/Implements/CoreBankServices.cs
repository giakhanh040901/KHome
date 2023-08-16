using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class CoreBankServices : ICoreBankServices
    {
        private readonly ILogger<CoreBankServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BankRepository _bankRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public CoreBankServices(ILogger<CoreBankServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
            _httpContext = httpContext;
            _connectionString = databaseOptions.ConnectionString;
            _bankRepository = new BankRepository(_connectionString, _logger);
        }

        /// <summary>
        /// Lấy list bank cho dropdown
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public PagingResult<CoreBank> GetListBank(string keyword)
        {
            return _bankRepository.GetListBank(keyword);
        }
    }
}
