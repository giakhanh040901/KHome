using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail
{
    public class MsbCollectionPaymentDto
    {
        public DataEntities.MsbRequestPaymentDetail RequestDetail { get; set; }
        public DataEntities.MsbNotificationPayment  NotificationPayment { get; set; }
    }
}
