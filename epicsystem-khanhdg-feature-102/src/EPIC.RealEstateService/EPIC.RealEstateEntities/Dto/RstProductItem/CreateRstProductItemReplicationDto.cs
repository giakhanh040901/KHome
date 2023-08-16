using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class CreateRstProductItemReplicationDto
    {
        private string _code;
        [Required(ErrorMessage = "Mã căn/mã sản phẩm không được để trống")]
        [StringLength(256, ErrorMessage = "Mã căn/mã sản phẩm không được dài hơn 256 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Số căn/tên không được để trống")]
        [StringLength(256, ErrorMessage = "Số căn/tên không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Tầng số bao nhiêu
        /// </summary>
        private string _numberFloor;
        public string NumberFloor
        {
            get => _numberFloor;
            set => _numberFloor = value?.Trim();
        }

        /// <summary>
        /// Số tầng bao nhiêu
        /// </summary>
        private string _noFloor;
        public string NoFloor
        {
            get => _noFloor;
            set => _noFloor = value?.Trim();
        }
    }
}
