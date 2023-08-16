using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectFile
{
    public class CreateRstProjectFilesDto
    {
        [Required(ErrorMessage = "Loại hồ sơ không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { RstProjectFileTypes.HoSoPhapLy, RstProjectFileTypes.TaiLieuBanHang }, ErrorMessage = "Vui lòng chọn 1 trong các loại hồ sơ sau")]
        public int JuridicalFileType { get; set; }
        public List<CreateRstProjectFileDto> RstProjectJuridicalFiles { get; set; }
    }
}
