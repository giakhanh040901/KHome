using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionNews;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class DistributionNewsRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_DISTRIBUTION_NEWS_ADD = "PKG_INV_DISTRIBUTION_NEWS.PROC_DISTRIBUTION_NEWS_ADD";
        private const string PROC_DISTRIBUTION_NEWS_DELETE = "PKG_INV_DISTRIBUTION_NEWS.PROC_DISTRIBUTION_NEWS_DELETE";
        private const string PROC_DISTRIBUTION_NEWS_GET = "PKG_INV_DISTRIBUTION_NEWS.PROC_DISTRIBUTION_NEWS_GET";
        private const string PROC_DISTRIBUTION_NEWS_GET_APP = "PKG_INV_DISTRIBUTION_NEWS.PROC_DISTRIBUTION_NEWS_GET_APP";
        private const string PROC_DISTRIBUTION_NEWS_UPDATE = "PKG_INV_DISTRIBUTION_NEWS.PROC_DISTRIBUTION_NEWS_UPDATE";
        private const string PROC_DISTRIBUTION_NEWS_GET_ALL = "PKG_INV_DISTRIBUTION_NEWS.PROC_DISTRIBUTION_NEWS_GET_ALL";
        private const string PROC_CHANGE_STATUS = "PKG_INV_DISTRIBUTION_NEWS.PROC_CHANGE_STATUS";

        public DistributionNewsRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public DistributionNews Add(DistributionNews entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<DistributionNews>(
                PROC_DISTRIBUTION_NEWS_ADD, new
                {
                    pv_DISTRIBUTION_ID = entity.DistributionId,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_IMG_URL = entity.ImgUrl,
                    pv_TITLE = entity.Title,
                    pv_CONTENT = entity.Content,
                    SESSION_USERNAME = entity.CreatedBy
                });
        }

        public PagingResult<ViewDistributionNewsDto> FindAll(int pageSize, int pageNumber, string status, int? DistributionId , int TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedurePaging<ViewDistributionNewsDto>(PROC_DISTRIBUTION_NEWS_GET_ALL, new
            {
                pv_DISTRIBUTION_ID = DistributionId,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                pv_STATUS = status
            });
        }

        public ViewDistributionNewsDto FindById(int id, int? TradingProviderId)
        {

            var result = _oracleHelper.ExecuteProcedureToFirst<ViewDistributionNewsDto>(PROC_DISTRIBUTION_NEWS_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
            });
            return result;
        }

        public ViewDistributionNewsDto FindbyDistributionId(int id)
        {

            var result = _oracleHelper.ExecuteProcedureToFirst<ViewDistributionNewsDto>(PROC_DISTRIBUTION_NEWS_GET_APP, new
            {
                pv_DISTRIBUTION_ID = id,
            });
            return result;
        }

        public int Update(DistributionNews entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_DISTRIBUTION_NEWS_UPDATE, new
                {
                    pv_ID = entity.Id,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_IMG_URL = entity.ImgUrl,
                    pv_TITLE = entity.Title,
                    pv_CONTENT = entity.Content,
                    SESSION_USERNAME = entity.ModifiedBy
                });
        }

        public int Delete(int id, int TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_NEWS_DELETE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
            });
        }

        public int UpdateStatus(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
            PROC_CHANGE_STATUS, new
            {
                pv_ID = id,
                pv_STATUS = status,
                SESSION_USERNAME = modifiedBy
            });
        }
    }
}
