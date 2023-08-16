using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
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
    public class GarnerProductFileEFRepository : BaseEFRepository<GarnerProductFile>
    {
        public GarnerProductFileEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerProductFile.SEQ}")
        {
        }

        public List<GarnerProductFile> FindAllListByProduct(int productId, int? documentType, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(FindAllListByProduct)}: productId = {productId},  documentType = {documentType}, partnerId = {partnerId}");

            return _dbSet.Where(d => d.ProductId == productId && (documentType == null || d.DocumentType == documentType) && (partnerId == null || d.PartnerId == partnerId) && d.Deleted == YesNo.NO).ToList();
        }

        public GarnerProductFile UpdateProductFile(CreateGarnerProductFileDto input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(UpdateProductFile)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            var fileFind = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (fileFind != null)
            {
                fileFind.DocumentType = input.DocumentType;
                fileFind.Title = input.Title;
                fileFind.Url = input.Url;
                fileFind.TotalValue = input.TotalValue;
                fileFind.Description = input.Description;
                fileFind.ModifiedBy = username;
                fileFind.ModifiedDate = DateTime.Now;
            }
            return fileFind;
        }

        public GarnerProductFile AddProductFile(int productId, CreateGarnerProductFileDto input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(AddProductFile)}:productId = {productId}, input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            return _dbSet.Add(new GarnerProductFile
            {
                Id = (int)NextKey(),
                ProductId = productId,
                PartnerId = partnerId,
                DocumentType = input.DocumentType,
                Title = input.Title,
                Url = input.Url,
                Status = Status.ACTIVE,
                CreatedBy = username,
                TotalValue = input.TotalValue,
                Description = input.Description
            }).Entity;
        }

        public GarnerProductFile DeletedProductFile(int id)
        {
            _logger.LogInformation($"{nameof(DeletedProductFile)}: id = {id}");

            var productFileRemove = _dbSet.FirstOrDefault(p => p.Id == id);
            productFileRemove.Deleted = YesNo.YES;
            return productFileRemove;
        }
    }
}
