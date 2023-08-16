using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.OpenXmlLibrary;
using EPIC.OpenXmlLibrary.Dtos;
using Newtonsoft.Json;
using QRCoder;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using A = DocumentFormat.OpenXml.Drawing;
using A14 = DocumentFormat.OpenXml.Office2010.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using W = DocumentFormat.OpenXml.Wordprocessing;

namespace ToolsUtils
{
    public static class OpenXmlExtensions
    {
        /// <summary>
        /// Thêm data validation<br/>
        /// <paramref name="flatList"/> danh sách các phần tử cách nhau bằng dấu ","<br/>
        /// <paramref name="sequenceOfReferences"/> dải ô vd A:A hoặc A1:A1048576
        /// </summary>
        public static void AddDataValidationList(this SheetData sheetData, string flatList, string sequenceOfReferences)
        {
            DataValidation dataValidation = new DataValidation
            {
                Type = DataValidationValues.List,
                AllowBlank = true,

                //Use A:A or A1:A1048576 to select the entire column A
                //1048576 (2^20) is the max row number since Excel 2007.
                SequenceOfReferences = new ListValue<StringValue>() { InnerText = sequenceOfReferences },

                //Set the formula to the list of dropdown values. Escape the double quotes.
                Formula1 = new Formula1("\"" + flatList + "\"")
            };

            sheetData.AddDataValidation(dataValidation);
        }

        public static void AddDataValidation(this SheetData sheetData, DataValidation dataValidation)
        {
            //Check if there are any other DataValidations already in the worksheet
            DataValidations dvs = sheetData.GetFirstChild<DataValidations>();
            if (dvs != null)
            {
                //If you already have existing validation for column A, you may need to Remove()
                //or Replace() the current validation to get the new validation to show.          

                //Add the new DataValidation to the list of DataValidations
                dvs.Count++;
                dvs.Append(dataValidation);
            }
            else
            {
                DataValidations newDVs = new DataValidations();
                newDVs.Append(dataValidation);
                newDVs.Count = 1;

                //Append the validation to the DocumentFormat.OpenXml.SpreadSheet.Worksheet variable
                sheetData.Append(newDVs);
            }
        }
    }

    internal class Program
    {
        static void GenerateListLinkLogoBank()
        {
            string xlsxFilePath = "D:/work/STE/EPIC-BOUND/EPIC.BondAPI/src/EPIC.ImageService/EPIC.ImageAPI/App_Data_Default/logo-bank/File-name.xlsx";
            //string xlsxFilePath = @"C:\Users\Vanmi\Downloads\BOQ hợp đồng..xlsx";

            string sqlResult = "";
            string folder = "logo-bank";
            string sqlTemplate = "INSERT INTO EP_CORE_BANK (BANK_ID, LOGO, BANK_NAME, FULL_BANK_NAME) VALUES ({0}, '{1}', '{2}', '{3}');\n";
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(xlsxFilePath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                //các trường lưu text ở đây
                IEnumerable<SharedStringItem> sharedStringItems = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>();
                //var test = workbookPart.WorksheetParts.ToList();
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                var rows = sheetData.Elements<Row>().ToList();
                for (int index = 1; index < rows.Count; index++)
                {
                    var cells = rows[index].Elements<Cell>().ToList();

                    string bankName = null;
                    if (int.TryParse(cells[0].InnerText, out int idShortName))
                    {
                        bankName = sharedStringItems.ElementAt(idShortName)?.InnerText?.Trim();
                    }

                    string bankFullName = null;
                    if (int.TryParse(cells[1].InnerText, out int idFullName))
                    {
                        bankFullName = sharedStringItems.ElementAt(idFullName)?.InnerText?.Trim();
                    }

                    string logo = $"api/file/get?folder={HttpUtility.UrlEncode(folder)}&file={HttpUtility.UrlEncode(bankName)}.svg";

                    sqlResult += string.Format(sqlTemplate, index, logo, bankName, bankFullName);
                }
            }
        }

        static void OpenXmlDataValidation()
        {
            string xlsxFilePath = Path.Combine(Environment.CurrentDirectory, "TemplateExcel", "Temp_Update_BDS.xlsx");
            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(xlsxFilePath, true);
            WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
            WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            string flatList = "FirstChoice,SecondChoice,ThirdChoice";
            sheetData.AddDataValidationList(flatList, "A:A");
        }

        static void GeneratePrivatePublicKeyPair()
        {
            //RSA rsa = RSA.Create();
            //byte[] pulicKey = rsa.ExportRSAPublicKey();
            //byte[] privateKey = rsa.ExportRSAPrivateKey();

            //string pulicKeyStr = TextEncodings.Base64Url.Encode(pulicKey);
            //string privateKeyStr = TextEncodings.Base64Url.Encode(privateKey);

            RSACryptoServiceProvider provider = new(1024);
            byte[] pulicKey = provider.ExportRSAPublicKey();
        }

        static void GemboxTest()
        {
            // If using Professional version, put your serial key below.
            //ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // In order to convert Word to PDF, we just need to:
            //   1. Load DOC or DOCX file into DocumentModel object.
            //   2. Save DocumentModel object to PDF file.


            //DocumentModel document = DocumentModel.Load(@"C:\Users\Vanmi\Downloads\ks_bond_HDSD_Cài_đặt_và_xử_lý_giao_dịch_mua_bán_trái_phiếu.docx");
            //document.Save("Output.pdf");
        }

        private static uint GenerateUniqueId(WordprocessingDocument document)
        {
            // Lấy tất cả các Id đã sử dụng trong tệp DOCX
            var allNonVisualId = document.MainDocumentPart.Document.Descendants<PIC.NonVisualDrawingProperties>()
                                                .Where(o => o.Id.HasValue)
                                                .Select(o => o.Id.Value);

            var allDocPropertyId = document.MainDocumentPart.Document.Descendants<DW.DocProperties>()
                                                .Where(o => o.Id.HasValue)
                                                .Select(o => o.Id.Value);

            var allIds = allNonVisualId.Concat(allDocPropertyId);

            // Sinh một Id mới và kiểm tra xem nó đã tồn tại hay chưa
            Random random = new();
            uint newId = (uint)random.Next(1, int.MaxValue);
            while (allIds.Contains(newId))
            {
                newId = (uint)random.Next(1, int.MaxValue);
            }
            return newId;
        }

        //private static W.Drawing AddImageToBody(string relationshipId)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="relationshipId"></param>
        /// <param name="widthInInch"></param>
        /// <param name="heightInInch"></param>
        /// <returns></returns>
        private static W.Drawing AddImage(WordprocessingDocument wordDoc, string relationshipId, double widthInInch, double heightInInch)
        {
            var emuPerInch = 914400;
            long widthInEmu = (long)(emuPerInch * widthInInch);
            long heightInEmu = (long)(emuPerInch * heightInInch);

            //long widthInEmu = 792000L;
            //long heightInEmu = 792000L;

            var graphicFrameLocks = new A.GraphicFrameLocks() { NoChangeAspect = true };
            graphicFrameLocks.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            var useLocalDpi = new A14.UseLocalDpi() { Val = false };
            useLocalDpi.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            uint nvId = GenerateUniqueId(wordDoc);
            uint docId = GenerateUniqueId(wordDoc);

            var picture = new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties()
                                        {
                                            //Id = (UInt32Value)5U,
                                            Id = nvId,
                                            Name = $"Picture 1"
                                        },
                                        new PIC.NonVisualPictureDrawingProperties(new A.PictureLocks()
                                        {
                                            NoChangeAspect = true,
                                            NoChangeArrowheads = true
                                        })),
                                    new PIC.BlipFill(
                                        new A.Blip(
                                            new A.BlipExtensionList(
                                                new A.BlipExtension(useLocalDpi)
                                                {
                                                    Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                }))
                                        {
                                            Embed = relationshipId,
                                            CompressionState = A.BlipCompressionValues.Print
                                        },
                                        new A.Stretch(new A.FillRectangle())),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset() { X = 0L, Y = 0L },
                                            new A.Extents() { Cx = widthInEmu, Cy = heightInEmu }),
                                            new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle },
                                            new A.NoFill(),
                                            new A.Outline(new A.NoFill())
                                    )
                                    {
                                        BlackWhiteMode = A.BlackWhiteModeValues.Auto
                                    });
            picture.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            var graphic = new A.Graphic(new A.GraphicData(picture)
            {
                Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
            });
            graphic.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            var element = new W.Drawing(
                             new DW.Inline(
                                 new DW.Extent()
                                 {
                                     Cx = widthInEmu,
                                     Cy = heightInEmu,
                                 },
                                 new DW.EffectExtent()
                                 {
                                     LeftEdge = 0L,
                                     TopEdge = 0L,
                                     RightEdge = 0L,
                                     BottomEdge = 0L
                                 },
                                 new DW.DocProperties()
                                 {
                                     Id = docId,
                                     Name = $"Picture {docId}"
                                 },
                                 new DW.NonVisualGraphicFrameDrawingProperties(graphicFrameLocks),
                                 graphic
                             )
                             {
                                 DistanceFromTop = (UInt32Value)0U,
                                 DistanceFromBottom = (UInt32Value)0U,
                                 DistanceFromLeft = (UInt32Value)0U,
                                 DistanceFromRight = (UInt32Value)0U,
                             });
            return element;
        }

        static async Task GenQR()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("EV-123412ABC12312", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var image = qrCode.GetGraphic(20);
            using FileStream fileStreamQrCode = new("./test.png", FileMode.OpenOrCreate);
            {
                image.SaveAsPng(fileStreamQrCode);
                fileStreamQrCode.Close();
            }

            // Search for text holder
            FileStream fileTemplate = new("./TemplateWord/MauVeEvent.docx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
            {
                var mainPart = wordDoc.MainDocumentPart;

                var test = mainPart.Document.Body;
                var test2 = mainPart.Document.Body.FirstOrDefault(e => e.InnerText == "{{QrCode}}");
                var textElements = mainPart.Document.Body.Descendants<W.Text>().ToList();

                var textPlaceHolder = mainPart.Document.Body.Descendants<W.Text>()
                    .FirstOrDefault(e => e.Text == "{{QrCode}}") ?? throw new Exception("Không có biến qr code");

                var text2 = mainPart.Document.Body.Descendants<W.Text>()
                    .FirstOrDefault(e => e.Text == "{{Test}}") ?? throw new Exception("Không có biến tên vé");

                //text2.Text = "Work qr";

                var images = mainPart.Document.Descendants<PIC.Picture>().ToList();
                var testImageParts = mainPart.ImageParts.ToList();

                // Define the reference of the image.
                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
                using (var stream = new FileStream("./test.png", FileMode.Open))
                {
                    stream.Position = 0;
                    imagePart.FeedData(stream);
                }
                string relationShipId = mainPart.GetIdOfPart(imagePart);

                var parent = textPlaceHolder.Parent;
                if (parent is not W.Run)  // Parent should be a run element.
                {
                    Console.Out.WriteLine("Parent is not run");
                }
                else
                {
                    //mainPart.Document.Body.Append(new W.Paragraph(new W.ParagraphProperties(new W.NoProof()), new W.Run(AddImage(relationShipId))));
                    parent.AppendChild(AddImage(wordDoc, relationShipId, 1, 1));
                    textPlaceHolder.Remove();
                }
                mainPart.Document.Save();
            }

            byte[] byteArrayResult = ms.ToArray();
            File.WriteAllBytes($"result{DateTime.Now.ToFileTime()}.docx", byteArrayResult);
        }

        static async Task<string> UploadFile(string urlImage, string fileName)
        {
            //if (!File.Exists(filePath))
            //{
            //    //log lỗi tại đây
            //    throw new Exception($"File \"{filePath}\" không tồn tại");
            //}
            string accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjhGREUxRkU1QUIwRUY0OUExRTMyRURFOTBDOTI1Nzg1IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2ODk3NTIyMDAsImV4cCI6MTY4OTc1OTQwMCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAxIiwiY2xpZW50X2lkIjoiY2xpZW50MSIsInN1YiI6IjY3NyIsImF1dGhfdGltZSI6MTY4OTc1MjIwMCwiaWRwIjoibG9jYWwiLCJpcF9hZGRyZXNzX2xvZ2luIjoiMTAuMjEyLjEzNC4xMDgiLCJ1c2VyX3R5cGUiOiJSRSIsInVzZXJuYW1lIjoibWluaGx2IiwiZGlzcGxheV9uYW1lIjoiTMOqIFbEg24gTWluaCIsImVtYWlsIjoidXNlckBleGFtcGxlLmNvbSIsImp0aSI6IjlDQjhEMjlGMjcxMkQzMUJDNUE1MzE4QTJGN0I2NzhEIiwiaWF0IjoxNjg5NzUyMjAwLCJzY29wZSI6WyJCb25kQVBJIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdfQ.lJCYFUDBw1pski2lLQSy0Q6M3mZZy45F-MyZGwrKc6SrdxHdYHPT63hRxG_HoPy77tpFWyk7yupqdubVUcl-88kpkBunAPw8USHQY6yEsGnh04vTgA9gX0gu0Ij3MNY7GiM3_WtABUsk8R0epjTmM7iKhS-1jllfHmV1tCzr5dYJNZxuOVLFYXoQhnBSA5a1-2KC-A3kuQU87UFDNIg1zgiGY0M4RYQlr_IGTpNS_LdXlIumt-zsXWtVrK4kVUArZ2iwjp80qrAcqCUOVQDVEOQDcYtBePkWe1Sy_IYMSsXvbtFr3o-WzzTwNTFX7Xh7eyvbY52mv3M4Jm3vJXocwA";
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5006"),
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var multipartFormContent = new MultipartFormDataContent();

            WebClient client = new();
            Stream imageStream = client.OpenRead(urlImage);

            //Load the file and set the file's Content-Type header
            var fileStreamContent = new StreamContent(imageStream);

            multipartFormContent.Add(fileStreamContent, name: "file", fileName: Path.GetFileName(fileName));

            var response = await httpClient.PostAsync($"/api/file/upload?folder=investor", multipartFormContent);
            var resStr = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"tải file bị lỗi: urlImage = {urlImage}, statusCode = {response.StatusCode}, resBody = {resStr}");
            }

            dynamic resBody = JsonConvert.DeserializeObject(resStr);
            return resBody.data;
        }

        static async Task TestOpenXml()
        {
            byte[] byteArray = File.ReadAllBytes("./TemplateWord/MauVeEvent.docx");

            using MemoryStream memoryStreamFileThuong = new();
            await memoryStreamFileThuong.WriteAsync(byteArray, 0, byteArray.Length);

            using (var wordDoc = WordprocessingDocument.Open(memoryStreamFileThuong, true))
            {
                var mainPart = wordDoc.MainDocumentPart;
                string docText = null;

                using (StreamReader sr = new(mainPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                docText = docText.Replace("{{Test}}", HttpUtility.HtmlEncode("Work"));
                using (StreamWriter sw = new(mainPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                try
                {
                    mainPart.Document.Save();
                }
                catch (XmlException)
                {

                }

                //if (wordDoc.ExtendedFilePropertiesPart.Properties.Pages?.Text != null)
                //{
                //    int pageCount = int.Parse(wordDoc.ExtendedFilePropertiesPart.Properties.Pages.Text.Trim());
                //}
                //else
                //{
                //}
                wordDoc.Close();
            }


            byte[] byteArrayThuong = memoryStreamFileThuong.ToArray();
            await File.WriteAllBytesAsync($"result{DateTime.Now.ToFileTime()}.docx", byteArrayThuong);
        }

        static void TestFill()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("EV-123412ABC12312", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var image = qrCode.GetGraphic(20);
            using MemoryStream fileStreamQrCode = new();
            image.SaveAsPng(fileStreamQrCode);

            var listReplace = new List<InputReplaceDto>()
            {
                new InputReplaceDto
                {
                    FindText = "{{QrCode}}",
                    ReplaceImage = fileStreamQrCode,
                    ReplaceImageWidth = 1,
                    ReplaceImageHeight = 1,
                    ReplaceImageExtension = ".png"
                },
                new InputReplaceDto
                {
                    FindText = "{{Test}}",
                    ReplaceText = "Work"
                }
            };
            FileStream fileTemplate = new("./TemplateWord/MauVeEvent.docx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true))
            {
                wordDoc.ReplaceTextPlaceHolder(listReplace);
            }
            File.WriteAllBytes($"result{DateTime.Now.ToFileTime()}.docx", ms.ToArray());
        }

        static async Task Main(string[] args)
        {
            //GenerateListLinkLogoBank();
            //GeneratePrivatePublicKeyPair();

            //RSACSPSample.Test();
            //OpenXmlDataValidation();

            //string imageUrlPath = await UploadFile("https://firebasestorage.googleapis.com/v0/b/sunshine-app-production.appspot.com/o/housing%2Fcards%2Fone-face%2F2022-3-3%2FCCCD%20Nguy%E1%BB%85n%20Th%C3%BAy%20Ng%C3%A0%201.jpg?alt=media&token=13be7c20-1450-4110-8677-0886c746e05b", "abc.jpg");
            //await GenQR();
            //await TestOpenXml();

            TestFill();
        }
    }
}
