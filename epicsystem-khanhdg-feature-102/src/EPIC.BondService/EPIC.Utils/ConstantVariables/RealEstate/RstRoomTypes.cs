using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;
using static EPIC.Utils.ConstantVariables.RealEstate.RstBuildingDensityTypes;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Kiểu phòng ngủ
    /// </summary>
    public static class RstRoomTypes
    {
        public const int OneBedroom = 1;
        public const int TwoBedroom = 2;
        public const int ThreeBedroom = 3;
        public const int FourBedroom = 4;
        public const int FiveBedroom = 5;
        public const int SixBedroom = 6;
        public const int SevenBedroom = 7;
        public const int EightBedroom = 8;
        public const int OneBedroomPlus1 = 9;
        public const int TwoBedroomPlus1 = 10;
        public const int ThreeBedroomPlus1 = 11;
        public const int FourBedroomPlus1 = 12;

        public static string RoomType(int? roomType)
        {
            return roomType switch
            {
                OneBedroom => RstProductItemRoomTypeText.OneBedroom,
                TwoBedroom => RstProductItemRoomTypeText.TwoBedroom,
                ThreeBedroom => RstProductItemRoomTypeText.ThreeBedroom,
                FourBedroom => RstProductItemRoomTypeText.FourBedroom,
                FiveBedroom => RstProductItemRoomTypeText.FiveBedroom,
                SixBedroom => RstProductItemRoomTypeText.SixBedroom,
                SevenBedroom => RstProductItemRoomTypeText.SevenBedroom,
                EightBedroom => RstProductItemRoomTypeText.EightBedroom,
                OneBedroomPlus1 => RstProductItemRoomTypeText.OneBedroomPlus1,
                TwoBedroomPlus1 => RstProductItemRoomTypeText.TwoBedroomPlus1,
                ThreeBedroomPlus1 => RstProductItemRoomTypeText.ThreeBedroomPlus1,
                FourBedroomPlus1 => RstProductItemRoomTypeText.FourBedroomPlus1,
                _ => string.Empty
            };
        }
    }

    public class RstProductItemRoomTypeText
    {
        public const string OneBedroom = "1PN";
        public const string TwoBedroom = "2PN";
        public const string ThreeBedroom = "3PN";
        public const string FourBedroom = "4PN";
        public const string FiveBedroom = "5PN";
        public const string SixBedroom = "6PN";
        public const string SevenBedroom = "7PN";
        public const string EightBedroom = "8PN";
        public const string OneBedroomPlus1 = "1PN+1";
        public const string TwoBedroomPlus1 = "2PN+1";
        public const string ThreeBedroomPlus1 = "3PN+1";
        public const string FourBedroomPlus1 = "4PN+1";
        public static readonly List<string> All = new List<string>()
        {
            OneBedroom, TwoBedroom, ThreeBedroom, FourBedroom, FiveBedroom, SixBedroom,
            SevenBedroom, EightBedroom, OneBedroomPlus1, TwoBedroomPlus1, ThreeBedroomPlus1, FourBedroomPlus1
        };
    }
}
