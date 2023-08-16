using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerProductOverviewFileEFRepository : BaseEFRepository<GarnerProductOverviewFile>
    {
        public GarnerProductOverviewFileEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerProductOverviewFile.SEQ}")
        {
        }
        private void UpdateProductOverviewFile(List<GarnerProductOverviewFile> productOverviewFileFind, List<CreateGarnerProductOverviewFileDto> input, int distributionId, int tradingProviderId, string username)
        {
            //Xóa đi những file không được truyền vào
            var productOverviewFileRemove = productOverviewFileFind.Where(p => !input.Select(r => r.Id).Contains(p.Id)).ToList();
            foreach (var productOverviewFileItem in productOverviewFileRemove)
            {
                productOverviewFileItem.Deleted = YesNo.YES;
            }

            foreach (var item in input)
            {
                //Nếu là thêm mới thì thêm vào
                if (item.Id == TrueOrFalseNum.FALSE)
                {
                    _dbSet.Add(new GarnerProductOverviewFile
                    {
                        Id = (int)NextKey(),
                        DistributionId = distributionId,
                        TradingProviderId = tradingProviderId,
                        DocumentType = item.DocumentType,
                        Title = item.Title,
                        Url = item.Url,
                        Status = Utils.Status.ACTIVE,
                        CreatedBy = username,
                        EffectiveDate = item.EffectiveDate,
                        ExpirationDate = item.ExpirationDate,
                        Description = item.Description
                    });
                }
                else
                {
                    var fileFind = _dbSet.FirstOrDefault(p => p.Id == item.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
                    if (fileFind != null)
                    {
                        fileFind.DocumentType = item.DocumentType;
                        fileFind.Title = item.Title;
                        fileFind.Url = item.Url;
                        fileFind.ExpirationDate = item.ExpirationDate;
                        fileFind.EffectiveDate = item.EffectiveDate;
                        fileFind.ModifiedBy = username;
                        fileFind.ModifiedDate = DateTime.Now;
                        fileFind.Description = item.Description;
                    }
                }
            }
        }

        /// <summary>
        /// Tìm kiếm file tổng quan
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerProductOverviewFile> FindAllListByDistribution(int distributionId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerProductOverviewFileEFRepository)}->{nameof(FindAllListByDistribution)}: distributionId = {distributionId}, tradingProviderId = {tradingProviderId}");

            return _dbSet.Where(d => d.DistributionId == distributionId && (tradingProviderId == null || d.TradingProviderId == tradingProviderId)
            && d.Deleted == YesNo.NO 
            && (d.DocumentType == DocumentTypes.THONG_TIN_SAN_PHAM || d.DocumentType == DocumentTypes.HO_SO_PHAP_LY || d.DocumentType == DocumentTypes.CHINH_SACH)).ToList();
        }

        /// <summary>
        /// Tìm kiếm file chính sách
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerProductOverviewFile> FindAllPolicyFileByDistribution(int distributionId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerProductOverviewFileEFRepository)}->{nameof(FindAllPolicyFileByDistribution)}: distributionId = {distributionId}, tradingProviderId = {tradingProviderId}");

            return _dbSet.Where(d => d.DistributionId == distributionId && (tradingProviderId == null || d.TradingProviderId == tradingProviderId)
            && d.Deleted == YesNo.NO && d.DocumentType == DocumentTypes.FILE_CHINH_SACH).ToList();
        }

        /// <summary>
        /// File tổng quan
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        public void UpdateProductOverviewFile(int distributionId, List<CreateGarnerProductOverviewFileDto> input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(GarnerProductOverviewFileEFRepository)}->{nameof(UpdateProductOverviewFile)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            //Lấy danh sách tổng quan file đã có trong db
            var productOverviewFileFind = FindAllListByDistribution(distributionId, tradingProviderId);
            //Thêm (nếu chưa có), Update(nếu đã có) tổng quan file
            UpdateProductOverviewFile(productOverviewFileFind, input, distributionId, tradingProviderId, username);
        }

        /// <summary>
        /// Thêm File chính sách bán theo kỳ hạn
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        public GarnerProductOverviewFile AddDistributionPolicyFile(int distributionId, CreateGarnerProductOverviewFileDto input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(GarnerProductOverviewFileEFRepository)}->{nameof(AddDistributionPolicyFile)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var insertDistributionPolicyFile = new GarnerProductOverviewFile()
            {
                Id = (int)NextKey(),
                DistributionId = distributionId,
                TradingProviderId = tradingProviderId,
                Title = input.Title,
                Url = input.Url,
                DocumentType = DocumentTypes.FILE_CHINH_SACH,
                Status = Status.ACTIVE,
                Description = input.Description,
                ExpirationDate = input.ExpirationDate,
                EffectiveDate = input.EffectiveDate,
                CreatedDate = DateTime.Now,
                CreatedBy = username
            };
            return _dbSet.Add(insertDistributionPolicyFile).Entity;
        }

        /// <summary>
        /// Thêm File chính sách bán theo kỳ hạn
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        public GarnerProductOverviewFile UpdateDistributionPolicyFile(int distributionId, CreateGarnerProductOverviewFileDto input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(GarnerProductOverviewFileEFRepository)}->{nameof(UpdateDistributionPolicyFile)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var distributionPolicyFile = _dbSet.FirstOrDefault(e => e.Id == input.Id && e.DistributionId == distributionId && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);

            distributionPolicyFile.Title = input.Title;
            distributionPolicyFile.Url = input.Url;
            distributionPolicyFile.DocumentType = DocumentTypes.FILE_CHINH_SACH;
            distributionPolicyFile.Description = input.Description;
            distributionPolicyFile.ExpirationDate = input.ExpirationDate;
            distributionPolicyFile.EffectiveDate = input.EffectiveDate;
            distributionPolicyFile.ModifiedDate = DateTime.Now;
            distributionPolicyFile.ModifiedBy = username;

            return distributionPolicyFile;
        }
    }
}
