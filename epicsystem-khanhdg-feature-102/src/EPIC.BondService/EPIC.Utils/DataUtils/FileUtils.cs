using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Web;

namespace EPIC.Utils.DataUtils
{
    public static class FileUtils
    {
        private static Dictionary<string, string> GetParams(string uri)
        {
            if (string.IsNullOrEmpty(uri)) return new();

            var matches = Regex.Matches(uri, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            var dic = matches.Cast<Match>().ToDictionary(
                m => Uri.UnescapeDataString(m.Groups[2].Value),
                m => Uri.UnescapeDataString(m.Groups[3].Value)
            );
            foreach (var d in dic)
            {
                dic[d.Key] = HttpUtility.UrlDecode(d.Value);
            }
            return dic;
        }

        /// <summary>
        /// Chuyển url path trong db thành path vật lý
        /// </summary>
        /// <param name="pathDb"></param>
        /// <param name="initPath"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public static PhysicalPathResultDto GetPhysicalPath(string pathDb, string initPath)
        {
            var path = GetParams(pathDb);
            var folder = path["folder"];
            var fileName = path["file"];

            var fullPath = Path.Combine(initPath, folder, fileName);
            return new PhysicalPathResultDto
            {
                FileName = fileName,
                Folder = folder,
                FullPath = fullPath
            };
        }

        /// <summary>
        /// Chuyển url path trong db thành path vật lý nhưng không kiểm tra tồn tại
        /// </summary>
        /// <param name="pathDb"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public static PhysicalPathResultDto GetPhysicalPathNoCheckExists(string pathDb, string filePath)
        {
            var path = GetParams(pathDb);
            var folder = path["folder"];
            var fileName = path["file"];

            var fullPath = Path.Combine(filePath, folder, fileName);

            return new PhysicalPathResultDto
            {
                FileName = fileName,
                Folder = folder,
                FullPath = fullPath
            };
        }

        public static void RemoveOrderContractFile(OrderContractFileRemoveDto orderContractFile, string filePath)
        {
            if (orderContractFile.FileTempUrl != null)
            {
                var fileResult = GetPhysicalPathNoCheckExists(orderContractFile.FileTempUrl, filePath);
                if (File.Exists(fileResult.FullPath))
                {
                    File.Delete(fileResult.FullPath);
                }
            }

            if (orderContractFile.FileTempPdfUrl != null)
            {
                var fileResult = GetPhysicalPathNoCheckExists(orderContractFile.FileTempPdfUrl, filePath);
                if (File.Exists(fileResult.FullPath))
                {
                    File.Delete(fileResult.FullPath);
                }
            }

            if (orderContractFile.FileScanUrl != null)
            {
                var fileResult = GetPhysicalPathNoCheckExists(orderContractFile.FileScanUrl, filePath);
                if (File.Exists(fileResult.FullPath))
                {
                    File.Delete(fileResult.FullPath);
                }
            }

            if (orderContractFile.FileSignatureUrl != null)
            {
                var fileResult = GetPhysicalPathNoCheckExists(orderContractFile.FileSignatureUrl, filePath);
                if (File.Exists(fileResult.FullPath))
                {
                    File.Delete(fileResult.FullPath);
                }

                //xóa file pdf có con dấu
                var FullSignPath = fileResult.FullPath.Replace(".pdf", "-Sign.pdf");
                if (File.Exists(FullSignPath))
                {
                    File.Delete(FullSignPath);
                }
            }
        }
    }

    public class PhysicalPathResultDto
    {
        /// <summary>
        /// Đường dẫn tới file
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// Tên file
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Tên thư mục
        /// </summary>
        public string Folder { get; set; }
    }

    public class OrderContractFileRemoveDto
    {
        public string FileTempUrl { get; set; }
        public string FileTempPdfUrl { get; set; }
        public string FileSignatureUrl { get; set; }
        public string FileScanUrl { get; set; }
    }

}
