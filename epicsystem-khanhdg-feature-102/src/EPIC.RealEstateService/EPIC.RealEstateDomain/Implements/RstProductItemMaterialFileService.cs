using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProducItemDesignDiagramFile;
using EPIC.RealEstateEntities.Dto.RstProductItemMaterialFile;
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
    public class RstProductItemMaterialFileService : IRstProductItemMaterialFileService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProductItemMaterialFileService> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProductItemMaterialFileEFRepository _rstProductItemMaterialFileEFRepository;

        public RstProductItemMaterialFileService(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProductItemMaterialFileService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProductItemMaterialFileEFRepository = new RstProductItemMaterialFileEFRepository(_dbContext, _logger);
        }

        public void AddMaterialFile(CreateRstProductItemMaterialFileDto input)
        {
            _logger.LogInformation($"{nameof(AddMaterialFile)}: input = {JsonSerializer.Serialize(input)}");
            foreach (var item in input.Files)
            {
                if (_dbContext.RstProductItemMaterialFiles.FirstOrDefault(e => e.Name.ToLower() == item.Name.ToLower()) == null)
                {
                    var insertItem = new RstProductItemMaterialFile
                    {
                        Id = (int)_rstProductItemMaterialFileEFRepository.NextKey(),
                        ProductItemId = input.ProductItemId,
                        Name = item.Name,
                        FileUrl = item.FileUrl
                    };
                    _dbContext.RstProductItemMaterialFiles.Add(insertItem);
                }
            }
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");
            var fileDesign = _dbContext.RstProductItemMaterialFiles.FirstOrDefault(f => f.Id == id).ThrowIfNull(_dbContext, Utils.ErrorCode.RstProductItemMaterialFileNotFound);
            _dbContext.RstProductItemMaterialFiles.Remove(fileDesign);
            _dbContext.SaveChanges();
        }

        public IEnumerable<RstProductItemMaterialFileDto> FindAll(int productItemId)
        {
            _logger.LogInformation($"{nameof(FindAll)}: productItemId = {productItemId}");
            var result = _dbContext.RstProductItemMaterialFiles.Where(f => f.ProductItemId == productItemId)
                .Select(f => new RstProductItemMaterialFileDto
                {
                    Id = f.Id,
                    ProductItemId = productItemId,
                    Name = f.Name,
                    FileUrl = f.FileUrl
                });
            return result;
        }

        public void UpdateMaterialFile(UpdateRstProductItemMaterialFileDto input)
        {
            _logger.LogInformation($"{nameof(UpdateMaterialFile)}: input = {JsonSerializer.Serialize(input)}");
            var fileDesign = _dbContext.RstProductItemMaterialFiles.FirstOrDefault(f => f.Id == input.Id).ThrowIfNull(_dbContext, Utils.ErrorCode.RstProductItemMaterialFileNotFound);
            fileDesign.Name = input.Name;
            fileDesign.FileUrl = input.FileUrl;
            _dbContext.SaveChanges();
        }
    }
}
