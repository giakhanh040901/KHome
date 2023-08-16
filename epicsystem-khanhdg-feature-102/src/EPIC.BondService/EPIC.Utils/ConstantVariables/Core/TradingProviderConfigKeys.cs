using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    public static class TradingProviderConfigKeys
    {
        public const string DeletedOrderInvest = "DeletedOrderInvest";
        public const string DeletedOrderGarner = "DeletedOrderGarner";
        public const string DeletedOrderRealEstate = "DeletedOrderRealEstate";

        /// <summary>
        /// Thời gian gửi thông báo sắp đến sinh nhật nhà đầu tư cho Sale
        /// </summary>
        public const string TimeSendNotiBirthDayCustomerForSale = "TimeSendNotiBirthDayCustomerForSale";

        /// <summary>
        /// Thời gian gửi thông báo chúc mừng sinh nhật
        /// </summary>
        public const string TimeSendNotiHappyBirthDayCustomer = "TimeSendNotiHappyBirthDayCustomer";

        /// <summary>
        /// Thời gian gửi thông báo sự kiện sắp diễn ra
        /// </summary>
        public const string EventTimeSendNotiEventUpcomingForCustomer = "EventTimeSendNotiEventUpComingForCustomer";

        /// <summary>
        /// Ngày gửi thông báo sự kiện sắp diễn ra/ Trước bao nhiêu ngày
        /// </summary>
        public const string EventDaySendNotiEventUpcomingForCustomer = "EventDaySendNotiEventUpcomingForCustomer";

        public static List<string> ConfigKeys = new()
        {
            DeletedOrderInvest,
            DeletedOrderGarner,
            DeletedOrderRealEstate,
            TimeSendNotiBirthDayCustomerForSale,
            TimeSendNotiHappyBirthDayCustomer,
            EventTimeSendNotiEventUpcomingForCustomer,
            EventDaySendNotiEventUpcomingForCustomer
        };
    }
}
