using Dapper.Oracle;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondDashboard;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ExportReport;
using EPIC.Entities.Dto.Order;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondDashboardRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_BOND_TIEN_VAO = "PKG_BOND_DASHBOARD.PROC_BOND_TIEN_VAO";
        private const string PROC_BOND_TIEN_RA = "PKG_BOND_DASHBOARD.PROC_BOND_TIEN_RA";
        private const string PROC_BOND_DONG_TIEN_THEO_NGAY = "PKG_BOND_DASHBOARD.PROC_BOND_DONG_TIEN_THEO_NGAY";
        private const string PROC_BOND_GET_BY_BOND_DETAIL = "PKG_BOND_DASHBOARD.PROC_BOND_GET_BY_BOND_DETAIL";
        private const string PROC_BOND_TOTAL_SALER_PARTNER = "PKG_BOND_DASHBOARD.PROC_BOND_TOTAL_SALER_PARTNER";
        private const string PROC_BOND_TOTAL_SALER_TRADING = "PKG_BOND_DASHBOARD.PROC_BOND_TOTAL_SALER_TRADING";

        public BondDashboardRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        /// <summary>
        /// Lấy tiền vào trong dashboard bond
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public DashboardTienVaoDto GetTienVao(GetBondDashboardDto dto, int? tradingProviderId)
        {
            decimal? tienVao = 0;
            decimal? luyKe = 0;

            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_FIRST_DATE", dto.FirstDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_END_DATE", dto.EndDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_SECONDARY_ID", dto.SecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_TOTAL", tienVao, OracleMappingType.Decimal, ParameterDirection.Output);
            parameters.Add("pv_LUY_KE", luyKe, OracleMappingType.Decimal, ParameterDirection.Output);

            _oracleHelper.ExecuteProcedureDynamicParams(PROC_BOND_TIEN_VAO, parameters);

            tienVao = parameters.Get<decimal>("pv_TOTAL");
            luyKe = parameters.Get<decimal>("pv_LUY_KE");

            return new DashboardTienVaoDto
            {
                LuyKe = luyKe,
                TienVao = tienVao,
            };
        }

        /// <summary>
        /// Lấy tiền ra trong dashboard bond
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public DashboardTienRaDto GetTienRa(GetBondDashboardDto dto, int? tradingProviderId)
        {
            decimal? tienRa = 0;
            decimal? luyKe = 0;

            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_FIRST_DATE", dto.FirstDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_END_DATE", dto.EndDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("pv_SECONDARY_ID", dto.SecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_TOTAL", tienRa, OracleMappingType.Decimal, ParameterDirection.Output);
            parameters.Add("pv_LUY_KE", luyKe, OracleMappingType.Decimal, ParameterDirection.Output);

            _oracleHelper.ExecuteProcedureDynamicParams(PROC_BOND_TIEN_RA, parameters);

            tienRa = parameters.Get<decimal>("pv_TOTAL");
            luyKe = parameters.Get<decimal>("pv_LUY_KE");

            return new DashboardTienRaDto
            {
                LuyKe = luyKe,
                TienRa = tienRa,
            };
        }

        /// <summary>
        /// Get dòng tiền theo ngày
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public IEnumerable<TempDongTienTheoNgay> GetDongTienTheoNgay(GetBondDashboardDto dto, int? tradingProviderId)
        {
            var data = _oracleHelper.ExecuteProcedure<TempDongTienTheoNgay>(PROC_BOND_DONG_TIEN_THEO_NGAY, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_FIRST_DATE = dto.FirstDate,
                pv_END_DATE = dto.EndDate,
                pv_SECONDARY_ID = dto.SecondaryId,
            });

            return data;
        }

        /// <summary>
        /// Lấy danh sách theo kỳ hạn sản phẩm
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public IEnumerable<DashboardDanhSachTheoKyHanSP> GetDanhSachTheoKyHanSP(GetBondDashboardDto dto, int? tradingProviderId)
        {
            var data = _oracleHelper.ExecuteProcedure<DashboardDanhSachTheoKyHanSP>(PROC_BOND_GET_BY_BOND_DETAIL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_FIRST_DATE = dto.FirstDate,
                pv_END_DATE = dto.EndDate,
                pv_SECONDARY_ID = dto.SecondaryId,
            });

            return data;
        }

        /// <summary>
        /// Lấy doanh số và số lượng bán theo đại lý sơ cấp
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public IEnumerable<DashboardDoanhSoVaSLBanTheoDLSC> GetDoanhSoVaSLBanTheoDLSC(GetBondDashboardDto dto, int? partnerId)
        {
            var data = _oracleHelper.ExecuteProcedure<DashboardDoanhSoVaSLBanTheoDLSC>(PROC_BOND_TOTAL_SALER_PARTNER, new
            {
                pv_PARTNER_ID = partnerId,
                pv_FIRST_DATE = dto.FirstDate,
                pv_END_DATE = dto.EndDate,
                pv_SECONDARY_ID = dto.SecondaryId,
            });

            return data;
        }

        /// <summary>
        /// Lấy doanh số và số lượng bán theo phòng ban
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public IEnumerable<DashboardDoanhSoVaSLBanTheoPhongBan> GetDoanhSoVaSoLuongBanTheoPhongBan(GetBondDashboardDto dto, int? tradingProviderId)
        {
            var data = _oracleHelper.ExecuteProcedure<DashboardDoanhSoVaSLBanTheoPhongBan>(PROC_BOND_TOTAL_SALER_TRADING, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_FIRST_DATE = dto.FirstDate,
                pv_END_DATE = dto.EndDate,
                pv_SECONDARY_ID = dto.SecondaryId,
            });

            return data;
        }

    }
}
