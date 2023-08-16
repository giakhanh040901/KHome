using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Garner;

namespace EPIC.GarnerEntities.Dto.GarnerProduct
{
    public class CreateGarnerProductFileDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Id sản phẩm không được bỏ trống")]
        public int ProductId { get; set; }


        [Required(ErrorMessage = "Loại tài liệu không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { GarnerDocumentTypes.TAI_SAN_DAM_BAO, GarnerDocumentTypes.HO_SO_PHAP_LY }, ErrorMessage = "Loại tài liệu: 1: Tài sản đảm bảo, 2: Hồ sơ pháp lý")]
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
        [Required(ErrorMessage = "Tổng giá trị tài sản không được bỏ trống")]
        public decimal TotalValue { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
