using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Interfaces.Payment
{
    /// <summary>
    /// Chi tiền tự động: NHững trường bắt buộc ở những bảng có tiền vào tiền ra
    /// </summary>
    public interface IPaymentAuto
    {
        /// <summary>
        /// Trạng thái từ Bank: 1: CHỜ PHẢN HỒI, 2 THÀNH CÔNG, 3 THẤT BẠ
        /// </summary>
        public int? StatusBank { get; set; }
    }
}
