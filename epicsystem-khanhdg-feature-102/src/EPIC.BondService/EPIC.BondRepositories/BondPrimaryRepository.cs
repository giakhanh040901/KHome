using Dapper.Oracle;
using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ProductBondPrimary;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondPrimaryRepository : BaseRepository
    {
        private const string ADD_EP_PRODUCT_BOND_PRIMARY_PROC = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_ADD";
        private const string DELETE_EP_PRODUCT_BOND_PRIMARY_PROC = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_DELETE";
        private const string PROC_BOND_PRIMARY_REQUEST = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_REQUEST";
        private const string PROC_BOND_PRIMARY_APPROVE = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_APPROVE";
        //private const string CHANGE_APPROVE_TO_CLOSE_EP_PRODUCT_BOND_PRIMARY_PROC = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_A_TO_C";
        private const string GET_ALL_EP_PRODUCT_BOND_PRIMARY_PROC = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_GET_ALL";
        private const string UPDATE_EP_PRODUCT_BOND_PRIMARY_PROC = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_UPDATE";
        private const string GET_EP_PRODUCT_BOND_PRIMARY_PROC = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_GET";
        private const string GET_EP_BOND_PRIMARY_FIND_BY_BOND_INFO = "PKG_PRODUCT_BOND_PRIMARY.PROC_PRIMARY_FIND_BY_INFO";
        private const string GET_EP_BOND_PRIMARY_FIND_BY_TRADING = "PKG_PRODUCT_BOND_PRIMARY.PROC_PRIMARY_FIND_BY_TRADING";
        private const string PROC_BOND_PRIMARY_BY_TRADING_FOR_PARTNER = "PKG_PRODUCT_BOND_PRIMARY.PROC_PRIMARY_BY_TRADING_FOR_PA";
        private const string PROC_BOND_PRIMARY_CHECK = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_CHECK";
        private const string PROC_BOND_PRIMARY_CANCEL = "PKG_PRODUCT_BOND_PRIMARY.PROC_BOND_PRIMARY_CANCEL";
        private const string PROC_SUM_QUANTITY_PRIMARY = "PKG_PRODUCT_BOND_PRIMARY.PROC_SUM_QUANTITY_PRIMARY";

        public BondPrimaryRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PagingResult<BondPrimary> FindAll(int pageSize, int pageNumber, string keyword)
        {
            var productBondPrimary = _oracleHelper.ExecuteProcedurePaging<BondPrimary>(GET_ALL_EP_PRODUCT_BOND_PRIMARY_PROC, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return productBondPrimary;
        }

        public PagingResult<BondPrimary> FindAllProductBondPrimary(int partnerId,int pageSize, int pageNumber, string keyword, string status)
        {
            var productBondPrimary = _oracleHelper.ExecuteProcedurePaging<BondPrimary>(GET_ALL_EP_PRODUCT_BOND_PRIMARY_PROC, new
            {
                pv_PARTNER_ID = partnerId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status
            });
            return productBondPrimary;
        }

        public IEnumerable<BondPrimary> GetAllByInfo(int productBondInfoId)
        {
            return _oracleHelper.ExecuteProcedure<BondPrimary>(GET_EP_BOND_PRIMARY_FIND_BY_BOND_INFO, new
            {
                pv_PRODUCT_BOND_ID = productBondInfoId,
            });
        }

        public IEnumerable<BondPrimary> GetAllByTrading(int tradingProviderId)
        {
            var result =  _oracleHelper.ExecuteProcedure<BondPrimary>(GET_EP_BOND_PRIMARY_FIND_BY_TRADING, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        /// <summary>
        /// Danh sách phát hành sơ cấp cho partner tạo hợp đồng phân phối
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public IEnumerable<BondPrimary> GetAllByTradingForPartner(int tradingProviderId, int partnerId)
        {
            var result = _oracleHelper.ExecuteProcedure<BondPrimary>(PROC_BOND_PRIMARY_BY_TRADING_FOR_PARTNER, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            });
            return result;
        }

        public BondPrimary FindById(int id, int? partnerId)
        {
            BondPrimary productBondPrimary = _oracleHelper.ExecuteProcedureToFirst<BondPrimary>(GET_EP_PRODUCT_BOND_PRIMARY_PROC, new
            {
                pv_BOND_PRIMARY_ID = id,
                pv_PARTNER_ID = partnerId
            });
            return productBondPrimary;
        }

        public BondPrimary Add(BondPrimary entity, bool closeConnection = true)
        {
            _logger.LogInformation("Add Product Bond Primary");
            return _oracleHelper.ExecuteProcedureToFirst<BondPrimary>(ADD_EP_PRODUCT_BOND_PRIMARY_PROC, new
            {
                pv_PARTNER_ID = entity.PartnerId,
                pv_PRODUCT_BOND_ID = entity.BondId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_BUSINESS_CUS_BANK_ACC_ID = entity.BusinessCustomerBankAccId,
                pv_CONTRACT_CODE = entity.ContractCode,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_BOND_TYPE_ID = 0,
                pv_OPEN_CELL_DATE = entity.OpenSellDate,
                pv_CLOSE_CELL_DATE = entity.CloseSellDate,
                pv_QUANTITY = entity.Quantity,
                pv_MIN_MONEY = entity.MinMoney,
                pv_PRICE_TYPE = entity.PriceType,
                pv_MAX_INVESTOR = entity.MaxInvestor,
                SESSION_USERNAME = entity.CreatedBy,
            }, closeConnection);
        }

        public int Update(BondPrimary entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_EP_PRODUCT_BOND_PRIMARY_PROC, new
            {
                pv_BOND_PRIMARY_ID = entity.Id,
                pv_PARTNER_ID = entity.PartnerId,
                pv_PRODUCT_BOND_ID = entity.BondId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_BUSINESS_CUS_BANK_ACC_ID = entity.BusinessCustomerBankAccId,
                pv_CONTRACT_CODE = entity.ContractCode,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_BOND_TYPE_ID = 0,
                pv_OPEN_CELL_DATE = entity.OpenSellDate,
                pv_CLOSE_CELL_DATE = entity.CloseSellDate,
                pv_QUANTITY = entity.Quantity,
                pv_MIN_MONEY = entity.MinMoney,
                pv_PRICE_TYPE = entity.PriceType,
                pv_MAX_INVESTOR = entity.MaxInvestor,
                pv_STATUS = entity.Status,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int Delete(int id, int partnerId)
        {
            _logger.LogInformation($"Delete Product Bond Primary: {id}");
            return _oracleHelper.ExecuteProcedureNonQuery(DELETE_EP_PRODUCT_BOND_PRIMARY_PROC, new
            {
                pv_PARTNER_ID = partnerId,
                pv_BOND_PRIMARY_ID = id
            });
        }
        public int BondPrimaryApprove(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_PRIMARY_APPROVE, new
            {
                pv_BOND_PRIMARY_ID = id
            });
            return result;
        }

        public int BondPrimaryRequest(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_PRIMARY_REQUEST, new
            {
                pv_BOND_PRIMARY_ID = id,
            });
            return result;
        }

        public int BondPrimaryCheck(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_PRIMARY_CHECK, new
            {
                pv_BOND_PRIMARY_ID = id
            });
            return result;
        }

        public int BondPrimaryCancel(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_PRIMARY_CANCEL, new
            {
                pv_BOND_PRIMARY_ID = id
            });
            return result;
        }

        public int SumQuantityPrimary(int? tradingProviderId, int bondInfoId)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_PRODUCT_BOND_ID", bondInfoId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_QUANTITY_PRIMARY, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (int)result;
        }
    }
}
