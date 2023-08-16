using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class RstOrderExtendedKeepTimeDto
    {
        /// <summary>
        /// ID lệnh
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Thời gian gia hạn (trên fe là phút nhưng dưới be lưu là giây)
        /// </summary>
        public int KeepTime { get; set; }
        /// <summary>
        /// Lý do 8. Khách ngoại giao, 9.Khách xin gia hạn thời gian, 10.Khác (Do Db đang lưu chung vào bảng historyUpdate)
        /// </summary>
        public int Reason { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Summary { get; set; }
    }
}
