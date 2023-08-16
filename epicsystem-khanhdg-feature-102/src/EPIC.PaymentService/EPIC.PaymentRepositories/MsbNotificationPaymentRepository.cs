using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.PaymentRepositories
{
    public class MsbNotificationPaymentRepository : BaseEFRepository<MsbNotificationPayment>
    {
        public MsbNotificationPaymentRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{MsbNotificationPayment.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MsbNotificationPayment Add(MsbNotificationPayment entity)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(entity)}");
            return _dbSet.Add(new MsbNotificationPayment
            {
                Id = (int)NextKey(),
                TransId = entity.TransId,
                TransDate = entity.TransDate,
                TId = entity.TId,
                MId = entity.MId,
                Note = entity.Note,
                SenderName = entity.SenderName,
                ReceiveName = entity.ReceiveName,
                SenderAccount = entity.SenderAccount,
                ReceiveAccount = entity.ReceiveAccount,
                ReceiveBank = entity.ReceiveBank,
                Amount = entity.Amount,
                Fee = entity.Fee,
                SecureHash = entity.SecureHash,
                NapasTransId = entity.NapasTransId,
                Rrn = entity.Rrn,
                Status = entity.Status,
                Ip = entity.Ip,
                HandleStatus = entity.HandleStatus,
                CreatedDate = DateTime.Now,
            }).Entity;
        }

        public void Update(MsbNotificationPayment entity)
        {
            _dbSet.Update(entity);
        }

        public MsbNotificationPayment FindByTranId (string tranId)
        {
            var notificationPayment = _dbSet.FirstOrDefault(e => e.TransId == tranId);
            return notificationPayment;
        }
    }
}
