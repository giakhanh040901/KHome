using EPIC.DataAccess;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class InvestorBankAccountRepository
    {
        private readonly OracleHelper _oracleHelper;
        private const string PROC_INVESTOR_BANK_ACCOUNT_UPDATE = "PKG_INVESTOR.PROC_INVES_BANK_ACC_UPDATE";
        private const string GET_BY_INVESTOR_PROC = "PKG_INVESTOR.PROC_INVES_BANK_ACC_GET";
        private const string GET_BY_ID_PROC = "PKG_INVESTOR.PROC_BANK_ACC_GET_BY_ID";

        public InvestorBankAccountRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public InvestorBankAccount GetByInvestorId(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorBankAccount>(
                GET_BY_INVESTOR_PROC,
                new
                {
                    pv_INVESTOR_ID = investorId
                }
             );
        }

        public InvestorBankAccount GetById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorBankAccount>(
                GET_BY_ID_PROC,
                new
                {
                    pv_ID = id
                }
             );
        }

        public int Update(string phone, int bankId, string bankAccount, string ownerAccount)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_BANK_ACCOUNT_UPDATE, new
            {
                pv_PHONE = phone,
                pv_BANK_ID = bankId,
                pv_BANK_ACCOUNT = bankAccount,
                pv_OWNER_ACCOUNT = ownerAccount
            });
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }
    }
}
