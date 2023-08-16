using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class ApproveRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        //private const string PROC_GET_REQUESTS = "PKG_CORE_APPROVE.PROC_GET_REQUESTS";
        private const string PROC_GET_BY_ID = "EPIC.PKG_CORE_APPROVE.PROC_GET_BY_ID";
        private const string PROC_GET_LIST_APPROVE = "EPIC.PKG_CORE_APPROVE.PROC_GET_LIST_APPROVE";
        private const string PROC_GET_ONE_BY_TEMP = "EPIC.PKG_CORE_APPROVE.PROC_GET_ONE_BY_TEMP";
        private const string PROC_GET_ONE_BY_ACTUAL = "EPIC.PKG_CORE_APPROVE.PROC_GET_ONE_BY_ACTUAL";

        private const string PROC_CREATE_REQUEST = "EPIC.PKG_CORE_APPROVE.PROC_CREATE_REQUEST";
        private const string PROC_REQ_TO_APPROVED_DATA = "EPIC.PKG_CORE_APPROVE.PROC_REQ_TO_APPROVED_DATA";
        private const string PROC_REQ_TO_APPROVED_STATUS = "EPIC.PKG_CORE_APPROVE.PROC_REQ_TO_APPROVED_STATUS";
        private const string PROC_APPROVED_TO_CHECKED = "EPIC.PKG_CORE_APPROVE.PROC_APPROVED_TO_CHECKED";

        private const string PROC_CANCEL = "EPIC.PKG_CORE_APPROVE.PROC_CANCEL";
        private const string PROC_CLOSE = "EPIC.PKG_CORE_APPROVE.PROC_CLOSE";
        private const string PROC_REQ_TO_APPROVED_DATA_TEMP = "EPIC.PKG_CORE_APPROVE.PROC_REQ_TO_APPROVED_DATA_TEMP";

        public ApproveRepository(string connectionString, ILogger logger)
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
        public CoreApprove CreateApproveRequest(CreateApproveRequestDto dto, int? tradingProviderId = null, int? partnerId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CoreApprove>(PROC_CREATE_REQUEST, new
            {
                pv_USER_REQUEST_ID = dto.UserRequestId,
                pv_USER_APPROVE_ID = dto.UserApproveId,
                pv_REQUEST_NOTE = dto.RequestNote,
                pv_ACTION_TYPE = dto.ActionType,
                pv_DATA_TYPE = dto.DataType,
                pv_REFER_ID = dto.ReferId,
                pv_REFER_ID_TEMP = dto.ReferIdTemp,
                pv_DATA_STATUS = dto.DataStatus,
                pv_DATA_STATUS_STR = dto.DataStatusStr,
                pv_SUMMARY = dto.Summary,
                pv_APPROVE_REQUEST_FILE_URL = dto.ApproveRequestFileUrl,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            });
        }

        /// <summary>
        /// Duyệt Dữ liệu 
        /// </summary>
        /// <param name="dto"></param>
        public void ApproveRequestData(ApproveRequestDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_REQ_TO_APPROVED_DATA, new
            {
                pv_REFER_ID = dto.ReferId,
                pv_APPROVE_ID = dto.ApproveID,
                pv_APPROVE_NOTE = dto.ApproveNote,
                pv_USER_APPROVE_ID = dto.UserApproveId
            }, false);
        }

        /// <summary>
        /// Duyệt Dữ liệu 
        /// </summary>
        /// <param name="dto"></param>
        public void ApproveRequestDataTemp(ApproveRequestTempDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_REQ_TO_APPROVED_DATA_TEMP, new
            {
                pv_REFER_ID_TEMP = dto.ReferIdTemp,
                pv_APPROVE_ID = dto.ApproveID,
                pv_APPROVE_NOTE = dto.ApproveNote
            });
        }

        /// <summary>
        /// Duyệt trạng thái
        /// </summary>
        /// <param name="dto"></param>
        public void ApproveRequestStatus(ApproveRequestDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_REQ_TO_APPROVED_STATUS, new
            {
                pv_APPROVE_ID = dto.ApproveID,
                pv_APPROVE_NOTE = dto.ApproveNote,
                pv_USER_APPROVE_ID = dto.UserApproveId
            });
        }

        /// <summary>
        /// Xác minh yêu cầu đã được duyệt
        /// </summary>
        /// <param name="dto"></param>
        public void CheckRequest(CheckRequestDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APPROVED_TO_CHECKED, new
            {
                pv_APPROVE_ID = dto.ApproveID,
                pv_USER_CHECK_ID = dto.UserCheckId,
            });
        }

        /// <summary>
        /// Huỷ
        /// </summary>
        /// <param name="dto"></param>
        public void CancelRequest(CancelRequestDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CANCEL, new
            {
                pv_APPROVE_ID = dto.ApproveID,
                pv_CANCEL_NOTE = dto.CancelNote,
                pv_USER_APPROVE_ID = dto.UserApproveId
            });
        }

        /// <summary>
        /// Đóng
        /// </summary>
        /// <param name="dto"></param>
        public void CloseRequest(CloseRequestDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CLOSE, new
            {
                pv_APPROVE_ID = dto.ApproveID,
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
        public PagingResult<CoreApprove> GetAll(GetApproveListDto dto, int? tradingProviderId = null, int? partnerId = null, string userType = null)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<CoreApprove>(PROC_GET_LIST_APPROVE, new
            {
                PAGE_SIZE = dto.PageSize,
                PAGE_NUMBER = dto.PageNumber,
                KEYWORD = dto.Keyword,
                pv_STATUS = dto.Status,
                pv_USER_APPROVE_ID = dto.UserApproveId,
                pv_USER_REQUEST_ID = dto.UserRequestId,
                pv_DATA_TYPE = dto.DataType,
                pv_ACTION_TYPE = dto.ActionType,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_REQUEST_DATE = dto.RequestDate,
                pv_APPROVE_DATE = dto.ApproveDate,
                pv_PARTNER_ID = partnerId,
                pv_REFER_ID = dto.ReferId,
                pv_REFER_ID_TEMP = dto.ReferIdTemp,
                pv_USER_TYPE = userType
            });
            return result;
        }

        /// <summary>
        /// Lấy yêu cầu duyệt theo Bảng nào và Refer Id temp 
        /// </summary>
        /// <param name="refIdTemp"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public CoreApprove GetOneByTemp(int refIdTemp, int dataType)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CoreApprove>(PROC_GET_ONE_BY_TEMP, new
            {
                pv_REFER_ID_TEMP = refIdTemp,
                pv_DATA_TYPE = dataType,
            });
        }

        /// <summary>
        /// Lấy yêu câu duyệt theo Bảng nào và Refer Id thật
        /// </summary>
        /// <param name="refId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public CoreApprove GetOneByActual(int refId, int dataType)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CoreApprove>(PROC_GET_ONE_BY_ACTUAL, new
            {
                pv_REFER_ID = refId,
                pv_DATA_TYPE = dataType,
            });
        }

        /// <summary>
        /// Lấy yêu cầu duyệt theo approve id
        /// </summary>
        /// <param name="approveId"></param>
        /// <returns></returns>
        public CoreApprove GetByApproveId(int approveId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CoreApprove>(PROC_GET_BY_ID, new
            {
                pv_APPROVE_ID = approveId,
            });
        }

        public void OpenConnection()
        {
            _oracleHelper.OpenConnection();
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }
    }
}
