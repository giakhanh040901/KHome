using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace EPIC.ToolsConvert.Controllers
{
    [RoutePrefix("api/tools-windows")]
    public class ConvertController : ApiController
    {
        [HttpPost]
        [Route("docx-to-pdf")]
        public async Task<HttpResponseMessage> ConvertDocxToPdfAsync()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                var wordFile = $"{Path.GetTempFileName()}.docx";
                string fileName = "";
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        fileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                        postedFile.SaveAs(wordFile);
                    }
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Vui lòng tải file word lên.")
                    };
                }

                string pdfFile = $"{Path.GetTempFileName()}.pdf";
                var appWord = new Application();
                if (appWord.Documents != null)
                {
                    //yourDoc is your word document
                    var wordDocument = appWord.Documents.Open(wordFile);
                    if (wordDocument != null)
                    {
                        wordDocument.ExportAsFixedFormat(pdfFile, WdExportFormat.wdExportFormatPDF);
                        wordDocument.Close();
                    }
                }
                appWord.Quit();
                Marshal.FinalReleaseComObject(appWord);
                if (File.Exists(wordFile))
                {
                    File.Delete(wordFile);
                }

                byte[] fileByte;
                using (FileStream stream = File.Open(pdfFile, FileMode.Open))
                {
                    fileByte = new byte[stream.Length];
                    await stream.ReadAsync(fileByte, 0, (int)stream.Length);
                }

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileByte) //File.ReadAllBytes(pdfFile)
                };
                if (File.Exists(pdfFile))
                {
                    File.Delete(pdfFile);
                }
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = $"{fileName}.pdf"
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Có lỗi xảy ra khi convert docx to pdf: {ex.Message}")
                };
            }
        }

        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        [HttpGet]
        [Route("healthy")]
        public HttpResponseMessage GetHealth()
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("healthy")
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"not healthy Exception: {ex.Message}")
                };
            }
        }
    }
}
