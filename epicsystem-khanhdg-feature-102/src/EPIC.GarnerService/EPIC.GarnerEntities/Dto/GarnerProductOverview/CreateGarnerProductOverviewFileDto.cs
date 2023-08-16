using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class CreateGarnerProductOverviewFileDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Id phân phối sản phẩm không được bỏ trống")]
        public int DistributionId { get; set; }


        [Required(ErrorMessage = "Loại tài liệu không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { DocumentTypes.THONG_TIN_SAN_PHAM, DocumentTypes.CHINH_SACH, DocumentTypes.HO_SO_PHAP_LY, DocumentTypes.FILE_CHINH_SACH }, ErrorMessage = "Loại tài liệu: 1: Thông tin sản phẩm, 2: Chính sách, 3: Hồ sơ pháp lý")]
        public int DocumentType { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set => _title = value?.Trim();
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => _url = value?.Trim();
        }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
