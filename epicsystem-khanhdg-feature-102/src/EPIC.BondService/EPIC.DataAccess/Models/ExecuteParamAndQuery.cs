using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataAccess.Models
{
    public class ExecuteParamAndQuery
    {
        public OracleParameter[] Parameters { get; set; }
        public string SqlQuery { get; set; }
    }
}
