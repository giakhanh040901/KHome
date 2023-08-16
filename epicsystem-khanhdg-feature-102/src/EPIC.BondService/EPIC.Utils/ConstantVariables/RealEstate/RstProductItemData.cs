using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public static class RstProductItemData
    {
        public static int ClassifyType(string classifyType, string message)
        {
            return classifyType switch
            {
                "Căn hộ thông thường" => RstClassifyType.CanHoThongThuong,
                "Căn hộ Studio" => RstClassifyType.CanHoStudio,
                "Căn hộ Officetel" => RstClassifyType.CanHoOfficetel,
                "Căn hộ Shophouse" => RstClassifyType.CanHoShophouse,
                "Căn hộ Penthouse" => RstClassifyType.CanHoPenthouse,
                "Căn hộ Duplex" => RstClassifyType.CanHoDuplex,
                "Căn hộ Sky Villa" => RstClassifyType.CanHoSkyVilla,
                "Nhà ở nông thôn" => RstClassifyType.NhaONongThon,
                "Biệt thự nhà ở" => RstClassifyType.BietThuNhaO,
                "Liền kề" => RstClassifyType.LienKe,
                "Chung cư thấp tầng" => RstClassifyType.ChungCuThapTang,
                "Căn Shophouse" => RstClassifyType.CanShophouse,
                "Biệt thự nghỉ dưỡng" => RstClassifyType.BietThuNghiDuong,
                "Villa" => RstClassifyType.Villa,
                "Duplex Pool" => RstClassifyType.DuplexPool,
                "Boutique Hotel" => RstClassifyType.BoutiqueHotel,
                _ => throw new Exception(message + $" giá trị: {classifyType}"),
            };
        }

        public static int? DoorDirection(string doorDirection, string message)
        {
            return doorDirection switch
            {
                "Đông" => RstDirections.Dong,
                "Tây" => RstDirections.Tay,
                "Nam" => RstDirections.Nam,
                "Bắc" => RstDirections.Bac,
                "Đông Nam" => RstDirections.DongNam,
                "Đông Bắc" => RstDirections.DongBac,
                "Tây Nam" => RstDirections.TayNam,
                "Tây Bắc" => RstDirections.TayBac,
                "Đông Nam + Tây Nam" => RstDirections.DongNamTayNam,
                "Đông Nam + Đông Bắc" => RstDirections.DongNamDongBac,
                "Tây Nam + Tây Bắc" => RstDirections.TayNamTayBac,
                "Đông Nam + Tây Bắc" => RstDirections.DongNamTayBac,
                "Đông Bắc + Tây Bắc" => RstDirections.DongBacTayBac,
                "Đông Bắc + Tây Nam" => RstDirections.DongBacTayNam,
                _ => throw new Exception(message + $" giá trị: {doorDirection}"),
            };
        }

        public static int? RoomType(string roomType, string message)
        {
            return roomType switch
            {
                RstProductItemRoomTypeText.OneBedroom => RstRoomTypes.OneBedroom,
                RstProductItemRoomTypeText.TwoBedroom => RstRoomTypes.TwoBedroom,
                RstProductItemRoomTypeText.ThreeBedroom => RstRoomTypes.ThreeBedroom,
                RstProductItemRoomTypeText.FourBedroom => RstRoomTypes.FourBedroom,
                RstProductItemRoomTypeText.FiveBedroom => RstRoomTypes.FiveBedroom,
                RstProductItemRoomTypeText.SixBedroom => RstRoomTypes.SixBedroom,
                RstProductItemRoomTypeText.SevenBedroom => RstRoomTypes.SevenBedroom,
                RstProductItemRoomTypeText.EightBedroom => RstRoomTypes.EightBedroom,
                RstProductItemRoomTypeText.OneBedroomPlus1 => RstRoomTypes.OneBedroomPlus1,
                RstProductItemRoomTypeText.TwoBedroomPlus1 => RstRoomTypes.TwoBedroomPlus1,
                RstProductItemRoomTypeText.ThreeBedroomPlus1 => RstRoomTypes.ThreeBedroomPlus1,
                RstProductItemRoomTypeText.FourBedroomPlus1 => RstRoomTypes.FourBedroomPlus1,
                _ => throw new Exception(message + $" giá trị: {roomType}"),
            };
        }

        public static int RedBookType(string redBookType, string message)
        {
            return redBookType switch
            {
                "Có sổ đỏ" => RstRedBookTypes.HasRedBook,
                "Sổ đỏ 50 năm" => RstRedBookTypes.HasRedBook50Year,
                "Sổ lâu dài" => RstRedBookTypes.HasRedBookLongTerm,
                "Chưa có sổ đỏ" => RstRedBookTypes.NoRedBook,
                _ => throw new Exception(message + $" giá trị: {redBookType}"),
            };
        }

        public static int ProductLocation(string productLocation, string message)
        {
            return productLocation switch
            {
                "Căn giữa" => RstProductLocations.CanGiua,
                "Căn góc" => RstProductLocations.CanGoc,
                "Cổng chính" => RstProductLocations.CongChinh,
                "Toà riêng" => RstProductLocations.ToaRieng,
                "Căn thông tầng" => RstProductLocations.CanThongTang,
                _ => throw new Exception(message + $" giá trị: {productLocation}"),
            };
        }

        public static int HandingType(string handingType, string message)
        {
            return handingType switch
            {
                "Bàn giao thô" => RstHandingTypes.BanGiaoTho,
                "Nội thất cơ bản" => RstHandingTypes.NoiThatCoBan,
                "Nội thất liền tường" => RstHandingTypes.NoiThatLienTuong,
                "Nội thất cao cấp" => RstHandingTypes.NoiThatCaoCap,
                "Full nội thất" => RstHandingTypes.FullNoiThat,
                _ => throw new Exception(message + $" giá trị: {handingType}"),
            };
        }

        public static string YesNoCheck(string yesNo, string message)
        {
            return yesNo switch
            {
                "Y" => YesNo.YES,
                "N" => YesNo.NO,
                _ => throw new Exception(message + $" giá trị: {yesNo}"),
            };
        }
    }
}
