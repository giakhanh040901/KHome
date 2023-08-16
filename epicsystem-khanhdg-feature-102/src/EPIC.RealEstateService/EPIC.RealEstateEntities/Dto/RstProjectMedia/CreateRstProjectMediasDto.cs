using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMedia
{
    public class CreateRstProjectMediasDto
    {
        private string _location;
        //[Required(ErrorMessage = "Vị trí không được để trống")]
        //[StringRange(AllowableValues = new string[] { RstMediaLocations.BANNER_QUANG_CAO_DU_AN, RstMediaLocations.ANH_DAI_DIEN_DU_AN, RstMediaLocations.SLIDE_HINH_ANH_DU_AN, RstMediaLocations.TVC,
        //    RstMediaLocations.ANH_360, RstMediaLocations.ANH_VR, RstMediaLocations.CAN_HO_MAU_DU_AN, RstMediaLocations.ANH_MAT_BANG_DU_AN, RstMediaLocations.TIEN_ICH_NOI_KHU,
        //    RstMediaLocations.TIEN_ICH_NGOAI_KHU}, ErrorMessage = "Vui lòng chọn 1 trong các vị trí hình ảnh dự án sau")]
        public string Location
        {
            get => _location;
            set => _location = value?.Trim();
        }

        public List<CreateRstProjectMediaDto> RstProjectMedias { get; set; }
    }
}
