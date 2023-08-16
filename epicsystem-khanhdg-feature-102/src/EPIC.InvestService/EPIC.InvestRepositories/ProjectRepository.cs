using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ExportExcel;
using EPIC.InvestEntities.Dto.Project;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class ProjectRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string PROC_PROJECT_ADD = "PKG_INV_PROJECT.PROC_PROJECT_ADD";
        private static string PROC_PROJECT_UPDATE = "PKG_INV_PROJECT.PROC_PROJECT_UPDATE";
        private static string PROC_PROJECT_DELETED = "PKG_INV_PROJECT.PROC_PROJECT_DELETED";
        private static string PROC_PROJECT_GET = "PKG_INV_PROJECT.PROC_PROJECT_GET";
        private static string PROC_PROJECT_GET_ALL = "PKG_INV_PROJECT.PROC_PROJECT_GET_ALL";
        private const string PROC_PROJECT_REQUEST = "PKG_INV_PROJECT.PROC_PROJECT_REQUEST";
        private const string PROC_PROJECT_CANCEL = "PKG_INV_PROJECT.PROC_PROJECT_CANCEL";
        private const string PROC_PROJECT_APPROVE = "PKG_INV_PROJECT.PROC_PROJECT_APPROVE";
        private const string PROC_PROJECT_CHECK = "PKG_INV_PROJECT.PROC_PROJECT_CHECK";
        private const string PROC_PROJECT_CLOSE = "PKG_INV_PROJECT.PROC_PROJECT_CLOSE";
        private const string PROC_PROJECT_TYPE_ADD = "PKG_INV_PROJECT_TYPE.PROC_PROJECT_TYPE_ADD";
        private const string PROC_PROJECT_TYPE_DELETE = "PKG_INV_PROJECT_TYPE.PROC_PROJECT_TYPE_DELETE";
        private const string PROC_PROJECT_TYPE_GET_BY_PROJECT_ID = "PKG_INV_PROJECT_TYPE.PROC_PROJECT_TYPE_GET_PROJ_ID";
        private const string PROC_PROJECT_GET_BY_ORDER = "PKG_INV_PROJECT.FIND_PROJECT_BY_ORDER";
        public ProjectRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public int ProjectRequest(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_REQUEST, new
            {
                pv_ID = id,
            });
            return result;
        }

        public int ProjectApprove(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_APPROVE, new
            {
                pv_ID = id
            });
            return result;
        }

        public int ProjectCheck(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_CHECK, new
            {
                pv_ID = id
            });
            return result;
        }

        public int ProjectCancel(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_CANCEL, new
            {
                pv_ID = id
            });
            return result;
        }

        public int ProjectClose(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_CLOSE, new
            {
                pv_ID = id
            });
            return result;
        }

        /// <summary>
        /// Lấy danh sách dự án
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="partnerId"></param>
        /// <param name="tradingProviderId">Đại lý get data với pageSize = -1 </param>
        /// <returns></returns>
        public PagingResult<ProjectDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, int? partnerId, int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ProjectDto>(PROC_PROJECT_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_PARTNER_ID = partnerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
            return result;
        }

        public Project FindById(int id, int? partnerId = null, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<Project>(PROC_PROJECT_GET, new
            {
                pv_ID = id,
                pv_PARTNER_ID = partnerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public ProjectDto FindByTradingProvider(int id, int? partnerId = null, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<ProjectDto>(PROC_PROJECT_GET, new
            {
                pv_ID = id,
                pv_PARTNER_ID = partnerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public Project Add(Project entity)
        {
            _logger.LogInformation("Add Project");
            return _oracleHelper.ExecuteProcedureToFirst<Project>(PROC_PROJECT_ADD, new
            {
                pv_PARTNER_ID = entity.PartnerId,
                pv_OWNER_ID = entity.OwnerId,
                pv_GENERAL_CONTRACTOR_ID = entity.GeneralContractorId,
                pv_INV_CODE = entity.InvCode,
                pv_INV_NAME = entity.InvName,
                pv_CONTENT = entity.Content,
                pv_START_DATE = entity.StartDate,
                pv_END_DATE = entity.EndDate,
                pv_IMAGE = entity.Image,
                pv_IS_PAYMENT_GUARANTEE = entity.IsPaymentGuarantee,
                pv_AREA = entity.Area,
                pv_LONGITUDE = entity.Longitude,
                pv_LATITUDE = entity.Latitude,
                pv_LOCATION_DESCRIPTION = entity.LocationDescription,
                pv_TOTAL_INVESTMENT = entity.TotalInvestment,
                pv_PROJECT_TYPE = entity.ProjectType,
                pv_PROJECT_PROGRESS = entity.ProjectProgress,
                pv_GUARANTEE_ORGANIZATION = entity.GuaranteeOrganization,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_INVESTMENT_DISPLAY = entity.TotalInvestmentDisplay,
                pv_HAS_TOTAL_INVESTMENT_SUB = entity.HasTotalInvestmentSub,
                SESSION_USERNAME = entity.CreatedBy
            }, false);
        }

        public void Update(Project entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_PARTNER_ID = entity.PartnerId,
                pv_INV_CODE = entity.InvCode,
                pv_INV_NAME = entity.InvName,
                pv_CONTENT = entity.Content,
                pv_START_DATE = entity.StartDate,
                pv_END_DATE = entity.EndDate,
                pv_IMAGE = entity.Image,
                pv_IS_PAYMENT_GUARANTEE = entity.IsPaymentGuarantee,
                pv_AREA = entity.Area,
                pv_LONGITUDE = entity.Longitude,
                pv_LATITUDE = entity.Latitude,
                pv_LOCATION_DESCRIPTION = entity.LocationDescription,
                pv_TOTAL_INVESTMENT = entity.TotalInvestment,
                pv_PROJECT_TYPE = entity.ProjectType,
                pv_PROJECT_PROGRESS = entity.ProjectProgress,
                pv_GUARANTEE_ORGANIZATION = entity.GuaranteeOrganization,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_INVESTMENT_DISPLAY = entity.TotalInvestmentDisplay,
                pv_HAS_TOTAL_INVESTMENT_SUB = entity.HasTotalInvestmentSub,
                SESSION_USERNAME = entity.ModifiedBy
            });
        }

        public int Delete(int id, int partnerId)
        {
            _logger.LogInformation($"Delete Project: {id}");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_DELETED, new
            {
                pv_ID = id,
                pv_PARTNER_ID = partnerId
            });
        }

        public void AddProjectType(ProjectType entity)
        {
            _logger.LogInformation("Add Project Type");
            _oracleHelper.ExecuteProcedureToFirst<ProjectType>(PROC_PROJECT_TYPE_ADD, new
            {
                pv_PROJECT_ID = entity.ProjectId,
                pv_TYPE = entity.Type,
            });
        }

        public int DeleteProjectType(int? id)
        {
            _logger.LogInformation($"Delete Project Type");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_TYPE_DELETE, new
            {
                pv_ID = id
            });
        }

        public List<ProjectType> FindByProjectId(int? projectId)
        {
            var result = _oracleHelper.ExecuteProcedure<ProjectType>(PROC_PROJECT_TYPE_GET_BY_PROJECT_ID, new
            {
                pv_PROJECT_ID = projectId,
            }).ToList();
            return result;
        }

        public Project FindByOrderId(int? orderId, int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<Project>(PROC_PROJECT_GET_BY_ORDER, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;

        }
    }
}
