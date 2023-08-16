using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    public class UpdateLoyConversionPointDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Loại yêu cầu: 1: Đổi Voucher, 2: Đổi điểm
        /// <see cref="LoyRequestTypes"/>
        /// </summary>
        public int RequestType { get; set; }

        /// <summary>
        /// Loại cấp phát: 1: Khách hàng đổi điểm (Mặc định khi tạo trên App), 2: Tặng khách hàng
        /// <see cref="LoyAllocationTypes"/>
        /// </summary>
        public int AllocationType { get; set; }

        /// <summary>
        /// Có trừ điểm không? Mặc định là có
        /// <see cref="YesNo"/>
        /// </summary>
        public string IsMinusPoint { get; set; }

        /// <summary>
        /// Ngày yêu cầu
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Chi tiết yêu cầu
        /// </summary>
        public List<UpdateLoyConversionPointDetailDto> Details { get; set; }
    }
}
