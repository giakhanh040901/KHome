﻿using EPIC.SIGN.HSN;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
using Microsoft.Extensions.Logging;

namespace EPIC.Utils.SharedApiService
{
    /// <summary>
    /// Class gọi đến các api dịch vụ chữ ký số, cách sử dụng chỉ cần inject vào
    /// </summary>
    public class SharedSignServerApiUtils
    {
        private readonly ILogger _logger;

        public static string DEFAULT_BASE64_IMAGE = "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";

        public SharedSignServerApiUtils(ILogger<SharedMediaApiUtils> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Yêu cầu ký file ảnh dạng base64 và trả về mảng byte
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public byte[] RequestSignBase64(SignPdfDto dto)
        {
            var signer = new PDFSigner(dto.Server, dto.AccessKey, dto.SecretKey, "hn1");
            return signer.SignBase64(dto.Base64Pdf, "SHA-1", dto.TypeSign, dto.Base64Image, dto.TextOut, dto.SignatureName, dto.pageSign, dto.XPoint, dto.YPoint, dto.Width, dto.Height);
        }

        /// <summary>
        /// Yêu cầu ký file pdf và trả về mảng byte
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public byte[] RequestSignPdf(RequestSignPdfDto dto)
        {
            var signer = new PDFSigner(dto.Server, dto.AccessKey, dto.SecretKey, "hn1");
            return signer.Sign(dto.FilePdfByteArray, "SHA-1", dto.TypeSign, dto.Base64Image, dto.TextOut, dto.SignatureName, dto.pageSign, dto.XPoint, dto.YPoint, dto.Width, dto.Height);
        }

    }

}