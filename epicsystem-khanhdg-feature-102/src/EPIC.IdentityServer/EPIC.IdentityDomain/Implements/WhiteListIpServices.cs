using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.WhiteListIp;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace EPIC.IdentityDomain.Implements
{
    public class WhiteListIpServices : IWhiteListIpServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly WhiteListIpEFRepository _whiteListIpEFRepository;
        private readonly WhiteListIpDetailEFRepository _whiteListIpDetailEFRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WhiteListIpServices(EpicSchemaDbContext dbContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _whiteListIpEFRepository = new WhiteListIpEFRepository(dbContext);
            _whiteListIpDetailEFRepository = new WhiteListIpDetailEFRepository(dbContext);
        }

        public int? GetCurrentTradingProvider()
        {
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
            }
            return tradingProviderId;
        }

        /// <summary>
        /// Thêm mới WhiteListIP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void CreateWhiteListIP(CreateWhiteListDto input)
        {
            int? tradingProviderId = GetCurrentTradingProvider();
            if (tradingProviderId != null)
            {
                var whiteListIp = _whiteListIpEFRepository.Entity.FirstOrDefault(e => e.Type == input.Type && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);
                if (whiteListIp != null)
                {
                    throw new FaultException(new FaultReason($"Đã cấu hình dải địa chỉ ip cho chức năng này"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            } 
            else
            {
                var whiteListIp = _whiteListIpEFRepository.Entity.FirstOrDefault(e => e.Type == input.Type && e.TradingProviderId == input.TradingProviderId && e.Deleted == YesNo.NO);
                if (whiteListIp != null)
                {
                    throw new FaultException(new FaultReason($"Đã cấu hình dải địa chỉ ip cho chức năng này"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }
            var whiteListIpInsert = _whiteListIpEFRepository.Entity.Add(new WhiteListIp
            {
                Id = (int)_whiteListIpEFRepository.NextKey(),
                CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                Name = input.Name,
                Type = input.Type,
                TradingProviderId = tradingProviderId ?? input.TradingProviderId,
            });
            foreach (var whiteIp in input.WhiteListIPDetails)
            {
                string[] childStartIpArr = whiteIp.IpAddressStart.Split('.');
                string[] childEndIpArr = whiteIp.IpAddressEnd.Split('.');
                if (childStartIpArr.Length == 4 && childEndIpArr.Length == 4)
                {
                    for (int i = childStartIpArr.Length - 1; i >= 0; i--)
                    {
                        int number;
                        bool childStartIpCheck = int.TryParse(childStartIpArr[i], out number);
                        bool childEndIpCheck = int.TryParse(childEndIpArr[i], out number);
                        if (!(childStartIpCheck && childEndIpCheck))
                        {
                            throw new FaultException(new FaultReason($"Các cụm trong địa chỉ Ip phải là một số hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }
                    }
                }
                else
                {
                    throw new FaultException(new FaultReason($"Địa chỉ Ip chưa đúng định dạng. VD: '127.0.0.1'"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }

                _whiteListIpDetailEFRepository.Entity.Add(new WhiteListIpDetail
                {
                    Id = (int)_whiteListIpDetailEFRepository.NextKey(),
                    WhiteListIpId = whiteListIpInsert.Entity.Id,
                    IpAddressStart = whiteIp.IpAddressStart,
                    IpAddressEnd = whiteIp.IpAddressEnd,
                    TradingProviderId = tradingProviderId ?? input.TradingProviderId,
                });
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xem danh sách WhiteListIP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<WhiteListIpDto> FindAllWhiteListIp(FilterWhiteListIpDto input)
        {
            var result = new PagingResult<WhiteListIpDto>();
            var find = _whiteListIpEFRepository.FindAll(input, GetCurrentTradingProvider());
            result.Items = _mapper.Map<IEnumerable<WhiteListIpDto>>(find.Items);
            foreach (var item in result.Items) // lấy danh sách Ip con
            {
                var childList = _whiteListIpDetailEFRepository.Entity.AsQueryable()
                    .Where(c => c.WhiteListIpId == item.Id && c.Deleted == YesNo.NO).ToList();
                item.WhiteListIPDetails = childList;
            }
            result.TotalItems = find.TotalItems;
            return result;
        }

        /// <summary>
        /// Xem chi tiết
        /// </summary>
        /// <param name="whiteListIpId"></param>
        /// <returns></returns>
        public WhiteListIpDto GetById(int whiteListIpId)
        {
            int? tradingProviderId = GetCurrentTradingProvider();
            var whiteListIpFind = _whiteListIpEFRepository.Entity.FirstOrDefault(r => r.Id == whiteListIpId && (tradingProviderId == null || r.TradingProviderId == tradingProviderId) && r.Deleted == YesNo.NO);
            if (whiteListIpFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin white list ip"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var childList = _whiteListIpDetailEFRepository.Entity.AsQueryable()
                .Where(c => c.WhiteListIpId == whiteListIpId && c.Deleted == YesNo.NO).ToList();
            return new WhiteListIpDto()
            {
                Id = whiteListIpId,
                TradingProviderId = tradingProviderId ?? whiteListIpFind.TradingProviderId,
                Name = whiteListIpFind.Name,
                Type = whiteListIpFind.Type,
                WhiteListIPDetails = childList,
            };
        }

        /// <summary>
        /// Cập nhật WhiteListIP, tìm trong ds con nếu ko tìm thấy thì xóa, nếu thiếu thì thêm
        /// </summary>
        /// <param name="whiteListIpId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public void UpdateWhiteListIP(int whiteListIpId, UpdateWhiteListIpDto input)
        {
            int? tradingProviderId = GetCurrentTradingProvider();
            var whiteListIpQuery = _whiteListIpEFRepository.Entity.FirstOrDefault(r => r.Id == whiteListIpId && (tradingProviderId == null || r.TradingProviderId == tradingProviderId) && r.Deleted == YesNo.NO);
            if (whiteListIpQuery == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin white list Ip"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            // Sửa WhiteListIp Cha
            whiteListIpQuery.Name = input.Name;
            whiteListIpQuery.Type = input.Type;
            // Sửa WhiteListIp Con
            // Xóa ds id con ko nằm trong input
            var childIpList = input.whiteListIPDetails.Select(o => o.Id).ToList();
            var removeList = _whiteListIpDetailEFRepository.Entity.AsQueryable()
                .Where(r => r.WhiteListIpId == whiteListIpId && !childIpList.Contains(r.Id)).ToList();
            _whiteListIpDetailEFRepository.Entity.RemoveRange(removeList);

            var whiteListchildIp = _whiteListIpDetailEFRepository.Entity.AsQueryable()
                .Where(c => c.WhiteListIpId == whiteListIpId && childIpList.Contains(c.Id)).ToList();
            foreach (var item in input.whiteListIPDetails)
            {
                if (whiteListchildIp.Count == 0)
                {
                    var whiteChildIpInsert = _whiteListIpDetailEFRepository.Entity.Add(new WhiteListIpDetail
                    {
                        Id = (int)_whiteListIpDetailEFRepository.NextKey(),
                        WhiteListIpId = whiteListIpId,
                        IpAddressStart = item.IpAddressStart,
                        IpAddressEnd = item.IpAddressEnd,
                        TradingProviderId = tradingProviderId,
                    });
                }
                else
                {
                    foreach (var child in whiteListchildIp)
                    {
                        if (item.Id != child.Id) // nếu chưa có thì thêm
                        {
                            var whiteChildIpInsert = _whiteListIpDetailEFRepository.Entity.Add(new WhiteListIpDetail
                            {
                                Id = (int)_whiteListIpDetailEFRepository.NextKey(),
                                WhiteListIpId = whiteListIpId,
                                IpAddressStart = item.IpAddressStart,
                                IpAddressEnd = item.IpAddressEnd,
                                TradingProviderId = tradingProviderId,
                            });
                        }
                        else // nếu đã có thì update
                        {
                            var whiteChildIpUpdate = _whiteListIpDetailEFRepository.Entity.FirstOrDefault(c => c.Id == item.Id);
                            whiteChildIpUpdate.IpAddressStart = item.IpAddressStart;
                            whiteChildIpUpdate.IpAddressEnd = item.IpAddressEnd;
                        }
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xoá WhiteListIP. Xóa cha, xóa cả danh sách con
        /// </summary>
        /// <param name="whiteListIpId"></param>
        /// <returns></returns>
        public void DeleteWhiteListIP(int whiteListIpId)
        {
            int? tradingProviderId = GetCurrentTradingProvider();
            var whiteListIpFind = _whiteListIpEFRepository.Entity.FirstOrDefault(r => r.Id == whiteListIpId && (tradingProviderId == null || r.TradingProviderId == tradingProviderId) && r.Deleted == YesNo.NO);
            if (whiteListIpFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin white list ip"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            whiteListIpFind.Deleted = YesNo.YES;
            var whiteListIpDetail = _whiteListIpDetailEFRepository.Entity.AsQueryable()
                .Where(c => c.WhiteListIpId == whiteListIpId && c.Deleted == YesNo.NO).ToList();
            foreach (var item in whiteListIpDetail)
            {
                item.Deleted = YesNo.YES;
            }
            _dbContext.SaveChanges();
        }
    }
}
