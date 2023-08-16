using EPIC.DataAccess;
using EPIC.InvestEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class ProjectImageRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_PROJECT_IMAGE_ADD = "PKG_INV_PROJECT_IMAGE.PROC_PROJECT_IMAGE_ADD";
        private const string PROC_PROJECT_IMAGE_DELETE = "PKG_INV_PROJECT_IMAGE.PROC_PROJECT_IMAGE_DELETE";
        private const string PROC_PROJECT_IMAGE_GET_BY_PROJECT_ID = "PKG_INV_PROJECT_IMAGE.PROC_PROJECT_IMAGE_GET_PROJ_ID";

        public ProjectImageRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public ProjectImage Add(ProjectImage entity)
        {
            _logger.LogInformation("Add Project Image");
            var result = _oracleHelper.ExecuteProcedureToFirst<ProjectImage>(PROC_PROJECT_IMAGE_ADD, new
            {
                pv_PROJECT_ID = entity.ProjectId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation($"Delete Project Image");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_IMAGE_DELETE, new
            {
                pv_ID = id
            });
        }

        public List<ProjectImage> FindByProjectId(int projectId)
        {
            var result = _oracleHelper.ExecuteProcedure<ProjectImage>(PROC_PROJECT_IMAGE_GET_BY_PROJECT_ID, new
            {
                pv_PROJECT_ID = projectId,
            }).ToList();
            return result;
        }
    }
}
