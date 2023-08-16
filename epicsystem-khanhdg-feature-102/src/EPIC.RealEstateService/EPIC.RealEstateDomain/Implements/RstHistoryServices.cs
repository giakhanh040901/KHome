using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.RealEstateRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstHistoryServices : IRstHistoryServices
    {

        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstHistoryServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;
        public RstHistoryServices(
            DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext,
            ILogger<RstHistoryServices> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _mapper = mapper;
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);
        }

        /// <summary>
        /// tìm kiếm lịch sử của các bảng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstHistoryUpdateDto> FindAllHistoryTable(FilterRstHistoryUpdateDto input, int [] uploadTable)
        {
            _logger.LogInformation($"{nameof(FindAllHistoryTable)}: input = {JsonSerializer.Serialize(input)}");
            var resultPaging = new PagingResult<RstHistoryUpdateDto>();
            var history = _rstHistoryUpdateEFRepository.FindAllByTable(input, uploadTable);
            var rstHistory = _mapper.Map<List<RstHistoryUpdateDto>>(history.Items);

            resultPaging.Items = rstHistory;
            resultPaging.TotalItems = history.TotalItems;
            return resultPaging;
        }
    }
}
