using EPIC.SIGN.CORE;
using EPIC.SIGN.HSM;
using EPIC.SIGN.PDF;
using Newtonsoft.Json;
using System;
using System.IO;

namespace EPIC.SIGN.HSN
{
    public class PDFSigner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private readonly string inputPDF = "";
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private readonly string outputPDF = "";
        private readonly string appid;
        private readonly string secret;
        private readonly string region;
        private readonly string server;
        private readonly IByteSigner _byteSigner;

        public PDFSigner(string server, string appid, string secret, string region)
        {
            region ??= "demo";
            this.appid = appid;
            this.secret = secret;
            this.server = server;
            _byteSigner = new ByteSigner(server, appid, secret, region);
        }

        public ApiResp Verify(byte[] input)
        {
            var signClient = new SignClient(server, appid, secret, "sign", region);

            string base64pdf = Convert.ToBase64String(input);
            var responseString = signClient.UploadString("/api/v2/pdf/verify", "POST", base64pdf);
            var verifyResult = JsonConvert.DeserializeObject<ApiResp>(responseString);
            if (verifyResult.status == 0)
            {
                return verifyResult;
            }

            return null;
        }


        public byte[] SignBase64(string input, string sHash, int typeSignature, string base64Image, string textOut, string signatureName, int pageSign = 1,
            int xPoint = 100, int yPoint = 20, int width = 200, int height = 50)
        {
            var signClient = new SignClient(server, appid, secret, "sign", "hn1");

            var pdfOriginalData = new
            {
                base64image = base64Image,
                hashalg = sHash,
                textout = textOut,
                pagesign = pageSign,
                signaturename = signatureName,
                xpoint = xPoint,
                ypoint = yPoint,
                width = width,
                height = height,
                typesignature = typeSignature,
                base64pdf = input
            };
            string pdfStringJson = JsonConvert.SerializeObject(pdfOriginalData);
            var responseString = signClient.UploadString("/api/v2/pdf/sign/originaldata", "POST", pdfStringJson);
            var dataSigned = JsonConvert.DeserializeObject<ApiResp>(responseString);
            if (dataSigned.status == 0)
            {
                var bytesout = Convert.FromBase64String(dataSigned.obj.ToString());
                return bytesout;
            }

            return null;
        }


        public byte[] Sign(byte[] input, string sHash, int typeSignature, string base64Image, string textOut, string signatureName, int pageSign = 1,
           int xPoint = 100, int yPoint = 20, int width = 200, int height = 50)
        {
            var signClient = new SignClient(server, appid, secret, "sign", "hn1");

            string base64pdf = Convert.ToBase64String(input);


            var pdfOriginalData = new
            {
                base64image = base64Image,
                hashalg = sHash,
                textout = textOut,
                pagesign = pageSign,
                signaturename = signatureName,
                xpoint = xPoint,
                ypoint = yPoint,
                width = width,
                height = height,
                typesignature = typeSignature,
                base64pdf = base64pdf
            };
            string pdfStringJson = JsonConvert.SerializeObject(pdfOriginalData);
            try
            {
                var responseString = signClient.UploadString("/api/v2/pdf/sign/originaldata", "POST", pdfStringJson);
                if (!string.IsNullOrEmpty(responseString))
                {
                    var dataSigned = JsonConvert.DeserializeObject<ApiResp>(responseString);
                    if (dataSigned.status == 0)
                    {
                        var bytesout = Convert.FromBase64String(dataSigned.obj.ToString());
                        if (bytesout == null)
                        {
                            throw new Exception(responseString);
                        }
                        return bytesout;
                    }
                    else
                    {
                        throw new Exception(responseString);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

    }
}
