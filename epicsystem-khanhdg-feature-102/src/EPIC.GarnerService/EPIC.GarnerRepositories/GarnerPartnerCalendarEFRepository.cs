using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerPartnerCalendarEFRepository : BaseEFRepository<GarnerPartnerCalendar>
    {
        private const string PROC_GAN_CALENDAR_GET_ALL = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR_PARTNER.PROC_GAN_CALENDAR_GET_ALL";
        private const string PROC_GAN_CALENDAR_ADD_BY_YEAR = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR_PARTNER.PROC_GAN_CALENDAR_ADD_BY_YEAR";
        private const string PROC_GAN_CALENDAR_UPDATE = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR_PARTNER.PROC_GAN_CALENDAR_UPDATE";
        private const string PROC_GAN_CALENDAR_GET = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR_PARTNER.PROC_GAN_CALENDAR_GET";
        private const string PROC_CHECK_HOLIDAY = DbSchemas.EPIC_GARNER + ".PKG_CALENDAR_PARTNER.PROC_CHECK_HOLIDAY";

        public GarnerPartnerCalendarEFRepository(EpicSchemaDbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, seqName)
        {
        }

        /// <summary>
        /// Add vào calendar theo năm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddByYear(GarnerPartnerCalendar entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {(entity != null ? JsonSerializer.Serialize(entity) : "")}");

            var conveted = ObjectToParamAndQuery(PROC_GAN_CALENDAR_ADD_BY_YEAR, new
            {
                pv_WORKING_YEAR = entity.WorkingYear,
                pv_PARTNER_ID = entity.PartnerId
            });

            var result = _epicSchemaDbContext.Database.ExecuteSqlRaw(conveted.SqlQuery, conveted.Parameters);

            return result;
        }

        /// <summary>
        /// Tìm ngày theo năm
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="workingYear"></param>
        /// <returns></returns>
        public List<GarnerPartnerCalendar> FindAllByWorkingYear(int? partnerId, int? workingYear)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: partnerId = {partnerId}; workingYear = {workingYear}");

            var conveted = ObjectToParamAndQueryList(PROC_GAN_CALENDAR_GET_ALL, new
            {
                pv_PARTNER_ID = partnerId,
                pv_WORKING_YEAR = workingYear,
            });

            try
            {
                var result = _epicSchemaDbContext.GarnerPartnerCalendars.FromSqlRaw(conveted.SqlQuery, conveted.Parameters)?.ToList();
                return result;
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
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public GarnerPartnerCalendar FindByWorkingDate(DateTime workingDate, int partnerId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: partnerId = {partnerId}; workingDate = {workingDate}");

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
                pv_PARTNER_ID = partnerId,
                pv_WORKING_DATE = workingDate
            });

            try
            {
                var result = _epicSchemaDbContext.GarnerPartnerCalendars.FromSqlRaw(conveted.SqlQuery, conveted.Parameters).ToList();
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
        public int Update(GarnerPartnerCalendar entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {(entity != null ? JsonSerializer.Serialize(entity) : "")}");

            var conveted = ObjectToParamAndQuery(PROC_GAN_CALENDAR_UPDATE, new
            {
                pv_PARTNER_ID = entity.PartnerId,
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
        /// <param name="ngayTraLai"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public bool CheckHoliday(DateTime ngayTraLai, int partnerId)
        {
            string result = YesNo.NO;

            //OracleDynamicParameters parameters = new();
            //parameters.Add("pv_CHECK_DAY", ngayTraLai, OracleMappingType.Date, ParameterDirection.Input);
            //parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            //parameters.Add("pv_RESULT", result, OracleMappingType.Varchar2, ParameterDirection.Output);
            //ExecuteProcedureDynamicParams(PROC_CHECK_HOLIDAY, parameters);

            //result = parameters.Get<string>("pv_RESULT");
            return result == YesNo.YES;
        }

        /// <summary>
        /// Ngày làm việc tiếp theo
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public DateTime NextWorkDay(DateTime ngayDangXet, int partnerId)
        {
            DateTime ngayLamViecTiepTheo = ngayDangXet.Date;
            while (true)
            {
                if (CheckHoliday(ngayLamViecTiepTheo, partnerId))
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
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public DateTime WorkDay(DateTime ngayDangXet, int soNgayLamViec, int partnerId)
        {
            DateTime ngayLamViecTruoc = ngayDangXet.Date;
            int countEnd = Math.Abs(soNgayLamViec);
            int count = 0;
            while (count == countEnd)
            {
                if (!CheckHoliday(ngayLamViecTruoc, partnerId)) //không phải ngày nghỉ + 1
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
