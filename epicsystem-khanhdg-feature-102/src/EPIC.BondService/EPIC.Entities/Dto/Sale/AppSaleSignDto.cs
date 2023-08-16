using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSaleSignItemDto
    {
        [Range(1, int.MaxValue)]
        public int SaleTempId { get; set; }
        public bool IsSign { get; set; }
    }

    public class AppSaleSignDto
    {
        private string _otp;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã otp không được để trống")]
        public string Otp
        { 
            get => _otp;
            set => _otp = value?.Trim(); 
        }

        [Required(ErrorMessage = "Danh sách ký không được để trống")]
        [MinLength(1, ErrorMessage = "Chọn tối thiểu một đại lý")]
        public List<AppSaleSignItemDto> ListSign { get; set; }
    }
}
