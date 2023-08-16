using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Loại chính sách ưu đãi hiện thị trên App : 
    /// 1: Từ chủ đầu tư ( Đối tác cài chính sách ưu đãi vào thông tin căn hộ)
    /// 2: Từ đại lý (Cài trong mở bán)
    /// </summary>
    public class AppRstPolicyTypes
    {
        public const int FROM_PARTNER = 1;
        public const int FROM_TRADING_PROVIDER = 2;
    }
}
