using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Phân loại sản phẩm
    /// </summary>
    public static class RstClassifyType
    {
        public const int CanHoThongThuong = 1;
        public const int CanHoStudio = 2;
        public const int CanHoOfficetel = 3;
        public const int CanHoShophouse = 4;
        public const int CanHoPenthouse = 5;
        public const int CanHoDuplex = 6;
        public const int CanHoSkyVilla = 7;
        public const int NhaONongThon = 8;
        public const int BietThuNhaO = 9;
        public const int LienKe = 10;
        public const int ChungCuThapTang = 11;
        public const int CanShophouse = 12;
        public const int BietThuNghiDuong = 13;
        public const int Villa = 14;
        public const int DuplexPool = 15;
        public const int BoutiqueHotel = 16;

        public static readonly List<int> All = new List<int>()
        {
            CanHoThongThuong, CanHoStudio, CanHoOfficetel, CanHoShophouse, CanHoPenthouse, CanHoDuplex,
            CanHoSkyVilla, NhaONongThon, BietThuNhaO, LienKe, ChungCuThapTang, CanShophouse, BietThuNghiDuong, Villa, DuplexPool, BoutiqueHotel
        };
        public static string GetClassifyTypeText(int classifyType)
        {
            var classifyTypeText = classifyType switch
            {
                CanHoThongThuong => RstProductItemClassifyTypeText.CanHoThongThuong,
                CanHoStudio => RstProductItemClassifyTypeText.CanHoStudio,
                CanHoOfficetel => RstProductItemClassifyTypeText.CanHoOfficetel,
                CanHoShophouse => RstProductItemClassifyTypeText.CanHoShophouse,
                CanHoPenthouse => RstProductItemClassifyTypeText.CanHoShophouse,
                CanHoDuplex => RstProductItemClassifyTypeText.CanHoShophouse,
                CanHoSkyVilla => RstProductItemClassifyTypeText.CanHoSkyVilla,
                NhaONongThon => RstProductItemClassifyTypeText.NhaONongThon,
                BietThuNhaO => RstProductItemClassifyTypeText.BietThuNhaO,
                LienKe => RstProductItemClassifyTypeText.LienKe,
                ChungCuThapTang => RstProductItemClassifyTypeText.ChungCuThapTang,
                CanShophouse => RstProductItemClassifyTypeText.CanShophouse,
                BietThuNghiDuong => RstProductItemClassifyTypeText.BietThuNghiDuong,
                Villa => RstProductItemClassifyTypeText.Villa,
                DuplexPool => RstProductItemClassifyTypeText.DuplexPool,
                BoutiqueHotel => RstProductItemClassifyTypeText.BoutiqueHotel,
                _ => "",
            };
            return classifyTypeText;
        }
    }

    public class RstProductItemClassifyTypeText
    {
        public const string CanHoThongThuong = "Căn hộ thông thường";
        public const string CanHoStudio = "Căn hộ Studio";
        public const string CanHoOfficetel = "Căn hộ Officetel";
        public const string CanHoShophouse = "Căn hộ ShopHouse";
        public const string CanHoPenthouse = "Căn hộ Penthouse";
        public const string CanHoDuplex = "Căn hộ Duplex";
        public const string CanHoSkyVilla = "Căn hộ Sky Villa";
        public const string NhaONongThon = "Nhà ở nông thôn";
        public const string BietThuNhaO = "Biệt thự nhà ở";
        public const string LienKe = "Liền kề";
        public const string ChungCuThapTang = "Chung cư thấp tầng";
        public const string CanShophouse = "Căn ShopHouse";
        public const string BietThuNghiDuong = "Biệt thự nghỉ dưỡng";
        public const string Villa = "Villa";
        public const string DuplexPool = "Duplex Pool";
        public const string BoutiqueHotel = "Boutique Hotel";
        public static readonly List<string> All = new List<string>()
        {
            CanHoThongThuong, CanHoStudio, CanHoOfficetel, CanHoShophouse, CanHoPenthouse, CanHoDuplex, 
            CanHoSkyVilla, NhaONongThon, BietThuNhaO, LienKe, ChungCuThapTang, CanShophouse, BietThuNghiDuong, Villa, DuplexPool, BoutiqueHotel
        };
    }
}
