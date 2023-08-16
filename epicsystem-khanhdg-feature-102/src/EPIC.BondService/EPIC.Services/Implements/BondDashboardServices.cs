using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondDashboard;
using EPIC.BondRepositories;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondDashboardServices : IBondDashboardService
    {
        private readonly ILogger<BondDashboardServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly BondDashboardRepository _dashboardRepository;

        public BondDashboardServices(ILogger<BondDashboardServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _dashboardRepository = new BondDashboardRepository(_connectionString, _logger);
        }

        /// <summary>
        /// Lấy thông số bond dashboard
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ViewBondDashboard GetBondDashboard(GetBondDashboardDto dto)
        {
            int? tradingProviderId = 0;
            int? partnerId = 0;

            try
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("Không tìm thấy TradingProviderId: ", ex.Message);
            }

            try
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("Không tìm thấy PartnerId: ", ex.Message);
            }

            // Lấy tiền vào
            var tienVao = _dashboardRepository.GetTienVao(dto, tradingProviderId);

            // Lấy tiền ra
            var tienRa = _dashboardRepository.GetTienRa(dto, tradingProviderId);

            var soDu = new DashboardSoDuDto
            {
                SoDu = 0,
                TyLe = 0,
            };

            // Tính số dư
            if (tienVao?.LuyKe > 0 && tienRa?.LuyKe >= 0)
            {
                soDu.SoDu = tienVao.LuyKe - tienRa.LuyKe;
                soDu.TyLe = (soDu.SoDu ?? 0) / (tienVao.TienVao ?? 0);
            }

            var result = new ViewBondDashboard
            {
                TienVao = tienVao,
                TienRa = tienRa,
                SoDu = soDu,
                DongTienTheoNgay = new List<DashboardDongTienTheoNgayDto>(),
            };

            // Lấy dòng tiền theo ngày
            var dongTienTheoNgay = _dashboardRepository.GetDongTienTheoNgay(dto, tradingProviderId);

            foreach (var dongTien in dongTienTheoNgay)
            {
                result.DongTienTheoNgay.Add(new DashboardDongTienTheoNgayDto
                {
                    Date = dongTien.Ngay,
                    TotalValue = dongTien.TienVao,
                    TotalValueOut = dongTien.TienRa
                });
            }

            // Lấy danh sách theo kỳ hạn sản phẩm
            result.DanhSachTheoKyHanSP = _dashboardRepository.GetDanhSachTheoKyHanSP(dto, tradingProviderId)?.ToList();

            if (partnerId > 0)
            {
                // Lấy doanh số và số lượng bán theo đại lý sơ cấp
                result.DoanhSoVaSLBanTheoDLSC = _dashboardRepository.GetDoanhSoVaSLBanTheoDLSC(dto, partnerId)?.ToList();
            } 
            else
            {
                // Lấy doanh số và số lượng bán theo phòng ban
                result.DoanhSoVaSLBanTheoPhongBan = _dashboardRepository.GetDoanhSoVaSoLuongBanTheoPhongBan(dto, tradingProviderId)?.ToList();
            }

            return result;
        }
    }
}
