using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSellFile;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstOpenSellFileEFRepository : BaseEFRepository<RstOpenSellFile>
    {
        public RstOpenSellFileEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOpenSellFile.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOpenSellFile Add(RstOpenSellFile input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellFileEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            return _dbSet.Add(new RstOpenSellFile()
            {
                Id = (int)NextKey(),
                TradingProviderId = tradingProviderId,
                OpenSellId = input.OpenSellId,
                Name = input.Name,
                Url = input.Url,
                OpenSellFileType = input.OpenSellFileType,
                CreatedBy = username,
                ValidTime = input.ValidTime,
            }).Entity;
        }

        /// <summary>
        /// Cập nhật hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstOpenSellFile Update(RstOpenSellFile input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(RstOpenSellFileEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            var file = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (file != null)
            {
                file.OpenSellId = input.OpenSellId;
                file.Name = input.Name;
                file.Url = input.Url;
                file.ModifiedBy = username;
                file.ModifiedDate = DateTime.Now;
                file.ValidTime = input.ValidTime;
            }

            return file;
        }

        /// <summary>
        /// Tìm hồ sơ mở bán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOpenSellFile FindById(int id, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellFileEFRepository)}->{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(p => p.Id == id && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thay đổi trạng thái hồ sơ mở bán: kích hoạt -> huỷ kích hoạt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOpenSellFile ChangeStatus(int id, string status, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellFileEFRepository)}->{nameof(ChangeStatus)}: id = {id}, status = {status}, tradingProviderId = {tradingProviderId}");

            var file = _dbSet.FirstOrDefault(p => p.Id == id && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) && p.Deleted == YesNo.NO);

            if (file != null)
            {
                file.Status = status;
            }

            return file;
        }

        /// <summary>
        /// Tìm kiếm danh sách hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<RstOpenSellFile> FindAll(FilterRstOpenSellFileDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectFileEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<RstOpenSellFile> result = new();
            IQueryable<RstOpenSellFile> openSellFileQuery = _dbSet.Where(p => p.Deleted == YesNo.NO && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) 
            && (input.Status == null || p.Status == input.Status) && (input.Name == null || p.Name.ToLower().Contains(input.Name.ToLower()))
            && (input.Keyword == null || p.Name.ToLower().Contains(input.Keyword.ToLower())));

            // Lọc theo id mở bán
            if (input.OpenSellId != null)
            {
                openSellFileQuery = openSellFileQuery.Where(p => p.OpenSellId == input.OpenSellId);
            }

            // Lọc theo loại file
            if (input.OpenSellFileType != null)
            {
                openSellFileQuery = openSellFileQuery.Where(p => p.OpenSellFileType == input.OpenSellFileType);
            }

            result.TotalItems = openSellFileQuery.Count();

            openSellFileQuery = openSellFileQuery.OrderByDescending(p => p.Id);

            if (input.PageSize != -1)
            {
                openSellFileQuery = openSellFileQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = openSellFileQuery.ToList();
            return result;
        }

        /// <summary>
        /// App lấy tài liệu active theo Dự án, Loại tài liệu
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<RstOpenSellFile> AppFindActiveByProjectId(int projectId, int type, string status = "A")
        {
            var query = from openSell in _epicSchemaDbContext.RstOpenSells.AsNoTracking().Where(x => x.ProjectId == projectId && x.Deleted == YesNo.NO)
                        from openSellFile in _dbSet.AsNoTracking().Where(x => x.OpenSellId == openSell.Id && x.OpenSellFileType == type && (string.IsNullOrEmpty(status) || x.Status == status) && x.Deleted == YesNo.NO)
                        select openSellFile;
            return query.ToList();
        }
    }
}
