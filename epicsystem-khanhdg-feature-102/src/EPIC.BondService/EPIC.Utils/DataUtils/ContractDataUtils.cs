using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.EnumType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace EPIC.Utils.DataUtils
{
    public static class ContractDataUtils
    {
        public static Dictionary<string, string> GetParams(string uri)
        {
            var matches = Regex.Matches(uri, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            return matches.Cast<Match>().ToDictionary(
                m => Uri.UnescapeDataString(m.Groups[2].Value),
                m => Uri.UnescapeDataString(m.Groups[3].Value)
            );
        }

        public static string GetNameGender(string gender)
        {
            string result = gender switch
            {
                Genders.MALE => "Nam",
                Genders.FEMALE => "Nữ",
                _ => "Nam",
            };
            return result;
        }

        public static string GetNameDateType(string typeDate)
        {
            string result = typeDate switch
            {
                PeriodUnit.DAY => "ngày",
                PeriodUnit.MONTH => "tháng",
                PeriodUnit.YEAR => "năm",
                _ => "ngày",
            };
            return result;
        }

        public static string GetIdType(string idType)
        {
            string result = idType switch
            {
                IDTypes.CMND => "CMND",
                IDTypes.CCCD => "CCCD",
                IDTypes.PASSPORT => "Hộ chiếu",
                _ => "CCCD",
            };
            return result;
        }

        public static string GetFullPathFile(string folder, string fileName, string filePath)
        {
            var fullPath = Path.Combine(filePath, folder, fileName);
            return fullPath;
        }

        /// <summary>
        /// Tạo url path cho file vật lý đã lưu
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetEndPoint(string endpoint, string folder, string fileName)
        {
            return $"api/{endpoint}?folder={HttpUtility.UrlEncode(folder)}&file={HttpUtility.UrlEncode(fileName)}";
        }

        /// <summary>
        /// Tạo url path cho file vật lý đã lưu với endpoint là api/file/get
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetEndPoint(string folder, string fileName)
        {
            return $"api/file/get?folder={HttpUtility.UrlEncode(folder)}&file={HttpUtility.UrlEncode(fileName)}";
        }

        /// <summary>
        /// Sinh tên file mới bằng <c>"tên cũ" + DateTime.Now.ToFileTime()</c>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GenerateNewFileName(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            return $"{fileNameWithoutExt}-{DateTime.Now.ToFileTime()}{ext}";
        }

        public static string GetInterestPeriodTypeName(int interestType, int? interestPeriodQuantity, string interestPeriodType)
        {
            string text = "";
            if (interestType == InterestTypes.CUOI_KY)
            {
                text = "Cuối kỳ";
            }
            else
            {
                text = $"{interestPeriodQuantity} {GetNameDateType(interestPeriodType)}/lần";
            }

            return text;
        }

        public static string GetNameClassify(decimal? classify)
        {
            if (classify != null)
            {
                return classify switch
                {
                    1 => "PRO",
                    2 => "PROA",
                    3 => "PNOTE",
                    _ => "PRO",
                };
            }
            return "PRO";
        }

        public static string GetNameINVClassify(decimal? classify)
        {
            if (classify != null)
            {
                return classify switch
                {
                    1 => "FLEX",
                    2 => "FLASH",
                    3 => "FIX",
                    _ => "FLEX",
                };
            }
            return "FLEX";
        }

        public static string GetNameINVType(string type)
        {
            if (type != null)
            {
                return type switch
                {
                    "I" => "Cá nhân",
                    "B" => "Doanh nghiệp",
                    _ => "Cá nhân",
                };
            }
            return "Cá nhân";
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static string FindAndReplace(string docText, List<ReplaceTextDto> replateTextDtos)
        {
            string docTextReplate = docText;
            foreach (var text in replateTextDtos)
            {
                var replaceTextXml = text.ReplaceText;
                if (text.ReplaceText != null)
                {
                    replaceTextXml = HttpUtility.HtmlEncode(text.ReplaceText);
                }
                docTextReplate = docTextReplate.Replace(text.FindText, replaceTextXml);
            }
            return docTextReplate;
        }

        /// <summary>
        /// Fill con dấu vào hợp đồng
        /// </summary>
        /// <param name="stampImageUrl"></param>
        /// <param name="mainPart"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task FillStampImage(string stampImageUrl, MainDocumentPart mainPart, string filePath)
        {
            if (stampImageUrl != null)
            {
                var stampImage = FileUtils.GetPhysicalPath(stampImageUrl, filePath);
                if (File.Exists(stampImage.FullPath))
                {
                    var images = await File.ReadAllBytesAsync(stampImage.FullPath);
                    MemoryStream imageStream = new(images);
                    var picture = mainPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Pictures.Picture>().FirstOrDefault(p => p.NonVisualPictureProperties.NonVisualDrawingProperties.Name.Value.Contains("Picture"));
                    if (picture != null)
                    {
                        var blip = picture.BlipFill.Blip;
                        ImagePart newImagePath = mainPart.AddImagePart(ImagePartType.Png);
                        newImagePath.FeedData(imageStream);
                        blip.Embed = mainPart.GetIdOfPart(newImagePath);
                    }
                }
            }
        }

        public static TableProperties GetBorder()
        {
            TableProperties tblProperties = new TableProperties();

            //// Create Table Borders

            TableBorders tblBorders = new TableBorders();



            TopBorder topBorder = new TopBorder();

            topBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            topBorder.Color = "CC0000";

            tblBorders.AppendChild(topBorder);



            BottomBorder bottomBorder = new BottomBorder();

            bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            bottomBorder.Color = "000000";

            tblBorders.AppendChild(bottomBorder);



            RightBorder rightBorder = new RightBorder();

            rightBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            rightBorder.Color = "000000";

            tblBorders.AppendChild(rightBorder);



            LeftBorder leftBorder = new LeftBorder();

            leftBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            leftBorder.Color = "000000";

            tblBorders.AppendChild(leftBorder);



            InsideHorizontalBorder insideHBorder = new InsideHorizontalBorder();

            insideHBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            insideHBorder.Color = "000000";

            tblBorders.AppendChild(insideHBorder);



            InsideVerticalBorder insideVBorder = new InsideVerticalBorder();

            insideVBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);

            insideVBorder.Color = "000000";

            tblBorders.AppendChild(insideVBorder);



            //// Add the table borders to the properties

            tblProperties.AppendChild(tblBorders);

            return tblProperties;
        }
    }
}
