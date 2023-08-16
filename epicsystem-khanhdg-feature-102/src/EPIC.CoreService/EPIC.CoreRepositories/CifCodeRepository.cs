using EPIC.BondRepositories;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class CifCodeRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string GET_BY_CIF_CODE_PROC = "PKG_CIF_CODE.PROC_CIF_CODE_GET";
        private const string GET_BY_CUSTOMER_ID = "PKG_CIF_CODE.PROC_CIF_CODE_GET_BY_CUS_ID";
        
        public CifCodeRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public CifCodes GetByCifCode(string cifCode)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CifCodes>(
                GET_BY_CIF_CODE_PROC,
                new
                {
                    pv_CIF_CODE = cifCode
                }
             );
        }

        public CifCodes GetByCustomerId(int? investorId = null, int? businessCustomerId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CifCodes>(GET_BY_CUSTOMER_ID, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId,
                pv_INVESTOR_ID = investorId
            });
        }
    }
}
