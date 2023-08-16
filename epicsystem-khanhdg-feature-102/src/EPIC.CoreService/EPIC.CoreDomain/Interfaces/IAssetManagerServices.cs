using EPIC.Entities.Dto.ManagerInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IAssetManagerServices
    {
        /// <summary>
        /// Thông tin tài sản của khách hàng cá nhân màn Tổng quan
        /// </summary>
        /// <returns></returns>
        AssetManagerDto AssetManagerInvestor();

        /// <summary>
        /// Thông tin tài sản đầu tư của khách hàng cá nhân màn Đầu tư
        /// </summary>
        /// <returns></returns>
        InvestManagerDto InvestManager();
    }
}
