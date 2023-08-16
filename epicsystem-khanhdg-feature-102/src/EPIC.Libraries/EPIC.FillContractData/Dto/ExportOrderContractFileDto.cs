using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.FillContractData.Dto
{
    public class ExportOrderContractFileDto
    {
        /// <summary>
        /// File mẫu - KHÔNG CẦN TRUYỀN VÀO API
        /// </summary>
        public string FileTempUrl { get; set; }
        /// <summary>
        /// File mẫu PDF - KHÔNG CẦN TRUYỀN VÀO API
        /// </summary>
        public string FileTempPdfUrl { get; set; }
        /// <summary>
        /// File Scan - KHÔNG CẦN TRUYỀN VÀO API
        /// </summary>
        public string FileScanUrl { get; set; }
        /// <summary>
        /// File đã ký - KHÔNG CẦN TRUYỀN VÀO API
        /// </summary>
        public string FileSignatureUrl { get; set; }
        /// <summary>
        /// Loại file hđ
        /// </summary>
        public string ContractType { get; set; }
    }
}
