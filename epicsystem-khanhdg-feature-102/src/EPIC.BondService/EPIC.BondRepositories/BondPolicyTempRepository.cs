using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
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
    public class BondPolicyTempRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string PROC_ADD_PRODUCT_POLICY_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_TEMP_ADD";
        private static string PROC_ADD_PRODUCT_POLICY_DE_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_DE_TEMP_ADD";
        private static string PROC_UPDATE_PRODUCT_POLICY_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_TEMP_UPDATE";
        private static string PROC_UPDATE_PRODUCT_POLICY_DETAIL_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_POLICY_DE_TEMP_UPDATE";
        private static string PROC_CHANGE_STATUS_PRODUCT_POLICY_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_POLICY_TEMP_RE_STATUS";
        private static string PROC_CHANGE_STATUS_PRODUCT_POLICY_DETAIL_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_POLICY_DE_TEMP_RE_STATUS";
        private static string PROC_DELETE_PRODUCT_POLICY_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_TEMP_DELETE";
        private static string PROC_DELETE_PRODUCT_POLICY_DETAIL_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_POLICY_DE_TEMP_DELETE";
        private static string PROC_GET_PRODUCT_POLICY_DETAIL_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_DE_TEMP_GET";
        private static string PROC_GET_PRODUCT_POLICY_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_TEMP_GET";
        private static string PROC_GET_ALL = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_TEMP_GET_ALL";
        private static string PROC_GET_BY_ID = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_BOND_POLICY_TEMP_FIND";
        private static string PROC_GET_POLICY_DE_TEMP_BY_POLICY_TEMP = "PKG_PRODUCT_BOND_POLICY_TEMP.PROC_POL_DETAIL_TEMP_ALL";

        public BondPolicyTempRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public PagingResult<BondPolicyTemp> FindAll(int pageSize, int pageNumber, string keyword)
        {
            throw new NotImplementedException();
        }

        public PagingResult<ProductBondPolicyTempView> FindAllProductBondPolicyTemp(int pageSize, int pageNumber, string keyword, string status, decimal? classify)
        {
            var productBondPolicyTemp = _oracleHelper.ExecuteProcedurePaging<ProductBondPolicyTempView>(PROC_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_CLASSIFY = classify,
            });
            return productBondPolicyTemp;
        }

        /// <summary>
        /// Lấy Chính sách theo Id kèm theo kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductBondPolicyTempView> FindProductBondPolicyById(int id)
        {
            List<ProductBondPolicyTempView> productBondPolicyTempView = _oracleHelper.ExecuteProcedure<ProductBondPolicyTempView>(PROC_GET_BY_ID, new
            {
                pv_BOND_POLICY_TEMP_ID = id,
            }).ToList();
            return productBondPolicyTempView;
        }

        public BondPolicyTemp FindById(int id)
        {
            BondPolicyTemp productBondPolicyTemp = _oracleHelper.ExecuteProcedureToFirst<BondPolicyTemp>(PROC_GET_PRODUCT_POLICY_TEMP, new
            {
                pv_BOND_POLICY_TEMP_ID = id,
            });
            return productBondPolicyTemp;
        }

        public BondPolicyDetailTemp FindBondPolicyDetailTempById(int id)
        {
            BondPolicyDetailTemp productBondPolicyDetailTemp = _oracleHelper.ExecuteProcedureToFirst<BondPolicyDetailTemp>(PROC_GET_PRODUCT_POLICY_DETAIL_TEMP, new
            {
                pv_BOND_POLICY_DETAIL_TEMP_ID = id,
            });
            return productBondPolicyDetailTemp;
        }
        
        public List<BondPolicyDetailTemp> FindBondPolicyDetailTempByPolicyTempId(int id)
        {
            List<BondPolicyDetailTemp> productBondPolicyDetailTemp = _oracleHelper.ExecuteProcedure<BondPolicyDetailTemp>(PROC_GET_POLICY_DE_TEMP_BY_POLICY_TEMP, new
            {
                pv_BOND_POLICY_TEMP_ID = id
            }).ToList();
            return productBondPolicyDetailTemp;
        }

        public BondPolicyTemp Add(BondPolicyTemp entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondPolicyTemp>(
                    PROC_ADD_PRODUCT_POLICY_TEMP, new
                    {
                        pv_CODE = entity.Code,
                        pv_NAME = entity.Name,
                        pv_TYPE = entity.Type,
                        pv_INVESTOR_TYPE = entity.InvestorType,
                        pv_INCOME_TAX = entity.IncomeTax,
                        pv_MIN_MONEY = entity.MinMoney,
                        pv_IS_TRANSFER = entity.IsTransfer,
                        pv_TRANSFER_TAX = entity.TransferTax,
                        pv_CLASSIFY = entity.Classify,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }


        public BondPolicyDetailTemp AddPolicyDetailTemp(BondPolicyDetailTemp entity)
        {

            return _oracleHelper.ExecuteProcedureToFirst<BondPolicyDetailTemp>(
                    PROC_ADD_PRODUCT_POLICY_DE_TEMP, new
                    {
                        pv_BOND_POLICY_TEMP_ID = entity.PolicyTempId,
                        pv_NAME = entity.Name,
                        pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                        pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                        pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                        pv_PERIOD_TYPE = entity.PeriodType,
                        pv_SHORT_NAME = entity.ShortName,
                        pv_PROFIT = entity.Profit,
                        pv_INTEREST_DAYS = entity.InterestDays,
                        pv_INTEREST_TYPE = entity.InterestType,
                        pv_STT = entity.STT,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public int Update(BondPolicyTemp entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_PRODUCT_POLICY_TEMP, new
            {
                pv_BOND_POLICY_TEMP_ID = entity.Id,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_TYPE = entity.Type,
                pv_INVESTOR_TYPE = entity.InvestorType,
                pv_INCOME_TAX = entity.IncomeTax,
                pv_MIN_MONEY = entity.MinMoney,
                pv_IS_TRANSFER = entity.IsTransfer,
                pv_TRANSFER_TAX = entity.TransferTax,
                pv_CLASSIFY = entity.Classify,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int UpdateProductBondPolicyDetailTemp(BondPolicyDetailTemp entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_PRODUCT_POLICY_DETAIL_TEMP, new
            {
                pv_BOND_POLICY_DETAIL_TEMP_ID = entity.Id,
                pv_NAME = entity.Name,
                pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                pv_PERIOD_TYPE = entity.PeriodType,
                pv_SHORT_NAME = entity.ShortName,
                pv_PROFIT = entity.Profit,
                pv_INTEREST_DAYS = entity.InterestDays,
                pv_INTEREST_TYPE = entity.InterestType,
                pv_STT = entity.STT,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation("Delete Product Bond Policy Template - SQL: {}", PROC_DELETE_PRODUCT_POLICY_TEMP);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DELETE_PRODUCT_POLICY_TEMP, new
            {
                pv_BOND_POLICY_TEMP_ID = id
            });
            return result;
        }

        public int DeleteProductBondPolicyDetailTemp(int id)
        {
            _logger.LogInformation("Delete Product Bond Policy Detail Template - SQL: {}", PROC_DELETE_PRODUCT_POLICY_DETAIL_TEMP);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DELETE_PRODUCT_POLICY_DETAIL_TEMP, new
            {
                pv_BOND_POLICY_DETAIL_TEMP_ID = id
            });
            return result;
        }

        public int UpdateStatusProductBondPolicyTemp(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_CHANGE_STATUS_PRODUCT_POLICY_TEMP, new
                    {
                        pv_BOND_POLICY_TEMP_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }

        public int UpdateStatusProductBondPolicyDetailTemp(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_CHANGE_STATUS_PRODUCT_POLICY_DETAIL_TEMP, new
                    {
                        pv_BOND_POLICY_DETAIL_TEMP_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }
    }
}
