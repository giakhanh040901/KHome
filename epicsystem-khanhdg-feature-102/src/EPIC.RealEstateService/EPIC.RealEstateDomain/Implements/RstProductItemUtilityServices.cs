using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProductItemUtilityServices : IRstProductItemUtilityServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectUtilityEFRepository _rstProjectUtilityEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstProductItemUtilityEFRepository _rstProductItemUtitilyEFRepository;

        public RstProductItemUtilityServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectUtilityEFRepository = new RstProjectUtilityEFRepository(dbContext, logger);
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProductItemUtitilyEFRepository = new RstProductItemUtilityEFRepository(_dbContext, logger);
        }

        /// <summary>
        /// Cập nhập tiện ích căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProductItemUtilityDto> UpdateProductItemUtility(CreateRstProductItemUtilityDto input)
        {
            _logger.LogInformation($"{nameof(UpdateProductItemUtility)} : input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var productItemFind = _rstProductItemEFRepository.FindById(input.ProductItemId, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);

            var productItemUtilities = _rstProductItemUtitilyEFRepository.Entity.Where(e => e.ProductItemId == productItemFind.Id && e.Deleted == YesNo.NO).Select(e => e.ProductItemUtilityId).ToList();

            var result = new List<RstProductItemUtilityDto>();

            //Lấy danh sách data utility
            var listDefault = RstProductItemUtilityData.UtilityData;
            var groupUtilityData = GroupRstProjectUtility.GroupData;
            var inputList = input.ProductItemUtilities;

            //danh sách phần tử chưa có trong db 
            var insertList = inputList.Except(productItemUtilities);

            //danh sách phần tử không có trong danh sách đầu vào
            var removeList = productItemUtilities.Except(inputList);

            //danh sách phần tử đã có trong database
            var updateList = productItemUtilities.Intersect(inputList);

            //Thêm tiện ích chưa có trong db
            foreach (var item in insertList)
            {
                var productItemUtilityInsert = new RstProductItemUtility()
                {
                    ProductItemUtilityId = item ?? 0,
                    ProductItemId = productItemFind.Id,
                    Status = StatusSymbol.ACTIVE,
                };
                _rstProductItemUtitilyEFRepository.Add(productItemUtilityInsert, username, partnerId);
                result.Add(_mapper.Map<RstProductItemUtilityDto>(productItemUtilityInsert));
            }

            //Cập nhật tiện ích
            foreach (var item in updateList)
            {
                var updateUtility = _rstProductItemUtitilyEFRepository.FindByProductItemUtilityId(item, partnerId, input.ProductItemId);
                updateUtility.ModifiedDate = DateTime.Now;
                updateUtility.ModifiedBy = username;
                result.Add(_mapper.Map<RstProductItemUtilityDto>(updateUtility));
            }

            //Xóa tiện ích đã có trong db những không có trong đầu vào
            foreach (var item in removeList)
            {
                var removeUtility = _rstProductItemUtitilyEFRepository.FindByProductItemUtilityId(item, partnerId, input.ProductItemId);
                _dbContext.Remove(removeUtility);
            }

            _dbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Xóa chọn tiện ích
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProductItemUtility(int id)
        {
            _logger.LogInformation($"{nameof(DeleteProductItemUtility)} : id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            //Xóa tiện ích
            var productItemProjectUtilities = _rstProductItemUtitilyEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityNotFound);
            if (productItemProjectUtilities != null)
            {
                productItemProjectUtilities.Deleted = YesNo.YES;
                productItemProjectUtilities.ModifiedBy = username;
                productItemProjectUtilities.ModifiedDate = DateTime.Now;

                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// lấy ra tất cả tiện ích của căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstProductItemUtilityDto> FindAll(FilterProductItemProjectUtilityDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)} :input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            PagingResult<RstProductItemUtilityDto> result = new();
            List<RstProductItemUtilityDto> listItem = new();

            //Lấy ra danh sách tiện ích đá được chọn
            var getProductItemProjectUtility = _rstProductItemUtitilyEFRepository.Entity.Where(e => e.ProductItemId == input.ProductItemId && e.Deleted == YesNo.NO);
            var utilities = RstProductItemUtilityData.UtilityData;
            var groupUtilityData = GroupRstProjectUtility.GroupData;

            var query = from utility in utilities
                        join groupUtility in groupUtilityData on utility.GroupUtilityId equals groupUtility.Id
                        join productItemUtilities in getProductItemProjectUtility on utility.Id equals productItemUtilities.ProductItemUtilityId into projectUtiliteList
                        from itemUtility in projectUtiliteList.DefaultIfEmpty()
                        where (input.Name == null || utility.Name.Contains(input.Name))
                              && (input.Status == null || itemUtility?.Status == input.Status)
                              && (input.UtilityId == null || itemUtility?.ProductItemUtilityId == input.UtilityId)
                              && (input.Selected == null || (itemUtility != null))
                        select new
                        {
                            utility,
                            groupUtility,
                            itemUtility,
                        };

            var querys = query.ToList();
            result.TotalItems = query.Count();

            query = input.PageSize != -1 ? query.Skip(input.Skip).Take(input.PageSize) : query;

            foreach (var item in query)
            {
                var utility = new RstProductItemUtilityDto()
                {
                    Id = item.itemUtility?.Id ?? 0,
                    UtilityId = item.utility?.Id ?? 0,
                    Name = item.utility?.Name,
                    IsProductItemSelected = item.itemUtility == null ? YesNo.NO : YesNo.YES,
                    Status = item.itemUtility?.Status
                };

                listItem.Add(utility);
            }

            result.Items = listItem;
            return result;
        }
    }
}
