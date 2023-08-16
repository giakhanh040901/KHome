using EPIC.Utils.Attributes;
using System;

namespace EPIC.PaymentEntities.Dto.Msb
{
    public class MsbNotificationDto
    {
        public long Id { get; set; }

        public string TranSeq { get; set; }
        public string TranId { get; set; }

        public string VaCode { get; set; }

        public string VaNumber { get; set; }

        public string FromAccountName { get; set; }

        public string FromAccountNumber { get; set; }

        public string ToAccountName { get; set; }

        public string ToAccountNumber { get; set; }

        public string TranAmount { get; set; }

        public string TranRemark { get; set; }

        public string TranDate { get; set; }

        //public string Signature { get; set; }

        public string Exception { get; set; }

        public int Status { get; set; }

        public string Ip { get; set; }

        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Loại sản phẩm
        /// </summary>
        public string ProjectType { get; set; }

        public long? ReferId { get; set; }
    }
}
