using EPIC.Utils.ConstantVariables.Contract;

namespace EPIC.Utils.ConstantVariables.Event
{
    /// <summary>
    /// Tên các trường fill thông tin
    /// </summary>
    public class EvtPropertiesTicketFill : PropertiesContractFile
    {
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public const string EVENT_NAME = "{{EventName}}";

        /// <summary>
        /// Hình ảnh sự kiện (hình đại diện)
        /// </summary>
        public const string EVENT_AVATAR = "{{EventAvatar}}";

        /// <summary>
        /// Địa điểm tổ chức sự kiện
        /// </summary>
        public const string EVENT_LOCATION = "{{EventLocation}}";

        /// <summary>
        /// Thời gian bắt đầu sự kiện
        /// </summary>
        public const string EVENT_START_DATE = "{{EventStartDate}}";

        /// <summary>
        /// Tên vé
        /// </summary>
        public const string TICKET_NAME = "{{TicketName}}";

        /// <summary>
        /// Mã qr của vé
        /// </summary>
        public const string TICKET_QR_CODE = "{{TicketQrCode}}";
        /// <summary>
        /// Thời gian check in dự kiến
        /// </summary>
        public const string TICKET_CHECK_IN = "{{CheckIn}}";
        /// <summary>
        /// Mã QR giao nhận vé
        /// </summary>
        public const string DELIVERY_QR_CODE = "{{DeliveryQrCode}}";
    }
}
