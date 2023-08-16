using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.DataUtils
{
    public static class StringUtils
    {
        public const string SEPARATOR = ",";

        /// <summary>
        /// Che số điện thoại bằng dấu sao (038****291)
        /// </summary>
        /// <param name="phone">Số điện thoại</param>
        /// <param name="startIndex">Vị trí bắt đầu che</param>
        /// <param name="endIndexFromRight">Vị trí che cuối cùng, tính từ phải sang</param>
        /// <returns></returns>
        public static string HidePhone(string phone, int startIndexFromRight = 3, int hideLen = 4)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                int startIndex = phone.Length - hideLen - startIndexFromRight;
                string hideText = new string('*', hideLen);

                string result = phone.Remove(startIndex, hideLen).Insert(startIndex, hideText);
                return result;
            }

            return "";
        }

        /// <summary>
        /// Che email bằng dấu sao (ha******@gmail.com)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string HideEmail(string email, int startIndex = 2)
        {
            if (!string.IsNullOrEmpty(email))
            {
                int splitIndex = email.LastIndexOf("@");

                int hideLen = splitIndex - startIndex;
                string hideText = hideLen > 0 ? new string('*', hideLen) : new string('*', startIndex);

                string result = "";
                if (hideLen > 0)
                {
                    result = email.Remove(startIndex, hideLen).Insert(startIndex, hideText);
                }
                else
                {
                    result = email.Remove(0, startIndex).Insert(0, hideText);
                }
                return result;
            }

            return "";
        }

        /// <summary>
        /// Chuyển string sang dạng snake upper case
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToSnakeUpperCase(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length < 2)
            {
                return text.ToUpper();
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// Chuyển sang chữ không dấu
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnSign(this string str)
        {
            if (str == null)
            {
                return null;
            }    
            string stFormD = str.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        /// <summary>
        /// Lấy chữ cái đầu từng từ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstLetterEachWord(this string str)
        {
            if (str == null)
            {
                return null;
            }
            string shortName = "";
            str.Split(' ').ToList().ForEach(i => shortName += i[0].ToString());
            return shortName;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Hàm chuyển sang dạng PascalCase (Ex: "examplePascal -> ExamplePascal")
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string text)
        {
            if (text == null)
            {
                return null;
            }
            if (text.Length < 2)
            {
                return text.ToUpper();
            }
            string pascalCase = char.ToUpper(text[0]) + text.Substring(1);

            return pascalCase;
        }
    }
}
