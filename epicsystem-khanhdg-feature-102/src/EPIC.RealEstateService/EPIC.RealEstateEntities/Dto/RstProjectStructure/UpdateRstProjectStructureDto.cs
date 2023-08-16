using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectStructure
{
    public class UpdateRstProjectStructureDto
    {
        public int Id { get; set; }

        private string _code;
        [Required(ErrorMessage = "Mã đơn vị không được để trống")]
        [StringLength(256, ErrorMessage = "Mã đơn vị không được dài hơn 256 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên gọi không được để trống")]
        [StringLength(256, ErrorMessage = "Tên gọi không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Loạt mật độ xây dựng
        /// </summary>
        public int BuildingDensityType { get; set; }
    }
}
