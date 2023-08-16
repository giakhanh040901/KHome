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
    public class InvestorIdentificationRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string GET_BY_INVESTOR_PROC = "PKG_INVESTOR_IDENTIFICATION.PROC_INVESTOR_ID_GET";
        private static string GET_BY_ID_PROC = "PKG_INVESTOR_IDENTIFICATION.PROC_ID_GET";
        private static string GET_LIST_BY_INVESTOR_PROC = "PKG_INVESTOR_IDENTIFICATION.PROC_LIST_INVESTOR_ID_GET";


        public InvestorIdentificationRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public InvestorIdentification GetByInvestorId(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorIdentification>(
                GET_BY_INVESTOR_PROC,
                new
                {
                    pv_INVESTOR_ID = investorId
                }
             );
        }

        public IEnumerable<InvestorIdentification> GetListByInvestorId(int investorId)
        {
            return _oracleHelper.ExecuteProcedure<InvestorIdentification>(GET_LIST_BY_INVESTOR_PROC, new
            {
                pv_INVESTOR_ID = investorId
            });
        }

        public InvestorIdentification FindById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorIdentification>(
                GET_BY_ID_PROC,
                new
                {
                    pv_ID = id
                }
             );
        }
    }
}
