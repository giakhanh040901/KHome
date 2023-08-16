using Dapper.Oracle;
using EPIC.DataAccess;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EPIC.InvestRepositories
{
    public class CalendarRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private static string PROC_CALENDAR_ADD_BY_YEAR = "PKG_INV_CALENDAR.PROC_CALENDAR_ADD_BY_YEAR";
        private static string PROC_CALENDAR_UPDATE = "PKG_INV_CALENDAR.PROC_CALENDAR_UPDATE";
        private static string PROC_CALENDAR_GET_ALL = "PKG_INV_CALENDAR.PROC_CALENDAR_GET_ALL";
        private static string PROC_CALENDAR_GET = "PKG_INV_CALENDAR.PROC_CALENDAR_GET";
        private const string PROC_CHECK_HOLIDAY = "PKG_INV_CALENDAR.PROC_CHECK_HOLIDAY";

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public CalendarRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public int AddByYear(Calendar entity)
        {
            _logger.LogInformation("Add By Year Calendar");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_CALENDAR_ADD_BY_YEAR, new
            {
                pv_WORKING_YEAR = entity.WorkingYear,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId
            });
            return rslt;
        }

        public List<Calendar> FindAllByWorkingYear(int workingYear, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<Calendar>(PROC_CALENDAR_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_WORKING_YEAR = workingYear
            });
            return result.ToList();
        }

        public Calendar FindByWorkingDate(DateTime workingDate, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Calendar>(PROC_CALENDAR_GET, new
            {
                pv_WORKING_DATE = workingDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int Update(Calendar entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CALENDAR_UPDATE, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_WORKING_DATE = entity.WorkingDate,
                pv_BUS_DATE = entity.BusDate,
                pv_IS_DAYOFF = entity.IsDayOff,
            });
            return result;
        }

        public bool CheckHoliday(DateTime ngayTraLai, int tradingProviderId, bool isCloseConnection = true)
        {
            string result = YesNo.NO;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_CHECK_DAY", ngayTraLai, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_CHECK_HOLIDAY, parameters, isCloseConnection);

            result = parameters.Get<string>("pv_RESULT");
            return result == YesNo.YES;
        }

    }
}
