using EPIC.CoreDomain.Interfaces;
using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Partner;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Utils.ConstantVariables.Identity;
using System.ServiceModel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using Microsoft.EntityFrameworkCore;

namespace EPIC.CoreDomain.Implements
{
    public class PartnerServices : IPartnerServices
    {
        private readonly ILogger<PartnerServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly PartnerRepository _partnerRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly PartnerEFRepository _partnerEFRepository;

        public PartnerServices(ILogger<PartnerServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, EpicSchemaDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _dbContext = dbContext;
            _partnerEFRepository = new PartnerEFRepository(_dbContext, _logger);
        }

        public Partner Add(CreatePartnerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
            {
                throw new FaultException(new FaultReason($"Không phải là EPIC không thể tạo đối tác"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
            var partner = new Partner
            {
                Code = input.Code,
                Name = input.Name,
                ShortName = input.ShortName,
                Address = input.Address,
                Phone = input.Phone,
                Mobile = input.Mobile,
                Email = input.Email,
                TaxCode = input.TaxCode,
                LicenseDate = input.LicenseDate,
                LicenseIssuer = input.LicenseIssuer,
                Capital = input.Capital,
                RepName = input.RepName,
                RepPosition = input.RepPosition,
                TradingAddress = input.TradingAddress,
                Nation = input.Nation,
                DecisionNo = input.DecisionNo,
                DecisionDate = input.DecisionDate,
                NumberModified = input.NumberModified,
                DateModified = input.DateModified,
            };
            return _partnerRepository.Add(partner);
        }

        public int Delete(int id)
        {
            return _partnerRepository.Delete(id);
        }

        public PagingResult<Partner> FindAll(int pageSize, int pageNumber, string keyword)
        {
            return _partnerRepository.FindAll(pageSize, pageNumber, keyword);
        }

        public Partner FindById(int id)
        {
            return _partnerRepository.FindById(id);
        }

        public int Update(int id, UpdatePartnerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                var partnerFind = _partnerRepository.FindById(partnerId.Value);
                if (partnerFind == null)
                {
                    _partnerEFRepository.ThrowException(ErrorCode.CorePartnerNotFound);
                }
            }
            else if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                throw new FaultException(new FaultReason($"Không phải là EPIC không thể tạo đối tác"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
            return _partnerRepository.Update(new Partner
            {
                PartnerId = partnerId ?? id,
                Code = input.Code,
                Name = input.Name,
                ShortName = input.ShortName,
                Address = input.Address,
                Phone = input.Phone,
                Mobile = input.Mobile,
                Email = input.Email,
                TaxCode = input.TaxCode,
                LicenseDate = input.LicenseDate,
                LicenseIssuer = input.LicenseIssuer,
                Capital = input.Capital,
                RepName = input.RepName,
                RepPosition = input.RepPosition,
                TradingAddress = input.TradingAddress,
                Nation = input.Nation,
                DecisionNo = input.DecisionNo,
                DecisionDate = input.DecisionDate,
                NumberModified = input.NumberModified,
                DateModified = input.DateModified,
                ModifiedBy = username
            });
        }

        public List<TradingProviderDto> FindTradingProviderByPartner()
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _partnerRepository.FindTradingProviderByPartner(partnerId);
        }
    }
}
