using AutoMapper.Execution;
using Dapper.Oracle;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.RocketChat;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ExportExcel;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestEntities.Dto.Order;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestOrderRepository : BaseRepository
    {
        private const string PROC_ORDER_ADD = "PKG_INV_ORDER.PROC_ORDER_ADD";
        private const string PROC_ORDER_UPDATE = "PKG_INV_ORDER.PROC_ORDER_UPDATE";
        private const string PROC_ORDER_GET_ALL = "PKG_INV_ORDER.PROC_ORDER_GET_ALL";
        private const string PROC_ORDER_GET = "PKG_INV_ORDER.PROC_ORDER_GET";
        private const string PROC_INV_ORDER_GET_LIST = "PKG_INV_ORDER.PROC_INV_ORDER_GET_LIST";
        private const string PROC_UPDATE_TOTAL_VALUE = "PKG_INV_ORDER.PROC_UPDATE_TOTAL_VALUE";
        private const string PROC_UPDATE_POLICY_DETAIL = "PKG_INV_ORDER.PROC_UPDATE_POLICY_DETAIL";
        private const string PROC_UPDATE_REFERRAL_CODE = "PKG_INV_ORDER.PROC_UPDATE_REFERRAL_CODE";
        private const string PROC_UPDATE_SOURCE = "PKG_INV_ORDER.PROC_UPDATE_SOURCE";
        private const string PROC_ORDER_APPROVE = "PKG_INV_ORDER.PROC_ORDER_APPROVE";
        private const string PROC_ORDER_CANCEL = "PKG_INV_ORDER.PROC_ORDER_CANCEL";
        private const string PROC_ORDER_UPDATE_INVESTOR_BANK_ACCOUNT = "PKG_INV_ORDER.PROC_UPDATE_INVES_BANK_ACC";
        private const string PROC_UPDATE_INFO_CUSTOMER = "PKG_INV_ORDER.PROC_UPDATE_INFO_CUSTOMER";
        private const string PROC_ORDER_INVESTOR_ADD = "PKG_INV_ORDER.PROC_ORDER_INVESTOR_ADD";
        private const string PROC_APP_ORDER_GET_ALL = "PKG_INV_ORDER.PROC_APP_ORDER_GET_ALL";
        private const string PROC_APP_ORDER_DETAIL = "PKG_INV_ORDER.PROC_APP_ORDER_DETAIL";
        private const string PROC_ORDER_SUM_VALUE = "PKG_INV_ORDER.PROC_ORDER_SUM_VALUE";
        private const string PROC_ORDER_SUM_MONEY = "PKG_INV_ORDER.PROC_ORDER_SUM_MONEY";
        private const string PROC_MAX_TOTAL_INVESTMENT = "PKG_INV_ORDER.PROC_MAX_TOTAL_INVESTMENT";
        private const string PROC_DELIVERY_STATUS_DELIVERED = "PKG_INV_ORDER.PROC_DELIVERY_STATUS_DELIVERED";
        private const string PROC_DELIVERY_STATUS_RECEIVED = "PKG_INV_ORDER.PROC_DELIVERY_STATUS_RECEIVED";
        private const string PROC_DELIVERY_STATUS_DONE = "PKG_INV_ORDER.PROC_DELIVERY_STATUS_DONE";
        private const string PROC_APP_DELI_STATUS_RECEIVED = "PKG_INV_ORDER.PROC_APP_DELI_STATUS_RECEIVED";
        private const string PROC_DELI_STATUS_RECEIVED = "PKG_INV_ORDER.PROC_DELI_STATUS_RECEIVED";
        private const string PROC_GET_PHONE_BY_DELY_CODE = "PKG_INV_ORDER.PROC_GET_PHONE_BY_DELY_CODE";
        private const string PROC_GET_ALL_DELI_STATUS = "PKG_INV_ORDER.PROC_GET_ALL_DELI_STATUS";
        private const string PROC_INVEST_PAYMENT_DATE = "PKG_INV_ORDER.PROC_INVEST_PAYMENT_DATE_ADD";
        private const string PROC_APP_REQUEST_DELIVERY_STATUS = "PKG_INV_ORDER.PROC_ORDER_DELIVERY_STATUS";
        private const string PROC_INVEST_GET_ALL_PAYMENT_DATE = "PKG_INV_ORDER.PROC_INV_GET_ALL_PAYMENT_DATE";
        private const string PROC_CALENDAR_NEXT_WORK_DAY = "PKG_INV_CALENDAR.PROC_CALENDAR_NEXT_WORK_DAY";
        private const string PROC_APP_CHECK_SALE_CODE = "PKG_INV_ORDER.PROC_APP_CHECK_SALE_CODE";
        private const string PROC_UPDATE_SETTLEMENT_METHOD = "PKG_INV_ORDER.PROC_UPDATE_SETTLEMENT_METHOD";
        private const string PROC_PAYMENT_SUM_MONEY = "PKG_INV_INTEREST_PAYMENT.PROC_PAYMENT_SUM_MONEY";
        private const string PROC_APP_SALE_VIEW_ORDER = "PKG_INV_ORDER.PROC_APP_SALE_VIEW_ORDER";
        private const string PROC_INV_ORDER_HISTORY_UPDATE_GET_ALL = "PKG_INV_HISTORY_UPDATE.PROC_INV_ORDER_HISTORY_UPDATE_GET_ALL";
        private const string PROC_INVESTOR_GET_LIST_ORDER_BY_STOCK = "PKG_INV_ORDER.PROC_INVESTOR_GET_LIST_ORDER_BY_STOCK";
        private const string PROC_ORDER_PROCESS_CONTRACT = "PKG_INV_ORDER.PROC_ORDER_PROCESS_CONTRACT";
        private const string PROC_APP_CANCEL_ORDER = "PKG_INV_ORDER.PROC_APP_CANCEL_ORDER";
        public InvestOrderRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public InvOrder Add(InvOrder entity, bool closeConnection = true)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<InvOrder>(PROC_ORDER_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_CIF_CODE = entity.CifCode,
                pv_DISTRIBUTION_ID = entity.DistributionId,
                pv_POLICY_ID = entity.PolicyId,
                pv_POLICY_DETAIL_ID = entity.PolicyDetailId,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_IS_INTEREST = entity.IsInterest,
                pv_INVESTOR_BANK_ACC_ID = entity.InvestorBankAccId,
                pv_REFERRAL_CODE = entity.SaleReferralCode,
                pv_REFERRAL_CODE_SUB = entity.SaleReferralCodeSub,
                pv_DEPARTMENT_ID_SUB = entity.DepartmentIdSub,
                pv_INVESTOR_IDEN_ID = entity.InvestorIdenId,
                pv_CONTRACT_ADDRESS_ID = entity.ContractAddressId,
                SESSION_USERNAME = entity.CreatedBy,
            }, closeConnection);
            return result;
        }

        public PagingResult<InvOrder> FindAll(int? tradingProviderId, int? groupStatus, InvestOrderFilterDto input, List<int> tradingProviderChildIds)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvOrder>(PROC_ORDER_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = input.PageSize,
                PAGE_NUMBER = input.PageNumber,
                KEYWORD = input.Keyword,
                pv_STATUS = input.Status,
                pv_GROUP_STATUS = groupStatus,
                pv_SOURCE = input.Source,
                pv_DISTRIBUTION_ID = input.DistributionId,
                pv_POLICY = input.Policy,
                pv_POLICY_DETAIL_ID = input.PolicyDetailId,
                pv_CUSTOMER_NAME = input.CustomerName,
                pv_CONTRACT_CODE = input.ContractCode,
                pv_TRADING_DATE = input.TradingDate,
                pv_INVEST_DATE = input.InvestDate,
                pv_DELIVERY_STATUS = input.DeliveryStatus,
                pv_PHONE = input.Phone,
                pv_CIF_CODE = input.CifCode,
                pv_ORDERER = input.Orderer,
                pv_TRADING_PROVIDER_CHILD_IDS = tradingProviderChildIds != null ? string.Join(',', tradingProviderChildIds) : null,
                pv_CONTRACT_CODE_GEN = input.ContractCodeGen
            });
            return result;
        }

        public List<InvOrder> GetAllListOrder(int? tradingProviderId, int? status, string phone, string contractCode, string taxCode, int? orderId, int? projectId, List<int> tradingProviderChildIds, int? methodInterest = null)
        {
            return _oracleHelper.ExecuteProcedure<InvOrder>(PROC_INV_ORDER_GET_LIST, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status,
                pv_PHONE = phone,
                pv_CONTRACT_CODE = contractCode,
                pv_TAX_CODE = taxCode,
                pv_ORDER_ID = orderId,
                pv_PROJECT_ID = projectId,
                pv_METHOD_INTEREST = methodInterest,
                pv_TRADING_PROVIDER_CHILD_IDS = tradingProviderChildIds != null ? string.Join(',', tradingProviderChildIds) : null,
            }).ToList();
        }

        public PagingResult<InvOrder> FindAllDeliveryStatus(int? tradingProviderId, int? groupStatus, InvestOrderFilterDto input, List<int> tradingProviderChildIds)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvOrder>(PROC_GET_ALL_DELI_STATUS, new
            {
                pv_ORDER_ID = input.OrderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = input.PageSize,
                PAGE_NUMBER = input.PageNumber,
                KEYWORD = input.Keyword,
                pv_STATUS = input.Status,
                pv_GROUP_STATUS = groupStatus,
                pv_SOURCE = input.Source,
                pv_DISTRIBUTION_ID = input.DistributionId,
                pv_POLICY = input.Policy,
                pv_POLICY_DETAIL_ID = input.PolicyDetailId,
                pv_CUSTOMER_NAME = input.CustomerName,
                pv_CONTRACT_CODE = input.ContractCode,
                pv_TRADING_DATE = input.TradingDate,
                pv_DELIVERY_STATUS = input.DeliveryStatus,
                pv_PHONE = input.Phone,
                pv_CIF_CODE = input.CifCode,
                pv_PENDING_DATE = input.PendingDate,
                pv_DELIVERY_DATE = input.DeliveryDate,
                pv_RECEIVED_DATE = input.ReceivedDate,
                pv_FINISHED_DATE = input.FinishedDate,
                pv_DATE = input.Date,
                pv_CONTRACT_CODE_GEN = input.ContractCodeGen,
                pv_TRADING_PROVIDER_CHILD_IDS = tradingProviderChildIds != null ? string.Join(',', tradingProviderChildIds) : null,
            });
            return result;
        }

        public InvOrder FindById(long id, int? tradingProviderId = null, int? partnerId = null, bool isClose = true)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<InvOrder>(PROC_ORDER_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            }, isClose);
            return result;
        }

        public int Update(InvOrder entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_IS_INTEREST = entity.IsInterest,
                pv_POLICY_DETAIL_ID = entity.PolicyDetailId,
                pv_REFERRAL_CODE = entity.SaleReferralCode,
                pv_REFERRAL_CODE_SUB = entity.SaleReferralCodeSub,
                pv_DEPARTMENT_ID_SUB = entity.DepartmentIdSub,
                pv_DEPARTMENT_ID = entity.DepartmentId,
                pv_INVESTOR_BANK_ACC_ID = entity.InvestorBankAccId,
                pv_CONTRACT_ADDRESS_ID = entity.ContractAddressId,
                SESSION_USERNAME = entity.ModifiedBy
            });
            return result;
        }

        public AppOrderDto OrderInvestorAdd(AppCheckOrderDto input, int investorId, bool isCheck, string otp = null, string ipAddress = null, bool isSelfDoing = true, int? saleId = null, string username = null, bool closeConnection = true)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<AppOrderDto>(PROC_ORDER_INVESTOR_ADD, new
            {
                pv_INVESTOR_ID = investorId,
                pv_POLICY_DETAIL_ID = input.PolicyDetailId,
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
                pv_SALE_ID = saleId,
                pv_USERNAME = username,
            }, closeConnection);
            return result;
        }

        public int UpdateTotalValue(int id, int tradingProviderId, decimal? totalValue, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_TOTAL_VALUE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TOTAL_VALUE = totalValue,
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

        public int UpdatePolicyDetail(int id, int tradingProviderId, int? policyDetailId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_POLICY_DETAIL, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_POLICY_DETAIL_ID = policyDetailId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int UpdateSource(int id, int tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_SOURCE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int OrderApprove(int id, int tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_APPROVE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int OrderCancel(int id, int tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_CANCEL, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                SESSION_USERNAME = modifiedBy,
            });
            return result;
        }

        public int UpdateInvestorBankAccount(int id, int? bankAccId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_UPDATE_INVESTOR_BANK_ACCOUNT, new
            {
                pv_ORDER_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVESTOR_BANK_ACC_ID = bankAccId,
                SESSION_USERNAME = modifiedBy,
            });
        }

        public int UpdateInfoCustomer(int id, int? bankAccId, int? contractAdressId, int? investorIdenId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_INFO_CUSTOMER, new
            {
                pv_ORDER_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVESTOR_BANK_ACC_ID = bankAccId,
                pv_CONTRACT_ADDRESS_ID = contractAdressId,
                pv_INVESTOR_IDENTIFICATION_ID = investorIdenId,
                SESSION_USERNAME = modifiedBy,
            });
        }

        /// <summary>
        /// Lấy danh sách hợp đồng đầu tư cho investor
        /// 
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="groupStatus"> nếu truyền  null: status = 5,  
        /// 1: status = 5,6
        /// 2: status = 1,2,4
        /// 3: status = 8</param>
        /// <returns></returns>
        public IEnumerable<AppInvestOrderInvestorDto> AppGetAll(int investorId, int? groupStatus)
        {
            return _oracleHelper.ExecuteProcedure<AppInvestOrderInvestorDto>(PROC_APP_ORDER_GET_ALL, new
            {
                pv_INVESTOR_ID = investorId,
                pv_GROUP_STATUS = groupStatus
            });
        }

        public ViewOrderDto AppGetOrderDetail(int? investorId, int orderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ViewOrderDto>(PROC_APP_ORDER_DETAIL, new
            {
                pv_INVESTOR_ID = investorId,
                pv_ORDER_ID = orderId,
            });
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

        public decimal SumValue(int? tradingProviderId, int distributionId)
        {
            decimal result = 0;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_DISTRIBUTION_ID", distributionId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_ORDER_SUM_MONEY, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }

        /// <summary>
        /// Tính hạn mức đầu tư tối đa
        /// </summary>
        /// <returns></returns>
        public decimal MaxTotalInvestment(int? tradingProviderId, int distributionId)
        {
            decimal result = 0;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_DISTRIBUTION_ID", distributionId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_MAX_TOTAL_INVESTMENT, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }

        public int ChangeDeliveryStatusDelivered(int orderId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELIVERY_STATUS_DELIVERED, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                SESSION_USERNAME = modifiedBy
            });
        }
        public int ChangeDeliveryStatusReceived(int orderId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELIVERY_STATUS_RECEIVED, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                SESSION_USERNAME = modifiedBy
            });
        }
        public int ChangeDeliveryStatusDone(int orderId, int tradingProviderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELIVERY_STATUS_DONE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                SESSION_USERNAME = modifiedBy
            });
        }
        public int ChangeDeliveryStatusReceviredApp(string deliveryCode, int investorId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_DELI_STATUS_RECEIVED, new
            {
                pv_INVESTOR_ID = investorId,
                pv_DELIVERY_CODE = deliveryCode,
                SESSION_USERNAME = modifiedBy
            });
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

        public PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<PhoneReceiveDto>(PROC_GET_PHONE_BY_DELY_CODE, new
            {
                pv_DELIVERY_CODE = deliveryCode
            });
            return result;

        }
        /// <summary>
        /// Thêm ngày trả invest vào bảng Ngày trả invest
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="periodIndex"></param>
        /// <param name="payDate"></param>
        /// <returns></returns>
        public int AddPaymentDate(int orderId, int periodIndex, DateTime? payDate)
        {

            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_INVEST_PAYMENT_DATE, new
            {
                pv_ORDER_ID = orderId,
                pv_PERIOD_INDEX = periodIndex,
                pv_PAY_DATE = payDate
            });
            return result;
        }

        /// <summary>
        /// Yêu cầu nhận hợp đồng invest
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AppRequestDeliveryStatus(int orderId, int investorId, string username)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_REQUEST_DELIVERY_STATUS, new
            {
                pv_ID = orderId,
                pv_INVESTOR_ID = investorId,
                SESSION_USERNAME = username
            });
        }
        /// <summary>
        /// Lấy ra danh sách ngày tri trả invest
        /// </summary>
        /// <returns></returns>
        public List<DateTime> GetAllPaymentDate(int orderId)
        {
            var result = _oracleHelper.ExecuteProcedure<DateTime>(PROC_INVEST_GET_ALL_PAYMENT_DATE, new
            {
                pv_ORDER_ID = orderId
            }).ToList();
            return result;
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

        /// <summary>
        /// Tính ngày đáo hạn 
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="isClose"></param>
        /// <returns></returns>
        public DateTime CalculateDueDate(PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, DateTime? distributionCloseSellDate, bool isClose = true)
        {
            //Số kỳ trả lợi tức, thời gian KH đầu tư
            int soKyDaoHan = policyDetail.PeriodQuantity ?? 0;
            //Ngày đáo hạn
            DateTime ngayDaoHan = ngayBatDauTinhLai.Date;
            if (policyDetail.InterestDays != null) //nếu có cài ngày chính xác
            {
                ngayDaoHan = ngayDaoHan.AddDays(policyDetail.InterestDays.Value);
            }
            else //không cài ngày chính xác
            {
                if (policyDetail.PeriodType == PeriodUnit.DAY)
                {
                    ngayDaoHan = ngayDaoHan.AddDays(soKyDaoHan);
                }
                else if (policyDetail.PeriodType == PeriodUnit.MONTH)
                {
                    ngayDaoHan = ngayDaoHan.AddMonths(soKyDaoHan);
                }
                else if (policyDetail.PeriodType == PeriodUnit.YEAR)
                {
                    ngayDaoHan = ngayDaoHan.AddYears(soKyDaoHan);
                }
            }
            // Kiểm tra xem ngày đáo hạn có vượt quá ngày đóng phân phối distribution
            if (distributionCloseSellDate != null)
            {
                ngayDaoHan = (distributionCloseSellDate.Value.Date < ngayDaoHan) ? distributionCloseSellDate.Value.Date : ngayDaoHan;
            }
            ngayDaoHan = NextWorkDay(ngayDaoHan.Date, policyDetail.TradingProviderId, isClose);
            ngayDaoHan = ngayDaoHan.Date;
            return ngayDaoHan;
        }

        /// <summary>
        /// Check sale có đủ điều kiện ko trước khi investor tự đặt lệnh. Nếu đủ điều kiện thì lấy ra thông tin sale
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="distributionId"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public ViewCheckSaleBeforeAddOrderDto AppCheckSaleBeforeAddOrder(string referralCode, int distributionId, int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<ViewCheckSaleBeforeAddOrderDto>(PROC_APP_CHECK_SALE_CODE, new
            {
                pv_REFERRAL_CODE = referralCode,
                pv_DISTRIBUTION_ID = distributionId,
                pv_INVESTOR_ID = investorId
            });
        }

        public decimal InterestPaymentSumMoney(DateTime payDate, long orderId, int tradingProviderId)
        {
            var result = new Decimal();
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_PAY_DATE", payDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_ORDER_ID", orderId, OracleMappingType.Long, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_PAYMENT_SUM_MONEY, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return result;
        }

        public PagingResult<HistoryUpdateDto> GetAllOrderHistoryUpdate(int pageNumber, int? pageSize, string keyword, int orderId)
        {
            return _oracleHelper.ExecuteProcedurePaging<HistoryUpdateDto>(PROC_INV_ORDER_HISTORY_UPDATE_GET_ALL, new
            {
                pv_ORDER_ID = orderId,
                PAGE_SIZE = pageSize,
                KEYWORD = keyword,
                PAGE_NUMBER = pageNumber,
            });
        }

        public List<InvOrder> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount, DateTime? startDate, DateTime? endDate)
        {
            return _oracleHelper.ExecuteProcedure<InvOrder>(PROC_INVESTOR_GET_LIST_ORDER_BY_STOCK, new
            {
                pv_SECURITY_COMPANY = securityCompany,
                pv_STOCK_TRADING_ACCOUNT = stockTradingAccount,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate
            }).ToList();
        }

        public int ProcessContract(InvOrder entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_PROCESS_CONTRACT, new
            {
                pv_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                SESSION_USERNAME = entity.PendingDateModifiedBy
            });
            return result;
        }

        /// Hủy lệnh hợp đồng trong trạng thái khởi tạo hoặc chờ thanh toán của App
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="orderId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public int AppCancelOrder(int investorId, int orderId, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_CANCEL_ORDER, new
            {
                pv_INVESTOR_ID = investorId,
                pv_ORDER_ID = orderId,
                SESSION_USERNAME = modifiedBy,
            });
        }
    }
}
