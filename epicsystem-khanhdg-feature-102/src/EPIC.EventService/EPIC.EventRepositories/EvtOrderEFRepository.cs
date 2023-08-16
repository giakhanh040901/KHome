using EPIC.DataAccess.Base;
using EPIC.EventEntites.Entites;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventRepositories
{
    public class EvtOrderEFRepository : BaseEFRepository<EvtOrder>
    {
        public EvtOrderEFRepository(DbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, $"{DbSchemas.EPIC_EVENT}.{EvtOrder.SEQ}")
        {
        }

        /// <summary>
        /// Sinh mã giao nhận
        /// </summary>
        /// <returns></returns>
        public string FuncVerifyCodeGenerate()
        {
            while (true)
            {

                var numberRandom = DeliveryCodes.EVENT + RandomNumberUtils.RandomNumber(10);
                var checkDeliveryCodeOrder = _dbSet.Where(o => o.DeliveryCode == numberRandom);
                if (!checkDeliveryCodeOrder.Any())
                {
                    return numberRandom;
                }
            }
        }
    }
}
