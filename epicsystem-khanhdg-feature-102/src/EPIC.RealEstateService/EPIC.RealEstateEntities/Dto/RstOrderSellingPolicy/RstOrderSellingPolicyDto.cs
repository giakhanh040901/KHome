using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy
{
    /// <summary>
    /// Chính sách ưu đãi của mở bán
    /// </summary>
    public class RstOrderSellingPolicyDto
    {
        public int? Id { get; set; }
        /// <summary>
        /// Id chính sách của đối tác
        /// </summary>
        public int? ProjectPolicyId { get; set; }
        /// <summary>
        /// Id chính sách của đại lý
        /// </summary>
        public int? SellingPolicyId { get; set; }
        /// <summary>
        /// Loại chính sách ưu đãi hiện thị trên App : 
        /// 1: Từ chủ đầu tư ( Đối tác cài chính sách ưu đãi vào thông tin căn hộ)
        /// 2: Từ đại lý (Cài trong mở bán) <br/>
        /// <see cref="AppRstPolicyTypes"/>
        /// </summary>
        public int PolicyType { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Giá trị quy ra tiền
        /// </summary>
        public decimal? ConversionValue { get; set; }
        /// <summary>
        /// được chọn hay không? Y: ĐƯợc tích, N: không được tích
        /// </summary>
        public string IsSelected { get; set; }
        public int Source { get; set; }
    }
}
