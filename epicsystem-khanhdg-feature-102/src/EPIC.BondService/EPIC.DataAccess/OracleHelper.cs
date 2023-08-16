using Dapper;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPIC.DataAccess
{
    public class OracleHelper : SQLHelper
    {
        public OracleHelper(string connectionString, ILogger logger)
        {
            Logger = logger;
            ConnectionString = connectionString;
            Connection = new OracleConnection(ConnectionString);
        }
    }
}
