using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class ProjectJuridicalFileRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_JURIDICAL_FILE_ADD = "PKG_INV_PROJECT_JURIDICAL_FILE.PROC_JURIDICAL_FILE_ADD";
        private const string PROC_JURIDICAL_FILE_GET = "PKG_INV_PROJECT_JURIDICAL_FILE.PROC_JURIDICAL_FILE_GET";
        private const string PROC_JURIDICAL_FILE_DELETE = "PKG_INV_PROJECT_JURIDICAL_FILE.PROC_JURIDICAL_FILE_DELETE";
        private const string PROC_JURIDICAL_FILE_UPDATE = "PKG_INV_PROJECT_JURIDICAL_FILE.PROC_JURIDICAL_FILE_UPDATE";
        private const string PROC_JURIDICAL_FILE_GET_ALL = "PKG_INV_PROJECT_JURIDICAL_FILE.PROC_JURIDICAL_FILE_GET_ALL";

        public ProjectJuridicalFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PagingResult<ProjectJuridicalFile> FindAll(int projectId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ProjectJuridicalFile>(PROC_JURIDICAL_FILE_GET_ALL, new
            {
                pv_PROJECT_ID = projectId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public ProjectJuridicalFile FindById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ProjectJuridicalFile>(PROC_JURIDICAL_FILE_GET, new
            {
                pv_ID = id,
            });
            return result;
        }

        public ProjectJuridicalFile Add(ProjectJuridicalFile entity)
        {
            _logger.LogInformation("Add JuridicalFile");
            var result = _oracleHelper.ExecuteProcedureToFirst<ProjectJuridicalFile>(PROC_JURIDICAL_FILE_ADD, new
            {
                pv_PROJECT_ID = entity.ProjectId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int Update(ProjectJuridicalFile entity)
        {
            _logger.LogInformation("Update JuridicalFile");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_JURIDICAL_FILE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_PROJECT_ID = entity.ProjectId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation($"Delete Juridical File ");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_JURIDICAL_FILE_DELETE, new
            {
                pv_ID = id
            });
        }
    }
}
