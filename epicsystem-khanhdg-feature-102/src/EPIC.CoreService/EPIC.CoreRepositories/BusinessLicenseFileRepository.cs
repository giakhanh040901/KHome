using EPIC.DataAccess;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessLicenseFile;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class BusinessLicenseFileRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_BUSINESS_LICENSE_FILE_ADD = "EPIC.PKG_CORE_BUSINESS_LICENSE_FILE.PROC_BUSINESS_LICENSE_FILE_ADD";
        private const string PROC_BUSINESS_LICENSE_FILE_TEMP_ADD = "EPIC.PKG_CORE_BUSINESS_LICENSE_FILE.PROC_BUSINESS_LICENSE_FILE_TEMP_ADD";
        private const string PROC_BUSINESS_LICENSE_FILE_GET = "EPIC.PKG_CORE_BUSINESS_LICENSE_FILE.PROC_BUSINESS_LICENSE_FILE_GET";
        private const string PROC_BUSINESS_LICENSE_FILE_DEL = "EPIC.PKG_CORE_BUSINESS_LICENSE_FILE.PROC_BUSINESS_LICENSE_FILE_DEL";
        private const string PROC_BUSINESS_LICENSE_UPDATE = "EPIC.PKG_CORE_BUSINESS_LICENSE_FILE.PROC_BUSINESS_LICENSE_UPDATE";
        private const string PROC_BUSINESS_LICENSE_UPDATE_TEMP = "EPIC.PKG_CORE_BUSINESS_LICENSE_FILE.PROC_BUSINESS_LICENSE_UPDATE_TEMP";
        private const string PROC_BUSINESS_LICENSE_GET_ALL = "EPIC.PKG_CORE_BUSINESS_LICENSE_FILE.PROC_BUSINESS_LICENSE_GET_ALL";

        public BusinessLicenseFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public BusinessLicenseFile FindById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessLicenseFile>(PROC_BUSINESS_LICENSE_FILE_GET, new
            {
                pv_ID = id,
            });
            return result;
        }

        public BusinessLicenseFile Add(CreateBusinessLicenseFileDto entity, string createdBy)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessLicenseFile>(PROC_BUSINESS_LICENSE_FILE_ADD, new
            {
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                SESSION_USERNAME = createdBy,
            });
            return result;
        }

        public BusinessLicenseFile AddTemp(CreateBusinessLicenseFileTempDto entity, string createdBy)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessLicenseFile>(PROC_BUSINESS_LICENSE_FILE_TEMP_ADD, new
            {
                pv_BUSINESS_CUSTOMER_TEMP_ID = entity.BusinessCustomerTempId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                SESSION_USERNAME = createdBy,
            });
            return result;
        }

        public int Update(UpdateBusinessLicenseFileDto entity, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_LICENSE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                pv_MODIFIED_BY = modifiedBy,
            });
            return result;
        }

        public int UpdateTemp(UpdateBusinessLicenseFileTempDto entity, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_LICENSE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_BUSINESS_CUSTOMER_TEMP_ID = entity.BusinessCustomerTempId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                pv_MODIFIED_BY = modifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_LICENSE_FILE_DEL, new
            {
                pv_ID = id
            });
        }

        /// <summary>
        /// Lấy thông tin ĐKKD, theo doanh nghiệp tạm mới tạo, hoặc doanh nghiệp đang hoạt động 
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <param name="businessCustomerTempId"></param>
        /// <returns></returns>
        public List<BusinessLicenseFileDto> FindAll(int? businessCustomerId, int? businessCustomerTempId)
        {
            var result = _oracleHelper.ExecuteProcedure<BusinessLicenseFileDto>(PROC_BUSINESS_LICENSE_GET_ALL, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId,
                pv_BUSINESS_CUSTOMER_TEMP_ID = businessCustomerTempId,
            }).ToList();
            return result;
        }
    }
}
