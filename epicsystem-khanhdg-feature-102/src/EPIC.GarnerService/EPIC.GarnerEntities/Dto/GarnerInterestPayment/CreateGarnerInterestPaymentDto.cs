using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    public class CreateGarnerInterestPaymentDto
    {
        private string _cifCode;
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }

        /// <summary>
        /// Id chính sách 
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Tổng số tiền phải chi
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Ngày chi
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Kỳ chi trả
        /// </summary>
        public int PeriodIndex { get; set; }

        /// <summary>
        /// Chi tiết
        /// </summary>
        public List<CreateGarnerInterestPaymentDetailDto> Details { get; set; }
    }
}
