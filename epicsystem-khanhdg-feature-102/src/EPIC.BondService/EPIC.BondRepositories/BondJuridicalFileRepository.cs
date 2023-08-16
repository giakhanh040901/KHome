using EPIC.BondRepositories;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using EPIC.Entities.Dto.JuridicalFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.BondEntities.DataEntities;

namespace EPIC.BondRepositories
{

    public class BondJuridicalFileRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_JURIDICAL_FILE_ADD = "PKG_JURIDICAL_FILE.PROC_JURIDICAL_FILE_ADD";
        private const string PROC_JURIDICAL_FILE_GET = "PKG_JURIDICAL_FILE.PROC_JURIDICAL_FILE_GET";
        private const string PROC_JURIDICAL_FILE_DELETE = "PKG_JURIDICAL_FILE.PROC_JURIDICAL_FILE_DELETE";
        private const string PROC_JURIDICAL_FILE_UPDATE = "PKG_JURIDICAL_FILE.PROC_JURIDICAL_FILE_UPDATE";
        private const string PROC_JURIDICAL_FILE_GET_ALL = "PKG_JURIDICAL_FILE.PROC_JURIDICAL_FILE_GET_ALL";

        public BondJuridicalFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PagingResult<BondJuridicalFile> FindAllJuridicalFile(int productBondId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BondJuridicalFile>(PROC_JURIDICAL_FILE_GET_ALL, new
            {
                pv_PRODUCT_BOND_ID = productBondId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public BondJuridicalFile FindJuridicalFileById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondJuridicalFile>(PROC_JURIDICAL_FILE_GET, new
            {
                pv_JURIDICAL_FILE_ID = id,
            });
            return result;
        }

        public BondJuridicalFile Add(BondJuridicalFile entity)
        {
            _logger.LogInformation("Add JuridicalFile");
            var result = _oracleHelper.ExecuteProcedureToFirst<BondJuridicalFile>(PROC_JURIDICAL_FILE_ADD, new
            {
                pv_PRODUCT_BOND_ID = entity.BondId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int JuridicalFileUpdate(BondJuridicalFile entity)
        {
            _logger.LogInformation("Update JuridicalFile");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_JURIDICAL_FILE_UPDATE, new
            {
                pv_JURIDICAL_FILE_ID = entity.Id,
                pv_PRODUCT_BOND_ID = entity.BondId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int DeleteJuridicalFile(int id)
        {
            _logger.LogInformation($"Delete Juridical File ");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_JURIDICAL_FILE_DELETE, new
            {
                pv_JURIDICAL_FILE_ID = id
            });
        }
    }
}
