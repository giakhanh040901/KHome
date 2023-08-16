using Microsoft.Extensions.Logging;
using System.Data;

namespace EPIC.DataAccess.Base
{
    public class BaseRepository
    {
        protected OracleHelper _oracleHelper;
        protected ILogger _logger;

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public IDbTransaction BeginTransaction()
        {
            _logger.LogInformation("-----Begin Transaction----");
            return _oracleHelper.BeginTransaction();
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            _logger.LogInformation("-----Commit Transaction----");
            _oracleHelper.CommitTransaction(transaction);
        }
    }
}
