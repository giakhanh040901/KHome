using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerOrderPayment;
using EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EPIC.GarnerRepositories
{
    public class GarnerReceiveContractTemplateEFRepository : BaseEFRepository<GarnerReceiveContractTemp>
    {
        public GarnerReceiveContractTemplateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerReceiveContractTemp.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerReceiveContractTemp Add(GarnerReceiveContractTemp input, string username)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)} => {nameof(Add)}: entity = {JsonSerializer.Serialize(input)}");

            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.CreatedBy = username;
            input.Deleted = YesNo.NO;
            var result = _dbSet.Add(input);
            _dbContext.SaveChanges();

            return result.Entity;
        }

        /// <summary>
        /// Cập nhập mẫu giao nhận hợp đồng 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerReceiveContractTemp Update(GarnerReceiveContractTemp input, string username)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(Update)}: entity = {JsonSerializer.Serialize(input)}");
            var receiveContractTempFind = _dbSet.FirstOrDefault(r => r.Id == input.Id);

            if (receiveContractTempFind != null)
            {
                receiveContractTempFind.Code = input.Code;
                receiveContractTempFind.FileUrl = input.FileUrl;
                receiveContractTempFind.Name = input.Name;
                receiveContractTempFind.ModifiedBy = username;
                receiveContractTempFind.ModifiedDate = DateTime.Now;
            }

            return receiveContractTempFind;
        }

        /// <summary>
        /// Xóa mẫu giao nhận giao nhận hợp đồng
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(Delete)}: id = {id}");
            var receiveContractTempFind = _dbSet.FirstOrDefault(r => r.Id == id);
            if (receiveContractTempFind != null)
            {
                receiveContractTempFind.Deleted = YesNo.YES;
            }
        }

        /// <summary>
        /// Lấy thông tin của mẫu giao nhận hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerReceiveContractTemp FindById(int? id, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");
            var receiveContractTempFind = _dbSet.FirstOrDefault(r => r.Id == id && r.TradingProviderId == tradingProviderId);
            return receiveContractTempFind;
        }

        /// <summary>
        /// Thay đổi status của mẫu giao nhận hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public GarnerReceiveContractTemp ChangeStatus(int? id, string status, string username)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(ChangeStatus)}: id = {id}, status = {status}");
            var receiveContractTempFind = _dbSet.FirstOrDefault(r => r.Id == id);

            if (receiveContractTempFind != null)
            {
                receiveContractTempFind.Status = status;
                receiveContractTempFind.ModifiedBy = username;
                receiveContractTempFind.ModifiedDate = DateTime.Now;
            }

            return receiveContractTempFind;
        }

        /// <summary>
        /// Lấy mẫu giao nhận hợp đồng theo distribution id
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<GarnerReceiveContractTemp> FindByDistributionId(FilterGarnerReceiveContractTemplateDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(FindByDistributionId)}: input = {input}, tradingProviderId = {tradingProviderId}");
            var result = new PagingResult<GarnerReceiveContractTemp>();

            var receiveContractTemps = _dbSet.Where(r => input.DistributionId == r.DistributionId && r.TradingProviderId == tradingProviderId && r.Deleted == YesNo.NO);

            result.TotalItems = receiveContractTemps.Count();
            if (input.PageSize != -1)
            {
                receiveContractTemps = receiveContractTemps.Skip(input.Skip).Take(input.PageSize);
            }

            receiveContractTemps = receiveContractTemps.OrderByDescending(o => o.Id);

            result.Items = receiveContractTemps.ToList();
            return result;
        }

        /// Lấy mẫu giao nhận đầu tiên theo distributionId 
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerReceiveContractTemp GetByDistribution(int distributionId, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GetByDistribution)}: distributionId = {distributionId}, tradingProviderId = {tradingProviderId}");
            var receiveContractTempFind = _dbSet.FirstOrDefault(r => r.DistributionId == distributionId && r.TradingProviderId == tradingProviderId && r.Deleted == YesNo.NO && r.Status == Status.ACTIVE);
            return receiveContractTempFind;
        }

        /// <summary>
        /// Lấy mẫu giao nhận hợp đồng theo distribution id không phân trang
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerReceiveContractTemp> FindAllByDistributionId(int? distributionId, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(FindAllByDistributionId)}: distributionId = {distributionId}, tradingProviderId = {tradingProviderId}");
            var receiveContractTemps = _dbSet.Where(r => r.DistributionId == distributionId && r.TradingProviderId == tradingProviderId && r.Deleted == YesNo.NO).ToList();
            return receiveContractTemps;
        }

        /// <summary>
        /// Lấy thông tin của mẫu giao nhận hợp đồng có trạng thái khoá
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerReceiveContractTemp FindDetectiveById(int? id, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(FindDetectiveById)}: id = {id}, tradingProviderId = {tradingProviderId}");
            var receiveContractTempFind = _dbSet.FirstOrDefault(r => r.Id == id && r.TradingProviderId == tradingProviderId && r.Status == InvestorStatus.DEACTIVE);
            return receiveContractTempFind;
        }
    }
}
