using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public static class RstFieldName
    {
        /// <summary>
        /// Common
        /// </summary>
        public const string UPDATE_OPEN_SELL_DETAIL = "OPEN_SELL_DETAIL";
        public const string UPDATE_DISTRIBUTION_POLICY = "DISTRIBUTION_POLICY_ID";
        public const string UPDATE_SALE_REFERRAL_CODE = "SALE_REFERRAL_CODE";
        public const string UPDATE_SOURCE = "SOURCE";
        public const string UPDATE_BUSINESS_BANK_ACC = "BUSINESS_CUSTOMER_BANK_ACC_ID";
        public const string UPDATE_INVESTOR_IDEN_ID = "INVESTOR_IDEN_ID";
        public const string UPDATE_STATUS = "STATUS";
        public const string UPDATE_CONTACT_ADDRESS_ID = "CONTACT_ADDRESS_ID";
        /// <summary>
        /// Hình thức thanh toán của hợp đồng
        /// </summary>
        public const string UPDATE_ORDER_PAYMEN_TYPE = "PAYMENT_TYPE";
        public const string UPDATE_BUILDING_DENSITY_ID = "BUILDING_DENSITY_ID";

        #region Product item
        public const string UPDATE_PRODUCT_ITEM = "PRODUCT_ITEM_ID";
        public const string UPDATE_PRODUCT_ITEM_CLASSIFY_TYPE = "CLASSIFY_TYPE";
        public const string UPDATE_PRODUCT_ITEM_RED_BOOK_TYPE = "RED_BOOK_TYPE";
        public const string UPDATE_PRODUCT_ITEM_CODE = "CODE";
        public const string UPDATE_PRODUCT_ITEM_NAME = "NAME";
        public const string UPDATE_PRODUCT_ITEM_NUMBER_FLOOR = "NUMBER_FLOOR";
        public const string UPDATE_PRODUCT_ITEM_NO_FLOOR = "NO_FLOOR";
        public const string UPDATE_PRODUCT_ITEM_ROOM_TYPE = "ROOM_TYPE";
        public const string UPDATE_PRODUCT_ITEM_DOOR_DIRECTION = "DOOR_DIRECTION";
        public const string UPDATE_PRODUCT_ITEM_BALCONY_DIRECTION = "BALCONY_DIRECTION";
        public const string UPDATE_PRODUCT_ITEM_PRODUCT_LOCATION = "PRODUCT_LOCATION";
        public const string UPDATE_PRODUCT_ITEM_PRODUCT_TYPE = "PRODUCT_TYPE";
        public const string UPDATE_PRODUCT_ITEM_HANDING_TYPE = "HANDING_TYPE";
        public const string UPDATE_PRODUCT_ITEM_VIEW_DESCRIPTION = "VIEW_DESCRIPTION";
        public const string UPDATE_PRODUCT_ITEM_HANDOVER_TIME = "HANDOVER_TIME";
        public const string UPDATE_PRODUCT_ITEM_CARPET_AREA = "CARPET_AREA";
        public const string UPDATE_PRODUCT_ITEM_BUILT_UP_AREA = "BUILT_UP_AREA";
        public const string UPDATE_PRODUCT_ITEM_FLOOR_BUILDING_AREA = "FLOOR_BUILDING_AREA";
        public const string UPDATE_PRODUCT_ITEM_PRICE_AREA = "PRICE_AREA";
        public const string UPDATE_PRODUCT_ITEM_COMPOUND_ROOM = "COMPOUND_ROOM";
        public const string UPDATE_PRODUCT_ITEM_COMPOUND_FLOOR = "COMPOUND_FLOOR";
        public const string UPDATE_PRODUCT_ITEM_UNIT_PRICE = "UNIT_PRICE";
        public const string UPDATE_PRODUCT_ITEM_STATUS = "STATUS_POLICY";
        public const string UPDATE_PRODUCT_ITEM_PRICE = "PRICE";
        public const string UPDATE_PRODUCT_ITEM_BUILDING_DENSITY_ID = "BUILDING_DENSITY_ID";

        #endregion

        #region Oder payment
        public const string UPDATE_ORDER_PAYMNET_TRAN_DATE = "TRAN_DATE";
        public const string UPDATE_ORDER_PAYMNET_AMOUNT = "PAYMNET_AMOUNT";
        public const string UPDATE_ORDER_PAYMNET_TYPE = "PAYMNET_TYPE";
        public const string UPDATE_ORDER_PAYMNET_DESCRIPTION = "DESCRIPTION";
        #endregion

        #region Oder
        public const string UPDATE_ORDER_UPLOAD_FILE_SCAN = "UPLOAD_FILE_SCAN";
        public const string UPDATE_ORDER_CANCEL = "CANCEL";
        public const string UPDATE_ORDER_APPROVE = "APPROVE";
        public const string UPDATE_ORDER_EXP_TIME_DEPOSIT = "EXP_TIME_DEPOSIT";
        #endregion

        #region Order co owner
        public const string UPDATE_ORDER_CO_OWNER_FULL_NAME = "FULL_NAME";
        public const string UPDATE_ORDER_CO_OWNER_PLACE_OF_ORIGIN = "PLACE_OF_ORIGIN";
        public const string UPDATE_ORDER_CO_OWNER_ID_NO = "ID_NO";
        #endregion
    }
}
