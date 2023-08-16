using EPIC.DataAccess;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class ProvinceRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;

        private static string PROC_GET_LIST_PROVINCE = "EPIC.PKG_PROVINCE.PROC_GET_LIST_PROVINCE";
        private static string PROC_GET_LIST_DISTRICT = "EPIC.PKG_PROVINCE.PROC_GET_LIST_DISTRICT";
        private static string PROC_GET_LIST_WARD = "EPIC.PKG_PROVINCE.PROC_GET_LIST_WARD";

        public ProvinceRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        /// <summary>
        /// Danh sách tỉnh thành
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Province> GetListProvince(string keyword)
        {
            var result = _oracleHelper.ExecuteProcedure<Province>(PROC_GET_LIST_PROVINCE, new
            {
                KEYWORD = keyword
            })?.ToList();
            return result;
        }

        /// <summary>
        /// Danh sách quận huyện
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public List<District> GetListDistrict(string keyword, string provinceCode)
        {
            return _oracleHelper.ExecuteProcedure<District>(PROC_GET_LIST_DISTRICT, new
            {
                pv_PROVINCE_CODE = provinceCode,
                KEYWORD = keyword,
            })?.ToList();
        }

        /// <summary>
        /// Danh sách xã phường
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="districtCode"></param>
        /// <returns></returns>
        public List<Ward> GetListWard(string keyword, string districtCode)
        {
            return _oracleHelper.ExecuteProcedure<Ward>(PROC_GET_LIST_WARD, new
            {
                pv_DISTRICT_CODE = districtCode,
                KEYWORD = keyword,
            })?.ToList();
        }

    }
}
