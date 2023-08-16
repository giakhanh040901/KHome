using EPIC.CoreEntities.DataEntities;
using EPIC.DataAccess.Base;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;

namespace EPIC.CoreRepositories
{
    public class XptTokenEFRepository : BaseEFRepository<XptToken>
    {
        private readonly EpicSchemaDbContext _epicDbContext;
        public XptTokenEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{XptToken.SEQ}")
        {
            _epicDbContext = dbContext;
        }
        
    }
}
