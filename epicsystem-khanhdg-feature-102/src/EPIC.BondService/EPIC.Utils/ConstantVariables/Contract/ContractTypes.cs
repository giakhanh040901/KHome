using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Contract
{
    public static class ContractTypes
    {
        /// <summary>
        /// Loại hợp đồng đặt lệnh
        /// </summary>
        public const int DAT_LENH = 1;
        /// <summary>
        /// Loại hợp đồng rút tiền
        /// </summary>
        public const int RUT_TIEN = 2;
        /// <summary>
        /// Loại hợp đồng tái tục gốc
        /// </summary>
        public const int TAI_TUC_GOC = 3;
        /// <summary>
        /// Loại hợp đồng Rút tiền 
        /// </summary>
        public const int RUT_TIEN_APP = 4;

        /// <summary>
        /// Loại hợp đồng tái tục gốc + lợi nhuận
        /// </summary>
        public const int TAI_TUC_GOC_VA_LOI_NHUAN = 5;
    }
}
