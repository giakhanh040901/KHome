using AutoMapper;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using EPIC.InvestRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EPIC.InvestDomain.Implements
{
    public class ProjectJuridicalFileServices : IProjectJuridicalFileServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ProjectJuridicalFileRepository _juridicalFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ProjectJuridicalFileServices(
            ILogger<ProjectJuridicalFileServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _juridicalFileRepository = new ProjectJuridicalFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public ProjectJuridicalFile Add(CreateProjectJuridicalFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var file = new ProjectJuridicalFile()
            {
                ProjectId = input.ProjectId,
                Name = input.Name,
                Url = input.Url,
                CreatedBy = username
            };
            return _juridicalFileRepository.Add(file);
        }

        public int Delete(int id)
        {
            return _juridicalFileRepository.Delete(id);
        }

        public PagingResult<ProjectJuridicalFile> FindAll(int projectId, int pageSize, int pageNumber, string keyword)
        {
            return _juridicalFileRepository.FindAll(projectId, pageSize, pageNumber, keyword);
        }

        public ProjectJuridicalFile FindById(int id)
        {
            return _juridicalFileRepository.FindById(id);
        }

        public int Update(int id, UpdateProjectJuridicalFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            return _juridicalFileRepository.Update(new ProjectJuridicalFile
            {
                Id = id,
                ProjectId = input.ProjectId,
                Name = input.Name,
                Url = input.Url,
                ModifiedBy = username
            });
        }
    }
}
