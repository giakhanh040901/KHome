using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSellFile;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOpenSellFileServices : IRstOpenSellFileServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstOpenSellFileServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstOpenSellFileEFRepository _rstOpenSellFileEFRepository;

        public RstOpenSellFileServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstOpenSellFileServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstOpenSellFileEFRepository = new RstOpenSellFileEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstOpenSellFile> Add(CreateRstOpenSellFilesDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            var openSellFileList = new List<RstOpenSellFile>();

            foreach (var item in input.RstOpenSellFiles)
            {
                var insert = _rstOpenSellFileEFRepository.Add(_mapper.Map<RstOpenSellFile>(item), username, tradingProviderId);
                openSellFileList.Add(insert);
            }

            foreach (var item in openSellFileList)
            {
                item.OpenSellFileType = input.OpenSellFileType;
            }

            _dbContext.SaveChanges();
            return openSellFileList;
        }

        /// <summary>
        /// Cập nhập hồ sơ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOpenSellFile Update(UpdateRstOpenSellFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            
            var openSellFile = _rstOpenSellFileEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstOpenSellFile>(_dbContext, ErrorCode.RstOpenSellFileNotFound);
            var updateOpenSellFile = _rstOpenSellFileEFRepository.Update(_mapper.Map<RstOpenSellFile>(input), tradingProviderId, username);
            _dbContext.SaveChanges();
            return updateOpenSellFile;
        }

        /// <summary>
        /// Xoá hồ sơ mở bán
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: username = {username}, tradingProviderId = {tradingProviderId}");

            var openSellFile = _rstOpenSellFileEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstOpenSellFile>(_dbContext, ErrorCode.RstOpenSellFileNotFound);

            openSellFile.Deleted = YesNo.YES;
            openSellFile.ModifiedBy = username;
            openSellFile.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm hồ sơ mở bán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstOpenSellFile FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");

            var openSellFile = _rstOpenSellFileEFRepository.FindById(id, tradingProviderId).ThrowIfNull<RstOpenSellFile>(_dbContext, ErrorCode.RstOpenSellFileNotFound);

            return openSellFile;
        }

        /// <summary>
        /// Tìm danh sách hồ sơ mở bán có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstOpenSellFile> FindAll(FilterRstOpenSellFileDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var result = _rstOpenSellFileEFRepository.FindAll(input, tradingProviderId);
            return result;
        }

        /// <summary>
        /// Thay đổi trạng thái hồ sơ mở bán: đang kích hoạt -> dừng kích hoạt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstOpenSellFile ChangeStatus(int id, string status)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, tradingProviderId = {tradingProviderId}");

            var openSellFile = _rstOpenSellFileEFRepository.FindById(id, tradingProviderId).ThrowIfNull<RstOpenSellFile>(_dbContext, ErrorCode.RstOpenSellFileNotFound);
            var changeStatus = _rstOpenSellFileEFRepository.ChangeStatus(id, status, tradingProviderId);
            _dbContext.SaveChanges();
            return changeStatus;
        }
    }
}
