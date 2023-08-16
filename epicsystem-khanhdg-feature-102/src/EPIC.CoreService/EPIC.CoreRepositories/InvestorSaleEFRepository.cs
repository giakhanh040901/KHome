using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class InvestorSaleEFRepository : BaseEFRepository<InvestorSale>
    {
        private const string PROC_INVESTOR_SALE_LIST_TRADIN = DbSchemas.EPIC + ".PKG_INVESTOR_SALE.PROC_INVESTOR_SALE_LIST_TRADIN";
        private const string PROC_GET_LIST_SALE = DbSchemas.EPIC + ".PKG_INVESTOR_SALE.PROC_GET_LIST_SALE";
        private const string PROC_BUSINESS_LIST_TRADING = DbSchemas.EPIC + ".PKG_INVESTOR_SALE.PROC_BUSINESS_LIST_TRADING";

        private readonly EpicSchemaDbContext _investorSaleDbContext;

        public InvestorSaleEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, TradingProvider.SEQ)
        {
            _investorSaleDbContext = dbContext;
        }

        /// <summary>
        /// Danh sách đại lý sơ cấp là đại lý được bán hộ của đại lý mà investor đang tham gia
        /// </summary>
        /// <param name="investorId">Id của investor</param>
        /// <returns></returns>
        public List<AppListTradingProviderDto> BusinessSaleListTrading(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId};");

            var conveted = ObjectToParamAndQueryList(PROC_BUSINESS_LIST_TRADING, new
            {
                pv_INVESTOR_ID = investorId,
            });
            try
            {
                return _dbContext.Set<AppListTradingProviderDto>().FromSqlRaw(conveted.SqlQuery, conveted.Parameters)?.ToList();
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex);
            }
        }

        /// <summary>
        /// Lấy sale mặc định của nhà đầu tư đã chọn
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public InvestorSale FindInvestorSale(int investorId)
        {
            return _epicSchemaDbContext.InvestorSales.OrderByDescending(x => x.IsDefault).FirstOrDefault(i => i.InvestorId == investorId);
        }

        /// <summary>
        /// Check xem investor đã quét mã giới thiệu của sale bao giờ chưa 
        /// aka Đã xuất hiện trong bảng investor_sale chưa
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public bool IsInvestorScanReferralCode(int investorId)
        {
            return _epicSchemaDbContext.InvestorSales.Any(iss => iss.InvestorId == investorId && iss.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tạo bản ghi investor sale
        /// </summary>
        /// <param name="entity"></param>
        public void CreateInvestorSale(InvestorSale entity)
        {
            entity.Id = (int)NextKey(InvestorSale.SEQ);
            entity.CreatedDate = DateTime.Now;
            entity.Deleted = YesNo.NO;
            _dbSet.Add(entity);
        }

        /// <summary>
        ///  Thêm bản ghi investorSale, nếu đã tồn tại thì bỏ qua, nếu chưa có thì thêm vào quan hệ
        /// </summary>
        /// <param name="entity"></param>
        public void InsertInvestorSale(InvestorSale entity)
        {
            _logger.LogInformation($"{nameof(InsertInvestorSale)}: input = {JsonSerializer.Serialize(entity)}");
            // Kiểm tra xem đã có bản ghi hay chưa
            var checkInvestorSale = _dbSet.Any(i => i.InvestorId == entity.InvestorId && i.SaleId == entity.SaleId && i.Deleted == YesNo.NO);
            if (!checkInvestorSale)
            {
                entity.Id = (int)NextKey(InvestorSale.SEQ);
                entity.CreatedDate = DateTime.Now;
                // Tìm kiếm xem đã có sale mặc định hay chưa. Nếu chưa có thì Set làm mặc định
                entity.IsDefault = _dbSet.Any(i => i.InvestorId == entity.InvestorId && i.IsDefault == YesNo.YES && i.Deleted == YesNo.NO) ? YesNo.NO : YesNo.YES;
                entity.Deleted = YesNo.NO;
                _dbSet.Add(entity);
            }
        }
    }
}
