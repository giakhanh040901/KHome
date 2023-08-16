using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionPolicy
{
    public class RstDistributionPolicyDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id bán phân phối
        /// </summary>
        public int DistributionId { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loại thanh toán 1: Thanh toán thường, 2: Trả góp ngân hàng, 3: Trả trước<br/>
        /// <see cref="RstProjectDistributionPolicyPaymentTypes"/>
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// Loại hình đặt cọc<br/>
        /// <see cref="RstDistributionPolicyTypes"/>
        /// </summary>
        public int? DepositType { get; set; }

        /// <summary>
        /// Giá trị đặt cọc (%/VNĐ)
        /// </summary>
        public decimal DepositValue { get; set; }

        /// <summary>
        /// Loại hình lock căn hộ<br/>
        /// <see cref="RstDistributionPolicyTypes"/>
        /// </summary>
        public int? LockType { get; set; }

        /// <summary>
        /// Giá trị lock căn hộ (%/VNĐ)
        /// </summary>
        public decimal LockValue { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
