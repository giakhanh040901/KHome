using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Interfaces.Policy
{
    /// <summary>
    /// Kỳ hạn sản phẩm, những trường thông tin bắt buộc phải có của kỳ hạn
    /// </summary>
    public interface IProductPolicyDetail
    {
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên viết tắt
        /// </summary>
        public string ShortName { get; set; }

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
