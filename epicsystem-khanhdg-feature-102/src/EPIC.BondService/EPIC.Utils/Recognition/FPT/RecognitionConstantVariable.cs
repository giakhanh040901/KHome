using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Recognition.FPT
{
    public static class OCRErrorCodes
    {
        public const int NO_ERROR = 0;
    }

    public static class FaceMatchCodes
    {
        public const string SUCCESS = "200";
        public const string NO_FACES_DETECTED = "407";
        public const string ALLOWED_EXTENSIONS_JPG_JPEG = "408";
        public const string MORE_OR_LESS_THAN_2_IMAGES = "409";
    }

    public static class OCRTypes
    {
        public const string CMND_FRONT = "old";
        public const string CMND_BACK = "old_back";
        public const string CCCD_FRONT = "new";
        public const string CCCD_BACK = "new_back";
        public const string CCCD_CHIP_FRONT = "chip_front";
        public const string CCCD_CHIP_BACK = "chip_back";
    }

    public static class OCRTypesNews
    {
        public const string CMND_09_FRONT = "cmnd_09_front";
        public const string CMND_12_FRONT = "cmnd_12_front";
        public const string CCCD_12_FRONT = "cccd_12_front";
        public const string CCCD_CHIP_FRONT = "cccd_chip_front";        
    }

    public static class OCRGenders
    {
        public const string MALE = "NAM";
        public const string MALE2 = "NAM/M";

        public const string FEMALE = "NỮ";
        public const string FEMALE2 = "NỮ/F";

        public const string NA = "N/A";
        public static string ConvertStandard(string input)
        {
            if (input == MALE || input == MALE2)
                return Genders.MALE;
            else if (input == FEMALE || input == FEMALE2)
                return Genders.FEMALE;
            return Genders.MALE;
        }
    }

    public static class OCRNationality
    {
        public const string VietNam = "Việt Nam";

        public static string ConvertStandard(string input)
        {
            if (input != "N/A")
                return input;
            return VietNam;
        }
    }

    public static class RecognitionUtils
    {
        public static string GetValueStandard(string input)
        {
            if (input != "N/A")
                return input;
            return null;
        }
    }
}
