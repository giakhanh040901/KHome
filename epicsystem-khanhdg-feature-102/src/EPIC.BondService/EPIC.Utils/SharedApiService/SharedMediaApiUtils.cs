using EPIC.Utils.ConfigModel;
using EPIC.Utils.Net.MimeTypes;
using EPIC.Utils.SharedApiService.Dto.QrCodeDto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService
{
    /// <summary>
    /// Class gọi đến các api của media, cách sử dụng chỉ cần inject vào
    /// </summary>
    public class SharedMediaApiUtils
    {
        private readonly ILogger _logger;
        private readonly IOptions<SharedApiConfiguration> _sharedApiConfiguration;
        private readonly string _apiDocxToPdf;
        private readonly string _apiGenQrCode;

        public SharedMediaApiUtils(ILogger<SharedMediaApiUtils> logger, IOptions<SharedApiConfiguration> sharedApiConfiguration)
        {
            _logger = logger;
            _sharedApiConfiguration = sharedApiConfiguration;
            _apiDocxToPdf = sharedApiConfiguration.Value.ApiDocxToPdf;
            _apiGenQrCode = sharedApiConfiguration.Value.ApiQrCode;
        }

        /// <summary>
        /// trả về file pdf và lưu vào đường dẫn output, nếu trả về khác 200 thì sẽ ném ra httpexception
        /// </summary>
        /// <param name="docxFilePath">file path docx input</param>
        /// <param name="pdfFilePathOutput">file path pdf output</param>
        /// <returns></returns>
        [Obsolete("Chuyển qua dùng hàm ConvertWordToPdfAsync(byte[] docxFileStream, string pdfFilePathOutput)")]
        public async Task ConvertWordToPdfAsync(string docxFilePath, string pdfFilePathOutput)
        {
            HttpClient httpClient = new();

            using var multipartFormContent = new MultipartFormDataContent();
            var fileStream = File.OpenRead(docxFilePath);

            var fileStreamContent = new StreamContent(fileStream);
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);

            //Add the file
            multipartFormContent.Add(fileStreamContent, name: "document", fileName: Path.GetFileName(fileStream.Name));

            //Send it
            string path = _apiDocxToPdf + "/api/tools-windows/docx-to-pdf";
            var response = await httpClient.PostAsync(path, multipartFormContent);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"Error Call Api convert docx to pdf: Path = {path}, StatusCode = {response.StatusCode}, ResponseBody = {await response.Content.ReadAsStringAsync()}");
                throw new FaultException(new FaultReason($"Không gọi được api chuyển đổi docx to pdf"), new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
            }

            using FileStream outFileStream = new(pdfFilePathOutput, FileMode.Create);
            await response.Content.CopyToAsync(outFileStream);
            outFileStream.Close();
        }

        /// <summary>
        /// trả về file pdf và lưu vào đường dẫn output, nếu trả về khác 200 thì sẽ ném ra httpexception
        /// </summary>
        /// <param name="docxFileStream"></param>
        /// <param name="pdfFilePathOutput"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task ConvertWordToPdfAsync(byte[] docxFileStream, string pdfFilePathOutput)
        {
            HttpClient httpClient = new();

            using var multipartFormContent = new MultipartFormDataContent();

            var content = new ByteArrayContent(docxFileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);

            //Add the file
            multipartFormContent.Add(content, name: "document", fileName: "fileName.docx");

            //Send it
            string path = _apiDocxToPdf + "/api/tools-windows/docx-to-pdf";
            var response = await httpClient.PostAsync(path, multipartFormContent);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"Error Call Api convert docx to pdf: Path = {path}, StatusCode = {response.StatusCode}, ResponseBody = {await response.Content.ReadAsStringAsync()}");
                throw new FaultException(new FaultReason($"Không gọi được api chuyển đổi docx to pdf"), new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
            }

            using FileStream outFileStream = new(pdfFilePathOutput, FileMode.Create);
            await response.Content.CopyToAsync(outFileStream);
            outFileStream.Close();
        }

        /// <summary>
        /// trả về file pdf và lưu vào đường dẫn output, nếu trả về khác 200 thì sẽ ném ra httpexception
        /// </summary>
        /// <param name="docxFileStream"></param>
        /// <param name="pdfFilePathOutput"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task<byte[]> ConvertWordToPdfAsync(byte[] docxFileStream)
        {
            HttpClient httpClient = new();
            using var multipartFormContent = new MultipartFormDataContent();

            var content = new ByteArrayContent(docxFileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument);

            //Add the file
            multipartFormContent.Add(content, name: "document", fileName: "fileName.docx");

            //Send it
            string path = _apiDocxToPdf + "/api/tools-windows/docx-to-pdf";
            var response = await httpClient.PostAsync(path, multipartFormContent);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"Error Call Api convert docx to pdf: Path = {path}, StatusCode = {response.StatusCode}, ResponseBody = {await response.Content.ReadAsStringAsync()}");
                throw new FaultException(new FaultReason($"Không gọi được api chuyển đổi docx to pdf"), new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
            }

            using MemoryStream ms = new();
            await response.Content.CopyToAsync(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// trả về file pdf, nếu trả về khác 200 thì sẽ ném ra httpexception
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> GenQrCode(string content)
        {
            HttpClient httpClient = new();
            //Send it
            string path = _apiGenQrCode + $"/api/media/qr-code?content={content}";
            _logger.LogInformation($"Call Api Gen QrCode path: {path}");
            var response = await httpClient.GetAsync(path);
            var resBody = await response.Content.ReadAsStringAsync();
            if(response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonSerializer.Deserialize<QrCodeContentDto>(resBody);
                var resultByteArr = Convert.FromBase64String(result.base64?.Substring(22));
                return resultByteArr;
            }
            else
            {
                _logger.LogError($"Error Call Api gen QrCode: Path = {path}, StatusCode = {response.StatusCode}, ResponseBody = {await response.Content.ReadAsStringAsync()}");
                throw new FaultException(new FaultReason($"Không gọi được api gen QrCode"), new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
            }
        }
    }
}
