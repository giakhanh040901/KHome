using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemExtend
{
    public class CreateRstProductItemExtendDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tiêu đề thông tin
        /// </summary>
        private string _title;
        public string Title
        {
            get => _title;
            set => _title = value?.Trim();
        }

        /// <summary>
        /// Tên icon
        /// </summary>
        private string _iconName;
        public string IconName
        {
            get => _iconName;
            set => _iconName = value?.Trim();
        }

        /// <summary> 
        /// Mô tả nội dung thông tin
        /// </summary>
        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
