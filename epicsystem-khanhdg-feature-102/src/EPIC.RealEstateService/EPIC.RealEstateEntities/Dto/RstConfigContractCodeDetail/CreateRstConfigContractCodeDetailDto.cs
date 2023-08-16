using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail
{
    public class CreateRstConfigContractCodeDetailDto
    {
        public int Id { get; set; }
        public int ConfigContractCodeId { get; set; }
        public int SortOrder { get; set; }

        private string _key;
        [StringRange(AllowableValues = new string[] { ConfigContractCode.ORDER_ID, ConfigContractCode.POLICY_CODE, ConfigContractCode.POLICY_NAME, ConfigContractCode.PRODUCT_CODE, ConfigContractCode.PRODUCT_NAME, ConfigContractCode.FIX_TEXT,
                                                     ConfigContractCode.DATE_DD, ConfigContractCode.DATE_MM, ConfigContractCode.DATE_YY, ConfigContractCode.DATE_YYYY, ConfigContractCode.DATE_DD_MM_YYYY,
                                                      ConfigContractCode.SHORT_NAME, ConfigContractCode.PAYMENT_FULL_DATE, ConfigContractCode.INVEST_DATE, ConfigContractCode.PRODUCT_TYPE, ConfigContractCode.BUY_DATE, ConfigContractCode.ORDER_ID_PREFIX_0,
                                                      ConfigContractCode.RST_PRODUCT_ITEM_CODE, })]
        public string Key
        {
            get => _key;
            set => _key = value?.Trim();
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => _value = value?.Trim();
        }
    }
}
