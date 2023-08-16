using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities
{
    public class ModuleValidateInfo
    {
        [Column(Name = "VALIDATE_ID")]
        public int ValidateId { get; set; }
        [Column(Name = "MODID")]
        public string ModId { get; set; }
        [Column(Name = "FIELD_NAME")]
        public string FieldName { get; set; }
        [Column(Name = "DATA_TYPE")]
        public string DataType { get; set; }
        [Column(Name = "VALIDATE_TYPE")]
        public string ValidateType { get; set; }
        [Column(Name = "VALIDATE_CONTENT")]
        public string ValidateContent { get; set; }
        [Column(Name = "ERROR_CODE")]
        public int ErrorCode { get; set; }
        [Column(Name = "ERROR_MESSAGE")]
        public string ErrorMessage { get; set; }
    }
}
