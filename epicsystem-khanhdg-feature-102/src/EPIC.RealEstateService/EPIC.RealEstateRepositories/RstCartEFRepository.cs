using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstCartEFRepository : BaseEFRepository<RstCart>
    {
        public RstCartEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstCart.SEQ}")
        {
        }

        public void Add(RstCart input)
        {
            _logger.LogInformation($"{nameof(RstCartEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            _dbSet.Add(input);
        }

        public RstCart FindById(int id, int investorId)
        {
            _logger.LogInformation($"{nameof(RstCartEFRepository)}->{nameof(FindById)}: id = {id}, investorId = {investorId}");
            var result = _dbSet.FirstOrDefault(c => c.Id == id && c.InvestorId == investorId && c.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Xem các thông liên quan đến item giỏ hàng
        /// </summary>
        public RstCartDetailDto GetByIdAndInvestorId(int investorId, int id)
        {
            _logger.LogInformation($"{nameof(RstCartEFRepository)}->{nameof(GetByIdAndInvestorId)}: id = {id} investorId = {investorId}");
            return FindCartByInvestor(investorId, id).FirstOrDefault();
        }

        /// <summary>
        /// Danh sách giỏ hàng của nhà đầu tư
        /// Nếu truyền cartId thì lấy chính xác giỏ hàng đấy
        /// </summary>
        public IQueryable<RstCartDetailDto> FindCartByInvestor(int investorId, int? cartId = null, int? cartStatus = null)
        {
            _logger.LogInformation($"{nameof(RstCartEFRepository)}->{nameof(FindCartByInvestor)}: investorId = {investorId}");

            var cifCodeQuery = _epicSchemaDbContext.CifCodes.FirstOrDefault(r => r.InvestorId == investorId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.CoreCifCodeNotFound);
            var result = from cart in _dbSet
                         join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on cart.OpenSellDetailId equals openSellDetail.Id
                         join openSell in _epicSchemaDbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                         join distributionItem in _epicSchemaDbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionItem.Id
                         join productItem in _epicSchemaDbContext.RstProductItems on distributionItem.ProductItemId equals productItem.Id
                         join project in _epicSchemaDbContext.RstProjects on productItem.ProjectId equals project.Id
                         where cart.InvestorId == investorId && cart.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                         && (cartId == null || cart.Id == cartId) && project.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO
                         && (cartStatus == null || cart.Status == cartStatus)
                         // Lọc các item giỏ hàng đã có trong sổ lệnh
                         && ((openSellDetail.Status == RstProductItemStatus.KHOI_TAO) || !(from orderSub in _epicSchemaDbContext.RstOrders
                              join productItemSub in _epicSchemaDbContext.RstProductItems on orderSub.ProductItemId equals productItem.Id
                              where orderSub.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && orderSub.CifCode == cifCodeQuery.CifCode
                              && productItem.Id == productItemSub.Id
                              select orderSub).Any())
                         select new RstCartDetailDto
                         {
                             Id = cart.Id,
                             TradingProviderId = openSell.TradingProviderId,
                             OpenSellDetailId = openSellDetail.Id,
                             OpenSellId = openSell.Id,
                             DistributionId = distributionItem.DistributionId,
                             ProjectId = productItem.ProjectId,
                             ProductItemId = productItem.Id,
                             Code = productItem.Code,
                             ProjectCode = project.Code,
                             ProjectName = project.Name,
                             Name = productItem.Name,
                             DoorDirection = productItem.DoorDirection,
                             RoomType = productItem.RoomType,
                             PriceArea = productItem.PriceArea,
                             Price = productItem.Price ?? 0,
                             Hotline = openSell.Hotline,
                             ProductItemStatus = (productItem.Status == RstProductItemStatus.KHOI_TAO && openSell.StartDate.Date < DateTime.Now)
                                                 ? RstProductItemStatus.LOGIC_DANG_MO_BAN : productItem.Status,
                             OpenSellDetailStatus = openSellDetail.Status,
                             KeepTime = openSell.KeepTime,
                             IsShowPrice = openSellDetail.IsShowPrice,
                             ContactType = openSellDetail.ContactType,
                             ContactPhone = openSellDetail.ContactPhone
                         };

            result = result.OrderByDescending(c => c.Id);
            return result;
        }
    }
}
