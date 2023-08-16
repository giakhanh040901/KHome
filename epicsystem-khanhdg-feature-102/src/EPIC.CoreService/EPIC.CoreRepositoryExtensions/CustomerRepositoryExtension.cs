using EPIC.DataAccess.Base;
using EPIC.Utils;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositoryExtensions
{
    /// <summary>
    /// Mở rộng cho các repository để lấy thông tin khách hàng
    /// </summary>
    public static class CustomerRepositoryExtension
    {
        /// <summary>
        /// Lấy user modify theo cifcode, nếu là khách cá nhân thì lấy phone, nếu là khách doanh nghiệp thì lấy tên viết tắt
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        public static string GetUserByCifCode(this EpicSchemaDbContext dbContext, string cifCode)
        {
            string modifiedBy = null;
            var cifCodeFind = dbContext.CifCodes.FirstOrDefault(c => c.CifCode == cifCode && c.Deleted == YesNo.NO);
            if (cifCodeFind != null)
            {
                if (cifCodeFind.InvestorId != null)
                {
                    modifiedBy = dbContext.Investors.FirstOrDefault(i => i.InvestorId == cifCodeFind.InvestorId)?.Phone;
                }
                else
                {
                    modifiedBy = dbContext.BusinessCustomers.FirstOrDefault(b => b.BusinessCustomerId == cifCodeFind.BusinessCustomerId)?.ShortName?.Truncate(50);
                }
            }
            return modifiedBy;
        }
    }
}
