using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Interfaces.Policy
{
    /// <summary>
    /// Chính sách sản phẩm, những trường thông tin bắt buộc phải có của chính sách
    /// </summary>
    public interface IProductPolicy
    {
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loại nhà đầu tư P: chuyên nghiệp, A: tất cả
        /// </summary>
        public string InvestorType { get; set; }

        /// <summary>
        /// Tính lợi tức Net, Gross
        /// </summary>
        public int CalculateType { get; set; }

        /// <summary>
        /// Có show app
        /// </summary>
        public string IsShowApp { get; set; }

        /// <summary>
        /// Trạng thái A, D
        /// </summary>
        public string Status { get; set; }
    }
}
