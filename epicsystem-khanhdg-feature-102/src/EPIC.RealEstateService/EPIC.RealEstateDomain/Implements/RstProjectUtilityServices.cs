using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectUtilityServices : IRstProjectUtilityServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectUtilityEFRepository _rstProjectUtilityEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;

        public RstProjectUtilityServices(EpicSchemaDbContext dbContext,
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
        }

        /// <summary>
        /// Cập nhật tiện ích (Chưa có thì thêm, có rồi thì giữ nguyên)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProjectUtilityDto> UpdateProjectUtility(CreateRstProjectUtilityDto input)
        {
            _logger.LogInformation($"{nameof(UpdateProjectUtility)} : input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var projectFind = _rstProjectEFRepository.FindById(input.ProjectId, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);

            var result = new List<RstProjectUtilityDto>();
            var projectUtilities = _rstProjectUtilityEFRepository.Entity.Where(e => e.ProjectId == projectFind.Id && e.Deleted == YesNo.NO).Select(e => e.UtilityId).ToList();
            
            //Lấy danh sách data utility
            var listDefault = RstProjectUtilityData.UtilityData;
            var groupUtilityData = GroupRstProjectUtility.GroupData;
            var inputList = input.ProjectUtilites.Select(e => e.UtilityId);

            var insertList = inputList.Except(projectUtilities);
            var removeList = projectUtilities.Except(inputList);
            var updateList = projectUtilities.Except(removeList);
            
            //Thêm tiện ích chưa có trong db
            foreach (var item in insertList)
            {
                var utility = listDefault.FirstOrDefault(e => e.Id == item);
                var insertUtility = input.ProjectUtilites.FirstOrDefault(e => e.UtilityId == item);

                var projectUtilityInsert = new RstProjectUtility()
                {
                    UtilityId = item,
                    Type = utility.Type,
                    ProjectId = projectFind.Id,
                    IsHighlight = insertUtility != null ? insertUtility.IsHighlight : YesNo.NO
                };
                _rstProjectUtilityEFRepository.Add(projectUtilityInsert, username, partnerId, input.ProjectId);
                result.Add(_mapper.Map<RstProjectUtilityDto>(projectUtilityInsert));
            }

            //Cấp nhật tiện ích
            foreach (var item in updateList)
            {
                var insertUtility = input.ProjectUtilites.FirstOrDefault(e => e.UtilityId == item);
                var updateUtility = _rstProjectUtilityEFRepository.FindByUtilityId(item, partnerId, input.ProjectId);
                updateUtility.IsHighlight = insertUtility.IsHighlight;
                result.Add(_mapper.Map<RstProjectUtilityDto>(updateUtility));
            }

            //Xóa tiện ích đã có trong Db
            foreach (var item in removeList)
            {
                var removeUtility = _rstProjectUtilityEFRepository.FindByUtilityId(item, partnerId, input.ProjectId);
                _dbContext.Remove(removeUtility);
            }
            _dbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Xóa chọn tiện ích
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProjectUtility(int id)
        {
            _logger.LogInformation($"{nameof(UpdateProjectUtility)} : id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            //Xóa tiện ích
            var getProjectUtility = _rstProjectUtilityEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityNotFound);
            if (getProjectUtility != null)
            {
                getProjectUtility.Deleted = YesNo.YES;
                getProjectUtility.ModifiedBy = username;
                getProjectUtility.ModifiedDate = DateTime.Now;
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstProjectUtilityDto> FindAll(FilterRstProjectUtilityDto input, int projectId)
        {
            _logger.LogInformation($"{nameof(FindAll)} :input = {JsonSerializer.Serialize(input)}, projectId = {projectId}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            PagingResult<RstProjectUtilityDto> result = new();
            List<RstProjectUtilityDto> listItem = new();

            //Lấy ra danh sách tiện ích đá được chọn
            var getProjectUtility = _rstProjectUtilityEFRepository.Entity.Where(e => e.ProjectId == projectId && e.Deleted == YesNo.NO);
            var Utilities = RstProjectUtilityData.UtilityData;
            var groupUtilityData = GroupRstProjectUtility.GroupData;

            var query = from utility in Utilities
                        join groupUtility in groupUtilityData on utility.GroupUtilityId equals groupUtility.Id
                        join projectUtility in getProjectUtility on utility.Id equals projectUtility.UtilityId into projectUtiliteList
                        from itemUtility in projectUtiliteList.DefaultIfEmpty()
                        where (input.GroupId == null || utility.GroupUtilityId == input.GroupId)
                            && (input.Type == null || utility.Type == input.Type)
                            && (input.Name == null || utility.Name.Contains(input.Name))
                            && (input.IsHighlight == null || (itemUtility != null && itemUtility.IsHighlight == input.IsHighlight))
                            && (input.IsSelected == null || (itemUtility != null))
                        select new
                        {
                            utility,
                            groupUtility,
                            itemUtility
                        };

            result.TotalItems = query.Count();

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            foreach (var item in query)
            {
                var utility = new RstProjectUtilityDto()
                {
                    Id = item.itemUtility?.Id,
                    UtilityId = item.utility.Id,
                    Name = item.utility.Name,
                    GroupId = item.groupUtility.Id,
                    GroupName = item.groupUtility.Name,
                    Type = item.utility.Type,
                    IsSelected = item.itemUtility == null ? YesNo.NO : YesNo.YES,
                    IsHighlight = item.itemUtility == null ? YesNo.NO : item.itemUtility.IsHighlight,
                };

                listItem.Add(utility);
            }

            result.Items = listItem;
            return result;
        }

        /// <summary>
        /// Get DataUtlity
        /// </summary>
        /// <returns></returns>
        public List<RstProjectUtilityData> GetAllUtility()
        {
            _logger.LogInformation($"{nameof(FindAll)} :");
            var listDefault = RstProjectUtilityData.UtilityData;
            return listDefault;
        }

        /// <summary>
        /// Get DataGroup
        /// </summary>
        /// <returns></returns>
        public List<GroupRstProjectUtility> GetAllGroupUtility()
        {
            _logger.LogInformation($"{nameof(FindAll)} :");
            var groupUtilityData = GroupRstProjectUtility.GroupData;
            return groupUtilityData;
        }
    }
}
