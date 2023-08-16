using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.GeneralContractor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class GeneralContractorRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private const string PROC_CONTRACTOR_ADD = "PKG_INV_GENERAL_CONTRACTOR.PROC_CONTRACTOR_ADD";
        private const string PROC_CONTRACTOR_DELETE = "PKG_INV_GENERAL_CONTRACTOR.PROC_CONTRACTOR_DELETE";
        private const string PROC_CONTRACTOR_GET = "PKG_INV_GENERAL_CONTRACTOR.PROC_CONTRACTOR_GET";
        private const string PROC_CONTRACTOR_UPDATE = "PKG_INV_GENERAL_CONTRACTOR.PROC_CONTRACTOR_UPDATE";
        private const string PROC_CONTRACTOR_GET_ALL = "PKG_INV_GENERAL_CONTRACTOR.PROC_CONTRACTOR_GET_ALL";

        public GeneralContractorRepository(string connectionString, ILogger logger)
        {                                                                                                               
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public GeneralContractor Add(GeneralContractor entity)
        {
            _logger.LogInformation("Add InvGeneralContractor");
            return _oracleHelper.ExecuteProcedureToFirst<GeneralContractor>(PROC_CONTRACTOR_ADD, new
            {
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_PARTNER_ID = entity.PartnerId,
                SESSION_USERNAME  = entity.CreatedBy,
            });
        }
        
        public int Delete(int id, int partnerId)
        {
            _logger.LogInformation("Delete InvGeneralContractor");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACTOR_DELETE, new
            {
                pv_ID = id,
                pv_PARTNER_ID = partnerId
            });
            return rslt;
        }

        public PagingResult<ViewGeneralContractorDto> FindAll(int? partnerId, int pageSize, int pageNumber, string keyword, string status)
        {
            var generalContractor = _oracleHelper.ExecuteProcedurePaging<ViewGeneralContractorDto>(PROC_CONTRACTOR_GET_ALL, new
            {
                pv_PARTNER_ID = partnerId,
                pv_PAGE_SIZE = pageSize,
                pv_PAGE_NUMBER = pageNumber,
                pv_KEY_WORD = keyword,
                pv_STATUS = status
            });
            return generalContractor;
        }

        public GeneralContractor FindById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<GeneralContractor>(PROC_CONTRACTOR_GET, new
            {
                pv_ID = id,
            });
            return result;
        }
    }
}
