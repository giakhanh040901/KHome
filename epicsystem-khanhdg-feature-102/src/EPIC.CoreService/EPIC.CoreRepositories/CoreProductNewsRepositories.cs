using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreProductNews;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class CoreProductNewsRepositories
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_CORE_PRODUCT_NEWS_ADD = "PKG_CORE_PRODUCT_NEWS.PROC_CORE_PRODUCT_NEWS_ADD";
        private const string PROC_CORE_PRODUCT_NEWS_DELETE = "PKG_CORE_PRODUCT_NEWS.PROC_CORE_PRODUCT_NEWS_DELETE";
        private const string PROC_CORE_PRODUCT_NEWS_GET = "PKG_CORE_PRODUCT_NEWS.PROC_CORE_PRODUCT_NEWS_GET";
        private const string PROC_CORE_PRODUCT_NEWS_UPDATE = "PKG_CORE_PRODUCT_NEWS.PROC_CORE_PRODUCT_NEWS_UPDATE";
        private const string PROC_CORE_PRODUCT_NEWS_GET_ALL = "PKG_CORE_PRODUCT_NEWS.PROC_CORE_PRODUCT_NEWS_GET_ALL";
        private const string PROC_CHANGE_STATUS = "PKG_CORE_PRODUCT_NEWS.PROC_CHANGE_STATUS";
        private const string PROC_CHANGE_FEATURE = "PKG_CORE_PRODUCT_NEWS.PROC_CHANGE_FEATURE";


        public CoreProductNewsRepositories(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public CoreProductNews Add(CoreProductNews entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CoreProductNews>(
                PROC_CORE_PRODUCT_NEWS_ADD, new
                {
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_IMG_URL = entity.ImgUrl,
                    pv_TITLE = entity.Title,
                    pv_CONTENT = entity.Content,
                    pv_FEATURE = entity.Feature,
                    pv_LOCATION =   entity.Location,
                    SESSION_USERNAME = entity.CreatedBy
                });
        }

        public PagingResult<ViewCoreProductNewsDto> FindAll(int pageSize, int pageNumber, string status, int TradingProviderId, int? location)
        {
            return _oracleHelper.ExecuteProcedurePaging<ViewCoreProductNewsDto>(PROC_CORE_PRODUCT_NEWS_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = TradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                pv_STATUS = status,
                pv_LOCATION = location
            });
        }

        public ViewCoreProductNewsDto FindById(int id, int? TradingProviderId)
        {

            var result = _oracleHelper.ExecuteProcedureToFirst<ViewCoreProductNewsDto>(PROC_CORE_PRODUCT_NEWS_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
            });
            return result;
        }

        public int Update(CoreProductNews entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_CORE_PRODUCT_NEWS_UPDATE, new
                {
                    pv_ID = entity.Id,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_IMG_URL = entity.ImgUrl,
                    pv_TITLE = entity.Title,
                    pv_CONTENT = entity.Content,
                    pv_LOCATION = entity.Location,
                    SESSION_USERNAME = entity.ModifiedBy
                });
        }
        public int Delete(int id, int TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_CORE_PRODUCT_NEWS_DELETE, new
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

        public int UpdateFeature(int id, string feature, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
            PROC_CHANGE_FEATURE, new
            {
                pv_ID = id,
                pv_FEATURE = feature,
                SESSION_USERNAME = modifiedBy
            });
        }
    }
}
