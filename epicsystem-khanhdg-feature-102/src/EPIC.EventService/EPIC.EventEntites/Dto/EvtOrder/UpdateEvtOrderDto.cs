using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class UpdateEvtOrderDto : CreateEvtOrderDto
    {
        public int Id { get; set; }
    }

    public class UpdateReferralCode
    {
        /// <summary>
        /// id order
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Mã giới thiệu đặt lệnh
        /// </summary>
        private string _saleReferralCode;
        public string SaleReferralCode
        {
            get => _saleReferralCode;
            set => _saleReferralCode = value?.Trim();
        }
    }
}
