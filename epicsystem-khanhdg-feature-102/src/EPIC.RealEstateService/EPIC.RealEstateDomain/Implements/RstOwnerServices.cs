using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOwnerServices : IRstOwnerServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstOwnerServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstOwnerEFRepository _realEstateOwnerEFRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;

        public RstOwnerServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstOwnerServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _realEstateOwnerEFRepository = new RstOwnerEFRepository(dbContext, logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
        }

        public RstOwner Add(CreateRstOwnerDto input)
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(Add)}: input = {input}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var owner = _realEstateOwnerEFRepository.Entity.FirstOrDefault(p => p.BusinessCustomerId == input.BusinessCustomerId && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (owner != null)
            {
                _realEstateOwnerEFRepository.ThrowException(ErrorCode.RstOwnerExist);
            }

            var insert = _realEstateOwnerEFRepository.Add(_mapper.Map<RstOwner>(input), username, partnerId);
            _dbContext.SaveChanges();
            return insert;
        }

        //// <summary>
        /// Cập nhật chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOwner Update(UpdateRstOwnerDto input)
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(Update)}: input = {input}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var owner = _realEstateOwnerEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstOwner>(_dbContext, ErrorCode.RstOwnerNotFound);
            var updateOwner = _realEstateOwnerEFRepository.Update(_mapper.Map<RstOwner>(input), partnerId, username);
            _dbContext.SaveChanges();
            return updateOwner;
        }

        /// <summary>
        /// Xoá chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(Delete)}: id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var owner = _realEstateOwnerEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstOwner>(_dbContext, ErrorCode.RstOwnerNotFound);

            owner.Deleted = YesNo.YES;
            owner.ModifiedBy = username;
            owner.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewRstOwnerDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(FindById)}: id = {id}");
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var owner = _realEstateOwnerEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstOwner>(_dbContext, ErrorCode.RstOwnerNotFound);
            var result = _mapper.Map<ViewRstOwnerDto>(owner);

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(owner.BusinessCustomerId);
            if (businessCustomer != null)
            {
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                result.OwnerName = businessCustomer.Name;
                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankDefault(businessCustomer.BusinessCustomerId ?? 0, IsTemp.NO);
                if (businessCustomerBank != null)
                {
                    result.BusinessCustomer.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
                }
            }

            return result;
        }

        /// <summary>
        /// Tìm danh sách chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<ViewRstOwnerDto> FindAll(FilterRstOwnerDto input)
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(FindAll)}: input = {input}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var resultPaging = new PagingResult<ViewRstOwnerDto>();
            var items = new List<ViewRstOwnerDto>();
            var find = _realEstateOwnerEFRepository.FindAll(input, partnerId);

            foreach (var ownerFind in find.Items)
            {
                var owner = _mapper.Map<ViewRstOwnerDto>(ownerFind);
                
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(ownerFind.BusinessCustomerId);
                if (businessCustomer != null)
                {
                    owner.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    owner.OwnerName = businessCustomer.Name;
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    owner.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(owner);
            }

            resultPaging.Items = items;
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// Xem danh sách chủ đầu tư theo đối tác
        /// </summary>
        /// <returns></returns>
        public List<ViewRstOwnerDto> GetAllOwnerByPartner()
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(GetAllOwnerByPartner)}");
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _realEstateOwnerEFRepository.GetAllByPartner(partnerId);
        }

        /// <summary>
        /// Xem danh sách chủ đầu tư theo đối tác
        /// </summary>
        /// <returns></returns>
        public List<ViewRstOwnerDto> GetAllOwnerByTrading()
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(GetAllOwnerByTrading)}");
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _realEstateOwnerEFRepository.GetAllByTrading(tradingProviderId);
        }

        /// <summary>
        /// Thay đổi trạng thái chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstOwner ChangeStatus(int id, string status)
        {
            _logger.LogInformation($"{nameof(RstOwnerServices)}->{nameof(ChangeStatus)}: id = {id}, status = {status}");

            var owner = _realEstateOwnerEFRepository.FindById(id).ThrowIfNull<RstOwner>(_dbContext, ErrorCode.RstOwnerNotFound);
            
            var changeStatus = _realEstateOwnerEFRepository.ChangeStatus(id, status);
            _dbContext.SaveChanges();
            return changeStatus;
        }

        /// <summary>
        /// Cập nhật nội dung miêu tả chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        public void UpdateOwnerDescriptionContent(UpdateRstOwnerDescriptionDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateOwnerDescriptionContent)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var ownerFind = _realEstateOwnerEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstOwnerNotFound);
            ownerFind.DescriptionContent = input.DescriptionContent;
            ownerFind.DescriptionContentType = input.DescriptionContentType;
            _dbContext.SaveChanges();
        }
    }
}
