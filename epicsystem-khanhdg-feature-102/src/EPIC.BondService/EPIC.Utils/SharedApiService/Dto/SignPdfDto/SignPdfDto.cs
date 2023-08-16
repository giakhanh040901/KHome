using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SignPdfDto
{
    public class SignPdfDto
    {
        /// <summary>
        /// Đường dẫn file pdf cần ký
        /// </summary>
        [Required(ErrorMessage = "Base64 file pdf không được bỏ trống")]
        public string Base64Pdf { get; set; }
        /// <summary>
        /// Base 64 ảnh muốn ký theo
        /// </summary>
        public string Base64Image { get; set; }

        /// <summary>
        /// Loại ký 0: không hiển thị, 1: hiển thĩ text, 2: hiển thị ảnh, 3: hiển thị text và ảnh
        /// </summary>
        [Required(ErrorMessage = "Loại ký không được bỏ trống")]
        public int TypeSign { get; set; }

        /// <summary>
        /// Tên Vị trí người ký
        /// </summary>
        public string SignatureName { get; set; }

        /// <summary>
        /// Text hiển thị
        /// </summary>
        [Required(ErrorMessage = "Text hiển thị không được bỏ trống")]
        public string TextOut { get; set; }
        /// <summary>
        /// Trang ký
        /// </summary>
        [Required(ErrorMessage = "Số trang đặt chữ ký không được bỏ trống")]
        public int pageSign { get; set; }
        /// <summary>
        /// Vị trí x khung ký
        /// </summary>
        public int XPoint { get; set; } = 100;
        /// <summary>
        /// Vị trý y khung ký
        /// </summary>
        public int YPoint { get; set; } = 20;
        /// <summary>
        /// Độ rộng khung ký
        /// </summary>
        public int Width { get; set; } = 200;
        /// <summary>
        /// Độ cao khung ký
        /// </summary>
        public int Height { get; set; } = 50;
        /// <summary>
        /// Server
        /// </summary>
        [Required(ErrorMessage = "Server không được bỏ trống")]
        public string Server { get; set; }

        /// <summary>
        /// AccessKey
        /// </summary>
        [Required(ErrorMessage = "AccessKey không được bỏ trống")]
        public string AccessKey { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        [Required(ErrorMessage = "SecretKey không được bỏ trống")]
        public string SecretKey { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        [Required(ErrorMessage = "File Download Name không được bỏ trống")]
        public string FileDownloadName { get; set; }
    }

    public class RequestSignPdfDto
    {
        /// <summary>
        /// Mảng byte file pdf cần ký
        /// </summary>
        public byte[] FilePdfByteArray { get; set; }
        /// <summary>
        /// Base 64 ảnh muốn ký theo
        /// </summary>
        public string Base64Image { get; set; }

        /// <summary>
        /// Loại ký 0: không hiển thị, 1: hiển thĩ text, 2: hiển thị ảnh, 3: hiển thị text và ảnh
        /// </summary>
        public int TypeSign { get; set; }

        /// <summary>
        /// Tên Vị trí người ký
        /// </summary>
        public string SignatureName { get; set; }

        /// <summary>
        /// Text hiển thị
        /// </summary>
        public string TextOut { get; set; }
        /// <summary>
        /// Trang ký
        /// </summary>
        public int pageSign { get; set; }
        /// <summary>
        /// Vị trí x khung ký
        /// </summary>
        public int XPoint { get; set; } = 100;
        /// <summary>
        /// Vị trý y khung ký
        /// </summary>
        public int YPoint { get; set; } = 20;
        /// <summary>
        /// Độ rộng khung ký
        /// </summary>
        public int Width { get; set; } = 200;
        /// <summary>
        /// Độ cao khung ký
        /// </summary>
        public int Height { get; set; } = 50;
        /// <summary>
        /// Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        public string FileDownloadName { get; set; }
    }
}
