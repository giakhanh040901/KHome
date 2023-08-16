using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataAccess
{
    public class MysqlHelper: SQLHelper
    {
        public MysqlHelper(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = new MySqlConnection(ConnectionString);
        }
    }
}
