using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.JuridicalFile;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EPIC.BondDomain.Implements
{
    public class BondJuridicalFileServices : IBondJuridicalFileService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondJuridicalFileRepository _juridicalFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BondJuridicalFileServices(
            ILogger<BondDistributionContractService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _juridicalFileRepository = new BondJuridicalFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public BondJuridicalFile Add(CreateJuridicalFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var file = new BondJuridicalFile()
            {
                BondId = input.ProductBondId,
                Name = input.Name,
                Url = input.Url,
                CreatedBy = username
            };
            return _juridicalFileRepository.Add(file);
        }

        public int DeleteJuridicalFile(int id)
        {
            return _juridicalFileRepository.DeleteJuridicalFile(id);
        }

        public PagingResult<BondJuridicalFile> FindAllJuridicalFile(int productBondId, int pageSize, int pageNumber, string keyword)
        {
            return _juridicalFileRepository.FindAllJuridicalFile(productBondId, pageSize, pageNumber, keyword);
        }

        public BondJuridicalFile FindJuridicalFileById(int id)
        {
            return _juridicalFileRepository.FindJuridicalFileById(id);
        }



        public int JuridicalFileUpdate(int id, UpdateJuridicalFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            return _juridicalFileRepository.JuridicalFileUpdate(new BondJuridicalFile
            {
                Id = id,
                Name = input.Name,
                Url = input.Url,
            });
        }

    }
}
