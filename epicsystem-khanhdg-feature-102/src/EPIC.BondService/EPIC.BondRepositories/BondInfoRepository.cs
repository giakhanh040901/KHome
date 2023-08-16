using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondInfoRepository : BaseRepository
    {
        private const string ADD_EP_PRODUCT_BOND_INFO_PROC = "PKG_PRODUCT_BOND_INFO.PROC_EP_PRODUCT_BOND_INFO_ADD";
        private const string UPDATE_EP_PRODUCT_BOND_INFO_PROC = "PKG_PRODUCT_BOND_INFO.PROC_PRODUCT_BOND_INFO_UPDATE";
        private const string GET_ALL_EP_PRODUCT_BOND_INFO_PROC = "PKG_PRODUCT_BOND_INFO.PROC_EP_PRODUCT_BOND_INFO_FIND";
        private const string GET_EP_PRODUCT_BOND_INFO_PROC = "PKG_PRODUCT_BOND_INFO.PROC_PRODUCT_BOND_INFO_GET";
        private const string DELETE_EP_PRODUCT_BOND_INFO_PROC = "PKG_PRODUCT_BOND_INFO.PROC_PRODUCT_BOND_INFO_DELETE";
        private const string PROC_BOND_INFO_REQUEST = "PKG_PRODUCT_BOND_INFO.PROC_BOND_INFO_REQUEST";
        private const string PROC_BOND_INFO_APPROVE = "PKG_PRODUCT_BOND_INFO.PROC_BOND_INFO_APPROVE";
        private const string PROC_BOND_INFO_CHECK = "PKG_PRODUCT_BOND_INFO.PROC_BOND_INFO_CHECK";
        private const string PROC_BOND_INFO_ACTIVE = "PKG_PRODUCT_BOND_INFO.PROC_BOND_INFO_ACTIVE";
        private const string PROC_BOND_INFO_CANCEL = "PKG_PRODUCT_BOND_INFO.PROC_BOND_INFO_CANCEL";
        private const string PROC_BOND_INFO_CLOSE = "PKG_PRODUCT_BOND_INFO.PROC_BOND_INFO_CLOSE";

        public BondInfoRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public int BondInfoRequest(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_INFO_REQUEST, new
            {
                pv_BOND_INFO_ID = id,
            });
            return result;
        }

        public int BondInfoApprove(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_INFO_APPROVE, new
            {
                pv_BOND_INFO_ID = id
            });
            return result;
        }

        public int BondInfoCheck(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_INFO_CHECK, new
            {
                pv_BOND_INFO_ID = id
            });
            return result;
        }

        public int BondInfoCancel(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_INFO_CANCEL, new
            {
                pv_BOND_INFO_ID = id
            });
            return result;
        }

        public int BondInfoClose(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_INFO_CLOSE, new
            {
                pv_BOND_INFO_ID = id
            });
            return result;
        }

        public PagingResult<ProductBondInfoDto> FindAllProductBondInfo(int pageSize, int pageNumber, string keyword, string status, int partnerId, string isCheck, DateTime? issueDate, DateTime? dueDate)
        {
            var productBondInfo = _oracleHelper.ExecuteProcedurePaging<ProductBondInfoDto>(GET_ALL_EP_PRODUCT_BOND_INFO_PROC, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_PARTNER_ID = partnerId,
                pv_IS_CHECK = isCheck,
                pv_ISSUE_DATE = issueDate,
                pv_DUE_DATE = dueDate,
            });
            return productBondInfo;
        }

        public BondInfo FindById(int id)
        {
            BondInfo productBondInfo = _oracleHelper.ExecuteProcedureToFirst<BondInfo>(GET_EP_PRODUCT_BOND_INFO_PROC, new
            {
                pv_PRODUCT_BOND_ID = id,
            });
            return productBondInfo;
        }

        /// <summary>
        /// Tìm trực tiếp không qua partner id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BondInfo FindBondInfoById(int id)
        {
            BondInfo productBondInfo = _oracleHelper.ExecuteProcedureToFirst<BondInfo>(GET_EP_PRODUCT_BOND_INFO_PROC, new
            {
                pv_PRODUCT_BOND_ID = id,
            });
            return productBondInfo;
        }

        public BondInfo Add(BondInfo entity)
        {
            _logger.LogInformation("Add Product Bond Info");
            return _oracleHelper.ExecuteProcedureToFirst<BondInfo>(ADD_EP_PRODUCT_BOND_INFO_PROC, new
            {
                pv_ISSUER_ID = entity.IssuerId,
                pv_DEPOSIT_PROVIDER_ID = entity.DepositProviderId,
                pv_BOND_TYPE_ID = entity.BondTypeId,
                pv_BOND_CODE = entity.BondCode,
                pv_BOND_NAME = entity.BondName,
                pv_DESCRIPTION = entity.Description,
                pv_CONTENT = entity.Content,
                pv_ISSUE_DATE = entity.IssueDate,
                pv_DUE_DATE = entity.DueDate,
                pv_PAR_VALUE = entity.ParValue,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_BOND_PERIOD = entity.BondPeriod,
                pv_BOND_PERIOD_UNIT = entity.BondPeriodUnit,
                pv_INTEREST_RATE = entity.InterestRate,
                pv_INTEREST_PERIOD = entity.InterestPeriod,
                pv_INTEREST_PERIOD_UNIT = entity.InterestPeriodUnit,
                pv_INTEREST_TYPE = "",
                pv_INTEREST_RATE_TYPE = entity.InterestRateType,
                pv_INTEREST_COUPON_TYPE = "",
                pv_COUPON_BOND_TYPE = "",
                pv_IS_PAYMENT_GUARANTEE = entity.IsPaymentGuarantee,
                pv_ALLOW_SBD = entity.AllowSbd,
                pv_ALLOW_SBD_MONTH = entity.AllowSbdMonth,
                pv_IS_COLLATERAL = entity.IsCollateral,
                pv_MAX_INVESTOR = entity.MaxInvestor,
                pv_NUMBER_CLOSE_PER = entity.NumberClosePer,
                pv_COUNT_TYPE = entity.CountType,
                pv_NIEM_YET = entity.NiemYet,
                pv_QUANTITY = entity.Quantity,
                pv_FEE_RATE = entity.FeeRate,
                pv_IS_CHECK = entity.IsCheck,
                pv_POLICY_PAYMENT_CONTENT = entity.PolicyPaymentContent,
                pv_PARTNER_ID = entity.PartnerId,
                pv_ICON = entity.Icon,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }

        public int Update(BondInfo entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_EP_PRODUCT_BOND_INFO_PROC, new
            {
                pv_PRODUCT_BOND_ID = entity.Id,
                pv_ISSUER_ID = entity.IssuerId,
                pv_DEPOSIT_PROVIDER_ID = entity.DepositProviderId,
                pv_BOND_TYPE_ID = entity.BondTypeId,
                pv_BOND_CODE = entity.BondCode,
                pv_BOND_NAME = entity.BondName,
                pv_DESCRIPTION = entity.Description,
                pv_CONTENT = entity.Content,
                pv_ISSUE_DATE = entity.IssueDate,
                pv_DUE_DATE = entity.DueDate,
                pv_PAR_VALUE = entity.ParValue,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_BOND_PERIOD = entity.BondPeriod,
                pv_BOND_PERIOD_UNIT = entity.BondPeriodUnit,
                pv_INTEREST_RATE = entity.InterestRate,
                pv_INTEREST_PERIOD = entity.InterestPeriod,
                pv_INTEREST_PERIOD_UNIT = entity.InterestPeriodUnit,
                pv_INTEREST_TYPE = "",
                pv_INTEREST_RATE_TYPE = entity.InterestRateType,
                pv_INTEREST_COUPON_TYPE = "",
                pv_COUPON_BOND_TYPE = "",
                pv_IS_PAYMENT_GUARANTEE = entity.IsPaymentGuarantee,
                pv_ALLOW_SBD = entity.AllowSbd,
                pv_ALLOW_SBD_MONTH = entity.AllowSbdMonth,
                pv_IS_COLLATERAL = entity.IsCollateral,
                pv_MAX_INVESTOR = entity.MaxInvestor,
                pv_NUMBER_CLOSE_PER = entity.NumberClosePer,
                pv_COUNT_TYPE = entity.CountType,
                pv_NIEM_YET = entity.NiemYet,
                pv_QUANTITY = entity.Quantity,
                pv_FEE_RATE = entity.FeeRate,
                pv_IS_CHECK = entity.IsCheck,
                pv_STATUS = entity.Status,
                pv_POLICY_PAYMENT_CONTENT = entity.PolicyPaymentContent,
                pv_PARTNER_ID = entity.PartnerId,
                pv_ICON = entity.Icon,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int Delete(int id, int partnerId)
        {
            _logger.LogInformation($"Delete Product Bond Info: {id}");
            return _oracleHelper.ExecuteProcedureNonQuery(DELETE_EP_PRODUCT_BOND_INFO_PROC, new
            {
                pv_PRODUCT_BOND_ID = id,
                pv_PARTNER_ID = partnerId
            });
        }
    }
}
