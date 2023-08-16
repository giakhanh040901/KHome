﻿using Dapper.Oracle;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Order;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
//using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesRepositories
{
    public class OrderRepository : BaseRepository
    {
        private const string PROC_ORDER_ADD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_ADD";
        private const string PROC_ORDER_UPDATE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_UPDATE";
        private const string PROC_ORDER_GET_ALL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_GET_ALL";
        private const string PROC_ORDER_GET = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_GET";
        private const string PROC_ORDER_DELETE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_DELETE";
        private const string PROC_UPDATE_TOTAL_VALUE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_UPDATE_TOTAL_VALUE";
        private const string PROC_UPDATE_POLICY_DETAIL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_UPDATE_POLICY_DETAIL";
        private const string PROC_UPDATE_REFERRAL_CODE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_UPDATE_REFERRAL_CODE";
        private const string PROC_UPDATE_SOURCE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_UPDATE_SOURCE";
        private const string PROC_ORDER_APPROVE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_APPROVE";
        private const string PROC_ORDER_CANCEL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_CANCEL";
        private const string PROC_ORDER_UPDATE_INVESTOR_BANK_ACCOUNT = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_UPDATE_INVES_BANK_ACC";
        private const string PROC_SUM_QUANTITY = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_SUM_QUANTITY";
        private const string PROC_SUM_QUANTITY_BY_STATUS = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_SUM_QUANTITY_BY_STATUS";
        private const string PROC_SUM_VALUE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_SUM_VALUE";
        private const string PROC_SUM_VALUE_BY_STATUS = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_SUM_VALUE_BY_STATUS";
        private const string PROC_ORDER_INVESTOR_ADD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_INVESTOR_ADD";
        private const string PROC_DELIVERY_STATUS_DELIVERED = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_DELIVERY_STATUS_DELIVERED";
        private const string PROC_APP_DELI_STATUS_RECEIVED = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_APP_DELI_STATUS_RECEIVED";
        private const string PROC_DELI_STATUS_RECEIVED = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_DELI_STATUS_RECEIVED";
        private const string PROC_APP_ORDER_GET_ALL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_APP_ORDER_GET_ALL";
        private const string PROC_APP_ORDER_DETAIL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_APP_ORDER_DETAIL";
        private const string PROC_GET_PHONE_BY_DELY_CODE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_GET_PHONE_BY_DELY_CODE";
        private const string PROC_GET_CPS_INVESTMENT = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_ORDER_EXPORT_EXCEL";
        private const string PROC_GET_ALL_DELI_STATUS = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_GET_ALL_DELI_STATUS";
        private const string PROC_DELIVERY_STATUS_RECEIVED = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_DELIVERY_STATUS_RECEIVED";
        private const string PROC_DELIVERY_STATUS_DONE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_DELIVERY_STATUS_DONE";
        private const string PROC_APP_CHECK_SALE_CODE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_APP_CHECK_SALE_CODE";
        private const string PROC_UPDATE_SETTLEMENT_METHOD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_UPDATE_SETTLEMENT_METHOD";
        private const string PROC_GET_ALL_INTEREST_PAYMENT_DATE = "PKG_CPS_INTEREST_PAYMENT_DATE.PROC_INTEREST_PAYMENT_GET_ALL";
        private const string PROC_FIND_ORDER_BY_TRADING_PROVIDER_ID = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.GET_ORDER_BY_TRAD_PRO_ID";
        private const string PROC_CALENDAR_NEXT_WORK_DAY = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CALENDAR.PROC_CALENDAR_NEXT_WORK_DAY";
        private const string PROC_CPS_PAYMENT_SUM_MONEY = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INTEREST_PAYMENT.PROC_PAYMENT_SUM_MONEY";
        private const string PROC_ORDER_EXPORT_EXCEL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_EXCEL_REPORT.PROC_INTEREST_PRINCIPAL_DUE";
        private const string PROC_APP_SALE_VIEW_ORDER = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_APP_SALE_VIEW_ORDER";
        private const string PROC_INVESTOR_GET_LIST_ORDER_BY_STOCK = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER.PROC_INVESTOR_GET_LIST_ORDER_BY_STOCK";

        public OrderRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public Order Add(Order entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<Order>(PROC_ORDER_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_CIF_CODE = entity.CifCode,
                pv_CPS_SECONDARY_ID = entity.CpsSecondaryId,
                pv_CPS_POLICY_ID = entity.CpsPolicyId,
                pv_CPS_POLICY_DETAIL_ID = entity.CpsPolicyDetailId,
                pv_INVESTOR_BANK_ACC_ID = entity.InvestorBankAccId,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_IS_INTEREST = entity.IsInterest,
                pv_REFERRAL_CODE = entity.SaleReferralCode,
                pv_INVESTOR_IDEN_ID = entity.InvestorIdenId,
                pv_CONTRACT_ADDRESS_ID = entity.ContractAddressId,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int Delete(int id, int tradingProviderId)
        {
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_DELETE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return rslt;
        }

        /// <summary>
        /// NextWorkDay nếu là ngày nghỉ lễ
        /// </summary>
        /// <param name="workingDate"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="isClose"></param>
        /// <returns></returns>
        public DateTime NextWorkDay(DateTime workingDate, int tradingProviderId, bool isClose = true)
        {
            DateTime result = DateTime.Now;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_WORKING_DATE", workingDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Date, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_CALENDAR_NEXT_WORK_DAY, parameters, isClose);

            result = parameters.Get<DateTime>("pv_RESULT");
            return result;
        }

        public PagingResult<ViewOrderDto> FindAll(int? tradingProviderId, string taxCode, string idNo, string cifCode, string phone, int pageSize, int pageNumber, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer, List<int?> tradingProviderChildIds = null)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ViewOrderDto>(PROC_ORDER_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEYWORD = keyword,
                pv_STATUS = status,
                pv_GROUP_STATUS = groupStatus,
                pv_SOURCE = source,
                pv_CPS_SECONDARY_ID = bondSecondaryId,
                pv_CPS_POLICY = bondPolicy,
                pv_CIF_CODE = cifCode,
                pv_PHONE = phone,
                pv_ID_NO = idNo,
                pv_TAX_CODE = taxCode,
                pv_CPS_POLICY_DETAIL_ID = bondPolicyDetailId,
                pv_CUSTOMER_NAME = customerName,
                pv_CONTRACT_CODE = contractCode,
                pv_TRADING_DATE = tradingDate,
                pv_DELIVERY_STATUS = deliveryStatus,
                pv_ORDERER = orderer,
                pv_TRADING_PROVIDER_CHILD_IDS = tradingProviderChildIds != null ? string.Join(',', tradingProviderChildIds) : null
            });
            return result;
        }

        /// <summary>
        /// Tính ngày đáo hạn 
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="isClose"></param>
        /// <returns></returns>
        //public DateTime CalculateDueDate(PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, bool isClose = true)
        //{
        //    //Số kỳ trả lợi tức, thời gian KH đầu tư
        //    int soKyDaoHan = policyDetail.PeriodQuantity ?? 0;
        //    //Ngày đáo hạn
        //    DateTime ngayDaoHan = ngayBatDauTinhLai.Date;
        //    if (policyDetail.InterestDays != null) //nếu có cài ngày chính xác
        //    {
        //        ngayDaoHan = ngayDaoHan.AddDays(policyDetail.InterestDays.Value);
        //    }
        //    else //không cài ngày chính xác
        //    {
        //        if (policyDetail.PeriodType == PeriodUnit.DAY)
        //        {
        //            ngayDaoHan = ngayDaoHan.AddDays(soKyDaoHan);
        //        }
        //        else if (policyDetail.PeriodType == PeriodUnit.MONTH)
        //        {
        //            ngayDaoHan = ngayDaoHan.AddMonths(soKyDaoHan);
        //        }
        //        else if (policyDetail.PeriodType == PeriodUnit.YEAR)
        //        {
        //            ngayDaoHan = ngayDaoHan.AddYears(soKyDaoHan);
        //        }
        //    }
        //    ngayDaoHan = NextWorkDay(ngayDaoHan.Date, policyDetail.TradingProviderId, isClose);
        //    ngayDaoHan = ngayDaoHan.Date;
        //    return ngayDaoHan;
        //}

        public PagingResult<ViewOrderDto> FindAllDeliveryStatus(int tradingProviderId, string taxCode, string idNo, string cifCode, string phone, int pageSize, int pageNumber, string keyword, int? status, int? groupStatus, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ViewOrderDto>(PROC_GET_ALL_DELI_STATUS, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEYWORD = keyword,
                pv_STATUS = status,
                pv_GROUP_STATUS = groupStatus,
                pv_SOURCE = source,
                pv_CPS_SECONDARY_ID = cpsSecondaryId,
                pv_CPS_POLICY = cpsPolicy,
                pv_CIF_CODE = cifCode,
                pv_PHONE = phone,
                pv_ID_NO = idNo,
                pv_TAX_CODE = taxCode,
                pv_CPS_POLICY_DETAIL_ID = cpsPolicyDetailId,
                pv_CUSTOMER_NAME = customerName,
                pv_CONTRACT_CODE = contractCode,
                pv_TRADING_DATE = tradingDate,
                pv_DELIVERY_STATUS = deliveryStatus
            });
            return result;
        }

        public Order FindById(long id, int? tradingProviderId = null, int? partnerId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<Order>(PROC_ORDER_GET, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_ORDER_ID = id,
            });
            return result;
        }

        public int Update(Order entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_UPDATE, new
            {
                pv_ORDER_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_IS_INTEREST = entity.IsInterest,
                pv_CPS_POLICY_DETAIL_ID = entity.CpsPolicyDetailId,
                pv_INVESTOR_BANK_ACC_ID = entity.InvestorBankAccId,
                pv_REFERRAL_CODE = entity.SaleReferralCode,
                pv_CONTRACT_ADDRESS_ID = entity.ContractAddressId,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int UpdateTotalValue(int orderId, int tradingProviderId, decimal? totalValue, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_TOTAL_VALUE, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TOTAL_VALUE = totalValue,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int UpdatePolicyDetail(int orderId, int tradingProviderId, int? cpsPolicyDetailId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_POLICY_DETAIL, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_CPS_POLICY_DETAIL_ID = cpsPolicyDetailId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int UpdateReferralCode(int orderId, int tradingProviderId, string referralCode, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_REFERRAL_CODE, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_REFERRAL_CODE = referralCode,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int UpdateSource(int orderId, int tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_SOURCE, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int OrderApprove(int orderId, int tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_APPROVE, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int AppUpdateSettlementMethod(int orderId, SettlementMethodDto input, string modifiedBy, int? investorId, int? tradingProvider = null)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_SETTLEMENT_METHOD, new
            {
                pv_ORDER_ID = orderId,
                pv_INVESTOR_ID = investorId,
                pv_TRADING_PROVIDER_ID = tradingProvider,
                SESSION_USERNAME = modifiedBy,
                pv_SETTLEMENT_METHOD = input.SettlementMethod,
                pv_RENEWALS_POLICY_DETAIL_ID = input.RenewalsPolicyDetailId,
            });
            return result;
        }

        public int OrderCancel(int orderId, int tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_CANCEL, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int UpdateInvestorBankAccount(int orderId, int? bankAccId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_UPDATE_INVESTOR_BANK_ACCOUNT, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVESTOR_BANK_ACC_ID = bankAccId,
                SESSION_USERNAME = modifiedBy,
            });
        }

        public long SumQuantity(int? tradingProviderId, int? cpsSecondaryId)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_CPS_SECONDARY_ID", cpsSecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Int32, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_QUANTITY, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (long)result;
        }

        public long SumValue(int tradingProviderId, int cpsSecondaryId)
        {
            decimal result = 0;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_CPS_SECONDARY_ID", cpsSecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_VALUE, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (long)result;
        }

        public OrderAppDto InvestorAdd(CheckOrderAppDto input, int investorId, bool isCheck, string otp = null, string ipAddress = null, bool isSelfDoing = true, int? saleId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<OrderAppDto>(PROC_ORDER_INVESTOR_ADD, new
            {
                pv_INVESTOR_ID = investorId,
                pv_CPS_POLICY_DETAIL_ID = input.PolicyDetailId,
                pv_PROMOTION_ID = input.PromotionId,
                pv_TOTAL_VALUE = input.TotalValue,
                pv_INVESTOR_BANK_ACC_ID = input.BankAccId,
                pv_IDENTIFICATION_ID = input.IdentificationId,
                pv_IS_RECEIVE_CONTRACT = input.IsReceiveContract ? YesNo.YES : YesNo.NO,
                pv_CONTRACT_ADDRESS_ID = input.TranAddess,
                pv_REFERRAL_CODE = input.ReferralCode,
                pv_IS_CHECK = isCheck ? YesNo.YES : YesNo.NO,
                pv_OTP = otp,
                pv_IP_ADDRESS_CREATED = ipAddress,
                pv_IS_SELF_DOING = isSelfDoing,
                pv_SALE_ID = saleId
            });
        }

        public IEnumerable<AppOrderInvestorDto> AppGetAll(int investorId, int? groupStatus)
        {
            return _oracleHelper.ExecuteProcedure<AppOrderInvestorDto>(PROC_APP_ORDER_GET_ALL, new
            {
                pv_INVESTOR_ID = investorId,
                pv_GROUP_STATUS = groupStatus
            });
        }

        public ViewOrderDto AppGetOrderDetail(int investorId, int orderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ViewOrderDto>(PROC_APP_ORDER_DETAIL, new
            {
                pv_INVESTOR_ID = investorId,
                pv_ORDER_ID = orderId,
            });
            return result;
        }

        public PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<PhoneReceiveDto>(PROC_GET_PHONE_BY_DELY_CODE, new
            {
                pv_DELIVERY_CODE = deliveryCode
            });
            return result;
        }

        public int ChangeDeliveryStatusDelivered(int orderId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELIVERY_STATUS_DELIVERED, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                SESSION_USERNAME = modifiedBy,
            });
        }

        public int ChangeDeliveryStatusReceived(int orderId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELIVERY_STATUS_RECEIVED, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                SESSION_USERNAME = modifiedBy,
            });
        }

        public int ChangeDeliveryStatusDone(int orderId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELIVERY_STATUS_DONE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                SESSION_USERNAME = modifiedBy,
            });
        }

        public int ChangeDeliveryStatusReceviredApp(string deliveryCode, int investorId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_DELI_STATUS_RECEIVED, new
            {
                pv_INVESTOR_ID = investorId,
                pv_DELIVERY_CODE = deliveryCode,
                SESSION_USERNAME = modifiedBy,
            });
        }

        public long SumQuantityByStatus(int? tradingProviderId, int? cpsSecondaryId, int Status)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_CPS_SECONDARY_ID", cpsSecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_STATUS", cpsSecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Int32, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_QUANTITY_BY_STATUS, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (long)result;
        }

        public long SumValueByStatus(int tradingProviderId, int secondaryId, int Status)
        {
            decimal result = 0;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_CPS_SECONDARY_ID", secondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_STATUS", secondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_VALUE_BY_STATUS, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (long)result;
        }

        public decimal ChangeDeliveryStatusRecevired(string deliveryCode, string otp, string modifiedBy)
        {
            decimal result = 0;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_DELIVERY_CODE", deliveryCode, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_OTP", otp, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("SESSION_USERNAME", modifiedBy, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_DELI_STATUS_RECEIVED, parameters);
            result = parameters.Get<decimal>("pv_RESULT");
            return result;
        }

        //public List<BondInvestmentDto> ExportBondInvestment(int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        //{
        //    return _oracleHelper.ExecuteProcedure<BondInvestmentDto>(PROC_GET_CPS_INVESTMENT, new
        //    {
        //        pv_START_DATE = startDate,
        //        pv_END_DATE = endDate,
        //        pv_TRADING_PROVIDER_ID = tradingProviderId
        //    }).ToList();
        //}

        //public List<InterestPrincipalDue> ExportInterestPrincipalDue(int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        //{
        //    return _oracleHelper.ExecuteProcedure<InterestPrincipalDue>(PROC_ORDER_EXPORT_EXCEL, new
        //    {
        //        pv_START_DATE = startDate,
        //        pv_END_DATE = endDate,
        //        pv_TRADING_PROVIDER_ID = tradingProviderId
        //    }).ToList();
        //}

        /// <summary>
        /// Check sale có đủ điều kiện ko trước khi investor tự đặt lệnh. Nếu đủ điều kiện thì lấy ra thông tin sale
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="secondaryId"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public ViewCheckSaleBeforeAddOrderDto AppCheckSaleBeforeAddOrder(string referralCode, int secondaryId, int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<ViewCheckSaleBeforeAddOrderDto>(PROC_APP_CHECK_SALE_CODE, new
            {
                pv_REFERRAL_CODE = referralCode,
                pv_CPS_SECONDARY_ID = secondaryId,
                pv_INVESTOR_ID = investorId
            });
        }


        /// <summary>
        /// Lấy dữ liệu data của bảng ngày chi trả chia trang
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public List<InterestPaymentDate> GetAllInterestPaymentDate(int? orderId)
        {
            var result = _oracleHelper.ExecuteProcedure<InterestPaymentDate>(PROC_GET_ALL_INTEREST_PAYMENT_DATE, new
            {
                pv_ORDER_ID = orderId,
            }).ToList();
            return result;
        }

        public List<Order> FindOrderByTradingProviderId(int tradingPoviderId, string contractCode, string taxCode, string phone)
        {
            var result = _oracleHelper.ExecuteProcedure<Order>(PROC_FIND_ORDER_BY_TRADING_PROVIDER_ID, new
            {
                pv_TRADING_PROVIDER_ID = tradingPoviderId,
                pv_CONTRACT_CODE = contractCode,
                pv_TAX_CODE = taxCode,
                pv_PHONE = phone
            }).ToList();
            return result;
        }

        public decimal InterestPaymentSumMoney(DateTime payDate, long orderId, int tradingProviderId, bool isClose = true)
        {
            decimal result = 0;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_PAY_DATE", payDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_ORDER_ID", tradingProviderId, OracleMappingType.Long, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_CPS_PAYMENT_SUM_MONEY, parameters, isClose);

            result = parameters.Get<decimal>("pv_RESULT");
            return result;
        }

        public ViewOrderDto AppSaleViewOrder(int saleId, int orderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ViewOrderDto>(PROC_APP_SALE_VIEW_ORDER, new
            {
                pv_SALE_ID = saleId,
                pv_ORDER_ID = orderId,
            });
            return result;
        }

        public List<Order> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount)
        {
            return _oracleHelper.ExecuteProcedure<Order>(PROC_INVESTOR_GET_LIST_ORDER_BY_STOCK, new
            {
                pv_SECURITY_COMPANY = securityCompany,
                pv_STOCK_TRADING_ACCOUNT = stockTradingAccount
            }).ToList();
        }
    }
}
