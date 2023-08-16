using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.DataUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Contract
{
    public class ConfigContractCode
    {
        //Id sổ lệnh
        public const string ORDER_ID = "ORDER_ID";
        
        public const string ORDER_ID_PREFIX_0 = "ORDER_ID_PREFIX_0";

        /// <summary>
        /// Loại hình sản phẩm, viết liền không dấu
        /// </summary>
        public const string PRODUCT_TYPE = "PRODUCT_TYPE";
        public const string PRODUCT_CODE = "PRODUCT_CODE";
        public const string PRODUCT_NAME = "PRODUCT_NAME";
        public const string POLICY_CODE = "POLICY_CODE";
        public const string POLICY_NAME = "POLICY_NAME";


        public const string BUY_DATE = "BUY_DATE";
        /// <summary>
        /// Ngày thanh toán đủ
        /// </summary>
        public const string PAYMENT_FULL_DATE = "PAYMENT_FULL_DATE";
        public const string INVEST_DATE = "INVEST_DATE";

        /// <summary>
        /// Tên viết tắt nhà đầu tư (lấy các chữ cái đầu)
        /// </summary>
        public const string SHORT_NAME = "SHORT_NAME";
        /// <summary>
        /// Ký tự cố định bất kỳ, text, số, ký tự đặc biệt
        /// </summary>
        public const string FIX_TEXT = "FIX_TEXT";
        public const string DATE_DD = "DATE_DD";
        public const string DATE_MM = "DATE_MM";
        public const string DATE_YY = "DATE_YY";
        public const string DATE_YYYY = "DATE_YYYY";
        public const string DATE_DD_MM_YYYY = "DATE_DD_MM_YYYY";

        /// <summary>
        /// Tên viết tắt dự án
        /// </summary>
        public const string PROJECT_CODE = "PROJECT_CODE";

        /// <summary>
        /// RST Mã căn
        /// </summary>
        public const string RST_PRODUCT_ITEM_CODE = "RST_PRODUCT_ITEM_CODE";

        /// <summary>
        /// Mã dự án 
        /// </summary>
        public const string EVENT_CODE = "EVENT_CODE";

        /// <summary>
        /// Tất cả các config
        /// </summary>
        public static readonly string[] ALL = new string[]
        { 
            ORDER_ID, POLICY_NAME, POLICY_CODE, PRODUCT_CODE, PRODUCT_NAME, PRODUCT_TYPE,
            FIX_TEXT, DATE_DD, DATE_MM, DATE_YY,
            DATE_YYYY, PAYMENT_FULL_DATE, INVEST_DATE, SHORT_NAME, BUY_DATE,
            PROJECT_CODE, RST_PRODUCT_ITEM_CODE, EVENT_CODE
        };

        /// <summary>
        /// Sinh mã hợp đồng bằng từ cấu hình mã hợp đồng
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string GenContractCode(List<ConfigContractCodeDto> dictionary)
        {
            string contractCode = null;

            foreach (var item in dictionary)
            {
                if (item.Key == ConfigContractCode.ORDER_ID)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.ORDER_ID_PREFIX_0)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.PRODUCT_TYPE)
                {
                    contractCode += item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.PRODUCT_CODE)
                {
                    contractCode += item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.PRODUCT_NAME)
                {
                    contractCode += item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.POLICY_CODE)
                {
                    contractCode += item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.POLICY_NAME)
                {
                    contractCode += item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.SHORT_NAME)
                {
                    contractCode += item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.FIX_TEXT && item.Value != null)
                {
                    contractCode += item.Value.ToUnSign();
                }
                else if (item.Key == ConfigContractCode.DATE_DD)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.DATE_MM)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.DATE_YY)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.DATE_YYYY)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.BUY_DATE)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.PAYMENT_FULL_DATE)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.INVEST_DATE)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.PROJECT_CODE)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.RST_PRODUCT_ITEM_CODE)
                {
                    contractCode += item.Value;
                }
                else if (item.Key == ConfigContractCode.EVENT_CODE)
                {
                    contractCode += item.Value;
                }
            }
            return contractCode?.ToUpper();
        }


        public static readonly Dictionary<string, Func<dynamic, string>> Display = new Dictionary<string, Func<dynamic, string>>()
        {
            {
                ORDER_ID,
                (dynamic orderId) => orderId.ToString()
            },
            {
                POLICY_CODE,
                (dynamic policyCode) => policyCode.ToString()
            },
        };

        /// <summary>
        /// Sinh tên mã loại hình dự án GarnerProduct
        /// </summary>
        /// <returns></returns>
        public static string ProductTypes(int type)
        {
            return type switch
            {
                GarnerProductTypes.CO_PHIEU => "EB",
                GarnerProductTypes.CO_PHAN => "CSP",
                GarnerProductTypes.TRAI_PHIEU => "ET",
                GarnerProductTypes.BAT_DONG_SAN => "EI",
                _ => string.Empty
            };
        }
    }

    public class ConfigContractCodeDto 
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
