using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectFile;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectFileServices : IRstProjectFileServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectFileServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProjectFileEFRepository _rstProjectFileEFRepository;

        public RstProjectFileServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectFileServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProjectFileEFRepository = new RstProjectFileEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProjectFile> Add(CreateRstProjectFilesDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {input}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var juridicalFileList = new List<RstProjectFile>();

            foreach (var item in input.RstProjectJuridicalFiles)
            {
                var insert = _rstProjectFileEFRepository.Add(_mapper.Map<RstProjectFile>(item), username, partnerId);
                juridicalFileList.Add(insert);
            }

            foreach (var item in juridicalFileList)
            {
                item.ProjectFileType = input.JuridicalFileType;
            }

            _dbContext.SaveChanges();
            return juridicalFileList;
        }

        /// <summary>
        /// Cập nhập hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectFile Update(UpdateRstProjectFileDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {input}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var juridicalFile = _rstProjectFileEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectFile>(_dbContext, ErrorCode.RstProjectFileNotFound);
            var updateJuridicalFile = _rstProjectFileEFRepository.Update(_mapper.Map<RstProjectFile>(input), partnerId, username);
            _dbContext.SaveChanges();
            return updateJuridicalFile;
        }

        /// <summary>
        /// Xoá hồ sơ dự án
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var juridicalFile = _rstProjectFileEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectFile>(_dbContext, ErrorCode.RstProjectFileNotFound);

            juridicalFile.Deleted = YesNo.YES;
            juridicalFile.ModifiedBy = username;
            juridicalFile.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm hồ sơ dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectFile FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var policy = _rstProjectFileEFRepository.FindById(id).ThrowIfNull<RstProjectFile>(_dbContext, ErrorCode.RstProjectFileNotFound);

            return policy;
        }

        /// <summary>
        /// Tìm danh sách hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstProjectFile> FindAll(FilterRstProjectFileDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {input}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = _rstProjectFileEFRepository.FindAll(input, partnerId);

            return result;
        }

        /// <summary>
        /// Đổi trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectFile ChangeStatus(int id, string status)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}");

            var juridicalFile = _rstProjectFileEFRepository.FindById(id).ThrowIfNull<RstProjectFile>(_dbContext, ErrorCode.RstProjectFileNotFound);

            var changeStatus = _rstProjectFileEFRepository.ChangeStatus(id, status);
            _dbContext.SaveChanges();
            return changeStatus;
        }

    }
}
