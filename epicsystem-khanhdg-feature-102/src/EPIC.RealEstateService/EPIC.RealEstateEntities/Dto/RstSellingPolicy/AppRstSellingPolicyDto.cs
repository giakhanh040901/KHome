using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstSellingPolicy
{
    /// <summary>
    /// Chính sách ưu đãi của mở bán
    /// </summary>
    public class AppRstSellingPolicyDto
    {
        public int Id { get; set; }

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
        /// Mô tả chính sách
        /// </summary>
        public string Description { get; set; }
    }
}
