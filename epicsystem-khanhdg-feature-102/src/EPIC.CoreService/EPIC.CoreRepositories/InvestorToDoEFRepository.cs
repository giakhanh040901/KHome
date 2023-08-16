using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreEntities.DataEntities;
using EPIC.DataAccess.Base;
using EPIC.Utils.ConstantVariables.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class InvestorToDoEFRepository : BaseEFRepository<InvestorToDo>
    {
        public InvestorToDoEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, InvestorToDo.SEQ)
        {
        }
        public List<InvestorToDo> FindAllToDo()
        {
            _logger.LogInformation($"{nameof(FindAllToDo)}: ");

            return _dbSet.ToList();
        }

        public InvestorToDo FindToDoById(int id)
        {
            _logger.LogInformation($"{nameof(FindToDoById)}: id = {id} ");

            return _dbSet.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Kiểm tra xem nhà đầu từ đang có y/c thông báo của loại type đang ở trạng thái khởi tạo hay chưa
        /// Nếu có thì sẽ đè Detail vào
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public InvestorToDo FindToDoByInvestorId(int investorId, int type)
        {
            _logger.LogInformation($"{nameof(FindAllToDo)}: investorId = {investorId}, type = {type} ");

            return _dbSet.FirstOrDefault(e => e.InvestorId == investorId && e.Type == type && e.Status == InvestorTodoStatus.INIT);
        }

        public void Add (InvestorToDo input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            _dbSet.Add(new InvestorToDo
            {
                Id = (int)NextKey(),
                InvestorId = input.InvestorId,
                Detail = input.Detail,
                Status = InvestorTodoStatus.INIT,
                Type = input.Type
            });
        }
    }
}
