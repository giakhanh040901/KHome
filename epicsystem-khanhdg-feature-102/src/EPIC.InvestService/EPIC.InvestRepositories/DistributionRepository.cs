using Dapper.Oracle;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestSharedEntites.Dto.Order;
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
    public class DistributionRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string PROC_DISTRIBUTION_ADD = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_ADD";
        private const string PROC_DISTRIBUTION_UPDATE = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_UPDATE";
        private const string PROC_DISTRIBUTION_UPDATE_BANK = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_UPDATE_BANK";
        private const string PROC_DISTRIBUTION_DELETE = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_DELETE";
        private const string PROC_DISTRIBUTION_GET_ALL = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_GET_ALL";
        private const string PROC_DISTRIBUTION_GET = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_GET";
        private const string PROC_DISTRIBUTION_GET_ALL_ORDER = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_GET_ALL_OR";
        private const string PROC_CHECK_TRADING_HAVE_DISTRIBUTION = "PKG_INV_DISTRIBUTION.PROC_CHECK_TRADING_HAVE_DISTRIBUTION";

        #region DistributionTradingBankAcc
        private const string PROC_GET_ALL_TRADING_BANK_ACC = "PKG_INV_DIS_TRADING_BANK_ACC.PROC_GET_ALL_TRADING_BANK_ACC";
        private const string PROC_TRADING_BANK_ACC_ADD = "PKG_INV_DIS_TRADING_BANK_ACC.PROC_TRADING_BANK_ACC_ADD";
        private const string PROC_TRADING_BANK_ACC_DELETE = "PKG_INV_DIS_TRADING_BANK_ACC.PROC_TRADING_BANK_ACC_DELETE";
        private const string PROC_GET_LIST_BANK_BY_DISTRIBUTION = "PKG_INV_DIS_TRADING_BANK_ACC.GET_LIST_BANK_BY_DISTRIBUTION_ID";
        #endregion

        private const string PROC_IS_CLOSE = "PKG_INV_DISTRIBUTION.PROC_IS_CLOSE";
        private const string PROC_IS_SHOW_APP = "PKG_INV_DISTRIBUTION.PROC_IS_SHOW_APP";
        private const string PROC_DISTRIBUTION_REQUEST = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_REQUEST";
        private const string PROC_DISTRIBUTION_APPROVE = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_APPROVE";
        private const string PROC_DISTRIBUTION_CANCEL = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_CANCEL";
        private const string PROC_DISTRIBUTION_CHECK = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_CHECK";
        private const string PROC_DISTRIBUTION_UPDATE_OVERVIEW_CONTENT = "PKG_INV_DISTRIBUTION.PROC_DISTRIBUTION_UPDATE_OVERVIEW_CONTENT";
        private const string PROC_GET_PAY_BANK_BY_DISTRBUTION_ID = "PKG_INV_DIS_TRADING_BANK_ACC.GET_PAY_BANK_BY_DISTRIBUTION_ID";
        #region App
        private const string PROC_APP_DISTRIBUTION_GET_ALL = "PKG_INV_DISTRIBUTION.PROC_APP_DISTRIBUTION_GET_ALL";
        private const string PROC_APP_DISTRIBUTION_GET_INFO = "PKG_INV_DISTRIBUTION.PROC_APP_DISTRIBUTION_GET_INFO";
        #endregion

        public DistributionRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public Distribution Add(Distribution entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Distribution>(
                    PROC_DISTRIBUTION_ADD, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_PROJECT_ID = entity.ProjectId,
                        pv_OPEN_CELL_DATE = entity.OpenCellDate,
                        pv_CLOSE_CELL_DATE = entity.CloseCellDate,
                        pv_METHOD_INTEREST = entity.MethodInterest,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }


        public PagingResult<Distribution> FindAll(int pageSize, int pageNumber, string keyword, int? status, int? tradingProviderId, string isClose = null)
        {
            return _oracleHelper.ExecuteProcedurePaging<Distribution>(PROC_DISTRIBUTION_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_IS_CLOSE = isClose
            });
        }

        public List<Distribution> FindAllOrder(int? tradingProviderId, List<int> tradingProviderChildIds)
        {
            var result = _oracleHelper.ExecuteProcedure<Distribution>(PROC_DISTRIBUTION_GET_ALL_ORDER, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TRADING_PROVIDER_CHILD_IDS = tradingProviderChildIds != null ? string.Join(',', tradingProviderChildIds) : null,
            }).ToList();
            return result;
        }


        public Distribution FindById(int id, int? tradingProviderId = null, int? partnerId = null)
        {
            var rslt = _oracleHelper.ExecuteProcedureToFirst<Distribution>(PROC_DISTRIBUTION_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            });
            return rslt;
        }

        public int Update(Distribution entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_DISTRIBUTION_UPDATE, new
                    {
                        pv_ID = entity.Id,
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_OPEN_CELL_DATE = entity.OpenCellDate,
                        pv_CLOSE_CELL_DATE = entity.CloseCellDate,
                        pv_IMAGE = entity.Image,
                        pv_METHOD_INTEREST = entity.MethodInterest,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int UpdateBank(int id, int businessCustomerBankAccId, string modifiedBy, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_UPDATE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_BUSINESS_CUSTOMER_BANK_ID = businessCustomerBankAccId,
                SESSION_USERNAME = modifiedBy
            });
        }

        public int UpdateOverviewContent(UpdateDistributionOverviewContentDto input, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_UPDATE_OVERVIEW_CONTENT, new
            {
                pv_ID = input.Id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_OVERVIEW_IMAGE_URL = input.OverviewImageUrl,
                pv_CONTENT_TYPE = input.ContentType,
                pv_OVERVIEW_CONTENT = input.OverviewContent
            });
        }

        public int IsClose(int id, string isClose, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_IS_CLOSE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_CLOSE = isClose
            });
        }

        public int IsShowApp(int id, string isShowApp, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_IS_SHOW_APP, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public int DistributionRequest(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_REQUEST, new
            {
                pv_ID = id,
            });
            return result;
        }

        public int DistributionApprove(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_APPROVE, new
            {
                pv_ID = id
            });
            return result;
        }

        public int DistributionCheck(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_CHECK, new
            {
                pv_ID = id
            });
            return result;
        }

        public int DistributionCancel(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_CANCEL, new
            {
                pv_ID = id
            });
            return result;
        }

        public IEnumerable<ProjectDistributionFindDto> FindAllProject(string keyword, int? investorId, string periodType)
        {
            return _oracleHelper.ExecuteProcedure<ProjectDistributionFindDto>(PROC_APP_DISTRIBUTION_GET_ALL, new
            {
                KEY_WORD = keyword,
                pv_INVESTOR_ID = investorId,
                pv_PERIOD_TYPE = periodType
            });
        }

        public AppProjectDto AppFindDistributionById(int id, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppProjectDto>(PROC_APP_DISTRIBUTION_GET_INFO, new
            {
                pv_DISTRIBUTION_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int AddDistributionTradingBankAcc(DistributionTradingBankAccount entity, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_TRADING_BANK_ACC_ADD, new
                    {
                        pv_DISTRIBUTION_ID = entity.DistributionId,
                        pv_TRADING_BACK_ACC_ID = entity.TradingBankAccId,
                        SESSION_USERNAME = entity.CreatedBy,
                        pv_TRADING_PROVIDER_ID = tradingProviderId,
                        pv_TYPE = entity.Type
                    }, false);
        }

        public List<DistributionTradingBankAccount> GetAllTradingBankByDistribution(int distributionId, int? type = null)
        {
            var result = _oracleHelper.ExecuteProcedure<DistributionTradingBankAccount>(PROC_GET_ALL_TRADING_BANK_ACC, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TYPE = type
            }).ToList();
            return result;
        }

        public int DeletedDistributionTradingBankAcc(int id)
        {
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_BANK_ACC_DELETE, new
            {
                pv_ID = id
            });
            return rslt;
        }

        /// <summary>
        /// Lấy ngân hàng thu của distribution
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public List<AppTradingBankAccountDto> GetListBankCollectByDistributionId(int distributionId)
        {
            var result = _oracleHelper.ExecuteProcedure<AppTradingBankAccountDto>(PROC_GET_LIST_BANK_BY_DISTRIBUTION, new
            {
                pv_DISTRIBUTION_ID = distributionId
            }).ToList();
            return result;
        }

        public List<DistributionTradingBankAccount> FindBankByDistributionId(int distributionId, int type)
        {
            var result = _oracleHelper.ExecuteProcedure<DistributionTradingBankAccount>(PROC_GET_PAY_BANK_BY_DISTRBUTION_ID, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TYPE = type
            }).ToList();
            return result;
        }

        /// <summary>
        /// Kiem tra dai ly co phan phoi Invest
        /// </summary>
        public string CheckTradingHaveDistributionInvest(HashSet<int> tradingProviderIds)
        {
            var result = YesNo.NO;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_IDS", string.Join(',', tradingProviderIds), OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_CHECK_TRADING_HAVE_DISTRIBUTION, parameters);

            result = parameters.Get<string>("pv_RESULT");
            return result;
        }
    }
}
