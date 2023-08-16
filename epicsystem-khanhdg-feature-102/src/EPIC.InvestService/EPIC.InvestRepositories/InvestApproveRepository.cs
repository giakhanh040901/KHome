using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestApprove;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestApproveRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_GET_REQUESTS = "PKG_INV_APPROVE.PROC_GET_REQUESTS";
        private const string PROC_GET_LIST_APPROVE = "PKG_INV_APPROVE.PROC_GET_LIST_APPROVE";
        private const string PROC_GET_ONE_BY_TEMP = "PKG_INV_APPROVE.PROC_GET_ONE_BY_TEMP";
        private const string PROC_GET_ONE_BY_ACTUAL = "PKG_INV_APPROVE.PROC_GET_ONE_BY_ACTUAL";

        private const string PROC_CREATE_REQUEST = "PKG_INV_APPROVE.PROC_CREATE_REQUEST";
        private const string PROC_REQ_TO_APPROVED_DATA = "PKG_INV_APPROVE.PROC_REQ_TO_APPROVED_DATA";
        private const string PROC_REQ_TO_APPROVED_STATUS = "PKG_INV_APPROVE.PROC_REQ_TO_APPROVED_STATUS";
        private const string PROC_APPROVED_TO_CHECKED = "PKG_INV_APPROVE.PROC_APPROVED_TO_CHECKED";

        private const string PROC_CANCEL = "PKG_INV_APPROVE.PROC_CANCEL";
        private const string PROC_CLOSE = "PKG_INV_APPROVE.PROC_CLOSE";
        private const string PROC_REQ_TO_APPROVED_DATA_TEMP = "PKG_INV_APPROVE.PROC_REQ_TO_APPROVED_DATA_TEMP";

        public InvestApproveRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        /// <summary>
        /// Tạo yêu cầu trình duyệt
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        public void CreateApproveRequest(CreateInvestRequestDto dto, int? tradingProviderId = null, int? partnerId = null)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CREATE_REQUEST, new
            {
                pv_USER_REQUEST_ID = dto.UserRequestId,
                pv_USER_APPROVE_ID = dto.UserApproveId,
                pv_REQUEST_NOTE = dto.RequestNote,
                pv_ACTION_TYPE = dto.ActionType,
                pv_DATA_TYPE = dto.DataType,
                pv_REFER_ID = dto.ReferId,
                pv_SUMMARY = dto.Summary,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            });
        }

        /// <summary>
        /// Duyệt trạng thái
        /// </summary>
        /// <param name="dto"></param>
        public void ApproveRequestStatus(InvestApproveDto dto, bool closeConnection = true)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_REQ_TO_APPROVED_STATUS, new
            {
                pv_ID = dto.Id,
                pv_APPROVE_NOTE = dto.ApproveNote,
                pv_USER_APPROVE_ID = dto.UserApproveId,
            }, closeConnection);
        }

        /// <summary>
        /// Xác minh yêu cầu đã được duyệt
        /// </summary>
        /// <param name="dto"></param>
        public void CheckRequest(InvestCheckDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APPROVED_TO_CHECKED, new
            {
                pv_ID = dto.Id,
                pv_USER_CHECK_ID = dto.UserCheckId,
            });
        }

        /// <summary>
        /// Huỷ
        /// </summary>
        /// <param name="dto"></param>
        public void CancelRequest(InvestCancelDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CANCEL, new
            {
                pv_ID = dto.Id,
                pv_CANCEL_NOTE = dto.CancelNote,
            });
        }

        /// <summary>
        /// Đóng
        /// </summary>
        /// <param name="dto"></param>
        public void CloseRequest(InvestCloseDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CLOSE, new
            {
                pv_ID = dto.Id,
                pv_CLOSE_NOTE = dto.CloseNote,
            });
        }


        /// <summary>
        /// Get all core approve, những mục nào cần duyệt bằng dlsc, partner thì truyền id vào tương ứng
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<InvestApprove> GetAll(InvestApproveGetDto dto, int? tradingProviderId = null, int? partnerId = null)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvestApprove>(PROC_GET_LIST_APPROVE, new
            {
                PAGE_SIZE = dto.PageSize,
                PAGE_NUMBER = dto.PageNumber,
                KEYWORD = dto.Keyword,
                pv_STATUS = dto.Status,
                pv_USER_APPROVE_ID = dto.UserApproveId,
                pv_USER_REQUEST_ID = dto.UserRequestId,
                pv_DATA_TYPE = dto.DataType,
                pv_ACTION_TYPE = dto.ActionType,
                pv_REQUEST_DATE = dto.RequestDate,
                pv_APPROVE_DATE = dto.ApproveDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            });
            return result;
        }

        /// <summary>
        /// Lấy yêu câu duyệt theo Bảng nào và Refer Id thật
        /// </summary>
        /// <param name="refId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public InvestApprove GetOneByActual(int refId, int dataType, bool closeConnection = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestApprove>(PROC_GET_ONE_BY_ACTUAL, new
            {
                pv_REFER_ID = refId,
                pv_DATA_TYPE = dataType,
            }, closeConnection);
        }
    }
}
