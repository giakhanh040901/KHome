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
    public class CoreHistoryUpdateRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_CORE_HISTORY_ADD = "PKG_CORE_HISTORY_UPDATE.PROC_HIS_UPDATE_ADD";
        private const string PROC_GET_BY_APPROVE_ID = "PKG_CORE_HISTORY_UPDATE.PROC_GET_BY_APPROVE_ID";
        private const string PROC_CORE_SALE_HISTORY_UPDATE_GET_ALL = "PKG_CORE_HISTORY_UPDATE.PROC_CORE_SALE_HISTORY_UPDATE_GET_ALL";

        public CoreHistoryUpdateRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public int Add(CoreHistoryUpdate entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_CORE_HISTORY_ADD, new
                {
                    pv_APPROVE_ID = entity.ApproveId,
                    pv_REAL_TABLE_ID = entity.RealTableId,
                    pv_OLD_VALUE = entity.OldValue,
                    pv_NEW_VALUE = entity.NewValue,
                    pv_FIELD_NAME = entity.FieldName,
                    pv_UPDATE_TABLE = entity.UpdateTable,
                    SESSION_USERNAME = entity.CreatedBy
                });
        }

        /// <summary>
        /// Lấy các lịch sử thay đổi theo approve id
        /// </summary>
        /// <param name="approveId"></param>
        /// <returns></returns>
        public IEnumerable<CoreHistoryUpdate> GetByApproveId(int approveId)
        {
            return _oracleHelper.ExecuteProcedure<CoreHistoryUpdate>(
                PROC_GET_BY_APPROVE_ID, new
                {
                    pv_APPROVE_ID = approveId
                });
        }

        /// <summary>
        /// lấy lịch sử chỉnh sửa sale theo saleId phân trang
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public PagingResult<CoreHistoryUpdate> FindAllHistoryBySaleId(int saleId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<CoreHistoryUpdate>(PROC_CORE_SALE_HISTORY_UPDATE_GET_ALL, new
            {
                pv_SALE_ID = saleId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEYWORD = keyword
            });
            return result;
        }
    }
}
