namespace EPIC.RealEstateEntities.Dto.RstExportExcel
{
    public class RstProductProjectOverviewDto
    {
        //public RstProductItem productItem { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        public int? BuildingDensityType { get; set; }
        /// <summary>
        /// Phân loại sản phẩm 
        /// </summary>
        public int ClassifyType { get; set; }

        /// <summary>
        /// tên Mật độ
        /// </summary>
        public string BuildingDensityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BuildingDensityId { get; set; }
        /// <summary>
        /// trạng thái của sản phẩm
        /// </summary>
        public int Status { get; set; }

        public int CHThongThuong { get; set; }
        public int CHStudio { get; set; }
        public int CHShophouse { get; set; }
        public int CHPenthouse { get; set; }
        public int CHDuplex { get; set; }
        public int CHSkyVilla { get; set; }
        public int BietThu { get; set; }
        public int Villa { get; set; }
        public int LienKe { get; set; }
        public int NhaO { get; set; }
        public int CHOfficetel { get; set; }
        public int ChungCuThapTang { get; set; }
        public int CanShophouse { get; set; }

        public int SellCHThongThuong { get; set; }
        public int SellCHStudio { get; set; }
        public int SellCHShophouse { get; set; }
        public int SellCHPenthouse { get; set; }
        public int SellCHDuplex { get; set; }
        public int SellCHSkyVilla { get; set; }
        public int SellBietThu { get; set; }
        public int SellVilla { get; set; }
        public int SellLienKe { get; set; }
        public int SellNhaO { get; set; }
        public int SellCHOfficetel { get; set; }
        public int SellChungCuThapTang { get; set; }
        public int SellCanShophouse { get; set; }

        public decimal SoLuongChuaBan { get; set; }
        public decimal TyLeHangTon { get; set; }
    }
}