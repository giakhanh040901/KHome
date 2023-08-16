using AutoMapper;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.InvestDomain.Implements
{
    public class ProjectImageServices : IProjectImageServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ProjectImageRepository _projectImageRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ProjectImageServices(
            ILogger<ProjectImageServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _projectImageRepository = new ProjectImageRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }
        public void Add(CreateProjectImageDto input)
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var projectImage in input.ProjectImages)
                {
                    var projectImageItem = new ProjectImage()
                    {
                        ProjectId = input.ProjectId,
                        Url = projectImage
                    };

                    if (projectImage == null)
                    {
                        throw new FaultException(new FaultReason($" Đường dẫn hình ảnh dự án không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }

                    _projectImageRepository.Add(projectImageItem);
                }
                transaction.Complete();
            }
        }

        public int Delete(int id)
        {
            return _projectImageRepository.Delete(id);
        }

        public List<ProjectImage> FindByProjectId(int projectId)
        {
            var projectImageList = _projectImageRepository.FindByProjectId(projectId);
            var result = new List<ProjectImage>();
            foreach (var projectImage in projectImageList)
            {
                result.Add(projectImage);
            }
            return result;
        }
    }
}
