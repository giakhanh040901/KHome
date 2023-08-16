using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppFindProjectDto
    {
        private string _keyword;
        /// <summary>
        /// Loại hình dự án: 1: Đất đấu giá, 2: Đất BT, 3: Đất giao
        /// </summary>
        [FromQuery(Name = "projectType")]
        //[IntegerRange(AllowableValues = new int[] { RstProjectTypes.DAT_BT, RstProjectTypes.DAT_GIAO, RstProjectTypes.DAT_ĐAU_GIA })]
        public List<string> ProjectType { get; set; }
        /// <summary>
        /// Loại hình sản phẩm 1: Chung cư, 2 Biệt thự, 3: Liền kề, 4 Khách sạn...
        /// </summary>
        [FromQuery(Name = "productType")]
        //[IntegerRange(AllowableValues = new int[] { RstProductTypes.CanGhep, RstProductTypes.CanGhep })]
        public List<string> ProductType { get; set; }
        /// <summary>
        /// Mã tỉnh thành
        /// </summary>
        [FromQuery(Name = "provinceCode")]
        public List<string> ProvinceCode { get; set; }
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
        /// Giấy tờ pháp lý
        /// </summary>
        
        [FromQuery(Name = "redBook")]
        //[IntegerRange(AllowableValues = new int[] { RstRedBookTypes.HasRedBook50Year, RstRedBookTypes.NoRedBook, RstRedBookTypes.HasRedBook })]
        public List<string> RedBook { get; set; }

        [FromQuery(Name = "keyword")]
        public string Keyword { get => _keyword; set => _keyword = value?.Trim(); }

        private string _isOutstanding;
        [FromQuery(Name = "isOutstanding")]
        public string IsOutstanding { get => _isOutstanding; set => _isOutstanding = value.Trim(); }

        [FromQuery(Name = "isSaleView")]
        public bool IsSaleView { get; set; }

        /// <summary>
        /// Có phải dự án yêu thích hay không
        /// </summary>
        [FromQuery(Name = "isFavourite")]
        public bool? IsFavourite { get; set; }
    }
}
