using EPIC.RealEstateEntities.Dto.RstProjectFile;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellFile
{
    public class CreateRstOpenSellFilesDto
    {
        [Required(ErrorMessage = "Loại hồ sơ mở bán không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { RstOpenSellFileTypes.TaiLieuPhanPhoi, RstOpenSellFileTypes.ChinhSachUuDai, 
            RstOpenSellFileTypes.ChuongTrinhBanHang, RstOpenSellFileTypes.TaiLieuBanHang }, ErrorMessage = "Vui lòng chọn 1 trong các loại chính sách sau")]
        public int OpenSellFileType { get; set; }
        public List<CreateRstOpenSellFileDto> RstOpenSellFiles { get; set; }
    }
}
