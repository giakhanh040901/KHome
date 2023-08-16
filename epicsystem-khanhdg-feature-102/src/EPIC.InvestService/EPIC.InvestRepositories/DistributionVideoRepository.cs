using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionVideo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class DistributionVideoRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_DISTRIBUTION_VIDEO_ADD = "PKG_INV_DISTRIBUTION_VIDEO.PROC_DISTRIBUTION_VIDEO_ADD";
        private const string PROC_DISTRIBUTION_VIDEO_DELETE = "PKG_INV_DISTRIBUTION_VIDEO.PROC_DISTRIBUTION_VIDEO_DELETE";
        private const string PROC_DISTRIBUTION_VIDEO_GET = "PKG_INV_DISTRIBUTION_VIDEO.PROC_DISTRIBUTION_VIDEO_GET";
        private const string PROC_DISTRIBUTION_VIDEO_NEW = "PKG_INV_DISTRIBUTION_VIDEO.PROC_DISTRIBUTION_VIDEO_NEW";
        private const string PROC_DISTRI_VIDEO_GET_APP = "PKG_INV_DISTRIBUTION_VIDEO.PROC_DISTRI_VIDEO_GET_APP";
        private const string PROC_DISTRIBUTION_VIDEO_UPDATE = "PKG_INV_DISTRIBUTION_VIDEO.PROC_DISTRIBUTION_VIDEO_UPDATE";
        private const string PROC_DISTRI_VIDEO_GET_ALL = "PKG_INV_DISTRIBUTION_VIDEO.PROC_DISTRI_VIDEO_GET_ALL";
        private const string PROC_CHANGE_STATUS = "PKG_INV_DISTRIBUTION_VIDEO.PROC_CHANGE_STATUS";
        private const string PROC_CHANGE_FEATURE = "PKG_INV_DISTRIBUTION_VIDEO.PROC_CHANGE_FEATURE";

        public DistributionVideoRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public DistributionVideo Add(DistributionVideo entity, int TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<DistributionVideo>(
                PROC_DISTRIBUTION_VIDEO_ADD, new
                {
                    pv_DISTRIBUTION_ID = entity.DistributionId,
                    pv_TRADING_PROVIDER_ID = TradingProviderId,
                    pv_URL_VIDEO = entity.UrlVideo,
                    pv_TITLE = entity.Title,
                    SESSION_USERNAME = entity.CreatedBy
                 });
        }

        public PagingResult<ViewDistributionVideoDto> FindAll(int pageSize, int pageNumber, string status, int? DistributionId, int TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedurePaging<ViewDistributionVideoDto>(PROC_DISTRI_VIDEO_GET_ALL, new
            {
                pv_DISTRIBUTION_ID = DistributionId,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                pv_STATUS = status
            });
        }

        public ViewDistributionVideoDto FindById(int id, int TradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ViewDistributionVideoDto>(PROC_DISTRIBUTION_VIDEO_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
            });
            return result;
        }

        public ViewDistributionVideoDto FindNewVideo(int id, int TradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ViewDistributionVideoDto>(PROC_DISTRIBUTION_VIDEO_NEW, new
            {
                pv_DISTRIBUTION_ID = id,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
            });
            return result;
        }

        public ViewDistributionVideoDto FindDistributionId(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ViewDistributionVideoDto>(PROC_DISTRI_VIDEO_GET_APP, new
            {
                pv_DISTRIBUTION_ID = id,
            });
            return result;
        }

        public int Update(DistributionVideo entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_DISTRIBUTION_VIDEO_UPDATE, new
                {
                     pv_ID = entity.Id,
                     pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                     pv_URL_VIDEO = entity.UrlVideo,
                     pv_TITLE = entity.Title,
                     SESSION_USERNAME = entity.ModifiedBy
                });
        }

        public int Delete(int id, int TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRIBUTION_VIDEO_DELETE, new
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
