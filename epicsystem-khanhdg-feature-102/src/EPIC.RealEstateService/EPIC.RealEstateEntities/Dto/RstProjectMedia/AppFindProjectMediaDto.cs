using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMedia
{
    public class AppFindProjectMediaDto
    {
        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        public string UrlImage { get; set; }

        /// <summary>
        /// Đường dẫn điều hướng khi click ảnh
        /// </summary>
        public string UrlPath { get; set; }

        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        public string MediaType { get; set; }
    }
}
