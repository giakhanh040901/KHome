using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectStructure
{
    public class CreateRstProjectStructureDto
    {
        public int ProjectId { get; set; }

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

        //[Required(ErrorMessage = "Loại mật độ xây dựng không được bỏ trống")]
        //[IntegerRange(AllowableValues = new int[] { RstBuildingDensityTypes.ODat, RstBuildingDensityTypes.Lo, RstBuildingDensityTypes.PhanKhu, RstBuildingDensityTypes.Toa, RstBuildingDensityTypes.Tang }, ErrorMessage = "Vui lòng chọn 1 trong các loại mật độ xây dựng sau")]
        public int? BuildingDensityType { get; set; }
        public int? ParentId { get; set; }
    }
}
