using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class AppFilterProductItemDto
    {
        /// <summary>
        /// Id của mở bán
        /// </summary>
        [FromQuery(Name = "openSellId")]
        public int OpenSellId { get; set; }

        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        [FromQuery(Name = "buildingDensityId")]
        public int? BuildingDensityId { get; set; }

        private string _noFloor;
        /// <summary>
        /// Tầng số bao nhiêu
        /// </summary>
        [FromQuery(Name = "noFloor")]
        public string NoFloor
        {
            get => _noFloor;
            set => _noFloor = value?.Trim();
        }

        /// <summary>
        /// Hướng cửa
        /// </summary>
        [FromQuery(Name = "doorDirection")]
        public List<string> DoorDirection { get; set; }

        /// <summary>
        /// Giá bán tối thiểu
        /// </summary>
        [FromQuery(Name = "minSellingPrice")]
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa
        /// </summary>
        [FromQuery(Name = "maxSellingPrice")]
        public decimal? MaxSellingPrice { get; set; }

        /// <summary>
        /// Diện tích tối thiểu
        /// </summary>
        [FromQuery(Name = "minPriceArea")]
        public decimal? MinPriceArea { get; set; }

        /// <summary>
        /// Diện tích tối đa
        /// </summary>
        [FromQuery(Name = "maxPriceArea")]
        public decimal? MaxPriceArea { get; set; }

        /// <summary>
        /// Loại sổ đỏ
        /// </summary>
        [FromQuery(Name = "redBook")]
        public List<string> RedBook { get; set; }

        /// <summary>
        /// Trạng thái của sản phẩm: 2: Giữ chỗ, 5: Đã bán, 7: đang mở bán, 8: Đang giao dịch 
        /// </summary>
        public List<string> Status { get; set; }

        /// <summary>
        /// Loại phòng
        /// </summary>
        [FromQuery(Name = "roomType")]
        public int? RoomType { get; set; }

        private string _keyword { get; set; }
        /// <summary>
        /// Tìm kiếm theo mã căn hộ
        /// </summary>
        [FromQuery(Name = "keyword")]
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }
    }
}
