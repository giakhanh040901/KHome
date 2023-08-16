using EPIC.Utils.ConfigModel;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.Utils.Recognition.FPT
{
    public class OCRUtils
    {
        private readonly RecognitionApiConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        private const int AGE_MIN = 18;

        public OCRUtils(RecognitionApiConfiguration config, ILogger logger)
        {
            _config = config;
            _httpClient = initHttpClient(config);
            _logger = logger;
        }

        private HttpClient initHttpClient(RecognitionApiConfiguration config)
        {
            HttpClient httpClient = new();
            httpClient.BaseAddress = new Uri(config.ApiBaseAddress);
            httpClient.DefaultRequestHeaders.Add("api-key", config.ApiKey);

            return httpClient;
        }

        public void CheckIdType(string typeRequest, string typeResponse)
        {
            if (typeRequest != typeResponse)
            {
                throw new FaultException(new FaultReason("Ảnh giấy tờ tải lên không hợp lệ vui lòng kiểm tra lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRIDTypeInvalid).ToString()), "");
            }
        }

        /// <summary>
        /// Kiểm tra độ khớp khuôn mặt
        /// </summary>
        /// <param name="similarity"></param>
        /// <returns></returns>
        public bool CheckFaceSimilarity(double similarity)
        {
            return similarity >= _config.FaceSimilarity;
        }

        public void CheckBackImage(IFormFile bankImage)
        {
            if (bankImage == null)
            {
                throw new FaultException(new FaultReason("Ảnh mặt sau CMND/CCCD không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
        }

        /// <summary>
        /// Kiểm tra ảnh khác loại của cmnd/cccd
        /// </summary>
        /// <param name="frontData"></param>
        /// <param name="backData"></param>
        /// <exception cref="FaultException"></exception>
        public void CheckDifferenceImage(OCRFrontIdDataNewType frontData, OCRBackIdDataNewType backData)
        {
            bool cmndOk = frontData.Type == OCRTypes.CMND_FRONT && backData.Type == OCRTypes.CMND_BACK;
            bool cccdOk = frontData.Type == OCRTypes.CCCD_FRONT && backData.Type == OCRTypes.CCCD_BACK;
            bool chipOk = frontData.Type == OCRTypes.CCCD_CHIP_FRONT && backData.Type == OCRTypes.CCCD_CHIP_BACK;

            if (!(cmndOk || cccdOk || chipOk))
            {
                throw new FaultException(new FaultReason("Ảnh mặt trước và mặt sau không cùng loại giấy tờ"), new FaultCode(((int)ErrorCode.InvestorErrorOCRDifference).ToString()), "");
            }
        }

        /// <summary>
        /// Kiểm tra loại giấy tờ người dùng chọn và loại trên giấy tờ có khớp nhau ko
        /// </summary>
        /// <param name="uploadType"></param>
        /// <param name="frontData"></param>
        public void CheckUploadTypeAndIdType(string uploadType, OCRFrontIdDataNewType frontData)
        {
            var listCmndTypeOcr = new string[] { OCRTypesNews.CMND_12_FRONT, OCRTypesNews.CMND_09_FRONT };
            bool cmndOk = uploadType == CardTypesInput.CMND && listCmndTypeOcr.Contains(frontData.TypeNew);
            bool cccdOk = uploadType == CardTypesInput.CCCD && frontData.TypeNew == OCRTypesNews.CCCD_12_FRONT;
            bool chipOk = uploadType == CardTypesInput.CCCD && frontData.TypeNew == OCRTypesNews.CCCD_CHIP_FRONT;

            if (!(cmndOk || cccdOk | chipOk))
            {
                throw new FaultException(new FaultReason("Loại giấy tờ bạn chọn không khớp với giấy tờ tải lên"), new FaultCode(((int)ErrorCode.InvestorErrorOCRDifference).ToString()), "");
            }
        }

        /// <summary>
        /// Đọc cmnd/cccd mặt trước
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task<OCRFrontIdDataNewType> ReadFrontIdDataNewType(IFormFile image)
        {
            MultipartFormDataContent formDataContent = new();
            MemoryStream memoryStream = new();

            image.CopyTo(memoryStream);
            formDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "image", image.FileName);

            HttpResponseMessage resOCRApi = await _httpClient.PostAsync(_config.ApiOCRId, formDataContent);
            string resContentString = await resOCRApi.Content.ReadAsStringAsync();
            switch (resOCRApi.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.BadRequest:
                    _logger.LogError($"Call Api OCR fail Front ID Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                    throw new FaultException(new FaultReason("Không nhận đạng được ảnh mặt trước vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRFontId).ToString()), "");
                default:
                    _logger.LogError($"Call Api OCR fail Front ID Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                    throw new FaultException(new FaultReason("Không nhận đạng được ảnh mặt trước vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRFontId).ToString()), "");
            }
            OCRResponseFrontIdNewType OCRResFrontImage = JsonSerializer.Deserialize<OCRResponseFrontIdNewType>(resContentString);
            OCRFrontIdDataNewType result;
            if (OCRResFrontImage.ErrorCode != OCRErrorCodes.NO_ERROR)
            {
                _logger.LogError($"Call Api OCR fail Front ID Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                throw new FaultException(new FaultReason("Không nhận đạng được ảnh mặt trước vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRFontId).ToString()), "");
            }
            else
            {
                _logger.LogInformation($"OCR success with response = {resContentString}");
                result = OCRResFrontImage.Data.FirstOrDefault();
            }

            string nationality = "Việt Nam";
            var expDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(result.Doe);

            if (string.IsNullOrEmpty(result.Nationality))
            {
                nationality = RecognitionUtils.GetValueStandard(result.Nationality);
            }

            result.Nationality = nationality;
            result.Doe = expDate?.ToString("dd/MM/yyyy");

            return result;
        }

        /// <summary>
        /// Đọc cmnd/cccd mặt sau
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task<OCRBackIdDataNewType> ReadBackIdDataNewType(IFormFile image)
        {
            MultipartFormDataContent formDataContent = new();
            MemoryStream memoryStream = new();

            image.CopyTo(memoryStream);
            formDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "image", image.FileName);

            HttpResponseMessage resOCRApi = await _httpClient.PostAsync(_config.ApiOCRId, formDataContent);
            string resContentString = await resOCRApi.Content.ReadAsStringAsync();
            switch (resOCRApi.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.BadRequest:
                    _logger.LogError($"Call Api OCR fail Back ID Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                    throw new FaultException(new FaultReason("Không nhận đạng được ảnh mặt sau vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRBackId).ToString()), "");
                default:
                    _logger.LogError($"Call Api OCR fail Back ID Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                    throw new FaultException(new FaultReason("Không nhận đạng được ảnh mặt sau vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRBackId).ToString()), "");
            }

            OCRResponseBackIdNewType OCRResBackImage = JsonSerializer.Deserialize<OCRResponseBackIdNewType>(resContentString);
            OCRBackIdDataNewType result;
            if (OCRResBackImage.ErrorCode != OCRErrorCodes.NO_ERROR)
            {
                _logger.LogError($"Call Api OCR fail Back ID Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                throw new FaultException(new FaultReason("Không nhận đạng được ảnh mặt sau vui long thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRBackId).ToString()), "");
            }
            else
            {
                _logger.LogInformation($"OCR Back ID success with response = {resContentString}");
                result = OCRResBackImage.Data.FirstOrDefault();
            }

            return result;
        }

        /// <summary>
        /// Đọc hộ chiếu
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task<OCRDataPassport> ReadPassport(IFormFile image)
        {
            MultipartFormDataContent formDataContent = new();
            MemoryStream memoryStream = new();

            image.CopyTo(memoryStream);
            formDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "image", image.FileName);

            HttpResponseMessage resOCRApi = await _httpClient.PostAsync(_config.ApiOCRPassport, formDataContent);
            string resContentString = await resOCRApi.Content.ReadAsStringAsync();
            switch (resOCRApi.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.BadRequest:
                    _logger.LogError($"Call Api OCR fail Passport Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                    throw new FaultException(new FaultReason("Không nhận đạng được ảnh hộ chiếu vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRPassport).ToString()), "");
                default:
                    _logger.LogError($"Call Api OCR fail Passport Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                    throw new FaultException(new FaultReason("Không nhận đạng được ảnh hộ chiếu vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRPassport).ToString()), "");
            }
            OCRResponsePassport OCRResPassport = JsonSerializer.Deserialize<OCRResponsePassport>(resContentString);
            OCRDataPassport passportData;
            if (OCRResPassport.ErrorCode != OCRErrorCodes.NO_ERROR)
            {
                _logger.LogError($"Call Api OCR fail Passport Image with status = {resOCRApi.StatusCode}, response {resContentString}");
                throw new FaultException(new FaultReason("Không nhận đạng được ảnh hộ chiếu vui lòng thử lại"), new FaultCode(((int)ErrorCode.InvestorErrorOCRPassport).ToString()), "");
            }
            else
            {
                _logger.LogInformation($"OCR Passport success with response = {resContentString}");
                passportData = OCRResPassport.Data.FirstOrDefault();
            }

            return passportData;
        }

        /// <summary>
        /// So khớp khuôn mặt
        /// </summary>
        /// <param name="idImageUrl"></param>
        /// <param name="faceImageUrl"></param>
        /// <returns></returns>
        public async Task<FaceMatchData> FaceRecognition(IFormFile idImageUrl, IFormFile faceImageUrl)
        {
            MultipartFormDataContent formDataContent = new();

            MemoryStream memoryStream = new();
            idImageUrl.CopyTo(memoryStream);
            formDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "file[]", idImageUrl.FileName);

            memoryStream = new();
            faceImageUrl.CopyTo(memoryStream);
            formDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "file[]", faceImageUrl.FileName);

            HttpResponseMessage response = await _httpClient.PostAsync(_config.ApiFaceMatch, formDataContent);
            string resContentString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new FaultException(new FaultReason($"{response.StatusCode}: {resContentString}"), new FaultCode(((int)ErrorCode.System).ToString()), "");
            }
            FaceMatchResponse faceMatchRes = JsonSerializer.Deserialize<FaceMatchResponse>(resContentString);
            FaceMatchData faceMatchData;
            if (faceMatchRes.Code == FaceMatchCodes.SUCCESS)
            {
                _logger.LogInformation($"Call Api face match success with status = {response.StatusCode}, response = {resContentString}");
                faceMatchData = JsonSerializer.Deserialize<FaceMatchData>(JsonSerializer.Serialize(faceMatchRes.Data));
            }
            else if (faceMatchRes.Code == FaceMatchCodes.NO_FACES_DETECTED)
            {
                throw new FaultException(new FaultReason("Không có khuôn mặt trong ảnh"), new FaultCode(((int)ErrorCode.InvestorFaceRecognitionNoFaceDetected).ToString()), "");
            }
            else
            {
                _logger.LogError("Call Api face match fail with response {0}", resContentString);
                throw new FaultException(new FaultReason(faceMatchRes.Data.ToString()), new FaultCode(((int)ErrorCode.InvestorErrorFaceMatch).ToString()), "");
            }

            if (!(true || faceMatchData.IsMatch))
            {
                throw new FaultException(new FaultReason("Xác thực khuôn mặt không khớp"), new FaultCode(((int)ErrorCode.InvestorFaceRecognitionNotMatch).ToString()), "");
            }

            return faceMatchData;
        }

        /// <summary>
        /// Check nhỏ hơn 18 tuổi
        /// </summary>
        /// <param name="dob"></param>
        /// <exception cref="FaultException"></exception>
        public void CheckAge(DateTime? dob)
        {
            if (dob.HasValue)
            {
                double age = (DateTime.Now - dob.Value).Days / 365.0;
                if (age < AGE_MIN)
                {
                    throw new FaultException(new FaultReason($"Khách hàng phải từ {AGE_MIN} tuổi trở lên."), new FaultCode(((int)ErrorCode.InvestorErrorAgeInvalid).ToString()), "");
                }
            }
        }

        /// <summary>
        /// Check ngày hết hạn có quá ngày hiện tại ko
        /// </summary>
        /// <param name="exp"></param>
        /// <exception cref="FaultException"></exception>
        public void CheckExp(DateTime? exp)
        {
            if (exp.HasValue && exp < DateTime.Now)
            {
                throw new FaultException(new FaultReason($"Giấy tờ đã hết hạn"), new FaultCode(((int)ErrorCode.InvestorIdExpired).ToString()), "");
            }
        }

        /// <summary>
        /// Convert ngày cấp từ chuỗi sang datetime
        /// </summary>
        /// <param name="issueDate"></param>
        /// <returns></returns>
        public DateTime? ConvertStringIssudeDateToDateTime(string issueDate)
        {
            var result = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(issueDate);
            if (result == null)
            {
                result = DateTimeUtils.FromDateStrDD_MM_YY_ToDate(issueDate);
            }
            return result;
        }

        /// <summary>
        /// +15 năm từ ngày cấp nếu ngày hết hạn null (Mặc định là 15 năm)
        /// </summary>
        /// <param name="expDate"></param>
        /// <param name="issueDate"></param>
        /// <param name="docType"></param>
        /// <returns></returns>
        public DateTime? ProccessExpDate(string expDate, DateTime? issueDate, string docType)
        {
            var result = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(expDate);
            
            if (result == null && issueDate != null)
            {
                var year = _config.CmndExpiredAddYearIfNull;
                if (docType == IDTypes.CCCD)
                {
                    year = _config.CccdExpiredAddYearIfNull;
                }
                result = issueDate.Value.AddYears(year);
            }
            return result;
        }

    }
}
