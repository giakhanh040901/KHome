using EPIC.DataAccess;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Sale;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class InvestorSaleRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_INVESTOR_SALE_LIST_TRADIN = "PKG_INVESTOR_SALE.PROC_INVESTOR_SALE_LIST_TRADIN";
        private const string PROC_GET_LIST_SALE = "PKG_INVESTOR_SALE.PROC_GET_LIST_SALE";
        private const string PROC_BUSINESS_LIST_TRADING = "PKG_INVESTOR_SALE.PROC_BUSINESS_LIST_TRADING";

        public InvestorSaleRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        /// <summary>
        /// Danh sách đại lý theo sale đang chọn là mặc định
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<AppListTradingProviderDto> InvestorSaleListTrading(int investorId)
        {
            var result = _oracleHelper.ExecuteProcedure<AppListTradingProviderDto>(PROC_INVESTOR_SALE_LIST_TRADIN, new
            {
                pv_INVESTOR_ID = investorId,
            }).ToList();
            return result;
        }

        /// <summary>
        /// danh sách sale của investor
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<InvestorSale> GetListSaleByInvestorId(int investorId)
        {
            var rslt = _oracleHelper.ExecuteProcedure<InvestorSale>(PROC_GET_LIST_SALE, new
            {
                pv_INVESTOR_ID = investorId
            });

            return rslt.ToList();
        }

        /// <summary>
        /// Danh sách đại lý sơ cấp là đại lý được bán hộ của đại lý mà investor đang tham gia
        /// Trường hợp INVESTOR chọn SALE là Sale mặc định trong bảng INVESTOR_SALE : Nếu không có mặc định thì lấy một quan hệ mới nhất
        /// Lấy các Đại lý mà Sale đang thuộc
        /// Lấy các Đại lý mà Đại lý của sale đang bán bộ
        /// </summary>
        /// <param name="investorId">Id của investor</param>
        /// <returns></returns>
        public List<AppListTradingProviderDto> BusinessSaleListTrading(int investorId)
        {
            var result = _oracleHelper.ExecuteProcedure<AppListTradingProviderDto>(PROC_BUSINESS_LIST_TRADING, new
            {
                pv_INVESTOR_ID = investorId,
            }).ToList();

            return result;
        }
    }
}
