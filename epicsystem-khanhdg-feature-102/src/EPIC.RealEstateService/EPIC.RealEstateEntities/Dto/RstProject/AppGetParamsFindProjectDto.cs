using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppGetParamsFindProjectDto
    {
        /// <summary>
        /// Danh sách tỉnh thành
        /// </summary>
        public List<AppGetParamsProvince> ListProvince { get; set; }
        /// <summary>
        /// Loại hình dự án: 1: Đất đấu giá, 2: Đất BT, 3: Đất giao
        /// </summary>
        public List<int?> ListProjectTypes  { get; set; }
        /// <summary>
        /// Phân loại sản phẩm: 1: Chung cư, 2 Biệt thự, 3: Liền kề, 4 Khách sạn...
        /// </summary>
        public List<int> ListProductTypes { get; set; }
        /// <summary>
        /// Giá bán tối thiểu
        /// </summary>
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa
        /// </summary>
        public decimal? MaxSellingPrice { get; set; }
    }

    public class AppGetParamsProvince 
    { 
        /// <summary>
        /// Mã tỉnh thành
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        public string ProvinceName { get; set; }
    }
}
