using Dapper.Oracle;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesRepositories
{
    public class PartnerCalendarRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private const string schema = DbSchemas.EPIC_COMPANY_SHARES + ".";

        private static string PROC_PARTNER_CALENDAR_ADD_BY_YEAR = "PKG_CPS_CALENDAR_PARTNER.PROC_PARTNER_CALENDAR_ADD_BY_Y";
        private static string PROC_PARTNER_CALENDAR_UPDATE = "PKG_CPS_CALENDAR_PARTNER.PROC_PARTNER_CALENDAR_UPDATE";
        private static string PROC_PARTNER_CALENDAR_GET_ALL = "PKG_CPS_CALENDAR_PARTNER.PROC_PARTNER_CALENDAR_GET_ALL";
        private static string PROC_PARTNER_CALENDAR_GET = "PKG_CPS_CALENDAR_PARTNER.PROC_PARTNER_CALENDAR_GET";
        private const string PROC_CHECK_HOLIDAY = "PKG_CPS_CALENDAR_PARTNER.PROC_CHECK_HOLIDAY";

        public PartnerCalendarRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PartnerCalendar Add(PartnerCalendar entity)
        {
            throw new NotImplementedException();
        }

        public int AddByYear(PartnerCalendar entity)
        {
            _logger.LogInformation("Add By Year Calendar");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_PARTNER_CALENDAR_ADD_BY_YEAR, new
            {
                pv_WORKING_YEAR = entity.WorkingYear,
                pv_PARTNER_ID = entity.PartnerId
            });
            return rslt;
        }

        public List<PartnerCalendar> FindAllByWorkingYear(int workingYear, int partnerId)
        {
            var result = _oracleHelper.ExecuteProcedure<PartnerCalendar>(PROC_PARTNER_CALENDAR_GET_ALL, new
            {
                pv_PARTNER_ID = partnerId,
                pv_WORKING_YEAR = workingYear
            });
            return result.ToList();
        }

        public PartnerCalendar FindById(int id)
        {
            throw new NotImplementedException();
        }

        public PartnerCalendar FindByWorkingDate(DateTime workingDate, int partnerId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<PartnerCalendar>(PROC_PARTNER_CALENDAR_GET, new
            {
                pv_WORKING_DATE = workingDate,
                pv_PARTNER_ID = partnerId
            });
        }

        public int Update(PartnerCalendar entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PARTNER_CALENDAR_UPDATE, new
            {
                pv_PARTNER_ID = entity.PartnerId,
                pv_WORKING_DATE = entity.WorkingDate,
                pv_BUS_DATE = entity.BusDate,
                pv_IS_DAYOFF = entity.IsDayOff,
            });
            return result;
        }

        public bool CheckHoliday(DateTime ngayTraLai, int partnerId)
        {
            string result = YesNo.NO;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_CHECK_DAY", ngayTraLai, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_PARTNER_ID", partnerId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_CHECK_HOLIDAY, parameters);

            result = parameters.Get<string>("pv_RESULT");
            return result == YesNo.YES;
        }
    }
}
