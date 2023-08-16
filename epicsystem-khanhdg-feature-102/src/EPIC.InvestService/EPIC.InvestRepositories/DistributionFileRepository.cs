using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionFile;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class DistributionFileRepository
    {

        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_DISTRIBUTION_FILE_ADD = "PKG_INV_DISTRIBUTION_FILE.PROC_DISTRIBUTION_FILE_ADD";
        private const string PROC_DISTRIBUTION_FILE_GET = "PKG_INV_DISTRIBUTION_FILE.PROC_DISTRIBUTION_FILE_GET";
        private const string PROC_DISTRIBUTION_FILE_UPDATE = "PKG_INV_DISTRIBUTION_FILE.PROC_DISTRIBUTION_FILE_UPDATE";
        private const string PROC_DISTRIBUTION_FILE_GET_ALL = "PKG_INV_DISTRIBUTION_FILE.PROC_DISTRIBUTION_FILE_GET_ALL";
        private const string PROC_DISTRIBUTION_FILE_DELETE = "PKG_INV_DISTRIBUTION_FILE.PROC_DISTRIBUTION_FILE_DELETE";

        public DistributionFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public DistributionFile Add(DistributionFile entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<DistributionFile>(
                PROC_DISTRIBUTION_FILE_ADD, new
                {
                    pv_DISTRIBUTION_ID = entity.DistributionId,
                    pv_TITLE = entity.Title,
                    pv_FILE_URL = entity.FileUrl,
                    SESSION_USERNAME = entity.CreatedBy
                });
        }

        public PagingResult<DistributionFileDto> FindAll(int distributionId, int pageSize, int pageNumber)
        {
            return _oracleHelper.ExecuteProcedurePaging<DistributionFileDto>(PROC_DISTRIBUTION_FILE_GET_ALL, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
            });
        }

        public DistributionFile FindById(int id)
        {

            var result = _oracleHelper.ExecuteProcedureToFirst<DistributionFile>(PROC_DISTRIBUTION_FILE_GET, new
            {
                pv_ID = id,
            });
            return result;
        }

        public int Update(DistributionFile entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_DISTRIBUTION_FILE_UPDATE, new
                {
                    pv_ID = entity.Id,
                    pv_DISTRIBUTION_ID = entity.DistributionId,
                    pv_FILE_URL = entity.FileUrl,
                    pv_TITLE = entity.Title,
                    SESSION_USERNAME = entity.ModifiedBy
                });
        }

        public int Delete(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_FILE_DELETE, new
            {
                pv_ID = id
            });
        }
    }
}
