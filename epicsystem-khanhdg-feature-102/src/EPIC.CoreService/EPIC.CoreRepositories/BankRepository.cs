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
    public class BankRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;

        private static string PROC_GET_LIST_BANK = "EPIC.PKG_CORE_BANK.PROC_GET_LIST_BANK";
        private static string PROC_GET_LIST_BANK_SUPPORT = "EPIC.PKG_CORE_BANK.PROC_GET_LIST_BANK_SUPPORT";
        private static string PROC_GET_BANK_BY_ID = "EPIC.PKG_CORE_BANK.PROC_GET_BANK_BY_ID";

        public BankRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách ngân hàng cho dropdown
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public PagingResult<CoreBank> GetListBank(string keyword)
        {
            return _oracleHelper.ExecuteProcedurePaging<CoreBank>(PROC_GET_LIST_BANK, new
            {
                KEYWORD = keyword,
            });
        }

        /// <summary>
        /// Lấy danh sách ngân hàng hỗ trợ, và tìm kiếm theo tên viết tắt
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IEnumerable<CoreBank> GetListBankSupport(string keyword)
        {
            return _oracleHelper.ExecuteProcedure<CoreBank>(PROC_GET_LIST_BANK_SUPPORT, new
            {
                KEYWORD = keyword,
            });
        }

        /// <summary>
        /// Lấy thông tin bank theo id
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public CoreBank GetById(int bankId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CoreBank>(PROC_GET_BANK_BY_ID, new
            {
                pv_BANK_ID = bankId,
            });
        }
    }
}
