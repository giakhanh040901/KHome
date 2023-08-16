using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.GuaranteeAsset;
using EPIC.Entities.Dto.GuaranteeFile;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.BondDomain.Implements
{
    public class BondGuaranteeAssetService : IBondGuaranteeAssetService
    {
        private ILogger<BondGuaranteeAssetService> _logger;
        private IConfiguration _configuration;
        private string _connectionString;
        private BondGuaranteeAssetRepository _guaranteeAssetRepository;
        private IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BondGuaranteeAssetService(ILogger<BondGuaranteeAssetService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _guaranteeAssetRepository = new BondGuaranteeAssetRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public void Add(CreateGuaranteeAssetDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            using (TransactionScope scope = new TransactionScope())
            {
                var asset = _guaranteeAssetRepository.Add(new BondGuaranteeAsset
                {
                    BondId = input.ProductBondId,
                    Code = input.Code,
                    AssetValue = input.AssetValue,
                    DescriptionAsset = input.DescriptionAsset,
                    CreatedBy = username
                });

                foreach (var file in input.GuaranteeFiles)
                {
                    _guaranteeAssetRepository.AddFile(new BondGuaranteeFile
                    {
                        GuaranteeAssetId = asset.Id,
                        Title = file.Title,
                        FileUrl = file.FileUrl,
                        CreatedBy = username
                    });
                }
                scope.Complete();
            }
            _guaranteeAssetRepository.CloseConnection();
        }

        public int Delete(int id)
        {
            return _guaranteeAssetRepository.Delete(id);
        }

        public int DeleteFile(int id)
        {
            return _guaranteeAssetRepository.DeleteFile(id);
        }

        public GuaranteeAssetDto FindById(int id)
        {
            var query = _guaranteeAssetRepository.FindById(id);

            var asset = _mapper.Map<GuaranteeAssetDto>(query);
            var listFileRepo = _guaranteeAssetRepository.FindAllByIdFile(query.Id);
            if (listFileRepo != null)
            {
                var listFile = _mapper.Map<List<GuaranteeFileDto>>(listFileRepo);
                asset.GuaranteeFiles = listFile;
            }
            return asset;
        }

        public GuaranteeFileDto FindByIdFile(int id)
        {
            return _guaranteeAssetRepository.FindGuaranteeFileById(id);
        }

        public void Update(UpdateGuaranteeAssetDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            using (TransactionScope scope = new TransactionScope())
            {
                _guaranteeAssetRepository.Update(new BondGuaranteeAsset
                {
                    Id = input.GuaranteeAssetId,
                    BondId = input.ProductBondId,
                    Code = input.Code,
                    AssetValue = input.AssetValue,
                    DescriptionAsset = input.DescriptionAsset,
                    ModifiedBy = username
                });

                // xoa file 
                var listFileInput = input.GuaranteeFiles.ToList();
                var listFileDb = _guaranteeAssetRepository.FindAllGuaranteeFile(input.GuaranteeAssetId, -1, 0, "").Items.ToList();
                foreach (var fileDb in listFileDb)
                {
                    var checkFileId = listFileInput.FirstOrDefault(f => f.GuaranteeFileId == fileDb.Id);
                    if (checkFileId == null)
                    {
                        _guaranteeAssetRepository.DeleteFile(fileDb.Id);
                    };
                }

                foreach (var file in input.GuaranteeFiles)
                {
                    // Thêm mới file
                    if (file.GuaranteeFileId == 0)
                    {
                        _guaranteeAssetRepository.AddFile(new BondGuaranteeFile
                        {
                            GuaranteeAssetId = input.GuaranteeAssetId,
                            Title = file.Title,
                            FileUrl = file.FileUrl,
                            CreatedBy = username
                        });
                    }
                    //Cập nhật file
                    else
                    {
                        _guaranteeAssetRepository.UpdateFile(new BondGuaranteeFile
                        {
                            GuaranteeAssetId = input.GuaranteeAssetId,
                            Id = file.GuaranteeFileId,
                            Title = file.Title,
                            FileUrl = file.FileUrl
                        });
                    }
                }
                scope.Complete();
            }
            _guaranteeAssetRepository.CloseConnection();
        }

        public int UpdateFile(int id, UpdateGuaranteeFileDto input)
        {
            return _guaranteeAssetRepository.UpdateFile(new BondGuaranteeFile
            {
                Id = id,
                GuaranteeAssetId = input.GuaranteeAssetId,
                Title = input.Title,
                FileUrl = input.FileUrl
            });
        }

        public PagingResult<GuaranteeAssetDto> FindAll(int productBondId, int pageSize, int pageNumber, string keyword)
        {
            var query = _guaranteeAssetRepository.FindAll(productBondId, pageSize, pageNumber, keyword);

            var result = new PagingResult<GuaranteeAssetDto>();
            var items = new List<GuaranteeAssetDto>();

            result.TotalItems = query.TotalItems;
            foreach (var assetItem in query.Items)
            {
                var asset = _mapper.Map<GuaranteeAssetDto>(assetItem);
                var listFileRepo = _guaranteeAssetRepository.FindAllByIdFile(asset.GuaranteeAssetId);

                var listFile = _mapper.Map<List<GuaranteeFileDto>>(listFileRepo);
                asset.GuaranteeFiles = listFile;

                items.Add(asset);
            }
            result.Items = items;
            return result;
        }
    }
}
