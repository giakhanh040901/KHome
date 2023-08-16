using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.BondEntities.Dto.BondSecondaryOverviewFile;
using EPIC.BondEntities.Dto.BondSecondaryOverviewOrg;
using EPIC.BondRepositories;
using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace EPIC.BondDomain.Implements
{
    public class BondInfoOverviewService : IBondInfoOverviewService
    {
        private readonly ILogger<BondInfoOverviewService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _connectionString;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly BondInfoOverviewRepository _bondInfoOverviewRepository;
        private readonly IMapper _mapper;

        public BondInfoOverviewService(
            ILogger<BondInfoOverviewService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContext = httpContext;
            _connectionString = databaseOptions.ConnectionString;
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _bondInfoOverviewRepository = new BondInfoOverviewRepository(_connectionString, _logger);
            _mapper = mapper;
        }

        public void UpdateOverviewContent(UpdateBondInfoOverviewContentDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            using (TransactionScope scope = new TransactionScope())
            {
                _productBondSecondaryRepository.UpdateOverviewContent(input, tradingProviderId);

                #region Xử lý các file tổng quan
                // Lấy danh sách file truyền vào
                var listFileInput = input.BondSecondaryOverviewFiles.ToList();

                //Lấy danh sách file từ Db
                var listFileDb = _bondInfoOverviewRepository.FindAllListFile(input.SecondaryId);
                foreach (var fileDb in listFileDb)
                {
                    //Nếu file 
                    var checkFileId = listFileInput.FirstOrDefault(f => f.Id == fileDb.Id);
                    if (checkFileId == null)
                    {
                        _bondInfoOverviewRepository.DeleteFile(fileDb.Id);
                    };
                }

                foreach (var file in input.BondSecondaryOverviewFiles)
                {
                    // Thêm mới file
                    if (file.Id == 0)
                    {
                        _bondInfoOverviewRepository.AddFile(new BondInfoOverviewFile
                        {
                            SecondaryId = input.SecondaryId,
                            Title = file.Title,
                            Url = file.Url,
                            CreatedBy = username
                        }, tradingProviderId);
                    }
                    //Cập nhật file
                    else
                    {
                        _bondInfoOverviewRepository.UpdateFile(new BondInfoOverviewFile
                        {
                            Id = file.Id,
                            SecondaryId = input.SecondaryId,
                            Title = file.Title,
                            Url = file.Url
                        }, tradingProviderId);
                    }
                }
                #endregion

                #region Xử lý các thông tin tổ chức liên quan
                // Lấy danh sách file truyền vào
                var listOrgInput = input.BondSecondaryOverviewOrgs.ToList();

                //Lấy danh sách file từ Db
                var listOrgDb = _bondInfoOverviewRepository.FindAllListOrg(input.SecondaryId);
                foreach (var orgItem in listOrgDb)
                {
                    //Nếu file 
                    var checkOrgId = listOrgInput.FirstOrDefault(f => f.Id == orgItem.Id);
                    if (checkOrgId == null)
                    {
                        _bondInfoOverviewRepository.DeleteOrg(orgItem.Id);
                    };
                }

                foreach (var item in input.BondSecondaryOverviewOrgs)
                {
                    // Thêm mới file
                    if (item.Id == 0)
                    {
                        _bondInfoOverviewRepository.AddOrg(new BondInfoOverviewOrg
                        {
                            SecondaryId = input.SecondaryId,
                            Name = item.Name,
                            OrgCode = item.OrgCode,
                            Icon = item.Icon,
                            Url = item.Url,
                            CreatedBy = username
                        }, tradingProviderId);
                    }
                    //Cập nhật Org
                    else
                    {
                        _bondInfoOverviewRepository.UpdateOrg(new BondInfoOverviewOrg
                        {
                            Id = item.Id,
                            SecondaryId = input.SecondaryId,
                            Name = item.Name,
                            OrgCode = item.OrgCode,
                            Icon = item.Icon,
                            Url = item.Url
                        }, tradingProviderId);
                    }
                }
                #endregion
                scope.Complete();
            }
            _bondInfoOverviewRepository.CloseConnection();
        }

        public BondSecondaryOverviewDto FindOverViewById(int bondSecondaryid)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUsername(_httpContext);
            if (userType != UserTypes.EPIC || userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            }
            var first = _productBondSecondaryRepository.FindSecondaryById(bondSecondaryid, tradingProviderId);

            var result = _mapper.Map<BondSecondaryOverviewDto>(first);

            var listFileDb = _bondInfoOverviewRepository.FindAllListFile(bondSecondaryid, first.TradingProviderId);

            result.BondSecondaryOverviewFiles = _mapper.Map<List<ViewBondSecondaryOverViewFileDto>>(listFileDb);

            var overviewOrgsFind = _bondInfoOverviewRepository.FindAllListOrg(bondSecondaryid, first.TradingProviderId);
            result.BondSecondaryOverviewOrgs = _mapper.Map<List<ViewBondSecondaryOverViewOrgDto>>(overviewOrgsFind);

            return result;
        }
    }
}
