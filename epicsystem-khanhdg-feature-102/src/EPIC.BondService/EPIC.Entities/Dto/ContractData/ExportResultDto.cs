using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractData
{
    public class ExportResultDto
    {
        public byte[] fileData { get; set; }
        public string filePath { get; set; }
        public string fileDownloadName { get; set; }
    }

    public class ResultFileDto
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
    }
}
