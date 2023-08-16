using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondDashboard
{
    public class ViewBondDashboard
    {
        public DashboardTienVaoDto TienVao { get; set; }
        public DashboardTienRaDto TienRa { get; set; }
        public DashboardSoDuDto SoDu { get; set; }
        public List<DashboardDongTienTheoNgayDto> DongTienTheoNgay { get; set; }
        public List<DashboardDanhSachTheoKyHanSP> DanhSachTheoKyHanSP { get; set; }
        public List<DashboardDoanhSoVaSLBanTheoDLSC> DoanhSoVaSLBanTheoDLSC { get; set; }
        public List<DashboardDoanhSoVaSLBanTheoPhongBan> DoanhSoVaSLBanTheoPhongBan { get; set; }
    }
}
