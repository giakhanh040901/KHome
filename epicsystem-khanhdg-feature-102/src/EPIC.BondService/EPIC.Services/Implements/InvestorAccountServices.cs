using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.BondDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace EPIC.BondDomain.Implements
{
    public class InvestorAccountServices : IInvestorAccountService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<InvestorAccountServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly UserRepository _userRepository;
        private readonly UserEFRepository _userEFRepository;
        private readonly SaleRepository _saleRepository;
        private readonly IHttpContextAccessor _httpContext;
        public InvestorAccountServices(EpicSchemaDbContext dbContext, ILogger<InvestorAccountServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _userRepository = new UserRepository(_connectionString, _logger);
            _userEFRepository = new UserEFRepository(_dbContext, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }
        public PagingResult<UserIsInvestorDto> GetByType(FindBondInvestorAccountDto dto)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            var validUserTypes = new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC, UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER };
            if (!validUserTypes.Contains(userType))
            {
                throw new FaultException(new FaultReason($"Root không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            int? tradingProviderId = null;
            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(userType))
            {
                try
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                catch (Exception)
                {
                    throw new FaultException(new FaultReason($"Root không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }

            //var result = _userRepository.GetInvestorAccount(dto, tradingProviderId, null);
            dto.TradingProviderId = tradingProviderId;
            var query = _userEFRepository.GetInvestorAccount(dto);

            var result = new PagingResult<UserIsInvestorDto>();
            result.TotalItems = query.Count();
            query = query.OrderByDescending(e => e.UserId);
            query = query.OrderDynamic(dto.Sort);
            if (dto.PageSize != -1)
            {
                query = query.Skip(dto.Skip).Take(dto.PageSize);
            }

            var resultItems = new List<UserIsInvestorDto>();

            foreach (var user in query)
            {
                var resultItem = new UserIsInvestorDto();
                resultItem.UserId = user.UserId;
                resultItem.UserName = user.UserName;
                resultItem.DisplayName = user.DisplayName;
                resultItem.Sex = user.Sex;
                resultItem.Status = user.Status;
                resultItem.Email = user.Email;
                resultItem.Name = user.Name;
                resultItem.Phone = user.Phone;
                resultItem.InvestorId = user.InvestorId;
                resultItem.CifCode = user.CifCode;
                resultItem.IdNo = user.IdNo;
                resultItem.ReferralCodeSelf = user.ReferralCodeSelf;
                resultItem.Source = user.Source;
                var checkSaleStatus = _saleRepository.AppCheckSaler(user.InvestorId);
                resultItem.CheckSaleStatus = checkSaleStatus?.Status;
                resultItem.Email = StringUtils.HideEmail(user.Email ?? "");
                resultItem.Phone = StringUtils.HidePhone(user.Phone ?? "");
                resultItem.UserName = StringUtils.HidePhone(user.UserName ?? "");
                resultItems.Add(resultItem);
            }

            result.Items = resultItems;
            return result;
        }
    }
}
