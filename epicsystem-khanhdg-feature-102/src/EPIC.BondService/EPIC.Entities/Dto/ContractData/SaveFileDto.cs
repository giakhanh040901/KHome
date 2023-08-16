using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractData
{
    public class SaveFileDto
    {
        public string FileTempUrl { get; set; }
        /// <summary>
        /// File pdf không có con dấu
        /// </summary>
        public string FileSignatureUrl { get; set; }
        /// <summary>
        /// File pdf có con dấu
        /// </summary>
        public string FileSignatureStampUrl { get; set; }
        /// <summary>
        /// File chuẩn bị để xoá
        /// </summary>
        public string FilePathToBeDeleted { get; set; }
        public string FileName { get; set; }
        public int PageSign { get; set; }

        /// <summary>
        /// Các task xử lý file
        /// </summary>
        public Task SaveFileTasks { get; set; }
    }
}
