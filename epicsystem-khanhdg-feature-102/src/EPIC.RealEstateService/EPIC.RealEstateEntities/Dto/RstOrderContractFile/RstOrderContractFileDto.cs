using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderContractFile
{
    public class RstOrderContractFileDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Id sổ lệnh
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Id mẫu hợp đồng
        /// </summary>
        public int ContractTempId { get; set; }
        /// <summary>
        /// Tên file
        /// </summary>
        public string ContractTemplateTempName { get; set; }
        /// <summary>
        /// File hđ .docx
        /// </summary>
        public string FileTempUrl { get; set; }
        /// <summary>
        /// File hđ .pdf
        /// </summary>
        public string FileTempPdfUrl { get; set; }
        public string FileSignaturePdfUrl { get; set; }
        public string FileScanUrl { get; set; }
    }
}
