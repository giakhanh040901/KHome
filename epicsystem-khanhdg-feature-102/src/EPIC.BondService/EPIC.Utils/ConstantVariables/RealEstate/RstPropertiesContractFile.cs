using EPIC.Utils.ConstantVariables.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstPropertiesContractFile : PropertiesContractFile
    {
        #region Project
        /// <summary>
        /// Ký hiệu căn hộ
        /// </summary>
        public const string RST_PRODUCT_ITEM_CODE = "{{RstProductItemCode}}";
        /// <summary>
        /// Tên dự án
        /// </summary>
        public const string RST_PROJECT_NAME = "{{RstProjectName}}";
        /// <summary>
        /// Địa chỉ dự án
        /// </summary>
        public const string RST_PROJECT_ADDRESS = "{{RstProjectAddress}}";
        /// <summary>
        /// Thửa số
        /// </summary>
        public const string RST_PROJECT_LAND_PLOT_NO = "{{RstProjectLandPlotNo}}";
        /// <summary>
        /// Diện tích đất dự án
        /// </summary>
        public const string RST_PROJECT_LAND_AREA = "{{RstProjectLandArea}}";
        /// <summary>
        /// Diện tích xây dựng
        /// </summary>
        public const string RST_PROJECT_CONSTRUCTOR_AREA = "{{RstProjectConstructorArea}}";
        /// <summary>
        /// Thời gian dự kiến hoàn thành
        /// Tính 30 ngày kể từ ngày thực tế ký HĐMB để ra tháng BG dự kiến
        /// </summary>
        public const string RST_PROJECT_EXPECTED_HANDOVER_TIME = "{{RstProjectExpectedHandoverTime}}";
        #endregion

        #region ProductItem
        /// <summary>
        /// Loại căn hộ
        /// </summary>
        public const string RST_PRODUCT_ITEM_CLASSIFY_TYPE = "{{RstProductItemClassifyType}}";
        /// <summary>
        /// Diện tích thông thủy
        /// </summary>
        public const string RST_PRODUCT_ITEM_CARPET_AREA = "{{RstProductItemCarpetArea}}";
        /// <summary>
        /// Diện tích tim tường
        /// </summary>
        public const string RST_PRODUCT_ITEM_BUILT_UP_AREA = "{{RstProductItemBuiltUpArea}}";
        /// <summary>
        /// Đơn giá căn hộ
        /// </summary>
        public const string RST_PRODUCT_ITEM_UNIT_PRICE = "{{RstProductItemUnitPrice}}";
        /// <summary>
        /// Đơn giá căn hộ bằng chữ
        /// </summary>
        public const string RST_PRODUCT_ITEM_UNIT_PRICE_TEXT = "{{RstProductItemUnitPriceText}}";
        /// <summary>
        /// Tổng giá trị căn hộ
        /// </summary>
        public const string RST_PRODUCT_ITEM_PRICE = "{{RstProductItemPrice}}";
        /// <summary>
        /// Tổng giá trị căn hộ bằng chữ
        /// </summary>
        public const string RST_PRODUCT_ITEM_PRICE_TEXT = "{{RstProductItemPriceText}}";
        /// <summary>
        /// Tầng số
        /// </summary>
        public const string RST_PRODUCT_ITEM_NO_FLOOR = "{{RstProductItemNoFloor}}";
        public const string RST_PRODUCT_ITEM_LAND_AREA = "{{RstProductItemLandArea}}";

        #endregion

        #region Owner (CĐT)
        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public const string RST_OWNER = "{{RstOwner}}";
        #endregion

        #region Khác (Các giá trị bên ngoài)
        /// <summary>
        /// Có bao gồm chi phí bảo trì hay không
        /// </summary>
        public const string RST_IS_HAVE_MAINTENANCE_COST = "{{RstMaintenanceCost}}";
        /// <summary>
        /// Số tiền đặt cọc
        /// </summary>
        public const string RST_DEPOSIT_AMOUNT = "{{RstDepositAmount}}";
        public const string RST_GUARANTEE_AMOUNT = "{{RstGuaranteeAmount}}";
        /// <summary>
        /// Số tiền đặt cọc bằng chữ
        /// </summary>
        public const string RST_DEPOSIT_AMOUNT_TEXT = "{{RstDepositAmountText}}";
        public const string RST_GUARANTEE_AMOUNT_TEXT = "{{RstGuaranteeAmountText}}";
        #endregion
    }
}
