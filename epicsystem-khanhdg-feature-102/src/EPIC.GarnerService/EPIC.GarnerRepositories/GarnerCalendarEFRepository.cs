using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper.Oracle;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using EPIC.Entities.DataEntities;
using Oracle.ManagedDataAccess.Types;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.CoreEntities.Dto.Sale;

namespace EPIC.GarnerRepositories
{
    public class GarnerCalendarEFRepository : BaseEFRepository<GarnerCalendar>
    {
        private const string PROC_GAN_CALENDAR_GET_ALL = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR.PROC_GAN_CALENDAR_GET_ALL";
        private const string PROC_GAN_CALENDAR_ADD_BY_YEAR = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR.PROC_GAN_CALENDAR_ADD_BY_YEAR";
        private const string PROC_GAN_CALENDAR_UPDATE = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR.PROC_GAN_CALENDAR_UPDATE";
        private const string PROC_GAN_CALENDAR_GET = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR.PROC_GAN_CALENDAR_GET";
        private const string PROC_CHECK_HOLIDAY = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR.PROC_CHECK_HOLIDAY";

        public GarnerCalendarEFRepository(EpicSchemaDbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, seqName)
        {
        }

        /// <summary>
        /// Add vào calendar theo năm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddByYear(GarnerCalendar entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {(entity != null ? JsonSerializer.Serialize(entity) : "")}");

            var conveted = ObjectToParamAndQuery(PROC_GAN_CALENDAR_ADD_BY_YEAR, new
            {
                pv_WORKING_YEAR = entity.WorkingYear,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId
            });

            var result = _epicSchemaDbContext.Database.ExecuteSqlRaw(conveted.SqlQuery, conveted.Parameters);

            return result;
        }

        /// <summary>
        /// Tìm ngày theo năm
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="workingYear"></param>
        /// <returns></returns>
        public List<GarnerCalendar> FindAllByWorkingYear(int? tradingProviderId, int? workingYear)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: tradingProviderId = {tradingProviderId}; workingYear = {workingYear}");

            var conveted = ObjectToParamAndQueryList(PROC_GAN_CALENDAR_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_WORKING_YEAR = workingYear,
            });
            try
            {
                return _epicSchemaDbContext.GarnerCalendars.FromSqlRaw(conveted.SqlQuery, conveted.Parameters)?.ToList();
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex);
            }
        }

        /// <summary>
        /// Tìm ngày theo date
        /// </summary>
        /// <param name="workingDate"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerCalendar FindByWorkingDate(DateTime workingDate, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: tradingProviderId = {tradingProviderId}; workingDate = {workingDate}");

            //var conveted = ObjectToParamAndQueryPaging("EPIC_GARNER.PKG_CALENDAR.TEST_2", new
            //{
            //    pv_TRADING_PROVIDER_ID = 181,
            //    pv_WORKING_DATE = DateTime.Now,
            //});

            //var ssss = _garnerDbContext.Set<Test>().FromSqlRaw(conveted.SqlQuery, conveted.Parameters).ToList();
            //// _garnerDbContext.Database.ExecuteSqlRaw(conveted.SqlQuery, conveted.Parameters);

            //var totalItems = conveted.Parameters.FirstOrDefault(x => x.ParameterName == "total_items");
            //var vl = totalItems.Value.ToString();

            //return null;


            //var conveted = ObjectToParamAndQueryList("EPIC_GARNER.PKG_CALENDAR.TEST", new
            //{
            //    pv_TRADING_PROVIDER_ID = 181,
            //    pv_WORKING_DATE = DateTime.Now,
            //});

            //var ssss = _garnerDbContext.Test.FromSqlRaw(conveted.SqlQuery, conveted.Parameters).ToList();
            //return null;

            var conveted = ObjectToParamAndQueryList(PROC_GAN_CALENDAR_GET, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_WORKING_DATE = workingDate
            });

            try
            {
                var result = _epicSchemaDbContext.GarnerCalendars.FromSqlRaw(conveted.SqlQuery, conveted.Parameters).ToList();
                return result?.FirstOrDefault();
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex);
            }
        }

        /// <summary>
        /// Cập nhật ngày trong calendar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(GarnerCalendar entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {(entity != null ? JsonSerializer.Serialize(entity) : "")}");

            var conveted = ObjectToParamAndQuery(PROC_GAN_CALENDAR_UPDATE, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_WORKING_DATE = entity.WorkingDate,
                pv_BUS_DATE = entity.BusDate,
                pv_IS_DAYOFF = entity.IsDayOff,
            });
            var result = _epicSchemaDbContext.Database.ExecuteSqlRaw(conveted.SqlQuery, conveted.Parameters);

            return result;
        }

        /// <summary>
        /// Check ngày nghỉ lễ
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public bool CheckHoliday(DateTime ngayDangXet, int tradingProviderId)
        {
            var calendar = _dbSet.FirstOrDefault(c => c.WorkingDate == ngayDangXet && c.TradingProviderId == tradingProviderId)
                .ThrowIfNull<GarnerCalendar>(_epicSchemaDbContext, ErrorCode.GarnerCalenderNotFound);

            return calendar.IsDayOff == YesNo.YES;
        }

        /// <summary>
        /// Ngày làm việc tiếp theo
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public DateTime NextWorkDay(DateTime ngayDangXet, int tradingProviderId)
        {
            DateTime ngayLamViecTiepTheo = ngayDangXet.Date;
            while (true)
            {
                if (CheckHoliday(ngayLamViecTiepTheo, tradingProviderId))
                {
                    ngayLamViecTiepTheo = ngayLamViecTiepTheo.AddDays(1);
                }
                else
                {
                    return ngayLamViecTiepTheo;
                }
            }
        }

        /// <summary>
        /// Lùi hoặc tiến đến ngày làm việc
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="soNgayLamViec"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public DateTime WorkDay(DateTime ngayDangXet, int soNgayLamViec, int tradingProviderId)
        {
            DateTime ngayLamViecTruoc = ngayDangXet.Date;
            int countEnd = Math.Abs(soNgayLamViec);
            int count = 0;
            while (count == countEnd)
            {
                if (!CheckHoliday(ngayLamViecTruoc, tradingProviderId)) //không phải ngày nghỉ + 1
                {
                    count++;
                }
                if (soNgayLamViec < 0)
                {
                    ngayLamViecTruoc = ngayLamViecTruoc.AddDays(-1);
                }
                else
                {
                    ngayLamViecTruoc = ngayLamViecTruoc.AddDays(1);
                }
            }
            return ngayLamViecTruoc;
        }

    }
}
