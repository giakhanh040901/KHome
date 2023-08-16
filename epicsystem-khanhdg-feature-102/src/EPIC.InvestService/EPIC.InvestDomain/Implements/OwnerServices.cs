using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.InvestRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ServiceModel;

namespace EPIC.InvestDomain
{
    public class OwnerServices : IOwnerServices
    {
        private readonly ILogger<OwnerServices> _logger;
        private string _connectionString;
        private readonly OwnerRepository _ownerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public OwnerServices(ILogger<OwnerServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _ownerRepository = new OwnerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public Owner Add(CreateOwnerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var invOwner = new Owner
            {
                BusinessCustomerId = input.BusinessCustomerId,
                PartnerId = partnerId,
                BusinessTurnover = input.BusinessTurnover,
                BusinessProfit = input.BusinessProfit,
                Roa = input.Roa,
                Roe = input.Roe,
                Image = input.Image,
                Website = input.Website,
                Hotline = input.Hotline,
                Fanpage = input.Fanpage,
                CreatedBy = username,
            };
            return _ownerRepository.Add(invOwner);
        }

        public int Delete(int id)
        {
            return _ownerRepository.Delete(id, CommonUtils.GetCurrentPartnerId(_httpContext));
        }

        public PagingResult<ViewOwnerDto> FindAll(int pageSize, int pageNumber, string keyword, int? status)
        {
            var invOwenerList = _ownerRepository.FindAll(CommonUtils.GetCurrentPartnerId(_httpContext), pageSize, pageNumber, keyword, status);
            var result = new PagingResult<ViewOwnerDto>();
            var items = new List<ViewOwnerDto>();
            result.TotalItems = invOwenerList.TotalItems;
            foreach (var invOwnerFind in invOwenerList.Items)
            {
                var invOwner = new ViewOwnerDto()
                {
                    Id = invOwnerFind.Id,
                    BusinessCustomerId = invOwnerFind.BusinessCustomerId,
                    PartnerId = invOwnerFind.PartnerId,
                    BusinessProfit = invOwnerFind.BusinessProfit,
                    BusinessTurnover = invOwnerFind.BusinessTurnover,
                    Roa = invOwnerFind.Roa,
                    Roe = invOwnerFind.Roe,
                    Image = invOwnerFind.Image,
                    Website = invOwnerFind.Website,
                    Hotline = invOwnerFind.Hotline,
                    Fanpage = invOwnerFind.Fanpage,
                    Status = invOwnerFind.Status
                };
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(invOwnerFind.BusinessCustomerId);
                if (businessCustomer != null)
                {
                    invOwner.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    invOwner.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(invOwner);
            }
            result.Items = items;
            return result;
        }

        public ViewOwnerDto FindById(int id)
        {
            var invOwnerFind = _ownerRepository.FindById(id);
            if (invOwnerFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chủ đầu tư này "), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
            var result = _mapper.Map<ViewOwnerDto>(invOwnerFind);

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(invOwnerFind.BusinessCustomerId);
            if (businessCustomer != null)
            {
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankDefault(businessCustomer.BusinessCustomerId ?? 0, IsTemp.NO);
                if (businessCustomerBank != null)
                {
                    result.BusinessCustomer.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
                }
            }
            return result;
        }

        public int Update(int Id, UpdateOwnerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var invOwener = new Owner
            {
                Id = Id,
                PartnerId = partnerId,
                BusinessTurnover = input.BusinessTurnover,
                BusinessProfit = input.BusinessProfit,
                Roa = input.Roa,
                Roe = input.Roe,
                Image = input.Image,
                Website = input.Website,
                Hotline = input.Hotline,
                Fanpage = input.Fanpage,
                ModifiedBy = username,
            };
            return _ownerRepository.Update(invOwener);
        }
    }
}
