using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectFavourite;
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

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectFavouriteServices : IRstProjectFavouriteServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectFavouriteServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectFavouriteEFRepository _rstProjectFavouriteEFRepository;

        public RstProjectFavouriteServices(EpicSchemaDbContext dbContext, IMapper mapper, 
            ILogger<RstProjectFavouriteServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _rstProjectFavouriteEFRepository = new RstProjectFavouriteEFRepository(_dbContext, _logger);
        }

        public RstProjectFavouriteDto AppAddProjectFavourite(CreateRstProjectFavouriteDto input)
        {
            _logger.LogInformation($"{nameof(AppAddProjectFavourite)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            RstProjectFavourite insert = _mapper.Map<RstProjectFavourite>(input);
            insert.InvestorId = investorId;
            var result = _rstProjectFavouriteEFRepository.Add(insert, username);
            _dbContext.SaveChanges();
            return _mapper.Map<RstProjectFavouriteDto>(result);
        }

        public void AppDeleteProjectFavourite(int openSellId)
        {
            _logger.LogInformation($"{nameof(AppDeleteProjectFavourite)}: openSellId = {openSellId}");
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var projectFavourite = _rstProjectFavouriteEFRepository.Entity.FirstOrDefault(e => e.OpenSellId == openSellId && e.InvestorId == investorId).ThrowIfNull(_dbContext, ErrorCode.RsProjectFavouriteNotFound);
            _dbContext.Remove(projectFavourite);
            _dbContext.SaveChanges();
        }
    }
}
