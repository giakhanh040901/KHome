using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstOwnerEFRepository : BaseEFRepository<RstOwner>
    {
        public RstOwnerEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOwner.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstOwner Add(RstOwner input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(Add)}: input = {input}");
            return _dbSet.Add(new RstOwner()
            {
                Id = (int)NextKey(),
                BusinessCustomerId = input.BusinessCustomerId,
                PartnerId = partnerId,
                BusinessTurnover = input.BusinessTurnover,
                BusinessProfit = input.BusinessProfit,
                Roa = input.Roa,
                Roe = input.Roe,
                Website = input.Website,
                Hotline = input.Hotline,
                Fanpage = input.Fanpage,
                DescriptionContent = input.DescriptionContent,
                DescriptionContentType = input.DescriptionContentType,
                CreatedDate = DateTime.Now,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstOwner Update(RstOwner input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(Update)}: input = {input}, partnerId = {partnerId}, username = {username}");
            var owner = _dbSet.FirstOrDefault(o => o.Id == input.Id && o.PartnerId == partnerId && o.Deleted == YesNo.NO);
            if (owner != null)
            {
                owner.PartnerId = partnerId;
                owner.BusinessTurnover = input.BusinessTurnover;
                owner.BusinessProfit = input.BusinessProfit;
                owner.Roa = input.Roa;
                owner.Roe = input.Roe;
                owner.Website = input.Website;
                owner.Hotline = input.Hotline;
                owner.Fanpage = input.Fanpage;
                owner.DescriptionContentType = input.DescriptionContentType;
                owner.DescriptionContent = input.DescriptionContent;
                owner.ModifiedBy = username;
                owner.ModifiedDate = DateTime.Now;
            }
            return owner;
        }

        /// <summary>
        /// Tìm kiếm chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstOwner FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(FindById)}: id = {id}");

            return _dbSet.FirstOrDefault(o => o.Id == id && o.Deleted == YesNo.NO);
        }
        public List<ViewRstOwnerDto> GetAllByPartner(int partnerId)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(GetAllByPartner)}: partnerId = {partnerId}");
            return (from owner in _dbSet
                    join businessCustomer in _epicSchemaDbContext.BusinessCustomers on owner.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                    where owner.PartnerId == partnerId && owner.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                    select new ViewRstOwnerDto
                    {
                        Id = owner.Id,
                        BusinessCustomerId = owner.BusinessCustomerId,
                        BusinessProfit = owner.BusinessProfit,
                        BusinessTurnover = owner.BusinessTurnover,
                        Roa = owner.Roa,
                        Roe = owner.Roe,
                        Website = owner.Website,
                        Hotline = owner.Hotline,
                        Fanpage = owner.Fanpage,
                        Status = owner.Status,
                        OwnerName = businessCustomer.Name
                    }).ToList();
        }

        public List<ViewRstOwnerDto> GetAllByTrading(int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(GetAllByPartner)}: tradingProviderId = {tradingProviderId}");
            return (from owner in _dbSet
                    join project in _epicSchemaDbContext.RstProjects on owner.Id equals project.OwnerId
                    join distribution in _epicSchemaDbContext.RstDistributions on project.Id equals distribution.ProjectId
                    join businessCustomer in _epicSchemaDbContext.BusinessCustomers on owner.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                    where distribution.TradingProviderId == tradingProviderId && owner.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                    && distribution.Deleted == YesNo.NO && project.Deleted == YesNo.NO
                    select new ViewRstOwnerDto
                    {
                        Id = owner.Id,
                        BusinessCustomerId = owner.BusinessCustomerId,
                        BusinessProfit = owner.BusinessProfit,
                        BusinessTurnover = owner.BusinessTurnover,
                        Roa = owner.Roa,
                        Roe = owner.Roe,
                        Website = owner.Website,
                        Hotline = owner.Hotline,
                        Fanpage = owner.Fanpage,
                        Status = owner.Status,
                        OwnerName = businessCustomer.Name
                    }).Distinct().ToList();
        }

        /// <summary>
        /// Xoá chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(Delete)}: id = {id}");
            
            var owner = _dbSet.FirstOrDefault(o => o.Id == id && o.Deleted == YesNo.NO);
            if (owner != null)
            {
                owner.Deleted = YesNo.YES;
            }
        }

        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstOwner ChangeStatus(int id, string status)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(ChangeStatus)}: id = {id}");
            var owner = _dbSet.FirstOrDefault(o => o.Id == id && o.Deleted == YesNo.NO);
            if (owner != null)
            {
                owner.Status = status;
            }
            return owner;
        }

        /// <summary>
        /// Tìm kiếm theo danh sách
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstOwner> FindAll(FilterRstOwnerDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(FindAll)}: input = {input}, partnerId = {partnerId}");

            PagingResult<RstOwner> result = new();
            IQueryable<RstOwner> ownerQuery = from owner in _dbSet
                                              join businessCustomer in _epicSchemaDbContext.BusinessCustomers on owner.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                              where owner.Deleted == YesNo.NO && (partnerId == null || owner.PartnerId == partnerId)
                                              && (input.Name == null || businessCustomer.Name.Contains(input.Name))
                                              && (input.TaxCode == null || businessCustomer.TaxCode.Contains(input.TaxCode))
                                              && (input.Keyword == null || businessCustomer.TaxCode.Contains(input.Keyword))
                                              && (input.Status == null || owner.Status == input.Status)
                                              select owner;

            ownerQuery = ownerQuery.OrderByDescending(o => o.Id);

            result.TotalItems = ownerQuery.Count();

            if (input.PageSize != -1)
            {
                ownerQuery = ownerQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = ownerQuery;
            return result;
        }

        /// <summary>
        /// App lấy thông tin chủ đầu tư cho Tab chủ đầu tư ở Chi tiết dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppViewDetailProjectOwnerDto AppFindById(int id)
        {
            _logger.LogInformation($"{nameof(RstOwnerEFRepository)}->{nameof(FindById)}: id = {id}");
            var query = from owner in _epicSchemaDbContext.RstOwners
                        from bus in _epicSchemaDbContext.BusinessCustomers.Where(x => x.BusinessCustomerId == owner.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        where owner.Id == id && owner.Deleted == YesNo.NO
                        select new AppViewDetailProjectOwnerDto
                        {
                            DescriptionContent = owner.DescriptionContent,
                            DescriptionContentType = owner.DescriptionContentType,
                            IsCheck = bus.IsCheck == YesNo.YES,
                            OwnerName = bus.Name,
                            Fanpage = owner.Fanpage,
                            Website = owner.Website,
                            Hotline = owner.Hotline,
                        };
            return query.FirstOrDefault();
        }
    }
}
