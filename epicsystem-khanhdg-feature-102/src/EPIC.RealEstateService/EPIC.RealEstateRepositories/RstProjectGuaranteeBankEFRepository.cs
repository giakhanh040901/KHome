using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectGuaranteeBankEFRepository : BaseEFRepository<RstProjectGuaranteeBank>
    {
        public RstProjectGuaranteeBankEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectGuaranteeBank.SEQ}")
        {
        }

        public void UpdateGuaranteeBanks(int projectId, List<int> guaranteeBanks)
        {
            _logger.LogInformation($"{nameof(RstProjectGuaranteeBankEFRepository)}-> {nameof(UpdateGuaranteeBanks)}: input = {JsonSerializer.Serialize(guaranteeBanks)}");

            //Lấy danh sách theo loại hình sản phẩm
            var listGuaranteeBanks = _dbSet.Where(t => t.ProjectId == projectId);

            // Trường hợp truyền list null thì trả về mảng rỗng
            guaranteeBanks = guaranteeBanks != null ? guaranteeBanks : new List<int>();

            //Xóa đi những loại không được truyền vào
            var typesRemove = listGuaranteeBanks.Where(p => !guaranteeBanks.Contains(p.BankId));
            foreach (var bankItem in typesRemove)
            {
                _dbSet.Remove(bankItem);
            }

            foreach (var bankItem in guaranteeBanks)
            {
                var checkBank = _epicSchemaDbContext.CoreBanks.Where(b => b.BankId == bankItem);
                if (!checkBank.Any())
                {
                    ThrowException(ErrorCode.CoreBankNotFound, bankItem);
                }
                //Nếu là thêm mới thì thêm vào
                if (!listGuaranteeBanks.Select(r => r.BankId).Contains(bankItem))
                {
                    _dbSet.Add(new RstProjectGuaranteeBank
                    {
                        Id = (int)NextKey(),
                        ProjectId = projectId,
                        BankId = bankItem
                    });
                }
            }
        }

        /// <summary>
        /// Danh sách ngân hàng đảm bảo của dự án
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IQueryable<RstProjectGuaranteeBankDto> GetAllProjectGuaranteeBank(int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectGuaranteeBankEFRepository)}-> {nameof(GetAllProjectGuaranteeBank)}: projectId = {projectId}");
            return from guaranteeBank in _dbSet
                   join coreBank in _epicSchemaDbContext.CoreBanks on guaranteeBank.BankId equals coreBank.BankId
                   where guaranteeBank.ProjectId == projectId
                   select new RstProjectGuaranteeBankDto
                   {
                       Id = guaranteeBank.Id,
                       Logo = coreBank.Logo,
                       BankName = coreBank.BankName,
                       FullBankName = coreBank.FullBankName,
                   };
        }
    }
}
