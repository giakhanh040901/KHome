using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataAccess.Base
{
    public class SysVarRepository : BaseRepository
    {
        private const string PROC_GET_VAR_BY_NAME = "PKG_SYSVAR.PROC_GET_VAR_BY_NAME";

        public SysVarRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public SysVar GetVarByName(string grName, string varName)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<SysVar>(PROC_GET_VAR_BY_NAME, new
            {
                pv_GRNAME = grName,
                pv_VARNAME = varName
            });
            return result;
        }

        public int GetInvValueByName(string grName, string varName)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<SysVar>(PROC_GET_VAR_BY_NAME, new
            {
                pv_GRNAME = grName,
                pv_VARNAME = varName
            });
            return Convert.ToInt32(result?.VarValue);
        }
    }
}
