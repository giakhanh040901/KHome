using EPIC.DataAccess.Base;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RepositoryExtensions
{
    /// <summary>
    /// Mở rộng cho các repository để lấy thông tin lệnh
    /// </summary>
    public static class OrderRepositoryExtension
    {
        /// <summary>
        /// Random mã giao nhận hợp đồng
        /// </summary>
        /// <returns></returns>
        public static string FuncVerifyCodeGenerate(this EpicSchemaDbContext dbContext)
        {
            while (true)
            {
                var numberRandom = RandomNumberUtils.RandomNumber(10);
                var checkDeliveryCodeOrder = dbContext.GarnerOrders.Where(o => o.DeliveryCode == numberRandom);
                var checkDeliveryCodeOrderInv = dbContext.InvOrders.Where(o => o.DeliveryCode == numberRandom);
                var checkDeliveryCodeOrderBond = dbContext.BondOrders.Where(o => o.DeliveryCode == numberRandom);
                if (!checkDeliveryCodeOrder.Any() && !checkDeliveryCodeOrderInv.Any() && !checkDeliveryCodeOrderBond.Any())
                {
                    return numberRandom;
                }
            }
        }
    }
}
