using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConfigModel
{
    public class RecognitionApiConfiguration
    {
        public string ApiBaseAddress { get; set; }
        public string ApiOCRId { get; set; }
        public string ApiOCRPassport { get; set; }
        public string ApiFaceMatch { get; set; }
        public string ApiKey { get; set; }
        public double FaceSimilarity { get; set; }
        public int CmndExpiredAddYearIfNull { get; set; }
        public int CccdExpiredAddYearIfNull { get; set; }
    }
}
