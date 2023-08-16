using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderContractFile
{
    public class RstUpdateOrderContractFileDto
    {
        public long Id { get; set; }
        public string FileScanUrl { get; set; }
        public string FileSignatureUrl { get; set; }
        public string FileTempUrl { get; set; }
    }
}
