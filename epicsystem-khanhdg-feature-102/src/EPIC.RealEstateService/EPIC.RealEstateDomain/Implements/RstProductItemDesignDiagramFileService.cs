using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.IdentityRepositories;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProducItemDesignDiagramFile;
using EPIC.RealEstateRepositories;
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
    public class RstProductItemDesignDiagramFileService : IRstProductItemDesignDiagramFileService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProductItemDesignDiagramFileService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProductItemDesignDiagramFileEFRepository _rstProductItemDesignDiagramFileEFRepository;

        public RstProductItemDesignDiagramFileService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProductItemDesignDiagramFileService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProductItemDesignDiagramFileEFRepository = new RstProductItemDesignDiagramFileEFRepository(_dbContext, _logger);
        }

        public void Add(CreateRstProductItemDesignDiagramFileDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            foreach (var item in input.Files)
            {
                if (_dbContext.RstProductItemDesignDiagramFiles.FirstOrDefault(e => e.Name.ToLower() == item.Name.ToLower()) == null)
                {
                    var insertItem = new RstProductItemDesignDiagramFile
                    {
                        Id = (int)_rstProductItemDesignDiagramFileEFRepository.NextKey(),
                        ProductItemId = input.ProductItemId,
                        Name = item.Name,
                        FileUrl = item.FileUrl
                    };
                    _dbContext.RstProductItemDesignDiagramFiles.Add(insertItem);
                }
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");
            var fileDesign = _dbContext.RstProductItemDesignDiagramFiles.FirstOrDefault(f => f.Id == id).ThrowIfNull(_dbContext, Utils.ErrorCode.RstProductItemDesignDiagramFileNotFound);
            _dbContext.RstProductItemDesignDiagramFiles.Remove(fileDesign);
            _dbContext.SaveChanges();
        }

        public IEnumerable<RstProductItemDesignDiagramFileDto> GetAll(int productItemId)
        {
            _logger.LogInformation($"{nameof(GetAll)}: ");
            var result = _dbContext.RstProductItemDesignDiagramFiles.Where(f => f.ProductItemId == productItemId)
                .Select(f => new RstProductItemDesignDiagramFileDto
                {
                    Id = f.Id,
                    ProductItemId = productItemId,
                    Name = f.Name,
                    FileUrl = f.FileUrl
                });
            return result;

        }

        public void Update(UpdateRstProductItemDesignDiagramFileDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var fileDesign = _dbContext.RstProductItemDesignDiagramFiles.FirstOrDefault(f => f.Id == input.Id).ThrowIfNull(_dbContext, Utils.ErrorCode.RstProductItemDesignDiagramFileNotFound);
            fileDesign.Name = input.Name;
            fileDesign.FileUrl = input.FileUrl;
            _dbContext.SaveChanges();
        }
    }
}
